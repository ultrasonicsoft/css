using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class ClientGeneralInformation
    {
        public string FileNo { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CaseType { get; set; }
        public string OtherType { get; set; }
        public string CaseStatus { get; set; }
        public DateTime AccidentDate { get; set; }
        public string ClientCreatedOn { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string DrivingLicense { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string SSN { get; set; }

        public string InitialCaseInformation { get; set; }
        public string DefendantName { get; set; }

        public List<string> Evidences { get; set; }
        public string OriginatingAttorney { get; set; }
        public string AssignedAttorney { get; set; }
        public string Referral { get; set; }

        public string Email { get; set; }
        public string SuiteAddress { get; set; }
        public string WorkPhone { get; set; }
    }

}
