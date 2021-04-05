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
	public partial class WithSAPCode_Request: System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

            if (!IsPostBack)
            {

                string proc_vendor = "LOCAL BASIC SDN BHD";
                string proces_group = "IM";

          //      txtprocs.Text = "IM";
                

                this.initialmatrlcost(proc_vendor, proces_group);

                this.shimanoPIC();
                this.vendcurncy();
                getprocconnection();


                if (txtprocs.Text == "IM")
                {



                    this.matrlcost(proc_vendor, proces_group);

                }



            }

            this.getprocconnection();
            DataTable dt4 = new DataTable();
            dt4.Columns.AddRange(new DataColumn[6] { new DataColumn("totmatl"), new DataColumn("totproc"), new DataColumn("totsubmatl"), new DataColumn("totothritmcost"), new DataColumn("grndtotalcost"), new DataColumn("finalquote") });
            ViewState["PartUnit"] = dt4;
            DataRow dr4 = dt4.NewRow();
            dr4[0] = "$0.2916";

            dr4[1] = "$0.0799";
            dr4[2] = "$0.0750";
            dr4[3] = "$0.0015";
            dr4[4] = "$0.4480";
            dr4[5] = "$0.4480";



            dt4.Rows.Add(dr4);



            txttotmatl.Text = dt4.Rows[0]["totmatl"].ToString();
            txttotproc.Text = dt4.Rows[0]["totproc"].ToString();
            txttotsub.Text = dt4.Rows[0]["totsubmatl"].ToString();
            txttotother.Text = dt4.Rows[0]["totothritmcost"].ToString();
            txtgrandtot.Text = dt4.Rows[0]["grndtotalcost"].ToString();
            txtfinalquote.Text = dt4.Rows[0]["finalquote"].ToString();



            //     this.BindGrid4();

        }

        protected void BindGrid()
        {
            //  grdmatlcost.EditIndex = 9;
            grdmatlcost.DataSource = (DataTable)ViewState["Vendors_req"];
            grdmatlcost.Columns[5].Visible = false;

            grdmatlcost.Columns[8].Visible = false;
            grdmatlcost.Columns[9].Visible = false;
            grdmatlcost.Columns[10].Visible = false;
            grdmatlcost.Columns[11].Visible = false;
            grdmatlcost.Columns[12].Visible = false;
            grdmatlcost.Columns[13].Visible = false;
            grdmatlcost.Columns[14].Visible = false;

            grdmatlcost.DataBind();
        }

        protected void BindGridIM()
        {
            //  grdmatlcost.EditIndex = 9;
            grdmatlcost.DataSource = (DataTable)ViewState["Vendors_req"];
            grdmatlcost.Columns[5].Visible = false;

            //grdmatlcost.Columns[8].Visible = false;
            //grdmatlcost.Columns[9].Visible = false;
            //grdmatlcost.Columns[10].Visible = false;
            grdmatlcost.Columns[11].Visible = false;
            grdmatlcost.Columns[12].Visible = false;
            grdmatlcost.Columns[13].Visible = false;
            grdmatlcost.Columns[14].Visible = false;

            grdmatlcost.DataBind();
        }

        protected void getprocconnection()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");


            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;

            DataTable dt = new DataTable();

            sda.Fill(dt);

            grdproces.DataSource = dt;
            grdproces.DataBind();


            //grdproces.FooterRow.Cells[4].Text = "Total Process Cost/pcs ($)";
            //grdproces.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            //grdproces.FooterRow.Cells[5].BackColor = System.Drawing.Color.LightBlue;

            con.Close();

        }

        protected void BindGrid1()
        {

            grdproces.DataSource = (DataTable)ViewState["request"];
            grdproces.DataBind();
        }
        protected void BindGrid2()
        {
            //grdsubmatl.DataSource = (DataTable)ViewState["newrequest"];
            //grdsubmatl.DataBind();
        }

        protected void BindGrid3()
        {
            //  grdothercost.EditIndex = 4;
            //grdothercost.DataSource = (DataTable)ViewState["Othercost"];
            //grdothercost.DataBind();
        }
        //protected void BindGrid4()
        //{
        //    grdpartunitprice.DataSource = (DataTable)ViewState["PartUnit"];
        //    grdpartunitprice.DataBind();
        //}

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdproces.PageIndex = e.NewPageIndex;
            this.getprocconnection();
        }

        protected void initialmatrlcost(string vendor, string proc_code)
        {


            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection conplant;
            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();
            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();



            //string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorid='" + proc_code + "'  and procesGrp='" + vendor + "'";

            string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorname='" + vendor + "' and procesGrp='" + proc_code + "' ";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            txtreqdate.Text = plant.Rows[0]["reqdate"].ToString();
            txtvendr.Text = plant.Rows[0]["vendorname"].ToString();

            txt_vendorpic.Text = plant.Rows[0]["PICName"].ToString();
            txtauterisedEMAIL.Text = plant.Rows[0]["PICemail"].ToString();
            txtreq.Text = plant.Rows[0]["requestNo"].ToString();
            // txtreq.Text = plant.Rows[0]["requestNo"].ToString();
            txtprod.Text = plant.Rows[0]["prodcode"].ToString();
            txtprocs.Text = plant.Rows[0]["procesGrp"].ToString();
            //   txtpart.Text = plant.Rows[0]["partcode"].ToString();
            txtpartdesc.Text = plant.Rows[0]["partcode"].ToString();
            txtSAPJobType.Text = plant.Rows[0]["SAPJOBType"].ToString();
            txtPIRtype.Text = plant.Rows[0]["PIRJObcode"].ToString();
            txtpirdesc.Text = plant.Rows[0]["PIRDESC"].ToString();







            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("S.No"), new DataColumn("materialcode"), new DataColumn("matlgrade"), new DataColumn("rawmatlcost"), new DataColumn("totrawcost"), new DataColumn("matlgrsweight"), new DataColumn("matlcost"), new DataColumn("totmatlcost"), new DataColumn("runrweight"), new DataColumn("runrratio"), new DataColumn("recyclematlratio"), new DataColumn("scrapweight"), new DataColumn("scraplosallow"), new DataColumn("scrappric"), new DataColumn("scraprebate") });
            ViewState["Vendors_req"] = dt;

            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "ROH";
            dr[2] = "80000449";

            dr[3] = "AMILAN";
            dr[4] = "4.79";
            dr[5] = "6.1090";

            dr[6] = "13.58";
            dr[7] = "0.0829";

            dr[8] = "0.2916";
            //   dr[9] = "6";
            dr[10] = "8";
            dr[11] = "33";

            dt.Rows.Add(dr);

            //mygrid.Columns[2].Visible = false;

            this.BindGridIM();

            conplant.Close();
        }

        protected void matrlcost(string proc_code, string vendor)
        {


            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection conplant;
            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();
            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();



            //string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorid='" + proc_code + "'  and procesGrp='" + vendor + "'";

            string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorname='" + proc_code + "' and procesGrp='" + vendor + "' ";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            txtreqdate.Text = plant.Rows[0]["reqdate"].ToString();
            txtvendr.Text = plant.Rows[0]["vendorname"].ToString();

            txt_vendorpic.Text = plant.Rows[0]["PICName"].ToString();
            txtauterisedEMAIL.Text = plant.Rows[0]["PICemail"].ToString();
            txtreq.Text = plant.Rows[0]["requestNo"].ToString();
            txtreq.Text = plant.Rows[0]["requestNo"].ToString();
            txtprod.Text = plant.Rows[0]["prodcode"].ToString();
            txtprocs.Text = plant.Rows[0]["procesGrp"].ToString();
            //  txtpart.Text = plant.Rows[0]["partcode"].ToString();
            txtpartdesc.Text = plant.Rows[0]["partcode"].ToString();
            txtSAPJobType.Text = plant.Rows[0]["SAPJOBType"].ToString();
            txtPIRtype.Text = plant.Rows[0]["PIRJObcode"].ToString();
            txtpirdesc.Text = plant.Rows[0]["PIRDESC"].ToString();







            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[15] { new DataColumn("S.No"), new DataColumn("materialcode"), new DataColumn("matlgrade"), new DataColumn("rawmatlcost"), new DataColumn("totrawcost"), new DataColumn("matlgrsweight"), new DataColumn("matlcost"), new DataColumn("totmatlcost"), new DataColumn("runrweight"), new DataColumn("runrratio"), new DataColumn("recyclematlratio"), new DataColumn("scrapweight"), new DataColumn("scraplosallow"), new DataColumn("scrappric"), new DataColumn("scraprebate") });
            ViewState["Vendors_req"] = dt;

            DataRow dr = dt.NewRow();
            dr[0] = "1";
            dr[1] = "ROH";
            dr[2] = "80000449";

            dr[3] = "AMILAN";
            dr[4] = "4.79";
            dr[5] = "6.1090";

            dr[6] = "13.58";
            dr[7] = "0.0829";

            dr[8] = "0.2916";
            dr[9] = "6";
            dr[10] = "8";
            dr[11] = "33";

            dt.Rows.Add(dr);

            //mygrid.Columns[2].Visible = false;

            this.BindGridIM();

            conplant.Close();
        }


        protected void shimanoPIC()
        {


            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection conplant;
            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();
            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();



            //string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorid='" + proc_code + "'  and procesGrp='" + vendor + "'";

            string str = "select pic1,PIC1Email,* from SMNproductPIC where product='RD'";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            //txtreqdate.Text = plant.Rows[0]["reqdate"].ToString();
            //txtvendr.Text = plant.Rows[0]["vendorname"].ToString();

            txtsmnpic.Text = plant.Rows[0]["pic1"].ToString();
            txtemail.Text = plant.Rows[0]["PIC1Email"].ToString();


            conplant.Close();
        }


        protected void vendcurncy()
        {


            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection conplant;
            conplant = new SqlConnection(connetionStringplant);
            conplant.Open();
            DataTable plant = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();



            //string str = "select a.reqdate,a.requestNo,a.vendorId,a.vendorname,a.vendorID,VE.PICName,VE.PICemail,a.prodcode,a.partcode,A.procesGrp,a.SAPJOBType,a.PIRJObcode,a.PIRDESC from approval as a  inner join VendorPICwithEmail as VE on a.vendorid=ve.vendorcode where  a.vendorid='" + proc_code + "'  and procesGrp='" + vendor + "'";

            string str = "select  f21,* from VendorwithCurrencyinfo";
            da = new SqlDataAdapter(str, conplant);
            da.Fill(plant);

            //txtreqdate.Text = plant.Rows[0]["reqdate"].ToString();
            //txtvendr.Text = plant.Rows[0]["vendorname"].ToString();

            txtcurncy.Text = plant.Rows[0]["f21"].ToString();



            conplant.Close();
        }


    }
}