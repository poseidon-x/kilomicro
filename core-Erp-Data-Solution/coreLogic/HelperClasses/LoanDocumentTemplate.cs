using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using coreLogic.Models.Loans;

namespace coreLogic.HelperClasses
{
    public class LoanDocumentTemplate
    {
        private coreLoansEntities le = new coreLoansEntities();
        private core_dbEntities ctx = new core_dbEntities();

        private loan ln;
        private int documentId;

        //Constructor to intialize private variables
        public LoanDocumentTemplate(int loanId, int documentId)
        {
            ln = le.loans
                .Include(p => p.loanAdditionalInfoes
                .Select(q => q.loanMetaDatas))
                .FirstOrDefault(p => p.loanID == loanId);
            this.documentId = documentId;
        }

        public byte[] getCompLogo()
        {
            return ctx.comp_prof.First().logo;
        }

        public loan getLoan()
        {
            return ln;
        }

        public string getTemplateName()
        {
            return le.loanDocumentTemplates
                .FirstOrDefault(p => p.loanDocumentTemplateId == documentId).templateName;
        }
        public string getCompanyName()
        {
            return ctx.comp_prof.FirstOrDefault().comp_name;
        }

        public string getCompanyEmail()
        {
            return ctx.comp_prof.FirstOrDefault().email;
        }

        public string getCompanyCeoName()
        {
            var ceoJobTitleId = le.jobTitles.Where(p => p.jobTitleName.ToLower()
            .Contains("ceo")).FirstOrDefault().jobTitleID;
            var ceoDetails = le.staffs.FirstOrDefault(p => p.jobTitleID == ceoJobTitleId);

            return ceoDetails.surName + " " + ceoDetails.otherNames;
        }

        public string getCompanyOperManagerName()
        {
            var operMangJobTitleId = le.jobTitles.Where(p => p.jobTitleName.ToLower()
            .Contains("general manager operations")).FirstOrDefault().jobTitleID;
            var operMangDetails = le.staffs.FirstOrDefault(p => p.jobTitleID == operMangJobTitleId);

            return operMangDetails.surName + " " + operMangDetails.otherNames;
        }

        public string getCompanyPostalAddress()
        {
            return ctx.comp_prof.FirstOrDefault().addr_line_1;
        }

        public string getCompanyHeadOfficeLocationAddress()
        {
            return ctx.comp_prof.FirstOrDefault().addr_line_2;
        }

        public string getCompanyRegion()
        {
            var comp = ctx.comp_prof
                    .First();

            var distId = ctx.cities.FirstOrDefault(p => p.city_id == comp.city_id).districts.district_id;
            var regId = ctx.districts.FirstOrDefault(p => p.district_id == distId).regions.region_id;
            var region = ctx.regions.FirstOrDefault(p => p.region_id == regId).region_name;

            return region;
        }

        public string getClientName()
        {

            var cl = le.clients
                .Include(p => p.clientAddresses)
                .FirstOrDefault(p => p.clientID == ln.clientID);

            return cl.surName + " " + cl.otherNames;
        }

        public string getLoanProduct()
        {
            return le.loanTypes.FirstOrDefault(p => p.loanTypeID == ln.loanTypeID).loanTypeName;
        }

        public string getLoanTenure()
        {
            return ln.loanTenure + " Months";
        }

        public string getLoanRepaymentMode()
        {
            return le.repaymentModes.FirstOrDefault(p => p.repaymentModeID == ln.repaymentModeID).repaymentModeName;
        }

        public double getLoanInterestRate()
        {
            return ln.interestRate;
        }

        public string getLoanGuarantorName()
        {
            var lnGuarantor = ln.loanGurantors.FirstOrDefault(p => p.loanID == ln.loanID);
            return lnGuarantor.surName + " " + lnGuarantor.otherNames;
        }

        public string getLoanGuarantorAddress()
        {
            var lnGuarantor = ln.loanGurantors.FirstOrDefault(p => p.loanID == ln.loanID);
            var lnGuarantorAddress = le.addresses.FirstOrDefault(p => p.addressID == lnGuarantor.addressID);
            return lnGuarantorAddress.addressLine1;
        }

        public string getLoanGuarantorWitness()
        {
            var cl = le.clients
                .Include(p => p.clientAddresses)
                .FirstOrDefault(p => p.clientID == ln.clientID);

            return cl.surName + " " + cl.otherNames;
        }

        public string getLoanApplicationFee()
        {
            return ln.applicationFee.ToString("N0");
        }

        public string getLoanProcessingFee()
        {
            return ln.processingFee.ToString("N0");
        }

        public string getLoanInsuranceCharges()
        {
            return ln.insuranceAmount.ToString("N0");
        }

        public string getLoanMaturitySum()
        {
            return ((ln.amountDisbursed * ln.interestRate) * ln.loanTenure + ln.amountDisbursed).ToString("N0");
        }

        public DateTime getLoanmaturityDate()
        {
            return ln.disbursementDate.Value.AddMonths((int)ln.loanTenure); 
        }

        public string getLoanCollateralValue()
        {
            var collateral = ln.loanCollaterals.First();
            return collateral.fairValue.ToString("N");
        }

        public string getLoanCollateral()
        {
            var collateral = ln.loanCollaterals.First();
            return collateral.collateralDescription;
        }

        public string getClientLocation()
        {
            var cl = le.clients
                .Include(p => p.clientAddresses)
                .FirstOrDefault(p => p.clientID == ln.clientID);

            var addrId = cl.clientAddresses.First().addressID;

            return le.addresses.FirstOrDefault(p => p.addressID == addrId).addressLine1;
        }




    [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<LoanDocumentPageViewModel> GetPages(int documentId)
        {
            //Retrieve all pages for the document
            var pages = le.loanDocumentTemplatePages
               .Where(p => p.loanDocumentTemplateId == documentId)
               .Select(i => new LoanDocumentPageViewModel
               {
                   loanDocumentTemplatePageId = i.loanDocumentTemplateId,
                   pageNumber = i.pageNumber,
                   content = i.content,
                   placeHolderIds = i.loanDocumentTemplatePagePlaceHolders
                                .Where(p => p.loanDocumentTemplatePageId == i.loanDocumentTemplatePageId)
                                .Select(k => k.placeHolderTypeId)
               })
               .OrderBy(p => p.pageNumber)
               .ToList();

            //retrieve placeHolder codes from placeHolderTypes
            foreach (var page in pages)
            {
                page.placeHolders = le.loanDocumentPlaceHolderTypes
                    .Where(p => page.placeHolderIds.Contains(p.loanDocumentPlaceHolderTypeId))
                    .Select(p => p.placeHolderTypeCode)
                    .ToList();
            }
            return pages;

        }

        public LoanDocumentViewModel getLoanDocument(int LoanId)
        {
            LoanDocumentViewModel document = new LoanDocumentViewModel
            {
                date = DateTime.Now.ToString("dd-MMM-yyyy"),
                loanId = ln.loanID,
                loanNumber = ln.loanNo,
                clientId = ln.clientID,
                loanAmountInFigures = ln.amountDisbursed.ToString("N0"),
                loanAmountInWords = NumberToWordsConverter.NumberToWords((int)ln.amountDisbursed).ToUpper(),
            };

            return document;
        }
    }
}
