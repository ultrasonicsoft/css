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
using System.Data;
using System.Text.RegularExpressions;

namespace CaseControl
{
    /// <summary>
    /// Interaction logic for ClientInformation.xaml
    /// </summary>
    public partial class ClientInformation : Window
    {
        public HomePage HomePageHandler { get; set; }
        private ClientBilling clientBillingHandler = null;
        private Reports reportsHandler = null;
        public Style textBoxNormalStyle, textBoxErrorStyle;
        private FrameworkElement frameworkElement;

        #region Data Members

        private ListCollectionView m_ClientFileListForSearch;
        private string actionClientMedicalInsuranceNote = string.Empty;
        private string actionMiscNote = string.Empty;
        private string actionMainClient = string.Empty;

        ClientGeneralInformation selectedGeneralInfo = new ClientGeneralInformation();

        #endregion

        #region Properties

        public string SelectedFileNumber { get; set; }

        #endregion

        #region Constructor

        public ClientInformation()
        {
            InitializeComponent();

            try
            {
                frameworkElement = new FrameworkElement();
                textBoxNormalStyle = (Style)frameworkElement.TryFindResource("textBoxNormalStyle");
                textBoxErrorStyle = (Style)frameworkElement.TryFindResource("textBoxErrorStyle");
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Windows Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbActivityFilter.SelectedIndex = 0;
                FillClientFileList(Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY);
                if (dgClientFileList.Items.Count > 0)
                {
                    //dgClientFileList.SelectedIndex = 0;
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

                UpdateAllControlsStatus(true);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Private Methods

        #region Grid and Search Methods

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
        #endregion

        #region Select Methods - Display Client Information

        private void DisplaySelectedClientInformation(ClientFileInformation selectedClient)
        {
            ShowClientGeneralInformation(selectedClient);
            ShowClientAdditionalInformation(selectedClient);
            ShowClientAutoInformation(selectedClient);
            ShowClientMedicalInsuranceInformation(selectedClient);
            ShowClientDefendantDetailsInformation(selectedClient);
            ShowDefendantInsuranceInformation(selectedClient);
            ShowStatuteInformation(selectedClient);
            ShowClientCourtInformation(selectedClient);
            ShowClientMiscNotes(selectedClient);
        }

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

                    txtClientCreated.Text = selectedClient.ClientCreatedOn;

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

                    txtSuiteAddress.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtWorkPhone.Text = string.Empty;
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

                    txtWorkPhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_WORKPHONE_COLUMN].ToString();
                    txtSuiteAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_SUITEADDRESS_COLUMN].ToString();
                    txtEmail.Text = result.Tables[0].Rows[0][Constants.CLIENT_EMAIL_COLUMN].ToString();
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

        private void ShowClientAdditionalInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client General Addtional information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_ADDITIONAL_INFO_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtClientOccupation.Text = string.Empty;
                    txtEmployer.Text = string.Empty;
                    txtClientEmployerAddress.Text = string.Empty;
                    txtClientEmployerCity.Text = string.Empty;
                    txtClientEmployerState.Text = string.Empty;
                }
                else
                {

                    txtClientOccupation.Text = result.Tables[0].Rows[0][Constants.CLIENT_OCCUPATION_COLUMN].ToString();
                    txtEmployer.Text = result.Tables[0].Rows[0][Constants.CLIENT_EMPLOYER_COLUMN].ToString();
                    txtClientEmployerAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_ADDITIONAL_ADDRESS_COLUMN].ToString();
                    txtClientEmployerCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_ADDITIONAL_CITY_COLUMN].ToString();
                    txtClientEmployerState.Text = result.Tables[0].Rows[0][Constants.CLIENT_ADDITIONAL_STATE_COLUMN].ToString();

                }
                //Client Additional Spouse information
                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_ADDITIONAL_SPOUSE_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtClientSpousLastName.Text = string.Empty;
                    txtClientSpousFirstName.Text = string.Empty;
                    txtClientSpousOccupation.Text = string.Empty;
                    txtSpouseEmployer.Text = string.Empty;
                    txtClientSpouseEmployerAddress.Text = string.Empty;
                    txtClientSpouseEmployerCity.Text = string.Empty;
                    txtClientSpouseEmployerState.Text = string.Empty;

                }
                else
                {
                    txtClientSpousLastName.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_LAST_NAME_COLUMN].ToString();
                    txtClientSpousFirstName.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_FIRST_NAME_COLUMN].ToString();
                    txtClientSpousOccupation.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_OCCUPATION_COLUMN].ToString();
                    txtSpouseEmployer.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_EMPLOYER_COLUMN].ToString();
                    txtClientSpouseEmployerAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_ADDRESS_COLUMN].ToString(); ;
                    txtClientSpouseEmployerCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_CITY_COLUMN].ToString();
                    txtClientSpouseEmployerState.Text = result.Tables[0].Rows[0][Constants.CLIENT_SPOUSE_STATE_COLUMN].ToString();


                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowClientAutoInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client General Addtional information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_AUTO_INFO_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtClientAutoInsuranceCompany.Text = string.Empty;
                    txtClientAutoAddress.Text = string.Empty;
                    txtClientAutoCity.Text = string.Empty;
                    txtClientAutoState.Text = string.Empty;
                    txtClientAutoZip.Text = string.Empty;
                    txtClientAutoPhoneNumber.Text = string.Empty;
                    txtClientAutoAdjuster.Text = string.Empty;

                }
                else
                {
                    txtClientAutoInsuranceCompany.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_INSURANCE_COMPANY_COLUMN].ToString();
                    txtClientAutoAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_ADDRESS_COLUMN].ToString();
                    txtClientAutoCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_CITY_COLUMN].ToString();
                    txtClientAutoState.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_PHONE_NUMBER_COLUMN].ToString();
                    txtClientAutoZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_ADJUSTER_COLUMN].ToString();
                    txtClientAutoPhoneNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_STATE_COLUMN].ToString();
                    txtClientAutoAdjuster.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_ZIP_COLUMN].ToString();



                }
                //Client Additional Spouse information
                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_AUTO_POLICY_INFO_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtPolicyNumber.Text = string.Empty;
                    dtEffectiveStartDate.Text = string.Empty;
                    dtEffectiveEndtDate.Text = string.Empty;
                    txtMedPayAvailable.Text = string.Empty;
                    txtAmount.Text = string.Empty;
                    txtLiabilityMinimumCoverage.Text = string.Empty;
                    txtLiabilityMaximumCoverage.Text = string.Empty;
                    txtUMIMinimum.Text = string.Empty;
                    txtUMIMaximum.Text = string.Empty;
                    txtReimbursable.Text = string.Empty;
                    txtClientAutoInfoNotes.Text = string.Empty;
                }
                else
                {
                    txtPolicyNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_POLICY_NUMBER_COLUMN].ToString();
                    dtEffectiveStartDate.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.CLIENT_AUTO_EFFECTIVE_START_DATE_COLUMN].ToString());
                    dtEffectiveEndtDate.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.CLIENT_AUTO_EFFECTIVE_END_DATE_COLUMN].ToString());
                    txtMedPayAvailable.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_MED_PAY_AVAILABLE_COLUMN].ToString();
                    txtAmount.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_AMOUNT_COLUMN].ToString();
                    txtLiabilityMinimumCoverage.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_LIABILITY_MIN_COVERAGE_COLUMN].ToString();
                    txtLiabilityMaximumCoverage.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_LIABILITY_MAX_COVERAGE_COLUMN].ToString();
                    txtUMIMinimum.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_UMIMIN_VALUE_COLUMN].ToString();
                    txtUMIMaximum.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_UMIMAX_VALUE_COLUMN].ToString();
                    txtReimbursable.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_REIMBURSABLE_COLUMN].ToString();
                    txtClientAutoInfoNotes.Text = result.Tables[0].Rows[0][Constants.CLIENT_AUTO_NOTES_COLUMN].ToString();


                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowClientMedicalInsuranceInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client Medical Insurance information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_MEDICAL_INSURANCE_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtNamedInsured.Text = string.Empty;
                    txtInsuranceCompany.Text = string.Empty;
                    txtClientMedicalAddress.Text = string.Empty;
                    txtClientMedicalCity.Text = string.Empty;
                    txtClientMedicalState.Text = string.Empty;
                    txtClientMedicalZip.Text = string.Empty;
                    txtClientMedicalPhoneNumber.Text = string.Empty;
                    txtClientMedicalPolicyNumber.Text = string.Empty;
                    txtClientMedicalMedCalNumber.Text = string.Empty;
                    txtClientMedicalMedCareNumber.Text = string.Empty;
                    txtClaimNumber.Text = string.Empty;
                }
                else
                {

                    txtNamedInsured.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_NAMEDINSURED_COLUMN].ToString();
                    txtInsuranceCompany.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_INSURANCE_COMPANY_COLUMN].ToString();
                    txtClientMedicalAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_ADDRESS_COLUMN].ToString();
                    txtClientMedicalCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_CITY_COLUMN].ToString();
                    txtClientMedicalState.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_STATE_COLUMN].ToString();
                    txtClientMedicalZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_ZIP_COLUMN].ToString();
                    txtClientMedicalPhoneNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_PHONE_NUMBER_COLUMN].ToString();
                    txtClientMedicalPolicyNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_POLICY_NUMBER_COLUMN].ToString();
                    txtClientMedicalMedCalNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_MEDICAL_NUMBER_COLUMN].ToString();
                    txtClientMedicalMedCareNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_MEDICAL_MEDICARE_COLUMN].ToString();
                    txtClaimNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_CLAIM_NUMBER_COLUMN].ToString();
                }
                //Client Injuries Inromation
                FillClientInjuryList(selectedClient.FileID);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void FillClientInjuryList(string fileID)
        {
            try
            {
                var injuryList = BusinessLogic.GetAllClientInjury(string.Format(Constants.CLIENT_INJURY_QUERY, fileID));
                dgClientMedicalInsuranceNotes.ItemsSource = injuryList;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowClientDefendantDetailsInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client Defendant Details information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_DEFENDANT_INFORMATION_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtDefendantLastName.Text = string.Empty;
                    txtDefendantFirstName.Text = string.Empty;
                    txtDefendantAddress.Text = string.Empty;
                    txtDefendantCity.Text = string.Empty;
                    txtDefendantState.Text = string.Empty;
                    txtDefendantZip.Text = string.Empty;
                    txtDefendantHomePhone.Text = string.Empty;
                    txtDefendantBusinessPhone.Text = string.Empty;
                    dtDefendantDateOfBirth.Text = string.Empty;
                    txtDefendantDrivingLicense.Text = string.Empty;
                }
                else
                {


                    txtDefendantLastName.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_LAST_NAME_COLUMN].ToString();
                    txtDefendantFirstName.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_FIRST_NAME_COLUMN].ToString();
                    txtDefendantAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ADDRESS_COLUMN].ToString();
                    txtDefendantCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_CITY_COLUMN].ToString();
                    txtDefendantState.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_STATE_COLUMN].ToString();
                    txtDefendantZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ZIP_COLUMN].ToString();
                    txtDefendantHomePhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_HOME_PHONE_COLUMN].ToString();
                    txtDefendantBusinessPhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_BUSINESS_PHONE_COLUMN].ToString();
                    dtDefendantDateOfBirth.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_DATE_OF_BIRTH_COLUMN].ToString());
                    txtDefendantDrivingLicense.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_lICENSE_NUMBER_COLUMN].ToString();




                }
                //Client Defendant Attorney Inromation

                result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtDefendantAttorneyFirm.Text = string.Empty;
                    txtDefendantAttorneyName.Text = string.Empty;
                    txtDefendantAttorneyAddress.Text = string.Empty;
                    txtDefendantAttorneyCity.Text = string.Empty;
                    txtDefendantAttorneyState.Text = string.Empty;
                    txtDefendantAttorneyZip.Text = string.Empty;
                    txtDefendantAttorneyPhone.Text = string.Empty;
                }
                else
                {

                    txtDefendantAttorneyFirm.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_FIRM_COLUMN].ToString();
                    txtDefendantAttorneyName.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_COLUMN].ToString();
                    txtDefendantAttorneyAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_ADDRESS_COLUMN].ToString();
                    txtDefendantAttorneyCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_CITY_COLUMN].ToString();
                    txtDefendantAttorneyState.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_STATE_COLUMN].ToString();
                    txtDefendantAttorneyZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_ZIP_COLUMN].ToString();
                    txtDefendantAttorneyPhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_ATTORNEY_PHONE_NUMBER_COLUMN].ToString();

                }

            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowDefendantInsuranceInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client Defendant Insurance information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_DEFENDANT_INSURANCE_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtDefendantInsuranceNameOfInsured.Text = string.Empty;
                    txtDefendantInsuranceCompany.Text = string.Empty;
                    txtDefendantInsuranceAddress.Text = string.Empty;
                    txtDefendantInsuranceCity.Text = string.Empty;
                    txtDefendantInsuranceState.Text = string.Empty;
                    txtDefendantInsuranceZip.Text = string.Empty;
                    txtDefendantInsurancePhone.Text = string.Empty;
                    txtDefendantInsuranceAdjuster.Text = string.Empty;
                    txtDefendantInsuranceClaimNumber.Text = string.Empty;
                    txtDefendantInsurancePolicyLimits.Text = string.Empty;
                }
                else
                {
                    txtDefendantInsuranceNameOfInsured.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_NAME_INSURED_COLUMN].ToString();
                    txtDefendantInsuranceCompany.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_INSURANCE_COMPANY_COLUMN].ToString();
                    txtDefendantInsuranceAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_ADDRESS_COLUMN].ToString();
                    txtDefendantInsuranceCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_CITY_COLUMN].ToString();
                    txtDefendantInsuranceState.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_STATE_COLUMN].ToString();
                    txtDefendantInsuranceZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_ZIP_COLUMN].ToString();
                    txtDefendantInsurancePhone.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_PHONE_NUMBER_COLUMN].ToString();
                    txtDefendantInsuranceAdjuster.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_ADJUSTER_COLUMN].ToString();
                    txtDefendantInsuranceClaimNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_CLAIM_NUMBER_COLUMN].ToString();
                    txtDefendantInsurancePolicyLimits.Text = result.Tables[0].Rows[0][Constants.CLIENT_DEFENDANT_INSURANCE_POLOCY_LIMIT_COLUMN].ToString();

                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowStatuteInformation(ClientFileInformation selectedClient)
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_STATUTE_INFORMATION, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {

                }
                else
                {
                    dtStatuteComplaintFiledDate.SelectedDate = DateTime.Parse(result.Tables[0].Rows[0][Constants.STATUTE_COMPLAINTFILE_DATE].ToString());
                    cbIsItAGovernmentClaim.IsChecked = Boolean.Parse(result.Tables[0].Rows[0][Constants.STATUTE_IS_GOVT_CLAIM].ToString());

                    string tempValue = result.Tables[0].Rows[0][Constants.STATUTE_CITY_DENIED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtCityDeniedDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_CITY_CLAIM_DUE_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtCityFileDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_COUNTY_DENIED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtCountyDeniedDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_COUNTY_FILED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtCountyFileDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_STATE_DENIED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtStateDeniedDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_STATE_FILED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtStateFileDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_OTHER_DENIED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtOtherDeniedDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                    tempValue = result.Tables[0].Rows[0][Constants.STATUTE_OTHER_FILED_DATE].ToString();
                    if (!string.IsNullOrEmpty(tempValue))
                    {
                        dtOtherFileDate.SelectedDate = DateTime.Parse(tempValue);
                    }
                    else
                    {
                        tempValue = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowClientCourtInformation(ClientFileInformation selectedClient)
        {
            try
            {
                //Client Court Insurance information
                var result = DBHelper.GetSelectDataSet(string.Format(Constants.CLIENT_COURT_DETAILS_QUERY, selectedClient.FileID));
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    txtCourtInfoCaseNumber.Text = string.Empty;
                    txtCourtInfoCourt.Text = string.Empty;
                    txtCourtInfoAddress.Text = string.Empty;
                    txtCourtInfoCity.Text = string.Empty;
                    txtCourtInfoState.Text = string.Empty;
                    txtCourtInfoZip.Text = string.Empty;

                }
                else
                {
                    txtCourtInfoCaseNumber.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_CASE_NUMBER_COLUMN].ToString();
                    txtCourtInfoCourt.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_COURT_COLUMN].ToString();
                    txtCourtInfoAddress.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_ADDRESS_COLUMN].ToString();
                    txtCourtInfoCity.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_CITY_COLUMN].ToString();
                    txtCourtInfoState.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_STATE_COLUMN].ToString();
                    txtCourtInfoZip.Text = result.Tables[0].Rows[0][Constants.CLIENT_COURT_ZIP_COLUMN].ToString();


                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }

        }

        private void ShowClientMiscNotes(ClientFileInformation selectedClient)
        {
            FillClientMiscList(selectedClient.FileID);
        }

        private void FillClientMiscList(string fileID)
        {
            try
            {
                var miscNotesList = BusinessLogic.GetAllClientMiscNotes(string.Format(Constants.CLIENT_MISC_NOTES_QUERY, fileID));
                dgMiscNotes.ItemsSource = miscNotesList;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void FillAllCaseStatusDropDown()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("CaseStatus");
                table.Rows.Add("Open");
                table.Rows.Add("Closed");
                cmbCaseStatus.DisplayMemberPath = "CaseStatus";
                cmbCaseStatus.SelectedValuePath = "CaseStatus";
                cmbCaseStatus.ItemsSource = table.DefaultView;
                cmbCaseStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void FillAllCaseTypeDropDown()
        {
            try
            {
                var result = DBHelper.GetSelectDataSet(Constants.ALL_CASE_TYPE);
                if (result == null || result.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                cmbCaseType.ItemsSource = null;
                cmbCaseType.DisplayMemberPath = result.Tables[0].Columns[0].ToString();
                cmbCaseType.SelectedValuePath = result.Tables[0].Columns[0].ToString();
                cmbCaseType.ItemsSource = result.Tables[0].DefaultView;
                cmbCaseType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Save Method - Save New Client

        private void SaveClientDetails()
        {
            bool isRequiredProvided = IsRequiredFieldsProvided();
            //TODO: do validation check here
            SaveClientGeneralDetails();
            SaveAdditionalClientInformation();
            SaveClientAutoInformation();
            SaveClientMedicalInformation();
            SaveDefendantInformation();
            SaveDefendantInsuranceInformation();
            SaveStatuteInformation();
            SaveCourtInformation();
        }

        private bool IsRequiredFieldsProvided()
        {
            //TODO: perform required fields validation here
            return true;
        }

        private void SaveClientGeneralDetails()
        {
            try
            {
                ClientGeneralInformation generalInfo = new ClientGeneralInformation();
                generalInfo.FileNo = txtFileNo.Text;
                generalInfo.LastName = txtLastName.Text;
                generalInfo.FirstName = txtFirstName.Text;

                //TODO: check what is actual value for Other
                if (cmbCaseType.Text == "Other")
                {
                    generalInfo.CaseType = txtOtherCaseType.Text;
                }
                else
                {
                    generalInfo.CaseType = cmbCaseType.Text;
                }
                generalInfo.CaseStatus = cmbCaseStatus.Text;
                if (dtAccidentDate.SelectedDate != null && dtAccidentDate.SelectedDate.Value != null)
                {
                    generalInfo.AccidentDate = DateTime.Parse(dtAccidentDate.SelectedDate.Value.ToShortDateString());
                }
                generalInfo.ClientCreatedOn = DateTime.Now.ToShortDateString();
                generalInfo.Address = txtAddress.Text;
                generalInfo.City = txtCity.Text;
                generalInfo.State = txtState.Text;
                generalInfo.HomePhone = txtHomePhone.Text;
                generalInfo.CellPhone = txtCellPhone.Text;
                generalInfo.DrivingLicense = txtDrivingLicense.Text;

                if (dtDateOfBirth.SelectedDate != null && dtDateOfBirth.SelectedDate.Value != null)
                {
                    generalInfo.DateOfBirth = DateTime.Parse(dtDateOfBirth.SelectedDate.Value.ToShortDateString());
                }


                generalInfo.SSN = txtSSN.Text;

                generalInfo.WorkPhone= txtWorkPhone.Text;
                generalInfo.Email= txtEmail.Text;
                generalInfo.SuiteAddress = txtSuiteAddress.Text;

                generalInfo.InitialCaseInformation = txtInitialCaseInformation.Text;
                generalInfo.DefendantName = txtDefendantName.Text;

                generalInfo.OriginatingAttorney = txtOriginatingAttorney.Text;
                generalInfo.AssignedAttorney = txtAssignedAttorney.Text;
                generalInfo.Referral = txtRefferal.Text;

                BusinessLogic.SaveClientGeneralInfo(generalInfo);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveAdditionalClientInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtClientOccupation.Text) &&
                    string.IsNullOrEmpty(txtEmployer.Text) &&
                    string.IsNullOrEmpty(txtClientEmployerAddress.Text) &&
                    string.IsNullOrEmpty(txtClientEmployerCity.Text) &&
                    string.IsNullOrEmpty(txtClientEmployerState.Text) &&
                    string.IsNullOrEmpty(txtClientSpousLastName.Text) &&
                    string.IsNullOrEmpty(txtClientSpousFirstName.Text) &&
                    string.IsNullOrEmpty(txtClientSpousOccupation.Text) &&
                    string.IsNullOrEmpty(txtSpouseEmployer.Text) &&
                    string.IsNullOrEmpty(txtClientSpouseEmployerAddress.Text) &&
                    string.IsNullOrEmpty(txtClientSpouseEmployerCity.Text) &&
                    string.IsNullOrEmpty(txtClientSpouseEmployerState.Text))
                {
                    return;
                }
                ClientAdditionalInformation additionalInfo = new ClientAdditionalInformation();
                additionalInfo.Occupation = txtClientOccupation.Text;
                additionalInfo.Employer = txtEmployer.Text;
                additionalInfo.Address = txtClientEmployerAddress.Text;
                additionalInfo.City = txtClientEmployerCity.Text;
                additionalInfo.State = txtClientEmployerState.Text;
                additionalInfo.SpouseInfo = new SpouseInformation();
                additionalInfo.SpouseInfo.LastName = txtClientSpousLastName.Text;
                additionalInfo.SpouseInfo.FirstName = txtClientSpousFirstName.Text;
                additionalInfo.SpouseInfo.Occupation = txtClientSpousOccupation.Text;
                additionalInfo.SpouseInfo.Employer = txtSpouseEmployer.Text;
                additionalInfo.SpouseInfo.Address = txtClientSpouseEmployerAddress.Text;
                additionalInfo.SpouseInfo.City = txtClientSpouseEmployerCity.Text;
                additionalInfo.SpouseInfo.State = txtClientSpouseEmployerState.Text;

                BusinessLogic.SaveClientAdditionalInformation(additionalInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveClientAutoInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtClientAutoInsuranceCompany.Text) &&
                    string.IsNullOrEmpty(txtClientAutoAddress.Text) &&
                    string.IsNullOrEmpty(txtClientAutoCity.Text) &&
                    string.IsNullOrEmpty(txtClientAutoState.Text) &&
                    string.IsNullOrEmpty(txtClientAutoZip.Text) &&
                    string.IsNullOrEmpty(txtClientAutoPhoneNumber.Text) &&
                    string.IsNullOrEmpty(txtPolicyNumber.Text) &&
                    dtEffectiveStartDate.SelectedDate == null &&
                    dtEffectiveEndtDate.SelectedDate == null &&
                    string.IsNullOrEmpty(txtMedPayAvailable.Text) &&
                    string.IsNullOrEmpty(txtAmount.Text) &&
                    string.IsNullOrEmpty(txtLiabilityMinimumCoverage.Text) &&
                    string.IsNullOrEmpty(txtLiabilityMaximumCoverage.Text) &&
                    string.IsNullOrEmpty(txtUMIMinimum.Text) &&
                    string.IsNullOrEmpty(txtUMIMaximum.Text) &&
                    string.IsNullOrEmpty(txtReimbursable.Text) &&
                    string.IsNullOrEmpty(txtClientAutoInfoNotes.Text))
                {
                    return;
                }

                ClientAutoInformation autoInfo = new ClientAutoInformation();
                autoInfo.InsuranceCompany = txtClientAutoInsuranceCompany.Text;
                autoInfo.Address = txtClientAutoAddress.Text;
                autoInfo.City = txtClientAutoCity.Text;
                autoInfo.State = txtClientAutoState.Text;
                autoInfo.Zip = txtClientAutoZip.Text;
                autoInfo.PhoneNumber = txtClientAutoPhoneNumber.Text;
                autoInfo.Adjuster = txtClientAutoAdjuster.Text;
                autoInfo.PolicyNumber = txtPolicyNumber.Text;
                autoInfo.EffectiveStartDate = DateTime.Parse(dtEffectiveStartDate.SelectedDate.Value.ToShortDateString());
                autoInfo.EffectiveEndDate = DateTime.Parse(dtEffectiveEndtDate.SelectedDate.Value.ToShortDateString());
                autoInfo.MedPayAvailable = txtMedPayAvailable.Text;
                autoInfo.Amout = txtAmount.Text;
                autoInfo.LiabilityMinimumCoverage = txtLiabilityMinimumCoverage.Text;
                autoInfo.LiabilityMaximumCoverage = txtLiabilityMaximumCoverage.Text;
                autoInfo.UMIMinimum = txtUMIMinimum.Text;
                autoInfo.UMIMaximum = txtUMIMaximum.Text;
                autoInfo.Reimbursable = txtReimbursable.Text;
                autoInfo.Notes = txtClientAutoInfoNotes.Text;

                BusinessLogic.SaveClientAutoInformation(autoInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveClientMedicalInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtNamedInsured.Text) &&
                    string.IsNullOrEmpty(txtInsuranceCompany.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalAddress.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalCity.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalState.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalZip.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalPhoneNumber.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalPolicyNumber.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalMedCalNumber.Text) &&
                    string.IsNullOrEmpty(txtClaimNumber.Text) &&
                    string.IsNullOrEmpty(txtClientMedicalMedCareNumber.Text))
                {
                    return;
                }
                ClientMedicalInformation mediInfo = new ClientMedicalInformation();
                mediInfo.NamedInsured = txtNamedInsured.Text;
                mediInfo.InsuranceCompany = txtInsuranceCompany.Text;
                mediInfo.Address = txtClientMedicalAddress.Text;
                mediInfo.City = txtClientMedicalCity.Text;
                mediInfo.State = txtClientMedicalState.Text;
                mediInfo.Zip = txtClientMedicalZip.Text;
                mediInfo.PhoneNumber = txtClientMedicalPhoneNumber.Text;
                mediInfo.PolicyNumber = txtClientMedicalPolicyNumber.Text;
                mediInfo.MediCalNumber = txtClientMedicalMedCalNumber.Text;
                mediInfo.MediCareNumber = txtClientMedicalMedCareNumber.Text;
                mediInfo.ClaimNumber = txtClaimNumber.Text;

                BusinessLogic.SaveClientMedicationInformation(mediInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveDefendantInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtDefendantLastName.Text) &&
                    string.IsNullOrEmpty(txtDefendantFirstName.Text) &&
                    string.IsNullOrEmpty(txtDefendantAddress.Text) &&
                    string.IsNullOrEmpty(txtDefendantCity.Text) &&
                    string.IsNullOrEmpty(txtDefendantState.Text) &&
                    string.IsNullOrEmpty(txtDefendantZip.Text) &&
                    string.IsNullOrEmpty(txtDefendantHomePhone.Text) &&
                    string.IsNullOrEmpty(txtDefendantBusinessPhone.Text) &&
                    dtDefendantDateOfBirth.SelectedDate == null &&
                    string.IsNullOrEmpty(txtDefendantDrivingLicense.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyFirm.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyName.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyAddress.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyCity.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyState.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyZip.Text) &&
                    string.IsNullOrEmpty(txtDefendantAttorneyPhone.Text))
                {
                    return;
                }
                ClientDefendantInformation defInfo = new ClientDefendantInformation();
                defInfo.LastName = txtDefendantLastName.Text;
                defInfo.FirstName = txtDefendantFirstName.Text;
                defInfo.Address = txtDefendantAddress.Text;
                defInfo.City = txtDefendantCity.Text;
                defInfo.State = txtDefendantState.Text;
                defInfo.Zip = txtDefendantZip.Text;
                defInfo.HomePhone = txtDefendantHomePhone.Text;
                defInfo.BusinessPhone = txtDefendantBusinessPhone.Text;
                defInfo.DateOfBirth = DateTime.Parse(dtDefendantDateOfBirth.SelectedDate.Value.ToShortDateString());
                defInfo.DrivingLicense = txtDefendantDrivingLicense.Text;

                defInfo.AttorneyInfo = new DefendantAttorneyInformation();
                defInfo.AttorneyInfo.Firm = txtDefendantAttorneyFirm.Text;
                defInfo.AttorneyInfo.Attorney = txtDefendantAttorneyName.Text;
                defInfo.AttorneyInfo.Address = txtDefendantAttorneyAddress.Text;
                defInfo.AttorneyInfo.City = txtDefendantAttorneyCity.Text;
                defInfo.AttorneyInfo.State = txtDefendantAttorneyState.Text;
                defInfo.AttorneyInfo.Zip = txtDefendantAttorneyZip.Text;
                defInfo.AttorneyInfo.Phone = txtDefendantAttorneyPhone.Text;

                BusinessLogic.SaveClientDefendantInformation(defInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveDefendantInsuranceInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtDefendantInsuranceNameOfInsured.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceCompany.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceAddress.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceCity.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceState.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceZip.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsurancePhone.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceAdjuster.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsuranceClaimNumber.Text) &&
                    string.IsNullOrEmpty(txtDefendantInsurancePolicyLimits.Text))
                {
                    return;
                }
                DefendantInsuranceDetails defInsurance = new DefendantInsuranceDetails();
                defInsurance.NameOfInsured = txtDefendantInsuranceNameOfInsured.Text;
                defInsurance.InsuranceCompany = txtDefendantInsuranceCompany.Text;
                defInsurance.Address = txtDefendantInsuranceAddress.Text;
                defInsurance.City = txtDefendantInsuranceCity.Text;
                defInsurance.State = txtDefendantInsuranceState.Text;
                defInsurance.Zip = txtDefendantInsuranceZip.Text;
                defInsurance.Phone = txtDefendantInsurancePhone.Text;
                defInsurance.Adjuster = txtDefendantInsuranceAdjuster.Text;
                defInsurance.ClaimNumber = txtDefendantInsuranceClaimNumber.Text;
                defInsurance.PolicyLimits = txtDefendantInsurancePolicyLimits.Text;

                BusinessLogic.SaveDefendantInsuranceInformation(defInsurance, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveStatuteInformation()
        {
            try
            {
                if (dtStatuteComplaintFiledDate.SelectedDate == null &&
                    dtCityDeniedDate.SelectedDate == null &&
                    dtCityFileDate.SelectedDate == null &&
                    dtCountyDeniedDate.SelectedDate == null &&
                    dtCountyFileDate.SelectedDate == null &&
                    dtStateDeniedDate.SelectedDate == null &&
                    dtStateFileDate.SelectedDate == null &&
                    dtOtherDeniedDate.SelectedDate == null &&
                    dtOtherFileDate.SelectedDate == null)
                {
                    return;
                }
                StatuteInformation statuteInfo = new StatuteInformation();
                if (dtAccidentDate.SelectedDate != null)
                {
                    statuteInfo.AccidentDate = dtAccidentDate.SelectedDate.Value.ToShortDateString();
                    statuteInfo.AccDateAfter1yr = dtAccidentDate.SelectedDate.Value.AddYears(1).ToShortDateString();
                    statuteInfo.AccDateAfter2yr = dtAccidentDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                }
                if (dtStatuteComplaintFiledDate.SelectedDate != null)
                {
                    statuteInfo.ComplaintFileDate = dtStatuteComplaintFiledDate.SelectedDate.Value.ToShortDateString();
                    statuteInfo.ComplaintAfter60days = dtStatuteComplaintFiledDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                    statuteInfo.ComplaintAfter2yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                    statuteInfo.ComplaintAfter3yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                    statuteInfo.ComplaintAfter5yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                }
                statuteInfo.IsGovtDClaim = cbIsItAGovernmentClaim.IsChecked.Value;
                if (statuteInfo.IsGovtDClaim)
                {
                    if (dtCityDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.CityClaim = new GovertmentClaimInformation();
                        statuteInfo.CityClaim.DeniedDate = dtCityDeniedDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.CityClaim.ClaimDueDate = dtCityDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
                    }
                    if (dtCityFileDate.SelectedDate != null)
                    {
                        statuteInfo.CityClaim = statuteInfo.CityClaim ?? new GovertmentClaimInformation();
                        statuteInfo.CityClaim.FiledDate = dtCityFileDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.CityClaim.FiledDateAfter60Days = dtCityFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                        statuteInfo.CityClaim.FiledDateAfter2yrs = dtCityFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                        statuteInfo.CityClaim.FiledDateAfter3yrs = dtCityFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                        statuteInfo.CityClaim.FiledDateAfter5yrs = dtCityFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                    }

                    if (dtCountyDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.CountyClaim = new GovertmentClaimInformation();
                        statuteInfo.CountyClaim.DeniedDate = dtCountyDeniedDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.CountyClaim.ClaimDueDate = dtCountyDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
                    }
                    if (dtCountyFileDate.SelectedDate != null)
                    {
                        statuteInfo.CountyClaim = statuteInfo.CountyClaim ?? new GovertmentClaimInformation();
                        statuteInfo.CountyClaim.FiledDate = dtCountyFileDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.CountyClaim.FiledDateAfter60Days = dtCountyFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                        statuteInfo.CountyClaim.FiledDateAfter2yrs = dtCountyFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                        statuteInfo.CountyClaim.FiledDateAfter3yrs = dtCountyFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                        statuteInfo.CountyClaim.FiledDateAfter5yrs = dtCountyFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                    }

                    if (dtStateDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.StateClaim = new GovertmentClaimInformation();
                        statuteInfo.StateClaim.DeniedDate = dtStateDeniedDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.StateClaim.ClaimDueDate = dtStateDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
                    }
                    if (dtStateFileDate.SelectedDate != null)
                    {
                        statuteInfo.StateClaim = statuteInfo.StateClaim ?? new GovertmentClaimInformation();
                        statuteInfo.StateClaim.FiledDate = dtStateFileDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.StateClaim.FiledDateAfter60Days = dtStateFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                        statuteInfo.StateClaim.FiledDateAfter2yrs = dtStateFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                        statuteInfo.StateClaim.FiledDateAfter3yrs = dtStateFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                        statuteInfo.StateClaim.FiledDateAfter5yrs = dtStateFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                    }

                    if (dtOtherDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.OtherClaim = new GovertmentClaimInformation();
                        statuteInfo.OtherClaim.DeniedDate = dtOtherDeniedDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.OtherClaim.ClaimDueDate = dtOtherDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
                    }
                    if (dtOtherFileDate.SelectedDate != null)
                    {
                        statuteInfo.OtherClaim = statuteInfo.OtherClaim ?? new GovertmentClaimInformation();
                        statuteInfo.OtherClaim.FiledDate = dtOtherFileDate.SelectedDate.Value.ToShortDateString();
                        statuteInfo.OtherClaim.FiledDateAfter60Days = dtOtherFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                        statuteInfo.OtherClaim.FiledDateAfter2yrs = dtOtherFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                        statuteInfo.OtherClaim.FiledDateAfter3yrs = dtOtherFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                        statuteInfo.OtherClaim.FiledDateAfter5yrs = dtOtherFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                    }
                }
                BusinessLogic.SaveStatuteInformation(statuteInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void SaveCourtInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtCourtInfoCaseNumber.Text) &&
                    string.IsNullOrEmpty(txtCourtInfoCourt.Text) &&
                    string.IsNullOrEmpty(txtCourtInfoAddress.Text) &&
                    string.IsNullOrEmpty(txtCourtInfoCity.Text) &&
                    string.IsNullOrEmpty(txtCourtInfoState.Text) &&
                    string.IsNullOrEmpty(txtCourtInfoZip.Text))
                {
                    return;
                }
                CourtInformation courtInfo = new CourtInformation();
                courtInfo.CaseNumber = txtCourtInfoCaseNumber.Text;
                courtInfo.Court = txtCourtInfoCourt.Text;
                courtInfo.Address = txtCourtInfoAddress.Text;
                courtInfo.City = txtCourtInfoCity.Text;
                courtInfo.State = txtCourtInfoState.Text;
                courtInfo.Zip = txtCourtInfoZip.Text;

                BusinessLogic.SaveCourtInformation(courtInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Update Methods

        private void UpdateClientDetails()
        {
            //bool isRequiredProvided = IsRequiredFieldsProvided();
            //UpdateClientGeneralDetails();
            //UpdateAdditionalClientInformation();
            //UpdateClientAutoInformation();
            //UpdateClientMedicalInformation();
            //UpdateDefendantInformation();
            //UpdateDefendantInsuranceInformation();
            //UpdateStatuteInformation();
            //UpdateCourtInformation();
        }

        private void UpdateClientGeneralDetails()
        {
            try
            {
                selectedGeneralInfo = new ClientGeneralInformation();
                selectedGeneralInfo.FileNo = txtFileNo.Text;
                selectedGeneralInfo.LastName = txtLastName.Text;
                selectedGeneralInfo.FirstName = txtFirstName.Text;

                //TODO: check what is actual value for Other
                if (cmbCaseType.Text == "Other")
                {
                    selectedGeneralInfo.CaseType = txtOtherCaseType.Text;
                }
                else
                {
                    selectedGeneralInfo.CaseType = cmbCaseType.Text;
                }
                selectedGeneralInfo.CaseStatus = cmbCaseStatus.Text;
                if (dtAccidentDate.SelectedDate.Value != null)
                {
                    selectedGeneralInfo.AccidentDate = DateTime.Parse(dtAccidentDate.SelectedDate.Value.ToShortDateString());
                }

                selectedGeneralInfo.Address = txtAddress.Text;
                selectedGeneralInfo.City = txtCity.Text;
                selectedGeneralInfo.State = txtState.Text;
                selectedGeneralInfo.HomePhone = txtHomePhone.Text;
                selectedGeneralInfo.CellPhone = txtCellPhone.Text;
                selectedGeneralInfo.DrivingLicense = txtDrivingLicense.Text;
                selectedGeneralInfo.DateOfBirth = DateTime.Parse(dtDateOfBirth.SelectedDate.Value.ToShortDateString());
                selectedGeneralInfo.SSN = txtSSN.Text;
                selectedGeneralInfo.SSN = txtSSN.Text;

                selectedGeneralInfo.InitialCaseInformation = txtInitialCaseInformation.Text;
                selectedGeneralInfo.DefendantName = txtDefendantName.Text;

                selectedGeneralInfo.OriginatingAttorney = txtOriginatingAttorney.Text;
                selectedGeneralInfo.AssignedAttorney = txtAssignedAttorney.Text;
                selectedGeneralInfo.Referral = txtRefferal.Text;

                selectedGeneralInfo.SuiteAddress = txtSuiteAddress.Text;
                selectedGeneralInfo.WorkPhone = txtWorkPhone.Text;
                selectedGeneralInfo.Email = txtEmail.Text;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion
        #endregion

        #region Readonly all controls

        private void UpdateAllControlsStatus(bool isEnabled)
        {
            UpdateStatusGeneralSection(isEnabled);
            UpdateStatusClientAdditionalInfo(isEnabled);
            UpdateStatusClientSpouseInfo(isEnabled);
            UpdateStatusClientAutoInfo(isEnabled);
            UpdateClientMedicalInfo(isEnabled);
            UpdateStatusClientDefendentInfo(isEnabled);
            UpdateStatusClientDefendentAttorneyInfo(isEnabled);
            UpdateStatusClientDefendentInsuranceInfo(isEnabled);
            UpdateStatusClientStatuteInfo(isEnabled);
            UpdateStatusClientCourtInfo(isEnabled);
            UpdateStatusMiscNotesInfo(isEnabled);

            btnSaveClient.IsEnabled = !isEnabled;
            btnCancelEdit.IsEnabled = !isEnabled;
            btnAddNewClient.IsEnabled = isEnabled;
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
            txtEvidence1.IsReadOnly = true;
            txtEvidence2.IsReadOnly = true;
            txtOriginatingAttorney.IsReadOnly = isEnabled;
            txtAssignedAttorney.IsReadOnly = isEnabled;
            txtRefferal.IsReadOnly = isEnabled;

            txtWorkPhone.IsReadOnly = isEnabled;
            txtEmail.IsReadOnly = isEnabled;
            txtSuiteAddress.IsReadOnly = isEnabled;

            btnCancelEditGeneralClient.IsEnabled = !isEnabled;
            btnSaveEditGeneralClient.IsEnabled = !isEnabled;
            btnViewAllEvidence.IsEnabled = !isEnabled;
        }

        private void UpdateStatusClientAdditionalInfo(bool isEnabled)
        {
            txtClientOccupation.IsReadOnly = isEnabled;
            txtEmployer.IsReadOnly = isEnabled;
            txtClientEmployerAddress.IsReadOnly = isEnabled;
            txtClientEmployerCity.IsReadOnly = isEnabled;
            txtClientEmployerState.IsReadOnly = isEnabled;

            btnCancelAdditionalClientInfo.IsEnabled = !isEnabled;
            btnSaveAdditionalClientInfo.IsEnabled = !isEnabled;
        }

        private void UpdateStatusClientSpouseInfo(bool isEnabled)
        {
            txtClientSpousLastName.IsReadOnly = isEnabled;
            txtClientSpousFirstName.IsReadOnly = isEnabled;
            txtClientSpousOccupation.IsReadOnly = isEnabled;
            txtSpouseEmployer.IsReadOnly = isEnabled;
            txtClientSpouseEmployerAddress.IsReadOnly = isEnabled;
            txtClientSpouseEmployerCity.IsReadOnly = isEnabled;
            txtClientSpouseEmployerState.IsReadOnly = isEnabled;
        }

        private void UpdateStatusClientAutoInfo(bool isEnabled)
        {
            txtClientAutoInsuranceCompany.IsReadOnly = isEnabled;
            txtClientAutoAddress.IsReadOnly = isEnabled;
            txtClientAutoCity.IsReadOnly = isEnabled;
            txtClientAutoState.IsReadOnly = isEnabled;
            txtClientAutoZip.IsReadOnly = isEnabled;
            txtClientAutoPhoneNumber.IsReadOnly = isEnabled;
            txtClientAutoAdjuster.IsReadOnly = isEnabled;
            txtPolicyNumber.IsReadOnly = isEnabled;
            dtEffectiveStartDate.IsEnabled = !isEnabled;
            dtEffectiveEndtDate.IsEnabled = !isEnabled;
            txtMedPayAvailable.IsReadOnly = isEnabled;
            txtAmount.IsReadOnly = isEnabled;
            txtLiabilityMinimumCoverage.IsReadOnly = isEnabled;
            txtLiabilityMaximumCoverage.IsReadOnly = isEnabled;
            txtUMIMinimum.IsReadOnly = isEnabled;
            txtUMIMaximum.IsReadOnly = isEnabled;
            txtReimbursable.IsReadOnly = isEnabled;
            txtClientAutoInfoNotes.IsReadOnly = isEnabled;

            btnCancelClientAutoInfo.IsEnabled = !isEnabled;
            btnSaveClientAutoInfo.IsEnabled = !isEnabled;
        }

        private void UpdateClientMedicalInfo(bool isEnabled)
        {
            txtNamedInsured.IsReadOnly = isEnabled;
            txtInsuranceCompany.IsReadOnly = isEnabled;
            txtClientMedicalAddress.IsReadOnly = isEnabled;
            txtClientMedicalCity.IsReadOnly = isEnabled;
            txtClientMedicalState.IsReadOnly = isEnabled;
            txtClientMedicalZip.IsReadOnly = isEnabled;
            txtClientMedicalPhoneNumber.IsReadOnly = isEnabled;
            txtClientMedicalPolicyNumber.IsReadOnly = isEnabled;
            txtClientMedicalMedCalNumber.IsReadOnly = isEnabled;
            txtClientMedicalMedCareNumber.IsReadOnly = isEnabled;
            txtClaimNumber.IsReadOnly = isEnabled;

            btnAddNewClientMedicalInsuranceNote.IsEnabled = !isEnabled;
            btnEditClientMedicalInsuranceNote.IsEnabled = !isEnabled;
            btnDeleteClientMedicalInsuranceNote.IsEnabled = !isEnabled;

            btnCancelClientMedicalInsurance.IsEnabled = !isEnabled;
            btnSaveClientMedicalInsurance.IsEnabled = !isEnabled;
            exdClientInjuries.IsExpanded = !isEnabled;
        }


        private void UpdateClientInjuryInfo(bool isEnabled)
        {
            txtClientMedicalInsuranceNoteName.Text = string.Empty;
            txtClientMedicalInsuranceNoteDescription.Text = string.Empty;
            dtClientMedicalNoteDate.SelectedDate = null;

            txtClientMedicalInsuranceNoteName.IsReadOnly = isEnabled;
            txtClientMedicalInsuranceNoteDescription.IsReadOnly = isEnabled;

            dtClientMedicalNoteDate.IsEnabled = !isEnabled;
            btnAddNewClientMedicalInsuranceNote.IsEnabled = !isEnabled;
            btnEditClientMedicalInsuranceNote.IsEnabled = !isEnabled;
            btnSaveClientMedicalInsuranceNote.IsEnabled = !isEnabled;
            btnDeleteClientMedicalInsuranceNote.IsEnabled = !isEnabled;
        }
        private void UpdateStatusClientDefendentInfo(bool isEnabled)
        {
            txtDefendantLastName.IsReadOnly = isEnabled;
            txtDefendantFirstName.IsReadOnly = isEnabled;
            txtDefendantAddress.IsReadOnly = isEnabled;
            txtDefendantCity.IsReadOnly = isEnabled;
            txtDefendantState.IsReadOnly = isEnabled;
            txtDefendantZip.IsReadOnly = isEnabled;
            txtDefendantHomePhone.IsReadOnly = isEnabled;
            txtDefendantBusinessPhone.IsReadOnly = isEnabled;
            dtDefendantDateOfBirth.IsEnabled = !isEnabled;
            txtDefendantDrivingLicense.IsReadOnly = isEnabled;

            btnEditDefendantInfo.IsEnabled = isEnabled;
            btnCancelDefendantInfo.IsEnabled = !isEnabled;
            btnSaveDefendantInfo.IsEnabled = !isEnabled;
            exdDefendantAttorneyInformation.IsExpanded = !isEnabled;
        }

        private void UpdateStatusClientDefendentAttorneyInfo(bool isEnabled)
        {
            txtDefendantAttorneyFirm.IsReadOnly = isEnabled;
            txtDefendantAttorneyName.IsReadOnly = isEnabled;
            txtDefendantAttorneyAddress.IsReadOnly = isEnabled;
            txtDefendantAttorneyCity.IsReadOnly = isEnabled;
            txtDefendantAttorneyState.IsReadOnly = isEnabled;
            txtDefendantAttorneyZip.IsReadOnly = isEnabled;
            txtDefendantAttorneyPhone.IsReadOnly = isEnabled;
        }

        private void UpdateStatusClientDefendentInsuranceInfo(bool isEnabled)
        {
            txtDefendantInsuranceNameOfInsured.IsReadOnly = isEnabled;
            txtDefendantInsuranceCompany.IsReadOnly = isEnabled;
            txtDefendantInsuranceAddress.IsReadOnly = isEnabled;
            txtDefendantInsuranceCity.IsReadOnly = isEnabled;
            txtDefendantInsuranceState.IsReadOnly = isEnabled;
            txtDefendantInsuranceZip.IsReadOnly = isEnabled;
            txtDefendantInsurancePhone.IsReadOnly = isEnabled;
            txtDefendantInsuranceAdjuster.IsReadOnly = isEnabled;
            txtDefendantInsuranceClaimNumber.IsReadOnly = isEnabled;
            txtDefendantInsurancePolicyLimits.IsReadOnly = isEnabled;

            btnCancelDefendantInsuranceInfo.IsEnabled = !isEnabled;
            btnSaveDefendantInsuranceInfo.IsEnabled = !isEnabled;
            btnEditDefendantInsuranceInfo.IsEnabled = isEnabled;
            
        }

        private void UpdateStatusClientStatuteInfo(bool isEnabled)
        {
            cbIsItAGovernmentClaim.IsChecked = false;
            dtStatuteComplaintFiledDate.IsEnabled = !isEnabled;
            dtStatuteComplaintFiledDate.IsEnabled = !isEnabled;
            cbIsItAGovernmentClaim.IsChecked = false;
            dtCityDeniedDate.IsEnabled = !isEnabled;
            dtCityFileDate.IsEnabled = !isEnabled;

            btnCancelStatuteInfo.IsEnabled = !isEnabled;
            btnSaveStatuteInfo.IsEnabled = !isEnabled;
            btnEditStatuteInfo.IsEnabled = isEnabled;
        }

        private void UpdateStatusClientCourtInfo(bool isEnabled)
        {
            txtCourtInfoCaseNumber.IsReadOnly = isEnabled;
            txtCourtInfoCourt.IsReadOnly = isEnabled;
            txtCourtInfoAddress.IsReadOnly = isEnabled;
            txtCourtInfoCity.IsReadOnly = isEnabled;
            txtCourtInfoState.IsReadOnly = isEnabled;
            txtCourtInfoZip.IsReadOnly = isEnabled;

            btnCancelCourtInfo.IsEnabled = !isEnabled;
            btnSaveCourtInfo.IsEnabled = !isEnabled;
            btnEditCourtInfo.IsEnabled = isEnabled;
        }

        private void UpdateStatusMiscNotesInfo(bool isEnabled)
        {
            //dgMiscNotes.ItemsSource = null;
            txtMiscNoteName.IsReadOnly = isEnabled;
            txtMiscNoteDescription.IsReadOnly = isEnabled;
            dtMiscNoteDate.SelectedDate = null;

            btnAddNewMiscNote.IsEnabled = !isEnabled;
            btnEditMiscNote.IsEnabled = !isEnabled;
            btnSaveMiscNote.IsEnabled = !isEnabled;
            btnDeleteMiscNote.IsEnabled = !isEnabled;

            btnEditMiscNoteInfoSection.IsEnabled = isEnabled;
            btnCancelMiscNoteInfoSection.IsEnabled = !isEnabled;

        }

     
        #endregion

        #region Clear All Controls
        private void ClearAllControls()
        {
            ClearGeneralSection();
            ClearClientAdditionalInfo();
            ClearClientSpouseInfo();
            ClearClientAutoInfo();
            ClearClientMedicalInfo();
            ClearClientDefendentInfo();
            ClearClientDefendentAttorneyInfo();
            ClearClientDefendentInsuranceInfo();
            ClearClientStatuteInfo();
            ClearClientCourtInfo();
            ClearMiscNotesInfo();
        }
        private void ClearGeneralSection()
        {
            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtFileNo.Text = string.Empty;
            txtOtherCaseType.Text = string.Empty;
            dtAccidentDate.SelectedDate = null;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtHomePhone.Text = string.Empty;
            txtCellPhone.Text = string.Empty;
            txtDrivingLicense.Text = string.Empty;
            dtDateOfBirth.SelectedDate = null;
            txtSSN.Text = string.Empty;
            txtInitialCaseInformation.Text = string.Empty;
            txtDefendantName.Text = string.Empty;
            txtEvidence1.Text = string.Empty;
            txtEvidence2.Text = string.Empty;
            txtOriginatingAttorney.Text = string.Empty;
            txtAssignedAttorney.Text = string.Empty;
            txtRefferal.Text = string.Empty;

            txtSuiteAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtWorkPhone.Text = string.Empty;
        }
        private void ClearClientAdditionalInfo()
        {
            txtClientOccupation.Text = string.Empty;
            txtEmployer.Text = string.Empty;
            txtClientEmployerAddress.Text = string.Empty;
            txtClientEmployerCity.Text = string.Empty;
            txtClientEmployerState.Text = string.Empty;
        }
        private void ClearClientSpouseInfo()
        {
            txtClientSpousLastName.Text = string.Empty;
            txtClientSpousFirstName.Text = string.Empty;
            txtClientSpousOccupation.Text = string.Empty;
            txtSpouseEmployer.Text = string.Empty;
            txtClientSpouseEmployerAddress.Text = string.Empty;
            txtClientSpouseEmployerCity.Text = string.Empty;
            txtClientSpouseEmployerState.Text = string.Empty;
        }
        private void ClearClientAutoInfo()
        {
            txtClientAutoInsuranceCompany.Text = string.Empty;
            txtClientAutoAddress.Text = string.Empty;
            txtClientAutoCity.Text = string.Empty;
            txtClientAutoState.Text = string.Empty;
            txtClientAutoZip.Text = string.Empty;
            txtClientAutoPhoneNumber.Text = string.Empty;
            txtClientAutoAdjuster.Text = string.Empty;
            txtPolicyNumber.Text = string.Empty;
            dtEffectiveStartDate.Text = string.Empty;
            dtEffectiveEndtDate.Text = string.Empty;
            txtMedPayAvailable.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtLiabilityMinimumCoverage.Text = string.Empty;
            txtLiabilityMaximumCoverage.Text = string.Empty;
            txtUMIMinimum.Text = string.Empty;
            txtUMIMaximum.Text = string.Empty;
            txtReimbursable.Text = string.Empty;
            txtClientAutoInfoNotes.Text = string.Empty;

        }
        private void ClearClientMedicalInfo()
        {
            txtNamedInsured.Text = string.Empty;
            txtInsuranceCompany.Text = string.Empty;
            txtClientMedicalAddress.Text = string.Empty;
            txtClientMedicalCity.Text = string.Empty;
            txtClientMedicalState.Text = string.Empty;
            txtClientMedicalZip.Text = string.Empty;
            txtClientMedicalPhoneNumber.Text = string.Empty;
            txtClientMedicalPolicyNumber.Text = string.Empty;
            txtClientMedicalMedCalNumber.Text = string.Empty;
            txtClientMedicalMedCareNumber.Text = string.Empty;
            txtClaimNumber.Text = string.Empty;
        }
        private void ClearClientDefendentInfo()
        {
            txtDefendantLastName.Text = string.Empty;
            txtDefendantFirstName.Text = string.Empty;
            txtDefendantAddress.Text = string.Empty;
            txtDefendantCity.Text = string.Empty;
            txtDefendantState.Text = string.Empty;
            txtDefendantZip.Text = string.Empty;
            txtDefendantHomePhone.Text = string.Empty;
            txtDefendantBusinessPhone.Text = string.Empty;
            dtDefendantDateOfBirth.Text = string.Empty;
            txtDefendantDrivingLicense.Text = string.Empty;
        }
        private void ClearClientDefendentAttorneyInfo()
        {
            txtDefendantAttorneyFirm.Text = string.Empty;
            txtDefendantAttorneyName.Text = string.Empty;
            txtDefendantAttorneyAddress.Text = string.Empty;
            txtDefendantAttorneyCity.Text = string.Empty;
            txtDefendantAttorneyState.Text = string.Empty;
            txtDefendantAttorneyZip.Text = string.Empty;
            txtDefendantAttorneyPhone.Text = string.Empty;
        }
        private void ClearClientDefendentInsuranceInfo()
        {
            txtDefendantInsuranceNameOfInsured.Text = string.Empty;
            txtDefendantInsuranceCompany.Text = string.Empty;
            txtDefendantInsuranceAddress.Text = string.Empty;
            txtDefendantInsuranceCity.Text = string.Empty;
            txtDefendantInsuranceState.Text = string.Empty;
            txtDefendantInsuranceZip.Text = string.Empty;
            txtDefendantInsurancePhone.Text = string.Empty;
            txtDefendantInsuranceAdjuster.Text = string.Empty;
            txtDefendantInsuranceClaimNumber.Text = string.Empty;
            txtDefendantInsurancePolicyLimits.Text = string.Empty;
        }
        private void ClearClientStatuteInfo()
        {
            cbIsItAGovernmentClaim.IsChecked = false;
            dtStatuteComplaintFiledDate.Text = string.Empty;
            lbl1YrFromDateOfAccident.Content = string.Empty;
            lbl2YrFromDateOfAccident.Content = string.Empty;
            dtStatuteComplaintFiledDate.SelectedDate = null;
            cbIsItAGovernmentClaim.IsChecked = false;
            dtCityDeniedDate.SelectedDate = null;
            dtCityFileDate.SelectedDate = null;
            tbCityClaimDueDate.Content = string.Empty;
            lblCity60DaysFromFiledDate.Content = string.Empty;
            lblCity2yearsDate.Content = string.Empty;
            lblCity3yearsDate.Content = string.Empty;
            lblCity5yearsDate.Content = string.Empty;
        }
        private void ClearClientCourtInfo()
        {
            txtCourtInfoCaseNumber.Text = string.Empty;
            txtCourtInfoCourt.Text = string.Empty;
            txtCourtInfoAddress.Text = string.Empty;
            txtCourtInfoCity.Text = string.Empty;
            txtCourtInfoState.Text = string.Empty;
            txtCourtInfoZip.Text = string.Empty;

        }
        private void ClearMiscNotesInfo()
        {
            dgMiscNotes.ItemsSource = null;
            txtMiscNoteName.Text = string.Empty;
            txtMiscNoteDescription.Text = string.Empty;
            dtMiscNoteDate.SelectedDate = null;
        }
        #endregion

        #region Event Handlers

        #region Grid - All clients

        private void btnExpandCollapse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string con = DBHelper.DB_CONNECTION.State.ToString(); ;

                bool action = false;
                if (btnExpandCollapse.Content.ToString() == "Expand All")
                {
                    btnExpandCollapse.Content = "Collapse All";
                    action = true;
                }
                else
                {
                    btnExpandCollapse.Content = "Expand All";
                }
                exdAdditionalClientInformation.IsExpanded = action;
                exdClientAutoInformation.IsExpanded = action;
                exdClientMedicalInsurance.IsExpanded = action;
                exdCourtInformation.IsExpanded = action;
                exdCourtInformation.IsExpanded = action;
                exdDefendantInformation.IsExpanded = action;
                exdDefendantInsuranceInformation.IsExpanded = action;
                exdMiscNotes.IsExpanded = action;
                exdStatuteInformation.IsExpanded = action;
                exdSpouseInfo.IsExpanded = action;
                exdClientInjuries.IsExpanded = action;
                exdDefendantAttorneyInformation.IsExpanded = action;
                exdGovertmentClaims.IsExpanded = action;
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

        private void cbShowAllInactiveClients_Checked(object sender, RoutedEventArgs e)
        {
            FillClientFileList(Constants.ALL_CLIENT_NAME_FILE_ID_QUERY);
        }

        private void cbShowAllInactiveClients_Unchecked(object sender, RoutedEventArgs e)
        {
            FillClientFileList(Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY);
        }

        private void dgClientFileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClientFileInformation selectedClient = dgClientFileList.SelectedItem as ClientFileInformation;
                if (selectedClient == null)
                    return;

                DisplaySelectedClientInformation(selectedClient);
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

        private void ResetClientFileGridSelection()
        {
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }
        #endregion

        #region Client Evidences

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

        #region Add New Client

        private void btnAddNewClient_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllControlsStatus(false);
            ClearAllControls();
            FillAllCaseTypeDropDown();
            FillAllCaseStatusDropDown();
            dgClientFileList.SelectedIndex = -1;
            actionMainClient = "Add";
            //btnEditClient.IsEnabled = false;
            btnAddNewClient.IsEnabled = false;
            btnCancelEditGeneralClient.IsEnabled = false;
            btnSaveEditGeneralClient.IsEnabled = false;
            //TODO: Make Add New available in Evidence dialog
        }

        #endregion

        #region Save Client Details

        private void btnSaveClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidateRequiredFieldsProvided())
                {
                    Helper.ShowErrorMessageBox("Please provide all mandatory information", "Client Information");
                    return;
                }

                if (actionMainClient == "Add")
                {
                    SaveClientDetails();
                }
                else if (actionMainClient == "Edit")
                {
                    UpdateClientDetails();
                }
                FillClientFileList(Constants.ACTIVE_CLIENT_NAME_FILE_ID_QUERY);
                Helper.ShowInformationMessageBox("Client Saved successfully!");
                UpdateAllControlsStatus(true);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private bool IsAllMandatoryFieldsEntered()
        {
            bool result = false;
            //if(string.IsNullOrEmpty(txtFileNo.Text.Trim()
            return result;
        }

        #endregion

        #region Edit Client Details
        private void btnEditClient_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllControlsStatus(false);
            FillAllCaseTypeDropDown();
            FillAllCaseStatusDropDown();
            dgClientFileList.SelectedIndex = -1;
            actionMainClient = "Edit";
            btnAddNewClient.IsEnabled = false;
            //btnEditClient.IsEnabled = false;

        }
        #endregion

        private void btnAddNewClientMedicalInsuranceNote_Click(object sender, RoutedEventArgs e)
        {
            txtClientMedicalInsuranceNoteName.Text = string.Empty;
            dtClientMedicalNoteDate.SelectedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            txtClientMedicalInsuranceNoteDescription.Text = string.Empty;

            btnSaveClientMedicalInsuranceNote.IsEnabled = true;
            btnAddNewClientMedicalInsuranceNote.IsEnabled = false;
            btnEditClientMedicalInsuranceNote.IsEnabled = false;

            txtClientMedicalInsuranceNoteName.IsReadOnly = false;
            txtClientMedicalInsuranceNoteDescription.IsReadOnly = false;
            actionClientMedicalInsuranceNote = "Add";
            btnDeleteClientMedicalInsuranceNote.IsEnabled = false;
        }

        #region Client Medical Notes

        private void btnSaveClientMedicalInsuranceNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(actionClientMedicalInsuranceNote))
                {
                    btnSaveClientMedicalInsuranceNote.IsEnabled = false;
                    txtClientMedicalInsuranceNoteName.IsReadOnly = true;
                    txtClientMedicalInsuranceNoteDescription.IsReadOnly = true;
                    btnEditClientMedicalInsuranceNote.IsEnabled = true;
                    btnAddNewClientMedicalInsuranceNote.IsEnabled = true;
                    btnDeleteClientMedicalInsuranceNote.IsEnabled = true;
                    return;
                }
                string query = string.Empty;

                if (actionClientMedicalInsuranceNote == "Add")
                {
                    ClientInjury injury = new ClientInjury();
                    injury.Note = txtClientMedicalInsuranceNoteName.Text;
                    injury.CreatedDate = dtClientMedicalNoteDate.SelectedDate.Value.ToShortDateString();
                    injury.Description = txtClientMedicalInsuranceNoteDescription.Text;
                    BusinessLogic.SaveClientInjury(injury, txtFileNo.Text);
                }
                else if (actionClientMedicalInsuranceNote == "Edit")
                {
                    ClientInjury selectedValue = dgClientMedicalInsuranceNotes.SelectedValue as ClientInjury;
                    query = string.Format(Constants.UPDATE_CLIENT_INJURY_NOTE_QUERY, txtClientMedicalInsuranceNoteName.Text, selectedValue.CreatedDate, dtClientMedicalNoteDate.SelectedDate.Value.ToShortDateString(), txtClientMedicalInsuranceNoteDescription.Text,
                        selectedValue.FileID, selectedValue.Note, selectedValue.CreatedDate, selectedValue.LastModifiedDate, selectedValue.Description);
                    BusinessLogic.UpdateClientInjury(query);
                }

                FillClientInjuryList(txtFileNo.Text);

                btnSaveClientMedicalInsuranceNote.IsEnabled = false;
                btnAddNewClientMedicalInsuranceNote.IsEnabled = true;
                btnDeleteClientMedicalInsuranceNote.IsEnabled = true;
                btnEditClientMedicalInsuranceNote.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dgClientMedicalInsuranceNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ClientInjury selectedValue = dgClientMedicalInsuranceNotes.SelectedValue as ClientInjury;
                if (selectedValue != null)
                {
                    txtClientMedicalInsuranceNoteName.Text = selectedValue.Note;
                    //TODO: Ask Jay what should be updated, CreatedDate or LastModified
                    dtClientMedicalNoteDate.SelectedDate = DateTime.Parse(selectedValue.LastModifiedDate);
                    txtClientMedicalInsuranceNoteDescription.Text = selectedValue.Description;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnEditClientMedicalInsuranceNote_Click(object sender, RoutedEventArgs e)
        {
            btnSaveClientMedicalInsuranceNote.IsEnabled = true;
            btnEditClientMedicalInsuranceNote.IsEnabled = true;
            btnAddNewClientMedicalInsuranceNote.IsEnabled = true;

            txtClientMedicalInsuranceNoteName.IsReadOnly = false;
            txtClientMedicalInsuranceNoteDescription.IsReadOnly = false;
            actionClientMedicalInsuranceNote = "Edit";

        }

        private void btnDeleteClientMedicalInsuranceNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientInjury selectedValue = dgClientMedicalInsuranceNotes.SelectedValue as ClientInjury;
                if (selectedValue == null)
                    return;
                string query = string.Format(Constants.DELETE_CLIENT_INJURY_NOTE_QUERY, selectedValue.FileID, selectedValue.Note, selectedValue.CreatedDate, selectedValue.LastModifiedDate, selectedValue.Description);
                DBHelper.ExecuteNonQuery(query);

                FillClientInjuryList(txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Statute Information

        private void dtAccidentDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtAccidentDate.SelectedDate != null)
                {
                    lbl1YrFromDateOfAccident.Content = dtAccidentDate.SelectedDate.Value.AddYears(1).ToShortDateString();
                    lbl2YrFromDateOfAccident.Content = dtAccidentDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtStatuteComplaintFiledDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtStatuteComplaintFiledDate.SelectedDate != null)
                {
                    lblStatuteComplaint60DaysFromFiledDate.Content = dtStatuteComplaintFiledDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                    lblStatuteComplaint2yearsDate.Content = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                    lblStatuteComplaint3yearsDate.Content = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                    lblStatuteComplaint5yearsDate.Content = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void cbIsItAGovernmentClaim_Checked(object sender, RoutedEventArgs e)
        {
            exdGovertmentClaims.IsExpanded = true;
        }

        private void cbIsItAGovernmentClaim_Unchecked(object sender, RoutedEventArgs e)
        {
            exdGovertmentClaims.IsExpanded = false;
        }

        #region Govt Claim Tab

        private void dtCityDeniedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtCityDeniedDate.SelectedDate != null)
                {
                    tbCityClaimDueDate.Content = dtCityDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
                }
                else
                {
                    tbCityClaimDueDate.Content = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtCityFileDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dtCityFileDate.SelectedDate != null)
                {
                    lblCity60DaysFromFiledDate.Content = dtCityFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                    lblCity2yearsDate.Content = dtCityFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                    lblCity3yearsDate.Content = dtCityFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                    lblCity5yearsDate.Content = dtCityFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                }
                else
                {
                    lblCity60DaysFromFiledDate.Content = string.Empty;
                    lblCity2yearsDate.Content = string.Empty;
                    lblCity3yearsDate.Content = string.Empty;
                    lblCity5yearsDate.Content = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtCountyDeniedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tbCountyClaimDueDate.Content = dtCountyDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtCountyFileDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lblCounty60DaysFromFiledDate.Content = dtCountyDeniedDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                lblCounty2yearsDate.Content = dtCountyDeniedDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                lblCounty3yearsDate.Content = dtCountyDeniedDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                lblCounty5yearsDate.Content = dtCountyDeniedDate.SelectedDate.Value.AddYears(5).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtStateDeniedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tbStateClaimDueDate.Content = dtStateDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtStateFileDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lblState60DaysFromFiledDate.Content = dtStateFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                lblState2yearsDate.Content = dtStateFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                lblState3yearsDate.Content = dtStateFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                lblState5yearsDate.Content = dtStateFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtOtherDeniedDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tbOtherClaimDueDate.Content = dtOtherDeniedDate.SelectedDate.Value.AddDays(180).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void dtOtherFileDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lblOther60DaysFromFiledDate.Content = dtOtherFileDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                lblOther2yearsDate.Content = dtOtherFileDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                lblOther3yearsDate.Content = dtOtherFileDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                lblOther5yearsDate.Content = dtOtherFileDate.SelectedDate.Value.AddYears(5).ToShortDateString();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #endregion

        #region Misc Notes

        private void dgMiscNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MiscNotes selectedValue = dgMiscNotes.SelectedValue as MiscNotes;
                if (selectedValue != null)
                {
                    txtMiscNoteName.Text = selectedValue.NoteNumber;
                    //TODO: Ask Jay what should be updated, CreatedDate or LastModified
                    if (!string.IsNullOrEmpty(selectedValue.ModifiedDate))
                    {
                        dtMiscNoteDate.SelectedDate = DateTime.Parse(selectedValue.ModifiedDate);
                    }
                    else
                    {
                        dtMiscNoteDate.SelectedDate = null;
                    }
                    txtMiscNoteDescription.Text = selectedValue.Description;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnAddNewMiscNote_Click(object sender, RoutedEventArgs e)
        {
            txtMiscNoteName.IsReadOnly = false;
            txtMiscNoteDescription.IsReadOnly = false;
            dtMiscNoteDate.IsEnabled = true;
            btnSaveMiscNote.IsEnabled = true;
            actionMiscNote = "Add";

            btnAddNewMiscNote.IsEnabled = false;
            btnEditMiscNote.IsEnabled = false;
            btnSaveMiscNote.IsEnabled = true;
        }
        private void btnEditMiscNote_Click(object sender, RoutedEventArgs e)
        {
            txtMiscNoteName.IsReadOnly = false;
            txtMiscNoteDescription.IsReadOnly = false;
            dtMiscNoteDate.IsEnabled = true;
            btnSaveMiscNote.IsEnabled = true;
            actionMiscNote = "Edit";

            btnAddNewMiscNote.IsEnabled = false;
            btnEditMiscNote.IsEnabled = false;
            btnSaveMiscNote.IsEnabled = true;
        }

        private void btnSaveMiscNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(actionMiscNote))
                {
                    btnSaveMiscNote.IsEnabled = false;
                    txtMiscNoteName.IsReadOnly = true;
                    txtMiscNoteDescription.IsReadOnly = true;
                    return;
                }
                string query = string.Empty;

                if (actionMiscNote == "Add")
                {
                    MiscNotes miscNote = new MiscNotes();
                    miscNote.NoteNumber = txtMiscNoteName.Text;
                    miscNote.CreatedDate = dtMiscNoteDate.SelectedDate.Value.ToShortDateString();
                    miscNote.ModifiedDate = dtMiscNoteDate.SelectedDate.Value.ToShortDateString();
                    miscNote.Description = txtMiscNoteDescription.Text;
                    BusinessLogic.SaveMiscNotes(miscNote, txtFileNo.Text);
                }
                else if (actionMiscNote == "Edit")
                {
                    MiscNotes selectedValue = dgMiscNotes.SelectedValue as MiscNotes;
                    query = string.Format(Constants.UPDATE_MISC_NOTES_QUERY, txtMiscNoteName.Text, selectedValue.CreatedDate, dtMiscNoteDate.SelectedDate.Value.ToShortDateString(),
                                txtMiscNoteDescription.Text, txtFileNo.Text, selectedValue.NoteNumber, selectedValue.CreatedDate, selectedValue.ModifiedDate, selectedValue.Description);
                    int result = DBHelper.ExecuteNonQuery(query);
                }
                FillClientMiscList(txtFileNo.Text);

                btnAddNewMiscNote.IsEnabled = true;
                btnEditMiscNote.IsEnabled = true;
                btnSaveMiscNote.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnDeleteMiscNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MiscNotes selectedValue = dgMiscNotes.SelectedValue as MiscNotes;
                string query = string.Format(Constants.DELETE_MISC_NOTES_QUERY, txtFileNo.Text, selectedValue.NoteNumber, selectedValue.CreatedDate, selectedValue.ModifiedDate, selectedValue.Description);
                int result = DBHelper.ExecuteNonQuery(query);
                FillClientMiscList(txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        #endregion

        #region Close - Reactivate Clients

        private void btnCloseClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //var result = MessageBox.Show("Are you sure to close this client?", "Client Information", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                var result = Helper.ShowQuestionMessageBox("Are you sure to close this client?");
                if (result == MessageBoxResult.Yes)
                {
                    bool success = BusinessLogic.CloseClient(txtFileNo.Text);
                    if (success)
                    {
                        Helper.ShowInformationMessageBox("Selected client has been closed!");
                        //MessageBox.Show("Selected client has been closed!", "Client Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        RefreshClientFileList();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnReactivateClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = Helper.ShowQuestionMessageBox("Are you sure to Re-Activate this client?");
                if (result == MessageBoxResult.Yes)
                {
                    bool success = BusinessLogic.ReactivateClient(txtFileNo.Text);
                    if (success)
                    {
                        Helper.ShowInformationMessageBox("Selected client has been Re-Activated!");
                        RefreshClientFileList();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllControlsStatus(true);
            ClearAllControls();
            actionMainClient = string.Empty;
            btnAddNewClient.IsEnabled = true;
            //btnEditClient.IsEnabled = true;
            btnCancelEdit.IsEnabled = false;

        }

        #region Edit General Section

        private void btnEditGeneralClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateStatusGeneralSection(false);

                txtFileNo.IsReadOnly = true;

                int previousCaseStatusIndex = cmbCaseStatus.SelectedIndex;
                int previousCaseTypeIndex = cmbCaseType.SelectedIndex;

                selectedGeneralInfo.FileNo = txtFileNo.Text;
                selectedGeneralInfo.LastName = txtLastName.Text;
                selectedGeneralInfo.FirstName = txtFirstName.Text;
                if (cmbCaseType.Text == "Other")
                {
                    selectedGeneralInfo.CaseType = txtOtherCaseType.Text;
                }
                else
                {
                    selectedGeneralInfo.CaseType = cmbCaseType.Text;
                }
                selectedGeneralInfo.CaseStatus = cmbCaseStatus.Text;
                if (dtAccidentDate.SelectedDate.Value != null)
                {
                    selectedGeneralInfo.AccidentDate = DateTime.Parse(dtAccidentDate.SelectedDate.Value.ToShortDateString());
                }
                selectedGeneralInfo.Address = txtAddress.Text;
                selectedGeneralInfo.City = txtCity.Text;
                selectedGeneralInfo.State = txtState.Text;
                selectedGeneralInfo.HomePhone = txtHomePhone.Text;
                selectedGeneralInfo.CellPhone = txtCellPhone.Text;
                selectedGeneralInfo.DrivingLicense = txtDrivingLicense.Text;
                selectedGeneralInfo.DateOfBirth = DateTime.Parse(dtDateOfBirth.SelectedDate.Value.ToShortDateString());
                selectedGeneralInfo.SSN = txtSSN.Text;

                selectedGeneralInfo.WorkPhone = txtWorkPhone.Text;
                selectedGeneralInfo.SuiteAddress = txtSuiteAddress.Text;
                selectedGeneralInfo.Email= txtEmail.Text;

                selectedGeneralInfo.InitialCaseInformation = txtInitialCaseInformation.Text;
                selectedGeneralInfo.DefendantName = txtDefendantName.Text;

                selectedGeneralInfo.OriginatingAttorney = txtOriginatingAttorney.Text;
                selectedGeneralInfo.AssignedAttorney = txtAssignedAttorney.Text;
                selectedGeneralInfo.Referral = txtRefferal.Text;

                FillAllCaseTypeDropDown();
                cmbCaseType.SelectedIndex = previousCaseStatusIndex;
                FillAllCaseStatusDropDown();
                cmbCaseStatus.SelectedIndex = previousCaseTypeIndex;
                btnEditGeneralClient.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnSaveEditGeneralClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientGeneralInformation generalInfo = new ClientGeneralInformation();
                generalInfo.LastName = txtLastName.Text;
                generalInfo.FirstName = txtFirstName.Text;
                if (cmbCaseType.Text == "Other")
                {
                    generalInfo.CaseType = txtOtherCaseType.Text;
                }
                else
                {
                    generalInfo.CaseType = cmbCaseType.Text;
                }
                generalInfo.CaseStatus = cmbCaseStatus.Text;
                if (dtAccidentDate.SelectedDate.Value != null)
                {
                    generalInfo.AccidentDate = DateTime.Parse(dtAccidentDate.SelectedDate.Value.ToShortDateString());
                }
                generalInfo.ClientCreatedOn = DateTime.Now.ToShortDateString();
                generalInfo.Address = txtAddress.Text;
                generalInfo.City = txtCity.Text;
                generalInfo.State = txtState.Text;
                generalInfo.HomePhone = txtHomePhone.Text;
                generalInfo.CellPhone = txtCellPhone.Text;
                generalInfo.DrivingLicense = txtDrivingLicense.Text;
                if (dtDateOfBirth.SelectedDate != null)
                {
                    generalInfo.DateOfBirth = DateTime.Parse(dtDateOfBirth.SelectedDate.Value.ToShortDateString());
                }
                generalInfo.SSN = txtSSN.Text;

                generalInfo.WorkPhone= txtWorkPhone.Text;
                generalInfo.SuiteAddress= txtSuiteAddress.Text;
                generalInfo.Email = txtEmail.Text;

                generalInfo.InitialCaseInformation = txtInitialCaseInformation.Text;
                generalInfo.DefendantName = txtDefendantName.Text;

                generalInfo.OriginatingAttorney = txtOriginatingAttorney.Text;
                generalInfo.AssignedAttorney = txtAssignedAttorney.Text;
                generalInfo.Referral = txtRefferal.Text;

                BusinessLogic.UpdateClientGeneralInfo(generalInfo, selectedGeneralInfo);
                Helper.ShowInformationMessageBox("Client General Information Saved.");
                btnCancelEditGeneralClient.IsEnabled = false;
                btnSaveEditGeneralClient.IsEnabled = false;
                btnEditGeneralClient.IsEnabled = true;

                UpdateStatusGeneralSection(true);

                ResetClientFileGridSelection();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelEditGeneralClient_Click(object sender, RoutedEventArgs e)
        {
            btnSaveEditGeneralClient.IsEnabled = false;
            btnCancelEditGeneralClient.IsEnabled = false;
            btnEditGeneralClient.IsEnabled = true;
            btnViewAllEvidence.IsEnabled = false;
            ResetClientFileGridSelection();
        }

        #endregion

        #region Edit Additional Client

        private void btnEditAdditionalClientInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientSpouseInfo(false);
            UpdateStatusClientAdditionalInfo(false);
            btnEditAdditionalClientInfo.IsEnabled = false;

        }
        private void btnSaveAdditionalClientInfo_Click(object sender, RoutedEventArgs e)
        {
            updateAdditionalClientInformation();
            Helper.ShowInformationMessageBox("Addional Client Information Saved.");
            btnSaveAdditionalClientInfo.IsEnabled = false;
            btnCancelAdditionalClientInfo.IsEnabled = false;
            btnEditAdditionalClientInfo.IsEnabled = true;
            UpdateStatusClientSpouseInfo(true);
            UpdateStatusClientAdditionalInfo(true);
        }
        private void btnCancelAdditionalClientInfo_Click(object sender, RoutedEventArgs e)
        {
            btnCancelAdditionalClientInfo.IsEnabled = false;
            btnSaveAdditionalClientInfo.IsEnabled = false;
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
            btnEditAdditionalClientInfo.IsEnabled = true;
            UpdateStatusClientSpouseInfo(true);
            UpdateStatusClientAdditionalInfo(true);

        }
        private void updateAdditionalClientInformation()
        {
            try
            {
                if (string.IsNullOrEmpty(txtClientOccupation.Text) &&
                       string.IsNullOrEmpty(txtEmployer.Text) &&
                       string.IsNullOrEmpty(txtClientEmployerAddress.Text) &&
                       string.IsNullOrEmpty(txtClientEmployerCity.Text) &&
                       string.IsNullOrEmpty(txtClientEmployerState.Text) &&
                       string.IsNullOrEmpty(txtClientSpousLastName.Text) &&
                       string.IsNullOrEmpty(txtClientSpousFirstName.Text) &&
                       string.IsNullOrEmpty(txtClientSpousOccupation.Text) &&
                       string.IsNullOrEmpty(txtSpouseEmployer.Text) &&
                       string.IsNullOrEmpty(txtClientSpouseEmployerAddress.Text) &&
                       string.IsNullOrEmpty(txtClientSpouseEmployerCity.Text) &&
                       string.IsNullOrEmpty(txtClientSpouseEmployerState.Text))
                {
                    return;
                }
                ClientAdditionalInformation additionalInfo = new ClientAdditionalInformation();
                additionalInfo.Occupation = txtClientOccupation.Text;
                additionalInfo.Employer = txtEmployer.Text;
                additionalInfo.Address = txtClientEmployerAddress.Text;
                additionalInfo.City = txtClientEmployerCity.Text;
                additionalInfo.State = txtClientEmployerState.Text;
                additionalInfo.SpouseInfo = new SpouseInformation();
                additionalInfo.SpouseInfo.LastName = txtClientSpousLastName.Text;
                additionalInfo.SpouseInfo.FirstName = txtClientSpousFirstName.Text;
                additionalInfo.SpouseInfo.Occupation = txtClientSpousOccupation.Text;
                additionalInfo.SpouseInfo.Employer = txtSpouseEmployer.Text;
                additionalInfo.SpouseInfo.Address = txtClientSpouseEmployerAddress.Text;
                additionalInfo.SpouseInfo.City = txtClientSpouseEmployerCity.Text;
                additionalInfo.SpouseInfo.State = txtClientSpouseEmployerState.Text;

                BusinessLogic.UpdateClientAdditionalInformation(additionalInfo, txtFileNo.Text);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        #region Edit Client Auto

        private void btnEditClientAutoInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientAutoInfo(false);
            btnEditClientAutoInfo.IsEnabled = false;
        }

        private void btnSaveClientAutoInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtClientAutoInsuranceCompany.Text) &&
                       string.IsNullOrEmpty(txtClientAutoAddress.Text) &&
                       string.IsNullOrEmpty(txtClientAutoCity.Text) &&
                       string.IsNullOrEmpty(txtClientAutoState.Text) &&
                       string.IsNullOrEmpty(txtClientAutoZip.Text) &&
                       string.IsNullOrEmpty(txtClientAutoPhoneNumber.Text) &&
                       string.IsNullOrEmpty(txtPolicyNumber.Text) &&
                       dtEffectiveStartDate.SelectedDate == null &&
                       dtEffectiveEndtDate.SelectedDate == null &&
                       string.IsNullOrEmpty(txtMedPayAvailable.Text) &&
                       string.IsNullOrEmpty(txtAmount.Text) &&
                       string.IsNullOrEmpty(txtLiabilityMinimumCoverage.Text) &&
                       string.IsNullOrEmpty(txtLiabilityMaximumCoverage.Text) &&
                       string.IsNullOrEmpty(txtUMIMinimum.Text) &&
                       string.IsNullOrEmpty(txtUMIMaximum.Text) &&
                       string.IsNullOrEmpty(txtReimbursable.Text) &&
                       string.IsNullOrEmpty(txtClientAutoInfoNotes.Text))
                {
                    return;
                }

                ClientAutoInformation autoInfo = new ClientAutoInformation();
                autoInfo.InsuranceCompany = txtClientAutoInsuranceCompany.Text;
                autoInfo.Address = txtClientAutoAddress.Text;
                autoInfo.City = txtClientAutoCity.Text;
                autoInfo.State = txtClientAutoState.Text;
                autoInfo.Zip = txtClientAutoZip.Text;
                autoInfo.PhoneNumber = txtClientAutoPhoneNumber.Text;
                autoInfo.Adjuster = txtClientAutoAdjuster.Text;
                autoInfo.PolicyNumber = txtPolicyNumber.Text;
                if (dtEffectiveStartDate.SelectedDate != null)
                {
                    autoInfo.EffectiveStartDate = DateTime.Parse(dtEffectiveStartDate.SelectedDate.Value.ToShortDateString());
                }
                if (dtEffectiveEndtDate.SelectedDate != null)
                {
                    autoInfo.EffectiveEndDate = DateTime.Parse(dtEffectiveEndtDate.SelectedDate.Value.ToShortDateString());
                }
                autoInfo.MedPayAvailable = txtMedPayAvailable.Text;
                autoInfo.Amout = txtAmount.Text;
                autoInfo.LiabilityMinimumCoverage = txtLiabilityMinimumCoverage.Text;
                autoInfo.LiabilityMaximumCoverage = txtLiabilityMaximumCoverage.Text;
                autoInfo.UMIMinimum = txtUMIMinimum.Text;
                autoInfo.UMIMaximum = txtUMIMaximum.Text;
                autoInfo.Reimbursable = txtReimbursable.Text;
                autoInfo.Notes = txtClientAutoInfoNotes.Text;

                BusinessLogic.UpdateClientAutoInformation(autoInfo, txtFileNo.Text);

                Helper.ShowInformationMessageBox("Client Auto Information Saved.");
                btnCancelClientAutoInfo.IsEnabled = false;
                btnSaveClientAutoInfo.IsEnabled = false;
                btnEditClientAutoInfo.IsEnabled = true;
                UpdateStatusClientAutoInfo(true);

            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelClientAutoInfo_Click(object sender, RoutedEventArgs e)
        {
            btnCancelClientAutoInfo.IsEnabled = false;
            btnSaveClientAutoInfo.IsEnabled = false;
            btnEditClientAutoInfo.IsEnabled = true;
            UpdateStatusClientAutoInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }

        #endregion

        #region Edit Client Medical Insurance

        private void btnEditClientMedicalInsurance_Click(object sender, RoutedEventArgs e)
        {
            UpdateClientMedicalInfo(false);
        }

        private void btnSaveClientMedicalInsurance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNamedInsured.Text) &&
                        string.IsNullOrEmpty(txtInsuranceCompany.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalAddress.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalCity.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalState.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalZip.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalPhoneNumber.Text) &&
                        string.IsNullOrEmpty(txtClaimNumber.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalPolicyNumber.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalMedCalNumber.Text) &&
                        string.IsNullOrEmpty(txtClientMedicalMedCareNumber.Text))
                {
                    return;
                }
                ClientMedicalInformation mediInfo = new ClientMedicalInformation();
                mediInfo.NamedInsured = txtNamedInsured.Text;
                mediInfo.InsuranceCompany = txtInsuranceCompany.Text;
                mediInfo.Address = txtClientMedicalAddress.Text;
                mediInfo.City = txtClientMedicalCity.Text;
                mediInfo.State = txtClientMedicalState.Text;
                mediInfo.Zip = txtClientMedicalZip.Text;
                mediInfo.PhoneNumber = txtClientMedicalPhoneNumber.Text;
                mediInfo.PolicyNumber = txtClientMedicalPolicyNumber.Text;
                mediInfo.MediCalNumber = txtClientMedicalMedCalNumber.Text;
                mediInfo.MediCareNumber = txtClientMedicalMedCareNumber.Text;
                mediInfo.ClaimNumber = txtClaimNumber.Text;

                BusinessLogic.UpdateClientMedicationInformation(mediInfo, txtFileNo.Text);
                Helper.ShowInformationMessageBox("Client Medical Information Saved.");
                btnSaveClientMedicalInsurance.IsEnabled = false;
                btnCancelClientMedicalInsurance.IsEnabled = false;
                btnEditClientMedicalInsurance.IsEnabled = true;

                UpdateClientMedicalInfo(true);
                UpdateClientInjuryInfo(true);
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelClientMedicalInsurance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAddNewClientMedicalInsuranceNote.IsEnabled = false;
                btnEditClientMedicalInsuranceNote.IsEnabled = false;
                btnDeleteClientMedicalInsuranceNote.IsEnabled = false;
                btnCancelClientMedicalInsurance.IsEnabled = false;
                btnSaveClientMedicalInsurance.IsEnabled = false;
                
                UpdateClientMedicalInfo(true);
                UpdateClientInjuryInfo(true);

                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = 0;
                
             }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
          
        }

        #endregion

        #region Edit Defendant Information
        private void btnEditDefendantInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientDefendentInfo(false);
            UpdateStatusClientDefendentAttorneyInfo(false);
        }

        private void btnSaveDefendantInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDefendantLastName.Text) &&
                        string.IsNullOrEmpty(txtDefendantFirstName.Text) &&
                        string.IsNullOrEmpty(txtDefendantAddress.Text) &&
                        string.IsNullOrEmpty(txtDefendantCity.Text) &&
                        string.IsNullOrEmpty(txtDefendantState.Text) &&
                        string.IsNullOrEmpty(txtDefendantZip.Text) &&
                        string.IsNullOrEmpty(txtDefendantHomePhone.Text) &&
                        string.IsNullOrEmpty(txtDefendantBusinessPhone.Text) &&
                        dtDefendantDateOfBirth.SelectedDate == null &&
                        string.IsNullOrEmpty(txtDefendantDrivingLicense.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyFirm.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyName.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyAddress.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyCity.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyState.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyZip.Text) &&
                        string.IsNullOrEmpty(txtDefendantAttorneyPhone.Text))
                {
                    return;
                }
                ClientDefendantInformation defInfo = new ClientDefendantInformation();
                defInfo.LastName = txtDefendantLastName.Text;
                defInfo.FirstName = txtDefendantFirstName.Text;
                defInfo.Address = txtDefendantAddress.Text;
                defInfo.City = txtDefendantCity.Text;
                defInfo.State = txtDefendantState.Text;
                defInfo.Zip = txtDefendantZip.Text;
                defInfo.HomePhone = txtDefendantHomePhone.Text;
                defInfo.BusinessPhone = txtDefendantBusinessPhone.Text;
                if (dtDefendantDateOfBirth.SelectedDate != null)
                {
                    defInfo.DateOfBirth = DateTime.Parse(dtDefendantDateOfBirth.SelectedDate.Value.ToShortDateString());
                }
                defInfo.DrivingLicense = txtDefendantDrivingLicense.Text;

                defInfo.AttorneyInfo = new DefendantAttorneyInformation();
                defInfo.AttorneyInfo.Firm = txtDefendantAttorneyFirm.Text;
                defInfo.AttorneyInfo.Attorney = txtDefendantAttorneyName.Text;
                defInfo.AttorneyInfo.Address = txtDefendantAttorneyAddress.Text;
                defInfo.AttorneyInfo.City = txtDefendantAttorneyCity.Text;
                defInfo.AttorneyInfo.State = txtDefendantAttorneyState.Text;
                defInfo.AttorneyInfo.Zip = txtDefendantAttorneyZip.Text;
                defInfo.AttorneyInfo.Phone = txtDefendantAttorneyPhone.Text;

                BusinessLogic.UpdateClientDefendantInformation(defInfo, txtFileNo.Text);

                Helper.ShowInformationMessageBox("Client Defendant Information Saved.");
                btnSaveDefendantInfo.IsEnabled = false;
                btnCancelDefendantInfo.IsEnabled = false;
                btnEditDefendantInfo.IsEnabled = true;

                UpdateStatusClientDefendentInfo(true);
                UpdateStatusClientDefendentAttorneyInfo(true);

                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelDefendantInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientDefendentInfo(true);
            UpdateStatusClientDefendentAttorneyInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }
        #endregion

        #region Edit Defendant Insurance

        private void btnEditDefendantInsuranceInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientDefendentInsuranceInfo(false);
        }

        private void btnSaveDefendantInsuranceInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDefendantInsuranceNameOfInsured.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceCompany.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceAddress.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceCity.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceState.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceZip.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsurancePhone.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceAdjuster.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsuranceClaimNumber.Text) &&
                         string.IsNullOrEmpty(txtDefendantInsurancePolicyLimits.Text))
                {
                    return;
                }
                DefendantInsuranceDetails defInsurance = new DefendantInsuranceDetails();
                defInsurance.NameOfInsured = txtDefendantInsuranceNameOfInsured.Text;
                defInsurance.InsuranceCompany = txtDefendantInsuranceCompany.Text;
                defInsurance.Address = txtDefendantInsuranceAddress.Text;
                defInsurance.City = txtDefendantInsuranceCity.Text;
                defInsurance.State = txtDefendantInsuranceState.Text;
                defInsurance.Zip = txtDefendantInsuranceZip.Text;
                defInsurance.Phone = txtDefendantInsurancePhone.Text;
                defInsurance.Adjuster = txtDefendantInsuranceAdjuster.Text;
                defInsurance.ClaimNumber = txtDefendantInsuranceClaimNumber.Text;
                defInsurance.PolicyLimits = txtDefendantInsurancePolicyLimits.Text;

                BusinessLogic.UpdateDefendantInsuranceInformation(defInsurance, txtFileNo.Text);
                Helper.ShowInformationMessageBox("Defendant Insurance Information Saved.");
                UpdateStatusClientDefendentInsuranceInfo(true);
                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelDefendantInsuranceInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientDefendentInsuranceInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }

        #endregion

        #region Edit Court Inforamtion

        private void btnEditCourtInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientCourtInfo(false);
        }

        private void btnSaveCourtInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCourtInfoCaseNumber.Text) &&
                       string.IsNullOrEmpty(txtCourtInfoCourt.Text) &&
                       string.IsNullOrEmpty(txtCourtInfoAddress.Text) &&
                       string.IsNullOrEmpty(txtCourtInfoCity.Text) &&
                       string.IsNullOrEmpty(txtCourtInfoState.Text) &&
                       string.IsNullOrEmpty(txtCourtInfoZip.Text))
                {
                    return;
                }
                CourtInformation courtInfo = new CourtInformation();
                courtInfo.CaseNumber = txtCourtInfoCaseNumber.Text;
                courtInfo.Court = txtCourtInfoCourt.Text;
                courtInfo.Address = txtCourtInfoAddress.Text;
                courtInfo.City = txtCourtInfoCity.Text;
                courtInfo.State = txtCourtInfoState.Text;
                courtInfo.Zip = txtCourtInfoZip.Text;

                BusinessLogic.UpdateCourtInformation(courtInfo, txtFileNo.Text);

                UpdateStatusClientCourtInfo(true);
                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelCourtInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientCourtInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }

        #endregion

        #region Edit Statute Information
        private void btnEditStatuteInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientStatuteInfo(false);
        }

        private void btnSaveStatuteInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtStatuteComplaintFiledDate.SelectedDate == null &&
                        dtCityDeniedDate.SelectedDate == null &&
                        dtCityFileDate.SelectedDate == null &&
                        dtCountyDeniedDate.SelectedDate == null &&
                        dtCountyFileDate.SelectedDate == null &&
                        dtStateDeniedDate.SelectedDate == null &&
                        dtStateFileDate.SelectedDate == null &&
                        dtOtherDeniedDate.SelectedDate == null &&
                        dtOtherFileDate.SelectedDate == null)
                {
                    return;
                }
                StatuteInformation statuteInfo = new StatuteInformation();
                if (dtAccidentDate.SelectedDate != null)
                {
                    statuteInfo.AccidentDate = dtAccidentDate.SelectedDate.Value.ToShortDateString();
                    statuteInfo.AccDateAfter1yr = dtAccidentDate.SelectedDate.Value.AddYears(1).ToShortDateString();
                    statuteInfo.AccDateAfter2yr = dtAccidentDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                }
                if (dtStatuteComplaintFiledDate.SelectedDate != null)
                {
                    statuteInfo.ComplaintFileDate = dtStatuteComplaintFiledDate.SelectedDate.Value.ToShortDateString();
                    statuteInfo.ComplaintAfter60days = dtStatuteComplaintFiledDate.SelectedDate.Value.AddDays(60).ToShortDateString();
                    statuteInfo.ComplaintAfter2yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(2).ToShortDateString();
                    statuteInfo.ComplaintAfter3yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(3).ToShortDateString();
                    statuteInfo.ComplaintAfter5yrs = dtStatuteComplaintFiledDate.SelectedDate.Value.AddYears(5).ToShortDateString();
                }
                statuteInfo.IsGovtDClaim = cbIsItAGovernmentClaim.IsChecked.Value;
                if (statuteInfo.IsGovtDClaim)
                {
                    if (dtCityDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.CityClaim = new GovertmentClaimInformation();
                        statuteInfo.CityClaim.DeniedDate = dtCityDeniedDate.SelectedDate.Value.ToString();
                        statuteInfo.CityClaim.ClaimDueDate = dtCityDeniedDate.SelectedDate.Value.AddDays(180).ToString();
                    }
                    if (dtCityFileDate.SelectedDate != null)
                    {
                        statuteInfo.CityClaim = statuteInfo.CityClaim ?? new GovertmentClaimInformation();
                        statuteInfo.CityClaim.FiledDate = dtCityFileDate.SelectedDate.Value.ToString();
                        statuteInfo.CityClaim.FiledDateAfter60Days = dtCityFileDate.SelectedDate.Value.AddDays(60).ToString();
                        statuteInfo.CityClaim.FiledDateAfter2yrs = dtCityFileDate.SelectedDate.Value.AddYears(2).ToString();
                        statuteInfo.CityClaim.FiledDateAfter3yrs = dtCityFileDate.SelectedDate.Value.AddYears(3).ToString();
                        statuteInfo.CityClaim.FiledDateAfter5yrs = dtCityFileDate.SelectedDate.Value.AddYears(5).ToString();
                    }

                    if (dtCountyDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.CountyClaim = new GovertmentClaimInformation();
                        statuteInfo.CountyClaim.DeniedDate = dtCountyDeniedDate.SelectedDate.Value.ToString();
                        statuteInfo.CountyClaim.ClaimDueDate = dtCountyDeniedDate.SelectedDate.Value.AddDays(180).ToString();
                    }
                    if (dtCountyFileDate.SelectedDate != null)
                    {
                        statuteInfo.CountyClaim = statuteInfo.CountyClaim ?? new GovertmentClaimInformation();
                        statuteInfo.CountyClaim.FiledDate = dtCountyFileDate.SelectedDate.Value.ToString();
                        statuteInfo.CountyClaim.FiledDateAfter60Days = dtCountyFileDate.SelectedDate.Value.AddDays(60).ToString();
                        statuteInfo.CountyClaim.FiledDateAfter2yrs = dtCountyFileDate.SelectedDate.Value.AddYears(2).ToString();
                        statuteInfo.CountyClaim.FiledDateAfter3yrs = dtCountyFileDate.SelectedDate.Value.AddYears(3).ToString();
                        statuteInfo.CountyClaim.FiledDateAfter5yrs = dtCountyFileDate.SelectedDate.Value.AddYears(5).ToString();
                    }

                    if (dtStateDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.StateClaim = new GovertmentClaimInformation();
                        statuteInfo.StateClaim.DeniedDate = dtStateDeniedDate.SelectedDate.Value.ToString();
                        statuteInfo.StateClaim.ClaimDueDate = dtStateDeniedDate.SelectedDate.Value.AddDays(180).ToString();
                    }
                    if (dtStateFileDate.SelectedDate != null)
                    {
                        statuteInfo.StateClaim = statuteInfo.StateClaim ?? new GovertmentClaimInformation();
                        statuteInfo.StateClaim.FiledDate = dtStateFileDate.SelectedDate.Value.ToString();
                        statuteInfo.StateClaim.FiledDateAfter60Days = dtStateFileDate.SelectedDate.Value.AddDays(60).ToString();
                        statuteInfo.StateClaim.FiledDateAfter2yrs = dtStateFileDate.SelectedDate.Value.AddYears(2).ToString();
                        statuteInfo.StateClaim.FiledDateAfter3yrs = dtStateFileDate.SelectedDate.Value.AddYears(3).ToString();
                        statuteInfo.StateClaim.FiledDateAfter5yrs = dtStateFileDate.SelectedDate.Value.AddYears(5).ToString();
                    }

                    if (dtOtherDeniedDate.SelectedDate != null)
                    {
                        statuteInfo.OtherClaim = new GovertmentClaimInformation();
                        statuteInfo.OtherClaim.DeniedDate = dtOtherDeniedDate.SelectedDate.Value.ToString();
                        statuteInfo.OtherClaim.ClaimDueDate = dtOtherDeniedDate.SelectedDate.Value.AddDays(180).ToString();
                    }
                    if (dtOtherFileDate.SelectedDate != null)
                    {
                        statuteInfo.OtherClaim = statuteInfo.OtherClaim ?? new GovertmentClaimInformation();
                        statuteInfo.OtherClaim.FiledDate = dtOtherFileDate.SelectedDate.Value.ToString();
                        statuteInfo.OtherClaim.FiledDateAfter60Days = dtOtherFileDate.SelectedDate.Value.AddDays(60).ToString();
                        statuteInfo.OtherClaim.FiledDateAfter2yrs = dtOtherFileDate.SelectedDate.Value.AddYears(2).ToString();
                        statuteInfo.OtherClaim.FiledDateAfter3yrs = dtOtherFileDate.SelectedDate.Value.AddYears(3).ToString();
                        statuteInfo.OtherClaim.FiledDateAfter5yrs = dtOtherFileDate.SelectedDate.Value.AddYears(5).ToString();
                    }
                }
                BusinessLogic.UpdateStatuteInformation(statuteInfo, txtFileNo.Text);

                Helper.ShowInformationMessageBox("Statute Information Saved.");
                btnSaveStatuteInfo.IsEnabled = false;
                btnCancelStatuteInfo.IsEnabled = false;
                btnEditStatuteInfo.IsEnabled = true;

                UpdateStatusClientStatuteInfo(true);
                dgClientFileList.SelectedIndex = -1;
                dgClientFileList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnCancelStatuteInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusClientStatuteInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }
        #endregion

        #region Edit Misc note

        private void btnEditMiscNoteInfoSection_Click(object sender, RoutedEventArgs e)
        {
            btnAddNewMiscNote.IsEnabled = true;
            btnEditMiscNote.IsEnabled = true;
            btnSaveMiscNote.IsEnabled = false;
            btnDeleteMiscNote.IsEnabled = true;
        }

        private void btnCancelMiscNoteInfoSection_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatusMiscNotesInfo(true);
            dgClientFileList.SelectedIndex = -1;
            dgClientFileList.SelectedIndex = 0;
        }

        #endregion

        private void exdAdditionalClientInformation_Expanded(object sender, RoutedEventArgs e)
        {
            exdSpouseInfo.IsExpanded = true;
        }

        private void txtFileNo_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsValidateRequiredFieldsProvided())
                    return;

                if (!txtFileNo.IsReadOnly)
                {
                    bool isPresent = BusinessLogic.IsFileNoAlreadyPresent(txtFileNo.Text);
                    if (isPresent)
                    {
                        Helper.ShowErrorMessageBox("This file ID is already used. You can use next File ID.");
                        txtFileNo.Text = BusinessLogic.GetNewFileID().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private bool IsValidateRequiredFieldsProvided()
        {

            bool isValidationSuccessful = true;
            try
            {
                if (String.IsNullOrEmpty(txtFileNo.Text))
                {
                    txtFileNo.Style = textBoxErrorStyle;
                    isValidationSuccessful = false;
                }
                else
                {
                    txtFileNo.Style = textBoxNormalStyle;
                }
                if (String.IsNullOrEmpty(txtLastName.Text))
                {
                    txtLastName.Style = textBoxErrorStyle;
                    isValidationSuccessful = false;
                }
                else
                {
                    txtLastName.Style = textBoxNormalStyle;
                }
                if (String.IsNullOrEmpty(txtFirstName.Text))
                {
                    txtFirstName.Style = textBoxErrorStyle;
                    isValidationSuccessful = false;
                }
                else
                {
                    txtFirstName.Style = textBoxNormalStyle;
                }
                if (String.IsNullOrEmpty(txtAssignedAttorney.Text))
                {
                    txtAssignedAttorney.Style = textBoxErrorStyle;
                    isValidationSuccessful = false;
                }
                else
                {
                    txtAssignedAttorney.Style = textBoxNormalStyle;
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
            return isValidationSuccessful;
        }

        private void btnClientBilling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clientBillingHandler == null)
                {
                    clientBillingHandler = new ClientBilling();
                }
                string fileNo = txtFileNo.Text;
                if (btnSaveClient.IsEnabled == true)
                {
                    var result = MessageBox.Show("Do you want to exit without saving current changes?", "Client Information", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        ShowClientBilling();
                    }
                }
                else
                {
                    ShowClientBilling();
                }

            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowClientBilling()
        {
            clientBillingHandler.SelectedFileNumber = txtFileNo.Text;
            if (clientBillingHandler.WindowState == System.Windows.WindowState.Minimized)
            {
                clientBillingHandler.WindowState = System.Windows.WindowState.Maximized;
                clientBillingHandler.Activate();
            }
            clientBillingHandler.Show();
            //this.WindowState = System.Windows.WindowState.Minimized;
            this.Close();
        }

        private void btnClientReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (reportsHandler == null)
                {
                    reportsHandler = new Reports();
                }
                string fileNo = txtFileNo.Text;
                if (btnSaveClient.IsEnabled == true)
                {
                    var result = MessageBox.Show("Do you want to exit without saving current changes?", "Client Information", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        ShowReportForm();
                    }
                }
                else
                {
                    ShowReportForm();
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowReportForm()
        {
            if (reportsHandler.WindowState == System.Windows.WindowState.Minimized)
            {
                reportsHandler.WindowState = System.Windows.WindowState.Normal;
                reportsHandler.Activate();
            }
            reportsHandler.Show();
            this.Close();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HomePageHandler != null)
                {
                    HomePageHandler.Activate();
                    HomePageHandler.WindowState = System.Windows.WindowState.Normal;
                    HomePageHandler.clientInfoHandler = null;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
        #endregion

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HomePageHandler != null)
                {
                    if (btnSaveClient.IsEnabled == true)
                    {
                        var result = MessageBox.Show("Do you want to exit without saving current changes?", "Client Information", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Yes)
                        {
                            ShowHomePage();
                        }
                    }
                    else
                    {
                        ShowHomePage();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void ShowHomePage()
        {
            HomePageHandler.Activate();
            HomePageHandler.WindowState = System.Windows.WindowState.Normal;
            this.Close();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidateRequiredFieldsProvided();
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidateRequiredFieldsProvided();
        }

        private void txtAssignedAttorney_LostFocus(object sender, RoutedEventArgs e)
        {
            IsValidateRequiredFieldsProvided();
        }

        private void txtAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshClientFileList();
        }
    }
}
