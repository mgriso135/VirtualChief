<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUserPhoneNumber.ascx.cs" Inherits="KIS.Users.addUserPhoneNumber" %>
<asp:ImageButton runat="server" ID="imgShowForm" ImageUrl="~/img/iconAdd.jpg" OnClick="imgShowForm_Click" Height="40" />
Aggiungi un numero di telefono

<table runat="server" id="tblAddPhoneNumber" style="border: 1px dashed blue;">
    <tr>
        <td>Numero di telefono<br /><asp:TextBox runat="server" ID="txtPhoneNumber" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valAddr" ErrorMessage="<br/>* Campo obbligatorio" ForeColor="red" ControlToValidate="txtPhoneNumber" ValidationGroup="indirizzo" />
        </td>
        <td>Ambito (casa, ufficio)<br /><asp:TextBox runat="server" ID="txtNote" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ErrorMessage="<br/>* Campo obbligatorio" ForeColor="red" ControlToValidate="txtNote" ValidationGroup="indirizzo" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chkAlarm" />Utilizza questo numero di telefono per inviarmi gli allarmi
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="btnSave" ValidationGroup="indirizzo" ImageUrl="~/img/iconSave.jpg" OnClick="btnSave_Click" Height="40" ToolTip="Salva il numero di telefono" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" OnClick="btnUndo_Click" Height="40" ToolTip="Resetta il form" />
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="lbl1" />