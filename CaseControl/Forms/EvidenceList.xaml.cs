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
    /// Interaction logic for EvidenceList.xaml
    /// </summary>
    public partial class EvidenceList : Window
    {
        private string action = string.Empty;
        internal string FileID
        { get; set; }

        public EvidenceList()
        {
            InitializeComponent();
        }
        public void FirstMethod(Object sender, ExecutedRoutedEventArgs e)
        {
            btnSave_Click(sender, e);
        }
        private void ListAllEvidence()
        {
            try
            {
                string sqlQuery = string.Format(Constants.CLIENT_BASIC_EVIDENCE_DETAILS_QUERY, string.IsNullOrEmpty(FileID)?"-1":FileID);
                var result = DBHelper.GetSelectDataSet(sqlQuery);
                if (result == null || result.Tables.Count ==0 || result.Tables[0].Rows.Count == 0)
                    return;
                ObservableCollection<EvidenceInformation> evidenceList = new ObservableCollection<EvidenceInformation>();
                for (int index = 0; index < result.Tables[0].Rows.Count; index++)
                {
                    evidenceList.Add(new EvidenceInformation() { Evidence = result.Tables[0].Rows[index][Constants.CLIENT_EVIDENCE_COLUMN].ToString() });
                }

                dgEvidenceList.ItemsSource = evidenceList;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListAllEvidence();
        }

        private void dgEvidenceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EvidenceInformation selectedValue = dgEvidenceList.SelectedValue as EvidenceInformation;
                if (selectedValue != null)
                    txtEvidence.Text = selectedValue.Evidence;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = true;
            txtEvidence.IsReadOnly = false;
            action = "Add";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(action))
                {
                    btnSave.IsEnabled = false;
                    txtEvidence.IsReadOnly = true;
                    return;
                }
                if (action == "Add")
                {
                    query = string.Format(Constants.ADD_NEW_EVIDENCE_QUERY, FileID, txtEvidence.Text);
                }
                else if (action == "Edit")
                {
                    EvidenceInformation selectedValue = dgEvidenceList.SelectedValue as EvidenceInformation;
                    query = string.Format(Constants.EDIT_EVIDENCE_QUERY, txtEvidence.Text, FileID, selectedValue.Evidence);
                }
                DBHelper.ExecuteNonQuery(query);
                ListAllEvidence();
                action = string.Empty;
                btnSave.IsEnabled = false;
                txtEvidence.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = true;
            txtEvidence.IsReadOnly = false;
            action = "Edit";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EvidenceInformation selectedValue = dgEvidenceList.SelectedValue as EvidenceInformation;
                if (selectedValue == null)
                    return;
                string query = string.Format(Constants.DELETE_EVIDENCE_QUERY, FileID, selectedValue.Evidence);
                DBHelper.ExecuteNonQuery(query);
                ListAllEvidence();
            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }
    }

    public class EvidenceInformation
    {
        private string evidence;

        public string Evidence
        {
            get { return evidence; }
            set
            {
                evidence = value;
                NotifyPropertyChanged("Evidence");
            }
        }

        #region INotifyPropertyChanged Members

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
