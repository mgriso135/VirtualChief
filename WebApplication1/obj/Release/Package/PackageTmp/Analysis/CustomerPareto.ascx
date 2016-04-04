<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerPareto.ascx.cs" Inherits="KIS.Analysis.CustomerPareto1" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script>
<div class="row-fluid">
    <div class="span12">
        <table class="table-condensed" runat="server" id="tblSelectDate">
            <tr>
                <td>Inizio periodo di riferimento:</td>
                <td><asp:textbox runat="server" id="txtDateStart" Width="100px"  /></td>
            </tr>
             <tr>
                <td>Fine periodo di riferimento:</td>
                <td><asp:textbox runat="server" id="txtDateEnd" Width="100px"  /></td>
            </tr>
            <tr>
             <td colspan="2">
            <asp:ImageButton ID="imgSearch" runat="server" Height="40" ImageUrl="~/img/iconView.png" OnClick="imgSearch_Click" />RICERCA!
        </td></tr>
        </table>
    </div>
</div>

<div class="row-fluid">
    <div class="span12">
        <asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptCustomers">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
            <th></th>
                <th>Cliente</th>
                <th>Tempo lavorato (hh:mm:ss)</th>
                <th>%</th>
                </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td></td>
            <td><%#DataBinder.Eval(Container.DataItem, "RagioneSociale") %></td>
            <td><%# Math.Truncate(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoro")).TotalHours) + ":"+
                ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoro")).Minutes + ":"+
                ((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoro")).Seconds %>
                (<%#DataBinder.Eval(Container.DataItem, "TempoDiLavoroDbl") %> ore)
            </td>
            <td><%#Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "TempoDiLavoro")).TotalHours / ((TimeSpan)portClienti.TempoDiLavoroTotale).TotalHours * 100, 2) %>%</td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        <tr>
            <td></td>
            <td>TOTALE:</td>
            <td><%=Math.Round(portClienti.TempoDiLavoroTotale.TotalHours, 2) %> ore</td>
            <td></td>
        </tr>
        </tbody>
        </table>
    </FooterTemplate>
            </asp:Repeater>
        </div>
   </div>

<div class="row-fluid">
    <div class="span12">
        <asp:Chart runat="server" ID="Chart1" OnLoad="Chart1_Load">
            <Titles> 
      <asp:Title Text="Pareto clienti"></asp:Title> 
   </Titles> 
            <Series>
                <asp:Series Name="customers" ChartType="Column" ChartArea="ChartArea1" /></Series>
            <chartareas> 
      <asp:ChartArea Name="ChartArea1"> 
      </asp:ChartArea> 
   </chartareas> 
        </asp:Chart>
        </div>
    </div>