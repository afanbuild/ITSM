
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
using System.Data.OracleClient;
using System.Collections;
using System.Collections.Generic;

namespace Epower.ITSM.SqlDAL.Schedules
{
    /// <summary>
    /// 调整班次
    /// </summary>
    public class EngineerIssuesDP
    {
        private GS_CurSchedulesRuleDP CurAreaIssues
        { set; get; }

        private GS_PreSchedulesDP PreAreaIssues
        { set; get; }

        /// <summary>
        /// 工程师实体，对外操作的对象
        /// </summary>
        public Cst_ServiceStaffDP Staff
        { set; get; }

        private bool isChanged = false;


        public List<GS_TURN_RULEDP> TurnList
        { set; get; }

        /// <summary>
        /// 数据初始化加载，如果AreaId=-1，则表示是新工程师入职，以前从未排过班次。
        /// </summary>
        /// <param name="engineerId"></param>
        /// <param name="AreaId"></param>
        public void Load(long engineerId, long AreaId)
        {
            Staff = new Cst_ServiceStaffDP().GetReCorded(engineerId);
            if (AreaId == -1)
            {
                return;
            }
            CurAreaIssues = new GS_CurSchedulesRuleDP().GetReCorded(AreaId, engineerId);
            GS_SchedulesAreaDP PreArea = new GS_SchedulesAreaDP().GetPreArea(AreaId);
            if (PreArea != null)
            {
                PreAreaIssues = new GS_PreSchedulesDP().GetReCorded(PreArea.AREAID, engineerId);
            }
            
        }

        /// <summary>
        /// 更改
        /// </summary>
        public List<String> AcceptIssuesChange()
        {
            Staff.FIRSTFLAG = getForFirstFlag();

            string strCurAreaIssues = string.Empty;//工程师排班规则--SQL语句
            string strStaff = string.Empty;//工程师排班信息--SQL语句
            List<String> SQLStringList = new List<string>();

            isChanged = judegeChanged();
            Epower.DevBase.BaseTools.E8Logger.Info("isChanged=" + isChanged.ToString());
            if (!isChanged)
            {
                return null;
            }

            if (CurAreaIssues != null)
            {                
                setCurAreaIssues();
                strCurAreaIssues = CurAreaIssues.UpdateRecorded(CurAreaIssues);
            }          

            strStaff = Staff.UpdateRecordedSchedules(Staff);

            if (!strStaff.Equals(string.Empty) && !strCurAreaIssues.Equals(string.Empty))
            {
                SQLStringList.Add(strCurAreaIssues);
                SQLStringList.Add(strStaff);
            }
            return SQLStringList;
        }

        private bool judegeChanged()
        {
            return !(Staff.WORKCATEID == CurAreaIssues.WORKCATEID
                    && Staff.TRID == CurAreaIssues.TURNRULEID
                    && Staff.SCHEDULESID == CurAreaIssues.SCHEDULESID);
        }

        /// <summary>
        /// 设置FirstFLag
        /// </summary>
        /// <returns></returns>
        private int getForFirstFlag()
        {
            if (PreAreaIssues == null)
            {
                return 1;
            }
            if (PreAreaIssues.SCHEDULESID != Staff.SCHEDULESID)
            {       
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 给当期预排班信息赋值。
        /// </summary>
        private void setCurAreaIssues()
        {

            if (CurAreaIssues.EXPECTSCHEDULESID == 0)
            {
                CurAreaIssues.EXPECTSCHEDULESID = CurAreaIssues.SCHEDULESID;
            }
            if (Staff.TRID > 0)
            {
                //if (Staff.TRID != CurAreaIssues.TURNRULEID || Staff.SCHEDULESID != CurAreaIssues.SCHEDULESID)
                {
                    GS_TURN_RULEDP turn = TurnList.Find(p => { return p.TRID == Staff.TRID; });
                    CurAreaIssues.REMAINDERNUM = turn.TURNRATE;
                }
            }
            CurAreaIssues.WORKCATEID = long.Parse(Staff.WORKCATEID.ToString());//排班类型
            CurAreaIssues.SCHEDULESID = long.Parse(Staff.SCHEDULESID.ToString());//班次.
            CurAreaIssues.TURNRULEID = long.Parse(Staff.TRID.ToString());//轮班ID.


            CurAreaIssues.CRESTATUS = 1;
            CurAreaIssues.REMARK = getChangeRemark();
            //CurAreaIssues.LATMDYBY=Staff.r
            CurAreaIssues.LSTMDYTIME = DateTime.Now;
        }

        private string getChangeRemark()
        {
            if (CurAreaIssues == null)
            {
                return "";
            }
            if (CurAreaIssues.EXPECTSCHEDULESID == 0)
            {
                return "";
            }
            List<GS_Schedules_BaseDP> issuesBaseList = new GS_Schedules_BaseDP().GetAllList();
            string remark = issuesBaseList.Find(p => { return p.SCHEDULESID == CurAreaIssues.EXPECTSCHEDULESID; }).FULLNAME
                            + "=>"
                            + issuesBaseList.Find(p => { return p.SCHEDULESID == CurAreaIssues.SCHEDULESID; }).FULLNAME;
            return remark;
        
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static bool ExecuteSqlTran(List<String> SQLStringList)
        {
            if (SQLStringList == null || SQLStringList.Count == 0)
            {
                return true;
            }
            bool isSuccess = true;
            using (OracleConnection conn = ConfigTool.GetConnection())
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                OracleTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (string sql in SQLStringList)
                    {
                        E8Logger.Info(sql);
                        if (!String.IsNullOrEmpty(sql))
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                            Epower.DevBase.BaseTools.E8Logger.Info(sql);
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    isSuccess = false;
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return isSuccess;
        } 
    }
}
