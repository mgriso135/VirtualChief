<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="showPostazioni.ascx.cs" Inherits="KIS.Produzione.showPostazioni" %>

<asp:Label runat="server" ID="lblErr" />
<asp:Repeater runat="server" ID="rptPostazioniTasks" OnItemDataBound="rptPostazioniTasks_ItemCreated">
    <HeaderTemplate>
        <table>
            <thead>
            <tr>
                <th><asp:literal runat="server" id="lblTHTasks" Text="<%$Resources:lblTHTasks %>" /></th>
                <th><asp:literal runat="server" id="lblTHPostazioni" Text="<%$Resources:lblTHPostazioni %>" /></td>
            </tr>
                </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:HiddenField runat="server" ID="procID" />
                <%# DataBinder.Eval(Container.DataItem, "ProcessName") %></td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPostazioni" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlPostazioni_IndexChanged" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>