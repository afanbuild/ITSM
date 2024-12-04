using System.IO;
using System.Web;

using System;
using System.Data;
using System.Data.OracleClient;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// ������־������
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
		/// ������־ID��
		/// </summary>
		public string ID
		{
			get{return _OperateID;}
			set{_OperateID = value;}
		}

		/// <summary>
		/// Ӧ��ID��
		/// </summary>
		public string AppID
		{
			get{return _AppID;}
			set{_AppID = value;}
		}

		/// <summary>
		/// ������ID��
		/// </summary>
		public string UserID
		{
			get{return _UserID;}
			set{_UserID = value;}
		}

		/// <summary>
		/// ����������
		/// </summary>
		public string UserName
		{
			get{return _UserName;}
			set{_UserName = value;}
		}

		/// <summary>
		/// �����߲���ID��
		/// </summary>
		public string DeptID
		{
			get{return _DeptID;}
			set{_DeptID = value;}
		}

		/// <summary>
		/// �����߲�������
		/// </summary>
		public string Dept
		{
			get{return _Dept;}
			set{_Dept = value;}
		}

		/// <summary>
		/// ������IP��
		/// </summary>
		public string IPAddress
		{
			get{return _IPAddress;}
			set{_IPAddress = value;}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string Time
		{
			get{return _OPTime;}
			set{_OPTime = value;}
		}

		/// <summary>
		/// ����ҳ��
		/// </summary>
		public string Page
		{
			get{return _OPPage;}
			set{_OPPage = value;}
		}

		/// <summary>
		/// ��������
		/// </summary>
		public string Action
		{
			get{return _Action;}
			set{_Action = value;}
		}
		/// <summary>
		/// ������ʼʱ��
		/// </summary>
		public string BeginTime
		{
			get{return _OPBeginTime;}
			set{_OPBeginTime = value;}
		}

		/// <summary>
		/// ��������ʱ��
		/// </summary>
		public string EndTime
		{
			get{return _OPEndTime;}
			set{_OPEndTime = value;}
		}

		/// <summary>
		/// ��ע
		/// </summary>
		public string Remark
		{
			get{return _Memo;}
			set{_Memo = value;}
		}

        /// <summary>
        /// �������ݵ�ID
        /// </summary>
        public string SourceID
        {
            get { return _SourceID; }
            set { _SourceID = value; }
        }

        /// <summary>
        /// �������ݵı���
        /// </summary>
        public string SourceTable
        {
            get { return _SourceTable; }
            set { _SourceTable = value; }
        }

		/// <summary>
		/// ��Ӳ�����־
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
		/// �õ���ṹ
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
		/// �õ�������־��Ϣ
		/// </summary>
		/// <param name="where">����</param>
		/// <param name="pagesize">ҳ�Ĵ�С</param>
		/// <param name="flag">�Ƿ񷵻�����</param>
		/// <param name="pageindex">��ǰ��ҳ��</param>
		/// <param name="rowcount">��¼������</param>
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

        #region ClearLogData �����־
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
		/// �õ�����������־��Ϣ
		/// </summary>
		/// <param name="id">��־ID</param>
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
		/// �õ�������Ϣ
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

		//��¼��־���ļ�
		private void Log2File(string strLog)
		{
			string logPath = HttpRuntime.AppDomainAppPath + "Log";

			//ȷ���ļ��Ƿ���ڡ�
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
