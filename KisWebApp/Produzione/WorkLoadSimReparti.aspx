<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkLoadSimReparti.aspx.cs" Inherits="KIS.Produzione.WorkLoadSimReparti" %>
<%@ Register TagName="reparti" TagPrefix="SimWorkload" Src="~/Produzione/wlSimReparto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
                    <li>
						<a href="produzione.aspx"><asp:literal runat="server" id="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="workLoadListReparti.aspx"><asp:literal runat="server" id="lblNavCapacitaProd" Text="<%$Resources:lblNavCapacitaProd %>" /></a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="<%#Request.RawUrl %>"><asp:literal runat="server" id="lblNavSimWorkload" Text="<%$Resources:lblNavSimWorkload %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <SimWorkload:Reparti runat="server" ID="frmListReparto" />
</asp:Content>
