<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="/Site.master" AutoEventWireup="true" 
CodeBehind="MacroProcessi.aspx.cs" Inherits="KIS.MacroProcessi"%>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx">Process Manager</a>
						<span class="divider">/</span>
					</li>
				</ul>
<div class="page-header">Process Manager</div>				
<p class="lead">
<asp:label runat="server" ID="lbl1"/>
<asp:label runat="server" ID="lblErr" />
<a href="AddMacroProcesso.aspx">Aggiungi una linea di prodotti</a>
<asp:Repeater ID="rptMacroProc" runat="server" OnItemCreated="rptMacroProc_ItemCreated">
<headertemplate>
<table class="table table-hover table-striped table-condensed">
    <thead>
<tr>
<td></td>
<td>Linee di prodotti</td>
<td>Note</td>
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td><a href="showProcesso.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgView" ImageUrl="/img/iconView.png" Width="30px" AlternateText="View Value Stream" /></a></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processDescription") %></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table></FooterTemplate>
</asp:Repeater>
</p>
    </asp:Content>

