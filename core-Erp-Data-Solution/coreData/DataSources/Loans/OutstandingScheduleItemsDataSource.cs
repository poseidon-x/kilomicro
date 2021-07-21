using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using coreLogic.Models.Loans;

namespace coreData.DataSources.Loans
{
    [DataObject]
    public class OutstandingScheduleItemsDataSource
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<OutstandingScheduleItemsViewModel> GetOutstanding(int? branchId, int? clientId,
            DateTime startDate, DateTime endDate)
        {
            var ed = endDate.AddMonths(-1).Date.AddSeconds(-1);
            using (var le = new coreLoansEntities())
            {
                var data = (
                    from l in le.loans
                    from c in le.clients
                    join rs in le.repaymentSchedules on l.loanID equals rs.loanID into rsa
                    from rs in rsa.DefaultIfEmpty()
                    where l.clientID == c.clientID
                          && l.loanStatusID == 4
                          && rs.principalBalance + rs.interestBalance > 1
                          && rs.repaymentDate <= ed && rs.repaymentDate >= startDate
                          && (branchId == null || branchId == -1 || branchId == c.branchID)
                          && (clientId == null || clientId == -1 || clientId == c.clientID)
                    group rs by new
                    {
                        l.clientID,
                        l.loanID,
                        l.loanNo,
                        l.amountDisbursed,
                        l.disbursementDate,
                        clientName = c.surName + ", " + c.otherNames,
                        clientNumber = c.accountNumber,
                        l.loanTenure
                    }
                    into grsa
                    select new OutstandingScheduleItemsViewModel
                    {
                        amountDisbursed = grsa.Key.amountDisbursed,
                        loanId = grsa.Key.loanID,
                        loanNumber = grsa.Key.loanNo,
                        clientId = grsa.Key.clientID,
                        clientName = grsa.Key.clientName,
                        clientNumber = grsa.Key.clientNumber,
                        numberOfPaymentsInArears = (grsa.Any() ? grsa.Count() : 0),
                        amountInArears =
                            (grsa.Any() ? Math.Round(grsa.Sum(p => p.principalBalance + p.interestBalance), 2) : 0),
                        loanTenure = grsa.Key.loanTenure,
                        disbursementDate = grsa.Key.disbursementDate
                    }
                    ).Where(p => p.amountInArears > 10)
                    .OrderBy(p => p.clientName)
                    .ToList();

                using (var ent = new core_dbEntities())
                {
                    var prof = ent.comp_prof.First();
                    foreach (var item in data)
                    {
                        var client = le.clients.First(p => p.clientID == item.clientId);
                        string phoneNumber = "";
                        foreach (var phone in client.clientPhones)
                        {
                            if (phone.phone != null && phone.phone.phoneNo != "")
                            {
                                phoneNumber += (phoneNumber == "" ? "" : "/") + phone.phone.phoneNo;
                            }
                        }
                        string address = "";
                        string directions = "";
                        foreach (var add in client.clientAddresses)
                        {
                            if (add.address != null && add.addressTypeID == 1)
                            {
                                directions = add.address.addressLine1 + ", " + add.address.cityTown;
                            }
                            else if (add.address != null && add.addressTypeID == 2)
                            {
                                address = add.address.addressLine1 + ", " + add.address.cityTown;
                            }
                        }
                        string emailAddress = "";
                        foreach (var email in client.clientEmails)
                        {
                            if (email.email != null && email.email.emailAddress != "")
                            {
                                emailAddress += (emailAddress == "" ? "" : "/") + email.email.emailAddress;
                            }
                        }
                        item.phoneNumber = phoneNumber;
                        item.address = address;
                        item.directions = directions;
                        item.email = emailAddress;
                        var payments = le.loanRepayments
                            .Where(p => p.loanID == item.loanId && p.repaymentDate <= endDate
                                        &&
                                        (p.repaymentTypeID == 1 || p.repaymentTypeID == 2 || p.repaymentTypeID == 3 ||
                                         p.repaymentTypeID == 7))
                            .ToList();
                        if (payments.Any())
                        {
                            item.lastPaymentDate = payments.Max(p => p.repaymentDate);
                        }
                        else
                        {
                            item.lastPaymentDate = item.disbursementDate.Value;
                        }
                        item.companyAddress = prof.addr_line_1;
                        item.companyLogo = prof.logo;
                        item.expiryDate = item.disbursementDate.Value.AddMonths((int)item.loanTenure);
                    }
                }
                return data;
            }
        }
    }
}
