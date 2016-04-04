<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="statoAvanzamentoArticolo.ascx.cs" Inherits="KIS.Produzione.statoAvanzamentoArticolo1" %>

<script>
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#<%=lblInfo.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>

<h3>Stato avanzamento produzione articolo</h3>


<table>
    <tr><td>ID Commessa</td><td><asp:Label runat="server" ID="lblIDCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td></tr>
    <tr><td>Articolo</td><td><asp:Label runat="server" ID="lblID" />/<asp:Label runat="server" ID="lblAnno" /></td></tr>
    <tr><td>Processo</td><td><asp:Label runat="server" ID="lblProcesso" /></td></tr>
    <tr><td>Variante</td><td><asp:Label runat="server" ID="lblVariante" /></td></tr>
    <tr><td>Quantità Prevista</td><td><asp:Label runat="server" ID="lblQuantita" /></td></tr>
    <tr runat="server" id="trQtaProd"><td>Quantità Prodotta</td><td><asp:Label runat="server" ID="lblQuantitaProdotta" /></td></tr>
    <tr><td>Reparto</td><td><asp:Label runat="server" ID="lblReparto" /></td></tr>
    <tr><td>Matricola</td><td><asp:Label runat="server" ID="lblMatricola" /></td></tr>
    <tr><td>Status</td><td><asp:Label runat="server" ID="lblStatus" /></td></tr>
    <tr><td>Data fine produzione</td><td><asp:Label runat="server" ID="lblDataFineProduzione" /></td></tr>
    <tr><td>Data consegna</td><td><asp:Label runat="server" ID="lblDataConsegna" /></td></tr>
    </table>
<br />

<asp:ScriptManager runat="server" ID="ScriptMan1" />
<asp:UpdatePanel runat="server" ID="updStatoTasks" UpdateMode="Conditional">

    <ContentTemplate>
<asp:Label runat="server" ID="lblDataUpdate" />
        <asp:Repeater runat="server" ID="rptTasks" OnItemDataBound="rptTasks_ItemDataBound" OnItemCommand="rptTasks_ItemCommand">
            <HeaderTemplate>
                <table style="border:1px dashed blue">
                    <tr style="font-family: Calibri; font-size: 18px; font-weight: bold;">
                        <td>ID</td>
                        <td>Nome</td>
                        <td>Status</td>
                        <td>Quantità</td>
                        <td>Early Start</td>
                        <td>Late Start</td>
                        <td>Early Finish</td>
                        <td>Late Finish</td>
                        <td>Postazione</td>
                        <td>Data effettiva inizio task</td>
                        <td>Data effettiva fine task</td>
                        <td>Tempo di lavoro previsto (ore)</td>
                        <td>Tempo di lavoro effettivo (ore)</td>
                        <td>Tempo ciclo previsto (ore)</td>
                        <td>Tempo ciclo effettivo (ore)</td>
                        <td>Ritardo (ore)</td>
                        <td>Riesuma</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-family: Calibri; font-size: 14px;">
                    <td><asp:HiddenField runat="server" ID="hTaskID" Value='<%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Status") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %>&nbsp;(<%# DataBinder.Eval(Container.DataItem, "QuantitaPrevista") %>)</td>
                    <td><%# DataBinder.Eval(Container.DataItem, "EarlyStart") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "LateStart") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "EarlyFinish") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "LateFinish") %></td>
                    <td><asp:Label runat="server" ID="lblPostazione" /></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "DataInizioTask") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "DataFineTask") %></td>
                    <td>
                        <%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroPrevisto")).TotalHours.ToString("F2") %>
                    </td>
                    <td>
                        <a href='<%# "viewDetailsTaskProduzione.aspx?id=" + DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' target="_blank">
                            <asp:Image runat="server" ID="imgViewDetails" ImageUrl="~/img/iconView.png" Height="20" />
                        </a>
                        <%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroEffettivo")).TotalHours.ToString("F2") %>
                    </td>
                    <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoC")).TotalHours.ToString("F2")%></td>
                    <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoCicloEffettivo")).TotalHours.ToString("F2")%>
                    </td>
                    <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "Ritardo")).TotalHours.ToString("F2")%>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnRiesuma" Width="30" ImageUrl="~/img/iconExhume.png" CommandName="riesuma" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                <tr>
                    <td colspan="12"><asp:label runat="server" CssClass="lead">TOTALE</asp:label></td>
                    <td colspan="5">
                        <asp:Label CssClass="lead" runat="server" ID="lblTotTempoDiLavoro" />
                    </td>
                </tr>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Label runat="server" ID="lblInfo" />
        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="60000" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Label runat="server" ID="lbl1" />