<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="frmSolveProblem.ascx.cs" Inherits="KIS.Produzione.frmSolveProblem" %>
<h1><asp:literal runat="server" id="lblSolveWarning" Text="<%$Resources:lblSolveWarning %>" /></h1>
<table runat="server" id="tblResolution">
    <tr><td><asp:literal runat="server" id="lblCausa" Text="<%$Resources:lblCausa %>" /></td><td><asp:TextBox runat="server" ID="txtCausa" TextMode="MultiLine" ValidationGroup="val1" />
        <asp:RequiredFieldValidator runat="server" ID="valCausa" ForeColor="Red" ControlToValidate="txtCausa" ErrorMessage="<%$Resources:lblReqField %>" ValidationGroup="val1" />
</td></tr>
    <tr><td><asp:literal runat="server" id="lblRisoluzione" Text="<%$Resources:lblRisoluzione %>" /></td><td><asp:TextBox runat="server" ID="txtRisoluzione" TextMode="MultiLine" ValidationGroup="val1" />
        <asp:RequiredFieldValidator runat="server" ID="valRisoluzione" ForeColor="Red" ControlToValidate="txtRisoluzione" ErrorMessage="<%$Resources:lblReqField %>"  ValidationGroup="val1" />
</td></tr>
    <tr><td colspan="2">
        <asp:ImageButton runat="server" ID="imgSave" ImageUrl="~/img/iconSave.jpg" Height="80" ValidationGroup="val1" OnClick="imgSave_Click" />
        <asp:ImageButton runat="server" ID="imgUndo" ImageUrl="~/img/iconUndo.png" Height="80" OnClick="imgUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />