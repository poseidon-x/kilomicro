namespace DepositInterestUpgradeService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DepositInterestServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // DepositInterestServiceInstaller
            // 
            this.DepositInterestServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.DepositInterestServiceInstaller.Password = null;
            this.DepositInterestServiceInstaller.Username = null;
            this.DepositInterestServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "DepoositInterestUpgradeService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DepositInterestServiceInstaller,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DepositInterestServiceInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}