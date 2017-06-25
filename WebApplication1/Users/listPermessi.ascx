<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listPermessi.ascx.cs" Inherits="KIS.Users.listPermessi" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptPermessi" OnItemDataBound="rptListPermessi_ItemDataBound" OnItemCommand="rptPermessi_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead>
            <tr><th></th>
                <th>
                    <asp:Literal runat="server" ID="lblNomePerm" Text="<%$Resources:lblNomePerm %>" /></th>
                <th><asp:Literal runat="server" ID="lblTHDescrizione" Text="<%$Resources:lblTHDescrizione %>" /></th>
                <th></th>
            </tr>
                </thead><tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="/img/edit.png" Height="30px" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' ToolTip="<%$Resources:lblModificaPerm %>" />
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="/img/iconSave.jpg" Height="30px" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" ToolTip="<%$Resources:lblSalvaMod %>" />
            </td>
            <td><asp:label runat="server" id="lblNomeP" text='<%# DataBinder.Eval(Container.DataItem, "nome") %>' />
                <asp:TextBox runat="server" ID="txtNomeP" Text='<%# DataBinder.Eval(Container.DataItem, "nome") %>' Visible="false" />
            </td>
            <td>
                <asp:label runat="server" ID="lblDescP" Text='<%# DataBinder.Eval(Container.DataItem, "descrizione") %>' />
                <asp:TextBox runat="server" ID="txtDescP" Text='<%# DataBinder.Eval(Container.DataItem, "descrizione") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="btnDel" ImageUrl="/img/iconDelete.png" Height="30" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' Visible ="false" ToolTip="<%$Resources:lblCancellaPerm %>" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>