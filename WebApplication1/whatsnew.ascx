<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="whatsnew.ascx.cs" Inherits="KIS.whatsnew" %>

<asp:XmlDataSource ID="XmlDataSource1"   
    runat="server"   
    DataFile="~/whatsnew.xml"  
    XPath="Stories/Story">  
</asp:XmlDataSource>

<asp:Repeater runat="server" ID="rptNews" DataSourceID="XmlDataSource1">
    <HeaderTemplate>
<p class="lead">Cosa c'è di nuovo</p>
        <div style="overflow: scroll; height: 400px">
    </HeaderTemplate>
    <ItemTemplate>
<div class="well well-small">
            <p class="lead"><%# XPath("Date")%> <%# XPath("Title") %></p>
            <%# XPath("Description")%>
                </div>        
    </ItemTemplate>
    <SeparatorTemplate>
        
    </SeparatorTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>