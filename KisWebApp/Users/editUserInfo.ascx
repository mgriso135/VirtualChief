<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="editUserInfo.ascx.cs" Inherits="KIS.Users.editUserInfo" %>

<script>
    $(document).ready(function () {

        $("#<%=btnDisableUser.ClientID%>").click(function () {
            var username = $("#<%= hdUsername.ClientID %>").val();

                    $("#<%=btnDisableUser.ClientID%>").fadeOut();

                        $.ajax({
                            url: "../Users/Users/DisableUser",
                            type: 'POST',
                            data: {
                                user: username
                            },
                            success: function (data) {
                                if (data == "1") {
                                    window.location.href = "../Users/manageUsers.aspx";
                                }
                                else if (data == "0") {
                                    alert("Error");
                                }
                                else if (data == "2") {
                                    alert("User not allowed");
                                }
                                $("#<%=btnDisableUser.ClientID%>").fadeIn();
                            },
                            statusCode: {
                                404: function (content) { alert('cannot find resource'); $("#<%=btnDisableUser.ClientID%>").fadeIn(); },
                                500: function (content) { alert('internal server error'); $("#<%=btnDisableUser.ClientID%>").fadeIn(); }
                            },
                            error: function (req, status, errorObj) {
                                $(this).fadeIn();
                            }
                        });

            return false;
        });
        
    });
</script>

<asp:Label runat="server" ID="lbl1" />
<h1><asp:Label runat="server" ID="lblUsername" /></h1>
<div class="row-fluid">
    <div class="span9">
<table runat="server" id="tbEdit" class="table table-hover">
    <tbody>
    <tr>
        <td><asp:Literal runat="server" ID="lblNome" Text="<%$Resources:lblNome %>" />:</td>
        <td><asp:TextBox runat="server" ID="tbNome" /></td>
    </tr>
    <tr>
        <td><asp:Literal runat="server" ID="lblCognome" Text="<%$Resources:lblCognome %>" />:</td>
        <td><asp:TextBox runat="server" ID="tbCognome" /></td>
    </tr>
        </tbody>
    <tfoot>
    <tr>
        <td colspan="2" style="text-align:center;">
        <asp:ImageButton runat="server" ID="btnSave" ImageUrl="~/img/iconSave.jpg" Height="50px" OnClick="btnSave_Click" />
        <asp:ImageButton runat="server" ID="btnUndo" ImageUrl="~/img/iconUndo.png" Height="50px" OnClick="btnUndo_Click" />
            <asp:Label runat="server" ID="lblRes" ForeColor="Red" />
        </td>
    </tr>
        </tfoot>
</table>
        </div>
    <div class="span3">
        <asp:ImageButton runat="server" ID="btnDisableUser" ImageUrl="~/img/iconDelete.png" style="min-width:30px; max-width:60px;" Tooltip="<%$Resources:lblDisableUser %>" />
        <asp:HiddenField runat="server" ID="hdUsername" />
    </div>
    </div>