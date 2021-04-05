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

namespace Material_Evaluation
{
    public partial class Home_ : System.Web.UI.Page
    {

        bool IsAth;
        protected void isAuthor()
        {
            var connetionStringplant = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
            SqlConnection conplant;
            conplant = new SqlConnection(connetionStringplant);
            string sql;
            SqlDataReader reader;
            string FormName = "EMET_Home";
            string System = "EMET";
            try
            {
                conplant.Open();
                sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
                SqlCommand cmd = new SqlCommand(sql, conplant);
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
                conplant.Close();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }
        //end

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                string userId = Session["userID"].ToString();
                string sname = Session["UserName"].ToString();
                string srole = Session["userType"].ToString();
                string concat = sname + " " + srole;
                int flag = 0;
                lblUser.Text = sname;
                lblplant.Text = srole;

                isAuthor();
                if (IsAth == false)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');", true);
                    //Server.Transfer("login.aspx");
                    //Response.Redirect("login.aspx");
                    flag = 1;
                }
                else
                {
                    this.pirstatuscount();
                    this.vendorresponse();
                    this.Mrgstatus();
                    this.DIRstatus();
                    this.closed();
                    flag = 0;
                }

                if (flag == 1)
                {
                    //Response.Redirect("Login.aspx");
                }
            }

        }
        
        protected void vendorresponse()
        {
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            // string str = "select count(Distinct RequestNumber) from TQuoteDetails  where ApprovalStatus IS NULL and (CreateStatus != '' OR CreateStatus IS NOT NULL)"; // and PICApprovalStatus IS NULL";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null)"; // and PICApprovalStatus IS NULL";
            string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null)"; // and PICApprovalStatus IS NULL";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {
                string strcount = Result.Rows[0]["column1"].ToString();

                lblvenrresponse.Text = strcount.ToString();


                //  lblCount.Text = strcount.ToString();
            }

            con1.Close();
        }

        protected void pirstatuscount()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con_h;
            con_h = new SqlConnection(connetionString1);
            con_h.Open();
            DataTable Result_h = new DataTable();
            SqlDataAdapter da_h = new SqlDataAdapter();
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails  where (PICApprovalStatus IS NULL) and (ApprovalStatus =0 or ApprovalStatus IS NULL) and (CreateStatus != '' OR CreateStatus IS NOT NULL)";
            // string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 2 or PICApprovalStatus = 1 or PICApprovalStatus = 3  or PICApprovalStatus is  null)";
            string str_h = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber  in(select distinct RequestNumber from TQuoteDetails    where PICApprovalStatus = 0)";
            da_h = new SqlDataAdapter(str_h, con_h);
            Result_h = new DataTable();
            da_h.Fill(Result_h);

            if (Result_h.Rows.Count > 0)
            {
                string strcount = Result_h.Rows[0]["column1"].ToString();

                lblCount.Text = strcount.ToString();


                //  lblCount.Text = strcount.ToString();
            }

            con_h.Close();
        }
        protected void Mrgstatus()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails  where  (ManagerApprovalStatus IS NULL) and (PICApprovalStatus =1 or PICApprovalStatus=2) and (CreateStatus != '' OR CreateStatus IS NOT NULL) ";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ManagerApprovalStatus = 2 or ManagerApprovalStatus = 1 or ManagerApprovalStatus = 3  or ManagerApprovalStatus is  null)";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber  in(select distinct RequestNumber from TQuoteDetails    where ManagerApprovalStatus = 0)";
            string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0) and ManagerApprovalStatus=0 ";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {
                string strcount = Result.Rows[0]["column1"].ToString();

                lblmgr.Text = strcount.ToString();


                //  lblCount.Text = strcount.ToString();
            }

            con1.Close();
        }
        protected void DIRstatus()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails  where (DIRApprovalStatus IS NULL) and (ManagerApprovalStatus =1 or ManagerApprovalStatus =2) and (CreateStatus != '' OR CreateStatus IS NOT NULL)";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where DIRApprovalStatus = 2 or DIRApprovalStatus = 1 or DIRApprovalStatus = 3 or DIRApprovalStatus is  null)";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber  in(select distinct RequestNumber from TQuoteDetails    where DIRApprovalStatus = 0)";
            string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where ManagerApprovalStatus = 0) and DIRApprovalStatus = 0";
            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {
                string strcount = Result.Rows[0]["column1"].ToString();

                lblDirector.Text = strcount.ToString();


                //  lblCount.Text = strcount.ToString();
            }

            con1.Close();
        }
        protected void closed()
        {

            //
            //string strprod = (string)HttpContext.Current.Session["plant"].ToString();

            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            con1.Open();
            DataTable Result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails  where (DIRApprovalStatus IS NULL) and (ManagerApprovalStatus =1 or ManagerApprovalStatus =2) and (CreateStatus != '' OR CreateStatus IS NOT NULL)";
            //string str = "select count(Distinct RequestNumber) from TQuoteDetails where RequestNumber in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 3)";
            // string str = "select count(distinct RequestNumber) from TQuoteDetails where ApprovalStatus = 3 or ApprovalStatus = 1 and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 2 or PICApprovalStatus = 0 or PICApprovalStatus is null )and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ManagerApprovalStatus = 2 or ManagerApprovalStatus = 0)and RequestNumber not in(select distinct RequestNumber from TQuoteDetails where DIRApprovalStatus = 2 or DIRApprovalStatus = 0)";
            string str = "select count(distinct RequestNumber) from TQuoteDetails where ApprovalStatus = 3 or ApprovalStatus = 1 ";

            da = new SqlDataAdapter(str, con1);
            Result = new DataTable();
            da.Fill(Result);

            if (Result.Rows.Count > 0)
            {
                string strcount = Result.Rows[0]["column1"].ToString();

                lblclosed.Text = strcount.ToString();
                //  lblCount.Text = strcount.ToString();
            }
            con1.Close();
        }

    }
}