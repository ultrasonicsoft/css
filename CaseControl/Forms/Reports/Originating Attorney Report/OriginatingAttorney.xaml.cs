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
    public partial class OriginatingAttorneyReport : Window
    {
        public OriginatingAttorneyReport()
        {
            InitializeComponent();

            //Set the data context to a new instance of main window view model.
            this.DataContext = new OriginatingAttorneyMainViewModel(string.Empty);
            FillAllOriginatingAttorney();
            //mainModel.PrintGrid(dgTransactionDetails,fileID ,firstName,lastName,assignedAttorney);
            //Helper.ShowInformationMessageBox("Assigned Attorney Report Exported.", "Assigned Attorney Report");
            //this.Close();
        }

        private void FillAllOriginatingAttorney()
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(Constants.ALL_ORIGINATING_ATTORNEY_QUERY);
                if (result == null || result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                cmbOriginatingAttorney.DisplayMemberPath = "OriginatingAttorny";
                cmbOriginatingAttorney.SelectedValuePath = "OriginatingAttorny";
                cmbOriginatingAttorney.ItemsSource = result.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void cmbOriginatingAttorney_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOriginatingAttorney.SelectedValue != null)
                {
                    this.DataContext = new OriginatingAttorneyMainViewModel(cmbOriginatingAttorney.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnListAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = new OriginatingAttorneyMainViewModel(string.Empty);
                FillAllOriginatingAttorney();
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
