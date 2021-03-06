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
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Material_Evaluation
{
    public partial class RealTimeVendInvSMN : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;
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
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                        string FN = "EMET_RealTimeVendInvSMN";
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
                            // Session["UserName"] = userId;
                            //      string strprod = txtplant.Text;

                            //GetGridData();
                            if (Session["ReqWaitFilter"] != null)
                            {
                                string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
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
                            }
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            //if (Session["ShowEntryReqWaitt"] != null)
                            //{
                            //    TxtShowEntry.Text = Session["ShowEntryReqWaitt"].ToString();
                            //}
                            //ShowModalLatestUpdated();
                            ShowMainTable();
                            getVendorProgress();
                            //ShowTable();

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

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "sortingShowLoading();DatePitcker();GenerateSearchByColumn();GenerateNewMainTable();GenerateTbData();CloseLoading();", true);
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

                if (Session["AllReqFilter"] != null)
                {
                    string[] ArrFilter = Session["AllReqFilter"].ToString().Split('!');
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

        protected void ShowModalLatestUpdated()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"SELECT DISTINCT A.Plant, A.VendorCode, V.Description, A.CustomerNo, Z.Customerdescription, CASE WHEN ISNULL(MAX(A.UpdatedDate), '') = '' THEN FORMAT (MAX(A.CreatedDate), 'dd-MM-yyyy ') ELSE FORMAT (MAX(A.UpdatedDate), 'dd-MM-yyyy ') END AS 'LatestUpdated'
                          FROM TRealTimeVendorInventory A
                          LEFT JOIN " + DbMasterName + @".dbo.tVendorPOrg O ON A.VendorCode = O.Vendor and A.Plant = O.Plant
                          LEFT JOIN " + DbMasterName + @".dbo.tVendor_New V ON A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
					      LEFT JOIN " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z ON A.CustomerNo = Z.Customer AND A.SAPCode = Z.Material
                          WHERE A.Plant = @Plant
                          GROUP BY A.Plant, A.VendorCode, V.Description, A.CustomerNo, Z.Customerdescription";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GDVLatestUpdate.DataSource = dt;
                        GDVLatestUpdate.DataBind();
                        UpdatePanel2.Update();
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

        protected void getVendorProgress()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"DECLARE @TotalVendor int = 0, @TotalCustomer int = 0, @Total int = 0, @TotalSubmitted int = 0, @now datetime = GETDATE()
                            DECLARE @date datetime = CONVERT(date, @now);
                            DECLARE @start datetime = DATEADD(d, 1 - DATEPART(w, @date), @date),
                                    @end   datetime = DATEADD(d, 7 - DATEPART(w, @date), @date);

                            SET @TotalVendor = (SELECT COUNT (DISTINCT VendorCode) AS TotalVendor FROM TRealTimeVendorInventory
                            WHERE Plant = @Plant AND ISNULL(VendorCode, '') <> '')

                            SET @TotalCustomer = (SELECT COUNT (DISTINCT CustomerNo) AS TotalCustomer FROM TRealTimeVendorInventory
                            WHERE Plant = @Plant AND ISNULL(CustomerNo, '') <> '')

                            SET @Total = @TotalVendor + @TotalCustomer;

                            SET @TotalSubmitted = (SELECT Count(*) FROM (SELECT DISTINCT VendorCode, CustomerNo FROM TRealTimeVendorInventory
                            WHERE (CreatedDate >= FORMAT(@start, 'yyyy-MM-dd') AND CreatedDate <= FORMAT(@end, 'yyyy-MM-dd')) AND Plant = @Plant) Table1)


                            SELECT @Total AS TotalData, @TotalSubmitted AS TotalSubmitted, ROUND(@TotalSubmitted * 100 / @Total, 1) AS PercentageSubmission";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        lblTotalVendor.Text = "Total No. of Vendors for Inventory = " + dt.Rows[0]["TotalData"];
                        lblSubmitted.Text = "Vendor Submitted = " + dt.Rows[0]["TotalSubmitted"];
                        lblPercentageSubmission.Text = "% Submission = " + dt.Rows[0]["PercentageSubmission"] +"%";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ShowMainTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"
                            SET DATEFIRST 1; 
                            Declare @sql nvarchar(max), @cols varchar(max)
                            Declare @CurrentWeekNumber int = (select Datepart(week, GETDATE()))

                            set @cols = '['+ CAST((@CurrentWeekNumber-3) AS nvarchar(MAX)) +'],'+'['+ CAST((@CurrentWeekNumber-2) AS nvarchar(MAX)) +'],'++'['+ CAST((@CurrentWeekNumber-1) AS nvarchar(MAX)) +'],'+'['+ CAST((@CurrentWeekNumber) AS nvarchar(MAX)) +']';

                            set @sql = N'
                                select DISTINCT pvt.Plant, pvt.VendorCode, substring((V.Description),1,12) +'' ...'' as VendorDesc, pvt.CustomerNo, substring((Z.Customerdescription),1,12) +'' ...'' as CustomerDesc, ' + @cols + ', '+CAST(@CurrentWeekNumber as nvarchar(max))+' as CurrentWeekNumber from
                                (
                                    SELECT DISTINCT A.Plant, A.VendorCode, A.CustomerNo,Datepart(week, A.CreatedDate) AS ''UploadOnWeek'',A.CreatedDate as ''CreatedDate''
                            FROM TRealTimeVendorInventory A
                            left join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            left join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                            left join " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z on A.CustomerNo = Z.Customer AND A.SAPCode = Z.Material
                            WHERE A.Plant = '+@Plant+'
                            GROUP BY A.Plant, A.VendorCode, A.CustomerNo, A.CreatedDate
                                ) src
                                pivot
                                (
                                    max(createddate) for UploadOnWeek in (' + @cols + ')
                                ) pvt

                            left join " + DbMasterName + @".dbo.tVendorPOrg O on pvt.VendorCode = O.Vendor and pvt.Plant = O.Plant
                            left join " + DbMasterName + @".dbo.tVendor_New V on pvt.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                            left join " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z on pvt.CustomerNo = Z.Customer
                            '

                            exec sp_executesql @sql ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        var convertdata = dt.AsEnumerable().Select(row => new
                        {
                            Plant = row["Plant"],
                            VendorCode = row["VendorCode"],
                            VendorDesc = row["VendorDesc"],
                            CustomerNo = row["CustomerNo"],
                            CustomerDesc = row["CustomerDesc"],
                            LastWeek_3 = row[5],
                            LastWeek_2 = row[6],
                            LastWeek_1 = row[7],
                            CurrentWeek = row[8],
                            CurrentWeekNumber = row["CurrentWeekNumber"],
                            
                        });

                        string JsonResult = JsonConvert.SerializeObject(convertdata);
                        TxtDataMainJson.Text = JsonResult;
                    }
                }
                //UpdatePanel1.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true); 
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

        private class ListVendororCustomer
        {
            public string VendorCode { get; set; }
            public string CustomerNo { get; set; }
        }

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string StrDataListVndAmor = txtSelectedVendorAndCustomer.Text;
                List<ListVendororCustomer> MyListVndAmor = new List<ListVendororCustomer>();
                if (StrDataListVndAmor.Trim() != "")
                {
                    if (StrDataListVndAmor.Trim() != "" || StrDataListVndAmor.Trim() != "[]")
                    {
                        MyListVndAmor = (List<ListVendororCustomer>)JsonConvert.DeserializeObject((StrDataListVndAmor), typeof(List<ListVendororCustomer>));
                    }
                }

                string SelectedVendor = "";
                string SelectedCustomerNo = "";

                if (MyListVndAmor.Count() > 0)
                {
                    for (int i = 0; i < MyListVndAmor.Count(); i++)
                    {
                        if (MyListVndAmor[i].VendorCode != null && MyListVndAmor[i].VendorCode != "" && MyListVndAmor[i].VendorCode != "null")
                        {
                            SelectedVendor += "'" + MyListVndAmor[i].VendorCode + "',";
                        }

                        if (MyListVndAmor[i].CustomerNo != null && MyListVndAmor[i].CustomerNo != "" && MyListVndAmor[i].CustomerNo != "null")
                        {
                            SelectedCustomerNo += "'" + MyListVndAmor[i].CustomerNo + "',";
                        }
                    }
                }
                
                if (SelectedVendor != "")
                {
                    SelectedVendor = SelectedVendor.Remove(SelectedVendor.Length - 1);
                }
                if (SelectedCustomerNo != "")
                {
                    SelectedCustomerNo = SelectedCustomerNo.Remove(SelectedCustomerNo.Length - 1);
                }

                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" 
                            SET DATEFIRST 1;
                            Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date, @beforeSchedule date, @ScheduleDay NVARCHAR(10), @daystoadd int, @SchedulePerMonth date

                            -- strip the time part from your date
                            DECLARE @date datetime = CONVERT(date, @now);

                            -- do the day of week math
                            DECLARE @start datetime = DATEADD(d, 1 - DATEPART(w, @date), @date),
                                    @end   datetime = DATEADD(d, 7 - DATEPART(w, @date), @date);

                            SET @ScheduleDay = (SELECT IDValue FROM " + DbMasterName + @".dbo.tGlobal WHERE id = 'Realtime_Inv_Mail_Remider_Day')
                            SET @SchedulePerMonth = CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)

                            IF @ScheduleDay = 'MON'
                                BEGIN
		                            SET @daystoadd = 0
	                            END
                            ELSE IF @ScheduleDay = 'TUE'
                                BEGIN
                                    SET @daystoadd = 1
	                            END
                            ELSE IF @ScheduleDay = 'WED'
                                BEGIN
                                    SET @daystoadd = 2
	                            END
                            ELSE IF @ScheduleDay = 'THU'
                                BEGIN
                                    SET @daystoadd = 3
	                            END
                            ELSE IF @ScheduleDay = 'FRI'
                                BEGIN
                                    SET @daystoadd = 4
	                            END
                            ELSE IF @ScheduleDay = 'SAT'
                                BEGIN
                                    SET @daystoadd = 5
	                            END
                            ELSE IF @ScheduleDay = 'SUN'
                                BEGIN
		                            SET @daystoadd = 6
	                            END
                            set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false')
                            set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                            set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                            While @counter <= @max
                            Begin

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @SchedulePerMonth)
                            BEGIN
	                            SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                            END

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                            BEGIN
	                            SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                            END

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
                            BEGIN
	                            SET @nextSchedule = (DATEADD(day, -1, @nextSchedule))
	                            SET @totalHoliday = @totalHoliday + 1
	                            SET @isHoliday = 1
                            END
                            ELSE
                            BEGIN
	                            SET @isHoliday = 0
	                            --BREAK;
                            END

                            SET @counter = @counter + 1

                            End

                           Select distinct ROW_NUMBER() OVER (
                           PARTITION BY A.CreatedDate, A.VendorCode
	                        ORDER BY A.CreatedDate desc
                           ) row_num, A.Plant, T.Description as PlantName, A.VendorCode, substring(( v.Description),1,12) +' ...' as VndName, A.SAPCode, M.MaterialDesc,M.MaterialType,A.UOM, A.PirNo, 
                            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, A.CustomerNo, Z.Customerdescription,
                            CASE WHEN A.CreatedBy is null THEN null ELSE CONCAT(A.CreatedBy,' - ', U.UseNam) END as CreatedBy,
                            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                            A.CreatedDate as CreatedDateOri,
                            CASE WHEN A.UpdatedBy is null THEN null ELSE CONCAT(A.UpdatedBy,' - ', C.UseNam) END as UpdatedBy,
                            --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate', 
                            A.UpdatedDate,case when exists (SELECT * FROM TRealTimeVendorInventory WHERE CreatedDate < CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END
                          --AND CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) 
                            AND Plant = A.Plant AND VendorCode = A.VendorCode AND SAPCode = A.SAPCode AND CreatedDate = A.CreatedDate) then 'true' else 'false' end as IsOld
                            From TRealTimeVendorInventory A
                            left join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            left join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
							left join " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z on A.CustomerNo = Z.Customer AND A.SAPCode = Z.Material
                            join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                            join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                            left join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID
                            left join " + DbMasterName + @".dbo.Usr C on A.UpdatedBy = C.UseID 
                            WHERE A.Plant = @Plant ";

                    //sql = @" select distinct Plant,RequestNumber,
                    //        (select count(*) from TQuoteDetails B where B.RequestNumber = A.RequestNumber) as 'NoQuote',
                    //        CONVERT(VARCHAR(10), RequestDate, 103) as RequestDate, 
                    //        CONVERT(VARCHAR(10), QuoteResponseDueDate, 103) as QuoteResponseDueDate,
                    //        CONVERT(DateTime, RequestDate,101)as RqDate,CONVERT(DateTime, QuoteResponseDueDate,101)as QuoteResDueDate,
                    //         Product,Material,MaterialDesc,(select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                    //        SMNPicDept as 'UseDep' from TQuoteDetails A
                    //        where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null)
                    //        and isUseSAPCode = 1 and (isMassRevision=0 or isMassRevision is null)
                    //        and Plant  = '" + Session["EPlant"].ToString() + "' and A.QuoteNoRef is null ";

                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        if (DdlFltrDate.SelectedValue.ToString() == "CreatedDate")
                        {
                            sql += @" and format(A.CreatedDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                        else if (DdlFltrDate.SelectedValue.ToString() == "UpdatedDate")
                        {
                            sql += @" and format(A.UpdatedDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                    }
                    else
                    {
                        //sql += @" and A.CreatedDate > convert(date, (SELECT CASE WHEN datepart(weekday, GETDATE()) >5 THEN DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) ELSE DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) END), 104) ";
                        //sql += @" and (A.CreatedDate > DATEADD(day,-7, (select MAX(CreatedDate) from TRealTimeVendorInventory)) and A.CreatedDate <= (select MAX(CreatedDate) from TRealTimeVendorInventory)) ";
                        if (txtDateUpdated.Text != "")
                        {
                            sql += @" and format(A.CreatedDate, 'yyyy-MM-dd') between @start and @end ";
                        }
                        else
                        {
                            sql += @" and (A.CreatedDate >= @beforeSchedule and A.CreatedDate <= (select MAX(CreatedDate) from TRealTimeVendorInventory)) ";
                        }
                    }


                    //if (SelectedVendor != "" || SelectedCustomerNo != "")
                    //{
                    //    if (SelectedVendor != "" && SelectedCustomerNo != "")
                    //    {
                    //        sql += @" and (A.VendorCode IN (" + SelectedVendor + ") OR A.CustomerNo IN (" + SelectedCustomerNo + ")) ";
                    //    }
                    //    else
                    //    {
                    //        if (SelectedVendor != "")
                    //        {
                    //            sql += @" AND A.VendorCode IN (" + SelectedVendor + ") ";
                    //        }
                    //        else if (SelectedCustomerNo != "")
                    //        {
                    //            sql += @" AND A.CustomerNo IN (" + SelectedCustomerNo + ") ";
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (txtDetailVendorCode.Text != "")
                    //    {
                    //        sql += @" and A.VendorCode like '%'+@VendorCode+'%' ";
                    //    }
                    //    else if (txtDetailCustomerNo.Text != "")
                    //    {
                    //        sql += @" and A.CustomerNo like '%'+@CustomerNo+'%' ";
                    //    }
                    //}

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "Material")
                        {
                            sql += @" and A.SAPCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "MaterialDesc")
                        {
                            sql += @" and M.MaterialDesc like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode")
                        {
                            sql += @" and A.VendorCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorDesc")
                        {
                            sql += @" and v.Description like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "CustomerNo")
                        {
                            sql += @" and A.CustomerNo like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "CustomerDesc")
                        {
                            sql += @" and Z.Customerdescription like '%'+@Filter+'%' ";
                        }
                    }
                    else
                    {

                        if (SelectedVendor != "" || SelectedCustomerNo != "")
                        {
                            if (SelectedVendor != "" && SelectedCustomerNo != "")
                            {
                                sql += @" and (A.VendorCode IN (" + SelectedVendor + ") OR A.CustomerNo IN (" + SelectedCustomerNo + ")) ";
                            }
                            else
                            {
                                if (SelectedVendor != "")
                                {
                                    sql += @" AND A.VendorCode IN (" + SelectedVendor + ") ";
                                }
                                else if (SelectedCustomerNo != "")
                                {
                                    sql += @" AND A.CustomerNo IN (" + SelectedCustomerNo + ") ";
                                }
                            }
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "CreatedDateOri" || ViewState["SortExpression"].ToString() == "UpdatedDate")
                        {
                            //sql += @" Order by CONVERT(DateTime, " + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                            if (ViewState["SortExpression"].ToString() == "CreatedDateOri")
                            {
                                sql += " ORDER BY A.CreatedDate " + " " + ViewState["SortDirection"].ToString() + " ";
                            }
                            else
                            {
                                sql += @" Order by A." + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                            }
                        }
                        else
                        {
                            sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                    }
                    else
                    {
                        sql += " ORDER BY A.VendorCode ASC, CreatedDateOri Desc ";
                    }

                    DateTime dateUpdated = DateTime.Now;
                    if (txtDateUpdated.Text != "")
                    {
                        dateUpdated = DateTime.ParseExact(txtDateUpdated.Text, "dd/MM/yyyy", null);
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@now", dateUpdated.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@VendorCode", txtDetailVendorCode.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", txtDetailCustomerNo.Text);
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
                        var convertdata = dt.AsEnumerable().Select(row => new
                        {
                            row_num = row["row_num"],
                            Plant = row["Plant"],
                            VendorCode = row["VendorCode"],
                            VndName = row["VndName"],
                            SAPCode = row["SAPCode"],
                            MaterialDesc = row["MaterialDesc"],
                            MaterialType = row["MaterialType"],
                            UOM = row["UOM"],
                            PirNo = row["PirNo"],
                            Stock = row["Stock"],
                            Remark = row["Remark"],
                            PIRDelFlag = row["PIRDelFlag"],
                            PlantStatus = row["PlantStatus"],
                            CustomerNo = row["CustomerNo"],
                            CustomerDesc = row["Customerdescription"],
                            CreatedBy = row["CreatedBy"],
                            CreatedDate = row["CreatedDate"],
                            CreatedDateOri = row["CreatedDateOri"],
                            UpdatedBy = row["UpdatedBy"],
                            UpdatedDate = row["UpdatedDate"],
                            IsOld = row["IsOld"]
                        });

                        string JsonResult = JsonConvert.SerializeObject(convertdata);
                        TxtDataJson.Text = JsonResult;

                        //int ShowEntry = 1;
                        //if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
                        //{
                        //    ShowEntry = 1;
                        //    TxtShowEntry.Text = "1";
                        //}
                        //else
                        //{
                        //    ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
                        //}
                        //GridView1.PageSize = ShowEntry;
                        //Session["ShowEntryReqWaitt"] = ShowEntry.ToString();
                        //Session["DTRTVendorSMN"] = dt;
                        //GridView1.DataSource = dt;
                        //GridView1.DataBind();
                        //if (dt.Rows.Count > 0)
                        //{
                        //    int Record = dt.Rows.Count;
                        //    LbTtlRecords.Text = "Total Record : " + Record.ToString();

                        //    #region return nested and pagination last view
                        //    //if (Session["ReqWaitPgNo"] != null)
                        //    //{
                        //    //    int ReqWaitPgNo = Convert.ToInt32(Session["ReqWaitPgNo"].ToString());
                        //    //    if (GridView1.PageCount >= ReqWaitPgNo)
                        //    //    {
                        //    //        GridView1.PageIndex = ReqWaitPgNo;
                        //    //        //GridView1.DataSource = dt;
                        //    //        GridView1.DataBind();
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        Session["ReqWaitPgNo"] = null;
                        //    //    }
                        //    //}
                        //    #endregion
                        //}
                        //else
                        //{
                        //    LbTtlRecords.Text = "Total Record : 0";
                        //}
                    }
                }
                //UpdatePanel1.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
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

        protected void ShowTableDet(string RequestNumber, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select VendorCode1,VendorName,QuoteNo,'" + RowParentGv + @"' as ParentGvRowNo from TQuoteDetails 
                            where RequestNumber not in(select distinct RequestNumber from TQuoteDetails where ApprovalStatus = 2 or ApprovalStatus = 1 or ApprovalStatus = 3  or ApprovalStatus is  null) 
                            and RequestNumber = '" + RequestNumber + "' and QuoteNoRef is null and (isMassRevision=0 or isMassRevision is null)  ";
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
                            sql += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + @".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = ProcessGroup) like '%'+@Filter+'%' ";
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

        //bool CekVendorVsMaterial(string Vendor, string material)
        //{
        //    SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
        //    try
        //    {
        //        EmetCon.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd/MM/yyyy') as RequestDate,format(QuoteResponseDueDate,'dd/MM/yyyy') as QuoteResponseDueDate,
        //                    QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
        //                    from TQuoteDetails where Material = @Material and VendorCode1 =@VendorCode1 and (ApprovalStatus in ('0','2')) and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') and RequestNumber <> @RequestNumber
        //                    --from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') and RequestNumber <> @RequestNumber 
        //                    ";

        //            cmd = new SqlCommand(sql, EmetCon);
        //            cmd.Parameters.AddWithValue("@Material", material);
        //            cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
        //            cmd.Parameters.AddWithValue("@RequestNumber", TxtModalReqNo.Text);
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                if (dt.Rows.Count == 0)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    if (Session["InvalidRequest"] == null)
        //                    {
        //                        Session["InvalidRequest"] = dt;
        //                    }
        //                    else
        //                    {
        //                        DataTable DtTemp = (DataTable)Session["InvalidRequest"];
        //                        DataRow dr = DtTemp.NewRow();
        //                        dr["Plant"] = dt.Rows[0]["Plant"].ToString();
        //                        dr["RequestNumber"] = dt.Rows[0]["RequestNumber"].ToString();
        //                        dr["RequestDate"] = dt.Rows[0]["RequestDate"].ToString();
        //                        dr["QuoteResponseDueDate"] = dt.Rows[0]["QuoteResponseDueDate"].ToString();
        //                        dr["QuoteNo"] = dt.Rows[0]["QuoteNo"].ToString();
        //                        dr["Material"] = dt.Rows[0]["Material"].ToString();
        //                        dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
        //                        dr["VendorCode1"] = dt.Rows[0]["VendorCode1"].ToString();
        //                        dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
        //                        DtTemp.Rows.Add(dr);
        //                        DtTemp.AcceptChanges();
        //                        Session["InvalidRequest"] = DtTemp;
        //                    }
        //                    return false;
        //                }
        //            }
        //            //UpdatePanel18.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //        return false;
        //    }
        //    finally
        //    {
        //        EmetCon.Dispose();
        //    }
        //}

        protected void GvInvalidRequest_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                try
                {
                    GetDbMaster();
                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        string Plant = e.Row.Cells[2].Text;
                        string VendorCode = e.Row.Cells[4].Text;
                        string SAPCode = e.Row.Cells[6].Text;
                        string CreatedDate = e.Row.Cells[1].Text;
                        string[] ArrCreatedDate = CreatedDate.Split('/');
                        string fixCreatedDate = ArrCreatedDate[2] + "-" + ArrCreatedDate[1] + "-" + ArrCreatedDate[0];
                        DateTime oDate = DateTime.Parse(fixCreatedDate);
                        sql = @"Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date, @beforeSchedule date, @ScheduleDay NVARCHAR(10), @daystoadd int, @SchedulePerMonth date
                            SET @ScheduleDay = (SELECT IDValue FROM " + DbMasterName + @".dbo.tGlobal WHERE id = 'Realtime_Inv_Mail_Remider_Day')
                            SET @SchedulePerMonth = CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)
IF @ScheduleDay = 'MON'
                            BEGIN
		                        SET @daystoadd = 0
	                        END
                        ELSE IF @ScheduleDay = 'TUE'
                            BEGIN
                                SET @daystoadd = 1
	                        END
                        ELSE IF @ScheduleDay = 'WED'
                            BEGIN
                                SET @daystoadd = 2
	                        END
                        ELSE IF @ScheduleDay = 'THU'
                            BEGIN
                                SET @daystoadd = 3
	                        END
                        ELSE IF @ScheduleDay = 'FRI'
                            BEGIN
                                SET @daystoadd = 4
	                        END
                        ELSE IF @ScheduleDay = 'SAT'
                            BEGIN
                                SET @daystoadd = 5
	                        END
                        ELSE IF @ScheduleDay = 'SUN'
                            BEGIN
		                        SET @daystoadd = 6
	                        END
                        set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false')
                        set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                        set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                        While @counter <= @max
                        Begin

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @SchedulePerMonth)
                        BEGIN
	                        SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                        BEGIN
	                        SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["EPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
                        BEGIN
	                        SET @nextSchedule = (DATEADD(day, -1, @nextSchedule))
	                        SET @totalHoliday = @totalHoliday + 1
	                        SET @isHoliday = 1
                        END
                        ELSE
                        BEGIN
	                        SET @isHoliday = 0
	                        --BREAK;
                        END

                        SET @counter = @counter + 1

                        End
SELECT * FROM TRealTimeVendorInventory WHERE CreatedDate < CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END
                          --AND CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) 
                            AND Plant = @Plant AND VendorCode = @VendorCode AND SAPCode = @SAPCode AND CreatedDate = @CreatedDate";

                        //OlD
                        //sql = @"SELECT * FROM TRealTimeVendorInventory WHERE CreatedDate < convert(date, (SELECT CASE WHEN datepart(weekday, GETDATE()) >5 THEN DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) ELSE DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) END), 104) 
                        //  --AND CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) 
                        //    AND Plant = @Plant AND VendorCode = @VendorCode AND SAPCode = @SAPCode AND CreatedDate = @CreatedDate";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@Plant", Plant);
                        cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                        cmd.Parameters.AddWithValue("@SAPCode", SAPCode);
                        cmd.Parameters.AddWithValue("@CreatedDate", fixCreatedDate);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4F1F0");
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
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    try
            //    {
            //        e.Row.Cells[12].ColumnSpan = 2;
            //        e.Row.Cells[13].Visible = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //        EMETModule.SendExcepToDB(ex);
            //    }
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    try
            //    {

            //        int RowParentGv = e.Row.DataItemIndex;
            //        GridView GvDet = e.Row.FindControl("GvDet") as GridView;
            //        //string idCollege = grdview.DataKeys[e.Row.RowIndex].Value.ToString();
            //        string iBranc = e.Row.Cells[3].Text;
            //        ReqNo = iBranc;
            //        ShowTableDet(iBranc, (RowParentGv + 1));
            //        DataTable DtDetReqNo = new DataTable();
            //        DtDetReqNo = (DataTable)Session["TableDet"];
            //        GvDet.DataSource = DtDetReqNo;
            //        GvDet.DataBind();
            //    }
            //    catch (Exception ex)
            //    {
            //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //        EMETModule.SendExcepToDB(ex);
            //    }

            //}

            //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            //{
            //    SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            //    try
            //    {
            //        EmetCon.Open();
            //        using (SqlDataAdapter sda = new SqlDataAdapter())
            //        {
            //            ReqNo = e.Row.Cells[3].Text;
            //            sql = @"select * from TQuoteDetails where QuoteResponseDueDate <=GETDATE() and ApprovalStatus=0 and RequestNumber=@quotenumber";
            //            cmd = new SqlCommand(sql, EmetCon);
            //            cmd.Parameters.AddWithValue("@quotenumber", ReqNo);
            //            sda.SelectCommand = cmd;
            //            using (DataTable dt = new DataTable())
            //            {
            //                sda.Fill(dt);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    Button BtnApprove = (Button)e.Row.FindControl("BtnApprove");
            //                    Button BtnReject = (Button)e.Row.FindControl("BtnReject");
            //                    if (BtnApprove != null)
            //                    {
            //                        BtnApprove.Enabled = true;
            //                        BtnReject.Enabled = true;
            //                    }
            //                    //e.Row.Cells[9].Enabled = true;
            //                    //e.Row.Cells[10].Enabled = true;
            //                }
            //                else
            //                {
            //                    Button BtnApprove = (Button)e.Row.FindControl("BtnApprove");
            //                    Button BtnReject = (Button)e.Row.FindControl("BtnReject");
            //                    if (BtnApprove != null)
            //                    {
            //                        BtnApprove.Enabled = false;
            //                        BtnReject.Enabled = false;
            //                    }
            //                    //e.Row.Cells[9].Enabled = false;
            //                    //e.Row.Cells[10].Enabled = false;
            //                }
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //        EMETModule.SendExcepToDB(ex);
            //    }
            //    finally
            //    {
            //        EmetCon.Dispose();
            //    }
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            //{
            //    try
            //    {
            //    }
            //    catch (Exception ex)
            //    {
            //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //        EMETModule.SendExcepToDB(ex);
            //    }
            //}
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //try
            //{
            //    GridView1.PageIndex = e.NewPageIndex;
            //    Session["ReqWaitPgNo"] = (GridView1.PageIndex).ToString();

            //    if (Session["DTRTVendorSMN"] != null)
            //    {
            //        DataTable dt = new DataTable();
            //        dt = (DataTable)Session["DTRTVendorSMN"];
            //        GridView1.DataSource = dt;
            //        int ShowEntry = 1;
            //        if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
            //        {
            //            ShowEntry = 1;
            //            TxtShowEntry.Text = "1";
            //        }
            //        else
            //        {
            //            var regexItem = new Regex("^[0-9]+$");
            //            if (regexItem.IsMatch(TxtShowEntry.Text))
            //            {
            //                ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
            //            }
            //            else
            //            {
            //                ShowEntry = 1;
            //                TxtShowEntry.Text = "1";
            //            }
            //            //ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
            //        }
            //        GridView1.PageSize = ShowEntry;
            //        GridView1.DataBind();
            //    }
            //    //ShowTable();
            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();", true);
            //}
            //catch (Exception ex)
            //{
            //    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //    EMETModule.SendExcepToDB(ex);
            //}
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{

            //    if (e.CommandName == "Approve")
            //    {
            //        int rowIndex = Convert.ToInt32(e.CommandArgument);
            //        GridViewRow row = GridView1.Rows[rowIndex];
            //        string ReqNumber = row.Cells[3].Text;

            //        GetDataVendUpdateDueDate(ReqNumber);
            //        TxtModalReqNo.Text = ReqNumber;
            //        TxtModalDueDate.Text = row.Cells[5].Text;
            //        TxtMaterial.Text = row.Cells[8].Text;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();DatePitcker();", true);
            //        if (Session["ReqWaitNst"] != null)
            //        {
            //            string RowVsStatus = Session["ReqWaitNst"].ToString();
            //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
            //        }
            //        DvInvalidRequest.Visible = false;
            //        upModal.Update();

            //        //Response.Redirect("DateUpdate.aspx?Number=" + ReqNumber.ToString());
            //    }
            //    else if (e.CommandName == "Reject")
            //    {
            //        int rowIndex = Convert.ToInt32(e.CommandArgument);

            //        //Reference the GridView Row.
            //        GridViewRow row = GridView1.Rows[rowIndex];
            //        string Reason = "Quotation Canceled";
            //        string ReqNumber = row.Cells[3].Text;
            //        string Vendor = "";
            //        //subash
            //        if (Reason.ToString() != "")
            //        {
            //            UpdateGridData(ReqNumber, Vendor, 1, Reason);
            //            ShowTable();
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Plz fill  the Reason to Reject')", true);
            //        }
            //    }
            //    else if (e.CommandName == "TrgNestedExpand")
            //    {
            //        int rowIndex = Convert.ToInt32(e.CommandArgument);
            //        string RowVsStatus = rowIndex.ToString() + "-" + "Ex";
            //        Session["ReqWaitNst"] = RowVsStatus;
            //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');DatePitcker();", true);
            //    }
            //    else if (e.CommandName == "TrgNestedColapse")
            //    {
            //        int rowIndex = Convert.ToInt32(e.CommandArgument);
            //        string RowVsStatus = rowIndex.ToString() + "-" + "Colp";
            //        Session["ReqWaitNst"] = RowVsStatus;
            //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "');DatePitcker();", true);
            //    }
            //}
            //catch (ThreadAbortException ex2)
            //{

            //}
            //catch (Exception ex)
            //{
            //    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //    EMETModule.SendExcepToDB(ex);
            //}
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
                            string str1 = "Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + "',PICRejRemark = '" + pic + "', ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "',  UpdatedBy='" + userID + "', UpdatedOn='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "' where RequestNumber = '" + ReqNum + "'";

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
                                string Subject = "Request Number" + ReqNum.ToString() + " |Quotation Number: " + quoteno.ToString() + " - Shimano Approval By : " + nameC;
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
                if (Session["ReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
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
                //UpdatePanel1.Update();
                TxtFrom.Text = "";
                TxtTo.Text = "";
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                if (Session["ReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
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
                //UpdatePanel1.Update();
                txtFind.Text = "";
                ShowTable();

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                if (Session["ReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
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
                if (TxtFrom.Text != "" && TxtTo.Text != "")
                {
                    ShowTable();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading()", true);
                }

                //string column = "";
                //string sortDirection = "";
                //string FilterBy = "";
                //string TxtFnd = "";
                //string FilterDate = "";
                //string DateBetween = "";
                //if (Session["ReqWaitFilter"] == null)
                //{
                //    FilterBy = DdlFilterBy.SelectedValue;
                //    TxtFnd = txtFind.Text;
                //    FilterDate = DdlFltrDate.SelectedValue;
                //    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                //    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                //}
                //else
                //{
                //    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                //    column = ArrReqWaitFilter[0].ToString();
                //    sortDirection = ArrReqWaitFilter[1].ToString();
                //    FilterBy = DdlFilterBy.SelectedValue;
                //    TxtFnd = txtFind.Text;
                //    FilterDate = DdlFltrDate.SelectedValue;
                //    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                //    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                //}
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
                if (TxtFrom.Text != "" && TxtTo.Text != "")
                {
                    ShowTable();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading()", true);
                }

                //string column = "";
                //string sortDirection = "";
                //string FilterBy = "";
                //string TxtFnd = "";
                //string FilterDate = "";
                //string DateBetween = "";
                //if (Session["ReqWaitFilter"] == null)
                //{
                //    FilterBy = DdlFilterBy.SelectedValue;
                //    TxtFnd = txtFind.Text;
                //    FilterDate = DdlFltrDate.SelectedValue;
                //    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                //    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                //}
                //else
                //{
                //    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                //    column = ArrReqWaitFilter[0].ToString();
                //    sortDirection = ArrReqWaitFilter[1].ToString();
                //    FilterBy = DdlFilterBy.SelectedValue;
                //    TxtFnd = txtFind.Text;
                //    FilterDate = DdlFltrDate.SelectedValue;
                //    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                //    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                //}
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

                txtDetailVendorCode.Text = "";
                txtDetailCustomerNo.Text = "";
                txtDateUpdated.Text = "";

                string column = "";
                string sortDirection = "";
                string FilterBy = "";
                string TxtFnd = "";
                string FilterDate = "";
                string DateBetween = "";
                if (Session["ReqWaitFilter"] == null)
                {
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }
                else
                {
                    string[] ArrReqWaitFilter = Session["ReqWaitFilter"].ToString().Split('!');
                    column = ArrReqWaitFilter[0].ToString();
                    sortDirection = ArrReqWaitFilter[1].ToString();
                    FilterBy = DdlFilterBy.SelectedValue;
                    TxtFnd = txtFind.Text;
                    FilterDate = DdlFltrDate.SelectedValue;
                    DateBetween = TxtFrom.Text + "~" + TxtTo.Text;
                    Session["ReqWaitFilter"] = column + "!" + sortDirection + "!" + FilterBy + "!" + TxtFnd + "!" + FilterDate + "!" + DateBetween;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "showDetailDiv();CloseLoading();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showDetailDiv();DatePitcker();", true);
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
                Session["ReqWaitFilter"] = null;
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

        protected void GvDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                string[] CmdArg = (e.CommandArgument).ToString().Split('|');

                if (e.CommandName == "LinktoRedirect")
                {
                    Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
                    //Response.Redirect("Eview.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
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
            //SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            //try
            //{
            //    EmetCon.Open();
            //    Session["InvalidRequest"] = null;
            //    bool CanUpdate = true;
            //    if (grdvendor.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < grdvendor.Rows.Count; i++)
            //        {
            //            if (CekVendorVsMaterial(grdvendor.Rows[i].Cells[0].Text, TxtMaterial.Text) == false)
            //            {
            //                CanUpdate = false;
            //            }
            //        }
            //    }

            //    if (CanUpdate == true)
            //    {
            //        string CurentDate = DateTime.Now.ToString("dd/MM/yyyy");
            //        DateTime DtCurentDate = DateTime.ParseExact(CurentDate, "dd/MM/yyyy", null);
            //        DateTime DtDueDate = DateTime.ParseExact(TxtModalDueDate.Text, "dd/MM/yyyy", null);

            //        int result = DateTime.Compare(DtDueDate, DtCurentDate);
            //        if ((result > 0) && (TxtModalDueDate.Text != ""))
            //        {
            //            sql = "update TQuoteDetails set QuoteResponseDueDate = @QuoteResponseDueDate where RequestNumber = @RequestNumber ";
            //            cmd = new SqlCommand(sql, EmetCon);
            //            cmd.Parameters.AddWithValue("@RequestNumber", TxtModalReqNo.Text);
            //            cmd.Parameters.AddWithValue("@QuoteResponseDueDate", DtDueDate.ToString("yyyy-MM-dd"));
            //            cmd.ExecuteNonQuery();
            //            ShowTable();
            //            UpdatePanel1.Update();
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Updated !');CloseLoading();", true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Date should be greater than current date');CloseLoading();", true);
            //        }

            //        if (Session["ReqWaitNst"] != null)
            //        {
            //            string RowVsStatus = Session["ReqWaitNst"].ToString();
            //            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
            //        }
            //    }
            //    else
            //    {
            //        if (Session["InvalidRequest"] != null)
            //        {
            //            DataTable DtTemp = (DataTable)Session["InvalidRequest"];
            //            if (DtTemp.Rows.Count > 0)
            //            {
            //                GvInvalidRequest.DataSource = DtTemp;
            //                GvInvalidRequest.DataBind();
            //                DvInvalidRequest.Visible = true;
            //            }
            //            else
            //            {
            //                DvInvalidRequest.Visible = false;
            //            }
            //        }
            //        else
            //        {
            //            DvInvalidRequest.Visible = false;
            //        }
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "CloseLoading();", true);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //    EMETModule.SendExcepToDB(ex);
            //}
            //finally
            //{
            //    EmetCon.Dispose();
            //}
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();CloseLoading();", true);
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

        protected void BtnDownExport_Click(object sender, EventArgs e)
        {
            try
            {
                GetDataFortemplate();
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

        protected void GetDataFortemplate()
        {
            string path = "";
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" Select A.Plant, T.Description as PlantName, A.VendorCode, substring(( v.Description),1,12) +' ...' as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, 
                            CASE WHEN A.CreatedBy is null THEN null ELSE CONCAT(A.CreatedBy,' - ', U.UseNam) END as CreatedBy,
                            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                            A.CreatedDate as CreatedDateOri,
                            CASE WHEN A.UpdatedBy is null THEN null ELSE CONCAT(A.UpdatedBy,' - ', C.UseNam) END as UpdatedBy,
                            FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                            From TRealTimeVendorInventory A
                            join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                            join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                            join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                            left join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID
                            left join " + DbMasterName + @".dbo.Usr C on A.UpdatedBy = C.UseID 
                            WHERE A.Plant = @Plant ";

                    //sql = @" Select distinct A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                    //        A.Stock, A.Remark, U.UseNam  as CreatedBy, FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                    //        A.UpdatedBy, FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                    //        From TRealTimeVendorInventory A
                    //        join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                    //        join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                    //        join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                    //        join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                    //        join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID WHERE A.Plant = @Plant ";

                    if (TxtFrom.Text != "" && TxtTo.Text != "")
                    {
                        if (DdlFltrDate.SelectedValue.ToString() == "CreatedDate")
                        {
                            sql += @" and format(A.CreatedDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                        else if (DdlFltrDate.SelectedValue.ToString() == "UpdatedDate")
                        {
                            sql += @" and format(A.UpdatedDate, 'yyyy-MM-dd') between @From and @To ";
                        }
                    }
                    else
                    {
                        //sql += @" and A.CreatedDate > convert(date, (SELECT CASE WHEN datepart(weekday, GETDATE()) >5 THEN DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) ELSE DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) END), 104) ";
                        sql += @" and (A.CreatedDate > DATEADD(day,-7, (select MAX(CreatedDate) from TRealTimeVendorInventory)) and A.CreatedDate <= (select MAX(CreatedDate) from TRealTimeVendorInventory)) ";
                    }

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "Material")
                        {
                            sql += @" and A.SAPCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "MaterialDesc")
                        {
                            sql += @" and M.MaterialDesc like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorCode")
                        {
                            sql += @" and A.VendorCode like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "VendorDesc")
                        {
                            sql += @" and v.Description like '%'+@Filter+'%' ";
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "CreatedDateOri")
                        {
                            sql += " ORDER BY A.CreatedDate " + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                        else
                        {
                            if (ViewState["SortExpression"].ToString() == "UpdatedDate")
                            {
                                sql += @" Order by CONVERT(DateTime, " + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                            }
                            else
                            {
                                sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                            }
                        }
                    }
                    else
                    {
                        sql += " ORDER BY CreatedDateOri DESC ";
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
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
                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    //using (DataTable dt = new DataTable())
                    //{
                    //    sda.Fill(dt);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        string fname = "VendorRealTimeInventory";
                    //        DateTime Dat1 = DateTime.Now;
                    //        fname = fname + Convert.ToString(Dat1.ToString()) + ".xls";

                    //        string attachment = "attachment; filename=" + fname + "";
                    //        Response.ClearContent();
                    //        Response.AddHeader("content-disposition", attachment);
                    //        Response.ContentType = "application/vnd.ms-excel";
                    //        string tab = "";
                    //        foreach (DataColumn dc in dt.Columns)
                    //        {
                    //            Response.Write(tab + dc.ColumnName);
                    //            tab = "\t";
                    //        }
                    //        Response.Write("\n");
                    //        int i;
                    //        foreach (DataRow dr in dt.Rows)
                    //        {
                    //            tab = "";
                    //            for (i = 0; i < dt.Columns.Count; i++)
                    //            {
                    //                Response.Write(tab + dr[i].ToString());
                    //                tab = "\t";
                    //            }
                    //            Response.Write("\n");
                    //        }
                    //        Response.End();
                    //    }
                    //}
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    //string worksheetName = "sheet1";
                    string fname = "VendorRealTimeInventory";
                    DateTime Dat1 = DateTime.Now;
                    fname = fname + Convert.ToString(Dat1.ToString());
                    fname = fname.Replace(':', '_');
                    fname = fname.Replace('/', '_');

                    ////  get Application object.
                    //excel = new Microsoft.Office.Interop.Excel.Application();
                    //excel.Visible = false;
                    //excel.DisplayAlerts = false;

                    //// Creation a new Workbook
                    //excelworkBook = excel.Workbooks.Add(Type.Missing);

                    //// Workk sheet
                    //excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                    //excelSheet.Name = worksheetName;

                    //// loop through each row and add values to our sheet
                    //int rowcount = 1;
                    ////excelSheet.Cells[1, 1] = "(Please Don't Delete Highlighted Row)";
                    ////for (int i = 1; i < 15; i++)
                    ////{
                    ////    excelSheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    ////}

                    //foreach (DataRow datarow in dt.Rows)
                    //{
                    //    rowcount += 1;
                    //    for (int i = 1; i <= dt.Columns.Count; i++)
                    //    {
                    //        // on the first iteration we add the column headers
                    //        if (rowcount == 2)
                    //        {
                    //            excelSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                    //            excelSheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    //        }
                    //        // Filling the excel file 
                    //        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
                    //        //if (i <= 7)
                    //        //{
                    //        //    excelSheet.Cells[rowcount, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    //        //}
                    //        if (i == dt.Columns.Count)
                    //        {
                    //            //excelSheet.Columns[i].ColumnWidth = 15;
                    //            excelSheet.Columns[i].AutoFit();
                    //        }
                    //    }
                    //}

                    //string folder = Server.MapPath("/Files/Real Time Vend Inv/Export/");
                    //if (!Directory.Exists(folder)) //CHECK IF FOLDER EXIST
                    //{
                    //    Directory.CreateDirectory(folder);  // CREATE FOLDER IF NOT EXIST
                    //}
                    //path = folder + fname;

                    ////now save the workbook and exit Excel
                    ////excelworkBook.SaveAs(folder);
                    //excelworkBook.SaveAs(path);
                    //excelworkBook.Close();
                    //excel.Quit();

                    //Response.ContentType = "application/octet-stream";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + fname);
                    //Response.TransmitFile(path);
                    //Response.Flush();
                    //Response.End();

                    var workbook = new XLWorkbook();
                    var ws = workbook.Worksheets.Add(dt, "Sheet1");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + fname + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
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
            finally
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                EmetCon.Dispose();
            }
        }

        protected void TxtShowEntry_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseLoading();", true);
            //    UpdatePanel1.Update();
            //    if (Session["DTRTVendorSMN"] != null)
            //    {
            //        DataTable dt = new DataTable();
            //        dt = (DataTable)Session["DTRTVendorSMN"];
            //        GridView1.DataSource = dt;
            //        int ShowEntry = 1;
            //        if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
            //        {
            //            ShowEntry = 1;
            //            TxtShowEntry.Text = "1";
            //        }
            //        else
            //        {
            //            var regexItem = new Regex("^[0-9]+$");
            //            if (regexItem.IsMatch(TxtShowEntry.Text))
            //            {
            //                ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
            //            }
            //            else
            //            {
            //                ShowEntry = 1;
            //                TxtShowEntry.Text = "1";
            //            }
            //            //ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
            //        }
            //        GridView1.PageSize = ShowEntry;
            //        GridView1.DataBind();
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
            //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading()", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //    EMETModule.SendExcepToDB(ex);
            //}
        }

        protected void btnLatestUpdated_Click(object sender, EventArgs e)
        {
            ShowModalLatestUpdated();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LatestModal", "openModalLatestSummary();", true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalLatestSummary();", true);
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }
    }
}