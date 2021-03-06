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
using System.Threading;

namespace Material_Evaluation
{
    public partial class VViewRequestMassRev : System.Web.UI.Page
    {
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;

        string nameC;//
        string aemail;//
        string pemail;//
        string Uemail;//
        string body1;//
        string vendorC;//
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        public static string password;
        public static string domain;
        public static string path;

        string PlantDesc = "";
        string SMNPICSubmDept = "";
        string GA = "";
        string DbMasterName = "";
        string DbTransName = "";
        string Status = "";
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID_"] == null || Session["VendorType"] == null)
                {
                    Response.Redirect("Login.aspx");
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

                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_Vendorsubmission";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            string UserId = Session["userID_"].ToString();

                            if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                            {
                                LoadSavings();
                                string userId = Session["userID_"].ToString();
                                string sname = Session["UserName"].ToString();
                                string srole = Session["userType"].ToString();
                                string concat = sname + " - " + srole;
                                lblUser.Text = sname;
                                lblplant.Text = srole;


                                string QuoteNo = Request.QueryString["Number"];
                                Session["Qno"] = Request.QueryString["Number"];
                                Session["MtlAddCount"] = 1;
                                Session["PCAddCount"] = 1;
                                Session["SMCAddCount"] = 1;
                                Session["OthersAddCount"] = 1;
                                lblreqst.Text = ": " + QuoteNo.ToString();
                                if (Convert.ToString(Session["SystemVersion"]) == "")
                                {

                                    Session["SystemVersion"] = "NIL";
                                    LbsystemVersion.Text = Session["SystemVersion"].ToString();
                                }
                                else
                                {
                                    LbsystemVersion.Text = Session["SystemVersion"].ToString();
                                }

                                if (Session["VendorType"] != null)
                                {
                                    hdnVendorType.Value = Session["VendorType"].ToString();
                                }
                                else
                                {
                                    hdnVendorType.Value = "External";
                                }

                                //MtlAddCount = 1;
                                //PCAddCount = 1;
                                //SMCAddCount = 1;
                                //OthersAddCount = 1;
                                //UnitAddCount = 1;
                                //metfields = new List<string>();
                                //DtDynamic = new DataTable();
                                //DtMaterial = new DataTable();
                                //DicQuoteDetails = new Dictionary<string, string>();
                                //GetReqPlant(QuoteNo.Remove(0, 3).Replace("GP", ""));
                                GetQuoteandAllDetails(QuoteNo);
                                GetQuoteDetailsbyQuotenumber(QuoteNo);
                                CreateDynamicTablebasedonProcessField();
                                GetQuoteupdated(QuoteNo);
                                string struser = (string)HttpContext.Current.Session["userID_"].ToString();
                                GetSHMNPICDetails(struser);
                                // GetData(QuoteNo);
                                GetData(QuoteNo);
                                process();
                                if (txtPIRtype.Text.Contains("SUBCON"))
                                {
                                    //btnAddColumns.Enabled = false;
                                }
                                RetrieveAllCostDetails();
                                UnitCostDataStore();
                                OthersCostDataStore();
                                subMatCostDataStore();
                                ProcessCostDataStore();
                                MCCostDataStore();
                                Countryoforigin();

                                SetTitleForm(QuoteNo);
                                //CreateDynamicOthersCostDT(0);
                                //CreateDynamicDT(0);
                                //CreateDynamicProcessDT(0);
                                //CreateDynamicSubMaterialDT(0);
                                //CreateDynamicUnitDT(0);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "RestoreDataTbaleUnit();", true);
                            }
                        }

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

        protected void SetTitleForm(string QN)
        {
            try
            {
                if (QN.Substring(QN.Length - 1, 1) == "D")
                {
                    lbTitle.Text = "Quote Request Without SAP Code : Review";
                }
                else if (QN.Substring(QN.Length - 2, 2) == "GP")
                {
                    lbTitle.Text = "Quote Request Without SAP Code (GP) : Review";
                }
                else
                {
                    lbTitle.Text = "Quote Request With SAP Code : Review";
                }

                if (hdnQuoteNoRef.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + " &nbsp; - Revision ";
                }
                else if (hdnMassRevision.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + "&nbsp;  - Mass Revision";
                }


                string Stts = GetQuoteStatus(QN);
                if (Stts == "(Approved)")
                {
                    LbStatus.ForeColor = Color.Green;
                }
                else if (Stts == "(Rejected)")
                {
                    LbStatus.ForeColor = Color.Red;
                }
                else
                {
                    LbStatus.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0066cc");
                }

                Stts = "(Waiting for Confirmation)";
                LbStatus.Text = Stts;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
        }

        string GetQuoteStatus(string QN)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" SELECT 
                            CASE
                                WHEN (ManagerApprovalStatus = 2 and DIRApprovalStatus = 0) THEN '(Approved)'
                                WHEN (ManagerApprovalStatus = 1 and DIRApprovalStatus = 0) THEN '(Rejected)'
                                WHEN (ManagerApprovalStatus = 5 and DIRApprovalStatus = 5) THEN '(No Need Approval)'
	                            ELSE '(Waiting for SMN Approval)'
                            END AS 'Status'
                            FROM TQuoteDetails where QuoteNo = @QuoteNo";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QN);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Status = dt.Rows[0]["Status"].ToString();
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
                EmetCon.Dispose();
            }
            return Status;
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
                sql = @"select GA from TQuoteDetails where QuoteNo=@QuoteNo";

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

        protected void Countryoforigin()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select countrycode,CountryDescription,Currency from TCountrycode where active=1";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {

                }
                else
                {
                    //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
                }
                string QuoteNo = Session["Qno"].ToString();
                str = "select TQ.CountryOrg, TC.CountryDescription from " + TransDB.ToString() + "TQuoteDetails TQ INNER JOIN TCountrycode TC ON TC.CountryCode=TQ.CountryOrg WHERE QuoteNo = '" + QuoteNo + "'";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    //corg = Result.Rows[0]["CountryOrg"].ToString();
                    txtCorigin.Text = Result.Rows[0]["CountryDescription"].ToString();
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

        protected void GetOldQuotePIRMass(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select PIRNo,MassRevQutoteRef,
                            CAST(ROUND(TQ.OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost,
                            CAST(ROUND(TQ.OldTotSubMatCost,5) AS DECIMAL(12,5)) as OldTotSubMatCost,
                            CAST(ROUND(TQ.OldTotProCost,5) AS DECIMAL(12,5)) as OldTotProCost,
                            CAST(ROUND(TQ.OldTotOthCost,5) AS DECIMAL(12,5)) as OldTotOthCost,
                            FORMAT(MassUpdateDate,'dd/MM/yyyy hh:mm:ss') as MassUpdateDate 
                            from TQuoteDetails_D TQ 
                            where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            hdnQuoteNoRefmassRev.Value = dt.Rows[0]["MassRevQutoteRef"].ToString();
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

        protected string GetDataMacTypNtoonageRetrive(string MachineID)
        {
            string TypeAndToonage = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string str = " select MachineType,Tonnage from TVENDORMACHNLIST where Plant='" + strplant + "' and MachineID= '" + MachineID + "' and DELFLAG = 0  ";
                da = new SqlDataAdapter(str, MDMCon);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    TypeAndToonage = DtDataMacVnd.Rows[0]["MachineType"].ToString() + "," + DtDataMacVnd.Rows[0]["Tonnage"].ToString();
                }
                MDMCon.Dispose();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            return TypeAndToonage;
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
                        //Userid = dr.GetString(0);
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;

            }

        }

        private void OthersCostDataStore()
        {
            try
            {
                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                if (DtDynamicOtherCostsFields.Rows.Count > 0)
                {
                    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                    var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;
                    CreateDynamicOthersCostDT(0);
                    if (ccc > 1)
                        CreateDynamicOthersCostDT(ccc);
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

                if (DtDynamicSubMaterialsFields.Rows.Count > 0)
                {
                    var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                    CreateDynamicSubMaterialDT(0);

                    if (ccc > 1)
                    {

                        CreateDynamicSubMaterialDT(ccc);

                    }
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

                if (DtDynamicProcessFields.Rows.Count > 0)
                {
                    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                    CreateDynamicProcessDT(0);
                    if (ccc > 1)
                    {

                        CreateDynamicProcessDT(ccc);

                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void MCCostDataStore()
        {
            try
            {

                DataTable DtDynamic = new DataTable();
                DtDynamic = (DataTable)Session["DtDynamic"];

                if (DtDynamic.Rows.Count > 0)
                {
                    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                    CreateDynamicDT(0);


                    if (ccc > 1)
                    {

                        CreateDynamicDT(ccc);

                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        private void UnitCostDataStore()
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                if (DtDynamicUnitFields.Rows.Count > 0)
                {
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
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        // Used this method by Raja
        private void GetQuoteDetailsbyQuotenumber(string QuoteNo)

        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select * from " + TransDB.ToString() + "TQuoteDetails where QuoteNo='" + QuoteNo + "' and CreateStatus ='Article' ";
                da = new SqlDataAdapter(str, EmetCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();
                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();
                    string VendorCode = dtdate.Rows[0].ItemArray[14].ToString();
                    string ProcessGrp = dtdate.Rows[0].ItemArray[13].ToString();
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

                    string PartUnit = dtdate.Rows[0].ItemArray[18].ToString();

                    //strplant = plant;
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
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select top 1 A.RequestNumber,RequestDate,A.QuoteNo,Plant,MaterialType,PlantStatus
                                , SAPProcType, SAPSpProcType, Product, MaterialClass, A.Material, MaterialDesc, PIRType
                                , ProcessGroup, A.VendorCode1, VendorName, PIRJobType, Remarks, NetUnit
                                , DrawingNo, QuoteResponseDueDate,format( A.EffectiveDate, 'dd/MM/yyyy') as  EffectiveDate, format( A.DueOn, 'dd/MM/yyyy') as DueOn
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
                                MassUpdateDate, FORMAT(EmpSubmitionOn,'dd/MM/yyyy') as EmpSubmitionOn
                                ,(select distinct UseNam from " + DbMasterName + @".[dbo].Usr where UseID = EmpSubmitionBy ) as EmpSubmitionBy,
                                format(B.NewEffectiveDate, 'dd/MM/yyyy') as NewEffectiveDate,format(B.NewDueOn, 'dd/MM/yyyy') as NewDueOn,
                                A.IMRecycleRatio,A.SMNPicDept
                                from TQuoteDetails_D A 
                                left join TMngEffDateChgLog B on A.QuoteNo = B.QuoteNo 
                                where A.QuoteNo='" + QuoteNo + @"' and A.CreateStatus is not null 
                                order by B.CreatedOn desc";
                da = new SqlDataAdapter(str, EmetCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    Session["Plant_"] = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();

                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MaterialDesc = dtdate.Rows[0].ItemArray[11].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();

                    string PartunitTxt = dtdate.Rows[0].ItemArray[18].ToString();
                    TxtPlant.Text = GetPlantPICSMNSubmit(plant);
                    //TxtDepartment.Text = GetDeptPICSMNSubmit(dtdate.Rows[0].ItemArray[46].ToString());
                    TxtDepartment.Text = dtdate.Rows[0]["SMNPicDept"].ToString();
                    if (dtdate.Rows[0].ItemArray[44].ToString() != "" || dtdate.Rows[0].ItemArray[45].ToString() != "")
                    {
                        Label11.Text = dtdate.Rows[0].ItemArray[44].ToString();
                        Label15.Text = dtdate.Rows[0].ItemArray[45].ToString();

                        DateTime dtEfective1 = DateTime.Parse(dtdate.Rows[0].ItemArray[45].ToString());
                        Label15.Text = dtEfective1.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    else
                    {
                        Label11.Text = "";
                        Label15.Text = "";
                    }

                    DateTime dt = DateTime.Parse(dtdate.Rows[0].ItemArray[20].ToString());

                    txtquotationDueDate.Text = dt.ToShortDateString();

                    txtdrawng.Text = dtdate.Rows[0].ItemArray[19].ToString();

                    txtprod.Text = dtdate.Rows[0].ItemArray[8].ToString();
                    txtpartdesc.Text = Material + " - " + MaterialDesc;
                    txtSAPJobType.Text = dtdate.Rows[0].ItemArray[16].ToString();
                    txtPIRtype.Text = dtdate.Rows[0].ItemArray[12].ToString();
                    txtprocs.Text = dtdate.Rows[0].ItemArray[13].ToString();

                    txtPartUnit.Value = PartunitTxt.ToString();

                    if (dtdate.Rows[0]["EffectiveDate"].ToString() != "")
                    {
                        TextBox1.Text = dtdate.Rows[0]["EffectiveDate"].ToString();
                    }
                    if (dtdate.Rows[0]["DueOn"].ToString() != "")
                    {
                        txtfinal.Text = dtdate.Rows[0]["DueOn"].ToString();
                    }

                    //if (dtdate.Rows[0].ItemArray[21].ToString() != null && dtdate.Rows[0].ItemArray[21].ToString() != "")
                    //    TextBox1.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[21].ToString()).ToShortDateString();
                    //if (dtdate.Rows[0].ItemArray[22].ToString() != null && dtdate.Rows[0].ItemArray[22].ToString() != "")
                    //    txtfinal.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[22].ToString()).ToShortDateString();

                    if (dtdate.Rows[0]["FADate"].ToString() != "")
                    {
                        DateTime FADate = DateTime.Parse(dtdate.Rows[0]["FADate"].ToString());
                        TxtFADate.Text = FADate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TxtFADate.Text = "";
                    }
                    if (dtdate.Rows[0]["DelDate"].ToString() != "")
                    {
                        DateTime DelDate = DateTime.Parse(dtdate.Rows[0]["DelDate"].ToString());
                        TxtDelDate.Text = DelDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TxtDelDate.Text = "";
                    }

                    if (dtdate.Rows[0]["RequestDate"].ToString() != "")
                    {
                        DateTime RD = DateTime.Parse(dtdate.Rows[0]["RequestDate"].ToString());
                        TxtRequestDate.Text = RD.ToString("dd/MM/yyyy");
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
                        LbFlNameOri.Text = "No File";
                    }
                    else
                    {
                        LbFlName.Text = dtdate.Rows[0]["VndAttchmnt"].ToString().Replace(QN + "-", "");
                        LbFlNameOri.Text = dtdate.Rows[0]["VndAttchmnt"].ToString();
                    }


                    if (dtdate.Rows[0]["QuoteNoRef"].ToString() == "")
                    {
                        DvQuoteRef.Visible = false;
                        DvQuoreReqRevice.Visible = false;
                        hdnQuoteNoRef.Value = "";
                    }
                    else
                    {
                        DvQuoteRef.Visible = true;
                        LblQuNoRef.Text = ": " + dtdate.Rows[0]["QuoteNoRef"].ToString();

                        DvQuoreReqRevice.Visible = true;
                        LblQuNoRef.Text = ": " + dtdate.Rows[0]["QuoteNoRef"].ToString();
                        showDataQuoreReqRevice(dtdate.Rows[0]["QuoteNo"].ToString(), dtdate.Rows[0]["QuoteNoRef"].ToString());
                        hdnQuoteNoRef.Value = dtdate.Rows[0]["QuoteNoRef"].ToString();
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
                        HdnMAssTotMatCost.Value = dtdate.Rows[0]["TotalMaterialCost"].ToString();
                        HdnMAssTotProcCost.Value = dtdate.Rows[0]["TotalProcessCost"].ToString();
                        HdnMAssTotSubMatCost.Value = dtdate.Rows[0]["TotalSubMaterialCost"].ToString();
                        HdnMAssTotOthCost.Value = dtdate.Rows[0]["TotalOtheritemsCost"].ToString();
                        //GetBOMForMassRevision(dtdate.Rows[0]["Material"].ToString(), dtdate.Rows[0]["EffectiveDate"].ToString(), dtdate.Rows[0]["VendorCode1"].ToString(), dtdate.Rows[0]["Plant"].ToString());
                        GetOldQuotePIRMass(dtdate.Rows[0]["QuoteNo"].ToString());
                        DvGvPIROldQuoteMass.Visible = true;
                        if (hdnQuoteNoRefmassRev.Value != "")
                        {
                            DvOldMatCost.Visible = true;
                            DvOldOldProccost.Visible = true;
                            DvOldSubMat.Visible = true;
                            DvOldOthCost.Visible = true;
                            txtOldMatcost.Text = "Old Total Material Cost/pc : " + dtdate.Rows[0]["OldTotMatCost"].ToString();
                            txtOldProccost.Text = "Old Total Process Cost/pc : " + dtdate.Rows[0]["OldTotProCost"].ToString();
                            txtOldSubMat.Text = "Old Total SUB-MAT/T&J Cost/pc : " + dtdate.Rows[0]["OldTotSubMatCost"].ToString();
                            txtOldOthCost.Text = "Old Total Others Cost/pc : " + dtdate.Rows[0]["OldTotOthCost"].ToString();
                        }
                        else
                        {
                            DvOldMatCost.Visible = false;
                            DvOldOldProccost.Visible = false;
                            DvOldSubMat.Visible = false;
                            DvOldOthCost.Visible = false;
                        }
                        LbSmnSubmitBy.Text = dtdate.Rows[0]["EmpSubmitionBy"].ToString();
                        LbSmnSubmitOn.Text = ": " + dtdate.Rows[0]["EmpSubmitionOn"].ToString();
                        DVSmnSubmit.Visible = true;
                    }
                    else
                    {
                        DvGvPIROldQuoteMass.Visible = false;
                    }

                    GetBOMRawmaterialBefEffdate(dtdate.Rows[0]["QuoteNo"].ToString());

                    HdnSAPSpProcType.Value = dtdate.Rows[0]["SAPSpProcType"].ToString();
                    HdnAcsTabMatCost.Value = dtdate.Rows[0]["AcsTabMatCost"].ToString();
                    HdnAcsTabProCost.Value = dtdate.Rows[0]["AcsTabProcCost"].ToString();
                    HdnAcsTabSubMatCost.Value = dtdate.Rows[0]["AcsTabSubMatCost"].ToString();
                    HdnAcsTabOthCost.Value = dtdate.Rows[0]["AcsTabOthMatCost"].ToString();

                    if (dtdate.Rows[0]["IMRecycleRatio"].ToString() != "")
                    {
                        DvImRcylRatio.Visible = true;
                        TxtImRcylRatio.Text = dtdate.Rows[0]["IMRecycleRatio"].ToString();
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbTransName = "";
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
                    sql = @" select QuoteNo,TQ.CountryOrg,
                            (select CONCAT(TQ.VendorCode1 ,' - ',(select distinct TV.Description from " + DbMasterName + @".[dbo].tVendor_New TV where TV.Vendor = TQ.VendorCode1)) ) as 'vendor',
                            TQ.Product,TQ.MaterialClass,TQ.Material,TQ.MaterialDesc,
                            (select CONCAT(TQ.ProcessGroup ,' - ',(select distinct TP.Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code = TQ.ProcessGroup)) ) as 'ProcessGroup',
                            CAST(ROUND(TQ.TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TQ.TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TQ.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TQ.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(TQ.FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            (select TQ2.AcsTabMatCost from TQuoteDetails_D TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabMatCost',
                            (select TQ2.AcsTabProcCost from TQuoteDetails_D TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabProcCost',
                            (select TQ2.AcsTabSubMatCost from TQuoteDetails_D TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabSubMatCost',
                            (select TQ2.AcsTabOthMatCost from TQuoteDetails_D TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabOthMatCost'
                            from TQuoteDetails_D TQ 
                            where TQ.QuoteNo = @OldQN ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@newQN", newQN);
                    cmd.Parameters.AddWithValue("@OldQN", OldQN);
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
                            if (ChkMatRowRef != null && chkProcRowRef != null && chkSubMatRowRef != null && chkOthRowRef != null)
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

        private void GetQuoteupdated(string QuoteNo)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select useid,usenam from usr where useid='" + Label11.Text + "' ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {

                    Label14.Text = dtdate.Rows[0].ItemArray[0].ToString() + " - " + dtdate.Rows[0].ItemArray[1].ToString();

                }

                else
                {
                    Label14.Text = "Waiting for Vendor Submit";
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
                string str = "select distinct FieldGroup from tMETFileds ";
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


        #region old 25-06-2019 CreateDynamicDT(int ColumnType)
        //private void CreateDynamicDT(int ColumnType)
        //{
        //    DataTable DtDynamic = new DataTable();
        //    DtDynamic = (DataTable)Session["DtDynamic"];

        //    DataTable DtMaterialsDetails = new DataTable();
        //    DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code
        //        //int rowcount = 0;
        //        //if (DtMaterialsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    Table1.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }
        //        //    foreach (DataRow row in DtDynamic.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        Table1.Rows.Add(tRow);

        //        //        for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString();
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tCell.Controls.Add(tb);
        //        //                if (tb.ID.Contains("txtMaterialSAPCode"))
        //        //                {
        //        //                    tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtMaterialDescription"))
        //        //                {
        //        //                    tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //        //                    //tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                {
        //        //                    double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //        //                    double rawperkg = RawVal / 1000;
        //        //                    tb.Text = (rawperkg.ToString());
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //        //                {
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Text = txtPartUnit.Value;
        //        //                }

        //        //                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {
        //        //                    tb.MaxLength = 4;
        //        //                }

        //        //                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
        //        //                {

        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tCell.Controls.Add(tb);
        //        //                    Table1.Rows[cellCtr].Cells.Add(tCell);

        //        //                }

        //        //                if (txtPIRtype.Text.Contains("SUBCON"))
        //        //                {


        //        //                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                    {
        //        //                        tb.Text = "";
        //        //                    }
        //        //                    if (tb.ID.Contains("Cavity"))
        //        //                    {
        //        //                        //tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                }

        //        //                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //        //                {
        //        //                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == rowcount)
        //        //                        {
        //        //                            tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tRow.Cells.Add(tCell);

        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    Table1.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    //Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        Table1.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                Table1.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");

        //        //                tCell.Controls.Add(tb);


        //        //                if (tb.ID.Contains("txtMaterialSAPCode"))
        //        //                {
        //        //                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtMaterialDescription"))
        //        //                {
        //        //                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //        //                    //tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                {
        //        //                    //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //        //                    //double rawperkg = RawVal / 1000;
        //        //                    //tb.Text = (rawperkg.ToString());
        //        //                    //// tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //        //                {
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Text = txtPartUnit.Value;
        //        //                }

        //        //                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {
        //        //                    tb.MaxLength = 4;
        //        //                }

        //        //                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {

        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tCell.Controls.Add(tb);
        //        //                    Table1.Rows[cellCtr].Cells.Add(tCell);

        //        //                }

        //        //                if (txtPIRtype.Text.Contains("SUBCON"))
        //        //                {


        //        //                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                    {
        //        //                        tb.Text = "";
        //        //                    }
        //        //                    if (tb.ID.Contains("Cavity"))
        //        //                    {
        //        //                        //tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                }
        //        //                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //        //                {
        //        //                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == rowcountnew)
        //        //                        {
        //        //                            tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tRow.Cells.Add(tCell);
        //        //            }

        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion old code

        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        Table1.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        //Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            Table1.Rows.Add(tRow);

        //            for (int i = 0; i <= 1; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
        //                    {
        //                        lb.Text = "Base Qty / Cavity";
        //                    }
        //                    else
        //                    {
        //                        lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
        //                    }

        //                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
        //                    {
        //                        lb.Text = "Material/Melting Loss (%)";
        //                    }
        //                    lb.Width = 240;
        //                    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    tCell.Controls.Add(tb);


        //                    if (tb.ID.Contains("txtMaterialSAPCode"))
        //                    {
        //                        // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //                        // tb.Enabled = false;
        //                        tb.Attributes.Add("disabled", "disabled");
        //                    }
        //                    else if (tb.ID.Contains("txtMaterialDescription"))
        //                    {
        //                        // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //                        //tb.Enabled = false;
        //                        tb.Attributes.Add("disabled", "disabled");
        //                    }
        //                    else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //                    {
        //                        //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //                        //double rawperkg = RawVal / 1000;
        //                        //tb.Text = (rawperkg.ToString());
        //                        //// tb.Enabled = false;
        //                        tb.Attributes.Add("disabled", "disabled");
        //                    }
        //                    else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //                    {
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tb.Text = txtPartUnit.Value;
        //                    }

        //                    if (tb.ID.Contains("RunnerRatio/pcs(%)"))
        //                    {
        //                        tb.MaxLength = 4;
        //                    }

        //                    if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)"))
        //                    {

        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tCell.Controls.Add(tb);
        //                        Table1.Rows[cellCtr].Cells.Add(tCell);

        //                    }

        //                    if (txtPIRtype.Text.Contains("SUBCON"))
        //                    {


        //                        if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //                        {
        //                            tb.Text = "";
        //                        }
        //                        if (tb.ID.Contains("Cavity"))
        //                        {
        //                            //tb.Attributes.Add("disabled", "disabled");
        //                        }
        //                        else
        //                        {
        //                            tb.Attributes.Add("disabled", "disabled");
        //                        }
        //                    }
        //                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //                    {
        //                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempMClist.Count; ii++)
        //                        {
        //                            if (ii == rowcountnew)
        //                            {
        //                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tRow.Cells.Add(tCell);
        //                }

        //            }
        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }
        //        Session["Table"] = Table1;
        //    }
        //    else
        //    {
        //        Table1 = (Table)Session["Table"];

        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //            var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //            var TempMClistNew = tempMClist;

        //            //if (CellsCount == 2 && tempMClist.Count <= (DtDynamic.Rows.Count + 1))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i)) * (DtDynamic.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempSMClist.Count > 6)
        //            ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount >= 3 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamic.Rows.Count)).ToList();
        //            //else if (CellsCount >= 4 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount) * (DtDynamic.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(i * (DtDynamic.Rows.Count)).ToList();

        //            //if (i == 1)
        //            //    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();
        //            //else

        //            TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

        //            for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    if (tb.ID.Contains("RunnerWeight/shot(g)") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("PartNetUnitWeight(g)") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RecycleMaterialRatio(%)") || tb.ID.Contains("Cavity") || tb.ID.Contains("MaterialYield/MeltingLoss(%)"))
        //                    {
        //                        if (txtprocs.Text.ToUpper() != "ST")
        //                        {
        //                            Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                            //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            //if (tb.ID.Contains("PartNetUnitWeight(g)"))
        //                            //{
        //                            //    tb.Text = txtPartUnit.Value;
        //                            //    tCell.Controls.Add(tb);
        //                            //    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            //    //tb.Attributes.Add("disabled", "disabled");
        //                            //}
        //                            //else
        //                            //{
        //                            //    tCell.Controls.Add(tb);
        //                            //    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            //}
        //                        }
        //                        else
        //                        {
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            if (tb.ID.Contains("PartNetUnitWeight(g)"))
        //                            {
        //                                tb.Text = txtPartUnit.Value;
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                                //tb.Attributes.Add("disabled", "disabled");
        //                            }
        //                            else if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
        //                            {
        //                                Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
        //                            }
        //                            else if (tb.ID.Contains("txtMaterialGrossWeight/pc(g)"))
        //                            {
        //                                tb.Attributes.Add("disabled", "disabled");
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            }
        //                            else
        //                            {
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            }
        //                        }
        //                        //Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                        //if (tb.ID.Contains("PartNetUnitWeight(g)"))
        //                        //{
        //                        //    tb.Attributes.Add("disabled", "disabled");
        //                        //}

        //                    }
        //                    else if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
        //                    {

        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tCell.Controls.Add(tb);
        //                        Table1.Rows[cellCtr].Cells.Add(tCell);

        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        Table1.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < TempMClistNew.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {

        //                                tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");


        //                }
        //            }
        //        }


        //        Session["Table"] = Table1;
        //        Table1.DataBind();
        //    }
        //    Session["Table"] = Table1;


        //}
        #endregion old 25-06-2019 CreateDynamicDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicDT(int ColumnType)
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
                    int rowcount = 0;

                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                    {
                        #region condition with data filled by vendor
                        int rowcountnew = 0;
                        TableRow Hearderrow = new TableRow();

                        Table1.Rows.Add(Hearderrow);
                        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                        {
                            TableCell tCell1 = new TableCell();
                            if (cellCtr == 0)
                            {
                                tCell1.Text = "Field Name";
                            }
                            Hearderrow.Cells.Add(tCell1);
                            tCell1.Width = 150;
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
                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                    {
                                        tCell.Text = "Raw Material SAP Code";
                                    }
                                    else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                    {
                                        tCell.Text = "Raw Material Description";
                                    }
                                    else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                    {
                                        tCell.Text = "Base Qty / Cavity";
                                    }
                                    else
                                    {
                                        tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                    {
                                        tCell.Text = "Material/Melting Loss (%)";
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                    {
                                        tCell.Text = "Raw Material Cost";
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                    {
                                        tCell.Text = "Total Raw Material Cost";
                                    }

                                    if (hdnLayoutScreen.Value == "Layout7")
                                    {
                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                        {
                                            //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                        {
                                            tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                        }
                                    }

                                    tCell.Width = 150;
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                    if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                    {
                                        tCell.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tCell.Style.Add("text-align", "right");
                                    }

                                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                    {
                                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempMClist.Count; ii++)
                                        {
                                            if (ii == rowcountnew)
                                            {
                                                tCell.Text = tempMClist[ii].ToString().Replace("NaN", "").ToUpper();
                                                break;
                                            }

                                        }
                                    }
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    tCell.Width = 150;
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
                        #endregion with data filled by vendor
                    }
                    else
                    {
                        #region Waiting vendor response
                        if (DtMaterialsDetails.Rows.Count > 0)
                        {
                            #region Waiting vendor response if request with BOM
                            TableRow Hearderrow = new TableRow();
                            Table1.Rows.Add(Hearderrow);
                            for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                            {
                                TableCell tCell1 = new TableCell();
                                if (cellCtr == 0)
                                {
                                    tCell1.Text = "Field Name";
                                }

                                Hearderrow.Cells.Add(tCell1);
                                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                                Hearderrow.ForeColor = Color.White;
                            }
                            foreach (DataRow row in DtDynamic.Rows)
                            {
                                TableRow tRow = new TableRow();
                                Table1.Rows.Add(tRow);

                                for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                                {
                                    int count = 1;
                                    TableCell tCell = new TableCell();
                                    if (cellCtr == 0)
                                    {
                                        if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                        {
                                            tCell.Text = "Raw Material SAP Code";
                                        }
                                        else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                        {
                                            tCell.Text = "Raw Material Description";
                                        }
                                        else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                        {
                                            tCell.Text = "Base Qty / Cavity";
                                        }
                                        else
                                        {
                                            tCell.Text = row.ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                        {
                                            tCell.Text = "Material/Melting Loss (%)";
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                        {
                                            tCell.Text = "Raw Material Cost";
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                        {
                                            tCell.Text = "Total Raw Material Cost";
                                        }

                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                            {
                                                //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                            }

                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                            {
                                                tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                            }
                                        }
                                        tCell.Width = 150;
                                        tRow.Cells.Add(tCell);
                                    }
                                    else
                                    {
                                        string FieldName = row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                        if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Style.Add("text-align", "left");
                                        }
                                        else
                                        {
                                            tCell.Style.Add("text-align", "right");
                                        }

                                        if (FieldName.Contains("MaterialSAPCode"))
                                        {
                                            tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                        }
                                        else if (FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                        }
                                        else if (FieldName.Contains("RawMaterialCost/kg"))
                                        {
                                            double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                            double rawperkg = RawVal / 1000;
                                            tCell.Text = (String.Format("{0:0.000}", rawperkg));
                                        }
                                        else if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                        }
                                        else if (FieldName.Contains("TotalMaterialCost/pcs"))
                                        {
                                            if (hdnMassRevision.Value != "")
                                            {
                                                tCell.Text = HdnMAssTotMatCost.Value;
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }
                                        tCell.Width = 150;
                                        tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                            #endregion Waiting vendor response if request with BOM
                        }
                        else
                        {
                            #region Waiting vendor response No BOM
                            int rowcountnew = 0;

                            TableRow Hearderrow = new TableRow();

                            Table1.Rows.Add(Hearderrow);
                            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                            {
                                TableCell tCell1 = new TableCell();
                                if (cellCtr == 0)
                                {
                                    tCell1.Text = "Field Name";
                                }
                                Hearderrow.Cells.Add(tCell1);
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
                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                        {
                                            tCell.Text = "Raw Material SAP Code";
                                        }
                                        else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                        {
                                            tCell.Text = "Raw Material Description";
                                        }
                                        else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                        {
                                            tCell.Text = "Base Qty / Cavity";
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                        {
                                            tCell.Text = "Material/Melting Loss (%)";
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                        {
                                            tCell.Text = "Raw Material Cost";
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                        {
                                            tCell.Text = "Total Raw Material Cost";
                                        }

                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                            {
                                                //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                            }

                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                            {
                                                tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                            }
                                        }
                                        tCell.Width = 150;
                                        Table1.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                    else
                                    {
                                        string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                        if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Style.Add("text-align", "left");
                                        }
                                        else
                                        {
                                            tCell.Style.Add("text-align", "right");
                                        }

                                        if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                        }
                                        tCell.Width = 150;
                                        tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                            #endregion Waiting vendor response No BOM
                        }
                        #endregion Waiting vendor response
                    }
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
                                tCell.Width = 150;
                                Table1.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                if (FieldName.Contains("RunnerWeight/shot(g)") || FieldName.Contains("RunnerRatio/pcs(%)") ||
                                    FieldName.Contains("MaterialGrossWeight/pc(g)") || FieldName.Contains("PartNetUnitWeight(g)") ||
                                    FieldName.Contains("TotalMaterialCost/pcs") || FieldName.Contains("RecycleMaterialRatio(%)") ||
                                    FieldName.Contains("Cavity") || FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                {
                                    if (txtprocs.Text.ToUpper() != "ST")
                                    {
                                        Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                        ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                    }
                                    else
                                    {
                                        if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                            //tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (FieldName.Contains("TotalMaterialCost/pcs"))
                                        {
                                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                            Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                        }
                                        else
                                        {
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                    }

                                }
                                else
                                {
                                    tCell.Width = 150;
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }

                                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                {
                                    for (int ii = 0; ii < TempMClistNew.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = TempMClistNew[ii].ToString().Replace("NaN", "").ToUpper();
                                            break;
                                        }
                                    }
                                }

                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
        #endregion New 25-06-2019 CreateDynamicDT(int ColumnType)

        #region old 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        //private void CreateDynamicProcessDT(int ColumnType)
        //{
        //    DataTable DtDynamicProcessFields = new DataTable();
        //    DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

        //    DataTable DtDynamicProcessCostsDetails = new DataTable();
        //    DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TablePC.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }
        //        foreach (DataRow row in DtDynamicProcessFields.Rows)
        //        {
        //            TableRow tRow = new TableRow();
        //            TablePC.Rows.Add(tRow);
        //            for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("IFTURNKEY-VENDORNAME"))
        //                    {
        //                        tCell.Text = "If Subcon - Subcon Name";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("VENDORRATE"))
        //                    {
        //                        tCell.Text = "Vendor Rate/HR";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TURNKEYPROFIT"))
        //                    {
        //                        tCell.Text = "Turnkey Fees";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //                    {
        //                        tCell.Text = "Total Process Cost/pc";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("EFFICIENCY/PROCESSYIELD(%)"))
        //                    {
        //                        tCell.Text = "Efficiency";
        //                    }
        //                    else
        //                    {
        //                        tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
        //                    }
        //                    //tCell.Text = row.ItemArray[0].ToString();
        //                    tCell.Width = 280;
        //                    tRow.Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    #region create form control old

        //                    //if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlProcess" + (cellCtr - 1);
        //                    //    ddl.DataSource = Session["process"];
        //                    //    ddl.DataTextField = "Process_Grp_code";
        //                    //    //ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged2;
        //                    //    ddl.DataValueField = "Process_Grp_code";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();

        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                    string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                    MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                    string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                    MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                }
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data
        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubProcess" + (cellCtr - 1);


        //                    //    ddl.DataSource = Session["PSGroupwithUOM"];
        //                    //    ddl.DataTextField = "SubProcessName";

        //                    //    ddl.DataValueField = "SubProcessName";
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //                        }
        //                    //                    }
        //                    //                    if (ddlcheck1 != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //                        }
        //                    //                    }
        //                    //                    if (ddlcheck1 != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                        //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                }

        //                    //                break;

        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    #region old code 2019-03-20
        //                    //    //if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    //{
        //                    //    //    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //    //    for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //    //    {
        //                    //    //        if (ii == rowcount)
        //                    //    //        {
        //                    //    //            var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //    //            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //    //            {
        //                    //    //                for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //    //                {
        //                    //    //                    if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //    //                    {
        //                    //    //                    }
        //                    //    //                    else
        //                    //    //                    {
        //                    //    //                        ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //    //                    }
        //                    //    //                }
        //                    //    //                if (ddlcheck1 != null)
        //                    //    //                {
        //                    //    //                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //    //                } 
        //                    //    //            }
        //                    //    //            else
        //                    //    //            {
        //                    //    //                for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //    //                {
        //                    //    //                    if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //    //                    {
        //                    //    //                    }
        //                    //    //                    else
        //                    //    //                    {
        //                    //    //                        ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //    //                    }
        //                    //    //                }
        //                    //    //                if (ddlcheck1 != null)
        //                    //    //                {
        //                    //    //                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //    //                    ddl.Attributes.Add("disabled", "disabled");
        //                    //    //                }
        //                    //    //                else
        //                    //    //                {
        //                    //    //                    ddl.Attributes.Add("disabled", "disabled");
        //                    //    //                    //ddl.SelectedValue = "";
        //                    //    //                }
        //                    //    //            }

        //                    //    //            break;

        //                    //    //        }

        //                    //    //    }
        //                    //    //}
        //                    //    #endregion

        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubvendorname" + (cellCtr - 1);
        //                    //    ddl.DataSource = Session["SubVndorData"];
        //                    //    ddl.DataTextField = "SubVndorData";
        //                    //    ddl.DataValueField = "SubVndorData";
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");

        //                    //                    var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    ddl.SelectedIndex = 0;
        //                    //                }
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachineLabor" + (cellCtr - 1);
        //                    //    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
        //                    //    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.Attributes.Add("disabled", "disabled");


        //                    //    DropDownList ddlhide = new DropDownList();
        //                    //    ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
        //                    //    ddlhide.Style.Add("display", "none");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");
        //                    //    //ddlhide.Style.Add("width", "142px");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
        //                    //    {

        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
        //                    //                    {
        //                    //                        string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        var ddlcheck = ddl.Items.FindByText(txtddlTemp);
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(txtddlTemp).Selected = true;
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlhide.Style.Add("display", "block");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    ddl.Style.Add("display", "none");
        //                    //                    ddlhide.Style.Add("display", "block");
        //                    //                    break;
        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    #region old code 2019-03-20
        //                    //    if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();
        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                var ddlcheck = ddl.Items.FindByText(txtddlTemp);
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(txtddlTemp).Selected = true;
        //                    //                    }
        //                    //                    ddl.Style.Add("display", "block");
        //                    //                    ddlhide.Style.Add("display", "none");
        //                    //                    break;
        //                    //                }
        //                    //                else if (tempPClist[2].ToString() != "" || tempPClist[2].ToString() != null)
        //                    //                {
        //                    //                    ddl.Style.Add("display", "none");
        //                    //                    ddlhide.Style.Add("display", "block");
        //                    //                    break;
        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(ddlhide);

        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachine" + (cellCtr - 1);
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.DataSource = Session["MachineIDs"];
        //                    //    ddl.DataTextField = "MachineID";

        //                    //    ddl.DataValueField = "MachineID";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();
        //                    //    // ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.ID = "txtMachineId" + (cellCtr - 1);
        //                    //    tb.Attributes.Add("autocomplete", "off");
        //                    //    tb.Style.Add("display", "none");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    // style = "display: none;"
        //                    //    //tb.Visible = false;

        //                    //    DropDownList ddlMachide = new DropDownList();
        //                    //    ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
        //                    //    //ddlMachide.Style.Add("width", "142px");
        //                    //    ddlMachide.Style.Add("display", "none");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");


        //                    //    TextBox tbhide = new TextBox();
        //                    //    tbhide.ID = "txtHide" + (cellCtr - 1);
        //                    //    tbhide.Style.Add("display", "none");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
        //                    //                    {
        //                    //                        if (tempPClist[4].ToString() == "Machine")
        //                    //                        {
        //                    //                            var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                            if (ddlcheck != null)
        //                    //                            {
        //                    //                                ddl.Style.Add("display", "block");

        //                    //                                if (Session["MachineIDsbyProcess"] != null)
        //                    //                                {
        //                    //                                    ddl.DataSource = null;
        //                    //                                    ddl.DataSource = Session["MachineIDsbyProcess"];
        //                    //                                    ddl.DataTextField = "MachineID";
        //                    //                                    ddl.DataValueField = "MachineID";
        //                    //                                    ddl.DataBind();
        //                    //                                    ListItem match = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                                    if (match == null)
        //                    //                                    {
        //                    //                                        ddl.SelectedIndex = 0;
        //                    //                                    }
        //                    //                                    else
        //                    //                                    {
        //                    //                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                                    }
        //                    //                                }
        //                    //                                Session["MachineIDsbyProcess"] = null;
        //                    //                            }
        //                    //                        }
        //                    //                        else if (tempPClist[4].ToString() == "Labor")
        //                    //                        {
        //                    //                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                            ddl.Style.Add("display", "none");
        //                    //                            tb.Style.Add("display", "block");
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        tbhide.Style.Add("display", "none");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (tempPClist[4].ToString() == "Machine")
        //                    //                    {
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    else if (tempPClist[4].ToString() == "Labor")
        //                    //                    {
        //                    //                        tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        tb.Style.Add("display", "block");
        //                    //                        tb.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    #region old code 2019-03-20
        //                    //    //if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    //{
        //                    //    //    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //    //    for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //    //    {
        //                    //    //        if (ii == rowcount)
        //                    //    //        {
        //                    //    //            var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //    //            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //    //            {
        //                    //    //                if (tempPClist[3].ToString() == "Machine")
        //                    //    //                {
        //                    //    //                    if (ddlcheck != null)
        //                    //    //                    {
        //                    //    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //    //                    }
        //                    //    //                    ddl.Style.Add("display", "block");
        //                    //    //                    tb.Style.Add("display", "none");
        //                    //    //                }
        //                    //    //                else if (tempPClist[3].ToString() == "Labor")
        //                    //    //                {
        //                    //    //                    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //    //                    ddl.Style.Add("display", "none");
        //                    //    //                    tb.Style.Add("display", "block");
        //                    //    //                }


        //                    //    //                break;
        //                    //    //            }
        //                    //    //            else
        //                    //    //            {
        //                    //    //                ddl.Style.Add("display", "none");
        //                    //    //                ddlMachide.Style.Add("display", "block");
        //                    //    //                tb.Style.Add("display", "none");
        //                    //    //                //if (tempPClist[3].ToString() == "Machine")
        //                    //    //                //{
        //                    //    //                //    //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //    //                //    ddlMachide.Style.Add("display", "block");
        //                    //    //                //    ddl.Style.Add("display", "none");
        //                    //    //                //    ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //    //                //}
        //                    //    //                //else if (tempPClist[3].ToString() == "Labor")
        //                    //    //                //{
        //                    //    //                //    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //    //                //    ddl.Style.Add("display", "none");
        //                    //    //                //    tb.Style.Add("display", "block");
        //                    //    //                //    tb.Attributes.Add("disabled", "disabled");
        //                    //    //                //}

        //                    //    //                break;

        //                    //    //            }
        //                    //    //        }

        //                    //    //    }
        //                    //    //}
        //                    //    #endregion

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(tb);

        //                    //    tCell.Controls.Add(ddlMachide);
        //                    //    tCell.Controls.Add(tbhide);


        //                    //}

        //                    //else
        //                    //{
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.BorderStyle = BorderStyle.None;
        //                    //    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);


        //                    //    tb.Attributes.Add("autocomplete", "off");

        //                    //    tb.Attributes.Add("disabled", "disabled");

        //                    //    if (tb.ID.Contains("txtStandardRate/HR"))
        //                    //    {
        //                    //        grdMachinelisthidden.DataSource = Session["MachineListGrd"];
        //                    //        grdMachinelisthidden.DataBind();
        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
        //                    //        {

        //                    //            if (firsttimeload == 0)
        //                    //            {
        //                    //                tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
        //                    //                firsttimeload = 1;
        //                    //            }
        //                    //            tb.Attributes.Add("disabled", "disabled");
        //                    //            if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
        //                    //            {

        //                    //            }

        //                    //        }

        //                    //    }

        //                    //    if (tb.ID.Contains("txtVendorRate"))
        //                    //    {

        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
        //                    //        {

        //                    //            if (firsttimeload == 0)
        //                    //            {
        //                    //                tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
        //                    //                firsttimeload = 1;
        //                    //            }

        //                    //            if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
        //                    //            {
        //                    //                tb.Attributes.Add("disabled", "disabled");
        //                    //            }


        //                    //        }

        //                    //    }

        //                    //    if (tb.ID.Contains("txtProcessUOM"))
        //                    //    {

        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //        {
        //                    //            if (grdProcessGrphidden.Rows[h].Cells[0].Text.Contains(txtprocs.Text + " -"))
        //                    //            {
        //                    //                if (firsttimeload == 0)
        //                    //                {
        //                    //                    tb.Text = grdProcessGrphidden.Rows[h].Cells[2].Text;
        //                    //                    firsttimeload = 1;
        //                    //                }

        //                    //            }
        //                    //        }
        //                    //    }

        //                    //    #region New code
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion

        //                    //    #region old code 2019-03-20
        //                    //    //if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    //{
        //                    //    //    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //    //    for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //    //    {
        //                    //    //        if (ii == rowcount)
        //                    //    //        {
        //                    //    //            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //    //            {
        //                    //    //                if ((tb.ID.Contains("ProcessCost/pc")))

        //                    //    //                { tb.Attributes.Add("disabled", "disabled"); }

        //                    //    //            }

        //                    //    //            else
        //                    //    //            {
        //                    //    //            }
        //                    //    //            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //    //            break;
        //                    //    //        }

        //                    //    //    }
        //                    //    //}
        //                    //    #endregion old code

        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    tCell.Controls.Add(tb);
        //                    //}
        //                    #endregion

        //                    #region create form control old

        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);

        //                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    {
        //                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                        {
        //                            if (ii == rowcount)
        //                            {
        //                                string ifsubcon = tempPClist[2].ToString().Replace("NaN", "");
        //                                string ifturnkey = tempPClist[3].ToString().Replace("NaN", "");

        //                                if (tempPClist[ii].ToString().Replace("NaN", "").Contains("--Select--"))
        //                                {
        //                                    tb.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                                }

        //                                if (ifsubcon != "" || ifturnkey != "")
        //                                {
        //                                    if (ii == 4)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                    else if (ii == 5)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tCell.Controls.Add(tb);
        //                    #endregion

        //                    tRow.Cells.Add(tCell);
        //                }
        //            }

        //            if (rowcount % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcount++;
        //        }
        //        // TableMat = Table1;
        //        Session["TablePC"] = TablePC;
        //    }
        //    else
        //    {

        //        TablePC = (Table)Session["TablePC"];

        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {
        //            var tempPclist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //            var TempPclistNew = tempPclist;

        //            //if (CellsCount == 2 && tempPclist.Count <= (DtDynamicProcessFields.Rows.Count + 1))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i)) * (DtDynamicProcessFields.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempSMClist.Count > 6)
        //            ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount == 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i + 1)) * DtDynamicProcessFields.Rows.Count)).ToList();
        //            //else if (CellsCount >= 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount) * (DtDynamicProcessFields.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();

        //            //if(i == 1)
        //            //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count + 1)).ToList();
        //            //else

        //            TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();


        //            int rowcount = 0;
        //            for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    #region old code
        //                    //if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlProcess" + (i);
        //                    //    ddl.DataSource = Session["process"];
        //                    //    ddl.DataTextField = "Process_Grp_code";
        //                    //    ddl.DataValueField = "Process_Grp_code";

        //                    //    ddl.DataBind();
        //                    //    // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged3;
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            int CekDataNcolumn = tempPclist.Count / CellsCount;
        //                    //            if (TempPclistNew.Count > CellsCount)
        //                    //            {
        //                    //                string zz = TempPclistNew[ii].ToString();
        //                    //                if (ii == rowcount - 1)
        //                    //                {
        //                    //                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                    if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                    {
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                        }
        //                    //                        var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                        string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                        MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                        }
        //                    //                        var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                        string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                        MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                        //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}
        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubProcess" + (i);


        //                    //    ddl.DataSource = Session["PSGroupwithUOM"];
        //                    //    ddl.DataTextField = "SubProcessName";

        //                    //    ddl.DataValueField = "SubProcessName";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();
        //                    //    // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged4;
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));

        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(TempPclistNew[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

        //                    //                        }
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(TempPclistNew[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

        //                    //                        }
        //                    //                    }
        //                    //                    //ddl.Attributes.Add("disabled", "disabled");
        //                    //                }
        //                    //                //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubvendorname" + (i);
        //                    //    ddl.DataSource = Session["SubVndorData"];
        //                    //    ddl.DataTextField = "SubVndorData";
        //                    //    ddl.DataValueField = "SubVndorData";
        //                    //    //ddl.Attributes.Add("onchange", "alert('aaaa')");
        //                    //    ddl.Attributes.Add("onchange", "SubVendorData(" + i + ")");
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            string zz = TempPclistNew[ii].ToString();
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    // ddl.SelectedValue = grdProcessGrphidden.Rows[h].Cells[0].Text;
        //                    //                    string abc = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    ddl.SelectedIndex = 0;
        //                    //                    ddl.Attributes.Add("disabled", "disabled");
        //                    //                }
        //                    //                //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachineLabor" + (i);

        //                    //    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
        //                    //    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
        //                    //    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));

        //                    //    DropDownList ddlhide = new DropDownList();
        //                    //    ddlhide.ID = "dynamicddlHideMachineLabor" + (i);
        //                    //    ddlhide.Style.Add("display", "none");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");
        //                    //    //ddlhide.Style.Add("width", "142px");
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
        //                    //                    {
        //                    //                        var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                        if (ddlcheck != null)
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlhide.Style.Add("display", "block");
        //                    //                    }
        //                    //                }
        //                    //                else if (TempPclistNew[2].ToString() != "" || TempPclistNew[2].ToString() != null)
        //                    //                {
        //                    //                    ddl.Style.Add("display", "none");
        //                    //                    ddlhide.Style.Add("display", "block");
        //                    //                }
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(ddlhide);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachine" + (i);
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.DataSource = Session["MachineIDs"];
        //                    //    ddl.DataTextField = "MachineID";

        //                    //    ddl.DataValueField = "MachineID";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();

        //                    //    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));


        //                    //    TextBox tb = new TextBox();
        //                    //    tb.ID = "txtMachineId" + (i);
        //                    //    tb.Style.Add("display", "none");
        //                    //    tb.Attributes.Add("autocomplete", "off");

        //                    //    DropDownList ddlMachide = new DropDownList();
        //                    //    ddlMachide.ID = "ddlHideMachine" + (i);
        //                    //    //ddlMachide.Style.Add("width", "142px");
        //                    //    ddlMachide.Style.Add("display", "none");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");


        //                    //    TextBox tbhide = new TextBox();
        //                    //    tbhide.ID = "txtHide" + (i);
        //                    //    tbhide.Style.Add("display", "none");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {

        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
        //                    //                    {
        //                    //                        if (TempPclistNew[4].ToString() == "Machine")
        //                    //                        {
        //                    //                            ddl.Style.Add("display", "block");
        //                    //                            var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                            if (ddlcheck != null)
        //                    //                            {
        //                    //                                if (Session["MachineIDsbyProcess"] != null)
        //                    //                                {
        //                    //                                    DataTable dtMachineIDsbyProcess = (DataTable)Session["MachineIDsbyProcess"];
        //                    //                                    if (dtMachineIDsbyProcess.Rows.Count > 0)
        //                    //                                    {
        //                    //                                        ddl.DataSource = null;
        //                    //                                        ddl.DataSource = Session["MachineIDsbyProcess"];
        //                    //                                        ddl.DataTextField = "MachineID";
        //                    //                                        ddl.DataValueField = "MachineID";
        //                    //                                        ddl.DataBind();
        //                    //                                    }
        //                    //                                    dtMachineIDsbyProcess = null;
        //                    //                                }
        //                    //                                ListItem match = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                                if (match == null)
        //                    //                                {
        //                    //                                    ddl.SelectedIndex = 0;
        //                    //                                }
        //                    //                                else
        //                    //                                {
        //                    //                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                                }
        //                    //                                Session["MachineIDsbyProcess"] = null;
        //                    //                            }
        //                    //                        }
        //                    //                        else if (TempPclistNew[4].ToString() == "Labor")
        //                    //                        {
        //                    //                            tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                            ddl.Style.Add("display", "none");
        //                    //                            tb.Style.Add("display", "block");
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                    }

        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (TempPclistNew[4].ToString() == "Machine")
        //                    //                    {
        //                    //                        //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    else if (TempPclistNew[4].ToString() == "Labor")
        //                    //                    {
        //                    //                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        tb.Style.Add("display", "block");
        //                    //                        tb.Attributes.Add("disabled", "disabled");
        //                    //                    }

        //                    //                    break;

        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(tb);
        //                    //    tCell.Controls.Add(ddlMachide);
        //                    //    tCell.Controls.Add(tbhide);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);

        //                    //}
        //                    //else
        //                    //{
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.BorderStyle = BorderStyle.None;
        //                    //    tb.ID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    //    tb.Attributes.Add("autocomplete", "off");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
        //                    //    {
        //                    //        TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                    //        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                    //        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tCell.Controls.Add(tb);
        //                    //        TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //    }

        //                    //    if (tb.ID.Contains("txtStandardRate/HR"))
        //                    //    {

        //                    //        grdMachinelisthidden.DataSource = Session["MachineListGrd"];
        //                    //        grdMachinelisthidden.DataBind();
        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
        //                    //        {

        //                    //            if (firsttimeload == 0)
        //                    //            {
        //                    //                tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
        //                    //                firsttimeload = 1;
        //                    //            }
        //                    //            tb.Attributes.Add("disabled", "disabled");
        //                    //            if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
        //                    //            {

        //                    //            }


        //                    //        }
        //                    //    }

        //                    //    if (tb.ID.Contains("txtVendorRate"))
        //                    //    {

        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
        //                    //        {

        //                    //            if (firsttimeload == 0)
        //                    //            {
        //                    //                tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
        //                    //                firsttimeload = 1;
        //                    //            }

        //                    //            if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
        //                    //            {
        //                    //                tb.Attributes.Add("disabled", "disabled");
        //                    //            }


        //                    //        }
        //                    //    }


        //                    //    if (tb.ID.Contains("txtProcessUOM"))
        //                    //    {

        //                    //        int firsttimeload = 0;
        //                    //        for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //        {
        //                    //            if (grdProcessGrphidden.Rows[h].Cells[0].Text.Contains(txtprocs.Text + " -"))
        //                    //            {
        //                    //                if (firsttimeload == 0)
        //                    //                {
        //                    //                    tb.Text = grdProcessGrphidden.Rows[h].Cells[2].Text;
        //                    //                    firsttimeload = 1;
        //                    //                }

        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #region retriev data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == (rowcount - 1))
        //                    //            {
        //                    //                tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }

        //                    //    }
        //                    //    #endregion
        //                    //}

        //                    #endregion old code

        //                    #region New code
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
        //                    {
        //                        TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                        {
        //                            if (ii == (rowcount - 1))
        //                            {
        //                                string ifsubcon = TempPclistNew[2].ToString().Replace("NaN", "");
        //                                string ifturnkey = TempPclistNew[3].ToString().Replace("NaN", "");

        //                                if (TempPclistNew[ii].ToString().Replace("NaN", "").Contains("--Select--"))
        //                                {
        //                                    tb.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                                }

        //                                if (ifsubcon != "" || ifturnkey != "")
        //                                {
        //                                    if (ii == 4)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                    else if (ii == 5)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                }

        //                                break;
        //                            }
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                rowcount++;
        //            }
        //        }

        //        Session["TablePc"] = TablePC;
        //    }


        //}
        #endregion old 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        private void CreateDynamicProcessDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                DataTable DtDynamicProcessCostsDetails = new DataTable();
                DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

                if (ColumnType == 0)
                {
                    int rowcount = 0;

                    TableRow Hearderrow = new TableRow();

                    TablePC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        Hearderrow.Cells.Add(tCell1);
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
                                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("IFTURNKEY-VENDORNAME"))
                                {
                                    tCell.Text = "If Subcon - Subcon Name";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("VENDORRATE"))
                                {
                                    tCell.Text = "Vendor Rate/HR";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TURNKEYPROFIT"))
                                {
                                    tCell.Text = "Turnkey Fees";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                {
                                    tCell.Text = "Total Process Cost/pc";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("EFFICIENCY/PROCESSYIELD(%)"))
                                {
                                    tCell.Text = "Efficiency";
                                }
                                else
                                {
                                    tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                tRow.Cells.Add(tCell);
                                tRow.BackColor = ColorTranslator.FromHtml("#EBEBE4");
                            }
                            else
                            {
                                string FieldName = row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                if (FieldName.Contains("StandardRate/HR") || FieldName.Contains("VendorRate") || FieldName.Contains("Baseqty") || FieldName.Contains("DurationperProcessUOM(Sec)") || FieldName.Contains("Efficiency/ProcessYield(%)") || FieldName.Contains("TurnkeyCost/pc") || FieldName.Contains("TurnkeyProfit") || FieldName.Contains("ProcessCost/pc") || FieldName.Contains("TotalProcessesCost/pcs"))
                                {
                                    tCell.Style.Add("text-align", "right");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "left");
                                }

                                #region restore value
                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            string ifsubcon = tempPClist[2].ToString().Replace("NaN", "");
                                            string ifturnkey = tempPClist[3].ToString().Replace("NaN", "");

                                            if (tempPClist[ii].ToString().Replace("NaN", "").Contains("--Select--"))
                                            {
                                                tCell.Text = "";
                                            }
                                            else
                                            {
                                                tCell.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                            }

                                            if (ifsubcon != "" || ifturnkey != "")
                                            {
                                                if (ii == 4)
                                                {
                                                    tCell.Text = "";
                                                }
                                                else if (ii == 5)
                                                {
                                                    tCell.Text = "";
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalProcessesCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotProcCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                #endregion
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                    // TableMat = Table1;
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

                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();


                        int rowcount = 0;
                        for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                TablePC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                #region New code
                                string FieldName = DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("StandardRate/HR") || FieldName.Contains("VendorRate") || FieldName.Contains("Baseqty") || FieldName.Contains("DurationperProcessUOM(Sec)") || FieldName.Contains("Efficiency/ProcessYield(%)") || FieldName.Contains("TurnkeyCost/pc") || FieldName.Contains("TurnkeyProfit") || FieldName.Contains("ProcessCost/pc"))
                                {
                                    tCell.Style.Add("text-align", "right");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                if (FieldName.Contains("TotalProcessesCost/pcs"))
                                {
                                    TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                }
                                else
                                {
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == (rowcount - 1))
                                        {
                                            string ifsubcon = TempPclistNew[2].ToString().Replace("NaN", "");
                                            string ifturnkey = TempPclistNew[3].ToString().Replace("NaN", "");

                                            if (TempPclistNew[ii].ToString().Replace("NaN", "").Contains("--Select--"))
                                            {
                                                tCell.Text = "";
                                            }
                                            else
                                            {
                                                tCell.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                            }

                                            if (ifsubcon != "" || ifturnkey != "")
                                            {
                                                if (ii == 4)
                                                {
                                                    tCell.Text = "";
                                                }
                                                else if (ii == 5)
                                                {
                                                    tCell.Text = "";
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            rowcount++;
                        }
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
        #endregion New 25-06-2019 CreateDynamicProcessDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
        //private void CreateDynamicSubMaterialDT(int ColumnType)
        //{
        //    DataTable DtDynamicSubMaterialsFields = new DataTable();
        //    DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicSubMaterialsDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicSubMaterialsDetails == null)
        //        //    DtDynamicSubMaterialsDetails = new DataTable();
        //        //if (DtDynamicSubMaterialsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableSMC.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicSubMaterialsFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableSMC.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
        //        //                tCell.Width = 240;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tCell.Controls.Add(tb);
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableSMC.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableSMC.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
        //        //                lb.Width = 240;
        //        //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tb.Attributes.Add("autocomplete", "off");


        //        //                // Data Store and Retrieve
        //        //                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }


        //        //                tCell.Controls.Add(tb);
        //        //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion
        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TableSMC.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            //else
        //            //    tCell1.Text = "Material Cost";

        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        // Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            TableSMC.Rows.Add(tRow);

        //            for (int i = 0; i <= 1; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
        //                    lb.Width = 240;
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Style.Add("text-transform", "uppercase");

        //                    // Data Store and Retrieve
        //                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //                    {
        //                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempSMClist.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {
        //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }


        //                    tCell.Controls.Add(tb);
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //            }
        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }
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

        //            TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();

        //            for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tb.Style.Add("text-transform", "uppercase");
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
        #endregion old 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
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
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        Hearderrow.Cells.Add(tCell1);
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
                                tCell.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
                                tCell.Width = 150;
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                if (FieldName.Contains("Sub-Mat/T&JDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                // Data Store and Retrieve
                                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                {
                                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalSub-Mat/T&JCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotSubMatCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                tCell.Width = 150;
                            }
                            else
                            {
                                string FieldName = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("Sub-Mat/T&JDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                if (FieldName.Contains("TotalSub-Mat/T&JCost/pcs"))
                                {
                                    TableSMC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                }
                                else
                                {
                                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                }

                                // Data Retrieve and assign from Storage
                                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                {

                                    for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                    {

                                        if (ii == (cellCtr - 1))
                                        {

                                            tCell.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                            tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Session["TableSMC"] = TableSMC;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        #endregion New 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)

        #region old 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        //private void CreateDynamicOthersCostDT(int ColumnType)
        //{
        //    DataTable DtDynamicOtherCostsFields = new DataTable();
        //    DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicOtherCostsDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicOtherCostsDetails == null)
        //        //    DtDynamicOtherCostsDetails = new DataTable();
        //        //if (DtDynamicOtherCostsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableOthers.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicOtherCostsFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableOthers.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
        //        //                tCell.Width = 240;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //        //                //{

        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                //}

        //        //                // Data Store and Retrieve
        //        //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }


        //        //                tCell.Controls.Add(tb);
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableOthers.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableOthers.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
        //        //                lb.Width = 240;
        //        //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //        //                //{

        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                //}



        //        //                // Data Store and Retrieve
        //        //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }


        //        //                tCell.Controls.Add(tb);
        //        //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion

        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TableOthers.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            //else
        //            //    tCell1.Text = "Material Cost";

        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        // Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            TableOthers.Rows.Add(tRow);

        //            for (int i = 0; i <= 1; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
        //                    lb.Width = 240;
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //                    //{
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    //}



        //                    // Data Store and Retrieve
        //                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //                    {
        //                        var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempSMClist.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {
        //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }


        //                    tCell.Controls.Add(tb);
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //            }
        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }
        //        Session["TableOthers"] = TableOthers;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableOthers = (Table)Session["TableOthers"];


        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //            var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //            var tempOtherlistNew = tempOtherlist;

        //            //if (CellsCount == 2 && tempOtherlist.Count <= (DtDynamicOtherCostsFields.Rows.Count + 1))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i)) * (DtDynamicOtherCostsFields.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempOtherlist.Count > 6)
        //            ////    tempOtherlistNew = tempOtherlistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount == 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i + 1)) * DtDynamicOtherCostsFields.Rows.Count)).ToList();
        //            //else if (CellsCount >= 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(i * (DtDynamicOtherCostsFields.Rows.Count)).ToList();


        //            tempOtherlistNew = tempOtherlist.Skip(i * DtDynamicOtherCostsFields.Rows.Count).ToList();

        //            for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //                    {
        //                        TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    }

        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tb.Style.Add("text-transform", "uppercase");

        //                    // Data Retrieve and assign from Storage
        //                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {

        //                                tb.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        Session["TableOthers"] = TableOthers;
        //    }

        //}
        #endregion old 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        private void CreateDynamicOthersCostDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;
                    TableRow Hearderrow = new TableRow();

                    TableOthers.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        Hearderrow.Cells.Add(tCell1);
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
                                if (hdnLayoutScreen.Value == "Layout7")
                                {
                                    tCell.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/UOM");
                                }
                                else
                                {
                                    tCell.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                if (FieldName.Contains("ItemsDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                // Data Store and Retrieve
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalOtherItemCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotOthCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                    TableOthers = (Table)Session["TableOthers"];
                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {

                        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                        var tempOtherlistNew = tempOtherlist;

                        tempOtherlistNew = tempOtherlist.Skip(i * DtDynamicOtherCostsFields.Rows.Count).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("ItemsDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                if (FieldName.Contains("TotalOtherItemCost/pcs"))
                                {
                                    TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");

                                }
                                else
                                {
                                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
                                }
                                // Data Retrieve and assign from Storage
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {

                                            tCell.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
        #endregion New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicUnitDT(int ColumnType)
        //private void CreateDynamicUnitDT(int ColumnType)
        //{
        //    DataTable DtDynamicUnitFields = new DataTable();
        //    DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicUnitDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicUnitDetails == null)
        //        //    DtDynamicUnitDetails = new DataTable();
        //        //if (DtDynamicUnitDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableUnit.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicUnitFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableUnit.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //        //                {
        //        //                    tCell.Text = "Total Process Cost/pc";
        //        //                }
        //        //                else
        //        //                {
        //        //                    tCell.Text = row.ItemArray[0].ToString().Replace("Cost/pcs -", "/pc");
        //        //                }
        //        //                tCell.Width = 240;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Style.Add("text-transform", "uppercase");
        //        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tCell.Controls.Add(tb);
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}

        //        ////start from here
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableUnit.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //        {
        //        //            tCell1.Text = "Field Name";
        //        //        }
        //        //        else if (cellCtr == 2)
        //        //        {
        //        //            tCell1.Text = "Profit (%)";
        //        //        }
        //        //        else if (cellCtr == 3)
        //        //        {
        //        //            tCell1.Text = "Discount (%)";
        //        //        }
        //        //        else if (cellCtr == 4)
        //        //        {
        //        //            tCell1.Text = "Final Quote Price/pc";
        //        //        }
        //        //        else if (cellCtr == 5)
        //        //        {
        //        //            tCell1.Text = "Net Profit/Discount";
        //        //        }
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableUnit.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 5; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("Profit(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("Discount(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("FinalQuotePrice/pcs"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else
        //        //                {
        //        //                    Label lb = new Label();
        //        //                    tCell.Controls.Add(lb);
        //        //                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //        //                    {
        //        //                        lb.Text = "Total Process Cost/pc";
        //        //                    }
        //        //                    else
        //        //                    {

        //        //                        lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Cost/pcs -", "Cost/pc");
        //        //                    }
        //        //                    lb.Width = 240;
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //            else if (i == 1)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("Profit(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("Discount(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("FinalQuotePrice/pcs"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("GrandTotalCost/pcs"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }

        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //            else if (i == 2)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("TotalProcessesCost/pcs-"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtProfit(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    tb.Attributes.Add("disabled", "disabled");


        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("Profit(%)"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //                else
        //        //                {
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                }

        //        //            }
        //        //            else if (i == 3)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("TotalProcessesCost/pcs-"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtDiscount(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    tb.Attributes.Add("disabled", "disabled");

        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("Discount(%)"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //                else
        //        //                {
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                }
        //        //            }
        //        //            else if (i == 4)
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
        //        //                tCell.Controls.Add(tb);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Style.Add("text-transform", "uppercase");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //        //                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                {
        //        //                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }
        //        //                    }
        //        //                }

        //        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else if (i == 5)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("GrandTotalCost/pcs"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtNetProfit(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);

        //        //                }
        //        //                else
        //        //                {
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //        }

        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion

        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TableUnit.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //            {
        //                tCell1.Text = "Field Name";
        //            }
        //            else if (cellCtr == 2)
        //            {
        //                tCell1.Text = "Profit (%)";
        //            }
        //            else if (cellCtr == 3)
        //            {
        //                tCell1.Text = "Discount (%)";
        //            }
        //            else if (cellCtr == 4)
        //            {
        //                tCell1.Text = "Final Quote Price/pc";
        //            }
        //            else if (cellCtr == 5)
        //            {
        //                tCell1.Text = "Net Profit/Discount";
        //            }
        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        // Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            TableUnit.Rows.Add(tRow);

        //            for (int i = 0; i <= 5; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                    if (a.Contains("Profit(%)"))
        //                    {
        //                        break;
        //                    }
        //                    else if (a.Contains("Discount(%)"))
        //                    {
        //                        break;
        //                    }
        //                    else if (a.Contains("FinalQuotePrice/pcs"))
        //                    {
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //                        {
        //                            lb.Text = "Total Process Cost/pc";
        //                        }
        //                        else
        //                        {

        //                            lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Cost/pcs -", "Cost/pc");
        //                        }
        //                        lb.Width = 240;
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //                else if (i == 1)
        //                {
        //                    string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                    if (a.Contains("Profit(%)"))
        //                    {
        //                        break;
        //                    }
        //                    else if (a.Contains("Discount(%)"))
        //                    {
        //                        break;
        //                    }
        //                    else if (a.Contains("FinalQuotePrice/pcs"))
        //                    {
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                            {
        //                                string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                if (zz.Contains("GrandTotalCost/pcs"))
        //                                {
        //                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                    {
        //                                        if (ii == (z - 1))
        //                                        {
        //                                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //                else if (i == 2)
        //                {
        //                    string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                    if (a.Contains("TotalProcessesCost/pcs-"))
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txtProfit(%)0";
        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                        tb.Attributes.Add("disabled", "disabled");


        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                            {
        //                                string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                if (zz.Contains("Profit(%)"))
        //                                {
        //                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                    {
        //                                        if (ii == (z - 1))
        //                                        {
        //                                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                    }

        //                }
        //                else if (i == 3)
        //                {
        //                    string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                    if (a.Contains("TotalProcessesCost/pcs-"))
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txtDiscount(%)0";
        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                        tb.Attributes.Add("disabled", "disabled");

        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                            {
        //                                string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                if (zz.Contains("Discount(%)"))
        //                                {
        //                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                    {
        //                                        if (ii == (z - 1))
        //                                        {
        //                                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                    }
        //                }
        //                else if (i == 4)
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
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
        //                else if (i == 5)
        //                {
        //                    string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                    if (a.Contains("GrandTotalCost/pcs"))
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txtNetProfit(%)0";
        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);

        //                    }
        //                    else
        //                    {
        //                        tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //            }

        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }

        //        Session["TableUnit"] = TableUnit;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableUnit = (Table)Session["TableUnit"];

        //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell = new TableCell();
        //            if (cellCtr == 0)
        //            {
        //                Label lb = new Label();
        //                tCell.Controls.Add(lb);
        //                // lb.Text = "Material Cost";
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //            else
        //            {
        //                TextBox tb = new TextBox();
        //                tb.BorderStyle = BorderStyle.None;
        //                tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                tb.Attributes.Add("autocomplete", "off");
        //                tb.Style.Add("text-transform", "uppercase");
        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                tb.Attributes.Add("disabled", "disabled");
        //                tCell.Controls.Add(tb);
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //        }


        //        Session["TableUnit"] = TableUnit;
        //    }

        //}
        #endregion old 25-06-2019 CreateDynamicUnitDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicUnitDT(int ColumnType)
        private void CreateDynamicUnitDT(int ColumnType)
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
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
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
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
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
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
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
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
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
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
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
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 4)
                            {
                                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                {
                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
                                    {
                                        if (ii == (4))
                                        {
                                            tCell.Text = tempunitlist[7].ToString().Replace("NaN", "");
                                            break;
                                        }
                                        else if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }
                                    }
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else if (i == 5)
                            {
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                    TableUnit = (Table)Session["TableUnit"];

                    for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
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
                            tCell1.Text = "Final Cost/pc";
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
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
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
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    tCell.Text = GetGA().ToString();
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
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
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                }
                            }
                            else if (i == 4)
                            {
                                string a = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
                                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                {
                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
                                    {
                                        if (a.Contains("txtFinalQuotePrice/pcs4"))
                                        {
                                            if (ii >= 7)
                                            {
                                                tCell.Text = tempunitlist[7].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                        else if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }
                                    }
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
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
                    Session["TableUnit"] = TableUnit;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }
        #endregion New 25-06-2019 CreateDynamicUnitDT(int ColumnType)


        private void GetProcessDetailsbyQuoteDetails(string ProcessGrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + txtprocs.Text.ToString().ToUpper() + "' and DELFLAG = 0 ";
                da4 = new SqlDataAdapter(str4, MDMCon);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                {
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
                }

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select distinct ProcessGrpCode from TPROCESGROUP_SUBPROCESS Where ProcessGrpCode = '" + ProcessGrp + "' ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtProGrp);

                Session["DtDynamicProcessCostsDetails"] = dtProGrp;
                //DtDynamicProcessCostsDetails = dtProGrp;
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
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select CONCAT(ProcessGrpCode , ' - ', ProcessGrpDescription) as ProcessGrpCode, SubProcessName,ProcessUomDescription,ProcessUOM from TPROCESGROUP_SUBPROCESS  ";
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

        protected void MachineIdsbyProcess(string strProcessCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                string UserId = Session["userID_"].ToString();
                string VndCode = Session["mappedVendor"].ToString();
                string strplant = Session["strplant"].ToString();

                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM where VendorCode = '" + VndCode + "'  and Plant = '" + strplant + "' and ProcessGrp= '" + strProcessCode + "'  ";
                da2 = new SqlDataAdapter(str2, MDMCon);
                Result2 = new DataTable();
                da2.Fill(Result2);

                //ddlMachine.DataSource = Result2;
                //ddlMachine.DataBind();
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


        protected void GetData(string reqno)
        {

            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;


                strGetData = "select CONVERT(VARCHAR(10), S.RequestDate, 103) as RequestDate,S.QuoteNo,V.Description,v.Crcy,vp.PICName,vp.PICemail from tVendor_New as V inner join TVENDORPIC as VP" +
                              " on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + "TQuoteDetails as S on S.VendorCode1=v.Vendor where S.QuoteNo='" + reqno + "' ";
                da = new SqlDataAdapter(strGetData, MDMCon);
                da.Fill(dtget);

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
                SqlDataAdapter da = new SqlDataAdapter();
                // string str = "select  sp.PIC1Name as UseNam,sp.PIC1Email as UseEmail from Usr Inner join TSMNProductPIC sp on Usr.UseID = '" + userdet.Trim() + "'";
                string str1 = "select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product from TSMNProductPIC pp inner join " + TransDB.ToString() + "TQuotedetails TQ on pp.Product = TQ.Product and pp.Userid = Tq.CreatedBy and PP.Product = TQ.Product where QuoteNo='" + Session["Qno"].ToString() + "'  ";
                da = new SqlDataAdapter(str1, MDMCon);
                da.Fill(dtdate);
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


        protected void process()
        {
            DropDownList ddlProcess = new DropDownList();
            string UserId = Session["userID_"].ToString();
            string VndId = Session["mappedVendor"].ToString();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct CONCAT( tp.Process_Grp_code, ' - ', tp.Process_Grp_Description) as Process_Grp_code from TPROCESGROUP_LIST TP inner join TPROCESGROUP_SUBPROCESS TPS on Tp.Process_Grp_code = tps.ProcessGrpCode  ";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                ddlProcess.DataSource = Result;
                ddlProcess.DataTextField = "Process_Grp_code";
                ddlProcess.DataValueField = "Process_Grp_code";
                ddlProcess.DataBind();


                Session["process"] = ddlProcess.DataSource;

                #region  get subvendor data
                string[] DataVnd = lblVName.Text.Split('-');
                string VndCode = DataVnd[0].ToString();
                DropDownList ddlProcessSubVndorData = new DropDownList();
                DataTable ResultSubVndName = new DataTable();
                SqlDataAdapter daSubVndName = new SqlDataAdapter();
                string strSubVndName = "select distinct CONCAT( TKV.SubVendor, ' - ', TVN.Description) as SubVndorData  from TTURNKEY_VENDOR TKV join tVendor_New TVN on TKV.SubVendor = TVN.Vendor where TKV.TrnKeyVendor = '" + VndCode + "' ";
                da = new SqlDataAdapter(strSubVndName, MDMCon);
                ResultSubVndName = new DataTable();
                da.Fill(ResultSubVndName);

                ddlProcessSubVndorData.DataSource = ResultSubVndName;
                ddlProcessSubVndorData.DataTextField = "SubVndorData";
                ddlProcessSubVndorData.DataValueField = "SubVndorData";
                ddlProcessSubVndorData.DataBind();
                Session["SubVndorData"] = ddlProcessSubVndorData.DataSource;
                #endregion


                DataTable Result1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter();
                string str1 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as Machine, CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,TVM.FollowStdRate as FollowStdRate,TVM.Currency as Currency from TVENDORMACHNLIST TVM where TVM.VendorCode = '" + VndId + "'  ";
                da1 = new SqlDataAdapter(str1, MDMCon);
                Result1 = new DataTable();
                da1.Fill(Result1);

                grdMachinelisthidden.DataSource = Result1;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;


                DataTable Result3 = new DataTable();
                SqlDataAdapter da3 = new SqlDataAdapter();
                string str3 = "select CAST(ROUND(StdLabourRateHr,2) AS DECIMAL(12,2))as 'StdLabourRateHr',FollowStdRate,Currency from TVENDORLABRCOST TVC Where TVC.Vendorcode = '" + VndId + "'  ";
                da3 = new SqlDataAdapter(str3, MDMCon);
                Result3 = new DataTable();
                da3.Fill(Result3);

                grdLaborlisthidden.DataSource = Result3;
                grdLaborlisthidden.DataBind();
                Session["LaborListGrd"] = grdLaborlisthidden.DataSource;

                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM inner join " + TransDB.ToString() + "TQuoteDetails TQ on TVM.VendorCode = TQ.VendorCode1 Where TQ.QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'  ";
                da2 = new SqlDataAdapter(str2, MDMCon);
                Result2 = new DataTable();
                da2.Fill(Result2);

                ddlMachine.DataSource = Result2;
                ddlMachine.DataBind();
                Session["MachineIDs"] = ddlMachine.DataSource;

                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + txtprocs.Text.ToString().ToUpper() + "'  ";
                da4 = new SqlDataAdapter(str4, MDMCon);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
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

        private void RetrieveAllCostDetails()
        {
            string UserId = Session["userID_"].ToString();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                #region Other Cost

                string strGetData = "";

                strGetData = @"SELECT QuoteNo,ProcessGroup,UPPER(ItemsDescription) as 'ItemsDescription',[OtherItemCost/pcs] as 'OtherItemCost/pcs',CAST(ROUND([TotalOtherItemCost/pcs],5) AS DECIMAL(12,5)) as 'TotalOtherItemCost/pcs',RowId FROM TOtherCostDetails_D Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "' order by RowId asc ";
                da = new SqlDataAdapter(strGetData, EmetCon);
                da.Fill(dtget);

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
                strsub = @"SELECT QuoteNo,ProcessGroup,UPPER([Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',[Sub-Mat/T&JCost] as 'Sub-Mat/T&JCost',[Consumption(pcs)] as 'Consumption(pcs)',
                            [Sub-Mat/T&JCost/pcs] as 'Sub-Mat/T&JCost/pcs',CAST(ROUND([TotalSub-Mat/T&JCost/pcs],5) AS DECIMAL(12,5)) as 'TotalSub-Mat/T&JCost/pcs',RowId FROM TSMCCostDetails_D 
                            Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "' order by RowId asc ";
                da = new SqlDataAdapter(strsub, EmetCon);
                da.Fill(dtget);

                StringBuilder sbSub = new StringBuilder();

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtsubDesc = dtget.Rows[i].ItemArray[2].ToString();
                        var txtsubcost = dtget.Rows[i].ItemArray[3].ToString();
                        var txtConsumption = dtget.Rows[i].ItemArray[4].ToString();
                        var txtsubcostPC = dtget.Rows[i].ItemArray[5].ToString();
                        var txtTotalCostPC = dtget.Rows[i].ItemArray[6].ToString();

                        sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + txtTotalCostPC + ",");
                    }
                    hdnSMCTableValues.Value = sbSub.ToString();
                }

                #endregion SubMat Cost

                #region get sub vendor desc
                string DBNameFinal = EMETModule.GetDbMastername();
                #endregion get sub vendor desc

                #region Process Cost
                string strplant = Session["strplant"].ToString();
                string strProcess = "";
                dtget = new DataTable();
                strProcess = "SELECT PCD.[QuoteNo],PCD.[ProcessGroup],case when PCD.[ProcessGrpCode] = 'Select' then '' else PCD.[ProcessGrpCode] end as 'ProcessGrpCode',case when PCD.[SubProcess] = 'Select' then '' else PCD.[SubProcess] end as 'SubProcess',PCD.[IfTurnkey-VendorName]," +
                             "( CONCAT (RTRIM(PCD.[TurnKeySubVnd]),' - ',(select distinct Description from[" + DBNameFinal + "].[dbo].[tVendor_New] where Vendor = PCD.[TurnKeySubVnd])) ) as TurnKeySubVnd," +
                             "PCD.[Machine/Labor],PCD.[Machine],CAST(ROUND(PCD.[StandardRate/HR],7) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(PCD.VendorRate,7) AS DECIMAL(12,2))as 'VendorRate'," +
                             "PCD.[ProcessUOM],PCD.[Baseqty],PCD.[DurationperProcessUOM(Sec)],PCD.[Efficiency/ProcessYield(%)]," +
                             "PCD.[TurnKeyCost],PCD.[TurnKeyProfit],PCD.[ProcessCost/pc],CAST(ROUND(PCD.[TotalProcessesCost/pcs],5) AS DECIMAL(12,5)) as [TotalProcessesCost/pcs],PCD.[RowId]," +
                             "PCD.[UpdatedBy],PCD.[UpdatedOn] FROM [TProcessCostDetails_D] PCD  Where PCD.[QuoteNo] = '" + hdnQuoteNo.Value.ToString() + "' order by RowId asc ";
                da = new SqlDataAdapter(strProcess, EmetCon);
                da.Fill(dtget);

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

                                MDMCon.Open();
                                SqlConnection con1;
                                SqlCommand cmd;
                                SqlDataReader reader;
                                string str1 = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                                             " where Plant='" + strplant + "' and Process_Grp_code='" + Process_Grp_code + "' " +
                                             " and MachineType='" + MachineType + "' and (" + tonnage + " between Tonnage_From and Tonnage_To) ";
                                cmd = new SqlCommand(str1, MDMCon);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    txtProcUOM = txtProcUOM + '-' + reader["Strokes_min"].ToString();
                                }
                                reader.Dispose();
                            }
                            catch (Exception ee)
                            {
                                Response.Write(ee);
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

                strMCIM = @"SELECT QuoteNo,ProcessGroup,UPPER(MaterialSAPCode) as 'MaterialSAPCode',UPPER(MaterialDescription) as 'MaterialDescription',[RawMaterialCost/kg] as 'RawMaterialCost/kg',
                        CAST(ROUND([TotalRawMaterialCost/g],5) AS DECIMAL(12,4)) as 'TotalRawMaterialCost/g',[PartNetUnitWeight(g)] as '[PartNetUnitWeight(g)]',[~~DiameterID(mm)] as '~~DiameterID(mm)',[~~DiameterOD(mm)] as '~~DiameterOD(mm)', 
                        [~~Thickness(mm)] as '~~Thickness(mm)',[~~Width(mm)] as '~~Width(mm)', [~~Pitch(mm)] as '[~~Pitch(mm)]', [~MaterialDensity] as '~MaterialDensity',[~RunnerWeight/shot(g)] as '~RunnerWeight/shot(g)',
                        [~RunnerRatio/pcs(%)] as '~RunnerRatio/pcs(%)', [~RecycleMaterialRatio(%)] as '~RecycleMaterialRatio(%)', Cavity , [MaterialYield/MeltingLoss(%)] as 'MaterialYield/MeltingLoss(%)', 
                        CAST(ROUND([MaterialGrossWeight/pc(g)],5) AS DECIMAL(12,4)) as 'MaterialGrossWeight/pc(g)', [MaterialScrapWeight(g)] as 'MaterialScrapWeight(g)', [ScrapLossAllowance(%)] as 'ScrapLossAllowance(%)',
                        [ScrapPrice/kg] as 'ScrapPrice/kg', [ScrapRebate/pcs] as 'ScrapRebate/pcs', [MaterialCost/pcs] as 'MaterialCost/pcs', CAST(ROUND([TotalMaterialCost/pcs],5) AS DECIMAL(12,5)) as 'TotalMaterialCost/pcs',
                        RowId,RawMaterialCostUOM FROM TMCCostDetails_D Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "' order by RowId asc ";
                da = new SqlDataAdapter(strMCIM, EmetCon);
                da.Fill(dtget);

                StringBuilder sbMCIM = new StringBuilder();


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

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        strMCode = dtget.Rows[i].ItemArray[2].ToString();
                        strMDesc = dtget.Rows[i].ItemArray[3].ToString();
                        if (dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper() == "KG")
                        {
                            strRawCost = dtget.Rows[i].ItemArray[4].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                            strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString() + " / " + " G";
                        }
                        else
                        {
                            strRawCost = dtget.Rows[i].ItemArray[4].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                            strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                        }
                        strPartUnitW = dtget.Rows[i].ItemArray[6].ToString();

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
                        #endregion MS

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

                }

                #endregion MC Cost

                #region Unit Cost
                dtget = new DataTable();
                string strUnitData = "";

                strUnitData = @"select CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost',
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                            Profit,Discount,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as 'FinalQuotePrice',
                            CommentByVendor from TQuoteDetails_D  Where QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
                da = new SqlDataAdapter(strUnitData, EmetCon);
                da.Fill(dtget);

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
            finally
            {
                EmetCon.Dispose();
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

        protected void BtnPreview_Click(object sender, EventArgs e)
        {
            try
            {

                if (LbFlName.Text != "No File")
                {
                    string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string FileExtension = "pdf";
                    string filename = LbFlNameOri.Text;
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
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File deleted !')", true);
                            LbFlName.Text = "No File";
                            UnitCostDataStore();
                            OthersCostDataStore();
                            subMatCostDataStore();
                            ProcessCostDataStore();
                            MCCostDataStore();
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

        protected void GvQuoteDataPIR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "LinktoRedirect")
                {
                    //new Version
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GvQuoteDataPIR.Rows[rowIndex];
                    LinkButton LbMassRevQutoteRef = row.FindControl("LbMassRevQutoteRef") as LinkButton;
                    if (LbMassRevQutoteRef != null)
                    {
                        string QuoteNo = LbMassRevQutoteRef.Text;
                        Response.Redirect("VViewRequest.aspx?Number=" + QuoteNo);
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

        private void Getcreateuser()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select UseEmail from usr where UseID = '" + Session["userID_"].ToString() + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    Session["Createuser"] = dtdate.Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    Session["Createuser"] = "";
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

        bool SendingMail()
        {
            try
            {
                Getcreateuser();
                string MsgErr = "";
                //Email
                //testing sending new
                // getting Messageheader ID from IT Mailapp
                #region getting Messageheader ID from IT Mailapp
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
                        transactionHS.Rollback();
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                    cnn.Dispose();
                }
                #endregion

                Boolean IsAttachFile = true;
                int SequenceNumber = 1;
                string UserId = Session["userID_"].ToString();
                IsAttachFile = false;
                Session["SendFilename"] = "NOFILE";
                OriginalFilename = "NOFILE";
                format = "NO";

                //getting vendor mail id
                #region getting vendor mail id
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
                #endregion

                //getting User mail id
                #region getting User mail id
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
                #endregion

                //getting vendor mail content
                #region getting vendor mail content
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
                #endregion

                // Insert header and details to Mil server table to IT mailserverapp
                #region Insert header and details to Mil server table to IT mailserverapp
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
                    string Subject = "Quotation Number: " + hdnQuoteNo.Value + " - Confirm By : " + nameC + " - Plant : " + Session["VPlant"].ToString();
                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Confirm.<br /><br />" + body1.ToString();
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
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
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
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                    Email_inser.Dispose();
                    //End Details
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                return false;
            }
        }


        bool ProcessSubmit()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (Session["Qno"] != null)
                {
                    string QuNo = Session["Qno"].ToString();
                    sql = @"  update TQuoteDetails set 
                            TotalMaterialCost = (select TotalMaterialCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            TotalSubMaterialCost = (select TotalSubMaterialCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            TotalProcessCost = (select TotalProcessCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            TotalOtheritemsCost = (select TotalOtheritemsCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            GrandTotalCost = (select GrandTotalCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            FinalQuotePrice = (select FinalQuotePrice from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            Profit = (select Profit from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            Discount = (select Discount from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            UpdatedBy = @UpdatedBy,
                            UpdatedOn = CURRENT_TIMESTAMP,
                            CountryOrg = (select CountryOrg from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            CommentByVendor = (select CommentByVendor from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            EmpSubmitionOn = (select EmpSubmitionOn from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            EmpSubmitionBy = (select EmpSubmitionBy from TQuoteDetails_D where QuoteNo=@QuoteNo),
                            ApprovalStatus = '2' , PICApprovalStatus=0
                            where QuoteNo=@QuoteNo

                            delete from TMCCostDetails where QuoteNo=@QuoteNo
                            delete from TOtherCostDetails where QuoteNo=@QuoteNo
                            delete from TProcessCostDetails where QuoteNo=@QuoteNo
                            delete from TSMCCostDetails where QuoteNo=@QuoteNo

                            INSERT INTO TMCCostDetails SELECT * FROM TMCCostDetails_D WHERE QuoteNo=@QuoteNo;
                            INSERT INTO TOtherCostDetails SELECT * FROM TOtherCostDetails_D WHERE QuoteNo=@QuoteNo;
                            INSERT INTO TProcessCostDetails SELECT * FROM TProcessCostDetails_D WHERE QuoteNo=@QuoteNo;
                            INSERT INTO TSMCCostDetails SELECT * FROM TSMCCostDetails_D WHERE QuoteNo=@QuoteNo;

                            delete from TMCCostDetails_D where QuoteNo=@QuoteNo
                            delete from TOtherCostDetails_D where QuoteNo=@QuoteNo
                            delete from TProcessCostDetails_D where QuoteNo=@QuoteNo
                            delete from TSMCCostDetails_D where QuoteNo=@QuoteNo
                            delete from TQuoteDetails_D where QuoteNo=@QuoteNo
                          ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    cmd.Parameters.AddWithValue("@UpdatedBy", Session["userID_"].ToString());
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                else
                {
                    return false;
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

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (EMETModule.IsSubmit(hdnQuoteNo.Value) == false)
                {
                    if (ProcessSubmit() == true)
                    {
                        if (SendingMail() == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Approve request success !');CloseLoading();window.location ='Vendor.aspx';", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Approve request success ! Sending Maill Faill !!');CloseLoading();window.location ='Vendor.aspx';", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Submit Faill, Please Contact your administrator !');CloseLoading();window.location ='Vendor.aspx';", true);
                    }
                }
                else
                {
                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("This Quotation Already Approve !");
                    var script = string.Format("alert({0});CloseLoading();window.location ='Vendor.aspx';", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
            }
            catch (ThreadAbortException ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            catch (Exception ex2)
            {
                LbMsgErr.Text = ex2.StackTrace.ToString() + " - " + ex2.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex2);
            }
        }
    }
}