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
    public partial class BillingReport : Window
    {
        public BillingReport(string fileID,string firstName, string lastName, string assignedAttorney)
        {
            InitializeComponent();

            try
            {
            //Set the data context to a new instance of main window view model.
            MainWindowViewModel mainModel = new MainWindowViewModel(fileID );
            this.DataContext = mainModel;
            mainModel.PrintGrid(dgTransactionDetails,fileID ,firstName,lastName,assignedAttorney);
            Helper.ShowInformationMessageBox("Transaction Billing Report Exported.", "Billing Report");
            this.Close();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
    }
}
