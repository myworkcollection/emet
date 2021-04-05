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
    public partial class addcolumn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            SetInitialRow(); 

        }

        private void SetInitialRow()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;

      //      dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));

            dt.Columns.Add(new DataColumn("Column1", typeof(string)));

            dt.Columns.Add(new DataColumn("Column2", typeof(string)));

            //dt.Columns.Add(new DataColumn("Column3", typeof(string)));

            dr = dt.NewRow();

        //    dr["RowNumber"] = 1;

            dr["Column1"] = string.Empty;

            dr["Column2"] = string.Empty;

            //dr["Column3"] = string.Empty;

            dt.Rows.Add(dr);

            //dr = dt.NewRow();



            //Store the DataTable in ViewState

            ViewState["CurrentTable"] = dt;

            



            //Gridview1.DataSource = dt;

            //Gridview1.DataBind();

            if (dt.Rows.Count > 0)
            {
                this.Convertmatlcost();

                // this.BindGridIM();


            }

            



        }

        protected void Convertmatlcost()
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];

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


     private void BindGrid_matl(DataTable dt, bool rotate)
        {
            // this.grdproces.DataSourceID = "";
            Gridview1.ShowHeader = !rotate;
            this.Gridview1.DataSource = dt;
            Gridview1.DataBind();
            if (rotate)
            {
                foreach (GridViewRow row in Gridview1.Rows)
                {
                    row.Cells[0].CssClass = "headewidth";
                    row.Cells[1].CssClass = "headecellwidth";
                }
            }
        }

     private void SetPreviousData()
     {

         int rowIndex = 0;

         if (ViewState["CurrentTable"] != null)
         {

             DataTable dt = (DataTable)ViewState["CurrentTable"];

             if (dt.Rows.Count > 0)
             {

                 for (int i = 0; i < dt.Rows.Count; i++)
                 {

                     TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");

                     //TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");

                     //TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");



                     box1.Text = dt.Rows[i]["Column1"].ToString();

                     //box2.Text = dt.Rows[i]["Column2"].ToString();

                     //box3.Text = dt.Rows[i]["Column3"].ToString();



                     rowIndex++;

                 }

             }

         }

     }


    

        private void AddNewRowToGrid()

    {

        int rowIndex = 0;

 

        if (ViewState["CurrentTable"] != null)

        {

            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)

            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)

                {

                    //extract the TextBox values

                    TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("TextBox1");

                    

                    //TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");

                    //TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");

 

                    drCurrentRow = dtCurrentTable.NewRow();

                  //  drCurrentRow["RowNumber"] = i + 1;

 

                    dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                   

                    //dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;

                    //dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;

 

                    rowIndex++;

                }

                dtCurrentTable.Rows.Add(drCurrentRow);

                ViewState["CurrentTable"] = dtCurrentTable;

 

                Gridview1.DataSource = dtCurrentTable;

                Gridview1.DataBind();

            }

        }

        else

        {

            Response.Write("ViewState is null");

        }

 

        //Set Previous Data on Postbacks

        SetPreviousData();

    }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

       


    }
}