using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using coreNotificationsLibrary.Modules;
using coreNotificationsLibrary.Processors;
using System.Threading;

namespace coreNotificationsService
{
    public partial class notificationService : ServiceBase
    {
        private IModule<LoanRepaymentsCollectionProcessor> loanRepaymentsModule;
        private IModule<MessageEventSendingProcessor> loanRepaymentsSendingModule;
        private IModule<LoanApprovalCollectionProcessor> loanApprovalssSendingModule;
        private IModule<SavingsDepositCollectionProcessor> savingsDepositModule;
        private IModule<InvestmentDepositCollectionProcessor> investmentDepositModule;
        private IModule<SavingsWithdrawalCollectionProcessor> savingsWithdrawalModule;
        private IModule<InvestmentWithdrawalCollecionProcessor> investmentWithdrawalModule;
        private IModule<LoanScheduleCollectionProcessor> loanScheduleModule;
        private IModule<ClientBirthdayCollectionProcessor> clientBirthdaysModule;
        private IModule<ClientWelcomeCollectionProcessor> clientWelcomeModule;
        private IModule<InvestmentMaturityAdviceCollectionProcessor> investmentMaturityModule;
        private IModule<InvestmentPaymentDueAdviceCollectionProcessor> investmentDueModule;
        private IModule<InvestmentMaturityRolloverCollectionProcessor> investmentRolloverModule;
        private IModule<MiniStatementCollectionProcessor> miniStatementModule;


        public notificationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            loanRepaymentsModule = new EventModule<LoanRepaymentsCollectionProcessor>(
                new LoanRepaymentsCollectionProcessor());
            Thread thLoanRepaymentsModule = new Thread(new ThreadStart(loanRepaymentsModule.main));
            loanRepaymentsSendingModule = new EventModule<MessageEventSendingProcessor>(
                new MessageEventSendingProcessor());
            Thread thLoanRepaymentsSendingModule = new Thread(new ThreadStart(loanRepaymentsSendingModule.main));
            loanApprovalssSendingModule = new EventModule<LoanApprovalCollectionProcessor>(
                new LoanApprovalCollectionProcessor());
            Thread thLoanApprovalsSendingModule = new Thread(new ThreadStart(loanApprovalssSendingModule.main));
            savingsDepositModule = new EventModule<SavingsDepositCollectionProcessor>(
                new SavingsDepositCollectionProcessor());
            Thread thsavingsDepositModule = new Thread(new ThreadStart(savingsDepositModule.main));
            investmentDepositModule = new EventModule<InvestmentDepositCollectionProcessor>(
                new InvestmentDepositCollectionProcessor());
            Thread thinvestmentDepositModule = new Thread(new ThreadStart(investmentDepositModule.main));
            savingsWithdrawalModule = new EventModule<SavingsWithdrawalCollectionProcessor>(
                new SavingsWithdrawalCollectionProcessor());
            Thread thsavingsWithdrawalModule = new Thread(new ThreadStart(savingsWithdrawalModule.main));
            investmentWithdrawalModule = new EventModule<InvestmentWithdrawalCollecionProcessor>(
                new InvestmentWithdrawalCollecionProcessor());
            Thread thinvestmentWithdrawalModule = new Thread(new ThreadStart(investmentWithdrawalModule.main));
            loanScheduleModule = new EventModule<LoanScheduleCollectionProcessor>(
                new LoanScheduleCollectionProcessor());
            Thread thloanScheduleModule = new Thread(new ThreadStart(loanScheduleModule.main));
            clientBirthdaysModule = new EventModule<ClientBirthdayCollectionProcessor>(
                new ClientBirthdayCollectionProcessor());
            Thread thclientBirthdaysModule = new Thread(new ThreadStart(clientBirthdaysModule.main));
            clientWelcomeModule = new EventModule<ClientWelcomeCollectionProcessor>(
                new ClientWelcomeCollectionProcessor());
            Thread thclientWelcomeModule = new Thread(new ThreadStart(clientWelcomeModule.main));
            investmentMaturityModule = new EventModule<InvestmentMaturityAdviceCollectionProcessor>(
                new InvestmentMaturityAdviceCollectionProcessor());
            Thread thinvestmentMaturityModule = new Thread(new ThreadStart(investmentMaturityModule.main));
            investmentDueModule = new EventModule<InvestmentPaymentDueAdviceCollectionProcessor>(
                new InvestmentPaymentDueAdviceCollectionProcessor());
            Thread thinvestmentDueModule = new Thread(new ThreadStart(investmentDueModule.main));
            investmentRolloverModule = new EventModule<InvestmentMaturityRolloverCollectionProcessor>(
                new InvestmentMaturityRolloverCollectionProcessor());
            Thread thinvestmentRolloverModule = new Thread(new ThreadStart(investmentRolloverModule.main));

            //NB: Only the mini-statement scheduling is done here :-) Wendolin 
            var dayOfMonth = DateTime.Now.Day;
            var lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var hourOfProcess = DateTime.Now.Hour;
            var dayToExec = System.Configuration.ConfigurationManager.AppSettings["dayToExec"];
            var firstDayOfMonth = 1;
            var dayToExecIntparsed = int.Parse(dayToExec);


            var hourToProcess = int.Parse(System.Configuration.ConfigurationManager.AppSettings["hourToProcess"]);

            if ((dayOfMonth == dayToExecIntparsed || dayOfMonth == firstDayOfMonth))//|| dayOfMonth==lastDayOfMonth || hourOfProcess == hourToProcess)
            {
                miniStatementModule = new EventModule<MiniStatementCollectionProcessor>(
                new MiniStatementCollectionProcessor());
                Thread thMiniStatementModule = new Thread(new ThreadStart(miniStatementModule.main));

                thMiniStatementModule.Start();
            }


            //NB: Stopped using this SMS service  :-) Wendolin
            // thLoanRepaymentsSendingModule.Start();


            thLoanRepaymentsModule.Start();
            thLoanApprovalsSendingModule.Start();
            thsavingsDepositModule.Start();
            thinvestmentDepositModule.Start();
            thsavingsWithdrawalModule.Start();
            thinvestmentWithdrawalModule.Start();
            thloanScheduleModule.Start();
            thclientBirthdaysModule.Start();
            thclientWelcomeModule.Start();
            thinvestmentMaturityModule.Start();
            thinvestmentDueModule.Start();
            thinvestmentRolloverModule.Start();
        }

        protected override void OnStop()
        {
            loanRepaymentsModule.stopFlag = true;
            loanRepaymentsSendingModule.stopFlag = true;
            loanApprovalssSendingModule.stopFlag = true;
            savingsDepositModule.stopFlag = true;
            investmentDepositModule.stopFlag = true;
            savingsWithdrawalModule.stopFlag = true;
            investmentWithdrawalModule.stopFlag = true;
            loanScheduleModule.stopFlag = true;
            clientBirthdaysModule.stopFlag = true;
            clientWelcomeModule.stopFlag = true;
            investmentMaturityModule.stopFlag = true;
            investmentDueModule.stopFlag = true;
            investmentRolloverModule.stopFlag = true;
            miniStatementModule.stopFlag = true;

            for (int i = 0; i < 60; i++)
            {
                if (loanRepaymentsModule.stopped == true
                    && loanRepaymentsSendingModule.stopped == true
                    && loanApprovalssSendingModule.stopped == true
                    && savingsDepositModule.stopped == true
                    && investmentDepositModule.stopped==true
                    && savingsWithdrawalModule.stopped == true
                    && investmentWithdrawalModule.stopped == true
                    && loanScheduleModule.stopped == true
                    && clientBirthdaysModule.stopped == true
                    && clientWelcomeModule.stopped == true
                    && investmentMaturityModule.stopped == true
                    && investmentDueModule.stopped == true
                    && investmentRolloverModule.stopped==true
                    && miniStatementModule.stopped == true
                    ) break;
                Thread.Sleep(1000);
            }
        }

        public void OnDebug()
        {
            OnStart(null);
            
        }

        public void OnDebugStop()
        {
            OnStop();
        }
    }
}
