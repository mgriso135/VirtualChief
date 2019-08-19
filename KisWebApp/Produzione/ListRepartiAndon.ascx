<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListRepartiAndon.ascx.cs" Inherits="KIS.Produzione.ListRepartiAndon" %>
<asp:Label runat="server" ID="lbl1" />

    <asp:Repeater runat="server" ID="rptListReparti" OnItemDataBound="rptListReparti_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-striped table-hover">
                <thead>
                <tr>
                    <th></th>
                    <th><asp:literal runat="server" id="lblReparti" Text="<%$Resources:lblReparti %>" /></th>
                    <th><asp:literal runat="server" id="lblDescrizione" Text="<%$Resources:lblDescrizione %>" /></th>
                </tr>
                    </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td><a href="../../Andon/DepartmentAndon/Index?DepartmentID=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                    <asp:image runat="server" ID="imgview" ImageUrl="~/img/iconView.png" height="30px" ToolTip="<%$Resources:lblTTAndon %>" />
                    </a></td>
               
                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "description") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>