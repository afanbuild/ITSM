using System.IO;
using System.Web;

using System;
using System.Data;
using System.Data.OracleClient;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// 操作日志处理类
	/// </summary>
	public class EpowerOperateLogInfo
	{
		string _OperateID = string.Empty;
		string _AppID = string.Empty;
		string _UserID = string.Empty;
		string _UserName = string.Empty;
		string _DeptID = string.Empty;
		string _Dept = string.Empty;
		string _IPAddress = string.Empty;
		string _OPTime = string.Empty;
		string _OPPage = string.Empty;
		string _Action = string.Empty;
		string _OPBeginTime = string.Empty;
		string _OPEndTime = string.Empty;
		string _Memo = string.Empty;
        string _SourceID = string.Empty;
        string _SourceTable = string.Empty;

        public EpowerOperateLogInfo()
		{
		}

		/// <summary>
		/// 操作日志ID号
		/// </summary>
		public string ID
		{
			get{return _OperateID;}
			set{_OperateID = value;}
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
		/// 操作者部门ID号
		/// </summary>
		public string DeptID
		{
			get{return _DeptID;}
			set{_DeptID = value;}
		}

		/// <summary>
		/// 操作者部门名称
		/// </summary>
		public string Dept
		{
			get{return _Dept;}
			set{_Dept = value;}
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
		/// 操作时间
		/// </summary>
		public string Time
		{
			get{return _OPTime;}
			set{_OPTime = value;}
		}

		/// <summary>
		/// 操作页面
		/// </summary>
		public string Page
		{
			get{return _OPPage;}
			set{_OPPage = value;}
		}

		/// <summary>
		/// 所做动作
		/// </summary>
		public string Action
		{
			get{return _Action;}
			set{_Action = value;}
		}
		/// <summary>
		/// 操作开始时间
		/// </summary>
		public string BeginTime
		{
			get{return _OPBeginTime;}
			set{_OPBeginTime = value;}
		}

		/// <summary>
		/// 操作结束时间
		/// </summary>
		public string EndTime
		{
			get{return _OPEndTime;}
			set{_OPEndTime = value;}
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
        /// 操作数据的ID
        /// </summary>
        public string SourceID
        {
            get { return _SourceID; }
            set { _SourceID = value; }
        }

        /// <summary>
        /// 操作数据的表名
        /// </summary>
        public string SourceTable
        {
            get { return _SourceTable; }
            set { _SourceTable = value; }
        }

		/// <summary>
		/// 添加操作日志
		/// </summary>
		/// <returns></returns>
		public bool insertOperateLog()
		{
			bool flag = false;
			try
			{
                string stSql = "insert into EA_Operate(AppID,SourceID,SourceTable,UserID,UserName,DeptID,Dept,IPAddress,OPTime,OPPage,Action,OPBeginTime,OPEndTime,Memo)values(" +
                    AppID +
                    "," + SourceID +
                    "," + StringTool.SqlQ(SourceTable) +
                    "," + UserID +
                    "," + StringTool.SqlQ(UserName) +
                    "," + DeptID +
                    "," + StringTool.SqlQ(Dept) +
                    "," + StringTool.SqlQ(IPAddress) +
                    ",to_date('" + Time + "','yyyy-MM-dd HH24:mi:ss')" +
                    "," + StringTool.SqlQ(Page) +
                    "," + Action +
                    "," + StringTool.SqlQ(BeginTime) +
                    "," + StringTool.SqlQ(EndTime) +
                    "," + StringTool.SqlQ(Remark) +
                    ")";
                OracleConnection cn = ConfigTool.GetConnection();
				int row = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, stSql);
				if (row>0)
				{
					flag = true;
				}
                ConfigTool.CloseConnection(cn);
			}
			catch(Exception ex)
			{Log2File(ex.Message);}
			return flag;
		}

		/// <summary>
		/// 得到表结构
		/// </summary>
		/// <returns></returns>
		public DataTable getEmptyOperate()
		{
			DataTable dt = new DataTable();
            string stSql = "select * from EA_Operate where 1=0";
            OracleConnection cn = ConfigTool.GetConnection();
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

		/// <summary>
		/// 得到操作日志信息
		/// </summary>
		/// <param name="where">条件</param>
		/// <param name="pagesize">页的大小</param>
		/// <param name="flag">是否返回总数</param>
		/// <param name="pageindex">当前的页码</param>
		/// <param name="rowcount">记录的总数</param>
		/// <returns></returns>
        //public DataTable getOperate(string where,int pagesize,bool flag,int pageindex,ref int rowcount)
        //{
        //    DataTable dt = new DataTable();
        //    OracleConnection cn = ConfigTool.GetConnection();
        //    dt = OracleDbHelper.ExecuteDataTable(cn,"Operate","*"," order by OPEndTime desc",pagesize,pageindex,flag,where,ref rowcount);
        //    return dt;
        //}

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder,int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_EA_Operate", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string stSql = "select * from V_EA_Operate where " + sWhere + sOrder;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region ClearLogData 清除日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        public void ClearLogData(string sWhere)
        {
            string stSql = "delete EA_Operate where " + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, stSql);
            ConfigTool.CloseConnection(cn);
        }
        #endregion 

        /// <summary>
		/// 得到单条操作日志信息
		/// </summary>
		/// <param name="id">日志ID</param>
		/// <returns></returns>
		public DataTable getOneOperate(string id)
		{
			DataTable dt = new DataTable();
            string stSql = "SELECT EA_Operate.*,Ts_SystemDef.SysName FROM Ts_SystemDef INNER JOIN EA_Operate ON Ts_SystemDef.SysID = EA_Operate.AppID where EA_Operate.OperateID = " + id;
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
            string stSql = "select distinct DeptID,Dept from EA_Operate";
            OracleConnection cn = ConfigTool.GetConnection();
			dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, stSql);
			return dt;
		}

		//记录日志到文件
		private void Log2File(string strLog)
		{
			string logPath = HttpRuntime.AppDomainAppPath + "Log";

			//确字文件是否存在。
			if (!Directory.Exists(logPath))
			{
				DirectoryInfo d = Directory.CreateDirectory(logPath);
			}

			string LogFile = logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

			if (!File.Exists(LogFile))
			{
				StreamWriter rw = File.CreateText(LogFile);
				rw.Close();
			}

			StreamWriter sw = new StreamWriter(LogFile, true, System.Text.Encoding.Default);
			sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "	" + strLog + "\r\n");

			sw.Flush();
			sw.Close();
		}

	}
}
