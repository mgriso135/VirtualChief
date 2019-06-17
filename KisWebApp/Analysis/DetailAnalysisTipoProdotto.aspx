<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailAnalysisTipoProdotto.aspx.cs" Inherits="KIS.Analysis.DetailAnalysisTipoProdotto" %>
<%@ Register TagPrefix="TipoProdotto" TagName="Analysis" Src="~/Analysis/DetailAnalysisTipoProdotto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:scriptmanager runat="server" id="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTipoProdotto.aspx">Selezione tipo di prodotto</a>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:HyperLink NavigateUrl="DetailAnalysisTipoProdotto.aspx" runat="server" id="lnkNavigation">
                            Analisi per tipo di prodotto</asp:HyperLink>
						<span class="divider">/</span>
					</li>
				</ul>
    <TipoProdotto:Analysis runat="server" ID="frmAnalisiTipoProdotto" />
</asp:Content>
