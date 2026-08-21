<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wizConfigLogo.ascx.cs" Inherits="KIS.Configuration.wizConfigLogo1" %>
 <h3><asp:Literal runat="server" ID="lblTitleLogo" Text="<%$Resources:lblTitleLogo %>" /></h3>
        <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" />
<br />
<asp:Image runat="server" ID="imgCurrentLogo" Visible="false" ToolTip="<%$Resources:lblTTLogoConfigured %>" />
<asp:Label runat="server" ID="lbl1" CssClass="text-info" />