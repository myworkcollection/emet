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
    public partial class MasterData : System.Web.UI.Page
    {
        bool IsAth = false;
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
                    if (Session["sidebarToggle"] == null)
                    {
                        SideBarMenu.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        SideBarMenu.Attributes.Add("style", "display:none;");
                    }

                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_VendorMasterData";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            lblUser.Text = Session["UserName"].ToString();
                            lblplant.Text = Session["mappedVname"].ToString();
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                        }

                        if (Session["UnreadAnn"].ToString() != "")
                        {
                            lbUnreadAnn.Text = Session["UnreadAnn"].ToString() + " Unread Announcement";
                        }
                        else
                        {
                            LiUnReadAnn.Style.Add("display", "none");
                            lbUnreadAnn.Text = "";
                        }
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
            }
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
            catch (ThreadAbortException ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            catch (Exception ex2)
            {
                Response.Write(ex2);
            }
        }


        //protected void isAuthor()
        //{
        //    var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        //    SqlConnection conplant;
        //    conplant = new SqlConnection(connetionStringplant);
        //    string sql;
        //    SqlDataReader reader;
        //    string FormName = "EMET_VendorMasterData";
        //    string System = "EMET";
        //    try
        //    {
        //        conplant.Open();
        //        //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserID and formname=@FormName and System=@System";
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
        //        conplant.Close();
        //    }
        //}
    }
}