using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using System.Data;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    public class MailMessageTemManagerDP
    {
        /// <summary>
        /// 构造函数

        /// </summary>
        public MailMessageTemManagerDP()
        { }

        #region Property

        #region ID
        /// <summary>
        /// ID
        /// </summary>
        private decimal mID;
        public decimal ID
        {
            get { return mID; }
            set { mID = value; }
        }
        #endregion

        #region TemplateName

        private string mTemplateName;
        public string TemplateName
        {
            set { mTemplateName = value; }
            get { return mTemplateName; }
        }

        #endregion

        #region MailContent

        private string mMailContent;
        public string MailContent
        {
            set { mMailContent = value; }
            get { return mMailContent; }
        }

        #endregion

        #region LeaderContent
        private string mleadercontent;
        public string leadercontent
        {
            set { mleadercontent = value; }
            get { return mleadercontent; }
        }
        #endregion

        #region DealMainContent 上报人邮件模板内容


        private string mDealMainContent;
        public string DealMainContent
        {
            set { mDealMainContent = value; }
            get { return mDealMainContent; }
        }
        #endregion

        #region ModelContent

        private string mModelContent;
        public string ModelContent
        {
            set { mModelContent = value; }
            get { return mModelContent; }
        }

        #endregion

        #region Status

        private decimal mStatus;
        public decimal Status
        {
            set { mStatus = value; }
            get { return mStatus; }
        }

        #endregion

        #region RegTime

        private DateTime mRegTime;
        public DateTime RegTime
        {
            set { mRegTime = value; }
            get { return mRegTime; }
        }

        #endregion

        #region FlowModelID

        private String mFlowModelID;
        public String FlowModelID
        {
            set { mFlowModelID = value; }
            get { return mFlowModelID; }
        }

        #endregion

        #region RegUserId

        private decimal mRegUserId;
        public decimal RegUserId
        {
            set { mRegUserId = value; }
            get { return mRegUserId; }
        }

        #endregion

        #region RegUserName

        private string mRegUserName;
        public string RegUserName
        {
            set { mRegUserName = value; }
            get { return mRegUserName; }
        }

        #endregion

        #region RegDeptId

        private decimal mRegDeptId;
        public decimal RegDeptId
        {
            set { mRegDeptId = value; }
            get { return mRegDeptId; }
        }

        #endregion

        #region RegDeptName

        private string mRegDeptName;
        public string RegDeptName
        {
            set { mRegDeptName = value; }
            get { return mRegDeptName; }
        }

        #endregion

        #region Remark

        private string mRemark;
        public string Remark
        {
            set { mRemark = value; }
            get { return mRemark; }
        }

        #endregion

        #region SystemID
        /// <summary>
        ///  应用ID
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
        /// 应用名称
        /// </summary>
        private String mSystemName = string.Empty;
        public String SystemName
        {
            get { return mSystemName; }
            set { mSystemName = value; }
        }
        #endregion

        #endregion

        #region GetReCorded

        /// <summary>
        /// 根据ID获取短信模板信息
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public MailMessageTemManagerDP GetReCorded(long lngID)
        {
            MailMessageTemManagerDP ee = new MailMessageTemManagerDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM MailAndMessageTemplate WHERE ID=" + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow row in dt.Rows)
            {
                ee.ID = Decimal.Parse(row["ID"].ToString());
                ee.TemplateName = row["TemplateName"].ToString();
                ee.MailContent = row["MailContent"].ToString();
                ee.DealMainContent = row["DealMailContent"].ToString();
                ee.leadercontent = row["leadercontent"].ToString();
                ee.ModelContent = row["ModelContent"].ToString();
                ee.Status = Decimal.Parse(row["Status"].ToString());
                ee.RegTime = DateTime.Parse(row["RegTime"].ToString());
                ee.RegUserId = Decimal.Parse(row["RegUserId"].ToString());
                ee.RegUserName = row["RegUserName"].ToString();
                ee.RegDeptId = Decimal.Parse(row["RegDeptId"].ToString());
                ee.RegDeptName = row["RegDeptName"].ToString();
                ee.Remark = row["Remark"].ToString();

                ee.SystemID = Decimal.Parse(row["SystemID"].ToString() == "" ? "0" : row["SystemID"].ToString());
                ee.SystemName = row["SystemName"].ToString();
            }

            return ee;
        }

        #endregion

        #region GetDataTable

        /// <summary>
        /// 根据条件获取短信模板信息
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM MailAndMessageTemplate ";
            strSQL += "WHERE " + sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "MailAndMessageTemplate", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #endregion

        #region InsertRecorded

        /// <summary>
        /// 添加短信模板信息
        /// </summary>
        /// <param name="pCst_MailMessageTemManagerDP"></param>
        public string InsertRecorded(MailMessageTemManagerDP pCst_MailMessageTemManagerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction trans = cn.BeginTransaction();
            StringBuilder sbSQL = new StringBuilder();
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("MailAndMessageRuleID").ToString();
                pCst_MailMessageTemManagerDP.ID = decimal.Parse(strID);
                if (CheckRecorded(pCst_MailMessageTemManagerDP))
                {
                    #region 构建SQL语句

                    sbSQL.Append("INSERT INTO MailAndMessageTemplate (");
                    sbSQL.Append(@"ID,TemplateName,MailContent,ModelContent,Status,RegTime,RegUserId,
                                   RegUserName,RegDeptId,RegDeptName,Remark,SystemID,SystemName,DealMailContent,leadercontent, flowmodelid)");
                    sbSQL.Append("VALUES(");
                    sbSQL.Append(strID.ToString() + ",");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.TemplateName) + ",");
                    sbSQL.Append(":MailContent,");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.ModelContent) + ",");
                    sbSQL.Append(pCst_MailMessageTemManagerDP.Status.ToString() + ",");
                    sbSQL.Append("to_date(" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss'),");
                    sbSQL.Append(pCst_MailMessageTemManagerDP.RegUserId.ToString() + ",");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegUserName) + ",");
                    sbSQL.Append(pCst_MailMessageTemManagerDP.RegDeptId.ToString() + ",");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegDeptName) + ",");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.Remark) + ",");
                    sbSQL.Append(pCst_MailMessageTemManagerDP.SystemID.ToString() + ",");
                    sbSQL.Append(StringTool.SqlQ(pCst_MailMessageTemManagerDP.SystemName) + ",");
                    sbSQL.Append(":DealMailContent,");
                    sbSQL.Append(":leadercontent,");
                    sbSQL.AppendFormat("{0})", StringTool.SqlQ(pCst_MailMessageTemManagerDP.FlowModelID));

                    #endregion

                    #region 构建参数 MailContent

                    OracleCommand cmdDesk = new OracleCommand(sbSQL.ToString(), trans.Connection, trans);
                    cmdDesk.Parameters.Add("MailContent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.MailContent;
                    cmdDesk.Parameters.Add("DealMailContent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.DealMainContent;
                    cmdDesk.Parameters.Add("leadercontent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.leadercontent;
                    cmdDesk.ExecuteNonQuery();

                    #endregion
                    //OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sbSQL.ToString());

                    trans.Commit();
                    return "Success";
                }
                return "Error";
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion

        #region UpdateRecorded

        /// <summary>
        /// 更新短信模板信息
        /// </summary>
        /// <param name="pCst_MailMessageTemManagerDP"></param>
        public string UpdateRecorded(MailMessageTemManagerDP pCst_MailMessageTemManagerDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction trans = cn.BeginTransaction();
            StringBuilder sbSQL = new StringBuilder();
            try
            {
                if (CheckRecorded(pCst_MailMessageTemManagerDP))
                {

                    #region 构建SQL语句

                    sbSQL.Append("UPDATE MailAndMessageTemplate SET ");
                    sbSQL.Append("TemplateName=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.TemplateName) + ",");
                    sbSQL.Append("MailContent=:MailContent" + ",");
                    sbSQL.Append("ModelContent=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.ModelContent) + ",");
                    sbSQL.Append("Status=" + pCst_MailMessageTemManagerDP.Status.ToString() + ",");
                    sbSQL.Append("RegTime=to_date(" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegTime.ToString()) + ",'yyyy-MM-dd HH24:mi:ss'),");
                    sbSQL.Append("RegUserId=" + pCst_MailMessageTemManagerDP.RegUserId.ToString() + ",");
                    sbSQL.Append("RegUserName=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegUserName) + ",");
                    sbSQL.Append("RegDeptId=" + pCst_MailMessageTemManagerDP.RegDeptId.ToString() + ",");
                    sbSQL.Append("RegDeptName=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.RegDeptName) + ",");
                    sbSQL.Append("Remark=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.Remark) + ",");
                    sbSQL.Append("SystemID=" + pCst_MailMessageTemManagerDP.SystemID.ToString() + ",");
                    sbSQL.Append("SystemName=" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.SystemName) + ", ");
                    sbSQL.Append("DealMailContent=:DealMailContent" + ",");
                    sbSQL.Append("leadercontent=:leadercontent,");
                    sbSQL.AppendFormat("flowmodelid = {0}", StringTool.SqlQ(pCst_MailMessageTemManagerDP.FlowModelID));

                    sbSQL.Append(" WHERE ID=" + pCst_MailMessageTemManagerDP.ID.ToString() + "");

                    #endregion

                    #region 构建参数 MailContent
                    OracleCommand cmdDesk = new OracleCommand(sbSQL.ToString(), trans.Connection, trans);
                    cmdDesk.Parameters.Add("MailContent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.MailContent;
                    cmdDesk.Parameters.Add("DealMailContent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.DealMainContent;
                    cmdDesk.Parameters.Add("leadercontent", OracleType.Clob).Value = pCst_MailMessageTemManagerDP.leadercontent;
                    cmdDesk.ExecuteNonQuery();

                    #endregion

                    trans.Commit();
                    return "Success";
                }
                return "Error";
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }


        #endregion

        /// <summary>
        /// 判断模板是否在规则中用到
        /// </summary>
        /// <param name="tempLateId">模板id</param>
        /// <returns>返回模板名称</returns>
        public string selectMailAndMessage(long tempLateId)
        {
            string StrSQL = @" select b.templatename from mailandmessagerule t 
                left join mailandmessagetemplate b 
                on t.templateid=b.id
                where  t.templateid= " + tempLateId.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            string templatename = "";
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, StrSQL);
                if (dt.Rows.Count > 0)
                {
                    templatename = dt.Rows[0]["templatename"].ToString();
                }
            }
            catch
            {

            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return templatename;
        }


        #region DeleteRecorded

        /// <summary>
        /// 根据ID删除短信模板信息
        /// </summary>
        /// <param name="lngId"></param>
        public void DeleteRecorded(long lngId)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string strSQL = "DELETE MailAndMessageTemplate WHERE ID=" + lngId.ToString();
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
                trans.Dispose();
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion

        #region CheckRecorded

        /// <summary>
        /// 检查内容不能重复

        /// </summary>
        /// <param name="pCst_MailMessageTemManagerDP"></param>
        /// <returns></returns>
        public bool CheckRecorded(MailMessageTemManagerDP pCst_MailMessageTemManagerDP)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM MailAndMessageTemplate " +
                     "WHERE  (MailContent like " + StringTool.SqlQ(pCst_MailMessageTemManagerDP.MailContent) + " " +
                     "OR DealMailContent like" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.DealMainContent) + " " +
                     "OR leadercontent like" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.leadercontent) + " " +
                     "OR ModelContent =" + StringTool.SqlQ(pCst_MailMessageTemManagerDP.ModelContent) + ") " +
                     "AND ID<>" + pCst_MailMessageTemManagerDP.ID;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        #endregion

    }
}
