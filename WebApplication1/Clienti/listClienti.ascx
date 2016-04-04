<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listClienti.ascx.cs" Inherits="KIS.Clienti.listClienti" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rpt1" OnItemCommand="rpt1_ItemCommand" OnItemDataBound="rpt1_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-hover table-striped">
            <thead>
                <th></th>
                <th>Codice</th>
                <th>Ragione Sociale</th>
                <th>Partita IVA</th>
                <th>Codice Fiscale</th>
                <th>Indirizzo</th>
                <th>Citta</th>
                <th>Provincia</th>
                <th>CAP</th>
                <th>Stato</th>
                <th>Telefono</th>
                <th>Email</th>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hcodCliente" Value='<%#DataBinder.Eval(Container.DataItem, "CodiceCliente") %>' />
                <asp:HyperLink Width="40" runat="server" ID="lnkEditCustomer" NavigateUrl='<%# "EditCliente.aspx?idCliente=" + DataBinder.Eval(Container.DataItem, "CodiceCliente") %>'>
                <asp:Image runat="server" ID="imgDetail" ImageUrl="~/img/iconView.png" Height="40" Width="40" />
                </asp:HyperLink></td>
            <td><%#DataBinder.Eval(Container.DataItem, "CodiceCliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "RagioneSociale") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "PartitaIVA") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "CodiceFiscale") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Indirizzo") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Citta") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Provincia") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "CAP") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Stato") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Telefono") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Email") %></td>
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/img/iconDelete.png" Width="40" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CodiceCliente") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>