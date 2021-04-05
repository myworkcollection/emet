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
using System.Threading;

namespace Material_Evaluation
{
    public partial class MassRevReqWait : System.Web.UI.Page
    {
        public string MHid;
        public string seqNo = "1";
        public string format = "pdf";
        public string formatW = ".pdf";
        public string OriginalFilename;
        public static string fname = "";
        public static string Source = "";
        public static string RequestIncNumber1;
        public static string SendFilename;
        public static string userId1;
        public static string nameC;
        public static string aemail;
        public static string pemail;
        public static string pemail1;
        public static string Uemail;
        public static string body1;
        public static string quoteno;
        public static string quoteno1;
        public static int benable;
        public static string vname;


        public static string demail;

        public static string vemail;

        public static string customermail;
        public static string customermail1;
        public static string cc;

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        string DbMasterName = "";

        string ReqNo;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null || Session["UserName"] == null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }

            else
            {
                if (!Page.IsPostBack)
                {
                    string UI = Session["userID"].ToString();
                    string FN = "EMET_MassRevVndPending";
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
                        nameC = sname;
                        userId1 = Session["userID"].ToString();
                        //Session["UserName"] = userId;

                        if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                        {
                            if (Request.QueryString["Num"] == "2")
                            {
                                DdlTaskStatus.SelectedValue = "Draft";
                            }
                            else if (Request.QueryString["Num"] == "1")
                            {
                                DdlTaskStatus.SelectedValue = "New";
                            }
                        }

                        if (Session["ReqWaitFilterMassRev"] != null)
                        {
                            string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                            if (ArrReqWaitFilter[0].ToString() != "")
                            {
                                ViewState["SortExpression"] = ArrReqWaitFilter[0].ToString();
                            }
                            if (ArrReqWaitFilter[1].ToString() != "")
                            {
                                ViewState["SortDirection"] = ArrReqWaitFilter[1].ToString();
                            }
                            DdlFilterBy.SelectedValue = ArrReqWaitFilter[2].ToString();
                            txtFind.Text = ArrReqWaitFilter[3].ToString();

                            DdlFltrDate.SelectedValue = ArrReqWaitFilter[4].ToString();
                            string[] ArrDate = ArrReqWaitFilter[5].ToString().Split('~');

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
                            DdlTaskStatus.SelectedValue = ArrReqWaitFilter[6].ToString();
                        }
                        LbsystemVersion.Text = Session["SystemVersion"].ToString();
                        if (Session["ShowEntryReqWaitt"] != null)
                        {
                            TxtShowEntry.Text = Session["ShowEntryReqWaitt"].ToString();
                        }
                        ShowTable();

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
            }
        }

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                DbMasterName = "";
            }
        }

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,
                            (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate, 
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                             Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep' from TQuoteDetails A
                            where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null)
                            and (CreateStatus <> '' or CreateStatus is not null) and isUseSAPCode = 1 and (isMassRevision=1)
                            and Plant  = '" + Session["EPlant"].ToString() + "' and A.QuoteNoRef is null and isnull(A.CreateStatus,'') <> '' ";

                    if (DdlTaskStatus.SelectedValue == "New")
                    {
                        sql += @" and (select top 1 QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.isMassRevision = 1) ";
                    }
                    else if (DdlTaskStatus.SelectedValue == "Draft")
                    {
                        sql += @" and (select top 1  QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.isMassRevision = 1) ";
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
                    cmd.CommandTimeout = 10000;
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
                        Session["ShowEntryReqWaitt"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["ReqWaitPgNo"] != null)
                            {
                                int ReqWaitPgNo = Convert.ToInt32(Session["ReqWaitPgNo"].ToString());
                                if (GridView1.PageCount >= ReqWaitPgNo)
                                {
                                    GridView1.PageIndex = ReqWaitPgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["ReqWaitPgNo"] = null;
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
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void ShowTableDet(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select A.VendorCode1,A.VendorName,A.QuoteNo,'" + RowParentGv + @"' as ParentGvRowNo,
                            CASE
                            WHEN (select top 1  QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.isMassRevision = 1) THEN 'New'
                            WHEN (select top 1  QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.isMassRevision = 1) THEN 'Draft'
                            ELSE 'error'
                            END AS TaskStatus
                            from TQuoteDetails A
                            where isnull(A.CreateStatus,'') <> '' and A.RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or A.ApprovalStatus is  null) 
                            and (A.CreateStatus <> '' or A.CreateStatus is not null) and A.RequestNumber = '" + RequestNumber + "' and A.QuoteNoRef is null and (A.isMassRevision=1)  ";

                    if (DdlTaskStatus.SelectedValue == "New")
                    {
                        sql += @" and (select top 1  QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.isMassRevision = 1) ";
                    }
                    else if (DdlTaskStatus.SelectedValue == "Draft")
                    {
                        sql += @" and (select top 1  QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.isMassRevision = 1) ";
                    }

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "QuoteNo")
                        {
                            sql += @" and A.QuoteNo like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode1")
                        {
                            sql += @" and A.VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorName")
                        {
                            sql += @" and A.VendorName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroup")
                        {
                            sql += @" and A.ProcessGroup like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + @".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = A.ProcessGroup) like '%'+@Filter+'%' ";
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

        protected void GetDataVendUpdateDueDate(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select VendorCode1,VendorName,QuoteNo,RequestNumber,CONVERT(varchar, QuoteResponseDueDate, 103) 
                            from TQuoteDetails WHERE RequestNumber =@RequestNumber ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        grdvendor.DataSource = dt;
                        grdvendor.DataBind();
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

        protected bool BtnActionEnDis(int rowsGvMain)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            bool ActionEnDis = false;
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string ReqNo = "";
                    if (GridView1.Rows.Count > 0)
                    {
                        ReqNo = GridView1.Rows[rowsGvMain - 2].Cells[3].Text;
                    }
                    else
                    {
                        //ReqNo = GridView1.Rows[rowsGvMain - 2].Cells[3].Text;
                    }


                    cmd = new SqlCommand("Get_PIRTran", EmetCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@flag", 8);
                    cmd.Parameters.AddWithValue("@quotenumber", ReqNo);
                    
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ActionEnDis = true;
                            //e.Row.Cells[4].Enabled = true;
                            //e.Row.Cells[5].Enabled = true;
                        }
                        else
                        {
                            ActionEnDis = false;
                            //e.Row.Cells[4].Enabled = false;
                            //e.Row.Cells[5].Enabled = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ActionEnDis = false;
            }
            finally
            {
                EmetCon.Dispose();
            }
            return ActionEnDis;
        }

        


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[12].ColumnSpan = 2;
                    e.Row.Cells[13].Visible = false;
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;
                    GridView GvDet = e.Row.FindControl("GvDet") as GridView;
                    //string idCollege = grdview.DataKeys[e.Row.RowIndex].Value.ToString();
                    string iBranc = e.Row.Cells[3].Text;
                    ReqNo = iBranc;
                    ShowTableDet(iBranc, (RowParentGv + 1));
                    DataTable DtDetReqNo = new DataTable();
                    DtDetReqNo = (DataTable)Session["TableDet"];
                    GvDet.DataSource = DtDetReqNo;
                    GvDet.DataBind();

                }

                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                {
                    try
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            ReqNo = e.Row.Cells[3].Text;
                            sql = @"select * from TQuoteDetails where QuoteResponseDueDate <=GETDATE() and ApprovalStatus=0 and RequestNumber=@quotenumber";
                            cmd = new SqlCommand(sql, EmetCon);
                            cmd.Parameters.AddWithValue("@quotenumber", ReqNo);
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    Button BtnApprove = (Button)e.Row.FindControl("BtnApprove");
                                    Button BtnReject = (Button)e.Row.FindControl("BtnReject");
                                    if (BtnApprove != null)
                                    {
                                        BtnApprove.Enabled = true;
                                        BtnReject.Enabled = true;
                                    }
                                    //e.Row.Cells[9].Enabled = true;
                                    //e.Row.Cells[10].Enabled = true;
                                }
                                else
                                {
                                    Button BtnApprove = (Button)e.Row.FindControl("BtnApprove");
                                    Button BtnReject = (Button)e.Row.FindControl("BtnReject");
                                    if (BtnApprove != null)
                                    {
                                        BtnApprove.Enabled = false;
                                        BtnReject.Enabled = false;
                                    }
                                    //e.Row.Cells[9].Enabled = false;
                                    //e.Row.Cells[10].Enabled = false;
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

                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                {
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                Session["ReqWaitPgNo"] = (GridView1.PageIndex).ToString();
                ShowTable();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading()", true);
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

                if (e.CommandName == "Approve")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string ReqNumber = row.Cells[3].Text;

                    GetDataVendUpdateDueDate(ReqNumber);
                    TxtModalReqNo.Text = ReqNumber;
                    TxtModalDueDate.Text = row.Cells[5].Text;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();DatePitcker();", true);
                    if (Session["ReqWaitNst"] != null)
                    {
                        string RowVsStatus = Session["ReqWaitNst"].ToString();
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                    }

                    upModal.Update();

                    //Response.Redirect("DateUpdate.aspx?Number=" + ReqNumber.ToString());
                }
                else if (e.CommandName == "Reject")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string Reason = "Quotation Canceled";
                    string ReqNumber = row.Cells[3].Text;
                    string Vendor = "";
                    //subash
                    if (Reason.ToString() != "")
                    {
                        UpdateGridData(ReqNumber, Vendor, 1, Reason);
                        ShowTable();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Plz fill  the Reason to Reject')", true);
                    }
                }
                else if (e.CommandName == "TrgNestedExpand")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Ex";
                    Session["ReqWaitNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');DatePitcker();", true);
                }
                else if (e.CommandName == "TrgNestedColapse")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Colp";
                    Session["ReqWaitNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');DatePitcker();", true);
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

        public void UpdateGridData(string ReqNum, string Vendor, int Status, string Reason)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string userID = (string)HttpContext.Current.Session["UserName"].ToString();
                
                if (Status == 1)
                {

                    if (Reason.ToString() != "")
                    {
                        try
                        {

                            DataTable Result1 = new DataTable();
                            SqlDataAdapter da1 = new SqlDataAdapter();
                            string pic = nameC.ToString() + " - " + Reason.ToString();
                            string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "',   UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'";

                            da1 = new SqlDataAdapter(str1, EmetCon);
                            Result1 = new DataTable();
                            da1.Fill(Result1);

                            DataTable Result11 = new DataTable();
                            SqlDataAdapter da11 = new SqlDataAdapter();
                            string pic1 = nameC.ToString() + " - " + Reason.ToString();
                            string str11 = "Update TQuoteDetails SET  ManagerApprovalStatus= '" + 1 + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                            da11 = new SqlDataAdapter(str11, EmetCon);
                            Result11 = new DataTable();
                            da11.Fill(Result11);
                        }
                        catch (Exception ex)
                        {
                            LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                            EMETModule.SendExcepToDB(ex);
                        }
                        
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Rejected Successfully');", true);
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Please fill the Reason for Reject');", true);
                    }
                }





                if (Status == 2)
                {
                    try
                    {

                        DataTable Result = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter();

                        string str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',PICApprovalStatus = '" + Status + "', ManagerApprovalStatus = '" + Status + "', DIRApprovalStatus = '" + Status + "',  ManagerReason = '" + Reason + "', DIRReason = '" + Reason + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                        //da = new SqlDataAdapter(str, con1);
                        //Result = new DataTable();
                        //da.Fill(Result);

                        //string reason1 = "Auto Rejected By PIC";

                        //str = "Update TQuoteDetails SET PICApprovalStatus = '" + 1 + "',ManagerApprovalStatus = '" + 0 + "', PICReason = '" + reason1 + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 not in('" + Vendor + "') and (PICApprovalStatus = '" + 0 + "' or PICApprovalStatus is null)";

                        //da = new SqlDataAdapter(str, con1);
                        //Result = new DataTable();
                        //da.Fill(Result);

                        string pic = nameC.ToString() + " - " + Reason.ToString();
                        str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',PICApprovalStatus = '" + Status + "',  ManagerApprovalStatus = '" + Status + "', DIRApprovalStatus = '" + Status + "',  ManagerReason = '" + pic + "', DIRReason = '" + pic + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                        da = new SqlDataAdapter(str, EmetCon);
                        Result = new DataTable();
                        da.Fill(Result);

                        DataTable Result11 = new DataTable();
                        SqlDataAdapter da11 = new SqlDataAdapter();
                        string pic1 = nameC.ToString() + " - " + Reason.ToString();
                        string str11 = "Update TQuoteDetails SET  ManagerApprovalStatus= '" + Status + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                        da11 = new SqlDataAdapter(str11, EmetCon);
                        Result11 = new DataTable();
                        da11.Fill(Result11);
                    }
                    catch (Exception ex)
                    {
                        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                        EMETModule.SendExcepToDB(ex);
                    }

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
                        plant.Value = Session["VPlant"].ToString();
                        cmdget.Parameters.Add(plant);


                        SqlDataReader dr;
                        dr = cmdget.ExecuteReader();
                        pemail1 = string.Empty;
                        while (dr.Read())
                        {

                            pemail1 = string.Concat(pemail1, dr.GetString(0), ";");

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

                        while (dr.Read())
                        {
                            aemail = string.Concat(aemail, dr.GetString(0), ";");
                            //pemail = dr.GetString(1);

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

                        while (dr.Read())
                        {
                            demail = string.Concat(demail, dr.GetString(0), ";");
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
                        vendorid.Value = userId1;
                        cmdget.Parameters.Add(vendorid);

                        SqlDataReader dr;
                        dr = cmdget.ExecuteReader();
                        while (dr.Read())
                        {
                            Uemail = dr.GetString(0);
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

                        }
                        qdr.Dispose();
                        Qcnn.Dispose();
                    }


                    cc = string.Concat(pemail1, aemail, demail, Uemail);


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
                        Quote.Value = quoteno.ToString();
                        cmdget.Parameters.Add(Quote);

                        SqlDataReader dr_cus;
                        dr_cus = cmdget.ExecuteReader();
                        pemail1 = string.Empty;
                        while (dr_cus.Read())
                        {

                            customermail = dr_cus.GetString(0);
                            customermail1 = dr_cus.GetString(1);
                            customermail = string.Concat(customermail, ";", customermail1);

                            //while start

                            //email
                            // getting Messageheader ID from IT Mailapp
                            using (SqlConnection MHcnn = new SqlConnection(EMETModule.GenMailConnString()))
                            {
                                string returnValue1 = string.Empty;
                                MHcnn.Open();
                                SqlCommand MHcmdget = MHcnn.CreateCommand();
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
                                returnValue1 = pOutput.Value.ToString();
                                MHcnn.Dispose();
                                OriginalFilename = returnValue1;
                                MHid = returnValue1;
                                OriginalFilename = MHid + seqNo + formatW;
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
                                vendorid_.Value = quoteno.ToString();
                                cmdget_.Parameters.Add(vendorid_);

                                SqlDataReader dr_;
                                dr_ = cmdget_.ExecuteReader();
                                while (dr_.Read())
                                {
                                    body1 = dr_.GetString(1);
                                }
                                dr_.Dispose();
                                cnn_.Dispose();
                            }
                            // Insert header and details to Mil server table to IT mailserverapp
                            using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                            {
                                Email_inser.Open();
                                //Header
                                string MessageHeaderId = MHid;
                                string fromname = "eMET System";
                                string FromAddress = Uemail;
                                //string Recipient = aemail + "," + pemail;
                                string Recipient = customermail;
                                string CopyRecipient = cc;
                                string BlindCopyRecipient = "";
                                string ReplyTo = "subashdurai@shimano.com.sg";
                                string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + quoteno.ToString() + " - Shimano Approval By : " + nameC + " - Plant : " + Session["EPlant"].ToString();
                                //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + body1.ToString();
                                string BodyFormat = "HTML";
                                string BodyRemark = "0";
                                string Signature = " ";
                                string Importance = "High";
                                string Sensitivity = "Confidential";

                                string CreateUser = userId1;
                                DateTime CreateDate = DateTime.Now;
                                //end Header
                                string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                SqlCommand Header = new SqlCommand(Head, Email_inser);
                                Header.Parameters.AddWithValue("@MessageHeaderId", MessageHeaderId.ToString());
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
                                Header.Parameters.AddWithValue("@CreateUser", userId1.ToString());
                                Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                Header.CommandText = Head;
                                Header.ExecuteNonQuery();
                                //end Header
                                //Details

                                string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                Detail.Parameters.AddWithValue("@MessageHeaderId", MHid.ToString());
                                Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                Detail.Parameters.AddWithValue("@CreateUser", userId1.ToString());
                                Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                Detail.CommandText = Details;
                                Detail.ExecuteNonQuery();
                                Email_inser.Dispose();
                                //End Details
                            }

                            //while end

                        }
                        dr_cus.Dispose();
                        cnn.Dispose();
                    }



                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Approved Successfully and Request Moved to next Level');", true);

                    //End by subash

                    //end of email
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
                EMETModule.SendExcepToDB(ex); ;
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
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlTaskStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtShowEntry_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();CloseLoading();", true);
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

                ShowTable();
                txtFind.Focus();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["ReqWaitFilterMassRev"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilterMassRev"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdlTaskStatus.SelectedValue;
                    Session["ReqWaitFilterMassRev"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
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
                TxtFrom.Text = "";
                TxtTo.Text = "";
                txtFind.Text = "";
                Session["ReqWaitFilterMassRev"] = null;
                Session["ReqWaitNst"] = null;
                ShowTable();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvlDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                string[] CmdArg = (e.CommandArgument).ToString().Split('|');

                if (e.CommandName == "LinktoRedirect")
                {
                    GridViewRow Gv2Row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    GridView Childgrid = (GridView)(Gv2Row.Parent.Parent);
                    GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);

                    int ParentIdx = Gv1Row.RowIndex;

                    //int GvMainIdx = Convert.ToInt32(CmdArg[0]);
                    //int GvDetIdx = Convert.ToInt32(CmdArg[1]);

                    int GvMainIdx = ParentIdx;
                    int GvDetIdx = Gv2Row.RowIndex;
                    GridView GvDet = (GridView)GridView1.Rows[GvMainIdx].FindControl("GvDet");
                    if (GvDet != null)
                    {
                        string Quote = ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString();
                        string Vcode = GvDet.Rows[GvDetIdx].Cells[1].Text.ToString();
                        string VName = GvDet.Rows[GvDetIdx].Cells[2].Text.ToString();
                        string isDraft = GvDet.Rows[GvDetIdx].Cells[4].Text.ToString();
                        
                        using (SqlConnection con = new SqlConnection(EMETModule.GenEMETConnString()))
                        {
                            using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                            {
                                cmd.Connection = con;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 7;
                                cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quote;
                                
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    DataTable pir = new DataTable();
                                    sda.Fill(pir);
                                    //int a= pir.Rows
                                    if (pir.Rows.Count > 0)
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Quotation Response Due date Expired.  Change Response Due Date');CloseLoading();", true);
                                    }
                                    else
                                    {
                                        Session["mappedVendor"] = Vcode;
                                        Session["mappedVname"] = VName;
                                        if (isDraft.ToUpper() == "NEW")
                                        {
                                            Response.Redirect("NewReq_changesMass.aspx?Number=" + Quote);
                                        }
                                        else
                                        {
                                            Response.Redirect("Review_reqMass.aspx?Number=" + Quote);
                                        }
                                    }
                                }
                            }
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string CurentDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime DtCurentDate = DateTime.ParseExact(CurentDate, "dd/MM/yyyy", null);
                DateTime DtDueDate = DateTime.ParseExact(TxtModalDueDate.Text, "dd/MM/yyyy", null);

                int result = DateTime.Compare(DtDueDate, DtCurentDate);
                if ((result > 0) && (TxtModalDueDate.Text != ""))
                {
                    sql = "update TQuoteDetails set QuoteResponseDueDate = @QuoteResponseDueDate where RequestNumber = @RequestNumber ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", TxtModalReqNo.Text);
                    cmd.Parameters.AddWithValue("@QuoteResponseDueDate", DtDueDate.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                    ShowTable();
                    UpdatePanel1.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Updated !');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Date should be greater than current date');", true);
                }

                if (Session["ReqWaitNst"] != null)
                {
                    string RowVsStatus = Session["ReqWaitNst"].ToString();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
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
            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
        }

        protected void BtnCekLatestNested_Click(object sender, EventArgs e)
        {
            IsFirstLoad.Text = "2";
            if (Session["ReqWaitNst"] != null)
            {
                string RowVsStatus = Session["ReqWaitNst"].ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');DatePitcker();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
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