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
	/// ImplDataProcess ��ժҪ˵����
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

                //�����Զ�����������APPID���Զ�ʵ������Ӧ�Ķ���
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
            //2008-02-10 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
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
        /// ��ͨʱ�����ֶ���Ϣͳһ���
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
        /// ������Ϣ������
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngOpID"></param>
        /// <returns></returns>
        public DataTable GetFieldsDataTable(long lngFlowID, long lngOpID)
        {
            return dp.GetFieldsDataTable(lngFlowID, lngOpID);
        }

		/// <summary>
		/// ���̷��ʹ���֪ͨ�ӿ�(�����̷���/����������ύǰִ��)
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID">����ʵ��ID</param>
		/// <param name="lngNodeModelID"></param>
		/// <param name="lngFlowModelID"></param>
		/// <param name="lngOpID"></param>
		/// <param name="strXMlFieldValue"></param>
		/// <param name="strReceivers">�����ߺ���Ϣֵ���ַ��� ��ʽ: ������ID|��ϢID,������ID|��ϢID,... (���������Ϣ)</param>
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
        /// �����ֶ���Ϣͳһ���
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
		/// ����ʱӦ��ִ�еľ���ʵ�ֵĽӿ�
		/// ���ڻ��ղ�����Ҫɾ��һЩ���ݣ������û��Զ����������ǰ��ִ��
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
        /// �ָ�����ִ�е�ҵ��ӿ�
        /// 2009-02-05 ����
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngFlowID"></param>
        /// <param name="flowStartTime">��������ʱ��</param>
        /// <param name="flowPauseTime">������ͣʱ��</param>
        /// <param name="flowContTime">���ָ̻�ʱ��</param>
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
        /// ��ͣ����ִ�е�ҵ��ӿ�
        /// 2009-02-05����
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
        /// ��֪״̬�±����ֶ���Ϣͳһ���
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
		/// �˻�ʱӦ��ִ�еľ���ʵ�ֵĽӿ�
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
        /// ����ʱӦ��ִ�еľ���ʵ�ֵĽӿ�
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
		/// ��ȡ�ֶ���Ϣ��ͳһ���
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
        /// ��ȡҵ��������ݣ�XML��
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public string  GetBussinessShotValues(long lngFlowID)
        {
            string sRet = "";
            //ͨ��������ʽ������ʷ�汾
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
		/// ������Ϣ������
		/// </summary>
		/// <param name="lngFlowID"></param>
		/// <param name="lngOpID"></param>
		/// <returns></returns>
		public DataSet GetFieldsDataSet(long lngFlowID,long lngOpID)
		{
			return dp.GetFieldsDataSet(lngFlowID,lngOpID);
		}

		/// <summary>
		/// ͳһ�鵵���
		/// �鵵����ʱ��ע�⣬�ھ���ʵ���ж�������£����ܴ���һ������״̬
		///                    ����Ӧ�õĹ鵵����Դ�ڵ�������˾�ṩ�Ľӿڳ���
		///                    ��������¸��Կ����Լ���������ʱ�����������
		///                    �κ���;���жϵ������ӿ��Ƿ���ȷ������������׳�
		///                    һ������������ߣ��ɵ����߽���������ơ�
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngID">����ʵ��ID</param>
		public void DoFlowEnd(OracleTransaction trans,long lngID)
		{
			dp.DoFlowEnd(trans,lngID);
		}


        /// <summary>
        /// ��ֹʱͳһ�鵵���
        /// �鵵����ʱ��ע�⣬�ھ���ʵ���ж�������£����ܴ���һ������״̬
        ///                    ����Ӧ�õĹ鵵����Դ�ڵ�������˾�ṩ�Ľӿڳ���
        ///                    ��������¸��Կ����Լ���������ʱ�����������
        ///                    �κ���;���жϵ������ӿ��Ƿ���ȷ������������׳�
        ///                    һ������������ߣ��ɵ����߽���������ơ�
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="lngID">����ʵ��ID</param>
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
		/// ɾ��Ӧ����ص���Ϣ��
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="lngFlowID"></param>
		/// <param name="lngOpID"></param>
		public void DeleteFieldValues(OracleTransaction trans,long lngFlowID,long lngOpID)
		{
			dp.DeleteFieldValues(trans,lngFlowID,lngOpID);
		}

		/// <summary>
		///  ��Ӻͷ������̺��û������ӿ�
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
        /// �����ͷ���ʱ����MESSAGEʱ,�û��Զ��崦��ӿ�
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
        /// ɾ����Ϣ��Э��/��֪/ת��/����/��ͨ�������졿���û��ӿ�
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
		///  ʵ���û��Զ�����ο��� ������Ա ���
		///           ��:  �Զ����� (���ݹ�����)
		///                �ӱ�����ȡ  ��
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
		/// �û�����������Զ��崦��
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
		/// �жϻ�ǩ�����Ƿ���Խ���
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
