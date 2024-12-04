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
    public class Equ_ServerDP
    {
        /// <summary>
        /// ��ȡ���и�����(TemplateID=-1)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRootSubject()
        {
            DataTable dt;
            string strSQL = "SELECT * FROM EA_ServicesTemplate WHERE TemplateID=-1 ORDER BY TemplateID";
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;

        }

        /// <summary>
        /// ��ȡ����Ŀ¼��FULLID
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
                strSQL = "SELECT fullid FROM Equ_Category WHERE CatalogID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strFullID = dr.GetString(0).Trim();
                }
                dr.Close();
                
                return strFullID;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        /// <summary>
        /// ��ȡ���з���Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServers()
        {
            DataTable dt;
            string strSQL = "SELECT * FROM EA_ServicesTemplate  ORDER BY TemplateID";

            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetServerByID(long lngSubjectID)
        {
            DataTable dt;
            string strSQL = "SELECT * FROM EA_ServicesTemplate WHERE 1=1 and TemplateID=" + lngSubjectID.ToString() +
                " ORDER BY TemplateID";

            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;

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

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT imglogo FROM EA_ServicesTemplate WHERE TemplateID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strImageUrl = dr.GetString(0).Trim();
                }
                dr.Close();                

                return strImageUrl;
            }
            finally { ConfigTool.CloseConnection(cn); }
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
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT TemplateName FROM EA_ServicesTemplate WHERE  TemplateID = " + lngSubjectID.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    strSubjectName = dr.GetString(0).Trim();
                }
                dr.Close();                

                return strSubjectName;
            }
            finally { ConfigTool.CloseConnection(cn); }
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
                strSQL = "SELECT GetCatalogSchma(fullid) as stSchemma FROM EA_ServicesTemplate WHERE CatalogID = " + lngSubjectID.ToString();
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
        public static string Save(long templateID, string templateType, long oflowModelID, string templateName, string templateXml, long serviceLevelID, string serviceLevel, string content, string imglogo, long isParent, long issTempID, string isstempname, long attachment)
        {
            //���»��������ɣġ����������Σ��ͣš������ڡ��յ�ʱ��������
            string strSQL = string.Empty;
            string strSubjectID = "0";
            OracleConnection cn = ConfigTool.GetConnection();
            long lngNextID = 0;
            string strParentFullID = GetSubjectFullID(serviceLevelID);
            string strFullID = string.Empty;

            cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                if (templateID != 0)
                {
                    strSubjectID = templateID.ToString();
                    if (templateID == 1)
                    {
                        //���������⴦��
                        strFullID = "";
                    }
                    else
                    {
                        strFullID = strParentFullID + templateID.ToString().PadLeft(6, char.Parse("0"));
                    }
                    strSQL = "UPDATE EA_ServicesTemplate SET " +
                        " TemplateID =" + templateID + "," +
                        " TemplateType =" + StringTool.SqlQ(templateType) + "," +
                        " OFlowModelID = " + oflowModelID + "," +
                        " TemplateName = " + StringTool.SqlQ(templateName) + "," +
                        " TemplateXml = " + StringTool.SqlQ(templateXml) + "," +
                        " ServiceLevelID =" + serviceLevelID + "," +
                        " ServiceLevel = " + StringTool.SqlQ(serviceLevel) + "," +
                        " Content = " + StringTool.SqlQ(content) + "," +
                        " imglogo =" + StringTool.SqlQ(imglogo) + "," +
                        " IsParent =" + isParent + "," +
                        " issTempID =" + issTempID + "," +
                        " isstempname=" + StringTool.SqlQ(isstempname) + "," +
                        " attachment=" + attachment +
                        " WHERE templateID = " + templateID.ToString();
                }
                else
                {
                    lngNextID = EpowerGlobal.EPGlobal.GetNextID("Equ_CategoryID");
                    strSubjectID = lngNextID.ToString();
                    strFullID = strParentFullID + lngNextID.ToString().PadLeft(6, char.Parse("0"));
                    strSQL = @"INSERT INTO EA_ServicesTemplate   TemplateID,
									TemplateType,
									OFlowModelID,
									TemplateName,
									TemplateXml,
									ServiceLevelID,
                                    ServiceLevel,
                                    Content,
                                    ServiceKindID,
                                    ServiceKind,
                                    Guide,
                                    imglogo,
                                    IsParent,
                                    IssTempID,
                                    IssTempName" +
                        " Values(" +
                        lngNextID.ToString() + "," +
                        StringTool.SqlQ(templateType) + "," +
                        oflowModelID.ToString() + "," +
                        StringTool.SqlQ(templateName) + "," +
                        StringTool.SqlQ(templateXml) + "," +
                        serviceLevelID.ToString() + "," +
                        StringTool.SqlQ(serviceLevel) + "," +
                        StringTool.SqlQ(content) + "," +
                        StringTool.SqlQ(imglogo) + "," +
                        isParent.ToString() + "," +
                        issTempID.ToString() + "," +
                        StringTool.SqlQ(isstempname) + "," +
                        attachment.ToString() +
                        ")";
                }
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                //strSQL = "UPDATE Equ_CateLists SET CatalogName=" + StringTool.SqlQ(sSubjectName) + " WHERE CatalogID=" + lngSubjectID;//���¶�Ӧ�ʲ�Ŀ¼
                //OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

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
        public static void Delete(long lngServerID)
        {
            if (lngServerID == 1)
                return;

            string strSQL;

            //1���ж��Ƿ��ʲ�������Ա������,���������ɾ��
            //�����������ɾ������ͬʱɾ���û�����ɱ�������ʲ���ص���Ϣ��Ȩ�ޱ�������ʲ���ص���Ϣ��
            OracleConnection cn = ConfigTool.GetConnection();
            //����ʲ��������ʲ���������ɾ��
            strSQL = "SELECT COUNT(TEMPLATEID) FROM EA_ServicesTemplate WHERE ServiceLevelID =" + lngServerID.ToString();
            int count = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL));
            if (count > 0)
            {
                throw new Exception("����Ŀ¼�ڰ�����������Ŀ¼����ʱ����ɾ��������ɾ����������Ŀ¼��");
            }
            if (cn.State != ConnectionState.Open)
                cn.Open();

            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                strSQL = "delete from EA_ServicesTemplate  " + " WHERE TemplateID = " + lngServerID.ToString();
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

            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT TemplateID FROM EA_ServicesTemplate WHERE  ServiceLevelID= " + lngCataID.ToString();
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
                    strSQL = "SELECT TemplateID,ServiceLevelID,TemplateName FROM EA_ServicesTemplate  WHERE   " +
                        "  SUBSTRING(TemplateID,1," + lngFLength.ToString() + ") = " + StringTool.SqlQ(strFullID) +
                        " ORDER BY sortid ";

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


        #region ȡ������� - 2013-11-28 @������

        /// <summary>
        /// ȡ�������
        /// </summary>
        /// <param name="lngFlowID">���̱��</param>
        /// <returns></returns>
        public static String GetEQUChangeNO(long lngFlowID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                String strSQL = String.Format(@"select changeno  from equ_changeservice where flowid ={0}", lngFlowID);
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

                return dt.Rows[0]["changeno"].ToString().Trim();
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion


    }

}

