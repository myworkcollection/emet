using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{
    public partial class SaveFileUploadAttachment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Source"]))
            {
                if (Request.Files.Count > 0)
                {
                    if (Request.QueryString["Source"] == "NewReq") {
                        HttpPostedFile file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            Session["FlAttchDrawing"] = file;
                        }
                        else
                        {
                            Session["FlAttchDrawing"] = null;
                        }
                    }
                }
            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFile file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {

                        string fileName = Path.GetFileName(file.FileName);
                        Session["FlAttachment"] = file;
                    }
                    else
                    {
                        Session["FlAttachment"] = null;
                    }
                }
                else
                {
                    Session["FlAttachment"] = null;
                }
            }
        }
    }
}