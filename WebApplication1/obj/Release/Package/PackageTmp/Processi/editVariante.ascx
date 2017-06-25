<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editVariante.ascx.cs" Inherits="KIS.Processi.editVariante" %>
<asp:Label runat="server" ID="lblErr" />
<table runat="server" id="frmEditVariante" style="border-left: 1px dashed green; border-right: 1px dashed green; border-top: 1px dashed green;  border-bottom:1px dashed green;">
    <tr>
        <td><asp:Literal runat="server" ID="lblNomeVar" Text="<%$Resources:lblNomeVariante %>" />:</td>
        <td><asp:TextBox runat="server" ID="nomeVar" />
            <asp:HiddenField runat="server" ID="lblVarID" />
        </td>
        <td rowspan="2">
            <asp:ImageButton runat="server" ID="save" ImageUrl="/img/iconSave.jpg" Height="50" ToolTip="<%$Resources:lblTTSalva %>" OnClick="save_Click" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblDescVariante" Text="<%$Resources:lblDescVariante %>" />:</td>
        <td><asp:TextBox runat="server" ID="descVar" TextMode="MultiLine" /></td>
    </tr>
</table>