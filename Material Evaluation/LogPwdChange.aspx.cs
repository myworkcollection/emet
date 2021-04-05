using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class LogPwdChange : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool IsAth;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null || Session["userType"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_LogVendorPwdChange";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            lblUser.Text = Session["UserName"].ToString();
                            lblplant.Text = Session["userType"].ToString();
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            if (Session["ShowEntryLPC"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryLPC"].ToString();
                            }
                            ShowTable();

                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }
                        }
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        protected void ShowTable()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select A.UseID,B.UseNam,A.VendorCode,C.Description,(select UseNam from Usr where UseID = A.UpdateBy) as 'UpdateBy',A.UpdateAt,
                            format(A.UpdatedOn, 'dd/MM/yyyy HH:mm:sss') as  UpdatedOn,
                            CONVERT(DateTime, A.UpdatedOn,101)as UpOn
                             from TlogPwdchange A
                            inner join Usr B on A.UseID = B.UseID 
                            inner join tVendor_New C on A.VendorCode = C.Vendor ";
                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "UseID")
                        {
                            sql += @" where A.UseID like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "UseNam")
                        {
                            sql += @" where B.UseNam like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode")
                        {
                            sql += @" where A.VendorCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorDesc")
                        {
                            sql += @" where C.Description like '%'+@Filter+'%' ";
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "UpdatedOn")
                        {
                            sql += @" Order by CONVERT(DateTime, A." + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                        }
                        else
                        {
                            sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                    }

                    cmd = new SqlCommand(sql, MDMCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        int ShowEntry = 1;
                        if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
                        {
                            ShowEntry = 1;
                            TxtShowEntry.Text = "1";
                        }
                        else
                        {
                            ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
                        }
                        GridView1.PageSize = ShowEntry;
                        Session["ShowEntryLPC"] = ShowEntry.ToString();
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";
            try
            {

                // Retrieve the last column that was sorted.
                string sortExpression = ViewState["SortExpression"] as string;

                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["SortDirection"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }

                // Save new values in ViewState.
                ViewState["SortDirection"] = sortDirection;
                ViewState["SortExpression"] = column;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            return sortDirection;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                ShowTable();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    icon.ImageUrl = "~/images/" + sorting + ".png";
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                        //tc.Controls.Add(new LiteralControl(" "));
                                        //tc.Controls.Add(icon);
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                        //icon.ImageUrl = "~/images/default.png";
                                        //tc.Controls.Add(new LiteralControl(" "));
                                        //tc.Controls.Add(icon);
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                    //icon.ImageUrl = "~/images/default.png";
                                    //tc.Controls.Add(new LiteralControl(" "));
                                    //tc.Controls.Add(icon);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GetSortDirection(e.SortExpression);
                ShowTable();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowTable();
            txtFind.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        }

        protected void LbBtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void CtnCloseMdl_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void StartTimer_Click(object sender, EventArgs e)
        {
            TimerCntDown.Enabled = true;
        }

        protected void TimerCntDown_Tick(object sender, EventArgs e)
        {
            if (TimerCntDown.Enabled == true)
            {
                int seconds = Int32.Parse((countdown.Text));
                seconds--;

                if (seconds < 0)
                {
                    TimerCntDown.Enabled = false;
                    Session.Abandon();
                    Session.Clear();
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    countdown.Text = seconds.ToString();
                }
            }
        }

        protected void sidebarToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["sidebarToggle"] == null)
                {
                    Session["sidebarToggle"] = "Hide";
                    SideBarMenu.Attributes.Add("style", "display:block;");
                }
                else
                {
                    Session["sidebarToggle"] = null;
                    SideBarMenu.Attributes.Add("style", "display:none;");
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("Login.aspx");
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }




        //protected void isAuthor()
        //{
        //    var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        //    SqlConnection conplant;
        //    conplant = new SqlConnection(connetionStringplant);
        //    string sql;
        //    SqlDataReader reader;
        //    string FormName = "EMET_LogVendorPwdChange";
        //    string System = "EMET";
        //    try
        //    {
        //        conplant.Open();
        //        //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
        //        sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["EPlant"].ToString() + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conplant);
        //        cmd.Parameters.AddWithValue("@UserID", Session["userID"].ToString());
        //        cmd.Parameters.AddWithValue("@FormName", FormName);
        //        cmd.Parameters.AddWithValue("@System", System);
        //        reader = cmd.ExecuteReader();
        //        if (!reader.HasRows)
        //        {
        //            IsAth = false;
        //        }
        //        else
        //        {
        //            IsAth = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ee);
        //    }
        //    finally
        //    {
        //        conplant.Dispose();
        //    }
        //}
    }
}