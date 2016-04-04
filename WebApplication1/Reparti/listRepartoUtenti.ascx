<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listRepartoUtenti.ascx.cs" Inherits="KIS.Reparti.listRepartoUtenti" %>
<h3>Elenco utenti</h3>
<asp:Label runat="server" ID="lbl1" />


<asp:Repeater runat="server" ID="rptRepUsers" OnItemCommand="rptRepUsers_ItemCommand">
    <HeaderTemplate>
        <table style="border:1px dotted blue">
            <tr>
                <td></td>
                <td>Operatore</td>
                <td>Nome</td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:ImageButton runat="server" ID="imgDeleteOp" CommandName="deleteOP" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "username") %>' ImageUrl="/img/iconDelete.png" Height="30px" ToolTip="Rimuove l'operatore dal reparto" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "username") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "FullName") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

<asp:ImageButton runat="server" ID="imgShowAddFrm" ImageUrl="/img/iconUserAdd.jpg" Height="40" ToolTip="Aggiungi un operatore al reparto" OnClick="imgShowAddFrm_Click" />
<table runat="server" id="frmAddOperator" style="border:1px dashed blue">
    <tr><td>Utente</td>
        <td><asp:DropDownList runat="server" ID="ddlUser" /></td>
    <td><asp:ImageButton runat="server" ID="imgAddUser" ImageUrl="/img/iconSave.jpg" Height="40" OnClick="imgAddUser_Click" /></td></tr>
</table>