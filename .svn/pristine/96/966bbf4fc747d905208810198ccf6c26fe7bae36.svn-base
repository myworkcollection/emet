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

namespace Material_Evaluation.EmetServices.AllRequest
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
        public void LoadData(string Plant, string Status, string SMNStatus, string ReqType, string ReqStatus, string FltrDate, string From, string To, string FilterBy, string FilterValue
            , string VendorCode)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            DataTable DtMain = new DataTable();
            DataTable DtDetail = new DataTable();

            Model.AllRequest MyResult = new Model.AllRequest();
            try
            {
                GetDbMaster();
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                string sql = "";
                #region Query get Data
                sql = @" IF OBJECT_ID('tempdb..##temp1') IS NOT NULL DROP TABLE ##temp1
 
                        select distinct Plant,RequestNumber,(select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            RequestDate,QuoteResponseDueDate,Product,Material,MaterialDesc ,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep',
                            case 
                            when A.QuoteNoRef is null and ((SELECT RIGHT(A.QuoteNo, 1)) = 'D') then 'Without SAP Code'
                            when A.QuoteNoRef is null and ((SELECT RIGHT(A.QuoteNo, 2)) = 'GP') then 'Without SAP Code GP'
                            when A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null) then 'New' 
                            when A.QuoteNoRef is null and (A.isMassRevision = 1 ) then 'Mass Revision' 
                            when A.QuoteNoRef is not null then 'Revision' 
                            end as 'ReqType', 

                            case
                            WHEN 
	                            (2 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber)) 
                            THEN 'IN PROGRESS'
                            WHEN 
	                            (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber)) and (5 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber))
                            THEN 'IN PROGRESS'
							WHEN 
	                            (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber))
                            THEN 'OPEN'
                            
                            WHEN (a.ApprovalStatus = 0) then 'OPEN'
                            --when a.ApprovalStatus = 2  then 'IN PROGRESS'
                            WHEN (a.ApprovalStatus = 3) or (a.ApprovalStatus = 1) or (a.ApprovalStatus = 5) or (a.ApprovalStatus = 6) then 'CLOSED' 
                            end as 'ReqSts'

                            ,VendorCode1 as VendorCode,substring((VendorName),1,12) +'...' as VendorName,QuoteNo,";

                #region condition if from vendor page
                if (VendorCode.Trim() != "")
                {
                    sql += @" CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            CONVERT(nvarchar,
                            ROUND(
                            convert(float,
                            (
                            case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
                            else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
                            /convert(float,ISNULL(FinalQuotePrice,0))
                            )
                            *100)
                            ,1)
                            ) + '%'  as 'NetProfit_Discount', ";
                }
                else {
                    sql += @" case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) 
                            end as TotalMaterialCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5))
                            end as TotalProcessCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5))
                            end as TotalSubMaterialCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5))
                            end  as TotalOtheritemsCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5))
                            end as GrandTotalCost,


                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) 
                            end  as FinalQuotePrice,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CONVERT(nvarchar,
                            ROUND(
                            convert(float,
                            (
                            case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
                            else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
                            /convert(float,ISNULL(FinalQuotePrice,0))
                            )
                            *100)
                            ,1)
                            ) + '%'
                            end  as 'NetProfit_Discount', ";
                }
                #endregion
                
                sql += @"    case 
                            when A.UpdatedBy = 'EMET' then 'Auto Completed By Shimano'
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
                            end as 'ResponseStatus',
                            PICApprovalStatus,
                            ManagerApprovalStatus,DIRApprovalStatus,

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
                            end as 'MDecision',
                            
                            case 
                            when (ManagerReason is null and ManagerRemark is null) then ''
                            when ManagerReason is NULL then 'Remark : '+ ManagerRemark
                            else 'Reason : ' + ManagerReason end as 'MComment',

                            (select distinct US.UseNam from " + DbMasterName + @".dbo.Usr US where US.UseID = AprRejByMng) as Mname,
                            AprRejDateMng as MAprRejDt,

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
                            end as 'DDecision',

                            (select distinct US.UseNam from " + DbMasterName + @".dbo.Usr US where US.UseID = AprRejBy) as Dname,
                            AprRejDate as DAprRejDt,

                            case 
                            when (DIRReason is null and DIRRemark is null) then ''
                            when DIRReason is NULL then 'Remark : '+ DIRRemark
                            else 'Reason : ' + DIRReason end as 'DComment'

                            into ##temp1
                            from TQuoteDetails A where ((Plant  = @Plant) and (A.CreateStatus <> '' or A.CreateStatus is not null)) ";

                #region condition if from vendor page
                if (VendorCode.Trim() != "")
                {
                    sql += @" and (A.VendorCode1 = @VendorCode) ";
                }
                #endregion

                #region vend res status
                if (Status == "Pending")
                {
                    sql += @" and (ApprovalStatus = 0 or ApprovalStatus = 4) ";
                }
                else if (Status == "Completed")
                {
                    sql += @" and (ApprovalStatus = 1 or ApprovalStatus = 2 or ApprovalStatus = 3 or ApprovalStatus = 5) ";
                    sql += @" and (FinalQuotePrice <> '' or FinalQuotePrice is not null) ";
                }
                else if (Status == "Auto")
                {
                    sql += @" and ( (ApprovalStatus = 1 and FinalQuotePrice = '' or ApprovalStatus = 1 and FinalQuotePrice is null) or (ApprovalStatus = 6) )";
                }
                #endregion

                #region SMN res status
                if (SMNStatus == "Waiting")
                {
                    sql += @" and ( (ApprovalStatus = 0 and PICApprovalStatus = 0 and ManagerApprovalStatus is null and DIRApprovalStatus is null) or 
                                        (ApprovalStatus = 0 and PICApprovalStatus is null and ManagerApprovalStatus is null and DIRApprovalStatus is null) or
                                        (ApprovalStatus = 4)  
                                      ) ";
                }
                else if (SMNStatus == "MPending")
                {
                    sql += @" and (ApprovalStatus = 2 and PICApprovalStatus = 0 and ManagerApprovalStatus is null and DIRApprovalStatus is null) ";
                }
                else if (SMNStatus == "MResubmit")
                {
                    sql += @" and ( (ApprovalStatus = 0 and PICApprovalStatus = 0 and ManagerApprovalStatus is null and DIRApprovalStatus is null) or 
                                        (ApprovalStatus = 0 and PICApprovalStatus is null and ManagerApprovalStatus is null and DIRApprovalStatus is null) or
                                        (ApprovalStatus = 4)) and IsReSubmit = 1 ";
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

                #region Req Type
                if (ReqType == "WithSAPCode")
                {
                    sql += @" and (isUseSAPCode = 1) and (A.QuoteNoRef is null) and (A.isMassRevision = 0 or A.isMassRevision is null)";
                }
                else if (ReqType == "WithSAPCodeRevision")
                {
                    sql += @" and (isUseSAPCode = 1) and A.QuoteNoRef is not null ";
                }
                else if (ReqType == "WithoutSAPCode")
                {
                    sql += @" and ((SELECT RIGHT(QuoteNo, 1)) = 'D') ";
                }
                else if (ReqType == "WithoutSAPCodeGP")
                {
                    sql += @" and ((SELECT RIGHT(QuoteNo, 2)) = 'GP') ";
                }
                else if (ReqType == "WithSAPCodeMassRevision")
                {
                    sql += @" and (isUseSAPCode = 1) and (A.QuoteNoRef is null) and (A.isMassRevision = 1) ";
                }
                #endregion

                #region Req Status
                if (ReqStatus == "InProgress")
                {
                    sql += @" and ( 
                                        (2 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber))   
                                        or (4 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber)) and (5 in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber)) 
                                      ) ";
                }
                else if (ReqStatus == "Closed")
                {
                    sql += @"and ( (a.ApprovalStatus = 3) or (a.ApprovalStatus = 1) or (a.ApprovalStatus = 5) or (a.ApprovalStatus = 6) ) 
                                        and (4 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber))";
                }
                else if (ReqStatus == "Open")
                {
                    sql += @" and ( 
                             ( (a.ApprovalStatus = 0) and  (5 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber) ) and ((select count(*)  from TQuoteDetails where RequestNumber = A.RequestNumber and (FinalQuotePrice is not null or FinalQuotePrice <> '')) = 0 ) )
                            or 
                              ( (a.ApprovalStatus = 4) and (5 not in (select ApprovalStatus from TQuoteDetails where RequestNumber = A.RequestNumber) ) )
                            )";
                }
                #endregion

                if (From != "" && To != "")
                {
                    if (FltrDate == "RequestDate")
                    {
                        sql += @" and format(RequestDate, 'yyyy-MM-dd') between @From and @To ";
                    }
                    else if (FltrDate == "QuoteResponseDueDate")
                    {
                        sql += @" and format(QuoteResponseDueDate, 'yyyy-MM-dd') between @From and @To ";
                    }
                }

                if (FilterValue != "")
                {
                    if (FilterBy == "Plant")
                    {
                        sql += @" and Plant like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "RequestNumber")
                    {
                        sql += @" and RequestNumber like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "Product")
                    {
                        sql += @" and Product like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "Material")
                    {
                        sql += @" and Material like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "MaterialDesc")
                    {
                        sql += @" and MaterialDesc like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "QuoteNo")
                    {
                        sql += @" and QuoteNo like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "VendorCode1")
                    {
                        sql += @" and VendorCode1 like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "VendorName")
                    {
                        sql += @" and VendorName like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "CreatedBy")
                    {
                        sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "UseDep")
                    {
                        sql += @" and SMNPicDept like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "ProcessGroup")
                    {
                        sql += @" and A.ProcessGroup like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "ProcessGroupDesc")
                    {
                        sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = A.ProcessGroup) like '%'+@Filter+'%' ";
                    }
                }

                sql += @" Order by RequestNumber desc ";
                #endregion

                #region Execute Query
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@Plant", Plant);
                if (VendorCode.Trim() != "")
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
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                #endregion

                #region Set Up Data for Main Data
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,NoQuote,RequestDate,QuoteResponseDueDate,Product,Material,MaterialDesc,CreatedBy,UseDep,ReqType,ReqSts from ##temp1";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtMain);
                }
                #endregion

                #region Set Up Data for Data Detail
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from ##temp1";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtDetail);
                }
                #endregion


                #region empty temp table
                sql = @"IF OBJECT_ID('tempdb..##temp1') IS NOT NULL DROP TABLE ##temp1 ";
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                EmetTrans.Commit();
                #endregion

                #region sET UP DATA TO MODEL

                var MyMainData = DtMain.AsEnumerable().Select(row => new
                {
                    Plant = row["Plant"],
                    RequestNumber = row["RequestNumber"],
                    NoQuote = row["NoQuote"],
                    RequestDate = row["RequestDate"],
                    QuoteResponseDueDate = row["QuoteResponseDueDate"],
                    Product = row["Product"],
                    Material = row["Material"],
                    MaterialDesc = row["MaterialDesc"],
                    CreatedBy = row["CreatedBy"],
                    UseDep = row["UseDep"],
                    ReqType = row["ReqType"],
                    ReqSts = row["ReqSts"]
                });
                MyResult.MainData = MyMainData;

                var DataDetail = DtDetail.AsEnumerable().Select(row => new
                {
                    Plant = row["Plant"],
                    RequestNumber = row["RequestNumber"],
                    NoQuote = row["NoQuote"],
                    RequestDate = row["RequestDate"],
                    QuoteResponseDueDate = row["QuoteResponseDueDate"],
                    Product = row["Product"],
                    Material = row["Material"],
                    MaterialDesc = row["MaterialDesc"],
                    CreatedBy = row["CreatedBy"],
                    UseDep = row["UseDep"],
                    ReqType = row["ReqType"],
                    ReqSts = row["ReqSts"],
                    VendorCode = row["VendorCode"],
                    VendorName = row["VendorName"],
                    QuoteNo = row["QuoteNo"],
                    TotalMaterialCost = row["TotalMaterialCost"],
                    TotalProcessCost = row["TotalProcessCost"],
                    TotalSubMaterialCost = row["TotalSubMaterialCost"],
                    TotalOtheritemsCost = row["TotalOtheritemsCost"],
                    GrandTotalCost = row["GrandTotalCost"],
                    FinalQuotePrice = row["FinalQuotePrice"],
                    NetProfit_Discount = row["NetProfit_Discount"],
                    ResponseStatus = row["ResponseStatus"],
                    PICApprovalStatus = row["PICApprovalStatus"],
                    ManagerApprovalStatus = row["ManagerApprovalStatus"],
                    DIRApprovalStatus = row["DIRApprovalStatus"],
                    MDecision = row["MDecision"],
                    MComment = row["MComment"],
                    Mname = row["Mname"],
                    MAprRejDt = row["MAprRejDt"],
                    DDecision = row["DDecision"],
                    Dname = row["Dname"],
                    DAprRejDt = row["DAprRejDt"],
                    DComment = row["DComment"]
                });
                MyResult.AllRequestData = DataDetail;
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
                EmetCon.Dispose();
                EmetTrans.Dispose();
            }
        }
    }
}
