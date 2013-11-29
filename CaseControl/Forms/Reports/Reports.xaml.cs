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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        public HomePage HomePageHandler { get; set; }
        private ClientInformation clientInfoHandler = null;
        private ClientBilling clientBillingHandler = null;

        public Reports()
        {
            InitializeComponent();
        }

        private void btnByAssignedAttorney_Click(object sender, RoutedEventArgs e)
        {
            AssignedAttorneyReport report = new AssignedAttorneyReport();
            report.Show();
        }

        private void btnByAttorney_Click(object sender, RoutedEventArgs e)
        {
            OriginatingAttorneyReport report = new OriginatingAttorneyReport();
            report.Show();
        }

        private void btnByReferral_Click(object sender, RoutedEventArgs e)
        {
            ReferralReport report = new ReferralReport();
            report.Show();
        }

        private void btnAllClients_Click(object sender, RoutedEventArgs e)
        {
            AllClientsReport report = new AllClientsReport();
            report.Show();
        }

        private void btnByPrintGenTrustAccountsTotal_Click(object sender, RoutedEventArgs e)
        {
            AllClientBillingReport report = new AllClientBillingReport();
            report.Show();
        }

        private void btnStatuteCalendarReport_Click(object sender, RoutedEventArgs e)
        {
            StatuteReport report = new StatuteReport();
            report.Show();
        }

        private void btnClientInformation_Click(object sender, RoutedEventArgs e)
        {
            if (clientInfoHandler == null)
            {
                clientInfoHandler = new ClientInformation();
            }
            if (clientInfoHandler.WindowState == System.Windows.WindowState.Minimized)
            {
                clientInfoHandler.WindowState = System.Windows.WindowState.Maximized;
                clientInfoHandler.Activate();
            }
            clientInfoHandler.Show();
            this.Close();
        }

        private void btnClientBilling_Click(object sender, RoutedEventArgs e)
        {
            if (clientBillingHandler == null)
            {
                clientBillingHandler = new ClientBilling();
            }
            if (clientBillingHandler.WindowState == System.Windows.WindowState.Minimized)
            {
                clientBillingHandler.WindowState = System.Windows.WindowState.Maximized;
                clientBillingHandler.Activate();
            }
            clientBillingHandler.Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (HomePageHandler != null)
            {
                HomePageHandler.Activate();
                HomePageHandler.WindowState = System.Windows.WindowState.Normal;
                HomePageHandler.reportsHandler = null;
            }
            this.Close();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (HomePageHandler == null)
            {
                HomePageHandler = new HomePage();
            }
            HomePageHandler.WindowState = System.Windows.WindowState.Normal;
            HomePageHandler.Activate();
            this.Close();
        }

        private void btnExpandAll_Click(object sender, RoutedEventArgs e)
        {
            bool action = false;
            if (btnExpandAll.Content.ToString() == "Expand All")
            {
                btnExpandAll.Content = "Collapse All";
                action = true;
            }
            else
            {
                btnExpandAll.Content = "Expand All";
            }
            exdPrintClientListing.IsExpanded = action;
            exdClientBilling.IsExpanded = action;
            exdStatuteCalendar.IsExpanded = action;
        }
    }
}
