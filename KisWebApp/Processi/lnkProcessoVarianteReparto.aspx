<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="lnkProcessoVarianteReparto.aspx.cs" Inherits="KIS.Processi.lnkProcessoVarianteReparto" %>
<%@ Register TagPrefix="reparti" TagName="list" Src="~/Processi/lnkProcRepartoLista.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone" runat="server" id="tblPertNavBar" visible="false">
					<li>
                        <asp:HyperLink runat="server" ID="lnkManageProcesso" NavigateUrl="showProcesso.aspx">
                            <asp:Literal runat="server" ID="lblNavCreaProcesso" Text="<%$Resources:lblNavCreaProcesso %>" />
						</asp:HyperLink>
						<span class="divider">/</span>
                        <a href="<%#Request.RawUrl %>" class="lead">
                            <strong>
                                <asp:Literal runat="server" ID="lblNavAssociaReparto" Text="<%$Resources:lblNavAssociaReparto %>" />
                                </strong>
                            </a>
						<span class="divider">/</span>
                        <asp:LinkButton OnClick="lnkStep3_Click" runat="server" ID="LinkButton1" ToolTip="<%$Resources:lblTTAttrezzi %>">
                            <asp:Literal runat="server" ID="lblNavAssociaPostazioni" Text="<%$Resources:lblNavAssociaPostazioni %>" />
            </asp:LinkButton><span class="divider">/</span>
                        <div class="ui-widget" id="dvInfo" runat="server" visible="false">
	<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Label runat="server" ID="lblTooltip"><asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblTTAttrezzi %>" /></asp:Label></p>
	</div>
</div>	</li>
				</ul>
   <h3><asp:Literal runat="server" ID="Literal2" Text="<%$Resources:lblTitleAddProcesso %>" />
       </h3>

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

    <asp:HyperLink runat="server" ID="lnkGestisciReparti" NavigateUrl="~/Reparti/configReparto.aspx" Target="_blank">
        <asp:Image CssClass="btn btn-primary" runat="server" ID="imgReparti" ToolTip="<%$Resources:lblTTGestioneRep %>" ImageUrl="~/img/iconFactory.png" Height="80" />
    </asp:HyperLink>

    <reparti:list runat="server" id="frmListReparti" />
</asp:Content>
