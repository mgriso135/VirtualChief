<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configOrderStatusCustomer.ascx.cs" Inherits="KIS.Admin.configOrderStatusCustomer1" %>

<h3>CONFIGURAZIONE CLIENTE</h3>

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
<div class="row-fluid">
    <div class="span4">
    <asp:ImageButton runat="server" Height="40" ImageUrl="~/img/iconUndo.png" ID="btnResetIDCommessa" OnClick="btnReset_Click" />Resetta la configurazione</div>
    <div class="span8">
        <asp:Label runat="server" ID="lbl1" />
        </div>
    </div>
<div class="row-fluid" runat="server" id="frmConfigReport">
    <div class="span12">
<table class="table table-bordered table-condensed table-striped">
    <thead>
        <tr><th><asp:label runat="server" id="lblParametro" meta:resourcekey="lblParametro" /></th>
            <th><asp:label runat="server" id="lblOpzioni" meta:resourcekey="lblOpzioni" /></th></tr>
    </thead>
    <tbody>
    <tr>
        <td><asp:label runat="server" id="lblCommessa_IDCommessa" meta:resourcekey="lblCommessa_IDCommessa" /></td>
        <td><asp:DropDownList runat="server" ID="ddlIDCommessa" AutoPostBack="true" OnSelectedIndexChanged="ddlIDCommessa_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td><asp:label runat="server" id="lblCommessa_Cliente" meta:resourcekey="lblCommessa_Cliente" /></td>
        <td><asp:DropDownList runat="server" ID="ddlCliente" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
        <tr>
            <td><asp:label runat="server" id="lblCommessa_DataInserimento" meta:resourcekey="lblCommessa_DataInserimento" /></td>
            <td><asp:DropDownList runat="server" ID="ddlDataInserimentoOrdine" AutoPostBack="true" OnSelectedIndexChanged="ddlDataInserimentoOrdine_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblCommessa_Note" meta:resourcekey="lblCommessa_Note" /></td>
            <td><asp:DropDownList runat="server" ID="ddlNoteOrdine" AutoPostBack="true" OnSelectedIndexChanged="ddlNoteOrdine_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_IDProdotto" meta:resourcekey="lblProdotto_IDProdotto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlIDProdotto" AutoPostBack="true" OnSelectedIndexChanged="ddlIDProdotto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_LineaProdotto" meta:resourcekey="lblProdotto_LineaProdotto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlNomeProdotto" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeProdotto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_NomeProdotto" meta:resourcekey="lblProdotto_NomeProdotto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlNomeVariante" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeVariante_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_Matricola" meta:resourcekey="lblProdotto_Matricola" /></td>
            <td><asp:DropDownList runat="server" ID="ddlMatricola" AutoPostBack="true" OnSelectedIndexChanged="ddlMatricola_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_Status" meta:resourcekey="lblProdotto_Status" /></td>
            <td><asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_Reparto" meta:resourcekey="lblProdotto_Reparto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlReparto" AutoPostBack="true" OnSelectedIndexChanged="ddlReparto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_DataPrevConsegna" meta:resourcekey="lblProdotto_DataPrevConsegna" /></td>
            <td><asp:DropDownList runat="server" ID="ddlDataPrevistaConsegna" AutoPostBack="true" OnSelectedIndexChanged="ddlDataPrevistaConsegna_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_DataPrevFineProd" meta:resourcekey="lblProdotto_DataPrevFineProd" /></td>
            <td><asp:DropDownList runat="server" ID="ddlDataPrevistaFineProduzione" AutoPostBack="true" OnSelectedIndexChanged="ddlDataPrevistaFineProduzione_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_EarlyStart" meta:resourcekey="lblProdotto_EarlyStart" /></td>
            <td><asp:DropDownList runat="server" ID="ddlEarlyStart" AutoPostBack="true" OnSelectedIndexChanged="ddlEarlyStart_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_EarlyFinish" meta:resourcekey="lblProdotto_EarlyFinish" /></td>
            <td><asp:DropDownList runat="server" ID="ddlEarlyFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlEarlyFinish_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_LateStart" meta:resourcekey="lblProdotto_LateStart" /></td>
            <td><asp:DropDownList runat="server" ID="ddlLateStart" AutoPostBack="true" OnSelectedIndexChanged="ddlLateStart_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_LateFinish" meta:resourcekey="lblProdotto_LateFinish" /></td>
            <td><asp:DropDownList runat="server" ID="ddlLateFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlLateFinish_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_QtaPrevista" meta:resourcekey="lblProdotto_QtaPrevista" /></td>
            <td><asp:DropDownList runat="server" ID="ddlQuantita" AutoPostBack="true" OnSelectedIndexChanged="ddlQuantita_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_QtaProdotta" meta:resourcekey="lblProdotto_QtaProdotta" /></td>
            <td><asp:DropDownList runat="server" ID="ddlQuantitaProdotta" AutoPostBack="true" OnSelectedIndexChanged="ddlQuantitaProdotta_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_Ritardo" meta:resourcekey="lblProdotto_Ritardo" /></td>
            <td><asp:DropDownList runat="server" ID="ddlRitardo" AutoPostBack="true" OnSelectedIndexChanged="ddlRitardo_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_TempoLavTotale" meta:resourcekey="lblProdotto_TempoLavTotale" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTempoDiLavoroTotale" AutoPostBack="true" OnSelectedIndexChanged="ddlTempoDiLavoroTotale_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_LeadTime" meta:resourcekey="lblProdotto_LeadTime" /></td>
            <td><asp:DropDownList runat="server" ID="ddlLeadTime" AutoPostBack="true" OnSelectedIndexChanged="ddlLeadTime_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_TempoLavPrevisto" meta:resourcekey="lblProdotto_TempoLavPrevisto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTempoDiLavoroPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlTempoDiLavoroPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_IndComplTasks" meta:resourcekey="lblProdotto_IndComplTasks" /></td>
            <td><asp:DropDownList runat="server" ID="ddlIndicatoreCompletamentoTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlIndicatoreCompletamentoTasks_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_IndComplTasksTempo" meta:resourcekey="lblProdotto_IndComplTasksTempo" /></td>
            <td><asp:DropDownList runat="server" ID="ddlIndicatoreCompletamentoTempoPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlIndicatoreCompletamentoTempoPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_GANTT" meta:resourcekey="lblProdotto_GANTT" /></td>
            <td><asp:DropDownList runat="server" ID="ddlViewGanttTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlViewGanttTasks_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblProdotto_ElencoTasks" meta:resourcekey="lblProdotto_ElencoTasks" /></td>
            <td><asp:DropDownList runat="server" ID="ddlViewElencoTasks" AutoPostBack="true" OnSelectedIndexChanged="ddlViewElencoTasks_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr><td colspan="2"><asp:label runat="server" id="lblTaskInfo" meta:resourcekey="lblTaskInfo" /></td></tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_ID" meta:resourcekey="lblTask_ID" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_ID" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_ID_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_Nome" meta:resourcekey="lblTask_Nome" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Nome" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Nome_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_Descrizione" meta:resourcekey="lblTask_Descrizione" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Descrizione" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Descrizione_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_Postazione" meta:resourcekey="lblTask_Postazione" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Postazione" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Postazione_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_EarlyStart" meta:resourcekey="lblTask_EarlyStart" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_EarlyStart" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_EarlyStart_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_LateStart" meta:resourcekey="lblTask_LateStart" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_LateStart" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_LateStart_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_EarlyFinish" meta:resourcekey="lblTask_EarlyFinish" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_EarlyFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_EarlyFinish_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_LateFinish" meta:resourcekey="lblTask_LateFinish" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_LateFinish" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_LateFinish_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_NOps" meta:resourcekey="lblTask_NOps" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_NOperatori" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_NOperatori_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_TempoCiclo" meta:resourcekey="lblTask_TempoCiclo" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoCiclo" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoCiclo_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_TempoLavPrevisto" meta:resourcekey="lblTask_TempoLavPrevisto" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoDiLavoroPrevisto" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoDiLavoroPrevisto_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_TempoLavEffettivo" meta:resourcekey="lblTask_TempoLavEffettivo" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_TempoDiLavoroEffettivo" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_TempoDiLavoroEffettivo_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_Status" meta:resourcekey="lblTask_Status" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_Status" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_Status_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:label runat="server" id="lblTask_QtaProdotta" meta:resourcekey="lblTask_QtaProdotta" /></td>
            <td><asp:DropDownList runat="server" ID="ddlTask_QuantitaProdotta" AutoPostBack="true" OnSelectedIndexChanged="ddlTask_QuantitaProdotta_SelectedIndexChanged">
            <asp:ListItem value="true" Text="<%$resources:Visibile %>"></asp:ListItem>
            <asp:ListItem value="false" Text="<%$resources:NonVisibile %>"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        </tbody>
</table>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>