<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wlReparto.ascx.cs" Inherits="KIS.Produzione.wlReparto" %>

<table class="table table-condensed">
    <tr>
    <td>
        <div class="accordion" id="accordion1" runat="server">
            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Literal runat="server" ID="lblAccCalendario" Text="<%$Resources:lblAccCalendario %>" />
</a></div>
                <div id="collapseOne" class="accordion-body collapse in">
      <div class="accordion-inner">
    <asp:Calendar runat="server" ID="calDate" OnSelectionChanged="calDate_SelectionChanged" OnDayRender="calDate_PreRender" />
          </div></div>
                </div>
            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Literal runat="server" ID="lblAccPostazione" Text="<%$Resources:lblAccPostazioni %>" /></a></div>
                <div id="collapseTwo" class="accordion-body collapse">
    <div class="accordion-inner">
<asp:RadioButtonList CssClass="radio" runat="server" id="rbPostazioni" AutoPostBack="true" OnSelectedIndexChanged="rbPostazioni_SelectedIndexChanged">
    <asp:ListItem Selected="True" Value="0" Text="<%$Resources:lblVisTempiGlobal %>"></asp:ListItem>
    <asp:ListItem Value="1" Text="<%$Resources:lblVisTempiPostazione %>"></asp:ListItem>
</asp:RadioButtonList>
<asp:CheckBoxList runat="server" ID="chkLstPostazioni" OnSelectedIndexChanged="chkLstPostazioni_SelectedIndexChanged" AutoPostBack="true">

</asp:CheckBoxList>
         </div>  
                    </div> 
    </div>
            </div>



    </td>
    <td>
        
        <!-- Google Charts Column Chart: Workload per postazione -->
<div id="workloadChart" style="width: 800px; height: 400px;"></div>
<script type="text/javascript">
    // Google Charts: Workload per postazione
    google.charts.load('current', {'packages':['corechart']});
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Postazione');
        data.addColumn('number', 'Carico orario');
        // Data will be populated from server-side code via JSON
        // Example: data.addRow(['Postazione 1', 8.5]);
        
        var options = {
            title: 'Carico di lavoro per postazione',
            hAxis: {title: 'Postazione', minValue: 0},
            vAxis: {title: 'Ore'},
            seriesType: 'columns',
            series: {0: {type: 'column'}}
        };
        
        var chart = new google.visualization.ColumnChart(document.getElementById('workloadChart'));
        chart.draw(data, options);
    }
</script>
<div class="ui-widget">
    <div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
        <p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <span id="chartPlaceholder">Carico postazioni</span></p>
    </div>
</div>
        

    </td>
       </tr></table>



