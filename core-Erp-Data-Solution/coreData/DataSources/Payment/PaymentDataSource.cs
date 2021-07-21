using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using coreLogic;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Payment;

namespace coreData.DataSources.Payment
{
    [DataObject]
    public class PaymentDataSource
    {
            private readonly ICommerceEntities le;
            private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
            public PaymentDataSource()
        {
            var db2 = new CommerceEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

            [DataObjectMethod(DataObjectMethodType.Select)]
            public List<PaymentViewModel> GetPayments(long customerId)
            {
                var data = le.arPayments
                    .Where(p => p.customerId == customerId)
                    .Select(p => new PaymentViewModel
                    {
                        paymentId = p.arPaymentId,
                        customerId = p.customerId,
                        date = p.paymentDate,
                        paymentCurrencyId = p.currencyId,
                        paymentMethodId = p.paymentMethodId,
                        paymentTotal = p.totalAmountPaid,
                        checkNumber = p.checkNumber,
                        creditCardNumber = p.creditCardNumber,
                        mobileMoneyNumber = p.mobileMoneyNumber
                    }).OrderBy(p => p.date)
                    .ToList();

                foreach (var record in data)
                {
                    record.customerName = le.customers.FirstOrDefault(p => p.customerId == record.customerId).customerName;
                    record.paymentCurrency = ctx.currencies.FirstOrDefault(p => p.currency_id == record.paymentCurrencyId).major_name; ;
                    record.paymentMethod = le.paymentMethods.FirstOrDefault(p => p.paymentMethodID == record.paymentMethodId).paymentMethodName;
                    record.paymentDate = string.Format("{0:dd/MMM/yyyy}", record.date);
                    if (record.checkNumber != "")
                    {
                        record.isCheck = true;
                        record.isCreditCard = false;
                        record.isMobileMoney = false;
                    }
                    else if (record.creditCardNumber != "")
                    {
                        record.isCheck = false;
                        record.isCreditCard = true;
                        record.isMobileMoney = false;
                    }
                    else if (record.mobileMoneyNumber != "")
                    {
                        record.isCheck = false;
                        record.isCreditCard = false;
                        record.isMobileMoney = true;
                    }
                    record.invoiceTotal = 0;
                    record.paymentLines = GetPaymentLines(record.paymentId);
                    record.compamy = GetCompanyProfile();

                    foreach (var line in record.paymentLines)
                    {
                        record.invoiceTotal += line.amountPaid;
                    }
                    record.overPaymentAmount = record.paymentTotal - record.invoiceTotal;

                }

                return data;
            }


            [DataObjectMethod(DataObjectMethodType.Select)]
            public List<PaymentLinesViewModel> GetPaymentLines(long paymentId)
            {
                var data = le.arPaymentLines
                    .Where(p => p.arPaymentId == paymentId)
                            .Select(p => new PaymentLinesViewModel
                            {
                                paymentLineId = p.arPaymentLineId,
                                invoiceId = p.arinvoiceId,
                                amountPaid = p.amountPaid
                            }).OrderBy(p => p.paymentLineId)
                            .ToList();
                var num = 0;
                foreach (var record in data)
                {
                    var invoice = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == record.invoiceId);
                    record.lineNumber = num++;
                    record.invoiceNumber = invoice.invoiceNumber;
                    record.invoiceDate = string.Format("{0:dd/MMM/yyyy}", invoice.invoiceDate);
                    record.invoiceTotal = invoice.totalAmount;
                    record.invoiceBalance = invoice.balance;
                }

                return data;
            }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public CompanyProfileViewModel GetCompanyProfile()
        {
            var data = ctx.comp_prof
                            .Select(p => new CompanyProfileViewModel
                            {
                                companyProfileId = p.comp_prof_id,
                                companyName = p.comp_name,
                                companyLogo = p.logo,
                                companyAddressLine = p.addr_line_1,
                                companyPhoneNumber = p.phon_num,
                                companyEmail = p.email,
                                companyCityId = p.city_id,
                                companyCountryId = p.country_id
                            }).First();
                            
               data.companyCity = ctx.cities.FirstOrDefault(p => p.city_id == data.companyCityId).city_name;
               data.companyCountry = ctx.countries.FirstOrDefault(p => p.country_id == data.companyCountryId).country_name;
            
            return data;
        }


    }
}

