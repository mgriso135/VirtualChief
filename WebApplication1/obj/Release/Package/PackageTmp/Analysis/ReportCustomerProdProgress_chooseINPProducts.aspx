<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseINPProducts.aspx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseINPProducts" %>
<%@ Register TagPrefix="configReport" TagName="chooseProdottiINP" Src="~/Analysis/ReportCustomerProdProgress_chooseINPProducts.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseCustomer.aspx">Report avanzamento prodotti per cliente</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseINPProducts.aspx">Seleziona i prodotti da inserire nel report</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <configReport:chooseProdottiINP runat="server" id="frmChooseProductsINP" />
</asp:Content>
