<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserHomeBoxes.ascx.cs" Inherits="KIS.HomePage.UserHomeBoxes" %>
<div class="row-fluid" runat="server" id="tblOptions">
    <asp:Repeater runat="server" ID="rptUL" OnItemDataBound="rptUL_ItemDataBound">
        <HeaderTemplate>
    <ul class="thumbnails unstyled">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="span4 well" runat="server" id="li1">
                <asp:HiddenField runat="server" ID="boxID" Value='<%# DataBinder.Eval(Container.DataItem, "homeBox.ID") %>' />
            </li>
        </ItemTemplate>
        <FooterTemplate>
        </ul>
            </FooterTemplate>
        </asp:Repeater>
        </div>