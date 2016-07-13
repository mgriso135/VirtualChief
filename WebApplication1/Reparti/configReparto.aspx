<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="configReparto.aspx.cs" Title="Kaizen Indicator System"
 MasterPageFile="/Site.master" Inherits="KIS.configReparto.configReparto" %>

<%@ Register TagPrefix="reparti" TagName="list" Src="listReparti.ascx" %>
<%@ Register TagPrefix="reparti" TagName="add" Src="addReparto.ascx" %>
<%@ Register TagPrefix="reparti" TagName="processiVarianti" Src="configProcessiVarianti.ascx" %>
<%@ Register TagPrefix="config" TagName="cadenza" Src="configCadenza.ascx" %>
<%@ Register TagPrefix="config" TagName="turni" Src="configTurni.ascx" %>
<%@ Register TagPrefix="config" TagName="splitTasksTurni" Src="configSplitTasksTurni.ascx" %>
<%@ Register TagPrefix="config" TagName="anticipoTasks" Src="configAnticipoMinimo.ascx" %>
<%@ Register TagPrefix="reparti" TagName="Personale" Src="~/Reparti/listRepartoUtenti.ascx" %>
<%@ Register TagPrefix="reparti" TagName="eventoRitardo" Src="~/Eventi/RepartoRitardo.ascx" %>
<%@ Register TagPrefix="reparti" TagName="eventoWarning" Src="~/Eventi/RepartoWarning.ascx" %>
<%@ Register TagPrefix="reparti" TagName="ModoCalcoloTC" Src="~/Reparti/configModoCalcoloTC.ascx" %>
<%@ Register TagPrefix="config" TagName="AndonReparto" Src="~/Reparti/configAndonReparto.ascx" %>
<%@ Register TagPrefix="config" TagName="AvvioTasks" Src="~/Reparti/configAvvioTasks.ascx" %>
<%@ Register TagPrefix="Andon" TagName="MaxViewDays" Src="~/Andon/configMaxViewDaysAndonReparto.ascx" %>
<%@ Register TagPrefix="config" TagName="Kanban" Src="~/Reparti/configKanban.ascx" %>
<%@ Register TagPrefix="Andon" TagName="configViewFields" Src="~/Andon/AndonRepartoViewFields.ascx" %>
<%@ Register TagPrefix="Timezone" TagName="config" Src="~/Reparti/configRepartoTimezone.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="configReparto.aspx">Gestione reparti</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <asp:Label runat="server" ID="lbl1" />
    <h3 runat="server" id="lnkShowAddReparti">Aggiungi un reparto<asp:ImageButton CssClass="img-rounded" runat="server" ID="showAddReparti" ImageUrl="/img/iconAdd.jpg" Height="50px" OnClick="showAddReparti_Click" ToolTip="Aggiungi un reparto"/></h3>
    <br />
    <reparti:add runat="server" id="addReparti" />
    <br />
    <reparti:list runat="server" id="listReparti" />
    <br />
    <h1><asp:Label runat="server" ID="lblTitle" /></h1>
    <asp:Label runat="server" ID="lblDesc" />
    <br />
    <br />

    <h2 runat="server" id="titleConfig">Configurazione del reparto</h2>
    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Cadenza
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:cadenza runat="server" id="configCadenza" />
      </div>
    </div>
            </div>

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

                <div class="accordion-group" runat="server" visible="false">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          Operatori di reparto
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
        <reparti:Personale runat="server" ID="frmPersonale" />
      </div>
    </div>
            </div>

        <div class="accordion-group" runat="server" visible="false">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFour">
          Anticipo minimo
      </a>
    </div>
            <div id="collapseFour" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:anticipoTasks runat="server" id="configAnticipoMinimoTasks" />
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
      <div class="accordion-inner">
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

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseEleven">
          Gestione kanban
      </a>
    </div>
            <div id="collapseEleven" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:Kanban runat="server" id="frmConfigKanban" />         

      </div>
    </div>
    </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwelve">
          Fuso orario
      </a>
    </div>
            <div id="collapseTwelve" class="accordion-body collapse">
      <div class="accordion-inner">
        <timezone:config runat="server" id="frmConfigTimezone" />

      </div>
    </div>
    </div>

    </div>
    </asp:Content>