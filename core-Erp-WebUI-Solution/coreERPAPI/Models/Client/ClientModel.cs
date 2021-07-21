using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreERP.Models.Image;
using coreLogic;

namespace coreERP.Models.Client
{
    public class ClientModel
    {
        public int clientId { get; set; }
        public string accountNumber { get; set; }
        public string surName { get; set; }
        public string otherNames { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string sex { get; set; }
        public int? maritalStatusId { get; set; }
        public int? industryId { get; set; }
        public int? branchId { get; set; }
        public int? sectorId { get; set; }
        public int clientTypeId { get; set; }
        public int? clientCategoryId { get; set; }
        public int primaryIdTypeId { get; set; }
        public string primaryIdNo { get; set; }
        public DateTime? primaryIdExpiry { get; set; }
        public int? secondaryIdTypeId { get; set; }
        public string secondaryIdNo { get; set; }
        public DateTime? secondaryIdExpiry { get; set; }
        public ClientImageViewModel clientImage { get; set; }
        public ClientResidentialAddress clientAddress { get; set; }
        public ClientMailingAddress mailingAddress { get; set; }
        public List<ClientPhoneModel> clientPhones { get; set; }
        public List<ClientEmailModel> clientEmails { get; set; }
        public List<SupportingDocumentModel> supportingDocuments { get; set; }
        public ClientNextOfKin nextOfKin { get; set; }

        public string imageDataString { get; set; }
        public int? loanGroupId { get; set; }

    }

     public class ClientNextOfKin
    {
        public string emailAddress { get; set; }

        public string idNumber { get; set; }
        public int idNoTypeID { get; set; }
        public string imageDataString { get; set; }

        public string phoneNumber { get; set; }

        public string otherNames { get; set; }
        public string relationship { get; set; }
        public string surName { get; set; }
    }


    public class ClientResidentialAddress
    {
        public int addressId { get; set; }
        public string addressLine { get; set; }
        public string landMark { get; set; }
        public string city { get; set; }
        public ClientResidentialAddressImageViewModel addressImage { get; set; }
    }

    public class ClientMailingAddress
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public int city { get; set; }
    }

    public class ClientPhoneModel
    {
        public int clientId { get; set; }
        public int phoneId { get; set; }
        public int phoneTypeId { get; set; }
        public string phoneNumber { get; set; }
    }

    public class ClientEmailModel
    {
        public int clientId { get; set; }
        public int emailId { get; set; }
        public int emailTypeId { get; set; }
        public string emailAddress { get; set; }
    }

    public class SupportingDocumentModel 
    {
        public int documentId { get; set; }
        public int clientId { get; set; }
        public string description { get; set; }
        public string document { get; set; }
        public string fileName { get; set; }
        public string mimeType { get; set; }
    }
}