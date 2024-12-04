/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :mczhu
 * Create Date:2009年5月25日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class EA_FlowRelDP
    {
        /// <summary>
        /// 
        /// </summary>
        public EA_FlowRelDP()
        { }

        #region Property
        #region AppID
        /// <summary>
        ///
        /// </summary>
        private Decimal mAppID;
        public Decimal AppID
        {
            get { return mAppID; }
            set { mAppID = value; }
        }
        #endregion

        #region FlowID
        /// <summary>
        ///
        /// </summary>
        private Decimal mFlowID;
        public Decimal FlowID
        {
            get { return mFlowID; }
            set { mFlowID = value; }
        }
        #endregion

        #region RelAppID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelAppID;
        public Decimal RelAppID
        {
            get { return mRelAppID; }
            set { mRelAppID = value; }
        }
        #endregion

        #region RelFlowID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelFlowID;
        public Decimal RelFlowID
        {
            get { return mRelFlowID; }
            set { mRelFlowID = value; }
        }
        #endregion

        #region RelFlowName
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelFlowName;
        public Decimal RelFlowName
        {
            get { return mRelFlowName; }
            set { mRelFlowName = value; }
        }
        #endregion

        #region RelSubject
        /// <summary>
        ///
        /// </summary>
        private String mRelSubject = string.Empty;
        public String RelSubject
        {
            get { return mRelSubject; }
            set { mRelSubject = value; }
        }
        #endregion

        #region RelTime
        /// <summary>
        ///
        /// </summary>
        private DateTime mRelTime = DateTime.MinValue;
        public DateTime RelTime
        {
            get { return mRelTime; }
            set { mRelTime = value; }
        }
        #endregion

        #region RegDate
        /// <summary>
        ///
        /// </summary>
        private DateTime mRegDate = DateTime.MinValue;
        public DateTime RegDate
        {
            get { return mRegDate; }
            set { mRegDate = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>EA_FlowRelDP</returns>
        public EA_FlowRelDP GetReCorded(long lngID)
        {
            EA_FlowRelDP ee = new EA_FlowRelDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                strSQL = "SELECT * FROM EA_FlowRel WHERE ID = " + lngID.ToString();
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                foreach (DataRow dr in dt.Rows)
                {
                    ee.AppID = Decimal.Parse(dr["AppID"].ToString());
                    ee.FlowID = Decimal.Parse(dr["FlowID"].ToString());
                    ee.RelAppID = Decimal.Parse(dr["RelAppID"].ToString());
                    ee.RelFlowID = Decimal.Parse(dr["RelFlowID"].ToString());
                    ee.RelFlowName = Decimal.Parse(dr["RelFlowName"].ToString());
                    ee.RelSubject = dr["RelSubject"].ToString();
                    ee.RelTime = dr["RelTime"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RelTime"].ToString());
                    ee.RegDate = dr["RegDate"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["RegDate"].ToString());
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
                strSQL = "SELECT * FROM EA_FlowRel Where 1=1 ";
                strSQL += sWhere;
                strSQL += sOrder;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEA_FlowRelDP></param>
        public void UpdateRecorded(EA_FlowRelDP pEA_FlowRelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE EA_FlowRel Set " +
                                                        " FlowID = " + pEA_FlowRelDP.FlowID.ToString() + "," +
                            " RelAppID = " + pEA_FlowRelDP.RelAppID.ToString() + "," +
                            " RelFlowID = " + pEA_FlowRelDP.RelFlowID.ToString() + "," +
                            " RelFlowName = " + pEA_FlowRelDP.RelFlowName.ToString() + "," +
                            " RelSubject = " + StringTool.SqlQ(pEA_FlowRelDP.RelSubject) + "," +
                            " RelTime = " + (pEA_FlowRelDP.RelTime == DateTime.MinValue ? " null " : StringTool.SqlQ(pEA_FlowRelDP.RelTime.ToString())) + "," +
                            " RegDate = " + (pEA_FlowRelDP.RegDate == DateTime.MinValue ? " null " : StringTool.SqlQ(pEA_FlowRelDP.RegDate.ToString())) +
                                " WHERE AppID = " + pEA_FlowRelDP.AppID.ToString();

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
        /// <param name="lngAppID"></param>
        /// <param name="lngFlowID"></param>
        public void DeleteRecorded(long lngAppID, long lngFlowID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Delete EA_FlowRel WHERE AppID =" + lngAppID.ToString() + " and FlowID =" + lngFlowID.ToString();
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

        #region GetFieldsTable  选择流程查询
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="sWhere"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetFieldsTable(long lngUserID, long lngDeptID, long lngOrgID, string sWhere, RightEntity re)
        {
            string strList = string.Empty;
            string strSQL = @"select a.FlowID,a.AppID,a.Name,a.Subject,a.StartTime,a.Status,b.OFlowModelID from Es_Flow a,Es_FlowModel b
                            where a.FlowModelID=b.FlowModelID ";
            //strSQL += " AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
            if (re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += sWhere;
                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strSQL += "";
                        break;
                    case eO_RightRange.ePersonal:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                        break;
                    case eO_RightRange.eDeptDirect:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "% ") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "% ") + "))";
                        }
                        break;
                    default:
                        strSQL += "";
                        break;
                }


                #endregion
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region GetFlowModel  选择流程模型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngOFlowModealID"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static DataTable GetFlowModel(long lngOFlowModealID, string sWhere)
        {
            string strList = string.Empty;
            string strSQL = @"select distinct to_char(RelOFlowModelID) ||'|' || to_char(RelOperateID) OFlowModelID,RelFlowName FlowName,RelOperateID from EA_FlowRelConfig Where OFlowModelID= " + lngOFlowModealID.ToString();
            strSQL += sWhere;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion

        #region SavaData  保存数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <param name="lngoFlowModelID"></param>
        /// <returns></returns>
        public static bool SavaData(long lngFlowID, long lngoFlowModelID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            StringBuilder sb = new StringBuilder();
            bool breturn = true;
            string sSessionKey = lngoFlowModelID.ToString() + "EA_FlowRel";
            try
            {
                if (System.Web.HttpContext.Current.Session[sSessionKey] != null
                    && System.Web.HttpContext.Current.Session["EA_FlowRelChange"] != null
                    && System.Web.HttpContext.Current.Session["EA_FlowRelChange"].ToString().ToLower() == "true")
                {
                    DataTable dt = (DataTable)System.Web.HttpContext.Current.Session[sSessionKey];
                    foreach (DataRow dr in dt.Rows)
                    {
                        long lngtemappid = 0;
                        lngtemappid = long.Parse(dr["AppID"].ToString());
                        if (sb.ToString() == string.Empty)
                            sb.Append(" Delete EA_FlowRel WHERE AppID =" + lngtemappid.ToString() + " and FlowID =" + lngFlowID.ToString());
                        sb.Append(@" INSERT INTO EA_FlowRel(
									AppID,
									FlowID,
									RelAppID,
									RelFlowID,
									RelFlowName,
									RelSubject,
									RelTime,
									RegDate
					                )
					                VALUES( " +
                                            lngtemappid.ToString() + "," +
                                            lngFlowID.ToString() + "," +
                                            dr["RelAppID"].ToString() + "," +
                                            dr["RelFlowID"].ToString() + "," +
                                            StringTool.SqlQ(dr["RelFlowName"].ToString()) + "," +
                                            StringTool.SqlQ(dr["RelSubject"].ToString()) + "," +
                                            StringTool.SqlQ(dr["RelTime"].ToString()) + "," +
                                            "sysdate)");
                    }

                }
                if (sb.ToString() != string.Empty)
                {
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sb.ToString());
                }
                return breturn;
            }
            catch
            {
                return false;
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        #endregion
    }
}

