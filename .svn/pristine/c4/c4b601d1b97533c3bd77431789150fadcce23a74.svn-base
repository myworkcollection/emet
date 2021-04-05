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


namespace Material_Evaluation
{
    public partial class RevisionofMET : System.Web.UI.Page
    {
        public string getmat;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                getdate();
                this.vendorload();
                this.bindvendor();
            }

        }

        protected void getdate()
        {

            var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection condate;

            condate = new SqlConnection(connetionStringdate);
            condate.Open();

            DataTable dtdate = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string str = "SELECT CONVERT(DATE, GETDATE())";
            da = new SqlDataAdapter(str, condate);
            da.Fill(dtdate);

            txt_date.Text = dtdate.Rows[0]["column1"].ToString();

            Session["requestdate"] = txt_date.Text;


            condate.Close();

        }

        protected void getplant()
        {

            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection conplant;

            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();

            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string str = "select PlantID,name from plant";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            txtplant.Text = plant.Rows[0]["PlantID"].ToString();

            Session["plant"] = txtplant.Text;




            conplant.Close();

        }

        protected void OnRadio_Changed(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedItem.Text == "by Vendor")
            {
                txtplant.BackColor = System.Drawing.Color.LightGray;
                txtprocdesc.ReadOnly = true;
                txtprocdesc.BackColor = System.Drawing.Color.LightGray;
                // txtproddesc.BackColor = System.Drawing.Color.LightGray;
                //                ddlproduct.ForeColor = System.Drawing.Color.LightGray;
                ddlproduct.BackColor = System.Drawing.Color.LightGray;
                  txtpartdesc.BackColor = System.Drawing.Color.LightGray;
                  ddljobtypedesc.BackColor = System.Drawing.Color.LightGray;
                  DropDownList1.BackColor = System.Drawing.Color.LightGray;
            //    txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                txtplantstatus.BackColor = System.Drawing.Color.LightGray;
                txtproctype.BackColor = System.Drawing.Color.LightGray;
                txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                txtPIRType.BackColor = System.Drawing.Color.LightGray;
                txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                txtunitweight.BackColor = System.Drawing.Color.LightGray;
                txtUOM.BackColor = System.Drawing.Color.LightGray;
                ddlmatlclass.BackColor = System.Drawing.Color.LightGray;
                txtvendor.BackColor = System.Drawing.Color.Transparent;
                txtvendrID.BackColor = System.Drawing.Color.Transparent;


            }
            else if (RadioButtonList1.SelectedItem.Text == "by Product")
            {
                ddlproduct.BackColor = System.Drawing.Color.Transparent;
                txtplant.BackColor = System.Drawing.Color.LightGray;
                txtvendrID.ReadOnly = true;
                txtvendrID.BackColor = System.Drawing.Color.LightGray;
                txtvendor.BackColor = System.Drawing.Color.LightGray;
                //   txtpartdesc.BackColor = System.Drawing.Color.LightGray;
              //  txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                txtplantstatus.BackColor = System.Drawing.Color.LightGray;
                txtproctype.BackColor = System.Drawing.Color.LightGray;
                txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                txtPIRType.BackColor = System.Drawing.Color.LightGray;
                txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                txtunitweight.BackColor = System.Drawing.Color.LightGray;
                txtUOM.BackColor = System.Drawing.Color.LightGray;
                ddlmatlclass.BackColor = System.Drawing.Color.LightGray;

                txtpartdesc.BackColor = System.Drawing.Color.LightGray;
                ddljobtypedesc.BackColor = System.Drawing.Color.LightGray;
                DropDownList1.BackColor = System.Drawing.Color.LightGray;


            }
        }



        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> getvendorid(string prefixText)
        {

            // string strprod = (string)HttpContext.Current.Session["prodcode"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            string str = " select distinct (PR.product) + ' - ' + PR.productdescription as productdetails from product as PR inner join plant as p on p.plantID=pr.plant inner join materials as MT on MT.plant=PR.plant where PR.productdescription like '" + prefixText + "%'";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);
            List<string> Output = new List<string>();
            for (int i = 0; i < Result.Rows.Count; i++)
                Output.Add(Result.Rows[i][0].ToString());

            return Output;

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
                //grdvendor.DataSource = dt;
                //grdvendor.DataBind();
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

        protected void allChkBox_CheckedChanged(object sender, EventArgs e)
        {
            bool b = (sender as CheckBox).Checked;

            foreach (ListItem chkitem in allchkbox.Items)
            {
              //  chkitem.Selected = allchkbox.SelectedItem.Selected;

                getmat += chkitem.Text + ",";

            }
        //    Session["new_changeable"] = getmat.ToString();
        }

        //protected void Submit(object sender, EventArgs e)
        //{
        //    message = "Texts    Values";
        //    foreach (ListItem item in chkFruits.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            message += "\\n";
        //            message += item.Text + "    " + item.Value; ;

        //        }
        //    }

        //    Session["KeyName"] = message;
        //    Response.Redirect("Page2.aspx");
        //}


        protected void CheckBoxList1_CheckedChanged(object sender, EventArgs e)
        {
            bool b = (sender as CheckBox).Checked;

            foreach (ListItem chkitem in allchkbox.Items)
            {
                //if (chkitem.Selected)
                //{
                  //  getmat += ",";
                    getmat += chkitem.Text + ","  ;

                //}


            }


            

            //bool b = (sender as CheckBox).Checked;

            //for (int i = 0; i < allchkbox.Items.Count; i++)
            //{
            //    allchkbox.Items[i].Selected = b;
            //    getmat = allchkbox.Items[i].Selected.ToString();




            //}
    //        Session["new_changeable"] = getmat.ToString();

            



            //string temp = string.Empty;

            //string str = string.Empty;
            //string strname = string.Empty;


            //strname = strname.Trim(",".ToCharArray());
            //string[] data = strname.Trim().Split(',');

            //string[] id = str.Trim().Split(',');
            //string vendor = id[0].Split(' ')[0];

            //string name = string.Empty;
            //for (int i = 0; i < data.Length; i++)
            //{
            //    name = i == 0 ? data[i].Split(' ')[1] : data[i];


            //}

            //Session["changeable"] = name.ToString();



           
            




        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

         
            foreach (ListItem item in allchkbox.Items)
            {
                if (item.Selected)
                {

                    getmat += item.Text + ","; ;

                }
            }

            Session["KeyName"] = getmat;

          




            //bool b = (sender as CheckBox).Checked;

            //for (int i = 0; i < allchkbox.Items.Count; i++)
            //{
            //    allchkbox.Items[i].Selected = b;
            //    getmat = allchkbox.Items[i].Selected.ToString();




            //}
            //Session["new_changeable"] = getmat.ToString();
            //Response.Redirect("RevisionofMET_Request.aspx");


        }

        protected void article_CheckedChanged(object sender, EventArgs e)
        {

            if (article.Checked == true)
            {
                this.changevendr.Checked = false;
               // this.draftcost.Checked = false;
                Response.Redirect("RevisionofMET.aspx");


            }


        }

        protected void changevendr_CheckedChanged(object sender, EventArgs e)
        {

            if (changevendr.Checked == true)
            {
                this.article.Checked = false;
               // this.draftcost.Checked = false;

                Response.Redirect("DesignChnge.aspx");


            }


        }



         



    }
}