<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="forgotUsername.ascx.cs" Inherits="KIS.Login.forgotUsername" %>

Indirizzo e-mail:&nbsp;<asp:TextBox runat="server" ID="txtEmail" ValidationGroup="usern" />
<asp:Button runat="server" ID="btnUsername" Text="Inviami lo username" CssClass="btn" OnClick="btnUsername_Click" ValidationGroup="usern" />
<asp:RequiredFieldValidator runat="server" ID="val1" ValidationGroup="usern" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ControlToValidate="txtEmail" />
<br />
<asp:Label runat="server" ID="lbl1" CssClass="text-info" />