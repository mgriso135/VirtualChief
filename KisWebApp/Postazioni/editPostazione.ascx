<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editPostazione.ascx.cs" Inherits="KIS.Postazioni.editPostazione_ascx" %>

<h3><asp:Literal runat="server" ID="lblTitleModPostazione" Text="<%$Resources:lblTitleModPostazione %>" />
    </h3>
<asp:Label runat="server" ID="err" />
<table class="table">
    
    <tr><td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" /></td>
        <td><asp:TextBox runat="server" ID="postName" /></td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblDescrizione" Text="<%$Resources:lblDescrizione %>" /></td>
        <td><asp:TextBox TextMode="MultiLine" runat="server" ID="postDesc" /></td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblAutoCheckIn" Text="<%$Resources:lblAutoCheckIn %>" /></td>
        <td><asp:dropdownlist runat="server" ID="ddlAutoCheckIn">
            <asp:ListItem Value="0" Text="<%$Resources:lblLiDisabled %>" />
            <asp:ListItem Value="1" Text="<%$Resources:lblLiEnabled %>" />
            </asp:dropdownlist></td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:ImageButton CssClass="img-rounded" runat="server" ID="save" OnClick="save_Click" ImageUrl="~/img/iconSave.jpg" Height="60px" />
            <asp:ImageButton CssClass="img-rounded" runat="server" ID="reset" OnClick="reset_Click" ImageUrl="~/img/iconUndo.png" Height="60px" />
        </td>
    </tr>
</table>