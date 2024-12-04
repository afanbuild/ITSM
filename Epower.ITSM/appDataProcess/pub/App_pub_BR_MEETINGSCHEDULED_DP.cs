using System;
using System.Collections.Generic;
using System.Text;
using IappDataProcess;
using Epower.DevBase.BaseTools;
using EpowerCom;
using System.Data;
using System.Data.OracleClient;
using System.Xml;

namespace appDataProcess.pub
{
    class App_pub_BR_MEETINGSCHEDULED_DP : IDataProcess
    {
        public App_pub_BR_MEETINGSCHEDULED_DP() { }

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

            strSQL = "SELECT * FROM BR_MeetingScheduled WHERE FlowID =" + lngFlowID.ToString();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            finally
            {
                ConfigToolApp.CloseConnection(cn);
            }
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

            strSQL = "SELECT * FROM BR_MeetingScheduled WHERE FlowID =" + lngFlowID.ToString();
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
        public override void SaveFieldValues(OracleTransaction trans, long lngFlowID, long lngFlowModelID, long lngNodeModelID, long lngActionID, long lngOpID, string strValues, long lngMessageID)
        {
            #region
            long lngNextID = 0;
            FieldValues fv = new FieldValues(strValues);
            //判断如果为手机上操作
            //if (fv.GetFieldValue("Flag").Value.ToLower() == "true")
            //{
            //    return;
            //}
            string strSQL = "SELECT FlowID FROM BR_MeetingScheduled WHERE FlowID=" + lngFlowID.ToString();
            OracleDataReader dr = OracleDbHelper.ExecuteReader(trans, CommandType.Text, strSQL);
            while (dr.Read())
            {
                lngNextID = (long)dr.GetDecimal(0);
                break;
            }
            dr.Close();


            string strID = "0";
            try
            {
                if (lngNextID == 0)
                {
                    strID = EpowerGlobal.EPGlobal.GetNextID("BR_MeetingScheduledID").ToString();

                    #region
                    strSQL = @"INSERT INTO BR_MeetingScheduled(
								  ID,
								  FlowID,
                                  FlowModelID,
							      NodeModelID,
                                  MeetingName,
                                  Title,
                                  Address,
                                  MeetingID,
                                  MeetingRoom,
                                  DepartmentID,
                                  DepartmentName,
                                  HostID,
                                  HostName,
                                  StartTime,
                                  EndTime,
                                  Phone,
                                  Service,
                                  RemarKs,
                                  datetime
						)
						VALUES(" +
                            strID + "," +
                            lngFlowID.ToString() + "," +
                            lngFlowModelID.ToString() + "," +
                            lngNodeModelID.ToString() + "," +
                            StringTool.SqlQ(fv.GetFieldValue("MeetingName").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Title").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Address").Value) + "," +
                            fv.GetFieldValue("MeetingID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("MeetingRoom").Value) + "," +
                            fv.GetFieldValue("DepartmentID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("DepartmentName").Value) + "," +
                            fv.GetFieldValue("HostID").Value + "," +
                            StringTool.SqlQ(fv.GetFieldValue("HostName").Value) + "," +
                            "to_date(" + StringTool.SqlQ(fv.GetFieldValue("StartTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" + "," +
                            "to_date(" + StringTool.SqlQ(fv.GetFieldValue("EndTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Phone").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("Service").Value) + "," +
                            StringTool.SqlQ(fv.GetFieldValue("RemarKs").Value) + "," +
                           "to_date(" + StringTool.SqlQ(fv.GetFieldValue("datetime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" +
                            ")";
                    #endregion
                }
                else
                {
                    #region
                    strSQL = @"UPDATE BR_MeetingScheduled Set " +
                                    " NodeModelID = " + lngNodeModelID.ToString() + "," +
                                    " FlowModelID = " + lngFlowModelID.ToString() + "," +
                                    " MeetingName = " + StringTool.SqlQ(fv.GetFieldValue("MeetingName").Value) + "," +
                                    " Title = " + StringTool.SqlQ(fv.GetFieldValue("Title").Value) + "," +
                                    " Address = " + StringTool.SqlQ(fv.GetFieldValue("Address").Value) + "," +
                                    " MeetingID = " + fv.GetFieldValue("MeetingID").Value + "," +
                                    " MeetingRoom = " + StringTool.SqlQ(fv.GetFieldValue("MeetingRoom").Value) + "," +
                                    " DepartmentID = " + fv.GetFieldValue("DepartmentID").Value + "," +
                                    " DepartmentName = " + StringTool.SqlQ(fv.GetFieldValue("DepartmentName").Value) + "," +
                                    " HostID = " + fv.GetFieldValue("HostID").Value + "," +
                                    " HostName = " + StringTool.SqlQ(fv.GetFieldValue("HostName").Value) + "," +
                                     "StartTime = to_date(" + StringTool.SqlQ(fv.GetFieldValue("StartTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" + "," +
                                    " EndTime =  to_date(" + StringTool.SqlQ(fv.GetFieldValue("EndTime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" + "," +
                                    " Phone = " + StringTool.SqlQ(fv.GetFieldValue("Phone").Value) + "," +
                                    " Service = " + StringTool.SqlQ(fv.GetFieldValue("Service").Value) + "," +
                                    " RemarKs = " + StringTool.SqlQ(fv.GetFieldValue("RemarKs").Value) + "," +
                                    "datetime=to_date(" + StringTool.SqlQ(fv.GetFieldValue("datetime").Value) + ",'yyyy-MM-dd HH24:mi:ss')" +
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
                strSQL = "DELETE BR_MeetingScheduled WHERE FlowID = " + lngFlowID.ToString();
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
        //public override void DoFlowEnd(OracleTransaction trans, long lngID)
        //{
        //    string strSQL = "";
        //    try
        //    {
        //        strSQL = "UPDATE BR_OnLineSubmit SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
        //        OracleDbHelper.ExecuteNonQuery(trans, strSQL);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        /// <summary>
        /// 统一的归档处理接口


        /// </summary>
        /// <param name="lngID">流程ID</param>
        //public override void DoFlowAbort(OracleTransaction trans, long lngID)
        //{
        //    string strSQL = "";
        //    try
        //    {
        //        strSQL = "UPDATE BR_OnLineSubmit SET EndDate = sysdate WHERE FlowID = " + lngID.ToString();
        //        OracleDbHelper.ExecuteNonQuery(trans, strSQL);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

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
