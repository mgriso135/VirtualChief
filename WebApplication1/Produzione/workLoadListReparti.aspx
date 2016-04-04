<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="workLoadListReparti.aspx.cs" Inherits="KIS.Produzione.workLoadListReparti" %>
<%@ Register TagPrefix="workload" TagName="listreparti" Src="~/Produzione/wlListReparti.ascx" %>
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
				</ul>
    <workload:listreparti runat="server" ID="frmListReparti" />
</asp:Content>
