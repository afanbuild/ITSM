<%@ Register TagPrefix="uc1" TagName="CtrTitle" Src="../Controls/CtrTitle.ascx" %>
<%@ Page language="c#" validateRequest="false" Inherits="Epower.ITSM.Web.Forms.flow_sender" Codebehind="flow_sender.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD id="HEAD1" runat="server">
		<title>����/������Աѡ��</title>
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

		var currTv = null;   //��ǰ��treeview
		var selectedNodeList = [];

        //��һ��xml�ĵ���ʽ���ַ�������xml�ĵ�
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
		   var ret = "����";
		   
		   if(sSpec == "60")
		   {
		       ret = "��֪";
		   }
		   if(sSpec == "70")
		   {
		       ret = "Э��";
		   }
		   if(sSpec == "80")
		   {
		       ret = "��ͨ";
		   }
		   return ret;
		}
	    
		function RemoveItem()
		{
			//������ı��ָ�һ��XML��������������������ӻ���ɾ�� ��������
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
			if(document.all.DeleteMaster.value == "0" && UserName.indexOf("����") != -1)
			{
			    //����ɾ��
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
		
		//���Listbox�е�����
		function ClearItem() {

		    xmlDoc = createXml('<%=sIniXml%>');
		    while (document.all.lstSelected.options.length > 0) {
		        document.all.lstSelected.remove(0);
		    }
		}



		//��ѡ���б�����
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
						//����λ��
						LI=arrayLI[i];
						arrayLI[i]=new Option(arrayLI[j].text,arrayLI[j].value);
						arrayLI[j]=LI;
					}
				}
			}
		}



		//��ѡ���б�����
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
            if(!btree)   //�б�
            {
                var obj;
                if(document.getElementById("tablist0").style.display=="")   //����
                {
                    obj = document.getElementById("lstperson") ;
                    if(obj.selectedIndex!=-1)
                    {
                        AddSelectToList(obj.options[obj.selectedIndex].value);
                    }
                    else
                    {
                        alert("��ѡ�����ݣ�");
                        return;
                    }
                }
                else   //Э�����֪
                {
                    obj = document.getElementById("lstread") ;
                    for(i=0;i<obj.length;i++){      
                        if(obj.options[i].selected){   
                            AddSelectToList(obj.options[i].value);
                        }   
                    } 
                    if(obj.selectedIndex==-1)
                    {
                        alert("��ѡ�����ݣ�");
                        return;
                    }  
                }
                SortItem();//���������б��  
            }
	        else
	        {
			    AddToList(currTv);  //��
			}
	    }
	    
	  
		
		var clickedNode;
		function AddToList(tv)
		{		
		    if(tv==null)
		    {
		        alert("��ѡ�����ݣ�");
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
			    //QueryAllChillNode(treeNode);//���ӽڵ���ݹ������ӽڵ�
			    //ȥ��QueryAllChillNode��δ������������Ҫ����ĵط�lizg 2012-6-7
			    AddNodeToList(treeNode); //�ýڵ�û���ӽڵ�	
			}
			else {			
				AddNodeToList(treeNode);			 
			}
			
			SortItem();//���������б��  
			
			//alert(clickedNode.outerHTML);
			$(clickedNode).css('color', 'red');
					
		}
		
		
		//�����ڵ��µ��ӽڵ�
		function QueryAllChillNode(treeNode) {
		    var NodeArray = treeNode.getChildren();		 
			if(NodeArray.length == 0)
			{
			    AddNodeToList(treeNode);//�ýڵ�û���ӽڵ�	
			}
			else
			{
                for(var i=0; i<NodeArray.length;i++)
			    {
				    if(NodeArray[i].getChildren().length==0)
				    {
					    AddNodeToList(NodeArray[i]);//�ýڵ�û���ӽڵ�	
				    }
			    }
			}
		}
		

		function getSelectedItemOfArray(queryKey, json_key) {
		   
		    if (allNodeItems == null) {
		        alert("û��Json���顣");
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
		        alert("û��Json���顣");
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
		     * ȡ UserID ǰ׺, �ж����ĸ� TreeView.
		     */
		    var not_reader = UserID.indexOf('Reader');		    
		    		    
		    if (addNode == null) {
		         addNode =getSelectedItemByName(nodeText, not_reader);
		         if (addNode == null) {
		        return;
		        }
		    }
		    		    
		    
			var lngNodeID = addNode.nodeid;
			var lngNodeType = addNode.nodetype;������//�������� (��ǩ����(35,37)���Զ�ѡ����)
			var strName = addNode.name;
			var strNodeName = addNode.nodedata;
			var RoleID=addNode.roleid
			var Typepoint = addNode.typepoint;  //�жϣɣ����Ƿ�����������ʾ��ѡ�����
			var RecType = addNode.rectype;
			var ActorID =addNode.actorid;   //��ɫ�ɣ�

			if (RoleID == "Worker_" || RoleID == "Reader_"  || RoleID == "Assist_")
			{

				//��ͨ���� ���� ���� ��ֻ����ѡ��һ������ (��ǩ���Զ�ѡ����)
				if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25"   || document.all.SpecRightType.value == "50")
				{
				
				    if(RoleID=="Worker_" && lngNodeType != "35" && lngNodeType != "37") {
//					    nodelist = xmlDoc.documentElement.selectNodes("Receiver[@NodeID=" + lngNodeID + " and @ActorType='Worker_']");					
                
				        nodelist = getNodesByActorTypeAndNodeId(lngNodeID, 'Worker_');				        				        				
				        if (nodelist.length > 0) {				           
						    alert("����ѡ����һ��" + getSenderName() + ":" + nodelist[0].getAttribute("Name") + "."); //��ѡ���û���ɫ
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
				    //�����ڽ������ʱ���ɣĵ��ظ�Ҫ�жϡ����������Ϊ����Ϳ������ظ��ɣı�ʾ��ͬ����
				    //				    nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @ReceiveType=" + RecType + " and @NodeID=" + lngNodeID + "]");
				    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "][@ReceiveType=" + RecType +"]";
				    nodelist = getNodesByPath(path);
				}

				if(nodelist.length > 0)  //�û��ѱ�ѡ��
				{
					ExistUserRole=nodelist[0].getAttribute("ActorType");//��ѡ���û���ɫ
					switch(ExistUserRole)
					{
						case "Worker_": //����
							//�滻��ɫ
							/* �滻ԭ�����ɫ
							if (RoleID=="Assist_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"Э��:" + strName+"("+strNodeName + ")");
							}
							else if (RoleID=="Reader_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"��֪:" + strName+"("+strNodeName + ")");
							}
							else
								return false;
							*/
							
							return false;
							break;

							
						case "Assist_": //Э��
							//�滻��ɫ

							if (RoleID=="Worker_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"����:" + strName+"("+strNodeName + ")");
							}
							/*	�½�ɫΪ��֪ʱ���滻ԭЭ���ɫ						
							else if (RoleID=="Reader_")
							if (RoleID=="Reader_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(UserID.substring(7),"��֪:" + strName+"("+strNodeName + ")");
							}
							*/
							else
								return false;
							break;
							
							
						case "Reader_": //��֪
							//�滻��ɫ					
							if (RoleID=="Worker_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"����:" + strName+"("+strNodeName + ")");
							}
							else if (RoleID=="Assist_")
							{
								nodelist[0].setAttribute("ActorType",RoleID);
								Set_ItemText_By_Value(ActorID,"Э��:" + strName+"("+strNodeName + ")");
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
					    if(strintSpecRightType=="esrtTransmit")   //����
					    {
					        //��ǰ���������д��ģ����ܴ��� 
					        var sreturn = GetajaxData(ActorID,"esrtTransmit");
					        if(sreturn=="true")
					        {
					            alert("�Ѵ�����֪�������Ҫ��ӣ�");
					            return;
					        }
					    }
					    else if(strintSpecRightType=="esrtAssist")   //Э��
					    {
					        //�籾��������Э�죬����Э�죻������������Э�챾����û��Э�죬����Э�죬�����ϱ�ע���������Э��
					        var sreturn = GetajaxData(ActorID,"esrtAssist");
					        if(sreturn=="1")    //���ڣ����ǵ�ǰ����
					        {
					            alert("�Ѵ���Э���������Ҫ��ӣ�");
					            return;
					        }
					        else if(sreturn=="2")  //���ڣ������ǵ�ǰ����
					        {
					            alert("�Ѵ���Э���������Ҫ��ӣ�");
					            return;
					        }
					    }
						document.all.lstSelected.add(new Option(getSenderName() + ":" + strName+"("+strNodeName + ")",ActorID));
					}
					else if (RoleID == "Reader_")
					{
						document.all.lstSelected.add(new Option("��֪:" + strName+" ("+strNodeName + ")",ActorID));
					}
					else
					{
					    document.all.lstSelected.add(new Option("Э��:" + strName+" ("+strNodeName + ")",ActorID));
					    //document.all.lstSelected.add(new Option("Э��:" + strName , ActorID));
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
        //ȡ����֪��Э��ʱһЩ������Ϣ
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
			    //������ͨ����ʱ ���ϸ�ֱ���˳�
			    if(document.all.SpecRightType.value != "10")
			    {
			        parent.fraaddNew.cols='0,100%,0';
			        window.location='#';
			    }    
			 }
			 
		}
		
		/// ����Ƿ���Է�����
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
				//����·���µ�һ���������ڻ���ͨ����ʱ���ж��Ƿ���ѡ����Ա
//				if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25" || document.all.SpecRightType.value == "40")
//				{
				    if(xmlNodeList.length == 0 && (arrNodeList[i]== masterNodeID || masterNodeID=="-1"))
				    {  
				         if(document.all.SpecRightType.value == "10")
				         {
				            if(masterNodeName == "")
				            {
				                alert("�������漰����һ���ڱ���ѡ��һ��" + getSenderName() + "��Ա");
				            }
				            else
				            {
					            alert("�������漰����һ���ڱ���ѡ��[" + masterNodeName + "]��������Ա");
					        }
					     }
					    canSend=false;
					    return canSend;
				    }
//				}
                //ֻ����ͨ���Ͳ���ʾ,���� ���� ��ѡ��ǰ���Ѿ����ƣ� ��������¿��Զ�ѡ
                if(document.all.SpecRightType.value == "10")
                {
				    if (xmlNodeList.length > 1)
				    {
    					
					    if(lngNodeType != "35" && lngNodeType != "37") 
					    {
				            alert("�������漰����һ����ֻ��ѡ��һ��" + getSenderName() + "��Ա"); 
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
         * summary: �����tdʱ����ͬʱ������ moveover �¼�, ��ô���뿪ʱ��?
         * ����������δ�����������. ���, ��Ȼ������ chang_Class ��������ѡ��
         * ��td �ı���ͼ, ���Ǳ��Ƴ���.
         * modified: sunshaozong@gmail.com     
         */              
        //$(obj).css('background-image', 'url(../Images/' + img +')');		        
//$("#" +obj.id ).css("background-image","url(../Images/" + img +")");
    }
	//Tabѡ�
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
	        }		 		      //��ʾ����ѡ�
            //	     document.getElementById("name"+i).style.display =""; 
        }        

        /*     
         * Date: 2012-8-9 10:49
         * summary: �������� td ΪĬ�ϱ���ͼ; ����ѡ�� td �ı���ͼ; 
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
            case 0:   //���� 
            document.getElementById("Tab0").style.display ="";
            document.getElementById("Tab1").style.display ="none";
            document.getElementById("Tab2").style.display ="none"; 
            
            document.getElementById("tablist0").style.display =""; 
            document.getElementById("tablist1").style.display ="none"; 
            break;
            case 1:    //Э�� 
                document.getElementById("Tab0").style.display ="none";
                document.getElementById("Tab1").style.display ="";
                document.getElementById("Tab2").style.display ="none";  
                
                document.getElementById("tablist0").style.display ="none"; 
                document.getElementById("tablist1").style.display =""; 
            break;
            case 2:  //��֪
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
    
    function QueryPerson(obj)    //��������������
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
    
    //�б�ѡ��
    function AddSelectObjToList(obj) {     
	    if(obj.selectedIndex==-1){
	        return;}
	        var svalue = obj.options[obj.selectedIndex].value;	
	    AddSelectToList(svalue)
	    SortItem();//���������б��  
	}
	
    function AddSelectToList(svalue)
	{
	    var arrlist = svalue.split("|");
	    var UserID = 0;
	    var lngNodeIDType = "";   //���ڣɣĺ�����
	    var strName = "";
	    var strNodeName = "";
	    if(arrlist.length>0)
	    {
	        UserID = arrlist[0];
	        lngNodeIDType = arrlist[1];
	        strNodeName = arrlist[2];
	        strName = arrlist[3];
		}
		
		var lngNodeID = lngNodeIDType.substring(0,lngNodeIDType.indexOf("_"));����//���ڣɣ�
		var lngNodeType = lngNodeIDType.substring(lngNodeIDType.indexOf("_") + 1);������//�������� (��ǩ����(35,37)���Զ�ѡ����)
		
		var RoleID = UserID.substring(0, 7);

		if ($.browser.safari) {
		    //�����ټӡ�
		}
		else if ($.browser.msie) {
		    if (document.getElementById("Tab1").style.display == "")   //���⴦��
		    {
		        RoleID = "Assist_";
		    }
		}		
	
		var Typepoint = UserID.substring(7).indexOf("_");  //�жϣɣ����Ƿ�����������ʾ��ѡ�����
		var RecType = "";   //UserType != "" ��ʾ��Ҫ���գ��գӣţңԣ٣Уű�ʾ���ս�ɫ�����
		var ActorID ;   //��ɫ�ɣ�
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
			//��ͨ���� ���� ���� ��ֻ����ѡ��һ������ (��ǩ���Զ�ѡ����)
			if(document.all.SpecRightType.value == "10" || document.all.SpecRightType.value == "20"  || document.all.SpecRightType.value == "25"   || document.all.SpecRightType.value == "50")
			{			    
			    if(RoleID=="Worker_" && lngNodeType != "35" && lngNodeType != "37") {			       
			        //nodelist = xmlDoc.documentElement.selectNodes("Receiver[@NodeID=" + lngNodeID + " and @ActorType='Worker_']");
			        nodelist = getNodesByActorTypeAndNodeId(lngNodeID, 'Worker_');
				    if(nodelist.length > 0)
				    {
					    alert("����ѡ����һ��" + getSenderName() +  ":"+nodelist[0].getAttribute("Name"));//��ѡ���û���ɫ
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
			    //�����ڽ������ʱ���ɣĵ��ظ�Ҫ�жϡ����������Ϊ����Ϳ������ظ��ɣı�ʾ��ͬ����
			    //nodelist = xmlDoc.documentElement.selectNodes("Receiver[@UserID=" + ActorID + " and @ReceiveType=" + RecType + " and @NodeID=" + lngNodeID + "]");
			    var path = "/Receivers/Receiver[@UserID=" + ActorID.toString() + "][@NodeID=" + lngNodeID + "][@ReceiveType=" + RecType + "]";
			    nodelist = getNodesByPath(path);
			}
		
			if(nodelist.length > 0)  //�û��ѱ�ѡ��
			{
				ExistUserRole=nodelist[0].getAttribute("ActorType");//��ѡ���û���ɫ
				switch(ExistUserRole)
				{
					case "Worker_": //����
						return false;
						break;

						
					case "Assist_": //Э��
						//�滻��ɫ

						if (RoleID=="Worker_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"����:" + strName+"("+strNodeName + ")");
						}
						else
							return false;
						break;
						
						
					case "Reader_": //��֪
						//�滻��ɫ					
						if (RoleID=="Worker_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"����:" + strName+"("+strNodeName + ")");
						}
						else if (RoleID=="Assist_")
						{
							nodelist[0].setAttribute("ActorType",RoleID);
							Set_ItemText_By_Value(ActorID,"Э��:" + strName+"("+strNodeName + ")");
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
				    if(strintSpecRightType=="esrtTransmit")   //����
				    {
				        //��ǰ���������д��ģ����ܴ��� 
				        var sreturn = GetajaxData(ActorID,"esrtTransmit");
				        if(sreturn=="true")
				        {
				            alert("�Ѵ�����֪�������Ҫ��ӣ�");
				            return;
				        }
				    }
				    else if(strintSpecRightType=="esrtAssist")   //Э��
				    {
				        //�籾��������Э�죬����Э�죻������������Э�챾����û��Э�죬����Э�죬�����ϱ�ע���������Э��
				        var sreturn = GetajaxData(ActorID,"esrtAssist");
				        if(sreturn=="1")    //���ڣ����ǵ�ǰ����
				        {
				            alert("�Ѵ���Э���������Ҫ��ӣ�");
				            return;
				        }
				        else if(sreturn=="2")  //���ڣ������ǵ�ǰ����
				        {
				            alert("�Ѵ���Э���������Ҫ��ӣ�");
				            return;
				        }
				    }
					document.all.lstSelected.add(new Option(getSenderName() + ":" + strName+"("+strNodeName + ")",ActorID));
				}
				else if (RoleID == "Reader_")
				{
					document.all.lstSelected.add(new Option("��֪:" + strName+" ("+strNodeName + ")",ActorID));
				}
				else
				{
					document.all.lstSelected.add(new Option("Э��:" + strName+" ("+strNodeName + ")",ActorID));
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
        //  alert("��ѡ���б����");  
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
					<TD vAlign="top" align="center" width="50%" colSpan="3"><FONT face="����">
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
                                        <asp:Label ID="lblMast" runat="server" Text="����"></asp:Label>
                                    </td>
                                    <td id="name1" onclick="chang_Class(this,3,1)" height="25" align="center" style="cursor: hand;
                                        width: 95;"  background="../images/lm-a.gif" runat="server" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                        Э��
                                    </td>
                                    <td width="95" id="name2" onclick="chang_Class(this,3,2)" height="25" align="center" class="list"
                                        style="cursor: hand;" background="../images/lm-a.gif" runat="server" onmouseover="chang(this,'lm-c.gif')" onmouseout="chang(this,'lm-a.gif')">
                                        ��֪
                                    </td>
                                    <td width="7" valign="top"><img src="../Images/lm-right.gif" width="7" height="29" /></td>
                                    <td align="right"  >
                                        <asp:RadioButtonList ID="ckblSelect" runat="server" RepeatDirection="Horizontal" onclick="ChangeTreeList();" >
                                            <asp:ListItem Selected="True" Value="0">����</asp:ListItem>
                                            <asp:ListItem Value="1">�б�</asp:ListItem>
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
                                    ����������<asp:TextBox ID="txtSelect" runat="server"  Width="99%" onblur="QueryPerson(this);" onkeypress="return CheckInfo(this);" ></asp:TextBox>
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
						<P><FONT face="����"><INPUT class="btnClass" id="cmdSelect"  style="visibility:hidden;"  onclick="AddToListButton();"
									type="button" value="ѡ��(>)"></FONT></P>
						<P><FONT face="����"><INPUT class="btnClass" id="cmdDelete"  onclick="RemoveItem();" type="button"
									value="�Ƴ�(<)"></FONT></P>
						<P><FONT face="����"><INPUT class="btnClass" id="cmdClear"  onclick="ClearItem();" type="button"
									value="���(<<)" name="cmdClear"></FONT></P>
					</TD>
					<TD vAlign="top" align="left" width="40%" colSpan="1" rowSpan="1">
					    <div  id="div_lstSelect" >
					    <SELECT id="lstSelected" runat="server" ondblclick="RemoveItem();" 
					            style="WIDTH: 100%; HEIGHT: 380px;" multiple
							size="22" name="lstSelect"></SELECT>
					    </div>
							<INPUT class="btnClass" id="cmdOK"  onclick="FlowSubmit();"
							type="button" value="ȷ ��"><FONT face="����">&nbsp; </FONT><INPUT class="btnClass" id="cmdCancel"  onclick="parent.fraaddNew.cols='0,100%,0';window.location='#';"
							type="button" value="ȡ ��"></TD>
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
