<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="kisAdmin.aspx.cs" Inherits="KIS.Admin.kisAdmin" %>
<%@ Register TagPrefix="config" TagName="logo" Src="~/Admin/configLogo.ascx" %>
<%@ Register TagPrefix="config" TagName="tipoPERT" Src="~/Admin/configWizard_TipoPERT.ascx" %>

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
						<a href="kisAdmin.aspx">Configurazione sistema</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h1>Configurazione sistema</h1>
    <asp:Label runat="server" ID="lbl1" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Logo
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:logo runat="server" id="configLogo" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Interfaccia PERT su Wizard
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:tipoPERT runat="server" id="frmTipoPERT" />
      </div>
    </div>
            </div>

        </div>

</asp:Content>
