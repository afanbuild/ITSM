<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" validateRequest="false" Inherits="Epower.ITSM.Web.Forms.flow_sender" Codebehind="flow_sender.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD id="HEAD1" runat="server">
		<title>处理/接收人员选择</title>
		<script src="../Js/jquery-1.3.1.js" type="text/javascript"></script>
        <script src="../Js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
         <script type="text/javascript" src="../Js/Plugin/jquery.autocomplete.js"></script>         
         

    <link rel="stylesheet" type="text/css" href="../Js/themes/jquery.autocomplete.css" />
          <style type="text/css"> 
              .divNoBorder{BORDER: #376092 0px solid}
.SelectTd {BORDER: #376092 1px solid;OVERFLOW: hidden;  }
SELECT{LEFT: -2px; top:-2px;COLOR: black;POSITION: relative;MARGIN: 2px; TEXT-DECORATION: underline }
</style>
	</HEAD>
	
    
		<script language="javascript" type="text/javascript">
	<!--
		    $(document).ready(function() {
		        
		        $("#div_lstSelect").addClass("SelectTd");
		    });

		    var xmlDoc = createXml('<%=sIniXml%>');
		   
		 
		var lngSelectID =0;

		var currTv = null;   //当前的treeview
		var selectedNodeList = [];

        //将一个xml文档格式的字符串换成xml文档
        function createXml(xmlText) {
            if (xmlText == null) {
                return null;
            }
           
            if (window.DOMParser) {
                try {                  
                    var xml_doc = (new DOMParser()).parseFromString(xmlText, "text/xml");
                    
                    if (!xml_doc.selectNodes) {
                        xml_doc = for_ie9(xmlText);
                        if (!xml_doc) {
                            return (new DOMParser()).parseFromString(xmlText, "text/xml");
                        }
                    }
                    
                    return xml_doc;
                }
                catch (e) {
                    alert("DOMParser error:" + e.message);
                    return null;
                }
            }
            else {
                try {
                    var xmldocm = new ActiveXObject("Microsoft.XMLDOM");
                    xmldocm.loadXML(xmlText);
                    return xmldocm;
                }
                catch (e) {
                    alert("DOMParser loadXML error:" + e.message);
                }  
            }
           
        }
        
        function for_ie9(xmlText) {
                try {
                    var xmldocm = new ActiveXObject("Microsoft.XMLDOM");
                    xmldocm.loadXML(xmlText);
                    return xmldocm;
                }
                catch (e) {
                    return false;
                }  
            return null;       
        }
        
		function getNodesOfActorType(nodeId, actorType) {
		    if (selectedNodeList.length == 0) {
		        return null;
		    }
		    var nodesOfActor = [];
		    for(var i =0 ;i<selectedNodeList.length ;i++)
		    {
		        if (selectedNodeList[i].NodeID == nodeId && selectedNodeList[i].ActorType == actorType) {
		            nodesOfActor.pop(selectedNodeList[i]);
		        }
		    }
		    return nodesOfActor;
		}

		function getNodesById(nodeId) {
		    if (selectedNodeList.length == 0) {
		        return null;
		    }		 
		    for (var i = 0; i < selectedNodeList.length; i++) {
		        if (selectedNodeList[i].NodeID == nodeId ) {
		            return selectedNodeList[i];
		        }
		    }
		    return null;
		}

		function getNodesByUserId(userId) {
		    if (selectedNodeList.length == 0) {
		        return null;
		    }
		    for (var i = 0; i < selectedNodeList.length; i++) {
		        if (selectedNodeList[i].UserID == userId) {
		            return selectedNodeList[i];
		        }
		    }
		    return null;
		}

		function clearAllOfSelectedNodes() {
		    if (selectedNodeList.length == 0) {
		        return ;
		    }
		    for (var i = 0; i < selectedNodeList.length; i++) {
		        selectedNodeList.pop();
		    }
		}
		
		function setClickNodeValue(nodeValue) {
		    $("#<%=hidTreeNodeValue.ClientID %>").val(nodeValue);
		    return false;
		}
		
		
		function getSenderName()
		{
		   var sSpec = document.all.SpecRightType.value;
		   var ret = "主办";
		   
		   if(sSpec == "60")
		   {
		       ret = "阅知";
		   }
		   if(sSpec == "70")
		   {
		       ret = "协作";
		   }
		   if(sSpec == "80")
		   {
		       ret = "沟通";
		   }
		   return ret;
		}
	    
		function RemoveItem()
		{
			//将界面的表现跟一个XML串关联起来，无论是添加还是删除 。。。。
			var lngSelected = document.all.lstSelected.selectedIndex;
			if(lngSelected==-1)
			{
				if(document.all.lstSelected.options.length>0)
					lngSelected=0;
				else
					return false;
			}
			
			try{
			    var UserID = document.all.lstSelected.options[lngSelected].value;
			    var UserName = document.all.lstSelected.options[lngSelected].innerText;
			}catch(e) {  }
			if(document.all.DeleteMaster.value == "0" && UserName.indexOf("主办") != -1)
			{
			    //不能删除
			}
			else {

    		    if (window.ActiveXObject) {
    		        var xmlEle = xmlDoc.documentElement.selectSingleNode("Receiver[@UserID=" + UserID + "]");    		      
    		        if (xmlEle != null)
    		            xmlDoc.documentElement.removeChild(xmlEle);
    		        document.all.lstSelected.remove(lngSelected);
    		    }
    		    else if (document.implementation && document.implementation.createDocument) {
    		        //xmlEle = xmlDoc.documentElement.selectSingleNode("Receiver[@UserID=" + UserID + "]");
    		        var path = "/Receivers/Receiver[@UserID=" + UserID.toString() + "]";
    		        var xmlEle = getOneNodeByPath(path);    		       
    		        if (xmlEle != null)
    		            xmlDoc.documentElement.removeChild(xmlEle);
    		        document.all.lstSelected.remove(lngSelected);
    		    }			  
			}
		}
		
		//清楚Listbox中的内容
		function ClearItem() {

		    xmlDoc = createXml('<%=sIniXml%>');
		    while (document.all.lstSelected.options.length > 0) {
		        document.all.lstSelected.remove(0);
		    }
		}



		//对选择列表排序
		function SortItem()
		{
			var arrayLI= new Array();
			arrayLI = document.all.lstSelected.options;
			
			var LI=new Option("","");	
			for(var i=0; i<arrayLI.length;i++)
			{
				for(var j=0; j<arrayLI.length;j++)
				{
				   // alert(arrayLI[i].text.substring(0,2));
					if(arrayLI[i].text.substring(0,2)<arrayLI[j].text.substring(0,2))
					{
						//调换位置
						LI=arrayLI[i];
						arrayLI[i]=new Option(arrayLI[j].text,arrayLI[j].value);
						arrayLI[j]=LI;
					}
				}
			}
		}



		//对选择列表排序
		function Set_ItemText_By_Value(Value,NewText)
		{
			Options = document.all.lstSelected.options;
			for(var i=0; i<Options.length;i++)
			{
				if(Options[i].value==Value)
				{
					Options[i].text=NewText;
					break;
				}
			}
		}



		function SetCurrTreeView(tv)
		{
			currTv = tv;
		}
	
	    function AddToListButton()
	    {
	        var btree=document.getElementById("ckblSelect_0").checked;
            if(!btree)   //列表
            {
                var obj;
                if(document.getElementById("tablist0").style.display=="")   //主办
                {
                    obj = document.getElementById("lstperson") ;
                    if(obj.selectedIndex!=-1)
                    {
                        AddSelectToList(obj.options[obj.selectedIndex].value);
                    }
                    else
                    {
                        alert("请选择数据！");
                        return;
                    }
                }
                else   //协办和阅知
                {
                    obj = document.getElementById("lstread") ;
                    for(i=0;i<obj.length;i++){      
                        if(obj.options[i].selected){   
                            AddSelectToList(obj.options[i].value);
                        }   
                    } 
                    if(obj.selectedIndex==-1)
                    {
                        alert("请选择数据！");
                        return;
                    }  
                }
                SortItem();//重新排序列表框  
            }
	        else
	        {
			    AddToList(currTv);  //树
			}
	    }
	    
	  
		
		var clickedNode;
		function AddToList(tv)
		{		
		    if(tv==null)
		    {
		        alert("请选择数据！");
		        return;
		    }		   		    
		    		  
			var UserID;
			var lngNodeID;
			var strName;
			var strNodeName;
						
			$(clickedNode).css('color', 'black');
			clickedNode = event.target;
			currTv = tv;			
			//treeNode = tv.getTreeNode(tv.selectedNodeIndex);
			treeNode = event.srcElement;      
			treeNode.setAttribute("expanded", true, 0);
			//treeNode.selectExpands=true;

			if (tv.id == "tvAssistor" || tv.id == "tvReader"
			        || document.all.SpecRightType.value == "60" 
			        || document.all.SpecRightType.value=="70"
			        || document.all.SpecRightType.value == "80") {
			    //QueryAllChillNode(treeNode);//有子节点则递归所有子节点
			    //去掉QueryAllChillNode，未发现有特殊需要处理的地方lizg 2012-6-7
			    AddNodeToList(treeNode); //该节点没有子节点	
			}
			else {			
				AddNodeToList(treeNode);			 
			}
			
			SortItem();//重新排序列表框  
			
			//alert(clickedNode.outerHTML);
			$(clickedNode).css('color', 'red');
					
		}
		
		
		//遍历节点下的子节点
		function QueryAllChillNode(treeNode) {
		    var NodeArray = treeNode.getChildren();		 
			if(NodeArray.length == 0)
			{
			    AddNodeToList(treeNode);//该节点没有子节点	
			}
			else
			{
                for(var i=0; i<NodeArray.length;i++)
			    {
				    if(NodeArray[i].getChildren().length==0)
				    {
					    AddNodeToList(NodeArray[i]);//该节点没有子节点	
				    }
			    }
			}
		}
		

		function getSelectedItemOfArray(queryKey, json_key) {
		   
		    if (allNodeItems == null) {
		        alert("没有Json数组。");
		        return null;
		    }
//		    if (queryKey.indexOf("tvAssis") >= 0) {
//		        return null;
//		    }
		    var _node;
		    for (var i = 0; i < allNodeItems.length; i++) {
		        _node = allNodeItems[i];
		        if (_node.userid==json_key 
		            && (_node.key.substring(0, 2) == 'tv'
		            || _node.key.substring(0,3) == 'Rea'))
		        {
		            return allNodeItems[i];
		        }
		    }
		    return null;
		}

		function getSelectedItemByName(name, notReader) {

		    if (allNodeItems == null) {
		        alert("没有Json数组。");
		        return null;
		    }
		  
		    for (var i = 0; i < allNodeItems.length; i++) {
		        var idx = allNodeItems[i].key.indexOf('Reader');
		        if (idx < 0 && notReader <= 0) {
		            if (allNodeItems[i].name == name) {
		                return allNodeItems[i];
		            }
		        } else {		            		            
		            if (idx >= 0 && allNodeItems[i].name == name) {
		                return allNodeItems[i];
		            }
		        }
		    }
		    return null;
		}
		function getNodesByActorTypeAndNodeId(lngNodeID, actorType) {
		    var path = "/Receivers/Receiver[@NodeID=" + lngNodeID.toString() + "][@ActorType='" + actorType + "']";
		    return getNodesByPath(path);

		}

		function getNodesByPath(path) {
		    //var path = "/Receivers/Receiver[@NodeID=" + lngNodeID.toString() + "][@ActorType='" + actorType + "']";
		    if (window.ActiveXObject) {		        	    
		        return xmlDoc.selectNodes(path);
		    }		    
		    else if (document.implementation && document.implementation.createDocument) {		    
		        var nodes = xmlDoc.evaluate(path, xmlDoc, null, XPathResult.ANY_TYPE, null);
		        var rtnNodes = [];
		        if (nodes != null) {
		            var oElement = nodes.iterateNext();
		            while (oElement) {
		                rtnNodes.push(oElement);
		                oElement = nodes.iterateNext();
		            }		           
		        }		        
		        return rtnNodes;
		    }
		}

		function getOneNodeByPath(path) {
		    if (window.ActiveXObject) {		        
		        return xmlDoc.selectNodes(path);
		    }
		    else if (document.implementation && document.implementation.createDocument) {
		    var xPathRes = xmlDoc.evaluate(path, xmlDoc, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);

		    return xPathRes.singleNodeValue
		    
		    }
		}
		
		var __old_node;
		function AddNodeToList(treeNode) {
		    
		    var UserID = treeNode.getAttribute("ID");	
		    var json_key = $(treeNode).find('span').text();
		    // begin. sunshaozong@gmail.com
		    $('#' + UserID).css('color','red');
		    
		    if (__old_node) {
		        $('#' + __old_node).css('color','black');
		    }
		    
		    __old_node = UserID;
		    // end.
		    //debugger;
		    //objNode.getAttribute("innerHtml");
		    var nodeText =treeNode.getAttribute("innerHtml");
		    if (!nodeText) { nodeText = treeNode.innerHTML;}
		    if (UserID == "") {
		        return;
		    }
			//debugger;
		    var addNode = getSelectedItemOfArray(UserID, json_key);
		    /*
		     * 取 UserID 前缀, 判断是哪个 TreeView.
		     */
		    var not_reader = UserID.indexOf('Reader');		    
		    		    
		    if (addNode == null) {
		         addNode =getSelectedItemByName(nodeText, not_reader);
		         if (addNode == null) {
		        return;
		        }
		    }
		    		    
		    
			var lngNodeID = addNode.nodeid;
			var lngNodeType = addNode.nodetype;　　　//环节类型 (会签环节(35,37)可以多选主办)
			var strName = addNode.name;
			var strNodeName = addNode.nodedata;
			var RoleID=addNode.roleid
			var Typepoint = addNode.typepoint;  //判断ＩＤ中是否含有类别，有则表示是选择组的
			var RecType = addNode.rectype;
			var ActorID =addNode.actorid;   //角色ＩＤ

			if (RoleID == "Worker_" || RoleID == "Reader_"  || RoleID == "Assist_")
			{

				//普通发送 交接 调度 都只允许选择一个主办 (会签可以多选主办)
				if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25"   || document.all.SpecRightType.value == "50")
				{
				
				    if(RoleID=="Worker_" && lngNodeType != "35" && lngNodeType != "37") {
//					    nodelist = xmlDoc.documentElement.selectNodes("Receiver[@NodeID=" + lngNodeID + " and @ActorType='Worker_']");					
                
				        nodelist = getNodesByActorTypeAndNodeId(lngNodeID, 'Worker_');				        				        				
				        if (nodelist.length > 0) {				           
						    alert("您已选择了一个" + getSenderName() + ":" + nodelist[0].getAttribute("Name") + "."); //已选择用户角色
						    return false;
						}
				    }						    
				}
				
				if (RecType == "") {
				    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "]";
				    nodelist = getNodesByPath(path);
				    //nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @NodeID=" + lngNodeID + "]");
				}
				else {
				    //当存在接收类别时，ＩＤ的重复要判断　接收类别，因为本身就可能有重复ＩＤ表示不同对象
				    //				    nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @ReceiveType=" + RecType + " and @NodeID=" + lngNodeID + "]");
				    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "][@ReceiveType=" + RecType +"]";
				    nodelist = getNodesByPath(path);
				}

				if(nodelist.length > 0)  //用户已被选择
				{
					ExistUserRole=nodelist[0].getAttribute("ActorType");//已选择用户角色
					switch(ExistUserRole)
					{
						case "Worker_": //主办
							//替换角色
							/* 替换原主办角色
							if (RoleID=="Assist_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"协办:" + strName+"("+strNodeName + ")");
							}
							else if (RoleID=="Reader_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"阅知:" + strName+"("+strNodeName + ")");
							}
							else
								return false;
							*/
							
							return false;
							break;

							
						case "Assist_": //协办
							//替换角色

							if (RoleID=="Worker_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"主办:" + strName+"("+strNodeName + ")");
							}
							/*	新角色为阅知时，替换原协办角色						
							else if (RoleID=="Reader_")
							if (RoleID=="Reader_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"阅知:" + strName+"("+strNodeName + ")");
							}
							*/
							else
								return false;
							break;
							
							
						case "Reader_": //阅知
							//替换角色					
							if (RoleID=="Worker_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"主办:" + strName+"("+strNodeName + ")");
							}
							else if (RoleID=="Assist_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"协办:" + strName+"("+strNodeName + ")");
							}
							else
								return false;
							break;

					}
				}
				else
				{
				
				    var strintSpecRightType ='<%=intSpecRightType%>';
					if (RoleID == "Worker_")
					{
					    if(strintSpecRightType=="esrtTransmit")   //传阅
					    {
					        //当前环节如已有传阅，则不能传阅 
					        var sreturn = GetajaxData(ActorID,"esrtTransmit");
					        if(sreturn=="true")
					        {
					            alert("已存在阅知事项，不需要添加！");
					            return;
					        }
					    }
					    else if(strintSpecRightType=="esrtAssist")   //协办
					    {
					        //如本环节已有协办，则不能协办；如其它环节有协办本环节没有协办，则不能协办，但加上备注；否则可以协办
					        var sreturn = GetajaxData(ActorID,"esrtAssist");
					        if(sreturn=="1")    //存在，且是当前环节
					        {
					            alert("已存在协办事项，不需要添加！");
					            return;
					        }
					        else if(sreturn=="2")  //存在，但不是当前环节
					        {
					            alert("已存在协办事项，不需要添加！");
					            return;
					        }
					    }
						document.all.lstSelected.add(new Option(getSenderName() + ":" + strName+"("+strNodeName + ")",ActorID));
					}
					else if (RoleID == "Reader_")
					{
						document.all.lstSelected.add(new Option("阅知:" + strName+" ("+strNodeName + ")",ActorID));
					}
					else
					{
					    document.all.lstSelected.add(new Option("协办:" + strName+" ("+strNodeName + ")",ActorID));
					    //document.all.lstSelected.add(new Option("协办:" + strName , ActorID));
					}
					lngSelectID++;
					var addedNode = { "ID ": lngSelectID.toString(), "NodeID ": lngNodeID.toString(), "NodeType": lngNodeType, "UserID": ActorID, "Name": strName, "ActorType": RoleID, "ReceiveType": RecType };
					selectedNodeList.push(addedNode);

					xmlEle = xmlDoc.createElement("Receiver");
					xmlEle.setAttribute("ID",lngSelectID);
					xmlEle.setAttribute("NodeID",lngNodeID);
					xmlEle.setAttribute("NodeType",lngNodeType);
					xmlEle.setAttribute("UserID",ActorID);
					xmlEle.setAttribute("Name",strName);
					xmlEle.setAttribute("ActorType",RoleID);
					xmlEle.setAttribute("ReceiveType",RecType);
				
					xmlDoc.documentElement.appendChild(xmlEle);
				}			
				
				
			}
		}
        //取得阅知、协办时一些控制信息
		function GetajaxData(lngUserID,strType)
        {
            var lngMessageID = '<%= lngMessageID%>';
            var lngFlowID = '<%= lngFlowID%>';
            var lngNodeID = '<%= lngNodeID%>';
            var nowDay = new Date();
            var param = nowDay.getDate().toString() + nowDay.getMinutes().toString() + nowDay.getMilliseconds().toString();
            var svalue = $.ajax({  url: "frmXmlHttp.aspx?Type=" + strType + "&param=" + param + "&UserID=" + lngUserID
                 + "&MessageID=" + lngMessageID + "&FlowID=" + lngFlowID + "&NodeID=" + lngNodeID,  async: false      }).responseText;
            return svalue;
        }
        function getXmlString(xmlDoc) {
            if (window.ActiveXObject) {
                return xmlDoc.xml;
            }
            else if (document.implementation && document.implementation.createDocument) {
                var serializer = new XMLSerializer();
                var str = serializer.serializeToString(xmlDoc.documentElement);
                return str;
            }
        }
		
		function FlowSubmit()
		{
			
			 if (SendCheck()) {  
			    if(document.all.SelType.value == "1")
			    {
			        parent.header.flowInfo.Receivers.value = getXmlString (xmlDoc);
			        parent.header.flowInfo.LinkNodeID.value = document.all.LinkNodeID.value;
			        parent.header.flowInfo.LinkNodeType.value = document.all.LinkNodeType.value;
				    parent.header.flowInfo.action = "flow_Attemper_Submit.aspx?MessageID=" + parent.header.flowInfo.AttemperID.value ;
	                parent.header.flowInfo.target="_parent";
	         
	                parent.header.flowInfo.submit();
				    //setTimeout('',1000);
				    //self.close();
				    parent.fraaddNew.cols="0,100%,0";
				    window.location="#";
			    }
			    else {
			        parent.header.flowSubmit.Receivers.value = getXmlString(xmlDoc);
			        parent.header.flowSubmit.LinkNodeID.value = document.all.LinkNodeID.value;
			        parent.header.flowSubmit.LinkNodeType.value = document.all.LinkNodeType.value;
				    parent.header.FlowSubmit();
				    //setTimeout('',1000);
				    //self.close();
				    parent.fraaddNew.cols="0,100%,0";
				    window.location="#";
				}
				
			 }
			 else
			 {
			    //不是普通发送时 不合格直接退出
			    if(document.all.SpecRightType.value != "10")
			    {
			        parent.fraaddNew.cols='0,100%,0';
			        window.location='#';
			    }    
			 }
			 
		}
		
		/// 检查是否可以发送了
		function SendCheck()
		{
			var canSend=true;
			var lngNodeType = "";
			var arrNodeList = document.all.NodeList.value.split(",");
			var masterNodeID = document.all.MasterNodeID.value;
			var masterNodeName = document.all.MasterNodeName.value;
			
			for(i=0;i<arrNodeList.length;i++)
			{
			    //xmlNodeList = xmlDoc.documentElement.selectNodes("Receiver[@NodeID=" + arrNodeList[i] +"and @ActorType='Worker_']");
			    xmlNodeList = getNodesByActorTypeAndNodeId(arrNodeList[i], 'Worker_');
				if (xmlNodeList.length > 0)
				{
				    lngNodeType = xmlNodeList[0].getAttribute("NodeType");  
				}
				//主送路径下第一个分流环节或普通环节时需判断是否有选择人员
//				if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25" || document.all.SpecRightType.value == "40")
//				{
				    if(xmlNodeList.length == 0 && (arrNodeList[i]== masterNodeID || masterNodeID=="-1"))
				    {  
				         if(document.all.SpecRightType.value == "10")
				         {
				            if(masterNodeName == "")
				            {
				                alert("处理动作涉及的下一环节必须选择一个" + getSenderName() + "人员");
				            }
				            else
				            {
					            alert("处理动作涉及的下一环节必须选择[" + masterNodeName + "]的主办人员");
					        }
					     }
					    canSend=false;
					    return canSend;
				    }
//				}
                //只有普通发送才提示,调度 交接 多选在前面已经控制， 其它情况下可以多选
                if(document.all.SpecRightType.value == "10")
                {
				    if (xmlNodeList.length > 1)
				    {
    					
					    if(lngNodeType != "35" && lngNodeType != "37") 
					    {
				            alert("处理动作涉及的下一环节只能选择一个" + getSenderName() + "人员"); 
					        canSend=false;
					        return canSend;
					    }
    					
				    }
				}
			}
			return canSend;
		}
		
	var selectobj = document.getElementById("<%=name0.ClientID%>");
    
    function chang(obj,img){            
        /*     
         * Date: 2012-8-9 10:47
         * summary: 当点击td时，已同时进入了 moveover 事件, 那么当离开时呢?
         * 就是下面这段代码做的事情. 因此, 虽然我们在 chang_Class 中设置了选中
         * 的td 的背景图, 还是被移除了.
         * modified: sunshaozong@gmail.com     
         */              
        //$(obj).css('background-image', 'url(../Images/' + img +')');		        
//$("#" +obj.id ).css("background-image","url(../Images/" + img +")");
    }
	//Tab选项卡
    function chang_Class(name,num,my)
	{ 
	    var objname = name.id.substring(0,name.id.length-1);
        for(i=0;i<3;i++)
        {
	        if(i!=my){
	            if(typeof(document.getElementById(objname+i))!="undefined" && document.getElementById(objname+i)!=null)
	            {
		            document.getElementById(objname+i).className="";
		            document.getElementById(objname+i).background="../Images/lm-a.gif";
			    //$("#" +objname+i ).css("background-image","url(../Images/lm-a.gif)");
		        }
	        }		 		      //显示其它选项卡
            //	     document.getElementById("name"+i).style.display =""; 
        }        

        /*     
         * Date: 2012-8-9 10:49
         * summary: 设置所有 td 为默认背景图; 设置选中 td 的背景图; 
         * modified: sunshaozong@gmail.com     
         */              
                 
        var _bg_image = { normal : { 'background-image' : 'url(../Images/lm-a.gif)' },
                          selected : { 'background-image' : 'url(../Images/lm-b.gif)' }};
                
        $(name).parent().find('> td:gt(0)').filter(function(i){
                    return i < 3;
                })
               .css(_bg_image.normal);		  
        
        $(name).css(_bg_image.selected);		  
              
        
        selectobj = document.getElementById(objname+my);
        document.getElementById(objname+my).className="";
        document.getElementById(objname+my).background="../Images/lm-b.gif";
 	//$("#" + objname+i ).css("background-image","url(../Images/lm-b.gif)");
        switch(my)
        {	 
            case 0:   //主办 
            document.getElementById("Tab0").style.display ="";
            document.getElementById("Tab1").style.display ="none";
            document.getElementById("Tab2").style.display ="none"; 
            
            document.getElementById("tablist0").style.display =""; 
            document.getElementById("tablist1").style.display ="none"; 
            break;
            case 1:    //协办 
                document.getElementById("Tab0").style.display ="none";
                document.getElementById("Tab1").style.display ="";
                document.getElementById("Tab2").style.display ="none";  
                
                document.getElementById("tablist0").style.display ="none"; 
                document.getElementById("tablist1").style.display =""; 
            break;
            case 2:  //阅知
                document.getElementById("Tab0").style.display ="none";
                document.getElementById("Tab1").style.display ="none";
                document.getElementById("Tab2").style.display ="";    
                
                document.getElementById("tablist0").style.display ="none"; 
                document.getElementById("tablist1").style.display ="";            
                //Iframe2.location="../InformationManager/frmInf_InformationMain.aspx?IsSelect=1&randomid="+GetRandom()+"&PKey=" + encodeURIComponent(sKey); 
            break;	 	 
        }
        
        var btree=document.getElementById("ckblSelect_0").checked;
        if(!btree)
        {
            QueryPersonValue(document.getElementById("txtSelect").value);
        }
    }
    
    function ChangeTreeList()
    {
        var btree=document.getElementById("ckblSelect_0").checked;
        if(btree)
        {
            document.getElementById("divtree").style.display ="";
            document.getElementById("divlist").style.display ="none";
        }
        else 
        {
            document.getElementById("divtree").style.display ="none";
            document.getElementById("divlist").style.display ="";
            
            QueryPersonValue("");
            document.getElementById("txtSelect").value = "";
            
        }
    }
    
    function QueryPerson(obj)    //根据条件搜索人
    {
        QueryPersonValue(obj.value);
    }
    function QueryPersonValue(svalue)
    {
        //alert(obj.value);
        if(document.getElementById("tablist0").style.display=="")
        {
            document.getElementById("lstperson").length = 0 ;
            for (var i = 0; i < arrMastPerson.length; i++)
            {   
                //alert(arrMastPerson[i][1].toUpperCase().indexOf(obj.value.toUpperCase()));
                if(arrMastPerson[i][1].toUpperCase().indexOf(svalue.toUpperCase()) >= 0)
                {
                    document.getElementById("lstperson").options.add(new Option(arrMastPerson[i][1],arrMastPerson[i][0]));
                }
            }   
        }
        else
        {
            document.getElementById("lstread").length = 0;
            for (var i = 0; i < arrReadPerson.length; i++)
            {   
                //alert(arrMastPerson[i][1].toUpperCase().indexOf(obj.value.toUpperCase()));
                if(arrReadPerson[i][1].toUpperCase().indexOf(svalue.toUpperCase()) >= 0)
                {
                    document.getElementById("lstread").options.add(new Option(arrReadPerson[i][1],arrReadPerson[i][0]));
                }
            }
         }
    }
    
    //列表选人
    function AddSelectObjToList(obj) {     
	    if(obj.selectedIndex==-1){
	        return;}
	        var svalue = obj.options[obj.selectedIndex].value;	
	    AddSelectToList(svalue)
	    SortItem();//重新排序列表框  
	}
	
    function AddSelectToList(svalue)
	{
	    var arrlist = svalue.split("|");
	    var UserID = 0;
	    var lngNodeIDType = "";   //环节ＩＤ和类型
	    var strName = "";
	    var strNodeName = "";
	    if(arrlist.length>0)
	    {
	        UserID = arrlist[0];
	        lngNodeIDType = arrlist[1];
	        strNodeName = arrlist[2];
	        strName = arrlist[3];
		}
		
		var lngNodeID = lngNodeIDType.substring(0,lngNodeIDType.indexOf("_"));　　//环节ＩＤ
		var lngNodeType = lngNodeIDType.substring(lngNodeIDType.indexOf("_") + 1);　　　//环节类型 (会签环节(35,37)可以多选主办)
		
		var RoleID = UserID.substring(0, 7);

		if ($.browser.safari) {
		    //等下再加。
		}
		else if ($.browser.msie) {
		    if (document.getElementById("Tab1").style.display == "")   //特殊处理
		    {
		        RoleID = "Assist_";
		    }
		}		
	
		var Typepoint = UserID.substring(7).indexOf("_");  //判断ＩＤ中是否含有类别，有则表示是选择组的
		var RecType = "";   //UserType != "" 表示　要接收，ＵＳＥＲＴＹＰＥ表示接收角色的类别
		var ActorID ;   //角色ＩＤ
		if(Typepoint > 0 )
		{
		   RecType = UserID.substring(7).substring(Typepoint+1);
		   ActorID = UserID.substring(7).substring(0,Typepoint);
		   
		}
		else
		{
			 ActorID = UserID.substring(7);
        }
     
		if (RoleID == "Worker_" || RoleID == "Reader_"  || RoleID == "Assist_") {		   
			//普通发送 交接 调度 都只允许选择一个主办 (会签可以多选主办)
			if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25"   || document.all.SpecRightType.value == "50")
			{			    
			    if(RoleID=="Worker_" && lngNodeType != "35" && lngNodeType != "37") {			       
			        //nodelist = xmlDoc.documentElement.selectNodes("Receiver[@NodeID=" + lngNodeID + " and @ActorType='Worker_']");
			        nodelist = getNodesByActorTypeAndNodeId(lngNodeID, 'Worker_');
				    if(nodelist.length > 0)
				    {
					    alert("您已选择了一个" + getSenderName() +  ":"+nodelist[0].getAttribute("Name"));//已选择用户角色
					    return false;
				    }
			    }
			}
			
			if(RecType == "") {
			    // nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @NodeID=" + lngNodeID + "]");
			    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "]";
			    nodelist = getNodesByPath(path);
			}
			else {
			    //当存在接收类别时，ＩＤ的重复要判断　接收类别，因为本身就可能有重复ＩＤ表示不同对象
			    //nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @ReceiveType=" + RecType + " and @NodeID=" + lngNodeID + "]");
			    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "][@ReceiveType=" + RecType + "]";
			    nodelist = getNodesByPath(path);
			}
		
			if(nodelist.length > 0)  //用户已被选择
			{
				ExistUserRole=nodelist[0].getAttribute("ActorType");//已选择用户角色
				switch(ExistUserRole)
				{
					case "Worker_": //主办
						return false;
						break;

						
					case "Assist_": //协办
						//替换角色

						if (RoleID=="Worker_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"主办:" + strName+"("+strNodeName + ")");
						}
						else
							return false;
						break;
						
						
					case "Reader_": //阅知
						//替换角色					
						if (RoleID=="Worker_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"主办:" + strName+"("+strNodeName + ")");
						}
						else if (RoleID=="Assist_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"协办:" + strName+"("+strNodeName + ")");
						}
						else
							return false;
						break;

				}
			}
			else
			{
				 var strintSpecRightType ='<%=intSpecRightType%>';
				if (RoleID == "Worker_")
				{
				    if(strintSpecRightType=="esrtTransmit")   //传阅
				    {
				        //当前环节如已有传阅，则不能传阅 
				        var sreturn = GetajaxData(ActorID,"esrtTransmit");
				        if(sreturn=="true")
				        {
				            alert("已存在阅知事项，不需要添加！");
				            return;
				        }
				    }
				    else if(strintSpecRightType=="esrtAssist")   //协办
				    {
				        //如本环节已有协办，则不能协办；如其它环节有协办本环节没有协办，则不能协办，但加上备注；否则可以协办
				        var sreturn = GetajaxData(ActorID,"esrtAssist");
				        if(sreturn=="1")    //存在，且是当前环节
				        {
				            alert("已存在协办事项，不需要添加！");
				            return;
				        }
				        else if(sreturn=="2")  //存在，但不是当前环节
				        {
				            alert("已存在协办事项，不需要添加！");
				            return;
				        }
				    }
					document.all.lstSelected.add(new Option(getSenderName() + ":" + strName+"("+strNodeName + ")",ActorID));
				}
				else if (RoleID == "Reader_")
				{
					document.all.lstSelected.add(new Option("阅知:" + strName+" ("+strNodeName + ")",ActorID));
				}
				else
				{
					document.all.lstSelected.add(new Option("协办:" + strName+" ("+strNodeName + ")",ActorID));
				}				
				lngSelectID++;
				xmlEle = xmlDoc.createElement("Receiver");
				xmlEle.setAttribute("ID",lngSelectID);
				xmlEle.setAttribute("NodeID",lngNodeID);
				xmlEle.setAttribute("NodeType",lngNodeType);
				xmlEle.setAttribute("UserID",ActorID);
				xmlEle.setAttribute("Name",strName);
				xmlEle.setAttribute("ActorType",RoleID);
				xmlEle.setAttribute("ReceiveType",RecType);
			
				xmlDoc.documentElement.appendChild(xmlEle);
			}			
		}
	}
	//==zxl==
	
	function CheckInfo(obj)
    {
        if (event.keyCode==13) 
        { 
            QueryPerson(obj);
        //  alert("请选择列表框的项！");  
        return false;
        } 
    }

	//====zxl==
	
	
	//-->
		</script>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" onload="window.top.scroll(0,0);">
		<form id="Form1" method="post" runat="server">
		<asp:HiddenField ID="hidTreeNodeValue" runat="server" />
			<TABLE id="Table3" style="Z-INDEX: 105; LEFT: 8px; POSITION: absolute; TOP: 8px" cellSpacing="1"
				cellPadding="1" width="98%" align="center" border="0">
				<TR>
					<TD vAlign="top" align="center" width="50%" colSpan="3"><FONT face="宋体">
							<uc1:CtrTitle id="CtrTitle1" runat="server" EnableViewState="False"></uc1:CtrTitle></FONT></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" width="50%"></TD>
					<TD width="10%"></TD>
					<TD vAlign="top" align="left" width="40%"></TD>
				</TR>
				<TR>
					<TD vAlign="top" align="left" width="50%">
					    <table class="listContent" width="90%" align="center">
                        <tr>
                        <td class="list">
                            <table align="left" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="7" valign="top"><img src="../Images/lm-left.gif" width="7" height="29" /></td>
                                    <td id="name0" onclick="chang_Class(this,3,0)"  height="25" align="center" style="cursor: hand;
                                        width: 95;"  background="../images/lm-b.gif" runat="server" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                        <asp:Label ID="lblMast" runat="server" Text="主办"></asp:Label>
                                    </td>
                                    <td id="name1" onclick="chang_Class(this,3,1)" height="25" align="center" style="cursor: hand;
                                        width: 95;"  background="../images/lm-a.gif" runat="server" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                        协办
                                    </td>
                                    <td width="95" id="name2" onclick="chang_Class(this,3,2)" height="25" align="center" class="list"
                                        style="cursor: hand;" background="../images/lm-a.gif" runat="server" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                        阅知
                                    </td>
                                    <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                                    <td align="right"  >
                                        <asp:RadioButtonList ID="ckblSelect" runat="server" RepeatDirection="Horizontal" onclick="ChangeTreeList();" >
                                            <asp:ListItem Selected="True" Value="0">树型</asp:ListItem>
                                            <asp:ListItem Value="1">列表</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        </tr>
                        <tr>
                            <td class="list">
                                <div id="divtree" style="">
                                    <div id="Tab0" style="overflow-y:auto; height:400px;border:1px solid black">

                                        <asp:TreeView ID="tvReceiver" runat="server" ondblclick="return AddToList(this);" onclick="SetCurrTreeView(this);"
                                                 BackColor="White"  Width="100%" Height="360px" 
                                                 EnableViewState="false"  >
                                            
                                        </asp:TreeView>
                                    </div>
                                    <div id="Tab1" style="display:none;">

							            
							             <asp:TreeView ID="tvAssistor" runat="server" ondblclick="return AddToList(this);" onclick="SetCurrTreeView(this);"
                                                 BackColor="White" BorderStyle="Groove" BorderColor="Black" Width="100%" Height="360px" BorderWidth="1px" 
                                                 EnableViewState="false" >
                                          </asp:TreeView>
                                    </div>
							        <div id="Tab2" style="display:none;  height:100%;">    

							            
							             <asp:TreeView ID="tvReader" runat="server" ondblclick="return AddToList(this);" onclick="SetCurrTreeView(this);"
                                                 BackColor="White" BorderStyle="Groove" BorderColor="Black" Width="100%" Height="360px" BorderWidth="1px" 
                                                 EnableViewState="false" >
                                          </asp:TreeView>
							        </div>
							    </div>
							    <div id="divlist" style=" display:none;">    <%--onpropertychange="QueryPerson(this);"--%>
                                    搜索条件：<asp:TextBox ID="txtSelect" runat="server"  Width="99%" onblur="QueryPerson(this);" onkeypress="return CheckInfo(this);" ></asp:TextBox>
                                    <div id="tablist0">
                                        <SELECT id="lstperson" runat="server" ondblclick="AddSelectObjToList(this);" style="WIDTH: 100%; HEIGHT: 360px" size="22"></SELECT>
                                    </div>
                                    <div id="tablist1" style="display:none;">
                                        <SELECT id="lstread" runat="server" ondblclick="AddSelectObjToList(this);" style="WIDTH: 100%; HEIGHT: 360px" multiple size="22"></SELECT>
                                    </div>
                                     
							    </div>
                            </td>
                        </tr>
                        </table>
					    
					 </TD>
					 <div style=" v"></div>
					<TD width="10%" colSpan="1" rowSpan="1" >
						<P><FONT face="宋体"><INPUT class="btnClass" id="cmdSelect"  style="visibility:hidden;"  onclick="AddToListButton();"
									type="button" value="选择(>)"></FONT></P>
						<P><FONT face="宋体"><INPUT class="btnClass" id="cmdDelete"  onclick="RemoveItem();" type="button"
									value="移除(<)"></FONT></P>
						<P><FONT face="宋体"><INPUT class="btnClass" id="cmdClear"  onclick="ClearItem();" type="button"
									value="清除(<<)" name="cmdClear"></FONT></P>
					</TD>
					<TD vAlign="top" align="left" width="40%" colSpan="1" rowSpan="1">
					    <div  id="div_lstSelect" >
					    <SELECT id="lstSelected" runat="server" ondblclick="RemoveItem();" 
					            style="WIDTH: 100%; HEIGHT: 380px;" multiple
							size="22" name="lstSelect"></SELECT>
					    </div>
							<INPUT class="btnClass" id="cmdOK"  onclick="FlowSubmit();"
							type="button" value="确 定"><FONT face="宋体">&nbsp; </FONT><INPUT class="btnClass" id="cmdCancel"  onclick="parent.fraaddNew.cols='0,100%,0';window.location='#';"
							type="button" value="取 消"></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD></TD>
					<TD style="WIDTH: 165px"></TD>
				</TR>
			</TABLE>
			&nbsp;&nbsp;
			<P><input id="NodeList"  type="hidden" value="<%=strNodeList%>" name="NodeList"/> 
			<input id="LinkNodeID" type="hidden" value="<%=strLinkNodeID%>"/>
			 <input id="LinkNodeType" type="hidden" value="<%=strLinkNodeType%>"/>
			 <input id="MasterNodeID"  type="hidden" value="<%=strMasterNodeID%>" name="MasterNodeID"/>
			 <input id="MasterNodeName"  type="hidden" value="<%=strMasterNodeName%>" name="MasterNodeName"/>
			 <input id="SelType"  type="hidden" value="<%=strSelType%>" name="SelType"/>
			 <input id="SpecRightType" type="hidden" value="<%=strSpecRightType%>" name= "SpecRightType" /> 
			 
			 <input id="DeleteMaster" type="hidden" value="<%=sblnDeleteMaster%>" name= "DeleteMaster" /> 
			 
			 <asp:Literal ID="litMastPerson" runat="server"></asp:Literal>
			 <asp:Literal ID="litReadPerson" runat="server"></asp:Literal>
			</P>
          <script language="javascript" type="text/javascript">
                if (typeof(document.all.<%=name0.ClientID%>)=="undefined" )
                {
                    selectobj = document.getElementById('<%=name1.ClientID%>');
                    document.getElementById('<%=name1.ClientID%>').className="";
                    document.getElementById('<%=name1.ClientID%>').background="../Images/lm-b.gif";
        
                    document.getElementById("Tab0").style.display ="none";
                    document.getElementById("Tab1").style.display ="";
                    document.getElementById("Tab2").style.display ="none";  
                    
                    document.getElementById("tablist0").style.display ="none"; 
                    document.getElementById("tablist1").style.display =""; 
                }
                else
                    selectobj = document.getElementById("<%=name0.ClientID%>");
          </script> 
		</form>
	</body>
</HTML>
