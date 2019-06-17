<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailAnalysisTask.ascx.cs" Inherits="KIS.Analysis.DetailAnalysisTask1" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtProductDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtProductDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script> 
<h3><asp:label runat="server" id="lblTitleStorico" meta:resourcekey="lblTitleStorico" /></h3>
<asp:Label runat="server" ID="lbl1" />

<asp:Label runat="server" ID="taskName" /><br />
<asp:Label runat="server" ID="taskDescription" /><br />
<asp:Label runat="server" ID="taskRev" /><br />

<asp:label runat="server" id="lblConsideraProd1" meta:resourcekey="lblConsideraProd1" />&nbsp;<asp:textbox runat="server" id="txtProductDateStart" Width="100px"  />&nbsp;<asp:label runat="server" id="lblConsideraProd2" meta:resourcekey="lblConsideraProd2" />&nbsp;<asp:textbox runat="server" id="txtProductDateEnd" Width="100px"  />
<asp:ImageButton runat="server" ID="btnAnalizza" ImageUrl="~/img/iconView.png" Height="20" OnClick="btnAnalizza_Click" />

<div class="accordion" id="accordion1" runat="server">
<asp:Repeater runat="server" ID="rptProdotti">
    <HeaderTemplate>
            <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
          <asp:label runat="server" id="lblProdottiUtilizzo" meta:resourcekey="lblProdottiUtilizzo" />
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
          <asp:label runat="server" id="lblTempoGlobal" meta:resourcekey="lblTempoGlobal" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:label runat="server" id="lblMedia" meta:resourcekey="lblMedia" />:&nbsp;<asp:Label runat="server" ID="lblMediaTempoDiLavoro" />
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
                      <th><asp:label runat="server" id="lblTHIDProd" meta:resourcekey="lblTHIDProd" /></th>
                          <th><asp:label runat="server" id="lblTHDataRealizzazione" meta:resourcekey="lblTHDataRealizzazione" /></th>
                          <th><asp:label runat="server" id="lblTHTempoLav" meta:resourcekey="lblTHTempoLav" /></th>
                          <th><asp:label runat="server" id="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:label runat="server" id="lblTHTempoLavUnitario" meta:resourcekey="lblTHTempoLavUnitario" /></th>
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
          <asp:label runat="server" id="lblTHTempoCicloGlobal" meta:resourcekey="lblTHTempoCicloGlobal" />
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          
           <asp:label runat="server" id="lblMedia2" meta:resourcekey="lblMedia" />:&nbsp;<asp:Label runat="server" ID="lblMediaTempiCiclo" /><br />
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
                          <th><asp:label runat="server" id="lblTHIDProd2" meta:resourcekey="lblTHIDProd" /></th>
                          <th><asp:label runat="server" id="lblTHDataRealizzazione2" meta:resourcekey="lblTHDataRealizzazione" /></th>
                          <th><asp:label runat="server" id="lblTHTempoLav2" meta:resourcekey="lblTHTempoLav" /></th>
                          <th><asp:label runat="server" id="lblTHQuantita2" meta:resourcekey="lblTHQuantita" /></th>
                          <th><asp:label runat="server" id="lblTHTempoLavUnitario2" meta:resourcekey="lblTHTempoLavUnitario" /></th>
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