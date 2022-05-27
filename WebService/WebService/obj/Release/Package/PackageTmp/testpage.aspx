<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testpage.aspx.cs" Inherits="WebService.testpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="lbAppTitle" runat="server"></asp:Label>
        <asp:Button ID="btnTest" runat="server" Text="Test" OnClick="btnTest_Click" />
        
    </div>
    </form>
</body>
</html>
