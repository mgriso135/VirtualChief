<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticoloRitardo.ascx.cs" Inherits="KIS.Eventi.ArticoloRitardo" %>
<h3><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />
    <asp:Literal runat="server" ID="lblTitleCfgDelayWarning" Text="<%$Resources:lblTitleCfgDelayWarning %>" /></h3>
<asp:Label runat="server" ID="lbl1" />
<asp:Literal runat="server" ID="lblSegnalaRit" Text="<%$Resources:lblSegnalaRit %>" />:&nbsp;
<asp:Literal runat="server" ID="lblOre" Text="<%$Resources:lblOre %>" />:<asp:DropDownList runat="server" ID="ddlOre" CssClass="dropdown" Width="60px" />&nbsp;
mm:<asp:DropDownList runat="server" ID="ddlMinuti" CssClass="dropdown" Width="60px" />&nbsp;
ss:<asp:DropDownList runat="server" ID="ddlSecondi" CssClass="dropdown" Width="60px" />
<asp:ImageButton runat="server" ID="btnSaveRitMin" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitMin_Click" ToolTip="<%$Resources:lblTTModificaRitMin %>" />
<asp:ImageButton runat="server" ID="btnUndoRitMin" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitMin_Click" ToolTip="<%$Resources:lblTTFormReset %>" />

<table class="table table-condensed table-striped">
    <tr>
        <td>
<h5><asp:Literal runat="server" ID="lblGruppi" Text="<%$Resources:lblGruppi %>" /></h5>
<asp:ImageButton runat="server" ID="btnShowAddRitardoGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoGruppo_Click" />
<asp:Literal runat="server" ID="lblGruppiAdd" Text="<%$Resources:lblGruppiAdd %>" />
<table runat="server" id="frmAddRitardoGruppo" visible="false" class="table table-striped table-condensed table-hover">
    <tr>
        <td><asp:Literal runat="server" ID="lblGruppiSel" Text="<%$Resources:lblGruppiSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblGruppiSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoGruppo_Click" ToolTip="<%$Resources:lblTTGruppiAdd %>" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoGruppo_Click" ToolTip="<%$Resources:lblTTFormReset %>" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListGruppi" OnItemDataBound="rptListGruppi_ItemDataBound" OnItemCommand="rptListGruppi_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-condensed table-hover">
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
        <td><h5><asp:Literal runat="server" ID="lblUtenti" Text="<%$Resources:lblUtenti %>" /></h5>

            <asp:ImageButton runat="server" ID="btnShowAddRitardoUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddRitardoUtente_Click" />
            <asp:Literal runat="server" ID="lblUtentiAdd" Text="<%$Resources:lblUtentiAdd %>" />
<table runat="server" id="frmAddRitardoUtente" visible="false" class="table table-striped table-condensed table-hover">
    <tr>
        <td><asp:Literal runat="server" ID="lblUtentiSel" Text="<%$Resources:lblUtentiSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddRitardoUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblUtentiSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveRitardoUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveRitardoUtente_Click" ToolTip="<%$Resources:lblTTUtentiAdd %>" />
            <asp:ImageButton runat="server" ID="btnUndoRitardoUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoRitardoUtente_Click" ToolTip="<%$Resources:lblTTFormReset %>" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListUtenti" OnItemDataBound="rptListUtenti_ItemDataBound" OnItemCommand="rptListUtenti_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-condensed table-hover">
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