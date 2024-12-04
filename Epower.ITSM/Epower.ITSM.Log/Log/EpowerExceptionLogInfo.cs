using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	/// ������־��������
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
		/// ������־ID��
		/// </summary>
		public string ID
		{
			get{return _ExceptionID;}
			set{_ExceptionID = value;}
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
		/// �쳣����
		/// </summary>
		public string Message
		{
			get{return _Message;}
			set{_Message = value;}
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
		/// ��ϸ�쳣��Ϣ
		/// </summary>
		public string StackTrace
		{
			get{return _StackTrace;}
			set{_StackTrace = value;}
		}

		/// <summary>
		/// �쳣�ķ���
		/// </summary>
		public string TargetSite
		{
			get{return _TargetSite;}
			set{_TargetSite = value;}
		}

		/// <summary>
		/// �ύURL
		/// </summary>
		public string PostURL
		{
			get{return _PostURL;}
			set{_PostURL = value;}
		}
		
		/// <summary>
		/// �ύʱ��
		/// </summary>
		public string PostDateTime
		{
			get{return _PostDateTime;}
			set{_PostDateTime = value;}
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
		/// ���ȼ�
		/// </summary>
		public string Level
		{
			get{return _Level;}
			set{_Level = value;}
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
		/// ����״̬
		/// </summary>
		public string Status
		{
			get{return _Status;}
			set{_Status = value;}
		}

		/// <summary>
		/// �쳣���
		/// </summary>
		public string ExceptionType
		{
			get{return _ExceptionType;}
			set{_ExceptionType = value;}
		}

		/// <summary>
		/// ������
		/// </summary>
		public string Developer
		{
			get{return _Developer;}
			set{_Developer = value;}
		}

		/// <summary>
		/// �쳣����
		/// </summary>
		public string DeveloperMemo
		{
			get{return _DeveloperMemo;}
			set{_DeveloperMemo = value;}
		}
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string DealDateTime
		{
			get{return _DealDateTime;}
			set{_DealDateTime = value;}
		}

		/// <summary>
		/// ��Ӵ�����־
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
		/// �õ�����Ϣ
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
        ///// �õ�������־��Ϣ
        ///// </summary>
        ///// <param name="where">����</param>
        ///// <param name="pagesize">ҳ�Ĵ�С</param>
        ///// <param name="flag">�Ƿ񷵻�����</param>
        ///// <param name="pageindex">��ǰ��ҳ��</param>
        ///// <param name="rowcount">��¼������</param>
        ///// <returns></returns>
        //public DataTable getException(string where,int pagesize,bool flag,int pageindex,ref int rowcount)
        //{
        //    DataTable dt = new DataTable();
        //    OracleConnection cn = ConfigTool.GetConnection();
        //    dt = OracleDbHelper.ExecuteDataTable(cn,"V_Exception","*"," order by PostDateTime desc",pagesize,pageindex,flag,where,ref rowcount);
        //    return dt;
        //}

		/// <summary>
		/// �õ����е�Ӧ��ϵͳ��Ϣ
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
		/// �õ�����������־��Ϣ
		/// </summary>
		/// <param name="id">��־��ϢID</param>
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
		/// ������Աά��������־
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
