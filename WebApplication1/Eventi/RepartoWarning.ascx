<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepartoWarning.ascx.cs" Inherits="KIS.Eventi.RepartoWarning" %>
<h5><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />Configurazione allarme warning</h5>
<asp:Label runat="server" ID="lbl1" />
<asp:UpdatePanel runat="server" ID="upd1">
                <ContentTemplate>
<table style="border-color: blue; border-style: dashed; border-width: 1px">
    <tr>
        <td>
<h5>Gruppi a cui segnalare gli warning</h5>
<asp:ImageButton runat="server" ID="btnShowAddWarningGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningGruppo_Click" />
Aggiungi un gruppo a cui inviare segnalazioni di warning
<table runat="server" id="frmAddWarningGruppo" visible="false">
    <tr>
        <td>Seleziona un gruppo:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="Seleziona un gruppo" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningGruppo_Click" ToolTip="Aggiungi il gruppo selezionato all'elenco" />
            <asp:ImageButton runat="server" ID="btnUndoWarningGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningGruppo_Click" ToolTip="Resetta il form" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListGruppi" OnItemDataBound="rptListGruppi_ItemDataBound" OnItemCommand="rptListGruppi_ItemCommand">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton ImageUrl="~/img/iconDelete.png" Height="30" runat="server" ID="btnDeleteGruppo" CommandName="deleteGruppo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Nome") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Descrizione") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
            </td>
        <td><h5>Utenti a cui segnalare gli warning</h5>

            <asp:ImageButton runat="server" ID="btnShowAddWarningUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningUtente_Click" />
Aggiungi un utente a cui inviare segnalazioni di warning
<table runat="server" id="frmAddWarningUtente" visible="false">
    <tr>
        <td>Seleziona un gruppo:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="Seleziona un utente" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningUtente_Click" ToolTip="Aggiungi l'utente selezionato all'elenco" />
            <asp:ImageButton runat="server" ID="btnUndoWarningUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningUtente_Click" ToolTip="Resetta il form" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListUtenti" OnItemDataBound="rptListUtenti_ItemDataBound" OnItemCommand="rptListUtenti_ItemCommand">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr2">
            <td><asp:ImageButton ImageUrl="~/img/iconDelete.png" Height="30" runat="server" ID="btnDeleteUtente" CommandName="deleteUtente" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "username") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "username") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "FullName") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>




        </td>
        </tr>
    </table>
                    </ContentTemplate>
    </asp:UpdatePanel>