<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCliente.ascx.cs" Inherits="KIS.Clienti.EditCliente1" %>
<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblData" class="table table-striped table-hover">
    <tr>
        <td><asp:Label runat="server" ID="lblCodCliente" meta:resourcekey="lblCodCliente" />
            </td>
        <td><asp:TextBox runat="server" ID="txtCodCli" Enabled="false" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblRagSocCliente" meta:resourcekey="lblRagSocCliente" /></td>
        <td><asp:TextBox runat="server" ID="txtRagSoc" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblPartitaIVA" meta:resourcekey="lblPartitaIVA" /></td>
        <td><asp:TextBox runat="server" ID="txtPIva" MaxLength="11" ValidationGroup="salva" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblCodFiscale" meta:resourcekey="lblCodFiscale" /></td>
        <td><asp:TextBox runat="server" ID="txtCodFiscale" ValidationGroup="salva" MaxLength="16" />
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
        <td><asp:TextBox runat="server" ID="txtProvincia" Width="20" MaxLength="255" ValidationGroup="salva" />
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
        <td><asp:Label runat="server" ID="lblEMail" meta:resourcekey="lblEMail" /></td>
        <td><asp:TextBox runat="server" ID="txtEmail" MaxLength="255" /></td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblKBBox" meta:resourcekey="lblKBBox" /></td>
        <td>
            <asp:DropDownList runat="server" ID="ddlKanban">
                <asp:ListItem Value="0" Text="<%$Resources:lblKBDisabled %>"></asp:ListItem>
                <asp:ListItem Value="1" Text="<%$Resources:lblKBEnabled %>"></asp:ListItem>
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