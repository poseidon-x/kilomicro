using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using coreLogic;

namespace coreLogic.Tests.RepaymentManager
{
    [TestClass]
    public class RepaymentAmountDistributorTests
    {
        private loan loanWithNoBalance;
        private repaymentSchedule LWNBsch1;
        private repaymentSchedule LWNBsch2;
        private repaymentSchedule LWNBsch3;
        private repaymentSchedule LWNBsch4;
        private repaymentSchedule LWNBsch5;
        private repaymentSchedule LWNBsch6;
        private loanRepayment LWNBlrp1;
        private loanRepayment LWNBlrp2;
        private loanRepayment LWNBlrp3;
        private loanRepayment LWNBlrp3OverPayment;


        private loan loanWithOnlyPrincipalBalance;
        private repaymentSchedule LWOPBsch1;
        private repaymentSchedule LWOPBsch2;
        private repaymentSchedule LWOPBsch3;
        private loanRepayment LWOPBlrp1;
        private loanRepayment LWOPBlrp2;
        private loanRepayment LWOPBlrp3PayInterestOnly;
        private loanRepayment LWOPBlrp4OverPayment;

        private loan loanWithOnlyInterestBalance;
        private loan loanWithBothInterestAndPrincipalBalance;


        //private List<repaymentSchedule> loanWithNoBalanceSchedules;
        //private List<loanRepayment> loanWithNoBalanceRepayments;

        //private List<repaymentSchedule> loanWithOnlyPrincipalBalanceSchedules;
        //private List<loanRepayment> loanWithNoBalanceRepayments;

        private MockcoreLoansEntities _context;

        [TestInitialize]
        public void TestInit()
        {
            _context = new MockcoreLoansEntities();

            //Loan with No balance
            loanWithNoBalance = new loan
            {
                loanID = 1,
                balance = 0,
                loanStatusID = 4,
                interestRate = 6,
                repaymentModeID = 30,
                loanTenure = 6,
                closed = false,
                amountDisbursed = 6000,
                disbursementDate = DateTime.Today.AddMonths(-4)
            };

            //Loan with No balance Schedule 1
            LWNBsch1 = new repaymentSchedule
            {
                repaymentScheduleID = 1,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-3),
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch1);

            //Loan with No balance Schedule 2
            LWNBsch2 = new repaymentSchedule
            {
                repaymentScheduleID = 2,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-2),
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch2);

            //Loan with No balance Schedule 3
            LWNBsch3 = new repaymentSchedule
            {
                repaymentScheduleID = 3,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-1),
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch3);

            //Loan with No balance Schedule 4
            LWNBsch4 = new repaymentSchedule
            {
                repaymentScheduleID = 4,
                loanID = 1,
                repaymentDate = DateTime.Today,
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch4);

            //Loan with No balance Schedule 5
            LWNBsch5 = new repaymentSchedule
            {
                repaymentScheduleID = 5,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(1),
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch5);

            //Loan with No balance Schedule 6
            LWNBsch6 = new repaymentSchedule
            {
                repaymentScheduleID = 6,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(2),
                interestPayment = 360,
                principalPayment = 1000,
                interestBalance = 360,
                principalBalance = 1000
            };
            loanWithNoBalance.repaymentSchedules.Add(LWNBsch6);

            //Loan with No balance Repayment 1
            LWNBlrp1 = new loanRepayment
            {
                loanRepaymentID = 1,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-3),
                amountPaid = 2720,
                interestPaid = 720,
                principalPaid = 2000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            loanWithNoBalance.loanRepayments.Add(LWNBlrp1);

            //Loan with No balance Repayment 2
            LWNBlrp2 = new loanRepayment
            {
                loanRepaymentID = 2,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-2),
                amountPaid = 2720,
                interestPaid = 720,
                principalPaid = 2000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            loanWithNoBalance.loanRepayments.Add(LWNBlrp2);

            //Loan with No balance Repayment 3
            LWNBlrp3 = new loanRepayment
            {
                loanRepaymentID = 3,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-1),
                amountPaid = 2720,
                interestPaid = 720,
                principalPaid = 2000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            loanWithNoBalance.loanRepayments.Add(LWNBlrp3);

            //Loan with No balance Repayment 4 over payment
            LWNBlrp3OverPayment = new loanRepayment
            {
                loanRepaymentID = 4,
                loanID = 1,
                repaymentDate = DateTime.Today.AddMonths(-1),
                amountPaid = 2720,
                interestPaid = 720,
                principalPaid = 2000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            
            _context.loans.Add(loanWithNoBalance);
            



            //Loan with Only principal balance 
            loanWithOnlyPrincipalBalance = new loan
            {
                loanID = 2,
                balance = 1000,
                loanStatusID = 4,
                interestRate = 6,
                repaymentModeID = 30,
                loanTenure = 3,
                closed = false,
                amountDisbursed = 3000,
                disbursementDate = DateTime.Today.AddMonths(-3)
            };
            //Loan with Only principal balance Schedule 1
            LWOPBsch1 = new repaymentSchedule
            {
                repaymentScheduleID = 7,
                loanID = 2,
                repaymentDate = DateTime.Today.AddMonths(-2),
                interestPayment = 180,
                principalPayment = 1000,
                interestBalance = 180,
                principalBalance = 1000
            };
            loanWithOnlyPrincipalBalance.repaymentSchedules.Add(LWNBsch1);

            //Loan with Only principal balance Schedule 2
            LWOPBsch2 = new repaymentSchedule
            {
                repaymentScheduleID = 8,
                loanID = 2,
                repaymentDate = DateTime.Today.AddMonths(-1),
                interestPayment = 180,
                principalPayment = 1000,
                interestBalance = 180,
                principalBalance = 1000
            };
            loanWithOnlyPrincipalBalance.repaymentSchedules.Add(LWOPBsch2);

            //Loan with Only principal balance Schedule 3
            LWOPBsch3 = new repaymentSchedule
            {
                repaymentScheduleID = 9,
                loanID = 2,
                repaymentDate = DateTime.Today,
                interestPayment = 180,
                principalPayment = 1000,
                interestBalance = 180,
                principalBalance = 1000
            };
            loanWithOnlyPrincipalBalance.repaymentSchedules.Add(LWOPBsch3);

            //Loan with Only principal balance exact payment
            LWOPBlrp1 = new loanRepayment
            {
                loanRepaymentID = 5,
                loanID = 2,
                repaymentDate = DateTime.Today.AddMonths(-2),
                amountPaid = 1180,
                interestPaid = 180,
                principalPaid = 1000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            loanWithOnlyPrincipalBalance.loanRepayments.Add(LWOPBlrp1);

            //Loan with Only principal balance, exact payment Principal & Interest Payment
            LWOPBlrp2 = new loanRepayment
            {
                loanRepaymentID = 6,
                loanID = 2,
                repaymentDate = DateTime.Today.AddMonths(-1),
                amountPaid = 1180,
                interestPaid = 180,
                principalPaid = 1000,
                penaltyPaid = 0,
                repaymentTypeID = 1
            };
            loanWithOnlyPrincipalBalance.loanRepayments.Add(LWOPBlrp2);

            //Loan with Only principal balance, exact payment  Interest Only
            LWOPBlrp3PayInterestOnly = new loanRepayment
            {
                loanRepaymentID = 7,
                loanID = 2,
                repaymentDate = DateTime.Today,
                amountPaid = 180,
                interestPaid = 180,
                principalPaid = 0,
                penaltyPaid = 0,
                repaymentTypeID = 3
            };
            loanWithOnlyPrincipalBalance.loanRepayments.Add(LWOPBlrp3PayInterestOnly);

            //Loan with Only principal balance, over payment  Principal Only
            LWOPBlrp4OverPayment = new loanRepayment
            {
                loanRepaymentID = 6,
                loanID = 2,
                repaymentDate = DateTime.Today,
                amountPaid = 2000,
                interestPaid = 0,
                principalPaid = 2000,
                penaltyPaid = 0,
                repaymentTypeID = 2
            };

            _context.loans.Add(loanWithOnlyPrincipalBalance);
        }


        [TestMethod]
        public void Test_Repayment_Type_Principal_And_Interest_For_Loan_With_No_Balance_Pass()
        {
            RepaymentAmountDistributor repaymAmtDist = new RepaymentAmountDistributor(_context);
            var apportRepay = repaymAmtDist.receiveRepayment(1,1000,DateTime.Today,"1");
            Assert.IsTrue(apportRepay.principalPaid > 1);
        }

        [TestMethod]
        public void Test_Repayment_Type_Principal_Only_For_Loan_With_No_Balance_Pass()
        {
            RepaymentAmountDistributor repaymAmtDist = new RepaymentAmountDistributor(_context);
            var apportRepay = repaymAmtDist.receiveRepayment(1, 1000, DateTime.Today, "2");
            Assert.IsTrue(apportRepay.principalPaid > 1);
        }

        [TestMethod]
        public void Test_Repayment_Type_Interest_Only_For_Loan_With_No_Balance_Pass()
        {
            RepaymentAmountDistributor repaymAmtDist = new RepaymentAmountDistributor(_context);
            var apportRepay = repaymAmtDist.receiveRepayment(1, 1000, DateTime.Today, "3");
            Assert.IsTrue(apportRepay.principalPaid > 1);
        }

        [TestMethod]
        public void Test_Over_Payment_For_Repayment_Type_Principal_And_Interest_For_Loan_With_No_Interest_Balance_Pass()
        {
            RepaymentAmountDistributor repaymAmtDist = new RepaymentAmountDistributor(_context);
            var apportRepay = repaymAmtDist.receiveRepayment(2, 1180, DateTime.Today, "1");
            Assert.IsTrue(apportRepay.principalPaid > 1);
        }

        [TestMethod]
        public void Test_Repayment_Type_Interest_Only_For_Loan_With_No_Interest_Balance_Pass()
        {
            RepaymentAmountDistributor repaymAmtDist = new RepaymentAmountDistributor(_context);
            var apportRepay = repaymAmtDist.receiveRepayment(2, 1000, DateTime.Today, "1");
            Assert.IsTrue(apportRepay.principalPaid > 1);
        }


    }
}
