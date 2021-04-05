<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication2.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:gridview ID="Gridview1" runat="server" ShowFooter="true" AutoGenerateColumns="false">
            <Columns>
            <asp:BoundField DataField="RowNumber" HeaderText="Row Number" />
            <asp:TemplateField HeaderText="Header 1" Visible ="true">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </ItemTemplate>
               <FooterStyle HorizontalAlign="Right" />
                <FooterTemplate>
               
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Header 2" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Header 3" Visible ="false">
                <ItemTemplate>
                     <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </ItemTemplate>                
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Header 4" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Header 5" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Header 6" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
               <asp:TemplateField HeaderText="Header 7" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                </ItemTemplate>
                  </asp:TemplateField>
                <asp:TemplateField HeaderText="Header 8" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                </ItemTemplate>
                  </asp:TemplateField>
                <asp:TemplateField HeaderText="Header 9" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                </ItemTemplate>
                  </asp:TemplateField>
                <asp:TemplateField HeaderText="Header 10" Visible ="false">
                <ItemTemplate>
                    <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>

                </ItemTemplate>
                  
            </asp:TemplateField>
            </Columns>
        </asp:gridview>
    </div>
    <div>
     <%-- <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" 
                        onclick="ButtonAdd_Click" />  --%>             
               <asp:Button ID="ButtonAddCol" runat="server" Text="Add New Column" 
                        onclick="ButtonAddCol_Click" />
    </div>
    </form>
</body>
</html>
