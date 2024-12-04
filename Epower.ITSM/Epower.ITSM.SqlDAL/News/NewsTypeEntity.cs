using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// NewsTypeEntity 的摘要说明。
	/// </summary>
	public class NewsTypeEntity
	{
		public NewsTypeEntity()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		#region 属性
			private long _TypeId;
			private string _TypeName;
			e0A_IsInner _IsInner=e0A_IsInner.eTrue;
			e0A_IsOuter _IsOuter=e0A_IsOuter.eFalse;
			private string _Description;
		#endregion
		#region 属性定义
		public long TypeId
		{
			get{ return _TypeId;}
			set{_TypeId=value;}
		}
		public string TypeName
		{
			get{return _TypeName ;}
			set{_TypeName=value;}
		}
		public e0A_IsInner IsInner
		{
			get{return _IsInner;}
			set{_IsInner=value;}
		}
		public e0A_IsOuter IsOuter
		{
			get{return _IsOuter;}
			set{_IsOuter=value;}
		}

		public string Description
		{
			get{return _Description ;}
			set{_Description=value;}
		}

		#endregion

	#region 方法
		public NewsTypeEntity(int TypeId)
		{
			string strSQL = "SELECT * FROM OA_News_Type WHERE TypeId =" + TypeId;
			OracleConnection cn = ConfigTool.GetConnection();
			try
			{
				OracleDataReader dr = OracleDbHelper.ExecuteReader(cn,CommandType.Text,strSQL);

				if(dr.Read())
				{
					this.TypeId = (long)dr.GetDecimal(dr.GetOrdinal("TypeId"));
					this.TypeName = dr.IsDBNull(dr.GetOrdinal("TypeName")) == true? "" :dr.GetString(dr.GetOrdinal("TypeName"));
					this.IsInner =  dr.IsDBNull(dr.GetOrdinal("IsInner")) == true? e0A_IsInner.eTrue :(e0A_IsInner)dr.GetInt32(dr.GetOrdinal("IsInner"));
					this.IsOuter = dr.IsDBNull(dr.GetOrdinal("IsOuter")) == true? e0A_IsOuter.eFalse:(e0A_IsOuter)dr.GetInt32(dr.GetOrdinal("IsOuter"));
				}
				dr.Close();
			}
			catch(Exception err)
			{
				throw new Exception(err.Message);
			}
			finally
			{
				
				ConfigTool.CloseConnection(cn);
			}
		}

		public void Insert()
		{
			//long lngNewsType_ID=0;
			OracleConnection cn = ConfigTool.GetConnection();
			if(cn.State == ConnectionState.Closed )
			{
				cn.Open();
			}
           
			OracleTransaction trans = cn.BeginTransaction();
			try
			{
				//lngNewsType_ID=EpowerGlobal.EPGlobal.GetNextID(trans,"NEWSTYPE_ID");
				TypeId=EpowerGlobal.EPGlobal.GetNextID("NEWSTYPE_ID");
				string strSQL = "INSERT INTO OA_News_Type (TypeId,TypeName,IsInner,IsOuter,IndexID,Description)"+
					//" Values(" +lngNewsType_ID.ToString()+","+
					" Values(" +this.TypeId.ToString()+","+
					StringTool.SqlQ(this.TypeName)+","+
					(int)this.IsInner+","+
					(int)this.IsOuter+","+
					this.TypeId.ToString()+","+
					StringTool.SqlQ(this.Description) +					
					")";

				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);
				trans.Commit();
			}
			catch
			{
				trans.Rollback();
				throw new Exception();
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
			
		}


		public void Update()
		{
			OracleConnection cn = ConfigTool.GetConnection();
			if(cn.State == ConnectionState.Closed )
			{
				cn.Open();
			}
           
			OracleTransaction trans = cn.BeginTransaction();
			try
			{
				string strSQL = "Update OA_News_Type "+
								"Set TypeName="+StringTool.SqlQ(this.TypeName)+" , " +
									"IsInner="+(int)this.IsInner+" , " +
									"IsOuter="+(int)this.IsOuter + "," +
					                "Description =" + StringTool.SqlQ(this.Description) +	
								" Where TypeId="+this.TypeId.ToString() ;

				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);
				trans.Commit();
			}
			catch
			{
				trans.Rollback();
				throw new Exception();
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
			
		}


		public void Delete()
		{
			OracleConnection cn = ConfigTool.GetConnection();
			if(cn.State == ConnectionState.Closed )
			{
				cn.Open();
			}

			OracleTransaction trans = cn.BeginTransaction();
			try
			{
				string strSQL = "DELETE  FROM OA_News_Type where TypeId = " + this.TypeId;
				OracleDbHelper.ExecuteNonQuery(trans,CommandType.Text,strSQL);
				trans.Commit();
			}
			catch
			{
				trans.Rollback();
				throw new Exception();
			}
			finally
			{
				ConfigTool.CloseConnection(cn);
			}
			
		}

		public void Save()
		{
			//更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
			if(this.TypeId != 0)
			{
				Update();
			}
			else
			{
				Insert();
			}
		}

		#endregion

	}
}
