<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addStraordinario.ascx.cs" Inherits="KIS.Reparti.addStraordinario" %>

<asp:Label runat="server" ID="lbl1" />
<table runat="server" id="tblAddStraord">
    <tr style="text-align:center;"><td>INIZIO STRAORDINARIO</td>
        <td>FINE STRAORDINARIO</td>
    </tr>
    <tr style="text-align:center;">
        <td><asp:Calendar ID="inizioStraord" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraI" Width="60" />:<asp:DropDownList runat="server" ID="MinutoI" Width="60" /></td>
        <td><asp:Calendar ID="fineStraord" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraF" Width="60" />:<asp:DropDownList runat="server" ID="MinutoF" Width="60" /></td>
    </tr>
    <tr><td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" id="saveStraord" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="saveStraord_Click" />
        </td></tr>
</table>