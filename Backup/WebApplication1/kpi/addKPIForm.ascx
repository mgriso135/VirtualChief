﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addKPIForm.ascx.cs" Inherits="WebApplication1.kpi.addKPIForm" %>

<asp:Label runat="server" ID="lblKPI" />
<asp:label runat="server" style="font-size:20px" id="kpi_title">Key Performance Indicators</asp:label>
<asp:imagebutton runat="server" ID="imgAddKPI" src="/img/iconAdd.jpg" Height="40px" OnClick="imgAddKPI_Click" /><br />
<asp:label runat="server" ID="lblInputNameKPI">Name:&nbsp;</asp:label>
<asp:TextBox runat="server" ID="nomeKPI" style="font-size:12px; font-family: calibri;" />
<asp:RequiredFieldValidator ID="valNome" runat="server" ControlToValidate="nomeKPI" ErrorMessage="CAMPO OBBLIGATORIO" ForeColor="Red" />
<br />
<asp:label runat="server" ID="lblInputDescrKPI">Description:&nbsp;</asp:label>
<asp:TextBox runat="server" ID="descrKPI" style="font-size:12px; font-family: calibri;" Height="30px" Width="150px" TextMode="MultiLine" />
<asp:RequiredFieldValidator ID="valDesc" runat="server" ControlToValidate="descrKPI" ErrorMessage="CAMPO OBBLIGATORIO" ForeColor="Red" />
<br />
<asp:ImageButton runat="server" ID="btnADDKPI" src="/img/iconSave.jpg" onClick="btnADDKPI_Click" Height="40px" />
<asp:ImageButton runat="server" ID="imgCancel" src="/img/iconCancel.jpg" onClick="imgCancel_Click" Height="40px" />