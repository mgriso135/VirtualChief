<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addCliente.ascx.cs" Inherits="KIS.Clienti.addCliente" %>

<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="frmAddcliente" class="table table-hover table-striped">
    <tr>
        <td><asp:Label runat="server" ID="lblCodiceCliente" meta:resourcekey="lblCodiceCliente" /></td>
        <td><asp:TextBox runat="server" ID="txtCodiceCliente" ValidationGroup="salva" />
            <asp:RequiredFieldValidator runat="server" ID="valCodCliente" ControlToValidate="txtCodiceCliente" ValidationGroup="salva" ErrorMessage="<%$resources:reqField %>" ForeColor="Red" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblRagioneSocialeCliente" meta:resourcekey="lblRagioneSocialeCliente" /></td>
        <td><asp:TextBox runat="server" ID="txtRagSoc" ValidationGroup="salva" />
            <asp:RequiredFieldValidator runat="server" ID="valRagSoc" ControlToValidate="txtRagSoc" ValidationGroup="salva" ErrorMessage="<%$resources:reqField %>" ForeColor="Red" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblPartitaIVA" meta:resourcekey="lblPartitaIVA" /></td>
        <td><asp:TextBox runat="server" ID="txtPartitaIVA" MaxLength="255" ValidationGroup="salva" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblCodiceFiscale" meta:resourcekey="lblCodiceFiscale" /></td>
        <td><asp:TextBox runat="server" ID="txtCodFiscale" ValidationGroup="salva" MaxLength="255" />
        </td>
        
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblIndirizzo" meta:resourcekey="lblIndirizzo" /></td>
        <td><asp:TextBox runat="server" ID="txtIndirizzo" MaxLength="255" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblCitta" meta:resourcekey="lblCitta" /></td>
        <td><asp:TextBox runat="server" ID="txtCitta" MaxLength="255" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblProvincia" meta:resourcekey="lblProvincia" /></td>
        <td><asp:TextBox runat="server" ID="txtProvincia" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblCAP" meta:resourcekey="lblCAP" /></td>
        <td><asp:TextBox runat="server" ID="txtCAP" MaxLength="10" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblStato" meta:resourcekey="lblStato" /></td>
        <td><asp:TextBox runat="server" ID="txtStato" MaxLength="255" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblTelefono" meta:resourcekey="lblTelefono" /></td>
        <td><asp:TextBox runat="server" ID="txtTelefono" MaxLength="45" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblEmail" meta:resourcekey="lblEmail" /></td>
        <td><asp:TextBox runat="server" ID="txtEmail" MaxLength="255" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblKanban" meta:resourcekey="lblKanban" /></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlKanban">
                <asp:ListItem Value="0" Text="<%$resources:lblKBBoxDisabled %>"></asp:ListItem>
                <asp:ListItem Value="1" Text="<%$resources:lblKBBoxEnabled %>"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center;">
            <asp:ImageButton runat="server" ID="btnSave" OnClick="btnSave_Click" ImageUrl="~/img/iconSave.jpg" Height="40" ValidationGroup="salva" />
            <asp:ImageButton runat="server" ID="btnUndo" OnClick="btnUndo_Click" ImageUrl="~/img/iconUndo.png" Height="40" />
        </td>
    </tr>
</table>