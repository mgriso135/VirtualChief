<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="solveProblem.aspx.cs" Inherits="KIS.Produzione.solveProblem" %>
<%@ Register TagPrefix="warning" TagName="solve" Src="~/Produzione/frmSolveProblem.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <warning:solve runat="server" id="frmSolveProblem" />
</asp:Content>
