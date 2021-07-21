using System;
using System.Collections.Generic;
using System.Linq;/*
using System.Net;
using System.Net.Http; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;*/
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using coreData.Constants;
using coreErp.Models.Loan;
using coreERP.Models.Loan;
using coreErpApi.Controllers.Models;
using coreErpApi.Models;
using coreErpApi.Models.Loan;


namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class LoanController : ApiController
    {
        coreLoansEntities le;
        private IIDGenerator idGen;
        private IScheduleManager schMgr;
        private core_dbEntities ent;



        public LoanController()
        {
            ent = new core_dbEntities();
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;

            idGen = new IDGenerator();
            schMgr = new ScheduleManager();
        }

        // GET: api/Category
        public IEnumerable<Models.LookupEntry> Get(long id)
        {
            List<Models.LookupEntry> list = new List<Models.LookupEntry>();

            var loans = le.loans
                .Where(p => p.clientID == id)
                .AsNoTracking()
                .ToList();
            foreach (var item in loans)
            {
                var fullName = item.loanNo + " (" + item.applicationDate.ToString("dd-MMM-yyyy") + ")";
                list.Add(new Models.LookupEntry
                {
                    Description = fullName,
                    ID = item.loanID
                });
            }

            return list;
        }


        //By Manager ---- 03_Mar_2016
        [HttpGet]
        public IEnumerable<LoanModel> GetClientRunningDeposits(int id)
        {
            var data = le.loans
                .Where(p => p.clientID == id && p.balance > 0)
                .Select(p => new LoanModel
                {
                    loanId = p.loanID,
                    loanNumber = p.loanNo
                })
                .ToList();
            return data;
        }


        //By Manager ---- 15_Mar_2016
        [HttpGet]
        public LoanDetailViewModel GetLoanAccount()
        {
            LoanDetailViewModel data = new LoanDetailViewModel
            {
                lnGurantors = new List<LoanGurantorModel>(),
                lnCollateral = new List<LoanCollateralModel>(),
                loanFinancials = new List<loanFinancial>(),
                lnDocuments = new List<LoanDocumentModel>()
            };
            return data;
        }

        //By Manager ---- 15_Mar_2016
        [HttpGet]
        public LoanDetailViewModel GetLoanAccount(int id)
        {
            var data = le.loans.Where(p => p.loanID == id)
                .Select(p => new LoanDetailViewModel
                {
                    loanID = p.loanID,
                    loanNo = p.loanNo,
                    clientName = p.client.surName+", "+p.client.otherNames+" - "+p.client.accountNumber,
                    amountRequested = p.amountRequested,
                    loanTypeID = p.loanTypeID,
                    loanTenure = p.loanTenure,
                    applicationDate = p.applicationDate,
                    repaymentModeID = p.repaymentModeID,
                    interestRate = p.interestRate,
                    interestTypeID = p.interestTypeID,
                    staffID = p.staffID,
                    agentID =  p.agentID,
                    lnGurantors = p.loanGurantors.Select(q => new LoanGurantorModel
                    {
                        loanGurantorId = q.loanGurantorID,
                        loanID = q.loanID,
                        idTypeId = q.idNo.idNoTypeID,
                        IdNumber = q.idNo.idNo1,
                        phoneTypeId = q.phone != null?q.phone.phoneTypeID.Value:0,
                        phoneNumer = q.phone != null?q.phone.phoneNo:"",
                        addressLine = q.address != null?q.address.addressLine1:"",
                        city = q.address != null?q.address.cityTown:"",
                        emailAddress = q.email != null? q.email.emailAddress:"",
                        //gurantorPhotos = new List<GuarantorPhotoViewModel>().Add(new GuarantorPhotoViewModel
                        //{
                        //    fileName = q.image.description
                        //})
                    })

                }).FirstOrDefault();

            return data;
        }

        //By Manager ---- 15_Mar_2016
        [HttpPost]
        public loan Post(LoanDetailViewModel input)
        {
            if (input == null) return null;
            //validateInput(input);


            var user = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var staff = le.staffs.FirstOrDefault(p => p.userName == user);
            if (staff == null) throw new ApplicationException("Access denied, only staff are allowed");

            loan toBesaved = new loan();
            populateLoan(toBesaved, input);
            le.loans.Add(toBesaved);

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServe);
            }
            return toBesaved;
        }

        //By Manager ---- 15_Mar_2016
        [HttpPost]
        public loan Put(LoanDetailViewModel input)
        {
            if (input == null) return null;
            //validateInput(input);

            var user = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var staff = le.staffs.FirstOrDefault(p => p.userName == user);
            if (staff == null) throw new ApplicationException("Access denied, only staff are allowed");

            loan toBeUpdated = le.loans.FirstOrDefault(p => p.loanID == input.loanID);
            populateUpdatedLoan(toBeUpdated, input);

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServe);
            }
            return toBeUpdated;
        }

        //Populate Loan
        private void populateLoan(loan toBeSaved, LoanDetailViewModel input)
        {
            toBeSaved.creditManagerNotes = input.creditOfficerNotes;
            toBeSaved.insuranceAmount = input.insuranceAmount;
            toBeSaved.creditManagerNotes = input.creditManagerNotes;
            toBeSaved.loanTypeID = input.loanTypeID;
            toBeSaved.amountRequested = input.amountRequested;
            toBeSaved.applicationDate = input.applicationDate;
            toBeSaved.clientID = input.clientID;
            toBeSaved.gracePeriod = 0;
            toBeSaved.interestRate = input.interestRate;
            toBeSaved.interestTypeID = input.interestTypeID;
            toBeSaved.enteredBy = toBeSaved.creator;
            toBeSaved.applicationFee = 0;
            toBeSaved.applicationFeeBalance = 0;
            toBeSaved.commission = 0;
            toBeSaved.commissionBalance = 0;
            toBeSaved.securityDeposit = 0;
            toBeSaved.processingFee = input.processingFee;
            toBeSaved.securityDeposit = 0;
            toBeSaved.repaymentModeID = input.repaymentModeID;
            toBeSaved.staffID = input.staffID;
            toBeSaved.agentID = input.agentID;

            if (toBeSaved.loanStatusID <= 0) toBeSaved.loanStatusID = 1;
            toBeSaved.loanTenure = input.loanTenure;
            toBeSaved.repaymentModeID = toBeSaved.repaymentModeID;
            toBeSaved.tenureTypeID = 1;
            if (toBeSaved.loanID < 1)
            {
                toBeSaved.creation_date = DateTime.Now;
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.approvalComments = "";
                toBeSaved.creditOfficerNotes = "";
                saving loanRepaymentAccount = new saving();
                populateSavingsFields(loanRepaymentAccount, input);
                toBeSaved.saving = loanRepaymentAccount;

                var clnt = le.clients.Include(p => p.loans).Include(p => p.branch).FirstOrDefault(p => p.clientID == input.clientID);
                var loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == input.loanTypeID);
                if (ent.comp_prof.FirstOrDefault().traditionalLoanNo && clnt != null)
                {
                    toBeSaved.loanNo = idGen.NewLoanNumber(clnt.branchID.Value,
                         input.clientID, toBeSaved.loanID,
                        loanType.loanTypeName.Substring(0, 1).ToUpper());
                }
                else
                {
                    toBeSaved.loanNo = idGen.NewLoanNumber(clnt.branchID.Value,
                        input.clientID, toBeSaved.loanID,
                        loanType.loanTypeName.Substring(0, 1).ToUpper());
                }
            }


            if (toBeSaved.loanStatusID == 0 || toBeSaved.loanStatusID == 1)
            {
                var sch = le.repaymentSchedules.Where(p => p.loanID == toBeSaved.loanID).ToList();
                for (int i = sch.Count - 1; i >= 0; i--)
                {
                    le.repaymentSchedules.Remove(sch[i]);
                }

                //Create Repayment Schedule
                List<coreLogic.repaymentSchedule> sched;
                using (core_dbEntities ctx = new core_dbEntities())
                {
                    var comp = ctx.comp_prof.FirstOrDefault();
                    if (comp.comp_name.ToLower().Contains("ttl"))
                    {
                        sched =
                           schMgr.calculateScheduleTTL(toBeSaved.amountRequested, toBeSaved.interestRate,
                           toBeSaved.applicationDate, toBeSaved.gracePeriod, toBeSaved.loanTenure,
                           toBeSaved.interestTypeID ?? 1, toBeSaved.repaymentModeID, toBeSaved);
                    }
                    else
                    {
                        sched =
                            schMgr.calculateSchedule(toBeSaved.amountRequested, toBeSaved.interestRate,
                            toBeSaved.applicationDate, toBeSaved.gracePeriod, toBeSaved.loanTenure,
                            toBeSaved.interestTypeID ?? 1, toBeSaved.repaymentModeID, toBeSaved.client);
                    }
                    if (sched != null)
                        foreach (var rs in sched)
                        {
                            rs.creation_date = DateTime.Now;
                            rs.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                            toBeSaved.repaymentSchedules.Add(rs);
                        }
                }
            }
            //save Gurantors
            if (input.lnGurantors.Any())
                foreach (var gurantor in input.lnGurantors)
                {
                    if (gurantor.loanGurantorID > 0)
                    {
                        var loanGurantorToBeUpdated = le.loanGurantors
                                .FirstOrDefault(p => p.loanGurantorID == gurantor.loanGurantorID);
                        populateGurantor(loanGurantorToBeUpdated, gurantor);
                    }
                    else
                    {
                        loanGurantor loanGurantorToBesaved = new loanGurantor();
                        populateGurantor(loanGurantorToBesaved, gurantor);
                        toBeSaved.loanGurantors.Add(loanGurantorToBesaved);
                    }
                }

            //save Collaterals
            if (input.lnCollateral.Any())
                foreach (var collateral in input.lnCollateral)
                {
                    if (collateral.loanCollateralID > 0)
                    {
                        var loanCollateralToBeUpdated = le.loanCollaterals
                            .FirstOrDefault(p => p.loanCollateralID == collateral.loanCollateralID);
                        populateCollateral(loanCollateralToBeUpdated, collateral);
                    }
                    else
                    {
                        loanCollateral loanCollateralToBesaved = new loanCollateral();
                        populateCollateral(loanCollateralToBesaved, collateral);
                        toBeSaved.loanCollaterals.Add(loanCollateralToBesaved);
                    }
                }

            //Save Loan Financials
            if (input.loanFinancials.Any())
                foreach (var financial in input.loanFinancials)
                {
                    if (financial.loanFinancialID > 0)
                    {
                        var financialToBeUpdated = le.loanFinancials
                            .FirstOrDefault(p => p.loanFinancialID == financial.loanFinancialID);
                        populateFinancial(financialToBeUpdated, financial);
                    }
                    else
                    {
                        loanFinancial financialToBeSaved = new loanFinancial();
                        populateFinancial(financialToBeSaved, financial);
                        toBeSaved.loanFinancials.Add(financialToBeSaved);
                    }
                }

            //Save Documents
            if (input.lnDocuments.Any())
                foreach (var document in input.lnDocuments)
                {
                    if (document.loanDocumentID > 0)
                    {
                        var documentToBeUpdated = le.loanDocuments
                            .FirstOrDefault(p => p.loanDocumentID == document.loanDocumentID);
                        populateDocument(documentToBeUpdated, document);
                    }
                    else
                    {
                        loanDocument documentToSave = new loanDocument();
                        populateDocument(documentToSave, document);
                        toBeSaved.loanDocuments.Add(documentToSave);
                    }
                }
        }

        private void populateUpdatedLoan(loan toBeSaved, LoanDetailViewModel input)
        {
            toBeSaved.loanTypeID = input.loanTypeID;
            toBeSaved.amountRequested = input.amountRequested;
            toBeSaved.interestRate = input.interestRate;
            toBeSaved.interestTypeID = input.interestTypeID;
            toBeSaved.last_modifier = toBeSaved.creator;
            toBeSaved.modification_date = DateTime.Now;
            toBeSaved.repaymentModeID = input.repaymentModeID;
            toBeSaved.staffID = input.staffID;
            toBeSaved.agentID = input.agentID;
            toBeSaved.loanTenure = input.loanTenure;
            toBeSaved.repaymentModeID = toBeSaved.repaymentModeID;

            if (toBeSaved.loanStatusID == 0 || toBeSaved.loanStatusID == 1)
            {
                var sch = le.repaymentSchedules.Where(p => p.loanID == toBeSaved.loanID).ToList();
                for (int i = sch.Count - 1; i >= 0; i--)
                {
                    le.repaymentSchedules.Remove(sch[i]);
                }

                //Create Repayment Schedule
                List<coreLogic.repaymentSchedule> sched;
                using (core_dbEntities ctx = new core_dbEntities())
                {
                    var comp = ctx.comp_prof.FirstOrDefault();
                    if (comp.comp_name.ToLower().Contains("ttl"))
                    {
                        sched =
                           schMgr.calculateScheduleTTL(toBeSaved.amountRequested, toBeSaved.interestRate,
                           toBeSaved.applicationDate, toBeSaved.gracePeriod, toBeSaved.loanTenure,
                           toBeSaved.interestTypeID ?? 1, toBeSaved.repaymentModeID, toBeSaved);
                    }
                    else
                    {
                        sched =
                            schMgr.calculateSchedule(toBeSaved.amountRequested, toBeSaved.interestRate,
                            toBeSaved.applicationDate, toBeSaved.gracePeriod, toBeSaved.loanTenure,
                            toBeSaved.interestTypeID ?? 1, toBeSaved.repaymentModeID, toBeSaved.client);
                    }
                    if (sched != null)
                        foreach (var rs in sched)
                        {
                            rs.creation_date = DateTime.Now;
                            rs.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                            toBeSaved.repaymentSchedules.Add(rs);
                        }
                }
            }
            //save Gurantors
            if (input.lnGurantors.Any())
                foreach (var gurantor in input.lnGurantors)
                {
                    if (gurantor.loanGurantorID > 0)
                    {
                        var loanGurantorToBeUpdated = le.loanGurantors
                            .Include(p => p.address)
                            .Include(p => p.image)
                            .Include(p => p.phone)
                            .FirstOrDefault(p => p.loanGurantorID == gurantor.loanGurantorID);
                        populateUpdateGurantor(loanGurantorToBeUpdated, gurantor);
                    }
                    else
                    {
                        loanGurantor loanGurantorToBesaved = new loanGurantor();
                        populateGurantor(loanGurantorToBesaved, gurantor);
                        toBeSaved.loanGurantors.Add(loanGurantorToBesaved);
                    }
                }

            //save Collaterals
            if (input.lnCollateral.Any())
                foreach (var collateral in input.lnCollateral)
                {
                    if (collateral.loanCollateralID > 0)
                    {
                        var loanCollateralToBeUpdated = le.loanCollaterals
                            .FirstOrDefault(p => p.loanCollateralID == collateral.loanCollateralID);
                        populateCollateral(loanCollateralToBeUpdated, collateral);
                    }
                    else
                    {
                        loanCollateral loanCollateralToBesaved = new loanCollateral();
                        populateCollateral(loanCollateralToBesaved, collateral);
                        toBeSaved.loanCollaterals.Add(loanCollateralToBesaved);
                    }
                }

            //Save Loan Financials
            if (input.loanFinancials.Any())
                foreach (var financial in input.loanFinancials)
                {
                    if (financial.loanFinancialID > 0)
                    {
                        var financialToBeUpdated = le.loanFinancials
                            .FirstOrDefault(p => p.loanFinancialID == financial.loanFinancialID);
                        populateFinancial(financialToBeUpdated, financial);
                    }
                    else
                    {
                        loanFinancial financialToBeSaved = new loanFinancial();
                        populateFinancial(financialToBeSaved, financial);
                        toBeSaved.loanFinancials.Add(financialToBeSaved);
                    }
                }

            //Save Documents
            if (input.lnDocuments.Any())
                foreach (var document in input.lnDocuments)
                {
                    if (document.loanDocumentID > 0)
                    {
                        var documentToBeUpdated = le.loanDocuments
                            .FirstOrDefault(p => p.loanDocumentID == document.loanDocumentID);
                        populateDocument(documentToBeUpdated, document);
                    }
                    else
                    {
                        loanDocument documentToSave = new loanDocument();
                        populateDocument(documentToSave, document);
                        toBeSaved.loanDocuments.Add(documentToSave);
                    }
                }
        }

        private void populateGurantor(loanGurantor toBeSaved, LoanGurantorModel input)
        {
            toBeSaved.surName = input.surName;
            toBeSaved.otherNames = input.otherNames;
            toBeSaved.DOB = input.DOB;
            toBeSaved.idNo = new idNo
            {
                idNoTypeID = input.idTypeId,
                idNo1 = input.IdNumber
            };
            toBeSaved.address = new address
            {
                addressLine1 = input.addressLine,
                cityTown = input.city,
            };
            toBeSaved.phone = new phone
            {
                phoneTypeID = input.phoneTypeId,
                phoneNo = input.phoneNumer
            };
            toBeSaved.email = new email
            {
                emailAddress = input.emailAddress
            };
            if (input.gurantorPhotos != null)
            {
                var gurantorPhoto = input.gurantorPhotos.FirstOrDefault();
                if (gurantorPhoto != null)
                    toBeSaved.image = new image
                    {
                        description = gurantorPhoto.fileName,
                        image1 = Convert.FromBase64String(gurantorPhoto.photo),
                        content_type = gurantorPhoto.mimeType
                    };
            }
            
            if (toBeSaved.loanGurantorID < 1)
            {
                toBeSaved.creation_date = DateTime.Now;
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            else
            {
                toBeSaved.modification_date = DateTime.Now;
                toBeSaved.last_modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
        }

        private void populateUpdateGurantor(loanGurantor toBeSaved, LoanGurantorModel input)
        {
            toBeSaved.surName = input.surName;
            toBeSaved.otherNames = input.otherNames;
            toBeSaved.DOB = input.DOB;
            var id = le.idNoes.FirstOrDefault(p => p.idNoID == toBeSaved.idNo.idNoID);
            if (id != null)
            {
                id.idNoTypeID = input.idTypeId;
                id.idNo1 = input.IdNumber;
            }
            var addrs = le.addresses.FirstOrDefault(p => p.addressID == toBeSaved.address.addressID);
            if (addrs != null)
            {
                addrs.addressLine1 = input.addressLine;
                addrs.cityTown = input.city;
            }
            var phone = le.phones.FirstOrDefault(p => p.phoneID == toBeSaved.phone.phoneID);
            if (phone != null)
            {
                phone.phoneTypeID = input.phoneTypeId;
                phone.phoneNo = input.phoneNumer;
            }
            var email = le.emails.FirstOrDefault(p => p.emailID == toBeSaved.email.emailID);
            if (email != null)
            {
                email.emailAddress = input.emailAddress;
            }

            if (input.gurantorPhotos != null)
            {
                var gurantorPhoto = input.gurantorPhotos.FirstOrDefault();
                if (gurantorPhoto != null)
                {
                    var guranImg = le.images.FirstOrDefault(p => p.imageID == toBeSaved.image.imageID);
                    if (guranImg != null)
                    {
                        guranImg.description = gurantorPhoto.fileName;
                        guranImg.image1 = Convert.FromBase64String(gurantorPhoto.photo);
                        guranImg.content_type = gurantorPhoto.mimeType;
                    }
                }
                
            }

            if (toBeSaved.loanGurantorID < 1)
            {
                toBeSaved.creation_date = DateTime.Now;
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            else
            {
                toBeSaved.modification_date = DateTime.Now;
                toBeSaved.last_modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
        }

        private void populateCollateral(loanCollateral toBeSaved, LoanCollateralModel input)
        {
            toBeSaved.collateralTypeID = input.collateralTypeID;
            toBeSaved.legalOwner = input.legalOwner;
            toBeSaved.collateralDescription = input.collateralDescription;
            if (toBeSaved.loanCollateralID > 0)
            {
                toBeSaved.fairValue = input.fairValue;
                toBeSaved.modification_date = DateTime.Now;
                toBeSaved.last_modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            else
            {
                toBeSaved.fairValue = input.fairValue;
                toBeSaved.creation_date = DateTime.Now;
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }

            if (input.collateralPhotos != null)
                foreach (var photo in input.collateralPhotos)
                {
                    if (photo.collateralImageID > 0)
                    {
                        var collateralImageToBeUpdated = new collateralImage();
                        populateCollateralPhoto(collateralImageToBeUpdated, photo);
                    }
                    else
                    {
                        collateralImage collateralImageToBeSaved = new collateralImage();
                        populateCollateralPhoto(collateralImageToBeSaved, photo);
                        toBeSaved.collateralImages.Add(collateralImageToBeSaved);
                    }
                }
        }

        private void populateCollateralPhoto(collateralImage toBeSaved, CollateralImageViewModel input)
        {
            toBeSaved.image = new image
            {
                description = input.fileName,
                image1 = Convert.FromBase64String(input.photo),
                content_type = input.mimeType
            };
        }

        private void populateFinancial(loanFinancial toBeSaved, loanFinancial input)
        {
            toBeSaved.financialTypeID = input.financialTypeID;
            toBeSaved.revenue = input.revenue;
            toBeSaved.expenses = input.expenses;
            toBeSaved.otherCosts = input.otherCosts;
            toBeSaved.frequencyID = input.frequencyID;
        }

        private void populateDocument(loanDocument toBeSaved, LoanDocumentModel input)
        {
            toBeSaved.document = new document
            {
                documentImage = Convert.FromBase64String(input.docum),
                description = input.description,
                contentType = input.mimeType,
                fileName = input.fileName
            };
        }

        private void populateSavingsFields(saving target, LoanDetailViewModel source)
        {
            target.clientID = source.clientID;
            target.savingTypeID = 2;
            target.autoRollover = false;
            var svTyp = le.savingTypes.FirstOrDefault(p => p.savingTypeID == target.savingTypeID);
            var cl = le.clients.FirstOrDefault(p => p.clientID == source.clientID);
            target.savingNo = idGen.NewSavingsNumber(cl.branchID.Value,
                        cl.clientID, target.savingID, svTyp.savingTypeName.Substring(0, 2).ToUpper());
            target.period = (int)source.loanTenure;
            target.maturityDate = source.applicationDate.AddMonths((int)source.loanTenure);
            target.currencyID = 1;
            target.firstSavingDate = source.applicationDate;
            target.fxRate = 1;
            target.status = "A";
            target.interestMethod = true;
            target.principalRepaymentModeID = -1;
            target.interestRepaymentModeID = -1;
            target.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            target.creation_date = DateTime.Now;
        }


        //private void converToCompanyBaseCurrency(LoanDetailViewModel input)
        //{
        //    input.amountRequested = convertor.convertToCompanyBaseLocalAmount(input.applicationDate, input.amountRequested);
        //    input.commission = convertor.convertToCompanyBaseLocalAmount(input.applicationDate, input.commission);
        //}

    }
}
