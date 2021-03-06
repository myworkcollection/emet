using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

namespace Material_Evaluation
{
	public partial class login : System.Web.UI.Page
	{
        string sql;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlDataReader reader;
        bool IsAth;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userID"] = "";
            Session["userID_"] = "";
            if (Session["UserID"] != null)
            {
                //Response.Redirect("Home.aspx");
            }
            else
            {
                txtLogin.Focus();
            }

            //if (Request.QueryString["auth"] != "")
            //{
            //    string menu;
            //    menu = Request.QueryString["auth"];
            //    if (menu == "200")
            //    {
            //        menu = "90";
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Your Account dont have access for this page, please contact admin');", true);
            //    }

            //}
            if (!IsPostBack)
            {
                //txtLogin.Text = "";
                //if (txtLogin.Text != "")
                //{
                GetPlant();
                //}
            }

        }
        

        protected void GetPlant()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" select distinct plant as UseComCod from TGROUP where GroupID in(select distinct GroupID from TGROUPACCESS where system in(select distinct system from TUSER_AUTHORIZE where userid  = @UseID))";

                    sql = @" select distinct  tg.Plant as UseComCod from TUSER_AUTHORIZE tua inner join TGROUPACCESS tga on tua.GroupID = tga.GroupID inner join tgroup tg on tg.GroupID = tua.GroupID where tua.System = 'emet' and tua.UserID = @UseID and tua.DelFlag = 0";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@UseID", txtLogin.Text);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                for (int a = 0; a < dt.Rows.Count; a++)
                                {
                                    DllPlant.Items.Add(dt.Rows[a]["UseComCod"].ToString());
                                }
                            }
                            else
                            {
                                DllPlant.Items.Clear();
                                DllPlant.Items.Add("--Select Plant--");
                                for (int a = 0; a < dt.Rows.Count; a++)
                                {
                                    DllPlant.Items.Add(dt.Rows[a]["UseComCod"].ToString());
                                }
                            }
                        }
                        else
                        {
                            DllPlant.Items.Clear();
                            DllPlant.Items.Add("--Plant Not Exist--");
                            //DllPlant.Enabled = false;
                        }
                    }
                }
                //UpForm.Update();
            }
            catch (Exception ex)
            {
                //Response.Write(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                MDMCon.Dispose();
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
                Response.Write(ex);
            }
            finally
            {
                MDMCon.Dispose();
            }
            return IsTeamShimano;
        }

        bool CekUserPlant(string Plant)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            bool CekUserPlant = false;
            try
            {
                MDMCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    //sql = @" SELECT UseComCod FROM usr WHERE UseID = @UsrName  AND UseComCod = @Plant";
                    //sql = @" select * from TUSER_AUTHORIZE where GroupID in(select distinct GroupID from TGROUP where GroupID in(select distinct GroupID from TGROUPACCESS where system in (select distinct system from TUSER_AUTHORIZE where userid =@UsrName)) and plant=@Plant)and delflag=0";
                    sql = @"select * from TUSER_AUTHORIZE where userid =@UsrName and DelFlag = 0 and  GroupID in (select distinct GroupID from TGROUP where plant = @Plant)";
                    cmd = new SqlCommand(sql, MDMCon);
                    cmd.Parameters.AddWithValue("@UsrName", txtLogin.Text);
                    string plant = DllPlant.SelectedItem.ToString();
                    if (TxtDirectFromMDM.Text == "1")
                    {
                        plant = TxtPlant.Text;
                    }
                    cmd.Parameters.AddWithValue("@Plant", plant);
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            CekUserPlant = true;
                        }
                        else
                        {
                            CekUserPlant = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CekUserPlant = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                MDMCon.Close();
            }
            return CekUserPlant;
        }



        bool VndAnncmntUnread()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            bool UnreadAnn = false;
            try
            {
                EmetCon.Open();
                sql = @"select 
                        (select count (*) from tAnnouncement  where DelFlag=0 ) as 'TotContent',
                        (select count(*) from tAnnouncement A 
                        join tAnnReadBy B on A.id=B.id where B.UseID = @UseID and A.DelFlag=0 ) as 'Read' ";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int TotContent = (int)reader["TotContent"];
                        int Read = (int)reader["Read"];
                        int Unread = TotContent - Read;
                        if (Unread == 0)
                        {
                        }
                        else
                        {
                            UnreadAnn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session["UnreadAnn"] = "";
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return UnreadAnn;
        }

        protected void LoginButton_DoLogin_Click(object sender, EventArgs e)
        {
            string strUserName = txtLogin.Text;
            string strPassword = txtLoginpassword.Text;
            string plant = DllPlant.SelectedItem.ToString();
            if (TxtDirectFromMDM.Text == "1") {
                plant = TxtPlant.Text;
            }
            bool CUserPlant = false;
            try
            {
                if ((strUserName != "" && strPassword != "") || (strUserName != "" && TxtDirectFromMDM.Text == "1"))
                {
                    if (plant != "--Plant Not Exist--" && plant != "--Select Plant--" && plant != "")
                    {
                        SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
                        MDMCon.Open();
                        WindowsAuthentication su = new WindowsAuthentication();
                        //lblUserName.Text = "";
                        if (TxtDirectFromMDM.Text == "1")
                        {
                            sql = "SELECT U.UseID,U.UseNam, U.BusFunc, U.usedep, T.Description,U.UseComCod FROM usr U INNER JOIN TPLANT T ON U.UseComCod=T.PLANT WHERE u.UseID = @UsrName";
                            cmd = new SqlCommand(sql, MDMCon);
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@UsrName", strUserName);
                            reader = cmd.ExecuteReader();

                            if (!reader.HasRows)
                            {
                                lblUserName.Visible = true;
                                lblUserName.Text = "Invalid User ID and Password";
                                MDMCon.Dispose();
                            }
                            else
                            {
                                reader.Read();
                                Session["userID_"] = null;
                                Session["userID"] = reader.GetValue(0).ToString();
                                Session["UserName"] = reader.GetValue(1).ToString();
                                Session["userDept"] = reader.GetValue(3).ToString();
                                Session["GroupID"] = reader.GetValue(2).ToString();
                                Session["Plant"] = reader.GetValue(4).ToString();
                                Session["userType"] = reader.GetValue(4).ToString() + "-" + reader.GetValue(3).ToString();
                                //Session["EPlant"] = reader.GetValue(5).ToString();
                                Session["EPlant"] = plant;
                                Session["VPlant"] = null;
                                Session["SystemVersion"] = "( " + LbsystemVersion.Text + " )";

                                reader.Close();
                                MDMCon.Dispose();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "ShowLoading();", true);
                                Response.Redirect("Emet_author.aspx?num=1");
                            }
                        }
                        else if (!su.WinAuth(txtLogin.Text, txtLoginpassword.Text))
                        {
                            sql = "SELECT U.UseID,U.UseNam, U.BusFunc, U.usedep, T.Description, Uv.Vendor as mvendor, isnull(Uv.Vendor, U.UseID) as mappedvendor, isnull(substring((Uv.Description),1,14), 'Name Not Exist') as vname, U.UseComCod FROM usr U INNER JOIN TPLANT T ON U.UseComCod=T.PLANT left outer JOIN USERVSVENDOR UV on u.UseID=uv.UseID WHERE u.UseID = @UsrName AND PWDCOMPARE(@UsrPass, U.usepass) = 1";
                            cmd = new SqlCommand(sql, MDMCon);
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@UsrName", txtLogin.Text);
                            cmd.Parameters.AddWithValue("@UsrPass", txtLoginpassword.Text);
                            reader = cmd.ExecuteReader();

                            if (!reader.HasRows)
                            {

                                lblUserName.Visible = true;
                                lblUserName.Text = "Invalid User ID and Password";
                                MDMCon.Dispose();
                            }
                            else
                            {
                                CUserPlant = CekUserPlant(plant);

                                if (CUserPlant == true)
                                {
                                    reader.Read();
                                    Session["userID"] = null;
                                    Session["userID_"] = reader.GetValue(0).ToString();
                                    Session["UserName"] = reader.GetValue(1).ToString();
                                    Session["GroupID"] = reader.GetValue(2).ToString();
                                    Session["userType"] = "Vendor";
                                    //Session["VPlant"] = reader.GetValue(8).ToString();
                                    Session["VPlant"] = DllPlant.SelectedItem.ToString();
                                    Session["EPlant"] = null;
                                    Session["SystemVersion"] = "( " + LbsystemVersion.Text + " )";

                                    Session["mappedVendor"] = reader.GetValue(6).ToString();
                                    string mv = reader.GetValue(6).ToString();
                                    Session["mappedVname"] = reader.GetValue(7).ToString();
                                    string mvn = reader.GetValue(7).ToString();
                                    reader.Close();
                                    MDMCon.Dispose();
                                    if (IsTeamShimano(Session["mappedVendor"].ToString()) == false)
                                    {
                                        Session["VendorType"] = "External";
                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('External');", true);
                                    }
                                    else
                                    {
                                        Session["VendorType"] = "TeamShimano";
                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('TeamShimano');", true);
                                    }

                                    if (VndAnncmntUnread() == true)
                                    {
                                        string UI = Session["userID_"].ToString();
                                        string FN = "EMET_VendorAnnouncement";
                                        string PL = Session["VPlant"].ToString();
                                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                                        {
                                            FN = "EMET_VendorHome";
                                            if (EMETModule.IsAuthor(UI, FN, PL) == false)
                                            {
                                                Response.Redirect("RealTimeVendInv.aspx");
                                            }
                                            else
                                            {
                                                Response.Redirect("Vendor.aspx");
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect("Vannouncement.aspx");
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("Emet_author_V.aspx?num=15");
                                    }
                                }
                                else
                                {
                                    reader.Close();
                                    MDMCon.Dispose();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('This Plant Dont Register to Your ID, please contact Administrator');CloseLoading();", true);
                                }
                            }

                        }
                        else
                        {
                            sql = "SELECT U.UseID,U.UseNam, U.BusFunc, U.usedep, T.Description,U.UseComCod FROM usr U INNER JOIN TPLANT T ON U.UseComCod=T.PLANT WHERE u.UseID = @UsrName";
                            cmd = new SqlCommand(sql, MDMCon);
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@UsrName", txtLogin.Text);
                            reader = cmd.ExecuteReader();
                            if (!reader.HasRows)
                            {
                                MDMCon.Dispose();
                            }
                            else
                            {
                                CUserPlant = CekUserPlant(strPassword);
                                if (CUserPlant == true)
                                {
                                    reader.Read();
                                    Session["userID_"] = null;
                                    Session["userID"] = reader.GetValue(0).ToString();
                                    Session["UserName"] = reader.GetValue(1).ToString();
                                    Session["userDept"] = reader.GetValue(3).ToString();
                                    Session["GroupID"] = reader.GetValue(2).ToString();
                                    Session["Plant"] = reader.GetValue(4).ToString();
                                    Session["userType"] = reader.GetValue(4).ToString() + "-" + reader.GetValue(3).ToString();
                                    //Session["EPlant"] = reader.GetValue(5).ToString();
                                    Session["EPlant"] = DllPlant.SelectedItem.ToString();
                                    Session["VPlant"] = null;
                                    Session["SystemVersion"] = "( " + LbsystemVersion.Text + " )";

                                    reader.Close();
                                    MDMCon.Dispose();
                                    Response.Redirect("Emet_author.aspx?num=1");
                                }
                                else
                                {
                                    reader.Close();
                                    MDMCon.Dispose();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('This Plant Dont Register to Your ID, please contact Administrator');CloseLoading();", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (plant == "--Select Plant--")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select Plant');CloseLoading();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('This Plant Dont Register to Your ID, please contact Administrator');CloseLoading();", true);
                        }
                    }
                }
                else
                {
                    lblUserName.Visible = true;
                    lblUserName.Text = "Please Enter the Details";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "CloseLoading();", true);
            }//try
            catch (ThreadAbortException ex2)
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void isAuthor()
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            string sql;
            SqlDataReader reader;
            string FormName = "EMET_Home";
            string System = "EMET";
            try
            {
                MDMCon.Open();
                //sql = @"select * from TUSER_AUTHORIZE where UserID=@UserId and formname=@FormName and System=@System";
                sql = @" select * from TUSER_AUTHORIZE where GroupID in(select distinct GroupID from TGROUP where GroupID in(select distinct GroupID from TGROUPACCESS where system in (select distinct system from TUSER_AUTHORIZE where userid =@UsrName)) and plant=@Plant)and delflag=0";
                SqlCommand cmd = new SqlCommand(sql, MDMCon);
                cmd.Parameters.AddWithValue("@UserID", Session["userID"].ToString());
                cmd.Parameters.AddWithValue("@FormName", FormName);
                cmd.Parameters.AddWithValue("@System", System);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    IsAth = false;
                }
                else
                {
                    IsAth = true;
                }
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        public string Decrypt(string txtPassword)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(txtPassword);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool CheckUserName(string pstrUserLogin, string pstrCheckBy)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                if (pstrUserLogin != "")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    string str = "Select UseNam,UseEmail from usr where UPPER(" + pstrCheckBy + ") ='" + pstrUserLogin.Trim().ToUpper() + "' ";
                    da = new SqlDataAdapter(str, MDMCon);
                    dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Session["UserName"] = dt.Rows[0]["UseNam"].ToString();
                        Session["UseEmail"] = dt.Rows[0]["UseEmail"].ToString();
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        public void Insertpassword(string pstrUserLogin, string pstrUserNewPassword, string pstrCheckBy)
        {
            SqlConnection MDMCon = new SqlConnection(EMETModule.GenMDMConnString());
            try
            {
                string strEncPassWord = Encrypt(pstrUserNewPassword);

                MDMCon.Open();
                DataTable dtdate = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("update Usr set UsePass='" + strEncPassWord.Trim() + "' " + " where UPPER(" + pstrCheckBy + ") ='" + pstrUserLogin.Trim().ToUpper() + "'", MDMCon);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MDMCon.Dispose();
            }
        }

        public string Encrypt(string txtPassword)
        {
            try
            {
                byte[] encData_byte = new byte[txtPassword.Length - 1 + 1];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(txtPassword);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void CheckedChanged(object sender, EventArgs e)
        {
            if (chkremberme.Checked == true)
            {
                string strUserName = txtLogin.Text;
                string strpassword = txtLoginpassword.Text;
                lblUserName.Visible = true;
                if (strpassword == "" && strUserName == "")
                {
                    if (CheckUserName(strUserName, "UseID"))
                    {
                        Insertpassword(strUserName, strUserName, "UseID");
                        lblUserName.Text = "Password is sent to your Email ID.";
                    }
                }
                else
                {
                    lblUserName.Text = "The value in field login is required";
                }

            }

        }

        protected void txtLogin_TextChanged(object sender, EventArgs e)
        {
            DllPlant.Items.Clear();
            GetPlant();
            txtLoginpassword.Focus();
        }

    }
}