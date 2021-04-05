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
    public partial class ReqApproval : System.Web.UI.Page
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
        string menu;
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool IsAth;

        string DbMasterName = "";
        

        protected void isAuthor()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            string sql;
            SqlDataReader reader;
            string FormName = "EMET_ShimanoApproval_req";
            string System = "EMET";
            try
            {
                EmetCon.Open();
                //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
                //sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["EPlant"].ToString() + "'";
                sql = @" select distinct tua.* from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID inner join tgroup tg on tg.GroupID=tua.GroupID where tua.System=@System and tua.UserID=@UserId and tua.FormName=@FormName and tua.DelFlag=0 and tg.Plant='" + Session["EPlant"].ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"].ToString());
                cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@System", System);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    IsAth = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');window.location='Home.aspx';", true);
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
                EmetCon.Dispose();
            }
        }

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
                        string FN = "EMET_ShimanoApproval_req";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {

                            string userId = Session["userID"].ToString();
                            userId1 = userId.ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;

                            menu = Request.QueryString["num"];
                            if (menu == "17")
                            {
                                LbPageType.Text = "Manager Approval Pending";
                            }
                            else
                            {
                                LbPageType.Text = "DIR Approval Pending";
                            }

                            userId1 = Session["userID"].ToString();
                            nameC = sname;

                            lblUser.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            LastFilterCondition();
                            if (Session["ShowEntryReqApproval"] != null)
                            {
                                TxtShowEntry.Text = Session["ShowEntryReqApproval"].ToString();
                            }
                            ShowTable();
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

                if (Session["ReqApprFilter"] != null)
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
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

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                menu = Request.QueryString["num"];

                EMETModule.AutoUpdateRequest();
                EMETModule.autoupdate(Session["EPlant"].ToString());
                EMETModule.autoCloseWhitoutsapCode(Session["EPlant"].ToString());
                EMETModule.CekAndCloseExpiredRequest(Session["EPlant"].ToString());

                GetDbMaster();
                EmetCon.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    if (menu == "17")
                    {
                        sql = @" select distinct Plant,RequestNumber,CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                            (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep',
                            case 
                            when A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null) then 'New' 
                            when A.QuoteNoRef is null and (A.isMassRevision = 1) then 'Mass Revision' 
                            else 'Revision' 
                            end as 'ReqStatus'
                            from TQuoteDetails A where RequestNumber in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 0) 
                            and Plant  = '" + Session["EPlant"].ToString() + "' ";
                    }
                    else
                    {
                        sql = @" select distinct Plant,RequestNumber,
                            CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate,
                            CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                            (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                            CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                            Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            SMNPicDept as 'UseDep',
                            case 
                            when A.QuoteNoRef is null and (A.isMassRevision = 0 or A.isMassRevision is null) then 'New' 
                            when A.QuoteNoRef is null and (A.isMassRevision = 1) then 'Mass Revision' 
                            else 'Revision' 
                            end as 'ReqStatus'
                            from TQuoteDetails A 
                            where RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) 
                            and ManagerApprovalStatus=0) and Plant  = '" + Session["EPlant"].ToString() + "' ";
                    }

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
                        Session["ShowEntryReqApproval"] = ShowEntry.ToString();
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();

                            #region return nested and pagination last view
                            if (Session["ReqApprPgNo"] != null)
                            {
                                int PgNo = Convert.ToInt32(Session["ReqApprPgNo"].ToString());
                                if (GridView1.PageCount >= PgNo)
                                {
                                    GridView1.PageIndex = PgNo;
                                    //GridView1.DataSource = dt;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    Session["ReqApprPgNo"] = null;
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

        protected void ShowTableDet(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {

                    if (menu == "17")
                    {
                        sql = @" select VendorCode1,substring((VendorName),1,12) +'...' as VendorName,QuoteNo,
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            CONVERT(nvarchar,ROUND(convert(float,(case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end/convert(float,ISNULL(FinalQuotePrice,0)))	*100),1)) + '%' as 'NetProfit/Discount',
                            ApprovalStatus, 
                            case 
                            when PICApprovalStatus = 0 then 'M.Pending'
                            when PICApprovalStatus = 1 then 'Rejected'
                            when PICApprovalStatus = 2 then 'Approved'
                            when PICApprovalStatus = 3 then 'Approved / Closed'
                            when ApprovalStatus = 2 and PICApprovalStatus=0 and FinalQuotePrice is null then 'Auto Close-SMN'
                            else 'cannot find status'
                            end as 'MngResponseStatus',
                            PICApprovalStatus, 
                            case 
                            when pirstatus is null or pirstatus = '' then 'Waiting for Request completion from all vendors'
                            else 'Ready for process' 
                            end as 'pirstatus',
                            '" + RowParentGv + @"' as ParentGvRowNo,
                            format(AprRejDateMng,'dd/MM/yyyy') as AprRejDateMng,
                            (select distinct UseNam from " + DbMasterName + @".dbo.Usr where UseID=AprRejByMng) as 'AprRejByMng',ManagerReason,ManagerRemark
                            from TQuoteDetails where (RequestNumber in(select distinct RequestNumber from TQuoteDetails where PICApprovalStatus = 0) )
                            and ( RequestNumber = '" + RequestNumber + "' ) ";
                    }
                    else
                    {
                        sql = @" select distinct VendorCode1,substring((VendorName),1,12) +'...' as VendorName,QuoteNo,
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            CONVERT(nvarchar,ROUND(convert(float,(case when convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) = 0 then null else convert(float,isnull(FinalQuotePrice,0))-CONVERT(float,ISNULL(GrandTotalCost,0)) end/convert(float,ISNULL(FinalQuotePrice,0)))	*100),1)) + '%' as 'NetProfit/Discount',
                            ApprovalStatus, 
                            case 
                            when PICApprovalStatus = 0 then 'M.Pending'
                            when PICApprovalStatus = 1 then 'Rejected'
                            when PICApprovalStatus = 2 then 'Approved'
                            when PICApprovalStatus = 3 then 'Approved / Closed'
                            when ApprovalStatus = 2 and PICApprovalStatus=0 and FinalQuotePrice is null then 'Auto Close-SMN'
                            else 'cannot find status'
                            end as 'MngResponseStatus',
                            PICApprovalStatus, 
                            case 
                            when pirstatus is null or pirstatus = '' then 'Waiting for Request completion from all vendors'
                            else 'Ready for process' 
                            end as 'pirstatus',
                            '" + RowParentGv + @"' as ParentGvRowNo, ManagerApprovalStatus ,format(AprRejDateMng,'dd/MM/yyyy') as AprRejDateMng,
                            (select distinct UseNam from " + DbMasterName + @".dbo.Usr where UseID=AprRejByMng) as 'AprRejByMng',ManagerReason,ManagerRemark
                            from TQuoteDetails where RequestNumber in(select distinct requestnumber from TQuoteDetails where RequestNumber not in(select RequestNumber from TQuoteDetails where PICApprovalStatus =0 ) 
                            and ManagerApprovalStatus=0) and ( RequestNumber = '" + RequestNumber + "' ) ";

                    }

                    if (DdlReqStatus.SelectedValue == "New")
                    {
                        sql += @" and QuoteNoRef is null and (isMassRevision = 0 or isMassRevision is null)";
                    }
                    else if (DdlReqStatus.SelectedValue == "Revision")
                    {
                        sql += @" and QuoteNoRef is not null";
                    }
                    else if (DdlReqStatus.SelectedValue == "MassRevision")
                    {
                        sql += @" and (isMassRevision = 1) ";
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
                            UPPER((case when (select ScreenLayout from " + DbMasterName + @".[dbo].TPROCESGRP_SCREENLAYOUT where ProcessGrp=ProcessGroup) = 'Layout2' then MaterialSAPCode else MaterialDescription end)) as 'MaterialDescription',
                            ProcessGroup,CAST(ROUND([RawMaterialCost/kg],7) AS DECIMAL(12,2)) as 'RawMaterialCost/kg',
                            CAST(ROUND([MaterialCost/pcs],7) AS DECIMAL(12,6)) as 'MaterialCost/pcs'  , RowId
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
                    sql = @" select distinct QuoteNo,SubProcess,CAST(ROUND([ProcessCost/pc],7) AS DECIMAL(12,6)) as 'ProcessCost/pc',RowId
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
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNo,UPPER([Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',CAST(ROUND([Sub-Mat/T&JCost/pcs],7) AS DECIMAL(12,6)) as 'Sub-Mat/T&JCost/pcs', RowId
                            from TSMCCostDetails  
                            where QuoteNo=@QuoteNo order by RowId ASC";
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
                            A.Machine,CAST(ROUND(A.[StandardRate/HR],2) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(A.VendorRate,2) AS DECIMAL(12,2))as 'VendorRate',A.ProcessUOM,A.Baseqty,
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
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select B.VendorCode1,substring((B.VendorName),1,12) +'...' as VendorName,
                            (select distinct TVN.crcy from  " + DbMasterName + @".[dbo].tVendorPOrg TVP  inner join " + DbMasterName + @".dbo.tporgplant pp on (pp.plant = B.PLANT and pp.porg = tvp.porg) inner join " + DbMasterName + @".[dbo].tVendor_New TVN on (TVN.Vendor = TVP.Vendor and TVN.porg = tvp.porg ) where  TVN.Vendor = B.VENDORCODE1) as 'crcy'
                            ,UPPER(A.[Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',A.[Sub-Mat/T&JCost],
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

        protected bool CheckStatusallVendor(string QuoteNo)
        {
            bool Result = false;
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
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
            MDMCon.Open();
            try
            {
                getcreateusermail1(ReqNum, Vendor);

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
            finally
            {
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


        public void UpdateGridData(string ReqNum, string Vendor, int Status, string Reason)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            EmetCon.Open();
            if (Status == 1)
            {

                if (Reason.ToString() != "")
                {
                    try
                    {
                        DataTable Result1 = new DataTable();
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        string pic = Session["UserName"].ToString() + " - " + Reason.ToString();
                        //string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "', PICReason = '" + pic + "', ManagerReason = '" + pic + "', DIRReason = '" + pic + "', UpdatedBy='" + Session["userID"].ToString() + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";
                        string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "', PICReason = '" + pic + "', ManagerReason = '" + pic + "', DIRReason = '" + pic + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 ='" + Vendor + "'";

                        da1 = new SqlDataAdapter(str1, EmetCon);
                        Result1 = new DataTable();
                        da1.Fill(Result1);

                        DataTable Result11 = new DataTable();
                        SqlDataAdapter da11 = new SqlDataAdapter();
                        string pic1 = Session["UserName"].ToString() + " - " + Reason.ToString();
                        string str11 = "Update TQuoteDetails SET  ManagerApprovalStatus= '" + 1 + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                        da11 = new SqlDataAdapter(str11, EmetCon);
                        Result11 = new DataTable();
                        da11.Fill(Result11);

                        getcreateusermail(ReqNum, Vendor);
                    }
                    catch (Exception ex)
                    {
                        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                        EMETModule.SendExcepToDB(ex);
                    }
                    

                    #region sending email
                    bool sendingemail = false;
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
                            dr.Close();
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
                            dr.Close();
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
                            dr.Close();
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
                            qdr.Close();
                            Qcnn.Dispose();
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
                            dr.Close();
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
                                    dr_.Close();
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
                                    string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + Session["quoteno"].ToString() + " - Shimano Approval Rejection By : " + Session["UserName"].ToString() + " - Plant : " + Session["EPlant"].ToString();
                                    //string footer = "Please Login SHIMANO e-MET system for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                    //string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been submitted to Shimano PIC by " + nameC + " <br /> <br /> The details are<br /><br /> " + body1;
                                    string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Rejected By Shimano.<br /><br />" + Session["body1"].ToString();
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
                            dr_cus.Close();
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Please fill the Reason for Reject');", true);
                }
            }


            if (Status == 2)
            {
                try
                {

                    DataTable Result = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();

                    string str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',PICApprovalStatus = '" + Status + "', ManagerApprovalStatus = '" + Status + "', DIRApprovalStatus = '" + Status + "', PICReason = '" + Reason + "', ManagerReason = '" + Reason + "', DIRReason = '" + Reason + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                    //da = new SqlDataAdapter(str, con1);
                    //Result = new DataTable();
                    //da.Fill(Result);

                    //string reason1 = "Auto Rejected By PIC";

                    //str = "Update TQuoteDetails SET PICApprovalStatus = '" + 1 + "',ManagerApprovalStatus = '" + 0 + "', PICReason = '" + reason1 + "', UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'  and VendorCode1 not in('" + Vendor + "') and (PICApprovalStatus = '" + 0 + "' or PICApprovalStatus is null)";

                    //da = new SqlDataAdapter(str, con1);
                    //Result = new DataTable();
                    //da.Fill(Result);
                    string pic = Session["UserName"].ToString() + " - " + Reason.ToString();
                    str = "Update TQuoteDetails SET ApprovalStatus='" + 3 + "',PICApprovalStatus = '" + Status + "',  ManagerApprovalStatus = '" + Status + "', DIRApprovalStatus = '" + Status + "', PICReason = '" + pic + "', ManagerReason = '" + pic + "', DIRReason = '" + pic + "' where RequestNumber = '" + ReqNum + "' and VendorCode1 ='" + Vendor + "'";

                    da = new SqlDataAdapter(str, EmetCon);
                    Result = new DataTable();
                    da.Fill(Result);

                    DataTable Result11 = new DataTable();
                    SqlDataAdapter da11 = new SqlDataAdapter();
                    string pic1 = Session["UserName"].ToString() + " - " + Reason.ToString();
                    string str11 = "Update TQuoteDetails SET  ManagerApprovalStatus= '" + Status + "',  UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "' and ManagerApprovalStatus is null";

                    da11 = new SqlDataAdapter(str11, EmetCon);
                    Result11 = new DataTable();
                    da11.Fill(Result11);
                }
                catch (Exception ex)
                {
                    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                    EMETModule.SendExcepToDB(ex);
                }

                #region sending email
                bool sendingemail = false;
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
                        dr.Close();
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
                        dr.Close();
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
                        dr.Close();
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
                        qdr.Close();
                        Qcnn.Dispose();
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
                        dr.Close();
                        cnn.Dispose();
                    }

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
                                dr_.Close();
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
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that Quotation has been Approved By Shimano.<br /><br />" + Session["body1"].ToString();
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
                        dr_cus.Close();
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
            
            EmetCon.Dispose();
        }

        protected void LinkButton_Click(Object sender, CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                Response.Redirect("QuoteCostPlan.aspx?Number=" + e.CommandArgument.ToString());
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
                    int count = 0;
                    bool isAllVendorSubmit = false;
                    for (int d = 0; d < DtDetReqNo.Rows.Count; d++)
                    {
                        string GrandTotalCost = DtDetReqNo.Rows[d]["GrandTotalCost"].ToString();
                        string FinalQuotePrice = DtDetReqNo.Rows[d]["FinalQuotePrice"].ToString();
                        if (GrandTotalCost.Trim() != "" && FinalQuotePrice.Trim() != "")
                        {
                            count++;
                        }
                    }

                    #region TableDetailQuoteCondition
                    if (DtDetReqNo.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtDetReqNo.Rows.Count; i++)
                        {
                            if (DtDetReqNo.Rows[i]["PIRStatus"].ToString() == "Waiting for Request completion from all vendors")
                            {
                                isAllVendorSubmit = false;
                                GvDet.Rows[i].Cells[4].Text = "";
                                GvDet.Rows[i].Cells[5].Text = "";
                                GvDet.Rows[i].Cells[6].Text = "";
                                GvDet.Rows[i].Cells[7].Text = "";
                                GvDet.Rows[i].Cells[8].Text = "";
                                GvDet.Rows[i].Cells[9].Text = "";
                                GvDet.Rows[i].Cells[10].Text = "";
                            }
                            else
                            {
                                isAllVendorSubmit = true;
                            }

                            //if (IsTeamShimano(DtDetReqNo.Rows[i]["VendorCode1"].ToString()) == true)
                            //{
                            //    //GvDet.HeaderRow.Cells[10].Visible = false;
                            //    //GvDet.Rows[i].Cells[10].Visible = false;
                            //    GvDet.Rows[i].Cells[10].Text = "";
                            //}
                            //else
                            //{
                            //    //GvDet.HeaderRow.Cells[10].Visible = true;
                            //    //GvDet.Rows[i].Cells[10].Visible = true;
                            //    if (DtDetReqNo.Rows[i]["NetProfit/Discount"].ToString() == "")
                            //    {
                            //        GvDet.Rows[i].Cells[10].Text = "0.0 %";
                            //    }

                            //    if (DtDetReqNo.Rows[i]["PIRStatus"].ToString() == "Waiting for Request completion from all vendors")
                            //    {
                            //        GvDet.Rows[i].Cells[10].Text = "";
                            //    }
                            //}
                        }
                    }
                    #endregion

                    if (isAllVendorSubmit == true)
                    {
                        BtnCompare.Enabled = true;
                        if (GvDet.Rows.Count <= 1)
                        {

                            BtnCompare.Enabled = false;
                        }
                        else if (count <= 1)
                        {
                            BtnCompare.Enabled = false;
                        }
                    }
                    else
                    {
                        BtnCompare.Enabled = false;
                    }
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
                Session["ReqApprPgNo"] = (GridView1.PageIndex).ToString();
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
                    GetMainDataComp(ReqNumber);
                    GetDataSumarizeCost(ReqNumber);
                    GetDataMatCost(ReqNumber);
                    GetDataProcCost(ReqNumber);
                    AddRowSpanToGridViewProcCost();
                    GetDataSubMatCost(ReqNumber);
                    AddRowSpanToGridViewSubMatCost();
                    GetDataOthCost(ReqNumber);
                    AddRowSpanToGridViewOthCost();
                    UpdatePanel2.Update();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "OpenModalCompare();freezeheader();Tabs();Layout7Condition();", true);
                    if (Session["ApprNst"] != null)
                    {
                        string RowVsStatus = Session["ApprNst"].ToString();
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
                    }
                }
                else if (e.CommandName == "TrgNestedExpand")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Ex";
                    Session["ReqApprNst"] = RowVsStatus;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                }
                else if (e.CommandName == "TrgNestedColapse")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string RowVsStatus = rowIndex.ToString() + "-" + "Colp";
                    Session["ReqApprNst"] = RowVsStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqApprFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
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
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                string ReqStatus = "All";
                if (Session["ReqApprFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
                }
                else
                {
                    string[] ArrFilter = Session["ReqApprFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ReqApprFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween + "!" + ReqStatus;
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
                TxtFrom.Text = "";
                TxtTo.Text = "";
                txtFind.Text = "";
                Session["ReqApprPgNo"] = null;
                Session["ReqApprNst"] = null;
                Session["ReqApprFilter"] = null;
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
            try {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[15].Visible = false;
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

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "Overall status";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderCell.ColumnSpan = 5;
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

        protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

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
                    }
                    //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                }
                else
                {
                    #region reference gridview row
                    string[] CmdArg = (e.CommandArgument).ToString().Split('|');
                    int TotRecord = Convert.ToInt32(LbTtlRecords.Text.Replace("Total Record :", ""));

                    int rowIndexMain = Convert.ToInt32(CmdArg[0]) - 1;
                    int rowIndexDet = Convert.ToInt32(CmdArg[1]);

                    //4: reference from = total data can be show on 1 page -1 (because data index for row start from 0)
                    if (rowIndexMain > 4)
                    {
                        //5: reference from pagination has been set from client side / total data can be show on 1 page
                        while (rowIndexMain > 4)
                        {
                            rowIndexMain = rowIndexMain - 5;
                        }
                    }

                    GridViewRow rowGvMain = GridView1.Rows[rowIndexMain];
                    GridView GvDet = rowGvMain.FindControl("GvDet") as GridView;

                    GridViewRow rowGvDet = GvDet.Rows[rowIndexDet];
                    LinkButton Lb = rowGvDet.FindControl("LbQuoteNo") as LinkButton;

                    //string Reason = (rowGvDet.FindControl("txtReason") as TextBox).Text;
                    string ReqNumber = rowGvMain.Cells[3].Text;
                    string Vendor = rowGvDet.Cells[1].Text;
                    string Quote = Lb.Text;
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

        protected void GvDetMatCost_RowDataBound(object sender, GridViewRowEventArgs e)
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
                string ReqStatus = "All";
                if (Session["ApprovalFilter"] == null)
                {
                    Session["ApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrFilter = Session["ApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }

                if (Session["ApprNst"] != null)
                {
                    string RowVsStatus = Session["ApprNst"].ToString();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');", true);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseModalCompare();DatePitcker();", true);
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
                string ReqStatus = "All";
                if (Session["ApprovalFilter"] == null)
                {
                    Session["ApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrFilter = Session["ApprovalFilter"].ToString().Split('!');
                    column = ArrFilter[0].ToString();
                    sortDirection = ArrFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    ReqStatus = DdlReqStatus.SelectedValue;
                    Session["ApprovalFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
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
            if (Session["ReqApprNst"] != null)
            {
                string RowVsStatus = Session["ReqApprNst"].ToString();
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
            catch (ThreadAbortException ex)
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