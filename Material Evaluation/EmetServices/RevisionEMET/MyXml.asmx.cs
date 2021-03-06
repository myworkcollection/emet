using Material_Evaluation.EmetServices.Model;
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

namespace Material_Evaluation.EmetServices.RevisionEMET
{
    /// <summary>
    /// Summary description for MyXml
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MyXml : System.Web.Services.WebService
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

        public string SrvUserid;
        public string Srvpassword;
        public string Srvdomain;
        public string Srvpath;
        public string SrvURL;
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
        string GenerateReqNo()
        {
            SqlConnection EmetConReqNo = new SqlConnection(EMETModule.GenEMETConnString());
            string GenerateReqNo = "";
            try
            {
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
                    string sql = @" select MAX(RequestNumber) from TQuoteDetails ";
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
                                HttpContext.Current.Session["RequestIncNumber1"] = "00000";
                                string.Concat(currentYear, RequestIncNumber);
                                RequestIncNumber = string.Concat(currentYear, RequestIncNumber);
                                HttpContext.Current.Session["RequestIncNumber1"] = string.Concat(currentYear, RequestIncNumber);
                                GenerateReqNo = RequestIncNumber;
                            }
                            else
                            {

                                ReqNum = ReqNum.Remove(0, 2);
                                ReqNum = string.Concat(currentYear, ReqNum);
                                RequestIncNumber = ReqNum;
                                HttpContext.Current.Session["RequestIncNumber1"] = ReqNum;
                                GenerateReqNo = RequestIncNumber;
                            }
                            int newReq = (int.Parse(RequestIncNumber)) + (1);
                            RequestIncNumber = newReq.ToString();
                            GenerateReqNo = RequestIncNumber;
                            HttpContext.Current.Session["RequestIncNumber1"] = RequestIncNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetConReqNo.Dispose();
            }
            return GenerateReqNo;
        }

        [WebMethod(EnableSession = true)]
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
                string plant = HttpContext.Current.Session["EPlant"].ToString();
                DateTime dteffdate = DateTime.Now;
                dteffdate = DateTime.ParseExact(EffectiveDate, "dd-MM-yyyy", null);

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
                        TR.ValidFrom as ValidFrom,tc.ValidFrom as [CusMatValFrom],tc.ValidTo as [CusMatValTo]
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

                if (HttpContext.Current.Session["Rawmaterial"] == null)
                {
                    HttpContext.Current.Session["Rawmaterial"] = dtget;
                }
                else
                {
                    if (dtget.Rows.Count > 0)
                    {
                        DataTable DtRawmat = (DataTable)HttpContext.Current.Session["Rawmaterial"];
                        foreach (DataRow drdtget in dtget.Rows)
                        {
                            DtRawmat.ImportRow(drdtget);
                        }

                        HttpContext.Current.Session["Rawmaterial"] = DtRawmat;
                    }
                }

                if (dtget.Rows.Count > 0)
                {
                    DataTable DtCreateReqTemp = new DataTable();
                    if (HttpContext.Current.Session["CreateReqTemp"] == null)
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
                        DtCreateReqTemp.Columns.Add("ValidFrom", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("CusMatValFrom", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("CusMatValTo", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("IsUseToolAmortize", typeof(string));
                        DtCreateReqTemp.Columns.Add("Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("MaterialDesc", typeof(string));
                        DtCreateReqTemp.Columns.Add("ProcessGroup", typeof(string));
                    }
                    else
                    {
                        DtCreateReqTemp = (DataTable)HttpContext.Current.Session["CreateReqTemp"];
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
                        dr["ValidFrom"] = dtget.Rows[i]["ValidFrom"];
                        dr["CusMatValFrom"] = dtget.Rows[i]["CusMatValFrom"];
                        dr["CusMatValTo"] = dtget.Rows[i]["CusMatValTo"];
                        dr["IsUseToolAmortize"] = dtget.Rows[i]["IsUseToolAmortize"].ToString();
                        dr["Material"] = dtget.Rows[i]["Material"].ToString();
                        dr["MaterialDesc"] = dtget.Rows[i]["MaterialDesc"].ToString();
                        dr["ProcessGroup"] = dtget.Rows[i]["ProcessGroup"].ToString();
                        DtCreateReqTemp.Rows.Add(dr);
                    }
                    HttpContext.Current.Session["CreateReqTemp"] = DtCreateReqTemp;
                }
                else
                {
                    GetDataforNoBom(reqno.ToString());
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
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
                string plant = HttpContext.Current.Session["EPlant"].ToString();
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
                                from " + DbTransName + @"TQuoteDetails TQ 
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
                    DataTable DtCreateReqTemp = new DataTable();
                    if (HttpContext.Current.Session["CreateReqTemp"] == null)
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
                        DtCreateReqTemp.Columns.Add("ValidFrom", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("CusMatValFrom", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("CusMatValTo", typeof(DateTime));
                        DtCreateReqTemp.Columns.Add("IsUseToolAmortize", typeof(string));
                        DtCreateReqTemp.Columns.Add("Material", typeof(string));
                        DtCreateReqTemp.Columns.Add("MaterialDesc", typeof(string));
                        DtCreateReqTemp.Columns.Add("ProcessGroup", typeof(string));
                    }
                    else
                    {
                        DtCreateReqTemp = (DataTable)HttpContext.Current.Session["CreateReqTemp"];
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
                        dr["ValidFrom"] = DBNull.Value;
                        dr["CusMatValFrom"] = DBNull.Value;
                        dr["CusMatValTo"] = DBNull.Value;
                        dr["IsUseToolAmortize"] = dtget.Rows[i]["IsUseToolAmortize"].ToString();
                        dr["Material"] = dtget.Rows[i]["Material"].ToString();
                        dr["MaterialDesc"] = dtget.Rows[i]["MaterialDesc"].ToString();
                        dr["ProcessGroup"] = dtget.Rows[i]["ProcessGroup"].ToString();
                        DtCreateReqTemp.Rows.Add(dr);
                    }
                    HttpContext.Current.Session["CreateReqTemp"] = DtCreateReqTemp;
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMConDataforNoBom.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
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
						TR.ValidFrom as ValidFrom,tc.ValidFrom as [CusMatValFrom],tc.ValidTo as [CusMatValTo]
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
                            if (HttpContext.Current.Session["OldRawmaterial"] == null)
                            {
                                HttpContext.Current.Session["OldRawmaterial"] = dt;
                            }
                            else
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    DataTable DtRawmat = (DataTable)HttpContext.Current.Session["OldRawmaterial"];
                                    foreach (DataRow drdtget in dt.Rows)
                                    {
                                        DtRawmat.ImportRow(drdtget);
                                    }

                                    HttpContext.Current.Session["OldRawmaterial"] = DtRawmat;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeleteNonRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string sql = @"delete from TQuoteDetails Where (CreateStatus = '' and createdby= '" + HttpContext.Current.Session["userID"].ToString() + @"') 
                                or (CreateStatus is null and createdby= '" + HttpContext.Current.Session["userID"].ToString() + "')";
                cmd = new SqlCommand(sql, EmetCon);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetQuoteList(bool ActiveMaterial, string ReqType, string Product, string MatClassDesc, string ProcGroup, 
            string SubProc, string Filter, string FilterValue, bool IsExternal, List<VendorVsMaterial> VendorVsMaterialValid, List<VendorVsMaterial> VendorVsMaterialInvalid)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;
            DataTable DtResult = new DataTable();
            try
            {
                GetDbMaster();
                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    if (VendorVsMaterialValid.Count > 0) {
                        string Mysql = @" select top 0 VendorCode1 as vendor, Material into #tempVendorVsMaterialValid from TQuoteDetails ";
                        for (int i = 0; i < VendorVsMaterialValid.Count ; i++)
                        {
                            Mysql += @" insert into #tempVendorVsMaterialValid(vendor,Material)values('"+ VendorVsMaterialValid[i].Vendor + @"','" + VendorVsMaterialValid[i].Material + @"') ";
                        }
                        cmd = new SqlCommand(Mysql, EmetCon, EmetTrans);
                        cmd.ExecuteNonQuery();
                    }

                    if (VendorVsMaterialInvalid.Count > 0)
                    {
                        string Mysql = " select top 0 VendorCode1 as vendor, Material into #tempVendorVsMaterialValid from TQuoteDetails ";
                        for (int i = 0; i < VendorVsMaterialInvalid.Count; i++)
                        {
                            Mysql += @" insert into #tempVendorVsMaterialValid(vendor,Material)values('" + VendorVsMaterialInvalid[i].Vendor + @"','" + VendorVsMaterialInvalid[i].Material + @"') ";
                        }
                        cmd = new SqlCommand(Mysql, EmetCon, EmetTrans);
                        cmd.ExecuteNonQuery();
                    }

                    
                    #region Select Data
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
							when ((select count(*) from TToolAmortization TT where TT.QuoteNo = TQ.QuoteNo and CURRENT_TIMESTAMP between tt.EffectiveFrom and tt.DueDate) > 0) then 0
							else 1 end as 'ToolAmorExpired'
                            
							,case 
							when ((select count(*) from TMachineAmortization TM where TM.QuoteNo = TQ.QuoteNo ) > 0) then 1
							else 0 end as 'MacAmorExist'
							,case 
							when ((select count(*) from TMachineAmortization TM where TM.QuoteNo = TQ.QuoteNo ) = 0) then 0
							when ((select count(*) from TMachineAmortization TM where TM.QuoteNo = TQ.QuoteNo and (CURRENT_TIMESTAMP not between TM.EffectiveFrom and TM.DueDate)) > 0) then 1
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


                    #endregion

                    if (VendorVsMaterialValid.Count > 0)
                    {
                        sql += @" and (concat(VendorCode1,'-',VendorCode1) not in (select concat(vendor,'-',Material) from #tempVendorVsMaterialValid ) ) 
                                drop table #tempVendorVsMaterialValid ";
                    }

                    if (VendorVsMaterialInvalid.Count > 0)
                    {
                        sql += @" and (concat(VendorCode1,'-',VendorCode1) not in (select concat(vendor,'-',Material) from #tempVendorVsMaterialInvalid ) )
                                  drop table ##tempVendorVsMaterialInvalid ";
                    }

                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
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
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                ResultQueteRef MyResult = new ResultQueteRef();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CekVendorVsMaterialExpiredReq(List<VendorVsMaterial> VendVsMat)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            DataTable DtResult = new DataTable();
            try
            {
                #region setup vend vs mat data
                string VendVsMatList = "";
                for (int i = 0; i < VendVsMat.Count; i++)
                {
                    VendVsMatList += "N'" + VendVsMat[i].Vendor.ToString() + "-" + VendVsMat[i].Material.ToString() + "',";
                }

                if (VendVsMatList.Length > 0)
                {
                    VendVsMatList = VendVsMatList.Remove(VendVsMatList.Length - 1, 1);
                }
                #endregion

                Result_InvalidDataSelected MyResult = new Result_InvalidDataSelected();
                if (VendVsMatList != "")
                {
                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        string sql = @" select distinct Plant,RequestNumber,format(RequestDate,'dd-MM-yyyy') as RequestDate,format(QuoteResponseDueDate,'dd-MM-yyyy') as QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            into #temp
                            from TQuoteDetails 
                            where concat(VendorCode1,'-',Material) in  (" + VendVsMatList + @") 
                            and ApprovalStatus = 0 
                            and format(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') 

                            select distinct Plant,RequestNumber,RequestDate
                            ,QuoteResponseDueDate
                            ,QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            ,MaterialType,MaterialClass,UOM,PlantStatus,SAPProcType,SAPSpProcType
                            ,Product,PIRType,PIRJobType,NetUnit
                            from TQuoteDetails where RequestNumber in ( select RequestNumber from #temp)
                            and plant = @plant
                            order by Plant,RequestNumber asc
                            drop table #temp
                            ";

                        SqlCommand cmd = new SqlCommand(sql, EmetCon);
                        sda.SelectCommand = cmd;
                        cmd.Parameters.AddWithValue("@plant", HttpContext.Current.Session["EPlant"].ToString());
                        sda.Fill(DtResult);
                    }
                    
                    if (DtResult.Rows.Count > 0)
                    {
                        MyResult.success = true;
                        MyResult.message = "Data Invalid Found";
                        
                        var MainData = DtResult.AsEnumerable().Select(row => new
                            {
                                Plant = row["Plant"],
                                RequestNumber = row["RequestNumber"],
                                RequestDate = row["RequestDate"],
                                QuoteResponseDueDate = row["QuoteResponseDueDate"],
                                Material = row["Material"],
                                MaterialDesc = row["MaterialDesc"]
                            }).Distinct();
                        DataTable MainDataResult = LINQResultToDataTable(MainData);
                        var MyMainDataResult = MainDataResult.AsEnumerable().Select(row => new
                        {
                            Plant = row["Plant"],
                            RequestNumber = row["RequestNumber"],
                            RequestDate = row["RequestDate"],
                            QuoteResponseDueDate = row["QuoteResponseDueDate"],
                            Material = row["Material"],
                            MaterialDesc = row["MaterialDesc"]
                        });

                        var convertdata = DtResult.AsEnumerable().Select(row => new
                        {
                            Plant = row["Plant"],
                            RequestNumber = row["RequestNumber"],
                            RequestDate = row["RequestDate"],
                            QuoteResponseDueDate = row["QuoteResponseDueDate"],
                            QuoteNo = row["QuoteNo"],
                            Material = row["Material"],
                            MaterialDesc = row["MaterialDesc"],
                            VendorCode1 = row["VendorCode1"],
                            VendorName = row["VendorName"]
                        });
                        
                        MyResult.mainData = MyMainDataResult;
                        MyResult.InvalidDataSelected = convertdata;
                    }
                    else
                    {
                        MyResult.success = true;
                        MyResult.message = "Ok";
                    }
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data In List Need To Check";
                }
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue}.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                Result_InvalidDataSelected MyResult = new Result_InvalidDataSelected();
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CekVendorVsMaterial(List<VendorVsMaterial> VendVsMat)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            DataTable DtResult = new DataTable();
            Result_InvalidDataSelected MyResult = new Result_InvalidDataSelected();
            try
            {
                #region setup vend vs mat data
                string VendVsMatList = "";
                for (int i = 0; i < VendVsMat.Count; i++)
                {
                    VendVsMatList += "'" + VendVsMat[i].Vendor.ToString() + "-" + VendVsMat[i].Material.ToString() + "',";
                }

                if (VendVsMatList.Length > 0)
                {
                    VendVsMatList = VendVsMatList.Remove(VendVsMatList.Length - 1, 1);
                }
                #endregion

                if (VendVsMatList != "")
                {
                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        string sql = @" select distinct Plant,RequestNumber,RequestDate,QuoteResponseDueDate,
                            QuoteNo, Material,MaterialDesc,VendorCode1,VendorName
                            from TQuoteDetails where concat(VendorCode1,'-',Material) in  (" + VendVsMatList + @") and ApprovalStatus in ('0','2') ";

                        cmd = new SqlCommand(sql, EmetCon);
                        sda.SelectCommand = cmd;
                        sda.Fill(DtResult);
                        if (DtResult.Rows.Count > 0)
                        {
                            MyResult.success = true;
                            MyResult.message = "Data Invalid Found";
                            var convertdata = DtResult.AsEnumerable().Select(row => new
                            {
                                Plant = row["Plant"],
                                RequestNumber = row["RequestNumber"],
                                RequestDate = row["RequestDate"],
                                QuoteResponseDueDate = row["QuoteResponseDueDate"],
                                QuoteNo = row["QuoteNo"],
                                Material = row["Material"],
                                MaterialDesc = row["MaterialDesc"],
                                VendorCode1 = row["VendorCode1"],
                                VendorName = row["VendorName"]
                            });

                            MyResult.InvalidDataSelected = convertdata;
                        }
                        else
                        {
                            MyResult.success = true;
                            MyResult.message = "Ok";
                        }
                    }
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
                EMETModule.SendExcepToDB(ex);
                MyResult.success = false;
                MyResult.message = ex.ToString();
                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SumbitDuplicateReqList(List<DuplicateReqListAction> DuplicateReqListAction)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            SqlTransaction EmetTrans = null;

            GlobalResult MyResult = new GlobalResult();
            DataTable DtResult = new DataTable();
            try
            {

                EmetCon.Open();
                EmetTrans = EmetCon.BeginTransaction();
                string sql = "";
                for (int i = 0; i < DuplicateReqListAction.Count; i++)
                {
                    //string QuNo = DuplicateReqListAction[i].QuoteNo.ToString();
                    string ReqNo = DuplicateReqListAction[i].RequestNumber.ToString();
                    DateTime DtDueDate = DateTime.ParseExact(DuplicateReqListAction[i].NewResDueDate, "dd-MM-yyyy", null);
                    string StrNewQuoteResponseDueDate = DtDueDate.ToString("yyyy-MM-dd");
                    if (DuplicateReqListAction[i].ActionRej == true)
                    {
                        sql = @" Update TQuoteDetails SET ApprovalStatus='" + 1 + "', PICApprovalStatus = '" + 1 + @"',  --PICReason = 'Planning to create New Requset '
                                , ManagerApprovalStatus= '" + 1 + "', DIRApprovalStatus= '" + 1 + @"', UpdatedBy=@userID
                                , UpdatedOn =CURRENT_TIMESTAMP 
                                      where RequestNumber = @ReqNo ";
                    }
                    else
                    {
                        sql = @" update TQuoteDetails set QuoteResponseDueDate = @QuoteResponseDueDate where RequestNumber = @ReqNo ";
                    }

                    cmd = new SqlCommand(sql, EmetCon, EmetTrans);
                    cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
                    cmd.Parameters.AddWithValue("@userID", HttpContext.Current.Session["userID"].ToString());
                    cmd.Parameters.AddWithValue("@QuoteResponseDueDate", StrNewQuoteResponseDueDate);
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                EmetTrans.Commit();
                MyResult.success = true;
                MyResult.message = "Data Updated";
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            catch (Exception ex)
            {
                EmetTrans.Rollback();
                EMETModule.SendExcepToDB(ex);
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);

            }
            finally
            {
                EmetTrans.Dispose();
                EmetCon.Dispose();
            }
        }
        
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string createRequestTemp(List<FormDataRevisionEmetCreateReq> MyDataTemp)
        {
            DeleteNonRequest();
            HttpContext.Current.Session["CreateReqTemp"] = null;
            HttpContext.Current.Session["Rawmaterial"] = null;
            HttpContext.Current.Session["OldRawmaterial"] = null;
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            ResultReqTemp MyResult = new ResultReqTemp();
            DataTable DtResult = new DataTable();
            try
            {
                if (MyDataTemp.Count > 0)
                {
                    EmetCon.Open();
                    for (int i = 0; i < MyDataTemp.Count; i++)
                    {
                        
                        string RequestIncNumber = GenerateReqNo().ToString();
                        string QuoteSearchTerm = "";
                        QuoteSearchTerm = MyDataTemp[i].QuoteNo.ToString().Substring(0, 3);
                        string QuoteNo = String.Concat(QuoteSearchTerm, RequestIncNumber);

                        string AcsTabMatCost = MyDataTemp[i].IsMatcostAllow.ToString();
                        string AcsTabProcCost = MyDataTemp[i].IsProccostAllow.ToString();
                        string AcsTabSubMatCost = MyDataTemp[i].IsSubMatcostAllow.ToString();
                        string AcsTabOthMatCost = MyDataTemp[i].IsOthcostAllow.ToString();

                        string IsUseToolAmortize = "0";
                        string ToolAmortizeCondition = MyDataTemp[i].IsUseToolAmor.ToString();
                        if (ToolAmortizeCondition == "ADD")
                        {
                            IsUseToolAmortize = "1";
                        }
                        else if (ToolAmortizeCondition == "REMOVE")
                        {
                            IsUseToolAmortize = "0";
                        }
                        else
                        {
                            IsUseToolAmortize = "0";
                        }
                        
                        string IsUseMachineAmortize = "0";
                        string MachinelAmortizeCondition = MyDataTemp[i].IsUseMachineAmor.ToString();
                        if (MachinelAmortizeCondition == "ADD")
                        {
                            IsUseMachineAmortize = "1";
                        }
                        else if (MachinelAmortizeCondition == "REMOVE")
                        {
                            IsUseMachineAmortize = "0";
                        }
                        else
                        {
                            IsUseMachineAmortize = "0";
                        }

                        DateTime ResDueDate = MyDataTemp[i].ResDueDate;
                        DateTime DtmEffectiveDate = MyDataTemp[i].EffectiveDate;
                        DateTime DtmDueDateNextRev = MyDataTemp[i].DueDateNextRev;
                        string BaseUOM = MyDataTemp[i].BaseUOM == null ? "" : MyDataTemp[i].BaseUOM;
                        string MQty = MyDataTemp[i].MQty == null ? "" : MyDataTemp[i].MQty;
                        string Reason = MyDataTemp[i].ReqPurpose == null ? "" : MyDataTemp[i].ReqPurpose;
                        string Remark = MyDataTemp[i].Remark == null ? "" : MyDataTemp[i].Remark;
                        string RecycleRatio = MyDataTemp[i].RecycleRatio;

                        string sql = @"INSERT INTO TQuoteDetails (RequestNumber,RequestDate,QuoteNo,Plant,PlantStatus,
                                                MaterialType,SAPProcType,SAPSpProcType,Product, 
                                                MaterialClass,Material,MaterialDesc,PIRType,ProcessGroup,VendorCode1,VendorName,PIRJobType,NetUnit,
                                                QuoteResponseDueDate,EffectiveDate,DueOn,CreatedBy,BaseUOM,MQty,ERemarks,PICReason,UOM,ActualNU,
                                                AcsTabMatCost,AcsTabProcCost,AcsTabSubMatCost,AcsTabOthMatCost,QuoteNoRef,SMNPicDept,IMRecycleRatio
                                                ,IsUseToolAmortize,IsUseMachineAmortize,ToolAmorRemark,MachineAmorRemark) VALUES 
                                                (@RequestNumber,CURRENT_TIMESTAMP,@QuoteNo,@Plant,@PlantStatus,
                                                @MaterialType,@SAPProcType,@SAPSpProcType,@Product, 
                                                @MaterialClass,@Material,@MaterialDesc,@PIRType,@ProcessGroup,@VendorCode1,@VendorName,@PIRJobType,@NetUnit,
                                                @QuoteResponseDueDate,@EffectiveDate,@DueOn,@CreatedBy,@BaseUOM,@MQty,@ERemarks,@PICReason,@UOM,@ActualNU,
                                                @AcsTabMatCost,@AcsTabProcCost,@AcsTabSubMatCost,@AcsTabOthMatCost,@QuoteNoRef,@SMNPicDept,@IMRecycleRatio
                                                ,@IsUseToolAmortize,@IsUseMachineAmortize,@ToolAmorRemark,@MachineAmorRemark)";

                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@RequestNumber", Convert.ToInt32(RequestIncNumber.ToString()));
                        cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                        cmd.Parameters.AddWithValue("@Plant", Convert.ToInt32(HttpContext.Current.Session["EPlant"].ToString()));
                        cmd.Parameters.AddWithValue("@PlantStatus", MyDataTemp[i].PlantStatus == null ? "" : MyDataTemp[i].PlantStatus);
                        cmd.Parameters.AddWithValue("@MaterialType", MyDataTemp[i].MaterialType == null ? "" : MyDataTemp[i].MaterialType);
                        cmd.Parameters.AddWithValue("@SAPProcType", MyDataTemp[i].SAPProcType == null ? "" : MyDataTemp[i].SAPProcType);
                        cmd.Parameters.AddWithValue("@SAPSpProcType", MyDataTemp[i].SAPSpProcType == null ? "" : MyDataTemp[i].SAPSpProcType);
                        cmd.Parameters.AddWithValue("@Product", MyDataTemp[i].Product == null ? "" : MyDataTemp[i].Product);
                        cmd.Parameters.AddWithValue("@MaterialClass", MyDataTemp[i].MaterialClass == null ? "" : MyDataTemp[i].MaterialClass);
                        cmd.Parameters.AddWithValue("@Material", MyDataTemp[i].Material == null ? "" : MyDataTemp[i].Material);
                        cmd.Parameters.AddWithValue("@MaterialDesc", MyDataTemp[i].MaterialDesc == null ? "" : MyDataTemp[i].MaterialDesc);
                        cmd.Parameters.AddWithValue("@PIRType", MyDataTemp[i].PIRType == null ? "" : MyDataTemp[i].PIRType);
                        cmd.Parameters.AddWithValue("@ProcessGroup", MyDataTemp[i].ProcessGroup == null ? "" : MyDataTemp[i].ProcessGroup);
                        cmd.Parameters.AddWithValue("@VendorCode1", MyDataTemp[i].Vendor == null ? "" : MyDataTemp[i].Vendor);
                        cmd.Parameters.AddWithValue("@VendorName", MyDataTemp[i].VendorName == null ? "" : MyDataTemp[i].VendorName);
                        cmd.Parameters.AddWithValue("@PIRJobType", MyDataTemp[i].PIRJobType == null ? "" : MyDataTemp[i].PIRJobType);
                        cmd.Parameters.AddWithValue("@NetUnit", MyDataTemp[i].NetUnit == null ? "" : MyDataTemp[i].NetUnit);
                        cmd.Parameters.AddWithValue("@DrawingNo", MyDataTemp[i].FileName == null ? "" : MyDataTemp[i].FileName);
                        cmd.Parameters.AddWithValue("@QuoteResponseDueDate", ResDueDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@EffectiveDate", DtmEffectiveDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@DueOn", DtmDueDateNextRev.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@CreatedBy", HttpContext.Current.Session["userID"].ToString());
                        cmd.Parameters.AddWithValue("@BaseUOM", BaseUOM);
                        cmd.Parameters.AddWithValue("@MQty", MQty);
                        if (Reason == "Others")
                        {
                            cmd.Parameters.AddWithValue("@PICReason", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ERemarks", Remark.ToString().Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PICReason", Reason);
                            cmd.Parameters.AddWithValue("@ERemarks", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@UOM", MyDataTemp[i].UOM == null ? "" : MyDataTemp[i].UOM);
                        cmd.Parameters.AddWithValue("@ActualNU", MyDataTemp[i].NetUnit == null ? "" : MyDataTemp[i].NetUnit);
                        cmd.Parameters.AddWithValue("@AcsTabMatCost", AcsTabMatCost);
                        cmd.Parameters.AddWithValue("@AcsTabProcCost", AcsTabProcCost);
                        cmd.Parameters.AddWithValue("@AcsTabSubMatCost", AcsTabSubMatCost);
                        cmd.Parameters.AddWithValue("@AcsTabOthMatCost", AcsTabOthMatCost);
                        cmd.Parameters.AddWithValue("@QuoteNoRef", MyDataTemp[i].QuoteNo);
                        cmd.Parameters.AddWithValue("@SMNPicDept", HttpContext.Current.Session["userDept"].ToString());
                        cmd.Parameters.AddWithValue("@IMRecycleRatio", RecycleRatio);
                        cmd.Parameters.AddWithValue("@IsUseToolAmortize", IsUseToolAmortize);
                        cmd.Parameters.AddWithValue("@IsUseMachineAmortize", IsUseMachineAmortize);
                        cmd.Parameters.AddWithValue("@ToolAmorRemark", ToolAmortizeCondition);
                        cmd.Parameters.AddWithValue("@MachineAmorRemark", MachinelAmortizeCondition);
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        GetData(RequestIncNumber.ToString(), MyDataTemp[i].Material, MyDataTemp[i].EffectiveDate.ToString("dd-MM-yyyy"));
                        GetBOMRawmaterialBefEffdate(RequestIncNumber.ToString(), QuoteNo, MyDataTemp[i].Vendor, MyDataTemp[i].Material, HttpContext.Current.Session["EPlant"].ToString(), MyDataTemp[i].EffectiveDate.ToString("dd-MM-yyyy"));
                    }
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data Can Proceed";
                }

                if (HttpContext.Current.Session["CreateReqTemp"] != null)
                {
                    DtResult = (DataTable)HttpContext.Current.Session["CreateReqTemp"];
                    var convertdata = DtResult.AsEnumerable().Select(row => new
                    {
                        QuoteNoRef = row["Quote No Ref"],
                        ReqNo = row["Req No"],
                        Plant = row["Plant"],
                        CompMaterial = row["Comp Material"],
                        CompMaterialDesc = row["Comp Material Desc"],
                        VendorCode1 = row["Vendor Code"],
                        VendorName = row["Vendor Name"],
                        SearchTerm = row["SearchTerm"],
                        QuoteNo = row["Quote No New"],
                        VenPIC = row["Ven PIC"],
                        PICEmail = row["PIC Email"],
                        SellCurrency = row["Selling Crcy"],
                        AmtScur = row["Amt SCur"],
                        ExchangeRate = row["Exch Rate"],
                        VndCurrency = row["Vendor Crcy"],
                        AmtVcur = row["Amt VCur"],
                        Unit = row["Unit"],
                        UOM = row["UOM"],
                        ValidFrom = row["ValidFrom"],
                        CusMatValFrom = row["CusMatValFrom"],
                        CusMatValTo = row["CusMatValTo"],
                        Material = row["Material"],
                        MaterialDesc = row["MaterialDesc"],
                        ProcessGroup = row["ProcessGroup"]
                    });

                    MyResult.success = true;
                    MyResult.message = "Temp Data Created";
                    MyResult.MyDataTemp = convertdata;
                }
                else
                {
                    MyResult.success = false;
                    MyResult.message = "No Data Can Proceed";
                }

                return new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(MyResult);
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
                MyResult.success = false;
                MyResult.message = ex.Message.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);

            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendingMail(List<ListValidRequest> DataValidRequest)
        {
            sendingmail = false;
            SqlTransaction transaction11 = null;
            errmsg = "";
            try
            {
                string Userid = string.Empty;
                string password = string.Empty;
                string domain = string.Empty;
                string path = string.Empty;
                string URL = string.Empty;
                string MasterDB = string.Empty;
                string TransDB = string.Empty;

                string formatstatus = string.Empty;
                string remarks = string.Empty;
                string strdate = DateTime.Now.ToString("dd-MM-yyyy");

                if (DataValidRequest.Count > 0)
                {
                    for (int t = 0; t < DataValidRequest.Count; t++)
                    {
                        string ReqNumber = DataValidRequest[t].ReqNo;
                        string QuoteNoNew = DataValidRequest[t].QuoteNo;
                        string QuoteNoRef = DataValidRequest[t].QuoteNoRef;
                        string SAPPartCode = DataValidRequest[t].Material;
                        string SAPPartDesc = DataValidRequest[t].MaterialDesc;
                        string VendorCode1 = DataValidRequest[t].VendorCode1;
                        string VendorName = DataValidRequest[t].VendorName;
                        remarks = "Open Status";
                        //rowscount++;
                        DateTime QuoteDate = DataValidRequest[t].ResDueDate;

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
                        HttpPostedFile FlAttachment;
                        #region Uploading  ttachment to Mail sever using UNC credentials
                        fname = "";
                        if (Session["FlAtc" + QuoteNoRef] != null)
                        {
                            FlAttachment = (HttpPostedFile)Session["FlAtc" + QuoteNoRef];
                            if (FlAttachment.ContentLength > 0)
                            {
                                fname = FlAttachment.FileName;
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
                                            FlAttachment = (HttpPostedFile)Session["FlAtc" + QuoteNoRef];
                                            if (FlAttachment.ContentLength > 0)
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
                                vendorid.Value = VendorCode1;
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
                                errmsg += "Quote : " + QuoteNoNew + "function :getting User mail id " + "Msg Err :" + exc.Message.ToString() + "\n";
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
                                string body = "Dear Sir/Madam,<br /><br />Kindly be informed that a New request for Quotation <font color='red'> (Revision) </font> been created by Plant: " + Session["EPlant"].ToString() + " by " + Session["UserName"].ToString() + "<br /><br />The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + VendorName + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNoNew + "<br />  Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response Due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                body1 = "The details are<br /><br /> Plant : " + Session["EPlant"].ToString() + " <br />  Vendor Name  :   " + VendorName + "<br />  Request Number  :   " + ReqNumber + "<br />  Quote Number    :   " + QuoteNoNew + " <br /> Partcode And Description :   " + SAPPartCode + "  | " + SAPPartDesc + "<br />  Quotation Response due Date    :   " + QuoteDate.ToString("dd-MM-yyyy") + "<br /><br />" + footer;
                                string BodyFormat = "HTML";
                                string BodyRemark = "0";
                                string Signature = "";
                                string Importance = "High";
                                string Sensitivity = "Confidential";
                                string CreateUser = Session["UserName"].ToString();
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
                else
                {
                    errmsg += "Cannot Found Content For Mail \n";
                }

            }
            catch (Exception ex)
            {
                errmsg += "failed sending email from beginning process" + "Msg Err :" + ex.Message.ToString() + "\n";
            }

            if (errmsg != "")
            {
                GlobalResult MyResult = new GlobalResult();
                MyResult.success = false;
                MyResult.message = errmsg.ToString();
                return new JavaScriptSerializer().Serialize(MyResult);
            }
            else
            {
                GlobalResult MyResult = new GlobalResult();
                MyResult.success = true;
                MyResult.message = "Mail Content Submitted Successfully";
                return new JavaScriptSerializer().Serialize(MyResult);
            }
        }
    }
}
