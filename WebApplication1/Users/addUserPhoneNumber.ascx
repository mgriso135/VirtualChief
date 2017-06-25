<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUserPhoneNumber.ascx.cs" Inherits="KIS.Users.addUserPhoneNumber" %>
<asp:ImageButton runat="server" ID="imgShowForm" ImageUrl="~/img/iconAdd.jpg" OnClick="imgShowForm_Click" Height="40" />
<asp:Literal runat="server" ID="lblAddPhoneNumber" Text="<%$Resources:lblAddPhoneNumber %>" />

<table runat="server" id="tblAddPhoneNumber" style="border: 1px dashed blue;">
    <tr>
        <td><asp:Literal runat="server" ID="lblPhoneNumber" Text="<%$Resources:lblPhoneNumber %>" />
            <br /><asp:TextBox runat="server" ID="txtPhoneNumber" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valAddr" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="red" ControlToValidate="txtPhoneNumber" ValidationGroup="indirizzo" />
        </td>
        <td><asp:Literal runat="server" ID="lblAmbito" Text="<%$Resources:lblAmbito %>" />
            <br /><asp:TextBox runat="server" ID="txtNote" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="red" ControlToValidate="txtNote" ValidationGroup="indirizzo" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chkAlarm" />
            <asp:Literal runat="server" ID="lblUtilizzAllarmi" Text="<%$Resources:lblUtilizzAllarmi %>" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="btnSave" ValidationGroup="indirizzo" ImageUrl="~/img/iconSave.jpg" OnClick="btnSave_Click" Height="40" ToolTip="<%$Resources:lblSalvaNum %>" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" OnClick="btnUndo_Click" Height="40" ToolTip="<%$Resources:lblResetForm %>" />
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="lbl1" />