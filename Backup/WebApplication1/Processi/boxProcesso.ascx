<%@ Control className="boxProcesso" Language="C#" AutoEventWireup="true" CodeBehind="boxProcesso.ascx.cs" Inherits="MyUserControls.boxProcesso" %>




<table runat="server" id="tblRelation">
<tr><td style="vertical-align:middle; margin:0px; padding: 0px;" align="center">
<asp:image runat="server" ID="imgRel" width="80px" alt="rapportoPrec" /><br />
<asp:ImageButton runat="server" ID="imgEdit" Width="30px" ImageUrl ="/img/edit.png" onClick="showRelationList" />
<asp:DropDownList runat="server" ID="ddlRelations" Visible="false" /><br />
<asp:ImageButton runat="server" ID="imgSaveRel" OnClick="saveRelation" ImageUrl="/img/iconSave.jpg" Visible="false" Width="30px"  />&nbsp;
<asp:ImageButton runat="server" ID="imgCanModRel" OnClick="cancelModRelation" ImageUrl="/img/iconCancel.jpg" Visible="false" Width="30px" />
</td></tr>
</table>

<table runat="server" id="tblBoxProc" class="boxProcesso">
<tr>
<td style="vertical-align: top;height:160px;">
<asp:Label style="font-size: 14px; text-align:center; font-weight:bold" runat="server" ID="ProcN" />
<asp:Label runat="server" ID="lbl2"/>
<br />
<asp:Label runat="server" ID="lstKPIS" />
</td>
</tr>
<tr>
<td align="center">
<asp:ImageButton runat="server" ID="imgMoveLeft" OnClick="imgMoveLeft_Click" ImageUrl="/img/iconArrowLeft.png" width="25px" />
<asp:ImageButton runat="server" ID="imgDelete" onClick="imgDelete_Click" ImageUrl="/img/iconDelete.png" width="30px"/>
<asp:ImageButton runat="server" ID="imgMoveRight" OnClick="imgMoveRight_Click" ImageUrl="/img/iconArrowRight.png" width="25px" />
</td>
</tr>
</table>