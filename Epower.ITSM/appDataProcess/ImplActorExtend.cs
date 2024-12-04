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
	/// 环节人员扩展接口实现类
	/// </summary>
	public class ImplActorExtend
	{
		long mlngActorExtID=0;
		
		IActorExtend ae;

        private static Hashtable actorExtendCache = Hashtable.Synchronized(new Hashtable());


		public ImplActorExtend(long lngActorExtID)
		{
			this.mlngActorExtID = lngActorExtID;

            string hashKey = "ImplAE_" + lngActorExtID.ToString();

            IActorExtend iae = actorExtendCache[hashKey] as IActorExtend;
            if (iae == null)
            {

                object newInstance = Assembly.GetExecutingAssembly().CreateInstance(GetAppDataProcessAssembly(lngActorExtID));
                //object newInstance=Assembly.GetAssembly().CreateInstance(GetAppDataProcessAssembly(mlngAppID));
                ae = (IActorExtend)newInstance;

                actorExtendCache[hashKey] = iae;


            }
            else
            {
                ae = iae;
            }
			
			

		}

		private static string GetAppDataProcessAssembly(long lngActorExtID)
		{
			string strSQL = "";
			OracleDataReader dr;

			string strConn = System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"];;

			string str = "appDataProcess.ActorExtends.";

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {
                DataTable dt = EpSqlCacheHelper.GetDataTableFromCache("actorextdef");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("actorextid=" + lngActorExtID.ToString());

                    if (drs.Length > 0)
                    {
                        str = str + drs[0]["AssemblyName"].ToString();
                    }
                }
                dt.Dispose();
            }
            else
            {
                strSQL = "SELECT AssemblyName FROM es_actorextdef WHERE actorextid=" + lngActorExtID.ToString();
                dr = OracleDbHelper.ExecuteReader(strConn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    str = str + dr.GetString(0).Trim();
                    break;
                }
                dr.Close();
            }
			return str;

		}


        /// <summary>
        /// 获取各个接口的具体实现
        /// </summary>
        /// <param name="lngStarterID">起草人编号</param>
        /// <param name="lngSenderID">发送人编号(当前用户编号)</param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="strFormValue"></param>
        /// <returns></returns>
        public BaseCollection GetActors(long lngStarterID, long lngSenderID, long lngFlowID, long lngFlowModelID, long lngNodeModelID, string strFormValue)
        {
            try
            {
                return ae.GetActors(lngStarterID, lngSenderID, lngFlowID, lngFlowModelID, lngNodeModelID, strFormValue);
            }
            catch (Exception e)
            {
                return new BaseCollection();
            }
        }

        /// <summary>
        ///  交接、调度、协作、传阅、沟通扩展接口，获取各个接口的具体实现
        /// </summary>
        /// <param name="lngStarterID">起草人编号</param>
        /// <param name="lngSenderID">发送人编号(当前用户编号)</param>
        /// <param name="lngFlowID">流程编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="strFormValues">表单信息</param>
        /// <param name="lngNodeID">流程环节编号</param>
        /// <returns></returns>
        public BaseCollection GetSpecialActors(long lngStarterID, long lngSenderID, long lngFlowID, long lngFlowModelID, long lngNodeModelID,string strFormValues,long lngNodeID)
        {
            try
            {
                //return ae.GetSpecialActors(lngStarterID, lngSenderID, lngFlowID, lngFlowModelID, lngNodeModelID, strFormValues, lngNodeID);
                return null;
            }
            catch (Exception e)
            {
                return new BaseCollection();
            }
        }
	}
}
