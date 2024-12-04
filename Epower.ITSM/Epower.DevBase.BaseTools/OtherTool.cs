using System;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// OtherTool 的摘要说明。
	/// </summary>
	public class OtherTool
	{
        /// <summary>
        /// 
        /// </summary>
		public OtherTool()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		private static string mstrTmp="";
		/// <summary>
		/// Function       ：十进制整数转换为固定长度的2进制字符串,长度不够时在前面补0
		///	Parameter      ：intSource  十进制整数  bitNumber 位数   返回： 2进制字符串
		/// </summary>
		/// <param name="intSource"></param>
		/// <param name="bitNumber"></param>
		/// <returns></returns>
		/// 
		public static string DecToBin(int intSource,int bitNumber)
		{
			string strTmp;
			int intI;
			strTmp=DecToBin(intSource);
			for (intI = strTmp.Length;intI < bitNumber;intI++)
				strTmp = "0" + strTmp;
			mstrTmp = "";
			return strTmp;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intSource"></param>
        /// <returns></returns>
		public static string DecToBin(int intSource)
		{
			string strTmp;
    
			strTmp = "";
			if (intSource < 2)
			{
				strTmp = intSource.ToString().Trim().Substring(intSource.ToString().Trim().Length -1, 1) + mstrTmp;
				mstrTmp = "";
				return strTmp;
			}
			else
			{
				mstrTmp = ((int)(intSource % 2)).ToString().Trim().Substring(((int)(intSource % 2)).ToString().Trim().Length-1, 1) + mstrTmp;
				return DecToBin((intSource / 2));
			}
     

		}

		/// <summary>
		///		'Function       ：2进制字符串转换为10进制整数
		///		'Parameter      ：BinStr  2进制字符串   返回： 10进制整数
		/// </summary>
		/// <param name="BinStr"></param>
		/// <returns></returns>
		public static double BinToDec(string BinStr)
		{
			int intI;
			double dblTmp=0;
			for (intI = 0;intI<BinStr.Length;intI++)
				dblTmp = dblTmp + int.Parse(BinStr.Substring(intI, 1)) * Math.Pow(2, BinStr.Length - intI - 1);
        
			
			return dblTmp;
			
			
		}


		/// <summary>
		/// 将当前日期增加N个工作日
		/// </summary>
		/// <param name="nDays">正整数</param>
		/// <returns></returns>
		public static DateTime AddWorkDay( int nDays)
		{
			return AddWorkDay(DateTime.Now,nDays);
		}
		
		/// <summary>
		/// 将当指定日期增加N个工作日
		/// </summary>
		/// <param name="currDay"></param>
		/// <param name="nDays"></param>
		/// <returns></returns>
		public static DateTime AddWorkDay(DateTime currDay, int nDays)
		{
			if(nDays<0)
				return currDay;
			DateTime now=currDay;
			int nCount=0;
			while(true)
			{
				if(nCount==nDays)
					return now;
				now=now.AddDays(1);
				if(!(now.DayOfWeek ==DayOfWeek.Sunday || now.DayOfWeek==DayOfWeek.Saturday))
					nCount++;
			}
		}
	}
}
