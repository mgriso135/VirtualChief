<%@ Page Title="" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigReparti_Detail.aspx.cs" Inherits="KIS.Configuration.wizConfigReparti_Detail" %>

<%@ Register TagPrefix="config" TagName="turni" Src="~/Reparti/configTurni.ascx" %>
<%@ Register TagPrefix="config" TagName="splitTasksTurni" Src="~/Reparti/configSplitTasksTurni.ascx" %>
<%@ Register TagPrefix="reparti" TagName="ModoCalcoloTC" Src="~/Reparti/configModoCalcoloTC.ascx" %>
<%@ Register TagPrefix="config" TagName="AndonReparto" Src="~/Reparti/configAndonReparto.ascx" %>
<%@ Register TagPrefix="config" TagName="AvvioTasks" Src="~/Reparti/configAvvioTasks.ascx" %>
<%@ Register TagPrefix="Andon" TagName="MaxViewDays" Src="~/Andon/configMaxViewDaysAndonReparto.ascx" %>
<%@ Register TagPrefix="Andon" TagName="configViewFields" Src="~/Andon/AndonRepartoViewFields.ascx" %>
<%@ Register TagPrefix="reparti" TagName="eventoRitardo" Src="~/Eventi/RepartoRitardo.ascx" %>
<%@ Register TagPrefix="reparti" TagName="eventoWarning" Src="~/Eventi/RepartoWarning.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager runat="server" ID="scriptMan1" />
 
    <asp:Label runat="server" ID="lbl1" />
    <br />
    <h1><asp:Label runat="server" ID="lblTitle" /></h1>
    <asp:Label runat="server" ID="lblDesc" />
    <br />
    <br />

    <h2 runat="server" id="titleConfig">Configurazione del reparto</h2>

    <div class="accordion" id="accordion1" runat="server">
        
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Inserimento tasks in produzione
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:splitTasksTurni runat="server" id="configSplitTasksTurni" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFive">
          Turni di lavoro
      </a>
    </div>
            <div id="collapseFive" class="accordion-body collapse">
      <div class="accordion-inner">sdsd
        <config:turni runat="server" id="configTurni" />
      </div>
    </div>
            </div>
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseSix">
          Allarme ritardi
      </a>
    </div>
            <div id="collapseSix" class="accordion-body collapse">
      <div class="accordion-inner">
        <reparti:eventoRitardo runat="server" id="frmEvRitardo" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseSeven">
          Allarme warning
      </a>
    </div>
            <div id="collapseSeven" class="accordion-body collapse">
      <div class="accordion-inner">
        <reparti:eventoWarning runat="server" id="frmEvWarning" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseEight">
          Modalità calcolo tempo ciclo
      </a>
    </div>
            <div id="collapseEight" class="accordion-body collapse">
                <div class="accordion-inner">
                    <reparti:ModoCalcoloTC runat="server" ID="frmModoCalcoloTC" />
                </div>
            </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseNine">
          Andon
      </a>
    </div>
            <div id="collapseNine" class="accordion-body collapse">
      <div class="accordion-inner">
        
          <config:AndonReparto runat="server" ID="frmConfigAndonReparto" />
          <Andon:MaxViewDays runat="server" ID="frmAndonMaxDays" />
          <andon:configViewFields runat="server" id="frmAndonViewFields" />

      </div>
    </div>
            </div>

    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTen">
          Avvio task
      </a>
    </div>
            <div id="collapseTen" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:avvioTasks runat="server" id="frmAvvioTask" />
          

      </div>
    </div>
    </div>


    </div>
</asp:Content>
