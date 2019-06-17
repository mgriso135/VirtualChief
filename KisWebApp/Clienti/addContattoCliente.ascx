<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addContattoCliente.ascx.cs" Inherits="KIS.Clienti.addContattoCliente" %>
<script>

    $(document).ready(function () {
        $('#<%=rowUsername.ClientID%>').fadeOut();
        $('#<%=rowPassword.ClientID%>').fadeOut();
        $('#<%=rowPassword2.ClientID%>').fadeOut();
        $('#<%=rowEmail.ClientID%>').fadeOut();
        $('#<%=tblAddContatto.ClientID%>').fadeOut();

        function isEmail(email) {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return regex.test(email);
        }

        $('#chkContactCreateUser').click(function () {
            if ($('#chkContactCreateUser').is(":checked"))
            {
                $('#<%=rowUsername.ClientID%>').fadeIn();
                $('#<%=rowPassword.ClientID%>').fadeIn();
                $('#<%=rowPassword2.ClientID%>').fadeIn();
                $('#<%=rowEmail.ClientID%>').fadeIn();
            }
            else
                {
                $('#<%=rowUsername.ClientID%>').fadeOut();
                $('#<%=rowPassword.ClientID%>').fadeOut();
                $('#<%=rowPassword2.ClientID%>').fadeOut();
                $('#<%=rowEmail.ClientID%>').fadeOut();
                $('#txtUsername').val('');
                $('#txtPassword').val('');
                $('#txtPassword2').val('');
                $('#txtEmail').val('');
            }
        });

        $('#btnUndo').click(function () {
            $('#<%=txtFirstName.ClientID%>').val('');
            $('#<%=txtLastName.ClientID%>').val('');
            $('#<%=txtRuolo.ClientID%>').val('');
            $('#txtUsername').val('');
            $('#txtPassword').val('');
            $('#txtPassword2').val('');
            $('#txtEmail').val('');
        });

        $('#btnSave').click(function () {
            $('#btnSave').fadeOut();
            var Customer = $('#<%=customerID.ClientID%>').val();
            var Language = $('#<%=language.ClientID%>').val();
            var FirstName = $('#<%=txtFirstName.ClientID%>').val();
            var LastName = $('#<%=txtLastName.ClientID%>').val();
            var Role = $('#<%=txtRuolo.ClientID%>').val();
            var Username = $('#txtUsername').val();
            var Password = $('#txtPassword').val();
            var Password2 = $('#txtPassword2').val();
            var Email = $('#txtEmail').val();

            var checkCustomer = false;
            if(Customer.length > 0)
            {
                checkCustomer = true;
            }
            else
            {
                alert($('#<%= lblErrCustomer.ClientID %>').val());
            }

            var checkLang = false;
            if (Language != null && Language.length > 0)
            {
                checkLang = true;
            }
            else
            {
                alert($('#<%=lblErrLanguage.ClientID %>').val());
            }

            var checkFirstName = false;
            if (FirstName != null && FirstName.length > 0)
            {
                checkFirstName = true;
            }
            else
            {
                alert($('#<%=lblErrFirstName.ClientID%>').val());
            }

            var checkLastName = false;
            if (LastName != null && LastName.length > 0)
            {
                checkLastName = true;
            }
            else
            {
                alert($("#<%=lblErrLastName.ClientID %>").val());
            }

            if(checkCustomer && checkLang && checkFirstName && checkLastName)
                {
            if ($('#chkContactCreateUser').is(":checked"))
            {
                var checkUsr = false;
                if (Username != null && Username.length > 0)
                {
                    checkUsr = true;
                }
                else
                {
                    alert($("#<%=lblErrUsername.ClientID %>").val());
                }

                var checkPwd = false;
                if (Password != null && Password2 != null && Password.length >= 6 && Password == Password2) {
                    checkPwd = true;
                }
                else { alert($("#<%=lblErrPassword.ClientID %>").val()); }

                var checkEmail = false;
                if (Email != null && Email.length > 2 && isEmail(Email))
                {
                    checkEmail = true;
                }
                else
                {
                    alert($("#<%=lblErrEmail.ClientID %>").val());
                }

                if (checkUsr && checkPwd && checkEmail)
                    {
                $.ajax({
                    //url: '/Quality/NonCompliances/NCProductDel',
                    url: '../Customers/Customer/addCustomerContact',
                    type: 'POST',
                data:{
                    customer: Customer, 
                    FirstName: FirstName, 
                    LastName:LastName, 
                    Role: Role,
                    createUser: true,
                    username: Username,
                    password: Password,
                    password2: Password2,
                    language: Language,
                    mailAddress: Email
                },
                success: function (data) {
                    /*Returns:
         * 0 if all is ok
         * 2 if customer does not exist
         * 3 if mail is incorrect, but contact has been correctly added
         * 4 if user already exists
         * 5 if Group CustomerUser is not found
         */
                    if (data == "0") {
                        $('#<%=tblAddContatto.ClientID%>').fadeOut();
                        $('#<%=txtFirstName.ClientID%>').val('');
            $('#<%=txtLastName.ClientID%>').val('');
            $('#<%=txtRuolo.ClientID%>').val('');
            $('#txtUsername').val('');
            $('#txtPassword').val('');
            $('#txtPassword2').val('');
            $('#txtEmail').val('');
            $('#chkContactCreateUser').prop("checked", false);
                        $('#<%=rowUsername.ClientID%>').fadeOut();
                $('#<%=rowPassword.ClientID%>').fadeOut();
                $('#<%=rowPassword2.ClientID%>').fadeOut();
                $('#<%=rowEmail.ClientID%>').fadeOut();
                $('#txtUsername').val('');
                $('#txtPassword').val('');
                $('#txtPassword2').val('');
                $('#txtEmail').val('');
                window.location.href = window.location.href;
                    } else if (data == 2) {
                        alert($("#<%=lblErrCustomer.ClientID %>").val());
                    } else if (data == 3) {
                        alert($("#<%=lblErrEmail.ClientID %>").val());
                    }
                    else if (data == 4) {
                        alert($("#<%=lblErrUniqueUsername.ClientID %>").val());
                    }
                    else if (data == 5) {
                        alert($("#<%=lblGroupNotFound.ClientID %>").val());
                    }
                    $('#btnSave').fadeIn();
                },
                statusCode : {
                    404: function (content) { $('#btnSave').fadeIn(); $(this).prop('disabled', false); alert('Error 404 ' + content); $(this).show(); },
                    500: function (content) { $('#btnSave').fadeIn(); $(this).prop('disabled', false); alert('Error 500 ' + content); $(this).show(); }
                },
                error: function (req, status, errorObj) {
                    $('#btnSave').fadeIn();
                    alert($("#<%=lblGenericError.ClientID %>").val());
                }
                });
                }
                else
                {
                    $('#btnSave').fadeIn();
                }

            }
            else
            {
                $.ajax({
                    //url: '/Quality/NonCompliances/NCProductDel',
                    url: '../Customers/Customer/addCustomerContact',
                    type: 'POST',
                data:{
                    customer: Customer, 
                    FirstName: FirstName, 
                    LastName:LastName, 
                    Role: Role,
                    createUser: false,
                    username: '',
                    password: '',
                    password2: '',
                    language: '',
                    mailAddress: ''
                },
                success: function (data) {
                    /*Returns:
         * 0 if all is ok
         * 2 if customer does not exist
         * 3 if mail is incorrect, but contact has been correctly added
         * 4 if user already exists
         * 5 if Group CustomerUser is not found
         */
                    if (data == "0") {
                        $('#<%=tblAddContatto.ClientID%>').fadeOut();
                        $('#<%=txtFirstName.ClientID%>').val('');
            $('#<%=txtLastName.ClientID%>').val('');
            $('#<%=txtRuolo.ClientID%>').val('');
            $('#txtUsername').val('');
            $('#txtPassword').val('');
            $('#txtPassword2').val('');
            $('#txtEmail').val('');
            $('#chkContactCreateUser').prop("checked", false);
                        $('#<%=rowUsername.ClientID%>').fadeOut();
                $('#<%=rowPassword.ClientID%>').fadeOut();
                $('#<%=rowPassword2.ClientID%>').fadeOut();
                $('#<%=rowEmail.ClientID%>').fadeOut();
                $('#txtUsername').val('');
                $('#txtPassword').val('');
                $('#txtPassword2').val('');
                $('#txtEmail').val('');
                window.location.href = window.location.href;
                    } else if (data == 2) {
                        alert($("#<%=lblErrCustomer.ClientID %>").val());
                    } else if (data == 3) {
                        alert($("#<%=lblErrEmail.ClientID %>").val());
                    }
                    else if (data == 4) {
                        alert($("#<%=lblErrUniqueUsername.ClientID %>").val());
                    }
                    else if (data == 5) {
                        alert($("#<%=lblGroupNotFound.ClientID %>").val());
                    }

                    $('#btnSave').fadeIn();
                },
                statusCode : {
                    404: function (content) { $('#btnSave').fadeIn(); $(this).prop('disabled', false); alert('Error 404 ' + content); $(this).show(); },
                    500: function (content) { $('#btnSave').fadeIn(); $(this).prop('disabled', false); alert('Error 500 ' + content); $(this).show(); }
                },
                error: function (req, status, errorObj) {
                    $('#btnSave').fadeIn();
                    alert($("#<%=lblGenericError.ClientID %>").val());
                }
                });
            }
            }
            else
            {
                $('#btnSave').fadeIn();
            }
        });


        $('#<%=imgViewTableAdd.ClientID%>').click(function () {
            if ($('#<%=tblAddContatto.ClientID%>').is(":visible")) {
                $('#<%=tblAddContatto.ClientID%>').fadeOut();
            }
            else {
                $('#<%=tblAddContatto.ClientID%>').fadeIn();
            }
        });

    });
</script>

<asp:HiddenField runat="server" ID="customerID" />
<asp:HiddenField runat="server" ID="language" />
<asp:HiddenField runat="server" ID="lblErrCustomer" />
<asp:HiddenField runat="server" ID="lblErrLanguage" />
<asp:HiddenField runat="server" ID="lblErrFirstName" />
<asp:HiddenField runat="server" ID="lblErrLastName" />
<asp:HiddenField runat="server" ID="lblErrUsername" />
<asp:HiddenField runat="server" ID="lblErrPassword" />
<asp:HiddenField runat="server" ID="lblErrUniqueUsername" />
<asp:HiddenField runat="server" ID="lblGroupNotFound" />
<asp:HiddenField runat="server" ID="lblGenericError" />
<asp:HiddenField runat="server" ID="lblErrEmail" />

<asp:Label runat="server" ID="lbl1" />

<img runat="server" ID="imgViewTableAdd" src="~/img/iconAdd2.png" style="cursor: pointer; min-width:20px; max-width:40px;" ToolTip="<%$Resources:lblTTBtnAdd %>" />
<table runat="server" ID="tblAddContatto" class="table table-striped table-condensed">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblNominativo" meta:resourcekey="lblNominativo" /></td>
        <td>
            <asp:TextBox runat="server" ID="txtFirstName" ValidationGroup="contatto" /><asp:RequiredFieldValidator runat="server" ID="valFirstName" ControlToValidate="txtFirstName" ForeColor="Red" ValidationGroup="contatto" ErrorMessage="<%$Resources:lblValReqField %>" /></td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblLastName" meta:resourcekey="lblLastName" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtLastName" ValidationGroup="contatto" /><asp:RequiredFieldValidator runat="server" ID="valLastName" ControlToValidate="txtLastName" ForeColor="Red" ValidationGroup="contatto" ErrorMessage="<%$Resources:lblValReqField %>" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblRuolo" meta:resourcekey="lblRuolo" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtRuolo" ValidationGroup="contatto" />
        </td>
    </tr>
    <tr>
        <td><asp:literal runat="server" ID="lblCreateUser" Text="<%$Resources:lblCreateUser %>" /></td>
        <td><input type="checkbox" id="chkContactCreateUser" /></td>
    </tr>
    <tr id="rowUsername">
        <td><asp:literal runat="server" ID="Literal1" Text="<%$Resources:lblUsername %>" /></td>
        <td><input type="text" id="txtUsername" /></td>
    </tr>
    <tr id="rowPassword">
        <td><asp:literal runat="server" ID="Literal2" Text="<%$Resources:lblPassword %>" /></td>
        <td><input type="password" id="txtPassword" /></td>
    </tr>
    <tr id="rowPassword2">
        <td><asp:literal runat="server" ID="Literal3" Text="<%$Resources:lblPassword2 %>" /></td>
        <td><input type="password" id="txtPassword2" /></td>
    </tr>
    <tr id="rowEmail">
        <td><asp:literal runat="server" ID="Literal4" Text="<%$Resources:lblEmail %>" /></td>
        <td><input type="email" id="txtEmail" /></td>
</tr>
    <tr>
        <td Colspan="2" style="text-align:center;">
            <img ID="btnSave" src="../img/iconSave.jpg" style="cursor:pointer; min-width:20px; max-width:40px;" />
            <img src="../img/iconUndo.png" ID="btnUndo" style="cursor:pointer; min-width:20px; max-width:40px;" />
        </td>
    </tr>
</table>