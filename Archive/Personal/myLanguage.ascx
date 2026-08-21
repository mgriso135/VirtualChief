<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="myLanguage.ascx.cs" Inherits="KIS.Personal.myLanguage" %>

<asp:UpdatePanel runat="server" ID="upd1">
    <ContentTemplate>
        <script>
            $(document).ready(function () {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    $("#<%=lbl1.ClientID%>").delay(3000).fadeOut("slow");
                });
            });
</script>
<div class="row-fluid">
<div class="span12">
    <asp:DropDownList runat="server" ID="ddlLanguages" AppendDataBoundItems="true">
        <asp:ListItem Value="">
        </asp:ListItem>
        <asp:ListItem Value="en-US">English (United States)</asp:ListItem>
        <asp:ListItem Value="es-AR">Español (Argentina)</asp:ListItem>
        <asp:ListItem Value="it">Italiano (Italia)</asp:ListItem>
        </asp:DropDownList>
    <asp:ImageButton runat="server" ID="imgSave" ImageUrl="~/img/iconSave.jpg" Width="40" OnClick="imgSave_Click" />
    <asp:ImageButton runat="server" ID="imgUndo" ImageUrl="~/img/iconUndo.png" Width="40" OnClick="imgUndo_Click" />
    <br />
    <asp:Label runat="server" ID="lbl1" CssClass="text-info" />
    </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>