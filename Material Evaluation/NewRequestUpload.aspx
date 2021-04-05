<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewRequestUpload.aspx.cs" Inherits="Material_Evaluation.NewRequestUpload" %>

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
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
     <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <style type="text/css">
        .ui-datepicker .ui-datepicker-next {
        background-color : white !important;
        }
        .ui-datepicker .ui-datepicker-prev {
        background-color : white !important;
        }
        .ui-datepicker .ui-datepicker-prev span, .ui-datepicker .ui-datepicker-next span {
             background-color : white;
        }
        .SideBarMenu {
          width:300px;
          }
        .lbattachpad {
        padding:2px 2px 0px 2px;
        }
        .lbattachpad:hover {
        padding:2px;
        }
        .lbPreview {
        padding-top:1px;
        }
        .lbPreview:hover {
        padding-top:1px;
        }
        select[disabled] { background-color: #EBEBE4; }
        .WrapCnt td,th{
         white-space: normal !important; 
         /*word-wrap: break-word;*/  
         font-size:14px !important;
        }

        .selectedCell {
            background-color: lightblue;
        }

        .unselectedCell {
            background-color: white;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div id="loading" class="col-md-12" style="padding-top:200px;" >
                <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                <div class="col-md-12" style="text-align:center; opacity:1;">
                    <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                </div>
            </div>
        </div>
        
        <!-- Header -->
        <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
        <ContentTemplate>
        <div class="container-fluid">
            <div class="col-md-12" style="padding:5px;">
            <div class="row">
                <div class="col-md-10" style="padding-top:5px;">
                    <a onclick="ShowLoading();" href="Home.aspx"><asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                        <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> 
                        </asp:LinkButton>
                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-md-2 fa-pull-right" style="background-color:#E9ECEF;">
                    <asp:Label ID="lbluser1"  runat="server" Width="147px"></asp:Label><br />
                    <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
                    <asp:LinkButton runat="server"  ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
                </div>
            </div>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>

        <div id="wrapper">
            <!-- Sidebar -->
            <div id="SideBarMenu" style="width:300px;" runat="server" class="SideBarMenu">
            <ul class="sidebar">
                <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                    <i class="fas fa-fw fa-tachometer-alt"></i>
                    <span >Home</span>
                  </a>
                </li>
         

                <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=2" >
                    <i class="fas fa-fw fa-newspaper"></i>
                        <span >Create Request</span></a>
                </li>

                 <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                    <i class="fas fa-fw fa-table" ></i>
                   <span > Revision of MET</span></a>
			
                </li>

                <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=1">
                    <i class="fas fa-fw fa-chart-area"></i>
                    <span >Price Revision</span></a>
                </li>
      
		         <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                    <i class="fas fa-fw fa-table"></i>
                    <span >PIR Generation</span></a>

                </li>

          	        <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                    <i class="fas fa-fw fa-table"></i>
                    <span >Reports</span></a>

                </li>

                  <li class="sideMenu">
                      <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=18">
                        <i class="fas fa-fw fa-table"></i>
                        <span style="">Log Vendor Password Changes</span></a>

                    </li>
                <li class="sideMenu">
                  <a class="linkMenu" onclick="ShowLoading();" href="aboutemet.aspx">
                    <i class="fas  fa-fw fa-info"></i>
                    <span> About</span></a>

                </li>
              </ul>
            </div>

            <div id="content-wrapper" style="background-color:white;">
                <div class="container-fluid">
                    <div class="row" style="padding-bottom:10px;">
                        <div class="col-md-12">
                            <div class="col-md-12 card" style="padding:10px;background-color:white;">
                                <div class="col-md-12 card-body Padding-Nol">
                                    <div class="col-md-12" style="background-color:rgba(0,0,0,.03);">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <div class="row" style="padding-top:5px; padding-bottom:5px;">
                                            <div class="col-md-12">
                                                <asp:Label ID="lbTitle" runat="server" Text="New Request &nbsp;- Upload" Font-Bold="true" Font-Size="Large"/>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="col-md-12">
                                                        <div class="col-md-12" style="border-bottom:2px solid #006EB7"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <div class="row" style="padding-bottom:10px;">
                                            <div class="col-md-8">
                                                <asp:FileUpload runat="server" ID="FlUpload" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button runat="server" ID="BtnUpload" CssClass="btn btn-primary btn-sm btn-block" 
                                                    Text="Upload" OnClick="BtnUpload_Click" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button runat="server" ID="BtnTemplate" CssClass="btn btn-primary btn-sm btn-block" 
                                                    Text="Download Template" OnClick="BtnTemplate_Click" />
                                            </div>
                                        </div>

                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                        <%--Invalid data from file upload--%>
                                        <div class="row"  runat="server" id="DvInvalidData">
                                            <div class="col-md-12" style="padding-bottom:0px;">
                                            <div class="col-md-12 Padding-Nol">
                                                <asp:Label runat="server" ID="LbTitleInvalidData" Text="Invalid Data" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="col-lg-12 table-sm table-responsive" style="padding:0px; ">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GvInvalid" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                            AutoGenerateColumns="False"  OnRowDataBound="GvInvalid_RowDataBound"
                                                            CssClass="table table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt" >
                                                            <HeaderStyle  HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No.">
                                                                    <ItemTemplate> <%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PIRNo" HeaderText="PIR No" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                            <PagerSettings PageButtonCount="10" />
                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                            </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </div>
                                        </div>

                                        <%--valid data from file upload--%>
                                        <div class="row"  runat="server" id="DvValidData">
                                            <div class="col-md-12" style="padding-bottom:0px;">
                                            <div class="col-md-12 Padding-Nol">
                                                <asp:Label runat="server" ID="Label1" Text="Valid Data" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="col-lg-12 table-sm table-responsive" style="padding:0px; ">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GvValidData" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                            AutoGenerateColumns="False"  OnRowDataBound="GvValidData_RowDataBound"
                                                            CssClass="table table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt" >
                                                            <HeaderStyle  HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No.">
                                                                    <ItemTemplate> <%#(Container.DataItemIndex+1)%> </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PIRNo" HeaderText="PIR No" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialCode" HeaderText="Material Code" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                            <PagerSettings PageButtonCount="10" />
                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                            </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="DvFormContrl1" visible="false">
                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="Label2" Text="Cost"></asp:Label>
                                            </div>
                                            <div class="col-md-10">
                                                <asp:CheckBox runat="server" ID="ChcMatCost" Text="Material Cost" Checked="true"/>
                                                <asp:CheckBox runat="server" ID="ChcProcCost" Text="Process Cost" Enabled="false"/>
                                                <asp:CheckBox runat="server" ID="ChcSubMat" Text="Sub Mat Cost" Enabled="false"/>
                                                <asp:CheckBox runat="server" ID="ChcOthMat" Text="Others Cost" Enabled="false"/>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="DvFormContrl2" visible="false">
                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="Label3" Text="Response Due Date"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="group-main">
                                                    <div style="padding:0px;">
                                                        <asp:TextBox ID="TxtResDueDate" OnclientClick="return false;"
                                                            onkeydown="javascript:preventInput(event);" 
                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                            runat="server"  ForeColor="Black">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:1px 3px 0px 1px;">
                                                        <span class="fa fa-calendar" runat="server" id="IcnCalResduedate" style="color:#005496;"></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Label runat="server" ID="Label4" Text="Valid Date"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="group-main">
                                                    <div style="padding:0px;">
                                                        <asp:TextBox ID="TxtValidDate" OnclientClick="return false;"
                                                            onkeydown="javascript:preventInput(event);" 
                                                            autocomplete="off" AutoCompleteType="Disabled"
                                                            runat="server"  ForeColor="Black">
                                                        </asp:TextBox>
                                                    </div>
                                                    <span class="SearchBox-btn-cal" style="background-color:#E9ECEF; padding:1px 3px 0px 1px;">
                                                        <span class="fa fa-calendar" runat="server" id="Span1" style="color:#005496;"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom:10px;" runat="server" id="DvCreateReq" visible="false">
                                            <div class="col-md-2 pull-right">
                                                <asp:Button runat="server" ID="BtnCreateRequest" CssClass="btn btn-primary btn-sm btn-block" 
                                                    Text="Create Request" OnClick="BtnCreateRequest_Click" />
                                            </div>
                                        </div>

                                        <%--list data request quote--%>
                                        <div class="row" runat="server" id="DvGvReqList">
                                            <div class="col-md-12" style="padding-bottom:0px;">
                                            <div class="col-lg-12 table-sm table-responsive" style="padding:0px; ">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GvReqList" runat="server" ShowHeaderWhenEmpty="true" Width="100%" BackColor="White"
                                                            AutoGenerateColumns="False"  OnRowDataBound="GvReqList_RowDataBound"
                                                            CssClass="table table-responsive  table-sm table-bordered table-hover Padding-Nol WrapCnt" >
                                                            <HeaderStyle  HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:BoundField DataField="ReqDate" HeaderText="Material type" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ReqNo" HeaderText="Req No" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Plant" HeaderText="Plant" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="CompMaterial" HeaderText="Comp Material" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="CompMaterialDesc" HeaderText="Comp Material Desc" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="PICName" HeaderText="PIC Name" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="PICEmail" HeaderText="PIC Email" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Crcy" HeaderText="Crcy" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="SearchTerm" HeaderText="Search Term" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="AmtSCur" HeaderText="Amt SCur" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="ExchRate" HeaderText="Exch Rate" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="SellingCrcy" HeaderText="Selling Crcy" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="AmtVCur" HeaderText="Amt VCur" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="Unit" HeaderText="Unit" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                                <asp:BoundField DataField="UoM" HeaderText="UoM" ItemStyle-HorizontalAlign="left" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                                            </Columns>
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                            <PagerSettings PageButtonCount="10" />
                                                            <PagerStyle BackColor="#006DB4" ForeColor="White" HorizontalAlign="Center" CssClass="pagination-sm" />
                                                            </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-bottom:10px;" runat="server" id="DvSubmit" visible="false">
                                            <div class="col-md-10">
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button runat="server" ID="BtnSubmit" CssClass="btn btn-primary btn-sm btn-block" 
                                                    Text="Submit" OnClick="BtnSubmit_Click" />
                                            </div>
                                        </div>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    <!-- Footer -->
    <div class="container-fluid" style="background-color:#F5F5F5">
        <div class="row">
            <div class="col-md-12" style="padding:5px; align-content:center; text-align:center">
                <span style="font:bold 13px calibri, calibri">Copyright © ShimanoDT 2018</span>
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
                    <div class="modal-header" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-bottom-left-radius: 15px;border-bottom-right-radius: 15px;">
                        <div class="col-md-12 Padding-Nol" style="font:bold 22px calibri, calibri; text-align:center; align-content:center;"> Your Session Is About To Expire !!  </div>
                      <h4></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-2" style="padding:10px">
                                        <asp:Image ID="ImagWarning" runat="server" class="responsive" ImageUrl="~/js/jsextendsession/images/timeout-icon.png"/>
                                    </div>
                                    <div class="col-xs-10" style="padding:10px">
                                        <asp:Timer ID="TimerCntDown" runat="server" Interval="1000" OnTick="TimerCntDown_Tick" Enabled="false"></asp:Timer>
                                        You will be logged out in : <asp:Label ID="countdown" runat="server" Font-Bold="true" ForeColor="Red" Text="30"></asp:Label> seconds<br />
                                        do u want to stay Sign In?
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer" style="background: linear-gradient(90deg, #F5F5F5, #ffffff, #F5F5F5);border-top-left-radius: 15px;border-top-right-radius: 15px;">
                        <asp:Button ID="BtnRefresh" runat="server" Text="Yes, Keep me Sign In" OnClick="BtnRefresh_Click" CssClass="btn btn-sm btn-primary" Font-Names="calibri" Font-Size="18px"/>
                        <asp:Button ID="CtnCloseMdl" runat="server" Text="No, Sign Me Out" OnClick="CtnCloseMdl_Click" CssClass="btn btn-sm btn-default" Font-Names="calibri" Font-Size="18px"/>
                        <div style="display:none;"><asp:Button ID="StartTimer" runat="server" Text="Start" OnClick="StartTimer_Click" CssClass="btn btn-sm btn-primary" /></div>
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
    <%--<script src="vendor/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>

    <%--script loading page--%>
    <script lang="javascript" type="text/javascript">
         $(window).load(function() {
             $('#loading').fadeOut("fast");
         });

         $(document).ready(function () {
             DatePitckerStatic();
         });
    </script>
    <script type="text/javascript">
        function SidebarMenu()
        {
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

        function DatePitcker(txtID) {
            try {
                (function ($) {
                    $('#' + txtID).datepicker({
                        buttonImageOnly: true,
                        minDate: 0,
                        dateFormat: "dd/mm/yy"
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ": DatePitcker(txtID)");
            }
        }
        function DatePitckerStatic() {
            try {
                (function ($) {
                    $('#TxtValidDate').datepicker({
                        buttonImageOnly: true,
                        minDate: 0,
                        dateFormat: "dd/mm/yy"
                    });
                    $('#TxtResDueDate').datepicker({
                        buttonImageOnly: true,
                        minDate: 0,
                        dateFormat: "dd/mm/yy"
                    });
                })(jQuery);
            }
            catch (err) {
                alert(err + ": DatePitcker(txtID)");
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
           });(jQuery)


           $(document).on('keypress', '#txtDate', function (event) {
               var regex = new RegExp("^[a-zA-Z ]+$");
               var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
               if (!regex.test(key)) {
                   event.preventDefault();
                   return false;
               }
           });
        </script>
</body>
</html>
