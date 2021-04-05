<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WithSAPCode_Request.aspx.cs" Inherits="Material_Evaluation.WithSAPCode_Request.aspx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>eMET</title>
    <!-- Bootstrap core CSS-->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet">
    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet">
    <style type="text/css">
        .tetxboxcolor
        {
            background-color: #CAC8BF;
        }
        
        .GridStyle
        {
            font-family: calibri;
            font-size: 14px;
            overflow: auto;
            width: 100%;
        }
        
        
        .Login-button
        {
            border-radius: 5px;
            border: 1px solid #032742;
            background-color: #005496;
            color: #FFFFFF;
            font-family: Arial;
        }
        
        
        .HeaderStyle1
        {
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: Black;
        }
        .GridStyle, .GridStyle th, .GridStyle td
        {
            border: 1px solid #010a19;
        }
        
        
        .GridPosition
        {
            position: absolute;
            left: 100px;
            height: 200px;
            width: 200px;
        }
        
        .tdpossition
        {
            position: absolute;
            left: 250px;
            height: 200px;
            width: 200px;
            font-family: Calibri;
            color: #7da1db;
        }
        .style3
        {
            width: 100%;
        }
        .style5
        {
            width: 242px;
        }
        .style6
        {
            width: 281px;
        }
    </style>
</head>
<body id="page-top">
    <form id="form1" runat="server">
    <nav class="navbar navbar-expand navbar-dark bg-dark static-top">

      <a class="navbar-brand mr-1" href="index.html">eMET</a>

      <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
        <i class="fas fa-bars"></i>
      </button>

      <!-- Navbar Search -->
        <div class="input-group">
         
        </div>

      <!-- Navbar -->
      <ul class="navbar-nav ml-auto ml-md-0">
        <li class="nav-item dropdown no-arrow mx-1">
          <div class="dropdown-menu dropdown-menu-right" aria-labelledby="alertsDropdown">
            <a class="dropdown-item" href="#">Action</a>
            <a class="dropdown-item" href="#">Another action</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#">Something else here</a>
          </div>
        </li>
        <li class="nav-item dropdown no-arrow mx-1">
          <a class="nav-link dropdown-toggle" href="#" id="messagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            &nbsp;</a><div class="dropdown-menu dropdown-menu-right" aria-labelledby="messagesDropdown">
            <a class="dropdown-item" href="#">Approval Pending</a>
            <a class="dropdown-item" href="#">Revised Quote</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#">Something else here</a>
          </div>
        </li>
        <li class="nav-item dropdown no-arrow">
          <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-user-circle fa-fw"></i>
          </a>
          <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
            <a class="dropdown-item" href="#">Settings</a>
            <a class="dropdown-item" href="#">Activity Log</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">Logout</a>
          </div>
        </li>
      </ul>

    </nav>
    <div id="wrapper">
        <!-- Sidebar -->
        <ul class="sidebar navbar-nav">
            <li class="nav-item active"><a class="nav-link" href="Home.aspx"><i class="fas fa-fw fa-tachometer-alt">
            </i><span>Dashboard</span> </a></li>
            <li class="nav-item dropdown"><a class="nav-link dropdown-toggle" href="#" id="pagesDropdown"
                role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <i class="fas fa-fw fa-folder"></i><span>New Request</span> </a>
                <div class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <%-- <h6 class="dropdown-header">Active Material</h6>--%>
                    <a class="dropdown-item" href="NewRequest.aspx">Active Material</a> <a class="dropdown-item"
                        href="register.html">Vendor Change for Active Material</a> <a class="dropdown-item"
                            href="forgot-password.html">New Material(With and Without SAP Code)</a>
                    <%--<div class="dropdown-divider"></div>
            <h6 class="dropdown-header">Other Pages:</h6>
            <a class="dropdown-item" href="404.html">404 Page</a>
            <a class="dropdown-item" href="blank.html">Blank Page</a>--%>
                </div>
            </li>
            <li class="nav-item dropdown"><a class="nav-link dropdown-toggle" href="#" id="A1"
                role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <i class="fas fa-fw fa-folder"></i><span>Request Revision</span> </a>
                <div class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <%-- <h6 class="dropdown-header">Active Material</h6>--%>
                    <a class="dropdown-item" href="login.html">Active Material</a> <a class="dropdown-item"
                        href="register.html">Vendor Change for Active Material</a> <a class="dropdown-item"
                            href="forgot-password.html">New Material(With and Without SAP Code)</a>
                    <%--<div class="dropdown-divider"></div>
            <h6 class="dropdown-header">Other Pages:</h6>
            <a class="dropdown-item" href="404.html">404 Page</a>
            <a class="dropdown-item" href="blank.html">Blank Page</a>--%>
                </div>
            </li>
            <li class="nav-item"><a class="nav-link" href="NewRequest.aspx"><i class="fas fa-fw fa-chart-area">
            </i><span>New Request</span></a> </li>
            <li class="nav-item"><a class="nav-link" href="tables.html"><i class="fas fa-fw fa-table">
            </i><span>Change Request</span></a> </li>
            <li class="nav-item"><a class="nav-link" href="tables.html"><i class="fas fa-fw fa-table">
            </i><span>Approval pending</span></a> </li>
            <li class="nav-item"><a class="nav-link" href="tables.html"><i class="fas fa-fw fa-table">
            </i><span>PIR</span></a>
                <li class="nav-item"><a class="nav-link" href="tables.html"><i class="fas fa-fw fa-table">
                </i><span>Reports</span></a> </li>
            </li>
        </ul>
        <div id="header">
            <asp:ScriptManager ID="scriptmanager1" runat="server">
            </asp:ScriptManager>
        </div>
        <div id="content-wrapper">
            <div class="container-fluid">
                <!-- Breadcrumbs-->
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="#">New Request</a> </li>
                </ol>
                <!-- Icon Cards-->
                <div class="row">
                </div>
                <!-- Area Chart Example-->
                <div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION</div>
                    <div class="card-body">
                        <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->
                     
                       <table class="stylebody" border="3">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label1" runat="server" Text="Vendor Details" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server"  Text="Date of MET Request :" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                        <td id="Td1" runat="server">
                            <asp:TextBox ID="txtreqdate" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="lbl_vendrName" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Vendor Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvendr" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_autherisedPIC" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Authorised PIC :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_vendorpic" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="lblcurrency" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Autherised Email :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtauterisedEMAIL" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_PIC" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Height="27px" Text="Contact No :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtautherscontact" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label18" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Currency :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcurncy" runat="server" Height="27px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" ></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label26" runat="server" Text="Shimano Details" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5"></asp:Label>
                            &nbsp;
                        </td>
                 
                        <td>
                            <asp:Label ID="Label17" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="SMN PIC :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsmnpic" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_addres" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Email :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtemail" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lbl_cntact" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Contact No. :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcontact" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Quote Ref. No. :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtreq" runat="server" Height="27px" Style="margin-bottom: 0px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"
                                Width="300px"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART I: QUOTED PART INFO"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_cntact0" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Product :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprod" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="lbl_partdesc" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Part Desc :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpartdesc" runat="server" Height="41px" TextMode="MultiLine" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"
                                Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_cntact4" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="SAP PIR JOB TYPE :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSAPJobType" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="lbl_PIR" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="PIR Type :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPIRtype" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_partDRG" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="PART DRG :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdrawng" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lbl_proces" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Proces Group :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprocs" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label19" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="PIR Description :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpirdesc" runat="server" Height="27px" Width="300px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblmatlcost" runat="server" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5" Text="PART II: Material Cost"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblunit" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Part Net Unit Weight(g):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtunit" runat="server" Height="27px" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label9" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Cavity:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcavity" runat="server" Height="27px" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Material Yield/Meltting Loss(%):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmatlyield" runat="server" Height="27px" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="Label11" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Material Gross Weight/pc (g):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmatlgroswgt" runat="server" Height="27px" Text="" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label12" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Material Scrap Weight (g):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Height="27px" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label13" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Width(mm):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Height="27px" Text="" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label14" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Pitch/Length (mm):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" Height="27px" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label15" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Material Density:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" Height="27px" Text="" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div style="height: 150px; width: 90%; left: 100px; overflow: auto;" >
                            <%--<div style="height: 200px; width: 520px; overflow:scroll; overflow-y: hidden;" >--%>
                                <asp:GridView ID="grdmatlcost" runat="server" AutoGenerateColumns="False" BorderColor="Black"
                                     
                                    CellPadding="4" CssClass="GridStyle" ForeColor="#333333" GridLines="Both" 
                                    Height="150px">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField HeaderText="S.No" />
                                        <%--<asp:BoundField DataField="Type" HeaderText="Type of Material" />--%>
                                        <asp:BoundField DataField="materialcode" HeaderText="Material SAP Code" />
                                        <asp:BoundField DataField="matlgrade" HeaderText="Material Description" />
                                        <asp:BoundField DataField="rawmatlcost" HeaderText="Raw material Cost(Kg)" />
                                        <asp:BoundField DataField="totrawcost" HeaderText="Total Raw material Cost" />
                                        <asp:BoundField DataField="matlgrsweight" HeaderText="Material Gross Weight/pc (g)" />
                                        <asp:BoundField DataField="matlcost" HeaderText="Material Cost/pcs ($)" />
                                        <asp:BoundField DataField="totmatlcost" HeaderText="Total Material Cost/pcs ($)" />
                                        <asp:BoundField DataField="runrweight" HeaderText="Runner Weight/Shot (g)" />
                                        <asp:BoundField DataField="runrratio" HeaderText="Runner Ration/pcs (%)" />
                                        <asp:BoundField DataField="recyclematlratio" HeaderText="Recycle Matl Ratio (%)" />
                                        <asp:BoundField DataField="scrapweight" HeaderText="Material Scrap Weight (g)" />
                                        <asp:BoundField DataField="scraplosallow" HeaderText="Scrap Loss Allowance (%)" />
                                        <asp:BoundField DataField="scrappric" HeaderText="Scrap Price/kg ($)" />
                                        <asp:BoundField DataField="scraprebate" HeaderText="Scrap Rebate/pcs ($)" />
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Height="50px" CssClass="HeaderStyle1" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="right" Width="25px" />
                                    <RowStyle BackColor="#F7F6F3" CssClass="RowStyle" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART III: PROCESS COST"></asp:Label>
                        </td>
                        <td>
                        </td>
                        
                   
                        <td>
                            <asp:Label ID="Label10" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="If Turnkey - Vendor Name ? :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtturnkey" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_machne" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Machine /Labor :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmachinevend" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="lbl_baseqty" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Base qty :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtbaseqty" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblduration" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Duration per Process UOM (Sec) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtduration" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lblefeciancy" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Efficiency/Process Yield (%) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txteffic" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="grdproces" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                CellPadding="4" CssClass="GridStyle" DataKeyNames="processgrpdescription" ForeColor="#333333"
                                Height="157px" OnPageIndexChanging="OnPageIndexChanging" PageSize="5" ShowFooter="true" style="width:450px; overflow:auto" 
                                Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="processgrpdescription" HeaderText="ProcessDescritpion" />
                                    <asp:BoundField DataField="SubProcessName" HeaderText="Select Sub Process" />
                                    <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process Unit of Messure(UOM)" />
                                    <asp:BoundField DataField="HourlyRate" HeaderText="Standard Rate" />
                                    <asp:BoundField HeaderText="Sum of Process" />
                                    <asp:BoundField HeaderText="Process Cost/pcs ($)" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="right" Width="25px" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_submtlcost" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Sub-Mat/T&amp;J Description :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsubmtl" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="Label20" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text=" Sub-Mat/T&amp;J Cost ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsubmatlcost" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblcurrncy" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Currency :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcurrency" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="lblconsumption" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text=" Consumption (pcs) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtconsumption" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsubmatlcost" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Sub-Mat/T&amp;J Cost/pcs ($):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsubmatlcostTJ" runat="server" CssClass="tetxboxcolor" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="lbltotsubmtlcost" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Sub-Mat/T&amp;J Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotsubmatlcost" runat="server" CssClass="tetxboxcolor" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART V: OTHER COST"></asp:Label>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Items Description:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtitemdesc" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                  
                        <td>
                            <asp:Label ID="Label22" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Unit :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label23" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Currency:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label24" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Other Item Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox7" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label25" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Other Item Cost/pcs ($):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox8" runat="server" CssClass="tetxboxcolor" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>
                   
                        <td colspan="2">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                Width="200px" ForeColor="#2153a5" Text="PART VI: PART UNIT PRICE"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="2">
                            &nbsp;
                            <asp:GridView ID="grdpartunitprice" runat="server" AutoGenerateColumns="False" DataKeyNames="totmatl"
                                CellPadding="4" GridLines="both" CssClass="GridStyle" Height="197px" Width="100%" style="width:450px; overflow:auto" 
                                ForeColor="#333333">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="totmatl" HeaderText="Total material cost/pcs ($)" />
                                    <asp:BoundField DataField="totproc" HeaderText="Total Process Cost/pcs ($)" />
                                    <asp:BoundField DataField="totsubmatl" HeaderText="Total Sub-Mat/T&J Cost/pcs ($)" />
                                    <asp:BoundField DataField="totothritmcost" HeaderText="Total Other Item Cost/pcs ($)" />
                                    <asp:BoundField DataField="grndtotalcost" HeaderText="Grand Total Cost/pcs ($)" />
                                    <asp:BoundField DataField="finalquote" HeaderText="Final Quote Price/pcs ($)" />
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </td>
                    </tr>--%>
                     <tr>
                        <td>
                            <asp:Label ID="Label27" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total material cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotmatl" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" ></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label28" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Process Cost/pcs ($):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotproc" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label29" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Sub-Mat/T&J Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotsub" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label30" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Other Item Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotother" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>
                         </tr>
                     
 <tr>
                        <td>
                            <asp:Label ID="Label31" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Grand Total Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtgrandtot" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>
                       
                        <td>
                            <asp:Label ID="Label32" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Final Quote Price/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfinalquote" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>

                         </tr>

 <tr>
                        <td>
                            <asp:Label ID="Label33" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Profit (%) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprofit" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" ></asp:TextBox>
                        </td>

                   
                        <td>
                            <asp:Label ID="Label34" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Discount (%) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdiscount" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black"></asp:TextBox>
                        </td>


                    </tr>

                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label35" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Final Quote Price/pcs :"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtfinal" runat="server" Width="200px" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor"></asp:TextBox>
                        </td>


                    </tr>

                  
                    
                </table>

                    </div>
                    <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>
                <!-- DataTables Example -->
                <div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-table"></i>Data Table Example</div>
                    <div class="card-body">
                        <div class="table-responsive">
                        </div>
                    </div>
                    <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>
            </div>
            <!-- /.container-fluid -->
            <!-- Sticky Footer -->
            <footer class="sticky-footer">
          <div class="container my-auto">
            <div class="copyright text-center my-auto">
              <span>Copyright © Your Website 2018</span>
            </div>
          </div>
        </footer>
        </div>
        <!-- /.content-wrapper -->
    </div>
    <!-- /#wrapper -->
    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i>
    </a>
    <!-- Logout Modal-->
    <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">
                        Ready to Leave?</h5>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">
                    Select "Logout" below if you are ready to end your current session.</div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" type="button" data-dismiss="modal">
                        Cancel</button>
                    <a class="btn btn-primary" href="login.html">Logout</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>
    <!-- Page level plugin JavaScript-->
    <script src="vendor/chart.js/Chart.min.js"></script>
    <script src="vendor/datatables/jquery.dataTables.js"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.js"></script>
    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin.min.js"></script>
    <!-- Demo scripts for this page-->
    <script src="js/demo/datatables-demo.js"></script>
    <script src="js/demo/chart-area-demo.js"></script>
    </form>
</body>
</html>
