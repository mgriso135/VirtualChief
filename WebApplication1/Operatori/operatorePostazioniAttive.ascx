<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="operatorePostazioniAttive.ascx.cs" Inherits="KIS.Operatori.operatorePostazioniAttive" %>

<h3>Postazioni di lavoro attive</h3>


<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptPostazioniAttive" OnItemDataBound="rptPostazioniAttive_ItemDataBound" OnItemCommand="rptPostazioniAttive_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td>Altri utenti in postazione</td>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1" style="border: 1px dashed groove; font-size:16px; font-family: Calibri;">
            <td><asp:ImageButton runat="server" ID="btnCheckOut" CommandName="checkOut" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ImageUrl="/img/iconCheckIn5.jpg" ToolTip="Esci dalla postazione di lavoro" Height="40" /></td>
            <td><asp:HyperLink runat="server" ID="lnkGOTOWORK" NavigateUrl='<%# "doTasksPostazione.aspx?id=" + DataBinder.Eval(Container.DataItem, "id") %>'>
                <asp:Image runat="server" ID="btnWork" ImageUrl="/img/iconTask2.jpg"  Height="40" ToolTip="Esegui i task assegnati alla postazione" />
                </asp:HyperLink>
            </td>
            <td><asp:HiddenField runat="server" ID="id" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                <%# DataBinder.Eval(Container.DataItem, "id") %></td>
            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
            <td><asp:Label runat="server" ID="lblUserLogged" /></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="60000" />
        </ContentTemplate>
    </asp:UpdatePanel>