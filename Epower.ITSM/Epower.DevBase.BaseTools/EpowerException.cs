using System;
using System.Resources;
using System.Reflection;

namespace Epower.DevBase.BaseTools
{
	/// <summary>
	/// 业务异常类，错误写入日志，自定义错误项能灵活增加，能追踪系统错误
	/// </summary>
	/// 作者:
	///		苏康胜
	/// 创建日期：
	///		2002-12-17
	/// </summary>
	public class EpowerException:Exception
	{
		/// <summary>
		/// 业务异常类构造函数
		/// </summary>
		/// <param name="lngErrNumber">错误编号</param>
		/// 从错误编号中获取错误描述信息
		public EpowerException(long lngErrNumber)
		{
			string strMessage;
			string strErrText;
			string strErrSource;
			Assembly assembly = Assembly.GetCallingAssembly();
			strErrSource = assembly.GetName().FullName;
			//strErrText = "根据编号从资源文件中读取。。。。(" + lngErrNumber + ")";
			strErrText = GetDescription(lngErrNumber) + "(" + lngErrNumber + ")";
			strMessage = " 业务组件运行错误。详细信息：\n"+
				"组件模块:   " + strErrSource + "\n" +
				"错误编号:   " + lngErrNumber.ToString() + "\n" +
				"错误来源:    " + "\n" +
				"错误描述:   " + strErrText;
			EventTool.EventLog(strMessage);
			throw new Exception(strErrText);
		}
		

		/// <summary>
		/// 业务异常类构造函数
		/// </summary>
		/// <param name="lngErrNumber">错误编号</param>
		///          从错误编号中获取错误描述信息
		/// <param name="strErrSource">错误来源</param>
		///          指发生错误的程序模块 
		public EpowerException(long lngErrNumber,string strErrSource)
		{
			string strMessage;
			string strErrText;
			strErrText = GetDescription(lngErrNumber) + " (" + lngErrNumber + ")";
			strMessage = " 业务组件运行错误。详细信息：\n"+
				"组件模块:   " + strErrSource + "\n" +
				"错误编号:   " + lngErrNumber.ToString() + "\n" +
				"错误来源:    " + "\n" +
				"错误描述:   " + strErrText;
			EventTool.EventLog(strMessage);
			throw new Exception(strErrText);
		}

		/// <summary>
		/// 业务异常类构造函数（用于重抛捕获的系统异常）
		/// </summary>
		/// <param name="strErrText">异常描述信息</param>
		/// <param name="strErrSource">发生异常的模块名称</param>
		/// <param name="strOriSource">发生异常的源异常来源</param>
        public EpowerException(string strErrText, string strErrSource, string strOriSource)
		{
			string strMessage;
			long lngOriErrNumber=-1;
			strMessage = "业务组件运行错误。详细信息：\n"+
				"组件模块:   " + strErrSource + "\n" +
				"错误编号:   " + lngOriErrNumber.ToString() + "\n" +
				"错误来源:   " + strOriSource + "\n" +
				"错误描述:   " + strErrText;
			
			EventTool.EventLog(strMessage);
			throw new Exception("业务组件运行错误。详细信息请见系统日志");
		}


		private string GetDescription(long lngNumber)
		{
			System.Resources.ResourceManager rm = new ResourceManager("Epower.DevBase.BaseTools.ErrResource", Assembly.GetExecutingAssembly());
			
			return StringTool.ReplaceCrlf(rm.GetString(lngNumber.ToString()),12);
			
		}
	}
}
