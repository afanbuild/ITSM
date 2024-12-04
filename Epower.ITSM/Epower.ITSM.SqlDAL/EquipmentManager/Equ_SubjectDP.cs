/****************************************************************************
 * 
 * description:�ʲ�ά�����ݲ����
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
    public class Equ_SubjectDP
    {
        /// <summary>
        /// ��ȡ���и�����(parentid=-1)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRootSubject()
        {
            DataTable dt;

            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 AND ParentID=-1 ";

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "sortid";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {

                string strSQL = "SELECT * FROM Equ_Category WHERE ParentID=-1" +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY sortid";


                OracleConnection cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }

        /// <summary>
        /// ��ȡ�ʲ���FULLID
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetSubjectFullID(long lngSubjectID)
        {
            string strSQL;
            string strFullID = "";

            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID = " + lngSubjectID.ToString());

                    if (drs.Length > 0)
                    {
                        strFullID = drs[0]["fullid"].ToString();
                    }
                }
            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT fullid FROM Equ_Category WHERE CatalogID = " + lngSubjectID.ToString();
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
        /// ��ȡ�����ʲ�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSubjects()
        {
            DataTable dt;
            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0  ";

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "SortID";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {

                string strSQL = "SELECT * FROM Equ_Category WHERE 1=1" +
                    " and deleted = " + (int)eO_Deleted.eNormal +
                    " ORDER BY SortID";

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
        public static DataTable GetSubjectByID(long lngSubjectID)
        {
            DataTable dt;
            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");

                DataTable dtTemp = dt.Clone();
                dt.DefaultView.RowFilter = " deleted = 0 and CatalogID=" + lngSubjectID.ToString();

                dtTemp.Rows.Clear();

                dt.DefaultView.Sort = "SortID";
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    dtTemp.Rows.Add(dvr.Row.ItemArray);

                }
                return dtTemp;
            }
            else
            {

                string strSQL = "SELECT * FROM Equ_Category WHERE 1=1" +
                    " and deleted = " + (int)eO_Deleted.eNormal + " and CatalogID=" + lngSubjectID.ToString() +
                    " ORDER BY sortid";

                OracleConnection cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                ConfigTool.CloseConnection(cn);
                return dt;
            }
        }


        /// <summary>
        /// ��ȡ�ʲ�����ͼƬ·��
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetSubjectImageUrl(long lngSubjectID)
        {
            string strSQL;
            string strImageUrl = "";

            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID = " + lngSubjectID.ToString());

                    if (drs.Length > 0)
                    {
                        strImageUrl = drs[0]["imageurl"].ToString();
                    }
                }
            }
            else
            {


                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT imageurl FROM Equ_Category WHERE CatalogID = " + lngSubjectID.ToString();
                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    if (dr.Read())
                    {
                        strImageUrl = dr.GetString(0).Trim();
                    }
                    dr.Close();                    
                }
                finally { ConfigTool.CloseConnection(cn); }
            }
            return strImageUrl;
        }


        /// <summary>
        /// ��ȡ�ʲ���������
        /// </summary>
        /// <param name="lngSubjectID"></param>
        /// <returns></returns>
        public static string GetSubjectName(long lngSubjectID)
        {
            string strSQL;
            string strSubjectName = "";

            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                DataTable dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID = " + lngSubjectID.ToString());

                    if (drs.Length > 0)
                    {
                        strSubjectName = drs[0]["CatalogName"].ToString();
                    }
                }
            }
            else
            {


                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT CatalogName FROM Equ_Category WHERE deleted = 0 and  CatalogID = " + lngSubjectID.ToString();
                    OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                    if (dr.Read())
                    {
                        strSubjectName = dr.GetString(0).Trim();
                    }
                    dr.Close();                    
                }
                finally { ConfigTool.CloseConnection(cn); }
            }
            return strSubjectName;
        }

        /// <summary>
        /// ��ȡ�ʲ����������ģ��
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
                strSQL = "SELECT GetCatalogSchma(fullid) as stSchemma FROM Equ_Category WHERE CatalogID = " + lngSubjectID.ToString();
                // strSQL = "SELECT configureschema FROM Equ_Category WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strSchema = dr["stSchemma"].ToString().Trim();
                }
                dr.Close();                
                return strSchema;
            }
            finally { ConfigTool.CloseConnection(cn); }


        }

        /// <summary>
        /// �����ʲ�����
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
        public static string Save(long lngSubjectID, string sSubjectName, long lngParentID, int iSortID, string sRemark, string sOrgID, int inherit, string sSchema, string ImageUrl)
        {
            //���»��������ɣġ����������Σ��ͣš������ڡ��յ�ʱ��������
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
                    if (lngSubjectID == 1)
                    {
                        //���������⴦��
                        strFullID = "";
                    }
                    else
                    {
                        strFullID = strParentFullID + lngSubjectID.ToString().PadLeft(6, char.Parse("0"));
                    }
                    strSQL = "UPDATE Equ_Category SET " +
                        " CatalogName =" + StringTool.SqlQ(sSubjectName) + "," +
                        " OrgID =" + sOrgID + "," +
                        " FullID = " + StringTool.SqlQ(strFullID) + "," +
                        " parentid = " + lngParentID.ToString() + "," +
                        " sortid = " + iSortID.ToString() + "," +
                        " InheritSchema =" + inherit + "," +
                        " ImageUrl = " + StringTool.SqlQ(ImageUrl) + "," +
                        " Remark =" + StringTool.SqlQ(sRemark) + //"," +
                        " WHERE CatalogID = " + lngSubjectID.ToString();
                }
                else
                {
                    lngNextID = EpowerGlobal.EPGlobal.GetNextID("Equ_CategoryID");
                    strSubjectID = lngNextID.ToString();
                    strFullID = strParentFullID + lngNextID.ToString().PadLeft(6, char.Parse("0"));
                    strSQL = "INSERT INTO Equ_Category (CatalogID,fullid,OrgID,parentid,CatalogName,sortid,InheritSchema,ImageUrl,Remark,Deleted)" +
                        " Values(" +
                        lngNextID.ToString() + "," +
                        StringTool.SqlQ(strFullID) + "," +
                        sOrgID.ToString() + "," +
                        lngParentID.ToString() + "," +
                        StringTool.SqlQ(sSubjectName) + "," +
                        iSortID.ToString() + "," +
                        inherit + "," +
                        StringTool.SqlQ(ImageUrl) + "," +
                        StringTool.SqlQ(sRemark) + "," +
                        ((int)eO_Deleted.eNormal).ToString() +
                        ")";
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                strSQL = "update Equ_Category set ConfigureSchema=:ConfigureSchema where CatalogID=" + lngSubjectID;
                OracleCommand cmdCST = new OracleCommand(strSQL, trans.Connection, trans);
                cmdCST.Parameters.Add("ConfigureSchema", OracleType.Clob).Value = sSchema;
                cmdCST.ExecuteNonQuery();

                strSQL = "UPDATE Equ_CateLists SET CatalogName=" + StringTool.SqlQ(sSubjectName) + " WHERE CatalogID=" + lngSubjectID;//���¶�Ӧ�ʲ�Ŀ¼
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
        ///  ɾ��
        /// </summary>
        /// <param name="lngSubjectID"></param>
        public static void Delete(long lngSubjectID)
        {
            if (lngSubjectID == 1)
                return;

            string strSQL;

            //1���ж��Ƿ��ʲ�������Ա������,���������ɾ��
            //�����������ɾ������ͬʱɾ���û�����ɱ�������ʲ���ص���Ϣ��Ȩ�ޱ�������ʲ���ص���Ϣ��
            OracleConnection cn = ConfigTool.GetConnection();
            //����ʲ��������ʲ���������ɾ��
            strSQL = "SELECT COUNT(CatalogID) FROM Equ_Category WHERE ParentID =" + lngSubjectID.ToString() + " AND deleted = " + (int)eO_Deleted.eNormal;
            int count = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));
            if (count > 0)
            {
                throw new Exception("�ʲ��ڰ��������ʲ������ʱ����ɾ��������ɾ�������ʲ����");
            }
            if (cn.State != ConnectionState.Open)
                cn.Open();

            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                strSQL = "UPDATE Equ_Category SET deleted = " + (int)eO_Deleted.eDeleted +
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

        /// <summary>
        /// ��ȡ�����µ�[����]��������ı���,�����Լ�
        /// </summary>
        /// <param name="lngCataID"></param>
        /// <returns></returns>
        public static DataTable GetBelowCatas(decimal lngCataID)
        {
            string strSQL;

            DataTable dt;
            string strFullID = "-1";   //��ʾδ�ҵ�   
            int lngFLength = 0;

            //2008-05-01 ���SQL���������Ĵ���ʽ,�������ݿ����Ӵ���
            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {

                dt = CommSqlCacheHelper.GetDataTableFromCache("equcategory");
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("CatalogID = " + lngCataID.ToString());

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
                        dt.DefaultView.Sort = "sortid";
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


            }
            else
            {

                OracleConnection cn = ConfigTool.GetConnection();

                try
                {
                    strSQL = "SELECT fullid FROM Equ_Category WHERE catalogid = " + lngCataID.ToString();
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
                        strSQL = "SELECT catalogid,Parentid,CatalogName FROM Equ_Category WHERE   " +
                            " deleted = 0 ORDER BY fullid";
                    }
                    else
                    {
                        strSQL = "SELECT catalogid,Parentid,CatalogName FROM Equ_Category WHERE   " +
                            "  SUBSTRING(fullid,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
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


        }

    }
}
