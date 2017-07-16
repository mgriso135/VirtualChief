<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configOrderStatusCustomer.aspx.cs" Inherits="KIS.Admin.configOrderStatusCustomer" %>
<%@ Register TagPrefix="config" TagName="OrderCustomer" Src="~/Admin/configOrderStatusCustomer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="../Admin/admin.aspx">
                            <asp:Label runat="server" ID="lblMenuAdmin" meta:resourcekey="lblMenuAdmin" /></a>
						<span class="divider">/</span>
						<a href="manageReports.aspx">
                            <asp:Label runat="server" ID="lblMenuConfigReports" meta:resourcekey="lblMenuConfigReports" /></a>
						<span class="divider">/</span>
                        <a href="configOrderStatusBase.aspx">
                            <asp:Label runat="server" ID="lblMenuReportStatoProdotti" meta:resourcekey="lblMenuReportStatoProdotti" /></a>
						<span class="divider">/</span>
                        <asp:hyperlink NavigateUrl="configOrderStatusCustomer.aspx" runat="server" id="lnkCurrentPage">
                            <asp:Label runat="server" ID="lblMenuReportStatoProdotti2" meta:resourcekey="lblMenuReportStatoProdotti" /></asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <config:OrderCustomer runat="server" id="frmConfigOrderCustomer" />
</asp:Content>
