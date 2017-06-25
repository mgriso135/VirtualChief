<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AndonListReparti.aspx.cs" Inherits="KIS.Produzione.AndonListReparti" %>

<%@ Register TagPrefix="andon" TagName="listReparti" Src="~/Produzione/ListRepartiAndon.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:literal runat="server" id="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="AndonListReparti.aspx"><asp:literal runat="server" id="lblNavAndonRep" Text="<%$Resources:lblNavAndonRep %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <andon:listReparti runat="server" id="lstReparti" />
</asp:Content>
