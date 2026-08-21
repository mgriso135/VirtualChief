<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="kisAdmin.aspx.cs" Inherits="KIS.Admin.kisAdmin" %>
<%@ Register TagPrefix="config" TagName="logo" Src="~/Admin/configLogo.ascx" %>
<%@ Register TagPrefix="config" TagName="tipoPERT" Src="~/Admin/configWizard_TipoPERT.ascx" %>
<%@ Register TagPrefix="config" TagName="TimeZone" Src="~/Admin/configTimezone.ascx" %>
<%@ Register TagPrefix="config" TagName="Billing" Src="~/Admin/kisBilling.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function () { 

            function loadRemoteSalesOrders() {
                $.ajax({
                        url: "../Config/Config/SalesOrderImport3PartySystemActive",
                        type: 'POST',
                        dataType: "html",
                        data: {
                        },
                        success: function (result) {
                            $("#frmRemoteSalesOrders").html(result);
                            $("#imgLoadRemoteSalesOrders").fadeOut();
                        },
                        error: function (result) {
                            //alert("Error");
                            //$("#frmTasksInExecution").html("Error");
                            $("#imgLoadRemoteSalesOrders").fadeOut();
                        },
                        warning: function (result) {
                            //alert("Warning");
                            //$("#frmTasksInExecution").html("Warning");
                            $("#imgLoadRemoteSalesOrders").fadeOut();
                        }
                    });
            }

            $("#imgLoadRemoteSalesOrders").fadeIn();
            loadRemoteSalesOrders();

            });
    </script>
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
        <li>
						<a href="../Admin/admin.aspx">
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
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseExpiry">
          <asp:Label runat="server" ID="lblBilling" meta:resourcekey="lblBilling" />
      </a>
    </div>
            <div id="collapseExpiry" class="accordion-body collapse">
      <div class="accordion-inner">
        <config:Billing runat="server" id="frmBilling" />
      </div>
    </div>
            </div>

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

        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFour">
          <asp:Label runat="server" ID="Label1" meta:resourcekey="lblSalesOrder3rdParty" />
      </a>
    </div>
            <div id="collapseFour" class="accordion-body collapse">
      <div class="accordion-inner">
        
          <img src="../img/iconLoading.gif" style="min-width:15px; max-width:20px;" id="imgLoadRemoteSalesOrders" />
          <div id="frmRemoteSalesOrders" />

      </div>
    </div>
            </div>

        </div>

</asp:Content>
