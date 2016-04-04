<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addContattoCliente.ascx.cs" Inherits="KIS.Clienti.addContattoCliente" %>
<asp:Label runat="server" ID="lbl1" />

<asp:ImageButton runat="server" ID="imgViewTableAdd" ImageUrl="~/img/iconAdd2.png" OnClick="addContattoCliente_Click" Height="40" />

<asp:Table runat="server" ID="tblAddContatto" Visible="false">
    <asp:TableRow>
        <asp:TableCell>
            Nominativo
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox runat="server" ID="txtNominativo" ValidationGroup="contatto" />
            <asp:RequiredFieldValidator runat="server" ID="valNominativo" ControlToValidate="txtNominativo" ForeColor="Red" ValidationGroup="contatto" ErrorMessage="* Campo obbligatorio" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            Ruolo
        </asp:TableCell>
        <asp:TableCell>
            <asp:TextBox runat="server" ID="txtRuolo" ValidationGroup="contatto" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
            <asp:ImageButton runat="server" ID="btnSave" OnClick="btnSave_Click" ImageUrl="~/img/iconSave.jpg" ValidationGroup="contatto" Height="40" />
            <asp:ImageButton runat="server" ID="btnUndo" OnClick="btnUndo_Click" ImageUrl="~/img/iconUndo.png" Height="40" />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>