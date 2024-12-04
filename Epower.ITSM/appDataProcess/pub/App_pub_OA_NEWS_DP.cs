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
    class App_pub_OA_NEWS_DP : IDataProcess
    {

        public App_pub_OA_NEWS_DP()
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

            strSQL = "SELECT * FROM OA_NEWS WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return dt;
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

            strSQL = "SELECT * FROM OA_NEWS WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    FieldValues fv = new FieldValues();
                    string strTemp = "";

                    EA_DefineLanguageDP dl = new EA_DefineLanguageDP();
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

            strSQL = "SELECT * FROM OA_NEWS WHERE FlowID =" + lngFlowID.ToString();
            DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);
            ConfigToolApp.CloseConnection(cn);
            return ds;
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

            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT NewsId FROM OA_News WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = null;
            try
            {
                dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    lngNextID = (long)dr.GetDecimal(0);
                    break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dr.Close();
            }

            //判断如果为手机上操作
            if (fv.GetFieldValue("Flag").Value.ToLower() == "true")
            {
                long lngBusIDM = MessageDep.GetBusinessActionID(lngFlowModelID, lngNodeModelID, lngActionID); //业务动作ID 对应 处理状态ID
                string lngBusNameM = FlowDP.getBuidName(lngBusIDM);  //业务动作名称 对应 处理状态名称


                int flag = 0;

                if (lngBusNameM != "")
                {
                    if (lngBusIDM == 18641)
                    {
                        flag = 1;
                    }
                }
                if (lngNextID != 0)
                {
                    strSQL = @"UPDATE OA_News SET " +
                                " Flag=" + flag.ToString() +
                                " WHERE NewsId = " + lngNextID.ToString();

                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                }
                return;
            }


            long lngBusID = MessageDep.GetBusinessActionID(lngFlowModelID, lngNodeModelID, lngActionID); //业务动作ID 对应 处理状态ID
            string lngBusName = FlowDP.getBuidName(lngBusID);  //业务动作名称 对应 处理状态名称


            if (lngBusName != "")
            {
                if (lngBusID == 18641)
                {
                    fv.GetFieldValue("FLAG").Value = "1";
                }
            }

            try
            {
                if (lngNextID == 0)
                {
                    #region 新增

                    lngNextID = long.Parse(EPGlobal.GetNextID("OA_NewsId").ToString());
                    strSQL = @"INSERT INTO OA_News(
                               NewsId,
                               FlowID,
                               NodeModelID,
                               FlowModelID,
                               Title,
                               TypeId,
                               Writer,
                               InputDate,
                               PubDate,
                               OutDate,
                               Content,
                               InputUser,
                               DispFlag,
                               Photo,
                               FocusNews,
                               IsBulletin,
                               FileName,
                               SoftName,
                               InOrgID,
                               InDeptID,
                               IsInner,
                               IsAlert,
                               Flag
                        )
                        Values(" +
                           lngNextID + "," +
                           lngFlowID.ToString() + "," +
                           lngNodeModelID.ToString() + "," +
                           lngFlowModelID.ToString() + "," +
                           StringTool.SqlQ(fv.GetFieldValue("TITLE").Value) + "," +
                           fv.GetFieldValue("TYPE").Value + "," +
                           StringTool.SqlQ(fv.GetFieldValue("WRITER").Value) + "," +
                           "to_date(" + StringTool.EmptyToNullDate(fv.GetFieldValue("INPUTDATE").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                           "to_date(" + StringTool.EmptyToNullDate(fv.GetFieldValue("PUBDATE").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                           "to_date(" + StringTool.EmptyToNullDate(fv.GetFieldValue("OUTDATE").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                           StringTool.SqlQ("") + "," +
                           StringTool.SqlQ(fv.GetFieldValue("INPUTUSER").Value) + "," +
                           fv.GetFieldValue("DISPFLAG").Value + "," +
                           StringTool.SqlQ("") + "," +
                           StringTool.SqlQ(fv.GetFieldValue("FOCUSNEWS").Value) + "," +
                           StringTool.SqlQ(fv.GetFieldValue("BULLETIN").Value) + "," +
                           StringTool.SqlQ(fv.GetFieldValue("FILENAME").Value) + "," +
                           StringTool.SqlQ(fv.GetFieldValue("SOFTNAME").Value) + "," +
                           fv.GetFieldValue("INORGID").Value + "," +
                           fv.GetFieldValue("INDEPTID").Value + "," +
                           fv.GetFieldValue("ISINNER").Value + "," +
                           fv.GetFieldValue("ISALERT").Value + "," +
                           fv.GetFieldValue("FLAG").Value +
                   ")";

                    #endregion
                }
                else
                {
                    #region 更新

                    strSQL = @"UPDATE OA_News SET " +
                                " NodeModelID = " + lngNodeModelID.ToString() + "," +
                                " Title =" + StringTool.SqlQ(fv.GetFieldValue("TITLE").Value) + "," +
                                " TypeId = " + fv.GetFieldValue("TYPE").Value + "," +
                                " FileName = " + StringTool.SqlQ(fv.GetFieldValue("FILENAME").Value) + "," +
                                " SoftName = " + StringTool.SqlQ(fv.GetFieldValue("SOFTNAME").Value) + "," +
                                " Photo = " + StringTool.SqlQ("") + "," +
                                " Writer = " + StringTool.SqlQ(fv.GetFieldValue("WRITER").Value) + "," +
                                " PubDate = to_date(" + StringTool.EmptyToNullDate(fv.GetFieldValue("PUBDATE").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                " OutDate = to_date(" + StringTool.EmptyToNullDate(fv.GetFieldValue("OUTDATE").Value) + ",'yyyy-MM-dd HH24:mi:ss')," +
                                " DispFlag = " + fv.GetFieldValue("DISPFLAG").Value + "," +
                                " Content= " + StringTool.SqlQ("") + "," +
                                " FocusNews = " + StringTool.SqlQ(fv.GetFieldValue("FOCUSNEWS").Value) + "," +
                                " IsBulletin = " + StringTool.SqlQ(fv.GetFieldValue("BULLETIN").Value) + "," +
                                " InOrgID=" + fv.GetFieldValue("INORGID").Value + "," +
                                " InDeptID=" + fv.GetFieldValue("INDEPTID").Value + "," +
                                " IsInner=" + fv.GetFieldValue("ISINNER").Value + "," +
                                " IsAlert=" + fv.GetFieldValue("ISALERT").Value + "," +
                                " Flag=" + fv.GetFieldValue("FLAG").Value +
                                " WHERE NewsId = " + lngNextID.ToString();
                    #endregion
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                #region CLOB类型特殊处理

                strSQL = "UPDATE OA_News Set Content=:a,Photo=:b where NewsId=" + lngNextID.ToString();
                OracleCommand cmd = new OracleCommand(strSQL, trans.Connection, trans);
                cmd.Parameters.Add("a", OracleType.Clob).Value = fv.GetFieldValue("CONTENT").Value;
                cmd.Parameters.Add("b", OracleType.Clob).Value = fv.GetFieldValue("PHOTO").Value;
                cmd.ExecuteNonQuery();

                #endregion
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
            string strSQL = "DELETE OA_News WHERE flowid =" + lngFlowID.ToString();
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
