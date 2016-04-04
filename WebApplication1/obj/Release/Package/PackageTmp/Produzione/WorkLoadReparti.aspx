<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkLoadReparti.aspx.cs" Inherits="KIS.Produzione.WorkLoadReparti" %>
<%@ Register TagName="reparti" TagPrefix="workload" Src="~/Produzione/wlReparto.ascx" %>
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
						<a href="<%#Request.RawUrl %>">Capacità produttiva richiesta per reparto</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <workload:reparti runat="server" ID="frmListReparto" />
</asp:Content>
