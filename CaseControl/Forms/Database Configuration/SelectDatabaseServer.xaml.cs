using System;
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

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for SelectDatabaseServer.xaml
    /// </summary>
    public partial class SelectDatabaseServer : Window
    {
        internal bool IsDatabaseConfigured { get; set; }

        public SelectDatabaseServer()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.FileName = Constants.DATABASE_FILE_NAME;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDatabaseServerPath.Text = dialog.FileName;
            }
        }

        private void btnConnectToServer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDatabaseServerPath.Text))
            {
                Helper.ShowErrorMessageBox("Please provide server name/IP address");
                return;
            }
            try
            {
                Properties.Settings.Default.ConnectionString = string.Format(System.Configuration.ConfigurationSettings.AppSettings["defaultConStr"], txtDatabaseServerPath.Text,txtDatabaseName.Text,txtUserName.Text,txtPassword.Password);
                Properties.Settings.Default.Save();
                
                DBHelper.ConfigureConnectionString();

                Helper.ShowInformationMessageBox("Database server configured successfully!", "Connect To Database Server");
                IsDatabaseConfigured = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
                Helper.ShowErrorMessageBox("Database server configuration failed! Please contact your vendor.", "Connect To Database Server");
                Environment.Exit(0);
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
