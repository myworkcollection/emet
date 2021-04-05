<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Request_Waiting_vendor.aspx.cs" Inherits="Material_Evaluation.Request_Waiting_vendor" %>

<!DOCTYPE html>

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

        .blink {
          animation: blink-animation 1s steps(5, start) infinite;
          -webkit-animation: blink-animation 1s steps(5, start) infinite;
        }

        .my-btn-sm {
            padding:0px!important;
        }

        @keyframes blink-animation {
          to {
            visibility: hidden;
          }
        }
        @-webkit-keyframes blink-animation {
          to {
            visibility: hidden;
          }
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
    <script type="text/javascript" src="js/BootstrapDatePcr/js/bootstrap-datetimepicker.js"></script>
    <script type="text/javascript" src="js/BootstrapDatePcr/js/locales/bootstrap-datetimepicker.fr.js"></script>




    <script type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });

        $(document).ready(function () {
            DatePitcker();
            playAudio();
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

    <%--script alert unred announcement --%>
    <script type="text/javascript">
        function playAudio() {
            if (document.getElementById('LiUnReadAnn').style.display == "block")
            {
                var x = document.getElementById("myAlertAudio");
                //x.loop = true;
                x.play();
            }
        }
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

        function ShowLoading() {
            $('#loading').show();
        }
        function CloseLoading() {
            $('#loading').fadeOut("fast");
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
                })(jQuery);
            }
            catch (err) {
                alert(err + ' : DatePitcker');
            }
        }
    </script>

    <%--script alert and extend session--%>
    <script type="text/javascript">
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
                                <a onclick="ShowLoading();" href="Emet_author_V.aspx?num=15">
                                    <asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                                <%--<button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#"><i class="fas fa-bars"></i></button>--%>
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

        <div id="wrapper" class="bg-white">
            <!-- Sidebar -->
            <div id="SideBarMenu" style="width: 300px;" runat="server" class="SideBarMenu">
                <ul class="sidebar">
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=15">
                            <i class="fas fa-fw fa-tachometer-alt"></i>
                            <span>Home</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="Emet_author_V.aspx?num=16">
                            <i class="fas fa-fw fa-table"></i>
                            <span>Master Data</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="ChangePwd.aspx?num=15">
                            <i class="fas fa-fw fa-key"></i>
                            <span>Change Password</span>
                        </a>
                    </li>
                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="VndReport.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Report</span>
                        </a>
                    </li>

                    <li class="sideMenu">
                        <a class="linkMenu" onclick="ShowLoading();" href="RealTimeVendInv.aspx">
                            <i class="fas fa-fw fa-book-open"></i>
                            <span>Real Time Inventory</span>
                        </a>
                    </li>

                    <li class="sideMenu" runat="server" id="LiUnReadAnn" style="display:block">
                        <a onclick="ShowLoading();" href="Emet_author_V.aspx?num=14" class="MyLink">
                            <asp:Label runat="server" ID="lbUnreadAnn" CssClass="blink" Text="You Have Unread Announcements" ForeColor="Red"
                                 Font-Bold="true" Font-Size="20px"></asp:Label>
                        </a>
                        <audio id="myAlertAudio" controls="controls" style="display:none" allow="autoplay">
                          <source src="Styles/alert1.wav" type="audio/wav"/>
                          Your browser does not support the audio element.
                        </audio>
                    </li>
                </ul>
            </div>

            <div id="content-wrapper">
                <div class="container-fluid">
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <div class="row">
                                    <div class="col-md-12" style="word-wrap: break-word; font-size: 18px;">
                                        <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS - <b>Quote Request With SAP Code </b>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-body">
                            <div class="row" style="padding-bottom: 5px;">
                                <div class="col-sm-8">
                                    <asp:Label ID="LbVendorCode" runat="server"
                                        Font-Size="18px" Text=",hkjh"></asp:Label>
                                </div>
                                <div class="col-sm-4 text-right" style="padding-bottom: 5px;">
                                    <asp:Button runat="server" ID="BtnReset" Text="Reset" CssClass="btn btn-sm btn-warning" OnClick="BtnReset_Click" autopostback="true"></asp:Button>
                                    <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger" PostBackUrl="Vendor.aspx" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12" style="padding-bottom: 5px;">
                                    <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdltaskStatus"  ToolTip="Task Status">
                                                    <asp:ListItem Text="All Task Status" Value="All"></asp:ListItem>
                                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                    <asp:ListItem Text="Draft" Value="Draft"></asp:ListItem>
                                                    <asp:ListItem Text="Revision" Value="Revision"></asp:ListItem>
                                                    <asp:ListItem Text="Revision-Draft" Value="Revision-Draft"></asp:ListItem>
                                                    <%--<asp:ListItem Text="Mass Revision" Value="MassRevision"></asp:ListItem>
                                                    <asp:ListItem Text="Mass Revision - Draft" Value="MassRevision-Draft"></asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </div>
                                            
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
                                    <div id="DvNormalData" runat="server" style="display: block">
                                        <div class="row" style="padding-top: 10px; padding-bottom: 10px;">
                                            <div class="col-sm-12 ">
                                                <asp:Label runat="server" ID="Label1" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                <asp:TextBox runat="server" ID="TxtShowEntry" Text="10" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"></asp:TextBox>
                                                <asp:Label runat="server" ID="Label2" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="table table-responsive table-sm">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" RenderMode="Block">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                                                AllowSorting="True" OnSorting="GridView1_Sorting"
                                                                AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                                OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                                                                OnRowCreated="GridView1_RowCreated"
                                                                CssClass="table-sm table-bordered table-nowrap WrapCnt" Font-Bold="False" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant"></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Req. No" SortExpression="RequestNumber" HeaderStyle-CssClass="text-center ">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton Text='<%# Eval("RequestNumber") %>' runat="server" CommandName="LinktoRedirect"
                                                                                CommandArgument="  <%# ((GridViewRow) Container).RowIndex %>" OnClientClick="ShowLoading();" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="RequestDate" HeaderText="Req. Date" SortExpression="RequestDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Response Date" SortExpression="QuoteResponseDueDate" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" SortExpression="QuoteNo" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="CreatedBy" HeaderText="SMN PIC" SortExpression="CreatedBy" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="UseDep" HeaderText="Dept" SortExpression="UseDep" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                    <asp:BoundField DataField="TaskStatus" HeaderText="Task Status" SortExpression="TaskStatus" HeaderStyle-CssClass="text-center "></asp:BoundField>
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
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                    <%--//old table--%>
                                                    <asp:Table ID="Table1" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4" Visible="false" rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                    </asp:Table>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label ID="LbTtlRecords" runat="server" Text="Total Record : 0" Font-Bold="true" CssClass="fa-pull-right"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--Mass Approval Data--%>
                                    <div class="row" runat="server" id="DvMassRev" style="display: block">
                                        <div class="col-md-12" style="padding-bottom: 0px;">
                                            <div class="row" style="padding-bottom: 5px;">
                                                <div class="col-md-8">
                                                </div>
                                                <div class="col-sm-4">
                                                    <asp:Label runat="server" ID="Label14" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TxtShowEntMassRev" Text="5" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                        Width="50px" CssClass="fa-pull-right" Style="text-align: center"
                                                        OnTextChanged="TxtShowEntMassRev_TextChanged" AutoPostBack="true">
                                                    </asp:TextBox>
                                                    <asp:Label runat="server" ID="Label15" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-lg-12 table-sm table-responsive" style="padding: 0px;">
                                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GdvMassRev" runat="server" ShowHeaderWhenEmpty="false" Width="100%" BackColor="White"
                                                            AllowPaging="True" PageSize="5" OnPageIndexChanging="GdvMassRev_PageIndexChanging"
                                                            AutoGenerateColumns="False" OnRowDataBound="GdvMassRev_RowDataBound" OnRowCommand="GdvMassRev_RowCommand"
                                                            AllowSorting="True" OnSorting="GdvMassRev_Sorting" OnRowCreated="GdvMassRev_RowCreated"
                                                            CssClass="table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No.">
                                                                    <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-BackColor="#009933" HeaderText="Approve" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="RbAllApp" runat="server" AutoPostBack="true" Text="Approve" OnCheckedChanged="RbAllApp_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="RbApp" runat="server" CssClass="radiobtn" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="RbApp_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderStyle-BackColor="#cc0000" HeaderText="Reject" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="RbAllRej" runat="server" AutoPostBack="true" Text="Reject" OnCheckedChanged="RbAllRej_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="RbRej" runat="server" AutoPostBack="true" EnableViewState="true" OnCheckedChanged="RbRej_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>--%>
                                                                <asp:BoundField DataField="Plant" HeaderText="Plant" SortExpression="Plant" HeaderStyle-CssClass="text-center "></asp:BoundField>
                                                                <asp:BoundField DataField="RequestNumber" HeaderText="Req. No" SortExpression="RequestNumber" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Quote No" SortExpression="QuoteNo" HeaderStyle-CssClass="text-center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton Text='<%# Eval("QuoteNo") %>' ID="LbQuoteNo" runat="server" CommandName="LinktoRedirect" CommandArgument="  <%# ((GridViewRow) Container).RowIndex %>" OnClientClick="ShowLoading();" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="RequestDate" HeaderText="Req. Date" SortExpression="RequestDate" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="Respon Date" SortExpression="QuoteResponseDueDate" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Product" HeaderText="Product" SortExpression="Product" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorCode1" HeaderText="Vnd ID" SortExpression="VendorCode1" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vnd Name" SortExpression="VendorName" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="OldTotMatCost" HeaderText="Old T.Mat" ItemStyle-HorizontalAlign="Right" SortExpression="TotalMaterialCost" HeaderStyle-CssClass="text-center" HeaderStyle-BackColor="#cc6600"></asp:BoundField>
                                                                <asp:BoundField DataField="OldFinal" HeaderText="Old Final Cost" ItemStyle-HorizontalAlign="Right" SortExpression="FinalQuotePrice" HeaderStyle-CssClass="text-center" HeaderStyle-BackColor="#cc6600"></asp:BoundField>
                                                                <asp:BoundField DataField="TotalMaterialCost" HeaderText="New T.Mat" ItemStyle-HorizontalAlign="Right" SortExpression="TotalMaterialCost" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="TotalProcessCost" HeaderText="T.Proc" ItemStyle-HorizontalAlign="Right" SortExpression="TotalProcessCost" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="T.Sub Mat" ItemStyle-HorizontalAlign="Right" SortExpression="TotalSubMaterialCost" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="T. Oth" ItemStyle-HorizontalAlign="Right" SortExpression="TotalOtheritemsCost" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="GrandTotalCost" HeaderText="Grand Tot" ItemStyle-HorizontalAlign="Right" SortExpression="GrandTotalCost" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="FinalQuotePrice" HeaderText="New Final Cost" ItemStyle-HorizontalAlign="Right" SortExpression="FinalQuotePrice" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Diffrence" HeaderText="Diffrence" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                <asp:BoundField DataField="UpdatedOn" HeaderText="Updated Date" SortExpression="UpdatedOn" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Updatedby" HeaderText="Updated By" SortExpression="Updatedby" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <%--<asp:BoundField DataField="PIRStatus" HeaderText="Overall status" SortExpression="PIRStatus" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ResponseStatus" HeaderText="Ven. Res. Status" SortExpression="ResponseStatus" HeaderStyle-CssClass="text-center"></asp:BoundField>--%>
                                                                <asp:BoundField DataField="CreatedBy" HeaderText="SMN PIC" SortExpression="CreatedBy" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="UseDep" HeaderText="Dept" SortExpression="UseDep" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                            <PagerSettings PageButtonCount="10" />
                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="row" style="padding-bottom: 5px;">
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label runat="server" ID="LbTotalRecMassRev" Text="" CssClass="fa-pull-right" ForeColor="#0033cc" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label runat="server" ID="LbTotUncheck" Text="" CssClass="fa-pull-right" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Label runat="server" ID="LbTotApp" Text="" CssClass="fa-pull-right" Font-Bold="true" ForeColor="#009933"></asp:Label>
                                                </div>
                                                <%--<div class="col-md-3">
                                                    <asp:Label runat="server" ID="LbTotRej" Text="" CssClass="fa-pull-right" Font-Bold="true" ForeColor="#CC0000"></asp:Label>
                                                </div>--%>
                                            </div>
                                            <div class="row" style="padding-bottom: 5px;">
                                                <div class="col-md-10">
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button runat="server" ID="BtnProceed" Text="Submit" OnClientClick="return ValidateProceed();ShowLoading();" OnClick="BtnProceed_Click"
                                                        CssClass="btn btn-block btn-sm btn-primary" Font-Bold="true"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
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
</body>
</html>
