<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzCheckDeliveryDate.aspx.cs" Inherits="KIS.Commesse.wzCheckDeliveryDate" %>
<%@ Register TagPrefix="prodotto" TagName="setFineProduzione" Src="~/Commesse/wzCheckDeliveryDate.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleNuovoOrdine" Text="<%$Resources:lblTitleNuovoOrdine %>" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNavNuovoOrdine" Text="<%$Resources:lblNavNuovoOrdine %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert">
                <asp:Label runat="server" ID="lblDescProd" Text="<%$Resources:lblDescProd %>" />
                </asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server"><asp:Label runat="server" ID="lblDescProcesso" Text="<%$Resources:lblDescProcesso %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto"><asp:Label runat="server" ID="lblNavSelReparto" Text="<%$Resources:lblNavSelReparto %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks" Text="<%$Resources:lblNavAssociaTaskPostazione %>" />
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna" Text="<%$Resources:lblDataConsegna %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
            <asp:HyperLink runat="server" ID="lnkLancio" Text="<%$Resources:lblLancioProd %>">
		    </asp:HyperLink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:HyperLink runat="server" ID="lnkDataFineProd"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="Label1" Text="<%$Resources:lblDataFineProd %>" /></b></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <asp:Label runat="server" ID="lbl1"/>
    <div class="row-fluid">
        <div class="span9">
    <prodotto:setFineProduzione runat="server" ID="frmSetFineProduzione" />
            </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
</asp:Content>
