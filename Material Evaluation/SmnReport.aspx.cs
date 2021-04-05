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
using System.Threading;
using System.Text;
using System.Reflection;

namespace Material_Evaluation
{
    public partial class SmnReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_SMNReport";
                        string PL = Session["EPlant"].ToString();
                        TxtPlant.Text = Session["EPlant"].ToString();

                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            string userId = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;
                            lblUser.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

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
                    else
                    {
                        LastId.Text = "0";
                        if (TxtExtraFilter.Text != "")
                        {
                            string[] ExtraFilter = TxtExtraFilter.Text.Split('|');
                            LastId.Text = ExtraFilter.Count().ToString();
                            for (int ex = 0; ex < ExtraFilter.Count(); ex++)
                            {
                                string[] ExtraFilterDet = ExtraFilter[ex].ToString().Split(':');
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();GenerateMulti("+ LastId.Text + ");ReturnExtraFiltercondition();", true);
                    }
                }
            }
            catch (ThreadAbortException ex2)
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