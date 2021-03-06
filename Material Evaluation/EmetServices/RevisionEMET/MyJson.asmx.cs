using Material_Evaluation.EmetServices.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Material_Evaluation.EmetServices.RevisionEMET
{
    /// <summary>
    /// Summary description for MyJson
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyJson : System.Web.Services.WebService
    {
        string DbMasterName = "";
        SqlCommand cmd = new SqlCommand();

        protected void GetDbMaster()
        {
            try
            {
                DbMasterName = EMETModule.GetDbMastername();
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                DbMasterName = "";
            }
        }

        string GetQuoteNextRevDate(string plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string DefVal = "";
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select DefValue from DefaultValueMaster where Description = 'Quote Next Rev Date' and  Plant='" + plant + "' and DelFlag=0 ";

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

        DateTime DateDuenextRev(string Plant)
        {
            DateTime DtDuenextRev = new DateTime();
            try
            {
                string DefQuoteNextRev = GetQuoteNextRevDate(Plant);
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
                EMETModule.SendExcepToDB(ex);
                return DtDuenextRev;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void HelloWorld()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            GlobalResult MyResult = new GlobalResult();
            MyResult.success = true;
            MyResult.message = "Hello Word";
            //string strJSON = js.Serialize(MyResult);
            Context.Response.Write(js.Serialize(MyResult));
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetQuoteList(bool ActiveMaterial, string ReqType, string Product, string MatClassDesc, 
            string ProcGroup, string SubProc, string Filter, string FilterValue, bool IsExternal,
            string VndVsMaterialInvalidData, string VndVsMaterialSelectedData)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" With CTE
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

                            ,case 
							when ((select count(*) from TToolAmortization TT where TT.QuoteNo = TQ.QuoteNo ) > 0) then 1 
							else 0 end as 'ToolAmorExist'
							,case 
							when ((select count(*) from TToolAmortization TT where TT.QuoteNo = TQ.QuoteNo ) = 0) then 0 
							when ((select count(*) from " + DbMasterName + @".dbo.TToolAmortization TT 
                                   join TQuoteDetails TQ2 ON TQ2.QuoteNo = TQ.QuoteNo  
								   join TToolAmortization TT2 ON TQ.QuoteNo = TT2.QuoteNo and TT.plant = TQ.plant and  TT.VendorCode = TQ.VendorCode1 
								   and  TT.Process_Grp_code = TQ.ProcessGroup and  TT.Amortize_Tool_ID = TT2.Amortize_Tool_ID and  isnull(DelFlag,0)=0
                                  where CURRENT_TIMESTAMP between tt.EffectiveFrom and tt.DueDate
								  ) > 0 ) then 0
							else 1 end as 'ToolAmorExpired'
                            
							,case 
							when ((select count(*) from TMachineAmortization TM where TM.QuoteNo = TQ.QuoteNo ) > 0) then 1
							else 0 end as 'MacAmorExist'
							,case 
							when ((select count(*) from TMachineAmortization TM where TM.QuoteNo = TQ.QuoteNo ) = 0) then 0
							when ((select count(*) from " + DbMasterName + @".dbo.TMachineAmortization TM 
							join TQuoteDetails TQ2 ON TQ2.QuoteNo = TQ.QuoteNo  and TM.plant = TQ.Plant and TM.VendorCode = TQ.Vendorcode1 and TM.Process_Grp_code in (select Process_Grp_code from TProcessCostDetails PC where PC.QuoteNo = TQ2.QuoteNo)
							join TMachineAmortization TM2 on TM2.QuoteNo = TQ.QuoteNo and  TM.Process_Grp_code = TM2.Process_Grp_code and TM.Vend_MachineID = TM2.Vend_MachineID and tm.VendorCode = tm2.VendorCode and isnull(DelFlag,0)=0 
                            where (CURRENT_TIMESTAMP between TM.EffectiveFrom and TM.DueDate)
							) > 0 ) then 0
							else 1 end as 'MacAmorExpired'
                            
                            ,(select distinct top 1 upper(ScreenLayout) from " + DbMasterName + @".dbo.TPROCESGRP_SCREENLAYOUT PSL
                            WHERE isnull(PSL.DelFlag,0) = 0 and TQ.ProcessGroup = PSL.ProcessGrp) as Layout

                            from TQuoteDetails TQ 
                            inner join TProcessCostDetails TP on TQ.QuoteNo = TP.QuoteNo
                            INNER JOIN " + DbMasterName + @".dbo.tvendorporg TVPo ON TQ.VendorCode1 = TVPo.Vendor
                            INNER JOIN " + DbMasterName + @".dbo.TPOrgPlant TPPo ON TVPo.porg = TPPo.POrg
                            inner join " + DbMasterName + @".dbo.TMATERIAL TM on tq.Plant = tm.Plant and tq.Material = tm.Material  and ISNULL(tm.DelFlag,0)=0
                            where (TQ.ApprovalStatus='3') ";
                    if (ActiveMaterial == true)
                    {
                        sql += @" and isnull(tm.PlantStatus,'') not in ('z4','z9') ";
                    }
                    sql += @"  
                            --and (TQ.PICApprovalStatus='2') 
                            --and (isMassRevision = 0 or isMassRevision is null)
                            and (TQ.ManagerApprovalStatus='2') and (TQ.DIRApprovalStatus='0') and TQ.Plant= @Plant ";

                    if (ReqType == "New")
                    {
                        sql += @" and ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is null or QuoteNoRef = '')) ";
                    }
                    else if (ReqType == "Revision")
                    {
                        sql += @" and ((isMassRevision = 0 or isMassRevision is null) and (QuoteNoRef is not null or QuoteNoRef <> '')) ";
                    }
                    else if (ReqType == "MassRev")
                    {
                        sql += @" and (isMassRevision = 1) ";
                    }

                    if (Product != "0" && Product != "")
                    {
                        sql += @" and TQ.Product=@Product ";
                    }

                    if (MatClassDesc != "" && MatClassDesc != "0")
                    {
                        sql += @" and TQ.MaterialClass = @MaterialClass ";
                    }

                    if (ProcGroup != "0" && ProcGroup != "")
                    {
                        sql += @" and (select CONCAT(TQ.ProcessGroup,' - ', (select Process_Grp_Description from " + DbMasterName + @".[dbo].TPROCESGROUP_LIST TP where TP.Process_Grp_code=TQ.ProcessGroup) )) = @ProcessGroup ";
                    }
                    if (SubProc != "" && SubProc != "0")
                    {
                        sql += @" and TP.SubProcess like '%'+@SubProc+'%' ";
                    }

                    if (Filter != "" && Filter != "0")
                    {
                        if (Filter == "VendorCode")
                        {
                            sql += @" and TQ.VendorCode1 like '%'+@Filter+'%' ";
                        }
                        else if (Filter == "VendorName")
                        {
                            sql += @" and TQ.VendorName like '%'+@Filter+'%' ";
                        }
                        else if (Filter == "Material")
                        {
                            sql += @" and TQ.Material like '%'+@Filter+'%' ";
                        }
                        else if (Filter == "MaterialDesc")
                        {
                            sql += @" and TQ.MaterialDesc like '%'+@Filter+'%' ";
                        }
                        else if (Filter == "QuoteNo")
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
                        else if (Filter == "CreatedBy")
                        {
                            sql += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=TQ.CreatedBy) like '%'+@Filter+'%' ";
                        }
                        else if (Filter == "UseDep")
                        {
                            sql += @"  and TQ.SMNPicDept like '%'+@Filter+'%' ";
                        }
                    }

                    if (IsExternal == true)
                    {
                        sql += @" and TQ.VendorCode1 not in (select TS.VendorCode from " + DbMasterName + @".dbo.TSBMPRICINGPOLICY TS) ";
                    }
                    else
                    {
                        sql += @" and TQ.VendorCode1 in (select TS.VendorCode from " + DbMasterName + @".dbo.TSBMPRICINGPOLICY TS) ";
                    }

                    sql += @" )
                            Select *
                            From CTE
                            Where ROW_NUM = 1 
                            ";

                    if (VndVsMaterialInvalidData.Trim() != "") {
                        sql += @" and (concat(VendorCode1,'-',Material) not in ( "+ VndVsMaterialInvalidData + " ) ) ";
                    }
                    if (VndVsMaterialSelectedData.Trim() != "")
                    {
                        sql += @" and (concat(VendorCode1,'-',Material) not in ( " + VndVsMaterialSelectedData + " ) ) ";
                    }
                    
                    cmd = new SqlCommand(sql, EmetCon);
                    if (Product != "" && Product != "0")
                    {
                        cmd.Parameters.AddWithValue("@Product", Product);
                    }
                    if (MatClassDesc != "" && MatClassDesc != "0")
                    {
                        cmd.Parameters.AddWithValue("@MaterialClass", MatClassDesc);
                    }
                    if (ProcGroup != "" && ProcGroup != "0")
                    {
                        cmd.Parameters.AddWithValue("@ProcessGroup", ProcGroup);
                    }
                    if (SubProc != "" && SubProc != "0")
                    {
                        cmd.Parameters.AddWithValue("@SubProc", SubProc);
                    }
                    if (Filter != "" && Filter != "0")
                    {
                        cmd.Parameters.AddWithValue("@Filter", FilterValue);
                    }
                    cmd.Parameters.AddWithValue("@Plant", HttpContext.Current.Session["EPlant"].ToString());
                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                    HttpContext.Current.Session["GvQuoteRefList"] = DtResult;
                    HttpContext.Current.Session["GvQuoteRefListTemp"] = DtResult;
                }

                //JavaScriptSerializer js = new JavaScriptSerializer();
                //Context.Response.Clear();
                //Context.Response.ContentType = "application/json";
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Row_Num = row["Row_Num"],
                    QuoteNo = row["QuoteNo"],
                    VendorCode1 = row["VendorCode1"],
                    VendorName = row["VendorName"],
                    SearchTerm = row["SearchTerm"],
                    Product = row["Product"],
                    MaterialClass = row["MaterialClass"],
                    MaterialType = row["MaterialType"],
                    PlantStatus = row["PlantStatus"],
                    SAPProcType = row["SAPProcType"],
                    SAPSpProcType = row["SAPSpProcType"],
                    Material = row["Material"],
                    MaterialDesc = row["MaterialDesc"],
                    PlatingType = row["PlatingType"],
                    PIRType = row["PIRType"],
                    NetUnit = row["NetUnit"],
                    UOM = row["UOM"],
                    MQty = row["MQty"],
                    BaseUOM = row["BaseUOM"],
                    ProcessGroup = row["ProcessGroup"],
                    PrcGrpDesc = row["PrcGrpDesc"],
                    PIRJobType = row["PIRJobType"],
                    QuoteResponseDueDate = row["QuoteResponseDueDate"],
                    TotalMaterialCost = row["TotalMaterialCost"],
                    TotalProcessCost = row["TotalProcessCost"],
                    TotalSubMaterialCost = row["TotalSubMaterialCost"],
                    TotalOtheritemsCost = row["TotalOtheritemsCost"],
                    GrandTotalCost = row["GrandTotalCost"],
                    FinalQuotePrice = row["FinalQuotePrice"],
                    UpdatedOn = row["UpdatedOn"],
                    ReqType = row["ReqType"],
                    MMPlantStatus = row["MMPlantStatus"],
                    ToolAmorExist = row["ToolAmorExist"],
                    ToolAmorExpired = row["ToolAmorExpired"],
                    MacAmorExist = row["MacAmorExist"],
                    MacAmorExpired = row["MacAmorExpired"],
                    Layout = row["Layout"]
                });

                MyResult.QueteRef = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
                //return new JavaScriptSerializer().Serialize(MyResult);
                //return new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDdlReason(string ReasonType)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" 
                            select ReasonforRejection 
                            from TREASONFORMETREJECTION 
                            where DelFlag=0 and ReasonType=@ReasonType and SysCode = 'emet'
                             and Plant=@Plant and DelFlag = 0";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", HttpContext.Current.Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@ReasonType", ReasonType);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                }
                DropDownData MyResult = new DropDownData();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Value = row["ReasonforRejection"],
                    Text = row["ReasonforRejection"]
                });

                MyResult.MyData = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetImRecycleRatio()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select distinct RecycleRatio from tIMRecycleratio where Plant='" + HttpContext.Current.Session["EPlant"].ToString() + @"' and isnull(DelFlag,0)=0 
                             order by RecycleRatio asc";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                }

                DropDownData MyResult = new DropDownData();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Value = row["RecycleRatio"],
                    Text = row["RecycleRatio"]
                });

                MyResult.MyData = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDateDuenextRev()
        {
            try
            {
                DateTime MyDt = DateDuenextRev(HttpContext.Current.Session["EPlant"].ToString());

                GlobalResult MyResult = new GlobalResult();
                MyResult.success = true;
                MyResult.message = MyDt.ToString("yyyy-MM-dd");

                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDdlProduct()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select distinct PR.product,CONCAT(PR.product,+ ' - '+ PR.Description) as productdescription from TPRODUCT as PR 
                            inner join TPLANT as p on p.plant=pr.plant 
                            inner join TSMNProductPIC TPIC on PR.product = TPIC.product
                            where p.plant=@Plant and PR.DelFlag = 0";

                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                }

                DropDownData MyResult = new DropDownData();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Value = row["product"],
                    Text = row["productdescription"]
                });

                MyResult.MyData = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDdlProcGroup()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select CONCAT(Process_Grp_code,' - ',Process_Grp_Description) as 'procgroup' 
                            from TPROCESGROUP_LIST
                            where DelFlag=0";

                    cmd = new SqlCommand(sql, MDMCon);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                }

                DropDownData MyResult = new DropDownData();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Value = row["procgroup"],
                    Text = row["procgroup"]
                });

                MyResult.MyData = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDdlMatClassDesc(string Product)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" Select Distinct TR.ProdComDesc from TMATERIAL TM 
                                    Inner join TPRODCOM TR on TM.ProdComCode = TR.ProdComCode 
                                where  TM.product=@product and TM.SplPROCTYPE is null  and TM.Plant = @plant and TM.delflag = 0 ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@product", Product);
                    sda.SelectCommand = cmd;
                    sda.Fill(DtResult);
                }

                DropDownData MyResult = new DropDownData();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    Value = row["ProdComDesc"],
                    Text = row["ProdComDesc"]
                });

                MyResult.MyData = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVendorToolAmor(string VendorCode, string processgrp, bool IsExternal,string EffectiveDate, string Material)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            DataTable DtResult = new DataTable();
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select distinct TVP.VendorCode,TVP.VendorName, tv.CodeRef as SearchTerm 
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
                            and (ISNULL(tt.DelFlag,0)=0) and (tt.EffectiveFrom is null or @EffectiveDate between EffectiveFrom and DueDate)";
                    if (IsExternal == true)
                    {
                        sql += @" and ( TVP.VendorCode not in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    else
                    {
                        sql += @" and ( TVP.VendorCode in (select VendorCode from TSBMPRICINGPOLICY))  ";
                    }
                    sql += @" and ( tt.VendorCode = @VendorCode )  ";
                    sql += @" order by TVP.VendorCode asc ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant", Session["EPlant"].ToString());
                    cmd.Parameters.AddWithValue("@processgrp", processgrp);
                    DateTime DtEffectiveDate = DateTime.ParseExact(EffectiveDate, "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@EffectiveDate", DtEffectiveDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Material", Material);
                    cmd.Parameters.AddWithValue("@VendorCode", VendorCode);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtResult);
                }

                ResultVendorToolAmortize MyResult = new ResultVendorToolAmortize();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    VendorCode = row["VendorCode"],
                    VendorName = row["VendorName"],
                    SearchTerm = row["SearchTerm"],
                    ToolTypeID = row["ToolTypeID"],
                    Amortize_Tool_ID = row["Amortize_Tool_ID"],
                    Amortize_Tool_Desc = row["Amortize_Tool_Desc"],
                    AmortizeCost = row["AmortizeCost"],
                    AmortizeCurrency = row["AmortizeCurrency"],
                    ExchangeRate = row["ExchangeRate"],
                    AmortizePeriod = row["AmortizePeriod"],
                    AmortizePeriodUOM = row["AmortizePeriodUOM"],
                    TotalAmortizeQty = row["TotalAmortizeQty"],
                    QtyUOM = row["QtyUOM"],
                    AmortizeCost_Vend_Curr = row["AmortizeCost_Vend_Curr"],
                    AmortizeCost_Pc_Vend_Curr = row["AmortizeCost_Pc_Vend_Curr"],
                    EffectiveFrom = row["EffectiveFrom"],
                    DueDate = row["DueDate"]
                });

                MyResult.VendorToolAmortize = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVendorToolAmorOld(string QuoteNo)
        {
            SqlConnection EMETCon = new SqlConnection(EMETModule.GenEMETConnString());
            DataTable DtResult = new DataTable();
            try
            {
                EMETCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    string sql = @" select distinct TT.VendorCode,TQ.VendorName, SUBSTRING(TQ.QuoteNo,1,3) as SearchTerm 
                    ,tt.ToolTypeID,tt.Amortize_Tool_ID,tt.Amortize_Tool_Desc
                    ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost),0) as AmortizeCost
                    , tt.AmortizeCurrency,tt.ExchangeRate
                    ,tt.AmortizePeriod,tt.AmortizePeriodUOM,tt.TotalAmortizeQty,tt.QtyUOM
                    ,isnull(CONVERT(DECIMAL(10,2),tt.AmortizeCost_Vend_Curr),0) as AmortizeCost_Vend_Curr
                    ,IIF( right(tt.AmortizeCost_Pc_Vend_Curr,6) = '000000'
                    , convert(nvarchar(50),convert(int,tt.AmortizeCost_Pc_Vend_Curr)) 
                    ,convert(nvarchar(50),tt.AmortizeCost_Pc_Vend_Curr) ) as AmortizeCost_Pc_Vend_Curr
                    ,tt.EffectiveFrom, tt.DueDate
                    , FORMAT(tt.EffectiveFrom, 'yyyy-MM-dd') as EeffDt
                    , FORMAT(tt.DueDate, 'yyyy-MM-dd') as DuDate
                    from TToolAmortization TT 
                    join TQuoteDetails TQ on TT.QuoteNo = TQ.QuoteNo
                    Where TT.QuoteNo = @QuoteNo ";
                    cmd = new SqlCommand(sql, EMETCon);
                    cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    sda.Fill(DtResult);
                }

                ResultVendorToolAmortize MyResult = new ResultVendorToolAmortize();
                MyResult.success = true;
                MyResult.message = "Ok";
                var convertdata = DtResult.AsEnumerable().Select(row => new
                {
                    VendorCode = row["VendorCode"],
                    VendorName = row["VendorName"],
                    SearchTerm = row["SearchTerm"],
                    ToolTypeID = row["ToolTypeID"],
                    Amortize_Tool_ID = row["Amortize_Tool_ID"],
                    Amortize_Tool_Desc = row["Amortize_Tool_Desc"],
                    AmortizeCost = row["AmortizeCost"],
                    AmortizeCurrency = row["AmortizeCurrency"],
                    ExchangeRate = row["ExchangeRate"],
                    AmortizePeriod = row["AmortizePeriod"],
                    AmortizePeriodUOM = row["AmortizePeriodUOM"],
                    TotalAmortizeQty = row["TotalAmortizeQty"],
                    QtyUOM = row["QtyUOM"],
                    AmortizeCost_Vend_Curr = row["AmortizeCost_Vend_Curr"],
                    AmortizeCost_Pc_Vend_Curr = row["AmortizeCost_Pc_Vend_Curr"],
                    EffectiveFrom = row["EffectiveFrom"],
                    DueDate = row["DueDate"]
                });

                MyResult.VendorToolAmortize = convertdata;
                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                js.Serialize(MyResult);
                Context.Response.Write(js.Serialize(MyResult));
            }
            catch (Exception ex)
            {
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EMETCon.Dispose();
            }
        }
    }
}
