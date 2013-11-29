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
    public class AllClientsReportMainViewModel : BaseViewModel
    {
        public AllClientsReportMainViewModel(string clientStatus)
        {
            try
            {
                if (clientStatus == "All")
                {
                    this.ClientReportingData = BusinessLogic.GetClientReportData(Constants.ALL_CLIENTS_REPORT_QUERY, AssignedBy.AssignedAttorney);
                }
                else
                {
                    bool isActiveClient = clientStatus == "Active";
                    this.ClientReportingData = BusinessLogic.GetClientReportData(string.Format(Constants.ALL_CLIENTS_WITH_STATUS_REPORT_QUERY, isActiveClient.ToString()), AssignedBy.AssignedAttorney);
                }
                this.PrintCommand = new DelegateCommand(this.PrintGrid);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public ObservableCollection<CommonReportViewModel> ClientReportingData { get; set; }

        public ICommand PrintCommand { get; private set; }

        public void PrintGrid(object param)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();

                if (printDialog.ShowDialog() == false)
                    return;

                string documentTitle = "All Clients";
                Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

                CommonReportPrinter paginator = new CommonReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
                printDialog.PrintDocument(paginator, "Grid");

                Helper.ShowInformationMessageBox("All Clients report generated successfully!", "All Clients Report");
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

    }
}
