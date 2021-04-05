using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class VndLabRate : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID_"] == null || Session["UserName"] == null || Session["mappedVname"] == null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {
                if (!IsPostBack)
                {
                    GetPlant();
                    lblUser.Text = Session["UserName"].ToString();
                    lblplant.Text = Session["mappedVname"].ToString();
                }
            }
        }

        protected void OpenSqlConnection()
        {
            var connetionString = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            con = new SqlConnection(connetionString);
            con.Open();
        }

        private void GetPlant()
        {
            OpenSqlConnection();
            try
            {
                sql = @" select Plant from USERVSVENDOR where UseID = @UseID and DelFlag=0 ";
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Session["VndPlant"] = reader["Plant"].ToString();
                    }
                }
                else
                {
                    Session["VndPlant"] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void ShowTable()
        {
            try
            {
                OpenSqlConnection();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select CAST(ROUND(StdLabourRateHr,2) AS DECIMAL(12,2))as 'StdLabourRateHr',VendorCountry,Currency,
                             (case when FollowStdRate = 'Y' then 'YES' else 'NO' end) as  'FollowStdRate'
                              from TVENDORLABRCOST where delflag=0 and plant = @plant and VendorCode=@VendorCode ";
                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "StdLabourRateHr")
                        {
                            sql += @" and StdLabourRateHr like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCountry")
                        {
                            sql += @" and VendorCountry like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Currency")
                        {
                            sql += @" and Currency like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "FollowStdRate")
                        {
                            sql += @" and FollowStdRate like '%'+@Filter+'%' ";
                        }

                    }
                    cmd = new SqlCommand(sql, con);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    cmd.Parameters.AddWithValue("@plant", Session["VndPlant"].ToString());
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();
                        }
                        else
                        {
                            LbTtlRecords.Text = "Total Record : 0";
                        }
                    }
                }
                UpdatePanel1.Update();

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con.Close();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                ShowTable();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowTable();
            txtFind.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "HideLoading();", true);
        }

        protected void LbBtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}