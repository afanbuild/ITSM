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
using Epower.ITSM.SqlDAL;
using System.Data.OracleClient;



namespace appDataProcess.pub
{
    class App_pub_OA_RELEASEMANAGEMENT_DP: IDataProcess
    {
        /// <summary>逻辑构造函数
        /// 
        /// </summary>
        public App_pub_OA_RELEASEMANAGEMENT_DP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>返回信息项结果表
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override DataTable GetFieldsDataTable(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM OA_RELEASEMANAGEMENT WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return dt;
        }

        /// <summary>沟通时保存业务数据
        /// 
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

        /// <summary>获取字段的值的字符串（XML）
        ///  
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override string GetFieldValues(long lngID, long lngOpID)
        {
            return string.Empty;
        }

        /// <summary>返回信息项结果集
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public override DataSet GetFieldsDataSet(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM OA_RELEASEMANAGEMENT WHERE FlowID =" + lngFlowID.ToString();
            DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return ds;
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
        /// 回收时应用执行的具体实现的接口
        /// 由于回收操作需要删除一些内容，所以用户自定义操作会在前段执行
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void TakeBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {
        }

        /// <summary>流程发送处理通知接口(在流程发送/新增处理的提交前执行)
        /// 
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

        /// <summary>保存应用中的信息值
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strXMlFieldValue"></param>
        public override void SaveFieldValues(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngActionID, long lngOpID, string strValues, long lngMessageID)
        {
            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT FlowID FROM OA_RELEASEMANAGEMENT WHERE FlowID=" + lngFlowID.ToString();
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
                    lngNextID = long.Parse(fv.GetFieldValue("RMID").Value);
                    if (lngNextID == 0)
                        lngNextID = EPGlobal.GetNextID("OA_RELEASEMANAGEMENTID");//!!!!---
                    strSQL = @"INSERT INTO OA_RELEASEMANAGEMENT(
									RMID,
                                     FLOWID,
                                     NODEMODELID,
                                     FLOWMODELID,
                                     VERSIONNAME,
                                     VERSIONCODE,
                                     RELEASEDATE,
                                     RELEASESCOPEID,
                                     RELEASESCOPENAME,
                                     REGUSERID,
                                     REGUSERNAME,
                                     REGDEPTID,
                                     REGDEPTNAME,
                                     RELEASEPERSONID,
                                     RELEASEPERSONNAME,
                                     VERSIONKINDID,
                                     VERSIONKINDNAME,
                                     VERSIONTYPEID,
                                     VERSIONTYPENAME,
                                     RELEASECATEGORYID,
                                     RELEASECATEGORYNAME,
                                     RELEASECONTENT,
                                     RELEASEPHONE,
                                     REGORGID
						)
						VALUES( " +
                            lngNextID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                             StringTool.SqlQ(fv.GetFieldValue("VERSIONNAME").Value) + "," +
                             StringTool.SqlQ(fv.GetFieldValue("VERSIONCODE").Value) + "," +
                             StringTool.SqlQ(fv.GetFieldValue("RELEASEDATE").Value) + "," +

                            fv.GetFieldValue("RELEASESCOPEID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RELEASESCOPENAME").Value) + "," +

                            fv.GetFieldValue("REGUSERID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("REGUSERNAME").Value) + "," +

                            fv.GetFieldValue("REGDEPTID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("REGDEPTNAME").Value) + "," +

                            fv.GetFieldValue("RELEASEPERSONID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RELEASEPERSONNAME").Value) + "," +

                            fv.GetFieldValue("VERSIONKINDID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("VERSIONKINDNAME").Value) + "," +

                            fv.GetFieldValue("VERSIONTYPEID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("VERSIONTYPENAME").Value) + "," +

                              "0," +
                              "''," +

                            StringTool.SqlQ(fv.GetFieldValue("RELEASECONTENT").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RELEASEPHONE").Value) + "," +
                            fv.GetFieldValue("REGORGID").Value +
                        ")";
                }
                else
                {
                    strSQL = @"UPDATE OA_RELEASEMANAGEMENT Set " +
                                    " NodeModelID = " + lngNodeModelID.ToString() + "," +
                                    " FlowModelID = " + lngFlowModelID.ToString() + "," +
                                    " VERSIONNAME = " + StringTool.SqlQ(fv.GetFieldValue("VERSIONNAME").Value) + "," +
                                     " VERSIONCODE = " + StringTool.SqlQ(fv.GetFieldValue("VERSIONCODE").Value) + "," +
                                      " RELEASEDATE = " +  StringTool.SqlQ(fv.GetFieldValue("RELEASEDATE").Value) + "," +


                                    " RELEASESCOPEID = " + fv.GetFieldValue("RELEASESCOPEID").Value + "," +
                                    " RELEASESCOPENAME = " + StringTool.SqlQ(fv.GetFieldValue("RELEASESCOPENAME").Value) + "," +
                                    " REGUSERID = " + fv.GetFieldValue("REGUSERID").Value + "," +
                                    " REGUSERNAME = " + StringTool.SqlQ(fv.GetFieldValue("REGUSERNAME").Value) + "," +
                                    " REGDEPTID = " + fv.GetFieldValue("REGDEPTID").Value + "," +
                                    " REGDEPTNAME = " + StringTool.SqlQ(fv.GetFieldValue("REGDEPTNAME").Value) + "," +
                                     " RELEASEPERSONID = " + fv.GetFieldValue("RELEASEPERSONID").Value + "," +
                                    " RELEASEPERSONNAME = " + StringTool.SqlQ(fv.GetFieldValue("RELEASEPERSONNAME").Value) + "," +
                                    " VERSIONKINDID = " + fv.GetFieldValue("VERSIONKINDID").Value + "," +
                                    " VERSIONKINDNAME = " + StringTool.SqlQ(fv.GetFieldValue("VERSIONKINDNAME").Value) + "," +
                                    " VERSIONTYPEID = " + fv.GetFieldValue("VERSIONTYPEID").Value + "," +
                                    " VERSIONTYPENAME = " + StringTool.SqlQ(fv.GetFieldValue("VERSIONTYPENAME").Value) + "," +

                                    " RELEASECONTENT = " + StringTool.SqlQ(fv.GetFieldValue("RELEASECONTENT").Value) + "," +
                                    " RELEASEPHONE = " + StringTool.SqlQ(fv.GetFieldValue("RELEASEPHONE").Value) +
                                " WHERE FlowID = " + lngFlowID.ToString();
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>退回时应用执行的具体实现的接口
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {
        }
        
        /// <summary>删除应用相关的信息。（当前应用仅一个OP）
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            string strSQL = "";
            try
            {
                strSQL = "DELETE OA_RELEASEMANAGEMENT WHERE FlowID = " + lngFlowID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>统一的归档处理接口
        /// 
        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {
            //string strSQL = "";
            //try
            //{
            //    strSQL = "UPDATE App_pub_Normal_Head SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
            //    MyDataBase.ExecuteNonQuery(trans, strSQL);
            //}
            //catch
            //{
            //    throw;
            //}
        }

        /// <summary>统一的归档处理接口
        /// 
        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowAbort(OracleTransaction trans, long lngID)
        {
            //string strSQL = "";
            //try
            //{
            //    strSQL = "UPDATE App_pub_Normal_Head SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
            //    MyDataBase.ExecuteNonQuery(trans, strSQL);
            //}
            //catch
            //{
            //    throw;
            //}
        }

        /// <summary>新增和发送时产生MESSAGE时,用户自定义处理接口
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterMessageAddedForAddOrSend(OracleTransaction trans, long lngFlowID, long lngNodeID, long lngMessageID, int intActorType, string sFActor)
        {
        }

        /// <summary>删除消息（协办/阅知/转发/传阅/沟通【非主办】）用户接口
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        public override void DeleteMessage(OracleTransaction trans, long lngMessageID)
        {
        }

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
        
        /// <summary>用户接收事项后自定义处理
        /// 
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterReceiveMessage(OracleTransaction trans, long lngMessageID, long lngUserID)
        {

        }

        /// <summary>判断会签环节是否可以结束
        /// 
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
