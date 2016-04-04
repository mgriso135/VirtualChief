<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="managePrecedenzePERT.ascx.cs"
 Inherits="KIS.Processi.managePrecedenzePERT"%>

 <asp:Label runat="server" ID="lblCheck" />

<table class="table table-striped table-hover" runat="server" id="tblPrecedenze">
    <thead>
<tr style="font-size: 14px; font-weight: bold;">
<td style="border-right: 1px dashed blue; text-align:center">Precedenti</td>
<td style="text-align:center;">Attività corrente</td>
<td style="border-left: 1px dashed blue; text-align:center">Successivi</td>
</tr>
        </thead>
    
    <tbody>
<tr>
<td style="border-right: 1px dashed blue; text-align:center">

<asp:Repeater runat="server" ID="rptPrec">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "processID") %>&verso=prec&act=delprecedenze&variante=<% = Request.QueryString["variante"] %>">
<asp:Image runat="server" ImageUrl="/img/iconDelete.png" height="30" /></a>
<%#DataBinder.Eval(Container.DataItem, "processName") %><br />
</ItemTemplate>
</asp:Repeater>
</td>
<td style="text-align:center; font-size: 12px; font-weight:bold;" runat="server" id="curr"></td>
<td style="border-left: 1px dashed blue; text-align:center">
<asp:Repeater runat="server" ID="rptSucc">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "processID") %>&verso=succ&act=delprecedenze&variante=<% = Request.QueryString["variante"] %>">
<asp:Image runat="server" ImageUrl="/img/iconDelete.png" height="30" /></a>
<%#DataBinder.Eval(Container.DataItem, "processName") %><br />
</ItemTemplate>
</asp:Repeater>
    
</td>
</tr>
        </tbody>
    <tfoot>
<tr>
<td style="border-right: 1px dashed blue; text-align:center">
<asp:DropDownList runat="server" id="newPrec" OnSelectedIndexChanged="addPrecedenti_IndexChanged" AutoPostBack="true" />
</td>
<td></td>
<td style="border-left: 1px dashed blue; text-align:center"><asp:DropDownList runat="server" id="newSucc" OnSelectedIndexChanged="addSuccessivi_IndexChanged" AutoPostBack="true" /></td>
</tr>
        </tfoot>
</table>
