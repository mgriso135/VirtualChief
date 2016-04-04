<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerPareto.aspx.cs" Inherits="KIS.Analysis.CustomerPareto" %>
<%@ Register TagPrefix="customer" TagName="pareto" Src="~/Analysis/CustomerPareto.ascx" %>
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
          <li>
						<a href="CustomerPareto.aspx">Pareto clienti</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span12">
    <h1>Pareto clienti</h1>
            </div>
        </div>
    
    <div class="row-fluid">
        <div class="span12">
    <customer:pareto runat="server" id="frmCustomerPareto" />
        </div>
    </div>
</asp:Content>
