<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzQuestionWorkLoad.aspx.cs" Inherits="KIS.Commesse.wzQuestionWorkLoad" %>
<%@ Register TagPrefix="wzQuestionWorkLoad" TagName="quest" Src="~/Commesse/wzQuestionWorkLoad.ascx" %>
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
		    <asp:hyperlink runat="server" id="lnkAddPert" Text="<%$Resources:lblDescProd %>"></asp:hyperlink>
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
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks" Text="<%$Resources:lblAssociaTask %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna" Text="<%$Resources:lblDataConsegnaRichiesta %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
            <asp:HyperLink runat="server" ID="lnkLancio">
		    <b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblLancioProd" Text="<%$Resources:lblLancioProd %>" /></b></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <asp:Label runat="server" ID="lbl1"/>
    <div class="row-fluid">
        <div class="span9">
    <wzQuestionWorkLoad:quest runat="server" ID="frmQuestionWorkLoad" />
            </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
</asp:Content>
