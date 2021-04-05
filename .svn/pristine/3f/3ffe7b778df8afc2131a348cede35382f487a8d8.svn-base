using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class Announcement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // lblUserDetails.Text = Session["UserName"].ToString();

                // lblUserDetails.Text = "User : " + Session["UserName"].ToString() + "";
                string userId = Session["userID"].ToString();

                //   lblCount.Text = "";

                string sname = Session["UserName"].ToString();
                string srole = Session["userType"].ToString();
                string concat = sname + " " + srole;

                lblUser.Text = sname;
                lblplant.Text = srole;

                string a = Request.ApplicationPath + "/Files/attachment";
            }

        }
    }
}