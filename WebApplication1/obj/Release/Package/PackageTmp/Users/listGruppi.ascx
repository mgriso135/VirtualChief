<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listGruppi.ascx.cs" Inherits="KIS.Users.listGruppi" %>


<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptGruppi" OnItemDataBound="rptListGruppi_ItemDataBound" OnItemCommand="rptGruppi_ItemCommand">
    <HeaderTemplate>
    <table class="table table-condensed table-striped table-hover">
        <thead>
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td>Nome</td>
            <td>Descrizione</td>
            <td></td>
        </tr>
            </thead>
        <tbody>
        </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:ImageButton runat="server" ID="btnManagePermessi" ImageUrl="/img/iconPermissions.png" Height="30" CommandName="mngPermessi" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="btnManageMenu" ImageUrl="/img/iconMenuItem.jpg" Height="30" CommandName="mngMenu" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="Gestisci i menu associati al gruppo" />
            </td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="Modifica il gruppo" />
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" ToolTip="Salva le modifiche al gruppo" />
            </td>
            <td><asp:label runat="server" ID="lbNome" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' />
                <asp:TextBox runat="server" ID="tbNome" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' Visible="false" />
            </td>
            <td><asp:label runat="server" ID="lbDesc" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' />
                <asp:TextBox runat="server" ID="tbDesc" Text='<%# DataBinder.Eval(Container.DataItem, "Descrizione") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="btnDel" ImageUrl="/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" ToolTip="Elimina il gruppo" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
    </table>
        </FooterTemplate>
</asp:Repeater>