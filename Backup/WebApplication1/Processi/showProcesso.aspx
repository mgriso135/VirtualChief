<%@ Page Title="Process Monitor app" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"  
CodeBehind="showProcesso.aspx.cs" Inherits="WebApplication1.Processi.showProcesso" ValidateRequest="false" %>
<%@Register TagPrefix="kpi" TagName="add" src="/kpi/addKPIForm.ascx" %>
<%@Register TagPrefix="kpi" TagName="list" src="/kpi/listKPIs.ascx" %>
<%@Register TagPrefix="pert" TagName="showPert" src="/Processi/showPert.ascx" %>
<%@Register TagPrefix="pert" TagName="precedenzePert" src="/Processi/managePrecedenzePERT.ascx" %>


<%@Reference Control="/Processi/boxProcesso.ascx" %>



<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<asp:Label runat="server" ID="lblErr" />

<asp:Label runat="server" ID="lblLinkFatherProc" />
<asp:Label runat="server" ID="lblProcID" Visible="false" />
<asp:label runat="server" id="lblProcName" style="font-size:20px;"/><asp:TextBox runat="server" ID="inputProcName" style="font-size:20px;" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcName" ForeColor="Red" ErrorMessage="Campo obbligatorio" />
<br />
<asp:label runat="server" id="lblProcDesc" style="font-size:14px;" /><asp:TextBox runat="server" ID="inputProcDesc" style="font-size:14px;" />
<asp:RequiredFieldValidator runat="server" ControlToValidate="inputProcDesc" ForeColor="Red" ErrorMessage="Campo obbligatorio" />
<br />
<asp:label runat="server" id="vsm" />
<asp:DropDownList runat="server" ID="inputVSM">
<asp:ListItem Value="1">VSM</asp:ListItem>
<asp:ListItem Value="0">Pert</asp:ListItem>
</asp:DropDownList><br />
<asp:Label runat="server" ID="processOwner" />
<asp:DropDownList runat="server" id="ddlProcessOwner" AppendDataBoundItems="true" /><br />
<asp:imagebutton runat="server" id="imgEdit" src="/img/edit.png" Width="50px" onClick="imgEdit_Click"/>
<asp:imagebutton runat="server" id="imgDeleteProcess" src="/img/iconDelete.png" Width="50px" onClick="imgDeleteProcess_Click"/>
<asp:imagebutton runat="server" id="imgSave" src="/img/iconSave.jpg" Width="50px" OnClick="imgSave_Click" />
<asp:imagebutton runat="server" id="imgCancel" src="/img/iconCancel.jpg" Width="50px" OnClick="imgCancel_Click" />
<br />
<br />



<asp:panel style="height:200px; position: static;" runat="server" id="containerVSM">
<asp:PlaceHolder runat="server" ID="ProcStream" />
</asp:panel>


<pert:precedenzePert runat="server" id="precedenzePERT" />
<pert:showPert runat="server" ID="pert" />


<br />
<br />

<div style="position: relative; top: 300px; left: 0px;">
<kpi:add runat="server" ID="Add" />
<kpi:list runat="server" ID="ListKPI" />
</div>

</asp:Content>