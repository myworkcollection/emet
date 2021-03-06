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
using System.Threading;
using System.Globalization;
using System.Web.UI.HtmlControls;

using System.Data.SqlTypes;

namespace Material_Evaluation
{
    public partial class NewRequestMultiMat : System.Web.UI.Page
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
        public static string MasterDB;
        public static string TransDB;
        public string createuser;
        string sql;
        SqlCommand cmd;
        SqlDateTime sqldatenull;
        string errMsg;
        string previousCellValue = "";
        int previousCellCount = 1;
        bool isDuplicateReqWithOldNExpiredReqExtend = false;

        protected override void InitializeCulture()
        {
            String Languge = Request.Form["DdlListlanguage"];
            if ((Languge != null) && (Languge != ""))
            {
                if (Languge == "auto")
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(HttpContext.Current.Request.UserLanguages[0].Trim());
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(HttpContext.Current.Request.UserLanguages[0].Trim());
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Languge);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Languge);
                }
            }
        }

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
                        Session["FlAttchDrawing"] = null;

                        string UI = Session["userID"].ToString();
                        string FN = "EMET_Newrequest";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            article.Checked = true;

                            HttpContext.Current.Session["SAPCode"] = "1";
                            RbExternal.Checked = true;
                            userId = Session["userID"].ToString();
                            createuser = Session["userID"].ToString();
                            Label15.Text = Session["userID"].ToString();
                            userId1 = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            Label16.Text = Session["UserName"].ToString();
                            nameC = sname;
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;

                            lbluser1.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            objPirType = new Dictionary<int, string>();
                            this.article.Checked = true;

                            txtplant.Text = Session["EPlant"].ToString();
                            lblMessage.Text = "";

                            DeleteNonRequest();
                            LoadSavings();
                            getdate();
                            matltype();
                            getproctype();
                            GetPirType();
                            Getproduct();
                            getMatlClass();
                            GetSapPart();
                            process();
                            GetDdlReason();
                            GetImRecycleRatio();
                            if (!string.IsNullOrEmpty(Request.QueryString["num"]))
                            {
                                string num = Request.QueryString["num"].ToString();
                                if (num == "1")
                                {
                                    article.Checked = true;
                                    HttpContext.Current.Session["SAPCode"] = "1";
                                    DvDdlSAP.Style.Add("display", "block");
                                    DvTxtSAPCode.Style.Add("display", "none");
                                }
                                else
                                {
                                    RbWithouSAPCode.Checked = true;
                                    HttpContext.Current.Session["SAPCode"] = "0";
                                    LbPartCode.Text = "Part Code";
                                    DvDdlSAP.Style.Add("display", "none");
                                    DvTxtSAPCode.Style.Add("display", "block");
                                }
                            }
                            else
                            {
                                article.Checked = true;
                                HttpContext.Current.Session["SAPCode"] = "1";
                                DvDdlSAP.Style.Add("display", "block");
                                DvTxtSAPCode.Style.Add("display", "none");
                            }

                            SetDueOnDate();


                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }

                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }
                        }
                    }
                    else
                    {
                        if (Session["sidebarToggle"] == null)
                        {
                            SideBarMenu.Attributes.Add("style", "display:block;");
                        }
                        else
                        {
                            SideBarMenu.Attributes.Add("style", "display:none;");
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MyScript", "DatePitcker();CloseLoading();initdropdownMatList();initdropdownProduct();ChangeEmptyFieldColor();", true);
                        if (DvExternal.Visible == true || DvTeamShmnVendor.Visible == true || DvVndToolAmortize.Visible == true)
                        {
                            DvCreateRequest.Visible = true;
                        }
                        else
                        {
                            DvCreateRequest.Visible = false;
                        }
                        getdate();

                        string LayoutId = EMETModule.GetLayoutBaseOnprocGroup(ddlprocess.SelectedValue).ToUpper();
                        if (LayoutId == "LAYOUT1")
                        {
                            DvImRcylRatio.Visible = true;
                        }
                        else
                        {
                            DvImRcylRatio.Visible = false;
                        }
                        HdnLayoutId.Value = LayoutId;
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;

            }

        }

        /// <summary>
        /// SET request date Field
        /// </summary>
        protected void getdate()
        {

            //txtReqDate.Text = DateTime.Now.Date.ToShortDateString().ToString();
            txtReqDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtReqDate.Attributes.Add("disabled", "disabled");


        }


        /// <summary>
        /// Get Material Type
        /// </summary>
        protected void matltype()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = " select MT.Plant,MT.MaterialType, * from [dbo].[TMATERIALTYPE] as MT inner join TPLANT as P on MT.plant=p.Plant  where P.Plant= '" + txtplant.Text + "' and MT.DelFlag = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                ddlmatltype.DataSource = Result;
                ddlmatltype.DataTextField = "MaterialType";
                ddlmatltype.DataValueField = "MaterialType";
                ddlmatltype.DataBind();
                ddlmatltype.Items.Insert(0, new ListItem("-- Material Type --", String.Empty));
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
        /// Get Procurement Type
        /// </summary>
        protected void getproctype()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtmatl = new DataTable();
                SqlDataAdapter damatl = new SqlDataAdapter();
                // string strmatl = "select distinct Materialclassdescription from materials where product='" + matlclass + "'";

                string strmatl = " select distinct proctype from TPROCPIRTYPE WHERE (DelFlag = 0)";


                damatl = new SqlDataAdapter(strmatl, MDMCon);
                dtmatl = new DataTable();
                damatl.Fill(dtmatl);

                ddlproctype.DataSource = dtmatl;
                ddlproctype.DataTextField = "proctype";
                ddlproctype.DataValueField = "proctype";
                ddlproctype.DataBind();
                ddlproctype.Items.Insert(0, new ListItem("-- select Proctype --", String.Empty));
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
        /// Get PIR Type 
        /// </summary>
        private void GetPirType()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                string pirtype = "select * from TPIRTYPE where delflag = 0 order by PIRType Asc";

                SqlDataAdapter dapirtype = new SqlDataAdapter();
                dapirtype = new SqlDataAdapter(pirtype, MDMCon);
                DataTable dtpir = new DataTable();
                dapirtype.Fill(dtpir);

                //  txtPIRDesc.Text = dt.Rows[0]["DESCRIPTION"].ToString();

                if (dtpir.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtpir.Rows)
                    {
                        int key = int.Parse(dr["PIRType"].ToString());
                        string val = dr[1].ToString();
                        objPirType.Add(key, val);
                    }
                    //  objPirType.Add(dtpir.Rows[0].ItemArray[0], dtpir["Description"]);
                }
                else
                {


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

        public void Getproduct()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtResult = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                //string str = " select distinct (PR.product) , PR.Description as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant = '" + txtplant.Text + "' and PR.DelFlag = 0 ";
                string str = "  select PR.product, CONCAT(PR.product,+ ' - '+ PR.Description) as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant='" + Session["EPlant"].ToString() + "' and PR.DelFlag = 0";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(DtResult);

                DdlProduct.DataSource = DtResult;
                DdlProduct.Items.Clear();
                DdlProduct.DataTextField = "productdescription";
                DdlProduct.DataValueField = "product";
                DdlProduct.DataBind();
                DdlProduct.Items.Insert(0, new ListItem() { Text = "--Select Product --", Value = "" });
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

        protected void GetSapPart()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            MDMCon.Open();
            try
            {
                string strplant = Session["EPlant"].ToString();
                string strprod = DdlProduct.SelectedValue.ToString();
                string matrl_class_desc = DdlMatClass.SelectedValue.ToString();
                string strplantstatus = ddlplantstatus.SelectedValue.ToString();
                string matlType = ddlmatltype.SelectedValue.ToString();
                string proctype = ddlproctype.SelectedValue.ToString();
                string splproctype = ddlsplproctype.SelectedValue.ToString();

                string str = @" Select  distinct  Material, concat(Material,' - ', MaterialDesc) as MaterialDesc from TMATERIAL TM 
                                Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where tm.Plant = '" + strplant + "' ";
                if (splproctype != "" && splproctype.ToString().ToUpper() != "BLANK")
                {
                    str += @" and tm.SPlProcType= '" + splproctype + "'  ";
                }

                if (proctype != "")
                {
                    str += @" and tm.PROCTYPE = '" + proctype + "'  ";
                }

                if (strprod != "")
                {
                    str += @" and tm.Product = '" + strprod + "' ";
                }

                if (strprod != "")
                {
                    str += @" and tm.Product = '" + strprod + "' ";
                }

                if (matlType != "")
                {
                    str += @" and tm.MaterialType= '" + matlType + "' ";
                }

                if (strplantstatus != "")
                {
                    str += @" and tm.PlantStatus= '" + strplantstatus + "' ";
                }

                if (matrl_class_desc != "")
                {
                    str += @" and TR.ProdComDesc= '" + matrl_class_desc.ToString() + "' ";
                }

                str += @" and (TM.DelFlag = 0) ";


                DataTable DtResult = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(str, MDMCon))
                {
                    sda.Fill(DtResult);
                    DdlMatCodeList.DataSource = DtResult;
                    DdlMatCodeList.Items.Clear();
                    DdlMatCodeList.DataTextField = "MaterialDesc";
                    DdlMatCodeList.DataValueField = "Material";
                    DdlMatCodeList.DataBind();
                    DdlMatCodeList.Items.Insert(0, new ListItem() { Text = "--Please SAP Part Code--", Value = "" });

                    DdlMatCodeList2.DataSource = DtResult;
                    DdlMatCodeList2.Items.Clear();
                    DdlMatCodeList2.DataTextField = "MaterialDesc";
                    DdlMatCodeList2.DataValueField = "Material";
                    DdlMatCodeList2.DataBind();

                    if (txtpartdesc.Text != "")
                    {
                        if (DdlMatCodeList.Items.FindByValue(txtpartdesc.Text) != null)
                        {
                            DdlMatCodeList.SelectedValue = txtpartdesc.Text;
                        }
                        else
                        {
                            DdlMatCodeList.SelectedIndex = -1;
                            txtpartdesc.Text = "";
                            txtpartdescription.Text = "";
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
        /// Get Process
        /// </summary>
        protected void process()
        {
            string strprod = txtplant.Text;
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                //string str = " select Process_Grp_code,Process_Grp_Description from TPROCESGROUP_LIST";
                //string str = " select ProcessGrp as 'Process_Grp_code' from TPROCESGRP_SCREENLAYOUT where DelFlag = 0 order by ProcessGrp";
                string str = @" select A.ProcessGrp as 'Process_Grp_code',CONCAT(A.ProcessGrp,' - ',
                                (select Process_Grp_Description from TPROCESGROUP_LIST where Process_Grp_code=A.ProcessGrp)) as PrcGrpAndDesc from TPROCESGRP_SCREENLAYOUT A where A.DelFlag = 0 order by ProcessGrp";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                ddlprocess.DataSource = Result;
                ddlprocess.DataTextField = "PrcGrpAndDesc";
                ddlprocess.DataValueField = "Process_Grp_code";
                ddlprocess.DataBind();
                ddlprocess.Items.Insert(0, new ListItem("-- Select Process --", String.Empty));

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
                            DdlReason.Items.Insert(0, new ListItem("--Request Purpose--", "00"));
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
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["DefValue"].ToString() != "")
                            {
                                DefVal = dt.Rows[0]["DefValue"].ToString();
                            }
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

                    if (RbWithouSAPCode.Checked == true)
                    {
                        TxtDuenextRev.Enabled = true;
                    }
                }
                else
                {
                    if (RbWithouSAPCode.Checked == true)
                    {
                        TxtDuenextRev.Enabled = true;
                    }

                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Setting Quote Next Rev Date Not Set, Please contact Administrator");
                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }


        protected void GetProdUser()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "";

                //str = "select * from TSMNProductPIC where product in(select distinct product from TPRODUCT  tm where CONCAT(tm.product,+ '- '+ tm.Description)  like '%" + (txtprodID.Text.ToString()) + "%' and (TM.DelFlag = 0)) and userid='" + Session["userID"].ToString() + "' and (DelFlag = 0) ";
                str = @"select * from TSMNProductPIC where product = '" + DdlProduct.SelectedValue.ToString() + @"' and userid='" + Session["userID"].ToString() + "' and (DelFlag = 0) ";

                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count <= 0)
                {
                    DdlProduct.SelectedIndex = -1;
                    DdlMatCodeList.SelectedIndex = -1;
                    txtpartdesc.Text = "";
                    txtpartdescription.Text = "";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('You are not authorized for this product. please Contact MDMAdmin')", true);
                }
                else
                {
                    GetPlantStatus(DdlProduct.SelectedValue.ToString());
                    GetSplProcType(ddlproctype.SelectedValue.ToString());
                    getMatlClass();
                    GetSapPart();
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

        protected void getMatlClass()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            MDMCon.Open();
            try
            {
                string strplant = Session["EPlant"].ToString();
                string strPlantStatus = ddlplantstatus.SelectedValue.ToString();
                string strProduct = DdlProduct.SelectedValue.ToString();
                string strMatType = ddlmatltype.SelectedValue.ToString();
                string strProcType = ddlproctype.SelectedValue.ToString();
                string strSplProctype = ddlsplproctype.SelectedValue.ToString();

                string str = "";

                if (article.Checked == true)
                {
                    if (strSplProctype.ToUpper() != "BLANK" && strSplProctype.ToUpper() != "")
                    {
                        str = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE = '" + strSplProctype + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMatType + "' and tm.PlantStatus='" + strPlantStatus + "' and (TM.DelFlag = 0) ";
                    }
                    else
                    {
                        if (strPlantStatus != "")
                        {
                            str = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE is null  and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMatType + "' and tm.PlantStatus='" + strPlantStatus + "' and (TM.DelFlag = 0) ";
                        }
                        else
                        {
                            str = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE is null  and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMatType + "' and (tm.PlantStatus='" + strPlantStatus + "' or tm.PlantStatus is null)  and (TM.DelFlag = 0) ";
                        }
                    }
                }
                else
                {
                    str = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where TM.product='" + strProduct + "' and (TM.DelFlag = 0) ";
                }

                DataTable DtResult = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter(str, MDMCon))
                {
                    sda.Fill(DtResult);
                    DdlMatClass.DataSource = DtResult;
                    DdlMatClass.Items.Clear();
                    DdlMatClass.DataTextField = "ProdComDesc";
                    DdlMatClass.DataValueField = "ProdComDesc";
                    DdlMatClass.DataBind();
                    DdlMatClass.Items.Insert(0, new ListItem() { Text = "--Select Desc--", Value = "" });
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
        /// Get All the Details based on SAP Part Code
        /// </summary>
        /// <param name="materialid"></param>
        private void GetProdMaterial(string materialid)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "";

                str = @"Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating, TM.MaterialType,TM.PlantStatus, TM.PROCTYPE,TM.SplPROCTYPE, 
                        PR.product as Product,TR.ProdComDesc,
                        TM.BaseUOM 
                        from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode 
                        INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                        where Tm.Material = '" + materialid + "' and tm.Plant = '" + txtplant.Text.ToString() + "' and (TM.DelFlag = 0) ";

                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
                    txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                    txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                    txtBaseUOM1.Text = dtdate.Rows[0]["BaseUOM"].ToString();
                    txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();

                    if (ddlmatltype.Items.FindByValue(dtdate.Rows[0]["MaterialType"].ToString()) != null)
                    {
                        ddlmatltype.SelectedValue = dtdate.Rows[0]["MaterialType"].ToString();
                    }
                    else
                    {
                        ddlmatltype.SelectedIndex = -1;
                    }

                    if (ddlproctype.Items.FindByValue(dtdate.Rows[0]["PROCTYPE"].ToString()) != null)
                    {
                        ddlproctype.SelectedValue = dtdate.Rows[0]["PROCTYPE"].ToString();
                    }
                    else
                    {
                        ddlproctype.SelectedIndex = -1;
                    }

                    if (DdlProduct.Items.FindByValue(dtdate.Rows[0]["Product"].ToString()) != null)
                    {
                        DdlProduct.SelectedValue = dtdate.Rows[0]["Product"].ToString();
                    }
                    else
                    {
                        DdlProduct.SelectedIndex = -1;
                    }

                    GetProdUser();
                    if (DdlMatClass.Items.FindByValue(dtdate.Rows[0]["ProdComDesc"].ToString()) != null)
                    {
                        DdlMatClass.SelectedValue = dtdate.Rows[0]["ProdComDesc"].ToString();
                    }
                    else
                    {
                        DdlMatClass.SelectedIndex = -1;
                    }


                    string prod = ddlmatltype.SelectedItem.Text;
                    GetPlantStatus(prod);
                    GetSplProcType(dtdate.Rows[0]["PROCTYPE"].ToString());

                    string strddlpSval = dtdate.Rows[0]["PlantStatus"].ToString();
                    if (ddlplantstatus.Items.FindByValue(strddlpSval) != null)
                    {
                        ddlplantstatus.SelectedValue = strddlpSval;
                    }
                    else
                    {
                        ddlplantstatus.SelectedIndex = -1;
                    }


                    //if (strddlpSval == "Z2")
                    //{
                    //    ddlplantstatus.SelectedValue = strddlpSval;
                    //}
                    //else {
                    //    ddlplantstatus.SelectedValue = null;
                    //}

                    GetSplProcType(dtdate.Rows[0]["Product"].ToString());

                    string splProc = dtdate.Rows[0]["SplPROCTYPE"].ToString();
                    if (splProc == "")
                    {
                        if (ddlsplproctype.Items.FindByValue("Blank") != null)
                        {
                            ddlsplproctype.SelectedValue = "Blank";
                        }
                        else
                        {
                            ddlsplproctype.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        if (ddlsplproctype.Items.FindByValue(splProc) != null)
                        {
                            ddlsplproctype.SelectedValue = splProc;

                            if (ddlsplproctype.SelectedValue.ToString() == "30")
                            {
                                ChcDisMatCost.Visible = true;
                                ChcDisMatCost.Checked = true;
                            }
                            else
                            {
                                ChcDisMatCost.Visible = false;
                            }
                        }
                        else
                        {
                            ddlsplproctype.SelectedIndex = -1;
                        }
                    }



                    string proc = ddlproctype.SelectedValue.ToString();
                    string SpProc = ddlsplproctype.SelectedValue.ToString();
                    GetPIRTypesbysplproc(proc, SpProc);
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

                        dg_formid = row.Cells[0].Text.ToString();

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
        /// Load vendor based on Process Group
        /// </summary>
        /// <param name="processgrp"></param>
        protected void vendorload(string processgrp)
        {
            string strprod = "2100";
            if (HttpContext.Current.Session["EPlant"] != null)
            {
                strprod = (string)HttpContext.Current.Session["EPlant"].ToString();
            }
            else
            {
                strprod = "2100";
            }

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
                                and tp.plant = '" + txtplant.Text + @"' 
                                and tv.plant = '" + txtplant.Text + @"' 
                                and vp.plant = '" + txtplant.Text + @"'
                                and ( TVP.VendorCode not in (select VendorCode from TSBMPRICINGPOLICY)) 
                                and (tv.DelFlag = 0) 
                                and (TVP.DelFlag = 0) ";

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
                            ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost),0) as AmortizeCost
                            , tt.AmortizeCurrency,tt.ExchangeRate
                            ,tt.AmortizePeriod,tt.AmortizePeriodUOM,tt.TotalAmortizeQty,tt.QtyUOM
                            ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                            ,IIF( right(tt.AmortizeCost_Pc_Vend_Curr,6) = '000000', convert(nvarchar(50),convert(int,tt.AmortizeCost_Pc_Vend_Curr)) ,convert(nvarchar(50),tt.AmortizeCost_Pc_Vend_Curr) ) as AmortizeCost_Pc_Vend_Curr
                            ,tt.EffectiveFrom, tt.DueDate
                            , FORMAT(tt.EffectiveFrom, 'yyyy-MM-dd') as EeffDt
                            , FORMAT(tt.DueDate, 'yyyy-MM-dd') as DuDate
                            from TVENDOR_PROCESSGROUP TVP 
                            inner join tvendorporg tv ON TVP.VendorCode = tv.Vendor 
                            inner join TVENDORPIC as VP on VP.VendorCode = tv.Vendor 
                            inner join TPOrgPlant as tp on tp.porg = tv.POrg 
                            inner join TToolAmortization as TT on tv.Plant = tt.Plant and tv.Vendor = tt.VendorCode and TVP.ProcessGrp = tt.Process_Grp_code
                            inner join TToolAmortizationvsMember as TM  on TM.Plant = TT.Plant and tm.Amortize_Tool_ID = tt.Amortize_Tool_ID and tm.Material = @Material
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
                    cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text);
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

        /// <summary>
        /// Bind Vendor details to Gridview
        /// </summary>
        protected void bindvendor()
        {
            grdvendor.DataSource = (DataTable)ViewState["vendorlist"];
            grdvendor.Columns[0].Visible = true;
            grdvendor.Columns[1].Visible = true;
            grdvendor.Columns[2].Visible = true;
            grdvendor.Columns[3].Visible = false;
            grdvendor.DataBind();
        }

        /// <summary>
        /// Radio button change event - 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radiodraftcost_Changed(object sender, EventArgs e)
        {
            if (draftcost.Checked == true)
            {
                this.article.Checked = false;
                Session["firstarticle"] = draftcost.Checked.ToString();
                Response.Redirect("WithSApCode.aspx");
            }

        }

        /// <summary>
        /// Change vendor Radio button changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RbWithouSAPCode_CheckedChanged(object sender, EventArgs e)
        {
            if (RbWithouSAPCode.Checked == true)
            {
                //HttpContext.Current.Session["SAPCode"] = "0";
                //LbPartCode.Text = "Part Code";
                //ddlplantstatus.Enabled = false;
                //ddlplantstatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#EBEBE4");
                //CleanForm();
                Response.Redirect("NewRequest.aspx?num=2");
            }
        }

        protected void RbWithouSAPGp_CheckedChanged(object sender, EventArgs e)
        {
            Response.Redirect("NewReqWSAPgp.aspx");
        }

        /// <summary>
        /// First article item Radio button changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void article_CheckedChanged(object sender, EventArgs e)
        {
            if (article.Checked == true)
            {
                Response.Redirect("NewRequest.aspx?num=1");
            }
        }

        /// <summary>
        /// Draft & Cost Radio button changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void draftcost_CheckedChanged(object sender, EventArgs e)
        {

            if (draftcost.Checked == true)
            {
                this.article.Checked = false;
                this.draftcost.Checked = true;
                Response.Redirect("WithSApCode.aspx");
            }
        }


        /// <summary>
        /// Material type dropdownlist index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlmatltype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prod = ddlmatltype.SelectedItem.Text;
            GetPlantStatus(prod);
            getMatlClass();
            GetSapPart();
        }

        /// <summary>
        /// Get Plant Status based on Material
        /// </summary>
        /// <param name="prod"></param>
        private void GetPlantStatus(string prod)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                string plantstatus = "select REPLACE(plantstatus,'Blank','') as plantstatus from tMaterialTypevsPlantStatus where MaterialType='" + prod + "' and (DelFlag = 0)";
                SqlDataAdapter daplantstatus = new SqlDataAdapter();
                daplantstatus = new SqlDataAdapter(plantstatus, MDMCon);
                DataTable dtplantstaus = new DataTable();
                daplantstatus.Fill(dtplantstaus);

                if (dtplantstaus.Rows.Count > 0)
                {

                    ddlplantstatus.DataSource = dtplantstaus;
                    ddlplantstatus.DataTextField = "plantstatus";
                    ddlplantstatus.DataTextField = "plantstatus";
                    ddlplantstatus.Items.Insert(0, new ListItem("-- Select Plant Status --", String.Empty));
                    ddlplantstatus.DataBind();

                }
                else
                {
                    // ddlplantstatus.Text = "";

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
        /// Procurement Type Dropdownlist index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlproctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string prod = ddlproctype.SelectedItem.Text;
            GetSplProcType(prod);
            getMatlClass();
            GetSapPart();
        }


        /// <summary>
        /// Get Special Procurement Type based on Procurement Type
        /// </summary>
        /// <param name="prod"></param>
        private void GetSplProcType(string ProcType)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                //By subash//string plantstatus = "select Distinct SpProcType from TPROCPIRTYPE where procType='" + prod + "' ";
                string plantstatus = "select ProcType, (CASE WHEN (isnull(SPProcType,'0') ='0' ) THEN 'Blank' else cast(SPProcType as nvarchar)   END) as Spproctype from ProcurementType  where ProcType = '" + ProcType + "' and (DelFlag = 0) ";
                SqlDataAdapter daplantstatus = new SqlDataAdapter();
                daplantstatus = new SqlDataAdapter(plantstatus, MDMCon);
                DataTable dt = new DataTable();
                daplantstatus.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlsplproctype.DataSource = dt;

                    ddlsplproctype.DataTextField = "Spproctype";
                    ddlsplproctype.DataValueField = "Spproctype";
                    ddlsplproctype.DataBind();

                    ddlsplproctype.Items.Insert(0, new ListItem("-- Select Spproctype --", String.Empty));
                }
                else
                {
                    // ddlplantstatus.Text = "";
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
                            where A.VendorCode=@VendorCode and A.Delflag = 0";

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
        /// Special Procurement Type DDL index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlsplproctype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string proc = ddlproctype.SelectedItem.Text;
            string SpProc = ddlsplproctype.SelectedItem.Text;
            getMatlClass();
            GetPIRTypesbysplproc(proc, SpProc);
            if (ddlsplproctype.SelectedValue.ToString() == "30")
            {
                ChcDisMatCost.Visible = true;
                ChcDisMatCost.Checked = true;
            }
            else
            {
                ChcDisMatCost.Visible = false;
            }
            GetSapPart();
        }

        //subash



        /// <summary>
        /// Get PIR Type based on Procurement and Spl Procurement Types
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="SpProc"></param>
        private void GetPIRTypesbysplproc(string proc, string SpProc)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                string pirtype;
                if (SpProc == "Blank")
                {
                    SpProc = "";
                    pirtype = "select PIRTYPE,DESCRIPTION from TPROCPIRTYPE where procType='" + proc + "' and Spproctype='" + SpProc + "' and (DelFlag = 0) ";
                }
                else if (SpProc == "-- Select Spproctype --")
                {
                    pirtype = "";
                }
                else
                {
                    pirtype = "select PIRTYPE,DESCRIPTION from TPROCPIRTYPE where procType='" + proc + "' and Spproctype='" + SpProc + "' and (DelFlag = 0) ";
                }

                if (pirtype != "")
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlCommand cmd = new SqlCommand(pirtype, MDMCon);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlpirtype.DataSource = dt;
                                ddlpirtype.DataTextField = "PIRTYPE";
                                ddlpirtype.DataTextField = "PIRTYPE";

                                ddlpirtype.DataBind();

                                txtPIRDesc.Text = dt.Rows[0]["DESCRIPTION"].ToString();
                            }
                            else
                            {
                            }
                        }
                    }
                }

                if (pirtype != "")
                {
                    Session["Pirtype"] = ddlpirtype.SelectedItem.Text;
                }
                else
                {
                    Session["Pirtype"] = "";
                }

                Session["PirDescription"] = txtPIRDesc.Text;
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
        /// Process DDL index change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void ddlprocess_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            this.PIRTYPE();
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

        protected void TxtEffectiveDate_TextChanged(object sender, EventArgs e)
        {
            Session["VndToolAmortize"] = null;
            this.PIRTYPE();
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
                this.PIRTYPE();
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
            resetProgress();
            //this.vendorload(strprod);
        }

        /// <summary>
        /// PIR Type DDL index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlpirtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = " select description,ProcType,SPProcType from TPROCPIRTYPE where ProcType='' and SPProcType='' and PIRType='' and delflag = 0";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                ddlprocess.DataSource = Result;
                ddlprocess.DataTextField = "Process_Grp_code";
                ddlprocess.DataValueField = "Process_Grp_code";
                ddlprocess.Items.Insert(0, new ListItem("-- Select Process --", String.Empty));
                ddlprocess.DataBind();
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
        /// Get PIR Type
        /// </summary>
        protected void PIRTYPE()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select (JobCode)+ '- '+ JobCodeDetailDescription as PIRTYTypeDescription  from TPIRJOBTYPE_PROCESSGROUP where ProcessGrpcode='" + ddlprocess.SelectedValue.ToString() + "' and delflag = 0";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);



                if (Result.Rows.Count > 0)
                {
                    txtjobtypedesc.Text = Result.Rows[0]["PIRTYTypeDescription"].ToString();

                    ddlpirjtype.DataSource = Result;
                    ddlpirjtype.DataTextField = "PIRTYTypeDescription";
                    ddlpirjtype.DataValueField = "PIRTYTypeDescription";
                    ddlpirjtype.DataBind();

                    if (ddlpirjtype.Items.Count > 1)
                    {
                        ddlpirjtype.Items.Insert(0, new ListItem("-- Select Description --", String.Empty));
                    }
                    //ddlpirjtype.Items.Insert(0, new ListItem("-- select Description --", String.Empty));
                }
                else
                {
                    ddlpirjtype.Items.Clear();
                }
                Session["PIRJOBTYPE"] = txtjobtypedesc.Text;
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
        /// Plant Status DDL index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlplantstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            getMatlClass();
            GetSapPart();
        }


        /// <summary>
        /// Get Material Details based on Material Code
        /// </summary>
        /// <param name="getprodcode"></param>
        private void GetProdDesc(string getprodcode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where material like '%" + getprodcode + "%' and delflag = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
                txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
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
        /// PIR Type ddl index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlpirtype_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {

                if (ddlpirtype.SelectedIndex > -1)
                {
                    int ddlselectedItem = int.Parse(ddlpirtype.SelectedItem.Text.ToString());

                    var Keyvalue = objPirType.Single(x => x.Key == ddlselectedItem);
                    txtPIRDesc.Text = Keyvalue.Value;

                    Session["Pirtype"] = ddlpirtype.SelectedItem.Text;
                    Session["PirDescription"] = txtPIRDesc.Text;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        protected void DdlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtpartdesc.Text = "";
                txtpartdescription.Text = "";
                //ddlpirtype.SelectedValue = "";
                txtPIRDesc.Text = "";
                txtunitweight.Text = "";
                txtUOM.Text = "";
                txtBaseUOM1.Text = "";
                DdlMatClass.SelectedIndex = -1;

                GetProdUser();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }


        bool CekVendorVsMaterialExpiredReqVndNotRespon(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' 
                            and ApprovalStatus = 0  and format(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') 
                            and FinalQuotePrice is null";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    if (Session["InvalidRequestExpiredReqVndNotRespon"] == null)
                                    {
                                        Session["InvalidRequestExpiredReqVndNotRespon"] = dt2;
                                    }
                                    else
                                    {
                                        DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReqVndNotRespon"];
                                        if (dt2.Rows.Count > 0)
                                        {
                                            bool isExist = false;
                                            for (int L = 0; L < DtTemp.Rows.Count; L++)
                                            {
                                                if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                {
                                                    isExist = true;
                                                    break;
                                                }
                                            }

                                            if (isExist == false)
                                            {
                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                {
                                                    DataRow dr = DtTemp.NewRow();
                                                    dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                    dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                    dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                    dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                    dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                    dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                    dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                    dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                    dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                    DtTemp.Rows.Add(dr);
                                                    DtTemp.AcceptChanges();
                                                }
                                            }
                                        }
                                        Session["InvalidRequestExpiredReqVndNotRespon"] = DtTemp;
                                    }
                                }
                            }
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
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

        bool CekVendorVsMaterialExpiredReqVndRespon(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' 
                            and ApprovalStatus <> 0 and DIRApprovalStatus is null and format(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') 
                            and FinalQuotePrice is not null";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    if (Session["InvalidRequestExpiredReqVndRespon"] == null)
                                    {
                                        Session["InvalidRequestExpiredReqVndRespon"] = dt2;
                                    }
                                    else
                                    {
                                        DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReqVndRespon"];
                                        if (dt2.Rows.Count > 0)
                                        {
                                            bool isExist = false;
                                            for (int L = 0; L < DtTemp.Rows.Count; L++)
                                            {
                                                if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                {
                                                    isExist = true;
                                                    break;
                                                }
                                            }

                                            if (isExist == false)
                                            {
                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                {
                                                    DataRow dr = DtTemp.NewRow();
                                                    dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                    dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                    dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                    dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                    dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                    dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                    dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                    dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                    dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                    DtTemp.Rows.Add(dr);
                                                    DtTemp.AcceptChanges();
                                                }
                                            }
                                        }
                                        Session["InvalidRequestExpiredReqVndRespon"] = DtTemp;
                                    }
                                }
                            }

                            //if (Session["InvalidRequestExpiredReqVndRespon"] == null)
                            //{
                            //    Session["InvalidRequestExpiredReqVndRespon"] = dt;
                            //}
                            //else
                            //{
                            //    DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReqVndRespon"];
                            //    for (int i = 0; i < dt.Rows.Count; i++)
                            //    {
                            //        DataRow dr = DtTemp.NewRow();
                            //        dr["Plant"] = dt.Rows[i]["Plant"].ToString();
                            //        dr["RequestNumber"] = dt.Rows[i]["RequestNumber"].ToString();
                            //        dr["RequestDate"] = dt.Rows[i]["RequestDate"].ToString();
                            //        dr["QuoteResponseDueDate"] = dt.Rows[i]["QuoteResponseDueDate"].ToString();
                            //        dr["QuoteNo"] = dt.Rows[i]["QuoteNo"].ToString();
                            //        dr["Material"] = dt.Rows[i]["Material"].ToString();
                            //        dr["MaterialDesc"] = dt.Rows[i]["MaterialDesc"].ToString();
                            //        dr["VendorCode1"] = dt.Rows[i]["VendorCode1"].ToString();
                            //        dr["VendorName"] = dt.Rows[i]["VendorName"].ToString();
                            //        DtTemp.Rows.Add(dr);
                            //        DtTemp.AcceptChanges();
                            //    }
                            //    Session["InvalidRequestExpiredReqVndRespon"] = DtTemp;
                            //}

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
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

        bool CekVendorVsMaterialPendingReqVndRespon(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' 
                            and ApprovalStatus <> 0 and DIRApprovalStatus is null and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') 
                            and FinalQuotePrice is not null";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    if (Session["InvalidRequestVndRespon"] == null)
                                    {
                                        Session["InvalidRequestVndRespon"] = dt2;
                                    }
                                    else
                                    {
                                        DataTable DtTemp = (DataTable)Session["InvalidRequestVndRespon"];
                                        if (dt2.Rows.Count > 0)
                                        {
                                            bool isExist = false;
                                            for (int L = 0; L < DtTemp.Rows.Count; L++)
                                            {
                                                if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                {
                                                    isExist = true;
                                                    break;
                                                }
                                            }

                                            if (isExist == false)
                                            {
                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                {
                                                    DataRow dr = DtTemp.NewRow();
                                                    dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                    dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                    dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                    dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                    dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                    dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                    dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                    dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                    dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                    DtTemp.Rows.Add(dr);
                                                    DtTemp.AcceptChanges();
                                                }
                                            }
                                        }
                                        Session["InvalidRequestVndRespon"] = DtTemp;
                                    }
                                }
                            }

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
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

        bool CekVendorVsMaterialPendingReqVndNotRespon(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' and ApprovalStatus = 0
                            and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd')
                            and FinalQuotePrice is null";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName,FinalQuotePrice
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    bool IsHaveAnotherVendorSubmit = false;
                                    string VndCodeSbmit = "";
                                    for (int d = 0; d < dt2.Rows.Count; d++)
                                    {
                                        if (dt2.Rows[d]["FinalQuotePrice"].ToString() != "")
                                        {
                                            IsHaveAnotherVendorSubmit = true;
                                            VndCodeSbmit = dt2.Rows[d]["VendorCode1"].ToString();
                                            break;
                                        }
                                    }

                                    if (IsHaveAnotherVendorSubmit == true)
                                    {
                                        CekVendorVsMaterialPendingReqVndRespon(VndCodeSbmit, material);
                                    }
                                    else
                                    {
                                        if (Session["InvalidRequestVndNotRespon"] == null)
                                        {
                                            Session["InvalidRequestVndNotRespon"] = dt2;
                                        }
                                        else
                                        {
                                            DataTable DtTemp = (DataTable)Session["InvalidRequestVndNotRespon"];
                                            if (dt2.Rows.Count > 0)
                                            {
                                                bool isExist = false;
                                                for (int L = 0; L < DtTemp.Rows.Count; L++)
                                                {
                                                    if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                    {
                                                        isExist = true;
                                                        break;
                                                    }
                                                }

                                                if (isExist == false)
                                                {
                                                    for (int i = 0; i < dt2.Rows.Count; i++)
                                                    {
                                                        DataRow dr = DtTemp.NewRow();
                                                        dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                        dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                        dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                        dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                        dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                        dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                        dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                        dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                        dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                        DtTemp.Rows.Add(dr);
                                                        DtTemp.AcceptChanges();
                                                    }
                                                }
                                            }
                                            Session["InvalidRequestVndNotRespon"] = DtTemp;
                                        }
                                    }
                                }
                            }
                            return false;
                        }
                        else
                        {
                            return true;
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



        bool CekVendorVsMaterialExpiredReq(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
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
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    if (Session["InvalidRequestExpiredReq"] == null)
                                    {
                                        Session["InvalidRequestExpiredReq"] = dt2;
                                    }
                                    else
                                    {
                                        DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReq"];
                                        if (dt2.Rows.Count > 0)
                                        {
                                            bool isExist = false;
                                            for (int L = 0; L < DtTemp.Rows.Count; L++)
                                            {
                                                if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                {
                                                    isExist = true;
                                                    break;
                                                }
                                            }

                                            if (isExist == false)
                                            {
                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                {
                                                    DataRow dr = DtTemp.NewRow();
                                                    dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                    dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                    dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                    dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                    dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                    dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                    dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                    dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                    dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                    DtTemp.Rows.Add(dr);
                                                    DtTemp.AcceptChanges();
                                                }
                                            }
                                        }
                                        Session["InvalidRequestExpiredReq"] = DtTemp;
                                    }
                                }
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

        bool CekVendorVsMaterialPendingReq(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + "' and ApprovalStatus in ('0','2') ";

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
                            if (Session["InvalidRequest"] == null)
                            {
                                Session["InvalidRequest"] = dt;
                            }
                            else
                            {
                                DataTable DtTemp = (DataTable)Session["InvalidRequest"];
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
                                Session["InvalidRequest"] = DtTemp;
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

        bool CekVendorVsMaterialApprvReq(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"  With CTE
                            As
                            (
                            select  Row_Number() Over(Partition by TQ.VendorCode1,TQ.Material Order By TQ.VendorCode1 asc,TQ.UpdatedOn desc) As Row_Num
                            , TQ.Plant,TQ.RequestNumber,
                            format(TQ.RequestDate,'dd-MM-yyyy') as RequestDate,
                            format(TQ.QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            tq.QuoteNo,TQ.Material,TQ.MaterialDesc,TQ.VendorCode1,TQ.VendorName
                            from TQuoteDetails TQ 
                            where (TQ.ApprovalStatus='3') 
                            and (isMassRevision = 0 or isMassRevision is null)
                            and (TQ.ManagerApprovalStatus='2') and (TQ.DIRApprovalStatus='0') 
                            and TQ.Plant= @Plant  
                            and TQ.VendorCode1 = @VendorCode1
                            and tq.Material = @Material)
                            Select*
                            From CTE
                            Where ROW_NUM = 1  ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", txtplant.Text);
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
                            if (Session["InvalidRequestHasApprv"] == null)
                            {
                                Session["InvalidRequestHasApprv"] = dt;
                            }
                            else
                            {
                                DataTable DtTemp = (DataTable)Session["InvalidRequestHasApprv"];
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
                                Session["InvalidRequestHasApprv"] = DtTemp;
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



        protected void GvInvalidRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GvInvalidRequestHasApprv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
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

        protected void GvUpdateOldReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
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

        protected void resetProgress()
        {
            DvOldReqAutoRej.Visible = false;
            DvInvalidRequest.Visible = false;
            DvUpdateOldReq.Visible = false;

            DvValidRequest.Visible = false;
            DvConfirmForSubmit.Visible = false;
            DvInvalidRequestHasApprv.Visible = false;
            Button1.Visible = false;

            Session["InvalidRequestExpiredReq"] = null;
            Session["InvalidRequest"] = null;
            Session["InvalidRequestHasApprv"] = null;

            Session["InvalidRequestExpiredReqVndRespon"] = null;
            Session["InvalidRequestVndRespon"] = null;
            Session["InvalidRequestExpiredReqVndNotRespon"] = null;
            Session["InvalidRequestVndNotRespon"] = null;

            Session["OldReqAutoRej"] = null;
            Session["UpdateOldReqWithAddNewVnd"] = null;
        }

        protected void CreateNewReqTemp()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction TransEmet = null;
            try
            {
                string txtplatestring = txtplatingtype.Text.ToString();

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

                SqlConnection EmetCon2 = new SqlConnection(EMETModule.GenEMETConnString());
                EmetCon2.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("select MAX(RequestNumber) from TQuoteDetails", EmetCon2);
                //    string str = " select MAX(REQUESTNO) from Tstatus ";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string curentYear = DateTime.Now.Year.ToString();
                    curentYear = curentYear.Substring(curentYear.Length - 2);
                    currentYear = currentYear.Substring(currentYear.Length - 2);
                    string ReqNum = dr[0].ToString();

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

                    // char C = RequestIncNumber[RequestIncNumber.Length - 1];

                    int newReq = (int.Parse(RequestIncNumber)) + (1);
                    RequestIncNumber = newReq.ToString();
                    RequestIncNumber1 = RequestIncNumber;
                    Session["RequestIncNumber1"] = RequestIncNumber;
                    // int tempval = Convert.ToInt32(C.ToString()) + 1;


                }
                dr.Close();
                EmetCon2.Dispose();

                EmetCon.Open();
                TransEmet = EmetCon.BeginTransaction();
                Session["RequestNo"] = RequestIncNumber.ToString();
                bool havedata = false;
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

                    string VndId = "";
                    foreach (GridViewRow gr in GrdVndor.Rows)
                    {
                        CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                        string Vnd = gr.Cells[1].Text.ToString();
                        string MatCod = txtpartdesc.Text.ToString();

                        if (myCheckbox.Checked == true)
                        {
                            #region proces save new request
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
                            string strPlatingType = txtplatingtype.Text.ToString();


                            getQuoteRef = getquote;

                            remarks = "Open Status";
                            rowscount++;

                            string Proccode = "";
                            string PIRJobType = "";
                            string PITType = "";
                            string PITTypeDesc = "";
                            Proccode = DdlProduct.SelectedValue.ToString();
                            if (Session["PIRJOBTYPE"] != null)
                            {
                                PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                            }
                            if (Session["Pirtype"] != null)
                            {
                                PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                            }
                            if (Session["PirDescription"] != null)
                            {
                                PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();
                            }

                            string UnitNetWeight = txtunitweight.Text.ToString();
                            DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                            DateTime Dtstrdate = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);
                            //DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                            //QuoteDate = QuoteDate.ToLongDateString();

                            //RequestIncNumber

                            Session["flag"] = "Fail";
                            string PITTypeandDesc = PITType + "- " + PITTypeDesc;
                            if (Proccode != "")
                            {
                                if (article.Checked == true)
                                {
                                    sql = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,
                                                                    MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DueOn,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                                                    MaterialType,PlantStatus,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,isUseSAPCode,SMNPicDept,FADate,FAQty,DelDate,DelQty,IMRecycleRatio,IsUseToolAmortize) 
                                                                    values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,
                                                                    @PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@EffectiveDate,@DueOn,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,
                                                                    @MaterialType,@PlantStatus,@UserId,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,1,@SMNPicDept,@FADate,@FAQty,@DelDate,@DelQty,@IMRecycleRatio,@IsUseToolAmortize)";
                                }
                                else if (RbWithouSAPCode.Checked == true)
                                {
                                    sql = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,
                                                                    MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DueOn,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                                                    MaterialType,PlantStatus,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,ApprovalStatus,PICApprovalStatus,ManagerApprovalStatus,DIRApprovalStatus,
                                                                    isUseSAPCode,SMNPicDept,FADate,FAQty,DelDate,DelQty,IMRecycleRatio,IsUseToolAmortize) 
                                                                    values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,
                                                                    @PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@EffectiveDate,@DueOn,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,
                                                                    @MaterialType,@PlantStatus,@UserId,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,4,4,4,4,0,@SMNPicDept,@FADate,@FAQty,@DelDate,@DelQty,@IMRecycleRatio,@IsUseToolAmortize)";
                                }
                                cmd = new SqlCommand(sql, EmetCon, TransEmet);
                                cmd.Parameters.AddWithValue("@REQUESTNO", Convert.ToInt32(RequestIncNumber.ToString()));
                                strdate = Dtstrdate.ToString("yyyy-MM-dd");
                                cmd.Parameters.AddWithValue("@REQUESTDATE", strdate.ToString());
                                cmd.Parameters.AddWithValue("@PLANT", txtplant.Text.ToString());
                                if (article.Checked == true)
                                {
                                    DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                                    cmd.Parameters.AddWithValue("@QUOTENO", getQuoteRef.ToString());
                                    cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));

                                    DateTime DtDueOn = DateTime.ParseExact(TxtDuenextRev.Text, "dd-MM-yyyy", null);
                                    cmd.Parameters.AddWithValue("@DueOn", DtDueOn.ToString("yyyy-MM-dd"));
                                }
                                else
                                {
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

                                    cmd.Parameters.AddWithValue("@QUOTENO", (getQuoteRef.ToString() + "D"));
                                }
                                cmd.Parameters.AddWithValue("@VENDORCODE", dg_formid.ToString());
                                cmd.Parameters.AddWithValue("@VENDORNAME", dg_VenName.ToString());
                                cmd.Parameters.AddWithValue("@procesgrp", ddlprocess.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@Product", Proccode.ToString());
                                cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text.ToString());
                                cmd.Parameters.AddWithValue("@matlClass", DdlMatClass.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@PIRJobtype", ddlpirjtype.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@MaterialDesc", txtpartdescription.Text.ToString());
                                cmd.Parameters.AddWithValue("@PIRTypeDesc", PITTypeandDesc.ToString());
                                cmd.Parameters.AddWithValue("@UnitNet", UnitNetWeight.ToString());
                                cmd.Parameters.AddWithValue("@QuoteDue", QuoteDate.ToString("yyyy/MM/dd HH:mm:ss"));
                                cmd.Parameters.AddWithValue("@DrawingNo", lblMessage.Text);
                                cmd.Parameters.AddWithValue("@PlatingType", strPlatingType);
                                cmd.Parameters.AddWithValue("@SAPProcType", ddlproctype.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@SAPSPProcType", ddlsplproctype.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@MaterialType", ddlmatltype.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@PlantStatus", ddlplantstatus.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@UserId", Label15.Text.ToString());
                                cmd.Parameters.AddWithValue("@BaseUOM", txtBaseUOM1.Text.ToString());
                                cmd.Parameters.AddWithValue("@MQty", txtMQty.Text.ToString());
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
                                cmd.Parameters.AddWithValue("@UOM", txtUOM.Text.ToString());
                                cmd.Parameters.AddWithValue("@ActualNU", txtunitweight.Text.ToString());
                                cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());

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

                                if (HdnLayoutId.Value.ToString().ToUpper() == "LAYOUT1")
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
                                cmd = new SqlCommand(sql, EmetCon, TransEmet);
                                cmd.ExecuteNonQuery();
                                #endregion

                                Session["flag"] = "Pass";
                                havedata = true;

                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Product should not Empty')", true);
                            }
                            #endregion Process save new request
                        }
                        else
                        {
                            dg_checkvalue = "";
                        }
                        VndId = Vnd;
                    }

                    #endregion
                }
                else
                {
                    #region save Team Shimano
                    string QuoteSearchTerm = "";
                    QuoteSearchTerm = TxtSrcTerm.Text.ToString();
                    QuoteSearchTerm = QuoteSearchTerm.Substring(0, 3);
                    string getquote = String.Concat(QuoteSearchTerm, RequestIncNumber);
                    //string getquote = String.Concat(mstrvenname, RequestIncNumber);
                    string strPlatingType = txtplatingtype.Text.ToString();


                    getQuoteRef = getquote;

                    remarks = "Open Status";

                    string Proccode = "";
                    string PIRJobType = "";
                    string PITType = "";
                    string PITTypeDesc = "";
                    Proccode = DdlProduct.SelectedValue.ToString();
                    if (Session["PIRJOBTYPE"] != null)
                    {
                        PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                    }
                    if (Session["Pirtype"] != null)
                    {
                        PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                    }
                    if (Session["PirDescription"] != null)
                    {
                        PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();
                    }

                    string UnitNetWeight = txtunitweight.Text.ToString();
                    DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                    DateTime Dtstrdate = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);
                    Session["flag"] = "Fail";
                    string PITTypeandDesc = PITType + "- " + PITTypeDesc;
                    if (Proccode != "")
                    {
                        if (article.Checked == true)
                        {
                            sql = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,
                                                                        MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DueOn,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                                                        MaterialType,PlantStatus,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,isUseSAPCode,SMNPicDept,FADate,FAQty,DelDate,DelQty,IMRecycleRatio) 
                                                                        values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,
                                                                        @PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@EffectiveDate,@DueOn,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,
                                                                        @MaterialType,@PlantStatus,@UserId,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,1,@SMNPicDept,@FADate,@FAQty,@DelDate,@DelQty,@IMRecycleRatio)";
                        }
                        else if (RbWithouSAPCode.Checked == true)
                        {
                            sql = @" INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,
                                                                        MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DueOn,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                                                        MaterialType,PlantStatus,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,ApprovalStatus,PICApprovalStatus,ManagerApprovalStatus,
                                                                        DIRApprovalStatus,isUseSAPCode,SMNPicDept,FADate,FAQty,DelDate,DelQty,IMRecycleRatio) 
                                                                        values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,
                                                                        @PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@EffectiveDate,@DueOn,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,
                                                                        @MaterialType,@PlantStatus,@UserId,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,4,4,4,4,0,@SMNPicDept,@FADate,@FAQty,@DelDate,@DelQty,@IMRecycleRatio)";
                        }

                        cmd = new SqlCommand(sql, EmetCon, TransEmet);
                        cmd.Parameters.AddWithValue("@REQUESTNO", Convert.ToInt32(RequestIncNumber.ToString()));
                        //DateTime Dtstrdate = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);

                        strdate = Dtstrdate.ToString("yyyy-MM-dd");
                        cmd.Parameters.AddWithValue("@REQUESTDATE", strdate.ToString());
                        cmd.Parameters.AddWithValue("@PLANT", TxtPlantVendor.Text);
                        if (article.Checked == true)
                        {
                            DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                            cmd.Parameters.AddWithValue("@QUOTENO", getQuoteRef.ToString());
                            cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));

                            DateTime DtDueOn = DateTime.ParseExact(TxtDuenextRev.Text, "dd-MM-yyyy", null);
                            cmd.Parameters.AddWithValue("@DueOn", DtDueOn.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
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

                            cmd.Parameters.AddWithValue("@QUOTENO", (getQuoteRef.ToString() + "D"));
                        }
                        cmd.Parameters.AddWithValue("@VENDORCODE", DdlVendor.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@VENDORNAME", TxtVendorDesc.Text);
                        cmd.Parameters.AddWithValue("@procesgrp", ddlprocess.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Product", Proccode.ToString());
                        cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text.ToString());
                        cmd.Parameters.AddWithValue("@matlClass", DdlMatClass.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@PIRJobtype", ddlpirjtype.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaterialDesc", txtpartdescription.Text.ToString());
                        cmd.Parameters.AddWithValue("@PIRTypeDesc", PITTypeandDesc.ToString());
                        cmd.Parameters.AddWithValue("@UnitNet", UnitNetWeight.ToString());
                        cmd.Parameters.AddWithValue("@QuoteDue", QuoteDate.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@DrawingNo", lblMessage.Text);
                        cmd.Parameters.AddWithValue("@PlatingType", strPlatingType);
                        cmd.Parameters.AddWithValue("@SAPProcType", ddlproctype.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@SAPSPProcType", ddlsplproctype.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@MaterialType", ddlmatltype.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@PlantStatus", ddlplantstatus.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@UserId", Label15.Text.ToString());
                        cmd.Parameters.AddWithValue("@BaseUOM", txtBaseUOM1.Text.ToString());
                        cmd.Parameters.AddWithValue("@MQty", txtMQty.Text.ToString());
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
                        cmd.Parameters.AddWithValue("@UOM", txtUOM.Text.ToString());
                        cmd.Parameters.AddWithValue("@ActualNU", txtunitweight.Text.ToString());
                        cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());

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

                        if (ddlprocess.SelectedValue == "IM")
                        {
                            cmd.Parameters.AddWithValue("@IMRecycleRatio", DdlImRcylRatio.SelectedValue.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@IMRecycleRatio", DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();

                        Session["flag"] = "Pass";
                        havedata = true;

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Product should not Empty')", true);
                    }
                    #endregion
                }

                TransEmet.Commit();
                TransEmet.Dispose();

                if (havedata == true)
                {
                    if (article.Checked == true)
                    {
                        GetData(RequestIncNumber.ToString());
                    }
                    else
                    {
                        GetDataforNoBom(RequestIncNumber.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TransEmet.Rollback();
                TransEmet.Dispose();

                Session["flag"] = "Fail";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Submit Faill, due on : '" + ex.Message.ToString() + ")", true);

                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }


        protected void createRequest()
        {
            try
            {
                resetProgress();
                if ((RbExternal.Checked == true && DdlToolAmortize.SelectedValue == "YES") || (RbExternal.Checked == true && DdlToolAmortize.SelectedValue == "NO") || (RbTeamShimano.Checked == true && DdlToolAmortize.SelectedValue == "YES"))
                {
                    #region save method External vendor
                    int rowscount = 0;
                    int ValidCount = 0;
                    int VndChk = 0;

                    int cntVndChecd = 0;
                    if (DdlToolAmortize.SelectedValue == "YES")
                    {
                        string VndId = "";
                        foreach (GridViewRow gr in GvVndToolAmortize.Rows)
                        {
                            CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                            string Vnd = gr.Cells[1].Text.ToString();
                            if (myCheckbox.Checked == true && VndId != Vnd)
                            {
                                cntVndChecd++;
                            }

                        }
                    }
                    else if (DdlToolAmortize.SelectedValue == "NO")
                    {
                        foreach (GridViewRow gr in grdvendor.Rows)
                        {
                            CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                            if (myCheckbox.Checked == true)
                            {
                                cntVndChecd++;
                            }

                        }
                    }

                    if (DdlToolAmortize.SelectedValue == "YES")
                    {
                        string VndId = "";
                        foreach (GridViewRow gr in GvVndToolAmortize.Rows)
                        {
                            CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                            string Vnd = gr.Cells[1].Text.ToString();
                            string MatCod = txtpartdesc.Text.ToString();
                            if (myCheckbox.Checked == true)
                            {
                                if (CekVendorVsMaterialExpiredReqVndRespon(Vnd, MatCod) == true)
                                {
                                    if (CekVendorVsMaterialExpiredReqVndNotRespon(Vnd, MatCod) == true)
                                    {
                                        if (CekVendorVsMaterialPendingReqVndRespon(Vnd, MatCod) == true)
                                        {
                                            if (CekVendorVsMaterialPendingReqVndNotRespon(Vnd, MatCod) == true)
                                            {
                                                if (CekVendorVsMaterialPendingReq(Vnd, MatCod) == true)
                                                {
                                                    if (cntVndChecd == 1)
                                                    {
                                                        if (CekVendorVsMaterialApprvReq(Vnd, MatCod) == true)
                                                        {
                                                            ValidCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ValidCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                VndChk++;
                            }
                            VndId = Vnd;
                        }
                    }
                    else if (DdlToolAmortize.SelectedValue == "NO")
                    {
                        foreach (GridViewRow gr in grdvendor.Rows)
                        {
                            CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                            string Vnd = gr.Cells[1].Text.ToString();
                            string MatCod = txtpartdesc.Text.ToString();
                            if (myCheckbox.Checked == true)
                            {
                                if (CekVendorVsMaterialExpiredReqVndRespon(Vnd, MatCod) == true)
                                {
                                    if (CekVendorVsMaterialExpiredReqVndNotRespon(Vnd, MatCod) == true)
                                    {
                                        if (CekVendorVsMaterialPendingReqVndRespon(Vnd, MatCod) == true)
                                        {
                                            if (CekVendorVsMaterialPendingReqVndNotRespon(Vnd, MatCod) == true)
                                            {
                                                if (CekVendorVsMaterialPendingReq(Vnd, MatCod) == true)
                                                {
                                                    if (cntVndChecd == 1)
                                                    {
                                                        if (CekVendorVsMaterialApprvReq(Vnd, MatCod) == true)
                                                        {
                                                            ValidCount++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ValidCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                VndChk++;
                            }
                        }
                    }


                    if (ValidCount == VndChk)
                    {
                        if (Session["InvalidRequestExpiredReqVndRespon"] == null && Session["InvalidRequestVndRespon"] == null && Session["InvalidRequestExpiredReqVndNotRespon"] == null && Session["InvalidRequest"] == null && Session["InvalidRequestHasApprv"] == null)
                        {
                            CreateNewReqTemp();
                            DvConfirmForSubmit.Visible = true;
                            DvValidRequest.Visible = true;
                        }
                    }

                    #endregion
                }
                else
                {
                    #region save Team Shimano
                    string Vnd = DdlVendor.SelectedValue.ToString();
                    string MatCod = txtpartdesc.Text.ToString();

                    if (CekVendorVsMaterialExpiredReqVndRespon(Vnd, MatCod) == true)
                    {
                        if (CekVendorVsMaterialExpiredReqVndNotRespon(Vnd, MatCod) == true)
                        {
                            if (CekVendorVsMaterialPendingReqVndNotRespon(Vnd, MatCod) == true)
                            {
                                if (CekVendorVsMaterialPendingReqVndRespon(Vnd, MatCod) == true)
                                {
                                    if (CekVendorVsMaterialPendingReq(Vnd, MatCod) == true)
                                    {
                                        if (CekVendorVsMaterialApprvReq(Vnd, MatCod) == true)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Session["InvalidRequestExpiredReqVndRespon"] == null && Session["InvalidRequestVndRespon"] == null && Session["InvalidRequestExpiredReqVndNotRespon"] == null && Session["InvalidRequestVndNotRespon"] == null && Session["InvalidRequest"] == null && Session["InvalidRequestHasApprv"] == null)
                    {
                        CreateNewReqTemp();
                        DvConfirmForSubmit.Visible = true;
                        DvValidRequest.Visible = true;
                    }
                    #endregion
                }

                bool AutoRejectOldReq = false;
                bool UpdateOldReq = false;
                bool UnderProcess = false;
                bool RejOrExtendOldReq = false;

                if (Session["InvalidRequestExpiredReqVndRespon"] != null)
                {
                    #region process InvalidRequestExpiredReqVndRespon
                    DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReqVndRespon"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        UnderProcess = true;
                    }

                    if (UnderProcess == true)
                    {
                        GvInvalidRequest.DataSource = DtTemp;
                        GvInvalidRequest.DataBind();
                        DvInvalidRequest.Visible = true;
                    }
                    #endregion
                }
                else if (Session["InvalidRequestExpiredReqVndNotRespon"] != null)
                {
                    #region InvalidRequestExpiredReqVndNotRespon
                    DataTable DtTemp = (DataTable)Session["InvalidRequestExpiredReqVndNotRespon"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        bool IsVndCountingOldAndNewDiffrence = false;
                        bool IsVndNewReqAndOldEqual = false;
                        bool IsAllVendSelectedIndOldReq = false;
                        int VndSelected = 0;
                        DtTemp.DefaultView.Sort = "RequestNumber asc";

                        foreach (GridViewRow gr in grdvendor.Rows)
                        {
                            CheckBox VndChk = (CheckBox)(gr.Cells[0].Controls[1]);
                            if (VndChk.Checked == true)
                            {
                                VndSelected++;
                            }
                        }

                        if (VndSelected == DtTemp.Rows.Count)
                        {
                            IsVndCountingOldAndNewDiffrence = false;
                        }
                        else
                        {
                            IsVndCountingOldAndNewDiffrence = true;
                        }

                        if (IsVndCountingOldAndNewDiffrence == false)
                        {
                            #region cek is vndor selected equal with vendor in old request
                            for (int N = 0; N < grdvendor.Rows.Count; N++)
                            {
                                CheckBox VndChk = (CheckBox)grdvendor.Rows[N].FindControl("chk");
                                if (VndChk.Checked == true)
                                {
                                    bool IsVndNewInOldReq = false;
                                    for (int i = 0; i < DtTemp.Rows.Count; i++)
                                    {
                                        IsVndNewInOldReq = false;
                                        if (DtTemp.Rows[i]["VendorCode1"].ToString() == grdvendor.Rows[N].Cells[1].Text)
                                        {
                                            IsVndNewInOldReq = true;
                                            break;
                                        }
                                    }

                                    if (IsVndNewInOldReq == true)
                                    {
                                        IsVndNewReqAndOldEqual = true;
                                    }
                                    else
                                    {
                                        IsVndNewReqAndOldEqual = false;
                                        break;
                                    }
                                }
                            }
                            #endregion end cek is vndor selected equal with vendor in old request

                            if (IsVndNewReqAndOldEqual == false)
                            {
                                //auto reject old req
                                AutoRejectOldReq = true;
                            }
                            else
                            {
                                //Cannot Create-Prompt with Msg  -Req No: 999999 - Reject or Extend the Response Date
                                RejOrExtendOldReq = true;
                            }

                        }
                        else
                        {
                            //auto reject old request
                            AutoRejectOldReq = true;
                        }

                        if (AutoRejectOldReq == true)
                        {
                            GvOldReqAutoRej.DataSource = DtTemp;
                            GvOldReqAutoRej.DataBind();
                            Session["OldReqAutoRej"] = DtTemp;
                            CreateNewReqTemp();
                            DvOldReqAutoRej.Visible = true;
                            DvConfirmForSubmit.Visible = true;
                            DvValidRequest.Visible = true;
                            LbAutorejectText.Text = "Below Request Will Auto Rej due on : New Request Creation , Response Date is Expired And Vendor Not Response";
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
                        }
                        else if (RejOrExtendOldReq == true)
                        {
                            GvDuplicateWithExpiredReq.DataSource = DtTemp;
                            GvDuplicateWithExpiredReq.DataBind();
                            SpanRowGvInvalidExpired();
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();ChangeEmptyFieldColor();CloseLoading();OpenModalDuplicateExpired();", true);
                        }
                        else if (UnderProcess == true)
                        {
                            GvInvalidRequest.DataSource = DtTemp;
                            GvInvalidRequest.DataBind();
                            DvInvalidRequest.Visible = true;
                        }

                    }
                    #endregion
                }
                else if (Session["InvalidRequestVndRespon"] != null)
                {
                    #region InvalidRequestVndRespon
                    DataTable DtTemp = (DataTable)Session["InvalidRequestVndRespon"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        bool IsVndCountingOldAndNewDiffrence = false;
                        bool IsVndNewReqAndOldEqual = false;
                        bool IsAllVendSelectedIndOldReq = false;
                        int VndSelected = 0;
                        DtTemp.DefaultView.Sort = "RequestNumber asc";

                        //foreach (GridViewRow gr in grdvendor.Rows)
                        //{
                        //    CheckBox VndChk = (CheckBox)(gr.Cells[0].Controls[1]);
                        //    if (VndChk.Checked == true)
                        //    {
                        //        VndSelected++;
                        //    }
                        //}

                        //if (VndSelected == DtTemp.Rows.Count)
                        //{
                        //    IsVndCountingOldAndNewDiffrence = false;
                        //}
                        //else
                        //{
                        //    IsVndCountingOldAndNewDiffrence = true;
                        //}

                        //if (IsVndCountingOldAndNewDiffrence == false)
                        //{


                        //}
                        //else
                        //{
                        //    //auto reject old request
                        //    AutoRejectOldReq = true;
                        //}

                        #region cek is vndor selected equal with vendor in old request
                        for (int N = 0; N < grdvendor.Rows.Count; N++)
                        {
                            CheckBox VndChk = (CheckBox)grdvendor.Rows[N].FindControl("chk");
                            if (VndChk.Checked == true)
                            {
                                bool IsVndNewInOldReq = false;
                                for (int i = 0; i < DtTemp.Rows.Count; i++)
                                {
                                    IsVndNewInOldReq = false;
                                    if (DtTemp.Rows[i]["VendorCode1"].ToString() == grdvendor.Rows[N].Cells[1].Text)
                                    {
                                        IsVndNewInOldReq = true;
                                        break;
                                    }
                                }

                                if (IsVndNewInOldReq == true)
                                {
                                    IsVndNewReqAndOldEqual = true;
                                }
                                else
                                {
                                    IsVndNewReqAndOldEqual = false;
                                    break;
                                }
                            }
                        }
                        #endregion end cek is vndor selected equal with vendor in old request

                        if (IsVndNewReqAndOldEqual == false)
                        {
                            //update old req , add new vendor not in list of old request from vendor selected for new request
                            UpdateOldReq = true;
                        }
                        else
                        {
                            //Cannot Create- Prompt with Msg existing Req No : xxxx is still under Procesing
                            UnderProcess = true;
                        }


                        if (UpdateOldReq == true)
                        {
                            DtTemp.Columns.Add("Remark");
                            for (int r = 0; r < DtTemp.Rows.Count; r++)
                            {
                                DtTemp.Rows[r]["Remark"] = "Exist";
                                DtTemp.AcceptChanges();
                            }

                            for (int N = 0; N < grdvendor.Rows.Count; N++)
                            {
                                CheckBox VndChk = (CheckBox)grdvendor.Rows[N].FindControl("chk");
                                if (VndChk.Checked == true)
                                {
                                    bool IsVendorInTheList = false;
                                    for (int r = 0; r < DtTemp.Rows.Count; r++)
                                    {
                                        if (DtTemp.Rows[r]["VendorCode1"].ToString() == grdvendor.Rows[N].Cells[1].Text)
                                        {
                                            IsVendorInTheList = true;
                                            break;
                                        }
                                    }

                                    if (IsVendorInTheList == false)
                                    {
                                        DataRow dr = DtTemp.NewRow();
                                        dr["Plant"] = DtTemp.Rows[0]["Plant"].ToString();
                                        dr["RequestNumber"] = DtTemp.Rows[0]["RequestNumber"].ToString();
                                        dr["RequestDate"] = DtTemp.Rows[0]["RequestDate"].ToString();
                                        dr["QuoteResponseDueDate"] = DtTemp.Rows[0]["QuoteResponseDueDate"].ToString();
                                        dr["QuoteNo"] = grdvendor.Rows[N].Cells[3].Text + DtTemp.Rows[0]["RequestNumber"].ToString();
                                        dr["Material"] = DtTemp.Rows[0]["Material"].ToString();
                                        dr["MaterialDesc"] = DtTemp.Rows[0]["MaterialDesc"].ToString();
                                        dr["VendorCode1"] = grdvendor.Rows[N].Cells[1].Text;
                                        dr["VendorName"] = grdvendor.Rows[N].Cells[2].Text;
                                        dr["Remark"] = "Added";
                                        DtTemp.Rows.Add(dr);
                                        DtTemp.AcceptChanges();
                                    }
                                }
                            }
                            GvUpdateOldReq.DataSource = DtTemp;
                            GvUpdateOldReq.DataBind();
                            Session["UpdateOldReqWithAddNewVnd"] = DtTemp;
                            if (GvUpdateOldReq.Rows.Count > 0)
                            {
                                for (int r = 0; r < DtTemp.Rows.Count; r++)
                                {
                                    if (DtTemp.Rows[r]["Remark"].ToString() == "Added")
                                    {
                                        GvUpdateOldReq.Rows[r].BackColor = Color.LightSeaGreen;
                                    }
                                }
                            }

                            DvUpdateOldReq.Visible = true;
                            DvConfirmForSubmit.Visible = true;
                            Button1.Visible = true;
                        }
                        else if (UnderProcess == true)
                        {
                            GvInvalidRequest.DataSource = DtTemp;
                            GvInvalidRequest.DataBind();
                            DvInvalidRequest.Visible = true;
                        }

                    }
                    #endregion
                }
                else if (Session["InvalidRequestVndNotRespon"] != null)
                {
                    #region InvalidRequestVndNotRespon
                    DataTable DtTemp = (DataTable)Session["InvalidRequestVndNotRespon"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        bool IsVndCountingOldAndNewDiffrence = false;
                        bool IsVndNewReqAndOldEqual = false;
                        bool IsAllVendSelectedIndOldReq = false;
                        int VndSelected = 0;
                        DtTemp.DefaultView.Sort = "RequestNumber asc";

                        #region cek is vndor selected equal with vendor in old request
                        for (int N = 0; N < grdvendor.Rows.Count; N++)
                        {
                            CheckBox VndChk = (CheckBox)grdvendor.Rows[N].FindControl("chk");
                            if (VndChk.Checked == true)
                            {
                                bool IsVndNewInOldReq = false;
                                for (int i = 0; i < DtTemp.Rows.Count; i++)
                                {
                                    IsVndNewInOldReq = false;
                                    if (DtTemp.Rows[i]["VendorCode1"].ToString() == grdvendor.Rows[N].Cells[1].Text)
                                    {
                                        IsVndNewInOldReq = true;
                                        break;
                                    }
                                }

                                if (IsVndNewInOldReq == true)
                                {
                                    IsVndNewReqAndOldEqual = true;
                                }
                                else
                                {
                                    IsVndNewReqAndOldEqual = false;
                                    break;
                                }
                            }
                        }
                        #endregion end cek is vndor selected equal with vendor in old request

                        if (IsVndNewReqAndOldEqual == false)
                        {
                            //auto reject old req
                            AutoRejectOldReq = true;
                        }
                        else
                        {
                            //UnderProcess
                            UnderProcess = true;
                        }

                        if (AutoRejectOldReq == true)
                        {
                            GvOldReqAutoRej.DataSource = DtTemp;
                            GvOldReqAutoRej.DataBind();
                            Session["OldReqAutoRej"] = DtTemp;
                            CreateNewReqTemp();
                            DvOldReqAutoRej.Visible = true;
                            DvConfirmForSubmit.Visible = true;
                            DvValidRequest.Visible = true;
                            LbAutorejectText.Text = "Below Request Will Auto Rej due on : New Request Creation and Vendor Not Response";
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
                        }
                        else if (UnderProcess == true)
                        {
                            GvInvalidRequest.DataSource = DtTemp;
                            GvInvalidRequest.DataBind();
                            DvInvalidRequest.Visible = true;
                        }

                    }
                    #endregion
                }
                else if (Session["InvalidRequestHasApprv"] != null)
                {
                    DataTable DtTemp = (DataTable)Session["InvalidRequestHasApprv"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        GvInvalidRequestHasApprv.DataSource = DtTemp;
                        GvInvalidRequestHasApprv.DataBind();
                        DvInvalidRequestHasApprv.Visible = true;
                    }
                }
                else if (Session["InvalidRequest"] != null)
                {
                    DataTable DtTemp = (DataTable)Session["InvalidRequest"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        GvInvalidRequest.DataSource = DtTemp;
                        GvInvalidRequest.DataBind();
                        DvInvalidRequest.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        bool AutoRejectOldRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (Session["OldReqAutoRej"] != null)
                {
                    DataTable dt = (DataTable)Session["OldReqAutoRej"];
                    if (dt.Rows.Count > 0)
                    {
                        string ReqNoA = "";
                        string PICRejRemark = Session["UserName"].ToString() + " - Quotation Canceled";
                        sql = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string ReqNoB = dt.Rows[i]["RequestNumber"].ToString();
                            if (ReqNoA != ReqNoB)
                            {
                                sql += @" Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "',PICRejRemark = '" + PICRejRemark + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "',  UpdatedBy='" + Session["userID"].ToString() + "', UpdatedOn=CURRENT_TIMESTAMP where RequestNumber = '" + ReqNoB + "' ";
                                ReqNoA = ReqNoB;
                            }
                        }

                        if (sql != "")
                        {
                            cmd = new SqlCommand(sql, EmetCon);
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
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
                EmetCon.Dispose();
            }
        }

        bool UpdateOldReqWithAddNewVnd()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (Session["UpdateOldReqWithAddNewVnd"] != null)
                {
                    DataTable dt = (DataTable)Session["UpdateOldReqWithAddNewVnd"];
                    if (dt.Rows.Count > 0)
                    {
                        sql = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["Remark"].ToString() == "Added")
                            {
                                sql += @" insert into TQuoteDetails (RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,
                                            MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                            MaterialType,PlantStatus,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,isUseSAPCode,SMNPicDept,IMRecycleRatio)
                                            select distinct '" + dt.Rows[i]["RequestNumber"].ToString() + @"',RequestDate,Plant,'" + dt.Rows[i]["QuoteNo"].ToString() + @"',
                                            '" + dt.Rows[i]["VendorCode1"].ToString() + @"','" + dt.Rows[i]["VendorName"].ToString() + @"',ProcessGroup,Product,Material,
                                            MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,EffectiveDate,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,
                                            MaterialType,PlantStatus,'" + Session["userID"].ToString() + @"',BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,isUseSAPCode,'" + Session["userDept"].ToString() + @"' , IMRecycleRatio
                                            from TQuoteDetails where RequestNumber = '" + dt.Rows[i]["RequestNumber"].ToString() + @"' ";

                                sql += @" update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, 
                                        ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo ='" + dt.Rows[i]["QuoteNo"].ToString() + "' ";
                            }
                        }

                        if (sql != "")
                        {
                            cmd = new SqlCommand(sql, EmetCon);
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return false;
                }
                else
                {
                    return true;
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
                isDuplicateReqWithOldNExpiredReqExtend = false;
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
                                DateTime DtDueDate = DateTime.ParseExact(TxtNewDueDate.Text, "dd-MM-yyyy", null);
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
                                    isDuplicateReqWithOldNExpiredReqExtend = true;
                                }
                                PrevReqNo = ReqNo;
                            }
                        }
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.CommandText = sql;
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

        /// <summary>
        /// Create Request button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
            DvInvalidRequest.Visible = false;
            DeleteNonRequest();

            #region New
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
                    createRequest();
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
                        createRequest();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors has selected. Please select at least one vendor!')CloseLoading();", true);
                    }
                }
                else if (RbTeamShimano.Checked == true)
                {
                    if (RbWithouSAPCode.Checked == true && txtpartdesc.Text == "")
                    {
                        string VendorCode = "";
                        VendorCode = DdlVendor.SelectedValue.ToString();
                        if (VendorCode == "00")
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select Plant !');CloseLoading();", true);
                        }
                        else if (VendorCode == "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Plant Not Exist!');CloseLoading();", true);
                        }
                        else
                        {
                            createRequest();
                        }
                    }
                    else
                    {
                        createRequest();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('please select Vendor Type !');CloseLoading();", true);
                }
            }
            #endregion


        }

        protected void DdlMatClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetSapPart();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        //protected void chckchanged(object sender, EventArgs e)
        //{

        //    CheckBox chckheader = (CheckBox)GridView1.HeaderRow.FindControl("chkheader");

        //    foreach (GridViewRow row in grdvendor.Rows)
        //    {

        //        CheckBoxchckrw = (CheckBox)row.FindControl("CheckBox2");

        //        if (chckheader.Checked == true)
        //        {
        //            chckrw.Checked = true;
        //        }
        //        else
        //        {
        //            chckrw.Checked = false;
        //        }

        //    }

        //}  

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
                resetProgress();
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
                resetProgress();
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
                resetProgress();
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
                resetProgress();
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
            //if (FileUpload1.HasFile)
            //{
            //    Session["FlAttchDrawing"] = FileUpload1;
            //    lblMessage.Text = FileUpload1.FileName.ToString();
            //    FileUpload1 = (FileUpload)Session["FlAttchDrawing"];
            //}
            //getdate();

            if (Session["FlAttchDrawing"] != null)
            {
                HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                if (Fl != null)
                {
                    string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                    string FileName = System.IO.Path.GetFileName(Fl.FileName);
                    lblMessage.Text = FileName;
                }
            }
            getdate();
        }

        /// <summary>
        /// Load Shimano PIC details 
        /// </summary>
        private void GetSHMNPIC()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select UseID,UseNam from USR where UseID='SBM_BIKE940146' and delflag = 0";

                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                string struser = dtdate.Rows[0]["UseNam"].ToString();
                lbluser.Text = dtdate.Rows[0]["UseID"].ToString();

                Session["userID"] = userId;
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
        /// Sap Part code Description text change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdescription_TextChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Get Material code from Material Name. -- Material code = SAP Part Code , Material Name = Sap Part code description
        /// </summary>
        /// <param name="materialname"></param>
        private void GetMaterialcode(string materialname)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string str = "";
                str = @"Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating, TM.MaterialType,TM.PlantStatus, 
                        TM.PROCTYPE,TM.SplPROCTYPE,PR.product as Product,TR.ProdComDesc,TM.BaseUOM 
                        from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode 
                        INNER JOIN TPRODUCT PR ON TM.Product = PR.Product where Tm.Material ='" + materialname + @"' 
                        and tm.Plant = '" + txtplant.Text.ToString() + @"' and TM.delflag = 0 ";

                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);
                if (dtdate.Rows.Count > 0)
                {

                    txtpartdesc.Text = dtdate.Rows[0]["material"].ToString();
                    txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                    txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                    txtBaseUOM1.Text = dtdate.Rows[0]["BaseUOM"].ToString();
                    txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();

                    if (ddlmatltype.Items.FindByValue(dtdate.Rows[0]["MaterialType"].ToString()) != null)
                    {
                        ddlmatltype.SelectedValue = dtdate.Rows[0]["MaterialType"].ToString();
                    }
                    else
                    {
                        ddlmatltype.SelectedIndex = -1;
                    }

                    if (ddlproctype.Items.FindByValue(dtdate.Rows[0]["PROCTYPE"].ToString()) != null)
                    {
                        ddlproctype.SelectedValue = dtdate.Rows[0]["PROCTYPE"].ToString();
                    }
                    else
                    {
                        ddlproctype.SelectedIndex = -1;
                    }

                    if (DdlProduct.Items.FindByValue(dtdate.Rows[0]["Product"].ToString()) != null)
                    {
                        DdlProduct.SelectedValue = dtdate.Rows[0]["Product"].ToString();
                    }
                    else
                    {
                        DdlProduct.SelectedIndex = -1;
                    }


                    string prod = ddlmatltype.SelectedItem.Text;
                    GetPlantStatus(prod);
                    GetSplProcType(dtdate.Rows[0]["PROCTYPE"].ToString());

                    string strddlpSval = dtdate.Rows[0]["PlantStatus"].ToString();
                    if (ddlplantstatus.Items.FindByValue(strddlpSval) != null)
                    {
                        ddlplantstatus.SelectedValue = strddlpSval;
                    }
                    else
                    {
                        ddlplantstatus.SelectedIndex = -1;
                    }

                    string splProc = dtdate.Rows[0]["SplPROCTYPE"].ToString();
                    if (splProc == "")
                    {
                        if (ddlsplproctype.Items.FindByValue("Blank") != null)
                        {
                            ddlsplproctype.SelectedValue = "Blank";
                        }
                        else
                        {
                            ddlsplproctype.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        if (ddlsplproctype.Items.FindByValue(splProc) != null)
                        {
                            ddlsplproctype.SelectedValue = splProc;

                            if (ddlsplproctype.SelectedValue.ToString() == "30")
                            {
                                ChcDisMatCost.Visible = true;
                                ChcDisMatCost.Checked = true;
                            }
                            else
                            {
                                ChcDisMatCost.Visible = false;
                            }
                        }
                        else
                        {
                            ddlsplproctype.SelectedIndex = -1;
                        }
                    }



                    string proc = ddlproctype.SelectedValue.ToString();
                    string SpProc = ddlsplproctype.SelectedValue.ToString();
                    getMatlClass();
                    if (DdlMatClass.Items.FindByValue(dtdate.Rows[0]["ProdComDesc"].ToString()) != null)
                    {
                        DdlMatClass.SelectedValue = dtdate.Rows[0]["ProdComDesc"].ToString();
                    }
                    else
                    {
                        DdlMatClass.SelectedIndex = -1;
                    }
                    GetPIRTypesbysplproc(proc, SpProc);
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
        /// Job type Text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtjobtypedesc_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// SAP Part code text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdesc_TextChanged1(object sender, EventArgs e)
        {
            try
            {

                if (txtpartdesc.Text == "")
                {

                    txtpartdescription.Text = "";
                    txtunitweight.Text = "";
                    txtUOM.Text = "";
                    txtplatingtype.Text = "";
                }
                else
                {
                    GetProdMaterial(txtpartdesc.Text);
                    GetMaterialcode(txtpartdesc.Text);
                    //if (ddlsplproctype.SelectedIndex == 0)
                    //{
                    //    if (txtpartdesc.Text.Length >= 5)
                    //    {
                    //        string strmaterialDesc = txtpartdescription.Text;
                    //        string strmaterial = txtpartdesc.Text;
                    //        GetProdMaterial(strmaterial);
                    //    }
                    //    else
                    //    {
                    //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Enter min 5 chars, else choose the filter options!')", true);
                    //        // ddlmatltype.Focus();
                    //    }
                    //}
                    //else if (ddlsplproctype.SelectedIndex != 0)
                    //{
                    //    string strmaterialDesc = txtpartdescription.Text;
                    //    string strmaterial = txtpartdesc.Text;

                    //    //string[] matl = strmaterial.Split('-');
                    //    //string getmatl = matl[0].ToString();
                    //    //Session["material"] = getmatl.ToString();
                    //    GetProdMaterial(strmaterial);
                    //    if (ddlsplproctype.SelectedValue.ToString() == "30")
                    //    {
                    //        ChcDisMatCost.Visible = true;
                    //        ChcDisMatCost.Checked = true;
                    //    }
                    //    else
                    //    {
                    //        ChcDisMatCost.Visible = false;
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        /// <summary>
        /// SAP Part code description text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdescription_TextChanged1(object sender, EventArgs e)
        {
            try
            {

                //if (txtpartdescription.Text == "")
                //{
                //    txtpartdesc.Text = "";
                //    txtunitweight.Text = "";
                //    txtUOM.Text = "";
                //    txtplatingtype.Text = "";
                //}
                //else
                //{
                //    if (ddlsplproctype.SelectedIndex == 0)
                //    {
                //        if (txtpartdescription.Text.Length >= 10)
                //        {
                //            string strmaterialDesc = txtpartdescription.Text;
                //            string strmaterial = txtpartdesc.Text;
                //            GetMaterialcode(strmaterial);

                //        }
                //        else
                //        {
                //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Enter min 10 chars. else choose the filter options!')", true);
                //            ddlmatltype.Focus();
                //        }
                //    }

                //    else if (ddlsplproctype.SelectedIndex != 0)
                //    {
                //        string strmaterialDesc = txtpartdescription.Text;
                //        string strmaterial = txtpartdesc.Text;
                //        GetMaterialcode(strmaterial);

                //        if (ddlsplproctype.SelectedValue.ToString() == "30")
                //        {
                //            ChcDisMatCost.Visible = true;
                //            ChcDisMatCost.Checked = true;
                //        }
                //        else
                //        {
                //            ChcDisMatCost.Visible = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }


        /// <summary>
        /// Load data and Create Dynamic table with BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
        protected void GetData(string reqno)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                Session["Rawmaterial"] = null;
                Session["OldRawmaterial"] = null;
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
            finally
            {
                //MDMCon.Dispose();
            }

            try
            {
                DataTable dtget = new DataTable();
                DataTable dtget1 = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da1 = new SqlDataAdapter();

                string strGetData = string.Empty;
                string strGetData1 = string.Empty;
                string tran = Convert.ToString(TransDB.ToString());

                // strGetData1 = "select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as VendorName,v.SearchTerm,TQ.vendorCode1 as VendorCode,TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount as Amt_SCur,isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as ExchRate,v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,tc.Unit,tc.UoM from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material  inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor inner join " + Convert.ToString(TransDB.ToString()) + "TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 left outer join  TEXCHANGE_RATE tr on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  " +
                //" where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + txtplant.Text.ToString() + "' and TB.FGCode = '" + txtpartdesc.Text.ToString() + "' and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";
                string plant = txtplant.Text;
                if (RbTeamShimano.Checked == true)
                {
                    plant = TxtPlantVendor.Text;
                }

                #region Not Used
                //strGetData1 = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tvpo.Plant,TB.Material as [Comp Material],
                //            tm.MaterialDesc as [Comp Material Desc],v.Description as VendorName,tvpo.coderef as SearchTerm,TQ.vendorCode1 as VendorCode,TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,
                //            tc.Amount as Amt_SCur,isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as ExchRate,v.Crcy AS Venor_Crcy, 
                //            isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,tc.Unit,tc.UoM 
                //            from TMATERIAL tm  inner join TBOMLIST TB on tm.Material = TB.Material  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material   
                //            inner join tVendor_New v on v.CustomerNo = tc.Customer  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                //            inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1  
                //            left outer join  TEXCHANGE_RATE tr on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and  tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  " +
                //                " where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"' 
                //            and tvpo.Plant='" + plant + "' and TB.FGCode = '" + txtpartdesc.Text.ToString() + @"' 
                //            and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";

                //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                //      strGetData1 = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tvpo.Plant,TB.Material as [Comp Material],
                //                  tm.MaterialDesc as [Comp Material Desc],v.Description as VendorName,tvpo.coderef as SearchTerm,TQ.vendorCode1 as VendorCode,TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,
                //                  tc.Amount as Amt_SCur,isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,4)),2) ,'1') as ExchRate,v.Crcy AS Venor_Crcy, 
                //                  isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,tc.Unit,
                //                  --tc.UoM
                //                  tm.BaseUOM as UOM,
                //format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom
                //                  from TMATERIAL tm  inner join TBOMLIST TB on tm.Material = TB.Material  
                //                  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material    and tc.plant = '" + plant + @"' and tc.delflag = 0
                //                  inner join tVendor_New v on v.CustomerNo = tc.Customer  
                //                  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.POrg = v.POrg
                //                  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                //                  inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1  
                //                  left outer join  TEXCHANGE_RATE tr on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and  tm.Plant = TQ.Plant and VP.Plant = TQ.Plant 
                //                  and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                //                  where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"' 
                //                  and tvpo.Plant='" + plant + "' and vp.plant = '" + plant + @"' and TB.FGCode = '" + txtpartdesc.Text.ToString() + @"' 
                //                  and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                //                  and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0) ";
                //      if (article.Checked == true)
                //      {
                //          DateTime dteffdate = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                //          strGetData1 += " and ('" + dteffdate.ToString("yyyy-MM-dd") + @"' between tc.ValidFrom and tc.ValidTo) ";
                //      }
                //      //subash
                //      da1 = new SqlDataAdapter(strGetData1, con);
                //      da1.Fill(dtget1);
                //      if (dtget1.Rows.Count > 0)
                //      {
                //          grdvendor1.DataSource = dtget1;
                //          grdvendor1.DataBind();
                //      }
                //      else
                //      {
                //          grdvendor1.DataSource = dtget1;
                //          grdvendor1.DataBind();
                //      }


                //strGetData = @" select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                //        TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],
                //        TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                //        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,2)),2) ,'1') as ExchRate,
                //        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                //        tc.Unit,tc.UoM 
                //        from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material   
                //        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material    
                //        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                //        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                //        inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                //        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant   " +
                //            " where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"'  
                //        and tvpo.Plant='" + plant + @"' and TB.FGCode = '" + txtpartdesc.Text.ToString() + @"' 
                //        and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)";

                //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                //          strGetData = @" select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                //                  TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],
                //                  TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                //                  isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                //                  v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                //                  tc.Unit,
                //                  --tc.UoM 
                //                  tm.BaseUOM as UOM,
                //format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                //                  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material   
                //                  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material   and tc.plant = '" + plant + @"'  and tc.delflag = 0
                //                  inner join tVendor_New v on v.CustomerNo = tc.Customer   
                //                  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                //                  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                //                  inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                //                  left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant
                //                  and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                //                  where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"' 
                //                  and tvpo.Plant='" + plant + @"' and vp.plant = '" + plant + @"' and TB.FGCode = '" + txtpartdesc.Text.ToString() + @"' 
                //                  and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                //                  and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)
                //                  and tc.delflag = 0
                //                  ";
                #endregion

                //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                strGetData = @"  
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
                and [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate
                and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate 
                and UPPER([component plant status]) not in ('Z4','Z9')
                and UPPER([parent plant status]) not in ('Z4','Z9')				
                and [co-product] <> 'X'

                --get the list of e50 components
                SELECT Plant,[Component Material] as 'Material'
                into #e50 from tbomlistnew 
                where Plant=@plant and [Parent Material]=@mat 
                --no need altbom because not using PV for direct transfer
                --and [alternative bom]=@altbom
                and [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate 
                and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate  
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
	                where [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate
	                and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate
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
	                where [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate  
	                and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate  
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
                #region cancel cause data from bom list will take from #complist1
                //strGetData += @"  select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                //        TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],tvpo.coderef as SearchTerm,
                //        TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                //        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                //        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                //        tc.Unit,
                //        --tc.UoM 
                //        tm.BaseUOM as UOM,
                //        format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                //        from TMATERIAL tm inner join TBOMLISTnew TB on tm.Material = TB.[Component Material]   
                //        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @plant  and tc.delflag = 0
                //        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                //        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                //        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                //        inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                //        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant
                //        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                //        where TQ.RequestNumber=@reqno and tm.Plant=@plant
                //        and tvpo.Plant=@plant and vp.plant = @plant and TB.[Parent Material] = @mat 
                //        and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                //        and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)
                //        and tc.delflag = 0
                //        ";
                #endregion
                strGetData += @"  select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                        TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],tvpo.coderef as SearchTerm,
                        TQ.QuoteNo,vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,
                        --tc.UoM 
                        tm.BaseUOM as UOM,
                        format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                        from TMATERIAL tm inner join #complist1 TB on tm.Material = TB.[Component Material]
                        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @plant  and tc.delflag = 0
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                        inner join " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant
                        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                        where TQ.RequestNumber=@reqno and tm.Plant=@plant
                        and tvpo.Plant=@plant and vp.plant = @plant and TB.[Parent Material] = @mat 
                        and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                        and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)
                        and tc.delflag = 0
                         
                        ";
                if (article.Checked == true)
                {
                    strGetData += " and (@EffectiveDate between tc.ValidFrom and tc.ValidTo) ";
                }

                strGetData += @" --select * from #complist1
                                 drop table #complist1, #e50 ";

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strGetData, MDMCon);
                    cmd.Parameters.AddWithValue("@reqno", reqno);
                    cmd.Parameters.AddWithValue("@mat", txtpartdesc.Text);
                    cmd.Parameters.AddWithValue("@plant", txtplant.Text);
                    DateTime ValidTo = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@EffectiveDate", ValidTo.ToString("yyyy-MM-dd"));
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            dtget = dt;
                            grdvendor1.DataSource = dt;
                            grdvendor1.DataBind();
                        }
                    }
                }

                //subash
                //da = new SqlDataAdapter(strGetData, con);
                //da.Fill(dtget);

                //grdvendor1.DataSource = dtget;
                //grdvendor1.DataBind();

                //foreach (DataRow row in dtget.Rows)
                //{
                //    //string a = row.Cell[3]
                //}

                if (dtget.Rows.Count > 0)
                {
                    Session["Rawmaterial"] = dtget;

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

                            if (cellCtr == dtget.Rows[0].ItemArray.Length - 1)
                            {
                                if (txtPIRDesc.Text.ToString().ToUpper().Contains("SUBCON"))
                                {
                                    tCell.Text = "";
                                }
                            }
                        }
                        rowcount++;

                    }
                    Session["hdnReqNo"] = dtget.Rows[0]["Req No"].ToString();
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
            finally
            {
                MDMCon.Dispose();
            }
        }

        //private void BtnSubmitdynamic_Click(object sender, EventArgs e)
        //{
        //	Response.Redirect("NewReq_changes.aspx?Number=" + Table1.Rows[1].Cells[0].ToString());
        //	//ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Mail Send successfully to Vendors');", true);
        //}

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

            if (Session["FlAttchDrawing"] != null)
            {
                HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                if (Fl != null)
                {
                    string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                    string filename = System.IO.Path.GetFileName(Fl.FileName);
                    string PathAndFileName = folderPath + DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
                    Fl.SaveAs(PathAndFileName);

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

        bool ProcesSendEmail()
        {
            errMsg = "";
            try
            {
                SqlTransaction transaction11 = null;
                #region send emai
                //subash - email -Begin
                bool sendemail = false;
                string msg = "";
                try
                {
                    string dg_checkvalue, dg_formid = string.Empty;
                    string getQuoteRef = string.Empty;
                    string formatstatus = string.Empty;
                    string remarks = string.Empty;
                    string currentMonth = DateTime.Now.Month.ToString();
                    string currentYear = DateTime.Now.Year.ToString();
                    string RequestIncNumber = "";
                    int increquest = 000000;
                    string RequestInc = String.Format(currentYear, increquest);
                    string REQUEST = string.Concat(currentYear, Session["RequestIncNumber1"].ToString());
                    string strdate = txtReqDate.Text;
                    int rowscount = 0;

                    //getting number of vendors

                    foreach (GridViewRow gr in grdvendor1.Rows)
                    {
                        //CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                        //if (myCheckbox.Checked == true)
                        //{
                        //dg_checkvalue = "Y";
                        dg_formid = gr.Cells[0].Text.ToString();
                        string dg_VenName = gr.Cells[1].Text.ToString().Replace("amp;", " ");
                        dg_checkvalue = gr.Cells[1].Text.ToString();
                        string strvendname = dg_checkvalue.ToString();
                        //subash////string mstrvenname = strvendname.Substring(0, 4);
                        //subash////string getVendName = mstrvenname[0].ToString();
                        string QuoteSearchTerm = "";
                        QuoteSearchTerm = gr.Cells[2].Text.ToString();
                        QuoteSearchTerm = QuoteSearchTerm.Substring(0, 3);
                        string getquote = String.Concat(QuoteSearchTerm, Session["RequestIncNumber1"].ToString());
                        string strPlatingType = txtplatingtype.Text.ToString();

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
                                        if (article.Checked == true)
                                        {
                                            getQuoteRef = getquote;
                                        }
                                        else
                                        {
                                            getQuoteRef = getquote + "D";
                                        }
                                        remarks = "Open Status";
                                        rowscount++;

                                        string Proccode = DdlProduct.SelectedValue.ToString();
                                        string PIRJobType = "";
                                        string PITType = "";
                                        string PITTypeDesc = "";
                                        if (Session["PIRJOBTYPE"] != null)
                                        {
                                            PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                                        }
                                        if (Session["Pirtype"] != null)
                                        {
                                            PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                                        }
                                        if (Session["PirDescription"] != null)
                                        {
                                            PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();
                                        }
                                        string UnitNetWeight = txtunitweight.Text.ToString();
                                        DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd-MM-yyyy", null);
                                        string PITTypeandDesc = PITType + "- " + PITTypeDesc;
                                        // getting Messageheader ID from IT Mailapp
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
                                                EMETModule.SendExcepToDB(cc2);
                                                transactionHS.Rollback();
                                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + cc2 + " ");
                                                var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                            }
                                            cnn.Dispose();
                                        }

                                        Boolean IsAttachFile = true;
                                        int SequenceNumber = 1;
                                        string test = userId;

                                        //Uploading  ttachment to Mail sever using UNC credentials
                                        //fname = Session["fname"].ToString();
                                        if (Session["FlAttchDrawing"] != null)
                                        {
                                            HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                                            if (Fl != null)
                                            {
                                                string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                                                string FileName = System.IO.Path.GetFileName(Fl.FileName);
                                                fname = FileName;
                                            }
                                        }

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
                                                            if (Session["FlAttchDrawing"] != null)
                                                            {
                                                                HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                                                                if (Fl != null)
                                                                {
                                                                    string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                                                                    string FileName = System.IO.Path.GetFileName(Fl.FileName);
                                                                    filename = System.IO.Path.GetFileName(Fl.FileName);
                                                                    PathAndFileName = folderPath + filename;
                                                                    Fl.SaveAs(PathAndFileName);
                                                                }
                                                            }
                                                            Source = PathAndFileName;
                                                            File.Copy(Source, Destination, true);
                                                            //Source = Session["Source"].ToString();
                                                            //fname = Session["fname"].ToString();
                                                            //SendFilename = Session["SendFilename"].ToString();
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
                                                            EMETModule.SendExcepToDB(xw1);
                                                            IsAttachFile = false;
                                                            SendFilename = "NOFILE";
                                                            OriginalFilename = "NOFILE";
                                                            Session["OriginalFilename"] = "NOFILE";
                                                            Session["SendFilename"] = "NOFILE";
                                                            format = "NO";
                                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Attachment Failed: " + xw1 + " ");
                                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception x)
                                            {
                                                EMETModule.SendExcepToDB(x);
                                                string message = x.Message;
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

                                        //getting vendor mail id
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

                                        //getting User mail id
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
                                            vendorid.Value = Label15.Text.ToString();
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


                                        // Insert header and details to Mil server table to IT mailserverapp

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
                                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation been created by " + Label16.Text.ToString() + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + "<br />  Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response Due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                                body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + " <br /> Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                                string BodyFormat = "HTML";
                                                string BodyRemark = "0";
                                                string Signature = "";
                                                string Importance = "High";
                                                string Sensitivity = "Confidential";
                                                string CreateUser = Label15.Text.ToString();
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
                                                    EMETModule.SendExcepToDB(cc2);
                                                    transactionHe.Rollback();
                                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + cc2 + " ");
                                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
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
                                                    EMETModule.SendExcepToDB(cc1);
                                                    transactionDe.Rollback();
                                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email detail: " + cc1 + " ");
                                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
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
                                                    }
                                                    catch (Exception cc1)
                                                    {
                                                        EMETModule.SendExcepToDB(cc1);
                                                        transaction11.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Content Issue in Transaction table: " + cc1 + " ");
                                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                    }
                                                    Email_inser1.Dispose();
                                                }
                                                //End Details
                                            }

                                        }
                                        catch (Exception cc1)
                                        {
                                            EMETModule.SendExcepToDB(cc1);
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + cc1 + " ");
                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                        //}
                                        //else
                                        //{
                                        //    dg_checkvalue = "";
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    sendemail = true;
                }
                catch (Exception ex)
                {
                    EMETModule.SendExcepToDB(ex);
                    msg = ex.ToString();
                    errMsg = ex.ToString();
                    sendemail = false;
                }

                if (sendemail == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //End by subash
                // Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
                #endregion send emai
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                return false;
            }
        }

        bool ProcesSendEmail2()
        {
            try
            {
                SqlTransaction transaction11 = null;
                #region send emai
                //subash - email -Begin
                bool sendemail = false;
                string msg = "";
                try
                {
                    //string dg_checkvalue, dg_formid = string.Empty;
                    //string getQuoteRef = string.Empty;
                    //string formatstatus = string.Empty;
                    //string remarks = string.Empty;
                    //string currentMonth = DateTime.Now.Month.ToString();
                    //string currentYear = DateTime.Now.Year.ToString();
                    //string RequestIncNumber = "";
                    //int increquest = 000000;
                    //string RequestInc = String.Format(currentYear, increquest);
                    //string REQUEST = string.Concat(currentYear, Session["RequestIncNumber1"].ToString());
                    //string strdate = txtReqDate.Text;
                    //int rowscount = 0;

                    //getting number of vendors
                    if (Session["UpdateOldReqWithAddNewVnd"] != null)
                    {
                        DataTable dtUpdateOldReqWithAddNewVnd = (DataTable)Session["UpdateOldReqWithAddNewVnd"];
                        for (int i = 0; i < dtUpdateOldReqWithAddNewVnd.Rows.Count; i++)
                        {
                            string VC = dtUpdateOldReqWithAddNewVnd.Rows[i]["VendorCode1"].ToString();

                            if (dtUpdateOldReqWithAddNewVnd.Rows[i]["Remark"].ToString() == "Added")
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
                                        EMETModule.SendExcepToDB(cc2);
                                        transactionHS.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + cc2 + " ");
                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    cnn.Dispose();
                                }
                                #endregion


                                #region Uploading  ttachment to Mail sever using UNC credentials
                                Boolean IsAttachFile = true;
                                int SequenceNumber = 1;
                                string test = userId;

                                //Uploading  ttachment to Mail sever using UNC credentials
                                //fname = Session["fname"].ToString();
                                if (Session["FlAttchDrawing"] != null)
                                {
                                    HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                                    if (Fl != null)
                                    {
                                        string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                                        string FileName = System.IO.Path.GetFileName(Fl.FileName);
                                        fname = System.IO.Path.GetFileName(Fl.FileName);
                                    }
                                }

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
                                                    if (Session["FlAttchDrawing"] != null)
                                                    {
                                                        HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                                                        if (Fl != null)
                                                        {
                                                            string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                                                            string FileName = System.IO.Path.GetFileName(Fl.FileName);
                                                            filename = System.IO.Path.GetFileName(Fl.FileName);
                                                            PathAndFileName = folderPath + filename;
                                                            Fl.SaveAs(PathAndFileName);
                                                        }
                                                    }

                                                    Source = PathAndFileName;
                                                    File.Copy(Source, Destination, true);
                                                    //Source = Session["Source"].ToString();
                                                    //fname = Session["fname"].ToString();
                                                    //SendFilename = Session["SendFilename"].ToString();
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
                                                    EMETModule.SendExcepToDB(xw1);
                                                    IsAttachFile = false;
                                                    SendFilename = "NOFILE";
                                                    OriginalFilename = "NOFILE";
                                                    Session["OriginalFilename"] = "NOFILE";
                                                    Session["SendFilename"] = "NOFILE";
                                                    format = "NO";
                                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Attachment Failed: " + xw1 + " ");
                                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception x)
                                    {
                                        EMETModule.SendExcepToDB(x);
                                        string message = x.Message;
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
                                //getting vendor mail id
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
                                    vendorid.Value = VC;
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
                                //getting User mail id
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
                                    vendorid.Value = Label15.Text.ToString();
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
                                // Insert header and details to Mil server table to IT mailserverapp
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
                                        string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation been created by " + Label16.Text.ToString() + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :  " + dtUpdateOldReqWithAddNewVnd.Rows[i]["VendorName"].ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + dtUpdateOldReqWithAddNewVnd.Rows[i]["QuoteNo"].ToString() + "<br />  Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response Due Date    :   " + dtUpdateOldReqWithAddNewVnd.Rows[i]["QuoteResponseDueDate"].ToString() + "<br /><br />" + footer;
                                        body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dtUpdateOldReqWithAddNewVnd.Rows[i]["VendorName"].ToString() + "<br />  Request Number  :   " + Convert.ToInt32(Session["RequestIncNumber1"].ToString().ToString()) + "<br />  Quote Number    :   " + dtUpdateOldReqWithAddNewVnd.Rows[i]["QuoteNo"].ToString() + " <br /> Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response due Date    :   " + dtUpdateOldReqWithAddNewVnd.Rows[i]["QuoteResponseDueDate"].ToString() + "<br /><br />" + footer;
                                        string BodyFormat = "HTML";
                                        string BodyRemark = "0";
                                        string Signature = "";
                                        string Importance = "High";
                                        string Sensitivity = "Confidential";
                                        string CreateUser = Label15.Text.ToString();
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
                                            EMETModule.SendExcepToDB(cc2);
                                            transactionHe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + cc2 + " ");
                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
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
                                            EMETModule.SendExcepToDB(cc1);
                                            transactionDe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email detail: " + cc1 + " ");
                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
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
                                                Detail.Parameters.AddWithValue("@Quotenumber", dtUpdateOldReqWithAddNewVnd.Rows[i]["QuoteNo"].ToString());
                                                Detail.Parameters.AddWithValue("@body", body1.ToString());
                                                Detail.CommandText = Details;
                                                Detail.ExecuteNonQuery();
                                                transaction11.Commit();
                                            }
                                            catch (Exception cc1)
                                            {
                                                EMETModule.SendExcepToDB(cc1);
                                                transaction11.Rollback();
                                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Content Issue in Transaction table: " + cc1 + " ");
                                                var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                            }
                                            Email_inser1.Dispose();
                                        }
                                        //End Details
                                    }

                                }
                                catch (Exception cc1)
                                {
                                    EMETModule.SendExcepToDB(cc1);
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + cc1 + " ");
                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                #endregion
                            }
                        }
                    }

                    sendemail = true;
                }
                catch (Exception ex)
                {
                    EMETModule.SendExcepToDB(ex);
                    msg = ex.ToString();
                    errMsg = ex.ToString();
                    sendemail = false;
                }

                //End by subash
                // Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
                #endregion send emai

                if (sendemail == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                return false;
            }
        }

        /// <summary>
        /// Get BOM Detail Raw Material before efffective date
        /// </summary>
        protected void GetBOMRawmaterialBefEffdate(string ReqNo, string QuoteNo, string VendorCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    #region Not Used
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
                    #endregion

                    //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                    #region not used cause
                    //              string sql = @" select distinct @ReqNo as [Req No],@QuoteNo as [QuoteNo],@VendorCode [Vendor Code],tvpo.Plant,  
                    //                  TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                    //                  vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                    //                  isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                    //                  v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                    //                  tc.Unit,tm.BaseUOM as UOM,
                    //format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                    //                  into #temp1
                    //                  from TMATERIAL tm inner join TBOMLISTnew TB on tm.Material = TB.[Component Material]   
                    //                  inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @Plant and tc.delflag = 0
                    //                  inner join tVendor_New v on v.CustomerNo = tc.Customer   
                    //                  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                    //                  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                    //                  left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = @Plant and VP.Plant = @Plant  and tvpo.Plant = @Plant
                    //                  and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= @ValidTo )
                    //                  where tm.Plant=@Plant and tvpo.Plant=@Plant and vp.plant = @Plant and TB.[Parent Material] = @Material
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
                    #endregion

                    string sql = @" ";
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
                    where Plant=@plant and [Parent Material]=@Material 
                    --no need altbom because not using PV for direct transfer
                    --and [alternative bom]=@altbom 
                    and [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate
                    and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate 
                    and UPPER([component plant status]) not in ('Z4','Z9')
                    and UPPER([parent plant status]) not in ('Z4','Z9')				
                    and [co-product] <> 'X'

                    --get the list of e50 components
                    SELECT Plant,[Component Material] as 'Material'
                    into #e50 from tbomlistnew 
                    where Plant=@plant and [Parent Material]=@Material 
                    --no need altbom because not using PV for direct transfer
                    --and [alternative bom]=@altbom
                    and [header valid to date] > @EffectiveDate and [header valid from date] <= @EffectiveDate 
                    and [comp. valid to date] > @EffectiveDate and [comp. valid from date] <= @EffectiveDate  
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
	                    where [header valid to date] < @ValidTo 
	                    and [comp. valid to date] < @ValidTo
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
	                    where [header valid to date] < @ValidTo
	                    and [comp. valid to date] < @ValidTo
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
                        TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                        vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,tm.BaseUOM as UOM,
						format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                        into #temp1
                        from TMATERIAL tm inner join #complist1 TB on tm.Material = TB.[Component Material]   
                        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @Plant and tc.delflag = 0
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = @Plant and VP.Plant = @Plant  and tvpo.Plant = @Plant
                        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= @ValidTo )
                        where tm.Plant=@Plant and tvpo.Plant=@Plant and vp.plant = @Plant and TB.[Parent Material] = @Material
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

                    sql += @" --select * from #complist1
                                 drop table #complist1, #e50 ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    cmd.Parameters.AddWithValue("@Material", txtpartdesc.Text);
                    cmd.Parameters.AddWithValue("@Plant", txtplant.Text);
                    DateTime ValidTo = DateTime.ParseExact(TxtEffectiveDate.Text, "dd-MM-yyyy", null);
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

        /// <summary>
        /// submit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction transemet = null;
            try
            {
                MDMCon.Open();
                EmetCon.Open();
                transemet = EmetCon.BeginTransaction();

                if (AutoRejectOldRequest() == true)
                {
                    if (UpdateOldReqWithAddNewVnd() == true)
                    {
                        #region Process Submit request

                        if ((Session["flag"].ToString() == "Pass") && (txtMQty.Text != ""))
                        {
                            fname = "";
                            if (Session["FlAttchDrawing"] != null)
                            {
                                HttpPostedFile Fl = (HttpPostedFile)Session["FlAttchDrawing"];
                                if (Fl != null)
                                {
                                    string FileExtension = System.IO.Path.GetExtension(Fl.FileName);
                                    string FileName = System.IO.Path.GetFileName(Fl.FileName);
                                    fname = System.IO.Path.GetFileName(Fl.FileName);
                                }
                            }
                            //fname = Session["fname"].ToString();
                            if (fname != "")
                            {
                                DataTable dtdate = new DataTable();
                                SqlDataAdapter da = new SqlDataAdapter();

                                string strGetData = string.Empty;
                                string plnt = txtplant.Text;
                                if (RbTeamShimano.Checked == true)
                                {
                                    plnt = TxtPlantVendor.Text;
                                }

                                string ReqNum = Session["hdnReqNo"].ToString();

                                DataTable dtget = new DataTable();
                                if (Session["Rawmaterial"] != null)
                                {
                                    dtget = (DataTable)Session["Rawmaterial"];
                                }



                                if (dtget.Rows.Count > 0)
                                {
                                    Table dtTable1 = (Table)Session["dtTable1"];
                                    if (dtTable1 != null)
                                    {
                                        if (dtTable1.Rows.Count > 0)
                                        {
                                            DataTable dtget1 = new DataTable();

                                            DataTable dtdate1 = new DataTable();
                                            SqlDataAdapter da1 = new SqlDataAdapter();

                                            string sql = string.Empty;
                                            string PrevVndCode = "";
                                            string CurrentVndCode = "";

                                            #region Update Request data in Tquote Deatial
                                            for (int t = 1; t < dtTable1.Rows.Count; t++)
                                            {
                                                string Qno = "";
                                                string RequestNo = "";
                                                if (t > 1)
                                                {
                                                    Qno = dtTable1.Rows[t].Cells[5].Text.ToString();
                                                    RequestNo = dtTable1.Rows[1].Cells[(1)].Text.ToString();
                                                    CurrentVndCode = dtTable1.Rows[t].Cells[(3)].Text.ToString();
                                                }
                                                else
                                                {
                                                    Qno = dtTable1.Rows[t].Cells[(8)].Text.ToString();
                                                    RequestNo = dtTable1.Rows[t].Cells[(1)].Text.ToString();
                                                    CurrentVndCode = dtTable1.Rows[t].Cells[(6)].Text.ToString();
                                                }


                                                if (CurrentVndCode != PrevVndCode)
                                                {
                                                    if (article.Checked == true)
                                                    {
                                                        GetBOMRawmaterialBefEffdate(RequestNo, Qno, CurrentVndCode);
                                                    }
                                                    PrevVndCode = CurrentVndCode;
                                                }

                                                //updated by subash
                                                if (article.Checked == true)
                                                {
                                                    sql = @"update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, 
                                                ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo ='" + Qno + "'";
                                                }
                                                else
                                                {
                                                    sql = "update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus='4',PICApprovalStatus = '4', ManagerApprovalStatus = '4', DIRApprovalStatus = '4' where QuoteNo ='" + Qno + "'";
                                                }
                                                cmd = new SqlCommand(sql, EmetCon, transemet);
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

                                                                    cmd = new SqlCommand(sql, EmetCon, transemet);
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
                                            #endregion

                                            #region save data bom list raw material baed on effective date
                                            if (Session["Rawmaterial"] != null)
                                            {
                                                DataTable dtRawmat = (DataTable)Session["Rawmaterial"];
                                                if (dtRawmat.Rows.Count > 0)
                                                {
                                                    for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                                                    {
                                                        string ExchValidFrom = "";
                                                        if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                                        {
                                                            DateTime DtValFrm = DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);
                                                            ExchValidFrom = DtValFrm.ToString("yyyy-MM-dd");
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
                                                        sql += @" CURRENT_TIMESTAMP, '" + Session["userID"].ToString() + @"') ";

                                                        cmd = new SqlCommand(sql, EmetCon, transemet);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region save data bom list raw material before effective date
                                            if (Session["OldRawmaterial"] != null)
                                            {
                                                DataTable dtRawmat = (DataTable)Session["OldRawmaterial"];
                                                if (dtRawmat.Rows.Count > 0)
                                                {
                                                    for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                                                    {
                                                        string ExchValidFrom = "";
                                                        if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                                        {
                                                            DateTime DtValFrm = DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);
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
                                                        sql += @" CURRENT_TIMESTAMP, '" + Session["userID"].ToString() + @"') ";

                                                        cmd = new SqlCommand(sql, EmetCon, transemet);
                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            #endregion

                                        }
                                    }
                                }
                                else
                                {
                                    DataTable dtget1 = new DataTable();

                                    DataTable dtdate1 = new DataTable();
                                    SqlDataAdapter da1 = new SqlDataAdapter();

                                    string MDMSql = string.Empty;

                                    ReqNum = Session["hdnReqNo"].ToString();
                                    MDMSql = @"select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], TQ.Plant,TQ.Material as [Comp Material],
                                                TQ.MaterialDesc as [Comp Material Desc], TQ.VendorName as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, 
                                                TV.Crcy from " + Convert.ToString(TransDB.ToString()) + @"TQuoteDetails TQ 
                                                inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor 
                                                inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor   Where TQ.RequestNumber='" + ReqNum + "' ";

                                    da1 = new SqlDataAdapter(MDMSql, MDMCon);
                                    da1.Fill(dtget1);

                                    if (dtget1.Rows.Count > 0)
                                    {
                                        Table dtTable1 = (Table)Session["dtTable1"];
                                        if (dtTable1 != null)
                                        {
                                            if (dtTable1.Rows.Count > 0)
                                            {
                                                for (int t = 1; t < dtTable1.Rows.Count; t++)
                                                {
                                                    string Qno = "";
                                                    string CurrentVndCode = "";
                                                    if (t > 1)
                                                    {
                                                        Qno = dtTable1.Rows[t].Cells[4].Text.ToString();
                                                        CurrentVndCode = dtTable1.Rows[t].Cells[(6)].Text.ToString();
                                                    }
                                                    else
                                                    {
                                                        Qno = dtTable1.Rows[t].Cells[(7)].Text.ToString();
                                                        CurrentVndCode = dtTable1.Rows[t].Cells[(6)].Text.ToString();
                                                    }
                                                    DataTable dtget2 = new DataTable();

                                                    DataTable dtdate2 = new DataTable();
                                                    SqlDataAdapter da2 = new SqlDataAdapter();

                                                    string sql = string.Empty;
                                                    //updated by subash
                                                    if (article.Checked == true)
                                                    {
                                                        sql = @" update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo ='" + Qno + "'";
                                                    }
                                                    else
                                                    {
                                                        sql = @" update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus='4',PICApprovalStatus = '4', ManagerApprovalStatus = '4', DIRApprovalStatus = '4' where QuoteNo ='" + Qno + "'";
                                                    }

                                                    cmd = new SqlCommand(sql, EmetCon, transemet);
                                                    cmd.ExecuteNonQuery();

                                                    if (ddlsplproctype.SelectedValue.ToString() == "30")
                                                    {
                                                        if (ChcDisMatCost.Checked == true)
                                                        {
                                                            sql = " update TQuoteDetails SET AcsTabMatCost = 0 where QuoteNo ='" + Qno + "'";
                                                        }
                                                        else
                                                        {
                                                            sql = " update TQuoteDetails SET AcsTabMatCost = 1 where QuoteNo ='" + Qno + "'";
                                                        }
                                                    }

                                                    cmd = new SqlCommand(sql, EmetCon, transemet);
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

                                                                        cmd = new SqlCommand(sql, EmetCon, transemet);
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
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors for this Process group with Material!')", true);
                                    }
                                }
                                #region send emai

                                //subash - email -Begin
                                if (ProcesSendEmail() == false)
                                {
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submit success, failed sending email : " + errMsg + " ");
                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                else
                                {
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("submit success!");
                                    var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                //End by subash
                                // Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
                                #endregion send emai
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select the Attachment and upload !')", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please fill the Mnth.Est.Qty & UOM !');", true);
                            btnsave_Click(btnsave, EventArgs.Empty);
                            txtMQty.Focus();
                        }
                        #endregion
                    }
                    else
                    {

                        if (ProcesSendEmail2() == false)
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submit success, failed sending email : " + errMsg + " ");
                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                        else
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("submit success!");
                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                    }
                }

                transemet.Commit();
                transemet.Dispose();
                DeleteNonRequest();
            }
            catch (Exception ex)
            {
                transemet.Rollback();
                transemet.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;

                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(LbMsgErr.Text);
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
                EmetCon.Dispose();
            }
        }

        protected void BtnSubmitProcDuplicateReg_Click(object sender, EventArgs e)
        {
            ProcessDuplicateReqWithOldNExpiredReq();
            if (isDuplicateReqWithOldNExpiredReqExtend == true)
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Extend Old Request Success !');window.location ='Home.aspx';", true);
            }
            else
            {
                btnsave_Click(btnsave, EventArgs.Empty);
            }
        }

        protected void CleanForm()
        {
            try
            {

                this.ddlmatltype.SelectedIndex = 0;
                this.ddlproctype.SelectedIndex = 0;
                this.ddlplantstatus.Items.Clear();
                this.ddlsplproctype.Items.Clear();
                this.ddlpirtype.Items.Clear();
                DdlProduct.SelectedIndex = -1;
                this.txtpartdesc.Text = "";
                this.txtpartdescription.Text = "";
                this.txtPIRDesc.Text = "";
                this.txtunitweight.Text = "";
                this.txtUOM.Text = "";
                this.DdlReason.SelectedIndex = 0;
                this.txtMQty.Text = "";
                this.txtBaseUOM1.Text = "";
                this.ddlprocess.SelectedIndex = 0;
                this.ddlpirjtype.Items.Clear();
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

                            if (cellCtr == dtget.Rows[0].ItemArray.Length - 1)
                            {
                                if (txtPIRDesc.Text.ToString().ToUpper().Contains("SUBCON"))
                                {
                                    tCell.Text = "";
                                }
                            }


                        }
                        rowcount++;

                    }
                    Session["hdnReqNo"] = dtget.Rows[0]["Req No"].ToString();
                    Session["dtTable1"] = Table1;
                    hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();

                }
                else
                {
                    DeleteNonRequest();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('selected vendors does not have valid Data. Please check the Master Data !');CloseLoading();", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
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
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + Label15.Text + "') or (CreateStatus is null and createdby= '" + Label15.Text + "')";
                Label17.Text = str.ToString();
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

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
            Response.Redirect("NewRequest.aspx");
        }

        protected void ddlmatdescriptionC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                txtpartdesc.Text = "";
                txtpartdescription.Text = "";
                //ddlpirtype.SelectedValue = "";
                txtPIRDesc.Text = "";
                txtunitweight.Text = "";
                txtUOM.Text = "";
                txtBaseUOM1.Text = "";
                getMatlClass();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }


        protected void ddlpirjtype_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtjobtypedesc.Text = ddlpirjtype.Text;
            Session["PIRJOBTYPE"] = txtjobtypedesc.Text;
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

                if (RbExternal.Checked == true)
                {
                    this.vendorload(ddlprocess.SelectedValue.ToString());
                }
                else if (RbTeamShimano.Checked == true)
                {
                    GetDdlVendor(ddlprocess.SelectedValue.ToString());
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
                DvCreateRequest.Visible = true;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void RbExternal_CheckedChanged(object sender, EventArgs e)
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
            resetProgress();
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
            resetProgress();
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