<%@ Page Title="Kaizen Indicator System" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="KIS._Default" %>
<%@ Register TagPrefix="Homepage" TagName="Whatsnew" Src="~/whatsnew.ascx" %>
<%@ Register TagPrefix="Homepage" TagName="userboxes" Src="~/HomePage/UserHomeBoxes.ascx" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
				<ul class="breadcrumb hidden-phone">
					<li>
						<a href="#">Home</a>
						<span class="divider">/</span>
					</li>
				</ul>
<div class="page-header">
					<asp:label runat="server" ID="lblBenvenuto" meta:resourcekey="lblBenvenuto" />
    <asp:label runat="server" ID="lbl1" meta:resourcekey="lbl1"/>
				</div>
    

       <Homepage:userboxes runat="server" id="frmHomeBoxes" />
 
    <Homepage:whatsnew runat="server" />
</asp:Content>
