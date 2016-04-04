<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="risorseTurno.aspx.cs" Inherits="KIS.Reparti.risorseTurno" %>
<%@ Register TagPrefix="listPostazioni" TagName="risorse" Src="~/Reparti/risorseTurno.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <asp:Label runat="server" ID="lblTitoloRisorsa" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="/Reparti/configReparto.aspx">Reparti</a>
						<span class="divider">/</span>
                        <asp:hyperlink navigateurl="~/Reparti/configReparto.aspx?id=" runat="server" id="lnkReparto">Configurazione Reparto</asp:hyperlink>
						<span class="divider">/</span>
                        <asp:hyperlink navigateurl="~/Reparti/risorseTurno.aspx.aspx?idTurno=" runat="server" id="lnkRisorseTurno">Risorse produttive</asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>

    <listPostazioni:risorse runat="server" id="frmRisorsePostazioni" />
</asp:Content>
