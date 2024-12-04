/*
 *              IP地址解析类
 * 
 * 提供功能如下:〔均为静态方法〕
 * 
 *	1、验证IP地址的有效性
 *  2、判断某IP地址是否在给给定IP范围内
 * 
 * 创建时间：2005-04-05
 * 创建人：duanqs 
 * 
 * */
using System;
namespace Epower.DevBase.BaseTools
{

	/// <summary>
	/// IP解析的一些相关方法。
	/// </summary>
	public class IPParse
	{
		public IPParse()
		{
			
		}

		/// <summary>
		/// 验证IP地址的有效性
		/// </summary>
		/// <param name="strIP">要验证的IP地址(字符)</param>
		/// <returns>返回验证结果(Bool类型)</returns>
		public static bool IPValidate(string strIP)
		{
			
			//空串
			if(strIP.Length==0)
				return false;
			
			//无.
			if(strIP.IndexOf(".")==0)
				return false;

			//不能含字符.除外
			string sTmp=strIP.Replace(".","");
			for(int n=0;n<sTmp.Length;n++)
			{
				if(!char.IsNumber(sTmp,n))
					return false;
			}

			string [] arrIPElements= strIP.Split('.');

			//分割为4个子串
			if(arrIPElements.Length!=4)
				return false;
			
			//验证0-255
			foreach(string strIPElement in arrIPElements)
			{
				if(strIPElement=="")
					return false;

				if(int.Parse(strIPElement)<0 || int.Parse(strIPElement)>255)
					return false;

				if(strIPElement.Length!=1 && strIPElement.Substring(0,1)=="0")
					return false;
			}
			return true;
		}



		/// <summary>
		/// 验证IP地址是否在指定的范围内
		/// </summary>
		/// <param name="strIP">需要验证的IP地址(字符)</param>
		/// <param name="strBeginIP">指定IP范围的开始IP地址</param>
		/// <param name="strEndIP">指定IP范围的结束IP地址</param>
		/// <returns>返回验证结果(BOOL类型)</returns>
		public static bool IsIncluded(string strIP,string strBeginIP,string strEndIP)
		{
			if(!IPValidate(strIP) || !IPValidate(strBeginIP) || !IPValidate(strEndIP))
			{
				throw new Exception("IP地址格式错误");
			}
			
			strIP=FormatIPElement(strIP);
			strBeginIP=FormatIPElement(strBeginIP);
			strEndIP=FormatIPElement(strEndIP);

			if(string.Compare(strIP,strBeginIP)>=0  && string.Compare(strIP,strEndIP)<=0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 格式化IP地址，3.4.45.56 ＝> 003.004.045.056便于比较
		/// </summary>
		/// <param name="strIP">要格式化的IP地址</param>
		/// <returns>格式化后的IP地址</returns>
		private static string FormatIPElement(string strIP)
		{
			string [] arrIPElements= strIP.Split('.');
			string strTmp="";
			for(int n=0; n<arrIPElements.Length ;n++)
			{	
				strTmp=strTmp+arrIPElements[n].PadLeft(3,'0')+".";
			}
			strTmp=strTmp.TrimEnd('.');
			return strTmp;
		}

	}
}
