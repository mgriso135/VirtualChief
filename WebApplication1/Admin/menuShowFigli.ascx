<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuShowFigli.ascx.cs" Inherits="KIS.Admin.menuShowFigli" %>

<asp:Label runat="server" ID="lbl1" />

<asp:Repeater runat="server" ID="rptFigli" OnItemCommand="rptFigli_ItemCommand" OnItemDataBound="rptFigli_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-condensed table-hover table-striped">
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:ImageButton runat="server" ID="btnMoveUp" CommandName="MoveUp" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID")%>' Height="20" ImageUrl="~/img/iconArrowUp.png" />
                <asp:ImageButton runat="server" ID="btnMoveDown" CommandName="MoveDown" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID")%>' Height="20" ImageUrl="~/img/iconArrowDown.png" />
            </td>
            <td><asp:HyperLink runat="server" ID="lnkExpand" NavigateUrl='<%#"menuShowVoce.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID")%>'>
                <asp:Image runat="server" ID="imgView" ImageUrl="~/img/iconView.png" Height="20" class="btn btn-primary" />
                </asp:HyperLink>

            </td>
            <td><asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/img/edit.png" ToolTip="<%$resources:lblTTModificaVoce %>" class="btn btn-primary" Height="20" CommandName="edit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
            <td>
                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" ToolTip="<%$resources:lblTTCancellaVoce %>" class="btn btn-primary" Height="20" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
            </td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Titolo") %>' ID="lblTitolo" />
                <asp:TextBox runat="server" ID="txtTitolo" Text='<%#DataBinder.Eval(Container.DataItem, "Titolo") %>' Visible="false" />
            </td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' ID="lblDesc" />
                <asp:TextBox runat="server" ID="txtDesc" Text='<%#DataBinder.Eval(Container.DataItem, "Descrizione") %>' Visible="false" /></td>
            <td><asp:label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "URL") %>' ID="lblURL" />
                <asp:TextBox runat="server" ID="txtURL" Text='<%#DataBinder.Eval(Container.DataItem, "URL") %>' Visible="false" />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="imgSave" ImageUrl="~/img/iconSave.jpg" ToolTip="<%$resources:lblTTSalvaVoce %>" Height="40" CommandName="save" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" />
                <asp:ImageButton runat="server" ID="imgUndo" ImageUrl="~/img/iconUndo.png" ToolTip="<%$resources:lblTTReset %>" Height="40" CommandName="undo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' Visible="false" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>