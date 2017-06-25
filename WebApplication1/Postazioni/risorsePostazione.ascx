<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="risorsePostazione.ascx.cs" Inherits="KIS.Postazioni.risorsePostazione" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script>
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>

    <div class="row-fluid" runat="server" id="rowBody">
    <div class="span12">
        <asp:Repeater runat="server" ID="rptTurni" OnItemDataBound="rptTurni_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-condensed">
                <thead>
                    <th><asp:literal runat="server" ID="lblReparto" Text="<%$Resources:lblReparto %>" /></th>
                    <th><asp:literal runat="server" ID="lblTurno" Text="<%$Resources:lblTurno %>" /></th>
                    <th><asp:literal runat="server" ID="lblNumRisorse" Text="<%$Resources:lblNumRisorse %>" /></th>
                </thead><tbody></HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:HiddenField runat="server" ID="hIDTurno" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                        <asp:Label runat="server" ID="lblNomeReparto" />
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "nome") %></td>
                    <td><asp:DropDownList runat="server" ID="ddlRisorse" TextMode="Number" Width="60" AutoPostBack="true" OnSelectedIndexChanged="ddlRisorse_SelectedIndexChanged" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></tbody></table></FooterTemplate>
        </asp:Repeater>
        <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
    </div>
</div>
    </ContentTemplate>
    </asp:UpdatePanel>