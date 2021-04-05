<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Eview.aspx.cs" Inherits="Material_Evaluation.Eview" %>


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
    <style type="text/css">
        .ui-datepicker-trigger {
            margin-top: 5px;
        }

        input[type=text] {
            margin-right: 5px;
            position: relative;
            top: 0px;
        }

        .collapsible {
            background-color: #777;
            color: white;
            cursor: pointer;
            padding: 18px;
            width: 100%;
            border: none;
            text-align: left;
            outline: none;
            font-size: 15px;
        }

            .active, .collapsible:hover {
                background-color: #555;
            }

        .content {
            padding: 0 18px;
            display: none;
            overflow: hidden;
            background-color: #f1f1f1;
        }
    </style>
    <style type="text/css">
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
            font-family: Arial;
            height: 23px;
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
        .Grid td {
            background-color: #eee;
            color: black;
            font-family: Verdana;
            font-size: 10pt;
            line-height: 200%;
            cursor: pointer;
            width: 100px;
        }

        .header {
            background-color: #e6f2ff !important;
            color: black !important;
            font-family: Verdana;
            font-size: 10pt;
            line-height: 200%;
            width: 250px;
            text-align: left;
        }

        .headewidth {
            width: 250px;
        }

        .headecellwidth {
            width: 100px;
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
    </style>
    <style type="text/css">
        .classCollapsibleHeader {
            color: white;
            background-color: #719DDB;
            font: bold 11px auto "Trebuchet MS", Verdana;
            font-size: 12px;
            cursor: pointer;
            width: 450px;
            height: 18px;
            padding: 4px;
        }

        .classCollapsibleBody {
            /*       background-color: #DCE4F9;  */
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            width: 450px;
            padding: 4px;
            padding-top: 7px;
            height: 0px;
            overflow: hidden;
        }
    </style>

   <script type="text/javascript">
        function ReCalculate()
        {
            var TotMatCost = parseFloat(document.getElementById('txtTotalMaterialCost/pcs-0').value)
            var TotProCost = parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)
            var TotSubMatCost = parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value)
            var TotOthrCost = parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value)
            var ValueAfterProforDisc = parseFloat(document.getElementById('txtFinalQuotePrice/pcs1').value)
            var txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value)
            var txtDiscount = parseFloat(document.getElementById('txtDiscount(%)0').value)
            var textbox = document.getElementById('txtProfit(%)0');
            var textbox2 = document.getElementById('txtDiscount(%)0');

            var xx = txtProfit.toString().replace('0', '')

            

            document.getElementById("txtFinalQuotePrice/pcs0").value = document.getElementById("txtTotalMaterialCost/pcs-0").value;
            document.getElementById("txtTotalProcessesCost/pcs-0").value = document.getElementById("txtTotalProcessesCost/pcs0").value;
            document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;

            var GtValue1 = (TotMatCost) + (parseFloat(document.getElementById('txtTotalProcessesCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalSub-Mat/T&JCost/pcs-0').value)) + (parseFloat(document.getElementById('txtTotalOtherItemCost/pcs-0').value));
            document.getElementById("txtGrandTotalCost/pcs0").value = GtValue1.toFixed(6);

            if (xx > 0) {
                textbox.disabled = false;
                textbox2.disabled = true;
            }
            else {
                textbox.disabled = true;
                textbox2.disabled = false;
            }

            if (textbox.disabled) {
                //final value when Discount change
                if (txtDiscount.toString().length = 0)
                {
                    txtDiscount = 0;
                }
                var txtDiscount = parseFloat(document.getElementById('txtDiscount(%)0').value)
                var WithDisc = TotProCost + (TotProCost * (txtDiscount / 100));
                document.getElementById("txtFinalQuotePrice/pcs1").value = WithDisc.toFixed(6);
            }
            else {
                //final value whit profit
                if (txtProfit.toString().length = 0) {
                    txtProfit = 0;
                }
                var txtProfit = parseFloat(document.getElementById('txtProfit(%)0').value)
                var WithProfit = TotProCost + (TotProCost * (txtProfit / 100));
                document.getElementById("txtFinalQuotePrice/pcs1").value = WithProfit.toFixed(6);
            }
            document.getElementById("txtFinalQuotePrice/pcs2").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value;
            document.getElementById("txtFinalQuotePrice/pcs3").value = document.getElementById("txtTotalOtherItemCost/pcs-0").value;


            var a = parseFloat(document.getElementById('txtFinalQuotePrice/pcs0').value)
            var b = parseFloat(document.getElementById('txtFinalQuotePrice/pcs1').value)
            var c = parseFloat(document.getElementById('txtFinalQuotePrice/pcs2').value)
            var d = parseFloat(document.getElementById('txtFinalQuotePrice/pcs3').value)
            var SumValFinal = a + b + c + d
            document.getElementById("txtFinalQuotePrice/pcs4").value = SumValFinal.toFixed(6);

            var GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
            var FinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
            var NetProfDisc = (((FinalValue - GtValue) / FinalValue) * 100);
            document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';

            document.getElementById('txtProfit(%)0').disabled = true
            document.getElementById('txtDiscount(%)0').disabled = true

            $("#hdnTMatCost").val(document.getElementById("txtFinalQuotePrice/pcs0").value);
            $("#hdnTProCost").val(document.getElementById("txtFinalQuotePrice/pcs1").value);
            $("#hdnTSumMatCost").val(document.getElementById("txtFinalQuotePrice/pcs2").value);
            $("#hdnTOtherCost").val(document.getElementById("txtFinalQuotePrice/pcs3").value);
            $("#hdnProfit").val(txtProfit);
            $("#hdnDiscount").val(txtDiscount);
            $("#hdnTFinalQPrice").val(FinalValue);
        }
    </script>
    <script type="text/javascript">
        function RestoreDataTbaleUnit()
        {
            document.getElementById("txtTotalProcessesCost/pcs-0").value = document.getElementById("txtTotalProcessesCost/pcs0").value;
            document.getElementById("txtTotalSub-Mat/T&JCost/pcs-0").value = document.getElementById("txtTotalSub-Mat/T&JCost/pcs0").value;

            document.getElementById("txtProfit(%)0").value = $("#hdnProfit").val();
            document.getElementById("txtDiscount(%)0").value = $("#hdnDiscount").val();
            document.getElementById("txtFinalQuotePrice/pcs0").value = $("#hdnTMatCost").val();
            document.getElementById("txtFinalQuotePrice/pcs1").value = $("#hdnTProCost").val();
            document.getElementById("txtFinalQuotePrice/pcs2").value = $("#hdnTSumMatCost").val();
            document.getElementById("txtFinalQuotePrice/pcs3").value = $("#hdnTOtherCost").val();
            document.getElementById("txtFinalQuotePrice/pcs4").value = $("#hdnTGTotal").val();
            document.getElementById("txtFinalQuotePrice/pcs4").value = $("#hdnTFinalQPrice").val();


            document.getElementById('txtProfit(%)0').disabled = true
            document.getElementById('txtDiscount(%)0').disabled = true

            var GtValue = parseFloat(document.getElementById("txtGrandTotalCost/pcs0").value);
            var FinalValue = parseFloat(document.getElementById("txtFinalQuotePrice/pcs4").value);
            var NetProfDisc = (((FinalValue - GtValue) / FinalValue) * 100);
            document.getElementById("txtNetProfit(%)0").value = NetProfDisc.toFixed(1) + ' %';
        }
    </script>

     <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />

   
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
            <%--<li class="nav-item dropdown no-arrow">
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
                <li class="nav-item"><a class="nav-link" href="Home.aspx"><i class="fas fa-fw fa-tachometer-alt"></i><span>Home</span> </a></li>
                <li class="nav-item"><a class="nav-link" href="NewRequest.aspx"><i class="fas fa-fw fa-table"></i><span>New Request</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="RevisionofMET.aspx"><i class="fas fa-fw fa-table"></i><span>Revision of MET</span></a> </li>

                 <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-chart-area"></i><span>Price Revision</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                    <span>PIR Generation</span></a>
                    <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                        <span>Reports</span></a> </li>
                </li>

               <%--  <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>Create New MET Request</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
         
            <a class="dropdown-item" href="NewRequest.aspx">First Article Item</a>
            <a class="dropdown-item" href="ChangeofVendor.aspx">Change of Vendor</a>
            <a class="dropdown-item" href="WithSApCode.aspx">Draft & Cost Planning</a>
           
          </div>
        </li>--%>               <%-- <li class="nav-item"><a class="nav-link" href="NewRequest.aspx"><i class="fas fa-fw fa-table"></i><span>New Request</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="RevisionofMET.aspx"><i class="fas fa-fw fa-table"></i><span>Revision of MET</span></a> </li>--%>                <%-- <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="A1" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw fa-folder"></i>
            <span>Revision of MET</span>
          </a>
          <div class="dropdown-menu" aria-labelledby="pagesDropdown">
          
            <a class="dropdown-item" href="RevisionofMET.aspx">Changeable Elements</a>
            <a class="dropdown-item" href="DesignChnge.aspx">Design Change</a>
           
           
          </div>
        </li>--%>                <%--<li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-chart-area"></i><span>Price Revision</span></a> </li>
                <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                    <span>PIR Generation</span></a>
                    <li class="nav-item"><a class="nav-link" href="#"><i class="fas fa-fw fa-table"></i>
                        <span>Reports</span></a> </li>
                </li>--%>
            </ul>
            <div id="header">
                            <asp:ScriptManager ID="scriptmanager1" runat="server" EnablePageMethods="true">
            </asp:ScriptManager>
                            <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
                </asp:ToolkitScriptManager>--%>
            </div>
            <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                   
                    <!-- Icon Cards-->
                    <div class="row">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3">
                        <%-- <div class="card-header">
                            <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION
                        </div>--%>
                        <div class="card-body">
                            <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->
                            <%--  <table class="stylebody" >--%>
                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                               <tr>
                                    <td >
                                        <asp:Label ID="lblreqst" runat="server" Text="Quote No:"></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdnQuoteNo" />
                                    </td>

                                       <td>
                                        <asp:Label ID="lblVName" runat="server" Text="Vendor Name:"></asp:Label>
                                        
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrency" runat="server" Text="Currency:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCity" runat="server" Text="City:"></asp:Label></td>
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
                                        <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Black" Text="SMN PIC :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsmnpic" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_addres" runat="server" Font-Bold="True" ForeColor="Black" Text="Email :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtemail" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cntact" runat="server" Font-Bold="True" ForeColor="Black" Text="Contact No. :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcontact" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" Enabled="false" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>

                                        <asp:Label ID="Labelquote" runat="server" Font-Bold="True" ForeColor="Black" Text="Quotation Due Date :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtquotationDueDate" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6" Enabled="false"></asp:TextBox>
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
                                        <asp:Label ID="lbl_cntact0" runat="server" Font-Bold="True" ForeColor="Black" Text="Product :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtprod" Enabled="false" runat="server" Height="27px" Width="100px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_partdesc" runat="server" Font-Bold="True" ForeColor="Black" Text="Part Code & Desc :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpartdesc" Enabled="false" runat="server" Height="41px" TextMode="MultiLine" Font-Bold="True"
                                            ForeColor="Black" Width="300px" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cntact4" runat="server" Font-Bold="True" ForeColor="Black" Text="SAP PIR JOB TYPE & Desc :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSAPJobType" Enabled="false" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_PIR" runat="server" Font-Bold="True" ForeColor="Black" Text="PIR Type & Desc :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPIRtype" Enabled="false" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_partDRG" runat="server" Font-Bold="True" ForeColor="Black" Text="PART DRG :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdrawng" Enabled="false" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_proces" runat="server" Font-Bold="True" ForeColor="Black" Text="Proces Group :"
                                            Width="150px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtprocs" Enabled="false" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                        <asp:HiddenField runat="server" ID="txtPartUnit" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Font-Bold="True" ForeColor="Black" Text="BaseUOM: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBaseUOM" Enabled="false" runat="server" Height="27px" Width="300px" Font-Bold="True"
                                            ForeColor="Black" BackColor="#E6E6E6"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_proces0" runat="server" Font-Bold="True" ForeColor="Black" Text="SMN BOM & Material Cost details"
                                            Width="150px"></asp:Label>
                                          </td>
                                    <td>
                                        <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Width="100%">
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
                                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <RowStyle ForeColor="#000066" />
                                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                                    
                                
                                                </asp:GridView></td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Black" Text="Effective "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" Enabled="false" Width="200px" Font-Bold="True" ForeColor="Black"
                                            Height="27px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Black" Text="Due on "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfinal" runat="server" Enabled="false" Width="200px" Font-Bold="True" ForeColor="Black"
                                            Height="27px"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                               

                                

                            </table>

                            <div>
                                <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">


                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmatlcost" runat="server" Font-Bold="True" Font-Names="calibri"
                                                Font-Size="16px" ForeColor="#2153a5" Text="PART II: Material Cost"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>

                                                    <asp:Table ID="Table1" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                        rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                    </asp:Table>



                                        </td>
                                    </tr>
                                    <tr>
                                        <td>

                                            &nbsp;</td>
                                    </tr>

                                </table>
                            </div>
                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Font-Names="calibri" Font-Size="16px"
                                            ForeColor="#2153a5" Text="PART III: PROCESS COST"></asp:Label>
                                    </td>
                                </tr>
                               

                                <tr>
                                    <td colspan="2">

                                                <asp:Table ID="TablePC" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                    rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                </asp:Table>


                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                            ForeColor="#2153a5" Text="PART IV: SUB-MAT/T&amp;J COST"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td colspan="2">
                                                <asp:Table ID="TableSMC" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                    rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                </asp:Table>


                                    </td>
                                </tr>

                                <tr>
                                    <td>

                                        &nbsp;</td>
                                </tr>

                            </table>

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">

                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                            ForeColor="#2153a5" Text="PART V: OTHER COST"></asp:Label>
                                    </td>
                                </tr>
                               
                                <tr>
                                    <td colspan="2">


                                                <asp:Table ID="TableOthers" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                    rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                </asp:Table>

                                    </td>
                                </tr>
                                <tr>
                                    <td>

                                        &nbsp;</td>
                                </tr>

                            </table>

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                            Width="200px" ForeColor="#2153A5" Text="PART VI: PART UNIT PRICE" Height="24px"></asp:Label>
                                        <asp:Label ID="lblcry" runat="server" Font-Bold="True" Font-Names="calibri" Font-Size="16px"
                                            Width="200px" ForeColor="#2153A5"></asp:Label>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2">
                                                <asp:Table ID="TableUnit" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                    rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                </asp:Table>
                                    </td>
                                </tr>


                            </table>
                            <table>
                            <tr>
                                <td colspan="2" align="center">

                                     
                                                  <asp:Button ID="Button1" runat="server" PostBackUrl="~/Vendor.aspx" Text="Back to Home" CssClass="Login-button"  />

                                    
                                </td>
                            </tr>
</table>
                            <table>
                             <tr>
                                    <td colspan="4">
                                        <asp:Label ID="Label1" runat="server" Text="Vendor Details" Font-Bold="True" Font-Names="calibri"
                                            Font-Size="16px" ForeColor="#2153a5" Visible="false"></asp:Label>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 80%;">
                                        <asp:GridView ID="grdVendrDet" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="REQUESTDATE" HeaderText="Request Date" />
                                                <asp:BoundField DataField="QUOTENO" HeaderText="Quote No" />
                                                <asp:BoundField DataField="Description" HeaderText="Vendor Name" />
                                                <asp:BoundField DataField="Crcy" HeaderText=" Quote Currency" />
                                                <asp:BoundField DataField="PICName" HeaderText="PIC Name" />
                                                <asp:BoundField DataField="PICemail" HeaderText="PIC Email" />
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
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
                                    <td colspan="2" style="width: 80%;">

                                        <asp:GridView ID="grdProcessGrphidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                 
                                                <asp:BoundField DataField="ProcessGrpCode" HeaderText="Process Grp Code" />
                                                <asp:BoundField DataField="SubProcessName" HeaderText="Sub Process Name" />
                                                <asp:BoundField DataField="ProcessUomDescription" HeaderText="Process UOM Description" />
                                                <asp:BoundField DataField="ProcessUOM" HeaderText="Process UOM" />
                                                

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
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
                                    <td colspan="2" style="width: 80%;">

                                        <asp:GridView ID="grdMachinelisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                            <Columns>
                                                <asp:BoundField DataField="Machine" HeaderText="Machine" />
                                                <asp:BoundField DataField="SMNStdrateHr" HeaderText="SMNStdrateHr" />
                                                <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                <asp:BoundField DataField="Currency" HeaderText="CURR" />

                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
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
                                    <td colspan="2" style="width: 80%;">

                                        <asp:GridView ID="grdLaborlisthidden" runat="server" AutoGenerateColumns="False" Enabled="false" CellPadding="4"
                                            Style="color: #333333; border-collapse: collapse; visibility: collapse;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                            <Columns>
                                                <asp:BoundField DataField="StdLabourRateHr" HeaderText="StdLabourRateHr" />
                                                <asp:BoundField DataField="FollowStdRate" HeaderText="FollowStdRate" />
                                                <asp:BoundField DataField="Currency" HeaderText="CURR" />

                                            </Columns>

                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>

                                    </td>
                                </tr>

                                </table>
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
                             


                        </div>
                    </div>
                    <!-- DataTables Example -->
                    <!-- /.container-fluid -->
                    <!-- Sticky Footer -->
                </div>
                <!-- /.content-wrapper -->
            </div>
            <!-- /#wrapper -->
            <!-- Scroll to Top Button-->
            <a class="scroll-to-top rounded" href="#page-top"><i class="fas fa-angle-up"></i>
            </a>
            <!-- Logout Modal-->
            <div class="modal fade" id="logoutModal" tabindex="-1"
                role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog"
                    role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"
                                id="exampleModalLabel">Ready to Leave? Ready to Leave?"> Ready to Leave? Ready to Leave?</h5>
                            <button class="close"
                                type="button" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            Select "Logout" below if you are ready
    to end your current session.
                        </div>
                        <div class="modal-footer">
                            <button class="btn
    btn-secondary"
                                type="button" data-dismiss="modal">
                                Cancel</button>
                            <a class="btn
    btn-primary"
                                href="login.html">Logout</a>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Bootstrap  core JavaScript-->
    </form>
</body>
</html>