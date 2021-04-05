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
using System.Data.SqlTypes;
using System.Threading;

namespace Material_Evaluation
{
    public partial class NewReqWSAPgp : System.Web.UI.Page
    {
        CheckBox headerCheckBox = new CheckBox();
        int incNumber = 000000;


        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public string Source = "";
        public string RequestIncNumber1;
        public string Userid;
        public string userId;
        public string userId1;
        public static string password;
        public static string domain;
        public string path;
        public string SendFilename;
        public static Dictionary<int, string> objPirType;
        public static string nameC;
        public string aemail;
        public string pemail;
        public string Uemail;
        public string body1;
        public string URL;

        SqlDateTime sqldatenull;

        string MasterDB;
        string TransDB;
        public string createuser;
        string sql;
        SqlCommand cmd;
        bool IsAth;

        string ErrMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_NewReqWSAPgp";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            Session["hdnReqNoGp"] = null;
                            Session["FileAttach"] = null;

                            LoadSavings();
                            getdate();

                            HttpContext.Current.Session["SAPCode"] = "1";
                            RbWithouSAPGp.Checked = true;
                            RbExternal.Checked = true;

                            userId = Session["userID"].ToString();
                            userId1 = Session["userID"].ToString();
                            createuser = Session["userID"].ToString();
                            Label15.Text = Session["userID"].ToString();

                            string sname = Session["UserName"].ToString();
                            Label16.Text = Session["UserName"].ToString();
                            nameC = sname;
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;

                            lbluser1.Text = sname;
                            lblplant.Text = srole;
                            txtplant.Text = Session["EPlant"].ToString();
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            GetDdlProcesGroup();
                            DeleteNonRequest();
                            lblMessage.Text = "";
                            GetDdlReason();
                            GetDdlReqPlant();
                            GetDdlIncoterm();
                            GetImRecycleRatio();
                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }
                        }

                        SetDueOnDate();
                    }
                    else
                    {
                        if (DvExternal.Visible == true || DvTeamShmnVendor.Visible == true)
                        {
                            DvCreateRequest.Visible = true;
                        }
                        else
                        {
                            DvCreateRequest.Visible = false;
                        }

                        if (Session["sidebarToggle"] == null)
                        {
                            SideBarMenu.Attributes.Add("style", "display:block;");
                        }
                        else
                        {
                            SideBarMenu.Attributes.Add("style", "display:none;");
                        }

                        if (ddlprocess.SelectedValue == "IM")
                        {
                            DvImRcylRatio.Visible = true;
                        }
                        else
                        {
                            DvImRcylRatio.Visible = false;
                        }
                        getdate();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChangeEmptyFieldColor();DatePitcker();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChangeEmptyFieldColor();DatePitcker();multiselectDropDown();", true);
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        protected void GetDbTrans()
        {
            try
            {
                TransDB = EMETModule.GetDbTransname() + ".[dbo].";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                TransDB = "";
            }
        }

        protected void GetDbMaster()
        {
            try
            {
                MasterDB = EMETModule.GetDbTransname();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                MasterDB = "";
            }
        }
        
        protected void LoadSavings()
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
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void getdate()
        {
            txtReqDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtReqDate.Attributes.Add("disabled", "disabled");
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
                DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);
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
                    DefQuoteNextRev = DefQuoteNextRev.Replace(".", "-").Replace("/", "-");
                    if (ValidateDefQuoteNextRev(DefQuoteNextRev) == true)
                    {
                        DateTime DateDefQuoteNextRev = new DateTime();
                        DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);
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

                if (RbWithouSAPGp.Checked == true) {
                    TxtDuenextRev.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetImRecycleRatio()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct RecycleRatio from tIMRecycleratio where Plant='" + Session["EPlant"].ToString() + "' and DelFlag=0 order by RecycleRatio asc";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DdlImRcylRatio.DataSource = dt;
                        DdlImRcylRatio.DataTextField = "RecycleRatio";
                        DdlImRcylRatio.DataValueField = "RecycleRatio";
                        DdlImRcylRatio.DataBind();
                        if (DdlImRcylRatio.Items.Count > 1)
                        {
                            DdlImRcylRatio.Items.Insert(0, new ListItem("--Select--", "SELECT"));
                        }
                        else if (DdlImRcylRatio.Items.Count == 0)
                        {
                            DdlImRcylRatio.Items.Insert(0, new ListItem("IM Recycle Ration Not Exist", "NO DATA"));
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
        /// Get Process Group
        /// </summary>
        protected void GetDdlProcesGroup()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select A.ProcessGrp as 'Process_Grp_code',CONCAT(A.ProcessGrp,' - ',
                                (select Process_Grp_Description from TPROCESGROUP_LIST where Process_Grp_code=A.ProcessGrp)) as PrcGrpAndDesc from TPROCESGRP_SCREENLAYOUT A where A.DelFlag = 0 order by ProcessGrp ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@ProcessGrp", ddlprocess.SelectedValue.ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ddlprocess.Items.Clear();
                            ddlprocess.DataSource = dt;
                            ddlprocess.DataTextField = "PrcGrpAndDesc";
                            ddlprocess.DataValueField = "Process_Grp_code";
                            ddlprocess.DataBind();
                            if (dt.Rows.Count > 1)
                            {
                                ddlprocess.Items.Insert(0, new ListItem("-- Select Process --", "00"));
                            }
                        }
                        else
                        {
                            ddlprocess.Items.Clear();
                            ddlprocess.Items.Insert(0, new ListItem("--Process Not Exist--", "0"));
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

            string strprod = (string)HttpContext.Current.Session["plant"].ToString();
        }

        /// <summary>
        /// Get Reason for creation
        /// </summary>
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
                    cmd.Parameters.AddWithValue("@Plant", txtplant.Text);
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

        protected void GetDdlReqPlant()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,CONCAT(Plant, ' - ', Description) as PlantDesc, Description
                            from TPLANT where Plant <> @Plant and DelFlag = 0";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            //DdlPlantRequestor.Items.Clear();
                            //DdlPlantRequestor.DataTextField = "PlantDesc";
                            //DdlPlantRequestor.DataValueField = "Plant";

                            //DdlPlantRequestor.DataSource = dt;
                            //DdlPlantRequestor.DataBind();

                            LbSPlantRequestor.Items.Clear();
                            LbSPlantRequestor.DataTextField = "PlantDesc";
                            LbSPlantRequestor.DataValueField = "Plant";

                            LbSPlantRequestor.DataSource = dt;
                            LbSPlantRequestor.DataBind();
                            //if (dt.Rows.Count > 1)
                            //{
                            //    DdlPlantRequestor.Items.Insert(0, new ListItem("--Select GP Request Plant--", "00"));
                            //}
                        }
                        else
                        {
                            LbSPlantRequestor.Items.Clear();
                            //DdlPlantRequestor.Items.Clear();
                            //DdlPlantRequestor.Items.Insert(0, new ListItem("--GP Request Plant Not Exist--", "0"));
                        }
                    }
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

        protected void GetDdlIncoterm()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Incoterm,CONCAT(Incoterm, ' - ', Description) as IncDesc 
                            from TINCOTERM where DelFlag = 0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DdlIncoterms.Items.Clear();
                            DdlIncoterms.DataTextField = "IncDesc";
                            DdlIncoterms.DataValueField = "Incoterm";

                            DdlIncoterms.DataSource = dt;
                            DdlIncoterms.DataBind();
                            if (dt.Rows.Count > 1)
                            {
                                DdlIncoterms.Items.Insert(0, new ListItem("--Select Incoterm--", "00"));
                            }
                        }
                        else
                        {
                            DdlIncoterms.Items.Clear();
                            DdlIncoterms.Items.Insert(0, new ListItem("--Incoterm Not Exist--", "0"));
                        }
                    }
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

        /// <summary>
        /// Load vendor based on Process Group
        /// </summary>
        /// <param name="processgrp"></param>
        protected void vendorload(string processgrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
                //string str = "select TVP.VendorCode,TVP.VendorName, tv.SearchTerm from TVENDOR_PROCESSGROUP TVP inner join tVendor_New tv ON TVP.VendorCode = tv.Vendor inner join TVENDORPIC as VP on VP.VendorCode=tv.Vendor inner join TPOrgPlant as tp on tp.porg=tv.POrg Where TVP.processgrp='" + processgrp + "'  and tp.plant='" + txtplant.Text + "' and (TVP.DelFlag = 0)";
                string str = @"select distinct TVP.VendorCode,TVP.VendorName, tv.CodeRef as SearchTerm 
                                from TVENDOR_PROCESSGROUP TVP 
                                inner join tvendorporg tv ON TVP.VendorCode = tv.Vendor 
                                inner join TVENDORPIC as VP on VP.VendorCode = tv.Vendor 
                                inner join TPOrgPlant as tp on tp.porg = tv.POrg 
                                Where TVP.processgrp = '" + processgrp + @"'  
                                and tp.plant = '" + Session["EPlant"].ToString() + @"' 
                                and tv.plant = '" + Session["EPlant"].ToString() + "' and ( TVP.VendorCode not in (select VendorCode from TSBMPRICINGPOLICY)) and (tv.DelFlag = 0) and (TVP.DelFlag = 0)";

                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                //     Result.Rows.Add(dr);

                if (Result.Rows.Count > 0)
                {
                    grdvendor.DataSource = Result;
                    grdvendor.DataBind();
                }
                else
                {
                    grdvendor.DataSource = Result;
                    grdvendor.DataBind();
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
        /// Load vendor based on Process Group for tool amortize
        /// </summary>
        protected void vendorloadToolAmor()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct TVP.VendorCode,TVP.VendorName, tv.CodeRef as SearchTerm 
                             ,tt.ToolTypeID,tt.Amortize_Tool_ID,tt.Amortize_Tool_Desc
                            ,tt.AmortizeCost, tt.AmortizeCurrency,tt.ExchangeRate
                            ,tt.AmortizePeriod,tt.AmortizePeriodUOM,tt.TotalAmortizeQty,tt.QtyUOM
                            ,IIF( right(tt.AmortizeCost_Vend_Curr,4) = '000000', convert(nvarchar(50),convert(int,tt.AmortizeCost_Vend_Curr)) ,convert(nvarchar(50),tt.AmortizeCost_Vend_Curr) ) as AmortizeCost_Vend_Curr
                            ,IIF( right(tt.AmortizeCost_Pc_Vend_Curr,4) = '000000', convert(nvarchar(50),convert(int,tt.AmortizeCost_Pc_Vend_Curr)) ,convert(nvarchar(50),tt.AmortizeCost_Pc_Vend_Curr) ) as AmortizeCost_Pc_Vend_Curr
                            ,tt.EffectiveFrom, tt.DueDate
							, FORMAT(tt.EffectiveFrom, 'yyyy-MM-dd') as EeffDt
							, FORMAT(tt.DueDate, 'yyyy-MM-dd') as DuDate
                            from TVENDOR_PROCESSGROUP TVP 
                            inner join tvendorporg tv ON TVP.VendorCode = tv.Vendor 
                            inner join TVENDORPIC as VP on VP.VendorCode = tv.Vendor 
                            inner join TPOrgPlant as tp on tp.porg = tv.POrg 
                            inner join TToolAmortization as TT on tv.Plant = tt.Plant and tv.Vendor = tt.VendorCode and TVP.ProcessGrp = tt.Process_Grp_code
                            Where TVP.processgrp = @processgrp
                            and tp.plant = @Plant
                            and tv.plant = @Plant
                            and vp.plant = @Plant
                            and (tv.DelFlag = 0) 
                            and (TVP.DelFlag = 0) 
                            and (ISNULL(tt.DelFlag,0)=0) and (tt.EffectiveFrom is null or @EffectiveDate between EffectiveFrom and DueDate) ";
                    if (RbExternal.Checked == true)
                    {
                        sql += @" and ( TVP.VendorCode not in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    else
                    {
                        sql += @" and ( TVP.VendorCode in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    sql += @" order by TVP.VendorCode asc ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@processgrp", ddlprocess.SelectedValue.ToString());
                    DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["VndToolAmortize"] = dt;
                        GvVndToolAmortize.DataSource = dt;
                        GvVndToolAmortize.DataBind();
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

        protected void ResetFormCtrlBackColor()
        {
            try
            {

                txtpartdesc.BackColor = Color.White;
                txtpartdescription.BackColor = Color.White;
                txtMQty.BackColor = Color.White;
                txtBaseUOM1.BackColor = Color.White;
                txtunitweight.BackColor = Color.White;
                txtUOM.BackColor = Color.White;
                TxtFADate.BackColor = Color.White;
                TxtFAQty.BackColor = Color.White;
                TxtDelDate.BackColor = Color.White;
                TxtDelQty.BackColor = Color.White;
                DdlReason.BackColor = Color.White;
                DdlIncoterms.BackColor = Color.White;
                TxtPckRequirement.BackColor = Color.White;
                TxtOthRequirement.BackColor = Color.White;
                //DdlPlantRequestor.BackColor = Color.White;
                txtplatingtype.BackColor = Color.White;
                FileUpload1.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7F7F7");
                ddlprocess.BackColor = Color.White;
                txtDate.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        bool createRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();
                string txtplatestring = txtplatingtype.Text;

                string strdate = txtReqDate.Text;


                string dg_checkvalue, dg_formid = string.Empty;
                string getQuoteRef = string.Empty;
                string formatstatus = string.Empty;
                string remarks = string.Empty;
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string currentYear_ = DateTime.Now.Year.ToString();
                currentYear = currentYear.Substring(currentYear.Length - 2);


                string RequestIncNumber = "";

                int increquest = 00000;
                string RequestInc = String.Format(currentYear, increquest);

                string REQUEST = string.Concat(currentYear, RequestIncNumber);

                using (SqlDataAdapter sda = new SqlDataAdapter()) {
                    sql = @"select MAX(RequestNumber) as MaxRequestNumber from TQuoteDetails";
                    cmd = new SqlCommand(sql,EmetCon,EmetTrans);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable()) {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0) {
                            string curentYear = DateTime.Now.Year.ToString();
                            curentYear = curentYear.Substring(curentYear.Length - 2);
                            currentYear = currentYear.Substring(currentYear.Length - 2);
                            string ReqNum = dt.Rows[0]["MaxRequestNumber"].ToString();

                            if (ReqNum == "")
                            {
                                RequestIncNumber = "00000";
                                RequestIncNumber1 = "00000";
                                Session["RequestIncNumber1"] = "00000";
                                string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber = string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber1 = string.Concat(currentYear, RequestIncNumber);
                                Session["RequestIncNumber1"] = string.Concat(currentYear, RequestIncNumber);
                            }
                            else
                            {

                                ReqNum = ReqNum.Remove(0, 2);
                                ReqNum = string.Concat(currentYear, ReqNum);
                                RequestIncNumber = ReqNum;
                                RequestIncNumber1 = ReqNum;
                                Session["RequestIncNumber1"] = ReqNum;
                            }
                            int newReq = (int.Parse(RequestIncNumber)) + (1);
                            RequestIncNumber = newReq.ToString();
                            RequestIncNumber1 = RequestIncNumber;
                            Session["RequestIncNumber1"] = RequestIncNumber;
                        }
                    }
                }

                Session["RequestNo"] = RequestIncNumber.ToString();

                if ((RbExternal.Checked == true && DdlToolAmortize.SelectedValue == "YES") || (RbExternal.Checked == true && DdlToolAmortize.SelectedValue == "NO") || (RbTeamShimano.Checked == true && DdlToolAmortize.SelectedValue == "YES"))
                {
                    #region save method External vendor
                    int rowscount = 0;
                    var GrdVndor = new GridView();

                    if (DdlToolAmortize.SelectedValue == "YES")
                    {
                        GrdVndor = GvVndToolAmortize;
                    }
                    else if (DdlToolAmortize.SelectedValue == "NO")
                    {
                        GrdVndor = grdvendor;
                    }
                    int c = GrdVndor.Rows.Count;
                    foreach (GridViewRow gr in GrdVndor.Rows)
                    {
                        CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                        if (myCheckbox.Checked == true)
                        {
                            dg_checkvalue = "Y";
                            dg_formid = gr.Cells[1].Text.ToString();

                            string dg_VenName = (gr.Cells[2].Text.ToString()).Replace("amp;", " ");

                            dg_checkvalue = gr.Cells[2].Text.ToString();
                            string strvendname = dg_checkvalue.ToString();
                            //subash/////string mstrvenname = strvendname.Substring(0, 4);

                            //  string formattedIncNumber = String.Format("{000:D6}", incNumber);
                            //subash////string getVendName = mstrvenname[0].ToString();

                            string QuoteSearchTerm = "";
                            QuoteSearchTerm = gr.Cells[3].Text.ToString();
                            QuoteSearchTerm = QuoteSearchTerm.Substring(0, 3);
                            string getquote = String.Concat(QuoteSearchTerm, RequestIncNumber);
                            //string getquote = String.Concat(mstrvenname, RequestIncNumber);
                            string strPlatingType = txtplatingtype.Text;


                            getQuoteRef = getquote;

                            remarks = "Open Status";
                            rowscount++;

                            string query = "";
                            query = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,Material,MaterialDesc,MQty,BaseUOM,NetUnit,ActualNU,UOM,
                                            FADate,FAQty,DelDate,DelQty,ERemarks,PICReason,Incoterm,PckReqrmnt,OthReqrmnt,
                                            PlatingType,VendorCode1,VendorName,ProcessGroup,DrawingNo,QuoteResponseDueDate,EffectiveDate,DueOn,CreatedBy
                                            ,ApprovalStatus,PICApprovalStatus,ManagerApprovalStatus,DIRApprovalStatus,isUseSAPCode,SMNPicDept,IMRecycleRatio,IsUseToolAmortize) 
                                            values (@RequestNumber,@RequestDate,@Plant,@QuoteNo,@Material,@MaterialDesc,@MQty,@BaseUOM,@NetUnit,@ActualNU,@UOM,
                                            @FADate,@FAQty,@DelDate,@DelQty,@ERemarks,@PICReason,@Incoterm,@PckReqrmnt,@OthReqrmnt,
                                            @PlatingType,@VendorCode1,@VendorName,@ProcessGroup,@DrawingNo,@QuoteResponseDueDate,@EffectiveDate,@DueOn,@CreatedBy,4,4,4,4,0,@SMNPicDept,@IMRecycleRatio,@IsUseToolAmortize)";

                            cmd = new SqlCommand(query, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@RequestNumber", Convert.ToInt32(RequestIncNumber.ToString()));
                            DateTime DateReq = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);
                            cmd.Parameters.AddWithValue("@RequestDate", DateReq.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                            cmd.Parameters.AddWithValue("@QuoteNo", (getQuoteRef.ToString() + "GP"));
                            cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text);
                            cmd.Parameters.AddWithValue("@MaterialDesc", txtpartdescription.Text);
                            cmd.Parameters.AddWithValue("@MQty", txtMQty.Text);
                            cmd.Parameters.AddWithValue("@BaseUOM", txtBaseUOM1.Text);
                            cmd.Parameters.AddWithValue("@NetUnit", txtunitweight.Text);
                            cmd.Parameters.AddWithValue("@ActualNU", txtunitweight.Text);
                            cmd.Parameters.AddWithValue("@UOM", txtUOM.Text);

                            if (TxtFADate.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@FADate", sqldatenull);
                            }
                            else
                            {
                                DateTime FADate = DateTime.ParseExact(TxtFADate.Text, "dd-MM-yyyy", null);
                                cmd.Parameters.AddWithValue("@FADate", FADate.ToString("yyyy/MM/dd HH:mm:ss"));
                            }

                            if (TxtFAQty.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@FAQty", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@FAQty", TxtFAQty.Text);
                            }

                            if (TxtDelDate.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@DelDate", sqldatenull);
                            }
                            else
                            {
                                DateTime DelDate = DateTime.ParseExact(TxtDelDate.Text, "dd-MM-yyyy", null);
                                cmd.Parameters.AddWithValue("@DelDate", DelDate.ToString("yyyy/MM/dd HH:mm:ss"));
                            }

                            if (TxtDelQty.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@DelQty", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@DelQty", TxtDelQty.Text);
                            }

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

                            cmd.Parameters.AddWithValue("@Incoterm", DdlIncoterms.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PckReqrmnt", TxtPckRequirement.Text);
                            cmd.Parameters.AddWithValue("@OthReqrmnt", TxtOthRequirement.Text);
                            //cmd.Parameters.AddWithValue("@ReqPlant", DdlPlantRequestor.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PlatingType", txtplatingtype.Text);
                            cmd.Parameters.AddWithValue("@VendorCode1", dg_formid.ToString());
                            cmd.Parameters.AddWithValue("@VendorName", dg_VenName.ToString());
                            cmd.Parameters.AddWithValue("@ProcessGroup", ddlprocess.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@DrawingNo", lblMessage.Text);

                            DateTime DueDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                            cmd.Parameters.AddWithValue("@QuoteResponseDueDate", DueDate.ToString("yyyy/MM/dd HH:mm:ss"));

                            if (TxtEffectiveDate.Text != "")
                            {
                                DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                                cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@EffectiveDate", DBNull.Value);
                            }

                            DateTime DtDueOn = DateTime.ParseExact(TxtDuenextRev.Text, "dd-MM-yyyy", null);
                            cmd.Parameters.AddWithValue("@DueOn", DtDueOn.ToString("yyyy-MM-dd"));

                            cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());
                            if (ddlprocess.SelectedValue == "IM")
                            {
                                cmd.Parameters.AddWithValue("@IMRecycleRatio", DdlImRcylRatio.SelectedValue.ToString());
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@IMRecycleRatio", DBNull.Value);
                            }
                            if (DdlToolAmortize.SelectedValue == "YES")
                            {
                                cmd.Parameters.AddWithValue("@IsUseToolAmortize", true);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@IsUseToolAmortize", false);
                            }
                            cmd.ExecuteNonQuery();

                            #region delete duplicate record
                            sql = @"  WITH cte AS (
                                            SELECT material, plant,RequestNumber,QuoteNo, 
                                                ROW_NUMBER() OVER (
                                                    PARTITION BY material, plant,RequestNumber,QuoteNo
                                                    ORDER BY material, plant,RequestNumber,QuoteNo
                                                ) row_num
                                             FROM TQuoteDetails
                                        )
                                        DELETE FROM cte
                                        WHERE row_num > 1; ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.ExecuteNonQuery();
                            #endregion
                        }
                        else
                        {
                            dg_checkvalue = "";
                        }
                    }
                    #endregion
                }
                else
                {
                    #region save Team Shimano
                    string QuoteSearchTerm = "";
                    QuoteSearchTerm = TxtSrcTerm.Text;
                    QuoteSearchTerm = QuoteSearchTerm.Substring(0, 3);
                    string getquote = String.Concat(QuoteSearchTerm, RequestIncNumber);
                    getQuoteRef = getquote;
                    remarks = "Open Status";

                    string query = "";
                    query = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,Material,MaterialDesc,MQty,BaseUOM,NetUnit,ActualNU,UOM,
                                            FADate,FAQty,DelDate,DelQty,ERemarks,PICReason,Incoterm,PckReqrmnt,OthReqrmnt,
                                            PlatingType,VendorCode1,VendorName,ProcessGroup,DrawingNo,QuoteResponseDueDate,EffectiveDate,DueOn,CreatedBy
                                            ,ApprovalStatus,PICApprovalStatus,ManagerApprovalStatus,DIRApprovalStatus,isUseSAPCode,SMNPicDept,IMRecycleRatio) 
                                            values (@RequestNumber,@RequestDate,@Plant,@QuoteNo,@Material,@MaterialDesc,@MQty,@BaseUOM,@NetUnit,@ActualNU,@UOM,
                                            @FADate,@FAQty,@DelDate,@DelQty,@ERemarks,@PICReason,@Incoterm,@PckReqrmnt,@OthReqrmnt,
                                            @PlatingType,@VendorCode1,@VendorName,@ProcessGroup,@DrawingNo,@QuoteResponseDueDate,@EffectiveDate,@DueOn,@CreatedBy,4,4,4,4,0,@SMNPicDept,@IMRecycleRatio)";


                    cmd = new SqlCommand(query, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@RequestNumber", Convert.ToInt32(RequestIncNumber.ToString()));

                    DateTime DateReq = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@RequestDate", DateReq.ToString("yyyy/MM/dd HH:mm:ss"));

                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@QuoteNo", (getQuoteRef.ToString() + "GP"));
                    cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text);
                    cmd.Parameters.AddWithValue("@MaterialDesc", txtpartdescription.Text);
                    cmd.Parameters.AddWithValue("@MQty", txtMQty.Text);
                    cmd.Parameters.AddWithValue("@BaseUOM", txtBaseUOM1.Text);
                    cmd.Parameters.AddWithValue("@NetUnit", txtunitweight.Text);
                    cmd.Parameters.AddWithValue("@ActualNU", txtunitweight.Text);
                    cmd.Parameters.AddWithValue("@UOM", txtUOM.Text);

                    if (TxtFADate.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@FADate", sqldatenull);
                    }
                    else
                    {
                        DateTime FADate = DateTime.ParseExact(TxtFADate.Text, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@FADate", FADate.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    if (TxtFAQty.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@FAQty", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@FAQty", TxtFAQty.Text);
                    }

                    if (TxtDelDate.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@DelDate", sqldatenull);
                    }
                    else
                    {
                        DateTime DelDate = DateTime.ParseExact(TxtDelDate.Text, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@DelDate", DelDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    if (TxtDelQty.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@DelQty", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DelQty", TxtDelQty.Text);
                    }

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

                    cmd.Parameters.AddWithValue("@Incoterm", DdlIncoterms.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PckReqrmnt", TxtPckRequirement.Text);
                    cmd.Parameters.AddWithValue("@OthReqrmnt", TxtOthRequirement.Text);
                    //cmd.Parameters.AddWithValue("@ReqPlant", DdlPlantRequestor.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@PlatingType", txtplatingtype.Text);
                    cmd.Parameters.AddWithValue("@VendorCode1", DdlVendor.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@VendorName", TxtVendorDesc.Text.ToString());
                    cmd.Parameters.AddWithValue("@ProcessGroup", ddlprocess.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@DrawingNo", lblMessage.Text);

                    DateTime DueDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@QuoteResponseDueDate", DueDate.ToString("yyyy/MM/dd HH:mm:ss"));

                    if (TxtEffectiveDate.Text != "")
                    {
                        DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EffectiveDate", DBNull.Value);
                    }

                    DateTime DtDueOn = DateTime.ParseExact(TxtDuenextRev.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@DueOn", DtDueOn.ToString("yyyy-MM-dd"));

                    cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                    cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());
                    if (ddlprocess.SelectedValue == "IM")
                    {
                        cmd.Parameters.AddWithValue("@IMRecycleRatio", DdlImRcylRatio.SelectedValue.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IMRecycleRatio", DBNull.Value);
                    }
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();

                    #endregion
                }
                
                EmetTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                ErrMsg = ex.Message.ToString();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally {
                EmetCon.Dispose();
            }
        }

        protected void RbWithouSAPCode_CheckedChanged(object sender, EventArgs e)
        {
            Response.Redirect("NewRequest.aspx?num=2");
        }

        protected void article_CheckedChanged(object sender, EventArgs e)
        {
            Response.Redirect("NewRequest.aspx?num=1");
        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                string dg_checkvalue, dg_formid = string.Empty;
                CheckBox ChkBoxHeader = (CheckBox)grdvendor.HeaderRow.FindControl("chkheader");
                foreach (GridViewRow row in grdvendor.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkchild");
                    if (ChkBoxHeader.Checked == true)
                    {
                        ChkBoxRows.Checked = true;

                        dg_formid = row.Cells[0].Text;

                    }
                    else
                    {
                        ChkBoxRows.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        /// <summary>
        /// get data for dropdown plant when vendor type is team shimano
        /// </summary>
        /// <param name="ReqNo"></param>
        protected void GetDdlVendor(string procgrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct A.VendorCode, CONCAT(A.VendorCode, ' - ', (select distinct Description from tVendor_New where Vendor = a.VendorCode)) as 'VendorCodeAndDesc' from TSBMPRICINGPOLICY A 
                             inner join TVENDOR_PROCESSGROUP B on A.VendorCode = B.VendorCode
                            inner join tvendorporg C on B.VendorCode = C.Vendor where B.ProcessGrp = @ProcessGrp and A.DelFlag=0 and A.Plant=@Plant ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@ProcessGrp", ddlprocess.SelectedValue.ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DdlVendor.Items.Clear();
                            DdlVendor.DataTextField = "VendorCodeAndDesc";
                            DdlVendor.DataValueField = "VendorCode";

                            DdlVendor.DataSource = dt;
                            DdlVendor.DataBind();
                            if (dt.Rows.Count > 1)
                            {
                                DdlVendor.Items.Insert(0, new ListItem("--Select Vendor--", "00"));
                            }
                        }
                        else
                        {
                            DdlVendor.Items.Clear();
                            DdlVendor.Items.Insert(0, new ListItem("--Vendor Not Exist--", "0"));
                        }
                    }
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

        protected void GetTeamSmnVendor(string VendorCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct A.plant,(select PL.Description from TPLANT PL where PL.plant=A.plant)as PlantDesc , C.CodeRef as SearchTerm,
                            (select distinct TV.Description from tVendor_New TV where TV.Vendor=A.VendorCode) as VendDesc
                            from TSBMPRICINGPOLICY A 
                            inner join TVENDOR_PROCESSGROUP B on A.VendorCode = B.VendorCode
                            inner join tvendorporg C on B.VendorCode = C.Vendor
                            where A.VendorCode=@VendorCode and A.Delflag = 0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            TxtPlantVendor.Text = dt.Rows[0]["plant"].ToString();
                            TxtPlantVendorDesc.Text = dt.Rows[0]["PlantDesc"].ToString();
                            TxtSrcTerm.Text = dt.Rows[0]["SearchTerm"].ToString();
                            TxtVendorDesc.Text = dt.Rows[0]["VendDesc"].ToString();
                        }
                        else
                        {
                            TxtPlantVendor.Text = "";
                            TxtPlantVendorDesc.Text = "";
                            TxtSrcTerm.Text = "";
                            TxtVendorDesc.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtPlantVendor.Text = "";
                TxtPlantVendorDesc.Text = "";
                TxtSrcTerm.Text = "";
                TxtVendorDesc.Text = "";
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
        
        /// <summary>
        /// Process DDL index change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void ddlprocess_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            if (DdlToolAmortize.SelectedValue == "YES")
            {
                DvVndToolAmortize.Visible = true;
                DvExternal.Visible = false;
                DvTeamShmnVendor.Visible = false;
                vendorloadToolAmor();
                DvCreateRequest.Visible = true;
            }
            else if (DdlToolAmortize.SelectedValue == "NO")
            {
                DvVndToolAmortize.Visible = false;
                CekVendorType();
                Button1.Visible = false;
                DvCreateRequest.Visible = true;
            }
            
        }

        protected void TxtEffectiveDate_TextChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            Session["process"] = ddlprocess.SelectedValue.ToString();
            if (DdlToolAmortize.SelectedValue == "YES")
            {
                DvVndToolAmortize.Visible = true;
                DvExternal.Visible = false;
                DvTeamShmnVendor.Visible = false;
                vendorloadToolAmor();
                DvCreateRequest.Visible = true;
            }
            else if (DdlToolAmortize.SelectedValue == "NO")
            {
                DvVndToolAmortize.Visible = false;
                CekVendorType();
                Button1.Visible = false;
                DvCreateRequest.Visible = true;
            }

            //this.vendorload(strprod);
        }

        protected void DdlToolAmortize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            if (DdlToolAmortize.SelectedValue == "YES" && TxtEffectiveDate.Text == "")
            {
                DdlToolAmortize.SelectedValue = "0";
                DvVndToolAmortize.Visible = false;
                DvTeamShmnVendor.Visible = false;
                DvExternal.Visible = false;
                DvCreateRequest.Visible = false;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "alert('Please Select Effective Date First');", true);
            }
            else
            {
                Session["process"] = ddlprocess.SelectedValue.ToString();
                if (DdlToolAmortize.SelectedValue == "YES")
                {
                    DvVndToolAmortize.Visible = true;
                    DvExternal.Visible = false;
                    DvTeamShmnVendor.Visible = false;
                    vendorloadToolAmor();
                    DvCreateRequest.Visible = true;
                }
                else if (DdlToolAmortize.SelectedValue == "NO")
                {
                    DvVndToolAmortize.Visible = false;
                    CekVendorType();
                    Button1.Visible = false;
                    DvCreateRequest.Visible = true;
                }
                else
                {
                    DvVndToolAmortize.Visible = false;
                    DvTeamShmnVendor.Visible = false;
                    DvExternal.Visible = false;
                    DvCreateRequest.Visible = false;
                }

            }
            //this.vendorload(strprod);
        }

        protected void DdlReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DdlReason.SelectedValue == "Others")
                {
                    txtRem.Visible = true;
                }
                else
                {
                    txtRem.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        /// <summary>
        /// Create Request button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {

                DeleteNonRequest();
                if (DdlToolAmortize.SelectedValue == "YES")
                {
                    bool IsVendorSelected = false;
                    foreach (GridViewRow gr in GvVndToolAmortize.Rows)
                    {
                        CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                        if (myCheckbox.Checked == true)
                        {
                            IsVendorSelected = true;
                        }
                    }

                    if (IsVendorSelected)
                    {
                        if (createRequest() == true)
                        {
                            if (Session["RequestNo"] != null)
                            {
                                GetData(Session["RequestNo"].ToString());
                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
                            }
                        }
                        else
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Create Req Due On : " + ErrMsg);
                            var script = string.Format("alert({0});CloseLoading();", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors has selected. Please select at least one vendor!')CloseLoading();", true);
                    }
                }
                else if (DdlToolAmortize.SelectedValue == "NO")
                {
                    if (RbExternal.Checked == true)
                    {
                        bool IsVendorSelected = false;
                        foreach (GridViewRow gr in grdvendor.Rows)
                        {
                            CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                            if (myCheckbox.Checked == true)
                            {
                                IsVendorSelected = true;
                            }
                        }

                        if (IsVendorSelected)
                        {
                            if (createRequest() == true)
                            {
                                if (Session["RequestNo"] != null)
                                {
                                    GetData(Session["RequestNo"].ToString());
                                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
                                }
                            }
                            else
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Create Req Due On : " + ErrMsg);
                                var script = string.Format("alert({0});CloseLoading();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors has selected. Please select at least one vendor!');CloseLoading();", true);
                        }
                    }
                    else if (RbTeamShimano.Checked == true)
                    {
                        string VendorCode = "";
                        VendorCode = DdlVendor.SelectedValue.ToString();
                        if (VendorCode == "00")
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select Vendor !');CloseLoading();", true);
                        }
                        else if (VendorCode == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Vendor Not Exist!');CloseLoading();", true);
                        }
                        else
                        {
                            if (createRequest() == true)
                            {
                                if (Session["RequestNo"] != null)
                                {
                                    GetData(Session["RequestNo"].ToString());
                                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
                                }
                            }
                            else
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Create Req Due On : " + ErrMsg);
                                var script = string.Format("alert({0});CloseLoading();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('please select Vendor Type !');CloseLoading();", true);
                        RbExternal.BackColor = Color.LightPink;
                        RbTeamShimano.BackColor = Color.LightPink;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox chkAll = (CheckBox)grdvendor.HeaderRow.FindControl("chkSelectAll");
                if (chkAll.Checked == true)
                {
                    foreach (GridViewRow gvRow in grdvendor.Rows)
                    {
                        CheckBox chkSel =
                             (CheckBox)gvRow.FindControl("chk");
                        chkSel.Checked = true;

                    }
                }
                else
                {
                    foreach (GridViewRow gvRow in grdvendor.Rows)
                    {
                        CheckBox chkSel = (CheckBox)gvRow.FindControl("chk");
                        chkSel.Checked = false;

                    }
                }
                DeleteNonRequest();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                if (grdvendor.Rows.Count > 0)
                {
                    CheckBox CheckAll = (CheckBox)grdvendor.HeaderRow.FindControl("chkSelectAll");
                    for (int i = 0; i < grdvendor.Rows.Count; i++)
                    {
                        CheckBox CheckVnd = (CheckBox)grdvendor.Rows[i].FindControl("chk");
                        if (CheckVnd.Checked == true)
                        {
                            count++;
                        }
                    }

                    if (count == grdvendor.Rows.Count)
                    {
                        CheckAll.Checked = true;
                    }
                    else
                    {
                        CheckAll.Checked = false;
                    }
                }
                DeleteNonRequest();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void chkSelectAllVndToolAmor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox chkAll = (CheckBox)GvVndToolAmortize.HeaderRow.FindControl("chkSelectAllVndToolAmor");
                if (chkAll.Checked == true)
                {
                    //foreach (GridViewRow gvRow in GvVndToolAmortize.Rows)
                    //{
                    //    CheckBox chkSel =(CheckBox)GvVndToolAmortize.FindControl("chkVndToolAmor");
                    //    chkSel.Checked = true;

                    //}
                    for (int i = 0; i < GvVndToolAmortize.Rows.Count; i++)
                    {
                        CheckBox CheckVnd = (CheckBox)GvVndToolAmortize.Rows[i].FindControl("chkVndToolAmor");
                        CheckVnd.Checked = true;
                    }
                }
                else
                {
                    //foreach (GridViewRow gvRow in GvVndToolAmortize.Rows)
                    //{
                    //    CheckBox chkSel = (CheckBox)gvRow.FindControl("chkVndToolAmor");
                    //    chkSel.Checked = false;

                    //}
                    for (int i = 0; i < GvVndToolAmortize.Rows.Count; i++)
                    {
                        CheckBox CheckVnd = (CheckBox)GvVndToolAmortize.Rows[i].FindControl("chkVndToolAmor");
                        CheckVnd.Checked = false;
                    }
                }
                DeleteNonRequest();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void chkVndToolAmor_CheckedChanged(object sender, EventArgs e)
        {
            LbMsgErr.Text = "";
            try
            {
                int count = 0;
                CheckBox sndrchk = (CheckBox)sender;
                bool SenderChecked = sndrchk.Checked;
                string SenderValue = sndrchk.Text;

                if (GvVndToolAmortize.Rows.Count > 0)
                {
                    CheckBox CheckAll = (CheckBox)GvVndToolAmortize.HeaderRow.FindControl("chkSelectAllVndToolAmor");
                    for (int i = 0; i < GvVndToolAmortize.Rows.Count; i++)
                    {
                        CheckBox CheckVnd = (CheckBox)GvVndToolAmortize.Rows[i].FindControl("chkVndToolAmor");

                        string VendorCode = CheckVnd.Text;
                        if (SenderValue == VendorCode)
                        {
                            CheckVnd.Checked = false;
                        }

                        if (CheckVnd.Checked == true)
                        {
                            count++;
                        }
                    }

                    if (count == GvVndToolAmortize.Rows.Count)
                    {
                        CheckAll.Checked = true;
                    }
                    else
                    {
                        CheckAll.Checked = false;
                    }
                }
                if (SenderChecked == true)
                {
                    sndrchk.Checked = true;
                }
                else
                {
                    sndrchk.Checked = false;
                }

                DeleteNonRequest();
                DvCreateRequest.Visible = true;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            if (LbMsgErr.Text == "")
            {
                var script = string.Format("CloseLoading();");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
            else
            {
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Create Req Due On : " + LbMsgErr.Text);
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }


        /// <summary>
        /// Drawing number upload button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                Session["FileAttach"] = FileUpload1;
                lblMessage.Text = FileUpload1.FileName.ToString();
                FileUpload1 = (FileUpload)Session["FileAttach"];
            }
        }
        
        /// <summary>
        /// Load data and Create Dynamic table with BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
        protected void GetData(string reqno)
        {
            try
            {
                GetDbTrans();
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

                    //subash
                    //OriginalFilename = Session["OriginalFilename"].ToString();
                    //string Destination = Session["path"].ToString() + OriginalFilename;
                    //using (new SoddingNetworkAuth(Userid, domain, password))
                    //{
                    //    Source = Session["Source"].ToString();
                    //    //File.Copy(Source, Destination, true);
                    //    fname = Session["fname"].ToString();
                    //    SendFilename = fname.Remove(fname.Length - 4);
                    //    Session["SendFilename"]= fname.Remove(fname.Length - 4);
                    //    OriginalFilename = Session["OriginalFilename"].ToString();
                    //    OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                    //    Session["OriginalFilename"] = OriginalFilename.ToString();
                    //}
                    //end

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtget = new DataTable();
                DataTable dtget1 = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da1 = new SqlDataAdapter();

                string strGetData = string.Empty;
                string strGetData1 = string.Empty;

                string plant = txtplant.Text;
                if (RbTeamShimano.Checked == true)
                {
                    plant = TxtPlantVendor.Text;
                }

                strGetData1 = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tvpo.Plant,TB.Material as [Comp Material],
                            tm.MaterialDesc as [Comp Material Desc],v.Description as VendorName,tvpo.coderef as SearchTerm,TQ.vendorCode1 as VendorCode,TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,
                            tc.Amount as Amt_SCur,isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as ExchRate,v.Crcy AS Venor_Crcy, 
                            isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,tc.Unit,tc.UoM 
                            from TMATERIAL tm  inner join TBOMLIST TB on tm.Material = TB.Material  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material   
                            inner join tVendor_New v on v.CustomerNo = tc.Customer  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                            inner join " + TransDB + @"TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1  
                            left outer join  TEXCHANGE_RATE tr on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and  tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  " +
                                " where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"' 
                            and tvpo.Plant='" + plant + "' and TB.FGCode = '" + txtpartdesc.Text + @"' 
                            and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";


                //subash
                da1 = new SqlDataAdapter(strGetData1, MDMCon);
                da1.Fill(dtget1);
                if (dtget1.Rows.Count > 0)
                {
                    grdvendor1.DataSource = dtget1;
                    grdvendor1.DataBind();
                }
                else
                {
                    grdvendor1.DataSource = dtget1;
                    grdvendor1.DataBind();
                }
                //
                strGetData = @" select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                        TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],
                        TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,2)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,tc.UoM 
                        from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material   
                        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material    
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   
                        inner join " + TransDB + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant   " +
                            " where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"'  
                        and tvpo.Plant='" + plant + @"' and TB.FGCode = '" + txtpartdesc.Text + @"' 
                        and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";

                //subash
                da = new SqlDataAdapter(strGetData, MDMCon);
                da.Fill(dtget);

                foreach (DataRow row in dtget.Rows)
                {
                    //string a = row.Cell[3]
                }

                if (dtget.Rows.Count > 0)
                {
                    Label2.Visible = true;
                    Button1.Visible = true;

                    Session["ReqNoDT"] = reqno;
                    TableRow Hearderrow = new TableRow();

                    Table1.Rows.Add(Hearderrow);
                    foreach (DataColumn dt in dtget.Columns)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        tCell1.Text = dt.ColumnName.ToString();

                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;

                    }

                    int rowcount = 0;
                    foreach (DataRow row in dtget.Rows)
                    {
                        TableRow tRow = new TableRow();
                        Table1.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= dtget.Rows[0].ItemArray.Length - 1; cellCtr++)
                        {

                            TableCell tCell = new TableCell();

                            Label lb = new Label();
                            tCell.Controls.Add(lb);

                            if (rowcount > 0 && (cellCtr == 0 || cellCtr == 1 || cellCtr == 2))
                            {
                                Table1.Rows[1].Cells[cellCtr].Attributes.Add("rowspan", rowcount + 1.ToString());
                                //tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                tCell.Text = row.ItemArray[cellCtr].ToString();
                                tRow.Cells.Add(tCell);
                            }

                            //if (cellCtr == dtget.Rows[0].ItemArray.Length - 1)
                            //{
                            //    if (txtPIRDesc.Text.ToString().ToUpper().Contains("SUBCON"))
                            //    {
                            //        tCell.Text = "";
                            //    }
                            //}
                        }
                        rowcount++;

                    }
                    Session["hdnReqNoGp"] = dtget.Rows[0]["Req No"].ToString();
                    Session["dtTable1"] = Table1;
                    hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();
                }
                else
                {
                    Button1.Visible = false;
                    GetDataforNoBom(reqno.ToString());
                    //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('selected vendors does not have valid Data. Please check the Master Data !')", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally {
                MDMCon.Dispose();
            }
        }
        

        /// <summary>
        /// Download and View button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewDownPDF_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (!FileUpload1.HasFile)
            {
                if (Session["FileAttach"] != null)
                {
                    FileUpload1 = (FileUpload)Session["FileAttach"];
                }
            }

            if (FileUpload1.HasFile)
            {
                string FileExtension = System.IO.Path.GetExtension(FileUpload1.FileName);
                string filename = System.IO.Path.GetFileName(FileUpload1.FileName);
                string PathAndFileName = folderPath + DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
                if (FileUpload1.HasFile)
                {
                    FileUpload1.SaveAs(PathAndFileName);

                    if (filename == "" || string.IsNullOrEmpty(filename))
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No attachment to download !');", true);
                    }
                    else
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
                            FileInfo file = new FileInfo(PathAndFileName);
                            if (file.Exists) //check file exsit or not  
                            {
                                file.Delete();
                            }
                            Response.End();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('File Not Found !');openModal();", true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// submit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Button1_Click(object sender, EventArgs e)
        {
            if (ProcessSubmit() == true)
            {
                DeleteNonRequest();
                if (SendEmail() == true)
                {
                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submit Success");
                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
                else
                {
                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Send Mail Due On : " + ErrMsg + " ");
                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
            }
            else
            {
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Submit Data Due On : " + ErrMsg + " ");
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        bool ProcessSubmit() {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                MDMCon.Open();
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();
                GetDbTrans();

                if (Session["FileAttach"] != null)
                {
                    FileUpload Fu = (FileUpload)Session["FileAttach"];
                    fname = Fu.FileName.ToString();
                    if (fname != "")
                    {
                        DataTable dtget = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter();

                        string strGetData = string.Empty;
                        string plnt = txtplant.Text;
                        if (RbTeamShimano.Checked == true)
                        {
                            plnt = TxtPlantVendor.Text;
                        }
                        string ReqNum = Session["hdnReqNoGp"].ToString();
                        strGetData = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,
                                TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                                TQ.vendorCode1 as [Vendor Code],TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount as Amt_SCur,
                                isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as ExchRate,
                                v.Crcy AS Venor_Crcy, 
                                isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,
                                tc.Unit,tc.UoM 
                                from TMATERIAL tm inner join 
                                TBOMLIST TB on tm.Material = TB.Material 
                                inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material  
                                inner join tVendor_New v on v.CustomerNo = tc.Customer 
                                inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor 
                                inner join " + TransDB + @"TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 
                                left outer join  TEXCHANGE_RATE tr on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant 
                                where TQ.RequestNumber='" + ReqNum + "' and tm.Plant='" + plnt + "' and TB.FGCode = '" + txtpartdesc.Text + @"' 
                                and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) 
                                and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";

                        da = new SqlDataAdapter(strGetData, MDMCon);
                        da.Fill(dtget);

                        if (dtget.Rows.Count > 0)
                        {
                            Table dtTable1 = (Table)Session["dtTable1"];
                            if (dtTable1 != null)
                            {
                                if (dtTable1.Rows.Count > 0)
                                {
                                    for (int t = 1; t < dtTable1.Rows.Count; t++)
                                    {
                                        string Qno = "";
                                        if (t > 1)
                                        {
                                            Qno = dtTable1.Rows[t].Cells[4].Text.ToString();
                                        }
                                        else
                                        {
                                            Qno = dtTable1.Rows[t].Cells[(7)].Text.ToString();
                                        }

                                        sql = @"update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus='4',PICApprovalStatus = '4', 
                                            ManagerApprovalStatus = '4', DIRApprovalStatus = '4' where QuoteNo ='" + Qno + "'";
                                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        else
                        {
                            DataTable dtget1 = new DataTable();
                            SqlDataAdapter da1 = new SqlDataAdapter();

                            string strGetData1 = string.Empty;

                            ReqNum = Session["hdnReqNoGp"].ToString();
                            strGetData1 = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], TQ.Plant,
                                        TQ.Material as [Comp Material],TQ.MaterialDesc as [Comp Material Desc],
                                        TQ.VendorName as [Vendor Name],TQ.vendorCode1 as [Vendor Code], 
                                        TQ.QuoteNo,vp.PICName,vp.PICemail, TV.Crcy 
                                        from " + TransDB + @"TQuoteDetails TQ 
                                        inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor 
                                        inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor   
                                        Where TQ.RequestNumber='" + ReqNum + "' ";

                            da1 = new SqlDataAdapter(strGetData1, MDMCon);
                            da1.Fill(dtget1);

                            if (dtget1.Rows.Count > 0)
                            {
                                Table dtTable1 = (Table)Session["dtTable1"];
                                if (dtTable1 != null)
                                {
                                    if (dtTable1.Rows.Count > 0)
                                    {
                                        string CurrentVndCode = "";

                                        for (int t = 1; t < dtTable1.Rows.Count; t++)
                                        {
                                            string Qno = "";
                                            if (t > 1)
                                            {
                                                Qno = dtTable1.Rows[t].Cells[4].Text.ToString();
                                                CurrentVndCode = dtTable1.Rows[t].Cells[(3)].Text.ToString();
                                            }
                                            else
                                            {
                                                Qno = dtTable1.Rows[t].Cells[(7)].Text.ToString();
                                                CurrentVndCode = dtTable1.Rows[t].Cells[(6)].Text.ToString();
                                            }

                                            sql = @"update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus='4',PICApprovalStatus = '4', ManagerApprovalStatus = '4', 
                                                        DIRApprovalStatus = '4' where QuoteNo ='" + Qno + "'";
                                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                            cmd.ExecuteNonQuery();

                                            #region Save data Vendor tool Amortize
                                            if (DdlToolAmortize.SelectedValue == "YES")
                                            {
                                                if (Session["VndToolAmortize"] != null)
                                                {
                                                    DataTable dtVndToolAmor = (DataTable)Session["VndToolAmortize"];
                                                    if (dtVndToolAmor.Rows.Count > 0)
                                                    {
                                                        for (int i = 0; i < GvVndToolAmortize.Rows.Count; i++)
                                                        {
                                                            CheckBox CheckVnd = (CheckBox)GvVndToolAmortize.Rows[i].FindControl("chkVndToolAmor");
                                                            string vndcd = GvVndToolAmortize.Rows[i].Cells[1].Text;
                                                            if (CheckVnd.Checked == true && CurrentVndCode == vndcd)
                                                            {
                                                                sql = @" INSERT INTO TToolAmortization(Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                                                                                ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,AmortizeCost,AmortizeCurrency
                                                                                ,ExchangeRate,AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                                                                                ,EffectiveFrom,DueDate,CreatedBy,CreatedOn)
                                                                                VALUES
                                                                                (@Plant,@RequestNumber,@QuoteNo,null,@MaterialCode,@MaterialDescription,@VendorCode
                                                                                ,@VendorDescription,@Process_Grp_code,@ToolTypeID,@Amortize_Tool_ID,@Amortize_Tool_Desc,@AmortizeCost,@AmortizeCurrency
                                                                                ,@ExchangeRate,@AmortizeCost_Vend_Curr,@AmortizePeriod,@AmortizePeriodUOM,@TotalAmortizeQty,@QtyUOM,@AmortizeCost_Pc_Vend_Curr
                                                                                ,@EffectiveFrom,@DueDate,@CreatedBy,CURRENT_TIMESTAMP) ";

                                                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                                                cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                                                                cmd.Parameters.AddWithValue("@RequestNumber", ReqNum);
                                                                cmd.Parameters.AddWithValue("@QuoteNo", Qno);
                                                                cmd.Parameters.AddWithValue("@MaterialCode", txtpartdesc.Text);
                                                                cmd.Parameters.AddWithValue("@MaterialDescription", txtpartdescription.Text);
                                                                cmd.Parameters.AddWithValue("@VendorCode", dtVndToolAmor.Rows[i]["VendorCode"].ToString());
                                                                cmd.Parameters.AddWithValue("@VendorDescription", dtVndToolAmor.Rows[i]["VendorName"].ToString());
                                                                cmd.Parameters.AddWithValue("@Process_Grp_code", ddlprocess.SelectedValue.ToString());
                                                                cmd.Parameters.AddWithValue("@ToolTypeID", dtVndToolAmor.Rows[i]["ToolTypeID"].ToString());
                                                                cmd.Parameters.AddWithValue("@Amortize_Tool_ID", dtVndToolAmor.Rows[i]["Amortize_Tool_ID"].ToString());
                                                                cmd.Parameters.AddWithValue("@Amortize_Tool_Desc", dtVndToolAmor.Rows[i]["Amortize_Tool_Desc"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizeCost", dtVndToolAmor.Rows[i]["AmortizeCost"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizeCurrency", dtVndToolAmor.Rows[i]["AmortizeCurrency"].ToString());
                                                                cmd.Parameters.AddWithValue("@ExchangeRate", dtVndToolAmor.Rows[i]["ExchangeRate"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizeCost_Vend_Curr", dtVndToolAmor.Rows[i]["AmortizeCost_Vend_Curr"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizePeriod", dtVndToolAmor.Rows[i]["AmortizePeriod"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizePeriodUOM", dtVndToolAmor.Rows[i]["AmortizePeriodUOM"].ToString());
                                                                cmd.Parameters.AddWithValue("@TotalAmortizeQty", dtVndToolAmor.Rows[i]["TotalAmortizeQty"].ToString());
                                                                cmd.Parameters.AddWithValue("@QtyUOM", dtVndToolAmor.Rows[i]["QtyUOM"].ToString());
                                                                cmd.Parameters.AddWithValue("@AmortizeCost_Pc_Vend_Curr", dtVndToolAmor.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString());
                                                                cmd.Parameters.AddWithValue("@EffectiveFrom", dtVndToolAmor.Rows[i]["EeffDt"].ToString());
                                                                cmd.Parameters.AddWithValue("@DueDate", dtVndToolAmor.Rows[i]["DuDate"].ToString());
                                                                cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                                                                cmd.ExecuteNonQuery();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ErrMsg = "No vendors for this Process group with Material!";
                                return false;
                            }
                        }

                        #region insertdatareqplant
                        sql = "";
                        for (int i = 0; i < LbSPlantRequestor.Items.Count; i++)
                        {
                            if (LbSPlantRequestor.Items[i].Selected == true)
                            {
                                string pl = LbSPlantRequestor.Items[i].Value.ToString();
                                sql = @" insert into TPlantReq(RequestNumber,Plant,CreatedBy,CreatedOn) 
                                            values (@RequestNumber," + pl + ",@CreatedBy,CURRENT_TIMESTAMP) ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.Parameters.AddWithValue("@RequestNumber", Session["hdnReqNoGp"].ToString());
                                cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                                cmd.ExecuteNonQuery();
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        ErrMsg = "Please select the Attachment and upload !";
                        return false;
                    }
                }
                else
                {
                    ErrMsg = "Please select the Attachment and upload !";
                    return false;
                }

                EmetTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                ErrMsg = ex.ToString();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                MDMCon.Dispose();
                EmetCon.Dispose();
            }
        }

        bool SendEmail()
        {
            try
            {
                if (Session["FileAttach"] != null)
                {
                    FileUpload Fu = (FileUpload)Session["FileAttach"];
                    fname = Fu.FileName.ToString();
                    if (fname != "")
                    {
                        SqlTransaction transaction11 = null;

                        string dg_checkvalue, dg_formid = string.Empty;
                        string getQuoteRef = string.Empty;
                        string formatstatus = string.Empty;
                        string remarks = string.Empty;
                        string currentMonth = DateTime.Now.Month.ToString();
                        string currentYear = DateTime.Now.Year.ToString();
                        int increquest = 000000;
                        string RequestInc = String.Format(currentYear, increquest);
                        string REQUEST = string.Concat(currentYear, Session["RequestIncNumber1"].ToString());
                        string strdate = txtReqDate.Text;
                        int rowscount = 0;

                        bool Continue = true;
                        foreach (GridViewRow gr in grdvendor1.Rows)
                        {
                            dg_formid = gr.Cells[0].Text.ToString();
                            string dg_VenName = gr.Cells[1].Text.ToString().Replace("amp;", " ");
                            dg_checkvalue = gr.Cells[1].Text.ToString();
                            string strvendname = dg_checkvalue.ToString();
                            string QuoteSearchTerm = "";
                            QuoteSearchTerm = gr.Cells[2].Text.ToString();
                            QuoteSearchTerm = QuoteSearchTerm.Substring(0, 3);
                            string getquote = String.Concat(QuoteSearchTerm, Session["RequestIncNumber1"].ToString());
                            string strPlatingType = txtplatingtype.Text;
                            getQuoteRef = getquote + "GP";
                            remarks = "Open Status";
                            rowscount++;
                            string UnitNetWeight = txtunitweight.Text;
                            DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);

                            Table dtTable1 = (Table)Session["dtTable1"];
                            if (dtTable1 != null)
                            {
                                if (dtTable1.Rows.Count > 0)
                                {
                                    for (int t = 1; t < dtTable1.Rows.Count; t++)
                                    {
                                        string VC = "";
                                        if (t > 1)
                                        {
                                            VC = dtTable1.Rows[t].Cells[3].Text.ToString();
                                        }
                                        else
                                        {
                                            VC = dtTable1.Rows[t].Cells[(6)].Text.ToString();
                                        }

                                        if (VC == dg_formid)
                                        {
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
                                                    ErrMsg = cc2.ToString();
                                                    Continue = false;
                                                }
                                                cnn.Dispose();
                                            }
                                            #endregion

                                            Boolean IsAttachFile = true;
                                            int SequenceNumber = 1;
                                            string test = userId;

                                            #region Uploading  ttachment to Mail sever using UNC credentials
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
                                                                if (Fu.HasFile)
                                                                {
                                                                    filename = System.IO.Path.GetFileName(Fu.FileName);
                                                                    PathAndFileName = folderPath + filename;
                                                                    Fu.SaveAs(PathAndFileName);
                                                                }
                                                                Source = PathAndFileName;
                                                                File.Copy(Source, Destination, true);
                                                                SendFilename = fname.Remove(fname.Length - 4);
                                                                Session["SendFilename"] = fname.Remove(fname.Length - 4);
                                                                OriginalFilename = Session["OriginalFilename"].ToString();
                                                                OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                                                                Session["OriginalFilename"] = OriginalFilename.ToString();

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

                                                                ErrMsg = "Mail Attachment Failed: " + xw1;
                                                                Continue = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                catch (Exception x)
                                                {
                                                    ErrMsg = x.ToString();
                                                    Continue = false;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                IsAttachFile = false;
                                                SendFilename = "NOFILE";
                                                SendFilename = "NOFILE";
                                                OriginalFilename = "NOFILE";
                                                Session["OriginalFilename"] = "NOFILE";
                                                format = "NO";
                                            }
                                            #endregion

                                            #region getting vendor mail id
                                            aemail = string.Empty;
                                            pemail = string.Empty;
                                            using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                            {
                                                string returnValue = string.Empty;
                                                cnn.Open();
                                                SqlCommand cmdget = cnn.CreateCommand();
                                                cmdget.CommandType = CommandType.StoredProcedure;
                                                cmdget.CommandText = "dbo.Emet_Email_vendordetails";

                                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.Decimal);
                                                vendorid.Direction = ParameterDirection.Input;
                                                vendorid.Value = dg_formid;
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
                                                cnn.Dispose();
                                            }
                                            #endregion

                                            #region getting User mail id
                                            using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                            {
                                                string returnValue = string.Empty;
                                                cnn.Open();
                                                SqlCommand cmdget = cnn.CreateCommand();
                                                cmdget.CommandType = CommandType.StoredProcedure;
                                                cmdget.CommandText = "dbo.Emet_Email_userdetails";

                                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                                                vendorid.Direction = ParameterDirection.Input;
                                                //vendorid.Value = userId;
                                                vendorid.Value = Label15.Text;
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
                                                cnn.Dispose();
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
                                                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation been created by " + Label16.Text + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + "<br />  Partcode And Description :   " + txtpartdesc.Text + "  | " + txtpartdescription.Text + "<br />  Quotation Response Due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                                    body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + " <br /> Partcode And Description :   " + txtpartdesc.Text + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                                    string BodyFormat = "HTML";
                                                    string BodyRemark = "0";
                                                    string Signature = "";
                                                    string Importance = "High";
                                                    string Sensitivity = "Confidential";
                                                    string CreateUser = Label15.Text;
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
                                                        ErrMsg = "failed sending email Header: " + cc2;
                                                        Continue = false;
                                                        break;
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
                                                        ErrMsg = "failed sending email detail: " + cc1;
                                                        Continue = false;
                                                        break;
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
                                                            Detail.Parameters.AddWithValue("@Quotenumber", getQuoteRef.ToString());
                                                            Detail.Parameters.AddWithValue("@body", body1.ToString());
                                                            Detail.CommandText = Details;
                                                            Detail.ExecuteNonQuery();
                                                            transaction11.Commit();
                                                            Email_inser1.Dispose();
                                                        }
                                                        catch (Exception cc1)
                                                        {
                                                            transaction11.Rollback();
                                                            Email_inser1.Dispose();
                                                            ErrMsg = "Mail Content Issue in Transaction table: " + cc1;
                                                            Continue = false;
                                                            break;
                                                        }
                                                    }
                                                    //End Details
                                                }

                                            }
                                            catch (Exception cc1)
                                            {
                                                ErrMsg = "failed sending email : " + cc1;
                                                Continue = false;
                                                break;
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    Continue = false;
                                }
                            }
                            else
                            {
                                Continue = false;
                            }

                            if (Continue == false) {
                                break;
                            }
                        }

                        if (Continue == false) {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.ToString();
                return false;
            }
        }

        protected void CleanForm()
        {
            try
            {
                this.txtpartdesc.Text = "";
                this.txtpartdescription.Text = "";
                this.txtunitweight.Text = "";
                this.txtUOM.Text = "";
                this.DdlReason.SelectedIndex = 0;
                this.txtMQty.Text = "";
                this.txtBaseUOM1.Text = "";
                this.ddlprocess.SelectedIndex = 0;
                this.txtplatingtype.Text = "";
                this.txtDate.Text = "";
                lblMessage.Text = "";
                grdvendor.DataSource = null;
                grdvendor.DataBind();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        /// <summary>
        /// Load data and Create Dynamic table with No BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
        /// <summary>
        /// Load data and Create Dynamic table with No BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
        protected void GetDataforNoBom(string reqno)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;

                //subash

                DataTable dtget1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter();
                string plant = txtplant.Text;
                if (RbTeamShimano.Checked == true)
                {
                    plant = TxtPlantVendor.Text;
                }

                string tran = Convert.ToString(TransDB.ToString());

                strGetData = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], TQ.Plant, '-' as 'Comp Material' ,'-' as 'Comp Material Desc', 
                              TQ.VendorName as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, TV.Crcy,tvpo.coderef as SearchTerm 
                              from " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ 
                              inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor 
                              inner join tVendorPOrg tvpo ON TQ.VendorCode1 = tvpo.Vendor  and tv.porg=tvpo.porg 
                              inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor   
                              where TQ.RequestNumber = '" + reqno + "'  and tvpo.Plant = '" + plant + "' and vp.Plant= '" + plant + "'";

                da = new SqlDataAdapter(strGetData, MDMCon);
                da.Fill(dtget);

                grdvendor1.DataSource = dtget;
                grdvendor1.DataBind();

                if (dtget.Rows.Count > 0)
                {
                    Label2.Visible = true;
                    Button1.Visible = true;

                    Session["ReqNoDT"] = reqno;
                    TableRow Hearderrow = new TableRow();

                    Table1.Rows.Add(Hearderrow);
                    foreach (DataColumn dt in dtget.Columns)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        tCell1.Text = dt.ColumnName.ToString();

                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;

                    }

                    int rowcount = 0;
                    foreach (DataRow row in dtget.Rows)
                    {
                        TableRow tRow = new TableRow();
                        Table1.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= dtget.Rows[0].ItemArray.Length - 1; cellCtr++)
                        {

                            TableCell tCell = new TableCell();

                            Label lb = new Label();
                            tCell.Controls.Add(lb);

                            if (rowcount > 0 && (cellCtr == 0 || cellCtr == 1 || cellCtr == 2))
                            {
                                Table1.Rows[1].Cells[cellCtr].Attributes.Add("rowspan", rowcount + 1.ToString());
                                //tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                tCell.Text = row.ItemArray[cellCtr].ToString();
                                tRow.Cells.Add(tCell);
                            }

                            //if (cellCtr == dtget.Rows[0].ItemArray.Length - 1)
                            //{
                            //    if (txtPIRDesc.Text.ToString().ToUpper().Contains("SUBCON"))
                            //    {
                            //        tCell.Text = "";
                            //    }
                            //}


                        }
                        rowcount++;

                    }
                    Session["hdnReqNoGp"] = dtget.Rows[0]["Req No"].ToString();
                    Session["dtTable1"] = Table1;
                    hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();

                }
                else
                {
                    DeleteNonRequest();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('selected vendors does not have valid Data. Please check the Master Data !');CloseLoading();", true);
                    DvCreateRequest.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);

                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error Due On : " + LbMsgErr.Text + " ");
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }


        /// <summary>
        /// Delete unwanted requests which is not include in Process. 
        /// </summary>
        public void DeleteNonRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = "delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + Label15.Text + "') or (CreateStatus is null and createdby= '" + Label15.Text + "')";
                cmd = new SqlCommand(sql, EmetCon);
                cmd.ExecuteNonQuery();

                Button1.Visible = false;
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
        

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewReqWSAPgp.aspx");
        }

        
        protected void DdlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTeamSmnVendor(DdlVendor.SelectedValue.ToString());
        }

        protected void CekVendorType()
        {
            try
            {

                DvExternal.Visible = true;
                DvTeamShmnVendor.Visible = false;
                string ProcGroup = ddlprocess.SelectedValue.ToString();

                if (RbExternal.Checked == true)
                {
                    vendorload(ProcGroup);
                }
                else if (RbTeamShimano.Checked == true)
                {
                    GetDdlVendor(ProcGroup);
                    GetTeamSmnVendor(DdlVendor.SelectedValue.ToString());
                    DvExternal.Visible = false;
                    DvTeamShmnVendor.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select Vendor Type!')", true);
                    DvExternal.Visible = false;
                    DvTeamShmnVendor.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void RbExternal_CheckedChanged(object sender, EventArgs e)
        {
            //Session["VndToolAmortize"] = null;
            //if (DdlToolAmortize.SelectedValue == "YES")
            //{
            //    DvVndToolAmortize.Visible = true;
            //    vendorloadToolAmor();
            //}
            //else if (DdlToolAmortize.SelectedValue == "NO")
            //{
            //    DvVndToolAmortize.Visible = false;
            //    Button1.Visible = false;
            //    CekVendorType();
            //    Button1.Visible = false;
            //    DvCreateRequest.Visible = true;
            //}

            Session["VndToolAmortize"] = null;
            if (DdlToolAmortize.SelectedValue == "YES")
            {
                DvVndToolAmortize.Visible = true;
                vendorloadToolAmor();
            }
            else if (DdlToolAmortize.SelectedValue == "NO")
            {
                DvVndToolAmortize.Visible = false;
                CekVendorType();
            }
            DvCreateRequest.Visible = true;
            Button1.Visible = false;

        }

        protected void RbTeamShimano_CheckedChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            if (DdlToolAmortize.SelectedValue == "YES")
            {
                DvVndToolAmortize.Visible = true;
                vendorloadToolAmor();
            }
            else if (DdlToolAmortize.SelectedValue == "NO")
            {
                DvVndToolAmortize.Visible = false;
                CekVendorType();
            }
            DvCreateRequest.Visible = true;
            Button1.Visible = false;
        }


        protected void btnsubmit_Click(object sender, EventArgs e)
        {

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
            try
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
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
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