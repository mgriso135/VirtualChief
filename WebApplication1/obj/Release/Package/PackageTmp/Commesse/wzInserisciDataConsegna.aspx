<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzInserisciDataConsegna.aspx.cs" Inherits="KIS.Commesse.wzInserisciDataConsegna" %>

<%@ Register TagPrefix="prodotto" TagName="assegnaData" Src="~/Commesse/wzInserisciDataConsegna.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblWizNewOrder" Text="<%$Resources:lblWizNewOrder %>" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNewOrder" Text="<%$Resources:lblNewOrder %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert"><asp:Label runat="server" ID="lblDescProd" Text="<%$Resources:lblDescProd %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server" Text="<%$Resources:lblDescProcesso %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto" Text="<%$Resources:lblSelReparto %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks" Text="<%$Resources:lblAssociaTaskPostazioni %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblDataConsegna" Text="<%$Resources:lblDataConsegna %>" /></b></asp:hyperlink>
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
