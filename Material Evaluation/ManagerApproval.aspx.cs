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
using System.Web.UI.HtmlControls;
using System.Threading;

namespace Material_Evaluation
{
    public partial class ManagerApproval : System.Web.UI.Page
    {
        //email
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string SendFilename;
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
        public string demail;
        public string vemail;
        public string customermail;
        public string customermail1;
        public string cc;
        //email

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool IsAth;
        bool sendingemail = false;
        string DbMasterName = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null || Session["UserName"] == null || Session["userType"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (Session["sidebarToggle"] == null)
                    {
                        SideBarMenu.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        SideBarMenu.Attributes.Add("style", "display:none;");
                    }

                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_ShimanoManagerApr";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=19");
                        }
                        else
                        {
                            string userId = Session["userID"].ToString();
                            userId1 = userId.ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;

                            userId1 = Session["userID"].ToString();
                            nameC = sname;

                            lblUser.Text = sname;
                            lblplant.Text = srole;

                            //if (Session["DIRApprovalFilter"] != null)
                            //{
                            //    string[] ArrApprovalFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                            //    if (ArrApprovalFilter[0].ToString() != "")
                            //    {
                            //        ViewState["SortExpression"] = ArrApprovalFilter[0].ToString();
                            //    }
                            //    if (ArrApprovalFilter[1].ToString() != "")
                            //    {
                            //        ViewState["SortDirection"] = ArrApprovalFilter[1].ToString();
                            //    }
                            //    DdlFilterBy.SelectedValue = ArrApprovalFilter[2].ToString();
                            //    txtFind.Text = ArrApprovalFilter[3].ToString();

                            //    DdlFltrDate.SelectedValue = ArrApprovalFilter[4].ToString();
                            //    string[] ArrDate = ArrApprovalFilter[5].ToString().Split('~');

                            //    if (ArrDate.Count() == 2)
                            //    {
                            //        if (ArrDate[0].ToString() != "")
                            //        {
                            //            TxtFrom.Text = ArrDate[0].ToString();
                            //        }
                            //        if (ArrDate[1].ToString() != "")
                            //        {
                            //            TxtTo.Text = ArrDate[1].ToString();
                            //        }
                            //    }
                            //}

                            if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                            {
                                if (Request.QueryString["Num"] == "2")
                                {
                                    DdlReqStatus.SelectedValue = "MassRevision";
                                    DdlReqStatus.SelectedValue = "MassRevision";
                                    if (DdlDiffrence.SelectedValue == "><")
                                    {
                                        DvSingleCondition.Visible = false;
                                        DvMultiCondition.Visible = true;
                                    }
                                    else
                                    {
                                        DvSingleCondition.Visible = true;
                                        DvMultiCondition.Visible = false;
                                    }

                                    DvFilterDiffrence.Visible = true;

                                    DvMassRev.Visible = true;
                                    DvNormalData.Visible = false;
                                }
                                else if (Request.QueryString["Num"] == "1")
                                {
                                    DdlReqStatus.SelectedValue = "All";
                                    DvFilterDiffrence.Visible = false;
                                }
                            }

                            LastFilterCondition();
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            if (Session["TxtShowEntMassRevDIR"] != null)
                            {
                                TxtShowEntMassRev.Text = Session["TxtShowEntMassRevDIR"].ToString();
                            }

                            if (Session["ShowEntryMngApproval"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryMngApproval"].ToString();
                            }

                            if (DdlReqStatus.SelectedValue == "MassRevision")
                            {
                                DvMassRev.Visible = true;
                                DvNormalData.Visible = false;

                                if (Session["DtMassRevDIR"] != null)
                                {
                                    DataTable dt = (DataTable)Session["DtMassRevDIR"];
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (Session["TxtShowEntMassRev"] != null)
                                        {
                                            TxtShowEntMassRev.Text = Session["TxtShowEntMassRevDIR"].ToString();
                                        }
                                        GdvMassRev.DataSource = dt;
                                        int ShowEntry = 1;
                                        if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                                        {
                                            ShowEntry = 1;
                                            TxtShowEntMassRev.Text = "1";
                                        }
                                        else
                                        {
                                            ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                                        }
                                        GdvMassRev.PageSize = ShowEntry;
                                        GdvMassRev.DataBind();
                                        #region pagination last view
                                        if (Session["MassRevPgNoDIR"] != null)
                                        {
                                            int PgNo = Convert.ToInt32(Session["MassRevPgNoDIR"].ToString());
                                            if (GdvMassRev.PageCount >= PgNo)
                                            {
                                                GdvMassRev.PageIndex = PgNo;
                                                GdvMassRev.DataBind();
                                            }
                                            else
                                            {
                                                Session["MassRevPgNoDIR"] = null;
                                            }
                                        }
                                        #endregion
                                        CheckAllAprOrRej();
                                    }
                                }
                                else
                                {
                                    ShowTableMassRev();
                                }
                            }
                            else
                            {
                                ShowTable();
                                DvMassRev.Visible = false;
                                DvNormalData.Visible = true;
                            }

                            GetDdlReasonReject();
                            //GetGridData();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
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

        protected void LastFilterCondition()
        {
            try
            {

                if (Session["DIRApprovalFilter"] != null)
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    if (ArrFilter[0].ToString() != "")
                    {
                        ViewState["SortExpression"] = ArrFilter[0].ToString();
                    }
                    if (ArrFilter[1].ToString() != "")
                    {
                        ViewState["SortDirection"] = ArrFilter[1].ToString();
                    }
                    DdlFilterBy.SelectedValue = ArrFilter[2].ToString();
                    txtFind.Text = ArrFilter[3].ToString();

                    DdlFltrDate.SelectedValue = ArrFilter[4].ToString();
                    string[] ArrDate = ArrFilter[5].ToString().Split('~');

                    if (ArrDate.Count() == 2)
                    {
                        if (ArrDate[0].ToString() != "")
                        {
                            TxtFrom.Text = ArrDate[0].ToString();
                        }
                        if (ArrDate[1].ToString() != "")
                        {
                            TxtTo.Text = ArrDate[1].ToString();
                        }
                    }

                    DdlStatus.SelectedValue = ArrFilter[6].ToString();
                    if (DdlReqStatus.SelectedValue == "MassRevision")
                    {
                        if (DdlDiffrence.SelectedValue == "><")
                        {
                            DvSingleCondition.Visible = false;
                            DvMultiCondition.Visible = true;
                        }
                        else
                        {
                            DvSingleCondition.Visible = true;
                            DvMultiCondition.Visible = false;
                        }
                    }
                    DdlReqStatus.SelectedValue = ArrFilter[7].ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetDdlReasonReject()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select ReasonforRejection from TREASONFORMETREJECTION where DelFlag=0 and ReasonType='Rejection' and SysCode = 'emet' and Plant=@Plant order by ReasonforRejection ASC";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            //create new row
                            DataRow dr = null;
                            dr = dt.NewRow();
                            dr["ReasonforRejection"] = "VENDOR DID NOT REPLY";
                            //add the row to DataTable
                            dt.Rows.Add(dr);
                            Session["DdlReasonReject"] = dt;
                        }
                        else
                        {
                            Session["DdlReasonReject"] = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
        //old code
        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EMETModule.AutoUpdateRequest();
                EMETModule.autoupdate(Session["EPlant"].ToString());
                EMETModule.autoCloseWhitoutsapCode(Session["EPlant"].ToString());
                EMETModule.CekAndCloseExpiredRequest(Session["EPlant"].ToString());

                GetDbMaster();
                EmetCon.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct Plant,RequestNumber,
                    //        CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,
                    //        (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                    //        CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                    //        Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy'
                    //        from TQuoteDetails A where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0) and ManagerApprovalStatus=0";

                    sql = @" select distinct Plant,RequestNumber,(select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            case 
                            when A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null) then 'New' 
                            when A.QuoteNoRef is null and (A.isMassRevision = 1) then 'Mass Revision' 
                            else 'Revision' 
                            end as 'ReqStatus'
                            from TQuoteDetails A 
                            where  A.Plant  = '" + Session["EPlant"].ToString() + "' and (A.isMassRevision = 0 or A.isMassRevision is null) ";

                    sql += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 and s.System = 'eMET' )) ";

                    //sql += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";

                    if (DdlReqStatus.SelectedValue == "New")
                    {
                        sql += @" and A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null)";
                    }
                    else if (DdlReqStatus.SelectedValue == "Revision")
                    {
                        sql += @" and A.QuoteNoRef is not null";
                    }

                    if (DdlStatus.SelectedValue.ToString() == "Approved")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=2 and ManagerApprovalStatus = 0 ";
                    }
                    else if (DdlStatus.SelectedValue.ToString() == "Rejected")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=1 and ManagerApprovalStatus = 0 ";
                    }
                    else
                    {
                        sql += @" and RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) and ManagerApprovalStatus=0)  ";
                    }

                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        if (DdlFltrDate.SelectedValue.ToString() == "RequestDate")
                        {
                            sql += @" and format(RequestDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                        else if (DdlFltrDate.SelectedValue.ToString() == "QuoteResponseDueDate")
                        {
                            sql += @" and format(QuoteResponseDueDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                    }

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "Plant")
                        {
                            sql += @" and Plant like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "RequestNumber")
                        {
                            sql += @" and RequestNumber like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Product")
                        {
                            sql += @" and Product like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Material")
                        {
                            sql += @" and Material like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "MaterialDesc")
                        {
                            sql += @" and MaterialDesc like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "QuoteNo")
                        {
                            sql += @" and QuoteNo like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode1")
                        {
                            sql += @" and VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorName")
                        {
                            sql += @" and VendorName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "CreatedBy")
                        {
                            sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "UseDep")
                        {
                            sql += @" and SMNPicDept like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroup")
                        {
                            sql += @" and ProcessGroup like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = ProcessGroup) like '%'+@Filter+'%' ";
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "RequestDate" || ViewState["SortExpression"].ToString() == "QuoteResponseDueDate")
                        {
                            sql += @" Order by CONVERT(DateTime, " + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                        }
                        else
                        {
                            sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                    }
                    else
                    {
                        sql += @" Order by RequestNumber desc ";
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        DateTime DtFrom = DateTime.ParseExact(TxtFrom.Text, "dd/MM/yyyy", null);
                        DateTime Dtto = DateTime.ParseExact(TxtTo.Text, "dd/MM/yyyy", null);

                        cmd.Parameters.AddWithValue("@From", DtFrom.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Dtto.ToString("yyyy-MM-dd"));
                    }
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        int ShowEntry = 1;
                        if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
                        {
                            ShowEntry = 1;
                            TxtShowEntry.Text = "1";
                        }
                        else
                        {
                            ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
                        }
                        GridView1.PageSize = ShowEntry;
                        Session["ShowEntryMngApproval"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["DIRApprPgNo"] != null)
                            {
                                int PgNo = Convert.ToInt32(Session["DIRApprPgNo"].ToString());
                                if (GridView1.PageCount >= PgNo)
                                {
                                    GridView1.PageIndex = PgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["DIRApprPgNo"] = null;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            LbTtlRecords.Text = "Total Record : 0";
                        }
                    }
                }
                UpdatePanel1.Update();

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        string GetReqType(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            string TrsType = "";
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNoRef,isMassRevision from TQuoteDetails where QuoteNo = @QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["QuoteNoRef"].ToString() == "")
                            {
                                TrsType = "New";
                            }
                            else
                            {
                                TrsType = "Revision";
                            }
                        }
                        else
                        {
                            TrsType = "";
                        }
                    }
                }
                return TrsType;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return TrsType;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        //old code


        //New code  by subash
        public void UpdateGridData(string ReqNum, string Vendor, int Status, string Reason)
        {

            string userID = (string)HttpContext.Current.Session["UserName"].ToString();

            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (Status == 1)
                {
                    try
                    {

                        DataTable Result1 = new DataTable();
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        string pic = Reason.ToString();
                        //string pic = Reason.ToString();
                        //string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "', PICReason = '" + pic + "', ManagerReason = '" + pic + "', DIRReason = '" + pic + "', UpdatedBy='" + Session["userID"].ToString() + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', ManagerApprovalStatus= '" + 1 + "',  ManagerReason = '" + pic + "',DIRApprovalStatus = '" + 0 + "',DIRReason = '" + pic + "',  UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        //string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', ManagerApprovalStatus= '" + 1 + "',  ManagerReason = '" + pic + "',  UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        da1 = new SqlDataAdapter(str1, EmetCon);
                        Result1 = new DataTable();
                        da1.Fill(Result1);

                        DataTable Result11 = new DataTable();
                        SqlDataAdapter da11 = new SqlDataAdapter();
                        //string pic1 = Session["UserName"].ToString() + " - " + Reason.ToString();

                        //string pic1 = Reason.ToString();
                        //string str11 = "Update TQuoteDetails SET  PICApprovalStatus= '" + 1 + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                        //da11 = new SqlDataAdapter(str11, con1);
                        //Result11 = new DataTable();
                        //da11.Fill(Result11);

                        getcreateusermail(ReqNum, Vendor);
                    }
                    catch (Exception ex)
                    {
                        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                        EMETModule.SendExcepToDB(ex);
                    }

                    #region sending email
                    sendingemail = false;
                    string MsgErr = "";

                    try
                    {
                        //getting PIC mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_PIC_Details";

                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = ReqNum.ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            pemail1 = string.Empty;
                            Session["pemail1"] = "";
                            while (dr.Read())
                            {

                                pemail1 = string.Concat(pemail1, dr.GetString(0), ";");
                                Session["pemail1"] = string.Concat(Session["pemail1"].ToString(), dr.GetString(0), ";");

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        //getting manager mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            aemail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_managerdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = "1";
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["aemail"] = "";
                            while (dr.Read())
                            {
                                aemail = string.Concat(aemail, dr.GetString(0), ";");
                                Session["aemail"] = string.Concat(Session["aemail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }



                        //getting User mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_userdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = Session["userID"].ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            while (dr.Read())
                            {
                                Uemail = dr.GetString(0);
                                Session["Uemail"] = dr.GetString(0);
                                Session["dept"] = dr.GetString(1);
                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        //getting Quote details
                        using (SqlConnection Qcnn = new SqlConnection(EMETModule.GenEMETConnString()))
                        {
                            string returnValue2 = string.Empty;
                            Qcnn.Open();
                            SqlCommand qcmdget = Qcnn.CreateCommand();
                            qcmdget.CommandType = CommandType.StoredProcedure;
                            qcmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                            SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            id.Direction = ParameterDirection.Input;
                            id.Value = ReqNum.ToString();
                            qcmdget.Parameters.Add(id);

                            SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                            Vid.Direction = ParameterDirection.Input;
                            Vid.Value = Vendor.ToString();
                            qcmdget.Parameters.Add(Vid);

                            SqlDataReader qdr;
                            qdr = qcmdget.ExecuteReader();
                            while (qdr.Read())
                            {

                                quoteno = qdr.GetString(0);
                                Session["quoteno"] = qdr.GetString(0);

                            }
                            qdr.Dispose();
                            Qcnn.Dispose();
                        }



                        //// comments
                        try
                        {
                            DataTable dtgett = new DataTable();
                            DataTable dtdate = new DataTable();
                            SqlDataAdapter daa = new SqlDataAdapter();
                            string strGetDataa = string.Empty;
                            strGetDataa = @"select case when DIRReason is null then ' Remark : ' + DIRRemark
                                        else ' Reason : ' + DIRReason from TQuoteDetails where RequestNumber = '" + ReqNum.ToString() + "' and VendorCode1 ='" + Vendor.ToString() + "'  ";
                            daa = new SqlDataAdapter(strGetDataa, EmetCon);
                            daa.Fill(dtgett);
                            if (dtgett.Rows.Count > 0)
                            {
                                string ttt = dtgett.Rows[0].ItemArray[0].ToString();
                                Session["DIRcomment"] = dtgett.Rows[0].ItemArray[0].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex);
                        }
                        ////

                        //getting Director mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            demail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Dir_approval";

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlParameter dept = new SqlParameter("@dept", SqlDbType.NVarChar);
                            dept.Direction = ParameterDirection.Input;
                            dept.Value = Session["dept"].ToString();
                            cmdget.Parameters.Add(dept);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["demail"] = "";
                            while (dr.Read())
                            {
                                demail = string.Concat(demail, dr.GetString(0), ";");
                                Session["demail"] = string.Concat(Session["demail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        pemail1 = Session["pemail1"].ToString();
                        aemail = Session["aemail"].ToString();
                        demail = Session["demail"].ToString();
                        Uemail = Session["Uemail"].ToString();
                        getcreateusermail(ReqNum, Vendor);
                        //cc = string.Concat(pemail1, aemail, demail, Uemail);
                        string crem = Session["createemail"].ToString();
                        cc = string.Concat(demail, Session["createemail"].ToString());
                        //main mail
                        //getting Customer mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Customer_mail";

                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = ReqNum.ToString();
                            cmdget.Parameters.Add(vendorid);

                            int status1 = 1;
                            SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                            status.Direction = ParameterDirection.Input;
                            status.Value = Convert.ToInt32(status1.ToString());
                            cmdget.Parameters.Add(status);


                            SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                            Quote.Direction = ParameterDirection.Input;
                            Quote.Value = Session["quoteno"].ToString();
                            cmdget.Parameters.Add(Quote);

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlDataReader dr_cus;
                            dr_cus = cmdget.ExecuteReader();
                            pemail1 = string.Empty;
                            Session["customermail"] = "";
                            Session["customermail1"] = "";
                            while (dr_cus.Read())
                            {

                                customermail = dr_cus.GetString(0);
                                Session["customermail"] = dr_cus.GetString(0);
                                customermail1 = dr_cus.GetString(1);
                                Session["customermail1"] = dr_cus.GetString(1);
                                customermail = string.Concat(customermail, ";", customermail1);
                                //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                //while start


                                //email
                                // getting Messageheader ID from IT Mailapp
                                
                                using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    SqlTransaction transaction;
                                    string returnValue1 = string.Empty;
                                    MHcnn.Open();
                                    transaction = MHcnn.BeginTransaction("Approval");
                                    try
                                    {
                                        SqlCommand MHcmdget = MHcnn.CreateCommand();
                                        MHcmdget.Transaction = transaction;
                                        MHcmdget.CommandType = CommandType.StoredProcedure;
                                        MHcmdget.CommandText = "dbo.spGetControlNumber";
                                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                        CompanyCode.Direction = ParameterDirection.Input;
                                        CompanyCode.Value = 1;
                                        MHcmdget.Parameters.Add(CompanyCode);

                                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                        ControlField.Direction = ParameterDirection.Input;
                                        ControlField.Value = "MessageHeaderID";
                                        MHcmdget.Parameters.Add(ControlField);

                                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                        Param1.Direction = ParameterDirection.Input;
                                        Param1.Value = "";
                                        MHcmdget.Parameters.Add(Param1);

                                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                        Param2.Direction = ParameterDirection.Input;
                                        Param2.Value = "";
                                        MHcmdget.Parameters.Add(Param2);

                                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                        Param3.Direction = ParameterDirection.Input;
                                        Param3.Value = "";
                                        MHcmdget.Parameters.Add(Param3);

                                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                        Param4.Direction = ParameterDirection.Input;
                                        Param4.Value = "";
                                        MHcmdget.Parameters.Add(Param4);

                                        SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                        pOutput.Direction = ParameterDirection.Output;
                                        MHcmdget.ExecuteNonQuery();
                                        transaction.Commit();
                                        returnValue1 = pOutput.Value.ToString();
                                        OriginalFilename = returnValue1;
                                        MHid = returnValue1;
                                        Session["MHid"] = returnValue1;
                                        OriginalFilename = MHid + seqNo + formatW;
                                    }
                                    catch (Exception xw)
                                    {
                                        transaction.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    MHcnn.Dispose();
                                }
                                Boolean IsAttachFile = true;
                                int SequenceNumber = 1;
                                string test = userId1;
                                IsAttachFile = false;
                                SendFilename = "NOFILE";
                                OriginalFilename = "NOFILE";
                                format = "NO";

                                //getting vendor mail content
                                using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    string returnValue_ = string.Empty;
                                    cnn_.Open();
                                    SqlCommand cmdget_ = cnn_.CreateCommand();
                                    cmdget_.CommandType = CommandType.StoredProcedure;
                                    cmdget_.CommandText = "dbo.Emet_Email_content";

                                    SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                    vendorid_.Direction = ParameterDirection.Input;
                                    vendorid_.Value = Session["quoteno"].ToString();
                                    cmdget_.Parameters.Add(vendorid_);

                                    SqlDataReader dr_;
                                    dr_ = cmdget_.ExecuteReader();
                                    while (dr_.Read())
                                    {
                                        body1 = dr_.GetString(1);
                                        Session["body1"] = dr_.GetString(1);
                                    }
                                    dr_.Dispose();
                                    cnn_.Dispose();
                                }

                                // Insert header and details to Mil server table to IT mailserverapp
                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    Email_inser.Open();
                                    //Header
                                    string MessageHeaderId = Session["MHid"].ToString();
                                    string fromname = "eMET System";
                                    //string FromAddress = Session["Uemail"].ToString(); 
                                    string FromAddress = "eMET@Shimano.Com.sg";
                                    //string Recipient = aemail + "," + pemail;
                                    string Recipient = Session["customermail"].ToString();
                                    string CopyRecipient = cc;
                                    string BlindCopyRecipient = "";
                                    string ReplyTo = "subashdurai@shimano.com.sg";
                                    string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Rejection By: " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                    //Rejection Reason: " + Session["DIRcomment"].ToString() + " <br />

                                    string transType = GetReqType(Session["quoteno"].ToString());
                                    string body = "";
                                    if (transType == "Revision")
                                    {
                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation <font color='red'> (Revision) </font> has been Rejected.  " + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                    }
                                    else
                                    {
                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected.  " + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                    }


                                    string BodyFormat = "HTML";
                                    string BodyRemark = "0";
                                    string Signature = " ";
                                    string Importance = "High";
                                    string Sensitivity = "Confidential";
                                    string CreateUser = userId1;
                                    DateTime CreateDate = DateTime.Now;
                                    //end Header

                                    SqlTransaction transactionHe;
                                    transactionHe = Email_inser.BeginTransaction("Header");
                                    try
                                    {
                                        string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                        SqlCommand Header = new SqlCommand(Head, Email_inser);
                                        Header.Transaction = transactionHe;
                                        Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                        Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                        Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                        Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                        Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                        Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                        Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                        Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                        Header.Parameters.AddWithValue("@body", body.ToString());
                                        Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                        Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                        Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                        Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                        Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                        Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                        Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Header.CommandText = Head;
                                        Header.ExecuteNonQuery();
                                        transactionHe.Commit();
                                    }
                                    catch (Exception xw)
                                    {
                                        transactionHe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    //end Header
                                    //Details
                                    SqlTransaction transactionDe;
                                    transactionDe = Email_inser.BeginTransaction("Detail");
                                    try
                                    {
                                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                        Detail.Transaction = transactionDe;
                                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                        Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                        Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Detail.CommandText = Details;
                                        Detail.ExecuteNonQuery();
                                        transactionDe.Commit();
                                    }
                                    catch (Exception xw)
                                    {
                                        transactionDe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    Email_inser.Dispose();
                                    //End Details
                                }
                                //while end
                            }
                            dr_cus.Dispose();
                            cnn.Dispose();
                        }
                        //end Rejected
                        //End by subash
                        //end of email

                        sendingemail = true;
                    }
                    catch (Exception ex)
                    {
                        sendingemail = false;
                        MsgErr = ex.ToString();
                    }

                    if (sendingemail == false)
                    {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + MsgErr + " ");
                        var script = string.Format("alert({0});", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        ShowTable();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('failed sending email');", true);
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('failed sending email'); ", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Rejected Successfully');", true);
                        ShowTable();
                        //Response.Redirect("Approval.aspx");
                    }
                    #endregion sending email
                }


                if (Status == 2)
                {
                    try
                    {

                        DataTable Result = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter();
                        string str;
                        //string str = "Update TQuoteDetails SET ApprovalStatus='" + 2 + "',PICApprovalStatus = '" + Status + "', ManagerApprovalStatus = '" + 0 + "' PICReason = '" + Reason + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";
                        //da = new SqlDataAdapter(str, con1);
                        //Result = new DataTable();
                        //da.Fill(Result);
                        //string reason1 = "Auto Rejected By PIC";
                        //str = "Update TQuoteDetails SET PICApprovalStatus = '" + 1 + "',ManagerApprovalStatus = '" + 0 + "', PICReason = '" + reason1 + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 not in('" + Vendor + "') and (PICApprovalStatus = '" + 0 + "' or PICApprovalStatus is null)";
                        //da = new SqlDataAdapter(str, con1);
                        //Result = new DataTable();
                        //da.Fill(Result);
                        //string pic = Session["UserName"].ToString() + " - " + Reason.ToString();
                        string pic = "";
                        //DateTime adt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        if (Reason.ToString() != "")
                        {
                            pic = Reason.ToString();
                        }
                        str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "', ManagerApprovalStatus= '" + 2 + "',  ManagerReason = '" + pic + "',DIRApprovalStatus = '" + 0 + "',DIRReason = '" + pic + "',  UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        //str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "', ManagerApprovalStatus= '" + 2 + "',  ManagerReason = '" + pic + "',UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        da = new SqlDataAdapter(str, EmetCon);
                        Result = new DataTable();
                        da.Fill(Result);

                        //DataTable Result11 = new DataTable();
                        //SqlDataAdapter da11 = new SqlDataAdapter();
                        ////string pic1 = Session["UserName"].ToString() + " - " + Reason.ToString();
                        //string pic1 = Reason.ToString();
                        //string str11 = "Update TQuoteDetails SET  ManagerApprovalStatus= '" + Status + "',  UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                        //da11 = new SqlDataAdapter(str11, con1);
                        //Result11 = new DataTable();
                        //da11.Fill(Result11);
                    }
                    catch (Exception ex)
                    {
                        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                        EMETModule.SendExcepToDB(ex);
                    }

                    #region sending email
                    sendingemail = false;
                    string MsgErr = "";

                    try
                    {
                        //Email

                        //getting PIC mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_PIC_Details";

                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = ReqNum.ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);


                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            pemail1 = string.Empty;
                            Session["pemail1"] = "";
                            while (dr.Read())
                            {

                                pemail1 = string.Concat(pemail1, dr.GetString(0), ";");
                                Session["pemail1"] = string.Concat(Session["pemail1"].ToString(), dr.GetString(0), ";");
                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        //getting manager mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            aemail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_managerdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = "1";
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["aemail"] = "";
                            while (dr.Read())
                            {
                                aemail = string.Concat(aemail, dr.GetString(0), ";");
                                Session["aemail"] = string.Concat(Session["aemail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        //getting User mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_userdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = Session["userID"].ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            while (dr.Read())
                            {
                                Uemail = dr.GetString(0);
                                Session["Uemail"] = dr.GetString(0);
                                Session["dept"] = dr.GetString(1);
                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }

                        //getting Director mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            demail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Dir_approval";

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlParameter dept = new SqlParameter("@dept", SqlDbType.NVarChar);
                            dept.Direction = ParameterDirection.Input;
                            dept.Value = Session["dept"].ToString();
                            cmdget.Parameters.Add(dept);
                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["demail"] = "";
                            while (dr.Read())
                            {
                                demail = string.Concat(demail, dr.GetString(0), ";");
                                Session["demail"] = string.Concat(Session["demail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }



                        //getting Quote details
                        using (SqlConnection Qcnn = new SqlConnection(EMETModule.GenEMETConnString()))
                        {
                            string returnValue2 = string.Empty;
                            Qcnn.Open();
                            SqlCommand qcmdget = Qcnn.CreateCommand();
                            qcmdget.CommandType = CommandType.StoredProcedure;
                            qcmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                            SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            id.Direction = ParameterDirection.Input;
                            id.Value = ReqNum.ToString();
                            qcmdget.Parameters.Add(id);

                            SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                            Vid.Direction = ParameterDirection.Input;
                            Vid.Value = Vendor.ToString();
                            qcmdget.Parameters.Add(Vid);

                            SqlDataReader qdr;
                            qdr = qcmdget.ExecuteReader();
                            while (qdr.Read())
                            {

                                quoteno = qdr.GetString(0);
                                Session["quoteno"] = qdr.GetString(0);

                            }
                            qdr.Dispose();
                            Qcnn.Dispose();
                        }


                        //// comments
                        try
                        {
                            DataTable dtgett = new DataTable();
                            DataTable dtdate = new DataTable();
                            SqlDataAdapter daa = new SqlDataAdapter();
                            string strGetDataa = string.Empty;
                            strGetDataa = @"select case when DIRReason is null then 'Remark : ' + DIRRemark
                                        else 'Reason : ' + DIRReason end as comment from TQuoteDetails where RequestNumber = '" + ReqNum.ToString() + "' and VendorCode1 ='" + Vendor.ToString() + "'  ";
                            daa = new SqlDataAdapter(strGetDataa, EmetCon);
                            daa.Fill(dtgett);
                            if (dtgett.Rows.Count > 0)
                            {
                                string ttt = dtgett.Rows[0].ItemArray[0].ToString();
                                Session["DIRcomment"] = dtgett.Rows[0].ItemArray[0].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex);
                        }
                        ////



                        pemail1 = Session["pemail1"].ToString();
                        aemail = Session["aemail"].ToString();
                        demail = Session["demail"].ToString();
                        Uemail = Session["Uemail"].ToString();

                        //cc = string.Concat(pemail1, aemail, demail, Uemail);
                        getcreateusermail(ReqNum, Vendor);
                        cc = string.Concat(demail, Session["createemail"].ToString());

                        //main mail
                        //getting Customer mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Customer_mail";

                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = ReqNum.ToString();
                            cmdget.Parameters.Add(vendorid);

                            int status1 = 2;
                            SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                            status.Direction = ParameterDirection.Input;
                            status.Value = Convert.ToInt32(status1.ToString());
                            cmdget.Parameters.Add(status);


                            SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                            Quote.Direction = ParameterDirection.Input;
                            Quote.Value = Session["quoteno"].ToString();
                            cmdget.Parameters.Add(Quote);

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlDataReader dr_cus;
                            dr_cus = cmdget.ExecuteReader();
                            pemail1 = string.Empty;
                            Session["customermail"] = "";
                            Session["customermail1"] = "";

                            while (dr_cus.Read())
                            {

                                customermail = dr_cus.GetString(0);
                                Session["customermail"] = dr_cus.GetString(0);
                                customermail1 = dr_cus.GetString(1);
                                Session["customermail1"] = dr_cus.GetString(1);
                                customermail = string.Concat(customermail, ";", customermail1);
                                //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                //while start

                                //email
                                // getting Messageheader ID from IT Mailapp
                                
                                using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    string returnValue1 = string.Empty;
                                    MHcnn.Open();
                                    SqlTransaction transaction;
                                    transaction = MHcnn.BeginTransaction("Reject");
                                    try
                                    {
                                        SqlCommand MHcmdget = MHcnn.CreateCommand();
                                        MHcmdget.Transaction = transaction;
                                        MHcmdget.CommandType = CommandType.StoredProcedure;
                                        MHcmdget.CommandText = "dbo.spGetControlNumber";

                                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                        CompanyCode.Direction = ParameterDirection.Input;
                                        CompanyCode.Value = 1;
                                        MHcmdget.Parameters.Add(CompanyCode);

                                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                        ControlField.Direction = ParameterDirection.Input;
                                        ControlField.Value = "MessageHeaderID";
                                        MHcmdget.Parameters.Add(ControlField);

                                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                        Param1.Direction = ParameterDirection.Input;
                                        Param1.Value = "";
                                        MHcmdget.Parameters.Add(Param1);

                                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                        Param2.Direction = ParameterDirection.Input;
                                        Param2.Value = "";
                                        MHcmdget.Parameters.Add(Param2);

                                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                        Param3.Direction = ParameterDirection.Input;
                                        Param3.Value = "";
                                        MHcmdget.Parameters.Add(Param3);

                                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                        Param4.Direction = ParameterDirection.Input;
                                        Param4.Value = "";
                                        MHcmdget.Parameters.Add(Param4);

                                        SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                        pOutput.Direction = ParameterDirection.Output;

                                        MHcmdget.ExecuteNonQuery();
                                        transaction.Commit();
                                        returnValue1 = pOutput.Value.ToString();

                                        OriginalFilename = returnValue1;
                                        MHid = returnValue1;
                                        Session["MHid1"] = returnValue1;
                                        OriginalFilename = MHid + seqNo + formatW;
                                    }
                                    catch (Exception xw)
                                    {
                                        transaction.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                        var script = string.Format("alert({0});", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        sendingemail = false;
                                    }
                                    MHcnn.Dispose();
                                }

                                Boolean IsAttachFile = true;
                                int SequenceNumber = 1;
                                string test = userId1;
                                IsAttachFile = false;
                                SendFilename = "NOFILE";
                                OriginalFilename = "NOFILE";
                                format = "NO";




                                //getting vendor mail content
                                using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    string returnValue_ = string.Empty;
                                    cnn_.Open();
                                    SqlCommand cmdget_ = cnn_.CreateCommand();
                                    cmdget_.CommandType = CommandType.StoredProcedure;
                                    cmdget_.CommandText = "dbo.Emet_Email_content";

                                    SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                    vendorid_.Direction = ParameterDirection.Input;
                                    vendorid_.Value = Session["quoteno"].ToString();
                                    cmdget_.Parameters.Add(vendorid_);

                                    SqlDataReader dr_;
                                    dr_ = cmdget_.ExecuteReader();
                                    while (dr_.Read())
                                    {
                                        body1 = dr_.GetString(1);
                                        Session["body1"] = dr_.GetString(1);
                                    }
                                    dr_.Dispose();
                                    cnn_.Dispose();
                                }


                                // Insert header and details to Mil server table to IT mailserverapp
                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    Email_inser.Open();
                                    //Header
                                    string MessageHeaderId = Session["MHid1"].ToString();
                                    string fromname = "eMET System";
                                    //string FromAddress = Session["Uemail"].ToString();
                                    string FromAddress = "eMET@Shimano.Com.sg";
                                    //string Recipient = aemail + "," + pemail;
                                    string Recipient = Session["customermail"].ToString();
                                    string CopyRecipient = cc;
                                    string BlindCopyRecipient = "";
                                    string ReplyTo = "subashdurai@shimano.com.sg";
                                    string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Approval By : " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;

                                    string transType = GetReqType(Session["quoteno"].ToString());
                                    string body = "";
                                    if (transType == "Revision")
                                    {
                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation <font color='red'> (Revision) </font> has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                    }
                                    else
                                    {
                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                    }
                                    string BodyFormat = "HTML";
                                    string BodyRemark = "0";
                                    string Signature = " ";
                                    string Importance = "High";
                                    string Sensitivity = "Confidential";

                                    string CreateUser = userId1;
                                    DateTime CreateDate = DateTime.Now;
                                    //end Header
                                    SqlTransaction transactionHe;
                                    transactionHe = Email_inser.BeginTransaction("Header");
                                    try
                                    {
                                        string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                        SqlCommand Header = new SqlCommand(Head, Email_inser);
                                        Header.Transaction = transactionHe;
                                        Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                        Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                        Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                        Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                        Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                        Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                        Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                        Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                        Header.Parameters.AddWithValue("@body", body.ToString());
                                        Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                        Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                        Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                        Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                        Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                        Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                        Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Header.CommandText = Head;
                                        Header.ExecuteNonQuery();

                                        transactionHe.Commit();
                                    }

                                    ///
                                    catch (Exception xw)
                                    {
                                        transactionHe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                        var script = string.Format("alert({0});", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        sendingemail = false;
                                    }
                                    //end Header
                                    //Details

                                    SqlTransaction transactionDe;
                                    transactionDe = Email_inser.BeginTransaction("Detail");
                                    try
                                    {
                                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                        Detail.Transaction = transactionDe;
                                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                        Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                        Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Detail.CommandText = Details;
                                        Detail.ExecuteNonQuery();
                                        transactionDe.Commit();
                                    }

                                    ///
                                    catch (Exception xw)
                                    {
                                        sendingemail = false;
                                        transactionDe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                        var script = string.Format("alert({0});", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }

                                    Email_inser.Dispose();
                                    //End Details
                                }

                                //while end

                            }
                            dr_cus.Dispose();
                            cnn.Dispose();
                        }


                        //End by subash

                        //end of email

                        sendingemail = true;
                    }
                    catch (Exception ex)
                    {
                        sendingemail = false;
                        MsgErr = ex.ToString();
                    }

                    if (sendingemail == false)
                    {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + MsgErr + " ");
                        var script = string.Format("alert({0});", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        ShowTable();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('failed sending email');", true);
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('failed sending email'); ", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Approved Successfully and Request Moved to next Level');", true);
                        ShowTable();
                        //Response.Redirect("Approval.aspx");
                    }
                    #endregion sending email

                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally {
                EmetCon.Dispose();
            }
        }
        
        //old code comment by subash
        protected void ShowTableDet(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct VendorCode1,substring((VendorName),1,12) +'...' as VendorName,QuoteNo,
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            CONVERT(nvarchar,
                            ROUND(
                            convert(float,
                            (
                            case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
                            else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
                            /convert(float,ISNULL(FinalQuotePrice,0))
                            )
                            *100)
                            ,1)
                            ) + '%' as 'NetProfit/Discount',
                            ApprovalStatus, PICApprovalStatus, 
                            case 
                            when PICApprovalStatus = 1 then 'Rejected'
                            when PICApprovalStatus = 2 then 'Approved'
                            else 'cannot find status'
                            end as 'ResponseStatus',
                            '" + RowParentGv + @"' as ParentGvRowNo, ManagerApprovalStatus,format(AprRejDateMng,'dd/MM/yyyy') as AprRejDateMng,
                            (select distinct UseNam from " + DbMasterName + @".dbo.Usr where UseID=AprRejByMng) as 'Updatedby',ManagerReason,ManagerRemark
                            from TQuoteDetails where ( RequestNumber = '" + RequestNumber + "' ) and (isMassRevision = 0 or isMassRevision is null) ";

                    if (DdlReqStatus.SelectedValue == "New")
                    {
                        sql += @" and QuoteNoRef is null and (isMassRevision = 0 or isMassRevision is null)";
                    }
                    else if (DdlReqStatus.SelectedValue == "Revision")
                    {
                        sql += @" and QuoteNoRef is not null";
                    }

                    if (DdlStatus.SelectedValue.ToString() == "Approved")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=2 and ManagerApprovalStatus = 0 ";
                    }
                    else if (DdlStatus.SelectedValue.ToString() == "Rejected")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=1 and ManagerApprovalStatus = 0 ";
                    }
                    else
                    {
                        sql += @" and RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) and ManagerApprovalStatus=0)  ";
                    }

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "QuoteNo")
                        {
                            sql += @" and QuoteNo like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode1")
                        {
                            sql += @" and VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorName")
                        {
                            sql += @" and VendorName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroup")
                        {
                            sql += @" and ProcessGroup like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = ProcessGroup) like '%'+@Filter+'%' ";
                        }
                    }

                    sql += @" order by QuoteNo desc ";
                    cmd = new SqlCommand(sql, EmetCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["TableDet"] = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void ShowTableMassRev()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,(select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            A.VendorCode1,substring((A.VendorName),1,12) +'...' as VendorName,A.QuoteNo,
                            CAST(ROUND(A.TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(A.TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(A.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(A.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(A.GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(A.FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            
                            CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost,
							CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
							CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5) ) as OldFinal,

                            ROUND(
							convert(float,(
							(
							 CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
							 (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
							  CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
							 )
							)/
							(
							CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
							CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
							)
							)*100)
							,1) 
							as Diffrence,

                            A.ApprovalStatus, A.PICApprovalStatus, 
                            case 
                            when A.PICApprovalStatus = 1 then 'Rejected'
                            when A.PICApprovalStatus = 2 then 'Approved'
                            else 'cannot find status'
                            end as 'ResponseStatus',
                            A.ManagerApprovalStatus,format(A.AprRejDateMng,'dd/MM/yyyy') as AprRejDateMng,
                            (select distinct UseNam from " + DbMasterName + @".dbo.Usr where UseID=A.AprRejByMng) as 'Updatedby',A.ManagerReason,A.ManagerRemark
                            from TQuoteDetails A 
                            where  A.Plant  = '" + Session["EPlant"].ToString() + "' and (A.isMassRevision = 1) ";

                    sql += @" and (A.Product in (select distinct S.Product from " + DbMasterName + @".[dbo].TSMNProductPIC S where S.Userid='" + Session["userID"].ToString() + "' and s.Plant='" + Session["EPlant"].ToString() + "' and ISNULL(DelFlag,0)=0 and s.System = 'eMET' )) ";

                    //sql += @" and (A.SMNPicDept = '" + Session["userDept"].ToString() + "') ";

                    if (DdlReqStatus.SelectedValue == "New")
                    {
                        sql += @" and A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null)";
                    }
                    else if (DdlReqStatus.SelectedValue == "Revision")
                    {
                        sql += @" and A.QuoteNoRef is not null";
                    }

                    if (DdlStatus.SelectedValue.ToString() == "Approved")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=2 and ManagerApprovalStatus = 0 ";
                    }
                    else if (DdlStatus.SelectedValue.ToString() == "Rejected")
                    {
                        sql += @" and approvalstatus =2 and PICApprovalStatus=1 and ManagerApprovalStatus = 0 ";
                    }
                    else
                    {
                        sql += @" and RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) and ManagerApprovalStatus=0)  ";
                    }

                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        if (DdlFltrDate.SelectedValue.ToString() == "RequestDate")
                        {
                            sql += @" and format(RequestDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                        else if (DdlFltrDate.SelectedValue.ToString() == "QuoteResponseDueDate")
                        {
                            sql += @" and format(QuoteResponseDueDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                    }

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "Plant")
                        {
                            sql += @" and Plant like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "RequestNumber")
                        {
                            sql += @" and RequestNumber like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Product")
                        {
                            sql += @" and Product like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Material")
                        {
                            sql += @" and Material like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "MaterialDesc")
                        {
                            sql += @" and MaterialDesc like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "QuoteNo")
                        {
                            sql += @" and QuoteNo like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode1")
                        {
                            sql += @" and VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorName")
                        {
                            sql += @" and VendorName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "CreatedBy")
                        {
                            sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "UseDep")
                        {
                            sql += @" and SMNPicDept like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroup")
                        {
                            sql += @" and ProcessGroup like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = ProcessGroup) like '%'+@Filter+'%' ";
                        }
                    }

                    #region filter by difference
                    if (DdlReqStatus.SelectedValue == "MassRevision")
                    {
                        if (DdlDiffrence.SelectedIndex > 0)
                        {
                            if (DdlDiffrence.SelectedValue == "><")
                            {
                                if (TxtDiffrence1.Text != "" && TxtDiffrence2.Text != "")
                                {
                                    sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) > @Diff1 and

                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) < @Diff2
                                        ) ";
                                }
                            }
                            else
                            {
                                if (TxtSglConditionDiff.Text != "")
                                {
                                    if (DdlDiffrence.SelectedValue == ">")
                                    {
                                        sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) > @Diff1 ) ";
                                    }
                                    else if (DdlDiffrence.SelectedValue == "<")
                                    {
                                        sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) < @Diff1 )";
                                    }
                                    else if (DdlDiffrence.SelectedValue == ">=")
                                    {
                                        sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) >= @Diff1 )";
                                    }
                                    else if (DdlDiffrence.SelectedValue == "<=")
                                    {
                                        sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) <= @Diff1 )";
                                    }
                                    else if (DdlDiffrence.SelectedValue == "=")
                                    {
                                        sql += @" and (
                                        ROUND(
                                        convert(float,(
                                        (
                                        CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) - 
                                        (CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) + 
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) + CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )/
                                        (
                                        CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) + CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) +
                                        CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) +CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) 
                                        )
                                        )*100)
                                        ,1) = @Diff1 ) ";
                                    }
                                }
                            }
                        }
                    }
                    #endregion Filter difference

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "RequestDate" || ViewState["SortExpression"].ToString() == "QuoteResponseDueDate")
                        {
                            sql += @" Order by CONVERT(DateTime, " + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                        }
                        else
                        {
                            sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                    }
                    else
                    {
                        sql += @" Order by RequestNumber desc ";
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        DateTime DtFrom = DateTime.ParseExact(TxtFrom.Text, "dd/MM/yyyy", null);
                        DateTime Dtto = DateTime.ParseExact(TxtTo.Text, "dd/MM/yyyy", null);

                        cmd.Parameters.AddWithValue("@From", DtFrom.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@To", Dtto.ToString("yyyy-MM-dd"));
                    }

                    if (DdlReqStatus.SelectedValue == "MassRevision")
                    {
                        if (DdlDiffrence.SelectedIndex > 0)
                        {
                            if (DdlDiffrence.SelectedValue == "><")
                            {
                                cmd.Parameters.AddWithValue("@Diff1", TxtDiffrence1.Text);
                                cmd.Parameters.AddWithValue("@Diff2", TxtDiffrence2.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Diff1", TxtSglConditionDiff.Text);
                            }
                        }
                    }

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        dt.Columns.Add("App");
                        dt.Columns.Add("Rej");
                        Session["DtMassRevDIR"] = dt;
                        GdvMassRev.DataSource = dt;
                        int ShowEntry = 1;
                        if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                        {
                            ShowEntry = 1;
                            TxtShowEntMassRev.Text = "1";
                        }
                        else
                        {
                            ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                        }
                        GdvMassRev.DataBind();
                        CheckAllAprOrRej();
                        LbTotalRecMassRev.Text = "Total Record : " + dt.Rows.Count;

                        if (dt.Rows.Count > 0)
                        {
                            BtnProceed.Visible = true;
                        }
                        else
                        {
                            BtnProceed.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool IsTeamShimano(string VendorCode)
        {
            bool IsTeamShimano = false;
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            
            try
            {
                MDMCon.Open();
                sql = @"select distinct VendorCode from TSBMPRICINGPOLICY where VendorCode = @VendorCode ";

                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    IsTeamShimano = true;
                }
            }
            catch (Exception ex)
            {
                IsTeamShimano = false;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
            return IsTeamShimano;
        }

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }

        protected void GetMainDataComp(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" declare @procGroup nvarchar(20),@Layout nvarchar(20) 
                        set @procGroup = (select top 1 A.ProcessGroup from TQuoteDetails A where RequestNumber=@RequestNumber); 
                        set @Layout = (select top 1 A.ScreenLayout from [" + DbMasterName + @"].[dbo].[TPROCESGRP_SCREENLAYOUT] A where A.ProcessGrp=@procGroup); 
                        select distinct A.RequestNumber,format(A.RequestDate,'dd/MM/yyyy') as 'RequestDate',A.Product,A.Material,A.MaterialDesc,format(A.QuoteResponseDueDate,'dd/MM/yyyy')as 'QuoteResponseDueDate',
                        A.ProcessGroup,@Layout as 'Layout', (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy' from TQuoteDetails A where RequestNumber=@RequestNumber ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            LbReqNo.Text = ": " + dt.Rows[0]["RequestNumber"].ToString();
                            LbReqDate.Text = ": " + dt.Rows[0]["RequestDate"].ToString();
                            LbProduct.Text = ": " + dt.Rows[0]["Product"].ToString();
                            LbMaterial.Text = ": " + dt.Rows[0]["Material"].ToString();
                            LbMatDesc.Text = ": " + dt.Rows[0]["MaterialDesc"].ToString();
                            LbQuoteResDuDate.Text = ": " + dt.Rows[0]["QuoteResponseDueDate"].ToString();
                            Txtlayout.Text = dt.Rows[0]["Layout"].ToString();
                            LbSMNPIC.Text = ": " + dt.Rows[0]["CreatedBy"].ToString();
                        }
                        else
                        {
                            LbReqNo.Text = ": ";
                            LbReqDate.Text = ": ";
                            LbProduct.Text = ": ";
                            LbMaterial.Text = ": ";
                            LbMatDesc.Text = ": ";
                            LbQuoteResDuDate.Text = ": ";
                            Txtlayout.Text = "";
                            LbSMNPIC.Text = ": ";
                        }
                    }
                }
                UpdatePanel2.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void GetDataSumarizeCost(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct A.QuoteNo,A.VendorName,
                    //        A.GrandTotalCost,A.FinalQuotePrice,
                    //        A.Profit,A.Discount,A.ActualNU,A.EffectiveDate,A.DueOn,
                    //        B.MaterialDescription,B.[RawMaterialCost/kg],B.[MaterialCost/pcs],A.TotalMaterialCost,
                    //        C.SubProcess,C.[ProcessCost/pc],A.TotalProcessCost,
                    //        D.[Sub-Mat/T&JDescription],D.[Sub-Mat/T&JCost/pcs],A.TotalSubMaterialCost,
                    //        E.ItemsDescription,E.[OtherItemCost/pcs],
                    //        A.TotalOtheritemsCost
                    //        from TQuoteDetails A
                    //        left join TMCCostDetails B on A.QuoteNo = B.QuoteNo 
                    //        left join TProcessCostDetails C on C.QuoteNo = C.QuoteNo 
                    //        left join TSMCCostDetails D on D.QuoteNo = D.QuoteNo
                    //        left join TOtherCostDetails E on E.QuoteNo = E.QuoteNo
                    //        where RequestNumber='1900006'
                    //        order by A.QuoteNo Asc ";

                    sql = @" select distinct QuoteNo,substring((VendorName),1,12) +'...' as VendorName,
                            (select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = TQ.PLANT and pp.porg = tvp.porg) inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = TQ.VENDORCODE1) as 'crcy',
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as 'FinalQuotePrice',
                            CONVERT(nvarchar,
							ROUND(
							convert(float,
							(
							case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
							else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
							/convert(float,ISNULL(FinalQuotePrice,0))
							)
							*100)
							,1)
							) + '%' as 'NetProfit/Discount'
                            ,ActualNU,
                            format(EffectiveDate,'dd/MM/yyyy') as EffectiveDate,format(DueOn,'dd/MM/yyyy') as 'DueOn',
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                            (SELECT SUM(CAST(ROUND([ProcessCost/pc], 6) AS DECIMAL(12,5))) FROM TProcessCostDetails where QuoteNo=TQ.QuoteNo) as 'TotProcOri',Profit,Discount,
                            CAST(ROUND(TotalProcessCost, 5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                            CAST(ROUND(TotalSubMaterialCost, 5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                            CAST(ROUND(TotalOtheritemsCost, 5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost'
                            from TQuoteDetails TQ
                            where RequestNumber=@RequestNumber
                            order by QuoteNo Desc ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvCompSumarizeQuote.DataSource = dt;
                            GvCompSumarizeQuote.DataBind();
                        }
                    }
                }
                //UpdatePanel8.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        /// <summary>
        /// Get Data material detail cost for table sumarize
        /// </summary>
        /// <param name="QuoteNo"></param>
        protected void GetDtTbSumCostMatDet(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"select QuoteNo,
                            (case when (select ScreenLayout from " + DbMasterName + @".[dbo].TPROCESGRP_SCREENLAYOUT where ProcessGrp=ProcessGroup) = 'Layout2' then MaterialSAPCode else MaterialDescription end) as 'MaterialDescription',
                            ProcessGroup,CAST(ROUND([RawMaterialCost/kg],7) AS DECIMAL(12,2)) as 'RawMaterialCost/kg',
                            CAST(ROUND([MaterialCost/pcs],7) AS DECIMAL(12,6)) as 'MaterialCost/pcs'  ,RowId
                            from TMCCostDetails
                            where QuoteNo=@QuoteNo order by RowId ASC";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["GvDetMatCostSum"] = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        /// <summary>
        /// Get Data Process detail cost for table sumarize
        /// </summary>
        /// <param name="QuoteNo"></param>
        protected void GetDtTbSumCostProcDet(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNo,SubProcess,CAST(ROUND([ProcessCost/pc],7) AS DECIMAL(12,6)) as 'ProcessCost/pc' ,RowId
                            from TProcessCostDetails  
                            where QuoteNo=@QuoteNo order by RowId ASC";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["GvDetprocCostSum"] = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        /// <summary>
        /// Get Data Sub Material detail cost for table sumarize
        /// </summary>
        /// <param name="QuoteNo"></param>
        protected void GetDtTbSumCostSubMatDet(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            GetDbMaster();
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct A.QuoteNo,
                    --[Sub-Mat/T&JDescription],
                    case 
                        when 
	                    (select count(*) MachineDescription from " + DbMasterName + @".dbo.TVENDORMACHNLIST M
	                    where MachineID = A.[Sub-Mat/T&JDescription] and tq.Plant = M.Plant and TQ.VendorCode1 = M.VendorCode) > 0 
	                    then CONCAT([Sub-Mat/T&JDescription],'-',
	                    (select distinct top 1 MachineDescription from " + DbMasterName + @".dbo.TVENDORMACHNLIST M
	                    where MachineID = A.[Sub-Mat/T&JDescription] and tq.Plant = M.Plant and TQ.VendorCode1 = M.VendorCode))

                        when 
	                    (select count(*) MachineDescription from " + DbMasterName + @".dbo.TToolAmortization M
	                    where Amortize_Tool_ID = A.[Sub-Mat/T&JDescription] and tq.Plant = M.Plant and TQ.VendorCode1 = M.VendorCode) > 0 
	                    then CONCAT([Sub-Mat/T&JDescription],'-',
	                    (select distinct top 1 Amortize_Tool_Desc from " + DbMasterName + @".dbo.TToolAmortization M
	                    where Amortize_Tool_ID = A.[Sub-Mat/T&JDescription] and tq.Plant = M.Plant and TQ.VendorCode1 = M.VendorCode))


	                    else [Sub-Mat/T&JDescription]
                    end as 'Sub-Mat/T&JDescription',

                    CAST(ROUND([Sub-Mat/T&JCost/pcs],7) AS DECIMAL(12,6)) as 'Sub-Mat/T&JCost/pcs',RowId
                    from TSMCCostDetails  A 
                    join TQuoteDetails TQ on A.QuoteNo = TQ.QuoteNo
                    where TQ.QuoteNo=@QuoteNo order by RowId ASC";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["GvDetSubMatCostSum"] = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        /// <summary>
        /// Get Data Sub Material detail cost for table sumarize
        /// </summary>
        /// <param name="QuoteNo"></param>
        protected void GetDtTbSumCostOthDet(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNo,UPPER(ItemsDescription) as 'ItemsDescription',CAST(ROUND([OtherItemCost/pcs],7) AS DECIMAL(12,6)) as 'OtherItemCost/pcs',RowId
                            from TOtherCostDetails 
                            where QuoteNo=@QuoteNo order by RowId ASC";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["GvDetOthCostSum"] = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void GetDataMatCost(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select B.VendorCode1,substring((B.VendorName),1,12) +'...' as VendorName,A.MaterialSAPCode,A.MaterialDescription,A.[RawMaterialCost/kg],A.[TotalRawMaterialCost/g],
                    //        A.[PartNetUnitWeight(g)],A.[~~Thickness(mm)],a.[~~Width(mm)],A.[~~Pitch(mm)],A.[~MaterialDensity],A.[~RunnerWeight/shot(g)],
                    //        A.[~RunnerRatio/pcs(%)],A.[~RecycleMaterialRatio(%)],A.Cavity,A.[MaterialYield/MeltingLoss(%)],A.[MaterialGrossWeight/pc(g)],
                    //        A.[MaterialScrapWeight(g)],[ScrapLossAllowance(%)],A.[ScrapPrice/kg],A.[ScrapRebate/pcs],A.[MaterialCost/pcs],A.[TotalMaterialCost/pcs]
                    //        from TMCCostDetails A 
                    //        right join TQuoteDetails B on A.QuoteNo = B.QuoteNo
                    //        where  B.RequestNumber = @RequestNumber order by A.QuoteNo asc ";


                    sql = @" declare @procGroup nvarchar(20),
                            @Layout nvarchar(20),
                            @queryGetCoulumn nvarchar(MAX),
                            @FullQuery nvarchar(MAX),
                            @FinalQry nvarchar(MAX),
                            @reqno nvarchar(MAX),
                            @DbMasterName nvarchar(MAX)
                            set @reqno = @RequestNumber
                            set @DbMasterName = @DbMaster

                            declare @ResultqueryGetCoulumn TABLE (ColumnResult nvarchar(MAX))
                            declare @TFinalQry TABLE (TFinalQry nvarchar(MAX))

                            set @procGroup = (select top 1 A.ProcessGroup from TQuoteDetails A where RequestNumber=@reqno);
                            set @Layout = (select top 1 A.ScreenLayout from [" + DbMasterName + @"].[dbo].[TPROCESGRP_SCREENLAYOUT] A where A.ProcessGrp=@procGroup);

                            set @queryGetCoulumn = ' 
                            (select stuff((select '','' + ''A.['' + column_name + '']'' from information_schema.columns 
                            where table_name=''TMCCostDetails'' 
                            and column_name in 
                            (select distinct REPLACE(A.FieldName,'' '','''') from [" + DbMasterName + @"].[dbo].[tMETFileds] A join [" + DbMasterName + @"].[dbo].[tMETFieldsCondition] B on A.FieldId = B.FieldId where A.FieldGroup=''MC'' and b.'+@Layout+' = 1)  
                            FOR XML PATH(''''), TYPE).value(''.'', ''NVARCHAR(MAX)''), 1, 1, '''')
                            AS [column_name])
                             ';
 
                             INSERT @ResultqueryGetCoulumn
                            exec (@queryGetCoulumn)

                            set @FullQuery = 'select B.VendorCode1,substring((B.VendorName),1,12) +''...'' as VendorName,(select distinct TVN.crcy from  '+ @DbMasterName +'.[dbo].tVendorPOrg TVP  inner join '+ @DbMasterName +'.dbo.tporgplant pp on (pp.plant = B.PLANT and pp.porg = tvp.porg) inner join '+ @DbMasterName +'.[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = B.VENDORCODE1) as crcy,'+(select ColumnResult from @ResultqueryGetCoulumn)+'from TMCCostDetails A 
                            right join TQuoteDetails B on A.QuoteNo = B.QuoteNo
                            where  B.RequestNumber = '''+ @reqno +''' order by A.QuoteNo asc';

                            INSERT into @TFinalQry (TFinalQry)  values (@FullQuery)
                            set @FinalQry = (select * from @TFinalQry);

                            exec (@FinalQry)
                             ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    GetDbMaster();
                    cmd.Parameters.AddWithValue("@DbMaster", DbMasterName);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvCmpDataMaterial.DataSource = dt;
                            GvCmpDataMaterial.DataBind();

                            #region span column
                            int RowSpan = 2;
                            int LastColumn = GvCmpDataMaterial.Rows[0].Cells.Count - 1;

                            for (int i = GvCmpDataMaterial.Rows.Count - 2; i >= 0; i--)
                            {
                                GridViewRow currRow = GvCmpDataMaterial.Rows[i];
                                GridViewRow prevRow = GvCmpDataMaterial.Rows[i + 1];

                                if (currRow.Cells[0].Text == prevRow.Cells[0].Text)
                                {
                                    currRow.Cells[0].RowSpan = RowSpan;
                                    prevRow.Cells[0].Visible = false;

                                    currRow.Cells[1].RowSpan = RowSpan;
                                    prevRow.Cells[1].Visible = false;

                                    currRow.Cells[2].RowSpan = RowSpan;
                                    prevRow.Cells[2].Visible = false;

                                    currRow.Cells[LastColumn].RowSpan = RowSpan;
                                    prevRow.Cells[LastColumn].Visible = false;

                                    RowSpan += 1;
                                }
                                else
                                {
                                    currRow.Cells[0].RowSpan = 1;
                                    RowSpan = 2;
                                }
                            }
                            #endregion span column

                            #region Table header
                            int ColumnCnt = GvCmpDataMaterial.Rows[0].Cells.Count - 1;
                            for (int c = 0; c <= ColumnCnt; c++)
                            {
                                if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("CountryOrg"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Currency";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialSAPCode"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material SAP Code";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialDescription"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Description";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("RawMaterialCost/kg"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Raw Material Cost/ kg";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("TotalRawMaterialCost/g"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Total Raw Material Cost/g";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("PartNetUnitWeight(g)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Part Net Unit Weight (g)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~~DiameterID(mm)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Diameter ID (mm)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~~DiameterOD(mm)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "DiameterOD(mm)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~~Thickness(mm)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Thickness (mm)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~~Width(mm)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Width (mm)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~~Pitch(mm)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Pitch (mm)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~MaterialDensity"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Density";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~RunnerWeight/shot(g)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Runner Weight/shot (g)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~RunnerRatio/pcs(%)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Runner Ratio/pc (%)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("~RecycleMaterialRatio(%)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Recycle Material Ratio (%)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("Cavity"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Base Qty / Cavity";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialYield/MeltingLoss(%)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Loss/Melting Loss (%)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialGrossWeight/pc(g)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Gross Weight/pc (g)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialScrapWeight(g)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Scrap Weight (g)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("ScrapLossAllowance(%)"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Scrap Loss Allowance (%)";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("ScrapPrice/kg"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Scrap Price/kg";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("ScrapRebate/pcs"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Scrap Rebate/pc";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("MaterialCost/pcs"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Material Cost/pc";
                                }
                                else if (GvCmpDataMaterial.Rows[0].Cells[c].Text.Contains("TotalMaterialCost/pcs"))
                                {
                                    GvCmpDataMaterial.Rows[0].Cells[c].Text = "Total Material Cost/pc";
                                }
                            }
                            #endregion Table header

                            #region table struktur
                            //if (Txtlayout.Text == "Layout1")
                            //{
                            //    GvCmpDataMaterial.Columns[2].Visible = true;
                            //    GvCmpDataMaterial.Columns[3].Visible = true;
                            //    GvCmpDataMaterial.Columns[4].Visible = true;
                            //    GvCmpDataMaterial.Columns[5].Visible = true;
                            //    GvCmpDataMaterial.Columns[6].Visible = true;

                            //    GvCmpDataMaterial.Columns[7].Visible = false;
                            //    GvCmpDataMaterial.Columns[8].Visible = false;
                            //    GvCmpDataMaterial.Columns[9].Visible = false;
                            //    GvCmpDataMaterial.Columns[10].Visible = false;

                            //    GvCmpDataMaterial.Columns[11].Visible = true;
                            //    GvCmpDataMaterial.Columns[12].Visible = true;
                            //    GvCmpDataMaterial.Columns[13].Visible = true;
                            //    GvCmpDataMaterial.Columns[14].Visible = true;
                            //    GvCmpDataMaterial.Columns[15].Visible = true;
                            //    GvCmpDataMaterial.Columns[16].Visible = true;

                            //    GvCmpDataMaterial.Columns[17].Visible = false;
                            //    GvCmpDataMaterial.Columns[18].Visible = false;
                            //    GvCmpDataMaterial.Columns[19].Visible = false;
                            //    GvCmpDataMaterial.Columns[20].Visible = false;
                            //}
                            //else if (Txtlayout.Text == "Layout2")
                            //{
                            //    GvCmpDataMaterial.Columns[2].Visible = true;

                            //    GvCmpDataMaterial.Columns[3].Visible = false;
                            //    GvCmpDataMaterial.Columns[4].Visible = false;
                            //    GvCmpDataMaterial.Columns[5].Visible = false;

                            //    GvCmpDataMaterial.Columns[6].Visible = true;

                            //    GvCmpDataMaterial.Columns[7].Visible = false;
                            //    GvCmpDataMaterial.Columns[8].Visible = false;
                            //    GvCmpDataMaterial.Columns[9].Visible = false;
                            //    GvCmpDataMaterial.Columns[10].Visible = false;
                            //    GvCmpDataMaterial.Columns[11].Visible = false;
                            //    GvCmpDataMaterial.Columns[12].Visible = false;
                            //    GvCmpDataMaterial.Columns[13].Visible = false;

                            //    GvCmpDataMaterial.Columns[14].Visible = true;

                            //    GvCmpDataMaterial.Columns[15].Visible = false;
                            //    GvCmpDataMaterial.Columns[16].Visible = false;
                            //    GvCmpDataMaterial.Columns[17].Visible = false;
                            //    GvCmpDataMaterial.Columns[18].Visible = false;
                            //    GvCmpDataMaterial.Columns[19].Visible = false;
                            //    GvCmpDataMaterial.Columns[20].Visible = false;
                            //}
                            //else if (Txtlayout.Text == "Layout3" || Txtlayout.Text == "Layout4")
                            //{
                            //    GvCmpDataMaterial.Columns[2].Visible = true;
                            //    GvCmpDataMaterial.Columns[3].Visible = true;
                            //    GvCmpDataMaterial.Columns[4].Visible = true;
                            //    GvCmpDataMaterial.Columns[5].Visible = true;
                            //    GvCmpDataMaterial.Columns[6].Visible = true;

                            //    GvCmpDataMaterial.Columns[7].Visible = false;
                            //    GvCmpDataMaterial.Columns[8].Visible = false;
                            //    GvCmpDataMaterial.Columns[9].Visible = false;
                            //    GvCmpDataMaterial.Columns[10].Visible = false;
                            //    GvCmpDataMaterial.Columns[11].Visible = false;
                            //    GvCmpDataMaterial.Columns[12].Visible = false;
                            //    GvCmpDataMaterial.Columns[13].Visible = false;

                            //    GvCmpDataMaterial.Columns[14].Visible = true;
                            //    GvCmpDataMaterial.Columns[15].Visible = true;
                            //    GvCmpDataMaterial.Columns[16].Visible = true;

                            //    GvCmpDataMaterial.Columns[17].Visible = false;
                            //    GvCmpDataMaterial.Columns[18].Visible = false;
                            //    GvCmpDataMaterial.Columns[19].Visible = false;
                            //    GvCmpDataMaterial.Columns[20].Visible = false;
                            //}
                            //else if (Txtlayout.Text == "Layout5")
                            //{
                            //    GvCmpDataMaterial.Columns[2].Visible = true;
                            //    GvCmpDataMaterial.Columns[3].Visible = true;
                            //    GvCmpDataMaterial.Columns[4].Visible = true;
                            //    GvCmpDataMaterial.Columns[5].Visible = true;
                            //    GvCmpDataMaterial.Columns[6].Visible = true;
                            //    GvCmpDataMaterial.Columns[7].Visible = true;
                            //    GvCmpDataMaterial.Columns[8].Visible = true;
                            //    GvCmpDataMaterial.Columns[9].Visible = true;
                            //    GvCmpDataMaterial.Columns[10].Visible = true;

                            //    GvCmpDataMaterial.Columns[11].Visible = false;
                            //    GvCmpDataMaterial.Columns[12].Visible = false;
                            //    GvCmpDataMaterial.Columns[13].Visible = false;

                            //    GvCmpDataMaterial.Columns[14].Visible = true;
                            //    GvCmpDataMaterial.Columns[15].Visible = true;
                            //    GvCmpDataMaterial.Columns[16].Visible = true;
                            //    GvCmpDataMaterial.Columns[17].Visible = true;
                            //    GvCmpDataMaterial.Columns[18].Visible = true;
                            //    GvCmpDataMaterial.Columns[19].Visible = true;
                            //    GvCmpDataMaterial.Columns[20].Visible = true;
                            //}
                            //else if (Txtlayout.Text == "Layout6")
                            //{
                            //    GvCmpDataMaterial.Columns[2].Visible = true;
                            //    GvCmpDataMaterial.Columns[3].Visible = true;
                            //    GvCmpDataMaterial.Columns[4].Visible = true;
                            //    GvCmpDataMaterial.Columns[5].Visible = true;
                            //    GvCmpDataMaterial.Columns[6].Visible = true;

                            //    GvCmpDataMaterial.Columns[7].Visible = false;
                            //    GvCmpDataMaterial.Columns[8].Visible = false;
                            //    GvCmpDataMaterial.Columns[9].Visible = false;
                            //    GvCmpDataMaterial.Columns[10].Visible = false;
                            //    GvCmpDataMaterial.Columns[11].Visible = false;
                            //    GvCmpDataMaterial.Columns[12].Visible = false;
                            //    GvCmpDataMaterial.Columns[13].Visible = false;

                            //    GvCmpDataMaterial.Columns[14].Visible = true;
                            //    GvCmpDataMaterial.Columns[15].Visible = true;
                            //    GvCmpDataMaterial.Columns[16].Visible = true;

                            //    GvCmpDataMaterial.Columns[17].Visible = false;
                            //    GvCmpDataMaterial.Columns[18].Visible = false;
                            //    GvCmpDataMaterial.Columns[19].Visible = false;
                            //    GvCmpDataMaterial.Columns[20].Visible = false;
                            //}
                            #endregion table struktur
                        }
                    }
                }
                //UpCmpMaterial.Update();
                //UpdatePanel7.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void GetDataProcCost(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select B.VendorCode1,substring((B.VendorName),1,12) +'...' as VendorName,(select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = B.PLANT and pp.porg = tvp.porg) inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = B.VENDORCODE1) as 'crcy',
                            A.ProcessGrpCode,A.SubProcess,
                            A.[IfTurnkey-VendorName] as 'IfSubcon-SubconName',
                            (select distinct TV.Description from " + DbMasterName + @".[dbo].tVendor_New TV where TV.Vendor=A.TurnKeySubVnd) as 'IfTurnkey-Subvendorname',A.[Machine/Labor],
                            A.Machine,CAST(ROUND(A.[StandardRate/HR],7) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(A.VendorRate,7) AS DECIMAL(12,2))as 'VendorRate',A.ProcessUOM,A.Baseqty,
                            A.[DurationperProcessUOM(Sec)],A.[Efficiency/ProcessYield(%)],
                            A.TurnKeyCost,A.TurnKeyProfit as 'TurnkeyFees',CAST(ROUND(A.[ProcessCost/pc],7) AS DECIMAL(12,6)) as 'ProcessCost/pc',CAST(ROUND(A.[TotalProcessesCost/pcs],5) AS DECIMAL(12,5)) as 'TotalProcessesCost/pcs' 
                            from TProcessCostDetails A 
                            right join TQuoteDetails B on A.QuoteNo = B.QuoteNo
                            where  B.RequestNumber = @RequestNumber order by A.QuoteNo asc ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvCmpDataProcCost.DataSource = dt;
                            GvCmpDataProcCost.DataBind();
                        }
                    }
                }
                //UpCmpDataProcCost.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void GetDataSubMatCost(string ReqNo)
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select B.VendorCode1,substring((B.VendorName),1,12) +'...' as VendorName,
                            (select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  
                            inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = B.PLANT and pp.porg = tvp.porg) 
                            inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) 
                            where  TVN.Vendor = B.VENDORCODE1) as 'crcy',
                            --UPPER(A.[Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',
                            case 
                                when 
	                            (select count(*) MachineDescription from " + DbMasterName + @".dbo.TVENDORMACHNLIST M
	                            where MachineID = A.[Sub-Mat/T&JDescription] and b.Plant = M.Plant and b.VendorCode1 = M.VendorCode) > 0 
	                            then CONCAT([Sub-Mat/T&JDescription],'-',
	                            (select distinct top 1 MachineDescription from " + DbMasterName + @".dbo.TVENDORMACHNLIST M
	                            where MachineID = A.[Sub-Mat/T&JDescription] and b.Plant = M.Plant and b.VendorCode1 = M.VendorCode))

                                when 
	                            (select count(*) MachineDescription from " + DbMasterName + @".dbo.TToolAmortization M
	                             where Amortize_Tool_ID = A.[Sub-Mat/T&JDescription] and B.Plant = M.Plant and B.VendorCode1 = M.VendorCode) > 0 
	                             then CONCAT([Sub-Mat/T&JDescription],'-',
	                             (select distinct top 1 Amortize_Tool_Desc from " + DbMasterName + @".dbo.TToolAmortization M
	                             where Amortize_Tool_ID = A.[Sub-Mat/T&JDescription] and B.Plant = M.Plant and B.VendorCode1 = M.VendorCode))


	                            else [Sub-Mat/T&JDescription]
                            end as 'Sub-Mat/T&JDescription',
                            A.[Sub-Mat/T&JCost],
                            A.[Consumption(pcs)],
                            CAST(ROUND(A.[Sub-Mat/T&JCost/pcs],7) AS DECIMAL(12,6)) as 'Sub-Mat/T&JCost/pcs',
                            CAST(ROUND(A.[TotalSub-Mat/T&JCost/pcs],5) AS DECIMAL(12,5)) as 'TotalSub-Mat/T&JCost/pcs'
                            from TSMCCostDetails A 
                            right join TQuoteDetails B on A.QuoteNo = B.QuoteNo
                            where  B.RequestNumber = @RequestNumber order by A.QuoteNo asc ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvCmpDataSubMatCost.DataSource = dt;
                            GvCmpDataSubMatCost.DataBind();
                        }
                    }
                }
                UpCmpDataSubMatCost.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void GetDataOthCost(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select B.VendorCode1,substring((B.VendorName),1,12) +'...' as VendorName,
                            (select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = B.PLANT and pp.porg = tvp.porg) inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = B.VENDORCODE1) as 'crcy',
                            UPPER(A.ItemsDescription) as 'ItemsDescription',
                            CAST(ROUND(A.[OtherItemCost/pcs],7) AS DECIMAL(12,6)) as 'OtherItemCost/pcs',
                            CAST(ROUND(A.[TotalOtherItemCost/pcs],5) AS DECIMAL(12,5)) as 'TotalOtherItemCost/pcs'
                            from TOtherCostDetails A 
                            right join TQuoteDetails B on A.QuoteNo = B.QuoteNo
                            where  B.RequestNumber = @RequestNumber order by A.QuoteNo asc ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvCmpDataOthCost.DataSource = dt;
                            GvCmpDataOthCost.DataBind();
                        }
                    }
                }
                UpCmpDataSubMatCost.Update();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected bool CheckStatusallVendor(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            bool Result = false;
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select * from TQuoteDetails where PIRStatus is null and QuoteNo=@QuoteNo";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Result = false;
                        }
                        else
                        {
                            Result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                Result = false;
            }
            finally
            {
                EmetCon.Dispose();
            }
            return Result;
        }

        public void getcreateusermail(string ReqNum, string Vendor)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                getcreateusermail1(ReqNum, Vendor);
                MDMCon.Open();
                DataTable Result111 = new DataTable();
                SqlDataAdapter da111 = new SqlDataAdapter();

                //string str11 = "select distinct UseEmail from usr where useid in(select distinct CreatedBy from TQuoteDetails where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "')";

                //da111 = new SqlDataAdapter(str11, con1);
                //Result111 = new DataTable();
                //da111.Fill(Result111);



                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("select distinct UseEmail from usr where useid = '" + Session["createemail1"].ToString() + "'", MDMCon);
                //    string str = " select MAX(REQUESTNO) from Tstatus ";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string useremail = dr[0].ToString();
                    Session["createemail"] = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally {
                MDMCon.Dispose();
            }
        }

        public void getcreateusermail1(string ReqNum, string Vendor)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {

                EmetCon.Open();
                DataTable Result111 = new DataTable();
                SqlDataAdapter da111 = new SqlDataAdapter();

                //string str11 = "select distinct UseEmail from usr where useid in(select distinct CreatedBy from TQuoteDetails where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "')";

                //da111 = new SqlDataAdapter(str11, con1);
                //Result111 = new DataTable();
                //da111.Fill(Result111);



                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct CreatedBy from TQuoteDetails where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                SqlCommand cmd = new SqlCommand(str, EmetCon);
                //    string str = " select MAX(REQUESTNO) from Tstatus ";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string useremail = dr[0].ToString();
                    Session["createemail1"] = dr[0].ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        string LastReason(string QuoteNo)
        {
            string LastReason = "";
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select DIRReason from TQuoteDetails where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            LastReason = dt.Rows[0]["DIRReason"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastReason = "";
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return LastReason;
        }

        public void AddRowSpanToGridViewProcCost()
        {
            try
            {
                #region span column
                int RowSpan = 2;

                for (int i = GvCmpDataProcCost.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GvCmpDataProcCost.Rows[i];
                    GridViewRow prevRow = GvCmpDataProcCost.Rows[i + 1];

                    if (currRow.Cells[0].Text == prevRow.Cells[0].Text)
                    {
                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[18].RowSpan = RowSpan;
                        prevRow.Cells[18].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        currRow.Cells[0].RowSpan = 1;
                        RowSpan = 2;
                    }
                }
                #endregion span column
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        public void AddRowSpanToGridViewSubMatCost()
        {
            try
            {
                #region span column
                int RowSpan = 2;

                for (int i = GvCmpDataSubMatCost.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GvCmpDataSubMatCost.Rows[i];
                    GridViewRow prevRow = GvCmpDataSubMatCost.Rows[i + 1];

                    if (currRow.Cells[0].Text == prevRow.Cells[0].Text)
                    {
                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[7].RowSpan = RowSpan;
                        prevRow.Cells[7].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        currRow.Cells[0].RowSpan = 1;
                        RowSpan = 2;
                    }
                }
                #endregion span column
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        public void AddRowSpanToGridViewOthCost()
        {
            try
            {
                #region span column
                int RowSpan = 2;

                for (int i = GvCmpDataOthCost.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GvCmpDataOthCost.Rows[i];
                    GridViewRow prevRow = GvCmpDataOthCost.Rows[i + 1];

                    if (currRow.Cells[0].Text == prevRow.Cells[0].Text)
                    {
                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[5].RowSpan = RowSpan;
                        prevRow.Cells[5].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        currRow.Cells[0].RowSpan = 1;
                        RowSpan = 2;
                    }
                }
                #endregion span column
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void QuoteDetForApproval(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct TQ.VendorCode1,substring((TQ.VendorName),1,12) +'...' as VendorName,TQ.QuoteNo,
                            (select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = TQ.PLANT and pp.porg = tvp.porg) inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = TQ.VENDORCODE1) as 'crcy',
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.TotalMaterialCost,5) AS DECIMAL(12,5))) else null end) as TotalMaterialCost,
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.TotalProcessCost,5) AS DECIMAL(12,5))) else null end) as TotalProcessCost,
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.TotalSubMaterialCost,5) AS DECIMAL(12,5))) else null end) as TotalSubMaterialCost,
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.TotalOtheritemsCost,5) AS DECIMAL(12,5))) else null end) as TotalOtheritemsCost,
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.GrandTotalCost,5) AS DECIMAL(12,5))) else null end) as GrandTotalCost,
                            (CASE when pirstatus='V' then (CAST(ROUND(TQ.FinalQuotePrice,5) AS DECIMAL(12,5))) else null end) as FinalQuotePrice,
                            (CASE when pirstatus='V' then (
                            CONVERT(nvarchar,
                            ROUND(
                            convert(float,
                            (
                            case when convert(float,isnull(TQ.FinalQuotePrice,0))-CONVERT(float,ISNULL(TQ.GrandTotalCost,0)) = 0 then null
                            else convert(float,isnull(TQ.FinalQuotePrice,0))-CONVERT(float,ISNULL(TQ.GrandTotalCost,0)) end
                            /convert(float,ISNULL(TQ.FinalQuotePrice,0))
                            )
                            *100)
                            ,1)
                            ) + '%'
                            ) else null end)  as 'NetProfit/Discount',
                            (case 
	                        when (select top 1 Z.NewEffectiveDate from TMngEffDateChgLog Z where RequestNumber = @RequestNumber and Z.QuoteNo = TQ.QuoteNo order by z.CreatedOn desc) is null then (CASE when pirstatus='V' then (format(TQ.EffectiveDate,'dd/MM/yyyy')) else null end) 
	                            else (select top 1 format(Z.NewEffectiveDate, 'dd/MM/yyyy') from TMngEffDateChgLog Z where RequestNumber = @RequestNumber and Z.QuoteNo = TQ.QuoteNo order by z.CreatedOn desc)
                            END) as 'EffectiveDate',
                            (case 
	                            when (select top 1 Z.NewDueOn from TMngEffDateChgLog Z where RequestNumber = @RequestNumber and Z.QuoteNo = TQ.QuoteNo order by z.CreatedOn desc) is null then (CASE when pirstatus='V' then (format(TQ.DueOn,'dd/MM/yyyy')) else null end) 
	                            else (select top 1 format(Z.NewDueOn, 'dd/MM/yyyy') from TMngEffDateChgLog Z where RequestNumber = @RequestNumber and Z.QuoteNo = TQ.QuoteNo order by z.CreatedOn desc)
                            END) as 'DueOn',
                            '" + RowParentGv + @"' as ParentGvRowNo,isnull(pirstatus,'U') as pirstatus,ApprovalStatus
                            from TQuoteDetails TQ where TQ.RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) and ManagerApprovalStatus=0)
                            and ( TQ.RequestNumber = '" + RequestNumber + "' ) ";

                    sql += @" order by QuoteNo desc ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", RequestNumber);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["QuoteDetForApproval"] = dt;
                        GvDecision.DataSource = dt;
                        GvDecision.DataBind();

                        GvDecCompr.DataSource = dt;
                        GvDecCompr.DataBind();

                        if (dt.Rows.Count > 0)
                        {
                            TxtTotRecord.Text = dt.Rows.Count.ToString();

                            for (int c = 0; c < dt.Rows.Count; c++)
                            {
                                if (IsTeamShimano(dt.Rows[c]["VendorCode1"].ToString()) == true)
                                {
                                    //e.Row.Cells[11].Text = "";
                                    GvDecision.Columns[11].Visible = false;
                                    GvDecCompr.Columns[11].Visible = false;
                                    TxtIsSBM.Text = "1";
                                    TxtIsSBMCmpr.Text = "1";
                                }
                                else
                                {
                                    GvDecision.Columns[11].Visible = true;
                                    GvDecCompr.Columns[11].Visible = true;
                                    if (dt.Rows[c]["NetProfit/Discount"].ToString() == "")
                                    {
                                        GvDecision.Rows[c].Cells[11].Text = "0.0 %";
                                        GvDecCompr.Rows[c].Cells[11].Text = "0.0 %";
                                    }
                                    else
                                    {
                                        GvDecision.Rows[c].Cells[11].Text = dt.Rows[c]["NetProfit/Discount"].ToString();
                                        GvDecCompr.Rows[c].Cells[11].Text = dt.Rows[c]["NetProfit/Discount"].ToString();
                                    }
                                    TxtIsSBM.Text = "0";
                                    TxtIsSBMCmpr.Text = "0";
                                }
                            }
                        }
                        else
                        {
                            TxtTotRecord.Text = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtTotRecord.Text = "0";
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        

        protected void LinkButton_Click(Object sender, CommandEventArgs e)
        {
            try
            {

                if (e.CommandArgument != null)
                {
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + e.CommandArgument.ToString());
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

        private void LnkApp_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;
                    GridView GvDet = e.Row.FindControl("GvDet") as GridView;
                    //string idCollege = grdview.DataKeys[e.Row.RowIndex].Value.ToString();
                    string iBranc = e.Row.Cells[3].Text;
                    ShowTableDet(iBranc, (RowParentGv + 1));
                    DataTable DtDetReqNo = new DataTable();
                    DtDetReqNo = (DataTable)Session["TableDet"];
                    GvDet.DataSource = DtDetReqNo;
                    GvDet.DataBind();

                    Button BtnCompare = e.Row.FindControl("BtnCompare") as Button;
                    Button BtnSubmit = e.Row.FindControl("BtnSubmit") as Button;
                    int count = 0;
                    for (int d = 0; d < DtDetReqNo.Rows.Count; d++)
                    {
                        string GrandTotalCost = DtDetReqNo.Rows[d]["GrandTotalCost"].ToString();
                        string FinalQuotePrice = DtDetReqNo.Rows[d]["FinalQuotePrice"].ToString();
                        if (GrandTotalCost.Trim() != "" && FinalQuotePrice.Trim() != "")
                        {
                            count++;
                        }
                    }

                    if (GvDet.Rows.Count <= 1)
                    {

                        BtnCompare.Enabled = false;
                    }
                    else if (count <= 1)
                    {
                        BtnCompare.Enabled = false;
                    }

                    #region TableDetailQuoteCondition
                    if (DtDetReqNo.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtDetReqNo.Rows.Count; i++)
                        {

                            //if (IsTeamShimano(DtDetReqNo.Rows[i]["VendorCode1"].ToString()) == true)
                            //{
                            //    GvDet.Rows[i].Cells[10].Text = "";
                            //}
                            //else
                            //{
                            //    if (DtDetReqNo.Rows[i]["NetProfit/Discount"].ToString() == "")
                            //    {
                            //        GvDet.Rows[i].Cells[10].Text = "0.0 %";
                            //    }
                            //}
                        }
                    }
                    #endregion

                    BtnSubmit.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                Session["DIRApprPgNo"] = (GridView1.PageIndex).ToString();
                ShowTable();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Compare")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string ReqNumber = row.Cells[3].Text;
                    TxtReqNumber.Text = ReqNumber;
                    TxtGvName.Text = "GvDecCompr";

                    GetMainDataComp(ReqNumber);
                    GetDataSumarizeCost(ReqNumber);
                    GetDataMatCost(ReqNumber);
                    GetDataProcCost(ReqNumber);
                    AddRowSpanToGridViewProcCost();
                    GetDataSubMatCost(ReqNumber);
                    AddRowSpanToGridViewSubMatCost();
                    GetDataOthCost(ReqNumber);
                    AddRowSpanToGridViewOthCost();
                    QuoteDetForApproval(ReqNumber, rowIndex);
                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "OpenModalCompare();freezeheader();Tabs();Layout7Condition();", true);
                    if (Session["DIRApprNst"] != null)
                    {
                        string RowVsStatus = Session["DIRApprNst"].ToString();
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                    }
                }
                else if (e.CommandName == "Submit")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string ReqNumber = row.Cells[3].Text;
                    TxtReqNumber.Text = ReqNumber;
                    TxtGvName.Text = "GvDecision";
                    QuoteDetForApproval(ReqNumber, rowIndex);
                    int TotRecord = Convert.ToInt32(TxtTotRecord.Text);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "OpenModalReasonRejection();freezeheader();CloseLoading();", true);
                    if (Session["DIRApprNst"] != null)
                    {
                        string RowVsStatus = Session["DIRApprNst"].ToString();
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                    }
                }
                else if (e.CommandName == "TrgNestedExpand")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Ex";
                    Session["DIRApprNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                }
                else if (e.CommandName == "TrgNestedColapse")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Colp";
                    Session["DIRApprNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (tc.HasControls())
                        {
                            LinkButton lb = (LinkButton)tc.Controls[0];
                            if (lb != null)
                            {
                                System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                {
                                    string sorting = ViewState["SortDirection"].ToString();
                                    icon.ImageUrl = "~/images/" + sorting + ".png";
                                    if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                    {
                                        lb.Attributes.Add("style", "text-decoration:none;");
                                        lb.ForeColor = System.Drawing.Color.Yellow;
                                        //tc.Controls.Add(new LiteralControl(" "));
                                        //tc.Controls.Add(icon);
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                        //icon.ImageUrl = "~/images/default.png";
                                        //tc.Controls.Add(new LiteralControl(" "));
                                        //tc.Controls.Add(icon);
                                    }
                                }
                                else
                                {
                                    lb.Attributes.Add("style", "text-decoration:underline;");
                                    //icon.ImageUrl = "~/images/default.png";
                                    //tc.Controls.Add(new LiteralControl(" "));
                                    //tc.Controls.Add(icon);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GetSortDirection(e.SortExpression);
                ShowTable();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvCompSumarizeQuote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false; // Invisibiling Year Header Cell
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    if (Txtlayout.Text == "Layout7")
                    {
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                    }
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                    e.Row.Cells[17].Visible = false;
                    e.Row.Cells[18].Visible = false;

                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;

                    //e.Row.Cells[3].Text = "Material Cost Detail";
                    //e.Row.Cells[5].Text = "Process Cost Detail";
                    //e.Row.Cells[10].Text = "Sub-Mat/T&J Cost Detail";
                    //e.Row.Cells[12].Text = "Others Items Cost Detail";
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;
                    GridView GvDetMatCost = e.Row.FindControl("GvDetMatCost") as GridView;
                    GridView GvDetProcCost = e.Row.FindControl("GvDetProcCost") as GridView;
                    GridView GvDetSubMatCost = e.Row.FindControl("GvDetSubMatCost") as GridView;
                    GridView GvDetOthCost = e.Row.FindControl("GvDetOthCost") as GridView;
                    string iBranc = e.Row.Cells[0].Text;

                    #region get data material cost det
                    GetDtTbSumCostMatDet(iBranc);
                    DataTable DtGvDetMatCost = new DataTable();
                    DtGvDetMatCost = (DataTable)Session["GvDetMatCostSum"];
                    GvDetMatCost.DataSource = DtGvDetMatCost;
                    GvDetMatCost.DataBind();
                    #endregion get data material cost det

                    if (Txtlayout.Text == "Layout7")
                    {
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                    }
                    else
                    {
                        #region get data Process cost det
                        GetDtTbSumCostProcDet(iBranc);
                        DataTable DtGvDetProcCost = new DataTable();
                        DtGvDetProcCost = (DataTable)Session["GvDetprocCostSum"];
                        GvDetProcCost.DataSource = DtGvDetProcCost;
                        GvDetProcCost.DataBind();
                        #endregion get data Process cost det

                        #region get data Sub Mat cost det
                        GetDtTbSumCostSubMatDet(iBranc);
                        DataTable DtGvDetSubMatCost = new DataTable();
                        DtGvDetSubMatCost = (DataTable)Session["GvDetSubMatCostSum"];
                        GvDetSubMatCost.DataSource = DtGvDetSubMatCost;
                        GvDetSubMatCost.DataBind();
                        #endregion get data Sub Mat det
                    }

                    #region get data Others cost det
                    GetDtTbSumCostOthDet(iBranc);
                    DataTable DtGvDetOthCost = new DataTable();
                    DtGvDetOthCost = (DataTable)Session["GvDetOthCostSum"];
                    GvDetOthCost.DataSource = DtGvDetOthCost;
                    GvDetOthCost.DataBind();
                    #endregion get data Others det

                    #region background column color
                    e.Row.Cells[3].BackColor = Color.FromName("#fff7e6");
                    e.Row.Cells[4].BackColor = Color.FromName("#fff7e6");

                    e.Row.Cells[5].BackColor = Color.FromName("#e6f7ff");
                    e.Row.Cells[6].BackColor = Color.FromName("#e6f7ff");
                    e.Row.Cells[7].BackColor = Color.FromName("#e6f7ff");
                    e.Row.Cells[8].BackColor = Color.FromName("#e6f7ff");
                    e.Row.Cells[9].BackColor = Color.FromName("#e6f7ff");

                    e.Row.Cells[10].BackColor = Color.FromName("#e6fff2");
                    e.Row.Cells[11].BackColor = Color.FromName("#e6fff2");

                    e.Row.Cells[12].BackColor = Color.FromName("#ffe6e6");
                    e.Row.Cells[13].BackColor = Color.FromName("#ffe6e6");

                    #endregion background column color

                    if ((e.Row.Cells[14].Text == e.Row.Cells[15].Text) && e.Row.Cells[14].Text.Trim() != "&nbsp;" && e.Row.Cells[15].Text.Trim() != "&nbsp;")
                    {
                        e.Row.Cells[16].Text = "0.0 %";
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvCompSumarizeQuote_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell HeaderCell = new TableCell();

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Quote No";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Vendor Name";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "VendorName";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "CURR";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Material Cost Detail";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Total Material Cost";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    if (Txtlayout.Text != "Layout7")
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "Process Cost Detail";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 1;
                        HeaderCell.RowSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "Actual Process Cost";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 1;
                        HeaderCell.RowSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "On Grand Total Cost";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 2;
                        HeaderCell.RowSpan = 1;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "Final Process Cost";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 1;
                        HeaderCell.RowSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "Sub-Mat/T&J Cost Detail";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 1;
                        HeaderCell.RowSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "Total Sub Material Cost";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.CssClass = "HeaderStyle";
                        HeaderCell.ColumnSpan = 1;
                        HeaderCell.RowSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);
                    }

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Others Items Cost Detail";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Total Other items Cost";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Grand Total Cost";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Final Quote Price";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Prof/Disc";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Effective Date";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Due Dt Next Rev";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    GvCompSumarizeQuote.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception)
            {

            }
        }

        protected void GvDetMatCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Txtlayout.Text == "Layout7")
                    {
                        if (e.Row.Cells[2].Text == "Material Cost/pcs")
                        {
                            e.Row.Cells[2].Text = "Material Cost/UOM";
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text.Replace("&nbsp;", " ").ToUpper();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvDetSubMatCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text.Replace("&nbsp;", " ").ToUpper();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            
        }

        protected void GvDetOthCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Txtlayout.Text == "Layout7")
                    {
                        if (e.Row.Cells[1].Text == "Item Cost/pcs")
                        {
                            e.Row.Cells[1].Text = "Item Cost/UOM";
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Text = e.Row.Cells[0].Text.Replace("&nbsp;", " ").ToUpper();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvCmpDataOthCost_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (Txtlayout.Text == "Layout7")
                    {
                        if (e.Row.Cells[4].Text == "Other Item Cost/pc")
                        {
                            e.Row.Cells[4].Text = "Other Item Cost/UOM";
                        }
                        if (e.Row.Cells[5].Text == "Total Other Item Cost/pc")
                        {
                            e.Row.Cells[5].Text = "Total Other Item Cost/UOM";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";
            try
            {

                // Retrieve the last column that was sorted.
                string sortExpression = ViewState["SortExpression"] as string;

                if (sortExpression != null)
                {
                    // Check if the same column is being sorted.
                    // Otherwise, the default value can be returned.
                    if (sortExpression == column)
                    {
                        string lastDirection = ViewState["SortDirection"] as string;
                        if ((lastDirection != null) && (lastDirection == "ASC"))
                        {
                            sortDirection = "DESC";
                        }
                    }
                }

                // Save new values in ViewState.
                ViewState["SortDirection"] = sortDirection;
                ViewState["SortExpression"] = column;

                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            return sortDirection;
        }

        protected void DdlFltrDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                TxtFrom.Text = "";
                TxtTo.Text = "";
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                string column = "";
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlDiffrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                string column = "";
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    if (DdlDiffrence.SelectedValue == "><")
                    {
                        DvSingleCondition.Visible = false;
                        DvMultiCondition.Visible = true;
                    }
                    else
                    {
                        DvSingleCondition.Visible = true;
                        DvMultiCondition.Visible = false;
                    }

                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                    DvFilterDiffrence.Visible = true;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                    DvFilterDiffrence.Visible = false;
                }
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlReqStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                string column = "";
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                    DvFilterDiffrence.Visible = true;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                    DvFilterDiffrence.Visible = false;
                }
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtFrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                txtFind.Focus();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
                if (DdlReqStatus.SelectedValue == "MassRevision")
                {
                    ShowTableMassRev();
                    DvMassRev.Visible = true;
                    DvNormalData.Visible = false;
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {

                DdlFltrDate.SelectedIndex = 0;
                DdlFilterBy.SelectedIndex = 0;
                DdlReqStatus.SelectedIndex = 0;
                TxtFrom.Text = "";
                TxtTo.Text = "";
                txtFind.Text = "";
                Session["DIRApprovalFilter"] = null;
                Session["DIRApprNst"] = null;
                Session["DtMassRev"] = null;
                DvMassRev.Visible = false;
                DvNormalData.Visible = true;

                DvFilterDiffrence.Visible = false;
                TxtSglConditionDiff.Text = "";
                TxtDiffrence1.Text = "";
                TxtDiffrence2.Text = "";
                DdlDiffrence.SelectedIndex = 0;

                ShowTable();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvlDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    //e.Row.Cells[10].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        int RowParentGv = e.Row.DataItemIndex;
                        Label LbQuoteNo = e.Row.FindControl("LbQuoteNo") as Label;
                        string url = "QQPReview.aspx?Number=" + LbQuoteNo.Text;
                        LbQuoteNo.Attributes.Add("onclick", "openInNewTab('" + url + "');");
                    }
                    catch (Exception ex)
                    {
                        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                        EMETModule.SendExcepToDB(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        //protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "LinktoRedirect")
        //    {
        //        string QuoteNo = ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString();
        //        bool Result = CheckStatusallVendor(QuoteNo);
        //        if (Result == true)
        //        {
        //            Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Waiting for Request completion from all vendors');CloseLoading();", true);
        //            if (Session["DIRApprNst"] != null)
        //            {
        //                string RowVsStatus = Session["DIRApprNst"].ToString();
        //                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
        //            }
        //        }
        //        //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
        //    }
        //    else
        //    {
        //        #region reference gridview row
        //        string[] CmdArg = (e.CommandArgument).ToString().Split('|');
        //        int TotRecord = Convert.ToInt32(LbTtlRecords.Text.Replace("Total Record :", ""));

        //        int rowIndexMain = Convert.ToInt32(CmdArg[0]) - 1;
        //        int rowIndexDet = Convert.ToInt32(CmdArg[1]);

        //        //4: reference from = total data can be show on 1 page -1 (because data index for row start from 0)
        //        if (rowIndexMain > 4)
        //        {
        //            //5: reference from pagination has been set from client side / total data can be show on 1 page
        //            while (rowIndexMain > 4)
        //            {
        //                rowIndexMain = rowIndexMain - 5;
        //            }
        //        }

        //        GridViewRow rowGvMain = GridView1.Rows[rowIndexMain];
        //        GridView GvDet = rowGvMain.FindControl("GvDet") as GridView;

        //        GridViewRow rowGvDet = GvDet.Rows[rowIndexDet];
        //        LinkButton Lb = rowGvDet.FindControl("LbQuoteNo") as LinkButton;

        //        string Reason = (rowGvDet.FindControl("txtReason") as TextBox).Text;
        //        string ReqNumber = rowGvMain.Cells[3].Text;
        //        string Vendor = rowGvDet.Cells[1].Text;
        //        string Quote = Lb.Text;
        //        #endregion reference gridview row

        //        if (e.CommandName == "Approve")
        //        {
        //            UpdateGridData(ReqNumber, Vendor, 2, Reason);
        //            ShowTable();
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        //            if (Session["DIRApprNst"] != null)
        //            {
        //                string RowVsStatus = Session["DIRApprNst"].ToString();
        //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
        //            }
        //        }
        //        else if (e.CommandName == "Reject")
        //        {
        //            string Rs = Reason.ToString().Replace(",", "").Trim();
        //            if (Reason.ToString() == "" || Rs == "")
        //            {
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        //                if (Session["DIRApprNst"] != null)
        //                {
        //                    string RowVsStatus = Session["DIRApprNst"].ToString();
        //                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please fill  the Reason to Reject');TriggerNested('" + RowVsStatus + "');", true);
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Please fill  the Reason to Reject')", true);
        //                }
        //                IsFirstLoad.Text = "1";
        //            }
        //            else
        //            {
        //                UpdateGridData(ReqNumber, Vendor, 1, Reason);
        //                ShowTable();
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        //                if (Session["DIRApprNst"] != null)
        //                {
        //                    string RowVsStatus = Session["DIRApprNst"].ToString();
        //                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
        //                }
        //            }
        //        }
        //    }
        //}

        protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                TxtReqNumber.Text = "";
                TxtVendor.Text = "";
                //disByHafis TxtReason.Text = "";
                if (e.CommandName == "LinktoRedirect")
                {
                    string QuoteNo = ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString();
                    bool Result = CheckStatusallVendor(QuoteNo);
                    if (Result == true)
                    {
                        Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Waiting for Request completion from all vendors');CloseLoading();", true);
                        if (Session["DIRApprNst"] != null)
                        {
                            string RowVsStatus = Session["DIRApprNst"].ToString();
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
                        }
                    }
                    //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                }
                else
                {
                    #region reference gridview row
                    //string[] CmdArg = (e.CommandArgument).ToString().Split('|');
                    //int TotRecord = Convert.ToInt32(LbTtlRecords.Text.Replace("Total Record :", ""));

                    //int rowIndexMain = Convert.ToInt32(CmdArg[0]) - 1;
                    //int rowIndexDet = Convert.ToInt32(CmdArg[1]);

                    ////4: reference from = total data can be show on 1 page -1 (because data index for row start from 0)
                    //if (rowIndexMain > 4)
                    //{
                    //    //5: reference from pagination has been set from client side / total data can be show on 1 page
                    //    while (rowIndexMain > 4)
                    //    {
                    //        rowIndexMain = rowIndexMain - 5;
                    //    }
                    //}

                    //GridViewRow rowGvMain = GridView1.Rows[rowIndexMain];
                    //GridView GvDet = rowGvMain.FindControl("GvDet") as GridView;

                    //GridViewRow rowGvDet = GvDet.Rows[rowIndexDet];
                    //LinkButton Lb = rowGvDet.FindControl("LbQuoteNo") as LinkButton;

                    ////string Reason = (rowGvDet.FindControl("txtReason") as TextBox).Text;
                    //string Reason = rowGvDet.Cells[12].Text.Replace("&nbsp;", "");
                    //string ReqNumber = rowGvMain.Cells[3].Text;
                    //string Vendor = rowGvDet.Cells[1].Text;
                    //string Quote = Lb.Text;
                    #endregion reference gridview row
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

        protected void GvDet_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    GridView GvDet = sender as GridView;
                    GridView HeaderGrid = (GridView)sender;
                    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    TableCell HeaderCell = new TableCell();

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "No.";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Vendor";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Quote No";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Total Cost";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 6;
                    HeaderGridRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "Net Prof/Disc";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderCell.ColumnSpan = 1;
                    //HeaderCell.RowSpan = 2;
                    //HeaderGridRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Manager";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 5;
                    HeaderGridRow.Cells.Add(HeaderCell);
                    

                    GvDet.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnCloseCompare_Click(object sender, EventArgs e)
        {
            try
            {

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }

                if (Session["DIRApprNst"] != null)
                {
                    string RowVsStatus = Session["DIRApprNst"].ToString();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvCmpDataMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Text = "Vendor Code";
                    e.Row.Cells[1].Text = "Vendor Name";
                    e.Row.Cells[2].Text = "Currency";
                    int coulmcnt = e.Row.Cells.Count - 1;
                    for (int i = 2; i <= coulmcnt; i++)
                    {
                        if (e.Row.Cells[i].Text.Contains("MaterialSAPCode"))
                        {
                            e.Row.Cells[i].Text = "Material SAP Code";
                        }
                        else if (e.Row.Cells[i].Text.Contains("MaterialDescription"))
                        {
                            e.Row.Cells[i].Text = "Material Description";
                        }
                        else if (e.Row.Cells[i].Text.Contains("RawMaterialCost/kg"))
                        {
                            e.Row.Cells[i].Text = "Raw Material Cost/ kg";
                        }
                        else if (e.Row.Cells[i].Text.Contains("TotalRawMaterialCost/g"))
                        {
                            e.Row.Cells[i].Text = "Total Raw Material Cost/g";
                        }
                        else if (e.Row.Cells[i].Text.Contains("PartNetUnitWeight(g)"))
                        {
                            e.Row.Cells[i].Text = "Part Net Unit Weight (g)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~~DiameterID(mm)"))
                        {
                            e.Row.Cells[i].Text = "Diameter ID (mm)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~~DiameterOD(mm)"))
                        {
                            e.Row.Cells[i].Text = "DiameterOD(mm)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~~Thickness(mm)"))
                        {
                            e.Row.Cells[i].Text = "Thickness (mm)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~~Width(mm)"))
                        {
                            e.Row.Cells[i].Text = "Width (mm)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~~Pitch(mm)"))
                        {
                            e.Row.Cells[i].Text = "Pitch (mm)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~MaterialDensity"))
                        {
                            e.Row.Cells[i].Text = "Material Density";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~RunnerWeight/shot(g)"))
                        {
                            e.Row.Cells[i].Text = "Runner Weight/shot (g)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~RunnerRatio/pcs(%)"))
                        {
                            e.Row.Cells[i].Text = "Runner Ratio/pc (%)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("~RecycleMaterialRatio(%)"))
                        {
                            e.Row.Cells[i].Text = "Recycle Material Ratio (%)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("Cavity"))
                        {
                            e.Row.Cells[i].Text = "Base Qty / Cavity";
                        }
                        else if (e.Row.Cells[i].Text.Contains("MaterialYield/MeltingLoss(%)"))
                        {
                            e.Row.Cells[i].Text = "Material Loss/Melting Loss (%)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("MaterialGrossWeight/pc(g)"))
                        {
                            e.Row.Cells[i].Text = "Material Gross Weight/pc (g)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("MaterialScrapWeight(g)"))
                        {
                            e.Row.Cells[i].Text = "Material Scrap Weight (g)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("ScrapLossAllowance(%)"))
                        {
                            e.Row.Cells[i].Text = "Scrap Loss Allowance (%)";
                        }
                        else if (e.Row.Cells[i].Text.Contains("ScrapPrice/kg"))
                        {
                            e.Row.Cells[i].Text = "Scrap Price/kg";
                        }
                        else if (e.Row.Cells[i].Text.Contains("ScrapRebate/pcs"))
                        {
                            e.Row.Cells[i].Text = "Scrap Rebate/pc";
                        }

                        if (e.Row.Cells[i].Text == "MaterialCost/pcs")
                        {
                            e.Row.Cells[i].Text = "Material Cost/pc";
                        }
                        else if (e.Row.Cells[i].Text == "TotalMaterialCost/pcs")
                        {
                            e.Row.Cells[i].Text = "Total Material Cost/pc";
                        }

                        if (e.Row.Cells[i].Text.Contains("/pc"))
                        {
                            if (Txtlayout.Text == "Layout7")
                            {
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Replace("/pc", "/UOM");
                            }
                        }
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    decimal result;
                    int coulmcnt = e.Row.Cells.Count;

                    for (int z = 0; z < coulmcnt; z++)
                    {
                        if (z >= 3)
                        {
                            if (z == 3)
                            {
                                e.Row.Cells[z].HorizontalAlign = HorizontalAlign.Left;
                                e.Row.Cells[z].Text = e.Row.Cells[z].Text.Replace("&nbsp;", " ").ToUpper();
                            }
                            else
                            {
                                if (decimal.TryParse(e.Row.Cells[z].Text, out result))
                                {
                                    // The string was a valid integer => use result here
                                    e.Row.Cells[z].HorizontalAlign = HorizontalAlign.Right;
                                    if (z == (coulmcnt - 1))
                                    {
                                        float finalvalue = float.Parse(e.Row.Cells[z].Text);
                                        e.Row.Cells[z].Text = string.Format("{0:0.00000}", finalvalue);
                                    }
                                    else if (z == (coulmcnt - 2))
                                    {
                                        float finalvalue = float.Parse(e.Row.Cells[z].Text);
                                        e.Row.Cells[z].Text = string.Format("{0:0.000000}", finalvalue);
                                    }
                                    else
                                    {
                                        string[] Value = e.Row.Cells[z].Text.Split('.');
                                        if (Value.Length > 1)
                                        {
                                            string Val = Value[1].ToString();
                                            if (Val.Length >= 6)
                                            {
                                                float finalvalue = float.Parse(e.Row.Cells[z].Text);
                                                e.Row.Cells[z].Text = string.Format("{0:0.00000}", finalvalue);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    e.Row.Cells[z].HorizontalAlign = HorizontalAlign.Left;
                                    e.Row.Cells[z].Text = e.Row.Cells[z].Text.Replace("&nbsp;", " ").ToUpper();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        protected void GvDecision_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].ColumnSpan = 3;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                    
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;

                    RadioButton RbApprove = e.Row.FindControl("RbApprove") as RadioButton;
                    RbApprove.Attributes.Add("onchange", "ApprOrRejectCondition(" + RowParentGv + ")");

                    RadioButton RbReject = e.Row.FindControl("RbReject") as RadioButton;
                    RbReject.Attributes.Add("onchange", "ApprOrRejectCondition(" + RowParentGv + ")");

                    TextBox TxtreasonApproval = e.Row.FindControl("TxtreasonApproval") as TextBox;
                    Label LblengtVC = e.Row.FindControl("LblengtVC") as Label;
                    string TxtreasonApprovalId = ((TextBox)e.Row.FindControl("TxtreasonApproval")).ClientID;
                    TxtreasonApproval.Attributes.Add("onkeyup", "ApprReason(" + RowParentGv + ")");

                    DropDownList DdlReject = e.Row.FindControl("DdlReject") as DropDownList;
                    TextBox TxtRejOth = e.Row.FindControl("TxtRejOth") as TextBox;
                    Label LblengtVCRej = e.Row.FindControl("LblengtVCRej") as Label;
                    string DdlRejectId = ((DropDownList)e.Row.FindControl("DdlReject")).ClientID;
                    string TxtRejOthId = ((TextBox)e.Row.FindControl("TxtRejOth")).ClientID;
                    string LblengtVCRejId = ((Label)e.Row.FindControl("LblengtVCRej")).ClientID;
                    TxtRejOth.Attributes.Add("Style", "display:none;");
                    TxtRejOth.Attributes.Add("onkeyup", "RemarkLght('" + TxtRejOthId + "','" + LblengtVCRejId + "')");
                    LblengtVCRej.Attributes.Add("Style", "display:none;");
                    DdlReject.Attributes.Add("onchange", "DdlRejChange('" + DdlRejectId + "','" + TxtRejOthId + "','" + LblengtVCRejId + "')");

                    DataTable QuoteDetForApproval = (DataTable)Session["QuoteDetForApproval"];
                    DataTable DtDdlReasonReject = (DataTable)Session["DdlReasonReject"];

                    if (QuoteDetForApproval != null)
                    {
                        if (QuoteDetForApproval.Rows[RowParentGv]["FinalQuotePrice"].ToString() == "" && QuoteDetForApproval.Rows[RowParentGv]["ApprovalStatus"].ToString() == "2")
                        {
                            if (DtDdlReasonReject != null)
                            {
                                DdlReject.Items.Add("-- Select Reason For Rejection --");
                                for (int i = 0; i < DtDdlReasonReject.Rows.Count; i++)
                                {
                                    if (DtDdlReasonReject.Rows[i][0].ToString().ToUpper() == "VENDOR DID NOT REPLY")
                                    {
                                        DdlReject.Items.Add(DtDdlReasonReject.Rows[i][0].ToString().ToUpper());
                                    }
                                }
                                DdlReject.Items.Add(new ListItem("Others", "Others"));
                                DdlReject.SelectedIndex = 1;
                            }
                            RbApprove.Enabled = false;
                            RbReject.Enabled = false;
                            RbReject.Checked = true;
                            DdlReject.Enabled = false;
                            DdlReject.Attributes.Add("style", "display:block;");
                            TxtreasonApproval.Attributes.Add("style", "display:none;");
                            LblengtVC.Attributes.Add("style", "display:none;");
                        }
                        else
                        {
                            if (RbApprove.Checked == false && RbReject.Checked == false)
                            {
                                TxtreasonApproval.Attributes.Add("disabled", "disabled");
                                DdlReject.Attributes.Add("style", "display:none;");
                            }

                            if (DtDdlReasonReject != null)
                            {
                                DdlReject.Items.Add("-- Select Reason For Rejection --");
                                for (int i = 0; i < DtDdlReasonReject.Rows.Count; i++)
                                {
                                    if (DtDdlReasonReject.Rows[i][0].ToString().ToUpper() != "VENDOR DID NOT REPLY")
                                    {
                                        DdlReject.Items.Add(DtDdlReasonReject.Rows[i][0].ToString().ToUpper());
                                    }
                                }
                                DdlReject.Items.Add(new ListItem("Others", "Others"));
                            }
                        }
                    }

                    if (IsTeamShimano(e.Row.Cells[2].Text) == true)
                    {
                        e.Row.Cells[11].Text = "";
                        GvDecision.Columns[11].Visible = false;
                    }
                    else
                    {
                        GvDecision.Columns[11].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvDecCompr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].ColumnSpan = 3;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[16].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;

                    RadioButton RbApprove = e.Row.FindControl("RbApprove") as RadioButton;
                    RbApprove.Attributes.Add("onchange", "ApprOrRejectCondition(" + RowParentGv + ")");

                    RadioButton RbReject = e.Row.FindControl("RbReject") as RadioButton;
                    RbReject.Attributes.Add("onchange", "ApprOrRejectCondition(" + RowParentGv + ")");

                    TextBox TxtreasonApproval = e.Row.FindControl("TxtreasonApproval") as TextBox;
                    Label LblengtVC = e.Row.FindControl("LblengtVC") as Label;
                    string TxtreasonApprovalId = ((TextBox)e.Row.FindControl("TxtreasonApproval")).ClientID;
                    TxtreasonApproval.Attributes.Add("onkeyup", "ApprReason(" + RowParentGv + ")");

                    DropDownList DdlReject = e.Row.FindControl("DdlReject") as DropDownList;
                    TextBox TxtRejOth = e.Row.FindControl("TxtRejOth") as TextBox;
                    Label LblengtVCRej = e.Row.FindControl("LblengtVCRej") as Label;
                    string DdlRejectId = ((DropDownList)e.Row.FindControl("DdlReject")).ClientID;
                    string TxtRejOthId = ((TextBox)e.Row.FindControl("TxtRejOth")).ClientID;
                    string LblengtVCRejId = ((Label)e.Row.FindControl("LblengtVCRej")).ClientID;
                    TxtRejOth.Attributes.Add("Style", "display:none;");
                    TxtRejOth.Attributes.Add("onkeyup", "RemarkLght('" + TxtRejOthId + "','" + LblengtVCRejId + "')");
                    LblengtVCRej.Attributes.Add("Style", "display:none;");
                    DdlReject.Attributes.Add("onchange", "DdlRejChange('" + DdlRejectId + "','" + TxtRejOthId + "','" + LblengtVCRejId + "')");

                    DataTable QuoteDetForApproval = (DataTable)Session["QuoteDetForApproval"];
                    DataTable DtDdlReasonReject = (DataTable)Session["DdlReasonReject"];

                    if (QuoteDetForApproval != null)
                    {
                        if (QuoteDetForApproval.Rows[RowParentGv]["FinalQuotePrice"].ToString() == "" && QuoteDetForApproval.Rows[RowParentGv]["ApprovalStatus"].ToString() == "2")
                        {
                            if (DtDdlReasonReject != null)
                            {
                                DdlReject.Items.Add("-- Select Reason For Rejection --");
                                for (int i = 0; i < DtDdlReasonReject.Rows.Count; i++)
                                {
                                    if (DtDdlReasonReject.Rows[i][0].ToString().ToUpper() == "VENDOR DID NOT REPLY")
                                    {
                                        DdlReject.Items.Add(DtDdlReasonReject.Rows[i][0].ToString().ToUpper());
                                    }
                                }
                                DdlReject.Items.Add(new ListItem("Others", "Others"));
                                DdlReject.SelectedIndex = 1;
                            }
                            RbApprove.Enabled = false;
                            RbReject.Enabled = false;
                            RbReject.Checked = true;
                            DdlReject.Enabled = false;
                            DdlReject.Attributes.Add("style", "display:block;");
                            TxtreasonApproval.Attributes.Add("style", "display:none;");
                            LblengtVC.Attributes.Add("style", "display:none;");
                        }
                        else
                        {
                            if (RbApprove.Checked == false && RbReject.Checked == false)
                            {
                                TxtreasonApproval.Attributes.Add("disabled", "disabled");
                                DdlReject.Attributes.Add("style", "display:none;");
                            }

                            if (DtDdlReasonReject != null)
                            {
                                DdlReject.Items.Add("-- Select Reason For Rejection --");
                                for (int i = 0; i < DtDdlReasonReject.Rows.Count; i++)
                                {
                                    if (DtDdlReasonReject.Rows[i][0].ToString().ToUpper() != "VENDOR DID NOT REPLY")
                                    {
                                        DdlReject.Items.Add(DtDdlReasonReject.Rows[i][0].ToString().ToUpper());
                                    }
                                }
                                DdlReject.Items.Add(new ListItem("Others", "Others"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void CtnCloseMdl_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void StartTimer_Click(object sender, EventArgs e)
        {
            TimerCntDown.Enabled = true;
        }

        protected void TimerCntDown_Tick(object sender, EventArgs e)
        {
            if (TimerCntDown.Enabled == true)
            {
                int seconds = Int32.Parse((countdown.Text));
                seconds--;

                if (seconds < 0)
                {
                    TimerCntDown.Enabled = false;
                    Session.Abandon();
                    Session.Clear();
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    countdown.Text = seconds.ToString();
                }
            }
        }

        protected void BtnCekLatestNested_Click(object sender, EventArgs e)
        {
            IsFirstLoad.Text = "2";
            if (Session["DIRApprNst"] != null)
            {
                string RowVsStatus = Session["DIRApprNst"].ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
            }
        }

        bool ProcessSubmitApproval()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();
                
                int i = Convert.ToInt32(TxtTotRecord.Text);
                string ReqNumber = TxtReqNumber.Text;
                string GridViewName = TxtGvName.Text;
                sql = "";

                for (int a = 0; a < i; a++)
                {
                    RadioButton RbApprove = null;
                    RadioButton RbReject = null;
                    TextBox TxtreasonApproval = null;
                    DropDownList DdlReject = null;
                    TextBox TxtRejOth = null;

                    string QuNo = string.Empty;

                    if (GridViewName == "GvDecision")
                    {
                        RbApprove = GvDecision.Rows[a].FindControl("RbApprove") as RadioButton;
                        RbReject = GvDecision.Rows[a].FindControl("RbReject") as RadioButton;
                        TxtreasonApproval = GvDecision.Rows[a].FindControl("TxtreasonApproval") as TextBox;
                        DdlReject = GvDecision.Rows[a].FindControl("DdlReject") as DropDownList;
                        TxtRejOth = GvDecision.Rows[a].FindControl("TxtRejOth") as TextBox;
                        QuNo = GvDecision.Rows[a].Cells[1].Text;
                    }
                    else
                    {
                        RbApprove = GvDecCompr.Rows[a].FindControl("RbApprove") as RadioButton;
                        RbReject = GvDecCompr.Rows[a].FindControl("RbReject") as RadioButton;
                        TxtreasonApproval = GvDecCompr.Rows[a].FindControl("TxtreasonApproval") as TextBox;
                        DdlReject = GvDecCompr.Rows[a].FindControl("DdlReject") as DropDownList;
                        TxtRejOth = GvDecCompr.Rows[a].FindControl("TxtRejOth") as TextBox;
                        QuNo = GvDecCompr.Rows[a].Cells[1].Text;
                    }

                    string ApprReason = "";
                    string RejReason = "";
                    string RejRemark = "";
                    if (RbApprove.Checked == true)
                    {
                        ApprReason = TxtreasonApproval.Text;
                        //if (ApprReason.ToString().Trim() != "")
                        //{
                        //    ApprReason = " Apr: " + ApprReason + " - By: " + Session["UserName"].ToString();
                        //}
                    }
                    else
                    {
                        RejReason = DdlReject.SelectedItem.ToString();
                        RejRemark = TxtRejOth.Text;
                        //if (RejReason.ToString().Trim() != "")
                        //{
                        //    RejReason = " Reason: " + RejReason + " - By: " + Session["UserName"].ToString();
                        //}
                    }
                    

                    if (RbApprove.Checked == false)
                    {
                        if (DdlReject.SelectedValue.ToString() == "Others")
                        {
                            sql = @" Update TQuoteDetails SET ApprovalStatus='" + 1 + @"', ManagerApprovalStatus= '" + 1 + @"',  
                                DIRApprovalStatus = '" + 0 + "',DIRRemark = '" + RejRemark + @"',
                                AprRejBy = '" + Session["userID"].ToString() + @"', AprRejDate = CURRENT_TIMESTAMP
                                where RequestNumber = '" + ReqNumber + "'  and QuoteNo ='" + QuNo + "' ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sql = @" Update TQuoteDetails SET ApprovalStatus='" + 1 + @"', ManagerApprovalStatus= '" + 1 + @"',  
                                DIRApprovalStatus = '" + 0 + "',DIRReason = '" + RejReason + @"',
                                AprRejBy = '" + Session["userID"].ToString() + @"', AprRejDate = CURRENT_TIMESTAMP
                                where RequestNumber = '" + ReqNumber + "'  and QuoteNo ='" + QuNo + "' ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        GetDbMaster();
                        if (IsUseToolAmortize(QuNo) == true) {
                            sql = @" UPDATE "+  DbMasterName + @".dbo.TToolAmortization
                                    SET 
                                    " + DbMasterName + @".dbo.TToolAmortization.EffectiveFrom = TQ.EffectiveDate,
                                    " + DbMasterName + @".dbo.TToolAmortization.QuoteRefNo = TQ.QuoteNo
                                    FROM 
                                    TQuoteDetails TQ 
                                    join TToolAmortization ETT on TQ.QuoteNo = ETT.QuoteNo
                                    JOIN " + DbMasterName + @".dbo.TToolAmortization MTT
                                    ON 
                                    TQ.Plant=MTT.Plant 
                                    and TQ.VendorCode1=MTT.VendorCode 
                                    and ETT.Process_Grp_code=MTT.Process_Grp_code
                                    and ETT.ToolTypeID=MTT.ToolTypeID
                                    and ETT.Amortize_Tool_ID=MTT.Amortize_Tool_ID
                                    and ETT.Amortize_Tool_ID=MTT.Amortize_Tool_ID 
                                    where TQ.QuoteNo = @QuoteNo";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();

                            sql = @" declare @PeriodUOM nvarchar(50) = (select distinct AmortizePeriodUOM from " + DbMasterName + @".dbo.TToolAmortization where QuoteRefNo = @QuoteNo);
                            declare @Period int = (select distinct isnull(AmortizePeriod,0) from " + DbMasterName + @".dbo.TToolAmortization where QuoteRefNo = @QuoteNo);
                            if @PeriodUOM = 'DAY'
                            begin
	                            update " + DbMasterName + @".dbo.TToolAmortization set DueDate = DATEADD(DAY,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'WEEK'
                            begin
	                            update " + DbMasterName + @".dbo.TToolAmortization set DueDate = DATEADD(WEEK,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'MONTH'
                            begin
	                            update " + DbMasterName + @".dbo.TToolAmortization set DueDate = DATEADD(MONTH,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'YEAR'
                            begin 
	                            update " + DbMasterName + @".dbo.TToolAmortization set DueDate = DATEADD(YEAR,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();


                            sql = @" update TToolAmortization set QuoteApproveddate = CURRENT_TIMESTAMP 
                            where QuoteNo = @QuoteNo ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();
                        }

                        if (IsUseMachineAmortize(QuNo) == true)
                        {
                            sql = @" UPDATE " + DbMasterName + @".dbo.TMachineAmortization
                            SET 
                            " + DbMasterName + @".dbo.TMachineAmortization.EffectiveFrom = TQ.EffectiveDate,
                            " + DbMasterName + @".dbo.TMachineAmortization.QuoteRefNo = TQ.QuoteNo
                            FROM 
                            TQuoteDetails TQ 
                            join TMachineAmortization EMA on TQ.QuoteNo = EMA.QuoteNo
                            JOIN " + DbMasterName + @".dbo.TMachineAmortization MMA
                            ON 
                            TQ.Plant=MMA.Plant 
                            and TQ.VendorCode1=MMA.VendorCode 
                            and EMA.Process_Grp_code=MMA.Process_Grp_code
                            and EMA.Vend_MachineID=MMA.Vend_MachineID
                            where TQ.QuoteNo = @QuoteNo";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();

                            sql = @" declare @PeriodUOM nvarchar(50) = (select distinct AmortizePeriodUOM from " + DbMasterName + @".dbo.TMachineAmortization where QuoteRefNo = @QuoteNo );
                            declare @Period int = (select distinct isnull(AmortizePeriod,0) from " + DbMasterName + @".dbo.TMachineAmortization where QuoteRefNo = @QuoteNo );
                            if @PeriodUOM = 'DAY'
                            begin
	                            update " + DbMasterName + @".dbo.TMachineAmortization set DueDate = DATEADD(DAY,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'WEEK'
                            begin
	                            update " + DbMasterName + @".dbo.TMachineAmortization set DueDate = DATEADD(WEEK,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'MONTH'
                            begin
	                            update " + DbMasterName + @".dbo.TMachineAmortization set DueDate = DATEADD(MONTH,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end
                            else if @PeriodUOM = 'YEAR'
                            begin 
	                            update " + DbMasterName + @".dbo.TMachineAmortization set DueDate = DATEADD(YEAR,@Period, EffectiveFrom) where QuoteRefNo=@QuoteNo
                            end ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();

                            sql = @" update TMachineAmortization set QuoteApproveddate = CURRENT_TIMESTAMP 
                            where QuoteNo = @QuoteNo ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                            cmd.ExecuteNonQuery();
                        }

                        sql = @" Update TQuoteDetails SET ApprovalStatus='" + 3 + "', ManagerApprovalStatus= '" + 2 + "',  DIRRemark = '" + ApprReason + @"',
                              DIRApprovalStatus = '" + 0 + "',  AprRejBy = '" + Session["userID"].ToString() + @"', AprRejDate = CURRENT_TIMESTAMP
                               where RequestNumber = '" + ReqNumber + "'  and QuoteNo ='" + QuNo + "' ";
                        cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                        cmd.ExecuteNonQuery();
                    }
                }

                EmetTrans.Commit();
                EmetTrans.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                EmetTrans.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString();
                DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool IsUseToolAmortize(string QuoteNo) {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter()) {
                    using (DataTable dt = new DataTable()) {
                        sql = @"select IsUseToolAmortize from TQuoteDetails where QuoteNo = @QuoteNo ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        if ((bool)dt.Rows[0]["IsUseToolAmortize"] == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool IsUseMachineAmortize(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    using (DataTable dt = new DataTable())
                    {
                        sql = @"select IsUseMachineAmortize from TQuoteDetails where QuoteNo = @QuoteNo ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        if ((bool)dt.Rows[0]["IsUseMachineAmortize"] == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool  ProcsendingEmail()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string ReqNum = TxtReqNumber.Text;
                string GridViewName = TxtGvName.Text;
                int i = Convert.ToInt32(TxtTotRecord.Text);
                sql = "";

                for (int a = 0; a < i; a++)
                {
                    RadioButton RbApprove = null;
                    string QuNo = string.Empty;
                    string Vendor = string.Empty;

                    if (GridViewName == "GvDecision")
                    {
                        RbApprove = GvDecision.Rows[a].FindControl("RbApprove") as RadioButton;
                        Vendor = GvDecision.Rows[a].Cells[2].Text;
                        QuNo = GvDecision.Rows[a].Cells[1].Text;
                    }
                    else
                    {
                        RbApprove = GvDecCompr.Rows[a].FindControl("RbApprove") as RadioButton;
                        Vendor = GvDecCompr.Rows[a].Cells[2].Text;
                        QuNo = GvDecCompr.Rows[a].Cells[1].Text;
                    }

                    getcreateusermail(ReqNum, Vendor);

                    #region sending email
                    sendingemail = false;
                    string MsgErr = "";

                    try
                    {
                        //getting PIC mail id
                        #region getting PIC mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_PIC_Details";

                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = ReqNum.ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            pemail1 = string.Empty;
                            Session["pemail1"] = "";
                            while (dr.Read())
                            {

                                pemail1 = string.Concat(pemail1, dr.GetString(0), ";");
                                Session["pemail1"] = string.Concat(Session["pemail1"].ToString(), dr.GetString(0), ";");

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }
                        #endregion

                        //getting manager mail id
                        #region getting manager mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            aemail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_managerdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = "1";
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["aemail"] = "";
                            while (dr.Read())
                            {
                                aemail = string.Concat(aemail, dr.GetString(0), ";");
                                Session["aemail"] = string.Concat(Session["aemail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }
                        #endregion


                        //getting User mail id
                        #region getting User mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Email_userdetails";

                            SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                            vendorid.Direction = ParameterDirection.Input;
                            vendorid.Value = Session["userID"].ToString();
                            cmdget.Parameters.Add(vendorid);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            while (dr.Read())
                            {
                                Uemail = dr.GetString(0);
                                Session["Uemail"] = dr.GetString(0);
                                Session["dept"] = dr.GetString(1);
                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }
                        #endregion

                        //getting Director mail id
                        #region  getting Director mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            demail = string.Empty;
                            SqlCommand cmdget = cnn.CreateCommand();
                            cmdget.CommandType = CommandType.StoredProcedure;
                            cmdget.CommandText = "dbo.Emet_Dir_approval";

                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                            plant.Direction = ParameterDirection.Input;
                            plant.Value = Session["EPlant"].ToString();
                            cmdget.Parameters.Add(plant);

                            SqlParameter dept = new SqlParameter("@dept", SqlDbType.NVarChar);
                            dept.Direction = ParameterDirection.Input;
                            dept.Value = Session["dept"].ToString();
                            cmdget.Parameters.Add(dept);

                            SqlDataReader dr;
                            dr = cmdget.ExecuteReader();
                            Session["demail"] = "";
                            while (dr.Read())
                            {
                                demail = string.Concat(demail, dr.GetString(0), ";");
                                Session["demail"] = string.Concat(Session["demail"].ToString(), dr.GetString(0), ";");
                                //pemail = dr.GetString(1);

                            }
                            dr.Dispose();
                            cnn.Dispose();
                        }
                        #endregion

                        //getting Quote details
                        #region 
                        using (SqlConnection Qcnn = new SqlConnection(EMETModule.GenEMETConnString()))
                        {
                            string returnValue2 = string.Empty;
                            Qcnn.Open();
                            SqlCommand qcmdget = Qcnn.CreateCommand();
                            qcmdget.CommandType = CommandType.StoredProcedure;
                            qcmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                            SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                            id.Direction = ParameterDirection.Input;
                            id.Value = ReqNum.ToString();
                            qcmdget.Parameters.Add(id);

                            SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                            Vid.Direction = ParameterDirection.Input;
                            Vid.Value = Vendor.ToString();
                            qcmdget.Parameters.Add(Vid);

                            SqlDataReader qdr;
                            qdr = qcmdget.ExecuteReader();
                            while (qdr.Read())
                            {

                                quoteno = qdr.GetString(0);
                                Session["quoteno"] = qdr.GetString(0);

                            }
                            qdr.Dispose();
                            Qcnn.Dispose();
                        }
                        #endregion

                        //// comments
                        #region comments

                        try
                        {
                            DataTable dtgett = new DataTable();
                            DataTable dtdate = new DataTable();
                            SqlDataAdapter daa = new SqlDataAdapter();
                            string strGetDataa = string.Empty;
                            //strGetDataa = "select managerreason from TQuoteDetails where RequestNumber = '" + ReqNum.ToString() + "' and VendorCode1 ='" + Vendor.ToString() + "'  ";
                            strGetDataa = @"select 
                                        case when DIRReason is null then ' Remark : ' + DIRRemark else ' Reason : ' + DIRReason end as DIRcomment from TQuoteDetails
                                        where RequestNumber = '" + ReqNum.ToString() + "' and VendorCode1 ='" + Vendor.ToString() + "'  ";
                            daa = new SqlDataAdapter(strGetDataa, EmetCon);
                            daa.Fill(dtgett);
                            if (dtgett.Rows.Count > 0)
                            {
                                string ttt = dtgett.Rows[0].ItemArray[0].ToString();
                                Session["DIRcomment"] = dtgett.Rows[0].ItemArray[0].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex);
                        }
                        #endregion

                        pemail1 = Session["pemail1"].ToString();
                        aemail = Session["aemail"].ToString();
                        demail = Session["demail"].ToString();
                        Uemail = Session["Uemail"].ToString();
                        getcreateusermail(ReqNum, Vendor);
                        //cc = string.Concat(pemail1, aemail, demail, Uemail);
                        string crem = Session["createemail"].ToString();
                        cc = string.Concat(demail, Session["createemail"].ToString());

                        //main mail
                        if (RbApprove.Checked == false)
                        {
                            #region main mail reject
                            aemail = string.Empty;
                            pemail = string.Empty;
                            using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                            {
                                string returnValue = string.Empty;
                                cnn.Open();
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Customer_mail";

                                SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                                vendorid.Direction = ParameterDirection.Input;
                                vendorid.Value = ReqNum.ToString();
                                cmdget.Parameters.Add(vendorid);

                                int status1 = 1;
                                SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                                status.Direction = ParameterDirection.Input;
                                status.Value = Convert.ToInt32(status1.ToString());
                                cmdget.Parameters.Add(status);


                                SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                                Quote.Direction = ParameterDirection.Input;
                                Quote.Value = Session["quoteno"].ToString();
                                cmdget.Parameters.Add(Quote);

                                SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                plant.Direction = ParameterDirection.Input;
                                plant.Value = Session["EPlant"].ToString();
                                cmdget.Parameters.Add(plant);

                                SqlDataReader dr_cus;
                                dr_cus = cmdget.ExecuteReader();
                                pemail1 = string.Empty;
                                Session["customermail"] = "";
                                Session["customermail1"] = "";
                                while (dr_cus.Read())
                                {

                                    customermail = dr_cus.GetString(0);
                                    Session["customermail"] = dr_cus.GetString(0);
                                    customermail1 = dr_cus.GetString(1);
                                    Session["customermail1"] = dr_cus.GetString(1);
                                    customermail = string.Concat(customermail, ";", customermail1);
                                    //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                    Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                    //while start


                                    //email
                                    // getting Messageheader ID from IT Mailapp
                                    
                                    using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                    {
                                        SqlTransaction transaction;
                                        string returnValue1 = string.Empty;
                                        MHcnn.Open();
                                        transaction = MHcnn.BeginTransaction("Approval");
                                        try
                                        {
                                            SqlCommand MHcmdget = MHcnn.CreateCommand();
                                            MHcmdget.Transaction = transaction;
                                            MHcmdget.CommandType = CommandType.StoredProcedure;
                                            MHcmdget.CommandText = "dbo.spGetControlNumber";
                                            SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                            CompanyCode.Direction = ParameterDirection.Input;
                                            CompanyCode.Value = 1;
                                            MHcmdget.Parameters.Add(CompanyCode);

                                            SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                            ControlField.Direction = ParameterDirection.Input;
                                            ControlField.Value = "MessageHeaderID";
                                            MHcmdget.Parameters.Add(ControlField);

                                            SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                            Param1.Direction = ParameterDirection.Input;
                                            Param1.Value = "";
                                            MHcmdget.Parameters.Add(Param1);

                                            SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                            Param2.Direction = ParameterDirection.Input;
                                            Param2.Value = "";
                                            MHcmdget.Parameters.Add(Param2);

                                            SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                            Param3.Direction = ParameterDirection.Input;
                                            Param3.Value = "";
                                            MHcmdget.Parameters.Add(Param3);

                                            SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                            Param4.Direction = ParameterDirection.Input;
                                            Param4.Value = "";
                                            MHcmdget.Parameters.Add(Param4);

                                            SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                            pOutput.Direction = ParameterDirection.Output;
                                            MHcmdget.ExecuteNonQuery();
                                            transaction.Commit();
                                            returnValue1 = pOutput.Value.ToString();
                                            OriginalFilename = returnValue1;
                                            MHid = returnValue1;
                                            Session["MHid"] = returnValue1;
                                            OriginalFilename = MHid + seqNo + formatW;
                                        }
                                        catch (Exception xw)
                                        {
                                            transaction.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                        MHcnn.Dispose();
                                    }
                                    Boolean IsAttachFile = true;
                                    int SequenceNumber = 1;
                                    string test = userId1;
                                    IsAttachFile = false;
                                    SendFilename = "NOFILE";
                                    OriginalFilename = "NOFILE";
                                    format = "NO";

                                    //getting vendor mail content
                                    using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                    {
                                        string returnValue_ = string.Empty;
                                        cnn_.Open();
                                        SqlCommand cmdget_ = cnn_.CreateCommand();
                                        cmdget_.CommandType = CommandType.StoredProcedure;
                                        cmdget_.CommandText = "dbo.Emet_Email_content";

                                        SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                        vendorid_.Direction = ParameterDirection.Input;
                                        vendorid_.Value = Session["quoteno"].ToString();
                                        cmdget_.Parameters.Add(vendorid_);

                                        SqlDataReader dr_;
                                        dr_ = cmdget_.ExecuteReader();
                                        while (dr_.Read())
                                        {
                                            body1 = dr_.GetString(1);
                                            Session["body1"] = dr_.GetString(1);
                                        }
                                        dr_.Dispose();
                                        cnn_.Dispose();
                                    }

                                    // Insert header and details to Mil server table to IT mailserverapp
                                    using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                    {
                                        Email_inser.Open();
                                        //Header
                                        string MessageHeaderId = Session["MHid"].ToString();
                                        string fromname = "eMET System";
                                        //string FromAddress = Session["Uemail"].ToString(); 
                                        string FromAddress = "eMET@Shimano.Com.sg";
                                        //string Recipient = aemail + "," + pemail;
                                        string Recipient = Session["customermail"].ToString();
                                        string CopyRecipient = cc;
                                        string BlindCopyRecipient = "";
                                        string ReplyTo = "subashdurai@shimano.com.sg";
                                        string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Rejection By: " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                        //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                        //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                        //Rejection Reason: " + Session["DIRcomment"].ToString() + " <br />
                                        string transType = GetReqType(Session["quoteno"].ToString());
                                        string body = "";
                                        if (transType == "Revision")
                                        {
                                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation  <font color='red'> (Revision) </font> has been Rejected.  " + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                        }
                                        else
                                        {
                                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected.  " + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                        }
                                        string BodyFormat = "HTML";
                                        string BodyRemark = "0";
                                        string Signature = " ";
                                        string Importance = "High";
                                        string Sensitivity = "Confidential";
                                        string CreateUser = userId1;
                                        DateTime CreateDate = DateTime.Now;
                                        //end Header

                                        SqlTransaction transactionHe;
                                        transactionHe = Email_inser.BeginTransaction("Header");
                                        try
                                        {
                                            string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                            SqlCommand Header = new SqlCommand(Head, Email_inser);
                                            Header.Transaction = transactionHe;
                                            Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                            Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                            Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                            Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                            Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                            Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                            Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                            Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                            Header.Parameters.AddWithValue("@body", body.ToString());
                                            Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                            Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                            Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                            Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                            Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                            Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                            Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                            Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            Header.CommandText = Head;
                                            Header.ExecuteNonQuery();
                                            transactionHe.Commit();
                                        }
                                        catch (Exception xw)
                                        {
                                            transactionHe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                            var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                        //end Header
                                        //Details
                                        SqlTransaction transactionDe;
                                        transactionDe = Email_inser.BeginTransaction("Detail");
                                        try
                                        {
                                            string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                            SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                            Detail.Transaction = transactionDe;
                                            Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                            Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                            Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                            Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                            Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                            Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                            Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                            Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            Detail.CommandText = Details;
                                            Detail.ExecuteNonQuery();
                                            transactionDe.Commit();
                                        }
                                        catch (Exception xw)
                                        {
                                            transactionDe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                            var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                        Email_inser.Dispose();
                                        //End Details
                                    }
                                    //while end
                                }
                                dr_cus.Dispose();
                                cnn.Dispose();
                            }
                            #endregion
                        }
                        else
                        {
                            #region main mail Approv
                            aemail = string.Empty;
                            pemail = string.Empty;
                            using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                            {
                                string returnValue = string.Empty;
                                cnn.Open();
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Customer_mail";

                                SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                                vendorid.Direction = ParameterDirection.Input;
                                vendorid.Value = ReqNum.ToString();
                                cmdget.Parameters.Add(vendorid);

                                int status1 = 2;
                                SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                                status.Direction = ParameterDirection.Input;
                                status.Value = Convert.ToInt32(status1.ToString());
                                cmdget.Parameters.Add(status);


                                SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                                Quote.Direction = ParameterDirection.Input;
                                Quote.Value = Session["quoteno"].ToString();
                                cmdget.Parameters.Add(Quote);

                                SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                plant.Direction = ParameterDirection.Input;
                                plant.Value = Session["EPlant"].ToString();
                                cmdget.Parameters.Add(plant);

                                SqlDataReader dr_cus;
                                dr_cus = cmdget.ExecuteReader();
                                pemail1 = string.Empty;
                                Session["customermail"] = "";
                                Session["customermail1"] = "";

                                while (dr_cus.Read())
                                {

                                    customermail = dr_cus.GetString(0);
                                    Session["customermail"] = dr_cus.GetString(0);
                                    customermail1 = dr_cus.GetString(1);
                                    Session["customermail1"] = dr_cus.GetString(1);
                                    customermail = string.Concat(customermail, ";", customermail1);
                                    //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                    Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                    //while start

                                    //email
                                    // getting Messageheader ID from IT Mailapp
                                    
                                    using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                    {
                                        string returnValue1 = string.Empty;
                                        MHcnn.Open();
                                        SqlTransaction transaction;
                                        transaction = MHcnn.BeginTransaction("Reject");
                                        try
                                        {
                                            SqlCommand MHcmdget = MHcnn.CreateCommand();
                                            MHcmdget.Transaction = transaction;
                                            MHcmdget.CommandType = CommandType.StoredProcedure;
                                            MHcmdget.CommandText = "dbo.spGetControlNumber";

                                            SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                            CompanyCode.Direction = ParameterDirection.Input;
                                            CompanyCode.Value = 1;
                                            MHcmdget.Parameters.Add(CompanyCode);

                                            SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                            ControlField.Direction = ParameterDirection.Input;
                                            ControlField.Value = "MessageHeaderID";
                                            MHcmdget.Parameters.Add(ControlField);

                                            SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                            Param1.Direction = ParameterDirection.Input;
                                            Param1.Value = "";
                                            MHcmdget.Parameters.Add(Param1);

                                            SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                            Param2.Direction = ParameterDirection.Input;
                                            Param2.Value = "";
                                            MHcmdget.Parameters.Add(Param2);

                                            SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                            Param3.Direction = ParameterDirection.Input;
                                            Param3.Value = "";
                                            MHcmdget.Parameters.Add(Param3);

                                            SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                            Param4.Direction = ParameterDirection.Input;
                                            Param4.Value = "";
                                            MHcmdget.Parameters.Add(Param4);

                                            SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                            pOutput.Direction = ParameterDirection.Output;

                                            MHcmdget.ExecuteNonQuery();
                                            transaction.Commit();
                                            returnValue1 = pOutput.Value.ToString();

                                            OriginalFilename = returnValue1;
                                            MHid = returnValue1;
                                            Session["MHid1"] = returnValue1;
                                            OriginalFilename = MHid + seqNo + formatW;
                                        }
                                        catch (Exception xw)
                                        {
                                            transaction.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                            var script = string.Format("alert({0});", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                            sendingemail = false;
                                        }
                                        MHcnn.Dispose();
                                    }

                                    Boolean IsAttachFile = true;
                                    int SequenceNumber = 1;
                                    string test = userId1;
                                    IsAttachFile = false;
                                    SendFilename = "NOFILE";
                                    OriginalFilename = "NOFILE";
                                    format = "NO";




                                    //getting vendor mail content
                                    using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                    {
                                        string returnValue_ = string.Empty;
                                        cnn_.Open();
                                        SqlCommand cmdget_ = cnn_.CreateCommand();
                                        cmdget_.CommandType = CommandType.StoredProcedure;
                                        cmdget_.CommandText = "dbo.Emet_Email_content";

                                        SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                        vendorid_.Direction = ParameterDirection.Input;
                                        vendorid_.Value = Session["quoteno"].ToString();
                                        cmdget_.Parameters.Add(vendorid_);

                                        SqlDataReader dr_;
                                        dr_ = cmdget_.ExecuteReader();
                                        while (dr_.Read())
                                        {
                                            body1 = dr_.GetString(1);
                                            Session["body1"] = dr_.GetString(1);
                                        }
                                        dr_.Dispose();
                                        cnn_.Dispose();
                                    }


                                    // Insert header and details to Mil server table to IT mailserverapp
                                    using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                    {
                                        Email_inser.Open();
                                        //Header
                                        string MessageHeaderId = Session["MHid1"].ToString();
                                        string fromname = "eMET System";
                                        //string FromAddress = Session["Uemail"].ToString();
                                        string FromAddress = "eMET@Shimano.Com.sg";
                                        //string Recipient = aemail + "," + pemail;
                                        string Recipient = Session["customermail"].ToString();
                                        string CopyRecipient = cc;
                                        string BlindCopyRecipient = "";
                                        string ReplyTo = "subashdurai@shimano.com.sg";
                                        string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Approval By : " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                        //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                        //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;

                                        string transType = GetReqType(Session["quoteno"].ToString());
                                        string body = "";
                                        if (transType == "Revision")
                                        {
                                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation <font color='red'> (Revision) </font> has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                        }
                                        else
                                        {
                                            body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                        }
                                        string BodyFormat = "HTML";
                                        string BodyRemark = "0";
                                        string Signature = " ";
                                        string Importance = "High";
                                        string Sensitivity = "Confidential";

                                        string CreateUser = userId1;
                                        DateTime CreateDate = DateTime.Now;
                                        //end Header
                                        SqlTransaction transactionHe;
                                        transactionHe = Email_inser.BeginTransaction("Header");
                                        try
                                        {
                                            string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                            SqlCommand Header = new SqlCommand(Head, Email_inser);
                                            Header.Transaction = transactionHe;
                                            Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                            Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                            Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                            Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                            Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                            Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                            Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                            Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                            Header.Parameters.AddWithValue("@body", body.ToString());
                                            Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                            Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                            Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                            Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                            Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                            Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                            Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                            Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            Header.CommandText = Head;
                                            Header.ExecuteNonQuery();

                                            transactionHe.Commit();
                                        }

                                        ///
                                        catch (Exception xw)
                                        {
                                            transactionHe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                            var script = string.Format("alert({0});", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                            sendingemail = false;
                                        }
                                        //end Header
                                        //Details

                                        SqlTransaction transactionDe;
                                        transactionDe = Email_inser.BeginTransaction("Detail");
                                        try
                                        {
                                            string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                            SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                            Detail.Transaction = transactionDe;
                                            Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                            Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                            Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                            Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                            Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                            Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                            Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                            Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                            Detail.CommandText = Details;
                                            Detail.ExecuteNonQuery();
                                            transactionDe.Commit();
                                        }

                                        ///
                                        catch (Exception xw)
                                        {
                                            sendingemail = false;
                                            transactionDe.Rollback();
                                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                            var script = string.Format("alert({0});", message);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }

                                        Email_inser.Dispose();
                                        //End Details
                                    }

                                    //while end

                                }
                                dr_cus.Dispose();
                                cnn.Dispose();
                            }
                            #endregion
                        }

                        sendingemail = true;
                    }
                    catch (Exception ex)
                    {
                        sendingemail = false;
                        MsgErr = ex.ToString();
                    }

                    if (sendingemail == false)
                    {
                        return false;

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('failed sending email');", true);
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('failed sending email'); ", true);
                    }
                    #endregion sending email
                }
                return true;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void BtnSubmitReject_Click(object sender, EventArgs e)
        {
            try
            {

                string GridViewName = TxtGvName.Text;
                if (ProcessSubmitApproval() == true)
                {
                    if (ProcsendingEmail() == false)
                    {
                        if (GridViewName == "GvDecision")
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted - sending email faill!!");
                            var script = string.Format("alert({0});CloseLoading();CloseModalReasonRejection();DatePitcker();", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalReasonRejection();DatePitcker();CloseLoading();alert('Data Submitted - sending email faill!!');", true);
                        }
                        else
                        {
                            var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted - sending email faill!!");
                            var script = string.Format("alert({0});CloseLoading();CloseModalCompare();DatePitcker();", message);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalCompare();DatePitcker();CloseLoading();alert('Data Submitted - sending email faill!!');", true);
                        }
                    }
                    else
                    {
                        if (sendingemail == false)
                        {
                            if (GridViewName == "GvDecision")
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted - sending email faill!!");
                                var script = string.Format("alert({0});CloseLoading();CloseModalReasonRejection();DatePitcker();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalReasonRejection();DatePitcker();CloseLoading();alert('Data Submitted - sending email faill!!');", true);
                            }
                            else
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted - sending email faill!!");
                                var script = string.Format("alert({0});CloseLoading();CloseModalCompare();DatePitcker();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalCompare();DatePitcker();CloseLoading();alert('Data Submitted - sending email faill!!');", true);
                            }
                        }
                        else
                        {
                            if (GridViewName == "GvDecision")
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submitted successfully");
                                var script = string.Format("alert({0});CloseLoading();CloseModalReasonRejection();DatePitcker();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalReasonRejection();DatePitcker();CloseLoading();alert('Submitted successfully');", true);
                            }
                            else
                            {
                                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Submitted successfully");
                                var script = string.Format("alert({0});CloseLoading();CloseModalCompare();DatePitcker();", message);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalCompare();DatePitcker();CloseLoading();alert('Submitted successfully');", true);
                            }
                        }
                    }
                }
                else
                {
                    if (GridViewName == "GvDecision")
                    {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted Faill !!!: " + LbMsgErr.Text);
                        var script = string.Format("alert({0});CloseLoading();CloseModalReasonRejection();DatePitcker();", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalReasonRejection();DatePitcker();CloseLoading();alert('Data Submitted Faill !!! " + LbMsgErr.Text + "');", true);
                    }
                    else
                    {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted Faill !!!: " + LbMsgErr.Text);
                        var script = string.Format("alert({0});CloseLoading();CloseModalCompare();DatePitcker();", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseModalCompare();DatePitcker();CloseLoading();alert('Data Submitted Faill !!! " + LbMsgErr.Text + "');", true);
                    }
                }
                ShowTable();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Data Submitted Faill !!!: " + LbMsgErr.Text);
                var script = string.Format("CloseLoading();CloseModalCompare();DatePitcker();alert({0});", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected void BtnCancelRejection_Click(object sender, EventArgs e)
        {
            try
            {

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "All";
                string ReqStatus = "All";
                if (Session["DIRApprovalFilter"] == null)
                {
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["DIRApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedValue.ToString();
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["DIRApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status + "!" + ReqStatus;
                }

                if (Session["DIRApprNst"] != null)
                {
                    string RowVsStatus = Session["DIRApprNst"].ToString();
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalUpdateDate();DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void sidebarToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["sidebarToggle"] == null)
                {
                    Session["sidebarToggle"] = "Hide";
                    SideBarMenu.Attributes.Add("style", "display:block;");
                }
                else
                {
                    Session["sidebarToggle"] = null;
                    SideBarMenu.Attributes.Add("style", "display:none;");
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Response.Redirect("Login.aspx");
            }
            catch (ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }


        protected void CheckAllAprOrRej()
        {
            try
            {
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    int CApr = 0;
                    int CRej = 0;
                    if (dt.Rows.Count > 0)
                    {
                        bool AllEmpty = true;
                        CheckBox RbHeaderApp = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllApp");
                        CheckBox RbHeaderRej = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllRej");
                        if (RbHeaderApp != null && RbHeaderRej != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["App"].ToString() == "1")
                                {
                                    CApr++;
                                }
                                else if (dt.Rows[i]["App"].ToString() == "0")
                                {
                                    CRej++;
                                }
                            }

                            if (CApr == 0 && CRej == 0)
                            {
                                AllEmpty = true;
                            }
                            else
                            {
                                AllEmpty = false;
                            }
                            if (AllEmpty == false)
                            {
                                if (CApr == dt.Rows.Count)
                                {
                                    RbHeaderApp.Checked = true;
                                    RbHeaderRej.Checked = false;
                                }
                                else if (CRej == dt.Rows.Count)
                                {
                                    RbHeaderApp.Checked = false;
                                    RbHeaderRej.Checked = true;
                                }
                            }
                            else
                            {
                                RbHeaderApp.Checked = false;
                                RbHeaderRej.Checked = false;
                            }
                            LbTotalRecMassRev.Text = "Tot Record : " + dt.Rows.Count.ToString();
                            LbTotUncheck.Text = "Tot Uncheck : " + (dt.Rows.Count - CApr - CRej).ToString();
                            LbTotApp.Text = "Tot Approve Checked : " + CApr.ToString();
                            LbTotRej.Text = "Tot Reject Checked : " + CRej.ToString();
                        }
                    }
                    else
                    {
                        LbTotalRecMassRev.Text = "Tot Record : 0";
                        LbTotUncheck.Text = "Tot Uncheck : 0";
                        LbTotApp.Text = "Tot Approve Checked : 0";
                        LbTotRej.Text = "Tot Reject Checked : 0";
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                throw;
            }
        }

        protected void TxtShowEntMassRev_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                    Session["TxtShowEntMassRevDIR"] = TxtShowEntMassRev.Text;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvMassRev_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int Idx = e.Row.DataItemIndex;

                    Label LbQuoteNo = e.Row.FindControl("LbQuoteNo") as Label;
                    string url = "QQPReview.aspx?Number=" + LbQuoteNo.Text;
                    LbQuoteNo.Attributes.Add("onclick", "openInNewTab('" + url + "');");

                    CheckBox RbApp = e.Row.FindControl("RbApp") as CheckBox;
                    CheckBox RbRej = e.Row.FindControl("RbRej") as CheckBox;
                    string ReqNo = e.Row.Cells[4].Text;
                    if (Session["DtMassRevDIR"] != null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)Session["DtMassRevDIR"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["RequestNumber"].ToString() == ReqNo)
                            {
                                if (dt.Rows[i]["App"].ToString() == "1")
                                {
                                    RbApp.Checked = true;
                                    RbRej.Checked = false;
                                }
                                else if (dt.Rows[i]["App"].ToString() == "0")
                                {
                                    RbApp.Checked = false;
                                    RbRej.Checked = true;
                                }
                                else
                                {
                                    RbApp.Checked = false;
                                    RbRej.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvMassRev_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GdvMassRev.PageIndex = e.NewPageIndex;
                Session["MassRevPgNoDIR"] = (GdvMassRev.PageIndex).ToString();
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvMassRev_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    dt.DefaultView.Sort = e.SortExpression.ToString().Replace(" ", "") + " " + GetSortDirection(e.SortExpression);
                    dt = dt.DefaultView.ToTable();
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                    Session["DtMassRevDIR"] = dt;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvMassRev_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    int cellHeaderidx = 0;
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        if (cellHeaderidx > 2)
                        {
                            if (tc.HasControls())
                            {
                                LinkButton lb = (LinkButton)tc.Controls[0];
                                if (lb != null)
                                {
                                    System.Web.UI.WebControls.Image icon = new System.Web.UI.WebControls.Image();
                                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                                    {
                                        string sorting = ViewState["SortDirection"].ToString();
                                        if (ViewState["SortExpression"].ToString() == lb.CommandArgument)
                                        {
                                            lb.Attributes.Add("style", "text-decoration:none;");
                                            lb.ForeColor = System.Drawing.Color.Yellow;
                                        }
                                        else
                                        {
                                            lb.Attributes.Add("style", "text-decoration:underline;");
                                        }
                                    }
                                    else
                                    {
                                        lb.Attributes.Add("style", "text-decoration:underline;");
                                    }
                                }
                            }
                        }
                        cellHeaderidx++;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvMassRev_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "LinktoRedirect")
                {
                    string QuoteNo = ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString();
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString(), false);
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

        protected void RbAllApp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox RbHeaderApp = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllApp");
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (RbHeaderApp.Checked == true)
                        {
                            dt.Rows[i]["App"] = 1;
                            dt.Rows[i]["Rej"] = 0;
                        }
                        else if (RbHeaderApp.Checked == false)
                        {
                            dt.Rows[i]["App"] = "";
                            dt.Rows[i]["Rej"] = "";
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRevDIR"] = dt;
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void RbApp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox RbApp = (CheckBox)sender;
                GridViewRow row = (GridViewRow)RbApp.NamingContainer;
                int idx = row.RowIndex;
                string ReqNo = GdvMassRev.Rows[idx].Cells[4].Text;
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["RequestNumber"].ToString() == ReqNo)
                        {
                            if (RbApp.Checked == true)
                            {
                                dt.Rows[i]["App"] = 1;
                                dt.Rows[i]["Rej"] = 0;
                            }
                            else if (RbApp.Checked == false)
                            {
                                dt.Rows[i]["App"] = "";
                                dt.Rows[i]["Rej"] = "";
                            }
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRevDIR"] = dt;
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void RbAllRej_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox RbHeaderRej = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllRej");
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (RbHeaderRej.Checked == true)
                        {
                            dt.Rows[i]["App"] = 0;
                            dt.Rows[i]["Rej"] = 1;
                        }
                        else if (RbHeaderRej.Checked == false)
                        {
                            dt.Rows[i]["App"] = "";
                            dt.Rows[i]["Rej"] = "";
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRevDIR"] = dt;
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void RbRej_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox RbRej = (CheckBox)sender;
                GridViewRow row = (GridViewRow)RbRej.NamingContainer;
                int idx = row.RowIndex;
                string ReqNo = GdvMassRev.Rows[idx].Cells[4].Text;
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRevDIR"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["RequestNumber"].ToString() == ReqNo)
                        {
                            if (RbRej.Checked == true)
                            {
                                dt.Rows[i]["App"] = 0;
                                dt.Rows[i]["Rej"] = 1;
                            }
                            else if (RbRej.Checked == false)
                            {
                                dt.Rows[i]["App"] = "";
                                dt.Rows[i]["Rej"] = "";
                            }
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRevDIR"] = dt;
                    GdvMassRev.DataSource = dt;
                    int ShowEntry = 1;
                    if (TxtShowEntMassRev.Text == "" || TxtShowEntMassRev.Text == "0")
                    {
                        ShowEntry = 1;
                        TxtShowEntMassRev.Text = "1";
                    }
                    else
                    {
                        ShowEntry = Convert.ToInt32(TxtShowEntMassRev.Text);
                    }
                    GdvMassRev.PageSize = ShowEntry;
                    GdvMassRev.DataBind();
                    CheckAllAprOrRej();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        bool ProcessSubmitMassRev()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            try
            {
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable DtMassRevDIR = (DataTable)Session["DtMassRevDIR"];
                    if (DtMassRevDIR.Rows.Count > 0)
                    {
                        EmetCon.Open();
                        EmetTrans = EmetCon.BeginTransaction();
                        sql = "";
                        for (int m = 0; m < DtMassRevDIR.Rows.Count; m++)
                        {
                            string ReqNumber = DtMassRevDIR.Rows[m]["RequestNumber"].ToString();
                            string QuNo = DtMassRevDIR.Rows[m]["QuoteNo"].ToString();
                            if (DtMassRevDIR.Rows[m]["App"].ToString() == "1")
                            {
                                sql = @" Update TQuoteDetails SET ApprovalStatus='" + 3 + "', ManagerApprovalStatus= '" + 2 + @"',
                              DIRApprovalStatus = '" + 0 + "',  AprRejBy = '" + Session["userID"].ToString() + @"', AprRejDate = CURRENT_TIMESTAMP
                               where RequestNumber = '" + ReqNumber + "'  and QuoteNo ='" + QuNo + "' ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();
                            }
                            else if (DtMassRevDIR.Rows[m]["App"].ToString() == "0")
                            {
                                sql = @" Update TQuoteDetails SET ApprovalStatus='" + 1 + @"', ManagerApprovalStatus= '" + 1 + @"',  
                                DIRApprovalStatus = '" + 0 + @"',
                                AprRejBy = '" + Session["userID"].ToString() + @"', AprRejDate = CURRENT_TIMESTAMP
                                where RequestNumber = '" + ReqNumber + "'  and QuoteNo ='" + QuNo + "' ";
                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        EmetTrans.Commit();
                        EmetTrans.Dispose();
                        return true;
                    }
                    else
                    {
                        EmetTrans.Rollback();
                        EmetTrans.Dispose();
                        return false;
                    }
                }
                else
                {
                    EmetTrans.Rollback();
                    EmetTrans.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                EmetTrans.Dispose();
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool ProcsendingEmailMassRev()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                if (Session["DtMassRevDIR"] != null)
                {
                    DataTable DtMassRevDIR = (DataTable)Session["DtMassRevDIR"];
                    if (DtMassRevDIR.Rows.Count > 0)
                    {
                        EmetCon.Open();
                        sql = "";
                        for (int m = 0; m < DtMassRevDIR.Rows.Count; m++)
                        {
                            string ReqNum = DtMassRevDIR.Rows[m]["RequestNumber"].ToString();
                            string QuNo = DtMassRevDIR.Rows[m]["QuoteNo"].ToString();
                            string Vendor = DtMassRevDIR.Rows[m]["VendorCode1"].ToString();
                            if (DtMassRevDIR.Rows[m]["App"].ToString() != "")
                            {
                                getcreateusermail(ReqNum, Vendor);

                                #region sending email
                                sendingemail = false;
                                string MsgErr = "";

                                try
                                {
                                    //getting PIC mail id
                                    #region getting PIC mail id
                                    aemail = string.Empty;
                                    pemail = string.Empty;
                                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                    {
                                        string returnValue = string.Empty;
                                        cnn.Open();
                                        SqlCommand cmdget = cnn.CreateCommand();
                                        cmdget.CommandType = CommandType.StoredProcedure;
                                        cmdget.CommandText = "dbo.Emet_PIC_Details";

                                        SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar);
                                        vendorid.Direction = ParameterDirection.Input;
                                        vendorid.Value = ReqNum.ToString();
                                        cmdget.Parameters.Add(vendorid);

                                        SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                        plant.Direction = ParameterDirection.Input;
                                        plant.Value = Session["EPlant"].ToString();
                                        cmdget.Parameters.Add(plant);

                                        SqlDataReader dr;
                                        dr = cmdget.ExecuteReader();
                                        pemail1 = string.Empty;
                                        Session["pemail1"] = "";
                                        while (dr.Read())
                                        {

                                            pemail1 = string.Concat(pemail1, dr.GetString(0), ";");
                                            Session["pemail1"] = string.Concat(Session["pemail1"].ToString(), dr.GetString(0), ";");

                                        }
                                        dr.Dispose();
                                        cnn.Dispose();
                                    }
                                    #endregion

                                    //getting manager mail id
                                    #region getting manager mail id
                                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                    {
                                        string returnValue = string.Empty;
                                        cnn.Open();
                                        aemail = string.Empty;
                                        SqlCommand cmdget = cnn.CreateCommand();
                                        cmdget.CommandType = CommandType.StoredProcedure;
                                        cmdget.CommandText = "dbo.Emet_Email_managerdetails";

                                        SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                        vendorid.Direction = ParameterDirection.Input;
                                        vendorid.Value = "1";
                                        cmdget.Parameters.Add(vendorid);

                                        SqlDataReader dr;
                                        dr = cmdget.ExecuteReader();
                                        Session["aemail"] = "";
                                        while (dr.Read())
                                        {
                                            aemail = string.Concat(aemail, dr.GetString(0), ";");
                                            Session["aemail"] = string.Concat(Session["aemail"].ToString(), dr.GetString(0), ";");
                                            //pemail = dr.GetString(1);

                                        }
                                        dr.Dispose();
                                        cnn.Dispose();
                                    }
                                    #endregion


                                    //getting User mail id
                                    #region getting User mail id
                                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                    {
                                        string returnValue = string.Empty;
                                        cnn.Open();
                                        SqlCommand cmdget = cnn.CreateCommand();
                                        cmdget.CommandType = CommandType.StoredProcedure;
                                        cmdget.CommandText = "dbo.Emet_Email_userdetails";

                                        SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                                        vendorid.Direction = ParameterDirection.Input;
                                        vendorid.Value = Session["userID"].ToString();
                                        cmdget.Parameters.Add(vendorid);

                                        SqlDataReader dr;
                                        dr = cmdget.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            Uemail = dr.GetString(0);
                                            Session["Uemail"] = dr.GetString(0);
                                            Session["dept"] = dr.GetString(1);
                                        }
                                        dr.Dispose();
                                        cnn.Dispose();
                                    }
                                    #endregion

                                    //getting Director mail id
                                    #region  getting Director mail id
                                    using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                    {
                                        string returnValue = string.Empty;
                                        cnn.Open();
                                        demail = string.Empty;
                                        SqlCommand cmdget = cnn.CreateCommand();
                                        cmdget.CommandType = CommandType.StoredProcedure;
                                        cmdget.CommandText = "dbo.Emet_Dir_approval";

                                        SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                        plant.Direction = ParameterDirection.Input;
                                        plant.Value = Session["EPlant"].ToString();
                                        cmdget.Parameters.Add(plant);

                                        SqlParameter dept = new SqlParameter("@dept", SqlDbType.NVarChar);
                                        dept.Direction = ParameterDirection.Input;
                                        dept.Value = Session["dept"].ToString();
                                        cmdget.Parameters.Add(dept);

                                        SqlDataReader dr;
                                        dr = cmdget.ExecuteReader();
                                        Session["demail"] = "";
                                        while (dr.Read())
                                        {
                                            demail = string.Concat(demail, dr.GetString(0), ";");
                                            Session["demail"] = string.Concat(Session["demail"].ToString(), dr.GetString(0), ";");
                                            //pemail = dr.GetString(1);

                                        }
                                        dr.Dispose();
                                        cnn.Dispose();
                                    }
                                    #endregion

                                    //getting Quote details
                                    #region 
                                    using (SqlConnection Qcnn = new SqlConnection(EMETModule.GenEMETConnString()))
                                    {
                                        string returnValue2 = string.Empty;
                                        Qcnn.Open();
                                        SqlCommand qcmdget = Qcnn.CreateCommand();
                                        qcmdget.CommandType = CommandType.StoredProcedure;
                                        qcmdget.CommandText = "dbo.[Emet_get_Quotedetails]";

                                        SqlParameter id = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                        id.Direction = ParameterDirection.Input;
                                        id.Value = ReqNum.ToString();
                                        qcmdget.Parameters.Add(id);

                                        SqlParameter Vid = new SqlParameter("@Vid", SqlDbType.NVarChar, 50);
                                        Vid.Direction = ParameterDirection.Input;
                                        Vid.Value = Vendor.ToString();
                                        qcmdget.Parameters.Add(Vid);

                                        SqlDataReader qdr;
                                        qdr = qcmdget.ExecuteReader();
                                        while (qdr.Read())
                                        {
                                            quoteno = qdr.GetString(0);
                                            Session["quoteno"] = qdr.GetString(0);
                                        }
                                        qdr.Dispose();
                                        Qcnn.Dispose();
                                    }
                                    #endregion

                                    //// comments
                                    #region comments
                                    try
                                    {
                                        DataTable dtgett = new DataTable();
                                        DataTable dtdate = new DataTable();
                                        SqlDataAdapter daa = new SqlDataAdapter();
                                        string strGetDataa = string.Empty;
                                        strGetDataa = @"select 
                                                        case when DIRReason is null then ' Remark : ' + DIRRemark else ' Reason : ' + DIRReason end as DIRcomment 
                                                        from TQuoteDetails 
                                                        where RequestNumber = '" + ReqNum.ToString() + @"' and VendorCode1 ='" + Vendor.ToString() + "'  ";
                                        daa = new SqlDataAdapter(strGetDataa, EmetCon);
                                        daa.Fill(dtgett);
                                        if (dtgett.Rows.Count > 0)
                                        {
                                            string ttt = dtgett.Rows[0].ItemArray[0].ToString();
                                            Session["DIRcomment"] = dtgett.Rows[0].ItemArray[0].ToString();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Write(ex);
                                    }
                                    #endregion

                                    pemail1 = Session["pemail1"].ToString();
                                    aemail = Session["aemail"].ToString();
                                    demail = Session["demail"].ToString();
                                    Uemail = Session["Uemail"].ToString();
                                    getcreateusermail(ReqNum, Vendor);
                                    //cc = string.Concat(pemail1, aemail, demail, Uemail);
                                    string crem = Session["createemail"].ToString();
                                    cc = string.Concat(demail, Session["createemail"].ToString());

                                    //main mail
                                    if (DtMassRevDIR.Rows[m]["App"].ToString() == "1")
                                    {
                                        #region main mail Approv
                                        aemail = string.Empty;
                                        pemail = string.Empty;
                                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                        {
                                            string returnValue = string.Empty;
                                            cnn.Open();
                                            SqlCommand cmdget = cnn.CreateCommand();
                                            cmdget.CommandType = CommandType.StoredProcedure;
                                            cmdget.CommandText = "dbo.Emet_Customer_mail";

                                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                                            vendorid.Direction = ParameterDirection.Input;
                                            vendorid.Value = ReqNum.ToString();
                                            cmdget.Parameters.Add(vendorid);

                                            int status1 = 2;
                                            SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                                            status.Direction = ParameterDirection.Input;
                                            status.Value = Convert.ToInt32(status1.ToString());
                                            cmdget.Parameters.Add(status);


                                            SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                                            Quote.Direction = ParameterDirection.Input;
                                            Quote.Value = Session["quoteno"].ToString();
                                            cmdget.Parameters.Add(Quote);

                                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                            plant.Direction = ParameterDirection.Input;
                                            plant.Value = Session["EPlant"].ToString();
                                            cmdget.Parameters.Add(plant);

                                            SqlDataReader dr_cus;
                                            dr_cus = cmdget.ExecuteReader();
                                            pemail1 = string.Empty;
                                            Session["customermail"] = "";
                                            Session["customermail1"] = "";

                                            while (dr_cus.Read())
                                            {

                                                customermail = dr_cus.GetString(0);
                                                Session["customermail"] = dr_cus.GetString(0);
                                                customermail1 = dr_cus.GetString(1);
                                                Session["customermail1"] = dr_cus.GetString(1);
                                                customermail = string.Concat(customermail, ";", customermail1);
                                                //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                                Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                                //while start

                                                //email
                                                // getting Messageheader ID from IT Mailapp
                                                
                                                using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                                {
                                                    string returnValue1 = string.Empty;
                                                    MHcnn.Open();
                                                    SqlTransaction transaction;
                                                    transaction = MHcnn.BeginTransaction("Reject");
                                                    try
                                                    {
                                                        SqlCommand MHcmdget = MHcnn.CreateCommand();
                                                        MHcmdget.Transaction = transaction;
                                                        MHcmdget.CommandType = CommandType.StoredProcedure;
                                                        MHcmdget.CommandText = "dbo.spGetControlNumber";

                                                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                                        CompanyCode.Direction = ParameterDirection.Input;
                                                        CompanyCode.Value = 1;
                                                        MHcmdget.Parameters.Add(CompanyCode);

                                                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                                        ControlField.Direction = ParameterDirection.Input;
                                                        ControlField.Value = "MessageHeaderID";
                                                        MHcmdget.Parameters.Add(ControlField);

                                                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                                        Param1.Direction = ParameterDirection.Input;
                                                        Param1.Value = "";
                                                        MHcmdget.Parameters.Add(Param1);

                                                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                                        Param2.Direction = ParameterDirection.Input;
                                                        Param2.Value = "";
                                                        MHcmdget.Parameters.Add(Param2);

                                                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                                        Param3.Direction = ParameterDirection.Input;
                                                        Param3.Value = "";
                                                        MHcmdget.Parameters.Add(Param3);

                                                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                                        Param4.Direction = ParameterDirection.Input;
                                                        Param4.Value = "";
                                                        MHcmdget.Parameters.Add(Param4);

                                                        SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                                        pOutput.Direction = ParameterDirection.Output;

                                                        MHcmdget.ExecuteNonQuery();
                                                        transaction.Commit();
                                                        returnValue1 = pOutput.Value.ToString();

                                                        OriginalFilename = returnValue1;
                                                        MHid = returnValue1;
                                                        Session["MHid1"] = returnValue1;
                                                        OriginalFilename = MHid + seqNo + formatW;
                                                    }
                                                    catch (Exception xw)
                                                    {
                                                        transaction.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                                        var script = string.Format("alert({0});", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                        sendingemail = false;
                                                    }
                                                    MHcnn.Dispose();
                                                }

                                                Boolean IsAttachFile = true;
                                                int SequenceNumber = 1;
                                                string test = userId1;
                                                IsAttachFile = false;
                                                SendFilename = "NOFILE";
                                                OriginalFilename = "NOFILE";
                                                format = "NO";




                                                //getting vendor mail content
                                                using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                                {
                                                    string returnValue_ = string.Empty;
                                                    cnn_.Open();
                                                    SqlCommand cmdget_ = cnn_.CreateCommand();
                                                    cmdget_.CommandType = CommandType.StoredProcedure;
                                                    cmdget_.CommandText = "dbo.Emet_Email_content";

                                                    SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                                    vendorid_.Direction = ParameterDirection.Input;
                                                    vendorid_.Value = Session["quoteno"].ToString();
                                                    cmdget_.Parameters.Add(vendorid_);

                                                    SqlDataReader dr_;
                                                    dr_ = cmdget_.ExecuteReader();
                                                    while (dr_.Read())
                                                    {
                                                        body1 = dr_.GetString(1);
                                                        Session["body1"] = dr_.GetString(1);
                                                    }
                                                    dr_.Dispose();
                                                    cnn_.Dispose();
                                                }


                                                // Insert header and details to Mil server table to IT mailserverapp
                                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                                {
                                                    Email_inser.Open();
                                                    //Header
                                                    string MessageHeaderId = Session["MHid1"].ToString();
                                                    string fromname = "eMET System";
                                                    //string FromAddress = Session["Uemail"].ToString();
                                                    string FromAddress = "eMET@Shimano.Com.sg";
                                                    //string Recipient = aemail + "," + pemail;
                                                    //string Recipient = Session["customermail"].ToString();
                                                    string Recipient = Session["Uemail"].ToString();
                                                    string CopyRecipient = cc;
                                                    string BlindCopyRecipient = "";
                                                    string ReplyTo = "subashdurai@shimano.com.sg";
                                                    string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Manager level Approval By : " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;

                                                    string transType = GetReqType(Session["quoteno"].ToString());
                                                    string body = "";
                                                    if (transType == "Revision")
                                                    {
                                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation <font color='red'> (Revision) </font> has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                                    }
                                                    else
                                                    {
                                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
                                                    }
                                                    string BodyFormat = "HTML";
                                                    string BodyRemark = "0";
                                                    string Signature = " ";
                                                    string Importance = "High";
                                                    string Sensitivity = "Confidential";

                                                    string CreateUser = userId1;
                                                    DateTime CreateDate = DateTime.Now;
                                                    //end Header
                                                    SqlTransaction transactionHe;
                                                    transactionHe = Email_inser.BeginTransaction("Header");
                                                    try
                                                    {
                                                        string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                                        SqlCommand Header = new SqlCommand(Head, Email_inser);
                                                        Header.Transaction = transactionHe;
                                                        Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                                        Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                                        Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                                        Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                                        Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                                        Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                                        Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                                        Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                                        Header.Parameters.AddWithValue("@body", body.ToString());
                                                        Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                                        Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                                        Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                                        Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                                        Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                                        Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                                        Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                                        Header.CommandText = Head;
                                                        Header.ExecuteNonQuery();

                                                        transactionHe.Commit();
                                                    }

                                                    ///
                                                    catch (Exception xw)
                                                    {
                                                        transactionHe.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                                        var script = string.Format("alert({0});", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                        sendingemail = false;
                                                    }
                                                    //end Header
                                                    //Details

                                                    SqlTransaction transactionDe;
                                                    transactionDe = Email_inser.BeginTransaction("Detail");
                                                    try
                                                    {
                                                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                                        Detail.Transaction = transactionDe;
                                                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid1"].ToString());
                                                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                                        Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                                        Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                                        Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                                        Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                                        Detail.CommandText = Details;
                                                        Detail.ExecuteNonQuery();
                                                        transactionDe.Commit();
                                                    }

                                                    ///
                                                    catch (Exception xw)
                                                    {
                                                        sendingemail = false;
                                                        transactionDe.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                                        var script = string.Format("alert({0});", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                    }

                                                    Email_inser.Dispose();
                                                    //End Details
                                                }

                                                //while end

                                            }
                                            dr_cus.Dispose();
                                            cnn.Dispose();
                                        }
                                        #endregion
                                    }
                                    else if (DtMassRevDIR.Rows[m]["App"].ToString() == "0")
                                    {
                                        #region main mail reject
                                        aemail = string.Empty;
                                        pemail = string.Empty;
                                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                        {
                                            string returnValue = string.Empty;
                                            cnn.Open();
                                            SqlCommand cmdget = cnn.CreateCommand();
                                            cmdget.CommandType = CommandType.StoredProcedure;
                                            cmdget.CommandText = "dbo.Emet_Customer_mail";

                                            SqlParameter vendorid = new SqlParameter("@ReqNum", SqlDbType.NVarChar, 50);
                                            vendorid.Direction = ParameterDirection.Input;
                                            vendorid.Value = ReqNum.ToString();
                                            cmdget.Parameters.Add(vendorid);

                                            int status1 = 1;
                                            SqlParameter status = new SqlParameter("@status", SqlDbType.Int);
                                            status.Direction = ParameterDirection.Input;
                                            status.Value = Convert.ToInt32(status1.ToString());
                                            cmdget.Parameters.Add(status);


                                            SqlParameter Quote = new SqlParameter("@Quoteno", SqlDbType.NVarChar, 50);
                                            Quote.Direction = ParameterDirection.Input;
                                            Quote.Value = Session["quoteno"].ToString();
                                            cmdget.Parameters.Add(Quote);

                                            SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                            plant.Direction = ParameterDirection.Input;
                                            plant.Value = Session["EPlant"].ToString();
                                            cmdget.Parameters.Add(plant);

                                            SqlDataReader dr_cus;
                                            dr_cus = cmdget.ExecuteReader();
                                            pemail1 = string.Empty;
                                            Session["customermail"] = "";
                                            Session["customermail1"] = "";
                                            while (dr_cus.Read())
                                            {

                                                customermail = dr_cus.GetString(0);
                                                Session["customermail"] = dr_cus.GetString(0);
                                                customermail1 = dr_cus.GetString(1);
                                                Session["customermail1"] = dr_cus.GetString(1);
                                                customermail = string.Concat(customermail, ";", customermail1);
                                                //Session["customermail"] = string.Concat(customermail, ";", customermail1);
                                                Session["customermail"] = string.Concat(Session["customermail"].ToString(), ";", Session["customermail1"].ToString());
                                                //while start


                                                //email
                                                // getting Messageheader ID from IT Mailapp
                                                
                                                using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                                                {
                                                    SqlTransaction transaction;
                                                    string returnValue1 = string.Empty;
                                                    MHcnn.Open();
                                                    transaction = MHcnn.BeginTransaction("Approval");
                                                    try
                                                    {
                                                        SqlCommand MHcmdget = MHcnn.CreateCommand();
                                                        MHcmdget.Transaction = transaction;
                                                        MHcmdget.CommandType = CommandType.StoredProcedure;
                                                        MHcmdget.CommandText = "dbo.spGetControlNumber";
                                                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                                        CompanyCode.Direction = ParameterDirection.Input;
                                                        CompanyCode.Value = 1;
                                                        MHcmdget.Parameters.Add(CompanyCode);

                                                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                                        ControlField.Direction = ParameterDirection.Input;
                                                        ControlField.Value = "MessageHeaderID";
                                                        MHcmdget.Parameters.Add(ControlField);

                                                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                                        Param1.Direction = ParameterDirection.Input;
                                                        Param1.Value = "";
                                                        MHcmdget.Parameters.Add(Param1);

                                                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                                        Param2.Direction = ParameterDirection.Input;
                                                        Param2.Value = "";
                                                        MHcmdget.Parameters.Add(Param2);

                                                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                                        Param3.Direction = ParameterDirection.Input;
                                                        Param3.Value = "";
                                                        MHcmdget.Parameters.Add(Param3);

                                                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                                        Param4.Direction = ParameterDirection.Input;
                                                        Param4.Value = "";
                                                        MHcmdget.Parameters.Add(Param4);

                                                        SqlParameter pOutput = MHcmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                                        pOutput.Direction = ParameterDirection.Output;
                                                        MHcmdget.ExecuteNonQuery();
                                                        transaction.Commit();
                                                        returnValue1 = pOutput.Value.ToString();
                                                        OriginalFilename = returnValue1;
                                                        MHid = returnValue1;
                                                        Session["MHid"] = returnValue1;
                                                        OriginalFilename = MHid + seqNo + formatW;
                                                    }
                                                    catch (Exception xw)
                                                    {
                                                        transaction.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                    }
                                                    MHcnn.Dispose();
                                                }
                                                Boolean IsAttachFile = true;
                                                int SequenceNumber = 1;
                                                string test = userId1;
                                                IsAttachFile = false;
                                                SendFilename = "NOFILE";
                                                OriginalFilename = "NOFILE";
                                                format = "NO";

                                                //getting vendor mail content
                                                using (SqlConnection cnn_ = new SqlConnection(EMETModule.GenEMETConnString()))
                                                {
                                                    string returnValue_ = string.Empty;
                                                    cnn_.Open();
                                                    SqlCommand cmdget_ = cnn_.CreateCommand();
                                                    cmdget_.CommandType = CommandType.StoredProcedure;
                                                    cmdget_.CommandText = "dbo.Emet_Email_content";

                                                    SqlParameter vendorid_ = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                                    vendorid_.Direction = ParameterDirection.Input;
                                                    vendorid_.Value = Session["quoteno"].ToString();
                                                    cmdget_.Parameters.Add(vendorid_);

                                                    SqlDataReader dr_;
                                                    dr_ = cmdget_.ExecuteReader();
                                                    while (dr_.Read())
                                                    {
                                                        body1 = dr_.GetString(1);
                                                        Session["body1"] = dr_.GetString(1);
                                                    }
                                                    dr_.Dispose();
                                                    cnn_.Dispose();
                                                }

                                                // Insert header and details to Mil server table to IT mailserverapp
                                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                                {
                                                    Email_inser.Open();
                                                    //Header
                                                    string MessageHeaderId = Session["MHid"].ToString();
                                                    string fromname = "eMET System";
                                                    //string FromAddress = Session["Uemail"].ToString(); 
                                                    string FromAddress = "eMET@Shimano.Com.sg";
                                                    //string Recipient = aemail + "," + pemail;
                                                    string Recipient = Session["Uemail"].ToString();
                                                    string CopyRecipient = cc;
                                                    string BlindCopyRecipient = "";
                                                    string ReplyTo = "subashdurai@shimano.com.sg";
                                                    string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Manager Level Rejection By: " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                                    //Rejection Reason: " + Session["DIRcomment"].ToString() + " <br />

                                                    string transType = GetReqType(Session["quoteno"].ToString());
                                                    string body = "";
                                                    if (transType == "Revision")
                                                    {
                                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation <font color='red'> (Revision) </font> has been Rejected" + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                                    }
                                                    else
                                                    {
                                                        body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected" + Session["DIRcomment"].ToString() + ".<br /> <br />" + Session["body1"].ToString();
                                                    }
                                                    string BodyFormat = "HTML";
                                                    string BodyRemark = "0";
                                                    string Signature = " ";
                                                    string Importance = "High";
                                                    string Sensitivity = "Confidential";
                                                    string CreateUser = userId1;
                                                    DateTime CreateDate = DateTime.Now;
                                                    //end Header

                                                    SqlTransaction transactionHe;
                                                    transactionHe = Email_inser.BeginTransaction("Header");
                                                    try
                                                    {
                                                        string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                                        SqlCommand Header = new SqlCommand(Head, Email_inser);
                                                        Header.Transaction = transactionHe;
                                                        Header.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                                        Header.Parameters.AddWithValue("@fromname", fromname.ToString());
                                                        Header.Parameters.AddWithValue("@FromAddress", FromAddress.ToString());
                                                        Header.Parameters.AddWithValue("@Recipient", Recipient.ToString());
                                                        Header.Parameters.AddWithValue("@CopyRecipient", CopyRecipient.ToString());
                                                        Header.Parameters.AddWithValue("@BlindCopyRecipient", BlindCopyRecipient.ToString());
                                                        Header.Parameters.AddWithValue("@ReplyTo", ReplyTo.ToString());
                                                        Header.Parameters.AddWithValue("@Subject", Subject.ToString());
                                                        Header.Parameters.AddWithValue("@body", body.ToString());
                                                        Header.Parameters.AddWithValue("@BodyFormat", BodyFormat.ToString());
                                                        Header.Parameters.AddWithValue("@BodyRemark", BodyRemark.ToString());
                                                        Header.Parameters.AddWithValue("@Signature", Signature.ToString());
                                                        Header.Parameters.AddWithValue("@Importance", Importance.ToString());
                                                        Header.Parameters.AddWithValue("@Sensitivity", Sensitivity.ToString());
                                                        Header.Parameters.AddWithValue("@IsAttachFile", IsAttachFile.ToString());
                                                        Header.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                                        Header.CommandText = Head;
                                                        Header.ExecuteNonQuery();
                                                        transactionHe.Commit();
                                                    }
                                                    catch (Exception xw)
                                                    {
                                                        transactionHe.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                                        var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                    }
                                                    //end Header
                                                    //Details
                                                    SqlTransaction transactionDe;
                                                    transactionDe = Email_inser.BeginTransaction("Detail");
                                                    try
                                                    {
                                                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                                        Detail.Transaction = transactionDe;
                                                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                                        Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                                        Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                                        Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                                        Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID"].ToString());
                                                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                                        Detail.CommandText = Details;
                                                        Detail.ExecuteNonQuery();
                                                        transactionDe.Commit();
                                                    }
                                                    catch (Exception xw)
                                                    {
                                                        transactionDe.Rollback();
                                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                                    }
                                                    Email_inser.Dispose();
                                                    //End Details
                                                }
                                                //while end
                                            }
                                            dr_cus.Dispose();
                                            cnn.Dispose();
                                        }
                                        #endregion
                                    }

                                    sendingemail = true;
                                }
                                catch (Exception ex)
                                {
                                    sendingemail = false;
                                    MsgErr = ex.ToString();
                                }

                                if (sendingemail == false)
                                {
                                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email :  " + MsgErr + " ");
                                    var script = string.Format("alert({0});", message);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                #endregion sending email
                            }
                        }
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void BtnProceed_Click(object sender, EventArgs e)
        {
            if (ProcessSubmitMassRev() == true)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "DatePitcker();CloseLoading();alert('Data Submitted!');", true);
                if (ProcsendingEmailMassRev() == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "DatePitcker();CloseLoading();alert('Data Submitted - sending email faill!!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "DatePitcker();CloseLoading();alert('Data Submitted!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "DatePitcker();CloseLoading();alert('Submit Faill!');", true);
            }
            ShowTableMassRev();
        }
    }
}