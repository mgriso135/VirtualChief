<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepartoRitardo.ascx.cs" Inherits="KIS.Eventi.RepartoRitardo" %>

<h5><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />Configurazione allarme ritardi</h5>
<asp:Label runat="server" ID="lbl1" />
<asp:UpdatePanel runat="server" ID="upd1">
                <ContentTemplate>
Segnala ritardi maggiori a: ore:<asp:DropDownList runat="server" ID="ddlOre" CssClass="dropdown" Width="70px"/>
mm:<asp:DropDownList runat="server" ID="ddlMinuti" CssClass="dropdown" Width="70px" />ss:<asp:DropDownList runat="server" ID="ddlSecondi" CssClass="dropdown" Width="70px" />
<asp:ImageButton runat="server" ID="btnSaveRitMin" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitMin_Click" ToolTip="Modifica il ritardo minimo da segnalare" />
<asp:ImageButton runat="server" ID="btnUndoRitMin" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitMin_Click" ToolTip="Resetta il form" />


<table style="border-color: blue; border-style: dashed; border-width: 1px">
    <tr>
        <td>
<h5>Gruppi a cui segnalare il ritardo</h5>
            
<asp:ImageButton runat="server" ID="btnShowAddRitardoGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoGruppo_Click" />
Aggiungi un gruppo a cui inviare segnalazioni di ritardi
<table runat="server" id="frmAddRitardoGruppo" visible="false">
    <tr>
        <td>Seleziona un gruppo:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="Seleziona un gruppo" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoGruppo_Click" ToolTip="Aggiungi il gruppo selezionato all'elenco" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoGruppo_Click" ToolTip="Resetta il form" />
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
        <td><h5>Utenti a cui segnalare il ritardo</h5>

            <asp:ImageButton runat="server" ID="btnShowAddRitardoUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoUtente_Click" />
Aggiungi un utente a cui inviare segnalazioni di ritardi
<table runat="server" id="frmAddRitardoUtente" visible="false">
    <tr>
        <td>Seleziona un gruppo:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="Seleziona un utente" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoUtente_Click" ToolTip="Aggiungi l'utente selezionato all'elenco" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoUtente_Click" ToolTip="Resetta il form" />
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
                <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnShowAddRitardoGruppo" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnUndoRitMin" EventName="Click" />
        </Triggers>
                </asp:UpdatePanel>