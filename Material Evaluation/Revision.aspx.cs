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
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Material_Evaluation
{
    public partial class Revision : System.Web.UI.Page
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

        public string SrvUserid ;
        public string Srvpassword ;
        public string Srvdomain ;
        public string Srvpath ;
        public string SrvURL;
        //email

        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        bool IsAth;
        bool sendingmail;
        string errmsg;
        string DbMasterName = "";
        string DbTransName = "";
        string GA = "";
        string PlantDesc = "";
        string SMNPICSubmDept = "";

        CheckBox chkAllRefHd;
        CheckBox chkAllMatRef;
        CheckBox chkAllProcRef;
        CheckBox chkAllSubMatRef;
        CheckBox chkAllOthRef;

        CheckBox chkAllToolAmortizeRef;
        CheckBox chkAllMachineAmortizeRef;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //Response.Redirect("Home.aspx");
                if (Session["userID"] == null || Session["UserName"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        TxtExtraUrl.Text = Properties.Settings.Default.ExtraUrl.ToString();

                        Session["VndToolAmortize"] = null;
                        Session["VndMachineAmortize"] = null;

                        Session["FlAttachment"] = null;
                        Session["IsMacAmorAdded"] = null;
                        Session["OldMachineAmorList"] = null;
                        Session["TotColProcOld"] = null;

                        string UI = Session["userID"].ToString();
                        string FN = "EMET_RevisionOfMET";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            clearSeason();
                            userId1 = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            nameC = sname;
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;

                            //HttpContext.Current.Session["MaterialType"] = null;
                            //HttpContext.Current.Session["plant_status"] = null;
                            //HttpContext.Current.Session["proctype"] = null;
                            //HttpContext.Current.Session["splproctype"] = null;
                            HttpContext.Current.Session["prod_code"] = null;
                            HttpContext.Current.Session["matlclass"] = null;
                            Session["RowGvTemp"] = null;

                            lbluser1.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            TxtPlant.Text = Session["EPlant"].ToString();
                            txtReqDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

                            //GetDdlReason();
                            //GetDdlProduct();
                            //GetDdlProcGroup();
                            //GetImRecycleRatio();
                            //GetAllLayout();
                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
                            }

                            Session["GvTemp"] = null;
                            Session["InvalidRequest"] = null;
                        }
                    }
                    else
                    {
                        UpdateDtGvTemp();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "DatePitcker();", true);
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChgEmptyFlColor();", true);

                        if (Session["sidebarToggle"] == null)
                        {
                            SideBarMenu.Attributes.Add("style", "display:block;");
                        }
                        else
                        {
                            SideBarMenu.Attributes.Add("style", "display:none;");
                        }

                        if (GvTemp.Rows.Count > 0)
                        {
                            BtnCreateReq.Visible = true;
                            //LbTitleQrefList.Visible = true;

                            //for (int r=0; r<GvTemp.Rows.Count; r++)
                            //{
                            //    Label LblengtVC = (Label)GvTemp.Rows[r].FindControl("LblengtVC");
                            //    TextBox TxtOthReason = (TextBox)GvTemp.Rows[r].FindControl("TxtOthReason");
                            //    if (TxtOthReason.Text != "")
                            //    {
                            //        int countres = TxtOthReason.Text.Length;
                            //        int rest = (200 - countres);
                            //        LblengtVC.Text = rest.ToString() + " Character left";
                            //    }
                            //}
                        }
                        else
                        {
                            BtnCreateReq.Visible = false;
                            //LbTitleQrefList.Visible = false;
                        }

                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("");
                        var script = string.Format("CloseLoading();SetMyFocus();ChgEmptyFlColor();", message);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                    }
                }
            }
            catch (ThreadAbortException)
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

        protected void GetDbTrans()
        {
            try
            {
                DbTransName = EMETModule.GetDbTransname ()+ ".[dbo].";
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                DbTransName = "";
            }
        }

        string GetQuoteNextRevDate()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string DefVal = "";
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select DefValue from DefaultValueMaster where Description = 'Quote Next Rev Date' and  Plant='" + Session["EPlant"].ToString() + "' and DelFlag=0 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows[0]["DefValue"].ToString() != "")
                        {
                            DefVal = dt.Rows[0]["DefValue"].ToString();
                        }
                    }
                }
                return DefVal;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return DefVal;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        bool ValidateDefQuoteNextRev(string DefQuoteNextRev)
        {
            try
            {
                DateTime DateDefQuoteNextRev = new DateTime();
                DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        DateTime DateDuenextRev()
        {
            DateTime DtDuenextRev = new DateTime();
            try
            {
                string DefQuoteNextRev = GetQuoteNextRevDate();
                if (DefQuoteNextRev != "")
                {
                    DefQuoteNextRev = DefQuoteNextRev.Replace(".", "-").Replace("/", "-");
                    if (ValidateDefQuoteNextRev(DefQuoteNextRev) == true)
                    {
                        DateTime DateDefQuoteNextRev = new DateTime();
                        DateDefQuoteNextRev = DateTime.ParseExact(DefQuoteNextRev, "dd-MM-yyyy", null);
                        DtDuenextRev = DateDefQuoteNextRev;
                    }
                }

                return DtDuenextRev;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return DtDuenextRev;
            }
        }

        protected void GetImRecycleRatio()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct RecycleRatio from tIMRecycleratio where Plant='" + Session["EPlant"].ToString() + @"' and isnull(DelFlag,0)=0 
                             order by RecycleRatio asc";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["RecycleRatio"] = dt;
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

        

        protected void GetDdlProduct()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct PR.product,CONCAT(PR.product,+ ' - '+ PR.Description) as productdescription from TPRODUCT as PR 
                            inner join TPLANT as p on p.plant=pr.plant 
                            inner join TSMNProductPIC TPIC on PR.product = TPIC.product
                            where p.plant=@Plant and PR.DelFlag = 0";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DdlProduct.Items.Clear();
                            DdlProduct.DataTextField = "productdescription";
                            DdlProduct.DataValueField = "product";

                            DdlProduct.DataSource = dt;
                            DdlProduct.DataBind();
                            if (dt.Rows.Count > 1)
                            {
                                DdlProduct.Items.Insert(0, new ListItem("--Select Product Code--", "Select Product Code"));
                            }
                        }
                        else
                        {
                            DdlProduct.Items.Clear();
                            DdlProduct.Items.Insert(0, new ListItem("--Product Code Not Exist--", "Product Code Not Exist"));
                        }
                    }
                    //UpdatePanel18.Update();
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

        string GetLAyoutFromProcGroup(string PG)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string ScreenLayout = "";
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct ScreenLayout from TPROCESGRP_SCREENLAYOUT where ProcessGrp = @ProcessGrp ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcessGrp", PG);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            ScreenLayout = dt.Rows[0]["ScreenLayout"].ToString();
                        }
                        else
                        {
                            ScreenLayout = "";
                        }
                    }
                }
                return ScreenLayout;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return ScreenLayout;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        protected void GetAllLayout()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct ProcessGrp,ScreenLayout from TPROCESGRP_SCREENLAYOUT WHERE isnull(DelFlag,0) = 0 ";
                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GvlayoutList.DataSource = dt;
                        GvlayoutList.DataBind();
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

        

        

        private void GetQuoteList()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" With CTE
                            As
                            (
                            select  Row_Number() Over(Partition by TQ.VendorCode1,TQ.Material Order By TQ.VendorCode1 asc,TQ.UpdatedOn desc) As Row_Num
                            , TQ.QuoteNo,TQ.VendorCode1,TQ.VendorName,TVPo.CodeRef as SearchTerm,TQ.Product,TQ.MaterialClass,TQ.MaterialType,TQ.PlantStatus,
                            TQ.SAPProcType,TQ.SAPSpProcType,TQ.Material,TQ.MaterialDesc,TQ.PlatingType,TQ.PIRType,
                            TQ.NetUnit,TQ.UOM,TQ.MQty,TQ.BaseUOM,TQ.ProcessGroup,
                            ((select Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code=TQ.ProcessGroup)) as PrcGrpDesc,
                            TQ.PIRJobType,
                            format(TQ.QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            CAST(ROUND(TQ.TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                            CAST(ROUND(TQ.TotalProcessCost,5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                            CAST(ROUND(TQ.TotalSubMaterialCost,5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                            CAST(ROUND(TQ.TotalOtheritemsCost,5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost',
                            CAST(ROUND(TQ.GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                            CAST(ROUND(TQ.FinalQuotePrice,5) AS DECIMAL(12,5)) as 'FinalQuotePrice',TQ.UpdatedOn,
                            case 
                            when ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is null or QuoteNoRef = '')) then 'New'
                            when ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is not null or QuoteNoRef <> '')) then 'Revision'
                            when (isMassRevision = 1) then 'Mass Revision'
                            else 'Unknown' 
                             end as 'ReqType',
                            isnull(tm.PlantStatus,'') as MMPlantStatus
                            from TQuoteDetails TQ 
                            inner join TProcessCostDetails TP on TQ.QuoteNo = TP.QuoteNo
                            INNER JOIN " + DbMasterName + @".dbo.tvendorporg TVPo ON TQ.VendorCode1 = TVPo.Vendor
                            INNER JOIN " + DbMasterName + @".dbo.TPOrgPlant TPPo ON TVPo.porg = TPPo.POrg
                            inner join " + DbMasterName + @".dbo.TMATERIAL TM on tq.Plant = tm.Plant and tq.Material = tm.Material  and ISNULL(tm.DelFlag,0)=0
                            where (TQ.ApprovalStatus='3') ";
                    if (chkActiveMaterial.Checked == true) {
                        sql += @" and isnull(tm.PlantStatus,'') not in ('z4','z9') ";
                    }
                    sql += @"  
                            --and (TQ.PICApprovalStatus='2') 
                            --and (isMassRevision = 0 or isMassRevision is null)
                            and (TQ.ManagerApprovalStatus='2') and (TQ.DIRApprovalStatus='0') and TQ.Plant= @Plant ";

                    if (DdlReqType.SelectedValue == "New")
                    {
                        sql += @" and ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is null or QuoteNoRef = '')) ";
                    }
                    else if (DdlReqType.SelectedValue == "Revision")
                    {
                        sql += @" and ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is not null or QuoteNoRef <> '')) ";
                    }
                    else if (DdlReqType.SelectedValue == "MassRev")
                    {
                        sql += @" and (isMassRevision = 1) ";
                    }

                    if (DdlProduct.SelectedIndex > 0)
                    {
                        sql += @" and TQ.Product=@Product ";
                    }
                    if (DdlMatClassDesc.SelectedIndex > 0)
                    {
                        sql += @" and TQ.MaterialClass = @MaterialClass ";
                    }
                    if (DdlProcGroup.SelectedIndex > 0)
                    {
                        sql += @" and (select CONCAT(TQ.ProcessGroup,' - ', (select Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code=TQ.ProcessGroup) )) = @ProcessGroup ";
                    }
                    if (TxtSubProc.Text != "")
                    {
                        sql += @" and TP.SubProcess like '%'+@SubProc+'%' ";
                    }
                    if (TxtFilter.Text != "")
                    {
                        if (DdlFilter.SelectedIndex > 0)
                        {
                            if (DdlFilter.SelectedValue.ToString() == "VendorCode")
                            {
                                sql += @" and TQ.VendorCode1 like '%'+@Filter+'%' ";
                            }
                            else if (DdlFilter.SelectedValue.ToString() == "VendorName")
                            {
                                sql += @" and TQ.VendorName like '%'+@Filter+'%' ";
                            }
                            else if (DdlFilter.SelectedValue.ToString() == "Material")
                            {
                                sql += @" and TQ.Material like '%'+@Filter+'%' ";
                            }
                            else if (DdlFilter.SelectedValue.ToString() == "MaterialDesc")
                            {
                                sql += @" and TQ.MaterialDesc like '%'+@Filter+'%' ";
                            }
                            else if (DdlFilter.SelectedValue.ToString() == "QuoteNo")
                            {
                                sql += @" and TQ.QuoteNo like '%'+@Filter+'%' ";
                            }
                            //else if (DdlFilter.SelectedValue.ToString() == "ProcessGroup")
                            //{
                            //    sql += @" and TQ.ProcessGroup like '%'+@Filter+'%' ";
                            //}
                            //else if (DdlFilter.SelectedValue.ToString() == "PrcGrpDesc")
                            //{
                            //    sql += @" and (select Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code=TQ.ProcessGroup) like '%'+@Filter+'%' ";
                            //}
                            else if (DdlFilter.SelectedValue.ToString() == "CreatedBy")
                            {
                                sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) like '%'+@Filter+'%' ";
                            }
                            else if (DdlFilter.SelectedValue.ToString() == "UseDep")
                            {
                                sql += @"  and TQ.SMNPicDept like '%'+@Filter+'%' ";
                            }
                        }
                    }
                    
                    if (RbExternal.Checked == true)
                    {
                        sql += @" and TQ.VendorCode1 not in (select TS.VendorCode from " + DbMasterName + @".dbo.TSBMPRICINGPOLICY TS) ";
                    }
                    else
                    {
                        sql += @" and TQ.VendorCode1 in (select TS.VendorCode from " + DbMasterName + @".dbo.TSBMPRICINGPOLICY TS) ";
                    }

                    sql += @" )
                            Select*
                            From CTE
                            Where ROW_NUM = 1 ";

                    #region cek quote no already in temp valid and invalid request list
                    string QNoinlist = "";
                    if (Session["GvTemp"] != null)
                    {
                        
                        DataTable dtGvTemp = (DataTable)Session["GvTemp"];
                        if (dtGvTemp.Rows.Count > 0)
                        {
                            for (int GTr = 0; GTr < dtGvTemp.Rows.Count; GTr++)
                            {
                                QNoinlist += dtGvTemp.Rows[GTr]["QuoteNo"].ToString() + "|";
                            }
                        }
                    }
                    if (Session["InvalidRequest"] != null)
                    {
                        DataTable dtGvTemp = (DataTable)Session["InvalidRequest"];
                        if (dtGvTemp.Rows.Count > 0)
                        {
                            for (int GTr = 0; GTr < dtGvTemp.Rows.Count; GTr++)
                            {
                                QNoinlist += dtGvTemp.Rows[GTr]["QuoteNoRef"].ToString() + "|";
                            }
                        }
                    }
                    if (QNoinlist != "")
                    {
                        sql += @" and '" + QNoinlist + "' not like '%' + QuoteNo + '%'  ";
                    }
                    #endregion cek quote no already in temp valid and invalid request list

                    cmd = new SqlCommand(sql, EmetCon);
                    if (DdlProduct.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@Product", DdlProduct.SelectedValue.ToString());
                    }
                    if (DdlMatClassDesc.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@MaterialClass", DdlMatClassDesc.SelectedValue.ToString());
                    }
                    if (DdlProcGroup.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@ProcessGroup", DdlProcGroup.SelectedValue.ToString());
                    }
                    if (TxtSubProc.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@SubProc", TxtSubProc.Text);
                    }
                    if (TxtFilter.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", TxtFilter.Text);
                    }
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        
                        GvQuoteRefList.DataSource = dt;
                        GvQuoteRefList.DataBind();
                        Session["GvQuoteRefList"] = dt;
                        Session["GvQuoteRefListTemp"] = dt;
                        if (GvQuoteRefList.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string Layout = GetLAyoutFromProcGroup(dt.Rows[i]["ProcessGroup"].ToString());
                                if (Layout == "Layout7")
                                {
                                    TxtIsLY7InTheList.Text = "true";
                                    break;
                                }
                                else
                                {
                                    TxtIsLY7InTheList.Text = "false";
                                }
                            }
                            BtnAddToList.Visible = true;
                            //DvTitleNoData.Visible = false;
                        }
                        else
                        {
                            BtnAddToList.Visible = false;
                            //DvTitleNoData.Visible = true;
                        }
                    }
                }
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
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

        protected void ShowActionDet(string QuoteNo, int RowParentGv)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select QuoteNo, '" + RowParentGv + @"' as ParentGvRowNo,BaseUOM from TQuoteDetails where QuoteNo = @QuoteNo ";
                    
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        Session["GvAction"] = dt;
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

        string GenerateReqNo()
        {
            SqlConnection EmetConReqNo = new SqlConnection(EMETModule.GenEMETConnString());
            string GenerateReqNo = "";
            try
            {
                string strdate = txtReqDate.Text;
                string formatstatus = string.Empty;
                string currentMonth = DateTime.Now.Month.ToString();
                string currentYear = DateTime.Now.Year.ToString();
                string currentYear_ = DateTime.Now.Year.ToString();
                currentYear = currentYear.Substring(currentYear.Length - 2);
                string RequestIncNumber = "";
                int increquest = 00000;
                string RequestInc = String.Format(currentYear, increquest);
                string REQUEST = string.Concat(currentYear, RequestIncNumber);

                SqlCommand cmd2;
                EmetConReqNo.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select MAX(RequestNumber) from TQuoteDetails ";
                    cmd2 = new SqlCommand(sql, EmetConReqNo);
                    //cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd2;
                    using (DataTable dt2 = new DataTable())
                    {
                        sda.Fill(dt2);
                        if (dt2.Rows.Count > 0)
                        {
                            string curentYear = DateTime.Now.Year.ToString();
                            curentYear = curentYear.Substring(curentYear.Length - 2);
                            currentYear = currentYear.Substring(currentYear.Length - 2);
                            string ReqNum = dt2.Rows[0][0].ToString();

                            if (ReqNum == "")
                            {
                                RequestIncNumber = "00000";
                                RequestIncNumber1 = "00000";
                                Session["RequestIncNumber1"] = "00000";
                                string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber = string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber1 = string.Concat(currentYear, RequestIncNumber);
                                Session["RequestIncNumber1"] = string.Concat(currentYear, RequestIncNumber);
                                GenerateReqNo = RequestIncNumber;
                            }
                            else
                            {

                                ReqNum = ReqNum.Remove(0, 2);
                                ReqNum = string.Concat(currentYear, ReqNum);
                                RequestIncNumber = ReqNum;
                                RequestIncNumber1 = ReqNum;
                                Session["RequestIncNumber1"] = ReqNum;
                                GenerateReqNo = RequestIncNumber;
                            }
                            int newReq = (int.Parse(RequestIncNumber)) + (1);
                            RequestIncNumber = newReq.ToString();
                            RequestIncNumber1 = RequestIncNumber;
                            GenerateReqNo = RequestIncNumber;
                            Session["RequestIncNumber1"] = RequestIncNumber;
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
                EmetConReqNo.Dispose();
            }
            return GenerateReqNo;
        }
        
        protected void createRequest()
        {
            try
            {
                Session["Rawmaterial"] = null;
                Session["OldRawmaterial"] = null;
                Session["CreateReqTemp"] = null;

                DateTime ReqDate = DateTime.ParseExact(txtReqDate.Text, "dd-MM-yyyy", null);
                DataTable dtGvTemp = new DataTable();

                if (Session["GvTemp"] != null)
                {
                    dtGvTemp = (DataTable)Session["GvTemp"];
                }

                if (GvTemp.Rows.Count > 0 && dtGvTemp.Rows.Count > 0)
                {
                    for (int r = 0; r < GvTemp.Rows.Count; r++)
                    {
                        string RequestIncNumber = GenerateReqNo().ToString();
                        SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                        SqlTransaction EmetTrans;
                        EmetCon.Open();
                        EmetTrans = EmetCon.BeginTransaction();
                        try
                        {

                            Session["flag"] = "Fail";
                            string QuoteSearchTerm = "";
                            QuoteSearchTerm = dtGvTemp.Rows[r]["QuoteNo"].ToString().Substring(0, 3);
                            string QuoteNo = String.Concat(QuoteSearchTerm, RequestIncNumber);

                            string AcsTabMatCost = dtGvTemp.Rows[r]["IsMatcostAllow"].ToString();
                            string AcsTabProcCost = dtGvTemp.Rows[r]["IsProccostAllow"].ToString();
                            string AcsTabSubMatCost = dtGvTemp.Rows[r]["IsSubMatcostAllow"].ToString();
                            string AcsTabOthMatCost = dtGvTemp.Rows[r]["IsOthcostAllow"].ToString();

                            string IsUseToolAmortize = dtGvTemp.Rows[r]["IsUseToolAmor"].ToString();
                            if (IsUseToolAmortize == "") { IsUseToolAmortize = "0"; }
                            string IsUseMachineAmortize = dtGvTemp.Rows[r]["IsUseMachineAmor"].ToString();
                            if (IsUseMachineAmortize == "") { IsUseMachineAmortize = "0"; }

                            string DrawingNo = dtGvTemp.Rows[r]["Attachment"].ToString();
                            DateTime ResDueDate = DateTime.ParseExact(dtGvTemp.Rows[r]["DueDate"].ToString(), "dd-MM-yyyy", null);
                            DateTime DtmEffectiveDate = DateTime.ParseExact(dtGvTemp.Rows[r]["EffectiveDate"].ToString(), "dd-MM-yyyy", null);
                            DateTime DtmDueDateNextRev = DateTime.ParseExact(dtGvTemp.Rows[r]["DueDateNextRev"].ToString(), "dd-MM-yyyy", null);
                            string BaseUOM = dtGvTemp.Rows[r]["BsUOM"].ToString();
                            string MQty = dtGvTemp.Rows[r]["MnthEstQty"].ToString();
                            string Reason = dtGvTemp.Rows[r]["Reason"].ToString();
                            string Remark = dtGvTemp.Rows[r]["Remark"].ToString();
                            string RecycleRatio = dtGvTemp.Rows[r]["RecycleRatio"].ToString();

                            sql = @"INSERT INTO TQuoteDetails (RequestNumber,RequestDate,QuoteNo,Plant,PlantStatus,
                                                MaterialType,SAPProcType,SAPSpProcType,Product, 
                                                MaterialClass,Material,MaterialDesc,PIRType,ProcessGroup,VendorCode1,VendorName,PIRJobType,NetUnit,
                                                DrawingNo,QuoteResponseDueDate,EffectiveDate,DueOn,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,
                                                AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost,QuoteNoRef,SMNPicDept,IMRecycleRatio
                                                ,IsUseToolAmortize,IsUseMachineAmortize) VALUES 
                                                (@RequestNumber,@RequestDate,@QuoteNo,@Plant,@PlantStatus,
                                                @MaterialType,@SAPProcType,@SAPSpProcType,@Product, 
                                                @MaterialClass,@Material,@MaterialDesc,@PIRType,@ProcessGroup,@VendorCode1,@VendorName,@PIRJobType,@NetUnit,
                                                @DrawingNo,@QuoteResponseDueDate,@EffectiveDate,@DueOn,@CreatedBy,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,
                                                @AcsTabMatCost,@AcsTabProcCost,@AcsTabSubMatCost,@AcsTabOthMatCost,@QuoteNoRef,@SMNPicDept,@IMRecycleRatio
                                                ,@IsUseToolAmortize,@IsUseMachineAmortize)";

                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.Parameters.AddWithValue("@RequestNumber", Convert.ToInt32(RequestIncNumber.ToString()));
                            cmd.Parameters.AddWithValue("@RequestDate", ReqDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                            cmd.Parameters.AddWithValue("@Plant", Convert.ToInt32(Session["EPlant"].ToString()));
                            cmd.Parameters.AddWithValue("@PlantStatus", dtGvTemp.Rows[r]["PlantStatus"].ToString());
                            cmd.Parameters.AddWithValue("@MaterialType", dtGvTemp.Rows[r]["MaterialType"].ToString());
                            cmd.Parameters.AddWithValue("@SAPProcType", dtGvTemp.Rows[r]["SAPProcType"].ToString());
                            cmd.Parameters.AddWithValue("@SAPSpProcType", dtGvTemp.Rows[r]["SAPSpProcType"].ToString());
                            cmd.Parameters.AddWithValue("@Product", dtGvTemp.Rows[r]["Product"].ToString());
                            cmd.Parameters.AddWithValue("@MaterialClass", dtGvTemp.Rows[r]["MaterialClass"].ToString());
                            cmd.Parameters.AddWithValue("@Material", dtGvTemp.Rows[r]["Material"].ToString());
                            cmd.Parameters.AddWithValue("@MaterialDesc", dtGvTemp.Rows[r]["MaterialDesc"].ToString());
                            cmd.Parameters.AddWithValue("@PIRType", dtGvTemp.Rows[r]["PIRType"].ToString());
                            cmd.Parameters.AddWithValue("@ProcessGroup", dtGvTemp.Rows[r]["ProcessGroup"].ToString());
                            cmd.Parameters.AddWithValue("@VendorCode1", dtGvTemp.Rows[r]["VendorCode1"].ToString());
                            cmd.Parameters.AddWithValue("@VendorName", dtGvTemp.Rows[r]["VendorName"].ToString());
                            cmd.Parameters.AddWithValue("@PIRJobType", dtGvTemp.Rows[r]["PIRJobType"].ToString());
                            cmd.Parameters.AddWithValue("@NetUnit", dtGvTemp.Rows[r]["NetUnit"].ToString());
                            cmd.Parameters.AddWithValue("@DrawingNo", DrawingNo);
                            cmd.Parameters.AddWithValue("@QuoteResponseDueDate", ResDueDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@EffectiveDate", DtmEffectiveDate.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@DueOn", DtmDueDateNextRev.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                            cmd.Parameters.AddWithValue("@BaseUOM", BaseUOM);
                            cmd.Parameters.AddWithValue("@MQty", MQty);
                            if (Reason == "Others")
                            {
                                cmd.Parameters.AddWithValue("@PICReason", DBNull.Value);
                                cmd.Parameters.AddWithValue("@ERemarks", Remark);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@PICReason", Reason);
                                cmd.Parameters.AddWithValue("@ERemarks", DBNull.Value);
                            }
                            cmd.Parameters.AddWithValue("@UOM", dtGvTemp.Rows[r]["UOM"].ToString());
                            cmd.Parameters.AddWithValue("@ActualNU", dtGvTemp.Rows[r]["NetUnit"].ToString());
                            cmd.Parameters.AddWithValue("@AcsTabMatCost", AcsTabMatCost);
                            cmd.Parameters.AddWithValue("@AcsTabProcCost", AcsTabProcCost);
                            cmd.Parameters.AddWithValue("@AcsTabSubMatCost", AcsTabSubMatCost);
                            cmd.Parameters.AddWithValue("@AcsTabOthMatCost", AcsTabOthMatCost);
                            cmd.Parameters.AddWithValue("@QuoteNoRef", dtGvTemp.Rows[r]["QuoteNo"].ToString());
                            cmd.Parameters.AddWithValue("@SMNPicDept", Session["userDept"].ToString());
                            cmd.Parameters.AddWithValue("@IMRecycleRatio", RecycleRatio);
                            cmd.Parameters.AddWithValue("@IsUseToolAmortize", IsUseToolAmortize);
                            cmd.Parameters.AddWithValue("@IsUseMachineAmortize", IsUseMachineAmortize);
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            EmetTrans.Commit();
                            GetData(RequestIncNumber.ToString(), dtGvTemp.Rows[r]["Material"].ToString(), dtGvTemp.Rows[r]["EffectiveDate"].ToString());
                            GetBOMRawmaterialBefEffdate(RequestIncNumber.ToString(), QuoteNo, dtGvTemp.Rows[r]["VendorCode1"].ToString(), dtGvTemp.Rows[r]["Material"].ToString(), Session["EPlant"].ToString(), dtGvTemp.Rows[r]["EffectiveDate"].ToString());
                            Session["flag"] = "Pass";
                            
                        }
                        catch (Exception xw)
                        {
                            try
                            {
                                EmetTrans.Rollback();
                                Session["flag"] = "Fail";
                            }
                            catch (SqlException ex)
                            {
                                if (EmetTrans.Connection != null)
                                {
                                    Session["flag"] = "Fail";
                                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error in Transaction Data :' " + ex + ")", true);
                                }
                            }

                            Session["flag"] = "Fail";
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Error in Transaction Data :' " + xw + ")", true);
                        }
                        finally {
                            EmetTrans.Dispose();
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "CloseLoading", "CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.Message.ToString() + " - " + ex.StackTrace.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);

                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(LbMsgErr.Text);
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected bool GetDataVndMachineAmor(string VendorCode,string ProcGrp, string EffDate)
        {
            SqlConnection MdmCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MdmCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select Vend_MachineID, EffectiveFrom from TMachineAmortization 
                            where Plant=@Plant and Process_Grp_code=@ProcGrp and VendorCode=@VendorCode
                            and (
                            (EffectiveFrom is null and DueDate is null) or (@EffectiveDate between format(EffectiveFrom,'yyyy-MM-dd') and format(DueDate,'yyyy-MM-dd')) 
                            )
                            and ISNULL(DelFlag,'') = 0 ";

                    cmd = new SqlCommand(sql, MdmCon);
                    cmd.Parameters.AddWithValue("@Plant", TxtPlant.Text);
                    if (EffDate != "")
                    {
                        DateTime DtEffectiveDate = DateTime.ParseExact(EffDate, "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@EffectiveDate", "");
                    }
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    cmd.Parameters.AddWithValue("@ProcGrp", ProcGrp);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
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
                return false;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MdmCon.Dispose();
            }
        }

        public void DeleteNonRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = "delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + Session["userID"].ToString() + "') or (CreateStatus is null and createdby= '" + Session["userID"].ToString() + "')";
                //Label17.Text = str.ToString();
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);
                BtnSubmitRequest.Visible = false;
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

        protected void UpdateDtGvTemp()
        {
            try
            {
                if (GvTemp.Rows.Count > 0)
                {
                    if (Session["GvTemp"] !=null)
                    {
                        DataTable dtGvtemp = (DataTable)Session["GvTemp"];
                        int i = 0;
                        foreach (GridViewRow row in GvTemp.Rows)
                        {
                            #region for nested gv
                            //GridView GvAct = (GridView)row.FindControl("GvAction");
                            //CheckBox IsAllcostAllow = (CheckBox)GvAct.Rows[0].FindControl("chkAllCost");
                            //CheckBox IsMatcostAllow = (CheckBox)GvAct.Rows[0].FindControl("chkMatCost");
                            //CheckBox IsProccostAllow = (CheckBox)GvAct.Rows[0].FindControl("chkProcCost");
                            //CheckBox IsSubMatcostAllow = (CheckBox)GvAct.Rows[0].FindControl("chkSubMatCost");
                            //CheckBox IsOthcostAllow = (CheckBox)GvAct.Rows[0].FindControl("chkOthCost");
                            //string DdlReasonID = ((DropDownList)GvAct.Rows[0].FindControl("DdlReason")).ClientID;
                            //DropDownList DdlReason = (DropDownList)GvAct.Rows[0].FindControl("DdlReason");
                            //Label lbFileName = (Label)GvAct.Rows[0].FindControl("lbFileName");
                            //TextBox TxtMQty = (TextBox)GvAct.Rows[0].FindControl("TxtMQty");
                            //TextBox TxtBaseUOM = (TextBox)GvAct.Rows[0].FindControl("TxtBaseUOM");
                            //TextBox TxtOthReason = (TextBox)GvAct.Rows[0].FindControl("TxtOthReason");
                            //Label LblengtVC = (Label)GvAct.Rows[0].FindControl("LblengtVC");
                            //TextBox TxtResDueDate = (TextBox)GvAct.Rows[0].FindControl("TxtResDueDate");
                            #endregion
                            
                            CheckBox IsAllcostAllow = (CheckBox)GvTemp.Rows[i].FindControl("chkAllCost");
                            CheckBox IsMatcostAllow = (CheckBox)GvTemp.Rows[i].FindControl("chkMatCost");
                            CheckBox IsProccostAllow = (CheckBox)GvTemp.Rows[i].FindControl("chkProcCost");
                            CheckBox IsSubMatcostAllow = (CheckBox)GvTemp.Rows[i].FindControl("chkSubMatCost");
                            CheckBox IsOthcostAllow = (CheckBox)GvTemp.Rows[i].FindControl("chkOthCost");

                            CheckBox chkToolAmortize = (CheckBox)GvTemp.Rows[i].FindControl("chkToolAmortize");
                            CheckBox chkMachineAmortize = (CheckBox)GvTemp.Rows[i].FindControl("chkMachineAmortize");

                            string DdlReasonID = ((DropDownList)GvTemp.Rows[i].FindControl("DdlReason")).ClientID;
                            DropDownList DdlReason = (DropDownList)GvTemp.Rows[i].FindControl("DdlReason");
                            DropDownList DdlRecycleRatio = (DropDownList)GvTemp.Rows[i].FindControl("DdlRecycleRatio");
                            Label lbFileName = (Label)GvTemp.Rows[i].FindControl("lbFileName");
                            TextBox TxtMQty = (TextBox)GvTemp.Rows[i].FindControl("TxtMQty");
                            TextBox TxtBaseUOM = (TextBox)GvTemp.Rows[i].FindControl("TxtBaseUOM");
                            TextBox TxtOthReason = (TextBox)GvTemp.Rows[i].FindControl("TxtOthReason");
                            Label LblengtVC = (Label)GvTemp.Rows[i].FindControl("LblengtVC");
                            TextBox TxtResDueDate = (TextBox)GvTemp.Rows[i].FindControl("TxtResDueDate");
                            TextBox TxtEffectiveDate = (TextBox)GvTemp.Rows[i].FindControl("TxtEffectiveDate");
                            TextBox TxtDueDateNextRev = (TextBox)GvTemp.Rows[i].FindControl("TxtDueDateNextRev");

                            if (IsAllcostAllow != null && IsMatcostAllow != null && IsProccostAllow != null && IsSubMatcostAllow != null && IsOthcostAllow != null && chkToolAmortize != null && chkMachineAmortize != null
                                && DdlReason != null &&
                                lbFileName != null && TxtMQty != null && TxtBaseUOM != null && TxtOthReason != null && LblengtVC != null && TxtResDueDate != null 
                                && TxtEffectiveDate != null && TxtDueDateNextRev != null)
                            {
                                if (IsAllcostAllow.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsAllcostAllow"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsAllcostAllow"] = "0";
                                }

                                if (IsMatcostAllow.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsMatcostAllow"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsMatcostAllow"] = "0";
                                }

                                if (IsProccostAllow.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsProccostAllow"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsProccostAllow"] = "0";
                                }

                                if (IsSubMatcostAllow.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsSubMatcostAllow"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsSubMatcostAllow"] = "0";
                                }

                                if (IsOthcostAllow.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsOthcostAllow"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsOthcostAllow"] = "0";
                                }

                                if (chkToolAmortize.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsUseToolAmor"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsUseToolAmor"] = "0";
                                }

                                if (chkMachineAmortize.Checked == true)
                                {
                                    dtGvtemp.Rows[i]["IsUseMachineAmor"] = "1";
                                }
                                else
                                {
                                    dtGvtemp.Rows[i]["IsUseMachineAmor"] = "0";
                                }

                                dtGvtemp.Rows[i]["Reason"] = DdlReason.SelectedValue.ToString();
                                //dtGvtemp.Rows[i]["Attachment"] = lbFileName.Text;
                                dtGvtemp.Rows[i]["MnthEstQty"] = TxtMQty.Text;
                                dtGvtemp.Rows[i]["BsUOM"] = TxtBaseUOM.Text;
                                dtGvtemp.Rows[i]["Remark"] = TxtOthReason.Text;
                                dtGvtemp.Rows[i]["DueDate"] = TxtResDueDate.Text;
                                dtGvtemp.Rows[i]["EffectiveDate"] = TxtEffectiveDate.Text;
                                dtGvtemp.Rows[i]["DueDateNextRev"] = TxtDueDateNextRev.Text;
                                dtGvtemp.Rows[i]["RecycleRatio"] = DdlRecycleRatio.SelectedValue.ToString();
                            }
                            i++;
                        }
                        Session["GvTemp"] = dtGvtemp;
                        GvTemp.DataSource = dtGvtemp;
                        GvTemp.DataBind();
                        TxtGvTempLeng.Text = dtGvtemp.Rows.Count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GetDataServerEmail()
        {
            SqlConnection MailCon = new SqlConnection(EMETModule.GenMailConnString());
            try
            {
                MailCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand("Email_UNC", MailCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@DeptId", txtFindDept.Text);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            SrvUserid = dt.Rows[0]["USERId"].ToString();
                            Srvpassword = dt.Rows[0]["password"].ToString();
                            Srvdomain = dt.Rows[0]["domain"].ToString();
                            Srvpath = dt.Rows[0]["path"].ToString();
                            Session["path"] = dt.Rows[0]["path"].ToString();
                            SrvURL = dt.Rows[0]["Url"].ToString();
                            //MasterDB = dt.Rows[0]["masterdb"].ToString();
                            //TransDB = dt.Rows[0]["transdb"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;
            }
            finally
            {
                MailCon.Dispose();
            }
        }

        protected void GetData(string reqno, string SAPPartCode, string EffectiveDate)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                GetDataServerEmail();
                GetDbTrans();
                MDMCon.Open();
                DataTable dtget = new DataTable();
                
                SqlDataAdapter da = new SqlDataAdapter();
                SqlDataAdapter da1 = new SqlDataAdapter();

                string strGetData = string.Empty;
                string strGetData1 = string.Empty;
                string plant = Session["EPlant"].ToString();
                DateTime dteffdate = DateTime.Now ;
                dteffdate = DateTime.ParseExact(EffectiveDate, "dd-MM-yyyy", null);
                //if (dtget1.Rows.Count > 0)
                //{
                //    grdvendor1.DataSource = dtget1;
                //    grdvendor1.DataBind();
                //}
                //else
                //{
                //    grdvendor1.DataSource = dtget1;
                //    grdvendor1.DataBind();
                //}

                //update for : To get the exchange rate based on effective date when more then one exchange rate is maintained 	
                //          strGetData = @" select distinct QuoteNoRef as [Quote No Ref],CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                //                  TB.Material as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],
                //                  TQ.QuoteNo as [Quote No New],vp.PICName as PICName,vp.PICemail,v.Crcy AS Crcy,tvpo.coderef as SearchTerm,tc.Amount   as Amt_SCur,
                //                  isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                //                  tc.UnitofCurrency as Selling_Crcy,
                //                  isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                //                  tc.Unit,--tc.UoM , 
                //tm.BaseUOM as UOM,
                //format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                //                  from TMATERIAL tm inner join TBOMLIST TB on tm.Material = TB.Material   
                //                  inner join TCUSTOMER_MATLPRICING tc on TB.Material = tc.Material    and tc.plant = '" + plant + @"' and tc.delflag = 0
                //                  inner join tVendor_New v on v.CustomerNo = tc.Customer   
                //                  inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   
                //                  inner join " + DbTransName+ @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                //                  left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant
                //                  and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                //                  where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"'  
                //                  and tvpo.Plant='" + plant + @"' and vp.plant = '" + plant + @"' and TB.FGCode = '" + SAPPartCode + @"' 
                //                  and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                //                  and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0) 
                //                  ";

                //update for : migrate the eMET BOM table from BOM Explosion to BOM list table
                strGetData = @"  
                --declare @plant nvarchar(4) ='2100'
                --declare @mat nvarchar(20)='40000721'

                declare @e50check bit = 1
                declare @e50mat nvarchar(20)
                set @e50mat=''

                --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                into #complist1 from tbomlistnew 
                where Plant='" + plant + @"' and [Parent Material]='" + SAPPartCode + @"'
                --no need altbom because not using PV for direct transfer
                --and [alternative bom]=@altbom 
                and [header valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [header valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
                and [comp. valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [comp. valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"' 
                and UPPER([component plant status]) not in ('Z4','Z9')
                and UPPER([parent plant status]) not in ('Z4','Z9')				
                and [co-product] <> 'X'

                --get the list of e50 components
                SELECT Plant,[Component Material] as 'Material'
                into #e50 from tbomlistnew 
                where Plant='" + plant + @"' and [Parent Material]='" + SAPPartCode + @"'
                --no need altbom because not using PV for direct transfer
                --and [alternative bom]=@altbom
                and [header valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [header valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
                and [comp. valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [comp. valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
                and UPPER([component plant status]) not in ('Z4','Z9')
                and UPPER([parent plant status]) not in ('Z4','Z9')
                and [Component special procurement type] = '50'
                and [co-product] <> 'X'

                IF ((select count (*) from #e50)=0)
                BEGIN
	                set @e50check=0
                END
                ELSE
                BEGIN
	                set @e50check=1
                END

                WHILE (@e50check=1)
                BEGIN
	                insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                where [header valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [header valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
	                and [comp. valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [comp. valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
	                and UPPER([component plant status]) not in ('Z4','Z9')
	                and UPPER([parent plant status]) not in ('Z4','Z9')
	                and [co-product] <> 'X'
	                group by
	                t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                select * into #temp from #e50
	                delete from #e50
	
	                insert into #e50 (Plant, Material) 
	                (SELECT t1.Plant, [Component Material]
	                from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                where [header valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"' and [header valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"'
	                and [comp. valid to date] > '" + dteffdate.ToString("yyyy-MM-dd") + @"'and [comp. valid from date] <= '" + dteffdate.ToString("yyyy-MM-dd") + @"' 
	                and UPPER([component plant status]) not in ('Z4','Z9')
	                and UPPER([parent plant status]) not in ('Z4','Z9')
	                and [Component special procurement type] = '50'
	                and [co-product] <> 'X')
	
	                drop table #temp
	
	                if ((select count (*) from #e50)=0)
	                BEGIN
		                set @e50check=0
	                END			
                END
                ";
                strGetData += @"  select distinct CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],TQ.RequestNumber as [Req No],   tvpo.Plant,  
                        TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],  TQ.vendorCode1 as [Vendor Code],tvpo.coderef as SearchTerm,
                        TQ.QuoteNo,TQ.QuoteNoRef as [Quote No Ref],vp.PICName as Ven_PIC,vp.PICemail as PICemail,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,
                        --tc.UoM 
                        tm.BaseUOM as UOM,
                        format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                        ,TQ.IsUseToolAmortize,TB.[Parent Material] as Material,(select top 1 MaterialDesc from TMATERIAL MT where MT.plant = tm.plant and MT.material=TB.[Parent Material] ) as MaterialDesc,TQ.ProcessGroup
                        from TMATERIAL tm inner join #complist1 TB on tm.Material = TB.[Component Material] and tb.Plant = tm.plant
                        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = '" + plant + @"'  and tc.delflag = 0
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                        inner join " + DbTransName + @"TQuoteDetails TQ   on Vp.VendorCode = TQ.VendorCode1 
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = TQ.Plant and VP.Plant = TQ.Plant  and tvpo.Plant = TQ.Plant
                        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= TQ.EffectiveDate )
                        where TQ.RequestNumber='" + reqno + "' and tm.Plant='" + plant + @"'
                        and tvpo.Plant='" + plant + @"' and vp.plant = '" + plant + @"' and TB.[Parent Material] = '" + SAPPartCode + @"' 
                        and (TQ.EffectiveDate BETWEEN TC.ValidFrom and TC.ValidTO) 
                        and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!=0)
                        and tc.delflag = 0
                        ";
                if (EffectiveDate != "")
                {
                    strGetData += " and ('" + dteffdate.ToString("yyyy-MM-dd") + @"' between tc.ValidFrom and tc.ValidTo) ";
                }

                strGetData += @" drop table #complist1, #e50 ";

                da = new SqlDataAdapter(strGetData, MDMCon);
                da.Fill(dtget);

                if (Session["Rawmaterial"] == null)
                {
                    Session["Rawmaterial"] = dtget;
                }
                else
                {
                    if (dtget.Rows.Count > 0)
                    {
                        DataTable DtRawmat = (DataTable)Session["Rawmaterial"];
                        foreach (DataRow drdtget in dtget.Rows)
                        {
                            DtRawmat.ImportRow(drdtget);
                        }

                        Session["Rawmaterial"] = DtRawmat;
                    }
                }

                if (dtget.Rows.Count > 0)
                {
                    Label2.Visible = true;
                    BtnSubmitRequest.Visible = true;
                    DataTable DtCreateReqTemp = new DataTable();
                    if (Session["CreateReqTemp"] == null)
                    {
                        DtCreateReqTemp.Clear();
                        DtCreateReqTemp.Columns.Add("Quote No Ref", typeof(string));
                        DtCreateReqTemp.Columns.Add("Req Date", typeof(string));
                        DtCreateReqTemp.Columns.Add("Req No", typeof(string));
                        DtCreateReqTemp.Columns.Add("Plant", typeof(string));
                        DtCreateReqTemp.Columns.Add("Comp Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("Comp Material Desc", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Name", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Code", typeof(string));
                        DtCreateReqTemp.Columns.Add("SearchTerm", typeof(string));
                        DtCreateReqTemp.Columns.Add("Quote No New", typeof(string));
                        DtCreateReqTemp.Columns.Add("Ven PIC", typeof(string));
                        DtCreateReqTemp.Columns.Add("PIC Email", typeof(string));
                        DtCreateReqTemp.Columns.Add("Selling Crcy", typeof(string));
                        DtCreateReqTemp.Columns.Add("Amt SCur", typeof(string));
                        DtCreateReqTemp.Columns.Add("Exch Rate", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Crcy", typeof(string));
                        DtCreateReqTemp.Columns.Add("Amt VCur", typeof(string));
                        DtCreateReqTemp.Columns.Add("Unit", typeof(string));
                        DtCreateReqTemp.Columns.Add("UOM", typeof(string));
                        DtCreateReqTemp.Columns.Add("ValidFrom", typeof(string));
                        DtCreateReqTemp.Columns.Add("CusMatValFrom", typeof(string));
                        DtCreateReqTemp.Columns.Add("CusMatValTo", typeof(string));
                        DtCreateReqTemp.Columns.Add("IsUseToolAmortize", typeof(string));
                        DtCreateReqTemp.Columns.Add("Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("MaterialDesc", typeof(string));
                        DtCreateReqTemp.Columns.Add("ProcessGroup", typeof(string));
                    }
                    else
                    {
                        DtCreateReqTemp = (DataTable)Session["CreateReqTemp"];
                    }

                    for (int i = 0; i < dtget.Rows.Count; i++)
                    {
                        DataRow dr = DtCreateReqTemp.NewRow();
                        dr["Quote No Ref"] = dtget.Rows[i]["Quote No Ref"].ToString();
                        dr["Req Date"] = dtget.Rows[i]["Req Date"].ToString();
                        dr["Req No"] = reqno;
                        dr["Plant"] = plant;
                        dr["Comp Material"] = dtget.Rows[i]["Comp Material"].ToString();
                        dr["Comp Material Desc"] = dtget.Rows[i]["Comp Material Desc"].ToString();
                        dr["Vendor Name"] = dtget.Rows[i]["Vendor Name"].ToString();
                        dr["Vendor Code"] = dtget.Rows[i]["Vendor Code"].ToString();
                        dr["SearchTerm"] = dtget.Rows[i]["SearchTerm"].ToString();
                        dr["Quote No New"] = dtget.Rows[i]["QuoteNo"].ToString();
                        dr["Ven PIC"] = dtget.Rows[i]["Ven_PIC"].ToString();
                        dr["PIC Email"] = dtget.Rows[i]["PICemail"].ToString();
                        dr["Selling Crcy"] = dtget.Rows[i]["Selling_Crcy"].ToString();
                        dr["Amt SCur"] = dtget.Rows[i]["Amt_SCur"].ToString();
                        dr["Exch Rate"] = dtget.Rows[i]["ExchRate"].ToString();
                        dr["Vendor Crcy"] = dtget.Rows[i]["Venor_Crcy"].ToString();
                        dr["Amt VCur"] = dtget.Rows[i]["Amt_VCur"].ToString();
                        dr["Unit"] = dtget.Rows[i]["Unit"].ToString();
                        dr["UOM"] = dtget.Rows[i]["UOM"].ToString();
                        dr["ValidFrom"] = dtget.Rows[i]["ValidFrom"].ToString();
                        dr["CusMatValFrom"] = dtget.Rows[i]["CusMatValFrom"].ToString();
                        dr["CusMatValTo"] = dtget.Rows[i]["CusMatValTo"].ToString();
                        dr["IsUseToolAmortize"] = dtget.Rows[i]["IsUseToolAmortize"].ToString();
                        dr["Material"] = dtget.Rows[i]["Material"].ToString();
                        dr["MaterialDesc"] = dtget.Rows[i]["MaterialDesc"].ToString();
                        dr["ProcessGroup"] = dtget.Rows[i]["ProcessGroup"].ToString();
                        DtCreateReqTemp.Rows.Add(dr);
                    }
                    Session["CreateReqTemp"] = DtCreateReqTemp;
                }
                else
                {
                    GetDataforNoBom(reqno.ToString());
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

        protected void GetDataforNoBom(string reqno)
        {
            SqlConnection MDMConDataforNoBom = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMConDataforNoBom.Open();
                DataTable dtget = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                string strGetData = string.Empty;
                SqlDataAdapter da1 = new SqlDataAdapter();
                string strGetData1 = string.Empty;
                string plant = TxtPlant.Text;

                strGetData = @"select distinct QuoteNoRef as [Quote No Ref],CONVERT(VARCHAR(10), TQ.RequestDate, 103) as [Req Date],
                                TQ.RequestNumber as [Req No], TQ.Plant, '-' as 'Comp Material' ,'-' as 'Comp Material Desc',
                                TQ.VendorName as [VendorName],TQ.vendorCode1 as [VendorCode], TQ.QuoteNo as [Quote No New],vp.PICName,vp.PICemail, 
                                TV.Crcy,tvpo.coderef as SearchTerm
                                ,NULL as 'Amt_SCur'
                                ,NULL as 'ExchRate'
                                ,NULL as 'Selling_Crcy'
                                ,NULL as 'Amt_VCur'
                                ,NULL as 'Unit'
                                ,NULL as 'UoM'
                                ,TQ.IsUseToolAmortize,TQ.Material,TQ.MaterialDesc as MaterialDesc,TQ.ProcessGroup
                                from " + DbTransName +@"TQuoteDetails TQ 
                                inner join tVendor_New TV ON TQ.VendorCode1 = TV.Vendor
                                inner join tVendorPOrg tvpo ON TQ.VendorCode1 = tvpo.Vendor  and tv.porg=tvpo.porg 
                                inner join TVENDORPIC as VP on vp.VendorCode=TV.Vendor    
                                where TQ.RequestNumber = '" + reqno + @"'  
                                and tvpo.Plant = '" + plant + @"' 
                                and vp.Plant= '" + plant + @"'
                                ";

                da = new SqlDataAdapter(strGetData, MDMConDataforNoBom);
                da.Fill(dtget);

                if (dtget.Rows.Count > 0)
                {
                    Label2.Visible = true;
                    BtnSubmitRequest.Visible = true;
                    DataTable DtCreateReqTemp = new DataTable();
                    if (Session["CreateReqTemp"] == null)
                    {
                        DtCreateReqTemp.Clear();
                        DtCreateReqTemp.Columns.Add("Quote No Ref", typeof(string));
                        DtCreateReqTemp.Columns.Add("Req Date", typeof(string));
                        DtCreateReqTemp.Columns.Add("Req No", typeof(string));
                        DtCreateReqTemp.Columns.Add("Plant", typeof(string));
                        DtCreateReqTemp.Columns.Add("Comp Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("Comp Material Desc", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Name", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Code", typeof(string));
                        DtCreateReqTemp.Columns.Add("SearchTerm", typeof(string));
                        DtCreateReqTemp.Columns.Add("Quote No New", typeof(string));
                        DtCreateReqTemp.Columns.Add("Ven PIC", typeof(string));
                        DtCreateReqTemp.Columns.Add("PIC Email", typeof(string));
                        DtCreateReqTemp.Columns.Add("Selling Crcy", typeof(string));
                        DtCreateReqTemp.Columns.Add("Amt SCur", typeof(string));
                        DtCreateReqTemp.Columns.Add("Exch Rate", typeof(string));
                        DtCreateReqTemp.Columns.Add("Vendor Crcy", typeof(string));
                        DtCreateReqTemp.Columns.Add("Amt VCur", typeof(string));
                        DtCreateReqTemp.Columns.Add("Unit", typeof(string));
                        DtCreateReqTemp.Columns.Add("UOM", typeof(string));
                        DtCreateReqTemp.Columns.Add("ValidFrom", typeof(string));
                        DtCreateReqTemp.Columns.Add("CusMatValFrom", typeof(string));
                        DtCreateReqTemp.Columns.Add("CusMatValTo", typeof(string));
                        DtCreateReqTemp.Columns.Add("IsUseToolAmortize", typeof(string));
                        DtCreateReqTemp.Columns.Add("Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("MaterialDesc", typeof(string));
                        DtCreateReqTemp.Columns.Add("ProcessGroup", typeof(string));
                    }
                    else
                    {
                        DtCreateReqTemp = (DataTable)Session["CreateReqTemp"];
                    }

                    for (int i = 0; i < dtget.Rows.Count; i++)
                    {
                        DataRow dr = DtCreateReqTemp.NewRow();
                        dr["Quote No Ref"] = dtget.Rows[i]["Quote No Ref"].ToString();
                        dr["Req Date"] = dtget.Rows[i]["Req Date"].ToString();
                        dr["Req No"] = reqno;
                        dr["Plant"] = plant;
                        dr["Comp Material"] = "-";
                        dr["Comp Material Desc"] = "-";
                        dr["Vendor Name"] = dtget.Rows[i]["VendorName"].ToString();
                        dr["Vendor Code"] = dtget.Rows[i]["VendorCode"].ToString();
                        dr["SearchTerm"] = dtget.Rows[i]["SearchTerm"].ToString();
                        dr["Quote No New"] = dtget.Rows[i]["Quote No New"].ToString();
                        dr["Ven PIC"] = dtget.Rows[i]["PICName"].ToString();
                        dr["PIC Email"] = dtget.Rows[i]["PICemail"].ToString();
                        dr["Selling Crcy"] = "";
                        dr["Amt SCur"] = "";
                        dr["Exch Rate"] = "";
                        dr["Vendor Crcy"] = "";
                        dr["Amt VCur"] = "";
                        dr["Unit"] = "";
                        dr["UOM"] = "";
                        dr["ValidFrom"] = "";
                        dr["CusMatValFrom"] = "";
                        dr["CusMatValTo"] = "";
                        dr["IsUseToolAmortize"] = dtget.Rows[i]["IsUseToolAmortize"].ToString();
                        dr["Material"] = dtget.Rows[i]["Material"].ToString();
                        dr["MaterialDesc"] = dtget.Rows[i]["MaterialDesc"].ToString();
                        dr["ProcessGroup"] = dtget.Rows[i]["ProcessGroup"].ToString();
                        DtCreateReqTemp.Rows.Add(dr);
                    }
                    Session["CreateReqTemp"] = DtCreateReqTemp;
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMConDataforNoBom.Dispose();
            }
        }

        protected void SendingMail()
        {
            sendingmail = false;
            SqlTransaction transaction11 = null;
            errmsg = "";
            try
            {
                DataTable dtGvTemp = new DataTable();
                DataTable dtCreateReqTemp = new DataTable();
                string Userid = string.Empty;
                string password = string.Empty;
                string domain = string.Empty;
                string path = string.Empty;
                string URL = string.Empty;
                string MasterDB = string.Empty;
                string TransDB = string.Empty;
                
                string formatstatus = string.Empty;
                string remarks = string.Empty;
                //string currentMonth = DateTime.Now.Month.ToString();
                //string currentYear = DateTime.Now.Year.ToString();
                //string RequestInc = String.Format(currentYear, increquest);
                //string REQUEST = string.Concat(currentYear, Session["RequestIncNumber1"].ToString());
                string strdate = txtReqDate.Text;
                //int rowscount = 0;
                
                if (Session["GvTemp"] != null)
                {
                    dtGvTemp = (DataTable)Session["GvTemp"];
                }
                
                if (Session["CreateReqTemp"] != null)
                {
                    dtCreateReqTemp = (DataTable)Session["CreateReqTemp"];
                }

                if (dtCreateReqTemp.Rows.Count > 0 && GvTemp.Rows.Count > 0 && GvTemp.Rows.Count > 0)
                {
                    for (int t = 0; t < dtCreateReqTemp.Rows.Count; t++)
                    {
                        string ReqNumber = dtCreateReqTemp.Rows[t]["Req No"].ToString();
                        string QuoteNoNew = dtCreateReqTemp.Rows[t]["Quote No New"].ToString();
                        string QuoteNoRef = dtCreateReqTemp.Rows[t]["Quote No Ref"].ToString();
                        string SAPPartCode = dtGvTemp.Rows[t]["Material"].ToString();
                        string SAPPartDesc = dtGvTemp.Rows[t]["MaterialDesc"].ToString();
                        remarks = "Open Status";
                        //rowscount++;
                        DateTime QuoteDate = DateTime.ParseExact(dtGvTemp.Rows[t]["DueDate"].ToString(), "dd-MM-yyyy", null);

                        #region getting Messageheader ID from IT Mailapp
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMailConnString()))
                        {
                            cnn.Open();
                            SqlTransaction transactionHS;
                            transactionHS = cnn.BeginTransaction("HeaderSelection");
                            try
                            {
                                string returnValue = string.Empty;

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
                                Session["OriginalFilename"] = returnValue;
                                MHid = returnValue;
                                Session["MHid"] = returnValue;
                                OriginalFilename = MHid + seqNo + formatW;
                                Session["OriginalFilename"] = MHid + seqNo + formatW;
                            }
                            catch (Exception cc2)
                            {
                                errmsg += "Quote NO : " + QuoteNoNew + "function getting Messageheader ID from IT Mailapp:" + "Msg Err :" + cc2.Message.ToString() + "\n";
                                transactionHS.Rollback();
                                //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Error in Mail Headerselection " + cc2 + " ");
                                //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        Boolean IsAttachFile = true;
                        int SequenceNumber = 1;
                        #region Uploading  ttachment to Mail sever using UNC credentials
                        fname = "";
                        if (Session["FlAtc" + QuoteNoRef] != null)
                        {
                            FlAttachment = (FileUpload)Session["FlAtc" + QuoteNoRef];
                            if (FlAttachment.HasFile)
                            {
                                fname = FlAttachment.PostedFile.FileName;
                            }

                        }
                        if (fname != "")
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
                                        Userid = dr.GetString(0);
                                        password = dr.GetString(1);
                                        domain = dr.GetString(2);
                                        path = dr.GetString(3);
                                        Session["path"] = dr.GetString(3);
                                        URL = dr.GetString(4);
                                        MasterDB = dr.GetString(5);
                                        TransDB = dr.GetString(6);
                                    }
                                    dr.Dispose();
                                    Email_inser.Dispose();

                                    string Destination = Session["path"].ToString() + Session["OriginalFilename"];
                                    using (new SoddingNetworkAuth(Userid, domain, password))
                                    {
                                        try
                                        {
                                            string folderPath = Server.MapPath("~/Files/");
                                            if (!Directory.Exists(folderPath))
                                            {
                                                Directory.CreateDirectory(folderPath);
                                            }
                                            string filename = "";
                                            string PathAndFileName = "";
                                            if (FlAttachment.HasFile)
                                            {
                                                filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                                                PathAndFileName = folderPath + filename;
                                                FlAttachment.SaveAs(PathAndFileName);
                                            }
                                            Source = PathAndFileName;
                                            File.Copy(Source, Destination, true);
                                            SendFilename = fname.Remove(fname.Length - 4);
                                            Session["SendFilename"] = fname.Remove(fname.Length - 4);
                                            OriginalFilename = Session["OriginalFilename"].ToString();
                                            OriginalFilename = OriginalFilename.Remove(OriginalFilename.Length - 4);
                                            Session["OriginalFilename"] = OriginalFilename.ToString();
                                            format = "pdf";
                                            FileInfo file = new FileInfo(PathAndFileName);
                                            if (file.Exists) //check file exsit or not  
                                            {
                                                file.Delete();
                                            }
                                        }
                                        catch (Exception xw1)
                                        {
                                            IsAttachFile = false;
                                            SendFilename = "NOFILE";
                                            OriginalFilename = "NOFILE";
                                            Session["OriginalFilename"] = "NOFILE";
                                            Session["SendFilename"] = "NOFILE";
                                            format = "NO";
                                            errmsg += "Quote NO : " + QuoteNoNew + "Mail Attachment Failed: " + "Msg Err :" + xw1.Message.ToString() + "\n";
                                            //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Attachment Failed: " + xw1 + " ");
                                            //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                        }
                                    }
                                }
                            }
                            catch (Exception x)
                            {
                                string message = x.Message;
                            }
                        }

                        else
                        {
                            IsAttachFile = false;
                            SendFilename = "NOFILE";
                            OriginalFilename = "NOFILE";
                            Session["OriginalFilename"] = "NOFILE";
                            Session["SendFilename"] = "NOFILE";
                            format = "NO";
                        }
                        #endregion

                        #region getting vendor mail id
                        aemail = string.Empty;
                        pemail = string.Empty;
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            cnn.Open();
                            try
                            {
                                string returnValue = string.Empty;
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Email_vendordetails";

                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.Decimal);
                                vendorid.Direction = ParameterDirection.Input;
                                vendorid.Value = dtGvTemp.Rows[(t)]["VendorCode1"].ToString();
                                cmdget.Parameters.Add(vendorid);

                                SqlParameter plant = new SqlParameter("@plant", SqlDbType.NVarChar);
                                plant.Direction = ParameterDirection.Input;
                                plant.Value = Session["EPlant"].ToString();
                                cmdget.Parameters.Add(plant);

                                SqlDataReader dr;
                                dr = cmdget.ExecuteReader();
                                while (dr.Read())
                                {
                                    aemail = dr.GetString(0);
                                    Session["aemail"] = dr.GetString(0);
                                    pemail = dr.GetString(1);
                                    Session["pemail"] = dr.GetString(1);

                                }
                                dr.Dispose();
                            }
                            catch (Exception exx)
                            {
                                errmsg += "Quote NO : " + QuoteNoNew + "function getting vendor mail id," + "Msg Err :" + exx.Message.ToString() + "\n";
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        #region getting User mail id
                        using (SqlConnection cnn = new SqlConnection(EMETModule.GenMDMConnString()))
                        {
                            string returnValue = string.Empty;
                            cnn.Open();
                            try
                            {
                                SqlCommand cmdget = cnn.CreateCommand();
                                cmdget.CommandType = CommandType.StoredProcedure;
                                cmdget.CommandText = "dbo.Emet_Email_userdetails";

                                SqlParameter vendorid = new SqlParameter("@id", SqlDbType.VarChar, 50);
                                vendorid.Direction = ParameterDirection.Input;
                                //vendorid.Value = userId;
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
                            }
                            catch (Exception exc)
                            {
                                errmsg += "Quote : " + dtGvTemp.Rows[t]["QuoteNo"].ToString() + "function :getting User mail id " + "Msg Err :" + exc.Message.ToString() + "\n";
                            }
                            finally
                            {
                                cnn.Dispose();
                            }
                        }
                        #endregion

                        #region Insert header and details to Mil server table to IT mailserverapp
                        try
                        {
                            using (SqlConnection Email_inser = new SqlConnection(EMETModule.GenMailConnString()))
                            {
                                Email_inser.Open();
                                //Header
                                string MessageHeaderId = Session["MHid"].ToString();
                                string fromname = "eMET System";
                                //string FromAddress = Uemail;
                                string FromAddress = "eMET@Shimano.Com.sg";
                                //string Recipient = aemail + "," + pemail;
                                aemail = Session["aemail"].ToString();
                                pemail = Session["pemail"].ToString();
                                Uemail = Session["Uemail"].ToString();
                                pemail = string.Concat(aemail, ";", pemail);
                                string Recipient = pemail;
                                string CopyRecipient = Uemail;
                                string BlindCopyRecipient = "";
                                string ReplyTo = "subashdurai@shimano.com.sg";
                                string Subject = "MailCenter from MES";
                                string footer = "Please <a href=" + Convert.ToString(URL.ToString()) + ">Login</a> SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br /><font  color='red'>This is System generated mail.  Please do not reply to this message.</font>";
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation <font color='red'> (Revision) </font> been created by Plant: " + Session["EPlant"].ToString() + " by " + lbluser1.Text + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dtGvTemp.Rows[(t)]["VendorName"].ToString() + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNoNew + "<br />  Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response Due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + dtGvTemp.Rows[(t)]["VendorName"].ToString() + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNoNew + " <br /> Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                string BodyFormat = "HTML";
                                string BodyRemark = "0";
                                string Signature = "";
                                string Importance = "High";
                                string Sensitivity = "Confidential";
                                string CreateUser = lbluser1.Text;
                                DateTime CreateDate = Convert.ToDateTime(DateTime.Now);
                                //end Header

                                SqlTransaction transactionHe;
                                transactionHe = Email_inser.BeginTransaction("Header");
                                try
                                {
                                    string Head = "insert into ctMessageHeader(MessageHeaderId, fromname,FromAddress,Recipient,CopyRecipient,BlindCopyRecipient, ReplyTo,Subject,body,BodyFormat,BodyRemark,Signature,Importance,Sensitivity,IsAttachFile,CreateUser,CreateDate) values(@MessageHeaderId,@fromname,@FromAddress,@Recipient,@CopyRecipient,@BlindCopyRecipient,@ReplyTo,@Subject,@body,@BodyFormat,@BodyRemark,@Signature,@Importance,@Sensitivity,@IsAttachFile,@CreateUser,@CreateDate)";
                                    SqlCommand Header = new SqlCommand(Head, Email_inser);
                                    Header.Connection = Email_inser;
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
                                    Header.Parameters.AddWithValue("@IsAttachFile", Convert.ToBoolean(IsAttachFile.ToString()));
                                    Header.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                    Header.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    Header.CommandText = Head;
                                    Header.ExecuteNonQuery();
                                    transactionHe.Commit();
                                    //end Header
                                    //Details
                                }
                                catch (Exception cc2)
                                {
                                    transactionHe.Rollback();
                                    errmsg += "Quote : " + QuoteNoNew + "failed sending email Header " + "Msg Err :" + cc2.Message.ToString() + "\n";
                                    //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email Header: " + cc2 + " ");
                                    //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }

                                SqlTransaction transactionDe;
                                transactionDe = Email_inser.BeginTransaction("Detail");
                                try
                                {
                                    if (Session["SendFilename"] != null)
                                    {
                                        SendFilename = Session["SendFilename"].ToString();
                                    }
                                    string Details = "insert into ctMessageDetail(MessageHeaderId, SequenceNumber,OriginalFilename,OriginalFileExtension,SendFilename,sendfileextension,CreateUser, CreateDate) values(@MessageHeaderId,@SequenceNumber,@OriginalFilename,@OriginalFileExtension,@SendFilename,@sendfileextension,@CreateUser,@CreateDate)";
                                    SqlCommand Detail = new SqlCommand(Details, Email_inser);
                                    Detail.Connection = Email_inser;
                                    Detail.Transaction = transactionDe;
                                    Detail.Parameters.AddWithValue("@MessageHeaderId", Session["MHid"].ToString());
                                    Detail.Parameters.AddWithValue("@SequenceNumber", Convert.ToInt32(SequenceNumber.ToString()));
                                    Detail.Parameters.AddWithValue("@OriginalFilename", Session["OriginalFilename"].ToString());
                                    Detail.Parameters.AddWithValue("@OriginalFileExtension", format.ToString());
                                    Detail.Parameters.AddWithValue("@SendFilename", SendFilename.ToString());
                                    Detail.Parameters.AddWithValue("@sendfileextension", format.ToString());
                                    Detail.Parameters.AddWithValue("@CreateUser", CreateUser.ToString());
                                    Detail.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                    Detail.CommandText = Details;
                                    Detail.ExecuteNonQuery();
                                    transactionDe.Commit();
                                }
                                catch (Exception cc1)
                                {
                                    transactionDe.Rollback();
                                    errmsg += "Quote : " + QuoteNoNew + " failed sending email detail: " + "Msg Err :" + cc1.Message.ToString() + "\n";
                                    //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email detail: " + cc1 + " ");
                                    //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                }
                                Email_inser.Dispose();
                                
                                using (SqlConnection Email_inser1 = new SqlConnection(EMETModule.GenEMETConnString()))
                                {
                                    Email_inser1.Open();
                                    try
                                    {
                                        transaction11 = Email_inser1.BeginTransaction("Mail11");
                                        string Details = "insert into email(quotenumber, body) values(@Quotenumber,@body)";
                                        SqlCommand Detail = new SqlCommand(Details, Email_inser1);
                                        Detail.Connection = Email_inser1;
                                        Detail.Transaction = transaction11;
                                        Detail.Parameters.AddWithValue("@Quotenumber", QuoteNoNew);
                                        Detail.Parameters.AddWithValue("@body", body1.ToString());
                                        Detail.CommandText = Details;
                                        Detail.ExecuteNonQuery();
                                        transaction11.Commit();
                                    }
                                    catch (Exception cc1)
                                    {
                                        transaction11.Rollback();
                                        errmsg += "Quote : " + QuoteNoNew + " Mail Content Issue in Transaction table " + "Msg Err :" + cc1.Message.ToString() + "\n";
                                        //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Mail Content Issue in Transaction table: " + cc1 + " ");
                                        //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                                    }
                                    Email_inser1.Dispose();
                                }
                                //End Details
                            }

                        }
                        catch (Exception cc1)
                        {
                            errmsg += "Quote : " + QuoteNoNew + "failed sending email " + "Msg Err :" + cc1.Message.ToString() + "\n";
                            //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("failed sending email : " + cc1 + " ");
                            //var script = string.Format("alert({0});window.location ='Home.aspx';", message);
                            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                        }
                        #endregion
                    }
                }
                
            }
            catch (Exception ex)
            {
                errmsg += "failed sending email from beginning process" + "Msg Err :" + ex.Message.ToString() + "\n";
            }

            if (errmsg != "")
            {
                sendingmail = false;
            }
            else
            {
                sendingmail = true;
            }
        }
        
        protected void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GetQuoteList();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        
        protected void EmptyForm()
        {
            //TxtQuoteNo.Text = "";
            //DdlReason.SelectedIndex = 0;
            //TxtSAPPartCode.Text = "";
            //TxtSAPPartDesc.Text = "";
            //TxtMaterialType.Text = "";
            //TxtPlantStatus.Text = "";
            //TxtSAPProcType.Text = "";
            //TxtSAPSpProcType.Text = "";
            //Txtpirtype.Text = "";
            //txtPIRDesc.Text = "";
            //txtunitweight.Text = "";
            //txtUOM.Text = "";
            //txtMQty.Text = "";
            //txtBaseUOM1.Text = "";
            //txtplatingtype.Text = "";
        }

        protected void clearSeason()
        {
            Session["InvalidRequest"] = null;
            Session["GvQuoteRefList"] = null;
            Session["GvQuoteRefListTemp"] = null;
            Session["GvAction"] = null;
            Session["RequestIncNumber1"] = null;
            Session["GvTemp"] = null;
            Session["flag"] = null;
            Session["InvalidRequestExpired"] = null;
            Session["InvalidRequest"] = null;
            Session["DtMaterialsDetails"] = null;
            Session["CreateReqTemp"] = null;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            DeleteNonRequest();
            Response.Redirect("Revision.aspx");
        }

        protected void BtnCreateReq_Click(object sender, EventArgs e)
        {
            DeleteNonRequest();
            UpdateDtGvTemp();

            bool Continue = true;
            string MsgError = "";
            if (Session["GvTemp"] != null) {
                DataTable dtGvTemp = (DataTable)Session["GvTemp"];
                if (dtGvTemp.Rows.Count > 0) {
                    for (int i = 0; i < dtGvTemp.Rows.Count; i++)
                    {
                        string VndCode = dtGvTemp.Rows[i]["VendorCode1"].ToString();
                        string Vnddesc = dtGvTemp.Rows[i]["VendorName"].ToString();
                        string procgrp = dtGvTemp.Rows[i]["ProcessGroup"].ToString();
                        string EfectivDate = dtGvTemp.Rows[i]["EffectiveDate"].ToString();
                        string QuoteRef = dtGvTemp.Rows[i]["QuoteNo"].ToString();
                        string IsUseMacAmor = dtGvTemp.Rows[i]["IsUseMachineAmor"].ToString();
                        if (IsUseMacAmor == "1") {
                            if (GetDataVndMachineAmor(VndCode, procgrp, EfectivDate) == false)
                            {
                                MsgError += @" Vendor "+ VndCode + "-" + Vnddesc + " with Quote No Ref "+ QuoteRef + " Do not have Machine Amortize !! \n ";
                                Continue = false;
                            }
                        }
                    }
                }
            }

            if (Continue == true)
            {
                createRequest();
                if (Session["CreateReqTemp"] != null)
                {
                    DataTable dtCreateReqTemp = (DataTable)Session["CreateReqTemp"];
                    GvCreateReqTemp.DataSource = dtCreateReqTemp;
                    GvCreateReqTemp.DataBind();
                }
            }
            else
            {
                DataTable dtCreateReqTemp = new DataTable();
                GvCreateReqTemp.DataSource = dtCreateReqTemp;
                GvCreateReqTemp.DataBind();

                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(MsgError);
                var script = string.Format("alert({0});CloseLoading();", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {

        }
        

        bool CheckQuoteIsSelect()
        {
            try
            {
                bool IsCheck = false;
                if (GvQuoteRefList.Rows.Count > 0)
                {
                    for (int i = 0; i < GvQuoteRefList.Rows.Count; i++)
                    {
                        CheckBox chkMatRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkMatRef");
                        CheckBox chkProcRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkProcRef");
                        CheckBox chkSubMatRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkSubMatRef");
                        CheckBox chkOthRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkOthRef");
                        CheckBox chkToolAmortizeRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkToolAmortizeRef");
                        CheckBox chkMachineAmortizeRef = (CheckBox)GvQuoteRefList.Rows[i].FindControl("chkMachineAmortizeRef");

                        if (chkMatRef.Checked == true || chkProcRef.Checked == true || chkSubMatRef.Checked == true || chkOthRef.Checked == true || chkToolAmortizeRef.Checked == true || chkMachineAmortizeRef.Checked == true)
                        {
                            IsCheck = true;
                            break;
                        }
                    }

                    if (IsCheck == true)
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
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return false;
            }
        }

        protected void SpanRowGvInvalidExpired()
        {
            try
            {
                int RowSpan = 2;
                for (int i = GvDuplicateWithExpiredReq.Rows.Count - 2; i >= 0; i--)
                {
                    GridViewRow currRow = GvDuplicateWithExpiredReq.Rows[i];
                    GridViewRow prevRow = GvDuplicateWithExpiredReq.Rows[i + 1];
                    if (currRow.Cells[1].Text == prevRow.Cells[1].Text)
                    {
                        currRow.Cells[0].RowSpan = RowSpan;
                        prevRow.Cells[0].Visible = false;

                        currRow.Cells[1].RowSpan = RowSpan;
                        prevRow.Cells[1].Visible = false;

                        currRow.Cells[2].RowSpan = RowSpan;
                        prevRow.Cells[2].Visible = false;

                        currRow.Cells[4].RowSpan = RowSpan;
                        prevRow.Cells[4].Visible = false;

                        currRow.Cells[5].RowSpan = RowSpan;
                        prevRow.Cells[5].Visible = false;

                        currRow.Cells[8].RowSpan = RowSpan;
                        prevRow.Cells[8].Visible = false;

                        currRow.Cells[9].RowSpan = RowSpan;
                        prevRow.Cells[9].Visible = false;

                        currRow.Cells[10].RowSpan = RowSpan;
                        prevRow.Cells[10].Visible = false;

                        RowSpan += 1;
                    }
                    else
                    {
                        RowSpan = 2;
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        bool CekVendorVsMaterialExpiredReq(string Vendor, string material)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + "' and ApprovalStatus = 0 and format(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
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
                            using (SqlDataAdapter sda2 = new SqlDataAdapter())
                            {
                                sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                                        QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                                        from TQuoteDetails where RequestNumber=@RequestNumber ";

                                cmd = new SqlCommand(sql, EmetCon);
                                cmd.Parameters.AddWithValue("@RequestNumber", dt.Rows[0]["RequestNumber"].ToString());
                                sda2.SelectCommand = cmd;
                                using (DataTable dt2 = new DataTable())
                                {
                                    sda2.Fill(dt2);
                                    if (Session["InvalidRequestExpired"] == null)
                                    {
                                        Session["InvalidRequestExpired"] = dt2;
                                    }
                                    else
                                    {
                                        DataTable DtTemp = (DataTable)Session["InvalidRequestExpired"];
                                        if (dt2.Rows.Count > 0)
                                        {
                                            bool isExist = false;
                                            for (int L = 0; L < DtTemp.Rows.Count; L++)
                                            {
                                                if (dt2.Rows[0]["RequestNumber"].ToString() == DtTemp.Rows[L]["RequestNumber"].ToString())
                                                {
                                                    isExist = true;
                                                    break;
                                                }
                                            }

                                            if (isExist == false)
                                            {
                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                {
                                                    DataRow dr = DtTemp.NewRow();
                                                    dr["Plant"] = dt2.Rows[i]["Plant"].ToString();
                                                    dr["RequestNumber"] = dt2.Rows[i]["RequestNumber"].ToString();
                                                    dr["RequestDate"] = dt2.Rows[i]["RequestDate"].ToString();
                                                    dr["QuoteResponseDueDate"] = dt2.Rows[i]["QuoteResponseDueDate"].ToString();
                                                    dr["QuoteNo"] = dt2.Rows[i]["QuoteNo"].ToString();
                                                    dr["Material"] = dt2.Rows[i]["Material"].ToString();
                                                    dr["MaterialDesc"] = dt2.Rows[i]["MaterialDesc"].ToString();
                                                    dr["VendorCode1"] = dt2.Rows[i]["VendorCode1"].ToString();
                                                    dr["VendorName"] = dt2.Rows[i]["VendorName"].ToString();
                                                    DtTemp.Rows.Add(dr);
                                                    DtTemp.AcceptChanges();
                                                }
                                            }
                                        }
                                        Session["InvalidRequestExpired"] = DtTemp;
                                    }
                                }
                            }
                            return false;

                            //if (Session["InvalidRequestExpired"] == null)
                            //{
                            //    Session["InvalidRequestExpired"] = dt;
                            //}
                            //else
                            //{
                            //    DataTable DtTemp = (DataTable)Session["InvalidRequestExpired"];
                            //    DataRow dr = DtTemp.NewRow();
                            //    dr["Plant"] = dt.Rows[0]["Plant"].ToString();
                            //    dr["RequestNumber"] = dt.Rows[0]["RequestNumber"].ToString();
                            //    dr["RequestDate"] = dt.Rows[0]["RequestDate"].ToString();
                            //    dr["QuoteResponseDueDate"] = dt.Rows[0]["QuoteResponseDueDate"].ToString();
                            //    dr["QuoteNo"] = dt.Rows[0]["QuoteNo"].ToString();
                            //    dr["Material"] = dt.Rows[0]["Material"].ToString();
                            //    dr["MaterialDesc"] = dt.Rows[0]["MaterialDesc"].ToString();
                            //    dr["VendorCode1"] = dt.Rows[0]["VendorCode1"].ToString();
                            //    dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                            //    DtTemp.Rows.Add(dr);
                            //    DtTemp.AcceptChanges();
                            //    Session["InvalidRequestExpired"] = DtTemp;
                            //}
                            //return false;
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

        bool CekVendorVsMaterial(string Vendor, string material, string QuoteNoRef)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            --from TQuoteDetails where Material = @Material and VendorCode1 =@VendorCode1 and (ApprovalStatus in ('0','2'))  and format(QuoteResponseDueDate,'yyyy-MM-dd') >= FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') 
                            from TQuoteDetails where Material = '" + material + "' and VendorCode1 ='" + Vendor + "' and ApprovalStatus in ('0','2') ";

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@Material", material);
                    cmd.Parameters.AddWithValue("@VendorCode1", Vendor);
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
                                dt.Columns.Add("QuoteNoRef", typeof(System.String));
                                foreach (DataRow row in dt.Rows)
                                {
                                    //need to set value to NewColumn column
                                    row["QuoteNoRef"] = QuoteNoRef;   // or set it to some other value
                                }
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
                                dr["VendorName"] = dt.Rows[0]["VendorName"].ToString();
                                dr["QuoteNoRef"] = QuoteNoRef;
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

        

        protected void BtnAddToList_Click(object sender, EventArgs e)
        {
            try
            {
                //GridView GvTemp = new GridView();
                //GvTemp = GvQuoteRefList;


                Session["InvalidRequestExpired"] = null;
                //Session["InvalidRequest"] = null;
                
                GvInvalidRequest.DataSource = null;
                GvInvalidRequest.DataBind();

                DataTable DtGvQuoteRefList = new DataTable();
                if (Session["GvQuoteRefList"] != null)
                {
                    DtGvQuoteRefList = (DataTable)Session["GvQuoteRefList"];
                }

                if (CheckQuoteIsSelect() == true)
                {
                    if (DtGvQuoteRefList.Rows.Count > 0)
                    {

                        GridView TempGvQuoteRefList = new GridView();
                        TempGvQuoteRefList.DataSource = DtGvQuoteRefList;
                        TempGvQuoteRefList.DataBind();
                        DataTable dt = new DataTable();

                        #region create header for data table gvTemp
                        if (Session["GvTemp"] == null)
                        {
                            for (int i = 1; i < TempGvQuoteRefList.HeaderRow.Cells.Count; i++)
                            {
                                dt.Columns.Add(TempGvQuoteRefList.HeaderRow.Cells[i].Text);
                            }
                            dt.Columns.Add("IsAllcostAllow");
                            dt.Columns.Add("IsMatcostAllow");
                            dt.Columns.Add("IsProccostAllow");
                            dt.Columns.Add("IsSubMatcostAllow");
                            dt.Columns.Add("IsOthcostAllow");
                            dt.Columns.Add("IsUseToolAmor");
                            dt.Columns.Add("IsUseMachineAmor");
                            dt.Columns.Add("Reason");
                            dt.Columns.Add("Remark");
                            dt.Columns.Add("MnthEstQty");
                            dt.Columns.Add("BsUOM");
                            dt.Columns.Add("Attachment");
                            dt.Columns.Add("DueDate");
                            dt.Columns.Add("EffectiveDate");
                            dt.Columns.Add("DueDateNextRev");
                            dt.Columns.Add("RecycleRatio");
                            Session["GvTemp"] = dt;
                        }
                        #endregion

                        if (Session["GvTemp"] != null)
                        {
                            dt = (DataTable)Session["GvTemp"];
                            if (TempGvQuoteRefList.Rows.Count > 0)
                            {
                                bool Ishaverequestexpiredpending = false;
                                for (int r = 0; r < GvQuoteRefList.Rows.Count; r++)
                                {
                                    Label LbQNModal = (Label)GvQuoteRefList.Rows[r].FindControl("LbQNModal");
                                    CheckBox chkAllRefRw = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkAllRefRw");
                                    CheckBox chkMatRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkMatRef");
                                    CheckBox chkProcRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkProcRef");
                                    CheckBox chkSubMatRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkSubMatRef");
                                    CheckBox chkOthRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkOthRef");

                                    CheckBox chkToolAmortizeRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkToolAmortizeRef");
                                    CheckBox chkMachineAmortizeRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkMachineAmortizeRef");
                                    if (chkMatRef.Checked == true || chkProcRef.Checked == true || chkSubMatRef.Checked == true || chkOthRef.Checked == true || chkToolAmortizeRef.Checked == true || chkMachineAmortizeRef.Checked == true)
                                    {
                                        if (CekVendorVsMaterialExpiredReq(GvQuoteRefList.Rows[r].Cells[1].Text, GvQuoteRefList.Rows[r].Cells[5].Text) == true)
                                        {
                                            if (Session["InvalidRequestExpired"] != null)
                                            {
                                                Ishaverequestexpiredpending = true;
                                            }
                                        }
                                    }
                                }

                                if (Ishaverequestexpiredpending == false)
                                {
                                    for (int r = 0; r < GvQuoteRefList.Rows.Count; r++)
                                    {
                                        Label LbQNModal = (Label)GvQuoteRefList.Rows[r].FindControl("LbQNModal");
                                        CheckBox chkAllRefRw = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkAllRefRw");
                                        CheckBox chkMatRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkMatRef");
                                        CheckBox chkProcRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkProcRef");
                                        CheckBox chkSubMatRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkSubMatRef");
                                        CheckBox chkOthRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkOthRef");

                                        CheckBox chkToolAmortizeRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkToolAmortizeRef");
                                        CheckBox chkMachineAmortizeRef = (CheckBox)GvQuoteRefList.Rows[r].FindControl("chkMachineAmortizeRef");

                                        if (chkMatRef.Checked == true || chkProcRef.Checked == true || chkSubMatRef.Checked == true || chkOthRef.Checked == true || chkToolAmortizeRef.Checked == true || chkMachineAmortizeRef.Checked == true)
                                        {
                                            if (CekVendorVsMaterial(GvQuoteRefList.Rows[r].Cells[1].Text, GvQuoteRefList.Rows[r].Cells[5].Text, LbQNModal.Text) == true)
                                            {
                                                if (Session["InvalidRequestExpired"] == null)
                                                {
                                                    #region save data to datatable
                                                    bool IsQuoteNoExistinTempTable = false;
                                                    for (int h = 0; h < dt.Rows.Count; h++)
                                                    {
                                                        if (dt.Rows[h]["QuoteNo"].ToString() == LbQNModal.Text)
                                                        {
                                                            IsQuoteNoExistinTempTable = true;
                                                            break;
                                                        }
                                                    }

                                                    if (IsQuoteNoExistinTempTable == false)
                                                    {
                                                        GridViewRow row = TempGvQuoteRefList.Rows[r];
                                                        DataRow dr = dt.NewRow();
                                                        for (int j = 1; j < row.Cells.Count; j++)
                                                        {
                                                            dr[TempGvQuoteRefList.HeaderRow.Cells[j].Text] = row.Cells[j].Text.Replace("&nbsp;", "");
                                                        }
                                                        if (chkAllRefRw.Checked == true)
                                                        {
                                                            dr["IsAllcostAllow"] = "1";
                                                        }
                                                        else
                                                        {
                                                            dr["IsAllcostAllow"] = "0";
                                                        }
                                                        if (chkMatRef.Checked == true)
                                                        {
                                                            dr["IsMatcostAllow"] = "1";
                                                        }
                                                        else
                                                        {
                                                            dr["IsMatcostAllow"] = "0";
                                                        }
                                                        if (chkProcRef.Checked == true)
                                                        {
                                                            dr["IsProccostAllow"] = "1";
                                                        }
                                                        else
                                                        {
                                                            dr["IsProccostAllow"] = "0";
                                                        }
                                                        if (chkSubMatRef.Checked == true)
                                                        {
                                                            dr["IsSubMatcostAllow"] = "1";
                                                        }
                                                        else
                                                        {
                                                            dr["IsSubMatcostAllow"] = "0";
                                                        }
                                                        if (chkOthRef.Checked == true)
                                                        {
                                                            dr["IsOthcostAllow"] = "1";
                                                        }
                                                        else
                                                        {
                                                            dr["IsOthcostAllow"] = "0";
                                                        }

                                                        if (chkToolAmortizeRef.Checked == true){ dr["IsUseToolAmor"] = "1";}else{dr["IsUseToolAmor"] = "0";}
                                                        if (chkMachineAmortizeRef.Checked == true) { dr["IsUseMachineAmor"] = "1"; } else { dr["IsUseMachineAmor"] = "0"; }

                                                        dr["Reason"] = "";
                                                        dr["Remark"] = "";
                                                        dr["MnthEstQty"] = "";
                                                        dr["Attachment"] = "";
                                                        dr["DueDate"] = "";
                                                        dr["EffectiveDate"] = "";
                                                        dr["DueDateNextRev"] = DateDuenextRev().ToString("dd-MM-yyyy");
                                                        dr["RecycleRatio"] = "";
                                                        dt.Rows.Add(dr);

                                                        int c = (dt.Rows.Count) - 1;
                                                        if (Session["FlAtc" + LbQNModal.Text] != null)
                                                        {
                                                            Session["FlAtc" + LbQNModal.Text] = null;
                                                        }
                                                        Session["GvTemp"] = dt;

                                                        GvTemp.DataSource = (DataTable)Session["GvTemp"];
                                                        GvTemp.DataBind();
                                                        TxtGvTempLeng.Text = dt.Rows.Count.ToString();
                                                        Session["GvTemp"] = (DataTable)GvTemp.DataSource;
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //if (Session["InvalidRequestExpired"] == null)
                            //{
                            //    GetQuoteList();
                            //    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "OpenModalConfirm();CloseLoading();", true);
                            //}
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No Data selected !');CloseLoading();", true);
                }

                if (GvTemp.Rows.Count > 0)
                {
                    BtnCreateReq.Visible = true;
                    //LbTitleQrefList.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ChgEmptyFlColor();", true);
                }
                else
                {
                    BtnCreateReq.Visible = false;
                    //LbTitleQrefList.Visible = false;
                }


                if (Session["InvalidRequestExpired"] != null)
                {
                    DataTable DtTemp = (DataTable)Session["InvalidRequestExpired"];
                    if (DtTemp.Rows.Count > 0)
                    {
                        DtTemp.DefaultView.Sort = "RequestNumber Desc";
                        GvDuplicateWithExpiredReq.DataSource = DtTemp;
                        GvDuplicateWithExpiredReq.DataBind();
                        SpanRowGvInvalidExpired();

                        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "DatePitcker();CloseLoading();OpenModalDuplicateExpired();", true);
                    }
                }
                else
                {
                    if (Session["InvalidRequest"] != null)
                    {
                        DataTable DtTemp = (DataTable)Session["InvalidRequest"];
                        if (DtTemp.Rows.Count > 0)
                        {
                            GvInvalidRequest.DataSource = DtTemp;
                            GvInvalidRequest.DataBind();
                            //DvInvalidRequest.Visible = true;
                        }
                        else
                        {
                            //DvInvalidRequest.Visible = false;
                        }
                    }
                    else
                    {
                        //DvInvalidRequest.Visible = false;
                    }

                    GetQuoteList();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "OpenModalConfirm();CloseLoading();", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void ProcessDuplicateReqWithOldNExpiredReq()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = "";
                if (GvDuplicateWithExpiredReq.Rows.Count > 0)
                {
                    string PrevReqNo = "";
                    for (int i = 0; i < GvDuplicateWithExpiredReq.Rows.Count; i++)
                    {
                        RadioButton RbReject = GvDuplicateWithExpiredReq.Rows[i].FindControl("RbReject") as RadioButton;
                        RadioButton RbChangeDate = GvDuplicateWithExpiredReq.Rows[i].FindControl("RbChangeDate") as RadioButton;
                        TextBox TxtNewDueDate = GvDuplicateWithExpiredReq.Rows[i].FindControl("TxtNewDueDate") as TextBox;

                        if (RbReject != null && RbChangeDate != null && TxtNewDueDate != null)
                        {
                            string ReqNo = GvDuplicateWithExpiredReq.Rows[i].Cells[1].Text;
                            if (PrevReqNo != ReqNo)
                            {
                                string QuNo = GvDuplicateWithExpiredReq.Rows[i].Cells[3].Text;
                                DateTime DtDueDate = DateTime.ParseExact(TxtNewDueDate.Text, "dd-MM-yyyy", null);
                                string StrNewQuoteResponseDueDate = DtDueDate.ToString("yyyy-MM-dd");
                                if (RbReject.Checked == true)
                                {
                                    sql += @" Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + @"',  --PICReason = 'New Requset Created with Req NO : " + ReqNo + @"', 
                                        ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + "', UpdatedBy='" + Session["userID"].ToString() + "', UpdatedOn='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"' 
                                      where RequestNumber = '" + ReqNo + "' ";
                                }
                                else
                                {
                                    sql += @" update TQuoteDetails set QuoteResponseDueDate = '" + StrNewQuoteResponseDueDate + "' where RequestNumber = '" + ReqNo + "' ";
                                }
                                PrevReqNo = ReqNo;
                            }
                        }
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
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

        protected void BtnSubmitProcDuplicateReg_Click(object sender, EventArgs e)
        {
            ProcessDuplicateReqWithOldNExpiredReq();
            BtnAddToList_Click(BtnAddToList, EventArgs.Empty);
        }

        protected void GvlayoutList_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GvCreateReqTemp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;
                    string ValidFrom = e.Row.Cells[18].Text;
                    string CusMatValFrom = e.Row.Cells[19].Text;
                    string CusMatValTo = e.Row.Cells[20].Text;

                    if (ValidFrom.Replace("&nbsp;", "").Trim() != "")
                    {
                        DateTime dt = DateTime.ParseExact(ValidFrom.Trim(), "yyyy-MM-dd", null);
                        e.Row.Cells[18].Text = dt.ToString("yyyy-MM-dd");
                    }

                    if (CusMatValFrom.Replace("&nbsp;", "").Trim() != "")
                    {
                        DateTime dt = DateTime.ParseExact(CusMatValFrom.Trim(), "yyyy-MM-dd", null);
                        e.Row.Cells[19].Text = dt.ToString("dd-MM-yyyy");
                    }

                    if (CusMatValTo.Replace("&nbsp;", "").Trim() != "")
                    {
                        DateTime dt = DateTime.ParseExact(CusMatValTo.Trim(), "yyyy-MM-dd", null);
                        e.Row.Cells[20].Text = dt.ToString("dd-MM-yyyy");
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GvDuplicateWithExpiredReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //DataRowView dr = (DataRowView)e.Row.DataItem;
                    //string NewreqNo = dr["RequestNumber"].ToString();

                    int RowParentGv = e.Row.DataItemIndex;
                    RadioButton RbReject = e.Row.FindControl("RbReject") as RadioButton;
                    RbReject.Attributes.Add("onchange", "RbRejectExpReq(" + RowParentGv + ")");

                    RadioButton RbChangeDate = e.Row.FindControl("RbChangeDate") as RadioButton;
                    RbChangeDate.Attributes.Add("onchange", "RbChangedateResDueDate(" + RowParentGv + ")");

                    TextBox TxtNewDueDate = e.Row.FindControl("TxtNewDueDate") as TextBox;
                    string TxtNewDueDateID = ((TextBox)e.Row.FindControl("TxtNewDueDate")).ClientID.ToString();

                    HtmlGenericControl IcnCalendarNewDueDate = ((HtmlGenericControl)(e.Row.FindControl("IcnCalendarNewDueDate")));
                    string IcnCalendarNewDueDateID = ((HtmlGenericControl)e.Row.FindControl("IcnCalendarNewDueDate")).ClientID.ToString();
                    if (IcnCalendarNewDueDate != null)
                    {
                        IcnCalendarNewDueDate.Attributes.Add("onclick", "FocusToTxt(" + RowParentGv + ",'" + TxtNewDueDateID + "')");
                    }
                }
            }
            catch (Exception ee)
            {
                LbMsgErr.Text = ee.StackTrace.ToString() + " - " + ee.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ee);
            }
        }

        protected void GvQuoteRefList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkAllRefHd = e.Row.FindControl("chkAllRefHd") as CheckBox;
                    chkAllMatRef = e.Row.FindControl("chkAllMatRef") as CheckBox;
                    chkAllProcRef = e.Row.FindControl("chkAllProcRef") as CheckBox;
                    chkAllSubMatRef = e.Row.FindControl("chkAllSubMatRef") as CheckBox;
                    chkAllOthRef = e.Row.FindControl("chkAllOthRef") as CheckBox;
                    
                    chkAllToolAmortizeRef = e.Row.FindControl("chkAllToolAmortizeRef") as CheckBox;
                    chkAllMachineAmortizeRef = e.Row.FindControl("chkAllMachineAmortizeRef") as CheckBox;

                    if (chkAllRefHd != null && chkAllMatRef != null && chkAllProcRef != null && chkAllSubMatRef != null && chkAllOthRef != null && chkAllToolAmortizeRef != null && chkAllMachineAmortizeRef != null)
                    {
                        string chkAllRefHdID = ((CheckBox)e.Row.FindControl("chkAllRefHd")).ClientID;
                        string chkAllMatRefID = ((CheckBox)e.Row.FindControl("chkAllMatRef")).ClientID;
                        string chkAllProcRefID = ((CheckBox)e.Row.FindControl("chkAllProcRef")).ClientID;
                        string chkAllSubMatRefID = ((CheckBox)e.Row.FindControl("chkAllSubMatRef")).ClientID;
                        string chkAllOthRefID = ((CheckBox)e.Row.FindControl("chkAllOthRef")).ClientID;

                        string chkAllToolAmortizeRefID = ((CheckBox)e.Row.FindControl("chkAllToolAmortizeRef")).ClientID;
                        string chkAllMachineAmortizeRefID = ((CheckBox)e.Row.FindControl("chkAllMachineAmortizeRef")).ClientID;

                        chkAllRefHd.Attributes.Add("onchange", "CheckorUncheckAllGvRefList();");
                        chkAllMatRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllMatCostRef();");
                        chkAllProcRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllProcCostRef();");
                        chkAllSubMatRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllSubMatCostRef();");
                        chkAllOthRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllOthCostRef();");

                        chkAllToolAmortizeRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllToolAmorRef();");
                        chkAllMachineAmortizeRef.Attributes.Add("onchange", "IsAllGvHdRefListCheck('" + chkAllRefHdID + "','" + chkAllMatRefID + "','" + chkAllProcRefID + "','" + chkAllSubMatRefID + "','" + chkAllOthRefID + "','" + chkAllToolAmortizeRefID + "','" + chkAllMachineAmortizeRefID + "');CheckAllmachineAmorRef();");
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label LbQNModal = e.Row.FindControl("LbQNModal") as Label;
                    string LbQNModalID = ((Label)e.Row.FindControl("LbQNModal")).ClientID;
                    string url = "QQPReview.aspx?Number=" + LbQNModal.Text;
                    LbQNModal.Attributes.Add("onclick", "openInNewTab2('" + url + "');");
                    //LbQNModal.Attributes.Add("onclick", "openInNewTab('QuoteCostPlan.aspx','" + LbQNModalID + "')");

                    CheckBox chkAllRefRw = e.Row.FindControl("chkAllRefRw") as CheckBox;
                    CheckBox ChkMatRowRef = e.Row.FindControl("chkMatRef") as CheckBox;
                    CheckBox chkProcRowRef = e.Row.FindControl("chkProcRef") as CheckBox;
                    CheckBox chkSubMatRowRef = e.Row.FindControl("chkSubMatRef") as CheckBox;
                    CheckBox chkOthRowRef = e.Row.FindControl("chkOthRef") as CheckBox;

                    CheckBox chkToolAmortizeRef = e.Row.FindControl("chkToolAmortizeRef") as CheckBox;
                    CheckBox chkMachineAmortizeRef = e.Row.FindControl("chkMachineAmortizeRef") as CheckBox;

                    DropDownList DdlToolAmortizeRef = e.Row.FindControl("DdlToolAmortizeRef") as DropDownList;
                    DropDownList DdlMachineAmortizeRef = e.Row.FindControl("DdlMachineAmortizeRef") as DropDownList;

                    if (chkAllRefRw!=null && ChkMatRowRef != null && chkProcRowRef != null && chkSubMatRowRef != null && chkOthRowRef != null && chkToolAmortizeRef != null && chkMachineAmortizeRef != null && DdlToolAmortizeRef != null && DdlMachineAmortizeRef != null)
                    {
                        DdlToolAmortizeRef.Attributes.Add("disabled", "disabled");
                        DdlMachineAmortizeRef.Attributes.Add("disabled", "disabled");

                        string chkAllRefRwfID = ((CheckBox)e.Row.FindControl("chkAllRefRw")).ClientID;
                        string ChkMatRowRefID = ((CheckBox)e.Row.FindControl("chkMatRef")).ClientID;
                        string chkProcRowRefID = ((CheckBox)e.Row.FindControl("chkProcRef")).ClientID;
                        string chkSubMatRowRefID = ((CheckBox)e.Row.FindControl("chkSubMatRef")).ClientID;
                        string chkOthRowRefID = ((CheckBox)e.Row.FindControl("chkOthRef")).ClientID;

                        string chkToolAmortizeRefID = ((CheckBox)e.Row.FindControl("chkToolAmortizeRef")).ClientID;
                        string chkMachineAmortizeRefID = ((CheckBox)e.Row.FindControl("chkMachineAmortizeRef")).ClientID;

                        string DdlToolAmortizeRefID = ((DropDownList)e.Row.FindControl("DdlToolAmortizeRef")).ClientID;
                        string DdlMachineAmortizeRefID = ((DropDownList)e.Row.FindControl("DdlMachineAmortizeRef")).ClientID;

                        chkAllRefRw.Attributes.Add("onchange", "chkAllRowRefClick('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllCostRowChecked();");
                        ChkMatRowRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllMatCostRefRowChecked();");
                        chkProcRowRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllProCostRefRowChecked();");
                        chkSubMatRowRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllSubMatCostRefRowChecked();");
                        chkOthRowRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllOthCostRefRowChecked();");

                        chkToolAmortizeRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllToolAmorRefRowChecked();");
                        chkMachineAmortizeRef.Attributes.Add("onchange", "IsAllGvRowRefListCheck('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "');IsAllMachineAmorRefRowChecked();");

                        DdlToolAmortizeRef.Attributes.Add("onchange", "DdlToolAmortizeRefChange('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "')");
                        DdlMachineAmortizeRef.Attributes.Add("onchange", "DdlMachineAmortizeRefChange('" + chkAllRefRwfID + "','" + ChkMatRowRefID + "','" + chkProcRowRefID + "','" + chkSubMatRowRefID + "','" + chkOthRowRefID + "','" + chkToolAmortizeRefID + "','" + chkMachineAmortizeRefID + "','" + DdlToolAmortizeRefID + "','" + DdlMachineAmortizeRefID + "')");

                        string PG = e.Row.Cells[7].Text;
                        string PLStts = e.Row.Cells[18].Text;
                        if (GetLAyoutFromProcGroup(PG) == "Layout7")
                        {
                            chkAllRefHd.Enabled = false;
                            chkAllProcRef.Enabled = false;
                            chkAllSubMatRef.Enabled = false;

                            chkAllRefRw.Enabled = false;
                            chkProcRowRef.Enabled = false;
                            chkSubMatRowRef.Enabled = false;

                            chkAllToolAmortizeRef.Enabled = false;
                            chkAllMachineAmortizeRef.Enabled = false;
                            chkToolAmortizeRef.Enabled = false;
                            chkMachineAmortizeRef.Enabled = false;

                            DdlToolAmortizeRef.Enabled = false;
                            DdlMachineAmortizeRef.Enabled = false;

                        }

                        if (PLStts.ToUpper() == "Z9" || PLStts.ToUpper() == "Z4") {
                            chkAllRefHd.Enabled = false;
                            chkAllMatRef.Enabled = false;
                            chkAllProcRef.Enabled = false;
                            chkAllSubMatRef.Enabled = false;
                            chkAllOthRef.Enabled = false;

                            chkAllRefRw.Enabled = false;
                            ChkMatRowRef.Enabled = false;
                            chkProcRowRef.Enabled = false;
                            chkSubMatRowRef.Enabled = false;
                            chkOthRowRef.Enabled = false;

                            chkAllToolAmortizeRef.Enabled = false;
                            chkAllMachineAmortizeRef.Enabled = false;
                            chkToolAmortizeRef.Enabled = false;
                            chkMachineAmortizeRef.Enabled = false;

                            DdlToolAmortizeRef.Enabled = false;
                            DdlMachineAmortizeRef.Enabled = false;
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

        protected void GvQuoteRefList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "QuoteDetails")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    Label LbQNModal = GvQuoteRefList.Rows[rowIndex].FindControl("LbQNModal") as Label;
                    string QuoteNo = LbQNModal.Text;

                    //Countryoforigin();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('QQPReview.aspx?Number=" + QuoteNo + "');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "OpenMdQuoteCostPlant();UnitCost();", true);
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvTemp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowParentGv = e.Row.DataItemIndex;
                    string QNo = e.Row.Cells[1].Text;
                    string ProcGroup = e.Row.Cells[6].Text;
                    string Material = e.Row.Cells[4].Text;
                    string layout = GetLAyoutFromProcGroup(ProcGroup);

                    #region Label cotrol
                    Label LbQNModal = e.Row.FindControl("LbQNModal") as Label;
                    string LbQNModalID = ((Label)e.Row.FindControl("LbQNModal")).ClientID;

                    string url = "QQPReview.aspx?Number=" + LbQNModal.Text;
                    LbQNModal.Attributes.Add("onclick", "openInNewTab2('" + url + "');");
                    //LbQNModal.Attributes.Add("onclick", "openInNewTab('QuoteCostPlan.aspx','" + LbQNModalID + "')");
                    #endregion

                    #region Cost check box & Ddl control
                    CheckBox chkAllCost = e.Row.FindControl("chkAllCost") as CheckBox;
                    CheckBox ChkMatCost = e.Row.FindControl("chkMatCost") as CheckBox;
                    CheckBox chkProcCost = e.Row.FindControl("chkProcCost") as CheckBox;
                    CheckBox chkSubMatCost = e.Row.FindControl("chkSubMatCost") as CheckBox;
                    CheckBox chkOthCost = e.Row.FindControl("chkOthCost") as CheckBox;

                    CheckBox chkToolAmortize = e.Row.FindControl("chkToolAmortize") as CheckBox;
                    CheckBox chkMachineAmortize = e.Row.FindControl("chkMachineAmortize") as CheckBox;

                    DropDownList DdlToolAmortize = e.Row.FindControl("DdlToolAmortize") as DropDownList;
                    DropDownList DdlMachineAmortize = e.Row.FindControl("DdlMachineAmortize") as DropDownList;

                    string chkAllCostID = ((CheckBox)e.Row.FindControl("chkAllCost")).ClientID;
                    string ChkMatCostID = ((CheckBox)e.Row.FindControl("chkMatCost")).ClientID;
                    string chkProcCostID = ((CheckBox)e.Row.FindControl("chkProcCost")).ClientID;
                    string chkSubMatCostID = ((CheckBox)e.Row.FindControl("chkSubMatCost")).ClientID;
                    string chkOthCostID = ((CheckBox)e.Row.FindControl("chkOthCost")).ClientID;

                    string chkToolAmortizeID = ((CheckBox)e.Row.FindControl("chkToolAmortize")).ClientID;
                    string chkMachineAmortizeID = ((CheckBox)e.Row.FindControl("chkMachineAmortize")).ClientID;

                    string DdlToolAmortizeID = ((DropDownList)e.Row.FindControl("DdlToolAmortize")).ClientID;
                    string DdlMachineAmortizeID = ((DropDownList)e.Row.FindControl("DdlMachineAmortize")).ClientID;

                    chkAllCost.Attributes.Add("onclick", "chkAllCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "')");
                    ChkMatCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "')");
                    chkProcCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "')");
                    chkSubMatCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "')");
                    chkOthCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "')");

                    
                    if (GetLAyoutFromProcGroup(e.Row.Cells[6].Text) == "Layout7")
                    {
                        chkAllCost.Enabled = false;
                        chkProcCost.Enabled = false;
                        chkSubMatCost.Enabled = false;
                        chkToolAmortize.Enabled = false;
                        chkMachineAmortize.Enabled = false;
                        DdlToolAmortize.Enabled = false;
                        DdlMachineAmortize.Enabled = false;
                    }
                    #endregion Cost check box control

                    #region Reason Control condition
                    DropDownList DdlReason = e.Row.FindControl("DdlReason") as DropDownList;
                    DropDownList DdlRecycleRatio = e.Row.FindControl("DdlRecycleRatio") as DropDownList;
                    TextBox TxtOthReason = e.Row.FindControl("TxtOthReason") as TextBox;
                    Label LblengtVC = e.Row.FindControl("LblengtVC") as Label;

                    string DdlReasonID = ((DropDownList)e.Row.FindControl("DdlReason")).ClientID;
                    string DdlRecycleRatioID = ((DropDownList)e.Row.FindControl("DdlRecycleRatio")).ClientID;
                    string TxtOthReasonID = ((TextBox)e.Row.FindControl("TxtOthReason")).ClientID;
                    string LblengtVCID = ((Label)e.Row.FindControl("LblengtVC")).ClientID;

                    TxtOthReason.Style.Add("display", "none");
                    LblengtVC.Style.Add("display", "none");
                    DdlReason.Attributes.Add("onchange", "DdlReasonchange('" + DdlReasonID + "','" + TxtOthReasonID + "','" + LblengtVCID + "')");
                    TxtOthReason.Attributes.Add("onkeyup", "RemarkLght('" + TxtOthReasonID + "','" + LblengtVCID + "')");
                    #endregion Reason Control condition

                    #region tetxbox control
                    TextBox TxtMQty = e.Row.FindControl("TxtMQty") as TextBox;
                    TextBox TxtBaseUOM = e.Row.FindControl("TxtBaseUOM") as TextBox;
                    TextBox TxtResDueDate = e.Row.FindControl("TxtResDueDate") as TextBox;
                    TextBox TxtEffectiveDate = e.Row.FindControl("TxtEffectiveDate") as TextBox;
                    TextBox TxtDueDateNextRev = e.Row.FindControl("TxtDueDateNextRev") as TextBox;
                    HtmlGenericControl IcnCalResduedate = ((HtmlGenericControl)(e.Row.FindControl("IcnCalResduedate")));
                    HtmlGenericControl IcnCalEffectiveDate = ((HtmlGenericControl)(e.Row.FindControl("IcnCalEffectiveDate")));
                    HtmlGenericControl IcnDueDateNextRev = ((HtmlGenericControl)(e.Row.FindControl("IcnDueDateNextRev")));

                    string TxtMQtyID = ((TextBox)e.Row.FindControl("TxtMQty")).ClientID;
                    string TxtBaseUOMID = ((TextBox)e.Row.FindControl("TxtBaseUOM")).ClientID;
                    string TxtResDueDateID = ((TextBox)e.Row.FindControl("TxtResDueDate")).ClientID;
                    string TxtEffectiveDateID = ((TextBox)e.Row.FindControl("TxtEffectiveDate")).ClientID;
                    string TxtDueDateNextRevID = ((TextBox)e.Row.FindControl("TxtDueDateNextRev")).ClientID;
                    string IcnCalResduedateID = ((HtmlGenericControl)e.Row.FindControl("IcnCalResduedate")).ClientID.ToString();
                    string IcnCalEffectiveDateID = ((HtmlGenericControl)e.Row.FindControl("IcnCalEffectiveDate")).ClientID.ToString();
                    string IcnDueDateNextRevID = ((HtmlGenericControl)e.Row.FindControl("IcnDueDateNextRev")).ClientID.ToString();

                    

                    if (IcnCalResduedate != null)
                    {
                        //IcnCalResduedate.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + IcnCalResduedateID + "','" + TxtResDueDateID + "');");
                        IcnCalResduedate.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + RowParentGv + "','" + TxtResDueDateID + "');");
                    }
                    if (IcnCalEffectiveDateID != null)
                    {
                        //IcnCalEffectiveDate.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + IcnCalEffectiveDateID + "','" + TxtEffectiveDateID + "');");
                        IcnCalEffectiveDate.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + RowParentGv + "','" + TxtEffectiveDateID + "');");
                    }

                    if (IcnDueDateNextRevID != null)
                    {
                        //IcnCalEffectiveDate.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + IcnCalEffectiveDateID + "','" + TxtEffectiveDateID + "');");
                        IcnDueDateNextRev.Attributes.Add("onclick", "DatePitcker();FocusToTxt('" + RowParentGv + "','" + TxtDueDateNextRevID + "');");
                    }

                    TxtMQty.Attributes.Add("oninput", "validateNumber('" + TxtMQtyID + "')");
                    TxtMQty.Attributes.Add("onkeyup", "SetBordrColor('" + TxtMQtyID + "')");
                    TxtBaseUOM.Attributes.Add("onkeyup", "SetBordrColor('" + TxtBaseUOMID + "')");
                    TxtResDueDate.Attributes.Add("onchange", "SetBordrColor('" + TxtResDueDateID + "')");
                    TxtResDueDate.Attributes.Add("class", "form_datetime");
                    TxtEffectiveDate.Attributes.Add("onchange", "ShowLoading();GetVndToolSelected();SetMyFocusID('" + TxtMQtyID + "');SetBordrColor('" + TxtEffectiveDateID + "');");
                    TxtEffectiveDate.Attributes.Add("class", "form_datetime");

                    TxtDueDateNextRev.Attributes.Add("onchange", "SetBordrColor('" + TxtDueDateNextRevID + "')");
                    TxtDueDateNextRev.Attributes.Add("class", "form_datetime");

                    chkToolAmortize.Attributes.Add("onclick", "ShowLoading();GetVndToolSelected();SetMyFocusID('" + TxtMQtyID + "');chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "');");
                    chkMachineAmortize.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "');");


                    DdlToolAmortize.Attributes.Add("onclick", "DdlAmortizeChange('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "','" + TxtMQtyID + "')");
                    DdlMachineAmortize.Attributes.Add("onclick", "DdlAmortizeChange('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "','" + chkToolAmortizeID + "','" + chkMachineAmortizeID + "','" + DdlToolAmortizeID + "','" + DdlMachineAmortizeID + "','" + TxtMQtyID + "')");

                    #endregion MQTY control

                    #region control File Attacch
                    Label lbFileName = e.Row.FindControl("lbFileName") as Label;
                    LinkButton LbPreview = e.Row.FindControl("LbPreview") as LinkButton;
                    LinkButton LbAttach = e.Row.FindControl("LbAttach") as LinkButton;

                    string lbFileNameID = ((Label)e.Row.FindControl("lbFileName")).ClientID;
                    string LbPreviewID = ((LinkButton)e.Row.FindControl("LbPreview")).ClientID;
                    string LbAttachID = ((LinkButton)e.Row.FindControl("LbAttach")).ClientID;

                    LbPreview.Attributes.Add("onclick", "return ValidatePreviewFile('" + lbFileNameID + "','" + LbPreviewID + "')");
                    //lbFileName.Attributes.Add("onclick", "TrigerFlUploadClick('" + LbAttachID + "')");
                    #endregion

                    #region fill Dropdown list
                    if (Session["DdlReason"] != null)
                    {
                        DataTable dtreason = (DataTable)Session["DdlReason"];
                        if (dtreason.Rows.Count > 0)
                        {
                            DdlReason.Items.Clear();
                            DdlReason.DataTextField = "ReasonforRejection";
                            DdlReason.DataValueField = "ReasonforRejection";

                            DdlReason.DataSource = dtreason;
                            DdlReason.DataBind();

                            DdlReason.Items.Insert(0, new ListItem("--Select Reason--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                        else
                        {
                            DdlReason.Items.Clear();
                            DdlReason.Items.Insert(0, new ListItem("--Select Reason--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                    }

                    if (Session["RecycleRatio"] != null)
                    {
                        DataTable dtRecycleRatio = (DataTable)Session["RecycleRatio"];
                        if (layout.ToString().ToUpper() == "LAYOUT1")
                        {
                            if (dtRecycleRatio.Rows.Count > 0)
                            {
                                DdlRecycleRatio.Items.Clear();
                                DdlRecycleRatio.DataTextField = "RecycleRatio";
                                DdlRecycleRatio.DataValueField = "RecycleRatio";

                                DdlRecycleRatio.DataSource = dtRecycleRatio;
                                DdlRecycleRatio.DataBind();

                                DdlRecycleRatio.Items.Insert(0, new ListItem("--Select Recycle Ratio--", "00"));
                            }
                            else
                            {
                                DdlRecycleRatio.Items.Clear();
                                DdlRecycleRatio.Items.Insert(0, new ListItem("IM Recycle Ration Not Exist", "NO DATA"));
                            }
                        }
                        else
                        {
                            DdlRecycleRatio.Items.Clear();
                            DdlRecycleRatio.Enabled = false;
                        }
                    }
                    else
                    {
                        if (layout.ToString().ToUpper() == "LAYOUT1")
                        {
                            DdlRecycleRatio.Items.Clear();
                            DdlRecycleRatio.Items.Insert(0, new ListItem("IM Recycle Ration Not Exist", "NO DATA"));
                        }
                        else
                        {
                            DdlRecycleRatio.Items.Clear();
                            DdlRecycleRatio.Enabled = false;
                        }
                    }
                    #endregion

                    #region fill all control based on temp data
                    if (Session["GvTemp"] != null)
                    {
                        DataTable dt = (DataTable)Session["GvTemp"];
                        if (dt.Rows[RowParentGv]["IsAllcostAllow"].ToString() == "1")
                        {
                            chkAllCost.Checked = true;
                        }
                        if (dt.Rows[RowParentGv]["IsMatcostAllow"].ToString() == "1")
                        {
                            ChkMatCost.Checked = true;
                        }
                        if (dt.Rows[RowParentGv]["IsProccostAllow"].ToString() == "1")
                        {
                            chkProcCost.Checked = true;
                        }
                        if (dt.Rows[RowParentGv]["IsSubMatcostAllow"].ToString() == "1")
                        {
                            chkSubMatCost.Checked = true;
                        }
                        if (dt.Rows[RowParentGv]["IsOthcostAllow"].ToString() == "1")
                        {
                            chkOthCost.Checked = true;
                        }

                        if (dt.Rows[RowParentGv]["IsUseToolAmor"].ToString() == "1")
                        {
                            chkToolAmortize.Checked = true;
                            DdlToolAmortize.SelectedValue = "ADD";
                        }
                        else
                        {
                            DdlToolAmortize.SelectedValue = "REMOVE";
                        }

                        if (dt.Rows[RowParentGv]["IsUseMachineAmor"].ToString() == "1")
                        {
                            chkMachineAmortize.Checked = true;
                            DdlMachineAmortize.SelectedValue = "ADD";
                        }
                        else
                        {
                            DdlMachineAmortize.SelectedValue = "REMOVE";
                        }

                        if (chkSubMatCost.Checked == true)
                        {
                            DdlToolAmortize.Attributes.Remove("disabled");
                            if (chkProcCost.Checked == true)
                            {
                                DdlMachineAmortize.Attributes.Remove("disabled");
                            }
                            else
                            {
                                DdlMachineAmortize.Attributes.Add("disabled", "disabled");
                            }
                        }
                        else
                        {
                            DdlToolAmortize.Attributes.Add("disabled", "disabled");
                            DdlMachineAmortize.Attributes.Add("disabled", "disabled");
                        }

                        ChkMatCost.Text = "&nbsp;" + dt.Rows[RowParentGv]["TotalMaterialCost"].ToString();
                        chkProcCost.Text = "&nbsp;" + dt.Rows[RowParentGv]["TotalProcessCost"].ToString();
                        chkSubMatCost.Text = "&nbsp;" + dt.Rows[RowParentGv]["TotalSubMaterialCost"].ToString();
                        chkOthCost.Text = "&nbsp;" + dt.Rows[RowParentGv]["TotalOtheritemsCost"].ToString();

                        if (dt.Rows[RowParentGv]["Reason"].ToString() != "")
                        {
                            if (DdlReason.Items.FindByText(dt.Rows[RowParentGv]["Reason"].ToString()) != null)
                            {
                                //DdlReason.SelectedItem.Text = dt.Rows[RowParentGv]["Reason"].ToString();
                                DdlReason.SelectedValue = dt.Rows[RowParentGv]["Reason"].ToString();
                                if (DdlReason.SelectedValue.ToString() == "Others")
                                {
                                    TxtOthReason.Style.Add("display", "block");
                                    LblengtVC.Style.Add("display", "block");
                                    TxtOthReason.Text = dt.Rows[RowParentGv]["Remark"].ToString();
                                }
                            }
                        }

                        if (dt.Rows[RowParentGv]["RecycleRatio"].ToString() != "")
                        {
                            if (DdlRecycleRatio.Items.FindByText(dt.Rows[RowParentGv]["RecycleRatio"].ToString()) != null)
                            {
                                //DdlReason.SelectedItem.Text = dt.Rows[RowParentGv]["Reason"].ToString();
                                DdlRecycleRatio.SelectedValue = dt.Rows[RowParentGv]["RecycleRatio"].ToString();
                            }
                        }

                        if (dt.Rows[RowParentGv]["Attachment"].ToString() == "")
                        {
                            lbFileName.Text = "No File";
                        }
                        else
                        {
                            lbFileName.Text = dt.Rows[RowParentGv]["Attachment"].ToString();
                        }
                        if (dt.Rows[RowParentGv]["MnthEstQty"].ToString() == "")
                        {
                            TxtMQty.Text = "";
                        }
                        else
                        {
                            TxtMQty.Text = dt.Rows[RowParentGv]["MnthEstQty"].ToString();
                        }
                        if (TxtBaseUOM.Text == "")
                        {
                            if (dt.Rows[RowParentGv]["BsUOM"].ToString() == "")
                            {
                                TxtBaseUOM.Text = "";
                            }
                            else
                            {
                                TxtBaseUOM.Text = dt.Rows[RowParentGv]["BsUOM"].ToString();
                            }
                        }

                        if (dt.Rows[RowParentGv]["DueDate"].ToString() == "")
                        {
                            TxtResDueDate.Text = "";
                        }
                        else
                        {
                            TxtResDueDate.Text = dt.Rows[RowParentGv]["DueDate"].ToString();
                        }

                        if (dt.Rows[RowParentGv]["EffectiveDate"].ToString() == "")
                        {
                            TxtEffectiveDate.Text = "";
                        }
                        else
                        {
                            TxtEffectiveDate.Text = dt.Rows[RowParentGv]["EffectiveDate"].ToString();
                        }

                        if (dt.Rows[RowParentGv]["DueDateNextRev"].ToString() == "")
                        {
                            TxtDueDateNextRev.Text = "";
                        }
                        else
                        {
                            if (DateDuenextRev() > DateTime.Today)
                            {
                                TxtDueDateNextRev.Enabled = false;
                            }
                            else
                            {
                                TxtDueDateNextRev.Enabled = true;
                            }
                            TxtDueDateNextRev.Text = dt.Rows[RowParentGv]["DueDateNextRev"].ToString();
                        }

                        if (chkToolAmortize.Checked == true && TxtEffectiveDate.Text != "") {
                            TxtMatGvTempFoc.Text = Material;
                            TxtProcGrpGvTempFoc.Text = ProcGroup;
                            TxtEffGvTempFoc.Text = TxtEffectiveDate.Text;

                            DataTable VndToolAmorList = vendorloadToolAmor();
                            GridView GvVndToolAmorList = e.Row.FindControl("GvDetToolAmor") as GridView;
                            if (GvVndToolAmorList != null)
                            {
                                GvVndToolAmorList.DataSource = VndToolAmorList;
                                GvVndToolAmorList.DataBind();

                                if (VndToolAmorList.Rows.Count == 1) {
                                    CheckBox chkVndToolAmor = (CheckBox)GvVndToolAmorList.Rows[0].FindControl("chkVndToolAmor");
                                    if (chkVndToolAmor != null)
                                    {
                                        //chkSelectAllVndToolAmor.Checked = true;
                                        chkVndToolAmor.Checked = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion


                    //ShowActionDet(QNo, RowParentGv);
                    //if (Session["GvAction"] != null)
                    //{
                    //    DataTable dtGvAction = (DataTable)Session["GvAction"];
                    //    GridView GvAction = e.Row.FindControl("GvAction") as GridView;
                    //    GvAction.DataSource = dtGvAction;
                    //    GvAction.DataBind();
                    //}
                    //if (e.Row.Cells[3].Text.Length > 12)
                    //{
                    //    e.Row.Cells[3].Text = e.Row.Cells[3].Text.Substring(0,12) + "...";
                    //} 
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvAction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //GridViewRow GvActRow = (GridViewRow)((GridView)sender).NamingContainer;
                    //GridView GvAct = (GridView)(GvActRow.Parent.Parent);
                    //GridViewRow GvMainRow = (GridViewRow)(GvAct.NamingContainer);
                    //int r = GvMainRow.RowIndex;

                    //GridViewRow gvMasterRow = (GridViewRow)e.Row.Parent.Parent;
                    string rowid = ((Label)e.Row.Parent.Parent.Parent.FindControl("LbRowParentGv")).Text;
                    int r = Convert.ToInt32(rowid);

                    #region Cost check box control
                    CheckBox chkAllCost = e.Row.FindControl("chkAllCost") as CheckBox;
                    CheckBox ChkMatCost = e.Row.FindControl("chkMatCost") as CheckBox;
                    CheckBox chkProcCost = e.Row.FindControl("chkProcCost") as CheckBox;
                    CheckBox chkSubMatCost = e.Row.FindControl("chkSubMatCost") as CheckBox;
                    CheckBox chkOthCost = e.Row.FindControl("chkOthCost") as CheckBox;

                    string chkAllCostID = ((CheckBox)e.Row.FindControl("chkAllCost")).ClientID;
                    string ChkMatCostID = ((CheckBox)e.Row.FindControl("chkMatCost")).ClientID;
                    string chkProcCostID = ((CheckBox)e.Row.FindControl("chkProcCost")).ClientID;
                    string chkSubMatCostID = ((CheckBox)e.Row.FindControl("chkSubMatCost")).ClientID;
                    string chkOthCostID = ((CheckBox)e.Row.FindControl("chkOthCost")).ClientID;

                    chkAllCost.Attributes.Add("onclick", "chkAllCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "')");
                    ChkMatCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "')");
                    chkProcCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "')");
                    chkSubMatCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "')");
                    chkOthCost.Attributes.Add("onclick", "chkCostClick('" + chkAllCostID + "','" + ChkMatCostID + "','" + chkProcCostID + "','" + chkSubMatCostID + "','" + chkOthCostID + "')");
                    #endregion Cost check box control

                    #region Reason Control condition
                    DropDownList DdlReason = e.Row.FindControl("DdlReason") as DropDownList;
                    TextBox TxtOthReason = e.Row.FindControl("TxtOthReason") as TextBox;
                    Label LblengtVC = e.Row.FindControl("LblengtVC") as Label;

                    string DdlReasonID = ((DropDownList)e.Row.FindControl("DdlReason")).ClientID;
                    string TxtOthReasonID = ((TextBox)e.Row.FindControl("TxtOthReason")).ClientID;
                    string LblengtVCID = ((Label)e.Row.FindControl("LblengtVC")).ClientID;

                    TxtOthReason.Style.Add("display","none");
                    LblengtVC.Style.Add("display", "none");
                    DdlReason.Attributes.Add("onchange", "DdlReasonchange('"+ DdlReasonID + "','" + TxtOthReasonID + "','" + LblengtVCID + "')");
                    TxtOthReason.Attributes.Add("onkeyup", "RemarkLght('" + TxtOthReasonID + "','" + LblengtVCID + "')");
                    #endregion Reason Control condition

                    #region tetxbox control
                    TextBox TxtMQty = e.Row.FindControl("TxtMQty") as TextBox;
                    TextBox TxtBaseUOM = e.Row.FindControl("TxtBaseUOM") as TextBox;
                    TextBox TxtResDueDate = e.Row.FindControl("TxtResDueDate") as TextBox;

                    string TxtMQtyID = ((TextBox)e.Row.FindControl("TxtMQty")).ClientID;
                    string TxtBaseUOMID = ((TextBox)e.Row.FindControl("TxtBaseUOM")).ClientID;
                    string TxtResDueDateID = ((TextBox)e.Row.FindControl("TxtResDueDate")).ClientID;

                    TxtMQty.Attributes.Add("oninput", "validateNumber('"+ TxtMQtyID + "')");
                    TxtMQty.Attributes.Add("onkeyup", "SetBordrColor('" + TxtMQtyID + "')");
                    TxtBaseUOM.Attributes.Add("onkeyup", "SetBordrColor('" + TxtBaseUOMID + "')");
                    TxtResDueDate.Attributes.Add("onchange", "SetBordrColor('" + TxtResDueDateID + "')");
                    TxtResDueDate.Attributes.Add("class", "form_datetime");
                    #endregion MQTY control

                    #region control File Attacch
                    Label lbFileName = e.Row.FindControl("lbFileName") as Label;
                    LinkButton LbPreview = e.Row.FindControl("LbPreview") as LinkButton;

                    string lbFileNameID = ((Label)e.Row.FindControl("lbFileName")).ClientID;
                    string LbPreviewID = ((LinkButton)e.Row.FindControl("LbPreview")).ClientID;

                    LbPreview.Attributes.Add("onclick", "ValidatePreviewFile('"+ lbFileNameID + "','"+ LbPreviewID + "')");
                    #endregion

                    if (Session["DdlReason"] != null)
                    {
                        DataTable dtreason = (DataTable)Session["DdlReason"];
                        if (dtreason.Rows.Count > 0)
                        {
                            DdlReason.Items.Clear();
                            DdlReason.DataTextField = "ReasonforRejection";
                            DdlReason.DataValueField = "ReasonforRejection";

                            DdlReason.DataSource = dtreason;
                            DdlReason.DataBind();

                            DdlReason.Items.Insert(0, new ListItem("--Select Reason--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                        else
                        {
                            DdlReason.Items.Clear();
                            DdlReason.Items.Insert(0, new ListItem("--Select Reason--", "00"));
                            DdlReason.Items.Add(new ListItem("Others", "Others"));
                        }
                    }

                    if (Session["GvTemp"] != null)
                    {
                        DataTable dt = (DataTable)Session["GvTemp"];
                        if (dt.Rows[r]["IsAllcostAllow"].ToString() == "1")
                        {
                            chkAllCost.Checked = true;
                        }
                        if (dt.Rows[r]["IsMatcostAllow"].ToString() == "1")
                        {
                            ChkMatCost.Checked = true;
                        }
                        if (dt.Rows[r]["IsProccostAllow"].ToString() == "1")
                        {
                            chkProcCost.Checked = true;
                        }
                        if (dt.Rows[r]["IsSubMatcostAllow"].ToString() == "1")
                        {
                            chkSubMatCost.Checked = true;
                        }
                        if (dt.Rows[r]["IsOthcostAllow"].ToString() == "1")
                        {
                            chkOthCost.Checked = true;
                        }

                        ChkMatCost.Text = "&nbsp;" + dt.Rows[r]["Tot.Mat. Cost"].ToString();
                        chkProcCost.Text = "&nbsp;" + dt.Rows[r]["Tot.Proc. Cost"].ToString();
                        chkSubMatCost.Text = "&nbsp;" + dt.Rows[r]["Tot.SubMat. Cost"].ToString();
                        chkOthCost.Text = "&nbsp;" + dt.Rows[r]["Tot.Oth. Cost"].ToString();

                        if (dt.Rows[r]["Reason"].ToString() != "")
                        {
                            if (DdlReason.Items.FindByText(dt.Rows[r]["Reason"].ToString()) != null)
                            {
                                //DdlReason.SelectedItem.Text = dt.Rows[RowParentGv]["Reason"].ToString();
                                DdlReason.SelectedValue = dt.Rows[r]["Reason"].ToString();
                                if (DdlReason.SelectedValue.ToString() == "Others")
                                {
                                    TxtOthReason.Style.Add("display", "block");
                                    LblengtVC.Style.Add("display", "block");
                                    TxtOthReason.Text = dt.Rows[r]["Remark"].ToString();
                                }
                            }
                        }
                        if (dt.Rows[r]["Attachment"].ToString() == "")
                        {
                            lbFileName.Text = "No File";
                        }
                        else
                        {
                            lbFileName.Text = dt.Rows[r]["Attachment"].ToString();
                        }
                        if (dt.Rows[r]["MnthEstQty"].ToString() == "")
                        {
                            TxtMQty.Text = "";
                        }
                        else
                        {
                            TxtMQty.Text = dt.Rows[r]["MnthEstQty"].ToString();
                        }
                        if (TxtBaseUOM.Text == "")
                        {
                            if (dt.Rows[r]["BsUOM"].ToString() == "")
                            {
                                TxtBaseUOM.Text = "";
                            }
                            else
                            {
                                TxtBaseUOM.Text = dt.Rows[r]["BsUOM"].ToString();
                            }
                        }
                        if (dt.Rows[r]["DueDate"].ToString() == "")
                        {
                            TxtResDueDate.Text = "";
                        }
                        else
                        {
                            TxtResDueDate.Text = dt.Rows[r]["DueDate"].ToString();
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

        protected void GvAction_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRec")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GvTemp.Rows[rowIndex];
                    string QuNo = GvTemp.Rows[rowIndex].Cells[1].Text;

                    if (Session["GvTemp"] != null)
                    {
                        DataTable DtGvtemp = (DataTable)Session["GvTemp"];
                        if (DtGvtemp.Rows.Count > 0)
                        {
                            for (int j = 0; j < DtGvtemp.Rows.Count; j++)
                            {
                                DataRow dr = DtGvtemp.Rows[j];
                                if (DtGvtemp.Rows[j]["QuoteNo"].ToString() == QuNo)
                                {
                                    dr.Delete();
                                }
                            }
                            DtGvtemp.AcceptChanges();
                        }
                        Session["GvTemp"] = DtGvtemp;
                        GvTemp.DataSource = DtGvtemp;
                        GvTemp.DataBind();
                        TxtGvTempLeng.Text = DtGvtemp.Rows.Count.ToString();
                        if (GvTemp.Rows.Count <= 0)
                        {
                            //LbTitleQrefList.Visible = false;
                            BtnCreateReq.Visible = false;
                        }
                        else
                        {
                            //LbTitleQrefList.Visible = true;
                            BtnCreateReq.Visible = true;
                        }
                        DeleteNonRequest();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "alert('Quote No : " + QuNo + " deleted ! ');", true);
                    }
                }
                else if (e.CommandName == "CUploadFile")
                {
                    DeleteNonRequest();
                    //int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //GridViewRow row = GvTemp.Rows[rowIndex];

                    GridViewRow Gv2Row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    GridView Childgrid = (GridView)(Gv2Row.Parent.Parent);
                    GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);
                    
                    int ParentIdx = Gv1Row.RowIndex;

                    Session["RowGvTemp"] = ParentIdx.ToString();
                    TxtRowGvTemp.Text = ParentIdx.ToString();
                }
                else if (e.CommandName == "CPreviewFile")
                {
                    //int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //GridViewRow row = GvTemp.Rows[rowIndex];
                    GridViewRow Gv2Row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    GridView Childgrid = (GridView)(Gv2Row.Parent.Parent);
                    GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);

                    int ParentIdx = Gv1Row.RowIndex;

                    Session["RowGvTemp"] = ParentIdx.ToString();
                    TxtRowGvTemp.Text = ParentIdx.ToString();
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GvTemp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRec")
                {
                    int c = GvTemp.Rows.Count;
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GvTemp.Rows[rowIndex];
                    string QuNo = GvTemp.Rows[rowIndex].Cells[1].Text;

                    #region Label cotrol
                    Label LbQNModal = GvTemp.Rows[rowIndex].FindControl("LbQNModal") as Label;
                    if (LbQNModal != null)
                    {
                        QuNo = LbQNModal.Text;
                    }
                    #endregion

                    if (Session["GvTemp"] != null)
                    {
                        DataTable DtGvtemp = (DataTable)Session["GvTemp"];
                        if (DtGvtemp.Rows.Count > 0)
                        {
                            for (int j = 0; j < DtGvtemp.Rows.Count; j++)
                            {
                                DataRow dr = DtGvtemp.Rows[j];
                                if (DtGvtemp.Rows[j]["QuoteNo"].ToString() == QuNo)
                                {
                                    dr.Delete();
                                    Session["FlAtc" + QuNo] = null;
                                }
                            }
                            DtGvtemp.AcceptChanges();
                        }
                        Session["GvTemp"] = DtGvtemp;
                        GvTemp.DataSource = DtGvtemp;
                        GvTemp.DataBind();
                        TxtGvTempLeng.Text = DtGvtemp.Rows.Count.ToString();
                        if (GvTemp.Rows.Count <= 0)
                        {
                            //LbTitleQrefList.Visible = false;
                            BtnCreateReq.Visible = false;
                        }
                        else
                        {
                            //LbTitleQrefList.Visible = true;
                            BtnCreateReq.Visible = true;
                        }
                        DeleteNonRequest();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Tabs", "alert('Quote No : " + QuNo + " deleted ! ');ChgEmptyFlColor();", true);
                    }
                }
                else if (e.CommandName == "CUploadFile")
                {
                    DeleteNonRequest();
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GvTemp.Rows[rowIndex];
                    TxtRowGvTemp.Text = rowIndex.ToString();
                    Session["RowGvTemp"] = rowIndex.ToString();
                }
                else if (e.CommandName == "CPreviewFile")
                {
                    DeleteNonRequest();
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GvTemp.Rows[rowIndex];
                    TxtRowGvTemp.Text = rowIndex.ToString();
                    Session["RowGvTemp"] = rowIndex.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "TrigerPreviewClick();", true);
                    //BtnPreview_Click(BtnPreview, EventArgs.Empty);
                }
                if (e.CommandName == "QuoteDetails")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    Label LbQNModal = GvTemp.Rows[rowIndex].FindControl("LbQNModal") as Label;
                    string QuoteNo = LbQNModal.Text;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('QQPReview.aspx?Number=" + QuoteNo + "');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "OpenMdQuoteCostPlant();UnitCost();", true);
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ex);
                var script = string.Format("alert({0});", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }
        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GetQuoteList();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (FlAttachment.HasFile)
                {
                    string RowmainIdx = Session["RowGvTemp"].ToString();
                    int GvTemidx = Convert.ToInt32(RowmainIdx);

                    Label LbQNModal = (Label)GvTemp.Rows[GvTemidx].FindControl("LbQNModal");
                    if (LbQNModal != null)
                    {
                        Session["FlAtc" + LbQNModal.Text] = FlAttachment;
                    }
                    

                    if (Session["GvTemp"] != null)
                    {
                        DataTable dt = (DataTable)Session["GvTemp"];
                        dt.Rows[GvTemidx]["Attachment"] = FlAttachment.PostedFile.FileName;
                        Session["GvTemp"] = dt;
                        UpdateDtGvTemp();
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDtGvTemp();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "ChgEmptyFlColor();", true);
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void BtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["RowGvTemp"] != null)
                {
                    string RowMainIdx = Session["RowGvTemp"].ToString();
                    string a = TxtRowGvTemp.Text;
                    int rowid = Convert.ToInt32(RowMainIdx);
                    Label LbQNModal = (Label)GvTemp.Rows[rowid].FindControl("LbQNModal");

                    if (Session["FlAtc" + LbQNModal.Text] != null)
                    {
                        FlAttachment = (FileUpload)Session["FlAtc" + LbQNModal.Text];
                        if (FlAttachment.HasFile)
                        {
                            string folderPath = Server.MapPath("~/Files/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            string FileExtension = System.IO.Path.GetExtension(FlAttachment.FileName);
                            string filename = System.IO.Path.GetFileName(FlAttachment.FileName);
                            string PathAndFileName = folderPath + DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
                            FlAttachment.SaveAs(PathAndFileName);

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
                                    FileInfo file = new FileInfo(PathAndFileName);
                                    if (file.Exists) //check file exsit or not  
                                    {
                                        file.Delete();
                                    }
                                    Response.End();
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
                var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ex);
                var script = string.Format("alert({0});", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
            }
        }

        protected void BtnSubmitRequest_Click(object sender, EventArgs e)
        {
            bool isSucces = false;
            string msger = "";
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            if ((Session["flag"].ToString() == "Pass"))
            {
                try
                {
                    EmetCon.Open();
                    EmetTrans = EmetCon.BeginTransaction();

                    DataTable dtGvTemp = new DataTable();
                    DataTable dtCreateReqTemp = new DataTable();
                    if (Session["GvTemp"] != null)
                    {
                        dtGvTemp = (DataTable)Session["GvTemp"];
                    }
                    if (Session["CreateReqTemp"] != null)
                    {
                        dtCreateReqTemp = (DataTable)Session["CreateReqTemp"];
                    }

                    string StrDataListVndAmor = TxtDataListVndAmor.Text;
                    List<ListVndAmor> MyListVndAmor = new List<ListVndAmor>();
                    if (StrDataListVndAmor.Trim() != "" || StrDataListVndAmor.Trim() != "[]") {
                        MyListVndAmor = (List<ListVndAmor>)JsonConvert.DeserializeObject((StrDataListVndAmor), typeof(List<ListVndAmor>));
                    }

                    if (GvTemp.Rows.Count > 0 && dtGvTemp.Rows.Count > 0 && dtCreateReqTemp.Rows.Count > 0)
                    {
                        sql = "";
                        string QuoteNo = "";
                        string QuoteNoRef = "";
                        string ReqNo = "";
                        string VendorCode = "";
                        string VendorDesc = "";
                        string IsUseToolAmortize = "0";
                        string Material = "0";
                        string MaterialDesc = "";
                        string ProcessGroup = "";

                        for (int g = 0; g < dtCreateReqTemp.Rows.Count; g++)
                        {
                            ReqNo = dtCreateReqTemp.Rows[g]["Req No"].ToString();
                            QuoteNo = dtCreateReqTemp.Rows[g]["Quote No New"].ToString();
                            QuoteNoRef = dtCreateReqTemp.Rows[g]["Quote No Ref"].ToString();

                            VendorCode = dtCreateReqTemp.Rows[g]["Vendor Code"].ToString();
                            VendorDesc = dtCreateReqTemp.Rows[g]["Vendor Name"].ToString();
                            IsUseToolAmortize = dtCreateReqTemp.Rows[g]["IsUseToolAmortize"].ToString();
                            Material = dtCreateReqTemp.Rows[g]["Material"].ToString();
                            MaterialDesc = dtCreateReqTemp.Rows[g]["MaterialDesc"].ToString();
                            ProcessGroup = dtCreateReqTemp.Rows[g]["ProcessGroup"].ToString();

                            //IEnumerable<DataRow> query = from EffectiveDate in dtGvTemp.AsEnumerable()
                            //                            where EffectiveDate.Field<string>("QuoteNo") == QuoteNoRef
                            //                            select EffectiveDate;

                            //DataTable dtGvTempFiltered = new DataTable();
                            //dtGvTempFiltered = query.CopyToDataTable<DataRow>();

                            sql = @" update TQuoteDetails SET CreateStatus = 'Article',ApprovalStatus=0,PICApprovalStatus = NULL, 
                                ManagerApprovalStatus = NULL, DIRApprovalStatus = NULL where QuoteNo = '" + QuoteNo + "' ";
                            cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                            cmd.ExecuteNonQuery();

                            if (IsUseToolAmortize == "1" || IsUseToolAmortize.ToUpper() == "TRUE") {
                                if (MyListVndAmor.ToString() != "") {
                                    if (MyListVndAmor.Count() > 0) {
                                        for (int t = 0; t < MyListVndAmor.Count(); t++)
                                        {
                                            if (MyListVndAmor[t].QuoteNoRef == QuoteNoRef) {
                                                sql = @" INSERT INTO TToolAmortization(Plant,RequestNumber,QuoteNo,QuoteApproveddate,MaterialCode,MaterialDescription,VendorCode
                                                        ,VendorDescription,Process_Grp_code,ToolTypeID,Amortize_Tool_ID,Amortize_Tool_Desc,AmortizeCost,AmortizeCurrency
                                                        ,ExchangeRate,AmortizeCost_Vend_Curr,AmortizePeriod,AmortizePeriodUOM,TotalAmortizeQty,QtyUOM,AmortizeCost_Pc_Vend_Curr
                                                        ,EffectiveFrom,DueDate,CreatedBy,CreatedOn)
                                                        select TT.Plant,@ReqNo,@NewQuoteNo,null,@Material,@MaterialDescription,@VendorCode
                                                        ,@VendorDesc,@Process_Grp_code,TT.ToolTypeID,TT.Amortize_Tool_ID,TT.Amortize_Tool_Desc,TT.AmortizeCost,TT.AmortizeCurrency
                                                        ,TT.ExchangeRate,TT.AmortizeCost_Vend_Curr,TT.AmortizePeriod,TT.AmortizePeriodUOM,TT.TotalAmortizeQty,TT.QtyUOM,TT.AmortizeCost_Pc_Vend_Curr
                                                        ,TT.EffectiveFrom,TT.DueDate,@CreatedBy,CURRENT_TIMESTAMP 
                                                        from " + DbMasterName + @".dbo.TToolAmortization as TT
                                                        inner join " + DbMasterName + @".dbo.TToolAmortizationvsMember as TM  on TM.Plant = TT.Plant and tm.Amortize_Tool_ID = tt.Amortize_Tool_ID and tm.Material = @Material 
                                                        where TT.Amortize_Tool_ID = @Amortize_Tool_ID and TT.Plant=@Plant and TT.VendorCode=@VendorCode and TT.Process_Grp_code=@Process_Grp_code";
                                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                                cmd.Parameters.AddWithValue("@Plant", TxtPlant.Text);
                                                cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                                                cmd.Parameters.AddWithValue("@VendorDesc", VendorDesc);
                                                cmd.Parameters.AddWithValue("@Process_Grp_code", ProcessGroup);
                                                cmd.Parameters.AddWithValue("@Material", Material);
                                                cmd.Parameters.AddWithValue("@MaterialDescription", MaterialDesc);
                                                cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                                                cmd.Parameters.AddWithValue("@NewQuoteNo", QuoteNo);
                                                cmd.Parameters.AddWithValue("@QuoteNoRef", QuoteNoRef);
                                                cmd.Parameters.AddWithValue("@Amortize_Tool_ID", MyListVndAmor[t].ToolID);
                                                cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                                                cmd.ExecuteNonQuery();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                    }

                    #region save data bom list raw material baed on effective date
                    if (Session["Rawmaterial"] != null)
                    {
                        DataTable dtRawmat = (DataTable)Session["Rawmaterial"];
                        if (dtRawmat.Rows.Count > 0)
                        {
                            for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                            {
                                string ValidFrom = "";
                                if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                {
                                    DateTime DtValFrm = DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);
                                    ValidFrom = DtValFrm.ToString("yyyy-MM-dd");
                                }

                                sql = @" insert into TSMMBOM_RAWMATCost_EffDate(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                                                        AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                                                                        values(
                                                                        '" + dtRawmat.Rows[rm]["Req No"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["QuoteNo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material Desc"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_SCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Selling_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_VCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Venor_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Unit"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["UOM"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValFrom"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValTo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["ExchRate"] + @"', ";
                                if (ValidFrom == "")
                                {
                                    sql += @" NULL,";
                                }
                                else
                                {
                                    sql += @" '" + ValidFrom + "',";
                                }
                                sql += @" CURRENT_TIMESTAMP, '" + Session["userID"].ToString() + @"') ";

                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    #endregion

                    #region save data bom list raw material before effective date
                    if (Session["OldRawmaterial"] != null)
                    {
                        DataTable dtRawmat = (DataTable)Session["OldRawmaterial"];
                        if (dtRawmat.Rows.Count > 0)
                        {
                            for (int rm = 0; rm < dtRawmat.Rows.Count; rm++)
                            {
                                string ExchValidFrom = "";
                                if (dtRawmat.Rows[rm]["ValidFrom"].ToString() != "")
                                {
                                    DateTime DtValFrm = DateTime.ParseExact(dtRawmat.Rows[rm]["ValidFrom"].ToString(), "dd-MM-yyyy", null);
                                    ExchValidFrom = DtValFrm.ToString("yyyy-MM-dd");
                                }

                                sql = @" insert into TSMNBOM_RAWMATCost(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                                                        AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                                                                        values(
                                                                        '" + dtRawmat.Rows[rm]["Req No"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["QuoteNo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Comp Material Desc"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_SCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Selling_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Amt_VCur"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Venor_Crcy"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["Unit"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["UOM"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValFrom"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["CusMatValTo"] + @"',
                                                                        '" + dtRawmat.Rows[rm]["ExchRate"] + @"', ";
                                if (ExchValidFrom == "")
                                {
                                    sql += @" NULL,";
                                }
                                else
                                {
                                    sql += @" '" + ExchValidFrom + "',";
                                }
                                sql += @" CURRENT_TIMESTAMP, '" + Session["userID"].ToString() + @"') ";

                                cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    #endregion


                    EmetTrans.Commit();

                    isSucces = true;
                }
                catch (Exception ex)
                {
                    EmetTrans.Rollback();
                    LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                    EMETModule.SendExcepToDB(ex);
                    msger = ex.ToString();
                    isSucces = false;
                }
                finally
                {
                    EmetTrans.Dispose();
                    EmetCon.Dispose();
                }

                if (isSucces == true)
                {
                    Session["Rawmaterial"] = null;
                    DeleteNonRequest();
                    SendingMail();
                    if (sendingmail == true)
                    {
                        //var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Successfully submitted");
                        //var script = string.Format("alert({0});CloseLoading();window.location ='Home.aspx';", message);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('Successfully submitted');window.location ='Home.aspx';", true);
                    }
                    else
                    {
                        var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Send Mail failed !! \n " + errmsg + " ");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert("+ message + ");window.location ='Home.aspx';", true);
                    }
                }
                else
                {
                    Session["Rawmaterial"] = null;
                    DeleteNonRequest();
                    var message = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Failed Create request : !! \n " + msger + " ");
                    var script = string.Format("alert({0});CloseLoading();", message);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", script, true);
                }
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
            catch (ThreadAbortException ex)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        /// <summary>
        /// Get BOM Detail Raw Material before efffective date
        /// </summary>
        protected void GetBOMRawmaterialBefEffdate(string ReqNo, string QuoteNo, string VendorCode, string Material, string Plant, string effdate)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" ";
                    sql = @"  
                    --declare @plant nvarchar(4) ='2100'
                    --declare @mat nvarchar(20)='40000721'

                    declare @e50check bit = 1
                    declare @e50mat nvarchar(20)
                    set @e50mat=''

                    --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                    SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                    [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                    into #complist1 from tbomlistnew 
                    where Plant=@plant and [Parent Material]=@mat 
                    --no need altbom because not using PV for direct transfer
                    --and [alternative bom]=@altbom 
                    and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                    and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo 
                    and UPPER([component plant status]) not in ('Z4','Z9')
                    and UPPER([parent plant status]) not in ('Z4','Z9')				
                    and [co-product] <> 'X'

                    --get the list of e50 components
                    SELECT Plant,[Component Material] as 'Material'
                    into #e50 from tbomlistnew 
                    where Plant=@plant and [Parent Material]=@mat 
                    --no need altbom because not using PV for direct transfer
                    --and [alternative bom]=@altbom
                    and [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                    and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo 
                    and UPPER([component plant status]) not in ('Z4','Z9')
                    and UPPER([parent plant status]) not in ('Z4','Z9')
                    and [Component special procurement type] = '50'
                    and [co-product] <> 'X'

                    IF ((select count (*) from #e50)=0)
                    BEGIN
	                    set @e50check=0
                    END
                    ELSE
                    BEGIN
	                    set @e50check=1
                    END

                    WHILE (@e50check=1)
                    BEGIN
	                    insert into #complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                    [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                    (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                    [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                    from tbomlistnew t1 inner join #e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                    where [header valid to date] < @ValidTo 
	                    and [comp. valid to date] < @ValidTo
	                    and UPPER([component plant status]) not in ('Z4','Z9')
	                    and UPPER([parent plant status]) not in ('Z4','Z9')
	                    and [co-product] <> 'X'
	                    group by
	                    t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                    [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                    select * into #temp from #e50
	                    delete from #e50
	
	                    insert into #e50 (Plant, Material) 
	                    (SELECT t1.Plant, [Component Material]
	                    from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                    where [header valid to date] < @ValidTo
	                    and [comp. valid to date] < @ValidTo
	                    and UPPER([component plant status]) not in ('Z4','Z9')
	                    and UPPER([parent plant status]) not in ('Z4','Z9')
	                    and [Component special procurement type] = '50'
	                    and [co-product] <> 'X')
	
	                    drop table #temp
	
	                    if ((select count (*) from #e50)=0)
	                    BEGIN
		                    set @e50check=0
	                    END			
                    END
                    ";

                    sql += @" select distinct @ReqNo as [Req No],@QuoteNo as [QuoteNo],@VendorCode [Vendor Code],tvpo.Plant,  
                        TB.[Component Material] as [Comp Material],tm.MaterialDesc as [Comp Material Desc],v.Description as [Vendor Name],
                        vp.PICName as Ven_PIC,tc.UnitofCurrency as Selling_Crcy,tc.Amount   as Amt_SCur,
                        isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate,
                        v.Crcy AS Venor_Crcy, isnull(ROUND(CAST((tc.amount*isnull((CASE   WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')as Amt_VCur,  
                        tc.Unit,tm.BaseUOM as UOM,
						format(TR.ValidFrom,'dd-MM-yyyy') as ValidFrom,FORMAT(tc.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom],format(tc.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                        into #temp1
                        from TMATERIAL tm inner join #complist1 TB on tm.Material = TB.[Component Material]   
                        inner join TCUSTOMER_MATLPRICING tc on TB.[Component Material] = tc.Material   and tc.plant = @plant and tc.delflag = 0
                        inner join tVendor_New v on v.CustomerNo = tc.Customer   
                        inner join TVENDORPIC as VP on vp.VendorCode=v.Vendor  
                        inner join tvendorporg tvpo on tvpo.Vendor=v.Vendor   and tvpo.porg = v.porg   
                        left outer join  TEXCHANGE_RATE tr   on tr.EFrm=tc.UnitofCurrency and tr.ETo =v.Crcy and tm.Plant = @plant and VP.Plant = @plant  and tvpo.Plant = @plant
                        and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TC.UnitofCurrency  AND ValidFrom <= @ValidTo )
                        where tm.Plant=@plant and tvpo.Plant=@plant and vp.plant = @plant and TB.[Parent Material] = @mat
                        and format(TC.ValidTo,'yyyy-MM-dd') < @ValidTo
                        and (isnull(ROUND(CAST((isnull((CASE WHEN (tc.UnitofCurrency != v.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1')!= 0 ) 
                        and v.Vendor = @VendorCode
                    
                        order by format(tc.ValidTo,'yyyy-MM-dd') desc
                        
                        ;with cte as
                        (
	                        select *, ROW_NUMBER() over (partition by [Comp Material] order by [CusMatValTo] desc) as RN
	                         from #temp1
                        )
                        select * from cte where RN = 1

                        drop table #temp1
                        ";

                    sql += @" drop table #complist1, #e50 ";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    cmd.Parameters.AddWithValue("@mat", Material);
                    cmd.Parameters.AddWithValue("@plant", Plant);
                    DateTime ValidTo = DateTime.ParseExact(effdate, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy-MM-dd"));
                    cmd.CommandTimeout = 10000;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (Session["OldRawmaterial"] == null)
                            {
                                Session["OldRawmaterial"] = dt;
                            }
                            else
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    DataTable DtRawmat = (DataTable)Session["OldRawmaterial"];
                                    foreach (DataRow drdtget in dt.Rows)
                                    {
                                        DtRawmat.ImportRow(drdtget);
                                    }

                                    Session["OldRawmaterial"] = DtRawmat;
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
            finally
            {
                MDMCon.Dispose();
            }
        }


        protected void GvDetToolAmor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int RowIdx = e.Row.DataItemIndex;
                    GridView gvAlt = (GridView)sender;
                    GridViewRow gvMasterRow = (GridViewRow)gvAlt.Parent.Parent.Parent;
                    
                    int TotRowGvTemp = 0;
                    if (Session["GvTemp"] != null) {
                        DataTable dt = (DataTable)Session["GvTemp"];
                        TotRowGvTemp = dt.Rows.Count;
                    }
                    int RowGvTempIdx = gvMasterRow.RowIndex;

                    #region Cost check box control
                    Label LbTotRecord = e.Row.FindControl("LbTotRecord") as Label;
                    string Totrow = LbTotRecord.Text;
                    
                    CheckBox chkVndToolAmor = e.Row.FindControl("chkVndToolAmor") as CheckBox;
                    string chkVndToolAmorID = ((CheckBox)e.Row.FindControl("chkVndToolAmor")).ClientID;
                    string MainID = chkVndToolAmorID.Substring(0, chkVndToolAmorID.Length - 1);
                    chkVndToolAmor.Attributes.Add("onclick", "RbVndToolAmor_onchange('" + Totrow + "','" + MainID + "','" + chkVndToolAmorID + "')");
                    
                    #endregion Cost check box control
                    
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        protected void BtnGetVndTool_OnClick(object sender, EventArgs e)
        {
            try
            {
                //int Rowid = 0;
                //if (TxtGvtempRowFoc.Text != "") {
                //    Rowid = Convert.ToInt32(TxtGvtempRowFoc.Text);
                //}
                //string Material = TxtMatGvTempFoc.Text;
                //string ProcGrp = TxtProcGrpGvTempFoc.Text;
                //string EffDate = TxtEffGvTempFoc.Text;
                //bool IsuseToolAmor = ChkToolAmo.Checked;

                //if (IsuseToolAmor == true && EffDate != "")
                //{
                //    DataTable VndToolAmorList = vendorloadToolAmor();
                //    GridView GvVndToolAmorList = GvTemp.Rows[Rowid].FindControl("GvDetToolAmor") as GridView;
                //    if (GvVndToolAmorList != null) {
                //        GvVndToolAmorList.DataSource = VndToolAmorList;
                //        GvVndToolAmorList.DataBind();
                //    }
                //}
                //else
                //{
                //}
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        

        protected DataTable vendorloadToolAmor()
        {
            DataTable dt = new DataTable();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct TVP.VendorCode,TVP.VendorName, tv.CodeRef as SearchTerm 
                            ,tt.ToolTypeID,tt.Amortize_Tool_ID,tt.Amortize_Tool_Desc
                            ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost),0) as AmortizeCost
                            , tt.AmortizeCurrency,tt.ExchangeRate
                            ,tt.AmortizePeriod,tt.AmortizePeriodUOM,tt.TotalAmortizeQty,tt.QtyUOM
                            ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                            ,IIF( right(tt.AmortizeCost_Pc_Vend_Curr,6) = '000000', convert(nvarchar(50),convert(int,tt.AmortizeCost_Pc_Vend_Curr)) ,convert(nvarchar(50),tt.AmortizeCost_Pc_Vend_Curr) ) as AmortizeCost_Pc_Vend_Curr
                            ,tt.EffectiveFrom, tt.DueDate
                            , FORMAT(tt.EffectiveFrom, 'yyyy-MM-dd') as EeffDt
                            , FORMAT(tt.DueDate, 'yyyy-MM-dd') as DuDate
                            from TVENDOR_PROCESSGROUP TVP 
                            inner join tvendorporg tv ON TVP.VendorCode = tv.Vendor 
                            inner join TVENDORPIC as VP on VP.VendorCode = tv.Vendor 
                            inner join TPOrgPlant as tp on tp.porg = tv.POrg 
                            inner join TToolAmortization as TT on tv.Plant = tt.Plant and tv.Vendor = tt.VendorCode and TVP.ProcessGrp = tt.Process_Grp_code
                            inner join TToolAmortizationvsMember as TM  on TM.Plant = TT.Plant and tm.Amortize_Tool_ID = tt.Amortize_Tool_ID and tm.Material = @Material
                            Where TVP.processgrp = @processgrp
                            and tp.plant = @Plant
                            and tv.plant = @Plant
                            and vp.plant = @Plant
                            and (tv.DelFlag = 0) 
                            and (TVP.DelFlag = 0) 
                            and (ISNULL(tt.DelFlag,0)=0) and (tt.EffectiveFrom is null or @EffectiveDate between EffectiveFrom and DueDate) ";
                    if (RbExternal.Checked == true)
                    {
                        sql += @" and ( TVP.VendorCode not in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    else
                    {
                        sql += @" and ( TVP.VendorCode in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    sql += @" order by TVP.VendorCode asc ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@processgrp", TxtProcGrpGvTempFoc.Text);
                    DateTime DtEffectiveDate = DateTime.ParseExact(TxtEffGvTempFoc.Text, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Material", TxtMatGvTempFoc.Text);
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);

                    DataColumn Col = dt.Columns.Add("TotRecord", typeof(Int32));
                    Col.AllowDBNull = true;
                    int TotRow = dt.Rows.Count;
                    foreach (DataRow row in dt.Rows)
                    {
                        row.SetField("TotRecord", TotRow);
                    }
                    dt.AcceptChanges();
                }
                return dt;
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                return dt;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }
 
        private class ListVndAmor
        {
            public string QuoteNoRef { get; set; }
            public string ToolID { get; set; }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------

        //protected void GetDdlMatClassDesc()
        //{
        //    SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
        //    try
        //    {
        //        MDMCon.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            sql = @" Select Distinct TR.ProdComDesc from TMATERIAL TM Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode where  TM.product=@product and TM.SplPROCTYPE is null  and TM.Plant = @plant and TM.delflag = 0 ";

        //            cmd = new SqlCommand(sql, MDMCon);
        //            cmd.Parameters.AddWithValue("@plant", Session["EPlant"].ToString());
        //            cmd.Parameters.AddWithValue("@product", DdlProduct.SelectedValue.ToString());
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    DdlMatClassDesc.Items.Clear();
        //                    DdlMatClassDesc.DataTextField = "ProdComDesc";
        //                    DdlMatClassDesc.DataValueField = "ProdComDesc";

        //                    DdlMatClassDesc.DataSource = dt;
        //                    DdlMatClassDesc.DataBind();
        //                    if (dt.Rows.Count > 1)
        //                    {
        //                        DdlMatClassDesc.Items.Insert(0, new ListItem("--Select Material Class--", "Select Material Class"));
        //                    }
        //                }
        //                else
        //                {
        //                    DdlMatClassDesc.Items.Clear();
        //                    DdlMatClassDesc.Items.Insert(0, new ListItem("--Material Class Not Exist--", "Material Class Not Exist"));
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
        //        MDMCon.Dispose();
        //    }
        //}
        //protected void GetDdlProcGroup()
        //{
        //    SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
        //    try
        //    {
        //        MDMCon.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            sql = @" select CONCAT(Process_Grp_code,' - ',Process_Grp_Description) as 'procgroup' 
        //                    from TPROCESGROUP_LIST
        //                    where DelFlag=0";

        //            cmd = new SqlCommand(sql, MDMCon);
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    DdlProcGroup.Items.Clear();
        //                    DdlProcGroup.DataSource = dt;
        //                    DdlProcGroup.DataTextField = "procgroup";
        //                    DdlProcGroup.DataValueField = "procgroup";
        //                    DdlProcGroup.DataBind();
        //                    if (dt.Rows.Count > 1)
        //                    {
        //                        DdlProcGroup.Items.Insert(0, new ListItem("-- Select Process Group--", "00"));
        //                    }
        //                }
        //                else
        //                {
        //                    DdlProcGroup.Items.Clear();
        //                    DdlProcGroup.Items.Insert(0, new ListItem("--Process Group Not Exist--", "0"));
        //                }
        //            }
        //            //UpdatePanel18.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        //    }
        //    finally
        //    {
        //        MDMCon.Dispose();
        //    }
        //}

        //protected void GetDdlReason()
        //{
        //    SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
        //    try
        //    {
        //        MDMCon.Open();
        //        using (SqlDataAdapter sda = new SqlDataAdapter())
        //        {
        //            sql = @" 
        //                    select ReasonforRejection 
        //                    from TREASONFORMETREJECTION 
        //                    where DelFlag=0 and ReasonType='Revision Of eMET' and SysCode = 'emet'
        //                     and Plant=@Plant and DelFlag = 0";

        //            cmd = new SqlCommand(sql, MDMCon);
        //            cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
        //            sda.SelectCommand = cmd;
        //            using (DataTable dt = new DataTable())
        //            {
        //                sda.Fill(dt);
        //                Session["DdlReason"] = dt;
        //            }
        //            //UpdatePanel18.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        //    }
        //    finally
        //    {
        //        MDMCon.Dispose();
        //    }
        //}

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //protected void DdlFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TxtFilter.Text = "";
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        //protected void btnSearchSubProc_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GetQuoteList();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        //protected void TxtSubProc_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        //protected void DdlProcGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TxtFilter.Text = "";
        //        TxtSubProc.Text = "";
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //        if (DdlProcGroup.SelectedIndex > 0)
        //        {
        //            TxtSubProc.Enabled = true;
        //        }
        //        else
        //        {
        //            TxtSubProc.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void DdlMatClassDesc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TxtFilter.Text = "";
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void DdlProduct_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TxtFilter.Text = "";
        //        GetDdlMatClassDesc();
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void DdlReqType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TxtFilter.Text = "";
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "CloseLoading();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void BtnFindQuoteRef_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["CreateReqTemp"] = null;
        //        GvCreateReqTemp.DataSource = null;
        //        GvCreateReqTemp.DataBind();

        //        //UpdateDtGvTemp();
        //        TxtSubProc.Enabled = false;
        //        if (RbTeamShimano.Checked == true)
        //        {
        //            LbQoRefTitle.Text = "Find Quote Reference List (SBM)";
        //        }
        //        else
        //        {
        //            LbQoRefTitle.Text = "Find Quote Reference List (External)";
        //        }
        //        DvTitleNoData.Visible = true;
        //        DdlProduct.SelectedIndex = 0;
        //        DdlMatClassDesc.SelectedIndex = 0;
        //        DdlProcGroup.SelectedIndex = 0;
        //        TxtSubProc.Text = "";
        //        TxtSubProc.Enabled = false;
        //        TxtFilter.Text = "";
        //        GvQuoteRefList.DataSource = null;
        //        GvQuoteRefList.DataBind();
        //        DeleteNonRequest();
        //        BtnSubmitRequest.Visible = false;
        //        ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "ChgEmptyFlColor();", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void RbTeamShimano_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DeleteNonRequest();
        //        Session["GvTemp"] = null;
        //        Session["InvalidRequest"] = null;
        //        DvInvalidRequest.Visible = false;
        //        GvTemp.DataSource = null;
        //        TxtGvTempLeng.Text = "0";
        //        GvTemp.DataBind();
        //        BtnCreateReq.Visible = false;
        //        LbTitleQrefList.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}


        //protected void RbExternal_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DeleteNonRequest();
        //        Session["GvTemp"] = null;
        //        Session["InvalidRequest"] = null;
        //        DvInvalidRequest.Visible = false;
        //        GvTemp.DataSource = null;
        //        TxtGvTempLeng.Text = "0";
        //        GvTemp.DataBind();
        //        BtnCreateReq.Visible = false;
        //        LbTitleQrefList.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}
    }
}