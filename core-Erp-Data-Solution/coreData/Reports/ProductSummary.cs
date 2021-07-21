namespace coreData.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for ProductSummary.
    /// </summary>
    public partial class ProductSummary : Telerik.Reporting.Report
    {
        public ProductSummary()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public  static string Format(Nullable<int> value)
        {
            if (value == null)
            {
                return "0";
            }
            return value.Value.ToString("#,##0");
        }

        public  static string Format(Nullable<double> value)
        {
            if (value == null)
            {
                return "0";
            }
            return value.Value.ToString("#,##0.#0");
        }

        public  static string Format(int value)
        { 
            return value.ToString("#,##0");
        }

        public  static string Format(double value)
        { 
            return value.ToString("#,##0.#0");
        }

        public static string Format(long value)
        {
            return value.ToString("#,##0");
        }
         
    }
}