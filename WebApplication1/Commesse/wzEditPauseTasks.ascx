<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzEditPauseTasks.ascx.cs" Inherits="KIS.Commesse.wzEditPauseTasks1" %>
<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#<%=lblInfo.ClientID%>").delay(3000).fadeOut("slow", function () {
                $(this).text('')
            });
        });
    });

    window.onload = function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
    }

    </script> 
<h3>Gestione pause tra tasks</h3>
<asp:Label runat="server" id="lbl1" />

<table runat="server" id="tblPausa">
    <thead>
    <tr>
        <th>Task precedente</th>
        <th>Pausa&nbsp;<asp:Image runat="server" ID="imgWait" ImageUrl="~/img/iconWait.png" Width="40" /></th>
        <th>Task successivo</th>
    </tr>
        </thead>
    <tbody>
    <tr>
        <td style="text-align:center;"><asp:Label runat="server" ID="lblNomePrec" /></td>
        <td style="text-align:center;">
            <asp:DropDownList runat="server" ID="ddlHour" Width="70" />hh:
            <asp:DropDownList runat="server" ID="ddlMinutes" Width="60" />mm:
            <asp:DropDownList runat="server" ID="ddlSeconds" Width="60" />ss
        </td>
        <td style="text-align:center;"><asp:Label runat="server" ID="lblNomeSucc" /></td>
    </tr>
        <tr>
            <td colspan="3" style="text-align:center;">
                <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Width="40" OnClick="btnSave_Click" />
                <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Width="40" OnClick="btnUndo_Click" />
            </td>
        </tr>
        </tbody>
</table>
<asp:Label runat="server" id="lblInfo" CssClass="text-info" />