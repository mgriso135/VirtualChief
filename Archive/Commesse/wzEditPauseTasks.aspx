<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzEditPauseTasks.aspx.cs" Inherits="KIS.Commesse.wzEditPauseTasks" %>
<%@ Register TagPrefix="process" TagName="pause" Src="~/Commesse/wzEditPauseTasks.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" ID="scriptMan1" />
    <process:pause runat="server" id="frmPause" />
</asp:Content>
