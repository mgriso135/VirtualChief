<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeBoxUser.ascx.cs" Inherits="KIS.Personal.HomeBoxUser" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script>
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>
        <asp:Label runat="server" ID="lbl1" />
        <asp:Repeater runat="server" ID="rptHomeBoxes" OnItemDataBound="rptHomeBoxes_ItemDataBound" OnItemCommand="rptHomeBoxes_ItemCommand">
            <HeaderTemplate>
                <table class="table table-condensed table-striped">
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:HiddenField runat="server" ID="hID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                        <asp:ImageButton runat="server" ID="btnAdd" CommandName="add" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconAdd2.png" Width="20" />
                        <asp:ImageButton runat="server" ID="btnArrowUp" CommandName="up" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconArrowUp.png" Width="20" />
                        </td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnArrowDown" CommandName="down" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconArrowDown.png" Width="20" />
                    </td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Nome") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Descrizione") %></td>
                    <td><asp:ImageButton runat="server" ID="btnDelete" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' ImageUrl="~/img/iconDelete.png" Width="20" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </ContentTemplate>
    </asp:UpdatePanel>