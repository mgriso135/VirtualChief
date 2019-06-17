<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wlSimReparto.ascx.cs" Inherits="KIS.Produzione.wlSimReparto" %>
<asp:ScriptManager runat="server" ID="scriptMan1" />

    <script type="text/javascript">

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(function () {
                $("[id*=txtProductDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
            });
        });

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(function () {
                $("[id*=txtProductDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
            });
        });

        $(function () {
            $("[id*=txtProductDate]").datepicker({ dateFormat: 'dd/mm/yy' })
        });
    </script>              

<table class="table table-condensed">
    <tr>
    <td style="vertical-align: top;">
        <div class="accordion" id="accordion1" runat="server">

            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseOne">
          <asp:Literal runat="server" ID="lblAccCalendario" Text="<%$Resources:lblAccCalendario %>" />
    </a></div>
                <div id="collapseOne" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <asp:Literal runat="server" ID="lblDataIniziale" Text="<%$Resources:lblDataIniziale %>" />:&nbsp;<asp:TextBox runat="server" ID="txtProductDateStart" Width="80" /><br />
        <asp:Literal runat="server" ID="lblDataFinale" Text="<%$Resources:lblDataFinale %>" />:&nbsp;<asp:TextBox runat="server" ID="txtProductDateEnd" Width="80" /><br />
        <asp:ImageButton runat="server" ID="btnUpdateDate" OnClick="btnUpdateDate_Click" ImageUrl="~/img/iconSave.jpg" Width="30" />
        </ContentTemplate>

              </asp:UpdatePanel>
          </div></div>
                </div>

             <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseTwo">
          <asp:Literal runat="server" ID="lblAccPostazione" Text="<%$Resources:lblAccPostazione %>" />
    </a></div>
                <div id="collapseTwo" class="accordion-body collapse">
      <div class="accordion-inner">
          <asp:UpdatePanel runat="server">
              <ContentTemplate>
    <asp:RadioButtonList CssClass="radio" runat="server" id="rbPostazioni" AutoPostBack="true" OnSelectedIndexChanged="rbPostazioni_SelectedIndexChanged">
    <asp:ListItem Selected="True" Value="0" Text="<%$Resources:lblVisTempiTotali %>"></asp:ListItem>
    <asp:ListItem Value="1" Text="<%$Resources:lblVisTempiPostazione %>"></asp:ListItem>
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
            
            <div class="accordion-group">
                <div class="accordion-heading">
      <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion1" href="#collapseThree">
          <asp:Literal runat="server" ID="lblAccProdottiNew" Text="<%$Resources:lblAccProdottiNew %>" />
    </a></div>
                <div id="collapseThree" class="accordion-body collapse">
      <div class="accordion-inner">
    <asp:Repeater runat="server" ID="rptProdotti" OnItemCommand="rptProdotti_ItemCommand" OnItemDataBound="rptProdotti_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-condensed table-striped">
                    </HeaderTemplate>
                    <ItemTemplate>
         
                            <td><asp:HiddenField runat="server" ID="itmID" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                <asp:HiddenField runat="server" ID="itmYear" Value='<%# DataBinder.Eval(Container.DataItem, "Year") %>' />
                                <asp:CheckBox runat="server" ID="chk" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" /></td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Cliente") %>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "ID") %>/<%# DataBinder.Eval(Container.DataItem, "Year") %></td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "Matricola") %>
                            </td>
                            <td>
                                <%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;
            <%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %>
                            </td>
                            <td>
                                Fine produzione:&nbsp;<asp:textbox runat="server" id="txtProductDate" Width="100px"  />
                                
                                &nbsp;<asp:literal runat="server" id="lblOre" Text="<%$Resources:lblOre %>" />:<asp:DropDownList runat="server" ID="calOre" CssClass="dropdown" Width="70px" />
                                &nbsp;<asp:literal runat="server" id="lblMinuti" Text="<%$Resources:lblMinuti %>" />:<asp:DropDownList runat="server" ID="calMinuti" CssClass="dropdown"  Width="70px" />
                                &nbsp;<asp:literal runat="server" id="lblSecondi" Text="<%$Resources:lblSecondi %>" />:<asp:DropDownList runat="server" ID="calSecondi" CssClass="dropdown" Width="70px" />
                                <asp:ImageButton runat="server" ImageUrl="~/img/iconSave.jpg" Height="30" ID="btnSaveDataFineProd" CommandName="SaveData" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") +"/"+ DataBinder.Eval(Container.DataItem, "Year") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
          </div></div>
                </div>

            </div>   



    </td>
    <td style="vertical-align: top;">
        
        <asp:UpdatePanel runat="server"><ContentTemplate>
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
		<strong><asp:literal runat="server" id="lblAttenzione" Text="<%$Resources:lblAttenzione %>" />:</strong>&nbsp;<asp:Label runat="server" ID="lblErr" /></p>
	</div>
</div>


        <div class="ui-widget" id="dvInfo" runat="server">
	<div class="ui-state-highlight ui-corner-all" style="margin-top: 20px; padding: 0 .7em;">
		<p><span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
		<asp:Label runat="server" ID="lbl1" />
            <asp:Label runat="server" ID="lblTurni" />
		</p>
	</div>
</div>
        
            </ContentTemplate>
            </asp:UpdatePanel>
    </td>
       </tr></table>