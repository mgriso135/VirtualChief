<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommessaRitardo.ascx.cs" Inherits="KIS.Eventi.CommessaRitardo" %>
<h3><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />
    <asp:literal runat="server" id="lblTitleCfgDelay" Text="<%$Resources:lblTitleCfgDelay %>" />
    </h3>
<asp:Label runat="server" ID="lbl1" />
<asp:literal runat="server" id="lblSegnalaMaggioreDi" Text="<%$Resources:lblSegnalaMaggioreDi %>" />&nbsp;
<asp:literal runat="server" id="lblOre" Text="<%$Resources:lblOre %>" />:
<asp:DropDownList runat="server" ID="ddlOre" CssClass="dropdown" Width="70px" />&nbsp;
mm:<asp:DropDownList runat="server" ID="ddlMinuti" CssClass="dropdown" Width="60px" />&nbsp;
ss:<asp:DropDownList runat="server" ID="ddlSecondi" CssClass="dropdown" Width="60px" />
<asp:ImageButton runat="server" ID="btnSaveRitMin" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitMin_Click" ToolTip="<%$Resources:lblTTModificaRitardoMin %>" />
<asp:ImageButton runat="server" ID="btnUndoRitMin" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitMin_Click" ToolTip="<%$Resources:lblTTFormReset %>" />

<table class="table table-striped table-condensed table-hover">
    <tbody>
    <tr>
        <td>
<h5><asp:literal runat="server" id="lblGruppiSegnala" Text="<%$Resources:lblGruppiSegnala %>" /></h5>
<asp:ImageButton runat="server" ID="btnShowAddRitardoGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoGruppo_Click" />
<asp:literal runat="server" id="lblGruppiAdd" Text="<%$Resources:lblGruppiAdd %>" />
<table runat="server" id="frmAddRitardoGruppo" visible="false" class="table table-condensed">

    <tr>
        <td><asp:literal runat="server" id="lblGruppiSel" Text="<%$Resources:lblGruppiSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblGruppiSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoGruppo_Click" ToolTip="<%$Resources:lblTTAddSelGroup %>" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoGruppo_Click" ToolTip="<%$Resources:lblTTFormReset %>" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListGruppi" OnItemDataBound="rptListGruppi_ItemDataBound" OnItemCommand="rptListGruppi_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-condensed table-hover">
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton ImageUrl="~/img/iconDelete.png" Height="30" runat="server" ID="btnDeleteGruppo" CommandName="deleteGruppo" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ID") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Nome") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Descrizione") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
            </td>
        <td><h5><asp:literal runat="server" id="lblUtentiList" Text="<%$Resources:lblUtentiList %>" /></h5>

            <asp:ImageButton runat="server" ID="btnShowAddRitardoUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoUtente_Click" />
<asp:literal runat="server" id="lblUtentiAdd" Text="<%$Resources:lblUtentiAdd %>" />
<table runat="server" id="frmAddRitardoUtente" visible="false" class="table table-condensed">
    <tbody>
    <tr>
        <td><asp:literal runat="server" id="lblUtentiSel" Text="<%$Resources:lblUtentiSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblUtentiSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoUtente_Click" ToolTip="<%$Resources:lblTTAddSelUser %>" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoUtente_Click" ToolTip="<%$Resources:lblTTFormReset %>" />
        </td>
    </tr>
        </tbody>
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
        </tbody>
    </table>