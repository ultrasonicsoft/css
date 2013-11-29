using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using CaseControl;

namespace CaseControl
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel(string fileID)
        {
            try
            {
                this.BillingReportData = BusinessLogic.GetAllBillingTransactionDetails(string.Format(Constants.CLIENT_TRANSACTION_DETAILS_QUERY, fileID));
                //this.PrintCommand = new DelegateCommand(this.PrintGrid);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public ObservableCollection<BillingReportViewModel> BillingReportData { get; set; }

        public ICommand PrintCommand { get; private set; }

        public void PrintGrid(object param, string fileID, string firstName, string lastName, string assignedAttorney)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == false)
                    return;

                string documentTitle = "Client Billing Report";
                Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

                BillingReportPrinter paginator = new BillingReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20), fileID, firstName, lastName, assignedAttorney);
                printDialog.PrintDocument(paginator, "Grid");
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

    }
}
