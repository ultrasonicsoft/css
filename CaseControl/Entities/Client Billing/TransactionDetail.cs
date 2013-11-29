using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class TransactionDetail
    {
        public string FileID { get; set; }
        public string TransactionID { get; set; }
        public string TransactionDate { get; set; }
        public string Description { get; set; }
        public string BillingType { get; set; }
        public string GeneralFund { get; set; }
        public string TrustFund { get; set; }
        public string CheckNo { get; set; }
    }
}
