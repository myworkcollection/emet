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
using System.Threading;

namespace Material_Evaluation
{
    public partial class Vendor : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;
        string mappeduserid;
        string mappedname;

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID_"] == null || Session["UserName"] == null || Session["VPlant"].ToString() == null || Session["mappedVendor"].ToString() == null)
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
                        string FN = "EMET_VendorHome";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            Session["VndReqWaitFilter"] = null;
                            Session["VndReqWaitPgNo"] = null;

                            Session["VndWSMNApprFilter"] = null;
                            Session["VndWSMNApprPgNo"] = null;

                            Session["VndApprFilter"] = null;
                            Session["VndApprPgNo"] = null;

                            Session["VndRejectFilter"] = null;
                            Session["VndRejectPgNo"] = null;

                            Session["VndClosedFilter"] = null;
                            Session["VndClosedPgNo"] = null;

                            Session["VndReqWaitFilterMass"] = null;
                            Session["VndReqWaitPgNoMass"] = null;

                            Session["AllReqFilter"] = null;
                            Session["VndReportFilter"] = null;

                            userId = Session["userID_"].ToString();
                            sname = Session["UserName"].ToString();
                            srole = Session["userType"].ToString();
                            mappeduserid = Session["mappedVendor"].ToString();
                            mappedname = Session["mappedVname"].ToString();
                            concat = sname + " - " + mappedname;
                            lblUser.Text = sname;
                            lblplant.Text = mappedname;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            //  Session["UserName"] = userId;

                            EMETModule.AutoUpdateRequest();
                            EMETModule.autoupdate(Session["VPlant"].ToString());
                            EMETModule.autoCloseWhitoutsapCode(Session["VPlant"].ToString());
                            EMETModule.CekAndCloseExpiredRequest(Session["VPlant"].ToString());

                            this.vendorresponse();
                            this.Shimanoresponse();
                            this.closed();
                            this.approved();
                            this.Rejected();
                            this.WithoutSAPCode();
                            this.WithoutSAPCodeGp();
                            this.MassRevision();
                            this.VndAnncmntUnread();
                            this.AllReq();
                            EMETModule.RealTimeVendInvLastCheck(Session["userID_"].ToString());
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
        
        protected void VndAnncmntUnread()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"select 
                        (select count (*) from tAnnouncement  where DelFlag=0 ) as 'TotContent',
                        (select count(*) from tAnnouncement A 
                        join tAnnReadBy B on A.id=B.id where B.UseID = @UseID and A.DelFlag=0 ) as 'Read' ";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int TotContent = (int)reader["TotContent"];
                        int Read = (int)reader["Read"];
                        int Unread = TotContent - Read;
                        if (Unread == 0)
                        {
                            LbUnread.Text = "";
                            Session["UnreadAnn"] = "";
                        }
                        else
                        {
                            LbUnread.Text = "New : " + Unread.ToString();
                            LbUnread.ForeColor = System.Drawing.Color.Red;
                            Session["UnreadAnn"] = Unread;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session["UnreadAnn"] = "";
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }
        
        protected void vendorresponse()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails  where vendorcode1='" + mappeduserid.ToString() + @"' 
                                and  ApprovalStatus = 0 and (CreateStatus != '' OR CreateStatus IS NOT NULL) and Plant = '" + Session["VPlant"].ToString() + @"' 
                                and (CreateStatus <> '' or CreateStatus is not null) and (isMassRevision = 0 or isMassRevision is null) "; // and PICApprovalStatus IS NULL";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblvenrresponse.Text = strcount.ToString();
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }
        
        protected void MassRevision()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select 
                    (select count(*) from TQuoteDetails A
                    where A.vendorcode1='" + mappeduserid.ToString() + @"' 
                    and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                    and (CreateStatus <> '' or CreateStatus is not null)
                    and (plant ='" + Session["VPlant"].ToString() + @"') and (isUseSAPCode = 1) 
                    and QuoteNoRef is null and (isMassRevision=1) 
                    and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.isMassRevision = 1)) as 'waittingforsubmition',

                    (select count(*) from TQuoteDetails A
                    where A.vendorcode1='" + mappeduserid.ToString() + @"' 
                    and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                    and (CreateStatus <> '' or CreateStatus is not null)
                    and (plant ='" + Session["VPlant"].ToString() + @"') and (isUseSAPCode = 1) 
                    and QuoteNoRef is null and (isMassRevision=1) 
                    and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.isMassRevision = 1)) as 'submited' ";

                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                if (Result.Rows.Count > 0)
                {
                    string WaiittingForSubmited = Result.Rows[0]["waittingforsubmition"].ToString();
                    string submited = Result.Rows[0]["submited"].ToString();
                    if (WaiittingForSubmited != "")
                    {
                        LbWaitSMN.Text = "Waiting For SMN Submission : " + WaiittingForSubmited.ToString();
                    }
                    else
                    {
                        LbWaitSMN.Text = "Waiting For SMN Submission : 0";
                    }
                    if (submited != "")
                    {
                        LbConfirm.Text = "Waiting For Confirmation : " + submited.ToString();
                    }
                    else
                    {
                        LbConfirm.Text = "Waiting For Confirmation : 0";
                    }
                    //UpdatePanel11.Update();
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void Shimanoresponse()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails  
                              where vendorcode1='" + mappeduserid.ToString() + "' and  ApprovalStatus = 2 and (CreateStatus != '' OR CreateStatus IS NOT NULL) and Plant = '" + Session["VPlant"].ToString() + "'"; // and PICApprovalStatus IS NULL";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblpending_S.Text = strcount.ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void closed()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select count(Distinct RequestNumber) from TQuoteDetails  where vendorcode1='" + mappeduserid.ToString() + "' and  (ApprovalStatus = 3 or ApprovalStatus = 1) and Plant = '" + Session["VPlant"].ToString() + "'"; // and PICApprovalStatus IS NULL";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblclosed.Text = strcount.ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void approved()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails  Where vendorcode1='" + mappeduserid.ToString() + @"' and  (ApprovalStatus = 3) 
                                and Plant = '" + Session["VPlant"].ToString() + "'";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblapprove.Text = strcount.ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void Rejected()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails  Where vendorcode1='" + mappeduserid.ToString() + @"' and  (ApprovalStatus = 1) 
                                and Plant = '" + Session["VPlant"].ToString() + "' "; // and PICApprovalStatus IS NULL";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblrejected.Text = strcount.ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void WithoutSAPCode()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select
                                (select count(*) from TQuoteDetails 
                                where  isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4'
                                and PICApprovalStatus='4' and ManagerApprovalStatus='4' and DIRApprovalStatus='4' and vendorcode1='" + mappeduserid.ToString() + @"' and Plant = '" + Session["VPlant"].ToString() + @"') as 'waittingforsubmition',
                                (select count(*) from TQuoteDetails 
                                where isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 1)) = 'D' and (ApprovalStatus='5' or ApprovalStatus='6')
                                and vendorcode1='" + mappeduserid.ToString() + @"' and Plant = '" + Session["VPlant"].ToString() + @"') as 'submited' ";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string WaiittingForSubmited = Result.Rows[0]["waittingforsubmition"].ToString();
                    string submited = Result.Rows[0]["submited"].ToString();
                    if (WaiittingForSubmited != "")
                    {
                        LblBeforeSubmited.Text = "Waiting For Submission : " + WaiittingForSubmited.ToString();
                    }
                    else
                    {
                        LblBeforeSubmited.Text = "Waiting For Submission : 0";
                    }
                    if (submited != "")
                    {
                        LblAfterSubmited.Text = "Submitted : " + submited.ToString();
                    }
                    else
                    {
                        LblAfterSubmited.Text = "Submitted : 0";
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void WithoutSAPCodeGp()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select
                                (select count(*) from TQuoteDetails 
                                where  isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 2)) = 'GP' and ApprovalStatus='4'
                                and PICApprovalStatus='4' and ManagerApprovalStatus='4' and DIRApprovalStatus='4' and vendorcode1='" + mappeduserid.ToString() + @"' and Plant = '" + Session["VPlant"].ToString() + @"') as 'waittingforsubmition',
                                (select count(*) from TQuoteDetails 
                                where  isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 2)) = 'GP' and (ApprovalStatus='5' or ApprovalStatus='6')
                                and vendorcode1='" + mappeduserid.ToString() + @"' and Plant = '" + Session["VPlant"].ToString() + @"') as 'submited' ";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string WaiittingForSubmited = Result.Rows[0]["waittingforsubmition"].ToString();
                    string submited = Result.Rows[0]["submited"].ToString();
                    if (WaiittingForSubmited != "")
                    {
                        LblBeforeSubmitedGp.Text = "Waiting For Submission : " + WaiittingForSubmited.ToString();
                    }
                    else
                    {
                        LblBeforeSubmitedGp.Text = "Waiting For Submission : 0";
                    }
                    if (submited != "")
                    {
                        LblAfterSubmitedGp.Text = "Submitted : " + submited.ToString();
                    }
                    else
                    {
                        LblAfterSubmitedGp.Text = "Submitted : 0";
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void AllReq()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select count(distinct RequestNumber) from TQuoteDetails ";
                str += @" where ((Plant  = '" + Session["VPlant"].ToString() + @"')) and (CreateStatus <> '' or CreateStatus is not null) 
                          and vendorcode1 = '"+ Session["mappedVendor"].ToString() + @"' ";

                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    int count = 0;
                    if (strcount.ToString() != "")
                    {
                        count = Convert.ToInt32(strcount);
                    }

                    if (count > 999999999)
                    {
                        LblAllReq.Text = "999.999.999+";
                    }
                    else
                    {
                        LblAllReq.Text = strcount.ToString();
                    }
                    //  lblCount.Text = strcount.ToString();
                }
                else
                {
                    LblAllReq.Text = "0";
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void CtnCloseMdl_Click(object sender, EventArgs e)
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
                EMETModule.SendExcepToDB(ex);
            }
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
                EMETModule.SendExcepToDB(ex);
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
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
                EMETModule.SendExcepToDB(ex);
            }
        }
    }
}