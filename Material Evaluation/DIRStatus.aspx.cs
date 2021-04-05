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
    public partial class DIRStatus : System.Web.UI.Page
    {
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
        public static string demail;
        public static string pemail1;
        public static string vemail;
        public static string pemail;
        public static string Uemail;
        public static string body1;
        public static string quoteno;
        public static string customermail;
        public static string customermail1;
        public static string cc;
        //email

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                string userId = Session["userID"].ToString();
                userId1 = userId.ToString();
                string sname = Session["UserName"].ToString();
                string srole = Session["userType"].ToString();
                string concat = sname + " - " + srole;
                lblUser.Text = sname;
                lblplant.Text = srole;
                nameC = sname.ToString();
                // Session["UserName"] = userId;
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
            //string str = "select Plant,RequestNumber,CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,Product,Material,MaterialDesc,VendorCode1,VendorName,QuoteNo,TotalMaterialCost,TotalProcessCost,TotalSubMaterialCost,TotalOtheritemsCost,GrandTotalCost,Profit,Discount,FinalQuotePrice,ApprovalStatus,PICApprovalStatus,PICReason,ManagerApprovalStatus,ManagerReason,'' as Reason from TQuoteDetails where RequestNumber  in(select distinct RequestNumber from TQuoteDetails where DIRApprovalStatus = 0 and DIRApprovalStatus is not null ) Order by RequestNumber desc";
            string str = "select Plant, RequestNumber, CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,Product,Material,MaterialDesc,VendorCode1,VendorName,QuoteNo,TotalMaterialCost,TotalProcessCost,TotalSubMaterialCost,TotalOtheritemsCost,GrandTotalCost,Profit,Discount,FinalQuotePrice,ApprovalStatus,PICApprovalStatus,PICReason,ManagerApprovalStatus,ManagerReason,'' as Reason from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where ManagerApprovalStatus = 0) and DIRApprovalStatus = 0";
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

                string pic = " Rejected by Director - " + nameC.ToString() + " - " + Reason.ToString();
                string str1 = "Update TQuoteDetails SET DIRApprovalStatus = '" + 1 + "', ApprovalStatus='" + 1 + "',  UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                da1 = new SqlDataAdapter(str1, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);



                    //Rejected


                    //main mail
                    //getting Customer mail id
                    aemail = string.Empty;
                    pemail = string.Empty;
                    var Customer1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                    using (SqlConnection cnn = new SqlConnection(Customer1))
                    {
                        string returnValue = string.Empty;
                        cnn.Open();
                        SqlCommand cmdget = cnn.CreateCommand();
                        cmdget.CommandType = CommandType.StoredProcedure;
                        cmdget.CommandText = "dbo.Emet_Customer_mail_R";

                        SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                        vendorid.Direction = ParameterDirection.Input;
                        vendorid.Value = ReqNum.ToString();
                        cmdget.Parameters.Add(vendorid);

                        int status1 = 1;
                        SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                        status.Direction = ParameterDirection.Input;
                        status.Value = Convert.ToInt32(status1.ToString());
                        cmdget.Parameters.Add(status);


                        SqlDataReader dr_cus;
                        dr_cus = cmdget.ExecuteReader();
                        pemail1 = string.Empty;
                        while (dr_cus.Read())
                        {

                            customermail = dr_cus.GetString(0);
                            customermail1 = dr_cus.GetString(1);
                            quoteno = dr_cus.GetString(2);
                            customermail = string.Concat(customermail, ";", customermail1);

                            //while start

                            //email
                            // getting Messageheader ID from IT Mailapp
                            var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                            using (SqlConnection MHcnn = new SqlConnection(Email))
                            {
                                string returnValue1 = string.Empty;
                                MHcnn.Open();
                                SqlCommand MHcmdget = MHcnn.CreateCommand();
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
                                returnValue1 = pOutput.Value.ToString();
                                MHcnn.Close();
                                OriginalFilename = returnValue1;
                                MHid = returnValue1;
                                OriginalFilename = MHid + seqNo + formatW;
                            }

                            Boolean IsAttachFile = true;
                            int SequenceNumber = 1;
                            string test = userId1;
                            IsAttachFile = false;
                            SendFilename = "NOFILE";
                            OriginalFilename = "NOFILE";
                            format = "NO";




                            //getting vendor mail content
                            var mailcontent = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                            using (SqlConnection cnn_ = new SqlConnection(mailcontent))
                            {
                                string returnValue_ = string.Empty;
                                cnn_.Open();
                                SqlCommand cmdget_ = cnn_.CreateCommand();
                                cmdget_.CommandType = CommandType.StoredProcedure;
                                cmdget_.CommandText = "dbo.Emet_Email_content";

                                SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                vendorid_.Direction = ParameterDirection.Input;
                                vendorid_.Value = quoteno.ToString();
                                cmdget_.Parameters.Add(vendorid_);

                                SqlDataReader dr_;
                                dr_ = cmdget_.ExecuteReader();
                                while (dr_.Read())
                                {
                                    body1 = dr_.GetString(1);
                                }
                                dr_.Close();
                                cnn_.Close();
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
                                string Recipient = customermail;
                                string CopyRecipient = cc;
                                string BlindCopyRecipient = "";
                                string ReplyTo = "subashdurai@shimano.com.sg";
                                string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + quoteno.ToString() + " - Shimano Approval Rejection By : " + nameC + " - Plant : " + Session["EPlant"].ToString();
                                //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected By Shimano.<br /><br />" + body1.ToString();
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

                            //while end

                        }
                        dr_cus.Close();
                        cnn.Close();
                    }

                    //end Rejected

                    

                    //End by subash
                    //end of email


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

                string str = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', DIRApprovalStatus = '" + 1 + "', DIRReason = '" + Reason + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 not in('" + Vendor + "')";

                //da = new SqlDataAdapter(str, con1);
                //Result = new DataTable();
                //da.Fill(Result);

                //string reason1 = "Auto Rejected By Director";
                //str = "Update TQuoteDetails SET  ApprovalStatus='" + 1 + "', DIRApprovalStatus = '" + 1 + "', DIRReason = '" + reason1 + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 not in('" + Vendor + "') and DIRApprovalStatus = '" + 0 + "'";

                //da = new SqlDataAdapter(str, con1);
                //Result = new DataTable();
                //da.Fill(Result);

                str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',DIRApprovalStatus = '" + Status + "', DIRReason = '" + Reason + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                da = new SqlDataAdapter(str, con1);
                Result = new DataTable();
                da.Fill(Result);

                //Email

                //getting PIC mail id
                aemail = string.Empty;
                pemail = string.Empty;
                var picmail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(picmail))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_PIC_Details";

                    SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = ReqNum.ToString();
                    cmdget.Parameters.Add(vendorid);

                    SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                    plant.Direction = ParameterDirection.Input;
                    plant.Value = Session["EPlant"].ToString();
                    cmdget.Parameters.Add(plant);

                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();
                    pemail1 = string.Empty;
                    while (dr.Read())
                    {

                        pemail1 = string.Concat(pemail1, dr.GetString(0), ";");

                    }
                    dr.Close();
                    cnn.Close();
                }

                //getting manager mail id
                var Vendormail1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Vendormail1))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    aemail = string.Empty;
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Email_managerdetails";

                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = "1";
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

                //getting Director mail id
                var Diremail = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Diremail))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    demail = string.Empty;
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Dir_approval";
                    SqlDataReader dr;
                    dr = cmdget.ExecuteReader();

                    while (dr.Read())
                    {
                        demail = string.Concat(demail, dr.GetString(0), ";");
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

                //getting Quote details
                var Vendormail = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                using (SqlConnection Qcnn = new SqlConnection(Vendormail))
                {
                    string returnValue2 = string.Empty;
                    Qcnn.Open();
                    SqlCommand qcmdget = Qcnn.CreateCommand();
                    qcmdget.CommandType = CommandType.StoredProcedure;
                    qcmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                    SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    id.Direction = ParameterDirection.Input;
                    id.Value = ReqNum.ToString();
                    qcmdget.Parameters.Add(id);

                    SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                    Vid.Direction = ParameterDirection.Input;
                    Vid.Value = Vendor.ToString();
                    qcmdget.Parameters.Add(Vid);

                    SqlDataReader qdr;
                    qdr = qcmdget.ExecuteReader();
                    while (qdr.Read())
                    {

                        quoteno = qdr.GetString(0);

                    }
                    qdr.Close();
                    Qcnn.Close();
                }


                cc = string.Concat(pemail1, aemail, demail, Uemail);


                //main mail
                //getting Customer mail id
                aemail = string.Empty;
                pemail = string.Empty;
                var Customer = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Customer))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Customer_mail";

                    SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar,50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = ReqNum.ToString();
                    cmdget.Parameters.Add(vendorid);

                    int status1 = 2;
                    SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                    status.Direction = ParameterDirection.Input;
                    status.Value = Convert.ToInt32(status1.ToString());
                    cmdget.Parameters.Add(status);

                  
                    SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                    Quote.Direction = ParameterDirection.Input;
                    Quote.Value = quoteno.ToString();
                    cmdget.Parameters.Add(Quote);

                    SqlDataReader dr_cus;
                    dr_cus = cmdget.ExecuteReader();
                    pemail1 = string.Empty;
                    while (dr_cus.Read())
                    {

                       customermail= dr_cus.GetString(0);
                       customermail1 = dr_cus.GetString(1);
                       customermail = string.Concat(customermail, ";", customermail1);

                        //while start

                        //email
                        // getting Messageheader ID from IT Mailapp
                        var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                        using (SqlConnection MHcnn = new SqlConnection(Email))
                        {
                            string returnValue1 = string.Empty;
                            MHcnn.Open();
                            SqlCommand MHcmdget = MHcnn.CreateCommand();
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
                            returnValue1 = pOutput.Value.ToString();
                            MHcnn.Close();
                            OriginalFilename = returnValue1;
                            MHid = returnValue1;
                            OriginalFilename = MHid + seqNo + formatW;
                        }

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        string test = userId1;
                        IsAttachFile = false;
                        SendFilename = "NOFILE";
                        OriginalFilename = "NOFILE";
                        format = "NO";




                        //getting vendor mail content
                        var mailcontent = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        using (SqlConnection cnn_ = new SqlConnection(mailcontent))
                        {
                            string returnValue_ = string.Empty;
                            cnn_.Open();
                            SqlCommand cmdget_ = cnn_.CreateCommand();
                            cmdget_.CommandType = CommandType.StoredProcedure;
                            cmdget_.CommandText = "dbo.Emet_Email_content";

                            SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            vendorid_.Direction = ParameterDirection.Input;
                            vendorid_.Value = quoteno.ToString();
                            cmdget_.Parameters.Add(vendorid_);

                            SqlDataReader dr_;
                            dr_ = cmdget_.ExecuteReader();
                            while (dr_.Read())
                            {
                                body1 = dr_.GetString(1);
                            }
                            dr_.Close();
                            cnn_.Close();
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
                            string Recipient = customermail;
                            string CopyRecipient = cc;
                            string BlindCopyRecipient = "";
                            string ReplyTo = "subashdurai@shimano.com.sg";
                            string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + quoteno.ToString() + " - Shimano Approval By : " + nameC + " - Plant : " + Session["EPlant"].ToString();
                            //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                            //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                            string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + body1.ToString();
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

                        //while end

                    }
                    dr_cus.Close();
                    cnn.Close();
                }



                //Rejected


                //main mail
                //getting Customer mail id
                aemail = string.Empty;
                pemail = string.Empty;
                var Customer1 = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
                using (SqlConnection cnn = new SqlConnection(Customer1))
                {
                    string returnValue = string.Empty;
                    cnn.Open();
                    SqlCommand cmdget = cnn.CreateCommand();
                    cmdget.CommandType = CommandType.StoredProcedure;
                    cmdget.CommandText = "dbo.Emet_Customer_mail_R";

                    SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                    vendorid.Direction = ParameterDirection.Input;
                    vendorid.Value = ReqNum.ToString();
                    cmdget.Parameters.Add(vendorid);

                    int status1 = 1;
                    SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                    status.Direction = ParameterDirection.Input;
                    status.Value = Convert.ToInt32(status1.ToString());
                    cmdget.Parameters.Add(status);


                    SqlDataReader dr_cus;
                    dr_cus = cmdget.ExecuteReader();
                    pemail1 = string.Empty;
                    while (dr_cus.Read())
                    {

                        customermail = dr_cus.GetString(0);
                        customermail1 = dr_cus.GetString(1);
                        quoteno= dr_cus.GetString(2);
                        customermail = string.Concat(customermail, ";", customermail1);

                        //while start

                        //email
                        // getting Messageheader ID from IT Mailapp
                        var Email = ConfigurationManager.ConnectionStrings["DbconnectionEmail"].ConnectionString;
                        using (SqlConnection MHcnn = new SqlConnection(Email))
                        {
                            string returnValue1 = string.Empty;
                            MHcnn.Open();
                            SqlCommand MHcmdget = MHcnn.CreateCommand();
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
                            returnValue1 = pOutput.Value.ToString();
                            MHcnn.Close();
                            OriginalFilename = returnValue1;
                            MHid = returnValue1;
                            OriginalFilename = MHid + seqNo + formatW;
                        }

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        string test = userId1;
                        IsAttachFile = false;
                        SendFilename = "NOFILE";
                        OriginalFilename = "NOFILE";
                        format = "NO";




                        //getting vendor mail content
                        var mailcontent = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                        using (SqlConnection cnn_ = new SqlConnection(mailcontent))
                        {
                            string returnValue_ = string.Empty;
                            cnn_.Open();
                            SqlCommand cmdget_ = cnn_.CreateCommand();
                            cmdget_.CommandType = CommandType.StoredProcedure;
                            cmdget_.CommandText = "dbo.Emet_Email_content";

                            SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            vendorid_.Direction = ParameterDirection.Input;
                            vendorid_.Value = quoteno.ToString();
                            cmdget_.Parameters.Add(vendorid_);

                            SqlDataReader dr_;
                            dr_ = cmdget_.ExecuteReader();
                            while (dr_.Read())
                            {
                                body1 = dr_.GetString(1);
                            }
                            dr_.Close();
                            cnn_.Close();
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
                            string Recipient = customermail;
                            string CopyRecipient = cc;
                            string BlindCopyRecipient = "";
                            string ReplyTo = "subashdurai@shimano.com.sg";
                            string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + quoteno.ToString() + " - Shimano Approval Rejection By : " + nameC + " - Plant : " + Session["EPlant"].ToString();
                            //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                            //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                            string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected By Shimano.<br /><br />" + body1.ToString();
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

                        //while end

                    }
                    dr_cus.Close();
                    cnn.Close();
                }

                //end Rejected

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
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Plz fill  the textbox')", true);
                hdnreason.Value = "0";
            }

            GridView1.HeaderRow.Cells[23].ColumnSpan = 2;
            //GridView1.HeaderRow.Cells.RemoveAt(18);

        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if(e.Row.RowType == DataControlRowType.Header)
            //{
            //    int hearderindex = GridView1.HeaderRow.RowIndex;
            //    string headerval = GridView1.RowHeaderColumn[17].ToString();
            //}

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
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

           

                //pic 
                if (e.Row.Cells[18].Text == "2")
                {
                    e.Row.Cells[18].Text = "Vendor Completed";
                }
                else if (e.Row.Cells[18].Text == "0")
                {
                    e.Row.Cells[18].Text = "Vendor Pending";
                    e.Row.Cells[23].Enabled = false;
                    e.Row.Cells[24].Enabled = false;
                }
                else if (e.Row.Cells[18].Text == "3")
                {
                    e.Row.Cells[18].Text = "Approved/Closed";
                }
                if ((e.Row.Cells[17].Text != "&nbsp;") && (e.Row.Cells[18].Text == "1"))
                {
                    e.Row.Cells[18].Text = "Vendor Completed";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                    e.Row.Cells[20].Enabled = false;
                    e.Row.Cells[21].Enabled = false;
                }
                if ((e.Row.Cells[17].Text == "&nbsp;") && (e.Row.Cells[18].Text == "1"))
                {
                    e.Row.Cells[18].Text = "Vendor Pending";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                    e.Row.Cells[23].Enabled = false;
                    e.Row.Cells[24].Enabled = false;
                }

                //pic end

                if (e.Row.Cells[19].Text == "2")
                {
                    e.Row.Cells[19].Text = "Approved";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                }
                else
                {
                    e.Row.Cells[19].Text = "Rejected";

                }

                if (e.Row.Cells[21].Text == "2")
                {
                    e.Row.Cells[21].Text = "Approved";
                    //e.Row.Cells[20].Attributes.Add("disabled", "disabled");
                }
                else
                {
                    e.Row.Cells[21].Text = "Rejected";
                    //e.Row.Cells[23].Enabled = false;
                    //e.Row.Cells[24].Enabled = false;

                }
            }


            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[23].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(24);
            }

        }
    }
}