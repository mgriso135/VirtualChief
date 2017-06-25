<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listReparti.ascx.cs" Inherits="KIS.Reparti.listReparti" %>

<asp:Label runat="server" ID="lbl1" />

    <asp:Repeater runat="server" ID="rptListReparti" OnItemDataBound="rptListReparti_ItemDataBound">
        <HeaderTemplate>
            <table class="table table-condensed table-striped table-hover">
                <thead>
                <tr>
                    <th></th>
                    <th><asp:literal runat="server" id="lblReparti" Text="<%$Resources:lblReparti %>" /></th>
                </tr>
                    </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td><a href="configReparto.aspx?id=<%# DataBinder.Eval(Container.DataItem, "id") %>">
                    <asp:image runat="server" ImageUrl="/img/iconView.png" height="30px" />
                    </a></td>
                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>