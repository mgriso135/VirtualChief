﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addReparto.ascx.cs" Inherits="KIS.Reparti.addReparto" %>

<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="frmAddReparto" class="table">
    <tr>
        <td>Nome:</td>
        <td><asp:TextBox runat="server" ID="nome" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ErrorMessage="Campo obbligatorio" ControlToValidate="nome" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td>Descrizione:</td>
        <td><asp:TextBox runat="server" ID="descrizione" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDescrizione" ErrorMessage="Campo obbligatorio" ControlToValidate="descrizione" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50px" ToolTip="Salva" ValidationGroup="addReparto" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="/img/iconUndo.png" Height="50px" ToolTip="Annulla" ValidationGroup="addReparto" />
        </td>
    </tr>
</table>