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
    public partial class AssignedAttorneyReport : Window
    {
        public AssignedAttorneyReport()
        {
            InitializeComponent();

            //Set the data context to a new instance of main window view model.
            this.DataContext = new AssignedAttorneyMainViewModel(string.Empty);
            FillAllAssignedAttorney();
            //mainModel.PrintGrid(dgTransactionDetails,fileID ,firstName,lastName,assignedAttorney);
            //Helper.ShowInformationMessageBox("Assigned Attorney Report Exported.", "Assigned Attorney Report");
            //this.Close();
        }

        private void FillAllAssignedAttorney()
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(Constants.ALL_ASSIGNED_ATTORNEY_QUERY);
                if (result == null || result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                cmbAssignedAttorney.DisplayMemberPath = "AssignedAttorny";
                cmbAssignedAttorney.SelectedValuePath = "AssignedAttorny";
                cmbAssignedAttorney.ItemsSource = result.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void cmbAssignedAttorney_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAssignedAttorney.SelectedValue != null)
                {
                    this.DataContext = new AssignedAttorneyMainViewModel(cmbAssignedAttorney.SelectedValue.ToString());
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
                this.DataContext = new AssignedAttorneyMainViewModel(string.Empty);
                FillAllAssignedAttorney();
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
