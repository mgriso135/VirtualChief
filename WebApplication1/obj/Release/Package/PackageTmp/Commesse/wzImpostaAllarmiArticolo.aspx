<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzImpostaAllarmiArticolo.aspx.cs" Inherits="KIS.Commesse.wzImpostaAllarmiArticolo" %>
<%@ Register TagPrefix="articolo" TagName="allarmeRitardo" Src="~/Eventi/ArticoloRitardo.ascx" %>
<%@ Register TagPrefix="articolo" TagName="allarmeWarning" Src="~/Eventi/ArticoloWarning.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="PrintBarcodes" Src="~/Commesse/wzPrintBarcordes.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleWizNewOrder" Text="<%$Resources:lblTitleWizNewOrder %>" /></h1>
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
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks" Text="<%$Resources:lblLinkTasksPostazioni %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkDataConsegna" Text="<%$Resources:lblDataConsegna %>"></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
            <asp:HyperLink runat="server" ID="lnkLancio" Text="<%$Resources:lblLancioInProduzione %>"></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:HyperLink runat="server" ID="lnkImpostaAllarmi"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblImpostaAllarmi" Text="<%$Resources:lblImpostaAllarmi %>" />
                </b></asp:HyperLink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <div class="row-fluid">
        <div class="span9">
    <asp:Image runat="server" ID="imgTick" ImageUrl="~/img/iconComplete.png" Height="40" />
            <asp:Label runat="server" ID="Label1" Text="<%$Resources:lblLancioOk %>" />
            </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
    <div class="row-fluid">
        <div class="span9">
            <Wizard:PrintBarcodes runat="server" id="frmPrintBarcodes" />
            </div>
        </div>
    <asp:imagebutton OnClick="lnkShowTabAlarms_Click" runat="server" id="lnkShowTabAlarms" ImageUrl="~/img/iconAdd2.png" Height="40"/>
    <asp:Label runat="server" ID="lblImpostaAllarmi2" Text="<%$Resources:lblImpostaAllarmi %>" />
     <table runat="server" id="tblAlarms">
        <tr>
            <td><articolo:allarmeRitardo runat="server" id="frmRitardo" /></td>
            <td>
                <articolo:allarmeWarning runat="server" id="frmWarning" />
            </td>
        </tr>
    </table>
    <br />
    <asp:HyperLink runat="server" ID="lnkNewProd">
    <asp:Image runat="server" ID="imgNuovoArticolo" ImageUrl="~/img/iconProductionPlan.png" Height="40" />
        <asp:Label runat="server" ID="lblAddProdotto" Text="<%$Resources:lblAddProdotto %>" />
        </asp:HyperLink>
</asp:Content>
