<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_summary.ascx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_summary1" %>
<div class="row-fluid">
            <div class="span12"><h3>
                <asp:Label runat="server" ID="lblSelProdF" meta:resourcekey="lblSelProdF" />
                </h3>
                </div>
            </div>
    <asp:label runat="server" id="lbl1" />
        <div class="row-fluid" runat="server" id="frmReport">
            <div class="span2">
                <asp:HyperLink runat="server" ID="lnkGoBack">
            <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
        </asp:HyperLink>
            </div>
            <div class="span8">
<asp:Label runat="server" ID="lblFormato" meta:resourcekey="lblFormato" />:&nbsp;<asp:DropDownList runat="server" ID="ddlFormato">
    <asp:ListItem Value="pdf">PDF</asp:ListItem>
    <asp:ListItem Value="xml" Enabled="false">XML</asp:ListItem>
                     </asp:DropDownList>
<asp:Repeater runat="server" ID="rpt1">
    <HeaderTemplate><table class="table table-striped table-condensed table-hover">
        <thead>
            <th><asp:Label runat="server" ID="lblTHIDProd" meta:resourcekey="lblTHIDProd" /></th>
            <th><asp:Label runat="server" ID="lblTHNome" meta:resourcekey="lblTHNome" /></th>
            <th><asp:Label runat="server" ID="lblTHMatricola" meta:resourcekey="lblTHMatricola" /></th>
            <th><asp:Label runat="server" ID="lblTHQuantita" meta:resourcekey="lblTHQuantita" /></th>
            <th><asp:Label runat="server" ID="lblTHStatus" meta:resourcekey="lblTHStatus" /></th>
        </thead></HeaderTemplate>
    <ItemTemplate>
        <tbody><tr><td>
                          <asp:HiddenField runat="server" ID="hArtID" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                          <asp:HiddenField runat="server" ID="hArtYear" Value='<%#DataBinder.Eval(Container.DataItem, "Year") %>' />
            <%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Proc.process.processName") %>&nbsp;-&nbsp;<%#DataBinder.Eval(Container.DataItem, "Proc.variant.nomeVariante") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Matricola") %></td>
                      <td><%#DataBinder.Eval(Container.DataItem, "Quantita") %></td>
            </td>
            <td><%#DataBinder.Eval(Container.DataItem, "Status") %></td>
        </tr>
            </tbody>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>
    </div>
            <div class="span2">
            <asp:ImageButton runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" OnClick="imgGoFwd_Click" /></div>
              </div>
