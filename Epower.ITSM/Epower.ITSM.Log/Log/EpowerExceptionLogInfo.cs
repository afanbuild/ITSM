using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// 错务日志处理类类
	/// </summary>
	public class EpowerExceptionLogInfo
	{
        string _ExceptionID = string.Empty;
		string _IPAddress = string.Empty;
		string _Message = string.Empty;
		string _AppID = string.Empty;
		string _StackTrace = string.Empty;
		string _TargetSite = string.Empty;
		string _PostURL = string.Empty;
		string _PostDateTime = System.DateTime.Now.ToString();
		string _UserID = string.Empty;
		string _UserName = string.Empty;
		string _Level = string.Empty;
		string _Memo = string.Empty;
		string _Status = string.Empty;
		string _ExceptionType = string.Empty;
		string _Developer = string.Empty;
		string _DeveloperMemo = string.Empty;
		string _DealDateTime = string.Empty;

        public EpowerExceptionLogInfo()
		{
			
		}

		/// <summary>
		/// 错误日志ID号
		/// </summary>
		public string ID
		{
			get{return _ExceptionID;}
			set{_ExceptionID = value;}
		}

		/// <summary>
		/// 操作者IP号
		/// </summary>
		public string IPAddress
		{
			get{return _IPAddress;}
			set{_IPAddress = value;}
		}

		/// <summary>
		/// 异常标题
		/// </summary>
		public string Message
		{
			get{return _Message;}
			set{_Message = value;}
		}

		/// <summary>
		/// 应用ID号
		/// </summary>
		public string AppID
		{
			get{return _AppID;}
			set{_AppID = value;}
		}

		/// <summary>
		/// 详细异常信息
		/// </summary>
		public string StackTrace
		{
			get{return _StackTrace;}
			set{_StackTrace = value;}
		}

		/// <summary>
		/// 异常的方法
		/// </summary>
		public string TargetSite
		{
			get{return _TargetSite;}
			set{_TargetSite = value;}
		}

		/// <summary>
		/// 提交URL
		/// </summary>
		public string PostURL
		{
			get{return _PostURL;}
			set{_PostURL = value;}
		}
		
		/// <summary>
		/// 提交时间
		/// </summary>
		public string PostDateTime
		{
			get{return _PostDateTime;}
			set{_PostDateTime = value;}
		}

		/// <summary>
		/// 操作者ID号
		/// </summary>
		public string UserID
		{
			get{return _UserID;}
			set{_UserID = value;}
		}

		/// <summary>
		/// 操作者姓名
		/// </summary>
		public string UserName
		{
			get{return _UserName;}
			set{_UserName = value;}
		}

		/// <summary>
		/// 优先级
		/// </summary>
		public string Level
		{
			get{return _Level;}
			set{_Level = value;}
		}

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get{return _Memo;}
			set{_Memo = value;}
		}

		/// <summary>
		/// 处理状态
		/// </summary>
		public string Status
		{
			get{return _Status;}
			set{_Status = value;}
		}

		/// <summary>
		/// 异常类别
		/// </summary>
		public string ExceptionType
		{
			get{return _ExceptionType;}
			set{_ExceptionType = value;}
		}

		/// <summary>
		/// 处理人
		/// </summary>
		public string Developer
		{
			get{return _Developer;}
			set{_Developer = value;}
		}

		/// <summary>
		/// 异常解释
		/// </summary>
		public string DeveloperMemo
		{
			get{return _DeveloperMemo;}
			set{_DeveloperMemo = value;}
		}
		/// <summary>
		/// 处理时间
		/// </summary>
		public string DealDateTime
		{
			get{return _DealDateTime;}
			set{_DealDateTime = value;}
		}

		/// <summary>
		/// 添加错误日志
		/// </summary>
		/// <returns></returns>
		public bool insertExceptionLog()
		{
			bool flag = false;
            OracleConnection cn = ConfigTool.GetConnection();
			try
			{
                string stSql = "INSERT INTO EA_Exception " +
					"(IPAddress, Message, AppID, StackTrace, TargetSite, PostURL, PostDateTime, UserID, UserName, Level, Memo, Status, ExceptionType)" +
					"VALUES " +
					"(@IPAddress, @Message, @AppID, @StackTrace, @TargetSite, @PostURL, @PostDateTime, @UserID, @UserName, @Level, @Memo, 1, 4)";

				OracleParameter[] commandParameters =
					{
						new OracleParameter("@IPAddress", IPAddress),
						new OracleParameter("@Message", Message),
						new OracleParameter("@AppID", int.Parse(AppID)),
						new OracleParameter("@StackTrace", StackTrace),
						new OracleParameter("@TargetSite", TargetSite),
						new OracleParameter("@PostURL", PostURL),
						new OracleParameter("@PostDateTime", Convert.ToDateTime(PostDateTime)),
						new OracleParameter("@UserID", long.Parse(UserID)),
						new OracleParameter("@UserName", UserName),
						new OracleParameter("@Level", int.Parse(Level)),
						new OracleParameter("@Memo", Remark)
					};

                
				int row = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, stSql, commandParameters);
				if (row > 0) flag = true;
			}
			catch(Exception ex)
			{
				Utils.Log2File(ex.Message);
			}
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

			return flag;
		}

		/// <summary>
		/// 得到列信息
		/// </summary>
		/// <returns></returns>
		public DataTable getEmptyException()
		{
			DataTable dt = new DataTable();
			string stSql = "select * from EA_V_Exception where 1=0";
            OracleConnection cn = ConfigTool.GetConnection();
			string s = cn.ConnectionString;
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

        ///// <summary>
        ///// 得到错误日志信息
        ///// </summary>
        ///// <param name="where">条件</param>
        ///// <param name="pagesize">页的大小</param>
        ///// <param name="flag">是否返回总数</param>
        ///// <param name="pageindex">当前的页码</param>
        ///// <param name="rowcount">记录的总数</param>
        ///// <returns></returns>
        //public DataTable getException(string where,int pagesize,bool flag,int pageindex,ref int rowcount)
        //{
        //    DataTable dt = new DataTable();
        //    OracleConnection cn = ConfigTool.GetConnection();
        //    dt = OracleDbHelper.ExecuteDataTable(cn,"V_Exception","*"," order by PostDateTime desc",pagesize,pageindex,flag,where,ref rowcount);
        //    return dt;
        //}

		/// <summary>
		/// 得到所有的应用系统信息
		/// </summary>
		/// <returns></returns>
		public DataTable getAppInfo()
		{
			DataTable dt = new DataTable();
            string stSql = "select * from Ts_SystemDef";
            OracleConnection cn = ConfigTool.GetConnection();
			string s = cn.ConnectionString;
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

		/// <summary>
		/// 得到单个错误日志信息
		/// </summary>
		/// <param name="id">日志信息ID</param>
		/// <returns></returns>
		public DataTable getOneException(string id)
		{
			DataTable dt = new DataTable();      
			string stSql = "SELECT * FROM EA_V_Exception where ExceptionID = "+ id;
            OracleConnection cn = ConfigTool.GetConnection();
			string s = cn.ConnectionString;
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}
		
		/// <summary>
		/// 开发人员维护错误日志
		/// </summary>
		/// <returns></returns>
		public bool UpdateException()
		{
			bool flag = false;
            string stSql = "UPDATE EA_Exception SET Status=@Status, ExceptionType=@ExceptionType, DeveloperMemo=@DeveloperMemo, Developer=@Developer, DealDateTime=sysdate WHERE ExceptionID=@ExceptionID";

			OracleParameter[] commandParameters =
				{
					new OracleParameter("@Status", int.Parse(Status)),
					new OracleParameter("@ExceptionType", int.Parse(ExceptionType)),
					new OracleParameter("@DeveloperMemo", DeveloperMemo),
					new OracleParameter("@Developer", Developer),
					new OracleParameter("@ExceptionID", long.Parse(ID))
				};

            OracleConnection cn = ConfigTool.GetConnection();
			string s = cn.ConnectionString;
			int row = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, stSql, commandParameters);
			if(row > 0) flag = true;

			return flag;
		}
	}
}
