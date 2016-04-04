<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configProcessoPerProduzione.ascx.cs" Inherits="KIS.Produzione.configProcessoPerProduzione" %>

<h3>Configura il processo per la produzione</h3>
<h5><asp:Label runat="server" ID="lblNomeProc" /></h5>
<h5><asp:Label runat="server" ID="lblRevProc" /></h5>
<h5><asp:Label runat="server" ID="lblNomeVariante" /></h5>
<p>
<asp:Label runat="server" ID="lblQuantita" /><br />
Data prevista fine produzione: <asp:Label runat="server" ID="lblDataPrevistaFP" /><br />
Data prevista consegna: <asp:Label runat="server" ID="lblDataPrevistaConsegna" /><br />
Reparto produttivo:
    <asp:DropDownList runat="server" ID="ddlRepartoProduttivo" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlRepartoProduttivo_SelectedIndexChanged">
        <asp:ListItem Value="-1" Text="Seleziona un reparto produttivo" />
        </asp:DropDownList>
<br />
    </p>
<asp:Repeater runat="server" ID="rptTasks" OnItemDataBound="rptTasks_ItemDataBound" OnItemCommand="rptTasks_ItemCommand">
    <HeaderTemplate>
        <table class="table table-condensed table-striped table-hover">
            <thead>
            <tr>
                <td>ID</td>
                <td>Task</td>
                <td>Numero operatori</td>
                <td>Setup</td>
                <td>Tempo ciclo</td>
                <td>Postazione</td>
                <td>Task precedenti</td>
                <td>Task successivi</td>
                <td>Early Start Time</td>
                <td>Late Start Time</td>
                <td>Early Finish Time</td>
                <td>Late Finish Time</td>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><%#DataBinder.Eval(Container.DataItem, "Task.Task.processID") %>
                <asp:hiddenfield runat="server" ID="taskID" value='<%#DataBinder.Eval(Container.DataItem, "Task.Task.processID") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Task.Task.processName") %></td>
            <td><asp:DropDownList CssClass="dropdown" Width="60px" runat="server" ID="ddlTempi" OnSelectedIndexChanged="ddlTempi_SelectedIndexChanged" AutoPostBack="true" /></td>
            <td><asp:Label runat="server" ID="setup" /></td>
            <td><asp:Label runat="server" ID="tc" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "PostazioneDiLavoro.name") %></td>
            <td><asp:Label runat="server" ID="lblPrecedenti" /></td>
            <td><asp:Label runat="server" ID="lblSuccessivi" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "EarlyStartTime") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "LateStartTime") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "EarlyFinishTime") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "LateFinishTime") %></td>

        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        <tfoot>
        <tr>
            <td colspan="10" style="text-align: center;">
                <asp:Button runat="server" ID="btnSumbit" CommandName="checkConfigurazione" CommandArgument="CLICK" Text="VERIFICA LA CONFIGURAZIONE" /></td>
        </tr>
            </tfoot>
        </table>
    </FooterTemplate>
</asp:Repeater>

<br />
<br />
<asp:Label runat="server" ID="lbl1" />
<br />
<br />

<asp:Repeater runat="server" ID="rptControllo" OnItemDataBound="rptControllo_ItemDataBound" OnItemCommand="rptControllo_ItemCommand">
    <HeaderTemplate>
        <table class="table table-condensed table-striped table-hover">
            <thead>
            <tr>
                <td>ID</td>
                <td>Nome</td>
                <td>Postazione</td>
                <td>Numero operatori</td>
                <td>Setup</td>
                <td>Tempo ciclo</td>
                <td>Early Start Date</td>
                <td>Late Start Date</td>
                <td>Early Finish Date</td>
                <td>Late Finish Date</td>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr2">
            <td><asp:HiddenField runat="server" ID="taskID" Value='<%#DataBinder.Eval(Container.DataItem, "Task.Task.processID") %>' />
                <%#DataBinder.Eval(Container.DataItem, "Task.Task.processID") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Task.Task.processName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "PostazioneDiLavoro.name") %></td>
            <td><asp:HiddenField runat="server" ID="numOps" Value='<%#DataBinder.Eval(Container.DataItem, "Tempo.NumeroOperatori") %>' />
                <%#DataBinder.Eval(Container.DataItem, "Tempo.NumeroOperatori") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Tempo.TempoSetup") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Tempo.Tempo") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "EarlyStartDate") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "LateStartDate") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "EarlyFinishDate") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "LateFinishDate") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        <tfoot>
        <tr>
            <td colspan="9" style="text-align:center;">
                <asp:ImageButton runat="server" ID="btnLANCIA" CommandName="ProductionLaunch" CommandArgument="OK" ImageUrl="/img/iconComplete.png" Height="100px" ToolTip="Lancia l'articolo con processo così configurato in produzione!" />
            </td>
        </tr>
            </tfoot>
        </table>
    </FooterTemplate>
</asp:Repeater>