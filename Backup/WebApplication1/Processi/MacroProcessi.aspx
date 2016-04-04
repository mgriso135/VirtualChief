<%@ Page Title="Process Monitor app" Language="C#" MasterPageFile="/Site.master" AutoEventWireup="true" 
CodeBehind="MacroProcessi.aspx.cs" Inherits="WebApplication1.MacroProcessi"%>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:label runat="server" ID="lblErr" /><br />
<a href="AddMacroProcesso.aspx">Aggiungi un macroprocesso</a>
<asp:Repeater ID="rptMacroProc" runat="server" OnItemCreated="rptMacroProc_ItemCreated">
<headertemplate>
<table border="1">
<tr>
<td></td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Macroprocesso</td>
<td align="center" style="font-size: 18px; font-family: Calibri; font-weight:bold">Note</td>
</tr>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td><a href="showProcesso.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgView" ImageUrl="/img/iconView.png" Width="30px" AlternateText="View Value Stream" /></a></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processDescription") %></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
</asp:Repeater>

    </asp:Content>

