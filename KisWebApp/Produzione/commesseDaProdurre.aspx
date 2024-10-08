﻿<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="commesseDaProdurre.aspx.cs" Inherits="KIS.Produzione.commesseDaProdurre" %>
<%@ Register TagPrefix="commesse" TagName="listStatoN" Src="~/Produzione/listCommesseStatoN.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:literal runat="server" ID="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
                        <a href="commesseDaProdurre.aspx"><asp:literal runat="server" ID="lblNavNuoveCommesse" Text="<%$Resources:lblNavNuoveCommesse %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <commesse:listStatoN runat="server" id="frmListCommesseStatoN" />
</asp:Content>
