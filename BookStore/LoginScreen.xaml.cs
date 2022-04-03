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
            try
            {
                var cypherText = AppConfig.GetValue(AppConfig.Password);
                var cypherTextInBytes = Convert.FromBase64String(cypherText!);

                var entropyText = AppConfig.GetValue(AppConfig.Entropy);
                var entropyTextInBytes = Convert.FromBase64String(entropyText);

                var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyTextInBytes, DataProtectionScope.CurrentUser);

                var password = Encoding.UTF8.GetString(passwordInBytes);
                MessageBox.Show("Last Login:" + AppConfig.GetValue(AppConfig.Username) + "," + password);
                var screen = new MainWindow();
                screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                screen.Owner = this;
                screen.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("First Login");
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
            if (usernameTextBox.Text == "admin" && passwordTextBox.Password.ToString() == "qwe3@1")

            {
                var screen = new MainWindow();
                screen.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                screen.Owner = this;
                screen.Show();
            }
        }
    }
}
