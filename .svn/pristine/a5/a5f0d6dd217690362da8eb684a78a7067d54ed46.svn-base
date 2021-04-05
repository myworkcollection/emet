<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VNewRequest.aspx.cs" Inherits="Material_Evaluation.VNewRequest" %>

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
    </style>

  

    
     <script src="/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="/Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />

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
     
        function DisplayNewImageInWidnow() {
        var img = document.getElementById('<%= Image3.ClientID %>').src;


            html = "<HTML><HEAD><TITLE>Photo</TITLE>"
                + "</HEAD><BODY LEFTMARGIN=0 "
                + "MARGINWIDTH=0 TOPMARGIN=0 MARGINHEIGHT=0><CENTER>"
                + "<IMG src='"
                + img
                + "' BORDER=0 NAME=image "
                + "onload='window.resizeTo(document.image.width,document.image.height)'>"
                + "</CENTER>"
                + "</BODY></HTML>";
            popup = window.open('', 'image', 'toolbar=0,location=0,directories=0,menuBar=0,scrollbars=0,resizable=1');
            popup.document.open();
            popup.document.write(html);
            popup.document.focus();
            popup.document.close();
        }

        function preventInput(evnt) {

            if (evnt.which != 9) evnt.preventDefault();

        }

        function checkAll(objRef) {

            var GridView = document.getElementById("<%=grdvendor.ClientID %>");

            var inputList = GridView.getElementsByTagName("chkheader");

         //   alert(inputList);

            for (var i = 0; i < inputList.length; i++) {

                //Get the Cell To find out ColumnIndex

                var row = inputList[i].parentNode.parentNode;

              //  alert(row);

              //  alert("row");

                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                    if (objRef.checked) {

                        //If the header checkbox is checked

                        //check all checkboxes

                        //and highlight all rows

                        row.style.backgroundColor = "aqua";

                        inputList[i].checked = true;

                    }

                    else {

                        //If the header checkbox is checked

                        //uncheck all checkboxes

                        //and change rowcolor back to original

                        if (row.rowIndex % 2 == 0) {

                            //Alternating Row Color

                            row.style.backgroundColor = "#C2D69B";

                        }

                        else {

                            row.style.backgroundColor = "white";

                        }

                        inputList[i].checked = false;

                    }

                }

            }

        }

        function Check_Click(objRef) {

            // alert("welcome");

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode;

            if (objRef.checked) {

                //If checked change color to Aqua

                row.style.backgroundColor = "aqua";

            }

            else {

                //If not checked change back to original color

                if (row.rowIndex % 2 == 0) {

                    //Alternating Row Color

                    row.style.backgroundColor = "#C2D69B";

                }

                else {

                    row.style.backgroundColor = "white";

                }

            }



            //Get the reference of GridView

            var GridView = row.parentNode;



            //Get all input elements in Gridview

            var inputList = GridView.getElementsByTagName("chkchild");



            for (var i = 0; i < inputList.length; i++) {

                //The First element is the Header Checkbox

                var headerCheckBox = inputList[0];



                //Based on all or none checkboxes

                //are checked check/uncheck Header Checkbox

                var checked = true;

                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {

                    if (!inputList[i].checked) {

                        checked = false;

                        break;

                    }

                }

            }

            headerCheckBox.checked = checked;

        }

        function SelectAll(id) {

            var grid = document.getElementById("<%= grdvendor.ClientID %>");
            //  alert(grid);
            var cell;

            // alert("muthu");

            if (grid.rows.length > 0) {

                for (i = 1; i < grdvendor.rows.length; i++) {



                    cell = grid.rows[i].cells[2];

                    alert(cell);


                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {

                            cell.childNodes[j].checked = id.checked;
                        }
                    }
                }
            }
        }


        function checkAll(ele) {
            var checkboxes = document.getElementsByTagName('input');
            if (ele.checked) {
                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = true;
                    }
                }
            } else {
                for (var i = 0; i < checkboxes.length; i++) {
                    console.log(i)
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = false;
                    }
                }
            }

        }

        function ShowDialog() {
            $("#dialog1").dialog();
        }

        function MarkCheckBox(cntrl) {


            var gvWorkSpace = document.getElementById('<%=grdvendor.ClientID %>');

           // alert("first");
            var gvRow = gvWorkSpace.getElementsByTagName('tr');
            for (var i = 1; i < gvRow.length; i++) {
                var gvCntrl = gvRow[i].getElementsByTagName('chkheader');
                for (var j = 0; j < gvCntrl.length; j++) {
                    if (gvCntrl[j].type == "checkbox") {

                      //  alert("Hi");

                        if (CmdtType == 'Delete') {
                            if (gvCntrl[j].disabled != true) {
                                gvCntrl[j].checked = cntrl.checked;
                                break;
                            }
                        }

                        else if (CmdtType == 'UpdateDate') {
                            gvCntrl[j + 1].checked = cntrl.checked;
                            break;
                        }
                    }
                }
            }
        }



    </script>
    <script type="text/javascript" language="javascript">



        function SelectAll(CheckBox) {
            TotalChkBx = parseInt('<%= this.grdvendor.Rows.Count %>');
            var TargetBaseControl = document.getElementById('<%= this.grdvendor.ClientID %>');
            var TargetChildControl = "chkSelect";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[iCount].checked = CheckBox.checked;
            }
        }

        function SelectDeSelectHeader(CheckBox) {
            TotalChkBx = parseInt('<%= this.grdvendor.Rows.Count %>');
        var TargetBaseControl = document.getElementById('<%= this.grdvendor.ClientID %>');
            var TargetChildControl = "chkSelect";
            var TargetHeaderControl = "chkSelectAll";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var flag = false;
            var HeaderCheckBox;
            for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetHeaderControl, 0) >= 0)
                    HeaderCheckBox = Inputs[iCount];
                if (Inputs[iCount] != CheckBox && Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0 && Inputs[iCount].id.indexOf(TargetHeaderControl, 0) == -1) {
                    if (CheckBox.checked) {
                        if (!Inputs[iCount].checked) {
                            flag = false;
                            HeaderCheckBox.checked = false;
                            return;
                        }
                        else
                            flag = true;
                    }
                    else if (!CheckBox.checked)
                        HeaderCheckBox.checked = false;
                }
            }
            if (flag)
                HeaderCheckBox.checked = CheckBox.checked
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
                    <a class="nav-link" href="Vendor.aspx">
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
                    <a class="nav-link" href="VNewRequest.aspx">
                        <i class="fas fa-fw fa-table"></i>
                        <span>Create DO</span></a>


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

              


             
            </ul>
            <div id="header">
                <asp:ScriptManager ID="scriptmanager1" runat="server">
                </asp:ScriptManager>
            </div>
            <div id="content-wrapper">
                <div class="container-fluid">
                    <!-- Breadcrumbs-->
                    <ol class="breadcrumb">
                        <li class="breadcrumb-item"><%--<a href="#">First Article Item</a>--%> 
                        </li>
                        <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                            <tr>
                                <td>

                                    <asp:RadioButton ID="article" runat="server" Text="First Article Item"
                                        GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true"
                                        OnCheckedChanged="article_CheckedChanged" />

                                </td>
                                <td>
                                    <asp:RadioButton ID="changevendr" runat="server" Text="Change of Vendor"
                                        GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true"
                                        OnCheckedChanged="changevendr_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="draftcost" runat="server" Text="Draft and Cost Planning"
                                        GroupName="RegularMenu" TextAlign="Left" AutoPostBack="true"
                                        OnCheckedChanged="draftcost_CheckedChanged" />
                                </td>
                            </tr>

                        </table>


                    </ol>
                    <!-- Icon Cards-->
                    <div class="row">
                    </div>
                    <!-- Area Chart Example-->
                    <div class="card mb-3">
                        <%--<div class="card-header">
                        <i class="fas fa-chart-area"></i>TEAM SHIMANO S.E.A. QUOTATION
                        
                       
                        </div>--%>
                        <div class="card-body">
                            <!--<canvas id="myAreaChart" width="100%" height="30"></canvas>-->

                            <table width="100%" border="0" style="border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: rgba(0,0,0,.03);">
                                <tr>
                                    <td colspan="4" align="right"></td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                    <td>
                                        <asp:Label ID="lbluser" runat="server" Text="Label" Visible="false"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_date" runat="server" Text="Date :"
                                            Width="150px" Font-Bold="True" ForeColor="Black"
                                            Style="margin-bottom: 0px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtReqDate" runat="server" AutoCompleteType="Disabled" autocomplete="off"
                                            onkeydown="javascript:preventInput(event);"></asp:TextBox>
                                    </td>

                                    <td align="left">
                                        <asp:Label ID="Label12" runat="server" Text="Plant :" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtplant" runat="server" Height="30px" Width="90px"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>

                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Material Type:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <%--<asp:TextBox ID="txtmatltype" runat="server" Height="31px" Width="300px" CssClass="tetxboxcolor"></asp:TextBox>--%>

                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>

                                                <asp:DropDownList ID="ddlmatltype" runat="server" Font-Bold="True" Height="30px" Width="100px"
                                                    ForeColor="Black" AutoPostBack="true" OnSelectedIndexChanged="ddlmatltype_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlmatltype" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>


                                    <td align="left">
                                        <asp:Label ID="Label4" runat="server" Text="Plant Status :" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <%--<asp:TextBox ID="txtplantstatus" runat="server" Height="23px" Width="300px" CssClass="tetxboxcolor"></asp:TextBox>--%>

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>

                                                <asp:DropDownList ID="ddlplantstatus" runat="server" Height="30px" AutoPostBack="true"
                                                    Width="100px" OnSelectedIndexChanged="ddlplantstatus_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlplantstatus" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="SAP Proc Type :"
                                            Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:TextBox ID="txtproctype" runat="server" AutoPostBack="true" Width="250px" 
                                Font-Bold="True" ForeColor="Black" Height="30px"></asp:TextBox>--%>

                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>

                                                <asp:DropDownList ID="ddlproctype" runat="server" AutoPostBack="true"
                                                    Width="100px" Font-Bold="True" ForeColor="Black" Height="30px"
                                                    OnSelectedIndexChanged="ddlproctype_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlproctype" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>

                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="SAP Sp Proc Type :"
                                            Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <%-- <asp:TextBox ID="txtSAPspProctype" runat="server" Width="250px" Height="30px"></asp:TextBox>
                             <asp:AutoCompleteExtender ID="acprod" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode"
                                EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getProduct"
                                TargetControlID="txtSAPspProctype" UseContextKey="True">
                            </asp:AutoCompleteExtender> --%>

                                                <asp:DropDownList ID="ddlsplproctype" runat="server" AutoPostBack="true"
                                                    Width="70px" Font-Bold="True" ForeColor="Black" Height="30px"
                                                    OnSelectedIndexChanged="ddlsplproctype_SelectedIndexChanged">
                                                </asp:DropDownList>



                                                <asp:TextBox ID="txtsplproc" runat="server" Width="69px" Visible="false"></asp:TextBox>



                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlsplproctype" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Product:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>


                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField runat="server" ID="hprodID" />
                                                <asp:TextBox ID="txtprodID" runat="server" Height="30px" TextMode="MultiLine" AutoPostBack="true"
                                                    Width="250px" OnTextChanged="txtprodID_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode"
                                                    EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getProduct"
                                                    TargetControlID="txtprodID" UseContextKey="True">
                                                </asp:AutoCompleteExtender>


                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtprodID" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>

                                    <%-- <tr>
                            <td class="style6">
                                <asp:Label ID="lbl_cntact0" runat="server"  Height="16px" 
                                    Text="Plant" Width="62px" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                            <td class="style5">
                                <asp:DropDownList ID="ddlplant" runat="server" AutoPostBack="true" 
                                    Height="30px" OnSelectedIndexChanged="ddlplant_SelectedIndexChanged" 
                                    Width="300px">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>

                                    <td>
                                        <asp:Label ID="lblmatldesc" runat="server" Text="Material Class Description:" Font-Bold="True"
                                            ForeColor="Black" Width="180px"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>

                                                <%-- <asp:DropDownList ID="ddlmatlclass" runat="server" AutoPostBack="true" Height="30px"
                                Width="250px">
                            </asp:DropDownList>--%>

                                                <asp:TextBox ID="txtmatlclass" runat="server" Height="30px" TextMode="MultiLine" AutoPostBack="true"
                                                    Width="250px" OnTextChanged="txtmatlclass_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode"
                                                    EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getMatlClass"
                                                    TargetControlID="txtmatlclass" UseContextKey="True">
                                                </asp:AutoCompleteExtender>


                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtmatlclass" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>

                                <%--<tr>
                        <td class="style6">
                            <asp:Label ID="Label6" runat="server" Text="SAP Part Code Description :" Font-Bold="True" 
                                ForeColor="Black"></asp:Label>
                        </td>
                        <td class="style5">
                            <asp:TextBox ID="txtpartdescription" runat="server" AutoPostBack="true" Height="29px" TextMode="MultiLine" CssClass="tetxboxcolor"
                                Width="300px"></asp:TextBox>
                           
                        </td>
                    </tr>--%>


                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="SAP Part Code :" Font-Bold="True" Enabled="false"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>

                                                <asp:TextBox ID="txtpartdesc" ToolTip="Enter min 5 chars if no filters choosen!" runat="server" Height="30px" AutoPostBack="true" TextMode="MultiLine"
                                                    Width="150px" OnTextChanged="txtpartdesc_TextChanged1"></asp:TextBox>
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
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Part Code Description :"
                                            Width="160px" Font-Bold="True" Enabled="false" ForeColor="Black"></asp:Label>
                                    </td>

                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                            <ContentTemplate>

                                                <asp:TextBox ID="txtpartdescription" ToolTip="Enter min 10 chars if no filters choosen!" runat="server" Width="250px" Height="30px" AutoPostBack="true"
                                                    Font-Bold="True" ForeColor="Black"
                                                    OnTextChanged="txtpartdescription_TextChanged1"></asp:TextBox>

                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" CompletionInterval="1" CompletionListElementID="pnlPrdCode"
                                                    EnableCaching="true" FirstRowSelected="true" MinimumPrefixLength="1" ServiceMethod="getSapPartdesc"
                                                    TargetControlID="txtpartdescription" UseContextKey="True">
                                                </asp:AutoCompleteExtender>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtpartdescription" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="PIR Type:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                            <ContentTemplate>


                                                <%-- <asp:TextBox ID="txtPIRType" runat="server" Height="21px" Width="62px"></asp:TextBox>--%>
                                                <asp:DropDownList ID="ddlpirtype" runat="server" Font-Bold="True" AutoPostBack="true"
                                                    ForeColor="Black" Height="30px" Width="100px"
                                                    OnSelectedIndexChanged="ddlpirtype_SelectedIndexChanged1">
                                                </asp:DropDownList>

                                                <asp:TextBox ID="txtPIRDesc" runat="server" Height="30px" Width="173px"></asp:TextBox>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlsplproctype" />
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </td>


                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Net Weight & Base UOM:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>


                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel111" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtunitweight" runat="server" AutoPostBack="true" AutoCompleteType="Disabled" autocomplete="off" Height="30px" Width="70px"></asp:TextBox>
                                                <asp:TextBox ID="txtUOM" runat="server" AutoPostBack="true" AutoCompleteType="Disabled" autocomplete="off" Height="30px" Width="80px"></asp:TextBox>
                                                <asp:TextBox ID="txtBaseUOM1" runat="server" AutoPostBack="true" AutoCompleteType="Disabled" autocomplete="off" Height="30px" Width="70px"></asp:TextBox>

                                                  </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtpartdesc" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                         
                                    </td>
                                  
                                </tr>

                                <tr>

                                    <td>
                                        <asp:Label ID="lbldrawng" runat="server" Text="Drawing No :"
                                            Width="150px" Font-Bold="True" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        
                                                 <asp:FileUpload ID="FileUpload1" ToolTip="" runat="server" EnableViewState="true" Height="30px"
                                            Width="200px" />

                                       
                                               
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                 <asp:Label ID="Label15" runat="server" Text="Check PO"></asp:Label>

                                       
                                               
                                    </td>

                                    <td>
                                         <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                            <ContentTemplate>
                                                  <asp:Button ID="btnUpload" runat="server" Text="Check Stock"
                                            Width="92px" OnClick="btnUpload_Click" />

                                        </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUpload" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                                </tr>
                               
                                <tr>

                                    <td colspan="4">

                                        <asp:UpdatePanel ID="UpdatePanel171" runat="server">
                                            <ContentTemplate>

                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Black" Text=""></asp:Label>

                                        </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnUpload" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </td>


                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="3">


                                        <asp:ImageButton ID="Image3" runat="server" Height="50" Width="50" Visible="false"
                                            />
                                        <asp:HiddenField ID="imgHidden" runat="server" />


                                        <%--<asp:Image ID="Image3" runat="server" Height = "100" Width = "100" />--%>
                                        <asp:LinkButton ID="btnViewDownPDF" target="_blank" OnClick="btnViewDownPDF_Click" runat="server" Text=" Download & View"></asp:LinkButton>
                                        <hr />
                                        <asp:Literal ID="ltEmbed" runat="server" />
                                    </td>


                                </tr>
                                <tr>

                                    <td>
                                        <asp:Label ID="lbl_cntact2" runat="server" Font-Bold="True" ForeColor="Black"
                                            Text="Process Group :" Width="150px"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>


                                                <asp:DropDownList ID="ddlprocess" runat="server" AutoPostBack="true" Height="30px"
                                                    Width="100px" Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="ddlprocess_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlprocess" />
                                            </Triggers>
                                        </asp:UpdatePanel>

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
                                    </td>
                                    <td>
                                        <asp:Label ID="lbljobtypedesc" runat="server" Text="PIR Job Type & Description:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>

                                                <asp:TextBox ID="txtjobtypedesc" runat="server" AutoPostBack="true" Height="30px"
                                                    Width="250px" OnTextChanged="txtjobtypedesc_TextChanged"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" CompletionInterval="1"
                                                    CompletionListElementID="pnlPrdCode" EnableCaching="true" FirstRowSelected="true"
                                                    MinimumPrefixLength="1" ServiceMethod="getSAPJOB" TargetControlID="txtjobtypedesc"
                                                    UseContextKey="True">
                                                </asp:AutoCompleteExtender>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtjobtypedesc" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>


                                <tr>

                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="Plating Type:" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtplatingtype" runat="server" AutoPostBack="true" Height="30px" autocomplete="off" Width="250px"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtplatingtype" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbljobtypedesc0" runat="server" Text="Quotation response due date" Font-Bold="True"
                                            ForeColor="Black"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" AutoCompleteType="Disabled" autocomplete="off"
                                            onkeydown="javascript:preventInput(event);"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>

                                    <td colspan="4"></td>
                                </tr>

                                <tr>

                                    <td>
                                        <asp:Label ID="lbl_vendrName" runat="server" Text="Select Vendors :"
                                            Font-Bold="True" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td colspan="3">

                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>

                                                <asp:GridView ID="grdvendor" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    GridLines="both" ForeColor="#333333" Width="450px">
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

                                                        <asp:TemplateField HeaderStyle-BackColor="#FBDCA3"
                                                            HeaderText="SELECT TO ACCESS">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chk" runat="server" EnableViewState="true" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:BoundField DataField="VendorCode" HeaderText="Vendor ID" />
                                                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                                                        <asp:BoundField DataField="SearchTerm"  HeaderText="Search Term" />

                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <FooterStyle BackColor="#1a2e4c" ForeColor="White" Font-Bold="True" />
                                                    <HeaderStyle BackColor="#e6f2ff" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#1a2e4c" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                                </asp:GridView>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdvendor" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="4"></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnsave" runat="server" CssClass="Login-button"
                                                        Text="Create Request" Height="30px"
                                                        Width="150px" OnClick="btnsave_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsubmit" runat="server" CssClass="Login-button" PostBackUrl="vendor_Quotation.aspx"
                                                        Text="Submit" Visible="false" Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Quote Details :"
                                            Font-Bold="True" Visible="false" ForeColor="Black"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <div style="overflow: scroll;height: 500px;width: 80%;">
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <%--<asp:GridView ID="GridView2" runat="server">
                                  </asp:GridView>--%>
                                                <asp:Table ID="Table1" runat="server" class="GridStyle" CellSpacing="0" CellPadding="4"
                                                    rules="all" border="1" Style="color: #333333; border-color: Black; height: 50px; width: 350px; border-collapse: collapse;">
                                                </asp:Table>

                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="Table1" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                            </div>
                                        
                                    </td>
                                   
                                </tr>
                               
                                <tr align="center">
                                     <td>
                                        <asp:HiddenField ID="hdnReqNo" runat="server" />
                                        <asp:Button ID="Button1" runat="server" CssClass="Login-button"
                                            Text="Submit" Visible="false" Width="100px" OnClick="Button1_Click" />
                                    </td>
                                </tr>
                                   
                            </table>
                            <div id="dialog1" title="Full Image View" style="display: none">
                                <asp:Image ID="img1" runat="server" />
                            </div>
                            <!-- DataTables Example -->

                        </div>
                        <!-- /.container-fluid -->
                        <!-- Sticky Footer -->
                        <%-- <div class="card-footer small text-muted">
                        Updated yesterday at 11:59 PM</div>--%>
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
