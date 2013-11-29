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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.IO;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public ClientInformation clientInfoHandler = null;
        public ClientBilling clientBillingHandler = null;
        public Reports reportsHandler = null;

        public HomePage()
        {
            InitializeComponent();

          
        }
        private void CreateDatabaseStructure()
        {
            try
            {
                StreamReader strReader = new StreamReader(@"QueryFile.txt");
                string str;
                bool result = true;

                while ((str = strReader.ReadLine()) != null & result != false)
                {
                    if (str != "")
                    {
                        result = ExecuteQuery(str);
                        if (result == false)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ExecuteQuery(string sqlQuery)
        {
            string connection = @"server=(local)\SQLExpress;database=Master;integrated Security=SSPI;";
            SqlConnection con = new SqlConnection(connection);
            bool result = true;

            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        private void btnClientInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientInfoHandler = new ClientInformation();
                clientInfoHandler.HomePageHandler = this;
                clientInfoHandler.Show();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnDeleteClients_Click(object sender, RoutedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.ShowDialog();
        }

        private void btnBilling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientBillingHandler = new ClientBilling();
                clientBillingHandler.HomePageHandler = this;
                clientBillingHandler.Show();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                reportsHandler = new Reports();
                reportsHandler.HomePageHandler = this;
                reportsHandler.Show();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnConfigureDatase_Click(object sender, RoutedEventArgs e)
        {
            ConfigureDatabase configure = new ConfigureDatabase();
            configure.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string connectionString = string.Format(DBHelper.DB_CONNECTION.ConnectionString);
            //if (!DBHelper.IsDatabaseInstalled(connectionString))
            //{
            //    //CreateDatabaseStructure();
            //    //MessageBox.Show(Application.Current.MainWindow, "Database Configured successfully! Please Restart application.", "Case Control System", MessageBoxButton.OK, MessageBoxImage.Information);

            //    Environment.Exit(0);
            //}

        }
    }
}
