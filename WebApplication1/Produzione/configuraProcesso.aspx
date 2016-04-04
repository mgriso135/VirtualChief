﻿<%@ Page Language="C#" Title="Kaizen Indicator System" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configuraProcesso.aspx.cs" Inherits="KIS.Produzione.configuraProcesso" %>

<%@ Register TagPrefix="processi" TagName="configPerProduzione" Src="~/Produzione/configProcessoPerProduzione.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx">Produzione</a>
						<span class="divider">/</span>
                        <a href="commesseDaProdurre.aspx">Nuove commesse</a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>">Inserimento in produzione</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <processi:configPerProduzione runat="server" id="frmConfigurazione" />
</asp:Content>
