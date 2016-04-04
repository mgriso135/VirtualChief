<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configLogo.ascx.cs" Inherits="KIS.Admin.configLogo" %>

        <h3>Logo aziendale</h3>
        <asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" />
<br />
<asp:Image runat="server" ID="imgCurrentLogo" Visible="false" ToolTip="Logo attualmente configurato" />
<asp:Label runat="server" ID="lbl1" />