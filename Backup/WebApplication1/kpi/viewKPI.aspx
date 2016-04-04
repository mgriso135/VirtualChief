<%@ Page Language="C#" MasterPageFile="/Site.master" AutoEventWireup="true"
CodeBehind="viewKPI.aspx.cs" Inherits="WebApplication1.kpi.viewKPI" %>
<%@Register TagPrefix="kpi" TagName="recordNow" src="/kpi/recordValueNow.ascx" %>
<%@Register TagPrefix="kpi" TagName="showKPIRecords" src="/kpi/showKPIRecords.ascx" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<asp:Label runat="server" ID="lbl1" />
<a href="" runat="server" id="lblBack">Torna al processo</a>
<br />
<br />

<h1>Key Performance Indicator</h1>

<table border="0" runat="server">
<tr>
<td valign="top">
<asp:ImageButton ID="imgRecordValue" runat="server" src="/img/iconRecord.png" onClick="iconRecord_Click" Height="100px" /><br />
<kpi:recordNow runat="server" ID="recNow" /></td>

<td valign="top">
<asp:Label runat="server" ID="nomePROC" style="font-size: 18px; font-weight: bold;" />
<asp:Label runat="server" ID="nomeKPI" style="font-size: 18px; font-weight: bold;" />
<asp:TextBox runat="server" ID="inputNomeKpi" />
<br />
<asp:Label runat="server" ID="descrKPI" style="font-size: 14px; font-style:italic;" />
<asp:textbox runat="server" TextMode="MultiLine" ID="inputDescrKPI" />
<br />
Default value:&nbsp;<asp:Label runat="server" ID="baseVal" style="font-size: 14px; font-style:italic;" />
<asp:textbox runat="server" ID="inputBaseVal" />&nbsp;secondi
<br />
<asp:ImageButton runat="server" ID="imgSaveKPI" src="/img/iconSave.jpg" Height="40px" OnClick="saveIcon_Click" />
<asp:ImageButton runat="server" ID="imgIconCancel" src="/img/iconCancel.jpg" Height="40px" OnClick="cancelSaveIcon_Click" />
<asp:ImageButton ID="imgEditKPI" runat="server" src="/img/edit.png" Height="40px" OnClick="editIcon_Click" />
<asp:ImageButton ID="imgTrashKPI" runat="server" src="/img/iconDelete.png" Height="40px" OnClick="trashIcon_Click" AlternateText="Move KPI to trash. It will be possible to resume it in the future without losing saved data."/>
<asp:ImageButton ID="imgDeleteKPI" runat="server" src="/img/iconMinus.png" Height="40px" OnClick="deleteIcon_Click" AlternateText="Definitely deletes KPI and related recorded values."/>
</td>
</tr>
</table>

<br />
<br />
<hr />
<kpi:showKPIRecords runat="server" id="showRecs" kpiID="-1" />

    </asp:Content>
