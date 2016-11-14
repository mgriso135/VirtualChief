<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewPostazione.aspx.cs"
    MasterPageFile="~/Site.master" Title="Kaizen Indicator System" Inherits="KIS.Postazioni.viewPostazione" %>
<%@ Register TagPrefix="postazione" TagName="viewCalendario" Src="viewCalendarioPostazione.ascx" %>
<%@ Register TagPrefix="postazione" TagName="workload" Src="~/Reparti/postazioneWorkLoad.ascx" %>
<%@ Register TagPrefix="postazione" TagName="risorse" Src="~/Postazioni/risorsePostazione.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    		<ul class="breadcrumb hidden-phone">
					<li>
						<a href="managePostazioniLavoro.aspx">Gestione postazioni di lavoro</a>
						<span class="divider">/</span>
                        <a href='<%# Request.RawUrl %>'>Configurazione postazione</a>
                        <span class="divider">/</span>
					</li>
				</ul>
<h3><asp:Label runat="server" ID="lblNomePost" /></h3>
    <asp:Label runat="server" ID="lblDescPost" />
    <br />
    <asp:Label runat="server" ID="lbl1" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Calendario postazione
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <postazione:viewCalendario runat="server" id="viewCalendarioPostazione" />
      </div>
    </div>
            </div>

    <!--    <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Carico di lavoro per processo
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
            <postazione:workload runat="server" ID="frmPstWorkload" />
      </div>
    </div>
            </div>-->

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          Risorse produttive per turno
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
            <postazione:risorse runat="server" ID="frmPstRisorse" />
      </div>
    </div>
            </div>

        </div>
</asp:Content>