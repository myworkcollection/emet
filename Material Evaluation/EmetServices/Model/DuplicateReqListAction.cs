using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.Model
{
    public class DuplicateReqListAction
    {
        public string RequestNumber { get; set; }
        public string QuoteNo { get; set; }
        public bool ActionRej { get; set; }
        public string NewResDueDate { get; set; }
    }
}