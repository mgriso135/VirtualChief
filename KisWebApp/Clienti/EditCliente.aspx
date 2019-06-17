<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCliente.aspx.cs" Inherits="KIS.Clienti.EditCliente" %>
<%@ Register TagPrefix="Clienti" TagName="Edit" Src="~/Clienti/EditCliente.ascx" %>
<%@ Register TagPrefix="Clienti" TagName="Contatti" Src="~/Clienti/ContattiClienti.ascx" %>
<%@ Register TagPrefix="Contatti" TagName="Add" Src="~/Clienti/addContattoCliente.ascx" %>

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
						<asp:hyperlink runat="server" ID="lnkCustomer" NavigateUrl="EditCliente.aspx">
                            <asp:Label runat="server" ID="lblNavModCliente" meta:resourcekey="lblNavModCliente" />
                            </asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span8">
            <h3><asp:Label runat="server" ID="lblTitleBaseInfo" meta:resourcekey="lblTitleBaseInfo" /></h3>
    <Clienti:Edit runat="server" ID="frmEditCliente" />
            </div>
        <div class="span4">
            <h3><asp:Label runat="server" ID="lblTitleContatti" meta:resourcekey="lblTitleContatti" /></h3>
            <Contatti:Add runat="server" id="frmAddContatto" />
            <br />
            <Clienti:Contatti runat="server" id="frmContattiClienti" />
        </div>
        </div>
</asp:Content>
