
 var checkSubmitFlg = false;    
 function checkSubmit()
 { 
	
      if (typeof(Page_ValidationActive) != "undefined")
      {        
        if (Page_ValidationActive)             
        { 
          checkSubmitFlg = !Page_IsValid;
          if (checkSubmitFlg == true)
          {
            return false;           
          }          
          return true;            
        }
      }
      else
      {
        if (checkSubmitFlg == true)
        {
			
            return false;
        }

        checkSubmitFlg = true;
        return true;
      }
  }
  
document.ondblclick = 
   function docondblclick()
   {
     window.event.returnValue = false;              
   }
   
document.onclick =
   function doconclick()
   {
      if (typeof(Page_ValidationActive) != "undefined")
      {
        checkSubmitFlg = !Page_IsValid;        
      }
      if (checkSubmitFlg)
	  {
         window.event.returnValue = false;
      }      
   }

