<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewDetailsTask.ascx.cs" Inherits="KIS.Produzione.viewDetailsTask" %>
<h3 runat="server" id="lblNomeTask"></h3>
<asp:Label runat="server" ID="lbl1" />
Legenda tipo evento: I = iniziato; P = in pausa; F = finito; W = segnalato problema
<asp:Repeater runat="server" ID="rptDettagli" OnItemDataBound="rptDettagli_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-condensed table-hover table-striped">
            <tr>
                <td>Data</td>
                <td>Tipo Evento</td>
                <td>Utente</td>
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