using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using System.Data;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 微信相关的一些公用方法
    /// </summary>
    public class WeiXinDP
    {
        /// <summary>
        /// 获取下一个环节的相关处理按钮列表，拼接成字符串返回
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID">当前消息ID</param>
        /// <param name="strReceivers">接收人员列表</param>
        /// <param name="lngFlowModelID">流程模型ID</param>
        /// <param name="lngFlowID">流程ID</param>
        /// <returns></returns>
        public static string getActionBtnList(OracleTransaction trans, long lngMessageID, string strReceivers, long lngFlowModelID, long lngFlowID)
        {
            //得到网页链接地址
            string httpURL = ConfigHelper.GetParameterValue("SystemConfig", "weixinURL") + "weixin/frmweixinsender.aspx";

            string strContent = "";

            //获取下一个环节处理按钮
            long nextNodeModelID = 0;

            string strSQL = "select nodeModelID from es_node where nodeid =(select nodeid from es_message where messageid = ( " +
                            " select tmessageid from es_messagefromto where fmessageid = " + lngMessageID + "))";

            DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                nextNodeModelID = long.Parse(dt.Rows[0]["nodeModelID"].ToString());
            }
            if (nextNodeModelID > 0)
            {
                //地址栏参数内容
                string strParam = "";
                //得到下一个环节处理人ID
                long lngUserID = 0;
                string[] people = strReceivers.Trim().Split(",".ToCharArray());
                if (people.Length > 0)
                {
                    string[] sRec = people[0].Trim().Split("|".ToCharArray());
                    lngUserID = long.Parse(sRec[0]);
                }
                //获取下一条messageID
                long nextMessageID = 0;
                strSQL = "select tmessageid from es_messagefromto where fmessageid = " + lngMessageID;
                dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    nextMessageID = long.Parse(dt.Rows[0]["tmessageid"].ToString());
                }

                string strGuid = "";

                //获取按钮列表
                DataRow[] drs = FlowModel.GetNodeActionNew(lngFlowModelID, nextNodeModelID);
                if (drs == null || drs.Length == 0)
                {
                    //拼接地址栏参数内容
                    //strParam = "userid=" + lngUserID + "&messageid=" + nextMessageID + "&flowmodelid=" + lngFlowModelID + "&flowid=" + lngFlowID + "&actionid=0";
                    WeiXinModel wxm = new WeiXinModel();
                    wxm.ReceiverID = lngUserID;
                    wxm.MessageID = nextMessageID;
                    wxm.FlowmodelID = lngFlowModelID;
                    wxm.FlowID = lngFlowID;
                    wxm.ActionID = 0;
                    strParam = Newtonsoft.Json.JsonConvert.SerializeObject(wxm, Newtonsoft.Json.Formatting.Indented);

                    strGuid = System.Guid.NewGuid().ToString();
                    strSQL = "insert into br_weixin_param(guid,content,addTime)values(" + StringTool.SqlQ(strGuid) + "," + StringTool.SqlQ(strParam) + ",sysdate)";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    strContent += "<a href=\"" + httpURL + "?strweixin=" + strGuid + "\">确定</a>";
                }
                else
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        //拼接地址栏参数内容
                        //strParam = "userid=" + lngUserID + "&messageid=" + nextMessageID + "&flowmodelid=" + lngFlowModelID + "&flowid=" + lngFlowID + "&actionid=" + drs[i]["ActionID"].ToString();
                        strGuid = System.Guid.NewGuid().ToString();
                        WeiXinModel wxm = new WeiXinModel();
                        wxm.ReceiverID = lngUserID;
                        wxm.MessageID = nextMessageID;
                        wxm.FlowmodelID = lngFlowModelID;
                        wxm.FlowID = lngFlowID;
                        wxm.ActionID = CTools.ToInt64(drs[i]["ActionID"].ToString());
                        strParam = Newtonsoft.Json.JsonConvert.SerializeObject(wxm, Newtonsoft.Json.Formatting.Indented);

                        strSQL = "insert into br_weixin_param(guid,content,addTime)values(" + StringTool.SqlQ(strGuid) + "," + StringTool.SqlQ(strParam) + ",sysdate)";
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                        strContent += "<a href=\"" + httpURL + "?strweixin=" + strGuid + "\">" + drs[i]["ActionName"].ToString() + "</a>";
                        if (i < drs.Length - 1)
                            strContent += "\t";
                    }
                }

            }

            return strContent;
        }
    }
}
