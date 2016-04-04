<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseCustomer.aspx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseCustomer" %>
<%@ Register TagPrefix="reportCustomer" TagName="chooseCustomer" Src="~/Analysis/ReportCustomerProdProgress_chooseCustomer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseCustomer.aspx">Report avanzamento prodotti per cliente</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <reportCustomer:chooseCustomer runat="server" id="frmChooseCustomer" />
</asp:Content>
