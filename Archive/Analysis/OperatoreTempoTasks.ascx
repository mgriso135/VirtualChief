<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatoreTempoTasks.ascx.cs" Inherits="KIS.Analysis.OperatoreTempoTasks" %>
<script type="text/javascript">
    $(document).ready(function () { 
    $(function () {
        $("[id*=txtStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
        });

        if ($("#<%=hdPostback.ClientID%>").val() == "True") {
            var start = $("#<%=hdStart.ClientID%>").val();
            var end = $("#<%=hdEnd.ClientID%>").val();
            var user = $("#<%=hdUser.ClientID%>").val();
                $.ajax({
                        url: "../Analysis/OperatorsAnalysis/GetOperatorProductivity",
                        type: 'POST',
                        dataType: "html",
                data: {
                            user: user,
                            start: start,
                            end: end
                        },
                    success: function (result) {
                        $("#imgLoadProductivity").fadeOut();
                        $("#lblUserProductivity").html(result);
                },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
            });

             $.ajax({
                        url: "../Analysis/OperatorsAnalysis/GetOperatorOccupation",
                        type: 'POST',
                        dataType: "html",
                data: {
                            user: user,
                            start: start,
                            end: end
                        },
                    success: function (result) {
                        $("#lblUserOccupation").html(result);
                        $("#imgLoadOccupation").fadeOut();
                },
                    error: function (result) {
                        alert("Error");
                    },
                    warning: function (result) {
                        alert("Warning");
                    }
                });

        }
        else {
            $("#imgLoadOccupation").fadeOut();
            $("#imgLoadProductivity").fadeOut();
        }

    });
    </script> 

<asp:Label runat="server" ID="lbl1" />
<asp:HiddenField runat="server" ID="hdPostback" />
<asp:HiddenField runat="server" ID="hdUser" />
<asp:HiddenField runat="server" ID="hdStart" />
<asp:HiddenField runat="server" ID="hdEnd" />

<div class="row-fluid" runat="server" id="boxMain">
        <div class="span12">
            <center>
                <asp:Label runat="server" ID="lblDateAnalisi" meta:resourcekey="lblDateAnalisi" />
            </center>
            <div class="row-fluid">
        <div class="span6">
            <center>
                <asp:Label runat="server" ID="lblDataInizio" meta:resourcekey="lblDataInizio" />
    <asp:textbox runat="server" id="txtStart" Width="100px"  /><br />
            HH:<asp:DropDownList runat="server" ID="hhStart" Width="60" />
            mm:<asp:DropDownList runat="server" ID="mmStart" Width="60" />
            ss:<asp:DropDownList runat="server" ID="ssStart" Width="60" />
                </center>
            </div>
        <div class="span6">
            <center>
                <asp:Label runat="server" ID="lblDataFine" meta:resourcekey="lblDataFine" />
    <asp:textbox runat="server" id="txtEnd" Width="100px"  /><br />
            HH:<asp:DropDownList runat="server" ID="hhEnd" Width="60" />
            mm:<asp:DropDownList runat="server" ID="mmEnd" Width="60" />
            ss:<asp:DropDownList runat="server" ID="ssEnd" Width="60" />
                </center>
            </div>
                </div>
            <div class="row-fluid">
                <div class="span12">
                    <center>
                        <asp:ImageButton runat="server" ID="imgCheckPeriod" ImageUrl="~/img/iconManufacturing2.png" Height="60" OnClick="imgCheckPeriod_Click" />
                        <asp:ImageButton runat="server" ID="imgUndoPeriod" ImageUrl="~/img/iconCancel.jpg" Height="40" OnClick="imgUndoPeriod_Click" />
                    </center>
                </div>
            </div>
            </div>
        </div>

    <div class="row-fluid">
        <div class="span6">
        

<asp:Label runat="server" ID="lblTempoTotaleAttivo" meta:resourcekey="lblTempoTotaleAttivo" />&nbsp;
<asp:Label runat="server" ID="lblTotaleTempo" />

            <div class="row-fluid">
                <div class="span12">
            <div id="lblUserProductivity" />
                    <img src="../img/iconLoading3.gif" style="min-width: 15px; max-width:20px;" id="imgLoadProductivity" />
                    </div></div>
            <div class="row-fluid">
                <div class="span12">
            <div id="lblUserOccupation" />
                    <img src="../img/iconLoading3.gif" style="min-width: 15px; max-width:20px;" id="imgLoadOccupation" />
                    </div></div>
            <div class="accordion" id="accordion1" runat="server">

                <!-- Tasks -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Label runat="server" ID="lblIntervalliLavorati" meta:resourcekey="lblIntervalliLavorati" />
    </a></div>
                <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
<asp:Repeater runat="server" ID="rptDetailTasks">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th><asp:Label runat="server" ID="lblTHTaskID" meta:resourcekey="lblTHTaskID" /></th>
                <th><asp:Label runat="server" ID="lblTHTaskDescrizione" meta:resourcekey="lblTHTaskDescrizione" /></th>
                <th><asp:Label runat="server" ID="lblTHTaskInizio" meta:resourcekey="lblTHTaskInizio" /></th>
                <th><asp:Label runat="server" ID="LablblTHTaskFineel2" meta:resourcekey="lblTHTaskFine" /></th>
                <th><asp:Label runat="server" ID="lblTHTaskDurataIntervallo" meta:resourcekey="lblTHTaskDurataIntervallo" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "TaskProduzioneID") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NomeTask") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "DataInizio") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "DataFine") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "DurataIntervallo") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
          </div>
                    </div>
                    </div>
                <!-- Fine tasks -->

                <!-- Postazioni -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Label runat="server" ID="lblDettagliPostazione" meta:resourcekey="lblDettagliPostazione" />
    </a></div>
                <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptDetailsPostazione">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th><asp:Label runat="server" ID="lblTHPostazione" meta:resourcekey="lblTHPostazione" /></th>
                <th><asp:Label runat="server" ID="lblTHOreLav" meta:resourcekey="lblTHOreLav" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "NomePostazione") %></td>
            <td><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours, 2) + " ore"%></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

          </div>
                    </div>
                    </div>
                <!-- Fine Postazioni -->

                <!-- Per tipo di task -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          <asp:Label runat="server" ID="lblTHDettagliAttivita" meta:resourcekey="lblTHDettagliAttivita" />
    </a></div>
                <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptTipoTasks">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th><asp:Label runat="server" ID="lblTHTask" meta:resourcekey="lblTHTask" /></th>
                <th><asp:Label runat="server" ID="lblTHOreLavorate" meta:resourcekey="lblTHOreLavorate" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "NomeTask") %></td>
            <td><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours, 2) + " " %>
                <asp:Label runat="server" ID="lblOre" meta:resourcekey="lblOre" />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
          </div>
                    </div>
                    </div>
                <!-- Fine tipo di task -->
                <!-- Per giorni di presenza operatore -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFour">
          <asp:Label runat="server" ID="lblGiorniPresenza" meta:resourcekey="lblGiorniPresenza" />
    </a></div>
                <div id="collapseFour" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptGiorniPresenza">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th><asp:Label runat="server" ID="lblTHGiorniPresenzaAttiva" meta:resourcekey="lblTHGiorniPresenzaAttiva" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "Giorno")
                + "/" + DataBinder.Eval(Container.DataItem, "Mese")
                + "/" + DataBinder.Eval(Container.DataItem, "Anno")
                %>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
          </div>
                    </div>
                    </div>
                <!-- Fine giorni di presenza operatore -->
                <!-- Per prodotto -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFive">
          <asp:Label runat="server" ID="lblTHDettagliProd" meta:resourcekey="lblTHDettagliProd" />
    </a></div>
                <div id="collapseFive" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptProdotto">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th><asp:Label runat="server" ID="lblTHCliente" meta:resourcekey="lblTHCliente" /></th>
                <th><asp:Label runat="server" ID="lblTHProdotto" meta:resourcekey="lblTHProdotto" /></th>
                <th><asp:Label runat="server" ID="lblTHNomeProdotto" meta:resourcekey="lblTHNomeProdotto" /></th>
                <th><asp:Label runat="server" ID="lblTHOreLavorate2" meta:resourcekey="lblTHOreLavorate" /></th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "RagioneSocialeCliente") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "IDProdotto") %>/<%#DataBinder.Eval(Container.DataItem, "AnnoProdotto") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "NomeProdotto") %></td>
            <td><%# Math.Round(((TimeSpan)DataBinder.Eval(Container.DataItem, "Tempo")).TotalHours, 2) + " ore"%></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
          </div>
                    </div>
                    </div>
                <!-- Fine prodotto -->
                </div>
            </div>
        </div>