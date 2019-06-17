<%@ Page Language="C#"  MasterPageFile="~/Site.Master" Title="Virtual Chief" AutoEventWireup="true" CodeBehind="PianoProduzioneCompleto.aspx.cs" Inherits="KIS.Produzione.PianoProduzioneCompleto" %>
<%@ Register TagPrefix="articoli" TagName="listINP" Src="~/Produzione/listArticoliINP.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:literal runat="server" id="lblNavProd" Text="<%$Resources:lblNavProd %>" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="PianoProduzioneCompleto.aspx"><asp:literal runat="server" id="lblNavPianoProd" Text="<%$Resources:lblNavPianoProd %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <articoli:listINP runat="server" ID="frmListArticoliINP" />
    </asp:Content>