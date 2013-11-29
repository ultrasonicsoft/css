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
    public class StatuteReportMainWindowViewModel : BaseViewModel
    {
        public StatuteReportMainWindowViewModel(string clientStatus)
        {
            try
            {
                if (clientStatus == "All")
                {
                    //this.BillingReportData = BusinessLogic.GetAllClientBillingTransactionDetails(Constants.ALL_CLIENTS_TRANSACTION_REPORT_QUERY);
                }
                else
                {
                    bool isActiveClient = clientStatus == "Active";
                    //this.BillingReportData = BusinessLogic.GetAllClientBillingTransactionDetails(string.Format(Constants.ALL_CLIENTS_TRANSACTION_WITH_STATUS_REPORT_QUERY, isActiveClient.ToString()));
                }

                this.PrintCommand = new DelegateCommand(this.PrintGrid);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public ObservableCollection<StatuteReportViewModel> SixtyDaysReportData { get; set; }
        public ObservableCollection<StatuteReportViewModel> OneYearReportData { get; set; }
        public ObservableCollection<StatuteReportViewModel> TwoYearsReportData { get; set; }
        public ObservableCollection<StatuteReportViewModel> ThreeYearsReportData { get; set; }
        public ObservableCollection<StatuteReportViewModel> FiveYearsReportData { get; set; }

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
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            //StatuteReportPrinter paginator = new StatuteReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
            //printDialog.PrintDocument(paginator, "Grid");
        }

    }
}
