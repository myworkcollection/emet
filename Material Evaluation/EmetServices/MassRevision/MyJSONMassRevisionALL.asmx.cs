using Material_Evaluation.EmetServices.Model;
using Material_Evaluation.EmetServices.Model.MassRevision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Material_Evaluation.EmetServices.MassRevision
{
    /// <summary>
    /// Summary description for MyJSONMassRevisionALL
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MyJSONMassRevisionALL : System.Web.Services.WebService
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
        public void ColectDataFileUpload(string UseID)
        {
            DataTable DtResult = new DataTable();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string PathAndFileName = "";
            try
            {
                BasicData MyResult = new BasicData();
                
                if (UseID == null)
                {
                    MyResult.success = false;
                    MyResult.message = "User Session Expired";
                    js.MaxJsonLength = int.MaxValue;
                    js.Serialize(MyResult);
                    Context.Response.Write(js.Serialize(MyResult));
                }
                else
                {
                    string SessionName = UseID + "_" + "FileMassUploadALL";
                    if (HttpContext.Current.Session[SessionName] == null)
                    {
                        MyResult.success = false;
                        MyResult.message = "Fail To Read File Attachment, please re upload file again";
                        
                        js.MaxJsonLength = int.MaxValue;
                        js.Serialize(MyResult);
                        Context.Response.Write(js.Serialize(MyResult));
                    }
                    else
                    {
                        HttpPostedFile files = (HttpPostedFile)HttpContext.Current.Session[SessionName];
                        string folderPath = Server.MapPath("~/Files/");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        string filename = "";
                        files = (HttpPostedFile)Session[SessionName];
                        if (files.ContentLength > 0)
                        {
                            filename = System.IO.Path.GetFileName(files.FileName);
                            PathAndFileName = folderPath + SessionName + filename;
                            files.SaveAs(PathAndFileName);
                        }
                        String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", PathAndFileName);
                        //Create Connection to Excel work book 
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                        {
                            excelConnection.Open();
                            using (OleDbDataAdapter oda = new OleDbDataAdapter())
                            {
                                DataTable dtExcelSchema = new DataTable();
                                dtExcelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string Query = "SELECT [Plant],[PIR No],[Material Code],[Material Desc],[Vendor Code],[Vendor Name],[Process Group] From [" + SheetName + "] where [Plant] <> '' ";
                                OleDbCommand oleDBcmd = new OleDbCommand(Query, excelConnection);
                                oda.SelectCommand = oleDBcmd;
                                DataSet ds = new DataSet();
                                oda.Fill(ds, "DsMassRevisonAll");
                                DtResult = ds.Tables["DsMassRevisonAll"];

                                //Session["BasicData_"+ SessionName + ""] = DtResult;
                            }
                        }

                        FileInfo file = new FileInfo(PathAndFileName);
                        if (file.Exists) //check file exsit or not  
                        {
                            file.Delete();
                        }

                        MyResult.success = true;
                        MyResult.message = "Ok";
                        var convertdata = DtResult.AsEnumerable().Select(row => new
                        {
                            Plant = row["Plant"],
                            PIRNo = row["PIR No"],
                            MaterialCode = row["Material Code"],
                            MaterialDesc = row["Material Desc"],
                            VendorCode = row["Vendor Code"],
                            VendorName = row["Vendor Name"],
                            ProcessGroup = row["Process Group"]
                        });

                        MyResult.MyBasicData = convertdata;
                        js.MaxJsonLength = int.MaxValue;
                        js.Serialize(MyResult);
                        Context.Response.Write(js.Serialize(MyResult));
                    }
                }
            }
            catch (Exception ex)
            {
                FileInfo file = new FileInfo(PathAndFileName);
                if (file.Exists) //check file exsit or not  
                {
                    file.Delete();
                }

                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();

                EMETModule.SendExcepToDB(ex);
                
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            finally
            {
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DeleteNonRequest(string UseID)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            GlobalResult MyResult = new GlobalResult();
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                EmetCon.Open();
                string sql = @" delete from TQuoteDetails Where (CreateStatus = '' and createdby= @UseID) or (CreateStatus is null and createdby=@UseID)";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UseID", UseID);
                cmd.ExecuteNonQuery();

                MyResult.success = true;
                MyResult.message = "Ok";
                
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        string GetQuoteNextRevDate(string Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string DefVal = "";
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select DefValue from DefaultValueMaster where Description = 'Quote Next Rev Date' and  Plant=@Plant and DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Plant);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows[0]["DefValue"].ToString() != "")
                        {
                            DefVal = dt.Rows[0]["DefValue"].ToString();
                        }
                    }
                }
                return DefVal;
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                return DefVal;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        bool ValidateDefQuoteNextRev(string DefQuoteNextRev)
        {
            try
            {
                DateTime DateDefQuoteNextRev = new DateTime();
                DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd/MM/yyyy", null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void SetDueOnDate(string  Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DueOnDateResult MyResult = new DueOnDateResult();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string DefQuoteNextRev;
            try
            {
                DefQuoteNextRev = GetQuoteNextRevDate(Plant);
                if (DefQuoteNextRev != "")
                {
                    DefQuoteNextRev = DefQuoteNextRev.Replace(".", "/").Replace("/", "-");
                    if (ValidateDefQuoteNextRev(DefQuoteNextRev) == true)
                    {
                        DateTime DateDefQuoteNextRev = new DateTime();
                        DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);
                        if (DateDefQuoteNextRev > DateTime.Today)
                        {
                            MyResult.Enabled = false;
                        }
                        else
                        {
                            MyResult.Enabled = true;
                        }
                        MyResult.message = DefQuoteNextRev;

                    }
                }

                MyResult.success = true;
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                MyResult.Enabled = true;
                EMETModule.SendExcepToDB(ex);
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            finally
            {
            }
        }
    }
}
