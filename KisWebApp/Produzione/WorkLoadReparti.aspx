<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkLoadReparti.aspx.cs" Inherits="KIS.Produzione.WorkLoadReparti" %>
<%@ Register TagName="reparti" TagPrefix="workload" Src="~/Produzione/wlReparto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
                    <li>
						<a href="produzione.aspx"><asp:literal runat="server" ID="lblNavProduzione" Text="<%$Resources:lblNavProduzione %>" /></a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="workLoadListReparti.aspx"><asp:literal runat="server" ID="lblNavCapProd" Text="<%$Resources:lblNavCapProd %>" /></a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="<%#Request.RawUrl %>"><asp:literal runat="server" ID="lblNavCapRichiesta" Text="<%$Resources:lblNavCapRichiesta %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <workload:reparti runat="server" ID="frmListReparto" />
</asp:Content>
