<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Material_Evaluation.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>eMET - Login</title>
    <!-- Bootstrap core CSS-->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <!-- Custom styles for this template-->
    <link href="css/sb-admin.css" rel="stylesheet" />
    <link href="Styles/NewStyle/Stylelogin.css" rel="stylesheet" />
    <style type="text/css">
      .container{
        /*width: 200px;
        height: 200px;
        position: relative;
        margin: 20px;*/
        }
        .box{
            width: 100%;
            height: 100%;            
            position: absolute;
            top: 0;
            left: 0;
        }
        .stack-top{
            z-index: 9;
            opacity:0.9;
        }
        .Mycenter {
          display: block;
          margin-top:15%;
          margin-left: auto;
          margin-right: auto;
          width: 10%;
        }
    </style>
</head>
<body class="bg-dark1">
    <form action="login.aspx" id="form2" runat="server">
        <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
        <div class="container">
            <div class="box">
                <div class="card card-login mx-auto mt-5">
                <div class="card-body">
                    <div class="form-group">
                        <div class="col-md-12" style="padding-bottom: 20px; text-align: center">
                            <img alt="Shimano-logo" src="images/shimanologo.png" width="100%" height="100%" />
                            <asp:Label runat="server" ID="lblogo" Text="eMET LOGIN" Font-Bold="true" Font-Size="Larger"></asp:Label>
                        </div>

                        <asp:UpdatePanel runat="server" ID="UpForm">
                            <ContentTemplate>
                                <div class="col-md-12" style="padding-bottom: 10px;">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtLogin" MaxLength="100" name="user[username]" placeholder="User ID" AutoPostBack="true" OnTextChanged="txtLogin_TextChanged"
                                            CssClass="form-control" TabIndex="1" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:TextBox ID="txtLoginpassword" MaxLength="100" placeholder="Password"
                                        CssClass="form-control" TabIndex="2" TextMode="Password" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="DllPlant" CssClass="form-control" runat="server">
                                        <asp:ListItem Text="--Plant Not Exist--" Value="--Plant Not Exist--"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-12" style="padding-bottom: 10px;">
                                    <asp:CheckBox ID="chkremberme" runat="server" OnCheckedChanged="CheckedChanged"></asp:CheckBox>Remember Password
                                </div>

                                <div class="col-md-12" style="padding-bottom: 10px;">
                                    <asp:Button ID="LoginButton_DoLogin" class="btn btn-primary btn-block"
                                        runat="server" Height="45px" Width="100%" TabIndex="3" Text="Login" OnClientClick="ShowLoading();" OnClick="LoginButton_DoLogin_Click" />
                                </div>

                                <div class="col-md-12" style="text-align: center">
                                    <asp:Label runat="server" ID="LbsystemVersion" Text="Version 3.4"></asp:Label>
                                </div>


                                <div class="col-md-12" style="text-align: center">
                                    <asp:Label ID="lblUserName" runat="server" size="20" Visible="False" ForeColor="Red"></asp:Label>
                                    <a class="d-block small" href="forgot-password.html"></a>
                                </div>

                                <div style="display: none">
                                    <asp:TextBox runat="server" ID="TxtPlant" Text=""></asp:TextBox>
                                    <asp:TextBox runat="server" ID="TxtDirectFromMDM" Text=""></asp:TextBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            </div>
            
            <div id="loading" class="box stack-top" style="background: white;">
                    <img id="loading-image" src="images/loading.gif" alt="Loading..."  class="Mycenter"/>
                    <div class="col-sm-12" style="text-align: center; opacity: 100;">
                        <asp:Label ID="lbLoading" runat="server" Text="please Wait..." Font-Bold="true" ForeColor="#0000ff" Visible="false"></asp:Label>
                    </div>
                </div>
        </div>
    </form>
    <!-- Bootstrap core JavaScript-->
    <script type="text/javascript" src="vendor/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- Core plugin JavaScript-->
    <script type="text/javascript" src="vendor/jquery-easing/jquery.easing.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.8.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery-v1.9.1-ui.min.js"></script>

    

    <script type="text/javascript">
        $(document).ready(function () {
            //window.history.pushState(null, "", window.location.href);
            //window.onpopstate = function () {
            //    window.history.pushState(null, "", window.location.href);
            //};

            LoginFromMDM();
        });

        $(window).load(function () {
            
        });
    </script>

    <script type="text/javascript">
        function ShowLoading() {
            $('#loading').show();
        }
        function CloseLoading() {
            $('#loading').fadeOut("fast");
        }

        function LoginFromMDM() {
            var plant = "";
            var Useid = "";

            var ca = document.cookie.split(';');
            var c = ca[0];
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                if (ca[i].toString().includes("useid")) {
                    var d = ca[i].split('=');
                    if (d.lenght = 2) {
                        Useid = d[1].toString();
                    }
                }
                else if (ca[i].toString().includes("Plant")) {
                    var d = ca[i].split('=');
                    if (d.lenght = 2) {
                        plant = d[1].toString();
                    }
                }
            }

            if (plant != "" && Useid != "") {
                $("#txtLogin").val(Useid);
                $("#TxtPlant").val(plant);
                $("#TxtDirectFromMDM").val("1");
                document.cookie = "useid=;";
                document.cookie = "Plant=;";
                document.getElementById("LoginButton_DoLogin").click();
            }
            else {
                $('#loading').fadeOut("fast");
            }
        }
    </script>
</body>
</html>
