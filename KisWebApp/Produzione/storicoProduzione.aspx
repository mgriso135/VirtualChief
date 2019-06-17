<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="storicoProduzione.aspx.cs" Inherits="KIS.Produzione.storicoProduzione" %>
<%@ Register TagPrefix="ArticoliTerminati" TagName="list" Src="~/Produzione/listArticoliTerminati.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="produzione.aspx"><asp:Literal runat="server" ID="lblNavProd" Text="<%$Resources:lblNavProd %>" />
                            </a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="<%#Request.RawUrl %>"><asp:Literal runat="server" ID="lblNavStoricoProd" Text="<%$Resources:lblNavStoricoProd %>" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <ArticoliTerminati:list runat="server" id="lstArticoliTerminati" />
</asp:Content>
