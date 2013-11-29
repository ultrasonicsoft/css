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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for DeleteClients.xaml
    /// </summary>
    public partial class DeleteClients : Window
    {
        private ListCollectionView m_ClientFileListForSearch;

        public DeleteClients()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbActivityFilter.SelectedIndex = 0;
                FillClientFileList(Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY);
                if (dgClientFileList.Items.Count > 0)
                {
                    dgClientFileList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void FillClientFileList(string sqlclientFileList)
        {
            try
            {
                var fileList = BusinessLogic.GetAllClientNameFileID(sqlclientFileList);
                dgClientFileList.ItemsSource = fileList;
                m_ClientFileListForSearch = new ListCollectionView(fileList);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public bool ContainsIt(object value)
        {
            try
            {
                ClientFileInformation currentRow = value as ClientFileInformation;

                if (dgClientFileList.Columns.Count > 1)
                {
                    //There is more than 1 column in DataGrid
                    //FileInformation currentRow = value as FileInformation;
                    if (currentRow != null)
                    {
                        if (currentRow.FirstName.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower()) ||
                            currentRow.LastName.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower()) ||
                            currentRow.FileID.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower())) return true;
                    }
                }
                else
                {
                    if (currentRow.FirstName.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower()) ||
                            currentRow.LastName.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower()) ||
                            currentRow.FileID.ToString()
                                  .ToLower()
                                  .Contains(txtSearch.Text
                                                    .ToLower())) return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return false;
        }

        public void FilterIt()
        {
            try
            {
                dgClientFileList.ItemsSource = null;
                ObservableCollection<ClientFileInformation> fileList = new ObservableCollection<ClientFileInformation>();

                foreach (ClientFileInformation row in m_ClientFileListForSearch)
                {
                    fileList.Add(row);
                }
                dgClientFileList.ItemsSource = fileList;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (m_ClientFileListForSearch == null || txtSearch.Text == "Search")
                    return;
                if (txtSearch.Text != "")
                {
                    if (m_ClientFileListForSearch.CanFilter)
                    {
                        m_ClientFileListForSearch.Filter =
                                new Predicate<object>(ContainsIt);

                        FilterIt();
                    }
                    else
                    {
                        m_ClientFileListForSearch.Filter = null;
                    }
                }
                else
                {
                    m_ClientFileListForSearch.Filter = null;
                    FilterIt();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search")
            {
                txtSearch.Text = string.Empty;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == string.Empty)
            {
                txtSearch.Text = "Search";
            }
        }

        private void cmbActivityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshClientFileList();
        }

        private void RefreshClientFileList()
        {
            try
            {
                ComboBoxItem typeItem = (ComboBoxItem)cmbActivityFilter.SelectedItem;
                string selectedValue = typeItem.Content.ToString();
                string query = string.Empty;
                switch (selectedValue)
                {
                    case "Active":
                        query = Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY;
                        break;
                    case "Inactive":
                        query = Constants.INACTIVE_CLIENT_NAME_FILE_ID_QUERY;
                        break;
                    case "All":
                        query = Constants.ALL_CLIENT_NAME_FILE_ID_QUERY;
                        break;
                }
                FillClientFileList(query);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientFileInformation selectedClient = dgClientFileList.SelectedItem as ClientFileInformation;
                if (selectedClient == null)
                    return;

                bool result = BusinessLogic.DeleteClient(selectedClient.FileID);

                Helper.ShowInformationMessageBox("Selected Client Deleted");
                RefreshClientFileList();
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
