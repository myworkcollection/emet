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
    public partial class PIRGeneration_C : System.Web.UI.Page
    {
        public string userId1;
        public string nameC;
        public string aemail;
        public string pemail;
        public string pemail1;
        public string Uemail;
        public string body1;
        public string quoteno;
        public string quoteno1;
        public int benable;
        public string vname;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {
                if (!IsPostBack)
                {
                    string userId = Session["userID"].ToString();
                    userId1 = userId.ToString();
                    string sname = Session["UserName"].ToString();
                    string srole = Session["userType"].ToString();
                    string concat = sname + " - " + srole;
                    userId1 = Session["userID"].ToString();
                    nameC = sname;
                    lbluser1.Text = sname;
                    lblplant.Text = srole;

                 
                }
            }

        }

        public void GetGridData()
        {
            string userID = (string)HttpContext.Current.Session["UserName"].ToString();
            var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
            SqlConnection con1;
            con1 = new SqlConnection(connetionString1);
            try
            {
                con1.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable Result1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter();
                string str1 = "delete from TQuoteDetails";
                da1 = new SqlDataAdapter(str1, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str2 = "delete from TMCCostDetails";
                da1 = new SqlDataAdapter(str2, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str3 = "delete from TOtherCostDetails";
                da1 = new SqlDataAdapter(str3, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str4 = "delete from TProcessCostDetails";
                da1 = new SqlDataAdapter(str4, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);


                string str5 = "delete from TSMCCostDetails";
                da1 = new SqlDataAdapter(str5, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str6 = "delete from TQuoteDetails_d";
                da1 = new SqlDataAdapter(str6, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str7 = "delete from TMCCostDetails_d";
                da1 = new SqlDataAdapter(str7, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str8 = "delete from TOtherCostDetails_d";
                da1 = new SqlDataAdapter(str8, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str9 = "delete from TProcessCostDetails_d";
                da1 = new SqlDataAdapter(str9, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);

                string str10 = "delete from TSMCCostDetails_d";
                da1 = new SqlDataAdapter(str10, con1);
                Result1 = new DataTable();
                da1.Fill(Result1);
                Response.Redirect("Home.aspx");


            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                con1.Close();
            }

        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            string userId = Session["userID"].ToString();

            if (userId == "SPLB8047")
            {
                GetGridData();
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');", true);
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Your Account dont have access for this page, please contact admin");
                var script = string.Format("alert({0});window.location ='login.aspx';", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);

            }
        }
    }
}
