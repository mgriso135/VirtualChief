<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListAnalysisTasks.ascx.cs" Inherits="KIS.Analysis.ListAnalysisTasks1" %>

<asp:Label runat="server" ID="lbl1" />
<asp:Repeater runat="server" ID="rptListTasks">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HyperLink runat="server" ID="lnkDetailTask" NavigateUrl='<%#"DetailAnalysisTask.aspx?processID=" + DataBinder.Eval(Container.DataItem, "processID") + "&rev=" + DataBinder.Eval(Container.DataItem, "revisione") %>'>
                    <asp:Image runat="server" ID="imgDetailTask" ImageUrl="~/img/iconView.png" Height="20" />
                </asp:HyperLink>
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "processName") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "processDescription") %></td>
        </tr>
    </ItemTemplate>
    <SeparatorTemplate>

    </SeparatorTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>