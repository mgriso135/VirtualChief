<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="showCalendarFesteStraordinari.aspx.cs" Inherits="KIS.Reparti.showCalendarFesteStraordinari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("[id*=txtDateStart]").datepicker({ dateFormat: 'dd/mm/yy' })
        });

        $(function () {
            $("[id*=txtDateEnd]").datepicker({ dateFormat: 'dd/mm/yy' })
        });
    </script>
    <h2><asp:Literal runat="server" ID="lblTitleCalendarRep" Text="<%$Resources:lblTitleCalendarRep %>" /></h2>
    <asp:Label runat="server" ID="lblNomeRep" />
    <br />
    <table>
        <tr>
            <td>
                <asp:textbox runat="server" id="txtDateStart" Width="100px"  />
            </td>
            <td>
                <asp:textbox runat="server" id="txtDateEnd" Width="100px"  />
            </td>
            <td>
                <asp:ImageButton runat="server" ID="btnShowCalendar" OnClick="btnShowCalendar_Click" ImageUrl="~/img/iconView.png" Height="30" />
            </td>
        </tr>
            </table>
    
    <asp:Chart runat="server" ID="crt" Width="1000px" Height="160px">
    <ChartAreas>
    <asp:ChartArea Name="turni"></asp:ChartArea>
  </ChartAreas>    
        </asp:chart>
    <br />
    <asp:Label runat="server" ID="log" />
</asp:Content>
