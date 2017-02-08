<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configOrariTrn.ascx.cs" Inherits="KIS.Reparti.configOrariTrn" %>

<asp:Label runat="server" ID="lbl1" />
<h3>Configurazione orari di lavoro</h3>
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
            <tr>
                <td></td><td>Giorno inizio</td><td>Ora inizio</td><td>Giorno fine</td><td>Ora fine</td><td>Durata turno</td>
            </tr>
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
<asp:ImageButton runat="server" ID="imgAddOrario" ImageUrl="/img/iconAdd2.png" OnClick="imgAddOrario_Click" Height="50px" ToolTip="Aggiungi un orario di lavoro" />
<table runat="server" id="formAddOrario">
    <tr>
        <td>Inizio:</td>
        <td><asp:DropDownList runat="server" ID="ddlDInizio" Width="100">
            <asp:ListItem Text="Lunedì" Value="1" />
            <asp:ListItem Text="Martedì" Value="2" />
            <asp:ListItem Text="Mercoledì" Value="3" />
            <asp:ListItem Text="Giovedì" Value="4" />
            <asp:ListItem Text="Venerdì" Value="5" />
            <asp:ListItem Text="Sabato" Value="6" />
            <asp:ListItem Text="Domenica" Value="0" />
            </asp:DropDownList>
        </td>
        <td><asp:DropDownList runat="server" ID="ddlOInizio" Width="60" />:<asp:DropDownList runat="server" ID="ddlMInizio" Width="60" /></td>
    </tr>
        <tr>
        <td>Fine:</td>
        <td><asp:DropDownList runat="server" ID="ddlDFine" Width="100">
            <asp:ListItem Text="Lunedì" Value="1" />
            <asp:ListItem Text="Martedì" Value="2" />
            <asp:ListItem Text="Mercoledì" Value="3" />
            <asp:ListItem Text="Giovedì" Value="4" />
            <asp:ListItem Text="Venerdì" Value="5" />
            <asp:ListItem Text="Sabato" Value="6" />
            <asp:ListItem Text="Domenica" Value="0" />
            </asp:DropDownList>
        </td>
        <td><asp:DropDownList runat="server" ID="ddlOFine" Width="60" />:<asp:DropDownList runat="server" ID="ddlMFine" Width="60" /></td>
    </tr>
    <tr>
        <td colspan="3" style="text-align:center;">
            <asp:ImageButton runat="server" ID="saveOrario" ToolTip="Salva" OnClick="saveOrario_Click" ImageUrl="/img/iconSave.jpg" Height="40px" />
        </td>
    </tr>
</table>