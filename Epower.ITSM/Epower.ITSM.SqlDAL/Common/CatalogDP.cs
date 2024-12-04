/*******************************************************************
 *
 * Description:常用分类数据处理
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// CatalogDP 的摘要说明。
	/// </summary>
	public class CatalogDP
	{
		/// <summary>
		/// 
		/// </summary>
		public CatalogDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		/// <summary>
		/// 获取所有根分类(parentid=-1)
		/// </summary>
		/// <returns></returns>
		public static DataTable GetRootCatalogs()
		{
            DataTable dt;
               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND ParentID=-1 ";

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                dt.Dispose();
                return dtTemp;
            }
            else
            {
                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE ParentID=-1" +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

                
                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                return dt;
            }
		}

		/// <summary>
		/// 获取当前机构的所有根分类(parentid=-1)
		/// </summary>
		/// <param CatalogName="orgid">机构ID</param>
		/// <returns></returns>
		public static DataTable GetRootCatalogs(long orgid)
		{

            DataTable dt;
               //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND ParentID=-1 AND orgid=" + orgid.ToString();

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                dt.Dispose();
                return dtTemp;
            }
            else
            {
                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE ParentID=-1 AND orgid=" + orgid.ToString() +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";


                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                return dt;
            }
		}

        /// <summary>
        /// 获取当前机构的所有根分类(parentid=-1)
        /// </summary>
        /// <param CatalogName="orgid">机构ID</param>
        /// <returns></returns>
        public static string GetCatalogNamebyID(long catalogId,long parentId)
        {

            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0  AND catalogId=" + catalogId.ToString() + " and  catalogID <>" + parentId.ToString();

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                dt.Dispose();
                if (dtTemp.Rows.Count > 0)
                {
                    return dtTemp.Rows[0]["CatalogName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE   catalogId=" + catalogId.ToString() + "  and catalogID <>" + parentId.ToString() +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["CatalogName"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        ///根据名称获得id
        /// </summary>
        /// <param CatalogName="catalogName">类别名称</param>
        /// <param name="ParentID">父类ID</param>
        /// <returns></returns>
        public static string GetCatalogIDbyName(string catalogName, long ParentID)
        {

            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND ParentID=" + ParentID.ToString() + " AND CatalogName=" + StringTool.SqlQ(catalogName.Trim());

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                dt.Dispose();
                if (dtTemp.Rows.Count > 0)
                {
                    return dtTemp.Rows[0]["catalogID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE  ParentID=" + ParentID.ToString() + " AND  CatalogName=" + StringTool.SqlQ(catalogName.Trim()) +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["catalogID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }


        /// <summary>
        ///根据名称获得id
        /// </summary>
        /// <param CatalogName="orgid">机构ID</param>
        /// <returns></returns>
        public static string GetCatalogIDbyName(string  catalogName)
        {

            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0  AND CatalogName=" + StringTool.SqlQ(catalogName.Trim());

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                dt.Dispose();
                if (dtTemp.Rows.Count > 0)
                {
                    return dtTemp.Rows[0]["catalogID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE    CatalogName=" + StringTool.SqlQ(catalogName.Trim()) +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }

                

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["catalogID"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

		/// <summary>
		/// 获取分类的FULLID
		/// </summary>
		/// <param name="lngCatalogID"></param>
		/// <returns></returns>
		public static string GetCatalogFullID(long lngCatalogID)
		{
			string strSQL;

			string strFullID = "";     

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID=" + lngCatalogID.ToString());

                    if (drs.Length > 0)
                    {
                        strFullID = drs[0]["fullid"].ToString();
                    }
                    dt.Dispose();
                }
                
            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT fullid FROM es_Catalog WHERE CatalogID = " + lngCatalogID.ToString();
                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                    if (dr.Read())
                    {
                        strFullID = dr.GetString(0).Trim();
                    }
                    dr.Close();                    
                }
                finally { ConfigTool.CloseConnection(cn); }
            }
			return strFullID;
		}

		/// <summary>
		/// 获取分类下的[所有]下属分类的表集合
		/// </summary>
		/// <param CatalogName="lngCatalogID"></param>
		/// <returns></returns>
		public static DataTable GetBelowCatalogs(long lngCatalogID)
		{
			string strSQL;

			DataTable dt;
			string strFullID = "-1";   //表示未找到   
			int lngFLength = 0;


            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID=" + lngCatalogID.ToString());

                    if (drs.Length > 0)
                    {
                        strFullID = drs[0]["fullid"].ToString();
                        lngFLength = strFullID.Length;
                    }
                }


                

                if (strFullID != "-1")
                {
                    
                    dt.DefaultView.RowFilter = " deleted = 0 ";
                    DataTable dtTemp = dt.Clone();
                    dtTemp.Rows.Clear();
                   
                    if (strFullID == "")
                    {
                        dt.DefaultView.Sort = "fullid";
                        foreach (DataRowView dvr in dt.DefaultView)
                        {
                            dtTemp.Rows.Add(dvr.Row.ItemArray);
                        }
                    }
                    else
                    {
                        dt.DefaultView.Sort = "sortid";
                        foreach (DataRowView dvr in dt.DefaultView)
                        {
                            string sCurrFull = dvr.Row["fullid"].ToString();
                            if (sCurrFull.Length >= lngFLength)
                            {
                                if (sCurrFull.Substring(0, lngFLength) == strFullID & sCurrFull != strFullID)
                                {
                                    dtTemp.Rows.Add(dvr.Row.ItemArray);
                                }
                            }
                        }

                    }

                    
                    return dtTemp;
                }
                else
                {
                    
                    return null;
                }

                dt.Dispose();



            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT fullid FROM es_Catalog WHERE CatalogID = " + lngCatalogID.ToString();
                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                    if (dr.Read())
                    {
                        strFullID = dr.GetString(0).Trim();
                        lngFLength = strFullID.Length;
                    }
                    dr.Close();

                    if (strFullID != "-1")
                    {

                        if (strFullID == "")
                        {
                            strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE   " +
                                " and deleted = " + (int)eO_Deleted.eNormal +
                                " ORDER BY fullid ";
                        }
                        else
                        {
                            strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE  fullid <> " + StringTool.SqlQ(strFullID) +
                                " AND SUBSTRING(fullid,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
                                " AND deleted = " + (int)eO_Deleted.eNormal + " ORDER BY sortid ";

                        }

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                        ConfigTool.CloseConnection(cn);

                        return dt;
                    }
                    else
                    {
                        ConfigTool.CloseConnection(cn);
                        return null;
                    }
                }
                finally { ConfigTool.CloseConnection(cn); }

            }
			
			
			
		}


		/// <summary>
		/// 获取分类下的[所有]下属分类的表集合,包括自己
		/// </summary>
		/// <param name="lngCataID"></param>
		/// <returns></returns>
		public static DataTable GetBelowCatas(long lngCataID)
		{
			string strSQL;

			DataTable dt;
			string strFullID = "-1";   //表示未找到   
			int lngFLength = 0;


            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID=" + lngCataID.ToString());

                    if (drs.Length > 0)
                    {
                        strFullID = drs[0]["fullid"].ToString();
                        lngFLength = strFullID.Length;
                    }
                }

                //
                if (strFullID != "-1")
                {
                    DataTable dtTemp =dt.Clone();
                    dt.DefaultView.RowFilter = " deleted = 0 ";

                    dtTemp.Rows.Clear();
                    if (strFullID == "")
                    {
                        dt.DefaultView.Sort = "fullid";
                        foreach (DataRowView dvr in dt.DefaultView)
                        {
                            dtTemp.Rows.Add(dvr.Row.ItemArray);
                        }
                    }
                    else
                    {
                        dt.DefaultView.Sort = "sortid";
                        foreach (DataRowView dvr in dt.DefaultView)
                        {
                            string sCurrFull = dvr.Row["fullid"].ToString();
                            if (sCurrFull.Length >= lngFLength)
                            {
                                if (sCurrFull.Substring(0, lngFLength) == strFullID)
                                {
                                    dtTemp.Rows.Add(dvr.Row.ItemArray);
                                }
                            }
                        }

                    }


                    
                    return dtTemp;
                }
                else
                {

                    return null;
                }

                dt.Dispose();
            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT fullid FROM Es_CataLog WHERE catalogid = " + lngCataID.ToString();
                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                    if (dr.Read())
                    {
                        strFullID = dr.GetString(0).Trim();
                        lngFLength = strFullID.Length;
                    }
                    dr.Close();

                    if (strFullID != "-1")
                    {
                        if (strFullID == "")
                        {
                            strSQL = "SELECT catalogid,Parentid,CatalogName FROM Es_CataLog WHERE   " +
                                " deleted = 0 ORDER BY fullid";
                        }
                        else
                        {
                            strSQL = "SELECT catalogid,Parentid,CatalogName FROM Es_CataLog WHERE   " +
                                "  Substr(fullid,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
                                " AND deleted = " + (int)eO_Deleted.eNormal + " ORDER BY sortid ";

                        }

                        dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                        ConfigTool.CloseConnection(cn);

                        return dt;
                    }
                    else
                    {
                        ConfigTool.CloseConnection(cn);
                        return null;
                    }
                }
                finally { ConfigTool.CloseConnection(cn); }


            }


			
			
		}

		
		/// <summary>
		/// 获取所有分类
		/// </summary>
		/// <returns></returns>
		public static DataTable GetCatalogs()
		{
            DataTable dt;
              //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
               
                    DataTable dtTemp = dt.Clone();
                    dt.DefaultView.RowFilter = " deleted = 0 ";

                    dtTemp.Rows.Clear();
                   
                    dt.DefaultView.Sort = "sortid";
                    foreach (DataRowView dvr in dt.DefaultView)
                    {
                        dtTemp.Rows.Add(dvr.Row.ItemArray);
                       
                    }

                    dt.Dispose();
                    return dtTemp;
            }
            else
            {

                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE 1=1" +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

               
                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                return dt;
            }
		}


		/// <summary>
		/// 获取下一级分类(名称/ID)
		/// </summary>
		/// <returns></returns>
		public static DataTable GetNextCatalogs(long lngID)
		{
            DataTable dt;
              //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND parentID = " + lngID.ToString();

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);
                    
                }

                dt.Dispose();
                return dtTemp;

            }
            else
            {

                string strSQL = "SELECT CatalogID,CatalogName FROM es_Catalog WHERE parentID =" + lngID.ToString() +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";

               
                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }                

                return dt;
            }
		}

		/// <summary>
		/// 获取机构下的所有分类
		/// </summary>
		/// <param CatalogName="orgid">机构ID</param>
		/// <returns></returns>
		public static DataTable GetCatalogs(long orgid)
		{

            DataTable dt;

              //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND orgid = " + orgid.ToString();

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }

                dt.Dispose();
                return dtTemp;

            }
            else
            {

                string strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM es_Catalog WHERE 1=1 AND orgid=" + orgid.ToString() +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";


                OracleConnection cn = ConfigTool.GetConnection();

                try { dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL); }
                finally { ConfigTool.CloseConnection(cn); }
                

                return dt;
            }
		}

		/// <summary>
		/// 获取部门父分类的ＩＤ
		/// </summary>
		/// <param name="lngCatalogID"></param>
		/// <returns></returns>
		public static long GetCatalogParentID(long lngCatalogID)
		{
			string strSQL="";
			OracleDataReader dr;
			long lngParentID = 1;

            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID=" + lngCatalogID.ToString() + " AND  deleted =0");

                    if (drs.Length > 0)
                    {
                        lngParentID = long.Parse(drs[0]["parentid"].ToString());
                    }
                    dt.Dispose();
                }
                
            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT parentid FROM Es_Catalog WHERE CatalogID = " + lngCatalogID.ToString() +
                        " And Deleted=" + ((int)eO_Deleted.eNormal).ToString();
                    dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    while (dr.Read())
                    {
                        lngParentID = (long)dr.GetDecimal(0);
                    }
                    dr.Close();
                }
                finally { ConfigTool.CloseConnection(cn); }                
            }
			return lngParentID;
		}
		/// <summary>
		/// 取分类名称
		/// </summary>
		/// <param name="lngCatalogID"></param>
		/// <returns></returns>
		public static string GetCatalogName(long lngCatalogID)
		{
			string strSQL="";
			OracleDataReader dr;
			string strName = "";
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = EpSqlCacheHelper.GetDataTableFromCache("catalog");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID=" + lngCatalogID.ToString());

                    if (drs.Length > 0)
                    {
                        strName = drs[0]["CatalogName"].ToString();
                    }
                    dt.Dispose();
                }
                
            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT CatalogName FROM Es_Catalog WHERE CatalogID = " + lngCatalogID.ToString() +
    " And Deleted=" + ((int)eO_Deleted.eNormal).ToString();
                    dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    while (dr.Read())
                    {
                        strName = dr.GetString(0);
                    }
                    dr.Close();

                }
                finally { ConfigTool.CloseConnection(cn); }
                
            }
			return strName;
		}


		/// <summary>
		/// 取分类名称(全称)
		/// </summary>
		/// <param name="lngCatalogID"></param>
		/// <param name="lngStartID">展示全称的起点 分类</param>
		/// <returns></returns>
		public static string GetFullCatalogName(long lngCatalogID,long lngStartID)
		{
			
			string strName = "";

            string strSQL = "";
            OracleDataReader dr;

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT GetFullCatalog(fullid,'-->'," + lngStartID.ToString() + " ) FROM Es_Catalog WHERE CatalogID = " + lngCatalogID.ToString() +
        " And Deleted=" + ((int)eO_Deleted.eNormal).ToString();
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strName = dr.GetString(0);
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }			
			
			return strName;
		}

		/// <summary>
		/// 取分类名称(全称)
		/// </summary>
		/// <param name="lngCatalogID"></param>
		/// <returns></returns>
		public static string GetFullCatalogName(long lngCatalogID)
		{
			
			//从根节点开始展示
			return GetFullCatalogName(lngCatalogID,1);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngCatalogID"></param>
        /// <returns></returns>
        public static string GetCatalogRemark(long lngCatalogID)
        {
            string strName = "";

            string strSQL = "";
            OracleDataReader dr;

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "select Remark from Es_Catalog WHERE CatalogID = " + lngCatalogID.ToString();
                dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    strName = dr.IsDBNull(0) == true ? "" : dr.GetString(0);
                }
                dr.Close();
            }
            finally { ConfigTool.CloseConnection(cn); }            

            return strName;

        }

        #region 获取某个分类的配置项信息
        /// <summary>
        /// 获取某个分类的配置项信息
        /// </summary>
        /// <param name="lngCatalogID"></param>
        /// <returns></returns>
        public static string GetCatalogSchema(long lngCatalogID)
        {
            string strSchema = "";
            string strSQL = "select configureSchema from es_catalog where catalogID = " + lngCatalogID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                strSchema = dt.Rows[0]["configureSchema"].ToString();
            }

            return strSchema;
        }
        #endregion

        #region 获取某RootID下的根分类列表
        /// <summary>
        /// 获取某RootID下的根分类列表
        /// </summary>
        /// <param name="strRootID"></param>
        /// <returns></returns>
        public static DataTable GetCatasByRootID(long strRootID)
        {
            string strSql = "select * from es_catalog where deleted=0 and ParentID =" + strRootID;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion
    }
}