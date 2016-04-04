<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configOrderStatusCustomer.aspx.cs" Inherits="KIS.Admin.configOrderStatusCustomer" %>
<%@ Register TagPrefix="config" TagName="OrderCustomer" Src="~/Admin/configOrderStatusCustomer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageReports.aspx">Configurazione reports</a>
						<span class="divider">/</span>
                        <a href="configOrderStatusBase.aspx">Report stato avanzamento prodotti per cliente</a>
						<span class="divider">/</span>
                        <asp:hyperlink NavigateUrl="configOrderStatusCustomer.aspx" runat="server" id="lnkCurrentPage">Report stato avanzamento prodotti per cliente</asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <config:OrderCustomer runat="server" id="frmConfigOrderCustomer" />
</asp:Content>
