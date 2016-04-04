<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuGruppiAddVoce.ascx.cs" Inherits="KIS.Admin.MenuGruppiAddVoce" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptAddVociGruppi" OnItemCommand="rptAddVociGruppi_ItemCommand">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:ImageButton runat="server" ID="imgAddVoce" Height="20" ImageUrl="~/img/iconAdd2.png" CommandName="addVoce" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
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