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
using System.Text;
using System.Drawing;

namespace Material_Evaluation
{
    public partial class ChangeofVendor : System.Web.UI.Page
    {
        int incNumber = 0000;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.changevendr.Checked = true;
            getdate();
            if (!IsPostBack)
            {

                string userId = Session["userID"].ToString();
                string sname = Session["UserName"].ToString();
                string srole = Session["userType"].ToString();
                string concat = sname + " - " + srole;
                lblUser.Text = sname;
                lblplant.Text = srole;
                // Session["UserName"] = userId;

                getdate();
                getplant();
                GetPlantStatus();
                this.changevendr.Checked = true;
                this.RadioButtonList1.SelectedIndex = 0;
                txtprodID.Enabled = false;
                txtmatlclass.Enabled = false;
                ddlplantstatus.Enabled = false;
              //  this.vendorload();
              //this.bindvendor();
                process();
            }

        }

        protected void getdate()
        {

            txtReqDate.Text = DateTime.Now.Date.ToShortDateString().ToString();
            txtReqDate.Attributes.Add("disabled", "disabled");


        }

        protected void GetPlantStatus()
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection matplant;
            matplant = new SqlConnection(connetionString1);
            matplant.Open();
            string plantstatus = "select distinct plantstatus from tMaterialTypevsPlantStatus ";
            SqlDataAdapter daplantstatus = new SqlDataAdapter();
            daplantstatus = new SqlDataAdapter(plantstatus, matplant);
            DataTable dtplantstaus = new DataTable();
            daplantstatus.Fill(dtplantstaus);

            if (dtplantstaus.Rows.Count > 0)
            {

                ddlplantstatus.DataSource = dtplantstaus;
                ddlplantstatus.DataTextField = "plantstatus";
                ddlplantstatus.DataValueField = "plantstatus";
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
        }

        protected void getplant()
        {

            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection conplant;

            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();

            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string str = "select Plant,Description from TPLANT";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            txtplant.Text = plant.Rows[0]["Plant"].ToString();
            Session["plant"] = Convert.ToInt32(txtplant.Text);
            conplant.Close();

        }



        protected void OnRadio_Changed(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedItem.Text == "by Vendor")
            {
               // txtplant.BackColor = System.Drawing.Color.LightGray;
                //txtprodID.BackColor = System.Drawing.Color.LightGray;
                //txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                //txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                //ddlplantstatus.BackColor = System.Drawing.Color.LightGray;
                //txtproctype.BackColor = System.Drawing.Color.LightGray;
                //txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                //txtPIRType.BackColor = System.Drawing.Color.LightGray;
                //txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                //txtunitweight.BackColor = System.Drawing.Color.LightGray;
                //txtUOM.BackColor = System.Drawing.Color.LightGray;
                //txtmatlclass.BackColor = System.Drawing.Color.LightGray;


                // txtvendor.BackColor = System.Drawing.Color.Transparent;
                // txtvendrID.BackColor = System.Drawing.Color.Transparent;
                txtVendorID.Enabled = true;
                txtvendorName.Enabled = true;

                Session["prod_code"] = "";
                Session["matlclass"] = "";

                txtprodID.Enabled = false;
                txtmatlclass.Enabled = false;
                ddlplantstatus.Enabled = false;
                txtprodID.Text = "";
                txtmatlclass.Text = "";
                ddlplantstatus.SelectedIndex = -1;
                

            }
            else if (RadioButtonList1.SelectedItem.Text == "by Product")
            {
                //txtprodID.BackColor = System.Drawing.Color.Transparent;
                //txtplant.BackColor = System.Drawing.Color.LightGray;
                // txtvendrID.ReadOnly = true;
                // txtvendrID.BackColor = System.Drawing.Color.LightGray;
                //txtvendor.BackColor = System.Drawing.Color.LightGray;
                Session["VendorID"] = "";
                txtprodID.Enabled = true;
                txtmatlclass.Enabled = true;
                ddlplantstatus.Enabled = true;
                txtVendorID.Enabled = false;
                txtvendorName.Enabled = false;
             //   txtpartdesc.BackColor = System.Drawing.Color.LightGray;
                //txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                //txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                //ddlplantstatus.BackColor = System.Drawing.Color.LightGray;
                //txtproctype.BackColor = System.Drawing.Color.LightGray;
                //txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                //txtPIRType.BackColor = System.Drawing.Color.LightGray;
                //txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                //txtunitweight.BackColor = System.Drawing.Color.LightGray;
                //txtUOM.BackColor = System.Drawing.Color.LightGray;
                //txtmatlclass.BackColor = System.Drawing.Color.LightGray;
            }
        }



        protected void txtprodID_TextChanged(object sender, EventArgs e)
        {
            string strproddesc = txtprodID.Text;
            string[] matl = strproddesc.Split('-');
            string getprod = matl[0].ToString();
            Session["prod_code"] = getprod.ToString();

        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getVendorName(string prefixText)
        {
           
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "SELECT Description FROM tVendor_New Where Description like '%" + prefixText + "%' ORDER BY Description ASC";

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
        public static List<string> getVendorID(string prefixText)
        {
            
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "SELECT Vendor FROM tVendor_New Where Vendor like '%" + prefixText + "%' ORDER BY Description ASC";

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

            string strplant = "";
            string VendorId = "";
            string ProductId = "";
            string strMtlClass = "";
            if (HttpContext.Current.Session["plant"] != null)
                strplant = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["VendorID"] != null)
                VendorId = (string)HttpContext.Current.Session["VendorID"].ToString();
            if (HttpContext.Current.Session["prod_code"] != null)
                ProductId = (string)HttpContext.Current.Session["prod_code"].ToString();
            if (HttpContext.Current.Session["matlclass"] != null)
                strMtlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";

            if(VendorId != null && VendorId != "")
            {
                str = "select Distinct Material from TQuoteDetails Where VendorCode1 = '" + VendorId + "' and Plant = '" + strplant + "' and Material != '' and Material like '%" + prefixText + "%' ";
            }
            else if(ProductId!= "" && strMtlClass !="")
            {
                str = "select distinct Material from TQuoteDetails where Product = '" + ProductId + "' and MaterialClass = '"+ strMtlClass + "' and Material like '%"+ prefixText + "%'";
            }

           

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;
        }

        private void GetVendorDesc(string vendorCode)
        {
            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "SELECT Description FROM tVendor_New Where Vendor = '"+ vendorCode + "'  ORDER BY Description ASC";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if(Result.Rows.Count > 0)
            {
                txtvendorName.Text = Result.Rows[0][0].ToString();
            }
            //List<string> Output = new List<string>();
            //for (int i = 0; i < Result.Rows.Count; i++)
            //    Output.Add(Result.Rows[i][0].ToString());
            
        }

        private void GetVendorCode(string vendorName)
        {
            string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "SELECT Vendor FROM tVendor_New Where Description = '" + vendorName + "'  ORDER BY Description ASC";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count >0)
            {
                txtVendorID.Text = Result.Rows[0][0].ToString();
            }
            //List<string> Output = new List<string>();
            //for (int i = 0; i < Result.Rows.Count; i++)
            //    Output.Add(Result.Rows[i][0].ToString());

        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getProductCV(string prefixText)
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
        public static List<string> getMatlClassCV(string prefixText)
        {

            string strplant = "";
            string VendorId = "";
            string strProduct = "";
            string strMtlClass = "";
            if (HttpContext.Current.Session["plant"] != null)
                strplant = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["VendorID"] != null)
                VendorId = (string)HttpContext.Current.Session["VendorID"].ToString();
            if (HttpContext.Current.Session["prod_code"] != null)
                strProduct = (string)HttpContext.Current.Session["prod_code"].ToString();
            if (HttpContext.Current.Session["matlclass"] != null)
                strMtlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

            //strMtlClass = (string)HttpContext.Current.Session["matlclass"].ToString();
            //string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            //string strProduct = (string)HttpContext.Current.Session["prod_code"].ToString();
            // string strPlantStatus = (string)HttpContext.Current.Session["plant_status"].ToString();
            //string strMat = (string)HttpContext.Current.Session["MaterialType"].ToString();
            //string strProcType = (string)HttpContext.Current.Session["proctype"].ToString();
            //string strSplProctype = (string)HttpContext.Current.Session["splproctype"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            // string str = "select distinct MaterialClassDescription from Tmaterial where Plant = '" + strplant + "' and product='" + strProduct + "' and PlantStatus='" + strPlantStatus + "' and  MaterialClassDescription like '%" + prefixText + "%'";

            string Strnew = "";
            //if (strSplProctype == "BLANK")
            //{

            //    Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.PROCTYPE = '" + strProcType + "' and  TM.product='" + strProduct + "' and TM.SplPROCTYPE IS NULL   and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and Tm.MaterialType = '" + strMat + "' and tm.PlantStatus='" + strPlantStatus + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
            //    // string str = " select distinct materialclassdescription from Tmaterial where materialclassdescription like '" + prefixText + "%'";
            //}
            //else
           // {
                Strnew = " Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.product='" + strProduct + "' and TR.Plant = '" + strplant + "' and TM.Plant = '" + strplant + "' and  TR.ProdComDesc like '%" + prefixText + "%'";
            //}
            da = new SqlDataAdapter(Strnew, con1);
            Result = new DataTable();
            da.Fill(Result);
            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;


        }

        private void GetExistingQuote(string MaterialCode)
        {


            string strplant = "";
            string VendorId = "";
            string ProductId = "";
            string strMtlClass = "";
            if (HttpContext.Current.Session["plant"] != null)
                strplant = (string)HttpContext.Current.Session["plant"].ToString();

            if (HttpContext.Current.Session["VendorID"] != null)
                VendorId = (string)HttpContext.Current.Session["VendorID"].ToString();
            if (HttpContext.Current.Session["prod_code"] != null)
                ProductId = (string)HttpContext.Current.Session["prod_code"].ToString();
            if (HttpContext.Current.Session["matlclass"] != null)
                strMtlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

            //string strplant = (string)HttpContext.Current.Session["plant"].ToString();
            //string VendorId = (string)HttpContext.Current.Session["VendorID"].ToString();

            //string ProductId = (string)HttpContext.Current.Session["prod_code"].ToString();

            //string strMtlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";

            if (VendorId != null && VendorId != "")
            {
                str = "select distinct QuoteNo,SAPProcType,SAPSpProcType,PIRType,NetUnit,Product,MaterialClass,PlantStatus,PlatingType,QuoteResponseDueDate,VendorCode1,MaterialDesc from TQuoteDetails Where VendorCode1 = '" + VendorId + "' and Plant = '" + strplant + "' and Material != '' and Material ='" + MaterialCode + "' ";
            }
            else if (ProductId != "" && strMtlClass != "")
            {
                str = "select distinct QuoteNo,SAPProcType,SAPSpProcType,PIRType,NetUnit,Product,MaterialClass,PlantStatus,PlatingType,QuoteResponseDueDate,VendorCode1,MaterialDesc from TQuoteDetails where Product = '" + ProductId + "' and MaterialClass = '" + strMtlClass + "' and Material ='" + MaterialCode + "'";
            }

            //str = "select distinct QuoteNo,SAPProcType,SAPSpProcType,PIRType,NetUnit,Product,MaterialClass,PlantStatus,PlatingType,QuoteResponseDueDate from TQuoteDetails Where VendorCode1 = '" + VendorId + "' and Plant = '"+ strplant + "' and Material ='"+ MaterialCode + "' ";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            ddlExistingcode.DataSource = "";

            if (Result.Rows.Count > 0)
            {

                ddlExistingcode.DataSource = Result;
                ddlExistingcode.DataTextField = "QuoteNo";
                ddlExistingcode.DataValueField = "QuoteNo";
                ddlExistingcode.Items.Insert(0, new ListItem("-- Select Status --", String.Empty));
                ddlExistingcode.DataBind();

                txtproctype.Text = Result.Rows[0][1].ToString();
                txtSAPspProctype.Text = Result.Rows[0][2].ToString();

                string strPIRType = Result.Rows[0][3].ToString();

                string[] matl = strPIRType.Split('-');
                txtPIRType.Text = matl[0].ToString();
                txtPIRDesc.Text = matl[1].ToString();

                txtunitweight.Text = Result.Rows[0][4].ToString();

                hdnPIR.Value = strPIRType;
                hdnprodID.Value = Result.Rows[0][5].ToString();
                hdnMtlClass.Value = Result.Rows[0][6].ToString();
                hdnPlantStatus.Value = Result.Rows[0][7].ToString();
                txtplatingtype.Text = Result.Rows[0][8].ToString();
                txtQRDate.Text = Convert.ToDateTime(Result.Rows[0][9].ToString()).ToShortDateString().ToString();
                hdnVendorID.Value = Result.Rows[0][10].ToString();
                hdnMaterialDesc.Value = Result.Rows[0][11].ToString();


            }
            //List<string> Output = new List<string>();
            //for (int i = 0; i < Result.Rows.Count; i++)
            //    Output.Add(Result.Rows[i][0].ToString());


        }

        protected void txtmatlclass_TextChanged(object sender, EventArgs e)
        {

            string strmaterialclass = txtmatlclass.Text;
            Session["matlclass"] = strmaterialclass.ToString();


        }

        protected void ddlplantstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strplantstatus = ddlplantstatus.SelectedItem.Text;

            Session["plant_status"] = strplantstatus.ToString();

        }

        protected void ddlprocess_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["process"] = ddlprocess.SelectedItem.Text;

            string strprod = (string)HttpContext.Current.Session["process"].ToString();


            this.PIRTYPE();
            this.vendorload(strprod);
        }

        protected void vendorload(string processgrp)
        {
            List<string> lstVendorId = new List<string>();

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

            for(int i=0; i<Result.Rows.Count;i++)
            {
                lstVendorId.Add(Result.Rows[i][0].ToString());
            }

            //grdvendor.DataSource = Result;

            //grdvendor.DataBind();
            con1.Close();

            string strprod1 = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString11 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con11;
            con11 = new SqlConnection(connetionString11);
            con11.Open();
            DataTable Result1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter();
            //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
            string str1 = "Select VendorCode1 from TQuoteDetails Where RequestNumber IN(select RequestNumber from TQuoteDetails where QuoteNo = '"+ddlExistingcode.SelectedItem.Text.ToString()+"') order by VendorCode1 asc";
            da1 = new SqlDataAdapter(str1, con11);
            Result1 = new DataTable();
            da1.Fill(Result1);

            //grdvendor.DataSource = Result;
            //grdvendor.DataBind();
            con1.Close();


            DataTable DTCopy = new DataTable();

            DTCopy = Result;
            for (int j = 0; j < Result1.Rows.Count; j++)
            {
                for (int i = 0; i < Result.Rows.Count; i++)
                {
                    if (Result.Rows[i][0].ToString() == Result1.Rows[j][0].ToString())
                    {
                        DTCopy.Rows.Remove(Result.Rows[i]);
                    }
                }
            }

            grdvendor.DataSource = DTCopy;
            grdvendor.DataBind();

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
        protected void txtjobtypedesc_TextChanged(object sender, EventArgs e)
        {

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
            //Image3.ImageUrl = "~/Files/" + Path.GetFileName(FileUpload1.FileName);
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

        protected void txtpartdesc_TextChanged1(object sender, EventArgs e)
        {
           
                string strmaterial = txtpartdesc.Text;

            GetExistingQuote(strmaterial);
            
        }

        protected void vendorload()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            // SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("S.No"), new DataColumn("VendorId"), new DataColumn("VendorName") });
            ViewState["vendorlist"] = dt;
            DataRow dr = dt.NewRow();

            dt.Rows.Add(dr);
            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                grdvendor.DataSource = dt;
                grdvendor.DataBind();
            }





            con.Close();

        }

        protected void bindvendor()
        {

            //grdvendor.DataSource = (DataTable)ViewState["vendorlist"];
            //grdvendor.Columns[0].Visible = true;
            //grdvendor.Columns[1].Visible = true;


            //grdvendor.Columns[2].Visible = true;


            //grdvendor.DataBind();
        }

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

        //protected void OnRadionew_Changed(object sender, EventArgs e)
        //{
        //    if (RadioButtonList2.SelectedItem.Text == "First Article Item")
        //    {


        //        Response.Redirect("NewRequest.aspx");


        //    }
        //    else if (RadioButtonList2.SelectedItem.Text == "Change of Vendor")
        //    {


        //        Response.Redirect("ChangeofVendor.aspx");



        //    }
        //    else if (RadioButtonList2.SelectedItem.Text == "Draft and Cost Planning")
        //    {

        //        Response.Redirect("WithSApCode.aspx");


        //    }

        //}


        protected void changevendr_CheckedChanged(object sender, EventArgs e)
        {

            if (changevendr.Checked == true)
            {
                this.article.Checked = false;
                this.draftcost.Checked = false;
                this.changevendr.Checked = true;
                // Response.Redirect("ChangeofVendor.aspx");


            }



        }

        protected void article_CheckedChanged(object sender, EventArgs e)
        {

            if (article.Checked == true)
            {
                this.changevendr.Checked = false;
                this.draftcost.Checked = false;
                
                Response.Redirect("NewRequest.aspx");


            }



        }

        protected void draftcost_CheckedChanged(object sender, EventArgs e)
        {

            if (draftcost.Checked == true)
            {
                this.changevendr.Checked = false;
                this.article.Checked = false;
                Response.Redirect("WithSApCode.aspx");


            }



        }

        protected void txtvendorName_TextChanged(object sender, EventArgs e)
        {
            GetVendorCode(txtvendorName.Text);

            Session["VendorID"] = txtVendorID.Text;
        }

        protected void txtVendorID_TextChanged(object sender, EventArgs e)
        {
            GetVendorDesc(txtVendorID.Text);

            Session["VendorID"] = txtVendorID.Text;
        }

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

        protected void ddlExistingcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAllDetails();
        }

        private void GetAllDetails()
        {

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = "";
           
                str = "select SAPProcType,SAPSpProcType,PIRType,NetUnit,Product,MaterialClass,PlantStatus,PlatingType,QuoteResponseDueDate,VendorCode1,MaterialType,MaterialDesc from TQuoteDetails Where QuoteNo ='" + ddlExistingcode.SelectedItem.Text.ToString() + "'";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {
                txtproctype.Text = Result.Rows[0][0].ToString();
                txtSAPspProctype.Text = Result.Rows[0][1].ToString();

                string strPIRType = Result.Rows[0][2].ToString();

                string[] matl = strPIRType.Split('-');
                txtPIRType.Text = matl[0].ToString();
                txtPIRDesc.Text = matl[1].ToString();

                txtunitweight.Text = Result.Rows[0][3].ToString();

                hdnPIR.Value = strPIRType;
                hdnprodID.Value = Result.Rows[0][4].ToString();
                hdnMtlClass.Value = Result.Rows[0][5].ToString();
                hdnPlantStatus.Value = Result.Rows[0][6].ToString();
                txtplatingtype.Text = Result.Rows[0][7].ToString();
                txtQRDate.Text = Convert.ToDateTime(Result.Rows[0][8].ToString()).ToShortDateString().ToString();

                hdnVendorID.Value = Result.Rows[0][9].ToString();
                hdnMaterialType.Value  = Result.Rows[0][10].ToString();
                hdnMaterialDesc.Value = Result.Rows[0][11].ToString();


            }

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {

                //string strplantsave = (string)HttpContext.Current.Session["plant"].ToString();
                //string strmatlClass = (string)HttpContext.Current.Session["matlclass"].ToString();

                //string txtplatestring = txtplatingtype.Text.ToString();

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

                    int newReq = (int.Parse(RequestIncNumber)) + (1);
                    RequestIncNumber = newReq.ToString();

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
                        //string strPlatingType = txtplatingtype.Text.ToString();


                        getQuoteRef = getquote;

                        remarks = "Open Status";
                        rowscount++;


                        string Proccode = hdnprodID.Value.ToString();
                        string matnum = txtpartdesc.Text.ToString() ;
                        string strproc1 = ddlprocess.SelectedItem.Text.ToString();

                        string PIRJobType = txtjobtypedesc.Text.ToString();
                        string MatDesc = hdnMaterialDesc.Value.ToString();
                        string Matclass = hdnMtlClass.Value.ToString();

                        string PITType = txtPIRType.Text.ToString();
                        string PITTypeDesc = txtPIRDesc.Text.ToString();

                        string UnitNetWeight = txtunitweight.Text.ToString();
                        DateTime QuoteDate = Convert.ToDateTime(txtQRDate.Text.ToString());
                        //QuoteDate = QuoteDate.ToLongDateString();

                        string PITTypeandDesc = PITType + "- " + PITTypeDesc;

                        var connetionStringdate = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        SqlConnection conins;
                        conins = new SqlConnection(connetionStringdate);
                        conins.Open();
                        //DataTable dtdate = new DataTable();
                        //SqlDataAdapter da = new SqlDataAdapter();

                        // string query = "INSERT INTO TVENDOR_PROCESSGROUP(VendorCode,VendorName,ProcessGrp,ProcessGrpDescription,UpdateBy,UpdatedOn) VALUES (@VendorCode,@VendorName,@ProcessGrp,@ProcessGrpDescription,@UpdateBy,@UpdatedOn)";
                        string query = " INSERT INTO TQuoteDetails(RequestNumber,RequestDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,Product,Material,MaterialClass,PIRJobType,MaterialDesc,PIRType,NetUnit,QuoteResponseDueDate,DrawingNo,PlatingType,SAPProcType,SAPSPProcType,MaterialType,PlantStatus) values (@REQUESTNO,@REQUESTDATE,@PLANT,@QUOTENO,@VENDORCODE,@VENDORNAME,@procesgrp,@Product,@Material,@matlClass,@PIRJobtype,@MaterialDesc,@PIRTypeDesc,@UnitNet,@QuoteDue,@DrawingNo,@PlatingType,@SAPProcType,@SAPSPProcType,@MaterialType,@PlantStatus)";
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
                        cmd1.Parameters.AddWithValue("@Material", matnum.ToString());
                        cmd1.Parameters.AddWithValue("@matlClass", Matclass);
                        cmd1.Parameters.AddWithValue("@PIRJobtype", PIRJobType.ToString());
                        cmd1.Parameters.AddWithValue("@MaterialDesc", MatDesc.ToString());
                        cmd1.Parameters.AddWithValue("@PIRTypeDesc", PITTypeandDesc.ToString());
                        cmd1.Parameters.AddWithValue("@UnitNet", UnitNetWeight.ToString());
                        cmd1.Parameters.AddWithValue("@QuoteDue", QuoteDate);
                        cmd1.Parameters.AddWithValue("@DrawingNo", imgHidden.Value.ToString());
                        cmd1.Parameters.AddWithValue("@PlatingType", txtplatingtype.Text.ToString());
                        cmd1.Parameters.AddWithValue("@SAPProcType", txtproctype.Text.ToString());
                        cmd1.Parameters.AddWithValue("@SAPSPProcType", txtSAPspProctype.Text.ToString());
                        cmd1.Parameters.AddWithValue("@MaterialType", hdnMaterialType.Value.ToString());
                        cmd1.Parameters.AddWithValue("@PlantStatus", ddlplantstatus.SelectedItem.Text.ToString());

                        cmd1.CommandText = query;
                        cmd1.ExecuteNonQuery();


                    }
                    else
                    {
                        dg_checkvalue = "";
                    }
                }

                //GridView2.Visible = true;
                GetData(RequestIncNumber.ToString());
                // Response.Redirect("NewReq_changes.aspx?Number=" + RequestIncNumber.ToString());
            }
            catch (Exception ex)
            {
            }

        }

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


            strGetData = "select CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No], tm.Plant,tm.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc] ,v.Description as [Vendor Name],TQ.vendorCode1 as [Vendor Code], TQ.QuoteNo,vp.PICName,vp.PICemail, v.Crcy, tc.Amount,tc.Unit" +
" from Tmaterial tm inner join TCUSTOMER_MATLPRICING tc on tm.Material = tc.Material" +
" inner join tVendor_New v on v.CustomerNo = tc.Customer inner join TVENDORPIC as VP" +
" on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant where TQ.RequestNumber='" + reqno + "' ";


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
                            if (txtPIRDesc.Text.Contains("SUBCON"))
                            {
                                tCell.Text = "";
                            }
                        }


                    }
                    rowcount++;

                }

                hdnReqNo.Value = dtget.Rows[0].ItemArray[1].ToString();
            }

            con.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

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
" on vp.VendorCode=v.Vendor inner join EMET.dbo.TQuoteDetails TQ on Vp.VendorCode = TQ.VendorCode1 and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant where TQ.RequestNumber='" + hdnReqNo.Value + "' ";


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

                    strGetData1 = "update TQuoteDetails SET CreateStatus = 'Article' where QuoteNo ='" + dtget.Rows[i].ItemArray[7].ToString() + "'";

                    da1 = new SqlDataAdapter(strGetData1, con1);
                    da1.Fill(dtget1);

                    con1.Close();
                }
            }




            Response.Redirect("NewReq_changes.aspx?Number=" + Session["ReqNoDT"].ToString());
        }
    }

}