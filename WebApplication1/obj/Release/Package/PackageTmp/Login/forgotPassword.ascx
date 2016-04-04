<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="forgotPassword.ascx.cs" Inherits="KIS.Login.forgotPassword" %>

<p class="text-info">Attenzione: per motivi di sicurezza la password verrà resettata e spedita all'indirizzo e-mail predefinito dell'utente indicato.</p>
Username:&nbsp;<asp:TextBox runat="server" ID="txtUsername" ValidationGroup="usern" />
<asp:Button runat="server" ID="btnUsername" Text="Reset password" CssClass="btn" OnClick="btnUsername_Click" ValidationGroup="usern" />
<asp:RequiredFieldValidator runat="server" ID="val1" ValidationGroup="usern" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ControlToValidate="txtUsername" />
<br />
<asp:Label runat="server" ID="lbl1" CssClass="text-info" />