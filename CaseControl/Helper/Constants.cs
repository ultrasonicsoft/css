using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class Constants
    {
        internal const string SPLASH_SCREEN_FILE = "SplashScreen.png";
        internal const string DATABASE_CONFIG = "dbsetting.cnf";
        internal const string DATABASE_FILE_NAME = "CaseControl_Database.sdf";
        internal const string DATABASE_FOLDER_NAME = "Database";
        internal const string CONNECTION_STRING = "Data Source={0};Max Database Size=4000;Persist Security Info=False;";
        internal const string MULTI_LINE_SEPARATOR = "   --  ";

        internal const string ACTIVE_CLIENT_NAME_FILE_ID_QUERY = "SELECT cm.FirstName, cm.LastName, cm.IsActive,cm.ClientCreatedOn, cfm.FileID FROM ClientMaster AS cm INNER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID WHERE cm.IsActive='true' order by cm.LastName desc";
        internal const string INACTIVE_CLIENT_NAME_FILE_ID_QUERY = "SELECT cm.FirstName, cm.LastName, cm.IsActive,cm.ClientCreatedOn, cfm.FileID FROM ClientMaster AS cm INNER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID WHERE cm.IsActive='false'  order by cm.LastName desc";
        internal const string ALL_CLIENT_NAME_FILE_ID_QUERY = "SELECT cm.FirstName, cm.LastName, cm.IsActive,cm.ClientCreatedOn, cfm.FileID FROM ClientMaster AS cm INNER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID  order by cm.LastName desc";
        internal const string CLIENT_BASIC_DETAILS_QUERY = "SELECT cfm.FileID, cfm.CaseStatus, cfm.AccidentDate, ct.CaseType FROM ClientFileMaster AS cfm INNER JOIN LuCaseType AS ct ON cfm.CaseTypeID = ct.CaseTypeID WHERE (cfm.FileID = {0})";
        internal const string CLIENT_BASIC_ADDRESS_DETAILS_QUERY = "SELECT Address, City, State, HomePhone, CellPhone, LicenseNumber, DateOfBirth, SSN, Email, SuiteAddress,WorkPhone FROM ClientBasicDetails WHERE FileID = {0}";
        internal const string CLIENT_BASIC_INSURANCE_DETAILS_QUERY = "SELECT InitialInformation, DefendantName, OriginatingAttorny, AssignedAttorny, Referral FROM CaseInformation WHERE FileID = {0}";
        internal const string CLIENT_BASIC_EVIDENCE_DETAILS_QUERY = "SELECT Evidence FROM ClientEvidences WHERE FileID = {0}";
        internal const string ALL_CASE_TYPE = "SELECT CaseType FROM LuCaseType";

        internal const string CLIENT_ADDITIONAL_INFO_QUERY = "SELECT Occupation,Employer,Address,City,State from ClientEmployerDetails WHERE FileID = {0}";
        internal const string CLIENT_ADDITIONAL_SPOUSE_QUERY = "SELECT FirstName, LastName,Occupation,Employer,Address,City,State from ClientSpouseDetails WHERE FileID = {0}";
        internal const string CLIENT_AUTO_INFO_QUERY = "SELECT InsuranceCompany,Address,City,PhoneNumber,Adjuster,State,zip from ClientAutoDetails WHERE FileID = {0}";
        internal const string CLIENT_AUTO_POLICY_INFO_QUERY = "SELECT PolicyNumber,EffectiveStartDate,MedPayAvailable,Amount,EffectiveEndDate,LiabilityMinCoverage,LiabilityMaxCoverage,UMIMinValue,UMIMaxValue,Reimbursable,Notes from ClientPolicyDetails WHERE FileID = {0}";
        internal const string CLIENT_MEDICAL_INSURANCE_QUERY = "SELECT NamedInsured,InsuranceCompany,Address,City,state,Zip,PhoneNumber,PolicyNumber,MediCalNumber,MediCareNumber,ClaimNumber from ClientMedicalInsurance WHERE FileID = {0}";
        internal const string CLIENT_DEFENDANT_INFORMATION_QUERY = "SELECT LastName,FirstName,Address,City,state,Zip,HomePhone,BusinessPhone,DateOfBirth,LicenseNumber from DefendantInformation WHERE FileID = {0}";
        internal const string CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY = "SELECT Firm,Attorney,Address,City,state,Zip,PhoneNumber from DefendantAttorneyDetails WHERE FileID = {0}";
        internal const string CLIENT_DEFENDANT_INSURANCE_QUERY = "SELECT NameOfInsured, InsuranceCompany, Address, City, State, Zip, PhoneNumber, ClaimNumber, Adjuster, PolicyLimits FROM DefendantInsuranceDetails WHERE FileID = {0}";
        internal const string CLIENT_COURT_DETAILS_QUERY = "SELECT  CaseNumber, Court, Address, City, Zip, State FROM   ClientCourtDetails WHERE FileID = {0}";
        internal const string CLIENT_INJURY_QUERY = "SELECT FileID, InjuryNoteNumber, CreatedDate, LastModifiedDate,Description FROM   ClientInjuries WHERE FileID = {0} order by LastModifiedDate desc ";
        internal const string CLIENT_MISC_NOTES_QUERY = "SELECT NoteNumber, CreatedDate, ModifiedDate, Description FROM   ClientMiscNotes WHERE FileID = {0} order by ModifiedDate desc";

        internal const string CLIENT_STATUTE_INFORMATION = @"SELECT     st.AccidentDate, st.ComplaintFileDate, st.IsGovtClaim, city.DeniedDate AS City_DeniedDate, city.ClaimDueDate AS City_ClaimDueDate, 
                                                                                state.DeniedDate AS State_DeniedDate, state.FiledDate AS State_FiledDate, county.DeniedDate AS county_DeniedDate, county.FiledDate AS county_FiledDate, 
                                                                                other.DeniedDate AS other_DeniedDate, other.FiledDate AS other_FiledDate
                                                                FROM            StatuteInformation AS st LEFT JOIN
                                                                                        [ClientGovtClaims-City] AS city ON city.FileID = st.FileID LEFT JOIN
                                                                                        [ClientGovtClaims-State] AS state ON state.FileID = st.FileID LEFT JOIN
                                                                                        [ClientGovtClaims-Country] AS county ON county.FileID = st.FileID LEFT JOIN
                                                                                        [ClientGovtClaims-Other] AS other ON other.FileID = st.FileID
                                                                WHERE     (st.FileID = {0})";


        internal const string ADD_NEW_EVIDENCE_QUERY = "INSERT INTO ClientEvidences(FileID,Evidence) VALUES({0},'{1}')";
        internal const string EDIT_EVIDENCE_QUERY = "UPDATE ClientEvidences SET Evidence = '{0}' WHERE FileID = {1} AND Evidence = '{2}'";
        internal const string DELETE_EVIDENCE_QUERY = "DELETE ClientEvidences WHERE FileID = {0} AND Evidence = '{1}'";

        internal const string NEW_CLIENT_ID_QUERY = "SELECT MAX(ClientID) FROM ClientMaster";
        internal const string CASE_TYPE_ID_QUERY = "SELECT CaseTypeID FROM LuCaseType WHERE CaseType='{0}'";
        internal const string INSERT_CLIENT_MASTER_QUERY = "INSERT INTO ClientMaster(FirstName,LastName,IsActive,ClientCreatedOn) VALUES('{0}','{1}','{2}','{3}')";
        internal const string INSERT_CLIENT_FILE_MASTER_QUERY = "INSERT INTO ClientFileMaster(ClientID,CaseTypeID,AccidentDate,CaseStatus,FileID) VALUES({0},{1},'{2}','{3}','{4}')";
        internal const string INSERT_CLIENT_BASIC_DETAILS_QUERY = "INSERT INTO ClientBasicDetails(FileID,Address,City,State,HomePhone,CellPhone,LicenseNumber,DateOfBirth,SSN,Email,WorkPhone,SuiteAddress) VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
        //internal const string INSERT_CLIENT_BASIC_DETAILS_QUERY = "INSERT INTO ClientBasicDetails(FileID,Address,City,State,HomePhone,CellPhone,LicenseNumber,DateOfBirth,SSN) VALUES({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
        internal const string INSERT_CLIENT_CASE_INFO_QUERY = "INSERT INTO CaseInformation(FileID,InitialInformation,DefendantName,OriginatingAttorny,AssignedAttorny,Referral) VALUES({0},'{1}','{2}','{3}','{4}','{5}')";


        internal const string INSERT_CLIENT_EMPLOYER_DETAILS_QUERY = "INSERT INTO ClientEmployerDetails(FileID, Occupation,Employer,Address,City,State) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";
        internal const string INSERT_CLIENT_SPOUSE_DETAILS_QUERY = "INSERT INTO ClientSpouseDetails(FileID,FirstName,LastName, Occupation,Employer,Address,City,State) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";

        internal const string INSERT_CLIENT_AUTO_DETAILS_QUERY = "INSERT INTO ClientAutoDetails(FileID, InsuranceCompany,Address,City,State,Zip,PhoneNumber,Adjuster) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
        internal const string INSERT_CLIENT_POLICY_DETAILS_QUERY = "INSERT INTO ClientPolicyDetails( FileID, PolicyNumber, EffectiveStartDate, Amount, EffectiveEndDate, LiabilityMinCoverage, LiabilityMaxCoverage, UMIMinValue, UMIMaxValue, Notes, MedPayAvailable, Reimbursable) VALUES('{0}','{1}','{2}',{3},'{4}',{5},{6},{7},{8},'{9}','{10}','{11}')";

        internal const string INSERT_CLIENT_MEDICAL_INFORMATION_QUERY = "INSERT INTO ClientMedicalInsurance(FileID, NamedInsured, InsuranceCompany, Address, City, State, Zip, PhoneNumber, PolicyNumber, MediCalNumber, MediCareNumber,ClaimNumber) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
        internal const string INSERT_CLIENT_INJURY_NOTE_QUERY = "INSERT INTO ClientInjuries(FileID, InjuryNoteNumber, CreatedDate, LastModifiedDate, Description) VALUES('{0}','{1}','{2}','{3}','{4}')";
        internal const string UPDATE_CLIENT_INJURY_NOTE_QUERY = "UPDATE ClientInjuries SET InjuryNoteNumber='{0}', CreatedDate='{1}', LastModifiedDate='{2}', Description='{3}' WHERE FileID ='{4}' AND InjuryNoteNumber='{5}' AND CreatedDate='{6}' AND LastModifiedDate='{7}' AND Description='{8}' ";
        internal const string DELETE_CLIENT_INJURY_NOTE_QUERY = "DELETE FROM ClientInjuries WHERE FileID ='{0}' AND InjuryNoteNumber='{1}' AND CreatedDate='{2}' AND LastModifiedDate='{3}' AND Description='{4}' ";

        internal const string INSERT_CLIENT_DEFENDANT_DETAILS_QUERY = "INSERT INTO DefendantInformation(FileID, LastName, FirstName, Address, City, State, Zip, HomePhone, BusinessPhone, DateOfBirth, LicenseNumber) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";
        internal const string INSERT_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY = "INSERT INTO DefendantAttorneyDetails(FileID, Firm, Attorney, Address, City, State, Zip, PhoneNumber) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";

        internal const string INSERT_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY = "INSERT INTO DefendantInsuranceDetails(FileID, NameOfInsured, InsuranceCompany, Address, City, State, Zip, PhoneNumber, Adjuster, ClaimNumber, PolicyLimits) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";

        internal const string INSERT_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY = "INSERT INTO StatuteInformation(FileID, AccidentDate, AccDateAfter1yr, AccDateAfter2yr, ComplaintFileDate, ComplaintAfter60days, ComplaintAfter2yrs, ComplaintAfter3yrs, ComplaintAfter5yrs, IsGovtClaim) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
        internal const string INSERT_GOVT_CITY_CLAIM_DETAILS_QUERY = "INSERT INTO [ClientGovtClaims-City](FileID, DeniedDate, DeniedDateAfter180days, ClaimDueDate, ClaimDueDateAfter60Days, ClaimDueDateAfter2yrs, ClaimDueDateAfter3yrs, ClaimDueDateAfter5yrs) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
        internal const string INSERT_GOVT_COUNTY_CLAIM_DETAILS_QUERY = "INSERT INTO [ClientGovtClaims-Country](FileID, DeniedDate, DeniedDateAfter180Days, FiledDate, FiledDateAfter60Days, FiledDateAfter2yrs, FiledDateAfter3yrs, FiledDateAfter5yrs) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
        internal const string INSERT_GOVT_STATE_CLAIM_DETAILS_QUERY = "INSERT INTO [ClientGovtClaims-State](FileID, DeniedDate, DeniedDateAfter180Days, FiledDate, FiledDateAfter60Days, FiledDateAfter2yrs, FiledDateAfter3yrs, FiledDateAfter5yrs) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
        internal const string INSERT_GOVT_OTHER_CLAIM_DETAILS_QUERY = "INSERT INTO [ClientGovtClaims-Other](FileID, DeniedDate, DeniedDateAfter180Days, FiledDate, FiledDateAfter60Days, FiledDateAfter2yrs, FiledDateAfter3yrs, FiledDateAfter5yrs) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";


        internal const string INSERT_COURT_INFORMATION_QUERY = "INSERT INTO ClientCourtDetails( FileID, CaseNumber, Court, Address, City, State, Zip) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";

        internal const string INSERT_MISC_NOTES_QUERY = "INSERT INTO ClientMiscNotes(FileID, NoteNumber, CreatedDate, ModifiedDate, Description) VALUES('{0}','{1}','{2}','{3}','{4}')";
        internal const string UPDATE_MISC_NOTES_QUERY = "UPDATE ClientMiscNotes SET NoteNumber='{0}', CreatedDate='{1}', ModifiedDate='{2}', Description='{3}' WHERE FileID='{4}' AND NoteNumber='{5}' AND CreatedDate='{6}' AND ModifiedDate='{7}' AND Description='{8}'";
        internal const string DELETE_MISC_NOTES_QUERY = "DELETE ClientMiscNotes WHERE FileID='{0}' AND NoteNumber='{1}' AND CreatedDate='{2}' AND ModifiedDate='{3}' AND Description='{4}'";


        internal const string UPDATE_CLIENT_STATUS_QUERY = "UPDATE ClientMaster SET IsActive = '{0}' WHERE (ClientID IN (SELECT ClientID FROM ClientFileMaster WHERE (FileID = '{1}')))";

        internal const string UPDATE_CLIENT_MASTER_QUERY = "UPDATE ClientMaster SET FirstName='{0}', LastName= '{1}',IsActive ='{2}', ClientCreatedOn='{3}' WHERE ClientID IN (SELECT ClientID FROM ClientFileMaster Where FileID = '{4}')";
        internal const string UPDATE_CLIENT_FILE_MASTER_QUERY = "UPDATE ClientFileMaster SET CaseTypeID='{0}',AccidentDate='{1}',CaseStatus='{2}' WHERE CaseTypeID='{3}' AND AccidentDate='{4}' AND CaseStatus='{5}' AND FileID='{6}'";
        internal const string UPDATE_CLIENT_BASIC_DETAILS_QUERY = "UPDATE ClientBasicDetails SET Address='{0}',City='{1}',State='{2}',HomePhone='{3}',CellPhone='{4}',LicenseNumber='{5}',DateOfBirth ='{6}',SSN='{7}',Email='{17}',SuiteAddress='{18}',WorkPhone='{19}' WHERE Address='{8}' AND City='{9}'  AND State='{10}' AND HomePhone='{11}' AND CellPhone='{12}' AND LicenseNumber='{13}' AND DateOfBirth ='{14}' AND SSN='{15}' AND FileID='{16}'";
        internal const string UPDATE_CLIENT_CASE_INFO_QUERY = "UPDATE CaseInformation SET InitialInformation='{0}',DefendantName='{1}',OriginatingAttorny='{2}',AssignedAttorny='{3}',Referral='{4}' WHERE InitialInformation='{5}' AND DefendantName='{6}' AND OriginatingAttorny='{7}' AND AssignedAttorny='{8}' AND Referral='{9}' AND FileID ='{10}'";

        internal const string EXISTS_CLIENT_EMPLOYER_DETAILS_QUERY = "SELECT 1 FROM ClientEmployerDetails Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_EMPLOYER_DETAILS_QUERY = "UPDATE ClientEmployerDetails SET Occupation='{1}',Employer='{2}',Address='{3}',City='{4}',State= '{5}'  Where FileID = '{0}'";
        internal const string EXISTS_CLIENT_SPOUSE_DETAILS_QUERY = "SELECT 1 FROM ClientSpouseDetails Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_SPOUSE_DETAILS_QUERY = "UPDATE ClientSpouseDetails SET FirstName='{1}',LastName='{2}', Occupation='{3}',Employer='{4}',Address='{5}',City='{6}',State='{7}'Where FileID = '{0}'";

        internal const string EXISTS_CLIENT_AUTO_DETAILS_QUERY = "SELECT 1 FROM ClientAutoDetails Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_AUTO_DETAILS_QUERY = "UPDATE ClientAutoDetails SET InsuranceCompany='{1}',Address='{2}',City='{3}',State='{4}',Zip='{5}',PhoneNumber='{6}',Adjuster='{7}' Where FileID = '{0}'";
        internal const string EXISTS_CLIENT_POLICY_DETAILS_QUERY = "SELECT 1 FROM ClientPolicyDetails Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_POLICY_DETAILS_QUERY = "UPDATE ClientPolicyDetails SET PolicyNumber='{1}', EffectiveStartDate='{2}', Amount='{3}', EffectiveEndDate='{4}', LiabilityMinCoverage='{5}', LiabilityMaxCoverage='{6}', UMIMinValue='{7}', UMIMaxValue='{8}', Notes='{9}', MedPayAvailable='{10}', Reimbursable='{11}' Where FileID = '{0}'";

        internal const string EXISTS_CLIENT_MEDICAL_INFORMATION_QUERY = "SELECT 1 FROM ClientMedicalInsurance Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_MEDICAL_INFORMATION_QUERY = "UPDATE ClientMedicalInsurance SET NamedInsured='{1}', InsuranceCompany='{2}', Address='{3}', City='{4}', State='{5}', Zip='{6}', PhoneNumber='{7}', PolicyNumber='{8}', MediCalNumber='{9}', MediCareNumber='{10}', ClaimNumber='{11}' Where FileID = '{0}'";


        internal const string EXISTS_CLIENT_DEFENDANT_DETAILS_QUERY = "SELECT 1 FROM DefendantInformation Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_DEFENDANT_DETAILS_QUERY = "UPDATE DefendantInformation SET  LastName='{1}', FirstName='{2}', Address='{3}', City='{4}', State='{5}', Zip='{6}', HomePhone='{7}', BusinessPhone='{8}', DateOfBirth='{9}', LicenseNumber='{10}' Where FileID = '{0}'";
        internal const string EXISTS_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY = "SELECT 1 FROM DefendantAttorneyDetails  WHERE FileID = '{0}'";
        internal const string UPDATE_CLIENT_DEFENDANT_ATTORNEY_DETAILS_QUERY = "UPDATE DefendantAttorneyDetails SET  Firm='{1}', Attorney='{2}', Address='{3}', City='{4}', State='{5}', Zip='{6}', PhoneNumber='{7}' WHERE FileID = '{0}'";

        internal const string EXISTS_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY = "SELECT 1 FROM  DefendantInsuranceDetails Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_DEFENDANT_INSURANCE_DETAILS_QUERY = "UPDATE DefendantInsuranceDetails SET NameOfInsured='{1}', InsuranceCompany='{2}', Address='{3}', City='{4}', State='{5}', Zip='{6}', PhoneNumber='{7}', Adjuster='{8}', ClaimNumber='{9}', PolicyLimits='{10}'  Where FileID = '{0}'";

        internal const string EXISTS_COURT_INFORMATION_QUERY = "SELECT 1 FROM  ClientCourtDetails Where FileID = '{0}'";
        internal const string UPDATE_COURT_INFORMATION_QUERY = "UPDATE ClientCourtDetails SET CaseNumber='{1}', Court='{2}', Address='{3}', City='{4}', State='{5}', Zip='{6}' Where FileID = '{0}'";


        internal const string EXISTS_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY = "SELECT 1 FROM  StatuteInformation  Where FileID = '{0}'";
        internal const string UPDATE_CLIENT_STATUTE_INFORMATION_DETAILS_QUERY = "UPDATE StatuteInformation SET  AccidentDate='{1}', AccDateAfter1yr='{2}', AccDateAfter2yr='{3}', ComplaintFileDate='{4}', ComplaintAfter60days='{5}', ComplaintAfter2yrs='{6}', ComplaintAfter3yrs='{7}', ComplaintAfter5yrs='{8}', IsGovtClaim='{9}' Where FileID = '{0}'";

        internal const string EXISTS_GOVT_CITY_CLAIM_DETAILS_QUERY = "SELECT 1 FROM  [ClientGovtClaims-City] Where FileID = '{0}'";
        internal const string UPDATE_GOVT_CITY_CLAIM_DETAILS_QUERY = "UPDATE [ClientGovtClaims-City] SET DeniedDate='{1}', DeniedDateAfter180days='{2}', ClaimDueDate='{3}', ClaimDueDateAfter60Days='{4}', ClaimDueDateAfter2yrs='{5}', ClaimDueDateAfter3yrs='{6}', ClaimDueDateAfter5yrs='{7}' Where FileID = '{0}'";

        internal const string EXISTS_GOVT_COUNTY_CLAIM_DETAILS_QUERY = "SELECT 1 FROM  [ClientGovtClaims-Country]  Where FileID = '{0}'";
        internal const string UPDATE_GOVT_COUNTY_CLAIM_DETAILS_QUERY = "UPDATE [ClientGovtClaims-Country] SET  DeniedDate='{1}', DeniedDateAfter180Days='{2}', FiledDate='{3}', FiledDateAfter60Days='{4}', FiledDateAfter2yrs='{5}', FiledDateAfter3yrs='{6}', FiledDateAfter5yrs='{7}' Where FileID = '{0}'";

        internal const string EXISTS_GOVT_STATE_CLAIM_DETAILS_QUERY = "SELECT 1 FROM [ClientGovtClaims-State]  Where FileID = '{0}'";
        internal const string UPDATE_GOVT_STATE_CLAIM_DETAILS_QUERY = "UPDATE [ClientGovtClaims-State] SET  DeniedDate='{1}', DeniedDateAfter180Days='{2}', FiledDate='{3}', FiledDateAfter60Days='{4}', FiledDateAfter2yrs='{5}', FiledDateAfter3yrs='{6}', FiledDateAfter5yrs='{7}' Where FileID = '{0}'";

        internal const string EXISTS_GOVT_OTHER_CLAIM_DETAILS_QUERY = "SELECT 1 FROM [ClientGovtClaims-Other] Where FileID = '{0}'";
        internal const string UPDATE_GOVT_OTHER_CLAIM_DETAILS_QUERY = "UPDATE [ClientGovtClaims-Other] SET  DeniedDate='{1}', DeniedDateAfter180Days='{2}', FiledDate='{3}', FiledDateAfter60Days='{4}', FiledDateAfter2yrs='{5}', FiledDateAfter3yrs='{6}', FiledDateAfter5yrs='{7}' Where FileID = '{0}'";

        internal const string EXISTS_FILE_ID_QUERY = "SELECT 1 FROM ClientFileMaster Where FileID = '{0}'";
        internal const string NEW_FILE_ID_QUERY = "SELECT MAX(FileID) FROM ClientFileMaster ";

        internal const string VALIDATE_USER_QUERY = "SELECT 1 FROM Users WHERE UserName='{0}' AND Password='{1}' AND IsAdmin='true'";

        internal const string GET_CLIENTID_FROM_FILE_ID_QUERY = "SELECT Distinct(ClientID) FROM ClientFileMaster WHERE FileID = '{0}'";
        internal const string GET_FILEID__QUERY = "SELECT FileID FROM ClientFileMaster WHERE ClientID = '{0}'";
        internal const string DELETE_CLIENT_FROM_CLIENTFILEMASTER_QUERY = "DELETE ClientFileMaster WHERE ClientID = '{0}'";
        internal const string DELETE_CLIENT_CLIENTMASTER_QUERY = "DELETE ClientMaster WHERE ClientID = '{0}'";
        internal const string DELETE_CASE_INFORMATION_QUERY = "DELETE CaseInformation WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_AUTO_DETAILS_QUERY = "DELETE ClientAutoDetails WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_BASIC_DETAILS_QUERY = "DELETE ClientBasicDetails WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_COURT_DETAILS_QUERY = "DELETE ClientCourtDetails WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_EMPLOYER_DETAILS_QUERY = "DELETE ClientEmployerDetails WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_EVIDENCES_QUERY = "DELETE ClientEvidences WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_GOVT_CLAIM_CITY_QUERY = "DELETE [ClientGovtClaims-City] WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_GOVT_CLAIM_STATE_QUERY = "DELETE [ClientGovtClaims-State] WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_GOVT_CLAIM_COUNTY_QUERY = "DELETE [ClientGovtClaims-Country] WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_GOVT_CLAIM_OTHER_QUERY = "DELETE [ClientGovtClaims-Other] WHERE FileID = '{0}'";
        internal const string DELETE_CASE_INJURIES_QUERY = "DELETE ClientInjuries WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_MEDICAL_INSURANCE_QUERY = "DELETE ClientMedicalInsurance WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_MISC_NOTE_QUERY = "DELETE ClientMiscNotes WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_POLICY_DETAILS_QUERY = "DELETE ClientPolicyDetails WHERE FileID = '{0}'";
        internal const string DELETE_CLIENT_SPOUSE_DETAILS_QUERY = "DELETE ClientSpouseDetails WHERE FileID = '{0}'";
        internal const string DELETE_DEFENDANT_ATTORNEY_DETAILS_QUERY = "DELETE DefendantAttorneyDetails WHERE FileID = '{0}'";
        internal const string DELETE_DEFENDANT_INSURANCE_DETAILS_QUERY = "DELETE DefendantInsuranceDetails WHERE FileID = '{0}'";
        internal const string DELETE_STATUTE_DETAILS_QUERY = "DELETE StatuteInformation WHERE FileID = '{0}'";


        internal const string BY_ASSIGNED_ATTORNEY_REPORT_QUERY = "SELECT cm.LastName + ' , ' + cm.FirstName AS ClientName, cfm.FileID, ci.AssignedAttorny, cfm.AccidentDate, ccd.CaseNumber FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID LEFT OUTER JOIN ClientCourtDetails AS ccd ON ccd.FileID = cfm.FileID WHERE (ci.AssignedAttorny LIKE '%{0}%')";
        internal const string ALL_ASSIGNED_ATTORNEY_QUERY = "SELECT DISTINCT AssignedAttorny FROM  CaseInformation";

        internal const string BY_ORIGINATING_CLIENT_REPORT_QUERY = "SELECT cm.LastName + ' , ' + cm.FirstName AS ClientName, cfm.FileID, ci.OriginatingAttorny, cfm.AccidentDate, ccd.CaseNumber FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID LEFT OUTER JOIN ClientCourtDetails AS ccd ON ccd.FileID = cfm.FileID WHERE (ci.OriginatingAttorny LIKE '%{0}%')";
        internal const string ALL_ORIGINATING_ATTORNEY_QUERY = "SELECT DISTINCT OriginatingAttorny FROM  CaseInformation";

        internal const string BY_REFERRAL_CLIENT_REPORT_QUERY = "SELECT cm.LastName + ' , ' + cm.FirstName AS ClientName, cfm.FileID, ci.Referral, cfm.AccidentDate, ccd.CaseNumber FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID LEFT OUTER JOIN ClientCourtDetails AS ccd ON ccd.FileID = cfm.FileID WHERE (ci.Referral LIKE '%{0}%')";
        internal const string ALL_REFERRAL_QUERY = "SELECT DISTINCT Referral FROM  CaseInformation";

        internal const string ALL_CLIENTS_REPORT_QUERY = "SELECT cm.LastName + ' , ' + cm.FirstName AS ClientName, cfm.FileID, ci.AssignedAttorny, cfm.AccidentDate, ccd.CaseNumber FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID LEFT OUTER JOIN ClientCourtDetails AS ccd ON ccd.FileID = cfm.FileID";
        internal const string ALL_CLIENTS_WITH_STATUS_REPORT_QUERY = "SELECT cm.LastName + ' , ' + cm.FirstName AS ClientName, cfm.FileID, ci.AssignedAttorny, cfm.AccidentDate, ccd.CaseNumber FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cm.ClientID = cfm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID LEFT OUTER JOIN ClientCourtDetails AS ccd ON ccd.FileID = cfm.FileID WHERE (cm.IsActive ='{0}')";

        internal const string ALL_CLIENTS_TRANSACTION_REPORT_QUERY = " SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cfm.ClientID = cm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID";
        internal const string ALL_CLIENTS_TRANSACTION_WITH_STATUS_REPORT_QUERY = "SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny FROM ClientMaster AS cm LEFT OUTER JOIN ClientFileMaster AS cfm ON cfm.ClientID = cm.ClientID LEFT OUTER JOIN CaseInformation AS ci ON ci.FileID = cfm.FileID WHERE (cm.IsActive = '{0}')";
        internal const string ALL_CLIENTS_TRANSACTION_TOTAL_QUERY = " SELECT SUM(GeneralFund) AS GenTotal, SUM(TrustFund) AS TrustTotal, FileID FROM ClientTransactionDetails GROUP BY FileID";


        internal const string STATUTE_60_DAYS_REPORT_QUERY = @"SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ClaimDueDateAfter60Days AS Date
                                                                FROM [ClientGovtClaims-City] AS city
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = city.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = city.FileID
                                                                WHERE (DATENAME(month, ClaimDueDateAfter60Days) = '{0}') AND (DATENAME(year, ClaimDueDateAfter60Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter60Days AS Date
                                                                FROM [ClientGovtClaims-Country] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, FiledDateAfter60Days) = '{0}') AND (DATENAME(year, FiledDateAfter60Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter60Days AS Date
                                                                FROM [ClientGovtClaims-Other] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, FiledDateAfter60Days) = '{0}') AND (DATENAME(year, FiledDateAfter60Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter60Days AS Date
                                                                FROM [ClientGovtClaims-State] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, FiledDateAfter60Days) = '{0}') AND (DATENAME(year, FiledDateAfter60Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ComplaintAfter60days AS Date
                                                                FROM StatuteInformation AS temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, ComplaintAfter60days) = '{0}') AND (DATENAME(year, ComplaintAfter60days) = '{1}')";

        internal const string STATUTE_180_DAYS_REPORT_QUERY = @"SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, DeniedDateAfter180days AS Date
                                                                FROM [ClientGovtClaims-City] AS city
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = city.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = city.FileID
                                                                WHERE (DATENAME(month, DeniedDateAfter180days) = '{0}') AND (DATENAME(year, DeniedDateAfter180days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, DeniedDateAfter180Days AS Date
                                                                FROM [ClientGovtClaims-Country] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, DeniedDateAfter180Days) = '{0}') AND (DATENAME(year, DeniedDateAfter180Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, DeniedDateAfter180Days AS Date
                                                                FROM [ClientGovtClaims-Other] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, DeniedDateAfter180Days) = '{0}') AND (DATENAME(year, DeniedDateAfter180Days) = '{1}')
                                                                UNION
                                                                SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, DeniedDateAfter180Days AS Date
                                                                FROM [ClientGovtClaims-State] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                WHERE (DATENAME(month, DeniedDateAfter180Days) = '{0}') AND (DATENAME(year, DeniedDateAfter180Days) = '{1}')";

        internal const string STATUTE_1_YEARS_REPORT_QUERY = @"	SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, AccDateAfter1yr AS Date
	                                                                FROM StatuteInformation si
	                                                                    INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = si.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = si.FileID
	                                                                WHERE (DATENAME(month, AccDateAfter1yr) = '{0}' AND DATENAME(year, AccDateAfter1yr) = '{1}')";

        internal const string STATUTE_2_YEARS_REPORT_QUERY = @"SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ClaimDueDateAfter2yrs AS Date
		                                                        FROM [ClientGovtClaims-City] AS city
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = city.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = city.FileID
		                                                        WHERE (DATENAME(month, ClaimDueDateAfter2yrs) = '{0}') AND (DATENAME(year, ClaimDueDateAfter2yrs) = '{1}')
		                                                        UNION
		                                                        SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter2yrs AS Date
		                                                        FROM [ClientGovtClaims-Country] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
		                                                        WHERE (DATENAME(month, FiledDateAfter2yrs) = '{0}') AND (DATENAME(year, FiledDateAfter2yrs) = '{1}')
		                                                        UNION
		                                                        SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter2yrs AS Date
		                                                        FROM [ClientGovtClaims-Other] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
		                                                        WHERE (DATENAME(month, FiledDateAfter2yrs) = '{0}') AND (DATENAME(year, FiledDateAfter2yrs) = '{1}')
		                                                        UNION
		                                                        SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter2yrs AS Date
		                                                        FROM [ClientGovtClaims-State] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
		                                                        WHERE (DATENAME(month, FiledDateAfter2yrs) = '{0}') AND (DATENAME(year, FiledDateAfter2yrs) = '{1}')
		                                                        UNION
		                                                        SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, AccDateAfter2yr AS Date
		                                                        FROM StatuteInformation AS temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
		                                                        WHERE (DATENAME(month, AccDateAfter2yr) = '{0}') AND (DATENAME(year, AccDateAfter2yr) = '{1}')
		                                                        UNION
		                                                        SELECT  cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ComplaintAfter2yrs AS Date
		                                                        FROM StatuteInformation AS temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
		                                                        WHERE (DATENAME(month, ComplaintAfter2yrs) = '{0}') AND (DATENAME(year, ComplaintAfter2yrs) = '{1}')";

        internal const string STATUTE_3_YEARS_REPORT_QUERY = @"	SELECT cm.LastName + ', ' + cm.FirstName AS Name,cfm.FileID, ci.AssignedAttorny, ClaimDueDateAfter3yrs AS Date
                                                                        FROM [ClientGovtClaims-City] AS city
			                                                            INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = city.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = city.FileID
                                                                        WHERE (DATENAME(month, ClaimDueDateAfter3yrs) = '{0}') AND (DATENAME(year, ClaimDueDateAfter3yrs) = '{1}')
                                                                        UNION
	                                                            SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter3yrs AS Date
                                                                        FROM [ClientGovtClaims-Country] temp
			                                                            INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                        WHERE (DATENAME(month, FiledDateAfter3yrs) = '{0}') AND (DATENAME(year, FiledDateAfter3yrs) = '{1}')
                                                                        UNION
	                                                            SELECT cm.LastName + ', ' + cm.FirstName AS Name,cfm.FileID, ci.AssignedAttorny, FiledDateAfter3yrs AS Date
                                                                        FROM [ClientGovtClaims-Other] temp
			                                                            INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                        WHERE (DATENAME(month, FiledDateAfter3yrs) = '{0}') AND (DATENAME(year, FiledDateAfter3yrs) = '{1}')
                                                                        UNION
	                                                            SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter3yrs AS Date
                                                                        FROM [ClientGovtClaims-State] temp
			                                                            INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                        WHERE (DATENAME(month, FiledDateAfter3yrs) = '{0}') AND (DATENAME(year, FiledDateAfter3yrs) = '{1}')
                                                                        UNION
	                                                            SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ComplaintAfter3yrs AS Date
                                                                        FROM StatuteInformation temp
			                                                            INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                            INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                            INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                        WHERE (DATENAME(month, ComplaintAfter3yrs) = '{0}') AND (DATENAME(year, ComplaintAfter3yrs) = '{1}')";

        internal const string STATUTE_5_YEARS_REPORT_QUERY = @"SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ClaimDueDateAfter5yrs AS Date
                                                                    FROM [ClientGovtClaims-City] AS city
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = city.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = city.FileID
                                                                    WHERE (DATENAME(month, ClaimDueDateAfter5yrs) = '{0}') AND (DATENAME(year, ClaimDueDateAfter5yrs) = '{1}')
                                                                    UNION
	                                                        SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter5yrs AS Date
                                                                    FROM [ClientGovtClaims-Country] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                    WHERE (DATENAME(month, FiledDateAfter5yrs) = '{0}') AND (DATENAME(year, FiledDateAfter5yrs) = '{1}')
                                                                    UNION
	                                                        SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter5yrs AS Date
                                                                    FROM [ClientGovtClaims-Other] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                    WHERE (DATENAME(month, FiledDateAfter5yrs) = '{0}') AND (DATENAME(year, FiledDateAfter5yrs) = '{1}')
                                                                    UNION
	                                                        SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, FiledDateAfter5yrs AS Date
                                                                    FROM [ClientGovtClaims-State] temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                    WHERE (DATENAME(month, FiledDateAfter5yrs) = '{0}') AND (DATENAME(year, FiledDateAfter5yrs) = '{1}')
                                                                    UNION
	                                                        SELECT cm.LastName + ', ' + cm.FirstName AS Name, cfm.FileID, ci.AssignedAttorny, ComplaintAfter5yrs AS Date
                                                                    FROM StatuteInformation temp
			                                                        INNER JOIN ClientFileMaster AS cfm ON cfm.FileID = temp.FileID
			                                                        INNER JOIN ClientMaster AS cm ON cm.ClientID = cfm.ClientID
			                                                        INNER JOIN CaseInformation AS ci ON ci.FileID = temp.FileID
                                                                    WHERE (DATENAME(month, ComplaintAfter5yrs) = '{0}') AND (DATENAME(year, ComplaintAfter5yrs) = '{1}')";
                                                                      
      
        internal const string NAME = "Name";
        internal const string FILEID = "FileID";
        internal const string ASSIGNEDATTORNY = "AssignedAttorny";
        internal const string GENTOTAL = "GenTotal";
        internal const string TRUSTTOTAL = "TrustTotal";
        internal const string DATE = "Date";

        /*
         * SELECT SUM(GeneralFund) AS GenTotal, SUM(TrustFund) AS TrustTotal, FileID FROM ClientTransactionDetails GROUP BY FileID
         */
        internal const string CLIENT_NAME = "ClientName";
        
        internal const string CLIENT_FIRST_NAME = "FirstName";
        internal const string CLIENT_LAST_NAME = "LastName";
        internal const string CLIENT_STATUS = "IsActive";
        internal const string CLIENT_CREATED_ON = "ClientCreatedOn";
        internal const string FILE_ID = "FileID";
        internal const string CASE_TYPE_COLUMN = "CaseType";
        internal const string CASE_STATUS_COLUMN = "CaseStatus";
        internal const string ACCIDENT_DATE_COLUMN = "AccidentDate";

        internal const string CLIENT_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_CITY_COLUMN = "City";
        internal const string CLIENT_STATE_COLUMN = "State";
        internal const string CLIENT_HOMEPHONE_COLUMN = "HomePhone";
        internal const string CLIENT_CELLPHONE_COLUMN = "CellPhone";
        internal const string CLIENT_LICENSENUMBER_COLUMN = "LicenseNumber";
        internal const string CLIENT_DATEOFBIRTH_COLUMN = "DateOfBirth";
        internal const string CLIENT_SSN_COLUMN = "SSN";

        internal const string CLIENT_EMAIL_COLUMN = "Email";
        internal const string CLIENT_WORKPHONE_COLUMN = "WorkPhone";
        internal const string CLIENT_SUITEADDRESS_COLUMN = "SuiteAddress";

        internal const string CLIENT_INITIALINFORMATION_COLUMN = "InitialInformation";
        internal const string CLIENT_DEFENDANTNAME_COLUMN = "DefendantName";
        internal const string CLIENT_ORIGINATINGATTORNY_COLUMN = "OriginatingAttorny";
        internal const string CLIENT_ASSIGNEDATTORNY_COLUMN = "AssignedAttorny";
        internal const string CLIENT_REFERRAL_COLUMN = "Referral";

        internal const string CLIENT_EVIDENCE_COLUMN = "Evidence";

        internal const string CLIENT_OCCUPATION_COLUMN = "Occupation";
        internal const string CLIENT_EMPLOYER_COLUMN = "Employer";
        internal const string CLIENT_ADDITIONAL_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_ADDITIONAL_CITY_COLUMN = "City";
        internal const string CLIENT_ADDITIONAL_STATE_COLUMN = "State";

        internal const string CLIENT_SPOUSE_FIRST_NAME_COLUMN = "FirstName";
        internal const string CLIENT_SPOUSE_LAST_NAME_COLUMN = "LastName";
        internal const string CLIENT_SPOUSE_OCCUPATION_COLUMN = "Occupation";
        internal const string CLIENT_SPOUSE_EMPLOYER_COLUMN = "Employer";
        internal const string CLIENT_SPOUSE_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_SPOUSE_CITY_COLUMN = "City";
        internal const string CLIENT_SPOUSE_STATE_COLUMN = "State";

        internal const string CLIENT_AUTO_INSURANCE_COMPANY_COLUMN = "InsuranceCompany";
        internal const string CLIENT_AUTO_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_AUTO_CITY_COLUMN = "City";
        internal const string CLIENT_AUTO_PHONE_NUMBER_COLUMN = "PhoneNumber";
        internal const string CLIENT_AUTO_ADJUSTER_COLUMN = "Adjuster";
        internal const string CLIENT_AUTO_STATE_COLUMN = "State";
        internal const string CLIENT_AUTO_ZIP_COLUMN = "Zip";

        internal const string CLIENT_AUTO_POLICY_NUMBER_COLUMN = "PolicyNumber";
        internal const string CLIENT_AUTO_EFFECTIVE_START_DATE_COLUMN = "EffectiveStartDate";
        internal const string CLIENT_AUTO_MED_PAY_AVAILABLE_COLUMN = "MedPayAvailable";
        internal const string CLIENT_AUTO_AMOUNT_COLUMN = "Amount";
        internal const string CLIENT_AUTO_EFFECTIVE_END_DATE_COLUMN = "EffectiveEndDate";
        internal const string CLIENT_AUTO_LIABILITY_MIN_COVERAGE_COLUMN = "LiabilityMinCoverage";
        internal const string CLIENT_AUTO_LIABILITY_MAX_COVERAGE_COLUMN = "LiabilityMaxCoverage";
        internal const string CLIENT_AUTO_UMIMIN_VALUE_COLUMN = "UMIMinValue";
        internal const string CLIENT_AUTO_UMIMAX_VALUE_COLUMN = "UMIMaxValue";
        internal const string CLIENT_AUTO_REIMBURSABLE_COLUMN = "Reimbursable";
        internal const string CLIENT_AUTO_NOTES_COLUMN = "Notes";

        internal const string CLIENT_MEDICAL_NAMEDINSURED_COLUMN = "NamedInsured";
        internal const string CLIENT_MEDICAL_INSURANCE_COMPANY_COLUMN = "InsuranceCompany";
        internal const string CLIENT_MEDICAL_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_MEDICAL_CITY_COLUMN = "City";
        internal const string CLIENT_MEDICAL_STATE_COLUMN = "state";
        internal const string CLIENT_MEDICAL_ZIP_COLUMN = "Zip";
        internal const string CLIENT_MEDICAL_PHONE_NUMBER_COLUMN = "PhoneNumber";
        internal const string CLIENT_MEDICAL_POLICY_NUMBER_COLUMN = "PolicyNumber";
        internal const string CLIENT_MEDICAL_MEDICAL_NUMBER_COLUMN = "MediCalNumber";
        internal const string CLIENT_MEDICAL_MEDICARE_COLUMN = "MediCareNumber";
        internal const string CLIENT_CLAIM_NUMBER_COLUMN = "ClaimNumber";

        internal const string CLIENT_DEFENDANT_LAST_NAME_COLUMN = "LastName";
        internal const string CLIENT_DEFENDANT_FIRST_NAME_COLUMN = "FirstName";
        internal const string CLIENT_DEFENDANT_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_DEFENDANT_CITY_COLUMN = "City";
        internal const string CLIENT_DEFENDANT_STATE_COLUMN = "state";
        internal const string CLIENT_DEFENDANT_ZIP_COLUMN = "Zip";
        internal const string CLIENT_DEFENDANT_HOME_PHONE_COLUMN = "HomePhone";
        internal const string CLIENT_DEFENDANT_BUSINESS_PHONE_COLUMN = "BusinessPhone";
        internal const string CLIENT_DEFENDANT_DATE_OF_BIRTH_COLUMN = "DateOfBirth";
        internal const string CLIENT_DEFENDANT_lICENSE_NUMBER_COLUMN = "LicenseNumber";

        internal const string CLIENT_DEFENDANT_ATTORNEY_FIRM_COLUMN = "FIRM";
        internal const string CLIENT_DEFENDANT_ATTORNEY_COLUMN = "Attorney";
        internal const string CLIENT_DEFENDANT_ATTORNEY_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_DEFENDANT_ATTORNEY_CITY_COLUMN = "City";
        internal const string CLIENT_DEFENDANT_ATTORNEY_STATE_COLUMN = "state";
        internal const string CLIENT_DEFENDANT_ATTORNEY_ZIP_COLUMN = "Zip";
        internal const string CLIENT_DEFENDANT_ATTORNEY_PHONE_NUMBER_COLUMN = "PhoneNumber";

        internal const string CLIENT_DEFENDANT_INSURANCE_NAME_INSURED_COLUMN = "NameOfInsured";
        internal const string CLIENT_DEFENDANT_INSURANCE_INSURANCE_COMPANY_COLUMN = "InsuranceCompany";
        internal const string CLIENT_DEFENDANT_INSURANCE_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_DEFENDANT_INSURANCE_CITY_COLUMN = "City";
        internal const string CLIENT_DEFENDANT_INSURANCE_STATE_COLUMN = "state";
        internal const string CLIENT_DEFENDANT_INSURANCE_ZIP_COLUMN = "Zip";
        internal const string CLIENT_DEFENDANT_INSURANCE_PHONE_NUMBER_COLUMN = "PhoneNumber";
        internal const string CLIENT_DEFENDANT_INSURANCE_CLAIM_NUMBER_COLUMN = "ClaimNumber";
        internal const string CLIENT_DEFENDANT_INSURANCE_ADJUSTER_COLUMN = "Adjuster";
        internal const string CLIENT_DEFENDANT_INSURANCE_POLOCY_LIMIT_COLUMN = "PolicyLimits";

        internal const string CLIENT_COURT_CASE_NUMBER_COLUMN = "CaseNumber";
        internal const string CLIENT_COURT_COURT_COLUMN = "Court";
        internal const string CLIENT_COURT_ADDRESS_COLUMN = "Address";
        internal const string CLIENT_COURT_CITY_COLUMN = "City";
        internal const string CLIENT_COURT_STATE_COLUMN = "state";
        internal const string CLIENT_COURT_ZIP_COLUMN = "Zip";

        internal const string CLIENT_INJURY_NOTE_NUMBER_COLUMN = "InjuryNoteNumber";
        internal const string CLIENT_INJURY_CREATED_DATE_COLUMN = "CreatedDate";
        internal const string CLIENT_INJURY_MODIFIED_DATE_COLUMN = "LastModifiedDate";
        internal const string CLIENT_INJURY_DESCRIPTION_COLUMN = "Description";
        
        internal const string CLIENT_MISC_NOTE_NUMBER_COLUMN = "NoteNumber";
        internal const string CLIENT_MISC_CREATED_DATE_COLUMN = "CreatedDate";
        internal const string CLIENT_MISC_MODIFIED_DATE_COLUMN = "ModifiedDate";
        internal const string CLIENT_MISC_DESCRIPTION_COLUMN = "Description";


        internal const string STATUTE_ACCIDENT_DATE = "AccidentDate";
        internal const string STATUTE_COMPLAINTFILE_DATE = "ComplaintFileDate";
        internal const string STATUTE_IS_GOVT_CLAIM = "IsGovtClaim";
        internal const string STATUTE_CITY_DENIED_DATE = "City_DeniedDate";
        internal const string STATUTE_CITY_CLAIM_DUE_DATE = "City_ClaimDueDate";
        internal const string STATUTE_STATE_DENIED_DATE = "State_DeniedDate";
        internal const string STATUTE_STATE_FILED_DATE = "State_FiledDate";
        internal const string STATUTE_COUNTY_DENIED_DATE = "county_DeniedDate";
        internal const string STATUTE_COUNTY_FILED_DATE = "county_FiledDate";
        internal const string STATUTE_OTHER_DENIED_DATE = "other_DeniedDate";
        internal const string STATUTE_OTHER_FILED_DATE = "other_FiledDate";

        // Client Billing section
        internal const string CLIENT_TRANSACTION_DETAILS_QUERY = "SELECT TransactionID, TransactionDate, Description, BillingType, GeneralFund, TrustFund, CheckNo FROM  ClientTransactionDetails WHERE FileID={0}";
        internal const string NEW_TRANSACTION_ID_QUERY = "SELECT MAX(TransactionID) AS TransactionID FROM  ClientTransactionDetails";
        internal const string ALL_BILLING_TYPE = "SELECT Type FROM LuBillingType";
        internal const string INSERT_NEW_TRANSACTION_QUERY = "INSERT INTO ClientTransactionDetails(FileID, TransactionID, TransactionDate, Description, BillingType, GeneralFund, TrustFund, CheckNo) VALUES('{0}',{1},'{2}','{3}','{4}',{5},{6},'{7}')";
        internal const string EDIT_TRANSACTION_QUERY = "UPDATE ClientTransactionDetails SET TransactionDate='{0}', Description='{1}', BillingType='{2}', GeneralFund={3}, TrustFund={4}, CheckNo='{5}' WHERE FileID ={6} AND TransactionID={7}";
        internal const string DELETE_TRANSACTION_QUERY = "DELETE ClientTransactionDetails WHERE FileID ={0} AND TransactionID={1}";
        internal const string GENERAL_ACCOUNT_TOTAL_QUERY = "SELECT SUM(GeneralFund) AS GenTotal FROM ClientTransactionDetails WHERE FileID = {0}";
        internal const string TRUST_ACCOUNT_TOTAL_QUERY = "SELECT SUM(TrustFund) AS GenTotal FROM ClientTransactionDetails WHERE FileID = {0}";
        
        internal const string TRANSACTION_ID = "TransactionID";
        internal const string TRANSACTION_DATE = "TransactionDate";
        internal const string TRANSACTION_DESCRIPTION = "Description";
        internal const string TRANSACTION_BILLING_TYPE = "BillingType";
        internal const string TRANSACTION_GENERAL_FUND = "GeneralFund";
        internal const string TRANSACTION_TRUST_FUND = "TrustFund";
        internal const string TRANSACTION_CHECK_NO = "CheckNo";

      

    }
}
