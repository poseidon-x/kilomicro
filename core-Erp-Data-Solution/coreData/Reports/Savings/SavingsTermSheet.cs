namespace coreData.Reports.Savings
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for SavingsTermSheet.
    /// </summary>
    public partial class SavingsTermSheet : Telerik.Reporting.Report
    {
        public SavingsTermSheet()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static string Format(DateTime value)
        {
            return value.ToString("dd-MMM-yyyy");
        }

        public static string Format(double value)
        {
            return value.ToString("#,##0.#0");
        }

    }
}