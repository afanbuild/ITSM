



/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :wangxiuwei
 * Create Date:2010年6月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Xml;
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
    public class Cst_RecommendRuleDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Cst_RecommendRuleDP()
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

        #region Desc
        /// <summary>
        ///
        /// </summary>
        private String mDesc = string.Empty;
        public String Desc
        {
            get { return mDesc; }
            set { mDesc = value; }
        }
        #endregion

        #region Condition
        /// <summary>
        ///
        /// </summary>
        private String mCondition;
        public String Condition
        {
            get { return mCondition; }
            set { mCondition = value; }
        }
        #endregion

        #region Deleted
        /// <summary>
        ///
        /// </summary>
        private Int32 mDeleted;
        public Int32 Deleted
        {
            get { return mDeleted; }
            set { mDeleted = value; }
        }
        #endregion

        #region IsAvail
        /// <summary>
        ///
        /// </summary>
        private Int32 mIsAvail;
        public Int32 IsAvail
        {
            get { return mIsAvail; }
            set { mIsAvail = value; }
        }
        #endregion

        #region LastUpdate
        /// <summary>
        ///
        /// </summary>
        private DateTime mLastUpdate = DateTime.MinValue;
        public DateTime LastUpdate
        {
            get { return mLastUpdate; }
            set { mLastUpdate = value; }
        }
        #endregion

        #region updateUserID
        /// <summary>
        ///
        /// </summary>
        private Decimal mupdateUserID;
        public Decimal updateUserID
        {
            get { return mupdateUserID; }
            set { mupdateUserID = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Cst_RecommendRuleDP</returns>
        public Cst_RecommendRuleDP GetReCorded(long lngID)
        {
            Cst_RecommendRuleDP ee = new Cst_RecommendRuleDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Cst_RecommendRule WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.RuleName = dr["RuleName"].ToString();
                ee.Desc = dr["DESCRIPT"].ToString();
                ee.Condition = dr["Condition"].ToString();
                ee.Deleted = Int32.Parse(dr["Deleted"].ToString());
                ee.IsAvail = Int32.Parse(dr["IsAvail"].ToString());
                ee.LastUpdate = dr["LastUpdate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LastUpdate"].ToString());
                ee.updateUserID = Decimal.Parse(dr["updateUserID"].ToString());
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
            strSQL = "SELECT * FROM Cst_RecommendRule Where 1=1 And Deleted=0 ";
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
        /// <param name=pCst_RecommendRuleDP></param>
        public void InsertRecorded(Cst_RecommendRuleDP pCst_RecommendRuleDP, DataTable dt)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_RecommendRuleID").ToString();
                pCst_RecommendRuleDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Cst_RecommendRule(
									ID,
									RuleName,
									DESCRIPT,
									Condition,
									Deleted,
									IsAvail,
									LastUpdate,
									updateUserID
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.RuleName) + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.Desc) + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.Condition) + "," +
                            pCst_RecommendRuleDP.Deleted.ToString() + "," +
                            pCst_RecommendRuleDP.IsAvail.ToString() + "," +
                            (pCst_RecommendRuleDP.LastUpdate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_RecommendRuleDP.LastUpdate.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            pCst_RecommendRuleDP.updateUserID.ToString() +
                    ")";

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
                UpdateDetails(long.Parse(strID), dt, tran);
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

        public void InsertRecorded(Cst_RecommendRuleDP pCst_RecommendRuleDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strID = EpowerGlobal.EPGlobal.GetNextID("Cst_RecommendRuleID").ToString();
                pCst_RecommendRuleDP.ID = decimal.Parse(strID);
                strSQL = @"INSERT INTO Cst_RecommendRule(
									ID,
									RuleName,
									DESCRIPT,
									Condition,
									Deleted,
									IsAvail,
									LastUpdate,
									updateUserID
					)
					VALUES( " +
                            strID.ToString() + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.RuleName) + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.Desc) + "," +
                            StringTool.SqlQ(pCst_RecommendRuleDP.Condition) + "," +
                            pCst_RecommendRuleDP.Deleted.ToString() + "," +
                            pCst_RecommendRuleDP.IsAvail.ToString() + "," +
                            (pCst_RecommendRuleDP.LastUpdate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_RecommendRuleDP.LastUpdate.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            pCst_RecommendRuleDP.updateUserID.ToString() +
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
        public void UpdateRecorded(Cst_RecommendRuleDP pCst_RecommendRuleDP,DataTable dt)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            OracleTransaction tran = cn.BeginTransaction();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Cst_RecommendRule Set " +
                                                        " RuleName = " + StringTool.SqlQ(pCst_RecommendRuleDP.RuleName) + "," +
                            " DESCRIPT = " + StringTool.SqlQ(pCst_RecommendRuleDP.Desc) + "," +
                            " Condition = " + StringTool.SqlQ(pCst_RecommendRuleDP.Condition) + "," +
                            " Deleted = " + pCst_RecommendRuleDP.Deleted.ToString() + "," +
                            " IsAvail = " + pCst_RecommendRuleDP.IsAvail.ToString() + "," +
                            " LastUpdate = " + (pCst_RecommendRuleDP.LastUpdate == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pCst_RecommendRuleDP.LastUpdate.ToString())) + ",'yyyy-MM-dd HH24:mi:ss')," +
                            " updateUserID = " + pCst_RecommendRuleDP.updateUserID.ToString() +
                                " WHERE ID = " + pCst_RecommendRuleDP.ID.ToString();

                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, strSQL);
               
                    UpdateDetails(long.Parse(pCst_RecommendRuleDP.ID.ToString()), dt, tran);
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
            try
            {
                string strSQL = "Update Cst_RecommendRule Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region Details

       /// <summary>
        /// 得到相关规则的工程师信息列表
       /// </summary>
       /// <param name="lngID"></param>
       /// <returns></returns>
        public static DataTable getDetails(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = "SELECT Cst_RecommendRuleDetails.RuleID,Cst_ServiceStaff.Name, Cst_RecommendRuleDetails.StaffID, Cst_ServiceStaff.BlongDeptName " +
                            "FROM Cst_ServiceStaff INNER JOIN Cst_RecommendRuleDetails ON Cst_ServiceStaff.ID = Cst_RecommendRuleDetails.StaffID" +
                            " WHERE Cst_ServiceStaff.Deleted=0 AND Cst_RecommendRuleDetails.RuleID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 更新相关规则的工程师
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static void UpdateDetails(long lngID, DataTable dt, OracleTransaction tran)
        {
            string strsql = "DELETE Cst_RecommendRuleDetails WHERE RuleID = " + lngID.ToString();
            OracleDbHelper.ExecuteScalar(tran, CommandType.Text, strsql);
            foreach (DataRow dr in dt.Rows)
            {
                strsql = " INSERT INTO Cst_RecommendRuleDetails(RuleID,StaffID) VALUES(" + lngID.ToString() + "," + dr["StaffID"].ToString() + ") ";
                OracleDbHelper.ExecuteScalar(tran, CommandType.Text, strsql);
            }
            
        }

        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            string strWhere = "1=1 And Deleted=0" + sWhere;
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "Cst_RecommendRule", "*", sOrder, pagesize, pageindex, strWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        private DataTable GetUserInfo(long lngRUID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, "SELECT * FROM Cst_RecommendRuleDetails WHERE RuleID="+lngRUID.ToString());
            ConfigTool.CloseConnection(cn);
            return dt;
        }


        public  DataTable getUserProject(long CustID, string EquID, long ServiceTypeID, long ServiceLevelID,string stCustName,string stMasterName)
        {
            DataTable dt=null;
            string ruleId = GetRUPersonList(CustID, EquID, ServiceTypeID, ServiceLevelID,stCustName,stMasterName);
            if (ruleId != "")
            {
                string SqlStr = @"SELECT Cst_ServiceStaff.Userid as userid,Cst_ServiceStaff.UserName as username FROM Cst_RecommendRuleDetails
                    left join 
                    Cst_ServiceStaff on Cst_RecommendRuleDetails.staffid=Cst_ServiceStaff.Id

                    where Cst_RecommendRuleDetails.ruleId in (" + ruleId + ")";

                OracleConnection cn = ConfigTool.GetConnection();
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SqlStr);
                ConfigTool.CloseConnection(cn);

            }
            return dt;

        }

        /// <summary>
        /// 得到相关事件单的推荐人员列表
        /// </summary>
        /// <param name="CustID">客户ID</param>
        /// <param name="EquID">资产名称</param>
        /// <param name="ServiceTypeID">事件类别ID</param>
        /// <param name="ServiceLevelID">服务级别ID</param>
        /// <param name="CustName">用户名称</param>
        /// <param name="MasterName">服务单位</param>
        /// <returns></returns>
        public string GetRUPersonList(long CustID, string EquID, long ServiceTypeID, long ServiceLevelID,string CustName,string MasterName)
        {
            DataTable dtCustAll = Br_MastCustomerDP.GetIssueByCust(CustID);//客户的所有服务;
            DataTable dtCustAllByName = Br_MastCustomerDP.GetIssueByCustName(CustName);//客户的所有服务;
            long uniontID = 0;
            if (dtCustAll.Rows.Count > 0)
            {
                uniontID = long.Parse(dtCustAll.Select("")[0]["MastCustID"].ToString());
            }
            DataTable dtUAll = Br_MastCustomerDP.GetIssueByMaster(uniontID);//单位的所有服务;
            DataTable dtUAllByMasterName = Br_MastCustomerDP.GetIssueByMasterName(MasterName);//单位的所有服务;
            DataTable dtRU = GetDataTable(" AND IsAvail=0 AND Condition LIKE '%Conditions%'", "");//规则列表
            string stRuID = "0";//规则ID列表      

            foreach (DataRow dr in dtRU.Rows)
            {
                DataTable dtUserList = GetUserInfo(long.Parse(dr["ID"].ToString()));
                string xmlCondition = dr["Condition"].ToString();//规则XML串
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlCondition);
                XmlNodeList relnodes = xmldoc.SelectNodes("Conditions/Condition");//规则计录列表
                string[] ArrRelation = new string[relnodes.Count];
                string[] ArrEq = new string[relnodes.Count];
                int i = 0;
                foreach (XmlNode xnode in relnodes)
                {
                    string stExpression = xnode.Attributes["Expression"].Value;
                    string stExpressionID = xnode.Attributes["Tag"].Value;
                    ArrRelation[i] = xnode.Attributes["Relation"].Value;
                    ArrEq[i] = "";
                    switch (xnode.Attributes["CondType"].Value)//比较对象
                    {
                        case "10,INTER"://单位.服务次数
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length == int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length == int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "1"://不等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length != int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length != int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "2"://大于dtUAllByMasterName
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length > int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length > int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "3"://大于等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length >= int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length >= int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "4"://小于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length < int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length < int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "5"://小于等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtUAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length <= int.Parse(stExpression.ToString()))||(dtUAllByMasterName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length <= int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "6"://包含
                                    break;
                                case "7"://不包含
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "11,CHAR"://单位.名称
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    if (MasterName.Trim() == stExpression.Trim())
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "1"://不等于
                                    if (MasterName.Trim() != stExpression.Trim())
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "2"://大于
                                    if (stExpression.Trim().Length > 0 && MasterName.Trim().Length >= stExpression.Trim().Length)
                                    {
                                        if (MasterName.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "3"://大于等于
                                    if (stExpression.Trim().Length > 0 && MasterName.Trim().Length >= stExpression.Trim().Length)
                                    {
                                        if (MasterName.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "4"://小于
                                    break;
                                case "5"://小于等于
                                    break;
                                case "6"://包含
                                    if (MasterName.IndexOf(stExpression) != -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "7"://不包含
                                    if (MasterName != stExpression)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "12,CATA"://事件类别
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    if (ServiceTypeID.ToString() == stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "1"://不等于
                                    if (ServiceTypeID.ToString() != stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "2"://大于
                                    break;
                                case "3"://大于等于
                                    break;
                                case "4"://小于
                                    break;
                                case "5"://小于等于
                                    break;
                                case "6"://属于 
                                    string stEfull = CatalogDP.GetCatalogFullID(long.Parse(stExpressionID));
                                    string stFull = CatalogDP.GetCatalogFullID(ServiceTypeID);
                                    if (stFull.IndexOf(stEfull) != -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    //foreach (DataRow drCata in dt.Rows)
                                    //{
                                    //    if (drCata["CatalogID"].ToString() == ServiceTypeID.ToString())
                                    //    {
                                    //        ArrEq[i] = "1 = 1";
                                    //        break;
                                    //    }
                                    //}
                                    break;
                                case "7"://不属于
                                    stEfull = CatalogDP.GetCatalogFullID(long.Parse(stExpressionID));
                                    stFull = CatalogDP.GetCatalogFullID(ServiceTypeID);
                                    if (stFull.IndexOf(stEfull) == -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "13,CATA"://服务级别
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    if (ServiceLevelID.ToString() == stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "1"://不等于
                                    if (ServiceLevelID.ToString() != stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "2"://大于
                                    break;
                                case "3"://大于等于
                                    if (ServiceLevelID.ToString() == stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "4"://小于
                                    break;
                                case "5"://小于等于
                                    break;
                                case "6"://包含
                                    if (ServiceLevelID.ToString() == stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "7"://不包含
                                    if (ServiceLevelID.ToString() != stExpressionID)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "14,CHAR"://资产名称
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    if (EquID.ToString() == stExpression)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "1"://不等于
                                    if (EquID.ToString() != stExpression)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "2"://大于
                                    if (stExpression.Trim().Length > 0 && EquID.Trim().Length >= stExpression.Trim().Length)
                                    {
                                        if (EquID.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "3"://大于等于
                                    if (stExpression.Trim().Length > 0 && EquID.Trim().Length >= stExpression.Trim().Length)
                                    {
                                        if (EquID.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "4"://小于
                                    break;
                                case "5"://小于等于
                                    break;
                                case "6"://包含
                                    if (EquID.IndexOf(stExpression) != -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "7"://不包含
                                    if (EquID.IndexOf(stExpression) == -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "15,INTER"://用户服务次数
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length == int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length == int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "1"://不等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length != int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length != int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "2"://大于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length > int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length > int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "3"://大于等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length >= int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length >= int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "4"://小于dtCustAllByName
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length < int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length < int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "5"://小于等于
                                    foreach (DataRow ddr in dtUserList.Rows)
                                    {
                                        if ((dtCustAll.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length <= int.Parse(stExpression.ToString()))||(dtCustAllByName.Select("SjwxrID='" + ddr["StaffID"].ToString() + "'").Length <= int.Parse(stExpression.ToString())))
                                        {
                                            ArrEq[i] = "1 = 1";
                                            break;
                                        }
                                    }
                                    break;
                                case "6"://包含
                                    break;
                                case "7"://不包含
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case "16,CHAR"://用户.名称
                            switch (xnode.Attributes["Operate"].Value)//操作符
                            {
                                case "0"://等于
                                    if (CustName == stExpression)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "1"://不等于

                                    if (CustName != stExpression)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "2"://大于
                                    if (stExpression.Trim().Length > 0 && CustName.Trim().Length >= stExpression.Trim().Length)
                                    {
                                        if (CustName.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "3"://大于等于
                                    if (stExpression.Trim().Length > 0 && CustName.Trim().Length>=stExpression.Trim().Length)
                                    {
                                        if (CustName.Substring(0, stExpression.Trim().Length).Trim() == stExpression.Trim())
                                        {
                                            ArrEq[i] = "1 = 1";
                                        }
                                    }
                                    break;
                                case "4"://小于
                                    break;
                                case "5"://小于等于
                                    break;
                                case "6"://包含
                                    if (CustName.IndexOf(stExpression) != -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                case "7"://不包含

                                    if (CustName.IndexOf(stExpression) == -1)
                                    {
                                        ArrEq[i] = "1 = 1";
                                    }
                                    break;
                                default:
                                    break;

                            }
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                string stWhere = "(1=1)";
                for (int j = 0; j < relnodes.Count; j++)
                {
                    if (j != 0)
                    {
                        if (ArrRelation[j] == "0")
                        {
                            if (stWhere != "")
                            {
                                stWhere += ArrEq[j] == "" ? " AND (1=2)" : " AND (" + ArrEq[j] + ")";
                            }
                            else
                            {
                                stWhere = ArrEq[j] == "" ? "(1=2)" : " (" + ArrEq[j] + ")";
                            }
                        }
                        else
                        {
                            if (stWhere != "")
                            {
                                stWhere += ArrEq[j] == "" ? " OR(1=2)" : " OR (" + ArrEq[j] + ")";
                            }
                            else
                            {
                                stWhere = ArrEq[j] == "" ? " (1=2)" : " (" + ArrEq[j] + ")";
                            }
                        }
                    }
                    else
                    {
                        stWhere = ArrEq[j] == "" ? "(1=2)" : "(" + ArrEq[j] + ")";
                    }
                }
                if (stWhere != string.Empty)
                {
                    stWhere = "(" + stWhere + ") AND (ID=" + dr["ID"].ToString() + ")";
                    DataRow[] drs = dtRU.Select(stWhere);
                    if (drs.Length > 0)
                    {
                        stRuID += stRuID.Length == 0 ? dr["ID"].ToString() : "," + dr["ID"].ToString();
                    }
                }
                
            }
            return stRuID;
        }

        #endregion
    }
}

