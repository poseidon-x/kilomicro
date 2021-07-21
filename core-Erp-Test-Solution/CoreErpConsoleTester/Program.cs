using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace CoreErpConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            RepaymentAmountDistributor nrm = new RepaymentAmountDistributor();
            DateTime date = new DateTime(2015,03,20);

            //Console.WriteLine("Enter loanId, Payment Amount, payment date, paymentTypeId. Separated by comma");

            var appliedRepay = nrm.receiveRepayment(98624, 2866, date, "3");

            Console.WriteLine("Amount received: {0}, Interest Applied: {1}, Principal Applied: {2}, Penalty Applied: {3}",
                appliedRepay.amountPaid,appliedRepay.interestPaid,appliedRepay.principalPaid,appliedRepay.penaltyPaid);

            Console.Read();
        }
    }
}
