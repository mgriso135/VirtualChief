<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailCostCommessa.ascx.cs" Inherits="KIS.Analysis.DetailCostCommessa1" %>

<asp:Label runat="server" ID="lbl1" />
<table id="tblCommessa" runat="server">
    <tr>
        <td>ID</td>
        <td><asp:Label runat="server" ID="lblIDComm" /></td>
    </tr>
    <tr>
        <td>Cliente</td>
        <td><asp:Label runat="server" ID="lblCliente" /></td>
    </tr>
    <tr>
        <td>Data inserimento</td>
        <td><asp:Label runat="server" ID="lblDataInserimento" /></td>
    </tr>
    <tr>
        <td>Note</td>
        <td><asp:Label runat="server" ID="lblNote" /></td>
    </tr>
    <tr>
        <td>Ore di lavoro</td>
        <td><b><asp:Label runat="server" ID="lblOreTot" /></b></td>
    </tr>
    </table>
    <asp:Repeater runat="server" ID="rptArticoli">
        <HeaderTemplate>
            <table>
            <tr>
                <th></th>
                <th>Articolo</th>
                <th>Nome</th>
                <th>Quantità</th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><asp:HyperLink Target="_blank" runat="server" ID="lnkDetailArticolo" NavigateUrl='<%# "~/Produzione/statoAvanzamentoArticolo.aspx?id=" + DataBinder.Eval(Container.DataItem, "ID") + "&anno=" + DataBinder.Eval(Container.DataItem, "Year") %>'>
                    <asp:Image runat="server" ID="imgDetailArticolo" ImageUrl="~/img/iconView.png" Height="40" />
                    </asp:HyperLink></td>
                <td>
                    <%# DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year")%>
                    </td>
                <td>
                    <%# DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;
                    <%# DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %>
                    </td><td><%# DataBinder.Eval(Container.DataItem, "Quantita") %>
                </td>
            </tr>
        </ItemTemplate>
        <SeparatorTemplate></SeparatorTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
</table>