<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerPortfolio.aspx.cs" Inherits="KIS.Analysis.CustomerPortfolio" %>
<%@ Register TagPrefix="customer" TagName="list" Src="~/Analysis/CustomerPortfolio.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx"><asp:Label runat="server" ID="lblNavAnalisi" meta:resourcekey="lblNavAnalisi" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="CustomerPortfolio.aspx"><asp:Label runat="server" ID="lblNavPortafoglioClienti" meta:resourcekey="lblNavPortafoglioClienti" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span11">
    <h1><asp:Label runat="server" ID="lblTitlePortafoglioClienti" meta:resourcekey="lblNavPortafoglioClienti" /></h1>
            </div>
        <div class="span1">
            <asp:HyperLink runat="server" ID="lnkCustomerPareto" NavigateUrl="CustomerPareto.aspx">
        <asp:Image runat="server" ID="imgCustomerPareto" ToolTip="<%$resources:lblTTShowPareto %>" AlternateText="<%$resources:lblTTShowPareto %>" ImageUrl="~/img/iconPareto.png" />
    </asp:HyperLink>
                  </div>
        </div>
    
    <div class="row-fluid">
        <div class="span12">
    <customer:list runat="server" id="frmCustomerList" />
        </div>
    </div>
</asp:Content>
