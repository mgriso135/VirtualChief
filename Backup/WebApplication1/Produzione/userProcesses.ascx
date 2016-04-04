<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="userProcesses.ascx.cs" Inherits="WebApplication1.Produzione.userProcesses" %>

<asp:Label runat="server" ID="elencoProc" />

<asp:Repeater runat="server" ID="rptOwnedProcesses">
<HeaderTemplate>
<table>
<tr>
<td><td></td></td>
</tr>
</HeaderTemplate>
<ItemTemplate>
<tr>
<td><%# DataBinder.Eval(Container.DataItem, "processID") %></td>
<td><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
<td><a href="/Produzione/startCadenza.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>" target="_NEW_<%# DataBinder.Eval(Container.DataItem, "processID") %>">Start</a></td>
</tr>
</ItemTemplate>
<FooterTemplate></table></FooterTemplate>
</asp:Repeater>