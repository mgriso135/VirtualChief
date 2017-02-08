<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="baseInfo.ascx.cs" Inherits="KIS.Personal.baseInfo" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<table class="table table-striped table-hover table-condensed">
    <tr>
        <td><asp:Label runat="server" ID="lblDescNome" meta:resourckey="lblDescNome" /></td>
        <td><asp:Label runat="server" ID="lblNome" />
            <asp:TextBox runat="server" ID="txtNome" />
        </td>
        <td><asp:ImageButton runat="server" ID="imgEditNome" OnClick="imgEditNome_Click" ImageUrl="~/img/edit.png" Height="30" />
            <asp:ImageButton runat="server" ID="imgSaveNome" OnClick="imgSaveNome_Click" ImageUrl="~/img/iconSave.jpg" Height="30" />
            <asp:ImageButton runat="server" ID="imgUndoNome" OnClick="imgUndoNome_Click" ImageUrl="~/img/iconUndo.png" Height="30" />
        </td>
    </tr>
    <tr>
        <td><asp:Label runat="server" ID="lblDescCognome" meta:resourckey="lblDescCognome" /></td>
        <td><asp:Label runat="server" ID="lblCognome" />
            <asp:TextBox runat="server" ID="txtCognome" />
        </td>
        <td><asp:ImageButton runat="server" ID="imgEditCognome" OnClick="imgEditCognome_Click" ImageUrl="~/img/edit.png" Height="30" />
            <asp:ImageButton runat="server" ID="imgSaveCognome" OnClick="imgSaveCognome_Click" ImageUrl="~/img/iconSave.jpg" Height="30" />
            <asp:ImageButton runat="server" ID="imgUndoCognome" OnClick="imgUndoCognome_Click" ImageUrl="~/img/iconUndo.png" Height="30" />
        </td>
    </tr>
</table>
        </ContentTemplate>
    </asp:UpdatePanel>