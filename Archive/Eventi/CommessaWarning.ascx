<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommessaWarning.ascx.cs" Inherits="KIS.Eventi.CommessaWarning" %>
<h3><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />
    <asp:Literal runat="server" ID="lblTitleCfgWarn" Text="<%$Resources:lblTitleCfgWarn %>" /></h3>
<asp:Label runat="server" ID="lbl1" />

<table class="table table-striped table-condensed table-hover">
    <tbody>
    <tr>
        <td>
<h5><asp:Literal runat="server" ID="lblCfgWarnGroup" Text="<%$Resources:lblCfgWarnGroup %>" /></h5>
<asp:ImageButton runat="server" ID="btnShowAddWarningGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningGruppo_Click" />
<asp:Literal runat="server" ID="lblCfgWarnGroupAdd" Text="<%$Resources:lblCfgWarnGroupAdd %>" />
<table runat="server" id="frmAddWarningGruppo" visible="false" class="table table-condensed">
    <tr>
        <td><asp:Literal runat="server" ID="lblCfgWarnGroupSel" Text="<%$Resources:lblCfgWarnGroupSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblCfgWarnGroupSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningGruppo_Click" ToolTip="<%$Resources:lblTTAddGroup %>" />
            <asp:ImageButton runat="server" ID="btnUndoWarningGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningGruppo_Click" ToolTip="<%$Resources:lblTTResetForm %>" />
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
        <td><h5><asp:Literal runat="server" ID="lblCfgWarnUser" Text="<%$Resources:lblCfgWarnUser %>" /></h5>

            <asp:ImageButton runat="server" ID="btnShowAddWarningUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningUtente_Click" />
<asp:Literal runat="server" ID="lblCfgWarnUserAdd" Text="<%$Resources:lblCfgWarnUserAdd %>" />
<table runat="server" id="frmAddWarningUtente" visible="false">
    <tr>
        <td><asp:Literal runat="server" ID="lblCfgWarnUserSel" Text="<%$Resources:lblCfgWarnUserSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblCfgWarnUserSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningUtente_Click" ToolTip="<%$Resources:lblTTAddUser %>" />
            <asp:ImageButton runat="server" ID="btnUndoWarningUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningUtente_Click" ToolTip="<%$Resources:lblTTResetForm %>" />
        </td>
    </tr>
</table>

<asp:Repeater runat="server" ID="rptListUtenti" OnItemDataBound="rptListUtenti_ItemDataBound" OnItemCommand="rptListUtenti_ItemCommand">
    <HeaderTemplate>
        <table class="table table-striped table-condensed table-hover">
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr2">
            <td><asp:ImageButton ImageUrl="~/img/iconDelete.png" Height="30" runat="server" ID="btnDeleteUtente" CommandName="deleteUtente" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "username") %>' /></td>
            <td><%#DataBinder.Eval(Container.DataItem, "username") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "FullName") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>




        </td>
        </tr>
        </tbody>
    </table>