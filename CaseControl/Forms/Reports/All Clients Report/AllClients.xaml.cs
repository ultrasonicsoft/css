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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AllClientsReport : Window
    {
        public AllClientsReport()
        {
            InitializeComponent();

            RefreshGridView();
        }

        private void cmbClientStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshGridView();
        }

        private void RefreshGridView()
        {
            try
            {
                ComboBoxItem clientStatus = (ComboBoxItem)cmbClientStatus.SelectedItem;
                if (clientStatus != null && !string.IsNullOrEmpty(clientStatus.Content.ToString()))
                {
                    this.DataContext = new AllClientsReportMainViewModel(clientStatus.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
