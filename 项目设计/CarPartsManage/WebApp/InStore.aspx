<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InStore.aspx.cs" Inherits="WebApp.InStore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="background-color:#99b9e0">
    <form id="form1" runat="server">
    <div>
        <table>
                <tr>
                    <td>&nbsp;&nbsp;入库单号&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;配件编号&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;配件名称&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;配件单价&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;入库数量&nbsp;&nbsp;</td>
                </tr>
            </table>
        <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
        <div>
            <%--<table>
                <tr>
                    <td>入库单号</td>
                    <td>配件编号</td>
                    <td>配件名称</td>
                    <td>配件单价</td>
                    <td>入库数量</td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txtInNum" runat="server" Text=<%#Eval("InNum") %>></asp:TextBox></td>
                    <td><asp:TextBox ID="txtPNum" runat="server" Text=<%#Eval("PNum") %>></asp:TextBox></td>
                    <td><asp:TextBox ID="txtPName" runat="server" Text=<%#Eval("PName") %>></asp:TextBox></td>
                    <td><asp:TextBox ID="txtUnitPrice" runat="server" Text=<%#Eval("UnitPrice") %>></asp:TextBox></td>
                    <td><asp:TextBox ID="txtInQuantity" runat="server" Text=<%#Eval("InQuantity") %>></asp:TextBox></td>
                </tr>
            </table>--%>
            <asp:TextBox ID="txtInNum" runat="server" Text=<%#Eval("InNum") %> Width="80"> </asp:TextBox>
            <asp:TextBox ID="txtPNum" runat="server" Text=<%#Eval("PNum") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtPName" runat="server" Text=<%#Eval("PName") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtUnitPrice" runat="server" Text=<%#Eval("UnitPrice") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtInQuantity" runat="server" Text=<%#Eval("InQuantity") %> Width="80"></asp:TextBox> 
        </div>
        </ItemTemplate>
        </asp:Repeater>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="InStore_Click" Text="入库 " />
    </div>
    </form>
</body>
</html>
