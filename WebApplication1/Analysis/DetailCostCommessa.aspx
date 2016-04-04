﻿<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailCostCommessa.aspx.cs" Inherits="KIS.Analysis.DetailCostCommessa" %>

<%@ Register TagPrefix="commessa" TagName="CostDetail" Src="~/Analysis/DetailCostCommessa.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListCommesseChiuse.aspx">Costificazione commesse: elenco</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="DetailCostCommessa.aspx" runat="server" id="lnkMenu">Dettaglio costificazione commessa</a>
						<span class="divider">/</span>
					</li>
				</ul>

    <commessa:costDetail runat="server" id="frmCostDetail" />
</asp:Content>
