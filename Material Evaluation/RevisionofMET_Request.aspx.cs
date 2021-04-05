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
    public partial class RevisionofMET_Request : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {



                this.getmaterial();
                //  this.BindGridIM();
                this.getprocconnection();
                //    this.bindgridproc();
                this.submatrlcost();

                //  this.bindgridsubmat();
                this.otheritemcost();
                //   this.bindotheritem();
                this.partperunit();
                //  this.bindpartperunit();



            }
        }

        protected void getmaterial()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            //    SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



            DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[16] { new DataColumn("S.No"), new DataColumn("materialType"), new DataColumn("materialcode"), new DataColumn("matlgrade"), new DataColumn("rawmatlcost"), new DataColumn("totrawcost"), new DataColumn("matlgrsweight"), new DataColumn("matlcost"), new DataColumn("totmatlcost"), new DataColumn("runrweight"), new DataColumn("runrratio"), new DataColumn("recyclematlratio"), new DataColumn("scrapweight"), new DataColumn("scraplosallow"), new DataColumn("scrappric"), new DataColumn("scraprebate") });

            dt.Columns.AddRange(new DataColumn[14] { new DataColumn("Material Type"), new DataColumn("Material code"), new DataColumn("Matl grade"), new DataColumn("Raw matl cost/Kg(SGD) "), new DataColumn("Total Raw Matl cost/KG (SGD)"), new DataColumn("Part Net Weight"), new DataColumn("Cavity"), new DataColumn("Runner Weight/shot(gm)"), new DataColumn("Runner Ratio/Pcs(%)"), new DataColumn("Recycle matl ratio"), new DataColumn("Matl Yeild/Melting loss (%)"), new DataColumn("Matl Gross Weight(gm)"), new DataColumn("Matl cost /pcs"), new DataColumn("Total matl Cost/Pcs(SGD)") });
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
                this.Convertmatlcost();

                // this.BindGridIM();


            }
            else
            {
                this.Convertmatlcost();
            }

            con.Close();

        }

        protected void getprocconnection()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            //    SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[12] { new DataColumn("Process grp Code"), new DataColumn("Sub Process"), new DataColumn("If Turnkey - Vendorname?"), new DataColumn("Machine/labour"), new DataColumn("Machine"), new DataColumn("Rate/Hr"), new DataColumn("Process Uom Description"), new DataColumn("Base Qty"), new DataColumn("Duration per proess UOM(sec)"), new DataColumn("Efficincy per Process yeild(%)"), new DataColumn("Process Cost/pc"), new DataColumn("Total Process Cost/pcs") });
            ViewState["process"] = dt;
            DataRow dr = dt.NewRow();

            //dr[0] = "Injection Modeling";
            //dr[1] = "ROH";
            //dr[2] = "80000449";
            //dr[3] = "PLASTIC";

            //dr[4] = "AMILAN";



            dt.Rows.Add(dr);
            if (dt.Rows.Count > 0)
            {
                grdproces.DataSource = dt;

                this.Convert();

                //   BindGrid(dt, true);

                // grdproces.ShowHeader = !rotate;

                // grdproces.DataBind();
            }
            else
            {
                grdproces.DataSource = dt;
                this.Convert();

                //grdproces.ShowHeader = !rotate;
                //grdproces.DataBind();
            }

            con.Close();

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
            grdmatlcost.ShowHeader = !rotate;
            this.grdmatlcost.DataSource = dt;
            grdmatlcost.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in grdmatlcost.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
        }

        private void BindGrid(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            grdproces.ShowHeader = !rotate;
            this.grdproces.DataSource = dt;
            grdproces.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in grdproces.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
        }

        protected void Convertmatlcost()
        {
            DataTable dt = (DataTable)ViewState["Vendors_req"];

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
            BindGrid_matl(dt2, true);
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

        protected void bindgridproc()
        {

            grdproces.DataSource = (DataTable)ViewState["process"];
            //grdproces.Columns[1].Visible = true;

            //grdmatlcost.Columns[8].Visible = false;
            //grdmatlcost.Columns[9].Visible = false;
            //grdmatlcost.Columns[10].Visible = false;
            //   grdproces.Columns[2].Visible = true;
            //  grdproces.Columns[3].Visible = true;
            //  grdproces.Columns[4].Visible = true;
            //  grdproces.Columns[5].Visible = true;
            //grdproces.Columns[6].Visible = true;

            grdproces.DataBind();
        }

        protected void submatrlcost()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
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





            con.Close();

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
            grdsubmatl.ShowHeader = !rotate;
            this.grdsubmatl.DataSource = dt;
            grdsubmatl.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in grdsubmatl.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
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





            con.Close();

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
            grdotheritem.ShowHeader = !rotate;
            this.grdotheritem.DataSource = dt;
            grdotheritem.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in grdotheritem.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
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
                grdsubmatl.DataSource = dt;
                grdsubmatl.DataBind();
            }





            con.Close();

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
            grdpartunitprice.ShowHeader = !rotate;
            this.grdpartunitprice.DataSource = dt;
            grdpartunitprice.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in grdpartunitprice.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
        }




        protected void bindpartperunit()
        {

            grdpartunitprice.DataSource = (DataTable)ViewState["partperunit"];
            grdpartunitprice.Columns[0].Visible = true;
            grdpartunitprice.Columns[1].Visible = true;


            grdpartunitprice.Columns[2].Visible = true;
            grdpartunitprice.Columns[3].Visible = true;
            grdpartunitprice.Columns[4].Visible = true;
            grdpartunitprice.Columns[5].Visible = true;
            grdpartunitprice.Columns[6].Visible = true;
            grdpartunitprice.Columns[7].Visible = true;
            //grdpartunitprice.Columns[8].Visible = true;
            //grdpartunitprice.Columns[9].Visible = true;

            grdpartunitprice.DataBind();
        }


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






    }
}