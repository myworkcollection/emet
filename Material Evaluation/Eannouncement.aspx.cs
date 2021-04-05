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
    public partial class Eannouncement : System.Web.UI.Page
    {
        string sql;
        SqlCommand cmd;
        SqlDataReader reader;
        DataSet ds = new DataSet();
        string DbMasterName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userID"] == null)
            {
                Response.Redirect("Login.aspx?auth=200");
            }
            else
            {
                if (!IsPostBack)
                {
                    string UI = Session["userID"].ToString();
                    string FN = "EMET_Announcement";
                    string PL = Session["EPlant"].ToString();
                    if (EMETModule.IsAuthor(UI, FN, PL) == false)
                    {
                        Response.Redirect("Emet_author.aspx?num=0");
                    }
                    else
                    {
                        string userId = Session["userID"].ToString();
                        string sname = Session["UserName"].ToString();
                        string srole = Session["userType"].ToString();
                        string concat = sname + " " + srole;
                        lblUser.Text = sname;
                        lblplant.Text = srole;
                        LbsystemVersion.Text = Session["SystemVersion"].ToString();
                        if (Session["ShowEntryEannouncement"] != null)
                        {
                            TxtShowEntry.Text = Session["ShowEntryEannouncement"].ToString();
                        }
                        ShowTable();

                        if (Session["sidebarToggle"] == null)
                        {
                            SideBarMenu.Attributes.Add("style", "display:block;");
                        }
                        else
                        {
                            SideBarMenu.Attributes.Add("style", "display:none;");
                        }
                        EMETModule.RealTimeVendInvLastCheck(Session["userID"].ToString());
                    }
                }
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
                Response.Write(ex);
                DbMasterName = "";
            }
        }

        protected void ShowTable()
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                GetDbMaster();
                EmetCon.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    sql = @" select distinct id,Subject,
                            CASE
	                            WHEN (select len(ContentAnnc) from tAnnouncement where id=A.id) <= 50 THEN ContentAnnc
                                ELSE substring((ContentAnnc),1,70) +' ...'
                            END as 'ContentAnnc',
                            (select UseNam from "+ DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) as 'CreatedBy',
                            format(CreatedDate,'dd/MM/yyyy') as CreatedDate,
                            CreatedAt,
                            (select UseNam from " + DbMasterName + @".[dbo].Usr where UseID=A.UpdatedBy) as 'UpdatedBy',
                            format(UpdatedDate,'dd/MM/yyyy') as UpdatedDate,UpdatedAt,
                            CONVERT(DateTime, UpdatedDate,101) as 'UpDate',CONVERT(DateTime, CreatedDate,101)as 'CrDate'
                            from tAnnouncement A where DelFlag = 0 ";

                    if (txtFind.Text != "")
                    {
                        if (DdlFilterBy.SelectedValue.ToString() == "Subject")
                        {
                            sql += @" and Subject like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "Content")
                        {
                            sql += @" and ContentAnnc like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "CreatedBy")
                        {
                            sql += @" and (select UseNam from " + DbMasterName + @".[dbo].Usr where UseID=A.CreatedBy) like '%'+@Filter+'%' ";
                        }
                        else if (DdlFilterBy.SelectedValue.ToString() == "UpdatedBy")
                        {
                            sql += @" and (select UseNam from " + DbMasterName + @".[dbo].Usr where UseID=A.UpdatedBy) like '%'+@Filter+'%' ";
                        }
                    }

                    if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                    {
                        if (ViewState["SortExpression"].ToString() == "CreatedDate" || ViewState["SortExpression"].ToString() == "UpdatedDate")
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
                        int ShowEntry = 1;
                        if (TxtShowEntry.Text == "" || TxtShowEntry.Text == "0")
                        {
                            ShowEntry = 1;
                            TxtShowEntry.Text = "1";
                        }
                        else
                        {
                            ShowEntry = Convert.ToInt32(TxtShowEntry.Text);
                        }
                        GridView1.PageSize = ShowEntry;
                        Session["ShowEntryEannouncement"] = ShowEntry.ToString();
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
                //Response.Write(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
        }

        string GetSubject(string Id)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            string subject = "";
            try
            {
                EmetCon.Open();
                sql = @"select distinct Subject from tAnnouncement where id=@id";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Subject", subject);
                cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    while (reader.Read())
                    {
                        subject = reader.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                subject = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return subject;
        }

        string GetFilePathAndName(string Id)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            string PathAndFileName = "";
            try
            {
                EmetCon.Open();
                sql = @"select distinct PathAndFileName from tAnnouncement where id=@id";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Id", Id);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PathAndFileName = Server.MapPath("~/Files/attachment/") + reader[0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                PathAndFileName = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return PathAndFileName;
        }

        bool CekExistSubject(string subject)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            
            bool Exist = true;
            try
            {
                EmetCon.Open();
                sql = @"select distinct Subject from tAnnouncement 
                            where Subject=@Subject";

                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Subject", subject);
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Exist = false;
                }
            }
            catch (Exception ex)
            {
                Exist = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert(" + ex + ");", true);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return Exist;
        }

        bool IUD(string Command)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            bool success = false;
            try
            {
                string NewFileName = "";
                string OldPathAndFileName = GetFilePathAndName(TxtId.Text);
                if (FuAttachment.HasFile)
                {
                    string folderPath = Server.MapPath("~/Files/attachment/");
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists Create it.
                        Directory.CreateDirectory(folderPath);
                    }

                    string FileExtension = System.IO.Path.GetExtension(FuAttachment.FileName);
                    string filename = System.IO.Path.GetFileName(FuAttachment.FileName);
                    NewFileName = DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
                    FuAttachment.SaveAs(folderPath + NewFileName);
                }
                EmetCon.Open();
                if (Command == "ADD")
                {
                    sql = @"insert into tAnnouncement (Subject,ContentAnnc,CreatedBy,CreatedDate,CreatedAt,PathAndFileName) 
                            values (@Subject,@ContentAnnc,@CreatedBy,CURRENT_TIMESTAMP,@CreatedAt,@PathAndFileName)";
                }
                else if (Command == "UPDATE")
                {
                    if (FuAttachment.HasFile)
                    {
                        if (OldPathAndFileName != "")
                        {
                            FileInfo file = new FileInfo(OldPathAndFileName);
                            if (file.Exists) //check file exsit or not  
                            {
                                file.Delete();
                            }
                        }

                        sql = @"Update tAnnouncement set ContentAnnc = @ContentAnnc, UpdatedBy=@CreatedBy,UpdatedDate=CURRENT_TIMESTAMP,UpdatedAt=@CreatedAt,PathAndFileName=@PathAndFileName where Id=@Id
                            delete from tAnnReadBy where id=@Id";
                    }
                    else
                    {
                        if (ChkDeleteAttachment.Checked == true)
                        {
                            FileInfo file = new FileInfo(OldPathAndFileName);
                            if (file.Exists) //check file exsit or not  
                            {
                                file.Delete();
                            }
                            sql = @"Update tAnnouncement set ContentAnnc = @ContentAnnc, UpdatedBy=@CreatedBy,UpdatedDate=CURRENT_TIMESTAMP,UpdatedAt=@CreatedAt,PathAndFileName=@PathAndFileName where Id=@Id
                            delete from tAnnReadBy where id=@Id";
                        }
                        else
                        {
                            sql = @"Update tAnnouncement set ContentAnnc = @ContentAnnc, UpdatedBy=@CreatedBy,UpdatedDate=CURRENT_TIMESTAMP,UpdatedAt=@CreatedAt where Id=@Id
                            delete from tAnnReadBy where id=@Id";
                        }
                    }
                }
                else if (Command == "DELETE")
                {
                    sql = @"Update tAnnouncement set UpdatedBy=@CreatedBy,UpdatedDate=CURRENT_TIMESTAMP,UpdatedAt=@CreatedAt,DelFlag=1 where Id=@Id
                            delete from tAnnReadBy where id=@Id";
                }
                cmd = new SqlCommand(sql, EmetCon);
                cmd.Parameters.AddWithValue("@Id", TxtId.Text);
                cmd.Parameters.AddWithValue("@Subject", TxtSubject.Text.Trim());
                cmd.Parameters.AddWithValue("@ContentAnnc", TxtContent.Text);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["userID"].ToString());
                cmd.Parameters.AddWithValue("@CreatedAt", Environment.MachineName.ToString());
                cmd.Parameters.AddWithValue("@PathAndFileName", NewFileName);
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ex", "alert('" + ex.Message + "');", false);
            }
            finally
            {
                EmetCon.Dispose();
            }
            return success;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string Command = "";

                if (LbModalHeader.Text == "Add Announcement")
                {
                    Command = "ADD";
                }
                else
                {
                    Command = "UPDATE";
                }

                if (Command == "ADD")
                {
                    string Subject = TxtSubject.Text.Trim();
                    if (CekExistSubject(Subject) == false)
                    {
                        if (IUD(Command) == true)
                        {
                            ShowTable();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Saved');closeModal();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Saved Faill !! Please contact your Admin');openModal();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('This Subject Already Exist !');openModal();", true);
                    }
                }
                else
                {
                    if (IUD(Command) == true)
                    {
                        ShowTable();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Updated');closeModal();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Saved Faill !! Please contact your Admin');openModal();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ex", "alert('" + ex.Message + "');", false);
            }
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            LbModalHeader.Text = "Add Announcement";
            DvdAttachmanetOld.Style.Add("display", "none");
            DvAttachmanet.Style.Add("display", "block");
            DvChangeAttachemnt.Visible = false;
            btnSubmit.Visible = true;
            TxtId.Text = "";
            TxtSubject.Text = "";
            TxtContent.Text = "";
            TxtSubject.Enabled = true;
            upModal.Update();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowTable();
            txtFind.Focus();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();closeModal();", true);
        }

        protected void DdlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtFind.Text = "";
                ShowTable();
                UpdatePanel2.Update();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                ShowTable();
            }
            catch (Exception ee)
            {
                Response.Write(ee);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editt")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];
                HiddenField theHiddenField = row.FindControl("HiddenId") as HiddenField;
                string Id = theHiddenField.Value.ToString();
                LbModalHeader.Text = "Edit Announcement";
                TxtSubject.Enabled = false;
                SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());

                try
                {

                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select id,Subject,ContentAnnc,PathAndFileName from tAnnouncement where Id = @Id ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                TxtId.Text = dt.Rows[0]["id"].ToString();
                                TxtSubject.Text = dt.Rows[0]["Subject"].ToString();
                                TxtContent.Text = dt.Rows[0]["ContentAnnc"].ToString();
                                TxtFilePath.Text = dt.Rows[0]["PathAndFileName"].ToString();
                                DvdAttachmanetOld.Style.Add("display", "block");
                                DvAttachmanet.Style.Add("display", "none");
                                DvChangeAttachemnt.Visible = true;
                                ChkDeleteAttachment.Checked = false;
                                string FullFileName = dt.Rows[0]["PathAndFileName"].ToString();
                                if (FullFileName == "")
                                {
                                    LblFileName.Text = "No Attachment";
                                    LblFileName.ForeColor = System.Drawing.Color.Red;
                                    LblFileName.Font.Bold = true;
                                    DvChkDeleteAttachment.Style.Add("display", "none");
                                }
                                else
                                {
                                    string FileName = FullFileName.Substring(14);
                                    LblFileName.Text = FileName;
                                    LblFileName.ForeColor = System.Drawing.Color.Black;
                                    LblFileName.Font.Bold = false;
                                    DvChkDeleteAttachment.Style.Add("display", "block");
                                }

                                TxtContent.Enabled = true;
                                BtnChangeAttachment.Visible = true;
                                btnSubmit.Visible = true;
                                DvdAttachmanetOld.Visible = true;

                                upModal.Update();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                TxtId.Text = "";
                                TxtSubject.Text = "";
                                TxtContent.Text = "";
                                TxtFilePath.Text = "";
                            }
                        }
                    }
                    upModal.Update();
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
                finally
                {
                    EmetCon.Dispose();
                }
            }
            if (e.CommandName == "CDelete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];
                HiddenField theHiddenField = row.FindControl("HiddenId") as HiddenField;
                string Id = theHiddenField.Value.ToString();
                string Command = "DELETE";
                TxtId.Text = Id.ToString();
                if (IUD(Command) == true)
                {
                    ShowTable();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Deleted');closeModal();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('Data Saved Faill !! Please contact your Admin');openModal();", true);
                }
            }
            if (e.CommandName == "CPreview")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GridView1.Rows[rowIndex];
                HiddenField theHiddenField = row.FindControl("HiddenId") as HiddenField;
                string Id = theHiddenField.Value.ToString();
                LbModalHeader.Text = "Preview Announcement";
                TxtSubject.Enabled = false;
                SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
                try
                {
                    EmetCon.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sql = @" select id,Subject,ContentAnnc,PathAndFileName from tAnnouncement where Id = @Id ";
                        cmd = new SqlCommand(sql, EmetCon);
                        cmd.Parameters.AddWithValue("@Id", Id);
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                TxtId.Text = dt.Rows[0]["id"].ToString();
                                TxtSubject.Text = dt.Rows[0]["Subject"].ToString();
                                TxtContent.Text = dt.Rows[0]["ContentAnnc"].ToString();
                                TxtFilePath.Text = dt.Rows[0]["PathAndFileName"].ToString();
                                DvdAttachmanetOld.Style.Add("display", "block");
                                DvAttachmanet.Style.Add("display", "none");
                                DvChangeAttachemnt.Visible = true;
                                ChkDeleteAttachment.Checked = false;
                                string FullFileName = dt.Rows[0]["PathAndFileName"].ToString();
                                if (FullFileName == "")
                                {
                                    LblFileName.Text = "No Attachment";
                                    LblFileName.ForeColor = System.Drawing.Color.Red;
                                    LblFileName.Font.Bold = true;
                                    DvChkDeleteAttachment.Style.Add("display", "none");
                                }
                                else
                                {
                                    string FileName = FullFileName.Substring(14);
                                    LblFileName.Text = FileName;
                                    LblFileName.ForeColor = System.Drawing.Color.Black;
                                    LblFileName.Font.Bold = false;
                                    DvChkDeleteAttachment.Style.Add("display", "block");
                                }

                                TxtContent.Enabled = false;
                                BtnChangeAttachment.Visible = false;
                                btnSubmit.Visible = false;
                                DvdAttachmanetOld.Visible = true;
                                DvChkDeleteAttachment.Style.Add("display", "none");

                                upModal.Update();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                            }
                            else
                            {
                                TxtId.Text = "";
                                TxtSubject.Text = "";
                                TxtContent.Text = "";
                                TxtFilePath.Text = "";
                            }
                        }
                    }
                    upModal.Update();
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
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
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

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

            return sortDirection;
        }

        protected void GridView1_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                Response.Write(ex);
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

        protected void LBtnDownload_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/attachment/");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string FileExtension = System.IO.Path.GetExtension(FuAttachment.FileName);
            string filename = System.IO.Path.GetFileName(FuAttachment.FileName);
            string PathAndFileName = folderPath + DateTime.Now.ToString("ddMMyyhhmmsstt") + filename;
            FuAttachment.SaveAs(PathAndFileName);

            if (filename == "" || string.IsNullOrEmpty(filename))
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('No attachment to download !');openModal();", true);
            }

            else
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Messagebox", "alert('File Not Found !');openModal();", true);
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
                for (int a = 0; a < (ArrFullFileName.Count()-1); a++)
                {
                    NewfileName += ArrFullFileName[a]+".";
                }

                NewfileName = NewfileName.Substring(14);
                NewfileName = NewfileName.Remove(NewfileName.Length - 1);
                FileExtension = ArrFullFileName[(ArrFullFileName.Count()-1)].ToString();
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
                    Response.AddHeader("content-disposition", "attachment;filename=" + (NewfileName+ "." +FileExtension));     // to open file prompt Box open or Save file         
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
                Response.Write(ex);
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
            catch (Exception ex2)
            {
                Response.Write(ex2);
            }
        }

    }
}