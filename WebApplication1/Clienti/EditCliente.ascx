<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCliente.ascx.cs" Inherits="KIS.Clienti.EditCliente1" %>
<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblData" class="table table-striped table-hover">
    <tr>
        <td>Codice cliente</td>
        <td><asp:TextBox runat="server" ID="txtCodCli" Enabled="false" /></td>
    </tr>
    <tr>
        <td>Ragione sociale</td>
        <td><asp:TextBox runat="server" ID="txtRagSoc" Enabled="false" /></td>
    </tr>
    <tr>
        <td>Partita IVA</td>
        <td><asp:TextBox runat="server" ID="txtPIva" MaxLength="11" ValidationGroup="salva" />
            <asp:CustomValidator ID="valPIva" runat="server" ControlToValidate="txtPIva" ForeColor="Red" ErrorMessage="* Il campo deve essere lungo 11 caratteri, oppure deve essere vuoto" OnServerValidate="valPIva_ServerValidate" ValidationGroup="salva" />
        </td>
    </tr>
    <tr>
        <td>Codice fiscale</td>
        <td><asp:TextBox runat="server" ID="txtCodFiscale" ValidationGroup="salva" MaxLength="16" />
             <asp:CustomValidator ID="valCodFiscale" runat="server" ControlToValidate="txtCodFiscale" ForeColor="Red" ErrorMessage="* Il campo deve essere lungo 16 caratteri, oppure deve essere vuoto" OnServerValidate="valCodFiscale_ServerValidate" ValidationGroup="salva" />
        </td>
        
    </tr>
    <tr>
        <td>Indirizzo</td>
        <td><asp:TextBox runat="server" ID="txtIndirizzo" MaxLength="255" /></td>
    </tr>
    <tr>
        <td>Città</td>
        <td><asp:TextBox runat="server" ID="txtCitta" MaxLength="255" /></td>
    </tr>
    <tr>
        <td>Provincia</td>
        <td><asp:TextBox runat="server" ID="txtProvincia" Width="20" MaxLength="2" ValidationGroup="salva" />
            <asp:CustomValidator ID="valProvincia" runat="server" ControlToValidate="txtProvincia" ForeColor="Red" ErrorMessage="* Il campo deve essere lungo 2 caratteri" OnServerValidate="valProvincia_ServerValidate" ValidationGroup="salva" />
        </td>
    </tr>
    <tr>
        <td>CAP</td>
        <td><asp:TextBox runat="server" ID="txtCAP" MaxLength="10" /></td>
    </tr>
    <tr>
        <td>Stato</td>
        <td><asp:TextBox runat="server" ID="txtStato" MaxLength="255" /></td>
    </tr>
    <tr>
        <td>Telefono</td>
        <td><asp:TextBox runat="server" ID="txtTelefono" MaxLength="45" /></td>
    </tr>
    <tr>
        <td>E-mail</td>
        <td><asp:TextBox runat="server" ID="txtEmail" MaxLength="255" /></td>
    </tr>
    <tr>
        <td>Gestione a kanban</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlKanban">
                <asp:ListItem Value="0">Disabilitato</asp:ListItem>
                <asp:ListItem Value="1">Abilitato</asp:ListItem>
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