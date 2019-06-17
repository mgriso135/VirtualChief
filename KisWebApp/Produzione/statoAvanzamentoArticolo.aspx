﻿<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="statoAvanzamentoArticolo.aspx.cs" Inherits="KIS.Produzione.statoAvanzamentoArticolo" %>
<%@ Register TagPrefix="articolo" TagName="stato" Src="~/Produzione/statoAvanzamentoArticolo.ascx" %>
<%@ Register TagPrefix="articolo" TagName="allarmeRitardo" Src="~/Eventi/ArticoloRitardo.ascx" %>
<%@ Register TagPrefix="articolo" TagName="allarmeWarning" Src="~/Eventi/ArticoloWarning.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:Literal runat="server" ID="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" />
                            </a>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>"><asp:Literal runat="server" ID="lblNavStato" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <articolo:stato runat="server" id="frmShowStatoArticolo" />

    <table>
        <tr>
            <td><articolo:allarmeRitardo runat="server" id="frmRitardo" /></td>
            <td>
                <articolo:allarmeWarning runat="server" id="frmWarning" />
            </td>
        </tr>
    </table>
    
</asp:Content>
