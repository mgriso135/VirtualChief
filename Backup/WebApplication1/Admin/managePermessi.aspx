<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
 CodeBehind="managePermessi.aspx.cs" Inherits="WebApplication1.Admin.managePermessi" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:Label runat="server" ID="lbl1" />
<a href="~/Login/login.aspx" runat="server" id="lnkLogin">You need to login first</a>
<br />
<asp:Label runat="server" ID="lblTitolo" runat="server" style="font-size:24px; font-weight:bold;">Gestore permessi</asp:Label><br /><br />
<asp:label runat="server" ID="nomeUtente" style="font-size: 20px; font-weight:bold;"/><br /><br />
<br />
<a href="~/Admin/managePermessi.aspx" runat="server" ID="lnkGoTop">Vai al processo di livello superiore<br /><br /></a>
<asp:Repeater runat="server" ID="rptPermessiProcessi" OnItemDataBound="rptPermessiProcessi_ItemDataBound">
<HeaderTemplate>
<table>
<tr>
<td style="text-align:center; font-size: 14px; font-weight: bold;" colspan="2">Processo</td>
<td style="text-align:center; font-size: 14px; font-weight:bold;">Permesso</td>
</tr>
</HeaderTemplate>
<ItemTemplate>

<tr runat="server" id="tr1">
<td><asp:hyperlink runat="server" id="lnkSons" /></td>
<td style="font-size: 12px;"><%# DataBinder.Eval(Container.DataItem, "processID") %></td>
<td style="font-size: 12px;"><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
<td>
<asp:HiddenField runat="server" ID="procID" />
<asp:DropDownList runat="server" ID="ddlPermessi" OnSelectedIndexChanged="imgSavePermessi_Click" AutoPostBack="true">
<asp:ListItem Text="Null" Value="0" />
<asp:ListItem Text="Read" Value="r" />
<asp:ListItem Text="Write" Value="w" />
<asp:ListItem Text="Manage" Value="m" />
</asp:DropDownList>
</td>
</tr>

</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>


</asp:Content>