<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteCostPlanMng.aspx.cs" Inherits="Material_Evaluation.QuoteCostPlanMng" %>

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

    <style type="text/css">
          .SideBarMenu {
          width:300px;
          }
          .lbattachpad {
        padding:2px;
        }
        .lbattachpad:hover {
        padding:2px;
        }
    </style>
   </head>
    <body id="page-top">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="scriptmanager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
           <%-- <div class="row">
                <div id="loading" class="col-sm-12" style="padding-top:200px;" >
                    <img id="loading-image" src="images/loading.gif" alt="Loading..."/>
                    <div class="col-sm-12" style="text-align:center; opacity:1;">
                        <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff"></asp:Label>
                    </div>
                </div>
            </div>--%>

            <!-- Header -->
            <asp:UpdatePanel runat="server" ID="UpsidebarToggle">
            <ContentTemplate>
            <div class="container-fluid">
            <div class="col-lg-12" style="padding:5px;">
            <div class="row">
                <div class="col-sm-10" style="padding-top:5px;">
                    <a onclick="ShowLoading();" href="Home.aspx"><asp:Image ID="Image1" runat="server" Height="31px" ImageUrl="~/images/logo.gif" Width="179px" /></a>
                    <asp:LinkButton runat="server" OnClientClick="SidebarMenu();" class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" OnClick="sidebarToggle_Click"><i class="fas fa-bars"></i> </asp:LinkButton>
                    <asp:Image ID="Image2" runat="server" Height="24px" ImageUrl="~/images/caption1.gif" Width="71px" />
                    <asp:Label runat="server" ID="LbsystemVersion" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-2 fa-pull-right" style="background-color:#E9ECEF;">
                    <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label><br />
                    <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
                    <asp:LinkButton runat="server"  ID="BtnLogOut" OnClick="BtnLogOut_Click" Text="Logout"></asp:LinkButton>
                </div>
            </div>
            </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>

            <div id="wrapper" class="bg-white">
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
              <a class="linkMenu" onclick="ShowLoading();" href="Revision.aspx">
                <i class="fas fa-fw fa-table" ></i>
               <span > Revision of MET</span></a>
			
            </li>

            <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="MassRevision.aspx">
                <i class="fas fa-fw fa-chart-area"></i>
                <span >Mass Revision</span></a>
            </li>
      
		     <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="Emet_author.aspx?num=16">
                <i class="fas fa-fw fa-table"></i>
                <span >PIR Generation</span></a>

            </li>

          	    <li class="sideMenu">
              <a class="linkMenu" onclick="ShowLoading();" href="PIRGenMassRev.aspx">
                <i class="fas fa-fw fa-table"></i>
                <span >PIR Generation Mass Revision</span></a>

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
                <!-- Content -->
                <div id="content-wrapper">
                    <div class="container-fluid">
                        <div class="card" style="background-color:white">
                            <%-- <div class="card-header">
                                <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION
                            </div>--%>
                            <div class="card-body">
                                <div class="col-md-12" style="background-color:rgba(0,0,0,.03);padding-top:10px;">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <asp:Label ID="lbTitle" runat="server" Text="Review (Vendor Draft)" Font-Bold="true" Font-Size="Large"/>
                                        </div>
                                    </div>
		                        </div>
                                <div class="col-md-12" style="padding-top:5px;background-color:rgba(0,0,0,.03);">
                                    <div class="col-md-12" style="border-bottom:2px solid #006EB7"></div>
                                </div>
                                <%--entrydata--%>
                                <div class="col-md-12" style="background-color:rgba(0,0,0,.03);">
                                    <div class="row" style="padding-bottom:10px; padding-top:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label16" runat="server" Text="QUOTATION STATUS"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:5px">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-4"> 
                                                    <asp:Label ID="Label18" runat="server" Text="Approved/Rejected By"></asp:Label>
                                                </div>
                                                <div class="col-md-8"> 
                                                    <asp:Label ID="LbUpByStatsQuot" runat="server" Text=": xxxxxxx"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                             <div class="row">
                                                <div class="col-md-4"> 
                                                    <asp:Label ID="Label21" runat="server" Text="Updated Date"></asp:Label>
                                                </div>
                                                <div class="col-md-8"> 
                                                    <asp:Label ID="LbUpDateStatsQuot" runat="server" Text=": xxxxxxx"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:5px">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-4"> 
                                                    <asp:Label ID="Label22" runat="server" Text="Status"></asp:Label>
                                                </div>
                                                <div class="col-md-8"> 
                                                    <asp:Label ID="LbStatsQuot" runat="server" Text=": xxxxxxx"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-4"> 
                                                    <asp:Label ID="Label23" runat="server" Text="Comment"></asp:Label>
                                                </div>
                                                <div class="col-md-8"> 
                                                    <asp:Label ID="LbCommentStatsQuot" runat="server" Text=": xxxxxxx"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px; padding-top:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label8" runat="server" Text="VENDOR DETAILS"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                     <div class="row" style="padding-bottom:5px">
                                        <div class="col-md-3">
                                            <asp:Label ID="Label10" runat="server" Text="Submitted By:"></asp:Label> 
                                        </div>
                                        <div class="col-md-5">
                                             <asp:Label ID="Label14" runat="server" Text="" Visible="true"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label ID="Label15" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="Label11" runat="server" Text="" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px; padding-top:5px;">
                                        <div class="col-md-3">
                                            <asp:Label ID="lblreqst" runat="server" Text="Quote No :"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hdnQuoteNo" />
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label ID="lblVName" runat="server" Text="Vendor Name :"></asp:Label>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCurrency" runat="server" Text="Currency :"></asp:Label>
                                        </div>
                                        <div class="col-md-2">
                                           <asp:Label ID="lblCity" runat="server" Text="City :"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label26" runat="server" Text="SHIMANO DETAILS"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label17" runat="server"  ForeColor="Black" Text="SMN PIC"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtsmnpic" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_addres" runat="server"  ForeColor="Black" Text="Email"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtemail" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                   <asp:Label ID="lbl_cntact" runat="server"  ForeColor="Black" Text="Plant & Department"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="TxtPlant" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="TxtDepartment" runat="server" ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Labelquote" runat="server"  ForeColor="Black" Text="Quote Response Due Date"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtquotationDueDate" runat="server" ForeColor="Black" BackColor="#E6E6E6" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label2" runat="server" Text="PART I: QUOTED PART INFO"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_partdesc" runat="server"  ForeColor="Black" Text="Part Code & Desc"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtpartdesc" Enabled="false" runat="server" Height="55px" 
                                                        TextMode="MultiLine" ForeColor="Black" Width="100%" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row" runat="server" id="DvProduct">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_cntact0" runat="server"  ForeColor="Black" Text="Product"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtprod" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row" runat="server" id="DvReqPlant" style="display:none;">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label33" runat="server"  ForeColor="Black" Text="GP Request Plant"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtReqPlant" Enabled="false" runat="server" Height="55px" 
                                                        TextMode="MultiLine" ForeColor="Black" Width="100%" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;" runat ="server" id="DvSAPPIR">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_cntact4" runat="server"  ForeColor="Black" Text="SAP PIR Job Type &amp; Desc"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtSAPJobType" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_PIR" runat="server"  ForeColor="Black" Text="PIR Type & Desc"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtPIRtype" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_partDRG" runat="server"  ForeColor="Black" Text="Part Drawing"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtdrawng" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="lbl_proces" runat="server"  ForeColor="Black" Text="Process Group"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtprocs" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="txtPartUnit" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label27" runat="server"  ForeColor="Black" Text="Base UOM"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtBaseUOM" Enabled="false" runat="server" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                              

                                         <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label5" runat="server" Text="Net Weight:" 
                                                ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <asp:TextBox ID="txtunitweight"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:TextBox ID="txtUOM"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                                </div>
                                                                </div>
                                                              </ContentTemplate>
                                                
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">

                                          <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label20" runat="server" Text="Request Purpose"  Enabled="false"
                                                ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                                        <ContentTemplate>
                                                           <asp:TextBox ID="txtRem"  Enabled="false"  runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                        </ContentTemplate>
                                                 
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label19" runat="server" Text="Mnth.Est.Qty & UOM:" 
                                                ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <asp:TextBox ID="txtMQty"  Enabled="false"  runat="server" AutoPostBack="true" Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                   <asp:TextBox ID="txtBaseUOM1"  Enabled="false"  runat="server" AutoPostBack="true"  Width="100%" AutoCompleteType="Disabled" autocomplete="off" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                              </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="txtpartdesc" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                    <!-- start whitout code gp field -->
                                    <div runat="server" id="DvWhitoutCodeGpField" style="display:none;">
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label24" runat="server" Text="FA Date & Qty" ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="TxtFADate" OnclientClick="return false;" Enabled="false" runat="server"  ForeColor="Black" ></asp:TextBox>
                                                        </div>
                                                            <div class="col-md-6">
                                                                <asp:TextBox ID="TxtFAQty" runat="server" Width="100%" Enabled="false" ></asp:TextBox>
                                                            </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label25" runat="server" Text="1<sup>st</sup> Delivery Date & Qty" ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <asp:TextBox ID="TxtDelDate" runat="server" Enabled="false" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                   <asp:TextBox ID="TxtDelQty" runat="server"  Width="100%" Enabled="false" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label29" runat="server" Text="Packing Requirements"  Enabled="false"
                                                ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtPckRequirement" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label30" runat="server" Text="Others Requirements"  Enabled="false" ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtOthRequirement" runat="server" Enabled="false" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label32" runat="server" Text="Incoterms" ></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtIncoterms" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label31" runat="server" Text="GP Request Plant"  ForeColor="Black"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TxtPlantRequestor" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <!-- end whitout code gp field -->

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label12" runat="server"  ForeColor="Black" Text="Effective Date"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="TextBox1" runat="server" Enabled="false" ForeColor="Black"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <asp:Label ID="Label13" runat="server"  ForeColor="Black" Text="Due Dt Next Rev"></asp:Label>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtfinal" runat="server" Enabled="false"  ForeColor="Black"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-5">
                                                   <asp:Label ID="Label28" runat="server"  ForeColor="Black" Text="Country Of Origin"></asp:Label> 
                                                </div>
                                                <div class="col-md-7">
                                                   <asp:TextBox ID="txtCorigin" Enabled="false" runat="server" Height="27px" Width="100%" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-lg-5">
                                                <asp:Label ID="Label34" runat="server"  ForeColor="Black" Text="Attachment"></asp:Label> 
                                            </div>
                                            <div class="col-lg-7">
                                                <asp:UpdatePanel ID="UpdatePanel22" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="LbFlName" runat="server"  ForeColor="Black" Text="No File" onclick="ClcBtnFlUpload();" ></asp:Label>
                                                    <asp:LinkButton runat="server" ID="BtnPreview" OnClientClick="return CheckFileUpload();" OnClick="BtnPreview_Click"
                                                         CssClass="lbattachpad pull-right">
                                                        <span class="glyphicon glyphicon-download" style="padding:0px;color:#0bd409;" runat="server" id="Span1"> </span>
                                                    </asp:LinkButton>
                                                    <div style="display:none">
                                                        <asp:Label ID="LbFlNameOri" runat="server"  ForeColor="Black" Text="No File" ></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="BtnPreview" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    </div>

                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12" style="padding-bottom:5px;">
                                                    <div class="col-md-12 Padding-Nol" style="border-bottom:2px double blue;">
                                                        <asp:Label ID="lbl_proces0" runat="server"  ForeColor="Black" 
                                                        Text="SMN BOM & Material Cost Details :" Visible="False"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="table table-responsive table-sm">
                                                        <asp:UpdatePanel ID="UpGvGridView1"  runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"  RenderMode="Block" >
                                                        <ContentTemplate>
                                                            <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="false" CssClass="table-nowrap" OnRowDataBound="GridView1_RowDataBound"
                                                                ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" BackColor="White" 
                                                                BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%" Visible="false" >
                                                            <Columns>
                                                                <asp:BoundField DataField="material" HeaderText="Material" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="MaterialDesc" HeaderText="MaterialDesc" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Selling_Crcy" HeaderText="Selling_Crcy" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>

                                                                  <asp:BoundField DataField="OAmount" HeaderText="Amt_SCur" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                  
                                                                <asp:BoundField DataField="Venor_Crcy" HeaderText="Venor_Crcy" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Amount" HeaderText="Amt_VCur" ItemStyle-Width="150px" >
                                                                <ItemStyle Width="150px" />
                                                                </asp:BoundField>

                                                            </Columns>
                                                    
                                
                                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                                            <HeaderStyle BackColor="#006699"  ForeColor="White" />
                                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                            <RowStyle ForeColor="#000066" />
                                                            <SelectedRowStyle BackColor="#669999"  ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    
                                
                                                        </asp:GridView>
                                                        </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                     </div>
                                    
                                    <%--Data for previous quotation--%>
                                    <div class="row" style="padding-bottom:10px;display:block;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label9" runat="server" Text="Previous Quotation"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 table table-responsive" style="padding:0px;display:block;">
                                        <asp:UpdatePanel ID="UpdatePanel1"  runat="server" ChildrenAsTriggers="true" 
                                            UpdateMode="Conditional"  RenderMode="Block">
                                            <ContentTemplate>
                                                <asp:GridView ID="GdvPrevQuote" runat="server" AutoGenerateColumns="False" 
                                                                OnRowDataBound="GdvPrevQuote_RowDataBound" OnRowCommand="GdvPrevQuote_RowCommand"
                                                                CssClass="table-responsive  table-sm table-bordered table-nowrap  Padding-Nol" >
                                                                <Columns>
                                                                    <asp:BoundField DataField="RequestDate" HeaderText="Req. Date" ItemStyle-Width="150px" SortExpression="RequestDate"></asp:BoundField>
                                                                    <asp:BoundField DataField="RequestNumber" HeaderText="Request Number" ></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="QuoteNo" >
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton Text='<%# Eval("QuoteNo") %>' ItemStyle-Width="150px" runat="server" 
                                                                        CommandName="LinktoRedirect" CommandArgument='<%((GridViewRow) Container).RowIndex %>' OnClientClick="ShowLoading();"/>
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="TotalMaterialCost" HeaderText="Tot. Material Cost" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="TotalProcessCost" HeaderText="Tot. Process Cost" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="Tot. Sub Material Cost" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="Tot. Other items Cost" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="GrandTotalCost" HeaderText="Grand Total Cost" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="Profit" HeaderText="Profit %" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="Discount" HeaderText="Discount %" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="FinalQuotePrice" HeaderText="Final Quote Price" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                    <asp:BoundField DataField="NetProfit/Discount" HeaderText="Prof/Disc" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                                                                    <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                    <asp:BoundField DataField="DueOn" HeaderText="Due Dt Next Rev" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                                                    <asp:BoundField DataField="ApprovebyDIR" HeaderText="Approve by DIR" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                    <asp:BoundField DataField="DIRApprovalDate" HeaderText="DIR Approval Date" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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

                                    <div class="row" style="padding-bottom:10px">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="lblmatlcost" runat="server" Text="PART II: MATERIAL COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <%--table material cost--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="Table1" runat="server" CssClass="table-bordered table-nowrap"></asp:Table>
                                            </div>
                                        </div>
                                    </div>

                                    <%--label part III--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label4" runat="server" Text="PART III: PROCESS COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table process cost--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TablePC" runat="server" CssClass="table-bordered table-nowrap" >
                                                    </asp:Table>
                                            </div>
                                        </div>
                                    </div>

                                    <%--label part IV--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label3" runat="server"  Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table SUB-MAT/T&J COST--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TableSMC" runat="server" CssClass="table-bordered table-nowrap">
                                                    </asp:Table>
                                            </div>
                                        </div>
                                    </div>

                                    <%--label part V--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label6" runat="server" Text="PART V: OTHER COST"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table OTHER COST--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TableOthers" runat="server" CssClass="table-bordered table-nowrap"></asp:Table>
                                            </div>
                                        </div>
                                    </div>

                                    <%--label part VI PART UNIT PRICE--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="col-md-12 title">
                                                <asp:Label ID="Label7" runat="server"  Text="PART VI: PART UNIT PRICE"></asp:Label>
                                                <asp:Label ID="lblcry" runat="server" ForeColor="Yellow" ></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <%--table PART UNIT PRICE--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="table table-responsive table-sm">
                                                <asp:Table ID="TableUnit" runat="server" CssClass="table-bordered table-nowrap"></asp:Table>
                                            </div>
                                        </div>
                                    </div>

                                    <%--comment by vendor--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <asp:Label ID="lbComnt" runat="server" Text ="Comment By Vendor"></asp:Label>
                                                </div>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="TxtComntByVendor" Enabled="false" runat="server" Height="55px" TextMode="MultiLine"  MaxLength="150" Width="100%" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--back to Home--%>
                                    <div class="row" style="padding-bottom:10px;">
                                        <div class="col-md-12 text-right">
                                            <asp:Button ID="Button1" runat="server" PostBackUrl="~/Home.aspx" Text="Back to Home" CssClass="btn btn-primary"  />
                                        </div>
                                    </div>

                                </div>

                                <asp:Label ID="Label1" runat="server" Text="Vendor Details"  Font-Names="calibri" Font-Size="16px" ForeColor="#2153a5" Visible="false"></asp:Label>

                                            <asp:GridView ID="grdVendrDet" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="table-nowrap"
                                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                <Columns>
                                                    <asp:BoundField DataField="REQUESTDATE" HeaderText="Request Date" />
                                                    <asp:BoundField DataField="QUOTENO" HeaderText="Quote No" />
                                                    <asp:BoundField DataField="Description" HeaderText="Vendor Name" />
                                                    <asp:BoundField DataField="Crcy" HeaderText=" Quote Currency" />
                                                    <asp:BoundField DataField="PICName" HeaderText="PIC Name" />
                                                    <asp:BoundField DataField="PICemail" HeaderText="PIC Email" />
                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <HeaderStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>

                                            <asp:GridView ID="grdProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                <Columns>
                                                 
                                                    <asp:BoundField DataField="ProcessGrpCode" HeaderText="Process Grp Code" />
                                                    <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                                    <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                                    <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                                

                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <HeaderStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>

                                            <asp:GridView ID="grdMachinelisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />

                                                <Columns>
                                                    <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                                    <asp:BoundField DataField="SMNStdrateHr" HeaderText="SMNStdrateHr" />
                                                    <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                    <asp:BoundField DataField="Currency" HeaderText="CURR" />

                                                </Columns>

                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <HeaderStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>

                                            <asp:GridView ID="grdLaborlisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                                Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />

                                                <Columns>
                                                    <asp:BoundField DataField="StdLabourRateHr" HeaderText="StdLabourRateHr" />
                                                    <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                    <asp:BoundField DataField="Currency" HeaderText="CURR" />

                                                </Columns>

                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <HeaderStyle BackColor="#1a2e4c"  ForeColor="White" />
                                                <PagerStyle BackColor="#1a2e4c" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                            </asp:GridView>
                                <asp:HiddenField ID="ddlSubprocess" runat="server" Value="" />
                                <asp:HiddenField ID="ddlUom" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTMatCost" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTProCost" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTSumMatCost" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTOtherCost" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTGTotal" runat="server" Value="" />
                                <asp:HiddenField ID="hdnTFinalQPrice" runat="server" Value="" />
                                <asp:HiddenField ID="hdnProfit" runat="server" Value="" />
                                <asp:HiddenField ID="hdnDiscount" runat="server" Value="" />

                                <asp:HiddenField ID="hdnSTDRate" runat="server" Value="" />
                                <asp:HiddenField ID="hdnVendorRate" runat="server" Value="" />
                                <asp:HiddenField ID="hdnUOM" runat="server" Value="" />
                                <asp:HiddenField ID="hdnVendorActivity" runat="server" Value="" />


                                <asp:HiddenField ID="hdnSMCTable" runat="server" Value="" />

                                <asp:HiddenField ID="hdnSMCTableValues" runat="server" Value="" />

                                <asp:HiddenField ID="hdnOtherValues" runat="server" Value="" />
                                <asp:HiddenField ID="hdnProcessValues" runat="server" Value="" />
                                <asp:HiddenField ID="hdnMCTableValues" runat="server" Value="" />

                                 <asp:HiddenField ID="hdnUnitValues" runat="server" Value="" />
                                 <asp:HiddenField ID="hdnLayoutScreen" runat="server" Value="" />
                                <asp:HiddenField ID="hdnVendorType" runat="server" Value="" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Footer -->
            <div class="container-fluid" style="background-color:#F5F5F5">
                <div class="row">
                    <div class="col-lg-12" style="padding:5px; align-content:center; text-align:center">
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
                        <div class="col-lg-12 Padding-Nol" style="font:bold 22px calibri, calibri; text-align:center; align-content:center;"> Your Session Is About To Expire !!  </div>
                      <h4></h4>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12">
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
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
    
        <%--script loading page--%>
        <script language="javascript" type="text/javascript">
             $(window).load(function() {
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
                var SideBarMenu = document.getElementById("SideBarMenu");
                if (SideBarMenu.style.display === "none") {
                    SideBarMenu.style.display = "block";
                    //$("#SideBarMenu").toggle(500, "easeOutQuint");
                } else {
                    //$("#SideBarMenu").toggle(500, "easeOutQuint");
                    SideBarMenu.style.display = "none";
                }
            }

            function freezeheader() {
                try {
                    (function ($) {
                        $("#Table1").tableHeadFixer({ 'left': 1 });
                        $("#TablePC").tableHeadFixer({ 'left': 1 });
                        $("#TableSMC").tableHeadFixer({ 'left': 1 });
                        $("#TableOthers").tableHeadFixer({ 'left': 1 });
                        $("#TableUnit").tableHeadFixer({ 'left': 1 });
                    })(jQuery);
                }
                catch (err) {
                    alert(err + ' : freezeheader');
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

            function CheckFileUpload() {
                try {
                    debugger;
                    var FL = document.getElementById("LbFlName").innerHTML;
                    if (FL == "No File") {
                        alert('No File to download');
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                catch (err) {
                    alert(err + ": CheckFileUpload")
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

        <%--additional script--%>
        <script type="text/javascript">
            $(document).ready(function () {
                try
                {
                    $(function () {
                        var hdnVendorType = $("#hdnVendorType").val();
                        if (hdnVendorType == "TeamShimano") {
                            var x = document.getElementById("TableOthers").rows.length;
                            var y = document.getElementById("TableOthers").rows[x - 1].cells[1].innerHTML;
                            document.getElementById("TableUnit").rows[4].cells[1].innerHTML = y;
                        }
                        else
                        {
                            var x = document.getElementById("TablePC").rows.length;
                            var y = document.getElementById("TablePC").rows[x - 1].cells[1].innerHTML;
                            document.getElementById("TableUnit").rows[2].cells[1].innerHTML = y;
                            var Grantot = document.getElementById("TableUnit").rows[5].cells[1].innerHTML;
                            var FinalGrantot = document.getElementById("TableUnit").rows[5].cells[4].innerHTML;
                            var NetprofDisc = null;
                            if (Grantot == "" && FinalGrantot == "") {
                                NetprofDisc = 0.0;
                            }
                            else {
                                NetprofDisc = (((FinalGrantot - Grantot) / FinalGrantot) * 100);
                            }
                            document.getElementById("TableUnit").rows[5].cells[5].innerHTML = NetprofDisc.toFixed(1) + ' %';
                        }
                    });

                    freezeheader();
                }
                catch(err)
                {
                    alert(err);
                }
        });
    </script>
    </body>
</html>
