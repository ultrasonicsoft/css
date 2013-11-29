using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    public class CommonReportViewModel : BaseViewModel
    {
        public CommonReportViewModel() { }

        private string clientName;
        private string fileNo;
        private string byAttorney;
        private string accidentDate;
        private string caseNo;

        public string ClientName
        {
            get { return clientName; }
            set
            {
                SetValue(ref clientName, value, "ClientName");
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

        public string ByAttorney
        {
            get { return byAttorney; }
            set
            {
                SetValue(ref byAttorney, value, "ByAttorney");
            }
        }

        public string AccidentDate
        {
            get { return accidentDate; }
            set
            {
                SetValue(ref accidentDate, value, "AccidentDate");
            }
        }

        public string CaseNo
        {
            get { return caseNo; }
            set
            {
                SetValue(ref caseNo, value, "CaseNo");
            }
        }
    }
}

