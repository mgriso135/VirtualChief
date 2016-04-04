<%@ Page Language="C#"  MasterPageFile="~/Site.Master" Title="Kaizen Indicator System" AutoEventWireup="true" CodeBehind="PianoProduzioneCompleto.aspx.cs" Inherits="KIS.Produzione.PianoProduzioneCompleto" %>
<%@ Register TagPrefix="articoli" TagName="listINP" Src="~/Produzione/listArticoliINP.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx">Produzione</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="PianoProduzioneCompleto.aspx">Piano produttivo</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <articoli:listINP runat="server" ID="frmListArticoliINP" />
    </asp:Content>