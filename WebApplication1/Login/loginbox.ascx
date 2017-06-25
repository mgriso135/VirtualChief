<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="loginbox.ascx.cs" Inherits="KIS.Login.loginbox" %>

<table id="tblLogin" border="0" runat="server" class="table table-bordered table-hover">
<tr><td colspan="2" style="font-size: 14px; font-weight: bold">
    <asp:Literal runat="server" ID="lblLoginBox" Text="<%$Resources:lblLoginBox %>" /></td></tr>
<tr>
<td><asp:Literal runat="server" ID="lblUsername" Text="<%$Resources:lblUsername %>" />:</td>
<td><asp:textbox runat="server" id="usr"/>
<asp:RequiredFieldValidator runat="server" ControlToValidate="usr" ErrorMessage="<%$Resources:lblValReqField %>" ForeColor="Red" id="valName" />
</td>
</tr>
<tr>
<td><asp:Literal runat="server" ID="lblPassword" Text="<%$Resources:lblPassword %>" />:</td>
<td>
<asp:textbox runat="server" id="pwd" TextMode="Password" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="pwd" ErrorMessage="<%$Resources:lblValReqField %>" ForeColor="Red" id="valPass" />
</td>
</tr>
<tr><td colspan="2"><asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="<%$Resources:lblBtnLogin %>" CausesValidation="true" /></td></tr>
</table>

<table border="0" runat="server" id="tblLogout">
<tr>
<td><asp:Button runat="server" Text="<%$Resources:lblBtnLogout %>" OnClick="btnLogout_Click" /></td>
</tr>
</table>

<asp:Label runat="server" ID="lblInfoLogin" />
<asp:HyperLink runat="server" ID="forgotPassword" NavigateUrl="forgot.aspx">
    <asp:Literal runat="server" ID="lblForgot" Text="<%$Resources:lblForgot %>" />
</asp:HyperLink>