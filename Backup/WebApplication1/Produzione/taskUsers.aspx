<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taskUsers.aspx.cs"
 MasterPageFile="/Site.master" Inherits="WebApplication1.Produzione.taskUsers" %>

 <%@ Register TagPrefix="workload" TagName="show" Src="userWorkLoad.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<h1>Assegnazione task agli utenti</h1>
<h2 runat="server" id="procTitle" />
<asp:Label runat="server" ID="lbl" />
<asp:Repeater ID="rptProcUsers" runat="server" OnItemDataBound="rptProcUsers_ItemDataBound">
<HeaderTemplate>
<table>
<tr><td>Process</td><td>Process Owner</td></tr>
</HeaderTemplate>
<ItemTemplate>
<tr runat="server" id="tr1">
<td><%# DataBinder.Eval(Container.DataItem, "processName")%></td>
<td>
<asp:HiddenField runat="server" ID="procID" />
<asp:Label runat="server" ID="numPrc" />
<asp:DropDownList runat="server" ID="ddlUsers" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="userTask_changed">

</asp:DropDownList>
</td>
</tr>
</ItemTemplate>
<FooterTemplate>
</table>
</FooterTemplate>
</asp:Repeater>


<workload:show runat="server" id="userWorkLoadGraph" />
</asp:Content>