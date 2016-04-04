<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListAnalysisTipoProdotto.aspx.cs" Inherits="KIS.Analysis.ListAnalysisTipoProdotto" %>
<%@ Register TagPrefix="TipoProdotto" TagName="List" Src="~/Analysis/ListAnalysisTipoProdotto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTipoProdotto.aspx">Selezione tipo di prodotto</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <TipoProdotto:List runat="server" id="frmListTipiProdotto" />
</asp:Content>
