<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ed.aspx.cs" Inherits="WebService.ed" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="En"></asp:Label>
                <br />
                <asp:TextBox ID="txten" runat="server" width="100%" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="De"></asp:Label>
                <br />
                <asp:TextBox ID="txtde" runat="server" width="100%" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="bten" runat="server" Text="En" OnClick="bten_Click" Width="70px" />&nbsp;&nbsp;&nbsp; <asp:Button ID="btde" runat="server" Text="De" OnClick="btde_Click" Width="70px" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
