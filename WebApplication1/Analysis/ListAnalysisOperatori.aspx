<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListAnalysisOperatori.aspx.cs" Inherits="KIS.Analysis.ListAnalysisOperatori" %>
<%@ Register TagPrefix="analisi" TagName="listOperatori" Src="~/Analysis/ListAnalysisOperatori.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisOperatori.aspx">Selezione operatore</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <analisi:listOperatori runat="server" id="frmListOperatori" />
</asp:Content>
