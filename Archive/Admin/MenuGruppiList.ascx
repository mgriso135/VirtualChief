<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuGruppiList.ascx.cs" Inherits="KIS.Admin.MenuGruppiList" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptVociGruppi" OnItemCommand="rptVociGruppi_ItemCommand">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:ImageButton runat="server" ID="imgDelete" CommandName="Delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconDelete.png" Height="20" />
                <asp:ImageButton runat="server" ID="imgMoveUp" CommandName="MoveUp" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconArrowUp.png" Height="20" />
                <asp:ImageButton runat="server" ID="ImageMoveDown" CommandName="MoveDown" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconArrowDown.png" Height="20" />
            </td>
            <td><asp:HiddenField runat="server" ID="menuID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "ID") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Titolo") %></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Descrizione") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>