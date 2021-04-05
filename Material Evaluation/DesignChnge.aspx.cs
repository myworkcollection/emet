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
    public partial class DesignChnge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void article_CheckedChanged(object sender, EventArgs e)
        {

            if (article.Checked == true)
            {
                this.changevendr.Checked = false;
                // this.draftcost.Checked = false;
                Response.Redirect("RevisionofMET.aspx");


            }


        }

        protected void changevendr_CheckedChanged(object sender, EventArgs e)
        {

            if (changevendr.Checked == true)
            {
                this.article.Checked = false;
                // this.draftcost.Checked = false;

                Response.Redirect("DesignChnge.aspx");


            }


        }

        protected void OnRadio_Changed(object sender, EventArgs e)
        {

             
            if (RadioButtonList1.SelectedItem.Text == "by Vendor")
            {
                txtplant.BackColor = System.Drawing.Color.LightGray;
                txtprocdesc.ReadOnly = true;
                txtprocdesc.BackColor = System.Drawing.Color.LightGray;
                // txtproddesc.BackColor = System.Drawing.Color.LightGray;
                //                ddlproduct.ForeColor = System.Drawing.Color.LightGray;
                ddlproduct.BackColor = System.Drawing.Color.LightGray;
                 // txtpartdesc.BackColor = System.Drawing.Color.LightGray;
                  //ddljobtypedesc.BackColor = System.Drawing.Color.LightGray;
                  //DropDownList1.BackColor = System.Drawing.Color.LightGray;
            //    txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                txtplantstatus.BackColor = System.Drawing.Color.LightGray;
                txtproctype.BackColor = System.Drawing.Color.LightGray;
                txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                txtPIRType.BackColor = System.Drawing.Color.LightGray;
                txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                txtunitweight.BackColor = System.Drawing.Color.LightGray;
                txtUOM.BackColor = System.Drawing.Color.LightGray;
                ddlmatlclass.BackColor = System.Drawing.Color.LightGray;
                txtvendor.BackColor = System.Drawing.Color.Transparent;
                txtvendrID.BackColor = System.Drawing.Color.Transparent;


            }
            else if (RadioButtonList1.SelectedItem.Text == "by Product")
            {
                ddlproduct.BackColor = System.Drawing.Color.Transparent;
                txtplant.BackColor = System.Drawing.Color.LightGray;
                txtvendrID.ReadOnly = true;
                txtvendrID.BackColor = System.Drawing.Color.LightGray;
                txtvendor.BackColor = System.Drawing.Color.LightGray;
                //   txtpartdesc.BackColor = System.Drawing.Color.LightGray;
              //  txtjobtypedesc.BackColor = System.Drawing.Color.LightGray;
                txtpartsurface.BackColor = System.Drawing.Color.LightGray;
                txtplantstatus.BackColor = System.Drawing.Color.LightGray;
                txtproctype.BackColor = System.Drawing.Color.LightGray;
                txtPIRDesc.BackColor = System.Drawing.Color.LightGray;
                txtPIRType.BackColor = System.Drawing.Color.LightGray;
                txtSAPspProctype.BackColor = System.Drawing.Color.LightGray;
                txtunitweight.BackColor = System.Drawing.Color.LightGray;
                txtUOM.BackColor = System.Drawing.Color.LightGray;
                ddlmatlclass.BackColor = System.Drawing.Color.LightGray;

              //  txtpartdesc.BackColor = System.Drawing.Color.LightGray;
                //ddljobtypedesc.BackColor = System.Drawing.Color.LightGray;
                //DropDownList1.BackColor = System.Drawing.Color.LightGray;


            }
        

        }

    }
}