<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wzAssociaProdottoReparto.ascx.cs" Inherits="KIS.Commesse.wzAssociaProdottoReparto" %>

<script language="javascript">
function SetUniqueRadioButton(nameregex, current) {
  var re = new RegExp(nameregex);
  for (var i = 0; i < document.forms[0].elements.length; i++) {
    var elm = document.forms[0].elements[i];
    if (elm.type == 'radio') {
      if (re.test(elm.name)) {
        elm.checked = false;
      }
    }
  }
  current.checked = true;
}
    </script>

<asp:Label runat="server" ID="lbl1" />
<div class="row-fluid">
    <div class="span1">
        <asp:HyperLink runat="server" ID="lnkGoBack" NavigateUrl="wzEditPERT.aspx">
        <asp:Image runat="server" ID="imgGoBack" ImageUrl="~/img/iconArrowLeft.png" Height="40" />
            </asp:HyperLink>
    </div>
    <div class="span6">
<asp:Repeater runat="server" ID="rptReparti" OnItemDataBound="rptReparti_ItemDataBound">
    <HeaderTemplate>
        <table class="table table-striped table-hover">
            <tbody>
                
    </HeaderTemplate>
    <ItemTemplate>
        <tr runat="server" id="tr1">
            <td>
                <input type="radio" name="rbReparto" id="rbReparto" value='<%#DataBinder.Eval(Container.DataItem,"ID") %>' />
                <asp:HiddenField runat="server" ID="idRep" Value='<%#DataBinder.Eval(Container.DataItem, "ID") %>' />
                </td>
            <td><%#DataBinder.Eval(Container.DataItem, "Name") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Description") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
        </div>
    <div class="span1">
        <asp:ImageButton runat="server" ID="imgGoFwd" ImageUrl="~/img/iconArrowRight.png" Height="40" OnClick="imgGoFwd_Click" />
    </div>
    </div>