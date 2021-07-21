namespace coreData.Reports.Loans
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for OutstandingScheduleItemsReport.
    /// </summary>
    public partial class OutstandingScheduleItemsReport : Telerik.Reporting.Report
    {
        public OutstandingScheduleItemsReport()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static string Format(object value)
        {
            if (value == null) return "N/A";
            var time = value as DateTime?;
            if (time != null)
                return Format2(time.Value);
            var d = value as double?;
            if (d != null)
                return Format2(d.Value);
            return "INVALID";
        }

        public static string Format2(DateTime value)
        {
            return value.ToString("dd-MMM-yyyy");
        }

        public static string Format(double? value)
        {
            if (value == null) return "N/A";
            return Format2(value.Value);
        }

        public static string Format2(double value)
        {
            return value.ToString("#,##0.#0");
        }
    }
}