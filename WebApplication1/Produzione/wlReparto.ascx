<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wlReparto.ascx.cs" Inherits="KIS.Produzione.wlReparto" %>

<table class="table table-condensed">
    <tr>
    <td>
        <div class="accordion" id="accordion1" runat="server">
            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
    Calendario</a></div>
                <div id="collapseOne" class="accordion-body collapse in">
      <div class="accordion-inner">
    <asp:Calendar runat="server" ID="calDate" OnSelectionChanged="calDate_SelectionChanged" OnDayRender="calDate_PreRender" />
          </div></div>
                </div>
            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
    Postazioni di lavoro</a></div>
                <div id="collapseTwo" class="accordion-body collapse">
    <div class="accordion-inner">
<asp:RadioButtonList CssClass="radio" runat="server" id="rbPostazioni" AutoPostBack="true" OnSelectedIndexChanged="rbPostazioni_SelectedIndexChanged">
    <asp:ListItem Selected="True" Value="0">Visualizza i tempi complessivi per il reparto</asp:ListItem>
    <asp:ListItem Value="1">Suddividi i tempi per postazione</asp:ListItem>
</asp:RadioButtonList>
<asp:CheckBoxList runat="server" ID="chkLstPostazioni" OnSelectedIndexChanged="chkLstPostazioni_SelectedIndexChanged" AutoPostBack="true">

</asp:CheckBoxList>
         </div>  
                    </div> 
    </div>
            </div>



    </td>
    <td>
        
        <asp:chart ID="Chart1" runat="server" Width="800">
<ChartAreas>
    <asp:ChartArea Name="ChartArea1">
        <AxisY Minimum="0">
            
        </AxisY>
    </asp:ChartArea>
  </ChartAreas>
</asp:chart>
        <br />
        <div class="ui-widget">
	<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Label runat="server" ID="lbl1" /></p>
	</div>
</div>
        

    </td>
       </tr></table>



