<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerPortfolio.ascx.cs" Inherits="KIS.Analysis.CustomerPortfolio1" %>

<asp:Label runat="server" ID="lbl1" />

<asp:Repeater runat="server" ID="rptCustomers">
    <HeaderTemplate>
        <table class="table table-striped table-hover table-condensed">
            <thead>
            <tr>
            <th></th>
            <th>Ragione sociale
                <asp:LinkButton ID="lnkRagSocUp" runat="server" onclick="lnkRagSocUp_Click">
      <asp:Image ID="Image1" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkRagSocDown" runat="server" onclick="lnkRagSocDown_Click">
      <asp:Image ID="Image2" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
            </th>
                <th>Partita IVA
                    <asp:LinkButton ID="lnkPartitaIVAUp" runat="server" onclick="lnkPartitaIVAUp_Click">
      <asp:Image ID="Image5" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkPartitaIVADown" runat="server" onclick="lnkPartitaIVADown_Click">
      <asp:Image ID="Image6" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                <th>Codice Fiscale
                     <asp:LinkButton ID="lnkCodFiscaleUp" runat="server" onclick="lnkCodFiscaleUp_Click">
      <asp:Image ID="Image7" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkCodFiscaleDown" runat="server" onclick="lnkCodFiscaleDown_Click">
      <asp:Image ID="Image8" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                <th>Citta
                     <asp:LinkButton ID="lnkCittaUp" runat="server" onclick="lnkCittaUp_Click">
      <asp:Image ID="Image9" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkCittaDown" runat="server" onclick="lnkCittaDown_Click">
      <asp:Image ID="Image10" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                <th>CAP
                    <asp:LinkButton ID="lnkCAPUp" runat="server" onclick="lnkCAPUp_Click">
      <asp:Image ID="Image11" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkCAPDown" runat="server" onclick="lnkCAPDown_Click">
      <asp:Image ID="Image12" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                <th>Provincia
                    <asp:LinkButton ID="lnkProvinciaUp" runat="server" onclick="lnkProvinciaUp_Click">
      <asp:Image ID="Image3" ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkProvinciaDown" runat="server" onclick="lnkProvinciaDown_Click">
      <asp:Image ID="Image4" ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                <th>Stato
                    <asp:LinkButton ID="lnkStatoUp" runat="server" onclick="lnkStatoUp_Click">
      <asp:Image ImageUrl="~/img/iconArrowUp.png" alt="Ascending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                    <asp:LinkButton ID="lnkStatoDown" runat="server" onclick="lnkStatoDown_Click">
      <asp:Image ImageUrl="~/img/iconArrowDown.png" alt="Descending" runat="server" height="20" Width="12" />
</asp:LinkButton>
                </th>
                </tr>
                </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:HyperLink runat="server" ID="lnkDetailCustomer" NavigateUrl='<%#"DetailAnalysisCustomer.aspx?customerID=" + DataBinder.Eval(Container.DataItem, "CodiceCliente") %>'>
                <asp:Image runat="server" ID="imgDetailAnalisiOperatore" Height="40" ImageUrl="~/img/iconView.png" />
                </asp:HyperLink></td>
            <td><%#DataBinder.Eval(Container.DataItem, "RagioneSociale") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "PartitaIVA") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "CodiceFiscale") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Citta") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "CAP") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Provincia") %></td>
            <td><%#DataBinder.Eval(Container.DataItem, "Stato") %></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>