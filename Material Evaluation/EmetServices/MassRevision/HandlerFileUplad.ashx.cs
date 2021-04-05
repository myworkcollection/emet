using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Material_Evaluation.EmetServices.MassRevision
{
    /// <summary>
    /// Summary description for HandlerFileUplad
    /// </summary>
    public class HandlerFileUplad : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //var formdata = HttpContext.Current.Request.Form;
                //if (formdata.AllKeys.Length > 0)
                //{

                //}
                //else
                //{

                //}

                #region File Attchment Info
                if (HttpContext.Current.Session["userID"] == null)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("User Session Expired");
                }
                else
                {
                    HttpPostedFile file = null;
                    if (context.Request.Files.Count > 0)
                    {
                        string filedata = string.Empty;
                        HttpFileCollection files = context.Request.Files;
                        string SessionName = HttpContext.Current.Session["userID"].ToString() + "_" + "FileMassUploadALL";
                        file = files[0];
                        if (file.ContentLength > 0)
                        {
                            string FileName = file.FileName.ToString();
                            context.Session.Add(SessionName, file);

                            context.Response.ContentType = "text/plain";
                            context.Response.Write("OK");
                        }
                        else
                        {
                            if (HttpContext.Current.Session[SessionName] != null)
                            {
                                HttpContext.Current.Session[SessionName] = null;
                            }

                            context.Response.ContentType = "text/plain";
                            context.Response.Write("Upload File File, Please Try Again or Contact Administrator");
                        }
                    }
                    else {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("Upload File File, Please Contact Administrator");
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}