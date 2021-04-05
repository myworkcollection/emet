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
using System.Drawing;

namespace Material_Evaluation
{
    public partial class ManagerStatus : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;

        //email
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string SendFilename;
        public static string userId1;
        public static string nameC;
        public static string aemail;
        public static string pemail;
        public static string Uemail;
        public static string body1;
        public static string quoteno;
        //email

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                userId = Session["userID"].ToString();
                userId1= Session["userID"].ToString();
                sname = Session["UserName"].ToString();
                srole = Session["userType"].ToString();
                concat = sname + " - " + srole;
                lblUser.Text = sname;
                nameC = sname.ToString();
                lblplant.Text = srole;
                //Session["UserName"] = userId;

                //      string strprod = txtplant.Text;

                GetGridData();

            }

        }

        private void LnkApp_Click(object sender, EventArgs e)
        {

        }

        public void GetGridData()
        {
            string userID = (string)HttpContext.Current.Session["UserName"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //string str = "select Plant,RequestNumber,ManagerApprovalStatus,CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,Product,Material,MaterialDesc,VendorCode1,VendorName,QuoteNo,TotalMaterialCost,TotalProcessCost,TotalSubMaterialCost,TotalOtheritemsCost,GrandTotalCost,Profit,Discount,FinalQuotePrice,ApprovalStatus,PICApprovalStatus,PICReason,'' as Reason from TQuoteDetails where RequestNumber  in(select distinct RequestNumber from TQuoteDetails  where ManagerApprovalStatus = 0 and ManagerApprovalStatus is not null) Order by RequestNumber desc";
            string str = "select Plant,RequestNumber,ManagerApprovalStatus,CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,Product,Material,MaterialDesc,VendorCode1,VendorName,QuoteNo,TotalMaterialCost,TotalProcessCost,TotalSubMaterialCost,TotalOtheritemsCost,GrandTotalCost,Profit,Discount,FinalQuotePrice,ApprovalStatus,PICApprovalStatus,PICReason,'' as Reason  from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0) and ManagerApprovalStatus=0 ";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            GridView1.DataSource = Result;
            GridView1.DataBind();
        }

        public void UpdateGridData(string ReqNum, string Vendor, int Status, string Reason)
        {

            string userID = (string)HttpContext.Current.Session["UserName"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();

            if (Status == 1)
            {

                if (Reason.ToString() != "")
                {
                    DataTable Result1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    string pic = " Rejected by Manager - " + nameC.ToString() + " - " + Reason.ToString();
                    string str1 = "Update TQuoteDetails SET ManagerApprovalStatus = '" + 1 + "', DIRApprovalStatus= '" + 0 + "',   ManagerReason = '" + pic + "',  UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "'and VendorCode1  in('" + Vendor + "')";

                    da1 = new SqlDataAdapter(str1, con1);
                    Result1 = new DataTable();
                    da1.Fill(Result1);


                    DataTable Result11 = new DataTable();
                    SqlDataAdapter da11 = new SqlDataAdapter();
                    string pic1 = "Approved by PIC - " + nameC.ToString() + " - " + Reason.ToString();
                    string str11 = "Update TQuoteDetails SET  DIRApprovalStatus= '" + 0 + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and DIRApprovalStatus is null";

                    da11 = new SqlDataAdapter(str11, con1);
                    Result11 = new DataTable();
                    da11.Fill(Result11);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Rejected Successfully');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Please fill the Reason for Reject');", true);
                }
            }

            if (Status == 2)
            {
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string str = "Update TQuoteDetails SET ManagerApprovalStatus = '" + 1 + "', DIRApprovalStatus =  '" + 0 + "', ManagerReason = '" + Reason + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 not in('" + Vendor + "')";

                //da = new SqlDataAdapter(str, con1);
                //Result = new DataTable();
                //da.Fill(Result);

                //string reason1 = "Auto Rejected By Manager";
                //str = "Update TQuoteDetails SET ManagerApprovalStatus = '" + 1 + "', DIRApprovalStatus =  '" + 0 + "', ManagerReason = '" + reason1 + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "'   and VendorCode1 not in('" + Vendor + "') and (ManagerApprovalStatus = '" + 0 + "' or ManagerApprovalStatus is null)";

                //da = new SqlDataAdapter(str, con1);
                //Result = new DataTable();
                //da.Fill(Result);

                string pic = "Approved by Manager - " + nameC.ToString() + " - " + Reason.ToString();
                str = "Update TQuoteDetails SET ManagerApprovalStatus = '" + Status + "', DIRApprovalStatus =  '" + 0 + "', ManagerReason = '" + pic + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                da = new SqlDataAdapter(str, con1);
                Result = new DataTable();
                da.Fill(Result);


                //email


                //getting Quote details
                var Vendormail = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Vendormail))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                    SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    id.Direction = ParameterDirection.Input;
                    id.Value = ReqNum.ToString();
                    cmdget.Parameters.Add(id);

                    SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                    Vid.Direction = ParameterDirection.Input;
                    Vid.Value = Vendor.ToString();
                    cmdget.Parameters.Add(Vid);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();
                    while (dr.Read())
                    {

                        quoteno = dr.GetString(0);

                    }
                    dr.Close();
                    cnn.Close();
                }



                //email
                // getting Messageheader ID from IT Mailapp
                var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Email))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.spGetControlNumber";

                    SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                    CompanyCode.Direction = ParameterDirection.Input;
                    CompanyCode.Value = 1;
                    cmdget.Parameters.Add(CompanyCode);

                    SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                    ControlField.Direction = ParameterDirection.Input;
                    ControlField.Value = "MessageHeaderID";
                    cmdget.Parameters.Add(ControlField);

                    SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                    Param1.Direction = ParameterDirection.Input;
                    Param1.Value = "";
                    cmdget.Parameters.Add(Param1);

                    SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                    Param2.Direction = ParameterDirection.Input;
                    Param2.Value = "";
                    cmdget.Parameters.Add(Param2);

                    SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                    Param3.Direction = ParameterDirection.Input;
                    Param3.Value = "";
                    cmdget.Parameters.Add(Param3);

                    SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                    Param4.Direction = ParameterDirection.Input;
                    Param4.Value = "";
                    cmdget.Parameters.Add(Param4);

                    SqlParameter pOutput = cmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                    pOutput.Direction = ParameterDirection.Output;

                    cmdget.ExecuteNonQuery();
                    returnValue = pOutput.Value.ToString();
                    cnn.Close();
                    OriginalFilename = returnValue;
                    MHid = returnValue;
                    OriginalFilename = MHid + seqNo + formatW;
                }

                Boolean IsAttachFile = true;
                int SequenceNumber = 1;
                string test = userId1;
                IsAttachFile = false;
                SendFilename = "NOFILE";
                OriginalFilename = "NOFILE";
                format = "NO";

                //getting vendor mail id
                var Vendormail1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Vendormail1))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    aemail = "";
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Email_dirdetails";

                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = quoteno.ToString();
                    cmdget.Parameters.Add(vendorid);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();

                    while (dr.Read())
                    {
                        aemail = string.Concat(aemail, dr.GetString(0), ";");
                        //pemail = dr.GetString(1);

                    }
                    dr.Close();
                    cnn.Close();
                }

                //getting User mail id
                var usermail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(usermail))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Email_userdetails";

                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = userId1;
                    cmdget.Parameters.Add(vendorid);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();
                    while (dr.Read())
                    {
                        Uemail = dr.GetString(0);
                        Session["dept"] = dr.GetString(1);
                    }
                    dr.Close();
                    cnn.Close();
                }

                //getting vendor mail content
                var mailcontent = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(mailcontent))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Email_content";

                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = quoteno.ToString();
                    cmdget.Parameters.Add(vendorid);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();
                    while (dr.Read())
                    {
                        body1 = dr.GetString(1);
                    }
                    dr.Close();
                    cnn.Close();
                }
                // Insert header and details to Mil server table to IT mailserverapp
                var Email_insert = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                using (SqlConnection Email_inser = new SqlConnection(Email_insert))
                {
                    Email_inser.Open();
                    //Header
                    string MessageHeaderId = MHid;
                    string fromname = "eMET System";
                    string FromAddress = Uemail;
                    //string Recipient = aemail + "," + pemail;
                    string Recipient = aemail;
                    string CopyRecipient = Uemail;
                    string BlindCopyRecipient = "";
                    string ReplyTo = "subashdurai@shimano.com.sg";
                    string Subject = "Request Number" + ReqNum .ToString()+ " |Quotation Number: " + quoteno.ToString() + " - Manager Approval By : " + nameC;
                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation request has been Approved By Manager.<br /><br />" + body1.ToString();
                    string BodyFormat = "HTML";
                    string BodyRemark = "0";
                    string Signature = " ";
                    string Importance = "High";
                    string Sensitivity = "Confidential";

                    string CreateUser = userId1;
                    DateTime CreateDate = DateTime.Now;
                    //end Header
                    string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                    SqlCommand Header = new SqlCommand(Head, Email_inser);
                    Header.Parameters.AddWithValue("@MessageHeaderId", MessageHeaderId.ToString());
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
                    Header.Parameters.AddWithValue("@CreateUser", userId1.ToString());
                    Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    Header.CommandText = Head;
                    Header.ExecuteNonQuery();
                    //end Header
                    //Details

                    string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                    SqlCommand Detail = new SqlCommand(Details, Email_inser);
                    Detail.Parameters.AddWithValue("@MessageHeaderId", MHid.ToString());
                    Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                    Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                    Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                    Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                    Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                    Detail.Parameters.AddWithValue("@CreateUser", userId1.ToString());
                    Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                    Detail.CommandText = Details;
                    Detail.ExecuteNonQuery();
                    Email_inser.Close();
                    //End Details
                }


                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Approved Successfully and Request Moved to next Level');", true);

                //End by subash

                //end of email



            }
        }

        protected void LinkButton_Click(Object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                Response.Redirect("NewReq_changes.aspx?Number=" + e.CommandArgument.ToString());
            }
        }

     

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (hdnreason.Value == "")
            {
                if (e.CommandName == "Approve")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GridView1.Rows[rowIndex];

                    string Reason = (row.FindControl("txtReason") as TextBox).Text;
                    string ReqNumber = row.Cells[1].Text;
                    string Vendor = row.Cells[7].Text;
                    string Quote = row.Cells[9].Text;

                    UpdateGridData(ReqNumber, Vendor, 2, Reason);
                    GetGridData();
                }
                else if (e.CommandName == "Reject")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string Reason = (row.FindControl("txtReason") as TextBox).Text;
                    string ReqNumber = row.Cells[1].Text;
                    string Vendor = row.Cells[7].Text;
                    string Quote = row.Cells[9].Text;

                    UpdateGridData(ReqNumber, Vendor, 1, Reason);
                    GetGridData();
                }
                else if (e.CommandName == "LinktoRedirect")
                {
                    //comment by subash
                    //Response.Redirect("NewReq_changes.aspx?Number=" + e.CommandArgument.ToString());
                   
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());

                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Plz fill  the textbox')", true);
                hdnreason.Value = "0";
            }

            GridView1.HeaderRow.Cells[21].ColumnSpan = 2;
            //GridView1.HeaderRow.Cells.RemoveAt(18);
           
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if(e.Row.RowType == DataControlRowType.Header)
            //{
            //    int hearderindex = GridView1.HeaderRow.RowIndex;
            //    string headerval = GridView1.RowHeaderColumn[17].ToString();
            //}

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType== DataControlRowType.Footer)
            {

                int RowSpan = 2;
                for (int i = GridView1.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GridView1.Rows[i];
                    GridViewRow prevRow = GridView1.Rows[i + 1];
                    if (currRow.Cells[1].Text == prevRow.Cells[1].Text)
                    {
                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[3].RowSpan = RowSpan;
                        prevRow.Cells[3].Visible = false;

                        currRow.Cells[4].RowSpan = RowSpan;
                        prevRow.Cells[4].Visible = false;

                        currRow.Cells[5].RowSpan = RowSpan;
                        prevRow.Cells[5].Visible = false;

                        currRow.Cells[6].RowSpan = RowSpan;
                        prevRow.Cells[6].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        RowSpan = 2;
                    }
                }
                //Vendor 
                if (e.Row.Cells[18].Text == "2")
                {
                    e.Row.Cells[18].Text = "Vendor Completed";
                }
                else if (e.Row.Cells[18].Text == "0")
                {
                    e.Row.Cells[18].Text = "Vendor Pending";
                    e.Row.Cells[22].Enabled = false;
                    e.Row.Cells[23].Enabled = false;
                }
                else if (e.Row.Cells[18].Text == "3")
                {
                    e.Row.Cells[18].Text = "Approved/Closed";
                    e.Row.Cells[23].Enabled = false;
                    e.Row.Cells[22].Enabled = false;
                }
                if ((e.Row.Cells[17].Text != "&nbsp;") && (e.Row.Cells[18].Text == "1"))
                {
                    e.Row.Cells[18].Text = "Vendor Completed";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                   
                }
                if ((e.Row.Cells[17].Text == "&nbsp;") && (e.Row.Cells[18].Text == "1"))
                {
                    e.Row.Cells[18].Text = "Vendor Pending";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                    e.Row.Cells[23].Enabled = false;
                    e.Row.Cells[22].Enabled = false;
                }
                //Vendor end


                //PIC
                if (e.Row.Cells[19].Text == "2")
                {
                    e.Row.Cells[19].Text = "PIC Approved";
                }
                else if (e.Row.Cells[19].Text == "1")
                {
                    e.Row.Cells[19].Text = "PIC Rejected";
                }

                else if (e.Row.Cells[19].Text == "0")
                {
                    e.Row.Cells[19].Text = "PIC Pending";
                }

                //PIC end

                //manager
                if (e.Row.Cells[21].Text == "0")
                {
                    e.Row.Cells[21].Text = "Manager Pending";
                }
                else if (e.Row.Cells[21].Text == "1")
                {
                    e.Row.Cells[21].Text = "Manager Rejected";
                    e.Row.Cells[23].Enabled = false;
                }
                else if (e.Row.Cells[18].Text == "2")
                {
                    e.Row.Cells[21].Text = "Manager Approved";
                    e.Row.Cells[23].Enabled = true;
                    e.Row.Cells[22].Enabled = false;
                }

                //manager end
            }
            




            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[22].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(23);
            }

        }
    }
}