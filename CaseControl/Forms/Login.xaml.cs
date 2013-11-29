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

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            bool isValidUser = BusinessLogic.IsValidUser(txtUserName.Text, txtPassword.Password);
            if (isValidUser)
            {
                DeleteClients deleteClientForm = new DeleteClients();
                deleteClientForm.Show();
                this.Close();
            }
            else
            {
                Helper.ShowErrorMessageBox("Invalid user name or password!");
            }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }
    }
}
