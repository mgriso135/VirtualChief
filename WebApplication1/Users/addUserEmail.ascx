<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUserEmail.ascx.cs" Inherits="KIS.Users.addUserEmail" %>
<asp:ImageButton runat="server" ID="imgShowForm" ImageUrl="~/img/iconAdd.jpg" OnClick="imgShowForm_Click" Height="40" />
Aggiungi un indirizzo e-mail

<table runat="server" id="tblAddEmail" class="table table-bordered">
    <tbody>
    <tr>
        <td>Indirizzo e-mail<br /><asp:TextBox runat="server" ID="txtEmail" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valAddr" ErrorMessage="<br/>* Campo obbligatorio" ForeColor="red" ControlToValidate="txtEmail" ValidationGroup="indirizzo" />
        </td>
        <td>Ambito (casa, ufficio)<br /><asp:TextBox runat="server" ID="txtNote" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ErrorMessage="<br/>* Campo obbligatorio" ForeColor="red" ControlToValidate="txtNote" ValidationGroup="indirizzo" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chkAlarm" />Utilizza questa e-mail per inviarmi gli allarmi
        </td>
    </tr>
        </tbody>
    <tfoot>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="btnSave" ValidationGroup="indirizzo" ImageUrl="~/img/iconSave.jpg" OnClick="btnSave_Click" Height="40" ToolTip="Salva l'indirizzo e-mail" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" OnClick="btnUndo_Click" Height="40" ToolTip="Resetta il form" />
        </td>
    </tr>
        </tfoot>
</table>
<asp:Label runat="server" ID="lbl1" />