<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="linkArticoliToCommessa.aspx.cs" Inherits="KIS.Produzione.linkArticoliToCommessa" %>
<%@Register TagPrefix="commessa" TagName="linkArticoli" Src="~/Commesse/linkArticoli.ascx" %>
<%@ Register TagPrefix="commessa" TagName="listArticoli" Src="~/Commesse/listArticoliCommessa.ascx" %>
<%@ Register TagPrefix="commessa" TagName="EventoRitardo" Src="~/Eventi/CommessaRitardo.ascx" %>
<%@ Register TagPrefix="commessa" TagName="EventoWarning" Src="~/Eventi/CommessaWarning.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="commesse.aspx">Commesse</a>
						<span class="divider">/</span>
                        <a href="<%# Request.RawUrl %>">Dettaglio commessa</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3>Aggiunta articoli a commessa</h3>
    
    Commessa: <asp:Label runat="server" ID="lblIDCommessa" />
    <br />
    Data inserimento: <asp:Label runat="server" ID="lblDataCommessa" />
    <br />
    Note: <asp:Label runat="server" ID="lblNote" />
    <br /><br />
    <asp:label runat="server" ID="lbl1" />
    <commessa:linkArticoli id="frmLinkArticolo" runat="server" />
    <br />
    <commessa:listArticoli id="frmListArticoli" runat="server" />
    <br />
    <table>
        <tr>
            <td style="vertical-align:top;">
    <commessa:eventoritardo runat="server" id="frmConfigRitardo" />
                </td>
            <td style="vertical-align:top;">
                <commessa:eventowarning runat="server" id="frmConfigWarning" />
            </td>
            </tr>
        </table>
</asp:Content>
