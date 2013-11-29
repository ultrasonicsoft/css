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
    public class ReferralAttorneyMainViewModel : BaseViewModel
    {
        public ReferralAttorneyMainViewModel(string referralAttorney)
        {
            this.ClientReportingData = BusinessLogic.GetClientReportData(string.Format(Constants.BY_REFERRAL_CLIENT_REPORT_QUERY, referralAttorney), AssignedBy.Referral);
            this.PrintCommand = new DelegateCommand(this.PrintGrid);
        }

        public ObservableCollection<CommonReportViewModel> ClientReportingData { get; set; }

        public ICommand PrintCommand { get; private set; }

        public void PrintGrid(object param)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == false)
                return;

            string documentTitle = "Report By Referral Attorney";
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            CommonReportPrinter paginator = new CommonReportPrinter(param as DataGrid, documentTitle, pageSize, new Thickness(30, 20, 30, 20));
            printDialog.PrintDocument(paginator, "Grid");

            Helper.ShowInformationMessageBox("Referral Report Exported", "Referral Report");
        }

    }
}
