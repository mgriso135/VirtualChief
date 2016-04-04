<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailAnalysisTask.ascx.cs" Inherits="KIS.Analysis.DetailAnalysisTask1" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtProductDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtProductDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script> 
<h3>Storico tempi del task (tutti i prodotti)</h3>
<asp:Label runat="server" ID="lbl1" />

<asp:Label runat="server" ID="taskName" /><br />
<asp:Label runat="server" ID="taskDescription" /><br />
<asp:Label runat="server" ID="taskRev" /><br />

Considera prodotti realizzati tra il giorno <asp:textbox runat="server" id="txtProductDateStart" Width="100px"  /> ed il giorno <asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
<asp:ImageButton runat="server" ID="btnAnalizza" ImageUrl="~/img/iconView.png" Height="20" OnClick="btnAnalizza_Click" />

<div class="accordion" id="accordion1" runat="server">
<asp:Repeater runat="server" ID="rptProdotti">
    <HeaderTemplate>
            <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
          Prodotti in cui viene usato il task
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HyperLink runat="server" ID="lnkDetailProcTask" NavigateUrl='<%#"DetailTaskProduct.aspx?taskID=" + processID.ToString() + "&revTask=" + revisione.ToString() + "&prodID=" + DataBinder.Eval(Container.DataItem, "process.processID") + "&prodRev=" + DataBinder.Eval(Container.DataItem, "process.revisione") + "&varianteID=" + DataBinder.Eval(Container.DataItem, "variant.idVariante") %>'>
        <asp:Image runat="server" ID="imgDetailProcTask" ImageUrl="~/img/iconView.png" Height="20" />
            </asp:HyperLink>
        <%#DataBinder.Eval(Container.DataItem, "process.processName") %>
        <%#DataBinder.Eval(Container.DataItem, "variant.nomeVariante") %>
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
          Tempi di lavoro globali
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          Media:&nbsp;<asp:Label runat="server" ID="lblMediaTempoDiLavoro" />
        <br />
            <asp:Chart runat="server" ID="chartTempiLavoro" Width="600">
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
                      <th>ID Prodotto</th>
                          <th>Data realizzazione task</th>
                          <th>Tempo di lavoro [ore]</th>
                          <th>Quantità</th>
                          <th>Tempo di lavoro unitario [ore]</th>
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
        </div>

    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseThree">
          Tempi ciclo globali
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          
           Media:&nbsp;<asp:Label runat="server" ID="lblMediaTempiCiclo" /><br />
            <asp:Chart runat="server" ID="chartTempiCiclo" Width="600">
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
                      <th>ID Prodotto</th>
                          <th>Data realizzazione task</th>
                          <th>Tempo ciclo [ore]</th>
                          <th>Quantità</th>
                          <th>Tempo ciclo unitario [ore]</th>
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


</div>