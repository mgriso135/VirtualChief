<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerPortfolio.aspx.cs" Inherits="KIS.Analysis.CustomerPortfolio" %>
<%@ Register TagPrefix="customer" TagName="list" Src="~/Analysis/CustomerPortfolio.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="CustomerPortfolio.aspx">Portafoglio clienti</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span11">
    <h1>Portafoglio clienti</h1>
            </div>
        <div class="span1">
            <asp:HyperLink runat="server" ID="lnkCustomerPareto" NavigateUrl="CustomerPareto.aspx">
        <asp:Image runat="server" ID="imgCustomerPareto" ToolTip="Mostra il diagramma di Pareto clienti" AlternateText="Mostra il diagramma di Pareto clienti" ImageUrl="~/img/iconPareto.png" />
    </asp:HyperLink>
                  </div>
        </div>
    
    <div class="row-fluid">
        <div class="span12">
    <customer:list runat="server" id="frmCustomerList" />
        </div>
    </div>
</asp:Content>
