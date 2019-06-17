<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCliente.aspx.cs" Inherits="KIS.Clienti.AddCliente" %>
<%@ Register TagPrefix="cliente" TagName="add" Src="~/Clienti/addCliente.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="Clienti.aspx">
                            <asp:Label runat="server" ID="lblNavAnagrafica" meta:resourcekey="lblNavAnagrafica" /></a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="#">
                            <asp:Label runat="server" ID="lblNavNewCustomer" meta:resourcekey="lblNavNewCustomer" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <cliente:add runat="server" ID="frmAddCliente" />
</asp:Content>
