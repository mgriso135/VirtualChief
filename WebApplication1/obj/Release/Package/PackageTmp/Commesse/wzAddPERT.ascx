<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzAddPERT.ascx.cs" Inherits="KIS.Commesse.wzAddPERT1" %>
<div class="span6">
    <asp:Label runat="server" ID="lbl1" />
    <h3><asp:Label runat="server" ID="lblTitle" /></h3>
    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Label runat="server" ID="lblAccProdStandard" meta:resourcekey="lblAccProdStandard" />
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd1">
              <ContentTemplate>
            <table class="table table-striped table-hover" runat="server" id="tblAddPERT">
                    <tr>
                        <td style="vertical-align: middle;">
                            <h4><asp:Label runat="server" ID="lblAccProdStandard1" meta:resourcekey="lblAccProdStandard" /></h4>
                            <asp:Label runat="server" ID="lblSelezionaProdotto" meta:resourcekey="lblSelezionaProdotto" />:&nbsp;<asp:DropDownList runat="server" ID="ddlAddProdStandard">
                                <asp:ListItem Value="" Text="<%Resources:ddlSelProdotto %>"></asp:ListItem>
                                                     </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblQuantita" meta:resourcekey="lblQuantita" />:&nbsp;<asp:TextBox runat="server" ID="txtStdQty" ValidationGroup="StandardProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStdQty" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="StandardProd" />
                            <br />
                            <asp:Label runat="server" ID="lblMatricola" Text="<%$Resources:lblMatricola %>" />:&nbsp;<asp:TextBox runat="server" ID="txtStdMatricola" />
                            <br />
                            <asp:Label runat="server" ID="lblFiltraCliente" meta:resourcekey="lblFiltraCliente" />:&nbsp;<asp:DropDownList runat="server" ID="ddlStdFiltroCliente" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlStdFiltroCliente_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="" Text="<%$Resources:lblNoSelection %>"></asp:ListItem>
                    </asp:DropDownList></td>
                        <td style="vertical-align: middle;">
                            <asp:ImageButton runat="server" ID="addProdStd" OnClick="addProdStd_Click" ImageUrl="~/img/iconArrowRight.png" ToolTip="<%$Resources:lblTTAddProdottoEsistente %>" Height="40px" ValidationGroup="StandardProd" />
                            </td>
                            </tr>
                </table>
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
                </div>
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Label runat="server" ID="lblAccAggiungiProdotto" meta:resourcekey="lblAccAggiungiProdotto" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd3">
              <ContentTemplate>
    <table>
                    <tr>
                        <td style="vertical-align: middle;">
                            <h4><asp:Label runat="server" ID="Label1" meta:resourcekey="lblAccAggiungiProdotto" /></h4>
                            <asp:Label runat="server" ID="lblNomeNuovoProd" meta:resourcekey="lblNomeNuovoProd" />:&nbsp;<asp:textbox runat="server" ID="txtNomeBlankProd" ValidationGroup="BlankProd" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNomeBlankProd" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="BlankProd" />
                            <br />
                            <asp:Label runat="server" ID="lblDescNuovoProd" meta:resourcekey="lblDescNuovoProd" />:&nbsp;<asp:textbox runat="server" ID="txtDescBlankProd" ValidationGroup="BlankProd" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescBlankProd" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="BlankProd" />
                            <br />
                            <asp:Label runat="server" ID="lblQuantita2" meta:resourcekey="lblQuantita" />:&nbsp;<asp:TextBox runat="server" ID="txtQtyBlank" ValidationGroup="BlankProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtQtyBlank" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="BlankProd" />
                            <br />
                            <asp:Label runat="server" ID="lblMatricola2" Text="<%$Resources:lblMatricola %>" />:&nbsp;<asp:TextBox runat="server" ID="txtMatricolaBlank" />
                        </td>
                        <td style="vertical-align: middle;">
    <asp:ImageButton runat="server" ID="addVariante" OnClick="addVariante_Click1" ImageUrl="~/img/iconArrowRight.png" ToolTip="<%$Resources:lblTTAddNuovoProdottoVuoto %>" Height="40px" ValidationGroup="BlankProd" />
                            </td>
    </tr>
        <tr runat="server" id="trBlankConfirm" visible="false">
            <td colspan="2"><div class="text-warning">
                <asp:Label runat="server" ID="lblErrorDuplicate" meta:resourcekey="lblErrorDuplicate" />
                <br /></div>
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
          <asp:Label runat="server" ID="lblAccAggiungiProdCopia" meta:resourcekey="lblAccAggiungiProdCopia" />
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
                
    <h4><asp:Label runat="server" ID="lblAccAggiungiProdCopia1" meta:resourcekey="lblAccAggiungiProdCopia" /></h4>
                <asp:Label runat="server" ID="lblNomeNuovoProd2" meta:resourcekey="lblNomeNuovoProd" />:&nbsp;<asp:TextBox runat="server" ID="txtNomeCopiaProd" ValidationGroup="CopiaProd" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNomeCopiaProd" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="CopiaProd" />
                <br />
                <asp:Label runat="server" ID="lblDescNuovoProd2" meta:resourcekey="lblDescNuovoProd" />:&nbsp;<asp:TextBox runat="server" ID="txtDescCopiaProd"  TextMode="MultiLine" ValidationGroup="CopiaProd" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDescCopiaProd" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="CopiaProd" />
                <br />
                <asp:Label runat="server" ID="lblQuantita3" meta:resourcekey="lblQuantita" />:&nbsp;<asp:TextBox runat="server" ID="txtQtyCopiaProd" ValidationGroup="CopiaProd" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQtyCopiaProd" ForeColor="Red" ErrorMessage="<%$Resources:valReqField %>" ValidationGroup="CopiaProd" />
                <br />
                <asp:Label runat="server" ID="Label2" Text="<%$Resources:lblMatricola %>" />:&nbsp;<asp:TextBox runat="server" ID="txtMatricolaCopiaProd" />
                <br />
                <asp:Label runat="server" ID="lblSelBaseProd" meta:resourcekey="lblSelBaseProd" />:&nbsp;<asp:DropDownList runat="server" ID="ddlCopiaPERT" />
                <asp:Label runat="server" ID="lblFiltraCliente3" meta:resourcekey="lblFiltraCliente" />:&nbsp;<asp:DropDownList runat="server" ID="ddlCopiaPERTClienti" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCopiaPERTClienti_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="" Text="<%$Resources:lblNoSelection %>"></asp:ListItem>
                    </asp:DropDownList>
            </asp:Label>
            <br /><asp:checkbox runat="server" ID="chkCopia" Checked="true" Enabled="false" /><asp:Label runat="server" ID="lblRiutilizzaTasks" meta:resourcekey="lblRiutilizzaTasks" /><br />
            <asp:checkbox runat="server" ID="chkCopiaTC" Checked="true" /><asp:Label runat="server" ID="lblCopiaTempiCiclo" meta:resourcekey="lblCopiaTempiCiclo" /><br />
            <asp:checkbox runat="server" ID="chkCopiaReparti" Checked="true" OnCheckedChanged="chkCopiaReparti_CheckedChanged" AutoPostBack="true" /><asp:Label runat="server" ID="lblCopiaReparti" meta:resourcekey="lblCopiaReparti" /><br />
            <asp:checkbox runat="server" ID="chkCopiaPostazioni" Checked="true" OnCheckedChanged="chkCopiaPostazioni_CheckedChanged" AutoPostBack="true" /><asp:Label runat="server" ID="lblCopiaPostazioni" meta:resourcekey="lblCopiaPostazioni" />
                        </td><td style="vertical-align: middle;">
                        <asp:ImageButton runat="server" ID="btnCopiaPERT" ImageUrl="~/img/iconArrowRight.png" Height="40px" OnClick="btnCopiaPERT_Click" ValidationGroup="CopiaProd"  />
                        <asp:Label runat="server" ID="lblCopiaPERTLog" />
                        </td>
                    </tr>
        <tr runat="server" id="trCopyConfirm" visible="false">
            <td colspan="2"><div class="text-warning">
                <asp:Label runat="server" ID="lblErrorDuplicate2" meta:resourcekey="lblErrorDuplicate" /><br /></div>
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
