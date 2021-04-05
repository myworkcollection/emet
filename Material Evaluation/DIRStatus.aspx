<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DIRStatus.aspx.cs" Inherits="Material_Evaluation.DIRStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>eMET</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <!-- Bootstrap core CSS-->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Page level plugin CSS-->
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />
    <link href="Styles/NewStyle/Style.css" rel="stylesheet" />
    <%--<style type="text/css">
        .tetxboxcolor {
            background-color: #CAC8BF;
        }

        .GridStyle {
            font-family: calibri;
            font-size: 14px;
            overflow: auto;
            width: 100%;
        }


        .Login-button {
            border-radius: 5px;
            border: 1px solid #032742;
            background-color: #005496;
            color: #FFFFFF;
            font-family: Verdana;
            font-size: 16px;
        }


        .HeaderStyle1 {
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: Black;
        }

        .GridStyle, .GridStyle th, .GridStyle td {
            border: 1px solid #010a19;
        }


        .GridPosition {
            position: absolute;
            left: 100px;
            height: 200px;
            width: 200px;
        }

        .tdpossition {
            position: absolute;
            left: 250px;
            height: 200px;
            width: 200px;
            font-family: Calibri;
            color: #7da1db;
        }
    </style>
    <style type="text/css">
        label {
            display: inline-block;
            float: left;
            clear: left;
            width: 250px;
            text-align: right;
            padding-right: 5px;
        }

        input[type=text], select, textarea {
            display: inline-block;
            float: left;
            border: 1px solid #ccc;
            border-radius: 3px;
            box-sizing: border-box;
            border-color: #0B243B;
            padding: 3px 5px;
        }

        select {
            display: inline-block;
            float: left;
        }

        td {
            padding-bottom: 15px !important;
        }

        table {
            font-family: Verdana;
            font-weight: 600;
            font-size: 12px;
        }
    </style>--%>

  <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        //$(document).ready(function () {

        //    var CellsCount = $("#Table1").find('tr')[0].cells.length;
        //    alert(CellsCount);
        //    for (var i = 0; i < CellsCount - 1; i++) {

        //        $(document).on('click', '#lnkApp' + (i), function () {
        //            alert(i);
        //        });
        //    }
        //});

         function ValidateAll() {
             var count = 0;
             var rowspancount = 0;
             $('.dummyClass').each(function (index, item) {
                 count++;
                 var rr = $(this).parent().siblings(":first").attr('rowspan');
                 rowspancount = rr;

                 if (count <= rr) {
                     if ($(this).val() != "") {
                         count = 1;
                     }
                     else {
                         alert("fill all the Reasons");
                         $("#hdnreason").val("1");

                         return false;
                     }
                 }
              
             });
             
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
          <ol class="breadcrumb">
            <li >
               <asp:Label ID="lblUser"  runat="server" Width="147px"></asp:Label>
             <br/>
                <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
              <a  href="login.aspx">Logout</a>
            </li>
          </ol>
       <%-- <li class="nav-item dropdown no-arrow">
          <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-user-circle fa-fw"></i>
          </a>
          <div class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
            <a class="dropdown-item" href="#">Settings</a>
            <a class="dropdown-item" href="#">Activity Log</a>
            <div class="dropdown-divider"></div>
            <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">Logout</a>
          </div>
        </li>--%>
      </ul>
    </nav>
        <div id="wrapper">
            <!-- Sidebar -->
            <ul class="sidebar navbar-nav">
                <li class="nav-item active"><a class="nav-link" href="Home.aspx"><i class="fas fa-fw fa-tachometer-alt"></i><span>Home</span> </a></li>
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
                <li class="nav-item"><a class="nav-link" href="NewRequest.aspx"><i class="fas fa-fw fa-table"></i><span>New Request</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="RevisionofMET.aspx"><i class="fas fa-fw fa-table"></i><span>Revision of MET</span></a> </li>
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
                <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-chart-area"></i><span>Mass Revision</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                    <span>PIR Generation</span></a>
                    <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                        <span>Reports</span></a> </li>
                </li>
            </ul>
            <div id="header">
                <asp:ScriptManager ID="scriptmanager1" runat="server">
                </asp:ScriptManager>
            </div>
            <div id="content-wrapper" style="width: 100%; overflow: auto">
                <div class="container-fluid" style="width: 100%; overflow: auto">
                    <!-- Breadcrumbs-->
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item">
                            <%--<a href="#">First Article Item</a>--%>
                        </li>
                    </ol>
                    <!-- Icon Cards-->
                    <div class="row" style="width: 100%; overflow: auto">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3" style="width: 100%; overflow: auto">
                        <div class="card-header" style="width: 100%; overflow: auto">
                            <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION STATUS
                        </div>
                        <div class="card-body" style="width: 100%; overflow: auto">
                            <div class="row" style="padding-bottom:10px;">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="lblRequest_view" runat="server" Font-Bold="true" Text="DIRECTOR APPROVAL "></asp:Label>
                                        </div>
                                        <div class="col-md-2" style="text-align:right">
                                            <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="Login-button" PostBackUrl="Home.aspx" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table table-responsive table-sm">
                                        <asp:UpdatePanel ID="UpdatePanel1"  runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional"  RenderMode="Block">
                                            <ContentTemplate>
                                              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" CssClass="table-bordered table-sm">
                                                    <Columns>
                                                        <asp:BoundField DataField="Plant" HeaderText="Plant" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequestNumber" HeaderText="RequestNumber" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="RequestDate" HeaderText="RequestDate" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="QuoteResponseDueDate" HeaderText="QuoteResponseDueDate" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Product" HeaderText="Product" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Material" HeaderText="Material" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MaterialDesc" HeaderText="MaterialDesc" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VendorCode1" HeaderText="VendorCode1" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="VendorName" HeaderText="VendorName" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="QuoteNo" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton Text='<%# Eval("QuoteNo") %>' ItemStyle-Width="150px" runat="server" CommandName="LinktoRedirect" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="TotalMaterialCost" HeaderText="TotalMaterialCost" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TotalProcessCost" HeaderText="TotalProcessCost" ItemStyle-Width="150px" >

                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="TotalSubMaterialCost" HeaderText="TotalSubMaterialCost" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TotalOtheritemsCost" HeaderText="TotalOtheritemsCost" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GrandTotalCost" HeaderText="GrandTotalCost" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Profit" HeaderText="Profit %" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Discount" HeaderText="Discount %" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FinalQuotePrice" HeaderText="FinalQuotePrice" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ApprovalStatus" HeaderText="Response Status" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="PICApprovalStatus" HeaderText="PIC Status" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="PICReason" HeaderText="PICReason" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="ManagerApprovalStatus" HeaderText="Manager Status" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                         <asp:BoundField DataField="ManagerReason" HeaderText="Manager Reason" ItemStyle-Width="150px" >
                                                        <ItemStyle Width="150px" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Director Approval" >
                                                            <ItemTemplate>
                                                                <asp:Button Text="Approve" runat="server"  CommandName="Approve" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false">
                                                            <ItemTemplate>
                                                                <asp:Button Text="Reject" runat="server" CommandName="Reject" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Reason" ItemStyle-Width="150">
                                                            <ItemTemplate>
                                                                <asp:TextBox AutoCompleteType="None" autocomplete="off" CssClass="dummyClass"  ID="txtReason" runat="server" Text='<%# Eval("Reason") %>' />
                                                                
                                                            </ItemTemplate>
                                                            <ItemStyle Width="150px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnclose" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:HiddenField ID="hdnreason" runat="server" Value="" />
                                </div>
                            </div>

                            <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->
                            <%--<table width="100%">
                                <tr>
                                    <td colspan="2">
                                        
                                    &nbsp;
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div  style="width: 100%; overflow: auto;height: 500px;">
                                        
                                            </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        
                                        &nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>--%>
                        </div>
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
                    </div>
                    <!-- DataTables Example -->
                    <%-- <td colspan="4"  >
                     <asp:Label ID="Label2" runat="server" Text="If Process Group=SF Then" Font-Bold="True"
                                 ForeColor="Black"></asp:Label>
                        
                        </td>
                        </tr>
                        <tr>

                        <td>                       
                       
                            <asp:Label ID="Label8" runat="server" Text="Part Surface area:" Font-Bold="True"
                                 ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpartsurface" runat="server" Height="28px" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Raw Material:" Font-Bold="True"
                                 ForeColor="Black"></asp:Label>
                        </td>
                        <td>
                         <asp:TextBox ID="txtProcSF" runat="server" Height="30px" Width="161px"></asp:TextBox>
                        </td>
                       

                        </tr>--%>
                </div>
                <!-- /.container-fluid -->
                <!-- Sticky Footer -->
                <%--<asp:TemplateField HeaderText="Select All">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkheader" runat="server" AutoPostBack="true"  OnCheckedChanged="chkheader_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkchild" runat="server" AutoPostBack="true" onclick = "Check_Click(this);"    />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
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
                        Ready to Leave?ent session.
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" type="button" data-dismiss="modal">
                            Cancel</button>
                        <a class="btn btn-primary" href="login.html">Logout</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Bootstrap core JavaScript-->

    </form>
</body>
</html>
