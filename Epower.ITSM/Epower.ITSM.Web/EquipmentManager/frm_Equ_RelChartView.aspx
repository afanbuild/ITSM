<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_Equ_RelChartView.aspx.cs"
    Inherits="Epower.ITSM.Web.EquipmentManager.frm_Equ_RelChartView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="title">资产关联图</title>
    <!--Begin: 右键菜单样式表 -->
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <!--End: 右键菜单样式表 -->
    <link href="../css/epower.equ.tabs.css" type="text/css" rel="Stylesheet" />
</head>
<body>


    <script language="javascript" type="text/javascript" src="../Js/jquery-1.7.2.min.js"> </script>    
       
    <script language="javascript" type="text/javascript" src="../Js/jquery.json-2.3.js"> </script>
    
    <script language="javascript" type="text/javascript" src="../Controls/Calendar/Popup.js"></script>

    <script language="javascript" type="text/javascript" src="../Js/App_Common.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/App_Base.js"> </script>

    <script language="javascript" type="text/javascript" src="../Js/jquery-ui-1.8.20.custom.min.js"> </script>

    <script src="../Js/jquery.tooltip.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Js/Jquery.Query.js"></script>

    <script type="text/javascript">
	    if (!$.browser.msie) {
	        window.location.href = "frm_Equ_RelChartView_SVG.aspx?id=" + $.query.get('id');
	    }
    </script>

    <script language="javascript" type="text/javascript">
    $.ajaxSetup({ cache: false });

 var xmlhttpGetShot;  //客户端XML对象
 var blnHasShow = false;
 function CreateDroplstXmlHttpObject()
    {
		try  
		{  
			xmlhttpDroplst = new ActiveXObject("MSXML2.XMLHTTP");  
		}  
		catch(e)  
		{  
			try  
			{  
				xmlhttpDroplst = new ActiveXObject("Microsoft.XMLHTTP");  
			}  
			catch(e2){}  
		}
		return xmlhttpDroplst;
    }

 function absoluteLocation(element, offset) 
    { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
    } 
    
function hideMe(id,status)
    {
        
        var object = document.getElementById(id);
        
        if(object != null)
        {
            object.style.display = status;
            
            if(status == "none")
              blnHasShow = false;
        }
        //alert(object.style.display);
    }


function LookEquDetail(obj) {
    if(document.all.<%=hidValue.ClientID%>.value != "1")
    {
        var idFields = obj.id.substring(5);
        idFields = idFields.split("-")[0];     
         window.open("frmEqu_DeskEdit.aspx?newWin=true&ShowDetail=true&id=" + idFields + "&FlowID=-1","_blank","scrollbars=yes,status=yes ,resizable=yes,width=680,height=480");
     }
     document.all.<%=hidValue.ClientID%>.value = "";
}

function GetEquShot(obj)
{

    var idFields = obj.id.substring(5);
    idFields = idFields.split("-")[0];     
      if(blnHasShow == true)
      {
          return;
      }
      
      blnHasShow = true;
      
       var pars = "id="+idFields;
            $.ajax({
                    type: "get",
                    data:pars,
                    async:false,
                    url: "frmEqu_DeskShot.aspx",
                    success: function(data, textStatus){
    					 var object = document.getElementById("divShowEquShot");
        
                         if(object != null)
                         {
                             object.style.display = status;
                         }
                         object.innerHTML = data;
                         if ($('#IMG_HIDDEN_' + idFields).size() > 0) {
                             $(object).find('#tabRender').prepend('<tr><td colspan="4"><div style="float:left;">资产状态 ==></div> <div style="float:left;"><img src="../Images/x.gif"/></div></td></tr>');
                         }
												       
						 if(absoluteLocation(obj, 'offsetLeft') >=300 && absoluteLocation(obj, 'offsetLeft') <=700 )
						 {
							object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth / 2 + "px";       
						 }
						 if(absoluteLocation(obj, 'offsetLeft') <300 )
						 {
							object.style.left = absoluteLocation(obj, 'offsetLeft') + 2 + "px";       
						 }
						 if(absoluteLocation(obj, 'offsetLeft') >700 )
						 {
							 object.style.left = absoluteLocation(obj, 'offsetLeft') - object.offsetWidth - 2 + "px";       
						 }
												         
						 if(absoluteLocation(obj, 'offsetTop') >350)
                         {
						    object.style.top = object.style.top = absoluteLocation(obj, 'offsetTop') - object.offsetHeight + "px"; 
												          
						 }
						 else
						 {
							 object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
						 }                                                               
		            }
                 });
}


String.prototype.trim = function()  //去空格

			{
				// 用正则表达式将前后空格

				// 用空字符串替代。

				return this.replace(/(^\s*)|(\s*$)/g, "");
			}
    </script>

    <style>
        #tooltip
        {
            position: absolute;
            z-index: 3000;
            border: 1px solid #111;
            background-color: #eee;
            padding: 5px;
            opacity: 0.85;
        }
        #tooltip h3, #tooltip div
        {
            margin: 0;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function GetEquShot123(obj) {

            var idFields = obj.id.substring(5);
            idFields = idFields.split("-")[0];        
            $("#" + obj.id).tooltip({ showURL: false, bodyHandler: function() { return $.ajax({ type: "GET", async: false, url: "frmEqu_DeskShot.aspx?id=" + idFields }).responseText; } });
        }
        
        var xmlhttp=null;
        
        function window.onunload()      
        {
           if(window.opener != undefined && window.opener != null)       
                window.opener.location.reload();
        }
    </script>

    <form id="form1" runat="server">
    <div style='overflow-y: auto; overflow-x: auto; width: 100%; height: 100%;'>
        <div id="div-tabs-container">
            <div id="div-tabs-head">
                <img src="../Images/lm-left.gif" width="7" height="29" />
            </div>
            <div class="div-tabs-content switchTab div-tabs-opend div-tabs-opend2">
                <span id="default" class="div-tabs-opend2">默认视角</span>
            </div>
            <asp:Literal ID="literalRelKeyList" runat="server">
            <div class="div-tabs-content" style="background-image: url(../Images/lm-a.gif)">           
                <span id="{0}" class="STYLE4">{1}</span>
            </div>
            </asp:Literal>
            <div class="div-tabs-content STYLE4 switchTab" style="background-image: url(../Images/lm-a.gif)">
                <span id="prefersetting" class="STYLE4">偏好设置</span>
            </div>
            <div id="div-tabs-foot">
                <img src="../Images/lm-right.gif" width="7" height="29" />
            </div>
        </div>
        <asp:Literal ID="graph" runat="server"></asp:Literal>
    </div>
    <div style="display: block; clear: both;">
        <asp:DataGrid ID="dgPrefer" runat="server" Visible="true" CssClass="table-layout"
            OnItemDataBound="dgPrefer_ItemDataBound">
            <Columns>
                <asp:BoundColumn DataField="ID" HeaderText="视角编号"></asp:BoundColumn>
                <asp:BoundColumn DataField="RelKey" HeaderText="视角名称"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="选择">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkprefer" runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Button ID="btnSaveChanges" Text="保存设置" runat="server" OnClick="btnSaveChanges_Click"
            Visible="false" />
    </div>
    <input id="hidValue" runat="server" type="hidden" value="" />
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    <div id="alert-float-panel-tpl" style="position: relative; display: none; background-color: white;
        border: 1px solid gray;">
        <table style="width: 330px;">
            <tr>
                <td style="width: 70px;">
                </td>
                <td colspan="4" align="right">
                    <strong style="cursor:pointer;" onclick="$('#alert-float-panel-tpl-1').remove();">X</strong>
                </td>
            </tr>        
            <tr style="display:none;" class="alert-float-panel-tpl-head">
                <td style="width: 70px;">
                    规则1
                </td>
                <td>
                    <img src="../images/resource_state_error.gif" />
                </td>
                <td colspan="3">
                </td>
            </tr>
            <tr style="display:none;" class="alert-float-panel-tpl-item">
                <td style="width:70px;" class="alert-float-panel-tpl-item-key">
                    CPU 负载
                </td>
                <td style="width:20px;">
                    <
                </td>
                <td style="width:30px;" class="alert-float-panel-tpl-item-preset">
                    22%
                </td>
                <td style="width:40px;">
                    当前值
                </td>
                <td style="width:30px;">
                    15%
                </td>
                <td style="width:40px;">
                    参考值
                </td>
                <td style="width:80px;" class="alert-float-panel-tpl-item-normalvalue">
                    35%
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<!--Begin: 基础 Javascript 脚本引用-->

<script language="javascript" type="text/javascript" src="../js/epower.base.js"></script>

<!--End: 基础 Javascript 脚本引用-->
<!--Begin: 页面参数传递到 Javascript 文件-->

<script type="text/javascript">                
    epower.equ.read_only = <%=ReadOnly %>;     
    epower.equ.resource_state_interval = <%=Interval %>;
</script>

<!--End: 页面参数传递到 Javascript 文件-->
<!--Begin: 右键菜单 Javascript 脚本-->

<script language="javascript" type="text/javascript" src="../js/jquery.contextMenu2.js"></script>

<script language="javascript" type="text/javascript" src="../Js/epower.equ.contextmenu.js"> </script>

<!--End: 右键菜单 Javascript 脚本-->
<!--Begin: 页面自定义脚本-->



<script language="javascript" type="text/javascript" src="../Js/epower.equ.frm_Equ_RelChartView.js"> </script>

<!--End: 页面自定义脚本-->
