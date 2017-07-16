<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addUserEmail.ascx.cs" Inherits="KIS.Users.addUserEmail" %>
<asp:ImageButton runat="server" ID="imgShowForm" ImageUrl="~/img/iconAdd.jpg" OnClick="imgShowForm_Click" Height="40" />
<asp:Literal runat="server" ID="lblTitleAddEmail" Text="<%$Resources:lblTitleAddEmail %>" />

<table runat="server" id="tblAddEmail" class="table table-bordered">
    <tbody>
    <tr>
        <td><asp:Literal runat="server" ID="lblEmail" Text="<%$Resources:lblEmail %>" />
            <br /><asp:TextBox runat="server" ID="txtEmail" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valAddr" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="red" ControlToValidate="txtEmail" ValidationGroup="indirizzo" />
        </td>
        <td><asp:Literal runat="server" ID="lblAmbito" Text="<%$Resources:lblAmbito %>" /><br /><asp:TextBox runat="server" ID="txtNote" ValidationGroup="indirizzo" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="red" ControlToValidate="txtNote" ValidationGroup="indirizzo" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" ID="chkAlarm" />
            <asp:Literal runat="server" ID="lblSendAlarm" Text="<%$Resources:lblSendAlarm %>" />
        </td>
    </tr>
        </tbody>
    <tfoot>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="btnSave" ValidationGroup="indirizzo" ImageUrl="~/img/iconSave.jpg" OnClick="btnSave_Click" Height="40" ToolTip="<%$Resources:lblSalva %>" />
            <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" OnClick="btnUndo_Click" Height="40" ToolTip="<%$Resources:lblReset %>" />
        </td>
    </tr>
        </tfoot>
</table>
<asp:Label runat="server" ID="lbl1" />