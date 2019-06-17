<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailAnalysisOperatore.aspx.cs" Inherits="KIS.Analysis.DetailAnalysisOperatore" %>
<%@ Register TagPrefix="analisioperatore" TagName="tasks" Src="~/Analysis/OperatoreTempoTasks.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    

    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx"><asp:label runat="server" id="lblNavAnalisi" meta:resourcekey="lblNavAnalisi" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisOperatori.aspx"><asp:label runat="server" id="lblNavSelezionaOp" meta:resourcekey="lblNavSelezionaOp" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:hyperlink runat="server" id="lnkNavCurr"><asp:label runat="server" id="lblNavDetailAttivita" meta:resourcekey="lblNavDetailAttivita" /></asp:hyperlink>
						<span class="divider">/</span>
					</li>
				</ul>

    <asp:Label runat="server" ID="lbl1" /><br />
    <analisioperatore:tasks runat="server" ID="frmAnalisiOperatore" />
</asp:Content>
