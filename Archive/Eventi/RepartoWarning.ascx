<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepartoWarning.ascx.cs" Inherits="KIS.Eventi.RepartoWarning" %>
<h5><asp:Image Height="50" runat="server" ID="imgTitle" ImageUrl="~/img/iconEmail.png" />
    <asp:Literal runat="server" ID="lblTitleCfgWarning" Text="<%$Resources:lblTitleCfgWarning %>" />
    </h5>
<asp:Label runat="server" ID="lbl1" />
<asp:UpdatePanel runat="server" ID="upd1">
                <ContentTemplate>
<table style="border-color: blue; border-style: dashed; border-width: 1px">
    <tr>
        <td>
<h5><asp:Literal runat="server" ID="lblTitleCfgWarningGroup" Text="<%$Resources:lblTitleCfgWarningGroup %>" /></h5>
<asp:ImageButton runat="server" ID="btnShowAddWarningGruppo" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningGruppo_Click" />
<asp:Literal runat="server" ID="lblTitleCfgWarningGroupAdd" Text="<%$Resources:lblTitleCfgWarningGroupAdd %>" />
<table runat="server" id="frmAddWarningGruppo" visible="false">
    <tr>
        <td><asp:Literal runat="server" ID="lblTitleCfgWarningGroupSel" Text="<%$Resources:lblTitleCfgWarningGroupSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningGruppo" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblTitleCfgWarningGroupSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningGruppo" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningGruppo_Click" ToolTip="<%$Resources:lblTitleCfgWarningGroupAddGroup %>" />
            <asp:ImageButton runat="server" ID="btnUndoWarningGruppo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningGruppo_Click" ToolTip="<%$Resources:lblTitleCfgWarningGroupFormReset %>" />
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
        <td><h5><asp:Literal runat="server" ID="lblTitleCfgWarningUsers" Text="<%$Resources:lblTitleCfgWarningUsers %>" /></h5>

            <asp:ImageButton runat="server" ID="btnShowAddWarningUtente" ImageUrl="~/img/iconAdd2.png" Height="30" OnClick="btnShowAddWarningUtente_Click" />
<asp:Literal runat="server" ID="lblTitleCfgWarningUserAdd" Text="<%$Resources:lblTitleCfgWarningUserAdd %>" />
<table runat="server" id="frmAddWarningUtente" visible="false">
    <tr>
        <td><asp:Literal runat="server" ID="lblTitleCfgWarningUserSel" Text="<%$Resources:lblTitleCfgWarningUserSel %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlAddWarningUtente" AppendDataBoundItems="true">
            <asp:ListItem Text="<%$Resources:lblTitleCfgWarningUserSel %>" Value="" />
            </asp:DropDownList></td>
        <td>
            <asp:ImageButton runat="server" ID="btnSaveWarningUtente" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSaveWarningUtente_Click" ToolTip="<%$Resources:lblTitleCfgWarningUserSelAdd %>" />
            <asp:ImageButton runat="server" ID="btnUndoWarningUtente" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="btnUndoWarningUtente_Click" ToolTip="<%$Resources:lblTitleCfgWarningGroupFormReset %>" />
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