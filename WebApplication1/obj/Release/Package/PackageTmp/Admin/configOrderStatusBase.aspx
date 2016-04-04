<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="configOrderStatusBase.aspx.cs" Inherits="KIS.Admin.configOrderStatusBase" %>
<%@ Register TagPrefix="configReport" TagName="OrderStatusBase" Src="~/Admin/configOrderStatusBase.ascx" %>
<%@ Register TagPrefix="configReport" TagName="CustomerList" Src="~/Admin/configOrderStatusCustomer_customerList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="/Admin/admin.aspx">Admin</a>
						<span class="divider">/</span>
						<a href="manageReports.aspx">Configurazione reports</a>
						<span class="divider">/</span>
                        <a href="configOrderStatusBase.aspx">Report stato avanzamento prodotti per cliente</a>
						<span class="divider">/</span>
					</li>
				</ul>

    <h3>Configurazione report stato avanzamento prodotti cliente</h3>
    <div class="accordion" id="accordion1" runat="server">
        <!-- 2 -->
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          Configurazione generica
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
          Configurazione specifica per cliente
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