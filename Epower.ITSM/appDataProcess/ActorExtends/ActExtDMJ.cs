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
	/// �������Ա
	/// </summary>
	public class ActExtDMJ:IActorExtend
	{
		public ActExtDMJ()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        /// <summary>
        /// ��ȡ�����ӿڵľ���ʵ��
        /// </summary>
        /// <param name="lngStarterID">����˱��</param>
        /// <param name="lngSenderID">�����˱��(��ǰ�û����)</param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID">����ģ�ͱ��</param>
        /// <param name="lngNodeModelID">����ģ�ͱ��</param>
        /// <param name="strFormValues"></param>
        /// <returns></returns>
        public override BaseCollection GetActors(long lngStarterID, long lngSenderID, long lngFlowID, long lngFlowModelID, long lngNodeModelID, string strFormValues)
        {
            BaseCollection bc = new BaseCollection();

            bc.Add(10016, "֣����");
            //if (HttpContext.Current.Session["ExtCRMCustID"] != null)
            //{
            //}
            return bc;
        }
	}
}
