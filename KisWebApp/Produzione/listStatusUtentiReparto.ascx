<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listStatusUtentiReparto.ascx.cs" Inherits="KIS.Produzione.listStatusUtentiReparto" %>
<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>

<asp:Label runat="server" ID="lblUser" />
        <asp:Label runat="server" ID="lblData" />
        <asp:Repeater runat="server" ID="rptUserList" OnItemDataBound="rptUserList_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <tr>
                        <td>User</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:Label runat="server" ID="lblUtente" Font-Size="16" />
                        <asp:HiddenField runat="server" ID="lblIDUser" Value='<%#DataBinder.Eval(Container.DataItem, "username") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Timer runat="server" ID="timer1" Interval="100000" OnTick="timer1_Tick" />
    </ContentTemplate>
</asp:UpdatePanel>