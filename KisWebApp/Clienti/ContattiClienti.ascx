<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContattiClienti.ascx.cs" Inherits="KIS.Clienti.ContattiClienti" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptContattiClienti" OnItemDataBound="rptContattiClienti_ItemDataBound" OnItemCommand="rptContattiClienti_ItemCommand">
    <HeaderTemplate>
<table class="table table-hover table-striped table-condensed">
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <asp:HyperLink Width="20" runat="server" ID="lnkDetailContatto" NavigateUrl='<%#"EditContattoDetails.aspx?idContatto=" + DataBinder.Eval(Container.DataItem, "ID") %>'>
                    <asp:Image runat="server" ImageUrl="~/img/iconView.png" Width="20" />
                </asp:HyperLink>
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "FirstName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "LastName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Ruolo") %></td>
            <td><asp:Label runat="server" ID="email" /></td>
            <td><asp:Label runat="server" ID="phone" /></td>
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Width="20" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>

    </asp:Repeater>