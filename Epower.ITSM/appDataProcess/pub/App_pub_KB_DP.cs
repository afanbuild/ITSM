using System;
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
    class App_pub_KB_DP : IDataProcess
    {
        public App_pub_KB_DP()
        { }


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

            strSQL = "SELECT * FROM INF_KMBASE WHERE FlowID =" + lngFlowID.ToString();
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

            strSQL = "SELECT * FROM INF_KMBASE WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = null;
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigToolApp.CloseConnection(cn); }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    FieldValues fv = new FieldValues();
                    EA_DefineLanguageDP dl = new EA_DefineLanguageDP();
                    fv.Add("登单人", row["RegUserName"].ToString());
                    fv.Add("关键字", row["pkey"].ToString());
                    fv.Add("摘要", row["tags"].ToString());
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
            strSQL = "SELECT * FROM INF_KMBASE WHERE FlowID =" + lngFlowID.ToString();
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
            BaseSaveFieldValues(trans, lngFlowID, lngNodeModelID, lngFlowModelID, strValues, lngMessageID);
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
        private void BaseSaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, string strValues, long lngMessageID)
        {
            #region 保存业务数据

            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT FlowID FROM INF_KMBASE WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();

            try
            {
                if (lngNextID == 0)
                {
                    #region 新增
                    lngNextID = EPGlobal.GetNextID("Inf_KMBaseKBID");
                    strSQL = @"INSERT INTO INF_KMBASE(
                                    KBID,
									FlowID,
									NodeModelID,
									FlowModelID,
                                    ListID,
                                    ListName,
                                    EquID,
                                    EquName,
									Title,
									PKey,
									Type,
									TypeName,
									Content,
                                    Tags,
									IsInKB,
                                    preflowid,
                                    reguser,
                                    regusername,
                                    RegTime,
									RegDeptID,
									RegDeptName
						)
						VALUES( " +
                            lngNextID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                            fv.GetFieldValue("listid").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("listname").Value) + "," +
                            fv.GetFieldValue("equid").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("equname").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("title").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("pkey").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("type").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("typename").Value) + "," +
                            ":Content," +//StringTool.SqlQ(fv.GetFieldValue("content").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("tags").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("isinkb").Value) + "," +
                            fv.GetFieldValue("preflowid").Value + "," +
                            fv.GetFieldValue("reguser").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("regusername").Value) + "," +
                            "to_date(" + StringTool.SqlQ(fv.GetFieldValue("RegTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            fv.GetFieldValue("DeptID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("DeptName").Value) +
                        ")";
                    #endregion
                }
                else
                {
                    #region 更新
                    strSQL = @"UPDATE INF_KMBASE Set " +
                                    " equid = " + StringTool.SqlQ(fv.GetFieldValue("equid").Value) + "," +
                                    " equname = " + StringTool.SqlQ(fv.GetFieldValue("equname").Value) + "," +
                                    " listid = " + StringTool.SqlQ(fv.GetFieldValue("listid").Value) + "," +
                                    " listname = " + StringTool.SqlQ(fv.GetFieldValue("listname").Value) + "," +
                                    " title = " + StringTool.SqlQ(fv.GetFieldValue("title").Value) + "," +
                                    " pkey = " + StringTool.SqlQ(fv.GetFieldValue("pkey").Value) + "," +
                                    " type = " + fv.GetFieldValue("type").Value + "," +
                                    " typename = " + StringTool.SqlQ(fv.GetFieldValue("typename").Value) + "," +
                                    " content =:Content, " + //StringTool.SqlQ(fv.GetFieldValue("content").Value) + "," +
                                    " Tags = " + StringTool.SqlQ(fv.GetFieldValue("tags").Value) + "," +
                                    " isinkb = " + fv.GetFieldValue("isinkb").Value +
                                " WHERE FlowID = " + lngNextID.ToString();
                    #endregion
                }

                //OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL, new OracleParameter[] { new OracleParameter("Content", fv.GetFieldValue("content").Value) });
                OracleCommand cmd = new OracleCommand(strSQL, trans.Connection, trans);
                cmd.Parameters.Add("Content", OracleType.Clob).Value = fv.GetFieldValue("content").Value;                
                cmd.ExecuteNonQuery();

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
            string strSQL = "DELETE inf_kmbase WHERE flowid =" + lngFlowID.ToString();
            OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
        }

        /// <summary>
        /// 统一的归档处理接口

        /// </summary>
        /// <param name="lngID"></param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {
            long lngNextID = 0;
            int blnInKB = 0;
            decimal listid = 0;
            string sListName = "";
            decimal equid = 0;
            string sEquName = "";
            string sTitle = "";
            string sPKey = "";
            string sTypeName = "";
            string sType = "";
            string sContent = "";
            string sRegUser = "";
            string sRegUserName = "";
            decimal lngPreFlowID = 0;
            string sTags = string.Empty;
            string strSQL = "SELECT isinkb,listid,listname,title,pkey,type,typename,content,reguser,regusername,preflowid,Tags FROM inf_kmbase WHERE FlowID=" + lngID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                blnInKB = Convert.ToInt32(dr["isinkb"].ToString());
                listid = Convert.ToDecimal(dr["listid"].ToString());
                sListName = dr["listname"].ToString();
                sTitle = dr["title"].ToString();
                sPKey = dr["pkey"].ToString();
                sType = dr["type"].ToString();
                sTypeName = dr["typename"].ToString();
                sContent = dr["content"].ToString();
                sRegUser = dr["reguser"].ToString();
                sRegUserName = dr["regusername"].ToString();
                lngPreFlowID = Convert.ToDecimal(dr["preflowid"].ToString());
                sTags = dr["Tags"].ToString();
                break;
            }
            dr.Close();

            try
            {
                if (blnInKB != 0)
                {
                    #region 添加

                    Inf_InformationDP ee = new Inf_InformationDP();
                    ee.DealKeyWordTags(trans, sPKey, 0);
                    string strID = EPGlobal.GetNextID("Inf_InformationID").ToString();
                    strSQL = @"INSERT INTO Inf_Information(
									ID,
                                    ListID,
                                    ListName,
                                    EquID,
                                    EquName,
									Title,
									PKey,
									Content,
                                    Type,
									TypeName,
									FullID,
                                    KBSource,
                                    FromID,
                                    ReadCount,
                                    KBVersion,
                                    DoneCount,
                                    Tags,
                                    Deleted,
									RegUserID,
									RegUserName,
									RegTime,
									UpdateUserID,
									UpdateUserName,
									UpdateTime
					)
					VALUES( " +
                                strID.ToString() + "," +
                                listid + "," +
                                StringTool.SqlQ(sListName) + "," +
                                equid + "," +
                                StringTool.SqlQ(sEquName) + "," +
                                StringTool.SqlQ(sTitle) + "," +
                                StringTool.SqlQ(sPKey) + "," +
                                StringTool.SqlQ(sContent) + "," +
                                sType + "," +
                                StringTool.SqlQ(sTypeName) + "," +
                                StringTool.SqlQ(Inf_SubjectDP.GetSubjectFullID(long.Parse(sType))) + "," +
                                (lngPreFlowID == 0 ? ((int)eOA_KBSource.eFlow).ToString() : ((int)eOA_KBSource.eFromFlow).ToString()) + "," +
                                (lngPreFlowID == 0 ? lngID.ToString() : lngPreFlowID.ToString()) + "," +
                                "0," +
                                "0," +
                                "0," +
                                StringTool.SqlQ(sTags) + "," +
                                "0," +
                                sRegUser + "," +
                                StringTool.SqlQ(sRegUserName) + "," +
                                "sysdate," +
                                sRegUser + "," +
                                StringTool.SqlQ(sRegUserName) + "," +
                                "sysdate" +
                        ")";

                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                    //插入附件的信息

                    strSQL = "insert into inf_attachment(fileid,kbid,filename,sufname,originid,status,uptime,upuserid,filepath,deleted,deletetime,requstfileid,monthpath ) select fileid," + strID + " as kbid,filename,sufname,originid,status,uptime,upuserid,filepath,deleted,deletetime,requstfileid,monthpath from es_attachment where flowid = " + lngID.ToString();
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                    #endregion
                }
            }
            catch
            {
                throw;
            }
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
