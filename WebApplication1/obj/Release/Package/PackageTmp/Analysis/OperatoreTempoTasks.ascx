<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatoreTempoTasks.ascx.cs" Inherits="KIS.Analysis.OperatoreTempoTasks" %>
<script type="text/javascript">
    $(function () {
        $("[id*=txtStart]").datepicker({ dateFormat: 'dd/mm/yy' })
    });

    $(function () {
        $("[id*=txtEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
    });
    </script> 

<asp:Label runat="server" ID="lbl1" />

<div class="row-fluid" runat="server" id="boxMain">
        <div class="span12">
            <center>
            Date di analisi</center>
            <div class="row-fluid">
        <div class="span6">
            <center>
    Data inizio:<asp:textbox runat="server" id="txtStart" Width="100px"  /><br />
            HH:<asp:DropDownList runat="server" ID="hhStart" Width="60" />
            mm:<asp:DropDownList runat="server" ID="mmStart" Width="60" />
            ss:<asp:DropDownList runat="server" ID="ssStart" Width="60" />
                </center>
            </div>
        <div class="span6">
            <center>
    Data fine:<asp:textbox runat="server" id="txtEnd" Width="100px"  /><br />
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
        

Tempo totale attivo: <asp:Label runat="server" ID="lblTotaleTempo" />

            <div class="accordion" id="accordion1" runat="server">

                <!-- Tasks -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
    Intervalli di tempo lavorati</a></div>
                <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
<asp:Repeater runat="server" ID="rptDetailTasks">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th>TaskID</th>
                <th>Descrizione</th>
                <th>Inizio</th>
                <th>Fine</th>
                <th>Durata Intervallo</th>
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
    Dettagli per postazione</a></div>
                <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptDetailsPostazione">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th>Postazione</th>
                <th>Ore lavorate</th>
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
    Dettagli tipologia di attività</a></div>
                <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptTipoTasks">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th>Task</th>
                <th>Ore lavorate</th>
            </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%#DataBinder.Eval(Container.DataItem, "NomeTask") %></td>
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
                <!-- Fine tipo di task -->
                <!-- Per giorni di presenza operatore -->
                <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFour">
    Giorni presenza</a></div>
                <div id="collapseFour" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptGiorniPresenza">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th>Giorni presenza attiva</th>
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
    Dettagli prodotto</a></div>
                <div id="collapseFive" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:Repeater runat="server" ID="rptProdotto">
    <HeaderTemplate>
        <table class="table table-bordered table-hover table-striped table-condensed">
            <thead>
            <tr>
                <th>Cliente</th>
                <th>Prodotto</th>
                <th>Nome prodotto</th>
                <th>Ore lavorate</th>
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