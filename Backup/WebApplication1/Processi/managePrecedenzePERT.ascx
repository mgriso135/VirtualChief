<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="managePrecedenzePERT.ascx.cs"
 Inherits="WebApplication1.Processi.managePrecedenzePERT"%>

 <asp:Label runat="server" ID="lblCheck" />



<table>
<tr style="font-size: 14px; font-weight: bold;">
<td>Precedenti</td>
<td></td>
<td>Successivi</td>
</tr>


<tr>
<td>
<asp:Repeater runat="server" ID="rptPrec">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "processID") %>&verso=prec&act=delprecedenze">
<img src="/img/iconDelete.png" height="30" /></a>
<%#DataBinder.Eval(Container.DataItem, "processName") %><br />
</ItemTemplate>
</asp:Repeater>
</td>
<td style="text-align:center; font-size: 12px; font-weight:bold;" runat="server" id="curr"></td>
<td>
<asp:Repeater runat="server" ID="rptSucc">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "processID") %>&verso=succ&act=delprecedenze">
<img src="/img/iconDelete.png" height="30" /></a>
<%#DataBinder.Eval(Container.DataItem, "processName") %><br />
</ItemTemplate>
</asp:Repeater>

</td>
</tr>
<tr>
<td>
<asp:DropDownList runat="server" id="newPrec" OnSelectedIndexChanged="addPrecedenti_IndexChanged" AutoPostBack="true" />
</td>
<td></td>
<td><asp:DropDownList runat="server" id="newSucc" OnSelectedIndexChanged="addSuccessivi_IndexChanged" AutoPostBack="true" /></td>
</tr>
</table>
</asp:Repeater>
