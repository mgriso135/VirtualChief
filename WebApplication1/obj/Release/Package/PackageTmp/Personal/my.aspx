<%@ Page Title="Personal Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="my.aspx.cs" Inherits="KIS.Personal.my" %>
<%@ Register TagPrefix="personal" TagName="baseInfo" Src="~/Personal/baseInfo.ascx" %>
<%@ Register TagPrefix="personal" TagName="mail" Src="~/Personal/myEmail.ascx" %>
<%@ Register TagPrefix="personal" TagName="phone" Src="~/Personal/myPhone.ascx" %>
<%@ Register TagPrefix="personal" TagName="gruppi" Src="~/Personal/listGruppiUtente.ascx" %>
<%@ Register TagPrefix="personal" TagName="eventoritardo" Src="~/Personal/myListEventoRitardo.ascx" %>
<%@ Register TagPrefix="personal" TagName="eventowarning" Src="~/Personal/myListEventoWarning.ascx" %>
<%@ Register TagPrefix="personal" TagName="changePassword" Src="~/Personal/myChangePassword.ascx" %>
<%@ Register TagPrefix="personal" TagName="homeBoxes" Src="~/Personal/HomeBoxUser.ascx" %>
<%@ Register TagPrefix="personal" TagName="language" Src="~/Personal/myLanguage.ascx" %>

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

    <h3><small><asp:label runat="server" id="lblDatiPersonali" meta:resourcekey="lblDatiPersonali" />
        </small><asp:Label runat="server" ID="lblUsername" /></h3>
    <div class="accordion" id="accordion1" runat="server">
        <div class="accordion-group">
            <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:label runat="server" lbl="lblInfoBase" meta:resourcekey="lblInfoBase" />
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
                    <asp:label runat="server" lbl="lblEMail" meta:resourcekey="lblEMail" />
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
                    <asp:Label runat="server" ID="lblTelefono" meta:resourcekey="lblTelefono" />
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
                    <asp:Label runat="server" ID="lblGruppi" meta:resourcekey="lblGruppi" />
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
                    <asp:Label runat="server" ID="lblSegnalazioneRitardi" meta:resourcekey="lblSegnalazioneRitardi" />
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
                    <asp:Label runat="server" ID="lblSegnalazioneWarning" meta:resourcekey="lblSegnalazioneWarning" />
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
                    <asp:Label runat="server" ID="lblCambiaPassword" meta:resourcekey="lblCambiaPassword" />
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
                    <asp:Label runat="server" ID="lblInfoBox" meta:resourcekey="lblInfoBox" />
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseEight">
                <div class="accordion-inner">
                    <personal:homeBoxes runat="server" id="frmHomeBoxes" />
                </div>
            </div>
        </div>

        <div class="accordion-group">
            <div class="accordion-heading">
                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseNine">
                    <asp:Label runat="server" ID="lblLanguage" meta:resourcekey="lblLanguage" />
                </a>
            </div>
            <div class="accordion-body collapse" id="collapseNine">
                <div class="accordion-inner">
                    <personal:language runat="server" id="frmLanguage" />
                </div>
            </div>
        </div>

        </div>
</asp:Content>
