using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreServiceEngine;

namespace coreService
{
    public partial class Application : ServiceBase
    {
        LoanPenaltyModule _penMod;
        EmailModule emailMod;
        DepositInterestModule intMod;  
        SavingsInterestModule siMod; 
        SavingsBalanceModule sbMod;
        SavingsPlanFlagModule sfMod;
        DepositMaturityNotificationModule dpMatNot;

        public Application()
        {
            InitializeComponent();
            this.ServiceName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        }

        protected override void OnStart(string[] args)
        {
            _penMod = new LoanPenaltyModule();
            Thread pthMod = new Thread(new ThreadStart(_penMod.Main));
            pthMod.Start();

            emailMod = new EmailModule();
            Thread thEm = new Thread(new ThreadStart(emailMod.Main));
            thEm.Start();

            intMod = new DepositInterestModule();
            Thread thInt = new Thread(new ThreadStart(intMod.Main));
            thInt.Start();
             
            siMod = new SavingsInterestModule();
            Thread sithMod = new Thread(new ThreadStart(siMod.Main));
            sithMod.Start();
             
            sbMod = new SavingsBalanceModule();
            Thread thSB= new Thread(new ThreadStart(sbMod.Main));
            thSB.Start();

            sfMod = new SavingsPlanFlagModule();
            Thread thSF = new Thread(new ThreadStart(sfMod.Main));
            thSF.Start();

            dpMatNot = new DepositMaturityNotificationModule();
            Thread thDMN = new Thread(new ThreadStart(dpMatNot.Main));
            thDMN.Start();

        }

        protected override void OnStop()
        {
            _penMod.StopFlag = true;  
            sfMod.StopFlag = true;
            siMod.StopFlag = true;
            intMod.StopFlag = true; 
            sbMod.StopFlag = true;
            dpMatNot.StopFlag = true;

            DateTime time = DateTime.Now;

            while (siMod.Stopped == false)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - time).TotalSeconds > 10) break;
            }

            while (intMod.Stopped == false)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - time).TotalSeconds > 10) break;
            }
             
            while (sbMod.Stopped == false)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - time).TotalSeconds > 30) break;
            }

            while (sfMod.Stopped == false)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - time).TotalSeconds > 30) break;
            }

            while (dpMatNot.StopFlag == false)
            {
                Thread.Sleep(1000);
                if ((DateTime.Now - time).TotalSeconds > 30) break;
            }
        }
    }
}
