<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="loginbox.ascx.cs" Inherits="KIS.Login.loginbox" %>

<table id="tblLogin" border="0" runat="server" class="table table-bordered table-hover">
<tr><td colspan="2" style="font-size: 14px; font-weight: bold">Login Box</td></tr>
<tr>
<td>Username:</td>
<td><asp:textbox runat="server" id="usr"/>
<asp:RequiredFieldValidator runat="server" ControlToValidate="usr" ErrorMessage="Errore: campo obbligatorio" ForeColor="Red" id="valName" />
</td>
</tr>
<tr>
<td>Password:</td>
<td>
<asp:textbox runat="server" id="pwd" TextMode="Password" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="pwd" ErrorMessage="Errore: campo obbligatorio" ForeColor="Red" id="valPass" />
</td>
</tr>
<tr><td colspan="2"><asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Login" CausesValidation="true" /></td></tr>
</table>

<table border="0" runat="server" id="tblLogout">
<tr>
<td><asp:Button runat="server" Text="Logout" OnClick="btnLogout_Click" /></td>
</tr>
</table>

<asp:Label runat="server" ID="lblInfoLogin" />
<asp:HyperLink runat="server" ID="forgotPassword" NavigateUrl="forgot.aspx">
    Hai dimenticato i dati di accesso?
</asp:HyperLink>