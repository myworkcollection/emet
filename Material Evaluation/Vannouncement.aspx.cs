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

namespace Material_Evaluation
{
    public partial class Vannouncement : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["userID_"] == null)
                {
                    Response.Redirect("Login.aspx?auth=200");
                }

                else
                {
                    if (Session["sidebarToggle"] == null)
                    {
                        SideBarMenu.Attributes.Add("style", "display:block;");
                    }
                    else
                    {
                        SideBarMenu.Attributes.Add("style", "display:none;");
                    }

                    if (!IsPostBack)
                    {
                        string UI = Session["userID_"].ToString();
                        string FN = "EMET_VendorAnnouncement";
                        string PL = Session["VPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author_V.aspx?num=0");
                        }
                        else
                        {
                            string userId = Session["userID_"].ToString();
                            string sname = Session["UserName"].ToString();
                            string srole = Session["userType"].ToString();
                            string concat = sname + " " + srole;
                            lblUser.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();

                            ShowTable();
                            NewOrRead();
                            CountRead();
                            EMETModule.RealTimeVendInvLastCheck(Session["userID_"].ToString());
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
        

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct id,Subject,isnull(format(UpdatedDate,'dd/MM/yyyy'), format(CreatedDate, 'dd/MM/yyyy'))as Date,
                            CONVERT(DateTime, CreatedDate,101)as Dt
                            from tAnnouncement
                            where DelFlag = 0 ";

                    if (txtFind.Text != "")
                    {
                        sql += @" and Subject like '%'+@Filter+'%' ";
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "Date")
                        {
                            sql += @" Order by CONVERT(DateTime, " + ViewState["SortExpression"].ToString() + ",101) " + ViewState["SortDirection"].ToString() + " ";
                        }
                        else
                        {
                            sql += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                        }
                    }
                    else
                    {
                        sql += @" Order by id desc ";
                    }

                    cmd = new SqlCommand(sql, EmetCon);
                    if (txtFind.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Filter", txtFind.Text);
                    }
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            int Record = dt.Rows.Count;
                            LbTtlRecords.Text = "Total Record : " + Record.ToString();
                        }
                        else
                        {
                            LbTtlRecords.Text = "Total Record : 0";
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

        protected void NewOrRead()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select id,UseID from tAnnReadBy where UseID=@UseID";
                    cmd = new SqlCommand(sql, EmetCon);
                    cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            for (int d = 0; d < dt.Rows.Count; d++)
                            {
                                for (int i = 0; i < GridView1.Rows.Count; i++)
                                {
                                    GridViewRow row = GridView1.Rows[i];
                                    HiddenField theHiddenField = row.FindControl("HiddenId") as HiddenField;
                                    HiddenField HiddenStatus = row.FindControl("HiddenStatus") as HiddenField;
                                    LinkButton LbSubject = row.FindControl("LbSubject") as LinkButton;
                                    if (HiddenStatus.Value == "")
                                    {
                                        if (theHiddenField.Value == dt.Rows[d]["id"].ToString())
                                        {
                                            LbSubject.Font.Bold = false;
                                            LbSubject.ForeColor = System.Drawing.Color.Black;
                                            GridView1.Rows[i].Cells[1].Font.Bold = false;
                                            GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Black;
                                            HiddenStatus.Value = "Read";
                                        }
                                        else
                                        {
                                            LbSubject.Font.Bold = true;
                                            LbSubject.ForeColor = System.Drawing.Color.Blue;
                                            GridView1.Rows[i].Cells[1].Font.Bold = true;
                                            GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Blue;
                                            HiddenStatus.Value = "UnRead";
                                        }
                                    }
                                    else
                                    {
                                        if (HiddenStatus.Value == "UnRead")
                                        {
                                            if (theHiddenField.Value == dt.Rows[d]["id"].ToString())
                                            {
                                                LbSubject.Font.Bold = false;
                                                LbSubject.ForeColor = System.Drawing.Color.Black;
                                                GridView1.Rows[i].Cells[1].Font.Bold = false;
                                                GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Black;
                                                HiddenStatus.Value = "Read";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (GridView1.Rows.Count > 0)
                            {
                                for (int i = 0; i < GridView1.Rows.Count; i++)
                                {
                                    GridViewRow row = GridView1.Rows[i];
                                    LinkButton LbSubject = row.FindControl("LbSubject") as LinkButton;
                                    LbSubject.Font.Bold = true;
                                    LbSubject.ForeColor = System.Drawing.Color.Blue;
                                    GridView1.Rows[i].Cells[1].Font.Bold = true;
                                    GridView1.Rows[i].Cells[1].ForeColor = System.Drawing.Color.Blue;
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
                EmetCon.Dispose();
            }
        }

        protected void CountRead()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"select count(*) id from tAnnouncement A 
                        join tAnnReadBy B on A.id=B.id where B.UseID = @UseID and A.DelFlag=0 ";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int Read = (int)reader["id"];
                        int TotRecord = int.Parse(LbTtlRecords.Text.Replace("Total Record : ", ""));
                        int Unread = TotRecord - Read;

                        LbRead.Text = "Read : " + Read.ToString();
                        LbUnread.Text = "UnRead : " + Unread.ToString();

                        if (Unread == 0)
                        {
                            Session["UnreadAnn"] = "";
                        }
                        else
                        {
                            Session["UnreadAnn"] = Unread;
                        }
                    }
                }
                else
                {
                    Session["UnreadAnn"] = "";
                }
            }
            catch (Exception ex)
            {
                Session["UnreadAnn"] = "";
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        bool CekExist(string id)
        {
            bool Exist = true;
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"select distinct UseID from tAnnReadBy where id=@id and UseID=@UseID";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Exist = false;
                }
            }
            catch (Exception ex)
            {
                Exist = false;
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return Exist;
        }

        protected void UpdateStatusRead(string Id)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                sql = @"insert into tAnnReadBy ([id],[UseID]) 
                            values (@id,@UseID)";
                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@UseID", Session["userID_"].ToString());
                cmd.ExecuteNonQuery();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowTable();
            NewOrRead();
            CountRead();
            txtFind.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        }

        protected void BtnCloseCntnt_Click(object sender, EventArgs e)
        {
            DvFilter.Visible = true;
            DvSubjectList.Visible = true;
            DvLbTotRecord.Visible = true;
            DvContent.Visible = false;
            LbSubject.Text = "";
            TxtContent.Text = "";
            ShowTable();
            NewOrRead();
            CountRead();
            txtFind.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                ShowTable();
                NewOrRead();
                CountRead();
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            if (e.CommandName == "CRead")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];
                HiddenField theHiddenField = row.FindControl("HiddenId") as HiddenField;
                string Id = theHiddenField.Value.ToString();
                try
                {
                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select Subject,ContentAnnc,PathAndFileName from tAnnouncement where Id = @Id ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DvFilter.Visible = false;
                                DvSubjectList.Visible = false;
                                DvLbTotRecord.Visible = false;
                                DvContent.Visible = true;
                                LbSubject.Text = dt.Rows[0]["Subject"].ToString();
                                TxtContent.Text = dt.Rows[0]["ContentAnnc"].ToString();
                                TxtFilePath.Text = dt.Rows[0]["PathAndFileName"].ToString();
                                string FullFileName = dt.Rows[0]["PathAndFileName"].ToString();
                                if (FullFileName == "")
                                {
                                    LblFileName.Text = "No Attachment";
                                    LblFileName.ForeColor = System.Drawing.Color.Red;
                                    LblFileName.Font.Bold = true;
                                    DvdAttachmanetOld.Style.Add("display", "none");
                                }
                                else
                                {
                                    string FileName = FullFileName.Substring(14);
                                    LblFileName.Text = FileName;
                                    LblFileName.ForeColor = System.Drawing.Color.Black;
                                    LblFileName.Font.Bold = false;
                                    DvdAttachmanetOld.Style.Add("display", "block");
                                }


                                if (CekExist(Id) == false)
                                {
                                    UpdateStatusRead(Id);
                                }
                            }
                            else
                            {
                                DvdAttachmanetOld.Style.Add("display", "none");
                                DvFilter.Visible = true;
                                DvSubjectList.Visible = true;
                                DvLbTotRecord.Visible = true;
                                DvContent.Visible = false;
                                LbSubject.Text = "";
                                TxtContent.Text = "";
                                TxtFilePath.Text = "";
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
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GetSortDirection(e.SortExpression);
                ShowTable();
                NewOrRead();
                CountRead();
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
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }

            return sortDirection;
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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

        protected void LBtnDownloadOld_Click(object sender, EventArgs e)
        {
            string FullFileName = TxtFilePath.Text;
            string FileExtension = "";
            string folderPath = Server.MapPath("~/Files/attachment/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string[] ArrFullFileName = FullFileName.Split('.');
            string NewfileName = "";
            if (ArrFullFileName.Count() > 0)
            {
                for (int a = 0; a < (ArrFullFileName.Count() - 1); a++)
                {
                    NewfileName += ArrFullFileName[a] + ".";
                }

                NewfileName = NewfileName.Substring(14);
                NewfileName = NewfileName.Remove(NewfileName.Length - 1);
                FileExtension = ArrFullFileName[(ArrFullFileName.Count() - 1)].ToString();
            }
            else
            {
                NewfileName = "";
                FileExtension = "";
            }

            if (NewfileName == "" || string.IsNullOrEmpty(NewfileName))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('No attachment to download !');openModal();", true);
            }

            else
            {
                FileInfo file = new FileInfo(folderPath + FullFileName);
                if (file.Exists) //check file exsit or not  
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/" + FileExtension.Replace(".", "") + "";
                    Response.AddHeader("content-disposition", "attachment;filename=" + (NewfileName + "." + FileExtension));     // to open file prompt Box open or Save file         
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.TransmitFile(folderPath + FullFileName);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('File Not Found !');openModal();", true);
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
            catch (ThreadAbortException exx)
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