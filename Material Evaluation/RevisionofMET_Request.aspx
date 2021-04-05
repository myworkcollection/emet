<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevisionofMET_Request.aspx.cs" Inherits="Material_Evaluation.RevisionofMET_Request" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        
         .headewidth
        {
            width:250px;
        }
         .headecellwidth
        {
            width:100px;
        }
        
    </style>


  </head>

  <body id="page-top">

      <form id="form1" runat="server">

    <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
      <a class="navbar-brand mr-1" href="Home.aspx"><asp:Image ID="Image1" 
          runat="server" Height="31px" 
          ImageUrl="~/images/logo.gif" Width="179px" /></a>
      <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
     
        <i class="fas fa-bars"></i>
      </button>
       <asp:Image ID="Image2" 
          runat="server" Height="24px" 
          ImageUrl="~/images/caption1.gif" Width="71px" />
      <!-- Navbar Search -->
        
        <div class="input-group">
         
          <div class="input-group-append">
        
          </div>
        </div>
      <!-- Navbar -->
      <ul class="navbar-nav ml-auto ml-md-0">
        <li class="nav-item dropdown no-arrow mx-1">
          <a class="nav-link dropdown-toggle" href="#" id="alertsDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            </a><div class="dropdown-menu dropdown-menu-right" aria-labelledby="alertsDropdown">
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
        <li class="nav-item active">
          <a class="nav-link" href="Home.aspx">
            <i class="fas fa-fw fa-tachometer-alt"></i>
            <span>Home</span>
          </a>
        </li>
       <%-- <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>Create New MET Request</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
         
            <a class="dropdown-item" href="NewRequest.aspx">First Article Item</a>
            <a class="dropdown-item" href="ChangeofVendor.aspx">Change of Vendor</a>
            <a class="dropdown-item" href="WithSApCode.aspx">Draft & Cost Planning</a>
           
          </div>
        </li>--%>

        <li class="nav-item">
          <a class="nav-link" href="NewRequest.aspx">
            <i class="fas fa-fw fa-table" ></i>
           <span > New Request</span></a>
			
			
        </li>

         <li class="nav-item">
          <a class="nav-link" href="RevisionofMET.aspx">
            <i class="fas fa-fw fa-table" ></i>
           <span > Revision of MET</span></a>
			
			
        </li>


        <%-- <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="A1" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>Revision of MET</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
          
            <a class="dropdown-item" href="RevisionofMET.aspx">Changeable Elements</a>
            <a class="dropdown-item" href="DesignChnge.aspx">Design Change</a>
           
           
          </div>
        </li>--%>

        <li class="nav-item">
          <a class="nav-link" href="#">
            <i class="fas fa-fw fa-chart-area"></i>
            <span>Price Revision</span></a>
        </li>
      
		
		        <li class="nav-item">
          <a class="nav-link" href="#">
            <i class="fas fa-fw fa-table"></i>
            <span>PIR Generation</span></a>
			
			        <li class="nav-item">
          <a class="nav-link" href="#">
            <i class="fas fa-fw fa-table"></i>
            <span>Reports</span></a>
			
			
        </li>
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
                    <li class="breadcrumb-item"><a href="#">Revision of MET</a> </li>
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
                     
                        <table width="100%"   border="0" style="border-collapse: collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; background-color:rgba(0,0,0,.03);">
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label1" runat="server" Text="Vendor Details" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server"  Text="Date of MET Request :"  Font-Bold="True" ForeColor="Black"></asp:Label>
                        </td>
                        <td id="Td1" runat="server">
                            <asp:TextBox ID="txtreqdate" runat="server" Height="30px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lbl_vendrName" runat="server"  Font-Bold="True" ForeColor="Black" Text="Vendor Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvendr" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_autherisedPIC" runat="server"  Font-Bold="True" ForeColor="Black" Text="Authorised PIC :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_vendorpic" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lblcurrency" runat="server"  Font-Bold="True" ForeColor="Black" Text="Autherised Email :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtauterisedEMAIL" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_PIC" runat="server"  Font-Bold="True" ForeColor="Black" Height="27px" Text="Contact No :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtautherscontact" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="Label18" runat="server"  Font-Bold="True" ForeColor="Black" Text="Quote Currency :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcurncy" runat="server" Height="27px"  Font-Bold="True" 
                                ForeColor="Black" BackColor="#E6E6E6" ></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label26" runat="server" Text="Shimano Details" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5"></asp:Label>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label17" runat="server"  Font-Bold="True" ForeColor="Black" Text="SMN PIC :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsmnpic" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    
                        <td>
                            <asp:Label ID="lbl_addres" runat="server"  Font-Bold="True" ForeColor="Black" Text="Email :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtemail" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_cntact" runat="server"  Font-Bold="True" ForeColor="Black" Text="Contact No. :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcontact" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label5" runat="server"  Font-Bold="True" ForeColor="Black" Text="Quote Ref. No. :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtreq" runat="server" Height="27px" 
                                Style="margin-bottom: 0px"  Font-Bold="True" ForeColor="Black"
                                Width="300px" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART I: QUOTED PART INFO"></asp:Label>
                        </td>
                       
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_cntact0" runat="server"  Font-Bold="True" ForeColor="Black" Text="Product :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprod" runat="server" Height="27px" Width="100px" 
                                 Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    
                 
                        <td>
                            <asp:Label ID="lbl_partdesc" runat="server"  Font-Bold="True" ForeColor="Black" Text="Part Code & Desc :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpartdesc" runat="server" Height="41px" 
                                TextMode="MultiLine"  Font-Bold="True" ForeColor="Black"
                                Width="300px" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_cntact4" runat="server"  Font-Bold="True" ForeColor="Black" Text="SAP PIR JOB TYPE & Desc :"
                                Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSAPJobType" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="lbl_PIR" runat="server"  Font-Bold="True" ForeColor="Black" Text="PIR Type & Desc :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPIRtype" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_partDRG" runat="server"  Font-Bold="True" ForeColor="Black" Text="PART DRG :" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdrawng" runat="server" Height="27px" Width="300px"  
                                Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="lbl_proces" runat="server"  Font-Bold="True" ForeColor="Black" Text="Proces Group :"
                                Width="150px"></asp:Label></td>
                        <td>
                             <asp:TextBox ID="txtprocs" runat="server" Height="27px" Width="300px"  
                                 Font-Bold="True" ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox> </td>
                    </tr>
                   
                    <tr>
                    <td colspan="4">
                     <asp:Label ID="lblmatlcost" runat="server" Font-Bold="True" Font-Names="calibri"
                                Font-Size="16px" ForeColor="#2153a5" Text="PART II: Material Cost"></asp:Label>
                    </td>
                    </tr>
                   
                  
                    <tr>
                        <td colspan="4">
                          
                            <%--<div style="height: 200px; width: 520px; overflow:scroll; overflow-y: hidden;" >--%>
                                  <asp:GridView ID="grdmatlcost" runat="server" BorderColor="Black" Width="350px"
                                     
                                    CellPadding="4" CssClass="GridStyle" ForeColor="#333333" GridLines="Both" 
                                    Height="50px" PageSize="1">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                       <%-- <asp:BoundField HeaderText="S.No" />
                                         <asp:BoundField DataField="materialcode" HeaderText="Material Type" />
                                        <asp:BoundField DataField="materialcode" HeaderText="Material SAP Code" />
                                        <asp:BoundField DataField="matlgrade" HeaderText="Material Description" />
                                        <asp:BoundField DataField="rawmatlcost" HeaderText="Raw material Cost(Kg)" />
                                        <asp:BoundField DataField="totrawcost" HeaderText="Total Raw material Cost" />
                                        <asp:BoundField DataField="matlgrsweight" HeaderText="Material Gross Weight/pc (g)" />
                                        <asp:BoundField DataField="matlcost" HeaderText="Material Cost/pcs ($)" />
                                        <asp:BoundField DataField="totmatlcost" HeaderText="Total Material Cost/pcs ($)" />
                                        <asp:BoundField DataField="runrweight" HeaderText="Runner Weight/Shot (g)" />
                                        <asp:BoundField DataField="runrratio" HeaderText="Runner Ratio/pcs (%)" />
                                        <asp:BoundField DataField="recyclematlratio" HeaderText="Recycle Matl Ratio (%)" />
                                        <asp:BoundField DataField="scrapweight" HeaderTex="Material Scrap Weight (g)" />
                                        <asp:BoundField DataField="scraplosallow" HeaderText="Scrap Loss Allowance (%)" />
                                        <asp:BoundField DataField="scrappric" HeaderText="Scrap Price/kg ($)" />
                                        <asp:BoundField DataField="scraprebate" HeaderText="Recycle Material Ratio (%)" />--%>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="black" 
                                        Height="30px" Wrap="True" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="right" Width="10px" />
                                    <RowStyle BackColor="#F7F6F3" CssClass="RowStyle" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" >
                            <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART III: PROCESS COST"></asp:Label>
                        </td>
                       
                        
                    </tr>
                 
                   <tr>
                        <td colspan="4">
                                       <asp:GridView ID="grdproces" runat="server"  AutoGenerateColumns="True"
                                CellPadding="4" CssClass="header"  ForeColor="#333333" Height="157px"   
                                Width="350px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                              <%--  <Columns>
                                    <asp:BoundField DataField="processgrpdescription" HeaderText="Process Group code & Desc" />
                                    <asp:BoundField DataField="SubProcessName" HeaderText="Select Sub Process" />
                                    <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process Unit of Messure(UOM)" />
                                    <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                    <asp:BoundField DataField="HourlyRate" HeaderText="Rate/HR" />
                                    <asp:BoundField HeaderText="Process UOM" />
                                    <asp:BoundField HeaderText="Process Cost/pcs ($)" />
                                    <asp:BoundField HeaderText="Total Process Cost/Pcs" />
                                </Columns>--%>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                               <%-- <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="right"  />--%>
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
                        <td>
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                    <td colspan="4">
                            <asp:GridView ID="grdsubmatl" runat="server"  AutoGenerateColumns="true"
                                CellPadding="4" CssClass="GridStyle"  ForeColor="#333333"
                                Height="157px"   
                                Width="350px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                               <%-- <Columns>
                                    <asp:BoundField DataField="submatlcostdesc" HeaderText="Sub-Mat/T&J Descrption" />
                                    <asp:BoundField DataField="submatlcost" HeaderText="Sub-Mat/T&J Cost" />
                                    <asp:BoundField DataField="consumption" HeaderText="Consumption(pcs)" />
                                    <asp:BoundField DataField="Sub-Mat/T&JCost/pcs" HeaderText="Sub-Mat/T&J Cost/pcs" />
                                    <asp:BoundField DataField="Totsub" HeaderText="Total Sub-Mat/T&J Cost/pcs" />
                                    
                                </Columns>--%>

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
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                ForeColor="#2153a5" Text="PART V: OTHER COST"></asp:Label>
                        </td>
                       
                    </tr>

                      <tr>
                    <td colspan="4">
                                 <asp:GridView ID="grdotheritem" runat="server"  AutoGenerateColumns="true"
                                CellPadding="4" CssClass="GridStyle"  ForeColor="#333333"
                                Height="157px"    
                                Width="350px">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                               <%-- <Columns>
                                    <asp:BoundField DataField="otheritem" HeaderText="Item Description" />
                                    <asp:BoundField DataField="unit" HeaderText="Unit" />
                                    <asp:BoundField DataField="otheritemcost" HeaderText="Other Item Cost/pcs" />
                                    <asp:BoundField DataField="Totalotheritem" HeaderText="Total Other Item Cost/pcs" />
                                   
                                    
                                </Columns>--%>
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
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                Width="200px" ForeColor="#2153a5" Text="PART VI: PART UNIT PRICE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                                 <asp:GridView ID="grdpartunitprice" runat="server" AutoGenerateColumns="true" 
                                CellPadding="4" GridLines="both" CssClass="GridStyle" Height="197px" Width="350px"  
                                ForeColor="#333333">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                               <%-- <Columns>
                                    <asp:BoundField DataField="totmatl" HeaderText="Total material cost/pcs ($)" />
                                    <asp:BoundField DataField="totproc" HeaderText="Total Process Cost/pcs ($)" />
                                    <asp:BoundField DataField="totsubmatl" HeaderText="Total Sub-Mat/T&J Cost/pcs ($)" />
                                    <asp:BoundField DataField="totothritmcost" HeaderText="Total Other Item Cost/pcs ($)" />
                                    <asp:BoundField DataField="grndtotalcost" HeaderText="Grand Total Cost/pcs ($)" />
                                     <asp:BoundField DataField="profit" HeaderText="profit (%)" />
                                      <asp:BoundField DataField="discount" HeaderText="Discount (%)" />
                                    <asp:BoundField DataField="finalquote" HeaderText="Final Quote Price/pcs ($)" />
                                </Columns>--%>
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
                    </tr>
                  <%--   <tr>
                        <td>
                            <asp:Label ID="Label27" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total material cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotmatl" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px" ></asp:TextBox>
                        </td>
                   
                        <td>
                            <asp:Label ID="Label28" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Process Cost/pcs ($):"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotproc" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:Label ID="Label29" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Sub-Mat/T&J Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotsub" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                  

                        <td>
                            <asp:Label ID="Label30" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Total Other Item Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttotother" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                         </tr>
                     
 <tr>
                        <td>
                            <asp:Label ID="Label31" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Grand Total Cost/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtgrandtot" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                        
                        <td>
                            <asp:Label ID="Label32" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Final Quote Price/pcs ($) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfinalquote" runat="server" Width="200px" 
                                Font-Size="X-Small" Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" 
                                Height="27px"></asp:TextBox>
                        </td>

                         </tr>

 <tr>
                        <td>
                            <asp:Label ID="Label33" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Profit (%) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtprofit" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" Height="27px" ></asp:TextBox>
                        </td>

                        
                        <td>
                            <asp:Label ID="Label34" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Discount (%) :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdiscount" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" Height="27px"></asp:TextBox>
                        </td>


                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label35" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Final Quote Price/pcs :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfinal" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td></td>

                    </tr>--%>

                     <tr>
                        <td colspan="4">
                          <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="14px"
                                Width="200px" Text="Quote Validity"></asp:Label>

                          
                        </td>
                        </tr>
                        <tr>

                        <td>
                         <asp:Label ID="Label12" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Effective "></asp:Label>
                        </td>
                        <td>
                         <asp:TextBox ID="TextBox1" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                        <td>
                        <asp:Label ID="Label13" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Black" Text="Due on "></asp:Label>
                            
                        </td>
                        <td>
                        <asp:TextBox ID="txtfinal" runat="server" Width="200px" Font-Size="X-Small" 
                                Font-Bold="True" ForeColor="Black" CssClass="tetxboxcolor" Height="27px"></asp:TextBox>
                        </td>
                        <td></td>

                    </tr>


                     <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="Button1" runat="server" Text="Submit" CssClass="Login-button" />
                    </td>
                    </tr>

                  
                    
                </table>

                    </div>
                   <%-- <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>--%>
                <!-- DataTables Example -->
               <%-- <div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-table"></i>Data Table Example</div>
                    <div class="card-body">
                        <div class="table-responsive">
                        </div>
                    </div>
                    <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>--%>
            </div>
            <!-- /.container-fluid -->
            <!-- Sticky Footer -->
           <%-- <footer class="sticky-footer">
          <div class="container my-auto">
            <div class="copyright text-center my-auto">
              <span>Copyright © Your Website 2018</span>
            </div>
          </div>
        </footer>--%>
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
