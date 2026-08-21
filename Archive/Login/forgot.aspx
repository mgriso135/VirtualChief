<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="forgot.aspx.cs" Inherits="KIS.Login.forgot" %>
<%@ Register TagPrefix="forgot" TagName="username" Src="~/Login/forgotUsername.ascx" %>
<%@ Register TagPrefix="forgot" TagName="password" Src="~/Login/forgotPassword.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3><asp:literal runat="server" ID="lblTitleRipristinoDati" Text="<%$Resources:lblTitle %>" /></h3>
    <asp:Label runat="server" ID="lbl1" />
    <div id="row-fluid">
        <span class="span12">
        <asp:RadioButtonList runat="server" ID="rbMain" CssClass="radio" AutoPostBack="true" OnSelectedIndexChanged="btnScelta_Click">
            <asp:ListItem Value="username" Text="<%$Resources:lblForgotUsername %>"></asp:ListItem>
            <asp:ListItem Value="password" Text="<%$Resources:lblForgotPassword %>"></asp:ListItem>
        </asp:RadioButtonList>
            </span>
    </div>

    <div class="row-fluid">
        <span class="span12">
            <forgot:username runat="server" ID="frmUsername" Visible="false" />
        </span>
    </div>
    <div class="row-fluid">
        <span class="span12">
            <forgot:password runat="server" ID="frmPassword" Visible="false" />
        </span>
    </div>
</asp:Content>
