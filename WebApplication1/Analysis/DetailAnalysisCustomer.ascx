<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailAnalysisCustomer.ascx.cs" Inherits="KIS.Analysis.DetailAnalysisCustomer1" %>

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
                <td><asp:label runat="server" id="lblDataInizio" meta:resourcekey="lblDataInizio" /></td>
                <td><asp:textbox runat="server" id="txtDateStart" Width="100px"  /></td>
            </tr>
             <tr>
                <td><asp:label runat="server" id="lblDataFine" meta:resourcekey="lblDataFine" /></td>
                <td><asp:textbox runat="server" id="txtDateEnd" Width="100px"  /></td>
            </tr>
            <tr>
             <td colspan="2">
            <asp:ImageButton ID="imgSearch" runat="server" Height="40" ImageUrl="~/img/iconView.png" OnClick="imgSearch_Click" />
                 <asp:label runat="server" id="lblRicerca" meta:resourcekey="lblRicerca" />
        </td></tr>
        </table>
    </div>
</div>



<asp:Label runat="server" ID="lblMonths" />

<div class="row-fluid">
    <div class="span12">
<asp:LinkButton runat="server" ID="lnkDays" OnClick="lnkDays_Click" Text="<%$resources:Giorni %>">Giorni</asp:LinkButton>&nbsp;|&nbsp;
<asp:LinkButton runat="server" ID="lnkMonths" OnClick="lnkMonths_Click" Text="<%$resources:Mesi %>"></asp:LinkButton>
        </div>
    </div>
<div class="row-fluid">
    <div class="span12">
        <asp:Chart runat="server" ID="Chart1" OnLoad="Chart1_Load">
            <Titles> 
      <asp:Title Text="<%$resources:lblOreLavoro %>"></asp:Title> 
   </Titles> 
            <Series>
                <asp:Series Name="interv" ChartType="Line" ChartArea="ChartArea1" /></Series>
            <chartareas> 
      <asp:ChartArea Name="ChartArea1"> 
      </asp:ChartArea> 
   </chartareas> 
        </asp:Chart>
        </div>
    </div>

<asp:Repeater runat="server" ID="rptMonths">
    <HeaderTemplate>
        <table style="text-align:center;">
        <tr>
            <th><asp:label runat="server" id="lblData" meta:resourcekey="lblData" /></th>
            <th><asp:label runat="server" id="lblTempoLav" meta:resourcekey="lblTempoLav" /></th>
        </tr></HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# ((DateTime)DataBinder.Eval(Container.DataItem, "Data")).ToString("dd/MM/yyyy") %></td>
        <td><%# Math.Round((Double)DataBinder.Eval(Container.DataItem, "TempoDiLavoro"),2) %></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
</asp:Repeater>

<asp:Repeater runat="server" ID="rptIntervalliDiLavoro" Visible="false">
    <HeaderTemplate><table>
        <tr>
            <th><asp:label runat="server" id="lblTHInizio" meta:resourcekey="lblTHInizio" /></th>
            <th><asp:label runat="server" id="lblTHFine" meta:resourcekey="lblTHFine" /><th>
            <th><asp:label runat="server" id="lblTHDurataInt" meta:resourcekey="lblTHDurataInt" /></th>
            <th><asp:label runat="server" id="lblTHDurataIntMin" meta:resourcekey="lblTHDurataIntMin" /></th>
            <th><asp:label runat="server" id="lblTHTaskProd" meta:resourcekey="lblTHTaskProd" /></th>
        </tr></HeaderTemplate>
    <ItemTemplate>
        <tr>
        <td><%#DataBinder.Eval(Container.DataItem, "DataInizio") %></td>
        <td><%#DataBinder.Eval(Container.DataItem, "DataFine") %></td>
        <td><%#DataBinder.Eval(Container.DataItem, "DurataIntervallo") %>

            </td><td><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "DurataIntervallo")).TotalMinutes, 2) %>
        </td>
        <td><%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
</asp:Repeater>