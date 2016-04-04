<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzEditPERT_updtable.ascx.cs" Inherits="KIS.Commesse.wzEditPERT_updtable1" %>

<script type="text/javascript">
    tempiCiclo = function (taskID, revTask, variante) {
        void (window.open("/Processi/addTempoCiclo.aspx?taskID=" + taskID + "&revTaskID=" + revTask + "&varianteID=" + variante, "new_window", "height=800,width=800"));
        return false;
    }

    precedenze = function (taskID, revTask, variante) {
        void (window.open("/Processi/pertManagePrecedenze2.aspx?id=" + taskID + "&revTaskID=" + revTask + "&variante=" + variante, "new_window", "height=800,width=800"));
        return false;
    }
</script>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>        
        <table class="table table-hover table-condensed table-striped" runat="server" id="tblAddTask">
    <tr>
        <td>
<b>Aggiungi un nuovo task al prodotto</b>&nbsp;<asp:ImageButton runat="server" ID="imgAddTaskPert" ImageUrl="/img/iconAdd.jpg" OnClick="addTaskPert" Height="40" />
            </td>
            </tr>
    <tr>
        <td>
<b>Collega un task esistente alla variante</b>&nbsp;
<asp:DropDownList runat="server" ID="ddlTaskEsistenti" />
<asp:ImageButton runat="server" ID="btnLnkTask" ImageUrl="/img/iconSave.jpg" Height="40px" OnClick="btnLnkTask_Click" />
            </td>
        </tr>
</table>
        <asp:Repeater runat="server" ID="rptTasks" OnItemCommand="rptTasks_ItemCommand" OnItemDataBound="rptTasks_ItemDataBound">
            <HeaderTemplate>
                <table class="table table-striped table-hover table-bordered">
                    <thead>
                    <tr>
                        <th></th>
                        <th>Nome task</th>
                        <th>Descrizione</th>
                        <th>Task precedenti</th>
                        <th>Tempi ciclo</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="vertical-align:middle;">
                    <td style="vertical-align:middle;">
                        <asp:HiddenField runat="server" ID="hTaskID" Value='<%# DataBinder.Eval(Container.DataItem, "processID") %>' />
                        <asp:HiddenField runat="server" ID="hRevTask" Value='<%# DataBinder.Eval(Container.DataItem, "revisione") %>' />
                        <asp:HyperLink runat="server" ID="lnkEditTask" NavigateUrl='<%# "~/Processi/editTask.aspx?taskID="+DataBinder.Eval(Container.DataItem, "processID")+"&revTaskID="+DataBinder.Eval(Container.DataItem, "revisione")+"&varianteID="%>' Target="_blank">
                            <asp:Image runat="server" ID="imgEditTask" Height="20" ImageUrl="~/img/edit.png" />
                        </asp:HyperLink>
                    </td>
                    <td style="vertical-align:middle;">                        
                        <%#DataBinder.Eval(Container.DataItem, "processName") %>
                    </td>
                    <td style="vertical-align:middle;">
                        <%#DataBinder.Eval(Container.DataItem, "processDescription") %>
                    </td>
                    <td style="vertical-align:middle;">
                                                <asp:label runat="server" ID="lblPrecedenti" />
                        <asp:ImageButton OnClientClick='<%# "return precedenze("+DataBinder.Eval(Container.DataItem, "processID") +", " + DataBinder.Eval(Container.DataItem, "revisione") + ", " + DataBinder.Eval(Container.DataItem, "varianteSelezionata.idVariante") + ");" %>' runat="server" ID="Image1" ImageUrl="~/img/iconConstraint1.png" Height="20" />
                        <asp:Image runat="server" ID="imgPrecedentiOK" ImageUrl="~/img/iconComplete.png" Visible="false" Height="20" />
                        <asp:Image runat="server" ID="imgPrecedentiKO" ImageUrl="~/img/iconCancel.jpg" Visible="false" Height="20" />
                    </td>
                    <td style="text-align:center; vertical-align:middle">
                        <asp:ImageButton OnClientClick='<%# "return tempiCiclo("+DataBinder.Eval(Container.DataItem, "processID") +", " + DataBinder.Eval(Container.DataItem, "revisione") + ", " + DataBinder.Eval(Container.DataItem, "varianteSelezionata.idVariante") + ");" %>' runat="server" ID="imgAddTempoCiclo" ImageUrl="~/img/iconClock.png" Height="20" />
                        
                        <asp:Image runat="server" ID="imgTempoCicloOK" ImageUrl="~/img/iconComplete.png" Visible="false" Height="20" />
                        <asp:Image runat="server" ID="imgTempoCicloKO" ImageUrl="~/img/iconCancel.jpg" Visible="false" Height="20" />
                        <asp:Label runat="server" ID="lblTCDef" />
                    </td>
                    <td style="vertical-align:middle;">
                        <asp:ImageButton runat="server" ID="imgDeleteTask" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "processID") + ";" + DataBinder.Eval(Container.DataItem, "revisione") %>' ImageUrl="~/img/iconDelete.png" Height="20">
                        </asp:ImageButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
            </asp:Repeater>
        <asp:Label runat="server" ID="lbl1" /><br />
        <asp:Label runat="server" ID="lblLastUpdate" />
        <asp:Timer runat="server" ID="timer1" OnTick="timer1_Tick" Interval="5000" />
    </ContentTemplate>
    
</asp:UpdatePanel>