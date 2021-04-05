<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevisionofMET.aspx.cs"
    Inherits="Material_Evaluation.RevisionofMET" %>

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
    </style>

    <script type="text/javascript">
        checked = false;
        function checkedAll(frm1) {
            var aa = document.getElementById('frm1');
            if (checked == false) {
                checked = true
            }
            else {
                checked = false
            }
            for (var i = 0; i < aa.elements.length; i++) {
                if (aa.elements[i].type == 'checkbox') {
                    aa.elements[i].checked = checked;
                }
            }
        }
</script>

   
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
                    <li class="breadcrumb-item"><%--<a href="#">Revision of MET</a> --%></li>

                         <table width="100%"  border="0" style="border-collapse: collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; background-color:rgba(0,0,0,.03);" >
                        <tr>
                        <td>
                        
                            <asp:RadioButton ID="article" runat="server" Text="Changeable Elements" 
                                GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true" oncheckedchanged="article_CheckedChanged" 
                                  />
                        
                        </td>
                        <td>
                        <asp:RadioButton ID="changevendr" runat="server" Text="Design Changes" 
                                GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true" oncheckedchanged="changevendr_CheckedChanged" 
                                 />
                        </td>
                       
                        </tr>
                       
                        </table>

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
                      <%--  <table class="w-100">--%>

                        <div class="container-fluid">
                <!-- Breadcrumbs-->
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><%--<a href="#">Change of Vendor</a>--%> </li>



                          <table width="100%"   border="0" style=" mso-table-lspace:0pt; mso-table-rspace:0pt; background-color:rgba(0,0,0,.03);">
                    <tr>
                    
                        <td colspan="4"  align="right">
                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Bold="True"  AutoPostBack="true"
                                ForeColor="Black" RepeatDirection="Horizontal" OnSelectedIndexChanged = "OnRadio_Changed">
                                <asp:ListItem>by Vendor</asp:ListItem>
                                <asp:ListItem>by Product</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                   
                    </table>

                     </ol>
                <!-- Icon Cards-->
                <div class="row">
                </div>
                        <table width="100%"   border="0" style="border-collapse: collapse; mso-table-lspace:0pt; mso-table-rspace:0pt; background-color:rgba(0,0,0,.03);">
                           
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lbl_date" runat="server"  Text="Date :" Width="150px"
                                        Font-Bold="True" ForeColor="Black" Style="margin-bottom: 0px"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txt_date" runat="server" Width="250px" Font-Bold="True" 
                                        ForeColor="Black" Height="30px"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_cntact0" runat="server"  Height="16px" Text="Plant"
                                        Width="62px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left">
                                    <%-- <asp:DropDownList ID="ddlplant" runat="server" AutoPostBack="true" Height="30px"
                                        OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" Width="300px">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="txtplant" runat="server"  Font-Bold="True" ForeColor="Black"
                                        Width="100px" Height="30px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Vendor Name:" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtvendrID" runat="server" Height="30px" TextMode="MultiLine" Font-Bold="True"
                                                 ForeColor="Black" Width="250px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="1"
                                                CompletionListElementID="pnlPrdCode" EnableCaching="true" FirstRowSelected="true"
                                                MinimumPrefixLength="1" ServiceMethod="getvendorid" TargetControlID="txtvendrID"
                                                UseContextKey="True">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtvendrID" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label6" runat="server" Text="Product :" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlproduct" runat="server" Font-Bold="True" 
                                        Width="250px" ForeColor="Black" Height="30px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label2" runat="server" Text="Vendor ID:" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtvendor" runat="server" Height="31px" Width="250px"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblmatldesc" runat="server" Text="Material Class Description" Font-Bold="True"
                                         ForeColor="Black" Width="180px"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlmatlclass" runat="server" AutoPostBack="true" Height="28px"
                                                Width="250px">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlmatlclass" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%-- <tr>
                            <td >
                                <asp:Label ID="lbl_cntact0" runat="server"  Height="16px" 
                                    Text="Plant" Width="62px" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlplant" runat="server" AutoPostBack="true" 
                                    Height="30px" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" 
                                    Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                            <tr>
                                <td >
                                    <asp:Label ID="Label3" runat="server" Text="SAP Part Code & Desc :" Font-Bold="True"
                                         ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtpartdesc" runat="server" AutoPostBack="true" Height="29px" TextMode="MultiLine"
                                                Width="250px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="acMaterial" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode"
                                                EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getSapPart"
                                                TargetControlID="txtpartdesc" UseContextKey="True">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtpartdesc" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td >
                                    <asp:Label ID="Label4" runat="server" Text="Plant Status :" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtplantstatus" runat="server" Height="23px" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="lbldrawng" runat="server"  Text="Drawing No :"
                                        Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                                <td  colspan="2">
                                    &nbsp;<table >
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="FileUpload2" runat="server" AllowMultiple="true" Height="30px"  Width="200px"  />

                                               
                                                <asp:Button ID="btnUpload" runat="server" accept="image/gif, image/jpeg" Text="Upload"
                                                    Width="100px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridView1" runat="server" AllowMultiple="true" AllowPaging="True"
                                                AutoGenerateColumns="False" CellPadding="3" Height="10px" Width="500px" BackColor="White"
                                                BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" GridLines="Horizontal">
                                                <AlternatingRowStyle BackColor="#F7F7F7" />
                                                <Columns>
                                                    <%--  <asp:TemplateField HeaderText="File Name">
                                    <EditItemTemplate>
                                       
                                        <asp:TextBox ID="txt_Name" runat="server"  Text='<%# Eval("drawingNo") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Image">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server"  ImageUrl='<%# Eval("Images") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Image ID="Image_user" HeaderText="Upload Image" runat="server" ImageUrl='<%# Eval("Images") %>'>
                                        </asp:Image>
                                        <br />
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>--%>
                                                    <asp:BoundField DataField="drawingNo" HeaderText="Drawing Number" />
                                                    <asp:BoundField DataField="images" HeaderText="Images" />
                                                    <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LkB1" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                                                            <asp:LinkButton ID="LkB11" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LkB2" runat="server" CommandName="Update">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="LkB3" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="150px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" Height="30px" />
                                                <PagerStyle ForeColor="#4A3C8C" HorizontalAlign="Right" BackColor="#E7E7FF" />
                                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                                <SortedAscendingCellStyle BackColor="#F4F4FD" />
                                                <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                                                <SortedDescendingCellStyle BackColor="#D8D8F0" />
                                                <SortedDescendingHeaderStyle BackColor="#3E3277" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="GridView1" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_cntact2" runat="server"  Font-Bold="True" ForeColor="Black"
                                        Text="Process Group & Desc :" Width="150px"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtprocdesc" runat="server" AutoPostBack="true" Height="30px" Width="250px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="1"
                                                CompletionListElementID="pnlPrdCode" EnableCaching="true" FirstRowSelected="true"
                                                MinimumPrefixLength="1" ServiceMethod="getProcess" TargetControlID="txtprocdesc"
                                                UseContextKey="True">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtprocdesc" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbljobtypedesc" runat="server" Text="SAP PIR Job Type:" Font-Bold="True"
                                         ForeColor="Black"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddljobtypedesc" runat="server"  Font-Bold="True"
                                         ForeColor="Black" Height="30px" Width="150px">
                                    </asp:DropDownList>

                                  
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label8" runat="server" Text="Part Surface area:" Font-Bold="True"
                                         ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtpartsurface" runat="server" Height="30px" Width="300px"></asp:TextBox>
                                </td>
                                <td >
                                <asp:Label ID="Label12" runat="server" Text="Job Type Desc:" Font-Bold="True"
                                         ForeColor="Black"></asp:Label>
                                </td>
                                <td >

                                    <asp:DropDownList ID="DropDownList1" runat="server" Font-Bold="True"
                                         ForeColor="Black" Height="30px" Width="203px">
                                    </asp:DropDownList>

                                  

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_existing" runat="server" Font-Bold="True" 
                                        ForeColor="Black" Text="Quotation No :"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlExistingcode" runat="server" Font-Bold="True" AutoPostBack="true"
                                         ForeColor="Black" Height="30px" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Valid Till:"  Font-Bold="True"
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"  Font-Bold="True" ForeColor="Black" 
                                        Height="30px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="lbl_vendrName" runat="server"  Text="Changeable Elements :"
                                        Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="allCheck_new"  runat="server" Text="Select all" />

                                                
                                                <asp:CheckBoxList ID="allchkbox" runat="server" AutoPostBack="true"  >
                                                    
                                                    <asp:ListItem Value="1">MaterialCost</asp:ListItem>
                                                    <asp:ListItem Value="2">ProcessCost</asp:ListItem>
                                                    <asp:ListItem Value="3">SubMaterial</asp:ListItem>
                                                    <asp:ListItem Value="4">Other</asp:ListItem>
                                                    
                                                </asp:CheckBoxList>
                                         
                                </td>
                                <td>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label7" runat="server"  Text="SAP Proc Type :"
                                        Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtproctype" runat="server" Width="300px" Height="30px"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="Label9" runat="server" Text="PIR Type:" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <table >
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPIRType" runat="server" Height="30px" Width="80px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPIRDesc" runat="server" Height="30px" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="Label10" runat="server"  Text="SAP Sp Proc Type :"
                                        Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtSAPspProctype" runat="server" Width="300px" Height="30px"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="Label11" runat="server" Text="Net Weight:" Font-Bold="True" 
                                        ForeColor="Black"></asp:Label>
                                </td>
                                <td >
                                    <table >
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtunitweight" runat="server" Height="30px" Width="150px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUOM" runat="server" Height="30px" Width="140px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    &nbsp;
                                </td>
                                <td >
                                    <table >
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnsave" runat="server" CssClass="Login-button" Text="Create Request"
                                                    Width="150px" PostBackUrl="RevisionofMET_Request.aspx"
                                                    />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnsubmit" runat="server" CssClass="Login-button" PostBackUrl="vendor_Quotation.aspx"
                                                    Text="Submit" Visible="false" Width="100px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%-- <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>--%>
                    <!-- DataTables Example -->
                    <%--<div class="card mb-3">
                    <div class="card-header">
                        <i class="fas fa-table"></i>Data Table Example</div>
                    <div class="card-body">
                        <div class="table-responsive">
                        </div>
                    </div>
                    <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>
                </div>
            </div>--%>
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
