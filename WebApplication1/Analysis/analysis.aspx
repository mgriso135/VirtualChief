﻿<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="analysis.aspx.cs" Inherits="KIS.Analysis.analysis" %>
<%@ Register TagPrefix="analysis" TagName="home" Src="~/Analysis/AnalysisHome.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx"><asp:label runat="server" id="lblNavAnalisi" meta:resourcekey="lblNavAnalisi" /></a>
						<span class="divider">/</span>
					</li>
				</ul>
    <analysis:home runat="server" ID="frmAnalysis" />
</asp:Content>
