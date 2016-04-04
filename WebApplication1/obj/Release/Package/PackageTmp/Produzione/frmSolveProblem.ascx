<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmSolveProblem.ascx.cs" Inherits="KIS.Produzione.frmSolveProblem" %>
<h1>Risoluzione warning</h1>
<table runat="server" id="tblResolution">
    <tr><td>Causa della segnalazione</td><td><asp:TextBox runat="server" ID="txtCausa" TextMode="MultiLine" ValidationGroup="val1" />
        <asp:RequiredFieldValidator runat="server" ID="valCausa" ForeColor="Red" ControlToValidate="txtCausa" ErrorMessage="* Campo obbligatorio" ValidationGroup="val1" />
</td></tr>
    <tr><td>Risoluzione</td><td><asp:TextBox runat="server" ID="txtRisoluzione" TextMode="MultiLine" ValidationGroup="val1" />
        <asp:RequiredFieldValidator runat="server" ID="valRisoluzione" ForeColor="Red" ControlToValidate="txtRisoluzione" ErrorMessage="* Campo obbligatorio"  ValidationGroup="val1" />
</td></tr>
    <tr><td colspan="2">
        <asp:ImageButton runat="server" ID="imgSave" ImageUrl="/img/iconSave.jpg" Height="80" ValidationGroup="val1" OnClick="imgSave_Click" />
        <asp:ImageButton runat="server" ID="imgUndo" ImageUrl="/img/iconUndo.png" Height="80" OnClick="imgUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />