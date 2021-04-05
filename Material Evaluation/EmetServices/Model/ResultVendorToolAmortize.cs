using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model
{
    public class ResultVendorToolAmortize
    {
        public bool success { get; set; }
        public string message { get; set; }
        public EnumerableRowCollection VendorToolAmortize { get; set; }
    }

    public class EnumerableRowCollection_VendorToolAmortize<VendorToolAmortize>
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string SearchTerm { get; set; }
        public string ToolTypeID { get; set; }
        public string Amortize_Tool_ID { get; set; }
        public string Amortize_Tool_Desc { get; set; }
        public decimal AmortizeCost { get; set; }
        public string AmortizeCurrency { get; set; }
        public string ExchangeRate { get; set; }
        public string AmortizePeriod { get; set; }
        public string AmortizePeriodUOM { get; set; }
        public string TotalAmortizeQty { get; set; }
        public string QtyUOM { get; set; }
        public decimal AmortizeCost_Vend_Curr { get; set; }
        public string AmortizeCost_Pc_Vend_Curr { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime DueDate { get; set; }
    }
}