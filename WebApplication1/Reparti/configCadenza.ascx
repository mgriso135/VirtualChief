<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configCadenza.ascx.cs" Inherits="KIS.Produzione.configCadenza" %>
<asp:Label runat="server" ID="lbl1" />
<h3>Cadenza</h3>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<table>
    <tr>
        <td><asp:Literal runat="server" ID="lblTaktTime" Text="<%$Resources:lblTaktTime %>" /></td>
        <td><asp:Literal runat="server" ID="lblOre" Text="<%$Resources:lblOre %>" />:<asp:TextBox Columns="3" ID="ore" runat="server" Width="40px" />
            <asp:Literal runat="server" ID="lblMM" Text="<%$Resources:lblMM %>" />:<asp:TextBox Columns="2" ID="minuti" runat="server" Width="40px" />
            <asp:Literal runat="server" ID="lblSS" Text="<%$Resources:lblSS %>" />:<asp:TextBox Columns="2" ID="secondi" runat="server" Width="40px" />
            <asp:ImageButton runat="server" ID="editCadenza" OnClick="editCadenza_Click" ImageUrl="/img/edit.png" Height="30px" />
        </td>
    </tr>
    <tr runat="server" id="valButtons">
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50px" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="/img/iconUndo.png" Height="50px" />
            <br />
            <asp:RequiredFieldValidator runat="server" ID="valOre" ValidationGroup="cadenza" ControlToValidate="ore" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeOre" ValidationGroup="cadenza" ControlToValidate="ore" MinimumValue="0" MaximumValue="10000000" ErrorMessage="<%$Resources:lblValNotValid %>" ForeColor="Red" />
            <asp:RequiredFieldValidator runat="server" ID="valMinuti" ValidationGroup="cadenza" ControlToValidate="minuti" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeMinuti" ValidationGroup="cadenza" ControlToValidate="minuti" MinimumValue="0" MaximumValue="59" ErrorMessage="<%$Resources:lblValNotValid %>" ForeColor="Red" />
            <asp:RequiredFieldValidator runat="server" ID="valSecondi" ValidationGroup="cadenza" ControlToValidate="secondi" ErrorMessage="<%$Resources:lblReqField %>" ForeColor="Red" />
            <asp:RangeValidator runat="server" ID="valRangeSecondi" ValidationGroup="cadenza" ControlToValidate="minuti" MinimumValue="0" MaximumValue="59" ErrorMessage="<%$Resources:lblValNotValid %>" ForeColor="Red" />
        </td>
    </tr>
</table>
        </ContentTemplate>
    </asp:UpdatePanel>