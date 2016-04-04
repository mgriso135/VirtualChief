<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_chooseINPProducts.ascx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_chooseINPProducts1" %>

       
<script>
            $(document).ready(function () {
                var prmI = Sys.WebForms.PageRequestManager.getInstance();
                var prmP = Sys.WebForms.PageRequestManager.getInstance();
                var prmN = Sys.WebForms.PageRequestManager.getInstance();
                prmI.add_endRequest(function () {
                    $("#<%=lblI.ClientID%>").delay(1000).fadeOut("slow", function () {
                        $(this).text('')
                    });
                });
                prmP.add_endRequest(function () {
                    $("#<%=lblP.ClientID%>").delay(1000).fadeOut("slow", function () {
                        $(this).text('&nbsp;a')
                    });
                });
                prmN.add_endRequest(function () {
                    $("#<%=lblN.ClientID%>").delay(1000).fadeOut("slow", function () {
                        $(this).text('&nbsp;a')
                    });
                });
            });
</script>
        <asp:Label runat="server" ID="lbl1" />
        <div class="row-fluid">
            <div class="span12"><h3>Seleziona prodotti da inserire nel report</h3>
                </div>
            </div>

        <div class="row-fluid" runat="server" id="frmReport">
            <div class="span2">
                <asp:HyperLink runat="server" ID="lnkGoBack" NavigateUrl="/Analysis/ReportCustomerProdProgress_chooseCustomer.aspx">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
        </asp:HyperLink>
            </div>
            <div class="span8">
                <div class="accordion" id="accordion1" runat="server">
        <!--Prodotti in corso di realizzazione-->
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Prodotti in fase di produzione
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          <!--Prodotti in corso di realizzazione-->
          <asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
          <asp:Repeater runat="server" ID="rptProdottiI" OnItemDataBound="rptProdottiI_ItemDataBound">
              <HeaderTemplate><table class="table table-condensed table-striped"></HeaderTemplate>
              <ItemTemplate>
                  <tr runat="server" id="tr1">
                      <td><asp:CheckBox runat="server" ID="chk" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" Checked="true" />
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
        <asp:Label runat="server" ID="lblI" CssClass="text-info" />
          </ContentTemplate>
    </asp:UpdatePanel>
          </div>
                </div>
            </div>
                    <!--Prodotti da iniziare-->
                    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Prodotti pianificati ma non avviati
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <!--Prodotti da iniziare-->
          <asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>
          <asp:Repeater runat="server" ID="rptProdottiP" OnItemDataBound="rptProdottiP_ItemDataBound">
              <HeaderTemplate><table class="table table-condensed table-striped"></HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td><asp:CheckBox runat="server" ID="chkP" AutoPostBack="true" OnCheckedChanged="chkP_CheckedChanged" Checked="true" />
                          <asp:HiddenField runat="server" ID="hArtID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                          <asp:HiddenField runat="server" ID="hArtYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                      </td>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate></table></FooterTemplate>
              </asp:Repeater>
        <asp:Label runat="server" ID="lblP" CssClass="text-info" />
          </ContentTemplate>
    </asp:UpdatePanel>
          </div>
                </div>
            </div>
                    <!--Prodotti da pianificare-->
                    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          Prodotti da pianificare
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          <!--Prodotti da pianificare-->
          <asp:UpdatePanel runat="server" ID="upd3">
    <ContentTemplate>
          <asp:Repeater runat="server" ID="rptProdottiN" OnItemDataBound="rptProdottiN_ItemDataBound">
              <HeaderTemplate><table class="table table-condensed table-striped"></HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td><asp:CheckBox runat="server" ID="chkN" AutoPostBack="true" OnCheckedChanged="chkN_CheckedChanged" Checked="true" />
                          <asp:HiddenField runat="server" ID="hArtID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                          <asp:HiddenField runat="server" ID="hArtYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
                      </td>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate></table></FooterTemplate>
              </asp:Repeater>
          <asp:Label runat="server" ID="lblN" CssClass="text-info" />
        </ContentTemplate>
    </asp:UpdatePanel>
          </div>

                </div>
            </div>
                    </div>
                

            </div>
            <div class="span2"><asp:HyperLink runat="server" ID="lnkGoFwd">
            <asp:Image runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" />
        </asp:HyperLink></div>
                
        </div>