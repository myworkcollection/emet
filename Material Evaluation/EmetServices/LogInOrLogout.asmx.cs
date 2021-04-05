using Material_Evaluation.EmetServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Material_Evaluation.EmetServices
{
    /// <summary>
    /// Summary description for LogInOrLogout
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class LogInOrLogout : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Logout()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            GlobalResult MyResult = new GlobalResult();

            Session.Abandon();
            Session.Clear();

            MyResult.success = true;
            MyResult.message = "Bye Bye";
            //string strJSON = js.Serialize(MyResult);
            Context.Response.Write(js.Serialize(MyResult));
        }
    }
}
