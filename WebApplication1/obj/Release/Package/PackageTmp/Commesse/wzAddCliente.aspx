<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAddCliente.aspx.cs" Inherits="KIS.Commesse.wzAddCliente" %>
<%@ Register TagPrefix="cliente" TagName="add" Src="~/Commesse/wzAddCliente.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
		    <a href="wzAddCommessa.aspx">
                <asp:Label runat="server" ID="lblNavProdCommessa" meta:resourcekey="lblWizOrdine" /></a>
			<span class="divider">/</span>
	    </li>
                    <li>
						<a href="<%=Request.RawUrl %>"><b><asp:Label runat="server" ID="lblNavNuovoCliente" meta:resourcekey="lblNavNuovoCliente" /></b></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <cliente:add runat="server" ID="frmAddCliente" />
    
</asp:Content>
