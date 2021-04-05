using Material_Evaluation.EmetServices.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.RevisionEMET
{
    /// <summary>
    /// Summary description for SubmitRequestRevisionEmet
    /// </summary>
    public class SubmitRequestRevisionEmet : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string DbMasterName = "";

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

        public void DeleteNonRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string sql = @"delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + HttpContext.Current.Session["userID"].ToString() + @"') 
                                or (CreateStatus is null and createdby= '" + HttpContext.Current.Session["userID"].ToString() + "')";
                SqlCommand cmd = new SqlCommand(sql, EmetCon);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }
        

        public void ProcessRequest(HttpContext context)
        {
            bool isSucces = false;
            string msger = "";
            try
            {
                var formdata = HttpContext.Current.Request.Form;
                if (formdata.AllKeys.Length > 0)
                {
                    GetDbMaster();
                    SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                    SqlTransaction EmetTrans = null;
                    try
                    {
                        EmetCon.Open();
                        EmetTrans = EmetCon.BeginTransaction();
                        
                        for (int i = 0; i < formdata.AllKeys.Length; i++)
                        {
                            if (HttpContext.Current.Request.Form["ReqNo_" + i] == null)
                            {
                                break;
                            }
                            else
                            {
                                #region Collect Information need to process
                                #region Main Info
                                string ReqNo = HttpContext.Current.Request.Form["ReqNo_" + i];
                                string QuoteNo = HttpContext.Current.Request.Form["QuoteNo_" + i];
                                string QuoteNoRef = HttpContext.Current.Request.Form["QuoteNoRef_" + i];
                                string VendorCode1 = HttpContext.Current.Request.Form["VendorCode1_" + i];
                                string VendorName = HttpContext.Current.Request.Form["VendorName_" + i];
                                string Material = HttpContext.Current.Request.Form["Material_" + i];
                                string MaterialDesc = HttpContext.Current.Request.Form["MaterialDesc_" + i];
                                string ProcessGroup = HttpContext.Current.Request.Form["ProcessGroup_" + i];
                                string IsUseToolAmor = HttpContext.Current.Request.Form["IsUseToolAmor_" + i];
                                string StrDataListVndAmor = HttpContext.Current.Request.Form["ToolAmorID_" + i];
                                string FlAtc = HttpContext.Current.Request.Form["FlAtc" + i];
                                string FileName = "";
                                #endregion

                                #region File Attchment Info
                                HttpPostedFile file = null;
                                if (context.Request.Files.Count > 0)
                                {
                                    string filedata = string.Empty;
                                    HttpFileCollection files = context.Request.Files;
                                    string SessionName = "";
                                    if (files.AllKeys[i] != null)
                                    {
                                        SessionName = files.AllKeys[i].ToString();
                                    }
                                    else
                                    {
                                        SessionName = FlAtc;
                                    }
                                    file = files[i];
                                    //if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                                    //{
                                    //    context.Response.ContentType = "text/plain";
                                    //    context.Response.Write("Only .pdf are allowed.!");
                                    //    return;
                                    //}
                                    //decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                                    //if (size > 2048)
                                    //{
                                    //    context.Response.ContentType = "text/plain";
                                    //    context.Response.Write("File size should not exceed 2 MB.!");
                                    //    return;
                                    //}

                                    if (file.ContentLength > 0)
                                    {
                                        FileName = file.FileName.ToString();
                                        context.Session.Add(SessionName, file);
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Session[SessionName] != null)
                                        {
                                            HttpContext.Current.Session[SessionName] = null;
                                        }
                                    }
                                }
                                #endregion
                                
                                #endregion

                                #region Process Data 

                                string sql = "";
                                sql = @" update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, 
                                ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL ,DrawingNo = '"+ FileName +"' where QuoteNo = '" + QuoteNo + "' ";
                                SqlCommand cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();

                                if (IsUseToolAmor == "ADD")
                                {
                                    sql = @" INSERT INTO TToolAmortization(Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                                                        ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,AmortizeCost,AmortizeCurrency
                                                        ,ExchangeRate,AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                                                        ,EffectiveFrom,DueDate,CreatedBy,CreatedOn)
                                                        select TT.Plant,@ReqNo,@NewQuoteNo,null,@Material,@MaterialDescription,@VendorCode
                                                        ,@VendorDesc,@Process_Grp_code,TT.ToolTypeID,TT.Amortize_Tool_ID,TT.Amortize_Tool_Desc,TT.AmortizeCost,TT.AmortizeCurrency
                                                        ,TT.ExchangeRate,TT.AmortizeCost_Vend_Curr,TT.AmortizePeriod,TT.AmortizePeriodUOM,TT.TotalAmortizeQty,TT.QtyUOM,TT.AmortizeCost_Pc_Vend_Curr
                                                        ,TT.EffectiveFrom,TT.DueDate,@CreatedBy,CURRENT_TIMESTAMP 
                                                        from " + DbMasterName + @".dbo.TToolAmortization as TT
                                                        inner join " + DbMasterName + @".dbo.TToolAmortizationvsMember as TM  on TM.Plant = TT.Plant and tm.Amortize_Tool_ID = tt.Amortize_Tool_ID and tm.Material = @Material 
                                                        where TT.Amortize_Tool_ID = @Amortize_Tool_ID and TT.Plant=@Plant and TT.VendorCode=@VendorCode and TT.Process_Grp_code=@Process_Grp_code";
                                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                    cmd.Parameters.AddWithValue("@Plant", HttpContext.Current.Session["EPlant"].ToString());
                                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode1);
                                    cmd.Parameters.AddWithValue("@VendorDesc", VendorName);
                                    cmd.Parameters.AddWithValue("@Process_Grp_code", ProcessGroup);
                                    cmd.Parameters.AddWithValue("@Material", Material);
                                    cmd.Parameters.AddWithValue("@MaterialDescription", MaterialDesc);
                                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                                    cmd.Parameters.AddWithValue("@NewQuoteNo", QuoteNo);
                                    cmd.Parameters.AddWithValue("@QuoteNoRef", QuoteNoRef);
                                    cmd.Parameters.AddWithValue("@Amortize_Tool_ID", StrDataListVndAmor);
                                    cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userID"].ToString());
                                    cmd.ExecuteNonQuery();
                                }
                                else if (IsUseToolAmor == "NO CHANGE")
                                {
                                    sql = @" INSERT INTO TToolAmortization(Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                                ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,AmortizeCost,AmortizeCurrency
                                ,ExchangeRate,AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                                ,EffectiveFrom,DueDate,CreatedBy,CreatedOn)
                                select TT.Plant,@ReqNo,@NewQuoteNo,null,@Material,@MaterialDescription,@VendorCode
                                ,@VendorDesc,@Process_Grp_code,TT.ToolTypeID,TT.Amortize_Tool_ID,TT.Amortize_Tool_Desc,TT.AmortizeCost,TT.AmortizeCurrency
                                ,TT.ExchangeRate,TT.AmortizeCost_Vend_Curr,TT.AmortizePeriod,TT.AmortizePeriodUOM,TT.TotalAmortizeQty,TT.QtyUOM,TT.AmortizeCost_Pc_Vend_Curr
                                ,TT.EffectiveFrom,TT.DueDate,@CreatedBy,CURRENT_TIMESTAMP 
                                from TToolAmortization as TT
                                where TT.Plant=@Plant and TT.VendorCode=@VendorCode and TT.Process_Grp_code=@Process_Grp_code
                                and TT.QuoteNo = @QuoteNoRef
                                and (CURRENT_TIMESTAMP between TT.EffectiveFrom And TT.DueDate)";
                                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                    cmd.Parameters.AddWithValue("@Plant", HttpContext.Current.Session["EPlant"].ToString());
                                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode1);
                                    cmd.Parameters.AddWithValue("@VendorDesc", VendorName);
                                    cmd.Parameters.AddWithValue("@Process_Grp_code", ProcessGroup);
                                    cmd.Parameters.AddWithValue("@Material", Material);
                                    cmd.Parameters.AddWithValue("@MaterialDescription", MaterialDesc);
                                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                                    cmd.Parameters.AddWithValue("@NewQuoteNo", QuoteNo);
                                    cmd.Parameters.AddWithValue("@QuoteNoRef", QuoteNoRef);
                                    cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userID"].ToString());
                                    cmd.ExecuteNonQuery();
                                }
                                #endregion

                                #region save data bom list raw material baed on effective date
                                if (HttpContext.Current.Session["Rawmaterial"] != null)
                                {
                                    DataTable dtRawmat = (DataTable)HttpContext.Current.Session["Rawmaterial"];
                                    if (dtRawmat.Rows.Count > 0)
                                    {
                                        CultureInfo culture = new CultureInfo("en-US");
                                        for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                                        {
                                            string ValidFrom = "";
                                            
                                            DateTime DtCusMatValFrom = Convert.ToDateTime(dtRawmat.Rows[rm]["CusMatValFrom"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["CusMatValFrom"].ToString(), "dd-MM-yyyy", null);*/
                                            DateTime DtCusMatValTo = Convert.ToDateTime(dtRawmat.Rows[rm]["CusMatValTo"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["CusMatValTo"].ToString(), "dd-MM-yyyy", null);*/
                                            if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                            {
                                                DateTime DtValFrm = Convert.ToDateTime(dtRawmat.Rows[rm]["ValidFrom"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);*/
                                                ValidFrom = DtValFrm.ToString("yyyy-MM-dd");
                                            }

                                            sql = @" insert into TSMMBOM_RAWMATCost_EffDate(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                                                        AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                                                                        values(
                                                                        '" + dtRawmat.Rows[rm]["Req No"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["QuoteNo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material Desc"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_SCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Selling_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_VCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Venor_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Unit"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["UOM"] + @"',
                                                                        '" + DtCusMatValFrom.ToString("yyyy-MM-dd") + @"',
                                                                        '" + DtCusMatValTo.ToString("yyyy-MM-dd") + @"',
                                                                        '" + dtRawmat.Rows[rm]["ExchRate"] + @"', ";
                                            if (ValidFrom == "")
                                            {
                                                sql += @" NULL,";
                                            }
                                            else
                                            {
                                                sql += @" '" + ValidFrom + "',";
                                            }
                                            sql += @" CURRENT_TIMESTAMP, '" + HttpContext.Current.Session["userID"].ToString() + @"') ";

                                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                #endregion

                                #region save data bom list raw material before effective date
                                if (HttpContext.Current.Session["OldRawmaterial"] != null)
                                {
                                    DataTable dtRawmat = (DataTable)HttpContext.Current.Session["OldRawmaterial"];
                                    if (dtRawmat.Rows.Count > 0)
                                    {
                                        CultureInfo culture = new CultureInfo("en-US");
                                        for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                                        {
                                            string ExchValidFrom = "";
                                            DateTime DtCusMatValFrom = Convert.ToDateTime(dtRawmat.Rows[rm]["CusMatValFrom"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["CusMatValFrom"].ToString(), "dd-MM-yyyy", null);*/
                                            DateTime DtCusMatValTo = Convert.ToDateTime(dtRawmat.Rows[rm]["CusMatValTo"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["CusMatValTo"].ToString(), "dd-MM-yyyy", null);*/
                                            if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                            {
                                                DateTime DtValFrm = Convert.ToDateTime(dtRawmat.Rows[rm]["ValidFrom"], culture); /*DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);*/
                                                ExchValidFrom = DtValFrm.ToString("yyyy-MM-dd");
                                            }

                                            sql = @" insert into TSMNBOM_RAWMATCost(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                                                        AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                                                                        values(
                                                                        '" + dtRawmat.Rows[rm]["Req No"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["QuoteNo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material Desc"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_SCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Selling_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_VCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Venor_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Unit"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["UOM"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValFrom"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValTo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["ExchRate"] + @"', ";
                                            if (ExchValidFrom == "")
                                            {
                                                sql += @" NULL,";
                                            }
                                            else
                                            {
                                                sql += @" '" + ExchValidFrom + "',";
                                            }
                                            sql += @" CURRENT_TIMESTAMP, '" + HttpContext.Current.Session["userID"].ToString() + @"') ";

                                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                #endregion
                                
                            }
                        }

                        EmetTrans.Commit();
                        isSucces = true;
                    }
                    catch (Exception ex)
                    {
                        msger = ex.ToString();
                        isSucces = false;
                    }
                    finally
                    {
                        EmetCon.Dispose();
                        EmetTrans.Dispose();
                    }

                    if (isSucces == false)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(msger);
                    }
                    else
                    {
                        HttpContext.Current.Session["Rawmaterial"] = null;
                        DeleteNonRequest();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Successfully submitted");
                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("No Data To Process");
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}