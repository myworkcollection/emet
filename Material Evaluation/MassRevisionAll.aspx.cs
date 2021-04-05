using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class MassRevisionAll : System.Web.UI.Page
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
                        string FN = "EMET_MassRevision";
                        string PL = Session["EPlant"].ToString();

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
                            TxtPlant.Text = Session["EPlant"].ToString();
                            TxtuseID.Text = userId;
                            TxtuserDept.Text = Session["userDept"].ToString();
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
                EMETModule.SendExcepToDB(ex);
            }
        }
    }
}