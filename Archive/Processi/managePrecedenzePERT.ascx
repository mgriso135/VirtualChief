<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="managePrecedenzePERT.ascx.cs"
 Inherits="KIS.Processi.managePrecedenzePERT"%>

 <asp:Label runat="server" ID="lblCheck" />

<table class="table table-striped table-hover" runat="server" id="tblPrecedenze">
    <thead>
<tr style="font-size: 14px; font-weight: bold;">
<td style="border-right: 1px dashed blue; text-align:center">
    <asp:Literal runat="server" ID="lblPrecedenti" Text="<%$Resources:lblPrecedenti %>" />
    </td>
<td style="text-align:center;"><asp:Literal runat="server" ID="lblTaskCorrente" Text="<%$Resources:lblTaskCorrente %>" /></td>
<td style="border-left: 1px dashed blue; text-align:center"><asp:Literal runat="server" ID="lblSuccessivi" Text="<%$Resources:lblSuccessivi %>" /></td>
</tr>
        </thead>
    
    <tbody>
<tr>
<td style="border-right: 1px dashed blue; text-align:center">

<asp:Repeater runat="server" ID="rptPrec">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "NearTaskID") %>&verso=prec&act=delprecedenze&variante=<% = Request.QueryString["variante"] %>">
<asp:Image runat="server" ImageUrl="~/img/iconDelete.png" style="min-width:30px; max-width:30px;" /></a>
<%#DataBinder.Eval(Container.DataItem, "NearTaskName") %>&nbsp;(<%#DataBinder.Eval(Container.DataItem, "ConstraintTypeDesc") %>)<br />
</ItemTemplate>
</asp:Repeater>
</td>
<td style="text-align:center; font-size: 12px; font-weight:bold;" runat="server" id="curr"></td>
<td style="border-left: 1px dashed blue; text-align:center">
<asp:Repeater runat="server" ID="rptSucc">
<HeaderTemplate></HeaderTemplate>
<ItemTemplate>
<a href="updatePERT.aspx?id=<% = Request.QueryString["id"] %>&delID=<%# DataBinder.Eval(Container.DataItem, "NearTaskID") %>&verso=succ&act=delprecedenze&variante=<% = Request.QueryString["variante"] %>">
<asp:Image runat="server" ImageUrl="~/img/iconDelete.png" style="min-width:30px; max-width:30px;" /></a>
<%#DataBinder.Eval(Container.DataItem, "NearTaskName") %>&nbsp;(<%#DataBinder.Eval(Container.DataItem, "ConstraintTypeDesc") %>)<br />
</ItemTemplate>
</asp:Repeater>
    
</td>
</tr>
        </tbody>
    <tfoot>
<tr>
<td style="border-right: 1px dashed blue; text-align:center">
<asp:Literal runat="server" ID="litConstraintType" Text="<%$Resources:litConstraintType %>" /><asp:DropDownList runat="server" ID="newPrecConstraint">
        <asp:ListItem Value="0" Text="<%$Resources:lblConstraintType0 %>" />
        <asp:ListItem Value="1" Text="<%$Resources:lblConstraintType1 %>"></asp:ListItem>
    </asp:DropDownList><br />
<asp:Literal runat="server" ID="litTaskPrec"  Text="<%$Resources:litTaskPrec %>" /><asp:DropDownList runat="server" id="newPrec" OnSelectedIndexChanged="addPrecedenti_IndexChanged" AutoPostBack="true" />
</td>
<td></td>
<td style="border-left: 1px dashed blue; text-align:center">
    <asp:Literal runat="server" ID="litConstraintType2" Text="<%$Resources:litConstraintType %>" /><asp:DropDownList runat="server" ID="newSuccConstraint">
        <asp:ListItem Value="0" Text="<%$Resources:lblConstraintType0 %>" />
        <asp:ListItem Value="1" Text="<%$Resources:lblConstraintType1 %>"></asp:ListItem>
    </asp:DropDownList><br />
    <asp:Literal runat="server" ID="litTaskSucc"  Text="<%$Resources:litTaskSucc %>" /><asp:DropDownList runat="server" id="newSucc" OnSelectedIndexChanged="addSuccessivi_IndexChanged" AutoPostBack="true" /></td>
</tr>
        </tfoot>
</table>
