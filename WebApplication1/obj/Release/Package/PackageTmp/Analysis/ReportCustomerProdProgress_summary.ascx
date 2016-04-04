<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportCustomerProdProgress_summary.ascx.cs" Inherits="KIS.Analysis.ReportCustomerProdProgress_summary1" %>
<script type="text/javascript">
    function FireConfirm() {
        /*if (confirm('Stai per lanciare il prodotto in produzione. Nessuna ulteriore modifica sarà possibile. Vuoi proseguire?'))
            return true;
        else
            return false;**/
        alert('Attendere prego: in pochi secondi sarà disponibile il download del report');
        return true;
    }
</script>
<div class="row-fluid">
            <div class="span12"><h3>Seleziona prodotti finiti da inserire nel report</h3>
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
Formato report:&nbsp;<asp:DropDownList runat="server" ID="ddlFormato">
    <asp:ListItem Value="pdf">PDF</asp:ListItem>
    <asp:ListItem Value="xml" Enabled="false">XML</asp:ListItem>
                     </asp:DropDownList>
<asp:Repeater runat="server" ID="rpt1">
    <HeaderTemplate><table class="table table-striped table-condensed table-hover">
        <thead>
            <th>ID prodotto</th>
            <th>Nome</th>
            <th>Matricola</th>
            <th>Quantità</th>
            <th>Status</th>
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
            <asp:ImageButton runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" OnClientClick="return FireConfirm();" OnClick="imgGoFwd_Click" /></div>
              </div>
