<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailAnalysisCustomer.aspx.cs" Inherits="KIS.Analysis.DetailAnalysisCustomer" %>
<%@ Register TagPrefix="analysis" TagName="detailCustomer" Src="~/Analysis/DetailAnalysisCustomer.ascx" %>
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
              			<asp:hyperlink runat="server" ID="lblThisLink" NavigateUrl="DetailAnalysistCustomer.aspx">Analisi cliente</asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span12">
    <h1 runat="server" id="lblTitle">Analisi cliente</h1>
            </div>
        </div>
    
    <div class="row-fluid">
        <div class="span12">
    <analysis:detailCustomer runat="server" id="frmDetailCustomer" />
        </div>
    </div>
</asp:Content>
