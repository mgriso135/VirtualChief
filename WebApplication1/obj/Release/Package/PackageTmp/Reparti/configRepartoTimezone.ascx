﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configRepartoTimezone.ascx.cs" Inherits="KIS.Reparti.configRepartoTimezone" %>

<asp:UpdatePanel runat="server" ID="upd1">
<ContentTemplate>

    <asp:Label runat="server" ID="lbl1"></asp:Label>
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

    function endRequestHandler(sender, args) {
        init();
    }

    function init() {
    }

    $(function () { // DOM ready
        init();
    });
    </script> 
        
        <asp:DropDownList runat="server" ID="ddlTimezones" OnSelectedIndexChanged="ddlTimezones_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>
        <br />
<asp:Label runat="server" ID="lblInfo" CssClass="text-info" />
</ContentTemplate>
</asp:UpdatePanel>