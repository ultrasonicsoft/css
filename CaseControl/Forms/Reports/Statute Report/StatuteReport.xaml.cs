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
using System.Text.RegularExpressions;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StatuteReport : Window
    {
        public StatuteReport()
        {
            InitializeComponent();

            FillYearDropDownList();
            
            RefreshGridView();
        }

        private void FillYearDropDownList()
        {
            try
            {
            //cmbYear.ItemsSource = null;
            //System.Data.DataTable table = new System.Data.DataTable();
            //table.Columns.Add("Year");
            //for (int index = 0; index < 200; index++)
            //{
            //    table.Rows.Add(DateTime.Now.Year-index);
            //}
            //cmbYear.DisplayMemberPath = "Year";
            //cmbYear.SelectedValuePath = "Year";
            //cmbYear.ItemsSource = table.DefaultView;
            //cmbYear.SelectedIndex = 0;

            for (int index = 0; index < 200; index++)
            {
                cmbYear.Items.Add((DateTime.Now.Year - index).ToString());
            }
            cmbYear.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void cmbClientStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbClientStatus.Items.Count == 3)
            //{
            //    RefreshGridView();
            //}
        }

        private void RefreshGridView()
        {
            try
            {
            ComboBoxItem selectedMonth = (ComboBoxItem)cmbMonths.SelectedItem;
            string selectedYear = (string)cmbYear.SelectedItem;
            if (selectedMonth != null && !string.IsNullOrEmpty(selectedMonth.Content.ToString()) && selectedYear != null && !string.IsNullOrEmpty(selectedYear))
            {
                string sql = string.Format(Constants.STATUTE_60_DAYS_REPORT_QUERY,selectedMonth.Content.ToString(),selectedYear);
                dg60Days.ItemsSource = BusinessLogic.GetStatuteReportData(sql);

                sql = string.Format(Constants.STATUTE_180_DAYS_REPORT_QUERY, selectedMonth.Content.ToString(), selectedYear);
                dg180Days.ItemsSource = BusinessLogic.GetStatuteReportData(sql);

                sql = string.Format(Constants.STATUTE_1_YEARS_REPORT_QUERY, selectedMonth.Content.ToString(), selectedYear);
                dg1Year.ItemsSource = BusinessLogic.GetStatuteReportData(sql);

                sql = string.Format(Constants.STATUTE_2_YEARS_REPORT_QUERY, selectedMonth.Content.ToString(), selectedYear);
                dg2Years.ItemsSource = BusinessLogic.GetStatuteReportData(sql);

                sql = string.Format(Constants.STATUTE_3_YEARS_REPORT_QUERY, selectedMonth.Content.ToString(), selectedYear);
                dg3Years.ItemsSource = BusinessLogic.GetStatuteReportData(sql);

                sql = string.Format(Constants.STATUTE_5_YEARS_REPORT_QUERY, selectedMonth.Content.ToString(), selectedYear);
                dg5Years.ItemsSource = BusinessLogic.GetStatuteReportData(sql);
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == false)
                return;

            ComboBoxItem selectedMonth = (ComboBoxItem)cmbMonths.SelectedItem;
            string selectedYear = (string)cmbYear.SelectedItem;

            string documentTitle = "Statute Report For " + selectedMonth.Content.ToString() + " " + selectedYear ;
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);

            StatuteReportPrinter paginator = new StatuteReportPrinter(dg60Days as DataGrid, dg180Days as DataGrid,
                                                                        dg1Year as DataGrid, dg2Years as DataGrid, dg3Years as DataGrid, dg5Years as DataGrid,
                                                                        documentTitle, pageSize, new Thickness(30, 20, 30, 20));
            printDialog.PrintDocument(paginator, "Grid");

            Helper.ShowInformationMessageBox("Statute Report Exported!", "Statute Report");

            //var printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    var paginator = new RandomTabularPaginator(50,
            //      new Size(printDialog.PrintableAreaWidth,
            //        printDialog.PrintableAreaHeight));

            //    printDialog.PrintDocument(paginator, "My Random Data Table");
            //}
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnShowReport_Click(object sender, RoutedEventArgs e)
        {
            RefreshGridView();
        }

        private void cmbYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
