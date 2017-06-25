<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listGruppi.ascx.cs" Inherits="KIS.Users.listGruppi" %>


<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptGruppi" OnItemDataBound="rptListGruppi_ItemDataBound" OnItemCommand="rptGruppi_ItemCommand">
    <HeaderTemplate>
    <table class="table table-condensed table-striped table-hover">
        <thead>
        <tr>
            <th></th>
            <th></th>
            <th></th>
            <th><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" /></th>
            <th><asp:Literal runat="server" ID="Literal1" Text="<%$Resources:lblDescrizione %>" /></th>
            <th></th>
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
                <asp:ImageButton runat="server" ID="btnManageMenu" ImageUrl="/img/iconMenuItem.jpg" Height="30" CommandName="mngMenu" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="<%$Resources:lblTTManageMenu %>" />
            </td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="/img/edit.png" Height="30" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="<%$Resources:lblTTModGruppo %>" />
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="30" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" ToolTip="<%$Resources:lblTTSalvaMod %>" />
            </td>
            <td><asp:label runat="server" ID="lbNome" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' />
                <asp:TextBox runat="server" ID="tbNome" Text='<%# DataBinder.Eval(Container.DataItem, "Nome") %>' Visible="false" />
            </td>
            <td><asp:label runat="server" ID="lbDesc" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' />
                <asp:TextBox runat="server" ID="tbDesc" Text='<%# DataBinder.Eval(Container.DataItem, "Descrizione") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="btnDel" ImageUrl="/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" ToolTip="<%$Resources:lblTTEliminaGruppo %>" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
    </table>
        </FooterTemplate>
</asp:Repeater>