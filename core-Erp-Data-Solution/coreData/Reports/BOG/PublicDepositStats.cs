namespace coreData.Reports.BOG
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for PublicDepositStatss.
    /// </summary>
    public partial class PublicDepositStats : Telerik.Reporting.Report
    {
        public PublicDepositStats()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static string Format(double? amount)
        {
            return amount.Value.ToString("#,##0.#0");
        }

        public static string Format(int? amount)
        {
            return amount.Value.ToString("#,##0");
        }

        public static string Format(double amount)
        {
            return amount.ToString("#,##0.#0");
        }

        public static string Format(int amount)
        {
            return amount.ToString("#,##0");
        }

    }
}