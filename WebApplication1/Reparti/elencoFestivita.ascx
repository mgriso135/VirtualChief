<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="elencoFestivita.ascx.cs" Inherits="KIS.Reparti.elencoFestivita" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptFest" OnItemDataBound="rptFest_ItemDataBound" OnItemCommand="rptFest_ItemCommand">
    <HeaderTemplate>
        <table>
            <tr>
                <th></th>
                <th><asp:Literal runat="server" ID="lblInizioFest" Text="<%$Resources:lblInizioFest %>" /></th>
                <th><asp:Literal runat="server" ID="lblFineFest" Text="<%$Resources:lblFineFest %>" /></th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" CommandName="deleteFest" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "idFestivita") %>' ImageUrl="/img/iconCancel.jpg" Height="30px" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Inizio") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Fine") %></td>
        </tr>

    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>