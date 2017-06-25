<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="statoAvanzamentoArticolo.ascx.cs" Inherits="KIS.Produzione.statoAvanzamentoArticolo1" %>

<script>
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#<%=lblInfo.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>

<h3><asp:Literal runat="server" ID="lblStatoProduzione" Text="<%$Resources:lblStatoProduzione %>" /></h3>


<table>
    <tr><td><asp:Literal runat="server" ID="lblTDIDCommessa" Text="<%$Resources:lblTDIDCommessa %>" /></td><td><asp:Label runat="server" ID="lblIDCommessa" />/<asp:Label runat="server" ID="lblAnnoCommessa" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDProdotto" Text="<%$Resources:lblTDProdotto %>" /></td><td><asp:Label runat="server" ID="lblID" />/<asp:Label runat="server" ID="lblAnno" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDTipoProd" Text="<%$Resources:lblTDTipoProd %>" /></td><td><asp:Label runat="server" ID="lblProcesso" /> - <asp:Label runat="server" ID="lblVariante" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDQtaPrev" Text="<%$Resources:lblTDQtaPrev %>" /></td><td><asp:Label runat="server" ID="lblQuantita" /></td></tr>
    <tr runat="server" id="trQtaProd"><td><asp:Literal runat="server" ID="lblTDQtaProd" Text="<%$Resources:lblTDQtaProd %>" /></td><td><asp:Label runat="server" ID="lblQuantitaProdotta" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDReparto" Text="<%$Resources:lblTDReparto %>" /></td><td><asp:Label runat="server" ID="lblReparto" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDMatricola" Text="<%$Resources:lblTDMatricola %>" /></td><td><asp:Label runat="server" ID="lblMatricola" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDStatus" Text="<%$Resources:lblTDStatus %>" /></td><td><asp:Label runat="server" ID="lblStatus" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDDataFineProd" Text="<%$Resources:lblTDDataFineProd %>" /></td><td><asp:Label runat="server" ID="lblDataFineProduzione" /></td></tr>
    <tr><td><asp:Literal runat="server" ID="lblTDDataConsegna" Text="<%$Resources:lblTDDataConsegna %>" /></td><td><asp:Label runat="server" ID="lblDataConsegna" /></td></tr>
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
                        <td><asp:Literal runat="server" ID="lblTDID" Text="<%$Resources:lblTDID %>" /></td>
                        <td><asp:Literal runat="server" ID="lblTDNome" Text="<%$Resources:lblTDNome %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDStatus %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDQta %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDEarlyStart %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDLateStart %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDEarlyFinish %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDLateFinish %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDPostazione %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDDataInizio %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDDataFine %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDTempoPrevisto %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDTempoEffettivo %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDTempoCicloPrev %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDTempoCicloEff %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDRitardo %>" /></td>
                        <td><asp:Literal runat="server" Text="<%$Resources:lblTDRiesuma %>" /></td>
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
                    <td colspan="12"><asp:label runat="server" CssClass="lead"><asp:literal runat="server" ID="lblTotale" Text="<%$Resources:lblTotale %>"/></asp:label></td>
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