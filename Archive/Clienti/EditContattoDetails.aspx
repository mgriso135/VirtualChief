<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditContattoDetails.aspx.cs" Inherits="KIS.Clienti.EditContattoDetails" %>
<%@Register TagPrefix="Clienti" TagName="EditContatto" Src="~/Clienti/EditContattoDetails.ascx" %>
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
                    <li>
						<asp:hyperlink runat="server" ID="lnkModCustomer" NavigateUrl="EditCliente.aspx">
                            <asp:Label runat="server" ID="lblNavModCliente" meta:resourcekey="lblNavModCliente" />
                            </asp:hyperlink>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:hyperlink runat="server" ID="lnkContattoDetail" NavigateUrl="EditContattoDetails.aspx">
                            <asp:Label runat="server" ID="lblNavDettaglioCliente" meta:resourcekey="lblNavDettaglioCliente" />
                            </asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <Clienti:EditContatto runat="server" ID="frmEditContatto" />
</asp:Content>
