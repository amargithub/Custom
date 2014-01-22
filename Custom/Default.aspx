
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Ldapform.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="Server" id="MainForm">
		<h2>QlikView WebTicket Example</h2>
		Username:  <asp:TextBox runat="server" id="txtUser"></asp:TextBox> <br />
        Password:  <asp:TextBox runat="server" id="txtPassword" TextMode="Password"></asp:TextBox> <br />
        Groups:  <asp:TextBox runat="server" id="txtGroups">Sales,Finance</asp:TextBox> <br />
        <asp:Button ID="GO" runat="server" Text="Login..." onclick="GO_Button_Click" />
        <asp:Label ID="result" runat="server" Text=""></asp:Label>
</form>
</body>
</html>
