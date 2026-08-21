<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editTask.ascx.cs" Inherits="KIS.Processi.editTask1" %>

<table style="background-color: white; border: 1px dashed black;">
        <tr>
            <td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" />:</td>
            <td>
    <asp:TextBox runat="server" ID="editTaskNome" />
                </td>
            </tr>
        <tr>
            <td><asp:Literal runat="server" ID="lblDescrizione" Text="<%$Resources:lblDescrizione %>" />:</td>
            <td>
    <asp:TextBox runat="server" TextMode="MultiLine" ID="editTaskDesc" />
        </td>
        </tr>
    <tr>
        <td colspan="2">
    <asp:ImageButton runat="server" ID="editTaskSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="editTaskSave_Click" />
    <asp:ImageButton runat="server" ID="editTaskUndo" ImageUrl="~/img/iconUndo.png" Height="40" OnClick="editTaskUndo_Click" />
            </td>
    </tr>
<tr>
    <td colspan="2"><asp:Label runat="server" ID="lbl1" /></td>
</tr>
        </table>
