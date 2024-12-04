using System;
using System.Data;
using System.Data.OracleClient;
using MyComponent;
using System.Xml;
using System.IO;
using System.Collections;
using IappDataProcess;
using EpowerGlobal;

namespace appDataProcess.ActorExtends
{
	/// <summary>
	/// 地面局人员
	/// </summary>
	public class ActExtDMJ:IActorExtend
	{
		public ActExtDMJ()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 获取各个接口的具体实现
        /// </summary>
        /// <param name="lngStarterID">起草人编号</param>
        /// <param name="lngSenderID">发送人编号(当前用户编号)</param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="strFormValues"></param>
        /// <returns></returns>
        public override BaseCollection GetActors(long lngStarterID, long lngSenderID, long lngFlowID, long lngFlowModelID, long lngNodeModelID, string strFormValues)
        {
            BaseCollection bc = new BaseCollection();

            bc.Add(10016, "郑军民");
            //if (HttpContext.Current.Session["ExtCRMCustID"] != null)
            //{
            //}
            return bc;
        }
	}
}
