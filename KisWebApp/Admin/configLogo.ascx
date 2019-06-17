<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configLogo.ascx.cs" Inherits="KIS.Admin.configLogo" %>

        <h3><asp:label runat="server" id="lblLogo" meta:resourcekey="lblLogo" /></h3>
        <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" />
<br />
<asp:Image runat="server" ID="imgCurrentLogo" Height="200" Visible="false" ToolTip="<%$resources:lblCurrentLogo %>" />
<asp:Label runat="server" ID="lbl1" />