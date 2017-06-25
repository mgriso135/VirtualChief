<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailAnalysisTipoProdotto.ascx.cs" Inherits="KIS.Analysis.DetailAnalysisTipoProdotto1" %>

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
        $("#<%=txtStart.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        $("#<%=txtEnd.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
    }

    $(function () { // DOM ready
        init();
    });
    </script> 

        <script type="text/javascript">
            function FireSearch() {
                $("#<%=lbl1.ClientID%>").fadeIn("fast", function() {$(this).text('Loading data...')});
                return true;
            }
</script>

<div class="row-fluid">
    <div class="span12">
<h3><asp:Label runat="server" ID="lblNomeTipoProdotto" /></h3>
<asp:Label runat="server" ID="lblDescTipoProdotto" /><br />

        </div>
    </div>

<div class="row-fluid"><div class="span12"><asp:Label runat="server" ID="lbl1" CssClass="text-info" /></div></div>

<div class="row-fluid" runat="server" id="container">
    <div class="span12">
        <table>
            <tr><td><asp:Label runat="server" ID="lblDataInizio" meta:resourcekey="lblDataInizio" /></td>
                <td><asp:TextBox runat="server" ID="txtStart" /></td>
            </tr>
            <tr><td><asp:Label runat="server" ID="lblDataFine" meta:resourcekey="lblDataFine" /></td>
                <td><asp:TextBox runat="server" ID="txtEnd" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ImageButton runat="server" ID="btnSearch" OnClientClick="return FireSearch();" OnClick="btnSearch_Click" Width="40" ImageUrl="~/img/iconView.png" />
                </td>
            </tr>
        </table>

<div class="accordion" id="accordion1" runat="server">
<asp:Repeater runat="server" ID="rptProdotti">
    <HeaderTemplate>
            <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
          <asp:Label runat="server" ID="lblProducts" meta:resourcekey="lblProducts" />
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        
    </HeaderTemplate>
    <ItemTemplate>
        <%# DataBinder.Eval(Container.DataItem, "ID") %>/<%# DataBinder.Eval(Container.DataItem, "Year") %> - <%# DataBinder.Eval(Container.DataItem, "Cliente") %>
    </ItemTemplate>
    <SeparatorTemplate>
        <br />
    </SeparatorTemplate>
    <FooterTemplate>
        </div>
    </div>
            </div>
        
    </FooterTemplate>
</asp:Repeater>

    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">
          <asp:Label runat="server" ID="lblTempiDiLavoro" meta:resourcekey="lblTempiDiLavoro" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Label runat="server" ID="lblMedia" meta:resourcekey="lblMedia" /><asp:Label runat="server" ID="lblMediaTempoDiLavoro" />
        <br />
            <asp:Chart runat="server" ID="chartTempi" Width="600">
                <Titles>
                    <asp:Title Name="titoloChartTempi" Text="<%$resources:lblChartTempoLavUnitario %>" Font="Calibri, 20pt, style=Bold" />
                </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Series>
                    <asp:Series Name="tempi" ChartType="Line">

                    </asp:Series>
                </Series>
            </asp:Chart>
          <br />
          <!--Dettaglio tempi di lavoro-->
          <asp:Repeater runat="server" ID="rptTempiDiLavoro">
              <HeaderTemplate>
                  <table class="table table-striped table-hover table-condensed">
                      <tr>
                          <th><asp:Label runat="server" ID="lblTHID" meta:resourcekey="lblTHID" /></th>
                          <th><asp:Label runat="server" ID="lblTHCliente" meta:resourcekey="lblTHCliente" /></th>
                          <th><asp:Label runat="server" ID="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:Label runat="server" ID="lblTempoLavTotale" meta:resourcekey="lblTempoLavTotale" /></th>
                          <th><asp:Label runat="server" ID="lblTempoLavUnitario" meta:resourcekey="lblTempoLavUnitario" /></th>
                          </tr>
              </HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "RagioneSocialeCliente") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %></td>
                      <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroTotale")).TotalHours.ToString("F2") %></td>
                      <td><%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroUnitario")).TotalHours.ToString("F2") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate></table></FooterTemplate>
          </asp:Repeater>
          </div>
                </div>
        </div>

    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseThree">
          <asp:Label runat="server" ID="lblLeadTimes" meta:resourcekey="lblLeadTimes" />
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          
           <asp:Label runat="server" ID="lblMedia1" meta:resourcekey="lblMedia" /><asp:Label runat="server" ID="lblMediaLeadTimes" /><br />
            <asp:Chart runat="server" ID="chartLeadTimes" Width="600">
                <Titles>
                    <asp:Title Name="titoloChartTempi" Text="<%$resources:lblLeadTimes %>" Font="Calibri, 20pt, style=Bold" />
                </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Series>
                    <asp:Series Name="leadTimes" ChartType="Line">

                    </asp:Series>
                </Series>
            </asp:Chart>
          <br />
          <asp:Repeater runat="server" ID="rptLeadTimes" OnItemDataBound="rptLeadTimes_ItemDataBound">
              <HeaderTemplate>
                  <table class="table table-striped table-hover table-condensed">
                      <tr>
                          <th><asp:Label runat="server" ID="lblTHID" meta:resourcekey="lblTHID" /></th>
                          <th><asp:Label runat="server" ID="lblTHCliente" meta:resourcekey="lblTHCliente" /></th>
                          <th><asp:Label runat="server" ID="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:Label runat="server" ID="lblTHLeadTime" meta:resourcekey="lblTHLeadTime" /></th>
                          </tr>
              </HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td><%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "RagioneSocialeCliente") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %></td>
                      <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "LeadTime")).TotalHours.ToString("F2") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate>
                  <tr>
                      <td colspan="3">
                          <b><asp:Label runat="server" ID="lblMedia2" meta:resourcekey="lblMedia" /></b>
                      </td>
                      <td>
                          <b><asp:Label runat="server" ID="lblLTMedio" /></b>
                      </td>
                  </tr></table></FooterTemplate>
          </asp:Repeater>

          </div>
                </div>
        </div>
    <!--TASKS-->
    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseFour">
          <asp:Label runat="server" ID="lblTempoLavTask" meta:resourcekey="lblTempoLavTask" />
      </a>
    </div>
            <div id="collapseFour" class="accordion-body collapse">
      <div class="accordion-inner">
          
          <asp:Repeater runat="server" ID="rptTempiLavoroTasks">
              <HeaderTemplate>
                  <table class="table table-striped table-hover table-condensed">
                      <tr>
                          <th></th>
                          <th><asp:Label runat="server" ID="lblTHTask" meta:resourcekey="lblTHTask" /></th>
                          <th><asp:Label runat="server" ID="lblTHQtaProd" meta:resourcekey="lblTHQtaProd" /></th>
                          <th><asp:Label runat="server" ID="lblTHTaskTempoMedioUnitario" meta:resourcekey="lblTHTaskTempoMedioUnitario" /></th>
                          </tr>
              </HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td><asp:HyperLink runat="server" ID="lnkTaskDetail" 
                          NavigateUrl='<%#"~/Analysis/DetailTaskProduct.aspx?taskID="
                          + DataBinder.Eval(Container.DataItem, "processID") 
                          +"&revTask="+DataBinder.Eval(Container.DataItem, "revisione") 
                          +"&prodID="+ idProc
                          +"&prodRev="+ rev
                          +"&varianteID=" + DataBinder.Eval(Container.DataItem, "varianteID") %>'>
                          <asp:Image runat="server" ID="imgTaskDetail" Width="20" ImageUrl="~/img/iconView.png" />
                          </asp:HyperLink></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "nomeTask") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "quantita") %></td>
                      <td><%# ((TimeSpan)DataBinder.Eval(Container.DataItem, "tempo")).TotalHours.ToString("F2") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate>
                  </table>

              </FooterTemplate>
          </asp:Repeater>

          </div>
                </div>
        </div>
    <!--END TASKS-->


</div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>