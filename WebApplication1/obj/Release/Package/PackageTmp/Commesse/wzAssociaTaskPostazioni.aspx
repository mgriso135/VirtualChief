<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzAssociaTaskPostazioni.aspx.cs" Inherits="KIS.Commesse.wzAssociaTaskPostazioni" %>
<%@ Register TagPrefix="task" TagName="associapostazioni" Src="~/Commesse/wzAssociaTaskPostazioni.ascx" %>
<%@ Register TagPrefix="postazione" TagName="add" Src="/Postazioni/addPostazione.ascx" %>
<%@ Register TagPrefix="postazione" TagName="workload" Src="/Reparti/processoWorkLoad.ascx" %>
<%@ Register TagPrefix="Wizard" TagName="InfoPanel" Src="~/Commesse/wzInfoPanel.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:scriptmanager runat="server" ID="scriptMan1" />
    <h1>Wizard: nuovo prodotto su commessa</h1>
    <ul class="breadcrumb hidden-phone">
        <li>
		    <a href="wzAddCommessa.aspx">Nuova commessa</a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" id="lnkAddPert">Descrivi il prodotto</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink ID="lnkEditPert" runat="server">Descrivi il processo produttivo</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaReparto">Seleziona il reparto produttivo</asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkAssociaTasks"><b style="font-size: 14px;">Associa i tasks alle postazioni</b></asp:hyperlink>
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
    <asp:ImageButton runat="server" ID="imgShowAddPostazioni" OnClick="imgShowAddPostazioni_Click" ImageUrl="/img/iconAdd2.png" Height="60px" ToolTip="Aggiungi una nuova postazione di lavoro" />
    <a href="/Postazioni/managePostazioniLavoro.aspx">
    <asp:Image ID="imgManagePostazioni" ImageUrl="/img/iconManage.png" Height="60px" ToolTip="Gestisci le postazioni di lavoro" runat="server" /></a>
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
