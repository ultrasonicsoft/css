create database CaseControl_Database


create table CaseInformation
(FileID bigint,
InitialInformation nvarchar(300),
defendantName nvarchar(100),
OriginatingAttorny nvarchar(200),
AssignedAttorny nvarchar(100),
Referral nvarchar(100))


create table ClientAutoDetails
(FileID bigint,
InsuranceCompany nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
PhoneNumber nvarchar(100),
Adjuster nvarchar(100),
State nvarchar(100),
Zip nvarchar(100))

create table ClientBasicDetails
(FileID bigint,
Address nvarchar(200),
City nvarchar(100),
State nvarchar(100),
HomePhone nvarchar(20),
CellPhone nvarchar(20),
LicenseNumber nvarchar(30),
DateOfBirth datetime,
SSN nvarchar(100),
Email nvarchar(100),
SuiteAddress nvarchar(100),
WorkPhone nvarchar(100))


create table ClientCourtDetails
(FileID bigint,
CaseNumber nvarchar(100),
Court nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100),
Zip nvarchar(100))

create table ClientEmployerDetails
(FileID bigint,
Occupation nvarchar(100),
Employer nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100))

create table ClientEvidences
(FileID int,
Evidence nvarchar(100))


create table ClientFileMaster
(ClientID bigint,
CaseTypeID int,
AccidentDate datetime,
CaseStatus nvarchar(100),
FileID int)

create table ClientGovtClaims
(FileID bigint,
DeniedDate datetime,
DeniedDateAfter180days datetime,
ClaimDueDate datetime,
ClaimDueDateAfter60Days datetime,
ClaimDueDateAfter2yrs datetime,
ClaimDueDateAfter3yrs datetime,
ClaimDueDateAfter5yrs datetime)

CREATE TABLE [dbo].[ClientGovtClaims-City](
	[FileID] [int] NULL,
	[DeniedDate] [datetime] NULL,
	[DeniedDateAfter180Days] [datetime] NULL,
	[ClaimDueDate] [datetime] NULL,
	[ClaimDueDateAfter60Days] [datetime] NULL,
	[ClaimDueDateAfter2yrs] [datetime] NULL,
	[ClaimDueDateAfter3yrs] [datetime] NULL,
	[ClaimDueDateAfter5yrs] [datetime] NULL
) 

create table [ClientGovtClaims-Country]
(FileID bigint,
DeniedDate datetime,
DeniedDateAfter180Days datetime,
FiledDate datetime,
FiledDateAfter60Days datetime,
FiledDateAfter2yrs datetime,
FiledDateAfter3yrs datetime,
FiledDateAfter5yrs datetime)


create table [ClientGovtClaims-Other]
(FileID bigint,
DeniedDate datetime,
DeniedDateAfter180Days datetime,
FiledDate datetime,
FiledDateAfter60Days datetime,
FiledDateAfter2yrs datetime,
FiledDateAfter3yrs datetime,
FiledDateAfter5yrs datetime)

create table [ClientGovtClaims-State]
(FileID bigint,
DeniedDate datetime,
DeniedDateAfter180Days datetime,
FiledDate datetime,
FiledDateAfter60Days datetime,
FiledDateAfter2yrs datetime,
FiledDateAfter3yrs datetime,
FiledDateAfter5yrs datetime)

create table ClientInjuries
(FileID int,
InjuryNoteNumber nvarchar(100),
CreatedDate datetime,
LastModifiedDate datetime,
Description nvarchar(500))




create table ClientMaster
(ClientID bigint identity(1,1) constraint PK_ClientMaster  primary key ,
 FirstName nvarchar(150) not null,
 LastName nvarchar(100)not null,
 IsActive bit not null,
 clientCreatedOn nvarchar(100))

create table ClientMedicalInsurance
(FileID bigint,
NamedInsured nvarchar(100),
InsuranceCompany nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100),
Zip nvarchar(100),
PhoneNumber nvarchar(100),
PolicyNumber nvarchar(100),
MediCalNumber nvarchar(100),
MediCareNumber nvarchar(100),
ClaimNumber nvarchar(100))

create table ClientMiscNotes
(FileID bigint,
NoteNumber nvarchar(100),
CreatedDate datetime,
ModifiedDate datetime,
Description nvarchar(500))

create table ClientPolicyDetails
(FileID  bigint,
PolicyNumber nvarchar(100),
EffectiveStartDate datetime,
Amount float,
EffectiveEndDate datetime,
LiabilityMinCoverage float,
LiabilityMaxCoverage float,
UMIMinValue float,
UMIMAXValue float,
Notes nvarchar(300),
MedPayAvailable nvarchar(1),
Reimbursable nvarchar(1))

create table ClientSpouseDetails
(FileID bigint,
FirstName nvarchar(100),
LastName nvarchar(100),
Occupation nvarchar(100),
Employer nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100))


create table ClientTransactionDetails
(FileID int,
TransactionID int,
TransactionDate datetime,
Description nvarchar(500),
BillingType nvarchar(50),
GeneralFund float,
TrustFund float,
CheckNo nvarchar(50))

create table DefendantAttorneyDetails
(FileID bigint,
Firm nvarchar(100),
Attorney nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100),
Zip nvarchar(100),
PhoneNumber nvarchar(100))

create table DefendantInformation
(FileID bigint,
LastName nvarchar(10),
FirstName nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100),
Zip nvarchar(100),
HomePhone nvarchar(20),
BusinessPhone nvarchar(20),
DateOfBirth datetime,
LicenseNumber nvarchar(30))

create table DefendantInsuranceDetails
(FileID bigint,
NameOfInsured nvarchar(100),
InsuranceCompany nvarchar(100),
Address nvarchar(100),
City nvarchar(100),
State nvarchar(100),
Zip nvarchar(100),
PhoneNumber nvarchar(100),
Adjuster nvarchar(100),
ClaimNumber nvarchar(100),
PolicyLimits nvarchar(100))


create table LuBillingType
(Id int,
Type nvarchar(50))

create table LuCaseType
(CaseType nvarchar(150),
CaseTypeID int Identity(1,1) )

create table OtherCaseType
(CaseTypeID int,
 CaseType nvarchar(100))
 
 
create table StatuteInformation
(FileID bigint,
AccidentDate datetime,
AccDateAfter1yr datetime,
AccDateAfter2yr datetime,
ComplaintFileDate datetime,
ComplaintAfter60days datetime,
ComplaintAfter2yrs datetime,
ComplaintAfter3yrs datetime,
ComplaintAfter5yrs datetime,
IsGOvtClaim bit)

create table Users
(UserName nvarchar(100),
IsAdmin bit,
Password nvarchar(100))