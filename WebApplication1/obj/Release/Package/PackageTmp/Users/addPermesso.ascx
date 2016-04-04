<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addPermesso.ascx.cs" Inherits="KIS.Users.addPermesso" %>

<asp:Label runat="server" ID="lbl1" />
<asp:ImageButton runat="server" ID="showAddPermesso" OnClick="showAddPermesso_Click" ImageUrl="/img/iconAdd2.png" Height="50" />

<table id="tblFormAddPermesso" runat="server">
    <tr>
        <td>Nome:</td>
        <td><asp:TextBox runat="server" ID="nomeP" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ControlToValidate="nomeP" ErrorMessage="* Campo richiesto" ForeColor="Red" ValidationGroup="Add" />
        </td>
    </tr>
    <tr>
        <td>Descrizione:</td>
        <td><asp:TextBox runat="server" ID="descP" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDesc" ControlToValidate="descP" ErrorMessage="* Campo richiesto" ForeColor="Red" ValidationGroup="Add" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:ImageButton runat="server" ID="imgAddPermesso" ImageUrl="/img/iconSave.jpg" Height="40" OnClick="imgAddPermesso_Click" ValidationGroup="Add" />
            <asp:ImageButton runat="server" ID="imgReset" ImageUrl="/img/iconUndo.png" Height="40" OnClick="imgReset_Click" />
        </td>
    </tr>
</table>
<asp:Label runat="server" ID="err" />