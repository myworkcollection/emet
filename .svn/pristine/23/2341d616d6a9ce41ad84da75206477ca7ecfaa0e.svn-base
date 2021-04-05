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
        //email

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        SqlConnection con;
        SqlConnection conMaster;
        SqlConnection conServerMail;
        bool IsAth;
        bool sendingmail;
        string errmsg;
        string DbMasterName = "";
        string DbTransName = "";
        string GA = "";
        string PlantDesc = "";
        string SMNPICSubmDept = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OpenSqlConnection()
        {
            var connetionString = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            con = new SqlConnection(connetionString);
            con.Open();
        }

        protected void ShowData()
        {
            try
            {
                OpenSqlConnection();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct A.Materialtype,A.plantstatus,A.sapproctype,A.sapspproctype, 
                            A.product,A.partcode,A.processgroup,A.prijobtype,A.Duetdate,A.vedorcode
                            from TestingUpload A
                            inner join MDM.dbo.TMATERIAL TM ON A.partcode = TM.Material and A.Materialtype = TM.MaterialType
                            inner join MDM.dbo.TPLANTSTATUS TP ON A.plantstatus = TP.plantstatus
                            inner join MDM.dbo.tProcPIRType TPP ON A.sapproctype = TPP.ProcType
                            inner join MDM.dbo.TPRODUCT TPR ON A.Product = TPR.Product
                            inner join MDM.dbo.tVendor_New TV ON A.vedorcode = TV.Vendor
                            where 
                            --(A.prijobtype in (select (JobCode)+ '- '+ JobCodeDetailDescription from mdm.dbo.TPIRJOBTYPE_PROCESSGROUP ))
                            ((case when A.sapspproctype = 'blank' then 0 else A.sapspproctype end) in (select TPP.SPProcType from MDM.dbo.tProcPIRType TPP))
                             ";

                    cmd = new SqlCommand(sql, con);
                    //cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GvValidData.DataSource = dt;
                        GvValidData.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
            }
            finally
            {
                con.Close();
            }
        }

        protected void CreateRequest()
        {
            OpenSqlConnection();
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                sql = @" select distinct
                        '' as 'ReqDate',
                        '' as 'ReqNo',
                        '' as 'Plant',
                        '' as 'CompMaterial',
                        '' as 'CompMaterialDesc',
                        '' as 'VendorName',
                        '' as 'VendorCode',
                        '' as 'QuoteNo',
                        '' as 'PICName',
                        '' as 'Crcy',
                        '' as 'SearchTerm',
                        '' as 'AmtSCur',
                        '' as 'ExchRate',
                        '' as 'SellingCrcy',
                        '' as 'AmtVCur',
                        '' as 'Unit',
                        '' as 'UoM'
                        from TQuoteDetails TQ";

                cmd = new SqlCommand(sql, con);
                //cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    GvReqList.DataSource = dt;
                    GvReqList.DataBind();
                }
            }
        }

        protected void UploadData2()
        {
            string sql = "";
            OleDbCommand oleDBcmd = null;
            SqlCommand cmd = null;
            SqlTransaction trans = null;
            DataTable dtSource = null;
            DataTable dtInvalid = null;
            DataTable dtAllData = null;
            DateTime curTime = DateTime.Now;
            string excelCols = "[Plant],[PIR No],[Material Code],[Material Desc],[Vendor Code],[Vendor Name],[Process Group]";
            string excelRange = "A1:G5000";
            string excelFirstCol = "[PIR No]";
            string path = string.Concat(Server.MapPath("~/Files/" + FlUpload.FileName));
            FlUpload.SaveAs(path);
            try
            {
                dtSource = new DataTable();
                dtInvalid = new DataTable();
                dtAllData = new DataTable();
                OpenSqlConnection();
                trans = con.BeginTransaction();
                sql = @"create table #temp1
                            (
                                Plant nvarchar (MAX),
	                            PIRNo nvarchar (MAX),
	                            MaterialCode nvarchar (MAX),
	                            MaterialDesc nvarchar (MAX),
	                            VendorCode nvarchar (MAX),
	                            VendorName nvarchar (MAX),
	                            ProcessGroup nvarchar (MAX),
	                            EffectiveDate nvarchar (MAX),
                            ) ";
                cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();

                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", path);
                //Create Connection to Excel work book 
                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                {
                    //Create OleDbCommand to fetch data from Excel 
                    //using (oleDBcmd = new OleDbCommand("select * from [Sheet1$]", excelConnection))
                    using (oleDBcmd = new OleDbCommand("Select " + excelCols + " from [Sheet1$" + excelRange + "] where " + excelFirstCol + " is not null", excelConnection))
                    {
                        excelConnection.Open();
                        using (OleDbDataReader dReader = oleDBcmd.ExecuteReader())
                        {
                            using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans))
                            {
                                //Give your Destination table name 
                                sqlBulk.DestinationTableName = "#temp1";
                                sqlBulk.BulkCopyTimeout = 0;
                                sqlBulk.BatchSize = 10000;
                                sqlBulk.WriteToServer(dReader);
                            }
                        }
                    }
                }

                oleDBcmd.Dispose();
                
                //validdata
                sql = @" select distinct 
                            A.PIRNo,A.MaterialCode,A.MaterialDesc,A.VendorCode,A.VendorName,A.ProcessGroup into #temp2
                            from #temp1 A
                            inner join MDM.dbo.tPir_New TPIR ON A.MaterialCode = TPIR.Material and A.VendorCode  = TPIR.Vendor
                            inner join MDM.dbo.TBOMLIST TB ON A.MaterialCode = TB.Material
                            inner join MDM.dbo.TPROCESGROUP_LIST TP ON A.ProcessGroup = TP.Process_Grp_code
                            --inner join MDM.dbo.TMATERIAL TM ON A.MaterialCode = TM.Material
                            --inner join MDM.dbo.tVendor_New TV ON A.VendorCode = TV.Vendor
                            --inner join MDM.dbo.TCUSTOMER_MATLPRICING TCM ON TM.Material = TCM.Material
                            ";
                sql += @" select * from #temp2 ";

                cmd.CommandText = sql;
                dtSource.Load(cmd.ExecuteReader());
                GvValidData.DataSource = dtSource;
                GvValidData.DataBind();
                if (GvValidData.Rows.Count <= 0)
                {
                    GvValidData.Visible = false;
                    DvCreateReq.Visible = false;
                    DvFormContrl1.Visible = false;
                    DvFormContrl2.Visible = false;
                }
                else
                {
                    GvValidData.Visible = true;
                    DvCreateReq.Visible = true;
                    DvFormContrl1.Visible = true;
                    DvFormContrl2.Visible = true;
                }

                //invalid data
                sql = @" select distinct 
                            PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup
                            from #temp1
                         where PIRNo not in (select PIRNo from #temp2) ";
                sql += " drop table #temp1 ";
                sql += " drop table #temp2 ";
                cmd.CommandText = sql;
                dtInvalid.Load(cmd.ExecuteReader());
                GvInvalid.DataSource = dtInvalid;
                GvInvalid.DataBind();
                if (GvInvalid.Rows.Count <= 0)
                {
                    GvInvalid.Visible = false;
                }
                else
                {
                    GvInvalid.Visible = true;
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
            }
        }
        
        protected void UploadData()
        {
            try
            {
                string sql = "";
                OleDbCommand oleDBcmd = null;
                SqlCommand cmd = null;
                SqlTransaction trans = null;
                DataTable dtSource = null;
                DateTime curTime = DateTime.Now;
                string excelCols = "[PIR No],[Material Code],[Material Desc],[Vendor Code],[Vendor Name],[Process Group],[Effective Date From]";
                string excelRange = "A1:G5000";
                string excelFirstCol = "[PIR No]";
                string path = string.Concat(Server.MapPath("~/Files/" + FlUpload.FileName));
                FlUpload.SaveAs(path);
                try
                {
                    dtSource = new DataTable();
                    OpenSqlConnection();
                    trans = con.BeginTransaction();
                    sql = @"create table #temp1
                            (
	                            PIRNo nvarchar (MAX),
	                            MaterialCode nvarchar (MAX),
	                            MaterialDesc nvarchar (MAX),
	                            VendorCode nvarchar (MAX),
	                            VendorName nvarchar (MAX),
	                            ProcessGroup nvarchar (MAX),
	                            EffectiveDate nvarchar (MAX),
                            ) ";
                    cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();

                    String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", path);
                    //Create Connection to Excel work book 
                    using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                    {
                        //Create OleDbCommand to fetch data from Excel 
                        //using (oleDBcmd = new OleDbCommand("select * from [Sheet1$]", excelConnection))
                        using (oleDBcmd = new OleDbCommand("Select " + excelCols + " from [Sheet1$" + excelRange + "] where " + excelFirstCol + " is not null", excelConnection))
                        {
                            excelConnection.Open();
                            using (OleDbDataReader dReader = oleDBcmd.ExecuteReader())
                            {
                                using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans))
                                {
                                    //Give your Destination table name 
                                    sqlBulk.DestinationTableName = "#temp1";
                                    sqlBulk.BulkCopyTimeout = 0;
                                    sqlBulk.BatchSize = 10000;
                                    sqlBulk.WriteToServer(dReader);
                                }
                            }
                        }
                    }

                    oleDBcmd.Dispose();

                    sql = @" select distinct 
                            A.PIRNo,A.MaterialCode,A.MaterialDesc,A.VendorCode,A.VendorName,A.ProcessGroup,A.EffectiveDate
                            from #temp1 A
                            --inner join MDM.dbo.TMATERIAL TM ON A.Material = TM.Material and A.Materialtype = TM.MaterialType
                            --inner join MDM.dbo.TPLANTSTATUS TP ON A.PlantStatus = TP.plantstatus
                            --inner join MDM.dbo.tProcPIRType TPP ON A.SAPProcType = TPP.ProcType
                            --inner join MDM.dbo.TPRODUCT TPR ON A.Product = TPR.Product
                            --inner join MDM.dbo.tVendor_New TV ON A.vedorcode = TV.Vendor
                            --where 
                            --(A.prijobtype in (select (JobCode)+ '- '+ JobCodeDetailDescription from mdm.dbo.TPIRJOBTYPE_PROCESSGROUP ))
                            --((case when A.sapspproctype = 'blank' then 0 else A.sapspproctype end) in (select TPP.SPProcType from MDM.dbo.tProcPIRType TPP))
                             ";
                    sql += " drop table #temp1 ";
                    cmd.CommandText = sql;
                    dtSource.Load(cmd.ExecuteReader());
                    GvValidData.DataSource = dtSource;
                    GvValidData.DataBind();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if ((FlUpload.HasFile))
                {
                    UploadData2();
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void BtnTemplate_Click(object sender, EventArgs e)
        {
        }

        protected void BtnCreateRequest_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRequest();
            }
            catch (Exception ex)
            {
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
        }

        protected void GvReqList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
            }
        }

        protected void GvValidData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
            }
        }

        protected void GvInvalid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
            }
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
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
            catch (Exception ex2)
            {
                Response.Write(ex2);
            }
        }
    }
}