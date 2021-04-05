<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2_Ra.aspx.cs" Inherits="Material_Evaluation.WebForm2_Ra" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        input[type=text]
        {
            margin-right:5px;
            position:relative;
            top:-2px
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("[id*=txtDate]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                dateFormat: "dd/mm/yy",
                minDate:0,
                buttonImage: 'images/calendar.png'
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:TextBox ID="txtDate" runat="server" ReadOnly = "true"></asp:TextBox>
    <hr />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
        />
    </form>
</body>
</html>
