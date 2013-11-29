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
    /// Interaction logic for DatabaseSettings.xaml
    /// </summary>
    public partial class ConfigureDatabase : Window
    {
        public bool IsDatabaseReady { get; set; }

        public ConfigureDatabase()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //TODO: check status
            this.Close();
        }

        private void btnConnectToDatabaseServer_Click(object sender, RoutedEventArgs e)
        {
            SelectDatabaseServer configueDB = new SelectDatabaseServer();
            configueDB.ShowDialog();
            IsDatabaseReady = configueDB.IsDatabaseConfigured;
            this.Close();
        }

        private void btnConfigureDatabaseServer_Click(object sender, RoutedEventArgs e)
        {
            DatabaseSettings settings = new DatabaseSettings();
            settings.ShowDialog();
            IsDatabaseReady = settings.IsDatabaseConfigured;
            this.Close();
        }
        
    }
}
