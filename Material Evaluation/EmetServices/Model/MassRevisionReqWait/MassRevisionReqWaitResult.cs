using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model.MassRevisionReqWait
{
    public class MassRevisionReqWaitResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection MainData { get; set; }
        public EnumerableRowCollection DataDetail { get; set; }
    }

    public class EnumerableRowCollection_MainData<MainData>
    {
        public string Plant { get; set; }
        public string RequestNumber { get; set; }
        public int NoQuote { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime QuoteResponseDueDate { get; set; }
        public string Product { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string CreatedBy { get; set; }
        public string UseDep { get; set; }

    }

    public class EnumerableRowCollection_AllRequestDataMain<DataDetail>
    {
        public string Plant { get; set; }
        public string RequestNumber { get; set; }
        public int NoQuote { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime QuoteResponseDueDate { get; set; }
        public string Product { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string CreatedBy { get; set; }
        public string UseDep { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string QuoteNo { get; set; }
    }
}