<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configurationStatus.ascx.cs" Inherits="KIS.Configuration.configurationStatus" %>

<div class="row-fluid">

    <div class="span10 offset2">
        <h3>Stato della configurazione di Kaizen Indicator System</h3>
<table class="table-center">
    <tr>
        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/img/logo_KP.jpg" Height="60" /></td>
<td><a href="wizConfigLogo.aspx"><p class="lead">Logo aziendale</p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="logoOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="Elemento configuration con successo" />
            <asp:Image runat="server" ID="logoKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="Attenzione: complatare la configurazione!"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image2" runat="server" ImageUrl="~/img/iconDepartment.png" Height="60" /></td>
<td><a href="#"><p class="lead">Reparti produttivi</p></a>
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
    </table>
    </div></div>