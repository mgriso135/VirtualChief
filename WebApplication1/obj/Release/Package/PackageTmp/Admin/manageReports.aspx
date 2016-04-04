<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="manageReports.aspx.cs" Inherits="KIS.Admin.manageReports" %>
<%@ Register TagPrefix="config" TagName="Reports" Src="~/Admin/manageReports.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageReports.aspx">Configurazione reports</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <config:Reports runat="server" ID="frmConfigReports" />
</asp:Content>
