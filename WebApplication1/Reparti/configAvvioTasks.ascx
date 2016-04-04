﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="configAvvioTasks.ascx.cs" Inherits="KIS.Reparti.configAvvioTasks" %>
<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Label runat="server" ID="lbl1" />
        <asp:RadioButtonList runat="server" ID="rb1" AutoPostBack="true" OnSelectedIndexChanged="rb1_SelectedIndexChanged" CssClass="radio">
            <asp:ListItem Value="0">Un operatore può avviare infiniti tasks in contemporanea</asp:ListItem>
            <asp:ListItem Value="1">Limita il numero di tasks che un operatore può avviare contemporaneamente</asp:ListItem>
        </asp:RadioButtonList>
        <asp:DropDownList runat="server" ID="ddlLimiteTask" Width="60" AutoPostBack="true" OnSelectedIndexChanged="ddlLimiteTask_SelectedIndexChanged" />
    </ContentTemplate>
</asp:UpdatePanel>