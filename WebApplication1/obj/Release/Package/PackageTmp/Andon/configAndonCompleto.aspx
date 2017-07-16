<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configAndonCompleto.aspx.cs" Inherits="KIS.Andon.configAndonCompleto" %>
<%@ Register TagPrefix="Andon" TagName="FormatoUsername" Src="~/Andon/configFormatoUsername.ascx" %>
<%@ Register TagPrefix="Andon" TagName="ViewFields" Src="~/Andon/AndonCompletoViewFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
        <li>
						<a href="../Admin/admin.aspx">
                            <asp:Label runat="server" ID="lblNavAdmin" meta:resourcekey="lblNavAdmin" />
                            </a>
						<span class="divider">/</span>
					</li>
					<li>
						<a href="configAndonCompleto.aspx">
                            <asp:Label runat="server" ID="lblNavAndonCfg" meta:resourcekey="lblNavAndonCfg" />
                            </a>
						<span class="divider">/</span>
					</li>
				</ul>
    <h3><asp:Label runat="server" ID="lblNavAndonCfg2" meta:resourcekey="lblNavAndonCfg" /></h3>
    <asp:Label runat="server" ID="lbl1" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Label runat="server" ID="lblAccVisUtenti" meta:resourcekey="lblAccVisUtenti" />
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
          <asp:Label runat="server" ID="lblAccVisCampiAndon" meta:resourcekey="lblAccVisCampiAndon" />
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
