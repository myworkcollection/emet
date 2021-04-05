using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using context = System.Web.HttpContext;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Material_Evaluation
{
    public class EMETModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }

        static DataTable dt;
        static SqlCommand cmd;
        static string sql;
        static SqlDataReader reader;
        private static String exepurl;

        public static bool IsAuthor(string UserId, string FormName, string Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct tua.* 
                        from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID=tga.GroupID 
                        inner join tgroup tg on tg.GroupID=tua.GroupID 
                        where tua.System=@System 
                        and tua.UserID=@UserId 
                        and tua.FormName=@FormName 
                        and tua.DelFlag=0 
                        and tg.Plant=@Plant ";
                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@System", "EMET");
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@Plant", Plant);
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

        public static bool isRltInv(string Plant, string VndCode)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                sql = @"select distinct V.Vendor, O.Plant
                from tVendor_New V 
                join tVendorPOrg O on V.POrg = O.POrg and V.Vendor=O.Vendor
                where isRltInv = 1 and isnull(V.DelFlag,0) = 0 and ISNULL(O.DelFlag,0) = 0
                and V.Vendor = @Vendor and O.Plant = @Plant ";
                cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@Plant", Plant);
                cmd.Parameters.AddWithValue("@Vendor", VndCode);
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

        public static bool IsSubmit(string QuoteNo)
        {
            SqlConnection EMETCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EMETCon.Open();
                sql = @"select distinct ISNULL(FinalQuotePrice,'') as FinalQuotePrice from TQuoteDetails where QuoteNo=@QuoteNo";
                cmd = new SqlCommand(sql, EMETCon);
                cmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string FinalQuoteVal = reader.GetValue(0).ToString();
                    reader.Dispose();

                    if (FinalQuoteVal == "") {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
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
                EMETCon.Dispose();
            }
        }

        public static void SendExcepToDB(Exception exdb)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                exepurl = context.Current.Request.Url.ToString();

                sql = "Insert into TERRORLOG(ErrDescription ,ErrType, ErrSource,ErrUrl,AddedBy,AddedOn) values (@ErrDescription ,@ErrType,@ErrSource,@ErrUrl,@AddedBy,CURRENT_TIMESTAMP) ";
                //sql = "";
                //SqlCommand com = new SqlCommand("Sp_ExToDb", contr);
                //com.CommandType = CommandType.StoredProcedure;
                SqlCommand com = new SqlCommand(sql, EmetCon);
                com.Parameters.AddWithValue("@ErrDescription", exdb.Message.ToString());
                com.Parameters.AddWithValue("@ErrType", exdb.GetType().Name.ToString());
                com.Parameters.AddWithValue("@ErrUrl", exepurl);
                com.Parameters.AddWithValue("@ErrSource", exdb.StackTrace.ToString());
                if (context.Current.Session["userID_"] != null)
                {
                    com.Parameters.AddWithValue("@AddedBy", context.Current.Session["userID_"].ToString());
                }
                else
                {
                    com.Parameters.AddWithValue("@AddedBy", context.Current.Session["userID"].ToString());
                }
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        public static string GetLayoutBaseOnprocGroup(string ProcGrp)
        {
            string Layout = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct ScreenLayout from TPROCESGRP_SCREENLAYOUT where ISNULL(DelFlag,0) = 0 and ProcessGrp=@ProcGrp ";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@ProcGrp", ProcGrp);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Layout = dt.Rows[0]["ScreenLayout"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
            return Layout;
        }

        #region auto 

        public static void RealTimeVendInvLastCheck(string useId)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string DbMasterName = GetDbMastername();
                EmetCon.Open();
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable dt = new DataTable();
                sql = @" declare @UpdtData nvarchar(1) = '0'
                    --declare @UserID nvarchar (20);
                    declare @lastdateupdate date = (SELECT distinct top 1  FORMAT(LastCheckDate,'yyyy-MM-dd') FROM tLastCheck WHERE Remark = 'Realtime_Inv_Data')

                    IF(EXISTS(SELECT Remark, LastCheckDate, LastCheckBy FROM tLastCheck WHERE Remark = 'Realtime_Inv_Data'))
                    BEGIN
	                    IF(FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd') < (DATEADD(WEEK, 4, (@lastdateupdate))))
	                    BEGIN
		                    set @UpdtData = '0';
	                    END
	                    ELSE
	                    BEGIN
		                    set @UpdtData = '1';
	                    END
 
	                    IF(@UpdtData = '1')
	                    BEGIN
		                    DELETE FROM TRealTimeVendorInventory WHERE CreatedDate < DATEADD(week,-(select CONVERT (int, IDValue) FROM " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Data(Weeks)'),convert(date, (SELECT TOP 1 CreatedDate FROM TRealTimeVendorInventory ORDER BY CreatedDate DESC), 104))
		                    UPDATE tLastCheck set LastCheckDate = CURRENT_TIMESTAMP, LastCheckBy = @UserID
	                    END
                    END
                    ELSE
                    BEGIN
	                    INSERT INTO tLastCheck (Remark, LastCheckBy, LastCheckDate) VALUES ('Realtime_Inv_Data', @UserID, CURRENT_TIMESTAMP)	
	                    DELETE FROM TRealTimeVendorInventory WHERE CreatedDate < DATEADD(week,-(select CONVERT (int, IDValue) FROM " + DbMasterName + @".dbo.tGlobal WHERE ID='Realtime_Inv_Data(Weeks)'),convert(date, (SELECT TOP 1 CreatedDate FROM TRealTimeVendorInventory ORDER BY CreatedDate DESC), 104))
	                    UPDATE tLastCheck set LastCheckDate = CURRENT_TIMESTAMP, LastCheckBy = @UserID
                    END ";
                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UserID", useId);
                cmd.CommandTimeout = 0;
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

        public static string GetDataDayTolerance(string Plant)
        {
            string DayTolerance = "";
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                string Sql = "select distinct DefValue from DefaultValueMaster where Description='Expired Request to close (Day)' and Plant=@Plant ";
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd = new SqlCommand(Sql, MDMCon);
                    cmd.Parameters.AddWithValue("@Plant",Plant);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DayTolerance = dt.Rows[0]["DefValue"].ToString();
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
                cmd.Dispose();
                MDMCon.Dispose();
            }
            return DayTolerance;
        }

        /// <summary>
        /// Auto Closed request for mass revision 
        /// </summary>
        /// <param name="Plant"></param>
        public static void CekAndCloseExpiredRequest(string Plant)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string DayTolerance = GetDataDayTolerance(Plant);
                if (DayTolerance != "")
                {
                    EmetCon.Open();
                    string Sql = "";

                    Sql += @"declare @c int = (select count(*) from TQuoteDetails_D where isMassRevision = '1'
                                           and format(DATEADD(DAY," + DayTolerance + @",QuoteResponseDueDate), 'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP, 'yyyy-MM-dd') 
                                            and IsReSubmit <> 1  );
                        declare @i int = 0;
                        declare @quoteNo nvarchar(50);
                        WHILE @i < @c
                        BEGIN
	                        set @quoteNo = (select top 1 QuoteNo from TQuoteDetails_D where isMassRevision = '1' 
                                            and format(DATEADD(DAY," + DayTolerance + @",QuoteResponseDueDate), 'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP, 'yyyy-MM-dd') );

	                        update TQuoteDetails set 
                            PIRStatus='V',
	                        TotalMaterialCost = (select TotalMaterialCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        TotalSubMaterialCost = (select TotalSubMaterialCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        TotalProcessCost = (select TotalProcessCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        TotalOtheritemsCost = (select TotalOtheritemsCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        GrandTotalCost = (select GrandTotalCost from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        FinalQuotePrice = (select FinalQuotePrice from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        Profit = (select Profit from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        Discount = (select Discount from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        UpdatedBy = 'EMET',
	                        UpdatedOn = CURRENT_TIMESTAMP,
	                        CountryOrg = (select CountryOrg from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        CommentByVendor = (select CommentByVendor from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        EmpSubmitionOn = (select EmpSubmitionOn from TQuoteDetails_D where QuoteNo=@QuoteNo),
	                        EmpSubmitionBy = (select EmpSubmitionBy from TQuoteDetails_D where QuoteNo=@QuoteNo)
	                        where QuoteNo=@QuoteNo

	                        delete from TMCCostDetails where QuoteNo=@QuoteNo
	                        delete from TOtherCostDetails where QuoteNo=@QuoteNo
	                        delete from TProcessCostDetails where QuoteNo=@QuoteNo
	                        delete from TSMCCostDetails where QuoteNo=@QuoteNo

	                        INSERT INTO TMCCostDetails SELECT * FROM TMCCostDetails_D WHERE QuoteNo=@QuoteNo;
	                        INSERT INTO TOtherCostDetails SELECT * FROM TOtherCostDetails_D WHERE QuoteNo=@QuoteNo;
	                        INSERT INTO TProcessCostDetails SELECT * FROM TProcessCostDetails_D WHERE QuoteNo=@QuoteNo;
	                        INSERT INTO TSMCCostDetails SELECT * FROM TSMCCostDetails_D WHERE QuoteNo=@QuoteNo;

	                        delete from TMCCostDetails_D where QuoteNo=@QuoteNo
	                        delete from TOtherCostDetails_D where QuoteNo=@QuoteNo
	                        delete from TProcessCostDetails_D where QuoteNo=@QuoteNo
	                        delete from TSMCCostDetails_D where QuoteNo=@QuoteNo
	                        delete from TQuoteDetails_D where QuoteNo=@QuoteNo
	                        set @i = @i+1;
                        END;
                        ";


                    Sql += @" Update TQuoteDetails SET ApprovalStatus='1', PICApprovalStatus = '1',ManagerApprovalStatus= '1', DIRApprovalStatus= '1',  
                          UpdatedBy='EMET', UpdatedOn=CURRENT_TIMESTAMP 
                          where format(DATEADD(DAY," + DayTolerance + @",QuoteResponseDueDate), 'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP, 'yyyy-MM-dd')
                           and ApprovalStatus='0' and  IsReSubmit <> 1 and isMassRevision = 1
                        ";

                    cmd = new SqlCommand(Sql, EmetCon);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                cmd.Dispose();
                EmetCon.Dispose();
            }
        }

        /// <summary>
        /// Auto Update Request To Show Data if All Vendor Submit
        /// </summary>
        public static void AutoUpdateRequest()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string query = @" SELECT RequestNumber,PIRStatus,QuoteNo,FinalQuotePrice into #temp FROM TQuoteDetails 
                where PIRStatus is null group by RequestNumber,PIRStatus,QuoteNo,FinalQuotePrice

                select RequestNumber into #temp1 from #temp GROUP BY RequestNumber --HAVING COUNT(*)>1

                select A.RequestNumber,PIRStatus,QuoteNo,FinalQuotePrice into #temp2
                from #temp A join #temp1 B on A.RequestNumber =B.RequestNumber 
                where FinalQuotePrice is not null

                select A.RequestNumber,PIRStatus,QuoteNo,FinalQuotePrice into #temp3
                from #temp A join #temp1 B on A.RequestNumber =B.RequestNumber 
                where FinalQuotePrice is null

                select * into #temp4
                from #temp2 where RequestNumber not in (select RequestNumber from #temp3) 

                update TQuoteDetails set PIRStatus='V' where RequestNumber in (select RequestNumber from #temp4)

                drop table #temp,#temp1,#temp2,#temp3,#temp4 ";
                cmd = new SqlCommand(query, EmetCon);
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

        /// <summary>
        /// Update request to make the quotation value visible after reach time expired and day tolerance except mass revision
        /// Update request to make the quotation become auto close after reach time expired and day tolerance except mass revision and no submission from vendor
        /// </summary>
        public static void autoupdate(string Plant)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string DayTolerance = GetDataDayTolerance(Plant);
                EmetCon.Open();
                string query = "";
                if (DayTolerance == "") {
                    DayTolerance = "0";
                }

                ///auto closed request vendor not resposnse when request expired and 1 of vendor is submited
                query = @" --collect request no have quote no expired not submit and other quote no is submited
                        select distinct RequestNumber into #tempReqNo from TQuoteDetails 
                        where FORMAT(QuoteResponseDueDate,'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd')
                        and isnull(PIRStatus,'') = '' and ApprovalStatus=2  and  isnull(IsReSubmit,0) <> 1 and isnull(isMassRevision,0) = 0

                        --collect quote no not submited 
                        select QuoteNo into #tempQuoteNoExpiredNotSubmit from TQuoteDetails 
                        where ApprovalStatus = 0 and isnull(FinalQuotePrice,'') = '' and RequestNumber in (select A.RequestNumber from #tempReqNo A)

                        --collect all request expired and no vendor submit
						select distinct RequestNumber into #tempReqNoOneSubmit from TQuoteDetails 
						where format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP,'yyyy-MM-dd')
						and ApprovalStatus=0 and isnull(FinalQuotePrice,'') = ''  and  isnull(IsReSubmit,0) <> 1 and isnull(isMassRevision,0) = 0
						and RequestNumber not in (select RequestNumber from #tempReqNo)

                        --closed quote no not submited
                        update TQuoteDetails SET ApprovalStatus='2', PICApprovalStatus = '0', UpdatedBy='EMET', UpdatedOn=CURRENT_TIMESTAMP
                        where QuoteNo in (select B.QuoteNo from #tempQuoteNoExpiredNotSubmit B) 
                        --display all quote value
                        update TQuoteDetails SET PIRStatus='V'
                        where RequestNumber in (select C.RequestNumber from #tempReqNo C) 

                        --closed req no not submited
						update TQuoteDetails SET PIRStatus='V',ApprovalStatus='1',PICApprovalStatus='1',ManagerApprovalStatus='1',DIRApprovalStatus='1', UpdatedBy='EMET', UpdatedOn=CURRENT_TIMESTAMP
						where RequestNumber in (select C.RequestNumber from #tempReqNoOneSubmit C) 

                        drop table #tempReqNo,#tempQuoteNoExpiredNotSubmit,#tempReqNoOneSubmit ";
                cmd = new SqlCommand(query, EmetCon);
                cmd.ExecuteNonQuery();


                /// Update request to make the quotation value visible after reach time expired and day tolerance except mass revision
                query = @"update TQuoteDetails set PIRStatus='V' 
                                where  isMassRevision <> '1' 
                                and format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < format(getdate(),'yyyy-MM-dd')
                                AND PIRStatus IS NULL ";
                cmd = new SqlCommand(query, EmetCon);
                cmd.ExecuteNonQuery();

                /// Update request to make the quotation become auto close after reach time expired and day tolerance except mass revision and no submission from vendor
                query = @"update TQuoteDetails set ApprovalStatus='2', PICApprovalStatus='0' 
                                where ApprovalStatus=0 and picapprovalstatus=0 
                                and format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < format(getdate(),'yyyy-MM-dd')
                                and isMassRevision <> '1' and  IsReSubmit <> 1 ";

                cmd = new SqlCommand(query, EmetCon);
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


        /// <summary>
        /// auto closed expired request and dislpay all submission value by vendor 
        /// </summary>
        public static void autoCloseWhitoutsapCode(string Plant)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                string DayTolerance = GetDataDayTolerance(Plant);
                EmetCon.Open();
                string query = "";
                query = @"update TQuoteDetails set PIRStatus='V' 
                                where isMassRevision <> '1' 
                                and format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < format(getdate(),'yyyy-MM-dd') 
                                and right(QuoteNo, 1) = 'D'
                                AND PIRStatus IS NULL ";

                query += @"update TQuoteDetails set PIRStatus='V' 
                                where isMassRevision <> '1' 
                                and format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < format(getdate(),'yyyy-MM-dd') 
                                and  right(QuoteNo, 2) = 'GP'
                                AND PIRStatus IS NULL ";

                query += @" update  TQuoteDetails  set ApprovalStatus='6',UpdatedBy='EMET', UpdatedOn=CURRENT_TIMESTAMP
                               where format(DATEADD(day," + DayTolerance + @",QuoteResponseDueDate),'yyyy-MM-dd') < FORMAT(CURRENT_TIMESTAMP, 'yyyy-MM-dd') 
                               and ApprovalStatus=4 ";

                cmd = new SqlCommand(query, EmetCon);
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

        public static string GetDbMastername()
        {
            string DbMaster = "";
            try
            {
                DbMaster = Decrypt(Properties.Settings.Default.MdmDb.ToString());
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            return DbMaster;
        }

        public static string GetDbTransname()
        {
            string DbTrans = "";
            try
            {
                DbTrans = Decrypt(Properties.Settings.Default.EmetDb.ToString());
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            return DbTrans;
        }

        public static string GenMDMConnString()
        {
            string connectionString = "";
            try
            {
                string Constr = "";
                string Db = "";
                string UserId = "";
                string Password = "";

                Constr = Decrypt(Properties.Settings.Default.MdmConString.ToString());
                Db = Decrypt(Properties.Settings.Default.MdmDb.ToString());
                UserId = Decrypt(Properties.Settings.Default.MdmuseId.ToString());
                Password = Decrypt(Properties.Settings.Default.MdmPass.ToString());

                connectionString = "Data Source=" + Constr + ";Initial Catalog=" + Db + ";User Id=" + UserId + "; Password=" + Password + ";";
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            return connectionString;
        }

        public static string GenEMETConnString()
        {
            string connectionString = "";
            try
            {
                string Constr = "";
                string Db = "";
                string UserId = "";
                string Password = "";

                Constr = Decrypt(Properties.Settings.Default.EmetConString.ToString());
                Db = Decrypt(Properties.Settings.Default.EmetDb.ToString());
                UserId = Decrypt(Properties.Settings.Default.EmetuseId.ToString());
                Password = Decrypt(Properties.Settings.Default.EmetPass.ToString());

                connectionString = "Data Source=" + Constr + ";Initial Catalog=" + Db + ";User Id=" + UserId + "; Password=" + Password + ";";
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            return connectionString;
        }

        public static string GenMailConnString()
        {
            string connectionString = "";
            try
            {
                string Constr = "";
                string Db = "";
                string UserId = "";
                string Password = "";

                Constr = Decrypt(Properties.Settings.Default.MailConString.ToString());
                Db = Decrypt(Properties.Settings.Default.MailDb.ToString());
                UserId = Decrypt(Properties.Settings.Default.MailuseId.ToString());
                Password = Decrypt(Properties.Settings.Default.MailPass.ToString());

                connectionString = "Data Source=" + Constr + ";Initial Catalog=" + Db + ";User Id=" + UserId + "; Password=" + Password + ";";
            }
            catch (Exception ex)
            {
                SendExcepToDB(ex);
            }
            return connectionString;
        }

        public static string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion
    }
}
