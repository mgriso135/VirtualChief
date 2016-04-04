<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="controlUserTasks.ascx.cs" Inherits="KIS.Produzione.controlUserTasks" %>
<asp:Label runat="server" ID="lbl1" />
    

<asp:UpdatePanel runat="server" ID="updTasks" UpdateMode="Conditional">
   
    <ContentTemplate>
   <asp:Label runat="server" ID="lblCheck" />
             <asp:Repeater runat="server" ID="rptTasks" OnItemDataBound="rptTasks_ItemDataBound">
            <HeaderTemplate>
                <table>
                <tr style="font-size:20px; font-weight:bold"><td>Linea</td>
                    <td>Matricola</td>
                    <td>Task ID</td>
                    <td>Task Name</td>
                    <td>Cadenza</td>
                    <td>Action</td>
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
                        <asp:linkbutton runat="server" ID="lnkStartTask" OnCommand="startTask_Command" CommandName="START" text="Start Task"/><br />
                        <asp:linkbutton runat="server" ID="lnkPauseTask" CommandName="PAUSE" OnCommand="pauseTask_Command" Text="PAUSE" /><br />
                        <asp:linkbutton runat="server" ID="lnkCompleteTask" CommandName="COMPLETE" OnCommand="completeTask_Command" Text="COMPLETE" />
                    </td>
                    <td>
                        <asp:linkbutton runat="server" ID="lnkWarning" CommandName="WARNING" OnCommand="warning_Command" Text="WARNING" />
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
