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
	public partial class flow_View_Chart : BasePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面

            long lngMessageID = long.Parse(Request.QueryString["MessageID"]);

            int intStatus = 0;
            string strFinishedList = "";
            long lngNMID = 0;
            long lngONMID = 0;
            int count = 0;

            long lngCurrNMID = 0;
            long lngFlowModelID = 0;
            long lngFlowID = 0;
            long lngNMMessageID =0;

            string strMsg = "";

            string strNodeList = "";  //正在处理的环节

            DataSet ds = Message.GetFlowInfo2009(lngMessageID);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lngNMID = long.Parse(ds.Tables[0].Rows[i]["NodeModelID"].ToString());
                lngFlowModelID = long.Parse(ds.Tables[0].Rows[i]["FlowModelID"].ToString());
                lngFlowID = long.Parse(ds.Tables[0].Rows[i]["FlowID"].ToString());
                lngNMMessageID = long.Parse(ds.Tables[0].Rows[i]["MessageID"].ToString());
                if (lngNMID != lngONMID)
                {
                    if (count != 0)
                    {
                        if (intStatus == (int)e_NodeStatus.ensFinished)
                        {
                            strFinishedList = strFinishedList + lngONMID.ToString() + ",";
                            //创建动态控件保存 MESSAGE
                            //Page.RegisterHiddenField("Msg" + lngONMID.ToString(), strMsg);
                        }
                    }
                    //strMsg = "环节最后的处理人员是" + ds.Tables[0].Rows[i]["Name"].ToString() +
                    //            " 并且" + GetNodeWorkTypeName(ds.Tables[0].Rows[i]["worktype"].ToString()) + "给了" + ds.Tables[0].Rows[i]["TActors"].ToString();
                    intStatus = int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
                    lngONMID = lngNMID;
                    count++;
                }
                else
                {
                    //strMsg = "环节最后的处理人员是" + ds.Tables[0].Rows[i]["Name"].ToString() +
                    //    " 并且" + GetNodeWorkTypeName(ds.Tables[0].Rows[i]["worktype"].ToString()) + "给了" + ds.Tables[0].Rows[i]["TActors"].ToString();
                    intStatus = int.Parse(ds.Tables[0].Rows[i]["Status"].ToString());
                }

                if (lngMessageID == lngNMMessageID)
                {
                    //取当前环节的模型ID
                    lngCurrNMID = lngNMID;
                }

                if (intStatus == 20)   //当前处理环节列表
                {
                    strNodeList += lngNMID.ToString() + ",";
                }

            }

            if (intStatus == (int)e_NodeStatus.ensFinished)
            {
                strFinishedList = strFinishedList + lngONMID.ToString() + ",";
                //Page.RegisterHiddenField("Msg" + lngONMID.ToString(), strMsg);

            }

            Page.RegisterHiddenField("hidFlowModelID", lngFlowModelID.ToString());
            Page.RegisterHiddenField("hidFlowID", lngFlowID.ToString());

            //前面一起获取了
            //lngCurrNMID = Message.GetCurrNodeModelID(lngMessageID);

            strFinishedList = strFinishedList.Replace(lngCurrNMID.ToString()+",", "");

            XmlDocument xmlDoc = new XmlDocument();

            //2008-02-10 修改实例时查看流程图 为支持缓存模式的方法
            //xmlDoc.LoadXml(FlowModel.GetFlowModelChart(lngMessageID));
            xmlDoc.LoadXml(FlowModel.GetFlowModelChartByFlowModel(lngFlowModelID));
		

		
			XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();

	
			XslTransform xmlXsl = new XslTransform();
			
			xmlXsl.Load(Server.MapPath("../xslt/FlowImageNew.xslt"));

			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddParam("FinishedNodeID","",strFinishedList);
			xslArg.AddParam("CurrNodeID","",lngCurrNMID);
            xslArg.AddParam("NodeList","", strNodeList);

			xmlXsl.Transform(nav,xslArg,Response.OutputStream);
		}


		private string GetNodeWorkTypeName(string strWorkType)
		{
			e_NodeWorkType nodeWorkType;
			if(strWorkType == "")
			{
				return "";
			}
			else
			{
				nodeWorkType = (e_NodeWorkType)int.Parse(strWorkType);
				string strRet = "";
				switch(nodeWorkType)
				{
					case e_NodeWorkType.enwtSkipTo:
						strRet = "转";
						break;
					case e_NodeWorkType.enwtTakeOver:
						strRet = "交接";
						break;
					case e_NodeWorkType.enwtSendBack:
						strRet = "退回";
						break;
					case e_NodeWorkType.enwtEnd:
						strRet = "结束";
						break;
					default:
						strRet = "发送";
						break;
				}
				return strRet;
			}
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
