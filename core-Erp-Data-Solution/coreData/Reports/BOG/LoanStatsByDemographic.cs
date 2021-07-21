namespace coreData.Reports.BOG
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for LoanStatsByDemographic.
    /// </summary>
    public partial class LoanStatsByDemographic : Telerik.Reporting.Report
    {
        public LoanStatsByDemographic()
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