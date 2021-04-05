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

namespace Material_Evaluation
{
    public partial class VAllRequest : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;
        string mappeduserid;
        string mappedname;
        
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
                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_VAllRequest";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            userId = Session["userID_"].ToString();
                            sname = Session["UserName"].ToString();
                            srole = Session["userType"].ToString();
                            mappeduserid = Session["mappedVendor"].ToString();
                            
                            mappedname = Session["mappedVname"].ToString();
                            
                            concat = sname + " - " + mappedname;
                            lblUser.Text = sname;
                            lblplant.Text = mappedname;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            TxtPlant.Text = Session["VPlant"].ToString();
                            TxtVendorCode.Text = Session["mappedVendor"].ToString();
                            TxtVendorType.Text = Session["VendorType"].ToString();
                        }

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