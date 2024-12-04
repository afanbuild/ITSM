/****************************************************************************
 * 
 * description:问题管理流程接口开发
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2007-07-02
 * *************************************************************************/
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
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;
using System.Data.OracleClient;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace appDataProcess.pub
{
    /// <summary>
    /// App_pub_ProblemSolve_DP 的摘要说明。

    /// </summary>
    public class App_pub_ProblemSolve_DP : IDataProcess
    {
        #region 构造方法


        public App_pub_ProblemSolve_DP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #endregion

        #region 返回信息项结果表

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

                strSQL = "SELECT * FROM Pro_ProblemDeal WHERE FlowID =" + lngFlowID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt;
            }
            finally { ConfigToolApp.CloseConnection(cn); }
        }

        #endregion

        #region 沟通时保存业务数据

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

        #endregion

        #region 获取业务快照数据

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

            strSQL = "SELECT * FROM Pro_ProblemDeal WHERE FlowID =" + lngFlowID.ToString();
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


                    //问题类别
                    fv.Add(dl.GetLanguageValue("LitProbleType"), row["Problem_TypeName"].ToString());
                    //问题描述
                    fv.Add(dl.GetLanguageValue("litProbleDescription"), row["Problem_Subject"].ToString());
                    //状态

                    fv.Add(dl.GetLanguageValue("litProbleState"), row["StateName"].ToString());



                    strRet = fv.GetXmlObject().InnerXml;


                }
            }

            return strRet;
        }


        public override string GetFieldValues(long lngID, long lngOpID)
        {
            string strRet = string.Empty;

            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM Pro_ProblemDeal WHERE FlowID =" + lngID.ToString();
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

                    if (lngOpID == 0)
                    {
                        //返回知识库

                        strTemp = "问题:" + row["Problem_Title"].ToString() + " 总结的知识";
                        fv.Add("Title", strTemp);

                        fv.Add("PKey", "");


                        strTemp = "问题描述:" + row["Problem_Subject"].ToString() + "<br>";
                        strTemp = strTemp + "问题处理:" + row["Remark"].ToString() + "<br>";

                        fv.Add("Content", strTemp);
                        fv.Add("EquipmentCatalogID", row["listID"].ToString());

                        fv.Add("EquipmentCatalogName", row["listName"].ToString());

                        fv.Add("EquipmentID", row["EquID"].ToString());
                        fv.Add("EquipmentName", row["EquName"].ToString());

                        strRet = fv.GetXmlObject().InnerXml;

                    }

                }
            }

            return strRet;
        }

        #endregion

        #region 返回信息项结果集

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
                strSQL = "SELECT * FROM Pro_ProblemDeal WHERE FlowID =" + lngFlowID.ToString();
                DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);

                return ds;
            }
            finally { ConfigToolApp.CloseConnection(cn); }

        }

        #endregion

        #region 回收时应用执行的具体实现的接口


        /// <summary>
        /// 回收时应用执行的具体实现的接口

        /// 由于回收操作需要删除一些内容，所以用户自定义操作会在前段执行
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void TakeBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {
        }


        #endregion

        #region 退回时应用执行的具体实现的接口

        /// <summary>
        /// 退回时应用执行的具体实现的接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {
        }
        #endregion

        #region 阅知状态下保存应用中的信息值


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

        #endregion

        #region 流程发送处理通知接口

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
        #endregion

        #region 保存应用中的信息值


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

        #endregion

        #region 添加和发送流程后用户代码

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
            //
        }

        #endregion

        #region 保存业务数据

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
            #region 保存业务数据
            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT FlowID FROM Pro_ProblemDeal WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();

            long lngBusID = MessageDep.GetBusinessActionID(lngFlowModelID, lngNodeModelID, lngActionID); //业务动作ID 对应 问题状态ID

            #region 根据不同业务动作绑定对应的问题状态


            string State = fv.GetFieldValue("State").Value;
            string StateName = fv.GetFieldValue("StateName").Value;

            string strBusName = "";

            if (lngBusID > 0)
            {
                if (lngBusID == 2284)
                    strBusName = "已登记";
                else
                    strBusName = FlowDP.getBuidName(lngBusID);

                //根据问题状态名称 查找对应的ID
                string strCatalogID = CatalogDP.GetCatalogIDbyName(strBusName, 1021);

                State = strCatalogID;
                StateName = strBusName;
            }
            #endregion

            try
            {

                List<OracleParameter> listParam = new List<OracleParameter>();

                if (lngNextID == 0)
                {
                    #region 新增
                    lngNextID = EPGlobal.GetNextID("ProblemDealID");
                    strSQL = @"INSERT INTO Pro_ProblemDeal(
                                    Problem_ID,
									FlowID,
									NodeModelID,
									FlowModelID,
									Problem_Type,
                                    Problem_TypeName,
									Problem_Level,
                                    Problem_LevelName,
                                    EffectID,
									EffectName,
                                    InstancyID,
									InstancyName,
									Problem_Subject,
									State,
                                    StateName,
									Problem_Title,
									Remark,
									RegUserID,
									RegUserName,
									RegDeptID,
									RegDeptName,
                                    RegOrgID,
									RegTime,
                                    ChangeServiceFlowID,
                                    ProblemNo,
                                    BuildCode,
                                    EquID,
                                    EquName,
                                    ListID,
                                    ListName,
                                    DealContent
						)
						VALUES( " +
                            lngNextID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                            fv.GetFieldValue("Problem_Type").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Problem_TypeName").Value) + "," +
                            fv.GetFieldValue("Problem_Level").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Problem_LevelName").Value) + "," +
                            fv.GetFieldValue("EffectID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("EffectName").Value) + "," +
                            fv.GetFieldValue("InstancyID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("InstancyName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Problem_Subject").Value) + "," +
                            fv.GetFieldValue("State").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("StateName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Problem_Title").Value) + ",:remark," +
                            fv.GetFieldValue("RegUserID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RegUserName").Value) + "," +
                            fv.GetFieldValue("RegDeptID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RegDeptName").Value) + "," +
                            fv.GetFieldValue("RegOrgID").Value + "," +
                            "to_date(" + StringTool.SqlQ(fv.GetFieldValue("RegTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            fv.GetFieldValue("FromFlowID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ProblemNo").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("BuildCode").Value) + "," +
                            fv.GetFieldValue("EquID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("EquName").Value) + "," +
                            fv.GetFieldValue("ListID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ListName").Value) + ", :dealcontent)";
                    #endregion

                    //更新事件单

                    decimal dFromFlowID = decimal.Parse(fv.GetFieldValue("FromFlowID").Value.Trim());
                    if (dFromFlowID != 0)
                    {
                        string ssql = "UPDATE Cst_Issues Set " +
                            " ChangeProblemFlowID = " + lngFlowID.ToString() + "," +
                            " ChangeProblem = " + StringTool.SqlQ("此事件单已升级为问题单[" + fv.GetFieldValue("Problem_Title").Value + "]");
                        ssql += " WHERE FlowID = " + dFromFlowID.ToString();
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, ssql);

                        //生成关联权重等

                        System.Text.StringBuilder sql = new System.Text.StringBuilder();
                        string strID = EPGlobal.GetNextID("Pro_ProblemAnalyse_SEQUENCE").ToString();
                        sql.Append(" Insert Into Pro_ProblemAnalyse(ID,Problem_FlowID,Problem_Title,Event_FlowID,Event_Title,Scale,Effect,Stress,Remark,RegUserID,RegUserName,RegDeptID,RegTime) Values(");
                        sql.AppendFormat(strID + "," + lngFlowID.ToString() + "," + StringTool.SqlQ(fv.GetFieldValue("Problem_Title").Value) + "," + dFromFlowID.ToString() + "," + StringTool.SqlQ(fv.GetFieldValue("ServiceTitle").Value.Trim()) + ",");
                        sql.AppendFormat("100" + "," + "100" + "," + "100" + "," + StringTool.SqlQ("事件升级生成") + ",");
                        sql.AppendFormat(fv.GetFieldValue("RegUserID").Value + "," + StringTool.SqlQ(fv.GetFieldValue("RegUserName").Value) + "," + fv.GetFieldValue("RegDeptID").Value + "," + "sysdate) ");
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());
                    }
                }
                else
                {
                    #region 更新
                    strSQL = @"UPDATE Pro_ProblemDeal Set " +
                            " NodeModelID = " + lngNodeModelID.ToString() + "," +
                                    " Problem_Type = " + fv.GetFieldValue("Problem_Type").Value + "," +
                                    " Problem_TypeName = " + StringTool.SqlQ(fv.GetFieldValue("Problem_TypeName").Value) + "," +
                                    " Problem_Level = " + fv.GetFieldValue("Problem_Level").Value + "," +
                                    " Problem_LevelName = " + StringTool.SqlQ(fv.GetFieldValue("Problem_LevelName").Value) + "," +
                                    " EffectID = " + fv.GetFieldValue("EffectID").Value + "," +
                                    " EffectName = " + StringTool.SqlQ(fv.GetFieldValue("EffectName").Value) + "," +
                                    " InstancyID = " + fv.GetFieldValue("InstancyID").Value + "," +
                                    " InstancyName = " + StringTool.SqlQ(fv.GetFieldValue("InstancyName").Value) + "," +
                                    " Problem_Subject = " + StringTool.SqlQ(fv.GetFieldValue("Problem_Subject").Value) + "," +
                                    " State = " + fv.GetFieldValue("State").Value + "," +
                                    " StateName = " + StringTool.SqlQ(fv.GetFieldValue("StateName").Value) + "," +
                                    " ProblemNo = " + StringTool.SqlQ(fv.GetFieldValue("ProblemNo").Value) + "," +
                                    " BuildCode = " + StringTool.SqlQ(fv.GetFieldValue("BuildCode").Value) + "," +
                                    " Problem_Title = " + StringTool.SqlQ(fv.GetFieldValue("Problem_Title").Value) + "," +
                                    " Remark = :remark," +
                                    " EquID = " + fv.GetFieldValue("EquID").Value + "," +
                                    " EquName = " + StringTool.SqlQ(fv.GetFieldValue("EquName").Value) + "," +
                                    " ListID = " + fv.GetFieldValue("ListID").Value + "," +
                                    " ListName = " + StringTool.SqlQ(fv.GetFieldValue("ListName").Value) + "," +
                                    "DealContent= :dealcontent " +
                                " WHERE FlowID = " + lngFlowID.ToString();
                    #endregion
                }


                listParam.Add(OracleDbHelper.GetClobParameter("Remark", fv.GetFieldValue("Remark").Value));    // 解决方案
                listParam.Add(OracleDbHelper.GetClobParameter("DealContent", fv.GetFieldValue("DealContent").Value));    // 原因分析

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL, listParam.ToArray());


                #region 保存合并问题单

                string strLSRisk = fv.GetFieldValue("ItemXml").Value;
                int itemcount = int.Parse(fv.GetFieldValue("ItemCount").Value);

                if (strLSRisk != "")
                {
                    string strLSR = "Delete from Pro_ProblemRel where MastFlowID = " + lngFlowID.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strLSR);

                    FieldValues fvitem = new FieldValues(strLSRisk);
                    string sitsmSql = string.Empty;
                    for (int i = 0; i < itemcount; i++)
                    {
                        int icheck = int.Parse(fvitem.GetFieldValue("strChk" + i).Value);
                        if (icheck == 1)  //如果为

                            icheck = 2;

                        string strRelID = EpowerGlobal.EPGlobal.GetNextID("Pro_ProblemRel_SEQUENCE").ToString();

                        sitsmSql = @"INSERT INTO Pro_ProblemRel(
                                    ID,
                                    MastFlowID,
									SubFlowID,
									FlowDealState
						)
						VALUES(" + strRelID + "," +
                            lngFlowID.ToString() + "," +
                            fvitem.GetFieldValue("sSubFlowID" + i).Value + "," +
                            icheck.ToString() +
                        ")";
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sitsmSql);

                        sitsmSql = "select Max(MessageID) MessageID from es_message where FlowID=" + fvitem.GetFieldValue("sSubFlowID" + i).Value;
                        OracleDataReader dritsm = OracleDbHelper.ExecuteReader(trans, CommandType.Text, sitsmSql);
                        long lngitemmessageid = 0;
                        while (dritsm.Read())
                        {
                            lngitemmessageid = (long)dritsm.GetDecimal(0);
                            break;
                        }
                        dritsm.Close();

                        if (fvitem.GetFieldValue("strChk" + i).Value == "1")
                        {
                            bool breturn = FlowDP.AutoEndFlow(trans, long.Parse(fv.GetFieldValue("RegUserID").Value), lngitemmessageid, "问题合并结束");
                        }
                    }
                }
                #endregion

                #region  保存扩展项信息


                string ExtensionList = fv.GetFieldValue("ExtensionDayList").Value.Trim();
                string[] strArray = ExtensionList.Split('&');
                if (strArray.Length > 0)
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string[] stritems = strArray[i].ToString().Split('@');
                        if (stritems.Length > 1)
                        {
                            EQU_deploy equdept = new EQU_deploy();
                            equdept.ID = long.Parse(stritems[0]);
                            equdept.EquID = lngFlowID;
                            equdept.CHName = stritems[2].ToString();
                            equdept.FieldID = long.Parse(stritems[3]);
                            equdept.Value = stritems[4].ToString();

                            equdept.save(equdept);

                        }


                    }


                }

                #endregion
            }
            catch
            {
                throw;
            }
            #endregion

        }

        #endregion

        #region 删除应用相关的信息


        /// <summary>
        /// 删除应用相关的信息。

        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            //删除主表
            string strSQL = "DELETE Pro_ProblemDeal WHERE flowid =" + lngFlowID.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

            strSQL = " Update Cst_Issues set ChangeProblemFlowID=0,ChangeProblem='' where nvl(ChangeProblemFlowID,0)=" + lngFlowID.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

            string strLSR = "Delete from Pro_ProblemRel where MastFlowID = " + lngFlowID.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strLSR);
        }
        #endregion

        #region 统一的归档处理接口


        /// <summary>
        /// 统一的归档处理接口

        /// </summary>
        /// <param name="lngID"></param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {

        }
        #endregion

        #region 新增和发送时产生MESSAGE时,用户自定义处理接口


        /// <summary>
        /// 新增和发送时产生MESSAGE时,用户自定义处理接口

        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterMessageAddedForAddOrSend(OracleTransaction trans, long lngFlowID, long lngNodeID, long lngMessageID, int intActorType, string sFActor)
        {

        }

        #endregion

        #region 实现用户自定义二次开发 接收人员 结果

        /// <summary>
        ///  实现用户自定义二次开发 接收人员 结果
        ///           如:  自动分配 (根据工作量)
        ///                从表单上提取  等

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

        #endregion

        #region 用户接收事项后自定义处理

        /// <summary>
        /// 用户接收事项后自定义处理
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterReceiveMessage(OracleTransaction trans, long lngMessageID, long lngUserID)
        {

        }

        #endregion

        #region 判断会签环节是否可以结束

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

        #endregion

    }
}
