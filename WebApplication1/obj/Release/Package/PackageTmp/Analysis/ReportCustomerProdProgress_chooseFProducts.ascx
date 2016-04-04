<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseFProducts.ascx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseFProducts1" %>
<asp:updatepanel runat="server" ID="upd1">
    <ContentTemplate>
<script>
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            $("#<%=lbl1.ClientID%>").delay(1000).fadeOut("slow", function () {
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
        $("#<%=txtProductDateStart.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#<%=txtProductDateEnd.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
}

$(function () { // DOM ready
    init();
});
    </script>

<div class="row-fluid">
            <div class="span12"><h3>Seleziona prodotti finiti da inserire nel report</h3>
                </div>
            </div>

        <div class="row-fluid" runat="server" id="frmReport">
            <div class="span2">
                <asp:HyperLink runat="server" ID="lnkGoBack">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
        </asp:HyperLink>
            </div>
            <div class="span8">
                        <asp:Label runat="server" ID="lbl1" CssClass="text-info" /><br />
                Filtra prodotti con data prevista produzione compresa tra <asp:textbox runat="server" id="txtProductDateStart" Width="100px"  /> e <asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
                <asp:ImageButton runat="server" ID="btnSearch" ImageUrl="~/img/iconView.png" Width="40" OnClick="btnSearch_Click" />
            
            <asp:Repeater runat="server" ID="rptProductsF" OnItemDataBound="rptProductsF_ItemDataBound">
                <HeaderTemplate><table class="table table-condensed table-striped"></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                      <td><asp:CheckBox runat="server" ID="chk" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" Checked="false" />
                          <asp:HiddenField runat="server" ID="hArtID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                          <asp:HiddenField runat="server" ID="hArtYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                      </td>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
                  </tr>
                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
            <asp:Label runat="server" ID="lblList" />
              </div>
            <div class="span2"><asp:HyperLink runat="server" ID="lnkGoFwd">
            <asp:Image runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" />
        </asp:HyperLink></div>
              </div>
        </ContentTemplate></asp:updatepanel>