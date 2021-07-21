using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using coreErpPointOfSale.Models;
using coreErpPointOfSale.ViewModel;
using coreLogic;
using Newtonsoft.Json;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Model;

namespace CoreErpPointOfSale
{
    /// <summary>
    /// Interaction logic for Sales_Receipt.xaml
    /// </summary>
    public partial class Sales_Receipt : Window
    {
        private List<comp_prof> companyProfile;
        private List<cities> citiesList;
        private cities city;
        private List<customer> cashCustomer;
        private arInvoice invoice;


        public Sales_Receipt(arInvoice currentInvoice)
        {
            InitializeComponent();
            invoice = currentInvoice;

            lblOrderDate.Content = invoice.invoiceDate.ToString("dd-MMM-yyyy");
            lblOrderTime.Content = invoice.invoiceDate.ToString("hh:mm:ss tt");
            lblOrderInvoiceNumber.Content = invoice.invoiceNumber;

            this.Loaded += Sales_Receipt_Loaded;

            ReceiptDataGrid.AutoGenerateColumns = false;
            ReceiptDataGrid.CanUserReorderColumns = false;
            ReceiptDataGrid.CanUserSortColumns = false;
            ReceiptDataGrid.ItemsSource = SalesReceiptViewModel.SetReceiptGrid(invoice);
            //ListGrid.ItemsSource = SalesReceiptViewModel.SetReceiptGrid(invoice);
        }

        void Sales_Receipt_Loaded(object sender, RoutedEventArgs e)
        {
            SetupUi();
        }

        private void SetupUi()
        {
            using (var client = new WebClient())
            {
                var companyUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/companyProfile/Get";
                var cityUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/city/Get";
                var customerUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/customer/GetCashCustomer";

                var token = mainwindow.token;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers[HttpRequestHeader.Authorization] = "coreBearer " + token;

                byte[] companyResult = client.DownloadData(companyUrl);
                byte[] citiesResult = client.DownloadData(cityUrl);
                byte[] customerResult = client.DownloadData(customerUrl);

                // now use a JSON parser to parse the resulting string back to some CLR object
                companyProfile = CompanyParse(companyResult);
                citiesList = CityParse(citiesResult);
                cashCustomer = CustomerParse(customerResult);

                //retrieve company cityName with companyCityId 
                foreach (var record in citiesList)
                {
                    if (record.city_id == companyProfile[0].city_id)
                    {
                        city = record;
                    }
                }

                var salesPerson = mainwindow.name;

                //setup company Details 
                lblCompanyName.Content = companyProfile[0].comp_name;
                lblAddress.Content = companyProfile[0].addr_line_1;
                lblLocation.Content = city.city_name;
                lblPhone.Content = companyProfile[0].phon_num;
                lblSalesperson.Content = salesPerson;


                //Setup Customer ComboBox
                lblCustomer.Content = cashCustomer[0].customerName;
                lblSalesperson.Content = "Awaiting";
            }
            int itemsCount = 0;
            //foreach (var record in ListGrid.Items)
            //{
            //    Sales row = (Sales)record;
            //    int value;
            //    if (int.TryParse(row.unitPrice, out value))
            //    {
            //        itemsCount ++;
            //    }
            //}
            //lblItemsCount.Content = itemsCount;
        }

        public static List<comp_prof> CompanyParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<comp_prof>>(jsonStr);
        }

        public static List<cities> CityParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<cities>>(jsonStr);
        }

        public static List<customer> CustomerParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<List<customer>>(jsonStr);
        }

        private void btnPrintReceipt_Click(object sender, RoutedEventArgs e)
        {
            
            PrintDialog printDlg = new PrintDialog();

            //pointOfSaleReceipt.Measure(new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight));
            //pointOfSaleReceipt.Arrange(new Rect(new Point(50, 50), pointOfSaleReceipt.DesiredSize));

            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(pointOfSaleReceipt, "Cash Sale");
            }

        }



    }
}
