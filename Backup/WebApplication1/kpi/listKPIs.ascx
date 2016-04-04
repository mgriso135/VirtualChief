<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listKPIs.ascx.cs" Inherits="WebApplication1.kpi.listKPIs" %>


<asp:Repeater ID="rptKPIs" runat="server" OnItemCreated="rptKPIs_ItemCreated">
<headertemplate>
<table style="border: 1px dashed green">
</headertemplate>
<ItemTemplate>
<tr runat="server" id="tr1">
<td><a href="/kpi/viewKPI.aspx?kpiID=<%# DataBinder.Eval(Container.DataItem, "id") %>"><asp:image runat="server" src="/img/iconView.png" Height="40px" /></a></td>
<td style="font-size: 14px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>