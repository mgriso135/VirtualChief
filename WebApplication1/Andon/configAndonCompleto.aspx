﻿<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configAndonCompleto.aspx.cs" Inherits="KIS.Andon.configAndonCompleto" %>
<%@ Register TagPrefix="Andon" TagName="FormatoUsername" Src="~/Andon/configFormatoUsername.ascx" %>
<%@ Register TagPrefix="Andon" TagName="ViewFields" Src="~/Andon/AndonCompletoViewFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
        <li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
					</li>
					<li>
						<a href="configAndonCompleto.aspx">Configurazione Andon Completo</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3>Configurazione Andon Completo</h3>
    <asp:Label runat="server" ID="lbl1" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Modalità di visualizzazione degli utenti nelle postazioni
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <andon:FormatoUsername runat="server" ID="frmFormatoUsername" />
      </div>
    </div>
            </div>
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Campi da visualizzare
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
        <andon:ViewFields runat="server" id="frmViewFields" />
      </div>
    </div>
            </div>
        </div>

</asp:Content>
