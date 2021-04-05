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
	public partial class Home : System.Web.UI.Page
	{

        bool IsAth;

        SqlCommand cmd;
        SqlDataReader reader;

        string DbMasterName = "";

        protected void Page_Load(object sender, EventArgs e)
		{
            try
            {

                if (Session["userID"] == null || Session["UserName"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_Home";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Login.aspx");
                        }
                        else
                        {
                            Session["ReqWaitFilter"] = null;
                            Session["ReqWaitPgNo"] = null;
                            Session["ReqWaitNst"] = null;

                            Session["ApprovalFilter"] = null;
                            Session["ApprPgNo"] = null;
                            Session["ApprNst"] = null;

                            Session["DIRApprovalFilter"] = null;
                            Session["DIRApprPgNo"] = null;
                            Session["DIRApprNst"] = null;

                            Session["ClStPgNo"] = null;
                            Session["ClStNst"] = null;
                            Session["ClosedStatusFilter"] = null;

                            Session["ReqApprPgNo"] = null;
                            Session["ReqApprNst"] = null;
                            Session["ReqApprFilter"] = null;

                            Session["WthSAPCodePgNo"] = null;
                            Session["WthSAPCodeNst"] = null;
                            Session["WthSAPFilter"] = null;

                            Session["DtMassRev"] = null;
                            Session["TxtShowEntMassRev"] = null;
                            Session["MassRevPgNo"] = null;

                            Session["DtMassRevDIR"] = null;
                            Session["TxtShowEntMassRevDIR"] = null;
                            Session["MassRevPgNoDIR"] = null;

                            Session["ReqWaitFilterMassRev"] = null;

                            Session["AllReqFilter"] = null;
                            Session["SMNReportFilter"] = null;

                            string userId = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " " + srole;
                            int flag = 0;
                            lblUser.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            EMETModule.AutoUpdateRequest();
                            EMETModule.autoupdate(Session["EPlant"].ToString());
                            EMETModule.autoCloseWhitoutsapCode(Session["EPlant"].ToString());
                            EMETModule.CekAndCloseExpiredRequest(Session["EPlant"].ToString());

                            this.pirstatuscount();
                            this.vendorresponse();
                            this.Mrgstatus();
                            this.DIRstatus();
                            this.closed();
                            this.WithoutSAPCode();
                            this.WithoutSAPCodeGp();
                            this.AllReq();
                            this.RevisionReq();
                            this.MassMatRevision();
                            EMETModule.RealTimeVendInvLastCheck(Session["userID"].ToString());


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
            }
		}
        
        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
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
                // string str = "select count(Distinct RequestNumber) from TQuoteDetails  where ApprovalStatus IS NULL and (CreateStatus != '' OR CreateStatus IS NOT NULL)"; // and PICApprovalStatus IS NULL";
                //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null)"; // and PICApprovalStatus IS NULL";
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails 
                                where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                                and (CreateStatus <> '' or CreateStatus is not null) and (plant ='" + Session["EPlant"].ToString() + "') and (isUseSAPCode = 1) and QuoteNoRef is null and (isMassRevision=0 or isMassRevision is null) "; // and PICApprovalStatus IS NULL";
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }

        }

        protected void RevisionReq()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails 
                                where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                                and (CreateStatus <> '' or CreateStatus is not null) and (plant ='" + Session["EPlant"].ToString() + "') and (isUseSAPCode = 1) and QuoteNoRef is not null"; // and PICApprovalStatus IS NULL";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    LblRevison.Text = strcount.ToString();
                }
                else
                {
                    LblRevison.Text = "0";
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

        protected void MassMatRevision()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select 
                    (select count(*) from TQuoteDetails A
                    where 
                    RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                    and (CreateStatus <> '' or CreateStatus is not null)
                    and (plant ='" + Session["EPlant"].ToString() + @"') and (isUseSAPCode = 1) 
                    and QuoteNoRef is null and (isMassRevision=1) 
                    and (select top 1 QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.isMassRevision = 1)) as 'waittingforsubmition',

                    (select count(*) from TQuoteDetails A
                    where 
                    RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                    and (CreateStatus <> '' or CreateStatus is not null)
                    and (plant ='" + Session["EPlant"].ToString() + @"') and (isUseSAPCode = 1) 
                    and QuoteNoRef is null and (isMassRevision=1) 
                    and (select top 1 QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.isMassRevision = 1)) as 'submited' ";

                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                if (Result.Rows.Count > 0)
                {
                    string WaiittingForSubmited = Result.Rows[0]["waittingforsubmition"].ToString();
                    string submited = Result.Rows[0]["submited"].ToString();
                    if (WaiittingForSubmited != "")
                    {
                        LbMassMatRevWait.Text = "Waiting For Submission : " + WaiittingForSubmited.ToString();
                    }
                    else
                    {
                        LbMassMatRevWait.Text = "Waiting For Submission : 0";
                    }
                    if (submited != "")
                    {
                        LbMassMatRevSubmit.Text = "Submitted : " + submited.ToString();
                    }
                    else
                    {
                        LbMassMatRevSubmit.Text = "Submitted : 0";
                    }
                    //UpdatePanel11.Update();
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

        protected void pirstatuscount()
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result_h = new DataTable();
                SqlDataAdapter da_h = new SqlDataAdapter();

                //new count
                string str_h = @"select count(Distinct RequestNumber) 
                                from TQuoteDetails A
                                where RequestNumber in (select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 0) 
                                and ( isMassRevision = 0 or isMassRevision is null ) 
                                and plant ='" + Session["EPlant"].ToString() + "' ";
                str_h += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 )) ";
                //str_h += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";
                da_h = new SqlDataAdapter(str_h, EmetCon);
                Result_h = new DataTable();
                da_h.Fill(Result_h);

                if (Result_h.Rows.Count > 0)
                {
                    string strcount = Result_h.Rows[0]["column1"].ToString();
                    lblCount.Text = "New : " + strcount.ToString();
                }
                else
                {
                    lblCount.Text = "New : 0";
                }

                //mass revision count
                str_h = @" select count(Distinct RequestNumber) 
                                from TQuoteDetails A
                                where RequestNumber in (select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 0) 
                                and ( isMassRevision = 1) 
                                and plant ='" + Session["EPlant"].ToString() + "' ";
                str_h += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 )) ";
                //str_h += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";
                da_h = new SqlDataAdapter(str_h, EmetCon);
                Result_h = new DataTable();
                da_h.Fill(Result_h);

                if (Result_h.Rows.Count > 0)
                {
                    string strcount = Result_h.Rows[0]["column1"].ToString();
                    LbMassRevision.Text = "Mass Revision : " + strcount.ToString();
                }
                else
                {
                    LbMassRevision.Text = "Mass Revision : 0";
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
        protected void Mrgstatus()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                //new
                string str = @"select count(Distinct RequestNumber) from TQuoteDetails A
                            where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0) 
                            and ManagerApprovalStatus=0 
                            and ( isMassRevision = 0 or isMassRevision is null ) 
                            and plant ='" + Session["EPlant"].ToString() + "' ";
                str += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 )) ";
                //str += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    lblD.Text = "New : " + strcount.ToString();
                }
                else
                {
                    lblD.Text = "New : 0";
                }

                //mass
                str = @" select count(Distinct RequestNumber) from TQuoteDetails A
                            where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus = 0) 
                            and ManagerApprovalStatus = 0
                            and(isMassRevision = 1)
                            and plant = '" + Session["EPlant"].ToString() + "' ";
                str += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 )) ";
                //str += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
                    LbDMass.Text = "Mass Revision : " + strcount.ToString();
                }
                else
                {
                    LbDMass.Text = "Mass Revision : 0";
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
        protected void DIRstatus()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where ManagerApprovalStatus = 0) and DIRApprovalStatus = 0 and plant ='" + Session["EPlant"].ToString() + "'";
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();
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
                //string str = "select count(Distinct RequestNumber) from TQuoteDetails  where (DIRApprovalStatus IS NULL) and (ManagerApprovalStatus =1 or ManagerApprovalStatus =2) and (CreateStatus != '' OR CreateStatus IS NOT NULL)";
                //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 3)";
                // string str = "select count(distinct RequestNumber) from TQuoteDetails where ApprovalStatus = 3 or ApprovalStatus = 1 and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 2 or PICApprovalStatus = 0 or PICApprovalStatus is null )and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ManagerApprovalStatus = 2 or ManagerApprovalStatus = 0)and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where DIRApprovalStatus = 2 or DIRApprovalStatus = 0)";
                //string str = " select count(distinct RequestNumber) from TQuoteDetails ";
                //str += @" where ((ApprovalStatus = 3) or (ApprovalStatus = 1))
                //            and(RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (PICApprovalStatus = 2) or(PICApprovalStatus = 0) or(PICApprovalStatus is null)))
                //            and(RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (ManagerApprovalStatus = 2) or(ManagerApprovalStatus = 0)) )
                //            and(RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (DIRApprovalStatus = 2) or(DIRApprovalStatus = 0)) ) ";

                string str = " select count(distinct RequestNumber) from TQuoteDetails ";
                str += @" where RequestNumber not in(select RequestNumber from TQuoteDetails where ManagerApprovalStatus =0 ) and (DIRApprovalStatus=0 or DIRApprovalStatus=1) and plant ='" + Session["EPlant"].ToString() + "'";

                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                if (Result.Rows.Count > 0)
                {
                    string strcount = Result.Rows[0]["column1"].ToString();

                    lblclosed.Text = strcount.ToString();
                    //  lblCount.Text = strcount.ToString();
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
                                where isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4'
                                and PICApprovalStatus='4' and ManagerApprovalStatus='4' and DIRApprovalStatus='4' and Plant = '" + Session["EPlant"].ToString() + @"') as 'waittingforsubmition',
                                (select count(*) from TQuoteDetails 
                                where isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 1)) = 'D' and (ApprovalStatus='5')
                                and Plant = '" + Session["EPlant"].ToString() + @"') as 'submited' ";

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
                    //UpdatePanel11.Update();
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
                                where isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 2)) = 'GP' and ApprovalStatus='4' and Plant = '" + Session["EPlant"].ToString() + @"') as 'waittingforsubmition',
                                (select count(*) from TQuoteDetails 
                                where isnull(CreateStatus,'') <> '' and (SELECT RIGHT(QuoteNo, 2)) = 'GP' and (ApprovalStatus='5')
                                and Plant = '" + Session["EPlant"].ToString() + @"') as 'submited' ";

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
                    //UpdatePanel11.Update();
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
                string str = " select count(distinct RequestNumber) from TQuoteDetails ";
                str += @" where ((Plant  = '" + Session["EPlant"].ToString() + "')) and (CreateStatus <> '' or CreateStatus is not null) ";

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
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }



        
    }
}