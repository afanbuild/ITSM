/*******************************************************************
 *
 * Description:������������������
 * 
 * 
 * Create By  :����ǰ
 * Create Date:2013-04-10
 * *****************************************************************/
using System;
using System.Data;
using System.Collections;
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
    public class BR_CatalogSchemaItemsDP
    {
        /// <summary>
        /// 
        /// </summary>
        public BR_CatalogSchemaItemsDP()
        { }

        #region Property
        #region ID
        /// <summary>
        /// ��ʾID
        /// </summary>
        private long mID;
        public long ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region FieldID
        /// <summary>
        /// ��������ID
        /// </summary>
        private String mFieldID;
        public String FieldID
        {
            get { return mFieldID; }
            set { mFieldID = value; }
        }
        #endregion

        #region CHName
        /// <summary>
        /// ����������
        /// </summary>
        private String mCHName;
        public String CHName
        {
            get { return mCHName; }
            set { mCHName = value; }
        }
        #endregion

        #region itemType
        /// <summary>
        ///  ����������
        /// </summary>
        private Int32 mitemType;
        public Int32 itemType
        {
            get { return mitemType; }
            set { mitemType = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///  �Ƿ�ɾ��
        /// </summary>
        private Int32 mDeleted;
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region CatalogID
        /// <summary>
        /// ���Ϊ�����������ʱ����س������ID
        /// </summary>
        private long mCatalogID;
        public long CatalogID
        {
            get { return mCatalogID; }
            set { mCatalogID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// ��ѯĳ����¼
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>BR_CatalogSchemaItemsDP</returns>
        public BR_CatalogSchemaItemsDP GetReCorded(long lngID)
        {
            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();

            string strSQL = string.Empty;

            strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE ID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = CTools.ToInt64(dr["id"].ToString());
                ee.FieldID = dr["FieldID"].ToString();
                ee.CHName = dr["CHName"].ToString();
                ee.itemType = CTools.ToInt(dr["itemType"].ToString(), 0);
                ee.Deleted = CTools.ToInt(dr["Deleted"].ToString());
                ee.CatalogID = CTools.ToInt64(dr["CatalogID"].ToString() == "" ? "1" : dr["CatalogID"].ToString());
            }
            return ee;

        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// ��ѯ���������������Ϣ
        /// </summary>
        /// <param name="sWhere">where���� �� and 1=1 </param>
        /// <param name="sOrder">�������� �� order by ID </param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM BR_CatalogSchemaItems Where 1=1 And Deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }

        /// <summary>
        /// ��ҳ��ѯ���������������Ϣ
        /// </summary>
        /// <param name="sWhere">where���� �� and 1=1</param>
        /// <param name="sOrder">�������� �� order by ID</param>
        /// <param name="pagesize">ÿҳ��ʾ����</param>
        /// <param name="pageindex">��ǰ�ڼ�ҳ</param>
        /// <param name="rowcount">����������</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            DataTable dt = null;

            string strWhere = " 1=1 And Deleted=0 " + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, "BR_CatalogSchemaItems", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            }
            catch { }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// ����¼�¼
        /// </summary>
        /// <param name=pBR_CatalogSchemaItemsDP></param>
        public void InsertRecorded(BR_CatalogSchemaItemsDP pBR_CatalogSchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_CatalogSchemaItemsID").ToString();
                pBR_CatalogSchemaItemsDP.ID = CTools.ToInt64(strID);
                strSQL = @"INSERT INTO BR_CatalogSchemaItems(
									id,
									FieldID,
									CHName,
									itemType,
									Deleted,
                                    CatalogID
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(getMaxFiledID().ToString()) + "," +
                            StringTool.SqlQ(pBR_CatalogSchemaItemsDP.CHName) + "," +
                            pBR_CatalogSchemaItemsDP.itemType.ToString() + "," +
                            pBR_CatalogSchemaItemsDP.Deleted.ToString() + "," +
                            pBR_CatalogSchemaItemsDP.CatalogID.ToString() +
                    ")";

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
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// ���¼�¼
        /// </summary>
        /// <param name=pBR_CatalogSchemaItemsDP></param>
        public void UpdateRecorded(BR_CatalogSchemaItemsDP pBR_CatalogSchemaItemsDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE BR_CatalogSchemaItems Set " +
                            " FieldID = " + StringTool.SqlQ(pBR_CatalogSchemaItemsDP.FieldID) + "," +
                            " CHName = " + StringTool.SqlQ(pBR_CatalogSchemaItemsDP.CHName) + "," +
                            " itemType = " + pBR_CatalogSchemaItemsDP.itemType.ToString() + "," +
                            " Deleted = " + pBR_CatalogSchemaItemsDP.Deleted.ToString() + "," +
                            " CatalogID = " + pBR_CatalogSchemaItemsDP.CatalogID.ToString() +
                                " WHERE id = " + pBR_CatalogSchemaItemsDP.ID.ToString();

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
        #endregion

        #region DeleteRecorded
        /// <summary>
        /// ɾ��ĳ����¼
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update BR_CatalogSchemaItems Set Deleted=1  WHERE ID =" + lngID.ToString();
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
        #endregion

        #region �жϸ��������Ƿ��Ѿ�ʹ��
        /// <summary>
        /// �жϸ��������Ƿ��Ѿ�ʹ��
        /// </summary>
        /// <param name="FeildID">��������ID</param>
        /// <returns></returns>
        public bool IsUsed(long FeildID)
        {

            bool isTrue = false;
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "select * from es_catalog where deleted=0 and configureSchema like " + StringTool.SqlQ("%ID=\"" + FeildID.ToString() + "\"%");
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                {
                    isTrue = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }

            return isTrue;
        }
        #endregion

        #region CheckIsRepeat
        /// <summary>
        /// ����Ƿ����
        /// </summary>
        /// <param name="FieldID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeat(string FieldID, long ID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            bool breturn = false;
            string strSQL = @"select ID from BR_CatalogSchemaItems
							 WHERE  FieldID =" + StringTool.SqlQ(FieldID);
            if (ID != 0)
                strSQL += " And ID<>" + ID.ToString();

            try
            {
                object pobject = OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
                if (pobject != null)
                    breturn = true;
                return breturn;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region CheckIsRepeatName
        /// <summary>
        /// ����Ƿ��ظ�
        /// </summary>
        /// <param name="FieldName">����������</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIsRepeatName(string FieldName, long ID, ref BR_CatalogSchemaItemsDP Schema)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            bool breturn = false;
            string strSQL = @"select ID from BR_CatalogSchemaItems 
							 WHERE Deleted=0 AND CHName =" + StringTool.SqlQ(FieldName);
            if (ID != 0)
                strSQL += " And ID<>" + ID.ToString();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                if (dt.Rows.Count > 0)
                {
                    //��ø���Ϣ
                    Schema = GetReCorded(long.Parse(dt.Rows[0]["ID"].ToString()));
                    breturn = true;
                }
                return breturn;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }


        }
        #endregion

        #region ������������ID��ѯĳ����¼
        /// <summary>
        /// ������������ID��ѯĳ����¼
        /// </summary>
        /// <param name="sFieldID">��������ID</param>
        /// <returns>BR_CatalogSchemaItemsDP</returns>
        public BR_CatalogSchemaItemsDP GetReCorded(string sFieldID)
        {
            BR_CatalogSchemaItemsDP ee = new BR_CatalogSchemaItemsDP();
            
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE  FieldID = " + StringTool.SqlQ(sFieldID);
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = CTools.ToInt64(dr["id"].ToString());
                ee.FieldID = dr["FieldID"].ToString();
                ee.CHName = dr["CHName"].ToString();
                ee.itemType = CTools.ToInt(dr["itemType"].ToString(), 0);
                ee.Deleted = CTools.ToInt(dr["Deleted"].ToString());
                ee.CatalogID = CTools.ToInt64(dr["CatalogID"].ToString() == "" ? "1" : dr["CatalogID"].ToString());
            }
            return ee;

        }
        #endregion

        #region GetAllFields

        public Hashtable GetAllFields()
        {
            return GetAllFields(-1);
        }

        public Hashtable GetAllFieldsHistory()
        {
            return GetAllFieldsHistory(-1);
        }

        /// <summary>
        /// ��ȡ�����ֶ�����ID��ֵ����Ϣ
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetAllFieldsHistory(int iType)
        {
            Hashtable ht = new Hashtable();

            string strSQL = string.Empty;
            
            if (iType == -1)
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems ";
            }
            else
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE  itemType = " + iType.ToString();
            }
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);            
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
            }

            return ht;
        }
        /// <summary>
        /// ��ȡ�����ֶ�����ID��ֵ����Ϣ
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetAllFields(int iType)
        {
            Hashtable ht = new Hashtable();

            string strSQL = string.Empty;
            
            if (iType == -1)
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0 ";
            }
            else
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
            }
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString());
            }

            return ht;
        }
        #endregion

        #region ��ȡĳ�������µ�������������Ϣ
        /// <summary>
        /// ��ȡĳ�������µ�������������Ϣ
        /// </summary>
        /// <param name="iType"></param>
        /// <returns></returns>
        public Hashtable GetItemsAllFields(int iType)
        {
            Hashtable ht = new Hashtable();

            string strSQL = string.Empty;
            
            if (iType == -1)
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0";
            }
            else
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString();
            }
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString() + dr["itemType"].ToString());
            }

            return ht;
        }
        #endregion

        #region ���ݲ�����ȡĳ����������Ϣ
        /// <summary>
        /// ���ݲ�����ȡĳ����������Ϣ
        /// </summary>
        /// <param name="iType">����������ID</param>
        /// <param name="id">��ʾID</param>
        /// <returns></returns>
        public Hashtable GetItemsByID(int iType, decimal id)
        {
            Hashtable ht = new Hashtable();            

            string strSQL = string.Empty;
            
            if (iType == -1)
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0 and ID=" + id;
            }
            else
            {
                strSQL = "SELECT * FROM BR_CatalogSchemaItems WHERE deleted = 0 AND itemType = " + iType.ToString() + "and ID=" + id;
            }
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            
            foreach (DataRow dr in dt.Rows)
            {
                ht.Add(dr["FieldID"].ToString(), dr["CHName"].ToString() + dr["itemType"].ToString());
            }

            return ht;
        }
        #endregion

        #region �õ����ı��ID

        /// <summary>
        /// �õ����ı��ID
        /// </summary>
        /// <returns></returns>
        public static decimal getMaxFiledID()
        {
            decimal maxID = 1;
            string strSQL = "select max(to_number(nvl(FieldID,0)))+1 as maxid from BR_CatalogSchemaItems";
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                OracleDataReader dReader = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dReader.Read())
                {
                    maxID = decimal.Parse(dReader.GetInt32(0).ToString());
                }
                dReader.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return maxID;
        }

        #endregion
    }
}

