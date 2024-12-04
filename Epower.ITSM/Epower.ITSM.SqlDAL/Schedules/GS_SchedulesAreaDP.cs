


/*******************************************************************
 *
 * Description
 * 
 * 
 * Create By  :yxq
 * Create Date:2012年8月16日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data.OracleClient;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GS_SchedulesAreaDP
    {
        /// <summary>
        /// 
        /// </summary>
        public GS_SchedulesAreaDP()
        { }

        #region Property
        #region AREAID
        private Int64 mAREAID;
        /// <summary>
        ///
        /// </summary>
        public Int64 AREAID
        {
            get { return mAREAID; }
            set { mAREAID = value; }
        }
        #endregion

        #region AREANAME
        private String mAREANAME = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String AREANAME
        {
            get { return mAREANAME; }
            set { mAREANAME = value; }
        }
        #endregion

        #region STARTDATE
        private DateTime mSTARTDATE = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime STARTDATE
        {
            get { return mSTARTDATE; }
            set { mSTARTDATE = value; }
        }
        #endregion

        #region ENDDATE
        private DateTime mENDDATE = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime ENDDATE
        {
            get { return mENDDATE; }
            set { mENDDATE = value; }
        }
        #endregion

        #region STATUS
        private Int64 mSTATUS;
        /// <summary>
        ///
        /// </summary>
        public Int64 STATUS
        {
            get { return mSTATUS; }
            set { mSTATUS = value; }
        }
        #endregion

        #region AREAFLAG
        private Int64 mAREAFLAG;
        /// <summary>
        ///
        /// </summary>
        public Int64 AREAFLAG
        {
            get { return mAREAFLAG; }
            set { mAREAFLAG = value; }
        }
        #endregion

        #region DELETED
        private Int64 mDELETED;
        /// <summary>
        ///
        /// </summary>
        public Int64 DELETED
        {
            get { return mDELETED; }
            set { mDELETED = value; }
        }
        #endregion

        #region CREBY
        private String mCREBY = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String CREBY
        {
            get { return mCREBY; }
            set { mCREBY = value; }
        }
        #endregion

        #region CRETIME
        private DateTime mCRETIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime CRETIME
        {
            get { return mCRETIME; }
            set { mCRETIME = value; }
        }
        #endregion

        #region LATMDYBY
        private String mLATMDYBY = string.Empty;
        /// <summary>
        ///
        /// </summary>
        public String LATMDYBY
        {
            get { return mLATMDYBY; }
            set { mLATMDYBY = value; }
        }
        #endregion

        #region LSTMDYTIME
        private DateTime mLSTMDYTIME = DateTime.MinValue;
        /// <summary>
        ///
        /// </summary>
        public DateTime LSTMDYTIME
        {
            get { return mLSTMDYTIME; }
            set { mLSTMDYTIME = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>GS_SchedulesAreaDP</returns>
        public GS_SchedulesAreaDP GetReCorded(long lngID)
        {
            GS_SchedulesAreaDP ee = new GS_SchedulesAreaDP();
            string strSQL = string.Empty;
            strSQL = "SELECT * FROM GS_SCHEDULESAREA WHERE  AREAID = " + lngID.ToString();
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.AREANAME = dr["AREANAME"].ToString();
                ee.STARTDATE = dr["STARTDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDATE"].ToString());
                ee.ENDDATE = dr["ENDDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDATE"].ToString());
                ee.STATUS = Int64.Parse(dr["STATUS"].ToString());
                ee.AREAFLAG = Int64.Parse(dr["AREAFLAG"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            throw new NotImplementedException("");
            DataTable dt = CommonDP.ExcuteSqlTablePage("GS_SCHEDULESAREA", "*", sWhere, sOrder, pagesize, pageindex, ref rowcount);
            return dt;
        }


        #endregion

        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_SchedulesAreaDP></param>
        public void InsertRecorded(GS_SchedulesAreaDP pER)
        {
            string strSQL = string.Empty;
            string strID = "0";
            try
            {

                strSQL = @"INSERT INTO GS_SCHEDULESAREA(
									AREAID,
									AREANAME,
									STARTDATE,
									ENDDATE,
									STATUS,
									AREAFLAG,
									DELETED,
									CREBY,
									CRETIME,
									LATMDYBY,
									LSTMDYTIME
					)
					VALUES( " +
                            pER.AREAID.ToString() + "," +
                            StringTool.SqlQ(pER.AREANAME) + "," +
                            (pER.STARTDATE == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pER.STARTDATE.ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd')") + "," +
                            (pER.ENDDATE == DateTime.MinValue ? " null " : "to_date(" + StringTool.SqlQ(pER.ENDDATE.ToString("yyyy-MM-dd")) + ",'yyyy-MM-dd')") + "," +
                            pER.STATUS.ToString() + "," +
                            pER.AREAFLAG.ToString() + "," +
                            pER.DELETED.ToString() + "," +
                            StringTool.SqlQ(pER.CREBY) + "," +
                            (pER.CRETIME.ToString() == DateTime.MinValue.ToString() ? " null " : "to_date(" + StringTool.SqlQ(pER.CRETIME.ToString().ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") + "," +
                            StringTool.SqlQ(pER.LATMDYBY) + "," +
                            (pER.LSTMDYTIME.ToString() == DateTime.MinValue.ToString() ? " null " : "to_date(" + StringTool.SqlQ(pER.LSTMDYTIME.ToString().ToString()) + ",'yyyy-MM-dd HH24:mi:ss')") +
                    ")";

                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pGS_SchedulesAreaDP></param>
        public void UpdateRecorded(GS_SchedulesAreaDP mod)
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE GS_SCHEDULESAREA Set " +
                                                        " AREANAME = " + StringTool.SqlQ(mod.AREANAME) + "," +
                            " STARTDATE = " + StringTool.SqlDate(mod.STARTDATE) + "," +
                            " ENDDATE = " + StringTool.SqlDate(mod.ENDDATE) + "," +
                            " STATUS = " + mod.STATUS.ToString() + "," +
                            " AREAFLAG = " + mod.AREAFLAG.ToString() + "," +
                            " DELETED = " + mod.DELETED.ToString() + "," +
                            " CREBY = " + StringTool.SqlQ(mod.CREBY) + "," +
                            " CRETIME = " + StringTool.SqlDate(mod.CRETIME) + "," +
                            " LATMDYBY = " + StringTool.SqlQ(mod.LATMDYBY) + "," +
                            " LSTMDYTIME = " + StringTool.SqlDate(mod.LSTMDYTIME) +
                                " WHERE AreaId  = " + mod.AREAID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
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
            try
            {
                string strSQL = "Update GS_SCHEDULESAREA Set Deleted=1  WHERE AreaId =" + lngID.ToString();
                CommonDP.ExcuteSql(strSQL);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public static DataTable GetLast5Row()
        {
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select areaid, areaname, startdate, enddate, status, areaflag, deleted, creby, cretime, latmdyby, lstmdytime, remark, ");
            sbStrSQL.Append("remark_newstaff from gs_schedulesarea where Deleted =0 and rownum<=20 order by areaid desc ");
            return CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
        }

        /// <summary>
        /// 获取排班周期开始时间
        /// </summary>
        /// <returns></returns>
        public static string GetMaxData()
        {
            DataTable dt = null;
            string startDate = string.Empty;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT max(A.Enddate+1) as startDate FROM GS_SchedulesArea A WHERE DELETED = 0");
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    startDate = dt.Rows[0]["startDate"].ToString();
                }
            }
            if (startDate.Equals(string.Empty))
            {
                DateTime datetime = DateTime.Now;
                //datetime = datetime.AddDays(1);
                startDate = datetime.ToString("yyyy-MM-dd");
            }
            return startDate;
        }

        /// <summary>
        /// 获取排班工作类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetWorkType()
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select * from Gs_Rule_Category");
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            return dt;
        }

        /// <summary>
        /// 获取工程师休息类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRestType()
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select restname, restvalue from gs_restcategory  order by seqnumber");
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            return dt;
        }

        /// <summary>
        /// 严重排班周期是否添加成功
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public static bool IsSchedulesArea(long areaID)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("SELECT * FROM GS_SchedulesArea where areaid=");
            sbStrSQL.Append(areaID);
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取排班区间未排班工程师信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNoSchedulingEngineer(long areaID)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select A.ID as EngineerID,A.Name as EngineerName,A.Regdeptname as DeptName from cst_servicestaff A where A.Id not in");
            sbStrSQL.Append("(SELECT engineerId FROM GS_CURSCHEDULESRULE WHERE deleted = 0 and AreaId = ");
            sbStrSQL.Append(areaID.ToString());
            sbStrSQL.Append(") and A.Deleted=0 ");
            sbStrSQL.AppendFormat("and a.joindate <=(select area.enddate from gs_schedulesarea area where area.areaid={0}) ", areaID);
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            return dt;
        }

        /// <summary>
        /// 获取排班区间未排班工程师总数
        /// </summary>
        /// <returns></returns>
        public static int GetNoSchedulingEngineerCount(long areaID)
        {
            int count = 0;
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select count(A.Id) as Total from cst_servicestaff A where A.Id not in");
            sbStrSQL.Append("(SELECT engineerId FROM GS_CURSCHEDULESRULE WHERE deleted = 0 and AreaId = ");
            sbStrSQL.Append(areaID.ToString());
            sbStrSQL.Append(") and A.Deleted=0 ");
            sbStrSQL.AppendFormat("and a.joindate <=(select area.enddate from gs_schedulesarea area where area.areaid={0}) ", areaID);
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0]["Total"].ToString());
                }
            }
            return count;
        }

        /// <summary>
        /// 获取轮班规则类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTurnRule()
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            sbStrSQL.Append("select trid, turnname, turnrate, remark, status, deleted, creby, cretime, latmdyby, lstmdytime from gs_turn_rule");
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            return dt;
        }

        /// <summary>
        /// 获取轮班规则详细班次
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTurnRuleDetl(long trID)
        {
            DataTable dt = null;
            StringBuilder sbStrSQL = new StringBuilder();
            //sbStrSQL.Append("SELECT  A.SCHEDULESID,A.Fullname  FROM gs_schedules_base A WHERE  A.Deleted=0 and A.Schedulesid in (SELECT B.Schedulesid FROM gs_turn_detl B WHERE B.Ruleid=");
            //sbStrSQL.Append(trID.ToString());
            //sbStrSQL.Append(")");
            sbStrSQL.AppendFormat(@"SELECT A.SCHEDULESID, A.Fullname
                                      FROM gs_schedules_base A, gs_turn_detl B
                                     WHERE A.Schedulesid = B.Schedulesid
                                       AND A.DELETED = 0
                                       AND B.DELETED = 0
                                       AND B.RULEID = {0}
                                     ORDER BY B.SEQNUM
                                    ", trID);
            dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
            return dt;
        }

        /// <summary>
        /// 获取轮班规则详细班次Dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<long, DataTable> GetTurnRuleDetl()
        {
            DataTable dt = null;
            Dictionary<long, DataTable> dicTurnRuleDetl = new Dictionary<long, DataTable>();
            dt = GetTurnRule();
            foreach (DataRow dr in dt.Rows)
            {
                long trID = 0;
                trID = long.Parse(dr["trid"].ToString());
                StringBuilder sbStrSQL = new StringBuilder();
                //sbStrSQL.Append("SELECT A.SCHEDULESID,A.Fullname FROM gs_schedules_base A WHERE  A.Deleted=0 and A.Schedulesid in (SELECT B.Schedulesid FROM gs_turn_detl B WHERE B.Ruleid=");
                //sbStrSQL.Append(trID.ToString());
                //sbStrSQL.Append(")");
                sbStrSQL.AppendFormat(@"SELECT A.SCHEDULESID, A.Fullname
                                      FROM gs_schedules_base A, gs_turn_detl B
                                     WHERE A.Schedulesid = B.Schedulesid
                                       AND A.DELETED = 0
                                       AND B.DELETED = 0
                                       AND B.RULEID = {0}
                                     ORDER BY B.SEQNUM
                                    ",trID);
                dt = CommonDP.ExcuteSqlTable(sbStrSQL.ToString());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dicTurnRuleDetl.Add(trID, dt);
                    }
                }
            }
            return dicTurnRuleDetl;
        }

        /// <summary>
        /// 得到下一个区间
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public GS_SchedulesAreaDP GetNextArea(long lngID)
        {
            GS_SchedulesAreaDP ee = null;
            string strSQL = string.Empty;
            strSQL = string.Format(@"SELECT * FROM GS_SCHEDULESAREA WHERE DELETED=0 
                                        AND STARTDATE > (SELECT STARTDATE FROM GS_SCHEDULESAREA WHERE  AREAID ={0}) AND ROWNUM =1
                                        ORDER BY STARTDATE ASC  ", lngID.ToString());
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee = new GS_SchedulesAreaDP();
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.AREANAME = dr["AREANAME"].ToString();
                ee.STARTDATE = dr["STARTDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDATE"].ToString());
                ee.ENDDATE = dr["ENDDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDATE"].ToString());
                ee.STATUS = Int64.Parse(dr["STATUS"].ToString());
                ee.AREAFLAG = Int64.Parse(dr["AREAFLAG"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
            }
            return ee;
        }

        /// <summary>
        /// 得到上一个排班周期
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public GS_SchedulesAreaDP GetPreArea(long lngID)
        {
            GS_SchedulesAreaDP ee = null;
            string strSQL = string.Empty;
            strSQL = string.Format(@"SELECT * FROM GS_SCHEDULESAREA WHERE DELETED=0 
                                        AND STARTDATE < (SELECT EndDATE FROM GS_SCHEDULESAREA WHERE  AREAID ={0}) AND ROWNUM =1
                                        ORDER BY STARTDATE ASC  ", lngID.ToString());
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            foreach (DataRow dr in dt.Rows)
            {
                ee = new GS_SchedulesAreaDP();
                ee.AREAID = Int64.Parse(dr["AREAID"].ToString());
                ee.AREANAME = dr["AREANAME"].ToString();
                ee.STARTDATE = dr["STARTDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["STARTDATE"].ToString());
                ee.ENDDATE = dr["ENDDATE"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["ENDDATE"].ToString());
                ee.STATUS = Int64.Parse(dr["STATUS"].ToString());
                ee.AREAFLAG = Int64.Parse(dr["AREAFLAG"].ToString());
                ee.DELETED = Int64.Parse(dr["DELETED"].ToString());
                ee.CREBY = dr["CREBY"].ToString();
                ee.CRETIME = dr["CRETIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["CRETIME"].ToString());
                ee.LATMDYBY = dr["LATMDYBY"].ToString();
                ee.LSTMDYTIME = dr["LSTMDYTIME"].ToString() == string.Empty ? DateTime.MinValue : DateTime.Parse(dr["LSTMDYTIME"].ToString());
            }
            return ee;
        }

        /// <summary>
        /// 判断上期排班区间是否可以用
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public bool JudegePreAreaIssues(long AreaId)
        {
            GS_SchedulesAreaDP preAreaDp = new GS_SchedulesAreaDP();
            preAreaDp = preAreaDp.GetNextArea(AreaId);
            if (preAreaDp == null)
            {
                return true;
            }
            //if (preAreaDp.STATUS >= 2)
            //{ 
            //}

            return (preAreaDp.STATUS > 1) ? false : true;

            //List<GS_PreSchedulesDP> preIssuesList = new GS_PreSchedulesDP().GetAllByAreaId(preAreaDp.AREAID);

            //if (preIssuesList == null || preIssuesList.Count == 0)
            //{
            //    return true;
            //}

            //GS_PreSchedulesDP preIssues = preIssuesList.Find(p => { return p.CRESTATUS == 0; });
            //if (preIssues != null)
            //{
            //    return false;
            //}
            //return true;
        }

        /// <summary>
        /// 补工程师排班信息
        /// </summary>
        /// <param name="AreaId"></param>
        public static void CreateEngineerWorkIssues(long AreaId, long EngineerId, long SchedulesId, long WorkCateId, long TurnRuleId)
        {
            OracleParameter[] parms = {
                      new OracleParameter("P_AREAID",OracleType.Number ),
                      new OracleParameter("P_EngineerId",OracleType.Number ),    
                      new OracleParameter("P_SchedulesId",OracleType.Number ),    
                      new OracleParameter("P_WorkCateId",OracleType.Number ),    
                      new OracleParameter("P_TurnRuleId",OracleType.Number )    
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[0].Value = AreaId;
            parms[1].Direction = ParameterDirection.Input;
            parms[1].Value = EngineerId;
            parms[2].Direction = ParameterDirection.Input;
            parms[2].Value = SchedulesId;
            parms[3].Direction = ParameterDirection.Input;
            parms[3].Value = WorkCateId;
            parms[4].Direction = ParameterDirection.Input;
            parms[4].Value = TurnRuleId;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, "PROC_Schedule_AddIssues", parms);
            }
            catch (Exception ex)
            {
                E8Logger.Error(ex);
                throw;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
    }
}

