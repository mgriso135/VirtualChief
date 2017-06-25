<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listClienti.ascx.cs" Inherits="KIS.Clienti.listClienti" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rpt1" OnItemCommand="rpt1_ItemCommand" OnItemDataBound="rpt1_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-hover table-striped">
            <thead>
                <th></th>
                <th><asp:label runat="server" ID="lblTHCodCliente" meta:resourcekey="lblTHCodCliente" /></th>
                <th><asp:label runat="server" ID="lblTHRagSoc" meta:resourcekey="lblTHRagSoc" /></th>
                <th><asp:label runat="server" ID="lblTHPIva" meta:resourcekey="lblTHPIva" /></th>
                <th><asp:label runat="server" ID="lblTHCodFiscale" meta:resourcekey="lblTHCodFiscale" /></th>
                <th><asp:label runat="server" ID="lblTHIndirizzo" meta:resourcekey="lblTHIndirizzo" /></th>
                <th><asp:label runat="server" ID="lblTHCitta" meta:resourcekey="lblTHCitta" /></th>
                <th><asp:label runat="server" ID="lblTHProvincia" meta:resourcekey="lblTHProvincia" /></th>
                <th><asp:label runat="server" ID="lblTHCAP" meta:resourcekey="lblTHCAP" /></th>
                <th><asp:label runat="server" ID="lblTHStato" meta:resourcekey="lblTHStato" /></th>
                <th><asp:label runat="server" ID="lblTHTelefono" meta:resourcekey="lblTHTelefono" /></th>
                <th><asp:label runat="server" ID="lblTHEmail" meta:resourcekey="lblTHEmail" /></th>
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