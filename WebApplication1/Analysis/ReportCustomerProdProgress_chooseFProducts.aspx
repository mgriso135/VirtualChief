<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseFProducts.aspx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseFProducts" %>
<%@ Register TagPrefix="report" TagName="prodProgressF" Src="~/Analysis/ReportCustomerProdProgress_chooseFProducts.ascx" %>
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
						<asp:hyperlink NavigateUrl="ReportCustomerProdProgress_chooseINPProducts.aspx" runat="server" id="lnkProdReportINP">Seleziona i prodotti da inserire nel report</asp:hyperlink>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:hyperlink NavigateUrl="ReportCustomerProdProgress_chooseFProducts.aspx" runat="server" id="lnkProdReportF">Seleziona i prodotti finiti da inserire nel report</asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <report:prodProgressF runat="server" id="frmChooseProductF" />
</asp:Content>
