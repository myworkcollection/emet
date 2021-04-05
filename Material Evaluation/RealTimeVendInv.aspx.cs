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
using System.Text;
using System.Collections;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Material_Evaluation
{
    public partial class RealTimeVendInv : System.Web.UI.Page
    {
        string userId;
        string sname;
        string srole;
        string concat;
        string mappeduserid;
        string mappedname;

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        string DbMasterName = "";
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        string ReqNo;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["userID_"] == null || Session["UserName"] == null || Session["VPlant"].ToString() == null || Session["mappedVendor"].ToString() == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }

                else
                {
                    if (!Page.IsPostBack)
                    {
                        FileUpload.Attributes.Clear();
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_RealTimeVendInv";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            var custNo = getCustNO();
                            var custName = "";
                            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
                            MDMCon.Open();
                            sql = "select Plant, UseID, CustomerNo, substring((Description),1,14) as Description from TUSERVSCUSTOMER WHERE UseID = @UseID";
                            cmd = new SqlCommand(sql, MDMCon);
                            cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                            cmd.CommandTimeout = 0;
                            reader = cmd.ExecuteReader();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    custName = reader["Description"].ToString();
                                }
                            }
                            reader.Close();
                            MDMCon.Dispose();
                            MDMCon.Close();

                            userId = Session["userID_"].ToString();
                            sname = Session["UserName"].ToString();
                            srole = Session["userType"].ToString();
                            mappeduserid = Session["mappedVendor"].ToString();
                            mappedname = Session["mappedVname"].ToString();
                            concat = sname + " - " + mappedname;
                            lblUser.Text = sname;
                            if (custNo == "")
                            {
                                lblplant.Text = mappedname;
                            }
                            else
                            {
                                lblplant.Text = custName;
                            }
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            // Session["UserName"] = userId;
                            //      string strprod = txtplant.Text;

                            //GetGridData();
                            LastFilterCondition();
                            //if (Session["ShowEntryClosedStatus"] != null)
                            //{
                            //    TxtShowEntry.Text = Session["ShowEntryClosedStatus"].ToString();
                            //}
                            ShowMainTable();
                            //ShowTable();

                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }

                            if (Session["VPlant"] != null || Session["mappedVendor"] != null)
                            {
                                string VndPlant = Session["VPlant"].ToString();
                                string VndCode = Session["mappedVendor"].ToString();
                                string VndUserID = Session["userID_"].ToString();
                                string VndCustomerNo = "";

                                if (VndUserID.Contains(VndCode))
                                {
                                    VndCustomerNo = getCustNO();
                                }

                                if (EMETModule.isRltInv(VndPlant, VndCode) == true)
                                {
                                    btnUpload.Enabled = true;
                                    btnTemplate.Enabled = true;
                                }
                                else
                                {
                                    if (isCustomerRltInv(VndPlant, VndCustomerNo, VndUserID) == true)
                                    {

                                        btnUpload.Enabled = true;
                                        btnTemplate.Enabled = true;
                                    }
                                    else
                                    {
                                        btnUpload.Enabled = false;
                                        btnTemplate.Enabled = false;
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "disabledButtonEdit();", true);
                                    }
                                }
                            }
                        }

                        if (Session["UnreadAnn"] != null)
                        {
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
                            LiUnReadAnn.Style.Add("display", "none");
                            lbUnreadAnn.Text = "";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "sortingShowLoading();DatePitcker();GenerateSearchByColumn();GenerateNewMainTable();GenerateTbData();CloseLoading()", true);

                        if (Session["VPlant"] != null || Session["mappedVendor"] != null)
                        {
                            string VndPlant = Session["VPlant"].ToString();
                            string VndCode = Session["mappedVendor"].ToString();
                            string VndUserID = Session["userID_"].ToString();
                            string VndCustomerNo = "";

                            if (VndUserID.Contains(VndCode))
                            {
                                VndCustomerNo = getCustNO();
                            }
                            if (EMETModule.isRltInv(VndPlant, VndCode) != true && isCustomerRltInv(VndPlant, VndCustomerNo, VndUserID) != true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "disabledButtonEdit();", true);
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

        public string getCustNO()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            MDMCon.Open();
            string custno = "";
            sql = @"SELECT * FROM TUSERVSCUSTOMER WHERE UseID = @UseID";
            cmd = new SqlCommand(sql, MDMCon);
            cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
            cmd.CommandTimeout = 0;
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    custno = reader["CustomerNo"].ToString();
                }
            }
            reader.Close();
            MDMCon.Dispose();
            MDMCon.Close();

            return custno;
        }

        public string getVendorCodeByCustNo(string CustomerNo)
        {
            string VendorCode = "";

            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"SELECT * FROM tVendor_New WHERE CustomerNo = @CustomerNo";
                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@CustomerNo", CustomerNo);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        VendorCode = reader["Vendor"].ToString();
                    }
                    reader.Dispose();
                    return VendorCode;
                }
                else
                {
                    reader.Dispose();
                    return VendorCode;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        public bool isCustomerRltInv(string Plant, string CustomerNo, string UserID)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"SELECT DISTINCT Plant, UseID, CustomerNo 
                        FROM TUSERVSCUSTOMER
                        WHERE Plant = @Plant AND UseID = @UseID AND CustomerNo = @CustomerNo";
                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@Plant", Plant);
                cmd.Parameters.AddWithValue("@CustomerNo", CustomerNo);
                cmd.Parameters.AddWithValue("@UseID", UserID);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Dispose();
                    return true;
                }
                else
                {
                    reader.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
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

        protected void ShowMainTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string VendorCode = "";
                string VndCustomerNo = getCustNO();
                string VndPlant = Session["VPlant"].ToString();
                string VndUserID = Session["userID_"].ToString();
                bool isCustomer = isCustomerRltInv(VndPlant, VndCustomerNo, VndUserID);
                if (isCustomer == true)
                {
                    VendorCode = getVendorCodeByCustNo(VndCustomerNo);
                }
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string FilterBy = "";
                    if (isCustomer == true)
                    {
                        FilterBy = "A.CustomerNo = ''" + VndCustomerNo + "'' ";
                    }
                    else
                    {
                        FilterBy = "A.VendorCode = ''" + Session["mappedVendor"].ToString() + "'' ";
                    }
                    sql = @"
                            SET DATEFIRST 1;
                            Declare @sql nvarchar(max), @cols varchar(max)
                            Declare @CurrentWeekNumber int = (select Datepart(week, GETDATE()))
                            Declare @FilterBy nvarchar(max) = '" + FilterBy + @"'

                            set @cols = '['+ CAST((@CurrentWeekNumber-3) AS nvarchar(MAX)) +'],'+'['+ CAST((@CurrentWeekNumber-2) AS nvarchar(MAX)) +'],'++'['+ CAST((@CurrentWeekNumber-1) AS nvarchar(MAX)) +'],'+'['+ CAST((@CurrentWeekNumber) AS nvarchar(MAX)) +']';

                            set @sql = N'
                                select DISTINCT pvt.Plant, pvt.VendorCode, substring((V.Description),1,12) +'' ...'' as VendorDesc, pvt.CustomerNo, substring((Z.Customerdescription),1,12) +'' ...'' as CustomerDesc, ' + @cols + ', '+CAST(@CurrentWeekNumber as nvarchar(max))+' as CurrentWeekNumber from
                                (
                                    SELECT DISTINCT A.Plant, A.VendorCode, A.CustomerNo,Datepart(week, A.CreatedDate) AS ''UploadOnWeek'',A.CreatedDate as ''CreatedDate''
                            FROM TRealTimeVendorInventory A
                            left join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            left join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                            left join " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z on A.CustomerNo = Z.Customer AND A.SAPCode = Z.Material
                            WHERE A.Plant = '+@Plant+' AND '+@FilterBy+'
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
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
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

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string VendorCode = "";
                string VndCustomerNo = getCustNO();
                string VndPlant = Session["VPlant"].ToString();
                string VndUserID = Session["userID_"].ToString();
                bool isCustomer = isCustomerRltInv(VndPlant, VndCustomerNo, VndUserID);
                if (isCustomer == true)
                {
                    VendorCode = getVendorCodeByCustNo(VndCustomerNo);
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
                            set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = @Plant and DelFlag = 'false')
                            set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                            set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                            While @counter <= @max
                            Begin

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = @Plant and DelFlag = 'false' and [date] = @SchedulePerMonth)
                            BEGIN
	                            SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                            END

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = @Plant and DelFlag = 'false' and [date] = @beforeSchedule)
                            BEGIN
	                            SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                            END

                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = @Plant and DelFlag = 'false' and [date] = @nextSchedule)
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
                           PARTITION BY A.CreatedDate
	                        ORDER BY A.CreatedDate desc
                           ) row_num,
                            A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc, M.MaterialType,A.UOM, A.PirNo, 
                            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, A.CustomerNo, Z.Customerdescription,
                            CASE WHEN A.CreatedBy is null THEN null ELSE CONCAT(A.CreatedBy,' - ', U.UseNam) END as CreatedBy,
                            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                            A.CreatedDate as CreatedDateOri,
                            CASE WHEN A.UpdatedBy is null THEN null ELSE CONCAT(A.UpdatedBy,' - ', C.UseNam) END as UpdatedBy,
                            --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                            A.UpdatedDate, case when exists (SELECT * FROM TRealTimeVendorInventory WHERE CreatedDate < CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END
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
                            WHERE A.Plant = @Plant AND ";

                            if (isCustomer == true)
                            {
                                sql += "A.CustomerNo = @CustomerNo ";
                            }
                            else
                            {
                                sql += "A.VendorCode = @VendorCode ";
                            }


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
                                sql += @" Order by A." + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";

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

                    DateTime dateUpdated = DateTime.Now;
                    if (txtDateUpdated.Text != "")
                    {
                        dateUpdated = DateTime.ParseExact(txtDateUpdated.Text, "dd/MM/yyyy", null);
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    cmd.Parameters.AddWithValue("@now", dateUpdated.ToString("yyyy-MM-dd"));

                    //if (isCustomer == true)
                    //{
                    //    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    //}

                    if (isCustomer == true)
                    {
                        cmd.Parameters.AddWithValue("@CustomerNo", VndCustomerNo);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
                    }

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
                            PlantName = row["PlantName"],
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
                        string test = TxtDataJson.Text.ToString();
                        UpForm.Update();
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
                        //GridView1.DataSource = dt;
                        //Session["DTRTVendor"] = dt;
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

        protected void GetDataFortemplate()
        {
            string path = "";
            string custno = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct P.Plant,P.Vendor,V.Description as VndName,P.Material,M.MaterialDesc,M.BaseUOM,
                    //                 Q.InfoRecord as PIRno,'' as Stock,'' as Remark,
                    //case 
                    //when RTRIM(LTRIM(concat(isnull(ffd,''),isnull(POrgDeleteFlag,''))))= '' then ''
                    //else 'X' end as 'PirDelFlag',
                    //m.PlantStatus
                    //                 from tPir_New P 
                    //                 join tPIRvsQuotation Q on P.Plant = Q.Plant and p.Vendor = Q.Vendor and P.Material = Q.Material
                    //                 join tVendorPOrg O on P.Vendor = O.Vendor and P.Plant = O.Plant and isnull(O.DelFlag,0) = 0
                    //                 join tVendor_New V on P.Vendor = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor and isnull(v.DelFlag,0) = 0
                    //                 join TMATERIAL M on P.Plant = M.Plant and P.Material = M.Material
                    //                  ";

                    sql = @"SELECT * FROM TUSERVSCUSTOMER WHERE UseID = @UseID";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                    cmd.CommandTimeout = 0;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            custno = reader["CustomerNo"].ToString();
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        sql = @"SELECT * FROM tVendor_New WHERE Vendor = @Vendor";
                        cmd = new SqlCommand(sql, MDMCon);
                        cmd.Parameters.AddWithValue("@Vendor", Session["mappedVendor"].ToString());
                        cmd.CommandTimeout = 0;
                        reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                custno = reader["CustomerNo"].ToString();
                            }
                            reader.Close();
                        }
                        else
                        {
                            reader.Close();
                        }
                    }

                    sql = @"SELECT DISTINCT P.Plant, P.Vendor, V.Description AS VndName, V.CustomerNo AS CustomerNo, V.Description as Customerdescription, P.Material, M.MaterialDesc, '' AS Stock, M.BaseUOM, '' AS Remark, M.MaterialType, Q.InfoRecord AS PIRno, 
                            Case 
	                            WHEN RTRIM(LTRIM(concat(isnull(FFD,''),isnull(POrgDeleteFlag,''))))= '' THEN ''
	                            ELSE 'X' 
                            END AS 'PirDelFlag', M.PlantStatus INTO #temp1
                            FROM tPir_New P 
                            JOIN tPIRvsQuotation Q ON P.Plant = Q.Plant AND P.Vendor = Q.Vendor AND P.Material = Q.Material
                            JOIN tVendorPOrg O ON P.Vendor = O.Vendor AND P.Plant = O.Plant AND isnull(O.DelFlag,0) = 0
                            JOIN tVendor_New V ON P.Vendor = V.Vendor AND V.POrg = O.POrg AND O.Vendor= V.Vendor and ISNULL(V.DelFlag,0) = 0
                            JOIN TMATERIAL M ON P.Plant = M.Plant AND P.Material = M.Material ";

                    if (rbActiveMaterial.Checked == true)
                    {
                        sql += @" WHERE (P.Plant = @Plant AND P.Vendor = @vendor
                                AND ISNULL(M.PlantStatus,'') in ('','Z2') AND M.MaterialType <> 'FERT' 
                                AND ISNULL(Q.POrgDeleteFlag,'') = '' AND ISNULL(q.FFD,'') = '') 
                                OR
                                (P.Plant = @Plant AND P.Vendor = @vendor
                                AND ISNULL(M.PlantStatus,'') in ('','Z1') AND M.MaterialType = 'FERT' 
                                AND ISNULL(Q.POrgDeleteFlag,'') = '' AND isnull(q.FFD,'') = '') ";
                    }
                    else
                    {
                        sql += @" WHERE P.Plant = @Plant and P.Vendor = @vendor ";
                    }

                    //sql += @"SELECT DISTINCT P.Plant, V.Vendor as Vendor, V.Description AS VndName, M.MaterialType, P.Material, P.Materialdescription AS MaterialDesc,  
                    //         M.BaseUOM, null AS PIRno, '' AS Stock, '' AS Remark, '' AS 'PirDelFlag', M.PlantStatus, P.Customer AS CustomerNo, P.Customerdescription INTO #temp2 
                    //         FROM TCUSTOMER_MATLPRICING P
                    //         --JOIN TUSERVSCUSTOMER V ON P.Customer = V.CustomerNo and isnull(v.DelFlag, 0) = 0
                    //         LEFT JOIN tVendor_New V ON P.Customer = V.CustomerNo and isnull(V.DelFlag,0) = 0
                    //         LEFT JOIN tVendorPOrg O ON V.Vendor = O.Vendor AND V.POrg = O.POrg AND P.Plant = O.Plant AND isnull(O.DelFlag,0) = 0
                    //         JOIN TMATERIAL M ON P.Plant = M.Plant AND P.Material = M.Material
                    //         WHERE P.Plant = @Plant AND V.CustomerNo = @customer ";

                    sql += @"SELECT DISTINCT P.Plant, V.Vendor as Vendor, V.Description AS VndName, P.Customer, P.Customerdescription, P.Material, P.Materialdescription AS MaterialDesc,
                            '' AS Stock, M.BaseUOM, '' AS Remark, M.MaterialType, null AS PIRno,  '' AS 'PirDelFlag', M.PlantStatus INTO #temp2 
                            FROM TCUSTOMER_MATLPRICING P
                            --JOIN TUSERVSCUSTOMER V ON P.Customer = V.CustomerNo and isnull(v.DelFlag,0) = 0
                            LEFT JOIN tVendor_New V ON P.Customer = V.CustomerNo and isnull(V.DelFlag,0) = 0
                            LEFT JOIN tVendorPOrg O ON V.Vendor = O.Vendor AND V.POrg = O.POrg AND P.Plant = O.Plant AND isnull(O.DelFlag,0) = 0
                            JOIN TMATERIAL M ON P.Plant = M.Plant AND P.Material = M.Material ";
                    if (rbActiveMaterial.Checked == true)
                    {
                        sql += @" WHERE (P.Plant = @Plant AND P.Customer = @customer
                                AND ISNULL(M.PlantStatus,'') in ('','Z2') AND M.MaterialType <> 'FERT') 
                                OR 
                                (P.Plant = @Plant AND P.Customer = @customer
                                AND ISNULL(M.PlantStatus,'') in ('','Z1') AND M.MaterialType = 'FERT') ";
                    }
                    else
                    {
                        sql += @" WHERE P.Plant = @Plant AND P.Customer = @customer ";
                    }

                    sql += @"SELECT DISTINCT P.Plant, P.VendorCode, V.Description AS VndName, V.CustomerNo AS CustomerNo, V.Description AS Customerdescription, P.Material, M.MaterialDesc, '' AS Stock, M.BaseUOM, '' AS Remark, M.MaterialType, null as PIRno, '' AS 'PirDelFlag', M.PlantStatus INTO #temp3
                             FROM
                             TMotherCoilvsVendor P
                             JOIN tVendorPOrg O ON P.VendorCode = O.Vendor AND P.Plant = O.Plant AND isnull(O.DelFlag,0) = 0
                             JOIN tVendor_New V ON P.VendorCode = V.Vendor AND V.POrg = O.POrg AND O.Vendor= V.Vendor and isnull(V.DelFlag,0) = 0
                             JOIN TMATERIAL M ON P.Plant = M.Plant AND P.Material = M.Material ";

                    if (rbActiveMaterial.Checked == true)
                    {
                        sql += @" WHERE (P.Plant = @Plant AND P.VendorCode = @vendor AND P.Active = 1
                                AND ISNULL(M.PlantStatus,'') in ('','Z2') AND M.MaterialType <> 'FERT') 
                                OR 
                                (P.Plant = @Plant AND P.VendorCode = @vendor AND P.Active = 1
                                AND ISNULL(M.PlantStatus,'') in ('','Z1') AND M.MaterialType = 'FERT') ";
                    }
                    else
                    {
                        sql += @" WHERE P.Plant = @Plant AND P.VendorCode = @vendor AND P.Active = 1  ";
                    }

                    sql += @"SELECT * FROM #temp1 WHERE #temp1.Material NOT IN (SELECT Material FROM TMotherCoilvsVendor) 
                             UNION ALL 
                             SELECT * FROM #temp2 WHERE #temp2.Material NOT IN (SELECT Material FROM #temp1) 
                             UNION ALL 
                             SELECT * FROM #temp3 WHERE #temp3.Material NOT IN (SELECT Material FROM #temp1) AND #temp3.Material NOT IN (SELECT Material FROM #temp2) 

                             DROP TABLE #temp1
                             DROP TABLE #temp2
                             DROP TABLE #temp3";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    cmd.Parameters.AddWithValue("@Vendor", Session["mappedVendor"].ToString());
                    cmd.Parameters.AddWithValue("@customer", custno);
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

                    //BARU
                    //DataTable dt = new DataTable();
                    //sda.Fill(dt);        

                    //string worksheetName = "sheet1";
                    //string fname = "VendorRealTimeInventory";
                    //DateTime Dat1 = DateTime.Now;
                    //fname = fname + Convert.ToString(Dat1.ToString()) + ".xlsx";
                    //fname = fname.Replace(':', '_');
                    //fname = fname.Replace('/', '_');

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
                    //int rowcount = 2;
                    //excelSheet.Cells[1, 1] = "(Please Don't Delete Highlighted Row)";
                    //for (int i = 1; i < 10; i++)
                    //{
                    //    excelSheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    //}

                    //foreach (DataRow datarow in dt.Rows)
                    //{
                    //    rowcount += 1;
                    //    for (int i = 1; i <= dt.Columns.Count; i++)
                    //    {
                    //        // on the first iteration we add the column headers
                    //        if (rowcount == 3)
                    //        {
                    //            excelSheet.Cells[2, i] = dt.Columns[i - 1].ColumnName;
                    //            excelSheet.Cells[2, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    //        }
                    //        // Filling the excel file 
                    //        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
                    //        //if (i <= 7)
                    //        //{
                    //        //    excelSheet.Cells[rowcount, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                    //        //}
                    //    }
                    //}

                    //string folder = Server.MapPath("/Files/Real Time Vend Inv/Template/");
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
                    //Response.AppendHeader("Content-Disposition", "attachment; filename="+ fname);
                    //Response.TransmitFile(path);
                    //Response.Flush();
                    //Response.End();
                    using (DataTable dt = new DataTable())
                    {
                        string fname = "VendorRealTimeInventory";
                        DateTime Dat1 = DateTime.Now;
                        fname = fname + Convert.ToString(Dat1.ToString());
                        sda.Fill(dt);
                        //using (XLWorkbook wb = new XLWorkbook())
                        //{
                        //    wb.Worksheets.Add(dt, "Sheet1");

                        //    Response.Clear();
                        //    Response.Buffer = true;
                        //    Response.Charset = "";
                        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        //    Response.AddHeader("content-disposition", "attachment;filename="+ fname + ".xlsx");
                        //    using (MemoryStream MyMemoryStream = new MemoryStream())
                        //    {
                        //        wb.SaveAs(MyMemoryStream);
                        //        MyMemoryStream.WriteTo(Response.OutputStream);
                        //        Response.Flush();
                        //        Response.End();
                        //    }
                        //}

                        var workbook = new XLWorkbook();
                        var ws = workbook.Worksheets.Add(dt, "Sheet1");

                        // Insert a row above the range
                        ws.Row(1).InsertRowsAbove(3);
                        ws.Range("A1:A1").Value = "(Please Don't Delete Highlighted Row)";
                        ws.Range("A1:N1").Style.Fill.BackgroundColor = XLColor.LightBlue;

                        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                        for (int i = 0; i < alpha.Length; i++)
                        {
                            if (i <= 13)
                            {
                                if (i == 2 || i == 4 || i == 6 || i == 12 || i == 13)
                                {
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "Optional";
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                                }
                                else if (i == 9)
                                {
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "New – Optional, Change – Mandatory";
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                                }
                                else
                                {
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "Mandatory";
                                    ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                                }
                            }
                        }

                        ws.Range("A3:A3").Value = "int";
                        ws.Range("B3:B3").Value = "nvarchar(8)";
                        ws.Range("C3:C3").Value = "nvarchar(40)";
                        ws.Range("D3:D3").Value = "int";
                        ws.Range("E3:E3").Value = "nvarchar(100)";
                        ws.Range("F3:F3").Value = "nvarchar(40)";
                        ws.Range("G3:G3").Value = "nvarchar(100)";
                        ws.Range("H3:H3").Value = "int";
                        ws.Range("I3:I3").Value = "nvarchar(4)";
                        ws.Range("J3:J3").Value = "nvarchar(200)";
                        ws.Range("K3:K3").Value = "nvarchar(4)";
                        ws.Range("L3:L3").Value = "numeric(10,0)";
                        ws.Range("M3:M3").Value = "nvarchar(1)";
                        ws.Range("N3:N3").Value = "nvarchar(6)";

                        ws.Range("A3:A3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("B3:B3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("C3:C3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("D3:D3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("E3:E3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("F3:F3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("G3:G3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("H3:H3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("I3:I3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("J3:J3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("K3:K3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("L3:L3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("M3:M3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Range("N3:N3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        ws.Columns("N").AdjustToContents();

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
                MDMCon.Dispose();
            }
        }
        protected void GetDataInvalid()
        {
            string path = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                DataTable dt = new DataTable();
                foreach (TableCell cell in GDVImportSummary.HeaderRow.Cells)
                {
                    dt.Columns.Add(cell.Text);
                }

                //Loop through the GridView and copy rows.
                foreach (GridViewRow row in GDVImportSummary.Rows)
                {
                    dt.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dt.Rows[row.RowIndex][i] = row.Cells[i].Text.Replace("&nbsp;", "");
                    }
                }

                string worksheetName = "sheet1";
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
                //int rowcount = 2;
                //excelSheet.Cells[1, 1] = "(Please Don't Delete Highlighted Row)";
                //for (int i = 1; i < 11; i++)
                //{
                //    excelSheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
                //}

                //foreach (DataRow datarow in dt.Rows)
                //{
                //    rowcount += 1;
                //    for (int i = 1; i <= dt.Columns.Count; i++)
                //    {
                //        // on the first iteration we add the column headers
                //        if (rowcount == 3)
                //        {
                //            excelSheet.Cells[2, i] = dt.Columns[i - 1].ColumnName;
                //            excelSheet.Cells[2, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromHtml("#9bc2e6"));
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

                //string folder = Server.MapPath("/Files/Real Time Vend Inv/Template/");
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

                // Insert a row above the range
                ws.Row(1).InsertRowsAbove(3);
                ws.Range("A1:A1").Value = "(Please Don't Delete Highlighted Row)";
                ws.Range("A1:O1").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("J2:O2").Style.Fill.BackgroundColor = XLColor.LightBlue;

                char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                for (int i = 0; i < alpha.Length; i++)
                {
                    if (i <= 13)
                    {
                        if (i == 2 || i == 4 || i == 11 || i == 12)
                        {
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "Optional";
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                        else if (i == 8)
                        {
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "New – Optional, Change – Mandatory";
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                        else
                        {
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Value = "Mandatory";
                            ws.Range(alpha[i] + "2:" + alpha[i] + "2").Style.Fill.BackgroundColor = XLColor.LightBlue;
                        }
                    }
                }

                ws.Range("A3:A3").Value = "int";
                ws.Range("B3:B3").Value = "nvarchar(8)";
                ws.Range("C3:C3").Value = "nvarchar(40)";
                ws.Range("D3:D3").Value = "int";
                ws.Range("E3:E3").Value = "nvarchar(100)";
                ws.Range("F3:F3").Value = "nvarchar(40)";
                ws.Range("G3:G3").Value = "nvarchar(100)";
                ws.Range("H3:H3").Value = "int";
                ws.Range("I3:I3").Value = "nvarchar(4)";
                ws.Range("J3:J3").Value = "nvarchar(200)";
                ws.Range("K3:K3").Value = "nvarchar(4)";
                ws.Range("L3:L3").Value = "numeric(10,0)";
                ws.Range("M3:M3").Value = "nvarchar(1)";
                ws.Range("N3:N3").Value = "nvarchar(6)";

                ws.Range("A3:A3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("B3:B3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("C3:C3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("D3:D3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("E3:E3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("F3:F3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("G3:G3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("H3:H3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("I3:I3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("J3:J3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("K3:K3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("L3:L3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("M3:M3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("N3:N3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Range("O3:O3").Style.Fill.BackgroundColor = XLColor.LightBlue;
                ws.Columns("O").AdjustToContents();

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
                MDMCon.Dispose();
            }
        }

        //protected void GetDataVendUpdateDueDate(string ReqNo)
        //{
        //    SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
        //    try
        //    {
        //        EmetCon.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            sql = @" select VendorCode1,VendorName,QuoteNo,RequestNumber,CONVERT(varchar, QuoteResponseDueDate, 103) 
        //                    from TQuoteDetails WHERE RequestNumber =@RequestNumber ";

        //            cmd = new SqlCommand(sql, EmetCon);
        //            cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                grdvendor.DataSource = dt;
        //                grdvendor.DataBind();
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

        protected bool BtnActionEnDis(int rowsGvMain)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            bool ActionEnDis = false;
            try
            {
                //EmetCon.Open();
                //using (SqlDataAdapter sda = new SqlDataAdapter())
                //{
                //    string ReqNo = "";
                //    if (GridView1.Rows.Count > 0)
                //    {
                //        ReqNo = GridView1.Rows[rowsGvMain - 2].Cells[3].Text;
                //    }
                //    else
                //    {
                //        //ReqNo = GridView1.Rows[rowsGvMain - 2].Cells[3].Text;
                //    }


                //    cmd = new SqlCommand("Get_PIRTran", EmetCon);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@flag", 8);
                //    cmd.Parameters.AddWithValue("@quotenumber", ReqNo);

                //    sda.SelectCommand = cmd;
                //    using (DataTable dt = new DataTable())
                //    {
                //        sda.Fill(dt);
                //        if (dt.Rows.Count > 0)
                //        {
                //            ActionEnDis = true;
                //            //e.Row.Cells[4].Enabled = true;
                //            //e.Row.Cells[5].Enabled = true;
                //        }
                //        else
                //        {
                //            ActionEnDis = false;
                //            //e.Row.Cells[4].Enabled = false;
                //            //e.Row.Cells[5].Enabled = false;
                //        }
                //    }
                //}

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



        bool CekVendorVsMaterial(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd/MM/yyyy') as RequestDate,format(QuoteResponseDueDate,'dd/MM/yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = @Material and VendorCode1 =@VendorCode1 and (ApprovalStatus in ('0','2')) and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') and RequestNumber <> @RequestNumber
                            --from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + @"' and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') and RequestNumber <> @RequestNumber 
                            ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
                    //cmd.Parameters.AddWithValue("@RequestNumber", TxtModalReqNo.Text);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            return true;
                        }
                        else
                        {
                            if (Session["InvalidRequest"] == null)
                            {
                                Session["InvalidRequest"] = dt;
                            }
                            else
                            {
                                DataTable DtTemp = (DataTable)Session["InvalidRequest"];
                                DataRow dr = DtTemp.NewRow();
                                dr["Plant"] = dt.Rows[0]["Plant"].ToString();
                                dr["RequestNumber"] = dt.Rows[0]["RequestNumber"].ToString();
                                dr["RequestDate"] = dt.Rows[0]["RequestDate"].ToString();
                                dr["QuoteResponseDueDate"] = dt.Rows[0]["QuoteResponseDueDate"].ToString();
                                dr["QuoteNo"] = dt.Rows[0]["QuoteNo"].ToString();
                                dr["Material"] = dt.Rows[0]["Material"].ToString();
                                dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
                                dr["VendorCode1"] = dt.Rows[0]["VendorCode1"].ToString();
                                dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                                DtTemp.Rows.Add(dr);
                                DtTemp.AcceptChanges();
                                Session["InvalidRequest"] = DtTemp;
                            }
                            return false;
                        }
                    }
                    //UpdatePanel18.Update();
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

        protected string getDaySetting()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string date = "";
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" SELECT IDVALUE FROM " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)' ";

                    cmd = new SqlCommand(sql, EmetCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            date = dt.Rows[0][0].ToString();
                        }
                    }
                }
                return date;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return "";
            }
            finally
            {
                EmetCon.Dispose();
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
                        set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false')
                        set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                        set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                        While @counter <= @max
                        Begin

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @SchedulePerMonth)
                        BEGIN
	                        SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                        BEGIN
	                        SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
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

                        //OLD
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
                                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                                btnEdit.Enabled = false;
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

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                    EMETModule.SendExcepToDB(ex);
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //try
            //{
            //    GridView1.PageIndex = e.NewPageIndex;
            //    Session["ReqWaitPgNo"] = (GridView1.PageIndex).ToString();
            //    if (Session["DTRTVendor"] != null)
            //    {
            //        DataTable dt = new DataTable();
            //        dt = (DataTable)Session["DTRTVendor"];
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
            //    ShowTable();
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
            try
            {
                //if (e.CommandName == "gEdit")
                //{
                //    GetDbMaster();
                //    SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                //    EmetCon.Open();
                //    int rowIndex = Convert.ToInt32(e.CommandArgument);
                //    GridViewRow row = GridView1.Rows[rowIndex];
                //    string[] ArrCreatedDate = row.Cells[1].Text.Split('/');
                //    string fixCreatedDate = ArrCreatedDate[2] + "-" + ArrCreatedDate[1] + "-" + ArrCreatedDate[0];
                //    DateTime oDate = DateTime.Parse(fixCreatedDate);

                //    sql = @" Select distinct 
                //            A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                //            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, U.UseNam  as CreatedBy, 
                //            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                //            A.CreatedDate as CreatedDateOri,
                //            A.UpdatedBy, 
                //            --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                //            A.UpdatedDate
                //            From TRealTimeVendorInventory A
                //            join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                //            join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                //            join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                //            join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                //            join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID WHERE A.Plant = @Plant AND A.VendorCode = @VendorCode AND A.SAPCode = @SAPCode AND A.CreatedDate = @CreatedDate";

                //    cmd = new SqlCommand(sql, EmetCon);
                //    cmd.Parameters.AddWithValue("@Plant", row.Cells[2].Text.ToString());
                //    cmd.Parameters.AddWithValue("@VendorCode", row.Cells[4].Text.ToString());
                //    cmd.Parameters.AddWithValue("@SAPCode", row.Cells[6].Text.ToString());
                //    cmd.Parameters.AddWithValue("@CreatedDate", oDate);
                //    reader = cmd.ExecuteReader();
                //    if (reader.HasRows)
                //    {
                //        while (reader.Read())
                //        {
                //            TxtPlant.Text = reader["Plant"].ToString();
                //            TxtPlantName.Text = reader["PlantName"].ToString();
                //            TxtVendor.Text = reader["VendorCode"].ToString();
                //            TxtvendorName.Text = reader["VndName"].ToString();
                //            TxtSAPCode.Text = reader["SAPCode"].ToString();
                //            TxtSAPDesc.Text = reader["MaterialDesc"].ToString();
                //            TxtUOM.Text = reader["UOM"].ToString();
                //            txtPirNo.Text = reader["PirNo"].ToString();
                //            TxtStock.Text = reader["Stock"].ToString();
                //            TxtRemark.Text = reader["Remark"].ToString();
                //            TxtCreatedDate.Text = reader["CreatedDate"].ToString();

                //        }
                //    }


                //    //TxtPlant.Text = row.Cells[2].Text;
                //    //TxtPlantName.Text = row.Cells[3].Text;
                //    //TxtVendor.Text = row.Cells[4].Text;
                //    //TxtvendorName.Text = row.Cells[5].Text;
                //    //TxtSAPCode.Text = row.Cells[6].Text;
                //    //TxtSAPDesc.Text = row.Cells[7].Text;
                //    //TxtUOM.Text = row.Cells[8].Text;
                //    //txtPirNo.Text = row.Cells[9].Text;
                //    //TxtStock.Text = row.Cells[10].Text;
                //    //TxtRemark.Text = row.Cells[11].Text.Replace("&nbsp;", "");
                //    //TxtCreatedDate.Text = row.Cells[1].Text;
                //    upModal.Update();

                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                //}
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
                UpForm.Update();
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
                UpForm.Update();
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
                UpForm.Update();
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

        protected void TxtTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpForm.Update();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                ShowTable();
                txtFind.Focus();

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "showDetailDiv();CloseLoading();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showDetailDiv();DatePitcker();", true);
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
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                if (TxtRemark.Text == "" || TxtStock.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Stock and Remark cannot blank');CloseLoading();", true);
                }
                else
                {
                    //DateTime createdDate = DateTime.ParseExact(TxtCreatedDate.Text, "dd/MM/yyyy", null);
                    string[] ArrCreatedDate = TxtCreatedDate.Text.Split('/');
                    string fixCreatedDate = ArrCreatedDate[2] + "-" + ArrCreatedDate[1] + "-" + ArrCreatedDate[0];
                    DateTime oDate = DateTime.Parse(fixCreatedDate);

                    EmetCon.Open();
                    sql = @" UPDATE TRealTimeVendorInventory SET Stock = @Stock, Remark = @Remark, UpdatedBy = @UserID, UpdatedDate = CURRENT_TIMESTAMP WHERE Plant = @Plant AND VendorCode = @VendorCode AND ISNULL(CustomerNo, '') = ISNULL(@CustomerNo, '') AND SAPCode = @SAPCode AND CreatedDate = @CreatedDate ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Stock", TxtStock.Text);
                    cmd.Parameters.AddWithValue("@Remark", TxtRemark.Text);
                    cmd.Parameters.AddWithValue("@Plant", TxtPlant.Text);
                    cmd.Parameters.AddWithValue("@VendorCode", TxtVendor.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", txtCustomerNo.Text);
                    cmd.Parameters.AddWithValue("@SAPCode", TxtSAPCode.Text);
                    cmd.Parameters.AddWithValue("@UserID", Session["userID_"].ToString());
                    cmd.Parameters.AddWithValue("@CreatedDate", oDate);
                    cmd.ExecuteNonQuery();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();DatePitcker();CloseLoading();UpdateRowIntable('" + TxtStock.Text + "','" + TxtRemark.Text + "', '" + Session["userID_"].ToString() + " - " + Session["UserName"].ToString() + "');alert('Data Updated!');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();DatePitcker();CloseLoading();", true);
                    //ShowTable();
                    //UpdatePanel1.Update();

                    //DataTable dt = new DataTable();
                    //dt = (DataTable)Session["DTRTVendor"];
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if (dt.Rows[i]["Plant"].ToString().ToUpper() == TxtPlant.Text.ToString() && dt.Rows[i]["VendorCode"].ToString().ToUpper() == TxtVendor.Text.ToString() && dt.Rows[i]["SAPCode"].ToString().ToUpper() == TxtSAPCode.Text.ToString() && dt.Rows[i]["CreatedDate"].ToString().ToUpper() == TxtCreatedDate.Text.ToString())
                    //    {
                    //        DateTime UpdateDate = DateTime.Now;
                    //        dt.Rows[i]["Stock"] = TxtStock.Text.ToString();
                    //        dt.Rows[i]["Remark"] = TxtRemark.Text.ToString();
                    //        dt.Rows[i]["UpdatedDate"] = UpdateDate;
                    //        dt.Rows[i]["UpdatedBy"] = Session["userID_"].ToString() + " - " + Session["UserName"].ToString(); 
                    //        break;
                    //    }
                    //}
                    //Session["DTRTVendor"] = dt;
                }
                //Session["InvalidRequest"] = null;
                //bool CanUpdate = true;
                //if (grdvendor.Rows.Count > 0)
                //{
                //    for (int i = 0; i < grdvendor.Rows.Count; i++)
                //    {
                //        if (CekVendorVsMaterial(grdvendor.Rows[i].Cells[0].Text, TxtMaterial.Text) == false)
                //        {
                //            CanUpdate = false;
                //        }
                //    }
                //}

                //if (CanUpdate == true)
                //{
                //    string CurentDate = DateTime.Now.ToString("dd/MM/yyyy");
                //    DateTime DtCurentDate = DateTime.ParseExact(CurentDate, "dd/MM/yyyy", null);
                //    DateTime DtDueDate = DateTime.ParseExact(TxtModalDueDate.Text, "dd/MM/yyyy", null);

                //    int result = DateTime.Compare(DtDueDate, DtCurentDate);
                //    if ((result > 0) && (TxtModalDueDate.Text != ""))
                //    {
                //        sql = "update TQuoteDetails set QuoteResponseDueDate = @QuoteResponseDueDate where RequestNumber = @RequestNumber ";
                //        cmd = new SqlCommand(sql, EmetCon);
                //        cmd.Parameters.AddWithValue("@RequestNumber", TxtModalReqNo.Text);
                //        cmd.Parameters.AddWithValue("@QuoteResponseDueDate", DtDueDate.ToString("yyyy-MM-dd"));
                //        cmd.ExecuteNonQuery();
                //        ShowTable();
                //        UpdatePanel1.Update();
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "closeModal();", true);
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Updated !');CloseLoading();", true);
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Date should be greater than current date');CloseLoading();", true);
                //    }

                //    if (Session["ReqWaitNst"] != null)
                //    {
                //        string RowVsStatus = Session["ReqWaitNst"].ToString();
                //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "TriggerNested('" + RowVsStatus + "')", true);
                //    }
                //}
                //else
                //{
                //    if (Session["InvalidRequest"] != null)
                //    {
                //        DataTable DtTemp = (DataTable)Session["InvalidRequest"];
                //        if (DtTemp.Rows.Count > 0)
                //        {
                //            GvInvalidRequest.DataSource = DtTemp;
                //            GvInvalidRequest.DataBind();
                //            DvInvalidRequest.Visible = true;
                //        }
                //        else
                //        {
                //            DvInvalidRequest.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        DvInvalidRequest.Visible = false;
                //    }
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "CloseLoading();", true);
                //}

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();CloseLoading();", true);
                EmetCon.Dispose();
            }
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

        protected void BtnDownLOadtemplate_Click(object sender, EventArgs e)
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

        protected void btnImportFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["userID_"].ToString() != "")
                {
                    GetDbMaster();
                    string VndCode = Session["mappedVendor"].ToString();
                    string VndUserID = Session["userID_"].ToString();
                    string VndCustomerNo = "";

                    if (VndUserID.Contains(VndCode))
                    {
                        VndCustomerNo = getCustNO();
                    }

                    if ((FileUpload.HasFile))
                    {
                        string currentMonth = DateTime.Now.Month.ToString();
                        string currentYear = DateTime.Now.Year.ToString();

                        string excelCol = "[Plant],[Vendor],[VndName],[Material],[MaterialDesc],[MaterialType],[BaseUOM],[PIRno],[Stock],[Remark],[PirDelFlag],[PlantStatus],[CustomerNo],[Customerdescription]"; //COL TO TAKE IN EXCEL
                        string excelRange = "A4:N5000"; //DEFAULT RANGE IS A4 TO BU5000, CHANGE IF NECCESARY
                                                        //OLD
                                                        //string query = string.Format(
                                                        //    @"UPDATE TRealTimeVendorInventory SET Stock = B.Stock, Remark = B.Remark, PirDelFlag = B.PirDelFlag, PlantStatus = B.PlantStatus, UpdatedBy = N'{0}', UpdatedDate = CURRENT_TIMESTAMP FROM TRealTimeVendorInventory A INNER JOIN ##temp B ON (A.Plant = B.Plant AND A.VendorCode = B.Vendor AND A.SAPCode = B.Material AND ((A.CreatedDate >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) OR (CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) OR CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) )) INSERT INTO TRealTimeVendorInventory ([Plant], [VendorCode],[SAPCode],[PirNo],[UOM],[Stock],[Remark],[PIRDelFlag],[PlantStatus],CreatedBy,CreatedDate) SELECT [Plant],[Vendor],[Material],[PIRno],[BaseUOM],[Stock],[Remark],[PirDelFlag],[PlantStatus], N'{0}', CURRENT_TIMESTAMP FROM ##temp WHERE NOT EXISTS (SELECT A.Plant, A.VendorCode, A.SAPCode FROM TRealTimeVendorInventory A WHERE A.Plant = ##temp.Plant AND A.VendorCode = ##temp.Vendor AND A.SAPCode = ##temp.Material AND CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) AND CURRENT_TIMESTAMP < '2020-11-19') AND (CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) OR CURRENT_TIMESTAMP between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) "
                                                        //    , Session["userID_"].ToString());

                        //THE TRUE ONE
                        //string query = string.Format(
                        //    @"UPDATE TRealTimeVendorInventory SET Stock = B.Stock, Remark = B.Remark, UpdatedBy = N'{0}', UpdatedDate = CURRENT_TIMESTAMP FROM TRealTimeVendorInventory A INNER JOIN ##temp B ON (A.Plant = B.Plant AND A.VendorCode = B.Vendor AND A.SAPCode = B.Material AND (A.CreatedDate >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) AND (CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) AND CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) ) INSERT INTO TRealTimeVendorInventory ([Plant], [VendorCode],[SAPCode],[PirNo],[UOM],[Stock],[Remark],CreatedBy,CreatedDate) SELECT [Plant],[Vendor],[Material],[PIRno],[BaseUOM],[Stock],[Remark], N'{0}', CURRENT_TIMESTAMP FROM ##temp WHERE NOT EXISTS (SELECT A.Plant, A.VendorCode, A.SAPCode FROM TRealTimeVendorInventory A WHERE A.Plant = ##temp.Plant AND A.VendorCode = ##temp.Vendor AND A.SAPCode = ##temp.Material AND CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) AND CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) AND CURRENT_TIMESTAMP between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) "
                        //    , Session["userID_"].ToString());

                        //                    string query = string.Format(
                        //                        @"
                        //                            Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date
                        //                            set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false')
                        //                            set @nextSchedule = (DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()))

                        //                            While @counter <= @max
                        //                            Begin

                        //                            IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
                        //                            BEGIN
                        //                             SET @nextSchedule = (DATEADD(day, -1, @nextSchedule))
                        //                             SET @totalHoliday = @totalHoliday + 1
                        //                             SET @isHoliday = 1
                        //                            END
                        //                            ELSE
                        //                            BEGIN
                        //                             SET @isHoliday = 0
                        //                             BREAK;
                        //                            END

                        //                            SET @counter = @counter + 1

                        //                            End

                        //                            UPDATE TRealTimeVendorInventory SET Stock = B.Stock, Remark = B.Remark, PirDelFlag = B.PirDelFlag, PlantStatus = B.PlantStatus, UpdatedBy = N'{0}', UpdatedDate = CURRENT_TIMESTAMP FROM TRealTimeVendorInventory A INNER JOIN ##temp B ON (A.Plant = B.Plant AND A.VendorCode = B.Vendor AND A.SAPCode = B.Material AND ((A.CreatedDate >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) OR (CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104)) OR CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) AND CONVERT(date, CURRENT_TIMESTAMP, 104) < @nextSchedule or CreatedDate between @nextSchedule and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()))) 


                        //INSERT INTO TRealTimeVendorInventory ([Plant], [VendorCode],[SAPCode],[PirNo],[UOM],[Stock],[Remark],[PIRDelFlag],[PlantStatus],CreatedBy,CreatedDate) SELECT [Plant],[Vendor],[Material],[PIRno],[BaseUOM],[Stock],[Remark],[PirDelFlag],[PlantStatus], N'{0}', CURRENT_TIMESTAMP FROM ##temp WHERE NOT EXISTS (SELECT A.Plant, A.VendorCode, A.SAPCode FROM TRealTimeVendorInventory A WHERE A.Plant = ##temp.Plant AND A.VendorCode = ##temp.Vendor AND A.SAPCode = ##temp.Material AND CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) AND CONVERT(date, CURRENT_TIMESTAMP, 104) < @nextSchedule or CreatedDate between @nextSchedule and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) AND (CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) OR CURRENT_TIMESTAMP between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) "
                        //                        , Session["userID_"].ToString());

                        string query = string.Format(
                            @"Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date, @beforeSchedule date, @ScheduleDay NVARCHAR(10), @daystoadd int, @SchedulePerMonth date

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
                        set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false')
                        set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                        set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                        While @counter <= @max
                        Begin

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @SchedulePerMonth)
                        BEGIN
	                        SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                        BEGIN
	                        SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
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

UPDATE TRealTimeVendorInventory SET Stock = B.Stock, Remark = B.Remark, PirDelFlag = B.PirDelFlag, PlantStatus = B.PlantStatus, CustomerNo = B.CustomerNo, UpdatedBy = N'{0}', UpdatedDate = CURRENT_TIMESTAMP FROM TRealTimeVendorInventory A INNER JOIN ##temp B ON (A.Plant = B.Plant AND A.VendorCode = CASE
    WHEN ISNULL(B.Vendor, '') = '' THEN ''
    ELSE B.Vendor
END AND ISNULL(A.CustomerNo, '') = ISNULL(B.CustomerNo, '') AND A.SAPCode = B.Material AND (A.CreatedDate Between @beforeSchedule AND @nextSchedule AND A.CreatedDate >= CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END))

INSERT INTO TRealTimeVendorInventory ([Plant], [VendorCode],[SAPCode],[PirNo],[UOM],[Stock],[Remark],[PIRDelFlag],[PlantStatus], [CustomerNo],CreatedBy,CreatedDate) SELECT [Plant],
CASE
    WHEN ISNULL([Vendor], '') = '' THEN ''
    ELSE [Vendor]
END,[Material],[PIRno],[BaseUOM],[Stock],[Remark],[PirDelFlag],[PlantStatus], [CustomerNo], N'{0}', CURRENT_TIMESTAMP FROM ##temp WHERE NOT EXISTS (SELECT A.Plant, A.VendorCode, A.CustomerNo, A.SAPCode FROM TRealTimeVendorInventory A WHERE A.Plant = ##temp.Plant AND A.VendorCode = CASE
    WHEN ISNULL(##temp.Vendor, '') = '' THEN ''
    ELSE ##temp.Vendor
END AND ISNULL(A.CustomerNo, '') = ISNULL(##temp.CustomerNo, '') AND A.SAPCode = ##temp.Material AND (A.CreatedDate Between @beforeSchedule AND @nextSchedule AND A.CreatedDate >= CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END))
 "
                            , Session["userID_"].ToString());


                        //WORK
                        //                    string query = string.Format(
                        //                        @"Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date, @beforeSchedule date
                        //                        set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false')
                        //                        set @nextSchedule = (DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()))
                        //                        set @beforeSchedule = (
                        //select convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104))

                        //                        While @counter <= @max
                        //                        Begin

                        //                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                        //                        BEGIN
                        //	                        SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                        //                        END

                        //                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
                        //                        BEGIN
                        //	                        SET @nextSchedule = (DATEADD(day, -1, @nextSchedule))
                        //	                        SET @totalHoliday = @totalHoliday + 1
                        //	                        SET @isHoliday = 1
                        //                        END
                        //                        ELSE
                        //                        BEGIN
                        //	                        SET @isHoliday = 0
                        //	                        --BREAK;
                        //                        END

                        //                        SET @counter = @counter + 1

                        //                        End

                        //                        UPDATE TRealTimeVendorInventory SET Stock = B.Stock, Remark = B.Remark, PirDelFlag = B.PirDelFlag, PlantStatus = B.PlantStatus, UpdatedBy = N'{0}', UpdatedDate = CURRENT_TIMESTAMP FROM TRealTimeVendorInventory A INNER JOIN ##temp B ON (A.Plant = B.Plant AND A.VendorCode = B.Vendor AND A.SAPCode = B.Material AND CreatedDate Between case WHEN @totalHoliday > 0 AND CONVERT(date, CURRENT_TIMESTAMP, 104) = @nextSchedule then @nextSchedule else CASE WHEN @beforeSchedule = convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) THEN
                        //	(convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104))
                        //	ELSE
                        //	@beforeSchedule
                        //	END end and CONVERT(date,  DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()), 104)
                        //OR (
                        //A.CreatedDate >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) AND CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104))) WHERE A.CreatedDate Between case WHEN @totalHoliday > 0 AND CONVERT(date, CURRENT_TIMESTAMP, 104) = @nextSchedule then @nextSchedule else CASE WHEN @beforeSchedule = convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) THEN
                        //	(convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104))
                        //	ELSE
                        //	@beforeSchedule
                        //	END end and CONVERT(date,  DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()), 104)  

                        //                        INSERT INTO TRealTimeVendorInventory ([Plant], [VendorCode],[SAPCode],[PirNo],[UOM],[Stock],[Remark],[PIRDelFlag],[PlantStatus],CreatedBy,CreatedDate) SELECT [Plant],[Vendor],[Material],[PIRno],[BaseUOM],[Stock],[Remark],[PirDelFlag],[PlantStatus], N'{0}', CURRENT_TIMESTAMP FROM ##temp WHERE NOT EXISTS (SELECT A.Plant, A.VendorCode, A.SAPCode FROM TRealTimeVendorInventory A WHERE A.Plant = ##temp.Plant AND A.VendorCode = ##temp.Vendor AND A.SAPCode = ##temp.Material AND ( CONVERT(date, CURRENT_TIMESTAMP, 104) >= CONVERT(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104) OR CONVERT(date, CURRENT_TIMESTAMP, 104) BETWEEN CONVERT(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) AND DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()) )
                        //                        AND A.CreatedDate Between case WHEN @totalHoliday > 0 AND CONVERT(date, CURRENT_TIMESTAMP, 104) = @nextSchedule then @nextSchedule else CASE WHEN @beforeSchedule = convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) THEN
                        //	(convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104))
                        //	ELSE
                        //	@beforeSchedule
                        //	END end and CONVERT(date,  DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()), 104) AND A.CreatedDate Between case WHEN @totalHoliday > 0 AND CONVERT(date, CURRENT_TIMESTAMP, 104) = @nextSchedule then @nextSchedule else CASE WHEN @beforeSchedule = convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) THEN
                        //	(convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104))
                        //	ELSE
                        //	@beforeSchedule
                        //	END end and CONVERT(date,  DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE()), 104) ) "
                        //                        , Session["userID_"].ToString());

                        ArrayList conditions = new ArrayList();
                        ArrayList condRemark = new ArrayList();
                        ArrayList specialCond = new ArrayList();
                        //Update Ramark to '0' if value null
                        //specialCond.Add("UPDATE a SET a.Remark = '0' from ##temp a where ISNULL(a.Remark, '') = '' AND EXISTS(SELECT * FROM TRealTimeVendorInventory b WHERE a.Plant = b.Plant AND a.Vendor = b.VendorCode AND a.Material = b.SAPCode AND a.PIRno = b.PirNo AND (CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())))");

                        specialCond.Add("UPDATE a SET a.Stock = '0' from ##temp a where ISNULL(a.Stock, '') = '' ");

                        specialCond.Add("UPDATE a SET a.PlantStatus = b.PlantStatus from ##temp a inner join " + DbMasterName + @".dbo.tMaterial b on a.Plant = b.Plant and a.Material = b.Material");
                        specialCond.Add(@"UPDATE a SET a.PIRDelFlag = case 
                    when RTRIM(LTRIM(concat(isnull(b.ffd,''),isnull(b.POrgDeleteFlag,''))))= '' then ''
                    else 'X' end from ##temp a inner join " + DbMasterName + @".dbo.tPIRvsQuotation b on a.Plant = b.Plant and a.Vendor = b.Vendor and a.Material = b.Material ");

                        conditions.Add(" ISNULL(Plant, '') = '' ");
                        condRemark.Add("Plant Cannot be Empty");

                        if (VndCustomerNo == "")
                        {
                            conditions.Add(" ISNULL(Vendor, '') = '' ");
                            condRemark.Add("Vendor Cannot be Empty");
                            conditions.Add(" LEN([Vendor]) > 8 ");
                            condRemark.Add("Vendor maximal 8 characters");
                        }
                        else
                        {
                            conditions.Add(" ISNULL(CustomerNo, '') = '' ");
                            condRemark.Add("CustomerNo Cannot be Empty");
                            conditions.Add(" LEN([CustomerNo]) > 10 OR ISNUMERIC(ISNULL([CustomerNo],'') + '0e0') = 0");
                            condRemark.Add("Fill Customer No in int");
                        }
                        conditions.Add(" ISNULL(Material, '') = '' ");
                        condRemark.Add("Material Cannot be Empty");
                        conditions.Add(" LEN([Material]) > 40 ");
                        condRemark.Add("Material maximal 40 characters");
                        conditions.Add(" ISNULL(BaseUOM, '') = '' ");
                        condRemark.Add("BaseUOM Cannot be Empty");
                        conditions.Add(" LEN([BaseUOM]) > 4 ");
                        condRemark.Add("Base UOM maximal 4 characters");
                        conditions.Add(" ISNULL(Stock, '') = '' ");
                        condRemark.Add("Stock Cannot be Empty");
                        conditions.Add(" ISNUMERIC(ISNULL([Stock],'') + '.0e0') = 0 or ISNULL([Stock],'') like '%-%' ");
                        condRemark.Add("Fill Stock in int");
                        conditions.Add(" cast([Stock] as float) > 2147483647 ");
                        condRemark.Add("Stock value exceed int limit");
                        conditions.Add(" NOT EXISTS(SELECT Plant FROM " + DbMasterName + @".dbo.TPLANT WHERE ##temp.Plant = '" + Session["VPlant"].ToString() + "') and ISNULL(##temp.Plant, '') != '' ");
                        condRemark.Add("Related Plant not maintained in master data Plant");
                        conditions.Add(" NOT EXISTS(SELECT b.Material FROM " + DbMasterName + @".dbo.TMATERIAL b WHERE ##temp.Material = b.Material and ##temp.Plant = b.Plant) and ISNULL(##temp.Material, '') != '' ");
                        condRemark.Add("Related Material not maintained in master data Material");
                        //OLD CONDITION FOR REMARK
                        //conditions.Add(" ISNULL(Remark, '') = '' AND EXISTS(SELECT * FROM TRealTimeVendorInventory b WHERE ##temp.Plant = b.Plant AND ##temp.Vendor = b.VendorCode AND ##temp.Material = b.SAPCode AND ##temp.PIRno = b.PirNo AND (CreatedDate >= convert(date, (select CAST(DATEFROMPARTS( YEAR(getdate()), MONTH(getdate()), (select IDValue from " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Vnd_Sub(DateInMonth)') ) AS DATETIME)), 104))) ");
                        //NEW CONDITION FOR REMARK
                        //conditions.Add(@" ISNULL(Remark, '') = '' AND EXISTS(SELECT * FROM TRealTimeVendorInventory b WHERE ##temp.Plant = b.Plant AND ##temp.Vendor = b.VendorCode AND ##temp.Material = b.SAPCode AND ##temp.PIRno = b.PirNo AND (CreatedDate between convert(date, (SELECT case when datepart(weekday, GETDATE()) >5 then DATEADD(DAY, +4, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) else DATEADD(DAY, -3, DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0)) end), 104) and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) AND CONVERT(date, CURRENT_TIMESTAMP, 104) < @nextSchedule or CreatedDate between @nextSchedule and DATEADD(DAY, 13 - (@@DATEFIRST + (DATEPART(WEEKDAY,GETDATE()) %7)), GETDATE())) ");
                        //condRemark.Add("Remark cannot be null");

                        if (VndCustomerNo == "")
                        {
                            conditions.Add(" ISNULL(PIRno, '') = '' AND (Material NOT IN(SELECT Material FROM " + DbMasterName + @".dbo.TMotherCoilvsVendor) AND Material NOT IN(SELECT Material FROM " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING)) ");
                            condRemark.Add("PIR No Cannot be Empty");
                            conditions.Add(" LEN([PIRno]) > 10 OR ISNUMERIC(ISNULL([PIRno],'') + '0e0') = 0");
                            condRemark.Add("Fill Pir No in numeric (10,0)");
                            conditions.Add(" NOT EXISTS(SELECT Vendor FROM " + DbMasterName + @".dbo.tVendor_New WHERE Vendor = '" + Session["mappedVendor"].ToString() + "') and ISNULL(##temp.Vendor, '') != '' ");
                            condRemark.Add("Related Vendor not maintained in master data Vendor");
                            conditions.Add(" Vendor != '" + Session["mappedVendor"].ToString() + "' ");
                            condRemark.Add("Invalid data Vendor");
                            conditions.Add(" NOT EXISTS(select Material,InfoRecord from " + DbMasterName + @".dbo.tPIRvsQuotation PIRQOU where PIRQOU.Vendor = '" + Session["mappedVendor"].ToString() + "' AND PIRQOU.Material = ##temp.Material) and ISNULL(##temp.Material, '') != '' AND NOT EXISTS(SELECT Material FROM " + DbMasterName + @".dbo.TMotherCoilvsVendor) ");
                            condRemark.Add("Invalid data Material");
                            conditions.Add(" NOT EXISTS(select Material,InfoRecord from " + DbMasterName + @".dbo.tPIRvsQuotation PIRQOU where PIRQOU.Vendor = '" + Session["mappedVendor"].ToString() + "' AND PIRQOU.InfoRecord = ##temp.PIRno AND PIRQOU.Material = ##temp.Material) and ISNULL(##temp.PIRno, '') != '' ");
                            condRemark.Add("Invalid data PIR No");
                            conditions.Add(" MaterialType = 'MCOL' AND ##temp.Material IN (SELECT Material FROM " + DbMasterName + @".dbo.TMotherCoilvsVendor WHERE Active = 0 AND Material = ##temp.Material AND VendorCode = ##temp.Vendor) ");
                            condRemark.Add("Material not active in master data Mothercoil vs Vendor");
                        }
                        else
                        {
                            conditions.Add(" CustomerNo != '" + VndCustomerNo + "' ");
                            condRemark.Add("Invalid data CustomerNo");
                            conditions.Add(" NOT EXISTS(SELECT CustomerNo FROM " + DbMasterName + @".dbo.TUSERVSCUSTOMER WHERE CustomerNo = '" + VndCustomerNo + "') and ISNULL(##temp.CustomerNo, '') != '' ");
                            condRemark.Add("Related CustomerNo not maintained in master data UserVsCustomer");
                            conditions.Add(" NOT EXISTS(select Material from " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING P JOIN " + DbMasterName + @".dbo.TUSERVSCUSTOMER V ON P.Customer = V.CustomerNo and isnull(v.DelFlag,0) = 0 WHERE P.Customer = '" + VndCustomerNo + "' AND P.Material = ##temp.Material) ");
                            condRemark.Add("Invalid data Material");
                            conditions.Add(" NOT EXISTS(select Material from " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING P JOIN " + DbMasterName + @".dbo.TUSERVSCUSTOMER V ON P.Customer = V.CustomerNo and isnull(v.DelFlag,0) = 0 WHERE P.Customer = '" + VndCustomerNo + "' AND P.Material = ##temp.Material) ");
                            condRemark.Add("Invalid data Material");
                            conditions.Add(" NOT EXISTS(select Material from " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING P JOIN " + DbMasterName + @".dbo.TUSERVSCUSTOMER V ON P.Customer = V.CustomerNo and isnull(v.DelFlag,0) = 0 WHERE ISNULL(##temp.CustomerNo, '') != '' AND P.Customer = '" + VndCustomerNo + "') ");
                            condRemark.Add("Invalid data Customer No");
                        }
                        conditions.Add(@" ISNULL(Remark, '') = '' AND EXISTS(SELECT * FROM TRealTimeVendorInventory b WHERE b.Plant = ##temp.Plant AND b.VendorCode = CASE
    WHEN ISNULL(##temp.Vendor, '') = '' THEN ''
    ELSE ##temp.Vendor
END AND ISNULL(b.CustomerNo, '') = ISNULL(##temp.CustomerNo, '') AND b.SAPCode = ##temp.Material AND (B.CreatedDate Between @beforeSchedule AND @nextSchedule AND B.CreatedDate >= CASE WHEN GETDATE() >= @SchedulePerMonth THEN @SchedulePerMonth ELSE @beforeSchedule END) ) ");
                        condRemark.Add("Remark Cannot be Empty");
                        conditions.Add(" LEN([Remark]) > 200 ");
                        condRemark.Add("Remark maximal 200 characters");

                        if (!Convert.IsDBNull(FileUpload.PostedFile) && FileUpload.PostedFile.ContentLength > 0 && ((FileUpload.FileName.ToUpper().EndsWith("XLS")) || (FileUpload.FileName.ToUpper().EndsWith("XLSX"))))
                        {
                            string uniqueField = " [Plant], [Vendor], [Material] ";
                            string folder = Server.MapPath("/Files/Real Time Vend Inv/Upload/");
                            if (!Directory.Exists(folder)) //CHECK IF FOLDER EXIST
                            {
                                Directory.CreateDirectory(folder);  // CREATE FOLDER IF NOT EXIST
                            }
                            string path = folder + FileUpload.FileName;
                            FileUpload.SaveAs(path); //SAVE FILE TO SERVER

                            string result = ImportFile(path, Session["userID_"].ToString(), "TRealTimeVendorInventory", query, excelCol, "RealTimeVendInv", conditions, condRemark, specialCond, excelRange, uniqueField);

                            if (result == "OK")
                            {
                                System.IO.File.Delete(path);

                                #region Invalid data process to table
                                List<Dictionary<string, object>> dataListInValid = new List<Dictionary<string, object>>();
                                Dictionary<string, object> row;
                                if (Session[Session["userID_"].ToString() + "InValidData"] != null)
                                {
                                    DataTable dt = (DataTable)System.Web.HttpContext.Current.Session[Session["userID_"].ToString() + "InValidData"];
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            row = new Dictionary<string, object>();
                                            foreach (DataColumn col in dt.Columns)
                                            {
                                                row.Add(col.ColumnName, dr[col]);
                                            }
                                            dataListInValid.Add(row);
                                        }
                                    }
                                }
                                #endregion
                                string validCount = System.Web.HttpContext.Current.Session[Session["userID_"].ToString() + "ValidDataCount"].ToString();
                                string invalidCount = dataListInValid.Count().ToString();
                                if (validCount == "0" && invalidCount == "0")
                                {
                                    //return Json(new { success = false, message = "No Data Found", JsonRequestBehavior.AllowGet });
                                }
                                else if (invalidCount != "0")
                                {
                                    DataTable dt = (DataTable)System.Web.HttpContext.Current.Session[Session["userID_"].ToString() + "InValidData"];
                                    GDVImportSummary.DataSource = dt;
                                    GDVImportSummary.DataBind();
                                    lblValidCount.Text = "Total Success Import : [" + validCount + "]; ";
                                    lblInvalidCount.Text = "Total Invalid Import : [" + invalidCount + "];";
                                    UpdatePanel2.Update();
                                    ShowMainTable();
                                    UpForm.Update();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalImportSummary();", true);
                                    //return Json(new { success = false, dataInvalid = dataListInValid, dataValidCount = validCount, dataInvalidCount = invalidCount, message = "Refer to Import Summary", JsonRequestBehavior.AllowGet });
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Import Success !');CloseLoading();clearFileUpload();", true);
                                    ShowMainTable();
                                    string test = TxtDataMainJson.Text.ToString();
                                    //UpForm.Update();
                                }
                                FileUpload.Attributes.Clear();
                            }
                            else
                            {
                                System.IO.File.Delete(path);
                                //return Json(new { success = false, message = result, JsonRequestBehavior.AllowGet });
                                //return Redirect(Request.UrlReferrer.ToString());
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('" + result + " !');CloseLoading();clearFileUpload();", true);
                                ShowTable();
                                UpForm.Update();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Import Failed !');CloseLoading();clearFileUpload();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('File not Found !');CloseLoading();clearFileUpload();", true);
                    }
                }
                else
                {
                    Response.Redirect(Request.RawUrl);
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


        public string ImportFile(string path, string user, string tableName, string query, string excelCol, string title, ArrayList conditions = null, ArrayList condRemark = null, ArrayList specialCond = null, string excelRange = "A4:BU5000", string uniqueField = "", string addCond = "")
        {
            System.Web.HttpContext.Current.Session[user + "InValidData"] = null;
            //SqlConnection conPSS = new SqlConnection(System.Configuration.ConfigurationManager.
            //ConnectionStrings["EMET"].ConnectionString);

            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());

            //string NameOnly = Path.GetFileName(path);
            string sql = "";
            SqlTransaction trans = null;
            SqlCommand cmd = null;

            FileInfo fi = new FileInfo(path);
            string ext = fi.Extension;
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);

            try
            {
                if ((EmetCon == null) || (EmetCon.State != ConnectionState.Open))
                {
                    EmetCon.Open();
                }
                trans = EmetCon.BeginTransaction();

                #region Process copy data from excel file to temp table
                sql = @" IF OBJECT_ID('tempdb..##temp') IS NOT NULL DROP TABLE ##temp";
                cmd = new SqlCommand(sql, EmetCon, trans);
                cmd.ExecuteNonQuery();

                String excelConnString = "";
                if (ext.ToUpper() == ".XLSX")
                {
                    excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", path);
                }
                else
                {
                    excelConnString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES;\"", path);
                }

                string ColumnNameNFieldType = "";
                if (ext.ToUpper() != ".TXT")
                {
                    string SheetName = "";
                    using (OleDbConnection excel_con = new OleDbConnection(excelConnString))
                    {
                        excel_con.Open();
                        DataTable dt = new DataTable();
                        dt = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt.Rows.Count > 0)
                        {
                            SheetName = dt.Rows[0]["TABLE_NAME"].ToString();

                            if (dt.Rows[0]["TABLE_TYPE"].ToString() == "TABLE")
                            {
                                if ((SheetName.Substring(SheetName.Length - 1) == "$") || (SheetName.Substring(SheetName.Length - 1) == "'"))
                                {
                                    if (SheetName.Contains("'"))
                                    {
                                        SheetName = SheetName.Substring(1, SheetName.Length - 2);

                                    }
                                }
                            }
                        }
                        excel_con.Close();
                    }

                    //string sqlqq = @"SELECT * FROM [" + fileNameWithoutExt + "$]";
                    string sqlqq = @"SELECT * FROM [" + SheetName + excelRange + "]";

                    using (OleDbDataAdapter adaptor = new OleDbDataAdapter(sqlqq, excelConnString))
                    {
                        //cmd = new SqlCommand(sqlqq, conPSS);
                        DataTable dt = new DataTable();
                        adaptor.Fill(dt);

                        foreach (DataColumn column in dt.Columns)
                        {
                            string ColumnName = column.ColumnName.Replace("#", ".").Trim();
                            ColumnNameNFieldType += "[" + ColumnName + "]" + " nvarchar (MAX),";
                        }

                        if (ColumnNameNFieldType != "" && !ColumnNameNFieldType.Contains("[Issue Remark]"))
                        {
                            ColumnNameNFieldType += "[Issue Remark] nvarchar (MAX)";
                        }
                        else if (ColumnNameNFieldType.Contains("[Issue Remark]"))
                        {
                            ColumnNameNFieldType = ColumnNameNFieldType.Remove(ColumnNameNFieldType.Length - 1, 1);
                        }

                        string sqltemptable = @" create table  ##temp ( " + ColumnNameNFieldType + " )  ";
                        cmd = new SqlCommand(sqltemptable, EmetCon, trans);
                        cmd.ExecuteNonQuery();

                        using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EmetCon, SqlBulkCopyOptions.Default, trans))
                        {
                            //Give your Destination table name 
                            sqlBulk.DestinationTableName = "##temp";
                            sqlBulk.BulkCopyTimeout = 0;
                            sqlBulk.BatchSize = 10000;
                            sqlBulk.WriteToServer(dt);
                        }
                    }
                }
                else
                {
                    int i = 0;
                    var sr = new StreamReader(path);
                    string line = sr.ReadLine();

                    string[] strArray = line.Split('\t');
                    var dta = new DataTable();

                    for (int index = 0; index < strArray.Length; index++)
                        dta.Columns.Add(new DataColumn());
                    do
                    {
                        DataRow row = dta.NewRow();

                        string[] itemArray = line.Split('\t');
                        row.ItemArray = itemArray;
                        dta.Rows.Add(row);
                        i = i + 1;
                        line = sr.ReadLine();
                    } while (!string.IsNullOrEmpty(line));
                    sr.Close();

                    for (int k = 0; k < dta.Rows[0].ItemArray.Count(); k++)
                    {
                        string ColumnName = dta.Rows[0].ItemArray[k].ToString().Trim();
                        ColumnNameNFieldType += "[" + ColumnName + "]" + " nvarchar (MAX),";
                    }

                    if (ColumnNameNFieldType != "" && !ColumnNameNFieldType.Contains("[Issue Remark]"))
                    {
                        ColumnNameNFieldType += "[Issue Remark] nvarchar (MAX)";
                    }
                    else if (ColumnNameNFieldType.Contains("[Issue Remark]"))
                    {
                        ColumnNameNFieldType = ColumnNameNFieldType.Remove(ColumnNameNFieldType.Length - 1, 1);
                    }

                    string sqltemptable = @" create table  ##temp ( " + ColumnNameNFieldType + " )  ";
                    cmd = new SqlCommand(sqltemptable, EmetCon, trans);
                    cmd.ExecuteNonQuery();

                    var bc = new SqlBulkCopy(EmetCon, SqlBulkCopyOptions.TableLock, trans)
                    {
                        DestinationTableName = "##temp",
                        BatchSize = dta.Rows.Count - 1
                    };
                    dta.Rows[0].Delete();

                    bc.WriteToServer(dta);
                    bc.Close();

                }
                #endregion

                #region Cek header file structure
                string ArrColName = ColumnNameNFieldType.Replace("nvarchar (MAX)", "");
                string[] exlColName = excelCol.Split(',');
                bool IsFieldOK = true;

                sql = @" UPDATE ##temp set [Issue Remark] = '' 
                        DELETE FROM ##temp where ";
                for (int i = 0; i < exlColName.Count(); i++)
                {
                    sql += " ISNULL(" + exlColName[i].ToString() + ", '') = '' AND ";
                    if (!ArrColName.Contains(exlColName[i].ToString().Trim()))
                    {
                        IsFieldOK = false;
                    }
                }
                sql = sql.Substring(0, sql.Length - 4);
                #endregion

                if (IsFieldOK == true)
                {
                    GetDbMaster();
                    string sql2 = "";
                    #region update special condition
                    if (specialCond != null && specialCond.Count > 0)
                    {
                        for (int i = 0; i < specialCond.Count; i++)
                        {
                            sql2 = specialCond[i].ToString();

                            cmd = new SqlCommand(sql2, EmetCon, trans);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    #endregion

                    #region cek invalid data
                    sql += @" 
                        IF OBJECT_ID('tempdb..#invaliddata') IS NOT NULL
                        drop table #invaliddata

                        IF OBJECT_ID('tempdb..##temp') IS NOT NULL 

                        SELECT TOP 0 * INTO #invaliddata from ##temp ";

                    sql += @"Declare @Id int, @counter INT = 1, @max INT = 0, @isHoliday bit = 1, @totalHoliday int = 0, @nextSchedule date, @beforeSchedule date, @ScheduleDay NVARCHAR(10), @daystoadd int, @SchedulePerMonth date

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
                        set @max = (Select Count(*) From " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false')
                        set @nextSchedule = (DATEADD(DAY, (DATEDIFF(DAY, @daystoadd, GETDATE()) / 7) * 7 + 7, @daystoadd))
                        set @beforeSchedule = (convert(date, DATEADD(week, datediff(d, @daystoadd, GETDATE()) / 7, @daystoadd)))

                        While @counter <= @max
                        Begin

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @SchedulePerMonth)
                        BEGIN
	                        SET @SchedulePerMonth = (DATEADD(day, -1, @SchedulePerMonth))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @beforeSchedule)
                        BEGIN
	                        SET @beforeSchedule = (DATEADD(day, -1, @beforeSchedule))
                        END

                        IF EXISTS(select [date] from " + DbMasterName + @".dbo.tHoliday where Plant = '" + Session["VPlant"].ToString() + @"' and DelFlag = 'false' and [date] = @nextSchedule)
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

                        End";

                    if (conditions != null && conditions.Count > 0)
                    {
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            sql += "  insert into #invaliddata(" + excelCol + @",[Issue Remark])
                        select " + excelCol + @", '" + condRemark[i].ToString() + @"' from ##temp where " + conditions[i].ToString() + @"
                        delete from ##temp where " + conditions[i].ToString();
                        }
                    }
                    if (uniqueField != "")
                    {
                        sql += @" BEGIN
                                    WITH cte AS (
                                        SELECT *, 
                                        ROW_NUMBER() OVER (
                                            PARTITION BY " + uniqueField + @"
                                            ORDER BY " + uniqueField + @"
                                        ) row_num
                                        FROM ##temp
                                    )
                                    insert into #invaliddata (" + excelCol + @", [Issue Remark])
                                    select " + excelCol + @", 'Duplicate Data' FROM cte
                                    WHERE row_num > 1;

                                    WITH cte AS (
                                    SELECT *, 
                                        ROW_NUMBER() OVER (
                                            PARTITION BY " + uniqueField + @"
                                            ORDER BY " + uniqueField + @"
                                            ) row_num
                                        FROM ##temp
                                    )
                                    DELETE FROM cte
                                    WHERE row_num > 1;
                                   END ";
                    }
                    sql += addCond;
                    sql += @" select * from #invaliddata ";
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd = new SqlCommand(sql, EmetCon, trans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                System.Web.HttpContext.Current.Session[user + "InValidData"] = dt;
                            }
                        }
                    }
                    #endregion

                    #region cek data in temp table
                    System.Web.HttpContext.Current.Session[user + "ValidDataCount"] = null;
                    sql2 = @" select * from ##temp ";
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd = new SqlCommand(sql2, EmetCon, trans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            int c = dt.Rows.Count;
                            System.Web.HttpContext.Current.Session[user + "ValidDataCount"] = c;
                        }
                    }
                    #endregion

                    query += " drop table ##temp "; //Drop the table after execute Query
                    cmd = new SqlCommand(query, EmetCon, trans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    trans.Commit();
                    EmetCon.Dispose();
                    trans.Dispose();
                    return "OK";
                }
                else
                {
                    trans.Commit();
                    EmetCon.Dispose();
                    trans.Dispose();
                    return "Invalid Data structure, please follow template format";
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("'" + fileNameWithoutExt + "$' is not a valid name"))
                {
                    trans.Rollback();
                    return "Invalid Sheet name";
                }
                else if (ex.ToString().Contains("External table is not in the expected format"))
                {
                    EmetCon.Close();
                    return "File Not in real XLSX or XLS format";
                }
                else
                {
                    trans.Rollback();
                    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                    EMETModule.SendExcepToDB(ex);
                    return ex.ToString();
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Session["VPlant"] != null || Session["mappedVendor"] != null)
            {
                string VndPlant = Session["VPlant"].ToString();
                string VndCode = Session["mappedVendor"].ToString();
                if (EMETModule.isRltInv(VndPlant, VndCode) == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalImport();", true);
                }
                else
                {
                    string VndUserID = Session["userID_"].ToString();
                    string VndCustomerNo = getCustNO();
                    //NEW
                    if (isCustomerRltInv(VndPlant, VndCustomerNo, VndUserID) == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalImport();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('No Need To Do Real Time Inventory This Time');", true);
                    }

                    //OLD
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "alert('No Need To Do Real Time Inventory This Time');", true);
                }
            }
        }

        protected void BtnDownloadInvalidData_Click(object sender, EventArgs e)
        {
            try
            {
                GetDataInvalid();
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
                ExportTable();
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

        protected void ExportTable()
        {
            string path = "";
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @"SELECT A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, 
                            CASE WHEN A.CreatedBy is null THEN null ELSE CONCAT(A.CreatedBy,' - ', U.UseNam) END as CreatedBy,
                            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                            A.CreatedDate as CreatedDateOri,
                            CASE WHEN A.UpdatedBy is null THEN null ELSE CONCAT(A.UpdatedBy,' - ', C.UseNam) END as UpdatedBy,
                            --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                            A.UpdatedDate
                            From TRealTimeVendorInventory A
                            join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                            join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                            join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                            left join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID 
                            left join " + DbMasterName + @".dbo.Usr C on A.UpdatedBy = C.UseID 
                            WHERE A.Plant = @Plant AND A.VendorCode = @VendorCode";

                    //sql = @" Select distinct ROW_NUMBER() OVER (
                    //       PARTITION BY A.CreatedDate
                    //     ORDER BY A.CreatedDate desc
                    //       ) row_num,
                    //        A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                    //        A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, U.UseNam  as CreatedBy, 
                    //        FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                    //        A.CreatedDate as CreatedDateOri,
                    //        A.UpdatedBy, 
                    //        --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                    //        A.UpdatedDate
                    //        From TRealTimeVendorInventory A
                    //        join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                    //        join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
                    //        join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                    //        join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                    //        join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID WHERE A.Plant = @Plant AND A.VendorCode = @VendorCode";

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
                    cmd.Parameters.AddWithValue("@Plant", Session["VPlant"].ToString());
                    cmd.Parameters.AddWithValue("@VendorCode", Session["mappedVendor"].ToString());
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
            //    UpdatePanel1.Update();
            //    if (Session["DTRTVendor"] != null)
            //    {
            //        DataTable dt = new DataTable();
            //        dt = (DataTable)Session["DTRTVendor"];
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
            //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading()", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
            //    EMETModule.SendExcepToDB(ex);
            //}
        }

        protected void btnEditAction_Click(object sender, EventArgs e)
        {
            try
            {
                GetDbMaster();
                SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                EmetCon.Open();
                string[] ArrCreatedDate = hiddenCreatedDate.Text.ToString().Split('/');
                string fixCreatedDate = ArrCreatedDate[2] + "-" + ArrCreatedDate[1] + "-" + ArrCreatedDate[0];
                DateTime oDate = DateTime.Parse(fixCreatedDate);

                sql = @" Select distinct 
                            A.Plant, T.Description as PlantName, A.VendorCode,v.Description as VndName, A.SAPCode, M.MaterialDesc,A.UOM, A.PirNo, 
                            A.Stock, A.Remark, A.PIRDelFlag, A.PlantStatus, A.CustomerNo, Z.Customerdescription, U.UseNam  as CreatedBy, 
                            FORMAT(A.CreatedDate,'dd/MM/yyyy') as 'CreatedDate', 
                            A.CreatedDate as CreatedDateOri,
                            A.UpdatedBy, 
                            --FORMAT(A.UpdatedDate,'dd/MM/yyyy') as 'UpdatedDate'
                            A.UpdatedDate
                            From TRealTimeVendorInventory A
                            left join " + DbMasterName + @".dbo.tVendorPOrg O on A.VendorCode = O.Vendor and A.Plant = O.Plant
                            left join " + DbMasterName + @".dbo.tVendor_New V on A.VendorCode = V.Vendor and V.POrg = O.POrg and O.Vendor= V.Vendor
							left join " + DbMasterName + @".dbo.TCUSTOMER_MATLPRICING Z on A.CustomerNo = Z.Customer AND A.SAPCode = Z.Material
                            join " + DbMasterName + @".dbo.TMATERIAL M on A.Plant = M.Plant and A.SAPCode = M.Material
                            join " + DbMasterName + @".dbo.TPLANT T on A.Plant = T.Plant
                            join " + DbMasterName + @".dbo.Usr U on A.CreatedBy = U.UseID WHERE A.Plant = @Plant AND (A.VendorCode = @VendorCode OR A.CustomerNo = @CustomerNo) AND A.SAPCode = @SAPCode AND A.CreatedDate = @CreatedDate";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Plant", hiddenPlant.Text.ToString());
                cmd.Parameters.AddWithValue("@VendorCode", hiddenVendorCode.Text.ToString());
                cmd.Parameters.AddWithValue("@CustomerNo", hiddenCustomerCode.Text.ToString());
                cmd.Parameters.AddWithValue("@SAPCode", hiddenSAPCode.Text.ToString());
                cmd.Parameters.AddWithValue("@CreatedDate", oDate);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TxtPlant.Text = reader["Plant"].ToString();
                        TxtPlantName.Text = reader["PlantName"].ToString();
                        TxtVendor.Text = reader["VendorCode"].ToString();
                        TxtvendorName.Text = reader["VndName"].ToString();
                        TxtSAPCode.Text = reader["SAPCode"].ToString();
                        TxtSAPDesc.Text = reader["MaterialDesc"].ToString();
                        TxtUOM.Text = reader["UOM"].ToString();
                        txtPirNo.Text = reader["PirNo"].ToString();
                        txtCustomerNo.Text = reader["CustomerNo"].ToString();
                        txtCustomerDesc.Text = reader["Customerdescription"].ToString();
                        TxtStock.Text = reader["Stock"].ToString();
                        TxtRemark.Text = reader["Remark"].ToString();
                        TxtCreatedDate.Text = reader["CreatedDate"].ToString();

                    }
                }


                //TxtPlant.Text = row.Cells[2].Text;
                //TxtPlantName.Text = row.Cells[3].Text;
                //TxtVendor.Text = row.Cells[4].Text;
                //TxtvendorName.Text = row.Cells[5].Text;
                //TxtSAPCode.Text = row.Cells[6].Text;
                //TxtSAPDesc.Text = row.Cells[7].Text;
                //TxtUOM.Text = row.Cells[8].Text;
                //txtPirNo.Text = row.Cells[9].Text;
                //TxtStock.Text = row.Cells[10].Text;
                //TxtRemark.Text = row.Cells[11].Text.Replace("&nbsp;", "");
                //TxtCreatedDate.Text = row.Cells[1].Text;
                upModal.Update();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }
    }
}