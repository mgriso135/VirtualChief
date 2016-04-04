<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzAssociaTaskPostazioni.ascx.cs" Inherits="KIS.Commesse.wzAssociaTaskPostazioni1" %>


<asp:Label runat="server" ID="lbl1" />   

    <h4><asp:Label runat="server" ID="lblTitle" /></h4>

    <asp:Repeater runat="server" ID="rptTasksPostazioni" OnItemDataBound="rptTasksPostazioni_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
        <thead>
        <tr>
            <td>Task</td>
            <td>Postazione</td>
        </tr>
            </thead>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr runat="server" id="tr1">
                <td>
                    <asp:HiddenField runat="server" ID="taskID" Value='<%#DataBinder.Eval(Container.DataItem, "processID") %>' />
                    <%#DataBinder.Eval(Container.DataItem, "processName") %>
                </td>
                <td>
                   
                    <asp:DropDownList Width="300" runat="server" ID="ddlPostazioni" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPostazioni_SelectedIndexChanged" AutoPostBack="true" />
                            
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>