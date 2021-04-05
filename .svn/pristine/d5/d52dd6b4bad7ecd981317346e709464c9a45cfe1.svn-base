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

namespace Material_Evaluation
{
    public partial class Eview : System.Web.UI.Page
    {
        public static string vendorC;
        public static List<string> metfields;
        public static Dictionary<string, string> DicQuoteDetails;
        public static string QuoteNo;
        public static DataTable DtDynamic;   /// Fields from METFields
        public static DataTable DtMaterial;
        public static DataTable DtMaterialsDetails;
        public static string ratiochange = "";
        public static int MtlAddCount = 1;
        public static int PCAddCount = 1;
        public static int SMCAddCount = 1;
        public static int OthersAddCount = 1;
        public static int UnitAddCount = 1;

        public static DropDownList ddlProcess;


        // public static DataTable DtDynamicMaterials;

        public static DataTable DtDynamicProcessFields;
        public static DataTable DtDynamicProcessCostsDetails;

        public static DataTable DtDynamicSubMaterialsFields;
        public static DataTable DtDynamicSubMaterialsDetails;

        public static DataTable DtDynamicOtherCostsFields;
        public static DataTable DtDynamicOtherCostsDetails;

        public static DataTable DtDynamicUnitFields;
        public static DataTable DtDynamicUnitDetails;

        // static string ProcessGrp = "IM";

        public static Table TableMat;

        public static string UserId;

        protected void Page_Load(object sender, EventArgs e)
        {

            DicQuoteDetails = new Dictionary<string, string>();

            // Raja load one by one Grid Dynamically
            UserId = Session["userID"].ToString();

            if (!IsPostBack)
            {



                if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                {

                    string userId = Session["userID"].ToString();
                    string sname = Session["UserName"].ToString();
                    string srole = Session["userType"].ToString();
                    string concat = sname + " - " + srole;
                    lblUser.Text = sname;
                    lblplant.Text = srole;
                    QuoteNo = Request.QueryString["Number"];

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
                                    lblVName.Text = "Vendor Name: " + vendorC + "-" + pir.Rows[0]["description"].ToString();
                                    lblCurrency.Text = "Vendor Currency: " + pir.Rows[0]["crcy"].ToString();
                                    lblCity.Text = "Vendor Country: " + pir.Rows[0]["cty"].ToString();
                                    lblcry.Text = "(" + pir.Rows[0]["crcy"].ToString() + ")";


                                }
                            }
                        }
                        //End
                    }


                    
                    MtlAddCount = 1;
                    PCAddCount = 1;
                    SMCAddCount = 1;
                    OthersAddCount = 1;
                    UnitAddCount = 1;
                    lblreqst.Text = "Quote No : " + QuoteNo.ToString();
                    metfields = new List<string>();
                    DtDynamic = new DataTable();
                    DtMaterial = new DataTable();
                    DicQuoteDetails = new Dictionary<string, string>();

                    GetQuoteandAllDetails(QuoteNo);
                    GetQuoteDetailsbyQuotenumber(QuoteNo);
                    CreateDynamicTablebasedonProcessField();

                    string struser = (string)HttpContext.Current.Session["userID"].ToString();

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
                    //CreateDynamicOthersCostDT(0);



                    //CreateDynamicDT(0);
                    //CreateDynamicProcessDT(0);
                    //CreateDynamicSubMaterialDT(0);

                    //CreateDynamicUnitDT(0);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "RestoreDataTbaleUnit();", true);

                }
            }
        }



        private void OthersCostDataStore()
        {
            var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

            var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;
            CreateDynamicOthersCostDT(0);
            if (ccc > 1)
                CreateDynamicOthersCostDT(ccc);



        }

        private void subMatCostDataStore()
        {
            var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

            var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

            CreateDynamicSubMaterialDT(0);

            if (ccc > 1)
            {

                CreateDynamicSubMaterialDT(ccc);

            }

        }

        private void ProcessCostDataStore()
        {
            var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

            var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

            CreateDynamicProcessDT(0);
            if (ccc > 1)
            {

                CreateDynamicProcessDT(ccc);

            }
        }

        private void MCCostDataStore()
        {
            var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

            var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

            CreateDynamicDT(0);


            if (ccc > 1)
            {

                CreateDynamicDT(ccc);

            }

        }

        private void UnitCostDataStore()
        {
            var tempUnitlistcount = hdnUnitValues.Value.ToString().Split(',').ToList();

            var ccc = tempUnitlistcount.Count / DtDynamicUnitFields.Rows.Count;

            CreateDynamicUnitDT(0);

        }



        // Used this method by Raja
        private void GetQuoteDetailsbyQuotenumber(string QuoteNo)

        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select * from EMET.dbo.TQuoteDetails where QuoteNo='" + QuoteNo + "' and CreateStatus ='Article' ";
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
                string baseUOm = dtdate.Rows[0].ItemArray[51].ToString();
                txtBaseUOM.Text = baseUOm.ToString();
                string PartUnit = dtdate.Rows[0].ItemArray[18].ToString();


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
                            lblVName.Text = "Vendor Name: " + vendorC + "-" + pir.Rows[0]["description"].ToString();
                            lblCurrency.Text = "Vendor Currency: " + pir.Rows[0]["crcy"].ToString();
                            lblCity.Text = "Vendor Country: " + pir.Rows[0]["cty"].ToString();
                            lblcry.Text = "(" + pir.Rows[0]["crcy"].ToString() + ")";


                        }
                    }
                }
                //End
            }


        }

        //Used this method by Raja
        private void GetMaterialDetailsbyQuoteDetails(string MaterialNo, string Product, string Plant, string MtlClass, string QuoteNo)
        {
            // Plant = "2100";
            //Product = "BO";
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
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
 " on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and  VP.Plant = TQ.Plant where TQ.QuoteNo='" + QuoteNo + "' and TB.FGcode = '" + MaterialNo + "' and (TQ.QuoteResponseduedate BETWEEN tc.ValidFrom and tc.ValidTO) ";

            da = new SqlDataAdapter(strGetData, consap);
            da.Fill(dtBOM);

            if (dtBOM.Rows.Count > 0)
            {
                dtMaterial = new DataTable();
                dtMaterial1 = new DataTable();
                foreach (DataRow row in dtBOM.Rows)
                {
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    //string str1 = "select top 1 tm.material,tm.MaterialDesc,tc.Unit,tc.Amount from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material where TB.FGcode='" + row.ItemArray[0].ToString() + "' ";
                    string str1 = "select distinct   tm.material,tm.MaterialDesc,tc.Unit,isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'1')) As decimal(20,2)),2) ,'1')as Amount,tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer inner join EMET.dbo.TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 inner join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tm.Plant = TQ.Plant where (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and TB.FGcode='" + row.ItemArray[0].ToString() + "' and tv.Vendor='" + vendorC.ToString() + "' ";
                    da1 = new SqlDataAdapter(str1, consap);
                    da1.Fill(dtMaterial);

                    GridView1.DataSource = dtMaterial;
                    GridView1.DataBind();

                }
                DtMaterialsDetails = dtMaterial;

            }
            else
            {
                DicQuoteDetails = new Dictionary<string, string>();
             
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    string str1 = "select distinct   tm.material,tm.MaterialDesc,tc.Unit,isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'1')) As decimal(20,2)),2) ,'1')as Amount,tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material inner join tVendor_New tv on tv.customerNo=tc.customer inner join EMET.dbo.TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 inner join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tm.Plant = TQ.Plant where (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) and TB.FGcode='" + 300208571 + "' and tv.Vendor='" + vendorC.ToString() + "' ";
                    da1 = new SqlDataAdapter(str1, consap);
                    da1.Fill(dtMaterial1);

                    GridView1.DataSource = dtMaterial1;
                    GridView1.DataBind();
                
            }
            if (DtMaterialsDetails == null)
                DtMaterialsDetails = new DataTable();
            //grdmatlcost.DataSource = DtMaterialsDetails;
            //grdmatlcost.DataBind();
            consap.Close();
        }

        private void GetQuoteandAllDetails(string QuoteNo)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select * from TQuoteDetails where QuoteNo='" + QuoteNo + "' and CreateStatus is not null ";
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


                DateTime dt = DateTime.Parse(dtdate.Rows[0].ItemArray[20].ToString());

                txtquotationDueDate.Text = dt.ToShortDateString();

                txtdrawng.Text = dtdate.Rows[0].ItemArray[19].ToString();

                txtprod.Text = dtdate.Rows[0].ItemArray[8].ToString();
                txtpartdesc.Text = Material + " - " + MaterialDesc;
                txtSAPJobType.Text = dtdate.Rows[0].ItemArray[16].ToString();
                txtPIRtype.Text = dtdate.Rows[0].ItemArray[12].ToString();
                txtprocs.Text = dtdate.Rows[0].ItemArray[13].ToString();

                txtPartUnit.Value = PartunitTxt.ToString();

                if (dtdate.Rows[0].ItemArray[21].ToString() != null && dtdate.Rows[0].ItemArray[21].ToString() != "")
                    TextBox1.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[21].ToString()).ToShortDateString();
                if (dtdate.Rows[0].ItemArray[22].ToString() != null && dtdate.Rows[0].ItemArray[22].ToString() != "")
                    txtfinal.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[22].ToString()).ToShortDateString();

                //GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass);
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
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProcessGrpCode", SqlDbType.NVarChar).Value = ProcessGrpCode;
            cmd.Parameters.Add("@FieldGroup", SqlDbType.NVarChar).Value = FieldGroup;

            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            if (FieldGroup == "MC")
            {
                DtDynamic = dt;
            }
            else if (FieldGroup == "PC")
            {
                DtDynamicProcessFields = dt;
            }
            else if (FieldGroup == "SMC")
            {
                DtDynamicSubMaterialsFields = dt;
            }
            else if (FieldGroup == "Unit")
            {
                DtDynamicUnitFields = dt;
            }
            else if (FieldGroup == "Others")
            {
                DtDynamicOtherCostsFields = dt;
            }


            //grdmatlcost.DataSource = DtMaterial;
            //grdmatlcost.DataBind();

            reader.Close();
            con.Close();


        }

        public void CreateDynamicTablebasedonProcessField()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select distinct FieldGroup from tMETFileds";
            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            foreach (DataRow row in dtdate.Rows)
            {
                GetMETfields(txtprocs.Text, row.ItemArray[0].ToString());
            }
            consap.Close();
        }

        private void CreateDynamicDT(int ColumnType)
        {
            if (ColumnType == 0)
            {
                int rowcount = 0;
                if (DtMaterialsDetails.Rows.Count > 0)
                {
                    TableRow Hearderrow = new TableRow();

                    Table1.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";

                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                        Hearderrow.ForeColor = Color.White;
                    }
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
                                tCell.Text = row.ItemArray[0].ToString();
                                tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Attributes.Add("disabled", "disabled");
                                tCell.Controls.Add(tb);
                                if (tb.ID.Contains("txtMaterialSAPCode"))
                                {
                                    tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                    // tb.Enabled = false;
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
                                    double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                    double rawperkg = RawVal / 1000;
                                    tb.Text = (rawperkg.ToString());
                                    // tb.Enabled = false;
                                    tb.Attributes.Add("disabled", "disabled");
                                }
                                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                    tb.Text = txtPartUnit.Value;
                                }

                                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                {
                                    tb.MaxLength = 4;
                                }

                                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
                                {

                                    tb.Attributes.Add("disabled", "disabled");
                                    tCell.Controls.Add(tb);
                                    Table1.Rows[cellCtr].Cells.Add(tCell);

                                }

                                if (txtPIRtype.Text.Contains("SUBCON"))
                                {


                                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    {
                                        tb.Text = "";
                                    }
                                    if (tb.ID.Contains("Cavity"))
                                    {
                                        //tb.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                }

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
                                tb.Attributes.Add("disabled", "disabled");
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
                }
                else
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    Table1.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                                lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString();
                                Table1.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");

                                tCell.Controls.Add(tb);


                                if (tb.ID.Contains("txtMaterialSAPCode"))
                                {
                                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                    // tb.Enabled = false;
                                    tb.Attributes.Add("disabled", "disabled");
                                }
                                else if (tb.ID.Contains("txtMaterialDescription"))
                                {
                                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                    //tb.Enabled = false;
                                    tb.Attributes.Add("disabled", "disabled");
                                }
                                else if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                {
                                    //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[3].ToString());

                                    //double rawperkg = RawVal / 1000;
                                    //tb.Text = (rawperkg.ToString());
                                    //// tb.Enabled = false;
                                    tb.Attributes.Add("disabled", "disabled");
                                }
                                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                    tb.Text = txtPartUnit.Value;
                                }

                                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
                                {
                                    tb.MaxLength = 4;
                                }

                                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)"))
                                {

                                    tb.Attributes.Add("disabled", "disabled");
                                    tCell.Controls.Add(tb);
                                    Table1.Rows[cellCtr].Cells.Add(tCell);

                                }

                                if (txtPIRtype.Text.Contains("SUBCON"))
                                {


                                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
                                    {
                                        tb.Text = "";
                                    }
                                    if (tb.ID.Contains("Cavity"))
                                    {
                                        //tb.Attributes.Add("disabled", "disabled");
                                    }
                                    else
                                    {
                                        tb.Attributes.Add("disabled", "disabled");
                                    }
                                }
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
                                tb.Attributes.Add("disabled", "disabled");
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
                }
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
                    ////else if (CellsCount ==  && tempSMClist.Count > 6)
                    ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                    //else if (CellsCount >= 3 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
                    //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamic.Rows.Count)).ToList();
                    //else if (CellsCount >= 4 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount == (i + 2))
                    //    TempMClistNew = TempMClistNew.Skip(((CellsCount) * (DtDynamic.Rows.Count))).ToList();
                    //else if (i >= 1 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
                    //    TempMClistNew = TempMClistNew.Skip(i * (DtDynamic.Rows.Count)).ToList();

                    //if (i == 1)
                    //    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();
                    //else

                    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

                    for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        if (cellCtr == 0)
                        {
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
                            // lb.Text = "Material Cost";
                            Table1.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");
                            if (tb.ID.Contains("RunnerWeight/shot(g)") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("PartNetUnitWeight(g)") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RecycleMaterialRatio(%)") || tb.ID.Contains("Cavity") || tb.ID.Contains("MaterialYield/MeltingLoss(%)"))
                            {
                                Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

                                ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
                                ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
                                if (tb.ID.Contains("PartNetUnitWeight(g)"))
                                {
                                    tb.Attributes.Add("disabled", "disabled");
                                }

                            }
                            else if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs"))
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

                            if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                            {
                                for (int ii = 0; ii < TempMClistNew.Count; ii++)
                                {
                                    if (ii == (cellCtr - 1))
                                    {

                                        tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
                                        break;
                                    }
                                }
                            }
                            tb.Attributes.Add("disabled", "disabled");


                        }
                    }
                }


                Session["Table"] = Table1;
                Table1.DataBind();
            }
            Session["Table"] = Table1;


        }

        private void CreateDynamicProcessDT(int ColumnType)
        {

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
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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
                            tCell.Text = row.ItemArray[0].ToString();
                            tRow.Cells.Add(tCell);
                        }
                        else
                        {

                            if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlProcess" + (cellCtr - 1);
                                ddl.DataSource = Session["process"];
                                ddl.DataTextField = "Process_Grp_code";
                                //ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged2;
                                ddl.DataValueField = "Process_Grp_code";
                                // ddl.da
                                ddl.DataBind();
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

                                ddl.Attributes.Add("disabled", "disabled");
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
                                                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                            break;
                                        }
                                    }
                                }
                                tCell.Controls.Add(ddl);
                            }
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlSubProcess" + (cellCtr - 1);


                                ddl.DataSource = Session["PSGroupwithUOM"];
                                ddl.DataTextField = "SubProcessName";

                                ddl.DataValueField = "SubProcessName";
                                ddl.DataBind();
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
                                ddl.Attributes.Add("disabled", "disabled");
                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {

                                            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                            {

                                                for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                                {
                                                    if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

                                                    }
                                                }

                                                var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

                                                if (ddlcheck1 != null)

                                                    ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                                ddl.SelectedValue = "";

                                            }

                                            break;

                                        }

                                    }
                                }

                                tCell.Controls.Add(ddl);
                            }
                            else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlMachineLabor" + (cellCtr - 1);
                                ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
                                ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
                                ddl.Style.Add("width", "142px");
                                ddl.Attributes.Add("disabled", "disabled");


                                DropDownList ddlhide = new DropDownList();
                                ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
                                ddlhide.Style.Add("display", "none");
                                ddlhide.Attributes.Add("disabled", "disabled");
                                ddlhide.Style.Add("width", "142px");
                                if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
                                {

                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                            {
                                                string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");

                                                var ddlcheck = ddl.Items.FindByText(txtddlTemp);
                                                if (ddlcheck != null)
                                                    ddl.Items.FindByText(txtddlTemp).Selected = true;
                                                break;
                                            }
                                            else if (tempPClist[2].ToString() != "" || tempPClist[2].ToString() != null)
                                            {
                                                ddl.Style.Add("display", "none");
                                                ddlhide.Style.Add("display", "block");
                                                break;
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
                                ddl.Style.Add("width", "142px");
                                ddl.DataSource = Session["MachineIDs"];
                                ddl.DataTextField = "MachineID";

                                ddl.DataValueField = "MachineID";
                                // ddl.da
                                ddl.DataBind();
                                // ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                ddl.Attributes.Add("disabled", "disabled");
                                TextBox tb = new TextBox();
                                tb.ID = "txtMachineId" + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Style.Add("display", "none");
                                tb.Attributes.Add("disabled", "disabled");
                                // style = "display: none;"
                                //tb.Visible = false;

                                DropDownList ddlMachide = new DropDownList();
                                ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
                                ddlMachide.Style.Add("width", "142px");
                                ddlMachide.Style.Add("display", "none");
                                ddlMachide.Attributes.Add("disabled", "disabled");


                                TextBox tbhide = new TextBox();
                                tbhide.ID = "txtHide" + (cellCtr - 1);
                                tbhide.Style.Add("display", "none");
                                tbhide.Attributes.Add("disabled", "disabled");

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                            {
                                                if (tempPClist[3].ToString() == "Machine")
                                                {
                                                    var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
                                                    if (ddlcheck != null)
                                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                    //ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
                                                    // ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "block");
                                                    // ddl.SelectedItem.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                }
                                                else if (tempPClist[3].ToString() == "Labor")
                                                {
                                                    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                }


                                                break;
                                            }
                                            else
                                            {
                                                if (tempPClist[3].ToString() == "Machine")
                                                {
                                                    //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                    ddlMachide.Style.Add("display", "block");
                                                    ddl.Style.Add("display", "none");
                                                    ddlMachide.Attributes.Add("disabled", "disabled");
                                                }
                                                else if (tempPClist[3].ToString() == "Labor")
                                                {
                                                    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }

                                                break;

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


                                tb.Attributes.Add("autocomplete", "off");

                                tb.Attributes.Add("disabled", "disabled");

                                if (tb.ID.Contains("txtStandardRate/HR"))
                                {
                                    grdMachinelisthidden.DataSource = Session["MachineListGrd"];
                                    grdMachinelisthidden.DataBind();
                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                    {

                                        if (firsttimeload == 0)
                                        {
                                            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                            firsttimeload = 1;
                                        }
                                        tb.Attributes.Add("disabled", "disabled");
                                        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                        {

                                        }

                                    }

                                }

                                if (tb.ID.Contains("txtVendorRate"))
                                {

                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                    {

                                        if (firsttimeload == 0)
                                        {
                                            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                            firsttimeload = 1;
                                        }

                                        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }


                                    }

                                }

                                if (tb.ID.Contains("txtProcessUOM"))
                                {

                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                    {
                                        if (grdProcessGrphidden.Rows[h].Cells[0].Text.Contains(txtprocs.Text + " -"))
                                        {
                                            if (firsttimeload == 0)
                                            {
                                                tb.Text = grdProcessGrphidden.Rows[h].Cells[2].Text;
                                                firsttimeload = 1;
                                            }

                                        }
                                    }
                                }

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
                                            {
                                                if ((tb.ID.Contains("ProcessCost/pc")))

                                                { tb.Attributes.Add("disabled", "disabled"); }

                                            }

                                            else
                                            {
                                            }
                                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                tb.Attributes.Add("disabled", "disabled");
                                tCell.Controls.Add(tb);
                            }


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

                    //if (CellsCount == 2 && tempPclist.Count <= (DtDynamicProcessFields.Rows.Count + 1))
                    //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i)) * (DtDynamicProcessFields.Rows.Count + 1))).ToList();
                    ////else if (CellsCount ==  && tempSMClist.Count > 6)
                    ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
                    //else if (CellsCount == 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
                    //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i + 1)) * DtDynamicProcessFields.Rows.Count)).ToList();
                    //else if (CellsCount >= 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount == (i + 2))
                    //    TempPclistNew = TempPclistNew.Skip(((CellsCount) * (DtDynamicProcessFields.Rows.Count))).ToList();
                    //else if (i >= 1 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
                    //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();

                    //if(i == 1)
                    //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count + 1)).ToList();
                    //else

                    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();


                    int rowcount = 0;
                    for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        if (cellCtr == 0)
                        {
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
                            // lb.Text = "Material Cost";
                            TablePC.Rows[cellCtr].Cells.Add(tCell);
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

                                ddl.DataBind();
                                // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged3;
                                ddl.Items.Insert(0, new ListItem("--Select--", "Select"));


                                ddl.Attributes.Add("disabled", "disabled");
                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    // var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == rowcount - 1)
                                        {

                                            if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                            {
                                                // ddl.SelectedValue = grdProcessGrphidden.Rows[h].Cells[0].Text;

                                                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;

                                                // ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                // ddl.Style.Add("display", "none");
                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                            }
                                            // ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                //else
                                //{
                                //    ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
                                //}

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
                                // ddl.da
                                ddl.DataBind();
                                // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged4;
                                ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));

                                ddl.Attributes.Add("disabled", "disabled");

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    // var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == rowcount - 1)
                                        {

                                            if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                            {
                                                // ddl.Style.Add("display", "none");
                                                // ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");

                                                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));

                                                if (ddlcheck != null)

                                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;

                                                // ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;

                                                for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                                {
                                                    if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(TempPclistNew[ii].ToString().Replace("NaN", "")))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

                                                    }
                                                }

                                            }
                                            else
                                            {
                                                ddl.Attributes.Add("disabled", "disabled");
                                                ddl.SelectedValue = "";

                                            }

                                            break;
                                        }

                                    }
                                }
                                //else

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

                                DropDownList ddlhide = new DropDownList();
                                ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
                                ddlhide.Style.Add("display", "none");
                                ddlhide.Attributes.Add("disabled", "disabled");
                                ddlhide.Style.Add("width", "142px");

                                ddl.Attributes.Add("disabled", "disabled");
                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {


                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == rowcount - 1)
                                        {
                                            if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                            {
                                                string txtddlTemp = TempPclistNew[ii].ToString().Replace("NaN", "");

                                                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                if (ddlcheck != null)
                                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                break;
                                            }
                                            else if (TempPclistNew[2].ToString() != "" || TempPclistNew[2].ToString() != null)
                                            {
                                                //ddlhide.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                ddl.Style.Add("display", "none");
                                                ddlhide.Style.Add("display", "block");
                                                break;
                                            }
                                        }

                                    }
                                }

                                ddl.Attributes.Add("disabled", "disabled");
                                ddlhide.Attributes.Add("disabled", "disabled");

                                tCell.Controls.Add(ddl);
                                tCell.Controls.Add(ddlhide);
                                TablePC.Rows[cellCtr].Cells.Add(tCell);
                            }

                            else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
                            {
                                DropDownList ddl = new DropDownList();
                                ddl.ID = "dynamicddlMachine" + (i);
                                ddl.Style.Add("width", "142px");
                                ddl.DataSource = Session["MachineIDs"];
                                ddl.DataTextField = "MachineID";

                                ddl.DataValueField = "MachineID";
                                // ddl.da
                                ddl.DataBind();

                                //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));


                                TextBox tb = new TextBox();
                                tb.ID = "txtMachineId" + (i);
                                tb.Style.Add("display", "none");
                                tb.Attributes.Add("autocomplete", "off");

                                DropDownList ddlMachide = new DropDownList();
                                ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
                                ddlMachide.Style.Add("width", "142px");
                                ddlMachide.Style.Add("display", "none");
                                ddlMachide.Attributes.Add("disabled", "disabled");


                                TextBox tbhide = new TextBox();
                                tbhide.ID = "txtHide" + (cellCtr - 1);
                                tbhide.Style.Add("display", "none");
                                tbhide.Attributes.Add("disabled", "disabled");


                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {

                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == rowcount - 1)
                                        {
                                            if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                            {
                                                if (TempPclistNew[3].ToString() == "Machine")
                                                {
                                                    // ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "block");

                                                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
                                                    if (ddlcheck != null)
                                                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;

                                                    // ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
                                                }
                                                else if (TempPclistNew[3].ToString() == "Labor")
                                                {
                                                    tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                }


                                                break;
                                            }
                                            else
                                            {
                                                if (TempPclistNew[3].ToString() == "Machine")
                                                {
                                                    //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
                                                    ddlMachide.Style.Add("display", "block");
                                                    ddl.Style.Add("display", "none");
                                                    ddlMachide.Attributes.Add("disabled", "disabled");
                                                }
                                                else if (TempPclistNew[3].ToString() == "Labor")
                                                {
                                                    tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                                    ddl.Style.Add("display", "none");
                                                    tb.Style.Add("display", "block");
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }

                                                break;

                                            }
                                        }

                                    }
                                }

                                ddl.Attributes.Add("disabled", "disabled");
                                tb.Attributes.Add("disabled", "disabled");
                                ddlMachide.Attributes.Add("disabled", "disabled");
                                tbhide.Attributes.Add("disabled", "disabled");

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
                                tb.Attributes.Add("disabled", "disabled");
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
                                //if ( tb.ID.Contains("TotalProcessesCost/pcs") || tb.ID.Contains("ProcessUOM"))
                                //{
                                //    if (tb.ID.Contains("DurationperProcessUOM(Sec)"))
                                //    {

                                //    }
                                //    else
                                //        tb.Attributes.Add("disabled", "disabled");
                                //}

                                if (tb.ID.Contains("txtStandardRate/HR"))
                                {

                                    grdMachinelisthidden.DataSource = Session["MachineListGrd"];
                                    grdMachinelisthidden.DataBind();
                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                    {

                                        if (firsttimeload == 0)
                                        {
                                            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                            firsttimeload = 1;
                                        }
                                        tb.Attributes.Add("disabled", "disabled");
                                        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                        {

                                        }


                                    }
                                }

                                if (tb.ID.Contains("txtVendorRate"))
                                {

                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdMachinelisthidden.Rows.Count; h++)
                                    {

                                        if (firsttimeload == 0)
                                        {
                                            tb.Text = grdMachinelisthidden.Rows[h].Cells[1].Text;
                                            firsttimeload = 1;
                                        }

                                        if (grdMachinelisthidden.Rows[h].Cells[2].Text == "Y")
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }


                                    }
                                }


                                if (tb.ID.Contains("txtProcessUOM"))
                                {

                                    int firsttimeload = 0;
                                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
                                    {
                                        if (grdProcessGrphidden.Rows[h].Cells[0].Text.Contains(txtprocs.Text + " -"))
                                        {
                                            if (firsttimeload == 0)
                                            {
                                                tb.Text = grdProcessGrphidden.Rows[h].Cells[2].Text;
                                                firsttimeload = 1;
                                            }

                                        }
                                    }
                                }

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    //var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == (rowcount - 1))
                                        {

                                            if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
                                            {


                                            }

                                            else
                                            {
                                                if ((tb.ID.Contains("txtProcessCost/pc")) || (tb.ID.Contains("txtIfTurnkey-VendorName")))
                                                {
                                                }
                                                else
                                                {
                                                    tb.Attributes.Add("disabled", "disabled");
                                                }

                                            }
                                            tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }

                                    if (TempPclistNew.Count == 0)
                                    {
                                        if ((tb.ID.Contains("ProcessCost/pc")))
                                        {
                                            tb.Attributes.Add("disabled", "disabled");
                                        }
                                    }
                                }


                            }


                        }
                        rowcount++;
                    }

                }

                Session["TablePc"] = TablePC;
            }


        }

        private void CreateDynamicSubMaterialDT(int ColumnType)
        {
            if (ColumnType == 0)
            {
                int rowcount = 0;
                if (DtDynamicSubMaterialsDetails == null)
                    DtDynamicSubMaterialsDetails = new DataTable();
                if (DtDynamicSubMaterialsDetails.Rows.Count > 0)
                {
                    TableRow Hearderrow = new TableRow();

                    TableSMC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                        Hearderrow.ForeColor = Color.White;
                    }

                    foreach (DataRow row in DtDynamicSubMaterialsFields.Rows)
                    {
                        TableRow tRow = new TableRow();
                        TableSMC.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                tCell.Text = row.ItemArray[0].ToString();
                                tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                tb.Attributes.Add("disabled", "disabled");
                                tCell.Controls.Add(tb);
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
                }
                else
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
                            tCell1.Text = "Field Name";
                        //else
                        //    tCell1.Text = "Material Cost";

                        Hearderrow.Cells.Add(tCell1);
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
                                lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString();
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                tb.Attributes.Add("disabled", "disabled");
                                tb.Attributes.Add("autocomplete", "off");


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
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
                            // lb.Text = "Material Cost";
                            TableSMC.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");
                            tb.Attributes.Add("disabled", "disabled");
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
                            if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                            {

                                for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                {

                                    if (ii == (cellCtr - 1))
                                    {

                                        tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                Session["TableSMC"] = TableSMC;
            }

            //  var Ss = divhdnSMC.InnerHtml;






        }

        private void CreateDynamicOthersCostDT(int ColumnType)
        {
            if (ColumnType == 0)
            {
                int rowcount = 0;
                if (DtDynamicOtherCostsDetails == null)
                    DtDynamicOtherCostsDetails = new DataTable();
                if (DtDynamicOtherCostsDetails.Rows.Count > 0)
                {
                    TableRow Hearderrow = new TableRow();

                    TableOthers.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        //else
                        //    tCell1.Text = "Material Cost";

                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                        Hearderrow.ForeColor = Color.White;
                    }

                    foreach (DataRow row in DtDynamicOtherCostsFields.Rows)
                    {
                        TableRow tRow = new TableRow();
                        TableOthers.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                tCell.Text = row.ItemArray[0].ToString();
                                tRow.Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                //{

                                tb.Attributes.Add("disabled", "disabled");
                                //}

                                // Data Store and Retrieve
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }


                                tCell.Controls.Add(tb);
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
                }
                else
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableOthers.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        //else
                        //    tCell1.Text = "Material Cost";

                        Hearderrow.Cells.Add(tCell1);
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
                                lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString();
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.BorderStyle = BorderStyle.None;
                                tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
                                tb.Attributes.Add("autocomplete", "off");
                                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
                                //{

                                tb.Attributes.Add("disabled", "disabled");
                                //}



                                // Data Store and Retrieve
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
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
                }
                // TableMat = Table1;
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
                            Label lb = new Label();
                            tCell.Controls.Add(lb);
                            // lb.Text = "Material Cost";
                            TableOthers.Rows[cellCtr].Cells.Add(tCell);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.BorderStyle = BorderStyle.None;
                            tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                            tb.Attributes.Add("autocomplete", "off");
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
                            tb.Attributes.Add("disabled", "disabled");


                            // Data Retrieve and assign from Storage
                            if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                            {
                                for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                {
                                    if (ii == (cellCtr - 1))
                                    {

                                        tb.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
                Session["TableOthers"] = TableOthers;
            }

        }


        private void CreateDynamicUnitDT(int ColumnType)
        {

            if (ColumnType == 0)
            {
                int rowcount = 0;
                if (DtDynamicUnitDetails == null)
                    DtDynamicUnitDetails = new DataTable();
                if (DtDynamicUnitDetails.Rows.Count > 0)
                {
                    TableRow Hearderrow = new TableRow();

                    TableUnit.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        //else
                        //    tCell1.Text = "Material Cost";

                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                        Hearderrow.ForeColor = Color.White;
                    }

                    foreach (DataRow row in DtDynamicUnitFields.Rows)
                    {
                        TableRow tRow = new TableRow();
                        TableUnit.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                Label lb = new Label();
                                tCell.Controls.Add(lb);
                                tCell.Text = row.ItemArray[0].ToString();
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
                                tCell.Controls.Add(tb);
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
                }
                else
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
                            tCell1.Text = "Final Quote Price/pcs";
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
                                    lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString();
                                    lb.Width = 180;
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
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

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
                                    tb.Style.Add("text-transform", "uppercase");
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");



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
                                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

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
                                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
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
                }
                // TableMat = Table1;
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
                        tb.Style.Add("text-transform", "uppercase");
                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
                        tCell.Controls.Add(tb);
                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    }
                }


                Session["TableUnit"] = TableUnit;
            }

        }

        //old
        //private void CreateDynamicUnitDT(int ColumnType)
        //{

        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;
        //        if (DtDynamicUnitDetails == null)
        //            DtDynamicUnitDetails = new DataTable();
        //        if (DtDynamicUnitDetails.Rows.Count > 0)
        //        {
        //            TableRow Hearderrow = new TableRow();

        //            TableUnit.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                //else
        //                //    tCell1.Text = "Material Cost";

        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            foreach (DataRow row in DtDynamicUnitFields.Rows)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableUnit.Rows.Add(tRow);
        //                for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
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


        //                        tCell.Controls.Add(tb);
        //                        tRow.Cells.Add(tCell);
        //                    }
        //                }

        //                if (rowcount % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#284775");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
        //                }
        //                rowcount++;
        //            }
        //        }
        //        else
        //        {
        //            int rowcountnew = 0;

        //            TableRow Hearderrow = new TableRow();

        //            TableUnit.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                //else
        //                //    tCell1.Text = "Material Cost";

        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            // Table1 = (Table)Session["Table"];
        //            for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableUnit.Rows.Add(tRow);

        //                for (int i = 0; i <= 1; i++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (i == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString();
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);


        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                            for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                            {
        //                                if (ii == (cellCtr - 1))
        //                                {
        //                                    tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                    break;
        //                                }

        //                            }
        //                        }


        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");


        //                        tb.Attributes.Add("disabled", "disabled");


        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //                if (rowcountnew % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#284775");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#F7F6F3");
        //                }
        //                rowcountnew++;
        //            }
        //        }
        //        // TableMat = Table1;
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
        //                tCell.Controls.Add(tb);
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //        }


        //        Session["TableUnit"] = TableUnit;
        //    }

        //}

        private void GetProcessDetailsbyQuoteDetails(string ProcessGrp)
        {
            // plant = "2100";
            // Product = "BO";
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();

            DataTable dtProGrp = new DataTable();
            DataTable dtVendorRate = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter();
            string str = "Select distinct ProcessGrpCode from TPROCESGROUP_SUBPROCESS Where ProcessGrpCode = '" + ProcessGrp + "' ";
            da = new SqlDataAdapter(str, consap);
            da.Fill(dtProGrp);


            DtDynamicProcessCostsDetails = dtProGrp;



        }


        private void GetProcessDetailsbyQuoteDetailsWithNoGroup()
        {
            // plant = "2100";
            // Product = "BO";
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();

            DataTable dtProGrp = new DataTable();
            DataTable dtVendorRate = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter();
            string str = "Select CONCAT(ProcessGrpCode , ' - ', ProcessGrpDescription) as ProcessGrpCode, SubProcessName,ProcessUomDescription,ProcessUOM from TPROCESGROUP_SUBPROCESS ";
            da = new SqlDataAdapter(str, consap);
            da.Fill(dtProGrp);

            grdProcessGrphidden.DataSource = dtProGrp;
            grdProcessGrphidden.DataBind();
            Session["PSGroupwithUOM"] = grdProcessGrphidden.DataSource;




            //DtDynamicProcessCostsDetails = dtProGrp;

        }

        // Till this used


        protected void GetData(string reqno)
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();

            DataTable dtget = new DataTable();

            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string strGetData = string.Empty;


            strGetData = "select CONVERT(VARCHAR(10), s.RequestDate, 103) as 'RequestDate',S.QuoteNo,V.Description,v.Crcy,vp.PICName,vp.PICemail from tVendor_New as V inner join TVENDORPIC as VP" +
                          " on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails as S on S.VendorCode1=v.Vendor where S.QuoteNo='" + reqno + "' ";
            da = new SqlDataAdapter(strGetData, con);
            da.Fill(dtget);

            if (dtget.Rows.Count > 0)
            {
                grdVendrDet.DataSource = dtget;
                grdVendrDet.DataBind();

                hdnQuoteNo.Value = dtget.Rows[0].ItemArray[1].ToString();

            }

            con.Close();
        }

        private void GetSHMNPICDetails(string userdet)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            // string str = "select  sp.PIC1Name as UseNam,sp.PIC1Email as UseEmail from Usr Inner join TSMNProductPIC sp on Usr.UseID = '" + userdet.Trim() + "'";
            string str1 = "select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product from TSMNProductPIC pp inner join EMET.dbo.TQuotedetails TQ on pp.Product = TQ.Product and pp.Userid = Tq.CreatedBy and PP.Product = TQ.Product where QuoteNo='" + QuoteNo + "' ";
            da = new SqlDataAdapter(str1, consap);
            da.Fill(dtdate);
            if (dtdate.Rows.Count > 0)
            {
                txtsmnpic.Text = dtdate.Rows[0]["PICName"].ToString();
                txtemail.Text = dtdate.Rows[0]["PICEmail"].ToString();
            }

            consap.Close();


        }


        protected void process()

        {
            ddlProcess = new DropDownList();
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select distinct CONCAT( tp.Process_Grp_code, ' - ', tp.Process_Grp_Description) as Process_Grp_code from TPROCESGROUP_LIST TP inner join TPROCESGROUP_SUBPROCESS TPS on Tp.Process_Grp_code = tps.ProcessGrpCode";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            ddlProcess.DataSource = Result;
            ddlProcess.DataTextField = "Process_Grp_code";
            ddlProcess.DataValueField = "Process_Grp_code";
            ddlProcess.DataBind();


            Session["process"] = ddlProcess.DataSource;

            DataTable Result1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter();
            string str1 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as Machine, CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,TVM.FollowStdRate as FollowStdRate,TVM.Currency as Currency from TVENDORMACHNLIST TVM where TVM.VendorCode = '" + UserId + "' ";
            da1 = new SqlDataAdapter(str1, con1);
            Result1 = new DataTable();
            da1.Fill(Result1);

            grdMachinelisthidden.DataSource = Result1;
            grdMachinelisthidden.DataBind();
            Session["MachineListGrd"] = grdMachinelisthidden.DataSource;


            DataTable Result3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter();
            string str3 = "select CAST(ROUND(StdLabourRateHr,2) AS DECIMAL(12,2))as 'StdLabourRateHr',FollowStdRate,Currency from TVENDORLABRCOST TVC Where TVC.Vendorcode = '" + UserId + "'";
            da3 = new SqlDataAdapter(str3, con1);
            Result3 = new DataTable();
            da3.Fill(Result3);

            grdLaborlisthidden.DataSource = Result3;
            grdLaborlisthidden.DataBind();
            Session["LaborListGrd"] = grdLaborlisthidden.DataSource;

            DropDownList ddlMachine = new DropDownList();

            DataTable Result2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter();
            string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM inner join EMET.dbo.TQuoteDetails TQ on TVM.VendorCode = TQ.VendorCode1 Where TQ.QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da2 = new SqlDataAdapter(str2, con1);
            Result2 = new DataTable();
            da2.Fill(Result2);

            ddlMachine.DataSource = Result2;
            ddlMachine.DataBind();
            Session["MachineIDs"] = ddlMachine.DataSource;




            con1.Close();

        }

        private void SaveallCostDetails()
        {
            #region other values
            if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
            {
                var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

                var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

                for (int i = 1; i <= ccc; i++)
                {
                    var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                    var tempOtherlistNew = tempOtherlist;
                    if (i > 1)
                        tempOtherlistNew = tempOtherlistNew.Skip(((i - 1) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();

                    var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                    SqlConnection conins;
                    conins = new SqlConnection(connetionStringdate);
                    conins.Open();

                    string query = "insert into [dbo].[TOtherCostDetails] (QuoteNo,ProcessGroup,ItemsDescription,[OtherItemCost/pcs],[TotalOtherItemCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@TotalCost,@RowId,@By,@On)";

                    string IDesc = tempOtherlistNew[0].ToString().Replace("NaN", "");
                    string ICost = tempOtherlistNew[1].ToString();
                    string ItotalCost = tempOtherlistNew[2].ToString();


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

            #endregion other values

            #region SMC Values
            if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
            {
                var tempMSClistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                var ccc = tempMSClistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                for (int i = 1; i <= ccc; i++)
                {
                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                    var tempSMClistNew = tempSMClist;
                    if (i > 1)
                        tempSMClistNew = tempSMClistNew.Skip(((i - 1) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();

                    var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                    SqlConnection conins;
                    conins = new SqlConnection(connetionStringdate);
                    conins.Open();

                    string query = "insert into [dbo].[TSMCCostDetails] (QuoteNo,ProcessGroup,[Sub-Mat/T&JDescription],[Sub-Mat/T&JCost],[Consumption(pcs)],[Sub-Mat/T&JCost/pcs],[TotalSub-Mat/T&JCost/pcs],RowId,UpdatedBy,UpdatedOn) VALUES (@QuoteNo,@PG,@Desc,@Cost,@IConsumption,@ICostpcs,@ItotalCost,@RowId,@By,@On)";

                    string IDesc = tempSMClistNew[0].ToString().Replace("NaN", "");
                    string ICost = tempSMClistNew[1].ToString();
                    string IConsumption = tempSMClistNew[2].ToString();
                    string ICostpcs = tempSMClistNew[3].ToString();
                    string ItotalCost = tempSMClistNew[4].ToString();


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

            #endregion SMC Values

            #region Process Values
            if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
            {
                var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();

                var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;

                for (int i = 1; i <= ccc; i++)
                {
                    var tempProcesslist = hdnProcessValues.Value.ToString().Split(',').ToList();

                    var tempProcesslistNew = tempProcesslist;
                    if (i > 1)
                        tempProcesslistNew = tempProcesslistNew.Skip(((i - 1) * (DtDynamicProcessFields.Rows.Count))).ToList();

                    var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                    SqlConnection conins;
                    conins = new SqlConnection(connetionStringdate);
                    conins.Open();

                    string IProc = tempProcesslistNew[0].ToString().Replace("NaN", "");
                    string ISubProc = tempProcesslistNew[1].ToString();
                    string ITurnKey = tempProcesslistNew[2].ToString();
                    string IMachineLabor = tempProcesslistNew[3].ToString();
                    string IMachine = tempProcesslistNew[4].ToString();
                    string IStRate = tempProcesslistNew[5].ToString().Replace("NaN", "");
                    string IVendorRate = tempProcesslistNew[6].ToString();
                    string IProcUOM = tempProcesslistNew[7].ToString();
                    string IBaseQty = tempProcesslistNew[8].ToString();
                    string IDPUOM = tempProcesslistNew[9].ToString();
                    string Iyield = tempProcesslistNew[10].ToString();
                    string ICostPc = tempProcesslistNew[11].ToString();
                    string ITotalCost = tempProcesslistNew[12].ToString();

                    string query = "insert into [dbo].[TProcessCostDetails] (QuoteNo,ProcessGroup,[ProcessGrpCode],[SubProcess],[IfTurnkey-VendorName],[Machine/Labor],[Machine],[StandardRate/HR] ,[VendorRate],[ProcessUOM],[Baseqty],[DurationperProcessUOM(Sec)],[Efficiency/ProcessYield(%)],[ProcessCost/pc],[TotalProcessesCost/pcs],RowId,UpdatedBy,UpdatedOn) "
                        + "VALUES (@QuoteNo,@PG,@IProc,@ISubProc,@ITurnKey,@IMachineLabor,@IMachine,@IStRate,@IVendorRate,@IProcUOM,@IBaseQty,@IDPUOM,@Iyield,@ICostPc,@ITotalCost, @RowId,@By,@On)";

                    SqlCommand cmd1 = new SqlCommand(query, conins);
                    cmd1.Parameters.AddWithValue("@QuoteNo", hdnQuoteNo.Value);
                    cmd1.Parameters.AddWithValue("@PG", txtprocs.Text.ToString());

                    cmd1.Parameters.AddWithValue("@IProc", IProc);
                    cmd1.Parameters.AddWithValue("@ISubProc", ISubProc);
                    cmd1.Parameters.AddWithValue("@ITurnKey", ITurnKey);
                    cmd1.Parameters.AddWithValue("@IMachineLabor", IMachineLabor);
                    cmd1.Parameters.AddWithValue("@IMachine", IMachine);
                    cmd1.Parameters.AddWithValue("@IStRate", IStRate);
                    cmd1.Parameters.AddWithValue("@IVendorRate", IVendorRate);
                    cmd1.Parameters.AddWithValue("@IProcUOM", IProcUOM);
                    cmd1.Parameters.AddWithValue("@IBaseQty", IBaseQty);
                    cmd1.Parameters.AddWithValue("@IDPUOM", IDPUOM);
                    cmd1.Parameters.AddWithValue("@Iyield", Iyield);
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

            #endregion Process Values

            #region Material Cost Values
            if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
            {
                var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();

                var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                for (int i = 1; i <= ccc; i++)
                {
                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                    var tempMClistNew = tempMClist;
                    if (i > 1)
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

                    #region IM
                    if (txtprocs.Text == "IM")
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

                    #region CA or SPR
                    else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                    {
                        strCavity = tempMClistNew[5].ToString();
                        strMLoss = tempMClistNew[6].ToString();
                        strMCrossWeight = tempMClistNew[7].ToString();

                        strMCostpcs = tempMClistNew[8].ToString();
                        strTotalcostpcs = tempMClistNew[9].ToString();
                    }

                    #endregion CA or SPR

                    #region ST
                    else if (txtprocs.Text == "ST")
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

                    #region MS 
                    else if (txtprocs.Text == "MS")
                    {
                        strDiaID = tempMClistNew[5].ToString();
                        strDiaOD = tempMClistNew[6].ToString();
                        strWidth = tempMClistNew[7].ToString();

                        strCavity = tempMClistNew[8].ToString();
                        strMLoss = tempMClistNew[9].ToString();
                        strMCrossWeight = tempMClistNew[10].ToString();

                        strMCostpcs = tempMClistNew[11].ToString();
                        strTotalcostpcs = tempMClistNew[12].ToString();
                    }
                    #endregion MS

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

            #endregion Process Values

        }

        private void RetrieveAllCostDetails()
        {
            var connetionString = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();

            DataTable dtget = new DataTable();

            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();


            #region Other Cost

            string strGetData = "";

            strGetData = "SELECT * FROM TOtherCostDetails Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da = new SqlDataAdapter(strGetData, con);
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
            strsub = "SELECT * FROM TSMCCostDetails Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da = new SqlDataAdapter(strsub, con);
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

            #region Process Cost

            string strProcess = "";
            dtget = new DataTable();
            strProcess = "SELECT * FROM TProcessCostDetails Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da = new SqlDataAdapter(strProcess, con);
            da.Fill(dtget);

            StringBuilder sbProcess = new StringBuilder();

            if (dtget.Rows.Count > 0)
            {
                for (var i = 0; i < dtget.Rows.Count; i++)
                {
                    var txtProc = dtget.Rows[i].ItemArray[2].ToString();
                    var txtsubProc = dtget.Rows[i].ItemArray[3].ToString();
                    var txtturnkey = dtget.Rows[i].ItemArray[4].ToString();
                    var txtML = dtget.Rows[i].ItemArray[5].ToString();
                    var txtMachine = dtget.Rows[i].ItemArray[6].ToString();

                    var txtstanRate = dtget.Rows[i].ItemArray[7].ToString();
                    var txtvendorRate = dtget.Rows[i].ItemArray[8].ToString();
                    var txtProcUOM = dtget.Rows[i].ItemArray[9].ToString();
                    var txtBaseQty = dtget.Rows[i].ItemArray[10].ToString();
                    var txtDPUOM = dtget.Rows[i].ItemArray[11].ToString();

                    var txtProcYeild = dtget.Rows[i].ItemArray[12].ToString();
                    var txtProcCost = dtget.Rows[i].ItemArray[13].ToString();
                    var txtProcTCost = dtget.Rows[i].ItemArray[14].ToString();

                    sbProcess.Append(txtProc + "," + txtsubProc + "," + txtturnkey + "," + txtML + "," + txtMachine + "," + txtstanRate + "," + txtvendorRate + "," + txtProcUOM + "," + txtBaseQty + "," + txtDPUOM + "," + txtProcYeild + "," + txtProcCost + "," + txtProcTCost + ",");
                }
                hdnProcessValues.Value = sbProcess.ToString();
            }

            #endregion SubMat Cost


            #region MC Cost
            dtget = new DataTable();
            string strMCIM = "";

            strMCIM = "SELECT * FROM TMCCostDetails Where  QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da = new SqlDataAdapter(strMCIM, con);
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
                    strRawCost = dtget.Rows[i].ItemArray[4].ToString();
                    strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString();
                    strPartUnitW = dtget.Rows[i].ItemArray[6].ToString();

                    #region IM
                    if (txtprocs.Text == "IM")
                    {
                        strRunnerWeight = dtget.Rows[i].ItemArray[14].ToString();
                        strRunnerRatio = dtget.Rows[i].ItemArray[15].ToString();
                        strRecycle = dtget.Rows[i].ItemArray[16].ToString();

                        strCavity = dtget.Rows[i].ItemArray[16].ToString();
                        strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                        strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                        strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                        strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                        sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strRunnerWeight + "," + strRunnerRatio + "," + strRecycle + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                    }
                    #endregion IM

                    #region CA or SPRT
                    else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                    {
                        strCavity = dtget.Rows[i].ItemArray[16].ToString();
                        strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                        strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                        strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                        strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                        sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                    }
                    #endregion CA or SPR

                    #region ST
                    else if (txtprocs.Text == "ST")
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

                    #region MS 
                    else if (txtprocs.Text == "MS")
                    {
                        strDiaID = dtget.Rows[i].ItemArray[7].ToString();
                        strDiaOD = dtget.Rows[i].ItemArray[8].ToString();
                        strWidth = dtget.Rows[i].ItemArray[10].ToString();

                        strCavity = dtget.Rows[i].ItemArray[16].ToString();
                        strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                        strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                        strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                        strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                        sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                    }
                    #endregion MS

                }
                hdnMCTableValues.Value = sbMCIM.ToString();


            }

            #endregion MC Cost

            #region Unit Cost
            dtget = new DataTable();
            string strUnitData = "";

            strUnitData = "select TotalMaterialCost,TotalSubMaterialCost,TotalProcessCost,TotalOtheritemsCost,GrandTotalCost,Profit,Discount,FinalQuotePrice from TQuoteDetails  Where QuoteNo = '" + hdnQuoteNo.Value.ToString() + "'";
            da = new SqlDataAdapter(strUnitData, con);
            da.Fill(dtget);

            StringBuilder sbUnit = new StringBuilder();

            //string hdnUnittemp = "";
            if (dtget.Rows.Count > 0)
            {
                for (var i = 0; i < dtget.Rows.Count; i++)
                {
                    var txtTMCost = dtget.Rows[i].ItemArray[0].ToString();
                    var txtTPCost = dtget.Rows[i].ItemArray[2].ToString();
                    var txtTSCost = dtget.Rows[i].ItemArray[1].ToString();
                    var txtTOCost = dtget.Rows[i].ItemArray[3].ToString();
                    var txtGrantCost = dtget.Rows[i].ItemArray[4].ToString();
                    var txtProfit = dtget.Rows[i].ItemArray[5].ToString();
                    var txtDiscount = dtget.Rows[i].ItemArray[6].ToString();
                    var txtfinalCost = dtget.Rows[i].ItemArray[7].ToString();

                    sbUnit.Append(txtTMCost + "," + txtTPCost + "," + txtTSCost + "," + txtTOCost + "," + txtGrantCost + "," + txtProfit + "," + txtDiscount + "," + txtfinalCost + ",");
                    //hdnUnittemp = (txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");

                    hdnProfit.Value = txtProfit;
                    hdnDiscount.Value = txtDiscount;

                    hdnTMatCost.Value = txtTMCost;
                    hdnTProCost.Value = txtTPCost;
                    hdnTSumMatCost.Value = txtTSCost;
                    hdnTOtherCost.Value = txtTOCost;
                    hdnTGTotal.Value = txtGrantCost;
                    hdnTFinalQPrice.Value = txtfinalCost;
                }

                //hdnSMCTableValues.Value = hdnvaltempNew.ToString();
                hdnUnitValues.Value = sbUnit.ToString();
            }

            #endregion Other Cost

            con.Close();


        }


    }
}