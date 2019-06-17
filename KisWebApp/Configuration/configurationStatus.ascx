<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configurationStatus.ascx.cs" Inherits="KIS.Configuration.configurationStatus" %>

<div class="row-fluid">

    <div class="span10 offset2">
        <h3><asp:Literal runat="server" ID="lblTitleCfgKIS" Text="<%$Resources:lblTitleCfgKIS %>" /></h3>
        <asp:Label runat="server" ID="lbl1" />
<table class="table-center">
    <tr>
        <td><asp:Image ID="Image5" runat="server" ImageUrl="~/img/iconAdmin.png" Height="60" /></td>
<td><a href="wizConfigAdminUser.aspx"><p class="lead"><asp:Literal runat="server" ID="lblAdmin" Text="<%$Resources:lblAdmin %>" /></p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="adminOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>" />
            <asp:Image runat="server" ID="adminKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgKo %>"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image1" runat="server" ImageUrl="~/img/LogoKIS.jpg" Height="60" /></td>
<td><a href="wizConfigLogo.aspx"><p class="lead"><asp:Literal runat="server" ID="lblLogo" Text="<%$Resources:lblLogo %>" /></p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="logoOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>" />
            <asp:Image runat="server" ID="logoKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgKo %>"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image6" runat="server" ImageUrl="~/img/iconTimezone.png" Height="60" /></td>
<td><a href="wizConfigTimezone.aspx"><p class="lead"><asp:Literal runat="server" ID="lblTimezone" Text="<%$Resources:lblTimezone %>" /></p></a>
    </td>
        <td>
            <asp:Image runat="server" ID="timezoneOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>" />
            <asp:Image runat="server" ID="timezoneKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgKo %>"/>
        </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image4" runat="server" ImageUrl="~/img/iconUser.png" Height="60" /></td>
<td>
<a href="wizConfigUsers_Main.aspx"><p class="lead"><asp:Literal runat="server" ID="lblUtenti" Text="<%$Resources:lblUtenti %>" /></p></a>
    </td>
        <td><asp:Image runat="server" ID="UtentiOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>"/>
            <asp:Image runat="server" ID="UtentiKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="<%$Resources:lblTTCfgKo %>"/></td>
        </tr>
    <tr>
        <td><asp:Image ID="Image2" runat="server" ImageUrl="~/img/iconDepartment.png" Height="60" /></td>
<td><a href="wizConfigReparti_Main.aspx"><p class="lead"><asp:Literal runat="server" ID="lblReparti" Text="<%$Resources:lblReparti %>" />
                                         </p></a>
    <td>
            <asp:Image runat="server" ID="repartoOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>"/>
            <asp:Image runat="server" ID="repartoKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="<%$Resources:lblTTCfgKo %>"/>
        </td>
    </tr>
    
<tr>
    <td><asp:Image ID="Image3" runat="server" ImageUrl="~/img/iconWorkspace.png" Height="60" /></td>
    <td>
    <a href="wizConfigPostazioni_Main.aspx"><p class="lead"><asp:Literal runat="server" ID="lblPostazioni" Text="<%$Resources:lblPostazioni %>" /></p></a>
        </td>
    <td>
        <asp:Image runat="server" ID="PostazioniOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>"/>
            <asp:Image runat="server" ID="PostazioniKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="<%$Resources:lblTTCfgKo %>"/>
    </td>
    </tr>
    <tr>
        <td><asp:Image ID="Image7" runat="server" ImageUrl="~/img/iconAndon.png" Height="60" /></td>
<td>
<a href="wizConfigAndon.aspx"><p class="lead"><asp:Literal runat="server" ID="lblAndon" Text="<%$Resources:lblAndon %>" /></p></a>
    </td>
        <td><asp:Image runat="server" ID="andonOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>"/>
            <asp:Image runat="server" ID="andonKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="<%$Resources:lblTTCfgKo %>"/></td>
        </tr>
    <tr>
        <td><asp:Image ID="Image8" runat="server" ImageUrl="~/img/iconReportProductionProgress.jpg" Height="60" /></td>
<td>
<a href="wizCustomerReport.aspx"><p class="lead"><asp:Literal runat="server" ID="lblReport" Text="<%$Resources:lblReport %>" /></p></a>
    </td>
        <td><asp:Image runat="server" ID="reportOK" ImageUrl="~/img/iconComplete.png" Height="60" Visible="false" ToolTip="<%$Resources:lblTTCfgOk %>"/>
            <asp:Image runat="server" ID="reportKO" ImageUrl="~/img/iconWarning.png" Height="60" Visible="false"  ToolTip="<%$Resources:lblTTCfgKo %>"/></td>
        </tr>
    </table>
    </div></div>