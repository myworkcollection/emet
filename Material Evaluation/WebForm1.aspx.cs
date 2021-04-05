using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace WebApplication2
{
   public partial class WebForm1 : System.Web.UI.Page
   {

      protected void Page_Load(object sender, EventArgs e)
      {
         if (!Page.IsPostBack)
         {
            SetInitialRow();
         }
      }

      private void SetInitialRow()
      {
         DataTable dt = new DataTable();
         DataRow dr = null;
         dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
         dt.Columns.Add(new DataColumn("Column1", typeof(string)));
         dt.Columns.Add(new DataColumn("Column2", typeof(string)));
         dt.Columns.Add(new DataColumn("Column3", typeof(string)));
         dt.Columns.Add(new DataColumn("Column4", typeof(string)));
         dt.Columns.Add(new DataColumn("Column5", typeof(string)));
         dt.Columns.Add(new DataColumn("Column6", typeof(string)));
         dt.Columns.Add(new DataColumn("Column7", typeof(string)));
         dt.Columns.Add(new DataColumn("Column8", typeof(string)));
         dt.Columns.Add(new DataColumn("Column9", typeof(string)));
         dt.Columns.Add(new DataColumn("Column10", typeof(string)));

       //  dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Material code"), new DataColumn("Material Description"), new DataColumn("Raw matl cost/Kg(SGD) "), new DataColumn("Total Raw Matl cost/KG (SGD)"), new DataColumn("Part Net Weight"), new DataColumn("Cavity"), new DataColumn("Runner Weight/shot(gm)"), new DataColumn("Runner Ratio/Pcs(%)"), new DataColumn("Recycle matl ratio"), new DataColumn("Matl Yeild/Melting loss (%)"), new DataColumn("Matl Gross Weight(gm)"), new DataColumn("Matl cost /pcs"), new DataColumn("Total matl Cost/Pcs(SGD)") });
         ViewState["Vendors_req"] = dt;

          dr = dt.NewRow();


         dr = dt.NewRow();
       //  dr["RowNumber"] = 1;
         dr["Column1"] = string.Empty;
         dr["Column2"] = string.Empty;
         dr["Column3"] = string.Empty;
         dr["Column4"] = string.Empty;
         dr["Column5"] = string.Empty;
         dr["Column6"] = string.Empty;
         dr["Column7"] = string.Empty;
         dr["Column8"] = string.Empty;
         dr["Column9"] = string.Empty;
         dr["Column10"] = string.Empty;
         dt.Rows.Add(dr);

         //Store the DataTable in ViewState
         ViewState["CurrentTable"] = dt;

         Gridview1.DataSource = dt;
         Gridview1.DataBind();
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
                  TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                  TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");
                  TextBox box4 = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("TextBox4");
                  TextBox box5 = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("TextBox5");
                  TextBox box6 = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("TextBox6");
                  TextBox box7 = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("TextBox7");
                  TextBox box8 = (TextBox)Gridview1.Rows[rowIndex].Cells[8].FindControl("TextBox8");
                  TextBox box9 = (TextBox)Gridview1.Rows[rowIndex].Cells[9].FindControl("TextBox9");
                  TextBox box10 = (TextBox)Gridview1.Rows[rowIndex].Cells[10].FindControl("TextBox10");

                  drCurrentRow = dtCurrentTable.NewRow();
                  drCurrentRow["RowNumber"] = i + 1;

                  dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                  dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                  dtCurrentTable.Rows[i - 1]["Column3"] = box3.Text;
                  dtCurrentTable.Rows[i - 1]["Column4"] = box4.Text;
                  dtCurrentTable.Rows[i - 1]["Column5"] = box5.Text;
                  dtCurrentTable.Rows[i - 1]["Column6"] = box6.Text;
                  dtCurrentTable.Rows[i - 1]["Column7"] = box7.Text;
                  dtCurrentTable.Rows[i - 1]["Column8"] = box8.Text;
                  dtCurrentTable.Rows[i - 1]["Column9"] = box9.Text;
                  dtCurrentTable.Rows[i - 1]["Column10"] = box10.Text;

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
                  TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                  TextBox box3 = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("TextBox3");
                  TextBox box4 = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("TextBox4");
                  TextBox box5 = (TextBox)Gridview1.Rows[rowIndex].Cells[5].FindControl("TextBox5");
                  TextBox box6 = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("TextBox6");
                  TextBox box7 = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("TextBox7");
                  TextBox box8 = (TextBox)Gridview1.Rows[rowIndex].Cells[8].FindControl("TextBox8");
                  TextBox box9 = (TextBox)Gridview1.Rows[rowIndex].Cells[9].FindControl("TextBox9");
                  TextBox box10 = (TextBox)Gridview1.Rows[rowIndex].Cells[10].FindControl("TextBox10");

                  box1.Text = dt.Rows[i]["Column1"].ToString();
                  box2.Text = dt.Rows[i]["Column2"].ToString();
                  box3.Text = dt.Rows[i]["Column3"].ToString();
                  box4.Text = dt.Rows[i]["Column4"].ToString();
                  box5.Text = dt.Rows[i]["Column5"].ToString();
                  box6.Text = dt.Rows[i]["Column6"].ToString();
                  box7.Text = dt.Rows[i]["Column7"].ToString();
                  box7.Text = dt.Rows[i]["Column8"].ToString();
                  box7.Text = dt.Rows[i]["Column9"].ToString();
                  box7.Text = dt.Rows[i]["Column10"].ToString();

                  rowIndex++;
               }
            }
         }
      }
      
      //protected void ButtonAdd_Click(object sender, EventArgs e)
      //{
      //   AddNewRowToGrid();
      //}

      protected void ButtonAddCol_Click(object sender, EventArgs e)
      {
         //Gridview1.Columns[0].Visible = true;
         //Gridview1.Columns[1].Visible = true;
         //Gridview1.Columns[2].Visible = false;
         //Gridview1.Columns[3].Visible = false;
         //Gridview1.Columns[4].Visible = false;
         //Gridview1.Columns[5].Visible = false;
         //Gridview1.Columns[6].Visible = false;

         int i = 0;
         foreach (DataControlField col in Gridview1.Columns)
         {
            if (!col.Visible)
            {
               if (col.HeaderText == "Header " + i)
               {
                  col.Visible = true;
                  return;
               }
            }
            i++;
         }

        
      }
   }
}