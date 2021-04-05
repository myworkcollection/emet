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
    public partial class ReqWaitVndMass : System.Web.UI.Page
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
        string DbMasterName = "";
        bool sendingemail = false;

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
                    if (Session["sidebarToggle"] == null)
                    {
                        SideBarMenu.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        SideBarMenu.Attributes.Add("style", "display:none;");
                    }

                    if (!Page.IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_VndWithSAPCodeMass";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            string userId = Session["userID_"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string mappeduserid = Session["mappedVendor"].ToString();
                            string mappedname = Session["mappedVname"].ToString();

                            string concat = sname + " - " + mappedname;
                            lblUser.Text = sname;
                            lblplant.Text = mappedname;
                            LbVendorCode.Text = GetVendorDesc();
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                            {
                                if (Request.QueryString["Num"] == "1")
                                {
                                    DdltaskStatus.SelectedValue = "MassRevision";
                                    DvFilterDiffrence.Visible = false;
                                }
                                else if (Request.QueryString["Num"] == "2")
                                {
                                    DdltaskStatus.SelectedValue = "MassRevision-Draft";
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
                            }

                            LastFilterCondition();
                            if (Session["ShowEntryRequestfromSMN"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryRequestfromSMN"].ToString();
                            }
                            if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                if (Session["VndReqWaitFilterMass"] != null)
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
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

                    DdltaskStatus.SelectedValue = ArrFilter[6].ToString();

                    if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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

                        DvFilterDiffrence.Visible = true;
                        ShowTableMassRev();
                        DvMassRev.Visible = true;
                        DvNormalData.Visible = false;
                    }
                    else
                    {
                        DvFilterDiffrence.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername() ;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }

        string GetVendorDesc()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string GetVendorDesc = "";
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = " select Description from tVendor_New where Vendor='" + Session["mappedVendor"].ToString() + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    GetVendorDesc = Session["mappedVendor"].ToString() + "-" + DtDataMacVnd.Rows[0]["Description"].ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }

            return GetVendorDesc;
        }

        protected void ShowTable()
        {
            string mappeduserid = Session["mappedVendor"].ToString();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select A.Plant,A.RequestNumber, 
                        CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate, QuoteNo, CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate, 
                        CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                        A.Product,A.Material,A.MaterialDesc,A.VendorCode1,A.VendorName ,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                        SMNPicDept as 'UseDep',
                        CASE
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.QuoteNoRef is null) and (A.isMassRevision = 1) THEN 'Mass Revision'
                        --WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.QuoteNoRef is null) and (A.isMassRevision = 1) THEN 'Mass Revision - Draft'
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.QuoteNoRef is null) and (A.isMassRevision = 1) THEN 'Mass Revision - Draft'
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.QuoteNoRef is null THEN 'New'
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.QuoteNoRef is not null THEN 'Revision'
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and A.QuoteNoRef is null THEN 'Draft'
                        WHEN (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and A.QuoteNoRef is not null THEN 'Revision - Draft'
                        ELSE 'error'
                         END AS TaskStatus
                        from TQuoteDetails A Where A.vendorcode1 = '" + mappeduserid.ToString() + @"' and A.ApprovalStatus = 0 and isnull(A.CreateStatus,'') <> ''
                        and A.Plant = '" + Session["VPlant"].ToString() + @"' and A.isUseSAPCode = 1 
                        and (A.RequestNumber not in (select RequestNumber from TQuoteDetails_D where isMassRevision = 1) ) and (isMassRevision = 1)
                        ";
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

                    //if (DdltaskStatus.SelectedValue.ToString() == "New")
                    //{
                    //    sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null)";
                    //}
                    //else if (DdltaskStatus.SelectedValue.ToString() == "Draft")
                    //{
                    //    sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null)";
                    //}
                    //else if (DdltaskStatus.SelectedValue.ToString() == "Revision")
                    //{
                    //    sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.QuoteNoRef is not null ";
                    //}
                    //else if (DdltaskStatus.SelectedValue.ToString() == "Revision-Draft")
                    //{
                    //    sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and A.QuoteNoRef is not null ";
                    //}
                    //else 
                    if (DdltaskStatus.SelectedValue.ToString() == "MassRevision")
                    {
                        sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and (A.QuoteNoRef is null) and (A.isMassRevision = 1) ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "MassRevision-Draft")
                    {
                        sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL and (A.QuoteNoRef is null) and (A.isMassRevision = 1) ";
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
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode1")
                        {
                            sql += @" and VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorName")
                        {
                            sql += @" and VendorName like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "QuoteNo")
                        {
                            sql += @" and QuoteNo like '%'+@Filter+'%' ";
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
                            sql += @" and A.ProcessGroup like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                        {
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = A.ProcessGroup) like '%'+@Filter+'%' ";
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
                        Session["ShowEntryRequestfromSMN"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["VndReqWaitPgNoMass"] != null)
                            {
                                int VndReqWaitPgNo = Convert.ToInt32(Session["VndReqWaitPgNoMass"].ToString());
                                if (GridView1.PageCount >= VndReqWaitPgNo)
                                {
                                    GridView1.PageIndex = VndReqWaitPgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["VndReqWaitPgNoMass"] = null;
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

        bool IsResubmit(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                bool IsResubmit = false;
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = "select IsReSubmit from TQuoteDetails where QuoteNo= @QuoteNo";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["IsReSubmit"].ToString() == "True")
                            {
                                IsResubmit = true;
                            }
                        }
                    }
                }
                if (IsResubmit == true)
                {
                    return true;
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

        private void Lnkbutton_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewReq_changes.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)sender).Text);
        }

        //protected void grdVendrRequest_DataBound(object sender, EventArgs e)
        //{

        //    for (int rowIndex = grdVendrRequest.Rows.Count - 2;
        //                                   rowIndex >= 0; rowIndex--)
        //    {
        //        GridViewRow gvRow = grdVendrRequest.Rows[rowIndex];
        //        GridViewRow gvPreviousRow = grdVendrRequest.Rows[rowIndex + 1];
        //        for (int cellCount = 0; cellCount < gvRow.Cells.Count;
        //                                                      cellCount++)
        //        {
        //            if (gvRow.Cells[0].Text == gvPreviousRow.Cells[0].Text)
        //                    {

        //            if (gvRow.Cells[cellCount].Text ==
        //                                   gvPreviousRow.Cells[cellCount].Text)
        //            {
        //                if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
        //                {
        //                    gvRow.Cells[cellCount].RowSpan = 2;
        //                }
        //                else
        //                {
        //                    gvRow.Cells[cellCount].RowSpan =
        //                        gvPreviousRow.Cells[cellCount].RowSpan + 1;
        //                }
        //                gvPreviousRow.Cells[cellCount].Visible = false;
        //            }
        //        }


        //        }




        //    }


        //}


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                Session["VndReqWaitPgNoMass"] = (GridView1.PageIndex).ToString();
                ShowTable();
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "LinktoRedirect")
                {

                    //new Version
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string rnumber = row.Cells[1].Text;
                    string QuoteNo = row.Cells[7].Text;
                    string isDraft = row.Cells[10].Text;

                    GridViewRow row1 = GridView1.Rows[rowIndex];
                    string Quote = row.Cells[7].Text;
                    
                    using (SqlConnection con = new SqlConnection(EMETModule.GenEMETConnString()))
                    {

                        //start
                        using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                        {
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 7;
                            cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = QuoteNo;
                            
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                DataTable pir = new DataTable();
                                sda.Fill(pir);
                                //int a= pir.Rows
                                if (pir.Rows.Count > 0)
                                {
                                    //benable = 1;
                                    if (IsResubmit(QuoteNo) == true)
                                    {
                                        if (isDraft.ToUpper() == "NEW" || isDraft.ToUpper() == "REVISION")
                                        {
                                            Response.Redirect("NewReq_changes.aspx?Number=" + QuoteNo);
                                        }
                                        else if (isDraft.ToUpper() == "MASS REVISION")
                                        {
                                            Response.Redirect("VViewRequest.aspx?Number=" + QuoteNo);
                                        }
                                        else
                                        {
                                            Response.Redirect("Review_req.aspx?Number=" + QuoteNo);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Quotation Response Due date Expired.  Please contact Shimano Admin');CloseLoading();", true);
                                    }
                                    

                                }
                                else
                                {
                                    //benable = 0;
                                    if (isDraft.ToUpper() == "NEW" || isDraft.ToUpper() == "REVISION")
                                    {
                                        Response.Redirect("NewReq_changes.aspx?Number=" + QuoteNo);
                                    }
                                    else if (isDraft.ToUpper() == "MASS REVISION")
                                    {
                                        Response.Redirect("VViewRequest.aspx?Number=" + QuoteNo);
                                    }
                                    else
                                    {
                                        Response.Redirect("Review_req.aspx?Number=" + QuoteNo);
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
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
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            return sortDirection;
        }

        protected void DdltaskStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue.ToString();
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void DdlFltrDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtFrom.Text = "";
                TxtTo.Text = "";
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void DdlDiffrence_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtFrom.Text = "";
                TxtTo.Text = "";
                TxtSglConditionDiff.Text = "";
                TxtDiffrence1.Text = "";
                TxtDiffrence2.Text = "";
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                }
                else
                {
                    ShowTable();
                    DvMassRev.Visible = false;
                    DvNormalData.Visible = true;
                    DdlDiffrence.SelectedValue = "0";
                }

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void DdlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtFind.Text = "";
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void TxtFrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void TxtTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (DdltaskStatus.SelectedValue == "MassRevision-Draft")
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
                txtFind.Focus();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilterMass"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilterMass"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilterMass"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            DdlFltrDate.SelectedIndex = 0;
            DdlFilterBy.SelectedIndex = 0;
            DdltaskStatus.SelectedIndex = 0;
            TxtFrom.Text = "";
            TxtTo.Text = "";
            txtFind.Text = "";
            Session["VndReqWaitFilterMass"] = null;
            Session["VndReqWaitPgNoMass"] = null;
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

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModalSession", "$('#myModalSession').modal('hide');", true);
                TimerCntDown.Enabled = false;
                countdown.Text = "30";
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
            catch (Exception ex2)
            {
                Response.Write(ex2);
                EMETModule.SendExcepToDB(ex2);
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
                    sql = @" select distinct Plant,RequestNumber,
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                            (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            Product,Material,MaterialDesc,
                            VendorCode1,substring((VendorName),1,12) +' ...' as VendorName,QuoteNo,
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,

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


                            case 
                            when ApprovalStatus = 0 then 'Pending'
                            when ApprovalStatus = 3 then 'Approved / Closed'
                            when (ApprovalStatus = 2 and FinalQuotePrice = '' or ApprovalStatus = 2 and FinalQuotePrice is not null) then 'Submitted'
                            when (ApprovalStatus = 2 and FinalQuotePrice = '' or ApprovalStatus = 2 and FinalQuotePrice IS NULL) then 'Auto Close-SMN'
                             when (ApprovalStatus = 0 and FinalQuotePrice = '' or ApprovalStatus = 0 and FinalQuotePrice is null) then 'Pending'
                            else 'cannot find status'
                            end as 'ResponseStatus',
                            --ApprovalStatus, 
                            PICApprovalStatus, 
                            case 
                            when pirstatus is null or pirstatus = '' then 'Waiting for Request completion from all vendors'
                            else 'Ready for process' 
                            end as 'pirstatus',
                            --isnull(pirstatus,'U') as pirstatus,
                            format(EmpSubmitionOn,'dd/MM/yyyy') as UpdatedOn,
                            (select distinct UseNam from " + DbMasterName + @".dbo.Usr where UseID=EmpSubmitionBy) as 'Updatedby',
                            (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep'
                            from TQuoteDetails_D A 
                            where A.Plant  = '" + Session["VPlant"].ToString() + @"' and A.ApprovalStatus = 0 and isnull(A.CreateStatus,'') <> ''
                            --RequestNumber in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 0) 
                            and  (A.isMassRevision = 1) and A.vendorcode1 = '" + Session["mappedVendor"].ToString() + @"'";

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

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        dt.Columns.Add("App");
                        //dt.Columns.Add("Rej");
                        Session["DtMassRev"] = dt;
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
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }


        protected void CheckAllAprOrRej()
        {
            try
            {
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
                    if (dt.Rows.Count > 0)
                    {
                        int CApr = 0;
                        //int CRej = 0;
                        bool AllEmpty = true;
                        CheckBox RbHeaderApp = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllApp");
                        //CheckBox RbHeaderRej = (CheckBox)GdvMassRev.HeaderRow.FindControl("RbAllRej");
                        if (RbHeaderApp != null /*&& RbHeaderRej != null*/)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["App"].ToString() == "1")
                                {
                                    CApr++;
                                }
                                //else if (dt.Rows[i]["App"].ToString() == "0")
                                //{
                                //    CRej++;
                                //}
                            }

                            if (CApr == 0 /*&& CRej == 0*/)
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
                                    //RbHeaderRej.Checked = false;
                                }
                                //else if (CRej == dt.Rows.Count)
                                //{
                                //    RbHeaderApp.Checked = false;
                                //    RbHeaderRej.Checked = true;
                                //}
                            }
                            else
                            {
                                RbHeaderApp.Checked = false;
                                //RbHeaderRej.Checked = false;
                            }
                            LbTotalRecMassRev.Text = "Tot Record : " + dt.Rows.Count.ToString();
                            LbTotUncheck.Text = "Tot Uncheck : " + (dt.Rows.Count - CApr /*- CRej*/).ToString();
                            LbTotApp.Text = "Tot Approve Checked : " + CApr.ToString();
                            //LbTotRej.Text = "Tot Reject Checked : " + CRej.ToString();
                        }
                    }
                    else
                    {
                        LbTotalRecMassRev.Text = "Tot Record : 0";
                        LbTotUncheck.Text = "Tot Uncheck : 0";
                        LbTotApp.Text = "Tot Approve Checked 0: ";
                        //LbTotRej.Text = "Tot Reject Checked 0: ";
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void TxtShowEntMassRev_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
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
                    Session["TxtShowEntMassRev"] = TxtShowEntMassRev.Text;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();", true);
            }
            catch (Exception ee)
            {
                Response.Write(e);
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GdvMassRev_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int Idx = e.Row.DataItemIndex;
                    CheckBox RbApp = e.Row.FindControl("RbApp") as CheckBox;
                    //CheckBox RbRej = e.Row.FindControl("RbRej") as CheckBox;
                    string ReqNo = e.Row.Cells[3].Text;
                    if (Session["DtMassRev"] != null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)Session["DtMassRev"];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["RequestNumber"].ToString() == ReqNo)
                            {
                                if (dt.Rows[i]["App"].ToString() == "1")
                                {
                                    RbApp.Checked = true;
                                    //RbRej.Checked = false;
                                }
                                else if (dt.Rows[i]["App"].ToString() == "0")
                                {
                                    RbApp.Checked = false;
                                    //RbRej.Checked = true;
                                }
                                else
                                {
                                    RbApp.Checked = false;
                                    //RbRej.Checked = false;
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
                Session["MassRevPgNo"] = (GdvMassRev.PageIndex).ToString();
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
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
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
            }
        }

        protected void GdvMassRev_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
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
                    Session["DtMassRev"] = dt;
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
                        if (cellHeaderidx > 1)
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
                    Response.Redirect("VViewRequestMassRev.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString(), false);
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
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (RbHeaderApp.Checked == true)
                        {
                            dt.Rows[i]["App"] = 1;
                            //dt.Rows[i]["Rej"] = 0;
                        }
                        else if (RbHeaderApp.Checked == false)
                        {
                            dt.Rows[i]["App"] = "";
                            //dt.Rows[i]["Rej"] = "";
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRev"] = dt;
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
                string ReqNo = GdvMassRev.Rows[idx].Cells[3].Text;
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["RequestNumber"].ToString() == ReqNo)
                        {
                            if (RbApp.Checked == true)
                            {
                                dt.Rows[i]["App"] = 1;
                                //dt.Rows[i]["Rej"] = 0;
                            }
                            else if (RbApp.Checked == false)
                            {
                                dt.Rows[i]["App"] = "";
                                //dt.Rows[i]["Rej"] = "";
                            }
                        }
                    }
                    dt.AcceptChanges();
                    Session["DtMassRev"] = dt;
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
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
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
                    Session["DtMassRev"] = dt;
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
                if (Session["DtMassRev"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["DtMassRev"];
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
                    Session["DtMassRev"] = dt;
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
            try
            {
                EmetCon.Open();
                if (Session["DtMassRev"] != null)
                {
                    DataTable dtMassRev = (DataTable)Session["DtMassRev"];
                    if (dtMassRev.Rows.Count > 0)
                    {
                        sql = "";
                        for (int m = 0; m < dtMassRev.Rows.Count; m++)
                        {
                            string ReqNumber = dtMassRev.Rows[m]["RequestNumber"].ToString();
                            string QuNo = dtMassRev.Rows[m]["QuoteNo"].ToString();
                            if (dtMassRev.Rows[m]["App"].ToString() == "1")
                            {
                                sql += @"  update TQuoteDetails set 
                                            TotalMaterialCost = (select TotalMaterialCost from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            TotalSubMaterialCost = (select TotalSubMaterialCost from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            TotalProcessCost = (select TotalProcessCost from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            TotalOtheritemsCost = (select TotalOtheritemsCost from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            GrandTotalCost = (select GrandTotalCost from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            FinalQuotePrice = (select FinalQuotePrice from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            Profit = (select Profit from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            Discount = (select Discount from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            UpdatedBy = '" + Session["userID_"].ToString() + @"',
                                            UpdatedOn = CURRENT_TIMESTAMP,
                                            CountryOrg = (select CountryOrg from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            CommentByVendor = (select CommentByVendor from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            EmpSubmitionOn = (select EmpSubmitionOn from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            EmpSubmitionBy = (select EmpSubmitionBy from TQuoteDetails_D where QuoteNo='" + QuNo + @"'),
                                            ApprovalStatus = '2' , PICApprovalStatus=0
                                            where QuoteNo='" + QuNo + @"'

                                            delete from TMCCostDetails where QuoteNo='" + QuNo + @"'
                                            delete from TOtherCostDetails where QuoteNo='" + QuNo + @"'
                                            delete from TProcessCostDetails where QuoteNo='" + QuNo + @"'
                                            delete from TSMCCostDetails where QuoteNo='" + QuNo + @"'

                                            INSERT INTO TMCCostDetails SELECT * FROM TMCCostDetails_D WHERE QuoteNo='" + QuNo + @"';
                                            INSERT INTO TOtherCostDetails SELECT * FROM TOtherCostDetails_D WHERE QuoteNo='" + QuNo + @"';
                                            INSERT INTO TProcessCostDetails SELECT * FROM TProcessCostDetails_D WHERE QuoteNo='" + QuNo + @"';
                                            INSERT INTO TSMCCostDetails SELECT * FROM TSMCCostDetails_D WHERE QuoteNo='" + QuNo + @"';

                                            delete from TMCCostDetails_D where QuoteNo='" + QuNo + @"'
                                            delete from TOtherCostDetails_D where QuoteNo='" + QuNo + @"'
                                            delete from TProcessCostDetails_D where QuoteNo='" + QuNo + @"'
                                            delete from TSMCCostDetails_D where QuoteNo='" + QuNo + @"'
                                            delete from TQuoteDetails_D where QuoteNo='" + QuNo + @"'
                                          ";
                            }
                        }
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
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
                return false;
            }
            finally
            {
                EmetCon.Dispose();
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

        public void getcreateusermail(string ReqNum, string Vendor)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                getcreateusermail1(ReqNum, Vendor);
                MDMCon.Open();
                DataTable Result111 = new DataTable();
                SqlDataAdapter da111 = new SqlDataAdapter();

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
            finally
            {
                MDMCon.Dispose();
            }
        }

        private void Getcreateuser()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select UseEmail from usr where UseID = '" + Session["userID_"].ToString() + "' and DELFLAG = 0 ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    Session["Createuser"] = dtdate.Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    Session["Createuser"] = "";
                }

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        bool ProcsendingEmailMassRev()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                if (Session["DtMassRev"] != null)
                {
                    DataTable dtMassRev = (DataTable)Session["DtMassRev"];
                    if (dtMassRev.Rows.Count > 0)
                    {
                        Getcreateuser();
                        string MsgErr = "";

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        string UserId = Session["userID_"].ToString();
                        IsAttachFile = false;
                        Session["SendFilename"] = "NOFILE";
                        OriginalFilename = "NOFILE";
                        format = "NO";


                        sql = "";
                        for (int m = 0; m < dtMassRev.Rows.Count; m++)
                        {
                            string ReqNum = dtMassRev.Rows[m]["RequestNumber"].ToString();
                            string QuNo = dtMassRev.Rows[m]["QuoteNo"].ToString();
                            string Vendor = dtMassRev.Rows[m]["VendorCode1"].ToString();
                            getcreateusermail(ReqNum, Vendor);
                            if (dtMassRev.Rows[m]["App"].ToString() == "1")
                            {
                                // getting Messageheader ID from IT Mailapp
                                #region getting Messageheader ID from IT Mailapp
                                using (SqlConnection cnn = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    string returnValue = string.Empty;
                                    cnn.Open();
                                    SqlTransaction transactionHS;
                                    transactionHS = cnn.BeginTransaction("HeaderSelection");
                                    try
                                    {
                                        SqlCommand cmdget = cnn.CreateCommand();

                                        cmdget.Transaction = transactionHS;
                                        cmdget.CommandType = CommandType.StoredProcedure;
                                        cmdget.CommandText = "dbo.spGetControlNumber";

                                        SqlParameter CompanyCode = new SqlParameter("@pCompanyCode", SqlDbType.Int);
                                        CompanyCode.Direction = ParameterDirection.Input;
                                        CompanyCode.Value = 1;
                                        cmdget.Parameters.Add(CompanyCode);

                                        SqlParameter ControlField = new SqlParameter("@pControlField", SqlDbType.VarChar, 50);
                                        ControlField.Direction = ParameterDirection.Input;
                                        ControlField.Value = "MessageHeaderID";
                                        cmdget.Parameters.Add(ControlField);

                                        SqlParameter Param1 = new SqlParameter("@pParameter1", SqlDbType.VarChar, 50);
                                        Param1.Direction = ParameterDirection.Input;
                                        Param1.Value = "";
                                        cmdget.Parameters.Add(Param1);

                                        SqlParameter Param2 = new SqlParameter("@pParameter2", SqlDbType.VarChar, 50);
                                        Param2.Direction = ParameterDirection.Input;
                                        Param2.Value = "";
                                        cmdget.Parameters.Add(Param2);

                                        SqlParameter Param3 = new SqlParameter("@pParameter3", SqlDbType.VarChar, 50);
                                        Param3.Direction = ParameterDirection.Input;
                                        Param3.Value = "";
                                        cmdget.Parameters.Add(Param3);

                                        SqlParameter Param4 = new SqlParameter("@pParameter4", SqlDbType.VarChar, 50);
                                        Param4.Direction = ParameterDirection.Input;
                                        Param4.Value = "";
                                        cmdget.Parameters.Add(Param4);

                                        SqlParameter pOutput = cmdget.Parameters.Add("@pOutput", SqlDbType.VarChar, 50);
                                        pOutput.Direction = ParameterDirection.Output;

                                        cmdget.ExecuteNonQuery();
                                        transactionHS.Commit();
                                        returnValue = pOutput.Value.ToString();

                                        OriginalFilename = returnValue;
                                        MHid = returnValue;
                                        Session["MHid"] = returnValue;
                                        OriginalFilename = MHid + seqNo + formatW;
                                    }

                                    ///
                                    catch (Exception xw)
                                    {
                                        EMETModule.SendExcepToDB(xw);
                                        transactionHS.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    cnn.Dispose();
                                }
                                #endregion

                                //getting vendor mail id
                                #region getting vendor mail id
                                using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                                {
                                    string returnValue = string.Empty;
                                    cnn.Open();
                                    SqlCommand cmdget = cnn.CreateCommand();
                                    cmdget.CommandType = CommandType.StoredProcedure;
                                    cmdget.CommandText = "dbo.Emet_Email_vendordetails1";

                                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                    vendorid.Direction = ParameterDirection.Input;
                                    vendorid.Value = QuNo;
                                    cmdget.Parameters.Add(vendorid);

                                    SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                    plant.Direction = ParameterDirection.Input;
                                    plant.Value = Session["VPlant"].ToString();
                                    cmdget.Parameters.Add(plant);

                                    SqlDataReader dr;
                                    dr = cmdget.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        // aemail = dr.GetString(0);
                                        // pemail = dr.GetString(1);
                                        // Session["Uemail"] = dr.GetString(0);
                                        aemail = dr.GetString(1);
                                        Session["aemail"] = dr.GetString(1);
                                        pemail = dr.GetString(2);
                                        Session["pemail"] = string.Concat(aemail, ";", pemail);

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
                                    cmdget.CommandText = "dbo.Emet_Email_Vendormaildid";
                                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                                    vendorid.Direction = ParameterDirection.Input;
                                    vendorid.Value = UserId;
                                    cmdget.Parameters.Add(vendorid);

                                    SqlDataReader dr;
                                    dr = cmdget.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        Uemail = dr.GetString(0);
                                        Session["Uemail"] = dr.GetString(0);

                                    }
                                    dr.Dispose();
                                    cnn.Dispose();
                                }
                                #endregion

                                //getting vendor mail content
                                #region getting vendor mail content
                                using (SqlConnection cnn = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    string returnValue = string.Empty;
                                    cnn.Open();
                                    SqlCommand cmdget = cnn.CreateCommand();
                                    cmdget.CommandType = CommandType.StoredProcedure;
                                    cmdget.CommandText = "dbo.Emet_Email_content";

                                    SqlParameter vendorid = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                                    vendorid.Direction = ParameterDirection.Input;
                                    vendorid.Value = QuNo;
                                    cmdget.Parameters.Add(vendorid);

                                    SqlDataReader dr;
                                    dr = cmdget.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        body1 = dr.GetString(1);
                                        Session["body1"] = dr.GetString(1);
                                    }
                                    dr.Dispose();
                                    cnn.Dispose();
                                }
                                #endregion

                                // Insert header and details to Mil server table to IT mailserverapp
                                #region Insert header and details to Mil server table to IT mailserverapp
                                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                                {
                                    Email_inser.Open();
                                    //Header

                                    UserId = Session["userID_"].ToString();

                                    //Uemail = Session["Uemail"].ToString();
                                    pemail = Session["pemail"].ToString();
                                    body1 = Session["body1"].ToString();
                                    nameC = Session["UserName"].ToString();

                                    string MessageHeaderId = Session["MHid"].ToString();
                                    string fromname = "eMET System";
                                    //string FromAddress = Uemail;
                                    string FromAddress = "eMET@Shimano.Com.sg";
                                    nameC = Session["UserName"].ToString();
                                    //string Recipient = aemail + "," + pemail;
                                    string Recipient = Session["pemail"].ToString();
                                    string CopyRecipient = Session["Createuser"].ToString();
                                    string BlindCopyRecipient = "";
                                    string ReplyTo = "subashdurai@shimano.com.sg";
                                    string Subject = "Quotation Number: " + QuNo + " - Confirm By : " + nameC + " - Plant : " + Session["VPlant"].ToString();
                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Confirm.<br /><br />" + body1.ToString();
                                    string BodyFormat = "HTML";
                                    string BodyRemark = "0";
                                    string Signature = " ";
                                    string Importance = "High";
                                    string Sensitivity = "Confidential";

                                    //string CreateUser = userId;
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
                                        Header.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                                        Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Header.CommandText = Head;
                                        Header.ExecuteNonQuery();

                                        transactionHe.Commit();
                                    }

                                    ///
                                    catch (Exception xw)
                                    {
                                        EMETModule.SendExcepToDB(xw);
                                        transactionHe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    //end Header
                                    //Details


                                    SqlTransaction transactionDe;
                                    transactionDe = Email_inser.BeginTransaction("Detail");
                                    try
                                    {
                                        string SendFilename = Session["SendFilename"].ToString();
                                        string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                        SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                        Detail.Transaction = transactionDe;
                                        Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                        Detail.Parameters.AddWithValue("@SequenceNumber", SequenceNumber.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFilename", OriginalFilename.ToString());
                                        Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                        Detail.Parameters.AddWithValue("@SendFilename", SendFilename);
                                        Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                        Detail.Parameters.AddWithValue("@CreateUser", Session["userID_"].ToString());
                                        Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                        Detail.CommandText = Details;
                                        Detail.ExecuteNonQuery();

                                        transactionDe.Commit();
                                    }
                                    catch (Exception xw)
                                    {
                                        EMETModule.SendExcepToDB(xw);
                                        transactionDe.Rollback();
                                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Detail: " + xw + " ");
                                        var script = string.Format("alert({0});window.location ='Vendor.aspx';", message);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    Email_inser.Dispose();
                                    //End Details
                                }
                                #endregion
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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "DatePitcker();CloseLoading();alert('Data Submitted!');", true);
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