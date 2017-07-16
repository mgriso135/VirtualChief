<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuAddMainVoce.ascx.cs" Inherits="KIS.Admin.menuAddMainVoce" %>
<asp:ImageButton runat="server" ID="imgShowFormAddMainMenu" ImageUrl="~/img/iconAdd2.png" OnClick="imgShowFormAddMainMenu_Click" Height="50px" ToolTip="Aggiungi una voce di menu principale" />
<table runat="server" id="tblFormAdd" visible="false" class="table table-bordered">
    <tr>
        <td><asp:label runat="server" id="lblTitolo" meta:resourcekey="lblTitolo" /></td>
        <td><asp:TextBox runat="server" ID="txtTitolo" ValidationGroup="val" />
            <asp:RequiredFieldValidator runat="server" ID="valTitolo" ForeColor="Red" ErrorMessage="<%$ resources:lblValValoreRichiesto %>" ValidationGroup="val" ControlToValidate="txtTitolo" />
        </td>
    </tr>
    <tr>
        <td><asp:label runat="server" id="lblDescrizione" meta:resourcekey="lblDescrizione" /></td>
        <td><asp:TextBox runat="server" ID="txtDesc" ValidationGroup="val" />
            <asp:RequiredFieldValidator runat="server" ID="valDesc" ForeColor="Red" ErrorMessage="<%$ resources:lblValValoreRichiesto %>" ValidationGroup="val" ControlToValidate="txtDesc" />

        </td>
    </tr>
    <tr>
        <td><asp:label runat="server" id="lblURL" meta:resourcekey="lblURL" /></td>
        <td>
            <asp:TextBox runat="server" ID="txtURL" ValidationGroup="val" />
            <asp:RequiredFieldValidator runat="server" ID="valURL" ForeColor="Red" ErrorMessage="<%$ resources:lblValValoreRichiesto %>" ValidationGroup="val" ControlToValidate="txtURL" />
        </td>
    </tr>
    <tr><td colspan="2">
        <asp:ImageButton runat="server" ID="save" ImageUrl="~/img/iconSave.jpg" OnClick="save_Click" ValidationGroup="val" Height="40" />
        <asp:ImageButton runat="server" ID="undo" ImageUrl="~/img/iconUndo.png" OnClick="undo_Click" Height="40" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />