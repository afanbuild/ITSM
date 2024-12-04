using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    public class Br_SubjectDP
    {
        /// <summary>
        /// 获取所有资产

        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubjects()
        {
            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brcategory");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0  ";

                dtTemp.Rows.Clear();
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {

                string strSQL = "SELECT * FROM Br_Category WHERE 1=1" +
                    " and deleted = " + (int)eO_Deleted.eNormal;

                OracleConnection cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubject()
        {
            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("brcategory");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0";

                dtTemp.Rows.Clear();
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {

                string strSQL = "SELECT * FROM Br_Category WHERE 1=1" +
                    " and deleted = " + (int)eO_Deleted.eNormal;

                OracleConnection cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }

        /// <summary>
        /// 获取资产分类的配置模型

        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetCatalogSchema(long lngSubjectID)
        {
            string strSQL;
            string strSchema = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT ConfigureSchema FROM Br_Category WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strSchema = dr["ConfigureSchema"].ToString().Trim();
                }
                dr.Close();                
            }
            finally { ConfigTool.CloseConnection(cn); }

            return strSchema;


        }

        /// <summary>
        /// 保存资产资料
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <param name="sSubjectName"></param>
        /// <param name="lngParentID"></param>
        /// <param name="iSortID"></param>
        /// <param name="sRemark"></param>
        /// <param name="sOrgID"></param>
        /// <param name="inherit"></param>
        /// <param name="sSchema"></param>
        /// <param name="ImageUrl"></param>
        /// <returns></returns>
        public static string Save(long lngSubjectID, string sSchema)
        {
            //更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
            string strSQL = string.Empty;
            string strSubjectID = "0";
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            long lngNextID = 0;
            try
            {
                if (lngSubjectID != 0)
                {
                    strSubjectID = lngSubjectID.ToString();
                    strSQL = "UPDATE Br_Category SET " +
                        " ConfigureSchema = " + StringTool.SqlQ(sSchema) +
                        " WHERE CatalogID = " + lngSubjectID.ToString();
                }
                else
                {
                    lngNextID = EpowerGlobal.EPGlobal.GetNextID("Br_CategoryID");
                    strSubjectID = lngNextID.ToString();
                    strSQL = "INSERT INTO Br_Category (CatalogID,ConfigureSchema,Deleted)" +
                        " Values(" +
                        lngNextID.ToString() + "," +
                        StringTool.SqlQ(sSchema) + "," +
                        ((int)eO_Deleted.eNormal).ToString() +
                        ")";
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                trans.Commit();
                return strSubjectID;
            }
            catch
            {
                trans.Rollback();
                return strSubjectID;
            }
            finally
            {
                trans.Dispose();
                ConfigTool.CloseConnection(cn);

            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="lngSubjectID"></param>
        public static void Delete(long lngSubjectID)
        {
            string strSQL;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL = "DELETE Br_Category  WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, strSQL);
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 获取分类下的[所有]下属分类的表集合,包括自己
        /// </summary>
        /// <param name="lngCataID"></param>
        /// <returns></returns>
        public static DataTable GetBelowCatas(decimal lngCataID)
        {
            string strSQL;
            DataTable dt;
            //2008-05-01 添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {
                dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID = " + lngCataID.ToString());
                    DataTable dtTemp = dt.Clone();
                    dtTemp.Rows.Clear();

                    foreach (DataRow dvr in drs)
                    {
                        dtTemp.Rows.Add(dvr);
                    }
                    dt = dtTemp;
                }
            }
            else
            {
                OracleConnection cn = ConfigTool.GetConnection();
                strSQL = "SELECT fullid FROM Br_Category WHERE catalogid = " + lngCataID.ToString();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

    }
}
