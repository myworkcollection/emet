using Material_Evaluation.EmetServices.Model;
using Material_Evaluation.EmetServices.Model.MassRevisionReqWait;
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

namespace Material_Evaluation.EmetServices.MassRevisionReqWait
{
    /// <summary>
    /// Summary description for MyJSONMassRevisionALLReqWait
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MyJSONMassRevisionALLReqWait : System.Web.Services.WebService
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
        public void LoadData(string Plant, string FltrDate, string From, string To, string FilterBy, string FilterValue)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            DataTable DtMain = new DataTable();
            DataTable DtDetail = new DataTable();

            MassRevisionReqWaitResult MyResult = new MassRevisionReqWaitResult();
            try
            {
                GetDbMaster();
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                string sql = "";
                #region Query get Data
                sql = @" select distinct Plant,RequestNumber,VendorCode1,VendorName,QuoteNo,
                            (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            RequestDate, QuoteResponseDueDate,Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep'
                            into ##temp1 
                            from TQuoteDetails A
                            where 
                            ApprovalStatus not in ('2','1','3') and (ApprovalStatus is not null)
                            and isUseSAPCode = 1 and (isMassRevision=0 or isMassRevision is null) 
                            and isMassRevisionAll=1
                            and Plant  = @Plant and isnull(A.CreateStatus,'') <> '' and A.QuoteNoRef is null ";

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

                 if (FilterBy != "")
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
                        sql += @" and ProcessGroup like '%'+@Filter+'%' ";
                    }
                    else if (FilterBy == "ProcessGroupDesc")
                    {
                        sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = ProcessGroup) like '%'+@Filter+'%' ";
                    }
                }

                sql += @" Order by RequestNumber desc ";
                #endregion

                #region Execute Query
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@Plant", Plant);
                
                cmd.Parameters.AddWithValue("@Filter", FilterValue);
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
                    sql = @" select distinct Plant,RequestNumber,NoQuote,
                            RequestDate, QuoteResponseDueDate,Product,Material,MaterialDesc,CreatedBy,UseDep
                            from ##temp1 ";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtMain);
                }
                #endregion

                #region Set Up Data for Data Detail
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,NoQuote,
                            RequestDate, QuoteResponseDueDate,Product,Material,MaterialDesc,CreatedBy,UseDep,VendorCode1,VendorName,QuoteNo from ##temp1";
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
                    UseDep = row["UseDep"]
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
                    VendorCode = row["VendorCode1"],
                    VendorName = row["VendorName"],
                    QuoteNo = row["QuoteNo"]
                });
                MyResult.DataDetail = DataDetail;
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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RejectQuote(string RequestNo, string UseId)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            
            GlobalResult MyResult = new GlobalResult();
            try
            {
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                string sql = @" Update TQuoteDetails SET ApprovalStatus='1', PICApprovalStatus = '1',
                                PICRejRemark = concat(@UseId,'-','Quotation Canceled'), 
                                ManagerApprovalStatus = '1', DIRApprovalStatus = '1',  
                                UpdatedBy = @UseId, 
                                UpdatedOn = CURRENT_TIMESTAMP
                                where RequestNumber = @RequestNo ";
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@RequestNo", RequestNo);
                cmd.Parameters.AddWithValue("@UseId", UseId);
                cmd.ExecuteNonQuery();
                EmetTrans.Commit();
                MyResult.success = true;
                MyResult.message = "Reject Success";

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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdatedateQuotation(string RequestNo, string NewDueDate, string UseId)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;

            GlobalResult MyResult = new GlobalResult();
            try
            {
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                string sql = @" update TQuoteDetails set QuoteResponseDueDate = @QuoteResponseDueDate where RequestNumber = @RequestNumber ";
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@RequestNumber", RequestNo);
                DateTime dtnewDueOn =  DateTime.ParseExact(NewDueDate.Replace("/","-"), "dd-MM-yyyy", null);
                cmd.Parameters.AddWithValue("@QuoteResponseDueDate", dtnewDueOn.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@UseId", UseId);
                cmd.ExecuteNonQuery();
                EmetTrans.Commit();
                MyResult.success = true;
                MyResult.message = "Response Due Date Updated";

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
