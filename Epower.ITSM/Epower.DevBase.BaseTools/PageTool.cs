using System;
using System.Web.UI;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// 页面处理的一些方法。
	/// </summary>
	public class PageTool
	{
        /// <summary>
        /// 
        /// </summary>
		public PageTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="page"></param>
		public static void SelfClose(System.Web.UI.Page page)
		{
			page.Response.Write(@"<script>self.close();</script>");
		}

		/// <summary>
		/// 客户端提示信息
		/// </summary>
		/// <param name="page"></param>
		/// <param name="strMsg"></param>
		public static void MsgBox(System.Web.UI.Page page,string strMsg)
		{
            strMsg = strMsg.Replace("\r\n", ",");
            if (strMsg.EndsWith(Environment.NewLine))
            {
                strMsg = strMsg.Substring(0, strMsg.Length - 1);
            }
			page.Response.Write(@"<script language='javascript'>alert(" + StringTool.JavaScriptQ(strMsg) + "); </script> ");
			//page.Response.Write(@"<script language='javascript'>alert('" + strMsg + "'); </script> ");
		}

		// duanqs [4/11/2005]
		/// <summary>
		/// 在服务端控制打开一个新窗口
		/// </summary>
		/// <param name="page"></param>
		/// <param name="strUrl"></param>
		public static void OpenWindow(System.Web.UI.Page page,string strUrl)
		{
			page.Response.Write(@"<script language='javascript'>window.open("+StringTool.JavaScriptQ(strUrl)+")</script>");
		}

		// duanqs [4/11/2005]
		/// <summary>
		/// 在服务端控制打开一个新窗口
		/// </summary>
		/// <param name="page"></param>
		/// <param name="strUrl"></param>
		public static void OpenDialog(System.Web.UI.Page page,string strUrl)
		{
			page.Response.Write(@"<script language='javascript'>window.showModelessDialog("+StringTool.JavaScriptQ(strUrl)+")</script>");
		}
		/// <summary>
        /// 添加隐含字段 在控件上
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="strFieldName"></param>
		/// <param name="strValue"></param>
		public static void AddHiddenField(HtmlTextWriter writer,string strFieldName,string strValue)
		{
			writer.Write("<INPUT id='" + strFieldName + "' style='Z-INDEX: 107; LEFT: 213px; POSITION: absolute; TOP: 67px' type='hidden' value=\"" + strValue + "\" name='" + strFieldName + "'>");
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="strFieldName"></param>
        /// <param name="lngValue"></param>
		public static void AddHiddenField(HtmlTextWriter writer,string strFieldName,long lngValue)
		{
			AddHiddenField(writer,strFieldName,lngValue.ToString());
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="strFieldName"></param>
		public static void AddHiddenField(HtmlTextWriter writer,string strFieldName)
		{
			AddHiddenField(writer,strFieldName,"");
		}


        /// <summary>
        ///  添加JAVESCRIP脚本
        /// </summary>
        /// <param name="page"></param>
        /// <param name="strScript"></param>
		public static void AddJavaScript(System.Web.UI.Page page,string strScript)
		{
			page.Response.Write(@"<script language='javascript' type='text/javascript'>"+strScript+ "</script> ");
		}


	}
}
