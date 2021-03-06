using Material_Evaluation.EmetServices.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Material_Evaluation.EmetServices.SMNReport
{
    /// <summary>
    /// Summary description for MyJson
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MyJson : System.Web.Services.WebService
    {
        string DbMasterName = "";
        SqlCommand cmd = new SqlCommand();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }
        

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void LoadDataReport(string Plant,string Status, string SMNStatus, string ReqType, string ReqStatus, string FltrDate, string From, string To, string FilterBy, string FilterValue
            , string ExtraFilter, string VendorCode)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            DataTable DtResult = new DataTable();
            DataTable DtTMCCostDetails = new DataTable();
            DataTable DtTProcessCostDetails = new DataTable();
            DataTable DtTSMCCostDetails = new DataTable();
            DataTable DtTOtherCostDetails = new DataTable();

            ResultSMNReport MyResult = new ResultSMNReport();
            try
            {
                GetDbMaster();
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                string sql = "";
                #region Main Quory
                sql = @" IF OBJECT_ID('tempdb..##tempTQuoteDetails') IS NOT NULL DROP TABLE ##tempTQuoteDetails
                        IF OBJECT_ID('tempdb..##tempTMCCostDetails') IS NOT NULL DROP TABLE ##tempTMCCostDetails
                        IF OBJECT_ID('tempdb..##tempTProcessCostDetails') IS NOT NULL DROP TABLE ##tempTProcessCostDetails
                        IF OBJECT_ID('tempdb..##tempTSMCCostDetails') IS NOT NULL DROP TABLE ##tempTSMCCostDetails
                        IF OBJECT_ID('tempdb..##tempTOtherCostDetails') IS NOT NULL DROP TABLE ##tempTOtherCostDetails

                        select distinct 
                        tq.RequestNumber as [Request_Number],
                        tq.RequestDate,
                        tq.QuoteResponseDueDate,
                        tq.QuoteNo as [Quote_No],
                        tq.Plant as [Plant],
                        ----
                        (select LTRIM((stuff((
                           SELECT ', '+  CONVERT(nvarchar,RP.Plant)
                           FROM TPlantReq RP 
                           WHERE RP.RequestNumber = tq.RequestNumber
                           FOR XML PATH('')),
                           Count('ID')
                        , 1, ' ')))) as [GP_Request_Plant],
                        ----
                        case 
                        when tq.QuoteNoRef is null and ((SELECT RIGHT(tq.QuoteNo, 1)) = 'D') then 'Without SAP Code'
                        when tq.QuoteNoRef is null and ((SELECT RIGHT(tq.QuoteNo, 2)) = 'GP') then 'Without SAP Code GP'
                        when tq.QuoteNoRef is null and (tq.isMassRevision = 0 or tq.isMassRevision is null) then 'New' 
                        when tq.QuoteNoRef is null and (tq.isMassRevision = 1 ) then 'Mass Revision' 
                        when tq.QuoteNoRef is not null then 'Revision' 
                        end as [Req_Type], 

                        ----
                        case
                        WHEN 
	                        (2 in (select ApprovalStatus from TQuoteDetails where RequestNumber = tq.RequestNumber)) 
                        THEN 'IN PROGRESS'
                        WHEN 
	                        (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = tq.RequestNumber)) and (5 in (select ApprovalStatus from TQuoteDetails where RequestNumber = tq.RequestNumber))
                        THEN 'IN PROGRESS'
                        WHEN 
	                        (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = tq.RequestNumber))
                        THEN 'OPEN'
                        WHEN (tq.ApprovalStatus = 0) 
                        then 'OPEN'
                        WHEN (tq.ApprovalStatus = 3) or (tq.ApprovalStatus = 1) or (tq.ApprovalStatus = 5) or (tq.ApprovalStatus = 6) then 'CLOSED' 
                        end as [Req_Status],
                        ----

                        case 
                        when TQ.UpdatedBy = 'EMET' then 'Auto Completed By Shimano'
                        when ApprovalStatus = 0 then 'Vendor Pending'
                        when (ApprovalStatus = 1 and FinalQuotePrice = '' or ApprovalStatus = 1 and FinalQuotePrice is null) then 'Auto Completed By Shimano'
                        when (ApprovalStatus = 1 and FinalQuotePrice != '' or ApprovalStatus = 1 and FinalQuotePrice is not null) then 'Vendor Completed'
                        when (ApprovalStatus = 2 and FinalQuotePrice = '' or ApprovalStatus = 2 and FinalQuotePrice is null) then 'Auto Completed By Shimano'
                        when (ApprovalStatus = 2 and FinalQuotePrice != '' or ApprovalStatus = 2 and FinalQuotePrice is not null) then 'Vendor Completed'
                        when (ApprovalStatus = 3 and FinalQuotePrice = '' or ApprovalStatus = 3 and FinalQuotePrice is null) then 'Auto Completed By Shimano'
                        when (ApprovalStatus = 3 and FinalQuotePrice != '' or ApprovalStatus = 3 and FinalQuotePrice is not null) then 'Vendor Completed'
                        when ApprovalStatus = 4 then 'Vendor Pending'
                        when ApprovalStatus = 5 then 'Vendor Completed'
                        when ApprovalStatus = 6 then 'Auto Completed By Shimano'
                        else 'cannot find status'
                        end as [VndRes_Status],
                        ----

                        case 
                            when (TQ.ApprovalStatus = '0' and TQ.PICApprovalStatus = '0' ) then 'Waiting for Vendor Submission'
                            when (TQ.ApprovalStatus = '2' and TQ.PICApprovalStatus = '0' ) then 'Waiting for Manager Approval'

                            when (TQ.PICApprovalStatus = '2' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null ) then TQ.ManagerRemark
                            when (TQ.PICApprovalStatus = '1' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null and TQ.ManagerReason is null) then TQ.ManagerRemark
                            when (TQ.PICApprovalStatus = '1' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null and TQ.ManagerReason is not null) then TQ.ManagerReason
                            when (TQ.ManagerApprovalStatus = '2' and DIRApprovalStatus= '0') then TQ.DIRRemark
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0'and TQ.DIRReason is null) then TQ.DIRRemark
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0'and TQ.DIRReason is not null) then TQ.DIRReason

                            when (TQ.ApprovalStatus = '4' and TQ.PICApprovalStatus = '4' and TQ.ManagerApprovalStatus = '4' and DIRApprovalStatus= '4') then '-'
                            when (TQ.ApprovalStatus = '5' and TQ.PICApprovalStatus = '5' and TQ.ManagerApprovalStatus = '5' and DIRApprovalStatus= '5') then 'Without SAP Code No Need Approval'
                            else '-'
                        end as [SMNResStatus],
                        ----

                        SMNPicDept as [SMN_PIC_Dept],
                        (select UPPER(UseNam) from  " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) as [SMN_PIC],
                        (select UseEmail from  " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) as [SMN_PIC_Email],

                        tq.Product,
                        tq.MaterialType as [Material_Type],
                        tq.MaterialClass as [Material_Class],
                        tq.Material,tq.MaterialDesc as [Material_Desc],
                        tq.BaseUOM as [Base_UOM],
                        tq.PlantStatus as [Plant_Status],
                        tq.DrawingNo as [Drawing_No],
                        tq.SAPProcType as [SAP_Proc_Type],
                        tq.SAPSpProcType as [SAP_Sp_Proc_Type],
                        tq.PIRType as [PIR_Type],
                        tq.PIRJobType as [PIR_Job_Type],
                        tq.PlatingType as [Plating_Type],
                        tq.ProcessGroup as [Process_Group],
                        TQ.IMRecycleRatio as [Req_Recycle_Ratio],
                        (case when TQ.PICReason is not null then TQ.PICReason else ERemarks end ) as [Request_Purpose],
                        tq.MQty as [MnthEstQty],
                        tq.BaseUOM as [MnthEstQty_UOM],
                        tq.FADate as [FA_Date],
                        tq.FAQty as [FA_Qty],
                        tq.DelDate as [Delivery_Date],
                        tq.DelQty as [Delivery_Qty],
                        tq.EffectiveDate as [Effective_Date],
                        case when pirstatus is null or pirstatus = '' then NULL else tq.DueOn end  as [Due_Dt_Next_Rev],
                        (select top 1 NewEffectiveDate from TMngEffDateChgLog MEC where MEC.QuoteNo = tq.QuoteNo order by MEC.NewEffectiveDate desc) as [New_Effective_Date],
                        (select top 1 NewDueOn from TMngEffDateChgLog MEC where MEC.QuoteNo = tq.QuoteNo order by MEC.NewDueOn desc) as [New_Due_Dt_Next_Rev],
                        tq.QuoteNoRef as [Previous_Quote_No],
                        tq.Incoterm,
                        tq.PckReqrmnt as [Packing_Req],
                        tq.OthReqrmnt as [Others_Req],
                         tq.VendorCode1 as [Vendor_Code],
                        tq.VendorName as [Vendor_Name],
                        (select distinct tv.cty from " + DbMasterName + @".[dbo].tVendor_New tv inner join " + DbMasterName + @".[dbo].tVendorPOrg tvp on tv.POrg=tvp.POrg  where tv.vendor =tq.VendorCode1 and tvp.plant = tq.Plant) as [Vendor_Country_Code],
                        (select distinct tv.crcy from " + DbMasterName + @".[dbo].tVendor_New tv inner join " + DbMasterName + @".[dbo].tVendorPOrg tvp on tv.POrg=tvp.POrg  where tv.vendor =tq.VendorCode1 and tvp.plant = tq.Plant) as [Vendor_Currency],
                        (select distinct UPPER(PICName) from  " + DbMasterName + @".[dbo].TVENDORPIC where plant=TQ.plant and VendorCode=TQ.VendorCode1) as [Vendor_PIC],
                        (select distinct PICEmail from  " + DbMasterName + @".[dbo].TVENDORPIC where plant=TQ.plant and VendorCode=TQ.VendorCode1) as [Vendor_PIC_Email],
                        tq.CountryOrg as [Country_Org],";

                if (VendorCode.Trim() == "")
                {
                    sql += @" case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.TotalMaterialCost,5) AS DECIMAL(12,5)) 
                        end as [Total_Material_Cost_pc],

                        case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.TotalProcessCost,5) AS DECIMAL(12,5))
                        end as [Total_Process_Cost_pc],

                        case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.TotalSubMaterialCost,5) AS DECIMAL(12,5))
                        end as [Total_Sub_Material_Cost_pc],

                        case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.TotalOtheritemsCost,5) AS DECIMAL(12,5))
                        end  as [Total_Other_items_Cost_pc],

                        case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.GrandTotalCost,5) AS DECIMAL(12,5))
                        end as [Grand_Total_Cost__pc],

                        case 
                        when tq.pirstatus is null or tq.pirstatus = '' then NULL
                        else CAST(ROUND(tq.FinalQuotePrice,5) AS DECIMAL(12,5)) 
                        end  as [Final_Quote_Price_pc],";
                }
                else {
                    sql += @" CAST(ROUND(tq.TotalMaterialCost,5) AS DECIMAL(12,5))  as [Total_Material_Cost_pc],

                        CAST(ROUND(tq.TotalProcessCost,5) AS DECIMAL(12,5)) as [Total_Process_Cost_pc],

                        CAST(ROUND(tq.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as [Total_Sub_Material_Cost_pc],

                        CAST(ROUND(tq.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as [Total_Other_items_Cost_pc],

                        CAST(ROUND(tq.GrandTotalCost,5) AS DECIMAL(12,5)) as [Grand_Total_Cost__pc],

                        CAST(ROUND(tq.FinalQuotePrice,5) AS DECIMAL(12,5)) as [Final_Quote_Price_pc],";
                }

                sql += @" TQ.NetProfDisc as [Net_ProfitDiscount],
                        TQ.GA as [GA],

                        case when pirstatus is null or pirstatus = '' then NULL else tq.Profit end as [profit],
                        case when pirstatus is null or pirstatus = '' then NULL else tq.Discount end as [discount],
                        
                        tq.CommentByVendor as [Comment_By_Vendor],
                        
                        case 
                        when IsReSubmit = 1 then 'Request to Resubmit'
                        when ApprovalStatus = 0 and PICApprovalStatus = 0 and ManagerApprovalStatus is null then 'Waiting Vend. Submission'
                        when ApprovalStatus = 0 and PICApprovalStatus is null and ManagerApprovalStatus is null then 'Waiting Vend. Submission'
                        when ApprovalStatus = 2 and PICApprovalStatus = 0 and ManagerApprovalStatus is null then 'Pending'
                        when PICApprovalStatus = 2 and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 2) then 'Approved' 
                        when PICApprovalStatus = 1 and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 2) then 'Rejected'
                        when PICApprovalStatus is null and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 2) then 'Rejected'
                        when ManagerApprovalStatus = 4 and DIRApprovalStatus = 4 then 'No Need Approval'
                        when ManagerApprovalStatus = 5 and DIRApprovalStatus = 5 then 'No Need Approval'
                        else 'cannot find status'
                        end as [Mgr_Decision],

                        case 
                        when (ManagerReason is null and ManagerRemark is null) then ''
                        when ManagerReason is NULL then 'Remark : '+ ManagerRemark
                        else 'Reason : ' + ManagerReason end as [Mgr_Comment],

                        (select distinct US.UseNam from " + DbMasterName + @".dbo.Usr US where US.UseID = AprRejByMng) as [Mgr_Name],
                        AprRejDateMng as [Mgr_AprRej_Date],

                        case 
                        when ApprovalStatus = 0 and PICApprovalStatus = 0 and ManagerApprovalStatus is null and DIRApprovalStatus is null then 'Pending'
                        when ApprovalStatus = 0 and PICApprovalStatus is null and ManagerApprovalStatus is null and DIRApprovalStatus is null then 'Pending'
                        when ApprovalStatus = 2 and PICApprovalStatus = 0 and ManagerApprovalStatus is null and DIRApprovalStatus is null then 'Pending'
                        when PICApprovalStatus = 2 and ManagerApprovalStatus = 0 and DIRApprovalStatus is null then 'Pending' 
                        when PICApprovalStatus = 1 and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1) and DIRApprovalStatus is null  then 'Pending'
                        when ManagerApprovalStatus = 2 and DIRApprovalStatus = 0 then 'Approved'
                        when ManagerApprovalStatus = 1 and (DIRApprovalStatus = 0 or DIRApprovalStatus = 1) then 'Rejected'
                        when ManagerApprovalStatus = 4 and DIRApprovalStatus = 4 then 'No Need Approval'
                        when ManagerApprovalStatus = 5 and DIRApprovalStatus = 5 then 'No Need Approval'
                        else 'cannot find status'
                        end as [DIR_Decision],

                        (select distinct US.UseNam from " + DbMasterName + @".dbo.Usr US where US.UseID = AprRejBy) as [DIR_Name],
                        AprRejDate as [DIRAprRejDate],

                        case 
                        when (DIRReason is null and DIRRemark is null) then ''
                        when DIRReason is NULL then 'Remark : '+ DIRRemark
                        else 'Reason : ' + DIRReason end as [DIR_Comment]

                        into ##tempTQuoteDetails
                        from TQuoteDetails tq
                        where (tq.CreateStatus <> '' or tq.CreateStatus is not null)
                        and (Plant  = @Plant) 
                            ";
                if (VendorCode != "") {
                    sql += @" and tq.VendorCode1 = @VendorCode";
                }
                #endregion

                #region vend res status
                if (Status == "Pending")
                {
                    sql += @" and (TQ.ApprovalStatus = 0 or TQ.ApprovalStatus = 4) ";
                }
                else if (Status == "Completed")
                {
                    sql += @" and (TQ.ApprovalStatus = 1 or TQ.ApprovalStatus = 2 or TQ.ApprovalStatus = 3 or TQ.ApprovalStatus = 5) ";
                    sql += @" and (TQ.FinalQuotePrice <> '' or TQ.FinalQuotePrice is not null) ";
                }
                else if (Status == "Auto")
                {
                    sql += @" and ( (TQ.ApprovalStatus = 1 and TQ.FinalQuotePrice = '' or TQ.ApprovalStatus = 1 and TQ.FinalQuotePrice is null) or (TQ.ApprovalStatus = 6) )";
                }
                #endregion

                #region SMN res status
                if (SMNStatus == "Waiting")
                {
                    sql += @" and ( (TQ.ApprovalStatus = 0 and TQ.PICApprovalStatus = 0 and TQ.ManagerApprovalStatus is null and TQ.DIRApprovalStatus is null) or 
                                        (TQ.ApprovalStatus = 0 and TQ.PICApprovalStatus is null and TQ.ManagerApprovalStatus is null and TQ.DIRApprovalStatus is null) or
                                        (TQ.ApprovalStatus = 4)  
                                      ) ";
                }
                else if (SMNStatus == "MPending")
                {
                    sql += @" and (TQ.ApprovalStatus = 2 and TQ.PICApprovalStatus = 0 and TQ.ManagerApprovalStatus is null and TQ.DIRApprovalStatus is null) ";
                }
                else if (SMNStatus == "MResubmit")
                {
                    sql += @" and ( (TQ.ApprovalStatus = 0 and TQ.PICApprovalStatus = 0 and TQ.ManagerApprovalStatus is null and TQ.DIRApprovalStatus is null) or 
                                        (TQ.ApprovalStatus = 0 and TQ.PICApprovalStatus is null and TQ.ManagerApprovalStatus is null and TQ.DIRApprovalStatus is null) or
                                        (TQ.ApprovalStatus = 4)) and TQ.IsReSubmit = 1 ";
                }
                else if (SMNStatus == "MApproved")
                {
                    sql += @" and ( PICApprovalStatus = 2 and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 2) ) ";
                }
                else if (SMNStatus == "MRejected")
                {
                    sql += @" and ( PICApprovalStatus = 1 and (ManagerApprovalStatus = 0 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 2) ) ";
                }
                else if (SMNStatus == "DApproved")
                {
                    //sql += @" and ( ManagerApprovalStatus = 2 and DIRApprovalStatus = 0 ) ";
                    sql += @" and (ApprovalStatus = 3 and ManagerApprovalStatus = 2 and DIRApprovalStatus = 0) ";
                }
                else if (SMNStatus == "DRejected")
                {
                    //sql += @" and ( (ManagerApprovalStatus = 1 and DIRApprovalStatus = 0) or (ManagerApprovalStatus = 1 and DIRApprovalStatus = 1) ) ";
                    sql += @" and (ApprovalStatus = 1 and ManagerApprovalStatus = 1 and DIRApprovalStatus = 0) ";
                }
                #endregion SMN res status

                #region Req_Type
                if (ReqType == "WithSAPCode")
                {
                    sql += @" and (TQ.isUseSAPCode = 1) and (TQ.QuoteNoRef is null) and (TQ.isMassRevision = 0 or TQ.isMassRevision is null)";
                }
                else if (ReqType == "WithSAPCodeRevision")
                {
                    sql += @" and (TQ.isUseSAPCode = 1) and TQ.QuoteNoRef is not null ";
                }
                else if (ReqType == "WithoutSAPCode")
                {
                    sql += @" and ((SELECT RIGHT(TQ.QuoteNo, 1)) = 'D') ";
                }
                else if (ReqType == "WithoutSAPCodeGP")
                {
                    sql += @" and ((SELECT RIGHT(TQ.QuoteNo, 2)) = 'GP') ";
                }
                else if (ReqType == "WithSAPCodeMassRevision")
                {
                    sql += @" and (TQ.isUseSAPCode = 1) and (TQ.QuoteNoRef is null) and (TQ.isMassRevision = 1) ";
                }
                #endregion

                #region Req_Status
                if (ReqStatus == "InProgress")
                {
                    sql += @" and ( 
                                        (2 in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber))   
                                        or (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber)) and (5 in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber)) 
                                      ) ";
                }
                else if (ReqStatus == "Closed")
                {
                    sql += @"and ( (TQ.ApprovalStatus = 3) or (TQ.ApprovalStatus = 1) or (TQ.ApprovalStatus = 5) or (TQ.ApprovalStatus = 6) ) 
                                        and (4 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber))";
                }
                else if (ReqStatus == "Open")
                {
                    sql += @" and ( 
                             ( (TQ.ApprovalStatus = 0) and  (5 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber) ) and ((select count(*)  from TQuoteDetails where RequestNumber = TQ.RequestNumber and (FinalQuotePrice is not null or FinalQuotePrice <> '')) = 0 ) )
                            or 
                              ( (TQ.ApprovalStatus = 4) and (5 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = TQ.RequestNumber) ) )
                            )";
                }
                #endregion

                #region From And To Date
                if (From != "" && To != "")
                {
                    if (FltrDate == "RequestDate")
                    {
                        sql += @" and format(TQ.RequestDate, 'yyyy-MM-dd') between @From and @To ";
                    }
                    else if (FltrDate == "QuoteResponseDueDate")
                    {
                        sql += @" and format(TQ.QuoteResponseDueDate, 'yyyy-MM-dd') between @From and @To ";
                    }
                }
                #endregion

                #region Filter By
                if (FilterValue != "")
                {
                    if (FilterBy == "Plant")
                    {
                        sql += @" and TQ.Plant like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "RequestNumber")
                    {
                        sql += @" and TQ.RequestNumber like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "Product")
                    {
                        sql += @" and TQ.Product like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "Material")
                    {
                        sql += @" and TQ.Material like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "MaterialDesc")
                    {
                        sql += @" and TQ.MaterialDesc like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "QuoteNo")
                    {
                        //sql += @" and TQ.QuoteNo like '%'+@Filter+'%' ";
                        sql += @" and TQ.QuoteNo = @Filter ";
                    }
                    else if (FilterBy == "VendorCode1")
                    {
                        sql += @" and TQ.VendorCode1 like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "VendorName")
                    {
                        sql += @" and TQ.VendorName like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "CreatedBy")
                    {
                        sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "UseDep")
                    {
                        sql += @" and (select UPPER(UseDep) from " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "ProcessGroup")
                    {
                        sql += @" and TQ.ProcessGroup like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "ProcessGroupDesc")
                    {
                        sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = TQ.ProcessGroup) like '%'+@Filter+'%' ";
                    }
                }
                #endregion

                #region Extra Filter
                if (ExtraFilter != "")
                {
                    string[] ArrExtraFilter = ExtraFilter.Split('|');
                    for (int ex = 0; ex < ArrExtraFilter.Count(); ex++)
                    {
                        string[] ExtraFilterDet = ArrExtraFilter[ex].ToString().Split(':');
                        if (ExtraFilterDet[0].ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = TQ.ProcessGroup) like '%'+ @ExtraFilter"+ex +" +'%' ";
                        }
                        else if (ExtraFilterDet[0].ToString() == "CreatedBy")
                        {
                            sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) like '%'+ @ExtraFilter" + ex + " +'%' ";
                        }
                        else if (ExtraFilterDet[0].ToString() == "UseDep")
                        {
                            sql += @" and SMNPicDept like '%'+ @ExtraFilter" + ex + " +'%' ";
                        }
                        else
                        {
                            sql += @" AND " + ExtraFilterDet[0] + " like '%'+ @ExtraFilter" + ex + " +'%' ";
                        }
                    }
                }
                #endregion

                sql += @" 
                        Order by TQ.RequestNumber desc 
                        ";

                #region mat Cost Data
                if (VendorCode.Trim() == "")
                {
                    sql += @" 
                            select tmc.QuoteNo,
                            case when pirstatus is null or pirstatus = '' then NULL else UPPER(tmc.MaterialSAPCode) end as [Raw_Material_SAP_Code],
                            case when pirstatus is null or pirstatus = '' then NULL else UPPER(tmc.MaterialDescription) end as [Raw_Material_Desc],
                            case when pirstatus is null or pirstatus = '' then NULL else UPPER(tmc.[RawMaterialCost/kg]) end as [Raw_Material_Cost],
                            case when pirstatus is null or pirstatus = '' then NULL else UPPER(tmc.RawMaterialCostUOM) end as [Raw_Material_Cost_UOM],
                            case when pirstatus is null or pirstatus = '' then NULL else tq2.ActualNU end as [Part_Net_Weight],
                            case when pirstatus is null or pirstatus = '' then NULL else tq2.UOM end as [Part_Net_Weight_UOM],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[PartNetUnitWeight(g)] end as [Part_Unit_Weight],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~~Thickness(mm)] end as [Thickness],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~~Width(mm)] end  as [Width],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~~Pitch(mm)] end  as [Pitch],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~MaterialDensity] end  as [Material_Density],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~RunnerWeight/shot(g)] end  as [Runner_Weightshot],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~RunnerRatio/pcs(%)] end as [Runner_Ratiopcs],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[~RecycleMaterialRatio(%)] end  as [Recycle_Material_Ratio],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.Cavity end as [Cavity],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[MaterialYield/MeltingLoss(%)] end as [MaterialMelting_Loss],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[MaterialGrossWeight/pc(g)] end as [Material_Gross_Weightpc],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[MaterialScrapWeight(g)] end as [Material_Scrap_Weight],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[ScrapLossAllowance(%)] end as [Scrap_Loss_Allowance],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[ScrapPrice/kg] end  as [Scrap_Pricekg],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[ScrapRebate/pcs] end  as [Scrap_Rebate_pcs],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[MaterialCost/pcs] end as [Material_Costpcs],
                            case when pirstatus is null or pirstatus = '' then NULL else tmc.[TotalMaterialCost/pcs] end as [Total_Material_Costpcs]
                            into ##tempTMCCostDetails
                            from TQuoteDetails tq2
                            join TMCCostDetails tmc on tq2.QuoteNo= tmc.QuoteNo
                            where TQ2.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp ) ";
                }
                else {
                    sql += @" 
                            select tmc.QuoteNo,
                            UPPER(tmc.MaterialSAPCode)  as [Raw_Material_SAP_Code],
                            UPPER(tmc.MaterialDescription)  as [Raw_Material_Desc],
                            UPPER(tmc.[RawMaterialCost/kg])  as [Raw_Material_Cost],
                            UPPER(tmc.RawMaterialCostUOM)  as [Raw_Material_Cost_UOM],
                            tq2.ActualNU  as [Part_Net_Weight],
                            tq2.UOM  as [Part_Net_Weight_UOM],
                            tmc.[PartNetUnitWeight(g)]  as [Part_Unit_Weight],
                            tmc.[~~Thickness(mm)]  as [Thickness],
                            tmc.[~~Width(mm)]   as [Width],
                            tmc.[~~Pitch(mm)]   as [Pitch],
                            tmc.[~MaterialDensity]   as [Material_Density],
                            tmc.[~RunnerWeight/shot(g)]   as [Runner_Weightshot],
                            tmc.[~RunnerRatio/pcs(%)]  as [Runner_Ratiopcs],
                            tmc.[~RecycleMaterialRatio(%)]   as [Recycle_Material_Ratio],
                            tmc.Cavity  as [Cavity],
                            tmc.[MaterialYield/MeltingLoss(%)]  as [MaterialMelting_Loss],
                            tmc.[MaterialGrossWeight/pc(g)]  as [Material_Gross_Weightpc],
                            tmc.[MaterialScrapWeight(g)]  as [Material_Scrap_Weight],
                            tmc.[ScrapLossAllowance(%)]  as [Scrap_Loss_Allowance],
                            tmc.[ScrapPrice/kg]   as [Scrap_Pricekg],
                            tmc.[ScrapRebate/pcs]   as [Scrap_Rebate_pcs],
                            tmc.[MaterialCost/pcs]  as [Material_Costpcs],
                            tmc.[TotalMaterialCost/pcs]  as [Total_Material_Costpcs]
                            into ##tempTMCCostDetails
                            from TQuoteDetails tq2
                            join TMCCostDetails tmc on tq2.QuoteNo= tmc.QuoteNo
                            where TQ2.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp ) ";
                }
                
                #endregion

                #region proc Cost Data
                if (VendorCode.Trim() == "")
                {
                    sql += @" 
                            select tpr.QuoteNo,
                            case when pirstatus is null or pirstatus = '' then NULL when tpr.ProcessGrpCode = 'Select' or tpr.ProcessGrpCode = '--Select--' then NULL else tpr.ProcessGrpCode end as [Process_Grp_Code],
                            case when pirstatus is null or pirstatus = '' then NULL when tpr.SubProcess = 'Select' or tpr.SubProcess = '--Select--' then NULL else tpr.SubProcess end as [Sub_Process],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[IfTurnkey-VendorName] end as [If_Subcon_Subcon_Name],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.TurnKeySubVnd end as [If_TurnkeySub_vendor_name],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[Machine/Labor] end as [Machine_Labor],
                            case when pirstatus is null or pirstatus = '' then NULL when tpr.Machine = 'Select' or tpr.Machine = '--Select--' then NULL else tpr.Machine end as [Machine],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[StandardRate/HR] end as [Standard_RateHR],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.VendorRate end as [Vendor_Rate_HR],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.ProcessUOM end as [Process_UOM],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.Baseqty end as [Base_Qty],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[DurationperProcessUOM(Sec)] end as [Duration_per_Process_UOM],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[Efficiency/ProcessYield(%)]  end as [Efficiency],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.TurnKeyCost end as [Turnkey_Costpc],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.TurnKeyProfit end as [Turnkey_Fees],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[ProcessCost/pc] end  as [Process_Costpc],
                            case when pirstatus is null or pirstatus = '' then NULL else tpr.[TotalProcessesCost/pcs] end as [Total_Processes_Costpcs] 
                            into ##tempTProcessCostDetails
                            from TQuoteDetails tq3
                            join TProcessCostDetails tpr  on tpr.QuoteNo= Tq3.QuoteNo
                            where tpr.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )  ";
                }
                else {
                    sql += @" 
                            select tpr.QuoteNo,
                            case  when tpr.ProcessGrpCode = 'Select' or tpr.ProcessGrpCode = '--Select--' then NULL else tpr.ProcessGrpCode end as [Process_Grp_Code],
                            case  when tpr.SubProcess = 'Select' or tpr.SubProcess = '--Select--' then NULL else tpr.SubProcess end as [Sub_Process],
                            tpr.[IfTurnkey-VendorName]  as [If_Subcon_Subcon_Name],
                            tpr.TurnKeySubVnd  as [If_TurnkeySub_vendor_name],
                            tpr.[Machine/Labor]  as [Machine_Labor],
                            case  when tpr.Machine = 'Select' or tpr.Machine = '--Select--' then NULL else tpr.Machine end as [Machine],
                            tpr.[StandardRate/HR]  as [Standard_RateHR],
                            tpr.VendorRate  as [Vendor_Rate_HR],
                            tpr.ProcessUOM  as [Process_UOM],
                            tpr.Baseqty  as [Base_Qty],
                            tpr.[DurationperProcessUOM(Sec)]  as [Duration_per_Process_UOM],
                            tpr.[Efficiency/ProcessYield(%)]   as [Efficiency],
                            tpr.TurnKeyCost  as [Turnkey_Costpc],
                            tpr.TurnKeyProfit  as [Turnkey_Fees],
                            tpr.[ProcessCost/pc]   as [Process_Costpc],
                            tpr.[TotalProcessesCost/pcs]  as [Total_Processes_Costpcs] 
                            into ##tempTProcessCostDetails
                            from TQuoteDetails tq3
                            join TProcessCostDetails tpr  on tpr.QuoteNo= Tq3.QuoteNo
                            where tpr.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )  ";
                }
                
                #endregion

                #region Sub Mat Cost Data
                if (VendorCode.Trim() == "")
                {
                    sql += @" 
                        select tsm.QuoteNo,case when pirstatus is null or pirstatus = '' then NULL else UPPER(tsm.[Sub-Mat/T&JDescription]) end  as [SubMat_TJ_Description],
                        case when pirstatus is null or pirstatus = '' then NULL else tsm.[Sub-Mat/T&JCost] end as [SubMat_TJ_Cost],
                        case when pirstatus is null or pirstatus = '' then NULL else tsm.[Consumption(pcs)] end  as [Consumption],
                        case when pirstatus is null or pirstatus = '' then NULL else tsm.[Sub-Mat/T&JCost/pcs] end as [SubMat_TJ_Costpcs],
                        case when pirstatus is null or pirstatus = '' then NULL else tsm.[TotalSub-Mat/T&JCost/pcs] end as [Total_SubMat_TJ_Costpcs] 
                        into ##tempTSMCCostDetails
                        from TQuoteDetails tq4
                        join TSMCCostDetails tsm  on tsm.QuoteNo= tq4.QuoteNo
                        where TQ4.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )  ";
                }
                else {
                    sql += @" 
                        select tsm.QuoteNo,
                        UPPER(tsm.[Sub-Mat/T&JDescription]) as [SubMat_TJ_Description],
                        tsm.[Sub-Mat/T&JCost] as [SubMat_TJ_Cost],
                        tsm.[Consumption(pcs)] as [Consumption],
                        tsm.[Sub-Mat/T&JCost/pcs] as [SubMat_TJ_Costpcs],
                        tsm.[TotalSub-Mat/T&JCost/pcs] as [Total_SubMat_TJ_Costpcs] 
                        into ##tempTSMCCostDetails
                        from TQuoteDetails tq4
                        join TSMCCostDetails tsm  on tsm.QuoteNo= tq4.QuoteNo
                        where TQ4.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )  ";
                }
                
                #endregion

                #region Oth Cost Data
                if (VendorCode.Trim() == "")
                {
                    sql += @" 
                        select tot.QuoteNo,case when pirstatus is null or pirstatus = '' then NULL else UPPER(tot.ItemsDescription) end as [Items_Description],
                            case when pirstatus is null or pirstatus = '' then NULL else tot.[OtherItemCost/pcs] end as [Other_Item_Costpcs],
                            case when pirstatus is null or pirstatus = '' then NULL else tot.[TotalOtherItemCost/pcs] end as [Total_Other_Item_Costpcs] 
                            into ##tempTOtherCostDetails
                            from TQuoteDetails tq5
                            join TOtherCostDetails tot  on tot.QuoteNo= tq5.QuoteNo
                            where tot.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )   ";
                }
                else {
                    sql += @" 
                        select tot.QuoteNo,
                        UPPER(tot.ItemsDescription) as [Items_Description],
                        tot.[OtherItemCost/pcs] as [Other_Item_Costpcs],
                        tot.[TotalOtherItemCost/pcs] as [Total_Other_Item_Costpcs] 
                        into ##tempTOtherCostDetails
                        from TQuoteDetails tq5
                        join TOtherCostDetails tot  on tot.QuoteNo= tq5.QuoteNo
                        where tot.QuoteNo in (select distinct Temp.Quote_No from ##tempTQuoteDetails Temp )   ";
                }
                
                #endregion

                #region Execute Query
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@Plant", Plant);
                if (VendorCode != "")
                {
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                }
                if (FilterValue != "")
                {
                    cmd.Parameters.AddWithValue("@Filter", FilterValue);
                }
                if (From != "" && To != "")
                {
                    DateTime DtFrom = DateTime.ParseExact(From, "dd/MM/yyyy", null);
                    DateTime Dtto = DateTime.ParseExact(To, "dd/MM/yyyy", null);

                    cmd.Parameters.AddWithValue("@From", DtFrom.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@To", Dtto.ToString("yyyy-MM-dd"));
                }
                if (ExtraFilter != "") {
                    string[] ArrExtraFilter = ExtraFilter.Split('|');
                    for (int ex = 0; ex < ArrExtraFilter.Count(); ex++) {
                        string[] ExtraFilterDet = ArrExtraFilter[ex].ToString().Split(':');
                        cmd.Parameters.AddWithValue("@ExtraFilter"+ ex + "", ExtraFilterDet[1]);
                    }
                }
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                #endregion

                #region Set Up Data for QuoteDeatils
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from ##tempTQuoteDetails";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtResult);
                }
                #endregion

                #region Set Up Data For Mat Cost Data
                using (SqlDataAdapter sda = new SqlDataAdapter()) {
                    sql = @" select * from ##tempTMCCostDetails";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtTMCCostDetails);
                }
                #endregion

                #region Set up data For Process Cost
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from ##tempTProcessCostDetails";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtTProcessCostDetails);
                }
                #endregion

                #region Set up data For SubMat Cost
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from ##tempTSMCCostDetails";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtTSMCCostDetails);
                }
                #endregion

                #region Set up data For Others Cost
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from ##tempTOtherCostDetails";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtTOtherCostDetails);
                }
                #endregion

                sql = @"IF OBJECT_ID('tempdb..##tempTQuoteDetails') IS NOT NULL DROP TABLE ##tempTQuoteDetails
                        IF OBJECT_ID('tempdb..##tempTMCCostDetails') IS NOT NULL DROP TABLE ##tempTMCCostDetails
                        IF OBJECT_ID('tempdb..##tempTProcessCostDetails') IS NOT NULL DROP TABLE ##tempTProcessCostDetails
                        IF OBJECT_ID('tempdb..##tempTSMCCostDetails') IS NOT NULL DROP TABLE ##tempTSMCCostDetails
                        IF OBJECT_ID('tempdb..##tempTOtherCostDetails') IS NOT NULL DROP TABLE ##tempTOtherCostDetails ";
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                EmetTrans.Commit();

                #region sET UP DATA TO MODEL
                var dataQuoteDet = DtResult.AsEnumerable().Select(row => new
                {
                    Request_Number = row["Request_Number"],
                    RequestDate = row["RequestDate"],
                    QuoteResponseDueDate = row["QuoteResponseDueDate"],
                    Quote_No = row["Quote_No"],
                    Plant = row["Plant"],
                    GP_Request_Plant = row["GP_Request_Plant"],
                    Req_Type = row["Req_Type"],
                    Req_Status = row["Req_Status"],
                    VndRes_Status = row["VndRes_Status"],
                    SMNResStatus = row["SMNResStatus"],
                    SMN_PIC_Dept = row["SMN_PIC_Dept"],
                    SMN_PIC = row["SMN_PIC"],
                    SMN_PIC_Email = row["SMN_PIC_Email"],
                    Product = row["Product"],
                    Material_Type = row["Material_Type"],
                    Material_Class = row["Material_Class"],
                    Material = row["Material"],
                    Material_Desc = row["Material_Desc"],
                    Base_UOM = row["Base_UOM"],
                    Plant_Status = row["Plant_Status"],
                    Drawing_No = row["Drawing_No"],
                    SAP_Proc_Type = row["SAP_Proc_Type"],
                    SAP_Sp_Proc_Type = row["SAP_Sp_Proc_Type"],
                    PIR_Type = row["PIR_Type"],
                    PIR_Job_Type = row["PIR_Job_Type"],
                    Plating_Type = row["Plating_Type"],
                    Process_Group = row["Process_Group"],
                    Req_Recycle_Ratio = row["Req_Recycle_Ratio"],
                    Request_Purpose = row["Request_Purpose"],
                    MnthEstQty = row["MnthEstQty"],
                    MnthEstQty_UOM = row["MnthEstQty_UOM"],
                    FA_Date = row["FA_Date"],
                    FA_Qty = row["FA_Qty"],
                    Delivery_Date = row["Delivery_Date"],
                    Delivery_Qty = row["Delivery_Qty"],
                    Effective_Date = row["Effective_Date"],
                    Due_Dt_Next_Rev = row["Due_Dt_Next_Rev"],
                    New_Effective_Date = row["New_Effective_Date"],
                    New_Due_Dt_Next_Rev = row["New_Due_Dt_Next_Rev"],
                    Previous_Quote_No = row["Previous_Quote_No"],
                    Incoterm = row["Incoterm"],
                    Packing_Req = row["Packing_Req"],
                    Others_Req = row["Others_Req"],
                    Vendor_Code = row["Vendor_Code"],
                    Vendor_Name = row["Vendor_Name"],
                    Vendor_Country_Code = row["Vendor_Country_Code"],
                    Vendor_Currency = row["Vendor_Currency"],
                    Vendor_PIC = row["Vendor_PIC"],
                    Vendor_PIC_Email = row["Vendor_PIC_Email"],
                    Country_Org = row["Country_Org"],
                    Total_Material_Cost_pc = row["Total_Material_Cost_pc"],
                    Total_Process_Cost_pc = row["Total_Process_Cost_pc"],
                    Total_Sub_Material_Cost_pc = row["Total_Sub_Material_Cost_pc"],
                    Total_Other_items_Cost_pc = row["Total_Other_items_Cost_pc"],
                    Grand_Total_Cost__pc = row["Grand_Total_Cost__pc"],
                    Final_Quote_Price_pc = row["Final_Quote_Price_pc"],
                    Net_ProfitDiscount = row["Net_ProfitDiscount"],
                    GA = row["GA"],
                    profit = row["profit"],
                    discount = row["discount"],
                    Comment_By_Vendor = row["Comment_By_Vendor"],
                    Mgr_Decision = row["Mgr_Decision"],
                    Mgr_Comment = row["Mgr_Comment"],
                    Mgr_Name = row["Mgr_Name"],
                    Mgr_AprRej_Date = row["Mgr_AprRej_Date"],
                    DIR_Decision = row["DIR_Decision"],
                    DIRAprRejDate = row["DIRAprRejDate"],
                    DIR_Name = row["DIR_Name"],
                    DIR_Comment = row["DIR_Comment"]
                });
                MyResult.SMNReportDataQuoteDetail = dataQuoteDet;

                var DataMatCost = DtTMCCostDetails.AsEnumerable().Select(row => new
                {
                    QuoteNo = row["QuoteNo"],
                    Raw_Material_SAP_Code = row["Raw_Material_SAP_Code"],
                    Raw_Material_Desc = row["Raw_Material_Desc"],
                    Raw_Material_Cost = row["Raw_Material_Cost"],
                    Raw_Material_Cost_UOM = row["Raw_Material_Cost_UOM"],
                    Part_Net_Weight = row["Part_Net_Weight"],
                    Part_Net_Weight_UOM = row["Part_Net_Weight_UOM"],
                    Part_Unit_Weight = row["Part_Unit_Weight"],
                    Thickness = row["Thickness"],
                    Width = row["Width"],
                    Pitch = row["Pitch"],
                    Material_Density = row["Material_Density"],
                    Runner_Weightshot = row["Runner_Weightshot"],
                    Runner_Ratiopcs = row["Runner_Ratiopcs"],
                    Recycle_Material_Ratio = row["Recycle_Material_Ratio"],
                    Cavity = row["Cavity"],
                    MaterialMelting_Loss = row["MaterialMelting_Loss"],
                    Material_Gross_Weightpc = row["Material_Gross_Weightpc"],
                    Material_Scrap_Weight = row["Material_Scrap_Weight"],
                    Scrap_Loss_Allowance = row["Scrap_Loss_Allowance"],
                    Scrap_Pricekg = row["Scrap_Pricekg"],
                    Scrap_Rebate_pcs = row["Scrap_Rebate_pcs"],
                    Material_Costpcs = row["Material_Costpcs"],
                    Total_Material_Costpcs = row["Total_Material_Costpcs"]
                });
                MyResult.SMNReportDataMatCost = DataMatCost;

                var dataprocCost = DtTProcessCostDetails.AsEnumerable().Select(row => new
                {
                    QuoteNo = row["QuoteNo"],
                    Process_Grp_Code = row["Process_Grp_Code"],
                    Sub_Process = row["Sub_Process"],
                    If_Subcon_Subcon_Name = row["If_Subcon_Subcon_Name"],
                    If_TurnkeySub_vendor_name = row["If_TurnkeySub_vendor_name"],
                    Machine_Labor = row["Machine_Labor"],
                    Machine = row["Machine"],
                    Standard_RateHR = row["Standard_RateHR"],
                    Vendor_Rate_HR = row["Vendor_Rate_HR"],
                    Process_UOM = row["Process_UOM"],
                    Base_Qty = row["Base_Qty"],
                    Duration_per_Process_UOM = row["Duration_per_Process_UOM"],
                    Efficiency = row["Efficiency"],
                    Turnkey_Costpc = row["Turnkey_Costpc"],
                    Turnkey_Fees = row["Turnkey_Fees"],
                    Process_Costpc = row["Process_Costpc"],
                    Total_Processes_Costpcs = row["Total_Processes_Costpcs"]
                });
                MyResult.SMNReportDataProcCost = dataprocCost;

                var dataSubMatCost = DtTSMCCostDetails.AsEnumerable().Select(row => new
                {
                    QuoteNo = row["QuoteNo"],
                    SubMat_TJ_Description = row["SubMat_TJ_Description"],
                    SubMat_TJ_Cost = row["SubMat_TJ_Cost"],
                    Consumption = row["Consumption"],
                    SubMat_TJ_Costpcs = row["SubMat_TJ_Costpcs"],
                    Total_SubMat_TJ_Costpcs = row["Total_SubMat_TJ_Costpcs"]
                });
                MyResult.SMNReportDataSubMatCost = dataSubMatCost;

                var dataOthersCost = DtTOtherCostDetails.AsEnumerable().Select(row => new
                {
                    QuoteNo = row["QuoteNo"],
                    Items_Description = row["Items_Description"],
                    Other_Item_Costpcs = row["Other_Item_Costpcs"],
                    Total_Other_Item_Costpcs = row["Total_Other_Item_Costpcs"]
                });
                MyResult.SMNReportDataOthCost = dataOthersCost;
                #endregion

                MyResult.success = true;
                MyResult.message = "Ok";
                
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
                EmetTrans.Rollback();
            }
            finally
            {
                EmetTrans.Dispose();
                EmetCon.Dispose();
            }
        }
        
    }
}
