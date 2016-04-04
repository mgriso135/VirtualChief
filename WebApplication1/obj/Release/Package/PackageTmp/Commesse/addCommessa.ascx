<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addCommessa.ascx.cs" Inherits="KIS.Commesse.addCommessa" %>
<asp:ImageButton CssClass="img-rounded" runat="server" ID="btnShowFrmAddCommessa" Height="40px" ImageUrl="/img/iconAdd.jpg" ToolTip="Aggiungi una nuova commessa" OnClick="btnShowFrmAddCommessa_Click" />
Aggiungi una commessa<br />

<table runat="server" id="frmAddCommessa" class="table table-bordered table-hover">
    <tr><td>Cliente</td><td>
        <asp:DropDownList runat="server" ID="ddlCliente" />
        <a href="../Clienti/AddCliente.aspx">Aggiungi un nuovo cliente</a>
        </td></tr>
    <tr><td>Note</td><td><asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" />
        <asp:RequiredFieldValidator runat="server" ID="valNote" ForeColor="Red" ControlToValidate="txtNote" ErrorMessage="* il campo non può essere vuoto" ValidationGroup="commessa" />
                     </td></tr>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="40" ToolTip="Salva la commessa" OnClick="btnSave_Click" ValidationGroup="commessa" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconUndo.png" Height="40" ToolTip="Resetta il form" OnClick="btnUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />