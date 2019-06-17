<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuBar.ascx.cs" Inherits="KIS.menuBar" %>

<script type="text/javascript">
    $(function () {
        //to fix collapse mode width issue
        $(".nav li,.nav li a,.nav li ul").removeAttr('style');

        //for dropdown menu
        $(".dropdown-menu").parent().removeClass().addClass('dropdown');
        $(".dropdown>a").removeClass().addClass('dropdown-toggle').append('<b class="caret"></b>').attr('data-toggle', 'dropdown');

        //remove default click redirect effect           
        $('.dropdown-toggle').attr('onclick', '').off('click');

    });
   </script>
<%--<asp:Label runat="server" ID="lbl1" />
<div class="clear hideSkiplink">

<asp:Menu runat="server" ID="frmMenu" CssClass="menu" >

</asp:Menu>--%>

    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner pull-center">
        <div class="container">
         <!-- .btn-navbar is used as the toggle for collapsed navbar content -->
            <button type="button" class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
							</button>
        <!-- Everything you want hidden at 940px or less, place within here -->
            <div class="nav-collapse collapse">

    <asp:Menu runat="server" ID="frmMenu" CssClass="navbar pull-right"
        DynamicMenuStyle-CssClass="dropdown-menu pull-right"
        StaticMenuStyle-CssClass="nav pull-right"
        IncludeStyleBlock="false" Orientation="Horizontal"
        StaticSelectedStyle-CssClass="active">


</asp:Menu>
                </div>
            </div>
        </div>
</div>