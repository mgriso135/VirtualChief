<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CostificazioneProdottiTerminatiRicerca.aspx.cs" Inherits="KIS.Analysis.CostificazioneProdottiTerminatiRicerca" %>
<%@ Register TagPrefix="ProdottiTerminati" TagName="Ricerca" Src="~/Analysis/CostificazioneProdottiTerminatiRicerca.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx"><asp:label runat="server" id="lblNavAnalisiDati" meta:resourcekey="lblNavAnalisiDati" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="CostificazioneProdottiTerminatiRicerca.aspx"><asp:label runat="server" id="lblNavCostificazione" meta:resourcekey="lblNavCostificazione" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <ProdottiTerminati:Ricerca runat="server" id="frmCercaProdottiTerminati" />
</asp:Content>
