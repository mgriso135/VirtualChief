<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAssociaPERTReparto.aspx.cs" Inherits="KIS.Commesse.wzAssociaPERTReparto" %>

<%@ Register TagPrefix="PERT" TagName="associaReparto" Src="~/Commesse/wzAssociaProdottoReparto.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblWizNuovoOrd" meta:resourcekey="lblWizNuovoOrd" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNavNewOrdine" meta:resourcekey="lblNavNewOrdine" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert">
                <asp:Label runat="server" ID="lblNavDescrProd" meta:resourcekey="lblNavDescrProd" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server">
                <asp:Label runat="server" ID="lblNavDescrProcesso" meta:resourcekey="lblNavDescrProcesso" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblNavSelReparto" meta:resourcekey="lblNavSelReparto" /></b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>

    <div class="row-fluid">
        <div class="span9">
    <pert:associaReparto runat="server" id="frmAssociaPERTReparto" />
            </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
</asp:Content>
