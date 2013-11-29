using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class ClientDefendantInformation
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string BusinessPhone { get; set; }
        public DateTime DateOfBirth{ get; set; }
        public string DrivingLicense { get; set; }
        public DefendantAttorneyInformation AttorneyInfo{ get; set; }
    }

    internal class DefendantAttorneyInformation
    {
        public string Firm { get; set; }
        public string Attorney { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
    }
}
