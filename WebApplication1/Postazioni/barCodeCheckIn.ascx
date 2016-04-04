<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="barCodeCheckIn.ascx.cs" Inherits="KIS.Postazioni.barCodeCheckIn" %>

<script language="javascript">
    
</script>

<asp:Label runat="server" ID="lbl1" />&nbsp;
<asp:Label runat="server" ID="lbl2" />
<br />
<asp:TextBox runat="server" ID="box1" OnTextChanged="box1_TextChanged" AutoPostBack="true" />
<asp:TextBox runat="server" ID="box2" OnTextChanged="box2_TextChanged" AutoPostBack="true" />
<asp:Label runat="server" ID="log" />