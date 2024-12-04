/*******************************************************************
 *
 * Description:模板
 * 
 * 
 * Create By  :zmc
 * Create Date:2008年8月27日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

using EpowerCom;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class EA_ShortCutTemplateDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_ShortCutTemplateDP()
        { }

        #region Property
        #region TemplateID
        /// <summary>
        ///
        /// </summary>
        private Decimal mTemplateID;
        public Decimal TemplateID
        {
            get { return mTemplateID; }
            set { mTemplateID = value; }
        }
        #endregion

        #region TemplateType
        /// <summary>
        ///
        /// </summary>
        private Int32 mTemplateType;
        public Int32 TemplateType
        {
            get { return mTemplateType; }
            set { mTemplateType = value; }
        }
        #endregion

        #region OFlowModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mOFlowModelID;
        public Decimal OFlowModelID
        {
            get { return mOFlowModelID; }
            set { mOFlowModelID = value; }
        }
        #endregion

        #region TemplateName
        /// <summary>
        ///
        /// </summary>
        private String mTemplateName = string.Empty;
        public String TemplateName
        {
            get { return mTemplateName; }
            set { mTemplateName = value; }
        }
        #endregion

        #region TemplateXml
        /// <summary>
        ///
        /// </summary>
        private String mTemplateXml = string.Empty;
        public String TemplateXml
        {
            get { return mTemplateXml; }
            set { mTemplateXml = value; }
        }
        #endregion

        #region Owner
        /// <summary>
        ///
        /// </summary>
        private Decimal mOwner;
        public Decimal Owner
        {
            get { return mOwner; }
            set { mOwner = value; }
        }
        #endregion

        #region AppID
        private long mAppID;
        /// <summary>
        ///  应用ID
        /// </summary>
        public long AppID
        {
            get { return mAppID; }
            set { mAppID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_ShortCutTemplateDP</returns>
        public EA_ShortCutTemplateDP GetReCorded(long lngID)
        {
            EA_ShortCutTemplateDP ee = new EA_ShortCutTemplateDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ShortCutTemplate WHERE TemplateID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                foreach (DataRow dr in dt.Rows)
                {
                    ee.TemplateID = Decimal.Parse(dr["TemplateID"].ToString());
                    ee.TemplateType = Int32.Parse(dr["TemplateType"].ToString());
                    ee.OFlowModelID = Decimal.Parse(dr["OFlowModelID"].ToString());
                    ee.TemplateName = dr["TemplateName"].ToString();
                    ee.TemplateXml = dr["TemplateXml"].ToString();
                    ee.Owner = Decimal.Parse(dr["Owner"].ToString());
                    ee.AppID = CTools.ToInt64(dr["AppID"].ToString());
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
                strSQL = "SELECT * FROM EA_ShortCutTemplate Where 1=1 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 判断是否已经存在同名模板
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public static bool IsExist(string sWhere)
        {
            bool result = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_ShortCutTemplate Where 1=1 ";
                strSQL += sWhere;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                if (dt != null && dt.Rows.Count > 0)
                    result = true;

                return result;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_ShortCutTemplateDP></param>
        public void InsertRecorded(EA_ShortCutTemplateDP pEA_ShortCutTemplateDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("EA_ShortCutTemplateID").ToString();
                pEA_ShortCutTemplateDP.TemplateID = decimal.Parse(strID);
                strSQL = @"INSERT INTO EA_ShortCutTemplate(
                                    TEMPLATEID,
									TemplateType,
									OFlowModelID,
									TemplateName,
									TemplateXml,
									Owner,
                                    AppID
					)
					VALUES( " +
                            pEA_ShortCutTemplateDP.TemplateID.ToString() + "," +
                            pEA_ShortCutTemplateDP.TemplateType.ToString() + "," +
                            pEA_ShortCutTemplateDP.OFlowModelID.ToString() + "," +
                            StringTool.SqlQ(pEA_ShortCutTemplateDP.TemplateName) + "," +
                            StringTool.SqlQ(pEA_ShortCutTemplateDP.TemplateXml) + "," +
                            pEA_ShortCutTemplateDP.Owner.ToString() + "," +
                            pEA_ShortCutTemplateDP.AppID.ToString() + 
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
        /// <param name=pEA_ShortCutTemplateDP></param>
        public void UpdateRecorded(EA_ShortCutTemplateDP pEA_ShortCutTemplateDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_ShortCutTemplate Set " +
                            " TemplateType = " + pEA_ShortCutTemplateDP.TemplateType.ToString() + "," +
                            " OFlowModelID = " + pEA_ShortCutTemplateDP.OFlowModelID.ToString() + "," +
                            " TemplateName = " + StringTool.SqlQ(pEA_ShortCutTemplateDP.TemplateName) + "," +
                            " TemplateXml = " + StringTool.SqlQ(pEA_ShortCutTemplateDP.TemplateXml) + "," +
                            " Owner = " + pEA_ShortCutTemplateDP.Owner.ToString() + "," +
                            " AppID = " + pEA_ShortCutTemplateDP.AppID.ToString() + 
                                " WHERE TemplateID = " + pEA_ShortCutTemplateDP.TemplateID.ToString();

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
                string strSQL = "Delete EA_ShortCutTemplate WHERE TemplateID =" + lngID.ToString();
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

        #region GetMyTemplaties 获取模版列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="eShortType"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public DataTable GetMyTemplaties(long lngUserID,e_ITSMShortCutReqType eShortType,bool isAll)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT a.*,to_char(a.TemplateID)||'|'||to_char(a.OFlowModelID) IDAndOFlowModelID FROM EA_ShortCutTemplate a Where (a.owner=0 or a.owner= " + lngUserID.ToString() + ") AND a.TemplateType = " + ((int)eShortType).ToString();

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);            

                if (isAll == false)
                {
                    //过滤暂时无效的流程模型
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        long lngOID = long.Parse(dt.Rows[i]["OFlowModelID"].ToString());
                        long lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOID);
                        if (lngFlowModelID != 0)
                        {
                            if (FlowModel.CanUseFlowModel(lngUserID, lngFlowModelID) != 0)
                            {
                                dt.Rows[i].Delete();
                            }
                        }
                        else
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    dt.AcceptChanges();
                }

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }

        public DataTable GetMyTemplaties(string sWhere,long lngUserID, e_ITSMShortCutReqType eShortType, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 AND TemplateType = " + ((int)eShortType).ToString();
            if (sWhere != "")
            {
                strWhere += " AND Templatename like "+StringTool.SqlQ("%"+sWhere+"%");
            }
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "EA_ShortCutTemplate a", "a.*,to_char(TemplateID)||'|'||to_char(OFlowModelID) IDAndOFlowModelID", "ORDER BY TemplateID DESC", pagesize, pageindex, strWhere, ref rowcount);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }


        public DataTable getMytempLatiesXmlHttp(long lngId)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT a.*,to_char(TemplateID)||'|'||to_char(OFlowModelID) IDAndOFlowModelID FROM EA_ShortCutTemplate a Where  a.temPlateId = " + lngId.ToString();

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }

        }
        #endregion

        #region 根据事件请求模板ID获取模板信息
        /// <summary>
        /// 根据事件请求模板ID获取模板信息
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public DataTable GetTemplsByID(string strID)
        {
            string strSql = "select * from EA_ShortCutTemplate where TemplateID = " + StringTool.SqlQ(strID);

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region 获取模版列表
        /// <summary>
        /// 获取模版列表
        /// </summary>
        /// <param name="strID">模板主表ID</param>
        /// <returns></returns>
        public DataTable GetMyTemplaties(string strID, long lngUserID, bool isAll)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT a.*,to_char(TemplateID)||'|'||to_char(OFlowModelID) IDAndOFlowModelID FROM EA_ShortCutTemplate a Where TemplateID =" + strID;

                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                

                if (isAll == false)
                {
                    //过滤暂时无效的流程模型
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        long lngOID = long.Parse(dt.Rows[i]["OFlowModelID"].ToString());
                        long lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOID);
                        if (lngFlowModelID != 0)
                        {
                            if (FlowModel.CanUseFlowModel(lngUserID, lngFlowModelID) != 0)
                            {
                                dt.Rows[i].Delete();
                            }
                        }
                        else
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    dt.AcceptChanges();
                }

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region 查询模板列表
        /// <summary>
        /// 查询模板列表
        /// </summary>
        /// <param name="strWhere">where条件 如 1=1 and </param>
        /// <param name="pagesize">每页显示数量</param>
        /// <param name="pageindex">当前第几页</param>
        /// <param name="rowcount">总记录数</param>
        /// <returns></returns>
        public DataTable GetTemplateData(string strWhere, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "EA_ShortCutTemplate a", "a.*,to_char(TemplateID)||'|'||to_char(OFlowModelID) IDAndOFlowModelID", "ORDER BY TemplateID DESC", pagesize, pageindex, strWhere, ref rowcount);

                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion
    }
}

