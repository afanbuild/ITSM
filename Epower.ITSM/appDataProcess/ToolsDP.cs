using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using MyComponent;
using System.Xml;
using System.IO;
using System.Collections;

namespace appDataProcess
{
	/// <summary>
	/// ToolsDP 的摘要说明。
	/// </summary>
	public class ToolsDP
	{
		public ToolsDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//                2
		}

		/// <summary>
		/// 流程发送处理通知接口(在流程发送/新增处理的提交前执行)
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID"></param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strXMlFieldValue"></param>
		/// <param name="strReceivers">接收者和消息值列字符串 格式: 接收者ID|消息ID,接收者ID|消息ID,... (仅主办的消息)</param>
		/// <param name="lngMessageID"></param>
		public static  void NotifyMessage(OracleTransaction trans,long lngID,long lngNodeModelID,long lngFlowModelID,long lngOpID,string strXMlFieldValue,string strReceivers,long lngMessageID)
		{
			//根据 strReceivers的值 存储到 OA_EmailNotify表中去 
			// ID 为GUID值 ,存储 ReceiverID,MessageID
			if(strReceivers.Length == 0)
			{
				return;
			}

			string strSQL="";

			string strFlowName="";

			string strGUID = "";
			

			string strTmp="";

			string strMessageID = "0";
			string strReceiverID = "0";

			int iEmailNotify = 0;

			

			XmlTextReader tr = new XmlTextReader(new StringReader(strXMlFieldValue));
			while(tr.Read())
			{
				if(tr.Name=="Field" && tr.NodeType == XmlNodeType.Element)
				{
					strTmp = tr.GetAttribute("Value").Trim();
					switch(tr.GetAttribute("FieldName"))
					{

						case "FlowName":
							strFlowName = strTmp;
							break;
						case "EmailNotify":
							iEmailNotify = int.Parse(strTmp);
							break;

						default:
							break;
					}
				}
			}

			tr.Close();

			if(iEmailNotify == 0)
			{
				//用户端不通知
				return;
			}

			

			

			string[] arrMsgs = strReceivers.Split(new char[]{','});

            for (int i = 0; i < arrMsgs.Length; i++)
            {

                Guid g = Guid.NewGuid();

                strGUID = g.ToString();

                strReceiverID = arrMsgs[i].Substring(0, arrMsgs[i].IndexOf("|"));
                strMessageID = arrMsgs[i].Substring(arrMsgs[i].IndexOf("|") + 1);

                //保存到数据库

                strSQL = "INSERT oa_emailnotify(id,messageid,receiverid,subject,msgdate,status) VALUES(" +
                    MyGlobalString.SqlQ(strGUID) + "," +
                    strMessageID + "," +
                    strReceiverID + "," +
                    MyGlobalString.SqlQ(strFlowName) + ",sysdate,0)";

                OracleDbHelper.ExecuteNonQuery(trans, strSQL);

            }





		}
	}
}
