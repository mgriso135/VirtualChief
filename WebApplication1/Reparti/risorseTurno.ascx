<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="risorseTurno.ascx.cs" Inherits="KIS.Reparti.risorseTurno1" %>
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
<div class="row-fluid" runat="server" id="rowTitolo">
    <div class="span12">
        <h3><asp:Label runat="server" ID="lblTitolo" /></h3>
    </div>
    </div>
    <div class="row-fluid" runat="server" id="rowBody">
    <div class="span12">
        <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
        <asp:Repeater runat="server" ID="rptPostazioni" OnItemDataBound="rptPostazioni_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-condensed">
                <thead>
                    <th><asp:Literal runat="server" ID="lblTHPostazione" Text="<%$Resources:lblTHPostazione %>" /></th>
                    <th><asp:Literal runat="server" ID="lblTHNumRisorse" Text="<%$Resources:lblTHNumRisorse %>" /></th>
                </thead><tbody></HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1">
                    <td><asp:HiddenField runat="server" ID="hIDPostazione" Value='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "name") %></td>
                    <td><asp:DropDownList runat="server" ID="txtRisorse" TextMode="Number" Width="60" AutoPostBack="true" OnSelectedIndexChanged="txtRisorse_TextChanged" /></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></tbody></table></FooterTemplate>
        </asp:Repeater>
    </div>
</div>
    </ContentTemplate>
    </asp:UpdatePanel>