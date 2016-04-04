<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailTaskProduct.aspx.cs" Inherits="KIS.Analysis.DetailTaskProduct" %>
<%@ Register TagPrefix="AnalisiTaskProdotto" TagName="Detail" Src="~/Analysis/DetailTaskProduct.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTasks.aspx">Selezione task</a>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:HyperLink NavigateUrl="DetailAnalysisTask.aspx" runat="server" id="lnkNavigation">
                            Analisi task</asp:HyperLink>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:HyperLink NavigateUrl="DetailTaskProduct.aspx" runat="server" id="lnkNavigation2">
                            Analisi task per prodotto</asp:HyperLink>
						<span class="divider">/</span>
					</li>
				</ul>
        <asp:Label runat="server" ID="lbl1" />
    <AnalisiTaskProdotto:Detail runat="server" id="frmDetailTaskProduct" />
</asp:Content>
