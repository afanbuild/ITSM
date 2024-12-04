/*******************************************************************
 *
 * Description:系统语言设置
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月11日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Web;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class EA_DefineLanguageDP
    {

        private static string Key = "EA_DefineLanguageCache";

        /// <summary>
        /// 
        /// </summary>
        public EA_DefineLanguageDP()
        { }

        #region Property
        #region ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mID;
        public Decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region KeyName
        /// <summary>
        ///
        /// </summary>
        private String mKeyName = string.Empty;
        public String KeyName
        {
            get { return mKeyName; }
            set { mKeyName = value; }
        }
        #endregion

        #region KeyValue
        /// <summary>
        ///
        /// </summary>
        private String mKeyValue = string.Empty;
        public String KeyValue
        {
            get { return mKeyValue; }
            set { mKeyValue = value; }
        }
        #endregion

        #region KeyValue2
        /// <summary>
        ///
        /// </summary>
        private string mKeyValue2;
        public string KeyValue2
        {
            get { return mKeyValue2; }
            set { mKeyValue2 = value; }
        }
        #endregion

        #region IsValid
        /// <summary>
        ///
        /// </summary>
        private Int32 mIsValid;
        public Int32 IsValid
        {
            get { return mIsValid; }
            set { mIsValid = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_DefineLanguageDP</returns>
        public EA_DefineLanguageDP GetReCorded(long lngID)
        {
            EA_DefineLanguageDP ee = new EA_DefineLanguageDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_DefineLanguage WHERE ID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.ID = Decimal.Parse(dr["ID"].ToString());
                    ee.KeyName = dr["KeyName"].ToString();
                    ee.KeyValue = dr["KeyValue"].ToString();
                    ee.KeyValue2 = dr["KeyValue2"].ToString();
                    ee.IsValid = Int32.Parse(dr["IsValid"].ToString());
                }
                return ee;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_DefineLanguage Where 1=1 And IsValid=0";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_DefineLanguageDP></param>
        public void InsertRecorded(EA_DefineLanguageDP pEA_DefineLanguageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                //strID = EpowerGlobal.EPGlobal.GetNextID("EA_DefineLanguageID").ToString();
               // pEA_DefineLanguageDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO EA_DefineLanguage(
									KeyName,
									KeyValue,
									KeyValue2,
									IsValid
					)
					VALUES( " +
                            //strID.ToString() + "," +
                            StringTool.SqlQ(pEA_DefineLanguageDP.KeyName) + "," +
                            StringTool.SqlQ(pEA_DefineLanguageDP.KeyValue) + "," +
                            StringTool.SqlQ(pEA_DefineLanguageDP.KeyValue2) + "," +
                            pEA_DefineLanguageDP.IsValid.ToString() +
                    ")";

                strSQL += " select @@IDENTITY";
                pEA_DefineLanguageDP.ID = (decimal)OracleDbHelper.ExecuteScalar(cn, CommandType.Text, strSQL);
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
        /// 
        /// </summary>
        /// <param name=pEA_DefineLanguageDP></param>
        public void UpdateRecorded(EA_DefineLanguageDP pEA_DefineLanguageDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_DefineLanguage Set " +
                                                        " KeyName = " + StringTool.SqlQ(pEA_DefineLanguageDP.KeyName) + "," +
                            " KeyValue = " + StringTool.SqlQ(pEA_DefineLanguageDP.KeyValue) + "," +
                            " KeyValue2 = " + StringTool.SqlQ(pEA_DefineLanguageDP.KeyValue2) + "," +
                            " IsValid = " + pEA_DefineLanguageDP.IsValid.ToString() +
                                " WHERE ID = " + pEA_DefineLanguageDP.ID.ToString();

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
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update EA_DefineLanguage Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        /// <summary>
        /// 获取关键字的值
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public  string GetLanguageValue(string sKey)
        {
            string sReturn = string.Empty;
            DataTable dt = new DataTable();
            dt = GetDataTableCache();
            DataRow[] arrdr = dt.Select("KeyName=" + Epower.DevBase.BaseTools.StringTool.SqlQ(sKey.Trim()));
            if (arrdr.Length > 0)
            {
                sReturn = arrdr[0]["KeyValue"].ToString();
            }
            return sReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  DataTable GetDataTableCache()
        {
            DataTable dt;
            if (HttpRuntime.Cache[Key] == null)
            {
                EA_DefineLanguageDP ee = new EA_DefineLanguageDP();
                dt = ee.GetDataTable(string.Empty, string.Empty);
                //插入缓存
                HttpRuntime.Cache.Insert(Key, dt);
            }
            else
            {
                dt = (DataTable)HttpRuntime.Cache[Key];
            }
            return dt;
        }


    }
}

