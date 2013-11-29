using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class ClientAutoInformation
    {
        public string InsuranceCompany { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Adjuster { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public string MedPayAvailable { get; set; }
        public string Amout { get; set; }
        public string LiabilityMinimumCoverage { get; set; }
        public string LiabilityMaximumCoverage { get; set; }
        public string UMIMinimum { get; set; }
        public string UMIMaximum{ get; set; }
        public string Reimbursable { get; set; }
        public string Notes { get; set; }
    }
}
