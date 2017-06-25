<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseCustomer.ascx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseCustomer1" %>
<h3><asp:label runat="server" id="lblTitleScegliCliente" meta:resourcekey="lblTitleScegliCliente" /></h3>
<asp:updatepanel runat="server" ID="upd1">
    <ContentTemplate>
        <script>
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>
<div class="row-fluid">
    <div class="span10">
        <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
        <asp:RadioButtonList runat="server" ID="rblClienti" CssClass="radio" AutoPostBack="true" OnSelectedIndexChanged="rblClienti_SelectedIndexChanged" />
    </div>
    <div class="span2">
        <asp:HyperLink runat="server" ID="lnkGoFwd">
            <asp:Image runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" Visible="false" />
        </asp:HyperLink>
    </div>
</div>
        </ContentTemplate>
    </asp:updatepanel>