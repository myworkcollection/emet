using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.SessionState;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Text;
using System.Data.OleDb;
using System.Data.Common;
using System.Collections;

namespace Material_Evaluation
{
    public partial class NewRequestUpload : System.Web.UI.Page
    {
        //email
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string SendFilename;
        public string userId1;
        public string nameC;
        public string aemail;
        public string pemail;
        public string pemail1;
        public string Uemail;
        public string body1;
        public string quoteno;
        public string quoteno1;
        public int benable;
        public string vname;
        public string demail;
        public string vemail;
        public string customermail;
        public string customermail1;
        public string cc;

        public string SrvUserid;
        public string Srvpassword;
        public string Srvdomain;
        public string Srvpath;
        public string SrvURL;

        public string UseridMail;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        public static string password;
        public static string domain;
        public static string path;

        //email

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool IsAth;
        bool sendingmail;
        string errmsg;
        string DbMasterName = "";
        string DbTransName = "";
        string GA = "";
        string PlantDesc = "";
        string SMNPICSubmDept = "";
        bool isUploadScs = false;
        bool ischecksucces = false;
        bool isFromMainFunction = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null || Session["UserName"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_MassRevision";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            userId1 = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            nameC = sname;
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;


                            lbluser1.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            GetDdlReason();
                            DeleteNonRequest();
                            SetDueOnDate();

                            Session["DtAllData"] = null;
                            Session["DtValidData"] = null;
                            Session["DtInValidData"] = null;
                            Session["dtRegList"] = null;
                            Session["DtForSubmit"] = null;
                            Session["dtRegList"] = null;
                            Session["dtInValidRegList"] = null;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();CloseLoading();", true);
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            }
        }
        

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }

        protected void GetDbTrans()
        {
            try
            {
                DbTransName = EMETModule.GetDbTransname() + ".[dbo].";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbTransName = "";
            }
        }

        string GetQuoteNextRevDate()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string DefVal = "";
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select DefValue from DefaultValueMaster where Description = 'Quote Next Rev Date' and  Plant='" + Session["EPlant"].ToString() + "' and DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return DefVal;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

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

        protected void SetDueOnDate()
        {
            try
            {
                string DefQuoteNextRev = GetQuoteNextRevDate();
                if (DefQuoteNextRev != "")
                {
                    DefQuoteNextRev = DefQuoteNextRev.Replace(".", "/").Replace("-", "/");
                    if (ValidateDefQuoteNextRev(DefQuoteNextRev) == true)
                    {
                        DateTime DateDefQuoteNextRev = new DateTime();
                        DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd/MM/yyyy", null);
                        if (DateDefQuoteNextRev > DateTime.Today)
                        {
                            TxtDuenextRev.Enabled = false;
                        }
                        else
                        {
                            TxtDuenextRev.Enabled = true;
                        }
                        TxtDuenextRev.Text = DefQuoteNextRev;

                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }


        public void DeleteNonRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + Session["userID"].ToString() + "') or (CreateStatus is null and createdby= '" + Session["userID"].ToString() + "')";
                //Label17.Text = str.ToString();
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                DvSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        string GenerateReqNo()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            string GenerateReqNo = "";
            try
            {
                EmetCon.Open();
                string formatstatus = string.Empty;
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string currentYear_ = DateTime.Now.Year.ToString();
                currentYear = currentYear.Substring(currentYear.Length - 2);
                string RequestIncNumber = "";
                int increquest = 00000;
                string RequestInc = String.Format(currentYear, increquest);
                string REQUEST = string.Concat(currentYear, RequestIncNumber);

                SqlCommand cmd;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select MAX(RequestNumber) from TQuoteDetails ";
                    cmd = new SqlCommand(sql, EmetCon);
                    //cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string curentYear = DateTime.Now.Year.ToString();
                            curentYear = curentYear.Substring(curentYear.Length - 2);
                            currentYear = currentYear.Substring(currentYear.Length - 2);
                            string ReqNum = dt.Rows[0][0].ToString();

                            if (ReqNum == "")
                            {
                                RequestIncNumber = "00000";
                                RequestIncNumber1 = "00000";
                                Session["RequestIncNumber1"] = "00000";
                                string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber = string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber1 = string.Concat(currentYear, RequestIncNumber);
                                Session["RequestIncNumber1"] = string.Concat(currentYear, RequestIncNumber);
                                GenerateReqNo = RequestIncNumber;
                            }
                            else
                            {

                                ReqNum = ReqNum.Remove(0, 2);
                                ReqNum = string.Concat(currentYear, ReqNum);
                                RequestIncNumber = ReqNum;
                                RequestIncNumber1 = ReqNum;
                                GenerateReqNo = RequestIncNumber;
                            }
                            int newReq = (int.Parse(RequestIncNumber)) + (1);
                            RequestIncNumber = newReq.ToString();
                            RequestIncNumber1 = RequestIncNumber;
                            GenerateReqNo = RequestIncNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return GenerateReqNo;
        }

        protected void TempInsDataToEmet(string PIRNo,string reqno, string QN,string VC, string VN,string PG, 
            string Material,string MaterialDesc, string PIRJobTypeNDesc)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @" INSERT INTO TQuoteDetails(PIRNo,RequestNumber,RequestDate,EffectiveDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,PIRJobType,Material,
                         MaterialDesc,CreatedBy,isUseSAPCode,SMNPicDept) 
                         values (@PIRNo,@REQUESTNO,CURRENT_TIMESTAMP,@EffectiveDate,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@PIRJobType,@Material,
                         @MaterialDesc,@UserId,1,@SMNPicDept)";
                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@PIRNo", PIRNo);
                cmd.Parameters.AddWithValue("@REQUESTNO", reqno);
                cmd.Parameters.AddWithValue("@QuoteNo", QN);
                cmd.Parameters.AddWithValue("@PLANT", Session["EPlant"].ToString());
                cmd.Parameters.AddWithValue("@VENDORCODE", VC);
                cmd.Parameters.AddWithValue("@VENDORNAME", VN);
                cmd.Parameters.AddWithValue("@procesgrp", PG);
                cmd.Parameters.AddWithValue("@PIRJobType", PIRJobTypeNDesc);
                cmd.Parameters.AddWithValue("@Material", Material);
                cmd.Parameters.AddWithValue("@MaterialDesc", MaterialDesc);
                cmd.Parameters.AddWithValue("@UserId", Session["userID"].ToString());
                cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());
                DateTime DtEff = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                cmd.Parameters.AddWithValue("@EffectiveDate", DtEff.ToString("yyyy/MM/dd"));
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally {
                EmetCon.Dispose();
            }
        }

        bool CekVendorVsMaterialExpiredReq(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd/MM/yyyy') as RequestDate,format(QuoteResponseDueDate,'dd/MM/yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + "' and ApprovalStatus = 0  and format(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            return true;
                        }
                        else
                        {
                            if (Session["InvalidRequestExpiredReq"] == null)
                            {
                                Session["InvalidRequestExpiredReq"] = dt;
                            }
                            else
                            {
                                DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReq"];
                                DataRow dr = DtTemp.NewRow();
                                dr["Plant"] = dt.Rows[0]["Plant"].ToString();
                                dr["RequestNumber"] = dt.Rows[0]["RequestNumber"].ToString();
                                dr["RequestDate"] = dt.Rows[0]["RequestDate"].ToString();
                                dr["QuoteResponseDueDate"] = dt.Rows[0]["QuoteResponseDueDate"].ToString();
                                dr["QuoteNo"] = dt.Rows[0]["QuoteNo"].ToString();
                                dr["Material"] = dt.Rows[0]["Material"].ToString();
                                dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
                                dr["VendorCode1"] = dt.Rows[0]["VendorCode1"].ToString();
                                dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                                DtTemp.Rows.Add(dr);
                                DtTemp.AcceptChanges();
                                Session["InvalidRequestExpiredReq"] = DtTemp;
                            }
                            return false;
                        }
                    }
                    //UpdatePanel18.Update();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void ProcessDuplicateReqWithOldNExpiredReq()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = "";
                if (GvDuplicateWithExpiredReq.Rows.Count > 0)
                {
                    string PrevReqNo = "";
                    for (int i = 0; i < GvDuplicateWithExpiredReq.Rows.Count; i++)
                    {
                        RadioButton RbReject = GvDuplicateWithExpiredReq.Rows[i].FindControl("RbReject") as RadioButton;
                        RadioButton RbChangeDate = GvDuplicateWithExpiredReq.Rows[i].FindControl("RbChangeDate") as RadioButton;
                        TextBox TxtNewDueDate = GvDuplicateWithExpiredReq.Rows[i].FindControl("TxtNewDueDate") as TextBox;

                        if (RbReject != null && RbChangeDate != null && TxtNewDueDate != null)
                        {
                            string ReqNo = GvDuplicateWithExpiredReq.Rows[i].Cells[1].Text;
                            if (PrevReqNo != ReqNo)
                            {
                                string QuNo = GvDuplicateWithExpiredReq.Rows[i].Cells[3].Text;
                                DateTime DtDueDate = DateTime.ParseExact(TxtNewDueDate.Text, "dd/MM/yyyy", null);
                                string StrNewQuoteResponseDueDate = DtDueDate.ToString("yyyy-MM-dd");
                                if (RbReject.Checked == true)
                                {
                                    sql += @" Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + @"',  --PICReason = 'New Requset Created with Req NO : " + ReqNo + @"', 
                                        ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "', UpdatedBy='" + Session["userID"].ToString() + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"' 
                                      where RequestNumber = '" + ReqNo + "' ";
                                }
                                else
                                {
                                    sql += @" update TQuoteDetails set QuoteResponseDueDate = '" + StrNewQuoteResponseDueDate + "' where RequestNumber = '" + ReqNo + "' ";
                                }
                                PrevReqNo = ReqNo;
                            }
                        }
                    }
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 10000;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void SpanRowGvInvalidExpired()
        {
            try
            {
                int RowSpan = 2;
                for (int i = GvDuplicateWithExpiredReq.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GvDuplicateWithExpiredReq.Rows[i];
                    GridViewRow prevRow = GvDuplicateWithExpiredReq.Rows[i + 1];
                    if (currRow.Cells[1].Text == prevRow.Cells[1].Text)
                    {
                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[4].RowSpan = RowSpan;
                        prevRow.Cells[4].Visible = false;

                        currRow.Cells[5].RowSpan = RowSpan;
                        prevRow.Cells[5].Visible = false;

                        currRow.Cells[8].RowSpan = RowSpan;
                        prevRow.Cells[8].Visible = false;

                        currRow.Cells[9].RowSpan = RowSpan;
                        prevRow.Cells[9].Visible = false;

                        currRow.Cells[10].RowSpan = RowSpan;
                        prevRow.Cells[10].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        RowSpan = 2;
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        bool ProceedSubmitRequest()
        {
            GetDbMaster();
            SqlTransaction EmetTrans = null;
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                MainDataForMail();
                sql = "";
                if (Session["dtRegList"] != null)
                {
                    DataTable dtRegList = (DataTable)Session["dtRegList"];
                    if (dtRegList.Rows.Count > 0)
                    {
                        EmetCon.Open();
                        EmetTrans = EmetCon.BeginTransaction();
                        
                        #region create temp table for valid data req list 
                        sql = @"create table #tempRegListemet
                                (
                                Plant nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                PIRNo nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                ReqNo nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                MaterialCode nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                CodeRef nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                VendorCode nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                ProcessGroup nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                PIRJobType nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                CompMaterial nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                CompMaterialDesc nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                Amount nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                UnitofCurrency nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                Unit nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                UoM nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                ValidFrom nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                ValidTo nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                QuoteNo nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                Product nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                MaterialClass nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                PIRType nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                UnitWeight nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                UnitWeightUOM nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                BaseUOM nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                SAPProcType nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                SAPSPProcType nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                MaterialType nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                PlantStatus nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                TotMatCost nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                TotProcCost nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                TotSubMatCost nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                TotOthCost nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                countryorg nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                MassUpdateDate nvarchar(500)  COLLATE DATABASE_DEFAULT
                                 )  ";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.ExecuteNonQuery();

                        if (dtRegList.Rows.Count > 0)
                        {
                            //string headerName = "";
                            //foreach (DataColumn c in dtRegList.Columns)
                            //{
                            //    headerName += c.ColumnName + " ";
                            //}
                            using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EmetCon, SqlBulkCopyOptions.Default, EmetTrans))
                            {
                                //Give your Destination table name 
                                sqlBulk.DestinationTableName = "#tempRegListemet";
                                sqlBulk.BulkCopyTimeout = 0;
                                sqlBulk.BatchSize = 10000;
                                sqlBulk.WriteToServer(dtRegList);
                            }
                        }

                        #region Generate manual query to insert data to temp table
                        //string temptabledatafortrans = @"";
                        //for (int i = 0; i < dtRegList.Rows.Count; i++)
                        //{
                        //    temptabledatafortrans += @" insert into #tempRegListemet(Plant,PIRNo,MaterialCode,MaterialDesc,CodeRef
                        //        ,VendorCode,VendorName,ProcessGroup,ReqNo,QuoteNo,PIRJobType,Product,MaterialClass,PIRType,UnitWeight,UnitWeightUOM,BaseUOM
                        //        ,SAPProcType,SAPSPProcType,MaterialType,PlantStatus,TotMatCost,TotProcCost,TotSubMatCost,TotOthCost,countryorg,MassUpdateDate)
                        //        values(
                        //        '" + dtRegList.Rows[i]["Plant"].ToString() + @"','" + dtRegList.Rows[i]["PIRNo"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["MaterialCode"].ToString() + @"','" + dtRegList.Rows[i]["MaterialDesc"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["CodeRef"].ToString() + @"','" + dtRegList.Rows[i]["VendorCode"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["VendorName"].ToString() + @"','" + dtRegList.Rows[i]["ProcessGroup"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["ReqNo"].ToString() + @"','" + dtRegList.Rows[i]["QuoteNo"].ToString() + @"','" + dtRegList.Rows[i]["PIRJobType"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["Product"].ToString() + @"','" + dtRegList.Rows[i]["MaterialClass"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["PIRType"].ToString() + @"','" + dtRegList.Rows[i]["UnitWeight"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["UnitWeightUOM"].ToString() + @"','" + dtRegList.Rows[i]["BaseUOM"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["SAPProcType"].ToString() + @"','" + dtRegList.Rows[i]["SAPSPProcType"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["MaterialType"].ToString() + @"','" + dtRegList.Rows[i]["PlantStatus"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["TotMatCost"].ToString() + @"','" + dtRegList.Rows[i]["TotProcCost"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["TotSubMatCost"].ToString() + @"','" + dtRegList.Rows[i]["TotOthCost"].ToString() + @"',
                        //        '" + dtRegList.Rows[i]["countryorg"].ToString() + @"','" + dtRegList.Rows[i]["MassUpdateDate"].ToString() + @"') ";
                        //}
                        #endregion

                        #endregion create temp table for valid data req list 

                        #region preapre and save content for mail
                        string footer = @" Please <a href=" + Convert.ToString(URL.ToString()) + ">Login</a> SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color=''red''>This is System generated mail.  Please do not reply to this message.</font>";
                        DateTime QuoteDatee = DateTime.ParseExact(TxtResDueDate.Text, "dd/MM/yyyy", null);

                        sql = @" insert into email(quotenumber, body) 
                                  SELECT QuoteNo,
                                  concat('The details are<br /><br /> Plant : "+ Session["EPlant"].ToString() + @" <br /> Vendor Name : ',VendorName,
                                         '<br /> Request Number : ',ReqNo,
                                         '<br />  Quote Number : ',QuoteNo,
                                         '<br /> Partcode And Description : ',MaterialCode ,' | ', MaterialDesc,
                                         '<br />  Quotation Response due Date : ',@ResDueDate, '<br /><br />',@footer
                                        )
                                    from #tempRegListemet ";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@ResDueDate", QuoteDatee);
                        cmd.Parameters.AddWithValue("@footer", footer);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region update data emet status non request to valid request
                        sql = @" UPDATE 
                                    TQuoteDetails
                                SET 
                                    TQuoteDetails.Product = #tempRegListemet.Product,
                                    TQuoteDetails.MaterialClass = #tempRegListemet.MaterialClass,
                                    TQuoteDetails.PIRJobType = #tempRegListemet.PIRJobType,
                                    TQuoteDetails.PIRType = #tempRegListemet.PIRType,
                                    TQuoteDetails.NetUnit = #tempRegListemet.UnitWeight,
                                    TQuoteDetails.ActualNU = #tempRegListemet.UnitWeight,
                                    TQuoteDetails.UOM = #tempRegListemet.UnitWeightUOM,
                                    TQuoteDetails.BaseUOM = #tempRegListemet.BaseUOM,
                                    TQuoteDetails.SAPProcType = #tempRegListemet.SAPProcType,
                                    TQuoteDetails.SAPSPProcType = case when #tempRegListemet.SAPSPProcType = '' then 'Blank' else #tempRegListemet.SAPSPProcType end,
                                    TQuoteDetails.MaterialType = #tempRegListemet.MaterialType,
                                    TQuoteDetails.PlantStatus = #tempRegListemet.PlantStatus,
                                    TQuoteDetails.ERemarks = @ERemarks,
                                    TQuoteDetails.PICReason = @PICReason,
                                    TQuoteDetails.CreateStatus = 'Article',
                                    TQuoteDetails.QuoteResponseDueDate = @QuoteDue,
                                    TQuoteDetails.EffectiveDate = @ValidDate,
                                    TQuoteDetails.DueOn = @DueOn,
                                    TQuoteDetails.ApprovalStatus = 0,
                                    TQuoteDetails.PICApprovalStatus = NULL,
                                    TQuoteDetails.ManagerApprovalStatus = NULL,
                                    TQuoteDetails.DIRApprovalStatus = NULL,
                                    TQuoteDetails.AcsTabMatCost = @AcsTabMatCost,
                                    TQuoteDetails.AcsTabProcCost = @AcsTabProcCost,
                                    TQuoteDetails.AcsTabSubMatCost=@AcsTabSubMatCost,
                                    TQuoteDetails.AcsTabOthMatCost=@AcsTabOthMatCost,
                                    TQuoteDetails.CreatedOn=CURRENT_TIMESTAMP,
                                    TQuoteDetails.isMassRevision=1,
                                    TQuoteDetails.TotalMaterialCost = #tempRegListemet.TotMatCost,
                                    TQuoteDetails.TotalProcessCost = #tempRegListemet.TotProcCost,
                                    TQuoteDetails.TotalSubMaterialCost = #tempRegListemet.TotSubMatCost,
                                    TQuoteDetails.TotalOtheritemsCost = #tempRegListemet.TotOthCost,
                                    TQuoteDetails.OldTotMatCost = #tempRegListemet.TotMatCost,
                                    TQuoteDetails.OldTotProCost = #tempRegListemet.TotProcCost,
                                    TQuoteDetails.OldTotSubMatCost = #tempRegListemet.TotSubMatCost,
                                    TQuoteDetails.OldTotOthCost = #tempRegListemet.TotOthCost,
                                    TQuoteDetails.countryorg = #tempRegListemet.countryorg,
                                    TQuoteDetails.MassUpdateDate = #tempRegListemet.MassUpdateDate
                                FROM 
                                    TQuoteDetails
                                    JOIN #tempRegListemet ON TQuoteDetails.RequestNumber=#tempRegListemet.ReqNo ";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        bool AcsTabMatCost = false;
                        bool AcsTabProcCost = false;
                        bool AcsTabSubMatCost = false;
                        bool AcsTabOthMatCost = false;
                        if (ChcMatCost.Checked == true)
                        {
                            AcsTabMatCost = true;
                        }
                        if (ChcProcCost.Checked == true)
                        {
                            AcsTabProcCost = true;
                        }
                        if (ChcSubMat.Checked == true)
                        {
                            AcsTabSubMatCost = true;
                        }
                        if (ChcOthMat.Checked == true)
                        {
                            AcsTabOthMatCost = true;
                        }
                        cmd.Parameters.AddWithValue("@AcsTabMatCost", AcsTabMatCost);
                        cmd.Parameters.AddWithValue("@AcsTabProcCost", AcsTabProcCost);
                        cmd.Parameters.AddWithValue("@AcsTabSubMatCost", AcsTabSubMatCost);
                        cmd.Parameters.AddWithValue("@AcsTabOthMatCost", AcsTabOthMatCost);
                        if (DdlReason.SelectedValue == "Others")
                        {
                            cmd.Parameters.AddWithValue("@PICReason", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ERemarks", txtRem.Text.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ERemarks", DBNull.Value);
                            cmd.Parameters.AddWithValue("@PICReason", DdlReason.SelectedItem.ToString());
                        }
                        DateTime QuoteDate = DateTime.ParseExact(TxtResDueDate.Text, "dd/MM/yyyy", null);
                        cmd.Parameters.AddWithValue("@QuoteDue", QuoteDate.ToString("yyyy/MM/dd HH:mm:ss"));
                        DateTime ValidDate = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                        cmd.Parameters.AddWithValue("@ValidDate", ValidDate.ToString("yyyy/MM/dd HH:mm:ss"));
                        DateTime DueOn = DateTime.ParseExact(TxtDuenextRev.Text, "dd/MM/yyyy", null);
                        cmd.Parameters.AddWithValue("@DueOn", DueOn.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region update data emet status non request to valid request
                        sql = @" UPDATE TQuoteDetails
                                SET 
                                    TQuoteDetails.MassRevQutoteRef = C.Quotation
                                FROM 
                                    TQuoteDetails A JOIN #tempRegListemet B ON A.RequestNumber=B.ReqNo 
                                    JOIN "+ DbMasterName + @".dbo.tPIRvsQuotation C ON A.Plant = C.plant AND A.Material = C.Material 
                                    JOIN " + DbMasterName + @".dbo.tPir_New PN ON PN.Plant = C.plant AND PN.Material = C.Material and PN.Quotation = c.Quotation
                                    AND A.VendorCode1 = C.Vendor AND B.PIRNo = C.InfoRecord and C.ValidTo > @ValidDate
                                    and c.Quotation in (select QuoteNo from TQuoteDetails where  ManagerApprovalStatus =2 and (DIRApprovalStatus=0 or DIRApprovalStatus=1) ) ";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@ValidDate", ValidDate.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        #endregion

                        #region save data bom list raw material baed on effective date
                        if (Session["Rawmaterial"] != null)
                        {
                            DataTable DtRawMat = (DataTable)Session["Rawmaterial"];
                            if (DtRawMat.Rows.Count > 0)
                            {
                                #region create temp table for valid data req list 
                                sql = @"create table ##tempRawmaterial
                                 (
                                    material nvarchar(500)  COLLATE DATABASE_DEFAULT,MaterialDesc nvarchar(500)  COLLATE DATABASE_DEFAULT,Unit nvarchar(500)  COLLATE DATABASE_DEFAULT,Amount nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                    Venor_Crcy nvarchar(500)  COLLATE DATABASE_DEFAULT,Selling_Crcy nvarchar(500)  COLLATE DATABASE_DEFAULT,OAmount nvarchar(500)  COLLATE DATABASE_DEFAULT,ExchRate nvarchar(500)  COLLATE DATABASE_DEFAULT,UOM nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                    ValidFrom nvarchar(500)  COLLATE DATABASE_DEFAULT, ValidFromOri nvarchar(500)  COLLATE DATABASE_DEFAULT,[Req No] nvarchar(500)  COLLATE DATABASE_DEFAULT,QuoteNo nvarchar(500)  COLLATE DATABASE_DEFAULT,[CusMatValFrom] nvarchar(500)  COLLATE DATABASE_DEFAULT,[CusMatValTo] nvarchar(500)  COLLATE DATABASE_DEFAULT 
                                 )  ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();

                                if (DtRawMat.Rows.Count > 0)
                                {
                                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EmetCon, SqlBulkCopyOptions.Default, EmetTrans))
                                    {
                                        //Give your Destination table name 
                                        sqlBulk.DestinationTableName = "##tempRawmaterial";
                                        sqlBulk.BulkCopyTimeout = 0;
                                        sqlBulk.BatchSize = 10000;
                                        sqlBulk.WriteToServer(DtRawMat);
                                    }
                                }

                                #region Generate manual query to insert data to temp table
                                //string temptabledatafortrans = @"";
                                //for (int i = 0; i < dtRegList.Rows.Count; i++)
                                //{
                                //    temptabledatafortrans += @" insert into #tempRawmaterial(material,MaterialDesc,Unit,Amount,Venor_Crcy,Selling_Crcy,OAmount,ExchRate,UOM,ValidFrom,ValidFromOri,
                                //    [Req No],QuoteNo,[CusMatValFrom],[CusMatValTo])
                                //        values('" + dtRegList.Rows[i]["material"].ToString() + @"','" + dtRegList.Rows[i]["MaterialDesc"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["Unit"].ToString() + @"','" + dtRegList.Rows[i]["Amount"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["Venor_Crcy"].ToString() + @"','" + dtRegList.Rows[i]["Selling_Crcy"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["OAmount"].ToString() + @"','" + dtRegList.Rows[i]["ExchRate"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["UOM"].ToString() + @"','" + dtRegList.Rows[i]["ValidFrom"].ToString() + @"','" + dtRegList.Rows[i]["ValidFromOri"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["[Req No]"].ToString() + @"','" + dtRegList.Rows[i]["QuoteNo"].ToString() + @"',
                                //        '" + dtRegList.Rows[i]["[CusMatValFrom]"].ToString() + @"','" + dtRegList.Rows[i]["[CusMatValTo]"].ToString() + @"')
                                //        ";
                                //}
                                #endregion

                                #endregion create temp table for valid data req list 

                                #region insert data to TSMMBOM_RAWMATCost_EffDate
                                sql = @" insert into TSMMBOM_RAWMATCost_EffDate(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                          AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                                          select [Req No],QuoteNo,material,MaterialDesc,OAmount,Selling_Crcy,Amount,Venor_Crcy,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,
                                          ValidFromOri,CURRENT_TIMESTAMP,@CreateBy
                                          from ##tempRawmaterial ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.AddWithValue("@CreateBy", Session["userID"].ToString());
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region delete temp table
                                sql = @" drop table ##tempRawmaterial";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();
                                #endregion
                            }
                        }
                        #endregion

                        #region save data bom list raw material before effective date
                        if (Session["OldRawmaterial"] != null)
                        {
                            DataTable dtRawmat = (DataTable)Session["OldRawmaterial"];
                            if (dtRawmat.Rows.Count > 0)
                            {
                                #region create temp table for valid data req list 
                                sql = @"create table ##tempOldRawmaterial
                                 (
                                    [Req No] nvarchar(500)  COLLATE DATABASE_DEFAULT,QuoteNo nvarchar(500)  COLLATE DATABASE_DEFAULT, [Vendor Code] nvarchar(500)  COLLATE DATABASE_DEFAULT,Plant nvarchar(500)  COLLATE DATABASE_DEFAULT, 
                                    [Comp Material] nvarchar(500)  COLLATE DATABASE_DEFAULT,[Comp Material Desc] nvarchar(500)  COLLATE DATABASE_DEFAULT,[Vendor Name] nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                    Ven_PIC nvarchar(500)  COLLATE DATABASE_DEFAULT,Selling_Crcy nvarchar(500)  COLLATE DATABASE_DEFAULT,Amt_SCur nvarchar(500)  COLLATE DATABASE_DEFAULT,ExchRate nvarchar(500)  COLLATE DATABASE_DEFAULT,Venor_Crcy nvarchar(500)  COLLATE DATABASE_DEFAULT, Amt_VCur nvarchar(500)  COLLATE DATABASE_DEFAULT,
                                    Unit nvarchar(500)  COLLATE DATABASE_DEFAULT,UOM nvarchar(500)  COLLATE DATABASE_DEFAULT,ValidFrom nvarchar(500)  COLLATE DATABASE_DEFAULT,ValidFromOri nvarchar(500)  COLLATE DATABASE_DEFAULT,[CusMatValFrom] nvarchar(500)  COLLATE DATABASE_DEFAULT,[CusMatValTo] nvarchar(500)  COLLATE DATABASE_DEFAULT,RN nvarchar(500)  COLLATE DATABASE_DEFAULT
                                 )  ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();

                                if (dtRawmat.Rows.Count > 0)
                                {
                                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EmetCon, SqlBulkCopyOptions.Default, EmetTrans))
                                    {
                                        //Give your Destination table name 
                                        sqlBulk.DestinationTableName = "##tempOldRawmaterial";
                                        sqlBulk.BulkCopyTimeout = 0;
                                        sqlBulk.BatchSize = 10000;
                                        sqlBulk.WriteToServer(dtRawmat);
                                    }
                                }
                                #endregion create temp table for valid data req list 

                                #region insert data to TSMNBOM_RAWMATCost
                                sql = @" insert into TSMNBOM_RAWMATCost(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                         AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy)
                                          select [Req No],QuoteNo,[Comp Material],[Comp Material Desc],Amt_SCur,Selling_Crcy,Amt_VCur,
                                          Venor_Crcy,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,ValidFromOri,CURRENT_TIMESTAMP,@CreateBy
                                          from ##tempOldRawmaterial ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.CommandTimeout = 0;
                                cmd.Parameters.AddWithValue("@CreateBy", Session["userID"].ToString());
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region delete temp table
                                sql = @" drop table ##tempOldRawmaterial";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.CommandTimeout = 0;
                                cmd.ExecuteNonQuery();
                                #endregion
                            }
                        }
                        #endregion

                        #region delete temp table
                        sql = @" drop table #tempRegListemet";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                        #endregion
                        
                        EmetTrans.Commit();
                        EmetTrans.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                EmetTrans.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }
        
        protected void SendingMail()
        {
            sendingmail = false;
            SqlTransaction transaction11 = null;
            errmsg = "";
            try
            {
                DataTable DtForSubmit = new DataTable();
                //Table dtTable1 = new Table();
                string Userid = string.Empty;
                string password = string.Empty;
                string domain = string.Empty;
                string path = string.Empty;
                string URL = string.Empty;
                string MasterDB = string.Empty;
                string TransDB = string.Empty;

                string formatstatus = string.Empty;
                string remarks = string.Empty;

                if (Session["DtForSubmit"] != null)
                {
                    DtForSubmit = (DataTable)Session["DtForSubmit"];
                }

                if (DtForSubmit.Rows.Count > 0)
                {
                    for (int t = 0; t < DtForSubmit.Rows.Count; t++)
                    {
                        string ReqNumber = DtForSubmit.Rows[t]["RequestNumber"].ToString();
                        string QuoteNo = DtForSubmit.Rows[t]["QuoteNo"].ToString();
                        string SAPPartCode = DtForSubmit.Rows[t]["Material"].ToString();
                        string SAPPartDesc = DtForSubmit.Rows[t]["MaterialDesc"].ToString();
                        remarks = "Open Status";
                        //rowscount++;
                        DateTime QuoteDate = DateTime.ParseExact(TxtResDueDate.Text, "dd/MM/yyyy", null);

                        #region getting Messageheader ID from IT Mailapp
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMailConnString()))
                        {
                            cnn.Open();
                            SqlTransaction transactionHS;
                            transactionHS = cnn.BeginTransaction("HeaderSelection");
                            try
                            {
                                string returnValue = string.Empty;

                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.Transaction = transactionHS;

                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.spGetControlNumber";

                                SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                CompanyCode.Direction = ParameterDirection.Input;
                                CompanyCode.Value = 1;
                                cmdget.Parameters.Add(CompanyCode);

                                SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                ControlField.Direction = ParameterDirection.Input;
                                ControlField.Value = "MessageHeaderID";
                                cmdget.Parameters.Add(ControlField);

                                SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                Param1.Direction = ParameterDirection.Input;
                                Param1.Value = "";
                                cmdget.Parameters.Add(Param1);

                                SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                Param2.Direction = ParameterDirection.Input;
                                Param2.Value = "";
                                cmdget.Parameters.Add(Param2);

                                SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                Param3.Direction = ParameterDirection.Input;
                                Param3.Value = "";
                                cmdget.Parameters.Add(Param3);

                                SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                Param4.Direction = ParameterDirection.Input;
                                Param4.Value = "";
                                cmdget.Parameters.Add(Param4);

                                SqlParameter pOutput = cmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                pOutput.Direction = ParameterDirection.Output;

                                cmdget.ExecuteNonQuery();
                                transactionHS.Commit();
                                returnValue = pOutput.Value.ToString();

                                OriginalFilename = returnValue;
                                Session["OriginalFilename"] = returnValue;
                                MHid = returnValue;
                                Session["MHid"] = returnValue;
                                OriginalFilename = MHid + seqNo + formatW;
                                Session["OriginalFilename"] = MHid + seqNo + formatW;
                            }
                            catch (Exception cc2)
                            {
                                errmsg += "Quote NO : " + QuoteNo+ "function getting Messageheader ID from IT Mailapp:" + "Msg Err :" + cc2.ToString() + "\n";
                                transactionHS.Rollback();
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        #region Uploading  ttachment to Mail sever using UNC credentials
                        fname = "";
                        //if (Session["FlAtc" + QuoteNoRef] != null)
                        //{
                        //    FlAttachment = (FileUpload)Session["FlAtc" + QuoteNoRef];
                        //    if (FlAttachment.HasFile)
                        //    {
                        //        fname = FlAttachment.PostedFile.FileName;
                        //    }

                        //}
                        if (fname != "")
                        {
                            try
                            {
                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    Email_inser.Open();
                                    SqlCommand sqlCmd = new SqlCommand("Email_UNC", Email_inser);
                                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    SqlDataReader dr;
                                    dr = sqlCmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        Userid = dr.GetString(0);
                                        password = dr.GetString(1);
                                        domain = dr.GetString(2);
                                        path = dr.GetString(3);
                                        Session["path"] = dr.GetString(3);
                                        URL = dr.GetString(4);
                                        MasterDB = dr.GetString(5);
                                        TransDB = dr.GetString(6);
                                    }
                                    dr.Dispose();
                                    Email_inser.Dispose();

                                    string Destination = Session["path"].ToString() + Session["OriginalFilename"];
                                    using (new SoddingNetworkAuth(Userid, domain, password))
                                    {
                                        try
                                        {
                                            string folderPath = Server.MapPath("~/Files/");
                                            if (!Directory.Exists(folderPath))
                                            {
                                                Directory.CreateDirectory(folderPath);
                                            }
                                            string filename = "";
                                            string PathAndFileName = "";
                                            //if (FlAttachment.HasFile)
                                            //{
                                            //    filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                                            //    PathAndFileName = folderPath + filename;
                                            //    FlAttachment.SaveAs(PathAndFileName);
                                            //}
                                            Source = PathAndFileName;
                                            File.Copy(Source, Destination, true);
                                            SendFilename = fname.Remove(fname.Length - 4);
                                            Session["SendFilename"] = fname.Remove(fname.Length - 4);
                                            OriginalFilename = Session["OriginalFilename"].ToString();
                                            OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                                            Session["OriginalFilename"] = OriginalFilename.ToString();
                                            format = "pdf";
                                            FileInfo file = new FileInfo(PathAndFileName);
                                            if (file.Exists) //check file exsit or not  
                                            {
                                                file.Delete();
                                            }
                                        }
                                        catch (Exception xw1)
                                        {
                                            IsAttachFile = false;
                                            SendFilename = "NOFILE";
                                            OriginalFilename = "NOFILE";
                                            Session["OriginalFilename"] = "NOFILE";
                                            Session["SendFilename"] = "NOFILE";
                                            format = "NO";
                                            errmsg += "Quote NO : " + QuoteNo + "Mail Attachment Failed: " + "Msg Err :" + xw1.ToString() + "\n";
                                            //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Attachment Failed: " + xw1 + " ");
                                            //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                    }
                                }
                            }
                            catch (Exception x)
                            {
                                string message = x.Message;
                            }
                        }

                        else
                        {
                            IsAttachFile = false;
                            SendFilename = "NOFILE";
                            OriginalFilename = "NOFILE";
                            Session["OriginalFilename"] = "NOFILE";
                            Session["SendFilename"] = "NOFILE";
                            format = "NO";
                        }
                        #endregion

                        #region getting vendor mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            cnn.Open();
                            try
                            {
                                string returnValue = string.Empty;
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Email_vendordetails";

                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.Decimal);
                                vendorid.Direction = ParameterDirection.Input;
                                vendorid.Value = DtForSubmit.Rows[t]["VendorCode"].ToString();
                                cmdget.Parameters.Add(vendorid);

                                SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                plant.Direction = ParameterDirection.Input;
                                plant.Value = Session["EPlant"].ToString();
                                cmdget.Parameters.Add(plant);

                                SqlDataReader dr;
                                dr = cmdget.ExecuteReader();
                                while (dr.Read())
                                {
                                    aemail = dr.GetString(0);
                                    Session["aemail"] = dr.GetString(0);
                                    pemail = dr.GetString(1);
                                    Session["pemail"] = dr.GetString(1);

                                }
                                dr.Dispose();
                            }
                            catch (Exception exx)
                            {
                                errmsg += "Quote NO : " + QuoteNo + "function getting vendor mail id," + "Msg Err :" + exx.ToString() + "\n";
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        #region getting User mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            try
                            {
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Email_userdetails";

                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                                vendorid.Direction = ParameterDirection.Input;
                                //vendorid.Value = userId;
                                vendorid.Value = Session["userID"].ToString();
                                cmdget.Parameters.Add(vendorid);

                                SqlDataReader dr;
                                dr = cmdget.ExecuteReader();
                                while (dr.Read())
                                {
                                    Uemail = dr.GetString(0);
                                    Session["Uemail"] = dr.GetString(0);
                                    Session["dept"] = dr.GetString(1);
                                }
                                dr.Dispose();
                            }
                            catch (Exception exc)
                            {
                                errmsg += "Quote : " + DtForSubmit.Rows[t]["QuoteNo"].ToString() + "function :getting User mail id " + "Msg Err :" + exc.ToString() + "\n";
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        #region Insert header and details to Mil server table to IT mailserverapp
                        try
                        {
                            using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                            {
                                Email_inser.Open();
                                //Header
                                string MessageHeaderId = Session["MHid"].ToString();
                                string fromname = "eMET System";
                                //string FromAddress = Uemail;
                                string FromAddress = "eMET@Shimano.Com.sg";
                                //string Recipient = aemail + "," + pemail;
                                aemail = Session["aemail"].ToString();
                                pemail = Session["pemail"].ToString();
                                Uemail = Session["Uemail"].ToString();
                                pemail = string.Concat(aemail, ";", pemail);
                                string Recipient = pemail;
                                string CopyRecipient = Uemail;
                                string BlindCopyRecipient = "";
                                string ReplyTo = "subashdurai@shimano.com.sg";
                                string Subject = "MailCenter from MES";
                                string footer = "Please <a href=" + Convert.ToString(URL.ToString()) + ">Login</a> SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation been created by " + lbluser1.Text + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + DtForSubmit.Rows[t]["VendorName"].ToString() + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNo + "<br />  Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response Due Date    :   " + QuoteDate.ToString("dd/MM/yyyy") + "<br /><br />" + footer;
                                body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + DtForSubmit.Rows[t]["VendorName"].ToString() + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNo + " <br /> Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response due Date    :   " + QuoteDate.ToString("dd/MM/yyyy") + "<br /><br />" + footer;
                                string BodyFormat = "HTML";
                                string BodyRemark = "0";
                                string Signature = "";
                                string Importance = "High";
                                string Sensitivity = "Confidential";
                                string CreateUser = lbluser1.Text;
                                DateTime CreateDate = Convert.ToDateTime(DateTime.Now);
                                //end Header

                                SqlTransaction transactionHe;
                                transactionHe = Email_inser.BeginTransaction("Header");
                                try
                                {
                                    string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                    SqlCommand Header = new SqlCommand(Head, Email_inser);
                                    Header.Connection = Email_inser;
                                    Header.Transaction = transactionHe;
                                    Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                    Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                    Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                    Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                    Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                    Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                    Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                    Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                    Header.Parameters.AddWithValue("@body", body.ToString());
                                    Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                    Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                    Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                    Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                    Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                    Header.Parameters.AddWithValue("@IsAttachFile", Convert.ToBoolean(IsAttachFile.ToString()));
                                    Header.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                    Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    Header.CommandText = Head;
                                    Header.ExecuteNonQuery();
                                    transactionHe.Commit();
                                    //end Header
                                    //Details
                                }
                                catch (Exception cc2)
                                {
                                    transactionHe.Rollback();
                                    errmsg += "Quote : " + QuoteNo + "failed sending email Header " + "Msg Err :" + cc2.ToString() + "\n";
                                    //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + cc2 + " ");
                                    //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }

                                SqlTransaction transactionDe;
                                transactionDe = Email_inser.BeginTransaction("Detail");
                                try
                                {
                                    if (Session["SendFilename"] != null)
                                    {
                                        SendFilename = Session["SendFilename"].ToString();
                                    }
                                    string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                    SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                    Detail.Connection = Email_inser;
                                    Detail.Transaction = transactionDe;
                                    Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                    Detail.Parameters.AddWithValue("@SequenceNumber", Convert.ToInt32(SequenceNumber.ToString()));
                                    Detail.Parameters.AddWithValue("@OriginalFilename", Session["OriginalFilename"].ToString());
                                    Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                    Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                    Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                    Detail.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                    Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    Detail.CommandText = Details;
                                    Detail.ExecuteNonQuery();
                                    transactionDe.Commit();
                                }
                                catch (Exception cc1)
                                {
                                    transactionDe.Rollback();
                                    errmsg += "Quote : " + QuoteNo + " failed sending email detail: " + "Msg Err :" + cc1.ToString() + "\n";
                                    //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email detail: " + cc1 + " ");
                                    //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                Email_inser.Dispose();
                                
                                using (SqlConnection Email_inser1 = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    Email_inser1.Open();
                                    try
                                    {
                                        transaction11 = Email_inser1.BeginTransaction("Mail11");
                                        string Details = "insert into email(quotenumber, body) values(@Quotenumber,@body)";
                                        SqlCommand Detail = new SqlCommand(Details, Email_inser1);
                                        Detail.Connection = Email_inser1;
                                        Detail.Transaction = transaction11;
                                        Detail.Parameters.AddWithValue("@Quotenumber", QuoteNo);
                                        Detail.Parameters.AddWithValue("@body", body1.ToString());
                                        Detail.CommandText = Details;
                                        Detail.ExecuteNonQuery();
                                        transaction11.Commit();
                                    }
                                    catch (Exception cc1)
                                    {
                                        transaction11.Rollback();
                                        errmsg += "Quote : " + QuoteNo + " Mail Content Issue in Transaction table " + "Msg Err :" + cc1.ToString() + "\n";
                                        //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Content Issue in Transaction table: " + cc1 + " ");
                                        //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    Email_inser1.Dispose();
                                }
                                //End Details
                            }

                        }
                        catch (Exception cc1)
                        {
                            errmsg += "Quote : " + QuoteNo + "failed sending email " + "Msg Err :" + cc1.ToString() + "\n";
                            //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + cc1 + " ");
                            //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                errmsg += "failed sending email from beginning process" + "Msg Err :" + ex.ToString() + "\n";
            }

            if (errmsg != "")
            {
                sendingmail = false;
            }
            else
            {
                sendingmail = true;
            }
        }

        protected void UploadData()
        {
            isUploadScs = false;
            string sql = "";
            OleDbCommand oleDBcmd = null;
            SqlCommand cmd = null;
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlTransaction MDMTrans = null;
            DataTable dtSource = null;
            DataTable dtInvalid = null;
            DataTable dtAllData = null;
            DateTime curTime = DateTime.Now;
            string excelCols = "[Plant],[PIR No],[Material Code],[Material Desc],[Vendor Code],[Vendor Name],[Process Group]";
            string excelRange = "A1:G1048576";
            string excelFirstCol = "[Plant]";
            string excelSecondtCol = "[PIR No]";

            string FileName = "MassmaterialRevision";
            string FileExtension = System.IO.Path.GetExtension(FlUpload.FileName);
            string folderPath = Server.MapPath("~/Files/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            //string path = string.Concat(Server.MapPath("~/Files/" + FlUpload.FileName));
            string PathNFileName = folderPath + FileName + FileExtension;
            FlUpload.SaveAs(PathNFileName);
            try
            {
                dtSource = new DataTable();
                dtInvalid = new DataTable();
                dtAllData = new DataTable();
                MDMCon.Open();
                MDMTrans = MDMCon.BeginTransaction();
                sql = @"create table #temp1
                            (
                                Plant nvarchar (500),
	                            PIRNo nvarchar (500),
	                            MaterialCode nvarchar (500),
                                MaterialDesc nvarchar (500),
	                            VendorCode nvarchar (500),
                                VendorName nvarchar (500),
	                            ProcessGroup nvarchar (500)
                            ) ";
                //sql = @" Delete From TestingUpload";
                cmd = new SqlCommand(sql,MDMCon, MDMTrans);
                cmd.ExecuteNonQuery();

                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", PathNFileName);
                //Create Connection to Excel work book 
                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                {
                    //Create OleDbCommand to fetch data from Excel 
                    //using (oleDBcmd = new OleDbCommand("select * from [Sheet1$]", excelConnection))
                    using (oleDBcmd = new OleDbCommand("Select " + excelCols + " from [Sheet1$" + excelRange + "] where " + excelFirstCol + " is not null or " + excelSecondtCol + " is not null ", excelConnection))
                    {
                        excelConnection.Open();
                        using (OleDbDataReader dReader = oleDBcmd.ExecuteReader())
                        {
                            using (SqlBulkCopy sqlBulk = new SqlBulkCopy(MDMCon, SqlBulkCopyOptions.Default, MDMTrans))
                            {
                                //Give your Destination table name 
                                sqlBulk.DestinationTableName = "#temp1";
                                //sqlBulk.DestinationTableName = "TestingUpload";
                                sqlBulk.BulkCopyTimeout = 0;
                                sqlBulk.BatchSize = 10000;
                                sqlBulk.WriteToServer(dReader);
                            }
                        }
                    }
                }

                oleDBcmd.Dispose();

                //All Data
                #region All Data
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select A.Plant,A.PIRNo,A.MaterialCode,(select distinct MaterialDesc from TMATERIAL where Material=A.MaterialCode) as MaterialDesc,
                    //        A.VendorCode,(select distinct Description from tVendor_New where Vendor=A.VendorCode) as VendorName,A.ProcessGroup
                    //         from #temp1 A ";

                    sql = @" select * from #temp1 ";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            #region populate valid data to grid
                            Session["DtAllData"] = dt;
                            GdvAllData.DataSource = dt;
                            int ShowEntry = 1;
                            if (TxtShowEntryAllData.Text == "" || TxtShowEntryAllData.Text == "0")
                            {
                                ShowEntry = 1;
                                TxtShowEntryAllData.Text = "1";
                            }
                            else
                            {
                                ShowEntry = Convert.ToInt32(TxtShowEntryAllData.Text);
                            }
                            GdvAllData.PageSize = ShowEntry;
                            GdvAllData.DataBind();
                            if (GdvAllData.Rows.Count <= 0)
                            {
                                DvAllDataUpload.Visible = false;
                                GdvAllData.Visible = false;
                                DvProceed.Visible = false;
                                LbTitleAllData.Text = "All Data (0 Record)";
                            }
                            else
                            {
                                DvAllDataUpload.Visible = true;
                                DvProceed.Visible = true;
                                GdvAllData.Visible = true;
                                LbTitleAllData.Text = "All Data (" + dt.Rows.Count + " Record)";
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                sql = @" drop table #temp1 ";
                cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                cmd.ExecuteNonQuery();

                MDMTrans.Commit();

                isUploadScs = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                isUploadScs = false;
                MDMTrans.Rollback();
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
        
        bool CheckAllData()
        {
            Session["InvalidRequestExpiredReq"] = null;
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                if (Session["DtAllData"] != null)
                {
                    DataTable DtAllData = (DataTable)Session["DtAllData"];
                    if (DtAllData.Rows.Count > 0)
                    {
                        bool IsHaveExpiredReq = false;
                        for (int h = 0; h < DtAllData.Rows.Count; h++)
                        {
                            if (CekVendorVsMaterialExpiredReq(DtAllData.Rows[h]["VendorCode"].ToString(), DtAllData.Rows[h]["MaterialCode"].ToString()) == false)
                            {
                                IsHaveExpiredReq = true;
                            }
                        }

                        if (IsHaveExpiredReq == false)
                        {
                            #region Continue Cek Data
                            DataTable DtTempValidData = new DataTable();
                            DtTempValidData = DtAllData.Clone();

                            DataTable DtValidData = new DataTable();
                            DtValidData = DtAllData.Clone();
                            DtValidData.Columns.Add("ProcDesc");

                            DataTable DtInValidData = new DataTable();
                            DtInValidData = DtAllData.Clone();
                            DtInValidData.Columns.Add("InvalidDesc");


                            #region get duplicate data
                            string UN2 = "";
                            foreach (DataRow dr in DtAllData.Rows)
                            {
                                string Plant = Convert.ToString(dr["Plant"]);
                                string PIRNo = Convert.ToString(dr["PIRNo"]);
                                string MaterialCode = Convert.ToString(dr["MaterialCode"]);
                                string VendorCode = Convert.ToString(dr["VendorCode"]);
                                string ProcessGroup = Convert.ToString(dr["ProcessGroup"]);
                                //,, "
                                string UN1 = Plant + "-" + PIRNo + "-" + MaterialCode + "-" + VendorCode + "-" + ProcessGroup;

                                int intFoundRows = DtAllData.Select("Plant= '" + Plant + "' AND PIRNo= '" + PIRNo + "' AND MaterialCode= '" + MaterialCode + "' AND VendorCode= '" + VendorCode + "' AND ProcessGroup= '" + ProcessGroup + "' ").Count();
                                if (intFoundRows > 1)
                                {
                                    if (UN1 != UN2)
                                    {
                                        UN2 = Plant + "-" + PIRNo + "-" + MaterialCode + "-" + VendorCode + "-" + ProcessGroup;
                                        DtInValidData.ImportRow(dr);
                                        DtInValidData.Rows[(DtInValidData.Rows.Count - 1)]["InvalidDesc"] = DtInValidData.Rows[(DtInValidData.Rows.Count - 1)]["InvalidDesc"].ToString() + " Duplicate Data";
                                    }
                                    else
                                    {
                                        DtTempValidData.ImportRow(dr);
                                    }
                                }
                                else
                                {
                                    DtTempValidData.ImportRow(dr);
                                }
                            }
                            DtInValidData.AcceptChanges();
                            DtTempValidData.AcceptChanges();
                            #endregion

                            MDMCon.Open();
                            GetDbTrans();
                            for (int i = 0; i < DtTempValidData.Rows.Count; i++)
                            {
                                string PIRNo = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                if (DtTempValidData.Rows[i]["PIRNo"].ToString() == "5300197907")
                                {
                                    string a = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                }
                                string InvalidMessage = "";
                                bool isValid = true;
                                #region Cek in emet quote detail
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    sql = @" select distinct TQ.PIRNo from " + DbTransName + @"TQuoteDetails TQ 
                                        where (TQ.PIRNo is not null) 
                                        and (TQ.ApprovalStatus not in ('3','1'))
                                        and TQ.PIRNo =@PIRNo ";

                                    cmd = new SqlCommand(sql, MDMCon);
                                    cmd.Parameters.AddWithValue("@PIRNo", PIRNo);
                                    sda.SelectCommand = cmd;
                                    using (DataTable dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            InvalidMessage = "- already in request progress.";
                                            isValid = false;
                                        }
                                    }
                                }
                                #endregion

                                #region Cek is material and vendor already in progress
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd/MM/yyyy') as RequestDate,format(QuoteResponseDueDate,'dd/MM/yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from " + DbTransName + @"TQuoteDetails where Material = @Material and VendorCode1 =@VendorCode1 and ApprovalStatus in ('0','2') and format(QuoteResponseDueDate,'yyyy-MM-dd') > FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') ";

                                    cmd = new SqlCommand(sql, MDMCon);
                                    cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                    cmd.Parameters.AddWithValue("@VendorCode1", DtTempValidData.Rows[i]["VendorCode"].ToString());
                                    sda.SelectCommand = cmd;
                                    using (DataTable dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        if (dt.Rows.Count > 0)
                                        {
                                            InvalidMessage = "- Vendor with this material is in progress.";
                                            isValid = false;
                                        }
                                    }
                                }
                                #endregion

                                if (isValid == true)
                                {
                                    #region Cek in Plant Table
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select distinct * from TPLANT where Plant = @Plant and DelFlag = 0 ";

                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Plant", DtTempValidData.Rows[i]["Plant"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += "- The Plant not exist in Master Data Plant .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in PIR new Table
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select distinct * from tPir_New where Material = @Material and Vendor = @VendorCode and DelFlag = 0 ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                        cmd.Parameters.AddWithValue("@VendorCode", DtTempValidData.Rows[i]["VendorCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - The Material and Vendor not exist in PIR .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in PIR vs Quotation Table
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select distinct InfoRecord from tPIRvsQuotation
                                        where Plant=@Plant and Material=@Material and Vendor=@VendorCode
                                        and DelFlag = 0 ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Plant", DtTempValidData.Rows[i]["Plant"].ToString());
                                        cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                        cmd.Parameters.AddWithValue("@VendorCode", DtTempValidData.Rows[i]["VendorCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - This PIR No not exist in PIR Vs Quotation .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek Job Type in PIR table
                                    //pending
                                    //using (SqlDataAdapter sda = new SqlDataAdapter())
                                    //{
                                    //    sql = @" select distinct jobtyp from tPir_New TP 
                                    //              inner join TPROCESGROUP_LIST TPG on TP.jobtyp = TPG.Process_Grp_code
                                    //             where Material = @Material and Vendor=@Vendor and Plant=@Plant and TP.DelFlag = 0 and TPG.DelFlag=0 ";

                                    //    cmd = new SqlCommand(sql, conMaster);
                                    //    cmd.Parameters.AddWithValue("@Material", DtInValidData.Rows[i]["MaterialCode"].ToString());
                                    //    cmd.Parameters.AddWithValue("@Vendor", DtInValidData.Rows[i]["VendorCode"].ToString());
                                    //    cmd.Parameters.AddWithValue("@Plant", DtInValidData.Rows[i]["Plant"].ToString());
                                    //    sda.SelectCommand = cmd;
                                    //    using (DataTable dt = new DataTable())
                                    //    {
                                    //        sda.Fill(dt);
                                    //        if (dt.Rows.Count <= 0)
                                    //        {
                                    //            if (Session["DtValidData"] != null)
                                    //            {
                                    //                DataTable DtVD = (DataTable)Session["DtValidData"];
                                    //            }
                                    //            InvalidMessage += " - PIR Job Type not exist in Process Group Data .";
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    #region Cek in BOM table
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select top 1 * from TBOMLISTnew where [Parent Material] = @Material ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - The Material not exist in BOM .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in Process Group table
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select top 1 * from TPROCESGROUP_LIST where Process_Grp_code = @ProcessGroup and DelFlag = 0  ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@ProcessGroup", DtTempValidData.Rows[i]["ProcessGroup"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - The Process group not exist in Process Group Data .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in table material
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select top 1 * from TMATERIAL where Material = @Material  and DelFlag = 0 ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - The Material not exist in Material Data .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in table Vendor
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select top 1 * from tVendor_New where Vendor = @VendorCode  and DelFlag = 0 ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Material", DtTempValidData.Rows[i]["MaterialCode"].ToString());
                                        cmd.Parameters.AddWithValue("@VendorCode", DtTempValidData.Rows[i]["VendorCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - The Vendor not exist in Vendor Data .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Cek in table Vendor PORG
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        sql = @" select top 1 * from tVendorPOrg where Vendor = @VendorCode and Plant=@Plant and DelFlag = 0 ";
                                        cmd.CommandTimeout = 10000;
                                        cmd = new SqlCommand(sql, MDMCon);
                                        cmd.Parameters.AddWithValue("@Plant", DtTempValidData.Rows[i]["Plant"].ToString());
                                        cmd.Parameters.AddWithValue("@VendorCode", DtTempValidData.Rows[i]["VendorCode"].ToString());
                                        sda.SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            sda.Fill(dt);
                                            if (dt.Rows.Count <= 0)
                                            {
                                                InvalidMessage += " - Vendor not exist in Vendor PORG Data .";
                                                isValid = false;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Get data relation in all table related
                                    if (isValid == true)
                                    {
                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                        {
                                            string Plant = DtTempValidData.Rows[i]["Plant"].ToString();
                                            PIRNo = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                            string MaterialCode = DtTempValidData.Rows[i]["MaterialCode"].ToString();
                                            string VendorCode = DtTempValidData.Rows[i]["VendorCode"].ToString();
                                            string ProcessGroup = DtTempValidData.Rows[i]["ProcessGroup"].ToString();

                                            //    sql = @" SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                            //    PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                            //    PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc
                                            //    FROM tPir_New PN
                                            //    INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                            //    INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                            //    INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                            //    INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                            //    Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                            //    INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                            //    inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType
                                            //    INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                            //    INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material
                                            //    INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                            //    inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                            //--WHERE PQ.InfoRecord=@PIRNo and PN.Material=@MaterialCode and PJPG.ProcessGrpcode=@ProcessGroup and TV.Vendor =@VendorCode 
                                            //WHERE PQ.InfoRecord='" + PIRNo + "' and PN.Material='" + MaterialCode + "' and PJPG.ProcessGrpcode='" + ProcessGroup + "' and TV.Vendor ='" + VendorCode + @"'
                                            //";

                                            //sql = @" ---firt level
                                            //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                            //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                            //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc
                                            //        FROM tPir_New PN
                                            //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                            //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                            //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                            //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                            //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                            //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                            //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType
                                            //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                            //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                                            //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                            //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                            //        -- this where condition based on each record from exel file
                                            //        --WHERE PQ.InfoRecord=@PIRNo and PN.Material=@MaterialCode and PJPG.ProcessGrpcode=@ProcessGroup and TV.Vendor =@VendorCode
                                            //        WHERE PQ.InfoRecord='" + PIRNo + "' and PN.Material='" + MaterialCode + "' and PJPG.ProcessGrpcode='" + ProcessGroup + "' and TV.Vendor ='" + VendorCode + @"'
                                            //        ";

                                            //16/10/2019
                                            //sql = @" ---First Level Query
                                            //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                            //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                            //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc
                                            //        FROM tPir_New PN
                                            //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                            //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                            //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                            //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                            //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                            //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                            //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                            //        and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                            //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                            //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                                            //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                            //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                            //        -- this where condition based on each record from exel file
                                            //        --WHERE PQ.InfoRecord=@PIRNo and PN.Material=@MaterialCode and PJPG.ProcessGrpcode=@ProcessGroup and TV.Vendor =@VendorCode and tm.Plant=@Plant
                                            //    WHERE PQ.InfoRecord='" + PIRNo + "' and PN.Material='" + MaterialCode + "' and PJPG.ProcessGrpcode='" + ProcessGroup + "' and TV.Vendor ='" + VendorCode + @"' 
                                            //    and tm.Plant='" + Plant + @"'
                                            //    ";

                                            //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                                            sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into #complist1 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            and [header valid to date] > getdate() and [header valid from date] <= getdate()
                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into #e50 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            and [header valid to date] > getdate() and [header valid from date] <= getdate() 
                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()  
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'

                                            IF ((select count (*) from #e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > getdate() and [header valid from date] <= getdate()
	                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from #e50
	                                            delete from #e50
	
	                                            insert into #e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > getdate() and [header valid from date] <= getdate()  
	                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()  
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from #e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";
                                            sql += @" SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                                PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                                PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc
                                                FROM tPir_New PN
                                                INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                                INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                                INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                                INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                                Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                                INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                                inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                                and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                                INNER JOIN #complist1 TB ON TM.Material = TB.[Parent Material]
                                                INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                                INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                                inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                                -- this where condition based on each record from exel file
                                                --WHERE PQ.InfoRecord=@PIRNo and PN.Material=@mat and PJPG.ProcessGrpcode=@ProcessGroup and TV.Vendor =@VendorCode and tm.Plant=@plant
                                                WHERE PQ.InfoRecord='" + PIRNo + "' and PN.Material='" + MaterialCode + "' and PJPG.ProcessGrpcode='" + ProcessGroup + "' and TV.Vendor ='" + VendorCode + @"' 
                                                and tm.Plant='" + Plant + @"'
                                                ";
                                            sql += " drop table #complist1, #e50 ";
                                            cmd = new SqlCommand(sql, MDMCon);
                                            cmd.CommandTimeout = 10000;
                                            cmd.Parameters.AddWithValue("@PIRNo", PIRNo);
                                            cmd.Parameters.AddWithValue("@mat", MaterialCode);
                                            cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                                            cmd.Parameters.AddWithValue("@ProcessGroup", ProcessGroup);
                                            cmd.Parameters.AddWithValue("@plant", Plant);
                                            sda.SelectCommand = cmd;
                                            using (DataTable dt = new DataTable())
                                            {
                                                sda.Fill(dt);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    DataRow dr = DtValidData.NewRow();
                                                    dr["Plant"] = DtTempValidData.Rows[i]["Plant"].ToString();
                                                    dr["PIRNo"] = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                                    dr["MaterialCode"] = DtTempValidData.Rows[i]["MaterialCode"].ToString();
                                                    dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
                                                    dr["VendorCode"] = DtTempValidData.Rows[i]["VendorCode"].ToString();
                                                    dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                                                    dr["ProcessGroup"] = DtTempValidData.Rows[i]["ProcessGroup"].ToString();
                                                    dr["ProcDesc"] = dt.Rows[0]["ProcDesc"].ToString();
                                                    DtValidData.Rows.Add(dr);
                                                }
                                                else
                                                {
                                                    DataRow drInvalid = DtInValidData.NewRow();
                                                    drInvalid["Plant"] = DtTempValidData.Rows[i]["Plant"].ToString();
                                                    drInvalid["PIRNo"] = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                                    drInvalid["MaterialCode"] = DtTempValidData.Rows[i]["MaterialCode"].ToString();
                                                    drInvalid["MaterialDesc"] = DtTempValidData.Rows[0]["MaterialDesc"].ToString();
                                                    drInvalid["VendorCode"] = DtTempValidData.Rows[i]["VendorCode"].ToString();
                                                    drInvalid["VendorName"] = DtTempValidData.Rows[i]["VendorName"].ToString();
                                                    drInvalid["ProcessGroup"] = DtTempValidData.Rows[i]["ProcessGroup"].ToString();
                                                    drInvalid["InvalidDesc"] = InvalidMessage + @"Based On Material and Vendor Code This PIR Not Exist in PIR Data, 
                                                                     Please check data in PIR New, PIR vs Quotation , Material , Process group List , BOM data and Customer Material Pricing  ";
                                                    DtInValidData.Rows.Add(drInvalid);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow drInvalid = DtInValidData.NewRow();
                                        drInvalid["Plant"] = DtTempValidData.Rows[i]["Plant"].ToString();
                                        drInvalid["PIRNo"] = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                        drInvalid["MaterialCode"] = DtTempValidData.Rows[i]["MaterialCode"].ToString();
                                        drInvalid["MaterialDesc"] = DtTempValidData.Rows[0]["MaterialDesc"].ToString();
                                        drInvalid["VendorCode"] = DtTempValidData.Rows[i]["VendorCode"].ToString();
                                        drInvalid["VendorName"] = DtTempValidData.Rows[i]["VendorName"].ToString();
                                        drInvalid["ProcessGroup"] = DtTempValidData.Rows[i]["ProcessGroup"].ToString();
                                        drInvalid["InvalidDesc"] = InvalidMessage + @"Based On Material and Vendor Code This PIR Not Exist in PIR Data, 
                                                                     Please check data in PIR New, PIR vs Quotation , Material , Process group List , BOM data and Customer Material Pricing  ";
                                        DtInValidData.Rows.Add(drInvalid);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    DataRow drInvalid = DtInValidData.NewRow();
                                    drInvalid["Plant"] = DtTempValidData.Rows[i]["Plant"].ToString();
                                    drInvalid["PIRNo"] = DtTempValidData.Rows[i]["PIRNo"].ToString();
                                    drInvalid["MaterialCode"] = DtTempValidData.Rows[i]["MaterialCode"].ToString();
                                    drInvalid["MaterialDesc"] = DtTempValidData.Rows[i]["MaterialDesc"].ToString();
                                    drInvalid["VendorCode"] = DtTempValidData.Rows[i]["VendorCode"].ToString();
                                    drInvalid["VendorName"] = DtTempValidData.Rows[i]["VendorName"].ToString();
                                    drInvalid["ProcessGroup"] = DtTempValidData.Rows[i]["ProcessGroup"].ToString();
                                    drInvalid["InvalidDesc"] = InvalidMessage;
                                    DtInValidData.Rows.Add(drInvalid);
                                }
                            }

                            #region populte data invalid to grid
                            if (DtInValidData.Rows.Count > 0)
                            {
                                Session["DtInValidData"] = DtInValidData;
                                GvInvalid.DataSource = DtInValidData;
                                int ShowEntryInvalid = 1;
                                if (TxtShowEntryInvalid.Text == "" || TxtShowEntryInvalid.Text == "0")
                                {
                                    ShowEntryInvalid = 1;
                                    TxtShowEntryInvalid.Text = "1";
                                }
                                else
                                {
                                    ShowEntryInvalid = Convert.ToInt32(TxtShowEntryInvalid.Text);
                                }
                                GvInvalid.PageSize = ShowEntryInvalid;
                                GvInvalid.DataBind();
                                DvInvalidData.Visible = true;
                                LbTitleInvalidData.Text = "Invalid Data (" + DtInValidData.Rows.Count + " Record)";
                            }
                            else
                            {
                                DvInvalidData.Visible = false;
                                LbTitleInvalidData.Text = "Invalid Data 0 Record";
                            }
                            #endregion

                            #region populte data valid to grid
                            if (DtValidData.Rows.Count > 0)
                            {
                                Session["DtValidData"] = DtValidData;
                                GvValidData.DataSource = DtValidData;
                                int ShowEntry = 1;
                                if (TxtShowEntryValid.Text == "" || TxtShowEntryValid.Text == "0")
                                {
                                    ShowEntry = 1;
                                    TxtShowEntryValid.Text = "1";
                                }
                                else
                                {
                                    ShowEntry = Convert.ToInt32(TxtShowEntryValid.Text);
                                }
                                GvValidData.PageSize = ShowEntry;
                                GvValidData.DataBind();
                                DvCreateReq.Visible = true;
                                DvValidData.Visible = true;
                                LbTitleValidData.Text = "Valid Data " + DtValidData.Rows.Count + " Record";
                            }
                            else
                            {
                                DvCreateReq.Visible = false;
                                DvValidData.Visible = false;
                                LbTitleValidData.Text = "Valid Data 0 Record";
                            }
                            #endregion
                            #endregion
                        }
                        else
                        {
                            if (Session["InvalidRequestExpiredReq"] != null)
                            {
                                DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReq"];
                                if (DtTemp.Rows.Count > 0)
                                {
                                    GvDuplicateWithExpiredReq.DataSource = DtTemp;
                                    GvDuplicateWithExpiredReq.DataBind();
                                    SpanRowGvInvalidExpired();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();OpenModalDuplicateExpired();", true);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
        
        protected void CreateRequest()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                Session["Rawmaterial"] = null;
                Session["OldRawmaterial"] = null;

                DeleteNonRequest();
                if (Session["DtValidData"] != null)
                {
                    DataTable DtValidData = (DataTable)Session["DtValidData"];
                    if (DtValidData.Rows.Count > 0)
                    {
                        #region proceed create request
                        DataTable dtRegList = new DataTable();
                        dtRegList.Columns.Add("Plant");
                        dtRegList.Columns.Add("PIRNo");
                        dtRegList.Columns.Add("ReqNo");
                        dtRegList.Columns.Add("MaterialCode");
                        dtRegList.Columns.Add("MaterialDesc");
                        dtRegList.Columns.Add("CodeRef");
                        dtRegList.Columns.Add("VendorCode");
                        dtRegList.Columns.Add("VendorName");
                        dtRegList.Columns.Add("ProcessGroup");
                        dtRegList.Columns.Add("PIRJobType");
                        dtRegList.Columns.Add("CompMaterial");
                        dtRegList.Columns.Add("CompMaterialDesc");
                        dtRegList.Columns.Add("Amount");
                        dtRegList.Columns.Add("UnitofCurrency");
                        dtRegList.Columns.Add("Unit");
                        dtRegList.Columns.Add("UoM");
                        dtRegList.Columns.Add("ValidFrom");
                        dtRegList.Columns.Add("ValidTo");

                        dtRegList.Columns.Add("QuoteNo");
                        dtRegList.Columns.Add("Product");
                        dtRegList.Columns.Add("MaterialClass");
                        dtRegList.Columns.Add("PIRType");
                        dtRegList.Columns.Add("UnitWeight");
                        dtRegList.Columns.Add("UnitWeightUOM");
                        dtRegList.Columns.Add("BaseUOM");
                        dtRegList.Columns.Add("SAPProcType");
                        dtRegList.Columns.Add("SAPSPProcType");
                        dtRegList.Columns.Add("MaterialType");
                        dtRegList.Columns.Add("PlantStatus");
                        dtRegList.Columns.Add("TotMatCost");
                        dtRegList.Columns.Add("TotProcCost");
                        dtRegList.Columns.Add("TotSubMatCost");
                        dtRegList.Columns.Add("TotOthCost");
                        dtRegList.Columns.Add("countryorg");
                        dtRegList.Columns.Add("MassUpdateDate");

                        DataTable dtInValidRegList = new DataTable();
                        dtInValidRegList = DtValidData.Clone();
                        
                        for (int i = 0; i < DtValidData.Rows.Count; i++)
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {

                                //sql = @" SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                //        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                //        TM.UnitWeight,TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                //        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                //        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                //        CAST(ROUND(PN.amt1/PN.per1,5) AS DECIMAL(12,4)) as amt1,
                                //        CAST(ROUND(PN.amt2/PN.per2,5) AS DECIMAL(12,4)) as amt2,
                                //        CAST(ROUND(PN.amt3/PN.per3,5) AS DECIMAL(12,4)) as amt3,
                                //        CAST(ROUND(PN.amt4/PN.per4,5) AS DECIMAL(12,4)) as amt4,
                                //        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                //        FROM tPir_New PN
                                //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType
                                //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material
                                //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                //        where PN.Material = @Material 
                                //        and TCM.ValidFrom <= @ValidTo
                                //        and TCM.ValidTo >= @ValidTo
                                //        and PN.Vendor = @VendorCode 
                                //        order by TCM.ValidTo desc ";

                                //sql = @" --------second level
                                //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                //        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                //        TM.UnitWeight,TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                //        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                //        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                //        CAST(ROUND(PN.amt1/PN.per1,5) AS DECIMAL(12,4)) as amt1,
                                //        CAST(ROUND(PN.amt2/PN.per2,5) AS DECIMAL(12,4)) as amt2,
                                //        CAST(ROUND(PN.amt3/PN.per3,5) AS DECIMAL(12,4)) as amt3,
                                //        CAST(ROUND(PN.amt4/PN.per4,5) AS DECIMAL(12,4)) as amt4,
                                //        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                //        FROM tPir_New PN
                                //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType
                                //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                                //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                //        --where confition based on each record in valid data 
                                //        where PN.Material = @Material 
                                //        and TCM.ValidFrom <= @ValidTo
                                //        and TCM.ValidTo >= @ValidTo
                                //        and PN.Vendor = @VendorCode 
                                //        order by TCM.ValidTo desc
                                //         ";


                                //16/10/2019
                                //sql = @" --------second level
                                //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                //        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                //        TM.UnitWeight,TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                //        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                //        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                //        CAST(ROUND(PN.amt1/PN.per1,5) AS DECIMAL(12,5)) as amt1,
                                //        CAST(ROUND(PN.amt2/PN.per2,5) AS DECIMAL(12,5)) as amt2,
                                //        CAST(ROUND(PN.amt3/PN.per3,5) AS DECIMAL(12,5)) as amt3,
                                //        CAST(ROUND(PN.amt4/PN.per4,5) AS DECIMAL(12,5)) as amt4,
                                //        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                //        FROM tPir_New PN
                                //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                                //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                //        --where confition based on each record in valid data 
                                //        where PN.Material = @Material 
                                //        and TCM.ValidFrom <= @ValidTo
                                //        and TCM.ValidTo >= @ValidTo
                                //        and PN.Vendor = @VendorCode 
                                //        and tm.Plant=@plant
                                //        order by TCM.ValidTo desc
                                //         ";
                                //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                                sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into #complist1 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into #e50 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'

                                            IF ((select count (*) from #e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from #e50
	                                            delete from #e50
	
	                                            insert into #e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from #e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";

                                sql += @" --------second level
                                        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                        TM.UnitWeight,TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                        
                                        CAST(ROUND(PN.amt1/nullif(PN.per1,0),5) AS DECIMAL(12,5)) as amt1,
                                        CAST(ROUND(PN.amt2/nullif(PN.per2,0),5) AS DECIMAL(12,5)) as amt2,
                                        CAST(ROUND(PN.amt3/nullif(PN.per3,0),5) AS DECIMAL(12,5)) as amt3,
                                        CAST(ROUND(PN.amt4/nullif(PN.per4,0),5) AS DECIMAL(12,5)) as amt4,

                                        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                        FROM tPir_New PN
                                        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                        INNER JOIN #complist1 TB ON TM.Material = TB.[Parent Material]
                                        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                        --where confition based on each record in valid data 
                                        where PN.Material = @mat 
                                        and TCM.ValidFrom <= @ValidTo
                                        and TCM.ValidTo >= @ValidTo
                                        and PN.Vendor = @VendorCode 
                                        and tm.Plant=@plant
                                        order by TCM.ValidTo desc
                                         ";
                                sql += " drop table #complist1, #e50 ";
                                cmd = new SqlCommand(sql, MDMCon);
                                cmd.CommandTimeout = 10000;
                                //cmd.Parameters.AddWithValue("@ImmedParent", DtValidData.Rows[i]["MaterialCode"].ToString());
                                cmd.Parameters.AddWithValue("@mat", DtValidData.Rows[i]["MaterialCode"].ToString());
                                cmd.Parameters.AddWithValue("@VendorCode", DtValidData.Rows[i]["VendorCode"].ToString());
                                cmd.Parameters.AddWithValue("@plant", DtValidData.Rows[i]["Plant"].ToString());
                                DateTime ValidTo = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                                cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy/MM/dd"));
                                sda.SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        string RequestIncNumber = GenerateReqNo().ToString();
                                        DataRow dr = dtRegList.NewRow();
                                        dr["Plant"] = DtValidData.Rows[i]["Plant"].ToString();
                                        dr["PIRNo"] = DtValidData.Rows[i]["PIRNo"].ToString();
                                        dr["ReqNo"] = RequestIncNumber;
                                        dr["QuoteNo"] = dt.Rows[0]["CodeRef"].ToString() + RequestIncNumber;
                                        dr["MaterialCode"] = dt.Rows[0]["MaterialCode"].ToString();
                                        dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
                                        dr["CodeRef"] = dt.Rows[0]["CodeRef"].ToString();
                                        dr["VendorCode"] = dt.Rows[0]["VendorCode"].ToString();
                                        dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                                        dr["ProcessGroup"] = DtValidData.Rows[i]["ProcessGroup"].ToString();
                                        dr["PIRJobType"] = dt.Rows[0]["PIRJobType"].ToString();

                                        dr["CompMaterial"] = dt.Rows[0]["CompMaterial"].ToString();
                                        dr["CompMaterialDesc"] = dt.Rows[0]["CompMaterialDesc"].ToString();
                                        dr["Amount"] = dt.Rows[0]["Amount"].ToString();
                                        dr["UnitofCurrency"] = dt.Rows[0]["UnitofCurrency"].ToString();
                                        dr["Unit"] = dt.Rows[0]["Unit"].ToString();
                                        dr["UoM"] = dt.Rows[0]["UoM"].ToString();
                                        dr["BaseUOM"] = dt.Rows[0]["BaseUOM"].ToString();
                                        dr["ValidFrom"] = dt.Rows[0]["ValidFrom"].ToString();
                                        dr["ValidTo"] = dt.Rows[0]["ValidToo"].ToString();

                                        dr["Product"] = dt.Rows[0]["Product"].ToString();
                                        dr["MaterialClass"] = dt.Rows[0]["MaterialClass"].ToString();
                                        dr["PIRType"] = dt.Rows[0]["PIRType"].ToString();
                                        dr["UnitWeight"] = dt.Rows[0]["UnitWeight"].ToString();
                                        dr["UnitWeightUOM"] = dt.Rows[0]["UnitWeightUOM"].ToString();
                                        dr["SAPProcType"] = dt.Rows[0]["SAPProcType"].ToString();
                                        dr["SAPSPProcType"] = dt.Rows[0]["SAPSPProcType"].ToString();
                                        dr["MaterialType"] = dt.Rows[0]["MaterialType"].ToString();
                                        dr["PlantStatus"] = dt.Rows[0]["PlantStatus"].ToString();
                                        dr["TotMatCost"] = dt.Rows[0]["amt1"].ToString();
                                        dr["TotProcCost"] = dt.Rows[0]["amt2"].ToString();
                                        dr["TotSubMatCost"] = dt.Rows[0]["amt3"].ToString();
                                        dr["TotOthCost"] = dt.Rows[0]["amt4"].ToString();
                                        dr["countryorg"] = dt.Rows[0]["countryorg"].ToString();
                                        dr["MassUpdateDate"] = dt.Rows[0]["MassUpdateDate"].ToString();

                                        dtRegList.Rows.Add(dr);

                                        string PIRNo = DtValidData.Rows[i]["PIRNo"].ToString();
                                        string VC = dt.Rows[0]["VendorCode"].ToString();
                                        string QN = dt.Rows[0]["CodeRef"].ToString() + RequestIncNumber;
                                        string VN = dt.Rows[0]["VendorName"].ToString();
                                        string PG = DtValidData.Rows[i]["ProcessGroup"].ToString();
                                        string Material = dt.Rows[0]["MaterialCode"].ToString();
                                        string MaterialDesc = dt.Rows[0]["MaterialDesc"].ToString();
                                        string PIRJobTypeNDesc = dt.Rows[0]["PIRJobType"].ToString();
                                        TempInsDataToEmet(PIRNo, RequestIncNumber, QN, VC, VN, PG, Material, MaterialDesc, PIRJobTypeNDesc);
                                    }
                                    else
                                    {
                                        DataRow drInvalid = dtInValidRegList.NewRow();
                                        drInvalid["Plant"] = DtValidData.Rows[i]["Plant"].ToString();
                                        drInvalid["PIRNo"] = DtValidData.Rows[i]["PIRNo"].ToString();
                                        drInvalid["MaterialCode"] = DtValidData.Rows[i]["MaterialCode"].ToString();
                                        drInvalid["MaterialDesc"] = DtValidData.Rows[i]["MaterialDesc"].ToString();
                                        drInvalid["VendorCode"] = DtValidData.Rows[i]["VendorCode"].ToString();
                                        drInvalid["VendorName"] = DtValidData.Rows[i]["VendorName"].ToString();
                                        drInvalid["ProcessGroup"] = DtValidData.Rows[i]["ProcessGroup"].ToString();
                                        dtInValidRegList.Rows.Add(drInvalid);
                                    }
                                }
                            }
                        }

                        Session["TempDtInValidRegList"] = dtInValidRegList;
                        Session["dtRegList"] = dtRegList;

                        Session["Rawmaterial"] = null;
                        if (dtRegList.Rows.Count > 0)
                        {
                            isFromMainFunction = true;
                            for (int d = 0; d < dtRegList.Rows.Count; d++)
                            {
                                string ReqNo = dtRegList.Rows[d]["ReqNo"].ToString();
                                string plant = dtRegList.Rows[d]["Plant"].ToString();
                                string material = dtRegList.Rows[d]["MaterialCode"].ToString();
                                string vendor = dtRegList.Rows[d]["VendorCode"].ToString();
                                GetBOMForMassRevision(plant, material, vendor, ReqNo);
                            }
                            isFromMainFunction = false;
                        }

                        #region populate data valid to grid
                        GvReqList.DataSource = dtRegList;
                        int ShowEntry = 1;
                        if (TxtShowEntryReqList.Text == "" || TxtShowEntryReqList.Text == "0")
                        {
                            ShowEntry = 1;
                            TxtShowEntryReqList.Text = "1";
                        }
                        else
                        {
                            ShowEntry = Convert.ToInt32(TxtShowEntryReqList.Text);
                        }
                        GvReqList.PageSize = ShowEntry;
                        GvReqList.DataBind();
                        if (dtRegList.Rows.Count > 0)
                        {
                            LbQuoteMsg.Visible = true;
                            LbQuoteMsg.Text = "Quote Request can be Created only for below Vendors";
                            DvGvReqList.Visible = true;
                            DvSubmit.Visible = true;
                            LbReqlistTotRecord.Text = "Total Record : " + dtRegList.Rows.Count.ToString();
                        }
                        else
                        {
                            LbQuoteMsg.Visible = true;
                            LbQuoteMsg.Text = "No Data";
                            DvGvReqList.Visible = false;
                            DvSubmit.Visible = false;
                            LbReqlistTotRecord.Text = "Total Record : 0";
                        }
                        #endregion

                        checkInvalidReqlist();
                        #endregion
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Response.Write("<script>alert('" + Server.HtmlEncode(ex.ToString()) + "')</script>");
                    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                    EMETModule.SendExcepToDB(ex);
                    //Console.WriteLine("Timeout occurred");
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }


        protected void checkInvalidReqlist()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                if (Session["TempDtInValidRegList"] != null)
                {
                    DataTable TempdtInValidRegList = (DataTable)Session["TempDtInValidRegList"];

                    DataTable dtMissingInValidRegList = new DataTable();
                    dtMissingInValidRegList = TempdtInValidRegList.Clone();

                    if (TempdtInValidRegList.Rows.Count > 0)
                    {
                        DataTable dtInValidRegList = new DataTable();
                        dtInValidRegList.Columns.Add("Plant");
                        dtInValidRegList.Columns.Add("PIRNo");
                        dtInValidRegList.Columns.Add("MaterialCode");
                        dtInValidRegList.Columns.Add("MaterialDesc");
                        dtInValidRegList.Columns.Add("CodeRef");
                        dtInValidRegList.Columns.Add("VendorCode");
                        dtInValidRegList.Columns.Add("VendorName");
                        dtInValidRegList.Columns.Add("ProcessGroup");
                        dtInValidRegList.Columns.Add("CompMaterial");
                        dtInValidRegList.Columns.Add("CompMaterialDesc");
                        dtInValidRegList.Columns.Add("Amount");
                        dtInValidRegList.Columns.Add("UnitofCurrency");
                        dtInValidRegList.Columns.Add("Unit");
                        dtInValidRegList.Columns.Add("UoM");
                        dtInValidRegList.Columns.Add("ValidFrom");
                        dtInValidRegList.Columns.Add("ValidTo");
                        
                        DateTime ValidTo = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                        for (int i = 0; i < TempdtInValidRegList.Rows.Count; i++)
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                //sql = @"-------- third level 
                                //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                //        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidFrom,
                                //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                //        TM.UnitWeight,TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                //        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                //        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                //        CAST(ROUND(PN.amt1/PN.per1,5) AS DECIMAL(12,4)) as amt1,
                                //        CAST(ROUND(PN.amt2/PN.per2,5) AS DECIMAL(12,4)) as amt2,
                                //        CAST(ROUND(PN.amt3/PN.per3,5) AS DECIMAL(12,4)) as amt3,
                                //        CAST(ROUND(PN.amt4/PN.per4,5) AS DECIMAL(12,4)) as amt4,
                                //        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                //        FROM tPir_New PN
                                //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType
                                //        INNER JOIN TBOMLIST TB ON TM.Material = TB.ImmedParent
                                //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material
                                //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                //        where PN.Material = @Material 
                                //        --and TCM.ValidFrom < @ValidTo
                                //        --and TCM.ValidTo < @ValidTo
                                //        and PN.Vendor = @VendorCode 
                                //        order by TCM.ValidFrom desc ";


                                //sql = @"-------- third level 
                                //        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                //        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                //        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                //        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',
                                //        TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                //        TM.UnitWeight,
                                //        TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                //        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                //        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,
                                //        concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                //        CAST(ROUND(PN.amt1/PN.per1,5) AS DECIMAL(12,5)) as amt1,
                                //        CAST(ROUND(PN.amt2/PN.per2,5) AS DECIMAL(12,5)) as amt2,
                                //        CAST(ROUND(PN.amt3/PN.per3,5) AS DECIMAL(12,5)) as amt3,
                                //        CAST(ROUND(PN.amt4/PN.per4,5) AS DECIMAL(12,5)) as amt4,
                                //        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                //        FROM tPir_New PN
                                //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                //        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                //        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                //        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                //        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                //        and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                                //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                                //        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                //        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                //        --where confition based on each record in invalid data 
                                //        where PN.Material = @Material 
                                //        --and TCM.ValidFrom <= @ValidTo
                                //        --and TCM.ValidTo >= @ValidTo
                                //        and PN.Vendor = @VendorCode 
                                //        and tm.Plant=@plant
                                //        order by TCM.ValidTo desc
                                //         ";

                                //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                                sql = @"-------- third level 
                                        SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                        PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                        PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                        (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',
                                        TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,
                                        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                                        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo',
                                        TM.UnitWeight,
                                        TM.UnitWeightUOM,TM.BaseUOM,TM.Plating, 
                                        TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product,
                                        TPRC.ProdComDesc as MaterialClass,TM.BaseUOM,
                                        concat(TPT.PIRType,' - ',TPT.Description) as PIRType,
                                        CAST(ROUND(PN.amt1/nullif(PN.per1,0),5) AS DECIMAL(12,5)) as amt1,
                                        CAST(ROUND(PN.amt2/nullif(PN.per2,0),5) AS DECIMAL(12,5)) as amt2,
                                        CAST(ROUND(PN.amt3/nullif(PN.per3,0),5) AS DECIMAL(12,5)) as amt3,
                                        CAST(ROUND(PN.amt4/nullif(PN.per4,0),5) AS DECIMAL(12,5)) as amt4,
                                        PN.countryorg,format(PN.UpdatedOn,'yyyy-MM-dd hh:mm:ss') as MassUpdateDate
                                        FROM tPir_New PN
                                        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                        INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                        Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode and TPRC.plant = TM.plant
                                        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                        inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                        and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                        INNER JOIN TBOMLISTnew TB ON TM.Material = TB.[Parent Material]
                                        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                        INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                        inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                        --where confition based on each record in invalid data 
                                        where PN.Material = @Material 
                                        --and TCM.ValidFrom <= @ValidTo
                                        --and TCM.ValidTo >= @ValidTo
                                        and PN.Vendor = @VendorCode 
                                        and tm.Plant=@plant
                                        order by TCM.ValidTo desc
                                         ";

                                cmd = new SqlCommand(sql, MDMCon);
                                cmd.CommandTimeout = 10000;
                                //cmd.Parameters.AddWithValue("@ImmedParent", DtValidData.Rows[i]["MaterialCode"].ToString());
                                cmd.Parameters.AddWithValue("@Material", TempdtInValidRegList.Rows[i]["MaterialCode"].ToString());
                                cmd.Parameters.AddWithValue("@VendorCode", TempdtInValidRegList.Rows[i]["VendorCode"].ToString());
                                cmd.Parameters.AddWithValue("@Plant", TempdtInValidRegList.Rows[i]["Plant"].ToString());
                                cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy/MM/dd"));
                                sda.SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        //string RequestIncNumber = GenerateReqNo().ToString();
                                        DataRow dr = dtInValidRegList.NewRow();
                                        dr["Plant"] = TempdtInValidRegList.Rows[i]["Plant"].ToString();
                                        dr["PIRNo"] = TempdtInValidRegList.Rows[i]["PIRNo"].ToString();
                                        dr["MaterialCode"] = TempdtInValidRegList.Rows[i]["MaterialCode"].ToString();
                                        dr["MaterialDesc"] = TempdtInValidRegList.Rows[i]["MaterialDesc"].ToString();
                                        dr["CodeRef"] = dt.Rows[0]["CodeRef"].ToString();
                                        dr["VendorCode"] = TempdtInValidRegList.Rows[i]["VendorCode"].ToString();
                                        dr["VendorName"] = TempdtInValidRegList.Rows[i]["VendorName"].ToString();
                                        dr["ProcessGroup"] = TempdtInValidRegList.Rows[i]["ProcessGroup"].ToString();
                                        dr["CompMaterial"] = dt.Rows[0]["CompMaterial"].ToString();
                                        dr["CompMaterialDesc"] = dt.Rows[0]["CompMaterialDesc"].ToString();
                                        dr["Amount"] = dt.Rows[0]["Amount"].ToString();
                                        dr["UnitofCurrency"] = dt.Rows[0]["UnitofCurrency"].ToString();
                                        dr["Unit"] = dt.Rows[0]["Unit"].ToString();
                                        dr["UoM"] = dt.Rows[0]["UoM"].ToString();
                                        dr["ValidFrom"] = dt.Rows[0]["ValidFrom"].ToString();
                                        dr["ValidTo"] = dt.Rows[0]["ValidToo"].ToString();

                                        dtInValidRegList.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        DataRow drMissingInvalid = dtMissingInValidRegList.NewRow();
                                        drMissingInvalid["Plant"] = TempdtInValidRegList.Rows[i]["Plant"].ToString();
                                        drMissingInvalid["PIRNo"] = TempdtInValidRegList.Rows[i]["PIRNo"].ToString();
                                        drMissingInvalid["MaterialCode"] = TempdtInValidRegList.Rows[i]["MaterialCode"].ToString();
                                        drMissingInvalid["MaterialDesc"] = TempdtInValidRegList.Rows[i]["MaterialDesc"].ToString();
                                        drMissingInvalid["VendorCode"] = TempdtInValidRegList.Rows[i]["VendorCode"].ToString();
                                        drMissingInvalid["VendorName"] = TempdtInValidRegList.Rows[i]["VendorName"].ToString();
                                        drMissingInvalid["ProcessGroup"] = TempdtInValidRegList.Rows[i]["ProcessGroup"].ToString();
                                        dtMissingInValidRegList.Rows.Add(drMissingInvalid);
                                    }
                                }
                            }
                        }
                        Session["dtInValidRegList"] = dtInValidRegList;
                        Session["dtMissingInValidRegList"] = dtMissingInValidRegList;
                        #region populate data invalid to grid
                        if (dtInValidRegList.Rows.Count > 0)
                        {
                            GdvInvalidRequest.DataSource = dtInValidRegList;
                            int ShowEntry = 1;
                            if (TxtShowEntryInvalidReqList.Text == "" || TxtShowEntryInvalidReqList.Text == "0")
                            {
                                ShowEntry = 1;
                                TxtShowEntryInvalidReqList.Text = "1";
                            }
                            else
                            {
                                ShowEntry = Convert.ToInt32(TxtShowEntryInvalidReqList.Text);
                            }
                            GdvInvalidRequest.PageSize = ShowEntry;
                            GdvInvalidRequest.DataBind();
                            DvInvalidListRequest.Visible = true;
                            LbInvalidListRequest.Text = "Total Record Invalid Request : " + dtInValidRegList.Rows.Count.ToString();
                        }
                        else
                        {
                            DvInvalidListRequest.Visible = false;
                            LbInvalidListRequest.Text = "Total Record Invalid Request : " + dtInValidRegList.Rows.Count.ToString();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";
            try
            {

                // Retrieve the last column that was sorted.
                string sortExpression = ViewState["SortExpression"] as string;

                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["SortDirection"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }

                // Save new values in ViewState.
                ViewState["SortDirection"] = sortDirection;
                ViewState["SortExpression"] = column;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            return sortDirection;
        }

        protected void GetDdlReason()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select ReasonforRejection from TREASONFORMETREJECTION where DelFlag=0 and ReasonType='Creation' and SysCode = 'emet' and Plant=@Plant and DelFlag = 0";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DdlReason.Items.Clear();
                            DdlReason.DataTextField = "ReasonforRejection";
                            DdlReason.DataValueField = "ReasonforRejection";

                            DdlReason.DataSource = dt;
                            DdlReason.DataBind();

                            DdlReason.Items.Insert(0, new ListItem("--Select Request Purpose--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                        else
                        {
                            DdlReason.Items.Clear();
                            //DdlReason.Items.Insert(0, new ListItem("--Reason Not Exist--", "0"));
                            DdlReason.Items.Insert(0, new ListItem("--Select Request Purpose--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                    }
                    //UpdatePanel18.Update();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if ((FlUpload.HasFile))
                {
                    Session["DtValidData"] = null;
                    Session["DtInValidData"] = null;
                    Session["dtRegList"] = null;
                    Session["DtForSubmit"] = null;
                    Session["dtRegList"] = null;
                    GvInvalid.DataSource = null;
                    GvInvalid.DataBind();
                    GvValidData.DataSource = null;
                    GvValidData.DataBind();
                    GvReqList.DataSource = null;
                    GvReqList.DataBind();
                    DeleteNonRequest();
                    UploadData();
                    if (isUploadScs == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Invalid Data File, Upload Faill !!');CloseLoading();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = Server.MapPath("~/template/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string FileExtension = "xlsx";
                string filename = "LBS PIR for Mass Upload.xlsx";
                string[] Arrfilename = filename.Split('.');
                int c = Arrfilename.Count();
                if (c > 0)
                {
                    FileExtension = Arrfilename[c - 1].ToString();
                }
                string PathAndFileName = folderPath + filename;

                if (filename != "")
                {
                    FileInfo fileCheck = new FileInfo(PathAndFileName);
                    if (fileCheck.Exists) //check file exsit or not  
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/" + FileExtension.Replace(".", "") + "";
                        Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.TransmitFile(PathAndFileName);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File Not Found !')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnCreateRequest_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteNonRequest();
                CreateRequest();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckAllData() == true)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();ChgEmptyFlColor();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Faill to check Data !! please try again.');DatePitcker();CloseLoading();", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnSubmitProcDuplicateReg_Click(object sender, EventArgs e)
        {
            ProcessDuplicateReqWithOldNExpiredReq();
            BtnProceed_Click(BtnProceed, EventArgs.Empty);
        }

        protected void MainDataForMail()
        {
            try
            {
                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenEMETConnString()))
                {
                    Email_inser.Open();
                    SqlCommand sqlCmd = new SqlCommand("Email_UNC", Email_inser);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr;
                    dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        UseridMail = dr.GetString(0);
                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
                        Session["path"] = dr.GetString(3);
                        URL = dr.GetString(4);
                        MasterDB = dr.GetString(5);
                        TransDB = dr.GetString(6);
                    }
                    dr.Dispose();
                    Email_inser.Dispose();

                    //string Destination = Session["path"].ToString() + Session["OriginalFilename"];
                    //using (new SoddingNetworkAuth(Userid, domain, password))
                    //{
                    //    try
                    //    {
                    //        string folderPath = Server.MapPath("~/Files/");
                    //        if (!Directory.Exists(folderPath))
                    //        {
                    //            Directory.CreateDirectory(folderPath);
                    //        }
                    //        string filename = "";
                    //        string PathAndFileName = "";
                    //        //if (FlAttachment.HasFile)
                    //        //{
                    //        //    filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                    //        //    PathAndFileName = folderPath + filename;
                    //        //    FlAttachment.SaveAs(PathAndFileName);
                    //        //}
                    //        Source = PathAndFileName;
                    //        File.Copy(Source, Destination, true);
                    //        SendFilename = fname.Remove(fname.Length - 4);
                    //        Session["SendFilename"] = fname.Remove(fname.Length - 4);
                    //        OriginalFilename = Session["OriginalFilename"].ToString();
                    //        OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                    //        Session["OriginalFilename"] = OriginalFilename.ToString();
                    //        format = "pdf";
                    //        FileInfo file = new FileInfo(PathAndFileName);
                    //        if (file.Exists) //check file exsit or not  
                    //        {
                    //            file.Delete();
                    //        }
                    //    }
                    //    catch (Exception xw1)
                    //    {
                    //        Response.Write(xw1);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProceedSubmitRequest() == true)
                {
                    DeleteNonRequest();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Submit request success !');CloseLoading();window.location ='Home.aspx';", true);
                    //SendingMail();
                    //if (sendingmail == true)
                    //{
                    //    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Successfully submitted");
                    //    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    //}
                    //else
                    //{
                    //    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed !! \n " + errmsg + " ");
                    //    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    //}
                }
                else
                {
                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submit request Faill !");
                    var script = string.Format("alert({0});CloseLoading();", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submit request Faill !: " + ex.ToString());
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected void BtnExportInvalidData_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtInValidData"] != null)
                {
                    DataTable dt = (DataTable)Session["DtInValidData"];
                    if (dt.Rows.Count > 0)
                    {
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearHeaders();
                        response.ClearContent();
                        response.Charset = Encoding.UTF8.WebName;
                        response.AddHeader("content-disposition", "attachment; filename=InvalidData.xls");
                        response.AddHeader("Content-Type", "application/Excel");
                        response.ContentType = "application/vnd.xlsx";
                        using (StringWriter sw = new StringWriter())
                        {
                            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                            {
                                GridView gridView = new GridView();
                                gridView.DataSource = dt;
                                gridView.DataBind();
                                gridView.RenderControl(htw);
                                //response.Write(sw.ToString());

                                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                                Response.Write(style);
                                StringBuilder sSchema = new StringBuilder();
                                sSchema.Append("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"  <head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1252\"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>ExportToExcel</x:Name><x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head> <body>");
                                sSchema.Append(sw.ToString() + "</body></html>");
                                Response.Output.Write(sSchema.ToString());

                                gridView.Dispose();
                                dt.Dispose();
                                response.End();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            
        }

        protected void BtnExportInvalidListRequest_Click(object sender, EventArgs e)
        {
            try
            {

                if (Session["dtInValidRegList"] != null)
                {
                    DataTable dt = (DataTable)Session["dtInValidRegList"];
                    if (dt.Rows.Count > 0)
                    {
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearHeaders();
                        response.ClearContent();
                        response.Charset = Encoding.UTF8.WebName;
                        response.AddHeader("content-disposition", "attachment; filename=InvalidDataListRequest.xls");
                        response.AddHeader("Content-Type", "application/Excel");
                        response.ContentType = "application/vnd.xlsx";
                        using (StringWriter sw = new StringWriter())
                        {
                            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                            {
                                GridView gridView = new GridView();
                                gridView.DataSource = dt;
                                gridView.DataBind();
                                gridView.RenderControl(htw);
                                //response.Write(sw.ToString());

                                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                                Response.Write(style);
                                StringBuilder sSchema = new StringBuilder();
                                sSchema.Append("<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"  <head><meta http-equiv=\"Content-Type\" content=\"text/html;charset=windows-1252\"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>ExportToExcel</x:Name><x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head> <body>");
                                sSchema.Append(sw.ToString() + "</body></html>");
                                Response.Output.Write(sSchema.ToString());

                                gridView.Dispose();
                                dt.Dispose();
                                response.End();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvAllData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvAllData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GdvAllData.PageIndex = e.NewPageIndex;
                if (Session["DtAllData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtAllData"];
                    GdvAllData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryAllData.Text == "" || TxtShowEntryAllData.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryAllData.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryAllData.Text);
                    }
                    GdvAllData.PageSize = ShowEntry;
                    GdvAllData.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvAllData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["DtAllData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtAllData"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GdvAllData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryAllData.Text == "" || TxtShowEntryAllData.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryAllData.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryAllData.Text);
                    }
                    GdvAllData.PageSize = ShowEntry;
                    GdvAllData.DataBind();
                    Session["DtAllData"] = dt;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvAllData_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvValidData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvValidData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvValidData.PageIndex = e.NewPageIndex;
                if (Session["DtValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtValidData"];
                    GvValidData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryValid.Text == "" || TxtShowEntryValid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryValid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryValid.Text);
                    }
                    GvValidData.PageSize = ShowEntry;
                    GvValidData.DataBind();
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvValidData_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["DtValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtValidData"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GvValidData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryValid.Text == "" || TxtShowEntryValid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryValid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryValid.Text);
                    }
                    GvValidData.PageSize = ShowEntry;
                    GvValidData.DataBind();
                    Session["DtValidData"] = dt;
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvValidData_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvInvalid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[8].Text = e.Row.Cells[8].Text.Replace(".", ".</br>");
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvInvalid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvInvalid.PageIndex = e.NewPageIndex;
                if(Session["DtInValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtInValidData"];
                    GvInvalid.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalid.Text == "" || TxtShowEntryInvalid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalid.Text);
                    }
                    GvInvalid.PageSize = ShowEntry;
                    GvInvalid.DataBind();
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvInvalid_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["DtInValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtInValidData"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GvInvalid.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalid.Text == "" || TxtShowEntryInvalid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalid.Text);
                    }
                    GvInvalid.PageSize = ShowEntry;
                    GvInvalid.DataBind();
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvInvalid_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetBOMForMassRevision(string plant, string Material, string vendor,string ReqNo)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                GetDbTrans();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" SELECT distinct 
                    //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',
                    //        TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TM.UoM,TM.UnitWeightUOM,
                    //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                    //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo'
                    //        FROM tPir_New PN
                    //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                    //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                    //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                    //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                    //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                    //        where PN.Material = @Material
                    //        and TCM.ValidFrom <= @ValidTo
                    //        and TCM.ValidTo >= @ValidTo
                    //        and PN.Vendor = @VendorCode
                    //        and tm.Plant=@Plant
                    //        order by TCM.ValidTo desc ";

                    //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                    //sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                    //        isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                    //        tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
                    //        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                    //        tm.BaseUOM as UOM,format(TR.ValidFrom,'dd/MM/yyyy') as ValidFrom,TQ.RequestNumber as [Req No],TQ.QuoteNo,
                    //        FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                    //        from TMATERIAL tm 
                    //        inner join TBOMLIST TB on tm.Material = TB.Material 
                    //        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material and tc.plant = @Plant and tc.delflag = 0
                    //        inner join tVendor_New tv on tv.customerNo=tc.customer  
                    //        inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                    //        inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                    //        left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                    //        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                    //        where  TQ.RequestNumber=@ReqNo and  tm.plant=@Plant  
                    //        and tvp.plant= @Plant and TB.FGcode=@Material 
                    //        and tv.Vendor=@VendorCode and TM.DELFLAG = 0
                    //        and tc.ValidFrom <= @ValidTo
                    //        and tc.ValidTo >= @ValidTo 
                    //        ";

                    //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                    sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into #complist1 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into #e50 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo  
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'

                                            IF ((select count (*) from #e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from #e50
	                                            delete from #e50
	
	                                            insert into #e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from #e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";
                    sql += @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                            isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                            tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
                            isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                            tm.BaseUOM as UOM,format(TR.ValidFrom,'dd/MM/yyyy') as ValidFrom,TR.ValidFrom as ValidFromori, TQ.RequestNumber as [Req No],TQ.QuoteNo,
                            FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                            from TMATERIAL tm 
                            inner join #complist1 TB on tm.Material = TB.[Component Material]
                            inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material and tc.plant = @plant and tc.delflag = 0
                            inner join tVendor_New tv on tv.customerNo=tc.customer  
                            inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                            inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                            left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                            and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                            where  TQ.RequestNumber=@ReqNo and  tm.plant=@plant  
                            and tvp.plant= @plant and TB.[Parent Material]=@mat
                            and tv.Vendor=@VendorCode and TM.DELFLAG = 0
                            and tc.ValidFrom <= @ValidTo
                            and tc.ValidTo >= @ValidTo 
                            ";
                    sql += " drop table #complist1, #e50 ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.CommandTimeout = 10000;
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@plant", plant);
                    cmd.Parameters.AddWithValue("@VendorCode", vendor);
                    cmd.Parameters.AddWithValue("@mat", Material);
                    DateTime ValidTo = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy/MM/dd"));
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Session["GvComMaterialInfo"] = dt;

                            if (isFromMainFunction == true)
                            {
                                if (Session["Rawmaterial"] == null)
                                {
                                    Session["Rawmaterial"] = dt;
                                }
                                else
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataTable DtRawmat = (DataTable)Session["Rawmaterial"];
                                        foreach (DataRow drdtget in dt.Rows)
                                        {
                                            DtRawmat.ImportRow(drdtget);
                                        }

                                        Session["Rawmaterial"] = DtRawmat;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Session["GvComMaterialInfo"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        protected void GetBOMForMassRevisionInvalid(string plant, string Material, string vendor)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" SELECT distinct 
                    //        TCM.Material as 'CompMaterial',TCM.Materialdescription as 'CompMaterialDesc',TCM.Amount,TCM.UnitofCurrency,TCM.Unit,TCM.UoM,TM.UnitWeightUOM,
                    //        format(TCM.ValidFrom, 'dd/MM/yyyy') as 'ValidFrom',TCM.ValidTo,
                    //        format(TCM.ValidTo, 'dd/MM/yyyy') as 'ValidToo'
                    //        FROM tPir_New PN
                    //        INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                    //        INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                    //        INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                    //        INNER JOIN TBOMLIST TB ON TM.Material = TB.fgcode
                    //        INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.Material = TCM.Material and TV.CustomerNo = TCM.Customer
                    //        where PN.Material = @Material
                    //        --and TCM.ValidFrom <= @ValidTo
                    //        --and TCM.ValidTo >= @ValidTo
                    //        and PN.Vendor = @VendorCode
                    //        and tm.Plant=@Plant
                    //        order by TCM.ValidTo desc ";

                    //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                    //sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                    //        isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                    //        tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
                    //        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                    //        tm.BaseUOM as UOM,format(TR.ValidFrom,'yyyy/MM/dd') as ValidFrom
                    //        from TMATERIAL tm 
                    //        inner join TBOMLIST TB on tm.Material = TB.Material 
                    //        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material and tc.plant = @Plant and tc.delflag = 0
                    //        inner join tVendor_New tv on tv.customerNo=tc.customer  
                    //        inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                    //        inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                    //        left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                    //        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                    //        where  tm.plant=@Plant  
                    //        and tvp.plant= @Plant and TB.FGcode=@Material 
                    //        and tv.Vendor=@VendorCode and TM.DELFLAG = 0
                    //        --and tc.ValidFrom <= @ValidTo
                    //        --and tc.ValidTo >= @ValidTo 
                    //        ";

                    //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                    sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                            isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                            tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
                            isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                            tm.BaseUOM as UOM,format(TR.ValidFrom,'yyyy/MM/dd') as ValidFrom
                            from TMATERIAL tm 
                            inner join TBOMLISTnew TB on tm.Material = TB.[Component Material]
                            inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material and tc.plant = @Plant and tc.delflag = 0
                            inner join tVendor_New tv on tv.customerNo=tc.customer  
                            inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                            inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                            left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                            and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                            where  tm.plant=@Plant  
                            and tvp.plant= @Plant and TB.[Parent Material]=@Material 
                            and tv.Vendor=@VendorCode and TM.DELFLAG = 0
                            --and tc.ValidFrom <= @ValidTo
                            --and tc.ValidTo >= @ValidTo 
                            ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.CommandTimeout = 10000;
                    cmd.Parameters.AddWithValue("@Plant", plant);
                    cmd.Parameters.AddWithValue("@VendorCode", vendor);
                    cmd.Parameters.AddWithValue("@Material", Material);
                    DateTime ValidTo = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy/MM/dd"));
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Session["GvComMaterialInfo"] = dt;
                        }
                        else
                        {
                            Session["GvComMaterialInfo"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        /// <summary>
        /// Get BOM Detail Raw Material before efffective date
        /// </summary>
        protected void GetBOMRawmaterialBefEffdate(string ReqNo, string QuoteNo, string VendorCode,string Material, string Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //              string sql = @" select distinct @ReqNo as [Req No],@QuoteNo as [QuoteNo],@VendorCode [Vendor Code],tvpo.Plant,  
                    //                  TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                    //                  vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                    //                  isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                    //                  v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                    //                  tc.Unit,tm.BaseUOM as UOM,
                    //format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                    //                  into #temp1
                    //                  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material   
                    //                  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material   and tc.plant = @Plant and tc.delflag = 0
                    //                  inner join tVendor_New v on v.CustomerNo = tc.Customer   
                    //                  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                    //                  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                    //                  left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = @Plant and VP.Plant = @Plant  and tvpo.Plant = @Plant
                    //                  and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= @ValidTo )
                    //                  where tm.Plant=@Plant and tvpo.Plant=@Plant and vp.plant = @Plant and TB.FGCode = @Material
                    //                  and format(TC.ValidTo,'yyyy-MM-dd') < @ValidTo
                    //                  and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!= 0 ) 
                    //                  and v.Vendor = @VendorCode
                    //                  order by format(tc.ValidTo,'yyyy-MM-dd') desc

                    //                  ;with cte as
                    //                  (
                    //                   select *, ROW_NUMBER() over (partition by [Comp Material] order by [CusMatValTo] desc) as RN
                    //                    from #temp1
                    //                  )
                    //                  select * from cte where RN = 1

                    //                  drop table #temp1
                    //                  ";

                    //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                    string sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into #complist1 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            and [header valid to date] < @ValidTo and [header valid from date] >= @ValidTo
                                            and [comp. valid to date] < @ValidTo and [comp. valid from date] >= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into #e50 from tbomlistnew 
                                            where Plant=@plant and [Parent Material]=@mat 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            and [header valid to date] < @ValidTo and [header valid from date] >= @ValidTo
                                            and [comp. valid to date] < @ValidTo and [comp. valid from date] >= @ValidTo  
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'

                                            IF ((select count (*) from #e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] < @ValidTo and [header valid from date] >= @ValidTo
	                                            and [comp. valid to date] < @ValidTo and [comp. valid from date] >= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from #e50
	                                            delete from #e50
	
	                                            insert into #e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] < @ValidTo and [header valid from date] >= @ValidTo
	                                            and [comp. valid to date] < @ValidTo and [comp. valid from date] >= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from #e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";
                    sql += @" select distinct @ReqNo as [Req No],@QuoteNo as [QuoteNo],@VendorCode [Vendor Code],tvpo.Plant,  
                        TC.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                        vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,tm.BaseUOM as UOM,
						format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,TR.ValidFrom as ValidFromOri,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                        into #temp1
                        from TMATERIAL tm inner join #complist1 TB on tm.Material = TB.[Component Material]
                        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @Plant and tc.delflag = 0
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = @plant and VP.Plant = @plant  and tvpo.Plant = @plant
                        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= @ValidTo )
                        where tm.Plant=@plant and tvpo.Plant=@plant and vp.plant = @plant and TB.[Parent Material] = @mat
                        and format(TC.ValidTo,'yyyy-MM-dd') < @ValidTo
                        and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!= 0 ) 
                        and v.Vendor = @VendorCode
                        order by format(tc.ValidTo,'yyyy-MM-dd') desc
                        
                        ;with cte as
                        (
	                        select *, ROW_NUMBER() over (partition by [Comp Material] order by [CusMatValTo] desc) as RN
	                         from #temp1
                        )
                        select * from cte where RN = 1

                        drop table #temp1
                        ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    cmd.Parameters.AddWithValue("@mat", Material);
                    cmd.Parameters.AddWithValue("@plant", Plant);
                    DateTime ValidTo = DateTime.ParseExact(TxtValidDate.Text, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy-MM-dd"));
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (Session["OldRawmaterial"] == null)
                            {
                                Session["OldRawmaterial"] = dt;
                            }
                            else
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    DataTable DtRawmat = (DataTable)Session["OldRawmaterial"];
                                    foreach (DataRow drdtget in dt.Rows)
                                    {
                                        DtRawmat.ImportRow(drdtget);
                                    }

                                    Session["OldRawmaterial"] = DtRawmat;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        protected void GvReqList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int idx = e.Row.DataItemIndex;
                    int index = e.Row.RowIndex;
                    DataTable dtRegList = (DataTable)Session["dtRegList"];

                    GridView GvComMaterialInfo = e.Row.FindControl("GvComMaterialInfo") as GridView;
                    string ReqNo = dtRegList.Rows[idx]["ReqNo"].ToString();
                    string plant = dtRegList.Rows[idx]["Plant"].ToString();
                    string material = dtRegList.Rows[idx]["MaterialCode"].ToString();
                    string vendor = dtRegList.Rows[idx]["VendorCode"].ToString();
                    string QuoteNo = dtRegList.Rows[idx]["QuoteNo"].ToString();

                    //GetBOMForMassRevision(plant, material, vendor, ReqNo);
                    //GetBOMRawmaterialBefEffdate(ReqNo,QuoteNo,vendor,material,plant);
                    //DataTable DtGvDetMatCost = new DataTable();
                    //if (Session["Rawmaterial"] != null)
                    //{
                    //    DtGvDetMatCost = (DataTable)Session["Rawmaterial"];
                    //    DataView dv = new DataView(DtGvDetMatCost);
                    //    dv.RowFilter = "[Req No]="+ ReqNo + ""; // query example = "id = 10"
                    //    GvComMaterialInfo.DataSource = dv;
                    //    GvComMaterialInfo.DataBind();
                    //}


                    GetBOMForMassRevision(plant, material, vendor, ReqNo);
                    GetBOMRawmaterialBefEffdate(ReqNo, QuoteNo, vendor, material, plant);
                    DataTable DtGvDetMatCost = new DataTable();
                    if (Session["GvComMaterialInfo"] != null)
                    {
                        DtGvDetMatCost = (DataTable)Session["GvComMaterialInfo"];
                        GvComMaterialInfo.DataSource = DtGvDetMatCost;
                        GvComMaterialInfo.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        //protected void GvReqList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            int idx = e.Row.DataItemIndex;
        //            int index = e.Row.RowIndex;
        //            DataTable dtRegList = (DataTable)Session["dtRegList"];

        //            GridView GvComMaterialInfo = e.Row.FindControl("GvComMaterialInfo") as GridView;
        //            string ReqNo = dtRegList.Rows[idx]["ReqNo"].ToString();
        //            string plant = dtRegList.Rows[idx]["Plant"].ToString();
        //            string material = dtRegList.Rows[idx]["MaterialCode"].ToString();
        //            string vendor = dtRegList.Rows[idx]["VendorCode"].ToString();
        //            string QuoteNo = dtRegList.Rows[idx]["QuoteNo"].ToString();
                    
        //            DataTable DtGvDetMatCost = new DataTable();
        //            if (Session["Rawmaterial"] != null)
        //            {
        //                DtGvDetMatCost = (DataTable)Session["Rawmaterial"];
        //                DataView dv = new DataView(DtGvDetMatCost);
        //                dv.RowFilter = "[Req No]=" + ReqNo + ""; // query example = "id = 10"
        //                GvComMaterialInfo.DataSource = dv;
        //                GvComMaterialInfo.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        protected void GvComMaterialInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvComMaterialInfoInvalid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int idx = e.Row.DataItemIndex;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvReqList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvReqList.PageIndex = e.NewPageIndex;
                if (Session["dtRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtRegList"];
                    GvReqList.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryReqList.Text == "" || TxtShowEntryReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryReqList.Text);
                    }
                    GvReqList.PageSize = ShowEntry;
                    GvReqList.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvReqList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["dtRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtRegList"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GvReqList.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryReqList.Text == "" || TxtShowEntryReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryReqList.Text);
                    }
                    GvReqList.PageSize = ShowEntry;
                    GvReqList.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvReqList_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvInvalidRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int idx = e.Row.DataItemIndex;
                    int index = e.Row.RowIndex;
                    DataTable TempDtInValidRegList = (DataTable)Session["TempDtInValidRegList"];

                    GridView GvComMaterialInfoInvalid = e.Row.FindControl("GvComMaterialInfoInvalid") as GridView;
                    string plant = TempDtInValidRegList.Rows[idx]["Plant"].ToString();
                    string PIRNo = TempDtInValidRegList.Rows[idx]["PIRNo"].ToString();
                    string material = TempDtInValidRegList.Rows[idx]["MaterialCode"].ToString();
                    string vendor = TempDtInValidRegList.Rows[idx]["VendorCode"].ToString();
                    string ProcessGroup = TempDtInValidRegList.Rows[idx]["ProcessGroup"].ToString();
                    GetBOMForMassRevisionInvalid(plant, material, vendor);
                    DataTable DtGvDetMatCost = new DataTable();
                    //if (Session["InvalidRawmaterial"] != null)
                    //{
                    //    DtGvDetMatCost = (DataTable)Session["InvalidRawmaterial"];
                    //    DataView dv = new DataView(DtGvDetMatCost);
                    //    dv.RowFilter = "[Plant] = '" + plant + "' AND [PIRNo]='" + PIRNo + "' AND [ParentMat]='" + material + "' AND [VendorCode]='" + vendor + "' AND [ProcessGroup]='" + ProcessGroup + "'";
                    //    GvComMaterialInfoInvalid.DataSource = dv;
                    //    GvComMaterialInfoInvalid.DataBind();
                    //}
                    if (Session["GvComMaterialInfo"] != null)
                    {
                        DtGvDetMatCost = (DataTable)Session["GvComMaterialInfo"];
                        GvComMaterialInfoInvalid.DataSource = DtGvDetMatCost;
                        GvComMaterialInfoInvalid.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        //protected void GdvInvalidRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            int idx = e.Row.DataItemIndex;
        //            int index = e.Row.RowIndex;
        //            DataTable TempDtInValidRegList = (DataTable)Session["TempDtInValidRegList"];

        //            GridView GvComMaterialInfoInvalid = e.Row.FindControl("GvComMaterialInfoInvalid") as GridView;
        //            string plant = TempDtInValidRegList.Rows[idx]["Plant"].ToString();
        //            string PIRNo = TempDtInValidRegList.Rows[idx]["PIRNo"].ToString();
        //            string material = TempDtInValidRegList.Rows[idx]["MaterialCode"].ToString();
        //            string vendor = TempDtInValidRegList.Rows[idx]["VendorCode"].ToString();
        //            string ProcessGroup = TempDtInValidRegList.Rows[idx]["ProcessGroup"].ToString();
        //            DataTable DtGvDetMatCost = new DataTable();
        //            if (Session["InvalidRawmaterial"] != null)
        //            {
        //                DtGvDetMatCost = (DataTable)Session["InvalidRawmaterial"];
        //                DataView dv = new DataView(DtGvDetMatCost);
        //                dv.RowFilter = "[Plant] = '" + plant + "' AND [PIRNo]='" + PIRNo + "' AND [ParentMat]='" + material + "' AND [VendorCode]='" + vendor + "' AND [ProcessGroup]='" + ProcessGroup + "'";
        //                GvComMaterialInfoInvalid.DataSource = dv;
        //                GvComMaterialInfoInvalid.DataBind();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        protected void GdvInvalidRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GdvInvalidRequest.PageIndex = e.NewPageIndex;
                if (Session["dtInValidRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtInValidRegList"];
                    GdvInvalidRequest.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalidReqList.Text == "" || TxtShowEntryInvalidReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalidReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalidReqList.Text);
                    }
                    GdvInvalidRequest.PageSize = ShowEntry;
                    GdvInvalidRequest.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvInvalidRequest_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["dtInValidRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtInValidRegList"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GdvInvalidRequest.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalidReqList.Text == "" || TxtShowEntryInvalidReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalidReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalidReqList.Text);
                    }
                    GdvInvalidRequest.PageSize = ShowEntry;
                    GdvInvalidRequest.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvInvalidRequest_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvDuplicateWithExpiredReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //DataRowView dr = (DataRowView)e.Row.DataItem;
                    //string NewreqNo = dr["RequestNumber"].ToString();

                    int RowParentGv = e.Row.DataItemIndex;
                    RadioButton RbReject = e.Row.FindControl("RbReject") as RadioButton;
                    RbReject.Attributes.Add("onchange", "RbRejectExpReq(" + RowParentGv + ")");

                    RadioButton RbChangeDate = e.Row.FindControl("RbChangeDate") as RadioButton;
                    RbChangeDate.Attributes.Add("onchange", "RbChangedateResDueDate(" + RowParentGv + ")");

                    TextBox TxtNewDueDate = e.Row.FindControl("TxtNewDueDate") as TextBox;
                    string TxtNewDueDateID = ((TextBox)e.Row.FindControl("TxtNewDueDate")).ClientID.ToString();

                    HtmlGenericControl IcnCalendarNewDueDate = ((HtmlGenericControl)(e.Row.FindControl("IcnCalendarNewDueDate")));
                    string IcnCalendarNewDueDateID = ((HtmlGenericControl)e.Row.FindControl("IcnCalendarNewDueDate")).ClientID.ToString();
                    if (IcnCalendarNewDueDate != null)
                    {
                        IcnCalendarNewDueDate.Attributes.Add("onclick", "FocusToTxt(" + RowParentGv + ",'" + TxtNewDueDateID + "')");
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void TxtShowEntryAllData_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtAllData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtAllData"];
                    GdvAllData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryAllData.Text == "" || TxtShowEntryAllData.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryAllData.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryAllData.Text);
                    }
                    GdvAllData.PageSize = ShowEntry;
                    GdvAllData.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtShowEntryValid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtValidData"];
                    GvValidData.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryValid.Text == "" || TxtShowEntryValid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryValid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryValid.Text);
                    }
                    GvValidData.PageSize = ShowEntry;
                    GvValidData.DataBind();
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtShowEntryInvalid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtInValidData"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtInValidData"];
                    GvInvalid.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalid.Text == "" || TxtShowEntryInvalid.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalid.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalid.Text);
                    }
                    GvInvalid.PageSize = ShowEntry;
                    GvInvalid.DataBind();
                }
                LbQuoteMsg.Visible = false;
                DvSubmit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtShowEntryReqList_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtRegList"];
                    GvReqList.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryReqList.Text == "" || TxtShowEntryReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryReqList.Text);
                    }
                    GvReqList.PageSize = ShowEntry;
                    GvReqList.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtShowEntryInvalidReqList_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtInValidRegList"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtInValidRegList"];
                    GdvInvalidRequest.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntryInvalidReqList.Text == "" || TxtShowEntryInvalidReqList.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntryInvalidReqList.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntryInvalidReqList.Text);
                    }
                    GdvInvalidRequest.PageSize = ShowEntry;
                    GdvInvalidRequest.DataBind();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Session["DtAllData"] = null;
                Session["DtValidData"] = null;
                Session["DtInValidData"] = null;
                Session["dtRegList"] = null;
                Session["DtForSubmit"] = null;
                Session["dtRegList"] = null;
                Session["dtInValidRegList"] = null;
                DvAllDataUpload.Visible = false;
                DvInvalidData.Visible = false;
                DvValidData.Visible = false;
                DvInvalidListRequest.Visible = false;
                //DvFormContrl1.Visible = false;
                //DvFormContrl2.Visible = false;
                DvCreateReq.Visible = false;
                DvGvReqList.Visible = false;
                DvSubmit.Visible = false;
                DvProceed.Visible = false;
                LbQuoteMsg.Visible = false;

                TxtResDueDate.Text = "";
                TxtValidDate.Text = "";
                TxtDuenextRev.Text = "";
                DdlReason.SelectedIndex = 0;
                txtRem.Text = "";

                DeleteNonRequest();
                SetDueOnDate();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {
            Session["DtValidData"] = null;
            Session["DtInValidData"] = null;
            Session["dtRegList"] = null;
            Session["DtForSubmit"] = null;
            Response.Redirect("Home.aspx");
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void CtnCloseMdl_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void StartTimer_Click(object sender, EventArgs e)
        {
            TimerCntDown.Enabled = true;
        }

        protected void TimerCntDown_Tick(object sender, EventArgs e)
        {
            if (TimerCntDown.Enabled == true)
            {
                int seconds = Int32.Parse((countdown.Text));
                seconds--;

                if (seconds < 0)
                {
                    TimerCntDown.Enabled = false;
                    Session.Abandon();
                    Session.Clear();
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    countdown.Text = seconds.ToString();
                }
            }
        }

        protected void sidebarToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["sidebarToggle"] == null)
                {
                    Session["sidebarToggle"] = "Hide";
                    SideBarMenu.Attributes.Add("style", "display:block;");
                }
                else
                {
                    Session["sidebarToggle"] = null;
                    SideBarMenu.Attributes.Add("style", "display:none;");
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("Login.aspx");
            }
            catch (ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
    }
}