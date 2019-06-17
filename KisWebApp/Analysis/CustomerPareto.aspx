<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerPareto.aspx.cs" Inherits="KIS.Analysis.CustomerPareto" %>
<%@ Register TagPrefix="customer" TagName="pareto" Src="~/Analysis/CustomerPareto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx"><asp:label runat="server" id="lblNavAnalisi" meta:resourcekey="lblNavAnalisi" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="CustomerPortfolio.aspx"><asp:label runat="server" id="lblNavPortafoglioClienti" meta:resourcekey="lblNavPortafoglioClienti" /></a>
						<span class="divider">/</span>
					</li>
          <li>
						<a href="CustomerPareto.aspx"><asp:label runat="server" id="lblNavPareto" meta:resourcekey="lblNavPareto" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <div class="row-fluid">
        <div class="span12">
    <h1><asp:label runat="server" id="lblNavPareto2" meta:resourcekey="lblNavPareto" /></h1>
            </div>
        </div>
    
    <div class="row-fluid">
        <div class="span12">
    <customer:pareto runat="server" id="frmCustomerPareto" />
        </div>
    </div>
</asp:Content>
