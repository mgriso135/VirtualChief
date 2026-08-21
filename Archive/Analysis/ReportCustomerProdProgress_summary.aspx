<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_summary.aspx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_summary" %>
<%@ Register TagPrefix="report" TagName="summary" Src="~/Analysis/ReportCustomerProdProgress_summary.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">
                            <asp:Label runat="server" ID="lblNavAnalisiDati" meta:resourcekey="lblNavAnalisiDati" />
                            </a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ReportCustomerProdProgress_chooseCustomer.aspx">
                            <asp:Label runat="server" ID="lblNavReportAvanProd" meta:resourcekey="lblNavReportAvanProd" />
                            </a>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:hyperlink NavigateUrl="ReportCustomerProdProgress_chooseINPProducts.aspx" runat="server" id="lnkProdReportINP">
                            <asp:Label runat="server" ID="lblNavChooseProds" meta:resourcekey="lblNavChooseProds" />
                            </asp:hyperlink>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:hyperlink NavigateUrl="ReportCustomerProdProgress_chooseFProducts.aspx" runat="server" id="lnkProdReportF">
                            <asp:Label runat="server" ID="lblNavChooseProdsF" meta:resourcekey="lblNavChooseProdsF" />
                            </asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <report:summary runat="server" ID="frmSummary" />
</asp:Content>
