<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configSplitTasksTurni.ascx.cs" Inherits="KIS.Reparti.configSplitTasksTurni" %>

<asp:Label runat="server" ID="lbl1" />
<h3>Opzioni di inserimento tasks in produzione</h3>
<p>
<asp:RadioButtonList CssClass="radio" runat="server" ID="splitTasks" OnSelectedIndexChanged="splitTasks_SelectedIndexChanged" AutoPostBack="true">
    <asp:ListItem Value="0">Inizia e finisci le attività all'interno dello stesso turno</asp:ListItem>
    <asp:ListItem Value="1">Si possono suddividere le attività tra turni</asp:ListItem>
</asp:RadioButtonList>
    </p>