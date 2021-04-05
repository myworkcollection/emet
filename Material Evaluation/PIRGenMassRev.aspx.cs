using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Material_Evaluation
{

    public partial class PIRGenMassRev : System.Web.UI.Page
    {
        public string vendor;
        public string material;
        public string purchasorg;
        public string plant;
        public string inforcatstd;
        public string jobtyp;
        public string countryorg;
        public string CoNumber;
        public string ordunit;
        public string denominate;
        public string numerator;
        public string plndeliver;
        public string purgrp;
        public string stdqty;
        public string minqty;
        public string grbaseiv;
        public string cfmctlkey;
        public string taxcode;
        public string netprice;
        public string currency;
        public string priuntval;
        public string ordpriunit;
        public string pridatectl;
        public string validon;
        public string validto;
        public string condition1;
        public string amt1;
        public string unit1;
        public string per1;
        public string uom1;
        public string condition2;
        public string amt2;
        public string unit2;
        public string per2;
        public string uom2;
        public string condition3;
        public string amt3;
        public string unit3;
        public string per3;
        public string uom3;
        public string condition4;
        public string amt4;
        public string unit4;
        public string per4;
        public string uom4;
        public string infornote;
        public string infotext1;
        public string infotext2;
        public string infotext3;
        public string infotext4;
        public string infotext5;
        public string potext;
        public string potext1;
        public string potext2;
        public string potext3;
        public string potext4;
        public string potext5;
        public string Quotation;
        public string Quotation_Due;
        public string Underdelivery_Tolerance_Percentage;
        public string Overdelivery_Tolerance_Percentage;
        public string Supply_availability_from;
        public string Supply_availability_to;
        public string Vendor_subrange;
        public string Prod_version;

        public string Userid;
        public string userId;
        public string userId1;
        public string nameC;
        public string concatenated;
        public string concatenated1;
        //public static string Userid;
        public static string password;
        public static string domain;
        public static string path;
        public static string URL;
        public static string MasterDB;
        public static string TransDB;
        string DbMasterName = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                        string FN = "PIRGenMassRev";
                        string PL = Session["EPlant"].ToString();
                        if (EMETModule.IsAuthor(UI, FN, PL) == false)
                        {
                            Response.Redirect("Emet_author.aspx?num=0");
                        }
                        else
                        {
                            LoadSavings();
                            userId = Session["userID"].ToString();
                            userId1 = Session["userID"].ToString();
                            string sname = Session["UserName"].ToString();
                            nameC = sname;
                            string srole = Session["userType"].ToString();
                            string concat = sname + " - " + srole;
                            lbluser1.Text = sname;
                            lblplant.Text = srole;
                            LbsystemVersion.Text = Session["SystemVersion"].ToString();
                            Quotedetailsload("PIR");

                            if (Session["sidebarToggle"] == null)
                            {
                                SideBarMenu.Attributes.Add("style", "display:block;");
                            }
                            else
                            {
                                SideBarMenu.Attributes.Add("style", "display:none;");
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
                        Userid = dr.GetString(0);
                        password = dr.GetString(1);
                        domain = dr.GetString(2);
                        path = dr.GetString(3);
                        URL = dr.GetString(4);
                        MasterDB = dr.GetString(5);
                        TransDB = dr.GetString(6);
                    }
                    dr.Close();
                    Email_inser.Close();
                }
            }

            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
                string message = ex.Message;

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

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox chkAll = (CheckBox)GridView1.HeaderRow.FindControl("chkSelectAll");
                if (chkAll.Checked == true)
                {
                    foreach (GridViewRow gvRow in GridView1.Rows)
                    {
                        CheckBox chkSel = (CheckBox)gvRow.FindControl("chk");
                        string pir = gvRow.Cells[7].Text.ToString();
                        //if (pir == "PIR Pending")
                        //{ 
                        chkSel.Checked = true;
                        // }
                    }
                }
                else
                {
                    foreach (GridViewRow gvRow in GridView1.Rows)
                    {
                        CheckBox chkSel = (CheckBox)gvRow.FindControl("chk");
                        chkSel.Checked = false;

                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void chkheader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                string dg_checkvalue, dg_formid = string.Empty;
                CheckBox ChkBoxHeader = (CheckBox)GridView1.HeaderRow.FindControl("chkheader");
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkchild");
                    if (ChkBoxHeader.Checked == true)
                    {
                        ChkBoxRows.Checked = true;

                        dg_formid = row.Cells[0].Text.ToString();

                    }
                    else
                    {
                        ChkBoxRows.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void Quotedetailsload(string processgrp)
        {
            GetDbMaster();
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                DataTable Result = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
                string str = @"select tq.Plant,tq.Requestnumber,tq.Quoteno,tq.vendorcode1,tq.vendorname, tq.Material,tq.MaterialDesc,
                               (CASE WHEN (isnull(tr.status,'1') ='1' ) THEN 'PIR Pending' ELSE 'PIR Completed' END) AS status,
                                (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=tq.CreatedBy) as 'CreatedBy',
                               SMNPicDept as 'UseDep'
                               from " + TransDB.ToString() + "TQuoteDetails tq left outer join " + TransDB.ToString() + @"tpir_report tr on tq.QuoteNo=tr.quotation 
                               where tq.ManagerApprovalStatus='" + 2 + "' and (isMassRevision = 1)";
                if (txtFind.Text != "")
                {
                    if (DdlFilterBy.SelectedValue.ToString() == "Quoteno")
                    {
                        str += @" and tq.Quoteno like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "Requestnumber")
                    {
                        str += @" and tq.RequestNumber like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "vendorcode1")
                    {
                        str += @" and tq.VendorCode1 like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "vendorname")
                    {
                        str += @" and tq.VendorName like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "Material")
                    {
                        str += @" and tq.Material like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "MaterialDesc")
                    {
                        str += @" and tq.MaterialDesc like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "status")
                    {
                        str += @" and (CASE WHEN (isnull(tr.status,'1') ='1' ) THEN 'PIR Pending' ELSE 'PIR Completed' END) like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "Plant")
                    {
                        str += @" and tq.Plant like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "CreatedBy")
                    {
                        str += @" and (select UPPER(UseNam) from " + DbMasterName + @".[dbo].Usr where UseID=tq.CreatedBy) like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "UseDep")
                    {
                        str += @" and SMNPicDept like '%'+ '" + txtFind.Text + "' + '%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroup")
                    {
                        str += @" and tq.ProcessGroup like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                    else if (DdlFilterBy.SelectedValue.ToString() == "ProcessGroupDesc")
                    {
                        str += @" and (select distinct TPG.Process_Grp_Description from " + DbMasterName + ".dbo.TPROCESGROUP_LIST TPG where TPG.Process_Grp_code = tq.ProcessGroup) like '%'+ '" + txtFind.Text + "' +'%' ";
                    }
                }

                if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                {
                    str += @"  Order by " + ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString() + " ";
                }
                else
                {
                    str += @" Order by status desc ";
                }
                da = new SqlDataAdapter(str, EmetCon);
                Result = new DataTable();
                da.Fill(Result);

                //     Result.Rows.Add(dr);

                if (Result.Rows.Count > 0)
                {
                    GridView1.DataSource = Result;
                    GridView1.DataBind();
                    LbTtlRecords.Text = "Total Record: " + Result.Rows.Count;
                }
                else
                {
                    GridView1.DataSource = Result;
                    GridView1.DataBind();
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Approve")
                {
                    #region
                    //int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //GridViewRow row = GridView1.Rows[rowIndex];
                    //string ReqNumber = row.Cells[3].Text;
                    //string Vendor = row.Cells[4].Text;
                    //string Quote = row.Cells[2].Text;
                    //LoadPIR(0, Quote);
                    ////UpdateGridData(ReqNumber, Vendor, 2, Reason);
                    ////GetGridData();
                    //Quotedetailsload("PIR");
                    #endregion
                }
                else if (e.CommandName == "Reject")
                {
                    
                }
                else if (e.CommandName == "ExpPdf")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GridView1.Rows[rowIndex];
                    string Qno = GridView1.Rows[rowIndex].Cells[2].Text.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "ExportPdf('" + Qno + "')", true);
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
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //e.Row.Cells[11].ColumnSpan = 2;
                    //e.Row.Cells.RemoveAt(12);
                }
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                {
                    #region old (never used)
                    //int RowSpan = 2;
                    //for (int i = GridView1.Rows.Count - 2; i >= 0; i--)
                    //{
                    //    GridViewRow currRow = GridView1.Rows[i];
                    //    GridViewRow prevRow = GridView1.Rows[i + 1];
                    //    if (currRow.Cells[2].Text == prevRow.Cells[2].Text)
                    //    {
                    //        //currRow.Cells[1].RowSpan = RowSpan;
                    //        //prevRow.Cells[1].Visible = false;

                    //        //currRow.Cells[0].RowSpan = RowSpan;
                    //        //prevRow.Cells[0].Visible = false;

                    //        //currRow.Cells[2].RowSpan = RowSpan;
                    //        //prevRow.Cells[2].Visible = false;

                    //        //currRow.Cells[3].RowSpan = RowSpan;
                    //        //prevRow.Cells[3].Visible = false;

                    //        //currRow.Cells[4].RowSpan = RowSpan;
                    //        //prevRow.Cells[4].Visible = false;

                    //        //currRow.Cells[5].RowSpan = RowSpan;
                    //        //prevRow.Cells[5].Visible = false;

                    //        //currRow.Cells[6].RowSpan = RowSpan;
                    //        //prevRow.Cells[6].Visible = false;

                    //        RowSpan += 1;
                    //    }
                    //    else
                    //    {
                    //        RowSpan = 2;
                    //    }
                    //}
                    //Vendor 
                    //if (e.Row.Cells[10].Text == "PIR Pending")
                    //{
                    //    //e.Row.Cells[18].Text = "Vendor Pending";
                    //    e.Row.Cells[11].Enabled = true;
                    //    e.Row.Cells[12].Enabled = false;
                    //    e.Row.Cells[11].Visible = false;
                    //    e.Row.Cells[12].Visible = false;
                    //}
                    //else
                    //{
                    //    e.Row.Cells[11].Enabled = false;
                    //    e.Row.Cells[12].Enabled = true;
                    //    e.Row.Cells[0].Enabled = false;
                    //    e.Row.Cells[11].Visible = false;
                    //    e.Row.Cells[12].Visible = false;
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                Quotedetailsload("PIR");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.Header)
                {
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (i > 0)
                        {
                            TableCell tc = e.Row.Cells[i];
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
                                        icon.CssClass = "topright";
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
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                GetSortDirection(e.SortExpression);
                Quotedetailsload("PIR");
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

        protected void DdlFltrDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                Quotedetailsload("PIR");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void DdlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                txtFind.Text = "";
                Quotedetailsload("PIR");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtFrom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                Quotedetailsload("PIR");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void TxtTo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePanel1.Update();
                Quotedetailsload("PIR");
            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Quotedetailsload("PIR");
            txtFind.Focus();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            DdlFilterBy.SelectedIndex = 0;
            txtFind.Text = "";
            Quotedetailsload("PIR");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "CloseLoading();", true);
        }

        protected void LoadPIR(int flag, string Quoteno)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                //var quoteNo = txtQuoteNo.Text;
                using (SqlConnection con = new SqlConnection(EMETModule.GenEMETConnString()))
                {

                    //start
                    vendor = string.Empty;
                    material = string.Empty;
                    plant = string.Empty;
                    inforcatstd = string.Empty;
                    jobtyp = string.Empty;
                    amt1 = string.Empty;
                    amt2 = string.Empty;
                    amt3 = string.Empty;
                    amt4 = string.Empty;
                    netprice = string.Empty;
                    validon = string.Empty;
                    Quotation_Due = string.Empty;
                    countryorg = string.Empty;
                    Quotation = string.Empty;
                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = flag;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quoteno;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {
                                vendor = pir.Rows[0]["vendorcode1"].ToString();
                                material = pir.Rows[0]["material"].ToString();
                                plant = pir.Rows[0]["Plant"].ToString();
                                inforcatstd = pir.Rows[0]["pirtype"].ToString();
                                jobtyp = pir.Rows[0]["pirjobtype"].ToString();
                                amt1 = pir.Rows[0]["per1"].ToString();
                                amt2 = pir.Rows[0]["per2"].ToString();
                                amt3 = pir.Rows[0]["per3"].ToString();
                                amt4 = pir.Rows[0]["per4"].ToString();
                                netprice = pir.Rows[0]["netprice"].ToString();
                                validon = Convert.ToString(pir.Rows[0]["edate"].ToString());
                                Quotation_Due = Convert.ToString(pir.Rows[0]["Ddate"].ToString());
                                countryorg = pir.Rows[0]["CountryOrg"].ToString();
                                Quotation = Quoteno.ToString();

                            }
                        }
                    }
                    //End

                    //start
                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran_Plant"))
                    {
                        int plant_pir = Convert.ToInt32(plant.ToString());
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = plant_pir;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quoteno;

                        purchasorg = string.Empty;
                        //countryorg = pir.Rows[0]["cty"].ToString();
                        purgrp = string.Empty;
                        ordunit = string.Empty;
                        ordpriunit = string.Empty;
                        unit1 = string.Empty;
                        unit2 = string.Empty;
                        unit3 = string.Empty;
                        unit4 = string.Empty;
                        uom1 = string.Empty;
                        uom2 = string.Empty;
                        uom3 = string.Empty;
                        uom4 = string.Empty;
                        cfmctlkey = string.Empty;
                        currency = string.Empty;
                        taxcode = string.Empty;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {
                                purchasorg = pir.Rows[0]["porg"].ToString();
                                //countryorg = pir.Rows[0]["cty"].ToString();
                                purgrp = pir.Rows[0]["PURGRP"].ToString();
                                ordunit = pir.Rows[0]["baseUOM"].ToString();
                                ordpriunit = pir.Rows[0]["baseUOM"].ToString();
                                unit1 = pir.Rows[0]["Crcy"].ToString();
                                unit2 = pir.Rows[0]["Crcy"].ToString();
                                unit3 = pir.Rows[0]["Crcy"].ToString();
                                unit4 = pir.Rows[0]["Crcy"].ToString();
                                uom1 = pir.Rows[0]["baseUOM"].ToString();
                                uom2 = pir.Rows[0]["baseUOM"].ToString();
                                uom3 = pir.Rows[0]["baseUOM"].ToString();
                                uom4 = pir.Rows[0]["baseUOM"].ToString();
                                cfmctlkey = pir.Rows[0]["ConfContlKey"].ToString();
                                currency = pir.Rows[0]["Crcy"].ToString();
                                taxcode = pir.Rows[0]["taxcode"].ToString();
                            }
                        }
                    }
                    //End


                    //start
                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        int plant_pir = Convert.ToInt32(plant.ToString());
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 2;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quoteno;

                        condition1 = string.Empty;
                        condition2 = string.Empty;
                        condition3 = string.Empty;
                        condition4 = string.Empty;
                        CoNumber = string.Empty;
                        denominate = string.Empty;
                        grbaseiv = string.Empty;
                        infornote = string.Empty;
                        infotext1 = string.Empty;
                        infotext2 = string.Empty;
                        infotext3 = string.Empty;
                        infotext4 = string.Empty;
                        infotext5 = string.Empty;
                        minqty = string.Empty;
                        numerator = string.Empty;
                        Overdelivery_Tolerance_Percentage = string.Empty;
                        per1 = string.Empty;
                        per2 = string.Empty;
                        per3 = string.Empty;
                        per4 = string.Empty;
                        plndeliver = string.Empty;
                        potext = string.Empty;
                        potext1 = string.Empty;
                        potext2 = string.Empty;
                        potext3 = string.Empty;
                        potext4 = string.Empty;
                        potext5 = string.Empty;
                        pridatectl = string.Empty;
                        priuntval = string.Empty;
                        Prod_version = string.Empty;
                        stdqty = string.Empty;
                        Supply_availability_from = string.Empty;
                        Supply_availability_to = string.Empty;
                        Underdelivery_Tolerance_Percentage = string.Empty;
                        validto = string.Empty;
                        Vendor_subrange = string.Empty;


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {

                                condition1 = Convert.ToString(pir.Rows[0]["defvalue"].ToString());
                                condition2 = Convert.ToString(pir.Rows[1]["defvalue"].ToString());
                                condition3 = Convert.ToString(pir.Rows[2]["defvalue"].ToString());
                                condition4 = Convert.ToString(pir.Rows[3]["defvalue"].ToString());
                                CoNumber = Convert.ToString(pir.Rows[4]["defvalue"].ToString());
                                denominate = Convert.ToString(pir.Rows[5]["defvalue"].ToString());
                                grbaseiv = Convert.ToString(pir.Rows[6]["defvalue"].ToString());
                                infornote = Convert.ToString(pir.Rows[7]["defvalue"].ToString());
                                infotext1 = Convert.ToString(pir.Rows[8]["defvalue"].ToString());
                                infotext2 = Convert.ToString(pir.Rows[9]["defvalue"].ToString());
                                infotext3 = Convert.ToString(pir.Rows[10]["defvalue"].ToString());
                                infotext4 = Convert.ToString(pir.Rows[11]["defvalue"].ToString());
                                infotext5 = Convert.ToString(pir.Rows[12]["defvalue"].ToString());
                                minqty = Convert.ToString(pir.Rows[13]["defvalue"].ToString());
                                numerator = Convert.ToString(pir.Rows[14]["defvalue"].ToString());
                                Overdelivery_Tolerance_Percentage = Convert.ToString(pir.Rows[15]["defvalue"].ToString());
                                per1 = Convert.ToString(pir.Rows[16]["defvalue"].ToString());
                                per2 = Convert.ToString(pir.Rows[17]["defvalue"].ToString());
                                per3 = Convert.ToString(pir.Rows[18]["defvalue"].ToString());
                                per4 = Convert.ToString(pir.Rows[19]["defvalue"].ToString());
                                plndeliver = Convert.ToString(pir.Rows[20]["defvalue"].ToString());
                                potext = Convert.ToString(pir.Rows[21]["defvalue"].ToString());
                                potext1 = Convert.ToString(pir.Rows[22]["defvalue"].ToString());
                                potext2 = Convert.ToString(pir.Rows[23]["defvalue"].ToString());
                                potext3 = Convert.ToString(pir.Rows[24]["defvalue"].ToString());
                                potext4 = Convert.ToString(pir.Rows[25]["defvalue"].ToString());
                                potext5 = Convert.ToString(pir.Rows[26]["defvalue"].ToString());
                                pridatectl = Convert.ToString(pir.Rows[27]["defvalue"].ToString());
                                priuntval = Convert.ToString(pir.Rows[28]["defvalue"].ToString());
                                Prod_version = Convert.ToString(pir.Rows[29]["defvalue"].ToString());
                                stdqty = Convert.ToString(pir.Rows[30]["defvalue"].ToString());
                                Supply_availability_from = Convert.ToString(pir.Rows[31]["defvalue"].ToString());
                                Supply_availability_to = Convert.ToString(pir.Rows[32]["defvalue"].ToString());
                                Underdelivery_Tolerance_Percentage = Convert.ToString(pir.Rows[33]["defvalue"].ToString());
                                validto = Convert.ToString(pir.Rows[34]["defvalue"].ToString());
                                Vendor_subrange = Convert.ToString(pir.Rows[35]["defvalue"].ToString());

                            }
                        }
                    }
                    //End


                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        int plant_pir = Convert.ToInt32(plant.ToString());
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 6;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quoteno;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count > 0)
                            {
                                plndeliver = Convert.ToString(pir.Rows[0]["PlDelTime"].ToString());
                            }
                        }
                    }
                    //End

                    //start

                    using (SqlCommand cmd = new SqlCommand("Get_PIRTran"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.Int).Value = 3;
                        cmd.Parameters.Add("@quotenumber", SqlDbType.NVarChar, 50).Value = Quoteno;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable pir = new DataTable();
                            sda.Fill(pir);
                            if (pir.Rows.Count <= 0)
                            {
                                int status = 0;
                                string query = "INSERT INTO " + TransDB.ToString() + "tpir_report(vendor, material, plant, inforcatstd, jobtyp, amt1, amt2, amt3, amt4, netprice, validon, Quotation_Due, Quotation, purchasorg,countryorg, purgrp, ordunit, ordpriunit, unit1, unit2, unit3, unit4, uom1, uom2, uom3, uom4, cfmctlkey, currency, taxcode, condition1,condition2, condition3, condition4, CoNumber, denominate, grbaseiv, infornote, infotext1, infotext2, infotext3, infotext4,infotext5, minqty, numerator, Overdelivery_Tolerance_Percentage, per1, per2, per3, per4, plndeliver, potext, potext1, potext2, potext3,potext4, potext5, pridatectl, priuntval, Prod_version, stdqty, Supply_availability_from, Supply_availability_to,Underdelivery_Tolerance_Percentage, validto, Vendor_subrange,status) values (@vendor, @material, @plant, @inforcatstd, @jobtyp, @amt1, @amt2, @amt3, @amt4, @netprice, @validon, @Quotation_Due, @Quotation, @purchasorg, @countryorg, @purgrp, @ordunit, @ordpriunit,@unit1, @unit2, @unit3, @unit4, @uom1, @uom2, @uom3, @uom4, @cfmctlkey, @currency, @taxcode, @condition1, @condition2, @condition3, @condition4, @CoNumber, @denominate, @grbaseiv,@infornote, @infotext1, @infotext2, @infotext3, @infotext4, @infotext5, @minqty, @numerator, @Overdelivery_Tolerance_Percentage, @per1, @per2, @per3, @per4, @plndeliver, @potext, @potext1,@potext2, @potext3, @potext4, @potext5, @pridatectl, @priuntval, @Prod_version, @stdqty, @Supply_availability_from, @Supply_availability_to, @Underdelivery_Tolerance_Percentage,@validto, @Vendor_subrange,@status)";
                                //    string text = "Data Saved!";
                                SqlCommand cmd1 = new SqlCommand(query, EmetCon);
                                cmd1.Parameters.AddWithValue("@vendor", vendor.ToString());
                                cmd1.Parameters.AddWithValue("@material", material.ToString());
                                cmd1.Parameters.AddWithValue("@plant", plant.ToString());
                                cmd1.Parameters.AddWithValue("@inforcatstd", inforcatstd.ToString());
                                cmd1.Parameters.AddWithValue("@jobtyp", jobtyp.ToString());
                                cmd1.Parameters.AddWithValue("@amt1", amt1.ToString());
                                cmd1.Parameters.AddWithValue("@amt2", amt2.ToString());
                                cmd1.Parameters.AddWithValue("@amt3", amt3.ToString());
                                cmd1.Parameters.AddWithValue("@amt4", amt4.ToString());
                                cmd1.Parameters.AddWithValue("@netprice", netprice.ToString());
                                cmd1.Parameters.AddWithValue("@validon", validon.ToString());
                                cmd1.Parameters.AddWithValue("@Quotation_Due", Quotation_Due.ToString());
                                cmd1.Parameters.AddWithValue("@Quotation", Quotation.ToString());
                                cmd1.Parameters.AddWithValue("@purchasorg", purchasorg.ToString());
                                cmd1.Parameters.AddWithValue("@countryorg", countryorg.ToString());
                                cmd1.Parameters.AddWithValue("@purgrp", purgrp.ToString());
                                cmd1.Parameters.AddWithValue("@ordunit", ordunit.ToString());
                                cmd1.Parameters.AddWithValue("@ordpriunit", ordpriunit.ToString());
                                cmd1.Parameters.AddWithValue("@unit1", unit1.ToString());
                                cmd1.Parameters.AddWithValue("@unit2", unit2.ToString());
                                cmd1.Parameters.AddWithValue("@unit3", unit3.ToString());
                                cmd1.Parameters.AddWithValue("@unit4", unit4.ToString());
                                cmd1.Parameters.AddWithValue("@uom1", uom1.ToString());
                                cmd1.Parameters.AddWithValue("@uom2", uom2.ToString());
                                cmd1.Parameters.AddWithValue("@uom3", uom3.ToString());
                                cmd1.Parameters.AddWithValue("@uom4", uom4.ToString());
                                cmd1.Parameters.AddWithValue("@cfmctlkey", cfmctlkey.ToString());
                                cmd1.Parameters.AddWithValue("@currency", currency.ToString());
                                cmd1.Parameters.AddWithValue("@taxcode", taxcode.ToString());
                                cmd1.Parameters.AddWithValue("@condition1", condition1.ToString());
                                cmd1.Parameters.AddWithValue("@condition2", condition2.ToString());
                                cmd1.Parameters.AddWithValue("@condition3", condition3.ToString());
                                cmd1.Parameters.AddWithValue("@condition4", condition4.ToString());
                                cmd1.Parameters.AddWithValue("@CoNumber", CoNumber.ToString());
                                cmd1.Parameters.AddWithValue("@denominate", denominate.ToString());
                                cmd1.Parameters.AddWithValue("@grbaseiv", grbaseiv.ToString());
                                cmd1.Parameters.AddWithValue("@infornote", infornote.ToString());
                                cmd1.Parameters.AddWithValue("@infotext1", infotext1.ToString());
                                cmd1.Parameters.AddWithValue("@infotext2", infotext2.ToString());
                                cmd1.Parameters.AddWithValue("@infotext3", infotext3.ToString());
                                cmd1.Parameters.AddWithValue("@infotext4", infotext4.ToString());
                                cmd1.Parameters.AddWithValue("@infotext5", infotext5.ToString());
                                cmd1.Parameters.AddWithValue("@minqty", minqty.ToString());
                                cmd1.Parameters.AddWithValue("@numerator", numerator.ToString());
                                cmd1.Parameters.AddWithValue("@Overdelivery_Tolerance_Percentage", Overdelivery_Tolerance_Percentage.ToString());
                                cmd1.Parameters.AddWithValue("@per1", per1.ToString());
                                cmd1.Parameters.AddWithValue("@per2", per2.ToString());
                                cmd1.Parameters.AddWithValue("@per3", per3.ToString());
                                cmd1.Parameters.AddWithValue("@per4", per4.ToString());
                                cmd1.Parameters.AddWithValue("@plndeliver", plndeliver.ToString());
                                cmd1.Parameters.AddWithValue("@potext", potext.ToString());
                                cmd1.Parameters.AddWithValue("@potext1", potext1.ToString());
                                cmd1.Parameters.AddWithValue("@potext2", potext2.ToString());
                                cmd1.Parameters.AddWithValue("@potext3", potext3.ToString());
                                cmd1.Parameters.AddWithValue("@potext4", potext4.ToString());
                                cmd1.Parameters.AddWithValue("@potext5", potext5.ToString());
                                cmd1.Parameters.AddWithValue("@pridatectl", pridatectl.ToString());
                                cmd1.Parameters.AddWithValue("@priuntval", priuntval.ToString());
                                cmd1.Parameters.AddWithValue("@Prod_version", Prod_version.ToString());
                                cmd1.Parameters.AddWithValue("@stdqty", stdqty.ToString());
                                cmd1.Parameters.AddWithValue("@Supply_availability_from", Supply_availability_from.ToString());
                                cmd1.Parameters.AddWithValue("@Supply_availability_to", Supply_availability_to.ToString());
                                cmd1.Parameters.AddWithValue("@Underdelivery_Tolerance_Percentage", Underdelivery_Tolerance_Percentage.ToString());
                                cmd1.Parameters.AddWithValue("@validto", validto.ToString());
                                cmd1.Parameters.AddWithValue("@Vendor_subrange", Vendor_subrange.ToString());
                                cmd1.Parameters.AddWithValue("@status", status);

                                cmd1.CommandText = query;
                                cmd1.ExecuteNonQuery();
                                //End

                                Label1.Text = "Generated Successfully";
                            }
                            else
                            {
                                Label1.Text = "Generated Successfully including Old PIR";
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            try
            {
                EmetCon.Open();
                string pirquote;
                concatenated = string.Empty;
                concatenated1 = "s";

                foreach (GridViewRow gr in GridView1.Rows)
                {
                    CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                    if (myCheckbox.Checked == true)
                    {
                        // dg_checkvalue = "Y";
                        pirquote = gr.Cells[2].Text.ToString();

                        LoadPIR(0, pirquote);
                        Quotedetailsload("PIR");
                        pirquote = string.Concat("'", pirquote.ToString(), "'");
                        if (concatenated1 == "s")
                        {
                            concatenated = pirquote.ToString();
                            concatenated1 = "A";
                        }
                        else
                        {
                            concatenated = string.Concat(concatenated, ",", pirquote);
                        }
                    }
                }

                if (concatenated != "") {
                    DataTable Result = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    //    string str = "select VendorCode,VendorName from TVENDOR_PROCESSGROUP where ProcessGrp='" + ddlprocess.SelectedItem.Text + "'";
                    string str = "select vendor,material,purchasorg,plant,inforcatstd,jobtyp,countryorg,CoNumber,ordunit,denominate,numerator,plndeliver,purgrp,stdqty,minqty,grbaseiv,cfmctlkey,taxcode,netprice,currency,priuntval,ordpriunit,pridatectl,validon,validto,condition1,amt1,unit1,per1,uom1,condition2,amt2,unit2,per2,uom2,condition3,amt3,unit3,per3,uom3,condition4,amt4,unit4,per4,uom4,infornote,infotext1,infotext2,infotext3,infotext4,infotext5,potext,potext1,potext2,potext3,potext4,potext5,Quotation,Quotation_Due,Underdelivery_Tolerance_Percentage,Overdelivery_Tolerance_Percentage,Supply_availability_from,Supply_availability_to,Vendor_subrange,Prod_version from " + TransDB.ToString() + "tpir_report where quotation in(" + concatenated.ToString() + ")";
                    da = new SqlDataAdapter(str, EmetCon);
                    Result = new DataTable();
                    da.Fill(Result);


                    string fname = "PIR_";
                    DateTime Dat1 = DateTime.Now;
                    fname = fname + Convert.ToString(Dat1.ToString()) + ".xls";

                    string attachment = "attachment; filename=" + fname + "";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in Result.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in Result.Rows)
                    {
                        tab = "";
                        for (i = 0; i < Result.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                    Quotedetailsload("PIR");
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
            finally
            {
                EmetCon.Dispose();
            }
        }

        protected void BtnExportAllSelecToPdf_Click(object sender, EventArgs e)
        {
            SqlConnection EmetCon = new SqlConnection(EMETModule.GenEMETConnString());
            GetDbMaster();
            try
            {
                EmetCon.Open();
                List<string> LsQuoteNo = new List<string>();
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    CheckBox myCheckbox = (CheckBox)(gr.Cells[0].Controls[1]);
                    if (myCheckbox.Checked == true)
                    {
                        string QuoNo = "'" + gr.Cells[2].Text + "'";
                        LsQuoteNo.Add(QuoNo);
                    }
                }


                if (LsQuoteNo.Count > 0)
                {
                    StringBuilder builderQuoteList = new StringBuilder();
                    // Loop through all strings.
                    for (int i = 0; i < LsQuoteNo.Count; i++)
                    {
                        if (i < (LsQuoteNo.Count - 1))
                        {
                            builderQuoteList.Append(LsQuoteNo[i].ToString()).Append(",");
                        }
                        else
                        {
                            builderQuoteList.Append(LsQuoteNo[i].ToString());
                        }
                    }
                    // Get string from StringBuilder.
                    string result = builderQuoteList.ToString();

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        string sql = @" select A.Material,replace((A.MaterialDesc + space(60-len(A.MaterialDesc))),' ','&nbsp;') as MaterialDesc,A.QuoteNo,A.VendorName,
                                     case 
                                    when A.ApprovalStatus = 0 and A.PICApprovalStatus = 0 and A.ManagerApprovalStatus is null and A.DIRApprovalStatus is null then 'Pending'
                                    when A.ApprovalStatus = 0 and A.PICApprovalStatus is null and A.ManagerApprovalStatus is null and A.DIRApprovalStatus is null then 'Pending'
                                    when A.ApprovalStatus = 2 and A.PICApprovalStatus = 0 and A.ManagerApprovalStatus is null and A.DIRApprovalStatus is null then 'Pending'
                                    when A.PICApprovalStatus = 2 and A.ManagerApprovalStatus = 0 and A.DIRApprovalStatus is null then 'Pending' 
                                    when A.PICApprovalStatus = 1 and (A.ManagerApprovalStatus = 0 or A.ManagerApprovalStatus = 1) and A.DIRApprovalStatus is null  then 'Pending'
                                    when A.ManagerApprovalStatus = 2 and A.DIRApprovalStatus = 0 then 'Approved'
                                    when A.ManagerApprovalStatus = 1 and (A.DIRApprovalStatus = 0 or A.DIRApprovalStatus = 1) then 'Rejected'
                                    when A.ManagerApprovalStatus = 4 and A.DIRApprovalStatus = 4 then 'No Need Approval'
                                    when A.ManagerApprovalStatus = 5 and A.DIRApprovalStatus = 5 then 'No Need Approval'
                                    else 'cannot find status'
                                    end as 'DDecision',

                                    (select distinct case when len(US.UseNam) > 13 then CONCAT(left(US.UseNam,13),'...') else UseNam end as UseNam from " + DbMasterName + @".dbo.Usr US where US.UseID = A.AprRejBy) as Dname,
                                    format(A.AprRejDate,'dd/MM/yyyy') as DAprRejDt,

                                    FORMAT(A.EffectiveDate,'dd/MM/yyyy') as 'EffectiveDate',

                                    case 
                                    when A.pirstatus is null or pirstatus = '' then NULL
                                    else CAST(ROUND(A.TotalMaterialCost,5) AS DECIMAL(12,5)) 
                                    end as TotalMaterialCost,

                                    case 
                                    when A.pirstatus is null or A.pirstatus = '' then NULL
                                    else CAST(ROUND(A.TotalProcessCost,5) AS DECIMAL(12,5))
                                    end as TotalProcessCost,

                                    case 
                                    when A.pirstatus is null or A.pirstatus = '' then NULL
                                    else CAST(ROUND(A.TotalSubMaterialCost,5) AS DECIMAL(12,5))
                                    end as TotalSubMaterialCost,

                                    case 
                                    when A.pirstatus is null or A.pirstatus = '' then NULL
                                    else CAST(ROUND(A.TotalOtheritemsCost,5) AS DECIMAL(12,5))
                                    end  as TotalOtheritemsCost,

                                    case 
                                    when A.pirstatus is null or A.pirstatus = '' then NULL
                                    else CAST(ROUND(A.GrandTotalCost,5) AS DECIMAL(12,5))
                                    end as GrandTotalCost,


                                    case 
                                    when A.pirstatus is null or A.pirstatus = '' then NULL
                                    else CAST(ROUND(A.FinalQuotePrice,5) AS DECIMAL(12,5)) 
                                    end  as FinalQuotePrice

                                    from TQuoteDetails A 
                                    left outer join tpir_report tr on A.QuoteNo=tr.quotation
                                    where A.QuoteNo in (" + result + @")
                                    and (CreateStatus <> '' or CreateStatus is not null)  
                                    and (ApprovalStatus = 3 and ManagerApprovalStatus = 2 and DIRApprovalStatus = 0)  
                                    and (isUseSAPCode = 1) and (QuoteNoRef is null) and (isMassRevision = 1)  
                                     ";
                        string SortBy = "";
                        string MySortDirection = "";
                        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                        {
                            if (ViewState["SortExpression"].ToString().ToUpper() == "STATUS")
                            {
                                SortBy = "TR." + ViewState["SortExpression"].ToString();
                            }
                            else if (ViewState["SortExpression"].ToString().ToUpper() == "USEDEP")
                            {
                                SortBy = "A.SMNPicDept";
                            }
                            else
                            {
                                SortBy = "A." + ViewState["SortExpression"].ToString();
                            }
                            MySortDirection = ViewState["SortDirection"].ToString();
                        }
                        else
                        {
                            SortBy = "A.QuoteNo";
                            MySortDirection = " DESC";
                        }
                        sql += @"  Order by " + SortBy.Trim() + " " + MySortDirection + " ";
                        SqlCommand cmd = new SqlCommand(sql, EmetCon);
                        //cmd.Parameters.AddWithValue("@QuoteList", builderQuoteList.ToString());
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                Session["QuoteListToPdf"] = dt;
                                //GenerateDesignPdf();
                                GenerateDesignPdfNew();
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Alert", "alert('No Data Selected !');CloseLoading();", true);
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
            finally
            {
                EmetCon.Dispose();
            }
        }

        public void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
        {
            try
            {
                object TargetFile = FileName;
                string ModifiedFileName = string.Empty;
                string FinalFileName = string.Empty;

                /* To add a Password to PDF -http://aspnettutorialonline.blogspot.com/ */
                iTextSharp.text.Rectangle MyPaperSize = new Rectangle(842, 595);
                TestPDF.HtmlToPdfBuilder builder = new TestPDF.HtmlToPdfBuilder(MyPaperSize);
                TestPDF.HtmlPdfPage first = builder.AddPage();
                first.AppendHtml(HtmlStream);
                //first.AppendHtml(HtmlStream + "asdadasd");
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

        string htmlTextNew(int PageNo, int pagesize, int TotPage)
        {
            string MyhtmlText = "";
            try
            {
                string CurrDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                string str = @"
                        <p style='font-size: 8px; padding-top:15px;'>
                                        <table border='1'>
                                            <thead>
                                                <tr> 
                                                    <th  border='0' style='text-align:left' colspan='14'> <h5><strong>e-MET PIR Generation Mass Revision</strong> </h5><br /> </td>
                                                </tr>
                                                <tr>
                                                    <th border='1' style='text-align:center' width='5%'><b>Material</b></th>
                                                    <th border='1' style='text-align:center' width='20%'><b>Material Desc</b></th>
                                                    <th border='1' style='text-align:center' width='7%'><b>Quote No</b></th>
                                                    <th border='1' style='text-align:center' width='15%'><b>Vendor Name</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Director Decision</b></th>
                                                    <th border='1' style='text-align:center' width='6%'><b>Director Name</b></th>
                                                    <th border='1' style='text-align:center' width='6%'><b>Director Aproved / Rejected Date</b></th>
                                                    <th border='1' style='text-align:center' width='6%'><b>Effective Date</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Total Material Cost</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Total Process Cost</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Total Sub Material Cost</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Total Other Items Cost</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Grand Total Cost</b></th>
                                                    <th border='1' style='text-align:center' width='5%'><b>Final Quote Price</b></th>
                                                </tr>
                                            </thead>
                                            <tbody>";
                if (Session["QuoteListToPdf"] != null)
                {
                    DataTable dtQuoteListToPdf = (DataTable)Session["QuoteListToPdf"];
                    if (dtQuoteListToPdf.Rows.Count > 0)
                    {
                        int startfrom = 0;
                        int MaxRecord = 0;

                        MaxRecord = PageNo * pagesize;
                        startfrom = (PageNo * pagesize) - pagesize;

                        for (int i = startfrom; i < dtQuoteListToPdf.Rows.Count; i++)
                        {
                            if (i < MaxRecord)
                            {
                                str += @"<tr >";
                                for (int c = 0; c < dtQuoteListToPdf.Columns.Count; c++)
                                {
                                    if (c < 8)
                                    {
                                        string colName = dtQuoteListToPdf.Columns[c].ColumnName;
                                        if (colName == "Material")
                                        {
                                            str += @"<td width='5%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "MaterialDesc")
                                        {
                                            str += @"<td width='20%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "QuoteNo")
                                        {
                                            str += @"<td width='7%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "VendorName")
                                        {
                                            str += @"<td width='15%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "Dname")
                                        {
                                            str += @"<td width='6%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "DAprRejDt")
                                        {
                                            str += @"<td width='6%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else if (colName == "EffectiveDate")
                                        {
                                            str += @"<td width='6%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                        else
                                        {
                                            str += @"<td width='5%'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                        }
                                    }
                                    else
                                    {
                                        str += @"<td  style='text-align:right'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
                                    }
                                }
                                str += @"</tr>";
                            }
                        }
                        str += @"<tr border='0' style='text-align:right'> 
                                 <td  border='0' style='text-align:center' colspan='7'> Page " + PageNo + @" of " + TotPage + @"  </td> 
                                 <td  border='0' style='text-align:right' colspan='7'> Download from e-MET PIR Generation on " + CurrDate + @"  </td> 
                                 </tr>";
                    }
                }
                str += @" </p>
                         </tbody>
                         </table>
                        ";

                MyhtmlText = str;
            }
            catch (Exception ex)
            {
                MyhtmlText = ex.Message.ToString();
            }
            return MyhtmlText;
        }

        protected void GenerateDesignPdfNew()
        {
            try
            {
                string str = "";

                if (Session["QuoteListToPdf"] != null)
                {
                    DataTable dtQuoteListToPdf = (DataTable)Session["QuoteListToPdf"];
                    if (dtQuoteListToPdf.Rows.Count > 0)
                    {
                        string CurrDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                        GvToExport.DataSource = dtQuoteListToPdf;
                        GvToExport.PageSize = 15;
                        GvToExport.DataBind();

                        str += @" <HTML>
                                <head>
                                    <meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1' />
                                    <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no' />
                                    <meta name='description' content='' />
                                    <meta name='author' content='' />
                                    <link href='../Styles/bootstrap-3.4.1-dist/css/bootstrap.min.css' rel='stylesheet' />
                                    <link href='../vendor/fontawesome-free/css/all.min.css' rel='stylesheet' type='text/css' />
                                    <link href='../vendor/datatables/dataTables.bootstrap4.css' rel='stylesheet' />
                                    <link href='../css/sb-admin.css' rel='stylesheet' />
                                    <link href='../Styles/NewStyle/NewStyle.css' rel='stylesheet' />
                                    <link href='../Scripts/jquery-ui.css' rel='Stylesheet' type='text/css' />
                                    <script type='text/javascript' src='../Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js'></script>
                                    <script type='text/javascript' src='../Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js'></script>
                                    <script type='text/javascript' src='../Scripts/jquery/jquery-v1.8.2.min.js'></script>
                                    <script type='text/javascript' src='../Scripts/jquery/jquery-v1.9.1-ui.min.js'></script>
                                    <script type='text/javascript' src='../Scripts/jquery-ui.min.js'></script>
                                    <script type='text/javascript' src='../js/jsextendsession/js/jquery.idle-timer.js'></script>
                                    <script type='text/javascript' src='../js/jsextendsession/js/timeout-dialog.js'></script>
                                </head>
                                <body> ";

                        //Loop through GridView Pages.
                        for (int i = 0; i < GvToExport.PageCount; i++)
                        {
                            //Set the Page Index.
                            GvToExport.PageIndex = i;

                            //Hide Page as not needed in PDF.
                            GvToExport.PagerSettings.Visible = false;

                            //Populate the GridView with records for the Page Index.
                            GvToExport.DataBind();

                            int pagesize = GvToExport.PageSize;
                            int pageNo = (i + 1);
                            int Totpage = GvToExport.PageCount;
                            string htmlbuilder = htmlTextNew(pageNo, pagesize, Totpage);
                            str += htmlbuilder;
                        }

                        str += @" 
                                </body>
                                </html>";
                    }
                }

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
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Approved MET Mass Revision.pdf");
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
            catch (ThreadAbortException ex2)
            {

            }
            catch (Exception ex)
            {
                LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
                EMETModule.SendExcepToDB(ex);
            }
        }

        #region not used

        //public void CreatePDFFromHTMLFile(string HtmlStream, string FileName)
        //{
        //    try
        //    {
        //        object TargetFile = FileName;
        //        string ModifiedFileName = string.Empty;
        //        string FinalFileName = string.Empty;

        //        /* To add a Password to PDF -http://aspnettutorialonline.blogspot.com/ */
        //        iTextSharp.text.Rectangle MyPaperSize = new Rectangle(842, 595);
        //        TestPDF.HtmlToPdfBuilder builder = new TestPDF.HtmlToPdfBuilder(MyPaperSize);
        //        TestPDF.HtmlPdfPage first = builder.AddPage();
        //        first.AppendHtml(HtmlStream);
        //        byte[] file = builder.RenderPdf();
        //        File.WriteAllBytes(TargetFile.ToString(), file);

        //        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(TargetFile.ToString());
        //        ModifiedFileName = TargetFile.ToString();
        //        ModifiedFileName = ModifiedFileName.Insert(ModifiedFileName.Length - 4, "1");

        //        string password = "";
        //        //iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, password, "", iTextSharp.text.pdf.PdfWriter.AllowPrinting);

        //        iTextSharp.text.pdf.PdfEncryptor.Encrypt(reader, new FileStream(ModifiedFileName, FileMode.Append), iTextSharp.text.pdf.PdfWriter.STRENGTH128BITS, password, "", iTextSharp.text.pdf.PdfWriter.AllowPrinting | iTextSharp.text.pdf.PdfWriter.AllowCopy);
        //        reader.Close();
        //        if (File.Exists(TargetFile.ToString()))
        //            File.Delete(TargetFile.ToString());
        //        FinalFileName = ModifiedFileName.Remove(ModifiedFileName.Length - 5, 1);
        //        File.Copy(ModifiedFileName, FinalFileName);
        //        if (File.Exists(ModifiedFileName))
        //            File.Delete(ModifiedFileName);

        //    }
        //    catch (Exception ex)
        //    {
        //        LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        EMETModule.SendExcepToDB(ex);
        //    }
        //}

        //string htmlText(int PageNo, int pagesize, int TotPage)
        //{
        //    string MyhtmlText = "";
        //    try
        //    {
        //        string CurrDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        //        string str = @" <HTML>
        //                        <head>
        //                        </head>
        //                        <body>
        //                            <p style='font-size: 8px; padding-top:15px;'>
        //                                <table border='1'>
        //                                    <thead>
        //                                        <tr> 
        //                                            <th  border='0' style='text-align:left' colspan='14'><h5><strong>e-MET PIR Generation Mass Revision</strong> </h5>  </td>
        //                                        </tr>
        //                                        <tr>
        //                                            <th border='1' style='text-align:center'><b>Material</b></th>
        //                                            <th border='1' style='text-align:center'><b>Material Desc</b></th>
        //                                            <th border='1' style='text-align:center'><b>Quote No</b></th>
        //                                            <th border='1' style='text-align:center'><b>Vendor Name</b></th>
        //                                            <th border='1' style='text-align:center'><b>Director Decision</b></th>
        //                                            <th border='1' style='text-align:center'><b>Director Name</b></th>
        //                                            <th border='1' style='text-align:center'><b>Director Aproved / Rejected Date</b></th>
        //                                            <th border='1' style='text-align:center'><b>Effective Date</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Material Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Process Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Sub Material Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Other Items Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Grand Total Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Final Quote Price</b></th>
        //                                        </tr>
        //                                    </thead>
        //                                    <tbody>";
        //        if (Session["QuoteListToPdf"] != null)
        //        {
        //            DataTable dtQuoteListToPdf = (DataTable)Session["QuoteListToPdf"];
        //            if (dtQuoteListToPdf.Rows.Count > 0)
        //            {
        //                int startfrom = 0;
        //                int MaxRecord = 0;

        //                MaxRecord = PageNo * pagesize;
        //                startfrom = (PageNo * pagesize) - pagesize;

        //                for (int i = startfrom; i < dtQuoteListToPdf.Rows.Count; i++)
        //                {
        //                    if (i < MaxRecord)
        //                    {
        //                        str += @"<tr >";
        //                        for (int c = 0; c < dtQuoteListToPdf.Columns.Count; c++)
        //                        {
        //                            if (c < 8)
        //                            {
        //                                string colName = dtQuoteListToPdf.Columns[c].ColumnName;
        //                                if (colName == "MaterialDesc")
        //                                {
        //                                    str += @"<td style='width:200px'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
        //                                }
        //                                else
        //                                {
        //                                    str += @"<td>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                str += @"<td style='text-align:right'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
        //                            }
        //                        }
        //                        str += @"</tr>";
        //                    }
        //                }
        //                str += @"<tr border='0' style='text-align:right'> 
        //                         <td  border='0' style='text-align:center' colspan='10'> Page " + PageNo + @" of " + TotPage + @"  </td> 
        //                         <td  border='0' style='text-align:right' colspan='4'> Download from e-MET PIR Generation on " + CurrDate + @"  </td> 
        //                         </tr>";
        //            }
        //        }
        //        str += @"                   </tbody>
        //                                </table>
        //                             </P>
        //                        </body>
        //                        </html>";

        //        MyhtmlText = str;
        //    }
        //    catch (Exception ex)
        //    {
        //        MyhtmlText = ex.Message.ToString();
        //    }
        //    return MyhtmlText;
        //}

        //protected void GenerateDesignPdf()
        //{
        //    try
        //    {
        //        string CurrDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        //        string str = @" <HTML>
        //                        <head>
        //                        </head>
        //                        <body>
        //                             <h3 style='text-align: center;'><strong>e-MET PIR Generation Mass Revision</strong></h3>
        //                             <br>
        //                            <p style='font-size: 8px;'>
        //                                <table border='1'>
        //                                    <thead>
        //                                        <tr>
        //                                            <th border='1' style='text-align:center'><b>Material</b></th>
        //                                            <th border='1' style='text-align:center'><b>Material Desc</b></th>
        //                                            <th border='1' style='text-align:center'><b>Quote No</b></th>
        //                                            <th border='1' style='text-align:center'><b>Vendor Name</b></th>
        //                                            <th border='1' style='text-align:center'><b>DDecision</b></th>
        //                                            <th border='1' style='text-align:center'><b>Dname</b></th>
        //                                            <th border='1' style='text-align:center'><b>DAprRejDt</b></th>
        //                                            <th border='1' style='text-align:center'><b>EffectiveDate</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Material Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Process Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Sub Material Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Total Other Items Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Grand Total Cost</b></th>
        //                                            <th border='1' style='text-align:center'><b>Final Quote Price</b></th>
        //                                        </tr>
        //                                    </thead>
        //                                    <tbody>";
        //        if (Session["QuoteListToPdf"] != null)
        //        {
        //            DataTable dtQuoteListToPdf = (DataTable)Session["QuoteListToPdf"];
        //            if (dtQuoteListToPdf.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtQuoteListToPdf.Rows.Count; i++)
        //                {
        //                    str += @"<tr >";
        //                    for (int c = 0; c < dtQuoteListToPdf.Columns.Count; c++)
        //                    {
        //                        if (c < 8)
        //                        {
        //                            str += @"<td>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
        //                        }
        //                        else
        //                        {
        //                            str += @"<td  style='text-align:right'>" + dtQuoteListToPdf.Rows[i][c].ToString() + @"</td>";
        //                        }
        //                    }
        //                    str += @"</tr>";
        //                }
        //                str += @"<tr border='0' style='text-align:right'> <td  border='0' style='text-align:right' colspan='14'> Download from e-MET PIR Generation Mass Revision on " + CurrDate + "  </td> </tr>";
        //            }
        //        }
        //        str += @"                   </tbody>
        //                                </table>
        //                             </P>
        //                        </body>
        //                        </html>";

        //        //insert html into notepad
        //        File.WriteAllText(Server.MapPath("~/Files/" + Session["userID"].ToString() + ".html"), str);

        //        string strHtml = string.Empty;
        //        //HTML File path
        //        string htmlFileName = Server.MapPath("~") + "\\Files\\" + Session["userID"].ToString() + ".html";

        //        //HTML File path to C:/Users/USER/Downloads/
        //        //string htmlFileName = Path.Combine(@"C:/Users/USER/Downloads/") + "report.htm";

        //        //pdf file path.
        //        string pdfFileName = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".pdf";

        //        //pdf filte path to C:/Users/USER/Downloads/
        //        //string pdfFileName = Path.Combine(@"C:/Users/USER/Downloads/") + SRVNumer + refno + ".pdf";

        //        //reading html code from html file
        //        FileStream fsHTMLDocument = new FileStream(htmlFileName, FileMode.Open, FileAccess.Read);
        //        StreamReader srHTMLDocument = new StreamReader(fsHTMLDocument);
        //        strHtml = srHTMLDocument.ReadToEnd();
        //        srHTMLDocument.Close();

        //        strHtml = strHtml.Replace("\r\n", "");
        //        strHtml = strHtml.Replace("\0", "");

        //        CreatePDFFromHTMLFile(strHtml, pdfFileName);

        //        //download pdf
        //        try
        //        {
        //            Response.ContentType = "application/octet-stream";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=Approved MET.pdf");
        //            Response.TransmitFile(("Files/" + Session["userID"].ToString() + ".pdf"));
        //            Response.Flush();
        //            Response.End();
        //        }
        //        catch (ThreadAbortException ex2)
        //        {

        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        finally
        //        {
        //            for (int i = 1; i <= 2; i++)
        //            {
        //                string path = "";
        //                if (i == 1)
        //                {
        //                    path = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".pdf";
        //                }
        //                else
        //                {
        //                    path = Request.PhysicalApplicationPath + "\\Files\\" + Session["userID"].ToString() + ".html";
        //                }
        //                FileInfo file = new FileInfo(path);
        //                if (file.Exists) //check file exsit or not  
        //                {
        //                    file.Delete();
        //                }
        //            }
        //        }
        //    }
        //    catch (ThreadAbortException ex2)
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        //EMETModule.SendExcepToDB(ex);
        //    }
        //}

        //protected void GenerateDesignPdf2()
        //{
        //    try
        //    {
        //        if (Session["QuoteListToPdf"] != null)
        //        {
        //            DataTable dtQuoteListToPdf = (DataTable)Session["QuoteListToPdf"];
        //            if (dtQuoteListToPdf.Rows.Count > 0)
        //            {
        //                string CurrDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        //                GvToExport.DataSource = dtQuoteListToPdf;
        //                GvToExport.PageSize = 9;
        //                GvToExport.DataBind();

        //                //Set the Size of PDF document.
        //                Rectangle rect = new Rectangle(842, 595);
        //                Document pdfDoc = new Document(rect, 10f, 10f, 10f, 0f);


        //                //Initialize the PDF document object.
        //                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //                pdfDoc.Open();

        //                //Loop through GridView Pages.
        //                for (int i = 0; i < GvToExport.PageCount; i++)
        //                {
        //                    //Set the Page Index.
        //                    GvToExport.PageIndex = i;

        //                    //Hide Page as not needed in PDF.
        //                    GvToExport.PagerSettings.Visible = false;

        //                    //Populate the GridView with records for the Page Index.
        //                    GvToExport.DataBind();

        //                    int pagesize = GvToExport.PageSize;
        //                    int pageNo = (i + 1);
        //                    int Totpage = GvToExport.PageCount;
        //                    string htmlbuilder = htmlText(pageNo, pagesize, Totpage);

        //                    //Render the GridView as HTML and add to PDF.
        //                    using (StringWriter sw = new StringWriter())
        //                    {
        //                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //                        {
        //                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //                            StringReader sr = new StringReader(htmlbuilder);
        //                            htmlparser.Parse(sr);
        //                        }
        //                    }

        //                    //Add a new Page to PDF document.
        //                    pdfDoc.NewPage();
        //                }
        //                //Close the PDF document.
        //                pdfDoc.Close();

        //                //Download the PDF file.
        //                Response.ContentType = "application/pdf";
        //                Response.AddHeader("content-disposition", "attachment;filename=eMET Approve Mass Revision.pdf");
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.Write(pdfDoc);
        //                Response.End();
        //            }
        //        }
        //    }
        //    catch (ThreadAbortException ex2)
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        //LbMsgErr.Text = ex.StackTrace.ToString() + " - " + ex.Message.ToString(); DvMsgErr.Visible = true;
        //        //EMETModule.SendExcepToDB(ex);
        //    }
        //}
        #endregion
    }
}