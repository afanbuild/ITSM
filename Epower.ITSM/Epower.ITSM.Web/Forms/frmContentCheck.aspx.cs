using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.ITSM.SqlDAL;
using EpowerCom;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmContentCheck 的摘要说明。
	/// </summary>
	public partial class frmContentCheck : BasePage
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            long lngUserID = (long)Session["UserID"];
            string sLastMessageID = "0,0,0";  //最新的待办消息ID，待接收的消息ID，短消息ID
            if (Request.Params["LastMessageID"] != null)
                sLastMessageID = Request.QueryString["LastMessageID"];

            int iCheckType = 0;
            if (Request.Params["CheckType"] != null)
                iCheckType = int.Parse(Request.QueryString["CheckType"]);
            if (iCheckType == 0)
            {
                sLastMessageID = "0";
                if (Request.Params["LastMessageID"] != null)
                    sLastMessageID = Request.QueryString["LastMessageID"];

                int blnHas = FlowDP.CheckNewMessageItems(long.Parse(sLastMessageID), lngUserID);
                Response.Clear();
                //返回是否有新增 0 无, 1 有
                Response.Write(blnHas.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {

                //获取最新的消息情况
                string strXml = GetNoticeMessageXml(lngUserID, sLastMessageID);
                //string strXml = GetNoticeMessageXml(lngUserID, 0);
                Response.Clear();
                //返回是否有新增 0 无, 1 有
                Response.Write(strXml);
                Response.Flush();
                Response.End();
            }
        }

        /// <summary>
        /// 获取OA信使通知方式的XML串
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="sLastMessageID"></param>
        /// <returns></returns>
        private string GetNoticeMessageXml(long lngUserID, string sLastMessageID)
        {
            string strXml = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            string[] sIDArr = sLastMessageID.Split(',');    //最新ID分别取出
            DataTable dt = FlowDP.GetUndoMessage(lngUserID, long.Parse(sIDArr[0]));
            if (dt.Rows.Count > 0)    //待办事项
            {
                XmlElement xmlRoot = xmlDoc.CreateElement("Messages");
                XmlElement xmlEle;
                xmlRoot.SetAttribute("undo", MessageCollectionDP.GetUndoMessageCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unreader", MessageCollectionDP.GetUnReadMessageCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unreceive", ReceiveList.GetReceiveMessageListCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unsysmessage", SMSDp.GetSMSReadCount(lngUserID));
                foreach (DataRow row in dt.Rows)
                {
                    xmlEle = xmlDoc.CreateElement("Message");
                    xmlEle.SetAttribute("MessageID", row["MessageID"].ToString() + "," + sIDArr[1] + "," + sIDArr[2]);
                    xmlEle.SetAttribute("FActors", row["FActors"].ToString());
                    xmlEle.SetAttribute("ReceiveTime", row["ReceiveTime"].ToString());
                    xmlEle.SetAttribute("actortype", row["actortype"].ToString());
                    xmlEle.SetAttribute("Subject", row["Subject"].ToString());
                    xmlEle.SetAttribute("isRec", row["isRec"].ToString());
                    xmlRoot.AppendChild(xmlEle);
                }
                xmlDoc.AppendChild(xmlRoot);
                strXml = xmlDoc.InnerXml;
            }
            else
            {
                DataTable dtrev = FlowDP.GetUnRecMessage(lngUserID, long.Parse(sIDArr[1]));
                if (dtrev.Rows.Count > 0)    //待接收事件
                {
                    XmlElement xmlRoot = xmlDoc.CreateElement("Messages");
                    XmlElement xmlEle;
                    xmlRoot.SetAttribute("undo", MessageCollectionDP.GetUndoMessageCount(lngUserID).ToString());
                    xmlRoot.SetAttribute("unreader", MessageCollectionDP.GetUnReadMessageCount(lngUserID).ToString());
                    xmlRoot.SetAttribute("unreceive", ReceiveList.GetReceiveMessageListCount(lngUserID).ToString());
                    xmlRoot.SetAttribute("unsysmessage", SMSDp.GetSMSReadCount(lngUserID));
                    foreach (DataRow row in dtrev.Rows)
                    {
                        xmlEle = xmlDoc.CreateElement("Message");
                        xmlEle.SetAttribute("MessageID", sIDArr[0] + "," + row["MessageID"].ToString() + "," + sIDArr[2]);
                        xmlEle.SetAttribute("FActors", row["FActors"].ToString());
                        xmlEle.SetAttribute("ReceiveTime", row["ReceiveTime"].ToString());
                        xmlEle.SetAttribute("actortype", row["actortype"].ToString());
                        xmlEle.SetAttribute("Subject", row["Subject"].ToString());
                        xmlEle.SetAttribute("isRec", row["isRec"].ToString());
                        xmlRoot.AppendChild(xmlEle);
                    }
                    xmlDoc.AppendChild(xmlRoot);
                    strXml = xmlDoc.InnerXml;
                } 
                else      //短消息
                {
                    string sWhere = " And smsid>" + long.Parse(sIDArr[2]);
                    DataTable dtMessage = SMSDp.GetSMS(lngUserID, 1, sWhere,0,string.Empty,string.Empty);
                    if (dtMessage.Rows.Count > 0)
                    {
                        XmlElement xmlRoot = xmlDoc.CreateElement("Messages");
                        XmlElement xmlEle;
                        xmlRoot.SetAttribute("undo", MessageCollectionDP.GetUndoMessageCount(lngUserID).ToString());
                        xmlRoot.SetAttribute("unreader", MessageCollectionDP.GetUnReadMessageCount(lngUserID).ToString());
                        xmlRoot.SetAttribute("unreceive", ReceiveList.GetReceiveMessageListCount(lngUserID).ToString());
                        xmlRoot.SetAttribute("unsysmessage", SMSDp.GetSMSReadCount(lngUserID));

                        foreach (DataRow row in dtMessage.Rows)
                        {
                            xmlEle = xmlDoc.CreateElement("Message");
                            xmlEle.SetAttribute("MessageID", sIDArr[0] + "," + sIDArr[1] + "," + row["smsid"].ToString());
                            xmlEle.SetAttribute("FActors", row["SenderName"].ToString());
                            xmlEle.SetAttribute("ReceiveTime", row["SendTime"].ToString());
                            xmlEle.SetAttribute("actortype", "0");
                            xmlEle.SetAttribute("Subject", row["Content"].ToString());
                            xmlEle.SetAttribute("isRec", "0");
                            xmlRoot.AppendChild(xmlEle);
                        }
                        xmlDoc.AppendChild(xmlRoot);
                        strXml = xmlDoc.InnerXml;
                    }
                }
            }
            if (strXml == string.Empty)
            {
                XmlElement xmlRoot = xmlDoc.CreateElement("Messages");
                XmlElement xmlEle;
                xmlRoot.SetAttribute("undo", MessageCollectionDP.GetUndoMessageCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unreader", MessageCollectionDP.GetUnReadMessageCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unreceive", ReceiveList.GetReceiveMessageListCount(lngUserID).ToString());
                xmlRoot.SetAttribute("unsysmessage", SMSDp.GetSMSReadCount(lngUserID));

                xmlEle = xmlDoc.CreateElement("Message");
                xmlEle.SetAttribute("MessageID", sIDArr[0] + "," + sIDArr[1] + "," + sIDArr[2]);
                xmlEle.SetAttribute("FActors", "");
                xmlEle.SetAttribute("ReceiveTime", "");
                xmlEle.SetAttribute("actortype", "0");
                xmlEle.SetAttribute("Subject", "");
                xmlEle.SetAttribute("isRec", "0");
                xmlRoot.AppendChild(xmlEle);

                xmlDoc.AppendChild(xmlRoot);
                strXml = xmlDoc.InnerXml;
            }
            return strXml;

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
