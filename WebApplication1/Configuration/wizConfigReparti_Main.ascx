<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wizConfigReparti_Main.ascx.cs" Inherits="KIS.Configuration.wizConfigReparti_Main1" %>

<h3><asp:Literal runat="server" ID="lblTitleAddReparto" Text="<%$Resources:lblTitleAddReparto %>" /></h3>
<asp:Label runat="server" ID="lbl1"  CssClass="text-info"/>
<table runat="server" id="frmAddReparto" class="table">
    <tr>
        <td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" />:</td>
        <td><asp:TextBox runat="server" ID="nome" />
            <asp:RequiredFieldValidator runat="server" ID="valNome" ErrorMessage="<%$Resources:lblValReqField %>" ControlToValidate="nome" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblDesc" Text="<%$Resources:lblDesc %>" />:</td>
        <td><asp:TextBox runat="server" ID="descrizione" TextMode="MultiLine" />
            <asp:RequiredFieldValidator runat="server" ID="valDescrizione" ErrorMessage="<%$Resources:lblValReqField %>" ControlToValidate="descrizione" ForeColor="Red" ValidationGroup="addReparto" />
        </td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblTimezone" Text="<%$Resources:lblTimezone %>" />:</td>
        <td><asp:DropDownList runat="server" ID="ddlTimezones"/></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ImageButton runat="server" ID="save" OnClick="save_Click" ImageUrl="/img/iconSave.jpg" Height="50px" ToolTip="<%$Resources:lblTTSalva %>" ValidationGroup="addReparto" />
            <asp:ImageButton runat="server" ID="reset" OnClick="reset_Click" ImageUrl="/img/iconUndo.png" Height="50px" ToolTip="<%$Resources:lblTTUndo %>" ValidationGroup="addReparto" />
        </td>
    </tr>
</table>