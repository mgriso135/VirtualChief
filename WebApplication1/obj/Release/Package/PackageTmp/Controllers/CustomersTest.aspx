<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomersTest.aspx.cs" Inherits="KIS.Controllers.CustomersTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <!-- Get customer list -->
        Get customer list<br />
        <asp:Button runat="server" ID="btnCustomerList" OnClick="btnCustomerList_Click" Text="Get customer list" />
        <br />
        <asp:Repeater runat="server" ID="rptCustomerList">
            <HeaderTemplate>
                <table>
                    <thead>
                        <th>codice</th>
                        <th>ragsociale</th>
                        <th>partitaiva</th>
                        <th>codfiscale</th>
                        <th>indirizzo</th>
                        <th>citta</th>
                        <th>provincia</th>
                        <th>CAP</th>
                        <th>stato</th>
                        <th>telefono</th>
                        <th>email</th>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#DataBinder.Eval(Container.DataItem, "codiceCliente")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "ragioneSociale")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "partitaiva")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "codiceFiscale")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "indirizzo")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "citta")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "provincia")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "CAP")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "stato")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "telefono")%></td>
                    <td><%#DataBinder.Eval(Container.DataItem, "email")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <hr />
        <!-- Get single customer data -->
        Get single customer data<br />
        <asp:TextBox runat="server" ID="txtCustomerID" /><asp:Button runat="server" ID="btnGetCustomer" Text="Get Customer" OnClick="btnGetCustomer_Click" />
        <table runat="server" id="tblCustomerData">
            <tr><td><asp:Label runat="server" ID="lblcodiceCliente" /></td></tr>
            <tr><td><asp:Label runat="server" ID="lblragioneSociale"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblpartitaiva"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblcodiceFiscale"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblindirizzo" /></td></tr>
            <tr><td><asp:Label runat="server" ID="lblcitta" /></td></tr>
            <tr><td><asp:Label runat="server" ID="lblprovincia"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblCAP"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblstato"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lbltelefono"/></td></tr>
            <tr><td><asp:Label runat="server" ID="lblemail"/></td>
            </tr>
        </table>
        <hr />
        <!-- Add new customer or modify existing customer -->
        Add new customer or modify existing customer<br />
        <table runat="server" id="tblAddCustomer">
            <tr><td>codiceCliente</td><td><asp:TextBox runat="server" ID="txtcodiceCliente" /></td></tr>
            <tr><td>ragioneSociale</td><td><asp:TextBox runat="server" ID="txtragioneSociale" /></td></tr>
            <tr><td>partitaiva</td><td><asp:TextBox runat="server" ID="txtpartitaiva" /></td></tr>
            <tr><td>codicefiscale</td><td><asp:TextBox runat="server" ID="txtcodicefiscale" /></td></tr>
            <tr><td>indirizzo</td><td><asp:TextBox runat="server" ID="txtindirizzo" /></td></tr>
            <tr><td>citta</td><td><asp:TextBox runat="server" ID="txtcitta" /></td></tr>
            <tr><td>provincia</td><td><asp:TextBox runat="server" ID="txtprovincia" /></td></tr>
            <tr><td>CAP</td><td><asp:TextBox runat="server" ID="txtcap" /></td></tr>
            <tr><td>stato</td><td><asp:TextBox runat="server" ID="txtstato" /></td></tr>
            <tr><td>telefono</td><td><asp:TextBox runat="server" ID="txttelefono" /></td></tr>
            <tr><td>email</td><td><asp:TextBox runat="server" ID="txtemail" /></td>
            </tr>
        </table>
        <asp:button runat="server" ID="btnAddCustomer" OnClick="btnAddCustomer_Click" Text="Add customer" />
        <asp:button runat="server" ID="btnModifyCustomer" OnClick="btnModifyCustomer_Click" Text="Modify existing customer" />
        <hr />
        <asp:TextBox runat="server" ID="txtCodClienteDelete" /><asp:Button runat="server" ID="btnDeleteCliente" Text="Delete existing customer" OnClick="btnDeleteCliente_Click" />
        <hr />

                <asp:Label runat="server" ID="lbl1" />
    </div>
    </form>
</body>
</html>
