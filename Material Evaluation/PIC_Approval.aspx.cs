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
    public partial class PIC_Approval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {



            }
        }

        protected void partperunit()
        {

            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

            SqlConnection con;

            con = new SqlConnection(connetionString);
            con.Open();
            // SqlCommand cmd = new SqlCommand("select  distinct pg.SubProcessName ,pg.processgrpcode,pg.ProcessUomDescription,pg.processgrpdescription,ML.HourlyRate from  PrsGrpvsSubProcess as pg left join MachineTypevsHourlyRate as ML on ML.ProcessGrp=pg.processgrpcode where pg.processgrpcode='" + txtprocs.Text + "' and ML.MACHINETYPE='Progressive'  and (ML.FromTonnage between 501 and 601 ) and pg.SubProcessName in ('ALUMINUM CASTING','ZINC CASTING','REAMING','BORING','DRILLING','DEBURRING')");



            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("S.No"), new DataColumn("requestno"), new DataColumn("plant"), new DataColumn("status"), new DataColumn("requeststatus"), new DataColumn("vendorname"), new DataColumn("city"), new DataColumn("Comments") });
            ViewState["partperunit"] = dt;
            DataRow dr = dt.NewRow();

            dt.Rows.Add(dr);
            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                grdPICApproval.DataSource = dt;
                grdPICApproval.DataBind();
            }





            con.Close();

        }


        protected void bindpartperunit()
        {
            

            grdPICApproval.DataSource = (DataTable)ViewState["partperunit"];
            grdPICApproval.Columns[0].Visible = true;
            grdPICApproval.Columns[1].Visible = true;


            grdPICApproval.Columns[2].Visible = true;
            grdPICApproval.Columns[3].Visible = true;
            grdPICApproval.Columns[4].Visible = true;
            grdPICApproval.Columns[5].Visible = true;
            grdPICApproval.Columns[6].Visible = true;
            grdPICApproval.Columns[7].Visible = true;
            //grdpartunitprice.Columns[8].Visible = true;
            //grdpartunitprice.Columns[9].Visible = true;

            grdPICApproval.DataBind();
        }

    }
}