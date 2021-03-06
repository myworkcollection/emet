<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AboutEMET.aspx.cs" Inherits="Material_Evaluation.AboutEMET" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eMET</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Bootstrap core CSS-->
    <%--<link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="Styles/bootstrap-3.4.1-dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />

    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }
    </style>

    <%--<script src="Scripts/jquery.min.js" type="text/javascript"></script>--%>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
        <!-- Loading Screen -->
        <div class="row">
            <div id="loading" class="col-sm-12" style="padding-top: 200px;">
                <img id="loading-image" src="images/loading.gif" alt="Loading..." />
                <div class="col-sm-12" style="text-align: center; opacity: 1;">
                    <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="col-lg-12" style="padding: 5px;">
                        <div class="row">
                            <div class="col-sm-10" style="padding-top: 5px;">
                                <a onclick="ShowLoading();" href="aboutemet.aspx">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                                <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lblUser" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="wrapper">
            <!-- Sidebar-->
            <div id="SideBarMenu" style="width: 300px;" runat="server" class="SideBarMenu">
                <ul class="sidebar">
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <span>Home</span>
                        </a>
                    </li>


                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=2">
                            <i class="fas fa-fw fa-newspaper"></i>
                            <span>Create Request</span></a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Revision.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Revision of MET</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="MassRevision.aspx">
                            <i class="fas fa-fw fa-chart-area"></i>
                            <span>Mass Revision</span></a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                            <i class="fas fa-fw fa-table"></i>
                            <span>PIR Generation</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="PIRGenMassRev.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>PIR Generation Mass Revision</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=18">
                            <i class="fas fa-fw fa-table"></i>
                            <span style="">Log Vendor Password Changes</span></a>

                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="EMasterData.aspx">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Master Data</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="SmnReport.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Report</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="RealTimeVendInvSMN.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Vendor Real Time Inventory</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="aboutemet.aspx">
                            <i class="fas  fa-fw fa-info"></i>
                            <span>About</span></a>

                    </li>
                </ul>
            </div>

            <!-- Sidebar Vendor-->
            <ul class="sidebar" id="MenuListVendor" runat="server" visible="false" style="">
                <li class="sideMenu"><a class="linkMenu" href="Emet_author_V.aspx?num=15"><i class="fas fa-fw fa-tachometer-alt"></i><span>Home</span> </a></li>
                <li class="sideMenu"><a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=16"><i class="fas fa-fw fa-table"></i><span>Master Data</span></a>
                </li>
                <li class="sideMenu"><a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=21"><i class="fas fa-fw fa-key"></i><span>Change Password</span></a></li>
                <li class="sideMenu"><a class="linkMenu" onclick="ShowLoading();" href="aboutemet.aspx"><i class="fas fa-fw fa-info"></i><span style="">About</span></a>
                </li>
            </ul>

            <!-- content -->
            <div id="content-wrapper">
                <div class="container-fluid">
                    <div class="col-md-12 card">
                        <div class="col-md-12 card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-info"></i>&nbsp; About
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger fa-pull-right" OnClick="btnclose_Click" />
                            </div>
                        </div>
                        <div class="col-md-12 card-body">
                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 3.4 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label29" runat="server">
                                     <font color="#009DDD"><b>Date : 2021-03-26 </b></font> <br />
                                        - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-151"> EMET-151 </a> 
                                       e-MET > SMN Login > Dashboard > When user access to "All Request" , "Completed Request" and "Vendor Real Time Inventory" pages > Do not load all records > Show Filter Option to allow user to select<br />
                                        - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-152"> EMET-152 </a> 
                                       e-MET > SMN Login > Report > Taking too long to display data (Performance Issue)<br />
                                        - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-165"> EMET-165 </a> 
                                       e-MET > All Page File Attachment > File name do not allow special char and file size must be less than 3MB  <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 3.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label28" runat="server">
                                     <font color="#009DDD"><b>Date : 2021-03-08 </b></font> <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-120"> Jira EMET-120 </a> 
                                       E-MET > Revision of MET > Added multiple quotation > Click "Create Request" > Long processing time and failed to display > Auto exit e-MET application after long waiting time > User failed to submit<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-122"> Jira EMET-122 </a> 
                                       E-MET > Revision of MET > Tool And Machine Amortize : Add Option No Change under selection Tool / Machine Amortize<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-129"> Jira EMET-129 </a> 
                                       e-MET > Vendor Inventory > Add new function to allow Template upload for Raw Material (For Eg. Stamping Mother Coil) that has no Material Code and PIR No. & Matl Code in Cust vs Matl Pricing belongs to the Vendor<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-149"> Jira EMET-149 </a> 
                                       e-MET (SPL QA Server) > Vendor login > Vendor Inventory page > Edit > “Updated Date” field show as "Invalid date" > After some time then “Updated Date” field show today date > Why there is a delay in showing data in Updated Date field?<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-158"> Jira EMET-158 </a> 
                                       e-MET > Vendor Login > Vendor Inventory > Add index page to show 1 record of this Vendor Code / Name / Last Updated Date with the checkbox for viewing the records (similar to SMN login new index page format)<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-153"> Jira EMET-153 </a> 
                                       e-MET > SMN Login > Vendor Inventory > Add index page of the list of Vendor Code / Name / Last Updated Date with the checkbox for viewing the records. <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-157"> Jira EMET-157 </a> 
                                       e-MET > Vendor login > Process Cost > Standard Rate / HR and Vendor Rate / HR field not showing data (When Web MDM > Vendor Machine List vs HR Rate >  Either "Machine ID" or "Machine Description" fields contain Chinese Characters)<br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 3.2 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label27" runat="server">
                                     <font color="#009DDD"><b>Date : 2021-01-11 </b></font> <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-145"> Jira 145 </a> 
                                       e-MET > New Request (Without SAP Code) > Response Date (today) > System auto close quotation (today) > Why system auto close quotation before Response Date over 7 days<br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-118"> Jira 118 </a> 
                                       e-MET > Material Cost Calculation formula is not correct when Recycle Ratio (%) = Follow Runner Ratio<br />

                                    <font color="#009DDD"><b>Date : 2021-01-11 </b></font> <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-139"> Jira 139 </a> 
                                       e-MET > Vendor Login > Real Time Inventory > Upload Excel template contain Stock = - (Vendor put "-" instead of a number) > Failed to upload and top page show error message <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-143"> Jira 143 </a> 
                                       e-MET > SMN login > "Vendor Real Time Inventory" page > “Total Record” is not showing correctly the total number of Inventory records uploaded by Vendor  <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-142"> Jira 142 </a> 
                                       e-MET > SMN login > "Vendor Real Time Inventory" page >  Long Processing time  when user change "Show 10 entries" to Total Record (991)  <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-144"> Jira 144 </a> 
                                       e-MET > SBM submitted "New" quotation request > SMN MGR Reject to Resubmit > Receive “Resubmit” Email > Why rejected items cannot be found as “Draft” in SBM login and Quotation status become "Close" as being rejected by SMN DIR? <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-141"> Jira 141 </a> 
                                       e-MET > Vendor Inventory > Vendor uploaded data > "Created By" field did not display Vendor User Name but show as "-" <br />

                                    <font color="#009DDD"><b>Date : 2020-12-31 </b></font> <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-124"> Jira 124 </a> 
                                        e-MET > SMN Login > All Request page > Filter > Select from drop down (For Eg Vendor Name) > User Key in some text > System will auto clear the field > User always need to 2nd time key in the required text <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-123"> Jira 123 </a> 
                                         e-MET > Revision of MET > Material Code under Plant Status = Z4 & Z9 > System to prompt alert message and do not allow SMN Requestor to submit <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-136"> Jira 136 </a> 
                                         e-MET > Vendor Login > Process = IM > Part III : Process Cost > Untick "Follow Material Base Qty / Cavity > Why Cost Calculation in Process Cost Grid still following Material Base Qty? <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-137"> Jira 137 </a> 
                                         e-MET > Vendor Login > Process = ST > Part III : Process Cost > Input all required Data > Click "Calculate" button > System prompt incorrect alert message "Only allow number, Please check Duration Process UOM value at column 1!" <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-132"> Jira 132 </a> 
                                         e-MET > Vendor Login > Part III : Process Cost > Base Quantity > Add check box > Untick to allow Vendor change value to be different from Material Cost Base Qty for ALL Process  <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-138"> Jira 138 </a> 
                                         New Request Page Min Input 2char to get SAP Mat Code <br />

                                    <font color="#009DDD"><b>Date : 2020-12-29 </b></font> <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-135"> Jira 135 </a> 
                                         - fix issue Vendor Login > Part III : Process Cost > Remove Subcon name > Previous column data went missing <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-133"> Jira 133 </a> 
                                         - draft page : set back to total cost field to empty if never calculate <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-131"> Jira 131 </a> 
                                         - fix issue Vendor Login > To Be Filled By Vendor > Attachment > Hide “Undo” icon <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-130"> Jira 130 </a> 
                                         - fix issue Vendor Login > Part III : Process Cost > If Subcon - Subcon Name field > Vendor did not input any value but press keyboard "space" > System did not prompt any alert message and still allow user to submit > Subcon Name field show empty <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-127"> Jira 127 </a> 
                                         - fix issue Vendor login > Part III : Process Cost > Machine / Labor field > Vendor selected "Labor" > Machine field become active to allow Vendor to input text? <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-134"> Jira 134 </a> 
                                         - fix issue Vendor Login > Part III : Process Cost > If Turnkey - Sub Vendor Name field > Vendor input Turnkey Cost / pc > Click Calculate > System prompt incorrect alert message "Please Enter Duration per Process UOM (Sec) !! <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-126"> Jira 126 </a> 
                                         - fix isue New Request > DIR Approved > Part III : Process Cost > "Machine" field show empty, solution : validate all column in all table process cost to make sure machine selected if Sub Process is empty, or If Turnkey- Sub vendor name not  selected, Machine /Labor selected is machine. before submit  <br />
                                    - <a href="https://shimanodt-apps.atlassian.net/browse/EMET-124"> Jira 124 </a> 
                                         - SMN Login > All Request page > Filter > Select from drop down (For Eg Vendor Name) > User Key in some text > System will auto clear the field > User always need to 2nd time key in the required text, solution : for lighten up system, will search the data only when button search click.  instead search data for every  filter type change. with this above issue will fixed <br />
                                    

                                    <font color="#009DDD"><b>Date : 2020-12-11 </b></font> <br />
                                    - e-MET > SMN login > Create Request > Without SAP Code and Without SAP Code (GP) > Unlock "Due Dt Next Rev" field to allow user to change date  <br />
                                    - E-MET > Revision of MET > Tool And Machine Amortize <br />
                                    - All Process > Do validation check on Base Qty field cannot be NULL or zero <br />
                                    - Process = IM > Part III : Process Cost > Base Qty = NULL or zero > System did not prompt alert msg<br />
                                    - New Request > System cannot detect previous approved Mass Rev > Allow to submit a "New" Request <br />
                                    - Vendor login > Part III : Process Cost > Duration per Process UOM (Sec) field should be not accept decimal places <br />
                                    - Revision of MET > Added multiple quotation > Click "Create Request" > Long processing time and failed to display > Auto exit e-MET application after long waiting time > User failed to submit <br />
                                    - Email Notification > Global Setting > Email Content need to indicate "Plant"<br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 3.1 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label26" runat="server">
                                    <font color="#009DDD"><b>Date : 2020-11-19 </b></font> <br />
                                    - Display machine id and description under submission page table sub material cost   <br />
                                    - Display Tool Amortize id and description under submission page table sub material cost   <br />
                                    - Fix issue : populate 2 Machine Amortize column when Part III Process Cost only selected 1 Machine ID  <br />
                                    - Vendor Realtime Inventory : Set remark as mandatory when edit both for upload and manual edit  <br />
                                    - Vendor Realtime Inventory : Set Qty to "0" if empty during create request <br />
                                    - Vendor Realtime Inventory : Fix issue slow performance after edit data manually <br />

                                    <font color="#009DDD"><b>Date : 2020-11-09 </b></font> <br />
                                    - Display "use tool Amortize" info under submission and review page   <br />

                                    <font color="#009DDD"><b>Date : 2020-11-05 </b></font> <br />
                                    - remove “Use Tool Amortize” field  from Create Request > Without SAP Code and Without SAP Code (GP) Page.   <br />
                                    - “Machine Amortize” column > “Sub-Mat/T&J Description” field > Display “Machine ID” concatenate “Machine Description”   <br />
                                    - Fix issue Un-Request(Un-submited request) display under without sap code and without sap code GP page (SMN and Vendor Login).  <br />

                                    <font color="#009DDD"><b>Date : 2020-10-28 </b></font> <br />
                                    - Add New Form Real Time Vendor Inventory for SMN and Vendor Side  <br />

                                    <font color="#009DDD"><b>Date : 2020-09-21 </b></font> <br />
                                    - Add Machine Amortization And Tooling Amortization During Create Request  <br />
                                    - 1 Quotation submitted to multiple Vendor > When SMN Manager rejected for all Vendor > Approval workflow will end  <br />
                                    - Mass Revision Show ALL Material Cost column <br />
                                    - Revision of MET > To include Mass Revision items <br />
                                    - PIR Generation Mass Revision PDF Summary <br />
                                    - Encrypt Configuration info user,password,dbname, and server adress from configuration file  <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 3.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label24" runat="server">
                                    <font color="#009DDD"><b>Date : 2020-09-30 </b></font> <br />
                                    - Lighter grey box colour display in all related screen  <br />
                                    - add character "/" after raw material cost  <br />

                                    <font color="#009DDD"><b>Date : 2020-08-19 </b></font> <br />
                                    - IM Recycle Ratio field will display in SMN Submission Page when the Process is being assigned to "IM Layout"  <br />

                                    <font color="#009DDD"><b>Date : 2020-08-07 </b></font> <br />
                                    - e-MET > IPO/PDA - To mass revise the IM Raw material for Duracon M90 Nat & Keptal F20 Nat to remove Master Batch Black  <br />

                                    <font color="#009DDD"><b>Date : 2020-07-23 </b></font> <br />
                                    - e-MET > "DIR Approval Pending" page > Control by SMN DIR WIN login ID to view only their assigned "Product" Quotation  <br />

                                    <font color="#009DDD"><b>Date : 2020-07-22 </b></font> <br />
                                    - After SMN add new announcement > Vendor login > Redirect to Announcement Page > Alert Vendor to see unread announcement  <br />

                                    <font color="#009DDD"><b>Date : 2020-07-13 </b></font> <br />
                                    - System cannot detect previous “New” Approved MET and allow SMN Requestor to submit another “New” Request with same SAP code for same Vendor <br />
                                    &nbsp;&nbsp;*logic only for case single vendor selected for multiple vendor selected keep allow to create request <br />

                                    <font color="#009DDD"><b>Date : 2020-07-09 </b></font> <br />
                                    - migrate the eMET BOM table from BOM Explosion to BOM list table<br />
                                    - update logic query to get reason for emet based on systemcode, then update data in table TREASONFORMETREJECTION for field system empty/null set into EMET.<br />
                                    - display unit information inside table BOM list before UOM column under review and submission page  <br /> 
                                    - change logic to get met field from tMETFieldsCondition(old table)  to tMETFieldsConditionNew(new table)  <br /> 
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.9 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label21" runat="server">
                                    <font color="#009DDD"><b>Date : 2020-07-20 </b></font> <br />
                                    - Change "Raw Material Cost > UOM" field from text box to become drop down box > Get data from "MDM > e-MET > PIR Default Value" table <br />

                                    <font color="#009DDD"><b>Date : 2020-07-14 </b></font> <br />
                                    - Fix issue display cost details under SMN Requestor Login when all vendor submit for 1 request with multiple vendor<br />

                                    <font color="#009DDD"><b>Date : 2020-07-07 </b></font> <br />
                                    - Fix issue Without SAP Code can see Vendor pricing before Response Due Date expired<br />

                                    <font color="#009DDD"><b>Date : 2020-06-29 </b></font> <br />
                                    - PIR Generation stored procedure changed  for 'valid to' and 'Duedate' purpose<br />
                                    - set value day length after request-response due date expired to auto-close the quotation.<br />
                                    - Create Request > Without SAP Code & Without SAP Code (GP) >  "Effective Date" as optional field input by SMN Requestor & "Due Date" field direct populate from PIR Default Table  <br /> 
                                    - Fix issue unwanttted column in vendor report : e-MET Dev Server > Vendor Login > Report > Export to Excel > Remove 8 extra column<br />
                                    - e-MET Dev Server > Rev of MET > Vendor login > BOM Table show New Raw Material Code > But Part II : Material Cost 1st column still showing Old Raw Material Code<br />

                                    <font color="#009DDD"><b>Date : 2020-06-06 </b></font> <br />
                                    - User Dept in Transaction Table should not linked to MDM User Dept <br />
                                    - wrong value was taken for Raw Material Cost/ kg in material cost table<br />
                                    - Revise "Exch Rate" field to display 4 dec plc <br />
                                    - Display "Plating Type" field in all Quotation Type Detail Page <br />
                                    - Rename 2 field name > Part II : Material Cost > "Material SAP Code" become "Raw Material SAP Code" and "Material Description" become "Raw Material Description"<br />
                                    - Create Request > Without SAP code (GP) > Rename "Requestor Plant" field name to become "GP Request Plant".<br />
                                    - Create new table in EMET DB to save information SMN BOM raw material related material and vendor during create request by SMN PIC  <br />
                                    - set value day length after request-response due date expired to auto-close the quotation.<br />
                                    - Report to export all cost details<br />
                                    &nbsp;&nbsp;*Create New Page for Report SMN Side<br />
                                    &nbsp;&nbsp;*Create New Page for Report Vendor Side<br />
                                    
                                    <font color="#009DDD"><b>Date : 2020-05-22 </b></font> <br />
                                    - Create Request > Without SAP Code & Without SAP Code (GP) >  "Effective Date" as optional field input by SMN Requestor & "Due Date" field direct populate from PIR Default Table <br />
                                    &nbsp;&nbsp;*Scenario 1 : Scenario 1 : “Effective Date” field when SMN PIC left blank and submit,in Vendor Page to allow  Vendor to submit even “Effective Date” field is left empty for both without SAP Code and Without SAP Code (GP) and set to disabled.<br />
                                    &nbsp;&nbsp;*Scenario 2 : “Effective Date” field if SMN PIC selected a date then submit, at Vendor Page this field will be disable and display as per SMN PIC input.<br />

                                    <font color="#009DDD"><b>Date : 2020-05-05 </b></font> <br />
                                    - Due date next revision value get from master data PIR default value <br />
                                    &nbsp;&nbsp;*set due date text box into disabled in vendor submission page, when the date value is in the future, if not then set to enabled and validate to makes sure the date selected by a user not in the past.<br />
                                    &nbsp;&nbsp;*under manager approval, if manager want to change the date, keep the due date enabled and validate to makes sure the date selected by a user, not in the past.<br />

                                    <font color="#009DDD"><b>Date : 2020-05-01 </b></font> <br />
                                    - Add Masterdata for non mandatory field (Sub material cost & Other Cost) <br />
                                    &nbsp;&nbsp;*Add 2 new table (Sub Mat Cost & Other Cost) in MDM then linked to e-MET Application to allow Vendor select option from drop down. Remove Vendor input free text function. Currently free text no standardisation so difficult to pull data for analysis. Sometime Vendor input info wrongly to the incorrect cost section<br />
                                    - Recycle Material Ratio based on PIC selected during create request<br />
                                    &nbsp;&nbsp;*Add dropdown Recycle Ratio (%) in new request page, and this dropdown only show when process group selected is “IM“<br />
                                    &nbsp;&nbsp;*In Vendor submission page<br />
                                    &nbsp;&nbsp; &nbsp;&nbsp;1. if recycle ratio selected by PIC is “Follow runner ratio“ then Recycle Material Ratio (%) value in table material cost will always equal with Runner Ratio/pc (%).<br />
                                    &nbsp;&nbsp; &nbsp;&nbsp;2. if the recycle ratio selected by PIC is bigger than  Runner Ratio/pc (%) value then Recycle Material Ratio (%)  in material cost table value will follow the Runner Ratio/pc (%)  value in material cost table , if not then follow the Recycle Ratio (%) was selected by PIC <br />
                                    - Raw Material Cost & Tot Material Cost rows in table material cost display the uom beside of the value for each column.<br />
                                    &nbsp;&nbsp;*following Raw Material Cost if “Raw Material Cost UOM“ is not KG  and UOM following Raw Material Cost UOM<br />
                                    &nbsp;&nbsp;*Divide by 1000 if “Raw Material Cost UOM“ is KG and UOM Set to G<br />
                                    
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.8 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label11" runat="server">
                                    
                                    <font color="#009DDD"><b>Date : 2020-03-24 </b></font> <br />
                                    - Add 2 New Optional Date field caption as "FA submission Date" and "1st Production Date" under Create Request for both with SAP and without SAP<br />
                                    -  Modify Without SAP Code (GP) layout 2 fields "Packing Requirement" and "Others Requirement" to increase max. no. of character from 20 to 100 <br />
                                    
                                    <font color="#009DDD"><b> Date : 2020-03-18 </b></font><br />
                                    - All Total Cost field change from 4 to 5 Decimal Place <br /> 
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.7 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label25" runat="server">

                                    <font color="#009DDD"><b> Date : 2020-01-17 </b></font><br />
                                    - Add Request Purposes information in export pdf at PIR generation page <br /> 
                                    - Add Masterdata tab in e-MET left pane under SMN Login to view all Vendor process and Machine details including rate <br /> 
                                    - Add “Mgr request Resubmit” inside SMN Res Status Filter Drop Down selection for both SMN and Internal SBM login “All Request” page* This “Mgr request Resubmit” drop down option do not need show for External Vendor login <br /> 

                                    <font color="#009DDD"><b> Date : 2020-01-14 </b></font><br />
                                    - Display pop up form when same vendor and same material have request pending and expired to let user decide for change the response due date or reject the request during create new request page with SAP Code and Revision of eMET page <br /> 
                                    -  Fix issue text field can put text in field with condition decimal/int number only with short cut key (ctrl + v)<br /> 

                                    <font color="#009DDD"><b> Date : 2020-01-02 </b></font><br />
                                    - Add "All Request" page under Vendor Login <br /> 
                                    -  SMN Manager & Director approval page : Add easy identification of Vendor input field<br /> 

                                    <font color="#009DDD"><b> Date : 2019-12-23 </b></font><br />
                                    - Stamping Material Scrap Weight (g) field control by formula using Material Gross Weight/pc (g) deduct Part Net Unit Weight (g) <br /> 
                                    -  Only for SBM Front Process request, SMN Manager can reject or change status to save as draft<br /> 
                                    -  Update change effective date by manager condition <br /> 
                                        &nbsp;&nbsp;* If manager change effective date <br /> 
                                        &nbsp;&nbsp;* when effective date have difference material price based on date selected display pop up : <br /> 
                                        &nbsp;&nbsp;&nbsp;** IF user WANT TO CONTINUE and display new price based on mat vs customer pricing  and new effective date was selected,<br /> 
                                        &nbsp;&nbsp;&nbsp;** IF manager select NO return to condition for select the effective date again, if YES insert a new date was select into history of effective date change by manager<br /> 
                                    -  After SMN Manager reject, status change to "end", request only applicable to quotation request if having 1 Vendor<br /> 
                                    - Add tick box to allow Vendor change base quantity under Process Grid (For Injection Molding and Stamping) <br /> 
                                    - add validation  duration per process uom only integer not decimal for process cost table <br /> 
                                    - new condition layout for Raw Material Quotation <br /> 

                                    <font color="#009DDD"><b> Date : 2019-12-04 </b></font> <br />
                                    - Material Cost - data entry to be activated based on Proc type F or F30 : <br /> 
                                      If SPPROC type is 30, then by default Material cost should be Zero and disable the entry to the fields. But some special scenario may need to add Material cost, so enable it with checkbox and layout to follow according to Process Group ID <br />
                                    - Add Field effective date in New Request page With SAP Code with default value is system date, editable and mandatory.<br />
                                    - Add Field effective date in Revision of MET Page with default value is system date, editable and mandatory.<br />
                                    - if SAP part code was selected have BOM, the BOM data must based on effective date was selected.<br />
                                    - During manager approval at decision form, if manager want to change the effective date, New effective Date cannot be earlier than effective date key in by the Requestor.<br />
                                    - add information request date at SMN Review and submission page under Label SHIMANO DETAIL<br />
                                    - adding validation cannot update due date if a new request has been made with the same material and vendor data for page : Request_Waiting.aspx and RevisionReq_waiting.aspx<br />
                                    - Fix issue invalid data show at review page SMN side (quote cost plant) in Table Previous Quotation where Data show rejected request .<br />
                                    -	Revision of eMET:<br />
                                    &nbsp;&nbsp;* create new request based on previous approval quotation<br />
                                    &nbsp;&nbsp;* new dashboard and request list for revison of emet request<br />
                                    &nbsp;&nbsp;* new condition for submission page for revision of emet request with enabled or disabled table mat cost,proc cost, sub mat cost and other cost<br />
                                    &nbsp;&nbsp;* add validation during create request in New request With SAP code and Revision of EMET with condition Same material with same vendor can’t be created until the existing quotation is approve or reject.  Or reject due to Quote response date expired<br />
                                    &nbsp;&nbsp;* update form revision of emet add new drop down for process group filter and exclude option processgrp code and process grp desc in filter by list <br />
                                    &nbsp;&nbsp;* update form revision of emet add filter based on sub process with condition only can select after select the drop down process group <br />
                                    &nbsp;&nbsp;* update form revision of emet change Attachment into optional <br />
                                    &nbsp;&nbsp;* update form revision of emet : confirmation pop up add more data to the list using pop up form not default browser confirmation box <br />
                                    &nbsp;&nbsp;* update form revision of emet : change from add link to show detail for quote reference into : display pop up form with display detail quote reference <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.6 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label23" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-11-21 </b></font> <br />
                                    - add filter based on differen in mass revision approval page<br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.5 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label22" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-11-18 </b></font> <br />
                                    - Display Information "SMN Submit by" for mass revision request for all related page <br />
                                    - Update form submission page : fix issue raw material cost / kg value not same with amount price raw material in BOM grid  <br />
                                     - Material Cost - data entry to be activated based on Proc type F or F30 <br />
                                    - Display BOM detail on review page <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->


                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.4 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label20" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-10-23 </b></font><br />
                                    - add validation for material cost table during submission,<br />
                                    all field  mandatory (cannot be 0 or null) except special condition for: <br />
                                    <br />
                                    Process Group :IM<br />
                                    1. Part Unit weight ≠ 0 g <br />
                                    2. runner weight/shot ≠ 0 g <br />
                                    3. recycle ratio can be 0% but user need to key in <br />
                                    4. base qty ≠ 0 g <br />
                                    5. melting loss default = 5 % (cannot null) -> editable <br />
                                     <br />
                                    Process Group : Casting <br />
                                    1. Melting Loss default = 10 % (cannot null) -> editable <br />
                                     <br />
                                    Process Group : Stamping <br />
                                    1. Density default = 7.86 (non editable) <br />
                                    2. Material Loss ≠ null <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label19" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-10-17 </b></font><br />
                                    - new dashboard box and new page for mass revision request list at vendor side - <br />
                                    - label counting for submit and waiting for submission for mass revison dashboard employee side <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label18" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-10-16 </b></font><br />
                                    - Manager approval - Final cost - Add one more column - %difference - <br />
                                    - employee submission page for mass revision request when submit or save as draft data always save as a draft, and the request not go to next level, and send the email to the vendor <br />
                                    - page request waiting vendor for request type mass revision - draft , display grid like manager approval grid mass revision, without reject option,and then send email to SMN after vendor confirm, and request will go to manager approval level <br />
                                    - manager approval dont send email, only for DIR approval. <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label17" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-10-14 </b></font> <br />
                                    - New page to create new request for mass approval <br />
                                    - New dashboard and req list <br />
                                    - Submission request by SMN not by vendor <br />
                                    - Separate data in mng/dir page to do mass approval <br />
                                    - New Page for PIR Generation Mass Revision <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label16" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-10-01 </b></font><br />
                                    - display header information in quote reference pop up form for quote list type is external or SBM ex : find quote reference(external) <br />
                                    - fix issue typo : changeMng chg to Mgr and submission chg to Submission <br />
                                    - display header information in pop up form find quote reference for quote list type is external or SBM ex : find quote reference(external) <br />
                                    - fix issue calendar not showing in without sap code GP page when page first load <br />
                                    - seperatenew request and revision list dashboard(employee side) <br />
                                    - new page revision request list <br />
                                    - new column information for request status (New or Revision) for all quote request list table <br />
                                    - new status for Quote Request With SAP Code and task status is : Revision, and Revison Draft<br />
                                    - display information which cost was revisewith disabled check box and populate all old data to the table for review form and vendor submission form <br />
                                    - update form vendor submission populate country of origin in new request page for revision request and disabled the drop down <br />
                                    - Change label quote no and quote no reference to new and old quote no for all related page <br />
                                    - fix issue logout not clear the session and other tabs opened still active <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label15" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-09-24 </b></font><br />
                                    - Auto close request without sap code and without SAP Code (GP) if quote response due date expired <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.3 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label14" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-09-18 </b></font><br />
                                    - New Page All Request List<br />
                                    - Allow Vendor to add attachment during submission and can download back to review after submitted by vendor<br />
                                    - Update Revison of eMET : add check box on quote reference list pop up form, add option filter for quote no, smn pic and department<br />
                                    - Update Revison of eMET : remove nested grid from quote list main page and display field control in one grid
                                    - New Option Others for rejection reason when apr/rej form by manager and DIR <br />
                                    - Display different field for manager and DIR apr/rej date, name, remark and reason for rejection <br />
                                    - new design for all table with multi level header <br />
                                    - New Page Revision <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.2 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label13" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-09-17 </b></font><br />
                                    - fix issue sorting function not correct for request date and response date <br />
                                    - fix issue manager approval -> decision page : date icon not active <br />
                                    - change title dashboard box from approval completed to "Completed request" and page header form approval and reject list closed request <br />
                                    - fix issue manager approval decision page : due date value missing after check and uncheck checkbox change date if vendor is SBM <br />
                                    - Add option filter process group Code and process group desc for all table under dashboard menu for employee and vendor side
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.2 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label12" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-09-12 </b></font><br />
                                    - fix issue attachment missing in mail development link during creation request <br />
                                    - fix issue button decision showing for employee user for approval page <br />
                                    - approval completed page: <br />
	                                    &nbsp;* approval reason if empty then : "Accepted By : user name was updated" <br />
	                                    &nbsp;* add filter status rejected or approved<br />
                                    - remove icon asc and desc when sorting for all table and change into underline as default and when click the header set color to yellow <br />
                                    - add button expand and collapse for table with multi level <br />
                                    - Add export to pdf in PIR generation page <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.1 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label10" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-09-10 </b></font><br />
                                    - Rename Title All Page Based On Dashboard Box Title for Sub Page <br />
                                    - Wrap text for table content
                                    - Change Title Quote Request (GP) to Quote Request Without SAP Code (GP) to all related page
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.1 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label9" runat="server">
                                   <font color="#009DDD"><b> Date : 2019-09-10 </b></font><br />
                                    - Add Header for New Request page <br />
                                    - Rename Reason to request purpose and remark into request purpose to all related page <br />
                                    - Fix issue vendor submission  page (new & draft) at table process cost cannot input machine name after select sub process for without sap and without (gp)  condition <br />
                                    - Update form without SAP code , material class desc not filter based on material type, plant status, SAP Proc Type, SAP Sp Proc Type, and set material type, plant status, SAP Proc Type, SAP Sp Proc Type, as optional<br />
                                    - Add note at bottom table material for cavity material cost change after have process cost has value <br />
                                    - Fix issue invalid req approval , data not show after select filter <br />
                                    - Fix issue invalid labor cost form multiple plant vendor <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.1 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label8" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-09-10 </b></font><br />
                                    - New Request Without SAP Code (GP) no need go through approval Workflow <br />
                                    - New Box at Dashboard (vendor and employee side) Without SAP Code (GP)<br />
                                    - New Page For list Without SAP Code (GP) For Vendor And Employee side <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.1 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label6" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-09-10 </b></font><br />
                                    - Change formula and add validation for runner ratio/pcs (%) for process group IM <br />
                                        &nbsp;*Formula : Runner Ratio/pc (%) = Runner Weight/(Unit Weight X Cavity + Runner Weight) <br />
                                    - Update New request page to set auto check select all in vendor list grid, when all vendor selected, and auto Uncheck select all if not all select <br />
                                    - Add List item  “Others” in reason drop down when create new request, when select Other Display Text box for free text (for case reason not in list or maintain) <br />
                                    - Pull past 3 approved quotation show as link for price comparison at "Quote Cost Plan" page <br />
                                        When click "Quote No" link will display all quotation field details. <br />
                                        column required = Quote No, Date, Material, Process, Sub Mat'l and Others <br />
                                    - New Request without SAP code no need go through approval Workflow <br />
                                    - New Box at Dashboard (vendor and employee side) without SAP code <br />
                                    - New Page For list SAP without SAP Code For Vendor And Employee side <br />
                                    - Allow attachment when SMN Staff or Administrator add a new announcement <br />
                                    - Add drop down plan in login page <br />
                                    - quote response due date header change to : response date <br /> 
                                    - vendor : home: announcement notification: change "new Announcement" -> new, if read all, show "" <br /> 
                                    - vendor side for without SAP COde labor/machine at table process cost free flow can entry machine name and rate <br />
                                    - add new box at dashboard for display data count Title: Code Without SAP Code (Vendor side and employee) <br />
                                    - add new page at vendor and employee side to display data whitout SAP Code list , with last column status for submit and without submit for Employee side and status new,draft,submitted for vendor side <br />
                                    - change approval date into Updated Date <br />
                                    - in comparison page under Summarize, now the process is sort based on alphabetical order. Show it base on sequence as entered <br />
                                    - SMN Requestor "Remark" field change to drop down list with reason for creation <br />
                                    - set all the value of total is 4 digit and detail 6 digit, doesnt matter the value is integer, just convert to decimal with 4 digit for total value and 6 digit for detail at  all the page example (0.000000(detail Value) and 0.0000 (Total Value)) <br />
                                    - employee side for create request page : add radio button for vendor type -> external and SBM, when Vendor Type selected SBM select vendor data form table SBM Pricing Policy , when selected external display list vendor not register into SBM Pricing Policy <br />
                                    - new table unit price structure for vendor type SBM <br />
                                    - Dynamic page size allocation for all page <br />
                                    - comparison : tabs sumarize add column "profit & discount" and "Actual Process Cost" between column process detail and Final Process Cost <br />
                                    - New Pop Up Form For approval by Manager And approval by DIR <br />
                                    - Fix issue calendar navigation <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label7" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-08-14 </b></font><br />
                                    - approval page: Rename the column for decision approval table as below<br />
                                        &nbsp;&nbsp;1.Currency = CURR <br />
                                        &nbsp;&nbsp;2.Net Profit/Discount = Prof/Disc <br />
                                        &nbsp;&nbsp;3.Cut vendor Description/name lengthinside of table <br />
                                        &nbsp;&nbsp;4.standard rate and vendor rate 2 digit at tabs process cost <br />
                                        &nbsp;&nbsp;5.Tot raw mat cost/g 4 digit  <br />
                                    - New Design And condition for approval <br />
                                    - Rename : All Form related :<br />
                                        &nbsp;&nbsp;1.SMN Quote effective Date = effective Date <br />
                                        &nbsp;&nbsp;2.SMN Due Date for Next Revision = Due Dt Next Rev <br />
                                        &nbsp;&nbsp;3.New SMN Quote effective Date = New effective Date <br />
                                        &nbsp;&nbsp;4.New SMN Due Date for Next Revision = New Due Dt Next Rev <br />
                                    - Align Rightfor text type is number <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- Start -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label5" runat="server">
                                    <font color="#009DDD"><b>Date : 2019-07-08 </b></font><br />
                                    - compare: wrong currency value(current display country of origin) <br />
                                    - approval page : data not updated after approveor reject <br />
                                    - quote response due date header change to : response date <br />
                                    - vendor : home: announcement notification: change "new Announcement" -> new, if read all, show "" <br />
                                    - vendor side ; exclude column vendor code and name from the table and drop down filter <br />
                                    - table process cost :  all field condition disabled before process group and sub process selected <br />
                                    - rename : Effective from as "effective Date" and Due On as "Due Dt Next Rev" <br />
                                    - PIR job type is getting default based on first record in the table PIRJobType vs Process Group if only one record for the process group.<br />
                                      if process group having more than one record then don't default, prompt user to select PIR Job Type before Create request <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- end -->


                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label4" runat="server">
                                   <font color="#009DDD"><b> Date : 2019-07-01 </b></font><br />
                                    - Add validation for select process group at page new request change and review request for table process cost <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 2.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label3" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-06-26 </b></font><br />
                                    - freeze column material- until mat desc, proc cost until sub process, Sub-Mat/T&J until Sub-Mat/T&J Description, and other cost until Items Description <br />
                                    - Total value set to align right and rest column after total other using same align with total other cost column, 4 digit behind decimal point at comparison page, and table detail display capital letter <br />
                                    - fix bug some value cut off when display data at dynamic table cost at quote cost plan and view request page <br />
                                    - fix bug data not display based on user plant at approval page <br />
                                    - remove link about at vendor side <br />
                                    - display system version no at the header <br />
                                    - add department at all filter and page based on smn PIC request id <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- start End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black;">
                                    <p style="text-align: left;">
                                        <b>Version 1.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label2" runat="server">
                                   <font color="#009DDD"><b> Date : 2019-06-24 </b></font> <br />
                                    - add filter by smn PIC and smnPIC information for all page vendor and shimano emp <br />
                                    - new request page : process group drop down include description <br />
                                    - freeze column at comparison until the currency column <br />
                                    - add form about <br />
                                    - All page data will show based on User Plant = All data should be show based on user plant was register for all the page <br />
                                    - Contact Number field not in used so rename to Plant & Dept (based on SMN Requestor login ID) 1 field (Contact Number) split to 2 field (Plant & Dept) Plant field show "2100 - SPL", Dept show "PDA" <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- start End version content -->

                            <!-- start new version content -->
                            <div class="col-md-12" style="padding: 0px 0px 10px 10px;">
                                <div class="col-md-12" style="border: 1px solid black">
                                    <p style="text-align: left;">
                                        <b>Version 1.0 </b>
                                    </p>
                                </div>
                                <div class="col-md-12" style="border: 1px solid black">
                                    <asp:Label ID="Label1" runat="server">
                                    <font color="#009DDD"><b> Date : 2019-06-18 </b></font><br /> 
                                    - change label for Efficiency/ProcessYield(%) / Efficiency/Process Yield(%) become Efficiency at table process cost<br />
                                    - new request : fix issue at page missing data after alert validation : Mnth.Est.Qty & Base UOM:<br />
                                    - Vendor login > Click "Master data" tab > Default display 100 record in 1 page <br />
                                    - Comparison :add more column for currency for all tabs at comparison <br />
                                    - Comparison :when selected Process group = SF but display wrong Material Description. Need to fix<br />
                                    - comparison :multiple data was submit in comparison page not show <br />
                                    - comparison :Vertical Alignment of column header in detail table should always on top and total value at the bottom <br />
                                    - announcement : fix label Announcements at vendor page <br />
                                    - announcement : Change Created by to username instead user id <br />
                                    - approval :remove profit and discount at comparison just display Net profit and disc value <br />
                                    - approval :add column (No. quote) between req date and quote respon due date <br />
                                    - approval :button compare only enabled if record detail more than one and submitted by vendor more than one column too <br />
                                    - Display 10 record under vendor respon,approval pending,approval complete,Quotation Request from SMN, <br /> 
                                      &nbsp; Waiting for SMN approval,approved Request,Rejected Request,Total Closed Request <br />
                                    </asp:Label>
                                </div>
                            </div>
                            <!-- start End version content -->
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Footer -->
        <div class="container-fluid" style="background-color: #F5F5F5">
            <div class="row">
                <div class="col-lg-12" style="padding: 5px; align-content: center; text-align: center">
                    <span style="font: bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
                </div>
            </div>
        </div>

        <!-- Scroll to Top Button-->
        <a class="scroll-to-top rounded" href="#page-top">
            <i class="fas fa-angle-up"></i>
        </a>

        <!-- Modal session expired -->
        <div class="modal fade" id="myModalSession" data-backdrop="static" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true" keyboard="false">
            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                <div class="col-lg-12 Padding-Nol" style="font: bold 22px calibri, calibri; text-align: center; align-content: center;">Your Session Is About To Expire !!  </div>
                                <h4></h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="row">
                                            <div class="col-xs-2" style="padding: 10px">
                                                <asp:Image ID="ImagWarning" runat="server" class="responsive" ImageUrl="~/js/jsextendsession/images/timeout-icon.png" />
                                            </div>
                                            <div class="col-xs-10" style="padding: 10px">
                                                <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" OnTick="TimerCntDown_Tick" Enabled="false"></asp:Timer>
                                                You will be logged out in :
                                                <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label>
                                                seconds<br />
                                                do u want to stay Sign In?
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5); border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" OnClick="BtnRefresh_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px" />
                                <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" OnClick="CtnCloseMdl_Click" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px" />
                                <div style="display: none;">
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

    <script type="text/javascript">
        $(function () {
            $("[id*=TxtFrom]").datepicker({
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy"
            });
        });
        $(function () {
            $("[id*=TxtTo]").datepicker({
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy"
            });
        });
        $(function () {
            $("[id*=TxtModalDueDate]").datepicker({
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy"
            });
        });

        $(document).on('keydown', '#TxtFrom', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
        $(document).on('keydown', '#TxtTo', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
    </script>

    <%--script open modal--%>
    <script type="text/javascript">
        function SidebarMenu() {
            var SideBarMenu = document.getElementById("SideBarMenu");
            if (SideBarMenu.style.display === "none") {
                SideBarMenu.style.display = "block";
                //$("#SideBarMenu").toggle(500, "easeOutQuint");
            } else {
                //$("#SideBarMenu").toggle(500, "easeOutQuint");
                SideBarMenu.style.display = "none";
            }
        }
        function OpenModalSession() {
            try {
                jQuery.noConflict();
                $("#myModalSession").modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
        }
        function ShowLoading() {
            try {
                $('#loading').show();
            }
            catch (err) {
                alert(err + ' : ShowLoading');
            }
        }
        function CloseLoading() {
            $('#loading').fadeOut("fast");
        }
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
        try {
            $(function () {
                var timeout = 570000;
                $(document).bind("idle.idleTimer", function () {
                    // function you want to fire when the user goes idle
                    CloseModalCompare();
                    OpenModalSession();
                    $("#StartTimer").click();
                    //$.timeoutDialog({ timeout: 0.25, countdown: 15, logout_redirect_url: 'Login.aspx', restart_on_yes: true });
                });
                $(document).bind("active.idleTimer", function () {
                    // function you want to fire when the user becomes active again
                });
                $.idleTimer(timeout);
            });
        }
        catch (err) {
            alert(err + ' : alert and extend session');
        }
    </script>

    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });
    </script>
</body>
</html>
