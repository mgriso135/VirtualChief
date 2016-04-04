<%@ Control className="newProcessBox" Language="C#" AutoEventWireup="true" CodeBehind="CreateSubProcess.ascx.cs" Inherits="MyUserControls.newProcessBox" %>



<table style="float:left;position:relative;height:200px;margin:0px;padding:0px;left:60px; border: 2px dotted grey; width: 200px" runat="server" id="tblNewProc">

	<tr>

		<td style="vertical-align:middle; margin:0px; padding: 0px; text-align:center;">

<asp:ImageButton runat="server" ImageUrl="/img/iconAdd.jpg" id="addSubProc" onClick="addSubProc_Click" Height="100px" OnClientClick="this.style.display='hidden';" ToolTip="Aggiungi un box allo stream di processi" />
<asp:Label runat="server" ID="lbl1" />
</td>

	</tr>

</table>
