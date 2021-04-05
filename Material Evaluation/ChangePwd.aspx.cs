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
    public partial class ChangePwd : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool OldPwd = false;
        bool IsAth;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID_"] == null)
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
                        string FN = "EMET_VendorChangePwd";
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

                        UI = Session["userID_"].ToString();
                        FN = "EMET_VendorAnnouncement";
                        PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            LiUnReadAnn.Style.Add("display", "none");
                            lbUnreadAnn.Text = "";
                        }
                        else
                        {
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
            }
            catch (ThreadAbortException ex2)
            {
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void CheckOldPasswd()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @" select * from usr where UseID = @UseID and PWDCOMPARE(@UsePass,UsePass)=1 ";
                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                cmd.Parameters.AddWithValue("@UsePass", txtOldPwd.Text);
                cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    OldPwd = false;
                }
                else
                {
                    OldPwd = true;
                }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CheckOldPasswd();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            if (OldPwd == true)
            {
                try
                {
                    if (txtNewPwd.Text.Contains(Session["userID_"].ToString()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Password Cannot contain user id !!');", true);
                    }
                    else
                    {
                        MDMCon.Open();
                        sql = @" update usr set UsePass = PWDENCRYPT(@UsePass) where UseID = @UseID ";
                        sql += @" insert into TlogPwdchange(UseID,VendorCode,UpdateBy,UpdatedOn,UpdateAt)values(@UseID,@VendorCode,@UseID,CURRENT_TIMESTAMP,@UpdateAt) ";
                        cmd = new SqlCommand(sql, MDMCon);
                        cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                        cmd.Parameters.AddWithValue("@UsePass", txtNewPwd.Text);
                        cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                        cmd.Parameters.AddWithValue("@UpdateAt", Environment.MachineName.ToString());
                        cmd.ExecuteNonQuery();
                        string userid = Session["userID_"].ToString();
                        passwordReset__(userid, txtNewPwd.Text);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Userid And Password Sent Successfully to Your Mail');", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Password Updated'); window.location='ChangePwd.aspx';", true);
                    }
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Incorrect Old Password!');", true);
            }
        }


        public void passwordReset__(string username, string Password)
        {

            string userID = (string)HttpContext.Current.Session["UserName"].ToString();
            Boolean sendingemail;
            //string MsgErr;
            string Uemail_;

            try
            {

                //getting User mail id
                using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Email_userdetails";

                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = Session["userID_"].ToString();
                    cmdget.Parameters.Add(vendorid);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();
                    while (dr.Read())
                    {
                        Uemail_ = dr.GetString(0);
                        Session["Uemail"] = dr.GetString(0);
                        Session["dept"] = dr.GetString(1);
                    }
                    dr.Dispose();
                    cnn.Dispose();
                }


                //email
                // getting Messageheader ID from IT Mailapp
                
                using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                {
                    SqlTransaction transaction;
                    string returnValue1 = string.Empty;
                    MHcnn.Open();
                    transaction = MHcnn.BeginTransaction("Approval");
                    try
                    {
                        SqlCommand MHcmdget = MHcnn.CreateCommand();
                        MHcmdget.Transaction = transaction;
                        MHcmdget.CommandType = CommandType.StoredProcedure;
                        MHcmdget.CommandText = "dbo.spGetControlNumber";
                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                        CompanyCode.Direction = ParameterDirection.Input;
                        CompanyCode.Value = 1;
                        MHcmdget.Parameters.Add(CompanyCode);

                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                        ControlField.Direction = ParameterDirection.Input;
                        ControlField.Value = "MessageHeaderID";
                        MHcmdget.Parameters.Add(ControlField);

                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                        Param1.Direction = ParameterDirection.Input;
                        Param1.Value = "";
                        MHcmdget.Parameters.Add(Param1);

                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                        Param2.Direction = ParameterDirection.Input;
                        Param2.Value = "";
                        MHcmdget.Parameters.Add(Param2);

                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                        Param3.Direction = ParameterDirection.Input;
                        Param3.Value = "";
                        MHcmdget.Parameters.Add(Param3);

                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                        Param4.Direction = ParameterDirection.Input;
                        Param4.Value = "";
                        MHcmdget.Parameters.Add(Param4);

                        SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                        pOutput.Direction = ParameterDirection.Output;
                        MHcmdget.ExecuteNonQuery();
                        transaction.Commit();
                        returnValue1 = pOutput.Value.ToString();
                        Session["MHid"] = returnValue1;

                    }
                    catch (Exception xw)
                    {
                        transaction.Rollback();
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                        EMETModule.SendExcepToDB(xw);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                    MHcnn.Dispose();
                }
                Boolean IsAttachFile = false;
                int SequenceNumber = 1;

                // Insert header and details to Mil server table to IT mailserverapp
                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                {
                    Email_inser.Open();
                    //Header
                    string MessageHeaderId = Session["MHid"].ToString();
                    string fromname = "eMET System";
                    //string FromAddress = Session["Uemail"].ToString(); 
                    string FromAddress = "eMET@Shimano.Com.sg";
                    //string Recipient = aemail + "," + pemail;
                    string Recipient = Session["Uemail"].ToString();
                    string CopyRecipient = "";
                    string BlindCopyRecipient = "";
                    string ReplyTo = "subashdurai@shimano.com.sg";
                    string Subject = "Change of Password for Vendor from eMET System";
                    string footer = "<br />Please Login SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                    //Rejection Reason: " + Session["managercomment"].ToString() + " <br />
                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that your password has been changed for the eMET System: <br /><br /> Plant : " + Session["VPlant"].ToString() + " <br /> User ID : " + username.ToString() + "<br />  Password : " + Password.ToString() + " <br />" + footer;
                    string BodyFormat = "HTML";
                    string BodyRemark = "0";
                    string Signature = " ";
                    string Importance = "High";
                    string Sensitivity = "Confidential";
                    //string CreateUser = userId1;
                    DateTime CreateDate = DateTime.Now;
                    //end Header

                    SqlTransaction transactionHe;
                    transactionHe = Email_inser.BeginTransaction("Header");
                    try
                    {
                        string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                        SqlCommand Header = new SqlCommand(Head, Email_inser);
                        Header.Transaction = transactionHe;
                        Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                        Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                        Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                        Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                        Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                        Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                        Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                        Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                        Header.Parameters.AddWithValue("@body", body.ToString());
                        Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                        Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                        Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                        Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                        Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                        Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                        Header.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        Header.CommandText = Head;
                        Header.ExecuteNonQuery();
                        transactionHe.Commit();
                    }
                    catch (Exception xw)
                    {
                        transactionHe.Rollback();
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                        EMETModule.SendExcepToDB(xw);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                    //end Header
                    //Details
                    SqlTransaction transactionDe;
                    transactionDe = Email_inser.BeginTransaction("Detail");
                    try
                    {
                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                        Detail.Transaction = transactionDe;
                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                        Detail.Parameters.AddWithValue("@OriginalFilename", "");
                        Detail.Parameters.AddWithValue("@OriginalFileExtension", "");
                        Detail.Parameters.AddWithValue("@SendFilename", "");
                        Detail.Parameters.AddWithValue("@sendfileextension", "");
                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                        Detail.CommandText = Details;
                        Detail.ExecuteNonQuery();
                        transactionDe.Commit();
                    }
                    catch (Exception xw)
                    {
                        transactionDe.Rollback();
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                        EMETModule.SendExcepToDB(xw);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                    Email_inser.Dispose();
                    //End Details
                }

                sendingemail = true;
            }
            catch (Exception ex)
            {
                sendingemail = false;
                Session[" MsgErr"] = ex.ToString();
                EMETModule.SendExcepToDB(ex);
            }

            if (sendingemail == false)
            {
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + Session[" MsgErr"].ToString() + " ");
                var script = string.Format("alert({0});", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                //ShowTable();
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('failed sending email');", true);
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('failed sending email'); ", true);
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Userid And Password Sent Successfully to Your Mail');", true);
                //ShowTable();
                //Response.Redirect("Approval.aspx");
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
                EMETModule.SendExcepToDB(ex2);
            }
        }
        
    }
}