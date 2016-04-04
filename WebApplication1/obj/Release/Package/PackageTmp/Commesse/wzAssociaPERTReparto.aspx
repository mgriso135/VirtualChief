<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAssociaPERTReparto.aspx.cs" Inherits="KIS.Commesse.wzAssociaPERTReparto" %>

<%@ Register TagPrefix="PERT" TagName="associaReparto" Src="~/Commesse/wzAssociaProdottoReparto.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Wizard: nuovo prodotto su commessa</h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx">Nuova commessa</a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert">Descrivi il prodotto</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server">Descrivi il processo produttivo</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto"><b style="font-size: 14px;">Seleziona il reparto produttivo</b></asp:hyperlink>
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
