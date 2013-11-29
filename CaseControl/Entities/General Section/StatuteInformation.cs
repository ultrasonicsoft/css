using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseControl
{
    internal class StatuteInformation
    {
        public string AccidentDate { get; set; }
        public string AccDateAfter1yr { get; set; }
        public string AccDateAfter2yr { get; set; }
        public string ComplaintFileDate { get; set; }
        public string ComplaintAfter60days { get; set; }
        public string ComplaintAfter2yrs { get; set; }
        public string ComplaintAfter3yrs { get; set; }
        public string ComplaintAfter5yrs { get; set; }
        public bool IsGovtDClaim { get; set; }
        public GovertmentClaimInformation CityClaim { get; set; }
        public GovertmentClaimInformation CountyClaim { get; set; }
        public GovertmentClaimInformation StateClaim { get; set; }
        public GovertmentClaimInformation OtherClaim { get; set; }
    }

    internal class GovertmentClaimInformation
    {
        public string DeniedDate { get; set; }
        public string ClaimDueDate { get; set; }
        public string FiledDate { get; set; }
        public string FiledDateAfter60Days { get; set; }
        public string FiledDateAfter2yrs { get; set; }
        public string FiledDateAfter3yrs { get; set; }
        public string FiledDateAfter5yrs { get; set; }
    }
}
