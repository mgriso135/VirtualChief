<%@ Page Title="Personal Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="my.aspx.cs" Inherits="KIS.Personal.my" %>
<%@ Register TagPrefix="personal" TagName="baseInfo" Src="~/Personal/baseInfo.ascx" %>
<%@ Register TagPrefix="personal" TagName="mail" Src="~/Personal/myEmail.ascx" %>
<%@ Register TagPrefix="personal" TagName="phone" Src="~/Personal/myPhone.ascx" %>
<%@ Register TagPrefix="personal" TagName="gruppi" Src="~/Personal/listGruppiUtente.ascx" %>
<%@ Register TagPrefix="personal" TagName="eventoritardo" Src="~/Personal/myListEventoRitardo.ascx" %>
<%@ Register TagPrefix="personal" TagName="eventowarning" Src="~/Personal/myListEventoWarning.ascx" %>
<%@ Register TagPrefix="personal" TagName="changePassword" Src="~/Personal/myChangePassword.ascx" %>
<%@ Register TagPrefix="personal" TagName="homeBoxes" Src="~/Personal/HomeBoxUser.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="my.aspx">Il tuo account</a>
						<span class="divider">/</span>
					</li>
				</ul>
        <asp:hyperlink NavigateUrl="~/Login/login.aspx" runat="server" ID="lblLogin">Non sei loggato, accedi per visualizzare i tuoi dati</asp:hyperlink>

    <h3><small>Dati personali di </small><asp:Label runat="server" ID="lblUsername" /></h3>
    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          Informazioni di base
      </a>
    </div>
            <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
        <personal:baseInfo runat="server" id="frmBaseInfo" />
      </div>
    </div>
            </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
                    E-mail
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseTwo">
                <div class="accordion-inner">
                    <personal:mail runat="server" ID="frmMail" />
                </div>
            </div>
        </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
                    Telefono
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseThree">
                <div class="accordion-inner">
                    <personal:phone runat="server" ID="frmPhone" />
                </div>
            </div>
        </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFour">
                    I tuoi gruppi
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseFour">
                <div class="accordion-inner">
                    <personal:gruppi runat="server" ID="lstGruppi" />
                </div>
            </div>
        </div>
        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseFive">
                    Segnalazione ritardi
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseFive">
                <div class="accordion-inner">
                    <personal:eventoRitardo runat="server" id="frmEventoRitardo" />
                </div>
            </div>
        </div>
        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseSix">
                    Segnalazione warning
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseSix">
                <div class="accordion-inner">
                    <personal:eventowarning runat="server" id="frmEventoWarning" />
                </div>
            </div>
        </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseSeven">
                    Cambia password
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseSeven">
                <div class="accordion-inner">
                    <personal:changePassword runat="server" id="frmChangePassword" />
                </div>
            </div>
        </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseEight">
                    Box informativi in HomePage
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseEight">
                <div class="accordion-inner">
                    <personal:homeBoxes runat="server" id="frmHomeBoxes" />
                </div>
            </div>
        </div>

        </div>
</asp:Content>
