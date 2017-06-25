<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailAnalysisTask.aspx.cs" Inherits="KIS.Analysis.DetailAnalysisTask" %>
<%@ Register TagPrefix="TasksAnalysis" TagName="Detail" Src="~/Analysis/DetailAnalysisTask.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="breadcrumb hidden-phone">
					<li>
						<a href="analysis.aspx">
                            <asp:Label runat="server" id="lblNavAnalysis" meta:resourcekey="lblNavAnalysis" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<a href="ListAnalysisTasks.aspx"><asp:Label runat="server" id="lblNavTaskSelection" meta:resourcekey="lblNavTaskSelection" /></a>
						<span class="divider">/</span>
					</li>
        <li>
						<asp:HyperLink NavigateUrl="DetailAnalysisTask.aspx" runat="server" id="lnkNavigation">
                            <asp:Label runat="server" id="lblNavAnalisiTask" meta:resourcekey="lblNavAnalisiTask" />
                            </asp:HyperLink>
						<span class="divider">/</span>
					</li>
				</ul>
        <asp:Label runat="server" ID="lbl1" />
    <TasksAnalysis:Detail runat="server" id="frmAnalisiTask" />
</asp:Content>
