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
using System.Text;
using System.Globalization;
using System.Threading;

namespace Material_Evaluation
{
    public partial class Review_req : System.Web.UI.Page
    {
        //public static List<string> metfields;
        //public static Dictionary<string, string> DicQuoteDetails;
        //public string QuoteNo;
        //public static DataTable DtDynamic;   /// Fields from METFields
        //public static DataTable DtMaterial;
        //public static DataTable DtMaterialsDetails;
        //public static DataTable DtDynamicMaterials;

        //public static DataTable DtDynamicProcessFields;
        //public static DataTable DtDynamicProcessCostsDetails;

        //public static DataTable DtDynamicSubMaterialsFields;
        //public static DataTable DtDynamicSubMaterialsDetails;

        //public static DataTable DtDynamicOtherCostsFields;
        //public static DataTable DtDynamicOtherCostsDetails;

        //public static DataTable DtDynamicUnitFields;
        //public static DataTable DtDynamicUnitDetails;

        //private static string ratiochange = "";

        //private static int MtlAddCount = 1;
        //public static int TempPCAddCount = 1;
        //public static int PCAddCount = 1;
        //public static int SMCAddCount = 1;
        //public static int OthersAddCount = 1;
        //public static int UnitAddCount = 1;

        //public string userId;
        //public string UserId;//
        //public string Userid;//
        //public string strplant;
        //public string Plant_;//

        //public static DropDownList ddlProcess;
        //public static DropDownList ddlProcessSubVndorData;

        //string ListColumnUseSubVnder = string.Empty;
        ////public string corg;
        //string userId1;//

        //public static string RequestIncNumber1;
        //public static string SendFilename;

        //email
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public string fname = "";
        public string Source = "";
        public string nameC;//
        public string aemail;//
        public string pemail;//
        public string Uemail;//
        public string body1;//
        
        public static string password;
        public static string domain;
        public static string path;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        
        int ColmNoMat = 1;
        int ColmNoProc = 1;
        int ColmNoSubMat = 1;
        int ColmNoOth = 1;

        string PlantDesc = "";
        string SMNPICSubmDept = "";
        string GA = "";
        string DbMasterName = "";
        string DbTransName = "";
        string MsgErr = "";

        bool isSaveallCostDetails = false;
        bool SaveAsDraftSccs = false;

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string mappeduserid;
                string mappedname;
                if (Session["userID_"] == null || Session["VPlant"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        Session["FlAttachment"] = null;
                        Session["VndMachineAmortize"] = null;
                        Session["IsMacAmorAdded"] = null;

                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_Vendorsubmission";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
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
                            string UserId = Session["userID_"].ToString();

                            TextBox1.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            txtfinal.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


                            if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                            {
                                GeUOMList();
                                UserId = Session["userID_"].ToString();
                                LoadSavings();

                                string userId = Session["userID_"].ToString();                     //subash
                                string sname = Session["UserName"].ToString();
                                nameC = sname;                                                  //subash
                                string srole = Session["userType"].ToString();
                                mappeduserid = Session["mappedVendor"].ToString();
                                mappedname = Session["mappedVname"].ToString();
                                if (Session["VendorType"] != null)
                                {
                                    hdnVendorType.Value = Session["VendorType"].ToString();
                                }
                                else
                                {
                                    hdnVendorType.Value = "External";
                                }

                                string concat = sname + " - " + mappedname;
                                lblUser.Text = sname;
                                lblplant.Text = mappedname;

                                Session["Qno"] = Request.QueryString["Number"];
                                Session["MtlAddCount"] = 1;
                                Session["PCAddCount"] = 1;
                                Session["SMCAddCount"] = 1;
                                Session["OthersAddCount"] = 1;

                                string QuoteNo = Session["Qno"].ToString();
                                lblreqst.Text = ": " + QuoteNo.ToString();

                                if (Isrevision(QuoteNo) == true)
                                {

                                }
                                else {
                                    IsMassrevision(QuoteNo);
                                }
                                GetPROCESGRPSCREENLAYOUT();
                                GetQuoteandAllDetails(QuoteNo);
                                GetDataMachineAmor(QuoteNo);
                                GetSubMatDdl();
                                GetOthCostDdl();
                                //GetReqPlant(QuoteNo.Remove(0, 3).Replace("GP", ""));
                                
                                GetQuoteDetailsbyQuotenumber(QuoteNo);
                                CreateDynamicTablebasedonProcessField();
                                GetQuoteupdated(QuoteNo);
                                Getcreateuser();


                                string struser = (string)HttpContext.Current.Session["userID_"].ToString();

                                GetSHMNPICDetails(struser);
                                // GetData(QuoteNo);
                                GetData(QuoteNo);

                                process();
                                TurnKeyprovit();

                                RetrieveAllCostDetails();
                                if (hdnQuoteNoRef.Value != "") {
                                    GetOldProcData();
                                }
                                GetDataMachineAmorBasedOnMacIdProcCostSeleced();

                                UnitCostDataStore();

                                OthersCostDataStore();
                                subMatCostDataStore();
                                ProcessCostDataStore();
                                MCCostDataStore();
                                RestoreDataTableUnit();
                                Countryoforigin();

                                
                                CekTableproc();
                                SetTitleForm(QuoteNo);

                                if (Session["UnreadAnn"].ToString() != "")
                                {
                                    lbUnreadAnn.Text = Session["UnreadAnn"].ToString() + " Unread Announcement";
                                }
                                else
                                {
                                    LiUnReadAnn.Style.Add("display", "none");
                                    lbUnreadAnn.Text = "";
                                }
                            }
                        }

                        if (Session["VndMachineAmortize"] != null) {
                            Session["OldMachineAmorList"] = Session["VndMachineAmortize"];
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CallMyFunction", "ReturnBaseQtyProcCheckStats();", true);
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

        protected void GeUOMList()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                string Sql = @"select upper(DefValue) as 'uomlist' from DefaultValueMaster where Description='eMET_RM_Mat Cost_UOM' and Plant=@plant and ISNULL(Delflag,0)=0";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(Sql, MDMCon);
                    cmd.Parameters.AddWithValue("@plant", Session["VPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        DataTable dtsplit = new DataTable();
                        dtsplit.Columns.Add("uomlist");
                        if (dt.Rows.Count > 0)
                        {
                            string[] uomlist = dt.Rows[0]["uomlist"].ToString().Split(',');
                            if (uomlist.Length > 1)
                            {
                                dtsplit.Rows.Add("--SELECT UOM--");
                            }
                            foreach (string value in uomlist)
                            {
                                dtsplit.Rows.Add(value);
                            }
                        }
                        else
                        {
                            dtsplit.Rows.Add("UOM NOT EXIST");
                        }
                        Session["uomlist"] = dtsplit;
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
            string DefVal = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select DefValue from DefaultValueMaster where Description = 'Quote Next Rev Date' and  Plant=@Plant and DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
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

        protected void GetPROCESGRPSCREENLAYOUT()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct ProcessGrp,ScreenLayout from TPROCESGRP_SCREENLAYOUT
                            where DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GvProcessGrpVsLayout.DataSource = dt;
                        GvProcessGrpVsLayout.DataBind();
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

        protected void GetSubMatDdl()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct UPPER(SubMaterial) as SubMaterial from TSubMaterial where Plant=@Plant and DelFlag=0 ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["SubMatDdl"] = dt;
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

        protected void GetOthCostDdl()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct upper(OtherCost) as OtherCost from TOtherCost where Plant=@Plant and DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["OthCostDdl"] = dt;
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

        protected void SetTitleForm(string QN)
        {
            try
            {
                string Cek = QN;
                if (QN.Substring(QN.Length - 1, 1) == "D")
                {
                    lbTitle.Text = "Quote Request Without SAP Code : Draft";
                }
                else if (QN.Substring(QN.Length - 2, 2) == "GP")
                {
                    lbTitle.Text = "Quote Request Without SAP Code (GP) : Draft";
                }
                else
                {
                    lbTitle.Text = "Quote Request With SAP Code : Draft";
                }

                if (hdnQuoteNoRef.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + " &nbsp; - Revision ";
                }
                else if (hdnMassRevision.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + "&nbsp;  - Mass Revision";
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetReqPlant(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select CONCAT(A.Plant, ' - ', (select distinct C.Description from " + DbMasterName + @".dbo.TPLANT C  where c.Plant = A.Plant)) as 'PlRequestor'
                            from TPlantReq A 
                            inner join " + DbMasterName + @".dbo.TPLANT B on A.Plant = B.Plant 
                            where A.RequestNumber=@RequestNumber ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string Plreq = "";
                            for (int p = 0; p < dt.Rows.Count; p++)
                            {
                                if (dt.Rows.Count == 1 || p == (dt.Rows.Count - 1))
                                {
                                    Plreq += dt.Rows[p]["PlRequestor"].ToString();
                                }
                                else
                                {
                                    Plreq += dt.Rows[p]["PlRequestor"].ToString() + ",";
                                }
                            }
                            TxtReqPlant.Text = Plreq;
                        }
                        else
                        {
                            TxtReqPlant.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool Isrevision(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                Session["AcsTabMatCost"] = true;
                Session["AcsTabProcCost"] = true;
                Session["AcsTabSubMatCost"] = true;
                Session["AcsTabOthMatCost"] = true;
                Session["IsUseToolAmortize"] = false;
                Session["IsUseMachineAmortize"] = false;

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNoRef,AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost 
                            ,IsUseToolAmortize ,IsUseMachineAmortize
                            from TQuoteDetails where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0 && dt.Rows[0]["QuoteNoRef"].ToString() != "")
                        {
                            Session["AcsTabMatCost"] = (bool)dt.Rows[0]["AcsTabMatCost"];
                            Session["AcsTabProcCost"] = (bool)dt.Rows[0]["AcsTabProcCost"];
                            Session["AcsTabSubMatCost"] = (bool)dt.Rows[0]["AcsTabSubMatCost"];
                            Session["AcsTabOthMatCost"] = (bool)dt.Rows[0]["AcsTabOthMatCost"];

                            Session["IsUseToolAmortize"] = (bool)dt.Rows[0]["IsUseToolAmortize"];
                            Session["IsUseMachineAmortize"] = (bool)dt.Rows[0]["IsUseMachineAmortize"];

                            hdnQuoteNoRef.Value = dt.Rows[0]["QuoteNoRef"].ToString();

                            if ((bool)Session["AcsTabMatCost"] == false)
                            {
                                btnAddColumns.Attributes.Add("style", "display:none;");
                                Button2.Attributes.Add("style", "display:none;");
                                Table1.Enabled = false;
                            }
                            if ((bool)Session["AcsTabProcCost"] == false)
                            {
                                if ((bool)dt.Rows[0]["IsUseMachineAmortize"] == false)
                                {
                                    btnaddProcessCost.Attributes.Add("style", "display:none;");
                                    Button6.Attributes.Add("style", "display:none;");
                                    TablePC.Enabled = false;
                                }
                                else
                                {
                                    btnaddProcessCost.Attributes.Add("style", "display:block;");
                                    Button6.Attributes.Add("style", "display:block;");
                                    TablePC.Enabled = true;
                                }
                            }
                            if ((bool)Session["AcsTabSubMatCost"] == false)
                            {
                                btnAddSubProcessCost.Attributes.Add("style", "display:none;");
                                if ((bool)dt.Rows[0]["IsUseToolAmortize"] == false)
                                {
                                    Button4.Attributes.Add("style", "display:none;");
                                }
                                else
                                {
                                    Button4.Attributes.Add("style", "display:block;");
                                }
                                TableSMC.Enabled = false;
                            }
                            if ((bool)Session["AcsTabOthMatCost"] == false)
                            {
                                btnAddOtherCost.Attributes.Add("style", "display:none;");
                                Button5.Attributes.Add("style", "display:none;");
                                TableOthers.Enabled = false;
                            }
                            Session["IsRevision"] = "1";
                            TableUnit.Enabled = false;
                            return true;
                        }
                        else
                        {
                            return false;
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

        private void GetOldProcData()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                EmetCon.Open();
                MDMCon.Open();

                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                string SubVndCode = string.Empty;

                #region Process Cost
                GetDbMaster();
                string strProcess = "";
                dtget = new DataTable();
                strProcess = "SELECT PCD.[QuoteNo],PCD.[ProcessGroup],PCD.[ProcessGrpCode],PCD.[SubProcess],PCD.[IfTurnkey-VendorName]," +
                             "( CONCAT (RTRIM(PCD.[TurnKeySubVnd]),' - ',(select distinct Description from[" + DbMasterName + "].[dbo].[tVendor_New] where Vendor = PCD.[TurnKeySubVnd])) ) as TurnKeySubVnd," +
                             "PCD.[Machine/Labor],PCD.[Machine],CAST(ROUND(PCD.[StandardRate/HR],7) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(PCD.VendorRate,7) AS DECIMAL(12,2))as 'VendorRate'," +
                             "PCD.[ProcessUOM],PCD.[Baseqty],PCD.[DurationperProcessUOM(Sec)],PCD.[Efficiency/ProcessYield(%)]," +
                             "PCD.[TurnKeyCost],PCD.[TurnKeyProfit],PCD.[ProcessCost/pc],CAST(ROUND(PCD.[TotalProcessesCost/pcs],5) AS DECIMAL(12,5)) as [TotalProcessesCost/pcs],PCD.[RowId]," +
                             "PCD.[UpdatedBy],PCD.[UpdatedOn] FROM  " + TransDB.ToString() + "[TProcessCostDetails] PCD  Where PCD.[QuoteNo] = @QuoteNo order by RowId asc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strProcess, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNoRef.Value);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sbProcess = new StringBuilder();

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtProc = dtget.Rows[i]["ProcessGrpCode"].ToString();
                        var txtsubProc = dtget.Rows[i]["SubProcess"].ToString();
                        var txtturnkey = dtget.Rows[i]["IfTurnkey-VendorName"].ToString();
                        var txtTurnKeySubVnd = dtget.Rows[i]["TurnKeySubVnd"].ToString();
                        if (txtTurnKeySubVnd.Trim() == "-")
                        {
                            txtTurnKeySubVnd = "";
                        }
                        var txtML = dtget.Rows[i]["Machine/Labor"].ToString();
                        var txtMachine = dtget.Rows[i]["Machine"].ToString();

                        var txtstanRate = dtget.Rows[i]["StandardRate/HR"].ToString();
                        var txtvendorRate = dtget.Rows[i]["VendorRate"].ToString();
                        var txtProcUOM = dtget.Rows[i]["ProcessUOM"].ToString();
                        var txtBaseQty = dtget.Rows[i]["Baseqty"].ToString();
                        var txtDPUOM = dtget.Rows[i]["DurationperProcessUOM(Sec)"].ToString();

                        var txtProcYeild = dtget.Rows[i]["Efficiency/ProcessYield(%)"].ToString();
                        var txtTurnKeyCost = dtget.Rows[i]["TurnKeyCost"].ToString();
                        var txtTurnKeyProfit = dtget.Rows[i]["TurnKeyProfit"].ToString();
                        var txtProcCost = dtget.Rows[i]["ProcessCost/pc"].ToString();
                        var txtProcTCost = dtget.Rows[i]["TotalProcessesCost/pcs"].ToString();

                        #region get value for proces uom is struk/min 
                        if (txtProcUOM.ToString().Contains("STROKES/MIN"))
                        {
                            try
                            {
                                string[] ArProcess_Grp_code = txtProc.ToString().Split('-');
                                string Process_Grp_code = ArProcess_Grp_code[0].ToString().Trim();
                                string[] ArMachineType = txtMachine.ToString().Split('-');
                                string MachineType = "";
                                int tonnage = 0;

                                string TypeAndToonage = GetDataMacTypNtoonageRetrive(ArMachineType[0].ToString());
                                string[] ArrTypeAndToonage = TypeAndToonage.Split(',');
                                if (ArrTypeAndToonage.Count() == 2)
                                {
                                    MachineType = ArrTypeAndToonage[0].ToString().Trim();
                                    tonnage = int.Parse(ArrTypeAndToonage[1].ToString());
                                }

                                string strplant = Session["strplant"].ToString();
                                string str1 = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                                             " where Plant=@Plant and Process_Grp_code=@Process_Grp_code " +
                                             " and MachineType=@MachineType and (@tonnage between Tonnage_From and Tonnage_To) ";
                                cmd = new SqlCommand(str1, MDMCon);
                                cmd.Parameters.AddWithValue("@Plant", strplant);
                                cmd.Parameters.AddWithValue("@Process_Grp_code", Process_Grp_code);
                                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                                cmd.Parameters.AddWithValue("@tonnage", tonnage);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    txtProcUOM = txtProcUOM + '-' + reader["Strokes_min"].ToString();
                                }
                                reader.Dispose();
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex);
                            }
                        }
                        #endregion

                        //var txtProc = dtget.Rows[i].ItemArray[2].ToString();
                        //var txtsubProc = dtget.Rows[i].ItemArray[3].ToString();
                        //var txtturnkey = dtget.Rows[i].ItemArray[4].ToString();
                        //var txtML = dtget.Rows[i].ItemArray[5].ToString();
                        //var txtMachine = dtget.Rows[i].ItemArray[6].ToString();

                        //var txtstanRate = dtget.Rows[i].ItemArray[7].ToString();
                        //var txtvendorRate = dtget.Rows[i].ItemArray[8].ToString();
                        //var txtProcUOM = dtget.Rows[i].ItemArray[9].ToString();
                        //var txtBaseQty = dtget.Rows[i].ItemArray[10].ToString();
                        //var txtDPUOM = dtget.Rows[i].ItemArray[11].ToString();

                        //var txtProcYeild = dtget.Rows[i].ItemArray[12].ToString();
                        //var txtProcCost = dtget.Rows[i].ItemArray[13].ToString();
                        //var txtProcTCost = dtget.Rows[i].ItemArray[14].ToString();

                        sbProcess.Append(txtProc + "," + txtsubProc + "," + txtturnkey + "," + txtTurnKeySubVnd + "," + txtML + "," + txtMachine + "," + txtstanRate + "," + txtvendorRate + "," + txtProcUOM + "," + txtBaseQty + "," + txtDPUOM + "," + txtProcYeild + "," + txtTurnKeyCost + "," + txtTurnKeyProfit + "," + txtProcCost + "," + txtProcTCost + ",");
                    }
                    DataTable DtDynamicOtherCostsFields = new DataTable();
                    DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];
                    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();
                    Session["TotColProcOld"] = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;
                }

                #endregion SubMat Cost

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
                MDMCon.Dispose();
            }
        }

        protected void IsMassrevision(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                Session["AcsTabMatCost"] = true;
                Session["AcsTabProcCost"] = true;
                Session["AcsTabSubMatCost"] = true;
                Session["AcsTabOthMatCost"] = true;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost,MassRevQutoteRef from TQuoteDetails where QuoteNo=@QuoteNo and isMassRevisionAll=1 ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Session["AcsTabMatCost"] = (bool)dt.Rows[0]["AcsTabMatCost"];
                            Session["AcsTabProcCost"] = (bool)dt.Rows[0]["AcsTabProcCost"];
                            Session["AcsTabSubMatCost"] = (bool)dt.Rows[0]["AcsTabSubMatCost"];
                            Session["AcsTabOthMatCost"] = (bool)dt.Rows[0]["AcsTabOthMatCost"];

                            if ((bool)Session["AcsTabMatCost"] == false)
                            {
                                btnAddColumns.Attributes.Add("style", "display:none;");
                                Button2.Attributes.Add("style", "display:none;");
                                Table1.Enabled = false;
                            }
                            if ((bool)Session["AcsTabProcCost"] == false)
                            {
                                btnaddProcessCost.Attributes.Add("style", "display:none;");
                                Button6.Attributes.Add("style", "display:none;");
                                TablePC.Enabled = false;
                            }
                            if ((bool)Session["AcsTabSubMatCost"] == false)
                            {
                                btnAddSubProcessCost.Attributes.Add("style", "display:none;");
                                Button4.Attributes.Add("style", "display:none;");
                                TableSMC.Enabled = false;
                            }
                            if ((bool)Session["AcsTabOthMatCost"] == false)
                            {
                                btnAddOtherCost.Attributes.Add("style", "display:none;");
                                Button5.Attributes.Add("style", "display:none;");
                                TableOthers.Enabled = false;
                            }
                            Session["IsMassRevision"] = "1";
                            TableUnit.Enabled = false;

                            string Quoteref = dt.Rows[0]["MassRevQutoteRef"].ToString().Trim();
                            if (Quoteref != "")
                            {
                                hdnQuoteNoRef.Value = Quoteref;
                                LblQuNoRef.Text = ": " + Quoteref;
                            }
                            DvQuoteRef.Visible = true;
                            DvQuoreReqRevice.Visible = true;
                            showDataQuoreReqRevice(QuNo, Quoteref);
                            GetOldQuotePIRMass(QuNo);
                            DvGvPIROldQuoteMass.Visible = true;
                        }
                        else
                        {
                            Session["IsMassRevision"] = "0";
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
        }

        protected void GetDbTrans()
        {
            try
            {
                DbTransName = EMETModule.GetDbTransname() + ".[dbo].";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
                DbTransName = "";
            }
        }

        #region Disabled this function because BOMListWillGetFrom Transaction Table isntead from MDM
        //protected void GetBOMForMassRevision(string Material)
        //{
        //    try
        //    {
        //        GetDbTrans();
        //        OpenSqlConnectionMaster();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            //sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
        //            //        isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
        //            //        tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  
        //            //        from TMATERIAL tm 
        //            //        inner join TBOMLIST TB on tm.Material = TB.Material 
        //            //        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material 
        //            //        inner join tVendor_New tv on tv.customerNo=tc.customer  
        //            //        inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
        //            //        inner join "+ DbTransName +@"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
        //            //        left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
        //            //        where  tm.plant=@Plant  
        //            //        and tvp.plant= @Plant and TB.FGcode=@Material 
        //            //        and tv.Vendor=@Vendor and TM.DELFLAG = 0
        //            //        and tc.ValidFrom <= @ValidTo
        //            //        and tc.ValidTo >= @ValidTo ";

        //            // update for :To get the exchange rate based on effective date when more then one exchange rate is maintained
        //            sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
        //                    isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
        //                    tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
        //                    isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
        //                    tm.BaseUOM as UOM,format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom
        //                    from TMATERIAL tm 
        //                    inner join TBOMLIST TB on tm.Material = TB.Material 
        //                    inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material 
        //                    inner join tVendor_New tv on tv.customerNo=tc.customer  
        //                    inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
        //                    inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
        //                    left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
        //                    and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
        //                    where  tm.plant=@Plant  
        //                    and tvp.plant= @Plant and TB.FGcode=@Material 
        //                    and tv.Vendor=@Vendor and TM.DELFLAG = 0
        //                    and tc.ValidFrom <= @ValidTo
        //                    and tc.ValidTo >= @ValidTo 
        //                    ";

        //            cmd = new SqlCommand(sql, con);
        //            cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
        //            cmd.Parameters.AddWithValue("@Vendor", Session["mappedVendor"].ToString());
        //            cmd.Parameters.AddWithValue("@Material", Material);
        //            DateTime ValidTo = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
        //            cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy/MM/dd"));
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    GvSMNBomEffctvDate.DataSource = dt;
        //                    GvSMNBomEffctvDate.DataBind();
        //                    DvGvPIROldQuoteMass.Visible = true;
        //                }
        //                else
        //                {
        //                    DvGvPIROldQuoteMass.Visible = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //    finally
        //    {
        //        con.Dispose();
        //    }
        //}
        #endregion

        protected void GetOldQuotePIRMass(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select PIRNo,
                            CAST(ROUND(TQ.OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost,
                            CAST(ROUND(TQ.OldTotSubMatCost,5) AS DECIMAL(12,5)) as OldTotSubMatCost,
                            CAST(ROUND(TQ.OldTotProCost,5) AS DECIMAL(12,5)) as OldTotProCost,
                            CAST(ROUND(TQ.OldTotOthCost,5) AS DECIMAL(12,5)) as OldTotOthCost,
                            FORMAT(MassUpdateDate,'dd-MM-yyyy hh:mm:ss') as MassUpdateDate 
                            from TQuoteDetails TQ 
                            where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvQuoteDataPIR.DataSource = dt;
                            GvQuoteDataPIR.DataBind();
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
        }

        string GetPlantPICSMNSubmit(string PlantId)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct CONCAT(Plant, ' - ', Description) as PlantDesc from TPLANT where Plant=@Plant";

                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@Plant", PlantId);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlantDesc = reader["PlantDesc"].ToString();
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
            return PlantDesc;
        }

        string GetDeptPICSMNSubmit(string UseId)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct UseDep as UseDep from usr where UseId=@UseId";

                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UseId", UseId);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SMNPICSubmDept = reader["UseDep"].ToString();
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
            return SMNPICSubmDept;
        }

        string GetGA()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"select GA from TQuoteDetails_D where QuoteNo=@QuoteNo";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@QuoteNo", Session["Qno"].ToString());
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GA = reader["GA"].ToString();
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
            return GA;
        }

        protected void RestoreDataTableUnit()
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "RestoreDataTbaleUnit();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert();", false);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "javascript:alert('test');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ComntByVendorLght();", true);
        }


        protected void ddlpirjtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string corg = ddlpirjtype.SelectedItem.Value;
            Session["corg_"] = ddlpirjtype.SelectedItem.Value;

        }

        protected void Countryoforigin()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select countrycode,CountryDescription,Currency from TCountrycode where active=1 order by CountryDescription";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    ddlpirjtype.DataSource = Result;
                    ddlpirjtype.DataTextField = "CountryDescription";
                    ddlpirjtype.DataValueField = "countrycode";
                    ddlpirjtype.DataBind();
                    ddlpirjtype.Items.Insert(0, new ListItem("-- select Country Of Origin --", String.Empty));
                }
                else
                {
                    //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
                }

                str = @"select TQ.CountryOrg, TC.CountryDescription from " + TransDB.ToString() + @"TQuoteDetails_d TQ INNER JOIN TCountrycode TC ON TC.CountryCode=TQ.CountryOrg 
                    WHERE QuoteNo = @QuoteNo ";
                Result = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result);
                }
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string corg = Result.Rows[0]["CountryOrg"].ToString();
                    Session["corg_"] = Result.Rows[0]["CountryOrg"].ToString();
                    //ddlpirjtype.SelectedItem.Text = Result.Rows[0]["CountryDescription"].ToString();
                    ddlpirjtype.SelectedValue = Session["corg_"].ToString();
                }
                else
                {
                    //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
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

        protected void GetDataPrcGroupVsStukMin()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                string ColumTblProcNo = hdnColumTblProcNo.Value;

                if (hdnProcGroup.Value.Trim() != "" && hdnMachineType.Value.Trim() != "" && hdnTonnage.Value.Trim() != "")
                {
                    MDMCon.Open();
                    DataTable DtDataPrcGroupVsStukMin = new DataTable();
                    string strplant = Session["strplant"].ToString();
                    string str = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                                 " where Plant=@Plant and Process_Grp_code=@Process_Grp_code " +
                                 " and MachineType=@MachineType and (@Tonnage between Tonnage_From and Tonnage_To) and DELFLAG = 0 ";
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd = new SqlCommand(str, MDMCon);
                        cmd.Parameters.AddWithValue("@Plant", strplant);
                        cmd.Parameters.AddWithValue("@Process_Grp_code", hdnProcGroup.Value.Trim());
                        cmd.Parameters.AddWithValue("@MachineType", hdnMachineType.Value.Trim());
                        cmd.Parameters.AddWithValue("@Tonnage", hdnTonnage.Value.Trim());
                        sda.SelectCommand = cmd;
                        sda.Fill(DtDataPrcGroupVsStukMin);
                    }

                    if (DtDataPrcGroupVsStukMin.Rows.Count > 0)
                    {
                        string efficiency = DtDataPrcGroupVsStukMin.Rows[0]["Efficiency"].ToString();
                        string Stk_Min = DtDataPrcGroupVsStukMin.Rows[0]["Strokes_min"].ToString();
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "PrcGrpVsStokes_Min(" + ColumTblProcNo + "," + Stk_Min + "," + efficiency + ");", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "NoProcessgrpVsProcUom(" + ColumTblProcNo + ");", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "NoProcessgrpVsProcUom(" + ColumTblProcNo + ");", true);
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

        protected string GetDataMacTypNtoonageRetrive(string MachineID)
        {
            string TypeAndToonage = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                string strplant = Session["strplant"].ToString();
                string str = " select MachineType,Tonnage from TVENDORMACHNLIST where Plant=@Plant and MachineID= @MachineID and DELFLAG = 0  ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@MachineID", MachineID);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataMacVnd);
                }

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    TypeAndToonage = DtDataMacVnd.Rows[0]["MachineType"].ToString() + "," + DtDataMacVnd.Rows[0]["Tonnage"].ToString();
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
            return TypeAndToonage;
        }

        protected void GetDataMacTypNtoonage()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                string strplant = Session["strplant"].ToString();
                string str = @" select MachineType,Tonnage from TVENDORMACHNLIST where Plant=@Plant and MachineID= @MachineID and DELFLAG = 0  ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@MachineID", hdnMachineId.Value);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataMacVnd);
                }

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    hdnMachineType.Value = DtDataMacVnd.Rows[0]["MachineType"].ToString();
                    hdnTonnage.Value = DtDataMacVnd.Rows[0]["Tonnage"].ToString();
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

        protected void GetDataSubProcesGroup()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataSubProcGroup = new DataTable();

                string str = @" Select SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= @ProcessGrpCode and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", hdnProcGroup.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataSubProcGroup);
                }
                //grdMachinelisthidden = null;
                grdSubProcessGrphidden.DataSource = DtDataSubProcGroup;
                grdSubProcessGrphidden.DataBind();
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

        protected void GetDataSubProcesGroup2(string ProcGroup)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataSubProcGroup = new DataTable();

                string str = @" Select SubProcessName from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= @ProcessGrpCode and DELFLAG = 0 order by SubProcessName ASC ";

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", ProcGroup);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataSubProcGroup);
                }
                Session["SubProcess"] = DtDataSubProcGroup;
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

        protected void GetDataVendRate(string ProcGroup, string MachineId)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataStdrRate = new DataTable();
                string strplant = Session["strplant"].ToString();
                string str = @" select  TVM.MachineID,CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,
                                TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = @VendorCode and Plant = @Plant and ProcessGrp= @ProcessGrp and  TVM.MachineID=@MachineID and TVM.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@ProcessGrp", ProcGroup);
                    cmd.Parameters.AddWithValue("@MachineID", MachineId);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataStdrRate);
                }
                if (DtDataStdrRate.Rows.Count > 0)
                {
                    hdnStdrRate.Value = DtDataStdrRate.Rows[0]["SMNStdrateHr"].ToString();
                    hdnFollowStdRate.Value = DtDataStdrRate.Rows[0]["FollowStdRate"].ToString();
                }
                else
                {
                    hdnStdrRate.Value = "";
                    hdnFollowStdRate.Value = "";
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

        protected void GetDataProcesUOM()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataProcesUOM = new DataTable();

                string str = @" Select SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= @ProcessGrpCode and SubProcessName = @SubProcessName and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", hdnProcGroup.Value.ToString());
                    cmd.Parameters.AddWithValue("@SubProcessName", hdnSubProcGroup.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataProcesUOM);
                }
                if (DtDataProcesUOM.Rows.Count > 0)
                {
                    hdnProcUOM.Value = DtDataProcesUOM.Rows[0]["ProcessUomDescription"].ToString();
                    //UphdnProcUOM.Update();
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

        string getMachineID()
        {
            string MachineId = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                string strplant = Session["strplant"].ToString();
                string str = @" select MachineID from TVENDORMACHNLIST where Plant=@Plant and MachineID = @MachineID and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@MachineID", hdnMachineId.Value);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataMacVnd);
                }
                if (DtDataMacVnd.Rows.Count > 0)
                {
                    MachineId = DtDataMacVnd.Rows[0]["MachineID"].ToString();
                }
                MDMCon.Dispose();
            }
            catch (Exception ex)
            {
                MachineId = "";
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
            return MachineId;
        }

        protected void GetDataDataVndmachine()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();

                string UserId = Session["userID_"].ToString();
                string strplant = Session["strplant"].ToString();
                string str = @" select TVM.MachineID as MacId, CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as Machine, 
                                CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,
                                TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp 
                                from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = @VendorCode and Plant = @Plant and ProcessGrp= @ProcessGrp and TVM.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@ProcessGrp", hdnProcGroup.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataMacVnd);
                }
                grdMachinelisthidden.DataSource = DtDataMacVnd;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;
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

        protected void GetDataVndmachine2(string ProcGroup)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                string strplant = Session["strplant"].ToString();
                string str = @" select CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as MachineID 
                                ,TVM.MachineID as 'MacId'
                                from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = @VendorCode and Plant =@Plant and ProcessGrp= @ProcessGrp and TVM.DELFLAG = 0";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@ProcessGrp", ProcGroup);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtDataMacVnd);
                }
                Session["SubVndMachineByProcGroup"] = DtDataMacVnd;

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

                        Session["USERIdMail"] = dr.GetString(0);
                        //Session["Userid"] = dr.GetString(0);

                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
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

        private void GetQuoteDetailsbyQuotenumber(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbTrans();
                EmetCon.Open();
                DataTable dtdate = new DataTable();
                string str = "select * from " + TransDB.ToString() + "TQuoteDetails where QuoteNo=@QuoteNo ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtdate);
                }
                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();
                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();
                    string VendorCode = dtdate.Rows[0].ItemArray[14].ToString();
                    string ProcessGrp = dtdate.Rows[0].ItemArray[13].ToString();

                    string PartUnit = dtdate.Rows[0].ItemArray[18].ToString();

                    Session["strplant"] = plant;
                    GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass, QuoteNo);
                    GetProcessDetailsbyQuoteDetails(ProcessGrp);
                    GetProcessDetailsbyQuoteDetailsWithNoGroup();
                }

                using (SqlConnection con = new SqlConnection(EMETModule.GenEMETConnString()))
                {

                    //start
                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 4;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = QuoteNo;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {
                                string vendorC = pir.Rows[0]["vendor"].ToString();
                                Session["vendorC"] = pir.Rows[0]["vendor"].ToString();
                                lblVName.Text = vendorC + "-" + pir.Rows[0]["description"].ToString();
                                lblCurrency.Text = ": " + pir.Rows[0]["crcy"].ToString();
                                lblCity.Text = ": " + pir.Rows[0]["cty"].ToString();
                                lblcry.Text = "(" + pir.Rows[0]["crcy"].ToString() + ")";
                            }
                        }
                    }
                    //End
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

        #region Get BOMListRawMaterial Based On Effective Date
        protected void GetMaterialDetailsbyQuoteDetails(string MaterialNo, string Product, string Plant, string MtlClass, string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select RawMaterialCode as [material],RawMaterialDesc as [MaterialDesc],AmtSCur,SellingCrcy,
                    AmtVCur,VendorCrcy,
                    Unit,uom,FORMAT(ValidFrom,'dd-MM-yyyy') as ValidFrom, FORMAT(ValidTo,'dd-MM-yyyy') as ValidTo ,
                    cast(ExchRate as numeric(10,4)) as ExchRate,
                    FORMAT(ExchValidFrom,'dd-MM-yyyy') as ExchValFrom
                    from TSMMBOM_RAWMATCost_EffDate
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvSMNBomEffctvDate.DataSource = dt;
                            GvSMNBomEffctvDate.DataBind();
                            Session["DtMaterialsDetails"] = dt;
                        }
                        else
                        {
                            GvSMNBomEffctvDate.DataSource = null;
                            GvSMNBomEffctvDate.DataBind();
                            Session["DtMaterialsDetails"] = new DataTable();
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
        }
        #endregion

        #region Get BOMListRawMaterial Based On Effective Date
        protected void GetBOMRawmaterialBefEffdate(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select RawMaterialCode as [material],RawMaterialDesc as [MaterialDesc],AmtSCur,SellingCrcy,
                    AmtVCur,VendorCrcy,
                    Unit,uom,FORMAT(ValidFrom,'dd-MM-yyyy') as ValidFrom, FORMAT(ValidTo,'dd-MM-yyyy') as ValidTo ,
                    cast(ExchRate as numeric(10,4)) as ExchRate,
                    FORMAT(ExchValidFrom,'dd-MM-yyyy') as ExchValFrom
                    from TSMNBOM_RAWMATCost
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GVBomListBefEffdate.DataSource = dt;
                            GVBomListBefEffdate.DataBind();
                        }
                        else
                        {
                            GVBomListBefEffdate.DataSource = null;
                            GVBomListBefEffdate.DataBind();
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
        }
        #endregion


        private void GetQuoteandAllDetails(string QuoteNo)
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (QuoteNo.Substring(QuoteNo.Length - 1, 1) == "D" || QuoteNo.Substring(QuoteNo.Length - 2, 2) == "GP")
                {
                    TextBox1.Enabled = true;
                }
                DataTable dtdate = new DataTable();
                string str = @"select top 1 A.RequestNumber,RequestDate,A.QuoteNo,Plant,MaterialType,PlantStatus
                                , SAPProcType, SAPSpProcType, Product, MaterialClass, A.Material, MaterialDesc, PIRType
                                , ProcessGroup, A.VendorCode1, VendorName, PIRJobType, Remarks, NetUnit
                                , DrawingNo, QuoteResponseDueDate,format( A.EffectiveDate, 'dd-MM-yyyy') as  EffectiveDate, format( A.DueOn, 'dd-MM-yyyy') as DueOn
                                , CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost
                                , CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost
                                , CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost
                                , CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost
                                , CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost
                                , CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice
                                , Profit, Discount, VendorPIC, VendorPICEmail
                                , ShimanoPIC, ShimanoPICEmail, CreateStatus, ApprovalStatus
                                , PICApprovalStatus, PICReason, ManagerApprovalStatus, ManagerReason
                                , DIRApprovalStatus, DIRReason, PlatingType, UpdatedBy
                                , UpdatedOn, A.CreatedBy, A.CreatedOn, PIRStatus, PIRNumber, PIRCreatedDate
                                , BaseUOM, CommentByVendor, CountryOrg, MQty, ERemarks, PerNetUnit
                                , UOM, ActualNU, ApprovalDate
                                , GA, QuoteNoRef, AcsTabMatCost, AcsTabProcCost, AcsTabSubMatCost
                                , AcsTabOthMatCost, AprRejBy, AprRejDate, isUseSAPCode, FADate, FAQty, DelDate, DelQty
                                , Incoterm, PckReqrmnt, OthReqrmnt, ReqPlant, ManagerRemark, DIRRemark, AprRejByMng, AprRejDateMng
                                , VndAttchmnt, isMassRevision, PIRNo, 
                                CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost, 
                                CAST(ROUND(OldTotSubMatCost,5) AS DECIMAL(12,5)) as OldTotSubMatCost, 
                                CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) as OldTotProCost, 
                                CAST(ROUND(OldTotOthCost,5) AS DECIMAL(12,5)) as OldTotOthCost, 
                                MassUpdateDate, FORMAT(EmpSubmitionOn,'dd-MM-yyyy') as EmpSubmitionOn
                                ,(select distinct UseNam from " + DbMasterName + @".[dbo].Usr where UseID = EmpSubmitionBy ) as EmpSubmitionBy,
                                format(B.NewEffectiveDate, 'dd-MM-yyyy') as NewEffectiveDate,format(B.NewDueOn, 'dd-MM-yyyy') as NewDueOn,
                                A.IMRecycleRatio,A.SMNPicDept,A.IsUseToolAmortize,A.IsUseMachineAmortize,A.NetProfDisc,A.MassRevQutoteRef,A.isMassRevisionAll
                                from TQuoteDetails_D A 
                                left join TMngEffDateChgLog B on A.QuoteNo = B.QuoteNo 
                                where A.QuoteNo=@QuoteNo and CreateStatus ='Article' 
                                order by B.CreatedOn desc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtdate);
                }

                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();

                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MaterialDesc = dtdate.Rows[0].ItemArray[11].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();
                    TxtPlant.Text = GetPlantPICSMNSubmit(plant);
                    //TxtDepartment.Text = GetDeptPICSMNSubmit(dtdate.Rows[0].ItemArray[46].ToString());
                    TxtDepartment.Text = dtdate.Rows[0]["SMNPicDept"].ToString();
                    Label11.Text = dtdate.Rows[0].ItemArray[44].ToString();
                    Label15.Text = dtdate.Rows[0].ItemArray[45].ToString();
                    if (dtdate.Rows[0]["UpdatedOn"].ToString() != "")
                    {
                        DateTime dtEfective1 = DateTime.Parse(dtdate.Rows[0]["UpdatedOn"].ToString());
                        Label15.Text = dtEfective1.ToString("dd-MM-yyyy HH:mm:ss");
                    }
                    else
                    {
                        Label15.Text = "";
                    }
                    lblcreateuser.Text = dtdate.Rows[0].ItemArray[46].ToString();
                    string PartunitTxt = dtdate.Rows[0].ItemArray[18].ToString();

                    string baseUOm = dtdate.Rows[0].ItemArray[51].ToString();
                    txtBaseUOM.Text = baseUOm.ToString();
                    txtBaseUOM1.Text = baseUOm.ToString();
                    txtMQty.Text = dtdate.Rows[0].ItemArray[54].ToString();
                    if (dtdate.Rows[0]["PICReason"].ToString() != "")
                    {
                        txtRem.Text = dtdate.Rows[0]["PICReason"].ToString();
                    }
                    else
                    {
                        txtRem.Text = dtdate.Rows[0]["ERemarks"].ToString();
                    }


                    txtunitweight.Text = dtdate.Rows[0].ItemArray[58].ToString();
                    txtUOM.Text = dtdate.Rows[0].ItemArray[57].ToString();

                    DateTime dt = DateTime.Parse(dtdate.Rows[0].ItemArray[20].ToString());

                    txtquotationDueDate.Text = dt.ToString("dd-MM-yyyy");

                    txtdrawng.Text = dtdate.Rows[0].ItemArray[19].ToString();

                    txtprod.Text = dtdate.Rows[0].ItemArray[8].ToString();
                    txtpartdesc.Text = Material + " - " + MaterialDesc;
                    txtSAPJobType.Text = dtdate.Rows[0].ItemArray[16].ToString();
                    txtPIRtype.Text = dtdate.Rows[0].ItemArray[12].ToString();
                    txtprocs.Text = dtdate.Rows[0].ItemArray[13].ToString();

                    txtPartUnit.Value = PartunitTxt.ToString();
                    if (dtdate.Rows[0].ItemArray[21].ToString() != null && dtdate.Rows[0].ItemArray[21].ToString() != "")
                    {
                        //DateTime dtEfective = DateTime.Parse(dtdate.Rows[0].ItemArray[21].ToString());
                        //TextBox1.Text = dtEfective.ToString("dd-MM-yyyy");
                        TextBox1.Text = dtdate.Rows[0]["EffectiveDate"].ToString();
                    }
                    TextBox1.Enabled = false;
                    if (dtdate.Rows[0].ItemArray[22].ToString() != null && dtdate.Rows[0].ItemArray[22].ToString() != "")
                    {
                        //DateTime dtDueDate = DateTime.Parse(dtdate.Rows[0].ItemArray[22].ToString());
                        //txtfinal.Text = dtDueDate.ToString("dd-MM-yyyy");
                        txtfinal.Text = dtdate.Rows[0]["DueOn"].ToString();
                    }

                    if (dtdate.Rows[0]["FADate"].ToString() != "")
                    {
                        DateTime FADate = DateTime.Parse(dtdate.Rows[0]["FADate"].ToString());
                        TxtFADate.Text = FADate.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        TxtFADate.Text = "";
                    }
                    if (dtdate.Rows[0]["DelDate"].ToString() != "")
                    {
                        DateTime DelDate = DateTime.Parse(dtdate.Rows[0]["DelDate"].ToString());
                        TxtDelDate.Text = DelDate.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        TxtDelDate.Text = "";
                    }

                    if (dtdate.Rows[0]["RequestDate"].ToString() != "")
                    {
                        DateTime RD = DateTime.Parse(dtdate.Rows[0]["RequestDate"].ToString());
                        TxtRequestDate.Text = RD.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        TxtRequestDate.Text = "";
                    }
                    TxtPlatingType.Text = dtdate.Rows[0]["PlatingType"].ToString();
                    TxtFAQty.Text = dtdate.Rows[0]["FAQty"].ToString();
                    TxtDelQty.Text = dtdate.Rows[0]["DelQty"].ToString();
                    TxtIncoterms.Text = dtdate.Rows[0]["Incoterm"].ToString();
                    TxtPckRequirement.Text = dtdate.Rows[0]["PckReqrmnt"].ToString();
                    TxtOthRequirement.Text = dtdate.Rows[0]["OthReqrmnt"].ToString();
                    //TxtPlantRequestor.Text = dtdate.Rows[0]["ReqPlant"].ToString();

                    string a = dtdate.Rows[0]["QuoteNo"].ToString().Substring(dtdate.Rows[0]["QuoteNo"].ToString().Length - 2);
                    if (dtdate.Rows[0]["QuoteNo"].ToString().Substring(dtdate.Rows[0]["QuoteNo"].ToString().Length - 2) == "GP")
                    {
                        GetReqPlant(QuoteNo.Remove(0, 3).Replace("GP", ""));
                        DvWhitoutCodeGpField.Style.Add("display", "block");
                        DvReqPlant.Style.Add("display", "block");
                        DvProduct.Style.Add("display", "none");
                        DvSAPPIR.Style.Add("display", "none");
                        DvPlatingType.Visible = false;
                    }
                    else
                    {
                        DvWhitoutCodeGpField.Style.Add("display", "none");
                        DvReqPlant.Style.Add("display", "none");
                        DvProduct.Style.Add("display", "block");
                        DvSAPPIR.Style.Add("display", "block");
                    }
                    
                    string QN = dtdate.Rows[0]["QuoteNo"].ToString();
                    if (dtdate.Rows[0]["VndAttchmnt"].ToString() == "")
                    {
                        LbFlName.Text = "No File";
                        TxtLbFlName.Text = "No File";
                        LbFlNameOri.Text = "No File";
                        LbOldFlName.Text = "No File";
                        TxtOriginalFilePath.Text = "";
                        DvOldFile.Attributes.Add("style", "display:none");
                        DvNewFile.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                        LbFlName.Text = dtdate.Rows[0]["VndAttchmnt"].ToString().Replace(QN + "-", "");
                        TxtLbFlName.Text = dtdate.Rows[0]["VndAttchmnt"].ToString().Replace(QN + "-", "");
                        LbFlNameOri.Text = dtdate.Rows[0]["VndAttchmnt"].ToString();
                        LbOldFlName.Text = dtdate.Rows[0]["VndAttchmnt"].ToString().Replace(QN + "-", "");

                        string folderPath = Server.MapPath("~/FileVendorAttachmant/"+ dtdate.Rows[0]["VndAttchmnt"].ToString());
                        if (!File.Exists(folderPath))
                        {
                            LbFlName.Text = "No File";
                            TxtLbFlName.Text = "No File";
                            LbFlNameOri.Text = "No File";
                            LbOldFlName.Text = "No File";

                            TxtOriginalFilePath.Text = "";
                            DvOldFile.Attributes.Add("style", "display:none");
                            DvNewFile.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            TxtOriginalFilePath.Text = folderPath;
                            string MainUrl = Request.Url.AbsoluteUri.ToString().Replace("Review_req.aspx?Number="+ QuoteNo +"","");
                            myOldFilelink.HRef = MainUrl + "/FileVendorAttachmant/" + dtdate.Rows[0]["VndAttchmnt"].ToString();
                            DvOldFile.Attributes.Add("style", "display:block");
                            DvNewFile.Attributes.Add("style", "display:none");
                        }
                    }

                    hdnIsSAPCode.Value = dtdate.Rows[0]["isUseSAPCode"].ToString();
                    if (dtdate.Rows[0]["QuoteNoRef"].ToString() == "" && dtdate.Rows[0]["MassRevQutoteRef"].ToString() == "")
                    {
                        DvQuoteRef.Visible = false;
                        if (dtdate.Rows[0]["isMassRevisionAll"].ToString() == "1" || dtdate.Rows[0]["isMassRevisionAll"].ToString().ToUpper() == "TRUE")
                        {
                            DvQuoreReqRevice.Visible = true;
                        }
                        else {
                            DvQuoreReqRevice.Visible = false;
                        }
                    }
                    else if (dtdate.Rows[0]["QuoteNoRef"].ToString() != "")
                    {
                        DvQuoteRef.Visible = true;
                        DvQuoreReqRevice.Visible = true;
                        LblQuNoRef.Text = ": " + dtdate.Rows[0]["QuoteNoRef"].ToString();
                        showDataQuoreReqRevice(dtdate.Rows[0]["QuoteNo"].ToString(), dtdate.Rows[0]["QuoteNoRef"].ToString());
                        //ddlpirjtype.SelectedValue = dtdate.Rows[0]["CountryOrg"].ToString();
                    }

                    if (dtdate.Rows[0]["isMassRevision"].ToString() == "True")
                    {
                        if (dtdate.Rows[0]["AcsTabMatCost"].ToString() == "True")
                        {
                            hdnMassRevision.Value = "MassMatRevision";
                        }
                        else
                        {
                            hdnMassRevision.Value = "MassMatRevision";
                        }
                        TextBox1.Enabled = false;
                        txtfinal.Enabled = false;
                        //GetBOMForMassRevision(dtdate.Rows[0]["material"].ToString());
                        GetOldQuotePIRMass(dtdate.Rows[0]["QuoteNo"].ToString());
                        DvOldMatCost.Visible = true;
                        txtOldMatcost.Text = "Old Total Material Cost/pc : " + dtdate.Rows[0]["OldTotMatCost"].ToString();
                    }

                    HdnAcsTabMatCost.Value = dtdate.Rows[0]["AcsTabMatCost"].ToString();
                    HdnAcsTabProcCost.Value = dtdate.Rows[0]["AcsTabProcCost"].ToString();
                    HdnAcsTabSubMatCost.Value = dtdate.Rows[0]["AcsTabSubMatCost"].ToString();

                    #region SAPSpProcType Condition
                    HdnSAPSpProcType.Value = dtdate.Rows[0]["SAPSpProcType"].ToString();
                    if (HdnSAPSpProcType.Value == "30")
                    {
                        if (HdnAcsTabMatCost.Value == "False")
                        {
                            btnAddColumns.Attributes.Add("style", "display:none;");
                            Button2.Attributes.Add("style", "display:none;");
                            Table1.Enabled = false;
                            LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> Material Cost calculation is disabled, as it is Subcon process and Material will be supplied by Shimano";
                        }
                        else
                        {
                            LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> If Material Cost have only one column and if, Base Qty/Cavity, is changed, after the Process Cost calculation, Please reselect the Process Group again & recalculate the Process Cost";
                        }
                    }
                    else
                    {
                        LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> If Material Cost have only one column and if, Base Qty/Cavity, is changed, after the Process Cost calculation, Please reselect the Process Group again & recalculate the Process Cost";
                    }
                    #endregion
                    //GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass);
                    GetBOMRawmaterialBefEffdate(dtdate.Rows[0]["QuoteNo"].ToString());
                    if (dtdate.Rows[0]["IMRecycleRatio"].ToString() != "")
                    {
                        DvImRcylRatio.Visible = true;
                        TxtImRcylRatio.Text = dtdate.Rows[0]["IMRecycleRatio"].ToString();
                    }

                    string DefQuoteNextRev = GetQuoteNextRevDate();
                    if (DefQuoteNextRev != "")
                    {
                        DefQuoteNextRev = DefQuoteNextRev.Replace(".", "-").Replace("/", "-");
                        if (ValidateDefQuoteNextRev(DefQuoteNextRev) == true)
                        {
                            if (dtdate.Rows[0]["DueOn"].ToString() != "")
                            {
                                DateTime DateDefQuoteNextRev = new DateTime();
                                DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);

                                DateTime DueOn = new DateTime();
                                DueOn = DateTime.ParseExact(dtdate.Rows[0]["DueOn"].ToString(), "dd-MM-yyyy", null);

                                if (DateDefQuoteNextRev == DueOn)
                                {
                                    txtfinal.Enabled = false;
                                    txtfinal.Text = DefQuoteNextRev;
                                }
                                else if (DueOn > DateTime.Today)
                                {
                                    txtfinal.Enabled = false;
                                    txtfinal.Text = DueOn.ToString("dd-MM-yyyy");
                                }
                                else
                                {
                                    txtfinal.Enabled = true;
                                    txtfinal.Text = DueOn.ToString("dd-MM-yyyy");
                                }
                            }
                        }
                    }

                    Session["IsUseToolAmortize"] = dtdate.Rows[0]["IsUseToolAmortize"].ToString();
                    if (Session["IsUseToolAmortize"].ToString() == "1" || Session["IsUseToolAmortize"].ToString() == "True")
                    {
                        GetDataToolAmor(dtdate.Rows[0]["QuoteNo"].ToString());
                    }
                    else
                    {
                        Session["VndToolAmortize"] = null;
                    }

                    if (dtdate.Rows[0]["IsUseMachineAmortize"].ToString() == "True")
                    {
                        HdnIsUseMachineAmor.Value = "1";
                        //Session["IsMacAmorAdded"] = true;
                    }
                    else
                    {
                        HdnIsUseMachineAmor.Value = "0";
                        Session["IsMacAmorAdded"] = null;
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
        }

        protected void GetDataToolAmor(string QuoteNo, string QuoteNoRef = "", bool Generatevalue = true)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable MyDta = new DataTable();
                if (QuoteNoRef != "")
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                    ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,isnull(CONVERT(DECIMAL(10,2),AmortizeCost),0) as AmortizeCost,AmortizeCurrency
                    ,ExchangeRate,isnull(CONVERT(DECIMAL(10,2),AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                    ,EffectiveFrom,DueDate,CreatedBy,CreatedOn,1 as OldToolAmor
                    from TToolAmortization
                    where QuoteNo = @QuoteNo ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@QuoteNo", QuoteNoRef);
                        sda.SelectCommand = cmd;
                        sda.Fill(MyDta);
                    }
                }

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                    ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,isnull(CONVERT(DECIMAL(10,2),AmortizeCost),0) as AmortizeCost,AmortizeCurrency
                    ,ExchangeRate,isnull(CONVERT(DECIMAL(10,2),AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                    ,EffectiveFrom,DueDate,CreatedBy,CreatedOn,0 as OldToolAmor
                    from TToolAmortization
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (MyDta.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                MyDta.Rows.Add(dr.ItemArray);
                            }
                        }
                        else {
                            MyDta = dt;
                        }
                    }
                }

                if (Generatevalue == true)
                {
                    if (MyDta.Rows.Count > 0)
                    {
                        StringBuilder sbSub = new StringBuilder();
                        decimal TotCostPc = 0;
                        for (int i = 0; i < MyDta.Rows.Count; i++)
                        {
                            var txtsubDesc = MyDta.Rows[i]["Amortize_Tool_ID"].ToString();
                            //var txtsubcost = MyDta.Rows[i]["AmortizeCost"].ToString();
                            var txtsubcost = MyDta.Rows[i]["AmortizeCost_Vend_Curr"].ToString();
                            var txtConsumption = MyDta.Rows[i]["TotalAmortizeQty"].ToString();
                            var txtsubcostPC = MyDta.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString();
                            for (int c = 0; c < MyDta.Rows.Count; c++)
                            {
                                TotCostPc = TotCostPc + decimal.Parse(MyDta.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString());
                            }
                            decimal txtTotalCostPC = TotCostPc;
                            string txtTotalCostPC2 = "";
                            //sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + String.Format("{0:0.00000}", txtTotalCostPC) + ",");
                            sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + txtTotalCostPC2 + ",");
                        }
                        hdnSMCTableValues.Value = sbSub.ToString();
                    }
                }

                Session["VndToolAmortize"] = MyDta;
                HdnIsUseToolAmortize.Value = MyDta.Rows.Count.ToString();
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

        protected void GetDataMachineAmor(string QuoteNo)
        {
            GetDbTrans();
            SqlConnection MdmCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MdmCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct TM.Plant,TM.VendorCode,TM.VendorCurrency,TM.Process_Grp_code,TM.Vend_MachineID,TV.MachineDescription
                    ,isnull(CONVERT(DECIMAL(10,2),tm.AmortizeCost),0) as AmortizeCost,TM.AmortizeCurrency,TM.ExchangeRate,isnull(CONVERT(DECIMAL(10,2),tm.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                    ,TM.AmortizePeriod,TM.AmortizePeriodUOM,TM.TotalAmortizeQty,TM.QtyUOM,TM.AmortizeCost_Pc_Vend_Curr
                    ,format(TM.EffectiveFrom,'yyyy-MM-dd') as EffectiveFrom,format(TM.DueDate,'yyyy-MM-dd') as DueDate
                    from  TMachineAmortization TM
                    join TVENDORMACHNLIST TV on tm.Plant = tv.Plant and tm.VendorCode = tv.VendorCode and tm.Vend_MachineID = tv.MachineID and tm.Process_Grp_code = tv.ProcessGrp and ISNULL(tv.IsAmortize_Machine,0) = 1
                    Where TM.plant = @Plant
                    and (ISNULL(TM.DelFlag,0)=0) 
                    and (format(TM.EffectiveFrom,'yyyy-MM-dd') is null or @EffectiveDate between format(TM.EffectiveFrom,'yyyy-MM-dd') and format(TM.DueDate,'yyyy-MM-dd'))
                    and TM.VendorCode = @VendorCode 
                    --and TM.Process_Grp_code = @ProcGrp 
                    --and TM.Vend_MachineID in (select [Sub-Mat/T&JDescription] from " + DbTransName + @"TSMCCostDetails_D where QuoteNo=@QuoteNo)
                    ORDER BY TM.Vend_MachineID";

                    cmd = new SqlCommand(sql, MdmCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    if (TextBox1.Text != "")
                    {
                        DateTime DtEffectiveDate = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EffectiveDate", "");
                    }
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    //cmd.Parameters.AddWithValue("@ProcGrp", txtprocs.Text);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvMachineAmor.DataSource = dt;
                            GvMachineAmor.DataBind();
                            //Session["VndMachineAmortize"] = dt;
                            //Session["OldMachineAmorList"] = dt;
                        }
                        else
                        {
                            //Session["VndMachineAmortize"] = null;
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
                MdmCon.Dispose();
            }
        }

        protected void GetDataMachineAmorBasedOnMacIdProcCostSeleced()
        {
            SqlConnection MdmCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MdmCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string[] machineListProcCost = HdnMachinListprocCost.Value.Split(',');
                    string MyMachList = "";
                    if (machineListProcCost.Count() > 0)
                    {
                        for (int i = 0; i < machineListProcCost.Count(); i++)
                        {
                            if (i == machineListProcCost.Count() - 1)
                            {
                                MyMachList += "N'" + machineListProcCost[i].ToString() + "'";
                            }
                            else
                            {
                                MyMachList += "N'" + machineListProcCost[i].ToString() + "',";
                            }

                        }
                    }

                    sql = @" select distinct TM.Plant,TM.VendorCode,TM.VendorCurrency,TM.Process_Grp_code,TM.Vend_MachineID,TV.MachineDescription
                    ,isnull(CONVERT(DECIMAL(10,2),tm.AmortizeCost),0) as AmortizeCost,TM.AmortizeCurrency,TM.ExchangeRate,isnull(CONVERT(DECIMAL(10,2),TM.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                    ,TM.AmortizePeriod,TM.AmortizePeriodUOM,TM.TotalAmortizeQty,TM.QtyUOM,TM.AmortizeCost_Pc_Vend_Curr
                    ,format(TM.EffectiveFrom,'yyyy-MM-dd') as EffectiveFrom,format(TM.DueDate,'yyyy-MM-dd') as DueDate,0 as OldMacAmor
                    from  TMachineAmortization TM
                    join TVENDORMACHNLIST TV on tm.Plant = tv.Plant and tm.VendorCode = tv.VendorCode and tm.Vend_MachineID = tv.MachineID and tm.Process_Grp_code = tv.ProcessGrp and ISNULL(tv.IsAmortize_Machine,0) = 1
                    Where TM.plant = @Plant
                    and (ISNULL(TM.DelFlag,0)=0) 
                    and (format(TM.EffectiveFrom,'yyyy-MM-dd') is null or @EffectiveDate between format(TM.EffectiveFrom,'yyyy-MM-dd') and format(TM.DueDate,'yyyy-MM-dd'))
                    and TM.VendorCode = @VendorCode 
                    --and TM.Process_Grp_code = @ProcGrp 
                    and TM.Vend_MachineID in (" + MyMachList + @")
                    ORDER BY TM.Vend_MachineID";

                    cmd = new SqlCommand(sql, MdmCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    if (TextBox1.Text != "")
                    {
                        DateTime DtEffectiveDate = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EffectiveDate", "");
                    }
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());

                    cmd.Parameters.AddWithValue("@MyMachList", MyMachList);
                    //cmd.Parameters.AddWithValue("@ProcGrp", txtprocs.Text);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Session["VndMachineAmortize"] = dt;
                            HdnIsUseMachineAmor.Value = "1";
                        }
                        else
                        {
                            Session["VndMachineAmortize"] = null;
                            HdnIsUseMachineAmor.Value = "0";
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
                MdmCon.Dispose();
            }
        }

        protected void showDataQuoreReqRevice(string newQN, string OldQN)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select iif(@newQN = @OldQN,'',QuoteNo) as QuoteNo,TQ.CountryOrg,
                            (select CONCAT(TQ.VendorCode1 ,' - ',(select distinct TV.Description from " + DbMasterName + @".[dbo].tVendor_New TV where TV.Vendor = TQ.VendorCode1)) ) as 'vendor',
                            TQ.Product,TQ.MaterialClass,TQ.Material,TQ.MaterialDesc,
                            (select CONCAT(TQ.ProcessGroup ,' - ',(select distinct TP.Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code = TQ.ProcessGroup)) ) as 'ProcessGroup',
                            CAST(ROUND(TQ.TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TQ.TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TQ.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TQ.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(TQ.FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            (select TQ2.AcsTabMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabMatCost',
                            (select TQ2.AcsTabProcCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabProcCost',
                            (select TQ2.AcsTabSubMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabSubMatCost',
                            (select TQ2.AcsTabOthMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabOthMatCost',
                            (select ISNULL(tq2.IsUseToolAmortize,0) from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'IsUseToolAmortize',
                            (select isnull(tq2.IsUseMachineAmortize,0) from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'IsUseMachineAmortize',
                            (select case when isnull(tq2.ToolAmorRemark,'') <> '' then TQ2.ToolAmorRemark when isnull(tq2.IsUseToolAmortize,0) = 1 then 'ADD' else 'REMOVE' end  from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'ActionToolAmortize',
                            (select case when isnull(tq2.MachineAmorRemark,'') <> '' then TQ2.MachineAmorRemark when isnull(tq2.IsUseMachineAmortize,0) = 1 then 'ADD' else 'REMOVE' end  from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'ActionMachineAmortize'
                            from TQuoteDetails TQ 
                            where TQ.QuoteNo = @OldQN ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@newQN", newQN);
                    if (OldQN == "")
                    {
                        cmd.Parameters.AddWithValue("@OldQN", newQN);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@OldQN", OldQN);
                    }
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GdvQuoreReqRevice.DataSource = dt;
                        GdvQuoreReqRevice.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            CheckBox ChkMatRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkMatRef") as CheckBox;
                            CheckBox chkProcRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkProcRef") as CheckBox;
                            CheckBox chkSubMatRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkSubMatRef") as CheckBox;
                            CheckBox chkOthRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkOthRef") as CheckBox;
                            CheckBox chkIsUseToolAmortize = GdvQuoreReqRevice.Rows[0].FindControl("chkIsUseToolAmortize") as CheckBox;
                            CheckBox chkIsUseMachineAmortize = GdvQuoreReqRevice.Rows[0].FindControl("chkIsUseMachineAmortize") as CheckBox;
                            if (ChkMatRowRef != null && chkProcRowRef != null && chkSubMatRowRef != null && chkOthRowRef != null && chkIsUseToolAmortize != null && chkIsUseMachineAmortize != null)
                            {
                                if (dt.Rows[0]["AcsTabMatCost"].ToString() == "True")
                                {
                                    ChkMatRowRef.Checked = true;
                                }
                                else
                                {
                                    ChkMatRowRef.Checked = false;
                                }

                                if (dt.Rows[0]["AcsTabProcCost"].ToString() == "True")
                                {
                                    chkProcRowRef.Checked = true;
                                }
                                else
                                {
                                    chkProcRowRef.Checked = false;
                                }

                                if (dt.Rows[0]["AcsTabSubMatCost"].ToString() == "True")
                                {
                                    chkSubMatRowRef.Checked = true;
                                }
                                else
                                {
                                    chkSubMatRowRef.Checked = false;
                                }

                                if (dt.Rows[0]["AcsTabOthMatCost"].ToString() == "True")
                                {
                                    chkOthRowRef.Checked = true;
                                }
                                else
                                {
                                    chkOthRowRef.Checked = false;
                                }

                                if (dt.Rows[0]["IsUseToolAmortize"].ToString() == "True")
                                {
                                    chkIsUseToolAmortize.Checked = true;
                                }
                                else
                                {
                                    chkIsUseToolAmortize.Checked = false;
                                }

                                if (dt.Rows[0]["IsUseMachineAmortize"].ToString() == "True")
                                {
                                    chkIsUseMachineAmortize.Checked = true;
                                }
                                else
                                {
                                    chkIsUseMachineAmortize.Checked = false;
                                }
                            }
                        }
                    }
                    //UpdatePanel18.Update();
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

        private void Getcreateuser()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                string str = "select UseEmail from usr where UseID = @UseID and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@UseID", lblcreateuser.Text);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtdate);
                }

                if (dtdate.Rows.Count > 0)
                {
                    Session["Createuser"] = dtdate.Rows[0].ItemArray[0].ToString();
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

        private void GetQuoteupdated(string QuoteNo)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                string str = "select useid,usenam from usr where useid='" + Label11.Text + "' and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@useid", Label11.Text);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtdate);
                }
                if (dtdate.Rows.Count > 0)
                {

                    Label14.Text = ": " + dtdate.Rows[0].ItemArray[0].ToString() + " - " + dtdate.Rows[0].ItemArray[1].ToString();

                }

                else
                {
                    Label14.Text = ": " + "Waiting for Vendor Submit";
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

        //Used this method by Raja
        public void GetMETfields(string ProcessGrpCode, string FieldGroup)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                #region access layout from New table
                MDMCon.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select f.FieldName from tMETFieldsConditionNew C
                            inner join tMETFileds F on c.FieldId = f.FieldId and ISNULL(f.DelFlag,0)=0
                            inner join TPROCESGRP_SCREENLAYOUT P on c.LayoutID = p.ScreenLayout  and ISNULL(p.DelFlag,0)=0
                            where p.ProcessGrp=@ProcessGrpCode and f.FieldGroup=@FieldGroup and isnull(c.delflag,0)=0
                            group by
                            c.LayoutID,c.FieldId,f.FieldName ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", ProcessGrpCode);
                    cmd.Parameters.AddWithValue("@FieldGroup", FieldGroup);
                    sda.SelectCommand = cmd;
                    using (DataTable Mydt = new DataTable())
                    {
                        sda.Fill(Mydt);
                        dt = Mydt;
                    }
                }

                if (FieldGroup == "MC")
                {
                    if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataTable dtn = new DataTable();
                            dtn.Columns.Add("FieldName", typeof(String));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow selectedRow = dt.Rows[i];

                                if (dt.Rows[i]["FieldName"].ToString() == "~Runner Weight/shot (g)")
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        if (dt.Rows[j]["FieldName"].ToString() == "Cavity")
                                        {
                                            DataRow RCavity = dtn.NewRow();
                                            RCavity["FieldName"] = "Cavity";
                                            dtn.Rows.InsertAt(RCavity, i);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Runner Weight/shot (g)")
                                        {
                                            DataRow newRow0 = dtn.NewRow();
                                            newRow0["FieldName"] = "~Runner Weight/shot (g)";
                                            dtn.Rows.InsertAt(newRow0, i + 1);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Runner Ratio/pcs (%)")
                                        {
                                            DataRow newRow1 = dtn.NewRow();
                                            newRow1["FieldName"] = "~Runner Ratio/pcs (%)";
                                            dtn.Rows.InsertAt(newRow1, i + 2);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Recycle Material Ratio (%)")
                                        {
                                            DataRow newRow2 = dtn.NewRow();
                                            newRow2["FieldName"] = "~Recycle Material Ratio (%)";
                                            dtn.Rows.InsertAt(newRow2, i + 3);
                                        }
                                    }
                                }
                                else if (dt.Rows[i][0].ToString() == "Cavity" || dt.Rows[i][0].ToString() == "~Recycle Material Ratio (%)" ||
                                    dt.Rows[i][0].ToString() == "~Runner Ratio/pcs (%)") { }
                                else
                                {
                                    DataRow newRow = dtn.NewRow();
                                    newRow.ItemArray = selectedRow.ItemArray; // copy data
                                    dtn.Rows.InsertAt(newRow, i);
                                }
                            }
                            dt = dtn;
                        }
                    }
                    Session["DtDynamic"] = dt;
                }
                else if (FieldGroup == "PC")
                {
                    Session["DtDynamicProcessFields"] = dt;
                }
                else if (FieldGroup == "SMC")
                {
                    Session["DtDynamicSubMaterialsFields"] = dt;
                }
                else if (FieldGroup == "Unit")
                {
                    Session["DtDynamicUnitFields"] = dt;
                }
                else if (FieldGroup == "Others")
                {
                    Session["DtDynamicOtherCostsFields"] = dt;
                }
                #endregion
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

        public void CreateDynamicTablebasedonProcessField()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct FieldGroup from tMETFileds where DELFLAG = 0";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                foreach (DataRow row in dtdate.Rows)
                {
                    GetMETfields(txtprocs.Text, row.ItemArray[0].ToString());
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
        /// Material Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        /// 

        private void CreateDynamicDT(int ColumnType)
        {
            try
            {
                DataTable DtDynamic = new DataTable();
                DtDynamic = (DataTable)Session["DtDynamic"];

                DataTable DtMaterialsDetails = new DataTable();
                DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

                if (ColumnType == 0)
                {
                    #region column 0
                    int rowcount = 0;
                    //creatte firs column
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    Table1.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                            Hearderrow.Cells.Add(tCell1);
                        }
                        else
                        {
                            if (DtMaterialsDetails.Rows.Count > 0)
                            {
                                tCell1.Controls.Add(lb1);
                                Hearderrow.Cells.Add(tCell1);
                                ColmNoMat++;
                            }
                            else
                            {
                                LinkButton BtnDel = new LinkButton();
                                int TtlField = DtDynamic.Rows.Count;
                                BtnDel.ID = "BtnDelete" + ColmNoMat;
                                BtnDel.Text = "Delete";
                                BtnDel.ForeColor = Color.Yellow;
                                BtnDel.OnClientClick = "DelMatDetail('" + ColmNoMat + "','" + TtlField + "'); return false;";

                                tCell1.Controls.Add(BtnDel);
                                Hearderrow.Cells.Add(tCell1);
                                ColmNoMat++;

                                if (Session["AcsTabMatCost"] != null)
                                {
                                    if ((bool)Session["AcsTabMatCost"] == false)
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");
                                    }
                                }

                                #region SAPSpProcType Condition
                                if (HdnSAPSpProcType.Value == "30")
                                {
                                    if (HdnAcsTabMatCost.Value == "False")
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");
                                    }
                                }
                                #endregion
                            }
                        }
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    //Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        Table1.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                {
                                    lb.Text = "Raw Material SAP Code";
                                }
                                else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                {
                                    lb.Text = "Raw Material Description";
                                }
                                else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                {
                                    lb.Text = "Base Qty / Cavity";
                                }
                                else
                                {
                                    lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                }

                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                {
                                    lb.Text = "Material/Melting Loss (%)";
                                }

                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                {
                                    lb.Text = "Raw Material Cost";
                                }

                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                {
                                    lb.Text = "Total Raw Material Cost";
                                }

                                if (hdnLayoutScreen.Value == "Layout7")
                                {
                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                    {
                                        //lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                    {
                                        lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                    }
                                }

                                lb.Width = 240;
                                Table1.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                //txtMaterialSAPCode0
                                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (0);
                                tb.Attributes.Add("autocomplete", "off");
                                //tb.Style.Add("min-width", "100px;");
                                tb.Style.Add("width", "200px;");
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                tb.Attributes.Add("class", "Mytxt");
                                string txtID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (0);

                                #region ExtraTextbox for raw material cost UOM
                                DropDownList ddlRawMaterialCostUOM = new DropDownList();
                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "RawMaterialCost/kg")
                                {
                                    DataTable dtUOMList = (DataTable)Session["uomlist"];
                                    ddlRawMaterialCostUOM.ID = "ddlRawMaterialCostUOM" + (0);
                                    ddlRawMaterialCostUOM.Style.Add("max-width", "100px;");
                                    ddlRawMaterialCostUOM.Attributes.Add("class", "pull-right");

                                    if (Session["uomlist"] != null)
                                    {
                                        ddlRawMaterialCostUOM.DataSource = Session["uomlist"];
                                        ddlRawMaterialCostUOM.DataTextField = "uomlist";
                                        ddlRawMaterialCostUOM.DataValueField = "uomlist";
                                        ddlRawMaterialCostUOM.Attributes.Add("onchange", "SetTotaRawMatCostAndUOM(" + 0 + ")");
                                        ddlRawMaterialCostUOM.DataBind();

                                        if (DtMaterialsDetails.Rows.Count > 0)
                                        {
                                            if (ColumnType <= DtMaterialsDetails.Rows.Count)
                                            {
                                                ddlRawMaterialCostUOM.Attributes.Add("disabled", "disabled");
                                                string ddltemval = DtMaterialsDetails.Rows[0].ItemArray[7].ToString();
                                                var ddlcheck = ddlRawMaterialCostUOM.Items.FindByText(ddltemval);
                                                if (ddlcheck != null)
                                                {
                                                    ddlRawMaterialCostUOM.SelectedValue = ddltemval;
                                                }
                                            }
                                        }

                                        #region retrive data
                                        int Clm = 0;
                                        var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                        if (ArrRwMatUom.Count() >= Clm && ArrRwMatUom.Count() > 1)
                                        {
                                            string ddltemval = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                            var ddlcheck = ddlRawMaterialCostUOM.Items.FindByText(ddltemval);
                                            if (ddlcheck != null)
                                            {
                                                //ddlRawMaterialCostUOM.Items.FindByText(ddltemval).Selected = true;
                                                ddlRawMaterialCostUOM.SelectedValue = ddltemval;
                                            }
                                        }
                                        #endregion retrive data
                                    }
                                    else
                                    {
                                        ddlRawMaterialCostUOM.Items.Insert(0, new ListItem("UOM NOT EXIST", "UOM NOT EXIST"));
                                    }

                                    tCell.Controls.Add(ddlRawMaterialCostUOM);
                                }
                                //TextBox tb2 = new TextBox();
                                //tb2.BorderStyle = BorderStyle.None;
                                //tb2.ID = "txtRawMaterialCostUOM" + (0);
                                //tb2.Attributes.Add("autocomplete", "off");
                                //tb2.Style.Add("max-width", "50px;");
                                //tb2.Attributes.Add("placeholder", "UOM");
                                //tb2.Style.Add("text-transform", "uppercase");
                                //tb2.Attributes.Add("onkeyup", "isincludecomma('" + tb2.ID.ToString() + "');SetTotaRawMatCostAndUOM(" + 0 + ");");
                                //tb2.Attributes.Add("class", "pull-right");
                                //tb2.Style.Add("text-align", "left");

                                TextBox tb3 = new TextBox();
                                tb3.BorderStyle = BorderStyle.None;
                                tb3.ID = "txtTotRawMaterialCostUOM" + (0);
                                tb3.Attributes.Add("autocomplete", "off");
                                tb3.Style.Add("max-width", "50px;");
                                tb3.Attributes.Add("placeholder", "UOM");
                                tb3.Style.Add("text-transform", "uppercase");
                                tb3.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                tb3.Attributes.Add("onkeyup", "isincludecomma('" + tb3.ID.ToString() + "');");
                                tb3.Attributes.Add("class", "pull-right");
                                tb3.Style.Add("text-align", "left");
                                tb3.Attributes.Add("disabled", "disabled");
                                #endregion


                                if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                                else
                                {
                                    tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                    tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                }
                                if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(0,'MATYIELD');");
                                }

                                if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(0,'SCRAPALLOWENCE');");
                                }

                                if (tb.ID.Contains("txtCavity"))
                                {
                                    tb.Attributes.Add("onkeyup", "ReturnBaseQtyProcBaseQtyValue();GetRunnerRatioPrcentagePerPiece();");
                                }

                                if (tb.ID.Contains("txt~RunnerWeight/shot(g)"))
                                {
                                    tb.Attributes.Add("onkeyup", "GetRunnerRatioPrcentagePerPiece();");
                                }

                                if (DtMaterialsDetails.Rows.Count > 0)
                                {
                                    if (ColumnType <= DtMaterialsDetails.Rows.Count)
                                    {
                                        if (tb.ID.Contains("txtMaterialSAPCode"))
                                        {
                                            //tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("txtMaterialDescription"))
                                        {
                                            //tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                        {
                                            //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

                                            //double rawperkg = RawVal / 1000;
                                            //tb.Text = (rawperkg.ToString());
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                            //tb2.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("TotalRawMaterialCost/g"))
                                        {
                                            //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

                                            //double rawperkg = RawVal / 1000;
                                            //tb.Text = (rawperkg.ToString());
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                            tb3.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }

                                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                {
                                    //tb.Attributes.Add("disabled", "disabled");
                                    tb.Text = txtPartUnit.Value;
                                }

                                if (tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("RecycleMaterialRatio(%)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                if (tb.ID.Contains("txtMaterialSAPCode") || tb.ID.Contains("txtMaterialDescription"))
                                {
                                    tb.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tb.Style.Add("text-align", "right");
                                }

                                if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout1")
                                    {
                                        tb.Text = "5";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout3")
                                    {
                                        tb.Text = "10";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tb.Text = "2";
                                    }
                                    else
                                    {
                                        tb.Text = "";
                                    }
                                }

                                if (tb.ID.Contains("txt~MaterialDensity"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tb.Text = "7.86";
                                        //tb.Enabled = false;
                                    }
                                    else
                                    {
                                        tb.Text = "";
                                    }
                                }

                                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") || tb.ID.Contains("txtMaterialScrapWeight(g)"))
                                {

                                    tb.Attributes.Add("disabled", "disabled");
                                    if (tb.ID.Contains("txtTotalRawMaterialCost/g"))
                                    {
                                        tb.Style.Add("width", "150px;");
                                        tCell.Controls.Add(tb);
                                        tCell.Controls.Add(tb3);
                                    }
                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                    }
                                    Table1.Rows[cellCtr].Cells.Add(tCell);

                                }

                                if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                {
                                    tb.Style.Add("width", "100px;");
                                    tCell.Controls.Add(tb);
                                    //tCell.Controls.Add(tb2);
                                }
                                else
                                {
                                    tCell.Controls.Add(tb);
                                }


                                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                {
                                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempMClist.Count; ii++)
                                    {
                                        if (ii == rowcountnew)
                                        {
                                            if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                            {
                                                if (HdnFirsLoad.Value == "1")
                                                {
                                                    if (hdnMassRevision.Value != "")
                                                    {
                                                        tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                    }
                                                    else
                                                    {
                                                        tb.Text = "";
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                }
                                            }
                                            else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                //int Clm = 0;
                                                //var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                                //if (ArrRwMatUom.Count() >= Clm)
                                                //{
                                                //    tb2.Text = ArrRwMatUom[Clm].ToString().Replace("NaN", "");
                                                //}
                                            }
                                            else if (tb.ID.Contains("txtTotalRawMaterialCost/g"))
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                int Clm = 0;
                                                var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                                if (ArrRwMatUom.Count() >= Clm)
                                                {
                                                    string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                                    if (UOM == "KG")
                                                    {
                                                        tb3.Text = "G";
                                                    }
                                                    else
                                                    {
                                                        tb3.Text = UOM;
                                                    }
                                                }
                                                //tb2.Text = tempMClist[ii+1].ToString().Replace("NaN", "");
                                                //ii = ii + 1;
                                            }
                                            else
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                            }
                                            break;
                                        }

                                    }
                                }
                                //tb.Attributes.Add("disabled", "disabled");
                                tRow.Cells.Add(tCell);
                            }

                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["Table"] = Table1;
                    #endregion column 0
                }
                else
                {
                    Table1 = (Table)Session["Table"];

                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {

                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                        var TempMClistNew = tempMClist;


                        TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                if (DtMaterialsDetails.Rows.Count > 0)
                                {
                                    if (ColumnType <= DtMaterialsDetails.Rows.Count)
                                    {
                                        Label lb = new Label();
                                        tCell.Controls.Add(lb);
                                        // lb.Text = "Material Cost";
                                        Table1.Rows[cellCtr].Cells.Add(tCell);
                                        ColmNoMat++;
                                    }
                                    else
                                    {
                                        if (ColmNoMat <= DtMaterialsDetails.Rows.Count)
                                        {
                                            Label lb = new Label();
                                            tCell.Controls.Add(lb);
                                            // lb.Text = "Material Cost";
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                        else
                                        {
                                            LinkButton BtnDel = new LinkButton();
                                            int TtlField = DtDynamic.Rows.Count;
                                            BtnDel.ID = "BtnDelete" + ColmNoMat;
                                            BtnDel.Text = "Delete";
                                            BtnDel.ForeColor = Color.Yellow;
                                            BtnDel.OnClientClick = "DelMatDetail(" + ColmNoMat + "," + TtlField + "); return false;";
                                            tCell.Controls.Add(BtnDel);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);

                                            if (Session["AcsTabMatCost"] != null)
                                            {
                                                if ((bool)Session["AcsTabMatCost"] == false)
                                                {
                                                    BtnDel.Attributes.Add("style", "display:none;");
                                                }
                                            }

                                            #region SAPSpProcType Condition
                                            if (HdnSAPSpProcType.Value == "30")
                                            {
                                                if (HdnAcsTabMatCost.Value == "False")
                                                {
                                                    BtnDel.Attributes.Add("style", "display:none;");
                                                }
                                            }
                                            #endregion
                                        }
                                        ColmNoMat++;
                                    }
                                }
                                else
                                {
                                    LinkButton BtnDel = new LinkButton();
                                    int TtlField = DtDynamic.Rows.Count;
                                    BtnDel.ID = "BtnDelete" + ColmNoMat;
                                    BtnDel.Text = "Delete";
                                    BtnDel.ForeColor = Color.Yellow;
                                    BtnDel.OnClientClick = "DelMatDetail(" + ColmNoMat + "," + TtlField + "); return false;";
                                    tCell.Controls.Add(BtnDel);
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                    ColmNoMat++;

                                    if (Session["AcsTabMatCost"] != null)
                                    {
                                        if ((bool)Session["AcsTabMatCost"] == false)
                                        {
                                            BtnDel.Attributes.Add("style", "display:none;");
                                        }
                                    }

                                    #region SAPSpProcType Condition
                                    if (HdnSAPSpProcType.Value == "30")
                                    {
                                        if (HdnAcsTabMatCost.Value == "False")
                                        {
                                            BtnDel.Attributes.Add("style", "display:none;");
                                        }
                                    }
                                    #endregion
                                }

                                //Label lb = new Label();
                                //tCell.Controls.Add(lb);
                                //// lb.Text = "Material Cost";
                                //Table1.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                //txtMaterialSAPCode1
                                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + ((i));
                                tb.Attributes.Add("autocomplete", "off");
                                //tb.Style.Add("min-width", "150px;");
                                tb.Style.Add("width", "200px;");
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                tb.Attributes.Add("class", "Mytxt");
                                string txtID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + ((i));

                                #region ExtraTextbox for raw material cost UOM
                                DropDownList ddlRawMaterialCostUOM = new DropDownList();
                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "RawMaterialCost/kg")
                                {
                                    DataTable dtUOMList = (DataTable)Session["uomlist"];
                                    ddlRawMaterialCostUOM.ID = "ddlRawMaterialCostUOM" + (i);
                                    ddlRawMaterialCostUOM.Style.Add("max-width", "100px;");
                                    ddlRawMaterialCostUOM.Attributes.Add("class", "pull-right");

                                    if (Session["uomlist"] != null)
                                    {
                                        ddlRawMaterialCostUOM.DataSource = Session["uomlist"];
                                        ddlRawMaterialCostUOM.DataTextField = "uomlist";
                                        ddlRawMaterialCostUOM.DataValueField = "uomlist";
                                        ddlRawMaterialCostUOM.Attributes.Add("onchange", "SetTotaRawMatCostAndUOM(" + i + ")");
                                        ddlRawMaterialCostUOM.DataBind();

                                        if (DtMaterialsDetails.Rows.Count > 0)
                                        {
                                            if (i < DtMaterialsDetails.Rows.Count)
                                            {
                                                ddlRawMaterialCostUOM.Attributes.Add("disabled", "disabled");
                                                string ddltemval = DtMaterialsDetails.Rows[i - 1].ItemArray[7].ToString();
                                                var ddlcheck = ddlRawMaterialCostUOM.Items.FindByText(ddltemval);
                                                if (ddlcheck != null)
                                                {
                                                    ddlRawMaterialCostUOM.SelectedValue = ddltemval;
                                                }
                                            }
                                        }

                                        #region retrive data
                                        int Clm = i;
                                        var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                        if (ArrRwMatUom.Count() >= Clm && ArrRwMatUom.Count() > 1)
                                        {
                                            string ddltemval = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                            var ddlcheck = ddlRawMaterialCostUOM.Items.FindByText(ddltemval);
                                            if (ddlcheck != null)
                                            {
                                                //ddlRawMaterialCostUOM.Items.FindByText(ddltemval).Selected = true;
                                                ddlRawMaterialCostUOM.SelectedValue = ddltemval;
                                            }
                                        }
                                        #endregion retrive data
                                    }
                                    else
                                    {
                                        ddlRawMaterialCostUOM.Items.Insert(0, new ListItem("UOM NOT EXIST", "UOM NOT EXIST"));
                                    }

                                    tCell.Controls.Add(ddlRawMaterialCostUOM);
                                }
                                //TextBox tb2 = new TextBox();
                                //tb2.BorderStyle = BorderStyle.None;
                                //tb2.ID = "txtRawMaterialCostUOM" + (i);
                                //tb2.Attributes.Add("autocomplete", "off");
                                //tb2.Style.Add("max-width", "50px;");
                                //tb2.Attributes.Add("placeholder", "UOM");
                                //tb2.Style.Add("text-transform", "uppercase");
                                //tb2.Attributes.Add("onkeyup", "isincludecomma('" + tb2.ID.ToString() + "');SetTotaRawMatCostAndUOM(" + i + ");");
                                //tb2.Attributes.Add("class", "pull-right");
                                //tb2.Style.Add("text-align", "left");

                                TextBox tb3 = new TextBox();
                                tb3.BorderStyle = BorderStyle.None;
                                tb3.ID = "txtTotRawMaterialCostUOM" + (i);
                                tb3.Attributes.Add("autocomplete", "off");
                                tb3.Style.Add("max-width", "50px;");
                                tb3.Attributes.Add("placeholder", "UOM");
                                tb3.Style.Add("text-transform", "uppercase");
                                tb3.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                tb3.Attributes.Add("onkeyup", "isincludecomma('" + tb3.ID.ToString() + "');");
                                tb3.Attributes.Add("class", "pull-right");
                                tb3.Style.Add("text-align", "left");
                                tb3.Attributes.Add("disabled", "disabled");
                                #endregion

                                if (tb.ID.Contains("txtMaterialSAPCode") || tb.ID.Contains("txtMaterialDescription"))
                                {
                                    tb.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tb.Style.Add("text-align", "right");
                                }

                                if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                                else
                                {
                                    tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                    tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                }
                                if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(" + i + ",'MATYIELD');");
                                }

                                if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(" + i + ",'SCRAPALLOWENCE');");
                                }

                                if (tb.ID.Contains("txtCavity"))
                                {
                                    tb.Attributes.Add("onkeyup", "ReturnBaseQtyProcBaseQtyValue();GetRunnerRatioPrcentagePerPiece();");
                                }

                                if (tb.ID.Contains("txt~RunnerWeight/shot(g)"))
                                {
                                    tb.Attributes.Add("onkeyup", "GetRunnerRatioPrcentagePerPiece();");
                                }

                                if (tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("RecycleMaterialRatio(%)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                if (tb.ID.Contains("RunnerWeight/shot(g)") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("PartNetUnitWeight(g)") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RecycleMaterialRatio(%)") || tb.ID.Contains("Cavity") || tb.ID.Contains("MaterialYield/MeltingLoss(%)"))
                                {
                                    if (txtprocs.Text.ToUpper() != "ST")
                                    {
                                        Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                        ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                        ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
                                        if (tb.ID.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tb.Text = txtPartUnit.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                        {
                                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
                                        }
                                        //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                        if (tb.ID.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tb.Text = txtPartUnit.Value;
                                            tCell.Controls.Add(tb);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                            //tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                        {
                                            Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                        }
                                        else if (tb.ID.Contains("txtMaterialGrossWeight/pc(g)"))
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                            tCell.Controls.Add(tb);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                        else
                                        {
                                            tCell.Controls.Add(tb);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                    }
                                }
                                else if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") || tb.ID.Contains("txtMaterialScrapWeight(g)"))
                                {
                                    if (tb.ID.Contains("txtTotalRawMaterialCost/g"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                        tb.Style.Add("width", "150px;");
                                        tCell.Controls.Add(tb);
                                        tCell.Controls.Add(tb3);
                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                        tCell.Controls.Add(tb);
                                    }
                                    Table1.Rows[cellCtr].Cells.Add(tCell);

                                }
                                else
                                {
                                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    {
                                        tb.Style.Add("width", "100px;");
                                        tCell.Controls.Add(tb);
                                        //tCell.Controls.Add(tb2);
                                    }
                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                    }
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }

                                if (DtMaterialsDetails.Rows.Count > 0)
                                {
                                    if (i < DtMaterialsDetails.Rows.Count)
                                    {
                                        if (tb.ID.Contains("txtMaterialSAPCode"))
                                        {
                                            //tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("txtMaterialDescription"))
                                        {
                                            //tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                            //tb2.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                        {
                                            //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

                                            //double rawperkg = RawVal / 1000;
                                            //tb.Text = (rawperkg.ToString());
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }

                                if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout1")
                                    {
                                        tb.Text = "5";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout3")
                                    {
                                        tb.Text = "10";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tb.Text = "2";
                                    }
                                    else
                                    {
                                        tb.Text = "";
                                    }
                                }

                                if (tb.ID.Contains("txt~MaterialDensity"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tb.Text = "7.86";
                                        //tb.Enabled = false;
                                    }
                                    else
                                    {
                                        tb.Text = "";
                                    }
                                }

                                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                {
                                    for (int ii = 0; ii < TempMClistNew.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                            {
                                                if (HdnFirsLoad.Value == "1")
                                                {
                                                    tb.Text = "";
                                                }
                                                else
                                                {
                                                    tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                                }
                                            }
                                            else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                            {
                                                tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                                //int Clm = i;
                                                //var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                                //if (ArrRwMatUom.Count() >= Clm)
                                                //{
                                                //    tb2.Text = ArrRwMatUom[Clm].ToString().Replace("NaN", "");
                                                //}
                                            }
                                            else if (tb.ID.Contains("txtTotalRawMaterialCost/g"))
                                            {
                                                tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                                int Clm = i;
                                                var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                                                if (ArrRwMatUom.Count() >= Clm)
                                                {
                                                    string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                                    if (UOM == "KG")
                                                    {
                                                        tb3.Text = "G";
                                                    }
                                                    else
                                                    {
                                                        tb3.Text = UOM;
                                                    }
                                                }
                                                //tb2.Text = tempMClist[ii+1].ToString().Replace("NaN", "");
                                                //ii = ii + 1;
                                            }
                                            else
                                            {
                                                tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                            }
                                            break;
                                        }
                                    }
                                }
                                //tb.Attributes.Add("disabled", "disabled");


                            }
                        }
                    }
                    Session["Table"] = Table1;
                    Table1.DataBind();
                }
                Session["Table"] = Table1;

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            

        }

        /// <summary>
        /// Process Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicProcessDT(int ColumnType)
        {
            try
            {
                int TotColProcOld = 0;
                if (Session["TotColProcOld"] != null)
                {
                    TotColProcOld = Convert.ToInt32(Session["TotColProcOld"].ToString());
                }

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                DataTable DtDynamicProcessCostsDetails = new DataTable();
                DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

                bool IsUseToolAmortize = false;
                bool IsUseMachineAmortize = false;
                bool AcsTabProcCost = true;
                if (Session["IsUseToolAmortize"] != null)
                {
                    IsUseToolAmortize = Convert.ToBoolean(Session["IsUseToolAmortize"].ToString());
                }
                if (Session["IsUseMachineAmortize"] != null)
                {
                    IsUseMachineAmortize = Convert.ToBoolean(Session["IsUseMachineAmortize"].ToString());
                }
                if (Session["AcsTabProcCost"] != null) {
                    AcsTabProcCost = Convert.ToBoolean(Session["AcsTabProcCost"].ToString());
                }

                if (ColumnType == 0)
                {
                    int rowcount = 0;
                    TableRow Hearderrow = new TableRow();

                    TablePC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();

                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                            Hearderrow.Cells.Add(tCell1);
                        }
                        else
                        {
                            LinkButton BtnDel = new LinkButton();
                            int TtlField = DtDynamicProcessFields.Rows.Count;
                            BtnDel.ID = "BtnDeleteProc" + ColmNoProc;
                            BtnDel.Text = "Delete";
                            BtnDel.ForeColor = Color.Yellow;
                            BtnDel.OnClientClick = "DelProces('" + ColmNoProc + "','" + TtlField + "');return false;";
                            tCell1.Controls.Add(BtnDel);
                            Hearderrow.Cells.Add(tCell1);
                            ColmNoProc++;

                            if (Session["AcsTabProcCost"] != null)
                            {
                                if ((bool)Session["AcsTabProcCost"] == false)
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");
                                }
                            }
                        }

                        //TableCell tCell1 = new TableCell();
                        //Label lb1 = new Label();
                        //tCell1.Controls.Add(lb1);
                        //if (cellCtr == 0)
                        //    tCell1.Text = "Field Name";
                        //Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }
                    foreach (DataRow row in DtDynamicProcessFields.Rows)
                    {
                        TableRow tRow = new TableRow();
                        TablePC.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                //tCell.Text = row.ItemArray[0].ToString();
                                lb.Text = row.ItemArray[0].ToString();
                                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("IFTURNKEY-VENDORNAME"))
                                {
                                    tCell.Text = "If Subcon - Subcon Name";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("VENDORRATE"))
                                {
                                    tCell.Text = "Vendor Rate/HR";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                {
                                    tCell.Text = "Total Process Cost/pc";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TURNKEYPROFIT"))
                                {
                                    tCell.Text = "Turnkey Fees";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("EFFICIENCY/PROCESSYIELD(%)"))
                                {
                                    tCell.Text = "Efficiency";
                                }
                                else
                                {
                                    tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                lb.Width = 240;
                                tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlProcess" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["process"];
                                    ddl.DataTextField = "Process_Grp_code";
                                    //ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged2;
                                    ddl.DataValueField = "ProcGrpCodeOnly";
                                    ddl.Attributes.Add("onchange", "ProcGrpChange(" + (cellCtr - 1) + ")");
                                    // ddl.da
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");
                                                var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                if (tempPClist.Count() > 2)
                                                {
                                                    if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                    {
                                                        if (ddlcheck != null)
                                                            ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                        var strddlSelitem = ddl.SelectedItem.Text.ToString();
                                                        string[] Arrstrdd = strddlSelitem.ToString().Split('-');
                                                        MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
                                                        ddl.Attributes.Remove("disabled");
                                                    }
                                                    else
                                                    {
                                                        if (ddlcheck != null)
                                                            ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                        var strddlSelitem = ddl.SelectedItem.Text.ToString();
                                                        string[] Arrstrdd = strddlSelitem.ToString().Split('-');
                                                        MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
                                                        ddl.Attributes.Add("disabled", "disabled");
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion retrive data


                                    if (Session["IsRevision"] != null) {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false) {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                }
                                else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubProcess" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");

                                    ddl.DataSource = Session["PSGroupwithUOM"];
                                    ddl.DataTextField = "SubProcessName";
                                    ddl.DataValueField = "SubProcessName";
                                    ddl.Attributes.Add("onchange", "SubProcgroupChnge(" + (cellCtr - 1) + ")");
                                    // ddl.da
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                string[] ArrProcGroup = tempPClist[0].ToString().Replace("NaN", "").Split('-');
                                                string ProcGroup = ArrProcGroup[0].ToString();
                                                GetDataSubProcesGroup2(ProcGroup);
                                                if (Session["SubProcess"] != null)
                                                {
                                                    ddl.Items.Clear();
                                                    ddl.DataSource = Session["SubProcess"];
                                                    ddl.DataTextField = "SubProcessName";
                                                    ddl.DataValueField = "SubProcessName";
                                                    ddl.DataBind();
                                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                                    var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                    if (ddlcheck1 != null)
                                                    {
                                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                        ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    ddl.Items.Clear();
                                                }

                                                if (tempPClist.Count() > 2)
                                                {
                                                    if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                    {
                                                        ddl.Attributes.Remove("disabled");
                                                    }
                                                    else
                                                    {
                                                        ddl.Attributes.Add("disabled", "disabled");
                                                    }
                                                }

                                                break;

                                            }

                                        }

                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "" || tempPClist[0].ToString().Replace("Select", "") == "" || tempPClist[0].ToString().Replace("--Select--", "") == "")
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    #region old retrive data
                                    //if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    //{
                                    //    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    //    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    //    {
                                    //        if (ii == rowcount)
                                    //        {

                                    //            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                    //            {

                                    //                for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                    //                {
                                    //                    if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
                                    //                    {
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

                                    //                    }
                                    //                }

                                    //                var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

                                    //                if (ddlcheck1 != null)

                                    //                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                    //                //ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;


                                    //            }
                                    //            else
                                    //            {
                                    //                ddl.Attributes.Add("disabled", "disabled");
                                    //                ddl.SelectedValue = "";
                                    //            }
                                    //            ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                    //            break;

                                    //        }

                                    //    }
                                    //}
                                    #endregion old retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                }
                                else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubvendorname" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");

                                    ddl.DataSource = Session["SubVndorData"];
                                    ddl.DataTextField = "SubVndorData";
                                    ddl.DataValueField = "SubVndorData";
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 0)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                    }
                                    ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    ddl.Attributes.Add("onchange", "SubVendorData(" + (cellCtr - 1) + ")");

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                {
                                                    string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");

                                                    var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

                                                    if (ddlcheck != null)
                                                    {
                                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                    }
                                                }
                                                else
                                                {
                                                    ddl.SelectedIndex = 0;
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }

                                                //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }

                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "" || tempPClist[0].ToString().Replace("Select", "") == "" || tempPClist[0].ToString().Replace("--Select--", "") == "")
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                }
                                else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlMachineLabor" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");
                                    //ddl.Items.Add("Machine");
                                    // ddl.Items.Add("Labor");
                                    // ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
                                    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
                                    ddl.Attributes.Add("onchange", "DdlMachineLaborChange(" + (cellCtr - 1) + ")");
                                    //ddl.Style.Add("width", "142px");
                                    //ddl.Attributes.Add("disabled", "disabled");


                                    DropDownList ddlhide = new DropDownList();
                                    ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
                                    ddlhide.Style.Add("display", "none");
                                    ddlhide.Attributes.Add("disabled", "disabled");
                                    //ddlhide.Style.Add("width", "142px");

                                    #region retrive data
                                    if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
                                    {

                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                {
                                                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
                                                    {
                                                        string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");
                                                        var ddlcheck = ddl.Items.FindByText(txtddlTemp);
                                                        if (ddlcheck != null)
                                                        {
                                                            ddl.Items.FindByText(txtddlTemp).Selected = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ddl.Style.Add("display", "none");
                                                        ddlhide.Style.Add("display", "block");
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    ddl.Style.Add("display", "none");
                                                    ddlhide.Style.Add("display", "block");
                                                    break;
                                                }
                                            }

                                        }
                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "" || tempPClist[0].ToString().Replace("Select", "") == "" || tempPClist[0].ToString().Replace("--Select--", "") == "")
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    #region old retrive data
                                    //if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
                                    //{

                                    //    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    //    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    //    {
                                    //        if (ii == rowcount)
                                    //        {
                                    //            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                    //            {
                                    //                string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");

                                    //                var ddlcheck = ddl.Items.FindByText(txtddlTemp);
                                    //                if (ddlcheck != null)
                                    //                    ddl.Items.FindByText(txtddlTemp).Selected = true;
                                    //                break;
                                    //            }
                                    //            else if (tempPClist[2].ToString() != "" || tempPClist[2].ToString() != null)
                                    //            {
                                    //                //ddlhide.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                    //                ddl.Style.Add("display", "none");
                                    //                ddlhide.Style.Add("display", "block");
                                    //                break;
                                    //            }
                                    //        }

                                    //    }
                                    //}
                                    #endregion old retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                    tCell.Controls.Add(ddlhide);

                                }
                                else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlMachine" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");
                                    //ddl.Style.Add("width", "142px");
                                    if (Session["MachineIDs"] != null)
                                    {
                                        DataTable DtMachine = (DataTable)Session["MachineIDs"];
                                        if (DtMachine.Rows.Count > 0)
                                        {
                                            for (int m = 0; m < DtMachine.Rows.Count; m++)
                                            {
                                                string MacId = DtMachine.Rows[m]["MacId"].ToString();
                                                string srText = DtMachine.Rows[m]["MachineID"].ToString();
                                                string srValue = DtMachine.Rows[m]["MachineID"].ToString();
                                                ListItem Ddlitem = new ListItem { Text = srText, Value = srValue };
                                                Ddlitem.Attributes.Add("Mac-Id", MacId);
                                                ddl.Items.Add(Ddlitem);
                                            }

                                        }
                                    }
                                    //ddl.DataSource = Session["MachineIDs"];
                                    //ddl.DataTextField = "MachineID";
                                    //ddl.DataValueField = "MachineID";
                                    ddl.Attributes.Add("onchange", "MachineChange(" + (cellCtr - 1) + ")");
                                    // ddl.da
                                    //ddl.DataBind();
                                    if (ddl.Items.Count > 0)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                    }
                                    // ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                    //ddl.Attributes.Add("disabled", "disabled");
                                    TextBox tb = new TextBox();
                                    tb.ID = "txtMachineId" + (cellCtr - 1);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("display", "none");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    //tb.Attributes.Add("disabled", "disabled");
                                    // style = "display: none;"
                                    //tb.Visible = false;

                                    DropDownList ddlMachide = new DropDownList();
                                    ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
                                    ddlMachide.Attributes.Add("disabled", "disabled");
                                    //ddlMachide.Style.Add("width", "142px");
                                    ddlMachide.Style.Add("display", "none");
                                    //ddlMachide.Attributes.Add("disabled", "disabled");


                                    TextBox tbhide = new TextBox();
                                    tbhide.ID = "txtHide" + (cellCtr - 1);
                                    tbhide.Style.Add("display", "none");
                                    tbhide.Attributes.Add("disabled", "disabled");

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                {
                                                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
                                                    {
                                                        if (tempPClist[4].ToString() == "Machine")
                                                        {
                                                            string[] ArrProcGroup = tempPClist[0].ToString().Replace("NaN", "").Split('-');
                                                            string ProcGroup = ArrProcGroup[0].ToString();
                                                            GetDataVndmachine2(ProcGroup);
                                                            if (Session["SubVndMachineByProcGroup"] != null)
                                                            {
                                                                ddl.Items.Clear();
                                                                DataTable DtMachine = (DataTable)Session["SubVndMachineByProcGroup"];
                                                                if (DtMachine.Rows.Count > 0)
                                                                {
                                                                    for (int m = 0; m < DtMachine.Rows.Count; m++)
                                                                    {
                                                                        string MacId = DtMachine.Rows[m]["MacId"].ToString();
                                                                        string srText = DtMachine.Rows[m]["MachineID"].ToString();
                                                                        string srValue = DtMachine.Rows[m]["MachineID"].ToString();
                                                                        ListItem Ddlitem = new ListItem { Text = srText, Value = srValue };
                                                                        Ddlitem.Attributes.Add("Mac-Id", MacId);
                                                                        ddl.Items.Add(Ddlitem);
                                                                    }

                                                                }
                                                                
                                                                //ddl.DataSource = Session["SubVndMachineByProcGroup"];
                                                                //ddl.DataTextField = "Machine";
                                                                //ddl.DataValueField = "Machine";
                                                                //ddl.DataBind();

                                                                //string Qno = hdnQuoteNo.Value;
                                                                //string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                string IsSAPCode = hdnIsSAPCode.Value;
                                                                if (IsSAPCode == "False")
                                                                {
                                                                    if (ddl.Items.Count <= 0)
                                                                    {
                                                                        tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                                        ddl.Style.Add("display", "none");
                                                                        tb.Style.Add("display", "block");
                                                                    }
                                                                    else
                                                                    {
                                                                        var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                                        if (ddlcheck1 != null)
                                                                        {
                                                                            ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                                            ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                                                    string a = tempPClist[ii].ToString().Replace("NaN", "");
                                                                    var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                                    if (ddlcheck1 != null)
                                                                    {
                                                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                                        ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ddl.Items.Clear();
                                                            }
                                                        }
                                                        else if (tempPClist[4].ToString() == "Labor")
                                                        {
                                                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                            ddl.Style.Add("display", "none");
                                                            tb.Style.Add("display", "block");
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Style.Add("display", "none");
                                                        tb.Style.Add("display", "block");
                                                        tb.Attributes.Add("disabled", "disabled");
                                                        if (tempPClist[4].ToString() == "Machine")
                                                        {
                                                            //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                            //ddlMachide.Style.Add("display", "block");
                                                            ddl.Style.Add("display", "none");
                                                            ddlMachide.Attributes.Add("disabled", "disabled");
                                                        }
                                                        else if (tempPClist[4].ToString() == "Labor")
                                                        {
                                                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                            ddl.Style.Add("display", "none");
                                                            ddlMachide.Style.Add("display", "none");
                                                            tb.Style.Add("display", "block");
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    ddl.Style.Add("display", "none");
                                                    ddlMachide.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                    tb.Attributes.Add("disabled", "disabled");
                                                    if (tempPClist[4].ToString() == "Machine")
                                                    {
                                                        //ddlMachide.Style.Add("display", "block");
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Attributes.Add("disabled", "disabled");
                                                    }
                                                    else if (tempPClist[4].ToString() == "Labor")
                                                    {
                                                        tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Style.Add("display", "none");
                                                        tb.Style.Add("display", "block");
                                                        tb.Attributes.Add("disabled", "disabled");
                                                    }
                                                    break;
                                                }
                                            }

                                        }
                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "" || tempPClist[0].ToString().Replace("Select", "") == "" || tempPClist[0].ToString().Replace("--Select--", "") == "")
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }

                                    tCell.Controls.Add(ddl);
                                    tCell.Controls.Add(tb);

                                    tCell.Controls.Add(ddlMachide);
                                    tCell.Controls.Add(tbhide);


                                }

                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    if (tb.ID.Contains("txtDurationperProcessUOM(Sec)"))
                                    {
                                        tb.Attributes.Add("onkeypress", "IsInteger(event);");
                                    }

                                    string txtID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);

                                    if (tb.ID.Contains("txtBaseqty"))
                                    {
                                        string Id = row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                        string HtmlCode = " <fieldset> <div style='display: none;' id='Dv" + Id + "'>" +
                                                          " <label><input type='checkbox' Checked='true' style='display: inline;' onchange='ChkBaseQtyProcessChange(" + (cellCtr - 1) + ");' id='Chk" + Id + "'>Follow material Base Qty / Cavity <br /></label>" +
                                                          " </ div > </fieldset>";
                                        tCell.Controls.Add(new LiteralControl(HtmlCode));
                                    }

                                    if (tb.ID.Contains("txtStandardRate/HR") || tb.ID.Contains("txtVendorRate") || tb.ID.Contains("txtBaseqty") || tb.ID.Contains("txtDurationperProcessUOM(Sec)") || tb.ID.Contains("txtEfficiency/ProcessYield(%)") || tb.ID.Contains("txtTurnkeyCost/pc") || tb.ID.Contains("txtTurnkeyProfit") || tb.ID.Contains("txtProcessCost/pc") || tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }

                                    if (tb.ID.Contains("txtIfTurnkey-VendorName"))
                                    {
                                        tb.Attributes.Add("onkeyup", "TurnKeyVendorUpdate('" + (cellCtr - 1) + "');");
                                    }

                                    if (tb.ID.Contains("Name")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }
                                    //tb.Attributes.Add("disabled", "disabled");
                                    #region retrive data

                                    if (tb.ID.Contains("txtTotalProcessesCost/pcs") || tb.ID.Contains("txtProcessCost/pc") || tb.ID.Contains("txtProcessUOM") || tb.ID.Contains("txtBaseqty") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") || tb.ID.Contains("txtTurnkeyCost/pc") || tb.ID.Contains("txtTurnkeyProfit"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();
                                        if (tempPClist.Count() >= (DtDynamicProcessFields.Rows.Count - 1))
                                        {
                                            for (int ii = 0; ii < tempPClist.Count; ii++)
                                            {
                                                if (ii == rowcount)
                                                {
                                                    if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                                    {
                                                        string cek = tempPClist[3].ToString();
                                                        if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
                                                        {
                                                            if (tempPClist[8].ToString().Contains("STROKES/MIN"))
                                                            {
                                                                #region condition STROKES/MIN
                                                                if (tb.ID.Contains("txtVendorRate") || tb.ID.Contains("txtIfTurnkey-VendorName") || tb.ID.Contains("txtBaseqty"))
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }

                                                                if ((tb.ID.Contains("txtDurationperProcessUOM(Sec)")) ||
                                                                    (tb.ID.Contains("txtEfficiency/ProcessYield(%)")) || (tb.ID.Contains("txtTurnkeyProfit")) ||
                                                                    (tb.ID.Contains("txtProcessCost/pc")))
                                                                {
                                                                    tb.Attributes.Add("disabled", "disabled");
                                                                }

                                                                if (tb.ID.Contains("txtBaseqty"))
                                                                {
                                                                    DataTable DtDynamic = new DataTable();
                                                                    DtDynamic = (DataTable)Session["DtDynamic"];
                                                                    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
                                                                    var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;
                                                                    if (ccc < 2)
                                                                    {
                                                                        string ProcGroup = tempPClist[0].ToString().Replace("NaN", "");
                                                                        string[] ArrProcGroup = ProcGroup.ToString().Split('-');
                                                                        string ProcGroupCode = ArrProcGroup[0].ToString().ToUpper().Trim();
                                                                        if (ProcGroupCode == "ST")
                                                                        {
                                                                            tb.Attributes.Add("disabled", "disabled");
                                                                        }
                                                                    }
                                                                }
                                                                #endregion
                                                            }
                                                            else if (tempPClist[8].ToString().Trim().Replace(" ", "").ToUpper().Contains("CYCLETIMEINSEC/SHOT"))
                                                            {
                                                                #region condition CYCLETIMEINSEC/SHOT
                                                                if (hdnProcGroup.Value.ToString().ToUpper() == "LAYOUT5")
                                                                {
                                                                    if ((tb.ID.Contains("txtProcessCost/pc")))
                                                                    {
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }

                                                                    if (tb.ID.Contains("txtBaseqty"))
                                                                    {
                                                                        DataTable DtDynamic = new DataTable();
                                                                        DtDynamic = (DataTable)Session["DtDynamic"];
                                                                        var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
                                                                        var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;
                                                                        if (ccc < 2)
                                                                        {
                                                                            tb.Attributes.Add("disabled", "disabled");
                                                                        }
                                                                        else
                                                                        {
                                                                            tb.Attributes.Remove("disabled");
                                                                        }
                                                                    }
                                                                }
                                                                #endregion CYCLETIMEINSEC/SHOT
                                                            }
                                                            else
                                                            {
                                                                if ((tb.ID.Contains("ProcessCost/pc")))
                                                                {
                                                                    tb.Attributes.Add("disabled", "disabled");
                                                                }

                                                                if (tempPClist[8].ToString().Trim().ToLower().Replace(" ", "") == "pcs/load" && ii == 9)
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }
                                                                else if (tempPClist[8].ToString().Trim().ToUpper().Replace(" ", "") == "CYCLETIMEINSEC" && ii == 9)
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }

                                                            }

                                                            if (tb.ID.Contains("txtVendorRate"))
                                                            {
                                                                if (tempPClist[4].ToString().Trim().Replace(" ", "").ToUpper().Contains("LABOR"))
                                                                {
                                                                    for (int h = 0; h < grdLaborlisthidden.Rows.Count; h++)
                                                                    {
                                                                        if (grdLaborlisthidden.Rows[h].Cells[1].Text == "N")
                                                                        {
                                                                            tb.Attributes.Remove("disabled");
                                                                        }
                                                                        else
                                                                        {
                                                                            //string IsSAPCode = hdnIsSAPCode.Value;
                                                                            //if (IsSAPCode == "False")
                                                                            //{
                                                                            //    tb.Attributes.Remove("disabled");
                                                                            //}
                                                                            //else
                                                                            //{
                                                                            //    tb.Attributes.Add("disabled", "disabled");
                                                                            //}
                                                                            tb.Attributes.Add("disabled", "disabled");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    // condition folowong standrat rate or not by machine
                                                                    string[] ArrProcGroup = tempPClist[0].ToString().Replace("NaN", "").Split('-');
                                                                    string ProcGroup = ArrProcGroup[0].ToString();

                                                                    string[] ArrMachineId = tempPClist[5].ToString().Replace("NaN", "").Split('-');
                                                                    string MachineId = ArrMachineId[0].ToString();

                                                                    GetDataVendRate(ProcGroup, MachineId);
                                                                    if (hdnFollowStdRate.Value == "N")
                                                                    {
                                                                        tb.Attributes.Remove("disabled");
                                                                    }
                                                                    else
                                                                    {
                                                                        //string IsSAPCode = hdnIsSAPCode.Value;
                                                                        //if (IsSAPCode == "False")
                                                                        //{
                                                                        //    tb.Attributes.Remove("disabled");
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    tb.Attributes.Add("disabled", "disabled");
                                                                        //}
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (tb.ID.Contains("txtTurnkeyCost/pc"))
                                                            {
                                                                tb.Attributes.Remove("disabled");
                                                            }
                                                            else
                                                            {
                                                                tb.Attributes.Add("disabled", "disabled");
                                                            }
                                                        }

                                                        #region SAPSpProcType Condition
                                                        if (tb.ID.Contains("txtBaseqty"))
                                                        {
                                                            if (HdnSAPSpProcType.Value == "30")
                                                            {
                                                                if (HdnAcsTabMatCost.Value == "False")
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        if ((tb.ID.Contains("txtProcessCost/pc")) || (tb.ID.Contains("txtIfTurnkey-VendorName")))
                                                        {
                                                            tb.Attributes.Remove("disabled");
                                                        }
                                                        else
                                                        {
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }

                                                    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                                    {
                                                        if (HdnFirsLoad.Value == "1")
                                                        {
                                                            if (hdnMassRevision.Value != "")
                                                            {
                                                                tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                            }
                                                            else
                                                            {
                                                                tb.Text = "";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                    }
                                                    break;
                                                }

                                            }
                                        }

                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "" || tempPClist[0].ToString().Replace("Select", "") == "" || tempPClist[0].ToString().Replace("--Select--", "") == "")
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        #region SAPSpProcType Condition
                                        if (tb.ID.Contains("txtBaseqty"))
                                        {
                                            if (HdnSAPSpProcType.Value == "30")
                                            {
                                                if (HdnAcsTabMatCost.Value == "False")
                                                {
                                                    tb.Text = "1";
                                                }
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }

                                    if (tb.ID.Contains("txtStandardRate/HR"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    tCell.Controls.Add(tb);
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                tRow.Cells.Add(tCell);
                            }
                        }

                        if (rowcount % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcount++;
                    }
                    Session["TablePC"] = TablePC;
                }
                else
                {

                    TablePC = (Table)Session["TablePC"];

                    int CellsCount = ColumnType;
                    for (int i = 1; i < CellsCount; i++)
                    {
                        var tempPclist = hdnProcessValues.Value.ToString().Split(',').ToList();

                        var TempPclistNew = tempPclist;

                        //if (CellsCount == 2 && tempPclist.Count <= (DtDynamicProcessFields.Rows.Count + 1))
                        //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i)) * (DtDynamicProcessFields.Rows.Count + 1))).ToList();
                        ////else if (CellsCount ==  && tempSMClist.Count > 6)
                        ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                        //else if (CellsCount == 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 1))
                        //    //TempPclistNew = TempPclistNew.Skip(((CellsCount - (i + 1)) * DtDynamicProcessFields.Rows.Count)).ToList();
                        //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                        ////else if (CellsCount >= 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount == (i + 1))
                        ////    //TempPclistNew = TempPclistNew.Skip(((CellsCount) * (DtDynamicProcessFields.Rows.Count))).ToList();
                        ////    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                        //else if (i >= 1 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 1))
                        //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                        //else
                        //{
                        //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                        //}
                        int NumertoSkip = (i * (DtDynamicProcessFields.Rows.Count));
                        int TotData = tempPclist.Count;
                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();

                        int rowcount = 0;

                        #region loop for Dynamic Binding - Process Cost
                        for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                LinkButton BtnDel = new LinkButton();
                                int TtlField = DtDynamicProcessFields.Rows.Count;
                                BtnDel.ID = "BtnDeleteProc" + ColmNoProc;
                                BtnDel.Text = "Delete";
                                BtnDel.ForeColor = Color.Yellow;
                                BtnDel.OnClientClick = "DelProces(" + ColmNoProc + "," + TtlField + "); return false;";
                                tCell.Controls.Add(BtnDel);
                                TablePC.Rows[cellCtr].Cells.Add(tCell);
                                ColmNoProc++;

                                if (Session["AcsTabProcCost"] != null)
                                {
                                    if ((bool)Session["AcsTabProcCost"] == false)
                                    {
                                        if (IsUseMachineAmortize == true && (ColmNoProc - 1) > TotColProcOld)
                                        {
                                            BtnDel.Attributes.Add("style", "display:block;");
                                        }
                                        else
                                        {
                                            BtnDel.Attributes.Add("style", "display:none;");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlProcess" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["process"];
                                    ddl.DataTextField = "Process_Grp_code";
                                    ddl.DataValueField = "ProcGrpCodeOnly";
                                    ddl.Attributes.Add("onchange", "ProcGrpChange(" + i + ")");
                                    ddl.DataBind();
                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                    #region retrive data
                                    if (TempPclistNew.Count >= DtDynamicProcessFields.Rows.Count)
                                    {
                                        if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                        {
                                            for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                            {
                                                int CekDataNcolumn = tempPclist.Count / CellsCount;
                                                string zz = TempPclistNew[ii].ToString();
                                                if (ii == rowcount - 1)
                                                {
                                                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                    if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                    {
                                                        if (ddlcheck != null)
                                                        {
                                                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                            var strddlSelitem = ddl.SelectedItem.Text.ToString();
                                                            string[] Arrstrdd = strddlSelitem.ToString().Split('-');
                                                            MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
                                                        }

                                                        ddl.Attributes.Remove("disabled");
                                                    }
                                                    else
                                                    {
                                                        if (ddlcheck != null)
                                                        {
                                                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                            var strddlSelitem = ddl.SelectedItem.Text.ToString();
                                                            string[] Arrstrdd = strddlSelitem.ToString().Split('-');
                                                            MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
                                                        }
                                                        ddl.Attributes.Add("disabled", "disabled");
                                                    }
                                                    //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubProcess" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["PSGroupwithUOM"];
                                    ddl.DataTextField = "SubProcessName";
                                    ddl.DataValueField = "SubProcessName";
                                    ddl.Attributes.Add("onchange", "SubProcgroupChnge(" + i + ")");
                                    // ddl.da
                                    ddl.DataBind();
                                    // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged4;
                                    ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                        {
                                            if (ii == rowcount - 1)
                                            {
                                                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                string[] ArrProcGroup = TempPclistNew[0].ToString().Replace("NaN", "").Split('-');
                                                string ProcGroup = ArrProcGroup[0].ToString();
                                                GetDataSubProcesGroup2(ProcGroup);
                                                if (Session["SubProcess"] != null)
                                                {
                                                    ddl.Items.Clear();
                                                    ddl.DataSource = Session["SubProcess"];
                                                    ddl.DataTextField = "SubProcessName";
                                                    ddl.DataValueField = "SubProcessName";
                                                    ddl.DataBind();
                                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                                    var ddlcheck1 = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                    if (ddlcheck1 != null)
                                                    {
                                                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                        ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    ddl.Items.Clear();
                                                }

                                                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                {
                                                    ddl.Attributes.Remove("disabled");
                                                }
                                                else
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }

                                                if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }
                                            if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "" || TempPclistNew[0].ToString().Replace("Select", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                        }

                                        if (TempPclistNew.Count == 0)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }

                                else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubvendorname" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["SubVndorData"];
                                    ddl.DataTextField = "SubVndorData";
                                    ddl.DataValueField = "SubVndorData";
                                    //ddl.Attributes.Add("onchange", "alert('aaaa')");
                                    ddl.Attributes.Add("onchange", "SubVendorData(" + i + ")");
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 0)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                    }

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                        {
                                            string zz = TempPclistNew[ii].ToString();
                                            if (ii == rowcount - 1)
                                            {
                                                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                {
                                                    // ddl.SelectedValue = grdProcessGrphidden.Rows[h].Cells[0].Text;
                                                    string abc = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                    if (ddlcheck != null)
                                                    {
                                                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                        //ListColumnUseSubVnder = "" + i + ",";
                                                    }
                                                }
                                                else
                                                {
                                                    //ddl.Attributes.Add("disabled", "disabled");
                                                    ddl.SelectedIndex = 0;
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                                //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                            if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "" || TempPclistNew[0].ToString().Replace("Select", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                        if (TempPclistNew.Count == 0)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }

                                else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlMachineLabor" + (i);
                                    ddl.Style.Add("min-width", "300px;");

                                    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
                                    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
                                    ddl.Attributes.Add("onchange", "DdlMachineLaborChange(" + i + ")");
                                    DropDownList ddlhide = new DropDownList();
                                    ddlhide.ID = "dynamicddlHideMachineLabor" + (i);
                                    ddlhide.Style.Add("display", "none");
                                    ddlhide.Attributes.Add("disabled", "disabled");
                                    //ddlhide.Style.Add("width", "142px");

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                        {
                                            if (ii == rowcount - 1)
                                            {
                                                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                {
                                                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
                                                    {
                                                        var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                        if (ddlcheck != null)
                                                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                    }
                                                    else
                                                    {
                                                        ddl.Style.Add("display", "none");
                                                        ddlhide.Style.Add("display", "block");
                                                    }
                                                }
                                                else if (TempPclistNew[2].ToString() != "" || TempPclistNew[2].ToString() != null)
                                                {
                                                    ddl.Style.Add("display", "none");
                                                    ddlhide.Style.Add("display", "block");
                                                }
                                                break;
                                            }

                                            if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "" || TempPclistNew[0].ToString().Replace("Select", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }

                                        }

                                        if (TempPclistNew.Count == 0)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                    tCell.Controls.Add(ddl);
                                    tCell.Controls.Add(ddlhide);
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }

                                else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlMachine" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    //ddl.Style.Add("width", "142px");
                                    if (Session["MachineIDs"] != null)
                                    {
                                        DataTable DtMachine = (DataTable)Session["MachineIDs"];
                                        if (DtMachine.Rows.Count > 0)
                                        {
                                            for (int m = 0; m < DtMachine.Rows.Count; m++)
                                            {
                                                string MacId = DtMachine.Rows[m]["MacId"].ToString();
                                                string srText = DtMachine.Rows[m]["MachineID"].ToString();
                                                string srValue = DtMachine.Rows[m]["MachineID"].ToString();
                                                ListItem Ddlitem = new ListItem { Text = srText, Value = srValue };
                                                Ddlitem.Attributes.Add("Mac-Id", MacId);
                                                ddl.Items.Add(Ddlitem);
                                            }

                                        }
                                    }
                                    //ddl.DataSource = Session["MachineIDs"];
                                    //ddl.DataTextField = "MachineID";
                                    //ddl.DataValueField = "MachineID";
                                    ddl.Attributes.Add("onchange", "MachineChange(" + i + ")");
                                    // ddl.da
                                    //ddl.DataBind();
                                    if (ddl.Items.Count > 0)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                    }
                                    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));


                                    TextBox tb = new TextBox();
                                    tb.ID = "txtMachineId" + (i);
                                    tb.Style.Add("display", "none");
                                    //tb.Attributes.Add("disabled", "disabled");
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");

                                    DropDownList ddlMachide = new DropDownList();
                                    ddlMachide.ID = "ddlHideMachine" + (i);
                                    //ddlMachide.Style.Add("width", "142px");
                                    ddlMachide.Style.Add("display", "none");
                                    ddlMachide.Attributes.Add("disabled", "disabled");


                                    TextBox tbhide = new TextBox();
                                    tbhide.ID = "txtHide" + (i);
                                    tbhide.Style.Add("display", "none");
                                    tbhide.Attributes.Add("disabled", "disabled");

                                    #region retrive data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {

                                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                        {
                                            if (ii == rowcount - 1)
                                            {
                                                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                {
                                                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
                                                    {
                                                        if (TempPclistNew[4].ToString() == "Machine")
                                                        {
                                                            string[] ArrProcGroup = TempPclistNew[0].ToString().Replace("NaN", "").Split('-');
                                                            string ProcGroup = ArrProcGroup[0].ToString();
                                                            GetDataVndmachine2(ProcGroup);
                                                            if (Session["SubVndMachineByProcGroup"] != null)
                                                            {
                                                                ddl.Items.Clear();
                                                                DataTable DtMachine = (DataTable)Session["SubVndMachineByProcGroup"];
                                                                if (DtMachine.Rows.Count > 0)
                                                                {
                                                                    for (int m = 0; m < DtMachine.Rows.Count; m++)
                                                                    {
                                                                        string MacId = DtMachine.Rows[m]["MacId"].ToString();
                                                                        string srText = DtMachine.Rows[m]["MachineID"].ToString();
                                                                        string srValue = DtMachine.Rows[m]["MachineID"].ToString();
                                                                        ListItem Ddlitem = new ListItem { Text = srText, Value = srValue };
                                                                        Ddlitem.Attributes.Add("Mac-Id", MacId);
                                                                        ddl.Items.Add(Ddlitem);
                                                                    }

                                                                }
                                                                //ddl.DataSource = Session["SubVndMachineByProcGroup"];
                                                                //ddl.DataTextField = "Machine";
                                                                //ddl.DataValueField = "Machine";
                                                                //ddl.DataBind();

                                                                //string Qno = hdnQuoteNo.Value;
                                                                //string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                string IsSAPCode = hdnIsSAPCode.Value;
                                                                if (IsSAPCode == "False")
                                                                {
                                                                    if (ddl.Items.Count <= 0)
                                                                    {
                                                                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                                        ddl.Style.Add("display", "none");
                                                                        tb.Style.Add("display", "block");
                                                                    }
                                                                    else
                                                                    {
                                                                        string q = TempPclistNew[ii].ToString();
                                                                        var ddlcheck1 = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                                        if (ddlcheck1 != null)
                                                                        {
                                                                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                                            ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                                        }
                                                                        ddl.Style.Add("display", "block");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                                                    string q = TempPclistNew[ii].ToString();
                                                                    var ddlcheck1 = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                                    if (ddlcheck1 != null)
                                                                    {
                                                                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                                        ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                                    }
                                                                    ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                                    ddl.Style.Add("display", "block");
                                                                }

                                                            }
                                                            else
                                                            {
                                                                ddl.Items.Clear();
                                                                ddl.Style.Add("display", "block");
                                                            }
                                                        }
                                                        else if (TempPclistNew[4].ToString() == "Labor")
                                                        {
                                                            tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                            ddl.Style.Add("display", "none");
                                                            tb.Style.Add("display", "block");
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Style.Add("display", "none");
                                                        tb.Style.Add("display", "block");
                                                        tb.Attributes.Add("disabled", "disabled");

                                                        if (TempPclistNew[4].ToString() == "Machine")
                                                        {
                                                            //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                            //ddlMachide.Style.Add("display", "block");
                                                            ddl.Style.Add("display", "none");
                                                            ddlMachide.Attributes.Add("disabled", "disabled");
                                                        }
                                                        else if (TempPclistNew[4].ToString() == "Labor")
                                                        {
                                                            tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                            ddl.Style.Add("display", "none");
                                                            ddlMachide.Style.Add("display", "none");
                                                            tb.Style.Add("display", "block");
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }

                                                    if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                    {
                                                        ddl.Attributes.Add("disabled", "disabled");
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    ddl.Style.Add("display", "none");
                                                    ddlMachide.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                    tb.Attributes.Add("disabled", "disabled");

                                                    if (TempPclistNew[4].ToString() == "Machine")
                                                    {
                                                        //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                        //ddlMachide.Style.Add("display", "block");
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Attributes.Add("disabled", "disabled");
                                                    }
                                                    else if (TempPclistNew[4].ToString() == "Labor")
                                                    {
                                                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                        ddl.Style.Add("display", "none");
                                                        ddlMachide.Style.Add("display", "none");
                                                        tb.Style.Add("display", "block");
                                                        tb.Attributes.Add("disabled", "disabled");
                                                    }

                                                    if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                    {
                                                        ddl.Attributes.Add("disabled", "disabled");
                                                    }
                                                    break;

                                                }
                                            }

                                            if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "" || TempPclistNew[0].ToString().Replace("Select", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }

                                        }

                                        if (TempPclistNew.Count == 0)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    #endregion retrive data

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }

                                    tCell.Controls.Add(ddl);
                                    tCell.Controls.Add(tb);
                                    tCell.Controls.Add(ddlMachide);
                                    tCell.Controls.Add(tbhide);
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);

                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    if (tb.ID.Contains("txtDurationperProcessUOM(Sec)"))
                                    {
                                        tb.Attributes.Add("onkeypress", "IsInteger(event);");
                                    }

                                    string txtID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                    if (tb.ID.Contains("txtBaseqty"))
                                    {
                                        string Id = DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                        string HtmlCode = " <fieldset> <div style='display: none;' id='Dv" + Id + "'>" +
                                                          " <label><input type='checkbox' Checked='true' style='display: inline;' onchange='ChkBaseQtyProcessChange(" + (i) + ");' id='Chk" + Id + "'>Follow material Base Qty / Cavity <br /></label>" +
                                                          " </ div > </fieldset>";
                                        tCell.Controls.Add(new LiteralControl(HtmlCode));
                                    }

                                    if (tb.ID.Contains("txtStandardRate/HR") || tb.ID.Contains("txtVendorRate") || tb.ID.Contains("txtBaseqty") || tb.ID.Contains("txtDurationperProcessUOM(Sec)") || tb.ID.Contains("txtEfficiency/ProcessYield(%)") || tb.ID.Contains("txtTurnkeyCost/pc") || tb.ID.Contains("txtTurnkeyProfit") || tb.ID.Contains("txtProcessCost/pc"))
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }

                                    if (tb.ID.Contains("txtIfTurnkey-VendorName"))
                                    {
                                        tb.Attributes.Add("onkeyup", "TurnKeyVendorUpdate('" + i + "');");
                                    }

                                    if (tb.ID.Contains("Name")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }
                                    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                    {
                                        TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

                                    }
                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                        TablePC.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                    if (tb.ID.Contains("TotalProcessesCost/pcs") || tb.ID.Contains("ProcessUOM") || (tb.ID.Contains("ProcessCost/pc")) || (tb.ID.ToString().ToLower().Contains("baseqty")) || (tb.ID.Contains("txtTurnkeyCost/pc")) || (tb.ID.Contains("txtTurnkeyProfit")))
                                    {
                                        if (tb.ID.Contains("DurationperProcessUOM(Sec)"))
                                        {

                                        }
                                        else
                                            tb.Attributes.Add("disabled", "disabled");
                                    }

                                    #region retriev data
                                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                    {
                                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                        {
                                            if (ii == (rowcount - 1))
                                            {
                                                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                {
                                                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
                                                    {
                                                        if (TempPclistNew[8].ToString().Contains("STROKES/MIN"))
                                                        {
                                                            #region condition STROKES/MIN
                                                            if (tb.ID.Contains("txtVendorRate") || tb.ID.Contains("txtIfTurnkey-VendorName") || tb.ID.Contains("txtBaseqty"))
                                                            {
                                                                tb.Attributes.Remove("disabled");
                                                            }

                                                            if ((tb.ID.Contains("txtDurationperProcessUOM(Sec)")) ||
                                                                (tb.ID.Contains("txtEfficiency/ProcessYield(%)")) || (tb.ID.Contains("txtTurnkeyProfit")) ||
                                                                (tb.ID.Contains("txtProcessCost/pc")))
                                                            {
                                                                tb.Attributes.Add("disabled", "disabled");
                                                            }

                                                            if (tb.ID.Contains("txtBaseqty"))
                                                            {
                                                                DataTable DtDynamic = new DataTable();
                                                                DtDynamic = (DataTable)Session["DtDynamic"];
                                                                var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
                                                                var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;
                                                                if (ccc < 2)
                                                                {
                                                                    string ProcGroup = TempPclistNew[0].ToString().Replace("NaN", "");
                                                                    string[] ArrProcGroup = ProcGroup.ToString().Split('-');
                                                                    string ProcGroupCode = ArrProcGroup[0].ToString().ToUpper().Trim();
                                                                    if (ProcGroupCode == "ST")
                                                                    {
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }
                                                                }
                                                            }
                                                            #endregion condition STROKES/MIN
                                                        }
                                                        else if (TempPclistNew[8].ToString().Trim().Replace(" ", "").ToUpper().Contains("CYCLETIMEINSEC/SHOT"))
                                                        {
                                                            #region condition CYCLETIMEINSEC/SHOT
                                                            if (hdnProcGroup.Value.ToString().ToUpper() == "LAYOUT5")
                                                            {
                                                                if ((tb.ID.Contains("txtProcessCost/pc")))
                                                                {
                                                                    tb.Attributes.Add("disabled", "disabled");
                                                                }

                                                                if (tb.ID.Contains("txtBaseqty"))
                                                                {
                                                                    DataTable DtDynamic = new DataTable();
                                                                    DtDynamic = (DataTable)Session["DtDynamic"];
                                                                    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
                                                                    var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;
                                                                    if (ccc < 2)
                                                                    {
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }
                                                                    else
                                                                    {
                                                                        tb.Attributes.Remove("disabled");
                                                                    }
                                                                }
                                                            }
                                                            #endregion CYCLETIMEINSEC/SHOT
                                                        }
                                                        else if (TempPclistNew[8].ToString().Trim().ToLower().Replace(" ", "") == "pcs/load" && ii == 9)
                                                        {
                                                            tb.Attributes.Remove("disabled");
                                                        }
                                                        else if (TempPclistNew[8].ToString().Trim().ToUpper().Replace(" ", "") == "CYCLETIMEINSEC" && ii == 9)
                                                        {
                                                            tb.Attributes.Remove("disabled");
                                                        }

                                                        if (tb.ID.Contains("txtVendorRate"))
                                                        {
                                                            if (TempPclistNew[4].ToString().Trim().Replace(" ", "").ToUpper().Contains("LABOR"))
                                                            {
                                                                for (int h = 0; h < grdLaborlisthidden.Rows.Count; h++)
                                                                {
                                                                    if (grdLaborlisthidden.Rows[h].Cells[1].Text == "N")
                                                                    {
                                                                        tb.Attributes.Remove("disabled");
                                                                    }
                                                                    else
                                                                    {
                                                                        //string IsSAPCode = hdnIsSAPCode.Value;
                                                                        //if (IsSAPCode == "False")
                                                                        //{
                                                                        //    tb.Attributes.Remove("disabled");
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    tb.Attributes.Add("disabled", "disabled");
                                                                        //}
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // condition folowong standrat rate or not by machine
                                                                string[] ArrProcGroup = TempPclistNew[0].ToString().Replace("NaN", "").Split('-');
                                                                string ProcGroup = ArrProcGroup[0].ToString();

                                                                string[] ArrMachineId = TempPclistNew[5].ToString().Replace("NaN", "").Split('-');
                                                                string MachineId = ArrMachineId[0].ToString();

                                                                GetDataVendRate(ProcGroup, MachineId);
                                                                if (hdnFollowStdRate.Value == "N")
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }
                                                                else
                                                                {
                                                                    //string IsSAPCode = hdnIsSAPCode.Value;
                                                                    //if (IsSAPCode == "False")
                                                                    //{
                                                                    //    tb.Attributes.Remove("disabled");
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    tb.Attributes.Add("disabled", "disabled");
                                                                    //}
                                                                    tb.Attributes.Add("disabled", "disabled");
                                                                }
                                                            }
                                                        }

                                                        #region SAPSpProcType Condition
                                                        if (tb.ID.Contains("txtBaseqty"))
                                                        {
                                                            if (HdnSAPSpProcType.Value == "30")
                                                            {
                                                                if (HdnAcsTabMatCost.Value == "False")
                                                                {
                                                                    tb.Attributes.Remove("disabled");
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        if (tb.ID.Contains("txtTurnkeyCost/pc"))
                                                        {
                                                            tb.Attributes.Remove("disabled");
                                                        }
                                                        else
                                                        {
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if ((tb.ID.Contains("txtProcessCost/pc")) || (tb.ID.Contains("txtIfTurnkey-VendorName")))
                                                    {
                                                        tb.Attributes.Remove("disabled");
                                                    }
                                                    else
                                                    {
                                                        tb.Attributes.Add("disabled", "disabled");
                                                    }
                                                }

                                                if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                                {
                                                    if (HdnFirsLoad.Value == "1")
                                                    {
                                                        tb.Text = "";
                                                    }
                                                    else
                                                    {
                                                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                }

                                                if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }

                                            if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "" || TempPclistNew[0].ToString().Replace("Select", "") == "")
                                            {
                                                tb.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                        if (TempPclistNew.Count == 0)
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    else
                                    {
                                        #region SAPSpProcType Condition
                                        if (tb.ID.Contains("txtBaseqty"))
                                        {
                                            if (HdnSAPSpProcType.Value == "30")
                                            {
                                                if (HdnAcsTabMatCost.Value == "False")
                                                {
                                                    tb.Text = "1";
                                                }
                                            }
                                        }
                                        #endregion
                                        else
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                    #endregion

                                    if (tb.ID.Contains("txtStandardRate/HR"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    if (Session["IsRevision"] != null)
                                    {
                                        if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == false)
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (Session["IsRevision"].ToString() == "1" && AcsTabProcCost == false && IsUseMachineAmortize == true)
                                        {
                                            if (Session["TotColProcOld"] != null)
                                            {
                                                if ((ColmNoProc - 1) <= TotColProcOld)
                                                {
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            rowcount++;
                        }
                        #endregion Loop
                    }
                    Session["TablePc"] = TablePC;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        /// <summary>
        /// Sub Material cost Dynamic Tables
        /// </summary>
        /// <param name="ColumnType"></param>
        /// 
        private void CreateDynamicSubMaterialDT(int ColumnType)
        {
            try
            {
                DataTable DtDynamicSubMaterialsFields = new DataTable();
                DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                if (ColumnType == 0)
                {
                    

                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableSMC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                            Hearderrow.Cells.Add(tCell1);
                        }
                        else
                        {
                            LinkButton BtnDel = new LinkButton();
                            int TtlField = DtDynamicSubMaterialsFields.Rows.Count;
                            BtnDel.ID = "BtnDelSubMatCost" + ColmNoSubMat;
                            BtnDel.Text = "Delete";
                            BtnDel.ForeColor = Color.Yellow;
                            BtnDel.OnClientClick = "DelSubMatCost('" + ColmNoSubMat + "','" + TtlField + "'); return false;";

                            Label LbToolOrMachineAmor = new Label();
                            LbToolOrMachineAmor.ID = "LbToolOrMachineAmor" + ColmNoSubMat;
                            LbToolOrMachineAmor.ForeColor = Color.White;

                            

                            tCell1.Controls.Add(BtnDel);
                            Hearderrow.Cells.Add(tCell1);
                            ColmNoSubMat++;

                            if (Session["AcsTabSubMatCost"] != null)
                            {
                                if ((bool)Session["AcsTabSubMatCost"] == false)
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");
                                }
                            }

                            #region amortize condition
                            DataTable dtTollAmot = new DataTable();
                            DataTable dtMachineAmor = new DataTable();
                            if (Session["VndMachineAmortize"] != null)
                            {
                                dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                            }

                            if (Session["VndToolAmortize"] != null)
                            {
                                dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                if (dtTollAmot.Rows.Count > 0 && (cellCtr - 1) < dtTollAmot.Rows.Count)
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");

                                    LbToolOrMachineAmor.Text = "Tool Amortize";
                                    tCell1.Controls.Add(LbToolOrMachineAmor);
                                }
                                else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > (cellCtr - 1) && HdnIsUseMachineAmor.Value == "1")
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");

                                    LbToolOrMachineAmor.Text = "Machine Amortize";
                                    tCell1.Controls.Add(LbToolOrMachineAmor);
                                }
                            }
                            else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && HdnIsUseMachineAmor.Value == "1")
                            {
                                if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (cellCtr - 1))
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");

                                    LbToolOrMachineAmor.Text = "Machine Amortize";
                                    tCell1.Controls.Add(LbToolOrMachineAmor);
                                }
                            }
                            #endregion
                        }
                        //TableCell tCell1 = new TableCell();
                        //Label lb1 = new Label();
                        //tCell1.Controls.Add(lb1);
                        //if (cellCtr == 0)
                        //    tCell1.Text = "Field Name";
                        //else
                        //    tCell1.Text = "Material Cost";
                        //Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableSMC.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)"); ;
                                lb.Width = 240;
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                if (DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Sub-Mat/T&JDescription")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubMat" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["SubMatDdl"];
                                    ddl.DataTextField = "SubMaterial";
                                    ddl.DataValueField = "SubMaterial";
                                    //ddl.Attributes.Add("onchange", "SubMaterial(" + (cellCtr - 1) + ")");
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 1)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", ""));
                                    }
                                    #region retrive data
                                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                    {
                                        var tempPClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");
                                                var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                {
                                                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                }
                                                else
                                                {
                                                    DataTable dtTollAmot = new DataTable();
                                                    if (Session["VndToolAmortize"] != null)
                                                    {
                                                        dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                                        if (dtTollAmot.Rows.Count > 0 && dtTollAmot.Rows.Count >= (cellCtr - 1))
                                                        {
                                                            string ItemValue = dtTollAmot.Rows[cellCtr - 1]["Amortize_Tool_ID"].ToString();
                                                            string ItemText = (dtTollAmot.Rows[cellCtr - 1]["Amortize_Tool_ID"].ToString() + "-" + dtTollAmot.Rows[cellCtr - 1]["Amortize_Tool_Desc"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                    }

                                                    DataTable dtMachineAmor = new DataTable();
                                                    if (Session["VndMachineAmortize"] != null)
                                                    {
                                                        dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                                        if (dtTollAmot.Rows.Count > 0 && dtTollAmot.Rows.Count < (cellCtr - 1) && dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (cellCtr - 1))
                                                        {
                                                            string ItemValue = dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString();
                                                            string ItemText = (dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString() + "-" + dtMachineAmor.Rows[cellCtr - 1]["MachineDescription"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                        else if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (cellCtr - 1) && Session["VndToolAmortize"] == null && HdnIsUseMachineAmor.Value == "1")
                                                        {
                                                            string ItemValue = dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString();
                                                            string ItemText = (dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString() + "-" + dtMachineAmor.Rows[cellCtr - 1]["MachineDescription"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                        else if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (cellCtr - 1) && dtTollAmot.Rows.Count == 0 && HdnIsUseMachineAmor.Value == "1")
                                                        {
                                                            string ItemValue = dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString();
                                                            string ItemText = (dtMachineAmor.Rows[cellCtr - 1]["Vend_MachineID"].ToString() + "-" + dtMachineAmor.Rows[cellCtr - 1]["MachineDescription"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion retrive data

                                    tCell.Controls.Add(ddl);
                                }
                                else {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                    //tb.Attributes.Add("disabled", "disabled");
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    string txtID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);

                                    if (tb.ID.Contains("txtSub-Mat/T&JDescription"))
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }

                                    if (tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }

                                    if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    #region Data Store and Retrieve
                                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                    {
                                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempSMClist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                if (tb.ID.Contains("txtTotalSub-Mat/T&JCost/pcs"))
                                                {
                                                    if (HdnFirsLoad.Value == "1")
                                                    {
                                                        if (hdnMassRevision.Value != "")
                                                        {
                                                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                        }
                                                        else
                                                        {
                                                            tb.Text = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                }
                                                break;
                                            }

                                        }
                                    }
                                    #endregion

                                    #region amortize condition
                                    DataTable dtTollAmot = new DataTable();
                                    DataTable dtMachineAmor = new DataTable();
                                    if (Session["VndMachineAmortize"] != null)
                                    {
                                        dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                    }

                                    if (Session["VndToolAmortize"] != null)
                                    {
                                        dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                        if (dtTollAmot.Rows.Count > 0 && (i - 1) <= dtTollAmot.Rows.Count)
                                        {
                                            tb.Enabled = false;
                                        }
                                        else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > (i - 1) && HdnIsUseMachineAmor.Value == "1")
                                        {
                                            tb.Enabled = false;
                                        }
                                    }
                                    else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && HdnIsUseMachineAmor.Value == "1")
                                    {
                                        if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (i - 1))
                                        {
                                            tb.Enabled = false;
                                        }
                                    }
                                    #endregion

                                    tCell.Controls.Add(tb);
                                }
                                
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    // TableMat = Table1;
                    Session["TableSMC"] = TableSMC;
                }
                else
                {
                    //  int Rowscount = -1;
                    TableSMC = (Table)Session["TableSMC"];

                    int CellsCount = ColumnType;

                    //int tempcount = 1;
                    for (int i = 1; i < CellsCount; i++)
                    {

                        // tempcount++;

                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                        var TempSMClistNew = tempSMClist;

                        TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                LinkButton BtnDel = new LinkButton();
                                int TtlField = DtDynamicSubMaterialsFields.Rows.Count;
                                BtnDel.ID = "BtnDelSubMatCost" + ColmNoSubMat;
                                BtnDel.Text = "Delete";
                                BtnDel.ForeColor = Color.Yellow;
                                BtnDel.OnClientClick = "DelSubMatCost(" + ColmNoSubMat + "," + TtlField + "); return false;";

                                Label LbToolOrMachineAmor = new Label();
                                LbToolOrMachineAmor.ID = "LbToolOrMachineAmor" + ColmNoSubMat;
                                LbToolOrMachineAmor.ForeColor = Color.White;
                                
                                tCell.Controls.Add(BtnDel);
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                ColmNoSubMat++;

                                if (Session["AcsTabSubMatCost"] != null)
                                {
                                    if ((bool)Session["AcsTabSubMatCost"] == false)
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");
                                    }
                                }

                                #region amortize condition
                                DataTable dtTollAmot = new DataTable();
                                DataTable dtMachineAmor = new DataTable();
                                if (Session["VndMachineAmortize"] != null)
                                {
                                    dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                }

                                if (Session["VndToolAmortize"] != null)
                                {
                                    dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                    if (dtTollAmot.Rows.Count > 0 && i < dtTollAmot.Rows.Count)
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");

                                        LbToolOrMachineAmor.Text = "Tool Amortize";
                                        tCell.Controls.Add(LbToolOrMachineAmor);
                                    }
                                    else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > i && HdnIsUseMachineAmor.Value == "1")
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");

                                        LbToolOrMachineAmor.Text = "Machine Amortize";
                                        tCell.Controls.Add(LbToolOrMachineAmor);
                                    }
                                }
                                else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && HdnIsUseMachineAmor.Value == "1")
                                {
                                    if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count > i)
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");

                                        LbToolOrMachineAmor.Text = "Machine Amortize";
                                        tCell.Controls.Add(LbToolOrMachineAmor);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                if (DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Sub-Mat/T&JDescription")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlSubMat" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["SubMatDdl"];
                                    ddl.DataTextField = "SubMaterial";
                                    ddl.DataValueField = "SubMaterial";
                                    //ddl.Attributes.Add("onchange", "SubMaterial(" + (cellCtr - 1) + ")");
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 1)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", ""));
                                    }
                                    tCell.Controls.Add(ddl);
                                    TableSMC.Rows[cellCtr].Cells.Add(tCell);

                                    #region Data Retrieve and assign from Storage
                                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                    {

                                        for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                        {

                                            if (ii == (cellCtr - 1))
                                            {
                                                string ddltemval = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                                var ddlcheck = ddl.Items.FindByText(TempSMClistNew[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                {
                                                    ddl.Items.FindByText(TempSMClistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                }
                                                else
                                                {
                                                    DataTable dtTollAmot = new DataTable();
                                                    DataTable dtMachineAmor = new DataTable();

                                                    if (Session["VndMachineAmortize"] != null)
                                                    {
                                                        dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                                    }

                                                    if (Session["VndToolAmortize"] != null)
                                                    {
                                                        dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                                        if (dtTollAmot.Rows.Count > 0 && dtTollAmot.Rows.Count > i)
                                                        {
                                                            string ItemValue = dtTollAmot.Rows[i]["Amortize_Tool_ID"].ToString();
                                                            string ItemText = (dtTollAmot.Rows[i]["Amortize_Tool_ID"].ToString() + "-" + dtTollAmot.Rows[i]["Amortize_Tool_Desc"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                        else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > i && HdnIsUseMachineAmor.Value == "1")
                                                        {
                                                            int r = i - dtTollAmot.Rows.Count;
                                                            if (r >= 0 && r < dtMachineAmor.Rows.Count)
                                                            {
                                                                string ItemValue = dtMachineAmor.Rows[r]["Vend_MachineID"].ToString();
                                                                string ItemText = (dtMachineAmor.Rows[r]["Vend_MachineID"].ToString() + "-" + dtMachineAmor.Rows[r]["MachineDescription"].ToString());
                                                                ListItem lst = new ListItem(ItemText, ItemValue);
                                                                ddl.Items.Insert(ddl.Items.Count, lst);
                                                                ddl.SelectedValue = ItemValue;
                                                                ddl.Enabled = false;
                                                            }
                                                        }
                                                    }
                                                    else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && HdnIsUseMachineAmor.Value == "1")
                                                    {
                                                        if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count > i)
                                                        {
                                                            string ItemValue = dtMachineAmor.Rows[i]["Vend_MachineID"].ToString();
                                                            string ItemText = (dtMachineAmor.Rows[i]["Vend_MachineID"].ToString() + "-" + dtMachineAmor.Rows[i]["MachineDescription"].ToString());
                                                            ListItem lst = new ListItem(ItemText, ItemValue);
                                                            ddl.Items.Insert(ddl.Items.Count, lst);
                                                            ddl.SelectedValue = ItemValue;
                                                            ddl.Enabled = false;
                                                        }
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                    
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");

                                    #region amortize condition
                                    DataTable dtTollAmot = new DataTable();
                                    DataTable dtMachineAmor = new DataTable();
                                    if (Session["VndMachineAmortize"] != null)
                                    {
                                        dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                    }

                                    if (Session["VndToolAmortize"] != null)
                                    {
                                        dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                        if (dtTollAmot.Rows.Count > 0 && i < dtTollAmot.Rows.Count)
                                        {
                                            tb.Enabled = false;
                                        }
                                        else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > i && HdnIsUseMachineAmor.Value == "1")
                                        {
                                            tb.Enabled = false;
                                        }
                                    }
                                    else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && HdnIsUseMachineAmor.Value == "1")
                                    {
                                        if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count > i)
                                        {
                                            tb.Enabled = false;
                                        }
                                    }
                                    #endregion

                                    string txtID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);

                                    if (tb.ID.Contains("txtSub-Mat/T&JDescription"))
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }

                                    if (tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }
                                    if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    if (tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                                    {
                                        TableSMC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

                                    }
                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                        TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                    }

                                    #region Data Retrieve and assign from Storage
                                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                    {

                                        for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                        {

                                            if (ii == (cellCtr - 1))
                                            {
                                                if (tb.ID.Contains("txtTotalSub-Mat/T&JCost/pcs"))
                                                {
                                                    if (HdnFirsLoad.Value == "1")
                                                    {
                                                        tb.Text = "";
                                                    }
                                                    else
                                                    {
                                                        tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    Session["TableSMC"] = TableSMC;
                }
                UpdatePanel5.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            
        }
        ///old
        //private void CreateDynamicSubMaterialDT(int ColumnType)
        //{
        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;
        //        if (DtDynamicSubMaterialsDetails == null)
        //            DtDynamicSubMaterialsDetails = new DataTable();
        //        if (DtDynamicSubMaterialsDetails.Rows.Count > 0)
        //        {
        //            TableRow Hearderrow = new TableRow();

        //            TableSMC.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                //else
        //                //    tCell1.Text = "Material Cost";

        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            foreach (DataRow row in DtDynamicSubMaterialsFields.Rows)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableSMC.Rows.Add(tRow);
        //                for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (cellCtr == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        tCell.Text = row.ItemArray[0].ToString();
        //                        tRow.Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                        if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                        {
        //                            tb.Attributes.Add("disabled", "disabled");
        //                        }
        //                        tCell.Controls.Add(tb);
        //                        tRow.Cells.Add(tCell);
        //                    }
        //                }

        //                if (rowcount % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcount++;
        //            }
        //        }
        //        else
        //        {
        //            int rowcountnew = 0;

        //            TableRow Hearderrow = new TableRow();

        //            TableSMC.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableSMC.Rows.Add(tRow);

        //                for (int i = 0; i <= 1; i++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (i == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString();
        //                        TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                        if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                        {
        //                            tb.Attributes.Add("disabled", "disabled");
        //                        }
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


        //                        // Data Store and Retrieve
        //                        if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //                        {
        //                            var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //                            for (int ii = 0; ii < tempSMClist.Count; ii++)
        //                            {
        //                                if (ii == (cellCtr - 1))
        //                                {
        //                                    tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //                                    break;
        //                                }

        //                            }
        //                        }


        //                        tCell.Controls.Add(tb);
        //                        TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //                if (rowcountnew % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcountnew++;
        //            }
        //        }
        //        // TableMat = Table1;
        //        Session["TableSMC"] = TableSMC;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableSMC = (Table)Session["TableSMC"];

        //        int CellsCount = ColumnType;

        //        //int tempcount = 1;
        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //            // tempcount++;

        //            var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //            var TempSMClistNew = tempSMClist;

        //            if (CellsCount == 2 && tempSMClist.Count <= (DtDynamicSubMaterialsFields.Rows.Count + 1))
        //                TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i)) * (DtDynamicSubMaterialsFields.Rows.Count + 1))).ToList();
        //            else if (CellsCount == 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount != (i + 1))
        //                TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamicSubMaterialsFields.Rows.Count)).ToList();
        //            else if (CellsCount >= 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount == (i + 1))
        //                TempSMClistNew = TempSMClistNew.Skip(((CellsCount) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();
        //            else if (i >= 1 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount != (i + 1))
        //                TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();



        //            for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                    if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                    {
        //                        tb.Attributes.Add("disabled", "disabled");
        //                    }

        //                    if (tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                    {
        //                        TableSMC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    // Data Retrieve and assign from Storage

        //                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //                    {

        //                        for (int ii = 0; ii < TempSMClistNew.Count; ii++)
        //                        {

        //                            if (ii == (cellCtr - 1))
        //                            {

        //                                tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        Session["TableSMC"] = TableSMC;
        //    }

        //    //  var Ss = divhdnSMC.InnerHtml;

        //}


        /// <summary>
        /// Other Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicOthersCostDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                if (ColumnType == 0)
                {
                    #region old code (condition with DtDynamicOtherCostsDetails never used)
                    //int rowcount = 0;
                    //if (DtDynamicOtherCostsDetails == null)
                    //    DtDynamicOtherCostsDetails = new DataTable();
                    //if (DtDynamicOtherCostsDetails.Rows.Count > 0)
                    //{
                    //    TableRow Hearderrow = new TableRow();

                    //    TableOthers.Rows.Add(Hearderrow);
                    //    for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
                    //    {
                    //        TableCell tCell1 = new TableCell();

                    //        if (cellCtr == 0)
                    //        {
                    //            tCell1.Text = "Field Name";
                    //            Hearderrow.Cells.Add(tCell1);
                    //        }
                    //        else
                    //        {
                    //            LinkButton BtnDel = new LinkButton();
                    //            int TtlField = DtDynamicOtherCostsFields.Rows.Count;
                    //            BtnDel.ID = "BtnDelOthCost" + ColmNoOth;
                    //            BtnDel.Text = "Delete";
                    //            BtnDel.ForeColor = Color.Yellow;
                    //            BtnDel.OnClientClick = "DelOthCost('" + ColmNoOth + "','" + TtlField + "');return false;";
                    //            tCell1.Controls.Add(BtnDel);
                    //            ColmNoOth++;
                    //        }

                    //        //TableCell tCell1 = new TableCell();
                    //        //Label lb1 = new Label();
                    //        //tCell1.Controls.Add(lb1);
                    //        //if (cellCtr == 0)
                    //        //    tCell1.Text = "Field Name";
                    //        ////else
                    //        ////    tCell1.Text = "Material Cost";
                    //        //Hearderrow.Cells.Add(tCell1);
                    //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                    //        Hearderrow.ForeColor = Color.White;
                    //    }

                    //    foreach (DataRow row in DtDynamicOtherCostsFields.Rows)
                    //    {
                    //        TableRow tRow = new TableRow();
                    //        TableOthers.Rows.Add(tRow);
                    //        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
                    //        {
                    //            TableCell tCell = new TableCell();
                    //            if (cellCtr == 0)
                    //            {
                    //                Label lb = new Label();
                    //                tCell.Controls.Add(lb);
                    //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
                    //                tRow.Cells.Add(tCell);
                    //            }
                    //            else
                    //            {
                    //                TextBox tb = new TextBox();
                    //                tb.BorderStyle = BorderStyle.None;
                    //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                    //                tb.Attributes.Add("autocomplete", "off");
                    //                tb.Style.Add("text-transform", "uppercase");

                    //                if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                    //                {
                    //                    tb.Attributes.Add("disabled", "disabled");
                    //                }


                    //                // Data Store and Retrieve
                    //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                    //                {
                    //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                    //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                    //                    {
                    //                        if (ii == (cellCtr - 1))
                    //                        {

                    //                            if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                    //                            {
                    //                                if (HdnFirsLoad.Value == "1")
                    //                                {
                    //                                    tb.Text = "";
                    //                                }
                    //                                else
                    //                                {
                    //                                    tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                    //                            }
                    //                            break;
                    //                        }

                    //                    }
                    //                }


                    //                tCell.Controls.Add(tb);
                    //                tRow.Cells.Add(tCell);
                    //            }
                    //        }

                    //        if (rowcount % 2 == 0)
                    //        {
                    //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                    //            tRow.BackColor = Color.White;
                    //        }
                    //        else
                    //        {
                    //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                    //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                    //        }
                    //        rowcount++;
                    //    }
                    //}
                    //else
                    //{
                    //    int rowcountnew = 0;

                    //    TableRow Hearderrow = new TableRow();

                    //    TableOthers.Rows.Add(Hearderrow);
                    //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    //    {
                    //        TableCell tCell1 = new TableCell();
                    //        Label lb1 = new Label();
                    //        tCell1.Controls.Add(lb1);
                    //        if (cellCtr == 0)
                    //        {
                    //            tCell1.Text = "Field Name";
                    //            Hearderrow.Cells.Add(tCell1);
                    //        }
                    //        else
                    //        {
                    //            LinkButton BtnDel = new LinkButton();
                    //            int TtlField = DtDynamicOtherCostsFields.Rows.Count;
                    //            BtnDel.ID = "BtnDelOthCost" + ColmNoOth;
                    //            BtnDel.Text = "Delete";
                    //            BtnDel.ForeColor = Color.Yellow;
                    //            BtnDel.OnClientClick = "DelOthCost('" + ColmNoOth + "','" + TtlField + "'); return false;";

                    //            tCell1.Controls.Add(BtnDel);
                    //            Hearderrow.Cells.Add(tCell1);
                    //            ColmNoOth++;
                    //        }
                    //        //TableCell tCell1 = new TableCell();
                    //        //Label lb1 = new Label();
                    //        //tCell1.Controls.Add(lb1);
                    //        //if (cellCtr == 0)
                    //        //    tCell1.Text = "Field Name";
                    //        ////else
                    //        ////    tCell1.Text = "Material Cost";
                    //        //Hearderrow.Cells.Add(tCell1);
                    //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                    //        Hearderrow.ForeColor = Color.White;
                    //    }

                    //    // Table1 = (Table)Session["Table"];
                    //    for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                    //    {
                    //        TableRow tRow = new TableRow();
                    //        TableOthers.Rows.Add(tRow);

                    //        for (int i = 0; i <= 1; i++)
                    //        {
                    //            TableCell tCell = new TableCell();
                    //            if (i == 0)
                    //            {
                    //                Label lb = new Label();
                    //                tCell.Controls.Add(lb);
                    //                lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
                    //                lb.Width = 240;
                    //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                    //            }
                    //            else
                    //            {
                    //                TextBox tb = new TextBox();
                    //                tb.BorderStyle = BorderStyle.None;
                    //                tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                    //                tb.Attributes.Add("autocomplete", "off");
                    //                tb.Style.Add("text-transform", "uppercase");
                    //                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                    //                //{

                    //                //tb.Attributes.Add("disabled", "disabled");
                    //                //}

                    //                if (tb.ID.Contains("txtTotalOtherItemCost/pcs"))
                    //                {
                    //                    tb.Attributes.Add("disabled", "disabled");
                    //                }

                    //                // Data Store and Retrieve
                    //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                    //                {
                    //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                    //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                    //                    {
                    //                        if (ii == (cellCtr - 1))
                    //                        {

                    //                            if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                    //                            {
                    //                                if (HdnFirsLoad.Value == "1")
                    //                                {
                    //                                    tb.Text = "";
                    //                                }
                    //                                else
                    //                                {
                    //                                    tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                    //                            }
                    //                            break;
                    //                        }

                    //                    }
                    //                }


                    //                tCell.Controls.Add(tb);
                    //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                    //            }
                    //        }
                    //        if (rowcountnew % 2 == 0)
                    //        {
                    //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                    //            tRow.BackColor = Color.White;
                    //        }
                    //        else
                    //        {
                    //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                    //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                    //        }
                    //        rowcountnew++;
                    //    }
                    //}
                    #endregion old code (condition with DtDynamicOtherCostsDetails never used)

                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableOthers.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                            Hearderrow.Cells.Add(tCell1);
                        }
                        else
                        {
                            LinkButton BtnDel = new LinkButton();
                            int TtlField = DtDynamicOtherCostsFields.Rows.Count;
                            BtnDel.ID = "BtnDelOthCost" + ColmNoOth;
                            BtnDel.Text = "Delete";
                            BtnDel.ForeColor = Color.Yellow;
                            BtnDel.OnClientClick = "DelOthCost('" + ColmNoOth + "','" + TtlField + "'); return false;";

                            tCell1.Controls.Add(BtnDel);
                            Hearderrow.Cells.Add(tCell1);
                            ColmNoOth++;

                            if (Session["AcsTabOthMatCost"] != null)
                            {
                                if ((bool)Session["AcsTabOthMatCost"] == false)
                                {
                                    BtnDel.Attributes.Add("style", "display:none;");
                                }
                            }
                        }
                        //TableCell tCell1 = new TableCell();
                        //Label lb1 = new Label();
                        //tCell1.Controls.Add(lb1);
                        //if (cellCtr == 0)
                        //    tCell1.Text = "Field Name";
                        ////else
                        ////    tCell1.Text = "Material Cost";
                        //Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableOthers.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                if (hdnLayoutScreen.Value == "Layout7")
                                {
                                    lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/UOM");
                                }
                                else
                                {
                                    lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                lb.Width = 240;
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                if (DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "ItemsDescription")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlOthCost" + (cellCtr - 1);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["OthCostDdl"];
                                    ddl.DataTextField = "OtherCost";
                                    ddl.DataValueField = "OtherCost";
                                    //ddl.Attributes.Add("onchange", "SubMaterial(" + (cellCtr - 1) + ")");
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 1)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", ""));
                                    }
                                    #region retrive data
                                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                    {
                                        var tempPClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempPClist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");
                                                var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                {
                                                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion retrive data

                                    tCell.Controls.Add(ddl);
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    string txtID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);

                                    if (tb.ID.Contains("txtItemsDescription"))
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }

                                    if (tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }

                                    if (tb.ID.Contains("txtTotalOtherItemCost/pcs"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    #region Data Store and Retrieve
                                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                    {
                                        var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempSMClist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {

                                                if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                                {
                                                    if (HdnFirsLoad.Value == "1")
                                                    {
                                                        if (hdnMassRevision.Value != "")
                                                        {
                                                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                        }
                                                        else
                                                        {
                                                            tb.Text = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                                }
                                                break;
                                            }

                                        }
                                    }
                                    #endregion

                                    tCell.Controls.Add(tb);
                                }
                                
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["TableOthers"] = TableOthers;
                }
                else
                {
                    //  int Rowscount = -1;
                    TableOthers = (Table)Session["TableOthers"];


                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {

                        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                        var tempOtherlistNew = tempOtherlist;

                        //if (CellsCount == 2 && tempOtherlist.Count <= (DtDynamicOtherCostsFields.Rows.Count + 1))
                        //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i)) * (DtDynamicOtherCostsFields.Rows.Count + 1))).ToList();
                        ////else if (CellsCount ==  && tempOtherlist.Count > 6)
                        ////    tempOtherlistNew = tempOtherlistNew.Skip((CellsCount - i)  * 5).ToList();
                        //else if (CellsCount == 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
                        //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i + 1)) * DtDynamicOtherCostsFields.Rows.Count)).ToList();
                        //else if (CellsCount >= 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount == (i + 2))
                        //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();
                        //else if (i >= 1 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
                        //    tempOtherlistNew = tempOtherlistNew.Skip(i * (DtDynamicOtherCostsFields.Rows.Count)).ToList();


                        tempOtherlistNew = tempOtherlist.Skip(i * DtDynamicOtherCostsFields.Rows.Count).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                LinkButton BtnDel = new LinkButton();
                                int TtlField = DtDynamicOtherCostsFields.Rows.Count;
                                BtnDel.ID = "BtnDelOthCost" + ColmNoOth;
                                BtnDel.Text = "Delete";
                                BtnDel.ForeColor = Color.Yellow;
                                BtnDel.OnClientClick = "DelOthCost(" + ColmNoOth + "," + TtlField + "); return false;";
                                tCell.Controls.Add(BtnDel);
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);

                                ColmNoOth++;

                                if (Session["AcsTabOthMatCost"] != null)
                                {
                                    if ((bool)Session["AcsTabOthMatCost"] == false)
                                    {
                                        BtnDel.Attributes.Add("style", "display:none;");
                                    }
                                }
                            }
                            else
                            {
                                if (DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "ItemsDescription")
                                {
                                    DropDownList ddl = new DropDownList();
                                    ddl.ID = "dynamicddlOthCost" + (i);
                                    ddl.Style.Add("min-width", "300px;");
                                    ddl.DataSource = Session["OthCostDdl"];
                                    ddl.DataTextField = "OtherCost";
                                    ddl.DataValueField = "OtherCost";
                                    //ddl.Attributes.Add("onchange", "SubMaterial(" + (cellCtr - 1) + ")");
                                    ddl.DataBind();
                                    if (ddl.Items.Count > 1)
                                    {
                                        ddl.Items.Insert(0, new ListItem("--Select--", ""));
                                    }

                                    #region Data Retrieve and assign from Storage
                                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                    {

                                        for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                        {

                                            if (ii == (cellCtr - 1))
                                            {
                                                string ddltemval = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                                var ddlcheck = ddl.Items.FindByText(tempOtherlistNew[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                {
                                                    ddl.Items.FindByText(tempOtherlistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion

                                    tCell.Controls.Add(ddl);
                                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "150px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    string txtID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);

                                    if (tb.ID.Contains("txtItemsDescription"))
                                    {
                                        tb.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tb.Style.Add("text-align", "right");
                                    }

                                    if (tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                        tb.Attributes.Add("onchange", "ValOnlyNo('" + txtID + "','RedirectTxt')");
                                    }
                                    if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                    {
                                        TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

                                    }

                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                        TableOthers.Rows[cellCtr].Cells.Add(tCell);
                                    }

                                    if (tb.ID.Contains("txtTotalOtherItemCost/pcs"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                    //tb.Attributes.Add("disabled", "disabled");


                                    // Data Retrieve and assign from Storage
                                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                    {
                                        for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {

                                                if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                                {
                                                    if (HdnFirsLoad.Value == "1")
                                                    {
                                                        tb.Text = "";
                                                    }
                                                    else
                                                    {
                                                        tb.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                }
                                                else
                                                {
                                                    tb.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                                }
                                                break;
                                            }

                                        }
                                    }
                                }

                                
                            }
                        }
                    }
                    Session["TableOthers"] = TableOthers;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        /// <summary>
        /// Unit Price Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicUnitDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                if (ColumnType == 0)
                {
                    #region old code (condition with DtDynamicOtherCostsDetails never used)
                    //int rowcount = 0;
                    //if (DtDynamicUnitDetails == null)
                    //    DtDynamicUnitDetails = new DataTable();
                    //if (DtDynamicUnitDetails.Rows.Count > 0)
                    //{
                    //    TableRow Hearderrow = new TableRow();

                    //    TableUnit.Rows.Add(Hearderrow);
                    //    for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
                    //    {
                    //        TableCell tCell1 = new TableCell();
                    //        Label lb1 = new Label();
                    //        tCell1.Controls.Add(lb1);
                    //        if (cellCtr == 0)
                    //            tCell1.Text = "Field Name";
                    //        //else
                    //        //    tCell1.Text = "Material Cost";

                    //        Hearderrow.Cells.Add(tCell1);
                    //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                    //        Hearderrow.ForeColor = Color.White;
                    //    }

                    //    foreach (DataRow row in DtDynamicUnitFields.Rows)
                    //    {
                    //        TableRow tRow = new TableRow();
                    //        TableUnit.Rows.Add(tRow);
                    //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
                    //        {
                    //            TableCell tCell = new TableCell();
                    //            if (cellCtr == 0)
                    //            {
                    //                Label lb = new Label();
                    //                tCell.Controls.Add(lb);
                    //                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                    //                {
                    //                    tCell.Text = "Total Process Cost/pc";
                    //                }
                    //                else
                    //                {
                    //                    tCell.Text = row.ItemArray[0].ToString().Replace("/pcs -", "/pc");
                    //                }
                    //                tRow.Cells.Add(tCell);
                    //            }
                    //            else
                    //            {
                    //                TextBox tb = new TextBox();
                    //                tb.BorderStyle = BorderStyle.None;
                    //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                    //                tb.Attributes.Add("autocomplete", "off");
                    //                tb.Style.Add("text-transform", "uppercase");
                    //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                    //                tCell.Controls.Add(tb);
                    //                tRow.Cells.Add(tCell);
                    //            }
                    //        }

                    //        if (rowcount % 2 == 0)
                    //        {
                    //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                    //            tRow.BackColor = Color.White;
                    //        }
                    //        else
                    //        {
                    //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                    //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                    //        }
                    //        rowcount++;
                    //    }
                    //}
                    //else
                    //{
                    //    int rowcountnew = 0;

                    //    TableRow Hearderrow = new TableRow();

                    //    TableUnit.Rows.Add(Hearderrow);
                    //    for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
                    //    {
                    //        TableCell tCell1 = new TableCell();
                    //        Label lb1 = new Label();
                    //        tCell1.Controls.Add(lb1);
                    //        if (cellCtr == 0)
                    //        {
                    //            tCell1.Text = "Field Name";
                    //        }
                    //        else if (cellCtr == 2)
                    //        {
                    //            tCell1.Text = "Profit (%)";
                    //        }
                    //        else if (cellCtr == 3)
                    //        {
                    //            tCell1.Text = "Discount (%)";
                    //        }
                    //        else if (cellCtr == 4)
                    //        {
                    //            tCell1.Text = "Final Quote Price/pc";
                    //        }
                    //        else if (cellCtr == 5)
                    //        {
                    //            tCell1.Text = "Net Profit/Discount";
                    //        }
                    //        Hearderrow.Cells.Add(tCell1);
                    //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                    //        Hearderrow.ForeColor = Color.White;
                    //    }

                    //    // Table1 = (Table)Session["Table"];
                    //    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    //    {
                    //        TableRow tRow = new TableRow();
                    //        TableUnit.Rows.Add(tRow);

                    //        for (int i = 0; i <= 5; i++)
                    //        {
                    //            TableCell tCell = new TableCell();
                    //            if (i == 0)
                    //            {
                    //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                if (a.Contains("Profit(%)"))
                    //                {
                    //                    break;
                    //                }
                    //                else if (a.Contains("Discount(%)"))
                    //                {
                    //                    break;
                    //                }
                    //                else if (a.Contains("FinalQuotePrice/pcs"))
                    //                {
                    //                    break;
                    //                }
                    //                else
                    //                {
                    //                    Label lb = new Label();
                    //                    tCell.Controls.Add(lb);
                    //                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                    //                    {
                    //                        lb.Text = "Total Process Cost/pc";
                    //                    }
                    //                    else
                    //                    {
                    //                        lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                    //                    }
                    //                    lb.Width = 240;
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                }
                    //            }
                    //            else if (i == 1)
                    //            {
                    //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                if (a.Contains("Profit(%)"))
                    //                {
                    //                    break;
                    //                }
                    //                else if (a.Contains("Discount(%)"))
                    //                {
                    //                    break;
                    //                }
                    //                else if (a.Contains("FinalQuotePrice/pcs"))
                    //                {
                    //                    break;
                    //                }
                    //                else
                    //                {
                    //                    TextBox tb = new TextBox();
                    //                    tb.BorderStyle = BorderStyle.None;
                    //                    tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                    //                    tCell.Controls.Add(tb);
                    //                    tb.Attributes.Add("autocomplete", "off");
                    //                    tb.Style.Add("text-transform", "uppercase");
                    //                    tb.Attributes.Add("disabled", "disabled");
                    //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                    //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                    //                    {
                    //                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                    //                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                    //                        {
                    //                            if (ii == (cellCtr - 1))
                    //                            {
                    //                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                    //                                break;
                    //                            }
                    //                        }
                    //                    }

                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                }
                    //            }
                    //            else if (i == 2)
                    //            {
                    //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                if (a.Contains("TotalProcessesCost/pcs-"))
                    //                {
                    //                    TextBox tb = new TextBox();
                    //                    tb.BorderStyle = BorderStyle.None;
                    //                    tb.ID = "txtProfit(%)0";
                    //                    tCell.Controls.Add(tb);
                    //                    tb.Attributes.Add("autocomplete", "off");
                    //                    tb.Style.Add("text-transform", "uppercase");
                    //                    tb.Attributes.Add("onkeydown", "return isNumberKey(event);");

                    //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                    //                    {
                    //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                    //                        {
                    //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                            if (zz.Contains("Profit(%)"))
                    //                            {
                    //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                    //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                    //                                {
                    //                                    if (ii == (z - 1))
                    //                                    {
                    //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                    //                                        break;
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                }
                    //                else
                    //                {
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                    //                }

                    //            }
                    //            else if (i == 3)
                    //            {
                    //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                if (a.Contains("TotalProcessesCost/pcs-"))
                    //                {
                    //                    TextBox tb = new TextBox();
                    //                    tb.BorderStyle = BorderStyle.None;
                    //                    tb.ID = "txtDiscount(%)0";
                    //                    tCell.Controls.Add(tb);
                    //                    tb.Attributes.Add("autocomplete", "off");
                    //                    tb.Style.Add("text-transform", "uppercase");
                    //                    tb.Attributes.Add("onkeydown", "return isNumberKey(event);");

                    //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                    //                    {
                    //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                    //                        {
                    //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                            if (zz.Contains("Discount(%)"))
                    //                            {
                    //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                    //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                    //                                {
                    //                                    if (ii == (z - 1))
                    //                                    {
                    //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                    //                                        break;
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                }
                    //                else
                    //                {
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                    //                }
                    //            }
                    //            else if (i == 4)
                    //            {
                    //                TextBox tb = new TextBox();
                    //                tb.BorderStyle = BorderStyle.None;
                    //                tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
                    //                tCell.Controls.Add(tb);
                    //                tb.Attributes.Add("autocomplete", "off");
                    //                tb.Style.Add("text-transform", "uppercase");
                    //                tb.Attributes.Add("disabled", "disabled");
                    //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                    //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //            }
                    //            else if (i == 5)
                    //            {
                    //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                    //                if (a.Contains("GrandTotalCost/pcs"))
                    //                {
                    //                    TextBox tb = new TextBox();
                    //                    tb.BorderStyle = BorderStyle.None;
                    //                    tb.ID = "txtNetProfit(%)0";
                    //                    tCell.Controls.Add(tb);
                    //                    tb.Attributes.Add("autocomplete", "off");
                    //                    tb.Attributes.Add("disabled", "disabled");
                    //                    tb.Style.Add("text-transform", "uppercase");
                    //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                }
                    //                else
                    //                {
                    //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                    //                }
                    //            }
                    //        }

                    //        if (rowcountnew % 2 == 0)
                    //        {
                    //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                    //            tRow.BackColor = Color.White;
                    //        }
                    //        else
                    //        {
                    //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                    //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                    //        }
                    //        rowcountnew++;
                    //    }
                    //}
                    #endregion old code (condition with DtDynamicOtherCostsDetails never used)

                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableUnit.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        else if (cellCtr == 2)
                        {
                            tCell1.Text = "Profit (%)";
                        }
                        else if (cellCtr == 3)
                        {
                            tCell1.Text = "Discount (%)";
                        }
                        else if (cellCtr == 4)
                        {
                            if (hdnLayoutScreen.Value == "Layout7")
                            {
                                tCell1.Text = "Final Quote Price/UOM";
                            }
                            else
                            {
                                tCell1.Text = "Final Quote Price/pc";
                            }
                        }
                        else if (cellCtr == 5)
                        {
                            tCell1.Text = "Net Profit/Discount";
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableUnit.Rows.Add(tRow);

                        for (int i = 0; i <= 5; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    Label lb = new Label();
                                    tCell.Controls.Add(lb);
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            lb.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            lb.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
                                    lb.Width = 240;
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 1)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("disabled", "disabled");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Style.Add("text-align", "right");
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }

                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txtProfit(%)0";
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Attributes.Add("oninput", "validateNumber('txtProfit(%)0')");
                                    tb.Attributes.Add("onchange", "ValOnlyNo('txtProfit(%)0','RedirectTxt')");
                                    tb.Style.Add("text-align", "right");
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Profit(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }

                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txtDiscount(%)0";
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Attributes.Add("oninput", "validateNumber('txtDiscount(%)0')");
                                    tb.Attributes.Add("oninput", "ValOnlyNo('txtDiscount(%)0','RedirectTxt')");
                                    tb.Style.Add("text-align", "right");
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Discount(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }
                            }
                            else if (i == 4)
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
                                tCell.Controls.Add(tb);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Style.Add("min-width", "100px;");
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("disabled", "disabled");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                tb.Style.Add("text-align", "right");
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else if (i == 5)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txtNetProfit(%)0";
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Attributes.Add("disabled", "disabled");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Style.Add("text-align", "right");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }
                            }
                        }

                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }

                    Session["TableUnit"] = TableUnit;
                }
                else
                {
                    //  int Rowscount = -1;
                    TableUnit = (Table)Session["TableUnit"];

                    for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        if (cellCtr == 0)
                        {
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
                            // lb.Text = "Material Cost";
                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Style.Add("min-width", "100px;");
                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                            tCell.Controls.Add(tb);
                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
                        }
                    }


                    Session["TableUnit"] = TableUnit;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }


        }

        private void CreateDynamicUnitDTTmShimano(int ColumnType)
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableUnit.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 4; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        else if (cellCtr == 1)
                        {
                            tCell1.Text = "e-MET Cost";
                        }
                        else if (cellCtr == 2)
                        {
                            tCell1.Text = "GA%";
                        }
                        else if (cellCtr == 3)
                        {
                            tCell1.Text = "Profit%";
                        }
                        else if (cellCtr == 4)
                        {
                            if (hdnLayoutScreen.Value == "Layout7")
                            {
                                tCell1.Text = "Final Quote Price/UOM";
                            }
                            else
                            {
                                tCell1.Text = "Final Quote Price/pc";
                            }
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableUnit.Rows.Add(tRow);

                        for (int i = 0; i <= 4; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    Label lb = new Label();
                                    tCell.Controls.Add(lb);
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            lb.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            lb.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
                                    lb.Width = 240;
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 1)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("disabled", "disabled");
                                    tb.Style.Add("text-align", "right");
                                    //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "GA(%)0";
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Attributes.Add("oninput", "validateNumber('GA(%)0')");
                                    tb.Attributes.Add("oninput", "ValOnlyNo('GA(%)0','RedirectTxt')");
                                    tb.Style.Add("text-align", "right");
                                    tb.Text = GetGA().ToString();
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }

                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txtProfit(%)0";
                                    tCell.Controls.Add(tb);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("min-width", "100px;");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                    tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                    tb.Attributes.Add("oninput", "validateNumber('txtProfit(%)0')");
                                    tb.Attributes.Add("onchange", "ValOnlyNo('txtProfit(%)0','RedirectTxt')");
                                    tb.Style.Add("text-align", "right");
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Profit(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }
                            }
                            else if (i == 4)
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
                                tCell.Controls.Add(tb);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Style.Add("min-width", "100px;");
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("disabled", "disabled");
                                tb.Style.Add("text-align", "right");
                                tb.Attributes.Add("onkeyup", "isincludecomma('" + tb.ID.ToString() + "');");
                                //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                {
                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
                                    {
                                        if (tb.ID.Contains("txtFinalQuotePrice/pcs4"))
                                        {
                                            if (ii >= 7)
                                            {
                                                tb.Text = tempunitlist[7].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                        else if (ii == (cellCtr - 1))
                                        {
                                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }
                                    }
                                }
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }



                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["TableUnit"] = TableUnit;
                }
                else
                {
                    //  int Rowscount = -1;
                    //TableUnit = (Table)Session["TableUnit"];

                    //for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    //{
                    //    TableCell tCell = new TableCell();
                    //    if (cellCtr == 0)
                    //    {
                    //        Label lb = new Label();
                    //        tCell.Controls.Add(lb);
                    //        // lb.Text = "Material Cost";
                    //        tCell.Width = 240;
                    //        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //    }
                    //    else
                    //    {
                    //        TextBox tb = new TextBox();
                    //        tb.BorderStyle = BorderStyle.None;
                    //        tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                    //        tb.Attributes.Add("autocomplete", "off");
                    //        tb.Style.Add("text-transform", "uppercase");
                    //        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                    //        tCell.Controls.Add(tb);
                    //        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    //    }
                    //}
                    Session["TableUnit"] = TableUnit;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void Ddl_SelectedIndexChanged4(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ddl_SelectedIndexChanged3(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ddl_SelectedIndexChanged2(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ddl_SelectedIndexChanged1(object sender, EventArgs e)
        {

            //throw new NotImplementedException();
        }

        private void Ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }



        void btnNewrow_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public class ProcessTemplate
        {
            public string SubProcess { get; set; }
            public string ProcessUOM { get; set; }
        }


        private void GetProcessDetailsbyQuoteDetails(string ProcessGrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());

            try
            {
                MDMCon.Open();

                DataTable Result4 = new DataTable();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = @ProcessGrp and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str4, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrp", txtprocs.Text.ToString().ToUpper());
                    sda.SelectCommand = cmd;
                    sda.Fill(Result4);
                }

                if (Result4.Rows.Count > 0)
                {
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
                }

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();
                
                string str = "";
                if (hdnLayoutScreen.Value == "Layout6" || hdnLayoutScreen.Value == "Layout7")
                {
                    str = "select distinct Process_Grp_code from TPROCESGROUP_LIST where Process_Grp_code = @ProcessGrpCode and DELFLAG = 0 ";
                }
                else
                {
                    str = "Select distinct ProcessGrpCode from TPROCESGROUP_SUBPROCESS Where ProcessGrpCode = @ProcessGrpCode and DELFLAG = 0 ";
                }
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", ProcessGrp);
                    sda.SelectCommand = cmd;
                    sda.Fill(dtProGrp);
                }
                Session["DtDynamicProcessCostsDetails"] = dtProGrp;
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


        private void GetProcessDetailsbyQuoteDetailsWithNoGroup()
        {
            // plant = "2100";
            // Product = "BO";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select CONCAT(ProcessGrpCode , ' - ', ProcessGrpDescription) as ProcessGrpCode, SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS where DELFLAG = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtProGrp);

                grdProcessGrphidden.DataSource = dtProGrp;
                grdProcessGrphidden.DataBind();
                Session["PSGroupwithUOM"] = grdProcessGrphidden.DataSource;
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
            //DtDynamicProcessCostsDetails = dtProGrp;
        }

        // Till this used


        //private void CreateDynamicGrid(DataTable dtMETFields, DataTable dtMaterials)
        //{

        //    foreach (DataRow Fieldrow in dtMETFields.Rows)
        //    {
        //        foreach (DataRow Materials in dtMaterials.Rows)
        //        {
        //            TableRow tRow = new TableRow();
        //            Table1.Rows.Add(tRow);


        //        }
        //    }


        //}

        //private void CreateDynamicTable()
        //{
        //    int rowCnt;
        //    int rowCtr;
        //    int cellCtr;
        //    int cellCnt;


        //    rowCnt = DtDynamic.Rows.Count; //Number of rows
        //    cellCnt = 2; //Number of columns
        //    DataTable dt = new DataTable();





        //    for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
        //    {
        //        TableRow tRow = new TableRow();
        //        Table1.Rows.Add(tRow);

        //        //TableRow tRow = new TableRow();

        //        //DtDynamic.Rows.Add(tRow);



        //        for (cellCtr = 1; cellCtr <= cellCnt; cellCtr++)
        //        {
        //            TableCell tCell = new TableCell();

        //            TextBox tb = new TextBox();
        //            tCell.Controls.Add(tb);
        //            // tCell.Text = "Row " + rowCtr + ", Cell " + cellCtr;
        //            tRow.Cells.Add(tCell);

        //        }
        //        tRow.BorderWidth = 1;
        //        tRow.BorderStyle = BorderStyle.Solid;
        //        tRow.BorderColor = Color.Blue;
        //    }

        //    // grdproces.DataSource = DtDynamic;
        //    // grdproces.DataBind();
        //}



        private void CreateDynamicDataTabletoGrid()
        {
            int rowcount = 0;
            DataTable DtDynamic = new DataTable();
            DtDynamic = (DataTable)Session["DtDynamic"];

            DataTable DtMaterialsDetails = new DataTable();
            DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

            foreach (DataRow row in DtDynamic.Rows)
            {
                DtDynamic.NewRow();

                for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                {

                    if (cellCtr == 0)
                    {
                        DtDynamic.Columns.Add(row.ItemArray[0].ToString(), typeof(string));
                    }
                    else
                    {
                        //DataColumn dc = new DataColumn();

                        if (rowcount == 0)
                            DtDynamic.Columns.Add(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString(), typeof(string));
                        else if (rowcount == 1)
                            DtDynamic.Columns.Add(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString(), typeof(string));
                        //else
                        //    DtDynamic.Columns.Add(dc);
                    }
                }
                rowcount++;

            }
            //grdmatlcost.DataSource = DtDynamic;
            //grdmatlcost.DataBind();
        }

        //private void CreateDynamicDTRows()
        //{

        //    //TableRow tr = new TableRow();
        //    //TableCell headerCell = new TableCell();

        //    //headerCell.Text = "FieldName";

        //    //tr.Cells.Add(headerCell);
        //    //Table1.Rows.Add(tr);

        //    foreach (DataRow row in DtMaterialsDetails.Rows)
        //    {
        //        TableRow tRow = new TableRow();
        //        Table1.Rows.Add(tRow);

        //        for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell = new TableCell();
        //            if (cellCtr == 0)
        //            {
        //                Label lb = new Label();
        //                tCell.Controls.Add(lb);
        //                tCell.Text = row.ItemArray[0].ToString();
        //                tRow.Cells.Add(tCell);
        //            }
        //            else
        //            {
        //                TextBox tb = new TextBox();
        //                tCell.Controls.Add(tb);
        //                if (cellCtr == 1)
        //                {

        //                    tCell.Text = row.ItemArray[0].ToString();
        //                }
        //                else if (cellCtr == 2)
        //                {
        //                    tCell.Text = row.ItemArray[1].ToString();
        //                }
        //                tRow.Cells.Add(tCell);
        //            }



        //        }
        //        tRow.BorderWidth = 1;
        //        tRow.BorderStyle = BorderStyle.Solid;
        //        tRow.BorderColor = Color.Blue;

        //        //grdmatlcost.DataSource = Table1;
        //        //grdmatlcost.DataBind();
        //    }
        //}

        protected void GetData(string reqno)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtget = new DataTable();
                string UserId = Session["userID_"].ToString();
                string strGetData = @"select CONVERT(VARCHAR(10), s.RequestDate, 103) as RequestDate,S.QuoteNo,V.Description,v.Crcy,vp.PICName
                    ,vp.PICemail from tVendor_New as V inner join TVENDORPIC as VP 
                    on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + @"TQuoteDetails_D as S on S.VendorCode1=v.Vendor 
                    where S.QuoteNo=@QuoteNo and S.VendorCode1= @VendorCode1 and V.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strGetData, MDMCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", reqno);
                    cmd.Parameters.AddWithValue("@VendorCode1", Session["mappedVendor"].ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                if (dtget.Rows.Count > 0)
                {
                    grdVendrDet.DataSource = dtget;
                    grdVendrDet.DataBind();

                    hdnQuoteNo.Value = dtget.Rows[0].ItemArray[1].ToString();

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

        private void GetSHMNPICDetails(string userdet)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                string UserId = Session["userID_"].ToString();
                string QNO = Request.QueryString["Number"];
                string str1 = "";
                if (QNO.Substring(QNO.Length - 2, 2) == "GP")
                {
                    str1 = @"select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail 
                    from TSMNProductPIC pp inner join " + TransDB.ToString() + @"TQuotedetails_D TQ on pp.Userid = Tq.CreatedBy 
                    where QuoteNo=@QuoteNo ";
                }
                else
                {
                    str1 = @"select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product 
                    from TSMNProductPIC pp inner join " + TransDB.ToString() + @"TQuotedetails_D TQ on pp.Product = TQ.Product and pp.Userid = Tq.CreatedBy and PP.Product = TQ.Product 
                    where QuoteNo=@QuoteNo and pp.DELFLAG = 0 ";
                }
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str1, MDMCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", Session["Qno"].ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtdate);
                }
                if (dtdate.Rows.Count > 0)
                {
                    txtsmnpic.Text = dtdate.Rows[0]["PICName"].ToString();
                    txtemail.Text = dtdate.Rows[0]["PICEmail"].ToString();
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

        
        protected void TurnKeyprovit()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select DefValue from [DefaultValueMaster] where Description = 'Turnkey Profit' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    hdnHidenProfit.Value = Result.Rows[0]["DefValue"].ToString();
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

        protected void process()
        {
            DropDownList ddlProcess = new DropDownList();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                string str = @"select distinct CONCAT( tp.Process_Grp_code, ' - ', tp.Process_Grp_Description) as Process_Grp_code,tp.Process_Grp_code as ProcGrpCodeOnly 
                from TPROCESGROUP_LIST TP inner join TPROCESGROUP_SUBPROCESS TPS on Tp.Process_Grp_code = tps.ProcessGrpCode where TP.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str, MDMCon);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result);
                }

                ddlProcess.DataSource = Result;
                ddlProcess.DataTextField = "Process_Grp_code";
                ddlProcess.DataValueField = "ProcGrpCodeOnly";
                ddlProcess.DataBind();

                Session["process"] = ddlProcess.DataSource;

                #region  get subvendor data
                string[] DataVnd = lblVName.Text.Split('-');
                string VndCode = DataVnd[0].ToString();
                DropDownList ddlProcessSubVndorData = new DropDownList();
                DataTable ResultSubVndName = new DataTable();
                SqlDataAdapter daSubVndName = new SqlDataAdapter();
                string strSubVndName = @"select distinct CONCAT( TKV.SubVendor, ' - ', TVN.Description) as SubVndorData  from TTURNKEY_VENDOR TKV 
                                        join tVendor_New TVN on TKV.SubVendor = TVN.Vendor 
                                        where TKV.TrnKeyVendor = @TrnKeyVendor and TKV.Plant = @Plant
                                        and TKV.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strSubVndName, MDMCon);
                    cmd.Parameters.AddWithValue("@TrnKeyVendor", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(ResultSubVndName);
                }

                ddlProcessSubVndorData.DataSource = ResultSubVndName;
                ddlProcessSubVndorData.DataTextField = "SubVndorData";
                ddlProcessSubVndorData.DataValueField = "SubVndorData";
                ddlProcessSubVndorData.DataBind();
                Session["SubVndorData"] = ddlProcessSubVndorData.DataSource;
                #endregion

                DataTable Result1 = new DataTable();
                string UserId = Session["userID_"].ToString();
                string strplant = Session["strplant"].ToString();
                string str1 = @"select TVM.MachineID as MacId, CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as Machine, 
                                CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,TVM.FollowStdRate  as FollowStdRate,
                                TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = @VendorCode and Plant = @Plant and TVM.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str1, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result1);
                }

                grdMachinelisthidden.DataSource = Result1;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;


                DataTable Result3 = new DataTable();
                UserId = Session["userID_"].ToString();
                string str3 = @"select CAST(ROUND(StdLabourRateHr,2) AS DECIMAL(12,2))as 'StdLabourRateHr',FollowStdRate,Currency 
                    from TVENDORLABRCOST TVC Where TVC.Vendorcode = @VendorCode and  Plant = @Plant and TVC.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str3, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result3);
                }

                grdLaborlisthidden.DataSource = Result3;
                grdLaborlisthidden.DataBind();
                Session["LaborListGrd"] = grdLaborlisthidden.DataSource;

                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                UserId = Session["userID_"].ToString();
                strplant = Session["strplant"].ToString();
                string str2 = @"select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID,TVM.MachineID as MacId from 
                                TVENDORMACHNLIST TVM where VendorCode =@VendorCode  and Plant = @Plant and TVM.DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str2, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result2);
                }

                if (Result2.Rows.Count > 0)
                {
                    for (int i = 0; i < Result2.Rows.Count; i++)
                    {
                        string MacId = Result2.Rows[i]["MacId"].ToString();
                        string srText = Result2.Rows[i]["MachineID"].ToString();
                        string srValue = Result2.Rows[i]["MachineID"].ToString();
                        ListItem Ddlitem = new ListItem { Text = srText, Value = srValue };
                        Ddlitem.Attributes.Add("Mac-Id", MacId);
                        ddlMachine.Items.Add(Ddlitem);
                    }
                }

                //ddlMachine.DataSource = Result2;
                //ddlMachine.DataBind();
                Session["MachineIDs"] = Result2;



                DataTable Result4 = new DataTable();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = @ProcessGrp and DELFLAG = 0 ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str4, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrp", txtprocs.Text.ToString().ToUpper());
                    sda.SelectCommand = cmd;
                    sda.Fill(Result4);
                }

                if (Result4.Rows.Count > 0)
                {
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
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

        protected void MachineIdsbyProcess(string strProcessCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());

            try
            {
                MDMCon.Open();
                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                string UserId = Session["userID_"].ToString();
                string strplant = Session["strplant"].ToString();
                string str2 = @"select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID,TVM.MachineID as 'MacId'
                                from TVENDORMACHNLIST TVM where VendorCode = @VendorCode and Plant = @Plant and ProcessGrp=@ProcessGrp and TVM.DELFLAG = 0";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(str2, MDMCon);
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", strplant);
                    cmd.Parameters.AddWithValue("@ProcessGrp", strProcessCode);
                    sda.SelectCommand = cmd;
                    sda.Fill(Result2);
                }

                ddlMachine.DataSource = Result2;
                ddlMachine.DataBind();
                Session["MachineIDsbyProcess"] = Result2;
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

        protected void btnaddProcessCost_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                int PCAddCount = int.Parse(Session["PCAddCount"].ToString());

                var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();
                var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                CreateDynamicProcessDT(0);
                PCAddCount++;
                Session["PCAddCount"] = PCAddCount;

                if (PCAddCount <= ccc)
                {
                    PCAddCount = PCAddCount + (ccc - PCAddCount) + 1;
                }
                CreateDynamicProcessDT(PCAddCount);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ProcessCostDataStore();freezeheader();ReturnBaseQtyProcCheckStats();", true);
                //CreateDynamicProcessDT(0);
                //PCAddCount++;
                //CreateDynamicProcessDT(PCAddCount);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnDelProcess_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                int PCAddCount = int.Parse(Session["PCAddCount"].ToString());

                var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();
                var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                if (hdnProcessValues.Value.ToString() == "")
                {
                    CreateDynamicProcessDT(0);
                }
                else
                {
                    CreateDynamicProcessDT(0);
                    PCAddCount--;
                    Session["PCAddCount"] = PCAddCount;
                    if (PCAddCount <= ccc)
                    {
                        PCAddCount = PCAddCount + (ccc - PCAddCount);
                        if (PCAddCount > 0)
                        {
                            CreateDynamicProcessDT(PCAddCount);
                        }
                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "processcost();", true);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ProcessCostDataStore();freezeheader();ReturnBaseQtyProcCheckStats();", true);
                //ColmNoMat = ColmNoMat - 1;
                //int a = ColmNoMat;
                //if (ColmNoMat > 1)
                //{
                //    ColmNoMat = ColmNoMat - 1;
                //}
                //MCCostDataStore();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MatlCalculation();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnAddSubProcessCost_Click(object sender, EventArgs e)
        {
            try
            {
                RegenerateSubmatData();
                subMatCostDataStore();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "freezeheader();", true);
                //DataTable DtDynamicSubMaterialsFields = new DataTable();
                //DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                //int SMCAddCount = int.Parse(Session["SMCAddCount"].ToString());

                //var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                //var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                //CreateDynamicSubMaterialDT(0);
                //SMCAddCount++;
                //Session["SMCAddCount"] = SMCAddCount;
                ////string FL = HdnFirstLoadSubProc.Value.ToString();

                //if (SMCAddCount <= ccc)
                //{
                //    SMCAddCount = SMCAddCount + (ccc - SMCAddCount) + 1;
                //}
                //CreateDynamicSubMaterialDT(SMCAddCount);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "submatlCostDataStore();freezeheader();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnDelSubMatCost_Click(object sender, EventArgs e)
        {
            try
            {
                RegenerateSubmatData();
                subMatCostDataStore();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "freezeheader();", true);
                //DataTable DtDynamicSubMaterialsFields = new DataTable();
                //DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                //int SMCAddCount = int.Parse(Session["SMCAddCount"].ToString());

                //var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();
                //var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                //if (hdnSMCTableValues.Value.ToString() == "")
                //{
                //    CreateDynamicSubMaterialDT(0);
                //}
                //else
                //{
                //    CreateDynamicSubMaterialDT(0);
                //    SMCAddCount--;
                //    Session["SMCAddCount"] = SMCAddCount;
                //    //string FL = HdnFirstLoadSubProc.Value.ToString();

                //    if (SMCAddCount <= ccc)
                //    {
                //        SMCAddCount = SMCAddCount + (ccc - SMCAddCount);
                //        if (SMCAddCount > 0)
                //        {
                //            CreateDynamicSubMaterialDT(SMCAddCount);
                //        }
                //    }
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "submatlcost();freezeheader();", true);
                //}
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnAddOtherCost_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                int OthersAddCount = int.Parse(Session["OthersAddCount"].ToString());

                var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

                CreateDynamicOthersCostDT(0);
                OthersAddCount++;
                Session["OthersAddCount"] = OthersAddCount;

                if (OthersAddCount <= ccc)
                {
                    OthersAddCount = OthersAddCount + (ccc - OthersAddCount) + 1;
                }
                CreateDynamicOthersCostDT(OthersAddCount);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "OthersCostDataStore();freezeheader();", true);
                //CreateDynamicOthersCostDT(0);
                //OthersAddCount++;
                //CreateDynamicOthersCostDT(OthersAddCount);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnDelOthCost_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];
                int OthersAddCount = int.Parse(Session["OthersAddCount"].ToString());
                var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

                if (hdnOtherValues.Value.ToString() == "")
                {
                    CreateDynamicOthersCostDT(0);
                }
                else
                {
                    CreateDynamicOthersCostDT(0);
                    OthersAddCount--;
                    Session["OthersAddCount"] = OthersAddCount;
                    if (OthersAddCount <= ccc)
                    {
                        OthersAddCount = OthersAddCount + (ccc - OthersAddCount);
                        if (OthersAddCount > 0)
                        {
                            CreateDynamicOthersCostDT(OthersAddCount);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "othercost();freezeheader();", true);
                }


                //CreateDynamicOthersCostDT(0);
                //OthersAddCount++;
                //CreateDynamicOthersCostDT(OthersAddCount);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnAddColumns_Click(object sender, EventArgs e)
        {
            DataTable DtDynamic = new DataTable();
            DtDynamic = (DataTable)Session["DtDynamic"];
            int MtlAddCount = int.Parse(Session["MtlAddCount"].ToString());
            var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
            var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

            CreateDynamicDT(0);
            MtlAddCount++;
            Session["MtlAddCount"] = MtlAddCount;
            if (MtlAddCount <= ccc)
            {
                MtlAddCount = MtlAddCount + (ccc - MtlAddCount) + 1;
            }
            CreateDynamicDT(MtlAddCount);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MCDataStore();freezeheader();", true);
            //FirstLoad = false;
            //CreateDynamicDT(0);
            //MtlAddCount++;
            //CreateDynamicDT(MtlAddCount);

            //CreateDynamicProcessDT(0);
            //CreateDynamicProcessDT(PCAddCount);
        }

        protected void BtnDelMaterial_Click(object sender, EventArgs e)
        {
            DataTable DtDynamic = new DataTable();
            DtDynamic = (DataTable)Session["DtDynamic"];
            int MtlAddCount = int.Parse(Session["MtlAddCount"].ToString());
            var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
            var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

            if (hdnMCTableValues.Value.ToString() == "")
            {
                CreateDynamicDT(0);
            }
            else
            {
                CreateDynamicDT(0);
                MtlAddCount--;
                Session["MtlAddCount"] = MtlAddCount;
                if (MtlAddCount <= ccc)
                {
                    MtlAddCount = MtlAddCount + (ccc - MtlAddCount);
                    if (MtlAddCount > 0)
                    {
                        CreateDynamicDT(MtlAddCount);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MatlCalculation();freezeheader();", true);
            }

            //ColmNoMat = ColmNoMat - 1;
            //int a = ColmNoMat;
            //if (ColmNoMat > 1)
            //{
            //    ColmNoMat = ColmNoMat - 1;
            //}
            //MCCostDataStore();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MatlCalculation();", true);
        }

        bool ValidateCost()
        {
            try
            {
                MsgErr = "";
                bool isOK = true;
                #region Process Values
                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                {
                    DataTable DtDynamicProcessFields = new DataTable();
                    DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempProcesslist = hdnProcessValues.Value.ToString().Split(',').ToList();

                        var tempProcesslistNew = tempProcesslist;
                        if (i > 1)
                            tempProcesslistNew = tempProcesslistNew.Skip(((i - 1) * (DtDynamicProcessFields.Rows.Count))).ToList();

                        string IProc = tempProcesslistNew[0].ToString().Replace("NaN", "");
                        string ISubProc = tempProcesslistNew[1].ToString();
                        string ITurnKey = tempProcesslistNew[2].ToString();
                        string ITurnKeySubVnd = tempProcesslistNew[3].ToString();
                        if (tempProcesslistNew[3].ToString() != null || tempProcesslistNew[3].ToString() != "")
                        {
                            string[] getITurnKeySubVndID = tempProcesslistNew[3].ToString().Split('-');
                            if (getITurnKeySubVndID.Count() > 0)
                            {
                                ITurnKeySubVnd = getITurnKeySubVndID[0].ToString();
                            }
                            else
                            {
                                ITurnKeySubVnd = "";
                            }
                        }
                        else
                        {
                            ITurnKeySubVnd = "";
                        }

                        string IMachineLabor = tempProcesslistNew[4].ToString();
                        string IMachine = tempProcesslistNew[5].ToString().Replace("  ", " ");

                        if (ITurnKey != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKeySubVnd = "";
                        }
                        else if (ITurnKeySubVnd != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKey = "";
                        }

                        string IStRate = tempProcesslistNew[6].ToString().Replace("NaN", "");
                        string IVendorRate = tempProcesslistNew[7].ToString();
                        string IProcUOM = tempProcesslistNew[8].ToString();
                        string[] ArIProcUOM = IProcUOM.Split('-');
                        if (ArIProcUOM.Count() > 0)
                        {
                            IProcUOM = ArIProcUOM[0].ToString().Trim();
                        }
                        string IBaseQty = tempProcesslistNew[9].ToString();
                        string IDPUOM = tempProcesslistNew[10].ToString();
                        string Iyield = tempProcesslistNew[11].ToString();
                        string ITurnKeyCost = tempProcesslistNew[12].ToString();
                        string ITurnKeyProfit = tempProcesslistNew[13].ToString();
                        string ICostPc = tempProcesslistNew[14].ToString();
                        string ITotalCost = tempProcesslistNew[15].ToString();

                        if (ITurnKey == "" && ITurnKeySubVnd == "")
                        {
                            if (IMachineLabor.ToUpper() == "MACHINE")
                            {
                                if (IMachine.Trim() == "")
                                {
                                    MsgErr = "Please Select Machine at Column " + (i);
                                    isOK = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion Process Values

                if (isOK == true)
                {
                    return true;
                }
                else
                {
                    return isOK;
                }
            }
            catch (Exception ex)
            {
                MsgErr = ex.Message.ToString();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
        }

        bool SubmitData()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlConnection EmetConDraft = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                EmetCon.Open();
                EmetConDraft.Open();
                EmetTrans = EmetCon.BeginTransaction();

                #region Cek Is Data Exist In Draft Table
                string sqlDel = "";
                sql = "select * from TOtherCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TOtherCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = "select * from TSMCCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TSMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = "select * from TProcessCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TProcessCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = " select * from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                if (sqlDel != "")
                {
                    cmd = new SqlCommand(sqlDel, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region Delete Old mat Cost
                if (hdnMassRevision.Value != "")
                {
                    sql = " delete from TMCCostDetails where QuoteNo=@QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@QuoteNo", Session["Qno"].ToString());
                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region other values
                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                {
                    DataTable DtDynamicOtherCostsFields = new DataTable();
                    DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                    var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                        var tempOtherlistNew = tempOtherlist;
                        if (i > 1)
                            tempOtherlistNew = tempOtherlistNew.Skip(((i - 1) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();

                        sql = "insert into [dbo].[TOtherCostDetails] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

                        string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempOtherlistNew[1].ToString();
                        string ItotalCost = tempOtherlistNew[2].ToString();
                        string UserId = Session["userID_"].ToString();
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                        cmd.Parameters.AddWithValue("@Desc", IDesc);
                        cmd.Parameters.AddWithValue("@Cost", ICost);
                        cmd.Parameters.AddWithValue("@TotalCost", ItotalCost);
                        cmd.Parameters.AddWithValue("@RowId", i);
                        cmd.Parameters.AddWithValue("@By", UserId);
                        cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                    }
                }

                #endregion other values

                #region SMC Values
                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                {
                    DataTable DtDynamicSubMaterialsFields = new DataTable();
                    DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                    var tempMSClistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempMSClistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                        var tempSMClistNew = tempSMClist;
                        if (i > 1)
                            tempSMClistNew = tempSMClistNew.Skip(((i - 1) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();

                        string IDesc = tempSMClistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempSMClistNew[1].ToString();
                        string IConsumption = tempSMClistNew[2].ToString();
                        string ICostpcs = tempSMClistNew[3].ToString();
                        string ItotalCost = tempSMClistNew[4].ToString();

                        string UserId = Session["userID_"].ToString();
                        bool isInsert = false;
                        if (i == 1)
                        {
                            isInsert = true;
                        }
                        else if (IDesc.Trim() == "" && ICost.Trim() == "" && IConsumption.Trim() == "")
                        {
                            isInsert = false;
                        }
                        else
                        {
                            isInsert = true;
                        }

                        if (isInsert == true) {
                            sql = "insert into [dbo].[TSMCCostDetails] (QuoteNo,ProcessGroup,[Sub-Mat/T&JDescription],[Sub-Mat/T&JCost],[Consumption(pcs)],[Sub-Mat/T&JCost/pcs],[TotalSub-Mat/T&JCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@IConsumption,@ICostpcs,@ItotalCost,@RowId,@By,@On)";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                            cmd.Parameters.AddWithValue("@Desc", IDesc);
                            cmd.Parameters.AddWithValue("@Cost", ICost);
                            cmd.Parameters.AddWithValue("@IConsumption", IConsumption);
                            cmd.Parameters.AddWithValue("@ICostpcs", ICostpcs);
                            cmd.Parameters.AddWithValue("@ItotalCost", ItotalCost);
                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", Session["userID_"].ToString());
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                #endregion SMC Values

                #region Process Values
                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                {
                    DataTable DtDynamicProcessFields = new DataTable();
                    DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempProcesslist = hdnProcessValues.Value.ToString().Split(',').ToList();

                        var tempProcesslistNew = tempProcesslist;
                        if (i > 1)
                            tempProcesslistNew = tempProcesslistNew.Skip(((i - 1) * (DtDynamicProcessFields.Rows.Count))).ToList();

                        string IProc = tempProcesslistNew[0].ToString().Replace("NaN", "");
                        string ISubProc = tempProcesslistNew[1].ToString();
                        string ITurnKey = tempProcesslistNew[2].ToString();
                        string ITurnKeySubVnd = tempProcesslistNew[3].ToString();
                        if (tempProcesslistNew[3].ToString() != null || tempProcesslistNew[3].ToString() != "")
                        {
                            string[] getITurnKeySubVndID = tempProcesslistNew[3].ToString().Split('-');
                            if (getITurnKeySubVndID.Count() > 0)
                            {
                                ITurnKeySubVnd = getITurnKeySubVndID[0].ToString();
                            }
                            else
                            {
                                ITurnKeySubVnd = "";
                            }
                        }
                        else
                        {
                            ITurnKeySubVnd = "";
                        }

                        string IMachineLabor = tempProcesslistNew[4].ToString();
                        string IMachine = tempProcesslistNew[5].ToString().Replace("  ", " ");

                        if (ITurnKey != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKeySubVnd = "";
                        }
                        else if (ITurnKeySubVnd != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKey = "";
                        }

                        string IStRate = tempProcesslistNew[6].ToString().Replace("NaN", "");
                        string IVendorRate = tempProcesslistNew[7].ToString();
                        string IProcUOM = tempProcesslistNew[8].ToString();
                        string[] ArIProcUOM = IProcUOM.Split('-');
                        if (ArIProcUOM.Count() > 0)
                        {
                            IProcUOM = ArIProcUOM[0].ToString().Trim();
                        }
                        string IBaseQty = tempProcesslistNew[9].ToString();
                        string IDPUOM = tempProcesslistNew[10].ToString();
                        string Iyield = tempProcesslistNew[11].ToString();
                        string ITurnKeyCost = tempProcesslistNew[12].ToString();
                        string ITurnKeyProfit = tempProcesslistNew[13].ToString();
                        string ICostPc = tempProcesslistNew[14].ToString();
                        string ITotalCost = tempProcesslistNew[15].ToString();


                        string UserId = Session["userID_"].ToString();
                        sql = @"insert into [dbo].[TProcessCostDetails] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn,TurnKeySubVnd,TurnKeyCost,TurnKeyProfit) "
                            + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On,@ITurnKeySubVnd,@ITurnKeyCost,@ITurnKeyProfit)";

                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                        cmd.Parameters.AddWithValue("@IProc", IProc);
                        cmd.Parameters.AddWithValue("@ISubProc", ISubProc);
                        cmd.Parameters.AddWithValue("@ITurnKey", ITurnKey);
                        cmd.Parameters.AddWithValue("@ITurnKeySubVnd", ITurnKeySubVnd);
                        cmd.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
                        cmd.Parameters.AddWithValue("@IMachine", IMachine);
                        cmd.Parameters.AddWithValue("@IStRate", IStRate);
                        cmd.Parameters.AddWithValue("@IVendorRate", IVendorRate);
                        cmd.Parameters.AddWithValue("@IProcUOM", IProcUOM);
                        cmd.Parameters.AddWithValue("@IBaseQty", IBaseQty);
                        cmd.Parameters.AddWithValue("@IDPUOM", IDPUOM);
                        cmd.Parameters.AddWithValue("@Iyield", Iyield);
                        cmd.Parameters.AddWithValue("@ITurnKeyCost", ITurnKeyCost);
                        cmd.Parameters.AddWithValue("@ITurnKeyProfit", ITurnKeyProfit);
                        cmd.Parameters.AddWithValue("@ICostPc", ICostPc);
                        cmd.Parameters.AddWithValue("@ITotalCost", ITotalCost);
                        cmd.Parameters.AddWithValue("@RowId", i);
                        cmd.Parameters.AddWithValue("@By", UserId);
                        cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                    }
                }

                #endregion Process Values

                #region Material Cost Values
                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                {
                    DataTable DtDynamic = new DataTable();
                    DtDynamic = (DataTable)Session["DtDynamic"];

                    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                    string strMCode = "";
                    string strMDesc = "";
                    string strRawCost = "";
                    string strTotalRawCost = "";
                    string strPartUnitW = "";
                    string strDiaID = "";
                    string strDiaOD = "";
                    string strThick = "";
                    string strWidth = "";
                    string strPitch = "";
                    string strMDensity = "";
                    string strRunnerWeight = "";
                    string strRunnerRatio = "";
                    string strRecycle = "";
                    string strCavity = "";
                    string strMLoss = "";
                    string strMCrossWeight = "";
                    string strMScrapWeight = "";
                    string strScrapLoss = "";
                    string strScrapPrice = "";
                    string strScrapRebate = "";
                    string strMCostpcs = "";
                    string strTotalcostpcs = "";

                    string strRawCostUOM = "";

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                        var tempMClistNew = tempMClist;
                        if (i > 1)
                        {
                            tempMClistNew = tempMClistNew.Skip(((i - 1) * (DtDynamic.Rows.Count))).ToList();

                            #region Common for All
                            strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                            strMDesc = tempMClistNew[1].ToString();
                            strRawCost = tempMClistNew[2].ToString();

                            int Clm = (i - 1);
                            var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                            if (ArrRwMatUom.Count() >= Clm)
                            {
                                string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                strRawCostUOM = UOM;
                            }

                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                            }
                            else
                            {
                                strTotalRawCost = tempMClistNew[3].ToString();
                                strPartUnitW = tempMClistNew[4].ToString();
                            }
                            #endregion Common for all

                            #region IM / LAYOUT1
                            //if (txtprocs.Text == "IM")
                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strRunnerWeight = tempMClistNew[6].ToString();
                                strRunnerRatio = tempMClistNew[7].ToString();
                                strRecycle = tempMClistNew[8].ToString();

                                strMLoss = tempMClistNew[9].ToString();
                                strMCrossWeight = tempMClistNew[10].ToString();

                                strMCostpcs = tempMClistNew[11].ToString();
                                strTotalcostpcs = tempMClistNew[12].ToString();


                            }
                            #endregion IM

                            #region CA or SPR / layout3 / layout6
                            //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();
                            }

                            #endregion CA or SPR

                            #region ST / layout 5
                            //else if (txtprocs.Text == "ST")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                            {
                                strThick = tempMClistNew[5].ToString();
                                strWidth = tempMClistNew[6].ToString();
                                strPitch = tempMClistNew[7].ToString();
                                strMDensity = tempMClistNew[8].ToString();

                                strCavity = tempMClistNew[9].ToString();
                                strMLoss = tempMClistNew[10].ToString();
                                strMCrossWeight = tempMClistNew[11].ToString();

                                strMScrapWeight = tempMClistNew[12].ToString();
                                strScrapLoss = tempMClistNew[13].ToString();
                                strScrapPrice = tempMClistNew[14].ToString();
                                strScrapRebate = tempMClistNew[15].ToString();

                                strMCostpcs = tempMClistNew[16].ToString();
                                strTotalcostpcs = tempMClistNew[17].ToString();
                            }
                            #endregion ST

                            #region MS / layout 4
                            //else if (txtprocs.Text == "MS")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();

                                //strDiaID = tempMClistNew[5].ToString();
                                //strDiaOD = tempMClistNew[6].ToString();
                                //strWidth = tempMClistNew[7].ToString();

                                //strCavity = tempMClistNew[8].ToString();
                                //strMLoss = tempMClistNew[9].ToString();
                                //strMCrossWeight = tempMClistNew[10].ToString();

                                //strMCostpcs = tempMClistNew[11].ToString();
                                //strTotalcostpcs = tempMClistNew[12].ToString();
                            }
                            #endregion MS

                            #region layout 2
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                            {
                                strCavity = tempMClistNew[2].ToString();
                                strPartUnitW = tempMClistNew[1].ToString();

                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion MS

                            #region layout 7
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion layout 2

                            sql = "insert into [dbo].[TMCCostDetails] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                    + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                    + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                    + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                    + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn,RawMaterialCostUOM)"

                                    + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                    + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                    + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On,@RawMaterialCostUOM)";

                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd.Parameters.AddWithValue("@strThick", strThick);
                            cmd.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);
                            cmd.Parameters.AddWithValue("@RawMaterialCostUOM", strRawCostUOM);

                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", Session["userID_"].ToString());
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            #region Common for All
                            strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                            strMDesc = tempMClistNew[1].ToString();
                            strRawCost = tempMClistNew[2].ToString();

                            int Clm = (i - 1);
                            var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                            if (ArrRwMatUom.Count() >= Clm)
                            {
                                string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                strRawCostUOM = UOM;
                            }

                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                            }
                            else
                            {
                                strTotalRawCost = tempMClistNew[3].ToString();
                                strPartUnitW = tempMClistNew[4].ToString();
                            }
                            #endregion Common for all

                            #region IM / LAYOUT1
                            //if (txtprocs.Text == "IM")
                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strRunnerWeight = tempMClistNew[6].ToString();
                                strRunnerRatio = tempMClistNew[7].ToString();
                                strRecycle = tempMClistNew[8].ToString();

                                strMLoss = tempMClistNew[9].ToString();
                                strMCrossWeight = tempMClistNew[10].ToString();

                                strMCostpcs = tempMClistNew[11].ToString();
                                strTotalcostpcs = tempMClistNew[12].ToString();


                            }
                            #endregion IM

                            #region CA or SPR / layout3 / layout6
                            //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();
                            }

                            #endregion CA or SPR

                            #region ST / layout 5
                            //else if (txtprocs.Text == "ST")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                            {
                                strThick = tempMClistNew[5].ToString();
                                strWidth = tempMClistNew[6].ToString();
                                strPitch = tempMClistNew[7].ToString();
                                strMDensity = tempMClistNew[8].ToString();

                                strCavity = tempMClistNew[9].ToString();
                                strMLoss = tempMClistNew[10].ToString();
                                strMCrossWeight = tempMClistNew[11].ToString();

                                strMScrapWeight = tempMClistNew[12].ToString();
                                strScrapLoss = tempMClistNew[13].ToString();
                                strScrapPrice = tempMClistNew[14].ToString();
                                strScrapRebate = tempMClistNew[15].ToString();

                                strMCostpcs = tempMClistNew[16].ToString();
                                strTotalcostpcs = tempMClistNew[17].ToString();
                            }
                            #endregion ST

                            #region MS / layout 4
                            //else if (txtprocs.Text == "MS")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();

                                //strDiaID = tempMClistNew[5].ToString();
                                //strDiaOD = tempMClistNew[6].ToString();
                                //strWidth = tempMClistNew[7].ToString();

                                //strCavity = tempMClistNew[8].ToString();
                                //strMLoss = tempMClistNew[9].ToString();
                                //strMCrossWeight = tempMClistNew[10].ToString();

                                //strMCostpcs = tempMClistNew[11].ToString();
                                //strTotalcostpcs = tempMClistNew[12].ToString();
                            }
                            #endregion MS

                            #region layout 2
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                            {
                                strCavity = tempMClistNew[2].ToString();
                                strPartUnitW = tempMClistNew[1].ToString();

                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion MS

                            #region layout 7
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion layout 2

                            string UserId = Session["userID_"].ToString();

                            sql = "insert into [dbo].[TMCCostDetails] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn,RawMaterialCostUOM)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On,@RawMaterialCostUOM)";

                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd.Parameters.AddWithValue("@strThick", strThick);
                            cmd.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);
                            cmd.Parameters.AddWithValue("@RawMaterialCostUOM", strRawCostUOM);

                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", UserId);
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                #endregion Process Values

                #region update data in tquotedetails
                string SMNPIC = txtsmnpic.Text.ToString();
                string SMNPICEmail = txtemail.Text.ToString();

                string TMCost = hdnTMatCost.Value;
                string TPCost = hdnTProCost.Value;

                string TSMCost = hdnTSumMatCost.Value;
                string TOCost = hdnTOtherCost.Value;
                string GTCost = hdnTGTotal.Value;
                string FTCost = hdnTFinalQPrice.Value;

                string TProfit = hdnProfit.Value;
                string TDiscount = hdnDiscount.Value;
                string TGA = hdnGA.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnGA.Value.ToString();
                if (TGA == "")
                {
                    TGA = "0";
                }
                if (TProfit.ToUpper() == "NAN")
                {
                    TProfit = "";
                }
                if (TDiscount.ToUpper() == "NAN")
                {
                    TDiscount = "";
                }

                string userSession = (string)HttpContext.Current.Session["userID_"].ToString();
                
                sql = @" update TQuoteDetails SET  TotalMaterialCost = @TMCost, TotalSubMaterialCost = @TSMCost, TotalProcessCost =@TPCost
                                , TotalOtheritemsCost = @TOCost,GrandTotalCost =@GTCost ,FinalQuotePrice = @FTCost,
                                Profit = @TProfit,Discount = @TDiscount,NetProfDisc = @TNetProfDisc,GA = @TGA, 
                                ShimanoPIC = @SMNPIC,ShimanoPICEmail = @SMNPICEmail , ApprovalStatus = '2' , PICApprovalStatus=0, 
                                EffectiveDate = @newEffDate  , DueOn = @DueOn  , UpdatedBy = @userSession , 
                                UpdatedOn = CURRENT_TIMESTAMP,CommentByVendor=@CommentByVendor, countryorg= @ddlpirjtype 
                                ,IsUseMachineAmortize = @IsUseMachineAmortize
                                Where QuoteNo = @QuoteNo ";

                if (Session["FlAttachment"] != null && TxtLbFlName.Text != "No File")
                {
                    HttpPostedFile Fl = (HttpPostedFile)Session["FlAttachment"];
                    if (Fl != null)
                    {
                        sql += " update TQuoteDetails SET VndAttchmnt=@VndAttchmnt Where QuoteNo = @QuoteNo ";
                    }
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text == "No File")
                {
                    sql += " update TQuoteDetails SET VndAttchmnt=@VndAttchmnt Where QuoteNo = @QuoteNo ";
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text != "No File")
                {
                    sql += " update TQuoteDetails SET VndAttchmnt=@VndAttchmnt Where QuoteNo = @QuoteNo ";
                }

                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@TMCost", TMCost);
                cmd.Parameters.AddWithValue("@TSMCost", TSMCost);
                cmd.Parameters.AddWithValue("@TPCost", TPCost);
                cmd.Parameters.AddWithValue("@TOCost", TOCost);
                cmd.Parameters.AddWithValue("@GTCost", GTCost);
                cmd.Parameters.AddWithValue("@FTCost", FTCost);
                cmd.Parameters.AddWithValue("@TProfit", TProfit);
                cmd.Parameters.AddWithValue("@TDiscount", TDiscount);
                cmd.Parameters.AddWithValue("@TGA", TGA);
                cmd.Parameters.AddWithValue("@SMNPIC", SMNPIC);
                cmd.Parameters.AddWithValue("@userSession", userSession);
                cmd.Parameters.AddWithValue("@ddlpirjtype", ddlpirjtype.SelectedValue);
                cmd.Parameters.AddWithValue("@SMNPICEmail", SMNPICEmail);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                cmd.Parameters.AddWithValue("@TNetProfDisc", HdnNetProfnDisc.Value.ToString() == "" ? DBNull.Value.ToString() : HdnNetProfnDisc.Value.ToString());

                if (TextBox1.Text != "")
                {
                    DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@newEffDate", newEffDate.ToString("yyyy/MM/dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@newEffDate", DBNull.Value);
                }
                if (txtfinal.Text != "")
                {
                    DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@DueOn", newDueon.ToString("yyyy/MM/dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DueOn", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@CommentByVendor", TxtComntByVendor.Text);
                if (Session["FlAttachment"] != null && TxtLbFlName.Text != "No File")
                {
                    HttpPostedFile file = (HttpPostedFile)Session["FlAttachment"];
                    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    if (file != null)
                    {
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string FileExtension = System.IO.Path.GetExtension(file.FileName);
                        string FileName = System.IO.Path.GetFileName(file.FileName);
                        cmd.Parameters.AddWithValue("@VndAttchmnt", hdnQuoteNo.Value + "-" + FileName);
                        string PathAndFileName = folderPath + hdnQuoteNo.Value + "-" + FileName;
                        file.SaveAs(PathAndFileName);
                    }

                    string OldFile = folderPath + LbFlNameOri.Text;
                    FileInfo Myfile = new FileInfo(OldFile);
                    if (Myfile.Exists) //check file exsit or not  
                    {
                        Myfile.Delete();
                    }

                    //FlAttachment = (FileUpload)Session["FlAttachment"];
                    //if (FlAttachment.HasFile)
                    //{
                    //    string FileName = FlAttachment.PostedFile.FileName;
                    //    cmd.Parameters.AddWithValue("@VndAttchmnt", hdnQuoteNo.Value + "-" + FileName);

                    //    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    //    if (!Directory.Exists(folderPath))
                    //    {
                    //        Directory.CreateDirectory(folderPath);
                    //    }

                    //    string OldFile = folderPath + LbFlNameOri.Text;
                    //    FileInfo file = new FileInfo(OldFile);
                    //    if (file.Exists) //check file exsit or not  
                    //    {
                    //        file.Delete();
                    //    }

                    //    string FileExtension = System.IO.Path.GetExtension(FlAttachment.FileName);
                    //    string filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                    //    string PathAndFileName = folderPath + hdnQuoteNo.Value + "-" + filename;
                    //    FlAttachment.SaveAs(PathAndFileName);

                    //}
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text == "No File")
                {
                    cmd.Parameters.AddWithValue("@VndAttchmnt", DBNull.Value);
                    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string OldFile = folderPath + LbFlNameOri.Text;
                    FileInfo Myfile = new FileInfo(OldFile);
                    if (Myfile.Exists) //check file exsit or not  
                    {
                        Myfile.Delete();
                    }
                    //string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}
                    //string OldFile = folderPath + LbFlNameOri.Text;
                    //FileInfo file = new FileInfo(OldFile);
                    //if (file.Exists) //check file exsit or not  
                    //{
                    //    file.Delete();
                    //}
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text != "No File")
                {
                    cmd.Parameters.AddWithValue("@VndAttchmnt", LbFlNameOri.Text);
                }
                cmd.Parameters.AddWithValue("@IsUseMachineAmortize", HdnIsUseMachineAmor.Value);
                cmd.ExecuteNonQuery();

                string IsSAPCode = hdnIsSAPCode.Value;
                if (IsSAPCode == "False")
                {
                    sql = @"update TQuoteDetails SET  ApprovalStatus='5',PICApprovalStatus = '5', ManagerApprovalStatus = '5', DIRApprovalStatus = '5' 
                                        where QuoteNo = @QuoteNo ";
                }
                else
                {
                    sql = "update TQuoteDetails SET  PICApprovalStatus=0 where  requestnumber in(select distinct RequestNumber from TQuoteDetails where  QuoteNo = @QuoteNo)";
                }
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                cmd.ExecuteNonQuery();


                sql = " delete from TQuoteDetails_D where QuoteNo=@QuoteNo ";
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                cmd.ExecuteNonQuery();
                #endregion

                #region Delete data Machine Amortize in emet tr table
                if (hdnQuoteNoRef.Value != "")
                {
                    sql = @" delete from TMachineAmortization where QuoteNo = @QuoteNo";
                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region insert data Machine Amortize in emet tr table
                if (HdnIsUseMachineAmor.Value == "1")
                {
                    if (Session["VndMachineAmortize"] != null)
                    {
                        DataTable dtMachineAmortize = (DataTable)Session["VndMachineAmortize"];
                        for (int i = 0; i < dtMachineAmortize.Rows.Count; i++)
                        {
                            sql = @" insert into TMachineAmortization
                                    (
                                    [Plant] ,[RequestNumber],[QuoteNo] ,[VendorCode],
                                    [VendorCurrency] ,[Process_Grp_code],[Vend_MachineID] ,[AmortizeCost] ,
                                    [AmortizeCurrency] ,[ExchangeRate] ,[AmortizeCost_Vend_Curr] ,[AmortizePeriod] ,
                                    [AmortizePeriodUOM],[TotalAmortizeQty],[QtyUOM],[AmortizeCost_Pc_Vend_Curr],
                                    [EffectiveFrom],[DueDate],[CreatedBy],[CreatedOn]
                                    )
                                    values
                                    (
                                    @Plant ,(select distinct RequestNumber from TQuoteDetails where QuoteNo=@QuoteNo),@QuoteNo ,@VendorCode,
                                    @VendorCurrency ,@Process_Grp_code,@Vend_MachineID ,@AmortizeCost ,
                                    @AmortizeCurrency ,@ExchangeRate ,@AmortizeCost_Vend_Curr ,@AmortizePeriod ,
                                    @AmortizePeriodUOM,@TotalAmortizeQty,@QtyUOM,@AmortizeCost_Pc_Vend_Curr,
                                    @EffectiveFrom,@DueDate,@CreatedBy,current_timestamp
                                    ) ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@Plant", dtMachineAmortize.Rows[i]["Plant"].ToString());
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@VendorCode", dtMachineAmortize.Rows[i]["VendorCode"].ToString());
                            cmd.Parameters.AddWithValue("@VendorCurrency", dtMachineAmortize.Rows[i]["VendorCurrency"].ToString());
                            cmd.Parameters.AddWithValue("@Process_Grp_code", dtMachineAmortize.Rows[i]["Process_Grp_code"].ToString());
                            cmd.Parameters.AddWithValue("@Vend_MachineID", dtMachineAmortize.Rows[i]["Vend_MachineID"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizeCost", dtMachineAmortize.Rows[i]["AmortizeCost"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizeCurrency", dtMachineAmortize.Rows[i]["AmortizeCurrency"].ToString());
                            cmd.Parameters.AddWithValue("@ExchangeRate", dtMachineAmortize.Rows[i]["ExchangeRate"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizeCost_Vend_Curr", dtMachineAmortize.Rows[i]["AmortizeCost_Vend_Curr"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizePeriod", dtMachineAmortize.Rows[i]["AmortizePeriod"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizePeriodUOM", dtMachineAmortize.Rows[i]["AmortizePeriodUOM"].ToString());
                            cmd.Parameters.AddWithValue("@TotalAmortizeQty", dtMachineAmortize.Rows[i]["TotalAmortizeQty"].ToString());
                            cmd.Parameters.AddWithValue("@QtyUOM", dtMachineAmortize.Rows[i]["QtyUOM"].ToString());
                            cmd.Parameters.AddWithValue("@AmortizeCost_Pc_Vend_Curr", dtMachineAmortize.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString());
                            cmd.Parameters.AddWithValue("@EffectiveFrom", dtMachineAmortize.Rows[i]["EffectiveFrom"].ToString());
                            cmd.Parameters.AddWithValue("@DueDate", dtMachineAmortize.Rows[i]["DueDate"].ToString());
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["userID_"].ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                EmetTrans.Commit();
                EmetTrans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MsgErr = ex.Message.ToString();
                EmetTrans.Rollback();
                EmetTrans.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
                EmetConDraft.Dispose();
            }
        }

        bool SaveAsDraft()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlConnection EmetConDraft = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                EmetCon.Open();
                EmetConDraft.Open();
                EmetTrans = EmetCon.BeginTransaction();

                #region Cek Is Data Exist In Draft Table
                string sqlDel = "";
                sql = "select * from TOtherCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TOtherCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = "select * from TSMCCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TSMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = "select * from TProcessCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TProcessCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                sql = " select * from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                if (sqlDel != "")
                {
                    cmd = new SqlCommand(sqlDel, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region SaveallCostDetailsDraft

                #region other values
                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                {
                    DataTable DtDynamicOtherCostsFields = new DataTable();
                    DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                    var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                        var tempOtherlistNew = tempOtherlist;
                        if (i > 1)
                            tempOtherlistNew = tempOtherlistNew.Skip(((i - 1) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();

                        sql = "insert into [dbo].[TOtherCostDetails_D] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

                        string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempOtherlistNew[1].ToString();
                        string ItotalCost = tempOtherlistNew[2].ToString();

                        string UserId = Session["userID_"].ToString();
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                        cmd.Parameters.AddWithValue("@Desc", IDesc);
                        cmd.Parameters.AddWithValue("@Cost", ICost);
                        cmd.Parameters.AddWithValue("@TotalCost", ItotalCost);
                        cmd.Parameters.AddWithValue("@RowId", i);
                        cmd.Parameters.AddWithValue("@By", UserId);
                        cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                    }
                }

                #endregion other values

                #region SMC Values
                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                {
                    DataTable DtDynamicSubMaterialsFields = new DataTable();
                    DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                    var tempMSClistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempMSClistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                        var tempSMClistNew = tempSMClist;
                        if (i > 1)
                            tempSMClistNew = tempSMClistNew.Skip(((i - 1) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();

                        string IDesc = tempSMClistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempSMClistNew[1].ToString();
                        string IConsumption = tempSMClistNew[2].ToString();
                        string ICostpcs = tempSMClistNew[3].ToString();
                        string ItotalCost = tempSMClistNew[4].ToString();

                        string UserId = Session["userID_"].ToString();
                        bool isInsert = false;
                        if (i == 1)
                        {
                            isInsert = true;
                        }
                        else if (IDesc.Trim() == "" && ICost.Trim() == "" && IConsumption.Trim() == "")
                        {
                            isInsert = false;
                        }
                        else
                        {
                            isInsert = true;
                        }

                        if (isInsert == true) {
                            string sql = "insert into [dbo].[TSMCCostDetails_D] (QuoteNo,ProcessGroup,[Sub-Mat/T&JDescription],[Sub-Mat/T&JCost],[Consumption(pcs)],[Sub-Mat/T&JCost/pcs],[TotalSub-Mat/T&JCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@IConsumption,@ICostpcs,@ItotalCost,@RowId,@By,@On)";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                            cmd.Parameters.AddWithValue("@Desc", IDesc);
                            cmd.Parameters.AddWithValue("@Cost", ICost);
                            cmd.Parameters.AddWithValue("@IConsumption", IConsumption);
                            cmd.Parameters.AddWithValue("@ICostpcs", ICostpcs);
                            cmd.Parameters.AddWithValue("@ItotalCost", ItotalCost);
                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", UserId);
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                #endregion SMC Values

                #region Process Values New
                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                {
                    DataTable DtDynamicProcessFields = new DataTable();
                    DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempProcesslist = hdnProcessValues.Value.ToString().Split(',').ToList();

                        var tempProcesslistNew = tempProcesslist;
                        if (i > 1)
                            tempProcesslistNew = tempProcesslistNew.Skip(((i - 1) * (DtDynamicProcessFields.Rows.Count))).ToList();

                        string IProc = tempProcesslistNew[0].ToString().Replace("NaN", "");
                        string ISubProc = tempProcesslistNew[1].ToString();
                        string ITurnKey = tempProcesslistNew[2].ToString();
                        string ITurnKeySubVnd = tempProcesslistNew[3].ToString();
                        if (tempProcesslistNew[3].ToString() != null || tempProcesslistNew[3].ToString() != "")
                        {
                            string[] getITurnKeySubVndID = tempProcesslistNew[3].ToString().Split('-');
                            if (getITurnKeySubVndID.Count() > 0)
                            {
                                ITurnKeySubVnd = getITurnKeySubVndID[0].ToString();
                                if (ITurnKeySubVnd.Trim().ToUpper().Replace("-", "") == "SELECT")
                                {
                                    ITurnKeySubVnd = "";
                                }
                            }
                            else
                            {
                                ITurnKeySubVnd = "";
                            }
                        }
                        else
                        {
                            ITurnKeySubVnd = "";
                        }

                        string IMachineLabor = tempProcesslistNew[4].ToString();
                        string IMachine = tempProcesslistNew[5].ToString().Replace("  ", " ");

                        if (ITurnKey != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKeySubVnd = "";
                        }
                        else if (ITurnKeySubVnd != "")
                        {
                            IMachineLabor = "";
                            IMachine = "";
                            ITurnKey = "";
                        }

                        string IStRate = tempProcesslistNew[6].ToString().Replace("NaN", "");
                        string IVendorRate = tempProcesslistNew[7].ToString();
                        string IProcUOM = tempProcesslistNew[8].ToString();
                        string[] ArIProcUOM = IProcUOM.Split('-');
                        if (ArIProcUOM.Count() > 0)
                        {
                            IProcUOM = ArIProcUOM[0].ToString().Trim();
                        }
                        string IBaseQty = tempProcesslistNew[9].ToString();
                        string IDPUOM = tempProcesslistNew[10].ToString();
                        string Iyield = tempProcesslistNew[11].ToString();
                        string ITurnKeyCost = tempProcesslistNew[12].ToString();
                        string ITurnKeyProfit = tempProcesslistNew[13].ToString();
                        string ICostPc = tempProcesslistNew[14].ToString();
                        string ITotalCost = tempProcesslistNew[15].ToString();

                        string UserId = Session["userID_"].ToString();
                        string sql = "insert into [dbo].[TProcessCostDetails_D] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn,TurnKeySubVnd,TurnKeyCost,TurnKeyProfit) "
                            + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On,@ITurnKeySubVnd,@ITurnKeyCost,@ITurnKeyProfit)";

                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                        cmd.Parameters.AddWithValue("@IProc", IProc);
                        cmd.Parameters.AddWithValue("@ISubProc", ISubProc);
                        cmd.Parameters.AddWithValue("@ITurnKey", ITurnKey);
                        cmd.Parameters.AddWithValue("@ITurnKeySubVnd", ITurnKeySubVnd);
                        cmd.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
                        cmd.Parameters.AddWithValue("@IMachine", IMachine);
                        cmd.Parameters.AddWithValue("@IStRate", IStRate);
                        cmd.Parameters.AddWithValue("@IVendorRate", IVendorRate);
                        cmd.Parameters.AddWithValue("@IProcUOM", IProcUOM);
                        cmd.Parameters.AddWithValue("@IBaseQty", IBaseQty);
                        cmd.Parameters.AddWithValue("@IDPUOM", IDPUOM);
                        cmd.Parameters.AddWithValue("@Iyield", Iyield);
                        cmd.Parameters.AddWithValue("@ITurnKeyCost", ITurnKeyCost);
                        cmd.Parameters.AddWithValue("@ITurnKeyProfit", ITurnKeyProfit);
                        cmd.Parameters.AddWithValue("@ICostPc", ICostPc);
                        cmd.Parameters.AddWithValue("@ITotalCost", ITotalCost);

                        cmd.Parameters.AddWithValue("@RowId", i);
                        cmd.Parameters.AddWithValue("@By", UserId);
                        cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        cmd.ExecuteNonQuery();
                    }
                }

                #endregion Process Values

                #region Material Cost Values
                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                {
                    DataTable DtDynamic = new DataTable();
                    DtDynamic = (DataTable)Session["DtDynamic"];

                    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                    string strMCode = "";
                    string strMDesc = "";
                    string strRawCost = "";
                    string strTotalRawCost = "";
                    string strPartUnitW = "";
                    string strDiaID = "";
                    string strDiaOD = "";
                    string strThick = "";
                    string strWidth = "";
                    string strPitch = "";
                    string strMDensity = "";
                    string strRunnerWeight = "";
                    string strRunnerRatio = "";
                    string strRecycle = "";
                    string strCavity = "";
                    string strMLoss = "";
                    string strMCrossWeight = "";
                    string strMScrapWeight = "";
                    string strScrapLoss = "";
                    string strScrapPrice = "";
                    string strScrapRebate = "";
                    string strMCostpcs = "";
                    string strTotalcostpcs = "";

                    string strRawCostUOM = "";
                    for (int i = 1; i <= ccc; i++)
                    {
                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                        var tempMClistNew = tempMClist;
                        if (i > 1)
                        {
                            tempMClistNew = tempMClistNew.Skip(((i - 1) * (DtDynamic.Rows.Count))).ToList();

                            #region Common for All
                            strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                            strMDesc = tempMClistNew[1].ToString();
                            strRawCost = tempMClistNew[2].ToString();

                            int Clm = (i - 1);
                            var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                            if (ArrRwMatUom.Count() >= Clm)
                            {
                                string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                strRawCostUOM = UOM;
                            }

                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                            }
                            else
                            {
                                strTotalRawCost = tempMClistNew[3].ToString();
                                strPartUnitW = tempMClistNew[4].ToString();
                            }
                            #endregion Common for all

                            #region IM / LAYOUT1
                            //if (txtprocs.Text == "IM")
                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strRunnerWeight = tempMClistNew[6].ToString();
                                strRunnerRatio = tempMClistNew[7].ToString();
                                strRecycle = tempMClistNew[8].ToString();

                                strMLoss = tempMClistNew[9].ToString();
                                strMCrossWeight = tempMClistNew[10].ToString();

                                strMCostpcs = tempMClistNew[11].ToString();
                                strTotalcostpcs = tempMClistNew[12].ToString();


                            }
                            #endregion IM

                            #region CA or SPR / layout3 / layout6
                            //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();
                            }

                            #endregion CA or SPR

                            #region ST / layout 5
                            //else if (txtprocs.Text == "ST")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                            {
                                strThick = tempMClistNew[5].ToString();
                                strWidth = tempMClistNew[6].ToString();
                                strPitch = tempMClistNew[7].ToString();
                                strMDensity = tempMClistNew[8].ToString();

                                strCavity = tempMClistNew[9].ToString();
                                strMLoss = tempMClistNew[10].ToString();
                                strMCrossWeight = tempMClistNew[11].ToString();

                                strMScrapWeight = tempMClistNew[12].ToString();
                                strScrapLoss = tempMClistNew[13].ToString();
                                strScrapPrice = tempMClistNew[14].ToString();
                                strScrapRebate = tempMClistNew[15].ToString();

                                strMCostpcs = tempMClistNew[16].ToString();
                                strTotalcostpcs = tempMClistNew[17].ToString();
                            }
                            #endregion ST

                            #region MS / layout 4
                            //else if (txtprocs.Text == "MS")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();

                                //strDiaID = tempMClistNew[5].ToString();
                                //strDiaOD = tempMClistNew[6].ToString();
                                //strWidth = tempMClistNew[7].ToString();

                                //strCavity = tempMClistNew[8].ToString();
                                //strMLoss = tempMClistNew[9].ToString();
                                //strMCrossWeight = tempMClistNew[10].ToString();

                                //strMCostpcs = tempMClistNew[11].ToString();
                                //strTotalcostpcs = tempMClistNew[12].ToString();
                            }
                            #endregion MS

                            #region layout 2
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                            {
                                strCavity = tempMClistNew[2].ToString();
                                strPartUnitW = tempMClistNew[1].ToString();

                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion layout 2

                            #region layout 7
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion layout 7

                            string UserId = Session["userID_"].ToString();
                            sql = "insert into [dbo].[TMCCostDetails_D] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn,RawMaterialCostUOM)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On,@RawMaterialCostUOM)";

                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd.Parameters.AddWithValue("@strThick", strThick);
                            cmd.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);
                            cmd.Parameters.AddWithValue("@RawMaterialCostUOM", strRawCostUOM);

                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", UserId);
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            #region Common for All
                            strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                            strMDesc = tempMClistNew[1].ToString();
                            strRawCost = tempMClistNew[2].ToString();

                            int Clm = (i - 1);
                            var ArrRwMatUom = hdnMCTableRawMatUom.Value.ToString().Replace("NaN", "").Split(',');
                            if (ArrRwMatUom.Count() >= Clm)
                            {
                                string UOM = ArrRwMatUom[Clm].ToString().Replace("NaN", "").ToUpper();
                                strRawCostUOM = UOM;
                            }

                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                            }
                            else
                            {
                                strTotalRawCost = tempMClistNew[3].ToString();
                                strPartUnitW = tempMClistNew[4].ToString();
                            }
                            #endregion Common for all

                            #region IM / LAYOUT1
                            //if (txtprocs.Text == "IM")
                            if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strRunnerWeight = tempMClistNew[6].ToString();
                                strRunnerRatio = tempMClistNew[7].ToString();
                                strRecycle = tempMClistNew[8].ToString();

                                strMLoss = tempMClistNew[9].ToString();
                                strMCrossWeight = tempMClistNew[10].ToString();

                                strMCostpcs = tempMClistNew[11].ToString();
                                strTotalcostpcs = tempMClistNew[12].ToString();


                            }
                            #endregion IM

                            #region CA or SPR / layout3 / layout6
                            //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();
                            }

                            #endregion CA or SPR

                            #region ST / layout 5
                            //else if (txtprocs.Text == "ST")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                            {
                                strThick = tempMClistNew[5].ToString();
                                strWidth = tempMClistNew[6].ToString();
                                strPitch = tempMClistNew[7].ToString();
                                strMDensity = tempMClistNew[8].ToString();

                                strCavity = tempMClistNew[9].ToString();
                                strMLoss = tempMClistNew[10].ToString();
                                strMCrossWeight = tempMClistNew[11].ToString();

                                strMScrapWeight = tempMClistNew[12].ToString();
                                strScrapLoss = tempMClistNew[13].ToString();
                                strScrapPrice = tempMClistNew[14].ToString();
                                strScrapRebate = tempMClistNew[15].ToString();

                                strMCostpcs = tempMClistNew[16].ToString();
                                strTotalcostpcs = tempMClistNew[17].ToString();
                            }
                            #endregion ST

                            #region MS / layout 4
                            //else if (txtprocs.Text == "MS")
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                            {
                                strCavity = tempMClistNew[5].ToString();
                                strMLoss = tempMClistNew[6].ToString();
                                strMCrossWeight = tempMClistNew[7].ToString();

                                strMCostpcs = tempMClistNew[8].ToString();
                                strTotalcostpcs = tempMClistNew[9].ToString();

                                //strDiaID = tempMClistNew[5].ToString();
                                //strDiaOD = tempMClistNew[6].ToString();
                                //strWidth = tempMClistNew[7].ToString();

                                //strCavity = tempMClistNew[8].ToString();
                                //strMLoss = tempMClistNew[9].ToString();
                                //strMCrossWeight = tempMClistNew[10].ToString();

                                //strMCostpcs = tempMClistNew[11].ToString();
                                //strTotalcostpcs = tempMClistNew[12].ToString();
                            }
                            #endregion MS

                            #region layout 2
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                            {
                                strCavity = tempMClistNew[2].ToString();
                                strPartUnitW = tempMClistNew[1].ToString();

                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion MS

                            #region layout 7
                            else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                            {
                                strMCostpcs = tempMClistNew[3].ToString();
                                strTotalcostpcs = tempMClistNew[4].ToString();
                            }
                            #endregion layout 2

                            string UserId = Session["userID_"].ToString();
                            sql = "insert into [dbo].[TMCCostDetails_D] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn,RawMaterialCostUOM)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On,@RawMaterialCostUOM)";

                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd.Parameters.AddWithValue("@strThick", strThick);
                            cmd.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);
                            cmd.Parameters.AddWithValue("@RawMaterialCostUOM", strRawCostUOM);

                            cmd.Parameters.AddWithValue("@RowId", i);
                            cmd.Parameters.AddWithValue("@By", UserId);
                            cmd.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                #endregion Process Values

                #endregion

                #region InsertQuoteDetailDraft
                string sqlInsr = "";
                sql = " select * from TQuoteDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, EmetConDraft);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlInsr = " ";
                }
                else
                {
                    sqlInsr = @" insert into TQuoteDetails_D (RequestNumber,RequestDate,QuoteNo,Plant,MaterialType,PlantStatus,SAPProcType,SAPSpProcType,Product,
                                    MaterialClass,Material,MaterialDesc,PIRType,ProcessGroup,VendorCode1,VendorName,PIRJobType,Remarks,
                                    NetUnit,DrawingNo,QuoteResponseDueDate,EffectiveDate,DueOn,TotalMaterialCost,TotalSubMaterialCost,
                                    TotalProcessCost,TotalOtheritemsCost,GrandTotalCost,FinalQuotePrice,Profit,Discount,VendorPIC,VendorPICEmail,
                                    ShimanoPIC,ShimanoPICEmail,CreateStatus,ApprovalStatus,PICApprovalStatus,PICReason,ManagerApprovalStatus,
                                    ManagerReason,DIRApprovalStatus,DIRReason,PlatingType,UpdatedBy,UpdatedOn,CreatedBy,CreatedOn,PIRStatus,
                                    PIRNumber,PIRCreatedDate,BaseUOM,CommentByVendor,CountryOrg,MQty,ERemarks,PerNetUnit,
                                    UOM,ActualNU,ApprovalDate,GA,QuoteNoRef,AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost,
                                    AprRejBy,AprRejDate,isUseSAPCode,FADate,FAQty,DelDate,DelQty,Incoterm,PckReqrmnt,OthReqrmnt,ReqPlant,
                                    ManagerRemark,DIRRemark,AprRejByMng,AprRejDateMng,VndAttchmnt,isMassRevision,PIRNo,OldTotMatCost,
                                    OldTotSubMatCost,OldTotProCost,OldTotOthCost,MassUpdateDate,EmpSubmitionOn,EmpSubmitionBy,SMNPicDept,
                                    PICRejReason,PICRejRemark,IsReSubmit,IMRecycleRatio,MassRevQutoteRef,IsUseToolAmortize,IsUseMachineAmortize,ToolAmorRemark,MachineAmorRemark)
                                select RequestNumber,RequestDate,QuoteNo,Plant,MaterialType,PlantStatus,SAPProcType,SAPSpProcType,Product,
                                    MaterialClass,Material,MaterialDesc,PIRType,ProcessGroup,VendorCode1,VendorName,PIRJobType,Remarks,
                                    NetUnit,DrawingNo,QuoteResponseDueDate,EffectiveDate,DueOn,TotalMaterialCost,TotalSubMaterialCost,
                                    TotalProcessCost,TotalOtheritemsCost,GrandTotalCost,FinalQuotePrice,Profit,Discount,VendorPIC,VendorPICEmail,
                                    ShimanoPIC,ShimanoPICEmail,CreateStatus,ApprovalStatus,PICApprovalStatus,PICReason,ManagerApprovalStatus,
                                    ManagerReason,DIRApprovalStatus,DIRReason,PlatingType,UpdatedBy,UpdatedOn,CreatedBy,CreatedOn,PIRStatus,
                                    PIRNumber,PIRCreatedDate,BaseUOM,CommentByVendor,CountryOrg,MQty,ERemarks,PerNetUnit,
                                    UOM,ActualNU,ApprovalDate,GA,QuoteNoRef,AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost,
                                    AprRejBy,AprRejDate,isUseSAPCode,FADate,FAQty,DelDate,DelQty,Incoterm,PckReqrmnt,OthReqrmnt,ReqPlant,
                                    ManagerRemark,DIRRemark,AprRejByMng,AprRejDateMng,VndAttchmnt,isMassRevision,PIRNo,OldTotMatCost,
                                    OldTotSubMatCost,OldTotProCost,OldTotOthCost,MassUpdateDate,EmpSubmitionOn,EmpSubmitionBy,SMNPicDept,
                                    PICRejReason,PICRejRemark,IsReSubmit,IMRecycleRatio,MassRevQutoteRef,IsUseToolAmortize,@IsUseMachineAmortize,ToolAmorRemark,MachineAmorRemark
                                    from TQuoteDetails where QuoteNo = @QuoteNo ";
                }
                reader.Dispose();

                if (sqlInsr != "")
                {
                    cmd = new SqlCommand(sqlInsr, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.Parameters.AddWithValue("@IsUseMachineAmortize", HdnIsUseMachineAmor.Value);
                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region UpdateQuoteDetDraft
                string TTMatCost = hdnTMatCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTMatCost.Value.ToString();
                string TSumMatCost = hdnTSumMatCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTSumMatCost.Value.ToString();
                string TProCost = hdnTProCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTProCost.Value.ToString();
                string TTOtherCost = hdnTOtherCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTOtherCost.Value.ToString();
                string TGTotal = hdnTGTotal.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTGTotal.Value.ToString();
                string TFinalQPrice = hdnTFinalQPrice.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTFinalQPrice.Value.ToString();
                string TProfit = hdnProfit.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnProfit.Value.ToString();
                string TDiscount = hdnDiscount.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnDiscount.Value.ToString();
                string TGA = hdnGA.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnGA.Value.ToString();
                if (TGA == "")
                {
                    TGA = "0";
                }

                sql = @"update " + TransDB.ToString() + @"TQuoteDetails_D set TotalMaterialCost=@TotalMaterialCost, TotalSubMaterialCost=@TotalSubMaterialCost, 
                                TotalProcessCost=@TotalProcessCost, TotalOtheritemsCost=@TotalOtheritemsCost, GrandTotalCost=@GrandTotalCost,FinalQuotePrice = @FinalQuotePrice,Profit=@Profit,Discount=@Discount,GA=@GA,
                                CommentByVendor=@CommentByVendor, UpdatedBy=@UpdatedBy , UpdatedOn=CURRENT_TIMESTAMP, ";

                if (TextBox1.Text != "")
                {
                    sql += " EffectiveDate = @EffectiveDate, ";
                }
                if (txtfinal.Text != "")
                {
                    sql += " DueOn=@DueOn, ";
                }

                if (Session["FlAttachment"] != null && TxtLbFlName.Text != "No File")
                {
                    HttpPostedFile Fl = (HttpPostedFile)Session["FlAttachment"];
                    if (Fl != null)
                    {
                        sql += " VndAttchmnt=@VndAttchmnt, ";
                    }
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text == "No File")
                {
                    sql += " VndAttchmnt=@VndAttchmnt, ";
                }
                sql += " countryorg=@countryorg Where QuoteNo =@QuoteNo ";

                if (Session["FlAttachment"] == null && TxtLbFlName.Text != "No File")
                {
                    sql += " update TQuoteDetails SET VndAttchmnt=@VndAttchmnt Where QuoteNo = @QuoteNo ";
                }

                string corg = "";
                if (Session["corg_"] != null)
                {
                    corg = Session["corg_"].ToString();
                }
                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                cmd.Parameters.AddWithValue("@TotalMaterialCost", TTMatCost);
                cmd.Parameters.AddWithValue("@TotalSubMaterialCost", TSumMatCost);
                cmd.Parameters.AddWithValue("@TotalProcessCost", TProCost);
                cmd.Parameters.AddWithValue("@TotalOtheritemsCost", TTOtherCost);
                cmd.Parameters.AddWithValue("@GrandTotalCost", TGTotal);
                cmd.Parameters.AddWithValue("@FinalQuotePrice", TFinalQPrice);
                cmd.Parameters.AddWithValue("@Profit", TProfit);
                cmd.Parameters.AddWithValue("@Discount", TDiscount);
                cmd.Parameters.AddWithValue("@GA", TGA);
                cmd.Parameters.AddWithValue("@CommentByVendor", TxtComntByVendor.Text);
                if (TextBox1.Text != "")
                {
                    DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@EffectiveDate", newEffDate);
                }
                if (txtfinal.Text != "")
                {
                    DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@DueOn", newDueon);
                }
                cmd.Parameters.AddWithValue("@UpdatedBy", (string)HttpContext.Current.Session["userID_"].ToString());
                cmd.Parameters.AddWithValue("@countryorg", ddlpirjtype.SelectedValue);
                if (Session["FlAttachment"] != null && TxtLbFlName.Text != "No File")
                {
                    HttpPostedFile file = (HttpPostedFile)Session["FlAttachment"];
                    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    if (file != null)
                    {
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string FileExtension = System.IO.Path.GetExtension(file.FileName);
                        string FileName = System.IO.Path.GetFileName(file.FileName);
                        cmd.Parameters.AddWithValue("@VndAttchmnt", hdnQuoteNo.Value + "-" + FileName);
                        string PathAndFileName = folderPath + hdnQuoteNo.Value + "-" + FileName;
                        file.SaveAs(PathAndFileName);
                    }

                    string OldFile = folderPath + LbFlNameOri.Text;
                    FileInfo Myfile = new FileInfo(OldFile);
                    if (Myfile.Exists) //check file exsit or not  
                    {
                        Myfile.Delete();
                    }

                    //FlAttachment = (FileUpload)Session["FlAttachment"];
                    //if (FlAttachment.HasFile)
                    //{
                    //    string FileName = FlAttachment.PostedFile.FileName;
                    //    cmd.Parameters.AddWithValue("@VndAttchmnt", hdnQuoteNo.Value + "-" + FileName);

                    //    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    //    if (!Directory.Exists(folderPath))
                    //    {
                    //        Directory.CreateDirectory(folderPath);
                    //    }

                    //    string OldFile = folderPath + LbFlNameOri.Text;
                    //    FileInfo file = new FileInfo(OldFile);
                    //    if (file.Exists) //check file exsit or not  
                    //    {
                    //        file.Delete();
                    //    }

                    //    string FileExtension = System.IO.Path.GetExtension(FlAttachment.FileName);
                    //    string filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                    //    string PathAndFileName = folderPath + hdnQuoteNo.Value + "-" + filename;
                    //    FlAttachment.SaveAs(PathAndFileName);

                    //}
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text == "No File")
                {
                    cmd.Parameters.AddWithValue("@VndAttchmnt", DBNull.Value);

                    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string OldFile = folderPath + LbFlNameOri.Text;
                    FileInfo Myfile = new FileInfo(OldFile);
                    if (Myfile.Exists) //check file exsit or not  
                    {
                        Myfile.Delete();
                    }

                    //string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    //if (!Directory.Exists(folderPath))
                    //{
                    //    Directory.CreateDirectory(folderPath);
                    //}
                    //string OldFile = folderPath + LbFlNameOri.Text;
                    //FileInfo file = new FileInfo(OldFile);
                    //if (file.Exists) //check file exsit or not  
                    //{
                    //    file.Delete();
                    //}
                }
                else if (Session["FlAttachment"] == null && TxtLbFlName.Text != "No File")
                {
                    cmd.Parameters.AddWithValue("@VndAttchmnt", LbFlNameOri.Text);
                }
                cmd.ExecuteNonQuery();
                #endregion

                EmetTrans.Commit();
                EmetTrans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MsgErr = ex.Message.ToString();
                EmetTrans.Rollback();
                EmetTrans.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
                EmetConDraft.Dispose();
            }
        }

        bool SendMail()
        {
            MsgErr = "";
            try
            {
                #region sending email
                bool sendingemail = false;
                string MsgErr = "";
                try
                {
                    //Email
                    //testing sending new
                    // getting Messageheader ID from IT Mailapp

                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMailConnString()))
                    {
                        string returnValue = string.Empty;
                        cnn.Open();
                        SqlTransaction transactionHS;
                        transactionHS = cnn.BeginTransaction("HeaderSelection");
                        try
                        {
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
                            MHid = returnValue;
                            Session["MHid"] = returnValue;
                            OriginalFilename = MHid + seqNo + formatW;
                        }

                        ///
                        catch (Exception xw)
                        {
                            MsgErr += "Error in Mail Headerselection " + xw + " ,";
                            transactionHS.Rollback();
                        }
                        cnn.Dispose();
                    }

                    Boolean IsAttachFile = true;
                    int SequenceNumber = 1;
                    string UserId = Session["userID_"].ToString();
                    IsAttachFile = false;
                    Session["SendFilename"] = "NOFILE";
                    OriginalFilename = "NOFILE";
                    format = "NO";

                    //getting vendor mail id
                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                    {
                        string returnValue = string.Empty;
                        cnn.Open();
                        SqlCommand cmdget = cnn.CreateCommand();
                        cmdget.CommandType = CommandType.StoredProcedure;
                        cmdget.CommandText = "dbo.Emet_Email_vendordetails1";

                        SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                        vendorid.Direction = ParameterDirection.Input;
                        vendorid.Value = hdnQuoteNo.Value;
                        cmdget.Parameters.Add(vendorid);

                        SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                        plant.Direction = ParameterDirection.Input;
                        plant.Value = Session["VPlant"].ToString();
                        cmdget.Parameters.Add(plant);

                        SqlDataReader dr;
                        dr = cmdget.ExecuteReader();
                        while (dr.Read())
                        {
                            // aemail = dr.GetString(0);
                            // pemail = dr.GetString(1);
                            // Session["Uemail"] = dr.GetString(0);
                            aemail = dr.GetString(1);
                            Session["aemail"] = dr.GetString(1);
                            pemail = dr.GetString(2);
                            Session["pemail"] = string.Concat(aemail, ";", pemail);

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
                        cmdget.CommandText = "dbo.Emet_Email_Vendormaildid";
                        SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                        vendorid.Direction = ParameterDirection.Input;
                        vendorid.Value = UserId;
                        cmdget.Parameters.Add(vendorid);

                        SqlDataReader dr;
                        dr = cmdget.ExecuteReader();
                        while (dr.Read())
                        {
                            Uemail = dr.GetString(0);
                            Session["Uemail"] = dr.GetString(0);

                        }
                        dr.Dispose();
                        cnn.Dispose();
                    }

                    //getting vendor mail content

                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenEMETConnString()))
                    {
                        string returnValue = string.Empty;
                        cnn.Open();
                        SqlCommand cmdget = cnn.CreateCommand();
                        cmdget.CommandType = CommandType.StoredProcedure;
                        cmdget.CommandText = "dbo.Emet_Email_content";

                        SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                        vendorid.Direction = ParameterDirection.Input;
                        vendorid.Value = hdnQuoteNo.Value;
                        cmdget.Parameters.Add(vendorid);

                        SqlDataReader dr;
                        dr = cmdget.ExecuteReader();
                        while (dr.Read())
                        {
                            body1 = dr.GetString(1);
                            Session["body1"] = dr.GetString(1);
                        }
                        dr.Dispose();
                        cnn.Dispose();
                    }
                    // Insert header and details to Mil server table to IT mailserverapp

                    using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                    {
                        Email_inser.Open();
                        //Header

                        UserId = Session["userID_"].ToString();

                        //Uemail = Session["Uemail"].ToString();
                        pemail = Session["pemail"].ToString();
                        body1 = Session["body1"].ToString();
                        nameC = Session["UserName"].ToString();

                        string MessageHeaderId = Session["MHid"].ToString();
                        string fromname = "eMET System";
                        //string FromAddress = Uemail;
                        string FromAddress = "eMET@Shimano.Com.sg";
                        nameC = Session["UserName"].ToString();
                        //string Recipient = aemail + "," + pemail;
                        string Recipient = Session["pemail"].ToString();
                        string CopyRecipient = Session["Createuser"].ToString();
                        string BlindCopyRecipient = "";
                        string ReplyTo = "subashdurai@shimano.com.sg";
                        string Subject = "Quotation Number: " + hdnQuoteNo.Value + " - Submitted By : " + nameC + " - Plant : " + Session["VPlant"].ToString();
                        //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                        //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                        string body = "";
                        if (lbTitle.Text.Contains("Revision"))
                        {
                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation  <font color='red'> (Revision) </font> has been submitted.<br /><br />" + body1.ToString();
                        }
                        else
                        {
                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted.<br /><br />" + body1.ToString();
                        }
                        string BodyFormat = "HTML";
                        string BodyRemark = "0";
                        string Signature = " ";
                        string Importance = "High";
                        string Sensitivity = "Confidential";

                        //string CreateUser = userId;
                        DateTime CreateDate = DateTime.Now;
                        //end Header
                        SqlTransaction transactionHe;
                        transactionHe = Email_inser.BeginTransaction("Header");
                        try
                        {
                            string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                            SqlCommand Header = new SqlCommand(Head, Email_inser);

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
                            Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                            Header.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                            Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            Header.CommandText = Head;
                            Header.ExecuteNonQuery();

                            transactionHe.Commit();
                        }

                        ///
                        catch (Exception xw)
                        {
                            transactionHe.Rollback();
                            MsgErr += "Fail Sending Email Header " + xw + " ,";
                        }
                        //end Header
                        //Details


                        SqlTransaction transactionDe;
                        transactionDe = Email_inser.BeginTransaction("Detail");
                        try
                        {
                            string SendFilename = Session["SendFilename"].ToString();
                            string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                            SqlCommand Detail = new SqlCommand(Details, Email_inser);
                            Detail.Transaction = transactionDe;
                            Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                            Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                            Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                            Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                            Detail.Parameters.AddWithValue("@SendFilename", SendFilename);
                            Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                            Detail.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                            Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            Detail.CommandText = Details;
                            Detail.ExecuteNonQuery();

                            transactionDe.Commit();
                        }
                        catch (Exception xw)
                        {
                            transactionDe.Rollback();
                            MsgErr += "Fail Send Email Detail " + xw + " ,";
                        }
                        Email_inser.Dispose();
                        //End Details
                    }
                    //End by subash
                    //End Email

                    sendingemail = true;
                }
                catch (Exception ex)
                {
                    MsgErr += ex.ToString();
                    sendingemail = false;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MsgErr += ex.Message.ToString();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            bool IsWithSAPCode = true;
            bool IsCanContinue = false;

            if (ddlpirjtype.Text == "-- select Country Of Origin --" || ddlpirjtype.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();alert('Please select Country of Origin');", true);
            }
            else
            {
                if (Session["Qno"] != null)
                {
                    string QuNo = Session["Qno"].ToString();
                    if (QuNo.Substring(QuNo.Length - 1) == "D" || QuNo.Substring(QuNo.Length - 2) == "GP")
                    {
                        IsWithSAPCode = false;
                    }

                    if (IsWithSAPCode == true)
                    {
                        if (TextBox1.Text == "")
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();alert('Effective Date should not be null')", true);
                        }
                        else if (txtfinal.Text == "")
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();alert('Due On should not be null')", true);
                        }
                        else
                        {
                            DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd-MM-yyyy", null);
                            DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd-MM-yyyy", null);
                            int result = DateTime.Compare(newEffDate, newDueon);
                            if (result > 0)
                            {
                                MsgErr = "Effective Date should not be Lessthan Due date";
                            }
                            else
                            {
                                if (hdnTGTotal.Value == "")
                                {
                                    MsgErr = "Could not submit without done calculation";
                                }
                                else
                                {
                                    IsCanContinue = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (txtfinal.Text == "")
                        {
                            MsgErr = "Due On should not be null";
                        }
                        else
                        {
                            IsCanContinue = true;
                        }
                    }

                    if (IsCanContinue == true)
                    {
                        if (EMETModule.IsSubmit(hdnQuoteNo.Value) == false)
                        {
                            if (ValidateCost() == true)
                            {
                                if (SubmitData() == true)
                                {
                                    if (SendMail() == true)
                                    {
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();alert('Submit Data Success');window.location ='Vendor.aspx';", true);
                                    }
                                    else
                                    {
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Send Maill Fail , Due On : " + MsgErr);
                                        var script = string.Format("alert({0});CloseLoading();window.location ='Vendor.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                }
                                else
                                {
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Save Data Fail , Due On : " + MsgErr);
                                    var script = string.Format("alert({0});CloseLoading();", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                            }
                            else {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(MsgErr);
                                var script = string.Format("alert({0});CloseLoading();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            }
                        }
                        else
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("This Quotation Already Submited !");
                            var script = string.Format("alert({0});CloseLoading();window.location ='Vendor.aspx';", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                    }
                    else {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(MsgErr);
                        var script = string.Format("alert({0});CloseLoading();", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                }
            }
        }

        protected void BtnSaveDraft_Click(object sender, EventArgs e)
        {
            if (EMETModule.IsSubmit(hdnQuoteNo.Value) == false)
            {
                if (SaveAsDraft() == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Save As Draft Successfully');window.location ='Vendor.aspx';", true);
                    //Response.Redirect("Vendor.aspx");
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();alert('Save As Draft Fail , Due On :" + MsgErr + "')", true);
                }
            }
            else
            {
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("This Quotation Already Submited !");
                var script = string.Format("alert({0});CloseLoading();window.location ='Vendor.aspx';", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["Table1"] = Table1;
        }

        //protected void btnAddPart_Click(object sender, EventArgs e)
        //{
        //    CreateDynamicUnitDT(0);
        //    UnitAddCount++;
        //    CreateDynamicUnitDT(UnitAddCount);
        //}

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static void UpdateSMCGrid(string S)
        {


        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect
            string qno = Session["Qno"].ToString();
            Response.Redirect("NewReq_changes.aspx?Number=" + qno.ToString());
        }

        private void RetrieveAllCostDetails()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                string SubVndCode = string.Empty;

                #region Other Cost

                string strGetData = "";

                strGetData = @"SELECT QuoteNo,ProcessGroup,UPPER(ItemsDescription) as 'ItemsDescription',[OtherItemCost/pcs] as 'OtherItemCost/pcs',
                iif(isnull([TotalOtherItemCost/pcs],'')= '','',convert(nvarchar(12),CAST(ROUND([TotalOtherItemCost/pcs],5) AS DECIMAL(12,5)))) as 'TotalOtherItemCost/pcs',RowId 
                FROM TOtherCostDetails_D Where  QuoteNo = @QuoteNo order by RowId asc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strGetData, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sb = new StringBuilder();

                string hdnvaltempNew = "";
                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtIDesc = dtget.Rows[i].ItemArray[2].ToString();
                        var txtOtherItemCost = dtget.Rows[i].ItemArray[3].ToString();
                        var txtTotalCost = dtget.Rows[i].ItemArray[4].ToString();

                        sb.Append(txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                        hdnvaltempNew = (txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                    }
                    //hdnSMCTableValues.Value = hdnvaltempNew.ToString();
                    hdnOtherValues.Value = sb.ToString();
                }

                #endregion Other Cost

                #region SubMat Cost

                string strsub = "";
                dtget = new DataTable();
                strsub = @"SELECT QuoteNo,ProcessGroup,UPPER([Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',[Sub-Mat/T&JCost] as 'Sub-Mat/T&JCost',
                            [Consumption(pcs)] as 'Consumption(pcs)',[Sub-Mat/T&JCost/pcs] as 'Sub-Mat/T&JCost/pcs',
                            iif(isnull([TotalSub-Mat/T&JCost/pcs],'')= '','',convert(nvarchar(12),CAST(ROUND([TotalSub-Mat/T&JCost/pcs],5) AS DECIMAL(12,5))))  as 'TotalSub-Mat/T&JCost/pcs',RowId 
                            FROM TSMCCostDetails_D Where  QuoteNo = @QuoteNo order by RowId asc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strsub, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sbSub = new StringBuilder();
                StringBuilder MachineAmorListTemp = new StringBuilder();
                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtsubDesc = dtget.Rows[i].ItemArray[2].ToString();
                        var txtsubcost = dtget.Rows[i].ItemArray[3].ToString();
                        var txtConsumption = dtget.Rows[i].ItemArray[4].ToString();
                        var txtsubcostPC = dtget.Rows[i].ItemArray[5].ToString();
                        var txtTotalCostPC = dtget.Rows[i].ItemArray[6].ToString();

                        MachineAmorListTemp.Append(txtsubDesc + ",");
                        sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + txtTotalCostPC + ",");
                    }
                    hdnSMCTableValues.Value = sbSub.ToString();
                    HdnMachinListprocCost.Value = MachineAmorListTemp.ToString();
                }

                #endregion SubMat Cost

                #region get sub vendor desc
                GetDbMaster();
                #endregion get sub vendor desc

                #region Process Cost

                string strProcess = "";
                dtget = new DataTable();
                strProcess = @"SELECT PCD.[QuoteNo],PCD.[ProcessGroup],PCD.[ProcessGrpCode],PCD.[SubProcess],PCD.[IfTurnkey-VendorName]
                                ,( CONCAT (RTRIM(PCD.[TurnKeySubVnd]),' - ',(select distinct Description from [" + DbMasterName + @"].[dbo].[tVendor_New] where Vendor = PCD.[TurnKeySubVnd])) ) as TurnKeySubVnd
                                ,PCD.[Machine/Labor],PCD.[Machine],CAST(ROUND(PCD.[StandardRate/HR],2) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(PCD.VendorRate,2) AS DECIMAL(12,2))as 'VendorRate'


                                ,isnull((select distinct top 1 PU.ProcessUomDescription from [" + DbMasterName + @"].[dbo].TPROCESGROUP_SUBPROCESS PG
                                join [" + DbMasterName + @"].[dbo].TPROCESS_UOM PU on PU.ProcessUom = PG.ProcessUOM 
                                where PG.SubProcessName = PCD.SubProcess and CONCAT(Pg.ProcessGrpCode,' - ',PG.ProcessGrpDescription) = PCD.ProcessGrpCode),PCD.[ProcessUOM]) as 'ProcessUOM'
                                --,PCD.[ProcessUOM]


                                ,PCD.[Baseqty],PCD.[DurationperProcessUOM(Sec)],PCD.[Efficiency/ProcessYield(%)]
                                ,PCD.[TurnKeyCost],PCD.[TurnKeyProfit],PCD.[ProcessCost/pc]
                                ,iif(isnull(PCD.[TotalProcessesCost/pcs],'')= '','',convert(nvarchar(12),CAST(ROUND(PCD.[TotalProcessesCost/pcs],5) AS DECIMAL(12,5)))) as [TotalProcessesCost/pcs]
                                ,PCD.[RowId],PCD.[UpdatedBy],PCD.[UpdatedOn] FROM  [TProcessCostDetails_D] PCD  Where PCD.[QuoteNo] = @QuoteNo order by RowId asc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strProcess, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sbProcess = new StringBuilder();

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtProc = dtget.Rows[i]["ProcessGrpCode"].ToString();
                        var txtsubProc = dtget.Rows[i]["SubProcess"].ToString();
                        var txtturnkey = dtget.Rows[i]["IfTurnkey-VendorName"].ToString();
                        var txtTurnKeySubVnd = dtget.Rows[i]["TurnKeySubVnd"].ToString();
                        if (txtTurnKeySubVnd.Trim() == "-")
                        {
                            txtTurnKeySubVnd = "";
                        }
                        var txtML = dtget.Rows[i]["Machine/Labor"].ToString();
                        var txtMachine = dtget.Rows[i]["Machine"].ToString();

                        var txtstanRate = dtget.Rows[i]["StandardRate/HR"].ToString();
                        var txtvendorRate = dtget.Rows[i]["VendorRate"].ToString();
                        var txtProcUOM = dtget.Rows[i]["ProcessUOM"].ToString();
                        var txtBaseQty = dtget.Rows[i]["Baseqty"].ToString();
                        var txtDPUOM = dtget.Rows[i]["DurationperProcessUOM(Sec)"].ToString();

                        var txtProcYeild = dtget.Rows[i]["Efficiency/ProcessYield(%)"].ToString();
                        var txtTurnKeyCost = dtget.Rows[i]["TurnKeyCost"].ToString();
                        var txtTurnKeyProfit = dtget.Rows[i]["TurnKeyProfit"].ToString();
                        var txtProcCost = dtget.Rows[i]["ProcessCost/pc"].ToString();
                        var txtProcTCost = dtget.Rows[i]["TotalProcessesCost/pcs"].ToString();

                        #region get value for proces uom is struk/min 
                        if (txtProcUOM.ToString().Contains("STROKES/MIN"))
                        {
                            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
                            try
                            {
                                MDMCon.Open();
                                string[] ArProcess_Grp_code = txtProc.ToString().Split('-');
                                string Process_Grp_code = ArProcess_Grp_code[0].ToString().Trim();
                                string[] ArMachineType = txtMachine.ToString().Split('-');
                                string MachineType = "";
                                int tonnage = 0;

                                string TypeAndToonage = GetDataMacTypNtoonageRetrive(ArMachineType[0].ToString());
                                string[] ArrTypeAndToonage = TypeAndToonage.Split(',');
                                if (ArrTypeAndToonage.Count() == 2)
                                {
                                    MachineType = ArrTypeAndToonage[0].ToString().Trim();
                                    tonnage = int.Parse(ArrTypeAndToonage[1].ToString());
                                }

                                SqlConnection con1;
                                SqlCommand cmd;
                                SqlDataReader reader;
                                string strplant = Session["strplant"].ToString();
                                string str1 = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                                             " where Plant=@Plant and Process_Grp_code=@Process_Grp_code " +
                                             " and MachineType=@MachineType and (@tonnage between Tonnage_From and Tonnage_To) ";
                                cmd = new SqlCommand(str1, MDMCon);
                                cmd.Parameters.AddWithValue("@Process_Grp_code", Process_Grp_code);
                                cmd.Parameters.AddWithValue("@Plant", strplant);
                                cmd.Parameters.AddWithValue("@MachineType", MachineType);
                                cmd.Parameters.AddWithValue("@tonnage", tonnage);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    txtProcUOM = txtProcUOM + '-' + reader["Strokes_min"].ToString();
                                }
                                reader.Dispose();
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex);
                            }
                            finally
                            {
                                MDMCon.Dispose();
                            }
                        }
                        #endregion

                        //var txtProc = dtget.Rows[i].ItemArray[2].ToString();
                        //var txtsubProc = dtget.Rows[i].ItemArray[3].ToString();
                        //var txtturnkey = dtget.Rows[i].ItemArray[4].ToString();
                        //var txtML = dtget.Rows[i].ItemArray[5].ToString();
                        //var txtMachine = dtget.Rows[i].ItemArray[6].ToString();

                        //var txtstanRate = dtget.Rows[i].ItemArray[7].ToString();
                        //var txtvendorRate = dtget.Rows[i].ItemArray[8].ToString();
                        //var txtProcUOM = dtget.Rows[i].ItemArray[9].ToString();
                        //var txtBaseQty = dtget.Rows[i].ItemArray[10].ToString();
                        //var txtDPUOM = dtget.Rows[i].ItemArray[11].ToString();

                        //var txtProcYeild = dtget.Rows[i].ItemArray[12].ToString();
                        //var txtProcCost = dtget.Rows[i].ItemArray[13].ToString();
                        //var txtProcTCost = dtget.Rows[i].ItemArray[14].ToString();

                        sbProcess.Append(txtProc + "," + txtsubProc + "," + txtturnkey + "," + txtTurnKeySubVnd + "," + txtML + "," + txtMachine + "," + txtstanRate + "," + txtvendorRate + "," + txtProcUOM + "," + txtBaseQty + "," + txtDPUOM + "," + txtProcYeild + "," + txtTurnKeyCost + "," + txtTurnKeyProfit + "," + txtProcCost + "," + txtProcTCost + ",");
                    }
                    hdnProcessValues.Value = sbProcess.ToString();
                }

                #endregion SubMat Cost

                #region MC Cost
                dtget = new DataTable();
                string strMCIM = "";

                strMCIM = @"SELECT TMC.QuoteNo,TMC.ProcessGroup,UPPER(MaterialSAPCode) as 'MaterialSAPCode',UPPER(MaterialDescription) as 'MaterialDescription',[RawMaterialCost/kg] as 'RawMaterialCost/kg',
                        CAST(ROUND([TotalRawMaterialCost/g],5) AS DECIMAL(12,4)) as 'TotalRawMaterialCost/g',[PartNetUnitWeight(g)] as '[PartNetUnitWeight(g)]',[~~DiameterID(mm)] as '~~DiameterID(mm)',[~~DiameterOD(mm)] as '~~DiameterOD(mm)', 
                        [~~Thickness(mm)] as '~~Thickness(mm)',[~~Width(mm)] as '~~Width(mm)', [~~Pitch(mm)] as '[~~Pitch(mm)]', [~MaterialDensity] as '~MaterialDensity',[~RunnerWeight/shot(g)] as '~RunnerWeight/shot(g)',
                        [~RunnerRatio/pcs(%)] as '~RunnerRatio/pcs(%)', [~RecycleMaterialRatio(%)] as '~RecycleMaterialRatio(%)', Cavity , [MaterialYield/MeltingLoss(%)] as 'MaterialYield/MeltingLoss(%)', 
                        CAST(ROUND([MaterialGrossWeight/pc(g)],5) AS DECIMAL(12,4)) as 'MaterialGrossWeight/pc(g)', [MaterialScrapWeight(g)] as 'MaterialScrapWeight(g)', [ScrapLossAllowance(%)] as 'ScrapLossAllowance(%)',
                        [ScrapPrice/kg] as 'ScrapPrice/kg', [ScrapRebate/pcs] as 'ScrapRebate/pcs', [MaterialCost/pcs] as 'MaterialCost/pcs', 
                        iif(isnull([TotalMaterialCost/pcs],'')= '','',convert(nvarchar(12),CAST(ROUND([TotalMaterialCost/pcs],5) AS DECIMAL(12,5)))) as 'TotalMaterialCost/pcs'
                        ,RowId 
                        ,isnull(
						case 
						when ((ISNULL(RawMaterialCostUOM,'') = '')  or (ISNULL(RawMaterialCostUOM,'') like '%' + 'SELECT UOM' + '%')) then (select distinct top 1 isnull([Component unit],'') as RawMaterialCostUOM from " + DbMasterName + @".dbo.TBOMLISTnew TB where tb.Plant = TQ.plant and TB.[Component Material] = TMC.MaterialSAPCode )
						else RawMaterialCostUOM
						end
						,'') as RawMaterialCostUOM
						
                        FROM TMCCostDetails_D  TMC
                        join TQuoteDetails_D TQ on TMC.QuoteNo = TQ.QuoteNo
                        Where  TMC.QuoteNo = @QuoteNo order by RowId asc ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strMCIM, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sbMCIM = new StringBuilder();
                StringBuilder sbMCRawMatUOM = new StringBuilder();

                string strMCode = "";
                string strMDesc = "";
                string strRawCost = "";
                string strTotalRawCost = "";
                string strPartUnitW = "";
                string strDiaID = "";
                string strDiaOD = "";
                string strThick = "";
                string strWidth = "";
                string strPitch = "";
                string strMDensity = "";
                string strRunnerWeight = "";
                string strRunnerRatio = "";
                string strRecycle = "";
                string strCavity = "";
                string strMLoss = "";
                string strMCrossWeight = "";
                string strMScrapWeight = "";
                string strScrapLoss = "";
                string strScrapPrice = "";
                string strScrapRebate = "";
                string strMCostpcs = "";
                string strTotalcostpcs = "";
                string strRawMaterialUOM = "";

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        strMCode = dtget.Rows[i].ItemArray[2].ToString();
                        strMDesc = dtget.Rows[i].ItemArray[3].ToString();
                        strRawCost = dtget.Rows[i].ItemArray[4].ToString();
                        strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString();
                        strPartUnitW = dtget.Rows[i].ItemArray[6].ToString();

                        strRawMaterialUOM = dtget.Rows[i]["RawMaterialCostUOM"].ToString();
                        sbMCRawMatUOM.Append(strRawMaterialUOM + ",");

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = dtget.Rows[i].ItemArray[13].ToString();
                            strRunnerRatio = dtget.Rows[i].ItemArray[14].ToString();
                            strRecycle = dtget.Rows[i].ItemArray[15].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strRunnerWeight + "," + strRunnerRatio + "," + strRecycle + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strRunnerWeight + "," + strRunnerRatio + "," + strRecycle + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion IM

                        #region CA or SPRT / layout3 / layout6
                        //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                        {
                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion CA or SPR

                        #region ST / layout 5
                        //else if (txtprocs.Text == "ST")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                        {
                            strThick = dtget.Rows[i].ItemArray[9].ToString();
                            strWidth = dtget.Rows[i].ItemArray[10].ToString();
                            strPitch = dtget.Rows[i].ItemArray[11].ToString();
                            strMDensity = dtget.Rows[i].ItemArray[12].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();


                            strMScrapWeight = dtget.Rows[i].ItemArray[19].ToString();
                            strScrapLoss = dtget.Rows[i].ItemArray[20].ToString();
                            strScrapPrice = dtget.Rows[i].ItemArray[21].ToString();
                            strScrapRebate = dtget.Rows[i].ItemArray[22].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();


                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strThick + "," + strWidth + "," + strPitch + "," + strMDensity + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMScrapWeight + "," + strScrapLoss + "," + strScrapPrice + "," + strScrapRebate + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion ST

                        #region MS / layout 4
                        //else if (txtprocs.Text == "MS")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                        {
                            //strDiaID = dtget.Rows[i].ItemArray[7].ToString();
                            //strDiaOD = dtget.Rows[i].ItemArray[8].ToString();
                            //strWidth = dtget.Rows[i].ItemArray[10].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion MS

                        #region layout 2
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                        {
                            strCavity = dtget.Rows[i].ItemArray[16].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strPartUnitW + "," + strCavity + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion layout 2

                        #region layout 7
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                        {
                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion layout 7
                    }
                    hdnMCTableValues.Value = sbMCIM.ToString();
                    hdnMCTableRawMatUom.Value = sbMCRawMatUOM.ToString();
                }

                #endregion MC Cost

                #region Unit Cost
                dtget = new DataTable();
                string strUnitData = "";

                //strUnitData = @"select CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                //            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                //            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                //            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost',
                //            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                //            Profit,Discount,
                //            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as 'FinalQuotePrice',
                //            CommentByVendor from TQuoteDetails_D  Where QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
                strUnitData = @"select 
                            iif(isnull(TotalMaterialCost,'')= '','',convert(nvarchar(12),CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)))) as 'TotalMaterialCost',
                            iif(isnull(TotalProcessCost,'')= '','',convert(nvarchar(12),CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)))) as 'TotalProcessCost',
                            iif(isnull(TotalSubMaterialCost,'')= '','',convert(nvarchar(12),CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)))) as 'TotalSubMaterialCost',
                            iif(isnull(TotalOtheritemsCost,'')= '','',convert(nvarchar(12),CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5))))  as 'TotalOtheritemsCost',
                            iif(isnull(GrandTotalCost,'')= '','',convert(nvarchar(12),CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)))) as 'GrandTotalCost',
                            Profit,Discount,
                            iif(isnull(FinalQuotePrice,'')= '','',convert(nvarchar(12),CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)))) as 'FinalQuotePrice',
                            CommentByVendor from TQuoteDetails_D  Where QuoteNo = @QuoteNo";

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(strUnitData, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value.ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(dtget);
                }

                StringBuilder sbUnit = new StringBuilder();

                //string hdnUnittemp = "";
                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        //var txtTMCost = dtget.Rows[i].ItemArray[0].ToString();
                        //var txtTPCost = dtget.Rows[i].ItemArray[1].ToString();
                        //var txtTSCost = dtget.Rows[i].ItemArray[2].ToString();
                        //var txtTOCost = dtget.Rows[i].ItemArray[3].ToString();
                        //var txtGrantCost = dtget.Rows[i].ItemArray[4].ToString();
                        //var txtProfit = dtget.Rows[i].ItemArray[5].ToString();
                        //var txtDiscount = dtget.Rows[i].ItemArray[6].ToString();
                        //var txtfinalCost = dtget.Rows[i].ItemArray[7].ToString();
                        //TxtComntByVendor.Text = dtget.Rows[i].ItemArray[8].ToString();

                        var txtTMCost = dtget.Rows[i]["TotalMaterialCost"].ToString();
                        var txtTPCost = dtget.Rows[i]["TotalProcessCost"].ToString();
                        var txtTSCost = dtget.Rows[i]["TotalSubMaterialCost"].ToString();
                        var txtTOCost = dtget.Rows[i]["TotalOtheritemsCost"].ToString();
                        var txtGrantCost = dtget.Rows[i]["GrandTotalCost"].ToString();
                        var txtProfit = dtget.Rows[i]["Profit"].ToString();
                        var txtDiscount = dtget.Rows[i]["Discount"].ToString();
                        var txtfinalCost = dtget.Rows[i]["FinalQuotePrice"].ToString();
                        TxtComntByVendor.Text = dtget.Rows[i]["CommentByVendor"].ToString();

                        hdnTMatCost.Value = txtTMCost;
                        hdnTProCost.Value = txtTPCost;
                        hdnTSumMatCost.Value = txtTSCost;
                        hdnTOtherCost.Value = txtTOCost;
                        hdnTGTotal.Value = txtGrantCost;
                        hdnTFinalQPrice.Value = txtfinalCost;

                        sbUnit.Append(txtTMCost + "," + txtTPCost + "," + txtTSCost + "," + txtTOCost + "," + txtGrantCost + "," + txtProfit + "," + txtDiscount + "," + txtfinalCost + ",");
                        //hdnUnittemp = (txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                    }
                    //hdnSMCTableValues.Value = hdnvaltempNew.ToString();
                    hdnUnitValues.Value = sbUnit.ToString();
                }

                #endregion Other Cost

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

        private void UnitCostDataStore()
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                var tempUnitlistcount = hdnUnitValues.Value.ToString().Split(',').ToList();

                var ccc = tempUnitlistcount.Count / DtDynamicUnitFields.Rows.Count;

                if (hdnVendorType.Value == "TeamShimano")
                {
                    CreateDynamicUnitDTTmShimano(0);
                }
                else
                {
                    CreateDynamicUnitDT(0);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        private void OthersCostDataStore()
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;
                CreateDynamicOthersCostDT(0);
                if (ccc > 1)
                    CreateDynamicOthersCostDT(ccc);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void RegenerateSubmatData()
        {
            try
            {
                GetDataToolAmor(hdnQuoteNo.Value, "", false);
                if (HdnMachinListprocCost.Value != "")
                {
                    GetDataMachineAmorBasedOnMacIdProcCostSeleced();
                }

                if (HdnIsUseMachineAmor.Value == "0")
                {
                    Session["IsMacAmorAdded"] = null;
                }

                #region remove old Machine amortize from sub mat cost
                if (Session["OldMachineAmorList"] != null)
                {
                    StringBuilder sbSubmatReplace = new StringBuilder();
                    decimal TotCostPc = 0;
                    DataTable DtOldMatAmorList = (DataTable)Session["OldMachineAmorList"];
                    if (DtOldMatAmorList.Rows.Count > 0)
                    {
                        String CurrVal = hdnSMCTableValues.Value.Replace("NaN", "");
                        List<string> Arrtotal = CurrVal.Split(',').ToList();
                        //string SmCostTot = "0";

                        if (Arrtotal.Count > 0)
                        {
                            DataTable DtDynamicSubMaterialsFields = new DataTable();
                            DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];
                            int TotCol = (Arrtotal.Count) / DtDynamicSubMaterialsFields.Rows.Count;
                            int nextcoltoempty = (DtDynamicSubMaterialsFields.Rows.Count - 1);
                            for (int i = 0; i < TotCol; i++)
                            {
                                for (int l = 0; l < Arrtotal.Count; l++)
                                {
                                    if (l == nextcoltoempty)
                                    {
                                        Arrtotal[l] = "";
                                        nextcoltoempty = nextcoltoempty + (DtDynamicSubMaterialsFields.Rows.Count);
                                        break;
                                    }
                                }
                            }

                            CurrVal = string.Join(",", Arrtotal);
                            //SmCostTot = Arrtotal[Arrtotal.Length - 2].ToString();
                        }

                        for (int MacAmorC = 0; MacAmorC < DtOldMatAmorList.Rows.Count; MacAmorC++)
                        {
                            var txtsubDesc = DtOldMatAmorList.Rows[MacAmorC]["Vend_MachineID"].ToString();
                            //var txtsubcost = DtOldMatAmorList.Rows[MacAmorC]["AmortizeCost"].ToString();
                            var txtsubcost = DtOldMatAmorList.Rows[MacAmorC]["AmortizeCost_Vend_Curr"].ToString();
                            var txtConsumption = DtOldMatAmorList.Rows[MacAmorC]["TotalAmortizeQty"].ToString();
                            float SubCost = float.Parse(txtsubcost) / float.Parse(txtConsumption);
                            var txtsubcostPC = SubCost.ToString("0.000000");
                            sbSubmatReplace.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + "" + ",");
                        }

                        String RepVal = sbSubmatReplace.ToString();
                        String FinalVal = CurrVal.Replace(RepVal, @"");
                        hdnSMCTableValues.Value = FinalVal;

                        Session["OldMachineAmorList"] = null;
                        Session["IsMacAmorAdded"] = false;
                    }
                }
                #endregion

                //HdnIsUseMachineAmor.Value = "";
                if (HdnIsUseMachineAmor.Value == "1")
                {
                    if (Session["VndMachineAmortize"] != null)
                    {
                        DataTable dtMacAmor = (DataTable)Session["VndMachineAmortize"];
                        if (dtMacAmor.Rows.Count > 0)
                        {

                            DataTable dtToolAmor = new DataTable();
                            if (Session["VndToolAmortize"] != null)
                            {
                                dtToolAmor = (DataTable)Session["VndToolAmortize"];
                                if (dtToolAmor.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtToolAmor.Rows)
                                    {
                                        if (dr["OldToolAmor"].ToString() == "1")
                                            dr.Delete();
                                    }
                                }
                            }

                            DataTable DtDynamicSubMaterialsFields = new DataTable();
                            DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                            var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();
                            var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;
                            //List<int> part = new List<int>();
                            //for (int c = 0; c < tempSublistcount.Count; c++)
                            //{
                            //    int level = c + (DtDynamicSubMaterialsFields.Rows.Count);
                            //    if (level < tempSublistcount.Count) {
                            //        part.Add(level);
                            //        c = (level - 1);
                            //    }
                            //}

                            int MaxFirstDataUntil = 0;
                            if (dtToolAmor.Rows.Count > 0)
                            {
                                MaxFirstDataUntil = dtToolAmor.Rows.Count * DtDynamicSubMaterialsFields.Rows.Count;
                            }
                            bool IsMacAmorAdded = false;
                            if (Session["IsMacAmorAdded"] != null)
                            {
                                IsMacAmorAdded = (bool)Session["IsMacAmorAdded"];
                            }

                            StringBuilder sbSub = new StringBuilder();
                            string NewSubMatList = "";
                            decimal TotCostPc = 0;
                            for (int i = 0; i < tempSublistcount.Count; i++)
                            {
                                if (dtToolAmor.Rows.Count > 0)
                                {
                                    if (i < MaxFirstDataUntil)
                                    {
                                        NewSubMatList += tempSublistcount[i].ToString() + ",";
                                    }
                                    else
                                    {
                                        if (IsMacAmorAdded == false)
                                        {
                                            for (int MacAmorC = 0; MacAmorC < dtMacAmor.Rows.Count; MacAmorC++)
                                            {
                                                var txtsubDesc = dtMacAmor.Rows[MacAmorC]["Vend_MachineID"].ToString();
                                                //var txtsubcost = dtMacAmor.Rows[MacAmorC]["AmortizeCost"].ToString();
                                                var txtsubcost = dtMacAmor.Rows[MacAmorC]["AmortizeCost_Vend_Curr"].ToString();
                                                var txtConsumption = dtMacAmor.Rows[MacAmorC]["TotalAmortizeQty"].ToString();
                                                var txtsubcostPC = dtMacAmor.Rows[MacAmorC]["AmortizeCost_Pc_Vend_Curr"].ToString();
                                                for (int c = 0; c < dtMacAmor.Rows.Count; c++)
                                                {
                                                    TotCostPc = TotCostPc + decimal.Parse(dtMacAmor.Rows[MacAmorC]["AmortizeCost_Pc_Vend_Curr"].ToString());
                                                }
                                                decimal txtTotalCostPC = TotCostPc;
                                                sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + String.Format("{0:0.00000}", txtTotalCostPC) + ",");
                                            }
                                            NewSubMatList += sbSub.ToString();
                                            Session["IsMacAmorAdded"] = true;
                                            IsMacAmorAdded = true;
                                            Session["OldMachineAmorList"] = dtMacAmor;
                                            i--;
                                        }
                                        else if (Session["IsMacAmorAdded"] == null)
                                        {
                                            NewSubMatList += tempSublistcount[i - 1].ToString() + ",";
                                        }
                                        else
                                        {
                                            NewSubMatList += tempSublistcount[i].ToString() + ",";
                                        }
                                    }
                                }
                                else
                                {
                                    if (IsMacAmorAdded == false)
                                    {
                                        for (int MacAmorC = 0; MacAmorC < dtMacAmor.Rows.Count; MacAmorC++)
                                        {
                                            var txtsubDesc = dtMacAmor.Rows[MacAmorC]["Vend_MachineID"].ToString();
                                            //var txtsubcost = dtMacAmor.Rows[MacAmorC]["AmortizeCost"].ToString();
                                            var txtsubcost = dtMacAmor.Rows[MacAmorC]["AmortizeCost_Vend_Curr"].ToString();
                                            var txtConsumption = dtMacAmor.Rows[MacAmorC]["TotalAmortizeQty"].ToString();
                                            var txtsubcostPC = dtMacAmor.Rows[MacAmorC]["AmortizeCost_Pc_Vend_Curr"].ToString();
                                            for (int c = 0; c < dtMacAmor.Rows.Count; c++)
                                            {
                                                TotCostPc = TotCostPc + decimal.Parse(dtMacAmor.Rows[MacAmorC]["AmortizeCost_Pc_Vend_Curr"].ToString());
                                            }
                                            decimal txtTotalCostPC = TotCostPc;
                                            sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + String.Format("{0:0.00000}", txtTotalCostPC) + ",");
                                        }
                                        NewSubMatList += sbSub.ToString();
                                        Session["IsMacAmorAdded"] = true;
                                        Session["OldMachineAmorList"] = dtMacAmor;
                                        IsMacAmorAdded = true;
                                        i--;
                                    }
                                    //else if (Session["IsMacAmorAdded"] == null)
                                    //{
                                    //    NewSubMatList += tempSublistcount[i - 1].ToString() + ",";
                                    //}
                                    else
                                    {
                                        if (NewSubMatList == "")
                                        {
                                            NewSubMatList += tempSublistcount[i].ToString() + ",";
                                        }
                                        else
                                        {
                                            NewSubMatList += tempSublistcount[i].ToString() + ",";
                                        }
                                    }
                                }
                            }
                            var tempSubMatList = NewSubMatList.ToString().Split(',').ToList();
                            hdnSMCTableValues.Value = NewSubMatList.ToString();
                            //hdnSMCTableValues.Value = "TESTING123,10000.00,10000,1.000000,1.50000,IM100-120TGF,12000.00,200000,0.060000,0.12000,IM70T,10000.00,100,100.000000,200.12000,CARTON BOX,1,2,0.500000,1.50000,,,,,";
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

        private void subMatCostDataStore()
        {
            try
            {

                DataTable DtDynamicSubMaterialsFields = new DataTable();
                DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                CreateDynamicSubMaterialDT(0);

                if (ccc > 1)
                {

                    CreateDynamicSubMaterialDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void ProcessCostDataStore()
        {
            try
            {

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                CreateDynamicProcessDT(0);
                if (ccc > 1)
                {
                    CreateDynamicProcessDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        private void CekTableproc()
        {
            //if (ListColumnUseSubVnder != "")
            //{
            //    string[] ArListColumnUseSubVnder = ListColumnUseSubVnder.Split(',');
            //    for (int k = 0; k < ArListColumnUseSubVnder.Count() - 1; k++)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "SubVendorData(" + ArListColumnUseSubVnder[k].ToString() + ");", true);
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "SubVendorData(" + ArListColumnUseSubVnder[k].ToString() + ");", true);
            //    }
            //}
        }

        private void MCCostDataStore()
        {
            try
            {

                DataTable DtDynamic = new DataTable();
                DtDynamic = (DataTable)Session["DtDynamic"];

                var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

                var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                CreateDynamicDT(0);
                if (ccc > 1)
                {
                    CreateDynamicDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        protected void btnClickCalcPrcUOMStkMin_Click(object sender, EventArgs e)
        {
            GetDataMacTypNtoonage();
            GetDataPrcGroupVsStukMin();
        }

        protected void BtnFndVndMachine_Click(object sender, EventArgs e)
        {
            GetDataSubProcesGroup();
            GetDataDataVndmachine();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreSubprocGroup(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "StoreSubprocGroup(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
        }

        protected void BtnProcMachineAmor_Click(object sender, EventArgs e)
        {
            try
            {
                RegenerateSubmatData();
                subMatCostDataStore();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "submatlcost();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnFndVndMachineVsProcUom_Click(object sender, EventArgs e)
        {
            GetDataDataVndmachine();
            GetDataProcesUOM();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "CallMyFunction", "StoreProcUOM(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreProcUOM(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
        }

        protected void BtnFndProcUom_Click(object sender, EventArgs e)
        {
            GetDataProcesUOM();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "CallMyFunction", "StoreProcUOM(" + ColumnNo + ");", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreProcUOM(" + ColumnNo + ");", true);
        }

        protected void BtnFndVndRate_Click(object sender, EventArgs e)
        {
            string ProcGroup = hdnProcGroup.Value.ToString();
            string MachineId = hdnMachineId.Value.ToString();
            //MachineId = getMachineID();
            GetDataVendRate(ProcGroup, MachineId);
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "CallMyFunction", "StoreVndRate(" + ColumnNo + ");", true);
            //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "StoreVndRate(" + ColumnNo + ");", true);
        }

        protected void BtnMacList_Click(object sender, EventArgs e)
        {
            GetDataDataVndmachine();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            System.Web.UI.ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "CallMyFunction", "StoreVnderMachineList(" + ColumnNo + ");", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreVnderMachineList(" + ColumnNo + ");", true);
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
                Response.Write(ex);
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

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //if (FlAttachment.HasFile)
                //{
                //    LbFlName.Text = FlAttachment.PostedFile.FileName;
                //    Session["FlAttachment"] = FlAttachment;
                //}
                //MCCostDataStore();
                //ProcessCostDataStore();
                //subMatCostDataStore();
                //OthersCostDataStore();
                //UnitCostDataStore();
                
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "returnProfDiscGA();MatlCalculation();othercost();submatlcost(); RestoreDataTbaleUnit();ReCalculate();AllRecalculate();ComntByVendorLght();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnPreview_Click(object sender, EventArgs e)
        {
            //if (Session["FlAttachment"] != null)
            //{
            //    FlAttachment = (FileUpload)Session["FlAttachment"];
            //    if (FlAttachment.HasFile)
            //    {
            //        string folderPath = Server.MapPath("~/FileVendorAttachmant/");
            //        if (!Directory.Exists(folderPath))
            //        {
            //            Directory.CreateDirectory(folderPath);
            //        }

            //        string FileExtension = System.IO.Path.GetExtension(FlAttachment.FileName);
            //        string filename = System.IO.Path.GetFileName(FlAttachment.FileName);
            //        string PathAndFileName = folderPath + DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
            //        FlAttachment.SaveAs(PathAndFileName);

            //        if (filename != "")
            //        {
            //            FileInfo fileCheck = new FileInfo(PathAndFileName);
            //            if (fileCheck.Exists) //check file exsit or not  
            //            {
            //                Response.Clear();
            //                Response.Buffer = true;
            //                Response.ContentType = "application/" + FileExtension.Replace(".", "") + "";
            //                Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
            //                Response.Charset = "";
            //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //                Response.TransmitFile(PathAndFileName);
            //                Response.Flush();
            //                FileInfo file = new FileInfo(PathAndFileName);
            //                if (file.Exists) //check file exsit or not  
            //                {
            //                    file.Delete();
            //                }
            //                Response.End();
            //            }
            //        }
            //    }
            //}
            //else if (Session["FlAttachment"] == null && TxtLbFlName.Text != "No File")
            //{
            //    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
            //    if (!Directory.Exists(folderPath))
            //    {
            //        Directory.CreateDirectory(folderPath);
            //    }

            //    string FileExtension = "pdf";
            //    string filename = LbFlNameOri.Text;
            //    string[] Arrfilename = filename.Split('.');
            //    int c = Arrfilename.Count();
            //    if (c > 0)
            //    {
            //        FileExtension = Arrfilename[c-1].ToString();
            //    }
            //    string PathAndFileName = folderPath + filename;

            //    if (filename != "")
            //    {
            //        FileInfo fileCheck = new FileInfo(PathAndFileName);
            //        if (fileCheck.Exists) //check file exsit or not  
            //        {
            //            Response.Clear();
            //            Response.Buffer = true;
            //            Response.ContentType = "application/" + FileExtension.Replace(".", "") + "";
            //            Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
            //            Response.Charset = "";
            //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //            Response.TransmitFile(PathAndFileName);
            //            Response.Flush();
            //            Response.End();
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File deleted !')", true);
            //            LbFlName.Text = "No File";
            //            UnitCostDataStore();
            //            OthersCostDataStore();
            //            subMatCostDataStore();
            //            ProcessCostDataStore();
            //            MCCostDataStore();
            //            Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "returnProfDiscGA();RestoreDataTbaleUnit();AllRecalculate();ComntByVendorLght();", true);
            //        }
            //    }
            //}
        }

        protected void BtnDelAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["FlAttachment"] != null)
                //{
                //    Session["FlAttachment"] = null;
                //    LbFlName.Text = "No File";
                //}
                //else
                //{
                //    LbFlName.Text = "No File";
                //}
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void LbRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["FlAttachment"] = null;
                //LbFlName.Text = LbFlNameOri.Text.Replace(hdnQuoteNo.Value + "-", "");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvQuoreReqRevice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton LbGvQNo = e.Row.FindControl("LbGvQNo") as LinkButton;
                    string LbGvQNoID = ((LinkButton)e.Row.FindControl("LbGvQNo")).ClientID;
                    LbGvQNo.Attributes.Add("onclick", "openInNewTabDynamic('VViewRequest.aspx','" + LbGvQNoID + "')");
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvQuoreReqRevice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LinktoRedirect")
            {
                Response.Redirect("VViewRequest.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
            }
        }

        protected void GvProcessGrpVsLayout_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("Login.aspx");
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




        
    }
}