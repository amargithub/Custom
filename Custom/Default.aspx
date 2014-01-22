
<%@ Page Language="C#" AutoEventWireup="true" ViewStateMode="Disabled" CodeBehind="Default.aspx.cs" Inherits="Ldapform.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>QlikView - AccessPoint</title>
        <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
        <link rel="stylesheet" href="global.css" type="text/css" media="screen" />
        <link rel="stylesheet" href="custom.css" type="text/css" media="screen" />
       </head>
<body>
     <div id="frame">
        <div id="nav_utility"></div>
        <div id="header">
            <a href="#">
                <img src="images/logo_main.png" alt="QlikView" id="logo_main"/>
            </a>
            <span class="docNum"></span>
            <span class="lastUpdated"></span>
        </div>
        <div id="loginBox">
            <form runat="Server" id="MainForm" autocomplete="off">
		<span class="formTitle top">User Name:</span>
        <span class="formField">
         <asp:TextBox runat="server" id="txtUser"   ToolTip="username" CssClass="top"></asp:TextBox>
        </span> 
        <span class="formTitle">Password:</span> 
           <span class="formField">
                <asp:TextBox runat="server" id="txtPassword" TextMode="Password"></asp:TextBox>
           </span>
      <%--  Groups:  <asp:TextBox runat="server" id="txtGroups">Sales,Finance</asp:TextBox> <br />--%>
        <asp:Button ID="GO"  runat="server" Text="Login..." onclick="GO_Button_Click" CssClass="loginSubmitButton" />
        <asp:Label ID="result" runat="server" Text=""></asp:Label>
</form>
        </div>
    </div>
    <div id="footer"></div>


   <script type="text/javascript" src="js/jquery-1.7.min.js"></script>
</body>
</html>


