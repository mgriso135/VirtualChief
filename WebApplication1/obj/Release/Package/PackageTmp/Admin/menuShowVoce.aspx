<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="menuShowVoce.aspx.cs" Inherits="KIS.Admin.menuShowVoce" %>

<%@ Register TagPrefix="menu" TagName="showFigli" Src="~/Admin/menuShowFigli.ascx" %>
<%@ Register TagPrefix="menu" TagName="addFiglio" Src="~/Admin/menuAddVoceFiglia.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageMenu.aspx">Gestione menu</a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>">Gestione voce di secondo livello</a>
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
