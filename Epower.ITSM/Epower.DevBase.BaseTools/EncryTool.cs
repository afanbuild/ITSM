using System;
using HCTECHLib;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// ���ܽ���
	/// </summary>
	public class EncryTool
	{
		public EncryTool()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		static string CONST_ENCRYKEY = "LouDaoKejifayangguangdasukangsheng";
		/// <summary>
		/// ��Ĭ�ϼ�ֵ���м���
		/// </summary>
		/// <param name="strContent">Ҫ�����ַ�</param>
		/// <returns></returns>
		public static string EnCrypt(string strContent)
		{
			
			HCTECHLib.CryptClass cry = new CryptClass();
			return cry.EncryptMessageB(strContent,CONST_ENCRYKEY);
		}
		/// <summary>
		/// ��ָ����ֵ���м���
		/// </summary>
		/// <param name="strContent">Ҫ�����ַ�</param>
		/// <param name="strKey">�Զ����ֵ</param>
		/// <returns></returns>
		public static string EnCrypt(string strContent,string strKey)
		{
			
			HCTECHLib.CryptClass cry = new CryptClass();
			return cry.EncryptMessageB(strContent,strKey);
		}
		/// <summary>
		/// ��Ĭ�ϼ�ֵ���н���
		/// </summary>
		/// <param name="strContent">Ҫ�����ַ�</param>
		/// <returns></returns>
		public static string DeCrypt(string strContent)
		{
            if (!string.IsNullOrEmpty(strContent))
            {
                HCTECHLib.CryptClass cry = new CryptClass();
                return cry.DecryptMessageB(strContent, CONST_ENCRYKEY);
            }
            else
            {
                return string.Empty;
            }
		}
		/// <summary>
		/// ��ָ����ֵ���н���
		/// </summary>
		/// <param name="strContent">Ҫ�����ַ�</param>
		/// <param name="strKey">����ʱʹ�õļ�ֵ</param>
		/// <returns></returns>
		public static string DeCrypt(string strContent,string strKey)
		{
            if (!string.IsNullOrEmpty(strContent))
            {
                HCTECHLib.CryptClass cry = new CryptClass();
                return cry.DecryptMessageB(strContent, strKey);
            }
            else
            {
                return string.Empty;
            }
		}

	}
}
