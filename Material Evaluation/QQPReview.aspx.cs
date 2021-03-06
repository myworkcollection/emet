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
using System.Text;
using System.Threading;
using System.Globalization;

namespace Material_Evaluation
{
    public partial class QQPReview : System.Web.UI.Page
    {
        public static string password;
        public static string domain;
        public static string path;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        string DbTransName = "";

        string PlantDesc = "";
        string SMNPICSubmDept = "";
        string GA = "";
        string DbMasterName = "";
        string QuoteStatus = "";
        bool IsAth;
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

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

        protected void EmptyPublicDataTable()
        {
            Session["DtMaterialsDetails"] = null;
            //DtMaterialsDetails = null;
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

        string GetGA()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"select GA from TQuoteDetails where QuoteNo=@QuoteNo";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@QuoteNo", Session["Qno"].ToString());
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GA = reader["GA"].ToString();
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
            return GA;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                Session["VndToolAmortize"] = null;
                Session["VndMachineAmortize"] = null;

                if (Session["userID"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {

                    if (!IsPostBack)
                    {
                        string UI = Session["userID"].ToString();
                        string FN = "EMET_QuotecostView";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                            {
                                LoadSavings();
                                EmptyPublicDataTable();
                                string UserId = Session["userID"].ToString();
                                string userId = Session["userID"].ToString();
                                string sname = Session["UserName"].ToString();
                                string srole = Session["userType"].ToString();
                                string concat = sname + " - " + srole;

                                Session["Qno"] = Request.QueryString["Number"];
                                Session["MtlAddCount"] = 1;
                                Session["PCAddCount"] = 1;
                                Session["SMCAddCount"] = 1;
                                Session["OthersAddCount"] = 1;
                                string QuoteNo = Request.QueryString["Number"];
                                lblreqst.Text = "Quote No : " + QuoteNo.ToString();
                                GetQuoteStatus(QuoteNo);
                                GetReqPlant(QuoteNo.Remove(0, 3).Replace("GP", ""));
                                GetQuoteandAllDetails(QuoteNo);
                                GetQuoteDetailsbyQuotenumber(QuoteNo);
                                CreateDynamicTablebasedonProcessField();

                                GetQuoteupdated(QuoteNo);

                                string struser = (string)HttpContext.Current.Session["userID"].ToString();

                                GetSHMNPICDetails(struser);
                                GetData(QuoteNo);

                                if (txtPIRtype.Text.Contains("SUBCON"))
                                {
                                    //btnAddColumns.Enabled = false;
                                }

                                if (IsrevisionAndNotSubmit(QuoteNo) == true)
                                {
                                    string OldQuNo = hdnQuoteNoRef.Value.ToString();
                                    process(OldQuNo);
                                    if (QuoteStatus != "")
                                    {
                                        RetrieveAllCostDetails(OldQuNo);
                                    }
                                }
                                else
                                {
                                    process(QuoteNo);
                                    if (QuoteStatus != "")
                                    {
                                        RetrieveAllCostDetails(QuoteNo);
                                    }
                                    else
                                    {
                                        if (hdnQuoteNoRefmassRev.Value != "")
                                        {
                                            RetrieveAllCostDetails(hdnQuoteNoRefmassRev.Value);
                                        }
                                    }
                                }

                                UnitCostDataStore();

                                OthersCostDataStore();
                                subMatCostDataStore();
                                ProcessCostDataStore();
                                MCCostDataStore();
                                Countryoforigin();

                                SetTitleForm(QuoteNo);
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
        

        bool IsrevisionAndNotSubmit(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct QuoteNoRef,UpdatedBy from TQuoteDetails where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0 && dt.Rows[0]["QuoteNoRef"].ToString() != "")
                        {

                            if (dt.Rows[0]["UpdatedBy"].ToString() == "")
                            {
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

        protected void GetQuoteStatus(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select TQ.QuoteNo,TQ.ApprovalStatus,TQ.PICApprovalStatus,TQ.PICReason,
                            TQ.ManagerApprovalStatus,TQ.ManagerReason,TQ.DIRApprovalStatus,TQ.DIRReason ,

                            case 
                            when TQ.UpdatedBy = 'EMET' then 'EMET'
                            when (TQ.AprRejBy is not null) then (select distinct US.UseNam from " + DbMasterName + @".[dbo].Usr US where US.UseID = TQ.AprRejBy)
                            when (TQ.AprRejBy is null and TQ.AprRejByMng is not null) then (select distinct US.UseNam from " + DbMasterName + @".[dbo].Usr US where US.UseID = TQ.AprRejByMng)
                            when (TQ.AprRejBy is null and TQ.AprRejByMng is null) then '-'
                            end as  [AprRejBy],

                            case 
                            when TQ.UpdatedBy = 'EMET' then format(TQ.UpdatedOn , 'dd/MM/yyyy')
                            when (TQ.AprRejDate is not null) then format(TQ.AprRejDate , 'dd/MM/yyyy')
                            when (TQ.AprRejDate is null and TQ.AprRejDateMng is not null) then format(TQ.AprRejDateMng , 'dd/MM/yyyy')
                            when (TQ.AprRejDate is null and TQ.AprRejDateMng is null) then NULL
                            end as  [AprRejDate],

                            case 
                            when TQ.UpdatedBy = 'EMET' then 'Auto Completed By Shimano'
                            when (TQ.ApprovalStatus = 1 and TQ.FinalQuotePrice = '' or TQ.ApprovalStatus = 1 and TQ.FinalQuotePrice is null) then 'Auto Completed By Shimano'
                            when (TQ.ApprovalStatus = '0' and TQ.PICApprovalStatus = '0' ) then 'Waiting for Vendor Submission'
                            when (TQ.ApprovalStatus = '2' and TQ.PICApprovalStatus = '0' ) then 'Vendor Submitted'
                            when (TQ.ApprovalStatus = '2' and TQ.PICApprovalStatus = '2' ) then 'Approved by Manager'
                            when (TQ.ApprovalStatus = '2' and TQ.PICApprovalStatus = '1' ) then 'Rejected by Manager'
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0' and DIRReason is null and DIRRemark is null) then 'Rejected by Manager'
                            when (TQ.ManagerApprovalStatus = '2' and DIRApprovalStatus= '0') then 'Approved by DIR'
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0') then 'Rejected by DIR'
                            when (TQ.ApprovalStatus = '4' and TQ.PICApprovalStatus = '4' and TQ.ManagerApprovalStatus = '4' and DIRApprovalStatus= '4') then 'Waiting for Vendor Submission'
                            when (TQ.ApprovalStatus = '5' and TQ.PICApprovalStatus = '5' and TQ.ManagerApprovalStatus = '5' and DIRApprovalStatus= '5') then 'Completed By Vendor'
                            else 'Waiting for Vendor Submission'
                            end as 'Status',

                            case 
                            when (TQ.ApprovalStatus = '0' and TQ.PICApprovalStatus = '0' ) then 'Waiting for Vendor Submission'
                            when (TQ.ApprovalStatus = '2' and TQ.PICApprovalStatus = '0' ) then 'Waiting for Manager Approval'
                            when (TQ.PICApprovalStatus = '2' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null ) then TQ.ManagerRemark
                            when (TQ.PICApprovalStatus = '1' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null and TQ.ManagerReason is null) then TQ.ManagerRemark
                            when (TQ.PICApprovalStatus = '1' and TQ.ManagerApprovalStatus = '0' and DIRApprovalStatus is null and TQ.ManagerReason is not null) then TQ.ManagerReason
                            when (TQ.ManagerApprovalStatus = '2' and DIRApprovalStatus= '0') then TQ.DIRRemark
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0'and TQ.DIRReason is null) then TQ.DIRRemark
                            when (TQ.ManagerApprovalStatus = '1' and DIRApprovalStatus= '0'and TQ.DIRReason is not null) then TQ.DIRReason
                            when (TQ.ApprovalStatus = '4' and TQ.PICApprovalStatus = '4' and TQ.ManagerApprovalStatus = '4' and DIRApprovalStatus= '4') then '-'
                            when (TQ.ApprovalStatus = '5' and TQ.PICApprovalStatus = '5' and TQ.ManagerApprovalStatus = '5' and DIRApprovalStatus= '5') then 'Without SAP Code No Need Approval'
                            else '-'
                            end as 'Comment'
                            from TQuoteDetails TQ
                            where TQ.QuoteNo = @QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            LbUpByStatsQuot.Text = ": " + dt.Rows[0]["AprRejBy"].ToString();
                            LbUpDateStatsQuot.Text = ": " + dt.Rows[0]["AprRejDate"].ToString();
                            LbStatsQuot.Text = ": " + dt.Rows[0]["Status"].ToString();
                            LbCommentStatsQuot.Text = ": " + dt.Rows[0]["Comment"].ToString().Replace("Apr: ", "").Replace("Rej: ", "").Replace("Reason: ", "");
                        }
                        else
                        {
                            LbUpByStatsQuot.Text = ": ";
                            LbUpDateStatsQuot.Text = ": --/--/----";
                            LbStatsQuot.Text = ": Waiting for Vendor Submission";
                            LbCommentStatsQuot.Text = ": ";
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
                EmetCon.Dispose();
            }
        }

        protected void SetTitleForm(string QN)
        {
            try
            {
                if (QN.Substring(QN.Length - 1, 1) == "D")
                {
                    lbTitle.Text = "Quote Request Without SAP Code : Review ";
                }
                else if (QN.Substring(QN.Length - 2, 2) == "GP")
                {
                    lbTitle.Text = "Quote Request Without SAP Code (GP) : Review ";
                }
                else
                {
                    lbTitle.Text = "Quote Request With SAP Code : Review ";
                }

                if (hdnQuoteNoRef.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + " (Revision)";
                }
                else if (hdnMassRevision.Value != "")
                {
                    lbTitle.Text = lbTitle.Text + " (Mass Revision)";
                }
                else
                {
                    lbTitle.Text = lbTitle.Text;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
            }
        }

        protected void GetReqPlant(string ReqNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select CONCAT(A.Plant, ' - ', (select distinct C.Description from " + DbMasterName + @".dbo.TPLANT C  where c.Plant = A.Plant)) as 'PlRequestor'
                            from TPlantReq A 
                            inner join " + DbMasterName + @".dbo.TPLANT B on A.Plant = B.Plant 
                            where A.RequestNumber=@RequestNumber ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@RequestNumber", ReqNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string Plreq = "";
                            for (int p = 0; p < dt.Rows.Count; p++)
                            {
                                if (dt.Rows.Count == 1 || p == (dt.Rows.Count - 1))
                                {
                                    Plreq += dt.Rows[p]["PlRequestor"].ToString();
                                }
                                else
                                {
                                    Plreq += dt.Rows[p]["PlRequestor"].ToString() + ",";
                                }
                            }
                            TxtReqPlant.Text = Plreq;
                        }
                        else
                        {
                            TxtReqPlant.Text = "";
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
                EmetCon.Dispose();
            }
        }

        protected void ShowTablePrevQuote(string Material, string VendorCode, string RequestNumber, string requestdate, string ProcessGrp)
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            EmetCon.Open();
            try
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select TOP(3) FORMAT(requestdate, 'dd/MM/yyyy') as 'requestdate',RequestNumber,QuoteNo,Material,
                            CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            Profit,Discount,
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
                            format(EffectiveDate, 'dd/MM/yyyy') as 'EffectiveDate', 
                            format(DueOn, 'dd/MM/yyyy') as 'DueOn' ,
                            (select distinct US.UseNam from " + DbMasterName + @".[dbo].Usr US where US.UseID = TQ.AprRejBy) as 'ApprovebyDIR',
                            format(AprRejDate, 'dd/MM/yyyy') as 'DIRApprovalDate'
                            from TQuoteDetails TQ
                            where RequestNumber not in(select RequestNumber from TQuoteDetails where ManagerApprovalStatus =0 ) and ManagerApprovalStatus=2 and  DIRApprovalStatus=0 and ApprovalStatus = 3
                            and Material = @Material and VendorCode1 = @VendorCode1 and RequestNumber <> @RequestNumber and format(requestdate, 'yyyy-MM-dd') <= @requestdate
                            and ProcessGroup= @ProcessGrp
                            order by format(requestdate,'yyyy-MM-dd') Desc  ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", Material);
                    cmd.Parameters.AddWithValue("@VendorCode1", VendorCode);
                    cmd.Parameters.AddWithValue("@RequestNumber", RequestNumber);
                    cmd.Parameters.AddWithValue("@requestdate", requestdate);
                    cmd.Parameters.AddWithValue("@ProcessGrp", ProcessGrp);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GdvPrevQuote.DataSource = dt;
                        GdvPrevQuote.DataBind();
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

        protected void showDataQuoreReqRevice(string newQN, string OldQN)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select QuoteNo,TQ.CountryOrg,
                            (select CONCAT(TQ.VendorCode1 ,' - ',(select distinct TV.Description from " + DbMasterName + @".[dbo].tVendor_New TV where TV.Vendor = TQ.VendorCode1)) ) as 'vendor',
                            TQ.Product,TQ.MaterialClass,TQ.Material,TQ.MaterialDesc,
                            (select CONCAT(TQ.ProcessGroup ,' - ',(select distinct TP.Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code = TQ.ProcessGroup)) ) as 'ProcessGroup',
                            CAST(ROUND(TQ.TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost,
                            CAST(ROUND(TQ.TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost,
                            CAST(ROUND(TQ.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost,
                            CAST(ROUND(TQ.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost,
                            CAST(ROUND(TQ.FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice,
                            (select TQ2.AcsTabMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabMatCost',
                            (select TQ2.AcsTabProcCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabProcCost',
                            (select TQ2.AcsTabSubMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabSubMatCost',
                            (select TQ2.AcsTabOthMatCost from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'AcsTabOthMatCost',
                            (select ISNULL(tq2.IsUseToolAmortize,0) from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'IsUseToolAmortize',
                            (select isnull(tq2.IsUseMachineAmortize,0) from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'IsUseMachineAmortize',
                            (select case when isnull(tq2.ToolAmorRemark,'') <> '' then TQ2.ToolAmorRemark  when isnull(tq2.IsUseToolAmortize,0) = 1 then 'ADD' else 'REMOVE' end  from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'ActionToolAmortize',
                            (select case when isnull(tq2.MachineAmorRemark,'') <> '' then TQ2.MachineAmorRemark  when isnull(tq2.IsUseMachineAmortize,0) = 1 then 'ADD' else 'REMOVE' end  from TQuoteDetails TQ2 where TQ2.QuoteNo = @newQN) as 'ActionMachineAmortize'
                            from TQuoteDetails TQ 
                            where TQ.QuoteNo = @OldQN ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@newQN", newQN);
                    cmd.Parameters.AddWithValue("@OldQN", OldQN);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GdvQuoreReqRevice.DataSource = dt;
                        GdvQuoreReqRevice.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            CheckBox ChkMatRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkMatRef") as CheckBox;
                            CheckBox chkProcRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkProcRef") as CheckBox;
                            CheckBox chkSubMatRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkSubMatRef") as CheckBox;
                            CheckBox chkOthRowRef = GdvQuoreReqRevice.Rows[0].FindControl("chkOthRef") as CheckBox;
                            CheckBox chkIsUseToolAmortize = GdvQuoreReqRevice.Rows[0].FindControl("chkIsUseToolAmortize") as CheckBox;
                            CheckBox chkIsUseMachineAmortize = GdvQuoreReqRevice.Rows[0].FindControl("chkIsUseMachineAmortize") as CheckBox;
                            if (ChkMatRowRef != null && chkProcRowRef != null && chkSubMatRowRef != null && chkOthRowRef != null && chkIsUseToolAmortize != null && chkIsUseMachineAmortize != null)
                            {
                                if (Convert.ToBoolean(dt.Rows[0]["AcsTabMatCost"]) == true)
                                {
                                    ChkMatRowRef.Checked = true;
                                }
                                else
                                {
                                    ChkMatRowRef.Checked = false;
                                }

                                if (Convert.ToBoolean(dt.Rows[0]["AcsTabProcCost"]) == true)
                                {
                                    chkProcRowRef.Checked = true;
                                }
                                else
                                {
                                    chkProcRowRef.Checked = false;
                                }

                                if (Convert.ToBoolean(dt.Rows[0]["AcsTabSubMatCost"]) == true)
                                {
                                    chkSubMatRowRef.Checked = true;
                                }
                                else
                                {
                                    chkSubMatRowRef.Checked = false;
                                }

                                if (Convert.ToBoolean(dt.Rows[0]["AcsTabOthMatCost"]) == true)
                                {
                                    chkOthRowRef.Checked = true;
                                }
                                else
                                {
                                    chkOthRowRef.Checked = false;
                                }

                                if (dt.Rows[0]["IsUseToolAmortize"].ToString() == "True")
                                {
                                    chkIsUseToolAmortize.Checked = true;
                                }
                                else
                                {
                                    chkIsUseToolAmortize.Checked = false;
                                }

                                if (dt.Rows[0]["IsUseMachineAmortize"].ToString() == "True")
                                {
                                    chkIsUseMachineAmortize.Checked = true;
                                }
                                else
                                {
                                    chkIsUseMachineAmortize.Checked = false;
                                }
                            }

                            CountryoforiginRev(dt.Rows[0]["CountryOrg"].ToString());
                        }
                    }
                    //UpdatePanel18.Update();
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

        string GetPlantPICSMNSubmit(string PlantId)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct CONCAT(Plant, ' - ', Description) as PlantDesc from TPLANT where Plant=@Plant";

                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@Plant", PlantId);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlantDesc = reader["PlantDesc"].ToString();
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
                MDMCon.Dispose();
            }
            return PlantDesc;
        }

        string GetDeptPICSMNSubmit(string UseId)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct UseDep as UseDep from usr where UseId=@UseId";

                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UseId", UseId);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SMNPICSubmDept = reader["UseDep"].ToString();
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
                MDMCon.Dispose();
            }
            return SMNPICSubmDept;
        }


        protected void Countryoforigin()
        {
            //string strproc1 = (string)HttpContext.Current.Session["process"].ToString();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select countrycode,CountryDescription,Currency from TCountrycode where active=1";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {

                }
                else
                {
                    //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
                }
                string QuoteNo = Session["Qno"].ToString();
                str = "select TQ.CountryOrg, TC.CountryDescription from " + TransDB.ToString() + "TQuoteDetails TQ INNER JOIN TCountrycode TC ON TC.CountryCode=TQ.CountryOrg WHERE QuoteNo = '" + QuoteNo + "'";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);
                if (Result.Rows.Count > 0)
                {
                    //corg = Result.Rows[0]["CountryOrg"].ToString();
                    txtCorigin.Text = Result.Rows[0]["CountryDescription"].ToString();
                }
                else
                {
                    //  txtjobtypedesc.Text = Result.Rows[0]["JobCode"].ToString();
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

        protected void CountryoforiginRev(string CountryCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct CountryDescription from TCountrycode where CountryCode = @CountryCode ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtCorigin.Text = dt.Rows[0]["CountryDescription"].ToString();
                        }
                        else
                        {
                            txtCorigin.Text = "";
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
                MDMCon.Dispose();
            }
        }

        protected void GetOldQuotePIRMass(string QuNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select PIRNo,MassRevQutoteRef,
                            CAST(ROUND(TQ.OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost,
                            CAST(ROUND(TQ.OldTotSubMatCost,5) AS DECIMAL(12,5)) as OldTotSubMatCost,
                            CAST(ROUND(TQ.OldTotProCost,5) AS DECIMAL(12,5)) as OldTotProCost,
                            CAST(ROUND(TQ.OldTotOthCost,5) AS DECIMAL(12,5)) as OldTotOthCost,
                            FORMAT(MassUpdateDate,'dd/MM/yyyy hh:mm:ss') as MassUpdateDate 
                            from TQuoteDetails TQ 
                            where QuoteNo=@QuoteNo ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            hdnQuoteNoRefmassRev.Value = dt.Rows[0]["MassRevQutoteRef"].ToString();
                            GvQuoteDataPIR.DataSource = dt;
                            GvQuoteDataPIR.DataBind();
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


        protected void GvQuoteDataPIR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LbQuoteNo = e.Row.FindControl("LbQuoteMassNoRef") as Label;
                    if (LbQuoteNo != null)
                    {
                        string url = "QQPReview.aspx?Number=" + LbQuoteNo.Text;
                        LbQuoteNo.Attributes.Add("onclick", "openInNewTab2('" + url + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void LoadSavings()
        {

            try
            {
                using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenEMETConnString()))
                {
                    Email_inser.Open();
                    SqlCommand sqlCmd = new SqlCommand("Email_UNC", Email_inser);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader dr;
                    dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Session["USERIdMail"] = dr.GetString(0);
                        //Userid = dr.GetString(0);
                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
                        URL = dr.GetString(4);
                        MasterDB = dr.GetString(5);
                        TransDB = dr.GetString(6);
                    }
                    dr.Dispose();
                    Email_inser.Dispose();
                }
            }

            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;

            }

        }

        //private void OthersCostDataStore()
        //{
        //    var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();

        //    if (DtDynamicOtherCostsFields.Rows.Count>0)
        //    {
        //        var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;

        //        if (ccc > 1)
        //        {
        //            CreateDynamicOthersCostDT(ccc);
        //        }
        //    }
        //    CreateDynamicOthersCostDT(0);

        //}


        private void OthersCostDataStore()
        {
            try
            {
                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                var tempOtherlistcount = hdnOtherValues.Value.ToString().Split(',').ToList();
                var ccc = tempOtherlistcount.Count / DtDynamicOtherCostsFields.Rows.Count;
                CreateDynamicOthersCostDT(0);
                if (ccc > 1)
                {
                    CreateDynamicOthersCostDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        //private void subMatCostDataStore()
        //{
        //    var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //    if (DtDynamicOtherCostsFields.Rows.Count > 0)
        //    {
        //        var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;
        //        if (ccc > 1)
        //        {

        //            CreateDynamicSubMaterialDT(ccc);

        //        }
        //    }
        //    CreateDynamicSubMaterialDT(0);



        //}

        private void subMatCostDataStore()
        {
            try
            {
                DataTable DtDynamicSubMaterialsFields = new DataTable();
                DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                var tempSublistcount = hdnSMCTableValues.Value.ToString().Split(',').ToList();
                var ccc = tempSublistcount.Count / DtDynamicSubMaterialsFields.Rows.Count;

                CreateDynamicSubMaterialDT(0);

                if (ccc > 1)
                {
                    CreateDynamicSubMaterialDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        //private void ProcessCostDataStore()
        //{
        //    var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();
        //    if (DtDynamicOtherCostsFields.Rows.Count > 0)
        //    {
        //        var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;
        //        if (ccc > 1)
        //        {
        //            CreateDynamicProcessDT(ccc);
        //        }
        //    }
        //    CreateDynamicProcessDT(0);


        //}

        private void ProcessCostDataStore()
        {
            try
            {

                var tempProcesslistcount = hdnProcessValues.Value.ToString().Split(',').ToList();
                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];
                var ccc = tempProcesslistcount.Count / DtDynamicProcessFields.Rows.Count;
                CreateDynamicProcessDT(0);
                if (ccc > 1)
                {
                    CreateDynamicProcessDT(ccc);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        //private void MCCostDataStore()
        //{
        //    var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
        //    if (DtDynamicOtherCostsFields.Rows.Count > 0)
        //    {
        //        var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;
        //        if (ccc > 1)
        //        {

        //            CreateDynamicDT(ccc);

        //        }
        //    }
        //    CreateDynamicDT(0);




        //}

        private void MCCostDataStore()
        {
            try
            {
                DataTable DtDynamic = new DataTable();
                DtDynamic = (DataTable)Session["DtDynamic"];

                DataTable DtMaterialsDetails = new DataTable();
                if (Session["DtMaterialsDetails"] != null)
                {
                    DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];
                }

                var tempMClistcount = hdnMCTableValues.Value.ToString().Split(',').ToList();
                var ccc = tempMClistcount.Count / DtDynamic.Rows.Count;

                CreateDynamicDT(0);
                if (hdnQuoteNoRefmassRev.Value == "")
                {
                    if (ccc > 1)
                    {
                        CreateDynamicDT(ccc);
                    }
                }
                else
                {
                    if (QuoteStatus != "")
                    {
                        if (ccc > 1)
                        {
                            CreateDynamicDT(ccc);
                        }
                    }
                    else {
                        if (ccc > 1)
                        {
                            CreateDynamicDT(ccc);
                        }
                        //if (ccc > 1 && ccc <= DtMaterialsDetails.Rows.Count)
                        //{
                        //    CreateDynamicDT(ccc);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        //private void UnitCostDataStore()
        //{
        //    var tempUnitlistcount = hdnUnitValues.Value.ToString().Split(',').ToList();

        //    if (DtDynamicUnitFields.Rows.Count > 0)
        //    {
        //        var ccc = tempUnitlistcount.Count / DtDynamicUnitFields.Rows.Count;
        //        //CreateDynamicUnitDT(ccc);
        //    }
        //    CreateDynamicUnitDT(0);

        //}

        private void UnitCostDataStore()
        {
            try
            {
                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];
                var tempUnitlistcount = hdnUnitValues.Value.ToString().Split(',').ToList();
                var ccc = tempUnitlistcount.Count / DtDynamicUnitFields.Rows.Count;

                if (hdnVendorType.Value == "TeamShimano")
                {
                    CreateDynamicUnitDTTmShimano(0);
                }
                else
                {
                    CreateDynamicUnitDT(0);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }

        protected void MachineIdsbyProcess(string strProcessCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                string VndId = Session["VndId"].ToString();
                string Vndplant = Session["Vndplant"].ToString();
                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM where VendorCode = '" + VndId + "'  and Plant = '" + Vndplant + "' and ProcessGrp= '" + strProcessCode + "'";
                da2 = new SqlDataAdapter(str2, MDMCon);
                Result2 = new DataTable();
                da2.Fill(Result2);

                //ddlMachine.DataSource = Result2;
                //ddlMachine.DataBind();
                Session["MachineIDsbyProcess"] = Result2;
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

        // Used this method by Raja
        private void GetQuoteDetailsbyQuotenumber(string QuoteNo)

        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select * from " + TransDB.ToString() + "TQuoteDetails where QuoteNo='" + QuoteNo + "' and CreateStatus ='Article' ";
                da = new SqlDataAdapter(str, EmetCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();
                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();
                    string VendorCode = dtdate.Rows[0].ItemArray[14].ToString();
                    string ProcessGrp = dtdate.Rows[0].ItemArray[13].ToString();

                    string PartUnit = dtdate.Rows[0].ItemArray[18].ToString();

                    //string Vndplant = plant;
                    Session["Vndplant"] = plant;
                    Session["VndId"] = VendorCode;
                    GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass, QuoteNo);
                    GetProcessDetailsbyQuoteDetails(ProcessGrp);
                    GetProcessDetailsbyQuoteDetailsWithNoGroup();


                }
                //var quoteNo = txtQuoteNo.Text;
                using (SqlConnection con = new SqlConnection(EMETModule.GenEMETConnString()))
                {

                    //start
                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 4;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = QuoteNo;
                        
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {
                                string vendorC = pir.Rows[0]["vendor"].ToString();
                                Session["vendorC"] = pir.Rows[0]["vendor"].ToString();
                                lblVName.Text = vendorC + "-" + pir.Rows[0]["description"].ToString();
                                lblCurrency.Text = "Currency: " + pir.Rows[0]["crcy"].ToString();
                                lblCity.Text = "Country: " + pir.Rows[0]["cty"].ToString();
                                lblcry.Text = "(" + pir.Rows[0]["crcy"].ToString() + ")";
                            }
                        }
                    }
                    //End
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
        

        #region Get BOMListRawMaterial Based On Effective Date
        protected void GetMaterialDetailsbyQuoteDetails(string MaterialNo, string Product, string Plant, string MtlClass, string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select RawMaterialCode as [material],RawMaterialDesc as [MaterialDesc],AmtSCur,SellingCrcy,
                    AmtVCur,VendorCrcy,
                    Unit,uom,FORMAT(ValidFrom,'dd-MM-yyyy') as ValidFrom, FORMAT(ValidTo,'dd-MM-yyyy') as ValidTo ,
                    cast(ExchRate as numeric(10,4)) as ExchRate,
                    FORMAT(ExchValidFrom,'dd-MM-yyyy') as ExchValFrom
                    from TSMMBOM_RAWMATCost_EffDate
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvSMNBomEffctvDate.DataSource = dt;
                            GvSMNBomEffctvDate.DataBind();
                            Session["DtMaterialsDetails"] = dt;
                        }
                        else
                        {
                            GvSMNBomEffctvDate.DataSource = null;
                            GvSMNBomEffctvDate.DataBind();
                            Session["DtMaterialsDetails"] = new DataTable();
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
        #endregion

        #region Get BOMListRawMaterial Based On Effective Date
        protected void GetBOMRawmaterialBefEffdate(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select RawMaterialCode as [material],RawMaterialDesc as [MaterialDesc],AmtSCur,SellingCrcy,
                    AmtVCur,VendorCrcy,
                    Unit,uom,FORMAT(ValidFrom,'dd-MM-yyyy') as ValidFrom, FORMAT(ValidTo,'dd-MM-yyyy') as ValidTo ,
                    cast(ExchRate as numeric(10,4)) as ExchRate,
                    FORMAT(ExchValidFrom,'dd-MM-yyyy') as ExchValFrom
                    from TSMNBOM_RAWMATCost
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GVBomListBefEffdate.DataSource = dt;
                            GVBomListBefEffdate.DataBind();
                        }
                        else
                        {
                            GVBomListBefEffdate.DataSource = null;
                            GVBomListBefEffdate.DataBind();
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
        #endregion

        private void GetQuoteandAllDetails(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                GetDbMaster();
                EmetCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @" select top 1 A.RequestNumber,RequestDate,A.QuoteNo,Plant,MaterialType,PlantStatus
                                , SAPProcType, SAPSpProcType, Product, MaterialClass, A.Material, MaterialDesc, PIRType
                                , ProcessGroup, A.VendorCode1, VendorName, PIRJobType, Remarks, NetUnit
                                , DrawingNo, QuoteResponseDueDate,format( A.EffectiveDate, 'dd/MM/yyyy') as  EffectiveDate, format( A.DueOn, 'dd/MM/yyyy') as DueOn
                                , CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as TotalMaterialCost
                                , CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as TotalSubMaterialCost
                                , CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as TotalProcessCost
                                , CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as TotalOtheritemsCost
                                , CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as GrandTotalCost
                                , CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as FinalQuotePrice
                                , Profit, Discount, VendorPIC, VendorPICEmail
                                , ShimanoPIC, ShimanoPICEmail, CreateStatus, ApprovalStatus
                                , PICApprovalStatus, PICReason, ManagerApprovalStatus, ManagerReason
                                , DIRApprovalStatus, DIRReason, PlatingType, UpdatedBy
                                , UpdatedOn, A.CreatedBy, A.CreatedOn, PIRStatus, PIRNumber, PIRCreatedDate
                                , BaseUOM, CommentByVendor, CountryOrg, MQty, ERemarks, PerNetUnit
                                , UOM, ActualNU, ApprovalDate
                                , GA, QuoteNoRef, AcsTabMatCost, AcsTabProcCost, AcsTabSubMatCost
                                , AcsTabOthMatCost, AprRejBy, AprRejDate, isUseSAPCode, FADate, FAQty, DelDate, DelQty
                                , Incoterm, PckReqrmnt, OthReqrmnt, ReqPlant, ManagerRemark, DIRRemark, AprRejByMng, AprRejDateMng
                                , VndAttchmnt, isMassRevision, PIRNo, 
                                CAST(ROUND(OldTotMatCost,5) AS DECIMAL(12,5)) as OldTotMatCost, 
                                CAST(ROUND(OldTotSubMatCost,5) AS DECIMAL(12,5)) as OldTotSubMatCost, 
                                CAST(ROUND(OldTotProCost,5) AS DECIMAL(12,5)) as OldTotProCost, 
                                CAST(ROUND(OldTotOthCost,5) AS DECIMAL(12,5)) as OldTotOthCost, 
                                MassUpdateDate, FORMAT(EmpSubmitionOn,'dd/MM/yyyy') as EmpSubmitionOn
                                ,(select distinct UseNam from " + DbMasterName + @".[dbo].Usr where UseID = EmpSubmitionBy ) as EmpSubmitionBy,
                                format(B.NewEffectiveDate, 'dd/MM/yyyy') as NewEffectiveDate,format(B.NewDueOn, 'dd/MM/yyyy') as NewDueOn,
                                A.IMRecycleRatio,A.SMNPicDept,A.IsUseToolAmortize,A.IsUseMachineAmortize
                                from TQuoteDetails A
                                left join TMngEffDateChgLog B on A.QuoteNo = B.QuoteNo 
                                where A.QuoteNo='" + QuoteNo + @"' and CreateStatus is not null
                                order by B.CreatedOn desc ";
                da = new SqlDataAdapter(str, EmetCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    QuoteStatus = dtdate.Rows[0]["PIRStatus"].ToString();
                    HdnStatus.Value = dtdate.Rows[0]["PIRStatus"].ToString();
                    string plant = dtdate.Rows[0].ItemArray[3].ToString();
                    string Product = dtdate.Rows[0].ItemArray[8].ToString();

                    string Material = dtdate.Rows[0].ItemArray[10].ToString();
                    string MaterialDesc = dtdate.Rows[0].ItemArray[11].ToString();
                    string MtlClass = dtdate.Rows[0].ItemArray[9].ToString();

                    string PartunitTxt = dtdate.Rows[0].ItemArray[18].ToString();
                    string baseUOm = dtdate.Rows[0].ItemArray[51].ToString();
                    TxtPlant.Text = GetPlantPICSMNSubmit(plant);
                    //TxtDepartment.Text = GetDeptPICSMNSubmit(dtdate.Rows[0].ItemArray[46].ToString());
                    TxtDepartment.Text = dtdate.Rows[0]["SMNPicDept"].ToString();
                    txtBaseUOM.Text = baseUOm.ToString();
                    txtBaseUOM1.Text = baseUOm.ToString();
                    txtMQty.Text = dtdate.Rows[0].ItemArray[54].ToString();
                    if (dtdate.Rows[0]["PICReason"].ToString() != "")
                    {
                        txtRem.Text = dtdate.Rows[0]["PICReason"].ToString();
                    }
                    else
                    {
                        txtRem.Text = dtdate.Rows[0]["ERemarks"].ToString();
                    }


                    txtunitweight.Text = dtdate.Rows[0].ItemArray[58].ToString();
                    txtUOM.Text = dtdate.Rows[0].ItemArray[57].ToString();

                    Label11.Text = dtdate.Rows[0].ItemArray[44].ToString();
                    Label15.Text = dtdate.Rows[0].ItemArray[45].ToString();

                    if (dtdate.Rows[0].ItemArray[45].ToString() != "")
                    {
                        DateTime dtEfective11 = DateTime.Parse(dtdate.Rows[0].ItemArray[45].ToString());
                        Label15.Text = dtEfective11.ToString("dd/MM/yyyy HH:mm:ss");
                    }

                    DateTime dt = DateTime.Parse(dtdate.Rows[0].ItemArray[20].ToString());

                    txtquotationDueDate.Text = dt.ToShortDateString();

                    txtdrawng.Text = dtdate.Rows[0].ItemArray[19].ToString();

                    txtprod.Text = dtdate.Rows[0].ItemArray[8].ToString();
                    txtpartdesc.Text = Material + " - " + MaterialDesc;
                    txtSAPJobType.Text = dtdate.Rows[0].ItemArray[16].ToString();
                    txtPIRtype.Text = dtdate.Rows[0].ItemArray[12].ToString();
                    txtprocs.Text = dtdate.Rows[0].ItemArray[13].ToString();

                    txtPartUnit.Value = PartunitTxt.ToString();

                    if (dtdate.Rows[0]["EffectiveDate"].ToString() != "")
                    {
                        TextBox1.Text = dtdate.Rows[0]["EffectiveDate"].ToString();
                    }
                    if (dtdate.Rows[0]["DueOn"].ToString() != "")
                    {
                        txtfinal.Text = dtdate.Rows[0]["DueOn"].ToString();
                    }

                    //if (dtdate.Rows[0].ItemArray[21].ToString() != null && dtdate.Rows[0].ItemArray[21].ToString() != "")
                    //    TextBox1.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[21].ToString()).ToShortDateString();
                    //if (dtdate.Rows[0].ItemArray[22].ToString() != null && dtdate.Rows[0].ItemArray[22].ToString() != "")
                    //    txtfinal.Text = Convert.ToDateTime(dtdate.Rows[0].ItemArray[22].ToString()).ToShortDateString();

                    if (dtdate.Rows[0]["FADate"].ToString() != "")
                    {
                        DateTime FADate = DateTime.Parse(dtdate.Rows[0]["FADate"].ToString());
                        TxtFADate.Text = FADate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TxtFADate.Text = "";
                    }
                    if (dtdate.Rows[0]["DelDate"].ToString() != "")
                    {
                        DateTime DelDate = DateTime.Parse(dtdate.Rows[0]["DelDate"].ToString());
                        TxtDelDate.Text = DelDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TxtDelDate.Text = "";
                    }
                    if (dtdate.Rows[0]["RequestDate"].ToString() != "")
                    {
                        DateTime RD = DateTime.Parse(dtdate.Rows[0]["RequestDate"].ToString());
                        TxtRequestDate.Text = RD.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TxtRequestDate.Text = "";
                    }
                    TxtPlatingType.Text = dtdate.Rows[0]["PlatingType"].ToString();
                    TxtFAQty.Text = dtdate.Rows[0]["FAQty"].ToString();
                    TxtDelQty.Text = dtdate.Rows[0]["DelQty"].ToString();
                    TxtIncoterms.Text = dtdate.Rows[0]["Incoterm"].ToString();
                    TxtPckRequirement.Text = dtdate.Rows[0]["PckReqrmnt"].ToString();
                    TxtOthRequirement.Text = dtdate.Rows[0]["OthReqrmnt"].ToString();
                    TxtPlantRequestor.Text = dtdate.Rows[0]["ReqPlant"].ToString();

                    if (dtdate.Rows[0]["NewEffectiveDate"].ToString() != "" && dtdate.Rows[0]["NewDueOn"].ToString() != "")
                    {
                        DvEffDateChngByMng.Visible = true;
                        TxtEffDateByMng.Text = dtdate.Rows[0]["NewEffectiveDate"].ToString();
                        TxtDueDateByMng.Text = dtdate.Rows[0]["NewDueOn"].ToString();
                    }
                    else
                    {
                        DvEffDateChngByMng.Visible = false;
                    }

                    string a = dtdate.Rows[0]["QuoteNo"].ToString().Substring(dtdate.Rows[0]["QuoteNo"].ToString().Length - 2);
                    if (dtdate.Rows[0]["QuoteNo"].ToString().Substring(dtdate.Rows[0]["QuoteNo"].ToString().Length - 2) == "GP")
                    {
                        DvWhitoutCodeGpField.Style.Add("display", "block");
                        DvReqPlant.Style.Add("display", "block");
                        DvProduct.Style.Add("display", "none");
                        DvSAPPIR.Style.Add("display", "none");
                        DvPlatingType.Visible = false;
                    }
                    else
                    {
                        DvWhitoutCodeGpField.Style.Add("display", "none");
                        DvReqPlant.Style.Add("display", "none");
                        DvProduct.Style.Add("display", "block");
                        DvSAPPIR.Style.Add("display", "block");
                    }

                    //GetMaterialDetailsbyQuoteDetails(Material, Product, plant, MtlClass);
                    GetBOMRawmaterialBefEffdate(dtdate.Rows[0]["QuoteNo"].ToString());

                    string ReqNo = dtdate.Rows[0].ItemArray[0].ToString();
                    string VendorCode = dtdate.Rows[0].ItemArray[14].ToString();
                    string ProcessGroup = dtdate.Rows[0].ItemArray[13].ToString();
                    DateTime requestdate = Convert.ToDateTime(dtdate.Rows[0].ItemArray[1].ToString());

                    ShowTablePrevQuote(Material, VendorCode, ReqNo, requestdate.ToString("yyyy-MM-dd"), ProcessGroup);

                    if (IsTeamShimano(VendorCode) == false)
                    {
                        hdnVendorType.Value = "External";
                    }
                    else
                    {
                        hdnVendorType.Value = "TeamShimano";
                    }

                    string QN = dtdate.Rows[0]["QuoteNo"].ToString();
                    if (dtdate.Rows[0]["VndAttchmnt"].ToString() == "")
                    {
                        LbFlName.Text = "No File";
                        LbFlNameOri.Text = "No File";
                    }
                    else
                    {
                        LbFlName.Text = dtdate.Rows[0]["VndAttchmnt"].ToString().Replace(QN + "-", "");
                        LbFlNameOri.Text = dtdate.Rows[0]["VndAttchmnt"].ToString();
                    }

                    if (dtdate.Rows[0]["QuoteNoRef"].ToString() != "")
                    {
                        DvQuoreReqRevice.Visible = true;
                        hdnQuoteNoRef.Value = dtdate.Rows[0]["QuoteNoRef"].ToString();
                        showDataQuoreReqRevice(dtdate.Rows[0]["QuoteNo"].ToString(), dtdate.Rows[0]["QuoteNoRef"].ToString());
                    }
                    else
                    {
                        DvQuoreReqRevice.Visible = false;
                    }

                    #region massrevision Condition
                    if (dtdate.Rows[0]["isMassRevision"].ToString() == "True")
                    {
                        if (dtdate.Rows[0]["AcsTabMatCost"].ToString() == "True")
                        {
                            hdnMassRevision.Value = "MassMatRevision";
                        }
                        else
                        {
                            hdnMassRevision.Value = "MassMatRevision";
                        }
                        HdnMAssTotMatCost.Value = dtdate.Rows[0]["TotalMaterialCost"].ToString();
                        HdnMAssTotProcCost.Value = dtdate.Rows[0]["TotalProcessCost"].ToString();
                        HdnMAssTotSubMatCost.Value = dtdate.Rows[0]["TotalSubMaterialCost"].ToString();
                        HdnMAssTotOthCost.Value = dtdate.Rows[0]["TotalOtheritemsCost"].ToString();
                        GetOldQuotePIRMass(dtdate.Rows[0]["QuoteNo"].ToString());
                        DvGvSMNBomMassRev.Visible = true;
                        //GetBOMForMassRevision(dtdate.Rows[0]["Material"].ToString(), dtdate.Rows[0]["VendorCode1"].ToString(), dtdate.Rows[0]["Plant"].ToString());
                        if (hdnQuoteNoRefmassRev.Value != "")
                        {
                            DvOldMatCost.Visible = true;
                            DvOldOldProccost.Visible = true;
                            DvOldSubMat.Visible = true;
                            DvOldOthCost.Visible = true;
                            txtOldMatcost.Text = "Old Total Material Cost/pc : " + dtdate.Rows[0]["OldTotMatCost"].ToString();
                            txtOldProccost.Text = "Old Total Process Cost/pc : " + dtdate.Rows[0]["OldTotProCost"].ToString();
                            txtOldSubMat.Text = "Old Total SUB-MAT/T&J Cost/pc : " + dtdate.Rows[0]["OldTotSubMatCost"].ToString();
                            txtOldOthCost.Text = "Old Total Others Cost/pc : " + dtdate.Rows[0]["OldTotOthCost"].ToString();
                        }
                        else
                        {
                            DvOldMatCost.Visible = false;
                            DvOldOldProccost.Visible = false;
                            DvOldSubMat.Visible = false;
                            DvOldOthCost.Visible = false;
                        }
                        if (dtdate.Rows[0]["EmpSubmitionBy"].ToString() != "")
                        {
                            LbSmnSubmitBy.Text = ": " + dtdate.Rows[0]["EmpSubmitionBy"].ToString();
                            LbSmnSubmitOn.Text = ": " + dtdate.Rows[0]["EmpSubmitionOn"].ToString();
                        }
                        else
                        {
                            LbSmnSubmitBy.Text = "Waiting For SMN Submit";
                            LbSmnSubmitOn.Text = "";
                        }
                        DVSmnSubmit.Visible = true;
                    }
                    else
                    {
                       DvGvSMNBomMassRev.Visible = false;
                    }
                    #endregion

                    HdnSAPSpProcType.Value = dtdate.Rows[0]["SAPSpProcType"].ToString();
                    HdnAcsTabMatCost.Value = dtdate.Rows[0]["AcsTabMatCost"].ToString();
                    HdnAcsTabProCost.Value = dtdate.Rows[0]["AcsTabProcCost"].ToString();
                    HdnAcsTabSubMatCost.Value = dtdate.Rows[0]["AcsTabSubMatCost"].ToString();
                    HdnAcsTabOthCost.Value = dtdate.Rows[0]["AcsTabOthMatCost"].ToString();

                    #region SAPSpProcType Condition
                    if (dtdate.Rows[0]["SAPSpProcType"].ToString() == "30")
                    {
                        if (dtdate.Rows[0]["AcsTabMatCost"].ToString() == "False")
                        {
                            LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> Material Cost calculation is disabled, as it is Subcon process and Material will be supplied by Shimano";
                        }
                        else
                        {
                            LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> If Material Cost have only one column and if, Base Qty/Cavity, is changed, after the Process Cost calculation, Please reselect the Process Group again & recalculate the Process Cost";
                        }
                    }
                    else
                    {
                        LbNoteMaterial.Text = "<b><font color='red' size='4'>*</font></b> If Material Cost have only one column and if, Base Qty/Cavity, is changed, after the Process Cost calculation, Please reselect the Process Group again & recalculate the Process Cost";
                    }
                    #endregion

                    if (dtdate.Rows[0]["IMRecycleRatio"].ToString() != "")
                    {
                        DvImRcylRatio.Visible = true;
                        TxtImRcylRatio.Text = dtdate.Rows[0]["IMRecycleRatio"].ToString();
                    }

                    Session["IsUseToolAmortize"] = dtdate.Rows[0]["IsUseToolAmortize"].ToString();
                    if (Session["IsUseToolAmortize"].ToString() == "1" || Session["IsUseToolAmortize"].ToString() == "True")
                    {
                        GetDataToolAmor(dtdate.Rows[0]["QuoteNo"].ToString());
                    }
                    else
                    {
                        Session["VndToolAmortize"] = null;
                    }

                    if (dtdate.Rows[0]["IsUseMachineAmortize"].ToString() == "True")
                    {
                        HdnIsUseMachineAmor.Value = dtdate.Rows[0]["IsUseMachineAmortize"].ToString();
                        string vendorcode = dtdate.Rows[0]["VendorCode1"].ToString();
                        GetDataMachineAmor(plant, vendorcode,QuoteNo);
                    }
                    else
                    {
                        HdnIsUseMachineAmor.Value = "0";
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

        protected void GetDataToolAmor(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                    ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,isnull(CONVERT(DECIMAL(10,2),AmortizeCost),0) as AmortizeCost,AmortizeCurrency
                    ,ExchangeRate,isnull(CONVERT(DECIMAL(10,2),AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                    ,EffectiveFrom,DueDate,CreatedBy,CreatedOn
                    from TToolAmortization
                    where QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            StringBuilder sbSub = new StringBuilder();
                            decimal TotCostPc = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var txtsubDesc = dt.Rows[i]["Amortize_Tool_ID"].ToString();
                                //var txtsubcost = MyDta.Rows[i]["AmortizeCost"].ToString();
                                var txtsubcost = dt.Rows[i]["AmortizeCost_Vend_Curr"].ToString();
                                var txtConsumption = dt.Rows[i]["TotalAmortizeQty"].ToString();
                                var txtsubcostPC = dt.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString();
                                for (int c = 0; c < dt.Rows.Count; c++)
                                {
                                    TotCostPc = TotCostPc + decimal.Parse(dt.Rows[i]["AmortizeCost_Pc_Vend_Curr"].ToString());
                                }
                                decimal txtTotalCostPC = TotCostPc;
                                sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + String.Format("{0:0.00000}", txtTotalCostPC) + ",");
                            }
                            hdnSMCTableValues.Value = sbSub.ToString();
                        }

                        Session["VndToolAmortize"] = dt;
                        HdnIsUseToolAmortize.Value = dt.Rows.Count.ToString();
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

        protected void GetDataMachineAmor(string Plant,string VendorCode, string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct TM.Plant,TM.VendorCode,TM.VendorCurrency,TM.Process_Grp_code,TM.Vend_MachineID
                    ,isnull(CONVERT(DECIMAL(10,2),tm.AmortizeCost),0) as AmortizeCost,TM.AmortizeCurrency,TM.ExchangeRate,isnull(CONVERT(DECIMAL(10,2),tm.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                    ,TM.AmortizePeriod,TM.AmortizePeriodUOM,TM.TotalAmortizeQty,TM.QtyUOM,TM.AmortizeCost_Pc_Vend_Curr
                    ,TM.EffectiveFrom,TM.DueDate
                    from  TMachineAmortization TM
                    Where TM.plant = @Plant
                    and TM.VendorCode = @VendorCode 
                    --and TM.Process_Grp_code = @ProcGrp 
                    and TM.QuoteNo=@QuoteNo
                    ORDER BY TM.Vend_MachineID";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Plant", Plant);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    //cmd.Parameters.AddWithValue("@ProcGrp", txtprocs.Text);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvMachineAmor.DataSource = dt;
                            GvMachineAmor.DataBind();
                            HdnIsUseMachineAmor.Value = dt.Rows.Count.ToString();
                            Session["VndMachineAmortize"] = dt;
                        }
                        else
                        {
                            Session["VndMachineAmortize"] = null;
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

        protected void GetDbTrans()
        {
            try
            {
                DbTransName = EMETModule.GetDbTransname() + ".[dbo].";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbTransName = "";
            }
        }


        /// <summary>
        /// Disabled because all grid bom will be keep show even if mass or new request type and new grid is created for bom before effective date
        /// </summary>
        /// <param name="Material"></param>
        /// <param name="VendorCode"></param>
        /// <param name="Plant"></param>
        protected void GetBOMForMassRevision(string Material, string VendorCode, string Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                GetDbTrans();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                    //        isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                    //        tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  
                    //        from TMATERIAL tm 
                    //        inner join TBOMLIST TB on tm.Material = TB.Material 
                    //        inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material 
                    //        inner join tVendor_New tv on tv.customerNo=tc.customer  
                    //        inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                    //        inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                    //        left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                    //        where  tm.plant=@Plant  
                    //        and tvp.plant= @Plant and TB.FGcode=@Material 
                    //        and tv.Vendor=@Vendor and TM.DELFLAG = 0
                    //        --and tc.ValidFrom <= @ValidTo
                    //        and tc.ValidTo >= @ValidTo ";

                    //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                    sql = @" select distinct   tm.material,tm.MaterialDesc,tc.Unit,
                            isnull(ROUND(CAST((tc.amount*isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amount,
                            tv.Crcy AS Venor_Crcy, tc.UnitofCurrency as Selling_Crcy,isnull(ROUND(CAST((tc.amount) As decimal(20,2)),2) ,'1')as OAmount  ,
                            isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                            tm.BaseUOM as UOM,format(TR.ValidFrom,'dd/MM/yyyy') as ValidFrom
                            from TMATERIAL tm 
                            inner join TBOMLIST TB on tm.Material = TB.Material 
                            inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material 
                            inner join tVendor_New tv on tv.customerNo=tc.customer  
                            inner join tVendorPOrg tvp on tvp.POrg=tv.POrg 
                            inner join " + DbTransName + @"TQuoteDetails TQ on tv.Vendor = TQ.VendorCode1 
                            left outer join TEXCHANGE_RATE tr on tc.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo and tm.Plant = TQ.Plant 
                            and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                            where  tm.plant=@Plant  
                            and tvp.plant= @Plant and TB.FGcode=@Material 
                            and tv.Vendor=@Vendor and TM.DELFLAG = 0
                            and tc.ValidFrom <= @ValidTo
                            and tc.ValidTo >= @ValidTo 
                            ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Plant);
                    cmd.Parameters.AddWithValue("@Vendor", VendorCode);
                    cmd.Parameters.AddWithValue("@Material", Material);
                    string[] ArrValidTo = TextBox1.Text.Split('/');
                    string Day = "01";
                    string Mnth = "01";
                    string Year = "9999";
                    string NewValidDate = "";
                    if (ArrValidTo.Length > 0)
                    {
                        if (ArrValidTo[0].ToString().Length == 1)
                        {
                            Day = "0" + ArrValidTo[0].ToString();
                        }
                        else
                        {
                            Day = ArrValidTo[0].ToString();
                        }
                        if (ArrValidTo[1].ToString().Length == 1)
                        {
                            Mnth = "0" + ArrValidTo[1].ToString();
                        }
                        else
                        {
                            Mnth = ArrValidTo[1].ToString();
                        }
                        Year = ArrValidTo[2].ToString();
                    }
                    NewValidDate = Day + "/" + Mnth + "/" + Year;
                    DateTime ValidToo = DateTime.ParseExact(NewValidDate, "dd/MM/yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidToo.ToString("yyyy/MM/dd"));
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            GvSMNBomEffctvDate.DataSource = dt;
                            GvSMNBomEffctvDate.DataBind();
                            DvGvSMNBomMassRev.Visible = true;
                            UpdatePanel4.Update();
                            Session["DtMaterialsDetails"] = dt;
                        }
                        else
                        {
                            DvGvSMNBomMassRev.Visible = false;
                            Session["DtMaterialsDetails"] = new DataTable();
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
                MDMCon.Dispose();
            }
        }


        private void GetQuoteupdated(string QuoteNo)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select useid,usenam from usr where useid='" + Label11.Text + "' ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {

                    Label14.Text = dtdate.Rows[0].ItemArray[0].ToString() + " - " + dtdate.Rows[0].ItemArray[1].ToString();

                }
                else if (Label11.Text == "EMET")
                {
                    Label14.Text = "-";
                }
                else
                {
                    Label14.Text = "Waiting for Vendor Submission";
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


        //Used this method by Raja
        public void GetMETfields(string ProcessGrpCode, string FieldGroup)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                #region access layout from New table
                DataTable dt = new DataTable();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select f.FieldName from tMETFieldsConditionNew C
                            inner join tMETFileds F on c.FieldId = f.FieldId and ISNULL(f.DelFlag,0)=0
                            inner join TPROCESGRP_SCREENLAYOUT P on c.LayoutID = p.ScreenLayout  and ISNULL(p.DelFlag,0)=0
                            where p.ProcessGrp=@ProcessGrpCode and f.FieldGroup=@FieldGroup and isnull(c.delflag,0)=0
                            group by
                            c.LayoutID,c.FieldId,f.FieldName ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrpCode", ProcessGrpCode);
                    cmd.Parameters.AddWithValue("@FieldGroup", FieldGroup);
                    sda.SelectCommand = cmd;
                    using (DataTable Mydt = new DataTable())
                    {
                        sda.Fill(Mydt);
                        dt = Mydt;
                    }
                }

                if (FieldGroup == "MC")
                {
                    if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            DataTable dtn = new DataTable();
                            dtn.Columns.Add("FieldName", typeof(String));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow selectedRow = dt.Rows[i];

                                if (dt.Rows[i]["FieldName"].ToString() == "~Runner Weight/shot (g)")
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        if (dt.Rows[j]["FieldName"].ToString() == "Cavity")
                                        {
                                            DataRow RCavity = dtn.NewRow();
                                            RCavity["FieldName"] = "Cavity";
                                            dtn.Rows.InsertAt(RCavity, i);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Runner Weight/shot (g)")
                                        {
                                            DataRow newRow0 = dtn.NewRow();
                                            newRow0["FieldName"] = "~Runner Weight/shot (g)";
                                            dtn.Rows.InsertAt(newRow0, i + 1);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Runner Ratio/pcs (%)")
                                        {
                                            DataRow newRow1 = dtn.NewRow();
                                            newRow1["FieldName"] = "~Runner Ratio/pcs (%)";
                                            dtn.Rows.InsertAt(newRow1, i + 2);
                                        }

                                        if (dt.Rows[j]["FieldName"].ToString() == "~Recycle Material Ratio (%)")
                                        {
                                            DataRow newRow2 = dtn.NewRow();
                                            newRow2["FieldName"] = "~Recycle Material Ratio (%)";
                                            dtn.Rows.InsertAt(newRow2, i + 3);
                                        }
                                    }
                                }
                                else if (dt.Rows[i][0].ToString() == "Cavity" || dt.Rows[i][0].ToString() == "~Recycle Material Ratio (%)" ||
                                    dt.Rows[i][0].ToString() == "~Runner Ratio/pcs (%)") { }
                                else
                                {
                                    DataRow newRow = dtn.NewRow();
                                    newRow.ItemArray = selectedRow.ItemArray; // copy data
                                    dtn.Rows.InsertAt(newRow, i);
                                }
                            }
                            dt = dtn;
                        }
                    }
                    Session["DtDynamic"] = dt;
                }
                else if (FieldGroup == "PC")
                {
                    Session["DtDynamicProcessFields"] = dt;
                }
                else if (FieldGroup == "SMC")
                {
                    Session["DtDynamicSubMaterialsFields"] = dt;
                }
                else if (FieldGroup == "Unit")
                {
                    Session["DtDynamicUnitFields"] = dt;
                }
                else if (FieldGroup == "Others")
                {
                    Session["DtDynamicOtherCostsFields"] = dt;
                }
                #endregion
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

        public void CreateDynamicTablebasedonProcessField()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct FieldGroup from tMETFileds";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtdate);

                foreach (DataRow row in dtdate.Rows)
                {
                    GetMETfields(txtprocs.Text, row.ItemArray[0].ToString());
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

        #region old 25-06-2019 CreateDynamicDT(int ColumnType)
        //private void CreateDynamicDT(int ColumnType)
        //{
        //    DataTable DtDynamic = new DataTable();
        //    DtDynamic = (DataTable)Session["DtDynamic"];

        //    DataTable DtMaterialsDetails = new DataTable();
        //    DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;
        //        #region OldWays
        //        //if (DtMaterialsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    Table1.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }
        //        //    foreach (DataRow row in DtDynamic.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        Table1.Rows.Add(tRow);

        //        //        for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString();
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tCell.Controls.Add(tb);
        //        //                if (tb.ID.Contains("txtMaterialSAPCode"))
        //        //                {
        //        //                    tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtMaterialDescription"))
        //        //                {
        //        //                    tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //        //                    //tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                {
        //        //                    double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //        //                    double rawperkg = RawVal / 1000;
        //        //                    tb.Text = (rawperkg.ToString());
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //        //                {
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Text = txtPartUnit.Value;
        //        //                }

        //        //                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {
        //        //                    tb.MaxLength = 4;
        //        //                }

        //        //                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)") || tb.ID.Contains("txtScrapRebate/pcs") )
        //        //                {

        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tCell.Controls.Add(tb);
        //        //                    Table1.Rows[cellCtr].Cells.Add(tCell);

        //        //                }

        //        //                if (txtPIRtype.Text.Contains("SUBCON"))
        //        //                {


        //        //                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                    {
        //        //                        tb.Text = "";
        //        //                    }
        //        //                    if (tb.ID.Contains("Cavity"))
        //        //                    {
        //        //                        //tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                }

        //        //                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //        //                {
        //        //                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == rowcount)
        //        //                        {
        //        //                            tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tRow.Cells.Add(tCell);

        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    Table1.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    //Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        Table1.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                Table1.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");

        //        //                tCell.Controls.Add(tb);


        //        //                if (tb.ID.Contains("txtMaterialSAPCode"))
        //        //                {
        //        //                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                    // tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtMaterialDescription"))
        //        //                {
        //        //                    // tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //        //                    //tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                {
        //        //                    //double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //        //                    //double rawperkg = RawVal / 1000;
        //        //                    //tb.Text = (rawperkg.ToString());
        //        //                    //// tb.Enabled = false;
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                }
        //        //                else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //        //                {
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Text = txtPartUnit.Value;
        //        //                }

        //        //                if (tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {
        //        //                    tb.MaxLength = 4;
        //        //                }

        //        //                if (tb.ID.Contains("TotalRawMaterialCost/g") || tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("MaterialCost/pcs") || tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RunnerRatio/pcs(%)"))
        //        //                {

        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tCell.Controls.Add(tb);
        //        //                    Table1.Rows[cellCtr].Cells.Add(tCell);

        //        //                }

        //        //                if (txtPIRtype.Text.Contains("SUBCON"))
        //        //                {


        //        //                    if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //        //                    {
        //        //                        tb.Text = "";
        //        //                    }
        //        //                    if (tb.ID.Contains("Cavity"))
        //        //                    {
        //        //                        //tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        tb.Attributes.Add("disabled", "disabled");
        //        //                    }
        //        //                }
        //        //                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //        //                {
        //        //                    var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == rowcountnew)
        //        //                        {
        //        //                            tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tRow.Cells.Add(tCell);
        //        //            }

        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion OldWays

        //        if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //        {
        //            #region condition with data filled by vendor
        //            int rowcountnew = 0;
        //            TableRow Hearderrow = new TableRow();

        //            Table1.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                Hearderrow.Cells.Add(tCell1);
        //                tCell1.Width = 280;
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            //Table1 = (Table)Session["Table"];
        //            for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //            {
        //                TableRow tRow = new TableRow();
        //                Table1.Rows.Add(tRow);

        //                for (int i = 0; i <= 1; i++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (i == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
        //                        {
        //                            lb.Text = "Base Qty / Cavity";
        //                        }
        //                        else
        //                        {
        //                            lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
        //                        }

        //                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
        //                        {
        //                            lb.Text = "Material/Melting Loss (%)";
        //                        }
        //                        lb.Width = 280;
        //                        Table1.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        //tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                        tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tCell.Controls.Add(tb);

        //                        if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //                        {
        //                            var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //                            for (int ii = 0; ii < tempMClist.Count; ii++)
        //                            {
        //                                if (ii == rowcountnew)
        //                                {
        //                                    tb.Text = tempMClist[ii].ToString().Replace("NaN", "");
        //                                    break;
        //                                }

        //                            }
        //                        }
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tCell.Width = 280;
        //                        tRow.Cells.Add(tCell);
        //                    }

        //                }
        //                if (rowcountnew % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcountnew++;
        //            }
        //            Session["Table"] = Table1;
        //            #endregion with data filled by vendor
        //        }
        //        else
        //        {
        //            #region Waiting vendor response
        //            if (DtMaterialsDetails.Rows.Count > 0)
        //            {
        //                #region Waiting vendor response if request with BOM
        //                TableRow Hearderrow = new TableRow();
        //                Table1.Rows.Add(Hearderrow);
        //                for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //                {
        //                    TableCell tCell1 = new TableCell();
        //                    Label lb1 = new Label();
        //                    tCell1.Controls.Add(lb1);
        //                    if (cellCtr == 0)
        //                        tCell1.Text = "Field Name";

        //                    Hearderrow.Cells.Add(tCell1);
        //                    Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    Hearderrow.ForeColor = Color.White;
        //                }
        //                foreach (DataRow row in DtDynamic.Rows)
        //                {
        //                    TableRow tRow = new TableRow();
        //                    Table1.Rows.Add(tRow);

        //                    for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
        //                    {
        //                        int count = 1;
        //                        TableCell tCell = new TableCell();
        //                        if (cellCtr == 0)
        //                        {
        //                            Label lb = new Label();
        //                            tCell.Controls.Add(lb);
        //                            if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
        //                            {
        //                                lb.Text = "Base Qty / Cavity";
        //                            }
        //                            else
        //                            {
        //                                lb.Text = row.ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
        //                            }

        //                            if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
        //                            {
        //                                lb.Text = "Material/Melting Loss (%)";
        //                            }
        //                            tCell.Width = 280;
        //                            tRow.Cells.Add(tCell);
        //                        }
        //                        else
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Attributes.Add("disabled", "disabled");
        //                            tCell.Controls.Add(tb);
        //                            if (tb.ID.Contains("txtMaterialSAPCode"))
        //                            {
        //                                tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //                            }
        //                            else if (tb.ID.Contains("txtMaterialDescription"))
        //                            {
        //                                tb.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //                            }
        //                            else if (tb.ID.Contains("txtRawMaterialCost/kg"))
        //                            {
        //                                double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[6].ToString());

        //                                double rawperkg = RawVal / 1000;
        //                                tb.Text = (rawperkg.ToString());
        //                            }
        //                            else if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //                            {
        //                                tb.Text = txtPartUnit.Value;
        //                            }
        //                            tCell.Width = 280;
        //                            tRow.Cells.Add(tCell);
        //                        }
        //                    }

        //                    if (rowcount % 2 == 0)
        //                    {
        //                        tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                        tRow.BackColor = Color.White;
        //                    }
        //                    else
        //                    {
        //                        //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                        tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                    }
        //                    rowcount++;
        //                }
        //                #endregion Waiting vendor response if request with BOM
        //            }
        //            else
        //            {
        //                #region Waiting vendor response No BOM
        //                int rowcountnew = 0;

        //                TableRow Hearderrow = new TableRow();

        //                Table1.Rows.Add(Hearderrow);
        //                for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //                {
        //                    TableCell tCell1 = new TableCell();
        //                    Label lb1 = new Label();
        //                    tCell1.Controls.Add(lb1);
        //                    if (cellCtr == 0)
        //                        tCell1.Text = "Field Name";
        //                    Hearderrow.Cells.Add(tCell1);
        //                    Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    Hearderrow.ForeColor = Color.White;
        //                }

        //                //Table1 = (Table)Session["Table"];
        //                for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //                {
        //                    TableRow tRow = new TableRow();
        //                    Table1.Rows.Add(tRow);

        //                    for (int i = 0; i <= 1; i++)
        //                    {
        //                        TableCell tCell = new TableCell();
        //                        if (i == 0)
        //                        {
        //                            Label lb = new Label();
        //                            tCell.Controls.Add(lb);
        //                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
        //                            {
        //                                lb.Text = "Base Qty / Cavity";
        //                            }
        //                            else
        //                            {
        //                                lb.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
        //                            }

        //                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
        //                            {
        //                                lb.Text = "Material/Melting Loss (%)";
        //                            }
        //                            lb.Width = 280;
        //                            Table1.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                        else
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Attributes.Add("disabled", "disabled");
        //                            tCell.Controls.Add(tb);
        //                            if (tb.ID.Contains("txtPartNetUnitWeight(g)"))
        //                            {
        //                                tb.Text = txtPartUnit.Value;
        //                            }
        //                            tCell.Width = 280;
        //                            tRow.Cells.Add(tCell);
        //                        }

        //                    }
        //                    if (rowcountnew % 2 == 0)
        //                    {
        //                        tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                        tRow.BackColor = Color.White;
        //                    }
        //                    else
        //                    {
        //                        //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                        tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                    }
        //                    rowcountnew++;
        //                }
        //                #endregion Waiting vendor response No BOM
        //            }
        //            #endregion Waiting vendor response
        //        }
        //    }
        //    else
        //    {
        //        Table1 = (Table)Session["Table"];

        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //            var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

        //            var TempMClistNew = tempMClist;

        //            //if (CellsCount == 2 && tempMClist.Count <= (DtDynamic.Rows.Count + 1))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i)) * (DtDynamic.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempSMClist.Count > 6)
        //            ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount >= 3 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamic.Rows.Count)).ToList();
        //            //else if (CellsCount >= 4 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(((CellsCount) * (DtDynamic.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempMClist.Count > (DtDynamic.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempMClistNew = TempMClistNew.Skip(i * (DtDynamic.Rows.Count)).ToList();

        //            //if (i == 1)
        //            //    TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();
        //            //else

        //            TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

        //            for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    tCell.Width = 280;
        //                    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");

        //                    tb.Style.Add("text-transform", "uppercase");
        //                    if (tb.ID.Contains("RunnerWeight/shot(g)") || tb.ID.Contains("RunnerRatio/pcs(%)") || 
        //                        tb.ID.Contains("MaterialGrossWeight/pc(g)") || tb.ID.Contains("PartNetUnitWeight(g)") || 
        //                        tb.ID.Contains("TotalMaterialCost/pcs") || tb.ID.Contains("RecycleMaterialRatio(%)") || 
        //                        tb.ID.Contains("Cavity") || tb.ID.Contains("MaterialYield/MeltingLoss(%)"))
        //                    {
        //                        if (txtprocs.Text.ToUpper() != "ST")
        //                        {
        //                            Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                            //((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            //if (tb.ID.Contains("PartNetUnitWeight(g)"))
        //                            //{
        //                            //    tb.Text = txtPartUnit.Value;
        //                            //    tCell.Controls.Add(tb);
        //                            //    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            //}
        //                            //else
        //                            //{
        //                            //    tCell.Controls.Add(tb);
        //                            //    Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            //}
        //                        }
        //                        else
        //                        {
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");
        //                            if (tb.ID.Contains("PartNetUnitWeight(g)"))
        //                            {
        //                                tb.Text = txtPartUnit.Value;
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                                //tb.Attributes.Add("disabled", "disabled");
        //                            }
        //                            else if (tb.ID.Contains("txtTotalMaterialCost/pcs"))
        //                            {
        //                                Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
        //                            }
        //                            else if (tb.ID.Contains("txtMaterialGrossWeight/pc(g)"))
        //                            {
        //                                tb.Attributes.Add("disabled", "disabled");
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            }
        //                            else
        //                            {
        //                                tCell.Controls.Add(tb);
        //                                Table1.Rows[cellCtr].Cells.Add(tCell);
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        tCell.Width = 280;
        //                        Table1.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < TempMClistNew.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {
        //                                tb.Text = TempMClistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");
        //                }
        //            }
        //        }

        //        Session["Table"] = Table1;
        //        Table1.DataBind();
        //    }
        //    Session["Table"] = Table1;

        //}
        #endregion old 25-06-2019 CreateDynamicDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicDT(int ColumnType)
        private void CreateDynamicDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamic = new DataTable();
                DtDynamic = (DataTable)Session["DtDynamic"];

                DataTable DtMaterialsDetails = new DataTable();
                DtMaterialsDetails = (DataTable)Session["DtMaterialsDetails"];

                if (ColumnType == 0)
                {
                    int rowcount = 0;

                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                    {
                        #region condition with data filled by vendor
                        int rowcountnew = 0;
                        TableRow Hearderrow = new TableRow();

                        Table1.Rows.Add(Hearderrow);
                        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                        {
                            TableCell tCell1 = new TableCell();
                            if (cellCtr == 0)
                            {
                                tCell1.Text = "Field Name";
                            }
                            Hearderrow.Cells.Add(tCell1);
                            tCell1.Width = 150;
                            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                            Hearderrow.ForeColor = Color.White;
                        }

                        //Table1 = (Table)Session["Table"];
                        for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                        {
                            TableRow tRow = new TableRow();
                            Table1.Rows.Add(tRow);

                            for (int i = 0; i <= 1; i++)
                            {
                                TableCell tCell = new TableCell();
                                if (i == 0)
                                {
                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                    {
                                        tCell.Text = "Raw Material SAP Code";
                                    }
                                    else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                    {
                                        tCell.Text = "Raw Material Description";
                                    }
                                    else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                    {
                                        tCell.Text = "Base Qty / Cavity";
                                    }
                                    else
                                    {
                                        tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                    {
                                        tCell.Text = "Material/Melting Loss (%)";
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                    {
                                        tCell.Text = "Raw Material Cost";
                                    }

                                    if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                    {
                                        tCell.Text = "Total Raw Material Cost";
                                    }

                                    if (hdnLayoutScreen.Value == "Layout7")
                                    {
                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                        {
                                            //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                        {
                                            tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                        }
                                    }
                                    tCell.Width = 150;
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                    if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                    {
                                        tCell.Style.Add("text-align", "left");
                                    }
                                    else
                                    {
                                        tCell.Style.Add("text-align", "right");
                                    }

                                    if (FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout1")
                                        {
                                            tCell.Text = "5";
                                        }
                                        else if (hdnLayoutScreen.Value == "Layout3")
                                        {
                                            tCell.Text = "10";
                                        }
                                        else if (hdnLayoutScreen.Value == "Layout5")
                                        {
                                            tCell.Text = "2";
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }

                                    if (FieldName.Contains("MaterialDensity"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout5")
                                        {
                                            tCell.Text = "7.86";
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }

                                    if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                    {
                                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempMClist.Count; ii++)
                                        {
                                            if (ii == rowcountnew)
                                            {
                                                tCell.Text = tempMClist[ii].ToString().Replace("NaN", "").ToUpper();
                                                break;
                                            }

                                        }
                                    }
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    tCell.Width = 150;
                                    tRow.Cells.Add(tCell);
                                }

                            }
                            if (rowcountnew % 2 == 0)
                            {
                                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                                tRow.BackColor = Color.White;
                            }
                            else
                            {
                                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            rowcountnew++;
                        }
                        Session["Table"] = Table1;
                        #endregion with data filled by vendor
                    }
                    else
                    {
                        #region Waiting vendor response
                        if (DtMaterialsDetails.Rows.Count > 0)
                        {
                            #region Waiting vendor response if request with BOM
                            TableRow Hearderrow = new TableRow();
                            Table1.Rows.Add(Hearderrow);
                            for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                            {
                                TableCell tCell1 = new TableCell();
                                if (cellCtr == 0)
                                {
                                    tCell1.Text = "Field Name";
                                }

                                Hearderrow.Cells.Add(tCell1);
                                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                                Hearderrow.ForeColor = Color.White;
                            }
                            foreach (DataRow row in DtDynamic.Rows)
                            {
                                TableRow tRow = new TableRow();
                                Table1.Rows.Add(tRow);

                                for (int cellCtr = 0; cellCtr <= DtMaterialsDetails.Rows.Count; cellCtr++)
                                {
                                    int count = 1;
                                    TableCell tCell = new TableCell();
                                    if (cellCtr == 0)
                                    {
                                        if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                        {
                                            tCell.Text = "Raw Material SAP Code";
                                        }
                                        else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                        {
                                            tCell.Text = "Raw Material Description";
                                        }
                                        else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                        {
                                            tCell.Text = "Base Qty / Cavity";
                                        }
                                        else
                                        {
                                            tCell.Text = row.ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                        {
                                            tCell.Text = "Material/Melting Loss (%)";
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                        {
                                            tCell.Text = "Raw Material Cost";
                                        }

                                        if (row.ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                        {
                                            tCell.Text = "Total Raw Material Cost";
                                        }

                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                            {
                                                //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                            }

                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                            {
                                                tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                            }
                                        }
                                        tCell.Width = 150;
                                        tRow.Cells.Add(tCell);
                                    }
                                    else
                                    {
                                        string FieldName = row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                        if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Style.Add("text-align", "left");
                                        }
                                        else
                                        {
                                            tCell.Style.Add("text-align", "right");
                                        }

                                        if (FieldName.Contains("MaterialSAPCode"))
                                        {
                                            tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
                                        }
                                        else if (FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
                                        }
                                        else if (FieldName.Contains("RawMaterialCost/kg"))
                                        {
                                            string a = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[4].ToString();
                                            double RawVal = Double.Parse(DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[4].ToString());

                                            double rawperkg = RawVal / 1000;
                                            tCell.Text = (String.Format("{0:0.000}", rawperkg));
                                        }
                                        else if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                        }
                                        else if (FieldName.Contains("TotalMaterialCost/pcs"))
                                        {
                                            if (hdnMassRevision.Value != "")
                                            {
                                                tCell.Text = HdnMAssTotMatCost.Value;
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }

                                        if (FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                        {
                                            if (hdnLayoutScreen.Value == "Layout1")
                                            {
                                                tCell.Text = "5";
                                            }
                                            else if (hdnLayoutScreen.Value == "Layout3")
                                            {
                                                tCell.Text = "10";
                                            }
                                            else if (hdnLayoutScreen.Value == "Layout5")
                                            {
                                                tCell.Text = "2";
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }

                                        if (FieldName.Contains("MaterialDensity"))
                                        {
                                            if (hdnLayoutScreen.Value == "Layout5")
                                            {
                                                tCell.Text = "7.86";
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }

                                        tCell.Width = 150;
                                        tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                        tRow.Cells.Add(tCell);
                                    }
                                }

                                if (rowcount % 2 == 0)
                                {
                                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                                    tRow.BackColor = Color.White;
                                }
                                else
                                {
                                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                                }
                                rowcount++;
                            }
                            #endregion Waiting vendor response if request with BOM
                        }
                        else
                        {
                            #region Waiting vendor response No BOM
                            int rowcountnew = 0;

                            TableRow Hearderrow = new TableRow();

                            Table1.Rows.Add(Hearderrow);
                            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                            {
                                TableCell tCell1 = new TableCell();
                                if (cellCtr == 0)
                                {
                                    tCell1.Text = "Field Name";
                                }
                                Hearderrow.Cells.Add(tCell1);
                                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                                Hearderrow.ForeColor = Color.White;
                            }

                            //Table1 = (Table)Session["Table"];
                            for (int cellCtr = 1; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                            {
                                TableRow tRow = new TableRow();
                                Table1.Rows.Add(tRow);

                                for (int i = 0; i <= 1; i++)
                                {
                                    TableCell tCell = new TableCell();
                                    if (i == 0)
                                    {
                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALSAPCODE"))
                                        {
                                            tCell.Text = "Raw Material SAP Code";
                                        }
                                        else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALDESCRIPTION"))
                                        {
                                            tCell.Text = "Raw Material Description";
                                        }
                                        else if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("CAVITY"))
                                        {
                                            tCell.Text = "Base Qty / Cavity";
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "Loss").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc");
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("MATERIALYIELD/MELTINGLOSS(%)"))
                                        {
                                            tCell.Text = "Material/Melting Loss (%)";
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("RawMaterialCost/kg"))
                                        {
                                            tCell.Text = "Raw Material Cost";
                                        }

                                        if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").Contains("TotalRawMaterialCost/g"))
                                        {
                                            tCell.Text = "Total Raw Material Cost";
                                        }

                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/KG"))
                                            {
                                                //tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/pc").Replace("/ kg", "/ UOM");
                                            }

                                            if (DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("/PCS"))
                                            {
                                                tCell.Text = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("Yield", "").Replace("~", "").Replace("~~", "").Replace("/pcs", "/UOM").Replace("/ kg", "/ UOM");
                                            }
                                        }
                                        tCell.Width = 150;
                                        Table1.Rows[cellCtr].Cells.Add(tCell);
                                    }
                                    else
                                    {
                                        string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                        if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                        {
                                            tCell.Style.Add("text-align", "left");
                                        }
                                        else
                                        {
                                            tCell.Style.Add("text-align", "right");
                                        }
                                        if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                        }

                                        if (FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                        {
                                            if (hdnLayoutScreen.Value == "Layout1")
                                            {
                                                tCell.Text = "5";
                                            }
                                            else if (hdnLayoutScreen.Value == "Layout3")
                                            {
                                                tCell.Text = "10";
                                            }
                                            else if (hdnLayoutScreen.Value == "Layout5")
                                            {
                                                tCell.Text = "2";
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }

                                        if (FieldName.Contains("MaterialDensity"))
                                        {
                                            if (hdnLayoutScreen.Value == "Layout5")
                                            {
                                                tCell.Text = "7.86";
                                            }
                                            else
                                            {
                                                tCell.Text = "";
                                            }
                                        }

                                        tCell.Width = 150;
                                        tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                        tRow.Cells.Add(tCell);
                                    }

                                }
                                if (rowcountnew % 2 == 0)
                                {
                                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                                    tRow.BackColor = Color.White;
                                }
                                else
                                {
                                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                                }
                                rowcountnew++;
                            }
                            #endregion Waiting vendor response No BOM
                        }
                        #endregion Waiting vendor response
                    }
                }
                else
                {
                    Table1 = (Table)Session["Table"];

                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {

                        var tempMClist = hdnMCTableValues.Value.ToString().Split(',').ToList();

                        var TempMClistNew = tempMClist;

                        TempMClistNew = tempMClist.Skip(i * (DtDynamic.Rows.Count)).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamic.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                tCell.Width = 150;
                                Table1.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamic.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("MaterialSAPCode") || FieldName.Contains("MaterialDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                if (FieldName.Contains("RunnerWeight/shot(g)") || FieldName.Contains("RunnerRatio/pcs(%)") ||
                                    FieldName.Contains("MaterialGrossWeight/pc(g)") || FieldName.Contains("PartNetUnitWeight(g)") ||
                                    FieldName.Contains("TotalMaterialCost/pcs") || FieldName.Contains("RecycleMaterialRatio(%)") ||
                                    FieldName.Contains("Cavity") || FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                {
                                    if (txtprocs.Text.ToUpper() != "ST")
                                    {
                                        Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                        ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                    }
                                    else
                                    {
                                        if (FieldName.Contains("PartNetUnitWeight(g)"))
                                        {
                                            tCell.Text = txtPartUnit.Value;
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                            //tb.Attributes.Add("disabled", "disabled");
                                        }
                                        else if (FieldName.Contains("TotalMaterialCost/pcs"))
                                        {
                                            ((System.Web.UI.WebControls.WebControl)(Table1.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                            Table1.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                        }
                                        else
                                        {
                                            Table1.Rows[cellCtr].Cells.Add(tCell);
                                        }
                                    }

                                }
                                else
                                {
                                    tCell.Width = 150;
                                    Table1.Rows[cellCtr].Cells.Add(tCell);
                                }

                                if (FieldName.Contains("MaterialYield/MeltingLoss(%)"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout1")
                                    {
                                        tCell.Text = "5";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout3")
                                    {
                                        tCell.Text = "10";
                                    }
                                    else if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tCell.Text = "2";
                                    }
                                    else
                                    {
                                        tCell.Text = "";
                                    }
                                }

                                if (FieldName.Contains("MaterialDensity"))
                                {
                                    if (hdnLayoutScreen.Value == "Layout5")
                                    {
                                        tCell.Text = "7.86";
                                    }
                                    else
                                    {
                                        tCell.Text = "";
                                    }
                                }

                                if (hdnMCTableValues.Value != null && hdnMCTableValues.Value != "")
                                {
                                    for (int ii = 0; ii < TempMClistNew.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = TempMClistNew[ii].ToString().Replace("NaN", "").ToUpper();
                                            break;
                                        }
                                    }
                                }

                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                        }
                    }

                    Session["Table"] = Table1;
                    Table1.DataBind();
                }
                Session["Table"] = Table1;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }
        #endregion New 25-06-2019 CreateDynamicDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        //private void CreateDynamicProcessDT(int ColumnType)
        //{
        //    DataTable DtDynamicProcessFields = new DataTable();
        //    DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

        //    DataTable DtDynamicProcessCostsDetails = new DataTable();
        //    DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TablePC.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }
        //        foreach (DataRow row in DtDynamicProcessFields.Rows)
        //        {
        //            TableRow tRow = new TableRow();
        //            TablePC.Rows.Add(tRow);
        //            for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("IFTURNKEY-VENDORNAME"))
        //                    {
        //                        tCell.Text = "If Subcon - Subcon Name";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("VENDORRATE"))
        //                    {
        //                        tCell.Text = "Vendor Rate/HR";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //                    {
        //                        tCell.Text = "Total Process Cost/pc";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TURNKEYPROFIT"))
        //                    {
        //                        tCell.Text = "Turnkey Fees";
        //                    }
        //                    else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("EFFICIENCY/PROCESSYIELD(%)"))
        //                    {
        //                        tCell.Text = "Efficiency";
        //                    }
        //                    else
        //                    {
        //                        tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
        //                    }
        //                    //tCell.Text = row.ItemArray[0].ToString();
        //                    tCell.Width = 280;
        //                    tRow.Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    #region create form control old

        //                    //if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlProcess" + (cellCtr - 1);
        //                    //    ddl.DataSource = Session["process"];
        //                    //    ddl.DataTextField = "Process_Grp_code";
        //                    //    //ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged2;
        //                    //    ddl.DataValueField = "Process_Grp_code";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();

        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                    string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                    MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                    string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                    MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                }
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data
        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubProcess" + (cellCtr - 1);


        //                    //    ddl.DataSource = Session["PSGroupwithUOM"];
        //                    //    ddl.DataTextField = "SubProcessName";

        //                    //    ddl.DataValueField = "SubProcessName";
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                var ddlcheck1 = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //                        }
        //                    //                    }
        //                    //                    if (ddlcheck1 != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(tempPClist[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);
        //                    //                        }
        //                    //                    }
        //                    //                    if (ddlcheck1 != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                        //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                }

        //                    //                break;

        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubvendorname" + (cellCtr - 1);
        //                    //    ddl.DataSource = Session["SubVndorData"];
        //                    //    ddl.DataTextField = "SubVndorData";
        //                    //    ddl.DataValueField = "SubVndorData";
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    string ddltemval = tempPClist[ii].ToString().Replace("NaN", "");

        //                    //                    var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));

        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    ddl.SelectedIndex = 0;
        //                    //                }
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachineLabor" + (cellCtr - 1);
        //                    //    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
        //                    //    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.Attributes.Add("disabled", "disabled");


        //                    //    DropDownList ddlhide = new DropDownList();
        //                    //    ddlhide.ID = "dynamicddlHideMachineLabor" + (cellCtr - 1);
        //                    //    ddlhide.Style.Add("display", "none");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");
        //                    //    //ddlhide.Style.Add("width", "142px");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null || hdnProcessValues.Value != "")
        //                    //    {

        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
        //                    //                    {
        //                    //                        string txtddlTemp = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        var ddlcheck = ddl.Items.FindByText(txtddlTemp);
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(txtddlTemp).Selected = true;
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlhide.Style.Add("display", "block");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    ddl.Style.Add("display", "none");
        //                    //                    ddlhide.Style.Add("display", "block");
        //                    //                    break;
        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(ddlhide);

        //                    //}
        //                    //else if (row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachine" + (cellCtr - 1);
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.DataSource = Session["MachineIDs"];
        //                    //    ddl.DataTextField = "MachineID";

        //                    //    ddl.DataValueField = "MachineID";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.ID = "txtMachineId" + (cellCtr - 1);
        //                    //    tb.Attributes.Add("autocomplete", "off");
        //                    //    tb.Style.Add("display", "none");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    // style = "display: none;"
        //                    //    //tb.Visible = false;

        //                    //    DropDownList ddlMachide = new DropDownList();
        //                    //    ddlMachide.ID = "ddlHideMachine" + (cellCtr - 1);
        //                    //    //ddlMachide.Style.Add("width", "142px");
        //                    //    ddlMachide.Style.Add("display", "none");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");


        //                    //    TextBox tbhide = new TextBox();
        //                    //    tbhide.ID = "txtHide" + (cellCtr - 1);
        //                    //    tbhide.Style.Add("display", "none");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                if (tempPClist[2].ToString() == "" || tempPClist[2].ToString() == null)
        //                    //                {
        //                    //                    if (tempPClist[3].ToString() == "" || tempPClist[3].ToString() == null)
        //                    //                    {
        //                    //                        if (tempPClist[4].ToString() == "Machine")
        //                    //                        {
        //                    //                            var ddlcheck = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                            if (ddlcheck != null)
        //                    //                            {
        //                    //                                ddl.Style.Add("display", "block");

        //                    //                                if (Session["MachineIDsbyProcess"] != null)
        //                    //                                {
        //                    //                                    ddl.DataSource = null;
        //                    //                                    ddl.DataSource = Session["MachineIDsbyProcess"];
        //                    //                                    ddl.DataTextField = "MachineID";
        //                    //                                    ddl.DataValueField = "MachineID";
        //                    //                                    ddl.DataBind();
        //                    //                                    ListItem match = ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", ""));
        //                    //                                    if (match == null)
        //                    //                                    {
        //                    //                                        ddl.SelectedIndex = 0;
        //                    //                                    }
        //                    //                                    else
        //                    //                                    {
        //                    //                                        ddl.Items.FindByText(tempPClist[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                                    }
        //                    //                                }
        //                    //                                Session["MachineIDsbyProcess"] = null;
        //                    //                            }
        //                    //                        }
        //                    //                        else if (tempPClist[4].ToString() == "Labor")
        //                    //                        {
        //                    //                            tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                            ddl.Style.Add("display", "none");
        //                    //                            tb.Style.Add("display", "block");
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        tbhide.Style.Add("display", "none");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (tempPClist[4].ToString() == "Machine")
        //                    //                    {
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    else if (tempPClist[4].ToString() == "Labor")
        //                    //                    {
        //                    //                        tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        tb.Style.Add("display", "block");
        //                    //                        tb.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    break;
        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(tb);

        //                    //    tCell.Controls.Add(ddlMachide);
        //                    //    tCell.Controls.Add(tbhide);
        //                    //}
        //                    //else
        //                    //{
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.BorderStyle = BorderStyle.None;
        //                    //    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);


        //                    //    tb.Attributes.Add("autocomplete", "off");

        //                    //    tb.Attributes.Add("disabled", "disabled");

        //                    //    #region New code
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                    //        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount)
        //                    //            {
        //                    //                tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion

        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    tCell.Controls.Add(tb);
        //                    //}
        //                    #endregion create form control old

        //                    #region New code  29/04/2019
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);


        //                    tb.Attributes.Add("autocomplete", "off");

        //                    tb.Attributes.Add("disabled", "disabled");

        //                    #region New code
        //                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    {
        //                        var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempPClist.Count; ii++)
        //                        {
        //                            if (ii == rowcount)
        //                            {
        //                                string ifsubcon = tempPClist[2].ToString().Replace("NaN", "");
        //                                string ifturnkey = tempPClist[3].ToString().Replace("NaN", "");

        //                                if (tempPClist[ii].ToString().Replace("NaN", "").Contains("--Select--"))
        //                                {
        //                                    tb.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    tb.Text = tempPClist[ii].ToString().Replace("NaN", "");
        //                                }

        //                                if (ifsubcon != "" || ifturnkey != "")
        //                                {
        //                                    if (ii == 4)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                    else if (ii == 5)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    #endregion

        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tCell.Controls.Add(tb);
        //                    #endregion

        //                    tRow.Cells.Add(tCell);
        //                }
        //            }

        //            if (rowcount % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcount++;
        //        }
        //        // TableMat = Table1;
        //        Session["TablePC"] = TablePC;
        //    }
        //    else
        //    {

        //        TablePC = (Table)Session["TablePC"];

        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {
        //            var tempPclist = hdnProcessValues.Value.ToString().Split(',').ToList();

        //            var TempPclistNew = tempPclist;

        //            //if (CellsCount == 2 && tempPclist.Count <= (DtDynamicProcessFields.Rows.Count + 1))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i)) * (DtDynamicProcessFields.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempSMClist.Count > 6)
        //            ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount == 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount - (i + 1)) * DtDynamicProcessFields.Rows.Count)).ToList();
        //            //else if (CellsCount >= 3 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(((CellsCount) * (DtDynamicProcessFields.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempPclist.Count > (DtDynamicProcessFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();

        //            //if(i == 1)
        //            //    TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count + 1)).ToList();
        //            //else

        //            TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();


        //            int rowcount = 0;
        //            for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    #region Old Code
        //                    //if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("ProcessGrpCode"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlProcess" + (i);
        //                    //    ddl.DataSource = Session["process"];
        //                    //    ddl.DataTextField = "Process_Grp_code";
        //                    //    ddl.DataValueField = "Process_Grp_code";

        //                    //    ddl.DataBind();
        //                    //    // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged3;
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            int CekDataNcolumn = tempPclist.Count / CellsCount;
        //                    //            if (TempPclistNew.Count > CellsCount)
        //                    //            {
        //                    //                string zz = TempPclistNew[ii].ToString();
        //                    //                if (ii == rowcount - 1)
        //                    //                {
        //                    //                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                    if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                    {
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                            var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                            string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                            MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                            //var strddlSelitem = ddl.SelectedItem.Text.ToString().Substring(0, 4);

        //                    //                            //if (strddlSelitem.Contains("-"))
        //                    //                            //{
        //                    //                            //    strddlSelitem = ddl.SelectedItem.Text.ToString().Substring(0, 2);
        //                    //                            //}
        //                    //                            //else
        //                    //                            //{
        //                    //                            //    strddlSelitem = strddlSelitem.ToString().Trim();
        //                    //                            //}
        //                    //                            //MachineIdsbyProcess(strddlSelitem);
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        if (ddlcheck != null)
        //                    //                        {
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                            var strddlSelitem = ddl.SelectedItem.Text.ToString();
        //                    //                            string[] Arrstrdd = strddlSelitem.ToString().Split('-');
        //                    //                            MachineIdsbyProcess(Arrstrdd[0].ToString().Trim());
        //                    //                            //var strddlSelitem = ddl.SelectedItem.Text.ToString().Substring(0, 4);
        //                    //                            //if (strddlSelitem.Contains("-"))
        //                    //                            //{
        //                    //                            //    strddlSelitem = ddl.SelectedItem.Text.ToString().Substring(0, 2);
        //                    //                            //}
        //                    //                            //else
        //                    //                            //{
        //                    //                            //    strddlSelitem = strddlSelitem.ToString().Trim();
        //                    //                            //}
        //                    //                            //MachineIdsbyProcess(strddlSelitem);
        //                    //                        }
        //                    //                        //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}
        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("SubProcess"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubProcess" + (i);


        //                    //    ddl.DataSource = Session["PSGroupwithUOM"];
        //                    //    ddl.DataTextField = "SubProcessName";

        //                    //    ddl.DataValueField = "SubProcessName";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();
        //                    //    // ddl.SelectedIndexChanged += Ddl_SelectedIndexChanged4;
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));

        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(TempPclistNew[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

        //                    //                        }
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    for (int h = 0; h < grdProcessGrphidden.Rows.Count; h++)
        //                    //                    {
        //                    //                        if (grdProcessGrphidden.Rows[h].Cells[1].Text.Contains(TempPclistNew[ii].ToString().Replace("NaN", "")))
        //                    //                        {
        //                    //                        }
        //                    //                        else
        //                    //                        {
        //                    //                            ddl.Items.Remove(grdProcessGrphidden.Rows[h].Cells[1].Text);

        //                    //                        }
        //                    //                    }
        //                    //                    //ddl.Attributes.Add("disabled", "disabled");
        //                    //                }
        //                    //                //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("IfTurnkey-Subvendorname"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlSubvendorname" + (i);
        //                    //    ddl.DataSource = Session["SubVndorData"];
        //                    //    ddl.DataTextField = "SubVndorData";
        //                    //    ddl.DataValueField = "SubVndorData";
        //                    //    //ddl.Attributes.Add("onchange", "alert('aaaa')");
        //                    //    ddl.Attributes.Add("onchange", "SubVendorData(" + i + ")");
        //                    //    ddl.DataBind();
        //                    //    ddl.Items.Insert(0, new ListItem("--Select--", "Select"));
        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            string zz = TempPclistNew[ii].ToString();
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    // ddl.SelectedValue = grdProcessGrphidden.Rows[h].Cells[0].Text;
        //                    //                    string abc = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                    var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                    if (ddlcheck != null)
        //                    //                    {
        //                    //                        ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    //ddl.Attributes.Add("disabled", "disabled");
        //                    //                    ddl.SelectedIndex = 0;
        //                    //                    ddl.Attributes.Add("disabled", "disabled");
        //                    //                }
        //                    //                //ddl.SelectedValue = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    tCell.Controls.Add(ddl);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "").Contains("Machine/Labor"))
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachineLabor" + (i);

        //                    //    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));
        //                    //    ddl.Items.Insert(0, new ListItem("Machine", "Machine"));
        //                    //    ddl.Items.Insert(1, new ListItem("Labor", "Labor"));

        //                    //    DropDownList ddlhide = new DropDownList();
        //                    //    ddlhide.ID = "dynamicddlHideMachineLabor" + (i);
        //                    //    ddlhide.Style.Add("display", "none");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");
        //                    //    //ddlhide.Style.Add("width", "142px");
        //                    //    ddl.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
        //                    //                    {
        //                    //                        var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                        if (ddlcheck != null)
        //                    //                            ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlhide.Style.Add("display", "block");
        //                    //                    }
        //                    //                }
        //                    //                else if (TempPclistNew[2].ToString() != "" || TempPclistNew[2].ToString() != null)
        //                    //                {
        //                    //                    ddl.Style.Add("display", "none");
        //                    //                    ddlhide.Style.Add("display", "block");
        //                    //                }
        //                    //                break;
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    ddlhide.Attributes.Add("disabled", "disabled");

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(ddlhide);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //}

        //                    //else if (DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") == "Machine")
        //                    //{
        //                    //    DropDownList ddl = new DropDownList();
        //                    //    ddl.ID = "dynamicddlMachine" + (i);
        //                    //    //ddl.Style.Add("width", "142px");
        //                    //    ddl.DataSource = Session["MachineIDs"];
        //                    //    ddl.DataTextField = "MachineID";

        //                    //    ddl.DataValueField = "MachineID";
        //                    //    // ddl.da
        //                    //    ddl.DataBind();

        //                    //    //ddl.Items.Insert(0, new ListItem("--Select--", String.Empty));


        //                    //    TextBox tb = new TextBox();
        //                    //    tb.ID = "txtMachineId" + (i);
        //                    //    tb.Style.Add("display", "none");
        //                    //    tb.Attributes.Add("autocomplete", "off");

        //                    //    DropDownList ddlMachide = new DropDownList();
        //                    //    ddlMachide.ID = "ddlHideMachine" + (i);
        //                    //    //ddlMachide.Style.Add("width", "142px");
        //                    //    ddlMachide.Style.Add("display", "none");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");


        //                    //    TextBox tbhide = new TextBox();
        //                    //    tbhide.ID = "txtHide" + (i);
        //                    //    tbhide.Style.Add("display", "none");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    #region retrive data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {

        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == rowcount - 1)
        //                    //            {
        //                    //                if (TempPclistNew[2].ToString() == "" || TempPclistNew[2].ToString() == null)
        //                    //                {
        //                    //                    if (TempPclistNew[3].ToString() == "" || TempPclistNew[3].ToString() == null)
        //                    //                    {
        //                    //                        if (TempPclistNew[4].ToString() == "Machine")
        //                    //                        {
        //                    //                            ddl.Style.Add("display", "block");
        //                    //                            var ddlcheck = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                            if (ddlcheck != null)
        //                    //                            {
        //                    //                                if (Session["MachineIDsbyProcess"] != null)
        //                    //                                {
        //                    //                                    DataTable dtMachineIDsbyProcess = (DataTable)Session["MachineIDsbyProcess"];
        //                    //                                    if (dtMachineIDsbyProcess.Rows.Count > 0)
        //                    //                                    {
        //                    //                                        ddl.DataSource = null;
        //                    //                                        ddl.DataSource = Session["MachineIDsbyProcess"];
        //                    //                                        ddl.DataTextField = "MachineID";
        //                    //                                        ddl.DataValueField = "MachineID";
        //                    //                                        ddl.DataBind();
        //                    //                                    }
        //                    //                                    dtMachineIDsbyProcess = null;
        //                    //                                }
        //                    //                                ListItem match = ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", ""));
        //                    //                                if (match == null)
        //                    //                                {
        //                    //                                    ddl.SelectedIndex = 0;
        //                    //                                }
        //                    //                                else
        //                    //                                {
        //                    //                                    ddl.Items.FindByText(TempPclistNew[ii].ToString().Replace("NaN", "")).Selected = true;
        //                    //                                }
        //                    //                                Session["MachineIDsbyProcess"] = null;
        //                    //                            }
        //                    //                        }
        //                    //                        else if (TempPclistNew[4].ToString() == "Labor")
        //                    //                        {
        //                    //                            tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                            ddl.Style.Add("display", "none");
        //                    //                            tb.Style.Add("display", "block");
        //                    //                        }
        //                    //                    }
        //                    //                    else
        //                    //                    {
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                    }

        //                    //                    break;
        //                    //                }
        //                    //                else
        //                    //                {
        //                    //                    if (TempPclistNew[4].ToString() == "Machine")
        //                    //                    {
        //                    //                        //ddl.SelectedValue = tempPClist[ii].ToString().Replace("NaN", "");
        //                    //                        ddlMachide.Style.Add("display", "block");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //                    }
        //                    //                    else if (TempPclistNew[4].ToString() == "Labor")
        //                    //                    {
        //                    //                        tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                        ddl.Style.Add("display", "none");
        //                    //                        tb.Style.Add("display", "block");
        //                    //                        tb.Attributes.Add("disabled", "disabled");
        //                    //                    }

        //                    //                    break;

        //                    //                }
        //                    //            }

        //                    //        }
        //                    //    }
        //                    //    #endregion retrive data

        //                    //    ddl.Attributes.Add("disabled", "disabled");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    ddlMachide.Attributes.Add("disabled", "disabled");
        //                    //    tbhide.Attributes.Add("disabled", "disabled");

        //                    //    tCell.Controls.Add(ddl);
        //                    //    tCell.Controls.Add(tb);
        //                    //    tCell.Controls.Add(ddlMachide);
        //                    //    tCell.Controls.Add(tbhide);
        //                    //    TablePC.Rows[cellCtr].Cells.Add(tCell);

        //                    //}
        //                    //else
        //                    //{
        //                    //    TextBox tb = new TextBox();
        //                    //    tb.BorderStyle = BorderStyle.None;
        //                    //    tb.ID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    //    tb.Attributes.Add("autocomplete", "off");
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
        //                    //    {
        //                    //        TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                    //        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                    //        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tCell.Controls.Add(tb);
        //                    //        TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    //    }

        //                    //    #region retriev data
        //                    //    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    //    {
        //                    //        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                    //        {
        //                    //            if (ii == (rowcount - 1))
        //                    //            {
        //                    //                tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                    //                break;
        //                    //            }

        //                    //        }

        //                    //    }
        //                    //    #endregion
        //                    //}
        //                    #endregion old code

        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    if (tb.ID.Contains("txtTotalProcessesCost/pcs"))
        //                    {
        //                        TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        TablePC.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    #region retriev data
        //                    if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < TempPclistNew.Count; ii++)
        //                        {
        //                            if (ii == (rowcount - 1))
        //                            {
        //                                string ifsubcon = TempPclistNew[2].ToString().Replace("NaN", "");
        //                                string ifturnkey = TempPclistNew[3].ToString().Replace("NaN", "");

        //                                if (TempPclistNew[ii].ToString().Replace("NaN", "").Contains("--Select--"))
        //                                {
        //                                    tb.Text = "";
        //                                }
        //                                else
        //                                {
        //                                    tb.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
        //                                }

        //                                if (ifsubcon != "" || ifturnkey != "")
        //                                {
        //                                    if (ii == 4)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                    else if (ii == 5)
        //                                    {
        //                                        tb.Text = "";
        //                                    }
        //                                }

        //                                break;
        //                            }

        //                        }

        //                    }
        //                    #endregion
        //                }
        //                rowcount++;
        //            }

        //        }

        //        Session["TablePc"] = TablePC;
        //    }


        //}
        #endregion old 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicProcessDT(int ColumnType)
        private void CreateDynamicProcessDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicProcessFields = new DataTable();
                DtDynamicProcessFields = (DataTable)Session["DtDynamicProcessFields"];

                DataTable DtDynamicProcessCostsDetails = new DataTable();
                DtDynamicProcessCostsDetails = (DataTable)Session["DtDynamicProcessCostsDetails"];

                if (ColumnType == 0)
                {
                    int rowcount = 0;

                    TableRow Hearderrow = new TableRow();

                    TablePC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                            tCell1.Text = "Field Name";
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }
                    foreach (DataRow row in DtDynamicProcessFields.Rows)
                    {
                        TableRow tRow = new TableRow();
                        TablePC.Rows.Add(tRow);
                        for (int cellCtr = 0; cellCtr <= DtDynamicProcessCostsDetails.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("IFTURNKEY-VENDORNAME"))
                                {
                                    tCell.Text = "If Subcon - Subcon Name";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("VENDORRATE"))
                                {
                                    tCell.Text = "Vendor Rate/HR";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TURNKEYPROFIT"))
                                {
                                    tCell.Text = "Turnkey Fees";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                {
                                    tCell.Text = "Total Process Cost/pc";
                                }
                                else if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("EFFICIENCY/PROCESSYIELD(%)"))
                                {
                                    tCell.Text = "Efficiency";
                                }
                                else
                                {
                                    tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                tRow.Cells.Add(tCell);
                                tRow.BackColor = ColorTranslator.FromHtml("#EBEBE4");
                            }
                            else
                            {
                                string FieldName = row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
                                if (FieldName.Contains("StandardRate/HR") || FieldName.Contains("VendorRate") || FieldName.Contains("Baseqty") || FieldName.Contains("DurationperProcessUOM(Sec)") || FieldName.Contains("Efficiency/ProcessYield(%)") || FieldName.Contains("TurnkeyCost/pc") || FieldName.Contains("TurnkeyProfit") || FieldName.Contains("ProcessCost/pc") || FieldName.Contains("TotalProcessesCost/pcs"))
                                {
                                    tCell.Style.Add("text-align", "right");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "left");
                                }

                                #region restore value
                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    var tempPClist = hdnProcessValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempPClist.Count; ii++)
                                    {
                                        if (ii == rowcount)
                                        {
                                            string ifsubcon = tempPClist[2].ToString().Replace("NaN", "");
                                            string ifturnkey = tempPClist[3].ToString().Replace("NaN", "");

                                            if (tempPClist[ii].ToString().Replace("NaN", "").Contains("--Select--"))
                                            {
                                                tCell.Text = "";
                                            }
                                            else
                                            {
                                                tCell.Text = tempPClist[ii].ToString().Replace("NaN", "");
                                            }

                                            if (ifsubcon != "" || ifturnkey != "")
                                            {
                                                if (ii == 4)
                                                {
                                                    tCell.Text = "";
                                                }
                                                else if (ii == 5)
                                                {
                                                    tCell.Text = "";
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalProcessesCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotProcCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                #endregion
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                tRow.Cells.Add(tCell);
                            }
                        }

                        if (rowcount % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcount++;
                    }
                    // TableMat = Table1;
                    Session["TablePC"] = TablePC;
                }
                else
                {

                    TablePC = (Table)Session["TablePC"];

                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {
                        var tempPclist = hdnProcessValues.Value.ToString().Split(',').ToList();

                        var TempPclistNew = tempPclist;

                        TempPclistNew = TempPclistNew.Skip(i * (DtDynamicProcessFields.Rows.Count)).ToList();


                        int rowcount = 0;
                        for (int cellCtr = 0; cellCtr <= DtDynamicProcessFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                TablePC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                #region New code
                                string FieldName = DtDynamicProcessFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("StandardRate/HR") || FieldName.Contains("VendorRate") || FieldName.Contains("Baseqty") || FieldName.Contains("DurationperProcessUOM(Sec)") || FieldName.Contains("Efficiency/ProcessYield(%)") || FieldName.Contains("TurnkeyCost/pc") || FieldName.Contains("TurnkeyProfit") || FieldName.Contains("ProcessCost/pc"))
                                {
                                    tCell.Style.Add("text-align", "right");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "left");
                                }

                                if (FieldName.Contains("TotalProcessesCost/pcs"))
                                {
                                    TablePC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TablePC.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                }
                                else
                                {
                                    TablePC.Rows[cellCtr].Cells.Add(tCell);
                                }
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");

                                if (hdnProcessValues.Value != null && hdnProcessValues.Value != "")
                                {
                                    for (int ii = 0; ii < TempPclistNew.Count; ii++)
                                    {
                                        if (ii == (rowcount - 1))
                                        {
                                            string ifsubcon = TempPclistNew[2].ToString().Replace("NaN", "");
                                            string ifturnkey = TempPclistNew[3].ToString().Replace("NaN", "");

                                            if (TempPclistNew[ii].ToString().Replace("NaN", "").Contains("--Select--"))
                                            {
                                                tCell.Text = "";
                                            }
                                            else
                                            {
                                                tCell.Text = TempPclistNew[ii].ToString().Replace("NaN", "");
                                            }

                                            if (ifsubcon != "" || ifturnkey != "")
                                            {
                                                if (ii == 4)
                                                {
                                                    tCell.Text = "";
                                                }
                                                else if (ii == 5)
                                                {
                                                    tCell.Text = "";
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            rowcount++;
                        }
                    }
                    Session["TablePc"] = TablePC;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        #endregion New 25-06-2019 CreateDynamicProcessDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
        //private void CreateDynamicSubMaterialDT(int ColumnType)
        //{
        //    DataTable DtDynamicSubMaterialsFields = new DataTable();
        //    DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicSubMaterialsDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicSubMaterialsDetails == null)
        //        //    DtDynamicSubMaterialsDetails = new DataTable();
        //        //if (DtDynamicSubMaterialsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableSMC.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";
        //        //        tCell1.Width = 280;
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicSubMaterialsFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableSMC.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)"); ;
        //        //                tCell.Width = 280;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                // Data Store and Retrieve

        //        //                //if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //        //                //{
        //        //                //    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //        //                //    for (int ii = 1; ii < tempSMClist.Count; ii++)
        //        //                //    {

        //        //                //    }
        //        //                //}


        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //        //                //{
        //        //                //    tb.Attributes.Add("disabled", "disabled");
        //        //                //}
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                //TextBox tb = new TextBox();
        //        //                //tb.BorderStyle = BorderStyle.None;
        //        //                //tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                //tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //        //                //{
        //        //                //    tb.Attributes.Add("disabled", "disabled");
        //        //                //}

        //        //                tCell.Controls.Add(tb);
        //        //                //if (rowcount == 0)
        //        //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[0].ToString();
        //        //                //else if (rowcount == 1)
        //        //                //    tCell.Text = DtMaterialsDetails.Rows[cellCtr - 1].ItemArray[1].ToString();
        //        //                tCell.Width = 280;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableSMC.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";
        //        //        tCell1.Width = 280;
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableSMC.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
        //        //                lb.Width = 280;
        //        //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                //if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //        //                //{
        //        //                //    tb.Attributes.Add("disabled", "disabled");
        //        //                //}
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tb.Attributes.Add("autocomplete", "off");


        //        //                // Data Store and Retrieve
        //        //                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }

        //        //                tCell.Width = 280;
        //        //                tCell.Controls.Add(tb);
        //        //                TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion
        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TableSMC.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            //else
        //            //    tCell1.Text = "Material Cost";
        //            tCell1.Width = 280;
        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        // Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            TableSMC.Rows.Add(tRow);

        //            for (int i = 0; i <= 1; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    lb.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
        //                    lb.Width = 280;
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                    //if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                    //{
        //                    //    tb.Attributes.Add("disabled", "disabled");
        //                    //}
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Style.Add("text-transform", "uppercase");

        //                    // Data Store and Retrieve
        //                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //                    {
        //                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempSMClist.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {
        //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }

        //                    tCell.Width = 280;
        //                    tCell.Controls.Add(tb);
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //            }
        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }
        //        Session["TableSMC"] = TableSMC;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableSMC = (Table)Session["TableSMC"];

        //        int CellsCount = ColumnType;

        //        //int tempcount = 1;
        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //           // tempcount++;

        //            var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

        //            var TempSMClistNew = tempSMClist;

        //            //if (CellsCount == 2 && tempSMClist.Count <= (DtDynamicSubMaterialsFields.Rows.Count+1))
        //            //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i)) * (DtDynamicSubMaterialsFields.Rows.Count+1))).ToList();
        //            ////else if (CellsCount ==  && tempSMClist.Count > 6)
        //            ////    TempSMClistNew = TempSMClistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount == 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count +1) && CellsCount != (i + 2))
        //            //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount - (i + 1)) * DtDynamicSubMaterialsFields.Rows.Count)).ToList();
        //            //else if (CellsCount >= 3 && tempSMClist.Count > (DtDynamicSubMaterialsFields.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    TempSMClistNew = TempSMClistNew.Skip(((CellsCount) * (DtDynamicSubMaterialsFields.Rows.Count))).ToList();

        //                TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();



        //            for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    tCell.Width = 280;
        //                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    if (tb.ID.Contains("Sub-Mat/T&JCost/pcs") || tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                    {
        //                        tb.Attributes.Add("disabled", "disabled");
        //                    }

        //                    if (tb.ID.Contains("TotalSub-Mat/T&JCost/pcs"))
        //                    {
        //                        TableSMC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    }
        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        tCell.Width = 280;
        //                        TableSMC.Rows[cellCtr].Cells.Add(tCell);
        //                    }

        //                    // Data Retrieve and assign from Storage
        //                    if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
        //                    {


        //                        for (int ii = 0; ii < TempSMClistNew.Count; ii++)
        //                        {

        //                            if (ii == ( cellCtr-1))
        //                            {

        //                                tb.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }


        //                    //if (hdngrdSubMatCost.Rows.Count > 1)
        //                    //{

        //                    //    for (int i1 = 0; i1 < hdngrdSubMatCost.Rows.Count; i1++)
        //                    //    {
        //                    //        for (int j = 0; j < hdngrdSubMatCost.Rows[i1].Cells.Count; j++)
        //                    //        {
        //                    //            if (j == cellCtr)
        //                    //            {
        //                    //                tb.Text = hdngrdSubMatCost.Rows[i1].Cells[j].Text;
        //                    //                break;
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //}
        //                }
        //            }
        //        }

        //        Session["TableSMC"] = TableSMC;
        //    }

        //  //  var Ss = divhdnSMC.InnerHtml;






        //}
        #endregion old 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)
        private void CreateDynamicSubMaterialDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicSubMaterialsFields = new DataTable();
                DtDynamicSubMaterialsFields = (DataTable)Session["DtDynamicSubMaterialsFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableSMC.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        else
                        {
                            #region amortize condition
                            DataTable dtTollAmot = new DataTable();
                            DataTable dtMachineAmor = new DataTable();
                            if (Session["VndMachineAmortize"] != null)
                            {
                                dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                            }

                            if (Session["VndToolAmortize"] != null)
                            {
                                dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                if (dtTollAmot.Rows.Count > 0 && (cellCtr - 1) < dtTollAmot.Rows.Count)
                                {
                                    tCell1.Text = "Tool Amortize";
                                }
                                else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > (cellCtr - 1) && (HdnIsUseMachineAmor.Value != "" || HdnIsUseMachineAmor.Value != "0"))
                                {
                                    tCell1.Text = "Machine Amortize";
                                }
                            }
                            else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && (HdnIsUseMachineAmor.Value != "" || HdnIsUseMachineAmor.Value != "0"))
                            {
                                if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count >= (cellCtr - 1))
                                {
                                    tCell1.Text = "Machine Amortize";
                                }
                            }
                            #endregion
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableSMC.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                tCell.Text = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc").Replace("(pcs)", "(pc)");
                                tCell.Width = 150;
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                if (FieldName.Contains("Sub-Mat/T&JDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                // Data Store and Retrieve
                                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                {
                                    var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalSub-Mat/T&JCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotSubMatCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["TableSMC"] = TableSMC;
                }
                else
                {
                    //  int Rowscount = -1;
                    TableSMC = (Table)Session["TableSMC"];

                    int CellsCount = ColumnType;

                    //int tempcount = 1;
                    for (int i = 1; i < CellsCount; i++)
                    {

                        // tempcount++;

                        var tempSMClist = hdnSMCTableValues.Value.ToString().Split(',').ToList();

                        var TempSMClistNew = tempSMClist;

                        TempSMClistNew = TempSMClistNew.Skip(i * (DtDynamicSubMaterialsFields.Rows.Count)).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamicSubMaterialsFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                #region amortize condition
                                DataTable dtTollAmot = new DataTable();
                                DataTable dtMachineAmor = new DataTable();
                                if (Session["VndMachineAmortize"] != null)
                                {
                                    dtMachineAmor = (DataTable)Session["VndMachineAmortize"];
                                }

                                if (Session["VndToolAmortize"] != null)
                                {
                                    dtTollAmot = (DataTable)Session["VndToolAmortize"];
                                    if (dtTollAmot.Rows.Count > 0 && i < dtTollAmot.Rows.Count)
                                    {
                                        tCell.Text = "Tool Amortize";
                                    }
                                    else if (dtMachineAmor.Rows.Count > 0 && (dtTollAmot.Rows.Count + dtMachineAmor.Rows.Count) > i && (HdnIsUseMachineAmor.Value != "" || HdnIsUseMachineAmor.Value != "0") )
                                    {
                                        tCell.Text = "Machine Amortize";
                                    }
                                }
                                else if (Session["VndToolAmortize"] == null && Session["VndMachineAmortize"] != null && (HdnIsUseMachineAmor.Value != "" || HdnIsUseMachineAmor.Value != "0"))
                                {
                                    if (dtMachineAmor.Rows.Count > 0 && dtMachineAmor.Rows.Count > i)
                                    {
                                        tCell.Text = "Machine Amortize";
                                    }
                                }
                                #endregion

                                TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                tCell.Width = 150;
                            }
                            else
                            {
                                string FieldName = DtDynamicSubMaterialsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("Sub-Mat/T&JDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                if (FieldName.Contains("TotalSub-Mat/T&JCost/pcs"))
                                {
                                    TableSMC.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TableSMC.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");
                                }
                                else
                                {
                                    TableSMC.Rows[cellCtr].Cells.Add(tCell);
                                }

                                // Data Retrieve and assign from Storage
                                if (hdnSMCTableValues.Value != null && hdnSMCTableValues.Value != "")
                                {

                                    for (int ii = 0; ii < TempSMClistNew.Count; ii++)
                                    {

                                        if (ii == (cellCtr - 1))
                                        {

                                            tCell.Text = TempSMClistNew[ii].ToString().Replace("NaN", "");
                                            tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Session["TableSMC"] = TableSMC;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        #endregion New 25-06-2019 CreateDynamicSubMaterialDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        //private void CreateDynamicOthersCostDT(int ColumnType)
        //{
        //    DataTable DtDynamicOtherCostsFields = new DataTable();
        //    DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicOtherCostsDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicOtherCostsDetails == null)
        //        //    DtDynamicOtherCostsDetails = new DataTable();
        //        //if (DtDynamicOtherCostsDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableOthers.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicOtherCostsFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableOthers.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                tCell.Text = row.ItemArray[0].ToString().Replace("/pcs", "/pc");
        //        //                tCell.Width = 240;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //        //                //{

        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                //}

        //        //                // Data Store and Retrieve
        //        //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }


        //        //                tCell.Controls.Add(tb);
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableOthers.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableOthers.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 1; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
        //        //                lb.Width = 240;
        //        //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //        //                //{

        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                //}



        //        //                // Data Store and Retrieve
        //        //                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //        //                {
        //        //                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempSMClist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }

        //        //                    }
        //        //                }


        //        //                tCell.Controls.Add(tb);
        //        //                TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //        }
        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion

        //        int rowcountnew = 0;

        //        TableRow Hearderrow = new TableRow();

        //        TableOthers.Rows.Add(Hearderrow);
        //        for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //        {
        //            TableCell tCell1 = new TableCell();
        //            Label lb1 = new Label();
        //            tCell1.Controls.Add(lb1);
        //            if (cellCtr == 0)
        //                tCell1.Text = "Field Name";
        //            //else
        //            //    tCell1.Text = "Material Cost";

        //            Hearderrow.Cells.Add(tCell1);
        //            Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //            Hearderrow.ForeColor = Color.White;
        //        }

        //        // Table1 = (Table)Session["Table"];
        //        for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //        {
        //            TableRow tRow = new TableRow();
        //            TableOthers.Rows.Add(tRow);

        //            for (int i = 0; i <= 1; i++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (i == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    lb.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
        //                    lb.Width = 240;
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    //if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //                    //{
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    tb.Attributes.Add("disabled", "disabled");
        //                    //}



        //                    // Data Store and Retrieve
        //                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //                    {
        //                        var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //                        for (int ii = 0; ii < tempSMClist.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {
        //                                tb.Text = tempSMClist[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }


        //                    tCell.Controls.Add(tb);
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //            }
        //            if (rowcountnew % 2 == 0)
        //            {
        //                tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                tRow.BackColor = Color.White;
        //            }
        //            else
        //            {
        //                //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //            }
        //            rowcountnew++;
        //        }

        //        Session["TableOthers"] = TableOthers;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableOthers = (Table)Session["TableOthers"];


        //        int CellsCount = ColumnType;

        //        for (int i = 1; i < CellsCount; i++)
        //        {

        //            var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

        //            var tempOtherlistNew = tempOtherlist;

        //            //if (CellsCount == 2 && tempOtherlist.Count <= (DtDynamicOtherCostsFields.Rows.Count + 1))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i)) * (DtDynamicOtherCostsFields.Rows.Count + 1))).ToList();
        //            ////else if (CellsCount ==  && tempOtherlist.Count > 6)
        //            ////    tempOtherlistNew = tempOtherlistNew.Skip((CellsCount - i)  * 5).ToList();
        //            //else if (CellsCount == 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount - (i + 1)) * DtDynamicOtherCostsFields.Rows.Count)).ToList();
        //            //else if (CellsCount >= 3 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount == (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(((CellsCount) * (DtDynamicOtherCostsFields.Rows.Count))).ToList();
        //            //else if (i >= 1 && tempOtherlist.Count > (DtDynamicOtherCostsFields.Rows.Count + 1) && CellsCount != (i + 2))
        //            //    tempOtherlistNew = tempOtherlistNew.Skip(i * (DtDynamicOtherCostsFields.Rows.Count)).ToList();


        //            tempOtherlistNew = tempOtherlist.Skip(i * DtDynamicOtherCostsFields.Rows.Count).ToList();

        //            for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell = new TableCell();
        //                if (cellCtr == 0)
        //                {
        //                    Label lb = new Label();
        //                    tCell.Controls.Add(lb);
        //                    // lb.Text = "Material Cost";
        //                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                }
        //                else
        //                {
        //                    TextBox tb = new TextBox();
        //                    tb.BorderStyle = BorderStyle.None;
        //                    tb.ID = "txt" + DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
        //                    tb.Attributes.Add("autocomplete", "off");
        //                    tb.Style.Add("text-transform", "uppercase");
        //                    if (tb.ID.Contains("TotalOtherItemCost/pcs"))
        //                    {
        //                        TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());

        //                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("width", "-webkit-fill-available");
        //                        ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1].Controls[0])).Style.Add("text-align", "center");

        //                    }

        //                    else
        //                    {
        //                        tCell.Controls.Add(tb);
        //                        TableOthers.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    tb.Attributes.Add("disabled", "disabled");


        //                    // Data Retrieve and assign from Storage
        //                    if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
        //                    {
        //                        for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
        //                        {
        //                            if (ii == (cellCtr - 1))
        //                            {

        //                                tb.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
        //                                break;
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        Session["TableOthers"] = TableOthers;
        //    }

        //}
        #endregion old 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        private void CreateDynamicOthersCostDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicOtherCostsFields = new DataTable();
                DtDynamicOtherCostsFields = (DataTable)Session["DtDynamicOtherCostsFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;
                    TableRow Hearderrow = new TableRow();

                    TableOthers.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableOthers.Rows.Add(tRow);

                        for (int i = 0; i <= 1; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                if (hdnLayoutScreen.Value == "Layout7")
                                {
                                    tCell.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/UOM");
                                }
                                else
                                {
                                    tCell.Text = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs", "/pc");
                                }
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (ColumnType);
                                if (FieldName.Contains("ItemsDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }

                                // Data Store and Retrieve
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    var tempSMClist = hdnOtherValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempSMClist.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempSMClist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                else
                                {
                                    if (FieldName.Contains("TotalOtherItemCost/pcs"))
                                    {
                                        if (hdnMassRevision.Value != "")
                                        {
                                            tCell.Text = HdnMAssTotOthCost.Value;
                                        }
                                        else
                                        {
                                            tCell.Text = "";
                                        }
                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["TableOthers"] = TableOthers;
                }
                else
                {
                    TableOthers = (Table)Session["TableOthers"];
                    int CellsCount = ColumnType;

                    for (int i = 1; i < CellsCount; i++)
                    {

                        var tempOtherlist = hdnOtherValues.Value.ToString().Split(',').ToList();

                        var tempOtherlistNew = tempOtherlist;

                        tempOtherlistNew = tempOtherlist.Skip(i * DtDynamicOtherCostsFields.Rows.Count).ToList();

                        for (int cellCtr = 0; cellCtr <= DtDynamicOtherCostsFields.Rows.Count; cellCtr++)
                        {
                            TableCell tCell = new TableCell();
                            if (cellCtr == 0)
                            {
                                TableOthers.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else
                            {
                                string FieldName = DtDynamicOtherCostsFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i);
                                if (FieldName.Contains("ItemsDescription"))
                                {
                                    tCell.Style.Add("text-align", "left");
                                }
                                else
                                {
                                    tCell.Style.Add("text-align", "right");
                                }
                                if (FieldName.Contains("TotalOtherItemCost/pcs"))
                                {
                                    TableOthers.Rows[cellCtr].Cells[1].Attributes.Add("colspan", CellsCount.ToString());
                                    ((System.Web.UI.WebControls.WebControl)(TableOthers.Rows[cellCtr].Cells[1])).Style.Add("text-align", "center");

                                }
                                else
                                {
                                    TableOthers.Rows[cellCtr].Cells.Add(tCell);
                                }
                                // Data Retrieve and assign from Storage
                                if (hdnOtherValues.Value != null && hdnOtherValues.Value != "")
                                {
                                    for (int ii = 0; ii < tempOtherlistNew.Count; ii++)
                                    {
                                        if (ii == (cellCtr - 1))
                                        {

                                            tCell.Text = tempOtherlistNew[ii].ToString().Replace("NaN", "");
                                            break;
                                        }

                                    }
                                }
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                        }
                    }
                    Session["TableOthers"] = TableOthers;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }
        #endregion New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)


        #region old 25-06-2019 CreateDynamicUnitDT(int ColumnType)
        //private void CreateDynamicUnitDT(int ColumnType)
        //{
        //    DataTable DtDynamicUnitFields = new DataTable();
        //    DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

        //    if (ColumnType == 0)
        //    {
        //        #region old code (condition with DtDynamicUnitDetails never used)
        //        //int rowcount = 0;
        //        //if (DtDynamicUnitDetails == null)
        //        //    DtDynamicUnitDetails = new DataTable();
        //        //if (DtDynamicUnitDetails.Rows.Count > 0)
        //        //{
        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableUnit.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //            tCell1.Text = "Field Name";
        //        //        //else
        //        //        //    tCell1.Text = "Material Cost";

        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    foreach (DataRow row in DtDynamicUnitFields.Rows)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableUnit.Rows.Add(tRow);
        //        //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (cellCtr == 0)
        //        //            {
        //        //                Label lb = new Label();
        //        //                tCell.Controls.Add(lb);
        //        //                if (row.ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //        //                {
        //        //                    tCell.Text = "Total Process Cost/pc";
        //        //                }
        //        //                else
        //        //                {
        //        //                    tCell.Text = row.ItemArray[0].ToString().Replace("/pcs -", "/pc");
        //        //                }
        //        //                tCell.Width = 240;
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //            else
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Style.Add("text-transform", "uppercase");
        //        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tCell.Controls.Add(tb);
        //        //                tRow.Cells.Add(tCell);
        //        //            }
        //        //        }

        //        //        if (rowcount % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcount++;
        //        //    }
        //        //}

        //        ////start from here
        //        //else
        //        //{
        //        //    int rowcountnew = 0;

        //        //    TableRow Hearderrow = new TableRow();

        //        //    TableUnit.Rows.Add(Hearderrow);
        //        //    for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
        //        //    {
        //        //        TableCell tCell1 = new TableCell();
        //        //        Label lb1 = new Label();
        //        //        tCell1.Controls.Add(lb1);
        //        //        if (cellCtr == 0)
        //        //        {
        //        //            tCell1.Text = "Field Name";
        //        //        }
        //        //        else if (cellCtr == 2)
        //        //        {
        //        //            tCell1.Text = "Profit (%)";
        //        //        }
        //        //        else if (cellCtr == 3)
        //        //        {
        //        //            tCell1.Text = "Discount (%)";
        //        //        }
        //        //        else if (cellCtr == 4)
        //        //        {
        //        //            tCell1.Text = "Final Quote Price/pc";
        //        //        }
        //        //        else if (cellCtr == 5)
        //        //        {
        //        //            tCell1.Text = "Net Profit/Discount";
        //        //        }
        //        //        Hearderrow.Cells.Add(tCell1);
        //        //        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //        Hearderrow.ForeColor = Color.White;
        //        //    }

        //        //    // Table1 = (Table)Session["Table"];
        //        //    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        //    {
        //        //        TableRow tRow = new TableRow();
        //        //        TableUnit.Rows.Add(tRow);

        //        //        for (int i = 0; i <= 5; i++)
        //        //        {
        //        //            TableCell tCell = new TableCell();
        //        //            if (i == 0)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("Profit(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("Discount(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("FinalQuotePrice/pcs"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else
        //        //                {
        //        //                    Label lb = new Label();
        //        //                    tCell.Controls.Add(lb);
        //        //                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //        //                    {
        //        //                        lb.Text = "Total Process Cost/pc";
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
        //        //                    }
        //        //                    lb.Width = 240;
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //            else if (i == 1)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("Profit(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("Discount(%)"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else if (a.Contains("FinalQuotePrice/pcs"))
        //        //                {
        //        //                    break;
        //        //                }
        //        //                else
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("GrandTotalCost/pcs"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }

        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //            else if (i == 2)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("TotalProcessesCost/pcs-"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtProfit(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    tb.Attributes.Add("disabled", "disabled");


        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("Profit(%)"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //                else
        //        //                {
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                }

        //        //            }
        //        //            else if (i == 3)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("TotalProcessesCost/pcs-"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtDiscount(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    tb.Attributes.Add("disabled", "disabled");

        //        //                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                    {
        //        //                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //        //                        {
        //        //                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                            if (zz.Contains("Discount(%)"))
        //        //                            {
        //        //                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                                for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                                {
        //        //                                    if (ii == (z - 1))
        //        //                                    {
        //        //                                        tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                                        break;
        //        //                                    }
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //                else
        //        //                {
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                }
        //        //            }
        //        //            else if (i == 4)
        //        //            {
        //        //                TextBox tb = new TextBox();
        //        //                tb.BorderStyle = BorderStyle.None;
        //        //                tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
        //        //                tCell.Controls.Add(tb);
        //        //                tb.Attributes.Add("autocomplete", "off");
        //        //                tb.Style.Add("text-transform", "uppercase");
        //        //                tb.Attributes.Add("disabled", "disabled");
        //        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //        //                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //        //                {
        //        //                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //        //                    for (int ii = 0; ii < tempunitlist.Count; ii++)
        //        //                    {
        //        //                        if (ii == (cellCtr - 1))
        //        //                        {
        //        //                            tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //        //                            break;
        //        //                        }
        //        //                    }
        //        //                }

        //        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //            }
        //        //            else if (i == 5)
        //        //            {
        //        //                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //        //                if (a.Contains("GrandTotalCost/pcs"))
        //        //                {
        //        //                    TextBox tb = new TextBox();
        //        //                    tb.BorderStyle = BorderStyle.None;
        //        //                    tb.ID = "txtNetProfit(%)0";
        //        //                    tCell.Controls.Add(tb);
        //        //                    tb.Attributes.Add("autocomplete", "off");
        //        //                    tb.Attributes.Add("disabled", "disabled");
        //        //                    tb.Style.Add("text-transform", "uppercase");
        //        //                    tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);

        //        //                }
        //        //                else
        //        //                {
        //        //                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //        //                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //        //                }
        //        //            }
        //        //        }

        //        //        if (rowcountnew % 2 == 0)
        //        //        {
        //        //            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //        //            tRow.BackColor = Color.White;
        //        //        }
        //        //        else
        //        //        {
        //        //            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //        //            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //        //        }
        //        //        rowcountnew++;
        //        //    }
        //        //}
        //        #endregion

        //        int rowcountnew = 0;

        //            TableRow Hearderrow = new TableRow();

        //            TableUnit.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                {
        //                    tCell1.Text = "Field Name";
        //                }
        //                else if (cellCtr == 2)
        //                {
        //                    tCell1.Text = "Profit (%)";
        //                }
        //                else if (cellCtr == 3)
        //                {
        //                    tCell1.Text = "Discount (%)";
        //                }
        //                else if (cellCtr == 4)
        //                {
        //                    tCell1.Text = "Final Quote Price/pc";
        //                }
        //                else if (cellCtr == 5)
        //                {
        //                    tCell1.Text = "Net Profit/Discount";
        //                }
        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            // Table1 = (Table)Session["Table"];
        //            for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableUnit.Rows.Add(tRow);

        //                for (int i = 0; i <= 5; i++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (i == 0)
        //                    {
        //                        string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                        if (a.Contains("Profit(%)"))
        //                        {
        //                            break;
        //                        }
        //                        else if (a.Contains("Discount(%)"))
        //                        {
        //                            break;
        //                        }
        //                        else if (a.Contains("FinalQuotePrice/pcs"))
        //                        {
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            Label lb = new Label();
        //                            tCell.Controls.Add(lb);
        //                            if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
        //                            {
        //                                lb.Text = "Total Process Cost/pc";
        //                            }
        //                            else
        //                            {
        //                                lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
        //                            }
        //                            lb.Width = 240;
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                    }
        //                    else if (i == 1)
        //                    {
        //                        string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                        if (a.Contains("Profit(%)"))
        //                        {
        //                            break;
        //                        }
        //                        else if (a.Contains("Discount(%)"))
        //                        {
        //                            break;
        //                        }
        //                        else if (a.Contains("FinalQuotePrice/pcs"))
        //                        {
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);
        //                            tCell.Controls.Add(tb);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Style.Add("text-transform", "uppercase");
        //                            tb.Attributes.Add("disabled", "disabled");
        //                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //                            if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                            {
        //                                for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                                {
        //                                    string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                    if (zz.Contains("GrandTotalCost/pcs"))
        //                                    {
        //                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                        {
        //                                            if (ii == (z - 1))
        //                                            {
        //                                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                                break;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                    }
        //                    else if (i == 2)
        //                    {
        //                        string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                        if (a.Contains("TotalProcessesCost/pcs-"))
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txtProfit(%)0";
        //                            tCell.Controls.Add(tb);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Style.Add("text-transform", "uppercase");
        //                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                            tb.Attributes.Add("disabled", "disabled");


        //                            if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                            {
        //                                for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                                {
        //                                    string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                    if (zz.Contains("Profit(%)"))
        //                                    {
        //                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                        {
        //                                            if (ii == (z - 1))
        //                                            {
        //                                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                                break;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                        else
        //                        {
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                            tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                        }

        //                    }
        //                    else if (i == 3)
        //                    {
        //                        string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                        if (a.Contains("TotalProcessesCost/pcs-"))
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txtDiscount(%)0";
        //                            tCell.Controls.Add(tb);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Style.Add("text-transform", "uppercase");
        //                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                            tb.Attributes.Add("disabled", "disabled");

        //                            if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                            {
        //                                for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
        //                                {
        //                                    string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                                    if (zz.Contains("Discount(%)"))
        //                                    {
        //                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                                        {
        //                                            if (ii == (z - 1))
        //                                            {
        //                                                tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                                break;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                        else
        //                        {
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                            tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                        }
        //                    }
        //                    else if (i == 4)
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");
        //                        tb.Style.Add("text-transform", "uppercase");
        //                        tb.Attributes.Add("disabled", "disabled");
        //                        tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");

        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                            for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                            {
        //                                if (ii == (cellCtr - 1))
        //                                {
        //                                    tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                    break;
        //                                }
        //                            }
        //                        }

        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else if (i == 5)
        //                    {
        //                        string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
        //                        if (a.Contains("GrandTotalCost/pcs"))
        //                        {
        //                            TextBox tb = new TextBox();
        //                            tb.BorderStyle = BorderStyle.None;
        //                            tb.ID = "txtNetProfit(%)0";
        //                            tCell.Controls.Add(tb);
        //                            tb.Attributes.Add("autocomplete", "off");
        //                            tb.Attributes.Add("disabled", "disabled");
        //                            tb.Style.Add("text-transform", "uppercase");
        //                            tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);

        //                        }
        //                        else
        //                        {
        //                            tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
        //                            TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                        }
        //                    }
        //                }

        //                if (rowcountnew % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcountnew++;
        //            }
        //        // TableMat = Table1;
        //        Session["TableUnit"] = TableUnit;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableUnit = (Table)Session["TableUnit"];

        //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell = new TableCell();
        //            if (cellCtr == 0)
        //            {
        //                Label lb = new Label();
        //                tCell.Controls.Add(lb);
        //                // lb.Text = "Material Cost";
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //            else
        //            {
        //                TextBox tb = new TextBox();
        //                tb.BorderStyle = BorderStyle.None;
        //                tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                tb.Attributes.Add("autocomplete", "off");
        //                tb.Style.Add("text-transform", "uppercase");
        //                tb.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
        //                tb.Attributes.Add("disabled", "disabled");
        //                tCell.Controls.Add(tb);
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //        }


        //        Session["TableUnit"] = TableUnit;
        //    }

        //}
        #endregion old 25-06-2019 CreateDynamicUnitDT(int ColumnType)
        #region New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        private void CreateDynamicUnitDT(int ColumnType)
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableUnit.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 5; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        else if (cellCtr == 2)
                        {
                            tCell1.Text = "Profit (%)";
                        }
                        else if (cellCtr == 3)
                        {
                            tCell1.Text = "Discount (%)";
                        }
                        else if (cellCtr == 4)
                        {
                            if (hdnLayoutScreen.Value == "Layout7")
                            {
                                tCell1.Text = "Final Quote Price/UOM";
                            }
                            else
                            {
                                tCell1.Text = "Final Quote Price/pc";
                            }
                        }
                        else if (cellCtr == 5)
                        {
                            tCell1.Text = "Net Profit/Discount";
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableUnit.Rows.Add(tRow);

                        for (int i = 0; i <= 5; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 1)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Profit(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("TotalProcessesCost/pcs-"))
                                {
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Discount(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.BackColor = ColorTranslator.FromHtml("#ECEBE4");
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 4)
                            {
                                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                {
                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
                                    {
                                        if (ii == (4))
                                        {
                                            tCell.Text = tempunitlist[7].ToString().Replace("NaN", "");
                                            break;
                                        }
                                        else if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }
                                    }
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                            else if (i == 5)
                            {
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }

                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }

                    Session["TableUnit"] = TableUnit;
                }
                else
                {
                    //  int Rowscount = -1;
                    TableUnit = (Table)Session["TableUnit"];

                    for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableCell tCell = new TableCell();
                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
                    }
                    Session["TableUnit"] = TableUnit;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        private void CreateDynamicUnitDTTmShimano(int ColumnType)
        {
            try
            {

                DataTable DtDynamicUnitFields = new DataTable();
                DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

                if (ColumnType == 0)
                {
                    int rowcountnew = 0;

                    TableRow Hearderrow = new TableRow();

                    TableUnit.Rows.Add(Hearderrow);
                    for (int cellCtr = 0; cellCtr <= 4; cellCtr++)
                    {
                        TableCell tCell1 = new TableCell();
                        Label lb1 = new Label();
                        tCell1.Controls.Add(lb1);
                        if (cellCtr == 0)
                        {
                            tCell1.Text = "Field Name";
                        }
                        else if (cellCtr == 1)
                        {
                            tCell1.Text = "e-MET Cost";
                        }
                        else if (cellCtr == 2)
                        {
                            tCell1.Text = "GA%";
                        }
                        else if (cellCtr == 3)
                        {
                            tCell1.Text = "Profit%";
                        }
                        else if (cellCtr == 4)
                        {
                            if (hdnLayoutScreen.Value == "Layout7")
                            {
                                tCell1.Text = "Final Quote Price/UOM";
                            }
                            else
                            {
                                tCell1.Text = "Final Quote Price/pc";
                            }
                        }
                        Hearderrow.Cells.Add(tCell1);
                        Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
                        Hearderrow.ForeColor = Color.White;
                    }

                    // Table1 = (Table)Session["Table"];
                    for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                    {
                        TableRow tRow = new TableRow();
                        TableUnit.Rows.Add(tRow);

                        for (int i = 0; i <= 4; i++)
                        {
                            TableCell tCell = new TableCell();
                            if (i == 0)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("TOTALPROCESSESCOST/PC"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Total Process Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Total Process Cost/pc";
                                        }
                                    }
                                    else if (DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace(" ", "").ToUpper().Contains("GRANDTOTALCOST/PCS"))
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = "Grand Total Cost/UOM";
                                        }
                                        else
                                        {
                                            tCell.Text = "Grand Total Cost/pc";
                                        }
                                    }
                                    else
                                    {
                                        if (hdnLayoutScreen.Value == "Layout7")
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/UOM");
                                        }
                                        else
                                        {
                                            tCell.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Replace("/pcs -", "/pc");
                                        }
                                    }
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 1)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("Profit(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("Discount(%)"))
                                {
                                    break;
                                }
                                else if (a.Contains("FinalQuotePrice/pcs"))
                                {
                                    break;
                                }
                                else
                                {
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                        for (int ii = 0; ii < tempunitlist.Count; ii++)
                                        {
                                            if (ii == (cellCtr - 1))
                                            {
                                                tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                            }
                            else if (i == 2)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    tCell.Text = GetGA().ToString();
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                            }
                            else if (i == 3)
                            {
                                string a = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                if (a.Contains("GrandTotalCost/pcs"))
                                {
                                    if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                    {
                                        for (int z = 1; z <= DtDynamicUnitFields.Rows.Count; z++)
                                        {
                                            string zz = DtDynamicUnitFields.Rows[z - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "");
                                            if (zz.Contains("Profit(%)"))
                                            {
                                                var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                                for (int ii = 0; ii < tempunitlist.Count; ii++)
                                                {
                                                    if (ii == (z - 1))
                                                    {
                                                        tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    tCell.Style.Add("text-align", "right");
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                }
                                else
                                {
                                    TableUnit.Rows[cellCtr].Cells.Add(tCell);
                                    tCell.Width = 150;
                                    tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                }
                            }
                            else if (i == 4)
                            {
                                string a = "txtFinalQuotePrice/pcs" + (cellCtr - 1);
                                if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
                                {
                                    var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

                                    for (int ii = 0; ii < tempunitlist.Count; ii++)
                                    {
                                        if (a.Contains("txtFinalQuotePrice/pcs4"))
                                        {
                                            if (ii >= 7)
                                            {
                                                tCell.Text = tempunitlist[7].ToString().Replace("NaN", "");
                                                break;
                                            }
                                        }
                                        else if (ii == (cellCtr - 1))
                                        {
                                            tCell.Text = tempunitlist[ii].ToString().Replace("NaN", "");
                                            break;
                                        }
                                    }
                                }
                                tCell.Style.Add("text-align", "right");
                                tCell.Width = 150;
                                tCell.BackColor = ColorTranslator.FromHtml("#ffffff");
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }



                        if (rowcountnew % 2 == 0)
                        {
                            tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
                            tRow.BackColor = Color.White;
                        }
                        else
                        {
                            //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
                            tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
                        }
                        rowcountnew++;
                    }
                    Session["TableUnit"] = TableUnit;
                }
                else
                {
                    Session["TableUnit"] = TableUnit;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

        }
        #endregion New 25-06-2019 CreateDynamicUnitDT(int ColumnType)




        //old table version CreateDynamicUnitDT
        //private void CreateDynamicUnitDT(int ColumnType)
        //{

        //    if (ColumnType == 0)
        //    {
        //        int rowcount = 0;
        //        if (DtDynamicUnitDetails == null)
        //            DtDynamicUnitDetails = new DataTable();
        //        if (DtDynamicUnitDetails.Rows.Count > 0)
        //        {
        //            TableRow Hearderrow = new TableRow();

        //            TableUnit.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                //else
        //                //    tCell1.Text = "Material Cost";

        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            foreach (DataRow row in DtDynamicUnitFields.Rows)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableUnit.Rows.Add(tRow);
        //                for (int cellCtr = 0; cellCtr <= DtDynamicUnitDetails.Rows.Count; cellCtr++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (cellCtr == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        tCell.Text = row.ItemArray[0].ToString();
        //                        tRow.Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + row.ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                        tb.Attributes.Add("autocomplete", "off");


        //                        tCell.Controls.Add(tb);
        //                        tRow.Cells.Add(tCell);
        //                    }
        //                }

        //                if (rowcount % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcount++;
        //            }
        //        }
        //        else
        //        {
        //            int rowcountnew = 0;

        //            TableRow Hearderrow = new TableRow();

        //            TableUnit.Rows.Add(Hearderrow);
        //            for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
        //            {
        //                TableCell tCell1 = new TableCell();
        //                Label lb1 = new Label();
        //                tCell1.Controls.Add(lb1);
        //                if (cellCtr == 0)
        //                    tCell1.Text = "Field Name";
        //                //else
        //                //    tCell1.Text = "Material Cost";

        //                Hearderrow.Cells.Add(tCell1);
        //                Hearderrow.BackColor = ColorTranslator.FromHtml("#1a2e4c");
        //                Hearderrow.ForeColor = Color.White;
        //            }

        //            // Table1 = (Table)Session["Table"];
        //            for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //            {
        //                TableRow tRow = new TableRow();
        //                TableUnit.Rows.Add(tRow);

        //                for (int i = 0; i <= 1; i++)
        //                {
        //                    TableCell tCell = new TableCell();
        //                    if (i == 0)
        //                    {
        //                        Label lb = new Label();
        //                        tCell.Controls.Add(lb);
        //                        lb.Text = DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString();
        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                    else
        //                    {
        //                        TextBox tb = new TextBox();
        //                        tb.BorderStyle = BorderStyle.None;
        //                        tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (i - 1);


        //                        if (hdnUnitValues.Value != null && hdnUnitValues.Value != "")
        //                        {
        //                            var tempunitlist = hdnUnitValues.Value.ToString().Split(',').ToList();

        //                            for (int ii = 0; ii < tempunitlist.Count; ii++)
        //                            {
        //                                if (ii == (cellCtr - 1))
        //                                {
        //                                    tb.Text = tempunitlist[ii].ToString().Replace("NaN", "");
        //                                    break;
        //                                }

        //                            }
        //                        }


        //                        tCell.Controls.Add(tb);
        //                        tb.Attributes.Add("autocomplete", "off");


        //                            tb.Attributes.Add("disabled", "disabled");


        //                        TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //                    }
        //                }
        //                if (rowcountnew % 2 == 0)
        //                {
        //                    tRow.ForeColor = ColorTranslator.FromHtml("#1a2e4c");
        //                    tRow.BackColor = Color.White;
        //                }
        //                else
        //                {
        //                    //tRow.ForeColor = ColorTranslator.FromHtml("#333333");
        //                    tRow.BackColor = ColorTranslator.FromHtml("#ffffff");
        //                }
        //                rowcountnew++;
        //            }
        //        }
        //        // TableMat = Table1;
        //        Session["TableUnit"] = TableUnit;
        //    }
        //    else
        //    {
        //        //  int Rowscount = -1;
        //        TableUnit = (Table)Session["TableUnit"];

        //        for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
        //        {
        //            TableCell tCell = new TableCell();
        //            if (cellCtr == 0)
        //            {
        //                Label lb = new Label();
        //                tCell.Controls.Add(lb);
        //                // lb.Text = "Material Cost";
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //            else
        //            {
        //                TextBox tb = new TextBox();
        //                tb.BorderStyle = BorderStyle.None;
        //                tb.ID = "txt" + DtDynamicUnitFields.Rows[cellCtr - 1].ItemArray[0].ToString().Trim().TrimEnd().TrimStart().Replace(" ", "") + (cellCtr - 1);
        //                tb.Attributes.Add("autocomplete", "off");
        //                tCell.Controls.Add(tb);
        //                TableUnit.Rows[cellCtr].Cells.Add(tCell);
        //            }
        //        }


        //        Session["TableUnit"] = TableUnit;
        //    }

        //}

        private void GetProcessDetailsbyQuoteDetails(string ProcessGrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + txtprocs.Text.ToString().ToUpper() + "' and DELFLAG = 0 ";
                da4 = new SqlDataAdapter(str4, MDMCon);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                {
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
                }

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select distinct ProcessGrpCode from TPROCESGROUP_SUBPROCESS Where ProcessGrpCode = '" + ProcessGrp + "' ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtProGrp);

                //DtDynamicProcessCostsDetails = dtProGrp;
                Session["DtDynamicProcessCostsDetails"] = dtProGrp;
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

        private void GetProcessDetailsbyQuoteDetailsWithNoGroup()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                DataTable dtProGrp = new DataTable();
                DataTable dtVendorRate = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter();
                string str = "Select CONCAT(ProcessGrpCode , ' - ', ProcessGrpDescription) as ProcessGrpCode, SubProcessName,ProcessUomDescription,ProcessUOM from TPROCESGROUP_SUBPROCESS ";
                da = new SqlDataAdapter(str, MDMCon);
                da.Fill(dtProGrp);

                grdProcessGrphidden.DataSource = dtProGrp;
                grdProcessGrphidden.DataBind();
                Session["PSGroupwithUOM"] = grdProcessGrphidden.DataSource;
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
            //DtDynamicProcessCostsDetails = dtProGrp;

        }

        // Till this used


        protected void GetData(string reqno)
        {

            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();

                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;


                strGetData = "select CONVERT(VARCHAR(10), S.RequestDate, 103) as RequestDate,S.QuoteNo,V.Description,v.Crcy,vp.PICName,vp.PICemail from tVendor_New as V inner join TVENDORPIC as VP" +
                              " on vp.VendorCode=v.Vendor inner join " + TransDB.ToString() + "TQuoteDetails as S on S.VendorCode1=v.Vendor where S.QuoteNo='" + reqno + "' ";
                da = new SqlDataAdapter(strGetData, MDMCon);
                da.Fill(dtget);

                if (dtget.Rows.Count > 0)
                {
                    grdVendrDet.DataSource = dtget;
                    grdVendrDet.DataBind();

                    hdnQuoteNo.Value = dtget.Rows[0].ItemArray[1].ToString();

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

        private void GetSHMNPICDetails(string userdet)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                // string str = "select  sp.PIC1Name as UseNam,sp.PIC1Email as UseEmail from Usr Inner join TSMNProductPIC sp on Usr.UseID = '" + userdet.Trim() + "'";

                string QNO = Request.QueryString["Number"];
                string str1 = "";
                if (QNO.Substring(QNO.Length - 2, 2) == "GP")
                {
                    str1 = "select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product from TSMNProductPIC pp inner join " + TransDB.ToString() + "TQuotedetails TQ on pp.Userid = Tq.CreatedBy where QuoteNo='" + Session["Qno"].ToString() + "' ";
                }
                else
                {
                    str1 = "select distinct PP.PIC1Name as PICName,pp.PIC1Email as PICEmail,pp.Product from TSMNProductPIC pp inner join " + TransDB.ToString() + "TQuotedetails TQ on pp.Product = TQ.Product and pp.Userid = Tq.CreatedBy and PP.Product = TQ.Product where QuoteNo='" + Session["Qno"].ToString() + "' ";
                }

                da = new SqlDataAdapter(str1, MDMCon);
                da.Fill(dtdate);
                if (dtdate.Rows.Count > 0)
                {
                    txtsmnpic.Text = dtdate.Rows[0]["PICName"].ToString();
                    txtemail.Text = dtdate.Rows[0]["PICEmail"].ToString();
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


        protected void process(string QuNo)
        {
            string UserId = Session["VndId"].ToString();
            DropDownList ddlProcess = new DropDownList();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "select distinct CONCAT( tp.Process_Grp_code, ' - ', tp.Process_Grp_Description) as Process_Grp_code from TPROCESGROUP_LIST TP inner join TPROCESGROUP_SUBPROCESS TPS on Tp.Process_Grp_code = tps.ProcessGrpCode";
                da = new SqlDataAdapter(str, MDMCon);
                Result = new DataTable();
                da.Fill(Result);

                ddlProcess.DataSource = Result;
                ddlProcess.DataTextField = "Process_Grp_code";
                ddlProcess.DataValueField = "Process_Grp_code";
                ddlProcess.DataBind();


                Session["process"] = ddlProcess.DataSource;

                DataTable Result1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter();
                string str1 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as Machine, CAST(ROUND(TVM.SMNStdrateHr,2) AS DECIMAL(12,2))as 'SMNStdrateHr' ,TVM.FollowStdRate as FollowStdRate,TVM.Currency as Currency from TVENDORMACHNLIST TVM where TVM.VendorCode = '" + UserId + "' ";
                da1 = new SqlDataAdapter(str1, MDMCon);
                Result1 = new DataTable();
                da1.Fill(Result1);

                grdMachinelisthidden.DataSource = Result1;
                grdMachinelisthidden.DataBind();
                Session["MachineListGrd"] = grdMachinelisthidden.DataSource;


                DataTable Result3 = new DataTable();
                SqlDataAdapter da3 = new SqlDataAdapter();
                string str3 = "select CAST(ROUND(StdLabourRateHr,2) AS DECIMAL(12,2))as 'StdLabourRateHr',FollowStdRate,Currency from TVENDORLABRCOST TVC Where TVC.Vendorcode = '" + UserId + "'";
                da3 = new SqlDataAdapter(str3, MDMCon);
                Result3 = new DataTable();
                da3.Fill(Result3);

                grdLaborlisthidden.DataSource = Result3;
                grdLaborlisthidden.DataBind();
                Session["LaborListGrd"] = grdLaborlisthidden.DataSource;

                DropDownList ddlMachine = new DropDownList();

                DataTable Result2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter();
                string str2 = "select CONCAT(TVM.MachineID, ' - ', TVM.MachineDescription) as MachineID from TVENDORMACHNLIST TVM inner join " + TransDB.ToString() + "TQuoteDetails TQ on TVM.VendorCode = TQ.VendorCode1 Where TQ.QuoteNo = '" + QuNo + "'";
                da2 = new SqlDataAdapter(str2, MDMCon);
                Result2 = new DataTable();
                da2.Fill(Result2);

                ddlMachine.DataSource = Result2;
                ddlMachine.DataBind();
                Session["MachineIDs"] = ddlMachine.DataSource;

                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + txtprocs.Text.ToString().ToUpper() + "'";
                da4 = new SqlDataAdapter(str4, MDMCon);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
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

        protected string GetDataMacTypNtoonageRetrive(string MachineID)
        {
            string TypeAndToonage = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable DtDataMacVnd = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string Vndplant = Session["Vndplant"].ToString();
                string str = " select MachineType,Tonnage from TVENDORMACHNLIST where Plant='" + Vndplant + "' and MachineID= '" + MachineID + "' and DELFLAG = 0  ";
                da = new SqlDataAdapter(str, MDMCon);
                DtDataMacVnd = new DataTable();
                da.Fill(DtDataMacVnd);

                if (DtDataMacVnd.Rows.Count > 0)
                {
                    TypeAndToonage = DtDataMacVnd.Rows[0]["MachineType"].ToString() + "," + DtDataMacVnd.Rows[0]["Tonnage"].ToString();
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
            return TypeAndToonage;
        }

        private void RetrieveAllCostDetails(string QuNo)
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {

                string Vndplant = Session["Vndplant"].ToString();
                EmetCon.Open();

                DataTable dtget = new DataTable();

                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();


                #region Other Cost

                string strGetData = "";

                strGetData = "SELECT QuoteNo,ProcessGroup,UPPER(ItemsDescription) as 'ItemsDescription',[OtherItemCost/pcs] as 'OtherItemCost/pcs',CAST(ROUND([TotalOtherItemCost/pcs],5) AS DECIMAL(12,5)) as 'TotalOtherItemCost/pcs',RowId FROM TOtherCostDetails Where  QuoteNo = '" + QuNo + "' order by RowId asc ";
                da = new SqlDataAdapter(strGetData, EmetCon);
                da.Fill(dtget);

                StringBuilder sb = new StringBuilder();

                string hdnvaltempNew = "";
                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtIDesc = dtget.Rows[i].ItemArray[2].ToString();
                        var txtOtherItemCost = dtget.Rows[i].ItemArray[3].ToString();
                        var txtTotalCost = dtget.Rows[i].ItemArray[4].ToString();

                        sb.Append(txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                        hdnvaltempNew = (txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                    }
                    //hdnSMCTableValues.Value = hdnvaltempNew.ToString();
                    hdnOtherValues.Value = sb.ToString();
                }

                #endregion Other Cost

                #region SubMat Cost

                string strsub = "";
                dtget = new DataTable();
                strsub = @"SELECT A.QuoteNo,A.ProcessGroup,
                        --UPPER([Sub-Mat/T&JDescription]) as 'Sub-Mat/T&JDescription',
                        case 
                            when 
	                        (select count(*) MachineDescription from "+ DbMasterName + @".dbo.TVENDORMACHNLIST M
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
                        A.[Sub-Mat/T&JCost] as 'Sub-Mat/T&JCost',A.[Consumption(pcs)] as 'Consumption(pcs)',A.[Sub-Mat/T&JCost/pcs] as 'Sub-Mat/T&JCost/pcs',
                        CAST(ROUND(A.[TotalSub-Mat/T&JCost/pcs],5) AS DECIMAL(12,5)) as 'TotalSub-Mat/T&JCost/pcs',A.RowId 
                        FROM TSMCCostDetails A 
                        join TQuoteDetails TQ on A.QuoteNo = TQ.QuoteNo
                        Where  A.QuoteNo = '" + QuNo + "' order by A.RowId asc ";
                da = new SqlDataAdapter(strsub, EmetCon);
                da.Fill(dtget);

                StringBuilder sbSub = new StringBuilder();

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtsubDesc = dtget.Rows[i].ItemArray[2].ToString();
                        var txtsubcost = dtget.Rows[i].ItemArray[3].ToString();
                        var txtConsumption = dtget.Rows[i].ItemArray[4].ToString();
                        var txtsubcostPC = dtget.Rows[i].ItemArray[5].ToString();
                        var txtTotalCostPC = dtget.Rows[i].ItemArray[6].ToString();

                        sbSub.Append(txtsubDesc + "," + txtsubcost + "," + txtConsumption + "," + txtsubcostPC + "," + txtTotalCostPC + ",");
                    }
                    hdnSMCTableValues.Value = sbSub.ToString();
                }

                #endregion SubMat Cost

                #region get sub vendor desc
                string DBNameFinal = EMETModule.GetDbMastername();
                #endregion get sub vendor desc

                #region Process Cost

                string strProcess = "";
                dtget = new DataTable();
                strProcess = "SELECT PCD.[QuoteNo],PCD.[ProcessGroup],case when PCD.[ProcessGrpCode] = 'Select' then '' else PCD.[ProcessGrpCode] end as 'ProcessGrpCode',case when PCD.[SubProcess] = 'Select' then '' else PCD.[SubProcess] end as 'SubProcess',PCD.[IfTurnkey-VendorName]," +
                             "( CONCAT (RTRIM(PCD.[TurnKeySubVnd]),' - ',(select distinct Description from[" + DBNameFinal + "].[dbo].[tVendor_New] where Vendor = PCD.[TurnKeySubVnd])) ) as TurnKeySubVnd," +
                             "PCD.[Machine/Labor],PCD.[Machine],CAST(ROUND(PCD.[StandardRate/HR],2) AS DECIMAL(12,2))as 'StandardRate/HR',CAST(ROUND(PCD.VendorRate,2) AS DECIMAL(12,2))as 'VendorRate'," +
                             "PCD.[ProcessUOM],PCD.[Baseqty],PCD.[DurationperProcessUOM(Sec)],PCD.[Efficiency/ProcessYield(%)]," +
                             "PCD.[TurnKeyCost],PCD.[TurnKeyProfit],PCD.[ProcessCost/pc],CAST(ROUND(PCD.[TotalProcessesCost/pcs],5) AS DECIMAL(12,5)) as [TotalProcessesCost/pcs],PCD.[RowId]," +
                             "PCD.[UpdatedBy],PCD.[UpdatedOn] FROM [TProcessCostDetails] PCD  Where PCD.[QuoteNo] = '" + QuNo + "' order by RowId asc ";
                da = new SqlDataAdapter(strProcess, EmetCon);
                da.Fill(dtget);

                StringBuilder sbProcess = new StringBuilder();

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        var txtProc = dtget.Rows[i]["ProcessGrpCode"].ToString();
                        var txtsubProc = dtget.Rows[i]["SubProcess"].ToString();
                        var txtturnkey = dtget.Rows[i]["IfTurnkey-VendorName"].ToString();
                        var txtTurnKeySubVnd = dtget.Rows[i]["TurnKeySubVnd"].ToString();
                        if (txtTurnKeySubVnd.Trim() == "-")
                        {
                            txtTurnKeySubVnd = "";
                        }
                        var txtML = dtget.Rows[i]["Machine/Labor"].ToString();
                        var txtMachine = dtget.Rows[i]["Machine"].ToString();

                        var txtstanRate = dtget.Rows[i]["StandardRate/HR"].ToString();
                        var txtvendorRate = dtget.Rows[i]["VendorRate"].ToString();
                        var txtProcUOM = dtget.Rows[i]["ProcessUOM"].ToString();
                        var txtBaseQty = dtget.Rows[i]["Baseqty"].ToString();
                        var txtDPUOM = dtget.Rows[i]["DurationperProcessUOM(Sec)"].ToString();

                        var txtProcYeild = dtget.Rows[i]["Efficiency/ProcessYield(%)"].ToString();
                        var txtTurnKeyCost = dtget.Rows[i]["TurnKeyCost"].ToString();
                        var txtTurnKeyProfit = dtget.Rows[i]["TurnKeyProfit"].ToString();
                        var txtProcCost = dtget.Rows[i]["ProcessCost/pc"].ToString();
                        var txtProcTCost = dtget.Rows[i]["TotalProcessesCost/pcs"].ToString();

                        #region get value for proces uom is struk/min 
                        if (txtProcUOM.ToString().Contains("STROKES/MIN"))
                        {
                            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
                            MDMCon.Open();
                            try
                            {
                                string[] ArProcess_Grp_code = txtProc.ToString().Split('-');
                                string Process_Grp_code = ArProcess_Grp_code[0].ToString().Trim();
                                string[] ArMachineType = txtMachine.ToString().Split('-');
                                string MachineType = "";
                                int tonnage = 0;

                                string TypeAndToonage = GetDataMacTypNtoonageRetrive(ArMachineType[0].ToString());
                                string[] ArrTypeAndToonage = TypeAndToonage.Split(',');
                                if (ArrTypeAndToonage.Count() == 2)
                                {
                                    MachineType = ArrTypeAndToonage[0].ToString().Trim();
                                    tonnage = int.Parse(ArrTypeAndToonage[1].ToString());
                                }


                                string str1 = " select Plant,Process_Grp_code,MachineType,Tonnage_From,Tonnage_To,Strokes_min,Efficiency from TPROCESSGRPVSSTROKES_MIN  " +
                                             " where Plant='" + Vndplant + "' and Process_Grp_code='" + Process_Grp_code + "' " +
                                             " and MachineType='" + MachineType + "' and (" + tonnage + " between Tonnage_From and Tonnage_To) ";
                                cmd = new SqlCommand(str1, MDMCon);
                                reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    txtProcUOM = txtProcUOM + '-' + reader["Strokes_min"].ToString();
                                }
                                reader.Dispose();
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
                        #endregion

                        //var txtProc = dtget.Rows[i].ItemArray[2].ToString();
                        //var txtsubProc = dtget.Rows[i].ItemArray[3].ToString();
                        //var txtturnkey = dtget.Rows[i].ItemArray[4].ToString();
                        //var txtML = dtget.Rows[i].ItemArray[5].ToString();
                        //var txtMachine = dtget.Rows[i].ItemArray[6].ToString();

                        //var txtstanRate = dtget.Rows[i].ItemArray[7].ToString();
                        //var txtvendorRate = dtget.Rows[i].ItemArray[8].ToString();
                        //var txtProcUOM = dtget.Rows[i].ItemArray[9].ToString();
                        //var txtBaseQty = dtget.Rows[i].ItemArray[10].ToString();
                        //var txtDPUOM = dtget.Rows[i].ItemArray[11].ToString();

                        //var txtProcYeild = dtget.Rows[i].ItemArray[12].ToString();
                        //var txtProcCost = dtget.Rows[i].ItemArray[13].ToString();
                        //var txtProcTCost = dtget.Rows[i].ItemArray[14].ToString();

                        sbProcess.Append(txtProc + "," + txtsubProc + "," + txtturnkey + "," + txtTurnKeySubVnd + "," + txtML + "," + txtMachine + "," + txtstanRate + "," + txtvendorRate + "," + txtProcUOM + "," + txtBaseQty + "," + txtDPUOM + "," + txtProcYeild + "," + txtTurnKeyCost + "," + txtTurnKeyProfit + "," + txtProcCost + "," + txtProcTCost + ",");
                    }
                    hdnProcessValues.Value = sbProcess.ToString();
                }

                #endregion SubMat Cost

                #region MC Cost
                dtget = new DataTable();
                string strMCIM = "";

                strMCIM = @"SELECT TMC.QuoteNo,TMC.ProcessGroup,UPPER(MaterialSAPCode) as 'MaterialSAPCode',UPPER(MaterialDescription) as 'MaterialDescription',[RawMaterialCost/kg] as 'RawMaterialCost/kg',
                        CAST(ROUND([TotalRawMaterialCost/g],5) AS DECIMAL(12,4)) as 'TotalRawMaterialCost/g',[PartNetUnitWeight(g)] as '[PartNetUnitWeight(g)]',[~~DiameterID(mm)] as '~~DiameterID(mm)',[~~DiameterOD(mm)] as '~~DiameterOD(mm)', 
                        [~~Thickness(mm)] as '~~Thickness(mm)',[~~Width(mm)] as '~~Width(mm)', [~~Pitch(mm)] as '[~~Pitch(mm)]', [~MaterialDensity] as '~MaterialDensity',[~RunnerWeight/shot(g)] as '~RunnerWeight/shot(g)',
                        [~RunnerRatio/pcs(%)] as '~RunnerRatio/pcs(%)', [~RecycleMaterialRatio(%)] as '~RecycleMaterialRatio(%)', Cavity , [MaterialYield/MeltingLoss(%)] as 'MaterialYield/MeltingLoss(%)', 
                        CAST(ROUND([MaterialGrossWeight/pc(g)],5) AS DECIMAL(12,4)) as 'MaterialGrossWeight/pc(g)', [MaterialScrapWeight(g)] as 'MaterialScrapWeight(g)', [ScrapLossAllowance(%)] as 'ScrapLossAllowance(%)',
                        [ScrapPrice/kg] as 'ScrapPrice/kg', [ScrapRebate/pcs] as 'ScrapRebate/pcs', [MaterialCost/pcs] as 'MaterialCost/pcs', CAST(ROUND([TotalMaterialCost/pcs],5) AS DECIMAL(12,5)) as 'TotalMaterialCost/pcs',
                        RowId
                        ,isnull(
						case 
						when ((ISNULL(RawMaterialCostUOM,'') = '')  or (ISNULL(RawMaterialCostUOM,'') like '%' + 'SELECT UOM' + '%')) then (select distinct top 1 isnull([Component unit],'') as RawMaterialCostUOM from " + DbMasterName + @".dbo.TBOMLISTnew TB where tb.Plant = TQ.plant and TB.[Component Material] = TMC.MaterialSAPCode )
						else RawMaterialCostUOM
						end
						,'') as RawMaterialCostUOM
						
						FROM TMCCostDetails TMC
						join TQuoteDetails TQ on TMC.QuoteNo = TQ.QuoteNo
                        Where  TMC.QuoteNo = N'" + QuNo + "' order by RowId asc ";
                da = new SqlDataAdapter(strMCIM, EmetCon);
                da.Fill(dtget);

                StringBuilder sbMCIM = new StringBuilder();


                string strMCode = "";
                string strMDesc = "";
                string strRawCost = "";
                string strTotalRawCost = "";
                string strPartUnitW = "";
                string strDiaID = "";
                string strDiaOD = "";
                string strThick = "";
                string strWidth = "";
                string strPitch = "";
                string strMDensity = "";
                string strRunnerWeight = "";
                string strRunnerRatio = "";
                string strRecycle = "";
                string strCavity = "";
                string strMLoss = "";
                string strMCrossWeight = "";
                string strMScrapWeight = "";
                string strScrapLoss = "";
                string strScrapPrice = "";
                string strScrapRebate = "";
                string strMCostpcs = "";
                string strTotalcostpcs = "";

                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        strMCode = dtget.Rows[i].ItemArray[2].ToString();
                        strMDesc = dtget.Rows[i].ItemArray[3].ToString();
                        if (dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper() == "KG")
                        {
                            strRawCost = dtget.Rows[i].ItemArray[4].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                            strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString() + " / " + " G";
                        }
                        else
                        {
                            strRawCost = dtget.Rows[i].ItemArray[4].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                            strTotalRawCost = dtget.Rows[i].ItemArray[5].ToString() + " / " + dtget.Rows[i]["RawMaterialCostUOM"].ToString().ToUpper();
                        }
                        strPartUnitW = dtget.Rows[i].ItemArray[6].ToString();

                        #region IM / LAYOUT1
                        //if (txtprocs.Text == "IM")
                        if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT1")
                        {
                            strRunnerWeight = dtget.Rows[i].ItemArray[13].ToString();
                            strRunnerRatio = dtget.Rows[i].ItemArray[14].ToString();
                            strRecycle = dtget.Rows[i].ItemArray[15].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strRunnerWeight + "," + strRunnerRatio + "," + strRecycle + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strRunnerWeight + "," + strRunnerRatio + "," + strRecycle + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion IM

                        #region CA or SPRT / layout3 / layout6
                        //else if (txtprocs.Text == "CA" || txtprocs.Text == "SPR")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT3" || hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT6")
                        {
                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion CA or SPR

                        #region ST / layout 5
                        //else if (txtprocs.Text == "ST")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT5")
                        {
                            strThick = dtget.Rows[i].ItemArray[9].ToString();
                            strWidth = dtget.Rows[i].ItemArray[10].ToString();
                            strPitch = dtget.Rows[i].ItemArray[11].ToString();
                            strMDensity = dtget.Rows[i].ItemArray[12].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();


                            strMScrapWeight = dtget.Rows[i].ItemArray[19].ToString();
                            strScrapLoss = dtget.Rows[i].ItemArray[20].ToString();
                            strScrapPrice = dtget.Rows[i].ItemArray[21].ToString();
                            strScrapRebate = dtget.Rows[i].ItemArray[22].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();


                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strThick + "," + strWidth + "," + strPitch + "," + strMDensity + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMScrapWeight + "," + strScrapLoss + "," + strScrapPrice + "," + strScrapRebate + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                        }
                        #endregion ST

                        #region MS / layout 4
                        //else if (txtprocs.Text == "MS")
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT4")
                        {
                            //strDiaID = dtget.Rows[i].ItemArray[7].ToString();
                            //strDiaOD = dtget.Rows[i].ItemArray[8].ToString();
                            //strWidth = dtget.Rows[i].ItemArray[10].ToString();

                            strCavity = dtget.Rows[i].ItemArray[16].ToString();
                            strMLoss = dtget.Rows[i].ItemArray[17].ToString();
                            strMCrossWeight = dtget.Rows[i].ItemArray[18].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion MS

                        #region layout 2
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT2")
                        {
                            strCavity = dtget.Rows[i].ItemArray[16].ToString();

                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strPartUnitW + "," + strCavity + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion MS

                        #region layout 7
                        else if (hdnLayoutScreen.Value.ToString().ToUpper() == "LAYOUT7")
                        {
                            strMCostpcs = dtget.Rows[i].ItemArray[23].ToString();
                            strTotalcostpcs = dtget.Rows[i].ItemArray[24].ToString();

                            //sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strTotalRawCost + "," + strPartUnitW + "," + strDiaID + "," + strDiaOD + "," + strWidth + "," + strCavity + "," + strMLoss + "," + strMCrossWeight + "," + strMCostpcs + "," + strTotalcostpcs + ",");
                            sbMCIM.Append(strMCode + "," + strMDesc + "," + strRawCost + "," + strMCostpcs + "," + strTotalcostpcs + ",");

                        }
                        #endregion layout 7

                    }
                    hdnMCTableValues.Value = sbMCIM.ToString();

                }

                #endregion MC Cost

                #region Unit Cost
                dtget = new DataTable();
                string strUnitData = "";

                strUnitData = @"select CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost',
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                            Profit,Discount,
                            CAST(ROUND(FinalQuotePrice,5) AS DECIMAL(12,5)) as 'FinalQuotePrice',
                            CommentByVendor from TQuoteDetails  Where QuoteNo = '" + QuNo + "'";
                da = new SqlDataAdapter(strUnitData, EmetCon);
                da.Fill(dtget);

                StringBuilder sbUnit = new StringBuilder();

                //string hdnUnittemp = "";
                if (dtget.Rows.Count > 0)
                {
                    for (var i = 0; i < dtget.Rows.Count; i++)
                    {
                        //var txtTMCost = dtget.Rows[i].ItemArray[0].ToString();
                        //var txtTPCost = dtget.Rows[i].ItemArray[1].ToString();
                        //var txtTSCost = dtget.Rows[i].ItemArray[2].ToString();
                        //var txtTOCost = dtget.Rows[i].ItemArray[3].ToString();
                        //var txtGrantCost = dtget.Rows[i].ItemArray[4].ToString();
                        //var txtProfit = dtget.Rows[i].ItemArray[5].ToString();
                        //var txtDiscount = dtget.Rows[i].ItemArray[6].ToString();
                        //var txtfinalCost = dtget.Rows[i].ItemArray[7].ToString();
                        //TxtComntByVendor.Text = dtget.Rows[i].ItemArray[8].ToString();

                        var txtTMCost = dtget.Rows[i]["TotalMaterialCost"].ToString();
                        var txtTPCost = dtget.Rows[i]["TotalProcessCost"].ToString();
                        var txtTSCost = dtget.Rows[i]["TotalSubMaterialCost"].ToString();
                        var txtTOCost = dtget.Rows[i]["TotalOtheritemsCost"].ToString();
                        var txtGrantCost = dtget.Rows[i]["GrandTotalCost"].ToString();
                        var txtProfit = dtget.Rows[i]["Profit"].ToString();
                        var txtDiscount = dtget.Rows[i]["Discount"].ToString();
                        var txtfinalCost = dtget.Rows[i]["FinalQuotePrice"].ToString();
                        TxtComntByVendor.Text = dtget.Rows[i]["CommentByVendor"].ToString();

                        hdnTMatCost.Value = txtTMCost;
                        hdnTProCost.Value = txtTPCost;
                        hdnTSumMatCost.Value = txtTSCost;
                        hdnTOtherCost.Value = txtTOCost;
                        hdnTGTotal.Value = txtGrantCost;
                        hdnTFinalQPrice.Value = txtfinalCost;

                        sbUnit.Append(txtTMCost + "," + txtTPCost + "," + txtTSCost + "," + txtTOCost + "," + txtGrantCost + "," + txtProfit + "," + txtDiscount + "," + txtfinalCost + ",");
                        //hdnUnittemp = (txtIDesc + "," + txtOtherItemCost + "," + txtTotalCost + ",");
                    }
                    //hdnSMCTableValues.Value = hdnvaltempNew.ToString();
                    hdnUnitValues.Value = sbUnit.ToString();
                }

                #endregion Other Cost

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
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Close", "javascript:window.open('','_self').close();", true);
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
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Close", "javascript:window.open('','_self').close();", true);
                }
                else
                {
                    countdown.Text = seconds.ToString();
                }
            }
        }

        protected void GVBomListBefEffdate_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GdvPrevQuote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LbQuoteNo = e.Row.FindControl("LbQuoteNo") as Label;
                    if (LbQuoteNo != null)
                    {
                        string url = "QQPReview.aspx?Number=" + LbQuoteNo.Text;
                        LbQuoteNo.Attributes.Add("onclick", "openInNewTab2('" + url + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvPrevQuote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LinktoRedirect")
            {
                Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
            }
        }

        protected void GdvQuoreReqRevice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LbQuoteNo = e.Row.FindControl("LbQuoteNo") as Label;
                    if (LbQuoteNo != null)
                    {
                        string url = "QQPReview.aspx?Number=" + LbQuoteNo.Text;
                        LbQuoteNo.Attributes.Add("onclick", "openInNewTab2('" + url + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GdvQuoreReqRevice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "LinktoRedirect")
            {
                Response.Redirect("QuoteCostPlan.aspx?Number=" + ((System.Web.UI.WebControls.LinkButton)e.CommandSource).Text.ToString());
            }
        }
        

        protected void BtnPreview_Click(object sender, EventArgs e)
        {
            if (LbFlName.Text != "No File")
            {
                string folderPath = Server.MapPath("~/FileVendorAttachmant/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string FileExtension = "pdf";
                string filename = LbFlNameOri.Text;
                string[] Arrfilename = filename.Split('.');
                int c = Arrfilename.Count();
                if (c > 0)
                {
                    FileExtension = Arrfilename[c - 1].ToString();
                }
                string PathAndFileName = folderPath + filename;

                if (filename != "")
                {
                    FileInfo fileCheck = new FileInfo(PathAndFileName);
                    if (fileCheck.Exists) //check file exsit or not  
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/" + FileExtension.Replace(".", "") + "";
                        Response.AddHeader("content-disposition", "attachment;filename=" + filename);     // to open file prompt Box open or Save file         
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.TransmitFile(PathAndFileName);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File deleted !')", true);
                        LbFlName.Text = "No File";
                        UnitCostDataStore();
                        OthersCostDataStore();
                        subMatCostDataStore();
                        ProcessCostDataStore();
                        MCCostDataStore();
                    }
                }
            }
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Close", "javascript:window.open('','_self').close();", true);
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