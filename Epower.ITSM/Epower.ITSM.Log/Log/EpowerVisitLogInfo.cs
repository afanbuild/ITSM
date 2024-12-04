using System;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Web;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	///访问日志处理类类
	/// </summary>
	public class EpowerVisitLogInfo
	{
		string _VisitID = string.Empty;
		string _AppID = string.Empty;
		string _IPAddress = string.Empty;
		string _UserID = string.Empty;
		string _DeptID = string.Empty;
		string _Dept = string.Empty;
		string _VisitModule = string.Empty;
		string _VisitURL = string.Empty;
		string _VisitDateTime = string.Empty;
		string _UserName = string.Empty;

		//与浏览器相关的信息
		string _Browser = string.Empty; //浏览器版本
		string _Platform = string.Empty; //操作系统
		string _Color = string.Empty; //颜色
		string _Resolution = string.Empty; //分辨率
		string _Language = string.Empty; //语言
		string _UserAgent = string.Empty; //浏览器返回的UserAgent信息
		string _Referrer = string.Empty; //来路

        public EpowerVisitLogInfo()
		{
			
		}

		/// <summary>
		/// 访问日ID号
		/// </summary>
		public string ID
		{
			get{return _VisitID;}
			set{_VisitID = value;}
		}

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName
		{
			get{return _UserName;}
			set{_UserName = value;}
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
		///访问者IP号
		/// </summary>
		public string IPAddress
		{
			get{return _IPAddress;}
			set{_IPAddress = value;}
		}

		/// <summary>
		/// 访问者ID号
		/// </summary>
		public string UserID
		{
			get{return _UserID;}
			set{_UserID = value;}
		}

		/// <summary>
		/// 访问者部门ID号
		/// </summary>
		public string DeptID
		{
			get{return _DeptID;}
			set{_DeptID = value;}
		}

		/// <summary>
		/// 访问者部门名称
		/// </summary>
		public string Dept
		{
			get{return _Dept;}
			set{_Dept = value;}
		}

		/// <summary>
		/// 访问模块
		/// </summary>
		public string VisitModule
		{
			get{return _VisitModule;}
			set{_VisitModule = value;}
		}

		/// <summary>
		/// 访问的页面URL
		/// </summary>
		public string VisitURL
		{
			get{return _VisitURL;}
			set{_VisitURL = value;}
		}

		/// <summary>
		/// 访问时间
		/// </summary>
		public string VisitDateTime
		{
			get{return _VisitDateTime;}
			set{_VisitDateTime = value;}
		}

		#region 与浏览器相关的信息
		/// <summary>
		/// 浏览器版本
		/// </summary>
		public string Browser
		{
			get{return _Browser;}
			set{_Browser = value;}
		}

		/// <summary>
		/// 操作系统
		/// </summary>
		public string Platform
		{
			get{return _Platform;}
			set{_Platform = value;}
		}

		/// <summary>
		/// 颜色
		/// </summary>
		public string Color
		{
			get{return _Color;}
			set{_Color = value;}
		}

		/// <summary>
		/// 分辨率
		/// </summary>
		public string Resolution
		{
			get{return _Resolution;}
			set{_Resolution = value;}
		}

		/// <summary>
		/// 语言
		/// </summary>
		public string Language
		{
			get{return _Language;}
			set{_Language = value;}
		}

		/// <summary>
		/// 浏览器返回的UserAgent信息
		/// </summary>
		public string UserAgent
		{
			get{return _UserAgent;}
			set{_UserAgent = value;}
		}

		/// <summary>
		/// 来路
		/// </summary>
		public string Referrer
		{
			get{return _Referrer;}
			set{_Referrer = value;}
		}
		#endregion

		/// <summary>
		/// 添加访问日志
		/// </summary>
		/// <returns></returns>
		public bool insertVistiLog()
		{
			
			bool flag = false;
            OracleConnection cn = ConfigTool.GetConnection();
			try
			{
                string stSql = "INSERT INTO EA_Visit (AppID, IPAddress, UserID, UserName, DeptID, Dept, VisitModule, VisitURL, VisitDateTime, Browser, Platform, Color, Resolution, Language, UserAgent, Referrer)" +
					" VALUES (@AppID, @IPAddress, @UserID, @UserName, @DeptID, @Dept, @VisitModule, @VisitURL, @VisitDateTime, @Browser, @Platform, @Color, @Resolution, @Language, @UserAgent, @Referrer)";

				OracleParameter[] commandParameters =
					{
						new OracleParameter("@AppID", int.Parse(AppID)),
						new OracleParameter("@IPAddress", IPAddress),
						new OracleParameter("@UserID", long.Parse(UserID)),
						new OracleParameter("@UserName", UserName),
						new OracleParameter("@DeptID", DeptID),
						new OracleParameter("@Dept", Dept),
						new OracleParameter("@VisitModule", VisitModule),
						new OracleParameter("@VisitURL", VisitURL),
						new OracleParameter("@VisitDateTime", Convert.ToDateTime(VisitDateTime)),
						new OracleParameter("@Browser", Browser),
						new OracleParameter("@Platform", Platform),
						new OracleParameter("@Color", Color),
						new OracleParameter("@Resolution", Resolution),
						new OracleParameter("@Language", Language),
						new OracleParameter("@UserAgent", UserAgent),
						new OracleParameter("@Referrer", Referrer)
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
		/// 得到表结构
		/// </summary>
		/// <returns></returns>
		public DataTable getEmptyVisit()
		{
			DataTable dt = new DataTable();
			string stSql = "select * from EA_V_Visit where 1=0";
            OracleConnection cn = ConfigTool.GetConnection();
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

		/// <summary>
		/// 得到访问日志信息
		/// </summary>
		/// <param name="where">条件</param>
		/// <param name="pagesize">页的大小</param>
		/// <param name="flag">是否返回总数</param>
		/// <param name="pageindex">当前的页码</param>
		/// <param name="rowcount">记录的总数</param>
		/// <returns></returns>
        //public DataTable getVisit(string where,int pagesize,bool flag,int pageindex,ref int rowcount)
        //{
        //    DataTable dt = new DataTable();
        //    OracleConnection cn = ConfigTool.GetConnection();
        //    dt = OracleDbHelper.ExecuteDataTable(cn,"V_Visit","*"," order by VisitDateTime desc",pagesize,pageindex,flag,where,ref rowcount);
        //    return dt;
        //}

		/// <summary>
		/// 得到单条访问日志信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DataTable getOneVisit(string id)
		{
			DataTable dt = new DataTable();
            string stSql = "SELECT EA_Visit.*,Ts_SystemDef.SysName FROM Ts_SystemDef INNER JOIN Visit ON Ts_SystemDef.SysID = Ts_SystemDef.AppID where Ts_SystemDef.VisitID = " + id;
            OracleConnection cn = ConfigTool.GetConnection();
			string s = cn.ConnectionString;
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

		/// <summary>
		/// 得到部门信息
		/// </summary>
		/// <returns></returns>
		public DataTable GetDeptInfo()
		{
			DataTable dt = new DataTable();
            string stSql = "select distinct DeptID,Dept from EA_Visit";
            OracleConnection cn = ConfigTool.GetConnection();
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}
	}
}
