<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="linkArticoli.ascx.cs" Inherits="KIS.Commesse.linkArticoli" %>

<table runat="server" id="frmLinkArticolo" class="table table-condensed">
    <tr>
        <td>Articolo:</td>
        <td><asp:DropDownList runat="server" ID="ddlArticoli" AppendDataBoundItems="true">
            <asp:ListItem Text="Scegli un processo" Value="-1" />
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>Quantità</td>
        <td><asp:TextBox runat="server" ID="txtQuantita" />
            <asp:RequiredFieldValidator runat="server" ID="valNote" ForeColor="Red" ControlToValidate="txtQuantita" ErrorMessage="* il campo non può essere vuoto" ValidationGroup="articolo" />
        </td>
    </tr>
    <tr>
        <td>Data consegna prevista:</td>
        <td><asp:Calendar runat="server" ID="consegnaprevista" /></td>
    </tr>
    <tr><td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="40" ToolTip="Salva l'articolo" OnClick="btnSave_Click" ValidationGroup="articolo" />
        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconUndo.png" Height="40" ToolTip="Form reset" OnClick="btnUndo_Click" />
        </td></tr>
</table>
<asp:Label runat="server" ID="lbl1" />