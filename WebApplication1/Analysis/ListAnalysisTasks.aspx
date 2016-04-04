<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListAnalysisTasks.aspx.cs" Inherits="KIS.Analysis.ListAnalysisTasks" %>
<%@ Register TagPrefix="Tasks" TagName="List" Src="~/Analysis/ListAnalysisTasks.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">Analisi dati</a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTasks.aspx">Selezione task</a>
						<span class="divider">/</span>
					</li>
				</ul>
    <Tasks:list runat="server" id="frmListTasks" />
</asp:Content>
