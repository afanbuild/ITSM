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
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// flow_View_Chart 的摘要说明。
	/// </summary>
	public partial class flow_SelectNode : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

			long lngMessageID = long.Parse(Request.QueryString["MessageID"]);

            int iType = 0;  //0表示跳转  1 驳回

            if (Request.QueryString["iType"] != null)
                iType = int.Parse(Request.QueryString["iType"]);

            if (iType == 1)
                Page.Title = "选择驳回的环节";

			int intStatus=0;
			string strFinishedList ="";
			long lngNMID=0;
			long lngONMID=0;
			int count = 0;

			long lngCurrNMID =0;
            long lngNMMessageID = 0;
            long lngFlowModelID = 0;
            long lngFlowID = 0;

            int lngCurrPathID = -1;   //用于驳回时路径判断

			string strMsg="";

			


			
			//获取已完成环节模型列表
            DataSet ds = Message.GetFlowInfo2009(lngMessageID);

			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				lngNMID = long.Parse(ds.Tables[0].Rows[i]["NodeModelID"].ToString());
                lngNMMessageID = long.Parse(ds.Tables[0].Rows[i]["MessageID"].ToString());
                lngFlowModelID = long.Parse(ds.Tables[0].Rows[i]["FlowModelID"].ToString());
                lngFlowID = long.Parse(ds.Tables[0].Rows[i]["FlowID"].ToString());
				if(lngNMID != lngONMID)
				{
					if(count != 0)
					{
						if(intStatus == (int)e_NodeStatus.ensFinished)
						{
							strFinishedList = strFinishedList + lngONMID.ToString() + ",";
							//创建动态控件保存 MESSAGE
							//Page.RegisterHiddenField("Msg" + lngONMID.ToString(),strMsg);
						}
					}

					intStatus=int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
					lngONMID = lngNMID;
					count++;
				}
				else
				{
					//strMsg = "环节最后的处理人员是" + ds.Tables[0].Rows[i]["Name"].ToString() +
					//	" 并且发送给了" + ds.Tables[0].Rows[i]["TActors"].ToString();
					intStatus=int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
				}

                if (lngMessageID == lngNMMessageID)
                {
                    //取当前环节的模型ID
                    lngCurrNMID = lngNMID;
                }

				
			}

            


			if(intStatus == (int)e_NodeStatus.ensFinished)
			{
				strFinishedList = strFinishedList + lngONMID.ToString() + ",";
				//Page.RegisterHiddenField("Msg" + lngONMID.ToString(),strMsg);

			}

            //前面一起获取了
			//lngCurrNMID = Message.GetCurrNodeModelID(lngMessageID);

            strFinishedList = strFinishedList.Replace(lngCurrNMID.ToString() + ",", "");

            //保存能跳转环节的列表在隐含字段
            string strCanJumpNList = "0";
            if (iType == 0)
            {
                strCanJumpNList = Message.GetCanJumpNodeList2009(lngFlowModelID, lngCurrNMID);
            }
            else
            {
                if (lngFlowModelID != 0 && lngCurrNMID != 0)
                {
                    ObjNodeModel onm = new ObjNodeModel(lngFlowModelID, lngCurrNMID);
                    lngCurrPathID = onm.PathID;
                }
                //这里返回的是 1|2,2|2,...   [环节模型ID]|[路径ID]，跟跳转不同
                strCanJumpNList = Message.GetCanTakeBackHasDoneNodeList2009(lngFlowModelID, lngCurrNMID,strFinishedList);
            }

            Page.RegisterHiddenField("hidJumpType", iType.ToString());
            Page.RegisterHiddenField("CanJumpNodeList", strCanJumpNList);
            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());
            Page.RegisterHiddenField("hidFlowID", lngFlowID.ToString());

            Page.RegisterHiddenField("hidPahtID", lngCurrPathID.ToString());  //仅用于驳回

            Page.RegisterHiddenField("MsgTitle1", "确定" + (iType == 0 ? "跳转" : "驳回") + "到　");
            Page.RegisterHiddenField("MsgTitle2", "　环节？");

			XmlDocument xmlDoc = new XmlDocument();
			
			xmlDoc.LoadXml(FlowModel.GetFlowModelChart(lngMessageID));

		
			XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

	
			XslTransform xmlXsl = new XslTransform();
            
            if (iType == 0)
            {
                tb001.Visible = false;
                xmlXsl.Load(Server.MapPath("../xslt/FlowNodeSelectNew.xslt"));
            }
            else
            {
                xmlXsl.Load(Server.MapPath("../xslt/FlowNodeSelectHasDone.xslt"));
            }
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddParam("FinishedNodeID","",strFinishedList);
			xslArg.AddParam("CurrNodeID","",lngCurrNMID);

			xmlXsl.Transform(nav,xslArg,Response.OutputStream);
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
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
