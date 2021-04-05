<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addcolumn.aspx.cs" Inherits="Material_Evaluation.addcolumn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" >
    
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
<body>
    <form id="form1" runat="server">
    <div>
    <table>
    <tr>
    
    <td>
    
    <asp:gridview ID="Gridview1" runat="server" ShowFooter="true" 
            AutoGenerateColumns="true">

        <Columns>

        <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" />--%>

        <asp:TemplateField HeaderText="Header 1">

            <ItemTemplate>

                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

            </ItemTemplate>

        </asp:TemplateField>

        <asp:TemplateField HeaderText="Header 2">

            <ItemTemplate>

                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>

            </ItemTemplate>

        </asp:TemplateField>

        <asp:TemplateField HeaderText="Header 3">

            <ItemTemplate>

                 <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>

            </ItemTemplate>

            <FooterStyle HorizontalAlign="Right" />

            <FooterTemplate>

            

            </FooterTemplate>

        </asp:TemplateField>

        </Columns>

</asp:gridview>

    </td>
    </tr>
    <tr>
    <td>
    <asp:Button ID="ButtonAdd" runat="server" Text="Add New Column" 
            onclick="ButtonAdd_Click" />
    </td>
    </tr>
    </table>

    </div>
    </form>
</body>
</html>
