//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Security.Policy;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace coreErpApi.Controllers.Controllers.Customers
//{
//    [AuthorizationFilter()]
//    public class CustomerController : ApiController
//    {
//        private const string CUSTOMER_DROP_DOWN_ERROR_MESSAGE = "One or More form drop down field(s) have invalid value<br />";
//        private const string CUSTOMER_EMPTY_FIELDS_ERROR_MESSAGE = "One or More form field(s) are Empty<br />";
//        private const string BUSINESS_ADDRESS_DROP_DOWN_N_AUTO_COMPLETE_ERROR_MESSAGE = "Invalid address type/country for Business Address Grid<br />";
//        private const string BUSINESS_ADDRESS_EMPTY_FIELDS_ERROR_MESSAGE = "Business Address Grid has one or more Empty fields<br />";
//        private const string CONTACT_PERSON_INVALID_DATA_ERROR_MESSAGE = "Invalid field value for Contact Person Grid<br />";
//        private const string CONTACT_PERSON_INVALID_EMAIL_ERROR_MESSAGE = "Contact Person Grid has invalid email<br />";
//        private const string CONTACT_PERSON_EMPTY_FIELDS_ERROR_MESSAGE = "Contact Person Grid has one or more Empty fields<br />";        
//        private const string CUSTOMER_EMAIL_EMPTY_FIELDS_ERROR_MESSAGE = "Email Grid has one or more Empty fields<br />";
//        private const string CUSTOMER_EMAIL_INVALID_EMAIL_ERROR_MESSAGE = "Email Grid has invalid email<br />";
//        private const string CUSTOMER_EMAIL_LENGTH_ERROR_MESSAGE = "Email Grid has an email which is too long<br />";
//        private const string CUSTOMER_PHONE_EMPTY_FIELDS_ERROR_MESSAGE = "Contact Phone Grid has one or more Empty fields<br />";
//        private const string CUSTOMER_PHONE_INVALID_EMAIL_ERROR_MESSAGE = "Contact Phone Grid has invalid phone number<br />";
//        private const string CUSTOMER_PHONE_LENGTH_ERROR_MESSAGE = "Contact Phone Grid has a value that exceed phone number length<br />";
//        private const string CUSTOMER_SHIPPING_ADDRESS_EMPTY_FIELDS_ERROR_MESSAGE = "Shipping Address Grid has one or more Empty fields<br />";
//        private const string CUSTOMER_SHIPPING_ADDRESS_INVALID_DATA_ERROR_MESSAGE = "Invalid Shipping method/City name forShipping Address Grid has invalid phone number<br />";
//        private const string CUSTOMER_SHIPPING_ADDRESS_LENGTH_ERROR_MESSAGE = "Shipping Address Grid has a value, whose length is too long<br />";

        
//        private const string A_CUSTOMER_GRID_WITHOUT_DATA_ERROR_MESSAGE = "One or More Grid(s) are Empty<br />";

//        private string errorMessage = "";
//        private string nextCustomerNumber;


//        // db entities
//        private ICommerceEntities le;
//        private Icore_dbEntities ctx;


//        public CustomerController()
//        {
//            le = new CommerceEntities();
//            ctx = new core_dbEntities();

//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//            ctx.Configuration.LazyLoadingEnabled = false;
//            ctx.Configuration.ProxyCreationEnabled = false;
//        }

//        public CustomerController(ICommerceEntities lent, Icore_dbEntities ent)
//        {
//            le = lent;
//            ctx = ent;
//        }


//        //Get all customers with details
//        public async Task<IEnumerable<customer>> Get()
//        {
//            return await le.customers
//                .Include(p => p.customerBusinessAddresses)
//                .Include(p => p.customerContactPersons)
//                .Include(p => p.customerEmails)
//                .Include(p => p.customerPhones)
//                .Include(p => p.customerShippingAddresses)
//                .OrderBy(p => p.customerNumber)
//                .ToListAsync();
//        }


//        //Get a customer with details
//        [HttpGet]
//        public customer Get(int Id)
//        {
//            customer value = le.customers
//                .Include(p => p.customerBusinessAddresses)
//                .Include(p => p.customerContactPersons)
//                .Include(p => p.customerEmails)
//                .Include(p => p.customerPhones)
//                .Include(p => p.customerShippingAddresses)
//                .FirstOrDefault(p => p.customerId == Id);

//            if (value == null)
//            {
//                value = new customer();
//            }
//            return value;
//        }

//        //Get a customer with details
//        [HttpGet]
//        public async Task<IEnumerable<customer>> GetCashCustomer()
//        {
//            return await le.customers
//                .Where(p => p.customerName == "CASH CUSTOMER")
//                .ToListAsync();
//        }

//        [HttpGet]
//        public async Task<IEnumerable<customer>> GetNonCashCustomer()
//        {
//            return await le.customers
//                .Where(p => p.customerName != "CASH CUSTOMER")
//                .ToListAsync();
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody] KendoRequest req)
//        {
//            string order = "customerNumber";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.customers.AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }

//        //Create and Update a cutomer
//        [HttpPost]
//        public customer Post(customer value)
//        {
//            customer toBeSaved = null;

//            //If form validation fails, throw exception
//            if (ValidateCustomerFields(value) == false)
//            {
//                throw new ApplicationException(errorMessage);
//            }

//            //If form validation pass Start saving process
//            if (ValidateCustomerFields(value))
//            {

//            // If customerId is greater than zero its an Update, so fetch the  existing cutomer & update record
//            if (value.customerId > 0)
//            {
//                toBeSaved = le.customers
//                    .Include(p => p.customerBusinessAddresses)
//                    .Include(p => p.customerContactPersons)
//                    .Include(p => p.customerEmails)
//                    .Include(p => p.customerPhones)
//                    .Include(p => p.customerShippingAddresses)
//                    .First(p => p.customerId == value.customerId);
//                populateFields(toBeSaved, value);

//            }
//            else //If customerId is less tan one, its a new record so Create a new cutomer assign vales to it
//            {
//                toBeSaved = new customer();
//                populateFields(toBeSaved, value);
//                le.customers.Add(toBeSaved);
//            }


            
//            foreach (var cusBusAddrs in value.customerBusinessAddresses)
//            {
//                if (ValidateBusinessAddress(cusBusAddrs) == false)
//                {
//                    throw new ApplicationException(errorMessage);
//                }
//                if (ValidateBusinessAddress(cusBusAddrs))
//                {
//                customerBusinessAddress cusBusAddToBeSaved = null;

//                //Update existing customer business address
//                if (cusBusAddrs.customerBusinessAddressId > 0)
//                {
//                    cusBusAddToBeSaved = toBeSaved.customerBusinessAddresses
//                        .First(p => p.customerBusinessAddressId == cusBusAddrs.customerBusinessAddressId);
//                    populateCustomerBusinessAddress(cusBusAddToBeSaved, cusBusAddrs);
//                }
//                else //Create a new cutomer business address
//                {
//                    cusBusAddToBeSaved = new customerBusinessAddress();
//                    populateCustomerBusinessAddress(cusBusAddToBeSaved, cusBusAddrs);
//                    toBeSaved.customerBusinessAddresses.Add(cusBusAddToBeSaved);
//                }
//                }
//            }

//            foreach (var cusContPers in value.customerContactPersons)
//            {
//                if (ValidateContactPerson(cusContPers) == false)
//                {
//                    throw new ApplicationException(errorMessage);
//                }
//                if (ValidateContactPerson(cusContPers))
//                {
//                customerContactPerson cusContPersToBeSaved = null;

//                //Update existing customer Contact Person
//                if (cusContPers.customerContactPersonId > 0)
//                {
//                    cusContPersToBeSaved = toBeSaved.customerContactPersons
//                        .First(p => p.customerContactPersonId == cusContPers.customerContactPersonId);
//                    populateCustomerContactPerson(cusContPersToBeSaved, cusContPers);
//                }
//                else //Create a new cutomer Contact Person
//                {
//                    cusContPersToBeSaved = new customerContactPerson();
//                    populateCustomerContactPerson(cusContPersToBeSaved, cusContPers);
//                    toBeSaved.customerContactPersons.Add(cusContPersToBeSaved);
//                }
//                }
//            }

//            foreach (var cusEmail in value.customerEmails)
//            {
//                if (ValidateCustomerEmail(cusEmail) == false)
//                {
//                    throw new ApplicationException(errorMessage);
//                }
//                if (ValidateCustomerEmail(cusEmail))
//                {
//                customerEmail cusEmailToBeSaved = null;

//                //Update existing customer Email
//                if (cusEmail.customerEmailId > 0)
//                {
//                    cusEmailToBeSaved = toBeSaved.customerEmails
//                        .First(p => p.customerEmailId == cusEmail.customerEmailId);
//                    populateCustomerEmail(cusEmailToBeSaved, cusEmail);
//                }
//                else //Create a new cutomer Email
//                {
//                    cusEmailToBeSaved = new customerEmail();
//                    populateCustomerEmail(cusEmailToBeSaved, cusEmail);
//                    toBeSaved.customerEmails.Add(cusEmailToBeSaved);
//                }
//                }
//            }

//            foreach (var cusPhone in value.customerPhones)
//            {
//                if (ValidateCustomerPhone(cusPhone) == false)
//                {
//                    throw new ApplicationException(errorMessage);
//                }
//                if (ValidateCustomerPhone(cusPhone))
//                {
//                customerPhone cusPhoneToBeSaved = null;

//                //Update existing customer Phone
//                if (cusPhone.customerPhoneId > 0)
//                {
//                    cusPhoneToBeSaved = toBeSaved.customerPhones
//                        .First(p => p.customerPhoneId == cusPhone.customerPhoneId);
//                    populateCustomerPhone(cusPhoneToBeSaved, cusPhone);
//                }
//                else //Create a new cutomer Phone
//                {
//                    cusPhoneToBeSaved = new customerPhone();
//                    populateCustomerPhone(cusPhoneToBeSaved, cusPhone);
//                    toBeSaved.customerPhones.Add(cusPhoneToBeSaved);
//                }
//                }
//            }

//            foreach (var cusShippAddrs in value.customerShippingAddresses)
//            {
//                if (ValidateCustomerShippingAddress(cusShippAddrs) == false)
//                {
//                    throw new ApplicationException(errorMessage);
//                }
//                if (ValidateCustomerShippingAddress(cusShippAddrs))
//                {
//                customerShippingAddress cusShippAddToBeSaved = null;

//                //Update existing customer Shipping Address
//                if (cusShippAddrs.customerShippingAddressId > 0)
//                {
//                    cusShippAddToBeSaved = toBeSaved.customerShippingAddresses
//                        .First(p => p.customerShippingAddressId == cusShippAddrs.customerShippingAddressId);
//                    populateCustomerShippAddrs(cusShippAddToBeSaved, cusShippAddrs);
//                }
//                else //Create a new cutomer Shipping Address
//                {
//                    cusShippAddToBeSaved = new customerShippingAddress();
//                    populateCustomerShippAddrs(cusShippAddToBeSaved, cusShippAddrs);
//                    toBeSaved.customerShippingAddresses.Add(cusShippAddToBeSaved);
//                }
//                }
//            }

//            }
//            le.SaveChanges();
//            return toBeSaved;
//        }


//        //populate Customer
//        private void populateFields(customer toBeSaved, customer value)
//        {
//            if (value.customerId < 1)
//            {
//                toBeSaved.customerNumber = generateCustomerNumber();
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.customerName = value.customerName;
//            toBeSaved.paymentTermID = value.paymentTermID;
//            toBeSaved.currencyId = value.currencyId;
//            toBeSaved.balance = 0.00;
//            toBeSaved.balanceLocal = 0.00;
//            toBeSaved.contactPersonName = "";
//            toBeSaved.glAccountId = value.glAccountId;
//            toBeSaved.locationId = value.locationId;
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate CustomerBusinessAddress
//        private void populateCustomerBusinessAddress(customerBusinessAddress toBeSaved, customerBusinessAddress value)
//        {
//            toBeSaved.addressTypeId = value.addressTypeId;
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.addressLine = value.addressLine;
//            toBeSaved.landmark = value.landmark;
//            toBeSaved.cityName = value.cityName;
//            toBeSaved.countryName = value.countryName;
//            if (value.customerBusinessAddressId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate CustomerContactPerson
//        private void populateCustomerContactPerson(customerContactPerson toBeSaved, customerContactPerson value)
//        {
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.contactPersonName = value.contactPersonName;
//            toBeSaved.jobTitle = value.jobTitle;
//            toBeSaved.mobilePhoneNumber = value.mobilePhoneNumber;
//            toBeSaved.landlinePhoneNumber = value.landlinePhoneNumber;
//            toBeSaved.officeExtension = value.officeExtension == "" ? " " : value.officeExtension;
//            toBeSaved.emailAddress = value.emailAddress;
//            toBeSaved.skypeId = value.skypeId == "" ? " " : value.skypeId;
//            if (value.customerContactPersonId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate CustomerCustomerEmail
//        private void populateCustomerEmail(customerEmail toBeSaved, customerEmail value)
//        {
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.emailAddress1 = value.emailAddress1;
//            toBeSaved.emailAddress2 = value.emailAddress2 == "" ? " " : value.emailAddress2;
//            toBeSaved.emailAddress3 = value.emailAddress3 == "" ? " " : value.emailAddress3;
//            if (value.customerEmailId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate CustomerCustomerPhone
//        private void populateCustomerPhone(customerPhone toBeSaved, customerPhone value)
//        {
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.mobilePhoneNumber = value.mobilePhoneNumber;
//            toBeSaved.landlinePhoneNumber = value.landlinePhoneNumber;
//            toBeSaved.faxNumber = value.faxNumber == "" ? " " : value.faxNumber;
//            if (value.customerPhoneId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate CustomerCustomerShippingAddress
//        private void populateCustomerShippAddrs(customerShippingAddress toBeSaved, customerShippingAddress value)
//        {
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.shippingMethodId = value.shippingMethodId;
//            toBeSaved.shipTo = value.shipTo;
//            toBeSaved.addressLine1 = value.addressLine1;
//            toBeSaved.addressLine2 = value.addressLine2 == "" ? " " : value.addressLine2;
//            toBeSaved.cityName = value.cityName;
//            toBeSaved.countryName = value.countryName;
//            if (value.customerShippingAddressId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }


//        [HttpDelete]
//        // DELETE: api/customer/
//        public void Delete([FromBody] customer value)
//        {
//            var forDelete = le.customers
//                .Include(p => p.customerBusinessAddresses)
//                .Include(p => p.customerContactPersons)
//                .Include(p => p.customerEmails)
//                .Include(p => p.customerPhones)
//                .Include(p => p.customerShippingAddresses)
//                .FirstOrDefault(p => p.customerId == value.customerId);

//            if (forDelete != null)
//            {
//                foreach (var cusBusAddrs in value.customerBusinessAddresses)
//                {
//                    if (
//                        value.customerBusinessAddresses.Any(
//                            p => p.customerBusinessAddressId == cusBusAddrs.customerBusinessAddressId))
//                    {
//                        forDelete.customerBusinessAddresses.Remove(cusBusAddrs);
//                    }
//                }

//                foreach (var cusContPers in value.customerContactPersons)
//                {
//                    if (
//                        value.customerContactPersons.Any(
//                            p => p.customerContactPersonId == cusContPers.customerContactPersonId))
//                    {
//                        forDelete.customerContactPersons.Remove(cusContPers);
//                    }
//                }

//                foreach (var cusEmail in value.customerEmails)
//                {
//                    if (value.customerEmails.Any(p => p.customerEmailId == cusEmail.customerEmailId))
//                    {
//                        forDelete.customerEmails.Remove(cusEmail);
//                    }
//                }

//                foreach (var cusPhone in value.customerPhones)
//                {
//                    if (value.customerPhones.Any(p => p.customerPhoneId == cusPhone.customerPhoneId))
//                    {
//                        forDelete.customerPhones.Remove(cusPhone);
//                    }
//                }

//                foreach (var cusShippAddrs in value.customerShippingAddresses)
//                {
//                    if (
//                        value.customerShippingAddresses.Any(
//                            p => p.customerShippingAddressId == cusShippAddrs.customerShippingAddressId))
//                    {
//                        forDelete.customerShippingAddresses.Remove(cusShippAddrs);
//                    }
//                }

//                le.customers.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }

//        private string generateCustomerNumber()
//        {
//            nextCustomerNumber = !le.customers.Any()
//                ? "CUS000001"
//                : // if it's empty, start with CUS000001
//                "CUS" +
//                (int.Parse(
//                    le.customers.OrderByDescending(i => i.customerNumber) // order by code descending
//                    .First() // get first one (last code)
//                    .customerNumber.Split('S')[1]) // get only the number part
//                + 1).ToString("000000"); // add 1 and format with 6 digits

//            return nextCustomerNumber;
//        }




//        //valiadtions
//        private bool ValidateCustomerFields(customer cust)
//        {
//            //If Shrinkage Grid is empty, Catch the error and return false
//            if (cust.customerBusinessAddresses.Any() && cust.customerContactPersons.Any()
//                    && cust.customerEmails.Any() && cust.customerPhones.Any()
//                    && cust.customerShippingAddresses.Any())
//            {
//                ValidateCustomerDropDowns(cust);
//                ValidateCustomerEmptyFields(cust);

//                //If errorMessage is empty test Pass
//                if (errorMessage == "")
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            else
//            {
//                errorMessage += A_CUSTOMER_GRID_WITHOUT_DATA_ERROR_MESSAGE;
//                return false;
//            }
//        }


//        private bool ValidateBusinessAddress(customerBusinessAddress cusBusAdd)
//        {
//            ValidateBusinessAddressDropDowns(cusBusAdd);
//            ValidateBusinessAddressEmptyFields(cusBusAdd);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool ValidateContactPerson(customerContactPerson cusContPers)
//        {
//            ValidateContactPersonInvalidValues(cusContPers);
//            ValidateContactPersonEmail(cusContPers);
//            ValidateContactPersonEmptyFields(cusContPers);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool ValidateCustomerEmail(customerEmail cusEmail)
//        {
//            ValidateCustomerEmailFields(cusEmail);
//            ValidateCustomerEmailEmptyFields(cusEmail);
//            ValidateCustomerEmailLength(cusEmail);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool ValidateCustomerPhone(customerPhone cusPhone)
//        {
//            ValidateCustomerPhoneFields(cusPhone);
//            ValidateCustomerPhoneEmptyFields(cusPhone);
//            ValidateCustomerPhoneFieldsLength(cusPhone);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool ValidateCustomerShippingAddress(customerShippingAddress cusShippAddrs)
//        {
//            ValidateCustomerShippingAddressFields(cusShippAddrs);
//            ValidateCustomerShippingAddressEmptyFields(cusShippAddrs);
//            ValidateCustomerShippingAddressFieldsLength(cusShippAddrs);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private void ValidateCustomerDropDowns(customer cust)
//        {
//            if (currencyExists(cust.currencyId) == false || cityExists(cust.locationId) == false
//                || accountExists(cust.glAccountId) == false)
//            {
//                errorMessage += CUSTOMER_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerEmptyFields(customer cust)
//        {
//            if (String.IsNullOrEmpty(cust.customerName) || cust.paymentTermID < 1 || cust.currencyId < 1
//                || cust.glAccountId < 1 || cust.locationId < 1)
//            {
//                errorMessage += CUSTOMER_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private bool currencyExists(int? currId)
//        {
//            if (ctx.currencies.Any(p => p.currency_id == currId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool cityExists(int? cityId)
//        {
//            if (ctx.cities.Any(p => p.city_id == cityId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool accountExists(int? glAccId)
//        {
//            if (ctx.accts.Any(p => p.acct_id == glAccId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private void ValidateBusinessAddressDropDowns(customerBusinessAddress cusBusAdd)
//        {
//            if (countryExists(cusBusAdd.countryName) == false || cusBusAdd.addressTypeId < 1
//                || cityNameExists(cusBusAdd.cityName) == false)
//            {
//                errorMessage += BUSINESS_ADDRESS_DROP_DOWN_N_AUTO_COMPLETE_ERROR_MESSAGE;
//            }
//        }

//        private bool countryExists(string country)
//        {
//                if (ctx.countries.Any(p => p.country_name  == country))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//       }

//        private bool cityNameExists(string cityName)
//        {
//            if (ctx.cities.Any(p => p.city_name == cityName))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }




//        private void ValidateBusinessAddressEmptyFields(customerBusinessAddress cusBusAdd)
//        {
//            if ((cusBusAdd.addressTypeId < 1) || String.IsNullOrEmpty(cusBusAdd.addressLine)
//                || String.IsNullOrEmpty(cusBusAdd.landmark) || String.IsNullOrEmpty(cusBusAdd.cityName)
//                || String.IsNullOrEmpty(cusBusAdd.countryName))
//            {
//                errorMessage += BUSINESS_ADDRESS_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateContactPersonInvalidValues(customerContactPerson cusContPers)
//        {
//            var offficeExt = cusContPers.officeExtension == "" ? "0" : cusContPers.officeExtension;
//            if ((!Regex.IsMatch(cusContPers.contactPersonName, "^[a-zA-Z\\s]+$"))
//                || (!Regex.IsMatch(cusContPers.jobTitle, "^[a-zA-Z\\s]+$"))
//                || (!Regex.IsMatch(cusContPers.mobilePhoneNumber, "^[0-9]+$"))
//                || (!Regex.IsMatch(cusContPers.landlinePhoneNumber, "^[0-9]+$"))
//                || (!Regex.IsMatch(offficeExt, "^[0-9]+$"))
//                )

//            {
//                errorMessage += CONTACT_PERSON_INVALID_DATA_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateContactPersonEmail(customerContactPerson cusContPers)
//        {
//            if (!IsValidEmail(cusContPers.emailAddress))

//            {
//                errorMessage += CONTACT_PERSON_INVALID_EMAIL_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateContactPersonEmptyFields(customerContactPerson cusContPers)
//        {
//            if (String.IsNullOrEmpty(cusContPers.contactPersonName) || String.IsNullOrEmpty(cusContPers.jobTitle)
//                || String.IsNullOrEmpty(cusContPers.mobilePhoneNumber) || String.IsNullOrEmpty(cusContPers.landlinePhoneNumber)
//                || String.IsNullOrEmpty(cusContPers.emailAddress))
//            {
//                errorMessage += CONTACT_PERSON_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }


//        private void ValidateCustomerEmailFields(customerEmail cusEmail)
//        {
//            var email2 = cusEmail.emailAddress2 == "" ? "kk@yahoo.com" : cusEmail.emailAddress2;
//            var email3 = cusEmail.emailAddress3 == "" ? "kk@yahoo.com" : cusEmail.emailAddress3;

//            if (!IsValidEmail(cusEmail.emailAddress1) || !IsValidEmail(email2)
//                || !IsValidEmail(email3))

//            {
//                errorMessage += CUSTOMER_EMAIL_INVALID_EMAIL_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerEmailEmptyFields(customerEmail cusEmail)
//        {
//            if (String.IsNullOrEmpty(cusEmail.emailAddress1))
//            {
//                errorMessage += CUSTOMER_EMAIL_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerEmailLength(customerEmail cusEmail)
//        {
//            if (cusEmail.emailAddress1.Length > 49 || cusEmail.emailAddress2.Length > 49
//                || cusEmail.emailAddress3.Length > 49 )
//            {
//                errorMessage += CUSTOMER_EMAIL_LENGTH_ERROR_MESSAGE;
//            }
//        }

//        private bool IsValidEmail(string email)
//        {
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        private void ValidateCustomerPhoneFields(customerPhone cusPhone)
//        {
//            var faxNum = cusPhone.faxNumber == "" ? "0" : cusPhone.faxNumber;
//            long num;
//            if (!long.TryParse(cusPhone.mobilePhoneNumber, out num) || !long.TryParse(cusPhone.landlinePhoneNumber, out num)
//                || !long.TryParse(faxNum, out num))

//            {
//                errorMessage += CUSTOMER_PHONE_INVALID_EMAIL_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerPhoneEmptyFields(customerPhone cusPhone)
//        {
//            if (String.IsNullOrEmpty(cusPhone.mobilePhoneNumber) || String.IsNullOrEmpty(cusPhone.landlinePhoneNumber))
//            {
//                errorMessage += CUSTOMER_PHONE_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerPhoneFieldsLength(customerPhone cusPhone)
//        {
//            if (cusPhone.mobilePhoneNumber.Length > 14 || cusPhone.landlinePhoneNumber.Length > 14
//                || cusPhone.faxNumber.Length > 14)
//            {
//                errorMessage += CUSTOMER_PHONE_LENGTH_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerShippingAddressFields(customerShippingAddress cusShippAddrs)
//        {
//            if(shippingMethodExists(cusShippAddrs.shippingMethodId) == false || cityNameExists(cusShippAddrs.cityName) == false
//                || countryExists(cusShippAddrs.countryName) == false)
//            {
//                errorMessage += CUSTOMER_SHIPPING_ADDRESS_INVALID_DATA_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerShippingAddressEmptyFields(customerShippingAddress cusShippAddrs)
//        {
//            if (cusShippAddrs.shippingMethodId < 1 || String.IsNullOrEmpty(cusShippAddrs.shipTo)
//                || String.IsNullOrEmpty(cusShippAddrs.addressLine1) || String.IsNullOrEmpty(cusShippAddrs.cityName))
//            {
//                errorMessage += CUSTOMER_SHIPPING_ADDRESS_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateCustomerShippingAddressFieldsLength(customerShippingAddress cusShippAddrs)
//        {
//            if (cusShippAddrs.shipTo.Length > 399 || cusShippAddrs.addressLine1.Length > 399
//                || cusShippAddrs.addressLine2.Length > 399 || cusShippAddrs.cityName.Length > 100)
//            {
//                errorMessage += CUSTOMER_SHIPPING_ADDRESS_LENGTH_ERROR_MESSAGE;
//            }
//        }



//        private bool shippingMethodExists(int id)
//        {
//            if (le.shippingMethods.Any(p => p.shippingMethodID == id))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }


            
//    }
//}


