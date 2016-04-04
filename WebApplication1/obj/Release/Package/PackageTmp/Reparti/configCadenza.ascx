<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configCadenza.ascx.cs" Inherits="KIS.Produzione.configCadenza" %>
<asp:Label runat="server" ID="lbl1" />
<h3>Cadenza</h3>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<table>
    <tr>
        <td>Cadenza</td>
        <td>Ore:<asp:TextBox Columns="3" ID="ore" runat="server" Width="40px" />
            mm:<asp:TextBox Columns="2" ID="minuti" runat="server" Width="40px" />
            ss:<asp:TextBox Columns="2" ID="secondi" runat="server" Width="40px" />
            <asp:ImageButton runat="server" ID="editCadenza" OnClick="editCadenza_Click" ImageUrl="/img/edit.png" Height="30px" />
        </td>
    </tr>
    <tr runat="server" id="valButtons">
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50px" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="/img/iconUndo.png" Height="50px" />
            <br />
            <asp:RequiredFieldValidator runat="server" ID="valOre" ValidationGroup="cadenza" ControlToValidate="ore" ErrorMessage="Il campo ore non può essere vuoto.<br />" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeOre" ValidationGroup="cadenza" ControlToValidate="ore" MinimumValue="0" MaximumValue="10000000" ErrorMessage="Valore non valido per il campo ore.<br/>" ForeColor="Red" />
            <asp:RequiredFieldValidator runat="server" ID="valMinuti" ValidationGroup="cadenza" ControlToValidate="minuti" ErrorMessage="Il campo minuti non può essere vuoto.<br />" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeMinuti" ValidationGroup="cadenza" ControlToValidate="minuti" MinimumValue="0" MaximumValue="59" ErrorMessage="Valore non valido per il campo minuti.<br/>" ForeColor="Red" />
            <asp:RequiredFieldValidator runat="server" ID="valSecondi" ValidationGroup="cadenza" ControlToValidate="secondi" ErrorMessage="Il campo secondi non può essere vuoto.<br />" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeSecondi" ValidationGroup="cadenza" ControlToValidate="minuti" MinimumValue="0" MaximumValue="59" ErrorMessage="Valore non valido per il campo secondi.<br/>" ForeColor="Red" />
        </td>
    </tr>
</table>
        </ContentTemplate>
    </asp:UpdatePanel>