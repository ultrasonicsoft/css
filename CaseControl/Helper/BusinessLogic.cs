using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;

namespace CaseControl
{
    internal class BusinessLogic
    {
        #region Select Methods

        internal static ObservableCollection<ClientFileInformation> GetAllClientNameFileID(string sqlClientFileList)
        {
            ObservableCollection<ClientFileInformation> fileList = new ObservableCollection<ClientFileInformation>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sqlClientFileList);
                if (result == null)
                    return null;
                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    fileList.Add(new ClientFileInformation()
                    {
                        FileID = result.Tables[0].Rows[rowIndex][Constants.FILE_ID].ToString(),
                        FirstName = result.Tables[0].Rows[rowIndex][Constants.CLIENT_FIRST_NAME].ToString(),
                        LastName = result.Tables[0].Rows[rowIndex][Constants.CLIENT_LAST_NAME].ToString(),
                        Status = result.Tables[0].Rows[rowIndex][Constants.CLIENT_STATUS].ToString() == "True" ? "Active" : "Inactive",
                        ClientCreatedOn = result.Tables[0].Rows[rowIndex][Constants.CLIENT_CREATED_ON].ToString(),

                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }

        internal static ObservableCollection<MiscNotes> GetAllClientMiscNotes(string sqlClientMiscList)
        {
            ObservableCollection<MiscNotes> miscNoteList = new ObservableCollection<MiscNotes>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sqlClientMiscList);
                if (result == null)
                    return null;
                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    miscNoteList.Add(new MiscNotes()
                    {
                        NoteNumber = result.Tables[0].Rows[rowIndex][Constants.CLIENT_MISC_NOTE_NUMBER_COLUMN].ToString(),
                        CreatedDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.CLIENT_MISC_CREATED_DATE_COLUMN].ToString()).ToShortDateString(),
                        ModifiedDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.CLIENT_MISC_MODIFIED_DATE_COLUMN].ToString()).ToShortDateString(),
                        Description = result.Tables[0].Rows[rowIndex][Constants.CLIENT_MISC_DESCRIPTION_COLUMN].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return miscNoteList;
        }
        internal static ObservableCollection<ClientInjury> GetAllClientInjury(string sqlClientInjuryList)
        {
            ObservableCollection<ClientInjury> injuryList = new ObservableCollection<ClientInjury>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sqlClientInjuryList);
                if (result == null)
                    return null;
                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    injuryList.Add(new ClientInjury()
                    {
                        Note = result.Tables[0].Rows[rowIndex][Constants.CLIENT_INJURY_NOTE_NUMBER_COLUMN].ToString(),
                        CreatedDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.CLIENT_INJURY_CREATED_DATE_COLUMN].ToString()).ToShortDateString(),
                        Description = result.Tables[0].Rows[rowIndex][Constants.CLIENT_INJURY_DESCRIPTION_COLUMN].ToString(),
                        LastModifiedDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.CLIENT_INJURY_MODIFIED_DATE_COLUMN].ToString()).ToShortDateString(),
                        FileID = result.Tables[0].Rows[rowIndex][Constants.FILE_ID].ToString()
                    });

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return injuryList;
        }
        #endregion

        #region Save Methods

        internal static void SaveClientGeneralInfo(ClientGeneralInformation generalInfo)
        {
            try
            {
                // Client Master
                string query = string.Format(Constants.INSERT_CLIENT_MASTER_QUERY, generalInfo.FirstName, generalInfo.LastName, "true", generalInfo.ClientCreatedOn);

                int result = DBHelper.ExecuteNonQuery(query);

                // Client File Master
                string clientID = DBHelper.GetScalarValue(Constants.NEW_CLIENT_ID_QUERY).ToString();
                string caseTypeID = DBHelper.GetScalarValue(string.Format(Constants.CASE_TYPE_ID_QUERY, generalInfo.CaseType)).ToString();
                string accDate = generalInfo.AccidentDate == DateTime.MinValue ? string.Empty : generalInfo.AccidentDate.ToString("yyyy-MM-dd");
                query = string.Format(Constants.INSERT_CLIENT_FILE_MASTER_QUERY, clientID, caseTypeID, accDate, generalInfo.CaseStatus, generalInfo.FileNo);

                result = DBHelper.ExecuteNonQuery(query);

                // Client Basic details
                string dateOfBirth = generalInfo.DateOfBirth == DateTime.MinValue ? string.Empty : generalInfo.DateOfBirth.ToString("yyyy-MM-dd");
                query = string.Format(Constants.INSERT_CLIENT_BASIC_DETAILS_QUERY, generalInfo.FileNo, generalInfo.Address,
                                   generalInfo.City, generalInfo.State, generalInfo.HomePhone, generalInfo.CellPhone, generalInfo.DrivingLicense, dateOfBirth, generalInfo.SSN
                                   , generalInfo.Email, generalInfo.WorkPhone, generalInfo.SuiteAddress);

                result = DBHelper.ExecuteNonQuery(query);

                // Client Case details
                query = string.Format(Constants.INSERT_CLIENT_CASE_INFO_QUERY, generalInfo.FileNo, generalInfo.InitialCaseInformation, generalInfo.DefendantName, generalInfo.OriginatingAttorney, generalInfo.AssignedAttorney, generalInfo.Referral);
                result = DBHelper.ExecuteNonQuery(query);

            }
            catch (Exception ex)
            {
                Helper.LogException(ex);
            }
        }

        internal static void SaveClientAdditionalInformation(ClientAdditionalInformation additionalInfo, string fileID)
        {
            try
            {
                string query = string.Format(Constants.INSERT_CLIENT_EMPLOYER_DETAILS_QUERY, fileID, additionalInfo.Occupation, additionalInfo.Employer, additionalInfo.Address, additionalInfo.City, additionalInfo.State);
                int result = DBHelper.ExecuteNonQuery(query);

                query = string.Format(Constants.INSERT_CLIENT_SPOUSE_DETAILS_QUERY, fileID, additionalInfo.SpouseInfo.FirstName, additionalInfo.SpouseInfo.LastName, additionalInfo.SpouseInfo.Occupation, additionalInfo.SpouseInfo.Employer, additionalInfo.SpouseInfo.Address, additionalInfo.SpouseInfo.City, additionalInfo.SpouseInfo.State);
                result = DBHelper.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void SaveClientAutoInformation(ClientAutoInformation autoInfo, string fileID)
        {
            string query = string.Format(Constants.INSERT_CLIENT_AUTO_DETAILS_QUERY, fileID, autoInfo.InsuranceCompany, autoInfo.Address, autoInfo.City, autoInfo.State, autoInfo.Zip, autoInfo.PhoneNumber, autoInfo.Adjuster);
            int result = DBHelper.ExecuteNonQuery(query);

            query = string.Format(Constants.INSERT_CLIENT_POLICY_DETAILS_QUERY, fileID, autoInfo.PolicyNumber, autoInfo.EffectiveStartDate.ToString(), GetNonNullIntValue(autoInfo.Amout), autoInfo.EffectiveEndDate.ToString(), GetNonNullIntValue(autoInfo.LiabilityMinimumCoverage), GetNonNullIntValue(autoInfo.LiabilityMaximumCoverage), GetNonNullIntValue(autoInfo.UMIMinimum), GetNonNullIntValue(autoInfo.UMIMaximum), autoInfo.Notes, autoInfo.MedPayAvailable, autoInfo.Reimbursable);
            result = DBHelper.ExecuteNonQuery(query);
        }

        internal static void SaveClientMedicationInformation(ClientMedicalInformation mediInfo, string fileID)
        {
            string query = string.Format(Constants.INSERT_CLIENT_MEDICAL_INFORMATION_QUERY, fileID, mediInfo.NamedInsured, mediInfo.InsuranceCompany, mediInfo.Address, mediInfo.City, mediInfo.State, mediInfo.Zip, mediInfo.PhoneNumber, mediInfo.PolicyNumber, mediInfo.MediCalNumber, mediInfo.MediCareNumber, mediInfo.ClaimNumber);
            int result = DBHelper.ExecuteNonQuery(query);
        }

        internal static void SaveClientInjury(ClientInjury injury, string fileID)
        {
            try
            {
                string query = string.Format(Constants.INSERT_CLIENT_INJURY_NOTE_QUERY, fileID, injury.Note, injury.CreatedDate.ToString(), injury.CreatedDate.ToString(), injury.Description);
                int result = DBHelper.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static void UpdateClientInjury(string query)
        {
            try
            {
                int result = DBHelper.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void SaveClientDefendantInformation(ClientDefendantInformation defInfo, string fileID)
        {
            //"INSERT INTO DefendantAttorneyDetails(FileID, Firm, Attorney, Address, City, State, Zip, PhoneNumber) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            string query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_DETAILS_QUERY, fileID, defInfo.LastName, defInfo.FirstName, defInfo.Address, defInfo.City, defInfo.State,
                defInfo.Zip, defInfo.HomePhone, defInfo.BusinessPhone, defInfo.DateOfBirth, defInfo.DrivingLicense);
            int result = DBHelper.ExecuteNonQuery(query);

            query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY, fileID, defInfo.AttorneyInfo.Firm, defInfo.AttorneyInfo.Attorney, defInfo.AttorneyInfo.Address,
                defInfo.AttorneyInfo.City, defInfo.AttorneyInfo.State, defInfo.AttorneyInfo.Zip, defInfo.AttorneyInfo.Phone);
            result = DBHelper.ExecuteNonQuery(query);
        }
        #endregion

        internal static void SaveDefendantInsuranceInformation(DefendantInsuranceDetails defInsurance, string fileID)
        {
            string query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY, fileID, defInsurance.NameOfInsured, defInsurance.InsuranceCompany, defInsurance.Address, defInsurance.City, defInsurance.State, defInsurance.Zip, defInsurance.Phone, defInsurance.Adjuster, defInsurance.ClaimNumber, defInsurance.PolicyLimits);
            int result = DBHelper.ExecuteNonQuery(query);
        }

        internal static void SaveStatuteInformation(StatuteInformation statuteInfo, string fileID)
        {
            string query = string.Format(Constants.INSERT_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY, fileID, statuteInfo.AccidentDate, statuteInfo.AccDateAfter1yr, statuteInfo.AccDateAfter2yr,
                statuteInfo.ComplaintFileDate, statuteInfo.ComplaintAfter60days, statuteInfo.ComplaintAfter2yrs, statuteInfo.ComplaintAfter3yrs, statuteInfo.ComplaintAfter5yrs, statuteInfo.IsGovtDClaim);
            int result = DBHelper.ExecuteNonQuery(query);

            if (statuteInfo.CityClaim != null)
            {
                query = string.Format(Constants.INSERT_GOVT_CITY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CityClaim.DeniedDate, statuteInfo.CityClaim.ClaimDueDate,
                    statuteInfo.CityClaim.FiledDate, statuteInfo.CityClaim.FiledDateAfter60Days, statuteInfo.CityClaim.FiledDateAfter2yrs, statuteInfo.CityClaim.FiledDateAfter3yrs,
                    statuteInfo.CityClaim.FiledDateAfter5yrs);
                result = DBHelper.ExecuteNonQuery(query);
            }
            if (statuteInfo.StateClaim != null)
            {
                query = string.Format(Constants.INSERT_GOVT_STATE_CLAIM_DETAILS_QUERY, fileID, statuteInfo.StateClaim.DeniedDate, statuteInfo.StateClaim.ClaimDueDate,
                    statuteInfo.StateClaim.FiledDate, statuteInfo.StateClaim.FiledDateAfter60Days, statuteInfo.StateClaim.FiledDateAfter2yrs, statuteInfo.StateClaim.FiledDateAfter3yrs,
                    statuteInfo.StateClaim.FiledDateAfter5yrs);
                result = DBHelper.ExecuteNonQuery(query);
            }
            if (statuteInfo.CountyClaim != null)
            {
                query = string.Format(Constants.INSERT_GOVT_COUNTY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CountyClaim.DeniedDate, statuteInfo.CountyClaim.ClaimDueDate,
                    statuteInfo.CountyClaim.FiledDate, statuteInfo.CountyClaim.FiledDateAfter60Days, statuteInfo.CountyClaim.FiledDateAfter2yrs, statuteInfo.CountyClaim.FiledDateAfter3yrs,
                    statuteInfo.CountyClaim.FiledDateAfter5yrs);
                result = DBHelper.ExecuteNonQuery(query);
            }
            if (statuteInfo.OtherClaim != null)
            {
                query = string.Format(Constants.INSERT_GOVT_OTHER_CLAIM_DETAILS_QUERY, fileID, statuteInfo.OtherClaim.DeniedDate, statuteInfo.OtherClaim.ClaimDueDate,
                    statuteInfo.OtherClaim.FiledDate, statuteInfo.OtherClaim.FiledDateAfter60Days, statuteInfo.OtherClaim.FiledDateAfter2yrs, statuteInfo.OtherClaim.FiledDateAfter3yrs,
                    statuteInfo.OtherClaim.FiledDateAfter5yrs);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }

        internal static void SaveCourtInformation(CourtInformation courtInfo, string fileID)
        {
            string query = string.Format(Constants.INSERT_COURT_INFORMATION_QUERY, fileID, courtInfo.CaseNumber, courtInfo.Court, courtInfo.Address, courtInfo.City, courtInfo.State, courtInfo.Zip);
            int result = DBHelper.ExecuteNonQuery(query);
        }

        internal static void SaveMiscNotes(MiscNotes miscNote, string fileID)
        {
            string query = string.Format(Constants.INSERT_MISC_NOTES_QUERY, fileID, miscNote.NoteNumber, miscNote.CreatedDate, miscNote.ModifiedDate, miscNote.Description);
            int result = DBHelper.ExecuteNonQuery(query);
        }

        internal static bool CloseClient(string fileID)
        {
            string query = string.Format(Constants.UPDATE_CLIENT_STATUS_QUERY, "false", fileID);
            int result = DBHelper.ExecuteNonQuery(query);
            return result != 0;
        }

        internal static bool ReactivateClient(string fileID)
        {
            string query = string.Format(Constants.UPDATE_CLIENT_STATUS_QUERY, "true", fileID);
            int result = DBHelper.ExecuteNonQuery(query);
            return result != 0;
        }

        #region Helper Methods

        private static string GetNonNullIntValue(string value)
        {
            return string.IsNullOrEmpty(value) ? "0" : value;
        }
        #endregion

        internal static void UpdateClientGeneralInfo(ClientGeneralInformation generalInfo, ClientGeneralInformation oldGeneralInfo)
        {
            try
            {
                // Client Master
                string query = string.Format(Constants.UPDATE_CLIENT_MASTER_QUERY, generalInfo.FirstName, generalInfo.LastName, "true", generalInfo.ClientCreatedOn, oldGeneralInfo.FileNo);
                int result = DBHelper.ExecuteNonQuery(query);

                // Client File Master
                string caseTypeID = DBHelper.GetScalarValue(string.Format(Constants.CASE_TYPE_ID_QUERY, generalInfo.CaseType)).ToString();
                string oldCaseTypeID = DBHelper.GetScalarValue(string.Format(Constants.CASE_TYPE_ID_QUERY, oldGeneralInfo.CaseType)).ToString();
                query = string.Format(Constants.UPDATE_CLIENT_FILE_MASTER_QUERY, caseTypeID, generalInfo.AccidentDate.ToString(), generalInfo.CaseStatus, oldCaseTypeID, oldGeneralInfo.AccidentDate, oldGeneralInfo.CaseStatus, oldGeneralInfo.FileNo);
                result = DBHelper.ExecuteNonQuery(query);

                // Client Basic details
                query = string.Format(Constants.UPDATE_CLIENT_BASIC_DETAILS_QUERY, generalInfo.Address, generalInfo.City, generalInfo.State, generalInfo.HomePhone, generalInfo.CellPhone, generalInfo.DrivingLicense, generalInfo.DateOfBirth.ToString(), generalInfo.SSN,
                       oldGeneralInfo.Address, oldGeneralInfo.City, oldGeneralInfo.State, oldGeneralInfo.HomePhone, oldGeneralInfo.CellPhone, oldGeneralInfo.DrivingLicense, oldGeneralInfo.DateOfBirth,
                       oldGeneralInfo.SSN, oldGeneralInfo.FileNo, generalInfo.Email, generalInfo.SuiteAddress, generalInfo.WorkPhone);
                result = DBHelper.ExecuteNonQuery(query);

                // Client Basic details
                query = string.Format(Constants.UPDATE_CLIENT_CASE_INFO_QUERY, generalInfo.InitialCaseInformation, generalInfo.DefendantName, generalInfo.OriginatingAttorney, generalInfo.AssignedAttorney, generalInfo.Referral,
                    oldGeneralInfo.InitialCaseInformation, oldGeneralInfo.DefendantName, oldGeneralInfo.OriginatingAttorney, oldGeneralInfo.AssignedAttorney, oldGeneralInfo.Referral, oldGeneralInfo.FileNo);
                result = DBHelper.ExecuteNonQuery(query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static void UpdateClientAdditionalInformation(ClientAdditionalInformation additionalInfo, string fileID)
        {
            try
            {
                string query = string.Format(Constants.EXISTS_CLIENT_EMPLOYER_DETAILS_QUERY, fileID);
                object isPresent = DBHelper.GetScalarValue(query);
                int result = 0;
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_CLIENT_EMPLOYER_DETAILS_QUERY, fileID, additionalInfo.Occupation, additionalInfo.Employer, additionalInfo.Address, additionalInfo.City, additionalInfo.State);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_CLIENT_EMPLOYER_DETAILS_QUERY, fileID, additionalInfo.Occupation, additionalInfo.Employer, additionalInfo.Address, additionalInfo.City, additionalInfo.State);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                query = string.Format(Constants.EXISTS_CLIENT_SPOUSE_DETAILS_QUERY, fileID);
                isPresent = DBHelper.GetScalarValue(query);
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_CLIENT_SPOUSE_DETAILS_QUERY, fileID, additionalInfo.SpouseInfo.FirstName, additionalInfo.SpouseInfo.LastName, additionalInfo.SpouseInfo.Occupation, additionalInfo.SpouseInfo.Employer, additionalInfo.SpouseInfo.Address, additionalInfo.SpouseInfo.City, additionalInfo.SpouseInfo.State);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_CLIENT_SPOUSE_DETAILS_QUERY, fileID, additionalInfo.SpouseInfo.FirstName, additionalInfo.SpouseInfo.LastName, additionalInfo.SpouseInfo.Occupation, additionalInfo.SpouseInfo.Employer, additionalInfo.SpouseInfo.Address, additionalInfo.SpouseInfo.City, additionalInfo.SpouseInfo.State);
                    result = DBHelper.ExecuteNonQuery(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static void UpdateClientAutoInformation(ClientAutoInformation autoInfo, string fileID)
        {
            string query = string.Format(Constants.EXISTS_CLIENT_AUTO_DETAILS_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_AUTO_DETAILS_QUERY, fileID, autoInfo.InsuranceCompany, autoInfo.Address, autoInfo.City, autoInfo.State, autoInfo.Zip, autoInfo.PhoneNumber, autoInfo.Adjuster);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_AUTO_DETAILS_QUERY, fileID, autoInfo.InsuranceCompany, autoInfo.Address, autoInfo.City, autoInfo.State, autoInfo.Zip, autoInfo.PhoneNumber, autoInfo.Adjuster);
                result = DBHelper.ExecuteNonQuery(query);
            }
            query = string.Format(Constants.EXISTS_CLIENT_POLICY_DETAILS_QUERY, fileID);
            isPresent = DBHelper.GetScalarValue(query);
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_POLICY_DETAILS_QUERY, fileID, autoInfo.PolicyNumber, autoInfo.EffectiveStartDate.ToString(), GetNonNullIntValue(autoInfo.Amout), autoInfo.EffectiveEndDate.ToString(), GetNonNullIntValue(autoInfo.LiabilityMinimumCoverage), GetNonNullIntValue(autoInfo.LiabilityMaximumCoverage), GetNonNullIntValue(autoInfo.UMIMinimum), GetNonNullIntValue(autoInfo.UMIMaximum), autoInfo.Notes, autoInfo.MedPayAvailable, autoInfo.Reimbursable);
                result = DBHelper.ExecuteNonQuery(query);
            }
            {
                query = string.Format(Constants.UPDATE_CLIENT_POLICY_DETAILS_QUERY, fileID, autoInfo.PolicyNumber, autoInfo.EffectiveStartDate.ToString(), GetNonNullIntValue(autoInfo.Amout), autoInfo.EffectiveEndDate.ToString(), GetNonNullIntValue(autoInfo.LiabilityMinimumCoverage), GetNonNullIntValue(autoInfo.LiabilityMaximumCoverage), GetNonNullIntValue(autoInfo.UMIMinimum), GetNonNullIntValue(autoInfo.UMIMaximum), autoInfo.Notes, autoInfo.MedPayAvailable, autoInfo.Reimbursable);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }
        internal static void UpdateClientMedicationInformation(ClientMedicalInformation mediInfo, string fileID)
        {
            string query = string.Format(Constants.EXISTS_CLIENT_MEDICAL_INFORMATION_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_MEDICAL_INFORMATION_QUERY, fileID, mediInfo.NamedInsured, mediInfo.InsuranceCompany, mediInfo.Address, mediInfo.City, mediInfo.State, mediInfo.Zip, mediInfo.PhoneNumber, mediInfo.PolicyNumber, mediInfo.MediCalNumber, mediInfo.MediCareNumber, mediInfo.ClaimNumber);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_MEDICAL_INFORMATION_QUERY, fileID, mediInfo.NamedInsured, mediInfo.InsuranceCompany, mediInfo.Address, mediInfo.City, mediInfo.State, mediInfo.Zip, mediInfo.PhoneNumber, mediInfo.PolicyNumber, mediInfo.MediCalNumber,
                    mediInfo.MediCareNumber, mediInfo.ClaimNumber);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }

        internal static void UpdateClientDefendantInformation(ClientDefendantInformation defInfo, string fileID)
        {
            string query = string.Format(Constants.EXISTS_CLIENT_DEFENDANT_DETAILS_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_DETAILS_QUERY, fileID, defInfo.LastName, defInfo.FirstName, defInfo.Address, defInfo.City, defInfo.State,
                   defInfo.Zip, defInfo.HomePhone, defInfo.BusinessPhone, defInfo.DateOfBirth, defInfo.DrivingLicense);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_DEFENDANT_DETAILS_QUERY, fileID, defInfo.LastName, defInfo.FirstName, defInfo.Address, defInfo.City, defInfo.State,
                    defInfo.Zip, defInfo.HomePhone, defInfo.BusinessPhone, defInfo.DateOfBirth, defInfo.DrivingLicense);
                result = DBHelper.ExecuteNonQuery(query);
            }
            query = string.Format(Constants.EXISTS_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY, fileID);
            isPresent = DBHelper.GetScalarValue(query);
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY, fileID, defInfo.AttorneyInfo.Firm, defInfo.AttorneyInfo.Attorney, defInfo.AttorneyInfo.Address,
                    defInfo.AttorneyInfo.City, defInfo.AttorneyInfo.State, defInfo.AttorneyInfo.Zip, defInfo.AttorneyInfo.Phone);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY, fileID, defInfo.AttorneyInfo.Firm, defInfo.AttorneyInfo.Attorney, defInfo.AttorneyInfo.Address,
                    defInfo.AttorneyInfo.City, defInfo.AttorneyInfo.State, defInfo.AttorneyInfo.Zip, defInfo.AttorneyInfo.Phone);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }

        internal static void UpdateDefendantInsuranceInformation(DefendantInsuranceDetails defInsurance, string fileID)
        {
            string query = string.Format(Constants.EXISTS_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY, fileID, defInsurance.NameOfInsured, defInsurance.InsuranceCompany, defInsurance.Address, defInsurance.City, defInsurance.State, defInsurance.Zip, defInsurance.Phone, defInsurance.Adjuster, defInsurance.ClaimNumber, defInsurance.PolicyLimits);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY, fileID, defInsurance.NameOfInsured, defInsurance.InsuranceCompany, defInsurance.Address, defInsurance.City, defInsurance.State, defInsurance.Zip, defInsurance.Phone, defInsurance.Adjuster, defInsurance.ClaimNumber, defInsurance.PolicyLimits);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }
        internal static void UpdateCourtInformation(CourtInformation courtInfo, string fileID)
        {
            string query = string.Format(Constants.EXISTS_COURT_INFORMATION_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_COURT_INFORMATION_QUERY, fileID, courtInfo.CaseNumber, courtInfo.Court, courtInfo.Address, courtInfo.City, courtInfo.State, courtInfo.Zip);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_COURT_INFORMATION_QUERY, fileID, courtInfo.CaseNumber, courtInfo.Court, courtInfo.Address, courtInfo.City, courtInfo.State, courtInfo.Zip);
                result = DBHelper.ExecuteNonQuery(query);
            }
        }
        internal static void UpdateStatuteInformation(StatuteInformation statuteInfo, string fileID)
        {
            string query = string.Format(Constants.EXISTS_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            int result = 0;
            if (isPresent == null)
            {
                query = string.Format(Constants.INSERT_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY, fileID, statuteInfo.AccidentDate, statuteInfo.AccDateAfter1yr, statuteInfo.AccDateAfter2yr,
                statuteInfo.ComplaintFileDate, statuteInfo.ComplaintAfter60days, statuteInfo.ComplaintAfter2yrs, statuteInfo.ComplaintAfter3yrs, statuteInfo.ComplaintAfter5yrs, statuteInfo.IsGovtDClaim);
                result = DBHelper.ExecuteNonQuery(query);
            }
            else
            {
                query = string.Format(Constants.UPDATE_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY, fileID, statuteInfo.AccidentDate, statuteInfo.AccDateAfter1yr, statuteInfo.AccDateAfter2yr,
                statuteInfo.ComplaintFileDate, statuteInfo.ComplaintAfter60days, statuteInfo.ComplaintAfter2yrs, statuteInfo.ComplaintAfter3yrs, statuteInfo.ComplaintAfter5yrs, statuteInfo.IsGovtDClaim);
                result = DBHelper.ExecuteNonQuery(query);
            }

            if (statuteInfo.CityClaim != null)
            {
                query = string.Format(Constants.EXISTS_GOVT_CITY_CLAIM_DETAILS_QUERY, fileID);
                isPresent = DBHelper.GetScalarValue(query);
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_GOVT_CITY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CityClaim.DeniedDate, statuteInfo.CityClaim.ClaimDueDate,
                               statuteInfo.CityClaim.FiledDate, statuteInfo.CityClaim.FiledDateAfter60Days, statuteInfo.CityClaim.FiledDateAfter2yrs, statuteInfo.CityClaim.FiledDateAfter3yrs,
                               statuteInfo.CityClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_GOVT_CITY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CityClaim.DeniedDate, statuteInfo.CityClaim.ClaimDueDate,
                                statuteInfo.CityClaim.FiledDate, statuteInfo.CityClaim.FiledDateAfter60Days, statuteInfo.CityClaim.FiledDateAfter2yrs, statuteInfo.CityClaim.FiledDateAfter3yrs,
                                statuteInfo.CityClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }

            }
            if (statuteInfo.StateClaim != null)
            {
                query = string.Format(Constants.EXISTS_GOVT_STATE_CLAIM_DETAILS_QUERY, fileID);
                isPresent = DBHelper.GetScalarValue(query);
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_GOVT_STATE_CLAIM_DETAILS_QUERY, fileID, statuteInfo.StateClaim.DeniedDate, statuteInfo.StateClaim.ClaimDueDate,
                    statuteInfo.StateClaim.FiledDate, statuteInfo.StateClaim.FiledDateAfter60Days, statuteInfo.StateClaim.FiledDateAfter2yrs, statuteInfo.StateClaim.FiledDateAfter3yrs,
                    statuteInfo.StateClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_GOVT_STATE_CLAIM_DETAILS_QUERY, fileID, statuteInfo.StateClaim.DeniedDate, statuteInfo.StateClaim.ClaimDueDate,
                  statuteInfo.StateClaim.FiledDate, statuteInfo.StateClaim.FiledDateAfter60Days, statuteInfo.StateClaim.FiledDateAfter2yrs, statuteInfo.StateClaim.FiledDateAfter3yrs,
                  statuteInfo.StateClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }

            }
            if (statuteInfo.CountyClaim != null)
            {
                query = string.Format(Constants.EXISTS_GOVT_COUNTY_CLAIM_DETAILS_QUERY, fileID);
                isPresent = DBHelper.GetScalarValue(query);
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_GOVT_COUNTY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CountyClaim.DeniedDate, statuteInfo.CountyClaim.ClaimDueDate,
                    statuteInfo.CountyClaim.FiledDate, statuteInfo.CountyClaim.FiledDateAfter60Days, statuteInfo.CountyClaim.FiledDateAfter2yrs, statuteInfo.CountyClaim.FiledDateAfter3yrs,
                    statuteInfo.CountyClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_GOVT_COUNTY_CLAIM_DETAILS_QUERY, fileID, statuteInfo.CountyClaim.DeniedDate, statuteInfo.CountyClaim.ClaimDueDate,
                    statuteInfo.CountyClaim.FiledDate, statuteInfo.CountyClaim.FiledDateAfter60Days, statuteInfo.CountyClaim.FiledDateAfter2yrs, statuteInfo.CountyClaim.FiledDateAfter3yrs,
                    statuteInfo.CountyClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }

            }
            if (statuteInfo.OtherClaim != null)
            {
                query = string.Format(Constants.EXISTS_GOVT_OTHER_CLAIM_DETAILS_QUERY, fileID);
                isPresent = DBHelper.GetScalarValue(query);
                if (isPresent == null)
                {
                    query = string.Format(Constants.INSERT_GOVT_OTHER_CLAIM_DETAILS_QUERY, fileID, statuteInfo.OtherClaim.DeniedDate, statuteInfo.OtherClaim.ClaimDueDate,
                   statuteInfo.OtherClaim.FiledDate, statuteInfo.OtherClaim.FiledDateAfter60Days, statuteInfo.OtherClaim.FiledDateAfter2yrs, statuteInfo.OtherClaim.FiledDateAfter3yrs,
                   statuteInfo.OtherClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }
                else
                {
                    query = string.Format(Constants.UPDATE_GOVT_OTHER_CLAIM_DETAILS_QUERY, fileID, statuteInfo.OtherClaim.DeniedDate, statuteInfo.OtherClaim.ClaimDueDate,
                   statuteInfo.OtherClaim.FiledDate, statuteInfo.OtherClaim.FiledDateAfter60Days, statuteInfo.OtherClaim.FiledDateAfter2yrs, statuteInfo.OtherClaim.FiledDateAfter3yrs,
                   statuteInfo.OtherClaim.FiledDateAfter5yrs);
                    result = DBHelper.ExecuteNonQuery(query);
                }

            }
        }

        internal static bool IsFileNoAlreadyPresent(string fileID)
        {
            string query = string.Format(Constants.EXISTS_FILE_ID_QUERY, fileID);
            object isPresent = DBHelper.GetScalarValue(query);
            return isPresent != null;
        }

        internal static int GetNewFileID()
        {
            int newFileID = 0;
            object value = DBHelper.GetScalarValue(Constants.NEW_FILE_ID_QUERY);
            if (value != null)
            {
                newFileID = (int)value;
            }
            return newFileID + 1;
        }

        internal static bool IsValidUser(string userName, string password)
        {
            string query = string.Format(Constants.VALIDATE_USER_QUERY, userName, password);
            object isPresent = DBHelper.GetScalarValue(query);
            return isPresent != null;
        }

        internal static bool DeleteClient(string fileID)
        {
            string query = string.Format(Constants.GET_CLIENTID_FROM_FILE_ID_QUERY, fileID);
            object clientID = DBHelper.GetScalarValue(query);
            int result = 0;
            if (clientID != null)
            {
                //TODO: delete data from all tables using FileID
                DataSet allFileIDs = DBHelper.GetSelectDataSet(string.Format(Constants.GET_FILEID__QUERY, clientID));
                for (int index = 0; index < allFileIDs.Tables[0].Rows.Count; index++)
                {
                    query = string.Format(Constants.DELETE_CASE_INFORMATION_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_AUTO_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_BASIC_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_COURT_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_EMPLOYER_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_EVIDENCES_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_GOVT_CLAIM_CITY_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_GOVT_CLAIM_STATE_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_GOVT_CLAIM_COUNTY_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_GOVT_CLAIM_OTHER_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CASE_INJURIES_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_MEDICAL_INSURANCE_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_MISC_NOTE_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_POLICY_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_CLIENT_SPOUSE_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_DEFENDANT_ATTORNEY_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_DEFENDANT_INSURANCE_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);

                    query = string.Format(Constants.DELETE_STATUTE_DETAILS_QUERY, allFileIDs.Tables[0].Rows[index][0].ToString());
                    result = DBHelper.ExecuteNonQuery(query);
                }
                query = string.Format(Constants.DELETE_CLIENT_FROM_CLIENTFILEMASTER_QUERY, clientID.ToString());
                result = DBHelper.ExecuteNonQuery(query);

                query = string.Format(Constants.DELETE_CLIENT_CLIENTMASTER_QUERY, clientID.ToString());
                result = DBHelper.ExecuteNonQuery(query);
            }
            return result != 0;
        }

        internal static ObservableCollection<TransactionDetail> GetAllTransactionDetails(string sql)
        {
            ObservableCollection<TransactionDetail> fileList = new ObservableCollection<TransactionDetail>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sql);
                if (result == null)
                    return null;
                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    TransactionDetail newTransaction = new TransactionDetail()
                    {
                        TransactionID = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_ID].ToString(),
                        TransactionDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_DATE].ToString()).ToShortDateString(),
                        Description = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_DESCRIPTION].ToString(),
                        BillingType = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_BILLING_TYPE].ToString(),
                        GeneralFund = float.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_GENERAL_FUND].ToString()).ToString("0.00"),
                        //GeneralFund = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_GENERAL_FUND].ToString(),
                        //TrustFund = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_TRUST_FUND].ToString(),
                        TrustFund = float.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_TRUST_FUND].ToString()).ToString("0.00"),
                        CheckNo = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_CHECK_NO].ToString(),
                    };
                    newTransaction.GeneralFund = newTransaction.GeneralFund;
                    newTransaction.TrustFund = newTransaction.TrustFund;
                    fileList.Add(newTransaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }

        internal static ObservableCollection<BillingReportViewModel> GetAllBillingTransactionDetails(string sql)
        {
            ObservableCollection<BillingReportViewModel> reportItems = new ObservableCollection<BillingReportViewModel>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sql);
                if (result == null)
                    return null;
                float genAccountGrandTotal = 0;
                float trustAccountGrandTotal = 0;
                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    BillingReportViewModel newTransaction = new BillingReportViewModel()
                    {
                        ItemNo = (rowIndex + 1).ToString(),
                        //ItemNo = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_ID].ToString(),
                        Date = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_DATE].ToString()).ToShortDateString(),
                        Description = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_DESCRIPTION].ToString(),
                        BillingType = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_BILLING_TYPE].ToString(),
                        GeneralAccountFunds = float.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_GENERAL_FUND].ToString()).ToString("0.00"),
                        //GeneralAccountFunds = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_GENERAL_FUND].ToString(),
                        TrustAccountFunds = float.Parse(result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_TRUST_FUND].ToString()).ToString("0.00"),
                        //TrustAccountFunds = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_TRUST_FUND].ToString(),
                        CheckNo = result.Tables[0].Rows[rowIndex][Constants.TRANSACTION_CHECK_NO].ToString(),
                    };
                    genAccountGrandTotal += float.Parse(newTransaction.GeneralAccountFunds);
                    trustAccountGrandTotal += float.Parse(newTransaction.TrustAccountFunds);
                    reportItems.Add(newTransaction);
                }

                BillingReportViewModel emptyRow = new BillingReportViewModel();
                reportItems.Add(emptyRow);

                emptyRow = new BillingReportViewModel();
                emptyRow.BillingType = "-----------------";
                emptyRow.GeneralAccountFunds = "-----------------";
                emptyRow.TrustAccountFunds = "-----------------";
                reportItems.Add(emptyRow);

                BillingReportViewModel footer = new BillingReportViewModel();
                footer.BillingType = "Grand Total:";
                footer.GeneralAccountFunds = genAccountGrandTotal.ToString("0.00");
                footer.TrustAccountFunds = trustAccountGrandTotal.ToString("0.00");
                reportItems.Add(footer);

                emptyRow = new BillingReportViewModel();
                emptyRow.BillingType = "-----------------";
                emptyRow.GeneralAccountFunds = "-----------------";
                emptyRow.TrustAccountFunds = "-----------------";
                reportItems.Add(emptyRow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportItems;
        }

        internal static ObservableCollection<AllClientBillingReportViewModel> GetAllClientBillingTransactionDetails(string sql)
        {
            ObservableCollection<AllClientBillingReportViewModel> reportItems = new ObservableCollection<AllClientBillingReportViewModel>();
            try
            {
                float genAccountGrandTotal = 0;
                float trustAccountGrandTotal = 0;

                var allClientDetails = DBHelper.GetSelectDataSet(sql);
                if (allClientDetails == null || allClientDetails.Tables.Count == 0 || allClientDetails.Tables[0].Rows.Count == 0)
                    return null;

                var allTransactionTotal = DBHelper.GetSelectDataSet(Constants.ALL_CLIENTS_TRANSACTION_TOTAL_QUERY);
                if (allTransactionTotal == null || allTransactionTotal.Tables.Count == 0 || allTransactionTotal.Tables[0].Rows.Count == 0)
                    return null;

                for (int rowIndex = 0; rowIndex < allClientDetails.Tables[0].Rows.Count; rowIndex++)
                {
                    AllClientBillingReportViewModel newTransaction = new AllClientBillingReportViewModel()
                    {
                        Name = allClientDetails.Tables[0].Rows[rowIndex][Constants.NAME].ToString(),
                        FileNo = allClientDetails.Tables[0].Rows[rowIndex][Constants.FILEID].ToString(),
                        GenAccountTotal = "0.00",
                        TrustAccountTotal = "0.00",
                        AssignedAttorney = allClientDetails.Tables[0].Rows[rowIndex][Constants.ASSIGNEDATTORNY].ToString()
                    };
                    reportItems.Add(newTransaction);
                }

                string tempFileNo = string.Empty;
                AllClientBillingReportViewModel tempItem = null;
                for (int rowIndex = 0; rowIndex < allTransactionTotal.Tables[0].Rows.Count; rowIndex++)
                {
                    tempFileNo = allTransactionTotal.Tables[0].Rows[rowIndex][Constants.FILEID].ToString();
                    tempItem = reportItems.FirstOrDefault(item => item.FileNo == tempFileNo);
                    if (tempItem != null)
                    {
                        tempItem.GenAccountTotal = float.Parse(allTransactionTotal.Tables[0].Rows[rowIndex][Constants.GENTOTAL].ToString()).ToString("0.00");
                        genAccountGrandTotal += float.Parse(allTransactionTotal.Tables[0].Rows[rowIndex][Constants.GENTOTAL].ToString());
                        tempItem.TrustAccountTotal = float.Parse(allTransactionTotal.Tables[0].Rows[rowIndex][Constants.TRUSTTOTAL].ToString()).ToString("0.00");
                        trustAccountGrandTotal += float.Parse(allTransactionTotal.Tables[0].Rows[rowIndex][Constants.TRUSTTOTAL].ToString());
                    }
                }

                AllClientBillingReportViewModel emptyRow = new AllClientBillingReportViewModel();
                reportItems.Add(emptyRow);

                emptyRow = new AllClientBillingReportViewModel();
                emptyRow.FileNo = "-----------------";
                emptyRow.GenAccountTotal = "-----------------";
                emptyRow.TrustAccountTotal = "-----------------";
                reportItems.Add(emptyRow);

                AllClientBillingReportViewModel footer = new AllClientBillingReportViewModel();
                footer.FileNo = "Grand Total:";
                footer.GenAccountTotal = genAccountGrandTotal.ToString("0.00");
                footer.TrustAccountTotal = trustAccountGrandTotal.ToString("0.00");
                reportItems.Add(footer);

                emptyRow = new AllClientBillingReportViewModel();
                emptyRow.FileNo = "-----------------";
                emptyRow.GenAccountTotal = "-----------------";
                emptyRow.TrustAccountTotal = "-----------------";
                reportItems.Add(emptyRow);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportItems;
        }

        internal static int GetNewTransactionID()
        {
            int newID = 0;
            object isPresent = DBHelper.GetScalarValue(Constants.NEW_TRANSACTION_ID_QUERY) as object;
            if (isPresent != null && isPresent != DBNull.Value)
            {
                newID = int.Parse(isPresent.ToString());
            }
            return newID;
        }

        internal static bool SaveNewTransaction(TransactionDetail newTransaction)
        {
            string query = string.Format(Constants.INSERT_NEW_TRANSACTION_QUERY, newTransaction.FileID, newTransaction.TransactionID, newTransaction.
                                    TransactionDate, newTransaction.Description, newTransaction.BillingType, GetNonNullIntValue(newTransaction.GeneralFund), GetNonNullIntValue(newTransaction.TrustFund), newTransaction.CheckNo);
            int rowsAffected = DBHelper.ExecuteNonQuery(query);
            return rowsAffected != 0;
        }

        internal static bool UpdateTransaction(TransactionDetail newTransaction)
        {
            string query = string.Format(Constants.EDIT_TRANSACTION_QUERY, newTransaction.TransactionDate, newTransaction.Description, newTransaction.BillingType,
                        GetNonNullIntValue(newTransaction.GeneralFund), GetNonNullIntValue(newTransaction.TrustFund), newTransaction.CheckNo, newTransaction.FileID, newTransaction.TransactionID);
            int rowsAffected = DBHelper.ExecuteNonQuery(query);
            return rowsAffected != 0;
        }

        internal static ObservableCollection<CommonReportViewModel> GetClientReportData(string sql, AssignedBy criteria)
        {
            ObservableCollection<CommonReportViewModel> reportItems = new ObservableCollection<CommonReportViewModel>();
            try
            {
                var result = DBHelper.GetSelectDataSet(sql);
                if (result == null)
                    return null;

                string byAttorneyColumn = string.Empty;

                switch (criteria)
                {
                    case AssignedBy.AssignedAttorney:
                        byAttorneyColumn = Constants.CLIENT_ASSIGNEDATTORNY_COLUMN;
                        break;
                    case AssignedBy.OriginatingAttorney:
                        byAttorneyColumn = Constants.CLIENT_ORIGINATINGATTORNY_COLUMN;
                        break;
                    case AssignedBy.Referral:
                        byAttorneyColumn = Constants.CLIENT_REFERRAL_COLUMN;
                        break;
                }

                for (int rowIndex = 0; rowIndex < result.Tables[0].Rows.Count; rowIndex++)
                {
                    //CommonReportViewModel newTransaction = new CommonReportViewModel()
                    //{
                    //    ClientName = result.Tables[0].Rows[rowIndex][Constants.CLIENT_NAME].ToString(),
                    //    FileNo = result.Tables[0].Rows[rowIndex][Constants.FILE_ID].ToString(),
                    //    ByAttorney = result.Tables[0].Rows[rowIndex][byAttorneyColumn].ToString(),
                    //    AccidentDate = DateTime.Parse(result.Tables[0].Rows[rowIndex][Constants.ACCIDENT_DATE_COLUMN].ToString()).ToShortDateString(),
                    //    CaseNo = result.Tables[0].Rows[rowIndex][Constants.CLIENT_COURT_CASE_NUMBER_COLUMN].ToString()
                    //};
                    CommonReportViewModel newTransaction = new CommonReportViewModel();

                    newTransaction.ClientName = result.Tables[0].Rows[rowIndex][Constants.CLIENT_NAME].ToString();
                    newTransaction.FileNo = result.Tables[0].Rows[rowIndex][Constants.FILE_ID].ToString();
                    newTransaction.ByAttorney = result.Tables[0].Rows[rowIndex][byAttorneyColumn].ToString();
                    string date = result.Tables[0].Rows[rowIndex][Constants.ACCIDENT_DATE_COLUMN].ToString();
                    if (false == string.IsNullOrEmpty(date))
                    {
                        newTransaction.AccidentDate = DateTime.Parse(date).ToShortDateString();
                    }
                    newTransaction.CaseNo = result.Tables[0].Rows[rowIndex][Constants.CLIENT_COURT_CASE_NUMBER_COLUMN].ToString();

                    reportItems.Add(newTransaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportItems;
        }

        internal static ObservableCollection<StatuteReportViewModel> GetStatuteReportData(string sql)
        {
            ObservableCollection<StatuteReportViewModel> reportItems = new ObservableCollection<StatuteReportViewModel>();
            try
            {
                var allClientDetails = DBHelper.GetSelectDataSet(sql);
                if (allClientDetails == null || allClientDetails.Tables.Count == 0 || allClientDetails.Tables[0].Rows.Count == 0)
                    return null;
                for (int rowIndex = 0; rowIndex < allClientDetails.Tables[0].Rows.Count; rowIndex++)
                {
                    StatuteReportViewModel newTransaction = new StatuteReportViewModel()
                    {
                        Name = allClientDetails.Tables[0].Rows[rowIndex][Constants.NAME].ToString(),
                        FileNo = allClientDetails.Tables[0].Rows[rowIndex][Constants.FILE_ID].ToString(),
                        Date = allClientDetails.Tables[0].Rows[rowIndex][Constants.DATE].ToString(),
                        Attorney = allClientDetails.Tables[0].Rows[rowIndex][Constants.ASSIGNEDATTORNY].ToString(),

                    };
                    reportItems.Add(newTransaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reportItems;
        }
    }

    internal enum AssignedBy
    {
        AssignedAttorney,
        OriginatingAttorney,
        Referral,
        All
    }
}
