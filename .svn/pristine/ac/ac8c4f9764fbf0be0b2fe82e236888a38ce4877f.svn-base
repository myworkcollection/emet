<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintApproved.aspx.cs" Inherits="Material_Evaluation.PrintApproved" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background-color:white">
<head runat="server" style="background-color:white">
    <title>eMET</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <link href="Styles/bootstrap-3.4.1-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />
    <link href="css/sb-admin.css" rel="stylesheet" />
    <link href="Styles/NewStyle/NewStyle.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
     <link rel="stylesheet" href="js/jsextendsession/css/timeout-dialog.css" />
    <style type="text/css">
        div {
        background-color:white;
        }
        .zeropading {
        padding:0px!important;
        }
    </style>

    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/jQuery-v3.4.0.min.js"></script>
    <script type="text/javascript" src="Styles/bootstrap-3.4.1-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Scripts/stickycolumandheaderplugin/tableHeadFixer.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jsextendsession/js/jquery.idle-timer.js"></script>
    <script type="text/javascript" src="js/jsextendsession/js/timeout-dialog.js"></script>
</head>
<body">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <div class="col-md-12" id="DvMsgErr" runat="server" visible="false">
            <asp:Label runat="server" ID="LbMsgErr" Font-Bold="true" Visible="true"></asp:Label>
        </div>
    <div class="container-fluid">
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
        <div class="col-md-12" style="padding-left:50px; padding-right:50px; padding-top:28px;">
            <div class="col-md-12" style="padding:10px;">
                <div class="row">
                    <div class="col-md-12 text-center">
                        <asp:Label runat="server" ID="Label1" Text="SHIMANO e-MET" Font-Size="30px"
                            Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row" style="padding-bottom:2px;">
                    <div class="col-md-12" style="border:2px solid black">
                        <asp:Label runat="server" ID="Label2" Text="SHIMANO DETAILS" Font-Size="24px" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 Padding-Nol">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label3" Text="SMN PIC" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbSMNPIC" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="display:none;">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label4" Text="Email" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LblEmail" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label5" Text="Plant & Department" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LblPlnDept" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="display:none">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label6" Text="Purpose of Quotation" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LblPurposeQuote" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="padding-bottom:2px;">
                    <div class="col-md-12" style="border:2px solid black">
                        <asp:Label runat="server" ID="Label7" Text="VENDOR DETAILS" Font-Size="24px" 
                            Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 Padding-Nol">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label8" Text="Submitted By" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbSubmitBy" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label10" Text="Submitted Date" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbSubmitDate" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label12" Text="Vendor" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbVendor" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label14" Text="Country" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbCountry" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label16" Text="Currency" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbCurrency" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label18" Text="Quote No" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbQuoteNo" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="padding-bottom:2px;">
                    <div class="col-md-12" style="border:2px solid black">
                        <asp:Label runat="server" ID="Label9" Text="PART I : QUOTED PART INFO" Font-Size="24px" 
                            Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 Padding-Nol">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label11" Text="Product" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbProduct" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label15" Text="Part Code & Desc" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="lbPartnDesc" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label19" Text="SAP PIR Job Type & Desc" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbSAPPIRJobType" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label21" Text="PIR Type & Desc" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbPIRType" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="display:none">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label23" Text="Part Drawing" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbPartDrawing" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label25" Text="Process Group" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbProcGroup" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label27" Text="Base UOM" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbBaseUOM" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label29" Text="Net Weight" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbNetWeight" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="display:none">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label26" Text="Remark" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbERemark" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label33" Text="Mnth.Est.Qty & UOM" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbMQty" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label35" Text="SMN Effective Date" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbQuoteEffDate" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label37" Text="SMN Next Revision" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbDueDate" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label39" Text="Country of Origin" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbCrOrgi" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label30" Text="Request Purposes" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbReqReason" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="padding-bottom:2px;">
                    <div class="col-md-12" style="border:2px solid black">
                        <asp:Label runat="server" ID="Label13" Text="PART VI : PART UNIT PRICE" Font-Size="24px" 
                            Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="table-responsive table-sm table-bordered zeropading" style="background-color:white;">
                        <asp:Table ID="TableUnit" runat="server" CssClass="table-bordered table-nowrap table-bordered" BackColor="White" ></asp:Table>
                    </div>
                </div>
                <div class="row" style="display:none;">
                    <div class="col-md-12 Padding-Nol">
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label22" Text="Comment By Vendor" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbCmntVnd" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12" style="border:2px solid black">
                        <asp:Label runat="server" ID="Label17" Text="MANAGEMENT DECISION : APPROVED" Font-Size="24px" 
                            Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 Padding-Nol">
                        <div class="row" style="display:none">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label20" Text="Approval Status" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbApprStatus" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label28" Text="Approved By" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbApprvalBy" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label31" Text="Approval Date" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbApprDate" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <asp:Label runat="server" ID="Label24" Text="Approval Comment" Font-Size="20px"></asp:Label>
                            </div>
                            <div class="col-xs-8">
                                <asp:Label runat="server" ID="LbApprCmnt" Text=": " Font-Size="20px"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hdnUnitValues" runat="server" Value="" />
        <asp:HiddenField ID="HdnTotProcCostBeforeProfNDisc" runat="server" Value="" />
        <asp:HiddenField ID="hdnVendorType" runat="server" Value="" />
        <asp:HiddenField ID="hdnNetProfDisc" runat="server" Value="" />
        <asp:HiddenField ID="hdnGA" runat="server" Value="" />
            <asp:HiddenField ID="hdnLayoutScreen" runat="server" Value="" />
        </contenttemplate>
        </asp:UpdatePanel>
        
        <div style="display:block;">
            <asp:Button Width="100px" Style="margin-left: 3px" ID="Button1" Height="27px" runat="server" OnClick="Button1_Click" Text="Create PDF" />
        </div>
    </div>
    </form>
    <script lang="javascript" type="text/javascript">
        $(window).load(function () {
            
        });

        $(document).ready(function () {
            try {
                $('#Button1').click();

                $(function () {
                    var timeout = 500;
                    $(document).bind("idle.idleTimer", function () {
                        // function you want to fire when the user goes idle
                        window.close();
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
        });
    </script>
</body>
</html>
