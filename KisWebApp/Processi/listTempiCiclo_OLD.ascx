<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTempiCiclo_OLD.ascx.cs" Inherits="KIS.Processi.listTempiCiclo" %>

<asp:Label runat="server" ID="lbl1" />
<asp:HiddenField runat="server" ID="taskID" />
<asp:HiddenField runat="server" ID="varID" />
<asp:Repeater runat="server" ID="rptTempi" OnItemCommand="rptTempi_ItemCommand">
    <HeaderTemplate>
        <table style="border: 1px dashed blue">
            <tr>
                <td></td>
                <td>Numero operatori</td>
                <td>Tempo di default</td>
                <td>Default</td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:ImageButton runat="server" ID="btnDelete" ImageUrl="/img/iconDelete.png" Height="30px" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="Cancella il tempo ciclo" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Tempo") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Default") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>