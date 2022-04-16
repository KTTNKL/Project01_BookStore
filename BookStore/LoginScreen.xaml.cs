using BookStore.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace BookStore
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var status = AppConfig.GetValue(AppConfig.Status);
            if (status == "Login")
            {
                var screen = new MainWindow();

                screen.Show();

                this.Close();
            }
            else { 
                
            }
            
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordTextBox.Password.ToString();

            var passwordInBytes = Encoding.UTF8.GetBytes(password);

            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var entropyBase64 = Convert.ToBase64String(entropy);

            var cypherText = ProtectedData.Protect(passwordInBytes, entropy,
                DataProtectionScope.CurrentUser);
            var cypherTextBase64 = Convert.ToBase64String(cypherText);

            AppConfig.SetValue(AppConfig.Password, cypherTextBase64);
            AppConfig.SetValue(AppConfig.Entropy, entropyBase64);
            AppConfig.SetValue(AppConfig.Username, usernameTextBox.Text);

            Business _bus = null;
            string? connectionString = AppConfig.ConnectionString();
            var dao = new SqlDataAccess(connectionString!);
            if (dao.CanConnect())
            {
                dao.Connect();
                var screen = new MainWindow();
               
                screen.Show();
                this.Close();

                AppConfig.SetValue(AppConfig.Status, "Login");

            }
            else
            {
                MessageBox.Show("Wrong password. Cannot connect to db");
            }
           
        }
    }
}
