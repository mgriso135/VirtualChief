<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizConfigTimezone.ascx.cs" Inherits="KIS.Configuration.WizConfigTimezone1" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow", function () {
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
        <h3><asp:Literal runat="server" ID="lblSelTimezone" Text="<%$Resources:lblSelTimezone %>" /></h3>
        <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlTimezones">
        </asp:DropDownList>
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="40" OnClick="btnSave_Click" />
        <br />
<asp:Label runat="server" ID="lbl1" CssClass="text-info" />
        </ContentTemplate>
    </asp:UpdatePanel>