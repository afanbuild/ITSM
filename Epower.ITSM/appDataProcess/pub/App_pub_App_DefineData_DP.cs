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
using MyComponent;
using System.Data.OracleClient;

namespace appDataProcess.pub
{
    /// <summary>
    /// 
    /// </summary>
    public class App_pub_App_DefineData_DP : IDataProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public App_pub_App_DefineData_DP()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
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

            strSQL = "SELECT * FROM App_DefineData WHERE FlowID =" + lngFlowID.ToString();
            DataTable dt = null;
            try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
            finally { ConfigToolApp.CloseConnection(cn); }
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
        ///  获取字段的值的字符串（XML）

        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="lngAppID"></param>
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
        public override DataSet GetFieldsDataSet(long lngFlowID, long lngOpID)
        {
            string strSQL = "";

            OracleConnection cn = ConfigToolApp.GetConnection();

            strSQL = "SELECT * FROM App_DefineData WHERE FlowID =" + lngFlowID.ToString();
            try
            {
                DataSet ds = OracleDbHelper.ExecuteDataset(cn, CommandType.Text, strSQL);
                return ds;
            }
            finally { ConfigToolApp.CloseConnection(cn); }
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
            ToolsDP.NotifyMessage(trans, lngID, lngNodeModelID, lngFlowModelID, lngOpID, strXMlFieldValue, strReceivers, lngMessageID);


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
            #region
            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            string strSQL = "SELECT FlowID FROM App_DefineData WHERE FlowID=" + lngFlowID.ToString();
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
                    #region
                    lngNextID = EPGlobal.GetNextID("App_DefineDataID");
                    strSQL = @"INSERT INTO App_DefineData(
									ID,
									FlowID,
									NodeModelID,
									FlowModelID,
									ApplyID,
									ApplyName,
									DeptID,
									DeptName,
									FlowName,
									StartDate,
									ContentXml
						)
						VALUES( " +
                            lngNextID.ToString() + "," +
                            lngFlowID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                            fv.GetFieldValue("ApplyID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ApplyName").Value) + "," +
                            fv.GetFieldValue("DeptID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("DeptName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("FlowName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("StartDate").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("ContentXml").Value) +
                        ")";
                    #endregion
                }
                else
                {
                    #region
                    strSQL = @"UPDATE App_DefineData Set " +
                                    " NodeModelID = " + lngNodeModelID.ToString() + "," +
                                    " FlowModelID = " + lngFlowModelID.ToString() + "," +
                                    " FlowName = " + StringTool.SqlQ(fv.GetFieldValue("FlowName").Value) + "," +
                                    " ContentXml = " + StringTool.SqlQ(fv.GetFieldValue("ContentXml").Value) +
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
        /// 退回时应用执行的具体实现的接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void SendBackUserProcess(OracleTransaction trans, long lngFlowID, long lngMessageID)
        {

        }


        /// <summary>
        /// 删除应用相关的信息。（当前应用仅一个OP）

        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        public override void DeleteFieldValues(OracleTransaction trans, long lngFlowID, long lngOpID)
        {
            string strSQL = "";
            try
            {
                strSQL = "DELETE App_DefineData WHERE FlowID = " + lngFlowID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 统一的归档处理接口

        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowEnd(OracleTransaction trans, long lngID)
        {
            string strSQL = "";
            try
            {
                strSQL = "UPDATE App_DefineData SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 统一的归档处理接口

        /// </summary>
        /// <param name="lngID">流程ID</param>
        public override void DoFlowAbort(OracleTransaction trans, long lngID)
        {
            string strSQL = "";
            try
            {
                strSQL = "UPDATE App_DefineData SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, strSQL);
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
        /// 删除消息（协办/阅知/转发/传阅/沟通【非主办】）用户接口
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


        /// <summary>
        /// 用户接收事项后自定义处理
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngMessageID"></param>
        public override void AfterReceiveMessage(OracleTransaction trans, long lngMessageID, long lngUserID)
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

