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
    public class AssignedAttorneyMainViewModel : BaseViewModel
    {
        public AssignedAttorneyMainViewModel(string assignedAttorney)
        {
            this.ClientReportingData = BusinessLogic.GetClientReportData(string.Format(Constants.BY_ASSIGNED_ATTORNEY_REPORT_QUERY,assignedAttorney),AssignedBy.AssignedAttorney);
            this.PrintCommand = new DelegateCommand(this.PrintGrid);
        }

        public ObservableCollection<CommonReportViewModel> ClientReportingData { get; set; }

        public ICommand PrintCommand { get; private set; }

        public void PrintGrid(object param)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == false)
                return;

            string documentTitle = "Report By Assigned Attorney";
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            CommonReportPrinter paginator = new CommonReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
            printDialog.PrintDocument(paginator, "Grid");

            Helper.ShowInformationMessageBox("Report By Assigned Attorney Exported", "By Assigned Attorney Report");

        }

    }
}
