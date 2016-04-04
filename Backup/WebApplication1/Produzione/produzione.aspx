<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="produzione.aspx.cs" 
 MasterPageFile="/Site.master" Inherits="WebApplication1.ProduzioneUI.produzioneUI" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:HyperLink runat="server" ID="lnkProcPadre" NavigateUrl="produzione.aspx">Torna al processo padre</asp:HyperLink>
<br />
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater ID="rptMacroProc" runat="server" OnItemCreated="rptMacroProc_ItemCreated">
<headertemplate>
<table border="1">
<tr>
<td></td>
<td></td>
<td></td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Macroprocesso</td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Note</td>
</tr>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td><a href="produzione.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgExpand" ImageUrl="/img/iconExpand.gif" height="20px" AlternateText="Processi di livello inferiore" /></a></td>
        <td><a href="productionPlan.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgView" ImageUrl="/img/iconProductionPlan.png" Height="40px" AlternateText="Manage Production Plan" /></a></td>
        <td><a href="taskUsers.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgTaskUsers" ImageUrl="/img/iconUser.png" height="40px" AlternateText="Link users to tasks and estimate the workload" /></a></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processDescription") %></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
</asp:Repeater>

</asp:Content>