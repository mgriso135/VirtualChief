<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="manageReports.ascx.cs" Inherits="KIS.Admin.manageReports1" %>
<asp:Label runat="server" ID="lbl1" />
<div class="row-fluid" runat="server" id="tblOptions">
    <ul class="thumbnails unstyled">
        <li class="span2" runat="server" id="boxOrderStatusReport">
            <a href="~/Admin/configOrderStatusBase.aspx" runat="server" id="lnkConfigOrderStatus">
<asp:Image runat="server" ImageUrl="~/img/iconReportProductionProgress.jpg" id="imgConfigOrderStatus" ToolTip="<%$resources:lblCfgReportAvanzamentoProdotti.Text %>" height="100" CssClass="btn btn-primary"/>
<p><asp:label runat="server" ID="lblCfgReportAvanzamentoProdotti" meta:resourcekey="lblCfgReportAvanzamentoProdotti" /></p>
            </a>
            </li>
        </ul>
    </div>