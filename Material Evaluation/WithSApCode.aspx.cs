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

namespace Material_Evaluation
{
    public partial class WithSApCode : System.Web.UI.Page
    {

        CheckBox headerCheckBox = new CheckBox();
        int incNumber = 0000;
        string DbTransName = "";

        public static Dictionary<int, string> objPirType;


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["FileUpload1"] != null)
            //{
            //	FileUpload1 = (FileUpload) Session["FileUpload1"];
            //	lblMessage.Text = FileUpload1.FileName;
            //	Image3.ImageUrl = FileUpload1.FileName;
            //}
           // lblUser1.Text = "" + Session["UserName"].ToString() + "";
            getdate();
            this.draftcost.Checked = true;

            if (!IsPostBack)
            {

                string userId = Session["userID"].ToString();
                string sname = Session["UserName"].ToString();
                string srole = Session["userType"].ToString();
                string concat = sname + " - " + srole;
                //lblUser1.Text = concat;
                //Session["UserName"] = userId;
                // GridView2.Visible = false;
                objPirType = new Dictionary<int, string>();

                //this.draftcost.Checked = false;

                getdate();
                getplant();
                matltype();
                getproctype();

                GetSHMNPIC();
                GetPirType();
                //  Getproduct();
                this.process();
                //   this.vendorload();
                //   this.bindvendor();


            }

        }

        protected void GetDbTrans()
        {
            try
            {
                string DBTrans = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                string[] ArrDbTrans = DBTrans.Split(';');
                string[] ArrDbTransName = ArrDbTrans[1].ToString().Split('=');
                DbTransName = ArrDbTransName[1].ToString().Trim() + ".[dbo].";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex + "');", true);
                DbTransName = "";
            }
        }

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

        protected void getdate()
        {

            txtReqDate.Text = DateTime.Now.Date.ToShortDateString().ToString();
            txtReqDate.Attributes.Add("disabled", "disabled");


        }

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



            //txtSAPProdID.AutoCompleteType = AutoCompleteType.;
            //ddlproduct.DataTextField = "product";
            //ddlproduct.DataTextField = "product";
            //ddlproduct.DataBind();

            con1.Close();

        }

        protected void ddlproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string prod = ddlproduct.SelectedItem.Text;
            string prod = txtSAPProdID.Text;

            Session["prod"] = prod.ToString();

            getmatlclass(prod);

        }

        protected void getmatlclass(string matlclass)
        {
            // string strprod = (string)HttpContext.Current.Session["PRODUCTCODE"].ToString();


            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matl;
            matl = new SqlConnection(connetionString1);
            matl.Open();
            DataTable dtmatl = new DataTable();
            SqlDataAdapter damatl = new SqlDataAdapter();
            // string strmatl = "select distinct Materialclassdescription from materials where product='" + matlclass + "'";

            string strmatl = " select distinct materialclassdescription,plant,product from materials where product='" + matlclass + "'";


            damatl = new SqlDataAdapter(strmatl, matl);
            dtmatl = new DataTable();
            damatl.Fill(dtmatl);
            //ddlmatlclass.DataSource = dtmatl;
            //ddlmatlclass.DataTextField = "Materialclassdescription";
            //ddlmatlclass.DataValueField = "Materialclassdescription";
            //ddlmatlclass.DataBind();
            //    ddlprodcode.Items.Insert(0, new ListItem("-- Select Prod Code --", String.Empty));

            // ddlmatlclass.Items.Insert(0, "select material class");
            matl.Close();

        }

        protected void ddlmatlclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string mat_class = ddlmatlclass.SelectedItem.Text;
            string mat_class = txtmatlclass.Text;
            Session["materialclass"] = mat_class.ToString();
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getProduct(string prefixText)
        {

            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
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


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getSapPart(string prefixText)
        {
            int splnewproc = 0;
            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            string strprod = (string)HttpContext.Current.Session["prod_code"].ToString();

            // string strprod = "CM";



            string matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            string strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            string matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            string proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            string splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            if (splproctype != "BLANK")
                splnewproc = int.Parse(splproctype);




            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";
            //  if(splproctype != "BLANK")
            //  {
            ////  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
            //  str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //      "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType='" + splnewproc + "' and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  material like '%" + prefixText + "%' ";
            //  }
            //  else

            //  {
            //      str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //      "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType IS NULL and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  material like '%" + prefixText + "%' ";
            //  }

            if (splproctype != "BLANK")
            {
                //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
                str = "Select distinct TB.FGCode from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType='" + splnewproc + "' and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGCode like '%" + prefixText + "%'";
            }
            else
            {
                str = "Select distinct TB.FGCode from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType IS NULL and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGCode like '%" + prefixText + "%'";
            }

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {

                //txtpartdescription.Text = Result.Rows[0]["MaterialDesc"].ToString();
                //txtunitweight.Text = Result.Rows[0]["UnitWeight"].ToString();
                //txtUOM.Text = Result.Rows[0]["UnitWeightUOM"].ToString();
            }

            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;


        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getSapPartdesc(string prefixText)
        {
            int splnewproc = 0;

            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            string strprod = (string)HttpContext.Current.Session["prod_code"].ToString();

            string matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            string strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            string matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            string proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            string splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            if (splproctype != "BLANK")
                splnewproc = int.Parse(splproctype);

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string str = "";
            //if (splproctype != "BLANK")
            //{
            //    //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //        "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType='" + splnewproc + "' and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  materialdesc like '%" + prefixText + "%' ";
            //}
            //else
            //{
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //    "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType IS NULL and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  materialdesc like '%" + prefixText + "%' ";
            //}

            if (splproctype != "BLANK")
            {
                //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
                str = "Select distinct TB.FGName from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType='" + splnewproc + "' and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGName like '%" + prefixText + "%'";
            }
            else
            {
                str = "Select distinct TB.FGName from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + proctype + "' and  Tm.Product='" + strprod + "' and TM.SPlProcType IS NULL and TR.Plant = '" + strplant + "' and TM.Plant ='" + strplant + "' and Tm.MaterialType = '" + matlType + "' and tm.PlantStatus='" + strplantstatus + "' and TR.ProdComDesc = '" + matrl_class_desc.ToString() + "' and TB.FGName like '%" + prefixText + "%'";
            }

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);



            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;


        }



        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getMatlClass(string prefixText)
        {

            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            string strProduct = (string)HttpContext.Current.Session["prod_code"].ToString();
            string strPlantStatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            string strMat = (string)HttpContext.Current.Session["MaterialType"].ToString();
            string strProcType = (string)HttpContext.Current.Session["proctype"].ToString();
            string strSplProctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            // string str = "select distinct MaterialClassDescription from Tmaterial where Plant = '" + strplant + "' and product='" + strProduct + "' and PlantStatus='" + strPlantStatus + "' and  MaterialClassDescription like '%" + prefixText + "%'";

            string Strnew = "";
            if (strSplProctype == "BLANK")
            {
                strSplProctype = "0";
                Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE ='" + strSplProctype + "'   and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
                // string str = " select distinct materialclassdescription from Tmaterial where materialclassdescription like '" + prefixText + "%'";
            }
            else
            {
                //Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE ='" + strSplProctype+"'   and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";

                Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE is null   and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
            }
            da = new SqlDataAdapter(Strnew, con1);
            Result = new DataTable();
            da.Fill(Result);
            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;


        }

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

        private void GetProdMaterial(string materialid)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";

            string splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            //if (splproctype != "BLANK")
            //{
            //    // string str = "select material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where material like '%" + materialid + "%' ";
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where plant='" + txtplant.Text + "' and MaterialType='" + ddlmatltype.SelectedItem.Text + "'" +
            //       "and  PlantStatus='" + ddlplantstatus.SelectedItem.Text + "' and ProcType='" + ddlproctype.SelectedItem.Text + "' and SPlProcType='" + ddlsplproctype.SelectedItem.Text + "' and Product='" + txtSAPProdID.Text + "' and MaterialClassDescription='" + txtmatlclass.Text.ToString() + "'  and  material like '%" + materialid + "%' ";
            //}
            //else
            //{
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where plant='" + txtplant.Text + "' and MaterialType='" + ddlmatltype.SelectedItem.Text + "'" +
            //       "and  PlantStatus='" + ddlplantstatus.SelectedItem.Text + "' and ProcType='" + ddlproctype.SelectedItem.Text + "' and SPlProcType IS NULL and Product='" + txtSAPProdID.Text + "' and MaterialClassDescription='" + txtmatlclass.Text.ToString() + "'  and  material like '%" + materialid + "%' ";
            //}

            if (splproctype != "BLANK")
            {
                //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
                str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + ddlproctype.SelectedItem.Text.ToString() + "' and  Tm.Product='" + txtSAPProdID.Text.ToString() + "' and TM.SPlProcType='" + ddlsplproctype.SelectedItem.Text.ToString() + "' and TR.Plant = '" + txtplant.Text.ToString() + "' and TM.Plant ='" + txtplant.Text.ToString() + "' and Tm.MaterialType = '" + ddlmatltype.SelectedItem.Text.ToString() + "' and tm.PlantStatus='" + ddlplantstatus.SelectedItem.Text.ToString() + "' and TR.ProdComDesc = '" + txtmatlclass.Text.ToString() + "' and TM.Material ='" + materialid + "'";
            }
            else
            {
                str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + ddlproctype.SelectedItem.Text.ToString() + "' and  Tm.Product='" + txtSAPProdID.Text.ToString() + "' and TM.SPlProcType IS NULL and TR.Plant = '" + txtplant.Text.ToString() + "' and TM.Plant ='" + txtplant.Text.ToString() + "' and Tm.MaterialType = '" + ddlmatltype.SelectedItem.Text.ToString() + "' and tm.PlantStatus='" + ddlplantstatus.SelectedItem.Text.ToString() + "' and TR.ProdComDesc = '" + txtmatlclass.Text.ToString() + "' and TM.Material ='" + materialid + "'";
            }


            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            if (dtdate.Rows.Count > 0)
            {

                txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
                txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();
                Session["materialDesc"] = txtpartdescription.Text.ToString();
            }

            //txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
            //txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
            //txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
            //txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();

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
            string str = "select VendorCode,VendorName,* from TVENDOR_PROCESSGROUP where processgrp='" + processgrp + "'";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            //     Result.Rows.Add(dr);

            if (Result.Rows.Count > 0)
            {
                grdvendor.DataSource = Result;
                //grdvendor.Columns[0].Visible = true;
                //grdvendor.Columns[1].Visible = true;
                //grdvendor.Columns[2].Visible = true;
                grdvendor.DataBind();
            }
            else
            {
                grdvendor.DataSource = Result;
                grdvendor.DataBind();
            }
            con1.Close();
        }

        protected void bindvendor()
        {
            grdvendor.DataSource = (DataTable)ViewState["vendorlist"];
            grdvendor.Columns[0].Visible = true;
            grdvendor.Columns[1].Visible = true;
            grdvendor.Columns[2].Visible = true;
            grdvendor.DataBind();
        }

        //protected void OnRadionew_Changed(object sender, EventArgs e)
        //{
        //    if (RadioButtonList1.SelectedItem.Text == "First Article Item")
        //    {


        //        Response.Redirect("NewRequest.aspx");


        //    }
        //    else if (RadioButtonList1.SelectedItem.Text == "Change of Vendor")
        //    {


        //        Response.Redirect("ChangeofVendor.aspx");



        //    }
        //    else if (RadioButtonList1.SelectedItem.Text == "Draft and Cost Planning")
        //    {

        //        Response.Redirect("WithSApCode.aspx");


        //    }

        //}





        protected void radiodraftcost_Changed(object sender, EventArgs e)
        {
            if (draftcost.Checked == true)
            {

                this.article.Checked = false;
                this.changevendr.Checked = false;

                //Response.Redirect("WithSApCode.aspx");


            }
          
        }



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

        protected void article_CheckedChanged(object sender, EventArgs e)
        {

            if (article.Checked == true)
            {
                this.changevendr.Checked = false;
                this.draftcost.Checked = false;
                this.article.Checked = true;
                Response.Redirect("NewRequest.aspx");
            }
        }

        protected void draftcost_CheckedChanged(object sender, EventArgs e)
        {
            if (draftcost.Checked == true)
            {
                this.changevendr.Checked = false;
                this.article.Checked = false;
                this.draftcost.Checked = true;
            }
        }



        protected void ddlmatltype_SelectedIndexChanged(object sender, EventArgs e)
        {

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant;
            matplant = new SqlConnection(connetionString1);
            matplant.Open();
            string prod = ddlmatltype.SelectedItem.Text;
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

        protected void ddlproctype_SelectedIndexChanged(object sender, EventArgs e)
        {

            string prod = ddlproctype.SelectedItem.Text;

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


                //  ddlplantstatus.Text = dtplantstaus.Rows[0]["plantstatus"].ToString();
            }
            else
            {
                // ddlplantstatus.Text = "";

            }

            //if(ddlproctype.SelectedItem.Text=="E")
            //{

            //    ddlsplproctype.Visible = false;
            //    txtsplproc.Visible = true;
            //}

            Session["proctype"] = ddlproctype.SelectedItem.Text;

            Session["splproctype"] = ddlsplproctype.SelectedItem.Text;



        }

        protected void ddlsplproctype_SelectedIndexChanged(object sender, EventArgs e)
        {

            string proc = ddlproctype.SelectedItem.Text;
            string SpProc = ddlsplproctype.SelectedItem.Text;

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

            //  txtPIRDesc.Text = dt.Rows[0]["DESCRIPTION"].ToString();

            if (dtpir.Rows.Count > 0)
            {
                ddlpirtype.DataSource = dtpir;
                ddlpirtype.DataTextField = "PIRTYPE";
                ddlpirtype.DataTextField = "PIRTYPE";

                ddlpirtype.DataBind();


                //   ddlsplproctype.Items.Insert(0, new ListItem("-- Spproctype --", String.Empty));


                //      ddlpirtype.Items.Insert(0, new ListItem("-- Select PIRTYPE --", String.Empty));


                txtPIRDesc.Text = dtpir.Rows[0]["DESCRIPTION"].ToString();
                //  ddlplantstatus.Text = dtplantstaus.Rows[0]["plantstatus"].ToString();
            }
            else
            {

                //ddlpirtype.DataSource = dt;
                //ddlpirtype.DataBind();

                // ddlplantstatus.Text = "";

            }

            Session["Pirtype"] = ddlpirtype.SelectedItem.Text;
            Session["PirDescription"] = txtPIRDesc.Text;


            Session["splproctype"] = ddlsplproctype.SelectedItem.Text;

            //     matplant1.Close();





        }



        //protected void txtSAPProdID_Unload(object sender, EventArgs e)
        //{
        //    string prod = txtSAPProdID.Text;

        //    Session["product"] = prod.ToString();  
        //}

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

        protected void ddlprocess_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["process"] = ddlprocess.SelectedItem.Text;

            string strprod = (string)HttpContext.Current.Session["process"].ToString();


            this.PIRTYPE();
            this.vendorload(strprod);
        }

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




        protected void ddlplantstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strplantstatus = ddlplantstatus.SelectedItem.Text;

            Session["plant_status"] = strplantstatus.ToString();

        }

        public void AssignHdnValues()
        {
            try
            {
                hprodID.Value = txtSAPProdID.Text;

            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }



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

        protected void txtSAPProdID_TextChanged(object sender, EventArgs e)
        {
            string strproddesc = txtSAPProdID.Text;
            string[] matl = strproddesc.Split('-');
            string getprod = matl[0].ToString();
            Session["prod_code"] = getprod.ToString();
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string vendor1 = "";
                string vendor2 = "";

                string strplantsave = (string)HttpContext.Current.Session["plant"].ToString();
                string strmatlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

                string txtplatestring = txtplatingtype.Text.ToString();

                string strdate = txtReqDate.Text;


                string dg_checkvalue, dg_formid = string.Empty;
                string getQuoteRef = string.Empty;
                string formatstatus = string.Empty;
                string remarks = string.Empty;
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();


                string RequestIncNumber = "";

                int increquest = 0000;
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
                        RequestIncNumber = "0000";
                        RequestIncNumber = string.Concat(currentYear, RequestIncNumber);

                    }
                    else
                    {
                        RequestIncNumber = ReqNum;
                    }

                    // char C = RequestIncNumber[RequestIncNumber.Length - 1];

                    int newReq = (int.Parse(RequestIncNumber)) + (1);
                    RequestIncNumber = newReq.ToString();
                    // int tempval = Convert.ToInt32(C.ToString()) + 1;

                    // string formattedIncNumber = String.Format("{000:D4}", 000 + tempval);
                    // RequestIncNumber = RequestIncNumber.Remove(RequestIncNumber.Length - 1, 1);
                    // RequestIncNumber = RequestIncNumber + tempval;


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

                        string formattedIncNumber = String.Format("{000:D4}", incNumber);
                        string getVendName = mstrvenname[0].ToString();
                        string getquote = String.Concat(mstrvenname, RequestIncNumber);
                        string strPlatingType = txtplatingtype.Text.ToString();


                        getQuoteRef = getquote;

                        remarks = "Open Status";
                        rowscount++;


                        string Proccode = (string)HttpContext.Current.Session["prod_code"].ToString();
                        //string matnum = (string)HttpContext.Current.Session["materialNumber"].ToString();
                        string strproc1 = (string)HttpContext.Current.Session["process"].ToString();

                        string PIRJobType = (string)HttpContext.Current.Session["PIRJOBTYPE"].ToString();
                        //string MatDesc = (string)HttpContext.Current.Session["materialDesc"].ToString();

                        string PITType = (string)HttpContext.Current.Session["Pirtype"].ToString();
                        string PITTypeDesc = (string)HttpContext.Current.Session["PirDescription"].ToString();

                        //string UnitNetWeight = txtunitweight.Text.ToString();
                        DateTime QuoteDate = Convert.ToDateTime(txtDate.Text.ToString());
                        //QuoteDate = QuoteDate.ToLongDateString();

                        string PITTypeandDesc = PITType + "- " + PITTypeDesc;

                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();
                        //DataTable dtdate = new DataTable();
                        //SqlDataAdapter da = new SqlDataAdapter();

                        // string query = "INSERT INTO TVENDOR_PROCESSGROUP(VendorCode,VendorName,ProcessGrp,ProcessGrpDescription,UpdateBy,UpdatedOn) VALUES (@VendorCode,@VendorName,@ProcessGrp,@ProcessGrpDescription,@UpdateBy,@UpdatedOn)";
                        string query = " INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,MaterialClass,PIRJobType,PIRType,QuoteResponseDueDate,DrawingNo,PlatingType,CreateStatus) values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@matlClass,@PIRJobtype,@PIRTypeDesc,@QuoteDue,@DrawingNo,@PlatingType,@CreateStatus)";
                        //    string text = "Data Saved!";
                        SqlCommand cmd1 = new SqlCommand(query, conins);
                        cmd1.Parameters.AddWithValue("@REQUESTNO", Convert.ToInt32(RequestIncNumber.ToString()));
                        DateTime Dtstrdate = DateTime.ParseExact(txtReqDate.Text, "dd/MM/yyyy", null);
                        strdate = Dtstrdate.ToString("yyyy-MM-dd");
                        cmd1.Parameters.AddWithValue("@REQUESTDATE", strdate.ToString());
                        cmd1.Parameters.AddWithValue("@PLANT", strplantsave.ToString());
                        cmd1.Parameters.AddWithValue("@QUOTENO", getQuoteRef.ToString());
                        cmd1.Parameters.AddWithValue("@VENDORCODE", dg_formid.ToString());
                        cmd1.Parameters.AddWithValue("@VENDORNAME", dg_VenName.ToString());
                        cmd1.Parameters.AddWithValue("@procesgrp", strproc1.ToString());
                        cmd1.Parameters.AddWithValue("@Product", Proccode.ToString());
                       // cmd1.Parameters.AddWithValue("@Material", matnum.ToString());
                        cmd1.Parameters.AddWithValue("@matlClass", strmatlClass.ToString());
                        cmd1.Parameters.AddWithValue("@PIRJobtype", PIRJobType.ToString());
                       // cmd1.Parameters.AddWithValue("@MaterialDesc", MatDesc.ToString());
                        cmd1.Parameters.AddWithValue("@PIRTypeDesc", PITTypeandDesc.ToString());
                       // cmd1.Parameters.AddWithValue("@UnitNet", UnitNetWeight.ToString());
                        cmd1.Parameters.AddWithValue("@QuoteDue", QuoteDate);
                        cmd1.Parameters.AddWithValue("@DrawingNo", imgHidden.Value.ToString());
                        cmd1.Parameters.AddWithValue("@PlatingType", strPlatingType);
                        cmd1.Parameters.AddWithValue("@CreateStatus", "Draft");




                        cmd1.CommandText = query;
                        cmd1.ExecuteNonQuery();


                    }
                    else
                    {
                        dg_checkvalue = "";
                    }
                }

                //GridView2.Visible = true;
               // GetData(RequestIncNumber.ToString());
                 Response.Redirect("Home.aspx");
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
            }
        }

        protected void txtmatlclass_TextChanged(object sender, EventArgs e)
        {

            string strmaterialclass = txtmatlclass.Text;
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //string folderPath = Server.MapPath("~/Upload/");

            ////Check whether Directory (Folder) exists.
            //if (!Directory.Exists(folderPath))
            //{
            //    //If Directory (Folder) does not exists. Create it.
            //    Directory.CreateDirectory(folderPath);
            //}

            ////Save the File to the Directory (Folder).
            //FileUpload1.SaveAs(folderPath + Path.GetFileName(FileUpload1.FileName));

            //Image1.ImageUrl = "~/Files/" + Path.GetFileName(FileUpload1.FileName);
            ////Display the success message.

            //String strmessage = Path.GetFileName(FileUpload1.FileName);

            //string strDraw = strmessage.ToString();
            //string[] matl = strDraw.Split('.');
            //string getmatl = matl[0].ToString();
            //lblMessage.Text = getmatl.ToString() + " File has been Uploaded Successfuly.";
            // Session["DrawingNo"]=lblMessage.Text;


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
            lblMessage.Text = FileUpload1.FileName + " has been Uploaded Successfuly.";


            Session["FileUpload1"] = FileUpload1;

            //Stream fs = FileUpload1.PostedFile.InputStream;
            //BinaryReader br = new BinaryReader(fs);                                 //reads the   binary files
            //Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=" + FileUpload1.FileName);     // to open file prompt Box open or Save file         
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(bytes);
            //Response.End();

        }


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

            Session["userID"] = lbluser.Text;

            consap.Close();


        }

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

        private void GetMaterialcode(string materialname)
        {
            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection consap;
            consap = new SqlConnection(connetionStringdate);
            consap.Open();
            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            int splnewproc = 0;

            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            string strprod = (string)HttpContext.Current.Session["prod_code"].ToString();

            string matrl_class_desc = (string)HttpContext.Current.Session["matlclass"].ToString();
            string strplantstatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            string matlType = (string)HttpContext.Current.Session["MaterialType"].ToString();
            string proctype = (string)HttpContext.Current.Session["proctype"].ToString();
            string splproctype = (string)HttpContext.Current.Session["splproctype"].ToString();


            string str = "";
            //if (splproctype != "BLANK")
            //{
            //    //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //        "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType='" + splnewproc + "' and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  materialdesc like '%" + materialname + "%' ";
            //}
            //else
            //{
            //    str = " select distinct material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where plant='" + strplant + "' and MaterialType='" + matlType + "'" +
            //    "and  PlantStatus='" + strplantstatus + "' and ProcType='" + proctype + "' and SPlProcType IS NULL and Product='" + strprod + "' and MaterialClassDescription='" + matrl_class_desc.ToString() + "'  and  materialdesc like '%" + materialname + "%' ";
            //}

            if (splproctype != "BLANK")
            {
                //  string str = "select str(material) + ' - ' + materialdesc from materials where product='" + strprod + "' and materialclassdescription='" + matrl_class + "' and  materialdesc like '" + prefixText + "%'";
                str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + ddlproctype.SelectedItem.Text.ToString() + "' and  Tm.Product='" + txtSAPProdID.Text.ToString() + "' and TM.SPlProcType='" + ddlsplproctype.SelectedItem.Text.ToString() + "' and TR.Plant = '" + txtplant.Text.ToString() + "' and TM.Plant ='" + txtplant.Text.ToString() + "' and Tm.MaterialType = '" + ddlmatltype.SelectedItem.Text.ToString() + "' and tm.PlantStatus='" + ddlplantstatus.SelectedItem.Text.ToString() + "' and TR.ProdComDesc = '" + txtmatlclass.Text.ToString() + "' and TM.MaterialDesc ='" + materialname + "'";
            }
            else
            {
                str = "Select distinct TM.Material,TM.MaterialDesc,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode INNER JOIN TBOMLIST TB on TM.Material = TB.FGCode where  TM.PROCTYPE = '" + ddlproctype.SelectedItem.Text.ToString() + "' and  Tm.Product='" + txtSAPProdID.Text.ToString() + "' and TM.SPlProcType IS NULL and TR.Plant = '" + txtplant.Text.ToString() + "' and TM.Plant ='" + txtplant.Text.ToString() + "' and Tm.MaterialType = '" + ddlmatltype.SelectedItem.Text.ToString() + "' and tm.PlantStatus='" + ddlplantstatus.SelectedItem.Text.ToString() + "' and TR.ProdComDesc = '" + txtmatlclass.Text.ToString() + "' and TM.MaterialDesc ='" + materialname + "'";
            }

            // string str = "select material,MaterialDesc, UnitWeight,UnitWeightUOM,Plating from Tmaterial where materialdesc='" + materialname + "' ";

            da = new SqlDataAdapter(str, consap);
            da.Fill(dtdate);

            if (dtdate.Rows.Count > 0)
            {

                txtpartdesc.Text = dtdate.Rows[0]["material"].ToString();
                // txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
                txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
                txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
                txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();

            }

            //   txtpartdesc.Text = dtdate.Rows[0]["material"].ToString();
            // txtpartdescription.Text = dtdate.Rows[0]["MaterialDesc"].ToString();
            //txtunitweight.Text = dtdate.Rows[0]["UnitWeight"].ToString();
            //txtUOM.Text = dtdate.Rows[0]["UnitWeightUOM"].ToString();
            //txtplatingtype.Text = dtdate.Rows[0]["Plating"].ToString();

            consap.Close();


        }

        protected void txtjobtypedesc_TextChanged(object sender, EventArgs e)
        {

        }

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
        }

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


                string strmaterialDesc = txtpartdescription.Text;
                string strmaterial = txtpartdesc.Text;

                //string[] matl = strmaterial.Split('-');
                //string getmatl = matl[0].ToString();

                GetMaterialcode(strmaterialDesc);
                Session["materialNumber"] = strmaterial.ToString();
                Session["materialDesc"] = txtpartdescription.Text.ToString();
            }
        }

        protected void GetData(string reqno)
        {
            GetDbTrans();
            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();

            DataTable dtget = new DataTable();

            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string strGetData = string.Empty;


            strGetData = "select CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,tm.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc] ,v.Description as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, v.Crcy, tc.Amount,tc.Unit,tc.UoM" +
" from Tmaterial tm inner join TCUSTOMER_MATLPRICING tc on tm.Material = tc.Material" +
" inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP" +
" on vp.VendorCode=v.Vendor inner join "+ DbTransName +@"TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant where TQ.RequestNumber='" + reqno + "' ";


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
                            if (txtPIRDesc.Text.Contains("SUBCON"))
                            {
                                tCell.Text = "";
                            }
                        }


                    }
                    rowcount++;

                }
                //TableCell tCellbutton = new TableCell();
                //Button btnSubmitdynamic = new Button();
                //btnSubmitdynamic.ID = "BtnSubumit";
                //btnSubmitdynamic.Width = 100;

                //btnSubmitdynamic.Text = "Submit";
                //btnSubmitdynamic.Click += BtnSubmitdynamic_Click;
                //tCellbutton.Controls.Add(btnSubmitdynamic);
                //Table1.Rows[1].Cells.Add(tCellbutton);
                //Table1.Rows[1].Cells[dtget.Rows[0].ItemArray.Length].Attributes.Add("rowspan", rowcount.ToString());

            }

            con.Close();
        }

        //private void BtnSubmitdynamic_Click(object sender, EventArgs e)
        //{
        //	Response.Redirect("NewReq_changes.aspx?Number=" + Table1.Rows[1].Cells[0].ToString());
        //	//ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Mail Send successfully to Vendors');", true);
        //}



        protected void btnViewDownPDF_Click(object sender, EventArgs e)
        {
            string filename = imgHidden.Value;

            //Stream fs = FileUpload1.PostedFile.InputStream;
            //BinaryReader br = new BinaryReader(fs);                                 //reads the   binary files
            //Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            Response.Clear();
            Response.Buffer = true;
            //string path = MapPath(filename);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.BinaryWrite(bytes);
            Response.WriteFile(Image3.ImageUrl.ToString());

            //Response.Write("<script>");
            //Response.Write("window.open('"+ Image3.ImageUrl + "', '_newtab');");
            //Response.Write("</script>");
            Response.End();


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

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
        }
    }
}


