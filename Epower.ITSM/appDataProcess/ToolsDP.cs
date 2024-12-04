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
	/// ToolsDP ��ժҪ˵����
	/// </summary>
	public class ToolsDP
	{
		public ToolsDP()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//                2
		}

		/// <summary>
		/// ���̷��ʹ���֪ͨ�ӿ�(�����̷���/����������ύǰִ��)
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID"></param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strXMlFieldValue"></param>
		/// <param name="strReceivers">�����ߺ���Ϣֵ���ַ��� ��ʽ: ������ID|��ϢID,������ID|��ϢID,... (���������Ϣ)</param>
		/// <param name="lngMessageID"></param>
		public static  void NotifyMessage(OracleTransaction trans,long lngID,long lngNodeModelID,long lngFlowModelID,long lngOpID,string strXMlFieldValue,string strReceivers,long lngMessageID)
		{
			//���� strReceivers��ֵ �洢�� OA_EmailNotify����ȥ 
			// ID ΪGUIDֵ ,�洢 ReceiverID,MessageID
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
				//�û��˲�֪ͨ
				return;
			}

			

			

			string[] arrMsgs = strReceivers.Split(new char[]{','});

            for (int i = 0; i < arrMsgs.Length; i++)
            {

                Guid g = Guid.NewGuid();

                strGUID = g.ToString();

                strReceiverID = arrMsgs[i].Substring(0, arrMsgs[i].IndexOf("|"));
                strMessageID = arrMsgs[i].Substring(arrMsgs[i].IndexOf("|") + 1);

                //���浽���ݿ�

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
