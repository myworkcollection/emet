using Material_Evaluation.EmetServices.Model;
using Material_Evaluation.EmetServices.Model.MassRevision;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace Material_Evaluation.EmetServices.MassRevision
{
    /// <summary>
    /// Summary description for MyXMLMassRevisionALL
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyXMLMassRevisionALL : System.Web.Services.WebService
    {
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

        public string SrvUserid;
        public string Srvpassword;
        public string Srvdomain;
        public string Srvpath;
        public string SrvURL;

        public string UseridMail;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        public static string password;
        public static string domain;
        public static string path;
        //email

        string DbMasterName = "";
        string DbTransName = "";

        bool sendingmail;
        string errmsg;

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

        protected void GetDbTrans()
        {
            try
            {
                DbTransName = EMETModule.GetDbTransname() + ".[dbo].";
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                DbTransName = "";
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
                            HttpContext.Current.Session["path"] = dt.Rows[0]["path"].ToString();
                            SrvURL = dt.Rows[0]["Url"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;
            }
            finally
            {
                MailCon.Dispose();
            }
        }

        protected void MainDataForMail()
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
                        UseridMail = dr.GetString(0);
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
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
        }

        private DataTable DtRawmatBefEffDate(DateTime EffDate, IEnumerable<object> MainAndCompData) {
            DataTable Dtresult = new DataTable();
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlTransaction MDMTrans = null;
            string sql = "";
            try
            {
                MDMCon.Open();
                MDMTrans = MDMCon.BeginTransaction();
                #region Create Temp Table
                sql += @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                sql = @"create table ##MassRevtemp1
                            (
                                NewRequestNumber nvarchar (500),
                                CodeRef nvarchar (500),
                                Plant nvarchar (500),
	                            PIRNo nvarchar (500),
	                            MaterialCode nvarchar (500),
                                MaterialDesc nvarchar (500),
	                            VendorCode nvarchar (500),
                                VendorName nvarchar (500),
	                            ProcessGroup nvarchar (500)
                            ) "; ;
                cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                cmd.ExecuteNonQuery();
                #endregion

                DataTable MyData = LINQResultToDataTable(MainAndCompData);
                #region Generate manual query to insert data to temp table
                sql = @"";
                for (int i = 0; i < MyData.Rows.Count; i++)
                {
                    sql += @" insert into ##MassRevtemp1(NewRequestNumber,CodeRef,Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode
                        ,VendorName,ProcessGroup)
                        values(
                        '" + MyData.Rows[i]["NewRequestNumber"].ToString() + @"',
                        '" + MyData.Rows[i]["CodeRef"].ToString() + @"',
                        '" + MyData.Rows[i]["Plant"].ToString() + @"',
                        '" + MyData.Rows[i]["PIRNo"].ToString() + @"',
                        '" + MyData.Rows[i]["MaterialCode"].ToString() + @"',
                        '" + MyData.Rows[i]["MaterialDesc"].ToString() + @"',
                        '" + MyData.Rows[i]["VendorCode"].ToString() + @"',
                        '" + MyData.Rows[i]["VendorName"].ToString() + @"',
                        '" + MyData.Rows[i]["ProcessGroup"].ToString() + @"') ";
                }
                #endregion

                #region bulk copy
                if (MyData.Rows.Count > 0)
                {
                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(MDMCon, SqlBulkCopyOptions.Default, MDMTrans))
                    {
                        //Give your Destination table name 
                        sqlBulk.DestinationTableName = "##MassRevtemp1";
                        sqlBulk.BulkCopyTimeout = 0;
                        sqlBulk.BatchSize = 10000;
                        sqlBulk.WriteToServer(MyData);
                    }
                }
                sql = "";
                #endregion

                #region Get data relation in all table related
                sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into ##complist1 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            [header valid to date] < @ValidTo --and [header valid from date] >= @ValidTo
                                            and [comp. valid to date] < @ValidTo --and [comp. valid from date] >= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'
                                            and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into ##e50 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            [header valid to date] < @ValidTo --and [header valid from date] >= @ValidTo 
                                            and [comp. valid to date] < @ValidTo --and [comp. valid from date] >= @ValidTo 
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'
                                             and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            IF ((select count (*) from ##e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into ##complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join ##e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] < @ValidTo --and [header valid from date] >= @ValidTo
	                                            and [comp. valid to date] < @ValidTo --and [comp. valid from date] >= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from ##e50
	                                            delete from ##e50
	
	                                            insert into ##e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] < @ValidTo --and [header valid from date] >= @ValidTo 
	                                            and [comp. valid to date] < @ValidTo --and [comp. valid from date] >= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from ##e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";

                //sql += " insert into ##tempFinalValidData (Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup,ProcDesc) ";
                sql += @" SELECT distinct MT.NewRequestNumber,concat(MT.CodeRef,MT.NewRequestNumber) as QuoteNo, PQ.Plant,PQ.INFORECORD as PIRNo,
                                                PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                                PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                    
                                                 (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                                
                                                TCM.Material as 'CompMaterial'
                                                ,TCM.Materialdescription as 'CompMaterialDesc'
                                                ,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating

                                                ,isnull(ROUND(CAST((tcm.amount) As decimal(20,2)),2) ,'1')as AmtSCur
                                                ,tcm.UnitofCurrency as SellingCrcy
                                                ,isnull(ROUND(CAST((tcm.amount*isnull((CASE WHEN (tcm.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as AmtVCur
                                                ,tv.Crcy AS VendorCrcy ,TCM.Unit
                                                ,(select top 1 TM2.BaseUOM from tmaterial TM2 where TM2.plant=PQ.Plant and TM2.material = TCM.Material ) as UOM
                                                ,FORMAT(tcm.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom]
                                                ,format(tcm.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                                                ,isnull(ROUND(CAST((isnull((CASE WHEN (tcm.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate
                                                ,TR.ValidFrom as ExchRateValidFrom

                                                ,TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product
                                                ,TPRC.ProdComDesc as MaterialClass,concat(TPT.PIRType,' - ',TPT.Description) as PIRType
                                        
                                                ,CAST(ROUND(PN.amt1/nullif(PN.per1,0),5) AS DECIMAL(12,5)) as amt1
                                                ,CAST(ROUND(PN.amt2/nullif(PN.per2,0),5) AS DECIMAL(12,5)) as amt2
                                                ,CAST(ROUND(PN.amt3/nullif(PN.per3,0),5) AS DECIMAL(12,5)) as amt3
                                                ,CAST(ROUND(PN.amt4/nullif(PN.per4,0),5) AS DECIMAL(12,5)) as amt4

                                                ,PN.countryorg,PN.UpdatedOn as MassUpdateDate

                                                into ##tempFinalValidData

                                                FROM tPir_New PN
                                                INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                                INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                                INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                                INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                                Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                                INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                                inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                                and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                                INNER JOIN ##complist1 TB ON TM.Material = TB.[Parent Material]
                                                INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                                
                                                left outer join TEXCHANGE_RATE tr on TCM.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo 
                                                and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TCM.UnitofCurrency  AND ValidFrom <= @ValidTo )

                                                INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                                inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                                inner join ##MassRevtemp1 MT on MT.Plant = tm.Plant and MT.PIRNo = PQ.InfoRecord and MT.MaterialCode = PN.Material and
                                                MT.ProcessGroup = PJPG.ProcessGrpcode and MT.VendorCode = TV.Vendor
                                                WHERE TCM.ValidTo < @ValidTo";

                sql += @" ;with cte as
                        (
	                        select *, ROW_NUMBER() over (partition by NewRequestNumber,QuoteNo,Plant,CompMaterial order by [CusMatValTo] desc) as RN
	                         from ##tempFinalValidData
                        )
                        select NewRequestNumber,QuoteNo,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy,Unit
                            ,UOM,CusMatValFrom,CusMatValTo,ExchRate,
                            ExchRateValidFrom from cte where RN = 1 ";
                
                using (SqlDataAdapter sda = new SqlDataAdapter()) {
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.Parameters.AddWithValue("@ValidTo", EffDate.ToString("yyyy-MM-dd"));
                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    sda.Fill(Dtresult);
                }
                #endregion

                #region drop temp table
                sql = @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                #endregion

                MDMTrans.Commit();
            }
            catch (Exception ex)
            {
                if (MDMTrans != null) {
                    MDMTrans.Rollback();
                }
                EMETModule.SendExcepToDB(ex);
            }
            finally {
                MDMCon.Dispose();
            }
            return Dtresult;
        }

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                               == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                           (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CekVendorVsMaterialValid(List<MassrevVendorVsMaterial> MassrevVendorVsMaterial)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlTransaction MDMTrans = null;
            GetDbTrans();
            string sql = "";
            try
            {
                GetDbTrans();
                MassRevResultValidateData MyResult = new MassRevResultValidateData();
                var DistinctData = MassrevVendorVsMaterial.Select(p => new {
                    p.Plant,
                    p.PIRNo,
                    p.MaterialCode,
                    p.MaterialDesc,
                    p.VendorCode,
                    p.VendorName,
                    p.ProcessGroup
                });

                DataTable MyData = LINQResultToDataTable(DistinctData);
                DataTable ValidData = new DataTable();
                DataTable InvalidData = new DataTable();
                if (MassrevVendorVsMaterial.Count > 0)
                {
                    MDMCon.Open();
                    MDMTrans = MDMCon.BeginTransaction();

                    #region Create Temp Table
                    sql += @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                    sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                    sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1Invalid') IS NOT NULL DROP TABLE ##MassRevtemp1Invalid";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    sql = @"create table ##MassRevtemp1
                            (
                                Plant nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar (500)
                            ) ";
                    sql += @"create table ##MassRevtemp1Invalid
                            (
                                Plant nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar (500) COLLATE DATABASE_DEFAULT,
                                Remark nvarchar (1000) COLLATE DATABASE_DEFAULT
                            ) ";
                    sql += @"create table ##tempFinalValidData
                            (
                                Plant nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar (500) COLLATE DATABASE_DEFAULT,
                                ProcDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
                            ) ";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.ExecuteNonQuery();
                    #endregion 

                    #region Generate manual query to insert data to temp table
                    sql = @"";
                    for (int i = 0; i < MyData.Rows.Count; i++)
                    {
                        sql += @" insert into ##MassRevtemp1(Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode
                            ,VendorName,ProcessGroup)
                            values(
                            '" + MyData.Rows[i]["Plant"].ToString() + @"',
                            '" + MyData.Rows[i]["PIRNo"].ToString() + @"',
                            '" + MyData.Rows[i]["MaterialCode"].ToString() + @"',
                            '" + MyData.Rows[i]["MaterialDesc"].ToString() + @"',
                            '" + MyData.Rows[i]["VendorCode"].ToString() + @"','" + MyData.Rows[i]["VendorName"].ToString() + @"',
                            '" + MyData.Rows[i]["ProcessGroup"].ToString() + @"') ";
                    }
                    #endregion

                    #region bulk copy
                    if (MyData.Rows.Count > 0)
                    {
                        using (SqlBulkCopy sqlBulk = new SqlBulkCopy(MDMCon, SqlBulkCopyOptions.Default, MDMTrans))
                        {
                            //Give your Destination table name 
                            sqlBulk.DestinationTableName = "##MassRevtemp1";
                            sqlBulk.BulkCopyTimeout = 0;
                            sqlBulk.BatchSize = 10000;
                            sqlBulk.WriteToServer(MyData);
                        }
                    }
                    sql = "";
                    #endregion

                    #region get Duplicate record
                    sql = @" begin
                            WITH cte AS (
                                SELECT Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup 
                                    ,ROW_NUMBER() OVER (
                                        PARTITION BY Plant,PIRNo,MaterialCode,VendorCode,ProcessGroup
                                        ORDER BY Plant,PIRNo,MaterialCode,VendorCode,ProcessGroup
                                    ) row_num
                                 FROM ##MassRevtemp1
                            )
							insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                            select Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup,'Duplicate Record' FROM cte
                            WHERE row_num > 1
							end ";

                    sql += @"  begin
							WITH cte AS (
                                SELECT Plant,PIRNo,MaterialCode,VendorCode,ProcessGroup, 
                                    ROW_NUMBER() OVER (
                                        PARTITION BY Plant,PIRNo,MaterialCode,VendorCode,ProcessGroup
                                        ORDER BY Plant,PIRNo,MaterialCode,VendorCode,ProcessGroup
                                    ) row_num
                                 FROM ##MassRevtemp1
                            )
                            delete FROM cte
                            WHERE row_num > 1;
							end ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek is PIR Already Approved
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'PIR No Already Exist In EMET and Approved' 
                                from ##MassRevtemp1 where PIRNo in (select distinct PIRNo from "+ DbTransName + "TQuoteDetails where ApprovalStatus='3' and picapprovalstatus='2' and ManagerApprovalStatus='2' and dirapprovalstatus='0') ";

                    sql += @" delete from ##MassRevtemp1 where PIRNo in (select distinct PIRNo from ##MassRevtemp1Invalid) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in Plant Table
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Plant not exist in Master Data Plant' 
                                from ##MassRevtemp1 where plant not in (select distinct plant from tplant) ";

                    sql += @" delete from ##MassRevtemp1 where plant not in (select distinct plant from tplant) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in PIR new Table
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Material and Vendor not exist in PIR' 
                                from ##MassRevtemp1 where concat(MaterialCode,'-',VendorCode) not in (select distinct concat(Material,'-',Vendor) from tPir_New where DelFlag = 0) ";

                    sql += @" delete from ##MassRevtemp1 where concat(MaterialCode,'-',VendorCode) 
                              not in 
                              (select distinct concat(Material,'-',Vendor) from tPir_New where DelFlag = 0) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in PIR vs Quotation Table
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'This PIR No not exist in PIR Vs Quotation' 
                                from ##MassRevtemp1 where concat(Plant,'-',MaterialCode,'-',VendorCode)  
                                not in (select distinct concat(Plant,'-',Material,'-',Vendor) from tPIRvsQuotation where DelFlag = 0) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',MaterialCode,'-',VendorCode) 
                              not in 
                              (select distinct concat(Plant,'-',Material,'-',Vendor) from tPIRvsQuotation where DelFlag = 0) ";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in BOM table
                    //sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                   //         VendorCode,VendorName,ProcessGroup,Remark)
                    //            select *,
                    //            'Material not exist in BOM' 
                    //            from ##MassRevtemp1 where concat(Plant,'-',MaterialCode)  not in (select concat(Plant,'-',[Parent Material]) from TBOMLISTnew) ";

                    //sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',MaterialCode) not in (select concat(Plant,'-',[Parent Material]) from TBOMLISTnew) ";

                    //cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    //cmd.CommandTimeout = 0;
                    //cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in Process Group table
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Process group not exist in Process Group Data' 
                                from ##MassRevtemp1 where concat(Plant,'-',ProcessGroup)  not in (select concat(Plant,'-',Process_Grp_code) from TPROCESGROUP_LIST where DelFlag = 0 ) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',ProcessGroup) not in (select concat(Plant,'-',Process_Grp_code) from TPROCESGROUP_LIST where DelFlag = 0 ) ";
                     
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in table material
                    //sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                   //         VendorCode,VendorName,ProcessGroup,Remark)
                    //            select *,
                    //            'Material not exist in Material Data' 
                    //            from ##MassRevtemp1 where concat(Plant,'-',MaterialCode)  not in (select concat(Plant,'-',Material) from TMATERIAL where DelFlag = 0 ) ";

                    //sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',MaterialCode) not in (select concat(Plant,'-',Material) from TMATERIAL where DelFlag = 0 ) ";

                    //cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    //cmd.CommandTimeout = 0;
                    //cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in table Vendor
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Vendor not exist in Vendor Data' 
                                from ##MassRevtemp1 where concat(Plant,'-',VendorCode)  not in (select concat(Plant,'-',Vendor) from tVendor_New where DelFlag = 0 ) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',VendorCode) not in (select concat(Plant,'-',Vendor) from tVendor_New where DelFlag = 0 ) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Cek in table Vendor PORG
                    sql = @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Vendor not exist in Vendor PORG Data' 
                                from ##MassRevtemp1 where concat(Plant,'-',VendorCode)  not in (select concat(Plant,'-',Vendor) from tVendorPOrg where DelFlag = 0 ) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',VendorCode) not in (select concat(Plant,'-',Vendor) from tVendorPOrg where DelFlag = 0 ) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Get data relation in all table related
                    sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into ##complist1 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            [header valid to date] > getdate() and [header valid from date] <= getdate()
                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'
                                            and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into ##e50 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            [header valid to date] > getdate() and [header valid from date] <= getdate() 
                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()  
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'
                                             and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            IF ((select count (*) from ##e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into ##complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join ##e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > getdate() and [header valid from date] <= getdate()
	                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from ##e50
	                                            delete from ##e50
	
	                                            insert into ##e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > getdate() and [header valid from date] <= getdate()  
	                                            and [comp. valid to date] > getdate() and [comp. valid from date] <= getdate()  
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from ##e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";

                    sql += " insert into ##tempFinalValidData (Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup,ProcDesc) ";
                    sql += @" SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                                PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                                PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc
                                                FROM tPir_New PN
                                                INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                                INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                                INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                                INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                                Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                                INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                                inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                                and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                                INNER JOIN ##complist1 TB ON TM.Material = TB.[Parent Material]
                                                INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                                left outer join TEXCHANGE_RATE tr on TCM.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo 
                                                and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TCM.UnitofCurrency  AND ValidFrom <= getdate() )
                                                INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                                inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                                WHERE concat(tm.Plant,'-',PQ.InfoRecord,'-',PN.Material,'-',PJPG.ProcessGrpcode,'-',TV.Vendor) 
                                                in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##MassRevtemp1)
                                                ";

                    sql += @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup,Remark)
                                select *,
                                'Based On Material and Vendor Code This PIR Not Exist in PIR Data, Please check data in PIR New, PIR vs Quotation , Material , Process group List , BOM data and Customer Material Pricing' 
                                from ##MassRevtemp1 where concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode)  not in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##tempFinalValidData ) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) not in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##tempFinalValidData ) ";
                    
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion
                    
                    #region Store Valid Data To data table
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select * from ##tempFinalValidData";
                        cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(ValidData);
                        }
                    }
                    #endregion

                    #region Store InValid Data To data table
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select * from ##MassRevtemp1Invalid";
                        cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(InvalidData);
                        }
                    }
                    #endregion

                    #region drop temp table 
                    sql = @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                    sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                    sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1Invalid') IS NOT NULL DROP TABLE ##MassRevtemp1Invalid";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    MDMTrans.Commit();
                    #endregion

                    var MyValidData = ValidData.AsEnumerable().Select(row => new
                    {
                        Plant = row["Plant"],
                        PIRNo = row["PIRNo"],
                        MaterialCode = row["MaterialCode"],
                        MaterialDesc = row["MaterialDesc"],
                        VendorCode = row["VendorCode"],
                        VendorName = row["VendorName"],
                        ProcessGroup = row["ProcessGroup"],
                    });

                    var MyInvalidData = InvalidData.AsEnumerable().Select(row => new
                    {
                        Plant = row["Plant"],
                        PIRNo = row["PIRNo"],
                        MaterialCode = row["MaterialCode"],
                        MaterialDesc = row["MaterialDesc"],
                        VendorCode = row["VendorCode"],
                        VendorName = row["VendorName"],
                        ProcessGroup = row["ProcessGroup"],
                        Remark = row["Remark"],
                    });

                    MyResult.success = true;
                    MyResult.message = "";
                    MyResult.ValidData = MyValidData;
                    MyResult.InValidData = MyInvalidData;
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data In List Need To Check";
                }
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                if (MDMTrans != null) {
                    MDMTrans.Dispose();
                }
                MDMCon.Dispose();

                EMETModule.SendExcepToDB(ex);
                GlobalResult MyResult = new GlobalResult();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            finally
            {
                MDMTrans.Dispose();
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateReqTemp(bool IsMatCost, bool IsProcCost, bool IsSubMatCost, bool IsOthCost,string EffDate,string DueDatenextRev,string RespDueDate,
            string RePurposeReason, string RePurposeRemark,string UserId, string SMNPicDept, List<MassrevVendorVsMaterial> MassrevVendorVsMaterial)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            SqlTransaction MDMTrans = null;
            string sql = "";
            GetDbTrans();
            try
            {
                MassRevResultReqTemp MyResult = new MassRevResultReqTemp();
                var DistinctData = MassrevVendorVsMaterial.Select(p => new {
                    p.Plant,
                    p.PIRNo,
                    p.MaterialCode,
                    p.MaterialDesc,
                    p.VendorCode,
                    p.VendorName,
                    p.ProcessGroup
                }).Distinct();
                DataTable MyData = LINQResultToDataTable(DistinctData);
                DataTable ValidData = new DataTable();
                DataTable ValidDataComponent = new DataTable();
                DataTable InValidData = new DataTable();
                if (MassrevVendorVsMaterial.Count > 0)
                {
                    MDMCon.Open();
                    MDMTrans = MDMCon.BeginTransaction();

                    #region Create Temp Table
                    sql += @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                    sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                    sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1Invalid') IS NOT NULL DROP TABLE ##MassRevtemp1Invalid";
                    sql += @" IF OBJECT_ID('tempdb..##tempDataToInsert') IS NOT NULL DROP TABLE ##tempDataToInsert";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    sql = @"create table ##MassRevtemp1
                            (
                                Plant nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar (500) COLLATE DATABASE_DEFAULT
                            ) ";
                    sql += @"create table ##MassRevtemp1Invalid
                            (
                                Plant nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar (500) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar (500) COLLATE DATABASE_DEFAULT
                            ) ";
                    sql += @"create table ##tempDataToInsert
                            (
                                NewRequestNumber nvarchar(100) COLLATE DATABASE_DEFAULT,
                                Plant nvarchar (40) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar(50) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar(50) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar(10) COLLATE DATABASE_DEFAULT,
                                ProcDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
                                PIRJobType nvarchar(100) COLLATE DATABASE_DEFAULT,
                                CodeRef nvarchar (10) COLLATE DATABASE_DEFAULT,
                                UnitWeight nvarchar (100) COLLATE DATABASE_DEFAULT,
                                UnitWeightUOM nvarchar (10) COLLATE DATABASE_DEFAULT,
                                Plating nvarchar(100) COLLATE DATABASE_DEFAULT,
                                MaterialType nvarchar (100) COLLATE DATABASE_DEFAULT,
                                PlantStatus nvarchar (10) COLLATE DATABASE_DEFAULT,
                                SAPProcType nvarchar(2) COLLATE DATABASE_DEFAULT,
                                SAPSPProcType nvarchar(10) COLLATE DATABASE_DEFAULT,
                                product nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MaterialClass nvarchar(100) COLLATE DATABASE_DEFAULT,
                                PIRType nvarchar(100) COLLATE DATABASE_DEFAULT,
                                amt1 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt2 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt3 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt4 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MassUpdateDate datetime ,
                                countryorg nvarchar(50) COLLATE DATABASE_DEFAULT,
                                CompMaterial nvarchar(50) COLLATE DATABASE_DEFAULT,
                                CompMaterialDesc nvarchar(50) COLLATE DATABASE_DEFAULT,
                                AmtSCur numeric(10, 2),
                                SellingCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                AmtVCur numeric(10, 2),
                                VendorCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                Unit numeric(4, 0),
                                UOM nvarchar(4) COLLATE DATABASE_DEFAULT,
                                CusMatValFrom datetime,
                                CusMatValTo datetime,
                                ExchRate numeric(10, 5),
                                ExchRateValidFrom datetime
                            ) ";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.ExecuteNonQuery();
                    #endregion 

                    #region Generate manual query to insert data to temp table
                    //sql = @"";
                    //for (int i = 0; i < MyData.Rows.Count; i++)
                    //{
                    //    sql += @" insert into ##MassRevtemp1(Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode
                    //        ,VendorName,ProcessGroup)
                    //        values(
                    //        '" + MyData.Rows[i]["Plant"].ToString() + @"',
                    //        '" + MyData.Rows[i]["PIRNo"].ToString() + @"',
                    //        '" + MyData.Rows[i]["MaterialCode"].ToString() + @"',
                    //        '" + MyData.Rows[i]["MaterialDesc"].ToString() + @"',
                    //        '" + MyData.Rows[i]["VendorCode"].ToString() + @"','" + MyData.Rows[i]["VendorName"].ToString() + @"',
                    //        '" + MyData.Rows[i]["ProcessGroup"].ToString() + @"') ";
                    //}
                    #endregion

                    #region bulk copy
                    if (MyData.Rows.Count > 0)
                    {
                        using (SqlBulkCopy sqlBulk = new SqlBulkCopy(MDMCon, SqlBulkCopyOptions.Default, MDMTrans))
                        {
                            //Give your Destination table name 
                            sqlBulk.DestinationTableName = "##MassRevtemp1";
                            sqlBulk.BulkCopyTimeout = 0;
                            sqlBulk.BatchSize = 10000;
                            sqlBulk.WriteToServer(MyData);
                        }
                    }
                    sql = "";
                    #endregion
                    
                    #region Get data relation in all table related
                    sql = @"  
                                            --declare @plant nvarchar(4) ='2100'
                                            --declare @mat nvarchar(20)='40000721'

                                            declare @e50check bit = 1
                                            declare @e50mat nvarchar(20)
                                            set @e50mat=''

                                            --get list of components which is valid (plant status not in z4/49, not expired date, not co product
                                            SELECT Plant, [Parent Material],[Component Material], [Base Qty],
                                            [Base Unit of Measure],[Quantity],[Component Unit], [Component special procurement type], [Alternative Bom], cast(0 as decimal(12,0)) as 'ReqQty'
                                            into ##complist1 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom 
                                            [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')				
                                            and [co-product] <> 'X'
                                            and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            --get the list of e50 components
                                            SELECT Plant,[Component Material] as 'Material'
                                            into ##e50 from tbomlistnew 
                                            where 
                                            --no need altbom because not using PV for direct transfer
                                            --and [alternative bom]=@altbom
                                            [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo 
                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo 
                                            and UPPER([component plant status]) not in ('Z4','Z9')
                                            and UPPER([parent plant status]) not in ('Z4','Z9')
                                            and [Component special procurement type] = '50'
                                            and [co-product] <> 'X'
                                             and concat(Plant,'-',[Parent Material]) in (select concat(Plant,'-',MaterialCode) from ##MassRevtemp1 )

                                            IF ((select count (*) from ##e50)=0)
                                            BEGIN
	                                            set @e50check=0
                                            END
                                            ELSE
                                            BEGIN
	                                            set @e50check=1
                                            END

                                            WHILE (@e50check=1)
                                            BEGIN
	                                            insert into ##complist1 (Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type], [Alternative Bom])
	                                            (SELECT t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type],MIN([Alternative Bom])
	                                            from tbomlistnew t1 inner join ##e50 t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [co-product] <> 'X'
	                                            group by
	                                            t1.Plant, [Parent Material],[Component Material], [Base Qty],
	                                            [Base Unit of Measure],[Quantity],[Component Unit],[Component special procurement type])
	
	                                            select * into #temp from ##e50
	                                            delete from ##e50
	
	                                            insert into ##e50 (Plant, Material) 
	                                            (SELECT t1.Plant, [Component Material]
	                                            from tbomlistnew t1 inner join #temp t2 on t1.plant=t2.plant and t1.[Parent Material]=t2.Material 
	                                            where [header valid to date] > @ValidTo and [header valid from date] <= @ValidTo 
	                                            and [comp. valid to date] > @ValidTo and [comp. valid from date] <= @ValidTo
	                                            and UPPER([component plant status]) not in ('Z4','Z9')
	                                            and UPPER([parent plant status]) not in ('Z4','Z9')
	                                            and [Component special procurement type] = '50'
	                                            and [co-product] <> 'X')
	
	                                            drop table #temp
	
	                                            if ((select count (*) from ##e50)=0)
	                                            BEGIN
		                                            set @e50check=0
	                                            END			
                                            END
                                            ";

                    //sql += " insert into ##tempFinalValidData (Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup,ProcDesc) ";
                    sql += @" SELECT distinct PQ.Plant,PQ.INFORECORD as PIRNo,
                                                PN.Material as MaterialCode,TM.MaterialDesc as MaterialDesc,TV.Vendor as VendorCode,TV.Description as VendorName,
                                                PJPG.ProcessGrpcode as ProcessGroup,TPL.Process_Grp_Description as ProcDesc,
                                    
                                                 (PN.jobtyp+'- '+PJPG.JobCodeDesc) as PIRJobType,TVP.CodeRef,
                                                
                                                TCM.Material as 'CompMaterial'
                                                ,TCM.Materialdescription as 'CompMaterialDesc'
                                                ,TM.UnitWeight,TM.UnitWeightUOM,TM.Plating

                                                ,isnull(ROUND(CAST((tcm.amount) As decimal(20,2)),2) ,'1')as AmtSCur
                                                ,tcm.UnitofCurrency as SellingCrcy
                                                ,isnull(ROUND(CAST((tcm.amount*isnull((CASE WHEN (tcm.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),'0')) As decimal(20,2)),2) ,'1') as AmtVCur
                                                ,tv.Crcy AS VendorCrcy ,TCM.Unit
                                                ,(select top 1 TM2.BaseUOM from tmaterial TM2 where TM2.plant=PQ.Plant and TM2.material = TCM.Material ) as UOM
                                                ,FORMAT(tcm.ValidFrom, 'yyyy-MM-dd') as [CusMatValFrom]
                                                ,format(tcm.ValidTo, 'yyyy-MM-dd') as [CusMatValTo]
                                                ,isnull(ROUND(CAST((isnull((CASE WHEN (tcm.UnitofCurrency != tv.Crcy ) THEN tr.ExchRate ELSE '1' END),  '0')) As decimal(20,4)),2) ,'1') as ExchRate
                                                ,TR.ValidFrom as ExchRateValidFrom

                                                ,TM.MaterialType,TM.PlantStatus, TM.PROCTYPE as SAPProcType,TM.SplPROCTYPE as SAPSPProcType, PR.product
                                                ,TPRC.ProdComDesc as MaterialClass,concat(TPT.PIRType,' - ',TPT.Description) as PIRType
                                        
                                                ,CAST(ROUND(PN.amt1/nullif(PN.per1,0),5) AS DECIMAL(12,5)) as amt1
                                                ,CAST(ROUND(PN.amt2/nullif(PN.per2,0),5) AS DECIMAL(12,5)) as amt2
                                                ,CAST(ROUND(PN.amt3/nullif(PN.per3,0),5) AS DECIMAL(12,5)) as amt3
                                                ,CAST(ROUND(PN.amt4/nullif(PN.per4,0),5) AS DECIMAL(12,5)) as amt4

                                                ,PN.countryorg,PN.UpdatedOn as MassUpdateDate

                                                into ##tempFinalValidData

                                                FROM tPir_New PN
                                                INNER JOIN tPIRvsQuotation PQ ON PQ.Material=PN.Material AND PQ.Vendor=PN.Vendor
                                                INNER JOIN TPIRJOBTYPE_PROCESSGROUP PJPG ON PJPG.JOBCODE=PN.JOBTYP
                                                INNER JOIN tVendor_New TV ON PN.Vendor = TV.Vendor 
                                                INNER JOIN TMATERIAL TM ON PN.Material = TM.Material
                                                Inner join TPRODCOM TPRC on TM.ProdComCode = TPRC.ProdComCode
                                                INNER JOIN TPRODUCT PR ON TM.Product = PR.product 
                                                inner join TPROCPIRTYPE TPT ON TM.PROCTYPE = TPT.procType 
                                                and (case when TM.SplPROCTYPE is null then 0 else TM.SplPROCTYPE end) = TPT.SPProcType and tpt.PIRType=PN.inforcatstd
                                                INNER JOIN ##complist1 TB ON TM.Material = TB.[Parent Material]
                                                INNER JOIN TCUSTOMER_MATLPRICING TCM ON TB.[Component Material] = TCM.Material and TV.CustomerNo = TCM.Customer
                                                
                                                left outer join TEXCHANGE_RATE tr on TCM.UnitofCurrency=tr.EFrm and tv.Crcy=tr.ETo 
                                                and tr.ValidFrom in(select MAX(ValidFrom) from TEXCHANGE_RATE WHERE EFRM=TCM.UnitofCurrency  AND ValidFrom <= @ValidTo )

                                                INNER JOIN TPROCESGROUP_LIST TPL ON PJPG.ProcessGrpcode = TPL.Process_Grp_code
                                                inner join tVendorPOrg TVP ON TV.Vendor = TVP.Vendor and TVP.Plant=PQ.Plant
                                                WHERE concat(tm.Plant,'-',PQ.InfoRecord,'-',PN.Material,'-',PJPG.ProcessGrpcode,'-',TV.Vendor) 
                                                in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##MassRevtemp1)
                                                and TCM.ValidFrom <= @ValidTo and TCM.ValidTo >= @ValidTo";

                    sql += @" insert into ##MassRevtemp1Invalid (Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup)
                                select Plant,PIRNo,MaterialCode,MaterialDesc,
	                            VendorCode,VendorName,ProcessGroup from ##MassRevtemp1 where concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode)  not in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##tempFinalValidData ) ";

                    sql += @" delete from ##MassRevtemp1 where concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) not in (select concat(Plant,'-',PIRNo,'-',MaterialCode,'-',ProcessGroup,'-',VendorCode) from ##tempFinalValidData ) ";

                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    DateTime ValidTo = DateTime.ParseExact(DueDatenextRev.Replace("/","-"), "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidTo", ValidTo.ToString("yyyy-MM-dd"));
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region Store Valid Data To data table
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" declare @LastReqNo nvarchar(100) = (select MAX(RequestNumber) from "+ DbTransName + @"TQuoteDetails)
                        declare @RunningNo int = 0
                        declare @Curryear nvarchar(2) = (select FORMAT(CURRENT_TIMESTAMP,'yy'))
                        if @LastReqNo is not null
                        begin
	                        set @RunningNo = SUBSTRING(@LastReqNo,3,len(@LastReqNo))
                        end ";
                        
                        sql += @" 
                        insert into ##tempDataToInsert (
                        NewRequestNumber
                        ,Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName ,ProcessGroup 
                        ,ProcDesc ,PIRJobType ,CodeRef ,UnitWeight ,UnitWeightUOM 
                        ,Plating ,MaterialType ,PlantStatus ,SAPProcType 
                        ,SAPSPProcType ,product ,MaterialClass ,PIRType 
                        ,amt1 ,amt2 ,amt3 ,amt4 ,MassUpdateDate ,countryorg
                        ,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy
                        ,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,ExchRateValidFrom )
                        select distinct @Curryear + RIGHT('00000' + CAST( (ROW_NUMBER() OVER(ORDER BY Plant) + @RunningNo) AS NVARCHAR), 5) as 'NewRequestNumber'
                        ,Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,ProcessGroup,
                        ProcDesc,PIRJobType,CodeRef,UnitWeight,UnitWeightUOM
                        ,Plating,MaterialType,PlantStatus,SAPProcType
                        ,SAPSPProcType,product,MaterialClass,PIRType,
                        amt1,amt2,amt3,amt4,MassUpdateDate,countryorg 
                        ,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy
                        ,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,ExchRateValidFrom
                        from ##tempFinalValidData";

                        sql += @" 
                        IF (select count (*) from ##tempDataToInsert) > 0
                        BEGIN
                        INSERT INTO " + DbTransName + @"TQuoteDetails(PIRNo,RequestNumber,RequestDate,EffectiveDate,Plant,QuoteNo,VendorCode1,VendorName,ProcessGroup,PIRJobType,Material,
                         MaterialDesc,CreatedBy,isUseSAPCode,SMNPicDept) 
                        select distinct PIRNo,NewRequestNumber,CURRENT_TIMESTAMP,@EffectiveDate,Plant,concat(CodeRef,NewRequestNumber),VendorCode,VendorName,ProcessGroup,PIRJobType,MaterialCode
                        ,MaterialDesc,@UserId,1,@SMNPicDept from ##tempDataToInsert
                        END ";

                        sql += @" select distinct NewRequestNumber
                        ,Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName ,ProcessGroup 
                        ,ProcDesc ,PIRJobType ,CodeRef ,UnitWeight ,UnitWeightUOM 
                        ,Plating ,MaterialType ,PlantStatus ,SAPProcType 
                        ,SAPSPProcType ,product ,MaterialClass ,PIRType 
                        ,amt1 ,amt2 ,amt3 ,amt4 ,MassUpdateDate ,countryorg from ##tempDataToInsert";
                        cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                        DateTime DtEffeDate = DateTime.ParseExact(EffDate.Replace("/", "-"), "dd-MM-yyyy", null);
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtEffeDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.Parameters.AddWithValue("@SMNPicDept", SMNPicDept);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(ValidData);
                        }
                    }
                    #endregion

                    #region Store ValidDataComponent Data To data table
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select distinct NewRequestNumber
                        ,Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName ,ProcessGroup 
                        ,ProcDesc ,PIRJobType ,CodeRef ,UnitWeight ,UnitWeightUOM 
                        ,Plating ,MaterialType ,PlantStatus ,SAPProcType 
                        ,SAPSPProcType ,product ,MaterialClass ,PIRType 
                        ,amt1 ,amt2 ,amt3 ,amt4 ,MassUpdateDate ,countryorg
                        ,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy
                        ,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,ExchRateValidFrom
                        from ##tempDataToInsert ";
                        cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(ValidDataComponent);
                        }
                    }
                    #endregion

                    #region Store InValidData Data To data table
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select * from ##MassRevtemp1Invalid ";
                        cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(InValidData);
                        }
                    }
                    #endregion

                    #region drop temp table 
                    sql = @" IF OBJECT_ID('tempdb..##complist1') IS NOT NULL DROP TABLE ##complist1";
                    sql += @" IF OBJECT_ID('tempdb..##e50') IS NOT NULL DROP TABLE ##e50";
                    sql += @" IF OBJECT_ID('tempdb..##tempFinalValidData') IS NOT NULL DROP TABLE ##tempFinalValidData";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1') IS NOT NULL DROP TABLE  ##MassRevtemp1";
                    sql += @" IF OBJECT_ID('tempdb..##MassRevtemp1Invalid') IS NOT NULL DROP TABLE ##MassRevtemp1Invalid";
                    sql += @" IF OBJECT_ID('tempdb..##tempDataToInsert') IS NOT NULL DROP TABLE ##tempDataToInsert";
                    cmd = new SqlCommand(sql, MDMCon, MDMTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    MDMTrans.Commit();
                    #endregion

                    var MyValidDataMain = ValidData.AsEnumerable().Select(row => new
                    {
                        NewRequestNumber = row["NewRequestNumber"],
                        Plant = row["Plant"],
                        PIRNo = row["PIRNo"],
                        MaterialCode = row["MaterialCode"],
                        MaterialDesc = row["MaterialDesc"],
                        VendorCode = row["VendorCode"],
                        VendorName = row["VendorName"],
                        ProcessGroup = row["ProcessGroup"],
                        ProcDesc = row["ProcDesc"],
                        PIRJobType = row["PIRJobType"],
                        CodeRef = row["CodeRef"],
                        UnitWeight = row["UnitWeight"],
                        UnitWeightUOM = row["UnitWeightUOM"],
                        Plating = row["Plating"],
                        MaterialType = row["MaterialType"],
                        PlantStatus = row["PlantStatus"],
                        SAPProcType = row["SAPProcType"],
                        SAPSPProcType = row["SAPSPProcType"],
                        product = row["product"],
                        MaterialClass = row["MaterialClass"],
                        PIRType = row["PIRType"],
                        amt1 = row["amt1"],
                        amt2 = row["amt2"],
                        amt3 = row["amt3"],
                        amt4 = row["amt4"],
                        MassUpdateDate = row["MassUpdateDate"],
                        countryorg = row["countryorg"]
                    });

                    var MyValidDataMainComponent = ValidDataComponent.AsEnumerable().Select(row => new
                    {
                        NewRequestNumber = row["NewRequestNumber"],
                        Plant = row["Plant"],
                        PIRNo = row["PIRNo"],
                        MaterialCode = row["MaterialCode"],
                        MaterialDesc = row["MaterialDesc"],
                        VendorCode = row["VendorCode"],
                        VendorName = row["VendorName"],
                        ProcessGroup = row["ProcessGroup"],
                        ProcDesc = row["ProcDesc"],
                        PIRJobType = row["PIRJobType"],
                        CodeRef = row["CodeRef"],
                        UnitWeight = row["UnitWeight"],
                        UnitWeightUOM = row["UnitWeightUOM"],
                        Plating = row["Plating"],
                        MaterialType = row["MaterialType"],
                        PlantStatus = row["PlantStatus"],
                        SAPProcType = row["SAPProcType"],
                        SAPSPProcType = row["SAPSPProcType"],
                        product = row["product"],
                        MaterialClass = row["MaterialClass"],
                        PIRType = row["PIRType"],
                        amt1 = row["amt1"],
                        amt2 = row["amt2"],
                        amt3 = row["amt3"],
                        amt4 = row["amt4"],
                        MassUpdateDate = row["MassUpdateDate"],
                        countryorg = row["countryorg"],

                        CompMaterial = row["CompMaterial"],
                        CompMaterialDesc = row["CompMaterialDesc"],
                        AmtSCur = row["AmtSCur"],
                        SellingCrcy = row["SellingCrcy"],
                        AmtVCur = row["AmtVCur"],
                        VendorCrcy = row["VendorCrcy"],
                        Unit = row["Unit"],
                        UOM = row["UOM"],
                        CusMatValFrom = row["CusMatValFrom"],
                        CusMatValTo = row["CusMatValTo"],
                        ExchRate = row["ExchRate"],
                        ExchRateValidFrom = row["ExchRateValidFrom"]
                    });

                    var MyInValidData = InValidData.AsEnumerable().Select(row => new
                    {
                        Plant = row["Plant"],
                        PIRNo = row["PIRNo"],
                        MaterialCode = row["MaterialCode"],
                        MaterialDesc = row["MaterialDesc"],
                        VendorCode = row["VendorCode"],
                        VendorName = row["VendorName"],
                        ProcessGroup = row["ProcessGroup"],
                    });

                    MyResult.success = true;
                    MyResult.message = "";
                    MyResult.ValidDataMain = MyValidDataMain;
                    MyResult.ValidDataMainComponent = MyValidDataMainComponent;
                    MyResult.InValidData = MyInValidData;
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data In List Need To Check";
                }
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                if (MDMTrans != null)
                {
                    //MDMTrans.Rollback();
                }

                EMETModule.SendExcepToDB(ex);
                GlobalResult MyResult = new GlobalResult();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            finally
            {
                MDMTrans.Dispose();
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ProceedSubmitRequest(bool isMassRevisionAll,bool IsMatCost, bool IsProcCost, bool IsSubMatCost, bool IsOthCost, string EffDate, string DueDatenextRev, string RespDueDate,
            string RePurposeReason, string RePurposeRemark, string UserId, string SMNPicDept,List<MassrevVendorVsMaterial> MainAndCompData)
        {
            SqlConnection EMETCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EMETTrans = null;
            string sql = "";
            GetDbMaster();
            MainDataForMail();
            GlobalResult MyResult = new GlobalResult();
            try
            {
                DataTable MyData = LINQResultToDataTable(MainAndCompData);
                DataTable ValidData = new DataTable();
                DataTable ValidDataComponent = new DataTable();
                DataTable InValidData = new DataTable();
                if (MainAndCompData.Count > 0)
                {
                    EMETCon.Open();
                    EMETTrans = EMETCon.BeginTransaction();

                    #region Create Temp Table
                    sql += @" IF OBJECT_ID('tempdb..##tempOldRawmaterial') IS NOT NULL DROP TABLE ##tempOldRawmaterial";
                    sql += @" IF OBJECT_ID('tempdb..##tempDataToInsert') IS NOT NULL DROP TABLE ##tempDataToInsert";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    
                    sql = @"create table ##tempDataToInsert
                            (
                                NewRequestNumber nvarchar(100) COLLATE DATABASE_DEFAULT,
                                Plant nvarchar (40) COLLATE DATABASE_DEFAULT,
	                            PIRNo nvarchar(50) COLLATE DATABASE_DEFAULT,
	                            MaterialCode nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MaterialDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            VendorCode nvarchar(50) COLLATE DATABASE_DEFAULT,
                                VendorName nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            ProcessGroup nvarchar(10) COLLATE DATABASE_DEFAULT,
                                ProcDesc nvarchar (500) COLLATE DATABASE_DEFAULT,
                                PIRJobType nvarchar(100) COLLATE DATABASE_DEFAULT,
                                CodeRef nvarchar (10) COLLATE DATABASE_DEFAULT,
                                UnitWeight nvarchar (100) COLLATE DATABASE_DEFAULT,
                                UnitWeightUOM nvarchar (10) COLLATE DATABASE_DEFAULT,
                                Plating nvarchar(100) COLLATE DATABASE_DEFAULT,
                                MaterialType nvarchar (100) COLLATE DATABASE_DEFAULT,
                                PlantStatus nvarchar (10) COLLATE DATABASE_DEFAULT,
                                SAPProcType nvarchar(2) COLLATE DATABASE_DEFAULT,
                                SAPSPProcType nvarchar(10) COLLATE DATABASE_DEFAULT,
                                product nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MaterialClass nvarchar(100) COLLATE DATABASE_DEFAULT,
                                PIRType nvarchar(100) COLLATE DATABASE_DEFAULT,
                                amt1 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt2 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt3 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                amt4 nvarchar(50) COLLATE DATABASE_DEFAULT,
                                MassUpdateDate datetime,
                                countryorg nvarchar(50) COLLATE DATABASE_DEFAULT,
                                CompMaterial nvarchar(50) COLLATE DATABASE_DEFAULT,
                                CompMaterialDesc nvarchar(50) COLLATE DATABASE_DEFAULT,
                                AmtSCur numeric(10, 2),
                                SellingCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                AmtVCur numeric(10, 2),
                                VendorCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                Unit numeric(4, 0),
                                UOM nvarchar(4) COLLATE DATABASE_DEFAULT,
                                CusMatValFrom datetime,
                                CusMatValTo datetime,
                                ExchRate numeric(10, 5) NULL,
                                ExchRateValidFrom datetime NULL
                            ) ";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.ExecuteNonQuery();
                    #endregion 

                    #region Generate manual query to insert data to temp table
                    //sql = @"";
                    //for (int i = 0; i < MainAndCompData.Count; i++)
                    //{
                    //    sql += @" insert into ##tempDataToInsert(Plant,PIRNo,MaterialCode,MaterialDesc,VendorCode,VendorName,
                    //        ProcessGroup,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,
                    //        VendorCrcy,Unit,UOM,CusMatValFrom,CusMatValTo,ExchRate,ExchRateValidFrom,
                    //        NewRequestNumber)
                    //        values(
                    //        '" + MainAndCompData[i].Plant + @"',
                    //        '" + MainAndCompData[i].PIRNo + @"',
                    //        '" + MainAndCompData[i].MaterialCode + @"',
                    //        '" + MainAndCompData[i].MaterialDesc + @"',
                    //        '" + MainAndCompData[i].VendorCode + @"',
                    //        '" + MainAndCompData[i].VendorName + @"',
                    //        '" + MainAndCompData[i].ProcessGroup + @"',
                    //        '" + MainAndCompData[i].CompMaterial + @"',
                    //        '" + MainAndCompData[i].CompMaterialDesc + @"',
                    //        '" + MainAndCompData[i].AmtSCur + @"',
                    //        '" + MainAndCompData[i].SellingCrcy + @"',
                    //        '" + MainAndCompData[i].AmtVCur + @"',
                    //        '" + MainAndCompData[i].VendorCrcy + @"',
                    //        '" + MainAndCompData[i].Unit + @"',
                    //        '" + MainAndCompData[i].UOM + @"',
                    //        '" + MainAndCompData[i].CusMatValFrom.ToString("yyyy-MM-dd") + @"',
                    //        '" + MainAndCompData[i].CusMatValTo.ToString("yyyy-MM-dd") + @"',";
                    //    if (MainAndCompData[i].ExchRate == null) {
                    //        sql += @" NULL,";
                    //    }
                    //    else
                    //    {
                    //        sql += @" '" + MainAndCompData[i].ExchRate + @"', ";
                    //    }
                    //    if (MainAndCompData[i].ExchRateValidFrom == null)
                    //    {
                    //        sql += @" NULL,";
                    //    }
                    //    else
                    //    {
                    //        sql += @" '" + MainAndCompData[i].ExchRateValidFrom.ToString("yyyy-MM-dd") + @"',";
                    //    }
                    //    sql += @" '" + MainAndCompData[i].NewRequestNumber + @"',) ";
                    //}
                    #endregion

                    #region bulk copy
                    if (MyData.Rows.Count > 0)
                    {
                        using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EMETCon, SqlBulkCopyOptions.Default, EMETTrans))
                        {
                            //Give your Destination table name 
                            sqlBulk.DestinationTableName = "##tempDataToInsert";
                            sqlBulk.BulkCopyTimeout = 0;
                            sqlBulk.BatchSize = 10000;
                            sqlBulk.WriteToServer(MyData);
                        }
                    }
                    sql = "";
                    #endregion

                    #region update data emet status non request to valid request
                    sql = @" UPDATE 
                                    TQuoteDetails
                                SET 
                                    TQuoteDetails.Product = ##tempDataToInsert.Product,
                                    TQuoteDetails.MaterialClass = ##tempDataToInsert.MaterialClass,
                                    TQuoteDetails.PIRJobType = ##tempDataToInsert.PIRJobType,
                                    TQuoteDetails.PIRType = ##tempDataToInsert.PIRType,
                                    TQuoteDetails.NetUnit = ##tempDataToInsert.UnitWeight,
                                    TQuoteDetails.ActualNU = ##tempDataToInsert.UnitWeight,
                                    TQuoteDetails.UOM = ##tempDataToInsert.UnitWeightUOM,
                                    TQuoteDetails.BaseUOM = (select top 1 BaseUOM from " + DbMasterName + @".dbo.TMATERIAL M where M.Plant = ##tempDataToInsert.Plant and ISNULL(M.DelFlag,0) = 0),
                                    TQuoteDetails.SAPProcType = ##tempDataToInsert.SAPProcType,
                                    TQuoteDetails.SAPSPProcType = case when ##tempDataToInsert.SAPSPProcType = '' then 'Blank' else ##tempDataToInsert.SAPSPProcType end,
                                    TQuoteDetails.MaterialType = ##tempDataToInsert.MaterialType,
                                    TQuoteDetails.PlantStatus = ##tempDataToInsert.PlantStatus,
                                    TQuoteDetails.ERemarks = @ERemarks,
                                    TQuoteDetails.PICReason = @PICReason,
                                    TQuoteDetails.CreateStatus = 'Article',
                                    TQuoteDetails.QuoteResponseDueDate = @QuoteDue,
                                    TQuoteDetails.EffectiveDate = @ValidDate,
                                    TQuoteDetails.DueOn = @DueOn,
                                    TQuoteDetails.ApprovalStatus = 0,
                                    TQuoteDetails.PICApprovalStatus = NULL,
                                    TQuoteDetails.ManagerApprovalStatus = NULL,
                                    TQuoteDetails.DIRApprovalStatus = NULL,
                                    TQuoteDetails.AcsTabMatCost = @AcsTabMatCost,
                                    TQuoteDetails.AcsTabProcCost = @AcsTabProcCost,
                                    TQuoteDetails.AcsTabSubMatCost=@AcsTabSubMatCost,
                                    TQuoteDetails.AcsTabOthMatCost=@AcsTabOthMatCost,
                                    TQuoteDetails.CreatedOn=CURRENT_TIMESTAMP,
                                    TQuoteDetails.isMassRevision=@isMassRevision,
                                    TQuoteDetails.isMassRevisionAll=@isMassRevisionAll,
                                    TQuoteDetails.TotalMaterialCost = ##tempDataToInsert.amt1,
                                    TQuoteDetails.TotalProcessCost = ##tempDataToInsert.amt2,
                                    TQuoteDetails.TotalSubMaterialCost = ##tempDataToInsert.amt3,
                                    TQuoteDetails.TotalOtheritemsCost = ##tempDataToInsert.amt4,
                                    TQuoteDetails.OldTotMatCost = ##tempDataToInsert.amt1,
                                    TQuoteDetails.OldTotProCost = ##tempDataToInsert.amt2,
                                    TQuoteDetails.OldTotSubMatCost = ##tempDataToInsert.amt3,
                                    TQuoteDetails.OldTotOthCost = ##tempDataToInsert.amt4,
                                    TQuoteDetails.countryorg = ##tempDataToInsert.countryorg,
                                    TQuoteDetails.MassUpdateDate = ##tempDataToInsert.MassUpdateDate,
                                    TQuoteDetails.ToolAmorRemark = 'NO CHANGE',
                                    TQuoteDetails.MachineAmorRemark = 'NO CHANGE'
                                FROM 
                                    TQuoteDetails
                                    JOIN ##tempDataToInsert ON TQuoteDetails.RequestNumber=##tempDataToInsert.NewRequestNumber ";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.Parameters.AddWithValue("@AcsTabMatCost", IsMatCost);
                    cmd.Parameters.AddWithValue("@AcsTabProcCost", IsProcCost);
                    cmd.Parameters.AddWithValue("@AcsTabSubMatCost", IsSubMatCost);
                    cmd.Parameters.AddWithValue("@AcsTabOthMatCost", IsOthCost);
                    if (isMassRevisionAll == true)
                    {
                        cmd.Parameters.AddWithValue("@isMassRevision", false);
                    }
                    else {
                        cmd.Parameters.AddWithValue("@isMassRevision", true);
                    }
                    cmd.Parameters.AddWithValue("@isMassRevisionAll", isMassRevisionAll);
                    if (RePurposeReason == "Others")
                    {
                        cmd.Parameters.AddWithValue("@PICReason", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ERemarks", RePurposeRemark);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ERemarks", DBNull.Value);
                        cmd.Parameters.AddWithValue("@PICReason", RePurposeReason);
                    }
                    DateTime QuoteDate = DateTime.ParseExact(DueDatenextRev.Replace("/","-"), "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@QuoteDue", QuoteDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    DateTime ValidDate = DateTime.ParseExact(EffDate.Replace("/", "-"), "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@ValidDate", ValidDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    DateTime DueOn = DateTime.ParseExact(DueDatenextRev.Replace("/", "-"), "dd-MM-yyyy",null);
                    cmd.Parameters.AddWithValue("@DueOn", DueOn.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region update data emet status For IPO (the PIR Quote No Exist In emet)
                    sql = @" UPDATE TQuoteDetails
                                SET 
                                    TQuoteDetails.MassRevQutoteRef = C.Quotation
                                FROM 
                                    TQuoteDetails A JOIN ##tempDataToInsert B ON A.RequestNumber=B.NewRequestNumber 
                                    JOIN " + DbMasterName + @".dbo.tPIRvsQuotation C ON A.Plant = C.plant AND A.Material = C.Material 
                                    JOIN " + DbMasterName + @".dbo.tPir_New PN ON PN.Plant = C.plant AND PN.Material = C.Material and PN.Quotation = c.Quotation
                                    AND A.VendorCode1 = C.Vendor AND B.PIRNo = C.InfoRecord and C.ValidTo > @ValidDate
                                    and c.Quotation in (select QuoteNo from TQuoteDetails where  ManagerApprovalStatus =2 and (DIRApprovalStatus=0 or DIRApprovalStatus=1) ) ";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.Parameters.AddWithValue("@ValidDate", ValidDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region insert data to TSMMBOM_RAWMATCost_EffDate
                    sql = @" insert into TSMMBOM_RAWMATCost_EffDate(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                            AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy) 
                            select distinct NewRequestNumber,concat(CodeRef,NewRequestNumber),CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy,Unit
                            ,UOM,CusMatValFrom,CusMatValTo,ExchRate,
                            ExchRateValidFrom,CURRENT_TIMESTAMP,@CreateBy
                            from ##tempDataToInsert 
                            where concat(NewRequestNumber,'-',concat(CodeRef,NewRequestNumber),'-',CompMaterial) not in (select concat(RequestNo,'-',QuoteNo,'-',RawMaterialCode) from TSMMBOM_RAWMATCost_EffDate )";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@CreateBy", UserId);
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region save data bom list raw material before effective date
                    var DistinctData = MainAndCompData.Select(p => new {
                        p.NewRequestNumber,
                        p.CodeRef,
                        p.Plant,
                        p.PIRNo,
                        p.MaterialCode,
                        p.MaterialDesc,
                        p.VendorCode,
                        p.VendorName,
                        p.ProcessGroup
                    }).Distinct();
                    

                    DataTable dtRawmatBefEffdate = DtRawmatBefEffDate(ValidDate, DistinctData);
                    if (dtRawmatBefEffdate.Rows.Count > 0)
                    {
                        #region create temp table for valid data req list 
                        sql = @"create table ##tempOldRawmaterial
                            (
                                NewRequestNumber nvarchar (500) COLLATE DATABASE_DEFAULT,
                                QuoteNo nvarchar (500) COLLATE DATABASE_DEFAULT,
	                            CompMaterial nvarchar(509) COLLATE DATABASE_DEFAULT,
                                CompMaterialDesc nvarchar(500) COLLATE DATABASE_DEFAULT,
                                AmtSCur numeric(10, 2),
                                SellingCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                AmtVCur numeric(10, 2),
                                VendorCrcy nvarchar(3) COLLATE DATABASE_DEFAULT,
                                Unit numeric(4, 0),
                                UOM nvarchar(4) COLLATE DATABASE_DEFAULT,
                                CusMatValFrom datetime,
                                CusMatValTo datetime,
                                ExchRate numeric(10, 5) NULL,
                                ExchRateValidFrom datetime NULL
                            ) "; ;
                        cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                        cmd.ExecuteNonQuery();

                        if (dtRawmatBefEffdate.Rows.Count > 0)
                        {
                            using (SqlBulkCopy sqlBulk = new SqlBulkCopy(EMETCon, SqlBulkCopyOptions.Default, EMETTrans))
                            {
                                //Give your Destination table name 
                                sqlBulk.DestinationTableName = "##tempOldRawmaterial";
                                sqlBulk.BulkCopyTimeout = 0;
                                sqlBulk.BatchSize = 10000;
                                sqlBulk.WriteToServer(dtRawmatBefEffdate);
                            }
                        }
                        #endregion create temp table for valid data req list 

                        #region insert data to TSMNBOM_RAWMATCost
                        sql = @" insert into TSMNBOM_RAWMATCost(RequestNo,QuoteNo,RawMaterialCode,RawMaterialDesc,AmtSCur,SellingCrcy,
                                         AmtVCur,VendorCrcy,Unit,UOM,ValidFrom,ValidTo,ExchRate,ExchValidFrom,CreatedON,CreateBy)
                                          select distinct NewRequestNumber,QuoteNo,CompMaterial,CompMaterialDesc,AmtSCur,SellingCrcy,AmtVCur,VendorCrcy,Unit
                            ,UOM,CusMatValFrom,CusMatValTo,ExchRate,
                            ExchRateValidFrom,CURRENT_TIMESTAMP,@CreateBy
                            from ##tempOldRawmaterial 
                            where concat(NewRequestNumber,'-',QuoteNo,'-',CompMaterial) not in (select concat(RequestNo,'-',QuoteNo,'-',RawMaterialCode) from TSMNBOM_RAWMATCost )";
                        cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@CreateBy", UserId);
                        cmd.ExecuteNonQuery();
                        #endregion
                        
                    }
                    #endregion

                    #region preapre and save content for mail
                    string footer = @" Please <a href=" + Convert.ToString(URL.ToString()) + @">Login</a> 
                    SHIMANO e-MET system  for Quote submission.<br /><br />Thank you and Best Regards,<br />Manufacturing Execution System Administrator<br />
                    <font  color=''red''>This is System generated mail.  Please do not reply to this message.</font>";
                    
                    sql = @" insert into email(quotenumber, body) 
                                  SELECT concat(CodeRef,NewRequestNumber),
                                  concat('The details are<br /><br /> Plant : ',Plant,' <br /> Vendor Name : ',VendorName,
                                         '<br /> Request Number : ',NewRequestNumber,
                                         '<br />  Quote Number : ',concat(CodeRef,NewRequestNumber),
                                         '<br /> Partcode And Description : ',MaterialCode ,' | ', MaterialDesc,
                                         '<br />  Quotation Response due Date : ',@ResDueDate, '<br /><br />',@footer
                                        )
                                    from ##tempDataToInsert 
                             where concat(CodeRef,NewRequestNumber) not in (select quotenumber from email) ";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    DateTime ResDueDate = DateTime.ParseExact(RespDueDate.Replace("/", "-"), "dd-MM-yyyy", null);
                    cmd.Parameters.AddWithValue("@ResDueDate", ResDueDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@footer", footer);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                    #endregion

                    #region drop temp table 
                    sql = @" IF OBJECT_ID('tempdb..##tempDataToInsert') IS NOT NULL DROP TABLE ##tempDataToInsert";
                    sql += @" IF OBJECT_ID('tempdb..##tempOldRawmaterial') IS NOT NULL DROP TABLE ##tempOldRawmaterial";
                    cmd = new SqlCommand(sql, EMETCon, EMETTrans);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    EMETTrans.Commit();
                    #endregion


                    MyResult.success = true;
                    MyResult.message = "OK";
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data In List Need To Check";
                }
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                if (EMETTrans != null)
                {
                    EMETTrans.Rollback();
                }

                EMETModule.SendExcepToDB(ex);
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            finally
            {
                EMETTrans.Dispose();
                EMETCon.Dispose();
            }
        }
        
    }
}
