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
    public partial class EWhitoutSAPCode : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        string DbMasterName = "";
        bool IsAth;

        int TotDetailRecord = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EWhitoutSAPCode";
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
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            // Session["UserName"] = userId;
                            //      string strprod = txtplant.Text;

                            //GetGridData();
                            if (!string.IsNullOrEmpty(Request.QueryString["Num"]))
                            {
                                if (Request.QueryString["Num"] == "2")
                                {
                                    DdlStatus.SelectedValue = "submit";
                                }
                                else if (Request.QueryString["Num"] == "1")
                                {
                                    DdlStatus.SelectedValue = "wait";
                                }
                            }

                            LastFilterCondition();
                            if (Session["ShowEntryEWhitoutSAPCode"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryEWhitoutSAPCode"].ToString();
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

        protected void isAuthor()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            
            string sql;
            SqlDataReader reader;
            string FormName = "EWhitoutSAPCode";
            string System = "EMET";
            try
            {
                MDMCon.Open();
                sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
                string a = Session["userID_"].ToString();
                SqlCommand cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"].ToString());
                cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@System", System);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    IsAth = false;
                }
                else
                {
                    IsAth = true;
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

        protected void LastFilterCondition()
        {
            try
            {

                if (Session["WthSAPFilter"] != null)
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
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

                    if (ArrFilter[6].ToString() != "")
                    {
                        DdlStatus.SelectedIndex = Convert.ToInt32(ArrFilter[6]);
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
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }

        bool IsTeamShimano(string VendorCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            
            bool IsTeamShimano = false;
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

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                TotDetailRecord = 0;
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct Plant,RequestNumber,CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                    //         Product,Material,MaterialDesc ,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy'
                    //        from TQuoteDetails A where ((ApprovalStatus = 3) or (ApprovalStatus = 1))
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (PICApprovalStatus = 2) or (PICApprovalStatus = 0) or (PICApprovalStatus is null) ))
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (ManagerApprovalStatus = 2) or (ManagerApprovalStatus = 0) ) )
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (DIRApprovalStatus = 2) or (DIRApprovalStatus = 0) ) ) ";

                    sql = @" select distinct Plant,RequestNumber,(select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                             Product,Material,MaterialDesc ,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep'
                            from TQuoteDetails A  
                            where  isnull(A.CreateStatus,'') <> '' and ";

                    if (DdlStatus.SelectedIndex == 1)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='5' and Plant  = '" + Session["EPlant"].ToString() + @"' ) ";
                    }
                    else if (DdlStatus.SelectedIndex == 2)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4' and Plant  = '" + Session["EPlant"].ToString() + @"' ) ";
                    }
                    else if (DdlStatus.SelectedIndex == 3)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='6' and Plant  = '" + Session["EPlant"].ToString() + @"' ) ";
                    }
                    else
                    {
                        sql += @"( ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4' and Plant  = '" + Session["EPlant"].ToString() + @"' ) or
                                   ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='5' and Plant  = '" + Session["EPlant"].ToString() + @"' ) or
                                   ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='6' and Plant  = '" + Session["EPlant"].ToString() + @"' ) ) ";
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
                        Session["ShowEntryEWhitoutSAPCode"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Request : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["WthSAPCodePgNo"] != null)
                            {
                                int ClStPgNo = Convert.ToInt32(Session["WthSAPCodePgNo"].ToString());
                                if (GridView1.PageCount >= ClStPgNo)
                                {
                                    GridView1.PageIndex = ClStPgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["WthSAPCodePgNo"] = null;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            LbTtlRecords.Text = "Total Request : 0";
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

        protected void ShowTableDet(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select '" + RowParentGv + @"' as ParentGvRowNo,VendorCode1,VendorName,QuoteNo,TotalMaterialCost,TotalProcessCost,TotalSubMaterialCost,TotalOtheritemsCost,
                    //        GrandTotalCost,Profit,Discount,FinalQuotePrice,ApprovalStatus,PICApprovalStatus,PICReason,
                    //        ManagerApprovalStatus,ManagerReason,'' as Reason,DIRApprovalStatus,DIRReason
                    //        from TQuoteDetails where ((ApprovalStatus = 3) or (ApprovalStatus = 1))
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (PICApprovalStatus = 2) or (PICApprovalStatus = 0) or (PICApprovalStatus is null) ))
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (ManagerApprovalStatus = 2) or (ManagerApprovalStatus = 0) ) )
                    //        and (RequestNumber not in(select distinct RequestNumber from TQuoteDetails where (DIRApprovalStatus = 2) or (DIRApprovalStatus = 0) ) )
                    //        and RequestNumber = '" + RequestNumber + "'  ";

                    sql = @" select '" + RowParentGv + @"' as ParentGvRowNo,VendorCode1,substring((VendorName),1,12) +'...' as VendorName,QuoteNo,
                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) 
                            end as TotalMaterialCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5))
                            end as TotalProcessCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5))
                            end as TotalSubMaterialCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5))
                            end  as TotalOtheritemsCost,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5))
                            end as GrandTotalCost,


                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) 
                            end  as FinalQuotePrice,

                            case 
                            when pirstatus is null or pirstatus = '' then NULL
                            else CONVERT(nvarchar,
                            ROUND(
                            convert(float,
                            (
                            case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null
                            else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end
                            /convert(float,ISNULL(FinalQuotePrice,0))
                            )
                            *100)
                            ,1)
                            ) + '%'
                            end  as 'NetProfit/Discount',
                            ApprovalStatus,PICApprovalStatus,PICReason,
                            ManagerApprovalStatus,ManagerReason,'' as Reason,DIRApprovalStatus,DIRReason,format(UpdatedOn,'dd/MM/yyyy') as ApprovalDate,
                            case 
                                 when TQ.UpdatedBy = 'EMET' then 'Auto Completed By Shimano'
                                 when ApprovalStatus = '4' then 'Waiting For Submission' 
	                             when ApprovalStatus = '5' then 'Submitted'
                                 when ApprovalStatus = '6' then 'Auto Completed By Shimano' 
	                             end 
                            as 'Status'
                            from TQuoteDetails TQ where isnull(TQ.CreateStatus,'') <> '' and ";

                    if (DdlStatus.SelectedIndex == 1)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='5' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) ";
                    }
                    else if (DdlStatus.SelectedIndex == 2)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) ";
                    }
                    else if (DdlStatus.SelectedIndex == 3)
                    {
                        sql += @" ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='6' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) ";
                    }
                    else
                    {
                        sql += @" ( ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='4' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) or
                                    ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='5' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) or
                                    ( (SELECT RIGHT(QuoteNo, 1)) = 'D' and ApprovalStatus='6' and Plant  = '" + Session["EPlant"].ToString() + @"' and RequestNumber = '" + RequestNumber + @"' ) ) ";
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

        private void LnkApp_Click(object sender, EventArgs e)
        {

        }
        
        public void UpdateGridData(string ReqNum, string Vendor, int Status, string Reason)
        {
            string userID = (string)HttpContext.Current.Session["UserName"].ToString();

            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();

                if (Status == 1)
                {

                    DataTable Result1 = new DataTable();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    string str1 = "Update TQuoteDetails SET DIRApprovalStatus = '" + 1 + "', PICApprovalStatus = '" + 2 + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                    da1 = new SqlDataAdapter(str1, EmetCon);
                    Result1 = new DataTable();
                    da1.Fill(Result1);
                }

                if (Status == 2)
                {
                    DataTable Result = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();

                    string str = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', DIRApprovalStatus = '" + 1 + "',PICApprovalStatus = '" + 2 + "', DIRReason = '" + Reason + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 not in('" + Vendor + "')";

                    da = new SqlDataAdapter(str, EmetCon);
                    Result = new DataTable();
                    da.Fill(Result);

                    str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',DIRApprovalStatus = '" + Status + "',PICApprovalStatus = '" + 2 + "', DIRReason = '" + Reason + "', UpdatedBy='" + userID + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                    da = new SqlDataAdapter(str, EmetCon);
                    Result = new DataTable();
                    da.Fill(Result);
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

        protected void LinkButton_Click(Object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                Response.Redirect("NewReq_changes.aspx?Number=" + e.CommandArgument.ToString());
            }
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
                    //ReqNo = iBranc;
                    ShowTableDet(iBranc, (RowParentGv + 1));
                    DataTable DtDetReqNo = new DataTable();
                    DtDetReqNo = (DataTable)Session["TableDet"];
                    GvDet.DataSource = DtDetReqNo;
                    GvDet.DataBind();

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

                            //    if (DtDetReqNo.Rows[i]["Status"].ToString() == "Waiting For Submission")
                            //    {
                            //        GvDet.Rows[i].Cells[10].Text = "";
                            //    }
                            //}
                        }
                        TotDetailRecord = TotDetailRecord + DtDetReqNo.Rows.Count;
                    }
                    LbTtlRecordsDet.Text = "Total Quote Details : " + TotDetailRecord;
                }

                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                {
                    #region span column
                    //int RowSpan = 2;
                    //for (int i = GridView1.Rows.Count - 2; i >= 0; i--)
                    //{
                    //    GridViewRow currRow = GridView1.Rows[i];
                    //    GridViewRow prevRow = GridView1.Rows[i + 1];
                    //    if (currRow.Cells[1].Text == prevRow.Cells[1].Text)
                    //    {
                    //        currRow.Cells[1].RowSpan = RowSpan;
                    //        prevRow.Cells[1].Visible = false;

                    //        currRow.Cells[0].RowSpan = RowSpan;
                    //        prevRow.Cells[0].Visible = false;

                    //        currRow.Cells[2].RowSpan = RowSpan;
                    //        prevRow.Cells[2].Visible = false;

                    //        currRow.Cells[3].RowSpan = RowSpan;
                    //        prevRow.Cells[3].Visible = false;

                    //        currRow.Cells[4].RowSpan = RowSpan;
                    //        prevRow.Cells[4].Visible = false;

                    //        currRow.Cells[5].RowSpan = RowSpan;
                    //        prevRow.Cells[5].Visible = false;

                    //        currRow.Cells[6].RowSpan = RowSpan;
                    //        prevRow.Cells[6].Visible = false;

                    //        RowSpan += 1;
                    //    }
                    //    else
                    //    {
                    //        currRow.Cells[0].RowSpan = 1;
                    //        RowSpan = 2;
                    //    }
                    //}
                    #endregion span column
                
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
                Session["WthSAPCodePgNo"] = (GridView1.PageIndex).ToString();
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

                if (e.CommandName == "TrgNestedExpand")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Ex";
                    Session["WthSAPCodeNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                }
                else if (e.CommandName == "TrgNestedColapse")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Colp";
                    Session["WthSAPCodeNst"] = RowVsStatus;
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
                string Status = "";
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["WthSAPFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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
                string Status = "";
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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
                UpdatePanel1.Update();
                txtFind.Text = "";
                ShowTable();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "";
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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
                ShowTable();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "";
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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
                string Status = DdlStatus.SelectedIndex.ToString();
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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
                string Status = DdlStatus.SelectedIndex.ToString();
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
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

                ShowTable();
                txtFind.Focus();
                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string Status = "";
                if (Session["WthSAPFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                else
                {
                    string[] ArrFilter = Session["WthSAPFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Status = DdlStatus.SelectedIndex.ToString();
                    Session["WthSAPFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + Status;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);

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
                Session["WthSAPCodePgNo"] = null;
                Session["WthSAPCodeNst"] = null;
                Session["WthSAPFilter"] = null;
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
                    e.Row.Cells[10].Visible = false;
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

        protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                string[] CmdArg = (e.CommandArgument).ToString().Split('|');

                if (e.CommandName == "LinktoRedirect")
                {
                    //int RowMainGv = Convert.ToInt32(CmdArg[0]) - 1;
                    //int RowDetGv = Convert.ToInt32(CmdArg[1]);
                    //GridView GvDet = GridView1.Rows[RowMainGv].FindControl("GvDet") as GridView;
                    //String QuoteNo = GvDet.Rows[RowDetGv].Cells[3].Text.ToString();
                    //Response.Redirect("QuoteCostPlan.aspx?Number=" + QuoteNo);
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                    //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                }
                else if (e.CommandName == "Approve")
                {
                    int rowIndex = Convert.ToInt32(CmdArg[0].ToString());
                    GridViewRow row = GridView1.Rows[rowIndex - 1];
                    string ReqNumber = row.Cells[3].Text;
                    Response.Redirect("DateUpdate.aspx?Number=" + ReqNumber.ToString());
                }
                else if (e.CommandName == "Reject")
                {
                    int rowIndex = Convert.ToInt32(CmdArg[0].ToString());
                    GridViewRow row = GridView1.Rows[rowIndex - 1];
                    string Reason = "Quotation response due date Expired";
                    string ReqNumber = row.Cells[3].Text;
                    string Vendor = "";
                    string Quote = "";
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        if (i == (Convert.ToInt32(CmdArg[0]) - 1))
                        {
                            GridView GvDet = GridView1.Rows[i].FindControl("GvDet") as GridView;
                            for (int a = 0; a < GvDet.Rows.Count; a++)
                            {
                                if (a == (Convert.ToInt32(CmdArg[1])))
                                {
                                    Vendor = GvDet.Rows[a].Cells[1].Text;
                                    Quote = GvDet.Rows[a].Cells[3].Text;
                                    break;
                                }
                            }
                            break;
                        }
                    }
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
                    HeaderCell.Text = "Status";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.RowSpan = 2;
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

        protected void BtnCekLatestNested_Click(object sender, EventArgs e)
        {
            IsFirstLoad.Text = "2";
            if (Session["WthSAPCodeNst"] != null)
            {
                string RowVsStatus = Session["WthSAPCodeNst"].ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
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