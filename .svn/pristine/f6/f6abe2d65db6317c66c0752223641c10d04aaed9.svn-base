<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Eannouncement.aspx.cs" Inherits="Material_Evaluation.Eannouncement" %>

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

    <style type="text/css">
        .modal {
            margin-top: 0px;
        }

        .SideBarMenu {
            width: 300px;
        }

        .WrapCnt td {
            white-space: normal !important;
            word-wrap: break-word;
        }
    </style>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
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

        <div id="wrapper" class="bg-white">
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
                    <div class="card">
                        <div class="card-header">
                            <div class="card-header-content ">
                                <i class="fas fa-info-circle"></i>Announcements
                                <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-sm btn-danger fa-pull-right" PostBackUrl="Home.aspx" />
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-12" style="padding-bottom: 5px;">
                                            <asp:Label runat="server" ID="LbFilter" Text="Filter By :"></asp:Label>
                                            <asp:Button ID="BtnAdd" CssClass="btn btn-sm btn-primary fa-pull-right" OnClick="BtnAdd_Click"
                                                OnClientClick="openModal();" runat="server" Text="Add" autopostback="false" Width="50px" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="UpForm" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" DefaultButton="btnSearch">
                                        <div class="row">
                                            <div class="col-lg-4" style="padding-bottom: 5px;">
                                                <asp:DropDownList runat="server" ID="DdlFilterBy" OnSelectedIndexChanged="DdlFilterBy_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Subject" Value="Subject"></asp:ListItem>
                                                    <asp:ListItem Text="Content" Value="Content"></asp:ListItem>
                                                    <asp:ListItem Text="Created By" Value="CreatedBy"></asp:ListItem>
                                                    <asp:ListItem Text="Updated By" Value="UpdatedBy"></asp:ListItem>
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

                                        <div class="row" style="padding-top: 10px; padding-bottom: 10px;">
                                            <div class="col-sm-12 ">
                                                <asp:Label runat="server" ID="Label3" Text="&nbsp; Entries" CssClass="fa-pull-right"></asp:Label>
                                                <asp:TextBox runat="server" ID="TxtShowEntry" Text="10" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"
                                                    Width="50px" CssClass="fa-pull-right" Style="text-align: center"></asp:TextBox>
                                                <asp:Label runat="server" ID="Label4" Text="Show &nbsp;" CssClass="fa-pull-right"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-lg-12 table table-responsive" style="padding: 0px;">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" RenderMode="Block">
                                                <ContentTemplate>
                                                    <asp:GridView ID="GridView1" runat="server" Width="100%"
                                                        AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                        AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand"
                                                        AllowSorting="True" OnSorting="GridView1_Sorting"
                                                        OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                                        CssClass="table-responsive  table-sm table-bordered table-nowrap Padding-Nol table-hover WrapCnt">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="No.">
                                                                <ItemTemplate><%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                <ItemStyle Width="10px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject"></asp:BoundField>
                                                            <asp:BoundField DataField="ContentAnnc" HeaderText="Content" ItemStyle-Width="240px" SortExpression="ContentAnnc"></asp:BoundField>
                                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
                                                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate"></asp:BoundField>
                                                            <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy"></asp:BoundField>
                                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate"></asp:BoundField>
                                                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" ItemStyle-Width="250px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Text="Preview" ID="BtnPreview" runat="server" CssClass="btn btn-info btn-sm"
                                                                        CommandName="CPreview" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"> 
                                                                                    <span aria-hidden="true" class="glyphicon glyphicon-eye-open"></span> Preview
                                                                    </asp:LinkButton>
                                                                    <asp:HiddenField ID="HiddenId" Value='<%# Eval("id") %>' runat="server" />
                                                                    <asp:LinkButton ID="BtnEdit" Text="Edit" runat="server" CssClass="btn btn-primary btn-sm"
                                                                        CommandName="Editt" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">
                                                                                    <span aria-hidden="true" class="glyphicon glyphicon-edit"></span> Edit
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton Text="Delete" ID="BtnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                                                        CommandName="CDelete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        OnClientClick="return confirm('Are you sure you want to delete this Data?');">
                                                                                    <span aria-hidden="true" class="glyphicon glyphicon-trash"></span> Delete
                                                                    </asp:LinkButton>
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
                                    </asp:Panel>
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

        <%--modal add/edit data--%>
        <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Bootstrap Modal add data user Dialog -->
                <div class="modal fade" id="myModal" data-backdrop="static" tabindex="-1" role="dialog"
                    aria-labelledby="myModalLabel" aria-hidden="true" style="padding-top: 0px;">
                    <div class="modal-dialog modal-lg">
                        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-content" style="background: linear-gradient(90deg, #F7F7F7, #ffffff, #F7F7F7); border-radius: 15px;">
                                    <div class="modal-header" style="margin: 0 auto; display: block;">
                                        <div class="row">
                                            <div class="col-sm-12 text-uppercase text-center" style="text-shadow: 1px 2px 1px white;">
                                                <asp:Label ID="LbModalHeader" runat="server" Text="Add Announcement"
                                                    ForeColor="#004080" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body" style="background-color: white; padding-bottom: 0px;">
                                        <div class="row">
                                            <div class="col-sm-12" style="padding: 10px;">
                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-12">
                                                        <asp:Label ID="Label11" runat="server" Text="Subject"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <div style="display: none;">
                                                            <asp:TextBox ID="TxtId" runat="server"></asp:TextBox></div>
                                                        <asp:TextBox ID="TxtSubject" runat="server" placeholder="Subject Maximal 100 charcter" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;">
                                                    <div class="col-sm-12">
                                                        <asp:Label ID="Label1" runat="server" Text="Content" placeholder="Content Maximal 50 charcter"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <asp:TextBox ID="TxtContent" runat="server" TextMode="MultiLine" Height="200px" Width="100%" Font-Size="14px"
                                                            CssClass="form-control" placeholder="Content Maximal 1000 charcter" MaxLength="1000"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-12" style="display: none">
                                                        <asp:TextBox ID="TxtFilePath" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="row" id="DvChangeAttachemnt" runat="server" style="padding-bottom: 5px;">
                                                    <div class="col-sm-12">
                                                        <asp:Button ID="BtnChangeAttachment" runat="server" Text="Change Attachment" OnClientClick="ChangeAttachment();" CssClass="btn btn-sm btn-info pull-left"></asp:Button>
                                                        <div id="DvChkDeleteAttachment" runat="server" class="pull-right">
                                                            <asp:CheckBox ID="ChkDeleteAttachment" runat="server" Text="&nbsp; Delete Attachment"></asp:CheckBox></div>
                                                    </div>
                                                </div>

                                                <div class="row" id="DvdAttachmanetOld" runat="server" style="padding-bottom: 5px;">
                                                    <div class="col-sm-9">
                                                        <asp:Label ID="LblFileName" runat="server" Text="" placeholder=""></asp:Label>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton ID="LBtnDownloadOld" runat="server" Text="Download And View" CssClass="pull-right"
                                                            OnClientClick="if(CheckFileUploadOld()==false) return false;" OnClick="LBtnDownloadOld_Click"></asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px;" id="DvAttachmanet" runat="server">
                                                    <div class="col-sm-9">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <asp:Label ID="Label2" runat="server" Text="Attachment File Size Max : 3 MB" placeholder=""></asp:Label>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <asp:FileUpload ID="FuAttachment" runat="server" CssClass="form-control-sm" onchange="preview_image(event);Javascript: return CheckFileSize();" Font-Size="14px"></asp:FileUpload>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:LinkButton ID="LBtnDownload" runat="server" Text="Download And View" CssClass="pull-right"
                                                            OnClientClick="if(CheckFileUpload()==false) return false;" OnClick="LBtnDownload_Click"></asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="row" style="padding-bottom: 5px; display: none;">
                                                    <div class="col-sm-12">
                                                        <div class="col-sm-12 Padding-Nol" id="DvImagePrev" style="display: none;">
                                                            <center><asp:Image ID="MpImageAttachment" runat="server" class="img-responsive" ImageUrl=""/></center>
                                                        </div>
                                                        <script type='text/javascript'>
                                                            function preview_image(event) {
                                                                var DvImagePrev = document.getElementById("DvImagePrev");
                                                                var FileUpload = document.getElementById("FuAttachment");
                                                                var Extension = FileUpload.value.substring(FileUpload.value.lastIndexOf('.') + 1).toLowerCase();
                                                                if (Extension == 'jpeg' || Extension == 'jpg' || Extension == 'bmp' || Extension == 'png') {
                                                                    DvImagePrev.style.display = "block";
                                                                    var reader = new FileReader();
                                                                    reader.onload = function () {
                                                                        var output = document.getElementById('<%=MpImageAttachment.ClientID%>');
                                                                        output.src = reader.result;
                                                                    }
                                                                    reader.readAsDataURL(event.target.files[0]);
                                                                }
                                                                else {
                                                                    DvImagePrev.style.display = "none";
                                                                }
                                                            }
                                                        </script>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer" style="background-color: #F5F5F5; border-bottom-right-radius: 15px; border-bottom-left-radius: 15px; border-top: 0px;">
                                        <div class="row">
                                            <div class="col-sm-6" style="padding-bottom: 8px;">
                                                <asp:Button ID="btnSubmit" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" Width="100%"
                                                    Font-Size="14px"
                                                    OnClientClick="if(ValidationSave()==false) return false;" runat="server" Text="Submit" />
                                            </div>
                                            <div class="col-sm-6 " style="padding-bottom: 8px;">
                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-sm btn-default" Text="Close" Width="100%"
                                                    Font-Size="14px"
                                                    data-dismiss="modal" aria-hidden="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="LBtnDownload" />
                                <asp:PostBackTrigger ControlID="btnSubmit" />
                                <asp:PostBackTrigger ControlID="LBtnDownloadOld" />
                            </Triggers>
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
                                    <asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" /></div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>

    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            $('#loading').fadeOut("fast");
        });
    </script>
    <script type="text/javascript">
        function ShowLoading() {
            $('#loading').show();
        }
    </script>
    <script type="text/javascript">
        function CloseLoading() {
            $('#loading').fadeOut("fast");
        }
    </script>

    <%--script open modal--%>
    <script type="text/javascript">
        function SidebarMenu() {
            try {
                var SideBarMenu = document.getElementById("SideBarMenu");
                if (SideBarMenu.style.display === "none") {
                    SideBarMenu.style.display = "block";
                    //$("#SideBarMenu").toggle(500, "easeOutQuint");
                } else {
                    //$("#SideBarMenu").toggle(500, "easeOutQuint");
                    SideBarMenu.style.display = "none";
                }
            }
            catch (err) {
                alert(err + ": SidebarMenu()");
            }
        }

        function CheckFileUpload() {
            var FU = document.getElementById("FuAttachment").files.length;
            if (FU == 0) {
                alert('No attachment to download');
                return false;
            }
        }
        function CheckFileUploadOld() {
            var FU = $("#TxtFilePath").val();
            if (FU == "") {
                alert('No attachment to download');
                return false;
            }
        }

        function CheckFileSize() {
            //1 mb = 1024kb
            //1 kb = 1024 byte
            //1 mb = 1024 * 1024 = 1048576
            //3 mb = 3 * 1048576 = 3145728
            var fileSize = document.getElementById("FuAttachment").files[0].size;
            if (fileSize <= 3145728) {
                return true;
            }
            else {
                var MB = fileSize / 1048576;
                document.getElementById("FuAttachment").value = '';
                alert("File is too large, Maximum file size 3 Mb. File  Size: " + MB.toFixed(1) + " Mb");
                return false;
            }
        }

        function ChangeAttachment() {
            var BtnChangeAttachment = document.getElementById("BtnChangeAttachment").value;
            var TxtFilePath = document.getElementById("TxtFilePath").value;
            var DvChkDeleteAttachment = document.getElementById("DvChkDeleteAttachment");
            var DvAttachmanet = document.getElementById("DvAttachmanet");
            var DvdAttachmanetOld = document.getElementById("DvdAttachmanetOld");
            var DvImagePrev = document.getElementById("DvImagePrev");

            if (BtnChangeAttachment == "Change Attachment") {
                DvAttachmanet.style.display = "block";
                DvdAttachmanetOld.style.display = "none";
                DvChkDeleteAttachment.style.display = "none";
                document.getElementById("BtnChangeAttachment").value = "Cancel Change Attachment";
                document.getElementById("ChkDeleteAttachment").checked = false;
            }
            else {
                DvAttachmanet.style.display = "none";
                DvdAttachmanetOld.style.display = "block";
                if (TxtFilePath != "") {
                    DvChkDeleteAttachment.style.display = "block";
                }
                document.getElementById("BtnChangeAttachment").value = "Change Attachment";
                document.getElementById("ChkDeleteAttachment").checked = false;
            }
            document.getElementById("FuAttachment").value = '';
            DvImagePrev.style.display = "none";
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

    <%--validation--%>
    <script type="text/javascript">
        function ValidationSave() {
            if ($("#TxtSubject").val() == "") {
                alert('please enter Subject');
                return false;
            }
            if ($("#TxtContent").val() == "") {
                alert('please enter Content');
                return false;
            }
        }
    </script>
</body>

</html>
