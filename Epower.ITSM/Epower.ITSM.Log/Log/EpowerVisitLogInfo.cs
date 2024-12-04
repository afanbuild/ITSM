using System;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Web;

using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Log
{
	/// <summary>
	///������־��������
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

		//���������ص���Ϣ
		string _Browser = string.Empty; //������汾
		string _Platform = string.Empty; //����ϵͳ
		string _Color = string.Empty; //��ɫ
		string _Resolution = string.Empty; //�ֱ���
		string _Language = string.Empty; //����
		string _UserAgent = string.Empty; //��������ص�UserAgent��Ϣ
		string _Referrer = string.Empty; //��·

        public EpowerVisitLogInfo()
		{
			
		}

		/// <summary>
		/// ������ID��
		/// </summary>
		public string ID
		{
			get{return _VisitID;}
			set{_VisitID = value;}
		}

		/// <summary>
		/// �û���
		/// </summary>
		public string UserName
		{
			get{return _UserName;}
			set{_UserName = value;}
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
		///������IP��
		/// </summary>
		public string IPAddress
		{
			get{return _IPAddress;}
			set{_IPAddress = value;}
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
		/// ����ģ��
		/// </summary>
		public string VisitModule
		{
			get{return _VisitModule;}
			set{_VisitModule = value;}
		}

		/// <summary>
		/// ���ʵ�ҳ��URL
		/// </summary>
		public string VisitURL
		{
			get{return _VisitURL;}
			set{_VisitURL = value;}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string VisitDateTime
		{
			get{return _VisitDateTime;}
			set{_VisitDateTime = value;}
		}

		#region ���������ص���Ϣ
		/// <summary>
		/// ������汾
		/// </summary>
		public string Browser
		{
			get{return _Browser;}
			set{_Browser = value;}
		}

		/// <summary>
		/// ����ϵͳ
		/// </summary>
		public string Platform
		{
			get{return _Platform;}
			set{_Platform = value;}
		}

		/// <summary>
		/// ��ɫ
		/// </summary>
		public string Color
		{
			get{return _Color;}
			set{_Color = value;}
		}

		/// <summary>
		/// �ֱ���
		/// </summary>
		public string Resolution
		{
			get{return _Resolution;}
			set{_Resolution = value;}
		}

		/// <summary>
		/// ����
		/// </summary>
		public string Language
		{
			get{return _Language;}
			set{_Language = value;}
		}

		/// <summary>
		/// ��������ص�UserAgent��Ϣ
		/// </summary>
		public string UserAgent
		{
			get{return _UserAgent;}
			set{_UserAgent = value;}
		}

		/// <summary>
		/// ��·
		/// </summary>
		public string Referrer
		{
			get{return _Referrer;}
			set{_Referrer = value;}
		}
		#endregion

		/// <summary>
		/// ��ӷ�����־
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
		/// �õ���ṹ
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
		/// �õ�������־��Ϣ
		/// </summary>
		/// <param name="where">����</param>
		/// <param name="pagesize">ҳ�Ĵ�С</param>
		/// <param name="flag">�Ƿ񷵻�����</param>
		/// <param name="pageindex">��ǰ��ҳ��</param>
		/// <param name="rowcount">��¼������</param>
		/// <returns></returns>
        //public DataTable getVisit(string where,int pagesize,bool flag,int pageindex,ref int rowcount)
        //{
        //    DataTable dt = new DataTable();
        //    OracleConnection cn = ConfigTool.GetConnection();
        //    dt = OracleDbHelper.ExecuteDataTable(cn,"V_Visit","*"," order by VisitDateTime desc",pagesize,pageindex,flag,where,ref rowcount);
        //    return dt;
        //}

		/// <summary>
		/// �õ�����������־��Ϣ
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
		/// �õ�������Ϣ
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
