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
    public partial class eMET_login : System.Web.UI.Page
    {

    


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               // string strLDAP_LoginID = Request.ServerVariables("LOGON_USER").ToString();

               // string strLDAP_LoginID = HttpContext.Current.Request.ServerVariables["LOGON_USER"].ToString();

                string strLDAP_LoginID = HttpContext.Current.Request.LogonUserIdentity.Name;

                string[] logincred = strLDAP_LoginID.Split('\\');
                string getLDAPLogin = logincred[1].ToString();

            //    base.Page_Load_Common();
                if (!Page.IsPostBack)
                {


                    // MyBase.WriteUserLog(strLDAP_LoginID)

                    if (CheckEldapLogin(getLDAPLogin))
                    {
                        Response.Redirect("Home.aspx", false);
                    }
                    else
                    {

                        Response.Redirect("Login.aspx", false);
                    }
                }

            }
            catch (Exception ex)
            {
               throw ex;
            }
        }


        public bool CheckEldapLogin(string pstrUserName)
        {
            try
            {
                if (CheckUserCredintal(pstrUserName, "UseID") == true)
                {
                //    gobjUserSession.gboolUserLoged = true;
                    
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckUserCredintal(string pstrUserLogin, string pstrCheckBy)
        {
            try
            {

                var connetionStringdate = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

                SqlConnection condate;

                condate = new SqlConnection(connetionStringdate);
                condate.Open();

                DataTable dtUser = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

              string   str = "select UseID,UseNam,UsePass,UseDep,UseLev,UseEmail,UseComCod,BusFunc from usr where UPPER(" + pstrCheckBy + ") ='" + pstrUserLogin.Trim().ToUpper() + "'";
                da = new SqlDataAdapter(str, condate);
                da.Fill(dtUser);
                if (pstrUserLogin != "")
                {
                    if (dtUser.Rows.Count > 0)
                    {

                        Session["userID"] = dtUser.Rows[0]["UseID"].ToString();
                        Session["UserName"] = dtUser.Rows[0]["UseNam"].ToString();
                        Session["UserPassword"] = dtUser.Rows[0]["UsePass"].ToString();
                        Session["UserDept"] = dtUser.Rows[0]["UseDep"].ToString();
                        Session["UserLevel"] = dtUser.Rows[0]["UseLev"].ToString();
                        Session["UserEmail"] = dtUser.Rows[0]["UseEmail"].ToString();
                        Session["UserComCod"] = dtUser.Rows[0]["UseComCod"].ToString();
                        Session["UserBusFunc"] = dtUser.Rows[0]["BusFunc"].ToString();


                        //gobjUserSession.gstrUserID = dtUser.Rows(0).Item("user_id");
                        //gobjUserSession.gstrGroupID = dtUser.Rows(0).Item("group_id");
                        //gobjUserSession.gstrUserName = dtUser.Rows(0).Item("name");
                        //gobjUserSession.gstrUserRoleId = dtUser.Rows(0).Item("user_role");
                        //// gobjUserSession.gstrUserpassword = dtUser.Rows(0).Item("user_password")
                        //gobjUserSession.gintLanguageID = dtUser.Rows(0).Item("LANGUAGE_ID");
                        //gobjUserSession.gboolDiscontAnyMat = dtUser.Rows(0).Item("ALLOW_DISCONT_MAT");
                        //gobjUserSession.gstr_UserEmai = dtUser.Rows(0).Item("email");
                        //gobjUserSession.gboolUserLoged = true;

                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {

                    return false;
                }


             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}