<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListCommesseChiuse.aspx.cs" Inherits="KIS.Analysis.ListCommesseChiuse" %>
<%@ Register TagPrefix="commesse" TagName="listChiuse" Src="~/Analysis/ListCommesseChiuse.ascx" %>

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
				</ul>
    <commesse:listChiuse runat="server" id="frmCommesseChiuse" />
</asp:Content>
