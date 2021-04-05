using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class ProcGrpVsSubProc : System.Web.UI.Page
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
                if (Session["userID_"] == null || Session["UserName"] == null || Session["mappedVname"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_VendorPrcGrpVsSubPrc";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            LbVendorName.Text = Session["mappedVendor"].ToString() + "-" + Session["mappedVname"].ToString() + " ";

                            if (Session["ShowEntryProcGrpVsSubProc"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryProcGrpVsSubProc"].ToString();
                            }
                            ShowTable();
                        }
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
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
                    sql = @" select distinct A.ProcessGrpCode,A.ProcessGrpDescription,A.SubProcessCode,A.SubProcessName,A.ProcessUOM,A.ProcessUomDescription
                            from TPROCESGROUP_SUBPROCESS A 
                            inner join TVENDOR_PROCESSGROUP B on A.ProcessGrpCode = B.ProcessGrp 
                            inner join tVendor_New C on B.VendorCode = C.Vendor
                            inner join tVendorPOrg D on C.POrg = D.POrg 
                            where B.VendorCode=@VendorCode and D.Plant=@Plant and A.delflag=0  ";
                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "ProcessGrpCode")
                        {
                            sql += @" and A.ProcessGrpCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGrpDescription")
                        {
                            sql += @" and A.ProcessGrpDescription like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "SubProcessCode")
                        {
                            sql += @" and A.SubProcessCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "SubProcessName")
                        {
                            sql += @" and A.SubProcessName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessUOM")
                        {
                            sql += @" and A.ProcessUOM like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessUomDescription")
                        {
                            sql += @" and A.ProcessUomDescription like '%'+@Filter+'%' ";
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        sql += @"  Order by A." + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                    }

                    cmd = new SqlCommand(sql, MDMCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    Debug.WriteLine(Session["VPlant"].ToString());
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
                        Session["ShowEntryProcGrpVsSubProc"] = ShowEntry.ToString();
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

                return sortDirection;
            }
            catch (Exception ex)
            {
                return sortDirection;
                EMETModule.SendExcepToDB(ex);
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
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Close", "window.Dispose();", true);
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
                }
                else
                {
                    Session["sidebarToggle"] = null;
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
            catch (ThreadAbortException ex)
            {

            }
            catch (Exception ex2)
            {
                Response.Write(ex2);
                EMETModule.SendExcepToDB(ex2);
            }
        }



        //protected void isAuthor()
        //{
        //    var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        //    SqlConnection conplant;
        //    conplant = new SqlConnection(connetionStringplant);
        //    string sql;
        //    SqlDataReader reader;
        //    string FormName = "EMET_VendorPrcGrpVsSubPrc";
        //    string System = "EMET";
        //    try
        //    {
        //        conplant.Open();
        //        //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
        //        sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["VPlant"].ToString() + "'";
        //        SqlCommand cmd = new SqlCommand(sql, conplant);
        //        cmd.Parameters.AddWithValue("@UserID", Session["userID_"].ToString());
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
        //    catch (Exception ee)
        //    {
        //        LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
        //    }
        //    finally
        //    {
        //        conplant.Dispose();
        //    }
        //}
    }
}