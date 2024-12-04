using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EpowerCom;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// OA_AddNew 的摘要说明。
	/// </summary>
	public partial class OA_AddNew : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strExtendParameter="";


			long lngFlowModelID = long.Parse(Request.QueryString["flowmodelid"]);

            if (Request.QueryString["IsFirst"] != null)
            {
                //如果FlowModelID参数为原始FlowModelID,则获取最新版本的编号
                if (Request.QueryString["IsFirst"] == "true")
                {
                    long OlngFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);

                    //lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngFlowModelID);                    
                    lngFlowModelID = FlowModel.GetLastVersionFlowModelID(OlngFlowModelID);                    
                }
            }

            //如果是弹出新窗口,则工作流表单上的退出,统一执行关闭窗口
            if (Request.QueryString["NewWin"] != null)
            {
                if (Request.QueryString["NewWin"].ToLower() == "true")
                    Session["FromUrl"] = "close";
            }

			if(Request.QueryString["ep"]!=null)	
				strExtendParameter=this.Request.QueryString["ep"];
			else
				strExtendParameter="";

			long lngPreMessageID = 0;   //流程前置消息ID
			if(Request.QueryString["PreMessageID"]!=null)	
				lngPreMessageID=long.Parse(Request.QueryString["PreMessageID"]);
			
			int iFlowJoinType = 10;   //流程接入类别
			if(Request.QueryString["FlowJoinType"]!=null)	
				iFlowJoinType=int.Parse(Request.QueryString["FlowJoinType"]);
			
			long lngUserID = (long)Session["UserID"];

			Session["FlowModelID"] = lngFlowModelID;
			Session["ExtendParameter"]=strExtendParameter;
			Session["PreMessageID"] = lngPreMessageID;
			Session["FlowJoinType"] = iFlowJoinType;
            Session["SaveKBMessionId"] = strExtendParameter;


			//判断是否可以启动流程
			int intCanStart= FlowModel.CanUseFlowModel(lngUserID,lngFlowModelID);



			if(intCanStart == 0)
			{
				

				Response.Redirect("OA_AddNew.htm");
			}
			else
			{
				if(intCanStart == -2)
				{
					PageTool.MsgBox(this,"您没有权限执行此操作！请与系统管理人员联系");
				}
				else
				{
					PageTool.MsgBox(this,"您所执行的流程模型暂时无效！请与系统管理人员联系");
				}
				PageTool.SelfClose(this);
			}
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
