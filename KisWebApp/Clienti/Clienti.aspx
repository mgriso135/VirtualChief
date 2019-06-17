<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clienti.aspx.cs" Inherits="KIS.Clienti.Clienti" %>
<%@ Register TagPrefix="clienti" TagName="list" Src="~/Clienti/listClienti.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="Clienti.aspx">
                            <asp:Label runat="server" ID="lblNavAnagClienti" meta:resourcekey="lblNavAnagClienti" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <a href="AddCliente.aspx"><asp:Label runat="server" ID="lblTitleAddCustomer" meta:resourcekey="lblTitleAddCustomer" /></a>
    <br />
    <clienti:list runat="server" ID="frmListClienti" />
</asp:Content>
