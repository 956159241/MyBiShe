<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutStore.aspx.cs" Inherits="WebApp.OutStore" %>

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
                    <td>&nbsp;&nbsp;出库单号&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;配件编号&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;配件名称&nbsp;&nbsp;</td>
                    <td>&nbsp;&nbsp;出库数量&nbsp;&nbsp;</td>
                     <td>&nbsp;&nbsp;需求商&nbsp;&nbsp;</td>
                </tr>
            </table>
        <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
        <div>
            <asp:TextBox ID="txtOutNum" runat="server" Text=<%#Eval("OutNum") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtPNum" runat="server" Text=<%#Eval("PNum") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtPName" runat="server" Text=<%#Eval("PName") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtOutQuantity" runat="server" Text=<%#Eval("OutQuantity") %> Width="80"></asp:TextBox>
            <asp:TextBox ID="txtNeedMerchant" runat="server" Text=<%#Eval("NeedMerchant") %> Width="80"></asp:TextBox> 
        </div>
        </ItemTemplate>
        </asp:Repeater>
        <br />
        <asp:Button ID="Button1" runat="server" Text="出库" OnClick="OutStore_Click" />
    </div>
    </form>
</body>
</html>
