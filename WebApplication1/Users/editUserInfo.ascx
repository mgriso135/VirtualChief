<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editUserInfo.ascx.cs" Inherits="KIS.Users.editUserInfo" %>

<asp:Label runat="server" ID="lbl1" />
<h1><asp:Label runat="server" ID="lblUsername" /></h1>
<table runat="server" id="tbEdit" class="table table-hover">
    <tbody>
    <tr>
        <td>Nome:</td>
        <td><asp:TextBox runat="server" ID="tbNome" /></td>
    </tr>
    <tr>
        <td>Cognome:</td>
        <td><asp:TextBox runat="server" ID="tbCognome" /></td>
    </tr>
        </tbody>
    <tfoot>
    <tr>
        <td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="50px" OnClick="btnSave_Click" />
        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="/img/iconUndo.png" Height="50px" OnClick="btnUndo_Click" />
            <asp:Label runat="server" ID="lblRes" ForeColor="Red" />
        </td>
    </tr>
        </tfoot>
</table>