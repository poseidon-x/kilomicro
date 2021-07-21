//*******************************************
//***   CLIENT DEPOSIT CHECK API - CONTROLLER                
//***   CREATOR: EMMANUEL OWUSU(MAN)    	   
//***   WEEK: AUG 28TH, 2015  	
// MODIFIED BY: SAMUEL WENDOLIN KETECHIE
// MODIFIED DATE: DEC 20, 2020
//*******************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using coreData.Constants;
using coreData.ErrorLog;
using coreERP.Models.Client;
using coreERP.Models.Image;

namespace coreERP.Controllers.Clients
{
    [AuthorizationFilter()]
    public class ClientController : ApiController
    {
        IcoreLoansEntities le;
        Icore_dbEntities ctx;
        private readonly ErrorMessages error = new ErrorMessages();
        private IIDGenerator idGen = new IDGenerator();
        private HelperMethod helper;

        //Default constructor
        public ClientController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx = new core_dbEntities();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
            helper = new HelperMethod();
        }

        //Constructor for unit testing
        public ClientController(IcoreLoansEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }

        // GET: api/Category
        public IEnumerable<Models.LookupEntry> Get()
        {
            List<Models.LookupEntry> list = new List<Models.LookupEntry>();

            var clients = le.clients
                .AsNoTracking()
                .ToList();
            using (var ent = new coreSecurityEntities())
            {
                foreach (var item in clients)
                {
                    var fullName = (item.clientCompany != null && item.clientCompany.companyName != "") ?
                        item.clientCompany.companyName : ((item.accountName != null && item.accountName.Trim() != "") ?
                        item.accountName : item.surName + ", " + item.otherNames);
                    list.Add(new Models.LookupEntry
                    {
                        Description = fullName,
                        ID = item.clientID
                    });
                }
            }

            return list;
        }


        // GET: api/
        [HttpGet]
        public ClientModel Get(int id)
        {
            var client = le.clients
                .Where(p => p.clientID == id)
                .Select(p => new ClientModel
                {
                    clientId = p.clientID,
                    accountNumber = p.accountNumber,
                    clientTypeId = p.clientTypeID,
                    surName = p.surName,
                    otherNames = p.otherNames,
                    dateOfBirth = p.DOB,
                    sex = p.sex,
                    maritalStatusId = p.maritalStatusID,
                    industryId = p.industryID,
                    branchId = p.branchID,
                    sectorId = p.sectorID,
                    clientCategoryId = p.categoryID,
                    primaryIdTypeId = p.idNo.idNoTypeID,
                    primaryIdNo = p.idNo.idNo1,
                    primaryIdExpiry = p.idNo.expriryDate,
                    secondaryIdTypeId = p.idNo1.idNoTypeID,
                    secondaryIdNo = p.idNo1.idNo1,
                    secondaryIdExpiry = p.idNo1.expriryDate
                })
                .FirstOrDefault();

            if (client == null)
            {
                client = new ClientModel
                {
                    clientPhones = new List<ClientPhoneModel>(),
                    clientEmails = new List<ClientEmailModel>(),
                    supportingDocuments = new List<SupportingDocumentModel>()
                };
            }

            return client;
        }

        [HttpPost]
        public client Post(ClientModel input)
        {
            //Validate input
            //validateInput(input);
            client client = null;
            if (input.clientId > 0)
            {
                client = le.clients
                    .FirstOrDefault(p => p.clientID == input.clientId);
                PopulateClientFields(client, input);
            }
            else
            {
                client = new client();
                PopulateClientFields(client, input);
                le.clients.Add(client);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                //If saving fails, Log the the Exception and display message to user.
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return client;
        }


        [HttpPost]
        public client SaveNewClient(ClientModel input)
        {
            var client = new client();
            PopulateClientFields(client, input);
            le.clients.Add(client);

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            return client;
        }



        //populate clientCheck the fields to save
        private void PopulateClientFields(client clientToBeSaved, ClientModel input)
        {
            var comp = ctx.comp_prof.FirstOrDefault();
            var branch = le.branches.FirstOrDefault(p => p.branchID == input.branchId);
            clientToBeSaved.surName = helper.NameToCamelCase(input.surName);
            clientToBeSaved.otherNames = helper.NameToCamelCase(input.otherNames);
            clientToBeSaved.DOB = input.dateOfBirth;
            clientToBeSaved.maritalStatusID = input.maritalStatusId;
            clientToBeSaved.sex = input.sex;
            clientToBeSaved.clientTypeID = input.clientTypeId;
            clientToBeSaved.industryID = input.industryId;
            clientToBeSaved.sectorID = input.sectorId;
            clientToBeSaved.branchID = input.branchId;
            clientToBeSaved.categoryID = input.clientCategoryId;
            clientToBeSaved.companyName = comp.comp_name;
            clientToBeSaved.creation_date = DateTime.Now;
            clientToBeSaved.accountName = helper.NameToCamelCase(input.surName + " " + input.otherNames);

            //Save ID informations
            PopulateClientIDNumberFields(clientToBeSaved, input);

            var currentUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            //Generate client account number
            if (input.clientId < 1)
            {
                if (comp.traditionalLoanNo)
                {
                    if (input.clientCategoryId == 0)
                        input.clientCategoryId = 1;
                    clientToBeSaved.accountNumber = idGen.NewClientAccountNumber(input.branchId.Value, input.clientCategoryId.Value);
                }
                else
                {
                    clientToBeSaved.accountNumber = coreExtensions.NextSystemNumber("loan.cl.accountNumber." + branch.branchName.Substring(0, 2).ToUpper());
                }
                clientToBeSaved.creator = currentUser;
            }
            else
            {
                clientToBeSaved.last_modifier = currentUser;
                clientToBeSaved.modification_date = DateTime.Now;
            }

            //Save Client Email
            SaveClientEmails(clientToBeSaved, input);
            
            //Save Client Phones
            SaveClientPhones(clientToBeSaved, input);

            //Save Client Supporting Documents
            if (input.supportingDocuments != null)
            {
                SaveClientSupportingDocuments(clientToBeSaved, input);
            }

            //Save Cient Residential Address
            SaveClientAddresses(clientToBeSaved, input);

            //Save client image
            SaveClientImage(clientToBeSaved, input);

            //Save Client NOK Info
            PopulateNOKInfo(clientToBeSaved, input);

            //Loan Group Client
            if (input.loanGroupId != null && input.clientTypeId == 7)
            {
                AddClientToLoanGroup(clientToBeSaved, input, currentUser);
            }

        }

        private void PopulateClientIDNumberFields(client clientToBeSaved, ClientModel input)
        {
            clientToBeSaved.idNo = new idNo
            {
                idNo1 = input.primaryIdNo,
                idNoTypeID = input.primaryIdTypeId,
                expriryDate = input.primaryIdExpiry
            };

            if (input.secondaryIdTypeId != null && !string.IsNullOrWhiteSpace(input.secondaryIdNo))
            {
                clientToBeSaved.idNo1 = new idNo
                {
                    idNo1 = input.secondaryIdNo,
                    idNoTypeID = input.secondaryIdTypeId.Value,
                    expriryDate = input.secondaryIdExpiry
                };
            }
        }

        private void AddClientToLoanGroup(client clientToBeSaved, ClientModel input, string currentUser)
        {
            clientToBeSaved.loanGroupClients.Add(new loanGroupClient
            {
                loanGroupId = input.loanGroupId.Value,
                created = DateTime.Now,
                creator = currentUser
            });
        }

        private void SaveClientImage(client clientToBeSaved, ClientModel input)
        {
            clientImage img = new clientImage();
            byte[] b = Convert.FromBase64String(input.imageDataString);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
            System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
            i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
            ms = null;
            ms = new System.IO.MemoryStream();
            i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            b = ms.ToArray();
            i2 = null;
            ms = null;

            var camImage = new image
            {
                description = clientToBeSaved.surName + " " + clientToBeSaved.otherNames + " Webcam-Picure",
                image1 = b,
                content_type = "image/png"
            };
            img.image = camImage;
            clientToBeSaved.clientImages.Add(img);
        }

        private void SaveClientAddresses(client toBeSaved, ClientModel input)
        {
            if (input.clientAddress.addressId > 0)
            {
                var addressToUpdate = le.clientAddresses
                    .FirstOrDefault(p => p.addressID == input.clientAddress.addressId);
                if (addressToUpdate != null)
                {
                    populateclientAddress(addressToUpdate, input.clientAddress);
                }
            }
            else
            {
                clientAddress addressToSave = new clientAddress
                {
                    addressTypeID = 1
                };
                populateclientAddress(addressToSave, input.clientAddress);
                toBeSaved.clientAddresses.Add(addressToSave);
            }
        }

        private void SaveClientSupportingDocuments(client toBeSaved, ClientModel input)
        {
            foreach (var document in input.supportingDocuments)
            {
                if (document.documentId > 0)
                {
                    var documentToBeUpdated = le.clientDocuments
                        .FirstOrDefault(p => p.documentID == document.documentId);
                    populateDocument(documentToBeUpdated, document);
                }
                else
                {
                    clientDocument documentToSave = new clientDocument();
                    populateDocument(documentToSave, document);
                    toBeSaved.clientDocuments.Add(documentToSave);
                }
            }
        }

        private void SaveClientPhones(client clientToBeSaved, ClientModel input)
        {
            foreach (var phone in input.clientPhones)
            {
                if (phone.phoneId > 0)
                {
                    var toBeUpdated = clientToBeSaved.clientPhones.FirstOrDefault(p => p.phoneID == phone.phoneId);
                    if (toBeUpdated != null)
                    {
                        toBeUpdated.phoneTypeID = phone.phoneTypeId;
                        populatePhone(toBeUpdated, phone);
                    }
                }
                else
                {
                    coreLogic.clientPhone phoneToBeSaved = new coreLogic.clientPhone
                    {
                        phoneTypeID = phone.phoneTypeId
                    };
                    populatePhone(phoneToBeSaved, phone);
                    clientToBeSaved.clientPhones.Add(phoneToBeSaved);
                }
            }
        }

        private void SaveClientEmails(client toBeSaved, ClientModel input)
        {
            foreach (var email in input.clientEmails)
            {
                if (email.emailId > 0)
                {
                    var toBeUpdated = toBeSaved.clientEmails.FirstOrDefault(p => p.emailID == email.emailId);
                    if (toBeUpdated != null)
                    {
                        toBeUpdated.emailTypeID = email.emailTypeId;
                        populateEmail(toBeUpdated, email);
                    }
                }
                else
                {
                    coreLogic.clientEmail emailToBeSaved = new coreLogic.clientEmail
                    {
                        emailTypeID = email.emailTypeId
                    };
                    populateEmail(emailToBeSaved, email);
                    toBeSaved.clientEmails.Add(emailToBeSaved);
                }
            }
        }

        private void populateEmail(clientEmail toBeSaved, ClientEmailModel input)
        {
            toBeSaved.email = new email
            {
                emailTypeID = input.emailTypeId,
                emailAddress = input.emailAddress
            };
        }

        private void populatePhone(clientPhone toBeSaved, ClientPhoneModel input)
        {
            toBeSaved.phone = new phone
            {
                phoneTypeID = input.phoneTypeId,
                phoneNo = input.phoneNumber
            };
        }

        private void populateDocument(clientDocument toBeSaved, SupportingDocumentModel input)
        {
            toBeSaved.document = new document
            {
                documentImage = Convert.FromBase64String(input.document),
                description = input.description,
                contentType = input.mimeType,
                fileName = input.fileName
            };
        }

        private void populateclientAddress(clientAddress toBeSaved, ClientResidentialAddress input)
        {
            if (toBeSaved.clientAddressID > 0)
            {
                var adddressToBeUpdated = le.addresses.FirstOrDefault(p => p.addressID == toBeSaved.addressID);
                if (adddressToBeUpdated != null)
                {
                    populateAddress(adddressToBeUpdated, input);
                }
            }
            else
            {
                var adddressToBeSaved = new address();
                populateAddress(adddressToBeSaved, input);
            }
        }

        private void populateAddress(address toBeSaved, ClientResidentialAddress input)
        {
            toBeSaved.addressLine1 = input.addressLine;
            toBeSaved.addressLine2 = input.landMark;
            toBeSaved.cityTown = input.city;
            //Save Address Image
            if (input.addressImage != null)
            {
                if (input.addressImage.imageId > 0)
                {
                    var image = le.images.FirstOrDefault(p => p.imageID == input.addressImage.imageId);
                    if (image != null)
                    {
                        populateAddressImage(image, input.addressImage);
                    }
                }
                else
                {
                    var imageToBeSaved = new image();
                    populateAddressImage(imageToBeSaved, input.addressImage);
                    le.images.Add(imageToBeSaved);
                }
            }
        }

        private void populateAddressImage(image toBeSaved, ClientResidentialAddressImageViewModel input)
        {
            toBeSaved.description = input.fileName;
            toBeSaved.image1 = Convert.FromBase64String(input.photo);
            toBeSaved.content_type = input.mimeType;
        }

        //populate clientCheck the fields to save
        private void validateInput(clientServiceCharge input)
        {

            if (input == null || input.posted)
            {
                StringBuilder errorToReturn = new StringBuilder();
                if (input == null)
                    errorToReturn.Append(error.InputDataEmpty);
                if (input.posted)
                    errorToReturn.Append(error.ChargePostedAlready);

                if (!String.IsNullOrEmpty(errorToReturn.ToString()))
                {
                    Logger.logError(errorToReturn.ToString());
                    throw new ApplicationException(errorToReturn.ToString());
                }

            }
        }


        //NEXT OK KIN


        private void PopulateNOKInfo(client clientToBeSaved, ClientModel input)
        {
            var nokToBeSaved = new nextOfKin();

            PopulateNOKEmail(nokToBeSaved, input.nextOfKin);
            PopulateNOKPhone(nokToBeSaved, input.nextOfKin);
            SaveNOKImage(nokToBeSaved, input.nextOfKin);
            PopulateNokID(nokToBeSaved, input.nextOfKin);
            nokToBeSaved.surName = input.nextOfKin.surName;
            nokToBeSaved.otherNames = input.nextOfKin.otherNames;
            nokToBeSaved.relationship = input.nextOfKin.relationship;
            clientToBeSaved.nextOfKins.Add(nokToBeSaved);
        }


        private void PopulateNOKEmail(nextOfKin nokToBeSaved, ClientNextOfKin input)
        {
            if (!string.IsNullOrWhiteSpace(input.emailAddress))
            {
                nokToBeSaved.email = new email
                {
                    emailTypeID = 2,
                    emailAddress = input.emailAddress
                };
            }
            
        }

        private void PopulateNokID(nextOfKin nokToBeSaved, ClientNextOfKin input)
        {
            if (!string.IsNullOrWhiteSpace(input.idNumber) && input.idNoTypeID >0)
            {
                nokToBeSaved.idNo = new idNo
                {
                    idNo1 = input.idNumber,
                    idNoTypeID = input.idNoTypeID
                };
            }
            
        }

        private void PopulateNOKPhone(nextOfKin nokToBeSaved, ClientNextOfKin input)
        {
            nokToBeSaved.phone = new phone
            {
                phoneTypeID = 2,
                phoneNo = input.phoneNumber
            };
        }

        private void SaveNOKImage(nextOfKin nokToBeSaved, ClientNextOfKin input)
        {
            if (!string.IsNullOrWhiteSpace(input.imageDataString)){
                byte[] b = Convert.FromBase64String(input.imageDataString);

                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                System.Drawing.Image i2 = System.Drawing.Image.FromStream(ms);
                i2 = i2.GetThumbnailImage(480, 480, null, IntPtr.Zero);
                ms = null;
                ms = new System.IO.MemoryStream();
                i2.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                b = ms.ToArray();
                i2 = null;
                ms = null;

                var camImage = new image
                {
                    description = input.surName + " " + input.otherNames + " Picture",
                    image1 = b,
                    content_type = "image/png"
                };
                nokToBeSaved.image = camImage;
            }
            
        }



    }
}
