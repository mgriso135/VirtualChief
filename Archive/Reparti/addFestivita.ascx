<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addFestivita.ascx.cs" Inherits="KIS.Reparti.addFestivita" %>
<asp:Label runat="server" ID="lbl1" />


<table runat="server" id="tblAddFest">
    <thead>
    <tr style="text-align:center;">
        <th><asp:Literal runat="server" ID="lblInizioFest" Text="<%$Resources:lblInizioFest %>" /></th>
        <th><asp:Literal runat="server" ID="lblFineFest" Text="<%$Resources:lblFineFest %>" /></th>
    </tr>
        </thead><tbody>
    <tr>
        <td><asp:Calendar ID="inizioFest" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraI" Width="60" />:<asp:DropDownList runat="server" ID="MinutoI" Width="60" /></td>
        <td><asp:Calendar ID="fineFest" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraF" Width="60" />:<asp:DropDownList runat="server" ID="MinutoF" Width="60" /></td>
    </tr>
    <tr><td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" id="saveFest" ImageUrl="~/img/iconSave.jpg" Height="40px" OnClick="saveFest_Click" />
        </td></tr>
    <tr><td colspan="2" runat="server" id="colSave" visible="false">
        <asp:Label runat="server" ID="lblListProd" />
        <asp:ImageButton runat="server" ID="imgSave" Width="40" ImageUrl="~/img/iconComplete.png" OnClick="imgSave_Click" />
        <asp:ImageButton runat="server" ID="imgUndo" Width="40" ImageUrl="~/img/iconUndo.png" OnClick="imgUndo_Click" />
        </td></tr></tbody>
</table>