namespace coreData.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for RegularSusuAccount.
    /// </summary>
    public partial class RegularSusuAccount : Telerik.Reporting.Report
    {
        public RegularSusuAccount()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }


        public static string FormatDate(DateTime input)
        {
            return input.ToString("dd-MMM-yyyy");
        }

        //format number
        public static string FormatNumber(double input)
        {
            return input.ToString("#,##0.#0");
        }
    }
}