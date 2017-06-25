<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListCommesseChiuse.ascx.cs" Inherits="KIS.Analysis.ListCommesseChiuse1" %>

<asp:Repeater runat="server" ID="rptCommesseChiuse">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="row-fluid">
        <div class="span12">
            <asp:HyperLink runat="server" ID="lnkCommessa" NavigateUrl='<%#"~/Analysis/DetailCostCommessa.aspx?id=" 
            + DataBinder.Eval(Container.DataItem, "ID") + "&year=" + DataBinder.Eval(Container.DataItem, "Year") %>'>

            <asp:Image runat="server" ID="imgCommessa" ImageUrl="~/img/iconView.png" Height="40" />
                </asp:HyperLink>
            <%#DataBinder.Eval(Container.DataItem, "ID") %>/<%#DataBinder.Eval(Container.DataItem, "Year") %>&nbsp;
            <%#DataBinder.Eval(Container.DataItem, "Cliente") %>&nbsp;
            <%#DataBinder.Eval(Container.DataItem, "DataInserimento") %>&nbsp;
            <%#DataBinder.Eval(Container.DataItem, "Note") %>
        </div>
            </div>
    </ItemTemplate>
    <SeparatorTemplate>

    </SeparatorTemplate>
    <FooterTemplate>
        
    </FooterTemplate>
</asp:Repeater>