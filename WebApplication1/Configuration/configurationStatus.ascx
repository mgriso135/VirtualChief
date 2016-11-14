<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configurationStatus.ascx.cs" Inherits="KIS.Configuration.configurationStatus" %>

<div class="row-fluid">

    <div class="span10 offset2">
        <h3>Stato della configurazione di Kaizen Indicator System</h3>
<table class="table-center">
    <tr>
        <td><asp:Image ID="Image5" runat="server" ImageUrl="~/img/iconAdmin.png" Height="60" /></td>
<td><a href="wizConfigAdminUser.aspx"><p class="lead">Utente amministratore</p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="adminOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo" />
            <asp:Image runat="server" ID="adminKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="Attenzione: complatare la configurazione!"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/img/LogoKIS.jpg" Height="60" /></td>
<td><a href="wizConfigLogo.aspx"><p class="lead">Logo aziendale</p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="logoOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo" />
            <asp:Image runat="server" ID="logoKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="Attenzione: complatare la configurazione!"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image6" runat="server" ImageUrl="~/img/iconTimezone.png" Height="60" /></td>
<td><a href="wizConfigTimezone.aspx"><p class="lead">Time Zone</p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="timezoneOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo" />
            <asp:Image runat="server" ID="timezoneKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="Attenzione: complatare la configurazione!"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image2" runat="server" ImageUrl="~/img/iconDepartment.png" Height="60" /></td>
<td><a href="wizConfigReparti_Main.aspx"><p class="lead">Reparti produttivi</p></a>
    <td>
            <asp:Image runat="server" ID="repartoOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo"/>
            <asp:Image runat="server" ID="repartoKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="Attenzione: complatare la configurazione!"/>
        </td>
    </tr>
    
<tr>
    <td><asp:Image ID="Image3" runat="server" ImageUrl="~/img/iconWorkspace.png" Height="60" /></td>
    <td>
    <a href="#"><p class="lead">Postazioni di lavoro</p></a>
        </td>
    <td>
        <asp:Image runat="server" ID="PostazioniOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo"/>
            <asp:Image runat="server" ID="PostazioniKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="Attenzione: complatare la configurazione!"/>
    </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image4" runat="server" ImageUrl="~/img/iconUser.png" Height="60" /></td>
<td>
<a href="#"><p class="lead">Utenti ed operatori</p></a>
    </td>
        <td><asp:Image runat="server" ID="UtentiOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo"/>
            <asp:Image runat="server" ID="UtentiKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="Attenzione: complatare la configurazione!"/></td>
        </tr>
    <tr>
        <td><asp:Image ID="Image7" runat="server" ImageUrl="~/img/iconAndon.png" Height="60" /></td>
<td>
<a href="#"><p class="lead">Andon</p></a>
    </td>
        <td><asp:Image runat="server" ID="andonOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo"/>
            <asp:Image runat="server" ID="andonKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="Attenzione: complatare la configurazione!"/></td>
        </tr>
    <tr>
        <td><asp:Image ID="Image8" runat="server" ImageUrl="~/img/iconReportProductionProgress.jpg" Height="60" /></td>
<td>
<a href="#"><p class="lead">Reportistica per cliente</p></a>
    </td>
        <td><asp:Image runat="server" ID="reportOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo"/>
            <asp:Image runat="server" ID="reportKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="Attenzione: complatare la configurazione!"/></td>
        </tr>
    </table>
    </div></div>