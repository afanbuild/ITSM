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
	/// OA_AddNew ��ժҪ˵����
	/// </summary>
	public partial class OA_AddNew : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string strExtendParameter="";


			long lngFlowModelID = long.Parse(Request.QueryString["flowmodelid"]);

            if (Request.QueryString["IsFirst"] != null)
            {
                //���FlowModelID����ΪԭʼFlowModelID,���ȡ���°汾�ı��
                if (Request.QueryString["IsFirst"] == "true")
                {
                    long OlngFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);

                    //lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngFlowModelID);                    
                    lngFlowModelID = FlowModel.GetLastVersionFlowModelID(OlngFlowModelID);                    
                }
            }

            //����ǵ����´���,���������ϵ��˳�,ͳһִ�йرմ���
            if (Request.QueryString["NewWin"] != null)
            {
                if (Request.QueryString["NewWin"].ToLower() == "true")
                    Session["FromUrl"] = "close";
            }

			if(Request.QueryString["ep"]!=null)	
				strExtendParameter=this.Request.QueryString["ep"];
			else
				strExtendParameter="";

			long lngPreMessageID = 0;   //����ǰ����ϢID
			if(Request.QueryString["PreMessageID"]!=null)	
				lngPreMessageID=long.Parse(Request.QueryString["PreMessageID"]);
			
			int iFlowJoinType = 10;   //���̽������
			if(Request.QueryString["FlowJoinType"]!=null)	
				iFlowJoinType=int.Parse(Request.QueryString["FlowJoinType"]);
			
			long lngUserID = (long)Session["UserID"];

			Session["FlowModelID"] = lngFlowModelID;
			Session["ExtendParameter"]=strExtendParameter;
			Session["PreMessageID"] = lngPreMessageID;
			Session["FlowJoinType"] = iFlowJoinType;
            Session["SaveKBMessionId"] = strExtendParameter;


			//�ж��Ƿ������������
			int intCanStart= FlowModel.CanUseFlowModel(lngUserID,lngFlowModelID);



			if(intCanStart == 0)
			{
				

				Response.Redirect("OA_AddNew.htm");
			}
			else
			{
				if(intCanStart == -2)
				{
					PageTool.MsgBox(this,"��û��Ȩ��ִ�д˲���������ϵͳ������Ա��ϵ");
				}
				else
				{
					PageTool.MsgBox(this,"����ִ�е�����ģ����ʱ��Ч������ϵͳ������Ա��ϵ");
				}
				PageTool.SelfClose(this);
			}
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
