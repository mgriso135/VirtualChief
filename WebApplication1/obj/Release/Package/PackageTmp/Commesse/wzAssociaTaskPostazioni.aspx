<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAssociaTaskPostazioni.aspx.cs" Inherits="KIS.Commesse.wzAssociaTaskPostazioni" %>
<%@ Register TagPrefix="task" TagName="associapostazioni" Src="~/Commesse/wzAssociaTaskPostazioni.ascx" %>
<%@ Register TagPrefix="postazione" TagName="add" Src="/Postazioni/addPostazione.ascx" %>
<%@ Register TagPrefix="postazione" TagName="workload" Src="/Reparti/processoWorkLoad.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:scriptmanager runat="server" ID="scriptMan1" />
    <h1><asp:Label runat="server" ID="lblTitleWizNuovoProd" Text="<%$Resources:lblTitleWizNuovoProd %>" /></h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNuovoOrdine" Text="<%$Resources:lblNuovoOrdine %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert"><asp:Label runat="server" ID="lblDescProd" Text="<%$Resources:lblDescProd %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server"><asp:Label runat="server" ID="lblDescProcesso" Text="<%$Resources:lblDescProcesso %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto"><asp:Label runat="server" ID="lblSelReparto" Text="<%$Resources:lblSelReparto %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks"><b style="font-size: 14px;"><asp:Label runat="server" ID="lblAssociaTaskPostazione" Text="<%$Resources:lblAssociaTaskPostazione %>" /></b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>

    <div class="row-fluid">
        <div class="span1">
            <asp:HyperLink runat="server" ID="lnkGoBack" NavigateUrl="~/Commesse/wzAssociaPERTReparto.aspx">
                <asp:Image runat="server" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
            </asp:HyperLink>
        </div>
        <div class="span7">
    <asp:ImageButton runat="server" ID="imgShowAddPostazioni" OnClick="imgShowAddPostazioni_Click" ImageUrl="/img/iconAdd2.png" Height="60px" ToolTip="<%$Resources:lblAddPostazione %>" />
    <a href="/Postazioni/managePostazioniLavoro.aspx">
    <asp:Image ID="imgManagePostazioni" ImageUrl="/img/iconManage.png" Height="60px" ToolTip="<%$Resources:lblManagePostazioni %>" runat="server" /></a>
    <br />
    <postazione:add runat="server" ID="frmAddPostazione" />
    <task:associapostazioni runat="server" ID="frmAssociaTasksPostazioni" />
    <postazione:workload runat="server" ID="frmWorkLoad" />
            </div>
        <div class="span1">
            <asp:UpdatePanel runat="server" ID="upd1">
                <ContentTemplate>
            <asp:HyperLink runat="server" ID="lnkGoFwd" NavigateUrl="">
                <asp:Image ID="imgGoFwd" runat="server" ImageUrl="~/img/iconArrowRight.png" Height="40" />
            </asp:HyperLink>
                    <asp:Timer runat="server" ID="timer1" Interval="5000" OnTick="timer1_Tick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
        </div>
        <div class="span3">
            <Wizard:InfoPanel runat="server" ID="InfoPanel" />
        </div>
        </div>
</asp:Content>
