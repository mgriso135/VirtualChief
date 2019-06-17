<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
CodeBehind="MacroProcessi.aspx.cs" Inherits="KIS.MacroProcessi"%>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="MacroProcessi.aspx"><asp:Literal runat="server" ID="lblNavProcessMan" Text="<%$Resources:lblNavProcessMan %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>		
<div class="row-fluid">
    <div class="span9">
<asp:label runat="server" ID="lbl1"/>
<asp:label runat="server" ID="lblErr" />
<a href="AddMacroProcesso.aspx"><asp:Literal runat="server" ID="lblAddLineaProd" Text="<%$Resources:lblAddLineaProd %>" /></a>
<asp:Repeater ID="rptMacroProc" runat="server" OnItemCreated="rptMacroProc_ItemCreated">
<headertemplate>
<table class="table table-hover table-striped table-condensed">
    <thead>
<tr>
<td></td>
<td><asp:Literal runat="server" ID="lblLineeProd" Text="<%$Resources:lblLineeProd %>" /></td>
<td><asp:Literal runat="server" ID="lblNote" Text="<%$Resources:lblNote %>" /></td>
</tr>
        </thead>
    <tbody>
        </headertemplate>
        <ItemTemplate>
        <tr runat="server" id="tr1">
        <td><a href="showProcesso.aspx?id=<%# DataBinder.Eval(Container.DataItem, "processID") %>"><asp:Image runat="server" id="imgView" ImageUrl="~/img/iconView.png" Width="30px" /></a></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processName") %></td>
        <td style="font-size: 12px; font-family: Calibri;"><%# DataBinder.Eval(Container.DataItem, "processDescription") %></td>
        </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table></FooterTemplate>
</asp:Repeater>
        </div>
    <div class="span3">
        <a href="../Products/ProductParametersCategories/Index">
            <img src="../img/iconCategory.png" style="min-width:20px; max-width:40px;" />
            <asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblProductParamCategories %>" />
        </a>
    </div>
    </div>
    </asp:Content>

