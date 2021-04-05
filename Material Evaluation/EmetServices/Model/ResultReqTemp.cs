using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model
{
    public class ResultReqTemp
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection MyDataTemp { get; set; }
    }

    public class EnumerableRowCollection_ResultReqTemp<MyDataTemp>
    {
        public string Plant { get; set; }
        public string ReqNo { get; set; }
        
        public string QuoteNo { get; set; }
        public string QuoteNoRef { get; set; }
        public string VendorCode1 { get; set; }
        public string VendorName { get; set; }
        public string SearchTerm { get; set; }
        public string VenPIC { get; set; }
        public string PICEmail { get; set; }
        public string SellCurrency { get; set; }
        public string AmtScur { get; set; }
        public string ExchangeRate { get; set; }
        public string VndCurrency { get; set; }
        public string AmtVcur { get; set; }
        public string Unit { get; set; }
        public string UOM { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime CusMatValFrom { get; set; }
        public DateTime CusMatValTo { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string ProcessGroup { get; set; }
    }
}