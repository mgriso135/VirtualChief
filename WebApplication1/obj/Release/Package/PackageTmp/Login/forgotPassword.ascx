<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="forgotPassword.ascx.cs" Inherits="KIS.Login.forgotPassword" %>

<p class="text-info"><asp:Literal runat="server" ID="lblDisclaimer" Text="<%$Resources:lblDisclaimer %>" /></p>
<asp:Literal runat="server" ID="lblUsername" Text="<%$Resources:lblUsername %>" />:&nbsp;<asp:TextBox runat="server" ID="txtUsername" ValidationGroup="usern" />
<asp:Button runat="server" ID="btnUsername" Text="Reset password" CssClass="btn" OnClick="btnUsername_Click" ValidationGroup="usern" />
<asp:RequiredFieldValidator runat="server" ID="val1" ValidationGroup="usern" ForeColor="Red" ErrorMessage="<%$Resources:lblValReqField %>" ControlToValidate="txtUsername" />
<br />
<asp:Label runat="server" ID="lbl1" CssClass="text-info" />