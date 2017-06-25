<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="listWarningApertiReparto.ascx.cs" Inherits="KIS.Produzione.listWarningApertiReparto" %>


<asp:UpdatePanel runat="server" ID="upd1" UpdateMode="Conditional">
    <ContentTemplate>
<h3 id="lblTitle" runat="server"><asp:Literal runat="server" ID="lblWarningAperti" Text="<%$Resources:lblWarningAperti %>" /></h3>
        <asp:Label runat="server" ID="lblData" />
        <asp:Label runat="server" ID="lbl1" />
        <asp:Repeater runat="server" ID="rptWarnings">
            <HeaderTemplate>
                <table>
            </HeaderTemplate>
            <ItemTemplate>
                <tr runat="server" id="tr1" style="font-size:18px; font-family:Calibri; background-color:red">
                    <td><asp:Image runat="server" ID="imgProblem" ImageUrl="/img/problemIcon.jpg" Height="80" /></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "id") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "NomeReparto") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "NomePostazione") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "User") %></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "DataChiamata") %></td>
                    <td style="background-color:white">
                        <asp:HyperLink runat="server" ID="lnkSolve" Target="_blank" NavigateUrl='<%# "solveProblem.aspx?id=" + DataBinder.Eval(Container.DataItem, "id") %>'>
                        <asp:Image runat="server" id="btnSolve" ImageUrl="/img/iconRubik.png" ToolTip="Problema risolto" Height="80" />
                            </asp:HyperLink>

                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Timer runat="server" ID="timer1" Interval="10000" OnTick="timer1_Tick" />
        </ContentTemplate>
    </asp:UpdatePanel>