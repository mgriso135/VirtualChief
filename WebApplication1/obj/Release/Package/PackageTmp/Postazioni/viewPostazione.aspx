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
						<a href="managePostazioniLavoro.aspx">
                            <asp:Literal runat="server" ID="lblNavGestPostazioni" Text="<%$Resources:lblNavGestPostazioni %>" />
                            </a>
						<span class="divider">/</span>
                        <a href='<%# Request.RawUrl %>'><asp:Literal runat="server" ID="lblNavCfgPostazione" Text="<%$Resources:lblNavCfgPostazione %>" /></a>
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
          <asp:Literal runat="server" ID="lblCalPostazione" Text="<%$Resources:lblCalPostazione %>" />
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <postazione:viewCalendario runat="server" id="viewCalendarioPostazione" />
      </div>
    </div>
            </div>
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          <asp:Literal runat="server" ID="lblRisorseTurno" Text="<%$Resources:lblRisorseTurno %>" />
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