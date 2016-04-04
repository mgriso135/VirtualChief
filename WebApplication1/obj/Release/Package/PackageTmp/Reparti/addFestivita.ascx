<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addFestivita.ascx.cs" Inherits="KIS.Reparti.addFestivita" %>
<asp:Label runat="server" ID="lbl1" />


<table runat="server" id="tblAddFest">
    <tr style="text-align:center;"><td>INIZIO FESTIVITA'</td>
        <td>FINE FESTIVITA'</td>
    </tr>
    <tr style="text-align:center;">
        <td><asp:Calendar ID="inizioFest" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraI" Width="60" />:<asp:DropDownList runat="server" ID="MinutoI" Width="60" /></td>
        <td><asp:Calendar ID="fineFest" runat="server" /><br />
        <asp:DropDownList runat="server" ID="OraF" Width="60" />:<asp:DropDownList runat="server" ID="MinutoF" Width="60" /></td>
    </tr>
    <tr><td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" id="saveFest" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="saveFest_Click" />
        </td></tr>
</table>