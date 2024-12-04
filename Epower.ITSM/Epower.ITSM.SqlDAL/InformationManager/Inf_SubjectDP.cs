/****************************************************************************
 * 
 * description:知识库维护数据层操作
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-09-22
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Inf_SubjectDP
    {


        /// <summary>
        /// 获取分类下的[所有]下属分类的表集合
        /// </summary>
        /// <param CatalogName="lngCatalogID"></param>
        /// <returns></returns>
        public static DataTable GetBelowCatalogs(decimal lngCatalogID)
        {
            string strSQL;

            DataTable dt;
            string strFullID = "-1";   //表示未找到   
            long lngFLength = 0;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT fullid FROM Inf_Category WHERE CatalogID = " + lngCatalogID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                if (dr.Read())
                {
                    strFullID = dr.GetString(0).Trim();
                    lngFLength = strFullID.Length;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            if (strFullID != "-1")
            {
                if (strFullID == "")
                {
                    strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM Inf_Category WHERE   " +
                        " and deleted = " + (int)eO_Deleted.eNormal +
                        " ORDER BY layer,sortid ";
                }
                else
                {
                    strSQL = "SELECT CatalogID,parentid,CatalogName,orgid,remark FROM Inf_Category WHERE  fullid <> " + StringTool.SqlQ(strFullID) +
                        " AND SUBSTR(fullid,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
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


        /// <summary>
        /// 获取分类下的[所有]下属分类的表集合,包括自己
        /// </summary>
        /// <param name="lngCataID"></param>
        /// <returns></returns>
        public static DataTable GetBelowCatas(decimal lngCataID, bool InformationLimit)
        {
            string strSQL;

            DataTable dt;
            string strFullID = "-1";   //表示未找到   
            long lngFLength = 0;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT fullid FROM Inf_Category WHERE catalogid = " + lngCataID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                if (dr.Read())
                {
                    strFullID = dr["fullid"].ToString().Trim();
                    lngFLength = strFullID.Length;
                }
                dr.Close();
            }
            catch (Exception ex) { ConfigTool.CloseConnection(cn); throw ex; }

            if (strFullID != "-1")
            {
                if (strFullID == "")
                {
                    strSQL = "SELECT catalogid,Parentid,CatalogName FROM Inf_Category WHERE   " +
                        " deleted = 0 ";
                }
                else
                {
                    strSQL = "SELECT catalogid,Parentid,CatalogName FROM Inf_Category WHERE   " +
                        "  SUBSTR(fullid,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
                        " AND deleted = " + (int)eO_Deleted.eNormal;

                }
                //知识分级控制
                if (InformationLimit && CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
                {
                    if (System.Web.HttpContext.Current.Session["InformationLimitList"] != null)
                    {
                        strSQL = strSQL + " and case catalogid when 1 then N'1' else SUBSTR(FullID,3,4) end in (" + System.Web.HttpContext.Current.Session["InformationLimitList"].ToString() + ",1" + ")";
                    }
                }
                strSQL += " ORDER BY SortID";

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


        /// <summary>
        /// 获取所有根分类(parentid=-1)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRootSubject()
        {
            string strSQL = "SELECT * FROM Inf_Category WHERE ParentID=-1" +
                " and deleted = " + (int)eO_Deleted.eNormal +
                " ORDER BY sortid";

            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取知识库的FULLID
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetSubjectFullID(long lngSubjectID)
        {
            string strSQL;
            string strFullID = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT fullid FROM Inf_Category WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strFullID = dr["fullid"].ToString().Trim();
                }
                dr.Close();

                return strFullID;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 获取所有知识库类别
        /// </summary>
        /// <param name="InformationLimit"></param>
        /// <returns></returns>
        public static DataTable GetSubjects(bool InformationLimit)
        {
            string strSQL = "SELECT * FROM Inf_Category WHERE 1=1" +
                " and deleted = " + (int)eO_Deleted.eNormal;
            //知识分级控制
            if (InformationLimit && CommonDP.GetConfigValue("Other", "InformationLimit") != null && CommonDP.GetConfigValue("Other", "InformationLimit") == "1")
            {
                if (System.Web.HttpContext.Current.Session["InformationLimitList"] != null)
                {
                    strSQL = strSQL + " and case catalogid when 1 then N'1' else substr(FullID,3,4) end in (" + System.Web.HttpContext.Current.Session["InformationLimitList"].ToString() + ")";
                }
            }
            strSQL += " ORDER BY SortID";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 根据知识库ID获取知识库资料
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubjectByID(long lngSubjectID)
        {
            string strSQL = "SELECT * FROM Inf_Category WHERE 1=1" +
                " and deleted = " + (int)eO_Deleted.eNormal + " and CatalogID=" + lngSubjectID.ToString() +
                " ORDER BY sortid";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取知识库的名称
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetSubjectName(long lngSubjectID)
        {
            string strSQL;
            string strSubjectName = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT CatalogName FROM Inf_Category WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strSubjectName = dr["CatalogName"].ToString().Trim();
                }
                dr.Close();

                return strSubjectName;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// 保存知识库资料
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <param name="sSubjectName"></param>
        /// <param name="lngParentID"></param>
        /// <param name="iSortID"></param>
        /// <param name="sRemark"></param>
        /// <param name="sOrgID"></param>
        /// <returns></returns>
        public static string Save(long lngSubjectID, string sSubjectName, long lngParentID, int iSortID, string sRemark, string sOrgID)
        {
            //更新或新增（ＩＤ　＝　０，ＮＡＭＥ　不等于　空的时候新增）
            string strSQL = string.Empty;
            string strSubjectID = "0";
            OracleConnection cn = ConfigTool.GetConnection();
            long lngNextID = 0;
            string strParentFullID = GetSubjectFullID(lngParentID);
            string strFullID = string.Empty;

            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                if (lngSubjectID != 0)
                {
                    strSubjectID = lngSubjectID.ToString();
                    strFullID = strParentFullID + lngSubjectID.ToString().PadLeft(6, char.Parse("0"));
                    strSQL = "UPDATE Inf_Category SET " +
                        " CatalogName =" + StringTool.SqlQ(sSubjectName) + "," +
                        " OrgID =" + sOrgID + "," +
                        " FullID = " + StringTool.SqlQ(strFullID) + "," +
                        " parentid = " + lngParentID.ToString() + "," +
                        " sortid = " + iSortID.ToString() + "," +
                        " Remark =" + StringTool.SqlQ(sRemark) + //"," +
                        " WHERE CatalogID = " + lngSubjectID.ToString();

                    
                    /*     
                     * Date: 2013-08-08 11:33
                     * summary: 修改知识类别时, 同步更新知识库中的知识类别名 和 FullID.
                     * modified: sunshaozong@gmail.com     
                     */

                    String strSqlUpdateInformation = String.Format("UPDATE Inf_Information SET TYPENAME = {0}, fullID = {2} WHERE TYPE = {1}",
                        StringTool.SqlQ(sSubjectName), lngSubjectID, StringTool.SqlQ(strFullID));
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSqlUpdateInformation);

                    /*     
                     * Date: 2013-08-08 10:22
                     * summary: 修改知识类别时, 同步更新知识单中的知识类别名.
                     * modified: sunshaozong@gmail.com     
                     */

                    String strSqlUpdateKMBase = String.Format("UPDATE INF_KMBASE SET TYPENAME = {0} WHERE TYPE = {1}",
                        StringTool.SqlQ(sSubjectName), lngSubjectID);
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSqlUpdateKMBase);
                }
                else
                {
                    lngNextID = EpowerGlobal.EPGlobal.GetNextID("Inf_CategoryID");
                    strSubjectID = lngNextID.ToString();
                    strFullID = strParentFullID + lngNextID.ToString().PadLeft(6, char.Parse("0"));
                    strSQL = "INSERT INTO Inf_Category (CatalogID,fullid,OrgID,parentid,CatalogName,sortid,Remark,Deleted)" +
                        " Values(" +
                        lngNextID.ToString() + "," +
                        StringTool.SqlQ(strFullID) + "," +
                        sOrgID.ToString() + "," +
                        lngParentID.ToString() + "," +
                        StringTool.SqlQ(sSubjectName) + "," +
                        iSortID.ToString() + "," +
                        StringTool.SqlQ(sRemark) + "," +
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
                ConfigTool.CloseConnection(cn);

            }
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="lngSubjectID"></param>
        public static void Delete(long lngSubjectID)
        {
            if (lngSubjectID == 1)
                return;

            string strSQL;

            //1、判断是否知识库内有人员的资料,如果有则不能删除
            //２、如果可以删除，则同时删除用户组组成表中与此知识库相关的信息和权限表中与此知识库相关的信息。
            OracleConnection cn = ConfigTool.GetConnection();
            cn.Open();

            //如果知识库有下属知识库，则不允许删除
            strSQL = "SELECT COUNT(CatalogID) FROM Inf_Category WHERE ParentID =" + lngSubjectID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;
            int count = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));
            if (count > 0)
            {
                throw new Exception("知识库内包含下属知识库类别，暂时不能删除，请先删除下属知识库类别！");
            }
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                strSQL = "UPDATE Inf_Category SET deleted = " + (int)eO_Deleted.eDeleted +
                    " WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }


        }
    }
}
