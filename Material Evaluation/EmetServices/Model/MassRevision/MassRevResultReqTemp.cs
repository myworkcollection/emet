using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model.MassRevision
{
    public class MassRevResultReqTemp
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection ValidDataMain { get; set; }
        public EnumerableRowCollection ValidDataMainComponent { get; set; }
        public EnumerableRowCollection InValidData { get; set; }
    }

    public class ER_ValidDataMain<ValidDataMain>
    {
        public string NewRequestNumber { get; set; }
        public string Plant { get; set; }
        public string PIRNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessGroup { get; set; }
        public string ProcDesc { get; set; }
        public string PIRJobType { get; set; }
        public string CodeRef { get; set; }
        public string UnitWeight { get; set; }
        public string UnitWeightUOM { get; set; }
        public string Plating { get; set; }
        public string MaterialType { get; set; }
        public string PlantStatus { get; set; }
        public string SAPProcType { get; set; }
        public string SAPSPProcType { get; set; }
        public string product { get; set; }
        public string MaterialClass { get; set; }
        public string PIRType { get; set; }
        public string amt1 { get; set; }
        public string amt2 { get; set; }
        public string amt3 { get; set; }
        public string amt4 { get; set; }
        public DateTime MassUpdateDate { get; set; }
        public string countryorg { get; set; }

    }

    public class ER_ValidDataMainComponent<ValidDataMainComponent>
    {
        public string NewRequestNumber { get; set; }
        public string Plant { get; set; }
        public string PIRNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessGroup { get; set; }
        public string ProcDesc { get; set; }
        public string PIRJobType { get; set; }
        public string CodeRef { get; set; }
        public string UnitWeight { get; set; }
        public string UnitWeightUOM { get; set; }
        public string Plating { get; set; }
        public string MaterialType { get; set; }
        public string PlantStatus { get; set; }
        public string SAPProcType { get; set; }
        public string SAPSPProcType { get; set; }
        public string product { get; set; }
        public string MaterialClass { get; set; }
        public string PIRType { get; set; }
        public string amt1 { get; set; }
        public string amt2 { get; set; }
        public string amt3 { get; set; }
        public string amt4 { get; set; }
        public DateTime MassUpdateDate { get; set; }
        public string countryorg { get; set; }

        public string CompMaterial { get; set; }
        public string CompMaterialDesc { get; set; }
        public string AmtSCur { get; set; }
        public string SellingCrcy { get; set; }
        public string AmtVCur { get; set; }
        public string VendorCrcy { get; set; }
        public string Unit { get; set; }
        public string UOM { get; set; }
        public DateTime CusMatValFrom { get; set; }
        public DateTime CusMatValTo { get; set; }
        public string ExchRate { get; set; }
        public DateTime ExchRateValidFrom { get; set; }
    }

    public class ER_InValidData<InValidData>
    {
        public string Plant { get; set; }
        public string PIRNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string ProcessGroup { get; set; }

    }
}