<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addGruppo.ascx.cs" Inherits="KIS.Users.addGruppo" %>

<asp:Label runat="server" ID="lbl1" />
<asp:ImageButton runat="server" ID="btnShowAddGruppo" ImageUrl="/img/iconAdd2.png" Height="50px" OnClick="btnShowAddGruppo_Click" />

<table runat="server" id="frmAddGruppo">
    <tr>
        <td>Nome:</td><td><asp:TextBox runat="server" ID="nomeG" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ControlToValidate="nomeG" ErrorMessage="* Campo obbligatorio" ForeColor="Red" ValidationGroup="add" />

                      </td>
    </tr>
    <tr>
        <td>Descrizione:</td>
        <td>
            <asp:TextBox runat="server" ID="descG" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDesc" ControlToValidate="descG" ErrorMessage="* Campo obbligatorio" ForeColor="Red" ValidationGroup="add" />
        </td>
    </tr>
    <tr><td colspan="2">
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="btnSave_Click" ValidationGroup="add" />
        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconUndo.png" Height="40px" OnClick="btnUndo_Click" />
        </td></tr>
</table>