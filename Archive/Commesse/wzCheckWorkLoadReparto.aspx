<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzCheckWorkLoadReparto.aspx.cs" Inherits="KIS.Commesse.wzCheckWorkLoadReparto" %>
<%@ Register TagPrefix="Reparto" TagName="CheckWorkLoadR" Src="~/Commesse/wzCheckWorkLoadReparto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleWizNewOrder" Text="<%$Resources:lblTitleWizNewOrder %>" /></h1>
    <ul class="breadcrumb hidden-phone">
         <li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNavNuovaCommessa" Text="<%$Resources:lblNavNuovaCommessa %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert"><asp:Label runat="server" ID="lblNavDescProd" Text="<%$Resources:lblNavDescProd %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
       <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server"><asp:Label runat="server" ID="lblNavDescProcesso" Text="<%$Resources:lblNavDescProcesso %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto"><asp:Label runat="server" ID="lblNavSelReparto" Text="<%$Resources:lblNavSelReparto %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks" Text="<%$Resources:lblNavAssociaTasksPostazioni %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna" Text="<%$Resources:lblNavDataConsegna %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
            <asp:HyperLink runat="server" ID="lnkLancio" Text="<%$Resources:lblLancioProduzione %>"></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:HyperLink runat="server" ID="lnkWorkLoad"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblControllaCaricoLav" Text="<%$Resources:lblControllaCaricoLav %>" />
                </b></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <asp:Label runat="server" ID="lbl1"/>
    <Reparto:CheckWorkLoadR runat="server" ID="frmCheckWorkLoad" />
</asp:Content>
