<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configOrderStatusBase.ascx.cs" Inherits="KIS.Admin.configOrderStatusBase1" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script>
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                   $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow");
                });                
    });
</script>
<div class="row-fluid" runat="server" id="frmConfigReport">
    <div class="span12">
        <asp:Label runat="server" ID="lbl1" />
<table class="table table-bordered table-condensed table-striped">
    <thead>
        <tr><th>Parametro</th><th>Opzioni</th></tr>
    </thead>
    <tbody>
    <tr>
        <td>Commessa: ID Commessa</td>
        <td><asp:DropDownList runat="server" ID="ddlIDCommessa" AutoPostBack="true" OnSelectedIndexChanged="ddlIDCommessa_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>Commessa: Cliente</td>
        <td><asp:DropDownList runat="server" ID="ddlCliente" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
        <tr>
            <td>Commessa: Data inserimento ordine</td>
            <td><asp:DropDownList runat="server" ID="ddlDataInserimentoOrdine" AutoPostBack="true" OnSelectedIndexChanged="ddlDataInserimentoOrdine_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Commessa: Note</td>
            <td><asp:DropDownList runat="server" ID="ddlNoteOrdine" AutoPostBack="true" OnSelectedIndexChanged="ddlNoteOrdine_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: ID Prodotto</td>
            <td><asp:DropDownList runat="server" ID="ddlIDProdotto" AutoPostBack="true" OnSelectedIndexChanged="ddlIDProdotto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Linea Prodotto</td>
            <td><asp:DropDownList runat="server" ID="ddlNomeProdotto" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeProdotto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Nome Prodotto</td>
            <td><asp:DropDownList runat="server" ID="ddlNomeVariante" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeVariante_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Matricola</td>
            <td><asp:DropDownList runat="server" ID="ddlMatricola" AutoPostBack="true" OnSelectedIndexChanged="ddlMatricola_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Status</td>
            <td><asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Reparto</td>
            <td><asp:DropDownList runat="server" ID="ddlReparto" AutoPostBack="true" OnSelectedIndexChanged="ddlReparto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Data prevista consegna</td>
            <td><asp:DropDownList runat="server" ID="ddlDataPrevistaConsegna" AutoPostBack="true" OnSelectedIndexChanged="ddlDataPrevistaConsegna_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Data prevista fine produzione</td>
            <td><asp:DropDownList runat="server" ID="ddlDataPrevistaFineProduzione" AutoPostBack="true" OnSelectedIndexChanged="ddlDataPrevistaFineProduzione_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Early Start</td>
            <td><asp:DropDownList runat="server" ID="ddlEarlyStart" AutoPostBack="true" OnSelectedIndexChanged="ddlEarlyStart_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Early Finish</td>
            <td><asp:DropDownList runat="server" ID="ddlEarlyFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlEarlyFinish_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Late Start</td>
            <td><asp:DropDownList runat="server" ID="ddlLateStart" AutoPostBack="true" OnSelectedIndexChanged="ddlLateStart_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Late Finish</td>
            <td><asp:DropDownList runat="server" ID="ddlLateFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlLateFinish_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Quantità prevista</td>
            <td><asp:DropDownList runat="server" ID="ddlQuantita" AutoPostBack="true" OnSelectedIndexChanged="ddlQuantita_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Quantità prodotta</td>
            <td><asp:DropDownList runat="server" ID="ddlQuantitaProdotta" AutoPostBack="true" OnSelectedIndexChanged="ddlQuantitaProdotta_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Ritardo</td>
            <td><asp:DropDownList runat="server" ID="ddlRitardo" AutoPostBack="true" OnSelectedIndexChanged="ddlRitardo_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Tempo di Lavoro totale</td>
            <td><asp:DropDownList runat="server" ID="ddlTempoDiLavoroTotale" AutoPostBack="true" OnSelectedIndexChanged="ddlTempoDiLavoroTotale_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Lead time</td>
            <td><asp:DropDownList runat="server" ID="ddlLeadTime" AutoPostBack="true" OnSelectedIndexChanged="ddlLeadTime_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Tempo di Lavoro Previsto</td>
            <td><asp:DropDownList runat="server" ID="ddlTempoDiLavoroPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlTempoDiLavoroPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Indicatore Completamento Tasks (n° task completati / n° task totali)</td>
            <td><asp:DropDownList runat="server" ID="ddlIndicatoreCompletamentoTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlIndicatoreCompletamentoTasks_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Indicatore Completamento Tasks su tempo previsto (Tempo lavoto tasks terminati / Tempo di lavoro totale)</td>
            <td><asp:DropDownList runat="server" ID="ddlIndicatoreCompletamentoTempoPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlIndicatoreCompletamentoTempoPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: GANTT avanzamento tasks</td>
            <td><asp:DropDownList runat="server" ID="ddlViewGanttTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlViewGanttTasks_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Prodotto: Elenco tasks</td>
            <td><asp:DropDownList runat="server" ID="ddlViewElencoTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlViewElencoTasks_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr><td colspan="2">Se parametro "Prodotto: Elenco tasks" è impostato su visibile:</td></tr>
        <tr>
            <td>Task: ID</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_ID_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Nome</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Nome" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Nome_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Descrizione</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Descrizione" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Descrizione_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Postazione</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Postazione" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Postazione_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: EarlyStart</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_EarlyStart" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_EarlyStart_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: LateStart</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_LateStart" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_LateStart_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: EarlyFinish</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_EarlyFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_EarlyFinish_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: LateFinish</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_LateFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_LateFinish_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Numero Operatori</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_NOperatori" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_NOperatori_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Tempo Ciclo</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoCiclo" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoCiclo_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Tempo di Lavoro Previsto</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoDiLavoroPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoDiLavoroPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Tempo di Lavoro Effettivo</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoDiLavoroEffettivo" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoDiLavoroEffettivo_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Status</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Status" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Status_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Task: Quantità Prodotta</td>
            <td><asp:DropDownList runat="server" ID="ddlTask_QuantitaProdotta" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_QuantitaProdotta_SelectedIndexChanged">
            <asp:ListItem value="true">Visibile</asp:ListItem>
            <asp:ListItem value="false">Non visibile</asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        </tbody>
</table>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>