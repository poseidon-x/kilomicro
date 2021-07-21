//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http; 
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using coreERP;
//using coreLogic;
//using System.IO;
//using System.Net.Http.Headers;
//using System.Web;
//using coreERP.Providers;
//using System.Web.Http;
//using System.Web.Http.Cors;
//using System.Data.Entity;
//using System.Text;
//using coreData.Constants;
//using coreData.ErrorLog;
//using coreErpApi.Controllers.Models;


//namespace coreERP.Controllers.Loans.PrivateCompanyStaff
//{
//    [AuthorizationFilter()]
//    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
//    public class PrivateCompanyStaffController : ApiController
//    {
//        private IcoreLoansEntities le;

//        public PrivateCompanyStaffController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false; 
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public PrivateCompanyStaffController(IcoreLoansEntities ent)
//        {
//            le = ent;
//        }


//        [HttpGet]
//        public privateCompanyStaff Get(int id)
//        {
//            var data = le.privateCompanyStaffs
//                .Include(p => p.privateCompanyStaffAddresses)
//                .Include(p => p.privateCompanyStaffVerifications)
//                .FirstOrDefault(p => p.privateCompanyStaffId == id);

//            if (data == null)
//            {
//                data = new privateCompanyStaff
//                {
//                    privateCompanyStaffAddresses = new List<privateCompanyStaffAddress>(),
//                    privateCompanyStaffVerifications = new List<privateCompanyStaffVerification>()
//                };
//            }
//            return data;
//        }

//        [HttpPost]
//        public privateCompanyStaff Post(privateCompanyStaff input)
//        {
//            //validate
//            validateInput(input);

//            if (input.privateCompanyStaffId > 0)
//            {
//                var staffToBeUpdated = le.privateCompanyStaffs
//                    .FirstOrDefault(p => p.privateCompanyStaffId == input.privateCompanyStaffId);
//                if (staffToBeUpdated != null)
//                {
//                    populateStaffDetail(staffToBeUpdated, input);
//                }            
//            }
//            else
//            {
//                var staffToBesaved = new privateCompanyStaff();
//                populateStaffDetail(staffToBesaved, input);
//                le.privateCompanyStaffs.Add(staffToBesaved);
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                Logger.logError(ex);
//                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
//            }

//            return input;
//        }

//        private void populateStaffDetail(privateCompanyStaff toBesaved, privateCompanyStaff input)
//        {
//            toBesaved.employerId = input.employerId;
//            toBesaved.employeeNumber = input.employeeNumber;
//            toBesaved.clientId = input.clientId;
//            toBesaved.employeeContractTypeId = input.employeeContractTypeId;
//            toBesaved.employmentStartDate = input.employmentStartDate;
//            toBesaved.socialSecurityNumber = input.socialSecurityNumber;
//            toBesaved.position = input.position;
//            if (toBesaved.privateCompanyStaffId < 1)
//            {
//                toBesaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
//                toBesaved.created = DateTime.Now;
//            }
//            else
//            {
//                if(toBesaved != input)
//                toBesaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
//                toBesaved.modified = DateTime.Now;
//            }

//            if(input.privateCompanyStaffAddresses.Any())
//                foreach (var address in input.privateCompanyStaffAddresses)
//                {
//                    if (address.privateCompanyStaffAddressId < 1)
//                    {
//                        var addToBeSaved = new privateCompanyStaffAddress();
//                        populateStaffAddress(addToBeSaved, address);
//                        toBesaved.privateCompanyStaffAddresses.Add(addToBeSaved);
//                    }
//                    else
//                    {
//                        var addToBeUpdated = toBesaved.privateCompanyStaffAddresses
//                            .FirstOrDefault(p => p.privateCompanyStaffAddressId == address.privateCompanyStaffAddressId);
//                        if (addToBeUpdated != null && (addToBeUpdated != address))
//                        {
//                            populateStaffAddress(addToBeUpdated, address);
//                        }
//                    }
//                }

//            if (input.privateCompanyStaffVerifications.Any())
//                foreach (var verification in input.privateCompanyStaffVerifications)
//                {
//                    if (verification.privateCompanyStaffVerificationId < 1)
//                    {
//                        var verificToBeSaved = new privateCompanyStaffVerification();
//                        populateStaffVerification(verificToBeSaved, verification);
//                        toBesaved.privateCompanyStaffVerifications.Add(verificToBeSaved);
//                    }
//                    else
//                    {
//                        var verificToBeUpdated = toBesaved.privateCompanyStaffVerifications
//                            .FirstOrDefault(p => p.privateCompanyStaffVerificationId == verification.privateCompanyStaffVerificationId);
//                        if (verificToBeUpdated != null && (verificToBeUpdated != verification))
//                        {
//                            populateStaffVerification(verificToBeUpdated, verification);
//                        }
//                    }
//                }

//        }

//        private void populateStaffAddress(privateCompanyStaffAddress toBesaved, privateCompanyStaffAddress input)
//        {
//            toBesaved.addressTypeId = input.addressTypeId;
//            toBesaved.cityId = input.cityId;
//            toBesaved.addressLine = input.addressLine;
//            if (input.privateCompanyStaffAddressId < 1)
//            {
//                toBesaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBesaved.created = DateTime.Now;
//            }
//            else
//            {
//                toBesaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBesaved.modified = DateTime.Now;
//            }
//        }

//        private void populateStaffVerification(privateCompanyStaffVerification toBesaved, privateCompanyStaffVerification input)
//        {
//            toBesaved.contactPersonName = input.contactPersonName;
//            toBesaved.contactPersonPosition = input.contactPersonPosition;
//            toBesaved.departmentId = input.departmentId;
//            toBesaved.email = input.email;
//            toBesaved.phone = input.phone;

//            if (input.privateCompanyStaffVerificationId < 1)
//            {
//                toBesaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBesaved.created = DateTime.Now;
//            }
//            else
//            {
//                toBesaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBesaved.modified = DateTime.Now;
//            }
//        }


//        private void validateInput(privateCompanyStaff input)
//        {
//            if (!EmpoyerExist(input.employerId) || !ClientExist(input.clientId) 
//                || !EmployeeContractTypeExist(input.employeeContractTypeId) 
//                || (input.employmentStartDate != null && input.employmentStartDate.Value.Date > DateTime.Today)
//                || (input.employmentStartDate != null && input.employmentStartDate.Value.Date == DateTime.MinValue)
//                //|| (input.position != null && (string.IsNullOrEmpty(input.position) || string.IsNullOrWhiteSpace(input.position)))
//                )
//            {
//                StringBuilder error = new StringBuilder();
//                if (!EmpoyerExist(input.employerId))
//                    error.Append("Invalid Emploer selected");
//                if (!ClientExist(input.clientId))
//                    error.Append("Invalid Client selected");
//                if (!EmployeeContractTypeExist(input.employeeContractTypeId))
//                    error.Append("Invalid employee contract type selected");
//                if (input.employmentStartDate != null && input.employmentStartDate.Value.Date > DateTime.Today)
//                    error.Append("Employeement start date cannot be a future date");
//                if (input.employmentStartDate != null && input.employmentStartDate.Value.Date == DateTime.MinValue)
//                    error.Append(" Invalid employeement start date");
//                throw new Exception(error.ToString());
//            }
//        }

//        private bool EmpoyerExist(int employerId)
//        {
//            if (le.employers.Any(p => p.employerID == employerId))
//            {
//                return true;
//            }
//            return false;
//        }

//        private bool ClientExist(int clientId)
//        {
//            if (le.clients.Any(p => p.clientID == clientId))
//            {
//                return true;
//            }
//            return false;
//        }

//        private bool EmployeeContractTypeExist(int contractTypeId)
//        {
//            if (le.employeeContractTypes.Any(p => p.employeeContractTypeID == contractTypeId))
//            {
//                return true;
//            }
//            return false;
//        }
//    }


        
//}
