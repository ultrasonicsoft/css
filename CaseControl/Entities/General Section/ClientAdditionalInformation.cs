using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class ClientAdditionalInformation
    {
        public string Occupation { get; set; }
        public string Employer { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public SpouseInformation SpouseInfo { get; set; }
    }

    internal class SpouseInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Occupation { get; set; }
        public string Employer { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
