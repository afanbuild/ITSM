using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.SqlDAL.MailAndMessageRule
{
    public class BR_MessageRulInstallDP
    {
        /// <summary>
        /// 
        /// </summary>
        public BR_MessageRulInstallDP()
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

        #region NodeID
        /// <summary>
        ///
        /// </summary>
        private Decimal mNodeID;
        public Decimal NodeID
        {
            get { return mNodeID; }
            set { mNodeID = value; }
        }
        #endregion

        #region NodeName
        /// <summary>
        ///
        /// </summary>
        private String mNodeName = string.Empty;
        public String NodeName
        {
            get { return mNodeName; }
            set { mNodeName = value; }
        }
        #endregion

        #region NodeContent
        /// <summary>
        ///
        /// </summary>
        private String mNodeContent = string.Empty;
        public String NodeContent
        {
            get { return mNodeContent; }
            set { mNodeContent = value; }
        }
        #endregion

        #region FlowNameID
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowNameID;
        public Decimal FlowNameID
        {
            get { return mFlowNameID; }
            set { mFlowNameID = value; }
        }
        #endregion

        #region FlowName
        /// <summary>
        ///
        /// </summary>
        private String mFlowName = string.Empty;
        public String FlowName
        {
            get { return mFlowName; }
            set { mFlowName = value; }
        }
        #endregion

        #region ReceiverStypeName
        /// <summary>
        ///
        /// </summary>
        private String mReceiverStypeName = string.Empty;
        public String ReceiverStypeName
        {
            get { return mReceiverStypeName; }
            set { mReceiverStypeName = value; }
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

        #endregion

        #region  保存配置的邮件规则


        public void InsertBRMessageTall(BR_MessageRulInstallDP br_messageRulInstalldp)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("BR_MessageRulInstall_ID").ToString();

                strSQL = @"INSERT INTO BR_MessageRulInstall(
                                    ID,
                                    OFlowModelID,
                                    NodeId,
                                    NodeName,
                                    NodeContent,
                                    FlowNameID,
                                    FlowName,
                                    ReceiverStypeName,
                                    TRIGGER_TYPE,
                                    INTERVAL_TIME,
                                    RECIPIENT_USERID,
                                    RECIPIENT_USER
                                    
					)
					VALUES( " +
                            strID + "," +
                            br_messageRulInstalldp.OFlowModelID.ToString() + "," +
                            br_messageRulInstalldp.NodeID.ToString() + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.NodeName) + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.NodeContent) + "," +
                            br_messageRulInstalldp.FlowNameID.ToString() + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.FlowName) + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.ReceiverStypeName) + ","+
                            StringTool.SqlQ(br_messageRulInstalldp.Trigger_Type) + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.Interval_time) + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.Recipient_UserID) + "," +
                            StringTool.SqlQ(br_messageRulInstalldp.Recipient_User) + ")";

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
        public void DeletedBRMessageTall(long oflowmodel)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                string strSQL = "delete from BR_MessageRulInstall where OFlowModelID=" + oflowmodel;
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

        /// <summary>
        /// 判断某个环节下是否已经存在规则
        /// </summary>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static bool IsExistsByFlowNode(string sWhere)
        {
            bool result = false;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Br_Messagerulinstall where 1=1 ";
            strSQL += sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }
        #endregion
    }
}
