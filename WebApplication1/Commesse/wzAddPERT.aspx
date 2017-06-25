<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAddPERT.aspx.cs" Inherits="KIS.Commesse.wzAddPERT" %>
<%@ Register TagPrefix="PERT" TagName="Add" Src="~/Commesse/wzAddPERT.ascx" %>
<%@ Register TagPrefix="Commessa" TagName="ListArticoliN" Src="~/Commesse/wzListArticoliNCommessa.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" EnablePageMethods="true"/>
    <h1><asp:Label runat="server" ID="lblTitleWizOrdine" meta:resourcekey="lblTitleWizOrdine" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx">
                <asp:Label runat="server" ID="lblNavNewOrder" meta:resourcekey="lblNavNewOrder" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink NavigateUrl="wzAddPERT.aspx" runat="server" id="lnkAddPert"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblNavDescrProduct" meta:resourcekey="lblNavDescrProduct" /></b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>

    <div class="row-fluid">
        <div class="span8">
        <pert:add runat="server" id="frmTipoAddProcesso" />
            </div>
        <div class="span2">
            <Commessa:ListArticoliN runat="server" ID="frmListArticoliN" />
        </div>
        <div class="span2">
            <Wizard:InfoPanel runat="server" ID="frmInfoPanel" />
        </div>
    </div>
    
</asp:Content>
