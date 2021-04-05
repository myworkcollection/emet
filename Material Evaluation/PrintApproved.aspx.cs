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

using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using System.Threading;

namespace Material_Evaluation
{
    public partial class PrintApproved : System.Web.UI.Page
    {

        string PlantDesc = "";
        string SMNPICSubmDept = "";
        string GA = "";
        string DbMasterName = "";
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        SqlConnection conmaster;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Number"]))
                    {
                        string QuoteNo = Request.QueryString["Number"];
                        GetQuoteandAllDetails(QuoteNo);
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

        private void GetProcessDetailsbyQuoteDetails(string ProcessGrp)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                DataTable Result4 = new DataTable();
                SqlDataAdapter da4 = new SqlDataAdapter();
                string str4 = "select ScreenLayout from [dbo].[TPROCESGRP_SCREENLAYOUT] Where ProcessGrp = '" + ProcessGrp + "' ";
                da4 = new SqlDataAdapter(str4, MDMCon);
                Result4 = new DataTable();
                da4.Fill(Result4);

                if (Result4.Rows.Count > 0)
                {
                    hdnLayoutScreen.Value = Result4.Rows[0].ItemArray[0].ToString();
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

        private void GetQuoteandAllDetails(string QuoteNo)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                GetDbMaster();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                string str = @"select distinct TQ.VendorCode1,TQ.QuoteNo,(select distinct UseNam  from " + DbMasterName + @".dbo.Usr where UseID=TQ.CreatedBy) as ShimanoPIC,TQ.ShimanoPICEmail,
                                (select distinct CONCAT(TQ.Plant, ' - ', (select distinct Description from " + DbMasterName + @".dbo.TPLANT TP where TP.Plant=TQ.Plant ),' - ',(TQ.SMNPicDept) )) as 'PlantDept',
                                TQ.ERemarks,VN.Cty as 'Country',

                                (select distinct Crcy from " + DbMasterName + @".dbo.tVendor_New VN 
                                inner join " + DbMasterName + @".dbo.tVendorPOrg VP on VN.POrg = VP.POrg
                                inner join " + DbMasterName + @".dbo.TPOrgPlant PO on VP.Plant = PO.Plant
                                where VN.Vendor=TQ.VendorCode1 and PO.Plant=TQ.Plant) as 'Crcy',TQ.UpdatedBy,

                                (select distinct UseNam from " + DbMasterName + @".dbo.usr where UseID= TQ.UpdatedBy) as 'SubmitBy',
                                format(TQ.UpdatedOn, 'dd/MM/yyyy hh:mm:ss') as 'UpdatedOn',
                                (select distinct CONCAT(TQ.VendorCode1, ' - ', VN.Description )) as 'vendor',

                                TQ.Product,TQ.Material,TQ.MaterialDesc,TQ.PIRJobType,TQ.PIRType,TQ.DrawingNo,
                                TQ.ProcessGroup,TQ.BaseUOM,TQ.ActualNU,TQ.UOM,TQ.MQty,
                                (case 
	                                when (select top 1 Z.NewEffectiveDate from TMngEffDateChgLog Z where Z.QuoteNo = '" + QuoteNo + @"' order by z.CreatedOn desc) is null then format(TQ.EffectiveDate,'dd/MM/yyyy') 
	                                else (select top 1 format(Z.NewEffectiveDate, 'dd/MM/yyyy') from TMngEffDateChgLog Z where Z.QuoteNo ='" + QuoteNo + @"')
                                END) as 'EffectiveDate',
                                (case 
	                                when (select top 1 Z.NewDueOn from TMngEffDateChgLog Z where Z.QuoteNo = '" + QuoteNo + @"' order by z.CreatedOn desc) is null then format(TQ.DueOn,'dd/MM/yyyy') 
	                                else (select top 1 format(Z.NewDueOn, 'dd/MM/yyyy') from TMngEffDateChgLog Z where Z.QuoteNo = '" + QuoteNo + @"' order by z.CreatedOn desc)
                                END) as 'DueOn',
                                (select concat(TQ.CountryOrg,' - ',(select distinct TC.CountryDescription from " + DbMasterName + @".dbo.TCountrycode TC where TC.CountryCode = TQ.CountryOrg )) ) as 'CountryOrg',
                                TQ.CommentByVendor,

                                case 
                                when TQ.ManagerApprovalStatus = 2 and TQ.DIRApprovalStatus = 0 then 'Approved' 
                                else 'Rejected'
                                end as 'AppStat',

                                TQ.DIRReason,TQ.DIRRemark,
                                (select distinct UseNam from " + DbMasterName + @".dbo.usr where UseID= TQ.AprRejBy) as 'AprRejBy',
                                format(TQ.AprRejDate, 'dd/MM/yyyy') as AprRejDate, 
								 (case when TQ.PICReason is not null then TQ.PICReason else ERemarks end ) as 'RequestReason'
                                from TQuoteDetails TQ
                                inner join " + DbMasterName + @".dbo.tVendor_New VN on TQ.VendorCode1 = VN.Vendor
                                where TQ.QuoteNo = '" + QuoteNo + @"' and CreateStatus is not null ";
                da = new SqlDataAdapter(str, EmetCon);
                da.Fill(dtdate);

                if (dtdate.Rows.Count > 0)
                {
                    GetProcessDetailsbyQuoteDetails(dtdate.Rows[0]["ProcessGroup"].ToString());

                    #region Shimano deatail
                    LbSMNPIC.Text = ": " + dtdate.Rows[0]["ShimanoPIC"].ToString();
                    LblEmail.Text = ": " + dtdate.Rows[0]["ShimanoPICEmail"].ToString() ;
                    LblPlnDept.Text = ": " + dtdate.Rows[0]["PlantDept"].ToString();
                    LblPurposeQuote.Text = ": " + dtdate.Rows[0]["ERemarks"].ToString();
                    #endregion

                    #region Vendor deatail
                    LbSubmitBy.Text = ": " + dtdate.Rows[0]["UpdatedBy"].ToString() + "-" + dtdate.Rows[0]["SubmitBy"].ToString();
                    LbSubmitDate.Text = ": " + dtdate.Rows[0]["UpdatedOn"].ToString();
                    LbVendor.Text = ": " + dtdate.Rows[0]["vendor"].ToString();
                    LbCountry.Text = ": " + dtdate.Rows[0]["Country"].ToString();
                    LbCurrency.Text = ": " + dtdate.Rows[0]["Crcy"].ToString();
                    LbQuoteNo.Text = ": " + dtdate.Rows[0]["QuoteNo"].ToString();
                    LbCmntVnd.Text = ": " + dtdate.Rows[0]["CommentByVendor"].ToString();
                    #endregion

                    #region quote Part info
                    LbProduct.Text = ": " + dtdate.Rows[0]["Product"].ToString();
                    lbPartnDesc.Text = ": " + dtdate.Rows[0]["Material"].ToString() + "-" + dtdate.Rows[0]["MaterialDesc"].ToString();
                    LbSAPPIRJobType.Text = ": " + dtdate.Rows[0]["PIRJobType"].ToString();
                    LbPIRType.Text = ": " + dtdate.Rows[0]["PIRType"].ToString();
                    LbPartDrawing.Text = ": " + dtdate.Rows[0]["DrawingNo"].ToString();
                    LbProcGroup.Text = ": " + dtdate.Rows[0]["ProcessGroup"].ToString();
                    LbBaseUOM.Text = ": " + dtdate.Rows[0]["BaseUOM"].ToString();
                    LbNetWeight.Text = ": " + dtdate.Rows[0]["ActualNU"].ToString() + " " + dtdate.Rows[0]["UOM"].ToString();
                    LbMQty.Text = ": " + dtdate.Rows[0]["MQty"].ToString() + " " + dtdate.Rows[0]["BaseUOM"].ToString();
                    LbQuoteEffDate.Text = ": " + dtdate.Rows[0]["EffectiveDate"].ToString();
                    LbDueDate.Text = ": " + dtdate.Rows[0]["DueOn"].ToString();
                    LbCrOrgi.Text = ": " + dtdate.Rows[0]["CountryOrg"].ToString();
                    LbReqReason.Text = ": " + dtdate.Rows[0]["RequestReason"].ToString();
                    //if (dtdate.Rows[0]["EffectiveDate"].ToString() != "")
                    //{
                    //    DateTime EffectiveDate = DateTime.Parse(dtdate.Rows[0]["EffectiveDate"].ToString());
                    //    LbQuoteEffDate.Text = ": " + dtdate.Rows[0]["EffectiveDate"].ToString();
                    //}
                    //if (dtdate.Rows[0]["DueOn"].ToString() != "")
                    //{
                    //    DateTime DueOn = DateTime.Parse(dtdate.Rows[0]["DueOn"].ToString());
                    //    LbDueDate.Text = ": " + dtdate.Rows[0]["DueOn"].ToString();
                    //}
                    #endregion

                    #region Management Decision
                    LbApprStatus.Text = ": " + dtdate.Rows[0]["AppStat"].ToString();
                    if (dtdate.Rows[0]["DIRReason"].ToString().Replace("Apr:", "").Trim() != "")
                    {
                        LbApprCmnt.Text = ": " + dtdate.Rows[0]["DIRReason"].ToString().Replace("Apr:", "");
                    }
                    else
                    {
                        LbApprCmnt.Text = ": " + dtdate.Rows[0]["DIRRemark"].ToString();
                    }
                    LbApprvalBy.Text = ": " + dtdate.Rows[0]["AprRejBy"].ToString();
                    LbApprDate.Text = ": " + dtdate.Rows[0]["AprRejDate"].ToString();
                    #endregion

                    #region Create Table Unit
                    string ProcGroup = dtdate.Rows[0]["ProcessGroup"].ToString();
                    string FieldGroup = "Unit";
                    string VndCode = dtdate.Rows[0]["VendorCode1"].ToString();
                    string Qno = dtdate.Rows[0]["QuoteNo"].ToString();
                    GetMETfields(ProcGroup, FieldGroup);
                    RetrieveAllCostDetails(Qno);
                    if (IsTeamShimano(VndCode) == false)
                    {
                        hdnVendorType.Value = "External";
                        CreateDynamicUnitDT(0);
                    }
                    else
                    {
                        hdnVendorType.Value = "TeamShimano";
                        CreateDynamicUnitDTTmShimano(0);
                    }
                    #endregion
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

        public void GetMETfields(string ProcessGrpCode, string FieldGroup)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                #region access layout from New table
                MDMCon.Open();
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
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString();
                DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        private void RetrieveAllCostDetails(string Qno)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();

                #region Process Cost
                dt = new DataTable();
                sql = @" select distinct [TotalProcessesCost/pcs] as TotProc from TProcessCostDetails where QuoteNo= '" + Qno + "'";
                da = new SqlDataAdapter(sql, EmetCon);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    HdnTotProcCostBeforeProfNDisc.Value = dt.Rows[0]["TotProc"].ToString();
                }
                #endregion SubMat Cost

                #region Unit Cost
                dt = new DataTable();
                sql = @" select CAST(ROUND(TotalMaterialCost,5) AS DECIMAL(12,5)) as 'TotalMaterialCost',
                            CAST(ROUND(TotalProcessCost,5) AS DECIMAL(12,5)) as 'TotalProcessCost',
                            CAST(ROUND(TotalSubMaterialCost,5) AS DECIMAL(12,5)) as 'TotalSubMaterialCost',
                            CAST(ROUND(TotalOtheritemsCost,5) AS DECIMAL(12,5)) as 'TotalOtheritemsCost',
                            CAST(ROUND(GrandTotalCost,5) AS DECIMAL(12,5)) as 'GrandTotalCost',
                            Profit,Discount,
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
                        ) + '%' as 'NetProfit/Discount',GA
                        from TQuoteDetails  Where QuoteNo = '" + Qno + "'";
                da = new SqlDataAdapter(sql, EmetCon);
                da.Fill(dt);
                StringBuilder sbUnit = new StringBuilder();

                //string hdnUnittemp = "";
                if (dt.Rows.Count > 0)
                {
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var txtTMCost = dt.Rows[i]["TotalMaterialCost"].ToString();
                        var txtTPCost = dt.Rows[i]["TotalProcessCost"].ToString();
                        var txtTSCost = dt.Rows[i]["TotalSubMaterialCost"].ToString();
                        var txtTOCost = dt.Rows[i]["TotalOtheritemsCost"].ToString();
                        var txtGrantCost = dt.Rows[i]["GrandTotalCost"].ToString();
                        var txtProfit = dt.Rows[i]["Profit"].ToString();
                        var txtDiscount = dt.Rows[i]["Discount"].ToString();
                        var txtfinalCost = dt.Rows[i]["FinalQuotePrice"].ToString();

                        //hdnTMatCost.Value = txtTMCost;
                        //hdnTProCost.Value = txtTPCost;
                        //hdnTSumMatCost.Value = txtTSCost;
                        //hdnTOtherCost.Value = txtTOCost;
                        //hdnTGTotal.Value = txtGrantCost;
                        //hdnTFinalQPrice.Value = txtfinalCost;

                        sbUnit.Append(txtTMCost + "," + txtTPCost + "," + txtTSCost + "," + txtTOCost + "," + txtGrantCost + "," + txtProfit + "," + txtDiscount + "," + txtfinalCost + ",");
                    }
                    hdnUnitValues.Value = sbUnit.ToString();
                    hdnNetProfDisc.Value = dt.Rows[0]["NetProfit/Discount"].ToString();
                    hdnGA.Value = dt.Rows[0]["GA"].ToString();
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

        #region New 25-06-2019 CreateDynamicOthersCostDT(int ColumnType)
        private void CreateDynamicUnitDT(int ColumnType)
        {
            DataTable DtDynamicUnitFields = new DataTable();
            DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

            if (ColumnType == 0)
            {
                int rowcountnew = 0;

                TableRow Hearderrow = new TableRow();

                TableUnit.Rows.Add(Hearderrow);
                for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
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
                        if (hdnLayoutScreen.Value == "Layout7")
                        {
                            tCell1.Text = "Final Quote Price/UOM";
                        }
                        else
                        {
                            tCell1.Text = "Final Quote Price/pc";
                        }
                    }
                    tCell1.Font.Size = 16;
                    Hearderrow.Cells.Add(tCell1);
                    //Hearderrow.BackColor = ColorTranslator.FromHtml("#000000");
                    tCell1.BorderWidth = 2;
                    tCell1.BorderColor = Color.Black;
                    Hearderrow.ForeColor = Color.Black;
                }

                // Table1 = (Table)Session["Table"];
                for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                {
                    TableRow tRow = new TableRow();
                    TableUnit.Rows.Add(tRow);

                    for (int i = 0; i <= 1; i++)
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
                                tCell.Font.Size = 16;
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        else if (i == 1)
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
                            tCell.Style.Add("text-align", "center");
                            tCell.Font.Size = 16;
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
            //else
            //{
            //    //  int Rowscount = -1;
            //    TableUnit = (Table)Session["TableUnit"];

            //    for (int cellCtr = 0; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
            //    {
            //        TableCell tCell = new TableCell();
            //        TableUnit.Rows[cellCtr].Cells.Add(tCell);
            //    }
            //    Session["TableUnit"] = TableUnit;
            //}
        }

        private void CreateDynamicUnitDTTmShimano(int ColumnType)
        {
            DataTable DtDynamicUnitFields = new DataTable();
            DtDynamicUnitFields = (DataTable)Session["DtDynamicUnitFields"];

            if (ColumnType == 0)
            {
                int rowcountnew = 0;

                TableRow Hearderrow = new TableRow();

                TableUnit.Rows.Add(Hearderrow);
                for (int cellCtr = 0; cellCtr <= 1; cellCtr++)
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
                        if (hdnLayoutScreen.Value == "Layout7")
                        {
                            tCell1.Text = "Final Quote Price/UOM";
                        }
                        else
                        {
                            tCell1.Text = "Final Quote Price/pc";
                        }
                    }
                    tCell1.Font.Size = 16;
                    Hearderrow.Cells.Add(tCell1);
                    //Hearderrow.BackColor = ColorTranslator.FromHtml("#000000");
                    tCell1.BorderWidth = 2;
                    tCell1.BorderColor = Color.Black;
                    Hearderrow.ForeColor = Color.Black;
                }

                // Table1 = (Table)Session["Table"];
                for (int cellCtr = 1; cellCtr <= DtDynamicUnitFields.Rows.Count; cellCtr++)
                {
                    TableRow tRow = new TableRow();
                    TableUnit.Rows.Add(tRow);

                    for (int i = 0; i <= 1; i++)
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
                                tCell.Font.Size = 16;
                                TableUnit.Rows[cellCtr].Cells.Add(tCell);
                            }
                        }
                        else if (i == 1)
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
                            tCell.Font.Size = 16;
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
        #endregion New 25-06-2019 CreateDynamicUnitDT(int ColumnType)

        protected void Exportpdf()
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);

            WebKitConverterSettings settings = new WebKitConverterSettings();

            //Set WebKit path
            settings.WebKitPath = Server.MapPath("~/QtBinaries");

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;

            //Get the current URL
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            //Convert URL to PDF
            Syncfusion.Pdf.PdfDocument document = htmlConverter.Convert(url);

            //Save the document
            document.Save("Approved MET.pdf", HttpContext.Current.Response, HttpReadType.Save);
        }

        public void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
        {
            try
            {
                object TargetFile = FileName;
                string ModifiedFileName = string.Empty;
                string FinalFileName = string.Empty;

                /* To add a Password to PDF -http://aspnettutorialonline.blogspot.com/ */
                TestPDF.HtmlToPdfBuilder builder = new TestPDF.HtmlToPdfBuilder(iTextSharp.text.PageSize.A4);
                TestPDF.HtmlPdfPage first = builder.AddPage();
                first.AppendHtml(HtmlStream);
                byte[] file = builder.RenderPdf();
                File.WriteAllBytes(TargetFile.ToString(), file);

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(TargetFile.ToString());
                ModifiedFileName = TargetFile.ToString();
                ModifiedFileName = ModifiedFileName.Insert(ModifiedFileName.Length - 4, "1");

                string password = "";
                //iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, password, "", iTextSharp.text.pdf.PdfWriter.AllowPrinting);

                iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, password, "", iTextSharp.text.pdf.PdfWriter.AllowPrinting | iTextSharp.text.pdf.PdfWriter.AllowCopy);
                reader.Dispose();
                if (File.Exists(TargetFile.ToString()))
                    File.Delete(TargetFile.ToString());
                FinalFileName = ModifiedFileName.Remove(ModifiedFileName.Length - 5, 1);
                File.Copy(ModifiedFileName, FinalFileName);
                if (File.Exists(ModifiedFileName))
                    File.Delete(ModifiedFileName);

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }
        
        protected void GenerateDesignPdf()
        {
            try
            {
                string TotMatCost = "";
                string TotProcCost = "";
                string TotSubMatCost = "";
                string TotOthCost = "";
                string GrandTotCost = "";
                string UOM = "pc";
                if (hdnLayoutScreen.Value == "Layout7")
                {
                    UOM = "UOM";
                }

                if (Session["TableUnit"] != null)
                {
                    Table TbUnit = (Table)Session["TableUnit"];
                    if (TbUnit.Rows.Count > 0)
                    {
                        TotMatCost = TbUnit.Rows[1].Cells[1].Text;
                        TotProcCost = TbUnit.Rows[2].Cells[1].Text;
                        TotSubMatCost = TbUnit.Rows[3].Cells[1].Text;
                        TotOthCost = TbUnit.Rows[4].Cells[1].Text;
                        GrandTotCost = TbUnit.Rows[5].Cells[1].Text;
                    }
                }

                string str =
                @"<HTML>
                <head>
                </head>
                <body>
                     <h3 style='text-align: center;'><strong>SHIMANO e-MET</strong></h3>
                     <br>
                     <p style='font-size: 8px;'>
                        <table border='0'>
                            <thead>
                                <tr>
                                    <th colspan='2' border='1'><b>SHIMANO DETAILS</b></th>
                                </tr>
                            </thead>
                            <tbody>
                               <tr>
                                    <td  width='20%'>SMN PIC</td> 
                                    <td  width='80%'> " + LbSMNPIC.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Plant & Department</td>
                                    <td  width='80%'> " + LblPlnDept.Text + @" </td>
                                </tr>
                            </tbody>
                        </table>
                     </P>
                    <br>
                    <p style='font-size: 8px;'>
                        <table border='0'>
                            <thead>
                                <tr>
                                    <th colspan='2' border='1'><b>VENDOR DETAILS</b></th>
                                </tr>
                            </thead>
                            <tbody>
                               <tr >
                                    <td  width='20%'>Submitted By </td> 
                                    <td  width='80%'> " + LbSubmitBy.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Submitted Date</td>
                                    <td  width='80%'> " + LbSubmitDate.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Vendor</td>
                                    <td  width='80%'> " + LbVendor.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Country</td>
                                    <td  width='80%'> " + LbCountry.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Currency</td>
                                    <td  width='80%'> " + LbCurrency.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Quote No </td>
                                    <td  width='80%'> " + LbQuoteNo.Text + @"  </td>
                                </tr>
                            </tbody>
                        </table>
                     </P>
                    <br>
                    <p style='font-size: 8px;'>
                        <table border='0'>
                            <thead>
                                <tr>
                                    <th colspan='2' border='1'><b>PART I : QUOTED PART INFO</b></th>
                                </tr>
                            </thead>
                            <tbody>
                               <tr >
                                    <td  width='20%'>Product </td> 
                                    <td  width='80%'> " + LbProduct.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Part Code & Desc</td>
                                    <td  width='80%'> " + lbPartnDesc.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>SAP PIR Job Type & Desc</td>
                                    <td  width='80%'> " + LbSAPPIRJobType.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>PIR Type & Desc</td>
                                    <td  width='80%'> " + LbPIRType.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Process Group</td>
                                    <td  width='80%'> " + LbProcGroup.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Base UOM </td>
                                    <td  width='80%'> " + LbBaseUOM.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Net Weight</td>
                                    <td  width='80%'> " + LbNetWeight.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Mnth.Est.Qty & UOM </td>
                                    <td  width='80%'> " + LbMQty.Text + @"  </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>SMN Effective Date</td>
                                    <td  width='80%'> " + LbQuoteEffDate.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>SMN Next Revision</td>
                                    <td  width='80%'> " + LbDueDate.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Country of Origin </td>
                                    <td  width='80%'> " + LbCrOrgi.Text + @"  </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Request Purposes</td>
                                    <td  width='80%'> " + LbReqReason.Text + @"  </td>
                                </tr>
                            </tbody>
                        </table>
                     </P>
                    <br>
                    <p style='font-size: 8px;'>
                        <table border='1'>
                            <thead>
                                <tr>
                                    <th colspan='2' border='1'><b>PART VI : PART UNIT PRICE</b></th>
                                </tr>
                            </thead>
                            <tbody>
                               <tr >
                                    <td  width='30%'><b>Field Name </b></td> 
                                    <td  width='70%'><b> Final Quote Price/" + UOM + @" </b></td>
                                </tr>
                                <tr >
                                    <td  width='30%'>Total Material Cost/" + UOM + @"</td>
                                    <td  width='70%'> " + TotMatCost + @" </td>
                                </tr>
                                <tr >
                                    <td  width='30%'>Total Process Cost/" + UOM + @" </td>
                                    <td  width='70%'> " + TotProcCost + @"  </td>
                                </tr>
                                <tr >
                                    <td  width='30%'>Total Sub-Mat/T&J Cost/" + UOM + @"</td>
                                    <td  width='70%'> " + TotSubMatCost + @" </td>
                                </tr>
                                <tr >
                                    <td  width='30%'>Total Other Item Cost/" + UOM + @"</td>
                                    <td  width='70%'> " + TotOthCost + @" </td>
                                </tr>
                                <tr >
                                    <td  width='30%'>Grand Total Cost/" + UOM + @" </td>
                                    <td  width='70%'> " + GrandTotCost + @"  </td>
                                </tr>
                            </tbody>
                        </table>
                     </P>
                     <br>
                    <p style='font-size: 8px;'>
                        <table border='0'>
                            <thead>
                                <tr>
                                    <th colspan='2' border='1'><b>MANAGEMENT DECISION : APPROVED</b></th>
                                </tr>
                            </thead>
                            <tbody>
                               <tr >
                                    <td  width='20%'>Approved By </td> 
                                    <td  width='80%'> " + LbApprvalBy.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Approval Date</td>
                                    <td  width='80%'> " + LbApprDate.Text + @" </td>
                                </tr>
                                <tr >
                                    <td  width='20%'>Approval Comment </td>
                                    <td  width='80%'> " + LbApprCmnt.Text + @"  </td>
                                </tr>
                            </tbody>
                        </table>
                     </P>
                </body>
                </html>";

                //insert html into notepad
                File.WriteAllText(Server.MapPath("~/Files/" + Session["userID"].ToString() + ".html"), str);

                string strHtml = string.Empty;
                //HTML File path
                string htmlFileName = Server.MapPath("~") + "\\Files\\" + Session["userID"].ToString() + ".html";

                //HTML File path to C:/Users/USER/Downloads/
                //string htmlFileName = Path.Combine(@"C:/Users/USER/Downloads/") + "report.htm";

                //pdf file path.
                string pdfFileName = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".pdf";

                //pdf filte path to C:/Users/USER/Downloads/
                //string pdfFileName = Path.Combine(@"C:/Users/USER/Downloads/") + SRVNumer + refno + ".pdf";

                //reading html code from html file
                FileStream fsHTMLDocument = new FileStream(htmlFileName, FileMode.Open, FileAccess.Read);
                StreamReader srHTMLDocument = new StreamReader(fsHTMLDocument);
                strHtml = srHTMLDocument.ReadToEnd();
                srHTMLDocument.Dispose();

                strHtml = strHtml.Replace("\r\n", "");
                strHtml = strHtml.Replace("\0", "");

                CreatePDFFromHTMLFile(strHtml, pdfFileName);

                //download pdf
                try
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Approved MET.pdf");
                    Response.TransmitFile(("Files/" + Session["userID"].ToString() + ".pdf"));
                    Response.Flush();
                    Response.End();
                }
                catch (ThreadAbortException ex2)
                {

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        string path = "";
                        if (i == 1)
                        {
                            path = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".pdf";
                        }
                        else
                        {
                            path = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".html";
                        }
                        FileInfo file = new FileInfo(path);
                        if (file.Exists) //check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                //LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                //EMETModule.SendExcepToDB(ex);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GenerateDesignPdf();
            //Exportpdf();
        }
    }
}