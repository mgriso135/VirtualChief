<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addReparto.ascx.cs" Inherits="KIS.Reparti.addReparto" %>

<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="frmAddReparto" class="table">
    <tr>
        <td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" />:</td>
        <td><asp:TextBox runat="server" ID="nome" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ErrorMessage="<%$Resources:lblReqField %>" ControlToValidate="nome" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblDescrizione" Text="<%$Resources:lblDescrizione %>" />:</td>
        <td><asp:TextBox runat="server" ID="descrizione" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDescrizione" ErrorMessage="<%$Resources:lblReqField %>" ControlToValidate="descrizione" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblFuso" Text="<%$Resources:lblFuso %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlTimezones"/></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50px" ToolTip="<%$Resources:lblSalva %>" ValidationGroup="addReparto" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="/img/iconUndo.png" Height="50px" ToolTip="<%$Resources:lblAnnulla %>" ValidationGroup="addReparto" />
        </td>
    </tr>
</table>