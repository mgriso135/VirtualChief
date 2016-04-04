<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showProductionPlan.ascx.cs" Inherits="WebApplication1.Processi.showProductionPlan" %>

<asp:Label runat="server" ID="lbl" />

<asp:Repeater runat="server" ID="rptProdList" OnItemCreated="rptProdList_ItemCreated">
<HeaderTemplate>
<table style="border: 2px dashed green">
<tr style="font-size: 16px; font-weight: bold">
<td>Matricola</td>
<td>Status</td>
<td>Cadenza iniziale prevista</td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr runat="server" id="tr1" style="font-size: 14px;">
<td><%#DataBinder.Eval(Container.DataItem, "matricola") %></td>
<td><%#DataBinder.Eval(Container.DataItem, "status") %></td>
<td><%#DataBinder.Eval(Container.DataItem, "cadenzaIniziale") %></td>
</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>



<asp:Repeater runat="server" ID="rptProd">
<HeaderTemplate><table>
<tr><td>Process ID</td>
<td>Process Name</td>
<td>Early Start Time</td>
<td>Early Finish Time</td>
<td>Late Start Time</td>
<td>Late Finish Time</td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%# DataBinder.Eval(Container.DataItem, "processID") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "earlyStartTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "earlyFinishTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "lateStartTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "lateFinishTime") %></td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>

<asp:Repeater runat="server" ID="rptCritical">
<HeaderTemplate><table>
<tr><td colspan="6" style="font-size:14px; color:Blue; text-align:center; font-weight: bold;">PROCESSI APPARTENENTI AL CRITICAL PATH</td></tr>
<tr><td>Process ID</td>
<td>Process Name</td>
<td>Early Start Time</td>
<td>Early Finish Time</td>
<td>Late Start Time</td>
<td>Late Finish Time</td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%# DataBinder.Eval(Container.DataItem, "processID") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "earlyStartTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "earlyFinishTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "lateStartTime") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "lateFinishTime") %></td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>

</asp:Repeater>