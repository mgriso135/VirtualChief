<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseINPProducts.aspx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseINPProducts" %>
<%@ Register TagPrefix="configReport" TagName="chooseProdottiINP" Src="~/Analysis/ReportCustomerProdProgress_chooseINPProducts.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">
                            <asp:label runat="server" ID="lblNavAnalisiDati" meta:resourcekey="lblNavAnalisiDati" />
                            </a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseCustomer.aspx">
                            <asp:label runat="server" ID="lblNavProdClienti" meta:resourcekey="lblNavProdClienti" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseINPProducts.aspx">
                            <asp:label runat="server" ID="lblNavSelProdReport" meta:resourcekey="lblNavSelProdReport" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <configReport:chooseProdottiINP runat="server" id="frmChooseProductsINP" />
</asp:Content>
