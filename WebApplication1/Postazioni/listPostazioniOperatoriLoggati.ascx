<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listPostazioniOperatoriLoggati.ascx.cs" Inherits="KIS.Postazioni.listPostazioniOperatoriLoggati" %>

<asp:UpdatePanel runat="server" ID="updPostUser" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Label runat="server" ID="lblData" />
<asp:Repeater runat="server" ID="rptPostazioniUtenti" OnItemDataBound="rptPostazioniUtenti_ItemDataBound">
<HeaderTemplate>
    <table class="table table-striped table-hover table-condensed">
        <tbody>
</HeaderTemplate>
    
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <asp:HiddenField runat="server" ID="hID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <%#DataBinder.Eval(Container.DataItem, "name") %>
            </td>
            <td>
                <asp:Label runat="server" ID="lblUsers" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate></tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

        <asp:Timer runat="server" ID="updTimer1" Interval="300000" OnTick="updTimer1_Tick"  />
    </ContentTemplate>
    </asp:UpdatePanel>