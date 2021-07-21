using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using coreLogic;
using coreData.Constants;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using System.Text;
using Microsoft.Ajax.Utilities;
using System.Threading.Tasks;
using agencyAPI.Models;
using agencyAPI.Providers;

namespace agencyAPI.Controllers
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class ClientController : ApiController
    {
        private readonly coreLoansEntities db;
        private readonly core_dbEntities le;
        private IIDGenerator idGen;
        private ErrorMessages error = new ErrorMessages();

     
        public ClientController()
        {//constructor
            var db2 = new coreLoansEntities();
            db2.Configuration.LazyLoadingEnabled = false;
            db2.Configuration.ProxyCreationEnabled = false;
            db = db2;

            var db1 = new core_dbEntities();
            db1.Configuration.LazyLoadingEnabled = false;
            db1.Configuration.ProxyCreationEnabled = false;
            le = db1;

            idGen = new IDGenerator();
        }

        // GET: Client
        [HttpGet]
        public IEnumerable<client> Get()
        {//get list of all entries 
            return db.clients.ToList();
        }

        // GET: Client/5
        [HttpGet]
        public ClientModel Get(int id)
        {//get list of all entries 
            ClientModel cl = new ClientModel();

            var data = db.clients
                .FirstOrDefault(p => p.clientID == id);
            
            if (data == null)
            {
                cl.client = new client();
            }

            return cl;
        }

        // POST: Client
        [HttpPost]
        public client Post(ClientModel CModel)
        {
            //Validate the input value
            validateClient(CModel.client);

            if (CModel.client.clientID > 0)
            { 
                //client
                var clientToBeSaved = db.clients
                    .FirstOrDefault(p => p.clientID == CModel.client.clientID);
                populateClientFields(clientToBeSaved, CModel.client);

                //address
                var addressToBeSaved = db.addresses
                    .FirstOrDefault(p => p.addressID == CModel.clientAddress.addressID);
                populateAddressFields(addressToBeSaved, CModel.clientAddress);

                //phone
                var phoneToBeSaved = db.phones
                    .FirstOrDefault(p => p.phoneID == CModel.clientPhone.phoneID);
                populatePhoneFields(phoneToBeSaved, CModel.clientPhone);

                //email 
                var emailToBeSaved = db.emails
                    .FirstOrDefault(p => p.emailID == CModel.clientEmail.emailID);
                populateEmailFields(emailToBeSaved, CModel.clientEmail);
            }
            else
            {
                //client
                client clientToBeSaved = new client();
                populateClientFields(clientToBeSaved, CModel.client);
                db.clients.Add(clientToBeSaved);

                //address
                address addToBeSaved = new address{
                    addressTypeID = CModel.clientAddress.addressTypeID,
                    addressLine1 = CModel.clientAddress.addressLine1,
                    cityTown = CModel.clientAddress.cityTown
                };
                db.addresses.Add(addToBeSaved);
                
                clientAddress clAddToBeSaved = new clientAddress
                {
                    addressTypeID = addToBeSaved.addressTypeID,
                    address = addToBeSaved
                };
                clientToBeSaved.clientAddresses.Add(clAddToBeSaved);

                //email
                email emailToBeSaved = new email
                {
                    emailTypeID = CModel.clientEmail.emailTypeID,
                    emailAddress = CModel.clientEmail.emailAddress
                };
                db.emails.Add(emailToBeSaved);
                
                clientEmail clientEmailToBeSaved = new clientEmail
                {
                    emailTypeID = emailToBeSaved.emailTypeID,
                    email = emailToBeSaved,
                };
                clientToBeSaved.clientEmails.Add(clientEmailToBeSaved);

                //phone
                phone phoneToBeSaved = new phone
                {
                    phoneTypeID = CModel.clientPhone.phoneTypeID,
                    phoneNo = CModel.clientPhone.phoneNo
                };
                db.emails.Add(emailToBeSaved);

                clientPhone clientPhoneToBeSaved = new clientPhone
                {
                    phoneTypeID = phoneToBeSaved.phoneTypeID,
                    phone = phoneToBeSaved,
                };
                clientToBeSaved.clientPhones.Add(clientPhoneToBeSaved);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return CModel.client;
        }

        private void populateClientFields(client target, client source)
        {
           //ClientModel CModel;

            //target.accountNumber = idGen.NewClientAccountNumber((int)source.branchID, (int)source.categoryID);
            var prof = le.comp_prof.FirstOrDefault();
            var br = db.branches.FirstOrDefault(p => p.branchID == source.branchID);
            var cat = db.categories.FirstOrDefault(p => p.categoryID == source.categoryID);

            if (prof.traditionalLoanNo == true)
            {
                var t = source.categoryID.ToString();
                if (t == "0") t = "1";
                target.accountNumber = idGen.NewClientAccountNumber(source.branchID.Value, source.categoryID.Value);
            }
            else
            {
                target.accountNumber = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + br.branchName.Substring(0, 2).ToUpper());
            }
            target.surName = source.surName;
            target.otherNames = source.otherNames;
            target.DOB = source.DOB;
            target.maritalStatusID = source.maritalStatusID;
            target.sex = source.sex;
            target.branchID = source.branchID;
            target.categoryID = source.categoryID;
            //target.idNoID = source.idNoID;
            target.creation_date = DateTime.Now;
            target.creator = User.Identity.Name;
            //target.creator = LoginHelper.getCurrentUser(le);
            target.clientTypeID = source.clientTypeID;
            target.companyName = "N/A";
            target.isCompany = false;
            target.secondSurName = source.secondSurName != null ? source.secondSurName: "";
            target.secondOtherNames = source.secondOtherNames != null ? source.secondOtherNames: "";
            target.thirdSurName = source.thirdSurName != null ? source.thirdSurName: "";
            target.thrifOtherNames = source.thrifOtherNames != null ? source.thrifOtherNames: "";
            target.accountName = source.accountName != null ? source.accountName: "";
            target.admissionFee = 0.00;
            
            if (source.clientID > 0)
            {
                target.modification_date = DateTime.Now;
                target.last_modifier = " ";
            }
        }

        private void populateAddressFields(address target, address source)
        {
            target.addressTypeID = source.addressTypeID;
            target.addressLine1 = source.addressLine1;
            target.cityTown = source.cityTown;
        }

        private void populatePhoneFields(phone target, phone source)
        {
            target.phoneTypeID = source.phoneTypeID;
            target.phoneNo = source.phoneNo;
        }

        private void populateEmailFields(email target, email source)
        {
            target.emailTypeID = source.emailTypeID;
            target.emailAddress = source.emailAddress;
        }

        private void validateClient(client client)
        {// validation for news category attributes
            if (client.surName.IsNullOrWhiteSpace()
                || client.surName.Length > 100 || client.otherNames.IsNullOrWhiteSpace() || 
                client.otherNames.Length > 100 || client.branchID < 1 || client.categoryID < 1 
                ||client.clientTypeID < 1)
            {
                StringBuilder errors = new StringBuilder();
                if (client.surName.IsNullOrWhiteSpace())
                    errors.Append(error.ClientSurnameEmpty);
                if (client.surName.Length > 100)
                    errors.Append(error.ClientSurnameLength);
                if (client.otherNames.IsNullOrWhiteSpace())
                    errors.Append(error.ClientOtherNamesEmpty);
                if (client.otherNames.Length > 100)
                    errors.Append(error.ClientOtherNamesLength);
                if (client.branchID < 1)
                    errors.Append(error.ClientBranchEmpty);
                if (client.categoryID < 1)
                    errors.Append(error.ClientCategoryEmpty);
                if (client.clientTypeID < 1)
                    errors.Append(error.ClientTypeEmpty);
                throw new ApplicationException(errors.ToString());
            }
        }
    }
}
