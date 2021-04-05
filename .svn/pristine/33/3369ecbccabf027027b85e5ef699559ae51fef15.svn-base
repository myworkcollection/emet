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
    public partial class VWhitoutSAPCodeGp : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        string DbMasterName = "";
        bool IsAth;

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
                    if (!Page.IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "VWhitoutSAPCodeGP";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
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
                                if (Request.QueryString["Num"] == "2")
                                {
                                    DdltaskStatus.SelectedValue = "Submitted-Expired";
                                }
                                else if (Request.QueryString["Num"] == "1")
                                {
                                    DdltaskStatus.SelectedValue = "New-Draft";
                                }
                            }
                            LastFilterCondition();
                            if (Session["ShowEntryVWhitoutSAPCode"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryVWhitoutSAPCode"].ToString();
                            }
                            ShowTable();
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

                if (Session["VndReqWaitFilter"] != null)
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
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
                DbMasterName = EMETModule.GetDbMastername();
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
            string GetVendorDesc = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
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
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            string mappeduserid = Session["mappedVendor"].ToString();
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
                    WHEN ((select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL AND a.ApprovalStatus='4') THEN 'New'
					WHEN ((select QuoteNo from TQuoteDetails where QuoteNo = A.QuoteNo) is not null and A.ApprovalStatus = '5') THEN 'Submitted'
                    WHEN ((select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.ApprovalStatus = '6' ) THEN 'Expired'
                    WHEN ((select QuoteNo from TQuoteDetails where QuoteNo = A.QuoteNo) IS NOT NULL and A.ApprovalStatus = '6') THEN 'Expired' 
                    ELSE 'Draft'
                     END AS TaskStatus
                    from TQuoteDetails A 
                    Where 
                    ( A.vendorcode1 = '" + mappeduserid.ToString() + @"' 
                      and A.Plant = '" + Session["VPlant"].ToString() + @"' 
                      and (SELECT RIGHT(A.QuoteNo, 2)) = 'GP' 
                    ) and isnull(A.CreateStatus,'') <> '' ";
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

                    if (DdltaskStatus.SelectedValue.ToString() == "New")
                    {
                        sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL";
                        sql += @" and A.ApprovalStatus = '4' ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "Draft")
                    {
                        sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "New-Draft")
                    {
                        sql += @" and ( ((select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.ApprovalStatus = '4') or ((select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NOT NULL) ) ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "Submitted")
                    {
                        sql += @" and (select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL ";
                        sql += @" and A.ApprovalStatus = '5' ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "Expired")
                    {
                        sql += @" and ( A.ApprovalStatus='6' ) ";
                    }
                    else if (DdltaskStatus.SelectedValue.ToString() == "Submitted-Expired")
                    {
                        sql += @" and ( ((select QuoteNo from TQuoteDetails_D where QuoteNo = A.QuoteNo) IS NULL and A.ApprovalStatus = '5') or ( ( A.ApprovalStatus='6' ) ) )";
                    }
                    else
                    {
                        sql += @" and ( 
                                ( A.ApprovalStatus='4' ) or 
                                ( A.ApprovalStatus='5' ) or
                                ( A.ApprovalStatus='6' )
                                )";
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
                        Session["ShowEntryVWhitoutSAPCode"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["VndReqWaitPgNo"] != null)
                            {
                                int VndReqWaitPgNo = Convert.ToInt32(Session["VndReqWaitPgNo"].ToString());
                                if (GridView1.PageCount >= VndReqWaitPgNo)
                                {
                                    GridView1.PageIndex = VndReqWaitPgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["VndReqWaitPgNo"] = null;
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
                Session["VndReqWaitPgNo"] = (GridView1.PageIndex).ToString();
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
                if (e.CommandName == "LinktoRedirect")
                {
                    GetDbMaster();
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
                                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Quotation Response Due date Expired.  Please contact Shimano Admin');CloseLoading();", true);

                                }
                                else
                                {
                                    //benable = 0;
                                    if (isDraft.ToUpper() == "NEW")
                                    {
                                        Response.Redirect("NewReq_changes.aspx?Number=" + QuoteNo);
                                    }
                                    else if (isDraft.ToUpper() == "SUBMITTED")
                                    {
                                        //Response.Redirect("VViewRequest.aspx?Number=" + QuoteNo);
                                    }
                                    else if (isDraft.ToUpper() == "EXPIRED")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Quotation Response Due date Expired');CloseLoading();", true);
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    try
                    {
                        int RowParentGv = e.Row.DataItemIndex;
                        string QuoteNo = e.Row.Cells[7].Text;
                        string isDraft = e.Row.Cells[10].Text;

                        LinkButton LbQuoteNo = e.Row.FindControl("LbReqNo") as LinkButton;
                        string url = "VViewRequest.aspx?Number=" + QuoteNo;

                        LbQuoteNo.Attributes.Add("onclick", "openInNewTab('" + url + "','" + isDraft.ToUpper() + "');");
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
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
                ShowTable();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlFltrDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TxtFrom.Text = "";
                TxtTo.Text = "";
                UpdatePanel1.Update();
                ShowTable();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
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
                txtFind.Text = "";
                UpdatePanel1.Update();
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string TaskStatus = "";
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
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
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
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
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
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
                if (Session["VndReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndReqWaitFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    TaskStatus = DdltaskStatus.SelectedValue;
                    Session["VndReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + TaskStatus;
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
            try
            {
                DdlFltrDate.SelectedIndex = 0;
                DdlFilterBy.SelectedIndex = 0;
                DdltaskStatus.SelectedIndex = 0;
                TxtFrom.Text = "";
                TxtTo.Text = "";
                txtFind.Text = "";
                Session["VndReqWaitFilter"] = null;
                Session["VndReqWaitPgNo"] = null;
                ShowTable();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
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