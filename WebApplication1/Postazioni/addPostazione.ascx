<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addPostazione.ascx.cs" Inherits="KIS.Produzione.addPostazione" %>

<asp:Label runat="server" ID="lblErr" />
<h2>Aggiungi una postazione</h2>
<table runat="server" id="frmAddPostazione" class="table table-condensed">
    <tr>
        <td>Nome</td><td><asp:textbox runat="server" ID="nomePost" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="nomePost" ErrorMessage="Questo campo non può essere vuoto" ForeColor="Red" ValidationGroup="addPostazione" />
                     </td>
        
    </tr>
    <tr>
        <td>Descrizione</td><td><asp:textbox runat="server" ID="descPost" TextMode="MultiLine" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="descPost" ErrorMessage="Questo campo non può essere vuoto" ForeColor="Red" ValidationGroup="addPostazione" />
                            </td>
    </tr>
    <tr><td colspan="2" style="text-align: center;">
        <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50" ToolTip="Aggiungi la postazione" ValidationGroup="addPostazione"/>
        <asp:ImageButton runat="server" ID="undo" OnClick="undo_Click" ImageUrl="/img/iconUndo.png" Height="50" ToolTip="Reset form" />
        </td></tr>
</table>