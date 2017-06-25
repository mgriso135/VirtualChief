<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzEditPERT.aspx.cs" Inherits="KIS.Commesse.wzEditPERT" %>
<%@ Register TagPrefix="pert" TagName="edit" Src="~/Commesse/wzEditPERT.ascx" %>
<%@ Register TagPrefix="variante" TagName="editData" Src="~/Commesse/wzVarianteDettagli.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleNewOrder" Text="<%$Resources:lblTitleNewOrder %>" /></h1>
    <ul class="breadcrumb hidden-phone">
		<li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNewOrder" Text="<%$Resources:lblNewOrder %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink NavigateUrl="wzAddPERT.aspx" runat="server" id="lnkAddPert">
                <asp:Label runat="server" ID="lblDescProd" Text="<%$Resources:lblDescProd %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkEditPert"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblDescProcess" Text="<%$Resources:lblDescProcess %>" /></b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <asp:Label runat="server" ID="lbl1" />
    <div class="row-fluid">
        <div class="span1">
            <asp:Hyperlink runat="server" ID="LinkBCK">
            <asp:Image runat="server" ID="lnkGoBack" ImageUrl="~/img/iconArrowLeft.png" ToolTip="<%$Resources:lblTTGoBack %>" Height="40" />
                </asp:Hyperlink>
        </div>
        
        <div class="span9"><variante:editData runat="server" ID="frmEditDatiVariante" /></div>
        <div class="span1">
            <asp:HyperLink runat="server" ID="lnkSwitchToGrid">
            <asp:Image runat="server" ID="imgSwitchToGrid" ImageUrl="~/img/iconGrid.jpg" Height="50" ToolTip="<%$Resources:lblTTGoToTablePERT %>" />
                </asp:HyperLink>
        </div>
        <div class="span1" style="align-content:center;">
            <asp:Hyperlink runat="server" ID="LinkFWD">
            <asp:Image runat="server" ID="lnkGoFwd" ImageUrl="~/img/iconArrowRight.png" ToolTip="<%$Resources:lblTTGoFwd %>" Height="40" />
                </asp:Hyperlink>
        </div>
        
        </div>
    <pert:edit runat="server" id="frmEditPERT" />
</asp:Content>
