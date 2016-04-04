<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="lnkProcessoVarianteReparto.aspx.cs" Inherits="KIS.Processi.lnkProcessoVarianteReparto" %>
<%@ Register TagPrefix="reparti" TagName="list" Src="~/Processi/lnkProcRepartoLista.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone" runat="server" id="tblPertNavBar" visible="false">
					<li>
                        <asp:HyperLink runat="server" ID="lnkManageProcesso" NavigateUrl="showProcesso.aspx">
						1. Crea il processo produttivo</asp:HyperLink>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>" class="lead">
                            <strong>
                            2. Associa il processo produttivo al reparto
                                </strong>
                            </a>
						<span class="divider">/</span>
                        <asp:LinkButton OnClick="lnkStep3_Click" runat="server" ID="LinkButton1" ToolTip="Clicca sull'icona degli attrezzi in fianco al reparto per proseguire">
            3. Associa i task alle postazioni di esecuzione
            </asp:LinkButton><span class="divider">/</span>
                        <div class="ui-widget" id="dvInfo" runat="server" visible="false">
	<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Label runat="server" ID="lblTooltip">Clicca sull'icona degli attrezzi in fianco al reparto per proseguire</asp:Label></p>
	</div>
</div>	</li>
				</ul>
   <h3>Associa il processo produttivo al reparto</h3>

   <table class="table table-hover table-condensed">
        <tr>
            <td>
    <asp:Label runat="server" ID="lblNomeProc" /> rev. <asp:Label runat="server" ID="lblRevProc" />
    </td></tr>
        <tr><td>
    <asp:Label runat="server" ID="lblDescProc" />
    </td></tr>
    <tr><td>
    <asp:Label runat="server" ID="lblNomeVar" />
    </td></tr>
    </table>

    <asp:HyperLink runat="server" ID="lnkGestisciReparti" NavigateUrl="/Reparti/configReparto.aspx" Target="_blank">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgReparti" ToolTip="Gestione reparti" ImageUrl="/img/iconFactory.png" Height="80" />
    </asp:HyperLink>

    <reparti:list runat="server" id="frmListReparti" />
</asp:Content>
