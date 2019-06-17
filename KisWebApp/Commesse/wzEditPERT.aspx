<%@ Page Title="Virtual Chief" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wzEditPERT.aspx.cs" Inherits="KIS.Commesse.wzEditPERT" %>

<%@ Register TagPrefix="pert" TagName="editP" Src="~/Commesse/wzEditPERT.ascx" %>
<%@ Register TagPrefix="variante" TagName="editData" Src="~/Commesse/wzVarianteDettagli.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1><asp:Label runat="server" ID="lblTitleNewOrder" Text="<%$Resources:lblTitleNewOrder %>" /></h1>
    <ul class="breadcrumb hidden-phone">
		<li>
		    <a href="wzAddCommessa.aspx"><asp:Label runat="server" ID="lblNewOrder" Text="<%$Resources:lblNewOrder %>" /></a>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink NavigateUrl="wzAddPERT.aspx" runat="server" id="lnkAddPert">
                <asp:Label runat="server" ID="lblDescProd" Text="<%$Resources:lblDescProd %>" /></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
        <li>
		    <asp:hyperlink runat="server" ID="lnkEditPert"><b style="font-size: 14px;">
                <asp:Label runat="server" ID="lblDescProcess" Text="<%$Resources:lblDescProcess %>" /></b></asp:hyperlink>
			<span class="divider">/</span>
	    </li>
	</ul>
    <script>
        $(document).ready(function () {
            function checkCTIntegrity()
            {
                $.ajax({
                    url: "../Products/Products/CheckProductIntegrityCycleTimes",
                    type: 'GET',
                dataType: 'html',
                data:{
                    processID: $('#<%=ProcessID.ClientID%>').val(),
                    processRev: $('#<%=ProcessRev.ClientID%>').val(),
                    variantID: $('#<%=VariantID.ClientID%>').val(),
                    },
                    success: function (result) {

                        if (result == "1" || result =="6")
                            {
                            $('#<%=lnkGoFwd.ClientID%>').fadeIn();
                            
                        }
                        else
                        {
                            $('#<%=lnkGoFwd.ClientID%>').fadeOut();
                        }
                },
                error: function (result) {
                    //alert("Error checkCTIntegrity");
                    $('#<%=lnkGoFwd.ClientID%>').fadeOut();
                },
                warning: function (result) {
                    //alert("Warning");
                    $('#<%=lnkGoFwd.ClientID%>').fadeOut();
                }
            });
            }

            $('#<%=lnkGoFwd.ClientID%>').fadeOut();
            checkCTIntegrity();
            myVar = setInterval(checkCTIntegrity, 10000);
        });
        </script>
    <asp:HiddenField runat="server" ID="ProcessID" />
    <asp:HiddenField runat="server" ID="ProcessRev" />
    <asp:HiddenField runat="server" ID="VariantID" />
    <asp:Label runat="server" ID="lbl1" />
    <div class="row-fluid">
        <div class="span1">
            <asp:Hyperlink runat="server" ID="LinkBCK">
            <asp:Image runat="server" ID="lnkGoBack" ImageUrl="~/img/iconArrowLeft.png" ToolTip="<%$Resources:lblTTGoBack %>" Height="40" />
                </asp:Hyperlink>
        </div>
        
        <div class="span9"><variante:editData runat="server" ID="frmEditDatiVariante" /></div>
        <div class="span1">
            <asp:HyperLink runat="server" ID="lnkSwitchToGrid">
            <asp:Image runat="server" ID="imgSwitchToGrid" ImageUrl="~/img/iconGrid.jpg" Height="50" ToolTip="<%$Resources:lblTTGoToTablePERT %>" />
                </asp:HyperLink>
        </div>
        <div class="span1" style="align-content:center;">
            <asp:Hyperlink runat="server" ID="LinkFWD">
            <asp:Image runat="server" ID="lnkGoFwd" ImageUrl="~/img/iconArrowRight.png" ToolTip="<%$Resources:lblTTGoFwd %>" Height="40" />
                </asp:Hyperlink>
        </div>
        
        </div>
    <pert:editP runat="server" id="frmEditPERT" />
</asp:Content>
