<%@ Page Title="" Language="C#" MasterPageFile="~/Configuration/WizConfig.Master" AutoEventWireup="true" CodeBehind="wizConfigAndon.aspx.cs" Inherits="KIS.Configuration.wizConfigAndon" %>
<%@ Register TagPrefix="Andon" TagName="FormatoUsername" Src="~/Andon/configFormatoUsername.ascx" %>
<%@ Register TagPrefix="Andon" TagName="ViewFields" Src="~/Andon/AndonCompletoViewFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<asp:ScriptManager runat="server" ID="scriptman1" />
    <h3><asp:Literal runat="server" ID="lblTitleAndonCfg" Text="<%$Resources:lblTitleCfgAndon %>" /></h3>
    <asp:Label runat="server" ID="lbl1" CssClass="text-info" />

    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Literal runat="server" ID="lblNavUserView" Text="<%$Resources:lblNavUserView %>" />
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <Andon:FormatoUsername runat="server" ID="frmFormatoUsername" />

      </div>
    </div>
            </div>
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Literal runat="server" ID="lblNavCampi" Text="<%$Resources:lblNavCampi %>" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
        <Andon:ViewFields runat="server" id="frmViewFields" />
      </div>
    </div>
            </div>
        </div>
</asp:Content>
