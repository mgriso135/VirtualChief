<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkLoadSimReparti.aspx.cs" Inherits="KIS.Produzione.WorkLoadSimReparti" %>
<%@ Register TagName="reparti" TagPrefix="SimWorkload" Src="~/Produzione/wlSimReparto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
                    <li>
						<a href="produzione.aspx">Produzione</a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="workLoadListReparti.aspx">Capacità produttiva</a>
						<span class="divider">/</span>
					</li>
                    <li>
						<a href="<%#Request.RawUrl %>">Simulazione capacità produttiva richiesta per reparto</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <SimWorkload:Reparti runat="server" ID="frmListReparto" />
</asp:Content>
