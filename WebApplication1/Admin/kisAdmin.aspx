<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="kisAdmin.aspx.cs" Inherits="KIS.Admin.kisAdmin" %>
<%@ Register TagPrefix="config" TagName="logo" Src="~/Admin/configLogo.ascx" %>
<%@ Register TagPrefix="config" TagName="tipoPERT" Src="~/Admin/configWizard_TipoPERT.ascx" %>
<%@ Register TagPrefix="config" TagName="TimeZone" Src="~/Admin/configTimezone.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
        <li>
						<a href="/Admin/admin.aspx">
                            <asp:Label runat="server" ID="lblNavAdmin" meta:resourcekey="lblNavAdmin" /></a>
						<span class="divider">/</span>
					</li>
					<li>
						<a href="kisAdmin.aspx">
                            <asp:Label runat="server" ID="lblNavSysCfg" meta:resourcekey="lblNavSysCfg" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h1><asp:Label runat="server" ID="lblTitleSysCfg" meta:resourcekey="lblTitleSysCfg" /></h1>
    <asp:Label runat="server" ID="lbl1" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Label runat="server" ID="lblLogo" meta:resourcekey="lblLogo" />
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
          <asp:Label runat="server" ID="lblPERT" meta:resourcekey="lblPERT" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:tipoPERT runat="server" id="frmTipoPERT" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          <asp:Label runat="server" ID="lblFusoOrario" meta:resourcekey="lblFusoOrario" />
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:timezone runat="server" id="frmTimezone" />
      </div>
    </div>
            </div>

        </div>

</asp:Content>
