<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="elencoStraordinari.ascx.cs" Inherits="KIS.Reparti.elencoStraordinari" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptStraord" OnItemDataBound="rptStraord_ItemDataBound" OnItemCommand="rptStraord_ItemCommand">
    <HeaderTemplate>
        <table class="table striped condensed">
            <tr>
                <th></th>
                <th><asp:Literal runat="server" ID="lblStartStaord" Text="<%$Resources:lblStartStraord %>" /></th>
                <th><asp:Literal runat="server" ID="lblEndStraord" Text="<%$Resources:lblEndStraord %>" /></th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton ID="ImageButton1" runat="server" CommandName="deleteStraord" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "idStraordinario") %>' ImageUrl="~/img/iconCancel.jpg" Height="30px" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Inizio") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Fine") %></td>
        </tr>

    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>