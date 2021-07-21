using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DepositInterestUpgradeServiceEngine.Module;
using coreData.ErrorLog;


namespace DepositInterestUpgradeService
{
    public partial class Application : ServiceBase
    {
        DepositInterestUpgradeModule depIntUpgradeMod;

        public Application()
        {
            try
            {
                Logger.serviceError("Deposit Interest Upgrade Initialize");
                InitializeComponent();
                this.ServiceName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
            }
            catch (Exception x)
            {
                Logger.logError(x);
            }
            
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.serviceError("Deposit Interest Upgrade Started");
                depIntUpgradeMod = new DepositInterestUpgradeModule();
                Thread depIntUpMod = new Thread(new ThreadStart(depIntUpgradeMod.Main));
                depIntUpMod.Start();
            }
            catch (Exception x)
            {
                Logger.logError(x);
            }
            
        }

        protected override void OnStop()
        {
            try
            {
                Logger.serviceError("Deposit Interest Upgrade Stopped");
                depIntUpgradeMod.StopFlag = true;
                DateTime time = DateTime.Now;

                while (depIntUpgradeMod.Stopped == false)
                {
                    Thread.Sleep(1000);
                    if ((DateTime.Now - time).TotalSeconds > 10) break;
                }
            }
            catch (Exception x)
            {
                Logger.logError(x);
            }
            
        }


    }
}
