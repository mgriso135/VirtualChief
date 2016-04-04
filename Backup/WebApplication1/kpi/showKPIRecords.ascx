<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showKPIRecords.ascx.cs" Inherits="WebApplication1.kpi.showKPIRecords" %>

<asp:Label runat="server" ID="lblKpiID" />

<asp:Label runat=server ID="lblMsg" />
<table>
<tr>
<td>
<asp:Label runat="server" ID="dataIniziale" />
<br />
<asp:LinkButton runat="server" ID="lblStartDate" onClick="lblStartDate_Click">Start date<br /></asp:LinkButton>
<asp:Calendar runat="server" ID="startDate" OnSelectionChanged="startDate_Click" Visible="false" />
</td>
<td>
<asp:Label runat="server" ID="dataFinale" />
<br />
<asp:LinkButton runat="server" ID="lblEndDate" onClick="lblEndDate_Click">End date<br /></asp:LinkButton>
<asp:Calendar runat="server" ID="endDate" Visible="false" OnSelectionChanged="endDate_Click"  />
</td>
</tr>
</table>

<asp:chart ID="Chart1" runat="server" width="1000px" onload="Chart1_Load">
<asp:series runat="server" />
<ChartAreas>
    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
  </ChartAreas>
</asp:chart>

<asp:Repeater ID="rptKPIRecs" runat="server" OnItemCreated="rptKPIRecs_ItemCreated" Visible="false">
<headertemplate>
<table style="border: 1px dashed green">
<tr>
<td style="text-align:center; font-size: 14px; font-weight:bold;">Date</td>
<td style="text-align:center; font-size: 14px; font-weight:bold;">Recorded Value</td>
</tr>
</headertemplate>
<ItemTemplate>
<tr runat="server" id="tr1">
<td><%# DataBinder.Eval(Container.DataItem, "date") %></td>
<td style="font-size: 14px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "valore") %></td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>