<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="manageMenu.aspx.cs" Inherits="KIS.Admin.manageMenu" %>
<%@ Register TagPrefix="menu" TagName="addMain" Src="~/Admin/menuAddMainVoce.ascx" %>
<%@ Register TagPrefix="menu" TagName="listMain" Src="~/Admin/menuMainVociList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageMenu.aspx">Gestione menu</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <menu:addMain runat="server" id="frmAddMainVoce" />
    <menu:listMain runat="server" id="frmListMainVoce" />
</asp:Content>
