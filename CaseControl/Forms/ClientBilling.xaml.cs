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
using System.Data;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for ClientBilling.xaml
    /// </summary>
    public partial class ClientBilling : Window
    {
        public string SelectedFileNumber { get; set; }
        public HomePage HomePageHandler { get; set; }
        private ClientInformation clientInfoHandler = null;
        private Reports reportsHandler = null;

        #region Members

        private ListCollectionView m_ClientFileListForSearch;
        private ListCollectionView m_TransactionListForSearch;
        private string actionSave = string.Empty;
        #endregion

        #region Constructor

        public ClientBilling()
        {
            InitializeComponent();
        }

        #endregion

        #region Windows Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbActivityFilter.SelectedIndex = 0;
                FillClientFileList(Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY);
                if (dgClientFileList.Items.Count > 0)
                {
                    if (string.IsNullOrEmpty(SelectedFileNumber))
                    {
                        dgClientFileList.SelectedIndex = 0;
                    }
                    else
                    {
                        ClientFileInformation item = null;
                        bool isFileIDFound = false;
                        int index = 0;
                        for (index = 0; index < dgClientFileList.Items.Count; index++)
                        {
                            item = dgClientFileList.Items[index] as ClientFileInformation;
                            if (item.FileID.Equals(SelectedFileNumber))
                            {
                                isFileIDFound = true;
                                break;
                            }
                        }
                        dgClientFileList.SelectedIndex = isFileIDFound ? index : 0;
                    }
                }

                UpdateStatusGeneralSection(true);
                UpdatestatusTransactionDetail(true);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void UpdateStatusGeneralSection(bool isEnabled)
        {
            txtLastName.IsReadOnly = isEnabled;
            txtFirstName.IsReadOnly = isEnabled;
            txtFileNo.IsReadOnly = isEnabled;

            dtAccidentDate.IsEnabled = !isEnabled;
            txtAddress.IsReadOnly = isEnabled;
            txtCity.IsReadOnly = isEnabled;
            txtState.IsReadOnly = isEnabled;
            txtHomePhone.IsReadOnly = isEnabled;
            txtCellPhone.IsReadOnly = isEnabled;
            txtDrivingLicense.IsReadOnly = isEnabled;
            dtDateOfBirth.IsEnabled = !isEnabled;
            txtSSN.IsReadOnly = isEnabled;
            txtInitialCaseInformation.IsReadOnly = isEnabled;
            txtDefendantName.IsReadOnly = isEnabled;
            txtEvidence1.IsReadOnly = isEnabled;
            txtEvidence2.IsReadOnly = isEnabled;
            txtOriginatingAttorney.IsReadOnly = isEnabled;
            txtAssignedAttorney.IsReadOnly = isEnabled;
            txtRefferal.IsReadOnly = isEnabled;

        }

        private void UpdatestatusTransactionDetail(bool isEnabled)
        {
            //txtItemNo.IsReadOnly = isEnabled;
            dtTransactionDate.IsEnabled = !isEnabled;
            txtDescription.IsReadOnly = isEnabled;
            cmbBillingType.IsEnabled = !isEnabled;
            txtGeneralFund.IsReadOnly = isEnabled;
            txtTrustFund.IsReadOnly = isEnabled;
            txtCheckNo.IsReadOnly = isEnabled;

            btnSaveTransaction.IsEnabled = !isEnabled;
            btnCancelTransaction.IsEnabled = !isEnabled;
            btnDeleteTransaction.IsEnabled = !isEnabled;

            btnAddNewTransaction.IsEnabled = isEnabled;
            btnEditTransaction.IsEnabled = isEnabled;

            if (dgTransactionDetails.Items.Count > 0)
                dgTransactionDetails.SelectedIndex = 0;
        }

        #endregion

        #region Client File List Grid View

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

        #endregion

        #region Client File Search

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

        #endregion

        #region Reset Client File Grid and Filter

        private void ResetClientFileGridSelection(int selectedIndex = 0)
        {
            try
            {
                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = selectedIndex;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
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
        #endregion

        #region Show Client General details

        private void ShowClientGeneralInformation(ClientFileInformation selectedClient)
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_BASIC_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    txtFileNo.Text = selectedClient.FileID;
                    txtLastName.Text = selectedClient.LastName;
                    txtFirstName.Text = selectedClient.FirstName;
                    cmbCaseType.ItemsSource = null;
                    //cmbCaseType.Items.Clear();
                    //cmbCaseType.Items.Add(result.Tables[0].Rows[0][Constants.CASE_TYPE_COLUMN].ToString());
                    DataTable table = new DataTable();
                    table.Columns.Add("CaseType");
                    table.Rows.Add(result.Tables[0].Rows[0][Constants.CASE_TYPE_COLUMN].ToString());
                    cmbCaseType.DisplayMemberPath = "CaseType";
                    cmbCaseType.SelectedValuePath = "CaseType";
                    cmbCaseType.ItemsSource = table.DefaultView;

                    cmbCaseType.SelectedIndex = 0;

                    cmbCaseStatus.ItemsSource = null;
                    DataTable caseStatusTable = new DataTable();
                    caseStatusTable.Columns.Add("CaseStatus");
                    caseStatusTable.Rows.Add(result.Tables[0].Rows[0][Constants.CASE_STATUS_COLUMN].ToString());
                    cmbCaseStatus.DisplayMemberPath = "CaseStatus";
                    cmbCaseStatus.SelectedValuePath = "CaseStatus";
                    cmbCaseStatus.ItemsSource = caseStatusTable.DefaultView;
                    cmbCaseStatus.SelectedIndex = 0;
                    dtAccidentDate.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.ACCIDENT_DATE_COLUMN].ToString());
                }
                //Client General Address information
                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_BASIC_ADDRESS_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtAddress.Text = string.Empty;
                    txtCity.Text = string.Empty;
                    txtState.Text = string.Empty;
                    txtHomePhone.Text = string.Empty;
                    txtCellPhone.Text = string.Empty;
                    txtDrivingLicense.Text = string.Empty;
                    dtDateOfBirth.SelectedDate = DateTime.Now;
                    txtSSN.Text = string.Empty;
                }
                else
                {
                    txtAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_ADDRESS_COLUMN].ToString();
                    txtCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_CITY_COLUMN].ToString();
                    txtState.Text = result.Tables[0].Rows[0][Constants.CLIENT_STATE_COLUMN].ToString();
                    txtHomePhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_HOMEPHONE_COLUMN].ToString();
                    txtCellPhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_CELLPHONE_COLUMN].ToString();
                    txtDrivingLicense.Text = result.Tables[0].Rows[0][Constants.CLIENT_LICENSENUMBER_COLUMN].ToString();
                    dtDateOfBirth.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.CLIENT_DATEOFBIRTH_COLUMN].ToString());
                    txtSSN.Text = result.Tables[0].Rows[0][Constants.CLIENT_SSN_COLUMN].ToString();
                }
                //Client General Case Information
                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_BASIC_INSURANCE_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    //TODO: check what to do here
                    txtInitialCaseInformation.Text = string.Empty;
                    txtDefendantName.Text = string.Empty;
                    txtOriginatingAttorney.Text = string.Empty;
                    txtAssignedAttorney.Text = string.Empty;
                    txtRefferal.Text = string.Empty;
                }
                else
                {
                    txtInitialCaseInformation.Text = result.Tables[0].Rows[0][Constants.CLIENT_INITIALINFORMATION_COLUMN].ToString();
                    txtDefendantName.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANTNAME_COLUMN].ToString();
                    txtOriginatingAttorney.Text = result.Tables[0].Rows[0][Constants.CLIENT_ORIGINATINGATTORNY_COLUMN].ToString();
                    txtAssignedAttorney.Text = result.Tables[0].Rows[0][Constants.CLIENT_ASSIGNEDATTORNY_COLUMN].ToString();
                    txtRefferal.Text = result.Tables[0].Rows[0][Constants.CLIENT_REFERRAL_COLUMN].ToString();
                }

                //Client General Case Evidence Information
                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_BASIC_EVIDENCE_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtEvidence1.Text = string.Empty;
                    txtEvidence2.Text = string.Empty;
                }
                else
                {
                    if (result.Tables[0].Rows.Count == 1)
                    {
                        txtEvidence1.Text = result.Tables[0].Rows[0][Constants.CLIENT_EVIDENCE_COLUMN].ToString();
                        txtEvidence2.Text = string.Empty;
                    }
                    else if (result.Tables[0].Rows.Count >= 2)
                    {
                        txtEvidence1.Text = result.Tables[0].Rows[0][Constants.CLIENT_EVIDENCE_COLUMN].ToString();
                        txtEvidence2.Text = result.Tables[0].Rows[1][Constants.CLIENT_EVIDENCE_COLUMN].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Client File List Events

        private void dgClientFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClientFileInformation selectedClient = dgClientFileList.SelectedItem as ClientFileInformation;
                if (selectedClient == null)
                {
                    return;
                }
                ShowClientGeneralInformation(selectedClient);

                ShowClientTransactionInformation(selectedClient);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

      

        #endregion

        #region Client General Section Events
        private void btnViewAllEvidence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EvidenceList evidenceList = new EvidenceList();
                evidenceList.FileID = txtFileNo.Text;
                evidenceList.ShowDialog();
                ResetClientFileGridSelection();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Transaction Details

        private void ShowClientTransactionInformation(ClientFileInformation selectedClient)
        {
            try
            {
                var result = BusinessLogic.GetAllTransactionDetails(string.Format(Constants.CLIENT_TRANSACTION_DETAILS_QUERY, selectedClient.FileID));
                dgTransactionDetails.ItemsSource = result;
                m_TransactionListForSearch = new ListCollectionView(result);
                if (dgTransactionDetails.Items.Count > 0)
                {
                    dgTransactionDetails.SelectedIndex = 0;
                }
                else
                {
                    ClearControlValues();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ClearControlValues()
        {
            txtItemNo.Text = string.Empty;
            dtTransactionDate.SelectedDate = DateTime.Now;
            txtDescription.Text = string.Empty;
            cmbBillingType.SelectedIndex = -1;
            txtGeneralFund.Text = string.Empty;
            txtTrustFund.Text = string.Empty;
            txtCheckNo.Text = string.Empty;
            txtSearchTransaction.Text = string.Empty;
            lblGenTotal.Content = string.Empty;
            lblTrustTotal.Content = string.Empty;
            
        }
        #endregion

        #region Transaction Grid Events

        private void dgTransactionDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TransactionDetail selectedTransaction = dgTransactionDetails.SelectedItem as TransactionDetail;
                if (selectedTransaction == null)
                    return;

                txtItemNo.Text = selectedTransaction.TransactionID;
                dtTransactionDate.SelectedDate = DateTime.Parse(selectedTransaction.TransactionDate);
                txtDescription.Text = selectedTransaction.Description;

                DataTable table = new DataTable();
                table.Columns.Add("BillingType");
                table.Rows.Add(selectedTransaction.BillingType);
                cmbBillingType.DisplayMemberPath = "BillingType";
                cmbBillingType.SelectedValuePath = "BillingType";
                cmbBillingType.ItemsSource = table.DefaultView;

                cmbBillingType.SelectedIndex = 0;

                txtGeneralFund.Text = selectedTransaction.GeneralFund;
                txtTrustFund.Text = selectedTransaction.TrustFund;
                txtCheckNo.Text = selectedTransaction.CheckNo;

                if (dgTransactionDetails.Items.Count > 0)
                {
                    btnDeleteTransaction.IsEnabled = true;
                }
                UpdateAccountTotals();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Summary Account Totals

        private void UpdateAccountTotals()
        {
            try
            {
                var genTotalValue = DBHelper.GetScalarValue(string.Format(Constants.GENERAL_ACCOUNT_TOTAL_QUERY, txtFileNo.Text));
                if (genTotalValue == null)
                {
                    lblGenTotal.Content = "Total: 0.00";
                }
                else
                {
                    lblGenTotal.Content = "Total: " + float.Parse(genTotalValue.ToString()).ToString("0.00");
                }

                var trustTotalValue = DBHelper.GetScalarValue(string.Format(Constants.TRUST_ACCOUNT_TOTAL_QUERY, txtFileNo.Text));
                if (trustTotalValue == null)
                {
                    lblTrustTotal.Content = "Total: 0.00";
                }
                else
                {
                    lblTrustTotal.Content = "Total: " + float.Parse(trustTotalValue.ToString()).ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Search Transaction Methods

        private void txtSearchTransaction_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (m_TransactionListForSearch == null || txtSearchTransaction.Text == "Search Transaction")
                    return;
                if (txtSearchTransaction.Text != "")
                {
                    if (m_TransactionListForSearch.CanFilter)
                    {
                        m_TransactionListForSearch.Filter =
                                new Predicate<object>(ContainsTransaction);

                        FilterTransaction();
                    }
                    else
                    {
                        m_TransactionListForSearch.Filter = null;
                    }
                }
                else
                {
                    m_TransactionListForSearch.Filter = null;
                    FilterTransaction();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void txtSearchTransaction_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchTransaction.Text == "Search Transaction")
            {
                txtSearchTransaction.Text = string.Empty;
            }
        }

        private void txtSearchTransaction_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearchTransaction.Text == string.Empty)
            {
                txtSearchTransaction.Text = "Search Transaction";
            }
        }

        public void FilterTransaction()
        {
            try
            {
                dgTransactionDetails.ItemsSource = null;
                ObservableCollection<TransactionDetail> transactionList = new ObservableCollection<TransactionDetail>();

                foreach (TransactionDetail row in m_TransactionListForSearch)
                {
                    transactionList.Add(row);
                }
                dgTransactionDetails.ItemsSource = transactionList;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        public bool ContainsTransaction(object value)
        {
            try
            {
                TransactionDetail currentRow = value as TransactionDetail;

                if (dgTransactionDetails.Columns.Count > 1)
                {
                    //There is more than 1 column in DataGrid
                    //FileInformation currentRow = value as FileInformation;
                    if (currentRow != null)
                    {
                        if (currentRow.BillingType.ToString()
                                  .ToLower()
                                  .Contains(txtSearchTransaction.Text
                                                    .ToLower()) ||
                            currentRow.CheckNo.ToString()
                                  .ToLower()
                                  .Contains(txtSearchTransaction.Text
                                                    .ToLower()) ||
                                currentRow.TransactionDate.ToString()
                                  .ToLower()
                                  .Contains(txtSearchTransaction.Text
                                                    .ToLower()) ||
                                                      currentRow.TransactionID.ToString()
                                  .ToLower()
                                  .Contains(txtSearchTransaction.Text
                                                    .ToLower()) ||
                            currentRow.Description.ToString()
                                  .ToLower()
                                  .Contains(txtSearchTransaction.Text
                                                    .ToLower())) return true;
                    }
                }
                else
                {
                    if (currentRow.BillingType.ToString()
                                   .ToLower()
                                   .Contains(txtSearchTransaction.Text
                                                     .ToLower()) ||
                             currentRow.CheckNo.ToString()
                                   .ToLower()
                                   .Contains(txtSearchTransaction.Text
                                                     .ToLower()) ||
                                 currentRow.TransactionDate.ToString()
                                   .ToLower()
                                   .Contains(txtSearchTransaction.Text
                                                     .ToLower()) ||
                                                       currentRow.TransactionID.ToString()
                                   .ToLower()
                                   .Contains(txtSearchTransaction.Text
                                                     .ToLower()) ||
                             currentRow.Description.ToString()
                                   .ToLower()
                                   .Contains(txtSearchTransaction.Text
                                                     .ToLower())) return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return false;
        }

        #endregion

        #region Add, Edit, Save, Cancel, Delete Transactions

        private void btnAddNewTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdatestatusTransactionDetail(false);

                txtItemNo.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtGeneralFund.Text = string.Empty;
                txtTrustFund.Text = string.Empty;
                txtCheckNo.Text = string.Empty;

                btnCancelTransaction.IsEnabled = true;
                btnSaveTransaction.IsEnabled = true;
                btnAddNewTransaction.IsEnabled = false;
                btnEditTransaction.IsEnabled = false;

                txtItemNo.Text = (BusinessLogic.GetNewTransactionID() + 1).ToString();
                actionSave = "Add";
                FillBillingTypeDropDown();
                dtTransactionDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnSaveTransaction_Click(object sender, RoutedEventArgs e)
        {
            string query = string.Empty;
            if (actionSave == "Add")
            {
                SaveNewTransaction();
            }
            else if (actionSave == "Edit")
            {
                EditTransaction();
            }
            UpdatestatusTransactionDetail(true);

        }

        private void EditTransaction()
        {
            try
            {
                TransactionDetail newTransaction = GetNewTransactionDetails();

                bool result = BusinessLogic.UpdateTransaction(newTransaction);
                if (result)
                {
                    Helper.ShowInformationMessageBox("Transaction Saved successfully!", "Client Billing");
                    ResetClientFileGridSelection();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveNewTransaction()
        {
            try
            {
                TransactionDetail newTransaction = GetNewTransactionDetails();
                bool result = BusinessLogic.SaveNewTransaction(newTransaction);
                if (result)
                {
                    //Helper.ShowInformationMessageBox("New Transaction Saved successfully!", "Client Billing");
                    ResetClientFileGridSelection(dgClientFileList.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        private TransactionDetail GetNewTransactionDetails()
        {
            TransactionDetail newTransaction = new TransactionDetail();
            try
            {
                newTransaction.TransactionID = txtItemNo.Text;
                newTransaction.TransactionDate = dtTransactionDate.SelectedDate.Value.ToShortDateString();

                newTransaction.BillingType = cmbBillingType.SelectedValue.ToString();

                newTransaction.GeneralFund = txtGeneralFund.Text;
                if (newTransaction.BillingType.Equals("General Account Credit") && !txtGeneralFund.Text.Contains("-"))
                {
                    newTransaction.GeneralFund = "-" + txtGeneralFund.Text;

                }
                if (!string.IsNullOrEmpty(newTransaction.GeneralFund))
                {
                    float genAcc = float.Parse(newTransaction.GeneralFund);
                    newTransaction.GeneralFund = genAcc.ToString("0.00");
                }
                newTransaction.TrustFund = txtTrustFund.Text;

                if (newTransaction.BillingType.Equals("Trust Account Debit") && !txtTrustFund.Text.Contains("-"))
                {
                    newTransaction.TrustFund = "-" + txtTrustFund.Text;

                }
                if (!string.IsNullOrEmpty(newTransaction.TrustFund))
                {
                    float trustAcc = float.Parse(newTransaction.TrustFund);
                    newTransaction.TrustFund = trustAcc.ToString("0.00");
                }
                newTransaction.CheckNo = txtCheckNo.Text;
                newTransaction.Description = txtDescription.Text;
                newTransaction.FileID = txtFileNo.Text;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return newTransaction;
        }
        private void btnCancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            UpdatestatusTransactionDetail(true);
        }

        private void btnEditTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdatestatusTransactionDetail(false);

                btnCancelTransaction.IsEnabled = true;
                btnSaveTransaction.IsEnabled = true;
                btnAddNewTransaction.IsEnabled = false;
                btnEditTransaction.IsEnabled = false;

                actionSave = "Edit";
                object previousBillingType = cmbBillingType.SelectedValue;
                FillBillingTypeDropDown();
                cmbBillingType.SelectedValue = previousBillingType;
                dtTransactionDate.SelectedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void FillBillingTypeDropDown()
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(Constants.ALL_BILLING_TYPE);
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                cmbBillingType.ItemsSource = null;
                cmbBillingType.DisplayMemberPath = result.Tables[0].Columns[0].ToString();
                cmbBillingType.SelectedValuePath = result.Tables[0].Columns[0].ToString();
                cmbBillingType.ItemsSource = result.Tables[0].DefaultView;
                cmbBillingType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void cmbBillingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbBillingType.SelectedValue == null)
                    return;

                if (cmbBillingType.SelectedValue.ToString().Equals("General Account Debit") || cmbBillingType.SelectedValue.ToString().Equals(("General Account Credit")))
                {
                    txtGeneralFund.IsReadOnly = false;
                    txtTrustFund.IsReadOnly = true;
                    txtTrustFund.Text = string.Empty;
                }

                else if (cmbBillingType.SelectedValue.ToString().Equals("Trust Account Debit") || cmbBillingType.SelectedValue.ToString().Equals(("Trust Account Credit")))
                {
                    txtGeneralFund.IsReadOnly = true;
                    txtTrustFund.IsReadOnly = false;
                    txtGeneralFund.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnDeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = Helper.ShowQuestionMessageBox("Are you sure to delete this transaction?", "Client Billing");
                if (result == MessageBoxResult.Yes)
                {
                    string query = string.Format(Constants.DELETE_TRANSACTION_QUERY, txtFileNo.Text, txtItemNo.Text);
                    DBHelper.ExecuteNonQuery(query);
                    ResetClientFileGridSelection();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Validations

        private void txtGeneralFund_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private void txtTrustFund_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            try
            {
                if (e.DataObject.GetDataPresent(typeof(String)))
                {
                    String text = (String)e.DataObject.GetData(typeof(String));
                    if (!IsTextAllowed(text))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    e.CancelCommand();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Print Report

        private void btnPrintPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BillingReport report = new BillingReport(txtFileNo.Text, txtFirstName.Text, txtLastName.Text, txtAssignedAttorney.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        private void btnClientReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (reportsHandler == null)
                {
                    reportsHandler = new Reports();
                }
                if (reportsHandler.WindowState == System.Windows.WindowState.Minimized)
                {
                    reportsHandler.WindowState = System.Windows.WindowState.Normal;
                    reportsHandler.Activate();
                }
                reportsHandler.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnClientInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clientInfoHandler == null)
                {
                    clientInfoHandler = new ClientInformation();
                }
                clientInfoHandler.SelectedFileNumber = txtFileNo.Text;

                if (clientInfoHandler.WindowState == System.Windows.WindowState.Minimized)
                {
                    clientInfoHandler.WindowState = System.Windows.WindowState.Maximized;
                    clientInfoHandler.Activate();
                }
                clientInfoHandler.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (HomePageHandler != null)
            {
                HomePageHandler.Activate();
                HomePageHandler.WindowState = System.Windows.WindowState.Normal;
                HomePageHandler.clientBillingHandler = null;
            }
            this.Close();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (HomePageHandler == null)
            {
                HomePageHandler = new HomePage();
            }
                HomePageHandler.Activate();
                HomePageHandler.WindowState = System.Windows.WindowState.Normal;
                this.Close();
        }

    }
}
