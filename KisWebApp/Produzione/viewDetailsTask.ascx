<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewDetailsTask.ascx.cs" Inherits="KIS.Produzione.viewDetailsTask" %>
<h3 runat="server" id="lblNomeTask"></h3>
<asp:Label runat="server" ID="lbl1" />
<asp:Literal runat="server" ID="lblLegenda" Text="<%$Resources:lblLegenda %>" />
<asp:Repeater runat="server" ID="rptDettagli" OnItemDataBound="rptDettagli_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-condensed table-hover table-striped">
            <tr>
                <td><asp:Literal runat="server" ID="lblData" Text="<%$Resources:lblData %>" /></td>
                <td><asp:Literal runat="server" ID="lblTipoEvento" Text="<%$Resources:lblTipoEvento %>" /></td>
                <td><asp:Literal runat="server" ID="lblUtente" Text="<%$Resources:lblUtente %>" /></td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:HiddenField runat="server" ID="idEvt" Value='<%# DataBinder.Eval(Container.DataItem, "IdEvento") %>' />
                <%# DataBinder.Eval(Container.DataItem, "Data") %></td>
            <td><%# DataBinder.Eval(Container.DataItem, "Evento") %></td>
            <td><%# DataBinder.Eval(Container.DataItem, "User") %>
                <asp:Label runat="server" ID="lblNomeUser" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>