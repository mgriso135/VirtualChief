<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configProcessoPerProduzione.ascx.cs" Inherits="KIS.Produzione.configProcessoPerProduzione" %>

<h3><asp:Literal runat="server" ID="lblTitleCfg" Text="<%$Resources:lblTitleCfg %>" /></h3>
<h5><asp:Label runat="server" ID="lblNomeProc" /></h5>
<h5><asp:Label runat="server" ID="lblRevProc" /></h5>
<h5><asp:Label runat="server" ID="lblNomeVariante" /></h5>
<p>
<asp:Label runat="server" ID="lblQuantita" /><br />
<asp:Literal runat="server" ID="lblDataFine" Text="<%$Resources:lblDataFine %>" />: <asp:Label runat="server" ID="lblDataPrevistaFP" /><br />
<asp:Literal runat="server" ID="lblDataConsegna" Text="<%$Resources:lblDataConsegna %>" />: <asp:Label runat="server" ID="lblDataPrevistaConsegna" /><br />
<asp:Literal runat="server" ID="lblReparto" Text="<%$Resources:lblReparto %>" />:
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
                <th><asp:Literal runat="server" ID="lblTaskID" Text="<%$Resources:lblTaskID %>" /></th>
                <th><asp:Literal runat="server" ID="lblNomeTask" Text="<%$Resources:lblNomeTask %>" /></th>
                <th><asp:Literal runat="server" ID="lblNumOp" Text="<%$Resources:lblNumOp %>" /></th>
                <th><asp:Literal runat="server" ID="lblSetup" Text="<%$Resources:lblSetup %>" /></th>
                <th><asp:Literal runat="server" ID="lblTempoCiclo" Text="<%$Resources:lblTempoCiclo %>" /></th>
                <th><asp:Literal runat="server" ID="lblPostazione" Text="<%$Resources:lblPostazione %>" /></th>
                <th><asp:Literal runat="server" ID="lblTaskPrecedenti" Text="<%$Resources:lblTaskPrecedenti %>" /></th>
                <th><asp:Literal runat="server" ID="lblTaskSuccessivi" Text="<%$Resources:lblTaskSuccessivi %>" /></th>
                <th><asp:Literal runat="server" ID="lblEarlyStart" Text="<%$Resources:lblEarlyStart %>" /></th>
                <th><asp:Literal runat="server" ID="lblLateStart" Text="<%$Resources:lblLateStart %>" /></th>
                <th><asp:Literal runat="server" ID="lblEarlyFinish" Text="<%$Resources:lblEarlyFinish %>" /></th>
                <th><asp:Literal runat="server" ID="lblLateFinish" Text="<%$Resources:lblLateFinish %>" /></th>
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
                <asp:Button runat="server" ID="btnSumbit" CommandName="checkConfigurazione" CommandArgument="CLICK" Text="<%$Resources:lblTTVerificaConfig %>" /></td>
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
                <th><asp:Literal runat="server" ID="lblTaskID" Text="<%$Resources:lblTaskID %>" /></th>
                <th><asp:Literal runat="server" ID="lblNomeTask" Text="<%$Resources:lblNomeTask %>" /></th>
                <th><asp:Literal runat="server" ID="lblPostazione" Text="<%$Resources:lblPostazione %>" /></th>
                <th><asp:Literal runat="server" ID="lblNumOp" Text="<%$Resources:lblNumOp %>" /> </th>
                <th><asp:Literal runat="server" ID="lblSetup" Text="<%$Resources:lblSetup %>" /></th>
                <th><asp:Literal runat="server" ID="lblTempoCiclo" Text="<%$Resources:lblTempoCiclo %>" /></th>
                <th><asp:Literal runat="server" ID="lblEarlyStart" Text="<%$Resources:lblEarlyStart %>" /></th>
                <th><asp:Literal runat="server" ID="lblLateStart" Text="<%$Resources:lblLateStart %>" /></th>
                <th><asp:Literal runat="server" ID="lblEarlyFinish" Text="<%$Resources:lblEarlyFinish %>" /></th>
                <th><asp:Literal runat="server" ID="lblLateFinish" Text="<%$Resources:lblLateFinish %>" /></th>
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
                <asp:ImageButton runat="server" ID="btnLANCIA" CommandName="ProductionLaunch" CommandArgument="OK" ImageUrl="~/img/iconComplete.png" Height="100px" ToolTip="<%$Resources:lblBtnLanciaProd %>" />
            </td>
        </tr>
            </tfoot>
        </table>
    </FooterTemplate>
</asp:Repeater>