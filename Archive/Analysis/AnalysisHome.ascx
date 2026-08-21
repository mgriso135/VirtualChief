<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalysisHome.ascx.cs" Inherits="KIS.Analysis.AnalysisHome" %>

<div class="row-fluid" runat="server" id="tblOptions">
    <ul class="thumbnails unstyled">
        <li class="span2" runat="server" id="boxProductionHistory">
            <a href="~/Analysis/ProductionHistory/Index" runat="server" id="lnkProductionHistory">
<asp:Image runat="server" ImageUrl="~/img/iconHistory.png" id="Image2" ToolTip="<%$resources:lblTTProductionHistory %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblProductionHistory" meta:resourcekey="lblProductionHistory" /></p>
            </a>
            </li>

        <li class="span2" runat="server" id="boxAnalisiOperatori">
            <a href="~/Analysis/ListAnalysisOperatori.aspx" runat="server" id="lnkAnalisiOperatori">
<asp:Image runat="server" ImageUrl="~/img/iconUser.png" id="imgAnalisiOperatori" ToolTip="<%$resources:lblTTAnalisiOp %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiOp" meta:resourcekey="lblAnalisiOp" /></p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxProductAnalysis">
            <a href="~/Analysis/ProductAnalysis/Index" runat="server" id="A2">
<asp:Image runat="server" ImageUrl="~/img/iconProduct.png" id="Image3" ToolTip="<%$resources:lblTTAnalisiTipoProd %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiTipoProd" meta:resourcekey="lblAnalisiTipoProd" /></p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxProductAnalysis2">
            <a href="~/Analysis/ListAnalysisTipoProdotto.aspx" runat="server" id="A3">
<asp:Image runat="server" ImageUrl="~/img/iconProduct.png" id="Image4" ToolTip="<%$resources:lblAnalisiTipoProdDetail %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiTipoProdDetail" meta:resourcekey="lblAnalisiTipoProdDetail" /></p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxAnalisiTasks">
            <a href="~/Analysis/TaskAnalysis/Index" runat="server" id="lnkAnalisiTasks">
<asp:Image runat="server" ImageUrl="~/img/iconTask.jpg" id="imgAnalisiTasks" ToolTip="<%$resources:lblTTAnalisiTask %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiTask" meta:resourcekey="lblAnalisiTask" /></p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxAnalisiTasks2">
            <a href="~/Analysis/ListAnalysisTasks.aspx" runat="server" id="lnkAnalisiTasks2">
<asp:Image runat="server" ImageUrl="~/img/iconTask.jpg" id="imgAnalisiTasks2" ToolTip="<%$resources:lblTTAnalisiTaskDetail %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiTask2" meta:resourcekey="lblTTAnalisiTaskDetail" /></p>
            </a>
            </li>
        
        <li class="span2" runat="server" id="boxAnalisiClienti">
            <a href="~/Analysis/CustomerPortfolio.aspx" runat="server" id="lnlCustomerAnalysis">
<asp:Image runat="server" ImageUrl="~/img/iconCustomer.jpg" id="imgCustomerAnalysis" ToolTip="<%$resources:lblTTAnalisiCliente %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblAnalisiCliente" meta:resourcekey="lblAnalisiCliente" /></p>
            </a>
            </li>
        <li class="span2" runat="server" id="boxReportAvanzamentoOrdini">
            <a href="~/Analysis/ReportCustomerProdProgress_chooseCustomer.aspx" runat="server" id="A1">
<asp:Image runat="server" ImageUrl="~/img/iconReportProductionProgress.jpg" id="Image1" ToolTip="<%$resources:lblTTReportCliente %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" id="lblReportCliente" meta:resourcekey="lblReportCliente" /></p>
            </a>
            </li>
        </ul>

</div>
