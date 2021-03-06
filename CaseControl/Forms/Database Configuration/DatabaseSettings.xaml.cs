﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Data.SqlClient;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for DatabaseSettings.xaml
    /// </summary>
    public partial class DatabaseSettings : Window
    {
        public bool IsDatabaseConfigured { get; set; }

        public DatabaseSettings()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtDatabaseServerPath.Text = dialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (ConfigureDatabaseServer())
            if (string.IsNullOrEmpty(txtDatabaseServerPath.Text))
            {
                Helper.ShowErrorMessageBox("Please provide server name or IP address to configure database");
                return;
            }
            try
            {
                Properties.Settings.Default.ConnectionString = string.Format(System.Configuration.ConfigurationSettings.AppSettings["defaultConStr"], txtDatabaseServerPath.Text, txtDatabaseName.Text, txtUserName.Text, txtPassword.Password);
                Properties.Settings.Default.Save();

                DBHelper.ConfigureConnectionString();

                Helper.ShowInformationMessageBox("Database server configured successfully!", "Configure Database Server");
                IsDatabaseConfigured = true;
                this.Close();
            }
            catch(Exception ex)
            {
                Helper.LogException(ex);
                Helper.ShowErrorMessageBox("Database server configuration failed! Please contact your vendor.", "Configure Database Server");
                Environment.Exit(0);
            }
        }


        private bool ConfigureDatabaseServer()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtDatabaseServerPath.Text))
            {
                Helper.ShowErrorMessageBox("Please select database server folder!", "Configure Database Server");
                return result;
            }
            string sourceDatabaseFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                                                            System.IO.Path.DirectorySeparatorChar.ToString() + Constants.DATABASE_FOLDER_NAME +
                                                            System.IO.Path.DirectorySeparatorChar.ToString() + Constants.DATABASE_FILE_NAME;
            if (!File.Exists(sourceDatabaseFile))
            {
                Helper.ShowErrorMessageBox("Source database not found!", "Configure Database Server");
                return result;
            }
            string targetFileName = txtDatabaseServerPath.Text + System.IO.Path.DirectorySeparatorChar.ToString() + Constants.DATABASE_FILE_NAME;
            bool overwriteFlag = false;

            if (File.Exists(targetFileName))
            {
                var userResult = Helper.ShowQuestionMessageBox("Previous database is already present. Do you want to overwrite?", "Configure Database Server", MessageBoxButton.YesNo);
                overwriteFlag = userResult == MessageBoxResult.Yes;
                if (!overwriteFlag)
                {
                    Helper.ShowInformationMessageBox("Please take backup of existing database and try configuring database again by restarting application.", "Configure Database Server");
                    Environment.Exit(0);
                }
            }
            File.Copy(sourceDatabaseFile, targetFileName, overwriteFlag);

            string connectionString = string.Format(Constants.CONNECTION_STRING, targetFileName);
            result = DBHelper.TestConnection(connectionString);

            if (result)
            {
                string dbsettingFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + System.IO.Path.DirectorySeparatorChar.ToString() + Constants.DATABASE_CONFIG;
                File.WriteAllText(dbsettingFile, connectionString);

                DBHelper.ConfigureConnectionString();
            }
            return result;
        }
    }
}
