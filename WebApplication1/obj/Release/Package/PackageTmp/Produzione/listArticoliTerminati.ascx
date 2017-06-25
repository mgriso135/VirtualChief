<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listArticoliTerminati.ascx.cs" Inherits="KIS.Produzione.listArticoliTerminati" %>
<h3>Storico produzione</h3>

<table>
    <thead>
    <tr><td><asp:Literal runat="server" ID="lblInizio" Text="<%$Resources:lblInizio %>" />
        </td><td><asp:Literal runat="server" ID="lblFine" Text="<%$Resources:lblFine %>" /></td></tr></thead>
    <tbody>
    <tr><td><asp:Calendar runat="server" ID="calInizio" OnSelectionChanged="calInizio_SelectionChanged" /></td>
        <td><asp:Calendar runat="server" ID="calFine" OnSelectionChanged="calFine_SelectionChanged" /></td>
    </tr>
        </tbody>
</table>
<asp:Label runat="server" ID="lbl1" />
<asp:Label runat="server" ID="lblDate" />
<asp:Repeater runat="server" ID="rptArticoliTerminati" OnItemDataBound="rptArticoliTerminati_ItemDataBound" OnItemCommand="rptArticoliTerminati_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <th></th>
                <th><asp:Literal runat="server" ID="lblOrdine" Text="<%$Resources:lblOrdine %>" /></th>
                <th><asp:Literal runat="server" ID="lblCliente" Text="<%$Resources:lblCliente %>" /></th>
                <th><asp:Literal runat="server" ID="lblProdotto" Text="<%$Resources:lblProdotto %>" /></th>
                <th><asp:Literal runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" /></th>
                <th><asp:Literal runat="server" ID="lblTipoOrdine" Text="<%$Resources:lblTipoProdotto %>" /></th>
                <th><asp:Literal runat="server" ID="lblQuantita" Text="<%$Resources:lblQuantita %>" /></th>
                <th><asp:Literal runat="server" ID="lblReparto" Text="<%$Resources:lblReparto %>" /></th>
                <th><asp:Literal runat="server" ID="lblDataFineAtt" Text="<%$Resources:lblDataFineAtt %>" /></th>
                <th><asp:Literal runat="server" ID="lblDataPrevConsegna" Text="<%$Resources:lblDataPrevConsegna %>" /></th>
                <th><asp:Literal runat="server" ID="lblTempoLavoro" Text="<%$Resources:lblTempoLavoro %>" /></th>
                <th><asp:Literal runat="server" ID="lblRitardo" Text="<%$Resources:lblRitardo %>" /></th>
                <th><asp:Literal runat="server" ID="lblRiesuma" Text="<%$Resources:lblRiesuma %>" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="font-family:Calibri; font-size:14px;">
            <td><asp:HyperLink runat="server" ID="lnkShowHistoryArticolo" NavigateUrl='<%# "statoAvanzamentoArticolo.aspx?id=" +DataBinder.Eval(Container.DataItem, "ID")+"&anno=" +DataBinder.Eval(Container.DataItem, "Year") %>'>
                <asp:Image runat="server" ID="imgView" ImageUrl="/img/iconView.png" ToolTip="Visualizza la storia dell'articolo" Height="40" />
                </asp:HyperLink></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Commessa") %>/<%#DataBinder.Eval(Container.DataItem, "AnnoCommessa") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.Process.ProcessName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.Variant.nomeVariante") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %>&nbsp;(<%#DataBinder.Eval(Container.DataItem, "Quantita") %>)</td>
            <td><%#DataBinder.Eval(Container.DataItem, "RepartoNome") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "DataFineAttivita") %></td>
            <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
            <td><%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroTotale")).TotalHours.ToString("F2") %></td>
            <td><%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "Ritardo")).TotalHours.ToString("F2") %></td>
            <td><asp:ImageButton runat="server" ID="btnRiesuma" ImageUrl="~/img/iconExhume.png" Height="30" ToolTip="Riesuma il prodotto" CommandName="riesuma" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IDCombinato") %>' /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate></tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>