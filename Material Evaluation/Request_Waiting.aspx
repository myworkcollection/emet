<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Request_Waiting.aspx.cs"
    Inherits="Material_Evaluation.Request_Waiting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <link href="js/BootstrapDatePcr/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <style type="text/css">
        .SideBarMenu {
            width: 300px;
        }

        .WrapCnt td, th {
            white-space: normal !important;
            /*word-wrap: break-word;*/
            font-size: 14px !important;
        }

        .WrapCnt a {
            padding: 0px;
        }

        .my-btn-sm {
            padding:0px!important;
        }
    </style>

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>

    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            DatePitcker();
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

        function ExpandAll() {
            try {
                $(function () {
                    var table = document.getElementById('GridView1');
                    if (table != null) {
                        var count = $('#GridView1 tr').length;
                        for (var c = 0; c < count; c++) {
                            var ImgUrl = $("#GridView1_Image1_" + c).attr('src');
                            if (ImgUrl != null) {
                                if (ImgUrl.toString() == "images/plus1.png") {
                                    $("#GridView1_Image1_" + c).attr("src", "images/minus1.png");
                                    $("#GridView1_Image1_" + c).closest("tr").after("<tr><td></td><td colspan = '999'>" + $("#GridView1_Image1_" + c).next().html() + "</td></tr>");
                                    var panel = document.getElementById("GridView1_pnlDet_" + c + "");
                                    //if (panel != null) {
                                    //    document.getElementById("GridView1_pnlDet_" + c + "").remove();
                                    //}
                                }
                            }
                        }
                    }
                }); (jQuery)
            }
            catch (err) {
                alert(err + ":ExpandAll")
            }
        }
        function ColapsAll() {
            try {
                $(function () {
                    var table = document.getElementById('GridView1');
                    if (table != null) {
                        var count = $('#GridView1 tr').length;
                        for (var c = 0; c < count; c++) {
                            var ImgUrl = $("#GridView1_Image1_" + c).attr('src');
                            if (ImgUrl != null) {
                                if (ImgUrl.toString() == "images/minus1.png") {
                                    $("#GridView1_Image1_" + c).attr("src", "images/plus1.png");
                                    $("#GridView1_Image1_" + c).closest("tr").next().remove();
                                }
                            }
                        }
                    }
                }); (jQuery)
            }
            catch (err) {
                alert(err + ":expandgrid")
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
        function DatePitcker() {
            try {
                (function ($) {
                    $(".form_datetime").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        fontAwesome: 'font-awesome',
                        format: "dd/mm/yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        minView: 2
                    });

                    $("#TxtModalDueDate").datetimepicker({
                        //format: "dd/mm/yyyy - hh:ii",
                        fontAwesome: 'font-awesome',
                        startDate: new Date(),
                        format: "dd/mm/yyyy",
                        autoclose: true,
                        todayBtn: true,
                        todayHighlight: true,
                        minView: 2
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker');
            }
        }
        function openModal() {
            try {
                jQuery.noConflict();
                $('#myModal').modal('show');
            }
            catch (err) {
                alert(err + ' : OpenModalSession');
            }
        }
        function closeModal() {
            try {
                jQuery.noConflict();
                $('#myModal').modal('hide');
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
        function TriggerNested(IdVsSts) {
            try {
                (function ($) {
                    var ArrIdVsSts = IdVsSts.split('-');
                    var Id = ArrIdVsSts[0].toString();
                    var Status = ArrIdVsSts[1].toString();
                    if (Status == "Ex") {
                        $("#GridView1_Image1_" + Id).closest("tr").after("<tr><td></td><td colspan = '999'>" + $("#GridView1_Image1_" + Id).next().html() + "</td></tr>")
                        $("#GridView1_Image1_" + Id).attr("src", "images/minus1.png");
                    }
                    else {
                        $("#GridView1_Image1_" + Id).attr("src", "images/plus1.png");
                    }
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : TriggerNested');
            }
        }

        function openInNewTab(url) {
            var win = window.open(url, '_blank');
            win.focus();
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

    <script language="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");

            if ($('#IsFirstLoad').val("1")) {
                $('#BtnCekLatestNested').click();
            }
        });

        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus1.png");

            ////var ImageID = $(this).attr('ID');
            ////var ArrImageID = ImageID.split('_');
            ////$('#GridView1_BtnNstIdPls_' + ArrImageID[2]).click();
        });

        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus1.png");
            $(this).closest("tr").next().remove();

            //var ImageID = $(this).attr('ID');
            //var ArrImageID = ImageID.split('_');
            //$('#GridView1_BtnNstIdMin_' + ArrImageID[2]).click();
        });
    </script>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
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
                                <a onclick="ShowLoading();" href="Home.aspx">
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

            <!-- Content -->
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
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b>Quote Request With SAP Code</b>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-sm-8" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                        </div>
                                        <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                            <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClick="BtnReset_Click" autopostback="true"></asp:Button>
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" PostBackUrl="Home.aspx" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFltrDate">
                                                    <asp:ListItem Text="Request Date" Value="RequestDate"></asp:ListItem>
                                                    <asp:ListItem Text="Quote Response Due Date" Value="QuoteResponseDueDate"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4">
                                                <div class="col-lg-6 nopadding">
                                                    <div class="group-main" style="padding: 0px 2px 5px 2px;">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtFrom" OnclientClick="return false;" runat="server" placeholder="Date From" 
                                                                ToolTip="Date From" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 1px 1px 1px;" onclick="javascript: $('#TxtFrom').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6 nopadding">
                                                    <div class="group-main" style="padding: 0px 2px 5px 2px;">
                                                        <div class="SearchBox-txt">
                                                            <asp:TextBox ID="TxtTo" OnclientClick="return false;" runat="server" placeholder="Date To"
                                                                ToolTip="Date To" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black" CssClass="form_datetime">
                                                            </asp:TextBox>
                                                        </div>
                                                        <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 1px;">
                                                            <a class="fa fa-calendar" style="color: #005496; padding: 1px 1px 1px 1px;" onclick="javascript: $('#TxtTo').focus();"></a>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" onchange="javascript:document.getElementById('txtFind').value = ''">
                                                    <asp:ListItem Text="Plant" Value="Plant"></asp:ListItem>
                                                    <asp:ListItem Text="Request Number" Value="RequestNumber"></asp:ListItem>
                                                    <asp:ListItem Text="Product" Value="Product"></asp:ListItem>
                                                    <asp:ListItem Text="Material" Value="Material"></asp:ListItem>
                                                    <asp:ListItem Text="Material Desc" Value="MaterialDesc"></asp:ListItem>
                                                    <asp:ListItem Text="Quote No" Value="QuoteNo"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group ID" Value="ProcessGroup"></asp:ListItem>
                                                    <asp:ListItem Text="Process Group Desc" Value="ProcessGroupDesc"></asp:ListItem>
                                                    <asp:ListItem Text="SMN PIC" Value="CreatedBy"></asp:ListItem>
                                                    <asp:ListItem Text="Department" Value="UseDep"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:TextBox runat="server" ID="txtFind" Text=""></asp:TextBox>
                                            </div>
                                            
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-sm btn-primary btn-block my-btn-sm" runat="server" 
                                                    autopostback="true" OnClientClick="ShowLoading();" OnClick="btnSearch_Click"><i class="fa fa-search" aria-hidden="true" 
                                                            style="color:#F5F5F5;" ></i> Search </asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpForm" runat="server">
                                <ContentTemplate>
                                    <div class="row" style="padding-top: 10px; padding-bottom: 10px;">
                                        <div class="col-sm-6 ">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton runat="server" ID="BtnExpandAll" CssClass="btn btn-sm btn-primary btn-sm" Font-Size="14px"
                                                        OnClientClick="ExpandAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-down"></i> Expand All </asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="BtnColapsAll" CssClass="btn btn-sm btn-info btn-sm" Font-Size="14px"
                                                        OnClientClick="ColapsAll();return false;" autopostback="false"><i class="glyphicon glyphicon-collapse-up"></i> Collapse All </asp:LinkButton>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="col-sm-6 ">
                                            <asp:Label runat="server" ID="Label1" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                            <asp:TextBox runat="server" ID="TxtShowEntry" Text="10" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                Width="50px" CssClass="fa-pull-right" Style="text-align: center"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label2" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-12 table table-responsive" style="padding: 0px;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true"
                                            UpdateMode="Conditional" RenderMode="Block">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView1" runat="server"
                                                    AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                    AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                                                    AllowSorting="True" OnSorting="GridView1_Sorting"
                                                    OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                                    CssClass="table-responsive  table-sm table-bordered table-nowrap  Padding-Nol WrapCnt">
                                                    <Columns>
                                                        <%--Gridview Level 1--%>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image1" runat="server" Style="cursor: pointer" ImageUrl="~/images/plus1.png" Width="15px" />
                                                                <asp:Panel ID="pnlDet" runat="server" Style="display: none">
                                                                    <asp:GridView ID="GvDet" runat="server" AutoGenerateColumns="false" CssClass="table-hover Padding-Nol"
                                                                        OnRowDataBound="GvlDet_RowDataBound" OnRowCommand="GvDet_RowCommand" DataKeyNames="QuoteNo">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="No.">
                                                                                <ItemTemplate><%# Eval("ParentGvRowNo") %>.<%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="VendorCode1" HeaderText="Vendor" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Quote No" HeaderStyle-CssClass="text-center ">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ForeColor="#0033cc" Text='<%# Eval("QuoteNo") %>' runat="server" ID="LbQuoteNo" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#4d94ff" Font-Bold="True" ForeColor="White" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="No.">
                                                            <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                            <ItemStyle Width="10px" />
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" SortExpression="RequestNumber" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="NoQuote" HeaderText="No. Que" SortExpression="NoQuote" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="RequestDate" HeaderText="Req. Date" SortExpression="RequestDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Response Date" SortExpression="QuoteResponseDueDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="SMN PIC" SortExpression="CreatedBy" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:BoundField DataField="UseDep" HeaderText="Dept" SortExpression="UseDep" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                        <asp:TemplateField HeaderText="Q.Res.Due Dt" HeaderStyle-CssClass="text-center ">
                                                            <ItemTemplate>
                                                                <asp:Button ID="BtnApprove" Text="Update Date" runat="server" CssClass="btn Table-button" CommandName="Approve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:Button Text="Reject" ID="BtnReject" runat="server" CssClass="btn Table-button" CommandName="Reject" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                <div style="display: none">
                                                                    <asp:Button Text=".." ID="BtnNstIdPls" runat="server" CssClass="btn Table-button" CommandName="TrgNestedExpand" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                    <asp:Button Text=".." ID="BtnNstIdMin" runat="server" CssClass="btn Table-button" CommandName="TrgNestedColapse" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                </div>
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

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="display: none;">
                                        <asp:TextBox runat="server" ID="IsFirstLoad" Text="1"></asp:TextBox>
                                        <asp:Button runat="server" ID="BtnCekLatestNested" Text="reset" OnClick="BtnCekLatestNested_Click" autopostback="true"></asp:Button>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="lblhdnreason" runat="server" Style="visibility: collapse;"></asp:TextBox>
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

        <!-- Scroll to Top Button-->
        <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i>
        </a>

        <%--modal add/edit data--%>
        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Bootstrap Modal add data user Dialog -->
                <div class="modal fade" id="myModal" data-backdrop="static" tabindex="-1" role="dialog"
                    aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-content" style="background: linear-gradient(90deg, #F7F7F7, #ffffff, #F7F7F7); border-radius: 15px;">
                                    <div class="modal-header">
                                        <div class="row">
                                            <div class="col-sm-12 text-uppercase text-center" style="text-shadow: 1px 2px 1px white;">
                                                <asp:Label ID="LbModalHeader" runat="server" Text="Quote Respon Due Date"
                                                    ForeColor="#004080" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body" style="background-color: white; padding-bottom: 0px;">
                                        <div class="row" style="padding-bottom: 0px;">
                                            <div class="col-sm-12" style="padding-bottom: 0px;">

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="Label11" runat="server" Text="Request Number"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="TxtModalReqNo" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:TextBox ID="TxtMaterial" runat="server" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 10px;">
                                                    <div class="col-sm-4">
                                                        <asp:Label ID="lblUserName" runat="server" Text="Due Date"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <div class="group-main">
                                                            <div class="SearchBox-txt">
                                                                <asp:TextBox ID="TxtModalDueDate" OnclientClick="return false;" runat="server"
                                                                    OnTextChanged="TxtFrom_TextChanged" AutoPostBack="true" onkeydown="javascript:preventInput(event);" autocomplete="off" AutoCompleteType="Disabled" ForeColor="Black">
                                                                </asp:TextBox>
                                                            </div>
                                                            <span class="SearchBox-btn-cal" style="background-color: #E9ECEF; padding: 1px 3px 1px 3px;">
                                                                <a class="fa fa-calendar" style="color: #005496; padding: 1px 3px 1px 3px;" onclick="javascript: $('#TxtModalDueDate').focus();"></a>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12" style="padding-bottom: 0px;">
                                                        <div class="table table-responsive" style="padding-bottom: 0px;">
                                                            <asp:GridView ID="grdvendor" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                CssClass="table table-sm table-bordered table-nowrap">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="VendorCode1" HeaderText="Vendor ID" />
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" />
                                                                </Columns>
                                                                <EditRowStyle BackColor="#999999" />
                                                                <FooterStyle BackColor="#1a2e4c" ForeColor="White" />
                                                                <HeaderStyle BackColor="#4D94FF" ForeColor="White" />
                                                                <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" />
                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel ID="UpInvalidRequest" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row" style="padding-bottom: 10px" runat="server" id="DvInvalidRequest" visible="false">
                                                            <div class="col-md-12">
                                                                <div class="col-md-12" style="background: #fa0606">
                                                                    <asp:Label ID="Label18" runat="server" Text="Faill to update Due Date , New Request has been created for below vendor"
                                                                        Visible="true" ForeColor="White" Font-Bold="true"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="table table-responsive table-sm">
                                                                    <asp:GridView ID="GvInvalidRequest" runat="server" AutoGenerateColumns="False"
                                                                        AllowPaging="false" PageSize="10" OnRowDataBound="GvInvalidRequest_RowDataBound"
                                                                        CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Res Due Date" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="QuoteNo" HeaderText="QuoteNo" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="Material" HeaderText="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorCode1" HeaderText="Vendor Code" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                        </Columns>
                                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                                        <PagerSettings PageButtonCount="10" />
                                                                        <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" BorderColor="White" />
                                                                        <RowStyle ForeColor="#000066" />
                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer" style="background-color: #F5F5F5; border-bottom-right-radius: 15px; border-bottom-left-radius: 15px; border-top: 0px;">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="row">
                                                    <div class="col-sm-6" style="padding-bottom: 8px;">
                                                        <asp:Button ID="btnSubmit" CssClass="btn btn-sm btn-primary" OnClientClick="ShowLoading();" OnClick="btnSubmit_Click" Width="100%"
                                                            Font-Size="14px" runat="server" Text="Save" />
                                                    </div>
                                                    <div class="col-sm-6 " style="padding-bottom: 8px;">
                                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-default" Text="Close" Width="100%"
                                                            Font-Size="14px" data-dismiss="modal" aria-hidden="true" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

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

</body>
</html>
