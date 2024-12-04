/*******************************************************************
 *
 * Description:通用查询处理类
 * 
 * 
 * Create By  :
 * Create Date:2008年7月30日
 * *****************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL
{
    public class NormalAppDP
    {

        #region 新增查询
        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static DataTable GetNormalQuery(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string sWhere, string sOFlowModelID)
        {
            string strList = string.Empty;
            string strSQL = @"select a.*, b.subject,b.status,b.endtime,datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) as FlowDiffMinute
                                from App_pub_Normal_Head a,es_flow b, es_flowmodel c";
            strSQL += @" where a.FlowID=b.FlowID AND a.FlowModelID=c.FlowModelID and c.AppID=199 and c.OFlowModelID=" + sOFlowModelID;
            if (re != null)
            {
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
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }


                    #endregion

                    strSQL = strSQL + " ORDER BY a.ID DESC";
                }
            }
            else
            {
                //查询出空结果
                strSQL += " AND a.flowid = -1 ";
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

        #region 自定义通用查询语句
        /// <summary>
        /// 通用查询
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static DataTable GetDefineQuery(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string sWhere, string sOFlowModelID, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strSQL = @" OFlowModelID=" + sOFlowModelID;
            if (re != null)
            {
                if (re.CanRead == false)
                {
                    //查询出空结果
                    strSQL += " AND flowid = -1 ";
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
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_App_DefineData.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_App_DefineData.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_App_DefineData.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_App_DefineData.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_App_DefineData.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }


                    #endregion
                }
            }
            else
            {
                //查询出空结果
                strSQL += " AND flowid = -1 ";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_App_DefineData", "*", " ORDER BY ID DESC", pagesize, pageindex, strSQL, ref rowcount);
                
                return dt;
            }
            finally { ConfigTool.CloseConnection(cn); }
        }
        #endregion
    }
}
