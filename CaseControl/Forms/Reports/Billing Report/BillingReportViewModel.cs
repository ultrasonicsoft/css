using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    public class BillingReportViewModel : BaseViewModel
    {
        public BillingReportViewModel() { }

        public BillingReportViewModel(string name, string address, bool happy)
        {
           
        }

        private string itemNo;
        private string date;
        private string description;
        private string billingType;
        private string generalAccountFunds;
        private string trustAccountFunds;
        private string checkNo;

        public string ItemNo
        {
            get { return itemNo; }
            set
            {
                SetValue(ref itemNo, value, "ItemNo");
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

        public string Description
        {
            get { return description; }
            set
            {
                SetValue(ref description, value, "Description");
            }
        }

        public string BillingType
        {
            get { return billingType; }
            set
            {
                SetValue(ref billingType, value, "BillingType");
            }
        }

        public string GeneralAccountFunds
        {
            get { return generalAccountFunds; }
            set
            {
                SetValue(ref generalAccountFunds, value, "GeneralAccountFunds");
            }
        }

        public string TrustAccountFunds
        {
            get { return trustAccountFunds; }
            set
            {
                SetValue(ref trustAccountFunds, value, "TrustAccountFunds");
            }
        }

        public string CheckNo
        {
            get { return checkNo; }
            set
            {
                SetValue(ref checkNo, value, "CheckNo");
            }
        }
    }
}

