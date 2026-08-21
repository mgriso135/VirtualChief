<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="menuBar.ascx.cs" Inherits="KIS.menuBarMVC" %>
<form runat="server">
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

    <div class="container">
    <div class="navbar navbar-default">        
        <div class="navbar-header">
         <!-- .btn-navbar is used as the toggle for collapsed navbar content -->
            <button type="button" class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
							</button>
        <!-- Everything you want hidden at 940px or less, place within here -->
            <div class="nav-collapse">

    <asp:Menu runat="server" ID="frmMenu"
        DynamicMenuStyle-CssClass="dropdown-menu"
        StaticMenuStyle-CssClass="nav"
        IncludeStyleBlock="false" Orientation="Horizontal"
        StaticSelectedStyle-CssClass="active">


</asp:Menu>
                </div>
            </div>
        </div>
</div>
    </form>