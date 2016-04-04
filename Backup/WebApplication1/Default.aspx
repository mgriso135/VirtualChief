<%@ Page Title="Process Monitor app" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>
    <%@ Register TagPrefix="dashboard" TagName="processes" Src="/Produzione/userProcesses.ascx" %>
    <%@ Register TagPrefix="dashboard" TagName="tasks" Src="/Produzione/controlUserTasks.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome!
    </h2>
    
    <asp:label runat="server" ID="lbl1"/>
    
   <dashboard:processes runat="server" id="ownProc" />
   <dashboard:tasks runat="server" id="ownTasks" />

</asp:Content>
