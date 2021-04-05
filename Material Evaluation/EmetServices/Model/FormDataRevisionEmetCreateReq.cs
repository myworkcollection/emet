using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model
{
    public class FormDataRevisionEmetCreateReq
    {
        public string QuoteNo { get; set; }
        public string Vendor { get; set; }
        public string VendorName { get; set; }
        public string MaterialType { get; set; }
        public string MaterialClass { get; set; }
        public string Material { get; set; }
        public string MaterialDesc { get; set; }
        public string ProcessGroup { get; set; }
        public string PrcGrpDesc { get; set; }
        public string MQty { get; set; }
        public string BaseUOM { get; set; }
        public string UOM { get; set; }
        public string PlantStatus { get; set; }
        public string SAPProcType { get; set; }
        public string SAPSpProcType { get; set; }
        public string Product { get; set; }
        public string PIRType { get; set; }
        public string PIRJobType { get; set; }
        public string NetUnit { get; set; }
        public bool IsMatcostAllow { get; set; }
        public bool IsProccostAllow { get; set; }
        public bool IsSubMatcostAllow { get; set; }
        public bool IsOthcostAllow { get; set; }
        public string IsUseToolAmor { get; set; }
        public string IsUseMachineAmor { get; set; }

        public string ReqPurpose { get; set; }        
        public string Remark { get; set; }
        public DateTime ResDueDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime DueDateNextRev { get; set; }
        public string RecycleRatio { get; set; }
        public string FileName { get; set; }
    }
}