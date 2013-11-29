using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    public class AllClientBillingReportViewModel : BaseViewModel
    {
        public AllClientBillingReportViewModel() { }

        private string name;
        private string fileNo;
        private string genAccountTotal;
        private string trustAccountTotal;
        private string assignedAttorney;

        public string Name
        {
            get { return name; }
            set
            {
                SetValue(ref name, value, "Name");
            }
        }

        public string FileNo
        {
            get { return fileNo; }
            set
            {
                SetValue(ref fileNo, value, "FileNo");
            }
        }

        public string GenAccountTotal
        {
            get { return genAccountTotal; }
            set
            {
                SetValue(ref genAccountTotal, value, "GenAccountTotal");
            }
        }

        public string TrustAccountTotal
        {
            get { return trustAccountTotal; }
            set
            {
                SetValue(ref trustAccountTotal, value, "TrustAccountTotal");
            }
        }

        public string AssignedAttorney
        {
            get { return assignedAttorney; }
            set
            {
                SetValue(ref assignedAttorney, value, "AssignedAttorney");
            }
        }
    }
}

