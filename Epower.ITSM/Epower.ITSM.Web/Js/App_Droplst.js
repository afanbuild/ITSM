//Ӧ�ÿ����Ļ����ű���������ȡ�����б��������,2008-04-16 �� 
    var xmlDroplst = new ActiveXObject("Msxml2.DOMDocument.3.0");  //�ͻ���XML����
    var arrDroplstID;
    arrDroplstID = new Array();     //�����Ѿ�XMLHTTP��ȡ����ID
    var arrDroplstValue;
    arrDroplstValue = new Array();
    var iDroplstSavedCont = 0;
    var objOldValue = "";    //����ؼ�ԭ����ֵ
    var xmlhttpDroplst = null; 
    var currTextBoxID = "-1";
    var currDivID = "-1";
    var currOpendDivName = "";
    var oTimer;
    function SetInputCheckValid(id,idSelect,idText,idFields,obj)  
    {
        var args=[];
        var object = document.getElementById(id);
        var objSelect = document.getElementById(idSelect);
        var objText = document.getElementById(idText);
        args.push(id);
        args.push(object);
        args.push(objSelect);
        args.push(objText);
        args.push(idFields);
        args.push(objText);
        oTimer =  setInterval(function(){
                                AutoGetItems.apply(this,args);
                               },400);
        }
        
    function SetInputCheckDisable(){clearInterval(oTimer);}
    function AutoGetItems(id,object,objSelect,objText,idFields,obj)
    {
         var i = 0;
         var j=0;
         var sCurr = '';
         var soptText='';
         var blnAdd = false;
         var xmlDroplst = new ActiveXObject("Msxml2.DOMDocument.3.0");  //�ͻ���XML����
         if(objText != null){sCurr = objText.value;}
        //Ϊ�գ���� �˳�
        if(sCurr == "")
        {
              //�����������
            if(objSelect != null)
               objSelect.options.length = 0;  //�������
            return;
        }
        //ֵ��ͬ���˳�
        if(sCurr == objOldValue)
            return;
        objOldValue = sCurr;
        var sXml  = getXmlHttpFields(idFields + "_" + escape(sCurr));
        if(sXml != "-1")
        {
            //�ӻ����л�ȡ��
            if(object != null)
            {
                if(sXml=="")  //û��
		        {
		            xmlDroplst.loadXML("<Root></Root>");
	            }
	            else
	            {
                    xmlDroplst.loadXML(sXml);
                 }
                var nodes = xmlDroplst.documentElement.childNodes;
                if(objSelect != null)
                {
                   objSelect.options.length = 0;  //�������
                   for (i=0;i<nodes.length;i++)
                   {
                        soptText = nodes(i).getAttribute("Text");
                        //��̬��ȡ����������Ե������ǴӺ�̨ɸѡ����
                        objSelect.add(document.createElement("OPTION"));
                        objSelect.options[j].text= soptText;
                        objSelect.options[j].value=j; 
                        j++;
                    }
                }
                if(j >0)
                  {
                     //����ʱ��ʾ
                     object.style.left = absoluteLocation(obj, 'offsetLeft') - 2 + "px";       
                     object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
                     object.style.width = obj.offsetWidth + 2 + 'px';
                     hideMe(id,'');
                     currOpendDivName = id;
                  }
            }
        }
        else
        {
           if( sCurr != "")
           {
                //�첽��ȡ
                if(xmlhttpDroplst == null)
                     xmlhttpDroplst = CreateDroplstXmlHttpObject();       
                if(xmlhttpDroplst != null)
                {
                    try
                    {	
			            xmlhttpDroplst.open("GET", "../Common/frmXmlHttpDroplst.aspx?id=" + idFields + "&curr=" + escape(sCurr), true); 
			            //window.open("../Common/frmXmlHttpDroplst.aspx?id=" + idFields);
                        xmlhttpDroplst.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
			            xmlhttpDroplst.onreadystatechange = function() 
										            { 
											            if ( xmlhttpDroplst.readyState==4 ) 
											            {   
											                sXml = xmlhttpDroplst.responseText;
        											        
												            if(sXml=="")  //û��
												            {
												                xmlDroplst.loadXML("<Root></Root>");
											                }
											                else
											                {
											                    xmlDroplst.loadXML(sXml);
											                }
											                //������ν��
										                    arrDroplstID[iDroplstSavedCont] = idFields + "_" + escape(sCurr);
										                    arrDroplstValue[iDroplstSavedCont] = sXml;
										                    iDroplstSavedCont++;
											                if(object != null)
                                                            {
                                                                var nodes = xmlDroplst.documentElement.childNodes;
                                                                if(objSelect != null)
                                                                {
                                                                   objSelect.options.length = 0;  //�������
                                                                   for (i=0;i<nodes.length;i++)
                                                                   {
                                                                        soptText = nodes(i).getAttribute("Text");
                                                                        //��̬��ȡ����������Ե������ǴӺ�̨ɸѡ����
                                                                        objSelect.add(document.createElement("OPTION"));
                                                                        objSelect.options[j].text= soptText;
                                                                        objSelect.options[j].value=j; 
                                                                        j++;
                                                                        
                                                                    }
                                                                }
                                                                if(j >0)
                                                                  {
                                                                     //����ʱ��ʾ
                                                                     object.style.left = absoluteLocation(obj, 'offsetLeft') - 2 + "px";       
                                                                     object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
                                                                     object.style.width = obj.offsetWidth + 2 + 'px';
                                                                     hideMe(id,'');
                                                                  }
											                }
        											        
                                                         }
                                                    }   
                          xmlhttpDroplst.send(null);   
                     }
                     catch(e3)
                     {
                     }
               }
           }
       }
    }
    
    setInterval("AutoCloseDiv()",3000);
    function setInputID(idText){currTextBoxID = idText; }
    function setInputIDNone(){currTextBoxID = "-1"; }
    function AutoCloseDiv()
    {
        if(currTextBoxID == "-1" && currDivID == "-1")
        {
            try{hideMe(currOpendDivName,"none");}catch(e3){}
        }
    }
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
    function focusToDropDown(idSelect)
    {
         if(event.keyCode == 40)
         {
            var objSelect = document.getElementById(idSelect);
            if(objSelect != null)
            {   try
                {
                   //�пؼ����ɼ������
                    objSelect.focus();
                    if(objSelect.lenght > 0)
                        objSelect.options[0].selected = true;
                 }
                 catch(e1)
                 {
                 }
            }
         }
    }
    function MoverToDropDownLayer(idSelect)
    {
            var objSelect = document.getElementById(idSelect);
            if(objSelect != null)
            {   
                 try
                {
                   //�пؼ����ɼ������
                    objSelect.focus();
                    if(objSelect.selectedIndex == -1)
                        objSelect.selectedIndex = 0;
                        
                    currDivID = idSelect;
                }
                 catch(e1)
                 {
                 }
            }
    }
    function absoluteLocation(element, offset) 
    { var c = 0; while (element) {  c += element[offset];  element = element.offsetParent; } return c; 
    } 
    function getSelectedLabel(id,idText,obj) 
    {  
         var opt = obj.options[obj.selectedIndex]; 
         hideMe(id,'none'); 
         var objText = document.getElementById(idText);
        if(objText != null)
        {
            objText.value = getIdiomDeal(opt.text);
        }
    } 
    function getIdiomDeal(s)
    {
        var sRet = s;
        if(typeof(getUserIdiomDeal) !="undefined")
        {
            sRet = getUserIdiomDeal(s);
        }
        return sRet;
    }
    function selectOnReturn(id,idText,obj)
    {
       if(event.keyCode == 8)
          event.keyCode = null;
       if(event.keyCode==13)
       {
            event.returnValue = false;
            getSelectedLabel(id,idText,obj);
        }
    }
    function hideMe(id,status)
    {
        var object = document.getElementById(id);
        if(object != null)
        {
            object.style.display = status;
            if(status == "none")
            {
                currDivID = "-1";
                currTextBoxID = "-1";
            }
        }
    }
    function getItemsForDropdown(id,idSelect,idText,idFields,obj)
    {
         var object = document.getElementById(id);
         var objSelect = document.getElementById(idSelect);
          var objText = document.getElementById(idText);
          var objFields = document.getElementById(idFields);
         var i = 0;
         var j=0;
         var sCurr = '';
         var soptText='';
         var blnAdd = false;
         
         if(objFields != null)
         {  var sXml = objFields.value;
            xmlDroplst.loadXML(sXml);
         }
         if(objText != null){ sCurr = objText.value; }
        if(object != null)
        {
            var nodes = xmlDroplst.documentElement.childNodes;
            if(objSelect != null)
            {
               objSelect.options.length = 0;  //�������
               for (i=0;i<nodes.length;i++)
               {
                    soptText = nodes(i).getAttribute("Text");
                    blnAdd = false;
                    if(sCurr == '')
                    {
                       blnAdd = true;
                    }
                    else
                    {
                       if(soptText.toUpperCase().indexOf(sCurr.toUpperCase()) == 0)
                         {
                            blnAdd=true;
                         }
                    }
                    if(blnAdd == true)
                    {
                        objSelect.add(document.createElement("OPTION"));
                        objSelect.options[j].text= soptText;
                        objSelect.options[j].value=j; 
                        j++;
                        
                    }
                }
            }
            object.style.left = absoluteLocation(obj, 'offsetLeft') - 2 + "px";       
            object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
            object.style.width = obj.offsetWidth + 2 + 'px';
            hideMe(id,'');
            currOpendDivName = id;
        }
    }
    //��Դ��XMLHTTP�첽��ȡ,***δ��***
    function getItemsForDropdownXmlHttp(id,idSelect,idText,idFields,obj)
    {
         var object = document.getElementById(id);
         var objSelect = document.getElementById(idSelect);
          var objText = document.getElementById(idText);
         var i = 0;
         var j=0;
         var sCurr = '';
         var soptText='';
         var blnAdd = false;
         var xmlDroplst = new ActiveXObject("Msxml2.DOMDocument.3.0");  //�ͻ���XML����
         
         if(objText != null){sCurr = objText.value;}
        var sXml  = getXmlHttpFields(idFields);
        if(sXml != "-1")
        {
            //�ӻ����л�ȡ��
            if(object != null)
            {
                if(sXml=="")  //û��
		        {
		            xmlDroplst.loadXML("<Root></Root>");
	            }
	            else
	            {
                    xmlDroplst.loadXML(sXml);
                 }
                var nodes = xmlDroplst.documentElement.childNodes;
                if(objSelect != null)
                {
                   objSelect.options.length = 0;  //�������
                   for (i=0;i<nodes.length;i++)
                   {
                        soptText = nodes(i).getAttribute("Text");
                        blnAdd = false;
                        if(sCurr == '')
                        {
                           blnAdd = true;
                        }
                        else
                        {
                           if(soptText.toUpperCase().indexOf(sCurr.toUpperCase()) == 0)
                             {
                                blnAdd=true;
                             }
                        }
                        if(blnAdd == true)
                        {
                            objSelect.add(document.createElement("OPTION"));
                            objSelect.options[j].text= soptText;
                            objSelect.options[j].value=j;  
                            //objSelect.options[j].className = "combo-item";
                            j++;
                        }
                    }
                }
                if(j >0)
                  {
                     //����ʱ��ʾ
                     object.style.left = absoluteLocation(obj, 'offsetLeft') - 2 + "px";       
                     object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
                     object.style.width = obj.offsetWidth + 2 + 'px';
                     hideMe(id,'');
                     currOpendDivName = id;
                  }
            }
        }
        else
        {
            //�첽��ȡ
            if(xmlhttpDroplst == null)
                 xmlhttpDroplst = CreateDroplstXmlHttpObject();       
            if(xmlhttpDroplst != null)
            {
                try
                {	
			        xmlhttpDroplst.open("GET", "../Common/frmXmlHttpDroplst.aspx?id=" + idFields, true); 
			        //window.open("../Common/frmXmlHttpDroplst.aspx?id=" + idFields);
                    xmlhttpDroplst.setRequestHeader( "CONTENT-TYPE ", "application/x-www-form-urlencoded "); 
			        xmlhttpDroplst.onreadystatechange = function() 
										        { 
											        if ( xmlhttpDroplst.readyState==4 ) 
											        {   
											            sXml = xmlhttpDroplst.responseText;
    											        
												        if(sXml=="")  //û��
												        {
												            xmlDroplst.loadXML("<Root></Root>");
											            }
											            else
											            {
											                xmlDroplst.loadXML(sXml);
											                
											            }
											            //������ν��
										                arrDroplstID[iDroplstSavedCont] = idFields;
										                arrDroplstValue[iDroplstSavedCont] = sXml;
										                iDroplstSavedCont++;
											            if(object != null)
                                                        {
                                                            var nodes = xmlDroplst.documentElement.childNodes;
                                                            if(objSelect != null)
                                                            {
                                                               objSelect.options.length = 0;  //�������
                                                               for (i=0;i<nodes.length;i++)
                                                               {
                                                                    soptText = nodes(i).getAttribute("Text");
                                                                    blnAdd = false;
                                                                    if(sCurr == '')
                                                                    {
                                                                       blnAdd = true;
                                                                    }
                                                                    else
                                                                    {
                                                                       if(soptText.toUpperCase().indexOf(sCurr.toUpperCase()) == 0)
                                                                         {
                                                                            blnAdd=true;
                                                                         }
                                                                    }
                                                                    if(blnAdd == true)
                                                                    {
                                                                        objSelect.add(document.createElement("OPTION"));
                                                                        objSelect.options[j].text= soptText;
                                                                        objSelect.options[j].value=j; 
                                                                        j++;
                                                                    }
                                                                }

                                                            }
                                                            if(j >0)
                                                              {
                                                                 //����ʱ��ʾ
                                                                 object.style.left = absoluteLocation(obj, 'offsetLeft') - 2 + "px";       
                                                                 object.style.top = absoluteLocation(obj, 'offsetTop') + obj.offsetHeight + 2 + "px"; 
                                                                 object.style.width = obj.offsetWidth + 2 + 'px';
                                                                 hideMe(id,'');
                                                              }
											            }
                                                     }
                                                }   
                      xmlhttpDroplst.send(null);   
                 }
                 catch(e3)
                 {
                 }
           }
       }
    }
    //ͨ�����������ȡ����ѡ���ֵ
    function getXmlHttpFields(idFields)
    {
        var respText = "-1";
        var i = 0;
        for(i=0;i<iDroplstSavedCont;i++)
        {
            if(idFields == arrDroplstID[i])
            {
               respText = arrDroplstValue[i];
            }
        }
        return respText;
  }