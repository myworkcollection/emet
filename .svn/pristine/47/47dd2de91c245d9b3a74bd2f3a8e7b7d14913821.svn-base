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
using System.Globalization;

namespace Material_Evaluation
{
    public partial class DateUpdate : System.Web.UI.Page
    {
        
        int incNumber = 000000;


        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string Userid;
        public static string userId;
        public static string userId1;
        public static string password;
        public static string domain;
        public static string path;
        public static string SendFilename;
        public static Dictionary<int, string> objPirType;
        public static string nameC;
        public static string aemail;
        public static string pemail;
        public static string Uemail;
        public static string body1;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"].ToString() == "" || Session["userID"].ToString() == null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {
                string qq = Request.QueryString["Number"];

                if (!IsPostBack)
                {
                    //txtReqDate.Text = DateTime.Now.Date.ToShortDateString().ToString();
                    userId = Session["userID"].ToString();
                    userId1 = Session["userID"].ToString();
                    string sname = Session["UserName"].ToString();
                    nameC = sname;
                    string srole = Session["userType"].ToString();
                    string concat = sname + " - " + srole;
                    lbluser1.Text = sname;
                    lblplant.Text = srole;
                    LblReq0.Text = Request.QueryString["Number"];

                    var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
                    SqlConnection con1;
                    con1 = new SqlConnection(connetionString1);
                    con1.Open();
                    DataTable Result = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
                    string str = "select VendorCode1,VendorName,QuoteNo,RequestNumber,CONVERT(varchar, QuoteResponseDueDate, 103) from TQuoteDetails WHERE RequestNumber ='" + LblReq0.Text + "'";
                    da = new SqlDataAdapter(str, con1);
                    Result = new DataTable();
                    da.Fill(Result);

                    if (Result.Rows.Count > 0)
                    {
                        DateTime dt = DateTime.ParseExact(Result.Rows[0].ItemArray[4].ToString(), "dd/MM/yyyy", null);
                        //DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);

                        txtDate.Text = dt.ToString("dd/MM/yyyy");

                        txtReqDate.Text = dt.ToString("dd/MM/yyyy");
                        grdvendor.DataSource = Result;
                        grdvendor.DataBind();
                    }
                    else
                    {
                        txtDate.Text = Result.Rows[1].ToString();
                        grdvendor.DataSource = Result;
                        grdvendor.DataBind();
                    }
                    con1.Close();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();

            //DateTime ttt = Convert.ToDateTime(txtDate.Text);
            txtReqDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //DateTime newEffDate = Convert.ToDateTime(txtDate.Text);
            //DateTime newDueon = Convert.ToDateTime(txtReqDate.Text);
            //DateTime newEffDate = DateTime.Parse(txtDate.Text);
            //DateTime newDueon = DateTime.Parse(txtReqDate.Text);
            DateTime newEffDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
            DateTime newDueon = DateTime.ParseExact(txtReqDate.Text, "dd/MM/yyyy", null);

            int result = DateTime.Compare(newEffDate, newDueon);
            if ((result > 0) && (txtDate.Text != ""))
            {
                    DataTable Result1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    string str1 = "update TQuoteDetails set QuoteResponseDueDate = '" + newEffDate.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + LblReq0.Text + "'";
                    da1 = new SqlDataAdapter(str1, con1);
                    Result1 = new DataTable();
                    da1.Fill(Result1);
                    Response.Redirect("Request_Waiting.aspx");
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Date update Successfully');", true);
            }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Date should be greater than current date');", true);
                }
        }
    }
}