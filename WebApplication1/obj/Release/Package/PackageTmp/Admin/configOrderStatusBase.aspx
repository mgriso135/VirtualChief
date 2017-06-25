<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configOrderStatusBase.aspx.cs" Inherits="KIS.Admin.configOrderStatusBase" %>
<%@ Register TagPrefix="configReport" TagName="OrderStatusBase" Src="~/Admin/configOrderStatusBase.ascx" %>
<%@ Register TagPrefix="configReport" TagName="CustomerList" Src="~/Admin/configOrderStatusCustomer_customerList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx"><asp:label runat="server" id="lblMenuAdmin" meta:resourcekey="lblMenuAdmin" /></a>
						<span class="divider">/</span>
						<a href="manageReports.aspx"><asp:label runat="server" id="lblMenuConfigReports" meta:resourcekey="lblMenuConfigReports" /></a>
						<span class="divider">/</span>
                        <a href="configOrderStatusBase.aspx"><asp:label runat="server" id="lblMenuReportStatoProdotti" meta:resourcekey="lblMenuReportStatoProdotti" /></a>
						<span class="divider">/</span>
					</li>
				</ul>

    <h3><asp:label runat="server" id="lblTitoloConfigReportAvanzamentoProdotti" meta:resourcekey="lblTitoloConfigReportAvanzamentoProdotti" /></h3>
    <div class="accordion" id="accordion1" runat="server">
        <!-- 2 -->
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:label runat="server" id="lblConfigGenerica" meta:resourcekey="lblConfigGenerica" />
      </a>
    </div>
            <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd1">
              <ContentTemplate>
            <configReport:OrderStatusBase runat="server" id="frmCfgReportOrderStatusBase" />
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
                </div>
            </div>
        <!-- 3 -->
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          <asp:label runat="server" id="lblConfigSpecificaCliente" meta:resourcekey="lblConfigSpecificaCliente" />
      </a>
    </div>
            <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="UpdatePanel1">
              <ContentTemplate>
            <configReport:customerList runat="server" id="frmCustomerList" />
                  </ContentTemplate>
              </asp:UpdatePanel>
          </div>
                </div>
            </div>
        </div>


    
</asp:Content>