<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configTurni.ascx.cs" Inherits="KIS.Produzione.configTurni" %>

<h3>Turni di lavoro</h3>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:HyperLink Target="_blank" runat="server" ID="lnkCalendarioTotale" NavigateUrl='<%#"~/Reparti/showCalendarFesteStraordinari.aspx?id="%>'>
        <asp:Image ID="imgCalendarioTotale" Width="60px" ImageUrl="/img/iconCalendar.png" runat="server" CssClass="img-rounded" />
            </asp:HyperLink>
<asp:Repeater runat="server" ID="rptTurni" OnItemCommand="rptTurni_ItemCommand" OnItemDataBound="rptTurni_ItemDataBound">
    <HeaderTemplate><table class="table table-striped table-hover table-condensed">
        <thead>
        <tr>
            <td></td>
            <td style="font-size: 12px; font-weight: bold">Turno</td>
            <td></td>
        </tr>
            </thead>
        <tbody>
        </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:ImageButton CssClass="img-circle" runat="server" ImageUrl="/img/iconDelete.png" Height="30px" CommandName="delete" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "id") %>' />
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "nome") %></td>
            <td>
                <a href="/Reparti/configOrariTurno.aspx?id=<%#DataBinder.Eval(Container.DataItem, "id") %>">
                <asp:Image  CssClass="img-circle" runat="server" ImageUrl="/img/iconClock.png" Height="30px" ToolTip="Gestisci gli orari di lavoro del turno" />
                </a>

            </td>
            <td>
                <asp:HyperLink Width="30px" Target="_blank" NavigateUrl='<%#"~/Reparti/manageCalendarFesteStraordinari.aspx?id=" + DataBinder.Eval(Container.DataItem, "id") %>' runat="server" ID="lnkCalendar" ToolTip="Gestisci il calendario festività / straordinari per il turno">
        <asp:Image ID="Image1" Width="30px" ImageUrl="/img/iconCalendar.png" runat="server" CssClass="img-rounded" />
        </asp:HyperLink>
            </td>
            <td>
                <asp:HyperLink Width="30px" Target="_blank" NavigateUrl='<%#"~/Reparti/risorseTurno.aspx?idTurno=" + DataBinder.Eval(Container.DataItem, "id") %>' runat="server" ID="lnkLabor" ToolTip="Gestisci le risorse assegnate al turno di lavoro">
        <asp:Image ID="Image2" Width="30px" ImageUrl="~/img/iconLabor.png" runat="server" CssClass="img-rounded" />
        </asp:HyperLink>
            </td>
        </tr>

    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table></FooterTemplate>
</asp:Repeater>

<asp:ImageButton CssClass="img-circle" runat="server" ID="showAddTurno" ImageUrl="/img/iconAdd.jpg" OnClick="showAddTurno_Click" Height="40px" ToolTip="Aggiungi un nuovo turno" />Aggiungi un nuovo turno
<table runat="server" id="addTurno" class="table table-bordered">
    <tr>
        <td>Nome turno
            <asp:TextBox runat="server" ID="nomeTurno" ValidationGroup="turno" />
            <asp:RequiredFieldValidator runat="server" ErrorMessage="Campo obbligatorio" ControlToValidate="nomeTurno" ForeColor="Red" ValidationGroup="turno" />
        </td>
    </tr>
    <tr>
        <td>
            Colore
            <asp:DropDownlist runat="server" ID="coloreTurno"></asp:DropDownlist>
        </td>
    </tr>
    <tr>
         <td>
            <asp:ImageButton runat="server" id="save" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="save_Click" ValidationGroup="turno" />
            <asp:ImageButton runat="server" ID="reset" ImageUrl="/img/iconUndo.png" Height="40px" OnClick="reset_Click" />
        </td>
    </tr>
</table>
        </ContentTemplate>
    </asp:UpdatePanel>