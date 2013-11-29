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
    public class AllClientBillingMainWindowViewModel : BaseViewModel
    {
        public AllClientBillingMainWindowViewModel(string clientStatus)
        {
            try
            {
                if (clientStatus == "All")
                {
                    this.BillingReportData = BusinessLogic.GetAllClientBillingTransactionDetails(Constants.ALL_CLIENTS_TRANSACTION_REPORT_QUERY);
                }
                else
                {
                    bool isActiveClient = clientStatus == "Active";
                    this.BillingReportData = BusinessLogic.GetAllClientBillingTransactionDetails(string.Format(Constants.ALL_CLIENTS_TRANSACTION_WITH_STATUS_REPORT_QUERY, isActiveClient.ToString()));
                }

                this.PrintCommand = new DelegateCommand(this.PrintGrid);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public ObservableCollection<AllClientBillingReportViewModel> BillingReportData { get; set; }

        public ICommand PrintCommand { get; private set; }

        public void PrintGrid(object param)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == false)
                    return;

                string documentTitle = "All Clients Billing Report";
                Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

                AllClientBillingReportPrinter paginator = new AllClientBillingReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
                printDialog.PrintDocument(paginator, "Grid");
                Helper.ShowInformationMessageBox("All Clients Billing Report Exported.", "All Clients Billing Report");
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

    }
}
