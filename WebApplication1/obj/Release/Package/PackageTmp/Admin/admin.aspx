<%@ Page Title="Admin section" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
CodeBehind="admin.aspx.cs" Inherits="KIS.kpi.admin" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="#">Admin</a>
						<span class="divider">/</span>
					</li>
					<!--
					<li><a href="#">Library</a> <span class="divider">/</span></li>
				    <li class="active">Data</li>
				    -->
				</ul>

    <div class="page-header">
        <asp:label runat="server" id="lblPageHeader" meta:resourcekey="lblPageHeader" />
    </div>
    <p class="lead">
<asp:Label runat="server" ID="lbl1" />
<a href="~/Login/login.aspx" runat="server" id="lnkLogin">
    <asp:label runat="server" id="lblNeedToLogin" meta:resourcekey="lblNeedToLogin" />
</a>
        </p>

    <div class="row-fluid" runat="server" id="tblOptions">
    <ul class="thumbnails unstyled">
        <li class="span2">
            <a href="~/Admin/kisAdmin.aspx" runat="server" id="lnkConfigPrincipale">
<asp:Image runat="server" ImageUrl="~/img/iconConfiguration.png" id="Image2" ToolTip="Configurazione Logo e editor processi" height="100" CssClass="btn btn-primary"/></a>
<p><asp:label runat="server" id="lblMainConfig" meta:resourcekey="lblMainConfig" /></p>
            </li>
        <li class="span2">
            <a href="../Users/manageUsers.aspx" runat="server" id="A1">
<asp:Image runat="server" ImageUrl="~/img/iconUser.png" id="Image1" ToolTip="Gestione utenti" height="100" CssClass="btn btn-primary"/></a>
<p><asp:label runat="server" id="lblGestioneUtenti" meta:resourcekey="lblGestioneUtenti" /></p>
            </li>

        <li class="span2">
            <asp:HyperLink runat="server" ID="lnkGruppi" NavigateUrl="~/Users/manageGruppi.aspx">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgAdminGroups" ToolTip="Gestione gruppi" Height="100" ImageUrl="~/img/iconGroup.png" />
        </asp:HyperLink>
        <p><asp:label runat="server" id="lblGestioneGruppi" meta:resourcekey="lblGestioneGruppi" /></p>
        </li>
        <li class="span2">
        <asp:HyperLink runat="server" ID="lnkManagePermessi" NavigateUrl="~/Users/managePermessi.aspx">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgAdminPermessi" ToolTip="Gestione permessi" Height="100" ImageUrl="~/img/iconPermissions.png" /></asp:HyperLink>
        <p><asp:label runat="server" id="lblGestionePermessi" meta:resourcekey="lblGestionePermessi" /></p>
        </li>
        <li class="span2">
            <asp:HyperLink runat="server" ID="lnkMenu" NavigateUrl="~/Admin/manageMenu.aspx">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgMenu" ToolTip="Gestione menu principale" Height="100" ImageUrl="~/img/iconMenuItem.jpg" /></asp:HyperLink>
        <p><asp:label runat="server" id="lblGestioneMenu" meta:resourcekey="lblGestioneMenu" /></p>
        </li>
        <li class="span2">
            <asp:HyperLink runat="server" ID="lnkReports" NavigateUrl="~/Admin/manageReports.aspx">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="Image3" ToolTip="Configurazione dei reports" Height="100" ImageUrl="~/img/iconReport.png" /></asp:HyperLink>
        <p><asp:label runat="server" id="lblCfgReports" meta:resourcekey="lblCfgReports" /></p>
        </li>
        </ul>
        </div>
</asp:Content>

