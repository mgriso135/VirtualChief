<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showProductionPlan.ascx.cs" Inherits="KIS.Processi.showProductionPlan" %>

<asp:Label runat="server" ID="lbl" />

<asp:Repeater runat="server" ID="rptProdList" OnItemCreated="rptProdList_ItemCreated">
<HeaderTemplate>
<table style="border: 2px dashed green">
<tr style="font-size: 16px; font-weight: bold">
<td><asp:Literal runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" /></td>
<td><asp:Literal runat="server" ID="lblStatus" Text="<%$Resources:lblStatus %>" /></td>
    <td><asp:Literal runat="server" ID="lblInizioPrev" Text="<%$Resources:lblInizioPrev %>" /></td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr runat="server" id="tr1" style="font-size: 14px;">
<td><%#DataBinder.Eval(Container.DataItem, "matricola") %></td>
<td><%#DataBinder.Eval(Container.DataItem, "status") %></td>
    <td><%#DataBinder.Eval(Container.DataItem, "dataIniziale") %></td>
</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>