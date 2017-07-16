<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="userLoginPostazione.ascx.cs" Inherits="KIS.Operatori.userLoginPostazione" %>
<h3><asp:Literal runat="server" ID="lblTitleCheckIn" Text="<%$Resources:lblTitleCheckIn %>" /></h3>
<asp:Label runat="server" ID="lblNome" />

<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
<asp:Repeater runat="server" ID="rptPostazioni" OnItemDataBound="rptPostazioni_ItemDataBound" OnItemCommand="rptPostazioni_ItemCommand">
    <HeaderTemplate>
        <table class="table table-hover table-striped table-condensed">
            <thead>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td><asp:Literal runat="server" ID="lblUserInPlace" Text="<%$Resources:lblUserInPlace %>" /></td>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="border: 1px dashed groove; font-size:16px; font-family: Calibri;">
        <td><asp:ImageButton ToolTip="<%$Resources:lblCheckIn %>" runat="server" ID="btnCheckIn" ImageUrl="~/img/iconCheckIn4.jpg" Height="40" CommandName="checkIn" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
        <td><asp:HiddenField runat="server" ID="id" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
            <%#DataBinder.Eval(Container.DataItem, "ID") %></td>
        <td><%#DataBinder.Eval(Container.DataItem, "name") %></td>
            <td><asp:Label runat="server" ID="lblUserLogged" /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

<asp:Label runat="server" ID="lbl1" />
        <asp:Timer runat="server" ID="timer" Interval="60000" OnTick="timer_Tick"  />
        </ContentTemplate>
    </asp:UpdatePanel>