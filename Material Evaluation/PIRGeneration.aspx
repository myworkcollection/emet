<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PIRGeneration.aspx.cs" Inherits="Material_Evaluation.PIRGeneration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="Scripts/jquery.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>

    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .WrapCnt td {
            white-space: normal !important;
            word-wrap: break-word;
        }
    </style>

    <script type="text/javascript">
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

        $(document).ready(function () {
            CloseLoading();
        });

        $(window).on("load", function () {
            CloseLoading();
        });
    </script>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server" AsyncPostBackTimeout="36000"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
        <div class="row">
            <div id="loading" class="col-sm-12" style="padding-top: 200px;" runat="server">
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
                                <asp:UpdatePanel runat="server" ID="UpModalLoading">
                                    <ContentTemplate>
                                        <a onclick="ShowLoading();" href="Home.aspx">
                                        <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                    <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" ID="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </div>
                            <div class="col-sm-2 fa-pull-right" style="background-color: #E9ECEF;">
                                <asp:Label ID="lbluser1" runat="server" Width="147px"></asp:Label><br />
                                <asp:Label ID="lblplant" runat="server" Text=""></asp:Label>
                                <asp:LinkButton runat="server" ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="wrapper">
            <!-- Sidebar -->
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

            <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                    <%--<ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <a href="#">First Article Item</a>
                            </li>
                        </ol>--%>
                    <!-- Area Chart Example-->
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-chart-area"></i>PIR GENERATION
                                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger fa-pull-right" PostBackUrl="Home.aspx" />
                                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="card-body">

                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpForm" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" AutoPostBack="true" OnSelectedIndexChanged="DdlFilterBy_SelectedIndexChanged">
                                                    <asp:ListItem Text="Plant" Value="Plant"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="Quoteno"></asp:ListItem>
                                                    <asp:ListItem Text="Request Number" Value="Requestnumber"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Code" Value="vendorcode1"></asp:ListItem>
                                                    <asp:ListItem Text="Vendor Name" Value="vendorname"></asp:ListItem>
                                                    <asp:ListItem Text="Material" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group ID" Value="ProcessGroup"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group Desc" Value="ProcessGroupDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Status" Value="status"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-8" style="padding-bottom: 5px;">
                                                <div class="group-main">
                                                    <div class="SearchBox-txt">
                                                        <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox></div>
                                                    <span class="SearchBox-btn" style="background-color: #E9ECEF;">
                                                        <asp:LinkButton ID="btnSearch" runat="server" autopostback="true" OnClientClick="ShowLoading();" OnClick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                                style="color:#005496;" ></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-12 table table-responsive" style="padding: 0px; height:350px;">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true"
                                                UpdateMode="Conditional" RenderMode="Block">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                                                        AllowPaging="false" PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                        AllowSorting="True" OnSorting="GridView1_Sorting" OnRowCreated="GridView1_RowCreated"
                                                        OnRowDataBound="GridView1_RowDataBound" CssClass="table-bordered table-sm table-nowrap Padding-Nol WrapCnt">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-BackColor="#FBDCA3"
                                                                HeaderText="SELECT TO ACCESS">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk" runat="server" EnableViewState="true" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant"></asp:BoundField>
                                                            <asp:BoundField DataField="Quoteno" HeaderText="Quote No" SortExpression="Quoteno"></asp:BoundField>
                                                            <asp:BoundField DataField="Requestnumber" HeaderText="Req. No" SortExpression="Requestnumber"></asp:BoundField>
                                                            <asp:BoundField DataField="vendorcode1" HeaderText="Vendor Code" SortExpression="vendorcode1"></asp:BoundField>
                                                            <asp:BoundField DataField="vendorname" HeaderText="Vendor Name" SortExpression="vendorname"></asp:BoundField>
                                                            <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material"></asp:BoundField>
                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc"></asp:BoundField>
                                                            <asp:BoundField DataField="status" HeaderText="PIR Status" SortExpression="status"></asp:BoundField>
                                                            <asp:BoundField DataField="CreatedBy" HeaderText="SMN PIC" SortExpression="CreatedBy"></asp:BoundField>
                                                            <asp:BoundField DataField="UseDep" HeaderText="Dept" SortExpression="UseDep"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Export">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Text="pdf" ID="BtnExport" runat="server" CssClass="btn btn-default btn-sm"
                                                                        CommandName="ExpPdf" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"> 
                                                                                    <span aria-hidden="true" style="color:#e60000;border-color:white;" class="glyphicon glyphicon-file"></span> Pdf
                                                                    </asp:LinkButton>
                                                                    <asp:Button Text="Generate" runat="server" CommandName="Approve" CommandArgument="<%# Container.DataItemIndex %>" Visible="false" />
                                                                    <asp:Button Text="Duplicate" runat="server" CommandName="Reject" CommandArgument="<%# Container.DataItemIndex %>" Visible="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                        <PagerSettings PageButtonCount="10" />
                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    </asp:GridView>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div class="col-lg-12 table table-responsive" style="padding: 0px; display:none">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true"
                                                UpdateMode="Conditional" RenderMode="Block">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GvToExport" runat="server" AutoGenerateColumns="False"
                                                        AllowPaging="true" PageSize="10" 
                                                        CssClass="table-bordered table-sm table-nowrap Padding-Nol WrapCnt">
                                                        <Columns>
                                                            <asp:BoundField DataField="Material" HeaderText="Material" ></asp:BoundField>
                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ></asp:BoundField>
                                                            <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" ></asp:BoundField>
                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ></asp:BoundField>
                                                            <asp:BoundField DataField="DDecision" HeaderText="DDecision" ></asp:BoundField>
                                                            <asp:BoundField DataField="Dname" HeaderText="Dname" ></asp:BoundField>
                                                            <asp:BoundField DataField="DAprRejDt" HeaderText="DAprRejDt" ></asp:BoundField>
                                                            <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" ></asp:BoundField>
                                                            <asp:BoundField DataField="TotalMaterialCost" HeaderText="Total Material Cost" ></asp:BoundField>
                                                            <asp:BoundField DataField="TotalProcessCost" HeaderText="Total Process Cost" ></asp:BoundField>
                                                            <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="Total Sub Material Cost" ></asp:BoundField>
                                                            <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="Total Other Items Cost" ></asp:BoundField>
                                                            <asp:BoundField DataField="GrandTotalCost" HeaderText="Grand Total Cost" ></asp:BoundField>
                                                            <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final Quote Price" ></asp:BoundField>
                                                        </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                        <PagerSettings PageButtonCount="10" />
                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                        <RowStyle ForeColor="#000066" />
                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div class="row" style="display: none">
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="hdnReqNo" runat="server" />
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div id="dialog1" title="Full Image View" style="display: none">
                                                    <asp:Image ID="img1" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row" style="padding-bottom: 10px;">
                                <div class="col-md-12">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Generate and Download PIR" OnClick="Button1_Click" />
                                    <asp:LinkButton ID="BtnExportAllSelecToPdf" runat="server" CssClass="btn btn-default" OnClick="BtnExportAllSelecToPdf_Click" BorderStyle="Solid" BorderColor="#cc0000">
                                        <span aria-hidden="true" style="color:#e60000;border-color:white;" class="glyphicon glyphicon-file"></span> Export Selected To Pdf
                                    </asp:LinkButton>
                                    <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                </div>
                            </div>
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

        <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i></a>

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
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" /></div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
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
        function ExportPdf(Qno) {
            try {
                var url = "printapproved.aspx?Number=" + Qno + "";
                var win = window.open(url, '_blank');
                win.focus();
            }
            catch (err) {
                alert(err + ' : ExportPdf');
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
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
        try {
            $(function () {
                var timeout = 570000;
                $(document).bind("idle.idleTimer", function () {
                    // function you want to fire when the user goes idle
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
</body>
</html>

