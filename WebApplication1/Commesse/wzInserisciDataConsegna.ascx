<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzInserisciDataConsegna.ascx.cs" Inherits="KIS.Commesse.wzInserisciDataConsegna1" %>

<asp:Label runat="server" ID="lbl1" />
<div class="row-fluid">
    <div class="span1">
        <asp:HyperLink runat="server" ID="lnkGoBack">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
        </asp:HyperLink>
    </div>
    <div class="span6">
<table runat="server" id="tbl">
    <tr>
        <td style="vertical-align:top;">
        Data consegna richiesta
        </td>
        <td>
            <asp:Label runat="server" ID="lblCalendar" />
            <asp:Calendar runat="server" ID="calDate" OnSelectionChanged="calDate_SelectionChanged" />
        </td>
    </tr>
</table>
        </div>
    <div class="span1">
        <asp:ImageButton OnClick="btnGoFwd_Click" runat="server" ID="btnGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" />
    </div>
    </div>