﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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

        public static string? GetValue(string key)
        {
            string? value = ConfigurationManager
            .AppSettings[key];
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
