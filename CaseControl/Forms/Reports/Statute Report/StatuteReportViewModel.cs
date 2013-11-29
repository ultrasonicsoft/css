using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    public class StatuteReportViewModel : BaseViewModel
    {
        public StatuteReportViewModel() { }

        private string name;
        private string fileNo;
        private string date;
        private string attorney;

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

        public string Date
        {
            get { return date; }
            set
            {
                SetValue(ref date, value, "Date");
            }
        }

        public string Attorney
        {
            get { return attorney; }
            set
            {
                SetValue(ref attorney, value, "Attorney");
            }
        }
    }
}

