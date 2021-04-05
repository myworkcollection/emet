<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DateUpdate.aspx.cs" Inherits="Material_Evaluation.DateUpdate" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        .ui-datepicker-trigger {
            margin-top: 5px;
        }

        input[type=text] {
            margin-right: 5px;
            position: relative;
            top: 0px
        }

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
            border: 1px solid #0B243B;
            border-radius: 3px;
            box-sizing: border-box;
            padding: 3px 5px;
            left: 0px;
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

        $(function () {
            $("[id*=txtDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy",
                minDate: 0,
                buttonImage: 'images/calendar.png'
            });
        });
     
      

        function preventInput(evnt) {

            if (evnt.which != 9) evnt.preventDefault();

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

		  <ol class="breadcrumb">
            <li >
              <asp:Label ID="lbluser1"  runat="server" Width="147px"></asp:Label>
             <br/>
                <asp:Label ID="lblplant"  runat="server" Text=""></asp:Label>
              <a  href="login.aspx">Logout</a>
            </li>
          </ol>
      </ul>
    </nav>



        <div id="wrapper">

            <!-- Sidebar -->
      <ul class="sidebar navbar-nav">
        <li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=1">
            <i class="fas fa-fw fa-tachometer-alt"></i>
            <span>Home</span>
          </a>
        </li>
         

        <li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=2" >
            <i class="fas fa-fw fa-tachometer-alt"></i>
                <span>Create Request</span></a>
        </li>

         <li class="nav-item active">
          <a class="nav-link" href="Revision.aspx">
            <i class="fas fa-fw fa-table" ></i>
           <span > Revision of MET</span></a>
			
        </li>

        <li class="nav-item active">
          <a class="nav-link" href="MassRevision.aspx">
            <i class="fas fa-fw fa-chart-area"></i>
            <span>Mass Revision</span></a>
        </li>
      
		 <li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=16">
            <i class="fas fa-fw fa-table"></i>
            <span>PIR Generation</span></a>

        </li>

          	<li class="nav-item active">
          <a class="nav-link" href="Emet_author.aspx?num=2">
            <i class="fas fa-fw fa-table"></i>
            <span>Reports</span></a>

        </li>
      </ul>
            <div id="header">
                <asp:ScriptManager ID="scriptmanager1" runat="server">
                </asp:ScriptManager>
            </div>
            <div id="content-wrapper">
                <div class="container-fluid">

                    <%--radio button choice--%>
                    <div class="row ">
                        <div class="col-md-12">
                        </div>
                    </div>


                    <!-- Icon Cards-->
                    <div class="row">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3">
                        <div class="card-body">
                            <%--button reset--%>
                            <div class="col-md-12" style="background-color:rgba(0,0,0,.03);padding-top:10px; padding-bottom:10px;">
                                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="Login-button" PostBackUrl="Request_Waiting.aspx" />
                                    <asp:Label ID="lbluser" runat="server" Text="Label" Visible="false"></asp:Label>
		                                <br />
                                        <asp:Label ID="LblReq" runat="server" Text="Requestnumber: "></asp:Label>
                                        <asp:Label ID="LblReq0" runat="server"></asp:Label>
		                                <br />
                                                <asp:TextBox ID="txtReqDate" runat="server" AutoCompleteType="Disabled" autocomplete="off"
                                            onkeydown="javascript:preventInput(event);" Width="208px" Visible="False" ></asp:TextBox>
		                    </div>


                            <%--entrydata--%>
                            <div class="col-md-12" style="background-color:rgba(0,0,0,.03);">

                                <div class="row" style="padding-bottom:10px;">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-5">
                                                <asp:Label ID="lbljobtypedesc0" runat="server" Text="Quotation response due date" 
                                                     ForeColor="Black"></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtDate" runat="server" AutoCompleteType="Disabled"  Width="90%"
                                                    autocomplete="off" onkeydown="javascript:preventInput(event);"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="padding-bottom:10px">
                                                            <asp:GridView ID="grdvendor" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                GridLines="both" ForeColor="#333333" >
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#1a2e4c" />
                                                                <Columns>
                                                                    <%--<asp:TemplateField HeaderText="Select All">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkheader" runat="server" AutoPostBack="true"  OnCheckedChanged="chkheader_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkchild" runat="server" AutoPostBack="true" onclick = "Check_Click(this);"    />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>



                                                                    <asp:BoundField DataField="VendorCode1" HeaderText="Vendor ID" />
                                                                    <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                                    <asp:BoundField DataField="QuoteNo"  HeaderText="Quote No" />

                                                                </Columns>
                                                                <EditRowStyle BackColor="#999999" />
                                                                <FooterStyle BackColor="#1a2e4c" ForeColor="White"  />
                                                                <HeaderStyle BackColor="#e6f2ff"  ForeColor="Black" />
                                                                <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                            </asp:GridView>

                                      
                                </div>

                                 <div class="row" style="padding-bottom:10px">
                                       <asp:Button ID="Button1" runat="server" CssClass="Login-button"
                                            Text="Submit" Width="100px" OnClick="Button1_Click" />
                                     </div>

                                <div id="dialog1" title="Full Image View" style="display: none">
                                <asp:Image ID="img1" runat="server" />
                                </div>
                            </div>
                        </div>
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
                                Ready to Leave?dalLabel">
                        Ready to Leave?    </button>
                            </div>
                            <div class="modal-body">
                                Select "Logout" below if you are ready to end your current session.
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