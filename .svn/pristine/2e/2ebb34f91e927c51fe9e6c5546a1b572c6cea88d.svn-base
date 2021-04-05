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

namespace Material_Evaluation
{
    public partial class NewReq_changesTmShmn : System.Web.UI.Page
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
        //email

        int ColmNoMat = 1;
        int ColmNoProc = 1;
        int ColmNoSubMat = 1;
        int ColmNoOth = 1;

        string PlantDesc = "";
        string SMNPICSubmDept = "";
        string GA = "";
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            string mappeduserid;
            string mappedname;
            if (Session["userID_"] == null || Session["UserName"] == null )
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {

                //DicQuoteDetails = new Dictionary<string, string>();

                // Raja load one by one Grid Dynamically
                string UserId = Session["userID_"].ToString();
                // Userid = Session["Userid"].ToString();
                //userId = Session["userID_"].ToString();
                //userId1 = Session["userID_"].ToString();
                UserId = Session["userID_"].ToString();


                //concat = sname + " - " + mappedname;
                TextBox1.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                txtfinal.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                if (!IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                    {

                        string userId = Session["userID_"].ToString();
                        UserId = Session["userID_"].ToString();
                        // Userid = Session["Userid"].ToString();
                        userId = Session["userID_"].ToString();
                        UserId = Session["userID_"].ToString();
                        LoadSavings();

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
                        //Session["UserName"] = sname;
                        //Session["userID"] = userId;


                        Session["Qno"] = Request.QueryString["Number"];
                        Session["MtlAddCount"] = 1;
                        Session["PCAddCount"] = 1;
                        Session["SMCAddCount"] = 1;
                        Session["OthersAddCount"] = 1;
                        //QuoteNo = Session["Qno"].ToString();
                        //MtlAddCount = 1;
                        //PCAddCount = 1;
                        //SMCAddCount = 1;
                        //OthersAddCount = 1;
                        //UnitAddCount = 1;
                        string QuoteNo = Session["Qno"].ToString();
                        lblreqst.Text = "Quote No : " + QuoteNo.ToString();
                        //LbsystemVersion.Text = Session["SystemVersion"].ToString();
                        //metfields = new List<string>();
                        //DtDynamic = new DataTable();
                        //DtMaterial = new DataTable();
                        //DicQuoteDetails = new Dictionary<string, string>();

                        GetQuoteandAllDetails(QuoteNo);
                        GetQuoteDetailsbyQuotenumber(QuoteNo);
                        CreateDynamicTablebasedonProcessField();
                        Getcreateuser();



                        string struser = (string)HttpContext.Current.Session["userID_"].ToString();

                        GetSHMNPICDetails(struser);
                        // GetData(QuoteNo);
                        GetData(QuoteNo);

                        process();
                        TurnKeyprovit();
                        Countryoforigin();

                        //if (txtPIRtype.Text.ToString().ToUpper().Contains("SUBCON"))
                        //{
                        //    btnAddColumns.Enabled = false;

                        //}
                        CreateDynamicDT(0);
                        CreateDynamicProcessDT(0);
                        CreateDynamicSubMaterialDT(0);
                        CreateDynamicOthersCostDT(0);
                        if (hdnVendorType.Value == "TeamShimano")
                        {
                            CreateDynamicUnitDTTmShimano(0);
                        }
                        else
                        {
                            CreateDynamicUnitDT(0);
                        }

                        //CreateDynamicDT(0);
                        //CreateDynamicProcessDT(0);
                        //CreateDynamicSubMaterialDT(0);
                        //CreateDynamicOthersCostDT(0);
                        //CreateDynamicUnitDT(0);
                    }
                }
            }
        }

        protected void OpenSqlConnectionMaster()
        {
            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            con = new SqlConnection(connetionString);
            con.Open();
        }

        string GetPlantPICSMNSubmit(string PlantId)
        {
            try
            {
                OpenSqlConnectionMaster();
                sql = @"select distinct CONCAT(Plant, ' - ', Description) as PlantDesc from TPLANT where Plant=@Plant";

                cmd = new SqlCommand(sql, con);
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
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return PlantDesc;
        }

        string GetDeptPICSMNSubmit(string UseId)
        {
            try
            {
                OpenSqlConnectionMaster();
                sql = @"select distinct UseDep as UseDep from usr where UseId=@UseId";

                cmd = new SqlCommand(sql, con);
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
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return SMNPICSubmDept;
        }

        string GetGA()
        {
            try
            {
                OpenSqlConnectionMaster();
                sql = @"select GA from TUNITPRICETEAMSHMN where VendorCode=@VendorCode and Plant=@Plant and CostHeader=@CostHeader";

                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@VendorCode", Session["vendorC"].ToString());
                cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                cmd.Parameters.AddWithValue("@CostHeader", "Grand Total");
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
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
            return GA;
        }

        protected void Countryoforigin()
        {
            //string strproc1 = (string)HttpContext.Current.Session["process"].ToString();
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select countrycode,CountryDescription,Currency from TCountrycode where active=1 order by CountryDescription ";
                da = new SqlDataAdapter(str, con1);
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
                string corg = ddlpirjtype.SelectedItem.Value;
                Session["corg"] = ddlpirjtype.SelectedItem.Value;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con1.Close();
            }
        }

        protected void GetDataPrcGroupVsStukMin()
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataPrcGroupVsStukMin = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string str = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                             " where Plant='" + strplant + "' and Process_Grp_code='" + hdnProcGroup.Value + "' " +
                             " and MachineType='" + hdnMachineType.Value + "' and (" + hdnTonnage.Value + " between Tonnage_From and Tonnage_To) and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                DtDataPrcGroupVsStukMin = new DataTable();
                da.Fill(DtDataPrcGroupVsStukMin);

                if (DtDataPrcGroupVsStukMin.Rows.Count > 0)
                {
                    string efficiency = DtDataPrcGroupVsStukMin.Rows[0]["Efficiency"].ToString();
                    string Stk_Min = DtDataPrcGroupVsStukMin.Rows[0]["Strokes_min"].ToString();
                    string ColumTblProcNo = hdnColumTblProcNo.Value;
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "redirect", "PrcGrpVsStokes_Min(" + ColumTblProcNo + "," + Stk_Min + "," + efficiency + ");", true);
                }
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataMacTypNtoonage()
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string MacID = getMachineID();
                string str = " select MachineType,Tonnage from TVENDORMACHNLIST where Plant='" + strplant + "' and MachineID= '" + MacID + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    hdnMachineType.Value = DtDataMacVnd.Rows[0]["MachineType"].ToString();
                    hdnTonnage.Value = DtDataMacVnd.Rows[0]["Tonnage"].ToString();
                }
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataSubProcesGroup()
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataSubProcGroup = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string str = @" Select SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= '" + hdnProcGroup.Value.ToString() + "' and DELFLAG = 0 ";

                da = new SqlDataAdapter(str, con1);
                DtDataSubProcGroup = new DataTable();
                da.Fill(DtDataSubProcGroup);
                //grdMachinelisthidden = null;
                grdSubProcessGrphidden.DataSource = DtDataSubProcGroup;
                grdSubProcessGrphidden.DataBind();
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataSubProcesGroup2(string ProcGroup)
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataSubProcGroup = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string str = @" Select SubProcessName from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= '" + ProcGroup + "' and DELFLAG = 0 order by SubProcessName ASC ";

                da = new SqlDataAdapter(str, con1);
                DtDataSubProcGroup = new DataTable();
                da.Fill(DtDataSubProcGroup);
                Session["SubProcess"] = DtDataSubProcGroup;

                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataVendRate(string ProcGroup, string MachineId)
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataStdrRate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string str = @" select  TVM.MachineID,TVM.SMNStdrateHr as SMNStdrateHr ,
                                TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = '" + Session["mappedVendor"].ToString() + "' and Plant = '" + strplant + "' and ProcessGrp= '" + ProcGroup + "'and  TVM.MachineID= '" + MachineId + "' and TVM.DELFLAG = 0 ";

                da = new SqlDataAdapter(str, con1);
                DtDataStdrRate = new DataTable();
                da.Fill(DtDataStdrRate);
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
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataProcesUOM()
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataProcesUOM = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string str = @" Select SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS 
                                where  ProcessGrpCode= '" + hdnProcGroup.Value.ToString() + "' and SubProcessName = '" + hdnSubProcGroup.Value.ToString() + "' and DELFLAG = 0 ";

                da = new SqlDataAdapter(str, con1);
                DtDataProcesUOM = new DataTable();
                da.Fill(DtDataProcesUOM);
                if (DtDataProcesUOM.Rows.Count > 0)
                {
                    hdnProcUOM.Value = DtDataProcesUOM.Rows[0]["ProcessUomDescription"].ToString();
                }
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataDataVndmachine()
        {
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                //string str = @" select CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as Machine, TVM.SMNStdrateHr as SMNStdrateHr ,
                //                TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM 
                //                where TVM.VendorCode = '" + Session["mappedVendor"].ToString() + "' and Plant = '" + strplant + "' and ProcessGrp= '" + hdnProcGroup.Value.ToString() + "' and TVM.DELFLAG = 0 ";
                string str = @" select CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as Machine, TVM.SMNStdrateHr as SMNStdrateHr ,
                                TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = '" + Session["mappedVendor"].ToString() + "' and Plant = '" + strplant + "' and ProcessGrp= '" + hdnProcGroup.Value.ToString() + "' and TVM.DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);
                //grdMachinelisthidden = null;
                grdMachinelisthidden.DataSource = DtDataMacVnd;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;
                con1.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GetDataVndmachine2(string ProcGroup)
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                //string str = @" select CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as Machine from TVENDORMACHNLIST TVM 
                //                where TVM.VendorCode = '" + Session["mappedVendor"].ToString() + "' and Plant = '" + strplant + "' and ProcessGrp= '" + ProcGroup + "' and TVM.DELFLAG = 0 ";
                string str = @" select CONCAT(LTRIM(RTRIM(TVM.MachineID)),' - ',LTRIM(RTRIM(TVM.MachineDescription))) as Machine from TVENDORMACHNLIST TVM 
                                where TVM.VendorCode = '" + Session["mappedVendor"].ToString() + "' and Plant = '" + strplant + "' and ProcessGrp= '" + ProcGroup + "' and TVM.DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);
                Session["SubVndMachineByProcGroup"] = DtDataMacVnd;

            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
            finally
            {
                con1.Close();
            }
        }

        string getMachineID()
        {
            string MachineId = "";
            try
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);
                con1.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string str = " select MachineID from TVENDORMACHNLIST where Plant='" + strplant + "' and CONCAT(LTRIM(RTRIM(MachineID)),' - ',LTRIM(RTRIM(MachineDescription))) = '" + hdnMachineId.Value + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    MachineId = DtDataMacVnd.Rows[0]["MachineID"].ToString();
                }
                con1.Close();
            }
            catch (Exception ee)
            {
                MachineId = "";
                Response.Write(ee);
            }
            return MachineId;
        }

        protected void LoadSavings()
        {

            try
            {

                var Email_insert = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                using (SqlConnection Email_inser = new SqlConnection(Email_insert))
                {
                    Email_inser.Open();
                    SqlCommand sqlCmd = new SqlCommand("Email_UNC", Email_inser);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr;
                    dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        //Userid = dr.GetString(0);
                        Session["USERIdMail"] = dr.GetString(0);
                        //Session["Userid"] = dr.GetString(0);
                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
                        URL = dr.GetString(4);
                        MasterDB = dr.GetString(5);
                        TransDB = dr.GetString(6);
                    }
                    dr.Close();
                    Email_inser.Close();
                }
            }

            catch (Exception x)
            {

                string message = x.Message;

            }

        }

        // Used this method by Raja
        private void GetQuoteDetailsbyQuotenumber(string QuoteNo)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str;
                str = "select * from " + TransDB.ToString() + "TQuoteDetails where QuoteNo='" + QuoteNo + "'  ";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtdate);

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
                var connetionString = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                //var quoteNo = txtQuoteNo.Text;
                using (SqlConnection con = new SqlConnection(connetionString))
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
                                vendorC = pir.Rows[0]["vendor"].ToString();
                                Session["vendorC"] = pir.Rows[0]["vendor"].ToString();
                                lblVName.Text = vendorC + "-" + pir.Rows[0]["description"].ToString();
                                lblCurrency.Text = "Vendor Currency: " + pir.Rows[0]["crcy"].ToString();
                                lblCity.Text = "Vendor Country: " + pir.Rows[0]["cty"].ToString();
                                lblcry.Text = "(" + pir.Rows[0]["crcy"].ToString() + ")";


                            }
                        }
                    }
                    //End
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }
        }

        //Used this method by Raja
        private void GetMaterialDetailsbyQuoteDetails(string MaterialNo, string Product, string Plant, string MtlClass, string QuoteNo)
        {
            // Plant = "2100";
            //Product = "BO";
            var connetionString = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            //var quoteNo = txtQuoteNo.Text;
            using (SqlConnection con = new SqlConnection(connetionString))
            {
                try
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
                                vendorC = pir.Rows[0]["vendor"].ToString();
                                Session["vendorC"] = pir.Rows[0]["vendor"].ToString();
                                lblVName.Text = vendorC + "-" + pir.Rows[0]["description"].ToString();
                                lblCurrency.Text = pir.Rows[0]["crcy"].ToString();
                                lblCity.Text = pir.Rows[0]["cty"].ToString();

                            }
                        }
                    }
                    //End
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
                finally
                {
                    con.Close();
                }
            }

            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();
                DataTable dtBOM = new DataTable();
                DataTable dtMaterial = new DataTable();
                DataTable dtMaterial1 = new DataTable();
                string strGetData = string.Empty;
                SqlDataAdapter da = new SqlDataAdapter();
                // string str = "select tb.Material From TBOMLIST tb inner join TMATERIAL tm on tb.Material = tm.Material inner join TPRODCOM TR on Tm.ProdComCode = TR.ProdComCode  where tb.ImmedParent='" + MaterialNo + "' and tb.Product='" + Product + "' and tb.Plant='" + Plant + "' and TR.ProdComDesc='" + MtlClass + "'   ";
                strGetData = "select top 1 TQ.Material" +
     " from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material " +
     "inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material " +
     " inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP" +
     " on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + "TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and  VP.Plant = TQ.Plant where TQ.QuoteNo='" + QuoteNo + "' and TB.FGcode = '" + MaterialNo + "' and (TQ.QuoteResponseduedate BETWEEN tc.ValidFrom and tc.ValidTO) and TM.DELFLAG = 0 ";

                da = new SqlDataAdapter(strGetData, consap);
                da.Fill(dtBOM);

                if (dtBOM.Rows.Count > 0)
                {
                    dtMaterial = new DataTable();
                    foreach (DataRow row in dtBOM.Rows)
                    {
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        //string str1 = "select   tm.material,tm.MaterialDesc,tc.Unit,tc.Amount from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer where TB.FGcode='" + row.ItemArray[0].ToString() + "' and tv.Vendor='" + vendorC.ToString() + "'  and tc.ValidTo >= getdate()";
                        string str1 = "select distinct   tm.material,tm.MaterialDesc,tc.Unit,isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer inner join tVendorPOrg tvp on tvp.POrg=tv.POrg inner join " + TransDB.ToString() + "TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant where TQ.QuoteNo='" + Session["Qno"].ToString() + "' and tm.plant= '" + Session["strplant"].ToString() + "'  and tvp.plant= '" + Session["strplant"].ToString() + "' and   (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and TB.FGcode='" + row.ItemArray[0].ToString() + "' and tv.Vendor='" + Session["vendorC"].ToString() + "' and TM.DELFLAG = 0 ";
                        da1 = new SqlDataAdapter(str1, consap);
                        da1.Fill(dtMaterial);
                        GridView1.DataSource = dtMaterial;
                        GridView1.DataBind();

                    }
                    //DtMaterialsDetails = dtMaterial;
                    Session["DtMaterialsDetails"] = dtMaterial;
                }
                else
                {
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    string strGetData1 = "select distinct TQ.Material from TMATERIAL TM inner join TCUSTOMER_MATLPRICING tc on TM.Material = tc.Material  inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + "TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and  VP.Plant = TQ.Plant where TQ.QuoteNo='" + QuoteNo + "' and TM.DELFLAG = 0 ";

                    da1 = new SqlDataAdapter(strGetData1, consap);
                    da1.Fill(dtBOM);

                    if (dtBOM.Rows.Count > 0)
                    {
                        dtMaterial = new DataTable();
                        foreach (DataRow row in dtBOM.Rows)
                        {
                            SqlDataAdapter da2 = new SqlDataAdapter();
                            string str2 = "select distinct tm.material,tm.MaterialDesc,'' as Unit, '' as Amount from TMATERIAL tm inner join TCUSTOMER_MATLPRICING tc on TM.Material = tc.Material where TM.Material='" + row.ItemArray[0].ToString() + "' and TM.DELFLAG = 0 ";
                            da2 = new SqlDataAdapter(str2, consap);
                            da2.Fill(dtMaterial);
                        }
                        //DtMaterialsDetails = dtMaterial;
                        Session["DtMaterialsDetails"] = new DataTable();
                    }
                    else
                    {
                        dtMaterial1 = new DataTable();
                        SqlDataAdapter da11 = new SqlDataAdapter();
                        //string str1 = "select   tm.material,tm.MaterialDesc,tc.Unit,tc.Amount from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer where TB.FGcode='" + row.ItemArray[0].ToString() + "' and tv.Vendor='" + vendorC.ToString() + "'  and tc.ValidTo >= getdate()";
                        string str1 = "select distinct   tm.material,tm.MaterialDesc,tc.Unit,isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer inner join " + TransDB.ToString() + "TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant where (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and TB.FGcode='" + 300208571 + "' and tv.Vendor='" + Session["vendorC"].ToString() + "' and TM.DELFLAG = 0";
                        da11 = new SqlDataAdapter(str1, consap);
                        da11.Fill(dtMaterial1);
                        GridView1.DataSource = dtMaterial1;
                        GridView1.DataBind();
                        Session["DtMaterialsDetails"] = new DataTable();
                    }

                }

                //if (DtMaterialsDetails == null)
                //    DtMaterialsDetails = new DataTable();

                //grdmatlcost.DataSource = DtMaterialsDetails;
                //grdmatlcost.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }
        }

        private void GetQuoteandAllDetails(string QuoteNo)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select * from TQuoteDetails where QuoteNo='" + QuoteNo + "' and CreateStatus ='Article' ";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();

                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MaterialDesc = dtdate.Rows[0].ItemArray[11].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();

                    string PartunitTxt = dtdate.Rows[0].ItemArray[18].ToString();
                    string baseUOm = dtdate.Rows[0].ItemArray[51].ToString();
                    TxtPlant.Text = GetPlantPICSMNSubmit(plant);
                    TxtDepartment.Text = GetDeptPICSMNSubmit(dtdate.Rows[0].ItemArray[46].ToString());
                    txtBaseUOM.Text = baseUOm.ToString();
                    txtBaseUOM1.Text = baseUOm.ToString();
                    txtMQty.Text = dtdate.Rows[0].ItemArray[54].ToString();
                    txtRem.Text = dtdate.Rows[0].ItemArray[55].ToString();

                    txtunitweight.Text = dtdate.Rows[0].ItemArray[58].ToString();
                    txtUOM.Text = dtdate.Rows[0].ItemArray[57].ToString();

                    lblcreateuser.Text = dtdate.Rows[0].ItemArray[46].ToString();

                    DateTime dt = DateTime.Parse(dtdate.Rows[0].ItemArray[20].ToString());

                    txtquotationDueDate.Text = dt.ToShortDateString();

                    txtdrawng.Text = dtdate.Rows[0].ItemArray[19].ToString();

                    txtprod.Text = dtdate.Rows[0].ItemArray[8].ToString();
                    txtpartdesc.Text = Material + " - " + MaterialDesc;
                    txtSAPJobType.Text = dtdate.Rows[0].ItemArray[16].ToString();
                    txtPIRtype.Text = dtdate.Rows[0].ItemArray[12].ToString();
                    txtprocs.Text = dtdate.Rows[0].ItemArray[13].ToString();

                    txtPartUnit.Value = PartunitTxt.ToString();
                    //GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass);
                    if (dtdate.Rows[0].ItemArray[21].ToString() != null && dtdate.Rows[0].ItemArray[21].ToString() != "")
                    {
                        DateTime dtEfective = DateTime.Parse(dtdate.Rows[0].ItemArray[21].ToString());
                        //TextBox1.Text = dtEfective.ToShortDateString();
                        TextBox1.Text = dtEfective.ToString("dd/MM/yyyy");
                    }
                    if (dtdate.Rows[0].ItemArray[22].ToString() != null && dtdate.Rows[0].ItemArray[22].ToString() != "")
                    {
                        DateTime dtDueDate = DateTime.Parse(dtdate.Rows[0].ItemArray[22].ToString());
                        //txtfinal.Text = dtDueDate.ToShortDateString();
                        txtfinal.Text = dtDueDate.ToString("dd/MM/yyyy");
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }

        }

        private void Getcreateuser()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select UseEmail from usr where UseID = '" + lblcreateuser.Text + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    Session["Createuser"] = dtdate.Rows[0].ItemArray[0].ToString();
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }
        }

        //Used this method by Raja
        public void GetMETfields(string ProcessGrpCode, string FieldGroup)
        {
            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;
            SqlDataReader reader;
            con = new SqlConnection(connetionString);

            SqlCommand cmd = new SqlCommand("MDM_GetDynamicFieldsforProcessGrpLayout", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProcessGrpCode", SqlDbType.NVarChar).Value = ProcessGrpCode;
                cmd.Parameters.Add("@FieldGroup", SqlDbType.NVarChar).Value = FieldGroup;

                reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                if (FieldGroup == "MC")
                {
                    //DtDynamic = dt;
                    Session["DtDynamic"] = dt;
                }
                else if (FieldGroup == "PC")
                {
                    //DtDynamicProcessFields = dt;
                    Session["DtDynamicProcessFields"] = dt;
                }
                else if (FieldGroup == "SMC")
                {
                    Session["DtDynamicSubMaterialsFields"] = dt;
                    //DtDynamicSubMaterialsFields = dt;
                }
                else if (FieldGroup == "Unit")
                {
                    Session["DtDynamicUnitFields"] = dt;
                    //DtDynamicUnitFields = dt;
                }
                else if (FieldGroup == "Others")
                {
                    Session["DtDynamicOtherCostsFields"] = dt;
                    //DtDynamicOtherCostsFields = dt;
                }


                //grdmatlcost.DataSource = DtMaterial;
                //grdmatlcost.DataBind();

                reader.Close();

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        public void CreateDynamicTablebasedonProcessField()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct FieldGroup from tMETFileds where DELFLAG = 0";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtdate);

                foreach (DataRow row in dtdate.Rows)
                {
                    GetMETfields(txtprocs.Text, row.ItemArray[0].ToString());
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }
        }


        #region Dyamic Table Creation for All Cost calculations

        /// <summary>
        /// Material Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        /// 
        private void CreateDynamicDT(int ColumnType)
        {
            DataTable DtDynamic = new DataTable();
            DtDynamic = (DataTable)Session["DtDynamic"];

            DataTable DtMaterialsDetails = new DataTable();
            DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

            if (ColumnType == 0)
            {
                int rowcount = 0;

                #region hdnMCTableValues has been fill
                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                {
                    int rowcountnew = 0;
                    int columncountbyType = 0;

                    if (ColumnType > 1)
                    {
                        columncountbyType = ColumnType - 1;
                    }

                    #region create header column and buuton delete
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
                            }

                        }
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                        Hearderrow.ForeColor = Color.White;
                    }

                    #endregion header column

                    #region add met field and textbox
                    for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        Table1.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                #region add met field to column 0
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                {
                                    lb.Text = "Base Qty / Cavity";
                                }
                                else
                                {
                                    lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                }

                                if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                {
                                    lb.Text = "Material/Melting Loss (%)";
                                }
                                lb.Width = 240;
                                Table1.Rows[cellCtr].Cells.Add(tCell);
                                #endregion add met field to column 0
                            }
                            else
                            {
                                #region create textbox
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (columncountbyType);
                                tb.Attributes.Add("autocomplete", "off");

                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                                string txtID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (columncountbyType);
                                if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                                else
                                {
                                    tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                }

                                if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(" + columncountbyType + ",'MATYIELD');");
                                }

                                if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                                {
                                    tb.Attributes.Add("onkeyup", "Validate(" + columncountbyType + ",'SCRAPALLOWENCE');");
                                }

                                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                #region condition have BOM 
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
                                            //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                            //double rawperkg = RawVal / 1000;
                                            //tb.Text = (rawperkg.ToString());
                                            tb.Enabled = false;
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else
                                        {
                                            tb.Enabled = true;
                                        }
                                    }
                                }
                                #endregion codition have BOM 

                                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("txtPartNetUnitWeight(g)") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
                                {
                                    if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                    {
                                        tb.Text = txtPartUnit.Value;
                                        tCell.Controls.Add(tb);
                                        Table1.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                        tCell.Controls.Add(tb);
                                        Table1.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                }

                                //if (txtPIRtype.Text.ToString().ToUpper().Contains("SUBCON"))
                                //{
                                //    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                //    {
                                //        tb.Text = "";
                                //    }
                                //    else
                                //    {
                                //        tb.Attributes.Add("disabled", "disabled");
                                //    }
                                //}

                                tCell.Controls.Add(tb);

                                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                {
                                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempMClist.Count; ii++)
                                    {
                                        if (ii == rowcountnew)
                                        {
                                            if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                            {
                                                tb.Text = "";
                                            }
                                            else
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                            }
                                            break;
                                        }

                                    }
                                }
                                tRow.Cells.Add(tCell);
                                #endregion create textbox
                            }

                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                        }
                        rowcountnew++;
                    }
                    #endregion add met field and textbox
                }
                #endregion hdnMCTableValues has been fill

                #region hdnMCTableValues empty
                else
                {
                    #region with BOM
                    if (DtMaterialsDetails.Rows.Count > 0)
                    {
                        #region create Header table
                        TableRow Hearderrow = new TableRow();
                        Table1.Rows.Add(Hearderrow);
                        for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
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
                                }
                            }
                            Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                            Hearderrow.ForeColor = Color.White;
                        }
                        #endregion create Header table

                        #region create textbox
                        foreach (DataRow row in DtDynamic.Rows)
                        {
                            TableRow tRow = new TableRow();
                            Table1.Rows.Add(tRow);

                            for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                            {
                                TableCell tCell = new TableCell();
                                if (cellCtr == 0)
                                {
                                    Label lb = new Label();
                                    tCell.Controls.Add(lb);
                                    if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                    {
                                        lb.Text = "Base Qty / Cavity";
                                    }
                                    else
                                    {
                                        lb.Text = row.ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                    }

                                    if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                    {
                                        lb.Text = "Material/Melting Loss (%)";
                                    }
                                    tCell.Width = 240;
                                    tRow.Cells.Add(tCell);
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                    tb.Attributes.Add("autocomplete", "off");
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    string txtID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                    if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                    }

                                    if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                    {
                                        tb.Attributes.Add("onkeyup", "Validate(" + (cellCtr - 1) + ",'MATYIELD');");
                                    }

                                    if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                                    {
                                        tb.Attributes.Add("onkeyup", "Validate(" + (cellCtr - 1) + ",'SCRAPALLOWENCE');");
                                    }

                                    if (tb.ID.Contains("txtMaterialSAPCode"))
                                    {
                                        tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                        // tb.Enabled = false;
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    else if (tb.ID.Contains("txtMaterialDescription"))
                                    {
                                        tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                        //tb.Enabled = false;
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                    else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    {
                                        double RawVal = 0;
                                        if (DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString() != "")
                                        {
                                            RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else
                                        {

                                        }

                                        double rawperkg = RawVal / 1000;
                                        tb.Text = (rawperkg.ToString());
                                        // tb.Enabled = false;

                                    }
                                    else if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                    {
                                        tb.MaxLength = 4;
                                    }
                                    else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                    {
                                        //tb.Attributes.Add("disabled", "disabled");
                                        tb.Text = txtPartUnit.Value;
                                        //tCell.Controls.Add(tb);
                                        //Table1.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                    else if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }

                                    //if (txtPIRtype.Text.ToString().ToUpper().Contains("SUBCON"))
                                    //{
                                    //    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    //    {
                                    //        tb.Text = "";
                                    //    }
                                    //    if (tb.ID.Contains("Cavity"))
                                    //    {
                                    //        //tb.Attributes.Add("disabled", "disabled");
                                    //    }
                                    //    else
                                    //    {
                                    //        tb.Attributes.Add("disabled", "disabled");
                                    //    }
                                    //}

                                    if (cellCtr > 1)
                                    {
                                        if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                        {
                                            Table1.Rows[DtDynamic.Rows.Count].Cells[1].Attributes.Add("colspan", cellCtr.ToString());
                                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[DtDynamic.Rows.Count].Cells[1].Controls[0])).Style.Add("text-align", "center");
                                        }
                                        else
                                        {
                                            tCell.Controls.Add(tb);
                                            tRow.Cells.Add(tCell);
                                        }
                                    }
                                    else
                                    {
                                        tCell.Controls.Add(tb);
                                        tRow.Cells.Add(tCell);
                                    }

                                    //Table1.Rows[cellCtr].Cells.Add(tCell);

                                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                    {
                                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempMClist.Count; ii++)
                                        {
                                            if (ii == rowcount)
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }

                                        }
                                    }

                                }
                            }

                            if (rowcount % 2 == 0)
                            {
                                tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                                tRow.BackColor = Color.White;
                            }
                            else
                            {
                                tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                                tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                            }
                            rowcount++;
                        }
                        #endregion create textbox
                    }
                    #endregion with bom

                    #region without BOM
                    else
                    {
                        int rowcountnew = 0;
                        int columncountbyType = 0;
                        if (ColumnType > 1)
                        {
                            columncountbyType = ColumnType - 1;
                        }

                        #region create Header table
                        TableRow Hearderrow = new TableRow();
                        Table1.Rows.Add(Hearderrow);
                        for (int cellClm = 0; cellClm <= 1; cellClm++)
                        {
                            TableCell tCell1 = new TableCell();
                            Label lb1 = new Label();
                            tCell1.Controls.Add(lb1);
                            if (cellClm == 0)
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
                                }
                            }
                            Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                            Hearderrow.ForeColor = Color.White;
                        }
                        #endregion create Header table

                        #region create Textbox
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
                                    string a = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper();
                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
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
                                    lb.Width = 240;
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TextBox tb = new TextBox();
                                    tb.BorderStyle = BorderStyle.None;
                                    tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (columncountbyType);
                                    tb.Attributes.Add("autocomplete", "off");

                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    string txtID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (columncountbyType);
                                    if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                                    else
                                    {
                                        tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                    }
                                    if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                                    {
                                        tb.Attributes.Add("onkeyup", "Validate(" + columncountbyType + ",'MATYIELD');");
                                    }

                                    if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                                    {
                                        tb.Attributes.Add("onkeyup", "Validate(" + columncountbyType + ",'SCRAPALLOWENCE');");
                                    }

                                    if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
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
                                                //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                                //double rawperkg = RawVal / 1000;
                                                //tb.Text = (rawperkg.ToString());
                                                tb.Enabled = false;
                                                tb.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                    }

                                    if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                    {
                                        tb.MaxLength = 4;
                                    }

                                    if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("txtPartNetUnitWeight(g)") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
                                    {
                                        if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                        {
                                            tb.Text = txtPartUnit.Value;
                                            tCell.Controls.Add(tb);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                        else
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                            tCell.Controls.Add(tb);
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                    }

                                    //if (txtPIRtype.Text.ToString().ToUpper().Contains("SUBCON"))
                                    //{
                                    //    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    //    {
                                    //        tb.Text = "";
                                    //    }
                                    //    if (tb.ID.Contains("Cavity"))
                                    //    {
                                    //        //tb.Attributes.Add("disabled", "disabled");
                                    //    }
                                    //    else
                                    //    {
                                    //        tb.Attributes.Add("disabled", "disabled");
                                    //    }
                                    //}


                                    tCell.Controls.Add(tb);


                                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                    {
                                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempMClist.Count; ii++)
                                        {
                                            if (ii == rowcountnew)
                                            {
                                                tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    tRow.Cells.Add(tCell);
                                }

                            }
                            if (rowcountnew % 2 == 0)
                            {
                                tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                                tRow.BackColor = Color.White;
                            }
                            else
                            {
                                tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                                tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                            }
                            rowcountnew++;
                        }
                        #endregion create Textbox
                    }
                    #endregion without BOM
                }
                #endregion hdnMCTableValues empty
                Session["Table"] = Table1;
            }
            else
            {
                Table1 = (Table)Session["Table"];

                int CellsCount = ColumnType;

                for (int i = 1; i < CellsCount; i++)
                {
                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();
                    var TempMClistNew = tempMClist;


                    //if (CellsCount == 2 && tempMClist.Count <= (DtDynamic.Rows.Count + 1))
                    //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i)) * (DtDynamic.Rows.Count + 1))).ToList();
                    //else if (CellsCount ==  && tempSMClist.Count > 6)
                    //    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                    //else if (CellsCount == 3 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 1))
                    //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamic.Rows.Count)).ToList();
                    //else if (CellsCount >= 3 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount == (i + 1))
                    //    TempMClistNew = TempMClistNew.Skip(((CellsCount) * (DtDynamic.Rows.Count))).ToList();
                    //else if (i >= 1 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 1))
                    //    TempMClistNew = TempMClistNew.Skip(i * (DtDynamic.Rows.Count)).ToList();
                    //else
                    //{
                    //    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();
                    //}

                    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

                    #region loop for Dynamic Binding
                    for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        if (cellCtr == 0)
                        {
                            #region create header table
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
                            }
                            #endregion create header table
                        }
                        else
                        {
                            #region cretae textbox
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");

                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            string txtID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            if (tb.ID.Contains("Code") || tb.ID.Contains("Description")) { }
                            else
                            {
                                tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                            }
                            if (tb.ID.Contains("txtMaterialYield/MeltingLoss(%)"))
                            {
                                tb.Attributes.Add("onkeyup", "Validate(" + i + ",'MATYIELD');");
                            }

                            if (tb.ID.Contains("txtScrapLossAllowance(%)"))
                            {
                                tb.Attributes.Add("onkeyup", "Validate(" + i + ",'SCRAPALLOWENCE');");
                            }

                            #region condition with BOM
                            if (DtMaterialsDetails.Rows.Count > 0)
                            {
                                if ((i + 1) <= DtMaterialsDetails.Rows.Count)
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
                                        //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                        //double rawperkg = RawVal / 1000;
                                        //tb.Text = (rawperkg.ToString());
                                        tb.Enabled = false;
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                }
                            }
                            #endregion condition with BOM

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
                                    ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                    ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
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
                            else if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") || tb.ID.Contains("txtMaterialGrossWeight/pc(g)"))
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
                            #endregion cretae textbox

                            #region populte data
                            if (hdnMCTableCount.Value != null && hdnMCTableCount.Value != "")
                            {
                                var colcount = int.Parse(hdnMCTableCount.Value);

                                if (colcount == 0)
                                {

                                }
                                else
                                {
                                    if (colcount >= (i))
                                    {
                                        if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                        {
                                            for (int ii = 0; ii < TempMClistNew.Count; ii++)
                                            {
                                                if (ii == (cellCtr - 1))
                                                {
                                                    if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
                                                    {
                                                        tb.Text = "";
                                                    }
                                                    else
                                                    {
                                                        tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                    break;
                                                }
                                            }
                                        }

                                    }
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
                                            tb.Text = "";
                                        }
                                        else
                                        {
                                            tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                        }
                                        break;
                                    }
                                }
                            }
                            #endregion populate data
                        }
                    }

                    #endregion Loop for Dynamic Binding
                    // }

                    //}
                }

                Session["Table"] = Table1;
                Table1.DataBind();
            }
            Session["Table"] = Table1;


        }

        /// <summary>
        /// Process Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicProcessDT(int ColumnType)
        {
            DataTable DtDynamicProcessFields = new DataTable();
            DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

            DataTable DtDynamicProcessCostsDetails = new DataTable();
            DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

            if (ColumnType == 0)
            {
                #region FirsColumn
                int rowcount = 0;
                TableRow Hearderrow = new TableRow();

                TablePC.Rows.Add(Hearderrow);
                for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                {
                    TableCell tCell1 = new TableCell();
                    #region Header
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
                    }
                    //Label lb1 = new Label();
                    //tCell1.Controls.Add(lb1);
                    //if (cellCtr == 0)
                    //    tCell1.Text = "Field Name";
                    //Hearderrow.Cells.Add(tCell1);
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                    Hearderrow.ForeColor = Color.White;
                    #endregion Header
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
                            #region fieldName of column 0
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
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
                            tCell.Width = 280;
                            tRow.Cells.Add(tCell);
                            #endregion fieldName
                        }
                        else
                        {
                            #region ProcessGroupCode
                            if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlProcess" + (cellCtr - 1);
                                ddl.DataSource = Session["process"];
                                ddl.DataTextField = "Process_Grp_code";
                                ddl.DataValueField = "Process_Grp_code";
                                ddl.DataBind();
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                ddl.Attributes.Add("onchange", "ProcGrpChange(" + (cellCtr - 1) + ")");

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");

                                            var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

                                            if (ddlcheck != null)
                                            {
                                                ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                string DdlProcGrpSelected = ddl.SelectedItem.Text.ToString();
                                                string[] ArrProcGrpSelected = DdlProcGrpSelected.Split('-');
                                                string strddlSelitem = ArrProcGrpSelected[0].ToString();

                                                MachineIdsbyProcess(strddlSelitem);
                                            }
                                            ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");

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
                                }

                                tCell.Controls.Add(ddl);
                            }
                            #endregion
                            #region SubProcess
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlSubProcess" + (cellCtr - 1);
                                ddl.DataSource = Session["PSGroupwithUOM"];
                                ddl.DataTextField = "SubProcessName";

                                ddl.DataValueField = "SubProcessName";
                                ddl.DataBind();
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                ddl.Attributes.Add("onchange", "SubProcgroupChnge(" + (cellCtr - 1) + ")");

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

                                            if (tempPClist.Count > 2)
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

                                            if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                            break;

                                        }

                                    }
                                }
                                else
                                {
                                    ddl.Attributes.Add("disabled", "disabled");
                                }
                                tCell.Controls.Add(ddl);
                            }
                            #endregion SubProcess
                            #region IfTurnkey-Subvendorname
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlSubvendorname" + (cellCtr - 1);
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

                                            if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    ddl.Attributes.Add("disabled", "disabled");
                                }

                                tCell.Controls.Add(ddl);
                            }
                            #endregion IfTurnkey-Subvendorname
                            #region Machine/Labor
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlMachineLabor" + (cellCtr - 1);

                                ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
                                ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
                                //ddl.Style.Add("width", "142px");
                                ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                DropDownList ddlhide = new DropDownList();
                                ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
                                ddlhide.Style.Add("display", "none");
                                ddlhide.Attributes.Add("disabled", "disabled");
                                //ddlhide.Style.Add("width", "142px");
                                ddlhide.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                ddl.Attributes.Add("onchange", "DdlMachineLaborChange(" + (cellCtr - 1) + ")");

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

                                                if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                ddl.Style.Add("display", "none");
                                                ddlhide.Style.Add("display", "block");
                                                if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }
                                        }
                                        if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }
                                else
                                {
                                    ddl.Attributes.Add("disabled", "disabled");
                                }

                                tCell.Controls.Add(ddl);
                                tCell.Controls.Add(ddlhide);

                            }
                            #endregion Machine/Labor
                            #region Machine
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlMachine" + (cellCtr - 1);
                                //ddl.Style.Add("width", "142px");
                                ddl.DataSource = Session["MachineIDs"];
                                ddl.DataTextField = "MachineID";
                                ddl.DataValueField = "MachineID";
                                ddl.DataBind();
                                if (ddl.Items.Count > 0)
                                {
                                    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                }
                                ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                ddl.Attributes.Add("onchange", "MachineChange(" + (cellCtr - 1) + ")");

                                TextBox tb = new TextBox();
                                tb.ID = "txtMachineId" + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                //tb.Attributes.Add("disabled", "disabled");
                                tb.Style.Add("display", "none");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                                DropDownList ddlMachide = new DropDownList();
                                ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
                                //ddlMachide.Style.Add("width", "142px");
                                ddlMachide.Style.Add("display", "none");
                                ddlMachide.Attributes.Add("disabled", "disabled");
                                ddlMachide.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


                                TextBox tbhide = new TextBox();
                                tbhide.ID = "txtHide" + (cellCtr - 1);
                                tbhide.Style.Add("display", "none");
                                tbhide.Attributes.Add("disabled", "disabled");
                                tbhide.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


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
                                                            ddl.DataSource = Session["SubVndMachineByProcGroup"];
                                                            ddl.DataTextField = "Machine";
                                                            ddl.DataValueField = "Machine";
                                                            ddl.DataBind();

                                                            string Qno = hdnQuoteNo.Value;
                                                            string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                            if (LastQno == "D")
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

                                                if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
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

                                                if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    ddl.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    ddl.Attributes.Add("disabled", "disabled");
                                }

                                tCell.Controls.Add(ddl);
                                tCell.Controls.Add(tb);

                                tCell.Controls.Add(ddlMachide);
                                tCell.Controls.Add(tbhide);
                            }
                            #endregion Machine
                            #region TextBox
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                                string txtID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                if (tb.ID.Contains("Name")) { }
                                else
                                {
                                    tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                                }

                                if (tb.ID.Contains("TotalProcessesCost/pcs") || tb.ID.Contains("ProcessUOM") || (tb.ID.ToString().ToLower().Contains("baseqty")))
                                {
                                    if (tb.ID.Contains("DurationperProcessUOM(Sec)"))
                                    {

                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                }

                                if (tb.ID.Contains("txtProcessUOM"))
                                {
                                    tb.Attributes.Add("onchange", "PrcGrpVsStrokesMin(" + (cellCtr - 1) + ")");
                                }

                                if (tb.ID.Contains("txtEfficiency/ProcessYield"))
                                {
                                }

                                if (tb.ID.Contains("txtTotalProcessesCost/pcs") || tb.ID.Contains("txtTurnkeyCost/pc") || tb.ID.Contains("txtTurnkeyProfit") || tb.ID.Contains("txtProcessCost/pc") || tb.ID.Contains("txtProcessUOM") || tb.ID.Contains("txtBaseqty") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") || tb.ID.Contains("txtStandardRate/HR"))
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
                                                            #region condition CYCLETIMEINSEC/SHOT
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
                                                                        string Qno = hdnQuoteNo.Value;
                                                                        string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                        if (LastQno == "D")
                                                                        {
                                                                            tb.Attributes.Remove("disabled");
                                                                        }
                                                                        else
                                                                        {
                                                                            tb.Attributes.Add("disabled", "disabled");
                                                                        }
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
                                                                    string Qno = hdnQuoteNo.Value;
                                                                    string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                    if (LastQno == "D")
                                                                    {
                                                                        tb.Attributes.Remove("disabled");
                                                                    }
                                                                    else
                                                                    {
                                                                        tb.Attributes.Add("disabled", "disabled");
                                                                    }
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
                                                tb.Text = tempPClist[ii].ToString().Replace("NaN", "");

                                                if (tempPClist[0].ToString().Replace("NaNSelect", "") == "")
                                                {
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }
                                                break;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                if (tb.ID.Contains("txtStandardRate/HR"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                {
                                    tb.Text = "";
                                }

                                tCell.Controls.Add(tb);
                            }
                            #endregion TextBox
                            tRow.Cells.Add(tCell);
                        }
                    }

                    if (rowcount % 2 == 0)
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                        tRow.BackColor = Color.White;
                    }
                    else
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                        tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                    }
                    rowcount++;
                }

                Session["TablePC"] = TablePC;
                #endregion FirsColumn
            }
            else
            {
                #region NextColumn
                TablePC = (Table)Session["TablePC"];

                int CellsCount = ColumnType;

                for (int i = 1; i < CellsCount; i++)
                {
                    var tempPclist = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var TempPclistNew = tempPclist;

                    if (CellsCount == 2 && tempPclist.Count <= (DtDynamicProcessFields.Rows.Count + 1))
                        TempPclistNew = TempPclistNew.Skip(((CellsCount - (i)) * (DtDynamicProcessFields.Rows.Count + 1))).ToList();
                    //else if (CellsCount ==  && tempSMClist.Count > 6)
                    //    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                    else if (CellsCount == 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 1))
                        //TempPclistNew = TempPclistNew.Skip(((CellsCount - (i + 1)) * DtDynamicProcessFields.Rows.Count)).ToList();
                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                    else if (CellsCount >= 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount == (i + 1))
                        //TempPclistNew = TempPclistNew.Skip(((CellsCount) * (DtDynamicProcessFields.Rows.Count))).ToList();
                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                    else if (i >= 1 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 1))
                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                    else
                    {
                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();
                    }


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

                            //Label lb = new Label();
                            //tCell.Controls.Add(lb);
                            //// lb.Text = "Material Cost";
                            //TablePC.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlProcess" + (i);
                                ddl.DataSource = Session["process"];
                                ddl.DataTextField = "Process_Grp_code";
                                ddl.DataValueField = "Process_Grp_code";
                                ddl.Attributes.Add("onchange", "ProcGrpChange(" + i + ")");
                                ddl.DataBind();
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));


                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {
                                            if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                            {
                                                // var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                                {
                                                    string zz = TempPclistNew[ii].ToString();
                                                    if (ii == rowcount - 1)
                                                    {
                                                        var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                        if (ddlcheck != null)
                                                        {
                                                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                            string DdlProcGrpSelected = ddl.SelectedItem.Text.ToString();
                                                            string[] ArrProcGrpSelected = DdlProcGrpSelected.Split('-');
                                                            string strddlSelitem = ArrProcGrpSelected[0].ToString();

                                                            MachineIdsbyProcess(strddlSelitem);
                                                        }
                                                        else
                                                        {
                                                            ddl.SelectedIndex = 0;
                                                        }

                                                        //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");

                                                        if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                        {
                                                            ddl.Attributes.Remove("disabled");
                                                        }
                                                        else
                                                        {
                                                            ddl.Attributes.Add("disabled", "disabled");
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                            else
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
                                ddl.DataSource = Session["PSGroupwithUOM"];
                                ddl.DataTextField = "SubProcessName";
                                ddl.DataValueField = "SubProcessName";
                                ddl.Attributes.Add("onchange", "SubProcgroupChnge(" + i + ")");
                                // ddl.da
                                ddl.DataBind();
                                // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged4;
                                ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));


                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {
                                            if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                            {
                                                // var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                                {
                                                    if (ii == rowcount - 1)
                                                    {
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

                                                }
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                        else
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }
                                else
                                {
                                    ddl.Attributes.Add("disabled", "disabled");
                                }

                                tCell.Controls.Add(ddl);
                                TablePC.Rows[cellCtr].Cells.Add(tCell);
                            }

                            else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlSubvendorname" + (i);
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

                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {
                                            if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                            {
                                                // var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                                {
                                                    string zz = TempPclistNew[ii].ToString();
                                                    if (ii == rowcount - 1)
                                                    {
                                                        if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                                        {
                                                            if (TempPclistNew[8].ToString().Contains("STROKES/MIN"))
                                                            {
                                                                ddl.SelectedIndex = 0;
                                                                ddl.Attributes.Add("disabled", "disabled");
                                                            }
                                                            else
                                                            {
                                                                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                                if (ddlcheck != null)
                                                                {
                                                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //ddl.Attributes.Add("disabled", "disabled");
                                                            ddl.SelectedIndex = 0;
                                                            ddl.Attributes.Add("disabled", "disabled");
                                                        }
                                                        //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");

                                                        if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                        {
                                                            ddl.Attributes.Add("disabled", "disabled");
                                                        }
                                                        break;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }

                                        }
                                        else
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
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
                                //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
                                ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
                                ddl.Attributes.Add("onchange", "DdlMachineLaborChange(" + i + ")");

                                DropDownList ddlhide = new DropDownList();
                                ddlhide.ID = "dynamicddlHideMachineLabor" + (i);
                                ddlhide.Style.Add("display", "none");
                                ddlhide.Attributes.Add("disabled", "disabled");
                                //ddlhide.Style.Add("width", "142px");

                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {
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

                                                        if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                        {
                                                            ddl.Attributes.Add("disabled", "disabled");
                                                        }
                                                        break;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                        else
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
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
                                //ddl.Style.Add("width", "142px");
                                ddl.DataSource = Session["MachineIDs"];
                                ddl.DataTextField = "MachineID";
                                ddl.DataValueField = "MachineID";
                                ddl.Attributes.Add("onchange", "MachineChange(" + i + ")");
                                // ddl.da
                                ddl.DataBind();
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

                                DropDownList ddlMachide = new DropDownList();
                                ddlMachide.ID = "ddlHideMachine" + (i);
                                //ddlMachide.Style.Add("width", "142px");
                                ddlMachide.Style.Add("display", "none");
                                ddlMachide.Attributes.Add("disabled", "disabled");


                                TextBox tbhide = new TextBox();
                                tbhide.ID = "txtHide" + (i);
                                tbhide.Style.Add("display", "none");
                                tbhide.Attributes.Add("disabled", "disabled");

                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                        ddl.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {
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
                                                                        ddl.DataSource = Session["SubVndMachineByProcGroup"];
                                                                        ddl.DataTextField = "Machine";
                                                                        ddl.DataValueField = "Machine";
                                                                        ddl.DataBind();

                                                                        string Qno = hdnQuoteNo.Value;
                                                                        string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                        if (LastQno == "D")
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

                                                }
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                        }
                                        else
                                        {
                                            ddl.Attributes.Add("disabled", "disabled");
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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                string txtID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (tb.ID.Contains("Name")) { }
                                else
                                {
                                    tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
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

                                if (tb.ID.Contains("TotalProcessesCost/pcs") || tb.ID.Contains("txtTurnkeyCost/pc") || tb.ID.Contains("txtTurnkeyProfit") || tb.ID.Contains("ProcessUOM") || (tb.ID.Contains("ProcessCost/pc")) || (tb.ID.ToString().ToLower().Contains("baseqty")))
                                {
                                    if (tb.ID.Contains("DurationperProcessUOM(Sec)"))
                                    {
                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                }

                                //if (tb.ID.Contains("txtStandardRate/HR"))
                                //{
                                //    grdMachinelisthidden.DataSource = Session["MachineListGrd"];
                                //    grdMachinelisthidden.DataBind();
                                //    int firsttimeload = 0;
                                //    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                //    {
                                //        if (firsttimeload == 0)
                                //        {
                                //            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                //            firsttimeload = 1;
                                //        }
                                //        tb.Attributes.Add("disabled", "disabled");
                                //        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                //        {
                                //        }
                                //    }
                                //}

                                //if (tb.ID.Contains("txtVendorRate"))
                                //{

                                //    int firsttimeload = 0;
                                //    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                //    {
                                //        if (firsttimeload == 0)
                                //        {
                                //            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                //            firsttimeload = 1;
                                //        }

                                //        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                //        {
                                //            tb.Attributes.Add("disabled", "disabled");
                                //        }
                                //        else
                                //        {
                                //            tb.Attributes.Remove("disabled");
                                //        }
                                //    }
                                //}


                                //if (tb.ID.Contains("txtProcessUOM"))
                                //{

                                //    int firsttimeload = 0;
                                //    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                //    {
                                //        if (grdProcessGrphidden.Rows[h].Cells[0].Text.Contains(txtprocs.Text + " -"))
                                //        {
                                //            if (firsttimeload == 0)
                                //            {
                                //                // tb.Text = grdProcessGrphidden.Rows[h].Cells[2].Text;
                                //                firsttimeload = 1;
                                //            }
                                //        }
                                //    }
                                //}

                                if (hdnProcessTableCount.Value != null && hdnProcessTableCount.Value != "")
                                {
                                    var colcount = int.Parse(hdnProcessTableCount.Value);

                                    if (colcount == 0)
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        if (colcount >= (i))
                                        {

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
                                                                string a = TempPclistNew[8].ToString().Trim().Replace(" ", "").ToUpper();

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
                                                                                string Qno = hdnQuoteNo.Value;
                                                                                string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                                if (LastQno == "D")
                                                                                {
                                                                                    tb.Attributes.Remove("disabled");
                                                                                }
                                                                                else
                                                                                {
                                                                                    tb.Attributes.Add("disabled", "disabled");
                                                                                }
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
                                                                            string Qno = hdnQuoteNo.Value;
                                                                            string LastQno = Qno.Substring(Qno.Length - 1, 1);
                                                                            if (LastQno == "D")
                                                                            {
                                                                                tb.Attributes.Remove("disabled");
                                                                            }
                                                                            else
                                                                            {
                                                                                tb.Attributes.Add("disabled", "disabled");
                                                                            }
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
                                                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");

                                                        if (TempPclistNew[0].ToString().Replace("NaNSelect", "") == "")
                                                        {
                                                            tb.Attributes.Add("disabled", "disabled");
                                                        }
                                                        break;
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                tb.Attributes.Add("disabled", "disabled");
                                            }

                                        }
                                        else
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }

                                if (tb.ID.Contains("txtStandardRate/HR"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                                if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
                                {
                                    tb.Text = "";
                                }
                            }
                        }
                        rowcount++;
                    }
                    #endregion Loop
                }

                Session["TablePc"] = TablePC;
                #endregion NextColumn
            }
        }

        /// <summary>
        /// Sub Material cost Dynamic Tables
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicSubMaterialDT(int ColumnType)
        {
            DataTable DtDynamicSubMaterialsFields = new DataTable();
            DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

            if (ColumnType == 0)
            {
                #region old code (condition with DtDynamicSubMaterialsDetails never used)
                //int rowcount = 0;
                //if (DtDynamicSubMaterialsDetails == null)
                //    DtDynamicSubMaterialsDetails = new DataTable();
                //if (DtDynamicSubMaterialsDetails.Rows.Count > 0)
                //{
                //    TableRow Hearderrow = new TableRow();

                //    TableSMC.Rows.Add(Hearderrow);
                //    for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
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
                //            int TtlField = DtDynamicSubMaterialsDetails.Rows.Count;
                //            BtnDel.ID = "BtnDelSubMatCost" + ColmNoSubMat;
                //            BtnDel.Text = "Delete";
                //            BtnDel.ForeColor = Color.Yellow;
                //            BtnDel.OnClientClick = "DelSubMatCost('" + ColmNoSubMat + "','" + TtlField + "'); return false;";

                //            tCell1.Controls.Add(BtnDel);
                //            Hearderrow.Cells.Add(tCell1);
                //            ColmNoSubMat++;
                //        }
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                //        Hearderrow.ForeColor = Color.White;
                //    }

                //    foreach (DataRow row in DtDynamicSubMaterialsFields.Rows)
                //    {
                //        TableRow tRow = new TableRow();
                //        TableSMC.Rows.Add(tRow);
                //        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
                //        {
                //            TableCell tCell = new TableCell();
                //            if (cellCtr == 0)
                //            {
                //                Label lb = new Label();
                //                tCell.Controls.Add(lb);
                //                tCell.Width = 240;
                //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
                //                tRow.Cells.Add(tCell);
                //            }
                //            else
                //            {
                //                // Data Store and Retrieve

                //                //if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                //                //{
                //                //    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                //                //    for (int ii = 1; ii < tempSMClist.Count; ii++)
                //                //    {

                //                //    }
                //                //}


                //                TextBox tb = new TextBox();
                //                tb.BorderStyle = BorderStyle.None;
                //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                //                tb.Attributes.Add("autocomplete", "off");
                //                tb.Style.Add("text-transform", "uppercase");
                //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                //                if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                //                {
                //                    tb.Attributes.Add("disabled", "disabled");
                //                }

                //                //TextBox tb = new TextBox();
                //                //tb.BorderStyle = BorderStyle.None;
                //                //tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                //                //tb.Attributes.Add("autocomplete", "off");
                //                //if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                //                //{
                //                //    tb.Attributes.Add("disabled", "disabled");
                //                //}

                //                tCell.Controls.Add(tb);
                //                //if (rowcount == 0)
                //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                //                //else if (rowcount == 1)
                //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                //                tRow.Cells.Add(tCell);
                //            }
                //        }

                //        if (rowcount % 2 == 0)
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                //        }
                //        rowcount++;
                //    }
                //}
                //else
                //{
                //    int rowcountnew = 0;

                //    TableRow Hearderrow = new TableRow();

                //    TableSMC.Rows.Add(Hearderrow);
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
                //            int TtlField = DtDynamicSubMaterialsFields.Rows.Count;
                //            BtnDel.ID = "BtnDelSubMatCost" + ColmNoSubMat;
                //            BtnDel.Text = "Delete";
                //            BtnDel.ForeColor = Color.Yellow;
                //            BtnDel.OnClientClick = "DelSubMatCost('" + ColmNoSubMat + "','" + TtlField + "'); return false;";

                //            tCell1.Controls.Add(BtnDel);
                //            Hearderrow.Cells.Add(tCell1);
                //            ColmNoSubMat++;
                //        }
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                //        Hearderrow.ForeColor = Color.White;
                //    }

                //    // Table1 = (Table)Session["Table"];
                //    for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
                //    {
                //        TableRow tRow = new TableRow();
                //        TableSMC.Rows.Add(tRow);

                //        for (int i = 0; i <= 1; i++)
                //        {
                //            TableCell tCell = new TableCell();
                //            if (i == 0)
                //            {
                //                Label lb = new Label();
                //                tCell.Controls.Add(lb);
                //                lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
                //                lb.Width = 240;
                //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                //            }
                //            else
                //            {
                //                TextBox tb = new TextBox();
                //                tb.BorderStyle = BorderStyle.None;
                //                tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                //                if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                //                {
                //                    tb.Attributes.Add("disabled", "disabled");
                //                }
                //                tb.Attributes.Add("autocomplete", "off");
                //                tb.Style.Add("text-transform", "uppercase");
                //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


                //                // Data Store and Retrieve
                //                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                //                {
                //                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                //                    {
                //                        if (ii == (cellCtr - 1))
                //                        {
                //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                //                            break;
                //                        }

                //                    }
                //                }

                //                if (tb.ID.Contains("txtTotalSub-Mat/T&JCost/pcs"))
                //                {
                //                    tb.Text = "";
                //                }

                //                tCell.Controls.Add(tb);
                //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                //            }
                //        }
                //        if (rowcountnew % 2 == 0)
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                //        }
                //        rowcountnew++;
                //    }
                //}
                #endregion

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

                        tCell1.Controls.Add(BtnDel);
                        Hearderrow.Cells.Add(tCell1);
                        ColmNoSubMat++;
                    }
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                            lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
                            lb.Width = 240;
                            TableSMC.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            string txtID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                            if (tb.ID.Contains("Description")) { }
                            else
                            {
                                tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                            }

                            if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
                            {
                                tb.Attributes.Add("disabled", "disabled");
                            }
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Style.Add("text-transform", "uppercase");



                            // Data Store and Retrieve
                            if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                            {
                                var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                                for (int ii = 0; ii < tempSMClist.Count; ii++)
                                {
                                    if (ii == (cellCtr - 1))
                                    {
                                        tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                        break;
                                    }

                                }
                            }

                            if (tb.ID.Contains("txtTotalSub-Mat/T&JCost/pcs"))
                            {
                                tb.Text = "";
                            }

                            tCell.Controls.Add(tb);
                            TableSMC.Rows[cellCtr].Cells.Add(tCell);
                        }
                    }
                    if (rowcountnew % 2 == 0)
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                        tRow.BackColor = Color.White;
                    }
                    else
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                        tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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

                    //if (CellsCount == 2 && tempSMClist.Count <= (DtDynamicSubMaterialsFields.Rows.Count + 1))
                    //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i)) * (DtDynamicSubMaterialsFields.Rows.Count + 1))).ToList();
                    ////else if (CellsCount ==  && tempSMClist.Count > 6)
                    ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                    //else if (CellsCount == 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount != (i + 1))
                    //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamicSubMaterialsFields.Rows.Count)).ToList();
                    //else if (CellsCount >= 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount == (i + 1))
                    //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();
                    //else if (i >= 1 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount != (i + 1))
                    //    TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();

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
                            tCell.Controls.Add(BtnDel);
                            TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            ColmNoSubMat++;

                            //Label lb = new Label();
                            //tCell.Controls.Add(lb);
                            //// lb.Text = "Material Cost";
                            //TableSMC.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Style.Add("text-transform", "uppercase");

                            string txtID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            if (tb.ID.Contains("Description")) { }
                            else
                            {
                                tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
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

                            // Data Retrieve and assign from Storage

                            //if (CellsCount >= 2)
                            //{
                            //    var exactval = ((CellsCount - 1) * tempSMClist.Count);

                            //    var exactDivide = (exactval / (tempSMClist.Count));

                            //    if (exactDivide >= CellsCount)
                            //    {

                            if (hdnSMCTableCount.Value != null && hdnSMCTableCount.Value != "")
                            {
                                var colcount = int.Parse(hdnSMCTableCount.Value);

                                if (colcount == 0)
                                {

                                }
                                else
                                {
                                    if (colcount >= (i))
                                    {
                                        if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                        {

                                            for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                            {

                                                if (ii == (cellCtr - 1))
                                                {
                                                    if (tb.ID.Contains("txtTotalSub-Mat/T&JCost/pcs"))
                                                    {
                                                        tb.Text = "";
                                                    }
                                                    else
                                                    {
                                                        tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                                    }
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                }
                            }

                            //if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                            //{

                            //    for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                            //    {

                            //        if (ii == (cellCtr - 1))
                            //        {

                            //            tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                            //            break;
                            //        }

                            //    }
                            //}
                            //  }
                            // }


                            //if (hdngrdSubMatCost.Rows.Count > 1)
                            //{

                            //    for (int i1 = 0; i1 < hdngrdSubMatCost.Rows.Count; i1++)
                            //    {
                            //        for (int j = 0; j < hdngrdSubMatCost.Rows[i1].Cells.Count; j++)
                            //        {
                            //            if (j == cellCtr)
                            //            {
                            //                tb.Text = hdngrdSubMatCost.Rows[i1].Cells[j].Text;
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }

                Session["TableSMC"] = TableSMC;
            }

            //  var Ss = divhdnSMC.InnerHtml;






        }

        /// <summary>
        /// Other Cost Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicOthersCostDT(int ColumnType)
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
                //            int TtlField = DtDynamicOtherCostsFields.Rows.Count + 1;
                //            BtnDel.ID = "BtnDelOthCost" + ColmNoOth;
                //            BtnDel.Text = "Delete";
                //            BtnDel.ForeColor = Color.Yellow;
                //            BtnDel.OnClientClick = "DelOthCost('" + ColmNoOth + "','" + TtlField + "');return false;";
                //            tCell1.Controls.Add(BtnDel);
                //            ColmNoOth++;
                //        }
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                //                tCell.Text = row.ItemArray[0].ToString();
                //                tCell.Width = 240;
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
                //                                tb.Text = "";
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
                //                //if (rowcount == 0)
                //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                //                //else if (rowcount == 1)
                //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                //                tRow.Cells.Add(tCell);
                //            }
                //        }

                //        if (rowcount % 2 == 0)
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                //                if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                //                {

                //                    tb.Attributes.Add("disabled", "disabled");
                //                }
                //                if (tb.ID.Contains("Unit"))
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
                //                                tb.Text = "";
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
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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
                    }
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                            lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
                            lb.Width = 240;
                            TableOthers.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            string txtID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                            if (tb.ID.Contains("Description")) { }
                            else
                            {
                                tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                            }

                            if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                            {
                                tb.Attributes.Add("disabled", "disabled");
                            }
                            if (tb.ID.Contains("Unit"))
                            {
                                tb.Attributes.Add("disabled", "disabled");
                            }


                            // Data Store and Retrieve
                            if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                            {
                                var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                for (int ii = 0; ii < tempSMClist.Count; ii++)
                                {
                                    if (ii == (cellCtr - 1))
                                    {
                                        if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                        {
                                            tb.Text = "";
                                        }
                                        else
                                        {
                                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                        }
                                        break;
                                    }

                                }
                            }


                            tCell.Controls.Add(tb);
                            TableOthers.Rows[cellCtr].Cells.Add(tCell);
                        }
                    }
                    if (rowcountnew % 2 == 0)
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                        tRow.BackColor = Color.White;
                    }
                    else
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                        tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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
                    //else if (CellsCount == 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 1))
                    //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i + 1)) * DtDynamicOtherCostsFields.Rows.Count)).ToList();
                    //else if (CellsCount >= 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount == (i + 1))
                    //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();
                    //else if (i >= 1 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 1))
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

                            //Label lb = new Label();
                            //tCell.Controls.Add(lb);
                            //// lb.Text = "Material Cost";
                            //TableOthers.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                            string txtID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            if (tb.ID.Contains("Description")) { }
                            else
                            {
                                tb.Attributes.Add("oninput", "validateNumber('" + txtID + "')");
                            }

                            if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                            {
                                TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
                                tb.Attributes.Add("disabled", "disabled");
                            }

                            else
                            {
                                tCell.Controls.Add(tb);
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }

                            if (tb.ID.Contains("Unit"))
                            {
                                tb.Attributes.Add("disabled", "disabled");
                            }


                            if (hdnOthersTableCount.Value != null && hdnOthersTableCount.Value != "")
                            {
                                var colcount = int.Parse(hdnOthersTableCount.Value);

                                if (colcount == 0)
                                {

                                }
                                else
                                {
                                    if (colcount >= (i))
                                    {
                                        // Data Retrieve and assign from Storage
                                        if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                        {
                                            for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                            {
                                                if (ii == (cellCtr - 1))
                                                {

                                                    if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                                    {
                                                        tb.Text = "";
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
                    }
                }
                Session["TableOthers"] = TableOthers;
            }

            #region  Not using -- For dynamic  grid 

            //if(!IsPostBack)
            //{
            //    DataTable dt = new DataTable();
            //    DataRow NewRow = dt.NewRow();
            //    if (dt.Columns.Count == 0)
            //    {
            //        dt.Columns.Add("RowCount", typeof(string));
            //        dt.Columns.Add("ItemsDescription", typeof(string));
            //        dt.Columns.Add("OtherItemCost", typeof(string));
            //        dt.Columns.Add("TotalOtherItemCost", typeof(string));
            //    }
            //    for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
            //    {

            //        if (cellCtr == 0)
            //        {
            //            NewRow[0] = cellCtr;
            //        }
            //        if (cellCtr == 1)hdngrdOCTable
            //        {
            //            NewRow[1] = "NewTemp1";
            //        }
            //        if (cellCtr == 2)
            //        {
            //            NewRow[2] =  "0";
            //        }
            //        if (cellCtr == 3)
            //        {
            //            NewRow[3] = "0";
            //        }


            //        // dtOC.Rows.Add(drOC);
            //    }
            //    dt.Rows.Add(NewRow);
            //    hdngrdOCTable.DataSource = dt;
            //    hdngrdOCTable.DataBind();
            //}
            #endregion -- not using

            #region Othervalues Save check
            //if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
            //{
            //    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

            //    var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

            //    for (int i = 0; i < ccc; i++)
            //    {

            //        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

            //        var tempOtherlistNew = tempOtherlist;


            //        tempOtherlistNew = tempOtherlistNew.Skip(((i) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();

            //        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            //        SqlConnection conins;
            //        conins = new SqlConnection(connetionStringdate);
            //        conins.Open();

            //        string query = "insert into [dbo].[TOtherCostDetails] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

            //        string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
            //        string ICost = tempOtherlistNew[1].ToString();
            //        string ItotalCost = tempOtherlistNew[2].ToString();


            //        SqlCommand cmd1 = new SqlCommand(query, conins);
            //        cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
            //        cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
            //        cmd1.Parameters.AddWithValue("@Desc", IDesc);
            //        cmd1.Parameters.AddWithValue("@Cost", ICost);
            //        cmd1.Parameters.AddWithValue("@TotalCost", ItotalCost);
            //        cmd1.Parameters.AddWithValue("@RowId", i);
            //        cmd1.Parameters.AddWithValue("@By", UserId);
            //        cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));


            //        cmd1.CommandText = query;
            //        cmd1.ExecuteNonQuery();
            //    }
            //}
            #endregion other values Save check
        }


        /// <summary>
        /// Unit Price Dynamic Table
        /// </summary>
        /// <param name="ColumnType"></param>
        private void CreateDynamicUnitDT(int ColumnType)
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
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                //                tCell.Width = 240;
                //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs -", "/pc");
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
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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
                //        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                //            tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                //            tRow.BackColor = Color.White;
                //        }
                //        else
                //        {
                //            tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                //            tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
                //        }
                //        rowcountnew++;
                //    }
                //}
                #endregion
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
                        tCell1.Text = "Final Quote Price/pc";
                    }
                    else if (cellCtr == 5)
                    {
                        tCell1.Text = "Net Profit/Discount";
                    }
                    Hearderrow.Cells.Add(tCell1);
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                                    lb.Text = "Total Process Cost/pc";
                                }
                                else
                                {
                                    lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("disabled", "disabled");
                                //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                tb.Attributes.Add("oninput", "validateNumber('txtProfit(%)0')");
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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                tb.Attributes.Add("oninput", "validateNumber('txtDiscount(%)0')");
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
                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("disabled", "disabled");
                            //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

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
                                tb.Attributes.Add("disabled", "disabled");
                                tb.Style.Add("text-transform", "uppercase");
                                //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
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
                        tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                        tRow.BackColor = Color.White;
                    }
                    else
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                        tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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
                        tCell.Width = 240;
                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    }
                    else
                    {
                        TextBox tb = new TextBox();
                        tb.BorderStyle = BorderStyle.None;
                        tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                        tb.Attributes.Add("autocomplete", "off");
                        tb.Style.Add("text-transform", "uppercase");
                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                        tCell.Controls.Add(tb);
                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    }
                }


                Session["TableUnit"] = TableUnit;
            }

        }

        private void CreateDynamicUnitDTTmShimano(int ColumnType)
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
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                                    lb.Text = "Total Process Cost/pc";
                                }
                                else
                                {
                                    lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("disabled", "disabled");
                                //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                tb.Attributes.Add("oninput", "validateNumber('txtProfit(%)0')");
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
                                tb.Style.Add("text-transform", "uppercase");
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                //tb.Attributes.Add("onkeydown", "return isNumberKey(event);");
                                tb.Attributes.Add("oninput", "validateNumber('txtDiscount(%)0')");
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
                            tb.Style.Add("text-transform", "uppercase");
                            tb.Attributes.Add("disabled", "disabled");
                            //tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
                        }
                    }



                    if (rowcountnew % 2 == 0)
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#284775");
                        tRow.BackColor = Color.White;
                    }
                    else
                    {
                        tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                        tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
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

        #endregion

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
            // plant = "2100";
            // Product = "BO";
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select distinct ProcessGrpCode from TPROCESGROUP_SUBPROCESS Where ProcessGrpCode = '" + ProcessGrp + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtProGrp);
                //DtDynamicProcessCostsDetails = dtProGrp;
                Session["DtDynamicProcessCostsDetails"] = dtProGrp;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }
        }


        private void GetProcessDetailsbyQuoteDetailsWithNoGroup()
        {
            // plant = "2100";
            // Product = "BO";
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            try
            {
                consap.Open();

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select CONCAT(ProcessGrpCode , ' - ', ProcessGrpDescription) as ProcessGrpCode, SubProcessName,ProcessUomDescription,ProcessUOM,ProcessGrpCode from TPROCESGROUP_SUBPROCESS where DELFLAG = 0 ";
                da = new SqlDataAdapter(str, consap);
                da.Fill(dtProGrp);

                grdProcessGrphidden.DataSource = dtProGrp;
                grdProcessGrphidden.DataBind();
                Session["PSGroupwithUOM"] = grdProcessGrphidden.DataSource;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
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

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            try
            {
                con.Open();

                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;


                strGetData = "select CONVERT(VARCHAR(10), S.RequestDate, 103) as 'RequestDate',S.QuoteNo,V.Description,v.Crcy,vp.PICName,vp.PICemail from tVendor_New as V inner join TVENDORPIC as VP" +
                              " on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + "TQuoteDetails as S on S.VendorCode1=v.Vendor where S.QuoteNo='" + reqno + "' and S.VendorCode1= '" + Session["mappedVendor"].ToString() + "' and V.DELFLAG = 0 ";
                da = new SqlDataAdapter(strGetData, con);
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
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        private void ChcExistDraft()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection conins;
            conins = new SqlConnection(connetionStringdate);
            string sql;
            string sqlDel;

            conins.Open();

            SqlCommand cmd;
            SqlDataReader reader;
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                sqlDel = "";
                sql = "select * from TOtherCostDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, conins);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TOtherCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Close();

                sql = "select * from TSMCCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, conins);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TSMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Close();

                sql = "select * from TProcessCostDetails_D where QuoteNo = @QuoteNo";
                cmd = new SqlCommand(sql, conins);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TProcessCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Close();

                sql = " select * from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, conins);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " delete from TMCCostDetails_D where QuoteNo = @QuoteNo ";
                }
                reader.Close();

                if (sqlDel != "")
                {
                    cmd = new SqlCommand(sqlDel, conins);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
            finally
            {
                conins.Close();
            }
        }

        private void InsertQuoteDetailDraft()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection conins;
            conins = new SqlConnection(connetionStringdate);
            string sql;
            string sqlDel;

            conins.Open();

            SqlCommand cmd;
            SqlDataReader reader;
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                sqlDel = "";
                sql = " select * from TQuoteDetails_D where QuoteNo = @QuoteNo ";
                cmd = new SqlCommand(sql, conins);
                cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    sqlDel += " ";
                }
                else
                {
                    sqlDel += " insert into TQuoteDetails_D select* from TQuoteDetails where QuoteNo = @QuoteNo ";
                }
                reader.Close();

                if (sqlDel != "")
                {
                    cmd = new SqlCommand(sqlDel, conins);
                    cmd.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
            finally
            {
                conins.Close();
            }
        }

        private void SaveallCostDetailsDraft()
        {
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

                    if (tempOtherlistNew[2].ToString() == "" || tempOtherlistNew[2].ToString() == null)
                    { }
                    else
                    {
                        string query = "insert into [dbo].[TOtherCostDetails_D] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

                        string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempOtherlistNew[1].ToString();
                        string ItotalCost = tempOtherlistNew[2].ToString();

                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();
                        //Userid = Session["Userid"].ToString();
                        //userId = Session["userID_"].ToString();
                        SqlCommand cmd1 = new SqlCommand(query, conins);
                        cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                        cmd1.Parameters.AddWithValue("@Desc", IDesc);
                        cmd1.Parameters.AddWithValue("@Cost", ICost);
                        cmd1.Parameters.AddWithValue("@TotalCost", ItotalCost);
                        cmd1.Parameters.AddWithValue("@RowId", i);
                        cmd1.Parameters.AddWithValue("@By", Session["userID_"].ToString());
                        cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        cmd1.CommandText = query;
                        cmd1.ExecuteNonQuery();
                        conins.Close();
                    }

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

                    if (tempSMClistNew[3].ToString() == "" || tempSMClistNew[3].ToString() == null)
                    { }
                    else
                    {

                        string IDesc = tempSMClistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempSMClistNew[1].ToString();
                        string IConsumption = tempSMClistNew[2].ToString();
                        string ICostpcs = tempSMClistNew[3].ToString();
                        string ItotalCost = tempSMClistNew[4].ToString();


                        if (ICostpcs == null || ICostpcs == "")
                        {

                        }
                        else
                        {

                            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            SqlConnection conins;
                            conins = new SqlConnection(connetionStringdate);
                            conins.Open();
                            string UserId = Session["userID_"].ToString();
                            string query = "insert into [dbo].[TSMCCostDetails_D] (QuoteNo,ProcessGroup,[Sub-Mat/T&JDescription],[Sub-Mat/T&JCost],[Consumption(pcs)],[Sub-Mat/T&JCost/pcs],[TotalSub-Mat/T&JCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@IConsumption,@ICostpcs,@ItotalCost,@RowId,@By,@On)";
                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                            cmd1.Parameters.AddWithValue("@Desc", IDesc);
                            cmd1.Parameters.AddWithValue("@Cost", ICost);
                            cmd1.Parameters.AddWithValue("@IConsumption", IConsumption);
                            cmd1.Parameters.AddWithValue("@ICostpcs", ICostpcs);
                            cmd1.Parameters.AddWithValue("@ItotalCost", ItotalCost);
                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
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

                    if (tempProcesslistNew[14].ToString() == "" || tempProcesslistNew[14].ToString() == null)
                    { }
                    else
                    {

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

                        if (ICostPc == null || ICostPc == "") { }
                        else
                        {
                            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            SqlConnection conins;
                            conins = new SqlConnection(connetionStringdate);
                            conins.Open();

                            string query = "insert into [dbo].[TProcessCostDetails_D] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn,TurnKeySubVnd,TurnKeyCost,TurnKeyProfit) "
                                + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On,@ITurnKeySubVnd,@ITurnKeyCost,@ITurnKeyProfit)";
                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@IProc", IProc);
                            cmd1.Parameters.AddWithValue("@ISubProc", ISubProc);
                            cmd1.Parameters.AddWithValue("@ITurnKey", ITurnKey);
                            cmd1.Parameters.AddWithValue("@ITurnKeySubVnd", ITurnKeySubVnd);
                            cmd1.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
                            cmd1.Parameters.AddWithValue("@IMachine", IMachine);
                            cmd1.Parameters.AddWithValue("@IStRate", IStRate);
                            cmd1.Parameters.AddWithValue("@IVendorRate", IVendorRate);
                            cmd1.Parameters.AddWithValue("@IProcUOM", IProcUOM);
                            cmd1.Parameters.AddWithValue("@IBaseQty", IBaseQty);
                            cmd1.Parameters.AddWithValue("@IDPUOM", IDPUOM);
                            cmd1.Parameters.AddWithValue("@Iyield", Iyield);
                            cmd1.Parameters.AddWithValue("@ITurnKeyCost", ITurnKeyCost);
                            cmd1.Parameters.AddWithValue("@ITurnKeyProfit", ITurnKeyProfit);
                            cmd1.Parameters.AddWithValue("@ICostPc", ICostPc);
                            cmd1.Parameters.AddWithValue("@ITotalCost", ITotalCost);
                            //UserId = Session["userID_"].ToString();
                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", Session["userID_"].ToString());
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
                }
            }

            #endregion Process Values

            #region Process Values old
            //if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
            //{
            //    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

            //    var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

            //    for (int i = 1; i <= ccc; i++)
            //    {
            //        var tempProcesslist = hdnProcessValues.Value.ToString().Split(',').ToList();

            //        var tempProcesslistNew = tempProcesslist;
            //        if (i > 1)
            //            tempProcesslistNew = tempProcesslistNew.Skip(((i - 1) * (DtDynamicProcessFields.Rows.Count))).ToList();

            //        if (tempProcesslistNew[11].ToString() == "" || tempProcesslistNew[11].ToString() == null || double.Parse(tempProcesslistNew[11].ToString()) == 0)
            //        { }
            //        else
            //        {

            //            string IProc = tempProcesslistNew[0].ToString().Replace("NaN", "");
            //            string ISubProc = tempProcesslistNew[1].ToString();
            //            string ITurnKey = tempProcesslistNew[2].ToString();
            //            string IMachineLabor = tempProcesslistNew[3].ToString();
            //            string IMachine = tempProcesslistNew[4].ToString();
            //            string IStRate = tempProcesslistNew[5].ToString().Replace("NaN", "");
            //            string IVendorRate = tempProcesslistNew[6].ToString();
            //            string IProcUOM = tempProcesslistNew[7].ToString();
            //            string IBaseQty = tempProcesslistNew[8].ToString();
            //            string IDPUOM = tempProcesslistNew[9].ToString();
            //            string Iyield = tempProcesslistNew[10].ToString();
            //            string ICostPc = tempProcesslistNew[11].ToString();
            //            string ITotalCost = tempProcesslistNew[12].ToString();

            //            if (ICostPc == null || ICostPc == "") { }
            //            else
            //            {

            //                var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            //                SqlConnection conins;
            //                conins = new SqlConnection(connetionStringdate);
            //                conins.Open();

            //                string query = "insert into [dbo].[TProcessCostDetails_D] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn) "
            //                    + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On)";

            //                SqlCommand cmd1 = new SqlCommand(query, conins);
            //                cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
            //                cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

            //                cmd1.Parameters.AddWithValue("@IProc", IProc);
            //                cmd1.Parameters.AddWithValue("@ISubProc", ISubProc);
            //                cmd1.Parameters.AddWithValue("@ITurnKey", ITurnKey);
            //                cmd1.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
            //                cmd1.Parameters.AddWithValue("@IMachine", IMachine);
            //                cmd1.Parameters.AddWithValue("@IStRate", IStRate);
            //                cmd1.Parameters.AddWithValue("@IVendorRate", IVendorRate);
            //                cmd1.Parameters.AddWithValue("@IProcUOM", IProcUOM);
            //                cmd1.Parameters.AddWithValue("@IBaseQty", IBaseQty);
            //                cmd1.Parameters.AddWithValue("@IDPUOM", IDPUOM);
            //                cmd1.Parameters.AddWithValue("@Iyield", Iyield);
            //                cmd1.Parameters.AddWithValue("@ICostPc", ICostPc);
            //                cmd1.Parameters.AddWithValue("@ITotalCost", ITotalCost);

            //                cmd1.Parameters.AddWithValue("@RowId", i);
            //                cmd1.Parameters.AddWithValue("@By", UserId);
            //                cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            //                cmd1.CommandText = query;
            //                cmd1.ExecuteNonQuery();
            //                conins.Close();
            //            }
            //        }
            //    }
            //}

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

                var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                SqlConnection conins;
                conins = new SqlConnection(connetionStringdate);

                for (int i = 1; i <= ccc; i++)
                {
                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var tempMClistNew = tempMClist;
                    if (i > 1)
                    {
                        tempMClistNew = tempMClistNew.Skip(((i - 1) * (DtDynamic.Rows.Count))).ToList();

                        conins.Open();

                        #region Common for All
                        strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                        strMDesc = tempMClistNew[1].ToString();
                        strRawCost = tempMClistNew[2].ToString();
                        strTotalRawCost = tempMClistNew[3].ToString();
                        strPartUnitW = tempMClistNew[4].ToString();
                        #endregion Common for all

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = tempMClistNew[5].ToString();
                            strRunnerRatio = tempMClistNew[6].ToString();
                            strRecycle = tempMClistNew[7].ToString();

                            strCavity = tempMClistNew[8].ToString();
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
                            //strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                            //strMDesc = tempMClistNew[1].ToString();
                            //strRawCost = tempMClistNew[2].ToString();
                            //strTotalRawCost = tempMClistNew[3].ToString();
                            //strPartUnitW = tempMClistNew[4].ToString();

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

                        if (strMCostpcs == null || strMCostpcs == "")
                        {

                        }
                        else
                        {

                            string query = "insert into [dbo].[TMCCostDetails_D] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On)";

                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd1.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd1.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd1.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd1.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd1.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd1.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd1.Parameters.AddWithValue("@strThick", strThick);
                            cmd1.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd1.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd1.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd1.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd1.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd1.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd1.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd1.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd1.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd1.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd1.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd1.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd1.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd1.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd1.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);


                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", Session["userID_"].ToString());
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
                    else
                    {
                        conins.Open();

                        #region Common for All
                        strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                        strMDesc = tempMClistNew[1].ToString();
                        strRawCost = tempMClistNew[2].ToString();
                        strTotalRawCost = tempMClistNew[3].ToString();
                        strPartUnitW = tempMClistNew[4].ToString();
                        #endregion Common for all

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = tempMClistNew[5].ToString();
                            strRunnerRatio = tempMClistNew[6].ToString();
                            strRecycle = tempMClistNew[7].ToString();

                            strCavity = tempMClistNew[8].ToString();
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

                        if (strMCostpcs == null || strMCostpcs == "")
                        {

                        }
                        else
                        {

                            string query = "insert into [dbo].[TMCCostDetails_D] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On)";

                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd1.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd1.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd1.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd1.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd1.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd1.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd1.Parameters.AddWithValue("@strThick", strThick);
                            cmd1.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd1.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd1.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd1.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd1.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd1.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd1.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd1.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd1.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd1.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd1.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd1.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd1.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd1.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd1.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);

                            string UserId = Session["userID_"].ToString();
                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
                }
            }

            #endregion Process Values

        }

        private void updateQuoteDetDraft()
        {
            try
            {
                string TTMatCost = hdnTMatCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTMatCost.Value.ToString();
                string TSumMatCost = hdnTSumMatCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTSumMatCost.Value.ToString();
                string TProCost = hdnTProCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTProCost.Value.ToString();
                string TTOtherCost = hdnTOtherCost.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTOtherCost.Value.ToString();
                string TGTotal = hdnTGTotal.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTGTotal.Value.ToString();
                string TFinalQPrice = hdnTFinalQPrice.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnTFinalQPrice.Value.ToString();
                string TProfit = hdnProfit.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnProfit.Value.ToString();
                string TDiscount = hdnDiscount.Value.ToString() == "NaN" ? DBNull.Value.ToString() : hdnDiscount.Value.ToString();
                DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);
                DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd/MM/yyyy", null);

                var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                SqlConnection conins;
                conins = new SqlConnection(connetionStringdate);
                conins.Open();

                string query1 = "update " + TransDB.ToString() + "TQuoteDetails_D set TotalMaterialCost=@TotalMaterialCost, TotalSubMaterialCost=@TotalSubMaterialCost, TotalProcessCost=@TotalProcessCost, TotalOtheritemsCost=@TotalOtheritemsCost, GrandTotalCost=@GrandTotalCost,FinalQuotePrice = @FinalQuotePrice,Profit=@Profit,Discount=@Discount,EffectiveDate = @EffectiveDate, DueOn=@DueOn,CommentByVendor=@CommentByVendor, UpdatedBy=@UpdatedBy , UpdatedOn=CURRENT_TIMESTAMP, countryorg=@countryorg Where QuoteNo =@QuoteNo";

                //    string text = "Data Saved!";
                SqlCommand cmd1 = new SqlCommand(query1, conins);
                cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                cmd1.Parameters.AddWithValue("@TotalMaterialCost", TTMatCost);
                cmd1.Parameters.AddWithValue("@TotalSubMaterialCost", TSumMatCost);
                cmd1.Parameters.AddWithValue("@TotalProcessCost", TProCost);
                cmd1.Parameters.AddWithValue("@TotalOtheritemsCost", TTOtherCost);
                cmd1.Parameters.AddWithValue("@GrandTotalCost", TGTotal);
                cmd1.Parameters.AddWithValue("@FinalQuotePrice", TFinalQPrice);
                cmd1.Parameters.AddWithValue("@Profit", TProfit);
                cmd1.Parameters.AddWithValue("@Discount", TDiscount);
                cmd1.Parameters.AddWithValue("@CommentByVendor", TxtComntByVendor.Text);
                cmd1.Parameters.AddWithValue("@EffectiveDate", newEffDate.ToString("yyyy-MM-dd"));
                cmd1.Parameters.AddWithValue("@DueOn", newDueon.ToString("yyyy-MM-dd"));
                cmd1.Parameters.AddWithValue("@UpdatedBy", Session["userID_"].ToString());
                cmd1.Parameters.AddWithValue("@countryorg", ddlpirjtype.SelectedValue.ToString());
                cmd1.CommandText = query1;
                cmd1.ExecuteNonQuery();
                conins.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        private void GetSHMNPICDetails(string userdet)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);

            try
            {
                consap.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                // string str = "select  sp.PIC1Name as UseNam,sp.PIC1Email as UseEmail from Usr Inner join TSMNProductPIC sp on Usr.UseID = '" + userdet.Trim() + "'";
                string str1 = "select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product from TSMNProductPIC pp inner join " + TransDB.ToString() + "TQuotedetails TQ on pp.Product = TQ.Product and pp.Userid = Tq.CreatedBy and PP.Product = TQ.Product where QuoteNo='" + Session["Qno"].ToString() + "' and pp.DELFLAG = 0 ";
                da = new SqlDataAdapter(str1, consap);
                da.Fill(dtdate);
                if (dtdate.Rows.Count > 0)
                {
                    txtsmnpic.Text = dtdate.Rows[0]["PICName"].ToString();
                    txtemail.Text = dtdate.Rows[0]["PICEmail"].ToString();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                consap.Close();
            }

        }

        #region UnWanted Methods
        protected void grdmatlcost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    for (int i = 0; i < e.Row.Cells.Count; i++)
            //    {
            //        if (i != 0)
            //        {

            //            TextBox txt = new TextBox();
            //            //txt.Text = e.Row.Cells[i].Text;
            //            //e.Row.Cells[i].Text = "";
            //            e.Row.Cells[i].Controls.Add(txt);
            //            if (e.Row.Cells[0].Text == "Material SAP Code")
            //            {
            //                // var Keyvalue = DicQuoteDetails.Single(x => x.Key == e.Row.Cells[0].Text);
            //                txt.Text = DicQuoteDetails.Single().Key;
            //                txt.Enabled = false;
            //            }
            //            if (e.Row.Cells[0].Text.ToString().TrimEnd() == "Material Description")
            //            {
            //                // var Keyvalue = DicQuoteDetails.Single(x => x.Key == e.Row.Cells[0].Text);
            //                txt.Text = DicQuoteDetails.Single().Value;
            //                txt.Enabled = false;
            //            }

            //        }
            //    }
            //}
        }

        protected void getmaterial()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                DataTable Result = new DataTable();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                //string str = "select RawMaterial,Material,UnitWeight from TMATERIAL where material='10071001117'";
                //da = new SqlDataAdapter(str, con);
                //Result = new DataTable();
                //da.Fill(dt);




                //dt.Columns.AddRange(new DataColumn[16] { new DataColumn("S.No"), new DataColumn("materialType"), new DataColumn("materialcode"), new DataColumn("matlgrade"), new DataColumn("rawmatlcost"), new DataColumn("totrawcost"), new DataColumn("matlgrsweight"), new DataColumn("matlcost"), new DataColumn("totmatlcost"), new DataColumn("runrweight"), new DataColumn("runrratio"), new DataColumn("recyclematlratio"), new DataColumn("scrapweight"), new DataColumn("scraplosallow"), new DataColumn("scrappric"), new DataColumn("scraprebate") });

                dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Material code"), new DataColumn("Matl Description"), new DataColumn("Raw matl cost/Kg(SGD) "), new DataColumn("Total Raw Matl cost/KG (SGD)"), new DataColumn("Part Net Weight"), new DataColumn("Cavity"), new DataColumn("Runner Weight/shot(gm)"), new DataColumn("Runner Ratio/Pcs(%)"), new DataColumn("Recycle matl ratio"), new DataColumn("Matl Yeild/Melting loss (%)"), new DataColumn("Matl Gross Weight(gm)"), new DataColumn("Matl cost /pcs"), new DataColumn("Total matl Cost/Pcs(SGD)") });
                ViewState["Vendors_req"] = dt;

                DataRow dr = dt.NewRow();


                //dr[0] = "80000449";
                //dr[1] = "AC4C";
                //dr[2] = "3.3766";
                //dr[3] = "AMILAN";

                //dr[4] = "3.3766";

                //dr[5] = "32.25";
                //dr[6] = "0.0829";

                //dr[7] = "0.2916";
                ////   dr[9] = "6";
                //dr[8] = "8";
                //dr[9] = "33";

                dt.Rows.Add(dr);

                //mygrid.Columns[2].Visible = false;

                if (dt.Rows.Count > 0)
                {
                    //  this.Convertmatlcost();

                    this.BindGridIM();


                }
                else
                {
                    //   this.Convertmatlcost();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
            }
            con.Close();

        }

        protected void getprocconnection()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                //    SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[12] { new DataColumn("Process grp Code"), new DataColumn("Sub Process"), new DataColumn("If Turnkey - Vendorname?"), new DataColumn("Machine/labour"), new DataColumn("Machine"), new DataColumn("Rate/Hr"), new DataColumn("Process Uom Description"), new DataColumn("Base Qty"), new DataColumn("Duration per proess UOM(sec)"), new DataColumn("Efficincy per Process yeild(%)"), new DataColumn("Process Cost/pc"), new DataColumn("Total Process Cost/pcs") });
                // ViewState["process"] = dt;
                DataRow dr = dt.NewRow();

                //dr[0] = "Injection Modeling";
                //dr[1] = "ROH";
                //dr[2] = "80000449";
                //dr[3] = "PLASTIC";

                //dr[4] = "AMILAN";



                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    //  grdproces.DataSource = dt;

                    this.Convert();

                    //   BindGrid(dt, true);

                    // grdproces.ShowHeader = !rotate;

                    // grdproces.DataBind();
                }
                else
                {
                    // grdproces.DataSource = dt;
                    this.Convert();

                    //grdproces.ShowHeader = !rotate;
                    //grdproces.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void Convert()
        {
            DataTable dt = (DataTable)ViewState["process"];

            //btnConvert1.Visible = false;
            //btnConvert2.Visible = true;
            DataTable dt2 = new DataTable();
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                dt2.Columns.Add("", typeof(string));
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i][0] = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt2.Rows[i][j + 1] = dt.Rows[j][i];
                }
            }
            BindGrid(dt2, true);
        }

        private void BindGrid_matl(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            //grdmatlcost.ShowHeader = !rotate;
            //this.grdmatlcost.DataSource = dt;
            //grdmatlcost.DataBind();
            //if (rotate)
            //{
            //    foreach (GridViewRow row in grdmatlcost.Rows)
            //    {
            //        row.Cells[0].CssClass = "headewidth";
            //        row.Cells[1].CssClass = "headecellwidth";
            //        row.Cells[2].CssClass = "headecellwidth";
            //        row.Cells[3].CssClass = "headecellwidth";
            //        row.Cells[4].CssClass = "headecellwidth";
            //        row.Cells[5].CssClass = "headecellwidth";

            //        row.Cells[2].Visible = false;
            //        row.Cells[3].Visible = false;
            //        row.Cells[4].Visible = false;
            //        row.Cells[5].Visible = false;
            //    }

            //    //foreach (DataGridColumn col in grdmatlcost.Columns)
            //    //{
            //    //    var a = col;
            //    //}
            //}
        }

        private void BindGrid(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            // grdproces.ShowHeader = !rotate;
            // this.grdproces.DataSource = dt;
            // grdproces.DataBind();
            //if (rotate)
            //{
            //    foreach (GridViewRow row in grdproces.Rows)
            //    {
            //        row.Cells[0].CssClass = "headewidth";
            //        row.Cells[1].CssClass = "headecellwidth";
            //    }
            //}
        }

        protected void BindGridIM()
        {
            //  grdmatlcost.EditIndex = 9;
            // grdmatlcost.DataSource = (DataTable)ViewState["Vendors_req"];
            // grdmatlcost.Columns[5].Visible = false;

            //grdmatlcost.Columns[8].Visible = false;
            //grdmatlcost.Columns[9].Visible = false;
            //grdmatlcost.Columns[10].Visible = false;
            //grdmatlcost.Columns[11].Visible = false;
            //grdmatlcost.Columns[12].Visible = false;
            //grdmatlcost.Columns[13].Visible = false;
            //grdmatlcost.Columns[14].Visible = false;

            //  grdmatlcost.DataBind();
        }

        protected void bindgridproc()
        {

            // grdproces.DataSource = (DataTable)ViewState["process"];
            //grdproces.Columns[1].Visible = true;

            //grdmatlcost.Columns[8].Visible = false;
            //grdmatlcost.Columns[9].Visible = false;
            //grdmatlcost.Columns[10].Visible = false;
            //   grdproces.Columns[2].Visible = true;
            //  grdproces.Columns[3].Visible = true;
            //  grdproces.Columns[4].Visible = true;
            //  grdproces.Columns[5].Visible = true;
            //grdproces.Columns[6].Visible = true;

            //  grdproces.DataBind();
        }

        protected void submatrlcost()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                // SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Sub-Matl/T&J Desc"), new DataColumn("Sub-Mat/T&J Cost"), new DataColumn("Consumption(pcs)"), new DataColumn("Sub-Mat/T&J Cost/pcs"), new DataColumn("Total Sub-Mat/T&J Cost/pcs"), });
                ViewState["submatl"] = dt;
                DataRow dr = dt.NewRow();

                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    this.Convert_submatl();
                }
                else
                {
                    this.Convert_submatl();
                    //grdsubmatl.DataSource = dt;
                    //grdsubmatl.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void Convert_submatl()
        {
            DataTable dt = (DataTable)ViewState["submatl"];

            //btnConvert1.Visible = false;
            //btnConvert2.Visible = true;
            DataTable dt2 = new DataTable();
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                dt2.Columns.Add("", typeof(string));
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i][0] = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt2.Rows[i][j + 1] = dt.Rows[j][i];
                }
            }
            BindGrid_submatl(dt2, true);
        }

        private void BindGrid_submatl(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            //grdsubmatl.ShowHeader = !rotate;
            //this.grdsubmatl.DataSource = dt;
            //grdsubmatl.DataBind();
            //if (rotate)
            //{
            //    foreach (GridViewRow row in grdsubmatl.Rows)
            //    {
            //        row.Cells[0].CssClass = "headewidth";
            //        row.Cells[1].CssClass = "headecellwidth";
            //    }
            //}
        }

        //protected void bindgridsubmat()
        //{

        //    grdsubmatl.DataSource = (DataTable)ViewState["submatl"];
        //    grdsubmatl.Columns[1].Visible = true;


        //    grdsubmatl.Columns[2].Visible = true;
        //    grdsubmatl.Columns[3].Visible = true;
        //    grdsubmatl.Columns[4].Visible = true;
        //   // grdsubmatl.Columns[5].Visible = true;
        // //   grdsubmatl.Columns[6].Visible = true;

        //    grdsubmatl.DataBind();
        //}

        protected void otheritemcost()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            try
            {
                // SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Item Description"), new DataColumn("Unit"), new DataColumn("Other Item Cost/pcs"), new DataColumn("Total Other Item Cost/pcs") });
                ViewState["otheritem"] = dt;
                DataRow dr = dt.NewRow();

                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    this.Convert_othercost();
                }
                else
                {
                    this.Convert_othercost();

                    //grdsubmatl.DataSource = dt;
                    //grdsubmatl.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void Convert_othercost()
        {
            DataTable dt = (DataTable)ViewState["otheritem"];

            //btnConvert1.Visible = false;
            //btnConvert2.Visible = true;
            DataTable dt2 = new DataTable();
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                dt2.Columns.Add("", typeof(string));
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i][0] = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt2.Rows[i][j + 1] = dt.Rows[j][i];
                }
            }
            BindGrid_othercost(dt2, true);
        }

        private void BindGrid_othercost(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            //grdotheritem.ShowHeader = !rotate;
            //this.grdotheritem.DataSource = dt;
            //grdotheritem.DataBind();
            //if (rotate)
            //{
            //    foreach (GridViewRow row in grdotheritem.Rows)
            //    {
            //        row.Cells[0].CssClass = "headewidth";
            //        row.Cells[1].CssClass = "headecellwidth";
            //    }
            //}
        }

        //protected void bindotheritem()
        //{

        //    grdotheritem.DataSource = (DataTable)ViewState["otheritem"];
        //    grdotheritem.Columns[0].Visible = true;
        //    grdotheritem.Columns[1].Visible = true;


        //    grdotheritem.Columns[2].Visible = true;
        //    grdotheritem.Columns[3].Visible = true;
        //  //  grdotheritem.Columns[4].Visible = true;
        //    // grdsubmatl.Columns[5].Visible = true;
        //    //   grdsubmatl.Columns[6].Visible = true;

        //    grdotheritem.DataBind();
        //}

        protected void partperunit()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            try
            {
                // SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] { new DataColumn("Total material cost/pcs ($)"), new DataColumn("Total Process Cost/pcs ($)"), new DataColumn("Total Sub-Mat/T&J Cost/pcs ($)"), new DataColumn("Total Other Item Cost/pcs ($)"), new DataColumn("Grand Total Cost/pcs ($)"), new DataColumn("profit (%)"), new DataColumn("Discount (%)"), new DataColumn("Final Quote Price/pcs ($)") });
                ViewState["partperunit"] = dt;
                DataRow dr = dt.NewRow();

                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    this.Convert_partunit();

                }
                else
                {
                    //grdsubmatl.DataSource = dt;
                    //grdsubmatl.DataBind();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void Convert_partunit()
        {
            DataTable dt = (DataTable)ViewState["partperunit"];

            //btnConvert1.Visible = false;
            //btnConvert2.Visible = true;
            DataTable dt2 = new DataTable();
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                dt2.Columns.Add("", typeof(string));
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt2.Rows.Add();
                dt2.Rows[i][0] = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt2.Rows[i][j + 1] = dt.Rows[j][i];
                }
            }
            BindGrid_partunit(dt2, true);
        }

        private void BindGrid_partunit(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            //grdpartunitprice.ShowHeader = !rotate;
            //this.grdpartunitprice.DataSource = dt;
            //grdpartunitprice.DataBind();
            //if (rotate)
            //{
            //    foreach (GridViewRow row in grdpartunitprice.Rows)
            //    {
            //        row.Cells[0].CssClass = "headewidth";
            //        row.Cells[1].CssClass = "headecellwidth";
            //    }
            //}
        }

        protected void bindpartperunit()
        {

            //grdpartunitprice.DataSource = (DataTable)ViewState["partperunit"];
            //grdpartunitprice.Columns[0].Visible = true;
            //grdpartunitprice.Columns[1].Visible = true;


            //grdpartunitprice.Columns[2].Visible = true;
            //grdpartunitprice.Columns[3].Visible = true;
            //grdpartunitprice.Columns[4].Visible = true;
            //grdpartunitprice.Columns[5].Visible = true;
            //grdpartunitprice.Columns[6].Visible = true;
            //grdpartunitprice.Columns[7].Visible = true;
            ////grdpartunitprice.Columns[8].Visible = true;
            ////grdpartunitprice.Columns[9].Visible = true;

            //grdpartunitprice.DataBind();
        }
        //protected void btnaddcolmn_Click(object sender, EventArgs e)
        //{

        //      DataTable dt_addcol = (DataTable)ViewState["Vendors_req"];
        //      DataRow oItem = dt_addcol.NewRow();
        //      //grdmatlcost
        //      GridView gv = new GridView();
        //      gv.AutoGenerateColumns = false;
        //      BoundField nameColumn = new BoundField();
        //      nameColumn.DataField = "FirstName";
        //      nameColumn.HeaderText = "First Name";
        //      gv.Columns.Add(nameColumn);
        //      nameColumn = new BoundField();
        //      nameColumn.DataField = "LastName";
        //      nameColumn.HeaderText = "Last Name";
        //      gv.Columns.Add(nameColumn);
        //      nameColumn = new BoundField();
        //      nameColumn.DataField = "Age";
        //      nameColumn.HeaderText = "Age";
        //      gv.Columns.Add(nameColumn);
        //      // Here is template column portion
        //      TemplateField TmpCol = new TemplateField();
        //      TmpCol.HeaderText = "Click Me";
        //      gv.Columns.Add(TmpCol);
        ////      TmpCol.ItemTemplate = new NewReq_changes();
        //      gv.DataSource = dt_addcol;
        //      gv.DataBind();

        //      Form.Controls.Add(gv);


        //  }




        //protected void Convert(object sender, EventArgs e)
        //{
        //    DataTable dt = (DataTable)ViewState["dt"];
        //    if ((sender as Button).CommandArgument == "1")
        //    {
        //        //btnConvert1.Visible = false;
        //        //btnConvert2.Visible = true;
        //        DataTable dt2 = new DataTable();
        //        for (int i = 0; i <= dt.Rows.Count; i++)
        //        {
        //            dt2.Columns.Add("", typeof(string));
        //        }
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            dt2.Rows.Add();
        //            dt2.Rows[i][0] = dt.Columns[i].ColumnName;
        //        }
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Rows.Count; j++)
        //            {
        //                dt2.Rows[i][j + 1] = dt.Rows[j][i];
        //            }
        //        }
        //        BindGrid(dt2, true);
        //    }
        //    else
        //    {
        //        //btnConvert1.Visible = true;
        //        //btnConvert2.Visible = false;
        //        BindGrid(dt, false);
        //    }
        //}



        private void CommentedCodes()
        {
            //string strReqNo = (string)HttpContext.Current.Session["RequestNo"].ToString();

            //string strReqDate = (string)HttpContext.Current.Session["requestdate"].ToString();

            //string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            //string strprod = (string)HttpContext.Current.Session["prod_code"].ToString();
            //string matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            //string strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            //string matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            //string proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            //string splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            //string strproc = (string)HttpContext.Current.Session["process"].ToString();
            //txtprocs.Text = (string)HttpContext.Current.Session["process"].ToString();
            //txtprod.Text = (string)HttpContext.Current.Session["prod_code"].ToString();
            //txtprocs.Text = (string)HttpContext.Current.Session["process"].ToString();

            //txtSAPJobType.Text = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
            //txtpartdesc.Text = (string)HttpContext.Current.Session["material"].ToString();

            //txtdrawng.Text = (string)HttpContext.Current.Session["DrawingNo"].ToString();

            //string strpir = (string)HttpContext.Current.Session["Pirtype"].ToString();
            //string strPIRDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();
            //txtPIRtype.Text = strPIRDesc.ToString();



            //this.GetData(strReqNo);

            // this.getmaterial();




            //  //  this.BindGridIM();
            //    this.getprocconnection();
            ////    this.bindgridproc();
            //   this.submatrlcost();

            //  //  this.bindgridsubmat();
            //    this.otheritemcost();
            // //   this.bindotheritem();
            //    this.partperunit();
            //  //  this.bindpartperunit();



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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MCDataStore(); ", true);
            //CreateDynamicDT(0);
            //MtlAddCount++;
            //CreateDynamicDT(MtlAddCount);

            //CreateDynamicProcessDT(0);
            //CreateDynamicProcessDT(PCAddCount);
        }

        #endregion

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MatlCalculation();", true);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            string RequestIncNumber = Request.QueryString["Number"];
            string struser = (string)HttpContext.Current.Session["userID_"].ToString();
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection conins;
            conins = new SqlConnection(connetionStringdate);
            try
            {
                conins.Open();
                string tMatCost = "";
                string UserId = Session["userID_"].ToString();
                string query = "Update TQuoteDetails SET Status = 1 , TotalMaterialCost= '" + tMatCost + "' UpdatedBy = '" + struser + "', UpdatedOn = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' Where QuoteNo ='" + RequestIncNumber + "' and VendorCode1 = '" + Session["mappedVendor"].ToString() + "' ";
                //    string text = "Data Saved!";
                SqlCommand cmd1 = new SqlCommand(query, conins);
                cmd1.Parameters.AddWithValue("@REQUESTNO", int.Parse(RequestIncNumber.ToString()));
                cmd1.CommandText = query;
                cmd1.ExecuteNonQuery();

                Response.Redirect("Vendor.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                conins.Close();
            }

        }

        protected void TurnKeyprovit()
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select DefValue from [DefaultValueMaster] where Description = 'Turnkey Profit' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, con1);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    hdnHidenProfit.Value = Result.Rows[0]["DefValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con1.Close();
            }
        }

        protected void process()
        {
            DropDownList ddlProcess = new DropDownList();
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string UserId = Session["userID_"].ToString();
                string VndId = Session["mappedVendor"].ToString();
                string str = "select distinct CONCAT( tp.Process_Grp_code, ' - ', tp.Process_Grp_Description) as Process_Grp_code from TPROCESGROUP_LIST TP inner join TPROCESGROUP_SUBPROCESS TPS on Tp.Process_Grp_code = tps.ProcessGrpCode where TP.DELFLAG = 0";
                da = new SqlDataAdapter(str, con1);
                Result = new DataTable();
                da.Fill(Result);

                ddlProcess.DataSource = Result;
                ddlProcess.DataTextField = "Process_Grp_code";
                ddlProcess.DataValueField = "Process_Grp_code";
                ddlProcess.DataBind();
                Session["process"] = ddlProcess.DataSource;

                #region  get subvendor data
                string[] DataVnd = lblVName.Text.Split('-');
                string VndCode = Session["vendorC"].ToString();
                DropDownList ddlProcessSubVndorData = new DropDownList();
                DataTable ResultSubVndName = new DataTable();
                SqlDataAdapter daSubVndName = new SqlDataAdapter();
                string strSubVndName = "select distinct CONCAT( TKV.SubVendor, ' - ', TVN.Description) as SubVndorData  from TTURNKEY_VENDOR TKV join tVendor_New TVN on TKV.SubVendor = TVN.Vendor where TKV.TrnKeyVendor = '" + VndCode + "' and TKV.DELFLAG = 0 ";
                da = new SqlDataAdapter(strSubVndName, con1);
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
                string strplant = Session["strplant"].ToString();
                string str1 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as Machine, TVM.SMNStdrateHr as SMNStdrateHr ,TVM.FollowStdRate  as FollowStdRate,TVM.Currency as Currency, TVM.ProcessGrp as ProcessGrp from TVENDORMACHNLIST TVM where TVM.VendorCode = '" + VndId + "' and Plant = '" + strplant + "' and TVM.DELFLAG = 0 ";
                da1 = new SqlDataAdapter(str1, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                grdMachinelisthidden.DataSource = Result1;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;


                DataTable Result3 = new DataTable();
                SqlDataAdapter da3 = new SqlDataAdapter();

                string str3 = "select StdLabourRateHr,FollowStdRate,Currency from TVENDORLABRCOST TVC Where TVC.Vendorcode = '" + VndId + "' and TVC.DELFLAG = 0 ";
                da3 = new SqlDataAdapter(str3, con1);
                Result3 = new DataTable();
                da3.Fill(Result3);

                grdLaborlisthidden.DataSource = Result3;
                grdLaborlisthidden.DataBind();
                Session["LaborListGrd"] = grdLaborlisthidden.DataSource;

                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                strplant = Session["strplant"].ToString();
                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM where VendorCode = '" + VndId + "'  and Plant = '" + strplant + "' and TVM.DELFLAG = 0 ";
                da2 = new SqlDataAdapter(str2, con1);
                Result2 = new DataTable();
                da2.Fill(Result2);

                ddlMachine.DataSource = Result2;
                ddlMachine.DataBind();
                Session["MachineIDs"] = ddlMachine.DataSource;



                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + txtprocs.Text.ToString().ToUpper() + "' and DELFLAG = 0 ";
                da4 = new SqlDataAdapter(str4, con1);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con1.Close();
            }
        }

        protected void MachineIdsbyProcess(string strProcessCode)
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                string strplant = Session["strplant"].ToString();
                string UserId = Session["userID_"].ToString();
                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM where VendorCode = '" + Session["mappedVendor"].ToString() + "'  and Plant = '" + strplant + "' and ProcessGrp= '" + strProcessCode + "' and TVM.DELFLAG = 0 ";
                da2 = new SqlDataAdapter(str2, con1);
                Result2 = new DataTable();
                da2.Fill(Result2);

                //ddlMachine.DataSource = Result2;
                //ddlMachine.DataBind();
                Session["MachineIDsbyProcess"] = Result2;
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con1.Close();
            }
        }

        protected void btnaddProcessCost_Click(object sender, EventArgs e)
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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ProcessCostDataStore();", true);
            // CreateDynamicDT(0);
            // CreateDynamicDT(MtlAddCount);

            //CreateDynamicProcessDT(0);
            //PCAddCount++;
            //CreateDynamicProcessDT(PCAddCount);
        }

        protected void BtnDelProcess_Click(object sender, EventArgs e)
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
            //ColmNoMat = ColmNoMat - 1;
            //int a = ColmNoMat;
            //if (ColmNoMat > 1)
            //{
            //    ColmNoMat = ColmNoMat - 1;
            //}
            //MCCostDataStore();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "MatlCalculation();", true);
        }

        protected void btnAddSubProcessCost_Click(object sender, EventArgs e)
        {
            DataTable DtDynamicSubMaterialsFields = new DataTable();
            DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

            int SMCAddCount = int.Parse(Session["SMCAddCount"].ToString());

            var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

            var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

            CreateDynamicSubMaterialDT(0);
            SMCAddCount++;
            Session["SMCAddCount"] = SMCAddCount;
            //string FL = HdnFirstLoadSubProc.Value.ToString();

            if (SMCAddCount <= ccc)
            {
                SMCAddCount = SMCAddCount + (ccc - SMCAddCount) + 1;
            }
            CreateDynamicSubMaterialDT(SMCAddCount);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "submatlCostDataStore();", true);
            //CreateDynamicSubMaterialDT(0);
            //SMCAddCount++;
            //CreateDynamicSubMaterialDT(SMCAddCount);
        }

        protected void BtnDelSubMatCost_Click(object sender, EventArgs e)
        {
            DataTable DtDynamicSubMaterialsFields = new DataTable();
            DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

            int SMCAddCount = int.Parse(Session["SMCAddCount"].ToString());

            var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

            var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

            if (hdnSMCTableValues.Value.ToString() == "")
            {
                CreateDynamicSubMaterialDT(0);
            }
            else
            {
                CreateDynamicSubMaterialDT(0);
                SMCAddCount--;
                Session["SMCAddCount"] = SMCAddCount;
                //string FL = HdnFirstLoadSubProc.Value.ToString();

                if (SMCAddCount <= ccc)
                {
                    SMCAddCount = SMCAddCount + (ccc - SMCAddCount);
                    if (SMCAddCount > 0)
                    {
                        CreateDynamicSubMaterialDT(SMCAddCount);
                    }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "submatlcost();", true);
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

        protected void btnAddOtherCost_Click(object sender, EventArgs e)
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "OthersCostDataStore();", true);
            //CreateDynamicOthersCostDT(0);
            //OthersAddCount++;
            //CreateDynamicOthersCostDT(OthersAddCount);
        }

        protected void BtnDelOthCost_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "othercost();", true);
            }


            //CreateDynamicOthersCostDT(0);
            //OthersAddCount++;
            //CreateDynamicOthersCostDT(OthersAddCount);
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {

            if (TextBox1.Text != "" && txtfinal.Text != "")
            {
                DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);
                DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd/MM/yyyy", null);
                int result = DateTime.Compare(newEffDate, newDueon);

                if (result < 0)
                {

                    if (hdnTGTotal.Value != "")
                    {
                        SaveallCostDetails();

                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();

                        //string sste11 = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);



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

                        if (TProfit.ToUpper() == "NAN")
                        {
                            TProfit = "";
                        }
                        if (TDiscount.ToUpper() == "NAN")
                        {
                            TDiscount = "";
                        }

                        string userSession = (string)HttpContext.Current.Session["userID_"].ToString();

                        // string query = "INSERT INTO TVENDOR_PROCESSGROUP(VendorCode,VendorName,ProcessGrp,ProcessGrpDescription,UpdateBy,UpdatedOn) VALUES (@VendorCode,@VendorName,@ProcessGrp,@ProcessGrpDescription,@UpdateBy,@UpdatedOn)";
                        // string query = "update TQuoteDetails SET ApprovalStatus = 1 Where QuoteNo = '"+ QuoteNo + "' ";
                        string query1 = "update TQuoteDetails SET  TotalMaterialCost = '" + TMCost + "', TotalSubMaterialCost = '" + TSMCost + "', TotalProcessCost = '" + TPCost + "', TotalOtheritemsCost = '" + TOCost + "',GrandTotalCost = '" + GTCost + "' ,FinalQuotePrice = '" + FTCost + "',Profit = '" + TProfit + "',Discount = '" + TDiscount + "', ShimanoPIC = '" + SMNPIC + "',ShimanoPICEmail = '" + SMNPICEmail + "' , ApprovalStatus = '2' , PICApprovalStatus=0, EffectiveDate = '" + newEffDate.ToString("yyyy/MM/dd HH:mm:ss") + "'  , DueOn = '" + newDueon.ToString("yyyy/MM/dd HH:mm:ss") + "'  , UpdatedBy = '" + userSession + "' , UpdatedOn = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',CommentByVendor=@CommentByVendor ,countryorg=@countryorg Where QuoteNo = '" + hdnQuoteNo.Value + "'";

                        //    string text = "Data Saved!";
                        SqlCommand cmd1 = new SqlCommand(query1, conins);
                        cmd1.CommandText = query1;
                        cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd1.Parameters.AddWithValue("@countryorg", ddlpirjtype.SelectedValue.ToString());
                        cmd1.Parameters.AddWithValue("@CommentByVendor", TxtComntByVendor.Text);
                        cmd1.ExecuteNonQuery();

                        string PartCode = txtpartdesc.Text.Replace("-", "").Trim();
                        if (PartCode == "")
                        {
                            query1 = @"update TQuoteDetails SET  ApprovalStatus='5',PICApprovalStatus = '5', ManagerApprovalStatus = '5', DIRApprovalStatus = '5' 
                                        where  requestnumber in(select distinct RequestNumber from TQuoteDetails where  QuoteNo = '" + hdnQuoteNo.Value + "')";
                        }
                        else
                        {
                            query1 = "update TQuoteDetails SET  PICApprovalStatus=0 where  requestnumber in(select distinct RequestNumber from TQuoteDetails where  QuoteNo = '" + hdnQuoteNo.Value + "')";
                        }

                        //    string text = "Data Saved!";
                        SqlCommand cmd2 = new SqlCommand(query1, conins);
                        cmd2.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);

                        cmd2.CommandText = query1;
                        cmd2.ExecuteNonQuery();

                        //Email
                        // getting Messageheader ID from IT Mailapp
                        #region sending email
                        bool sendingemail = false;
                        string MsgErr = "";
                        try
                        {

                            var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                            using (SqlConnection cnn = new SqlConnection(Email))
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
                                    cnn.Close();
                                    OriginalFilename = returnValue;
                                    MHid = returnValue;
                                    Session["MHid"] = returnValue;
                                    OriginalFilename = MHid + seqNo + formatW;
                                }

                                catch (Exception xw)
                                {
                                    transactionHS.Rollback();
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                    var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                            }

                            Boolean IsAttachFile = true;
                            int SequenceNumber = 1;
                            string UserId = Session["userID_"].ToString();
                            IsAttachFile = false;
                            Session["SendFilename"] = "NOFILE";
                            OriginalFilename = "NOFILE";
                            format = "NO";

                            //getting vendor mail id
                            var Vendormail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                            using (SqlConnection cnn = new SqlConnection(Vendormail))
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
                                    aemail = dr.GetString(1);
                                    Session["aemail"] = dr.GetString(1);
                                    pemail = dr.GetString(2);
                                    Session["pemail"] = string.Concat(aemail, ";", pemail);

                                }
                                dr.Close();
                                cnn.Close();
                            }

                            //getting User mail id
                            var usermail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                            using (SqlConnection cnn = new SqlConnection(usermail))
                            {
                                //userId1 = Session["userID_"].ToString();
                                UserId = Session["userID_"].ToString();
                                // Userid = Session["Userid"].ToString();
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
                                dr.Close();
                                cnn.Close();
                            }

                            //getting vendor mail content
                            var mailcontent = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            using (SqlConnection cnn = new SqlConnection(mailcontent))
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
                                dr.Close();
                                cnn.Close();
                            }
                            // Insert header and details to Mil server table to IT mailserverapp
                            var Email_insert = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                            using (SqlConnection Email_inser = new SqlConnection(Email_insert))
                            {
                                Email_inser.Open();
                                //Header
                                Uemail = Session["Uemail"].ToString();
                                pemail = Session["pemail"].ToString();
                                body1 = Session["body1"].ToString();
                                nameC = Session["UserName"].ToString();
                                string MessageHeaderId = Session["MHid"].ToString();
                                string fromname = "eMET System";
                                //string FromAddress = pemail;

                                string FromAddress = "eMET@Shimano.Com.sg";
                                //string Recipient = aemail + "," + pemail;
                                string Recipient = Session["pemail"].ToString();
                                string CopyRecipient = Session["Createuser"].ToString();
                                string BlindCopyRecipient = "";
                                string ReplyTo = "subashdurai@shimano.com.sg";
                                string Subject = "Quotation Number: " + hdnQuoteNo.Value + " - Submitted By : " + nameC;
                                //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted.<br /><br />" + body1.ToString();
                                string BodyFormat = "HTML";
                                string BodyRemark = "0";
                                string Signature = " ";
                                string Importance = "High";
                                string Sensitivity = "Confidential";
                                UserId = Session["userID_"].ToString();
                                // Userid = Session["Userid"].ToString();
                                UserId = Session["userID_"].ToString();
                                string userId1 = Session["userID_"].ToString();
                                string CreateUser = Session["userID_"].ToString();
                                DateTime CreateDate = DateTime.Now;
                                //end Header

                                SqlTransaction transactionHe;
                                transactionHe = Email_inser.BeginTransaction("Header");
                                try
                                {
                                    string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                    SqlCommand Header = new SqlCommand(Head, Email_inser);
                                    Header.Transaction = transactionHe;
                                    Header.Parameters.AddWithValue("@MessageHeaderId", MessageHeaderId.ToString());
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
                                    Header.Parameters.AddWithValue("@CreateUser", userId1.ToString());
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
                                //userId1 = Session["userID_"].ToString();


                                SqlTransaction transactionDe;
                                transactionDe = conins.BeginTransaction("Detail");
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
                                    Detail.Parameters.AddWithValue("@CreateUser", userId1.ToString());
                                    Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    Detail.CommandText = Details;
                                    Detail.ExecuteNonQuery();
                                    transactionDe.Commit();
                                }
                                ///
                                catch (Exception xw)
                                {
                                    transactionDe.Rollback();
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                    var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                Email_inser.Close();
                                //End Details
                            }
                            sendingemail = true;
                        }
                        catch (Exception ex)
                        {
                            sendingemail = false;
                            MsgErr = ex.ToString();
                            //Response.Write(ex);
                        }

                        if (sendingemail == false)
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + MsgErr + " ");
                            var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);

                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('failed sending email');", true);
                            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('failed sending email'); ", true);
                        }
                        else
                        {
                            Response.Redirect("Vendor.aspx");
                        }
                        #endregion
                        //End by subash
                        //End Email
                        conins.Close();
                    }

                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Could not submit without done calculation')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Effective Date should not be Lessthan Due date')", true);
                }
            }
            else
            {
                if (TextBox1.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select SMN Quote Effective Date')", true);
                }
                else if (txtfinal.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please Select SMN Due Date for Next Revision')", true);
                }
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Session["Table1"] = Table1;
        }

        protected void BtnSaveDraft_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text != "" && txtfinal.Text != "")
            {

                DateTime newEffDate = DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);
                DateTime newDueon = DateTime.ParseExact(txtfinal.Text, "dd/MM/yyyy", null);
                int result = DateTime.Compare(newEffDate, newDueon);
                if (result < 0)
                {

                    if (hdnTGTotal.Value != "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ReCalculate();", true);
                        ChcExistDraft();
                        SaveallCostDetailsDraft();
                        InsertQuoteDetailDraft();
                        updateQuoteDetDraft();
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                        Response.Redirect("Vendor.aspx");
                    }

                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Could not submit without done calculation')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Effective Date should not be Lessthan Due date')", true);
                }
            }
            else
            {
                if (TextBox1.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Effective Date should not be null')", true);
                }
                else if (txtfinal.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Due On should not be null')", true);
                }
            }
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

        private void SaveallCostDetails()
        {
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

                    if (tempOtherlistNew[2].ToString() == "" || tempOtherlistNew[2].ToString() == null)
                    { }
                    else
                    {
                        string query = "insert into [dbo].[TOtherCostDetails] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

                        string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempOtherlistNew[1].ToString();
                        string ItotalCost = tempOtherlistNew[2].ToString();

                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();
                        string UserId = Session["userID_"].ToString();
                        SqlCommand cmd1 = new SqlCommand(query, conins);
                        cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                        cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                        cmd1.Parameters.AddWithValue("@Desc", IDesc);
                        cmd1.Parameters.AddWithValue("@Cost", ICost);
                        cmd1.Parameters.AddWithValue("@TotalCost", ItotalCost);
                        cmd1.Parameters.AddWithValue("@RowId", i);
                        cmd1.Parameters.AddWithValue("@By", UserId);
                        cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        cmd1.CommandText = query;
                        cmd1.ExecuteNonQuery();
                        conins.Close();
                    }

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

                    if (tempSMClistNew[3].ToString() == "" || tempSMClistNew[3].ToString() == null)
                    { }
                    else
                    {

                        string IDesc = tempSMClistNew[0].ToString().Replace("NaN", "");
                        string ICost = tempSMClistNew[1].ToString();
                        string IConsumption = tempSMClistNew[2].ToString();
                        string ICostpcs = tempSMClistNew[3].ToString();
                        string ItotalCost = tempSMClistNew[4].ToString();


                        if (ICostpcs == null || ICostpcs == "")
                        {

                        }
                        else
                        {
                            string UserId = Session["userID_"].ToString();
                            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            SqlConnection conins;
                            conins = new SqlConnection(connetionStringdate);
                            conins.Open();
                            string query = "insert into [dbo].[TSMCCostDetails] (QuoteNo,ProcessGroup,[Sub-Mat/T&JDescription],[Sub-Mat/T&JCost],[Consumption(pcs)],[Sub-Mat/T&JCost/pcs],[TotalSub-Mat/T&JCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@IConsumption,@ICostpcs,@ItotalCost,@RowId,@By,@On)";
                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());
                            cmd1.Parameters.AddWithValue("@Desc", IDesc);
                            cmd1.Parameters.AddWithValue("@Cost", ICost);
                            cmd1.Parameters.AddWithValue("@IConsumption", IConsumption);
                            cmd1.Parameters.AddWithValue("@ICostpcs", ICostpcs);
                            cmd1.Parameters.AddWithValue("@ItotalCost", ItotalCost);
                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
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

                    if (tempProcesslistNew[14].ToString() == "" || tempProcesslistNew[14].ToString() == null)
                    { }
                    else
                    {

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

                        if (ICostPc == null || ICostPc == "") { }
                        else
                        {
                            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            SqlConnection conins;
                            conins = new SqlConnection(connetionStringdate);
                            conins.Open();
                            string UserId = Session["userID_"].ToString();
                            string query = "insert into [dbo].[TProcessCostDetails] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn,TurnKeySubVnd,TurnKeyCost,TurnKeyProfit) "
                                + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On,@ITurnKeySubVnd,@ITurnKeyCost,@ITurnKeyProfit)";
                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@IProc", IProc);
                            cmd1.Parameters.AddWithValue("@ISubProc", ISubProc);
                            cmd1.Parameters.AddWithValue("@ITurnKey", ITurnKey);
                            cmd1.Parameters.AddWithValue("@ITurnKeySubVnd", ITurnKeySubVnd);
                            cmd1.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
                            cmd1.Parameters.AddWithValue("@IMachine", IMachine);
                            cmd1.Parameters.AddWithValue("@IStRate", IStRate);
                            cmd1.Parameters.AddWithValue("@IVendorRate", IVendorRate);
                            cmd1.Parameters.AddWithValue("@IProcUOM", IProcUOM);
                            cmd1.Parameters.AddWithValue("@IBaseQty", IBaseQty);
                            cmd1.Parameters.AddWithValue("@IDPUOM", IDPUOM);
                            cmd1.Parameters.AddWithValue("@Iyield", Iyield);
                            cmd1.Parameters.AddWithValue("@ITurnKeyCost", ITurnKeyCost);
                            cmd1.Parameters.AddWithValue("@ITurnKeyProfit", ITurnKeyProfit);
                            cmd1.Parameters.AddWithValue("@ICostPc", ICostPc);
                            cmd1.Parameters.AddWithValue("@ITotalCost", ITotalCost);

                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
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

                for (int i = 1; i <= ccc; i++)
                {
                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var tempMClistNew = tempMClist;
                    if (i > 1)
                    {
                        tempMClistNew = tempMClistNew.Skip(((i - 1) * (DtDynamic.Rows.Count))).ToList();
                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();

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



                        #region Common for All
                        strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                        strMDesc = tempMClistNew[1].ToString();
                        strRawCost = tempMClistNew[2].ToString();
                        strTotalRawCost = tempMClistNew[3].ToString();
                        strPartUnitW = tempMClistNew[4].ToString();
                        #endregion Common for all

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = tempMClistNew[5].ToString();
                            strRunnerRatio = tempMClistNew[6].ToString();
                            strRecycle = tempMClistNew[7].ToString();

                            strCavity = tempMClistNew[8].ToString();
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

                        if (strMCostpcs == null || strMCostpcs == "")
                        {

                        }
                        else
                        {
                            string UserId = Session["userID_"].ToString();
                            string query = "insert into [dbo].[TMCCostDetails] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On)";

                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd1.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd1.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd1.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd1.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd1.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd1.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd1.Parameters.AddWithValue("@strThick", strThick);
                            cmd1.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd1.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd1.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd1.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd1.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd1.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd1.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd1.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd1.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd1.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd1.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd1.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd1.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd1.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd1.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);


                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
                    else
                    {
                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();

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

                        #region Common for All
                        strMCode = tempMClistNew[0].ToString().Replace("NaN", "");
                        strMDesc = tempMClistNew[1].ToString();
                        strRawCost = tempMClistNew[2].ToString();
                        strTotalRawCost = tempMClistNew[3].ToString();
                        strPartUnitW = tempMClistNew[4].ToString();
                        #endregion Common for all

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = tempMClistNew[5].ToString();
                            strRunnerRatio = tempMClistNew[6].ToString();
                            strRecycle = tempMClistNew[7].ToString();

                            strCavity = tempMClistNew[8].ToString();
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

                        if (strMCostpcs == null || strMCostpcs == "")
                        {

                        }
                        else
                        {
                            string UserId = Session["userID_"].ToString();
                            string query = "insert into [dbo].[TMCCostDetails] (QuoteNo,ProcessGroup,[MaterialSAPCode],[MaterialDescription],[RawMaterialCost/kg],"
                                + "[TotalRawMaterialCost/g],[PartNetUnitWeight(g)],[~~DiameterID(mm)],[~~DiameterOD(mm)],[~~Thickness(mm)],[~~Width(mm)],[~~Pitch(mm)] ,"
                                + "[~MaterialDensity],[~RunnerWeight/shot(g)],[~RunnerRatio/pcs(%)],[~RecycleMaterialRatio(%)],[Cavity],[MaterialYield/MeltingLoss(%)],"
                                + "[MaterialGrossWeight/pc(g)],[MaterialScrapWeight(g)] ,[ScrapLossAllowance(%)],[ScrapPrice/kg] ,[ScrapRebate/pcs],[MaterialCost/pcs],"
                                + "[TotalMaterialCost/pcs],RowId,UpdatedBy,UpdatedOn)"

                                + "VALUES (@QuoteNo,@PG,@strMCode,@strMDesc,@strRawCost,@strTotalRawCost,@strPartUnitW,@strDiaID,@strDiaOD,@strThick,@strWidth,@strPitch,@strMDensity,"
                                + "@strRunnerWeight,@strRunnerRatio,@strRecycle,@strCavity,@strMLoss,@strMCrossWeight,@strMScrapWeight,"
                                + "@strScrapLoss,@strScrapPrice,@strScrapRebate,@strMCostpcs,@strTotalcostpcs, @RowId,@By,@On)";

                            SqlCommand cmd1 = new SqlCommand(query, conins);
                            cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                            cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                            cmd1.Parameters.AddWithValue("@strMCode", strMCode);
                            cmd1.Parameters.AddWithValue("@strMDesc", strMDesc);
                            cmd1.Parameters.AddWithValue("@strRawCost", strRawCost);
                            cmd1.Parameters.AddWithValue("@strTotalRawCost", strTotalRawCost);
                            cmd1.Parameters.AddWithValue("@strPartUnitW", strPartUnitW);
                            cmd1.Parameters.AddWithValue("@strDiaID", strDiaID);
                            cmd1.Parameters.AddWithValue("@strDiaOD", strDiaOD);
                            cmd1.Parameters.AddWithValue("@strThick", strThick);
                            cmd1.Parameters.AddWithValue("@strWidth", strWidth);
                            cmd1.Parameters.AddWithValue("@strPitch", strPitch);
                            cmd1.Parameters.AddWithValue("@strMDensity", strMDensity);
                            cmd1.Parameters.AddWithValue("@strRunnerWeight", strRunnerWeight);
                            cmd1.Parameters.AddWithValue("@strRunnerRatio", strRunnerRatio);
                            cmd1.Parameters.AddWithValue("@strRecycle", strRecycle);
                            cmd1.Parameters.AddWithValue("@strCavity", strCavity);
                            cmd1.Parameters.AddWithValue("@strMLoss", strMLoss);
                            cmd1.Parameters.AddWithValue("@strMCrossWeight", strMCrossWeight);
                            cmd1.Parameters.AddWithValue("@strMScrapWeight", strMScrapWeight);
                            cmd1.Parameters.AddWithValue("@strScrapLoss", strScrapLoss);
                            cmd1.Parameters.AddWithValue("@strScrapPrice", strScrapPrice);
                            cmd1.Parameters.AddWithValue("@strScrapRebate", strScrapRebate);
                            cmd1.Parameters.AddWithValue("@strMCostpcs", strMCostpcs);
                            cmd1.Parameters.AddWithValue("@strTotalcostpcs", strTotalcostpcs);


                            cmd1.Parameters.AddWithValue("@RowId", i);
                            cmd1.Parameters.AddWithValue("@By", UserId);
                            cmd1.Parameters.AddWithValue("@On", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            cmd1.CommandText = query;
                            cmd1.ExecuteNonQuery();
                            conins.Close();
                        }
                    }
                }
            }

            #endregion Process Values

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //Response.Redirect
            string qno = Session["Qno"].ToString();
            Response.Redirect("NewReq_changes.aspx?Number=" + qno.ToString());
        }

        protected void ddlpirjtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string corg = ddlpirjtype.SelectedItem.Value;
            Session["corg"] = ddlpirjtype.SelectedItem.Value;
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreSubprocGroup(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
        }

        protected void BtnFndVndMachineVsProcUom_Click(object sender, EventArgs e)
        {
            GetDataDataVndmachine();
            GetDataProcesUOM();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreProcUOM(" + ColumnNo + ");StoreVnderMachineList(" + ColumnNo + ");", true);
        }

        protected void BtnFndProcUom_Click(object sender, EventArgs e)
        {
            GetDataProcesUOM();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreProcUOM(" + ColumnNo + ");", true);
        }

        protected void BtnFndVndRate_Click(object sender, EventArgs e)
        {
            string ProcGroup = hdnProcGroup.Value.ToString();
            string MachineId = hdnMachineId.Value.ToString();
            MachineId = getMachineID();
            GetDataVendRate(ProcGroup, MachineId);
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "StoreVndRate(" + ColumnNo + ");", true);
        }

        protected void BtnMacList_Click(object sender, EventArgs e)
        {
            GetDataDataVndmachine();
            string ColumnNo = hdnColumTblProcNo.Value.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "StoreVnderMachineList(" + ColumnNo + ");", true);
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
    }
}