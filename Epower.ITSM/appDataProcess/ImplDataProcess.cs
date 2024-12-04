using System;
using System.Reflection;
using System.Data;
using System.Xml;
using System.Data.OracleClient;
using System.Collections;
using Epower.DevBase.BaseTools;
using IappDataProcess;
using EpowerGlobal;

namespace appDataProcess
{
	/// <summary>
	/// ImplDataProcess 的摘要说明。
	/// </summary>
	public class ImplDataProcess
	{
		long mlngAppID=0;
		IDataProcess dp;

        private static Hashtable appsCache = Hashtable.Synchronized(new Hashtable());

		public ImplDataProcess(long lngAppID)
		{
			this.mlngAppID = lngAppID;

            string hashKey = "ImplDP_" + lngAppID.ToString();

            IDataProcess idp = appsCache[hashKey] as IDataProcess;
            if (idp == null)
            {

                //后期自动棒定，根据APPID，自动实例化对应的对象
                object newInstance = Assembly.GetExecutingAssembly().CreateInstance(GetAppDataProcessAssembly(mlngAppID));
                //object newInstance=Assembly.GetAssembly().CreateInstance(GetAppDataProcessAssembly(mlngAppID));
                dp = (IDataProcess)newInstance;

                appsCache[hashKey] = dp;


            }
            else
            {
                dp = idp;
            }

		}

		private static string GetAppDataProcessAssembly(long lngAppID)
		{
            string strSQL = "";
            OracleDataReader dr;

            string strConn = System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]; ;

            string str = "appDataProcess.";
            //2008-02-10 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = EpSqlCacheHelper.GetDataTableFromCache("app");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("Appid=" + lngAppID.ToString());

                    if (drs.Length > 0)
                    {
                        str = str + drs[0]["Project"].ToString() + ".App_" + drs[0]["Project"].ToString() + "_" + drs[0]["AppCode"].ToString() + "_DP";
                    }
                }
                dt.Dispose();

            }
            else
            {
                strSQL = "SELECT Project,AppCode FROM Es_App WHERE AppID=" + lngAppID.ToString();
                dr = OracleDbHelper.ExecuteReader(strConn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    str = str + dr.GetString(0).Trim() + ".App_" + dr.GetString(0).Trim() + "_" + dr.GetString(1).Trim() + "_DP";
                    break;
                }
                dr.Close();
            }

            return str;



		}


        /// <summary>
        /// 沟通时保存字段信息统一入口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="lngOpID"></param>
        /// <param name="strValues"></param>
        /// <param name="lngMessageID"></param>
        public void SaveFieldValuesForCommunic(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngOpID, string strValues, long lngMessageID)
        {
            try
            {
                dp.SaveFieldValuesForCommunic(trans, lngFlowID, lngNodeModelID, lngFlowModelID, lngOpID, strValues, lngMessageID);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 返回信息项结果表
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public DataTable GetFieldsDataTable(long lngFlowID, long lngOpID)
        {
            return dp.GetFieldsDataTable(lngFlowID, lngOpID);
        }

		/// <summary>
		/// 流程发送处理通知接口(在流程发送/新增处理的提交前执行)
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID">流程实例ID</param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strXMlFieldValue"></param>
		/// <param name="strReceivers">接收者和消息值列字符串 格式: 接收者ID|消息ID,接收者ID|消息ID,... (仅主办的消息)</param>
		/// <param name="lngMessageID"></param>
		public void NotifyMessage(OracleTransaction trans,long lngID,long lngNodeModelID,long lngFlowModelID,long lngOpID,string strXMlFieldValue,string strReceivers,long lngMessageID)
		{
			try
			{
				dp.NotifyMessage(trans,lngID,lngNodeModelID,lngFlowModelID,lngOpID,strXMlFieldValue,strReceivers,lngMessageID);
			}
			catch(Exception e)
			{
				throw e;
			}
		}


		/// <summary>
        /// 保存字段信息统一入口
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngActionID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strValues"></param>
		/// <param name="lngMessageID"></param>
		public void SaveFieldValues(OracleTransaction trans,long lngFlowID,long lngNodeModelID,long lngFlowModelID,long lngActionID,long lngOpID,string strValues,long lngMessageID)
		{
			try
			{
				dp.SaveFieldValues(trans,lngFlowID,lngNodeModelID,lngFlowModelID,lngActionID,lngOpID,strValues,lngMessageID);
			}
			catch
			{
				throw ;
			}
		}


		/// <summary>
		/// 回收时应用执行的具体实现的接口
		/// 由于回收操作需要删除一些内容，所以用户自定义操作会在前段执行
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngMessageID"></param>
		public void TakeBackUserProcess(OracleTransaction trans,long lngFlowID,long lngMessageID)
		{
			try
			{
				dp.TakeBackUserProcess(trans,lngFlowID,lngMessageID);
			}
			catch
			{
				throw;
			}
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
        public void DealFlowContinue(OracleTransaction trans, long lngFlowID, DateTime flowStartTime, DateTime flowPauseTime, DateTime flowContTime, long lngUserID)
        {
            try
            {
                dp.DealFlowContinue(trans, lngFlowID, flowStartTime, flowPauseTime, flowContTime, lngUserID);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 暂停流程执行的业务接口
        /// 2009-02-05增加
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="strRemark"></param>
        public void DealFlowPause(OracleTransaction trans, long lngFlowID, long lngUserID, string strRemark)
        {
            try
            {
                dp.DealFlowPause(trans, lngFlowID, lngUserID, strRemark);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


		/// <summary>
        /// 阅知状态下保存字段信息统一入口
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strValues"></param>
		/// <param name="lngMessageID"></param>
		public void SaveFieldValuesForRead(OracleTransaction trans,long lngFlowID,long lngNodeModelID,long lngFlowModelID,long lngOpID,string strValues,long lngMessageID)
		{
			try
			{
				dp.SaveFieldValuesForRead(trans,lngFlowID,lngNodeModelID,lngFlowModelID,lngOpID,strValues,lngMessageID);
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// 退回时应用执行的具体实现的接口
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngMessageID"></param>
		public void SendBackUserProcess(OracleTransaction trans,long lngFlowID,long lngMessageID)
		{
			try
			{
				dp.SendBackUserProcess(trans,lngFlowID,lngMessageID);
			}
			catch
			{
				throw;
			}
		}

        /// <summary>
        /// 驳回时应用执行的具体实现的接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngNodeModelID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <param name="strValues"></param>
        /// <param name="lngMessageID"></param>
        /// <param name="intSpecRight"></param>
        /// <param name="strOpinionValue"></param>
        /// <param name="strRecMsgs"></param>
        public void RejectedBackUserProcess(OracleTransaction trans, long lngFlowID, long lngNodeModelID, long lngFlowModelID, long lngMessageID, string strOpinionValue, long lngReceiverID, long lngNextNodeID)
        {
            try
            {
                //dp.RejectedBackUserProcess(trans, lngFlowID, lngNodeModelID, lngFlowModelID, lngMessageID, strOpinionValue,lngReceiverID,lngNextNodeID);
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// 获取字段信息的统一入口
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="?"></param>
		/// <param name="?"></param>
		/// <returns></returns>
		public string GetFieldValues(long lngFlowID,long lngOpID)
		{
			return dp.GetFieldValues(lngFlowID,lngOpID);
		}


        /// <summary>
        /// 获取业务快照数据（XML）
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public string  GetBussinessShotValues(long lngFlowID)
        {
            string sRet = "";
            //通过错误处理方式兼容历史版本
            try
            {
                sRet = dp.GetBussinessShotValues(lngFlowID);
            }
            catch
            {
            }

            return sRet;

        }



		/// <summary>
		/// 返回信息项结果集
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngOpID"></param>
		/// <returns></returns>
		public DataSet GetFieldsDataSet(long lngFlowID,long lngOpID)
		{
			return dp.GetFieldsDataSet(lngFlowID,lngOpID);
		}

		/// <summary>
		/// 统一归档入口
		/// 归档处理时请注意，在具体实现中多数情况下，不能处于一个事务状态
		///                    具体应用的归档可能源于第三方公司提供的接口程序，
		///                    这种情况下各自控制自己的事务，这时传入的事务不做
		///                    任何用途，判断第三方接口是否正确，如果错误则抛出
		///                    一个错误给调用者，由调用者进行事务控制。
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID">流程实例ID</param>
		public void DoFlowEnd(OracleTransaction trans,long lngID)
		{
			dp.DoFlowEnd(trans,lngID);
		}


        /// <summary>
        /// 终止时统一归档入口
        /// 归档处理时请注意，在具体实现中多数情况下，不能处于一个事务状态
        ///                    具体应用的归档可能源于第三方公司提供的接口程序，
        ///                    这种情况下各自控制自己的事务，这时传入的事务不做
        ///                    任何用途，判断第三方接口是否正确，如果错误则抛出
        ///                    一个错误给调用者，由调用者进行事务控制。
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID">流程实例ID</param>
        public void DoFlowAbort(OracleTransaction trans, long lngID)
        {
            try
            {
                dp.DoFlowAbort(trans, lngID);
            }
            catch
            {
            }
        }


		/// <summary>
		/// 删除应用相关的信息。
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngOpID"></param>
		public void DeleteFieldValues(OracleTransaction trans,long lngFlowID,long lngOpID)
		{
			dp.DeleteFieldValues(trans,lngFlowID,lngOpID);
		}

		/// <summary>
		///  添加和发送流程后用户开发接口
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="lngMessageID"></param>
		public void SendFlowFinish(OracleTransaction trans,long lngFlowID,long lngNodeModelID,long lngFlowModelID,long lngOpID,long lngMessageID)
		{
			try

			{
				dp.SendFlowFinish(trans,lngFlowID,lngNodeModelID,lngFlowModelID,lngOpID,lngMessageID);

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
		/// <param name="lngNodeID"></param>
		/// <param name="lngMessageID"></param>
		/// <param name="intActorType"></param>
		/// <param name="sFActor"></param>
		public  void AfterMessageAddedForAddOrSend(OracleTransaction trans,long lngFlowID,long lngNodeID,long lngMessageID,int intActorType,string sFActor)
		{
			try
			{
				dp.AfterMessageAddedForAddOrSend(trans,lngFlowID,lngNodeID,lngMessageID,intActorType,sFActor);

			}
			catch

			{
				throw;
			}
		}

        /// <summary>
        /// 删除消息（协办/阅知/转发/传阅/沟通【非主办】）用户接口
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngMessageID"></param>
        public void DeleteMessage(OracleTransaction trans, long lngMessageID)
        {
            try
            {
                dp.DeleteMessage(trans, lngMessageID);
            }
            catch (Exception e)
            {
                throw e;
            }
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
		public void UserInterfaceReceivers(long lngAppID,long lngOpID,long lngFlowID,long lngNodeID,long lngMessageID,long lngFlowModelID,long lngNodeModelID,long lngUserID,string strFormXMLValue,ref XmlDocument xmlDoc)
		{
			try

			{
				dp.UserInterfaceReceivers(lngAppID,lngOpID,lngFlowID,lngNodeID,lngMessageID,lngFlowModelID,lngNodeModelID,lngUserID,strFormXMLValue,ref xmlDoc);

			}
			catch

			{
				throw;
			}
		}

		/// <summary>
		/// 用户接收事项后自定义处理
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngMessageID"></param>
		/// <param name="lngUserID"></param>
		public void AfterReceiveMessage(OracleTransaction trans,long lngMessageID,long lngUserID)
		{
			try
			{
				dp.AfterReceiveMessage(trans,lngMessageID,lngUserID);

			}
			catch

			{
				throw;
			}

		}


		/// <summary>
		/// 判断会签环节是否可以结束
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngMessageID"></param>
		/// <returns></returns>
		public bool IsInfluxMessageFinished(OracleTransaction trans,long lngMessageID)
		{
			try
			{
				return dp.IsInfluxMessageFinished(trans,lngMessageID);

			}
			catch
			{
				return false;
			}
			
		}


	}
}
