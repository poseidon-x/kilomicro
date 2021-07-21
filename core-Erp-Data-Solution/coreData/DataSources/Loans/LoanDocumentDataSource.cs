using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Loans;
using coreLogic.HelperClasses;

namespace coreData.DataSources.Loans
{
    [DataObject]
    public class LoanDocumentDataSource
    {
        private coreLoansEntities le = new coreLoansEntities();
        private core_dbEntities ctx = new core_dbEntities();

        private LoanDocumentTemplate document;

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<LoanDocumentPageViewModel> GetLoanDocument(int loanId, int documentId)
        {
            document = new LoanDocumentTemplate(loanId, documentId);
            loan ln = document.getLoan();

            var returnData = new LoanDocumentViewModel
            {
                companyLogo = ctx.comp_prof.First().logo,
                date = DateTime.Now.ToString("dd-MMM-yyyy"),
                loanId = ln.loanID,
                loanNumber = ln.loanNo,
                clientId = ln.clientID,
                loanAmountInFigures = ln.amountDisbursed.ToString("N0"),
                loanAmountInWords = NumberToWordsConverter.NumberToWords((int)ln.amountDisbursed).ToUpper(),
            };
            RetrieveAdditionalData(returnData, documentId);

            //Retrieve all additionalInformation on Loan
            var loanMetaData = ln.loanAdditionalInfoes
                            .First()
                            .loanMetaDatas
                            .Join(le.metaDataTypes, lai => lai.metaDataTypeId,
                            mdt => mdt.metaDataTypeId, (lai,mdt) => new LoanMetaDataViewModel
                            {
                                id = lai.metaDataTypeId,
                                value = lai.content,
                                code = mdt.nameCode
                            }).OrderBy( p => p.code)
                            .ToList();

            //Use the returnData's data type name to match metaDataCode and set values of returnData 
            Type t = returnData.GetType();
            var properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var name = prop.Name;
                foreach (var data in loanMetaData)
                {
                    if (data.code == name)
                    {
                        prop.SetValue(returnData,data.value,null);
                    }
                }
            }

            //Use returnData's data type name to match placeHolders and replace on pages
            foreach (var page in returnData.pages)
            {
                int length = page.placeHolders.Count();
                var PHs = page.placeHolders;

                for (int i = 0; i < length; i++)
                {                    
                    foreach (var prop in properties)
                    {
                        if (prop != null) { 
                            var value = prop.GetValue(returnData, null).ToString();
                            if ((PHs[i].Equals(prop.Name)) && value != null)
                            {
                                var stringToReplace = "$$" + prop.Name + "$$";
                                page.content = page.content.Replace(stringToReplace, " "+value);
                            }
                        }
                    }
                }
            }

            return returnData.pages;
        }

        private void RetrieveAdditionalData(LoanDocumentViewModel returnData,int  documentId )
        {
            returnData.templateName = document.getTemplateName();
            returnData.clientName = document.getClientName();
            returnData.clientLocation = document.getClientLocation();
            returnData.lendingCompanyName = document.getCompanyName();
            returnData.lendingCompanyName = document.getCompanyName();
            returnData.lendingCompanyPostalAddress = document.getCompanyPostalAddress();
            returnData.lendingCompanyOfficeAddress = document.getCompanyHeadOfficeLocationAddress();
            returnData.lendingCompanyRegion = document.getCompanyRegion();
            returnData.lendingCompanyCeo = document.getCompanyCeoName();
            returnData.lendingCompanyOperManager = document.getCompanyOperManagerName();
            returnData.loanProduct = document.getLoanProduct();
            returnData.loanTenure = document.getLoanTenure();
            returnData.repaymentMode = document.getLoanRepaymentMode();
            returnData.interestRate = document.getLoanInterestRate();
            returnData.applicationFee = document.getLoanApplicationFee();
            returnData.processingFee = document.getLoanProcessingFee();
            returnData.insuranceCharges = document.getLoanInsuranceCharges();
            returnData.loanMaturitySum = document.getLoanMaturitySum();
            returnData.maturityDate = document.getLoanmaturityDate();
            returnData.pages = document.GetPages(documentId);
            returnData.guarantorName = document.getLoanGuarantorName();
            returnData.guarantorAddress = document.getLoanGuarantorAddress();
            returnData.guarantorWitness = document.getLoanGuarantorWitness();
            returnData.collateralValueInFigures = document.getLoanCollateralValue();
            returnData.loancollateral = document.getLoanCollateral();
        }


    }
}
