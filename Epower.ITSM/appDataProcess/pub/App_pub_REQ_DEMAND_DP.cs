/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述："需求管理" 的应用接口类，在此定义需求管理模块需要与流程引擎交互的相关方法。
 * 
 * 关键方法: SaveFieldValues, NotifyMessage
 *            
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-04-25
 * 
 * 修改日志：
 * 修改时间：2013-04-25 修改人：孙绍棕
 * 修改描述：
 * *****************************************************************/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Collections;
using IappDataProcess;
using EpowerGlobal;
using EpowerCom;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

using System.Threading;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using Epower.ITSM.SqlDAL.Demand;
using System.Collections.Generic;

namespace appDataProcess.pub
{
    class App_pub_REQ_DEMAND_DP : IDataProcess
    {
        public App_pub_REQ_DEMAND_DP() { }

        /// <summary>
        /// 返回信息项结果表
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override DataTable GetFieldsDataTable(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            try
            {
                strSQL = "SELECT * FROM Req_Demand WHERE FlowID =" + lngFlowID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt;
            }
            finally { ConfigToolApp.CloseConnection(cn); }
        }

        /// <summary>
        /// 沟通时保存业务数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        /// <param name="lngMessageID"></param>
        public override void SaveFieldValuesForCommunic(OracleTransaction trans, long lngID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strXMlFieldValue, long lngMessageID)
        {
        }

        /// <summary>
        /// 获取业务快照数据（XML）
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public override string GetBussinessShotValues(long lngFlowID)
        {
            string strRet = string.Empty;

            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM Req_Demand WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = null;
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigToolApp.CloseConnection(cn); }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    FieldValues fv = new FieldValues();
                    string strTemp = "";

                    EA_DefineLanguageDP dl = new EA_DefineLanguageDP();


                    //需求单号
                    fv.Add(dl.GetLanguageValue("LitReqDemandNo"), row["DemandNO"].ToString());
                    //需求类别
                    fv.Add(dl.GetLanguageValue("LitReqDemandType"), row["DemandTypeName"].ToString());
                    //需求状态
                    fv.Add(dl.GetLanguageValue("LitReqDemandStatus"), row["DemandStatus"].ToString());
                    //需求主题
                    fv.Add(dl.GetLanguageValue("LitReqDemandSubject"), row["DemandSubject"].ToString());
                    //需求描述
                    fv.Add(dl.GetLanguageValue("LitReqDemandContent"), row["DemandContent"].ToString());


                    strRet = fv.GetXmlObject().InnerXml;


                }
            }

            return strRet;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override string GetFieldValues(long lngID, long lngOpID)
        {
            return String.Empty;
        }

        /// <summary>
        /// 返回信息项结果集
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        /// 
        public override DataSet GetFieldsDataSet(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();
            try
            {

                strSQL = "SELECT * FROM Req_Demand WHERE FlowID =" + lngFlowID.ToString();
                DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);

                return ds;
            }
            finally
            {
                ConfigToolApp.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 回收时应用执行的具体实现的接口

        /// 由于回收时需要删除一些内容,所以用户自定义操作会在前段执行
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void TakeBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {

        }

        /// <summary>
        /// 退回时应用执行的具体实现的接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {
        }

        /// <summary>
        /// 阅知状态下保存应用中的信息值

        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        public override void SaveFieldValuesForRead(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strValues, long lngMessageID)
        {
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
        public override void NotifyMessage(OracleTransaction trans, long lngID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strXMlFieldValue, string strReceivers, long lngMessageID)
        {
            FieldValues fv = new FieldValues(strXMlFieldValue);

            #region 机构环节时获取接收人员列表


            if (strReceivers == "")
            {
                string strSQL = "SELECT RECEIVEID,MESSAGEID FROM ES_RECEIVELIST WHERE MessageID IN (SELECT TMessageID FROM ES_MESSAGEFROMTO WHERE FMESSAGEID=" + lngMessageID.ToString() + ")";
                DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSQL).Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    strReceivers = strReceivers + row["RECEIVEID"].ToString() + "|" + row["MESSAGEID"].ToString() + ",";
                }
                if (strReceivers != "")
                    strReceivers = strReceivers.Substring(0, strReceivers.Length - 1);
            }
            #endregion

            #region 发送邮件


            if (fv.GetFieldValue("EmailNotify") != null)
            {
                //通知处理人的情况下


                if (fv.GetFieldValue("EmailNotify").Value.Trim().ToLower() == "true")
                {
                    MailSendDeal.SendEmailPublicV2(trans, lngID, strReceivers, fv, lngFlowModelID, 0, lngMessageID);
                }
            }
            #endregion

            #region 发送短信


            //短信发送


            //if (fv.GetFieldValue("SMSNotify") != null && fv.GetFieldValue("SMSNotify").Value.Trim().ToLower() == "true")
            //{
            //    SMSSendDeal.SendMessagePublic(trans, lngID, strReceivers, fv, lngFlowModelID);
            //}
            #endregion
        }

        /// <summary>
        /// 保存应用中的信息值
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        public override void SaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngActionID, long lngOpID, string strValues, long lngMessageID)
        {
            BaseSaveFieldValues(trans, lngFlowID, lngNodeModelID, lngFlowModelID, lngActionID, lngOpID, strValues, lngMessageID);
        }
        /// <summary>
        ///  添加和发送流程后用户代码
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendFlowFinish(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngOpID, long lngMessageID)
        {

        }


        /// <summary>
        /// 恢复流程执行的业务接口

        /// 2009-02-05 增加
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="flowStartTime">流程启动时间</param>
        /// <param name="flowPauseTime">流程暂停时间</param>
        /// <param name="flowContTime">流程恢复时间</param>
        /// <param name="lngUserID"></param>
        public override void DealFlowContinue(OracleTransaction trans, long lngFlowID, DateTime flowStartTime, DateTime flowPauseTime, DateTime flowContTime, long lngUserID)
        {

        }      

        /// <summary>
        /// 删除应用相关的信息.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            //删除主表
            string strSQL = "DELETE Req_Demand WHERE flowid =" + lngFlowID.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);                     
        }       

        /// <summary>
        /// 统一的归档处理接口

        /// </summary>
        /// <param name="lngID"></param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {

        }

        /// <summary>
        /// 新增和发送时产生MESSAGE时,用户自定义处理接口

        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterMessageAddedForAddOrSend(OracleTransaction trans, long lngFlowID, long lngNodeID, long lngMessageID, int intActorType, string sFActor)
        {
        }

        /// <summary>
        ///  实现用户自定义二次开发,接收人员 结果
        ///  如:自动分配(根据工作量)
        ///  从表单初提取 等

        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeID"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strFormXMLValue"></param>
        /// <param name="xmlDoc"></param>
        public override void UserInterfaceReceivers(long lngAppID, long lngOpID, long lngFlowID, long lngNodeID, long lngMessageID, long lngFlowModelID, long lngNodeModelID, long lngUserID, string strFormXMLValue, ref XmlDocument xmlDoc)
        {
        }


        /// <summary>
        /// 判断会签环节是否可以结束
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        /// <returns></returns>
        public override bool IsInfluxMessageFinished(OracleTransaction trans, long lngMessageID)
        {
            return false;
        }

        /// <summary>
        /// 用户接收事项后自定义处理
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="lngUserID"></param>
        public override void AfterReceiveMessage(OracleTransaction trans, long lngMessageID,
            long lngUserID)
        {
           
        }       

        /// <summary>
        /// 保存业务数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="strValues"></param>
        /// <param name="lngMessageID"></param>
        private void BaseSaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngActionID, long lngOpID, string strValues, long lngMessageID)
        {
            FieldValues fv = new FieldValues(strValues);

            #region 保存业务数据

            long lngNextID = 0;    // 标记是否新流程单. 0 新流程, > 0 已存在流程            

            string strSQL = "SELECT FlowID FROM Req_Demand WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();


            try
            {
                #region 更新或新增需求单 - 2013-04-26 @孙绍棕

                ReqDemandDP reqDemandDP = new ReqDemandDP();

                if (lngNextID == 0)
                {
                    reqDemandDP.Add(trans, lngFlowID, lngNodeModelID, lngFlowModelID, fv);
                }
                else
                {
                    reqDemandDP.UpdateByFlowID(trans, lngFlowID, lngNodeModelID, fv);
                }

                #endregion

                #region  保存扩展项信息

                #region 常用类别配置项
                
                int intRelateType = int.Parse(fv.GetFieldValue("CatalogSchemaRelateType").Value);
                BR_Schema_Deploy.DeleteAll(trans, lngFlowID, intRelateType);

                // 保存配置项信息
                string CalalogSchema = fv.GetFieldValue("CalalogSchema").Value.Trim();
                List<BR_Schema_Deploy> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BR_Schema_Deploy>>(CalalogSchema);

                foreach (BR_Schema_Deploy sd in list)
                {
                    sd.RelateID = lngFlowID;
                    sd.save(trans, sd);
                }              

                #endregion

                #endregion

            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);

                throw;
            }

            #endregion

        }
    }
}
