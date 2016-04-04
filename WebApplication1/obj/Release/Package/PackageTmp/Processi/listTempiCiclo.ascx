<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTempiCiclo.ascx.cs" Inherits="KIS.Processi.listTempiCiclo1" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Repeater runat="server" ID="rptTempi" OnItemCommand="rptTempi_ItemCommand" OnItemDataBound="rptTempi_ItemDataBound">
    <HeaderTemplate>
        <table style="border: 1px dashed blue">
            <thead>
            <tr>
                <th></th>
                <th>Numero operatori</th>
                <th>Setup</th>
                <th>Tempo ciclo</th>
                <th>Default</th>
            </tr>
                </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:HiddenField runat="server" ID="hNumOp" Value='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' />
                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="/img/iconDelete.png" Height="30px" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="Cancella il tempo ciclo" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %></td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Seconds%></td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Seconds%></td>
            <td><asp:Image runat="server" ID="imgIsDefault" ImageUrl="~/img/iconComplete.png" Width="30" />
                <asp:ImageButton ImageUrl="~/img/iconChoose.png" Width="30" runat="server" ID="imgMakeDefault" CommandName="MakeDefault" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="Rendi il tempo di default"/></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Label runat="server" ID="lbl1" />
        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="10000" />
        </ContentTemplate>
    </asp:UpdatePanel>