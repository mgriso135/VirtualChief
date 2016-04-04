<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzAddCommessa.ascx.cs" Inherits="KIS.Commesse.wzAddCommessa1" %>

<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="frmAddCommessa" class="table table-bordered table-hover">
    <tr><td>Cliente</td><td>
        <asp:DropDownList runat="server" ID="ddlCliente" /><br />
        <a href="../Commesse/wzAddCliente.aspx">Aggiungi un nuovo cliente</a>
        </td></tr>
    <tr><td>Note</td><td><asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" />
        <asp:RequiredFieldValidator runat="server" ID="valNote" ForeColor="Red" ControlToValidate="txtNote" ErrorMessage="* il campo non può essere vuoto" ValidationGroup="commessa" />
                     </td></tr>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:ImageButton runat="server" ID="imgBtnSave" ImageUrl="~/img/iconArrowRight.png" OnClick="btnSave_Click" Height="40" ValidationGroup="commessa" />
        </td></tr>
</table>
