using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model
{
    public class Result_InvalidDataSelected
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection mainData { get; set; }
        public EnumerableRowCollection InvalidDataSelected { get; set; }
    }

    public class EnumerableRowCollection_InvalidDataSelected<InvalidDataSelected>
    {
        public string Plant { get; set; }
        public string RequestNumber { get; set; }
        public string RequestDate { get; set; }
        public string QuoteResponseDueDate { get; set; }
        public string QuoteNo { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode1 { get; set; }
        public string VendorName { get; set; }
    }

    public class EnumerableRowCollection_mainData<mainData>
    {
        public string Plant { get; set; }
        public string RequestNumber { get; set; }
        public string RequestDate { get; set; }
        public string QuoteResponseDueDate { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
    }
}