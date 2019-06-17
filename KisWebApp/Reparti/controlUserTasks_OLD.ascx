<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="controlUserTasks_OLD.ascx.cs" Inherits="KIS.Produzione.controlUserTasks" %>
<asp:Label runat="server" ID="lbl1" />
    

<asp:UpdatePanel runat="server" ID="updTasks" UpdateMode="Conditional">
   
    <ContentTemplate>
   <asp:Label runat="server" ID="lblCheck" />
             <asp:Repeater runat="server" ID="rptTasks" OnItemDataBound="rptTasks_ItemDataBound">
            <HeaderTemplate>
                <table>
                <tr style="font-size:20px; font-weight:bold"><td>Linea</td>
                    <td><asp:literal runat="server" id="lblMatricola" Text="<%$Resources:lblMatricola %>" /></td>
                    <td><asp:literal runat="server" id="lblTaskID" Text="<%$Resources:lblTaskID %>" /></td>
                    <td><asp:literal runat="server" id="lblTaskName" Text="<%$Resources:lblTaskName %>" /></td>
                    <td><asp:literal runat="server" id="lblCadenza" Text="<%$Resources:lblCadenza %>" /></td>
                    <td><asp:literal runat="server" id="lblAzione" Text="<%$Resources:lblAzione %>" /></td>
                </tr></HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-size:16px">
                    <td><%#DataBinder.Eval(Container.DataItem, "padre") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "matricola") %></td>
                    <td><asp:HiddenField runat="server" ID="taskID" />
                        <%#DataBinder.Eval(Container.DataItem, "taskID") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "name") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "cadenza") %></td>
                    <td>
                        <asp:linkbutton runat="server" ID="lnkStartTask" OnCommand="startTask_Command" CommandName="START" text="<%$Resources:lblTTStart %>"/><br />
                        <asp:linkbutton runat="server" ID="lnkPauseTask" CommandName="PAUSE" OnCommand="pauseTask_Command" Text="<%$Resources:lblTTPause %>" /><br />
                        <asp:linkbutton runat="server" ID="lnkCompleteTask" CommandName="COMPLETE" OnCommand="completeTask_Command" Text="<%$Resources:lblTTCompleta %>" />
                    </td>
                    <td>
                        <asp:linkbutton runat="server" ID="lnkWarning" CommandName="WARNING" OnCommand="warning_Command" Text="<%$Resources:lblTTWarning %>" />
                    </td>
                </tr>

            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            
        </asp:Repeater>
        <asp:Timer runat="server" id="Timer1" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
    </ContentTemplate>
    <Triggers>
        
    </Triggers>
</asp:UpdatePanel>
