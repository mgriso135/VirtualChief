<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzInserisciDataConsegna.aspx.cs" Inherits="KIS.Commesse.wzInserisciDataConsegna" %>

<%@ Register TagPrefix="prodotto" TagName="assegnaData" Src="~/Commesse/wzInserisciDataConsegna.ascx" %>
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
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto">Seleziona il reparto produttivo</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks">Associa i tasks alle postazioni</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna"><b style="font-size: 14px;">Data di consegna richiesta</b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <div class="row-fluid">
        <div class="span9">
    <prodotto:assegnaData runat="server" ID="frmAssegnaDataConsegna" />
            </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
</asp:Content>
