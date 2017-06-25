<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listTempiCiclo.ascx.cs" Inherits="KIS.Processi.listTempiCiclo1" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<asp:Repeater runat="server" ID="rptTempi" OnItemCommand="rptTempi_ItemCommand" OnItemDataBound="rptTempi_ItemDataBound">
    <HeaderTemplate>
        <table style="border: 1px dashed blue">
            <thead>
            <tr>
                <th></th>
                <th><asp:Literal runat="server" ID="lblNumOperatori" Text="<%$Resources:lblNumOperatori %>" /></th>
                <th><asp:Literal runat="server" ID="lblSetup" Text="<%$Resources:lblSetup %>" /></th>
                <th><asp:Literal runat="server" ID="lblTempoCiclo" Text="<%$Resources:lblTempoCiclo %>" /></th>
                <th><asp:Literal runat="server" ID="lblDefault" Text="<%$Resources:lblDefault %>" /></th>
            </tr>
                </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:HiddenField runat="server" ID="hNumOp" Value='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' />
                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="/img/iconDelete.png" Height="30px" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="<%$Resources:lblDeleteTC %>" /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %></td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoSetup")).Seconds%></td>
            <td><%#Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours) + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Minutes + ":"
                + ((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).Seconds%></td>
            <td><asp:Image runat="server" ID="imgIsDefault" ImageUrl="~/img/iconComplete.png" Width="30" />
                <asp:ImageButton ImageUrl="~/img/iconChoose.png" Width="30" runat="server" ID="imgMakeDefault" CommandName="MakeDefault" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "NumeroOperatori") %>' ToolTip="<%$Resources:lblMakeDefaultTC %>"/></td>
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