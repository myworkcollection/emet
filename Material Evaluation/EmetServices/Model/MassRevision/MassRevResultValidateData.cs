using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model.MassRevision
{
    public class MassRevResultValidateData
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection ValidData { get; set; }
        public EnumerableRowCollection InValidData { get; set; }
    }

    public class EnumerableRowCollection_ValidData<ValidData>
    {
        public string Plant { get; set; }
        public string PIRNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessGroup { get; set; }

    }

    public class EnumerableRowCollection_InValidData<InValidData>
    {
        public string Plant { get; set; }
        public string PIRNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessGroup { get; set; }
        public string Remark { get; set; }

    }
}