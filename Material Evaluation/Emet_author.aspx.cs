using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

namespace Material_Evaluation
{
    public partial class Emet_author : System.Web.UI.Page
    {

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
        string PIRGeneration = "PIRGeneration";
        string Approval = "EMET_ShimanoApproval";
        string LogVendorPwdChange = "EMET_LogVendorPwdChange";
        string ManagerApproval = "EMET_ShimanoManagerApr";
        string RevisionOfMET = "EMET_RevisionOfMET";
        string AllRequest = "EMET_AllRequest";
        string RevisionReqWaitting = "EMET_RevisionReqWaitting";
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
                sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["EPlant"].ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"].ToString());
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
            if (Session["userID"] == null || Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                menu = Request.QueryString["num"];

                if (!IsPostBack)
                {
                    try
                    {
                        string userId = Session["userID"].ToString();
                        string sname = Session["UserName"].ToString();
                        string srole = Session["userType"].ToString();
                        string concat = sname + " " + srole;
                        lblUser.Text = sname;
                        lblplant.Text = srole;
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
                            FormName = PIRGeneration;
                            form = "PIRGeneration.aspx";
                        }
                        else if (menu == "17")
                        {
                            FormName = Approval;
                            form = "Approval.aspx";
                            isAuthor();
                            if (IsAth == false)
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Account dont have access for this page, please contact admin');", true);
                                form = "ReqApproval.aspx?num=17";
                                Response.Redirect(form);
                            }
                            else
                            {
                                Response.Redirect(form);
                            }

                        }

                        else if (menu == "18")
                        {
                            FormName = LogVendorPwdChange;
                            form = "LogPwdChange.aspx";
                        }

                        else if (menu == "19")
                        {
                            FormName = ManagerApproval;
                            form = "ManagerApproval.aspx";
                            isAuthor();
                            if (IsAth == false)
                            {
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Account dont have access for this page, please contact admin');", true);
                                form = "ReqApproval.aspx?num=19";
                                Response.Redirect(form);
                            }
                            else
                            {
                                Response.Redirect(form);
                            }
                        }

                        else if (menu == "20")
                        {
                            FormName = RevisionOfMET;
                            form = "Revision.aspx";
                        }

                        else if (menu == "21")
                        {
                            FormName = AllRequest;
                            form = "EAllRequest.aspx";
                        }

                        else if (menu == "22")
                        {
                            FormName = RevisionReqWaitting;
                            form = "RevisionReq_Waitting.aspx";
                        }

                        if (menu == "0") { }
                        else
                        {
                            isAuthor();
                            if (IsAth == false)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');", true);
                                //form = "ReqApproval.aspx?num=19";
                                Response.Redirect(form);
                            }
                            else
                            {
                                Response.Redirect(form);
                            }
                        }
                    }

                    catch
                    {

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