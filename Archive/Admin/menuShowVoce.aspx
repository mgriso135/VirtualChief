<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="menuShowVoce.aspx.cs" Inherits="KIS.Admin.menuShowVoce" %>

<%@ Register TagPrefix="menu" TagName="showFigli" Src="~/Admin/menuShowFigli.ascx" %>
<%@ Register TagPrefix="menu" TagName="addFiglio" Src="~/Admin/menuAddVoceFiglia.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="../Admin/admin.aspx"><asp:label runat="server" id="lblNavAdmin" meta:resourcekey="lblNavAdmin" /></a>
						<span class="divider">/</span>
						<a href="manageMenu.aspx"><asp:label runat="server" id="lblGestioneMenu" meta:resourcekey="lblGestioneMenu" /></a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>"><asp:label runat="server" id="lblNavGestioneVoce" meta:resourcekey="lblNavGestioneVoce" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3><asp:Label runat="server" ID="lblTitolo" /></h3>
    <h5><asp:Label runat="server" ID="lblDescrizione" /></h5>
    URL:&nbsp;<asp:Label runat="server" ID="lblURL" />
    <br />
    <br />
    <menu:addFiglio runat="server" id="frmAddFiglio" />
    <menu:showFigli runat="server" id="frmShowFigli" />
</asp:Content>
