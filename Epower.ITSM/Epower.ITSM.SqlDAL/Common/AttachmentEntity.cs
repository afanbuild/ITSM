/*******************************************************************
 *
 * Description:������
 * 
 * 
 * Create By  :
 * Create Date:2008��7��30��
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// AttachmentEntity ��ժҪ˵����
	/// </summary>
	public class AttachmentEntity
	{

		#region ����
		long _FileID;
		long _FlowID;
		string _FileName;
		string _SufName;
		long _OriginID;
		int _Status;
		long _UpUserID;
		DateTime _UpDateTime;

		/// <summary>
		/// �ļ���ʶ
		/// </summary>
		public long FileID
		{
			get{return _FileID;			}
			set{_FileID=value;			}
		}

		/// <summary>
		/// ����ID
		/// </summary>
		public long FlowID
		{
			get{return _FlowID;			}
			set{_FlowID=value;			}
		}

		/// <summary>
		/// �ļ�����
		/// </summary>
		public string FileName
		{
			get{return _FileName;			}
			set{_FileName=value;			}
		}

		/// <summary>
		/// �ļ�����
		/// </summary>
		public string SufName
		{
			get{return _SufName;			}
			set{_SufName=value;			}
		}

		/// <summary>
		/// //
		/// </summary>
		public long OriginID
		{
			get{return _OriginID;			}
			set{_OriginID=value;			}
		}

		/// <summary>
		/// ״̬
		/// </summary>
		public int Status
		{
			get{return _Status;			}
			set{_Status=value;			}
		}

		/// <summary>
		/// �����û�
		/// </summary>
		public long UpUserID
		{
			get{return _UpUserID;			}
			set{_UpUserID=value;			}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime UpTime
		{
			get{return _UpDateTime;			}
			//set{_UpDateTime=value;			}
		}

		#endregion
		
		#region ���캯��
		/// <summary>
		/// ���츽������ʵ��
		/// </summary>
		public AttachmentEntity()
		{
			
		}

		/// <summary>
		/// ͨ���ļ�ID���츽������ʵ��
		/// </summary>
		/// <param name="lngFileID"></param>
		public AttachmentEntity(long lngFileID)
		{
			string sConn=ConfigTool.GetConnectString();
			string sSql="SELECT * FROM Es_Attachment WHERE FileID="+lngFileID.ToString();
            OracleDataReader dr = null;
            try
            {
                dr = OracleDbHelper.ExecuteReader(sConn, CommandType.Text, sSql);
                if (dr.Read())
                {
                    int n = 0;
                    n = dr.GetOrdinal("FileID");
                    this.FileID = (long)dr.GetDecimal(n);

                    n = dr.GetOrdinal("FileName");
                    this.FileName = dr.IsDBNull(n) == true ? "" : dr.GetString(n);

                    n = dr.GetOrdinal("SufName");
                    this.SufName = dr.IsDBNull(n) == true ? "" : dr.GetString(n);

                    n = dr.GetOrdinal("FlowID");
                    this.FlowID = dr.IsDBNull(n) == true ? 0 : (long)dr.GetDecimal(n);

                    n = dr.GetOrdinal("OriginID");
                    this.OriginID = dr.IsDBNull(n) == true ? 0 : (long)dr.GetDecimal(n);

                    n = dr.GetOrdinal("UpUserID");
                    this.UpUserID = dr.IsDBNull(n) == true ? 0 : (long)dr.GetDecimal(n);

                    n = dr.GetOrdinal("Status");
                    this.Status = dr.IsDBNull(n) == true ? 0 : (int)dr.GetInt32(n);

                    n = dr.GetOrdinal("UpTime");
                    this._UpDateTime = dr.IsDBNull(n) == true ? DateTime.Now : (DateTime)dr.GetDateTime(n);
                }                
            }
            finally {
                if (dr != null) dr.Close(); ;
            }
		}
		#endregion

		#region Private[Update,Add]

		private void Update()
		{
			string sSql="UPDATE Es_Attachment SET FlowID="+this.FlowID.ToString()+
						",FileName="+StringTool.SqlQ(this.FileName)+
						",SufName="+StringTool.SqlQ(this.SufName)+
						",OriginID="+this.OriginID.ToString()+
						",Status="+this.Status.ToString()+
						",UpTime="+StringTool.ToMyDateFormat(DateTime.Now)+
						",UpUserID="+this.UpUserID.ToString()+
						" WHERE FileID="+this.FileID.ToString();
			OracleConnection conn=ConfigTool.GetConnection();
			try
			{
				OracleDbHelper.ExecuteNonQuery(conn,CommandType.Text,sSql);
			}
			catch(OracleException  sqle)
			{
				throw new EpowerException("������Ա����ʱ���ִ���","ActorCondEntity.cs/[private void Update]",sqle.Message.ToString());
			}
			catch(Exception ex)
			{
				throw new EpowerException("������Ա����ʱ���ִ���","ActorCondEntity.cs/[private void Update]",ex.Message.ToString());
			}
			finally
			{
				ConfigTool.CloseConnection(conn);
			}
		}

		private void Add()
		{

		}
		#endregion
		
	}
}
