<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListAnalysisTipoProdotto.ascx.cs" Inherits="KIS.Analysis.ListAnalysisTipoProdotto1" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptTipiProdotto">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead>
            <tr>
                <th></th>
                <th><asp:label runat="server" id="lblTHTipoProdotto" meta:resourcekey="lblTHTipoProdotto" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HyperLink runat="server" ID="lnkDetailProd" NavigateUrl='<%# "DetailAnalysisTipoProdotto.aspx?idProc="+ DataBinder.Eval(Container.DataItem, "process.processID") +"&rev=" + DataBinder.Eval(Container.DataItem, "process.revisione") + "&idVar=" + DataBinder.Eval(Container.DataItem, "variant.idVariante") %>'>
                <asp:Image runat="server" ID="imgDetailProd" ImageUrl="~/img/iconView.png" Height="40" />
                    </asp:HyperLink>
            </td>
            <td>
                <%#DataBinder.Eval(Container.DataItem, "NomeCombinato") %>
            </td>
        </tr>
    </ItemTemplate>
    <SeparatorTemplate></SeparatorTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>