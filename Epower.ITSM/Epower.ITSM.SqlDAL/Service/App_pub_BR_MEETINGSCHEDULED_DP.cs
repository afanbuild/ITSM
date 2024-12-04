using System;
using System.Collections.Generic;
using System.Text;
using Epower.DevBase.Organization.SqlDAL;
using System.Data;
using EpowerCom;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
    public class App_pub_BR_MEETINGSCHEDULED_DP
    {
        public App_pub_BR_MEETINGSCHEDULED_DP() { }

        #region
        /// <summary>
        /// 根据ＸＭＬ串条件获取数据
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForCond(string strXmlcond, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re)
        {
            // StringBuilder strS = new StringBuilder();
            string strSQL = string.Empty;        //存放SQL字符串

            string strWhere = string.Empty;  //存放拼条件


            string orderby = " order by a.FlowID desc ";

            string strList = string.Empty;

            if (re != null && re.CanRead == true && !string.IsNullOrEmpty(strXmlcond))
            {
                #region 获取查询参数的值

                FieldValues fv = new FieldValues(strXmlcond);
                ////会议名称
                //if (fv.GetFieldValue("MeetingName").Value != "" && long.Parse(fv.GetFieldValue("MeetingName").Value) > 0)
                //{
                //    strWhere += "  AND a.MeetingName = " + fv.GetFieldValue("MeetingName").Value;
                //}
                ////会议议题
                //if (fv.GetFieldValue("Title").Value.Trim() != string.Empty && long.Parse(fv.GetFieldValue("Title").Value) > 0)
                //{
                //    strWhere += " AND a.Title = " + fv.GetFieldValue("Title").Value.Trim();
                //}
                //开始时间和结束时间

                //预定日期
                if (fv.GetFieldValue("begin_time").Value != string.Empty)
                {
                    strWhere += " and a.StartTime>=to_date(" + StringTool.SqlQ(fv.GetFieldValue("begin_time").Value.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
                }
                if (fv.GetFieldValue("end_time").Value != string.Empty)
                {
                    strWhere += " and a.EndTime<=to_date(" + StringTool.SqlQ(fv.GetFieldValue("end_time").Value.Trim()) + ",'yyyy-MM-dd HH24:mi:ss')";
                }

                #endregion

                #region 范围条件
                switch (re.RightRange)
                {
                    case eO_RightRange.eFull:
                        strWhere += "";
                        break;
                    //case eO_RightRange.ePersonal:
                    //    strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND receiverid = " + lngUserID.ToString() + ")";
                    //    break;
                    case eO_RightRange.eDeptDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                        break;
                    case eO_RightRange.eOrgDirect:
                        strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                        break;
                    case eO_RightRange.eDept:
                        strList = DeptDP.GetDeptFullID(lngDeptID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    case eO_RightRange.eOrg:
                        strList = DeptDP.GetDeptFullID(lngOrgID);
                        if (strList.Trim().Length > 0)
                        {
                            //不是根部门并有找到

                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = a.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                        }
                        break;
                    default:
                        strWhere += "";
                        break;
                }
                #endregion
            }

            strSQL = @"SELECT  a.*,to_char(a.datetime,'yyyy-MM-dd') datetime2 FROM BR_MeetingScheduled a WHERE 1=1 ";

            if (re == null || re.CanRead == false)
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL = strSQL + strWhere;
            }

            strSQL = strSQL + orderby;

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// 查询所有会议室
        /// </summary>
        /// <returns></returns>
        public static DataTable Get_Es_Catalog()
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string sql = "select * from Es_Catalog ec where ec.parentid=10972 and ec.deleted=0";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sql);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 根据开始时间和结束时间 会议室，查询会议室在此时间段是否被占用
        /// </summary>
        /// <param name="sartime"></param>
        /// <param name="endtime"></param>
        /// <param name="meeting_id"></param>
        /// <returns></returns>
        public static DataTable Get_Meeting_Scheduled(string startime, string endtime, long meeting_id)
        {
            string strSQL = "";

            OracleConnection cn = ConfigTool.GetConnection();

            strSQL = "select count(*) num from (select * from  br_meetingscheduled bm where " +
                     " bm.starttime <= to_date('" + startime + "','yyyy-MM-dd HH24:mi:ss') and" +
                     " bm.endtime >=to_date('" + startime + "','yyyy-MM-dd HH24:mi:ss') or" +
                     " bm.starttime <= to_date('" + endtime + "','yyyy-MM-dd HH24:mi:ss') and" +
                     " bm.endtime >=to_date('" + endtime + "','yyyy-MM-dd HH24:mi:ss') or" +
                     " bm.starttime >to_date('" + startime + "','yyyy-MM-dd HH24:mi:ss') " +
                     " and bm.endtime <=to_date('" + endtime + "','yyyy-MM-dd HH24:mi:ss')" +
                     " ) b where b.meetingid =" + meeting_id + "";

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
    }
}
