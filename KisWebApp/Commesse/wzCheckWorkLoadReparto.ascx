<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzCheckWorkLoadReparto.ascx.cs" Inherits="KIS.Commesse.wzCheckWorkLoadReparto1" %>


<asp:ScriptManager runat="server" ID="scriptMan1" />

    <script type="text/javascript">
        $(function () {
            $("[id*=txtProductDate]").datepicker({ dateFormat: 'dd/mm/yy' },
                           showWeek: true,
                firstDay: 1,
                minDate: 'today')
        });
    </script>              

<script type="text/javascript">
    function FireConfirm() {
        if (confirm('<asp:literal runat="server" text="<%$Resources:lblConfirmLaunch%>"/>'))
            return true;
        else
            return false;
    }
</script>

<table class="table table-condensed" runat="server" id="tblContainer">
    <tr>
        <td>
            <asp:HyperLink runat="server" ID="lnkGoBack">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
                </asp:HyperLink>
        </td>
    <td style="vertical-align: top;">
        <div class="accordion" id="accordion1" runat="server">
            <div class="accordion-group">
                <div class="accordion-heading">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
                        <asp:Label runat="server" ID="lblAccDataFineProd" Text="<%$Resources:lblAccDataFineProd %>" />
    </a>
                </div>
                <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          
          <asp:Label runat="server" ID="lblData" Text="<%$Resources:lblData %>" />:&nbsp;<asp:textbox runat="server" id="txtProductDate" Width="100px"  />
          <br />
          <asp:Label runat="server" ID="lblOre" Text="<%$Resources:lblOre %>" />:<asp:DropDownList runat="server" ID="calOre" CssClass="dropdown" Width="70px" />
          &nbsp;<asp:Label runat="server" ID="lblMinuti" Text="<%$Resources:lblMinuti %>" />:<asp:DropDownList runat="server" ID="calMinuti" CssClass="dropdown"  Width="70px" />
          &nbsp;<asp:Label runat="server" ID="lblSecondi" Text="<%$Resources:lblSecondi %>" />:<asp:DropDownList runat="server" ID="calSecondi" CssClass="dropdown" Width="70px" />
          <br />
          <asp:ImageButton runat="server" ImageUrl="~/img/iconSave.jpg" Height="30" ID="btnSaveDataFineProd" OnClick="btnSaveDataFineProd_Click" />

          </div>
                    </div>
            </div>

             <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Label runat="server" ID="lblPostazioni" Text="<%$Resources:lblPostazioni %>" />
    </a></div>
                <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
              <ContentTemplate>
    <asp:RadioButtonList CssClass="radio" runat="server" id="rbPostazioni" AutoPostBack="true" OnSelectedIndexChanged="rbPostazioni_SelectedIndexChanged">
    <asp:ListItem Selected="True" Value="0" Text="<%$Resources:lblVisualizzaTempiComplessivi %>"></asp:ListItem>
    <asp:ListItem Value="1" Text="<%$Resources:lblTempiPost %>"></asp:ListItem>
</asp:RadioButtonList>
<asp:CheckBoxList runat="server" CssClass="checkbox" AutoPostBack="true" ID="chkLstPostazioni" OnSelectedIndexChanged="chkLstPostazioni_SelectedIndexChanged" >

</asp:CheckBoxList>
                  </ContentTemplate>
              <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="rbPostazioni" EventName="SelectedIndexChanged" />
                  <asp:AsyncPostBackTrigger ControlID="chkLstPostazioni" EventName="SelectedIndexChanged" />
              </Triggers>
              </asp:UpdatePanel>
          </div></div>
                </div>
                        </div>   



    </td>
    <td style="vertical-align: top;">
        
        <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
        <asp:chart ID="Chart1" runat="server" Width="1000" Height="400">
            <Series>
                <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" Name="Series1">
                    <EmptyPointStyle BorderDashStyle="Dot" IsVisibleInLegend="False" />
                </asp:Series>
            </Series>
<ChartAreas>
    <asp:ChartArea Name="ChartArea1">
        <AxisY Minimum="0">
            
        </AxisY>
    </asp:ChartArea>
  </ChartAreas>
</asp:chart>

        <br />

        <div class="ui-widget" runat="server" id="dvErr">
	<div class="ui-state-error ui-corner-all" style="padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
		<strong>Attenzione:</strong>&nbsp;<asp:Label runat="server" ID="lblErr" /></p>
	</div>
</div>


        <div class="ui-widget" id="dvInfo" runat="server">
	<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Label runat="server" ID="lbl1" /></p>
	</div>
</div>
        
            </ContentTemplate>
            </asp:UpdatePanel>
    </td>
        <td>
            <asp:ImageButton runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" OnClientClick="return FireConfirm();" OnClick="imgGoFwd_Click" />
        </td>
       </tr></table>