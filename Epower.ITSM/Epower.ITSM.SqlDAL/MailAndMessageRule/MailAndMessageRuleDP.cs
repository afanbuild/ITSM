/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :wangxiuwei
 * Create Date:2011年2月16日
 * *****************************************************************/
using System;
using System.Data;
using System.Xml;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Data.OracleClient;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class MailAndMessageRuleDP
    {
        /// <summary>
        /// 
        /// </summary>
        public MailAndMessageRuleDP()
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

        #region RuleName
        /// <summary>
        ///
        /// </summary>
        private String mRuleName = string.Empty;
        public String RuleName
        {
            get { return mRuleName; }
            set { mRuleName = value; }
        }
        #endregion
  
        #region SystemID
        /// <summary>
        ///
        /// </summary>
        private Decimal mSystemID;
        public Decimal SystemID
        {
            get { return mSystemID; }
            set { mSystemID = value; }
        }
        #endregion

        #region SystemName
        /// <summary>
        ///
        /// </summary>
        private String mSystemName = string.Empty;
        public String SystemName
        {
            get { return mSystemName; }
            set { mSystemName = value; }
        }
        #endregion

        #region ModelID
        /// <summary>
        ///
        /// </summary>
        private Decimal mModelID;
        public Decimal ModelID
        {
            get { return mModelID; }
            set { mModelID = value; }
        }
        #endregion

        #region ModelName
        /// <summary>
        ///
        /// </summary>
        private String mModelName = string.Empty;
        public String ModelName
        {
            get { return mModelName; }
            set { mModelName = value; }
        }
        #endregion

        #region ReceiversTypeID
        /// <summary>
        ///
        /// </summary>
        private Decimal mReceiversTypeID;
        public Decimal ReceiversTypeID
        {
            get { return mReceiversTypeID; }
            set { mReceiversTypeID = value; }
        }
        #endregion

        #region ReceiversTypeName
        /// <summary>
        ///
        /// </summary>
        private String mReceiversTypeName = string.Empty;
        public String ReceiversTypeName
        {
            get { return mReceiversTypeName; }
            set { mReceiversTypeName = value; }
        }
        #endregion


        #region TRIGGER_TYPE
        private String mTrigger_Type = string.Empty;
        public String Trigger_Type
        {
            get { return mTrigger_Type; }
            set { mTrigger_Type = value; }
        }
        #endregion

        #region INTERVAL_TIME
        private String mInterval_time = string.Empty;
        public String Interval_time
        {
            get { return mInterval_time; }
            set { mInterval_time = value; }
        }

        #endregion


        #region RECIPIENT_USERID
        private String mRecipient_UserID = string.Empty;
        public String Recipient_UserID
        {
            get { return mRecipient_UserID; }
            set { mRecipient_UserID = value; }
        }
        #endregion

        #region RECIPIENT_USER
        private String mRecipient_User = string.Empty;
        public String Recipient_User
        {
            get { return mRecipient_User; }
            set { mRecipient_User = value; }
        }
        #endregion


        #region RUserIDList
        /// <summary>
        ///
        /// </summary>
        private String mRUserIDList = string.Empty;
        public String RUserIDList
        {
            get { return mRUserIDList; }
            set { mRUserIDList = value; }
        }
        #endregion

        #region RUserNameList
        /// <summary>
        ///
        /// </summary>
        private String mRUserNameList = string.Empty;
        public String RUserNameList
        {
            get { return mRUserNameList; }
            set { mRUserNameList = value; }
        }
        #endregion

        #region SenderTypeID
        /// <summary>
        ///
        /// </summary>
        private Decimal mSenderTypeID;
        public Decimal SenderTypeID
        {
            get { return mSenderTypeID; }
            set { mSenderTypeID = value; }
        }
        #endregion

        #region SenderTypeName
        /// <summary>
        ///
        /// </summary>
        private String mSenderTypeName = string.Empty;
        public String SenderTypeName
        {
            get { return mSenderTypeName; }
            set { mSenderTypeName = value; }
        }
        #endregion         
  
        #region TimeCount
        /// <summary>
        ///
        /// </summary>
        private Decimal mTimeCount;
        public Decimal TimeCount
        {
            get { return mTimeCount; }
            set { mTimeCount = value; }
        }
        #endregion

        #region MailTitle
        /// <summary>
        ///
        /// </summary>
        private String mMailTitle = string.Empty;
        public String MailTitle
        {
            get { return mMailTitle; }
            set { mMailTitle = value; }
        }
        #endregion

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

        #region RegTime

        private DateTime mRegTime = DateTime.MinValue;
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegTime
        {
            get{return mRegTime;}
            set{mRegTime = value;}
        }

        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Decimal mDeleted;
        public Decimal Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region Status
        /// <summary>
        ///
        /// </summary>
        private Decimal mStatus;
        public Decimal Status
        {
            get { return mStatus; }
            set { mStatus = value; }
        }
        #endregion

        #region Remark
        /// <summary>
        ///
        /// </summary>
        private String mRemark = string.Empty;
        public String Remark
        {
            get { return mRemark; }
            set { mRemark = value; }
        }
        #endregion

        #region RegUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegUserID;
        public Decimal RegUserID
        {
            get { return mRegUserID; }
            set { mRegUserID = value; }
        }
        #endregion

        #region RegUserName
        /// <summary>
        ///
        /// </summary>
        private String mRegUserName = string.Empty;
        public String RegUserName
        {
            get { return mRegUserName; }
            set { mRegUserName = value; }
        }
        #endregion
       
        #region RegDeptID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRegDeptID;
        public Decimal RegDeptID
        {
            get { return mRegDeptID; }
            set { mRegDeptID = value; }
        }
        #endregion

        #region RegDeptName
        /// <summary>
        ///
        /// </summary>
        private String mRegDeptName = string.Empty;
        public String RegDeptName
        {
            get { return mRegDeptName; }
            set { mRegDeptName = value; }
        }
        #endregion         

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>MailAndMessageRuleDP</returns>
        public MailAndMessageRuleDP GetReCorded(long lngID)
        {
            MailAndMessageRuleDP ee = new MailAndMessageRuleDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM MailAndMessageRule WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.RuleName = dr["RuleName"].ToString();
                ee.Remark = dr["Remark"].ToString();
                ee.SystemID = decimal.Parse(dr["SystemID"].ToString());
                ee.SystemName = dr["SystemName"].ToString();
                ee.ModelID = decimal.Parse(dr["ModelID"].ToString());
                ee.ModelName = dr["ModelName"].ToString();
                ee.ReceiversTypeID = decimal.Parse(dr["ReceiversTypeID"].ToString());
                ee.ReceiversTypeName = dr["ReceiversTypeName"].ToString();
                ee.SenderTypeID = decimal.Parse(dr["SenderTypeID"].ToString());
                ee.SenderTypeName = dr["SenderTypeName"].ToString();
                ee.TimeCount = decimal.Parse(dr["TimeCount"].ToString());
                ee.Deleted = decimal.Parse(dr["Deleted"].ToString());
                ee.MailTitle = dr["MailTitle"].ToString();
                ee.TemplateID = decimal.Parse(dr["TemplateID"].ToString());
                ee.TemplateName = dr["TemplateName"].ToString();
                ee.RegTime = DateTime.Parse(dr["RegTime"].ToString());
                ee.Status = decimal.Parse(dr["Status"].ToString());
                ee.Remark = dr["Remark"].ToString();
                ee.RegUserID = decimal.Parse(dr["RegUserID"].ToString());
                ee.RegUserName = dr["RegUserName"].ToString();
                ee.RegDeptID = decimal.Parse(dr["RegDeptID"].ToString());
                ee.RegDeptName = dr["RegDeptName"].ToString();
                ee.RUserIDList = dr["RUserIDList"].ToString();
                ee.RUserNameList = dr["RUserNameList"].ToString();
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
            strSQL = "SELECT * FROM MailAndMessageRule";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 判断某个流程模型下是否已经存在规则
        /// <summary>
        /// 判断某个流程模型下是否已经存在规则
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static bool IsExistsByModel(string sWhere)
        {
            bool result = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM MailAndMessageRule where 1=1 ";
            strSQL += sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }
        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pMailAndMessageRuleDP></param>
        public void InsertRecorded(MailAndMessageRuleDP pMailAndMessageRuleDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("MailAndMessageRuleID").ToString();
                pMailAndMessageRuleDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO MailAndMessageRule(
									ID,
                                    RULENAME,
                                    SYSTEMID,
                                    SYSTEMNAME,
                                    MODELID,
                                    MODELNAME,
                                    RECEIVERSTYPEID,
                                    RECEIVERSTYPENAME,
                                    RUserIDList,
                                    RUserNameList,
                                    SENDERTYPEID,
                                    SENDERTYPENAME,
                                    TimeCount,
                                    MAILTITLE,
                                    TemplateID,
                                    TemplateName,
                                    Status,
                                    DELETED,
                                    REGTIME,
                                    REGUSERID,
                                    REGUSERNAME,
                                    REGDEPTID,
                                    REGDEPTNAME,
                                    REMARK
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.RuleName) + "," +
                            pMailAndMessageRuleDP.SystemID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.SystemName) + "," +
                            pMailAndMessageRuleDP.ModelID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.ModelName) + "," +
                            pMailAndMessageRuleDP.ReceiversTypeID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.ReceiversTypeName) + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.RUserIDList) + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.RUserNameList) + "," +
                            pMailAndMessageRuleDP.SenderTypeID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.SenderTypeName) + "," +
                            pMailAndMessageRuleDP.TimeCount.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.MailTitle) + "," +
                            pMailAndMessageRuleDP.TemplateID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.TemplateName) + "," +
                            pMailAndMessageRuleDP.Status.ToString() + "," +
                            "0," +
                            "to_date(" + StringTool.SqlQ(pMailAndMessageRuleDP.RegTime.ToString()) + ",'yyyy-mm-dd   hh24:mi:ss')," +
                            pMailAndMessageRuleDP.RegUserID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.RegUserName) + "," +
                            pMailAndMessageRuleDP.RegDeptID.ToString() + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.RegDeptName) + "," +
                            StringTool.SqlQ(pMailAndMessageRuleDP.Remark.ToString()) +
                    ")";

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                tran.Dispose();
                ConfigTool.CloseConnection(cn);
            }

        }

        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pCst_RecommendRuleDP></param>
        public void UpdateRecorded(MailAndMessageRuleDP pMailAndMessageRuleDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE MailAndMessageRule Set " +
                            "RuleName = " + StringTool.SqlQ(pMailAndMessageRuleDP.RuleName) + "," +
                            "SystemID = " + pMailAndMessageRuleDP.SystemID.ToString() + "," +
                            "SystemName = " + StringTool.SqlQ(pMailAndMessageRuleDP.SystemName) + "," +
                            "ModelID = " + pMailAndMessageRuleDP.ModelID.ToString() + "," +
                            "ModelName = " + StringTool.SqlQ(pMailAndMessageRuleDP.ModelName) + "," +
                            "ReceiversTypeID = " + pMailAndMessageRuleDP.ReceiversTypeID.ToString() + "," +
                            "ReceiversTypeName = " + StringTool.SqlQ(pMailAndMessageRuleDP.ReceiversTypeName) + "," +
                            "RUserIDList = " + StringTool.SqlQ(pMailAndMessageRuleDP.RUserIDList) + "," +
                            "RUserNameList = " + StringTool.SqlQ(pMailAndMessageRuleDP.RUserNameList) + "," +
                            "SenderTypeID = " + pMailAndMessageRuleDP.SenderTypeID.ToString() + "," +
                            "SenderTypeName = " + StringTool.SqlQ(pMailAndMessageRuleDP.SenderTypeName) + "," +
                            "TimeCount = " + pMailAndMessageRuleDP.TimeCount.ToString() + "," +
                            "MailTitle = " + StringTool.SqlQ(pMailAndMessageRuleDP.MailTitle) + "," +
                            "TemplateID = " + pMailAndMessageRuleDP.TemplateID.ToString() + "," +
                            "TemplateName = " + StringTool.SqlQ(pMailAndMessageRuleDP.TemplateName) + "," +
                            "Status = " + pMailAndMessageRuleDP.Status.ToString() + "," +
                            " Remark = " + StringTool.SqlQ(pMailAndMessageRuleDP.Remark.ToString()) +
                            " WHERE ID = " + pMailAndMessageRuleDP.ID.ToString();

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                tran.Dispose();
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
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                string strSQL = "DELETE MailAndMessageRule  WHERE ID =" + lngID.ToString();
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                tran.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion

        #region Details

        /// <summary>
        /// 得到相关规则的时间信息列表
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static DataTable getDetails(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = "SELECT * FROM MailAndMessageRule  WHERE ID=" + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

       

        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "MailAndMessageRule", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion


        /// <summary>
        /// 获应用名称(应用名称)
        /// </summary>
        /// <param name="lngFlowID">flowid</param>
        /// <returns></returns>
        public static string   getAppName(long appid)
        {
              OracleConnection cn = ConfigTool.GetConnection();
              string strSql = "select appname from es_app where appid =" + appid.ToString();
              try
              {
                  DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);

                  string appName ="";
                  if (dt != null)
                  {
                      if (dt.Rows.Count > 0)
                      {   
                          appName=dt.Rows[0]["appname"].ToString();
                      }
                  }
                  return appName;
              }
              catch (Exception ex)
              {
                  return "";
              }
              finally {
                  ConfigTool.CloseConnection(cn);
              }
        }


        /// <summary>
        ///返回流程oldFlowModelId
        /// </summary>
        /// <param name="lngFlowModelID">flowModelid</param>
        /// <param name="appid">应用id</param>
        /// <param name="flowname">流程名称</param>
        /// <returns>返回流程oldFlowModelId</returns>
        public static long getEmailModel(long lngFlowModelID,ref long appid, ref string flowname)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSql = "select  flowname ,appid ,oflowmodelid from es_flowmodel where flowmodelid=" + lngFlowModelID;
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);

                long Oflowmodelid = 0;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Oflowmodelid = long.Parse(dt.Rows[0]["Oflowmodelid"].ToString());
                        flowname = dt.Rows[0]["flowname"].ToString();
                        appid = long.Parse(dt.Rows[0]["appid"].ToString());
                    }
                }
                return Oflowmodelid;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        #region 获取应用名称 GetAppName

        /// <summary>
        /// 获应用名称(应用名称)
        /// </summary>
        /// <param name="lngFlowID">flowid</param>
        /// <returns></returns>
        public static string GetAppName(long lngAppID)
        {
            DataTable dt;
            string appName = "";
            //添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {
                dt = EpSqlCacheHelper.GetDataTableFromCache("app");
                dt.DefaultView.RowFilter = " appid= " + lngAppID;
                foreach (DataRowView dvr in dt.DefaultView)
                {
                    appName = dvr["appname"].ToString();
                }
                return appName;
            }
            else
            {
                OracleConnection cn = ConfigTool.GetConnection();
                string strSql = "select appname from es_app where appid =" + lngAppID.ToString();
                try
                {
                    dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        appName = dt.Rows[0]["appname"].ToString();
                    }
                    return appName;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
        }

        #endregion
       
        #region 获取OFlowModelID GetOFlowModelId

        /// <summary>
        ///返回流程oldFlowModelId
        /// </summary>
        /// <param name="lngFlowModelID">flowModelid</param>
        /// <param name="appid">应用id</param>
        /// <param name="flowname">流程名称</param>
        /// <returns>返回流程oldFlowModelId</returns>
        public static long GetOFlowModelId(long lngFlowModelID, ref long lngAppID, ref string flowname)
        {
            long lngOFlowModelID = 0;
            DataTable dt;

            //添加SQL缓存依赖的处理方式,减少数据库连接次数

            if (System.Configuration.ConfigurationSettings.AppSettings["SqlCacheModel"] == "1")
            {
                dt = EpSqlCacheHelper.GetDataTableFromCache("flowmodel");
                dt.DefaultView.RowFilter = " flowmodelid= " + lngFlowModelID;

                foreach (DataRowView dvr in dt.DefaultView)
                {
                    lngOFlowModelID = long.Parse(dvr["Oflowmodelid"].ToString());
                    flowname = dvr["flowname"].ToString();
                    lngAppID = long.Parse(dvr["appid"].ToString());
                }
                return lngOFlowModelID;
            }
            else
            {
                OracleConnection cn = ConfigTool.GetConnection();
                string strSql = "select  flowname ,appid ,oflowmodelid from es_flowmodel where flowmodelid=" + lngFlowModelID;
                try
                {
                    dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lngOFlowModelID = long.Parse(dt.Rows[0]["Oflowmodelid"].ToString());
                        flowname = dt.Rows[0]["flowname"].ToString();
                        lngAppID = long.Parse(dt.Rows[0]["appid"].ToString());
                    }
                    return lngOFlowModelID;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    ConfigTool.CloseConnection(cn);
                }
            }
        }

        #endregion

        #region 根据FlowID，人员的接收类型，发送类型,模板类型得到模板

        /// <summary>
        /// 根据FlowID，人员的接收类型，发送类型,模板类型得到模板
        /// </summary>
        /// <param name="lngFlowID">FlowID</param>
        /// <param name="lngSystemID">AppID</param>
        /// <param name="RTypeID">接收类型()</param>
        /// <param name="STypeID">发送类型()</param>
        /// <param name="iTemType">模板类型（1为邮邮，2为短信）</param>
        /// <returns></returns>
        public static string[] GetTemContent(OracleTransaction trans,long lngFlowID, long RTypeID, long STypeID)
        {

            string[] strTempContent = { "0", "", "0", "", "", "","" };//应用ID，诮用名称，规则ID，规则名称，邮个TITLE，邮件模板，短信模板
            string stFlowModelID = "0";
            DataTable dt;
            DataTable dtInfo;
            string strSql2;
            string strSql = "";
            strSql = "select a.appid,a.appname from es_flow f,es_app a where f.appid=a.appid and f.flowid=" + lngFlowID.ToString();
            if (strSql != string.Empty)
            {
                dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSql).Tables[0];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        strTempContent[0] = dt.Rows[0]["appid"].ToString();
                        strTempContent[1] = dt.Rows[0]["appname"].ToString();
                    }
                }
            }
            strSql = "";
            strSql2 = "";
            switch (long.Parse(strTempContent[0]))
            {
                case 1028://发布管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 201://自定义表单流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 400://知识管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 199://通用流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 210://问题管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 410://资产巡检
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 420://变更管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                case 1026://事件管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Cst_Issues where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select nvl(a.buildCode,'') || nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid and a.flowid=" + lngFlowID.ToString();
                    break;
                default:
                    break;
            }

            if (strSql != string.Empty)
            {
                dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSql).Tables[0];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        stFlowModelID = dt.Rows[0]["Oflowmodelid"].ToString();
                    }
                }
            }
            dtInfo = null;
            if (strSql2 != string.Empty)
            {
                dtInfo = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSql2).Tables[0];
            }
            strSql = "SELECT R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.modelcontent  FROM mailandmessagetemplate T,mailandmessagerule R " +
                     "WHERE T.ID=R.TEMPLATEID AND T.STATUS=1 AND R.DELETED=0 AND R.STATUS=1 AND " +
                     "R.SYSTEMID= " + strTempContent[0] + " AND R.MODELID= " + stFlowModelID + " AND " +
                     "R.receiverstypeid=" + RTypeID.ToString() + " AND sendertypeid=" + STypeID.ToString();

            dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, strSql).Tables[0];

            if (dt != null && dtInfo != null)
            {
                if (dt.Rows.Count > 0 && dtInfo.Rows.Count > 0)
                {
                    strTempContent[2] = dt.Rows[0]["ID"].ToString();
                    strTempContent[3] = dt.Rows[0]["TEMPLATENAME"].ToString();
                    strTempContent[4] = dt.Rows[0]["title"].ToString();
                    strTempContent[5] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["mailcontent"].ToString(),dtInfo);
                    strTempContent[6] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["modelcontent"].ToString(), dtInfo);
                }
            }

            return strTempContent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public static string[] GetTemContentbyID(long lngFlowID, long rid)
        {

            string[] strTempContent = { "0", "", "0", "", "", "", "" };//应用ID，诮用名称，规则ID，规则名称，邮个TITLE，邮件模板，短信模板
            string stFlowModelID = "0";
            OracleConnection cn;
            DataTable dt;
            DataTable dtInfo;
            string strSql2;
            string strSql = "";
            strSql = "select a.appid,a.appname from es_flow f,es_app a where f.appid=a.appid and f.flowid=" + lngFlowID.ToString();
            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
                ConfigTool.CloseConnection(cn);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        strTempContent[0] = dt.Rows[0]["appid"].ToString();
                        strTempContent[1] = dt.Rows[0]["appname"].ToString();
                    }
                }
            }
            strSql = "";
            strSql2 = "";
            switch (long.Parse(strTempContent[0]))
            {
                case 1028://发布管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from OA_RELEASEMANAGEMENT where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from OA_RELEASEMANAGEMENT a,es_flow f where f.flowid=a.flowid";
                    break;
                case 201://自定义表单流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_DefineData where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_DefineData a,es_flow f where f.flowid=a.flowid";
                    break;
                case 400://知识管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Inf_KMBase where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Inf_KMBase a,es_flow f where f.flowid=a.flowid";
                    break;
                case 199://通用流程
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from App_pub_Normal_Head where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from App_pub_Normal_Head a,es_flow f where f.flowid=a.flowid";
                    break;
                case 210://问题管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Pro_ProblemDeal where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Pro_ProblemDeal a,es_flow f where f.flowid=a.flowid";
                    break;
                case 410://资产巡检
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_PatrolService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select '' servicno,f.subject from Equ_PatrolService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 420://变更管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Equ_ChangeService where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select Changeno as servicno,f.subject from Equ_ChangeService a,es_flow f where f.flowid=a.flowid";
                    break;
                case 1026://事件管理
                    strSql = "select Oflowmodelid from es_flowmodel where AppID=" + strTempContent[0] + " AND flowmodelid=(select flowmodelid from Cst_Issues where flowid=" + lngFlowID.ToString() + ")";
                    strSql2 = "select nvl(a.buildCode,'')||nvl(a.ServiceNo,'') as servicno,f.subject from Cst_Issues a,es_flow f where f.flowid=a.flowid";
                    break;
                default:
                    break;
            }

            if (strSql != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
                ConfigTool.CloseConnection(cn);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        stFlowModelID = dt.Rows[0]["Oflowmodelid"].ToString();
                    }
                }
            }
            dtInfo = null;
            if (strSql2 != string.Empty)
            {
                cn = ConfigTool.GetConnection("SQLConnString");
                dtInfo = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql2);
                ConfigTool.CloseConnection(cn);
            }
            strSql = "SELECT R.ID,R.TEMPLATENAME,R.mailtitle as title,T.mailcontent,T.modelcontent  FROM mailandmessagetemplate T,mailandmessagerule R " +
                     "WHERE T.ID=R.TEMPLATEID AND T.STATUS=0 AND R.DELETED=0 AND R.STATUS=0 AND R.ID=" + rid.ToString();

            cn = ConfigTool.GetConnection("SQLConnString");
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            ConfigTool.CloseConnection(cn);
            if (dt != null && dtInfo != null)
            {
                if (dt.Rows.Count > 0 && dtInfo.Rows.Count > 0)
                {
                    strTempContent[2] = dt.Rows[0]["ID"].ToString();
                    strTempContent[3] = dt.Rows[0]["TEMPLATENAME"].ToString();
                    strTempContent[4] = dt.Rows[0]["title"].ToString();
                    strTempContent[5] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["mailcontent"].ToString(), dtInfo);
                    strTempContent[6] = MailBodyDeal.GetMailBody_ByString(dt.Rows[0]["modelcontent"].ToString(), dtInfo);
                }
            }

            return strTempContent;
        }

        #endregion

        #region GetAllFlowModel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stWhere"></param>
        /// <returns></returns>
        public static DataTable getAllFlowModel(string stWhere)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            strSQL = "SELECT oflowmodelid,flowname FROM es_flowmodel WHERE 1=1  and  deleted=0 and Status = 1 " + stWhere.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion

        #region  获取邮件规则设置
        public DataTable GetMessageRulInstall(long lngAppId, long lngFlowModelId)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "select * from BR_MessageRulInstall where OFlowModelID= (select OFlowModelid from es_flowmodel where appid=" + lngAppId
                + " and flowmodelid=" + lngFlowModelId + ")";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 根据应用ID和流程模型ID取环节名称 (除去开始和结束环节)

        /// <summary>
        /// 根据应用ID和流程模型ID取环节名称 (除去开始和结束环节)
        /// </summary>
        /// <param name="lngAppID"></param>
        /// <param name="lngFlowModelID"></param>
        /// <returns></returns>
        public DataTable GetNodeName(long lngAppID, long lngFlowModelID)
        {
            DataTable dt = new DataTable();
            OracleConnection cn = ConfigTool.GetConnection();
            string strSql = "select * from es_nodemodel where FLowMOdelID = (select max(FlowModelID) from es_flowmodel where appid="
                + lngAppID + " and OFlowModelID =" + lngFlowModelID + " and type<>10 and type<>60)";
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSql);
            ConfigTool.CloseConnection(cn);

            return dt;


        }
        #endregion


        #region 查询邮件规则
        /// <summary>
        /// 根据模型ID，查询邮件规则
        /// </summary>
        /// <param name="oflowmodeid"></param>
        /// <returns></returns>
        public DataTable GetMessageRulInstall(string oflowmodeid)
        {
            DataTable dt_message = new DataTable();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string seachmessage = string.Format("select * from BR_MessageRulInstall where oflowmodelid='{0}'",oflowmodeid);
                dt_message = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, seachmessage);
                return dt_message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
    }
}


