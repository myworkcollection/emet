using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

namespace Material_Evaluation
{
    public partial class Emet_author_V : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;
        string mappeduserid;
        string mappedname;

        bool IsAth;
        string System = "EMET";
        string Home = "EMET_Home";
        string Newrequest = "EMET_Newrequest";
        string VendorPending = "EMET_VendorPending";
        string PICPending = "EMET_PICPending";
        string ManagerPending = "EMET_ManagerPending";
        string DirPending = "EMET_DirPending";
        string PIRPending = "EMET_PIRPending";
        string Announcement = "EMET_Announcement";
        string Vendorsubmission = "EMET_Vendorsubmission";
        string ShimanoPending = "EMET_ShimanoPending";
        string Closed = "EMET_Closed";
        string Approved = "EMET_Approved";
        string Rejected = "EMET_Rejected";
        string VendorAnnouncement = "EMET_VendorAnnouncement";
        string VendorHome = "EMET_VendorHome";
        string MasterData = "EMET_VendorMasterData";
        string ProcGrpVsSubProc = "EMET_VendorPrcGrpVsSubPrc";
        string VendorPrcGrp = "EMET_VendorProcessGrp";
        string VndMachineList = "EMET_VendorMachineList";
        string VndPicMail = "EMET_VendorPICMail";
        string VndChangePwd = "EMET_VendorChangePwd";
        string FormName;
        string menu;
        string form;
        protected void isAuthor()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string sql;
            SqlDataReader reader;
            try
            {
                MDMCon.Open();
                //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
                sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["VPlant"].ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UserID", Session["userID_"].ToString());
                cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@System", System);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    IsAth = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');window.location='Home.aspx';", true);
                }
                else
                {
                    IsAth = true;
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
        //end

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID_"] == null || Session["UserName"]==null || Session["userType"]==null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {
                menu = Request.QueryString["num"];

                if (!IsPostBack)
                {
                    userId = Session["userID_"].ToString();
                    sname = Session["UserName"].ToString();
                    srole = Session["userType"].ToString();
                    mappeduserid = Session["mappedVendor"].ToString();
                    mappedname = Session["mappedVname"].ToString();
                    concat = sname + " - " + mappedname;
                    lblUser.Text = sname;
                    lblplant.Text = mappedname;

                    LbsystemVersion.Text = Session["SystemVersion"].ToString();
                    menu = Request.QueryString["num"];
                    if (menu == "1")
                    {
                        FormName = Home;
                        form = "Home.aspx";

                    }
                    else if (menu == "2")
                    {
                        FormName = Newrequest;
                        form = "NewRequest.aspx";
                    }

                    else if (menu == "3")
                    {
                        FormName = VendorPending;
                        form = "Request_Waiting.aspx";
                    }
                    else if (menu == "4")
                    {
                        FormName = PICPending;
                        form = "PICStatus.aspx";
                    }
                    else if (menu == "5")
                    {
                        FormName = ManagerPending;
                        form = "Manager_Status.aspx";
                    }
                    else if (menu == "6")
                    {
                        FormName = DirPending;
                        form = "DIRStatus.aspx";
                    }
                    else if (menu == "7")
                    {
                        FormName = PIRPending;
                        form = "ClosedStatus.aspx";
                    }
                    else if (menu == "8")
                    {
                        FormName = Announcement;
                        form = "Eannouncement.aspx";
                    }
                    //vendor
                    else if (menu == "9")
                    {
                        FormName = Vendorsubmission;
                        form = "Request_Waiting_vendor.aspx";
                    }
                    else if (menu == "10")
                    {
                        FormName = ShimanoPending;
                        form = "WSMNApproval.aspx";
                    }
                    else if (menu == "11")
                    {
                        FormName = Closed;
                        form = "CClosed.aspx";
                    }
                    else if (menu == "12")
                    {
                        FormName = Approved;
                        form = "VApproved.aspx";
                    }
                    else if (menu == "13")
                    {
                        FormName = Rejected;
                        form = "CRejected.aspx";
                    }
                    else if (menu == "14")
                    {
                        FormName = VendorAnnouncement;
                        form = "Vannouncement.aspx";
                    }
                    else if (menu == "15")
                    {
                        FormName = VendorHome;
                        form = "Vendor.aspx";
                    }
                    else if (menu == "16")
                    {
                        FormName = MasterData;
                        form = "MasterData.aspx";
                    }
                    else if (menu == "17")
                    {
                        FormName = ProcGrpVsSubProc;
                        form = "ProcGrpVsSubProc.aspx";
                    }
                    else if (menu == "18")
                    {
                        FormName = VendorPrcGrp;
                        form = "VndPrcGrp.aspx";
                    }
                    else if (menu == "19")
                    {
                        FormName = VndMachineList;
                        form = "VndMachineList.aspx";
                    }
                    else if (menu == "20")
                    {
                        FormName = VndPicMail;
                        form = "VndPICEmail.aspx";
                    }
                    else if (menu == "21")
                    {
                        FormName = VndChangePwd;
                        form = "ChangePwd.aspx";
                    }

                    if (menu == "0"){}
                    else
                    {
                        isAuthor();
                        if (IsAth == false)
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');", true);
                            if (menu == "15")
                            {
                                string UI = Session["userID_"].ToString();
                                string FN = "EMET_RealTimeVendInv";
                                string PL = Session["VPlant"].ToString();
                                if (EMETModule.IsAuthor(UI, FN, PL) == false)
                                {
                                    Response.Redirect("Emet_author_V.aspx?num=0");
                                }
                                else
                                {
                                    Response.Redirect("RealTimeVendInv.aspx");
                                }
                            }
                            else
                            {
                                Response.Redirect("Emet_author_V.aspx?num=0");
                            }
                        }
                        else
                        {
                            Response.Redirect(form);
                        }
                    }

                    if (Session["UnreadAnn"] != null)
                    {
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
                    else
                    {
                        LiUnReadAnn.Style.Add("display", "none");
                        lbUnreadAnn.Text = "";
                    }
                }
            }
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
                Response.Write(ex);
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

            }
            catch (Exception ex2)
            {
                Response.Write(ex2);
            }
        }
        
    }
}