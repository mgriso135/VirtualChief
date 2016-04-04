<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListAnalysisOperatori.ascx.cs" Inherits="KIS.Analysis.ListAnalysisOperatori1" %>

<asp:Label runat="server" ID="lbl1" />

<asp:Repeater runat="server" ID="rptOperatori">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <thead>
            <tr>
            <th></th>
            <th>Nome</th>
            <th>Username</th>
                </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:HyperLink runat="server" ID="lnkDetailAnalisiOperatore" NavigateUrl='<%#"DetailAnalysisOperatore.aspx?usr=" + DataBinder.Eval(Container.DataItem, "username") %>'>
                <asp:Image runat="server" ID="imgDetailAnalisiOperatore" Height="40" ImageUrl="~/img/iconView.png" />
                </asp:HyperLink></td>
                <td><%#DataBinder.Eval(Container.DataItem, "Cognome") %>&nbsp;<%#DataBinder.Eval(Container.DataItem, "name") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Username") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>