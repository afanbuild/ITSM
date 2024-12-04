/****************************************************************************
 * 
 * description:����������ݲ�ѯ��
 * 
 * 
 * 
 * Create by:zhumingchun
 * Create Date:2008-04-04
 * *************************************************************************/
using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Xml;
using System.IO;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// ChangeDealDP
    /// </summary>
    public class ChangeDealDP
    {
        /// <summary>
        /// ����������ȡ������ݱ�����
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealData(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //��Ų����б�
            string strSQL = @"select a.*,case when nvl(IS_PLAN_CHANGE,0)=1 then '��' else '��' end as isplanchange,
case when nvl(IS_BUS_EFFECT,0)=1 then '��' else '��' end as isbuseffect,
case when nvl(IS_DATA_EFFECT,0)=1 then '��' else '��' end as isdataeffect,
case when nvl(IS_STOP_SERVER,0)=1 then '��' else '��' end as isstopserver,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
                            from Equ_ChangeService a,es_flow b 
                            WHERE a.FlowID = b.FlowID  ";

            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += pWhere;
                #region ��Χ����
                if (re != null)
                {
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
                                //���Ǹ����Ų����ҵ�
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.ID DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        /// <summary>
        /// ����������ȡ������ݱ�����[���ݿͻ���Ϣ��ѯ�����]
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealDataForCustInfo(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //��Ų����б�
            string strSQL = @"select a.*,case when nvl(IS_PLAN_CHANGE,0)=1 then '��' else '��' end as isplanchange,
case when nvl(IS_BUS_EFFECT,0)=1 then '��' else '��' end as isbuseffect,
case when nvl(IS_DATA_EFFECT,0)=1 then '��' else '��' end as isdataeffect,
case when nvl(IS_STOP_SERVER,0)=1 then '��' else '��' end as isstopserver,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute,
                            d.MastCustName
                            from es_flow b,Equ_ChangeService a  left join br_ecustomer d on a.custid = d.id 
                            WHERE a.FlowID = b.FlowID  ";

            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strSQL += " AND a.flowid = -1 ";
            }
            else
            {
                strSQL += pWhere;
                #region ��Χ����
                if (re != null)
                {
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
                                //���Ǹ����Ų����ҵ�
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strSQL += "AND exists (SELECT messageid FROM es_message WHERE flowid = b.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                }
                #endregion
                strSQL = strSQL + " ORDER BY a.ID DESC";
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        public static DataTable getEquipment(string sWhere)
        {
            string SQLstr = "";
            if (sWhere == "")
            {
                SQLstr = "SELECT distinct ChangeID FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id";
            }
            else
            {
                SQLstr = "SELECT distinct ChangeID FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id  where " + sWhere;
            }
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static DataTable getCST_ISSUE_LISTFASTQUERY(string FormId, string LoginName, string Name)
        {
            Name = Name == "==ѡ���ղز�ѯ����==" ? "Temp1" : Name;
            string SQLstr = "";
            if (Name.Trim() == string.Empty)
            {
                SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')!='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + " order by SN desc";
            }
            else
            {
                if (Name == "Temp1")
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  SQLWhere='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND Name=" + StringTool.SqlQ(Name);
                }
                else
                {
                    SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                            FROM CST_ISSUE_QUERYSave
                            WHERE  nvl(SQLWhere,' ')!='Temp1' AND  FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND Name=" + StringTool.SqlQ(Name);
                }
            }

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static DataTable getCST_ISSUE_Where(string FormId, string LoginName, string Name)
        {
            string SQLstr = "";
            SQLstr = @"SELECT  ID,Name,FORMID,LOGINNAME,SQLWhere,SQLText ,DISPLAYCOLUMN 
                        FROM CST_ISSUE_QUERYSave
                        WHERE FORMID=" + StringTool.SqlQ(FormId) + " AND LOGINNAME=" + StringTool.SqlQ(LoginName) + "  AND (SQLWhere='Temp1' or Name in (" + StringTool.SqlQ(Name) + "))";

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        public static void updateCST_ISSUE_WhereNums(string sloginame, string sname)
        {
            string SQLstr = "update CST_ISSUE_QUERYSave set SN=nvl(SN,0)+1 where FORMID='frm_ChangeQuery' AND  nvl(SQLWhere,' ')!='Temp1' AND  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public static void deleteCST_ISSUE_Where(string sloginame, string sname)
        {
            string SQLstr = "delete from  CST_ISSUE_QUERYSave  where FORMID='frm_ChangeQuery'  and  nvl(SQLWhere,' ')!='Temp1' and  LOGINNAME=" + StringTool.SqlQ(sloginame) + " and Name=" + StringTool.SqlQ(sname);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            int i = OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }

        public static DataTable GetChangeDealDataForCustInfo(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere, int pagesize, int pageindex, ref int rowcount)
        {
            string strList = "";       //��Ų����б�
            string strWhere = "1=1";

            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region ��Χ����
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                }
                #endregion
                strWhere += pWhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_Equ_ChangeService", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        public static DataTable GetChangeDealDataForCustInfo1(long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, string pWhere)
        {
            string strList = "";       //��Ų����б�
            string strWhere = "1=1";
            string strSQL = "";
            if (re == null || re.CanRead == false)
            {
                //��ѯ���ս��
                strWhere += " AND flowid = -1 ";
            }
            else
            {
                #region ��Χ����
                if (re != null)
                {
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recdeptid in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //���Ǹ����Ų����ҵ�
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_Equ_ChangeService.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + "))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                }
                #endregion
                strWhere += pWhere;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                strSQL += "SELECT * FROM V_Equ_ChangeService WHERE " + strWhere;
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static string GetPersonList(long lngFlowID)
        {
            string sReturn = string.Empty;
            string strSQL = @"select Distinct TActors from es_message where FlowID=" + lngFlowID.ToString() + " and TActors is not null";

            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                foreach (DataRow dr in dt.Rows)
                {
                    sReturn += dr["TActors"].ToString() + ",";
                }
                return sReturn;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static bool CheckIsChangeProblem(long lngServiceFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT  IssuesFlowID " +
                " FROM Equ_ChangeService " +
                " WHERE nvl(IssuesFlowID,0)=" + lngServiceFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region �ж������Ƿ��ѹ������
        /// <summary>
        /// �ж������Ƿ��ѹ������
        /// </summary>
        /// <param name="lngProblemFlowID"></param>
        /// <returns></returns>
        public static bool CheckIsChangeFromPro(long lngProblemFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT ProblemFlowID " +
                " FROM Equ_ChangeService " +
                " WHERE nvl(ProblemFlowID,0)=" + lngProblemFlowID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ��ȡ���¼���صı��
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetIssuesForChange(long lngFlowID)
        {
            string strSQL = "";
            strSQL = " SELECT nvl(a.BuildCode, '') || nvl(a.ServiceNo, '') AS ServiceNo,a.ServiceType,a.Subject,a.DealStatus,b.status,c.appname,a.flowid " +
                     " FROM Cst_Issues a, es_flow b,es_app c " +
                     " WHERE a.FlowID = b.flowid AND b.appID = c.appID  " +
                     " AND a.AssociateFlowID = " + lngFlowID.ToString() + " ORDER BY a.flowid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        #region ��ȡ������ص�����
        /// <summary>
        /// ��ȡ������ص�����
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemsByChange(long lngFlowID)
        {
            string strSQL = "";
            strSQL = "SELECT nvl(a.BuildCode, '') || nvl(a.ProblemNo, '') AS ServiceNo,a.Problem_TypeName,a.Problem_Title,a.StateName,a.flowid,b.status,c.appname " +
                     " FROM Pro_ProblemDeal a, es_flow b,es_app c " +
                     " WHERE a.FlowID = b.flowid AND b.appID = c.appID  " +
                     " AND a.AssociateFlowID = " + lngFlowID.ToString() + " ORDER BY a.flowid DESC";

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        /// <summary>
        /// �¼���������
        /// </summary>
        /// <param name="lngFlowID">�¼�����ID</param>
        /// <returns></returns>
        public static DataTable GetChangeData(long lngFlowID)
        {
            string strSQL = @"select a.*,
                            b.status,
					        case when b.status=30 then 
					        datediff('Minute',nvl(b.endtime,sysdate),nvl(b.expectendtime,sysdate))
					        else 
					        datediff('Minute',sysdate,nvl(b.expectendtime,sysdate)) end FlowDiffMinute
                            from Equ_ChangeService a,es_flow b
                            WHERE a.FlowID = b.FlowID   ";
            strSQL += " And a.FlowID=" + lngFlowID.ToString();

            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// ����������ȡ������ݱ�����
        /// </summary>
        /// <param name="pWhere"></param>
        /// <returns></returns>
        public static DataTable GetChangeDealDataJQuery(long lngID)
        {
            string strSQL = @"select a.*,
                            b.status
                            from Equ_ChangeService a,es_flow b 
                            WHERE a.FlowID = b.FlowID AND a.ID=" + lngID.ToString();
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        #region �����ʲ���ȡ�������

        /// <summary>
        /// �����ʲ���ȡ�������
        /// </summary>
        /// <param name="changId">���ID</param>
        /// <param name="equId">�ʲ�ID</param>
        /// <returns>�������</returns>
        public string GetChangeNo(long changId, long equId)
        {
            string changNo = string.Empty, strSQL = string.Empty;//�������
            strSQL = "SELECT B.CHANGENO FROM EQU_CHANGESERVICE B WHERE B.ID IN (SELECT A.CHANGEID FROM EQU_CHANGESERVICEDETAILS A WHERE A.CHANGEID<>" + changId.ToString() + " AND A.CHANGESTATUS=0 AND A.EQUID=" + equId.ToString() + ") ";
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);
                if (dr.Read())
                {
                    changNo = dr["CHANGENO"].ToString();//������Ÿ�ֵ
                }
                dr.Close();
                return changNo;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #endregion
      

        #region �������ⵥFlowID��ȡ���ⵥ����
        /// <summary>
        /// �������ⵥFlowID��ȡ���ⵥ����
        /// </summary>
        /// <param name="lngFlowID"></param>
        /// <returns></returns>
        public static DataTable GetProblemsByFlowID(long lngFlowID)
        {
            string strSql = @"select A.*,B.partBranchName as DEPT,B.Code as EQUCODE,b.catalogid,b.catalogname,
                                     '' as CHANGECONTENT,'' as OLDVALUE,'' as NEWVALUE,'' as Remark
                                from 
	                                (
	                                select ListID,ListName,EQUID,EQUNAME from Pro_ProblemDeal where FlowID = " + lngFlowID + @"
	                                ) A
                           left join  Equ_Desk B
                                  on A.EQUID = B.ID";

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region �����ϸ���

        /// <summary>
        /// �����ϸ���
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCLFareItem(long id)
        {
            string sSql = @"SELECT a.*,b.PARTBRANCHNAME as DEPT FROM EQU_CHANGESERVICEDETAILS a LEFT JOIN EQU_DESK b ON a.EQUID=b.id WHERE ChangeID = " + id.ToString();

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        public static DataTable GetCLFareItem(OracleTransaction trans, long id)
        {
            string sSql = @"SELECT * FROM EQU_CHANGESERVICEDETAILS WHERE ChangeID = " + id.ToString();

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            try
            {
                DataTable dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, sSql).Tables[0];
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }


        /// <summary>
        /// �õ������ϸ��
        /// </summary>
        /// <param name="flowid"></param>
        /// <returns></returns>
        public static DataTable GetStatusID(OracleTransaction trans, long flowid)
        {
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            string sSql = "SELECT * FROM EQU_CHANGESERVICEDETAILS Where ChangeID=" + flowid.ToString();
            DataTable dt = null;
            try
            {
                dt = OracleDbHelper.ExecuteDataset(trans, CommandType.Text, sSql).Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// �����ʲ�XML��
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static void SaveCLFareDetailItem(DataTable dt, string deskChange)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();//����XML����
                XmlElement xmlEle = xmlDoc.CreateElement("XmlDesk");//�������ڵ�
                foreach (DataRow row in dt.Rows)
                {
                    #region �����ӽڵ�

                    XmlElement xmlEleSub = xmlDoc.CreateElement("DeskInfo");

                    #endregion

                    #region ��ֵ����ֵ

                    xmlEleSub.SetAttribute("DETAILSID", "0");
                    xmlEleSub.SetAttribute("EQUID", row["EQUID"].ToString());//�ʲ�ID
                    xmlEleSub.SetAttribute("EQUNAME", row["EQUNAME"].ToString());//�ʲ�����
                    xmlEleSub.SetAttribute("LISTID", row["LISTID"].ToString());//�ʲ�Ŀ¼ID
                    xmlEleSub.SetAttribute("LISTNAME", row["LISTNAME"].ToString());//�ʲ�Ŀ¼����
                    xmlEleSub.SetAttribute("EQUCODE", row["EQUCODE"].ToString());//�ʲ����
                    xmlEleSub.SetAttribute("CHANGECONTENT", row["CHANGECONTENT"].ToString());//�������
                    xmlEleSub.SetAttribute("OLDVALUE", row["OLDVALUE"].ToString());//���ǰֵ
                    xmlEleSub.SetAttribute("NEWVALUE", row["NEWVALUE"].ToString());//�����ֵ
                    xmlEleSub.SetAttribute("CHANGEDATE", row["CHANGEDATE"].ToString());//�������
                    xmlEleSub.SetAttribute("CHANGEUSERID", row["CHANGEUSERID"].ToString());//����û�ID
                    xmlEleSub.SetAttribute("CHANGEUSERNAME", row["CHANGEUSERNAME"].ToString());//����û���
                    xmlEleSub.SetAttribute("CHANGEDEPTID", row["CHANGEDEPTID"].ToString());//����û�����ID
                    xmlEleSub.SetAttribute("CHANGEDEPTNAME", row["CHANGEDEPTNAME"].ToString());//����û���������
                    xmlEleSub.SetAttribute("CHANGESTATUS", row["CHANGESTATUS"].ToString());//���״̬
                    xmlEleSub.SetAttribute("REMARK", row["Remark"].ToString());//�����ע
                    xmlEleSub.SetAttribute("DEPT", row["CHANGEDEPTNAME"].ToString());//�������

                    #endregion

                    #region ����ӽڵ�

                    xmlEle.AppendChild(xmlEleSub);//����ӽڵ�

                    #endregion
                }
                xmlDoc.AppendChild(xmlEle);
                xmlDoc.Save(deskChange);
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }



        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="strNo"></param>
        /// <param name="tran"></param>
        public static void DeleteCLFareDetailItem(long lngID, OracleTransaction tran)
        {

            string sSql = "Delete EQU_CHANGESERVICEDETAILS Where CHANGEID=" + lngID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="costID"></param>
        /// <returns></returns>
        public static bool DeleteCLFareDetailItem(long costID)
        {
            if (costID == 0)
                return true;
            bool result = true;
            string sSql = "Delete EQU_CHANGESERVICEDETAILS Where ID=" + costID.ToString();
            try
            {
                OracleDbHelper.ExecuteNonQuery(ConfigTool.GetConnection(), CommandType.Text, sSql);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        #endregion

        #region ����FlowID��ȡ�����ʱ��Ϣ

        /// <summary>
        /// GetTemporaryChangeByFlowId
        /// </summary>
        /// <param name="lngChangeId">���ID</param>
        /// <param name="lngEquId">�ʲ�ID</param>
        /// <returns></returns>
        public DataTable GetTemporaryChangeByChangeId(long lngChangeId, long lngEquId)
        {
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();

            #region �������

            sb.Append("select ChangeNO from Equ_Changeservice where id in (SELECT a.change_id FROM EQU_DESK a ");
            sb.Append("WHERE a.id=" + lngEquId + " AND a.change_Id<>" + lngChangeId +")");//��ѯ���

            #endregion

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//��ȡ����

            try
            {
                dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sb.ToString());//��ȡ����
            }
            catch
            {
                //�����־
                throw;
            }
            finally
            {
                //�ر�����
                ConfigTool.CloseConnection(cn);
            }
            return dt;
        }

        #endregion

        #region ��ȡ�����ط�����ͼ��
        /// <summary>
        /// ��ȡ�����ط�����ͼ��
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <returns></returns>
        public static DataTable GetSM(string strMastCust)
        {
            string strWhere = string.Empty;

            if (strMastCust == "")
            {
                strWhere = "";
            }
            else
            {
                strWhere = " and MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            string strSql = @"--�账������
                                select '�账������' as Title,count(1) as counts,'ChangeW' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0
	                                    and (
                                         (B.status = 30 and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM'))
                                         or B.status = 20)" + strWhere + @"
                                union all
                                --����������������ĵǼ�ʱ��Ϊ���£���ȥ�ͻ�Ϊ�պ��ݴ�δ�ύ�ġ�
                                select '��������' as Title,count(1) as counts,'ChangeB' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                 where A.FlowID = B.FlowID and  A.CustID = C.ID and C.deleted = 0
	                               and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')
	                               and A.flowid NOT IN (select flowid from es_message where status=20 and senderid=0)" + strWhere + @"
                                union all
                                --����δ���
                                select '����δ���' as Title,count(1) as counts,'ChangeA' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 20" + strWhere + @"
                                union all
                                --������ɡ����̽����ģ������̽���ʱ��Ϊ���µġ�
                                select '�������' as Title,count(1) as counts,'ChangeC' as Types
                                  from Equ_ChangeService A,es_Flow B,Br_ECustomer C
                                where A.FlowID = B.FlowID and A.CustID = C.ID and C.deleted = 0 and B.status = 30  and to_char(EndTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere + @"
                                union all
                                --���½��������������ID�̶�Ϊ100���ҵǼ�ʱ��Ϊ���¡�
                                select '���½������' as Title,count(1) as counts,'ChangeD' as Types
                                  from Equ_ChangeService A,Br_ECustomer C
                                where A.CustID = C.ID and C.deleted = 0 and InstancyID = 100 and to_char(A.RegTime,'yyyy-MM') = to_char(sysdate,'yyyy-MM')" + strWhere;

            DataTable dt = CommonDP.ExcuteSqlTable(strSql);

            return dt;
        }
        #endregion

        #region ����������ӵ㿪�鿴����
        /// <summary>
        /// ����������ӵ㿪�鿴����
        /// </summary>
        /// <param name="strMastCust"></param>
        /// <param name="strType"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetSMDetail(string strMastCust, string strType, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = new DataTable();
            string strWhere = " 1=1 ";
            string strSql = string.Empty;

            if (strMastCust != "")
            {
                strWhere += " and MastCustID = " + StringTool.SqlQ(strMastCust);
            }

            if (strType == "ChangeA")
            {
                //����δ���
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeA", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeB")
            {
                //��������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeB", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeC")
            {
                //�������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeC", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeD")
            {
                //���½������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeD", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else if (strType == "ChangeW")
            {
                //�账������
                dt = OracleDbHelper.ExecuteDataTable(cn, "v_ChangeW", "*", " ORDER BY ID DESC", pagesize, pageindex, strWhere, ref rowcount);
            }
            else
            {
                strSql = @"--�޼�¼
                                select A.*
                                  from Equ_ChangeService A where 1 = 2";
            }

            return dt;
        }
        #endregion

        #region �������ռ��
        /// <summary>
        /// �������ռ��
        /// </summary>
        /// <param name="type">0��ʾ���� ���߱�ʾ����</param>
        /// <param name="pColum">����������</param>
        /// <param name="p_strWhere">where����</param>
        /// <param name="p_strWhere2">����������ʱ���where����</param>
        /// <returns></returns>
        public static DataTable GetEqu_ChangeService(int type, string pColum, string p_strWhere, string p_strWhere2)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_Colum",OracleType.VarChar,50),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("p_strWhere2",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };
            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Input;
            parms[4].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = pColum;
            parms[2].Value = p_strWhere;
            parms[3].Value = p_strWhere2;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_Equ_ChangeService", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region �����������ͳ��
        /// <summary>
        /// �����������ͳ��
        /// </summary>
        /// <param name="GroupByType">�Ƿ񰴱�����ͳ��0��ʾ�� ���߱�ʾ��</param>
        /// <param name="type">0��ʾ���� ���߱�ʾ����</param>
        /// <param name="p_strWhere">where����</param>
        /// <returns></returns>
        public static DataTable GetCHANGEACCPTED(int GroupByType, int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_GroupByType",OracleType.Number,4),
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Input;
            parms[3].Direction = ParameterDirection.Output;

            parms[0].Value = GroupByType;
            parms[1].Value = type;
            parms[2].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_CHANGEACCPTED", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region ������ռ��
        /// <summary>
        /// ������ռ��
        /// </summary>
        /// <param name="p_strWhere">where����</param>
        /// <returns></returns>
        public static DataTable GetChangeTypePercentage(string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_strWhere",OracleType.NVarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Output;

            parms[0].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "PROC_ChangeTypePercentage", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        /// <summary>
        /// �ж��������Ƿ��Ѿ�����
        /// </summary>
        /// <returns></returns>
        public static bool WeekIsExist()
        {
            bool result = false;

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_WeekSetting ";           
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;

        }
        /// <summary>
        /// �ж��������Ƿ��Ѿ�����
        /// </summary>
        /// <returns></returns>
        public static bool MonthIsExist()
        {
            bool result = false;

            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_MonthSetting ";
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            if (dt != null && dt.Rows.Count > 0)
                result = true;

            return result;
        }


        #region �������ƽ��ʱ��KPI
        /// <summary>
        /// �������ƽ��ʱ��KPI
        /// </summary>
        /// <param name="type">0��ʾ���� 1��ʾ����</param>
        /// <param name="p_strWhere">where����</param>
        /// <returns></returns>
        public static DataTable GetChangeTimeAvgKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_ChangeTimeAvgKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

        #region �������׶ηֲ�KPI
        /// <summary>
        /// �������׶ηֲ�KPI
        /// </summary>
        /// <param name="type">0��ʾ���� 1��ʾ����</param>
        /// <param name="p_strWhere">where����</param>
        /// <returns></returns>
        public static DataTable GetChangeStatusKPI(int type, string p_strWhere)
        {
            DataTable dt = null;

            OracleParameter[] parms = {
                      new OracleParameter("p_type",OracleType.Number,4),
                      new OracleParameter("p_strWhere",OracleType.VarChar,2000),
                      new OracleParameter("P_GetData",OracleType.Cursor)
                };

            parms[0].Direction = ParameterDirection.Input;
            parms[1].Direction = ParameterDirection.Input;
            parms[2].Direction = ParameterDirection.Output;

            parms[0].Value = type;
            parms[1].Value = p_strWhere;

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.StoredProcedure, "proc_ChangeStatusKPI", parms);
            ConfigTool.CloseConnection(cn);

            return dt;
        }
        #endregion

















        










    }
}
