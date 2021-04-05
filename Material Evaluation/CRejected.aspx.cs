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

namespace Material_Evaluation
{
    public partial class CRejected : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        string DbMasterName = "";

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

                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_Rejected";
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
                            LastFilterCondition();
                            if (Session["ShowEntryCRejected"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryCRejected"].ToString();
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

        protected void LastFilterCondition()
        {
            try
            {

                if (Session["VndRejectFilter"] != null)
                {
                    string[] ArrFilter = Session["VndRejectFilter"].ToString().Split('!');
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
                    DdlReqStatus.SelectedValue = ArrFilter[6].ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void CreateSessionFilter()
        {
            try
            {
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string ReqStatus = "All";
                if (Session["VndRejectFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["VndRejectFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndRejectFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["VndRejectFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
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
                    sql = @" select  Plant,RequestNumber, 
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,QuoteNo,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                             Product,Material,MaterialDesc,VendorCode1,VendorName ,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep',
                            case 
                            when A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null) then 'New' 
                            when A.QuoteNoRef is null and (A.isMassRevision = 1) then 'Mass Revision' 
                            else 'Revision' 
                            end as 'ReqStatus'
                             from TQuoteDetails A Where vendorcode1='" + Session["mappedVendor"].ToString() + "' and  (ApprovalStatus = 1) and Plant = '" + Session["VPlant"].ToString() + "' ";

                    if (DdlReqStatus.SelectedValue == "New")
                    {
                        sql += @" and A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null)";
                    }
                    else if (DdlReqStatus.SelectedValue == "Revision")
                    {
                        sql += @" and A.QuoteNoRef is not null";
                    }
                    else if (DdlReqStatus.SelectedValue == "MassRevision")
                    {
                        sql += @" and (A.isMassRevision = 1) ";
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
                        Session["ShowEntryCRejected"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["VndRejectPgNo"] != null)
                            {
                                int VndRejectPgNo = Convert.ToInt32(Session["VndRejectPgNo"].ToString());
                                if (GridView1.PageCount >= VndRejectPgNo)
                                {
                                    GridView1.PageIndex = VndRejectPgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["VndRejectPgNo"] = null;
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

        //protected void ShowTable()
        //{
        //    string userId = Session["userID_"].ToString();
        //    string sname = Session["UserName"].ToString();
        //    string srole = Session["userType"].ToString();

        //    string mappeduserid;
        //    string mappedname;

        //    mappeduserid = Session["mappedVendor"].ToString();
        //    mappedname = Session["mappedVname"].ToString();

        //    string concat = sname + " - " + srole;
        //    lblUser.Text = sname;
        //    lblplant.Text = srole;
        //    //Session["UserName"] = userId;

        //    var connetionString1 = ConfigurationManager.ConnectionStrings["DbconnectionTransaction"].ConnectionString;
        //    SqlConnection con1;
        //    con1 = new SqlConnection(connetionString1);
        //    try
        //    {
        //        con1.Open();
        //        DataTable Result = new DataTable();
        //        SqlDataAdapter da = new SqlDataAdapter();

        //        string str = "select  Plant,RequestNumber, CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,QuoteNo,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,Product,Material,MaterialDesc,VendorCode1,VendorName from TQuoteDetails Where vendorcode1='" + Session["mappedVendor"].ToString() + "' and  (ApprovalStatus = 1)  Order by RequestNumber desc";

        //        da = new SqlDataAdapter(str, con1);
        //        Result = new DataTable();
        //        da.Fill(Result);

        //        GridView1.DataSource = Result;
        //        GridView1.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex);
        //    }
        //    finally
        //    {
        //        con1.Dispose();
        //    }
            

        //    #region old
        //    //if (Result.Rows.Count > 0)
        //    //{
        //    //    TableRow Hearderrow = new TableRow();

        //    //    Table1.Rows.Add(Hearderrow);
        //    //    foreach (DataColumn dt in Result.Columns)
        //    //    {
        //    //        TableCell tCell1 = new TableCell();
        //    //        Label lb1 = new Label();
        //    //        tCell1.Controls.Add(lb1);
        //    //        tCell1.Text = dt.ColumnName.ToString();

        //    //        Hearderrow.Cells.Add(tCell1);
        //    //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //    //        Hearderrow.ForeColor = Color.White;

        //    //    }

        //    //    int rowcount = 0;
        //    //    int Samerowscount = 0;

        //    //    //  var query = Result.Rows(x => x)
        //    //    //.Where(g => g.Count() > 1)
        //    //    //.Select(y => y.Key)
        //    //    //.ToList();


        //    //    foreach (DataRow row in Result.Rows)
        //    //    {
        //    //        TableRow tRow = new TableRow();
        //    //        Table1.Rows.Add(tRow);


        //    //        for (int cellCtr = 0; cellCtr <= Result.Rows[0].ItemArray.Length - 1; cellCtr++)
        //    //        {

        //    //            TableCell tCell = new TableCell();

        //    //            Label lb = new Label();
        //    //            tCell.Controls.Add(lb);

        //    //            if (rowcount > 0)
        //    //            {
        //    //                if (cellCtr == 0)
        //    //                {
        //    //                    if (row.ItemArray[1].ToString() == Result.Rows[rowcount - 1].ItemArray[1].ToString())
        //    //                    {
        //    //                        Samerowscount++;
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        Samerowscount = 0;
        //    //                    }
        //    //                }
        //    //            }

        //    //            if (rowcount > 0 && Samerowscount > 0 && (cellCtr == 0 || cellCtr == 1 || cellCtr == 2 || cellCtr == 3 || cellCtr == 4 || cellCtr == 5 || cellCtr == 6))
        //    //            {
        //    //                Table1.Rows[rowcount].Cells[cellCtr].Attributes.Add("rowspan", (Samerowscount + (1)).ToString());
        //    //            }
        //    //            else
        //    //            {
        //    //                if (cellCtr == 3)
        //    //                {
        //    //                    LinkButton lnkbutton = new LinkButton();
        //    //                    lnkbutton.ID = "lnk" + rowcount + cellCtr;
        //    //                    lnkbutton.Text = row.ItemArray[cellCtr].ToString();
        //    //                    lnkbutton.OnClientClick = "ShowLoading();";
        //    //                    lnkbutton.Click += Lnkbutton_Click;
        //    //                    tCell.Controls.Add(lnkbutton);
        //    //                    tRow.Cells.Add(tCell);
        //    //                }

        //    //                else
        //    //                {
        //    //                    tCell.Text = row.ItemArray[cellCtr].ToString();
        //    //                    tRow.Cells.Add(tCell);
        //    //                }
        //    //            }

        //    //        }
        //    //        rowcount++;

        //    //    }

        //    //}

        //    ////if (Result.Rows.Count > 0)
        //    ////{
        //    ////    grdVendrRequest.DataSource = Result;

        //    ////    grdVendrRequest.DataBind();



        //    ////}
        //    #endregion old
        //}

        private void Lnkbutton_Click(object sender, EventArgs e)
        {
            // Response.Redirect("NewReq_changes.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)sender).Text);

            Response.Redirect("VViewRequest.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)sender).Text);

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                Session["VndRejectPgNo"] = (GridView1.PageIndex).ToString();
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
            if (e.CommandName == "LinktoRedirect")
            {
                Response.Redirect("VViewRequest.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
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
                        Label LbQuoteNo = e.Row.FindControl("LbQuoteNo") as Label;
                        string url = "VViewRequest.aspx?Number=" + LbQuoteNo.Text;
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
                string ReqStatus = "All";
                if (Session["VndRejectFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["VndRejectFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["VndRejectFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["VndRejectFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                TxtFrom.Text = "";
                TxtTo.Text = "";
                UpdatePanel1.Update();
                ShowTable();
                CreateSessionFilter();
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
                CreateSessionFilter();
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
                txtFind.Text = "";
                UpdatePanel1.Update();
                ShowTable();
                CreateSessionFilter();
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
                CreateSessionFilter();
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
                CreateSessionFilter();
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
                CreateSessionFilter();
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
                TxtFrom.Text = "";
                TxtTo.Text = "";
                txtFind.Text = "";
                Session["VndRejectFilter"] = null;
                Session["VndRejectPgNo"] = null;
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
            try
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
            catch (ThreadAbortException ex2)
            {
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
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
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
    }
}