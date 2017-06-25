<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CostificazioneProdottiTerminatiRicerca.ascx.cs" Inherits="KIS.Analysis.CostificazioneProdottiTerminatiRicerca1" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtProductDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtProductDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script>      
<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblRicerca" class="table table-striped table-hover">
    <tr>
        <td>
            <asp:RadioButton id="uno" Text="" Checked="True" GroupName="ModRicerca" runat="server"/>
        </td>
        <td><asp:Label runat="server" ID="lblRicercaPerIDProd" meta:resourcekey="lblRicercaPerIDProd" />
            <asp:TextBox runat="server" ID="txtIDArticolo" PlaceHolder="<%$resources:txtPlaceIDArticolo %>" Width="70" ToolTip="<%$resources:txtTTIDArticolo %>" />/<asp:TextBox runat="server" ID="txtAnnoArticolo" PlaceHolder="<%$resources:txtPlaceAnnoArticolo %>" ToolTip="<%$resources:txtTTIDArticolo %>" />
        </td>
        </tr>
    <tr>
        <td style="vertical-align: top"><asp:RadioButton id="due" Text="" Checked="False" GroupName="ModRicerca" runat="server"/></td>
        <td style="vertical-align:middle;"><asp:Label runat="server" ID="lblRicercaPerProdCliente" meta:resourcekey="lblRicercaPerProdCliente" /><br />
            <asp:Label runat="server" ID="lblProdotto" meta:resourcekey="lblProdotto" />&nbsp;<asp:DropDownList runat="server" ID="ddlTipoProdotto" AppendDataBoundItems="true"><asp:ListItem Text="<%$resources:ddlTuttiProdotti %>" Value="-1/-1/-1"></asp:ListItem></asp:DropDownList><br />
            <asp:Label runat="server" ID="lblCliente" meta:resourcekey="lblCliente" />&nbsp;<asp:DropDownList runat="server" ID="ddlCliente" AppendDataBoundItems="true"><asp:ListItem Text="<%$resources:ddlTuttiClienti %>" Value=""></asp:ListItem></asp:DropDownList><br />
            <asp:CheckBox runat="server" id="chkConsideraDate" /><asp:Label runat="server" ID="lblFiltroDataProd1" meta:resourcekey="lblFiltroDataProd1" />
            <asp:textbox runat="server" id="txtProductDateStart" Width="100px"  />&nbsp;<asp:Label runat="server" ID="lblFiltroDataProd2" meta:resourcekey="lblFiltroDataProd2" />&nbsp;<asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton ID="imgSearch" runat="server" Height="40" ImageUrl="~/img/iconView.png" OnClick="imgSearch_Click" />RICERCA!
        </td>
    </tr>
</table>
<asp:Repeater runat="server" ID="rptArticoliTerminati">
    <HeaderTemplate>
        <h3><asp:Label runat="server" ID="lblSearchResults" meta:resourcekey="lblSearchResults" /></h3>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <th></th>
                <th><asp:Label runat="server" ID="lblTHCommessa" meta:resourcekey="lblTHCommessa" /></th>
                <th><asp:Label runat="server" ID="lblTHCliente" meta:resourcekey="lblTHCliente" /></th>
                <th><asp:Label runat="server" ID="lblTHArticolo" meta:resourcekey="lblTHArticolo" /></th>
                <th><asp:Label runat="server" ID="lblTHMatricola" meta:resourcekey="lblTHMatricola" /></th>
                <th><asp:Label runat="server" ID="lblTHProcesso" meta:resourcekey="lblTHProcesso" /></th>
                <th><asp:Label runat="server" ID="lblTHVariante" meta:resourcekey="lblTHVariante" /></th>
                <th><asp:Label runat="server" ID="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
                <th><asp:Label runat="server" ID="lblTHReparto" meta:resourcekey="lblTHReparto" /></th>
                <th><asp:Label runat="server" ID="lblTHDataFine" meta:resourcekey="lblTHDataFine" /></th>
                <th><asp:Label runat="server" ID="lblTHDataConsegna" meta:resourcekey="lblTHDataConsegna" /></th>
                <th><asp:Label runat="server" ID="lblTHTempoLavoro" meta:resourcekey="lblTHTempoLavoro" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="font-family:Calibri; font-size:14px;">
            <td><asp:HyperLink runat="server" ID="lnkShowHistoryArticolo" NavigateUrl='<%# "~/Produzione/statoAvanzamentoArticolo.aspx?id=" +DataBinder.Eval(Container.DataItem, "ID")+"&anno=" +DataBinder.Eval(Container.DataItem, "Year") %>'>
                <asp:Image runat="server" ID="imgView" ImageUrl="/img/iconView.png" ToolTip="<%$resources:lblTTStoriaArticolo %>" Height="40" />
                </asp:HyperLink></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Commessa") %>/<%#DataBinder.Eval(Container.DataItem, "AnnoCommessa") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Cliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.Process.ProcessName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Proc.Variant.nomeVariante") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "RepartoNome") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "DataFineAttivita") %></td>
            <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "DataPrevistaConsegna")).ToString("dd/MM/yyyy") %></td>
            <td><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroTotale")).TotalHours, 2).ToString() %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate></tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>