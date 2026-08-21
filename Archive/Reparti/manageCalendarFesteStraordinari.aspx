<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageCalendarFesteStraordinari.aspx.cs" Title="Virtual Chief"
 MasterPageFile="~/Site.master" Inherits="KIS.Reparti.manageCalendarFesteStraordinari" %>
<%@ Register TagPrefix="calendario" TagName="addStraordinario" Src="~/Reparti/addStraordinario.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h2><asp:Literal runat="server" ID="lblCalendarioRep" Text="<%$Resources:lblCalendarioRep %>" /></h2>
    <asp:Label runat="server" ID="lblNomeRep" />
    <br />
    <asp:HyperLink runat="server" ID="lnkFestivita" NavigateUrl="manageFestivita.aspx?id=" Target="_blank">
    <asp:Image runat="server" ID="imgShowAddFest" ImageUrl="~/img/iconHoliday.png" Height="60px" ToolTip="<%$Resources:lblTTFestivita %>" />
    </asp:HyperLink>
    <asp:HyperLink runat="server" ID="lnkStraordinario" NavigateUrl="manageStraordinario.aspx?id=" Target="_blank">
        <asp:Image runat="server" ID="imgShowAddStraordinario" ImageUrl="~/img/iconOvertime.jpg" Height="60px" ToolTip="<%$Resources:lblTTStraordinari %>" />
    </asp:HyperLink>
    <br />

    <table>
        <tr>
            <td>
     runat="server" ID="dtStartCal" OnSelectionChanged="dtStartCal_SelectionChanged"/></td>
            <td>
 runat="server" ID="dtEndCal" OnSelectionChanged="dtEndCal_SelectionChanged" />
            </td>
        </tr>
            </table>
    
    
    
    
  </ChartAreas>    
        
    <br />
    <asp:Label runat="server" ID="log" />
    </asp:Content>