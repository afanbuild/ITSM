
/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :Administrator
 * Create Date:2009年4月27日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class cst_RequestDP
    {
        /// <summary>
        /// 
        /// </summary>
        public cst_RequestDP()
        { }

        #region Property
        #region id
        /// <summary>
        ///
        /// </summary>
        private Decimal mid;
        public Decimal id
        {
            get { return mid; }
            set { mid = value; }
        }
        #endregion

        #region subject
        /// <summary>
        ///
        /// </summary>
        private String msubject;
        public String subject
        {
            get { return msubject; }
            set { msubject = value; }
        }
        #endregion

        #region Contract
        /// <summary>
        ///
        /// </summary>
        private String mContract;
        public String Contract
        {
            get { return mContract; }
            set { mContract = value; }
        }
        #endregion

        #region CTel
        /// <summary>
        ///
        /// </summary>
        private String mCTel;
        public String CTel
        {
            get { return mCTel; }
            set { mCTel = value; }
        }
        #endregion

        #region Content
        /// <summary>
        ///
        /// </summary>
        private String mContent;
        public String Content
        {
            get { return mContent; }
            set { mContent = value; }
        }
        #endregion

        #region inDate
        /// <summary>
        ///
        /// </summary>
        private DateTime minDate = DateTime.MinValue;
        public DateTime inDate
        {
            get { return minDate; }
            set { minDate = value; }
        }
        #endregion

        #region inType
        /// <summary>
        ///
        /// </summary>
        private Int32 minType;
        public Int32 inType
        {
            get { return minType; }
            set { minType = value; }
        }
        #endregion

        #region DealLog
        /// <summary>
        ///
        /// </summary>
        private long mDealLog;
        public long DealLog
        {
            get { return mDealLog; }
            set { mDealLog = value; }
        }
        #endregion

        #region updateTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mupdateTime = DateTime.MinValue;
        public DateTime updateTime
        {
            get { return mupdateTime; }
            set { mupdateTime = value; }
        }
        #endregion

        #region upuserid
        /// <summary>
        ///
        /// </summary>
        private Decimal mupuserid;
        public Decimal upuserid
        {
            get { return mupuserid; }
            set { mupuserid = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>cst_RequestDP</returns>
        public cst_RequestDP GetReCorded(long lngID)
        {
            cst_RequestDP ee = new cst_RequestDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM cst_Request WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.id = Decimal.Parse(dr["id"].ToString());
                ee.subject = dr["subject"].ToString();
                ee.Content = dr["Content"].ToString();
                ee.CTel = dr["CTel"].ToString();
                ee.Contract = dr["Contract"].ToString();
                ee.inDate = dr["inDate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["inDate"].ToString());
                ee.inType = Int32.Parse(dr["inType"].ToString());
                ee.DealLog = long.Parse(dr["DealLog"].ToString());
                ee.updateTime = dr["updateTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["updateTime"].ToString());
                ee.upuserid = Decimal.Parse(dr["upuserid"].ToString());
            }
            return ee;
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
            strSQL = "SELECT * FROM cst_Request Where 1=1  ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pcst_RequestDP></param>
        public void InsertRecorded(cst_RequestDP pcst_RequestDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = EPGlobal.GetNextID("cst_RequestID").ToString();
            try
            {
                strSQL = @"INSERT INTO cst_Request(
                                    ID,
									subject,
									Content,
                                    Contract,
									CTel,
									inDate,
									inType,
									DealLog
					)
					VALUES( " +
                            strID + "," +
                            StringTool.SqlQ(pcst_RequestDP.subject) + "," +
                            StringTool.SqlQ(pcst_RequestDP.Content) + "," +
                            StringTool.SqlQ(pcst_RequestDP.Contract) + "," +
                            StringTool.SqlQ(pcst_RequestDP.CTel) + "," +
                            (pcst_RequestDP.inDate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pcst_RequestDP.inDate.ToString()) + ",'yyyy-MM-dd   hh24:mi:ss')") + "," +
                            pcst_RequestDP.inType.ToString() + "," +
                            pcst_RequestDP.DealLog.ToString() +
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
        /// 
        /// </summary>
        /// <param name=pcst_RequestDP></param>
        public void UpdateRecorded(cst_RequestDP pcst_RequestDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE cst_Request Set " +
                                                        " subject = " + StringTool.SqlQ(pcst_RequestDP.subject) + "," +
                            " Contract = " + StringTool.SqlQ(pcst_RequestDP.Contract) + "," +
                            " CTel = " + StringTool.SqlQ(pcst_RequestDP.CTel) + "," +
                            " Content = " + StringTool.SqlQ(pcst_RequestDP.Content) + "," +
                            " inDate = " + (pcst_RequestDP.inDate == DateTime.MinValue ? " null " : StringTool.SqlQ(pcst_RequestDP.inDate.ToString())) + "," +
                            " inType = " + pcst_RequestDP.inType.ToString() + "," +
                            " DealLog = " + pcst_RequestDP.DealLog.ToString() +
                                " WHERE id = " + pcst_RequestDP.id.ToString();

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
                string strSQL = "delete cst_Request   WHERE ID =" + lngID.ToString();
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


        #region GetDataTable static 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(int iSize, string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM cst_Request Where deallog=0 and rownum<=" + iSize.ToString();
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 判断是否有新的请求
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string HasNewRequest(long id)
        {
            string strRet = "0";
            string strSQL = "";
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT count(*) FROM cst_Request Where deallog=0 AND id >  " + id.ToString();
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                while (dr.Read())
                {
                    int i = dr.GetInt32(0);
                    if (i > 0)
                        strRet = "1";
                }
                dr.Close();                

                return strRet;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 设置请求无效 SetRequestNoUse
        public static void SetRequestNoUse(long lngID,long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "UPDATE cst_request SET deallog = -1" +
                                     ", updateTime = sysdate ,  upuserid = " + lngUserID.ToString() +
                                     " WHERE id = " + lngID.ToString();
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

        #region 获取请求快照数据（XML）

        /// <summary>
        /// 获取业务快照数据（XML）
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static string GetRequestShotValues(long lngID)
        {
            string strRet = string.Empty;

            string strSQL = "";

            OracleConnection cn = ConfigTool.GetConnection();

            strSQL = "SELECT * FROM cst_Request WHERE id =" + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    FieldValues fv = new FieldValues();
                    string strTemp = "";

                    EA_DefineLanguageDP dl = new EA_DefineLanguageDP();


                    fv.Add("来源", row["contract"].ToString());

                    fv.Add("联系电话", row["ctel"].ToString());

                    fv.Add("内容", row["content"].ToString());



                    strRet = fv.GetXmlObject().InnerXml;


                }
            }

            return strRet;
        }

        #endregion
    }
}

