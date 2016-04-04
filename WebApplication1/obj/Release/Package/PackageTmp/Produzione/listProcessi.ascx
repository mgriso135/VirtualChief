<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listProcessi.ascx.cs" Inherits="KIS.Produzione.listProcessi" %>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptProcessi" OnItemCommand="rptProcessi_ItemCommand">
    <HeaderTemplate>
        <table><tr><td></td><td></td></tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:ImageButton runat="server" ID="btnExpand" ImageUrl="/img/iconExpand.gif" Height="30" CommandName="expand" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "process.processID") + "+" + DataBinder.Eval(Container.DataItem, "variant.idVariante") %>' /></td>
            <td>
                <asp:HyperLink runat="server" ID="lnkConfig" NavigateUrl='<%# "configuraProcesso.aspx?pid=" + DataBinder.Eval(Container.DataItem, "process.processID") + "&vid=" + DataBinder.Eval(Container.DataItem, "variant.idVariante")%>'>
                <asp:Image runat="server" ID="btnConfigura" ImageUrl="/img/iconSetup.png" Height="30" />
                    </asp:HyperLink>

            </td>

            <td><%#DataBinder.Eval(Container.DataItem, "process.processID") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "process.processName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "variant.idVariante") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "variant.nomeVariante") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>