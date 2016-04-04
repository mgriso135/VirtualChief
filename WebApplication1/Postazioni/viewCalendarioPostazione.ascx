<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="viewCalendarioPostazione.ascx.cs" Inherits="KIS.Postazioni.viewCalendarioPostazione" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtCalInizio]").datepicker({ dateFormat: 'dd/mm/yy' })
        });
    });

    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtCalFine]").datepicker({ dateFormat: 'dd/mm/yy' })
        });
    });

    </script> 
<table runat="server" id="frmCalInizioFine">
    <tr>
        <td>Data inizio calendario:&nbsp;
            <asp:TextBox runat="server" ID="txtCalInizio" Width="80" />
        </td>
        <td>
            Data fine calendario:&nbsp;
            <asp:TextBox runat="server" ID="txtCalFine" Width="80" />
        </td>
        <td>
            <asp:ImageButton runat="server" ID="btnUpdateCalendar" Width="40" ImageUrl="~/img/iconSave.jpg" OnClick="btnUpdateCalendar_Click" />
        </td>
            </tr>
    <tr>
        <td><asp:Label runat="server" ID="dataI" /></td>
        <td><asp:Label runat="server" ID="dataF" /></td>
    </tr>
    </table>

<br />

<asp:Chart runat="server" ID="wlPostazione" Width="1000px" Height="100px">
    
</asp:Chart>
<asp:Label runat="server" ID="lbl1" />
        </ContentTemplate>
    </asp:UpdatePanel>