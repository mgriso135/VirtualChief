<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAddCommessa.aspx.cs" Inherits="KIS.Commesse.wzAddCommessa" %>
<%@ Register TagPrefix="wizard" TagName="addCommessa" Src="~/Commesse/wzAddCommessa.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleWizNuovoOrdine" meta:resourcekey="lblTitleWizNuovoOrdine" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblNuovoOrdine" meta:resourcekey="lblNuovoOrdine" /></b></a>
			<span class="divider">/</span>
	    </li>
	</ul>
    <wizard:addCommessa runat="server" ID="frmAddCommessa" />
</asp:Content>
