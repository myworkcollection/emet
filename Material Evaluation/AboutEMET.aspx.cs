using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class AboutEMET : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["EPlant"] != null)
                {
                    if (Session["userID"] == null || Session["UserName"] == null)
                    {
                        Response.Redirect("Login.aspx?auth=200");
                    }
                    else
                    {
                        if (!Page.IsPostBack)
                        {
                            string userId = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;
                            lblUser.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            //MenuListEmployee.Visible = true;
                            MenuListVendor.Visible = false;

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
                else if (Session["VPlant"] != null)
                {
                    if (Session["userID_"] == null || Session["UserName"] == null)
                    {
                        Response.Redirect("Login.aspx?auth=200");
                    }
                    else
                    {
                        if (!Page.IsPostBack)
                        {
                            string userId = Session["userID_"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string mappeduserid = Session["mappedVendor"].ToString();
                            string mappedname = Session["mappedVname"].ToString();

                            string concat = sname + " - " + mappedname;
                            lblUser.Text = sname;
                            lblplant.Text = mappedname;
                            //MenuListEmployee.Visible = false;
                            MenuListVendor.Visible = true;

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
                else
                {
                    Response.Redirect("Login.aspx?auth=200");
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

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            if (Session["EPlant"] != null)
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                Response.Redirect("vendor.aspx");
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
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
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
    }
}