using System;
using System.Resources;
using System.Reflection;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// ҵ���쳣�࣬����д����־���Զ����������������ӣ���׷��ϵͳ����
	/// </summary>
	/// ����:
	///		�տ�ʤ
	/// �������ڣ�
	///		2002-12-17
	/// </summary>
	public class EpowerException:Exception
	{
		/// <summary>
		/// ҵ���쳣�๹�캯��
		/// </summary>
		/// <param name="lngErrNumber">������</param>
		/// �Ӵ������л�ȡ����������Ϣ
		public EpowerException(long lngErrNumber)
		{
			string strMessage;
			string strErrText;
			string strErrSource;
			Assembly assembly = Assembly.GetCallingAssembly();
			strErrSource = assembly.GetName().FullName;
			//strErrText = "���ݱ�Ŵ���Դ�ļ��ж�ȡ��������(" + lngErrNumber + ")";
			strErrText = GetDescription(lngErrNumber) + "(" + lngErrNumber + ")";
			strMessage = " ҵ��������д�����ϸ��Ϣ��\n"+
				"���ģ��:   " + strErrSource + "\n" +
				"������:   " + lngErrNumber.ToString() + "\n" +
				"������Դ:    " + "\n" +
				"��������:   " + strErrText;
			EventTool.EventLog(strMessage);
			throw new Exception(strErrText);
		}
		

		/// <summary>
		/// ҵ���쳣�๹�캯��
		/// </summary>
		/// <param name="lngErrNumber">������</param>
		///          �Ӵ������л�ȡ����������Ϣ
		/// <param name="strErrSource">������Դ</param>
		///          ָ��������ĳ���ģ�� 
		public EpowerException(long lngErrNumber,string strErrSource)
		{
			string strMessage;
			string strErrText;
			strErrText = GetDescription(lngErrNumber) + " (" + lngErrNumber + ")";
			strMessage = " ҵ��������д�����ϸ��Ϣ��\n"+
				"���ģ��:   " + strErrSource + "\n" +
				"������:   " + lngErrNumber.ToString() + "\n" +
				"������Դ:    " + "\n" +
				"��������:   " + strErrText;
			EventTool.EventLog(strMessage);
			throw new Exception(strErrText);
		}

		/// <summary>
		/// ҵ���쳣�๹�캯�����������ײ����ϵͳ�쳣��
		/// </summary>
		/// <param name="strErrText">�쳣������Ϣ</param>
		/// <param name="strErrSource">�����쳣��ģ������</param>
		/// <param name="strOriSource">�����쳣��Դ�쳣��Դ</param>
        public EpowerException(string strErrText, string strErrSource, string strOriSource)
		{
			string strMessage;
			long lngOriErrNumber=-1;
			strMessage = "ҵ��������д�����ϸ��Ϣ��\n"+
				"���ģ��:   " + strErrSource + "\n" +
				"������:   " + lngOriErrNumber.ToString() + "\n" +
				"������Դ:   " + strOriSource + "\n" +
				"��������:   " + strErrText;
			
			EventTool.EventLog(strMessage);
			throw new Exception("ҵ��������д�����ϸ��Ϣ���ϵͳ��־");
		}


		private string GetDescription(long lngNumber)
		{
			System.Resources.ResourceManager rm = new ResourceManager("Epower.DevBase.BaseTools.ErrResource", Assembly.GetExecutingAssembly());
			
			return StringTool.ReplaceCrlf(rm.GetString(lngNumber.ToString()),12);
			
		}
	}
}
