﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Collections;
using IappDataProcess;
using EpowerGlobal;
using EpowerCom;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;

namespace appDataProcess.pub
{
    class App_pub_Flow_QuestHouse_DP : IDataProcess
    {

        public App_pub_Flow_QuestHouse_DP()
        {
        }
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

            strSQL = "SELECT * FROM Flow_QuestHouse WHERE FlowID =" + lngFlowID.ToString();
            try
            {
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

            strSQL = "SELECT * FROM Flow_QuestHouse WHERE FlowID =" + lngFlowID.ToString();
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


                    fv.Add("提交人", row["execByName"].ToString());

                    fv.Add("提交时间", row["createDate"].ToString());



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
            return string.Empty;
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

            strSQL = "SELECT * FROM Flow_QuestHouse WHERE FlowID =" + lngFlowID.ToString();
            try
            {
                DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);

                return ds;
            }
            finally { ConfigToolApp.CloseConnection(cn); }
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
        ///退回时应用执行的具体实现的接口
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
        ///流程发送处理通知接口(在流程发送/新增处理的提交前执行)
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        /// <param name="strReceivers"></param>
        /// <param name="lngMessageID">>接收者和消息值列字符串 格式: 接收者ID|消息ID,接收者ID|消息ID,... (仅主办的消息)</param>
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
            BaseSaveFieldValues(trans, lngFlowID, lngNodeModelID, lngFlowModelID, strValues, lngMessageID, lngActionID);
        }
        /// <summary>
        /// 添加和发送流程后用户代码
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
        ///保存业务数据
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="strValues"></param>
        /// <param name="lngMessageID"></param>
        private void BaseSaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, string strValues, long lngMessageID, long lngActionID)
        {
            #region 保存业务数据
            long lngNextID = 0, statusId = 0;
            string statusName = string.Empty;
            bool flagStatusEdit = false;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT FlowID FROM Flow_QuestHouse WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();

            long lngBusID = MessageDep.GetBusinessActionID(lngFlowModelID, lngNodeModelID, lngActionID); //业务动作ID 对应 处理状态ID
            string lngBusName = FlowDP.getBuidName(lngBusID);  //业务动作名称 对应 处理状态名称


            if (lngBusName != "")
            {
                statusId = lngBusID;    //处理状态ID
                statusName = lngBusName.ToString();    //处理状态名称

                flagStatusEdit = true;
            }

            try
            {
                if (lngNextID == 0)
                {
                    #region 新增

                    lngNextID = long.Parse(fv.GetFieldValue("HouseID").Value);
                    strSQL = @"insert into flow_questhouse(houseid, flowid, nodemodelid, flowmodelid,ITILNO,
                               createbyid, createbyname, createbydeptid, CJRByPhone,
                               createbydeptname, execbyid, execbyname,
                               execbyphone, execbydeptid, execbydeptname,
                               execbyno, createdate, comeindate, outdate, 
                               zgflowdate,SZSZGflowDate, sqdescr, txjzname,txjzis,
                               isokscan, iscaozuoj, isahouse, isbhouse, ischouse,statusid,statusname,
                               ActionTypeID,ActionTypeName,Address,IsBudan,OpObj,OpObjId) 
						VALUES( " +
                            lngNextID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ITILNO").Value) + "," +
                            fv.GetFieldValue("createbyid").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("createbyname").Value) + "," +
                            fv.GetFieldValue("createbydeptid").Value + "," +
                             StringTool.SqlQ(fv.GetFieldValue("CJRByPhone").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("createbydeptname").Value) + "," +
                            fv.GetFieldValue("execbyid").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("execbyname").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("execbyphone").Value) + "," +
                            fv.GetFieldValue("execbydeptid").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("execbydeptname").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("execbyno").Value) + "," +
                            (fv.GetFieldValue("createdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("createdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            (fv.GetFieldValue("comeindate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("comeindate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            (fv.GetFieldValue("outdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("outdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            (fv.GetFieldValue("zgflowdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("zgflowdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            (fv.GetFieldValue("SZSZGflowDate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("SZSZGflowDate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            StringTool.SqlQ(fv.GetFieldValue("sqdescr").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("txjzname").Value) + "," +
                            fv.GetFieldValue("txjzis").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("isokscan").Value) + "," +
                            fv.GetFieldValue("iscaozuoj").Value + "," +
                            fv.GetFieldValue("isahouse").Value + "," +
                            fv.GetFieldValue("isbhouse").Value + "," +
                            fv.GetFieldValue("ischouse").Value + "," +
                            statusId + "," +
                            StringTool.SqlQ(statusName) + "," +
                            fv.GetFieldValue("ActionTypeID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ActionTypeName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Address").Value) + "," +
                            fv.GetFieldValue("IsBudan").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("opobj").Value) + "," +
                            (fv.GetFieldValue("opobjid").Value == "" ? "0" : fv.GetFieldValue("opobjid").Value) +
                        ")";
                    #endregion
                }
                else
                {
                    #region 更新

                    strSQL = "UPDATE flow_questhouse set createbyid=" + fv.GetFieldValue("createbyid").Value + "," +
                            "createbyname=" + StringTool.SqlQ(fv.GetFieldValue("createbyname").Value) + "," +
                            "createbydeptid=" + fv.GetFieldValue("createbydeptid").Value + "," +
                            "createbydeptname=" + StringTool.SqlQ(fv.GetFieldValue("createbydeptname").Value) + "," +
                            "CJRByPhone=" + StringTool.SqlQ(fv.GetFieldValue("CJRByPhone").Value) + "," +
                            "execbyid=" + fv.GetFieldValue("execbyid").Value + "," +
                            "execbyname=" + StringTool.SqlQ(fv.GetFieldValue("execbyname").Value) + "," +
                            "execbyphone=" + StringTool.SqlQ(fv.GetFieldValue("execbyphone").Value) + ",";
                    if (flagStatusEdit)
                    {
                        strSQL += "StatusID = " + statusId + "," +
                          "StatusName = " + StringTool.SqlQ(statusName) + ",";
                    }
                    if (fv.GetFieldValue("opobjid").Value != "")
                    {
                        strSQL += "OpObj = " + StringTool.SqlQ(fv.GetFieldValue("opobj").Value) + "," +
                                                 "OpObjId = " + fv.GetFieldValue("opobjid").Value + ",";
                    }
                    strSQL += "execbydeptid=" + fv.GetFieldValue("execbydeptid").Value + "," +
                            "execbydeptname=" + StringTool.SqlQ(fv.GetFieldValue("execbydeptname").Value) + "," +
                            "execbyno=" + StringTool.SqlQ(fv.GetFieldValue("execbyno").Value) + "," +
                            "createdate=" + (fv.GetFieldValue("createdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("createdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            "comeindate=" + (fv.GetFieldValue("comeindate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("comeindate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            "outdate=" + (fv.GetFieldValue("outdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("outdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            "zgflowdate=" + (fv.GetFieldValue("zgflowdate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("zgflowdate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            "SZSZGflowDate=" + (fv.GetFieldValue("SZSZGflowDate").Value == "" ? " null " : "to_date(" + StringTool.SqlQ(fv.GetFieldValue("SZSZGflowDate").Value) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            "sqdescr=" + StringTool.SqlQ(fv.GetFieldValue("sqdescr").Value) + "," +
                            "txjzname=" + StringTool.SqlQ(fv.GetFieldValue("txjzname").Value) + "," +
                            "txjzis=" + fv.GetFieldValue("txjzis").Value + "," +
                            "isokscan=" + StringTool.SqlQ(fv.GetFieldValue("isokscan").Value) + "," +
                            "iscaozuoj=" + fv.GetFieldValue("iscaozuoj").Value + "," +
                            "isahouse=" + fv.GetFieldValue("isahouse").Value + "," +
                            "isbhouse=" + fv.GetFieldValue("isbhouse").Value + "," +
                            "ischouse=" + fv.GetFieldValue("ischouse").Value + "," +
                            "ActionTypeID = " + fv.GetFieldValue("ActionTypeID").Value + "," +
                            "ActionTypeName = " + StringTool.SqlQ(fv.GetFieldValue("ActionTypeName").Value) + "," +
                            "Address = " + StringTool.SqlQ(fv.GetFieldValue("Address").Value) + "," +
                            "IsBudan=" + fv.GetFieldValue("IsBudan").Value +
                            " WHERE FlowID = " + lngFlowID.ToString();



                    #endregion
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            #endregion
        }


        /// <summary>
        ///删除应用相关的信息.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            //
            string strSQL = "DELETE Flow_QuestHouse WHERE flowid =" + lngFlowID.ToString();
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
    }
}
