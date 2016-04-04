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
        <td>Ricerca per:
            <asp:TextBox runat="server" ID="txtIDArticolo" PlaceHolder="ID prodotto" Width="70" ToolTip="NB: formato ID articolo: NUM/ANNO" />/<asp:TextBox runat="server" ID="txtAnnoArticolo" PlaceHolder="Anno Produzione (e.g. 2014)" ToolTip="NB: formato ID articolo: NUM/ANNO" />
        </td>
        </tr>
    <tr>
        <td style="vertical-align: top"><asp:RadioButton id="due" Text="" Checked="False" GroupName="ModRicerca" runat="server"/></td>
        <td style="vertical-align:middle;">Oppure ricerca per:<br />
            Prodotto:&nbsp;<asp:DropDownList runat="server" ID="ddlTipoProdotto" AppendDataBoundItems="true"><asp:ListItem Text="Tutti i prodotti" Value="-1/-1/-1"></asp:ListItem></asp:DropDownList><br />
            Cliente:&nbsp;<asp:DropDownList runat="server" ID="ddlCliente" AppendDataBoundItems="true"><asp:ListItem Text="Tutti i clienti" Value=""></asp:ListItem></asp:DropDownList><br />
            <asp:CheckBox runat="server" id="chkConsideraDate" />Considera prodotti con data prevista produzione compresa tra <asp:textbox runat="server" id="txtProductDateStart" Width="100px"  /> e <asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
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
        <h3>Risultati ricerca</h3>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <th></th>
                <th>Commessa</th>
                <th>Cliente</th>
                <th>Articolo</th>
                <th>Matricola</th>
                <th>Processo</th>
                <th>Variante</th>
                <th>Quantità</th>
                <th>Reparto</th>
                <th>Data Fine Attività</th>
                <th>Data Prevista Consegna</th>
                <th>Tempo di lavoro (ore)</th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="font-family:Calibri; font-size:14px;">
            <td><asp:HyperLink runat="server" ID="lnkShowHistoryArticolo" NavigateUrl='<%# "~/Produzione/statoAvanzamentoArticolo.aspx?id=" +DataBinder.Eval(Container.DataItem, "ID")+"&anno=" +DataBinder.Eval(Container.DataItem, "Year") %>'>
                <asp:Image runat="server" ID="imgView" ImageUrl="/img/iconView.png" ToolTip="Visualizza la storia dell'articolo" Height="40" />
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