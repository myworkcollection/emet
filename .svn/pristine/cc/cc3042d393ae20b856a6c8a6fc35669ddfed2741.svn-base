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
    public partial class NewRequest_do : System.Web.UI.Page
    {

        CheckBox headerCheckBox = new CheckBox();
        int incNumber = 000000;


        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string Userid;
        public static string password;
        public static string domain;
        public static string path;
        public static string SendFilename;
        public static Dictionary<int, string> objPirType;
        public static string userId;
        public static string nameC;
        public static string aemail;
        public static string pemail;
        public static string Uemail;
        public static string body1;
        public static string userId1;
        protected void Page_Load(object sender, EventArgs e)
        {
            getdate();
            this.article.Checked = true;
            string qq = Request.QueryString["num"];
            if (!IsPostBack)
            {
                userId = Session["userID"].ToString();
                userId1 = Session["userID"].ToString();
                string sname = Session["UserName"].ToString();
                nameC = sname;
                string srole = Session["userType"].ToString();
                string concat = sname + " - " + srole;

                HttpContext.Current.Session["splproctype"] = null;
                HttpContext.Current.Session["prod_code"] = null;
                HttpContext.Current.Session["matlclass"] = null;
                HttpContext.Current.Session["plant_status"] = null;
                HttpContext.Current.Session["MaterialType"] = null;
                HttpContext.Current.Session["proctype"] = null;
                HttpContext.Current.Session["splproctype"] = null;
                //lblUser1.Text = concat;
                lbluser1.Text = sname;
                lblplant.Text = srole;
                // Session["UserName"] = userId;
                // GridView2.Visible = false;
                objPirType = new Dictionary<int, string>();
                // Session["firstarticle"] = article.Checked.ToString();
                this.article.Checked = true;

                getdate();
                getplant();
                matltype();
                getproctype();

                GetSHMNPIC();
                GetPirType();
                //  Getproduct();
                this.process();
                DeleteNonRequest();
                lblMessage.Text = "";
            }

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                //Session["FileUpload1"] = FileUpload1;
                //lblMessage.Text = FileUpload1.FileName; // get the name 
            }
            // This condition will occur on next postbacks        
            else if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
            {
                //FileUpload1 = (FileUpload)Session["FileUpload1"];
                //lblMessage.Text = FileUpload1.FileName;
            }
            //  when Session will have File but user want to change the file 
            // i.e. wants to upload a new file using same FileUpload control
            // so update the session to have the newly uploaded file
            else if (FileUpload1.HasFile)
            {
                //Session["FileUpload1"] = FileUpload1;
                //lblMessage.Text = FileUpload1.FileName;
            }

        }

        /// <summary>
        /// Get PIR Type 
        /// </summary>
        private void GetPirType()
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant1;
            matplant1 = new SqlConnection(connetionString1);
            matplant1.Open();

            string pirtype = "select * from TPIRTYPE order by PIRType Asc";

            SqlDataAdapter dapirtype = new SqlDataAdapter();
            dapirtype = new SqlDataAdapter(pirtype, matplant1);
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
        /// <summary>
        /// SET request date Field
        /// </summary>
        protected void getdate()
        {

            txtReqDate.Text = DateTime.Now.Date.ToShortDateString().ToString();
            txtReqDate.Attributes.Add("disabled", "disabled");


        }
        /// <summary>
        /// Get Plant from DB and set to plant Field.
        /// </summary>
        protected void getplant()
        {

            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection conplant;

            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();

            DataTable plant = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter();

            string str = "select Plant,Description from TPLANT";
            dap = new SqlDataAdapter(str, conplant);
            dap.Fill(plant);

            txtplant.Text = plant.Rows[0]["Plant"].ToString();
            Session["plant"] = Convert.ToInt32(txtplant.Text);
            conplant.Close();
        }
        /// <summary>
        /// Get Material Type
        /// </summary>
        protected void matltype()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();
            string strprod = txtplant.Text;
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = " select MT.Plant,MT.MaterialType, * from [dbo].[TMATERIALTYPE] as MT inner join TPLANT as P on MT.plant=p.Plant  where P.Plant= '" + txtplant.Text + "'";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            ddlmatltype.DataSource = Result;
            ddlmatltype.DataTextField = "MaterialType";
            ddlmatltype.DataValueField = "MaterialType";
            ddlmatltype.DataBind();
            ddlmatltype.Items.Insert(0, new ListItem("-- Material Type --", String.Empty));

            con1.Close();
        }

        /// <summary>
        /// Get Product
        /// </summary>
        public void Getproduct()
        {

            string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = " select distinct (PR.product) , PR.Description as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant = '" + txtplant.Text + "'";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            con1.Close();

        }

        /// <summary>
        /// Product Dropdownlist Change Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string prod = ddlproduct.SelectedItem.Text;
            string prod = txtprodID.Text;

            Session["prod"] = prod.ToString();

            getmatlclass(prod);

        }

        /// <summary>
        /// Get Material class list based on Product
        /// </summary>
        /// <param name="matlclass"></param>
        protected void getmatlclass(string matlclass)
        {

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matl;
            matl = new SqlConnection(connetionString1);
            matl.Open();
            DataTable dtmatl = new DataTable();
            SqlDataAdapter damatl = new SqlDataAdapter();

            //string strmatl = " select distinct materialclassdescription,plant,product from materials where product='" + matlclass + "'";

            //damatl = new SqlDataAdapter(strmatl, matl);
            //dtmatl = new DataTable();
            //damatl.Fill(dtmatl);
            //matl.Close();

        }

        /// <summary>
        /// Material Class dropdownlist change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlmatlclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string mat_class = ddlmatlclass.SelectedItem.Text;
            string mat_class = txtmatlclass.Text;
            Session["materialclass"] = mat_class.ToString();
        }


        /// <summary>
        /// Get Product on load and assigning to autocomplete textbox
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getProduct(string prefixText)
        {

            string strplant;
            if (HttpContext.Current.Session["splproctype"] != null)
            {
                strplant = (string)HttpContext.Current.Session["plant"].ToString();
            }
            else
            {
                strplant = "2100";
            }
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //  string str = " select distinct (PR.product) + ' - ' + PR.productdescription as productdetails from product as PR inner join plant as p on p.plantID=pr.plant inner join materials as MT on MT.plant=PR.plant where PR.productdescription like '" + prefixText + "%'";
            //   string str = " select distinct (PR.product)+ '- '+ PR.Description as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant like '" + strplant + "%'";
            //  string str = " select  (PR.product)+ '- '+ PR.Description as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant='" + strplant + "' and PR.Description like '" + prefixText + "%'";
            string str = "  select CONCAT(PR.product,+ '- '+ PR.Description) as productdescription from TPRODUCT as PR inner join TPLANT as p on p.plant=pr.plant where p.plant='" + strplant + "' and PR.product like '%" + prefixText + "%'";


            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);
            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());
            return Output;
        }

        /// <summary>
        /// Get SAP Part Code on load and assigning to autocomplete textbox
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getSapPart(string prefixText)
        {
            int splnewproc = 0;

            List<string> Output = new List<string>();
            string strplant = "";
            string strprod = "";
            string matrl_class_desc = "";
            string strplantstatus = "";
            string matlType = "";
            string proctype = "";
            string splproctype = "";


            if (HttpContext.Current.Session["plant"] != null)
                strplant = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["prod_code"] != null)
            {
                strprod = (string)HttpContext.Current.Session["prod_code"].ToString();
                strprod = strprod.Substring(0, 2);
            }

            if (HttpContext.Current.Session["matlclass"] != null)
                matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            if (HttpContext.Current.Session["plant_status"] != null)
                strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            if (HttpContext.Current.Session["MaterialType"] != null)
                matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            if (HttpContext.Current.Session["proctype"] != null)
                proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            if (HttpContext.Current.Session["splproctype"] != null)
                splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            if (splproctype != "BLANK")
                splnewproc = 0;
            //if (prefixText.Length >= 4)
            //{

            string str = "";

            if (splproctype == "")
            {
                if (prefixText.Length >= 5)
                {
                    str = "Select distinct Material from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.Material like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '')";
                }

            }

            else if (splproctype.ToString().ToUpper() == "BLANK")
            {
                //txtpartdescription.Text = "";
                str = "Select distinct Material from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.Material like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '') and TM.SPlProcType is null";
            }
            else if (splproctype != "")
            {
                str = "Select distinct Material from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.Material like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '') and (tm.SPlProcType= '" + splproctype + "' or Coalesce('" + splproctype + "','') = '')";
            }

            if (str != "")
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);

                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                con1.Open();
                da = new SqlDataAdapter(str, con1);
                da.Fill(Result);

                for (int i = 0; i < Result.Rows.Count; i++)
                    Output.Add(Result.Rows[i][0].ToString());

                con1.Close();

            }
            return Output;


        }

        /// <summary>
        ///  Get SAP Part Code Description on load and assigning to autocomplete textbox
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getSapPartdesc(string prefixText)
        {
            List<string> Output = new List<string>();


            string strplant = "";
            string strprod = "";
            string matrl_class_desc = "";
            string strplantstatus = "";
            string matlType = "";
            string proctype = "";
            string splproctype = "";


            if (HttpContext.Current.Session["plant"] != null)
                strplant = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["prod_code"] != null)
            {
                strprod = (string)HttpContext.Current.Session["prod_code"].ToString();
                strprod = strprod.Substring(0, 2);
            }

            // strprod = "CM";
            if (HttpContext.Current.Session["matlclass"] != null)
                matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            if (HttpContext.Current.Session["plant_status"] != null)
                strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            if (HttpContext.Current.Session["MaterialType"] != null)
                matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            if (HttpContext.Current.Session["proctype"] != null)
                proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            if (HttpContext.Current.Session["splproctype"] != null)
                splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();


            string str = "";


            if (splproctype == "")
            {
                if (prefixText.Length >= 10)
                {
                    str = "Select distinct MaterialDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.MaterialDesc like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '')";
                }
            }

            else if (splproctype.ToString().ToUpper() == "BLANK")
            {
                //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
                //subash//str = "Select distinct TB.FGCode from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType='" + splnewproc + "' and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGCode like '%"+prefixText+"%'";
                // str = "Select distinct Tm.Material from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType is null and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and Tm.Material like '%" + prefixText + "%'";
                str = "Select distinct MaterialDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.MaterialDesc like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '') and TM.SPlProcType is null";
            }
            else if (splproctype != "")
            {
                //subash//str = "Select distinct TB.FGCode from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType IS NULL and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGCode like '%" + prefixText + "%'";
                // str = "Select distinct Tm.Material from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType='" + splnewproc + "' and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and Tm.Material like '%" + prefixText + "%'";
                str = "Select distinct MaterialDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where Tm.MaterialDesc like '%" + prefixText + "%' and tm.Plant = '" + strplant + "'  and  (tm.PROCTYPE = '" + proctype + "' or Coalesce('" + proctype + "','') = '') and  (tm.Product = '" + strprod + "' or Coalesce('" + strprod + "','') = '') and (tm.MaterialType= '" + matlType + "'  or Coalesce('" + matlType + "','') = '') and (tm.PlantStatus= '" + strplantstatus + "' or Coalesce('" + strplantstatus + "','') = '') and (TR.ProdComDesc= '" + matrl_class_desc.ToString() + "'  or Coalesce('" + matrl_class_desc.ToString() + "','') = '') and (tm.SPlProcType= '" + splproctype + "' or Coalesce('" + splproctype + "','') = '')";
            }


            if (str != "")
            {
                var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                SqlConnection con1;
                con1 = new SqlConnection(connetionString1);

                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                con1.Open();
                da = new SqlDataAdapter(str, con1);
                da.Fill(Result);

                for (int i = 0; i < Result.Rows.Count; i++)
                    Output.Add(Result.Rows[i][0].ToString());

                con1.Close();

            }
            return Output;


        }

        /// <summary>
        /// Get Material class on load and assigning to autocomplete textbox
        /// </summary>
        /// <param name="prefixText"></param>
        /// <returns></returns>

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getMatlClass(string prefixText)
        {

            string tes;
            //tes = Convert.ToString((string)HttpContext.Current.Session["prod_code"].ToString());
            //string  = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["prod_code"] != null)
            {
                tes = (string)HttpContext.Current.Session["prod_code"].ToString();
            }
            else
            {
                tes = "S";
            }

            string strProduct = Convert.ToString(tes);

            string strplant;
            if (HttpContext.Current.Session["plant"] != null)
            {
                strplant = (string)HttpContext.Current.Session["plant"].ToString();
            }
            else
            {
                strplant = "Z2";
            }



            // string strProduct = Convert.ToString(tes);

            string strPlantStatus;
            if (HttpContext.Current.Session["plant_status"] != null)
            {
                strPlantStatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            }
            else
            {
                strPlantStatus = "Z2";
            }

            string strMat;

            if (HttpContext.Current.Session["MaterialType"] != null)
            {
                strMat = (string)HttpContext.Current.Session["MaterialType"].ToString();
            }
            else
            {
                strMat = "SFPB";
            }

            string strProcType;

            if (HttpContext.Current.Session["proctype"] != null)
            {
                strProcType = (string)HttpContext.Current.Session["proctype"].ToString();
            }
            else
            {
                strProcType = "F";
            }
            string strSplProctype;

            if (HttpContext.Current.Session["splproctype"] != null)
            {
                strSplProctype = (string)HttpContext.Current.Session["splproctype"].ToString();
            }
            else
            {
                strSplProctype = "BLANK";
            }

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            // string str = "select distinct MaterialClassDescription from Tmaterial where Plant = '" + strplant + "' and product='" + strProduct + "' and PlantStatus='" + strPlantStatus + "' and  MaterialClassDescription like '%" + prefixText + "%'";

            string Strnew = "";
            if (strSplProctype.ToUpper() != "BLANK")
            {
                //strSplProctype = "0";
                Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE = '" + strSplProctype + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
                // string str = " select distinct materialclassdescription from Tmaterial where materialclassdescription like '" + prefixText + "%'";
            }
            else
            {
                //Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE ='" + strSplProctype+"'   and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";

                Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE is null  and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
            }
            da = new SqlDataAdapter(Strnew, con1);
            Result = new DataTable();
            da.Fill(Result);
            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;


        }

        /// <summary>
        /// SAP Part code text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdesc_TextChanged(object sender, EventArgs e)
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

                string strmaterial = txtpartdesc.Text;

                Session["materialNumber"] = strmaterial.ToString();
                //string[] matl = strmaterial.Split('-');
                //string getmatl = matl[0].ToString();
                //Session["material"] = getmatl.ToString();
                GetProdMaterial(strmaterial);

            }


        }

        //subash product code validation 

        //subash
        private void GetProdUser(string materialid)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";

            str = "select * from TSMNProductPIC where product in(select distinct product from TPRODUCT  tm where CONCAT(tm.product,+ '- '+ tm.Description)  like '%" + (txtprodID.Text.ToString()) + "%') and userid='" + userId1.ToString() + "'";

            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            if (dtdate.Rows.Count <= 0)
            {
                txtprodID.Text = "";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No PIC person for the Product. please Contact Admin')", true);
            }


            consap.Close();


        }

        /// <summary>
        /// Get All the Details based on SAP Part Code
        /// </summary>
        /// <param name="materialid"></param>
        private void GetProdMaterial(string materialid)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";

            str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating, TM.MaterialType,TM.PlantStatus, TM.PROCTYPE,TM.SplPROCTYPE, CONCAT(PR.product,+ '- '+ PR.Description) as Product,TR.ProdComDesc,TM.BaseUOM from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TPRODUCT PR ON TM.Product = PR.product where Tm.Material = '" + materialid + "' and tm.Plant = '" + txtplant.Text.ToString() + "'";

            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);
            if (txtmatlclass.Text == "")
            {
                ddlmatdescriptionC.DataSource = dtdate;
                ddlmatdescriptionC.DataTextField = "ProdComDesc";
                ddlmatdescriptionC.DataValueField = "ProdComDesc";
                ddlmatdescriptionC.DataBind();
                ddlmatdescriptionC.Items.Insert(0, new ListItem("-- select Description --", String.Empty));
            }

            if (dtdate.Rows.Count > 0)
            {
                txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
                txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                txtBaseUOM1.Text = dtdate.Rows[0]["BaseUOM"].ToString();
                txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();
                Session["materialDesc"] = txtpartdescription.Text.ToString();

                ddlmatltype.SelectedValue = dtdate.Rows[0]["MaterialType"].ToString();

                ddlproctype.SelectedValue = dtdate.Rows[0]["PROCTYPE"].ToString();
                // ddlsplproctype.SelectedValue = dtdate.Rows[0]["SplPROCTYPE"].ToString();
                txtprodID.Text = dtdate.Rows[0]["Product"].ToString();
                txtmatlclass.Text = dtdate.Rows[0]["ProdComDesc"].ToString();
                //if (txtmatlclass.Text == "")
                //{
                ddlmatdescriptionC.SelectedValue = txtmatlclass.Text;
                //}
                if (ddlplantstatus.Items.Count == 0)
                {
                    string prod = ddlmatltype.SelectedItem.Text;
                    GetPlantStatus(prod);
                }
                if (ddlsplproctype.Items.Count < 1)
                {
                    string prod = ddlproctype.SelectedItem.Text;
                    GetSplProcType(prod);
                }

                string strddlpSval = dtdate.Rows[0]["PlantStatus"].ToString();

                if (strddlpSval == "Z2")
                    ddlplantstatus.SelectedValue = strddlpSval;
                else
                    ddlplantstatus.SelectedValue = "Z2";

                //ddlplantstatus.SelectedValue = dtdate.Rows[0]["PlantStatus"].ToString();

                string splProc = dtdate.Rows[0]["SplPROCTYPE"].ToString();

                if (splProc == "")
                    ddlsplproctype.SelectedValue = "Blank";
                else
                    ddlsplproctype.SelectedValue = splProc;



                string proc = ddlproctype.SelectedItem.Text;
                string SpProc = ddlsplproctype.SelectedItem.Text;
                GetPIRTypesbysplproc(proc, SpProc);

                GetProdtextchange(txtprodID.Text);
                getMatlClassTextChange(txtmatlclass.Text);





            }

            consap.Close();


        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Load vendor based on Process Group
        /// </summary>
        /// <param name="processgrp"></param>
        protected void vendorload(string processgrp)
        {

            string strprod = (string)HttpContext.Current.Session["plant"].ToString();


            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
            string str = "select TVP.VendorCode,TVP.VendorName, tv.SearchTerm from TVENDOR_PROCESSGROUP TVP inner join tVendor_New tv ON TVP.VendorCode = tv.Vendor inner join TVENDORPIC as VP on VP.VendorCode=tv.Vendor Where TVP.processgrp='" + processgrp + "'";
            da = new SqlDataAdapter(str, con1);
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
            con1.Close();
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
                this.changevendr.Checked = false;
                Session["firstarticle"] = draftcost.Checked.ToString();
                Response.Redirect("WithSApCode.aspx");
            }

        }

        /// <summary>
        /// Change vendor Radio button changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changevendr_CheckedChanged(object sender, EventArgs e)
        {
            if (changevendr.Checked == true)
            {
                this.article.Checked = false;
                this.draftcost.Checked = false;
                this.changevendr.Checked = true;

                Response.Redirect("ChangeofVendor.aspx");
            }
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
                HttpContext.Current.Session["splproctype"] = "";
                this.changevendr.Checked = false;
                this.draftcost.Checked = false;
                this.article.Checked = true;

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
                this.changevendr.Checked = false;
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
        }

        /// <summary>
        /// Get Plant Status based on Material
        /// </summary>
        /// <param name="prod"></param>
        private void GetPlantStatus(string prod)
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant;
            matplant = new SqlConnection(connetionString1);
            matplant.Open();

            string plantstatus = "select plantstatus from tMaterialTypevsPlantStatus where MaterialType='" + prod + "' ";
            SqlDataAdapter daplantstatus = new SqlDataAdapter();
            daplantstatus = new SqlDataAdapter(plantstatus, matplant);
            DataTable dtplantstaus = new DataTable();
            daplantstatus.Fill(dtplantstaus);

            if (dtplantstaus.Rows.Count > 0)
            {

                ddlplantstatus.DataSource = dtplantstaus;
                ddlplantstatus.DataTextField = "plantstatus";
                ddlplantstatus.DataTextField = "plantstatus";
                ddlplantstatus.Items.Insert(0, new ListItem("-- Select Plant Status --", String.Empty));
                ddlplantstatus.DataBind();

                if (article.Checked == true)
                {
                    //  ddlplantstatus.Items.RemoveAt(0);
                }

                //  ddlplantstatus.Text = dtplantstaus.Rows[0]["plantstatus"].ToString();
                Session["plant_status"] = ddlplantstatus.SelectedItem.Text;
            }
            else
            {
                // ddlplantstatus.Text = "";

            }


            Session["MaterialType"] = ddlmatltype.SelectedItem.Text;
        }

        /// <summary>
        /// Get Procurement Type
        /// </summary>
        protected void getproctype()
        {
            // string strprod = (string)HttpContext.Current.Session["PRODUCTCODE"].ToString();


            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection proctype;
            proctype = new SqlConnection(connetionString1);
            proctype.Open();
            DataTable dtmatl = new DataTable();
            SqlDataAdapter damatl = new SqlDataAdapter();
            // string strmatl = "select distinct Materialclassdescription from materials where product='" + matlclass + "'";

            string strmatl = " select distinct proctype from TPROCPIRTYPE";


            damatl = new SqlDataAdapter(strmatl, proctype);
            dtmatl = new DataTable();
            damatl.Fill(dtmatl);

            ddlproctype.DataSource = dtmatl;
            ddlproctype.DataTextField = "proctype";
            ddlproctype.DataValueField = "proctype";
            ddlproctype.DataBind();
            ddlproctype.Items.Insert(0, new ListItem("-- select Proctype --", String.Empty));

            proctype.Close();

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
        }


        /// <summary>
        /// Get Special Procurement Type based on Procurement Type
        /// </summary>
        /// <param name="prod"></param>
        private void GetSplProcType(string prod)

        {

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant;
            matplant = new SqlConnection(connetionString1);
            matplant.Open();
            //By subash//string plantstatus = "select Distinct SpProcType from TPROCPIRTYPE where procType='" + prod + "' ";
            string plantstatus = "select ProcType, isnull(SPProcType,'Blank') as Spproctype  from ProcurementType  where ProcType = '" + prod + "' ";
            SqlDataAdapter daplantstatus = new SqlDataAdapter();
            daplantstatus = new SqlDataAdapter(plantstatus, matplant);
            DataTable dt = new DataTable();
            daplantstatus.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ddlsplproctype.DataSource = dt;

                ddlsplproctype.DataTextField = "Spproctype";
                ddlsplproctype.DataValueField = "Spproctype";
                ddlsplproctype.DataBind();

                ddlsplproctype.Items.Insert(0, new ListItem("-- Select Spproctype --", String.Empty));
                Session["proctype"] = ddlproctype.SelectedItem.Text;
                Session["splproctype"] = ddlsplproctype.SelectedItem.Text;
            }
            else
            {
                // ddlplantstatus.Text = "";
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
            GetPIRTypesbysplproc(proc, SpProc);
        }

        //subash



        /// <summary>
        /// Get PIR Type based on Procurement and Spl Procurement Types
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="SpProc"></param>
        private void GetPIRTypesbysplproc(string proc, string SpProc)
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant1;
            matplant1 = new SqlConnection(connetionString1);
            matplant1.Open();
            string pirtype;
            if (SpProc == "Blank")
            {
                SpProc = "";
                pirtype = "select PIRTYPE,DESCRIPTION from TPROCPIRTYPE where procType='" + proc + "' and Spproctype='" + SpProc + "' ";
            }
            else
            {
                pirtype = "select PIRTYPE,DESCRIPTION from TPROCPIRTYPE where procType='" + proc + "' and Spproctype='" + SpProc + "' ";
            }

            SqlDataAdapter dapirtype = new SqlDataAdapter();
            dapirtype = new SqlDataAdapter(pirtype, matplant1);
            DataTable dtpir = new DataTable();
            dapirtype.Fill(dtpir);

            if (dtpir.Rows.Count > 0)
            {
                ddlpirtype.DataSource = dtpir;
                ddlpirtype.DataTextField = "PIRTYPE";
                ddlpirtype.DataTextField = "PIRTYPE";

                ddlpirtype.DataBind();

                txtPIRDesc.Text = dtpir.Rows[0]["DESCRIPTION"].ToString();
            }
            else
            {
            }

            Session["Pirtype"] = ddlpirtype.SelectedItem.Text;
            Session["PirDescription"] = txtPIRDesc.Text;
            Session["splproctype"] = ddlsplproctype.SelectedItem.Text;
        }

        //protected void txtprodID_Unload(object sender, EventArgs e)
        //{
        //    string prod = txtprodID.Text;

        //    Session["product"] = prod.ToString();  
        //}

        /// <summary>
        /// Get Process
        /// </summary>
        protected void process()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();
            string strprod = txtplant.Text;
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = " select Process_Grp_code,Process_Grp_Description from TPROCESGROUP_LIST";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            ddlprocess.DataSource = Result;
            ddlprocess.DataTextField = "Process_Grp_code";
            ddlprocess.DataValueField = "Process_Grp_code";
            ddlprocess.DataBind();
            ddlprocess.Items.Insert(0, new ListItem("-- Select Process --", String.Empty));

            Session["process"] = ddlprocess.SelectedItem.Text;

            con1.Close();

        }

        /// <summary>
        /// Process DDL index change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected void ddlprocess_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtjobtypedesc.Text = "";
            Session["process"] = ddlprocess.SelectedItem.Text;

            string strprod = (string)HttpContext.Current.Session["process"].ToString();


            this.PIRTYPE();
            this.vendorload(strprod);
        }

        /// <summary>
        /// PIR Type DDL index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlpirtype_SelectedIndexChanged(object sender, EventArgs e)
        {

            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();


            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = " select description,ProcType,SPProcType from TPROCPIRTYPE where ProcType='' and SPProcType='' and PIRType=''";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            ddlprocess.DataSource = Result;
            ddlprocess.DataTextField = "Process_Grp_code";
            ddlprocess.DataValueField = "Process_Grp_code";
            ddlprocess.Items.Insert(0, new ListItem("-- Select Process --", String.Empty));
            ddlprocess.DataBind();

            Session["process"] = ddlprocess.SelectedItem.Text;

            con1.Close();
        }

        /// <summary>
        /// Get PIR Type
        /// </summary>
        protected void PIRTYPE()
        {

            string strproc1 = (string)HttpContext.Current.Session["process"].ToString();


            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select (JobCode)+ '- '+ JobcodeDesc as PIRTYTypeDescription  from TPIRJOBTYPE_PROCESSGROUP where ProcessGrpcode='" + strproc1 + "'";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            // txtjobtypedesc.Text=Result.rows[0]["JobCode"].

            if (Result.Rows.Count > 0)
            {
                txtjobtypedesc.Text = Result.Rows[0]["PIRTYTypeDescription"].ToString();
            }
            else
            {
                //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
            }
            Session["PIRJOBTYPE"] = txtjobtypedesc.Text;

            con1.Close();
        }
        /// <summary>
        /// Plant Status DDL index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlplantstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strplantstatus = ddlplantstatus.SelectedItem.Text;

            Session["plant_status"] = strplantstatus.ToString();

        }

        /// <summary>
        /// Assign HiddenValues in Hidden Fields
        /// </summary>
        public void AssignHdnValues()
        {
            try
            {
                hprodID.Value = txtprodID.Text;

            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Get Material Details based on Material Code
        /// </summary>
        /// <param name="getprodcode"></param>
        private void GetProdDesc(string getprodcode)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where material like '%" + getprodcode + "%' ";
            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
            txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
            txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
            consap.Close();


        }

        /// <summary>
        /// PIR Type ddl index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlpirtype_SelectedIndexChanged1(object sender, EventArgs e)
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

        /// <summary>
        /// Part Text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtprodID_TextChanged(object sender, EventArgs e)
        {
            string strproddesc = txtprodID.Text;
            txtpartdesc.Text = "";
            txtpartdescription.Text = "";
            //ddlpirtype.SelectedValue = "";
            txtPIRDesc.Text = "";
            txtunitweight.Text = "";
            txtUOM.Text = "";
            txtBaseUOM1.Text = "";
            txtmatlclass.Text = "";
            if (strproddesc.Length > 1)
                GetProdtextchange(strproddesc);

            GetProdUser(strproddesc);

            string[] matl = strproddesc.Split('-');
            string getmatl1 = matl[0].ToString();
            //Session["material"] = getmatl.ToString();
            ddlmatdescriptionC_SelectedIndexChanged1();
        }


        //subash ddl

        protected void ddlmatdescriptionC_SelectedIndexChanged1()
        {

            string[] matl = (txtprodID.Text).Split('-');
            string getmatl1 = matl[0].ToString();

            string tes;

            if (HttpContext.Current.Session["prod_code"] != null)
            {
                tes = Convert.ToString((string)HttpContext.Current.Session["prod_code"].ToString());
            }

            else
            {
                tes = "SL";
            }

            string strplant;

            if (HttpContext.Current.Session["plant"] != null)
            {
                strplant = (string)HttpContext.Current.Session["plant"].ToString();
            }
            else
            {
                strplant = "2100";
            }

            string strProduct = Convert.ToString(tes);

            string strPlantStatus;
            if (HttpContext.Current.Session["plant_status"] != null)
            {
                strPlantStatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            }
            else
            {
                strPlantStatus = "Z2";
            }

            string strMat;

            if (HttpContext.Current.Session["MaterialType"] != null)
            {
                strMat = (string)HttpContext.Current.Session["MaterialType"].ToString();
            }
            else
            {
                strMat = "SFPB";
            }
            string strProcType;
            if (HttpContext.Current.Session["proctype"] != null)
            {
                strProcType = (string)HttpContext.Current.Session["proctype"].ToString();
            }
            else
            {
                strProcType = "F";
            }
            string strSplProctype;
            if (HttpContext.Current.Session["splproctype"] != null)
            {
                strSplProctype = (string)HttpContext.Current.Session["splproctype"].ToString();
            }
            else
            {
                strSplProctype = "BLANK";
            }



            //string proc = ddlproctype.SelectedItem.Text;
            //string SpProc = ddlsplproctype.SelectedItem.Text;
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection proctype;
            proctype = new SqlConnection(connetionString1);
            proctype.Open();
            DataTable dtmatl = new DataTable();
            SqlDataAdapter damatl = new SqlDataAdapter();
            string strmatl = string.Empty;

            if (strSplProctype.ToUpper() != "BLANK")
            {
                strmatl = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE = '" + strSplProctype + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "'";
            }
            else
            {
                strmatl = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE is null  and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "'";
            }

            damatl = new SqlDataAdapter(strmatl, proctype);
            dtmatl = new DataTable();
            damatl.Fill(dtmatl);

            ddlmatdescriptionC.DataSource = dtmatl;
            ddlmatdescriptionC.DataTextField = "ProdComDesc";
            ddlmatdescriptionC.DataValueField = "ProdComDesc";
            ddlmatdescriptionC.DataBind();
            ddlmatdescriptionC.Items.Insert(0, new ListItem("-- select Description --", String.Empty));

            proctype.Close();

        }

        /// <summary>
        /// Method to Assign values to textbox from Product change event
        /// </summary>
        /// <param name="strproddesc"></param>
        private void GetProdtextchange(string strproddesc)
        {

            string[] matl = strproddesc.Split('-');
            string getprod = matl[0].ToString();
            Session["prod_code"] = getprod.ToString();
        }
        /// <summary>
        /// Create Request button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsave_Click(object sender, EventArgs e)
        {
            bool IsVendorSelected = false;
            if (ddlproctype.SelectedItem.Text != "E" || ddlproctype.SelectedValue.ToString() != "E")
            {
                if (txtDate.Text != "" && txtDate.Text != null)
                {
                    if (lblMessage.Text != "" && lblMessage.Text.ToString() != null)
                    {

                        if (txtjobtypedesc.Text.ToString() != "" && txtjobtypedesc.Text != null)
                        {

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

                                #region Save Method
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


                                    string RequestIncNumber = "";

                                    int increquest = 000000;
                                    string RequestInc = String.Format(currentYear, increquest);

                                    string REQUEST = string.Concat(currentYear, RequestIncNumber);

                                    var connetionStringreq = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                                    SqlConnection consap;
                                    consap = new SqlConnection(connetionStringreq);
                                    consap.Open();
                                    DataTable dtdate = new DataTable();
                                    SqlDataAdapter da = new SqlDataAdapter();
                                    SqlCommand cmd = new SqlCommand("select MAX(RequestNumber) from TQuoteDetails", consap);
                                    //    string str = " select MAX(REQUESTNO) from Tstatus ";
                                    SqlDataReader dr = cmd.ExecuteReader();
                                    if (dr.Read())
                                    {
                                        string curentYear = DateTime.Now.Year.ToString();

                                        string ReqNum = dr[0].ToString();

                                        if (ReqNum == "")
                                        {
                                            RequestIncNumber = "000000";
                                            RequestIncNumber1 = "000000";
                                            string.Concat(currentYear, RequestIncNumber);
                                            RequestIncNumber = string.Concat(currentYear, RequestIncNumber);
                                            RequestIncNumber1 = string.Concat(currentYear, RequestIncNumber);
                                        }
                                        else
                                        {

                                            ReqNum = ReqNum.Remove(0, 4);
                                            ReqNum = string.Concat(currentYear, ReqNum);
                                            RequestIncNumber = ReqNum;
                                            RequestIncNumber1 = ReqNum;
                                        }

                                        // char C = RequestIncNumber[RequestIncNumber.Length - 1];

                                        int newReq = (int.Parse(RequestIncNumber)) + (1);
                                        RequestIncNumber = newReq.ToString();
                                        RequestIncNumber1 = RequestIncNumber;
                                        // int tempval = Convert.ToInt32(C.ToString()) + 1;


                                    }

                                    Session["RequestNo"] = RequestIncNumber.ToString();


                                    int rowscount = 0;
                                    foreach (GridViewRow gr in grdvendor.Rows)
                                    {
                                        CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                                        if (myCheckbox.Checked == true)
                                        {
                                            dg_checkvalue = "Y";
                                            dg_formid = gr.Cells[1].Text.ToString();
                                            string dg_VenName = gr.Cells[2].Text.ToString();

                                            dg_checkvalue = gr.Cells[2].Text.ToString();
                                            string strvendname = dg_checkvalue.ToString();
                                            string mstrvenname = strvendname.Substring(0, 4);

                                            //  string formattedIncNumber = String.Format("{000:D6}", incNumber);
                                            string getVendName = mstrvenname[0].ToString();

                                            string QuoteSearchTerm = "";
                                            QuoteSearchTerm = gr.Cells[3].Text.ToString();
                                            string getquote = String.Concat(QuoteSearchTerm, RequestIncNumber);
                                            string strPlatingType = txtplatingtype.Text.ToString();


                                            getQuoteRef = getquote;

                                            remarks = "Open Status";
                                            rowscount++;


                                            string Proccode = (string)HttpContext.Current.Session["prod_code"].ToString();
                                            string matnum = (string)HttpContext.Current.Session["materialNumber"].ToString();
                                            string strproc1 = (string)HttpContext.Current.Session["process"].ToString();

                                            string PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                                            string MatDesc = (string)HttpContext.Current.Session["materialDesc"].ToString();

                                            string PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                                            string PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();

                                            string UnitNetWeight = txtunitweight.Text.ToString();
                                            DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);


                                            //QuoteDate = QuoteDate.ToLongDateString();

                                            string PITTypeandDesc = PITType + "- " + PITTypeDesc;

                                            var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                                            SqlConnection conins;
                                            conins = new SqlConnection(connetionStringdate);
                                            conins.Open();
                                            //DataTable dtdate = new DataTable();
                                            //SqlDataAdapter da = new SqlDataAdapter();
                                            // string query = "INSERT INTO TVENDOR_PROCESSGROUP(VendorCode,VendorName,ProcessGrp,ProcessGrpDescription,UpdateBy,UpdatedOn) VALUES (@VendorCode,@VendorName,@ProcessGrp,@ProcessGrpDescription,@UpdateBy,@UpdatedOn)";
                                            string query = " INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,MaterialType,PlantStatus,CreatedBy,BaseUOM) values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,@PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,@MaterialType,@PlantStatus,@UserId,@BaseUOM)";
                                            //    string text = "Data Saved!";
                                            SqlCommand cmd1 = new SqlCommand(query, conins);
                                            cmd1.Parameters.AddWithValue("@REQUESTNO", Convert.ToInt32(RequestIncNumber.ToString()));
                                            DateTime Dtstrdate = DateTime.ParseExact(txtReqDate.Text, "dd/MM/yyyy", null);
                                            strdate = Dtstrdate.ToString("yyyy-MM-dd");
                                            cmd1.Parameters.AddWithValue("@REQUESTDATE", strdate.ToString());
                                            cmd1.Parameters.AddWithValue("@PLANT", txtplant.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@QUOTENO", getQuoteRef.ToString());
                                            cmd1.Parameters.AddWithValue("@VENDORCODE", dg_formid.ToString());
                                            cmd1.Parameters.AddWithValue("@VENDORNAME", dg_VenName.ToString());
                                            cmd1.Parameters.AddWithValue("@procesgrp", strproc1.ToString());
                                            cmd1.Parameters.AddWithValue("@Product", Proccode.ToString());
                                            cmd1.Parameters.AddWithValue("@Material", txtpartdesc.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@matlClass", txtmatlclass.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@PIRJobtype", PIRJobType.ToString());
                                            cmd1.Parameters.AddWithValue("@MaterialDesc", txtpartdescription.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@PIRTypeDesc", PITTypeandDesc.ToString());
                                            cmd1.Parameters.AddWithValue("@UnitNet", UnitNetWeight.ToString());
                                            cmd1.Parameters.AddWithValue("@QuoteDue", QuoteDate.ToString("yyyy/MM/dd HH:mm:ss"));
                                            cmd1.Parameters.AddWithValue("@DrawingNo", imgHidden.Value.ToString());
                                            cmd1.Parameters.AddWithValue("@PlatingType", strPlatingType);
                                            cmd1.Parameters.AddWithValue("@SAPProcType", ddlproctype.SelectedItem.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@SAPSPProcType", ddlsplproctype.SelectedItem.Text.ToString());

                                            cmd1.Parameters.AddWithValue("@MaterialType", ddlmatltype.SelectedItem.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@PlantStatus", ddlplantstatus.SelectedItem.Text.ToString());
                                            cmd1.Parameters.AddWithValue("@UserId", userId.ToString());
                                            cmd1.Parameters.AddWithValue("@BaseUOM", txtBaseUOM1.Text.ToString());


                                            cmd1.CommandText = query;
                                            cmd1.ExecuteNonQuery();
                                            conins.Close();


                                            //subash - email

                                            //var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                                            //using (SqlConnection cnn = new SqlConnection(Email))
                                            //{
                                            //    string returnValue = string.Empty;
                                            //    cnn.Open();
                                            //    SqlCommand cmdget = cnn.CreateCommand();
                                            //    cmdget.CommandType = CommandType.StoredProcedure;
                                            //    cmdget.CommandText = "dbo.spGetControlNumber";

                                            //    SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                            //    CompanyCode.Direction = ParameterDirection.Input;
                                            //    CompanyCode.Value = 1;
                                            //    cmdget.Parameters.Add(CompanyCode);

                                            //    SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                            //    ControlField.Direction = ParameterDirection.Input;
                                            //    ControlField.Value = "MessageHeaderID";
                                            //    cmdget.Parameters.Add(ControlField);

                                            //    SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                            //    Param1.Direction = ParameterDirection.Input;
                                            //    Param1.Value = "";
                                            //    cmdget.Parameters.Add(Param1);

                                            //    SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                            //    Param2.Direction = ParameterDirection.Input;
                                            //    Param2.Value = "";
                                            //    cmdget.Parameters.Add(Param2);

                                            //    SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                            //    Param3.Direction = ParameterDirection.Input;
                                            //    Param3.Value = "";
                                            //    cmdget.Parameters.Add(Param3);

                                            //    SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                            //    Param4.Direction = ParameterDirection.Input;
                                            //    Param4.Value = "";
                                            //    cmdget.Parameters.Add(Param4);

                                            //    SqlParameter pOutput = cmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                            //    pOutput.Direction = ParameterDirection.Output;

                                            //    cmdget.ExecuteNonQuery();
                                            //    returnValue = pOutput.Value.ToString();
                                            //    cnn.Close();
                                            //    OriginalFilename = returnValue;
                                            //    MHid = returnValue;
                                            //    OriginalFilename = MHid + seqNo + formatW;
                                            //}


                                            //string Destination = "\\\\172.18.8.27\\AutoMail\\QA\\" + OriginalFilename;
                                            ////string Destination = "\\\\172.18.8.27\\AutoMail\\QA\\" + fname;
                                            ////Passing credentials - I need to change to get this data from table
                                            //using (new SoddingNetworkAuth(@"SPL-MESService", @"SHIMANOACE", @"$SPL-MESService"))
                                            //{
                                            //    File.Copy(Source, Destination, true);
                                            //}



                                            //var Email_insert = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                                            //using (SqlConnection Email_inser = new SqlConnection(Email_insert))
                                            //{
                                            //    Email_inser.Open();
                                            //    //Header
                                            //    string MessageHeaderId = MHid;
                                            //    string fromname = "eMET System";
                                            //    string FromAddress = "subashdurai@shimano.com.sg";
                                            //    string Recipient = "subashdurai@shimano.com.sg";
                                            //    string CopyRecipient = "NARAYANANSRIDER@shimano.com.sg";
                                            //    string BlindCopyRecipient = "subashdurai@shimano.com.sg";
                                            //    string ReplyTo = "subashdurai@shimano.com.sg";
                                            //    string Subject = "MailCenter from MES QA";
                                            //    string body = "Vendor Name: " + dg_VenName.ToString() + " | Request Number: " + Convert.ToInt32(RequestIncNumber.ToString()) + " | Quote Number:  " + getQuoteRef.ToString() + " | Partcode And Description: " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + " | Quotation Response Dat: " + QuoteDate.ToString("yyyy/MM/dd HH:mm:ss");
                                            //    string BodyFormat = "TEXT";
                                            //    string BodyRemark = "0";
                                            //    string Signature = "By DT";
                                            //    string Importance = "High";
                                            //    string Sensitivity = "Confidential";
                                            //    Boolean IsAttachFile = true;
                                            //    string CreateUser = userId;
                                            //    DateTime CreateDate = Convert.ToDateTime(DateTime.Now);
                                            //    //end Header
                                            //    string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                            //    SqlCommand Header = new SqlCommand(Head, Email_inser);
                                            //    Header.Parameters.AddWithValue("@MessageHeaderId", MessageHeaderId.ToString());
                                            //    Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                            //    Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                            //    Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                            //    Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                            //    Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                            //    Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                            //    Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                            //    Header.Parameters.AddWithValue("@body", body.ToString());
                                            //    Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                            //    Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                            //    Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                            //    Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                            //    Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                            //    Header.Parameters.AddWithValue("@IsAttachFile", Convert.ToBoolean(IsAttachFile.ToString()));
                                            //    Header.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                            //    Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            //    Header.CommandText = Head;
                                            //    Header.ExecuteNonQuery();
                                            //    //end Header
                                            //    //Details
                                            //    int SequenceNumber = 1;
                                            //    string SendFilename;
                                            //    SendFilename = fname.Remove(fname.Length - 4);
                                            //    OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                                            //    string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                            //    SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                            //    Detail.Parameters.AddWithValue("@MessageHeaderId", MHid.ToString());
                                            //    Detail.Parameters.AddWithValue("@SequenceNumber", Convert.ToInt32(SequenceNumber.ToString()));
                                            //    Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                            //    Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                            //    Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                            //    Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                            //    Detail.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                            //    Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            //    Detail.CommandText = Details;
                                            //    Detail.ExecuteNonQuery();
                                            //    //End Details
                                            //    Email_inser.Close();


                                            //}



                                            //End by subash

                                        }
                                        else
                                        {
                                            dg_checkvalue = "";
                                        }
                                    }

                                    GetData(RequestIncNumber.ToString());
                                }
                                catch (Exception ex)
                                {
                                }

                                #endregion Save Method
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors has selected. Please select at least one vendor!')", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('PIR Type not available for this Process Group!')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Drawing Number should not be null!')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Quote Response date should not be null!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Procurement Type E no need PIR and hence no need Quotation Request !')", true);
            }

        }

        /// <summary>
        /// Material Class Text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtmatlclass_TextChanged(object sender, EventArgs e)
        {

            string strmaterialclass = txtmatlclass.Text;

            getMatlClassTextChange(strmaterialclass);
        }
        /// <summary>
        /// Method to assign values to fields from  Material Class Text change event
        /// </summary>
        /// <param name="strmaterialclass"></param>
        private void getMatlClassTextChange(string strmaterialclass)
        {
            Session["matlclass"] = strmaterialclass.ToString();

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

        }


        /// <summary>
        /// Drawing number upload button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.FileName != "")
            {
                //.pdf

                string[] validFileTypes = { "pdf", "PDF" };
                string ext = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                bool isValidFile = false;
                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }
                if (!isValidFile)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Invalid File. Please upload a File with extension " +
                                   string.Join(",", validFileTypes);

                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Invalid File. Please upload a File with (.pdf)extension')", true);
                }
                else
                {

                    string test = userId;
                    string folderPath = Server.MapPath("~/Files/");
                    //Check whether Directory (Folder) exists.
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists Create it.
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save the File to the Directory (Folder).
                    FileUpload1.SaveAs(folderPath + Path.GetFileName(FileUpload1.FileName));
                    //Display the Picture in Image control.
                    Image3.ImageUrl = "~/Files/" + Path.GetFileName(FileUpload1.FileName);
                    imgHidden.Value = Path.GetFileName(FileUpload1.FileName);
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = FileUpload1.FileName + " has been Uploaded Successfuly.";
                    fname = FileUpload1.FileName;
                    Source = folderPath + FileUpload1.FileName;
                    //lblMessage.ForeColor = System.Drawing.Color.Red;
                    //lblMessage.Text = "File uploaded successfully.";
                }


                //.pdf eend



            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select the file')", true);
            }

        }

        /// <summary>
        /// Load Shimano PIC details 
        /// </summary>
        private void GetSHMNPIC()
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "select UseID,UseNam from USR where UseID='SBM_BIKE940146' ";

            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            string struser = dtdate.Rows[0]["UseNam"].ToString();
            lbluser.Text = dtdate.Rows[0]["UseID"].ToString();

            Session["userID"] = userId;

            consap.Close();


        }
        /// <summary>
        /// Sap Part code Description text change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdescription_TextChanged(object sender, EventArgs e)
        {

            if (txtpartdescription.Text == "")
            {
                txtpartdesc.Text = "";
                txtunitweight.Text = "";
                txtUOM.Text = "";
                txtplatingtype.Text = "";
            }
            else
            {


                string strmaterial = txtpartdescription.Text;
                //string[] matl = strmaterial.Split('-');
                //string getmatl = matl[0].ToString();
                Session["material"] = strmaterial.ToString();
                GetMaterialcode(strmaterial);
            }
        }

        /// <summary>
        /// Get Material code from Material Name. -- Material code = SAP Part Code , Material Name = Sap Part code description
        /// </summary>
        /// <param name="materialname"></param>
        private void GetMaterialcode(string materialname)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string str = "";
            str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating, TM.MaterialType,TM.PlantStatus, TM.PROCTYPE,TM.SplPROCTYPE,CONCAT(PR.product,+ '- '+ PR.Description) as Product,TR.ProdComDesc,TM.BaseUOM from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TPRODUCT PR ON TM.Product = PR.Product where Tm.MaterialDesc ='" + materialname + "' and tm.Plant = '" + txtplant.Text.ToString() + "'";

            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);
            if (txtmatlclass.Text == "")
            {
                ddlmatdescriptionC.DataSource = dtdate;
                ddlmatdescriptionC.DataTextField = "ProdComDesc";
                ddlmatdescriptionC.DataValueField = "ProdComDesc";
                ddlmatdescriptionC.DataBind();
                ddlmatdescriptionC.Items.Insert(0, new ListItem("-- select Description --", String.Empty));
            }
            if (dtdate.Rows.Count > 0)
            {

                txtpartdesc.Text = dtdate.Rows[0]["material"].ToString();
                txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                txtBaseUOM1.Text = dtdate.Rows[0]["BaseUOM"].ToString();
                txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();
                Session["materialDesc"] = txtpartdescription.Text.ToString();

                ddlmatltype.SelectedValue = dtdate.Rows[0]["MaterialType"].ToString();

                ddlproctype.SelectedValue = dtdate.Rows[0]["PROCTYPE"].ToString();
                // ddlsplproctype.SelectedValue = dtdate.Rows[0]["SplPROCTYPE"].ToString();
                txtprodID.Text = dtdate.Rows[0]["Product"].ToString();
                txtmatlclass.Text = dtdate.Rows[0]["ProdComDesc"].ToString();
                //if (txtmatlclass.Text == "")
                //{
                ddlmatdescriptionC.SelectedValue = txtmatlclass.Text;
                //}
                if (ddlplantstatus.Items.Count == 0)
                {
                    string prod = ddlmatltype.SelectedItem.Text;
                    GetPlantStatus(prod);
                }
                if (ddlsplproctype.Items.Count < 1)
                {
                    string prod = ddlproctype.SelectedItem.Text;
                    GetSplProcType(prod);
                }

                string strddlpSval = dtdate.Rows[0]["PlantStatus"].ToString();

                if (strddlpSval == "Z2")
                    ddlplantstatus.SelectedValue = strddlpSval;
                else
                    ddlplantstatus.SelectedValue = "Z2";

                //ddlplantstatus.SelectedValue = dtdate.Rows[0]["PlantStatus"].ToString();

                string splProc = dtdate.Rows[0]["SplPROCTYPE"].ToString();

                if (splProc == "")
                    ddlsplproctype.SelectedValue = "Blank";
                else
                    ddlsplproctype.SelectedValue = splProc;



                string proc = ddlproctype.SelectedItem.Text;
                string SpProc = ddlsplproctype.SelectedItem.Text;
                GetPIRTypesbysplproc(proc, SpProc);

                GetProdtextchange(txtprodID.Text);
                getMatlClassTextChange(txtmatlclass.Text);


            }

            consap.Close();


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
            if (txtpartdesc.Text == "")
            {

                txtpartdescription.Text = "";
                txtunitweight.Text = "";
                txtUOM.Text = "";
                txtplatingtype.Text = "";
            }
            else
            {
                if (ddlsplproctype.SelectedIndex == 0)
                {
                    if (txtpartdesc.Text.Length >= 5)
                    {
                        // string strmaterial = txtpartdesc.Text;
                        string strmaterialDesc = txtpartdescription.Text;
                        string strmaterial = txtpartdesc.Text;

                        //string[] matl = strmaterial.Split('-');
                        //string getmatl = matl[0].ToString();
                        //Session["material"] = getmatl.ToString();
                        GetProdMaterial(strmaterial);
                        Session["materialNumber"] = strmaterial.ToString();
                        Session["materialDesc"] = txtpartdescription.Text;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Enter min 5 chars, else choose the filter options!')", true);
                        // ddlmatltype.Focus();
                    }
                }
                else if (ddlsplproctype.SelectedIndex != 0)
                {
                    string strmaterialDesc = txtpartdescription.Text;
                    string strmaterial = txtpartdesc.Text;

                    //string[] matl = strmaterial.Split('-');
                    //string getmatl = matl[0].ToString();
                    //Session["material"] = getmatl.ToString();
                    GetProdMaterial(strmaterial);
                    Session["materialNumber"] = strmaterial.ToString();
                    Session["materialDesc"] = txtpartdescription.Text;
                }

            }

        }

        /// <summary>
        /// SAP Part code description text change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtpartdescription_TextChanged1(object sender, EventArgs e)
        {

            if (txtpartdescription.Text == "")
            {
                txtpartdesc.Text = "";
                txtunitweight.Text = "";
                txtUOM.Text = "";
                txtplatingtype.Text = "";
            }
            else
            {
                if (ddlsplproctype.SelectedIndex == 0)
                {
                    if (txtpartdescription.Text.Length >= 10)
                    {
                        string strmaterialDesc = txtpartdescription.Text;
                        string strmaterial = txtpartdesc.Text;

                        //string[] matl = strmaterial.Split('-');
                        //string getmatl = matl[0].ToString();

                        GetMaterialcode(strmaterialDesc);
                        Session["materialNumber"] = strmaterial.ToString();
                        Session["materialDesc"] = txtpartdescription.Text.ToString();


                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Enter min 10 chars. else choose the filter options!')", true);
                        ddlmatltype.Focus();
                    }
                }

                else if (ddlsplproctype.SelectedIndex != 0)
                {
                    string strmaterialDesc = txtpartdescription.Text;
                    string strmaterial = txtpartdesc.Text;

                    //string[] matl = strmaterial.Split('-');
                    //string getmatl = matl[0].ToString();
                    //Session["material"] = getmatl.ToString();
                    GetMaterialcode(strmaterialDesc);
                    Session["materialNumber"] = strmaterial.ToString();
                    Session["materialDesc"] = txtpartdescription.Text;
                }
            }

        }


        /// <summary>
        /// Load data and Create Dynamic table with BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
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


            strGetData = "select CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc] ,v.Description as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, v.Crcy, tc.Amount,tc.Unit,tc.UoM,tm.BaseUOM" +
" from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material " +
"inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material " +
" inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP" +
" on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant where TQ.RequestNumber='" + reqno + "' and TB.FGCode = '" + txtpartdesc.Text.ToString() + "' and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) ";


            da = new SqlDataAdapter(strGetData, con);
            da.Fill(dtget);

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
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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

                        if (rowcount > 0 && (cellCtr == 0 || cellCtr == 1 || cellCtr == 2 || cellCtr == 3 || cellCtr == 4))
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


                hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();

            }
            else
            {
                GetDataforNoBom(reqno.ToString());
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('selected vendors does not have valid Data. Please check the Master Data !')", true);
            }



            con.Close();
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
            string filename = imgHidden.Value.ToString();

            if (filename == "" || string.IsNullOrEmpty(filename))
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No attachment to download !')", true);
            }

            else
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.WriteFile(Image3.ImageUrl.ToString());

                Response.End();
            }


            // Same URL

            //string filename = imgHidden.Value;
            //string path = MapPath(filename);
            //byte[] bts = System.IO.File.ReadAllBytes(path);
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";
            //Response.WriteFile(path);
            //Response.Flush();
            //Response.End();


            // Show in panel without download
            //string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            //embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            //embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            //embed += "</object>";
            //ltEmbed.Text = string.Format(embed, ResolveUrl(filename));
            //Server.Transfer(filename);

            //string url = string.Format("NewRequest.aspx?FN={0}.pdf", filename);
            //string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
            //this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);

        }

        /// <summary>
        /// submit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Button1_Click(object sender, EventArgs e)
        {
            if (fname != "")
            {

                var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                SqlConnection con;

                con = new SqlConnection(connetionString);
                con.Open();

                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;


                strGetData = "select CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc] ,v.Description as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, v.Crcy, tc.Amount,tc.Unit,tc.UoM" +
    " from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material " +
    "inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material " +
    " inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP" +
    " on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant where TQ.RequestNumber='" + hdnReqNo.Value + "' and TB.FGCode = '" + txtpartdesc.Text.ToString() + "' and (TQ.QuoteResponseduedate BETWEEN TC.ValidFrom and TC.ValidTO) ";


                da = new SqlDataAdapter(strGetData, con);
                da.Fill(dtget);

                if (dtget.Rows.Count > 0)
                {
                    for (int i = 0; i < dtget.Rows.Count; i++)
                    {
                        var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;

                        SqlConnection con1;

                        con1 = new SqlConnection(connetionString1);
                        con1.Open();

                        DataTable dtget1 = new DataTable();

                        DataTable dtdate1 = new DataTable();
                        SqlDataAdapter da1 = new SqlDataAdapter();

                        string strGetData1 = string.Empty;
                        //updated by subash
                        strGetData1 = "update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo ='" + dtget.Rows[i].ItemArray[7].ToString() + "'";

                        da1 = new SqlDataAdapter(strGetData1, con1);
                        da1.Fill(dtget1);

                        con1.Close();
                    }
                }
                else
                {
                    var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                    SqlConnection con1;

                    con1 = new SqlConnection(connetionString1);
                    con1.Open();

                    DataTable dtget1 = new DataTable();

                    DataTable dtdate1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter();

                    string strGetData1 = string.Empty;


                    strGetData1 = "select distinct TQ.RequestDate as [Req Date],TQ.RequestNumber as [Req No], TQ.Plant,TQ.Material as [Comp Material],TQ.MaterialDesc as [Comp Material Desc]," +
        " TQ.VendorName as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, TV.Crcy from EMET.dbo.TQuoteDetails TQ " +
        " inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor" +
        " inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor and  VP.Plant = TQ.Plant  Where TQ.RequestNumber='" + hdnReqNo.Value + "' ";


                    da1 = new SqlDataAdapter(strGetData1, con1);
                    da1.Fill(dtget1);

                    if (dtget1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtget1.Rows.Count; i++)
                        {
                            var connetionString2 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;

                            SqlConnection con2;

                            con2 = new SqlConnection(connetionString2);
                            con2.Open();

                            DataTable dtget2 = new DataTable();

                            DataTable dtdate2 = new DataTable();
                            SqlDataAdapter da2 = new SqlDataAdapter();

                            string strGetData2 = string.Empty;
                            //updated by subash
                            strGetData2 = "update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo ='" + dtget1.Rows[i].ItemArray[7].ToString() + "'";

                            da2 = new SqlDataAdapter(strGetData2, con2);
                            da2.Fill(dtget2);

                            con2.Close();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No vendors for this Process group with Material!')", true);
                    }
                }
                DeleteNonRequest();

                //subash - email -Begin
                string dg_checkvalue, dg_formid = string.Empty;
                string getQuoteRef = string.Empty;
                string formatstatus = string.Empty;
                string remarks = string.Empty;
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string RequestIncNumber = "";
                int increquest = 000000;
                string RequestInc = String.Format(currentYear, increquest);
                string REQUEST = string.Concat(currentYear, RequestIncNumber1);
                string strdate = txtReqDate.Text;
                int rowscount = 0;

                //getting number of vendors
                foreach (GridViewRow gr in grdvendor.Rows)
                {
                    CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                    if (myCheckbox.Checked == true)
                    {
                        dg_checkvalue = "Y";
                        dg_formid = gr.Cells[1].Text.ToString();
                        string dg_VenName = gr.Cells[2].Text.ToString();
                        dg_checkvalue = gr.Cells[2].Text.ToString();
                        string strvendname = dg_checkvalue.ToString();
                        string mstrvenname = strvendname.Substring(0, 4);
                        string getVendName = mstrvenname[0].ToString();
                        string QuoteSearchTerm = "";
                        QuoteSearchTerm = gr.Cells[3].Text.ToString();
                        string getquote = String.Concat(QuoteSearchTerm, RequestIncNumber1);
                        string strPlatingType = txtplatingtype.Text.ToString();
                        getQuoteRef = getquote;
                        remarks = "Open Status";
                        rowscount++;
                        string Proccode = (string)HttpContext.Current.Session["prod_code"].ToString();
                        string matnum = (string)HttpContext.Current.Session["materialNumber"].ToString();
                        string strproc1 = (string)HttpContext.Current.Session["process"].ToString();
                        string PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                        string MatDesc = (string)HttpContext.Current.Session["materialDesc"].ToString();
                        string PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                        string PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();
                        string UnitNetWeight = txtunitweight.Text.ToString();
                        DateTime QuoteDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
                        string PITTypeandDesc = PITType + "- " + PITTypeDesc;
                        // getting Messageheader ID from IT Mailapp
                        var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                        using (SqlConnection cnn = new SqlConnection(Email))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
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
                            returnValue = pOutput.Value.ToString();
                            cnn.Close();
                            OriginalFilename = returnValue;
                            MHid = returnValue;
                            OriginalFilename = MHid + seqNo + formatW;
                        }

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        string test = userId;

                        //Uploading  ttachment to Mail sever using UNC credentials
                        if (fname != "")
                        {
                            try
                            {

                                var Email_insert1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                                using (SqlConnection Email_inser = new SqlConnection(Email_insert1))
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
                                    }
                                    dr.Close();
                                    Email_inser.Close();

                                    string Destination = path + OriginalFilename;
                                    using (new SoddingNetworkAuth(Userid, domain, password))
                                    {
                                        File.Copy(Source, Destination, true);
                                        SendFilename = fname.Remove(fname.Length - 4);
                                        OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
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
                            format = "NO";
                        }

                        //getting vendor mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        var Vendormail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                        using (SqlConnection cnn = new SqlConnection(Vendormail))
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

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            while (dr.Read())
                            {
                                aemail = dr.GetString(0);
                                pemail = dr.GetString(1);

                            }
                            dr.Close();
                            cnn.Close();
                        }

                        //getting User mail id
                        var usermail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                        using (SqlConnection cnn = new SqlConnection(usermail))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_userdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = userId;
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            while (dr.Read())
                            {
                                Uemail = dr.GetString(0);
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
                            string MessageHeaderId = MHid;
                            string fromname = "eMET System";
                            string FromAddress = Uemail;
                            //string Recipient = aemail + "," + pemail;
                            pemail = string.Concat(aemail, ";", pemail);
                            string Recipient = pemail;
                            string CopyRecipient = Uemail;
                            string BlindCopyRecipient = "";
                            string ReplyTo = "subashdurai@shimano.com.sg";
                            string Subject = "MailCenter from MES QA";
                            string footer = "Please Login SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                            string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation been created by 2100 - SPL by " + nameC + "<br /><br />The details are<br /><br /> Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(RequestIncNumber1.ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + "<br />  Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response Date    :   " + QuoteDate.ToString() + "<br /><br />" + footer;
                            body1 = "The details are<br /><br /> Vendor Name  :   " + dg_VenName.ToString() + "<br />  Request Number  :   " + Convert.ToInt32(RequestIncNumber1.ToString()) + "<br />  Quote Number    :   " + getQuoteRef.ToString() + " <br /> Partcode And Description :   " + txtpartdesc.Text.ToString() + "  | " + txtpartdescription.Text.ToString() + "<br />  Quotation Response Date    :   " + QuoteDate.ToString() + "<br /><br />" + footer;
                            string BodyFormat = "HTML";
                            string BodyRemark = "0";
                            string Signature = "";
                            string Importance = "High";
                            string Sensitivity = "Confidential";

                            string CreateUser = userId;
                            DateTime CreateDate = Convert.ToDateTime(DateTime.Now);
                            //end Header
                            string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                            SqlCommand Header = new SqlCommand(Head, Email_inser);
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
                            Header.Parameters.AddWithValue("@IsAttachFile", Convert.ToBoolean(IsAttachFile.ToString()));
                            Header.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                            Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            Header.CommandText = Head;
                            Header.ExecuteNonQuery();
                            //end Header
                            //Details

                            string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                            SqlCommand Detail = new SqlCommand(Details, Email_inser);
                            Detail.Parameters.AddWithValue("@MessageHeaderId", MHid.ToString());
                            Detail.Parameters.AddWithValue("@SequenceNumber", Convert.ToInt32(SequenceNumber.ToString()));
                            Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                            Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                            Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                            Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                            Detail.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                            Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                            Detail.CommandText = Details;
                            Detail.ExecuteNonQuery();


                            Email_inser.Close();

                            var Email_insert1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            using (SqlConnection Email_inser1 = new SqlConnection(Email_insert1))
                            {
                                Email_inser1.Open();
                                Details = "insert into email(quotenumber, body) values(@Quotenumber,@body)";
                                Detail = new SqlCommand(Details, Email_inser1);
                                Detail.Parameters.AddWithValue("@Quotenumber", getQuoteRef.ToString());
                                Detail.Parameters.AddWithValue("@body", body1.ToString());
                                Detail.CommandText = Details;
                                Detail.ExecuteNonQuery();
                                Email_inser1.Close();
                            }
                            //End Details
                        }
                    }
                    else
                    {
                        dg_checkvalue = "";
                    }
                }
                //End by subash
                Response.Redirect("Home.aspx");
                // Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please select the Attachment and upload !')", true);
            }
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
                        Userid = dr.GetString(0);
                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
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

        /// <summary>
        /// Load data and Create Dynamic table with No BOM Details of Create Request Button click event.
        /// </summary>
        /// <param name="reqno"></param>
        protected void GetDataforNoBom(string reqno)
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();

            DataTable dtget = new DataTable();

            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string strGetData = string.Empty;


            strGetData = "select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], TQ.Plant, '-' as 'Comp Material' ,'-' as 'Comp Material Desc',   " + //TQ.Material as [Comp Material],TQ.MaterialDesc as [Comp Material Desc] ," +
" TQ.VendorName as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, TV.Crcy from EMET.dbo.TQuoteDetails TQ " +
"inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor " +
" inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor and  VP.Plant = TQ.Plant where TQ.RequestNumber='" + reqno + "' ";


            da = new SqlDataAdapter(strGetData, con);
            da.Fill(dtget);

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
                    Hearderrow.BackColor = ColorTranslator.FromHtml("#5D7B9D");
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

                        if (rowcount > 0 && (cellCtr == 0 || cellCtr == 1 || cellCtr == 2 || cellCtr == 3 || cellCtr == 4))
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


                hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('selected vendors does not have valid Data. Please check the Master Data !')", true);
            }



            con.Close();
        }
        /// <summary>
        /// Delete unwanted requests which is not include in Process. 
        /// </summary>
        public void DeleteNonRequest()
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "delete from TQuoteDetails Where CreateStatus is null or CreateStatus = ''";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            Response.Redirect("NewRequest.aspx");
        }

        protected void ddlmatdescriptionC_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtpartdesc.Text = "";
            txtpartdescription.Text = "";
            //ddlpirtype.SelectedValue = "";
            txtPIRDesc.Text = "";
            txtunitweight.Text = "";
            txtUOM.Text = "";
            txtBaseUOM1.Text = "";
            txtmatlclass.Text = ddlmatdescriptionC.Text;
            getMatlClass(txtmatlclass.Text);
            string strmaterialclass = txtmatlclass.Text;
            getMatlClassTextChange(strmaterialclass);
            //ddlmatdescriptionC_SelectedIndexChanged1();
        }
    }
}