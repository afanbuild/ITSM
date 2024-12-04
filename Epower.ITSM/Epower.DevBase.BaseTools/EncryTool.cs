using System;
using HCTECHLib;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// 加密解密
	/// </summary>
	public class EncryTool
	{
		public EncryTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		static string CONST_ENCRYKEY = "LouDaoKejifayangguangdasukangsheng";
		/// <summary>
		/// 按默认键值进行加密
		/// </summary>
		/// <param name="strContent">要加密字符</param>
		/// <returns></returns>
		public static string EnCrypt(string strContent)
		{
			
			HCTECHLib.CryptClass cry = new CryptClass();
			return cry.EncryptMessageB(strContent,CONST_ENCRYKEY);
		}
		/// <summary>
		/// 按指定键值进行加密
		/// </summary>
		/// <param name="strContent">要加密字符</param>
		/// <param name="strKey">自定义键值</param>
		/// <returns></returns>
		public static string EnCrypt(string strContent,string strKey)
		{
			
			HCTECHLib.CryptClass cry = new CryptClass();
			return cry.EncryptMessageB(strContent,strKey);
		}
		/// <summary>
		/// 按默认键值进行解密
		/// </summary>
		/// <param name="strContent">要解密字符</param>
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
		/// 按指定键值进行解密
		/// </summary>
		/// <param name="strContent">要解密字符</param>
		/// <param name="strKey">加密时使用的键值</param>
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
