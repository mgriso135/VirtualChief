<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listUsers.ascx.cs" Inherits="WebApplication1.Admin.listUsers" %>

<asp:Label runat="server" ID="lblLstUsers" />
<asp:Repeater ID="rptUsers" runat="server" OnItemCreated="rptUsers_ItemCreated">
<headertemplate>
<table border="1">
<tr>
<td></td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Username</td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Nome</td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Cognome</td>
</tr>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td><a href="managePermessi.aspx?userID=<%# DataBinder.Eval(Container.DataItem, "username") %>"><img src="/img/iconPermessi.gif" height="30" /></a></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "cognome") %></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
</asp:Repeater>