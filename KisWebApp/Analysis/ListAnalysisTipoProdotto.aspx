<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListAnalysisTipoProdotto.aspx.cs" Inherits="KIS.Analysis.ListAnalysisTipoProdotto" %>
<%@ Register TagPrefix="TipoProdotto" TagName="List" Src="~/Analysis/ListAnalysisTipoProdotto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">
                            <asp:Label runat="server" ID="lblNavAnalisiDati" meta:resourcekey="lblNavAnalisiDati" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTipoProdotto.aspx">
                            <asp:Label runat="server" ID="lblNavSelProdotto" meta:resourcekey="lblNavSelProdotto" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <TipoProdotto:List runat="server" id="frmListTipiProdotto" />
</asp:Content>
