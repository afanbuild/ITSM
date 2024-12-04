<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrFlowCataDropListNew.ascx.cs"
    Inherits="Epower.ITSM.Web.Controls.ctrFlowCataDropListNew" %>

<script language="javascript" type="text/javascript" src="<%=sApplicationUrl %>/Js/jUtility.js"></script>

<script type="text/javascript" language="javascript">
    
    function CataSelecteChanged(obj) {        
        var val = $(obj).val();
        var text = $(obj).find('option:selected').text();                
        document.getElementById(obj.id.replace("ddlCate1", "hidCataID")).value = val;
        document.getElementById(obj.id.replace("ddlCate1", "hidCatalogName")).value = text;        
        return; 
        for (i = 0; i < obj.options.length; i++) {
            if (obj.options(i).selected) {            
                document.getElementById(obj.id.replace("ddlCate1", "hidCataID")).value = obj.options(i).value;
                document.getElementById(obj.id.replace("ddlCate1", "hidCatalogName")).value = obj.options(i).text;
                break;
            }
        }
    }

    function RadioSelecteChanged(obj) {
        var sName = obj.id.replaceAll("_", "$");
        var rbl = document.getElementsByName(sName);
        for (i = 0; i < rbl.length; i++) {
            if (rbl[i].checked) {
                document.getElementById(obj.id.replace("RadioCate1", "hidCataID")).value = rbl[i].value;
                document.getElementById(obj.id.replace("RadioCate1", "hidCatalogName")).value = rbl[i].parentElement.innerText;
                break;
            }
        }
    }

    function SelectCatalog(obj) {

        var RootId = document.getElementById(obj.id.replace("cmdPopParentCataLog", "HidRootID")).value;
        var IsPoint = document.getElementById(obj.id.replace("cmdPopParentCataLog", "HidIsPoint")).value;

        var value = window.showModalDialog('<%=sApplicationUrl %>' + "/Ajax/frmpopCatalog.aspx?RootID=" + RootId + "&IsPoint=" + IsPoint);
        if (value != null) {
            if (value.length > 1) {
                arr = value.split("@");
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "txtCatalog")).value = arr[1];
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCatalogName")).value = arr[1];
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCataID")).value = arr[0];
            }
            else {
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "txtCatalog")).value = "";
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCatalogName")).value = "";
                document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCataID")).value = 0;
            }
        }
        else {
            document.getElementById(obj.id.replace("cmdPopParentCataLog", "txtCatalog")).value = "";
            document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCatalogName")).value = "";
            document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidCataID")).value = 0;
        }
        //txtbox ¼ÇÔØ½Å±¾
        if (document.getElementById(obj.id.replace("cmdPopParentCataLog", "hidscript")).value.length > 0)
            document.getElementById(obj.id.replace("cmdPopParentCataLog", "txtCatalog")).onchange();

    }
</script>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            <asp:Label ID="labCata1" runat="server" Visible="False"></asp:Label><asp:DropDownList
                ID="ddlCate1" runat="server" Width="152px">
            </asp:DropDownList>
            <asp:RadioButtonList ID="RadioCate1" runat="server" RepeatColumns="4">
            </asp:RadioButtonList>
            <asp:TextBox ID="txtCatalog" runat="server" MaxLength="50" Visible="false" ReadOnly="true"></asp:TextBox><input
                id="cmdPopParentCataLog" runat="server" name="cmdPopParentCataLog" onclick="SelectCatalog(this)"
                type="button" value="..." class="btnClass2" visible="false" /><asp:Label ID="lblMessage"
                    runat="server" Visible="False"></asp:Label><asp:Label ID="rWarning" runat="server"
                        Font-Bold="False" Font-Size="Small" Style="margin-left:7px;" ForeColor="Red">*</asp:Label>
        </td>
    </tr>
</table>
<input id="hidCataID" runat="server" name="hidCataID" type="hidden" />
<input id="hidCatalogName" runat="server" name="hidCatalogName" type="hidden" />
<input id="HidRootID" runat="server" type="hidden" value="0" />
<input id="HidIsPoint" runat="server" type="hidden" value="0" />
<input id="hidscript" runat="server" type="hidden" value="" />
