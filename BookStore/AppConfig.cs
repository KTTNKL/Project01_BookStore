using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStore
{
    class AppConfig
    {
        public static string Server = "Server";
        public static string Instance = "Instance";
        public static string Database = "Database";
        public static string Username = "Username";
        public static string Password = "Password";
        public static string Entropy = "Entropy";
        public static string Status = "Status";
        public static string Page = "Page";

        public static string? GetValue(string key)
        {
            string? value = ConfigurationManager.AppSettings[key];
            return value;
        }

        public static void SetValue(string key, string value)
        {
            var configFile = ConfigurationManager
            .OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings.Remove(key);
            settings.Add(key, value);
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        public static string? ConnectionString()
        {
            string? result = "";

            var builder = new SqlConnectionStringBuilder();
            string? server = AppConfig.GetValue(AppConfig.Server);
            string? instance = AppConfig.GetValue(AppConfig.Instance);
            string? database = AppConfig.GetValue(AppConfig.Database);
            string? username = AppConfig.GetValue(AppConfig.Username);

            var cypherText = AppConfig.GetValue(AppConfig.Password);
            var cypherTextInBytes = Convert.FromBase64String(cypherText!);

            var entropyText = AppConfig.GetValue(AppConfig.Entropy);
            var entropyTextInBytes = Convert.FromBase64String(entropyText);

            var passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                entropyTextInBytes, DataProtectionScope.CurrentUser);


            string? password = Encoding.UTF8.GetString(passwordInBytes);

            builder.DataSource = $"{server}\\{instance}";
            builder.InitialCatalog = database;
            builder.IntegratedSecurity = true;
            builder.ConnectTimeout = 3; // s
            builder.UserID = username;
            builder.Password = password;
            result = builder.ToString();
            return result;
        }
        public static string? ConnectionString2()
        {
            string? result = "";
            string? instance = "sqlexpress";
            var builder = new SqlConnectionStringBuilder();
            string? server = AppConfig.GetValue(AppConfig.Server);
            string value = AppConfig.GetValue(AppConfig.Instance);
            if (value == "sqlexpress")
            {
                instance = "SQLEXPRESS";
            }
            else if (value == "SQLEXPRESS") {
                instance = "sqlexpress";
            }
            string? database = AppConfig.GetValue(AppConfig.Database);
            string? username = AppConfig.GetValue(AppConfig.Username);
            string? password = AppConfig.GetValue(AppConfig.Password);

            builder.DataSource = $"{server}\\{instance}";
            builder.InitialCatalog = database;
            builder.IntegratedSecurity = true;
            builder.ConnectTimeout = 3; // s

            result = builder.ToString();
            return result;
        }

    }
}
