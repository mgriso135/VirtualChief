<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalysisHome.ascx.cs" Inherits="KIS.Analysis.AnalysisHome" %>

<div class="row-fluid" runat="server" id="tblOptions">
    <ul class="thumbnails unstyled">
        <li class="span2" runat="server" id="boxCostArticolo">
            <a href="~/Analysis/CostificazioneProdottiTerminatiRicerca.aspx" runat="server" id="lnkCostArticolo">
<asp:Image runat="server" ImageUrl="~/img/iconCostCommessa.jpg" id="imgCostArticolo" ToolTip="Costificazione singolo prodotto" height="100" CssClass="btn btn-primary"/>
<p>Costificazione singolo prodotto</p>
            </a>
            </li>

        <li class="span2" runat="server" id="boxCostCommessa">
            <a href="~/Analysis/ListCommesseChiuse.aspx" runat="server" id="lnkCostCommessa">
<asp:Image runat="server" ImageUrl="~/img/iconCostCommessa.jpg" id="imgCostCommessa" ToolTip="Costificazione commessa" height="100" CssClass="btn btn-primary"/>
<p>Costificazione commessa</p>
            </a>
            </li>

        <li class="span2" runat="server" id="boxAnalisiOperatori">
            <a href="~/Analysis/ListAnalysisOperatori.aspx" runat="server" id="lnkAnalisiOperatori">
<asp:Image runat="server" ImageUrl="~/img/iconUser.png" id="imgAnalisiOperatori" ToolTip="Gestione utenti" height="100" CssClass="btn btn-primary"/>
<p>Analisi tempi operatori</p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxAnalisiTipoProdotto">
            <a href="~/Analysis/ListAnalysisTipoProdotto.aspx" runat="server" id="lnkAnalisiTipoProdotto">
<asp:Image runat="server" ImageUrl="~/img/iconProduct.png" id="imgAnalisiTipoProdotto" ToolTip="Analisi dati per tipo di prodotto" height="100" CssClass="btn btn-primary"/>
<p>Analisi per tipo di prodotto</p>
            </a>
            </li>

        <li class="span2" runat="server" id="boxAnalisiTasks">
            <a href="~/Analysis/ListAnalysisTasks.aspx" runat="server" id="lnkAnalisiTasks">
<asp:Image runat="server" ImageUrl="~/img/iconTask.jpg" id="imgAnalisiTasks" ToolTip="Analisi dati per task" height="100" CssClass="btn btn-primary"/>
<p>Analisi tasks</p>
            </a>
            </li>

        <li class="span2" runat="server" id="boxAnalisiClienti">
            <a href="~/Analysis/CustomerPortfolio.aspx" runat="server" id="lnlCustomerAnalysis">
<asp:Image runat="server" ImageUrl="~/img/iconCustomer.jpg" id="imgCustomerAnalysis" ToolTip="Analisi dati per cliente" height="100" CssClass="btn btn-primary"/>
<p>Analisi portafoglio clienti</p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxReportAvanzamentoOrdini">
            <a href="~/Analysis/ReportCustomerProdProgress_chooseCustomer.aspx" runat="server" id="A1">
<asp:Image runat="server" ImageUrl="~/img/iconReportProductionProgress.jpg" id="Image1" ToolTip="Report avanzamento ordini per cliente" height="100" CssClass="btn btn-primary"/>
<p>Report avanzamento ordini per cliente</p>
            </a>
            </li>
        </ul>

</div>
