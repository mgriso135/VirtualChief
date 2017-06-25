<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configOrariTrn.ascx.cs" Inherits="KIS.Reparti.configOrariTrn" %>

<asp:Label runat="server" ID="lbl1" />
<h3><asp:Literal runat="server" ID="lblCfgOrari" Text="<%$Resources:lblCfgOrari %>" /></h3>
<table runat="server" id="frmTurno">
    <tr><td><asp:Label runat="server" ID="nomeTurno" /><asp:TextBox runat="server" ID="txtNomeTurno" /><br /></td>
        <td rowspan="2">
            <asp:ImageButton runat="server" ID="editNomeTurno" ImageUrl="/img/edit.png" OnClick="editNomeTurno_Click" Height="40px"/>
            <asp:ImageButton runat="server" ID="saveNomeTurno" ImageUrl="/img/iconSave.jpg" OnClick="saveNomeTurno_Click" Height="40px" />
            <asp:ImageButton runat="server" ID="undoNomeTurno" ImageUrl="/img/iconUndo.png" OnClick="undoNomeTurno_Click" Height="40px" />
        </td>
    </tr>
    <tr><td><asp:DropDownList runat="server" ID="ddlColore" /><br />
        <asp:Label runat="server" ID="lblEsempioColore" Width="155px" Height="15px"/>
        </td></tr>
</table>
<br />
<asp:Repeater runat="server" ID="rptOrari" OnItemCommand="rptOrari_ItemCommand" OnItemCreated="rptOrari_ItemDataBound">
    <HeaderTemplate>
        <table>
            <thead>
            <tr>
                <td></td>
                <td><asp:Literal runat="server" ID="lblGiornoInizio" Text="<%$Resources:lblGiornoInizio %>" /></td>
                <td><asp:Literal runat="server" ID="lblOraInizio" Text="<%$Resources:lblOraInizio %>" /></td>
                <td<asp:Literal runat="server" ID="lblGiornoFine" Text="<%$Resources:lblGiornoFine %>" /></td>
                <td><asp:Literal runat="server" ID="lblOraFine" Text="<%$Resources:lblOraFine %>" /></td>
                <td><asp:Literal runat="server" ID="lblDurataTurno" Text="<%$Resources:lblDurataTurno %>" /></td>
            </tr>
                </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td><asp:ImageButton runat="server" ID="deleteInterval" CommandName="delete" ImageUrl="/img/iconDelete.png" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "idIntervallo") %>' Height="30px"/></td>
            <td><%#DataBinder.Eval(Container.DataItem, "GiornoInizio") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "OraInizio") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "GiornoFine") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "OraFine") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Durata") %></td>
        </tr>

    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:ImageButton runat="server" ID="imgAddOrario" ImageUrl="/img/iconAdd2.png" OnClick="imgAddOrario_Click" Height="50px" ToolTip="<%$Resources:lblTTAddOrario %>" />
<table runat="server" id="formAddOrario">
    <tr>
        <td><asp:Literal runat="server" ID="lblInizio" Text="<%$Resources:lblInizio %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlDInizio" Width="100">
            <asp:ListItem Text="<%$Resources:lblLunedi %>" Value="1" />
            <asp:ListItem Text="<%$Resources:lblMartedi %>" Value="2" />
            <asp:ListItem Text="<%$Resources:lblMercoledi %>" Value="3" />
            <asp:ListItem Text="<%$Resources:lblGiovedi %>" Value="4" />
            <asp:ListItem Text="<%$Resources:lblVenerdi %>" Value="5" />
            <asp:ListItem Text="<%$Resources:lblSabato %>" Value="6" />
            <asp:ListItem Text="<%$Resources:lblDomenica %>" Value="0" />
            </asp:DropDownList>
        </td>
        <td><asp:DropDownList runat="server" ID="ddlOInizio" Width="60" />:<asp:DropDownList runat="server" ID="ddlMInizio" Width="60" /></td>
    </tr>
        <tr>
        <td><asp:Literal runat="server" ID="lblFine" Text="<%$Resources:lblFine %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlDFine" Width="100">
            <asp:ListItem Text="<%$Resources:lblLunedi %>" Value="1" />
            <asp:ListItem Text="<%$Resources:lblMartedi %>" Value="2" />
            <asp:ListItem Text="<%$Resources:lblMercoledi %>" Value="3" />
            <asp:ListItem Text="<%$Resources:lblGiovedi %>" Value="4" />
            <asp:ListItem Text="<%$Resources:lblVenerdi %>" Value="5" />
            <asp:ListItem Text="<%$Resources:lblSabato %>" Value="6" />
            <asp:ListItem Text="<%$Resources:lblDomenica %>" Value="0" />
            </asp:DropDownList>
        </td>
        <td><asp:DropDownList runat="server" ID="ddlOFine" Width="60" />:<asp:DropDownList runat="server" ID="ddlMFine" Width="60" /></td>
    </tr>
    <tr>
        <td colspan="3" style="text-align:center;">
            <asp:ImageButton runat="server" ID="saveOrario" ToolTip="<%$Resources:lblSalva %>" OnClick="saveOrario_Click" ImageUrl="/img/iconSave.jpg" Height="40px" />
        </td>
    </tr>
</table>