<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailTaskProduct.ascx.cs" Inherits="KIS.Analysis.DetailTaskProduct1" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtProductDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtProductDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script> 
<asp:Label runat="server" ID="lbl1" />

<h3 runat="server" id="lblProdotto"></h3>
<asp:Label runat="server" ID="lblProdDataInizio1" meta:resourcekey="lblProdDataInizio1" />&nbsp;
<asp:textbox runat="server" id="txtProductDateStart" Width="100px"  />
&nbsp;<asp:Label runat="server" ID="lblProdDataFine1" meta:resourcekey="lblProdDataFine1" />&nbsp;
<asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
<asp:ImageButton runat="server" ID="btnAnalizza" ImageUrl="~/img/iconView.png" Height="20" OnClick="btnAnalizza_Click" />
<div class="accordion-group" runat="server" id="accordion1">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
          <asp:Label runat="server" ID="lblAccTempiLavGlobali" meta:resourcekey="lblAccTempiLavGlobali" />
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Label runat="server" ID="lblTempoLavMedio" meta:resourcekey="lblTempoLavMedio" />:&nbsp;
          <asp:Label runat="server" ID="lblMediaTempoDiLavoro" /><br />
<asp:Chart runat="server" ID="chartTempiLavoro" Width="600">
    <Titles><asp:Title Name="titoloChartTempi" Text="<%$resources:lblChartTempoLavUnitario %>" Font="Calibri, 20pt, style=Bold" /></Titles>
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
          <asp:Repeater runat="server" ID="rptTempiLavoro">
              <HeaderTemplate><table class="table table-condensed table-striped table-hover">
                  <thead>
                      <tr>
                      <th><asp:Label runat="server" ID="lblTHIDProd" meta:resourcekey="lblTHIDProd" /></th>
                          <th><asp:Label runat="server" ID="lblTHDataRealizzazione" meta:resourcekey="lblTHDataRealizzazione" /></th>
                          <th><asp:Label runat="server" ID="lblTHTempoLav" meta:resourcekey="lblTHTempoLav" /></th>
                          <th><asp:Label runat="server" ID="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:Label runat="server" ID="lblTHTempoLavUnitario" meta:resourcekey="lblTHTempoLavUnitario" /></th>
                          </tr>
                  </thead><tbody></HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td>
                      <%#DataBinder.Eval(Container.DataItem, "ArticoloID") %>/<%#DataBinder.Eval(Container.DataItem, "ArticoloAnno") %></td>
                      <td><%#((DateTime)DataBinder.Eval(Container.DataItem, "DataInizioTask")).ToString("dd/MM/yyyy") %></td>
                        <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroEffettivo")).TotalHours.ToString("F2") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %></td>
                      <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoroEffettivoUnitario")).TotalHours.ToString("F2") %></td>
                  </tr>

              </ItemTemplate>
              <FooterTemplate></tbody></table></FooterTemplate>
          </asp:Repeater>
          </div>
                </div>


    <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">
          <asp:Label runat="server" ID="lblAccTempiCicloGlobali" meta:resourcekey="lblAccTempiCicloGlobali" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">          
          <asp:Label runat="server" ID="lblMedia" meta:resourcekey="lblMedia" />:&nbsp;
          <asp:Label runat="server" ID="lblMediaTempiCiclo" /><br />
            <asp:Chart runat="server" ID="chartTempiCiclo" Width="600">
                <Titles>
                    <asp:Title Name="titoloChartTempiCiclo" Text="<%$resources:lblChartTempoCicloUnitario %>" Font="Calibri, 20pt, style=Bold" />
                </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Series>
                    <asp:Series Name="TempiCiclo" ChartType="Line">

                    </asp:Series>
                </Series>
            </asp:Chart>
          <br />
          <asp:Repeater runat="server" ID="rptTempiCiclo">
              <HeaderTemplate><table class="table table-condensed table-striped table-hover">
                  <thead>
                      <tr>
                          <th><asp:Label runat="server" ID="lblTHIDProd2" meta:resourcekey="lblTHIDProd" /></th>
                          <th><asp:Label runat="server" ID="lblTHDataRealizzazione2" meta:resourcekey="lblTHDataRealizzazione" /></th>
                          <th><asp:Label runat="server" ID="lblTHTempoCiclo" meta:resourcekey="lblTHTempoCiclo" /></th>
                          <th><asp:Label runat="server" ID="lblTHQuantita2" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:Label runat="server" ID="lblTHTempoCicloUnitario" meta:resourcekey="lblTHTempoCicloUnitario" /></th>
                          </tr>
                  </thead><tbody></HeaderTemplate>
              <ItemTemplate>
                  <tr>
                      <td>
                      <%#DataBinder.Eval(Container.DataItem, "ArticoloID") %>/<%#DataBinder.Eval(Container.DataItem, "ArticoloAnno") %></td>
                      <td><%#((DateTime)DataBinder.Eval(Container.DataItem, "DataInizioTask")).ToString("dd/MM/yyyy") %></td>
                        <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoCicloEffettivo")).TotalHours.ToString("F2") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "QuantitaProdotta") %></td>
                      <td><%#((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoCicloEffettivoUnitario")).TotalHours.ToString("F2") %></td>
                  </tr>
              </ItemTemplate>
              <FooterTemplate></tbody></table></FooterTemplate>
          </asp:Repeater>
          </div>
                </div>
    </div>