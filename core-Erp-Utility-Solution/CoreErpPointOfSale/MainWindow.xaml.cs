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
using coreData.ErrorLog;
using coreData.Constants;
using coreModels.Login;
using Newtonsoft.Json;

namespace CoreErpPointOfSale
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class mainwindow : Window
    {
        public LoginRequest loginUser;
        public LoginResponse loginResp;
        public static string token;
        public static string name;

        public mainwindow()
        {
            InitializeComponent();
            Loaded+=Login_Loaded;
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            System.Threading.Thread.Sleep(45000);
            this.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtUsername.Text) && !string.IsNullOrWhiteSpace(TxtUsername.Text)
                && !string.IsNullOrEmpty(TxtPassword.Password))
            {

                loginUser = new LoginRequest
                {
                    userName = TxtUsername.Text,
                    password = TxtPassword.Password
                };

                UserLogin(loginUser);
                if (loginResp != null)
                {
                    token = loginResp.token;
                    name = loginResp.name;
                    this.Hide();
                    PointOfSale pointOfSale = new PointOfSale();
                    pointOfSale.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Invalid Username or Password");
                }
            }
            else MessageBox.Show("Invalid Username or Password");
        }
        CoreInfoLogger cil = new CoreInfoLogger();

        private void UserLogin(LoginRequest value)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var loginUrl = System.Configuration.ConfigurationManager.AppSettings["apiUrlRoot"] + "/crud/login/Post";

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
                    byte[] result = client.UploadData(loginUrl, "POST", data);
                    // now use a JSON parser to parse the resulting string back to some CLR object
                    loginResp = LoginParse(result);
                }
            }
            catch (Exception x)
            {
                cil.logInfo(x.InnerException.Message);
                throw new ApplicationException(ErrorMessages.ErrorLoggingIn);
            }
            
        }

        public static LoginResponse LoginParse(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<LoginResponse>(jsonStr);
        }

    }
}
