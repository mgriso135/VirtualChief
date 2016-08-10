<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzAddPERT.ascx.cs" Inherits="KIS.Commesse.wzAddPERT1" %>
<div class="span6">
    <asp:Label runat="server" ID="lbl1" />
    <h3><asp:Label runat="server" ID="lblTitle" /></h3>
    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Aggiungi prodotto standard
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd1">
              <ContentTemplate>
            <table class="table table-striped table-hover" runat="server" id="tblAddPERT">
                    <tr>
                        <td style="vertical-align: middle;">
                            <h4>Aggiungi prodotto standard</h4>
                            Seleziona prodotto:&nbsp;<asp:DropDownList runat="server" ID="ddlAddProdStandard">
                                <asp:ListItem Value="">Seleziona un prodotto</asp:ListItem>
                                                     </asp:DropDownList>
                            <br />
                            Quantità:&nbsp;<asp:TextBox runat="server" ID="txtStdQty" ValidationGroup="StandardProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStdQty" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="StandardProd" />
                            <br />
                            Filtra per cliente:&nbsp;<asp:DropDownList runat="server" ID="ddlStdFiltroCliente" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlStdFiltroCliente_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="">Nessuna selezione</asp:ListItem>
                    </asp:DropDownList></td>
                        <td style="vertical-align: middle;">
                            <asp:ImageButton runat="server" ID="addProdStd" OnClick="addProdStd_Click" ImageUrl="~/img/iconArrowRight.png" ToolTip="Aggiungi un prodotto esistente" Height="40px" ValidationGroup="StandardProd" />
                            </td>
                            </tr>
                </table>
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
                </div>
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Aggiungi nuovo prodotto
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd3">
              <ContentTemplate>
    <table>
                    <tr>
                        <td style="vertical-align: middle;">
                            <h4>Nuovo prodotto</h4>
                            Nome del nuovo prodotto:&nbsp;<asp:textbox runat="server" ID="txtNomeBlankProd" ValidationGroup="BlankProd" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNomeBlankProd" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="BlankProd" />
                            <br />
                            Descrizione del nuovo prodotto:&nbsp;<asp:textbox runat="server" ID="txtDescBlankProd" ValidationGroup="BlankProd" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescBlankProd" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="BlankProd" />
                            <br />
                            Quantità:&nbsp;<asp:TextBox runat="server" ID="txtQtyBlank" ValidationGroup="BlankProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtQtyBlank" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="BlankProd" />
                        </td>
                        <td style="vertical-align: middle;">
    <asp:ImageButton runat="server" ID="addVariante" OnClick="addVariante_Click1" ImageUrl="~/img/iconArrowRight.png" ToolTip="Aggiungi una nuova variante vuota" Height="40px" ValidationGroup="BlankProd" />
                            </td>
    </tr>
        <tr runat="server" id="trBlankConfirm" visible="false">
            <td colspan="2"><div class="text-warning">Attenzione: esiste già un prodotto con lo stesso nome. <br />
                E' consigliabile utilizzare il box <i>Aggiungi prodotto standard.</i><br />
                Sei sicuro di voler proseguire?<br /></div>
                <asp:ImageButton runat="server" ID="imgBlankConfirmOK" ImageUrl="~/img/iconComplete.png" Width="30" OnClick="addVariante_Click" />
                <asp:ImageButton runat="server" ID="imgBlankConfirmKO" ImageUrl="~/img/iconKO.jpg" Width="30" OnClick="imgBlankConfirmKO_Click" />
            </td>
        </tr>
        </table>
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
</div>
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          Aggiungi nuovo prodotto copiandolo da uno esistente
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd2">
              <ContentTemplate>
    <table>
                <tr runat="server" id="trCopiaPERT">
                    <td>
            <asp:Label runat="server" ID="lblCopiaPERT">
                
    <h4>Crea prodotto copiandolo da uno esistente</h4>
                Nome del nuovo prodotto:&nbsp;<asp:TextBox runat="server" ID="txtNomeCopiaProd" ValidationGroup="CopiaProd" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNomeCopiaProd" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="CopiaProd" />
                <br />
                Descrizione del nuovo prodotto:&nbsp;<asp:TextBox runat="server" ID="txtDescCopiaProd"  TextMode="MultiLine" ValidationGroup="CopiaProd" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDescCopiaProd" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="CopiaProd" />
                <br />
                Quantità:&nbsp;<asp:TextBox runat="server" ID="txtQtyCopiaProd" ValidationGroup="CopiaProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQtyCopiaProd" ForeColor="Red" ErrorMessage="* Campo obbligatorio" ValidationGroup="CopiaProd" />
                <br />
                Seleziona prodotto base:&nbsp;<asp:DropDownList runat="server" ID="ddlCopiaPERT" />
                Filtra per cliente:&nbsp;<asp:DropDownList runat="server" ID="ddlCopiaPERTClienti" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCopiaPERTClienti_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="">Nessuna selezione</asp:ListItem>
                    </asp:DropDownList>
            </asp:Label>
            <br /><asp:checkbox runat="server" ID="chkCopia" Checked="true" Enabled="false" />Riutilizza i tasks esistenti<br />
            <asp:checkbox runat="server" ID="chkCopiaTC" Checked="true" />Copia tempi ciclo<br />
            <asp:checkbox runat="server" ID="chkCopiaReparti" Checked="true" OnCheckedChanged="chkCopiaReparti_CheckedChanged" AutoPostBack="true" />Copia reparti assegnati al prodotto<br />
            <asp:checkbox runat="server" ID="chkCopiaPostazioni" Checked="true" OnCheckedChanged="chkCopiaPostazioni_CheckedChanged" AutoPostBack="true" />Copia le postazioni assegnate ai tasks
                        </td><td style="vertical-align: middle;">
                        <asp:ImageButton runat="server" ID="btnCopiaPERT" ImageUrl="~/img/iconArrowRight.png" Height="40px" OnClick="btnCopiaPERT_Click" ValidationGroup="CopiaProd"  />
                        <asp:Label runat="server" ID="lblCopiaPERTLog" />
                        </td>
                    </tr>
        <tr runat="server" id="trCopyConfirm" visible="false">
            <td colspan="2"><div class="text-warning">Attenzione: esiste già un prodotto con lo stesso nome. <br />
                E' consigliabile utilizzare il box <i>Aggiungi prodotto standard.</i><br />
                Sei sicuro di voler proseguire?<br /></div>
                <asp:ImageButton runat="server" ID="imgCopyConfirmOK" ImageUrl="~/img/iconComplete.png" Width="30" OnClick="imgCopyConfirmOK_Click" />
                <asp:ImageButton runat="server" ID="imgCopyConfirmKO" ImageUrl="~/img/iconKO.jpg" Width="30" OnClick="imgCopyConfirmKO_Click" />
            </td>
        </tr>
            </table>
                  </ContentTemplate>
              </asp:UpdatePanel>
        </div>
            </div>
            </div>
        </div>
    </div>
